# CareNest Cross-Platform Setup Guide

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

This guide covers platform-specific prerequisites and validation for Android, Windows, iOS/iPadOS and Mac Catalyst. Always verify the SDK/workload versions installed on the actual development host.

## 1. Common prerequisites

All hosts need:

- Git;
- .NET 10 SDK compatible with the repository;
- NuGet restore access;
- repository clone;
- MAUI workload/toolchain for the selected target.

Check:

```bash
git --version
dotnet --info
dotnet workload list
```

Apple host:

```bash
xcodebuild -version
```

## 2. Clone

```bash
git clone https://github.com/sanskarIN/CareNest.git
cd CareNest
```

## 3. Maintainer identity

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Or use `build/scripts/setup-git.sh` / `setup-git.ps1`.

## 4. Validate platform-neutral source first

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release

dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

Local repository gate:

```bash
build/scripts/quality-gate.sh
```

or PowerShell equivalent.

## 5. Target isolation

Use:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f <tfm> -c Release \
  -p:CareNestTargetFramework=<tfm>
```

Current targets:

- Android `net10.0-android` — minimum API 24;
- iOS `net10.0-ios` — minimum iOS 15;
- Mac Catalyst `net10.0-maccatalyst` — minimum 15;
- Windows `net10.0-windows10.0.19041.0` — minimum 10.0.19041.0.

## Android

### Host/tooling

Use a supported host with Android SDK/JDK and MAUI Android workload.

```bash
dotnet workload install maui-android
```

### Build

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

### Audit MAUI graph

```bash
dotnet restore src/CareNest.App/CareNest.App.csproj \
  -p:CareNestTargetFramework=net10.0-android \
  -p:NuGetAudit=true \
  -p:NuGetAuditMode=all
```

### Manual Android validation

Before production validate representative targets for:

- fresh install/onboarding;
- notification permission denied/granted;
- actual reminder delivery;
- exact/inexact alarm behavior;
- battery optimization/vendor background behavior;
- force-stop limitation messaging;
- reboot/restart recovery;
- clock/time-zone/DST changes;
- reminder create/edit/delete;
- Taken/Skipped/Delayed/Missed action ordering;
- snooze cancellation/replacement;
- schedule/medicine/profile stale-request cleanup;
- file/document picker/share;
- backup/restore;
- packaged SQLite compatibility;
- app lock;
- accessibility.

CareNest does not guarantee notification delivery under every OS state.

## Windows

### Host/tooling

Use a supported Windows MAUI development host.

```powershell
dotnet workload install maui
```

### Build

```powershell
dotnet build src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0
```

### Manual Windows validation

Validate:

- install/startup/navigation;
- core CRUD;
- running-app notifications;
- closed-app limitation behavior;
- same-ID timer replacement/cancellation;
- reminder actions/snooze;
- restart/recovery;
- document/file picker/share;
- backup/restore;
- packaged SQLite compatibility;
- app lock;
- keyboard/focus;
- light/dark/system theme;
- accessibility.

The current Windows reminder fallback has in-process limitations and must not be presented as guaranteed closed-app delivery.

## iOS / iPadOS

### Host/tooling

Requires compatible macOS/Xcode/.NET Apple workload.

```bash
dotnet workload install maui-ios
```

### Simulator build

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-ios -c Release \
  -p:CareNestTargetFramework=net10.0-ios \
  -p:RuntimeIdentifier=iossimulator-arm64
```

Use a simulator RID compatible with the host architecture.

### Device/store signing

Production device/App Store work requires signing/provisioning outside Git.

Never commit private keys, certificate passwords or provisioning secrets.

### Manual iOS/iPadOS validation

Use real devices for:

- notification permission denied/granted;
- actual local notification delivery;
- reminder actions/snooze;
- stale request reconciliation;
- restart/lifecycle recovery;
- time-zone/DST changes;
- files/share;
- backup/restore;
- packaged SQLite compatibility;
- app lock;
- Dynamic Type;
- VoiceOver;
- theme/contrast;
- notification preview privacy.

Simulator compilation is not real-device notification evidence.

## Mac Catalyst

### Host/tooling

Requires compatible macOS/Xcode/.NET Mac Catalyst workload.

```bash
dotnet workload install maui-maccatalyst
```

### Build

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst
```

### Manual Mac validation

Validate:

- install/execution;
- notification permission/delivery;
- reminder actions/reconciliation;
- restart/lifecycle;
- file operations;
- backup/restore;
- packaged SQLite compatibility;
- app lock;
- keyboard/focus;
- theme/contrast;
- VoiceOver/accessibility;
- signed/notarized behavior when available.

## Strict XAML build behavior

Every target builds with the current strict XAML policy:

- Source binding compilation enabled;
- strict XAML compilation enabled;
- `XC0022`–`XC0025` as errors.

A target build that exposes a binding type error should be fixed in XAML/source rather than suppressed.

## Release preflight

Bash:

```bash
build/scripts/release-preflight.sh
```

PowerShell:

```powershell
./build/scripts/release-preflight.ps1
```

With target selection:

```bash
CARENEST_TARGET=net10.0-android build/scripts/release-preflight.sh
```

or PowerShell environment equivalent.

The current release build does not use an application funding-link toggle. The external BMC destination is absent from application package source by policy.

## Store-package preflight

Use:

```bash
CARENEST_TARGET=net10.0-android \
./build/scripts/store-package-preflight.sh
```

or the PowerShell wrapper with an explicit supported target.

The wrapper does not configure production signing or publish to a store.

## Internal inspection artifacts

The Store Inspection Artifacts workflow creates internal evidence for Android, Windows and Apple targets, runs the fail-closed payload scanner, records checksums/provenance and avoids production signing secrets.

These artifacts are not production/store-ready packages.

## Exact production tags

Production-style tags matching `v*` are configured to participate in:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

A successful local build or tag creation does not replace manual/device/package/signing/store evidence.

## Signing secrets

Never commit:

- Android keystores/private keys;
- Apple `.p12`/private key/provisioning secrets;
- Windows signing private keys/certificates containing private material;
- production service credentials;
- `.env` secrets.

## SQLite dependency note

The former tracked source dependency exception is remediated and the exact audit suppression is removed.

Current graph intent includes sqlite-net-pcl `1.9.172`, bundle_green `2.1.11`, native `lib.e_sqlite3` `3.53.3`, and Android/provider leaves `2.1.12` where pinned.

Do not change SQLite native/provider/bundle versions without full audit/test/platform/migration/packaged compatibility review.

## Troubleshooting sequence

1. capture `dotnet --info`;
2. capture `dotnet workload list`;
3. identify exact TFM;
4. run platform-neutral tests separately;
5. run dependency audit;
6. isolate restore versus compile versus platform toolchain failure;
7. compare local toolchain to CI;
8. use `docs/setup/TROUBLESHOOTING.md`.

## Current verified platform baseline

PR #74 passed Android, Windows, iOS simulator and Mac Catalyst normal Release builds plus all four store-candidate configurations and Android/Windows/Apple inspection workflows.

Real-device/manual production rows remain open. See `docs/PLATFORM_BEHAVIOR_MATRIX.md` and `docs/releases/NEXT_STEPS.md`.