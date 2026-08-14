# CareNest Configuration, Build, and Automation Reference

This document is the canonical reference for repository configuration that affects restore, compilation, testing, dependency security, local preflight, CI, release evidence, and platform builds.

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

## 12. Repository-local Git identity

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

## 13. Android configuration

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

Android manual release validation additionally covers notification permission, alarm capability, battery optimization, reboot, time/time-zone changes, and vendor/background behavior.

## 14. Windows configuration

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

## 15. iOS configuration

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

Production signing/provisioning belongs outside Git.

## 16. Mac Catalyst configuration

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

## 17. App resources and branding

Branding/application resources are under `src/CareNest.App/Resources/`.

Important assets include app icon foreground/background SVGs, CareNest marks, and voluntary-support artwork. Resource filenames, build actions, dark/light usage, accessibility contrast, and store export requirements are documented in `docs/design/STORE_ASSETS.md` and `docs/design/DESIGN_SYSTEM.md`.

## 18. GitHub workflows

### `ci.yml`

Responsibilities:

- platform-neutral formatting;
- unit/integration/UI-contract tests;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build.

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

## 19. Production tag behavior

Tags matching `v*` are intended to run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

A created tag is not by itself production approval.

## 20. GitHub repository support files

`.github/` also includes:

- `FUNDING.yml` — voluntary project-support metadata;
- `dependabot.yml` — dependency update automation configuration;
- issue templates;
- pull request template.

Dependabot proposals are inputs for review, not automatic proof that a dependency update is compatible with CareNest persistence/crypto/platform behavior.

## 21. Environment/secrets policy

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

## 22. Build reproducibility/provenance

Repository builds are deterministic where supported by the .NET build configuration. CI sets `ContinuousIntegrationBuild` through `Directory.Build.props`.

Production evidence must record the exact source commit/tag. Signed package provenance should resolve to that exact approved source.

## 23. Configuration change checklist

When changing project/workflow/package/build configuration:

1. identify affected source/platform/test/release surfaces;
2. update the relevant documentation in the same work;
3. run formatting and all three core test projects;
4. run unsuppressed dependency audit when package/restore graph can change;
5. run all affected MAUI Release builds;
6. run CodeQL when source/workflow changes affect the verified baseline;
7. perform required packaged compatibility checks for persistence/crypto changes;
8. create fresh exact-head verification before using the new source as a release baseline.

## 24. Current verification baseline

Authoritative release-engineering verification: PR #56.

- frozen source/base: `4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`
- marker head: `e3bc621cea05364a69abee0dadbd71a67c17bddb`
- CI #571 / `31770929379`: success
- 122 unit + 39 integration + 124 UI-contract/policy = 285/285
- Android/Windows/iOS simulator/Mac Catalyst Release builds: success
- CodeQL #571 / `31770929382`: success
- unsuppressed Dependency Audit #41 / `31770929383`: success

See `docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md` for the exact evidence record.
