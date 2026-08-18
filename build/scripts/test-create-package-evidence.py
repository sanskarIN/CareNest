#!/usr/bin/env python3
"""Self-test the CareNest package evidence tool with synthetic payloads only."""

from __future__ import annotations

import hashlib
import json
import subprocess
import sys
import tempfile
from pathlib import Path


def repository_root() -> Path:
    return Path(__file__).resolve().parents[2]


def run_tool(*args: str) -> subprocess.CompletedProcess[str]:
    tool = repository_root() / "build" / "scripts" / "create-package-evidence.py"
    return subprocess.run(
        [sys.executable, str(tool), *args],
        cwd=repository_root(),
        check=False,
        capture_output=True,
        text=True,
    )


def require(condition: bool, message: str) -> None:
    if not condition:
        raise RuntimeError(message)


def test_safe_file_manifest(temp: Path) -> None:
    payload = temp / "safe.bin"
    output = temp / "safe-evidence.json"
    content = b"synthetic CareNest package payload\n"
    payload.write_bytes(content)

    completed = run_tool(
        str(payload),
        "--platform",
        "android",
        "--version",
        "1.0.0-rc.1",
        "--build",
        "1",
        "--package-id",
        "com.sanskar.carenest",
        "--stage",
        "inspection",
        "--signing-provenance",
        "synthetic inspection artifact; not production signed",
        "--output",
        str(output),
    )
    require(completed.returncode == 0, f"safe manifest failed: {completed.stderr}")
    require(output.is_file(), "safe manifest output was not created")

    evidence = json.loads(output.read_text(encoding="utf-8"))
    expected = hashlib.sha256(content).hexdigest()
    require(evidence["schemaVersion"] == 1, "unexpected evidence schema version")
    require(evidence["stage"] == "inspection", "unexpected evidence stage")
    require(evidence["platform"] == "android", "unexpected evidence platform")
    require(evidence["payload"]["fileCount"] == 1, "unexpected file count")
    require(evidence["payload"]["sha256"] == expected, "payload SHA-256 mismatch")
    require(evidence["payload"]["files"][0]["sha256"] == expected, "file SHA-256 mismatch")
    require(evidence["storeSafePayloadScan"]["status"] == "passed", "store-safe scan was not recorded")


def test_safe_directory_manifest(temp: Path) -> None:
    payload = temp / "package-dir"
    payload.mkdir()
    (payload / "a.txt").write_text("alpha\n", encoding="utf-8")
    nested = payload / "nested"
    nested.mkdir()
    (nested / "b.txt").write_text("beta\n", encoding="utf-8")
    output = temp / "directory-evidence.json"

    completed = run_tool(
        str(payload),
        "--platform",
        "windows",
        "--version",
        "1.0.0-rc.1",
        "--build",
        "1",
        "--package-id",
        "com.sanskar.carenest",
        "--stage",
        "inspection",
        "--signing-provenance",
        "synthetic unpackaged inspection output",
        "--output",
        str(output),
    )
    require(completed.returncode == 0, f"directory manifest failed: {completed.stderr}")
    evidence = json.loads(output.read_text(encoding="utf-8"))
    require(evidence["payload"]["kind"] == "directory", "directory payload kind was not recorded")
    require(evidence["payload"]["fileCount"] == 2, "directory file count mismatch")
    require(
        [entry["path"] for entry in evidence["payload"]["files"]] == ["a.txt", "nested/b.txt"],
        "directory file ordering is not deterministic",
    )


def test_forbidden_marker_fails_closed(temp: Path) -> None:
    payload = temp / "unsafe.txt"
    payload.write_text("repository-only marker: ramsandesh.gumroad.com\n", encoding="utf-8")
    output = temp / "unsafe-evidence.json"

    completed = run_tool(
        str(payload),
        "--platform",
        "android",
        "--version",
        "1.0.0-rc.1",
        "--build",
        "1",
        "--package-id",
        "com.sanskar.carenest",
        "--stage",
        "inspection",
        "--signing-provenance",
        "synthetic inspection artifact",
        "--output",
        str(output),
    )
    require(completed.returncode != 0, "forbidden marker unexpectedly passed evidence generation")
    require(not output.exists(), "failed forbidden-marker scan unexpectedly produced evidence output")
    require("Store-safe payload scan failed" in completed.stderr, "failure did not identify store-safe scan")


def test_output_inside_payload_is_rejected(temp: Path) -> None:
    payload = temp / "inside-output-package"
    payload.mkdir()
    (payload / "app.bin").write_bytes(b"safe")
    output = payload / "evidence.json"

    completed = run_tool(
        str(payload),
        "--platform",
        "windows",
        "--version",
        "1.0.0-rc.1",
        "--build",
        "1",
        "--package-id",
        "com.sanskar.carenest",
        "--stage",
        "inspection",
        "--signing-provenance",
        "synthetic inspection artifact",
        "--output",
        str(output),
    )
    require(completed.returncode != 0, "evidence output inside payload unexpectedly passed")
    require(not output.exists(), "rejected inside-payload evidence output was created")
    require("outside the payload directory" in completed.stderr, "inside-payload rejection reason missing")


def test_production_requires_tag(temp: Path) -> None:
    payload = temp / "production-without-tag.bin"
    payload.write_bytes(b"synthetic signed-looking payload")
    output = temp / "production-without-tag.json"

    completed = run_tool(
        str(payload),
        "--platform",
        "android",
        "--version",
        "1.0.0",
        "--build",
        "1",
        "--package-id",
        "com.sanskar.carenest",
        "--stage",
        "production",
        "--signing-provenance",
        "Android production signing service fingerprint recorded separately",
        "--output",
        str(output),
    )
    require(completed.returncode != 0, "production evidence unexpectedly passed without a v* tag")
    require(not output.exists(), "failed production identity validation unexpectedly produced evidence")
    require("requires --source-tag" in completed.stderr, "production tag requirement was not enforced")


def main() -> int:
    try:
        with tempfile.TemporaryDirectory(prefix="carenest-package-evidence-test-") as directory:
            temp = Path(directory)
            test_safe_file_manifest(temp)
            test_safe_directory_manifest(temp)
            test_forbidden_marker_fails_closed(temp)
            test_output_inside_payload_is_rejected(temp)
            test_production_requires_tag(temp)
    except (OSError, RuntimeError, json.JSONDecodeError) as exc:
        print(f"Package evidence self-test failed: {exc}", file=sys.stderr)
        return 1

    print("Package evidence self-test passed.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
