# CareNest Cross-Platform Setup Guide

This guide complements `docs/setup/DEVELOPMENT.md` with platform-specific prerequisites and validation steps for Android, Windows, iOS, and Mac Catalyst development.

CareNest targets .NET 10 / .NET MAUI. Workload/toolchain versions can change, so always verify the currently installed .NET SDK and MAUI workloads on the development host before building.

## Common prerequisites

All development hosts need:

- Git;
- .NET 10 SDK compatible with the repository;
- access to NuGet package restore;
- repository clone;
- appropriate MAUI workload for the target being built.

Recommended first checks:

```bash
git --version
dotnet --info
dotnet workload list
dotnet restore src/CareNest.Domain/CareNest.Domain.csproj
```

The repository's Git setup scripts configure the requested repository-local maintainer identity:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Use:

- `build/scripts/setup-git.sh`, or
- `build/scripts/setup-git.ps1`.

The helper scripts locate the repository root, fail on Git errors, and verify the configured values.

## Clone

```bash
git clone https://github.com/sanskarIN/CareNest.git
cd CareNest
```

## Platform-neutral validation first

Before installing/diagnosing MAUI target workloads, validate shared projects where possible:

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release

dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

For local policy/audit checks use:

```bash
build/scripts/quality-gate.sh
```

or the PowerShell equivalent. The local quality gate includes blocking unsuppressed NuGet audit for the core test dependency graphs.

## Why `CareNestTargetFramework` exists

The MAUI application is multi-targeted. A host that only has one platform workload should not have to evaluate every unrelated target.

Use the repository's `CareNestTargetFramework` property when building a specific platform:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f <target-framework> \
  -c Release \
  -p:CareNestTargetFramework=<target-framework>
```

This keeps the platform-specific target narrow and prevents app target-framework values from leaking into referenced platform-neutral projects.

# Android

## Host

Android development can be performed on a supported Windows/macOS/Linux host with the Android/.NET MAUI workload and Android SDK tooling supported by the installed .NET SDK.

## Install workload

```bash
dotnet workload install maui-android
```

## Build

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android \
  -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

## Audit the Android app graph

```bash
dotnet restore src/CareNest.App/CareNest.App.csproj \
  -p:CareNestTargetFramework=net10.0-android \
  -p:NuGetAudit=true \
  -p:NuGetAuditMode=all
```

This is the MAUI graph audited by the repository Dependency Audit workflow.

## Android-specific manual verification

A release candidate must be tested for:

- fresh install/onboarding;
- notification permission denied;
- notification permission granted;
- future snooze whose original due time has passed;
- cancellation-first Taken/Skipped/Delayed/Missed actions;
- snooze replacement cancellation/order;
- stale OS request cleanup after schedule changes;
- medicine/profile delete reminder cleanup;
- exact/inexact alarm diagnostics;
- battery-optimization diagnostics;
- reboot reminder rebuild;
- time/time-zone change rebuild;
- force-stop/OS limitation messaging;
- file/document import/export/share;
- encrypted backup/restore;
- packaged SQLite existing-data compatibility after native/provider updates;
- app-lock cold start;
- large text/accessibility behavior.

CareNest does not claim that Android can guarantee reminder delivery under all OS/battery/force-stop states.

# Windows

## Host

Use a supported Windows development host with Visual Studio/Build Tools components required by .NET MAUI Windows development and the applicable .NET MAUI workload.

## Install workload

The current GitHub Actions Windows build installs the supported MAUI workload with:

```powershell
dotnet workload install maui
```

Use the installed .NET SDK/workload guidance if a future SDK changes the workload model. Keep local commands aligned with the repository CI that is actually proving the target.

## Build

```powershell
dotnet build src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0
```

Use the exact framework declared by the current project file if it changes in a later version.

## Windows-specific manual verification

Verify:

- fresh install/onboarding;
- window resizing;
- keyboard navigation;
- theme switching;
- document picker/share behavior;
- backup/restore;
- packaged SQLite existing-data compatibility after native/provider updates;
- app lock;
- in-process notification/fallback diagnostics;
- same-ID timer replacement/cancellation;
- cancellation-first handled reminder actions;
- snooze replacement.

The current Windows reminder path has explicit limitations and must not be described as guaranteed while the application is not running.

# iOS

## Host

iOS builds require a compatible macOS/Xcode/.NET MAUI Apple toolchain.

The GitHub-hosted CI currently uses a macOS 26 runner compatible with the .NET 10 Apple workload. A local developer must use an Xcode version supported by the installed .NET Apple workload.

## Install workload

```bash
dotnet workload install maui-ios
```

## Simulator build

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-ios \
  -c Release \
  -p:CareNestTargetFramework=net10.0-ios \
  -p:RuntimeIdentifier=iossimulator-arm64
```

Use a simulator RID compatible with the host architecture.

## Device/store signing

Production device/App Store builds require Apple signing certificates, provisioning profiles, bundle identity, and entitlements configured outside the repository.

Do not commit signing secrets/profiles.

## iOS-specific manual verification

Verify:

- notification permission denied/granted;
- local notification scheduling/delivery behavior;
- future snooze effective due time;
- cancellation-first handled reminder actions;
- snooze replacement;
- stale request reconciliation after schedule/state changes;
- app restart/rebuild behavior;
- time-zone changes;
- document picker/share;
- backup/restore;
- packaged SQLite existing-data compatibility after native/provider updates;
- app lock;
- Dynamic Type/text scaling;
- VoiceOver/semantic labels;
- light/dark/system theme.

OS notification delivery remains controlled by iOS policy.

# Mac Catalyst

## Host

Mac Catalyst requires compatible macOS, Xcode, .NET 10, and MAUI Mac Catalyst workload.

## Install workload

```bash
dotnet workload install maui-maccatalyst
```

## Build

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst \
  -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst
```

## Mac-specific manual verification

Verify:

- window resizing;
- keyboard/focus behavior;
- notifications;
- cancellation-first reminder actions;
- snooze/replacement reconciliation;
- file operations;
- backup/restore;
- packaged SQLite existing-data compatibility after native/provider updates;
- app lock;
- theme/accessibility behavior.

Production distribution may also require signing/notarization or store configuration outside Git.

# Formatting

The platform-neutral projects/test projects are subject to:

```bash
dotnet format <project> --verify-no-changes
```

CI runs platform-neutral formatting independently from MAUI platform build jobs.

# Release preflight scripts

On a fully provisioned host use:

```bash
build/scripts/release-preflight.sh
```

or PowerShell:

```powershell
./build/scripts/release-preflight.ps1
```

The preflight treats unsuppressed NuGet audit as blocking. When `CARENEST_TARGET` is set, that target is audited before the optional MAUI Release build.

Example Android target selection:

```bash
CARENEST_TARGET=net10.0-android build/scripts/release-preflight.sh
```

PowerShell:

```powershell
$env:CARENEST_TARGET = 'net10.0-android'
./build/scripts/release-preflight.ps1
```

# Exact release tags

Tags matching `v*` are configured to verify the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

A successful local platform build does not replace those exact-tag gates or the manual matrix.

# Signing secrets

Never commit:

- Android keystores;
- `.jks` signing files;
- Apple `.p12` certificates;
- private keys;
- provisioning-profile secrets;
- Windows signing certificates/private keys;
- `.env` files containing credentials;
- service-account credentials.

Repository policy tests reject common secret/signing file patterns, but maintainers remain responsible for secret hygiene.

# NuGet/SQLite dependency note

The former `GHSA-2m69-gcr7-jv3q` source exception is resolved in the current RC1 dependency graph.

Current graph intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- central transitive pinning enabled;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- Android native/provider leaves and selected providers at `2.1.12`;
- no old advisory `NuGetAuditSuppress` entry.

`SqliteDependencySecurityContractTests` protects the maintained package floor and suppression absence.

Read:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

Do not silently change SQLite provider/bundle/native versions without running the full migration/regression/platform/dependency matrix and packaged existing-data/encrypted-data compatibility checks.

Do not restore the old suppression to bypass those manual checks.

# Troubleshooting

See `docs/setup/TROUBLESHOOTING.md` for common restore/workload/platform issues.

When diagnosing:

1. capture `dotnet --info`;
2. capture `dotnet workload list`;
3. identify exact target framework;
4. reproduce platform-neutral build/tests separately;
5. run the blocking dependency audit;
6. verify Xcode/Android SDK/Windows tooling compatibility;
7. avoid posting user health data or real backups in issue logs.
