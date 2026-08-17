# Getting Started with CareNest

<p align="center">
  <a href="https://ramsandesh.gumroad.com">
    <img src="assets/gumroad_store_badge.svg" alt="Shop on Gumroad — https://ramsandesh.gumroad.com" width="780" />
  </a>
</p>

This guide is the shortest safe path into CareNest for users, evaluators, contributors, and maintainers. It points to deeper references when a task becomes specialized.

## Product boundary first

CareNest is a local-first health organizer. It can organize user-entered profiles, medicines, schedules, reminders, appointments, documents, stock/refill notes, reports and backups.

CareNest does **not** diagnose conditions, calculate or infer dosage, recommend treatment, perform clinical medication-interaction checking, create clinical risk scores, replace a clinician/pharmacist, provide emergency services, or guarantee operating-system notification delivery.

## Ram Sandesh Gumroad Store

**[Shop on Gumroad → https://ramsandesh.gumroad.com](https://ramsandesh.gumroad.com)**

The Gumroad storefront contains separate publisher/project resources and is not a CareNest health feature. A Gumroad purchase does not unlock diagnosis, dosage guidance, treatment recommendations, reminder priority/reliability, emergency assistance, accounts/cloud services, or access to user health data.

CareNest does not automatically send local health records to Gumroad, and the current application package intentionally excludes the Gumroad destination and repository promotional badge.

See `GUMROAD.md` and `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`.

## Current release state

Current release line: `1.0.0-rc.1`.

Latest fully verified pre-Gumroad source:

`7cbe5568b6cffa06c279b29f3cb1b107ea988791`

That exact source verified:

- 122 unit tests;
- 39 integration tests;
- 173 UI/source-policy tests;
- **334/334 total core tests**;
- Android, Windows, iOS simulator and Mac Catalyst Release builds;
- all four store-candidate configurations;
- CodeQL.

The current Gumroad rollout changes repository/source-policy tests and the package scanner, so it must complete its own exact-source workflow matrix before replacing that baseline. Use `PROJECT_STATUS.md` and `what_changed.md` for the active source boundary.

This is a release candidate, not a claim of completed production signing/store publication.

## For a person evaluating the product

Read in this order:

1. `README.md` — purpose, scope, current status and highlighted Gumroad storefront.
2. `GUMROAD.md` — storefront and health-functionality separation.
3. `docs/FEATURE_REFERENCE.md` — feature behavior.
4. `docs/USER_GUIDE.md` — complete workflows.
5. `docs/KNOWN_LIMITATIONS.md` — external and intentional limitations.
6. `PRIVACY.md` and `docs/privacy/PRIVACY_MODEL.md` — data handling.
7. `SECURITY.md` and `docs/security/SECURITY_MODEL.md` — security design.

## For a developer cloning the repository

### 1. Clone

```bash
git clone https://github.com/sanskarIN/CareNest.git
cd CareNest
```

### 2. Inspect the toolchain

```bash
git --version
dotnet --info
dotnet workload list
```

CareNest uses .NET 10 and .NET MAUI. Platform workloads are required only for the targets you intend to build.

### 3. Configure repository-local Git identity if you are the project maintainer

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Repository helpers are available:

```bash
build/scripts/setup-git.sh
```

or on PowerShell:

```powershell
./build/scripts/setup-git.ps1
```

### 4. Restore/build platform-neutral projects

```bash
dotnet restore CareNest.sln

dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
```

### 5. Run the core test suites

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

At the latest fully verified pre-Gumroad source boundary the totals were:

- 122 unit;
- 39 integration;
- 173 UI/source-policy;
- 334 total.

The Gumroad rollout adds additional independent source-policy coverage, so current-branch test totals can legitimately be higher. Treat an unexpected decrease as something to investigate.

### 6. Run the repository quality gate

```bash
build/scripts/quality-gate.sh
```

or:

```powershell
./build/scripts/quality-gate.ps1
```

### 7. Build one MAUI target

Android example:

```bash
dotnet workload install maui-android

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android \
  -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

See `docs/setup/PLATFORM_SETUP.md` for Windows, iOS simulator and Mac Catalyst commands.

## Supported project targets

The current application project declares:

- Android: `net10.0-android`, minimum Android API 24;
- iOS: `net10.0-ios`, minimum iOS 15;
- Mac Catalyst: `net10.0-maccatalyst`, minimum 15;
- Windows: `net10.0-windows10.0.19041.0`, minimum Windows target 10.0.19041.0.

The repository supports `CareNestTargetFramework` so CI/developers can isolate one MAUI target instead of evaluating every platform workload on every host.

## Current strict XAML policy

The app project enables compiled Source bindings and strict XAML compilation. `XC0022`, `XC0023`, `XC0024` and `XC0025` are treated as errors.

When adding a binding-bearing page/template:

- give the page a real root `x:DataType`;
- give each binding-bearing `DataTemplate` its own item `x:DataType`;
- type picker `ItemDisplayBinding` expressions when their context is an item;
- type explicit `Source`/ancestor bindings;
- do not hide these warnings with `NoWarn`, `x:Object` or `x:Null`.

See `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

## External-commerce package rule for developers

The following are repository-only under the current release/store policy:

- `https://ramsandesh.gumroad.com`;
- `https://buymeacoffee.com/sanskarIN`;
- `docs/assets/gumroad_store_badge.svg`.

Do not copy these into `src/CareNest.App`, runtime ViewModels/XAML, shared URL constants or packaged app resources unless a future explicitly reviewed product/store-policy redesign changes the boundary.

Relevant contracts:

- `tests/CareNest.UiTests/FundingLinkContractTests.cs`;
- `tests/CareNest.UiTests/StoreFundingPayloadContractTests.cs`;
- `build/scripts/verify-store-safe-payload.py`.

## Data safety during development

Use fictional/synthetic data only.

Never commit:

- real health records;
- real encrypted backups/documents containing personal data;
- PINs or backup passwords;
- cryptographic keys;
- production signing certificates/keystores/private keys;
- access tokens or service secrets.

Do not put private health information in Gumroad/Buy Me a Coffee notes or other third-party fields.

## Where data lives conceptually

CareNest currently uses local device storage:

- structured records: SQLite;
- imported document payloads: separately encrypted application-owned storage;
- secret/key material: platform secure storage where applicable;
- backups: manually created password-encrypted files;
- exports/shares: user-controlled copies that leave the CareNest-owned boundary.

The project does not claim whole-database encryption.

## Reminder model in one minute

A reminder crosses three state surfaces:

1. explicit user schedule intent;
2. persisted CareNest occurrence state;
3. operating-system notification/alarm state.

Because database and OS scheduling cannot be committed atomically, CareNest uses reconciliation and compensation logic. Do not simplify this into a single `scheduled=true` flag model.

See `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`.

## Before opening a pull request

- follow `CONTRIBUTING.md`;
- preserve the local-first and medical-safety boundaries;
- preserve the repository-storefront versus app-package boundary;
- add tests for behavior changes;
- update documentation in the same change;
- run the quality gate;
- run affected platform builds when applicable;
- run unsuppressed dependency audit for package changes;
- never mark manual/device/store evidence complete without performing it.

## Before release work

Start with:

- `PROJECT_STATUS.md`;
- `what_changed.md`;
- `docs/releases/NEXT_STEPS.md`;
- `docs/releases/RELEASE_PROCESS.md`;
- `docs/releases/RELEASE_CHECKLIST.md`;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`;
- `docs/releases/MANUAL_TEST_MATRIX.md`.

## Troubleshooting

Use `docs/setup/TROUBLESHOOTING.md` for environment, workloads, build/test and platform issues.

For a reminder issue, inspect explicit schedule/profile/medicine state, time zone, snooze effective due time, permissions/capabilities, persisted occurrence state, platform request reconciliation, OS restrictions and startup recovery—in that order.

## Complete documentation map

Use `docs/DOCUMENTATION_CATALOG.md` for the entire documentation suite.
