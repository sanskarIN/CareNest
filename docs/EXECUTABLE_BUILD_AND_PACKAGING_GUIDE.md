# CareNest Executable Build and Packaging Guide

**Release line:** `1.0.0-rc.1`  
**Documentation baseline:** 2026-08-17  
**Application project:** `src/CareNest.App/CareNest.App.csproj`  
**Application ID:** `com.sanskar.carenest`  
**SDK family:** .NET 10 / .NET MAUI 10  
**Repository Gumroad:** `https://ramsandesh.gumroad.com`

The complete executable guide that was active before the Gumroad rollout is preserved exactly at:

`docs/history/pre-gumroad-rollout-20260817/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`

This is the canonical end-to-end guide for creating CareNest executable/distribution artifacts. It covers repository build inputs, validation inputs, all four target platforms, signing boundaries, expected outputs, store-safe external-commerce isolation, package inspection, checksums, release evidence and troubleshooting.

> Publish the MAUI **application project**, not `CareNest.sln`. The solution also contains libraries and test projects that are not distributable applications.

---

## 1. Supported executable/package targets

CareNest is one .NET MAUI application with four target frameworks:

| Platform | Target framework | Minimum | Typical artifact |
|---|---|---:|---|
| Android | `net10.0-android` | API 24 | `.apk` and/or `.aab` |
| iOS/iPadOS | `net10.0-ios` | iOS 15 | signed `.ipa` |
| Mac Catalyst | `net10.0-maccatalyst` | 15 | `.app`, optionally `.pkg` |
| Windows | `net10.0-windows10.0.19041.0` | Windows 10 build 19041 | unpackaged `.exe` + publish folder |

The project currently uses an unpackaged Windows baseline with `WindowsPackageType=None`. A future MSIX/store packaging path is a separate reviewed packaging/signing decision.

---

## 2. Repository build surface

### Root build configuration

These affect restore/build/validation but are not normal runtime package content:

- `CareNest.sln` — development/test aggregation;
- `global.json` — SDK selection;
- `Directory.Build.props` — compiler/analyzer/build metadata;
- `Directory.Packages.props` — central package versions;
- `NuGet.config` — restore/package-source policy;
- `.editorconfig` — formatting/source conventions;
- `.gitignore` — generated/secret exclusions.

### Production source projects

```text
src/CareNest.Shared/CareNest.Shared.csproj
src/CareNest.Domain/CareNest.Domain.csproj
src/CareNest.Application/CareNest.Application.csproj
src/CareNest.Infrastructure/CareNest.Infrastructure.csproj
src/CareNest.App/CareNest.App.csproj
```

Dependency direction:

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

### Test projects

```text
tests/CareNest.UnitTests/CareNest.UnitTests.csproj
tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj
tests/CareNest.UiTests/CareNest.UiTests.csproj
```

These are release-validation inputs and must not be copied into production packages.

### Build/release tooling

Relevant scripts include:

- `build/scripts/quality-gate.sh` / `.ps1`;
- `build/scripts/release-preflight.sh` / `.ps1`;
- `build/scripts/store-package-preflight.sh` / `.ps1`;
- `build/scripts/setup-git.sh` / `.ps1`;
- `build/scripts/verify-store-safe-payload.py`.

### Documentation and repository marketing

Files under `docs/**`, root Markdown documentation and `.github/**` are repository/automation inputs rather than runtime application content unless a project file explicitly includes them.

Important repository-only Gumroad files:

- `GUMROAD.md`;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`;
- `docs/assets/gumroad_store_badge.svg`.

They must not be copied into the CareNest app package under the current release/store policy.

---

## 3. Application resources that do ship

MAUI resources matched by `src/CareNest.App/CareNest.App.csproj` can enter platform packages, including:

- `Resources/AppIcon/**`;
- `Resources/Splash/**`;
- `Resources/Images/**` according to project matching rules;
- `Resources/Raw/**` according to project matching rules;
- platform manifests/plists/resources under `Platforms/**` for the selected target;
- application C#/XAML compiled for the selected target.

Therefore repository promotional assets must remain outside these package-input paths unless an explicit future product/store review changes the policy.

---

## 4. Gumroad and Buy Me a Coffee package boundary

CareNest highlights these destinations in repository/documentation surfaces:

- Gumroad: `https://ramsandesh.gumroad.com`;
- Buy Me a Coffee: `https://buymeacoffee.com/sanskarIN`.

Under the current policy, distributed CareNest application source/package contains no external Gumroad/BMC destination, card, command or promotional artwork.

A purchase or contribution does not unlock or change:

- diagnosis;
- dosage decisions;
- treatment recommendations;
- clinical interaction/risk behavior;
- reminder priority or delivery guarantees;
- emergency assistance;
- CareNest health-data access;
- CareNest account/cloud functionality.

CareNest does not automatically transmit local health records to Gumroad or Buy Me a Coffee.

---

## 5. Store-safe payload scanner

Run:

```bash
python build/scripts/verify-store-safe-payload.py <file-or-directory>
```

Default forbidden markers:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

The scanner checks UTF-8, UTF-16 LE and UTF-16 BE byte representations, regular files and ZIP-compatible package entries such as AABs. It fails closed for missing/unreadable inspection paths.

Use the defaults for normal CareNest package verification. The repeatable `--forbidden` option exists for explicit additional checks, not to drop the canonical markers from release validation.

---

## 6. Toolchain prerequisites

### Common

```bash
git --version
dotnet --info
dotnet workload list
```

A compatible .NET 10 SDK must satisfy `global.json`.

### Android

Requires the .NET Android/MAUI workload, compatible Android SDK/build tools and JDK. Production signing requires a protected Android signing identity outside Git.

### Windows

Requires Windows 10/11 and current .NET MAUI/Windows build tooling. Production Authenticode/signing credentials remain outside Git.

### iOS / Mac Catalyst

Apple distribution builds require macOS, a compatible Xcode installation and appropriate Apple signing/provisioning identities. Private signing material remains outside Git.

---

## 7. Clean checkout and restore

From repository root:

```bash
git status --short
dotnet --info
dotnet restore CareNest.sln
```

For target-isolated MAUI restore:

```bash
dotnet restore src/CareNest.App/CareNest.App.csproj \
  -p:CareNestTargetFramework=net10.0-android
```

Replace the target with the appropriate iOS/Mac Catalyst/Windows TFM.

When diagnosing stale builds, clean affected `bin/` and `obj/` outputs. Do not include production keys/profiles in generic cleanup operations.

---

## 8. Quality gate before packaging

Bash:

```bash
./build/scripts/quality-gate.sh
./build/scripts/release-preflight.sh
```

PowerShell:

```powershell
./build/scripts/quality-gate.ps1
./build/scripts/release-preflight.ps1
```

Core test commands:

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

The UI/source-policy suite includes strict XAML contracts, architecture/privacy/release rules, source-line quality checks, structured-file validation and Gumroad/BMC package-isolation contracts.

---

## 9. Strict XAML compilation

The app project intentionally uses strict compiled binding behavior, including Source binding compilation and `XC0022`–`XC0025` as errors.

Do not add `NoWarn` or type-safety bypasses simply to make a platform publish succeed. Correct binding context/type information instead.

---

## 10. Windows x64 executable publish

Recommended self-contained unpackaged x64 publish:

```powershell
dotnet publish src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0 `
  -p:RuntimeIdentifierOverride=win-x64 `
  -p:WindowsPackageType=None `
  -p:WindowsAppSDKSelfContained=true
```

Typical output:

```text
src/CareNest.App/bin/Release/net10.0-windows10.0.19041.0/win-x64/publish/
```

The publish folder contains `CareNest.App.exe` plus required adjacent files. **Distribute/inspect the complete publish folder, not only the EXE.**

Then:

```powershell
python build/scripts/verify-store-safe-payload.py `
  src/CareNest.App/bin/Release/net10.0-windows10.0.19041.0/win-x64/publish
```

For production, apply approved signing without exposing private keys, then repeat/equivalently run the package scan on the signed final payload and smoke-test on a clean supported system.

---

## 11. Android APK

```powershell
dotnet publish src/CareNest.App/CareNest.App.csproj `
  -f net10.0-android `
  -c Release `
  -p:CareNestTargetFramework=net10.0-android `
  -p:AndroidPackageFormats=apk
```

Typical publish root:

```text
src/CareNest.App/bin/Release/net10.0-android/publish/
```

Verify:

- APK exists;
- application ID is `com.sanskar.carenest`;
- version/build are correct;
- protected release signing is used for production;
- install/launch on a representative real device;
- notification/reminder behavior;
- package scan for both canonical repository-only markers.

---

## 12. Android AAB

```powershell
dotnet publish src/CareNest.App/CareNest.App.csproj `
  -f net10.0-android `
  -c Release `
  -p:CareNestTargetFramework=net10.0-android `
  -p:AndroidPackageFormats=aab
```

Then scan the exact AAB:

```bash
python build/scripts/verify-store-safe-payload.py <path-to-aab>
```

Because AAB is ZIP-compatible, the scanner inspects package entries as well as marker encodings.

Record signing identity/provenance and verify store versioning requirements before submission.

---

## 13. iOS simulator verification build

CI-style simulator build:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-ios \
  -c Release \
  -p:CareNestTargetFramework=net10.0-ios \
  -p:RuntimeIdentifier=iossimulator-arm64
```

Simulator success proves compilation for that target, not real-device notification behavior, production signing or store readiness.

---

## 14. iOS device/IPA publish

On a properly configured Mac:

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

Typical output area:

```text
src/CareNest.App/bin/Release/net10.0-ios/ios-arm64/
```

Exact packaging layout can depend on the installed .NET Apple workload/Xcode/signing configuration.

Verify:

- registered bundle ID/application identity;
- certificate/profile/channel;
- actual real-device/TestFlight installation as appropriate;
- notification permission/delivery/recovery;
- package contents for both repository-only markers;
- SHA-256/provenance.

---

## 15. Mac Catalyst `.app`

```bash
dotnet publish src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst \
  -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst \
  -p:CreatePackage=false
```

Verify architecture, clean launch, runtime behavior, package marker absence and intended signing/notarization requirements.

---

## 16. Mac Catalyst `.pkg`

```bash
dotnet publish src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst \
  -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst \
  -p:CreatePackage=true
```

Packaging behavior depends on the Apple workload/signing channel. Verify actual generated package, installation/upgrade, signatures/notarization, final payload marker absence and checksum/provenance.

---

## 17. Store-package preflight

The wrapper requires an explicit supported target.

Android:

```bash
CARENEST_TARGET=net10.0-android \
./build/scripts/store-package-preflight.sh
```

Windows PowerShell:

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
./build/scripts/store-package-preflight.ps1
Remove-Item Env:CARENEST_TARGET
```

The preflight validates source/build policy; it does not substitute for final signed-package inspection.

---

## 18. Current GitHub Actions package/build matrix

Relevant workflow roles:

- `.github/workflows/ci.yml` — core tests plus Android/Windows/iOS simulator/Mac Catalyst Release builds;
- `.github/workflows/codeql.yml` — security analysis;
- `.github/workflows/dependency-review.yml` — dependency/audit policy;
- `.github/workflows/store-package-verification.yml` — four store-candidate configurations;
- `.github/workflows/store-inspection-artifacts.yml` — internal package inspection/provenance on configured triggers;
- `.github/workflows/release-gate.yml` — production-tag aggregate gate;
- `.github/workflows/release-evidence.yml` — exact-source release evidence.

An Actions result belongs only to the exact source SHA shown by the run.

---

## 19. Store Inspection Artifacts boundary

Internal inspection outputs may be unsigned, unpackaged or simulator-targeted by design. They are engineering evidence, not proof of production signing, notarization, store approval or universal installability.

The inspection workflow should keep the package scanner fail-closed and use the canonical repository-only markers.

---

## 20. Latest fully verified pre-Gumroad build baseline

Exact source:

`7cbe5568b6cffa06c279b29f3cb1b107ea988791`

Verified on that revision:

- 122/122 unit;
- 39/39 integration;
- 173/173 UI/source-policy;
- **334/334 total core tests**;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- all four store-candidate configurations;
- CodeQL.

The Gumroad rollout changes test/scanner/documentation policy and therefore requires its own exact final-source matrix before replacing that baseline.

---

## 21. Final package validation checklist

For each intended production artifact:

- [ ] exact source SHA/tag recorded;
- [ ] release/build number recorded;
- [ ] application/package identity verified;
- [ ] filename and architecture/RID recorded;
- [ ] production signing/notarization provenance recorded;
- [ ] SHA-256 recorded;
- [ ] final payload scan passes for `buymeacoffee.com/sanskarIN`;
- [ ] final payload scan passes for `ramsandesh.gumroad.com`;
- [ ] no Gumroad/BMC promotional runtime card/command/artwork appears;
- [ ] fresh install/launch succeeds;
- [ ] supported data upgrades correctly;
- [ ] encrypted document behavior works;
- [ ] encrypted backup/restore compatibility works;
- [ ] reminder delivery/reconciliation tested on real target where required;
- [ ] accessibility manually tested;
- [ ] privacy/terms/security/support surfaces verified;
- [ ] submission-time store-policy review completed.

Use `docs/releases/PACKAGED_RELEASE_VALIDATION.md` for detailed evidence steps.

---

## 22. Checksums

Windows PowerShell:

```powershell
Get-FileHash -Algorithm SHA256 "<artifact-path>"
```

macOS/Linux:

```bash
shasum -a 256 "<artifact-path>"
```

Record checksum alongside exact source and signing provenance.

---

## 23. Production signing rules

Never commit:

- Android private keystores/keys;
- Apple private signing certificates/keys/passwords/provisioning secrets;
- Windows private signing keys/PFX passwords;
- CI/service credentials;
- private `.env` values.

Repository evidence may record safe public certificate fingerprints/identifiers and signing/notarization results.

---

## 24. Packaged data compatibility

Source tests and dependency audit cannot prove real package upgrade compatibility.

Before production release, using synthetic data, validate:

- SQLite open/integrity/schema migration/read/edit;
- reminder rebuild/reconciliation after upgrade;
- encrypted document compatibility;
- backup create/restore/wrong-password/tamper/truncation/trailing-data behavior;
- genuine historical encrypted fixtures where genuine prior bytes safely exist.

Do not manufacture an artifact and call it historical evidence.

---

## 25. Real-device reminder validation

Real production evidence must cover platform-specific notification permissions, delivery, cancellation/snooze, restart/reboot/recovery and time-zone/DST behavior. Android also needs alarm/battery/vendor restriction validation.

Simulator compilation does not replace real iPhone/iPad behavior.

---

## 26. Accessibility validation

Production candidates require representative real assistive-technology checks for screen readers, text scaling, keyboard/focus, contrast/themes, reduced motion and color-independent state meaning.

Automated XAML/source contracts are necessary but not sufficient.

---

## 27. Submission-time store review

Apple, Google and Microsoft requirements can change. Review current rules for the exact package/listing at submission time, including rules affecting health-organizer claims, privacy/data-safety, permissions and external commerce.

The current source policy keeps Gumroad/BMC outside the app package. If a future store policy/product decision proposes in-app external commerce, treat that as a new reviewed feature with new tests and exact-source verification.

---

## 28. Troubleshooting

### Missing MAUI workload

Use:

```bash
dotnet workload list
```

Install/restore the target workload appropriate to the host.

### Multi-target restore/build fails on a host lacking unrelated workloads

Use `CareNestTargetFramework` to isolate the target.

### XAML compiler error `XC0022`–`XC0025`

Fix binding type/context information. Do not suppress the warning policy.

### Windows executable launches only with adjacent files

Expected for the unpackaged publish model: distribute the complete publish directory, not only `CareNest.App.exe`.

### Payload scanner finds Gumroad/BMC

Do not suppress the scanner. Inspect package inputs/resources/generated payloads, remove the unintended external-commerce marker from the application build surface, rebuild from clean outputs and rescan.

### Scanner cannot inspect a file/directory

Treat this as a failure. Correct path/permissions/package creation, then rerun.

### Signed package differs from internal inspection package

Repeat the scanner and manual installed-app checks on the signed final package. Internal evidence cannot substitute for the exact production artifact.

---

## 29. Release evidence

For final production promotion record at minimum:

```text
Source SHA/tag:
Release/build:
Platform/architecture:
Artifact filename:
SHA-256:
Signing/notarization/store provenance:
CI run:
CodeQL/dependency result:
Store-candidate result:
Buy Me a Coffee marker scan:
Gumroad marker scan:
Installed-app external-commerce surface check:
SQLite upgrade result:
Encrypted document/backup result:
Reminder/notification result:
Accessibility result:
Store-policy review date/source:
Publication/submission result:
```

---

## 30. Current next step

After the exact final Gumroad/documentation source is green in CI, the next meaningful work is **real production validation**, not another broad speculative refactor.

Use:

- `PROJECT_STATUS.md`;
- `what_changed.md`;
- `docs/releases/NEXT_STEPS.md`;
- `docs/releases/EXECUTABLE_BUILD_CHECKLIST.md`;
- `docs/releases/STORE_BUILD_POLICY.md`;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`.

The highlighted repository storefront is **https://ramsandesh.gumroad.com** and remains separate from the distributed CareNest health application under the current release policy.
