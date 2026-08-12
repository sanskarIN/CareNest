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

The repository's Git setup scripts configure the requested maintainer identity:

```bash
git config user.name "Sanskar"
git config user.email "sanskarin@outlook.in"
```

Use:

- `build/scripts/setup-git.sh`, or
- `build/scripts/setup-git.ps1`.

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

Android development can be performed on a supported Windows/macOS host with the Android/.NET MAUI workload and Android SDK tooling.

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

## Android-specific manual verification

A release candidate must be tested for:

- fresh install/onboarding;
- notification permission denied;
- notification permission granted;
- reminder registration;
- exact/inexact alarm diagnostics;
- battery-optimization diagnostics;
- reboot reminder rebuild;
- time/time-zone change rebuild;
- force-stop/OS limitation messaging;
- file/document import/export/share;
- encrypted backup/restore;
- app-lock cold start;
- large text/accessibility behavior.

CareNest does not claim that Android can guarantee reminder delivery under all OS/battery/force-stop states.

# Windows

## Host

Use a supported Windows development host with Visual Studio/Build Tools components required by .NET MAUI Windows development and the applicable .NET MAUI workload.

## Install workload

```powershell
dotnet workload install maui-windows
```

If the current .NET workload model on the host uses a different supported installation path, follow the installed SDK's workload guidance and repository CI as the source of truth for target commands.

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
- app lock;
- notification/fallback diagnostics.

The current Windows reminder path has explicit limitations and must not be described as guaranteed while the application is not running.

# iOS

## Host

iOS builds require a compatible macOS/Xcode/.NET MAUI Apple toolchain.

The GitHub-hosted CI currently uses a macOS runner compatible with the .NET 10 Apple workload. A local developer must use an Xcode version supported by the installed .NET Apple workload.

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
- app restart/rebuild behavior;
- document picker/share;
- backup/restore;
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
- file operations;
- backup/restore;
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

Target-specific build behavior is controlled by the script's documented target parameter/environment option.

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

# NuGet/SQLite advisory note

The repository currently tracks `GHSA-2m69-gcr7-jv3q` for the SQLitePCLRaw native `2.1.11` path.

The narrow `NuGetAuditSuppress` entry is not a vulnerability fix. Read:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

Do not silently change SQLite provider/bundle versions without running the migration/regression matrix.

# Troubleshooting

See `docs/setup/TROUBLESHOOTING.md` for common restore/workload/platform issues.

When diagnosing:

1. capture `dotnet --info`;
2. capture `dotnet workload list`;
3. identify exact target framework;
4. reproduce platform-neutral build/tests separately;
5. verify Xcode/Android SDK/Windows tooling compatibility;
6. avoid posting user health data or real backups in issue logs.