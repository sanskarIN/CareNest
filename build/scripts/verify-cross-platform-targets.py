#!/usr/bin/env python3
"""Fail closed when CareNest cross-platform build targets drift out of configuration."""

from argparse import ArgumentParser
from pathlib import Path
import sys
import xml.etree.ElementTree as ET

DEFAULT_ROOT = Path(__file__).resolve().parents[2]

CHECKS = {
    "src/CareNest.App/CareNest.App.csproj": (
        "net10.0-android",
        "net10.0-ios",
        "net10.0-maccatalyst",
        "net10.0-windows10.0.19041.0",
    ),
    "src/CareNest.CrossPlatform/CareNest.CrossPlatform.csproj": (
        "<TargetFramework>net10.0</TargetFramework>",
        "<PackageReference Include=\"Avalonia\" />",
    ),
    "src/CareNest.CrossPlatform/App.axaml.cs": (
        "class App : Avalonia.Application",
        "IClassicDesktopStyleApplicationLifetime",
        "ISingleViewApplicationLifetime",
    ),
    "src/CareNest.CrossPlatform.Desktop/CareNest.CrossPlatform.Desktop.csproj": (
        "<TargetFramework>net10.0</TargetFramework>",
        "<PackageReference Include=\"Avalonia.Desktop\" />",
    ),
    "src/CareNest.CrossPlatform.Desktop/Program.cs": (
        "AppBuilder.Configure<App>()",
        ".UsePlatformDetect()",
        ".StartWithClassicDesktopLifetime(args)",
    ),
    "src/CareNest.CrossPlatform.Browser/CareNest.CrossPlatform.Browser.csproj": (
        "Microsoft.NET.Sdk.WebAssembly",
        "<TargetFramework>net10.0-browser</TargetFramework>",
        "<PackageReference Include=\"Avalonia.Browser\" />",
    ),
    "src/CareNest.CrossPlatform.Browser/Program.cs": (
        "AppBuilder.Configure<App>()",
        ".StartBrowserAppAsync(\"out\")",
    ),
    "Directory.Packages.props": (
        "<PackageVersion Include=\"Avalonia\"",
        "<PackageVersion Include=\"Avalonia.Desktop\"",
        "<PackageVersion Include=\"Avalonia.Browser\"",
    ),
    "CareNest.sln": (
        "CareNest.CrossPlatform",
        "CareNest.CrossPlatform.Desktop",
        "CareNest.CrossPlatform.Browser",
    ),
    ".github/workflows/ci.yml": (
        "linux-desktop:",
        "browser:",
        "CareNest.CrossPlatform.Desktop.csproj",
        "CareNest.CrossPlatform.Browser.csproj",
    ),
    ".github/workflows/dependency-review.yml": (
        "Audit Avalonia desktop dependency graph",
        "Audit Avalonia browser dependency graph",
        "CareNest.CrossPlatform.Desktop.csproj",
        "CareNest.CrossPlatform.Browser.csproj",
    ),
    ".github/workflows/release-gate.yml": (
        "release-cross-platform-hosts:",
        "CareNest.CrossPlatform.Desktop.csproj",
        "CareNest.CrossPlatform.Browser.csproj",
        "verify-cross-platform-targets.py",
    ),
}

AVALONIA_XAML = (
    "src/CareNest.CrossPlatform/App.axaml",
    "src/CareNest.CrossPlatform/Views/MainView.axaml",
)


def collect_errors(root: Path) -> list[str]:
    errors: list[str] = []

    for relative_path, required_tokens in CHECKS.items():
        path = root / relative_path
        if not path.is_file():
            errors.append(f"missing required cross-platform file: {relative_path}")
            continue

        text = path.read_text(encoding="utf-8")
        for token in required_tokens:
            if token not in text:
                errors.append(f"{relative_path}: missing required token {token!r}")

    for relative_path in AVALONIA_XAML:
        path = root / relative_path
        if not path.is_file():
            errors.append(f"missing required Avalonia XAML file: {relative_path}")
            continue

        try:
            ET.parse(path)
        except ET.ParseError as exc:
            errors.append(f"{relative_path}: malformed XML/XAML: {exc}")

    return errors


def parse_args() -> Path:
    parser = ArgumentParser(description=__doc__)
    parser.add_argument(
        "--root",
        type=Path,
        default=DEFAULT_ROOT,
        help="repository root to verify (defaults to the current CareNest checkout)",
    )
    return parser.parse_args().root.resolve()


def main() -> int:
    root = parse_args()
    errors = collect_errors(root)

    if errors:
        print("CareNest cross-platform target verification failed:", file=sys.stderr)
        for error in errors:
            print(f"- {error}", file=sys.stderr)
        return 1

    print(
        "CareNest cross-platform target verification passed: "
        "Android, iOS/iPadOS, macOS, Windows, Linux desktop and browser hosts are configured, "
        "dependency-audited, release-gated, wired to their Avalonia lifetimes and backed by well-formed Avalonia XAML."
    )
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
