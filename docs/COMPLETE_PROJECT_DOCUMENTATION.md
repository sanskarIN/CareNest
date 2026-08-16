# CareNest Complete Project Documentation

**Project:** CareNest  
**Release line:** `1.0.0-rc.1`  
**Documentation baseline:** 2026-08-16  
**Repository:** `https://github.com/sanskarIN/CareNest`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`  
**Verified PR #74 source head:** `8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

This is the canonical end-to-end documentation for the current CareNest source scope. Specialized documents under `docs/` provide deeper subsystem detail; this document connects them into one coherent project reference.

> **Medical boundary:** CareNest is organizational software. It does not diagnose conditions, calculate or infer dosage, recommend treatment, perform clinical medication-interaction checking, calculate clinical risk, replace a clinician/pharmacist, provide emergency services, or guarantee notification delivery.

---

## 1. Project identity

- Product name: **CareNest**
- Application ID: `com.sanskar.carenest`
- Display version: `1.0.0-rc.1`
- Application version/build: `1`
- Primary branch: `main`
- License: Apache License 2.0
- Creator/GitHub: `https://github.com/sanskarIN`
- Business contact: `sanskarin@outlook.in`
- Support contact: `supportramsandesh@gmail.com`
- Repository-only voluntary project support: `https://buymeacoffee.com/sanskarIN`
- Branding watermark used by project documentation/design where applicable: `Made by the Sanskar`

The distributed application package currently does **not** contain an external Buy Me a Coffee destination/card/command/artwork. Repository funding documentation is separate from health functionality.

---

## 2. Product purpose

CareNest helps people organize user-entered health-related information locally on their own device. The current source scope includes:

- multiple local family/person profiles;
- medicines and opaque user-entered strength/instruction text;
- explicit medicine schedules;
- deterministic reminder occurrences;
- reminder history/status states;
- appointments and optional reminders;
- medicine stock/refill notes based on user-entered quantities;
- encrypted imported document storage;
- profile photos;
- reports and portable exports;
- manual password-encrypted backups and restore;
- optional local app lock;
- reminder diagnostics/recovery;
- theme and accessibility-oriented UI;
- local privacy cleanup/data-clear workflows.

Use `docs/USER_GUIDE.md` and `docs/FEATURE_REFERENCE.md` for end-user workflows.

---

## 3. Explicit non-goals

CareNest does not currently provide:

- diagnosis;
- dosage calculation or inference;
- treatment recommendations;
- clinical medication-interaction checking;
- clinical risk scoring;
- automatic prescription/refill ordering;
- emergency-service replacement;
- guaranteed notification delivery;
- required CareNest accounts;
- automatic CareNest cloud synchronization;
- silent remote caregiver sharing;
- server-side CareNest health-record storage;
- hidden runtime analytics/telemetry networking.

Any future networked or clinical feature requires a separate design, privacy, security, consent, legal/store and safety review.

---

## 4. Technology stack

### Runtime/application

- .NET 10
- .NET MAUI
- C#
- XAML
- MVVM-style presentation separation

### Persistence

- SQLite
- `sqlite-net-pcl`
- centrally managed package versions
- explicit schema migrations
- WAL/snapshot/integrity behavior where documented
- transactional multi-step persistence where consistency requires it

### Cryptography

- built-in .NET cryptographic primitives
- authenticated AES-GCM-based document/backup protection
- PBKDF2-HMAC-SHA256 for password/PIN-derived verifier/key workflows where documented
- authenticated chunked framing v2 for new encrypted document/backup streams
- retained v1 read compatibility for documented historical formats

### Quality/security automation

- xUnit
- GitHub Actions
- CodeQL
- unsuppressed NuGet dependency auditing
- source/repository policy tests
- package payload scanning/provenance workflows

---

## 5. Solution structure

```text
CareNest.sln
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
docs/
build/scripts/
.github/workflows/
```

Dependency direction:

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

### `CareNest.Shared`

Shared primitives/helpers with no MAUI/persistence dependency.

### `CareNest.Domain`

Framework-independent entities, enums and structural validation. Domain rules must not become a clinical recommendation engine.

### `CareNest.Application`

Application contracts/use cases, repository/service abstractions, reminder planning/coordinating and compensation/recovery logic.

### `CareNest.Infrastructure`

SQLite, migrations, repositories, encrypted document storage, backup/restore, reports/exports and related implementation concerns.

### `CareNest.App`

MAUI UI/composition, XAML, ViewModels, navigation, dependency injection, platform services and Android/iOS/Mac Catalyst/Windows adapters.

### Tests

- Unit: deterministic domain/application/service behavior.
- Integration: SQLite/encryption/backup/document/report behavior.
- UI/source-policy: XAML, architecture, async-safety, privacy/logging, packaging/release/workflow contracts.

Detailed file map: `docs/CODEBASE_REFERENCE.md`.

---

## 6. Application targets

Current app target frameworks:

- Android: `net10.0-android` — minimum Android API 24;
- iOS/iPadOS: `net10.0-ios` — minimum iOS 15;
- Mac Catalyst: `net10.0-maccatalyst` — minimum 15;
- Windows: `net10.0-windows10.0.19041.0` — minimum Windows 10.0.19041.0.

`CareNestTargetFramework` allows a host/CI job to isolate one target instead of evaluating unrelated platform workloads.

See `docs/setup/PLATFORM_SETUP.md` and `docs/PLATFORM_BEHAVIOR_MATRIX.md`.

---

## 7. Current package baseline

Central package management/transitive pinning is enabled in `Directory.Packages.props`.

Important versions at this documentation baseline:

- `Microsoft.Maui.Controls` `10.0.20`;
- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- selected SQLite providers `2.1.12`;
- `Microsoft.Extensions.Logging.Debug` `10.0.0`;
- `Microsoft.NET.Test.Sdk` `17.14.1`;
- `xunit` `2.9.3`;
- `xunit.runner.visualstudio` `3.1.4`;
- `coverlet.collector` `6.0.4`.

The former exact SQLite advisory suppression remains removed. Dependency security and packaged existing-data compatibility are separate release concerns.

---

## 8. XAML/compiler policy

Current application project enables:

```xml
<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
<WarningsAsErrors>$(WarningsAsErrors);XC0022;XC0023;XC0024;XC0025</WarningsAsErrors>
```

Binding policy:

- every binding-bearing page has a real root `x:DataType`;
- binding-bearing DataTemplates have item-specific `x:DataType`;
- picker display bindings are typed when their context changes to an item;
- explicit Source/RelativeSource bindings are typed;
- template parent commands use typed ancestor binding contexts;
- no matching `NoWarn`, `x:Object` or `x:Null` bypass is part of the intended baseline.

PR #74 migrated all 15 binding-bearing pages and added permanent contract tests.

See `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

---

## 9. Core data model

CareNest stores concepts for:

- person/profile;
- medicine;
- medicine schedule and schedule-time data;
- reminder occurrence;
- medication log/history;
- appointment;
- document metadata/tags;
- stock/refill information;
- settings/contact/audit-style supporting data where applicable.

Exact entities, keys, indexes, relationships and migrations are documented in `docs/architecture/DATABASE_SCHEMA.md`.

---

## 10. Reminder architecture

Reminder scheduling crosses three distinct state surfaces:

1. explicit user schedule intent;
2. persisted CareNest reminder-occurrence state;
3. operating-system scheduled request state.

These cannot be committed atomically as one database transaction. CareNest therefore uses deterministic planning, reconciliation, cancellation-first ordering and compensation/recovery.

Core rules include:

- schedules derive only from explicit user-entered values;
- ownership/state/date/time-zone validation;
- deterministic UTC planning windows;
- half-open window boundaries;
- stable occurrence identity;
- inactive/archive suppression;
- deterministic DST gap/overlap behavior;
- no invented reminder time for invalid DST-gap input;
- valid snooze requires explicit future UTC;
- `SnoozedUntilUtc` becomes the effective due time while snoozed;
- stale OS request reconciliation after edits/restarts;
- cancellation before replacement/suppression/handled-state persistence where required;
- restoration/rebuild attempts after later failure.

Detailed contract: `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`.

---

## 11. Reminder states

CareNest can organize lifecycle/history states including scheduled, snoozed, taken, skipped, delayed, missed and cancelled.

These are organizational application states. They do not independently prove ingestion, adherence, medical correctness or treatment effectiveness.

---

## 12. Appointment architecture

Appointments store explicit UTC instants. `StartsUtc` must genuinely be UTC; local/unspecified ticks are not silently relabeled.

Optional reminder lead time comes from explicit appointment/reminder configuration. Permission denial is not treated as successful platform scheduling.

Appointment persistence/scheduling uses compensation logic where database and OS state cannot succeed atomically.

---

## 13. SQLite architecture

Structured application data is local SQLite data protected primarily by sandbox/device security. CareNest does **not** claim transparent whole-database encryption.

Persistence design includes:

- ordered schema migrations;
- transactional consistency where required;
- WAL/snapshot/integrity support for backup workflows;
- parameterized repository operations;
- repository abstractions instead of direct ViewModel SQL;
- cleanup/compensation across database/filesystem/secure-store/platform surfaces.

---

## 14. Document vault

Imported document payloads use separately encrypted application-owned storage.

Important behaviors:

- master key held through platform secure storage where applicable;
- authenticated encryption;
- chunked framing v2 for new payloads;
- authenticated terminal/truncation/trailing-data protection;
- retained v1 read compatibility where required;
- missing/corrupt key with existing encrypted payloads fails closed;
- export creates plaintext outside the encrypted vault boundary by explicit user action;
- import/export failure paths attempt cleanup/rollback.

See `docs/architecture/DOCUMENT_VAULT.md`.

---

## 15. Backup and restore

Backups are manually initiated and password-protected.

Design includes:

- password-derived key material;
- authenticated encryption;
- versioned format;
- strict decrypted archive topology validation;
- database snapshot/integrity checks;
- recovery material needed for encrypted documents where documented;
- wrong-password/tamper/truncation/trailing-data rejection;
- rollback/cleanup after failed restore.

See `docs/architecture/BACKUP_AND_RESTORE.md`.

---

## 16. App lock

Optional app lock is a local privacy barrier, not whole-database encryption.

The design includes salted password/PIN derivation, fixed-time verifier comparison, secure-store ownership, strict material validation, rollback around multi-key changes and fail-closed corrupt/missing state.

A rooted/jailbroken/fully compromised device remains outside the security guarantee.

---

## 17. Reports and exports

Exports are explicit user actions and can include portable representations such as CSV/PDF/JSON and platform integrations where supported.

Important behavior includes:

- formula-like spreadsheet content neutralization;
- partial-file staging plus atomic final move for generated files where documented;
- best-effort cleanup of application-owned temporary files;
- privacy/medical-limit warnings where appropriate;
- external copies leave CareNest's controlled boundary.

See `docs/REPORTS_AND_EXPORTS.md`.

---

## 18. Privacy model

Current v1 intentionally has:

- no required CareNest account/backend;
- no automatic CareNest cloud sync/upload;
- no hidden runtime analytics/telemetry client;
- local structured storage;
- separately encrypted imported document payloads;
- encrypted manual backups;
- explicit outbound export/share/calendar/browser actions;
- privacy-minimized diagnostic logging.

External apps, screenshots, device backups and cloud destinations can retain copies after explicit handoff.

See `PRIVACY.md`, `docs/privacy/PRIVACY_MODEL.md`, and `docs/privacy/DATA_LIFECYCLE.md`.

---

## 19. Logging policy

Do not log raw health text, document contents, passwords/PINs, backup secrets, cryptographic keys, signing material or unnecessary full sensitive-path exception contents.

Use privacy-minimized operation/category information and exception type names where sufficient.

See `docs/security/LOGGING_PRIVACY.md`.

---

## 20. Security model

Security controls include:

- OS sandbox/device security;
- encrypted document payloads;
- encrypted manual backups;
- platform secure storage for secret material where applicable;
- app-lock privacy barrier;
- strict backup/archive validation;
- authenticated stream framing;
- dependency auditing;
- CodeQL;
- source-policy/architecture/privacy contracts;
- package forbidden-marker scanning;
- exact-source release evidence.

Residual risk includes compromised devices, exported copies, weak user-selected secrets, OS notification behavior, external destinations and process termination during cross-surface compensation.

See `docs/security/SECURITY_MODEL.md` and `docs/security/THREAT_MODEL.md`.

---

## 21. Platform behavior

### Android

Platform delivery depends on notification permission, alarm capability, battery/background policy, reboot, clock/time-zone changes, force-stop state and vendor behavior. Async receiver recovery uses appropriate lifetime handling.

### Windows

Current reminder fallback has in-process limitations. Timer ownership/replacement/cancellation races are tested, but closed-app behavior remains a manual/platform concern.

### iOS/iPadOS

CI verifies simulator compilation. Real-device notification behavior, signing/provisioning and store deployment remain external/manual evidence.

### Mac Catalyst

CI verifies compilation/inspection output. Signed/notarized behavior and desktop accessibility/notification behavior require manual validation.

See `docs/PLATFORM_BEHAVIOR_MATRIX.md`.

---

## 22. Accessibility

Accessibility-oriented XAML/design/source contracts do not equal real assistive-technology certification.

Manual release evidence must cover representative:

- screen readers;
- large text/scaling;
- keyboard/focus;
- light/dark/system contrast;
- reduced motion;
- color-independent meaning;
- privacy-safe actionable errors.

See `docs/design/ACCESSIBILITY.md`.

---

## 23. Localization

Localization strategy, resource handling, translation review and RTL considerations are documented in `docs/design/LOCALIZATION.md`.

A new locale requires actual translations plus layout/date-time/accessibility/target validation.

---

## 24. Development setup

Clone:

```bash
git clone https://github.com/sanskarIN/CareNest.git
cd CareNest
```

Inspect:

```bash
git --version
dotnet --info
dotnet workload list
```

Maintainer repository-local identity:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Use `docs/setup/DEVELOPMENT.md` for full setup.

---

## 25. Core build/test commands

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release

dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

Quality gate:

```bash
build/scripts/quality-gate.sh
```

PowerShell:

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

The dependency audit is intentionally blocking.

---

## 26. Current automated verification

PR #74 frozen source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

### CareNest CI #735 / run `31938301209`

- formatting: success;
- unit tests: 122/122;
- integration tests: 39/39;
- UI/source-policy tests: 170/170;
- total: **331/331**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success.

### Store Package Configuration #124 / run `31938301146`

All four store-candidate target configurations succeeded.

### Store Inspection Artifacts #47 / run `31938301275`

Scanner self-test and Android/Windows/Apple inspection-artifact workflows succeeded.

### Security/dependency

- CodeQL #735 / `31938301252`: success;
- Dependency Audit #91 / `31938301172`: success on platform-neutral and MAUI graphs.

This supports: **no known automated defect remains under the configured PR #74 matrix for that exact source**. It does not prove global bug absence.

---

## 27. Release workflows

GitHub Actions includes:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

Production-style `v*` release flow is designed to verify the exact tagged source. A tag is not by itself production approval.

---

## 28. Application funding/package boundary

The app package no longer uses an in-app external funding surface or per-package funding toggle.

Current invariant:

- no external Buy Me a Coffee destination under application runtime/package source;
- package payload scanning prevents the canonical marker from silently re-entering distributed artifacts;
- repository funding metadata/docs remain separate;
- funding does not unlock health/medical functionality, reminder priority or clinical service.

Historical documents can describe the earlier investigation/toggle architecture but are not current product design.

---

## 29. Testing strategy

Use the lowest suitable layer:

- unit for deterministic rules;
- integration for persistence/crypto/filesystem boundaries;
- UI/source-policy for XAML/architecture/repository/release contracts;
- manual/device tests for real OS behavior and accessibility.

Source tests are not a substitute for physical-device/store/signing evidence.

See `docs/testing/TESTING_GUIDE.md`.

---

## 30. Dependency update process

For package changes:

1. review release/security implications;
2. update central package versions deliberately;
3. restore/build/test;
4. run unsuppressed dependency audit;
5. run affected MAUI targets;
6. update compatibility/security docs if persistence/crypto/platform behavior can change;
7. create new exact-source verification before treating the changed source as the current automated baseline.

Do not reintroduce broad audit suppression merely for a green job.

---

## 31. Database migration process

For schema changes:

1. add ordered migration;
2. transactionally coordinate DDL/version state where required;
3. preserve supported upgrade paths;
4. add integration tests;
5. update `DATABASE_SCHEMA.md`;
6. review backup/restore compatibility;
7. update privacy/data lifecycle for new categories;
8. perform packaged existing-data validation before production promotion.

---

## 32. Encryption change process

Before changing encrypted document/backup format, derivation, topology or key ownership:

- define compatibility explicitly;
- update security/threat-model/architecture docs;
- add tamper/truncation/trailing-data tests;
- test wrong password/key and clean-install restore;
- preserve genuine historical fixtures where available;
- document migration/rollback/recovery behavior.

Do not silently remove legacy read support without an evidenced migration strategy.

---

## 33. New network/cloud feature process

A future networked feature requires explicit design for:

- authentication;
- authorization;
- user consent;
- encryption/key ownership;
- offline behavior/conflict resolution;
- privacy/data minimization;
- export/deletion;
- audit/logging;
- threat model;
- store disclosures/policies.

It cannot be introduced as an incidental infrastructure dependency.

---

## 34. Contributing

Contributors must:

- follow `CONTRIBUTING.md`;
- preserve architecture direction;
- preserve local-first/privacy/medical-safety boundaries;
- use synthetic/fictional data;
- keep secrets/signing material out of Git;
- add regression coverage;
- update related documentation;
- run applicable quality/platform checks.

---

## 35. Documentation governance

Current documentation precedence and historical-evidence rules are defined in `docs/REPOSITORY_GOVERNANCE.md`.

Use `docs/DOCUMENTATION_CATALOG.md` as the complete navigation map.

---

## 36. Known limitations

Use `docs/KNOWN_LIMITATIONS.md` for a consolidated list of intentional/external/RC limitations.

Important examples:

- no clinical decision making;
- no notification guarantee;
- no whole-database encryption claim;
- no automatic CareNest cloud sync;
- Windows closed-app reminder limitations;
- simulator build is not real iOS device evidence;
- accessibility/manual package compatibility remain external release gates.

---

## 37. Current production blockers

CareNest remains `1.0.0-rc.1` because the following are not completed solely by source/CI documentation:

- representative Android/Windows/iPhone/iPad/Mac Catalyst manual matrices;
- real notification permission/delivery/lifecycle checks;
- packaged SQLite existing-data compatibility;
- packaged encrypted document/backup compatibility;
- real assistive-technology accessibility;
- production signing outside Git;
- final signed-package inspection/checksums/provenance;
- current store-policy/metadata review;
- exact approved production source and immutable tag;
- tagged release gates/evidence;
- final store/publication evidence.

Use `docs/releases/NEXT_STEPS.md` for the operational checklist.

---

## 38. Definitions

### Source-complete

The documented RC source behavior exists, builds under configured target jobs, passes applicable tests/policies and has matching documentation.

### Automated-verified

The named exact source passed the configured automated workflow matrix.

### Production-complete

Applicable manual/device/package/accessibility/signing/store/tag/publication evidence is complete for the exact production candidate.

These terms are not interchangeable.

---

## 39. Current truthful status

CareNest is a **source-complete, heavily automated-verified `1.0.0-rc.1` release candidate** under the PR #74 verification matrix.

The current executable source has 331/331 core tests green plus all configured normal platform builds, store-candidate builds, inspection-artifact workflows, CodeQL and unsuppressed Dependency Audit green.

CareNest is **not yet** production-signed, store-approved, production-published or proven globally bug-free.

For current state use:

- `PROJECT_STATUS.md`;
- `docs/releases/NEXT_STEPS.md`;
- `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`;
- `docs/DOCUMENTATION_CATALOG.md`.