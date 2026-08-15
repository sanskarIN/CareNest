# CareNest Development Setup

This is the primary development setup for CareNest `1.0.0-rc.1`. For the full system overview see `docs/COMPLETE_PROJECT_DOCUMENTATION.md`; for package/build/workflow details see `docs/CONFIGURATION_REFERENCE.md`; for platform specifics see `docs/setup/PLATFORM_SETUP.md`.

## Repository

```bash
git clone https://github.com/sanskarIN/CareNest.git
cd CareNest
```

Default branch: `main`.

## Required toolchain

Core:

- Git;
- .NET 10 SDK;
- NuGet restore access;
- .NET MAUI workload(s) for target platforms being built;
- platform SDK/tooling for each selected target.

Platform tooling:

- Android — Android SDK/JDK + MAUI Android workload;
- Windows — supported Windows development host + MAUI prerequisites;
- iOS/Mac Catalyst — compatible macOS + Xcode + Apple MAUI workload.

Use the exact target frameworks/project configuration in the current branch if a later release changes the toolchain.

## Inspect the environment

```bash
git --version
dotnet --info
dotnet workload list
```

On Apple hosts:

```bash
xcodebuild -version
```

## Repository-local Git identity

Requested local maintainer identity:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Repository helpers:

```bash
build/scripts/setup-git.sh
```

PowerShell:

```powershell
./build/scripts/setup-git.ps1
```

Both helpers locate the repository root, require a valid Git work tree, use `--local`, fail on native Git errors, and verify the configured name/email.

GitHub web/API/connector commits can use the authenticated GitHub account identity. Do not claim they used an arbitrary local email unless their commit metadata proves it.

## Solution structure

```text
src/
  CareNest.Shared/
  CareNest.Domain/
  CareNest.Application/
  CareNest.Infrastructure/
  CareNest.App/

tests/
  CareNest.UnitTests/
  CareNest.IntegrationTests/
  CareNest.UiTests/
```

Detailed source mapping: `docs/CODEBASE_REFERENCE.md`.

## Intended dependency direction

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

Platform-neutral projects must not depend on MAUI. ViewModels must not issue SQL directly. Local-first v1 runtime code must not casually introduce HTTP/telemetry clients.

## Install MAUI workloads

Full MAUI workload on a host intended to build every supported target available on that OS:

```bash
dotnet workload install maui
```

Narrow host examples:

```bash
dotnet workload install maui-android
dotnet workload install maui-ios
dotnet workload install maui-maccatalyst
```

Install only workloads supported by the host OS.

## Restore strategy

Start with platform-neutral projects:

```bash
dotnet restore src/CareNest.Shared/CareNest.Shared.csproj
dotnet restore src/CareNest.Domain/CareNest.Domain.csproj
dotnet restore src/CareNest.Application/CareNest.Application.csproj
dotnet restore src/CareNest.Infrastructure/CareNest.Infrastructure.csproj
```

A fully provisioned host can restore the solution:

```bash
dotnet restore CareNest.sln
```

On a target-limited host, use the target-specific MAUI commands rather than forcing unrelated workloads.

## Platform-neutral build

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
```

This is the fastest way to separate shared source problems from platform workload/toolchain problems.

## `CareNestTargetFramework`

`CareNest.App` is multi-targeted. Supplying a broad global TFM override can leak app target values into referenced platform-neutral projects or force unrelated workloads.

The repository uses `CareNestTargetFramework` to narrow the MAUI app before restore/build.

Pattern:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f <tfm> -c Release \
  -p:CareNestTargetFramework=<tfm>
```

## `CareNestShowFundingLink`

`CareNest.App` also defines a build property controlling the optional external project-support card.

Default:

`CareNestShowFundingLink=true`

Store-safe source configuration:

`CareNestShowFundingLink=false`

The false configuration hides the complete in-app Buy Me a Coffee support card. It must not alter health-organizer behavior, medical-safety wording, reminders, data, documents, encryption, backups, reports, appointments, or permissions.

The current 2026-08-15 conservative Apple/Google store decision uses the false configuration for initial store candidates unless submission-time policy clearly permits the external link.

## Android build

```bash
dotnet workload install maui-android

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android \
  -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

Store-safe Android source build:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android \
  -c Release \
  -p:CareNestTargetFramework=net10.0-android \
  -p:CareNestShowFundingLink=false
```

## Windows build

Current target framework:

`net10.0-windows10.0.19041.0`

```powershell
dotnet workload install maui

dotnet build src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0
```

## iOS simulator build

```bash
dotnet workload install maui-ios

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-ios \
  -c Release \
  -p:CareNestTargetFramework=net10.0-ios \
  -p:RuntimeIdentifier=iossimulator-arm64
```

Use a simulator RID compatible with the host/toolchain.

The store-safe CI path uses the same simulator runtime and additionally sets `CareNestShowFundingLink=false`; it does not configure production signing.

## Mac Catalyst build

```bash
dotnet workload install maui-maccatalyst

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst \
  -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst
```

## Apple Xcode compatibility

The GitHub-hosted Apple CI uses a macOS runner/toolchain compatible with the current .NET 10 Apple workloads.

If local build reports an Xcode-version mismatch:

- inspect installed .NET workload version;
- inspect selected Xcode version;
- install/select a supported Xcode version;
- do not disable/bypass the workload compatibility check as a production strategy.

## Run tests

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

## Current authoritative automated baseline

Marker-only PR #59 verifies the current executable/project/test/workflow/build-script source:

- source/base SHA: `8489d19734d6142054156d5b57f2713195c16b65`;
- marker head: `ca58294fb7f7a56ee87da16d938f0f691c3a3c7e`;
- CareNest CI #622 / `31869214132`: success;
- UnitTests: **122 passed**;
- IntegrationTests: **39 passed**;
- UiTests/source-policy: **149 passed**;
- total core: **310 passed**;
- default Android Release: success;
- default Windows Release: success;
- default iOS simulator Release: success;
- default Mac Catalyst Release: success;
- Store Package Configuration #11 / `31869214047`: success;
- funding-disabled Android/Windows/iOS simulator/Mac Catalyst Release builds: success;
- Bash store-package preflight executable-mode guard: success;
- CodeQL #622 / `31869214042`: success;
- unsuppressed Dependency Audit #44 / `31869214093`: success.

PR #59 was closed without merge; its marker is not part of `main`.

PR #58 remains historical package/store-policy hardening evidence, PR #56 remains historical release-engineering evidence, and PR #54 remains the historical runtime bug-audit baseline.

See `docs/releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md`, `docs/releases/STORE_BUILD_POLICY.md`, and `docs/testing/TESTING_GUIDE.md`.

## Local quality gate

Bash:

```bash
build/scripts/quality-gate.sh
```

PowerShell:

```powershell
./build/scripts/quality-gate.ps1
```

The quality-gate scripts are intended to work from a clean checkout and:

- verify platform-neutral/test formatting;
- build platform-neutral source projects;
- restore/run all three core test projects;
- run blocking unsuppressed NuGet audit;
- fail on required native-command errors.

They do not replace the multi-platform MAUI build matrix.

## Formatting

Representative commands:

```bash
dotnet format src/CareNest.Shared/CareNest.Shared.csproj --verify-no-changes
dotnet format src/CareNest.Domain/CareNest.Domain.csproj --verify-no-changes
dotnet format src/CareNest.Application/CareNest.Application.csproj --verify-no-changes
dotnet format src/CareNest.Infrastructure/CareNest.Infrastructure.csproj --verify-no-changes
dotnet format tests/CareNest.UnitTests/CareNest.UnitTests.csproj --verify-no-changes
dotnet format tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj --verify-no-changes
dotnet format tests/CareNest.UiTests/CareNest.UiTests.csproj --verify-no-changes
```

CI treats applicable analyzer findings as blocking. Fix legitimate findings instead of adding blanket suppressions.

## Release preflight

Bash:

```bash
build/scripts/release-preflight.sh
```

PowerShell:

```powershell
./build/scripts/release-preflight.ps1
```

Preflight treats unsuppressed dependency audit as blocking.

When `CARENEST_TARGET` is set, the selected MAUI target is audited before the optional target Release build.

General release preflight accepts `CARENEST_SHOW_FUNDING_LINK=true|false` and fails closed for another value.

See `docs/CONFIGURATION_REFERENCE.md` and `docs/releases/RELEASE_PROCESS.md`.

## Fail-closed store-package preflight

For store candidates where the external support surface must be hidden, prefer the dedicated wrappers.

Bash:

```bash
CARENEST_TARGET=net10.0-android \
./build/scripts/store-package-preflight.sh
```

PowerShell:

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
./build/scripts/store-package-preflight.ps1
```

The wrappers:

- require an explicit supported target;
- allow Android, iOS, Mac Catalyst and Windows target framework values only;
- force `CARENEST_SHOW_FUNDING_LINK=false` after reading caller environment;
- delegate the standard release preflight rather than duplicating its checks.

The Bash wrapper is tracked executable and the Store Package Configuration workflow runs `test -x build/scripts/store-package-preflight.sh`.

These wrappers do not sign, publish or prove installed package behavior.

## Central package management

Package versions are managed in `Directory.Packages.props` with central transitive pinning enabled.

Current key versions include:

- `Microsoft.Maui.Controls` `10.0.20`;
- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- selected SQLitePCLRaw providers `2.1.12`;
- xUnit/test tooling listed in `docs/CONFIGURATION_REFERENCE.md`.

## SQLite dependency security

The former exact `GHSA-2m69-gcr7-jv3q` source exception is remediated.

Current rules:

- do not restore the former advisory suppression;
- run unsuppressed dependency audit after package changes;
- preserve maintained native/provider floor;
- use `SqliteDependencySecurityContractTests` as an executable package-policy guard;
- treat packaged existing-data compatibility as separate from source dependency security.

Before changing SQLite package/provider/native dependencies, follow `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

## Reminder development rules

When debugging or changing reminder behavior:

- use synthetic data;
- confirm profile/medicine/schedule ownership;
- confirm explicit schedule time zone;
- distinguish `ScheduledUtc` from snooze effective due time;
- remember `SnoozedUntilUtc` is effective due time for a valid snooze;
- distinguish persisted occurrence state from the OS scheduled request;
- confirm notification permission/capability;
- preserve cancellation-before-replacement/suppression/invalidation;
- preserve cancellation-first handled action ordering;
- preserve non-cancelled compensation/recovery where cross-surface consistency needs it;
- do not infer dosage or clinical intent from medicine text;
- do not assume OS delivery because planner materialization succeeded.

See `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` and `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`.

## Appointment development rules

- `Appointment.StartsUtc` must be actual `DateTimeKind.Utc`;
- local/unspecified values are rejected rather than relabeled;
- notification denial is not successful scheduling;
- background rebuild does not repeatedly prompt;
- DB/platform scheduling uses compensation because they are separate surfaces.

## Document/backup development rules

- do not silently create an unrelated document key when existing ciphertext depends on a missing/corrupt key;
- preserve authenticated encryption;
- preserve supported legacy v1 read compatibility unless a tested migration/recovery plan replaces it;
- validate backup topology before extraction;
- clean app-owned temporary plaintext/staging best effort after failure;
- use synthetic fixtures only;
- update security/threat/compatibility docs when format/key behavior changes.

## Local development data

Development installs can create local SQLite, encrypted document, backup, app-lock and settings state.

Never use or commit real health data as development fixtures.

Use fictional/synthetic profiles, medicine names, documents, backup archives and screenshots.

## Logging/privacy rules

Normal sensitive-path logs must not include:

- user health notes/medicine instructions;
- document/backup contents;
- passwords/PINs/keys;
- raw sensitive exception messages/stack traces;
- identifiers that are not necessary for safe diagnosis.

See `docs/security/LOGGING_PRIVACY.md`.

## Architecture rules

- UI/ViewModels do not issue SQL directly.
- Platform-neutral projects do not depend on MAUI.
- Runtime local-first v1 does not casually add networking/telemetry.
- Reminder planner remains deterministic/platform-neutral.
- OS scheduled-request state is reconciled explicitly with persisted state.
- Secure material stays in secure-storage abstractions where required.
- Medicine strength/instruction text remains opaque.
- No diagnosis/treatment/dosage/interaction/risk-scoring feature is introduced.

Architecture and repository policy tests enforce many of these rules.

## Documentation rule

When implementation, package, workflow, platform, security or release behavior changes, update the relevant documentation in the same work.

Primary current references:

- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`;
- `docs/CODEBASE_REFERENCE.md`;
- `docs/CONFIGURATION_REFERENCE.md`;
- `docs/MAINTENANCE_AND_OPERATIONS.md`;
- `docs/releases/STORE_BUILD_POLICY.md`;
- `docs/releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md`;
- `docs/README.md`.

## Do not commit

Never commit:

- signing keys/certificates/keystores;
- `.p12` / `.pfx` private signing files;
- API/service credentials;
- passwords/PINs;
- encryption keys;
- exported CareNest backups;
- real health documents;
- real SQLite user databases;
- decrypted temporary health files;
- production secret `.env` files.

Repository policy tests detect common patterns but do not replace human review.

## Troubleshooting

See `docs/setup/TROUBLESHOOTING.md`.

Useful first commands:

```bash
dotnet --info
dotnet workload list
git status
```

If platform-neutral tests pass while one platform build fails, investigate that platform’s workload/SDK/project configuration before assuming shared application logic is broken.

## Exact-head verification

Runtime/test/project/workflow/package/platform/build-script changes that need a new release baseline follow `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

Verification marker files must not be merged into `main`.

Documentation-only evidence updates after a verified source boundary do not change the executable source that passed, but any later verification-relevant source change requires a fresh marker-only exact-head verification.

## Production release boundary

A green automated matrix is necessary but not sufficient for public release.

Real remaining release evidence includes device/platform tests, notification delivery/recovery, packaged SQLite/encrypted-data compatibility, accessibility, submission-time store policy/disclosures, actual store-safe package UI inspection, signing, signed artifact inspection, and exact production-tag CareNest CI/CodeQL/Dependency Audit/Store Package Configuration/Release Gate/Release Evidence.

See `PROJECT_STATUS.md`, `docs/releases/RELEASE_CHECKLIST.md`, and `docs/releases/NEXT_STEPS.md`.