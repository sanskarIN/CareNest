#!/usr/bin/env python3
"""Validate repository-local links in active CareNest Markdown documentation.

The checker is intentionally offline. It verifies local file/directory targets and
repository containment, while leaving network URL availability to release/manual
review so CI does not depend on external sites.
"""

from __future__ import annotations

import argparse
import re
import subprocess
import sys
from pathlib import Path
from urllib.parse import unquote, urlsplit

MARKDOWN_LINK_RE = re.compile(r"!?\[[^\]]*\]\(([^)]+)\)")
HTML_LINK_RE = re.compile(r"(?:href|src)\s*=\s*[\"']([^\"']+)[\"']", re.IGNORECASE)
REFERENCE_LINK_RE = re.compile(r"^\s*\[[^\]]+\]:\s*(\S+)", re.MULTILINE)
SKIPPED_SCHEMES = {
    "data",
    "ftp",
    "http",
    "https",
    "mailto",
    "tel",
}
SKIPPED_DIRECTORY_NAMES = {
    ".git",
    ".vs",
    ".vscode",
    "artifacts",
    "bin",
    "node_modules",
    "obj",
}


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Verify local links in active CareNest Markdown documentation."
    )
    parser.add_argument(
        "--root",
        type=Path,
        default=Path(__file__).resolve().parents[2],
        help="Repository root. Defaults to the root containing this script.",
    )
    parser.add_argument(
        "--include-history",
        action="store_true",
        help="Also validate immutable docs/history snapshots.",
    )
    return parser.parse_args()


def tracked_markdown_files(root: Path) -> list[Path]:
    try:
        result = subprocess.run(
            ["git", "-C", str(root), "ls-files", "*.md"],
            check=True,
            capture_output=True,
            text=True,
        )
    except (FileNotFoundError, subprocess.CalledProcessError):
        return sorted(
            path
            for path in root.rglob("*.md")
            if not any(part in SKIPPED_DIRECTORY_NAMES for part in path.relative_to(root).parts)
        )

    return sorted(root / line for line in result.stdout.splitlines() if line.strip())


def is_history_path(root: Path, source: Path) -> bool:
    parts = source.relative_to(root).parts
    return len(parts) >= 2 and parts[0] == "docs" and parts[1] == "history"


def extract_targets(text: str) -> list[str]:
    targets: list[str] = []

    for match in MARKDOWN_LINK_RE.finditer(text):
        value = match.group(1).strip()
        if value.startswith("<") and ">" in value:
            value = value[1 : value.index(">")]
        else:
            # Markdown permits an optional title after the destination. CareNest
            # local paths are expected to encode spaces, so the first token is
            # the destination for the common non-angle-bracket form.
            value = value.split(maxsplit=1)[0] if value else value
        if value:
            targets.append(value)

    targets.extend(match.group(1).strip() for match in HTML_LINK_RE.finditer(text))
    targets.extend(match.group(1).strip() for match in REFERENCE_LINK_RE.finditer(text))
    return targets


def local_target_path(root: Path, source: Path, raw_target: str) -> tuple[Path | None, str | None]:
    target = raw_target.strip()
    if not target or target.startswith("#"):
        return None, None
    if "${{" in target or "{{" in target:
        return None, None

    parsed = urlsplit(target)
    if parsed.scheme.lower() in SKIPPED_SCHEMES or parsed.netloc:
        return None, None
    if parsed.scheme:
        # Unknown URI schemes are not local filesystem links.
        return None, None

    decoded_path = unquote(parsed.path).strip()
    if not decoded_path:
        return None, None

    root_resolved = root.resolve()
    if decoded_path.startswith("/"):
        candidate = root_resolved / decoded_path.lstrip("/")
    else:
        candidate = source.parent.resolve() / decoded_path
    candidate = candidate.resolve()

    try:
        candidate.relative_to(root_resolved)
    except ValueError:
        return candidate, "escapes repository root"

    return candidate, None


def main() -> int:
    args = parse_args()
    root = args.root.resolve()
    if not root.is_dir():
        print(f"Documentation root does not exist: {root}", file=sys.stderr)
        return 2

    failures: list[str] = []
    checked_links = 0
    checked_files = 0

    for source in tracked_markdown_files(root):
        if not source.is_file():
            continue
        if not args.include_history and is_history_path(root, source):
            continue

        checked_files += 1
        text = source.read_text(encoding="utf-8")
        for raw_target in extract_targets(text):
            candidate, error = local_target_path(root, source, raw_target)
            if candidate is None:
                continue

            checked_links += 1
            relative_source = source.relative_to(root).as_posix()
            if error is not None:
                failures.append(f"{relative_source}: {raw_target!r} {error}")
                continue
            if not candidate.exists():
                failures.append(
                    f"{relative_source}: {raw_target!r} -> missing "
                    f"{candidate.relative_to(root).as_posix()}"
                )

    if failures:
        print("Documentation link integrity check failed:", file=sys.stderr)
        for failure in failures:
            print(f"- {failure}", file=sys.stderr)
        print(
            f"Checked {checked_links} local links across {checked_files} active Markdown files; "
            f"found {len(failures)} problem(s).",
            file=sys.stderr,
        )
        return 1

    print(
        f"Documentation link integrity check passed: {checked_links} local links across "
        f"{checked_files} active Markdown files."
    )
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
