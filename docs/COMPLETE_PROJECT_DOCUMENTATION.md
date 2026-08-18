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
**Current automated baseline record:** `docs/releases/AUTOMATED_BASELINE.md`  
**Current dependency/toolchain baseline:** `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`  
**Documentation integrity guide:** `docs/testing/DOCUMENTATION_INTEGRITY.md`

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

The Ram Sandesh Gumroad storefront is a repository/documentation surface:

**https://ramsandesh.gumroad.com**

Canonical files:

- `GUMROAD.md` — reader-facing storefront guide;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — placement/package policy;
- `docs/marketing/GUMROAD_ROLLOUT_CHECKLIST.md` — rollout verification checklist;
- `docs/assets/gumroad_store_badge.svg` — repository-only visual CTA;
- `docs/assets/README.md` — asset usage/accessibility rules.

A Gumroad purchase is **not** a CareNest medical/health entitlement. It does not provide or change diagnosis, dosage guidance, treatment recommendations, clinical interaction/risk behavior, reminder reliability, emergency assistance, account/cloud functionality, or local health-data access.

CareNest does not automatically transmit local health records to Gumroad or Buy Me a Coffee.

---

## 3. External-commerce package boundary

The current CareNest application runtime/package intentionally excludes repository-only commercial/funding destinations and promotional artwork.

Forbidden package markers under the current policy include:

- `ramsandesh.gumroad.com`;
- `buymeacoffee.com/sanskarIN`.

Do not place those destinations in application ViewModels/XAML, shared runtime URL constants, platform manifests/plists for promotion, packaged application resources, or app commands/buttons under the current RC1 product/store policy.

`build/scripts/verify-store-safe-payload.py` scans both markers in UTF-8, UTF-16 little-endian and UTF-16 big-endian representations, including ZIP-compatible package entries.

Repository-only support/storefront material remains allowed in documentation, GitHub metadata and repository marketing assets.

---

## 4. Product purpose and implemented source scope

CareNest helps people organize user-entered health-related information locally on their own device.

Current source-controlled scope includes:

- onboarding, safety/privacy limitations and local-first first-run flow;
- multiple person/family profiles and local caregiver-style profile switching/dashboard behavior;
- profile photos from file selection or supported camera capture;
- emergency contact organization;
- medicines and opaque user-entered strength/instruction text;
- explicit medicine schedules and schedule-time data;
- deterministic reminder occurrences;
- reminder lifecycle/history states including scheduled/snoozed/taken/skipped/delayed/missed/cancelled behavior;
- reminder diagnostics, reconciliation and recovery;
- appointments and optional reminders;
- medicine stock/refill notes and user-entered quantity tracking;
- encrypted imported-document storage, tags and explicit export/delete boundaries;
- reports and portable JSON/PDF/CSV exports;
- per-profile export/deletion flows where documented;
- manual password-encrypted backup and restore;
- optional local app lock;
- quiet hours, reminder sound/vibration and related settings;
- local privacy cleanup/data-clear workflows;
- developer/diagnostic tools that remain privacy-minimized;
- light/dark/system themes, reduced-motion/large-interface settings and accessibility-oriented source contracts;
- localization/RTL readiness documentation;
- open-source repository metadata, issue templates, PR checklist and code ownership.

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

Any future networked, in-app-commerce, cloud-sync, caregiver-sharing or clinical feature requires a separate product, privacy, security, safety, consent, legal/store and testing review.

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

### Quality/security/release automation

- xUnit
- GitHub Actions
- CodeQL
- blocking NuGet dependency auditing
- source/repository policy tests
- strict XAML compiled-binding checks
- package marker scanning
- structured package evidence self-tests
- stable documentation-link integrity tooling
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
.github/
  ISSUE_TEMPLATE/
  workflows/
```

Intended dependency direction:

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

Detailed ownership: `docs/CODEBASE_REFERENCE.md` and `docs/architecture/SERVICE_BOUNDARIES.md`.

---

## 8. Project responsibilities

### `CareNest.Shared`

Small cross-layer primitives/helpers without MAUI or persistence coupling. External-commerce URLs must not be introduced as application constants under the current package policy.

### `CareNest.Domain`

Framework-independent entities, enums and structural/domain validation. It must not become a clinical decision engine.

### `CareNest.Application`

Use-case/service contracts, repository abstractions, reminder planning/coordinating, recovery and compensation logic.

### `CareNest.Infrastructure`

SQLite, migrations, repositories, encrypted document storage, backup/restore, reports/exports and related implementations.

### `CareNest.App`

MAUI composition, XAML, ViewModels, navigation, dependency injection, themes, file/camera/share gateways, platform notification services and Android/iOS/Mac Catalyst/Windows adapters.

Repository-only Gumroad/Buy Me a Coffee promotion must not leak into this project under the current store policy.

---

## 9. Platform targets

Current targets:

- Android: `net10.0-android`, minimum API 24;
- iOS/iPadOS: `net10.0-ios`, minimum iOS 15;
- Mac Catalyst: `net10.0-maccatalyst`, minimum 15;
- Windows: `net10.0-windows10.0.19041.0`, minimum Windows 10.0.19041.0.

`CareNestTargetFramework` can isolate one target on CI/hosts that do not have every workload.

See `docs/setup/PLATFORM_SETUP.md`, `docs/PLATFORM_BEHAVIOR_MATRIX.md`, and `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`.

---

## 10. Package baseline

Central package management is configured in `Directory.Packages.props`; the current human-readable reference is `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`.

Current candidate source versions include:

- `Microsoft.Maui.Controls` `10.0.90`;
- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- Android/provider SQLite leaves `2.1.12` where pinned;
- `Microsoft.Extensions.Logging.Debug` `10.0.0`;
- `Microsoft.Extensions.Logging.Abstractions` `10.0.0`;
- `Microsoft.Extensions.DependencyInjection.Abstractions` `10.0.0`;
- `Microsoft.NET.Test.Sdk` `18.9.0`;
- `xunit` `2.9.3`;
- `xunit.runner.visualstudio` `4.0.0`;
- `coverlet.collector` `10.0.1`.

The former exact SQLite advisory suppression remains removed. Do not reintroduce it merely to obtain a green dependency audit.

Package security and packaged existing-data compatibility are separate concerns.

---

## 11. Build/project configuration

Application identity and platform properties live in `src/CareNest.App/CareNest.App.csproj`.

Source build policy includes strict nullable/analyzer/compiler behavior and strict XAML compilation.

Use:

- `docs/CONFIGURATION_REFERENCE.md` for configuration ownership;
- `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md` for current package/action versions;
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

Rules include accurate root/item binding types, typed picker/Source/ancestor bindings, and no `NoWarn`, `x:Object` or `x:Null` shortcut around the intended policy.

Permanent migration evidence remains in `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

---

## 13. Core data model

CareNest organizes concepts for:

- person/profile;
- emergency contact/profile support data;
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

Core rules include explicit user-entered schedules, ownership/state/date/time-zone validation, deterministic UTC planning windows, half-open window boundaries, stable occurrence identity, inactive/archive suppression, deterministic DST behavior, future-UTC snooze, stale OS-request reconciliation and compensation after partial failure.

See `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`.

---

## 15. Reminder states

CareNest can organize scheduled, snoozed, taken, skipped, delayed, missed and cancelled states.

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

Persistence rules include explicit ordered migrations, consistency-sensitive transactions, repository abstractions rather than direct ViewModel SQL, parameterized persistence, WAL/snapshot/integrity support for backup and cleanup/compensation across database/filesystem/secure-store/platform surfaces.

---

## 19. Encrypted document vault

Imported document payloads use separately encrypted application-owned storage.

Important behaviors include master key ownership via platform secure storage where applicable, authenticated encryption, chunked framing v2 for new payloads, terminal/truncation/trailing-data protection, retained legacy read compatibility, fail-closed missing/corrupt key behavior, explicit plaintext export and cleanup/rollback on failure.

See `docs/architecture/DOCUMENT_VAULT.md`.

---

## 20. File, photo and share gateway

`IAppFileGateway` abstracts user-initiated file selection, supported camera capture, backup selection and share operations.

The MAUI implementation:

- checks cancellation before entering picker/camera/share operations;
- re-checks cancellation after picker/camera completion before returning a selected result;
- checks cancellation before opening a picked file stream;
- disposes a just-opened stream if cancellation becomes requested during the platform open boundary;
- does not claim that the underlying OS picker UI itself can always be cancelled programmatically by a .NET token.

This preserves the application cancellation contract without overstating platform capabilities.

---

## 21. Backup and restore

Backups are manually initiated and password-protected.

Design includes password-derived key material, authenticated encryption, versioned format, strict decrypted archive topology validation, SQLite snapshot/integrity checks, encrypted-document recovery material where documented, wrong-password/tamper/truncation/trailing-data rejection and rollback/cleanup after failed restore.

See `docs/architecture/BACKUP_AND_RESTORE.md`.

---

## 22. App lock

Optional app lock is a local privacy barrier, not whole-database/device encryption.

The design includes no plaintext PIN storage, random salt, PBKDF2-HMAC-SHA256 verification, fixed-time comparison, strict stored material validation, secure-store ownership, rollback around multi-key changes and fail-closed corrupt/missing state.

A rooted/jailbroken/fully compromised device remains outside the guarantee.

---

## 23. Reports and exports

Exports are explicit user actions and may include profile JSON/PDF, medication logs, upcoming schedules, appointment history, document lists, stock/refill and missed-reminder CSV data where supported.

Rules include spreadsheet-formula neutralization where needed, safe staging/final moves, best-effort cleanup, medical/privacy limitation messaging and explicit acknowledgement that external copies leave CareNest control after handoff.

See `docs/REPORTS_AND_EXPORTS.md`.

---

## 24. Privacy model

Current v1 intentionally has:

- no required CareNest account/backend;
- no automatic CareNest cloud sync/upload;
- no hidden runtime analytics/telemetry client;
- local structured storage;
- separately encrypted imported document payloads;
- encrypted manual backups;
- explicit outbound export/share/calendar/browser actions;
- privacy-minimized diagnostic logging.

Repository storefront links do not change these health-data guarantees.

See `PRIVACY.md`, `docs/privacy/PRIVACY_MODEL.md`, and `docs/privacy/DATA_LIFECYCLE.md`.

---

## 25. Logging policy

Do not log raw health text, document contents, passwords/PINs, backup secrets, cryptographic keys, signing credentials or unnecessary sensitive-path exception contents.

Use privacy-minimized operation/category context and exception types where sufficient.

See `docs/security/LOGGING_PRIVACY.md`.

---

## 26. Security model

Security controls include OS sandbox/device security, encrypted document payloads, password-encrypted manual backups, platform secure storage for secret material where applicable, optional app-lock privacy barrier, strict archive validation, authenticated stream framing, dependency auditing, CodeQL, source-policy/privacy contracts, package forbidden-marker scanning, structured final-package evidence tooling, documentation integrity and exact-source release evidence.

Residual risk includes compromised devices, external exported copies, weak user-selected secrets, OS notification behavior and process termination during cross-surface compensation.

See `docs/security/SECURITY_MODEL.md` and `docs/security/THREAT_MODEL.md`.

---

## 27. Accessibility

Automated XAML/semantic/source checks do not equal real assistive-technology certification.

Manual release evidence still needs representative screen-reader testing, large text/scaling, keyboard/focus, light/dark/system contrast, reduced-motion behavior, color-independent state meaning and actionable privacy-safe errors.

See `docs/design/ACCESSIBILITY.md`.

---

## 28. Localization

Localization strategy, resource ownership, translation review and RTL considerations are documented in `docs/design/LOCALIZATION.md`.

A new locale requires actual translations and layout/date-time/accessibility/platform validation.

---

## 29. Development setup

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

Maintainer repository-local Git identity convention:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Connector/API-created commits must be judged by their actual Git metadata; a connector that does not expose author-email fields cannot be assumed to apply local Git identity configuration.

Use `docs/setup/DEVELOPMENT.md` and `docs/setup/PLATFORM_SETUP.md` for complete setup.

---

## 30. Core build/test commands

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

Package-evidence self-test:

```bash
python3 build/scripts/test-create-package-evidence.py
```

Documentation-integrity self-test and stable check:

```bash
python3 build/scripts/test-verify-documentation-links.py
python3 build/scripts/verify-documentation-links.py
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

## 31. Source-line and structured-file quality contract

`tests/CareNest.UiTests/SourceLineQualityContractTests.cs` performs deterministic runtime source audits.

The line audit detects known defect patterns such as unresolved merge-conflict markers, `TODO`/`FIXME`/`HACK` placeholders, `NotImplementedException` placeholders, common sync-over-async forms, blocking thread/task patterns and `throw ex;` stack-trace destruction.

Structured runtime inputs including XAML, project/XML-family files and JSON are parsed for syntactic validity.

Time correctness remains the responsibility of semantic/time-zone contracts rather than a blanket clock-read ban.

---

## 32. Gumroad/package regression contracts

`FundingLinkContractTests.cs` protects required repository storefront/support visibility, absence from About/runtime surfaces, no medical/health entitlement language, Gumroad SVG accessibility metadata and repository-only asset placement.

`StoreFundingPayloadContractTests.cs` protects absence of external-commerce destinations from app runtime text-like sources/shared constants, absence of obsolete build switches, both payload-scanner markers, encoding/archive scanning and fail-closed scanner behavior.

---

## 33. Release-governance contracts

`ReleaseDocumentationConsistencyContractTests.cs` protects stable release policy from superseded intermediate claims, both external-commerce package markers, current store-policy linkage, open live store declarations, package-evidence integration, documentation-integrity tooling and Release Gate requirements.

Dynamic evidence files are deliberately not asserted for mutable SHA/count content by executable C# tests.

---

## 34. Structured package evidence tooling

`build/scripts/create-package-evidence.py` generates JSON checksum/provenance evidence for inspection or production artifacts.

Cross-platform wrappers:

- `build/scripts/create-package-evidence.sh`;
- `build/scripts/create-package-evidence.ps1`.

Synthetic self-test:

- `build/scripts/test-create-package-evidence.py`.

Production mode requires immutable `v*` source tag, tag/source/checked-out-HEAD equality, clean tracked workspace, non-secret real signing/notarization/store provenance, successful store-safe payload scan and evidence output outside the payload.

The tool does not sign packages, prove store approval, replace real-device/accessibility testing or replace packaged SQLite/document/backup compatibility.

See `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`.

---

## 35. Documentation-integrity tooling

`build/scripts/verify-documentation-links.py` verifies repository-local Markdown/HTML-reference targets offline.

It fails closed for missing local targets and repository-escaping paths while skipping external network availability.

Default exact-source scope excludes:

- immutable `docs/history/` snapshots;
- `PROJECT_STATUS.md`;
- `what_changed.md`;
- `docs/releases/AUTOMATED_BASELINE.md`;
- `docs/releases/NEXT_STEPS.md`.

The four non-history files are dynamic post-verification evidence/status records. Use `--include-dynamic` for an explicit documentation-only audit without turning mutable run IDs/test counts/source SHAs into an infinite exact-source verification loop.

Source/self-test contracts are protected by `DocumentationIntegrityToolContractTests.cs`.

See `docs/testing/DOCUMENTATION_INTEGRITY.md`.

---

## 36. Quality gates

Repository gates include:

```bash
build/scripts/quality-gate.sh
build/scripts/release-preflight.sh
```

PowerShell equivalents live beside those scripts.

Dependency audit, relevant source-policy tests, documentation integrity, package scanner/evidence checks and release gates are intentionally fail-closed.

Do not weaken tests/analyzers/audit/payload/evidence rules simply to obtain green status.

---

## 37. GitHub Actions

Current maintained action majors include:

- `actions/checkout@v7`;
- `actions/setup-dotnet@v6`;
- `github/codeql-action@v4` components;
- `actions/upload-artifact@v7` where artifacts are uploaded.

Current workflow roles include:

- `.github/workflows/ci.yml` — repository Python tooling syntax/self-tests, stable documentation integrity, formatting, tests, Android/Windows/Apple builds;
- `.github/workflows/codeql.yml` — CodeQL;
- `.github/workflows/dependency-review.yml` — dependency policy/audit;
- `.github/workflows/store-package-verification.yml` — store-candidate configuration builds;
- `.github/workflows/store-inspection-artifacts.yml` — internal package inspection artifacts and forbidden-marker scans;
- `.github/workflows/release-gate.yml` — production-tag policy/evidence/tooling/source-test gate;
- `.github/workflows/release-evidence.yml` — release evidence/provenance, including package/documentation tooling evidence.

Exact-source verification matters. Results from an older SHA must not be presented as proof for a newer verification-relevant source.

---

## 38. Open-source repository maintenance

Current repository-community surfaces include:

- `CONTRIBUTING.md`;
- `CODE_OF_CONDUCT.md`;
- `SECURITY.md`;
- `SUPPORT.md`;
- `.github/ISSUE_TEMPLATE/bug_report.yml`;
- `.github/ISSUE_TEMPLATE/feature_request.yml`;
- `.github/ISSUE_TEMPLATE/config.yml` with private security-advisory routing and support/privacy links;
- `.github/PULL_REQUEST_TEMPLATE.md` with safety/privacy/migration/testing/platform/release checks;
- `.github/CODEOWNERS` with default ownership.

Blank issues are disabled so public reports are routed through safer templates/contact links. Security reports should use private advisory/reporting paths rather than public issues, especially when sensitive information could be involved.

---

## 39. Automated verification authority

The current mutable automated evidence authority is:

`docs/releases/AUTOMATED_BASELINE.md`

It must record exact source/base SHA, verification PR/head marker SHA, workflow run IDs, actual observed test counts and conclusions.

Permanent historical evidence remains available under dated release records. For example, `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md` records the verified `94e867...` Gumroad implementation/source-policy baseline and its 336/336 core-test result.

Do not predict or transfer that count to the current package/toolchain/source candidate. The final exact-head matrix must produce the replacement count.

---

## 40. Exact-head verification rule

Verification-relevant current changes include dependency updates, workflow action updates, MAUI file gateway behavior, package/documentation evidence tooling, source-policy tests and stable documentation/configuration changes.

Use `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

The fresh matrix requires, as applicable:

- repository Python syntax/self-tests;
- stable documentation local-link integrity;
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

No replacement test total is valid until observed from that run.

---

## 41. Release process

Production promotion requires an exact approved source/tag and cannot be inferred from source completeness alone.

Use:

- `docs/releases/AUTOMATED_BASELINE.md`;
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

## 42. Remaining production blockers

Even after final in-repository automation is green, production still requires applicable external/manual evidence.

### Device/platform validation

- Android representative devices/emulators;
- notification permission granted/denied behavior;
- actual reminder delivery/cancellation/snooze;
- alarm/battery optimization behavior;
- reboot/restart/clock/time-zone/DST recovery;
- Windows installed/lifecycle/reminder behavior;
- real iPhone/iPad notification behavior;
- Mac Catalyst notification/lifecycle behavior.

### Packaged compatibility

Using fictional/synthetic data only:

- existing-data upgrade/readability/editability;
- SQLite integrity after packaged upgrade;
- reminder reconciliation;
- encrypted document compatibility;
- encrypted backup create/restore/wrong-password/tamper/truncation/trailing-data validation;
- genuine historical encrypted fixtures when real prior bytes safely exist.

### Accessibility

- representative screen readers;
- large text;
- keyboard/focus;
- contrast across themes;
- reduced motion;
- color-independent states.

### Signing/final-package evidence

- Android production signing material outside Git;
- Apple signing/provisioning outside Git;
- Windows signing material outside Git where applicable;
- final signed packages;
- structured package evidence JSON for each production artifact;
- independent SHA-256/provenance cross-check;
- both external-commerce marker scans;
- installed-package smoke/manual checks.

### Store/publication

- live Google Play Health apps declaration and Data safety;
- current Apple privacy/store metadata;
- current Microsoft/Partner Center metadata where applicable;
- current store screenshots/listing copy using fictional data;
- submission-time policy review;
- exact approved immutable `v*` tag;
- tagged CI/security/dependency/package/release evidence;
- final store approval/publication evidence.

---

## 43. Documentation governance

Current documentation authority:

1. `PROJECT_STATUS.md` — current status/verification boundary;
2. `docs/releases/AUTOMATED_BASELINE.md` — latest actual automated evidence record;
3. `docs/releases/NEXT_STEPS.md` — remaining operational work;
4. latest exact-source dated verification record — permanent automated evidence;
5. `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` — exact-head procedure;
6. `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — structured package evidence procedure;
7. `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md` — package/action source baseline;
8. `docs/testing/DOCUMENTATION_INTEGRITY.md` — stable/dynamic/history documentation verification boundary;
9. this document — end-to-end project reference;
10. specialized subsystem docs — technical detail;
11. `GUMROAD.md` and marketing policy — storefront/package rules;
12. `what_changed.md` and `docs/history/` — continuation/history.

Historical snapshots are preserved rather than rewritten to look current.

---

## 44. Contribution rules

Contributors must:

- use synthetic/fictional health data;
- never commit secrets/signing keys/private backups;
- preserve medical-safety boundaries;
- preserve local-first/privacy boundaries;
- preserve external-commerce package isolation;
- add/update the lowest appropriate regression tests;
- update relevant documentation in the same change series;
- run documentation integrity checks for documentation/path changes;
- not suppress legitimate failures merely to get green CI.

See `CONTRIBUTING.md` and `.github/PULL_REQUEST_TEMPLATE.md`.

---

## 45. Support and official links

- **Gumroad:** https://ramsandesh.gumroad.com
- **Repository:** https://github.com/sanskarIN/CareNest
- **Creator:** https://www.github.com/sanskarIN
- **Buy Me a Coffee:** https://buymeacoffee.com/sanskarIN
- **Business:** `sanskarin@outlook.in`
- **Support:** `supportramsandesh@gmail.com`

CareNest support cannot provide diagnosis, dosage decisions, treatment recommendations or emergency care.

---

## 46. License

CareNest is licensed under Apache License 2.0.

See `LICENSE` and `NOTICE`.

---

## 47. Current interpretation

CareNest remains `1.0.0-rc.1`.

The intended RC runtime feature scope is implemented and heavily automated. The Ram Sandesh Gumroad storefront is integrated across repository/documentation surfaces while remaining outside the CareNest health-app package under the current policy.

The current candidate source now includes updated .NET MAUI/test/coverage dependencies, maintained GitHub Actions majors, MAUI file/camera cancellation hardening, deterministic package evidence tooling, stable documentation-integrity tooling, stronger release contracts and improved open-source issue/PR ownership metadata.

Those verification-relevant changes require a fresh exact-head automated matrix before the mutable `docs/releases/AUTOMATED_BASELINE.md` record can promote them as the latest verified source.

Documentation/source completeness and automated green builds do not imply production signing, real-device accessibility/notification behavior, packaged compatibility, live store declarations, store approval or publication has already occurred.
