#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$ROOT_DIR"

say() { printf '\n==> %s\n' "$1"; }
fail() { printf '\nERROR: %s\n' "$1" >&2; exit 1; }

command -v dotnet >/dev/null 2>&1 || fail "dotnet SDK is required."

FUNDING_LINK="${CARENEST_SHOW_FUNDING_LINK:-true}"
case "$FUNDING_LINK" in
  true|false) ;;
  *) fail "CARENEST_SHOW_FUNDING_LINK must be 'true' or 'false'." ;;
esac

say "CareNest release preflight"
printf 'Repository: %s\n' "$ROOT_DIR"
printf 'CareNestShowFundingLink: %s\n' "$FUNDING_LINK"
dotnet --info

say "Source hygiene"
if command -v rg >/dev/null 2>&1; then
  if rg -n --hidden --glob '!**/bin/**' --glob '!**/obj/**' --glob '!.git/**' '(TODO|FIXME|NotImplementedException)' src tests; then
    fail "Release-blocking implementation marker found."
  fi
else
  if grep -RInE '(TODO|FIXME|NotImplementedException)' src tests --exclude-dir=bin --exclude-dir=obj --exclude-dir=.git; then
    fail "Release-blocking implementation marker found."
  fi
fi

say "Formatting"
dotnet format CareNest.sln --verify-no-changes

say "Core Release builds"
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release --nologo
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release --nologo
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release --nologo

say "Automated tests"
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release --nologo
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release --nologo
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release --nologo

say "Blocking dependency audit"
for project in \
  tests/CareNest.UnitTests/CareNest.UnitTests.csproj \
  tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj \
  tests/CareNest.UiTests/CareNest.UiTests.csproj; do
  dotnet restore "$project" --nologo -p:NuGetAudit=true -p:NuGetAuditMode=all
done

if [[ -n "${CARENEST_TARGET:-}" ]]; then
  say "Audit optional MAUI target: ${CARENEST_TARGET}"
  dotnet restore src/CareNest.App/CareNest.App.csproj \
    -p:CareNestTargetFramework="$CARENEST_TARGET" \
    -p:CareNestShowFundingLink="$FUNDING_LINK" \
    -p:NuGetAudit=true \
    -p:NuGetAuditMode=all \
    --nologo

  say "Optional MAUI Release build: ${CARENEST_TARGET}"
  dotnet build src/CareNest.App/CareNest.App.csproj \
    -f "$CARENEST_TARGET" \
    -c Release \
    -p:CareNestTargetFramework="$CARENEST_TARGET" \
    -p:CareNestShowFundingLink="$FUNDING_LINK" \
    --nologo
fi

say "Preflight complete"
printf '%s\n' "Automated source checks completed. Manual device, accessibility, signing, store-policy, and packaged existing-data/encrypted-data compatibility evidence are still required where applicable."
