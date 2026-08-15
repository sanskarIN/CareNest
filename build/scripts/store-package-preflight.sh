#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
TARGET="${CARENEST_TARGET:-}"

fail() {
  printf 'ERROR: %s\n' "$1" >&2
  exit 1
}

case "$TARGET" in
  net10.0-android|net10.0-ios|net10.0-maccatalyst|net10.0-windows10.0.19041.0)
    ;;
  "")
    fail "CARENEST_TARGET is required for store-package preflight."
    ;;
  *)
    fail "Unsupported CARENEST_TARGET: $TARGET"
    ;;
esac

printf 'CareNest store-package preflight\n'
printf 'Target: %s\n' "$TARGET"
printf 'External funding surface: absent from app runtime by source policy\n'

exec "$SCRIPT_DIR/release-preflight.sh"
