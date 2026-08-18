# CareNest Complete Project Documentation

<p align="center">
  <a href="https://ramsandesh.gumroad.com">
    <img src="assets/gumroad_store_badge.svg" alt="Shop on Gumroad — https://ramsandesh.gumroad.com" width="850" />
  </a>
</p>

**Project:** CareNest  
**Release line:** `1.0.0-rc.1`  
**Documentation baseline:** 2026-08-18  
**Repository:** `https://github.com/sanskarIN/CareNest`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`  
**Latest fully verified Gumroad implementation/source-policy source:** `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

The complete project guide that was active before the Gumroad rollout is preserved exactly at:

`docs/history/pre-gumroad-rollout-20260817/COMPLETE_PROJECT_DOCUMENTATION.md`

This file is the current end-to-end project reference. Specialized documents remain authoritative for deeper subsystem detail.

> **Medical boundary:** CareNest is organizational software. It does not diagnose conditions, calculate or infer dosage, recommend treatment, perform clinical medication-interaction checking, calculate clinical risk, replace a clinician/pharmacist, provide emergency services, or guarantee notification delivery.

---

## 1. Project identity

- Product name: **CareNest**
- Application ID: `com.sanskar.carenest`
- Display version: `1.0.0-rc.1`
- Application/build version: `1`
- Primary branch: `main`
- License: Apache License 2.0
- Creator/GitHub: `https://github.com/sanskarIN`
- Business contact: `sanskarin@outlook.in`
- Support contact: `supportramsandesh@gmail.com`
- Gumroad storefront: `https://ramsandesh.gumroad.com`
- Buy Me a Coffee: `https://buymeacoffee.com/sanskarIN`
- Project watermark where applicable: `Made by the Sanskar`

CareNest is open source and local-first. Repository marketing/support links are separate from application health functionality.

---

## 2. Gumroad storefront

The Ram Sandesh Gumroad storefront is prominently documented throughout current repository-owned support, marketing, metadata and documentation surfaces:

**https://ramsandesh.gumroad.com**

Canonical files:

- `GUMROAD.md` — reader-facing storefront guide;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — placement/package policy;
- `docs/marketing/GUMROAD_ROLLOUT_CHECKLIST.md` — rollout verification checklist;
- `docs/assets/gumroad_store_badge.svg` — repository-only visual CTA;
- `docs/assets/README.md` — asset usage/accessibility rules.

A Gumroad purchase is **not** a CareNest medical/health entitlement. It does not provide or change:

- diagnosis;
- dosage guidance;
- treatment recommendations;
- medication-interaction claims;
- clinical risk scoring;
- reminder priority or delivery guarantees;
- emergency assistance;
- CareNest health-data access;
- account/cloud functionality.

CareNest does not automatically transmit local health records to Gumroad.

---

## 3. External-commerce package boundary

The current CareNest application runtime/package intentionally excludes repository-only commercial/funding destinations and promotional artwork.

Forbidden package markers under the current policy include:

- `ramsandesh.gumroad.com`;
- `buymeacoffee.com/sanskarIN`.

Do not place those destinations in:

- `src/CareNest.App` ViewModels;
- application XAML;
- shared runtime URL constants;
- platform manifests/plists for promotional purposes;
- packaged image/resources;
- app commands/buttons under the current product/store policy.

`build/scripts/verify-store-safe-payload.py` defaults to scanning both markers in UTF-8, UTF-16 little-endian and UTF-16 big-endian representations, including ZIP-compatible package entries.

Repository-only support/storefront material remains allowed in documentation, GitHub metadata and repository marketing assets.

---

## 4. Product purpose

CareNest helps people organize user-entered health-related information locally on their own device.

Current source-controlled scope includes:

- multiple person/family profiles;
- medicines and user-entered strength/instruction text;
- explicit medicine schedules;
- deterministic reminder occurrences;
- reminder lifecycle/history states;
- appointments and optional reminders;
- medicine stock/refill notes;
- encrypted imported-document storage;
- profile images;
- reports and portable exports;
- manual password-encrypted backup and restore;
- optional local app lock;
- reminder diagnostics/recovery;
- local privacy cleanup/data-clear workflows;
- light/dark/system themes;
- accessibility-oriented UI/source contracts.

Use `docs/USER_GUIDE.md` and `docs/FEATURE_REFERENCE.md` for end-user workflows.

---

## 5. Explicit non-goals

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
- CareNest server-side health-record storage;
- hidden runtime analytics/telemetry networking.

Any future networked, commerce-in-app or clinical feature requires a separate product, privacy, security, safety, consent, legal/store and testing review.

---

## 6. Technology stack

### Application

- .NET 10
- .NET MAUI
- C#
- XAML
- MVVM-style presentation separation

### Persistence

- SQLite
- `sqlite-net-pcl`
- central package management/transitive pinning
- explicit ordered schema migrations
- transactions where consistency requires them
- WAL/snapshot/integrity behavior for backup workflows where documented

### Cryptography

- supported .NET cryptographic primitives
- authenticated AES-GCM-based document/backup protection
- PBKDF2-HMAC-SHA256 for documented password/PIN-derived workflows
- authenticated chunked framing v2 for current encrypted streams
- retained documented legacy-read compatibility

### Quality/security automation

- xUnit
- GitHub Actions
- CodeQL
- blocking NuGet dependency auditing
- source/repository policy tests
- package marker scanning
- structured package evidence self-tests
- release/store configuration gates
- exact-source evidence workflows

---

## 7. Solution structure

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
  architecture/
  assets/
  design/
  history/
  marketing/
  privacy/
  releases/
  security/
  setup/
  testing/
build/scripts/
.github/workflows/
```

Intended dependency direction:

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

Detailed ownership: `docs/CODEBASE_REFERENCE.md` and `docs/architecture/SERVICE_BOUNDARIES.md`.

---

## 8. Project responsibilities

### `CareNest.Shared`

Small cross-layer primitives/helpers without MAUI or persistence coupling.

External-commerce URLs must not be introduced as application constants under the current package policy.

### `CareNest.Domain`

Framework-independent entities, enums and structural/domain validation. It must not become a clinical decision engine.

### `CareNest.Application`

Use-case/service contracts, repository abstractions, reminder planning/coordinating, recovery and compensation logic.

### `CareNest.Infrastructure`

SQLite, migrations, repositories, encrypted document storage, backup/restore, reports/exports and related implementations.

### `CareNest.App`

MAUI composition, XAML, ViewModels, navigation, dependency injection, themes, platform services and Android/iOS/Mac Catalyst/Windows adapters.

Repository-only Gumroad/Buy Me a Coffee promotion must not leak into this project under the current store policy.

---

## 9. Platform targets

Current targets:

- Android: `net10.0-android`, minimum API 24;
- iOS/iPadOS: `net10.0-ios`, minimum iOS 15;
- Mac Catalyst: `net10.0-maccatalyst`, minimum 15;
- Windows: `net10.0-windows10.0.19041.0`, minimum Windows 10.0.19041.0.

`CareNestTargetFramework` can isolate one target on CI/hosts that do not have every workload.

See:

- `docs/setup/PLATFORM_SETUP.md`;
- `docs/PLATFORM_BEHAVIOR_MATRIX.md`;
- `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`.

---

## 10. Package baseline

Central package management is configured in `Directory.Packages.props`.

Important documented versions include:

- `Microsoft.Maui.Controls` `10.0.20`;
- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- Android/provider SQLite leaves `2.1.12` where pinned;
- `Microsoft.Extensions.Logging.Debug` `10.0.0`;
- `Microsoft.NET.Test.Sdk` `17.14.1`;
- `xunit` `2.9.3`;
- `xunit.runner.visualstudio` `3.1.4`;
- `coverlet.collector` `6.0.4`.

The former exact SQLite advisory suppression remains removed. Do not reintroduce it merely to obtain a green dependency audit.

Package security and packaged existing-data compatibility are separate concerns.

---

## 11. Build/project configuration

Application identity and platform properties live in `src/CareNest.App/CareNest.App.csproj`.

Source build policy includes strict nullable/analyzer/compiler behavior and strict XAML compilation.

Use:

- `docs/CONFIGURATION_REFERENCE.md` for configuration ownership;
- `docs/setup/DEVELOPMENT.md` for development setup;
- `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md` for publish/package instructions;
- `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` for final package checksum/provenance evidence.

---

## 12. XAML/compiler policy

The application project enables:

```xml
<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
<WarningsAsErrors>$(WarningsAsErrors);XC0022;XC0023;XC0024;XC0025</WarningsAsErrors>
```

Rules include:

- accurate root `x:DataType` on binding-bearing pages;
- item-specific `x:DataType` in binding-bearing DataTemplates;
- typed picker item display bindings where context changes;
- typed explicit Source/RelativeSource bindings;
- typed ancestor ViewModel binding-context patterns from templates;
- no `NoWarn`, `x:Object` or `x:Null` shortcut around the intended policy.

Permanent migration evidence remains in `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

---

## 13. Core data model

CareNest organizes concepts for:

- person/profile;
- medicine;
- medicine schedules/schedule-time data;
- reminder occurrence;
- medication log/history;
- appointment;
- document metadata/tags;
- stock/refill information;
- application settings/supporting local data where applicable.

Exact entities, keys, relationships, indexes and migrations are documented in `docs/architecture/DATABASE_SCHEMA.md`.

---

## 14. Reminder architecture

Reminder scheduling crosses three separate state surfaces:

1. explicit user schedule intent;
2. persisted CareNest reminder-occurrence state;
3. operating-system scheduled request state.

These cannot be committed atomically as one database transaction, so CareNest uses deterministic planning, reconciliation, cancellation-first ordering and compensation/recovery.

Core rules include:

- schedules come from explicit user-entered values;
- entity ownership/state/date/time-zone validation;
- deterministic UTC planning windows;
- half-open window boundaries;
- stable occurrence identity;
- inactive/archive suppression;
- deterministic DST gap/overlap behavior;
- no invented reminder time for invalid DST-gap input;
- future-UTC snooze contract;
- `SnoozedUntilUtc` as effective due time while snoozed;
- stale OS-request reconciliation after edits/restarts;
- cancellation before replacement/suppression/handled-state persistence where required;
- compensation/rebuild attempts after later failure.

See `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`.

---

## 15. Reminder states

CareNest can organize states such as:

- scheduled;
- snoozed;
- taken;
- skipped;
- delayed;
- missed;
- cancelled.

These are organizational application states and do not independently prove ingestion, adherence, medical correctness or treatment effectiveness.

---

## 16. Appointment architecture

Appointments store explicit instants. UTC fields must contain genuine UTC values rather than relabeled local/unspecified ticks.

Optional appointment reminders originate from explicit user configuration. Permission denial or OS scheduling failure is not treated as guaranteed delivery.

Persistence/platform scheduling uses compensation where separate state surfaces can fail independently.

---

## 17. Stock/refill organization

Stock/refill functionality is organizational and based on user-entered quantities/records.

It must not become a clinical prescription, dosage, refill-authorization or treatment recommendation engine.

Any future pharmacy/order integration would require a separate network/privacy/legal/store/security design.

---

## 18. SQLite architecture

Structured data is stored locally in SQLite and protected primarily by the application/device sandbox and device security.

CareNest does **not** claim transparent whole-database encryption.

Persistence rules include:

- explicit ordered migrations;
- transactions for consistency-sensitive multi-step changes;
- repository abstractions rather than direct ViewModel SQL;
- parameterized persistence operations;
- WAL/snapshot/integrity support for backup where documented;
- cleanup/compensation across database/filesystem/secure-store/platform surfaces.

---

## 19. Encrypted document vault

Imported document payloads use separately encrypted application-owned storage.

Important behaviors include:

- master key ownership via platform secure storage where applicable;
- authenticated encryption;
- chunked framing v2 for new payloads;
- authenticated terminal/truncation/trailing-data protection;
- retained documented legacy read compatibility;
- fail-closed behavior when required existing key material is missing/corrupt;
- explicit plaintext export outside the vault only by user action;
- cleanup/rollback on import/export failure paths.

See `docs/architecture/DOCUMENT_VAULT.md`.

---

## 20. Backup and restore

Backups are manually initiated and password-protected.

Design includes:

- password-derived key material;
- authenticated encryption;
- versioned format;
- strict decrypted archive topology validation;
- SQLite snapshot/integrity checks;
- recovery material needed for encrypted documents where documented;
- wrong-password/tamper/truncation/trailing-data rejection;
- rollback/cleanup after failed restore.

See `docs/architecture/BACKUP_AND_RESTORE.md`.

---

## 21. App lock

Optional app lock is a local privacy barrier, not whole-database/device encryption.

The design includes:

- no plaintext PIN storage;
- random salt;
- PBKDF2-HMAC-SHA256 verifier behavior;
- fixed-time comparison;
- strict stored verifier/salt validation;
- secure-store ownership;
- rollback around multi-key changes;
- fail-closed corrupt/missing state.

A rooted/jailbroken/fully compromised device remains outside the guarantee.

---

## 22. Reports and exports

Exports are explicit user actions and may include CSV/PDF/JSON or platform integrations where supported.

Rules include:

- neutralize spreadsheet formula-like user content where needed;
- use partial staging plus atomic final move for generated outputs where documented;
- best-effort cleanup of application-owned temporary files;
- maintain medical/privacy limitation messaging;
- external copies leave CareNest control after handoff.

See `docs/REPORTS_AND_EXPORTS.md`.

---

## 23. Privacy model

Current v1 intentionally has:

- no required CareNest account/backend;
- no automatic CareNest cloud sync/upload;
- no hidden runtime analytics/telemetry client;
- local structured storage;
- separately encrypted imported document payloads;
- encrypted manual backups;
- explicit outbound export/share/calendar/browser actions;
- privacy-minimized diagnostic logging.

Repository storefront links do not change these health-data guarantees. CareNest does not automatically upload health records to Gumroad or Buy Me a Coffee.

See:

- `PRIVACY.md`;
- `docs/privacy/PRIVACY_MODEL.md`;
- `docs/privacy/DATA_LIFECYCLE.md`.

---

## 24. Logging policy

Do not log:

- raw health text;
- document contents;
- passwords/PINs;
- backup secrets;
- cryptographic keys;
- signing credentials;
- unnecessary full sensitive-path exception contents.

Use privacy-minimized operation/category context and exception types where sufficient.

See `docs/security/LOGGING_PRIVACY.md`.

---

## 25. Security model

Security controls include:

- OS sandbox/device security;
- encrypted document payloads;
- password-encrypted manual backups;
- platform secure storage for secret material where applicable;
- optional app-lock privacy barrier;
- strict backup/archive validation;
- authenticated stream framing;
- dependency auditing;
- CodeQL;
- source-policy/architecture/privacy contracts;
- repository-commerce runtime-isolation contracts;
- package forbidden-marker scanning;
- structured final-package evidence tooling;
- exact-source release evidence.

Residual risk includes compromised devices, external exported copies, weak user-selected secrets, OS notification behavior and process termination during cross-surface compensation.

See `docs/security/SECURITY_MODEL.md` and `docs/security/THREAT_MODEL.md`.

---

## 26. Accessibility

Automated XAML/semantic/source checks do not equal real assistive-technology certification.

Manual release evidence still needs representative:

- screen-reader testing;
- large-text/scaling;
- keyboard/focus;
- light/dark/system contrast;
- reduced-motion behavior;
- color-independent state meaning;
- actionable privacy-safe errors.

Repository marketing assets should also include text alternatives and plain-text URL fallbacks.

See `docs/design/ACCESSIBILITY.md`.

---

## 27. Localization

Localization strategy, resource ownership, translation review and RTL considerations are documented in `docs/design/LOCALIZATION.md`.

A new locale requires actual translations and layout/date-time/accessibility/platform validation.

---

## 28. Development setup

Clone:

```bash
git clone https://github.com/sanskarIN/CareNest.git
cd CareNest
```

Inspect tools:

```bash
git --version
dotnet --info
dotnet workload list
```

Maintainer repository-local Git identity:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Use `docs/setup/DEVELOPMENT.md` and `docs/setup/PLATFORM_SETUP.md` for complete setup.

---

## 29. Core build/test commands

Platform-neutral build:

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

Package-evidence synthetic self-test:

```bash
python3 build/scripts/test-create-package-evidence.py
```

Android example:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android \
  -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

Complete executable/package commands are documented in `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`.

---

## 30. Source-line and structured-file quality contract

`tests/CareNest.UiTests/SourceLineQualityContractTests.cs` performs deterministic runtime source audits.

The line audit detects known defect patterns such as:

- unresolved merge-conflict markers;
- `TODO`/`FIXME`/`HACK` placeholders;
- `NotImplementedException` placeholders;
- common sync-over-async forms;
- `Thread.Sleep`/`Task.WaitAll`/`Task.WaitAny` runtime patterns;
- `throw ex;` stack-trace destruction.

Structured runtime inputs including XAML, project/XML-family files and JSON are parsed for syntactic validity.

The scanner deliberately avoids treating every current-clock read as a generic defect; time correctness belongs to semantic/time-zone contracts.

---

## 31. Gumroad/package regression contracts

`FundingLinkContractTests.cs` protects:

- required repository storefront/support visibility;
- absence from About/runtime surfaces;
- no medical/health entitlement language;
- Gumroad SVG accessibility metadata;
- repository-only asset placement.

`StoreFundingPayloadContractTests.cs` protects:

- absence of external-commerce destinations from app runtime text-like sources;
- absence from shared runtime URL constants;
- absence of obsolete external-commerce build switches;
- both default payload-scanner markers;
- UTF-8/UTF-16/ZIP scanning behavior;
- fail-closed scanner semantics.

---

## 32. Release-governance and package-evidence contracts

`ReleaseDocumentationConsistencyContractTests.cs` protects current active release documents from drifting back to superseded test counts/source claims and ensures:

- applicable current documents retain the latest fully verified source/result until a newer exact verification is real;
- both external-commerce markers remain in final-package evidence rules;
- live store declarations remain open until actually completed;
- package-evidence tooling remains integrated into release governance;
- Release Gate requires the current evidence/tooling set.

`PackageEvidenceToolContractTests.cs` protects:

- package evidence scripts/wrappers/guide existence;
- exact tag/source/HEAD/clean-workspace production requirements;
- mandatory store-safe scanner integration;
- payload hashing contracts;
- synthetic self-test coverage;
- CI syntax/self-test wiring;
- no-secret/release-boundary documentation.

---

## 33. Structured package evidence tooling

`build/scripts/create-package-evidence.py` generates JSON checksum/provenance evidence for inspection or production artifacts.

Cross-platform wrappers:

- `build/scripts/create-package-evidence.sh`;
- `build/scripts/create-package-evidence.ps1`.

Synthetic self-test:

- `build/scripts/test-create-package-evidence.py`.

Production mode requires:

- immutable `v*` source tag;
- tag SHA equals recorded source SHA;
- checked-out HEAD equals recorded source SHA;
- clean tracked Git workspace;
- non-secret real signing/notarization/store provenance description;
- successful store-safe payload scan;
- evidence output outside the package payload.

The generated JSON records per-file SHA-256 plus a top-level file/deterministic-directory payload SHA-256.

The tool does not sign packages, validate private signing credentials by itself, submit to a store, prove store approval, replace real-device testing, replace accessibility testing or replace packaged SQLite/document/backup compatibility.

See `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`.

---

## 34. Quality gates

Repository gates include:

```bash
build/scripts/quality-gate.sh
build/scripts/release-preflight.sh
```

PowerShell equivalents live beside those scripts.

Dependency audit is intentionally blocking.

Do not weaken tests/analyzers/audit/payload/evidence rules simply to obtain green status.

---

## 35. GitHub Actions

Current workflow roles include:

- `.github/workflows/ci.yml` — package-evidence Python syntax/self-test, formatting, tests, Android/Windows/Apple builds;
- `.github/workflows/codeql.yml` — CodeQL;
- `.github/workflows/dependency-review.yml` — dependency policy/audit;
- `.github/workflows/store-package-verification.yml` — store-candidate configuration builds;
- `.github/workflows/store-inspection-artifacts.yml` — internal package inspection artifacts and forbidden-marker scans;
- `.github/workflows/release-gate.yml` — production-tag policy/evidence/tooling/source-test gate;
- `.github/workflows/release-evidence.yml` — release evidence/provenance, including package-tooling self-test evidence.

Exact-source verification matters. Results from an older SHA must not be presented as proof for a newer verification-relevant source.

---

## 36. Latest fully verified Gumroad rollout baseline

Exact source:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

Verified on that exact revision:

- **122/122** unit tests;
- **39/39** integration tests;
- **175/175** UI/source-policy tests;
- **336/336** total core tests;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build;
- Android store-candidate build;
- Windows store-candidate build;
- iOS simulator store-candidate build;
- Mac Catalyst store-candidate build;
- CodeQL.

Authoritative record:

`docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`

This is the latest fully verified baseline currently recorded.

---

## 37. Current newer-source verification rule

Current `main` contains later verification-relevant changes including:

- release-documentation consistency tests;
- package-evidence tooling tests;
- package-evidence scripts/wrappers/self-test;
- CI changes;
- Release Gate changes;
- Release Evidence changes;
- current release documents consumed by the new contracts.

Therefore:

- 336/336 belongs only to `94e867...`;
- a replacement test total must not be predicted from source inspection;
- the final intended current source must complete a fresh exact-head matrix before a newer baseline is promoted.

Use `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` and `docs/releases/NEXT_STEPS.md`.

The fresh matrix requires, as applicable:

- package-evidence Python syntax/self-test;
- formatting;
- unit tests;
- integration tests;
- UI/source-policy tests;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- Store Package Configuration;
- Store Inspection Artifacts;
- CodeQL;
- unsuppressed Dependency Audit.

---

## 38. Release process

Production promotion requires an exact approved source/tag and cannot be inferred from source completeness alone.

Use:

- `docs/releases/NEXT_STEPS.md`;
- `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`;
- `docs/releases/RELEASE_PROCESS.md`;
- `docs/releases/RELEASE_CHECKLIST.md`;
- `docs/releases/QUALITY_GATE.md`;
- `docs/releases/MANUAL_TEST_MATRIX.md`;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`;
- `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`;
- `docs/releases/SECURITY_RELEASE_REVIEW.md`;
- `docs/releases/STORE_BUILD_POLICY.md`;
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md`;
- `docs/releases/STORE_POLICY_REVIEW_20260818.md`;
- `docs/releases/RELEASE_EVIDENCE.md`;
- `docs/releases/EXECUTABLE_BUILD_CHECKLIST.md`.

---

## 39. Remaining production blockers

### Fresh exact-head automation

- final verification-relevant source freeze;
- package-evidence syntax/self-test;
- actual unit/integration/UI/total test counts;
- all four Release builds;
- Store Package Configuration;
- Store Inspection Artifacts;
- CodeQL;
- unsuppressed Dependency Audit;
- exact source/run evidence.

### Device/platform validation

- Android representative devices/emulators;
- notification permission granted/denied behavior;
- actual reminder delivery/cancellation/snooze;
- alarm/battery optimization behavior;
- reboot/restart/clock/time-zone/DST recovery;
- Windows reminder/lifecycle behavior;
- real iPhone/iPad notification behavior;
- Mac Catalyst notification/lifecycle behavior.

### Packaged compatibility

Using fictional/synthetic data only:

- existing-data upgrade/readability/editability;
- SQLite integrity after packaged upgrade;
- reminder reconciliation;
- encrypted document compatibility;
- encrypted backup create/restore/wrong-password/tamper validation;
- genuine historical encrypted fixtures when real prior bytes safely exist.

### Accessibility

- screen-reader testing;
- large text;
- keyboard/focus;
- contrast across themes;
- reduced motion;
- color-independent states.

### Signing/final-package evidence

- production Android signing material outside Git;
- Apple signing/provisioning outside Git;
- Windows signing material outside Git;
- final signed packages;
- structured package evidence JSON for each production artifact;
- independent SHA-256/provenance cross-check;
- both external-commerce marker scans;
- installed-package smoke/manual checks.

### Store/publication

- current store metadata/screenshots/privacy/data-safety declarations;
- live Google Play Health apps declaration and Data safety;
- current Apple privacy/store metadata;
- current Microsoft/Partner Center privacy/store metadata where applicable;
- submission-time policy review;
- exact approved immutable `v*` tag;
- tagged release/security/dependency/package evidence;
- final publication/store-approval evidence.

---

## 40. Documentation governance

Current documentation authority:

1. `PROJECT_STATUS.md` — current state/verification boundary;
2. `docs/releases/NEXT_STEPS.md` — remaining operational work;
3. latest exact-source verification record — automated evidence;
4. `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` — current exact-head verification procedure;
5. `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — structured package evidence procedure;
6. this document — end-to-end project reference;
7. specialized subsystem docs — technical detail;
8. `GUMROAD.md` and marketing policy — storefront/package rules;
9. `what_changed.md` and `docs/history/` — continuation/history.

Historical snapshots are preserved rather than rewritten to look current.

See `docs/REPOSITORY_GOVERNANCE.md` and `docs/DOCUMENTATION_CATALOG.md`.

---

## 41. Contribution rules

Contributors must:

- use synthetic/fictional health data;
- never commit secrets/signing keys/private backups;
- preserve medical-safety boundaries;
- preserve local-first/privacy boundaries;
- preserve external-commerce package isolation;
- add/update the lowest appropriate regression tests;
- update relevant documentation in the same change series;
- not suppress legitimate failures merely to get green CI.

See `CONTRIBUTING.md`.

---

## 42. Support and official links

- **Gumroad:** https://ramsandesh.gumroad.com
- **Repository:** https://github.com/sanskarIN/CareNest
- **Creator:** https://www.github.com/sanskarIN
- **Buy Me a Coffee:** https://buymeacoffee.com/sanskarIN
- **Business:** `sanskarin@outlook.in`
- **Support:** `supportramsandesh@gmail.com`

CareNest support cannot provide diagnosis, dosage decisions, treatment recommendations or emergency care.

---

## 43. License

CareNest is licensed under Apache License 2.0.

See:

- `LICENSE`;
- `NOTICE`.

---

## 44. Current interpretation

CareNest remains `1.0.0-rc.1`.

The intended RC runtime feature scope is implemented and heavily automated. The Ram Sandesh Gumroad storefront is prominently integrated across current repository/documentation surfaces while remaining outside the CareNest health-app package under the current policy.

The latest fully verified Gumroad implementation/source-policy source is `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f` with 336/336 core tests and its recorded platform/store-candidate/CodeQL matrix.

Current `main` now includes additional verification-relevant release-documentation contracts, package-evidence tooling and CI/release workflow changes. A fresh exact-head automated matrix is required before those changes can replace the verified baseline.

Documentation completeness, source completeness and automated green builds do not imply that production signing, real-device accessibility/notification behavior, packaged compatibility, live store declarations, store approval or publication has already occurred.
