#!/usr/bin/env python3
"""Regression tests for the fail-closed cross-platform target verifier."""

from pathlib import Path
import shutil
import subprocess
import sys
import tempfile

ROOT = Path(__file__).resolve().parents[2]
VERIFIER = ROOT / "build/scripts/verify-cross-platform-targets.py"

FIXTURE_FILES = (
    "src/CareNest.App/CareNest.App.csproj",
    "src/CareNest.CrossPlatform/CareNest.CrossPlatform.csproj",
    "src/CareNest.CrossPlatform/App.axaml",
    "src/CareNest.CrossPlatform/App.axaml.cs",
    "src/CareNest.CrossPlatform/Views/MainView.axaml",
    "src/CareNest.CrossPlatform.Desktop/CareNest.CrossPlatform.Desktop.csproj",
    "src/CareNest.CrossPlatform.Desktop/Program.cs",
    "src/CareNest.CrossPlatform.Browser/CareNest.CrossPlatform.Browser.csproj",
    "src/CareNest.CrossPlatform.Browser/Program.cs",
    "Directory.Packages.props",
    "CareNest.sln",
    ".github/workflows/ci.yml",
    ".github/workflows/dependency-review.yml",
    ".github/workflows/release-gate.yml",
)


def copy_fixture(destination: Path) -> None:
    for relative in FIXTURE_FILES:
        source = ROOT / relative
        target = destination / relative
        target.parent.mkdir(parents=True, exist_ok=True)
        shutil.copy2(source, target)


def run_verifier(root: Path) -> subprocess.CompletedProcess[str]:
    return subprocess.run(
        [sys.executable, str(VERIFIER), "--root", str(root)],
        check=False,
        capture_output=True,
        text=True,
    )


def require(condition: bool, message: str) -> None:
    if not condition:
        raise AssertionError(message)


def main() -> int:
    with tempfile.TemporaryDirectory(prefix="carenest-cross-platform-") as temporary:
        fixture = Path(temporary)
        copy_fixture(fixture)

        valid = run_verifier(fixture)
        require(valid.returncode == 0, f"valid fixture failed:\n{valid.stderr}")

        desktop_program = fixture / "src/CareNest.CrossPlatform.Desktop/Program.cs"
        original_desktop = desktop_program.read_text(encoding="utf-8")
        desktop_program.write_text(
            original_desktop.replace(".UsePlatformDetect()", ".UseSkia()"),
            encoding="utf-8",
        )
        missing_wiring = run_verifier(fixture)
        require(missing_wiring.returncode == 1, "missing desktop wiring was not rejected")
        require(
            ".UsePlatformDetect()" in missing_wiring.stderr,
            "missing desktop wiring failure did not identify the required token",
        )
        desktop_program.write_text(original_desktop, encoding="utf-8")

        main_view = fixture / "src/CareNest.CrossPlatform/Views/MainView.axaml"
        main_view.write_text(main_view.read_text(encoding="utf-8") + "\n<broken>", encoding="utf-8")
        malformed_xaml = run_verifier(fixture)
        require(malformed_xaml.returncode == 1, "malformed Avalonia XAML was not rejected")
        require(
            "malformed XML/XAML" in malformed_xaml.stderr,
            "malformed Avalonia XAML failure was not classified clearly",
        )

    print("Cross-platform target verifier self-tests passed.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
