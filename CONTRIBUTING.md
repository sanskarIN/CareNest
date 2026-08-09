# Contributing to CareNest

Thank you for improving CareNest.

## Before opening a pull request

1. Read `SECURITY.md`, `PRIVACY.md`, and `docs/architecture/ARCHITECTURE.md`.
2. Do not introduce diagnosis, dosage inference, treatment recommendations, interaction claims, risk scoring, or emergency-service substitutes.
3. Do not add analytics, remote data transfer, accounts, or cloud sync without a separate architecture/privacy/security proposal.
4. Never commit secrets, signing materials, real health records, document contents, or personally identifying test fixtures.
5. Add or update tests and documentation for behavior changes.

## Local checks

```bash
dotnet format --verify-no-changes CareNest.sln
dotnet build CareNest.sln
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj
```

For platform changes, also build and smoke-test the affected MAUI target.

## Commit identity

Project maintainer local setup:

```bash
git config user.email "sanskarin@outlook.in"
```

The GitHub web/API may use the authenticated account's configured commit identity. Do not rewrite other contributors' authorship.

## Pull requests

Describe user impact, safety/privacy impact, migration impact, tests performed, and platform-specific limitations. Keep PRs reviewable and do not include real user data.
