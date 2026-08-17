# CareNest Executable Build Checklist

**Release line:** `1.0.0-rc.1`  
**Companion guide:** [`../EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`](../EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md)  
**Application project:** `src/CareNest.App/CareNest.App.csproj`  
**Repository Gumroad:** `https://ramsandesh.gumroad.com`

Use this checklist when producing a CareNest executable or distributable package. The companion guide is authoritative for detailed commands and signing boundaries; `STORE_BUILD_POLICY.md` and `PACKAGED_RELEASE_VALIDATION.md` are authoritative for the current external-commerce/package boundary and production validation.

## 1. Source and environment

- [ ] Confirm the intended branch/tag/commit.
- [ ] Confirm `git status --short` is clean for a production candidate.
- [ ] Run `dotnet --info`.
- [ ] Confirm a compatible .NET 10 SDK is selected by `global.json`.
- [ ] Run `dotnet workload list` and confirm required MAUI/platform workloads.
- [ ] Confirm required platform SDKs/tools are installed.
- [ ] Confirm all signing credentials are outside the repository.
- [ ] Confirm the exact source being built is the source being verified.

## 2. Restore and quality gates

From the repository root:

```powershell
dotnet restore CareNest.sln
./build/scripts/quality-gate.ps1
./build/scripts/release-preflight.ps1
```

On macOS/Linux:

```bash
dotnet restore CareNest.sln
./build/scripts/quality-gate.sh
./build/scripts/release-preflight.sh
```

Then confirm:

- [ ] Formatting gate passes.
- [ ] Source-hygiene/line-quality gate passes.
- [ ] Structured runtime-file validation passes.
- [ ] Core Release builds pass.
- [ ] Unit tests pass.
- [ ] Integration tests pass.
- [ ] UI/source-policy tests pass.
- [ ] Gumroad repository-placement/package-isolation contracts pass.
- [ ] NuGet dependency audit passes.
- [ ] Strict XAML compiled-binding checks pass.

## 3. Windows `.exe`

Current CareNest Windows packaging baseline is unpackaged (`WindowsPackageType=None`).

```powershell
dotnet publish src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0 `
  -p:RuntimeIdentifierOverride=win-x64 `
  -p:WindowsPackageType=None `
  -p:WindowsAppSDKSelfContained=true
```

Expected output root:

```text
src/CareNest.App/bin/Release/net10.0-windows10.0.19041.0/win-x64/publish/
```

- [ ] `CareNest.App.exe` exists.
- [ ] Keep/distribute the complete publish folder, not only the `.exe`.
- [ ] Verify intended architecture.
- [ ] Apply approved Authenticode signing if required, without exposing the private key.
- [ ] Test on a clean supported Windows device.
- [ ] Scan the complete final publish payload for both repository-only commerce markers.

## 4. Android APK

```powershell
dotnet publish src/CareNest.App/CareNest.App.csproj `
  -f net10.0-android `
  -c Release `
  -p:CareNestTargetFramework=net10.0-android `
  -p:AndroidPackageFormats=apk
```

Typical output root:

```text
src/CareNest.App/bin/Release/net10.0-android/publish/
```

- [ ] APK exists.
- [ ] For production, rebuild/sign using the protected release keystore.
- [ ] Confirm package ID `com.sanskar.carenest`.
- [ ] Install/test on a real Android device at/above API 24.
- [ ] Scan the final APK/payload for both repository-only commerce markers.

## 5. Android AAB

```powershell
dotnet publish src/CareNest.App/CareNest.App.csproj `
  -f net10.0-android `
  -c Release `
  -p:CareNestTargetFramework=net10.0-android `
  -p:AndroidPackageFormats=aab
```

- [ ] AAB exists.
- [ ] AAB is signed with the intended release identity.
- [ ] Store version/build number is valid/monotonic.
- [ ] Store-package configuration checks pass.
- [ ] ZIP/AAB payload scan passes for both repository-only commerce markers.

## 6. iOS `.ipa`

Run on a Mac with correct Apple signing/provisioning:

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

- [ ] IPA exists.
- [ ] Bundle ID matches the registered App ID.
- [ ] Certificate/profile/channel are correct.
- [ ] Real-device installation/TestFlight validation completed as appropriate.
- [ ] Final IPA/package contents pass external-commerce marker inspection.

## 7. Mac Catalyst `.app`

```bash
dotnet publish src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst \
  -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst \
  -p:CreatePackage=false
```

- [ ] `.app` exists.
- [ ] Architecture is correct.
- [ ] App launches from a clean location.
- [ ] Signing/notarization requirements are completed for intended channel.
- [ ] Final application bundle passes external-commerce marker inspection.

## 8. Mac Catalyst `.pkg`

```bash
dotnet publish src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst \
  -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst \
  -p:CreatePackage=true
```

- [ ] `.pkg` exists when requested.
- [ ] Correct Apple signing/package-signing identities are used.
- [ ] Install/upgrade behavior is tested.
- [ ] Notarization/store requirements are completed if applicable.
- [ ] Final package passes external-commerce marker inspection.

## 9. Repository-only external-commerce boundary

The following are intentionally promoted in repository documentation but excluded from the CareNest application package:

```text
https://buymeacoffee.com/sanskarIN
https://ramsandesh.gumroad.com
```

Repository-only Gumroad badge:

`docs/assets/gumroad_store_badge.svg`

Confirm:

- [ ] no Gumroad URL in application runtime/source resources;
- [ ] no Buy Me a Coffee URL in application runtime/source resources;
- [ ] no Gumroad repository promotional badge in app resources;
- [ ] no Gumroad/BMC card/command in About/runtime UI;
- [ ] no purchase/funding state changes any health behavior.

## 10. Store/payload preflight

Set an explicit target before invoking the store-package preflight as documented in `STORE_BUILD_POLICY.md`.

Example:

```bash
CARENEST_TARGET=net10.0-android ./build/scripts/store-package-preflight.sh
```

PowerShell:

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
./build/scripts/store-package-preflight.ps1
```

- [ ] Store preflight passes.
- [ ] Package contains no unintended repository-only files/secrets.
- [ ] Package identity/version/assets are inspected.
- [ ] `build/scripts/verify-store-safe-payload.py <payload>` passes with default markers.
- [ ] `buymeacoffee.com/sanskarIN` absent from payload.
- [ ] `ramsandesh.gumroad.com` absent from payload.

## 11. Runtime validation

On every production target:

- [ ] Fresh install succeeds.
- [ ] App launches without development tooling.
- [ ] Existing supported data upgrades correctly.
- [ ] Structured local data remains readable/editable.
- [ ] Document vault behavior is correct.
- [ ] Backup/restore are compatible.
- [ ] Reminder scheduling/cancellation/reconciliation behaves correctly.
- [ ] Notification permissions/delivery behavior are checked.
- [ ] App-lock/privacy lifecycle is checked.
- [ ] Explicit export/share flows work.
- [ ] Accessibility is checked with real assistive technology.
- [ ] No repository Gumroad/BMC promotional surface appears in the installed app.

## 12. Artifact evidence

Windows:

```powershell
Get-FileHash -Algorithm SHA256 "<artifact-path>"
```

macOS/Linux:

```bash
shasum -a 256 "<artifact-path>"
```

Record:

- [ ] Source commit/tag.
- [ ] Display version and application/build version.
- [ ] Platform and architecture/RID.
- [ ] .NET SDK and MAUI workload versions.
- [ ] Artifact filename.
- [ ] SHA-256.
- [ ] Signing identity/fingerprint as appropriate.
- [ ] Build machine/CI run.
- [ ] Buy Me a Coffee marker scan result.
- [ ] Gumroad marker scan result.
- [ ] Installed-app external-commerce surface result.
- [ ] Validation evidence.
- [ ] Store submission/publication evidence when complete.

## 13. Final release gate

- [ ] `docs/releases/RELEASE_CHECKLIST.md` completed.
- [ ] `docs/releases/PACKAGED_RELEASE_VALIDATION.md` completed.
- [ ] `docs/releases/STORE_SUBMISSION_CHECKLIST.md` completed where applicable.
- [ ] `docs/releases/SECURITY_RELEASE_REVIEW.md` completed.
- [ ] Required production-tag workflows pass for the exact approved source.
- [ ] Final signed packages are scanned for both repository-only external-commerce markers.
- [ ] No unsigned/internal artifact is mislabeled as production release.
- [ ] No store approval or real-device/accessibility evidence is claimed without actual evidence.

The highlighted repository storefront remains **https://ramsandesh.gumroad.com**; under the current policy it is not embedded in the distributed CareNest health app.
