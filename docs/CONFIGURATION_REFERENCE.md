# CareNest Configuration, Build, and Automation Reference

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`  
**Verified PR #74 head:** `8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

This document is the current canonical reference for repository configuration affecting restore, build, testing, dependency security, MAUI targets, XAML compilation, local preflight, CI, store-candidate verification, inspection artifacts and release evidence.

## 1. Central package management

`Directory.Packages.props` enables:

```xml
<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
<CentralPackageTransitivePinningEnabled>true</CentralPackageTransitivePinningEnabled>
```

Current centrally managed versions:

| Package | Version | Purpose |
|---|---:|---|
| `Microsoft.Maui.Controls` | `10.0.20` | MAUI application/UI runtime |
| `sqlite-net-pcl` | `1.9.172` | SQLite application API |
| `SQLitePCLRaw.bundle_green` | `2.1.11` | SQLite bundle path |
| `SQLitePCLRaw.lib.e_sqlite3` | `3.53.3` | maintained native SQLite leaf |
| `SQLitePCLRaw.lib.e_sqlite3.android` | `2.1.12` | Android SQLite native leaf |
| `SQLitePCLRaw.provider.e_sqlite3` | `2.1.12` | SQLite provider |
| `SQLitePCLRaw.provider.sqlite3` | `2.1.12` | SQLite provider |
| `SQLitePCLRaw.provider.dynamic_cdecl` | `2.1.12` | SQLite provider |
| `Microsoft.Extensions.Logging.Debug` | `10.0.0` | debug logging provider |
| `Microsoft.Extensions.Logging.Abstractions` | `10.0.0` | logging abstractions |
| `Microsoft.Extensions.DependencyInjection.Abstractions` | `10.0.0` | DI abstractions |
| `Microsoft.NET.Test.Sdk` | `17.14.1` | test host |
| `xunit` | `2.9.3` | test framework |
| `xunit.runner.visualstudio` | `3.1.4` | runner adapter |
| `coverlet.collector` | `6.0.4` | coverage collector |

Package updates are verification-relevant. Review release/security implications, run restore/build/test, unsuppressed audit, affected platform builds and compatibility validation when persistence/crypto/platform behavior can change.

## 2. Shared build properties

`Directory.Build.props` centralizes repository-wide compiler/analyzer/build behavior.

The intended model includes:

- current C# language version configured centrally;
- nullable reference types;
- implicit usings;
- .NET analyzers;
- deterministic build behavior;
- stricter CI warning handling;
- repository/author metadata.

Legitimate analyzer findings should be fixed rather than hidden with broad suppressions.

## 3. NuGet audit policy

The former exact `GHSA-2m69-gcr7-jv3q` suppression remains removed.

Current rules:

- dependency audit is blocking in configured quality/release paths;
- platform-neutral and MAUI graphs are audited in GitHub Actions;
- `SqliteDependencySecurityContractTests` protects maintained package floors and suppression absence;
- wildcard/severity-wide audit suppression is not an acceptable shortcut;
- packaged existing-database compatibility is a separate release gate from dependency-graph security.

PR #74 Dependency Audit #91 / run `31938301172` passed both configured graphs.

## 4. `NuGet.config`

`NuGet.config` controls repository package sources/restore behavior. Package-source, credential or signature-policy changes are security-sensitive.

Never commit package-feed credentials.

## 5. Solution graph

`CareNest.sln` contains:

```text
src/CareNest.Shared/CareNest.Shared.csproj
src/CareNest.Domain/CareNest.Domain.csproj
src/CareNest.Application/CareNest.Application.csproj
src/CareNest.Infrastructure/CareNest.Infrastructure.csproj
src/CareNest.App/CareNest.App.csproj

tests/CareNest.UnitTests/CareNest.UnitTests.csproj
tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj
tests/CareNest.UiTests/CareNest.UiTests.csproj
```

Intended dependency direction:

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

## 6. MAUI target selection

The application is multi-targeted. `CareNestTargetFramework` narrows evaluation to one target on a host/CI job:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f <tfm> -c Release \
  -p:CareNestTargetFramework=<tfm>
```

This avoids requiring unrelated workloads on a target-specific runner and prevents app TFMs from leaking into platform-neutral references.

## 7. Current application project metadata

`src/CareNest.App/CareNest.App.csproj` declares:

- `UseMaui=true`;
- `SingleProject=true`;
- application title `CareNest`;
- application ID `com.sanskar.carenest`;
- display version `1.0.0-rc.1`;
- application version `1`;
- Windows package type `None` in the project baseline.

## 8. Target frameworks and minimum platforms

- Android: `net10.0-android`; minimum Android API 24.
- iOS: `net10.0-ios`; minimum iOS 15.
- Mac Catalyst: `net10.0-maccatalyst`; minimum 15.
- Windows: `net10.0-windows10.0.19041.0`; minimum/target platform 10.0.19041.0.

The project file is the source of truth if these values change.

## 9. Strict XAML compiled-binding policy

The application project enables:

```xml
<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
<WarningsAsErrors>$(WarningsAsErrors);XC0022;XC0023;XC0024;XC0025</WarningsAsErrors>
```

Required conventions:

- binding-bearing pages have accurate root `x:DataType`;
- binding-bearing DataTemplates have item-specific `x:DataType`;
- picker display bindings are typed when context changes to an item;
- explicit Source/RelativeSource bindings include source type information;
- template-to-parent commands use typed ancestor binding contexts;
- no matching `NoWarn`, `x:Object` or `x:Null` bypass is part of the intended policy.

`CompiledBindingContractTests` protects this dynamically.

## 10. Application resources

The MAUI project includes:

- app icon resources under `Resources/AppIcon/`;
- splash resources under `Resources/Splash/`;
- images under `Resources/Images/`;
- raw assets under `Resources/Raw/`.

The distributed application source/package intentionally contains no external Buy Me a Coffee destination/card/command/artwork.

Repository funding documentation/metadata remains separate.

## 11. Core development commands

```bash
dotnet restore CareNest.sln

dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release

dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

## 12. Formatting

Representative verification commands:

```bash
dotnet format src/CareNest.Shared/CareNest.Shared.csproj --verify-no-changes
dotnet format src/CareNest.Domain/CareNest.Domain.csproj --verify-no-changes
dotnet format src/CareNest.Application/CareNest.Application.csproj --verify-no-changes
dotnet format src/CareNest.Infrastructure/CareNest.Infrastructure.csproj --verify-no-changes
dotnet format tests/CareNest.UnitTests/CareNest.UnitTests.csproj --verify-no-changes
dotnet format tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj --verify-no-changes
dotnet format tests/CareNest.UiTests/CareNest.UiTests.csproj --verify-no-changes
```

Do not weaken formatter/analyzer policy to bypass a deterministic failure.

## 13. Local quality gate

Bash:

```bash
build/scripts/quality-gate.sh
```

PowerShell:

```powershell
./build/scripts/quality-gate.ps1
```

The quality gate is intended to validate formatting, platform-neutral builds/tests and blocking dependency audit from a clean checkout.

## 14. Release preflight

Bash:

```bash
build/scripts/release-preflight.sh
```

PowerShell:

```powershell
./build/scripts/release-preflight.ps1
```

When `CARENEST_TARGET` is supplied, it selects an explicit supported MAUI target for audit/build work.

The current release preflight no longer carries an application funding-link build toggle. The external funding destination is absent from application runtime/package source by product policy.

## 15. Store-package preflight

Store-package wrappers require an explicit supported target and delegate to the normal release preflight.

Bash example:

```bash
CARENEST_TARGET=net10.0-android \
./build/scripts/store-package-preflight.sh
```

PowerShell example:

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
./build/scripts/store-package-preflight.ps1
```

Supported target families are Android, iOS, Mac Catalyst and Windows as declared by the app project.

The wrapper does not sign/publish a production store package.

## 16. Repository-local Git identity

Maintainer convention:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Helpers:

```bash
build/scripts/setup-git.sh
```

```powershell
./build/scripts/setup-git.ps1
```

GitHub API/connector commits can use authenticated account/connector identity; always rely on actual commit metadata.

## 17. Android configuration

Primary platform files live under `src/CareNest.App/Platforms/Android/`, including manifest/application/activity/notification integration.

Build:

```bash
dotnet workload install maui-android

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

Manual release evidence must cover notification permission, alarm/battery/background behavior, reboot/time-zone recovery, installed identity, reminder actions, files/backups/app lock and accessibility.

## 18. Windows configuration

Primary files live under `src/CareNest.App/Platforms/Windows/`.

Build:

```powershell
dotnet workload install maui
dotnet build src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0
```

Internal self-contained inspection publishing can additionally use `RuntimeIdentifierOverride=win-x64`, `WindowsPackageType=None` and `WindowsAppSDKSelfContained=true` as configured by repository workflows.

The resulting internal inspection output is not automatically a signed Microsoft Store package.

## 19. iOS configuration

iOS code lives under `src/CareNest.App/Platforms/iOS/`.

Simulator example:

```bash
dotnet workload install maui-ios

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-ios -c Release \
  -p:CareNestTargetFramework=net10.0-ios \
  -p:RuntimeIdentifier=iossimulator-arm64
```

Production device signing/provisioning belongs outside Git.

## 20. Mac Catalyst configuration

Mac Catalyst code lives under `src/CareNest.App/Platforms/MacCatalyst/`.

```bash
dotnet workload install maui-maccatalyst

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst
```

Internal unsigned inspection output is not signed/notarized production evidence.

## 21. GitHub workflow roles

### CareNest CI

`.github/workflows/ci.yml` verifies:

- platform-neutral formatting;
- all three core test projects;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release.

### CodeQL

`.github/workflows/codeql.yml` performs C# security analysis for configured events.

### Dependency Audit

`.github/workflows/dependency-review.yml` audits platform-neutral and MAUI dependency graphs with event-safe behavior.

### Store Package Configuration

`.github/workflows/store-package-verification.yml` builds store-candidate configurations for all four targets.

The current workflow does not use a funding-link build-property fork because the application package is funding-surface-free by source policy.

### Store Inspection Artifacts

`.github/workflows/store-inspection-artifacts.yml`:

- records exact source SHA/ref;
- runs a fail-closed forbidden-marker scanner self-test;
- creates an unsigned/internal Android AAB inspection artifact;
- creates a self-contained Windows inspection artifact;
- creates iOS simulator and unsigned Mac Catalyst inspection output;
- scans/stages payloads;
- records checksums/provenance;
- uploads internal evidence artifacts;
- does not inject production signing secrets.

Inspection artifacts are not store-ready production packages.

### Release Gate

`.github/workflows/release-gate.yml` is a fail-closed production release gate.

### Release Evidence

`.github/workflows/release-evidence.yml` records exact candidate provenance, test/evidence manifests, dependency information and checksums.

## 22. Production tag behavior

Production-style tags matching `v*` are configured to participate in:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

A tag is not automatically production approval. Manual/package/accessibility/signing/store evidence must also be complete.

## 23. Repository support files

`.github/` includes:

- funding metadata for voluntary repository support;
- Dependabot configuration;
- issue templates;
- pull request templates;
- Actions workflows.

Funding metadata is repository-level and must not be interpreted as an in-app health entitlement.

## 24. Environment/secrets policy

Never commit:

- Android production keystores/private keys;
- Apple signing private keys/certificates/provisioning secrets;
- Windows signing private keys;
- CI/service credentials;
- production `.env` secrets;
- app-lock PINs;
- backup passwords;
- encryption keys;
- real CareNest databases/backups/documents.

Use synthetic/fictional data in public artifacts and documentation examples.

## 25. Build reproducibility/provenance

Production evidence should resolve every signed artifact to the exact approved source SHA/tag and record package checksum/signing provenance.

Internal inspection artifacts record exact source identity separately from event/PR merge identity where relevant.

## 26. Configuration change checklist

When changing project/workflow/package/build configuration:

1. identify affected source/platform/test/release surfaces;
2. update documentation in the same change;
3. run formatting and all core tests;
4. run unsuppressed dependency audit when restore/dependencies can change;
5. run all affected normal platform builds;
6. run store-candidate and inspection workflows when packaging policy can change;
7. run CodeQL when relevant;
8. perform packaged compatibility checks for persistence/crypto changes;
9. create fresh exact-source verification before calling a changed executable source authoritative.

Do not weaken a workflow/contract simply to obtain green status.

## 27. Current verified baseline

PR #74 frozen source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Evidence:

- CareNest CI #735 / run `31938301209`: success;
- formatting: success;
- unit: 122/122;
- integration: 39/39;
- UI/source-policy: 170/170;
- total: 331/331;
- Android/Windows/iOS simulator/Mac Catalyst Release builds: success;
- Store Package Configuration #124 / `31938301146`: success;
- Store Inspection Artifacts #47 / `31938301275`: success;
- CodeQL #735 / `31938301252`: success;
- Dependency Audit #91 / `31938301172`: success.

Permanent evidence: `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

Older PR #61/#59/#58/#56/#54 records remain historical evidence for their own frozen source boundaries.