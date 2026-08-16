# CareNest Developer Reference

**Current release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`  
**Verified PR head:** `8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

This document is a practical current-state reference for engineers making source changes. Detailed subsystem documents remain authoritative for their specialties.

## 1. Solution architecture

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

### CareNest.Shared

Cross-cutting primitives/helpers that remain free of MAUI and persistence dependencies.

### CareNest.Domain

Framework-independent entities, enums and validation. Domain validation can reject structurally invalid input but must not become a clinical decision engine.

### CareNest.Application

Use-case orchestration, repository/service interfaces, reminder planning/coordinating, recovery and compensation logic that should remain testable without a MAUI host.

### CareNest.Infrastructure

SQLite persistence/migrations/repositories, encryption, document storage, backup/restore, report/export implementation and related platform-neutral infrastructure.

### CareNest.App

MAUI composition, XAML, ViewModels, navigation, dependency injection, themes, platform adapters, notification services and Android/iOS/Mac Catalyst/Windows integrations.

## 2. Test projects

- `CareNest.UnitTests` — domain/application/service behavior.
- `CareNest.IntegrationTests` — persistence/encryption/backup/document/report integration.
- `CareNest.UiTests` — source-policy, XAML, architecture, async-safety, logging/privacy, release/build/workflow contracts.

At the PR #74 frozen source head the verified counts were 122 + 39 + 170 = 331 tests.

## 3. Current MAUI target configuration

`src/CareNest.App/CareNest.App.csproj` targets:

```text
net10.0-android
net10.0-ios
net10.0-maccatalyst
net10.0-windows10.0.19041.0
```

Minimum platform declarations:

- Android 24;
- iOS 15;
- Mac Catalyst 15;
- Windows 10.0.19041.0.

Use `CareNestTargetFramework` when isolating a target on a host/CI job.

Application metadata:

- title `CareNest`;
- ID `com.sanskar.carenest`;
- display version `1.0.0-rc.1`;
- application version `1`.

## 4. Current package baseline

Versions are centrally managed through `Directory.Packages.props` with central transitive pinning enabled.

Important versions:

- Microsoft.Maui.Controls `10.0.20`;
- sqlite-net-pcl `1.9.172`;
- SQLitePCLRaw.bundle_green `2.1.11`;
- SQLitePCLRaw.lib.e_sqlite3 `3.53.3`;
- SQLitePCLRaw.lib.e_sqlite3.android `2.1.12`;
- SQLitePCLRaw providers `2.1.12` where pinned;
- Microsoft.Extensions.Logging.Debug `10.0.0`;
- Microsoft.NET.Test.Sdk `17.14.1`;
- xunit `2.9.3`;
- xunit.runner.visualstudio `3.1.4`;
- coverlet.collector `6.0.4`.

Do not restore the former SQLite audit suppression to make a dependency change green.

## 5. Build commands

Platform-neutral:

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
```

Tests:

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

Android example:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

Use `docs/setup/PLATFORM_SETUP.md` for complete platform commands.

## 6. XAML compiled-binding policy

Current build policy intentionally fails for common uncompiled/wrong-context bindings:

```xml
<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
<WarningsAsErrors>$(WarningsAsErrors);XC0022;XC0023;XC0024;XC0025</WarningsAsErrors>
```

Rules for new XAML:

1. Binding-bearing pages require an accurate root `x:DataType`.
2. Binding-bearing `DataTemplate` elements require an item-specific `x:DataType`.
3. Picker `ItemDisplayBinding` must be typed when it runs against an item context.
4. Explicit `Source`/`RelativeSource` bindings must carry enough source type information to compile.
5. Parent commands used from templates should use the typed ViewModel ancestor binding-context pattern.
6. Do not use `NoWarn`, `x:Object` or `x:Null` as a shortcut around the policy.

`CompiledBindingContractTests` dynamically scans the views directory so future binding-bearing files enter the policy.

## 7. Reminder invariants

Reminder code is sensitive because three state surfaces cannot be atomically committed together:

- explicit user schedule intent;
- persisted CareNest occurrence state;
- OS platform request state.

Preserve:

- explicit user input only;
- entity ownership validation;
- active/archive state suppression;
- explicit time-zone handling;
- UTC planning windows and deterministic occurrence identity;
- DST gap/overlap rules;
- snoozed effective due time;
- cancellation before replacement/suppression/handled-state transitions where required;
- retryable reconciliation state after platform cancellation failures;
- database/platform compensation when later steps fail.

Do not replace these rules with a simpler flag model without redesigning the contracts and tests.

## 8. Date/time rules

- Appointment `StartsUtc` must be a genuine UTC value.
- Snooze due time must be explicit future UTC.
- Local schedule rules retain an explicit time-zone context.
- Invalid DST-gap local times must not silently become invented reminder times.
- When a source/API requires UTC, do not simply relabel local/unspecified ticks as UTC.

## 9. SQLite rules

- keep schema migrations ordered and explicit;
- coordinate migration DDL and schema version changes transactionally;
- preserve upgrade paths for supported prior schema versions;
- use repository abstractions instead of SQL from ViewModels;
- use transactions for multi-step consistency-sensitive operations;
- preserve WAL/snapshot/integrity behavior used by backup workflows;
- review packaged compatibility separately from source dependency security.

## 10. Encryption/document rules

Document and backup formats are versioned compatibility surfaces.

Before changing framing, key derivation, chunking, package topology or key ownership:

- update architecture/security/threat-model docs;
- define old-read/new-write behavior;
- add wrong-key/password/tamper/truncation/trailing-data tests;
- preserve canonical historical fixtures where they genuinely exist;
- review restore/document migration behavior;
- test cleanup/rollback around failures.

Do not silently generate a replacement document master key when existing encrypted payloads indicate that the required key is missing/corrupt.

## 11. Privacy and logging rules

Do not log:

- raw health text;
- document contents;
- PINs/passwords;
- encryption keys/salts/verifier material;
- backup contents;
- authentication/signing secrets;
- unnecessary full sensitive-path exception messages/stack traces.

Use privacy-minimized operation/category context and exception type where sufficient.

## 12. Network boundary

Current v1 is local-first and account-free. Do not casually add:

- HTTP clients;
- telemetry/analytics SDKs;
- cloud synchronization;
- remote health-record storage;
- silent sharing.

A networked feature requires an explicit design review covering consent, authentication, authorization, key management, privacy, deletion/export, threat model, offline behavior and store disclosures.

## 13. Medical-safety boundary

Code must not infer dosage, treatment, clinical interaction or risk from medicine names, strength, instructions, symptoms, diagnoses or documents.

Avoid UI copy that could turn organizational states into medical claims.

## 14. Export/report rules

- require explicit user action;
- neutralize spreadsheet formula-like user content in CSV output;
- use partial staging and atomic final move for generated outputs where documented;
- clean application-owned temporary files best effort;
- never claim control over copies after external handoff.

## 15. App-lock rules

The app lock is a privacy barrier, not database encryption.

Preserve:

- salted PBKDF2 verifier behavior;
- fixed-time comparison;
- secure-store ownership;
- strict verifier/salt validation;
- fail-closed corrupt/missing state;
- multi-key rollback behavior;
- sensitive-buffer clearing where controllable.

## 16. Source-policy/async rules

Existing contracts protect ViewModels from patterns such as:

- `async void` where not required by event boundaries;
- `Task.Run` misuse;
- direct `SQLiteAsyncConnection`/repository implementation access;
- direct network-client creation.

Follow the existing async/cancellation/service abstraction patterns instead of weakening contracts.

## 17. Quality gates

Local quality gate:

```bash
build/scripts/quality-gate.sh
```

or:

```powershell
./build/scripts/quality-gate.ps1
```

Release preflight:

```bash
build/scripts/release-preflight.sh
```

or:

```powershell
./build/scripts/release-preflight.ps1
```

The dependency audit is intended to be blocking.

## 18. GitHub workflow roles

- `ci.yml` — formatting, tests and platform builds.
- `codeql.yml` — CodeQL analysis.
- `dependency-review.yml` — dependency/audit policy.
- `store-package-verification.yml` — store-candidate configuration builds.
- `store-inspection-artifacts.yml` — internal artifacts, payload scan and provenance.
- `release-gate.yml` — production-tag aggregate gating.
- `release-evidence.yml` — exact-source release evidence/provenance.

## 19. Funding/package boundary

The distributed application source/package intentionally contains no external Buy Me a Coffee destination/card/command/artwork. Repository funding metadata/documentation is separate.

Do not reintroduce the removed destination under `src/CareNest.App` without an explicit product/store-policy review. Existing source-policy and payload scanning are designed to catch regression.

## 20. Definition of done for a source change

A normal change is not done until applicable items are complete:

- implementation;
- lowest-suitable-layer regression tests;
- architecture/privacy/security review if boundaries changed;
- documentation update;
- formatting/quality gate;
- affected platform builds;
- dependency audit for package changes;
- exact-head verification if the change affects the release baseline.

Manual/device/store rows must stay open until actually performed.

## 21. Documentation references

Use:

- `docs/CODEBASE_REFERENCE.md` for concrete files/classes;
- `docs/CONFIGURATION_REFERENCE.md` for build/package configuration;
- `docs/architecture/` for subsystem architecture;
- `docs/testing/TESTING_GUIDE.md` for test details;
- `docs/MAINTENANCE_AND_OPERATIONS.md` for operational workflows;
- `docs/REPOSITORY_GOVERNANCE.md` for evidence/documentation precedence.