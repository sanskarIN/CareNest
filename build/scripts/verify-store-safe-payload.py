#!/usr/bin/env python3
"""Fail closed when a store-safe build payload contains a forbidden external-commerce marker."""

from __future__ import annotations

import argparse
import os
import sys
import zipfile
from pathlib import Path
from typing import BinaryIO, Iterable

DEFAULT_FORBIDDEN = (
    "buymeacoffee.com/sanskarIN",
    "ramsandesh.gumroad.com",
)
CHUNK_SIZE = 1024 * 1024


def encoded_needles(values: Iterable[str]) -> tuple[tuple[str, tuple[bytes, ...]], ...]:
    return tuple(
        (
            value,
            (
                value.encode("utf-8"),
                value.encode("utf-16-le"),
                value.encode("utf-16-be"),
            ),
        )
        for value in values
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


def scan_regular_file(
    path: Path,
    marker_needles: tuple[tuple[str, tuple[bytes, ...]], ...],
) -> list[tuple[str, str]]:
    matches: list[tuple[str, str]] = []
    try:
        for marker, needles in marker_needles:
            with path.open("rb") as stream:
                if stream_contains(stream, needles):
                    matches.append((marker, str(path)))
        return matches
    except (OSError, PermissionError) as exc:
        raise RuntimeError(f"Could not inspect payload file {path}: {exc}") from exc


def scan_zip(
    path: Path,
    marker_needles: tuple[tuple[str, tuple[bytes, ...]], ...],
) -> list[tuple[str, str]]:
    matches: list[tuple[str, str]] = []
    try:
        with zipfile.ZipFile(path, "r") as archive:
            for info in archive.infolist():
                if info.is_dir():
                    continue
                for marker, needles in marker_needles:
                    with archive.open(info, "r") as stream:
                        if stream_contains(stream, needles):
                            matches.append((marker, f"{path}!/{info.filename}"))
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


def scan_path(
    root: Path,
    marker_needles: tuple[tuple[str, tuple[bytes, ...]], ...],
) -> list[tuple[str, str]]:
    matches: list[tuple[str, str]] = []
    for path in iter_files(root):
        if zipfile.is_zipfile(path):
            matches.extend(scan_zip(path, marker_needles))
        else:
            matches.extend(scan_regular_file(path, marker_needles))
    return matches


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description=(
            "Fail if a store-safe build payload contains a forbidden repository-only "
            "funding or storefront marker."
        )
    )
    parser.add_argument("payload", type=Path, help="File, ZIP/AAB, or directory to inspect")
    parser.add_argument(
        "--forbidden",
        action="append",
        dest="forbidden",
        help=(
            "Forbidden text marker. Repeat to scan multiple markers. "
            "When omitted, CareNest scans its repository-only Buy Me a Coffee and Gumroad destinations."
        ),
    )
    return parser.parse_args()


def main() -> int:
    args = parse_args()
    forbidden = tuple(args.forbidden) if args.forbidden else DEFAULT_FORBIDDEN
    marker_needles = encoded_needles(forbidden)
    try:
        matches = scan_path(args.payload, marker_needles)
    except RuntimeError as exc:
        print(f"Store-safe payload inspection failed: {exc}", file=sys.stderr)
        return 2

    if matches:
        print("Store-safe payload contains forbidden repository-only marker(s):", file=sys.stderr)
        for marker, match in matches:
            print(f"- {marker!r}: {match}", file=sys.stderr)
        return 1

    marker_list = ", ".join(repr(marker) for marker in forbidden)
    print(
        f"Store-safe payload verified: forbidden marker(s) {marker_list} were not found in {args.payload}."
    )
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
