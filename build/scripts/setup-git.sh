#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$ROOT_DIR"

command -v git >/dev/null 2>&1 || {
  echo "ERROR: git is required." >&2
  exit 1
}

git rev-parse --is-inside-work-tree >/dev/null 2>&1 || {
  echo "ERROR: CareNest repository checkout was not found." >&2
  exit 1
}

git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"

configured_name="$(git config --local --get user.name)"
configured_email="$(git config --local --get user.email)"

[[ "$configured_name" == "Sanskar" ]] || {
  echo "ERROR: repository-local Git user.name verification failed." >&2
  exit 1
}
[[ "$configured_email" == "sanskarin@outlook.in" ]] || {
  echo "ERROR: repository-local Git user.email verification failed." >&2
  exit 1
}

echo "Configured repository-local Git identity."
