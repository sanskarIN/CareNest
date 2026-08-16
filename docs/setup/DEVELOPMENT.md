# CareNest Development Setup

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

This is the primary development setup for CareNest. For the whole-project reference use `docs/COMPLETE_PROJECT_DOCUMENTATION.md`; for configuration details use `docs/CONFIGURATION_REFERENCE.md`; for target-specific setup use `docs/setup/PLATFORM_SETUP.md`.

## 1. Repository

```bash
git clone https://github.com/sanskarIN/CareNest.git
cd CareNest
```

Default branch: `main`.

## 2. Required toolchain

Core:

- Git;
- .NET 10 SDK;
- NuGet restore access;
- .NET MAUI workload(s) for selected targets;
- platform SDK/tooling for those targets.

Platform families:

- Android — Android SDK/JDK + MAUI Android workload;
- Windows — supported Windows MAUI development host/tooling;
- iOS/Mac Catalyst — compatible macOS/Xcode + Apple MAUI workload.

## 3. Inspect environment

```bash
git --version
dotnet --info
dotnet workload list
```

Apple host:

```bash
xcodebuild -version
```

Record exact toolchain information when investigating reproducibility/platform issues.

## 4. Maintainer Git identity

Repository-local convention:

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

GitHub/API/connector commits should be described using their actual metadata.

## 5. Solution structure

```text
src/
  CareNest.Shared/
  CareNest.Domain/
  CareNest.Application/
  CareNest.Infrastructure/
  CareNest.App/

tests/
  CareNest.UnitTests/
  CareNest.IntegrationTests/
  CareNest.UiTests/
```

Dependency direction:

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

Platform-neutral projects must remain free of MAUI dependencies. ViewModels must not issue direct SQL or casually create network/telemetry clients.

## 6. Install workloads

A fully provisioned host can install:

```bash
dotnet workload install maui
```

Narrow examples:

```bash
dotnet workload install maui-android
dotnet workload install maui-ios
dotnet workload install maui-maccatalyst
```

Install only workloads supported by the host.

## 7. Restore platform-neutral source first

```bash
dotnet restore src/CareNest.Shared/CareNest.Shared.csproj
dotnet restore src/CareNest.Domain/CareNest.Domain.csproj
dotnet restore src/CareNest.Application/CareNest.Application.csproj
dotnet restore src/CareNest.Infrastructure/CareNest.Infrastructure.csproj
```

Or restore the full solution on a sufficiently provisioned host:

```bash
dotnet restore CareNest.sln
```

## 8. Build platform-neutral projects

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
```

## 9. Run tests

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

PR #74 verified 122 unit + 39 integration + 170 UI/source-policy = **331/331**.

Counts belong to that exact source and can increase as tests are added.

## 10. Why `CareNestTargetFramework` exists

The app is multi-targeted. Use `CareNestTargetFramework` to narrow a host/job to one TFM:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f <tfm> -c Release \
  -p:CareNestTargetFramework=<tfm>
```

This avoids evaluating unrelated workloads and prevents app target values from leaking into platform-neutral references.

## 11. Current targets

- `net10.0-android` — minimum Android API 24;
- `net10.0-ios` — minimum iOS 15;
- `net10.0-maccatalyst` — minimum 15;
- `net10.0-windows10.0.19041.0` — minimum Windows 10.0.19041.0.

Application ID: `com.sanskar.carenest`.

## 12. Android build example

```bash
dotnet workload install maui-android

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

## 13. iOS simulator example

```bash
dotnet workload install maui-ios

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-ios -c Release \
  -p:CareNestTargetFramework=net10.0-ios \
  -p:RuntimeIdentifier=iossimulator-arm64
```

Simulator compilation is not real-device notification/signing proof.

## 14. Mac Catalyst example

```bash
dotnet workload install maui-maccatalyst

dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst
```

## 15. Windows example

```powershell
dotnet workload install maui
dotnet build src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0
```

## 16. Strict XAML compiled-binding rules

Current app project enables Source binding compilation and strict XAML compilation and promotes `XC0022`, `XC0023`, `XC0024`, `XC0025` to errors.

When adding/editing XAML:

- give binding-bearing pages accurate root `x:DataType`;
- give each binding-bearing DataTemplate its actual item `x:DataType`;
- type picker display bindings when they use an item context;
- type explicit Source/ancestor bindings;
- use typed ViewModel ancestor contexts for parent commands inside templates;
- do not hide the policy with matching `NoWarn`, `x:Object` or `x:Null`.

See `docs/DEVELOPER_REFERENCE.md`.

## 17. Central package management

Versions live in `Directory.Packages.props` with central transitive pinning enabled.

Important versions include:

- MAUI Controls `10.0.20`;
- sqlite-net-pcl `1.9.172`;
- SQLitePCLRaw.bundle_green `2.1.11`;
- lib.e_sqlite3 `3.53.3`;
- Android/provider leaves `2.1.12` where pinned;
- current test tooling in `docs/CONFIGURATION_REFERENCE.md`.

## 18. Dependency security

The former exact SQLite advisory suppression remains removed.

After package changes:

- run unsuppressed dependency audit;
- inspect resolved graph;
- run tests/affected platform builds;
- perform packaged existing-data/encrypted-data compatibility when persistence/native provider behavior can change.

Do not re-add the old suppression merely because manual compatibility remains pending.

## 19. Local quality gate

Bash:

```bash
build/scripts/quality-gate.sh
```

PowerShell:

```powershell
./build/scripts/quality-gate.ps1
```

The quality gate validates platform-neutral formatting/build/tests and blocking audit according to the repository scripts.

## 20. Formatting

Representative commands:

```bash
dotnet format src/CareNest.Shared/CareNest.Shared.csproj --verify-no-changes
dotnet format src/CareNest.Domain/CareNest.Domain.csproj --verify-no-changes
dotnet format src/CareNest.Application/CareNest.Application.csproj --verify-no-changes
dotnet format src/CareNest.Infrastructure/CareNest.Infrastructure.csproj --verify-no-changes
dotnet format tests/CareNest.UnitTests/CareNest.UnitTests.csproj --verify-no-changes
dotnet format tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj --verify-no-changes
dotnet format tests/CareNest.UiTests/CareNest.UiTests.csproj --verify-no-changes
```

Fix legitimate formatter/analyzer findings rather than weakening the gate.

## 21. Release preflight

```bash
build/scripts/release-preflight.sh
```

```powershell
./build/scripts/release-preflight.ps1
```

When `CARENEST_TARGET` is set, the selected target is audited/built as defined by the current script.

There is no current application funding-link build toggle. The application package is funding-surface-free by source policy.

## 22. Store-package preflight

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

The wrappers require an explicit supported target and delegate to the standard release preflight. They do not sign/publish production packages.

## 23. Internal inspection artifacts

`store-inspection-artifacts.yml` creates internal engineering evidence:

- exact source identity/provenance;
- scanner self-test;
- Android unsigned AAB inspection output;
- Windows self-contained inspection output;
- iOS simulator inspection output;
- unsigned Mac Catalyst inspection output;
- payload scan/checksums/provenance;
- artifact upload without production signing secrets.

The current application package contains no external Buy Me a Coffee destination by source policy; the payload scanner remains defense-in-depth.

Never distribute an internal inspection artifact as a production/store-ready package.

## 24. Reminder development rules

When changing reminders:

- use synthetic data;
- validate ownership/state;
- preserve explicit time-zone/UTC rules;
- distinguish scheduled from snoozed effective due time;
- preserve cancellation-before-replacement/suppression/invalidation;
- preserve cancellation-first handled actions;
- preserve compensation/recovery between database and OS scheduler;
- do not infer dosage/clinical intent;
- do not assume platform delivery because planning succeeded.

See `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`.

## 25. Appointment rules

- `StartsUtc` is genuine UTC;
- local/unspecified timestamps rejected;
- notification denial is not successful scheduling;
- background rebuild avoids repeated prompts;
- database/platform reminder state uses compensation.

## 26. Document/backup rules

- preserve authenticated encryption;
- preserve required legacy read compatibility;
- fail closed for missing/corrupt required key material;
- validate backup topology;
- cleanup application-owned partial plaintext/staging files best effort;
- use synthetic fixtures;
- update architecture/security/threat/release docs when format/key behavior changes.

## 27. Privacy/logging rules

Do not log:

- user health text;
- document/backup content;
- PIN/password/key material;
- signing/service secrets;
- unnecessary sensitive exception content.

Use privacy-minimized diagnostics.

## 28. Architecture rules

- UI/ViewModels do not issue direct SQL;
- platform-neutral projects do not depend on MAUI;
- local-first v1 does not casually add networking/telemetry;
- reminder planner remains deterministic/platform-neutral;
- OS request state is reconciled explicitly;
- medicine strength/instruction text remains opaque;
- no diagnosis/treatment/dosage/interaction/risk feature is introduced.

## 29. Development data

Use fictional/synthetic profiles, medicine names, documents, backups and screenshots.

Never commit real health records, keys, passwords, PINs, access tokens or production signing material.

## 30. GitHub workflows

Current repository automation includes:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

Production-style tags are expected to participate in the applicable full release matrix.

## 31. Documentation rule

When implementation/package/workflow/platform/security/release behavior changes, update relevant documentation in the same work.

Primary references:

- `docs/DOCUMENTATION_CATALOG.md`;
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`;
- `docs/DEVELOPER_REFERENCE.md`;
- `docs/CODEBASE_REFERENCE.md`;
- `docs/CONFIGURATION_REFERENCE.md`;
- `docs/MAINTENANCE_AND_OPERATIONS.md`.

## 32. Current verified baseline

PR #74:

- 331/331 tests;
- all four normal Release builds;
- all four store-candidate builds;
- Store Inspection Artifacts;
- CodeQL;
- unsuppressed Dependency Audit.

See `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

## 33. Production boundary

Development/CI success does not complete real-device accessibility, packaged compatibility, production signing, final signed-package inspection, current store policy/metadata, exact tag gates or publication.

Use `docs/releases/NEXT_STEPS.md`.