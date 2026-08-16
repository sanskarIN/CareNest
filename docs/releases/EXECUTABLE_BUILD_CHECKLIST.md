# CareNest Executable Build Checklist

**Release line:** `1.0.0-rc.1`  
**Companion guide:** [`../EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`](../EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md)  
**Application project:** `src/CareNest.App/CareNest.App.csproj`

Use this checklist when producing a CareNest executable or distributable package. The companion guide is authoritative for explanations, file coverage, signing boundaries, troubleshooting, and release evidence.

## 1. Source and environment

- [ ] Confirm the intended branch/tag/commit.
- [ ] Confirm `git status --short` is clean for a production candidate.
- [ ] Run `dotnet --info`.
- [ ] Confirm a compatible .NET 10 SDK is selected by `global.json`.
- [ ] Run `dotnet workload list` and confirm the required MAUI/platform workloads.
- [ ] Confirm required platform SDKs/tools are installed.
- [ ] Confirm all signing credentials are outside the repository.

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
- [ ] Source-hygiene gate passes.
- [ ] Core Release builds pass.
- [ ] Unit tests pass.
- [ ] Integration tests pass.
- [ ] UI/source-policy tests pass.
- [ ] NuGet dependency audit passes.
- [ ] Strict XAML compiled-binding checks pass.

## 3. Windows `.exe`

Current CareNest Windows packaging baseline is unpackaged (`WindowsPackageType=None`).

### x64 self-contained publish

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
- [ ] Verify the intended architecture.
- [ ] If production signing is required, apply the approved Authenticode signature without exposing the private key.
- [ ] Test on a clean supported Windows device.

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
- [ ] Install and test on a real Android device at/above API 24.

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
- [ ] Store version/build number is valid and monotonic for the target store.
- [ ] Store-package and payload checks pass.

## 6. iOS `.ipa`

Run on a Mac with the correct Apple signing/provisioning setup:

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
- [ ] Signing/notarization requirements are completed for the intended distribution channel.

## 8. Mac Catalyst `.pkg`

```bash
dotnet publish src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst \
  -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst \
  -p:CreatePackage=true
```

- [ ] `.pkg` exists when packaging is requested.
- [ ] Correct Apple signing/package-signing identities are used.
- [ ] Install/upgrade behavior is tested.
- [ ] Notarization/store requirements are completed if applicable.

## 9. Store/payload preflight

PowerShell:

```powershell
./build/scripts/store-package-preflight.ps1
```

Shell:

```bash
./build/scripts/store-package-preflight.sh
```

- [ ] Store-safe payload verification passes.
- [ ] Package contains no unintended repository-only files/secrets.
- [ ] Package identity/version/assets are inspected.

## 10. Runtime validation

On every production target:

- [ ] Fresh install succeeds.
- [ ] App launches without development tooling.
- [ ] Existing supported data upgrades correctly.
- [ ] Structured local data remains readable.
- [ ] Document vault behavior is correct.
- [ ] Backup and restore are compatible.
- [ ] Reminder scheduling/cancellation/reconciliation behaves correctly.
- [ ] Notification permissions and delivery behavior are checked.
- [ ] App-lock/privacy lifecycle is checked.
- [ ] Explicit export/share flows work.
- [ ] Accessibility is checked with real assistive technology.

## 11. Artifact evidence

Windows PowerShell:

```powershell
Get-FileHash -Algorithm SHA256 "<artifact-path>"
```

macOS/Linux:

```bash
shasum -a 256 "<artifact-path>"
```

Record:

- [ ] Source commit/tag.
- [ ] Display version and build/application version.
- [ ] Platform and architecture/RID.
- [ ] .NET SDK and MAUI workload versions.
- [ ] Artifact filename.
- [ ] SHA-256.
- [ ] Signing identity/fingerprint as appropriate.
- [ ] Build machine/CI run.
- [ ] Validation evidence.
- [ ] Store submission/publication evidence when complete.

## 12. Final release gate

- [ ] `docs/releases/RELEASE_CHECKLIST.md` completed.
- [ ] `docs/releases/PACKAGED_RELEASE_VALIDATION.md` completed.
- [ ] `docs/releases/STORE_SUBMISSION_CHECKLIST.md` completed where applicable.
- [ ] `docs/releases/SECURITY_RELEASE_REVIEW.md` completed.
- [ ] `docs/releases/RELEASE_EVIDENCE.md` updated/preserved.
- [ ] Required production-tag workflows pass for the exact approved source.
- [ ] No unsigned/internal artifact is mislabeled as a production release.
- [ ] No store approval or real-device/accessibility evidence is claimed without actual evidence.
