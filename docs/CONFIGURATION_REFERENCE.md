# CareNest Configuration, Build, and Automation Reference

**Release line:** `1.0.0-rc.1`  
**Documentation baseline:** 2026-08-17  
**Latest fully verified pre-Gumroad source:** `7cbe5568b6cffa06c279b29f3cb1b107ea988791`  
**Gumroad:** `https://ramsandesh.gumroad.com`

The complete configuration reference that was active before the Gumroad rollout is preserved exactly at:

`docs/history/pre-gumroad-rollout-20260817/CONFIGURATION_REFERENCE.md`

This document is the current canonical reference for repository configuration affecting restore, build, testing, dependency security, MAUI targets, XAML compilation, local preflight, external-commerce package isolation, CI, store-candidate verification, inspection artifacts and release evidence.

## 1. Central package management

`Directory.Packages.props` enables central package management and central transitive pinning.

Important documented versions include:

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

Package changes are verification-relevant. Run restore/build/test, blocking dependency audit and affected platform builds, then review packaged compatibility when persistence/crypto/native behavior can change.

## 2. Shared build properties

`Directory.Build.props` centralizes compiler/analyzer/repository-wide build behavior.

Intended rules include:

- nullable reference types;
- implicit usings;
- current configured C# language version;
- .NET analyzers;
- deterministic builds;
- stricter CI warning handling;
- repository/author metadata.

Legitimate analyzer findings should be fixed instead of broadly suppressed.

## 3. NuGet audit policy

The former exact `GHSA-2m69-gcr7-jv3q` suppression remains removed.

Current rules:

- dependency audit is blocking in configured quality/release paths;
- platform-neutral and MAUI graphs are audited in Actions;
- SQLite security contracts protect maintained package floors and suppression absence;
- wildcard/severity-wide suppression is not an acceptable shortcut;
- dependency security does not replace packaged existing-data compatibility testing.

## 4. NuGet configuration

`NuGet.config` controls repository package-source/restore behavior.

Package-source, credential or signature-policy changes are security-sensitive. Never commit feed credentials or private access tokens.

## 5. Solution graph

`CareNest.sln` includes:

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

The application is multi-targeted. `CareNestTargetFramework` narrows evaluation to one target:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f <tfm> -c Release \
  -p:CareNestTargetFramework=<tfm>
```

This avoids requiring unrelated workloads on target-specific hosts/runners.

## 7. Application metadata

`src/CareNest.App/CareNest.App.csproj` currently declares:

- `UseMaui=true`;
- `SingleProject=true`;
- title `CareNest`;
- application ID `com.sanskar.carenest`;
- display version `1.0.0-rc.1`;
- application version `1`;
- Windows package type `None` in the source baseline.

## 8. Target frameworks and minimums

- Android: `net10.0-android`; minimum Android API 24.
- iOS: `net10.0-ios`; minimum iOS 15.
- Mac Catalyst: `net10.0-maccatalyst`; minimum 15.
- Windows: `net10.0-windows10.0.19041.0`; minimum/target 10.0.19041.0.

The project file remains the source of truth if these values change.

## 9. Strict XAML compiled-binding policy

The app project enables:

```xml
<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
<WarningsAsErrors>$(WarningsAsErrors);XC0022;XC0023;XC0024;XC0025</WarningsAsErrors>
```

Required conventions:

- accurate root `x:DataType` on binding-bearing pages;
- item-specific `x:DataType` on binding-bearing templates;
- typed picker display bindings when context changes;
- typed Source/RelativeSource bindings;
- typed ancestor binding-context patterns for template parent commands;
- no intended matching `NoWarn`, `x:Object` or `x:Null` bypass.

## 10. Application resources

The MAUI app owns runtime resources under `src/CareNest.App/Resources/`, including app icon, splash, images and raw assets.

Repository marketing assets are **not** application resources under the current policy.

Do not copy:

`docs/assets/gumroad_store_badge.svg`

into `src/CareNest.App/Resources/Images/`.

## 11. Repository-only external-commerce configuration

Current canonical repository destinations:

- Gumroad: `https://ramsandesh.gumroad.com`;
- Buy Me a Coffee: `https://buymeacoffee.com/sanskarIN`.

They may appear in:

- `README.md`;
- `SUPPORT.md`;
- `.github/FUNDING.yml`;
- `GUMROAD.md`;
- documentation/marketing files;
- repository-only promotional artwork.

They must not appear in the CareNest runtime/package under the current product/store boundary.

There is no intended application funding/storefront build toggle. Runtime absence is source policy, not a per-store boolean.

## 12. Store-safe payload scanner

Script:

`build/scripts/verify-store-safe-payload.py`

Default forbidden markers:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

The scanner:

- accepts a file, directory or ZIP-compatible package;
- scans UTF-8 marker bytes;
- scans UTF-16 LE marker bytes;
- scans UTF-16 BE marker bytes;
- inspects ZIP/AAB entries;
- supports repeatable `--forbidden` markers;
- fails when a marker is found;
- fails closed when inspection cannot be performed.

This scanner is a package-boundary control; it does not replace source-policy tests or store submission review.

## 13. Commercial-link source contracts

`tests/CareNest.UiTests/FundingLinkContractTests.cs` protects repository placement, no-health-entitlement language, About/runtime absence and Gumroad badge accessibility/package placement.

`tests/CareNest.UiTests/StoreFundingPayloadContractTests.cs` protects application-runtime absence, shared-constant absence, scanner marker/encoding/ZIP behavior and fail-closed semantics.

Do not weaken these tests merely to put repository marketing into the application.

## 14. Core development commands

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

## 15. Formatting

CI uses project-specific `dotnet format ... --verify-no-changes` checks for platform-neutral projects and test projects.

Do not weaken formatter/analyzer rules to bypass deterministic failures.

## 16. Local quality gate

Bash:

```bash
build/scripts/quality-gate.sh
```

PowerShell:

```powershell
./build/scripts/quality-gate.ps1
```

The quality gate is intended to validate formatting, platform-neutral build/tests and blocking dependency audit from a clean checkout.

## 17. Release preflight

Bash:

```bash
build/scripts/release-preflight.sh
```

PowerShell:

```powershell
./build/scripts/release-preflight.ps1
```

When `CARENEST_TARGET` is supplied, it selects an explicit supported target.

The preflight does not carry an application Gumroad/funding build toggle because external-commerce destinations are absent from app source by current product policy.

## 18. Store-package preflight

Store wrappers require an explicit supported target and delegate to the normal release preflight.

Android example:

```bash
CARENEST_TARGET=net10.0-android \
./build/scripts/store-package-preflight.sh
```

Windows PowerShell example:

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
./build/scripts/store-package-preflight.ps1
```

The wrapper does not create a production-signed store package by itself.

## 19. Repository-local Git identity

Maintainer convention:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Helpers:

- `build/scripts/setup-git.sh`;
- `build/scripts/setup-git.ps1`.

Always rely on actual Git commit metadata when connector/API commits are used.

## 20. Android configuration

Primary platform files live under `src/CareNest.App/Platforms/Android/`.

Example:

```bash
dotnet workload install maui-android

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android \
  -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

Manual evidence still needs notification permission, alarm/battery/background behavior, reboot/time-zone recovery, reminder actions, app-lock/files/backups and accessibility.

## 21. Windows configuration

Primary files live under `src/CareNest.App/Platforms/Windows/`.

Example:

```powershell
dotnet workload install maui
dotnet build src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0
```

Internal self-contained inspection output is not automatically a Microsoft Store package.

## 22. iOS configuration

Simulator example:

```bash
dotnet workload install maui-ios

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-ios \
  -c Release \
  -p:CareNestTargetFramework=net10.0-ios \
  -p:RuntimeIdentifier=iossimulator-arm64
```

Production device signing/provisioning belongs outside Git.

## 23. Mac Catalyst configuration

Example:

```bash
dotnet workload install maui-maccatalyst

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst \
  -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst
```

Internal unsigned inspection output is not signed/notarized production evidence.

## 24. CareNest CI

`.github/workflows/ci.yml` verifies configured platform-neutral formatting/tests plus Android, Windows, iOS simulator and Mac Catalyst Release builds.

The final exact workflow result must match the source SHA being claimed as verified.

## 25. CodeQL

`.github/workflows/codeql.yml` performs C# security analysis for configured events.

A green CodeQL run applies to its exact source, not to later changes automatically.

## 26. Dependency audit

`.github/workflows/dependency-review.yml` protects dependency policy/audit behavior.

Do not turn blocking audit failures into warning-only behavior without an explicit security decision.

## 27. Store Package Configuration

`.github/workflows/store-package-verification.yml` builds store-candidate configurations for:

- Android;
- Windows;
- iOS simulator;
- Mac Catalyst.

The workflow does not rely on a funding/storefront build-property fork.

## 28. Store Inspection Artifacts

`.github/workflows/store-inspection-artifacts.yml` provides internal package inspection/provenance work on its configured triggers.

It can:

- record exact source SHA/ref;
- self-test the forbidden-marker scanner;
- create unsigned/internal platform outputs;
- scan payloads;
- record checksums/provenance;
- upload evidence artifacts.

These are not automatically production packages.

## 29. Release Gate and Release Evidence

- `.github/workflows/release-gate.yml` — fail-closed production-tag aggregate gate.
- `.github/workflows/release-evidence.yml` — exact-source test/evidence/checksum/provenance record.

Production tags must refer to the exact approved source and must not be moved to hide a failed release attempt.

## 30. Production tag behavior

Production-style `v*` tags participate in applicable CI/security/dependency/store/release workflows.

A tag does not replace real-device, signing, accessibility, package compatibility or store-policy evidence.

## 31. Repository support metadata

`.github/FUNDING.yml` currently exposes repository-only custom links for:

- Buy Me a Coffee;
- Ram Sandesh Gumroad.

This GitHub metadata is not application runtime functionality.

## 32. Secrets policy

Never commit:

- Android signing private material;
- Apple private signing/provisioning secrets;
- Windows private signing keys;
- feed/CI/service credentials;
- production `.env` secrets;
- app-lock PINs;
- backup passwords;
- encryption keys;
- real health databases/backups/documents.

Use synthetic/fictional data for public artifacts and examples.

## 33. Build reproducibility and provenance

Production evidence should resolve every signed artifact to the exact approved source SHA/tag and record package checksum/signing provenance.

Repository-only Gumroad/Buy Me a Coffee documents/assets should remain distinguishable from app package contents in release evidence.

## 34. Source-line/structured-file quality configuration

The UI/source-policy suite deterministically scans runtime C# lines for known defect patterns and parses structured runtime files such as XAML/project/XML-family/JSON files.

This complements compiler/analyzer/platform build checks with actionable file/line regression reporting.

## 35. Configuration-change checklist

When changing project/workflow/package/build/marketing configuration:

1. identify affected source/platform/test/release surfaces;
2. preserve medical/local-first/privacy boundaries;
3. review external-commerce package isolation when storefront/funding changes;
4. update documentation in the same change series;
5. update the lowest appropriate regression tests;
6. run formatting/core tests;
7. run dependency audit when restore/dependencies can change;
8. run affected normal platform builds;
9. run store-candidate/inspection workflows when packaging policy can change;
10. run CodeQL where applicable;
11. perform packaged compatibility checks for persistence/crypto changes;
12. create fresh exact-source verification before promoting a changed baseline.

## 36. Latest fully verified pre-Gumroad baseline

Exact source:

`7cbe5568b6cffa06c279b29f3cb1b107ea988791`

Verified:

- 122/122 unit tests;
- 39/39 integration tests;
- 173/173 UI/source-policy tests;
- **334/334 total core tests**;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- all four store-candidate configurations;
- CodeQL.

The Gumroad rollout changes tests and the package scanner, so a newer source is authoritative only after its exact workflow matrix is green.

## 37. Current references

Use:

- `PROJECT_STATUS.md` — active project/release status;
- `what_changed.md` — exact current continuation/commit series;
- `GUMROAD.md` — storefront guide;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — storefront/package policy;
- `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md` — complete package creation guide;
- `docs/releases/NEXT_STEPS.md` — remaining production work;
- `docs/DOCUMENTATION_CATALOG.md` — complete documentation map.
