# CareNest Development Setup

This is the primary development setup for CareNest `1.0.0-rc.1`. For platform-specific details, also read `docs/setup/PLATFORM_SETUP.md`.

## Repository

```bash
git clone https://github.com/sanskarIN/CareNest.git
cd CareNest
```

Default branch: `main`.

## Required toolchain

Core requirements:

- Git;
- .NET 10 SDK;
- .NET MAUI workload(s) for target platforms being built;
- NuGet restore access;
- platform SDK/tooling for each intended target.

Platform tooling:

- Android: Android SDK/JDK + MAUI Android workload;
- iOS/Mac Catalyst: compatible macOS + Xcode + Apple MAUI workload;
- Windows: Windows App SDK/MAUI prerequisites on a supported Windows development host.

Use the exact project target frameworks/toolchain expected by the current branch if a later release changes them.

## Inspect the environment

```bash
git --version
dotnet --info
dotnet workload list
```

For Apple hosts also record:

```bash
xcodebuild -version
```

## Git maintainer identity

Requested repository-local identity:

```bash
git config --local user.email "sanskarin@outlook.in"
git config --local user.name "Sanskar"
```

Repository helper scripts:

```bash
build/scripts/setup-git.sh
```

PowerShell:

```powershell
./build/scripts/setup-git.ps1
```

Both helper scripts locate the repository root, use `--local`, fail on native Git errors, and verify the configured name/email.

GitHub web/API/connector commits can use the authenticated GitHub account identity rather than the local repository identity; do not misrepresent those commits as having an arbitrary local email.

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

See `docs/architecture/ARCHITECTURE.md` for responsibilities/dependency direction.

## Install MAUI workloads

Full MAUI workload when the machine is intended to build every platform available on that operating system:

```bash
dotnet workload install maui
```

Narrow host examples:

```bash
dotnet workload install maui-android
dotnet workload install maui-ios
dotnet workload install maui-maccatalyst
```

Install only supported workloads for the host OS.

## Restore strategy

Start with platform-neutral projects:

```bash
dotnet restore src/CareNest.Shared/CareNest.Shared.csproj
dotnet restore src/CareNest.Domain/CareNest.Domain.csproj
dotnet restore src/CareNest.Application/CareNest.Application.csproj
dotnet restore src/CareNest.Infrastructure/CareNest.Infrastructure.csproj
```

The full solution can be restored on a fully provisioned host:

```bash
dotnet restore CareNest.sln
```

On a target-limited host, prefer the target-specific MAUI commands below rather than forcing unrelated workloads.

## Platform-neutral build

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
```

This is the fastest way to separate shared source issues from platform workload/toolchain issues.

## Why `CareNestTargetFramework` is required

CareNest.App is multi-targeted. Supplying a global `TargetFrameworks` value can leak the app target into referenced `net10.0` projects.

The repository uses `CareNestTargetFramework` to narrow only the MAUI app before restore/build.

Pattern:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f <tfm> -c Release \
  -p:CareNestTargetFramework=<tfm>
```

## Android build

Install:

```bash
dotnet workload install maui-android
```

Build:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android \
  -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

## Windows build

Target framework:

`net10.0-windows10.0.19041.0`

On the current CI-backed setup, install the supported MAUI workload on the Windows host:

```powershell
dotnet workload install maui
```

Build:

```powershell
dotnet build src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0
```

## iOS simulator build

Install:

```bash
dotnet workload install maui-ios
```

Build example for Apple-silicon simulator:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-ios \
  -c Release \
  -p:CareNestTargetFramework=net10.0-ios \
  -p:RuntimeIdentifier=iossimulator-arm64
```

Use a RID compatible with your host/simulator.

## Mac Catalyst build

Install:

```bash
dotnet workload install maui-maccatalyst
```

Build:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst \
  -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst
```

## Apple Xcode compatibility

The GitHub-hosted Apple CI uses a macOS 26 runner compatible with the current .NET 10 Apple workload.

If local build reports an Xcode-version mismatch:

- inspect installed workload version;
- inspect selected Xcode version;
- install/select a supported Xcode version;
- do not suppress/bypass the workload compatibility check as a release strategy.

## Run tests

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

### Current verification lineage

The authoritative completed 2026-08-14 bug-audit baseline was PR #54:

- UnitTests: 122;
- IntegrationTests: 39;
- UiTests: 100;
- total: 261;
- all four platform Release builds, CodeQL and unsuppressed Dependency Audit passed.

Release-engineering hardening after PR #54 added workflow/script contracts. Superseded PR #55 already demonstrated:

- UnitTests: 122 passed;
- IntegrationTests: 39 passed;
- UiTests: 116 passed;
- total: 277 passed;
- Android/Windows builds, CodeQL and unsuppressed Dependency Audit passed before later confirmed release-tooling/documentation fixes required a newer exact-source verification.

The current `main` head must complete a fresh full marker-only verification before it becomes the next authoritative baseline.

See `docs/testing/TESTING_GUIDE.md`.

## Local quality gate

Bash:

```bash
build/scripts/quality-gate.sh
```

PowerShell:

```powershell
./build/scripts/quality-gate.ps1
```

Both scripts are intended to work from a clean checkout and:

- verify platform-neutral/test project formatting;
- build platform-neutral source projects;
- restore/run all three core test projects;
- run blocking unsuppressed NuGet audit for the test dependency graphs;
- fail on required native-command errors.

They do not replace the MAUI platform matrix.

## Formatting

CI verifies platform-neutral formatting project by project.

Examples:

```bash
dotnet format src/CareNest.Domain/CareNest.Domain.csproj --verify-no-changes
dotnet format tests/CareNest.UnitTests/CareNest.UnitTests.csproj --verify-no-changes
```

For a full local preflight use the repository release-preflight script when the host is provisioned.

## Release preflight

Bash:

```bash
build/scripts/release-preflight.sh
```

PowerShell:

```powershell
./build/scripts/release-preflight.ps1
```

Preflight treats the unsuppressed platform-neutral/test dependency audit as blocking. If `CARENEST_TARGET` is set, the selected MAUI target is audited before the target Release build.

See the script and `docs/releases/RELEASE_PROCESS.md` for target configuration and expectations.

## Running/debugging the MAUI app

Use an IDE or `dotnet` command appropriate to the target platform.

When debugging reminder behavior:

- use synthetic medicine/profile data;
- confirm selected schedule time zone;
- confirm notification permission/capability;
- distinguish persisted occurrence state from the operating-system scheduled request;
- remember snoozed `SnoozedUntilUtc` is the effective due time;
- verify cancellation-first handled actions before assuming a database state change means the OS request was removed;
- do not assume OS delivery from planner occurrence generation alone;
- use developer diagnostics/reminder rebuild tools where available.

## Local data during development

Development installs can create local SQLite/encrypted document/app-lock state.

Do not copy real user health data into development fixtures.

Use fictional/synthetic records for tests/screenshots/reproduction.

## Dependency management

Package versions are centrally managed in `Directory.Packages.props` where applicable.

Run unsuppressed dependency audit after package changes.

### SQLite native/provider path

The formerly tracked `GHSA-2m69-gcr7-jv3q` source exception has been remediated in the current RC1 graph.

Current intent includes:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- central transitive pinning;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- Android native/provider leaves and selected providers at `2.1.12`;
- no old advisory `NuGetAuditSuppress` entry.

The package floor/suppression absence is protected by `SqliteDependencySecurityContractTests`.

Before changing the SQLite provider/bundle/native chain, follow:

`docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

Do not restore the old audit suppression merely because packaged existing-database/encrypted-data compatibility evidence is incomplete. Those manual checks are a separate production-release gate.

## Analyzer policy

CI promotes applicable analyzer findings to build failures.

Historical exact-head verification intentionally exposed real analyzer defects rather than broadly suppressing them.

If an analyzer fails:

1. understand the finding;
2. fix source when legitimate;
3. scope advisory-only exceptions narrowly when the rule is non-correctness guidance;
4. never hide security/correctness issues with blanket suppression.

## Release workflow behavior

Production tags matching `v*` run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Release Evidence records tracked-source provenance/checksums, test TRX files, dependency inventories, workspace integrity and evidence checksums. It retains available evidence on a failed run and applies an aggregate failure gate after the upload.

Workflow/test/build-script changes are verification-relevant source and require a new exact-head verification before they are used as a production baseline.

## Architecture rules for contributors

- UI/ViewModels do not issue SQL directly.
- Platform-neutral projects do not reference MAUI.
- Runtime source does not add network/telemetry clients casually in local-first v1.
- Reminder planner remains platform-neutral/deterministic.
- Reminder platform request state is reconciled explicitly with persisted occurrence state.
- Secrets remain outside normal settings/database where secure secret storage is required.
- Medicine strength/instruction text remains opaque.
- No diagnosis/treatment/dosage/interaction/risk-scoring feature is introduced.

Architecture contracts enforce many of these rules.

## Documentation

Start at:

`docs/README.md`

When behavior, verification, dependency state, or release tooling changes, update the relevant user/architecture/security/testing/release documents in the same work.

## Do not commit

Never commit:

- signing keys/certificates/keystores;
- `.p12` / `.pfx` private signing files;
- API/service credentials;
- passwords;
- app-lock PINs;
- exported CareNest backups;
- real user health documents;
- real SQLite user databases;
- decrypted temporary health documents;
- secret `.env` files.

Repository policy tests detect common secret/signing patterns but do not replace human review.

## Troubleshooting

See `docs/setup/TROUBLESHOOTING.md`.

Useful first commands:

```bash
dotnet --info
dotnet workload list
dotnet workload repair
git status
```

If platform-neutral tests pass while one platform fails, investigate that platform workload/SDK/project source rather than assuming shared application logic is broken.

## Exact-head verification

Runtime/test/project/workflow/package/platform/release-script changes that need a new verified baseline follow the marker-only protocol in:

`docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

Verification markers must not be merged into `main`.
