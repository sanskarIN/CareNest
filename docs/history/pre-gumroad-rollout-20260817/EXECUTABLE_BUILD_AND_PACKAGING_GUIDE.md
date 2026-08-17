# CareNest Executable Build and Packaging Guide

**Release line:** `1.0.0-rc.1`  
**Documentation baseline:** 2026-08-16  
**Application project:** `src/CareNest.App/CareNest.App.csproj`  
**Application ID:** `com.sanskar.carenest`  
**Target SDK:** .NET `10.0.100` with .NET MAUI 10  

This is the canonical end-to-end guide for creating runnable and distributable CareNest application artifacts. It covers the complete repository build surface, every project that contributes to or validates the executable, all four target platforms, signing boundaries, expected output locations, preflight checks, release evidence and troubleshooting.

> Important: publish the MAUI **application project**, not `CareNest.sln`. Publishing the solution can attempt to publish library and test projects that are not executable applications.

---

## 1. What this guide creates

CareNest is a single .NET MAUI application with four target frameworks:

| Platform | Target framework | Minimum platform | Primary distributable artifact |
|---|---|---:|---|
| Android | `net10.0-android` | API 24 | `.apk` and/or `.aab` |
| iOS/iPadOS | `net10.0-ios` | iOS 15 | signed `.ipa` |
| Mac Catalyst | `net10.0-maccatalyst` | Mac Catalyst 15 | `.app` or signed `.pkg` |
| Windows | `net10.0-windows10.0.19041.0` | Windows 10 build 19041 | unpackaged `.exe` + publish folder |

The current project explicitly sets:

```xml
<OutputType>Exe</OutputType>
<UseMaui>true</UseMaui>
<SingleProject>true</SingleProject>
<WindowsPackageType>None</WindowsPackageType>
```

Therefore the current Windows baseline is an **unpackaged executable**, not an MSIX installer. A future MSIX/store package is a separate packaging/signing operation and must follow the release/store documentation and a verified package identity.

---

## 2. Repository-wide file coverage

No repository area is silently skipped. Every file is covered by one of the groups below.

### 2.1 Root build and repository files

| File | Role in executable creation |
|---|---|
| `.editorconfig` | Source formatting rules. Does not ship in the app package. |
| `.gitignore` | Prevents generated outputs/secrets from being committed. Does not ship. |
| `CareNest.sln` | Aggregates all source and test projects for development/validation. Do not use it as the `dotnet publish` target. |
| `global.json` | Pins the SDK baseline to `10.0.100` with feature-band roll-forward. Affects restore/build selection. |
| `Directory.Build.props` | Applies compiler/analyzer/deterministic-build metadata to projects. Affects build behavior. |
| `Directory.Packages.props` | Central NuGet package versions, including MAUI, SQLite and tests. Affects restore and final binaries. |
| `NuGet.config` | NuGet source/configuration policy. Affects restore. |
| `README.md` | Repository documentation only. Does not ship unless explicitly copied. |
| `PROJECT_STATUS.md` | Current release/evidence boundary. Does not ship. |
| `CHANGELOG.md` | Release history. Does not ship by default. |
| `DECISIONS.md` | Engineering decisions. Does not ship. |
| `CONTRIBUTING.md`, `CODE_OF_CONDUCT.md` | Contributor governance. Does not ship. |
| `PRIVACY.md`, `TERMS.md`, `SECURITY.md`, `SUPPORT.md`, `BUY_ME_A_COFFEE.md` | Repository/user/legal/support documentation. Not app package input unless explicitly referenced by the app. |
| `LICENSE`, `NOTICE` | Repository legal files. They are not automatically embedded in the application package by this project file. |
| `what_changed.md` | Active engineering handoff. Does not ship. |

### 2.2 GitHub automation files

All files under `.github/` are repository automation/governance rather than runtime application payload. They still matter to release confidence.

Current workflow surface includes:

- `.github/workflows/ci.yml`
- `.github/workflows/codeql.yml`
- `.github/workflows/dependency-review.yml`
- `.github/workflows/release-gate.yml`
- `.github/workflows/release-evidence.yml`
- `.github/workflows/store-package-verification.yml`
- `.github/workflows/store-inspection-artifacts.yml`

Also covered:

- `.github/dependabot.yml`
- `.github/FUNDING.yml`
- `.github/ISSUE_TEMPLATE/**`
- `.github/PULL_REQUEST_TEMPLATE.md`

These files do not become part of the executable, APK, AAB, IPA, APP, PKG or Windows publish folder.

### 2.3 Build scripts

Every current file under `build/scripts/` is part of release preparation or repository tooling:

- `quality-gate.ps1`
- `quality-gate.sh`
- `release-preflight.ps1`
- `release-preflight.sh`
- `setup-git.ps1`
- `setup-git.sh`
- `store-package-preflight.ps1`
- `store-package-preflight.sh`
- `verify-store-safe-payload.py`

These scripts do not ship in the application package unless someone explicitly copies them into a package. They are used to validate the source and candidate artifacts.

### 2.4 Source projects

The solution contains five production source projects:

1. `src/CareNest.Shared/CareNest.Shared.csproj`
2. `src/CareNest.Domain/CareNest.Domain.csproj`
3. `src/CareNest.Application/CareNest.Application.csproj`
4. `src/CareNest.Infrastructure/CareNest.Infrastructure.csproj`
5. `src/CareNest.App/CareNest.App.csproj`

All normal SDK-included C# files in these projects are compiled unless a project file explicitly removes them for the active target. The application project references all four non-UI projects, so their compiled assemblies/code contribute to the final application.

Intended dependency direction:

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

### 2.5 Application XAML and C# files

All app-level `.cs` and `.xaml` files under `src/CareNest.App/` are build inputs according to .NET MAUI SDK conventions unless explicitly excluded in the project file.

Examples include:

- `App.xaml`
- `App.xaml.cs`
- `MauiProgram.cs`
- `GlobalUsings.cs`
- `Converters/**`
- `Navigation/**`
- `Services/**`
- `ViewModels/**`
- `Views/**`

The project enables strict compiled XAML bindings:

```xml
<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
<WarningsAsErrors>$(WarningsAsErrors);XC0022;XC0023;XC0024;XC0025</WarningsAsErrors>
```

A publish must fail rather than silently accepting those binding warnings.

### 2.6 Platform-specific files

The project conditionally removes platform C# source that does not belong to the active target. Therefore each publish compiles the shared source plus only the matching platform source.

#### Android

Covered files include:

- `src/CareNest.App/Platforms/Android/AndroidManifest.xml`
- `src/CareNest.App/Platforms/Android/MainActivity.cs`
- `src/CareNest.App/Platforms/Android/MainApplication.cs`
- `src/CareNest.App/Platforms/Android/PlatformNotificationService.Android.cs`
- `src/CareNest.App/Platforms/Android/Resources/**`

#### iOS

Covered files include:

- `src/CareNest.App/Platforms/iOS/AppDelegate.cs`
- `src/CareNest.App/Platforms/iOS/Program.cs`
- `src/CareNest.App/Platforms/iOS/Info.plist`
- `src/CareNest.App/Platforms/iOS/PlatformNotificationService.iOS.cs`

#### Mac Catalyst

Covered files include:

- `src/CareNest.App/Platforms/MacCatalyst/AppDelegate.cs`
- `src/CareNest.App/Platforms/MacCatalyst/Program.cs`
- `src/CareNest.App/Platforms/MacCatalyst/Info.plist`
- `src/CareNest.App/Platforms/MacCatalyst/PlatformNotificationService.MacCatalyst.cs`

#### Windows

Covered files include:

- `src/CareNest.App/Platforms/Windows/App.xaml`
- `src/CareNest.App/Platforms/Windows/App.xaml.cs`
- `src/CareNest.App/Platforms/Windows/Package.appxmanifest`
- `src/CareNest.App/Platforms/Windows/PlatformNotificationService.Windows.cs`

The current Windows manifest identifies the package/app as CareNest and sets minimum Windows `10.0.19041.0`. Because `WindowsPackageType=None`, the manifest still participates in Windows app metadata/build behavior but the canonical executable path in this guide is the unpackaged publish path.

### 2.7 MAUI resources

Every resource matched by `CareNest.App.csproj` is a package input:

```xml
<MauiIcon Include="Resources\AppIcon\appicon.svg"
          ForegroundFile="Resources\AppIcon\appiconfg.svg"
          Color="#E8F2EE" />
<MauiSplashScreen Include="Resources\Splash\splash.svg"
                  Color="#F5FAF8"
                  BaseSize="512,512" />
<MauiImage Include="Resources\Images\*" />
<MauiAsset Include="Resources\Raw\**"
           LogicalName="%(RecursiveDir)%(Filename)%(Extension)" />
```

Therefore all current files beneath these matched paths are included/processed according to MAUI resource rules:

- `Resources/AppIcon/**`
- `Resources/Splash/**`
- `Resources/Images/*`
- `Resources/Raw/**`

Do not rename/remove a resource without checking all XAML/C# references and package output.

### 2.8 Test projects

The three test projects are required release-validation inputs but are never application executables to distribute:

- `tests/CareNest.UnitTests/CareNest.UnitTests.csproj`
- `tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj`
- `tests/CareNest.UiTests/CareNest.UiTests.csproj`

All test `.cs` files under those projects are covered by the quality/release gates. Test assemblies must not be copied into production packages manually.

### 2.9 Documentation tree

Every file under `docs/**` is documentation/evidence. Documentation files are not runtime package inputs unless the project later explicitly includes them as MAUI assets or content. This guide itself belongs to that non-runtime documentation surface.

### 2.10 Generated files and folders

These are generated and should not be treated as source inputs:

- `**/bin/**`
- `**/obj/**`
- temporary publish/package folders
- local keystores/certificates/provisioning files
- test-result folders
- local IDE state

Clean them when diagnosing stale build behavior.

---

## 3. Toolchain prerequisites

### 3.1 Required .NET SDK

From the repository root:

```powershell
dotnet --version
dotnet --info
```

Expected SDK family is selected by `global.json`:

```json
{
  "sdk": {
    "version": "10.0.100",
    "rollForward": "latestFeature",
    "allowPrerelease": false
  }
}
```

If `dotnet --version` cannot resolve a compatible installed SDK, install a supported .NET 10 SDK before continuing.

### 3.2 Required MAUI workloads

Check workloads:

```powershell
dotnet workload list
```

Restore/install workloads appropriate to the host and target before publishing. A Windows machine can build Windows and Android; Apple distribution builds require access to macOS/Xcode/signing infrastructure.

### 3.3 Platform tools

#### Windows

Use Windows 10/11 with a current Visual Studio/Build Tools installation that supports .NET MAUI and the Windows App SDK dependencies used by .NET MAUI 10.

#### Android

Required components include Android SDK/build tools and a JDK compatible with the installed .NET Android workload. For signed release artifacts you also need a private Android keystore.

#### iOS / Mac Catalyst

A Mac with a compatible Xcode installation is required for Apple signing/distribution work. iOS distribution additionally requires the appropriate Apple Developer certificate and provisioning profile.

---

## 4. Start from a clean checkout

From repository root:

```powershell
git status --short
dotnet --info
```

For a clean rebuild:

```powershell
dotnet clean CareNest.sln -c Release
```

If stale artifacts are suspected, remove `bin`/`obj` directories for the affected project(s) before restoring again.

Never delete signing keys or provisioning material as part of a generic cleanup.

---

## 5. Restore dependencies

Restore the solution for general development validation:

```powershell
dotnet restore CareNest.sln
```

For a platform-isolated MAUI restore, use the repository's `CareNestTargetFramework` switch.

### Android restore

```powershell
dotnet restore src/CareNest.App/CareNest.App.csproj `
  -p:CareNestTargetFramework=net10.0-android
```

### Windows restore

```powershell
dotnet restore src/CareNest.App/CareNest.App.csproj `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0
```

### iOS restore

```bash
dotnet restore src/CareNest.App/CareNest.App.csproj \
  -p:CareNestTargetFramework=net10.0-ios
```

### Mac Catalyst restore

```bash
dotnet restore src/CareNest.App/CareNest.App.csproj \
  -p:CareNestTargetFramework=net10.0-maccatalyst
```

---

## 6. Run the release preflight before publishing

### Windows PowerShell

```powershell
./build/scripts/quality-gate.ps1
./build/scripts/release-preflight.ps1
```

Target-specific preflight example:

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
./build/scripts/release-preflight.ps1
Remove-Item Env:CARENEST_TARGET
```

Android:

```powershell
$env:CARENEST_TARGET = 'net10.0-android'
./build/scripts/release-preflight.ps1
Remove-Item Env:CARENEST_TARGET
```

### macOS/Linux shell

```bash
./build/scripts/quality-gate.sh
./build/scripts/release-preflight.sh
```

Target-specific example:

```bash
CARENEST_TARGET=net10.0-maccatalyst ./build/scripts/release-preflight.sh
```

The preflight checks formatting, source hygiene, core Release builds, automated tests, dependency audit and the optional MAUI target. It does **not** replace real-device, signing, store-policy, accessibility or packaged existing-data validation.

---

## 7. Windows executable (`.exe`)

CareNest currently sets `WindowsPackageType=None`, so the canonical Windows artifact is an unpackaged app.

### 7.1 Recommended x64 self-contained Windows publish

Run from repository root in PowerShell or a Developer Command Prompt:

```powershell
dotnet publish src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0 `
  -p:RuntimeIdentifierOverride=win-x64 `
  -p:WindowsPackageType=None `
  -p:WindowsAppSDKSelfContained=true
```

Expected publish directory:

```text
src/CareNest.App/bin/Release/net10.0-windows10.0.19041.0/win-x64/publish/
```

The directory contains `CareNest.App.exe` plus the files required by the published app.

**Distribute the complete publish folder, not only the `.exe`.** An unpackaged Windows app can depend on adjacent files/native/runtime components.

### 7.2 Framework-dependent Windows publish

```powershell
dotnet publish src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0 `
  -p:RuntimeIdentifierOverride=win-x64 `
  -p:WindowsPackageType=None
```

This can reduce output size but requires the target machine to have the required runtime/deployment dependencies.

### 7.3 Other Windows architectures

For supported machines, replace the RID with the desired portable .NET 10 RID, for example:

```text
win-x86
win-arm64
```

Validate each architecture on matching real hardware before distribution.

### 7.4 Windows signing

The unpackaged executable can be Authenticode-signed after publish as part of a production release. Certificate choice/storage, timestamping and signing policy are release-operator responsibilities. Do not place a PFX password or private key in the repository.

### 7.5 Windows MSIX note

The repository contains `Platforms/Windows/Package.appxmanifest`, but the app project currently chooses `WindowsPackageType=None`. Do not describe the current `.exe` publish as an MSIX installer. If an MSIX/store package is introduced, validate package identity, publisher, version, assets, signing certificate and install/update behavior separately.

---

## 8. Android APK and AAB

.NET MAUI Android publishing can produce APK and AAB files. APK is appropriate for direct installation/testing; AAB is the normal Google Play submission format.

### 8.1 Unsigned/internal build check

APK-only candidate:

```powershell
dotnet publish src/CareNest.App/CareNest.App.csproj `
  -f net10.0-android `
  -c Release `
  -p:CareNestTargetFramework=net10.0-android `
  -p:AndroidPackageFormats=apk
```

AAB-only candidate:

```powershell
dotnet publish src/CareNest.App/CareNest.App.csproj `
  -f net10.0-android `
  -c Release `
  -p:CareNestTargetFramework=net10.0-android `
  -p:AndroidPackageFormats=aab
```

Typical output root:

```text
src/CareNest.App/bin/Release/net10.0-android/publish/
```

### 8.2 Signed Android release

Keep the keystore outside the repository. Example with placeholders:

```powershell
dotnet publish src/CareNest.App/CareNest.App.csproj `
  -f net10.0-android `
  -c Release `
  -p:CareNestTargetFramework=net10.0-android `
  -p:AndroidPackageFormats=apk `
  -p:AndroidKeyStore=true `
  -p:AndroidSigningKeyStore="C:\secure\carenest-release.keystore" `
  -p:AndroidSigningKeyAlias="<KEY_ALIAS>" `
  -p:AndroidSigningKeyPass="file:C:\secure\android-key-pass.txt" `
  -p:AndroidSigningStorePass="file:C:\secure\android-store-pass.txt"
```

For AAB, use the same signing identity with `AndroidPackageFormats=aab`. Keep passwords out of source control and ordinary shell history/build logs. Follow the current .NET Android signing behavior for the installed SDK; password-file based secret injection is preferable to committing secrets.

### 8.3 Android identity/version

Current values come from the app project:

```text
ApplicationTitle          CareNest
ApplicationId             com.sanskar.carenest
ApplicationDisplayVersion 1.0.0-rc.1
ApplicationVersion        1
```

Increase versions deliberately before production submission. Never reuse an already-consumed store version code/build number when the store requires a monotonic version.

### 8.4 Android verification

Before distributing:

- confirm the expected package file exists;
- confirm the package is signed with the intended release identity;
- install the APK on a real supported Android device;
- run reminder/notification, storage, backup/restore and document-vault checks;
- run the repository's store-package/preflight and payload checks where applicable;
- retain hashes and release evidence for the exact artifact.

---

## 9. iOS `.ipa`

A distributable iOS package requires Apple signing/provisioning and a Mac build environment (directly or as a paired build host).

### 9.1 Signed IPA on a Mac

Use the real distribution certificate/profile names from the release operator's Apple account:

```bash
dotnet publish src/CareNest.App/CareNest.App.csproj \
  -f net10.0-ios \
  -c Release \
  -p:CareNestTargetFramework=net10.0-ios \
  -p:ArchiveOnBuild=true \
  -p:RuntimeIdentifier=ios-arm64 \
  -p:CodesignKey="Apple Distribution: <NAME> (<TEAM_ID>)" \
  -p:CodesignProvision="<PROVISIONING_PROFILE>"
```

Expected output root:

```text
src/CareNest.App/bin/Release/net10.0-ios/ios-arm64/publish/
```

The `.ipa` generated there is only production-eligible if the certificate, profile, application ID, entitlements/capabilities and distribution channel are correct.

### 9.2 Windows with a Mac build host

CareNest can be driven from Windows with Pair to Mac/remote Mac build infrastructure, but the actual Apple compilation/signing requirements still exist on the Mac host. Prefer stored SSH keys rather than putting a Mac password into build scripts or source control.

### 9.3 iOS checks

Before App Store/TestFlight/ad-hoc distribution:

- verify `ApplicationId` matches the registered App ID;
- verify the signing certificate is valid and not expired/revoked;
- verify the provisioning profile matches the bundle ID and channel;
- verify any required capabilities are represented correctly;
- test on a real device, not only a simulator;
- validate local notifications/reminders, storage, document vault and backup/restore;
- complete Apple privacy/store metadata independently of build success.

---

## 10. Mac Catalyst `.app` and `.pkg`

Mac Catalyst publishing must run on macOS with the appropriate Xcode/.NET MAUI environment.

### 10.1 Unsigned/internal `.app`

```bash
dotnet publish src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst \
  -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst \
  -p:CreatePackage=false
```

The `.app` is produced beneath the Release Mac Catalyst output directory. Architecture-specific output may add a RID directory.

### 10.2 Package (`.pkg`) candidate

A package build can be requested with:

```bash
dotnet publish src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst \
  -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst \
  -p:CreatePackage=true
```

For distribution, add the correct Apple code-signing and package-signing identities/provisioning settings. Do not invent an entitlements file path: only provide `CodesignEntitlements` if an actual repository entitlements file exists and the app's capabilities require it.

### 10.3 Architecture

Release Mac Catalyst builds can target Intel, Apple silicon or a universal configuration depending on SDK/project properties. If publishing a single architecture, use the correct RID and test on that architecture.

### 10.4 Mac verification

Validate:

- app launch from a clean location;
- signing/notarization requirements for the chosen distribution channel;
- notification permission and delivery behavior;
- local data path behavior;
- backup/restore and encrypted document compatibility;
- upgrade behavior from the previous supported build.

---

## 11. Visual Studio publishing path

CLI commands are the canonical reproducible examples in this guide, but Visual Studio can also be used.

General flow:

1. Open `CareNest.sln`.
2. Set configuration to `Release`.
3. Select only the desired platform target.
4. Ensure the correct signing identity/profile/keystore is selected for release distribution.
5. Publish **CareNest.App**, not a library or test project.
6. Record the exact source commit, SDK/workload versions, signing identity and output hash.

Visual Studio configuration must produce the same identity/version/package intent as the checked-in project and release policy.

---

## 12. Build all non-app projects before final publish

The application references the shared/domain/application/infrastructure projects, so a publish will build what it needs. However, the release preflight intentionally builds core projects separately to catch layer-specific failures.

Manual equivalent:

```powershell
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
```

Then build the active MAUI target before publish.

---

## 13. Run every automated test project

```powershell
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

A package should not be promoted when a required test suite fails.

---

## 14. Dependency audit

The release preflight performs a blocking NuGet audit. Manual examples:

```powershell
dotnet restore tests/CareNest.UnitTests/CareNest.UnitTests.csproj -p:NuGetAudit=true -p:NuGetAuditMode=all
dotnet restore tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -p:NuGetAudit=true -p:NuGetAuditMode=all
dotnet restore tests/CareNest.UiTests/CareNest.UiTests.csproj -p:NuGetAudit=true -p:NuGetAuditMode=all
```

Target-specific app audit:

```powershell
dotnet restore src/CareNest.App/CareNest.App.csproj `
  -p:CareNestTargetFramework=net10.0-android `
  -p:NuGetAudit=true `
  -p:NuGetAuditMode=all
```

Repeat for the platform being released.

---

## 15. Store-package and payload preflight

Run the appropriate repository scripts before treating a build as a store candidate:

### PowerShell

```powershell
./build/scripts/store-package-preflight.ps1
```

### Shell

```bash
./build/scripts/store-package-preflight.sh
```

The repository also contains:

```text
build/scripts/verify-store-safe-payload.py
```

Use the documented workflow/script interface rather than copying or weakening the payload checks.

---

## 16. Release artifact naming and storage

Do not overwrite the only copy of a signed production candidate. Store immutable release candidates in a release-evidence location outside the source tree or as controlled CI artifacts.

Recommended metadata to record for every candidate:

```text
Product: CareNest
Display version: 1.0.0-rc.1
Application version/build: 1
Platform: android | ios | maccatalyst | windows
Architecture/RID: ...
Source commit: ...
SDK: ...
MAUI workload version: ...
Artifact filename: ...
SHA-256: ...
Signing identity fingerprint/name: ...
Build machine/runner: ...
Build date (UTC): ...
Validation evidence: ...
```

Generate a SHA-256 hash on Windows:

```powershell
Get-FileHash -Algorithm SHA256 "<artifact-path>"
```

On macOS/Linux:

```bash
shasum -a 256 "<artifact-path>"
```

---

## 17. Do not commit release secrets

Never commit:

- Android `.keystore` / `.jks` private signing stores;
- keystore passwords;
- Apple private certificates/keys;
- `.p12`/`.pfx` private key bundles;
- provisioning secrets that should remain private;
- notarization/store API secrets;
- CI signing secret values;
- machine passwords;
- generated production packages merely as source files unless the repository explicitly adopts a binary-release policy.

Use local secure storage or CI secret stores.

---

## 18. Versioning before a production build

Current values in `CareNest.App.csproj`:

```xml
<ApplicationDisplayVersion>1.0.0-rc.1</ApplicationDisplayVersion>
<ApplicationVersion>1</ApplicationVersion>
```

Before a new release:

1. choose the intended public display version;
2. increment the platform-compatible application/build version;
3. confirm Windows package/store version mapping if packaged Windows distribution is introduced;
4. update release notes/changelog/status documentation;
5. rerun all build/test/audit/package gates;
6. build signed artifacts from the exact approved commit/tag.

Do not change version values only in a local uncommitted build if the released binary is intended to be reproducible from source.

---

## 19. Clean rebuild procedure when publish output looks wrong

1. Confirm correct branch/commit:

```powershell
git status
git rev-parse HEAD
```

2. Confirm SDK:

```powershell
dotnet --info
```

3. Clean:

```powershell
dotnet clean CareNest.sln -c Release
```

4. Remove affected `bin`/`obj` directories if necessary.
5. Restore the exact target using `CareNestTargetFramework`.
6. Run target Release build.
7. Run target publish.
8. Inspect only the newly created output directory.

Avoid mixing artifacts from different commits, target frameworks, architectures or signing identities.

---

## 20. Common failures

### `NETSDK` / target framework not supported

Cause: incompatible/missing .NET 10 SDK or workload.

Check:

```powershell
dotnet --info
dotnet workload list
```

### MAUI workload missing

Restore/install the required workload for the host/target, then restore again.

### Windows RID error

For .NET 10 use portable Windows RIDs such as `win-x64`, `win-x86`, or `win-arm64`, not obsolete version-specific RIDs.

### Android signing failure

Check:

- keystore path;
- alias;
- password source;
- keystore validity;
- package format;
- build configuration is `Release`.

Do not solve signing failures by committing a keystore or plaintext password.

### iOS provisioning failure

Check:

- bundle/application ID;
- certificate key pair;
- provisioning profile;
- profile distribution type;
- Apple account/team;
- Mac/Xcode compatibility.

### XAML `XC0022`–`XC0025`

These are intentionally errors in CareNest. Fix the binding type/source declarations; do not add blanket warning suppression.

### SQLite/native provider build failure

Restore cleanly using the checked-in central package versions. Do not casually substitute provider/native packages only to make one host build pass; packaged compatibility is part of release validation.

### App launches on build machine but fails elsewhere

For Windows unpackaged builds, confirm the entire publish folder was copied and consider `WindowsAppSDKSelfContained=true`. Test on a clean supported machine.

---

## 21. Platform-specific output summary

### Windows x64 self-contained

```text
src/CareNest.App/bin/Release/
  net10.0-windows10.0.19041.0/
    win-x64/
      publish/
        CareNest.App.exe
        ...required dependencies...
```

### Android

```text
src/CareNest.App/bin/Release/net10.0-android/publish/
  ...apk/aab outputs...
```

### iOS

```text
src/CareNest.App/bin/Release/net10.0-ios/ios-arm64/publish/
  ...ipa output...
```

### Mac Catalyst

```text
src/CareNest.App/bin/Release/net10.0-maccatalyst/
  ...app and/or publish/pkg output...
```

Exact filenames can vary with SDK packaging behavior, RID, signing and version configuration. Treat the publish/build log as the authoritative artifact path for that run.

---

## 22. Production release sequence

Use this sequence for each platform:

1. Start from the exact intended source commit.
2. Ensure working tree is clean.
3. Confirm .NET SDK/workloads.
4. Restore.
5. Run formatting/quality gate.
6. Run release preflight.
7. Run all required tests.
8. Run blocking dependency audit.
9. Build the platform in `Release`.
10. Run store-package/payload preflight where applicable.
11. Publish using the platform-specific command in this guide.
12. Apply production signing using protected credentials.
13. Inspect the produced artifact/package.
14. Install on clean real hardware/OS.
15. Validate upgrades from supported previous data/package state.
16. Validate reminders/notification permissions and delivery behavior.
17. Validate document vault and encrypted backup/restore compatibility.
18. Validate accessibility manually with real assistive technology.
19. Verify store/legal/privacy metadata for the distribution channel.
20. Record artifact SHA-256 and signing identity.
21. Preserve release evidence.
22. Run/confirm the repository release workflows for the exact production tag.
23. Only then submit/publish.

A successful `dotnet publish` means a build artifact was produced; it does not by itself prove production signing, store approval, real-device behavior, accessibility or data-upgrade compatibility.

---

## 23. Relation to the rest of the documentation

Read this guide together with:

- `docs/setup/DEVELOPMENT.md`
- `docs/setup/PLATFORM_SETUP.md`
- `docs/setup/TROUBLESHOOTING.md`
- `docs/CONFIGURATION_REFERENCE.md`
- `docs/DEVELOPER_REFERENCE.md`
- `docs/testing/TESTING_GUIDE.md`
- `docs/releases/QUALITY_GATE.md`
- `docs/releases/RELEASE_PROCESS.md`
- `docs/releases/RELEASE_CHECKLIST.md`
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`
- `docs/releases/STORE_BUILD_POLICY.md`
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md`
- `docs/releases/RELEASE_EVIDENCE.md`
- `docs/releases/NEXT_STEPS.md`

This file owns the executable/package **creation procedure**. The other documents own their specialized setup, testing, security, store and evidence policies.

---

## 24. External technical references

The command patterns in this guide are aligned with the .NET MAUI 10 deployment documentation available on Microsoft Learn as of 2026-08-16:

- Android publishing: `https://learn.microsoft.com/dotnet/maui/android/deployment/?view=net-maui-10.0`
- Android CLI publishing: `https://learn.microsoft.com/dotnet/maui/android/deployment/publish-cli?view=net-maui-10.0`
- iOS publishing: `https://learn.microsoft.com/dotnet/maui/ios/deployment/?view=net-maui-10.0`
- iOS CLI publishing: `https://learn.microsoft.com/dotnet/maui/ios/deployment/publish-cli?view=net-maui-10.0`
- Mac Catalyst deployment: `https://learn.microsoft.com/dotnet/maui/mac-catalyst/deployment/?view=net-maui-10.0`
- Windows deployment: `https://learn.microsoft.com/dotnet/maui/windows/deployment/overview?view=net-maui-10.0`
- Windows unpackaged CLI publishing: `https://learn.microsoft.com/dotnet/maui/windows/deployment/publish-unpackaged-cli?view=net-maui-10.0`

When Microsoft changes SDK behavior, update this guide only after verifying the repository's pinned/current SDK and actual CareNest builds.

---

## 25. Final executable-build checklist

Before declaring an artifact ready, confirm all of the following:

- [ ] Correct source commit/tag.
- [ ] Clean working tree.
- [ ] Correct .NET 10 SDK selected by `global.json`.
- [ ] Required MAUI/platform workloads installed.
- [ ] NuGet restore succeeds.
- [ ] Formatting/quality gate passes.
- [ ] Release preflight passes.
- [ ] Unit tests pass.
- [ ] Integration tests pass.
- [ ] UI/source-policy tests pass.
- [ ] Dependency audit passes.
- [ ] Target `Release` build passes.
- [ ] Strict XAML binding compilation passes without suppression.
- [ ] Store-package/payload preflight passes where applicable.
- [ ] Correct package format selected.
- [ ] Correct architecture/RID selected.
- [ ] Correct application ID/version selected.
- [ ] Production signing identity is correct.
- [ ] No signing secret is stored in source control.
- [ ] Artifact exists in the expected publish/package directory.
- [ ] Artifact hash recorded.
- [ ] Package/signature inspected.
- [ ] Fresh install tested on supported real hardware.
- [ ] Upgrade from supported prior build/data tested.
- [ ] Reminder/notification behavior tested.
- [ ] Local data/document/backup compatibility tested.
- [ ] Accessibility validated manually.
- [ ] Store/legal/privacy metadata reviewed.
- [ ] Release evidence preserved.
- [ ] Production tag workflows pass for the exact artifact source.

Only after these gates should the generated executable/package be treated as a production release candidate.
