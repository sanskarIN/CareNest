#!/usr/bin/env python3
"""Synthetic self-test for verify-documentation-links.py."""

from __future__ import annotations

import subprocess
import sys
import tempfile
from pathlib import Path

CHECKER = Path(__file__).with_name("verify-documentation-links.py")


def run(root: Path, *extra_args: str) -> subprocess.CompletedProcess[str]:
    return subprocess.run(
        [sys.executable, str(CHECKER), "--root", str(root), *extra_args],
        capture_output=True,
        text=True,
        check=False,
    )


def write(path: Path, text: str) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(text, encoding="utf-8")


def main() -> int:
    with tempfile.TemporaryDirectory(prefix="carenest-doc-links-") as temp:
        root = Path(temp)
        write(root / "docs" / "guide.md", "# Guide\n")
        write(root / "assets" / "logo.svg", "<svg xmlns='http://www.w3.org/2000/svg'/>\n")
        write(
            root / "README.md",
            "\n".join(
                [
                    "# Synthetic docs",
                    "[Guide](docs/guide.md#usage)",
                    "![Logo](assets/logo.svg)",
                    '<a href="docs/guide.md">HTML guide</a>',
                    "[Reference][guide-ref]",
                    "[guide-ref]: docs/guide.md",
                    "[External](https://example.com)",
                    "[Mail](mailto:test@example.com)",
                    "[Anchor](#synthetic-docs)",
                ]
            ),
        )

        good = run(root)
        if good.returncode != 0:
            print(good.stdout, end="")
            print(good.stderr, end="", file=sys.stderr)
            raise AssertionError("Clean synthetic documentation should pass.")

        write(root / "BROKEN.md", "[Missing](docs/missing.md)\n")
        missing = run(root)
        if missing.returncode == 0 or "docs/missing.md" not in missing.stderr:
            raise AssertionError("Missing local targets must fail closed.")

        (root / "BROKEN.md").unlink()
        outside = root.parent / "outside-doc-link-test.txt"
        outside.write_text("outside", encoding="utf-8")
        try:
            write(root / "ESCAPE.md", f"[Escape](../{outside.name})\n")
            escaped = run(root)
            if escaped.returncode == 0 or "escapes repository root" not in escaped.stderr:
                raise AssertionError("Repository-escaping links must fail closed.")
        finally:
            outside.unlink(missing_ok=True)

        write(root / "docs" / "history" / "snapshot.md", "[Historical missing](old.md)\n")
        (root / "ESCAPE.md").unlink()
        history = run(root)
        if history.returncode != 0:
            raise AssertionError("docs/history must be excluded by default.")

        write(root / "PROJECT_STATUS.md", "[Dynamic missing](docs/dynamic-missing.md)\n")
        dynamic_default = run(root)
        if dynamic_default.returncode != 0:
            raise AssertionError("Dynamic evidence/status files must be excluded by default.")

        dynamic_explicit = run(root, "--include-dynamic")
        if dynamic_explicit.returncode == 0 or "docs/dynamic-missing.md" not in dynamic_explicit.stderr:
            raise AssertionError("--include-dynamic must audit dynamic evidence/status links.")

        history_explicit = run(root, "--include-history")
        if history_explicit.returncode == 0 or "old.md" not in history_explicit.stderr:
            raise AssertionError("--include-history must audit historical snapshots when requested.")

    print("Documentation link checker self-test passed.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
