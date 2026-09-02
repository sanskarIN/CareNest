#!/usr/bin/env python3
"""Create fail-closed CareNest package checksum/provenance evidence.

The tool does not sign packages or claim store approval. It records exact source,
package hashes and store-safe payload scan output for inspection or production
artifacts. Production mode additionally requires an immutable v* tag that
resolves to the recorded source SHA, a matching checked-out HEAD, a clean tracked
workspace, and non-secret signing/notarization provenance.
"""

from __future__ import annotations

import argparse
import hashlib
import json
import os
import re
import subprocess
import sys
import tempfile
from datetime import datetime, timezone
from pathlib import Path
from typing import Iterable

CHUNK_SIZE = 1024 * 1024
SHA_PATTERN = re.compile(r"^[0-9a-f]{40}$")
UNSIGNED_VALUES = {"", "none", "n/a", "na", "unsigned", "not-applicable", "not applicable"}


def repository_root() -> Path:
    return Path(__file__).resolve().parents[2]


def run_git(root: Path, *args: str) -> str:
    completed = subprocess.run(
        ["git", *args],
        cwd=root,
        check=False,
        capture_output=True,
        text=True,
    )
    if completed.returncode != 0:
        detail = completed.stderr.strip() or completed.stdout.strip() or "unknown git error"
        raise RuntimeError(f"git {' '.join(args)} failed: {detail}")
    return completed.stdout.strip()


def normalize_sha(value: str) -> str:
    candidate = value.strip().lower()
    if not SHA_PATTERN.fullmatch(candidate):
        raise RuntimeError(f"Expected a full 40-character Git SHA, got: {value!r}")
    return candidate


def resolve_source_sha(root: Path, supplied: str | None) -> str:
    if supplied:
        candidate = normalize_sha(supplied)
        resolved = normalize_sha(run_git(root, "rev-parse", "--verify", f"{candidate}^{{commit}}"))
        if resolved != candidate:
            raise RuntimeError(f"Supplied source SHA resolved unexpectedly: {candidate} -> {resolved}")
        return candidate
    return normalize_sha(run_git(root, "rev-parse", "HEAD"))


def tracked_workspace_status(root: Path) -> str:
    return run_git(root, "status", "--porcelain=v1", "--untracked-files=no")


def resolve_ref_sha(root: Path, ref: str) -> str:
    return normalize_sha(run_git(root, "rev-list", "-n", "1", ref))


def sha256_file(path: Path) -> str:
    digest = hashlib.sha256()
    try:
        with path.open("rb") as stream:
            while True:
                block = stream.read(CHUNK_SIZE)
                if not block:
                    break
                digest.update(block)
    except OSError as exc:
        raise RuntimeError(f"Could not hash payload file {path}: {exc}") from exc
    return digest.hexdigest()


def iter_payload_files(payload: Path) -> Iterable[tuple[str, Path]]:
    if payload.is_file():
        if payload.is_symlink():
            raise RuntimeError(f"Payload must not be a symbolic link: {payload}")
        yield payload.name, payload
        return
    if not payload.is_dir():
        raise RuntimeError(f"Payload does not exist or is not a file/directory: {payload}")

    candidates = sorted(payload.rglob("*"))
    for candidate in candidates:
        if candidate.is_symlink():
            raise RuntimeError(f"Payload contains a symbolic link: {candidate}")
        if candidate.is_file():
            yield candidate.relative_to(payload).as_posix(), candidate


def collect_payload_evidence(payload: Path) -> tuple[list[dict[str, object]], str, int]:
    files: list[dict[str, object]] = []
    aggregate = hashlib.sha256()
    total_bytes = 0

    for relative_path, path in iter_payload_files(payload):
        try:
            size = path.stat().st_size
        except OSError as exc:
            raise RuntimeError(f"Could not stat payload file {path}: {exc}") from exc
        digest = sha256_file(path)
        total_bytes += size
        files.append({"path": relative_path, "size": size, "sha256": digest})
        aggregate.update(relative_path.encode("utf-8"))
        aggregate.update(b"\0")
        aggregate.update(digest.encode("ascii"))
        aggregate.update(b"\0")
        aggregate.update(str(size).encode("ascii"))
        aggregate.update(b"\n")

    if not files:
        raise RuntimeError(f"Payload contains no files: {payload}")

    payload_digest = str(files[0]["sha256"]) if payload.is_file() else aggregate.hexdigest()
    return files, payload_digest, total_bytes


def display_scanner_path(scanner: Path) -> str:
    resolved = scanner.resolve()
    try:
        return resolved.relative_to(repository_root()).as_posix()
    except ValueError:
        return str(resolved)


def run_store_safe_scan(scanner: Path, payload: Path) -> dict[str, object]:
    if not scanner.is_file():
        raise RuntimeError(f"Store-safe scanner does not exist: {scanner}")

    completed = subprocess.run(
        [sys.executable, str(scanner), str(payload)],
        check=False,
        capture_output=True,
        text=True,
    )
    stdout = completed.stdout.strip()
    stderr = completed.stderr.strip()

    if completed.returncode != 0:
        detail = stderr or stdout or f"exit code {completed.returncode}"
        raise RuntimeError(f"Store-safe payload scan failed: {detail}")

    return {
        "status": "passed",
        "scanner": display_scanner_path(scanner),
        "stdout": stdout,
    }


def ensure_output_outside_payload(payload: Path, output: Path) -> None:
    payload_resolved = payload.resolve()
    output_resolved = output.resolve()

    if payload_resolved.is_file():
        if output_resolved == payload_resolved:
            raise RuntimeError("Evidence output must not replace the payload file.")
        return

    try:
        output_resolved.relative_to(payload_resolved)
    except ValueError:
        return
    raise RuntimeError("Evidence output must be outside the payload directory so hashing stays stable.")


def write_json_atomic(output: Path, data: dict[str, object]) -> None:
    output.parent.mkdir(parents=True, exist_ok=True)
    serialized = json.dumps(data, indent=2, sort_keys=True, ensure_ascii=False) + "\n"
    temp_name: str | None = None
    try:
        with tempfile.NamedTemporaryFile(
            mode="w",
            encoding="utf-8",
            dir=output.parent,
            prefix=f".{output.name}.",
            suffix=".tmp",
            delete=False,
        ) as stream:
            temp_name = stream.name
            stream.write(serialized)
            stream.flush()
            os.fsync(stream.fileno())
        os.replace(temp_name, output)
    except OSError as exc:
        if temp_name:
            try:
                Path(temp_name).unlink(missing_ok=True)
            except OSError:
                pass
        raise RuntimeError(f"Could not write evidence file {output}: {exc}") from exc


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Create checksum/provenance evidence for a CareNest inspection or production package."
    )
    parser.add_argument("payload", type=Path, help="Package file or published package directory")
    parser.add_argument("--platform", required=True, choices=("android", "windows", "ios", "maccatalyst"))
    parser.add_argument("--version", required=True, help="Display/release version, for example 1.0.0")
    parser.add_argument("--build", required=True, help="Platform build/version number")
    parser.add_argument("--package-id", required=True, help="Application/package/bundle identity")
    parser.add_argument("--stage", choices=("inspection", "production"), default="inspection")
    parser.add_argument("--source-sha", help="Full source SHA; defaults to repository HEAD")
    parser.add_argument("--source-tag", help="Immutable v* release tag; required for production stage")
    parser.add_argument(
        "--signing-provenance",
        required=True,
        help="Non-secret signing/notarization/store-managed provenance description",
    )
    parser.add_argument("--notes", default="", help="Optional non-sensitive operator notes")
    parser.add_argument(
        "--scanner",
        type=Path,
        default=Path(__file__).resolve().with_name("verify-store-safe-payload.py"),
        help="Store-safe payload scanner path",
    )
    parser.add_argument("--output", type=Path, required=True, help="JSON evidence output path")
    return parser.parse_args()


def validate_release_identity(args: argparse.Namespace, root: Path, source_sha: str) -> str:
    tracked_status = tracked_workspace_status(root)

    if args.stage != "production":
        return tracked_status

    if not args.source_tag or not args.source_tag.startswith("v"):
        raise RuntimeError("Production evidence requires --source-tag with an immutable v* release tag.")

    tag_sha = resolve_ref_sha(root, args.source_tag)
    if tag_sha != source_sha:
        raise RuntimeError(
            f"Production source tag {args.source_tag!r} resolves to {tag_sha}, not recorded source {source_sha}."
        )

    head_sha = normalize_sha(run_git(root, "rev-parse", "HEAD"))
    if head_sha != source_sha:
        raise RuntimeError(
            f"Production evidence requires checked-out HEAD {head_sha} to equal recorded source {source_sha}."
        )

    if tracked_status:
        raise RuntimeError("Production evidence requires a clean tracked Git workspace.")

    signing = args.signing_provenance.strip().lower()
    if signing in UNSIGNED_VALUES or "unsigned" in signing:
        raise RuntimeError("Production evidence requires real non-secret signing/notarization provenance.")

    return tracked_status


def main() -> int:
    args = parse_args()
    root = repository_root()
    payload = args.payload.expanduser()
    output = args.output.expanduser()
    scanner = args.scanner.expanduser()

    try:
        if not payload.exists():
            raise RuntimeError(f"Payload path does not exist: {payload}")
        ensure_output_outside_payload(payload, output)

        source_sha = resolve_source_sha(root, args.source_sha)
        tracked_status = validate_release_identity(args, root, source_sha)
        scan = run_store_safe_scan(scanner, payload)
        files, payload_digest, total_bytes = collect_payload_evidence(payload)

        evidence: dict[str, object] = {
            "schemaVersion": 1,
            "generatedAtUtc": datetime.now(timezone.utc).isoformat().replace("+00:00", "Z"),
            "stage": args.stage,
            "platform": args.platform,
            "version": args.version,
            "build": args.build,
            "packageIdentity": args.package_id,
            "sourceSha": source_sha,
            "sourceTag": args.source_tag or None,
            "trackedWorkspaceClean": tracked_status == "",
            "signingProvenance": args.signing_provenance.strip(),
            "payload": {
                "name": payload.name,
                "kind": "file" if payload.is_file() else "directory",
                "fileCount": len(files),
                "totalBytes": total_bytes,
                "sha256": payload_digest,
                "files": files,
            },
            "storeSafePayloadScan": scan,
            "notes": args.notes,
        }
        write_json_atomic(output, evidence)
    except RuntimeError as exc:
        print(f"CareNest package evidence generation failed: {exc}", file=sys.stderr)
        return 2

    print(f"CareNest package evidence written: {output}")
    print(f"Payload SHA-256: {payload_digest}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())