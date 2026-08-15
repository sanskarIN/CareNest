# CareNest Configuration, Build, and Automation Reference

This document is the canonical reference for repository configuration that affects restore, compilation, testing, dependency security, local preflight, CI, release evidence, store-safe source compilation, and platform builds.

## 1. Central package management

CareNest uses `Directory.Packages.props` with:

```xml
<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
<CentralPackageTransitivePinningEnabled>true</CentralPackageTransitivePinningEnabled>
```

Current centrally managed versions:

| Package | Version | Purpose / note |
|---|---:|---|
| `Microsoft.Maui.Controls` | `10.0.20` | Current verified MAUI UI/runtime package baseline. A newer version must be treated as verification-relevant. |
| `sqlite-net-pcl` | `1.9.172` | Application SQLite API path. |
| `SQLitePCLRaw.bundle_green` | `2.1.11` | Compatible bundle API path retained by the app. |
| `SQLitePCLRaw.lib.e_sqlite3` | `3.53.3` | Maintained native SQLite leaf selected by central transitive pinning. |
| `SQLitePCLRaw.lib.e_sqlite3.android` | `2.1.12` | Android native SQLite leaf. |
| `SQLitePCLRaw.provider.e_sqlite3` | `2.1.12` | SQLite provider leaf. |
| `SQLitePCLRaw.provider.sqlite3` | `2.1.12` | SQLite provider leaf. |
| `SQLitePCLRaw.provider.dynamic_cdecl` | `2.1.12` | SQLite provider leaf. |
| `Microsoft.Extensions.Logging.Debug` | `10.0.0` | Debug logging provider. |
| `Microsoft.Extensions.Logging.Abstractions` | `10.0.0` | Platform-neutral logging contracts. |
| `Microsoft.Extensions.DependencyInjection.Abstractions` | `10.0.0` | DI abstractions. |
| `Microsoft.NET.Test.Sdk` | `17.14.1` | .NET test host/SDK. |
| `xunit` | `2.9.3` | Test framework. |
| `xunit.runner.visualstudio` | `3.1.4` | Test runner adapter. |
| `coverlet.collector` | `6.0.4` | Coverage collector. |

Package updates must be deliberate. Do not merge a package bump only because Dependabot opened it; restore/build/test, platform builds, CodeQL/dependency audit, relevant compatibility checks, and exact-head verification must be considered first.

## 2. Shared build properties

`Directory.Build.props` defines repository-wide build behavior:

- `LangVersion=latest`;
- nullable reference types enabled;
- implicit usings enabled;
- local warnings are not globally promoted to errors;
- when `CI=true`, warnings are promoted to errors;
- `AnalysisLevel=latest-recommended`;
- .NET analyzers enabled;
- deterministic builds enabled;
- `ContinuousIntegrationBuild=true` in CI;
- repository metadata points to `https://github.com/sanskarIN/CareNest`;
- author/company/copyright metadata is centralized.

Analyzer failures exposed by verification are fixed in source when legitimate rather than hidden with blanket suppressions.

## 3. NuGet audit policy

The old exact `GHSA-2m69-gcr7-jv3q` `NuGetAuditSuppress` entry was removed after establishing the maintained SQLite native/provider path.

Current release rules:

- NuGet audit is unsuppressed for that former advisory;
- release-preflight and quality-gate scripts treat audit failures as blocking;
- Dependency Audit runs in GitHub Actions;
- `SqliteDependencySecurityContractTests` prevents restoration of the old package floor/suppression;
- wildcard/severity-wide audit suppression is not acceptable as a shortcut;
- a new exact temporary exception, if ever unavoidable, requires explicit risk-register/release review and must not be described as remediation.

PR #59 Dependency Audit #44 / run `31869214093` passed both the platform-neutral graph and Android MAUI application graph for the current frozen source.

## 4. `NuGet.config`

`NuGet.config` is the repository package-source configuration. Changes to sources, package signature behavior, credentials, or restore policy are security-sensitive and verification-relevant.

Never commit package-feed credentials to the repository.

## 5. Solution

`CareNest.sln` includes the five source projects and three test projects:

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

## 6. MAUI target selection

`CareNest.App` is multi-targeted. The repository uses the custom `CareNestTargetFramework` property when a host needs to narrow the app to one target before restore/build.

Pattern:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f <tfm> -c Release \
  -p:CareNestTargetFramework=<tfm>
```

This avoids forcing unrelated platform workloads on a target-specific runner and prevents an app TFM from leaking into referenced platform-neutral `net10.0` projects.

The project also defines:

`CareNestShowFundingLink`

Default:

`true`

When `true`, `CARENEST_FUNDING_LINK` is defined and the voluntary About-page support card is visible. When `false`, that compile symbol is absent and the About support card is hidden through `AboutViewModel.IsProjectSupportVisible`.

This property changes the voluntary external support surface only. It must not alter health-organizer data, reminders, permissions, documents, reports, backups, encryption, app lock, appointments, or medical-safety behavior.

## 7. Platform target frameworks

Current target families:

- Android: `net10.0-android`
- Windows: `net10.0-windows10.0.19041.0`
- iOS: `net10.0-ios`
- Mac Catalyst: `net10.0-maccatalyst`

Use the exact project file as the source of truth if target framework versions change later.

## 8. Core development commands

Restore/build platform-neutral source:

```bash
dotnet restore src/CareNest.Shared/CareNest.Shared.csproj
dotnet restore src/CareNest.Domain/CareNest.Domain.csproj
dotnet restore src/CareNest.Application/CareNest.Application.csproj
dotnet restore src/CareNest.Infrastructure/CareNest.Infrastructure.csproj

dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
```

Run tests:

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

## 9. Formatting

CI verifies formatting project by project. Representative commands:

```bash
dotnet format src/CareNest.Shared/CareNest.Shared.csproj --verify-no-changes
dotnet format src/CareNest.Domain/CareNest.Domain.csproj --verify-no-changes
dotnet format src/CareNest.Application/CareNest.Application.csproj --verify-no-changes
dotnet format src/CareNest.Infrastructure/CareNest.Infrastructure.csproj --verify-no-changes
dotnet format tests/CareNest.UnitTests/CareNest.UnitTests.csproj --verify-no-changes
dotnet format tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj --verify-no-changes
dotnet format tests/CareNest.UiTests/CareNest.UiTests.csproj --verify-no-changes
```

Do not work around a deterministic formatter/analyzer failure by weakening the quality gate.

## 10. Local quality gate

Bash:

```bash
build/scripts/quality-gate.sh
```

PowerShell:

```powershell
./build/scripts/quality-gate.ps1
```

The local quality gate is intended to operate from a clean checkout and performs the repository-defined platform-neutral formatting/build/test/audit sequence. PowerShell checks native command exit codes explicitly.

## 11. Release preflight

Bash:

```bash
build/scripts/release-preflight.sh
```

PowerShell:

```powershell
./build/scripts/release-preflight.ps1
```

Preflight treats unsuppressed dependency audit as blocking. When a supported `CARENEST_TARGET` is supplied, the selected MAUI app target is audited before its Release build.

`CARENEST_TARGET` is a build/preflight selector, not a user preference. Use a TFM supported by the app and current host workload.

General release preflight accepts:

`CARENEST_SHOW_FUNDING_LINK=true|false`

and propagates it into `CareNestShowFundingLink`. Any other value fails closed.

## 12. Fail-closed store-package preflight

For store candidates that must hide the external support surface, use the dedicated wrappers instead of relying on a caller-provided funding-link value.

Supported target allow-list:

- `net10.0-android`;
- `net10.0-ios`;
- `net10.0-maccatalyst`;
- `net10.0-windows10.0.19041.0`.

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

Both wrappers:

- require `CARENEST_TARGET`;
- reject unsupported target values;
- force `CARENEST_SHOW_FUNDING_LINK=false` after reading the caller environment;
- delegate the standard release preflight so formatting, core builds, tests, unsuppressed audit, target restore and target Release build use one underlying implementation.

The Bash wrapper is tracked with executable Git mode `100755`. `.github/workflows/store-package-verification.yml` runs `test -x build/scripts/store-package-preflight.sh` so executable-bit loss becomes a CI failure.

These wrappers do not configure signing, publish packages, or prove installed-artifact behavior.

## 13. Repository-local Git identity

Requested local maintainer identity:

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

Both scripts locate the repository root, require a Git work tree, use `--local`, verify the configured values, and fail on native Git errors.

GitHub web/API/connector commits can use authenticated GitHub account metadata; do not falsely claim a connector commit used an arbitrary local email unless actual commit metadata proves it.

## 14. Android configuration

Important Android configuration lives under:

- `src/CareNest.App/Platforms/Android/AndroidManifest.xml`
- `MainActivity.cs`
- `MainApplication.cs`
- `PlatformNotificationService.Android.cs`
- Android resources under the platform directory.

Build example:

```bash
dotnet workload install maui-android
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

Store-safe source compile example:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android -c Release \
  -p:CareNestTargetFramework=net10.0-android \
  -p:CareNestShowFundingLink=false
```

Android manual release validation additionally covers notification permission, alarm capability, battery optimization, reboot, time/time-zone changes, vendor/background behavior, installed package identity, and actual About-page store-policy behavior.

## 15. Windows configuration

Important Windows configuration lives under:

- `src/CareNest.App/Platforms/Windows/App.xaml`
- `App.xaml.cs`
- `Package.appxmanifest`
- `PlatformNotificationService.Windows.cs`

Build example:

```powershell
dotnet workload install maui
dotnet build src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0
```

The Windows reminder fallback has documented in-process limitations; a compile is not proof of closed-app notification delivery.

## 16. iOS configuration

Important iOS configuration lives under:

- `src/CareNest.App/Platforms/iOS/AppDelegate.cs`
- `Info.plist`
- `PlatformNotificationService.iOS.cs`
- `Program.cs`

Simulator example:

```bash
dotnet workload install maui-ios
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-ios -c Release \
  -p:CareNestTargetFramework=net10.0-ios \
  -p:RuntimeIdentifier=iossimulator-arm64
```

The store-safe CI path uses the same simulator runtime while also setting `CareNestShowFundingLink=false`.

Production signing/provisioning belongs outside Git.

## 17. Mac Catalyst configuration

Important configuration lives under:

- `src/CareNest.App/Platforms/MacCatalyst/AppDelegate.cs`
- `Info.plist`
- `PlatformNotificationService.MacCatalyst.cs`
- `Program.cs`

Build example:

```bash
dotnet workload install maui-maccatalyst
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst
```

Production signing/notarization remains external release work.

## 18. App resources and branding

Branding/application resources are under `src/CareNest.App/Resources/`.

Important assets include app icon foreground/background SVGs, CareNest marks, and voluntary-support artwork. Resource filenames, build actions, dark/light usage, accessibility contrast, and store export requirements are documented in `docs/design/STORE_ASSETS.md` and `docs/design/DESIGN_SYSTEM.md`.

## 19. GitHub workflows

### `ci.yml`

Responsibilities:

- platform-neutral formatting;
- unit/integration/UI-contract tests;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build.

This workflow exercises the normal/default application configuration unless a project default changes.

### `store-package-verification.yml`

Responsibilities:

- runs on pull requests to `main`;
- runs on pushes to `main` and `release/**`;
- runs on exact `v*` tags;
- supports manual `workflow_dispatch`;
- sets `CARENEST_STORE_FUNDING_LINK=false`;
- passes that value to `CareNestShowFundingLink`;
- verifies the Bash store-package wrapper remains executable;
- compiles Android Release with the external funding surface disabled;
- compiles Windows Release with the external funding surface disabled;
- compiles iOS simulator Release with the external funding surface disabled;
- compiles Mac Catalyst Release with the external funding surface disabled;
- does not upload unsigned binaries;
- does not run `dotnet publish`;
- does not configure signing credentials.

PR #59 Store Package Configuration #11 / run `31869214047` passed all four funding-disabled target builds.

### `codeql.yml`

Runs CodeQL security analysis for supported repository events, including the production tag path defined by the workflow.

### `dependency-review.yml`

Runs NuGet dependency audits. Pull-request-only dependency comparison logic is guarded so tag/manual runs do not dereference PR-only metadata.

### `release-gate.yml`

Fail-closed production gate that checks required status/security/evidence files, dependency risk state, unchecked release checklist rows, and core tests.

### `release-evidence.yml`

Captures exact release-candidate provenance and evidence, including:

- commit/ref/run identity;
- run attempt;
- toolchain data;
- tracked-file manifest/checksums;
- unit/integration/UI TRX;
- dependency inventories;
- workspace integrity;
- evidence checksums.

Available evidence is uploaded before the aggregate failure step so failed runs remain diagnosable.

## 20. Production tag behavior

Tags matching `v*` are intended to run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- CareNest Store Package Configuration;
- Release Gate;
- CareNest Release Evidence.

A created tag is not by itself production approval.

The store-package workflow adds funding-disabled source compilation evidence to the tagged automated matrix. It does not replace signed artifact generation, installed package inspection, accessibility, device behavior, or store approval.

## 21. GitHub repository support files

`.github/` also includes:

- `FUNDING.yml` — voluntary project-support metadata;
- `dependabot.yml` — dependency update automation configuration;
- issue templates;
- pull request template.

Dependabot proposals are inputs for review, not automatic proof that a dependency update is compatible with CareNest persistence/crypto/platform behavior.

## 22. Environment/secrets policy

Never commit:

- Android keystores/private signing keys;
- Apple signing certificates/private keys/provisioning secrets;
- Windows signing private keys;
- API/service credentials;
- production `.env` secrets;
- app-lock PINs;
- backup passwords;
- document encryption keys;
- real CareNest user databases/backups/documents.

Use platform/store secret management and protected CI variables when release signing is configured.

## 23. Build reproducibility/provenance

Repository builds are deterministic where supported by the .NET build configuration. CI sets `ContinuousIntegrationBuild` through `Directory.Build.props`.

Production evidence must record the exact source commit/tag. Signed package provenance should resolve to that exact approved source.

For store candidates, evidence must also record the selected `CareNestShowFundingLink` value, actual package checksum where directly handled, signing/notarization provenance, and installed About-page inspection result.

## 24. Configuration change checklist

When changing project/workflow/package/build configuration:

1. identify affected source/platform/test/release surfaces;
2. update the relevant documentation in the same work;
3. run formatting and all three core test projects;
4. run unsuppressed dependency audit when package/restore graph can change;
5. run all affected normal MAUI Release builds;
6. when store-policy configuration can be affected, run all affected funding-disabled store-safe Release builds;
7. run CodeQL when source/workflow changes affect the verified baseline;
8. perform required packaged compatibility checks for persistence/crypto changes;
9. create fresh exact-head verification before using the new source as a release baseline.

Do not weaken a workflow or contract merely to obtain a green result.

## 25. Current verification baseline

Authoritative exact automated verification: PR #59.

- frozen source/base: `8489d19734d6142054156d5b57f2713195c16b65`
- marker head: `ca58294fb7f7a56ee87da16d938f0f691c3a3c7e`
- CareNest CI #622 / `31869214132`: success
- 122 unit + 39 integration + 149 UI-contract/policy = 310/310
- default Android/Windows/iOS simulator/Mac Catalyst Release builds: success
- Store Package Configuration #11 / `31869214047`: success
- funding-disabled Android/Windows/iOS simulator/Mac Catalyst Release builds: success
- Bash store-package preflight executable-mode guard: success
- CodeQL #622 / `31869214042`: success
- unsuppressed Dependency Audit #44 / `31869214093`: success

PR #59 was marker-only and closed without merge.

See `docs/releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md` for the exact evidence record.

Historical exact-source evidence remains available in the PR #58, PR #56, and PR #54 dated verification documents and must not be rewritten as though those older boundaries were the current one.