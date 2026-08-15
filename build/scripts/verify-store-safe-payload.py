#!/usr/bin/env python3
"""Fail closed when a store-safe build payload contains a forbidden funding marker."""

from __future__ import annotations

import argparse
import os
import sys
import zipfile
from pathlib import Path
from typing import BinaryIO, Iterable

DEFAULT_FORBIDDEN = "buymeacoffee.com/sanskarIN"
CHUNK_SIZE = 1024 * 1024


def encoded_needles(value: str) -> tuple[bytes, ...]:
    return (
        value.encode("utf-8"),
        value.encode("utf-16-le"),
        value.encode("utf-16-be"),
    )


def stream_contains(stream: BinaryIO, needles: tuple[bytes, ...]) -> bool:
    overlap = max(len(needle) for needle in needles) - 1
    tail = b""
    while True:
        chunk = stream.read(CHUNK_SIZE)
        if not chunk:
            return False
        data = tail + chunk
        if any(needle in data for needle in needles):
            return True
        tail = data[-overlap:] if overlap > 0 else b""


def scan_regular_file(path: Path, needles: tuple[bytes, ...]) -> list[str]:
    try:
        with path.open("rb") as stream:
            return [str(path)] if stream_contains(stream, needles) else []
    except (OSError, PermissionError) as exc:
        raise RuntimeError(f"Could not inspect payload file {path}: {exc}") from exc


def scan_zip(path: Path, needles: tuple[bytes, ...]) -> list[str]:
    matches: list[str] = []
    try:
        with zipfile.ZipFile(path, "r") as archive:
            for info in archive.infolist():
                if info.is_dir():
                    continue
                with archive.open(info, "r") as stream:
                    if stream_contains(stream, needles):
                        matches.append(f"{path}!/{info.filename}")
    except (OSError, zipfile.BadZipFile) as exc:
        raise RuntimeError(f"Could not inspect ZIP payload {path}: {exc}") from exc
    return matches


def iter_files(root: Path) -> Iterable[Path]:
    if root.is_file():
        yield root
        return
    if not root.is_dir():
        raise RuntimeError(f"Payload path does not exist or is not a file/directory: {root}")
    for directory, _, files in os.walk(root):
        for filename in files:
            yield Path(directory) / filename


def scan_path(root: Path, needles: tuple[bytes, ...]) -> list[str]:
    matches: list[str] = []
    for path in iter_files(root):
        if zipfile.is_zipfile(path):
            matches.extend(scan_zip(path, needles))
        else:
            matches.extend(scan_regular_file(path, needles))
    return matches


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Fail if a store-safe build payload contains a forbidden funding marker."
    )
    parser.add_argument("payload", type=Path, help="File, ZIP/AAB, or directory to inspect")
    parser.add_argument(
        "--forbidden",
        default=DEFAULT_FORBIDDEN,
        help="Forbidden text marker (default: Buy Me a Coffee project-support host/path)",
    )
    return parser.parse_args()


def main() -> int:
    args = parse_args()
    needles = encoded_needles(args.forbidden)
    try:
        matches = scan_path(args.payload, needles)
    except RuntimeError as exc:
        print(f"Store-safe payload inspection failed: {exc}", file=sys.stderr)
        return 2

    if matches:
        print(
            f"Store-safe payload contains forbidden marker {args.forbidden!r}:",
            file=sys.stderr,
        )
        for match in matches:
            print(f"- {match}", file=sys.stderr)
        return 1

    print(
        f"Store-safe payload verified: forbidden marker {args.forbidden!r} was not found in {args.payload}."
    )
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
