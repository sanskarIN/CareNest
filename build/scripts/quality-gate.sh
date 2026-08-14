#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$ROOT_DIR"

projects=(
  src/CareNest.Shared/CareNest.Shared.csproj
  src/CareNest.Domain/CareNest.Domain.csproj
  src/CareNest.Application/CareNest.Application.csproj
  src/CareNest.Infrastructure/CareNest.Infrastructure.csproj
  tests/CareNest.UnitTests/CareNest.UnitTests.csproj
  tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj
  tests/CareNest.UiTests/CareNest.UiTests.csproj
)

test_projects=(
  tests/CareNest.UnitTests/CareNest.UnitTests.csproj
  tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj
  tests/CareNest.UiTests/CareNest.UiTests.csproj
)

for project in "${projects[@]}"; do
  dotnet format "$project" --verify-no-changes --verbosity minimal
done

dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release --nologo
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release --nologo
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release --nologo
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release --nologo

for project in "${test_projects[@]}"; do
  dotnet test "$project" -c Release --nologo
done

for project in "${test_projects[@]}"; do
  dotnet restore "$project" --nologo -p:NuGetAudit=true -p:NuGetAuditMode=all
done
