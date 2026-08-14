# CareNest Complete Project Documentation

CareNest is a local-first health organizer built with .NET 10, .NET MAUI, C#, XAML, SQLite, and platform services. This document is the canonical whole-project reference for contributors, maintainers, testers, reviewers, and release operators. It ties together the detailed documents already present under `docs/` without replacing them.

> **Medical boundary:** CareNest is organizational software. It does not diagnose conditions, calculate or infer dosage, recommend treatment, perform clinical interaction checking, create clinical risk scores, replace a clinician/pharmacist, provide emergency services, or guarantee notification delivery.

## 1. Project identity

- Product: **CareNest**
- Release line: `1.0.0-rc.1`
- Repository: `https://github.com/sanskarIN/CareNest`
- Creator profile: `https://github.com/sanskarIN`
- Business contact: `sanskarin@outlook.in`
- Support contact: `supportramsandesh@gmail.com`
- Voluntary project support: `https://buymeacoffee.com/sanskarIN`
- Watermark: `Made by the Sanskar`
- License: Apache License 2.0
- Primary branch: `main`

CareNest does not require a CareNest account or CareNest-owned backend for its current v1 scope.

## 2. What CareNest does

CareNest organizes user-entered health-related information locally on the device. Current feature groups include:

- multiple local family/person profiles;
- medicine records with opaque user-entered strength/instruction text;
- explicit medicine schedules and deterministic reminder occurrences;
- medication/reminder history states such as scheduled, snoozed, taken, skipped, delayed, missed, and cancelled;
- appointments and optional appointment reminders;
- stock/refill notes based only on user-entered quantities;
- encrypted imported document storage;
- profile photos;
- reports and per-profile structured exports;
- calendar export where supported;
- manual password-encrypted backup and restore;
- optional local app lock;
- notification diagnostics and recovery;
- theme/accessibility settings;
- local privacy cleanup/data-clear workflows;
- project/support/About surfaces.

For user-facing details see `docs/USER_GUIDE.md` and `docs/FEATURE_REFERENCE.md`.

## 3. Non-goals and safety limits

CareNest intentionally does not:

- decide what medicine a person should take;
- decide how much medicine a person should take;
- infer a schedule from medicine strength, instructions, diagnosis, symptoms, or other health text;
- recommend whether a medicine should be started, stopped, changed, combined, or avoided;
- interpret imported documents as medical advice;
- score health risk;
- provide emergency guidance as a substitute for emergency services;
- promise that an operating system will deliver a notification at an exact time;
- silently upload records to a CareNest cloud service;
- silently share data with caregivers or other people;
- silently add telemetry/analytics networking.

The code and documentation must preserve these boundaries.

## 4. Technology stack

### Core platform

- .NET 10
- .NET MAUI
- C#
- XAML
- MVVM-style presentation separation

### Data and persistence

- `sqlite-net-pcl`
- SQLite local structured database
- explicit schema migrations
- WAL mode and busy-timeout configuration
- transactional repository operations for multi-step consistency-sensitive changes

### Cryptography

- built-in .NET cryptographic primitives
- AES-256-GCM for encrypted document/backup streams
- PBKDF2-HMAC-SHA256 where password/PIN derivation is required
- chunked authenticated stream framing v2 for new encrypted document/backup payloads
- legacy framing v1 read compatibility where required for existing data

### Testing and automation

- xUnit
- GitHub Actions
- CodeQL
- unsuppressed NuGet dependency auditing
- repository/source policy tests
- architecture tests
- release workflow/script contract tests

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

### `CareNest.Shared`

Lowest-level shared primitives/constants/helpers that can be reused without depending on MAUI or persistence.

### `CareNest.Domain`

Framework-independent entities, enums, validation rules, and health-organizer data shape. Domain rules validate structural correctness but do not perform medical inference.

### `CareNest.Application`

Platform-neutral application contracts and orchestration. This layer owns use-case services, reminder planning/coordinating behavior, repository/service interfaces, scheduling boundaries, and compensation/recovery logic that should remain testable without MAUI.

### `CareNest.Infrastructure`

Persistence, SQLite, migrations, repositories, encryption, backup/restore, reports/exports, filesystem-oriented services, and other platform-neutral implementation concerns.

### `CareNest.App`

MAUI composition and presentation: XAML views, ViewModels, navigation, dependency injection, themes, platform adapters, notification services, file/share/browser/calendar integration, startup recovery, secure-storage adapters, and Android/iOS/Mac Catalyst/Windows target code.

### Tests

- `CareNest.UnitTests` — deterministic domain/application/service behavior.
- `CareNest.IntegrationTests` — SQLite, encryption, backup, document, report, and persistence integration.
- `CareNest.UiTests` — XAML/source/repository/architecture/security/release-policy contract tests. These are not a substitute for full real-device UI automation.

Detailed architecture: `docs/architecture/ARCHITECTURE.md`.

## 6. Dependency direction

The intended dependency direction is:

```text
Shared <- Domain <- Application <- Infrastructure <- App
```

Platform-neutral projects must not gain an accidental MAUI dependency. ViewModels must not issue SQL directly. Runtime local-first v1 code must not casually introduce HTTP/telemetry clients.

Architecture contracts under `tests/CareNest.UiTests` protect these rules.

## 7. Core data model

CareNest structured records include the concepts required for:

- people/profiles;
- medicines;
- medicine schedules;
- schedule times;
- reminder occurrences;
- medication logs;
- appointments;
- document metadata/tags;
- stock/refill information;
- contacts/settings/audit data where applicable.

The database schema, relationships, indexes, migration order, WAL model, and versioning rules are documented in `docs/architecture/DATABASE_SCHEMA.md`.

## 8. Reminder scheduling model

CareNest reminder scheduling separates three concepts:

1. **user intent** — explicit schedule values entered by the user;
2. **persisted CareNest occurrence state** — SQLite `ReminderOccurrence`-style records;
3. **operating-system request state** — platform notification/alarm registrations.

These are separate state surfaces and cannot be treated as one atomic transaction.

### Deterministic planner rules

The planner uses only explicit values and validates:

- profile/medicine/schedule ownership;
- schedule kind;
- explicit times/intervals/weekdays/cycles;
- explicit time-zone ID;
- UTC planning windows;
- half-open window boundaries;
- active profile/medicine/schedule state;
- date bounds;
- stable occurrence identity;
- duplicate-time deduplication;
- deterministic DST behavior.

Invalid DST-gap local times are not silently replaced with invented clock times.

### Effective due time

- normal scheduled occurrence: `ScheduledUtc`;
- valid snoozed occurrence: `SnoozedUntilUtc`.

A future snooze remains future even after its original scheduled time has passed.

### Platform reconciliation

CareNest attempts to cancel an old platform request before replacement, suppression, invalidation, or handled-state persistence where required. Cancellation failure remains retryable rather than being hidden as successful reconciliation.

Medicine/profile/appointment persistence flows use compensation where database state and OS scheduler state cannot succeed atomically.

### Handled reminder actions

Taken, Skipped, Delayed, Missed, Snoozed, and Cancelled use cancellation-first ordering. Later essential failure attempts non-cancelled restoration/rebuild so the app does not knowingly leave contradictory persisted/platform state.

Detailed contract: `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` and `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`.

## 9. Appointment model

Appointments store an explicit UTC instant. `StartsUtc` must actually be `DateTimeKind.Utc`; local/unspecified values are rejected rather than relabeled.

Appointment reminder lead time comes from the explicit stored UTC instant plus the user-entered lead-minutes value. Notification permission denial is not represented as successful platform scheduling.

Background rebuild does not repeatedly prompt for notification permission.

## 10. SQLite persistence model

CareNest structured information is local SQLite data protected primarily by the app sandbox/device security. The project does **not** claim transparent whole-database encryption.

Important persistence behavior includes:

- schema versioning;
- ordered migrations;
- transactional migration/version writes;
- WAL mode;
- busy timeout;
- parameterized repository operations;
- atomic multi-step repository helpers;
- snapshot/integrity checks for backup preparation;
- compensating cleanup when filesystem/secure-store/platform surfaces participate in the same user flow.

## 11. SQLite dependency security

The formerly tracked `GHSA-2m69-gcr7-jv3q` source exception is remediated in the verified graph.

Current package intent includes:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11` as the compatible bundle API path;
- central transitive pinning enabled;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- selected provider packages `2.1.12`;
- no former exact advisory `NuGetAuditSuppress` entry.

`SqliteDependencySecurityContractTests` protects the maintained dependency floor and suppression absence.

A clean dependency audit is not the same thing as packaged existing-database compatibility. Real packaged upgrade compatibility remains a separate release gate.

See `docs/security/DEPENDENCY_RISK_REGISTER.md` and `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

## 12. Encrypted document vault

Imported document payloads are stored separately from normal structured metadata and protected with authenticated encryption.

Important properties:

- application-owned 32-byte document master key stored via platform secure storage;
- new encrypted streams use chunked AEAD framing v2;
- terminal state is authenticated in v2;
- trailing data is rejected;
- legacy v1 remains readable for compatibility;
- missing/corrupt key with existing encrypted payloads fails closed;
- read/export does not silently create an unrelated replacement key;
- failed import/export paths attempt cleanup/rollback;
- explicit decrypted export creates plaintext outside the encrypted vault boundary.

See `docs/architecture/DOCUMENT_VAULT.md`.

## 13. Backup and restore

Backups are manually initiated and password-protected.

Important properties:

- password-derived AES key using PBKDF2-HMAC-SHA256;
- authenticated encryption;
- versioned package format;
- strict decrypted archive topology validation;
- database snapshot integrity checks;
- protected inclusion of document-recovery key material when needed;
- wrong-password/tamper/truncation/trailing-data rejection;
- backup/restore primary completion distinguished from later best-effort bookkeeping;
- rollback attempts restore prior secure-store key bytes after failed restore.

See `docs/architecture/BACKUP_AND_RESTORE.md`.

## 14. App lock

The optional app lock is a local privacy barrier, not whole-database encryption.

Controls include:

- numeric PIN policy;
- random salt;
- PBKDF2-HMAC-SHA256 verifier derivation;
- fixed-time comparison;
- platform secure storage;
- strict salt/verifier length validation;
- mutable verifier-buffer clearing where practical;
- rollback around multi-key update/disable changes;
- fail-closed corrupt/missing material.

## 15. Reports and exports

CareNest can create portable user-controlled outputs such as CSV, PDF, JSON, documents, and calendar data where supported.

Important boundaries:

- explicit user action controls export/share;
- CSV formula-like user strings are neutralized in portable spreadsheet output;
- CSV/PDF/JSON writers use staged partial files followed by atomic final move;
- failed/cancelled staging is cleaned best effort;
- shared application-owned report cache files are removed after share handoff where CareNest still owns the temporary copy;
- CareNest cannot delete copies already controlled by an external app, cloud location, screenshot, backup, or OS service.

See `docs/REPORTS_AND_EXPORTS.md`.

## 16. Privacy model

The current v1 privacy model is local-first:

- no required CareNest account/backend;
- no automatic CareNest cloud upload;
- no hidden analytics/telemetry client;
- local structured storage;
- encrypted imported document payloads;
- encrypted manual backups;
- explicit outbound export/share/calendar/browser boundaries;
- generic notification labels by default;
- privacy-minimized logs.

See `PRIVACY.md`, `docs/privacy/PRIVACY_MODEL.md`, and `docs/privacy/DATA_LIFECYCLE.md`.

## 17. Logging policy

Sensitive health data, raw document content, secrets, PINs, backup passwords, cryptographic keys, and routine full exception messages/stack traces must not be written to normal diagnostic logs.

Sensitive operation logging uses safe operation/category metadata and exception type names where needed.

See `docs/security/LOGGING_PRIVACY.md`.

## 18. Security model

Security controls include:

- OS sandbox/device security;
- encrypted document payloads;
- encrypted manual backups;
- secure-store protected secret material;
- strict backup topology validation;
- authenticated encrypted stream framing;
- dependency auditing;
- CodeQL;
- privacy-aware logging contracts;
- repository secret/signing-file policy tests;
- architecture/network-boundary contracts;
- exact-source release verification.

Residual risks include compromised/rooted/jailbroken devices, exported plaintext copies, weak user-selected secrets, OS notification behavior, external destinations, and process/OS termination during cross-surface compensation.

See `SECURITY.md`, `docs/security/SECURITY_MODEL.md`, and `docs/security/THREAT_MODEL.md`.

## 19. Platform behavior

### Android

Release/manual validation must account for notification permission, exact/inexact alarm capability, battery optimization, reboot, clock/time-zone changes, force-stop behavior, device/vendor background policy, and receiver lifecycle. Android recovery uses `BroadcastReceiver.GoAsync()` lifetime protection for async work.

### Windows

The current notification fallback has in-process limitations. Timer ownership/replacement/cancellation behavior is protected against same-ID races, but closed-app behavior must be documented/tested manually.

### iOS/iPadOS

Local notification behavior is controlled by Apple permission and OS policies. Real-device release testing and signing/provisioning are required.

### Mac Catalyst

Apple notification behavior plus desktop keyboard/focus/window/file behavior requires manual validation.

## 20. Accessibility and design

CareNest supports system/light/dark presentation and contains accessibility-oriented semantics/contracts. Automated source checks do not certify real assistive-technology usability.

Manual release testing must cover:

- screen readers;
- large text/text scaling;
- keyboard/focus order;
- contrast;
- theme behavior;
- reduced motion;
- color-independent status/validation cues.

See `docs/design/ACCESSIBILITY.md` and `docs/design/DESIGN_SYSTEM.md`.

## 21. Localization

Localization architecture and future RTL requirements are documented in `docs/design/LOCALIZATION.md`. Adding a locale requires translation-resource review, date/time presentation review, layout review, accessibility checks, and target testing.

## 22. Development setup

Clone and enter the repository:

```bash
git clone https://github.com/sanskarIN/CareNest.git
cd CareNest
```

Inspect toolchain:

```bash
git --version
dotnet --info
dotnet workload list
```

Requested repository-local maintainer Git identity:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Repository helpers:

```bash
build/scripts/setup-git.sh
```

```powershell
./build/scripts/setup-git.ps1
```

The scripts locate the repository root, use `--local`, verify the configured values, and fail on native Git errors.

Full setup: `docs/setup/DEVELOPMENT.md` and `docs/setup/PLATFORM_SETUP.md`.

## 23. Core build and test commands

Platform-neutral examples:

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release

dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

Local quality gate:

```bash
build/scripts/quality-gate.sh
```

```powershell
./build/scripts/quality-gate.ps1
```

Release preflight:

```bash
build/scripts/release-preflight.sh
```

```powershell
./build/scripts/release-preflight.ps1
```

Dependency audit is blocking; it must not be made warning-only to obtain a green result.

## 24. MAUI target builds

Use `CareNestTargetFramework` to narrow the active MAUI target so a target-specific host does not evaluate unrelated workloads or leak the app TFM into platform-neutral projects.

Android example:

```bash
dotnet workload install maui-android
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

Windows, iOS simulator, and Mac Catalyst commands are documented in `docs/setup/PLATFORM_SETUP.md`.

## 25. Automated verification baseline

The current authoritative automated release-engineering baseline is marker-only PR #56.

Frozen source/base:

`4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`

Marker head:

`e3bc621cea05364a69abee0dadbd71a67c17bddb`

Evidence:

- CareNest CI #571 / run `31770929379`: success;
- formatting: success;
- unit tests: 122 passed;
- integration tests: 39 passed;
- UI-contract/policy tests: 124 passed;
- total core tests: 285 passed;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #571 / run `31770929382`: success;
- unsuppressed Dependency Audit #41 / run `31770929383`: success.

PR #56 was closed without merge; its verification marker is not part of `main`.

See `docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md`.

## 26. Release workflows

Production tags matching `v*` are configured to run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Release Evidence records source/ref/run identity, tracked-file manifests/checksums, all core TRX suites, dependency inventories, workspace integrity, and evidence checksums. Available evidence is uploaded before the aggregate failure gate so a failed evidence run remains diagnosable.

A tag is not automatically production approval.

## 27. Release process

Before public `1.0.0` promotion, complete all applicable manual and external gates, including:

- Android/Windows/iOS/Mac Catalyst manual matrix;
- real notification permission/delivery/restart/reconciliation checks;
- packaged SQLite existing-data compatibility;
- encrypted document/backup compatibility;
- clean-install restore;
- accessibility;
- current Apple/Google store-policy review;
- signing identities kept outside Git;
- signed package generation/inspection;
- store metadata/screenshots/privacy/data-safety disclosures;
- exact production-tag workflows;
- final Release Evidence;
- version/build metadata, checksums, release notes, tag and publication.

See `docs/releases/RELEASE_PROCESS.md`, `RELEASE_CHECKLIST.md`, `MANUAL_TEST_MATRIX.md`, `SECURITY_RELEASE_REVIEW.md`, `STORE_SUBMISSION_CHECKLIST.md`, and `NEXT_STEPS.md`.

## 28. Dependency update process

For any package change:

1. review release notes/security implications;
2. update central package configuration deliberately;
3. run restore/build/test and unsuppressed audit;
4. run applicable platform builds;
5. update compatibility documentation if persistence/crypto/platform behavior can change;
6. create a new exact-head verification if verification-relevant source changed;
7. do not restore broad audit suppressions merely to make automation green.

SQLite provider/native changes additionally require packaged existing-data/encrypted-data compatibility evidence.

## 29. Database migration process

For a schema change:

1. add an ordered migration;
2. keep migration DDL and schema-version update transactionally coordinated;
3. preserve upgrade paths from supported prior schema versions;
4. add integration tests;
5. update `DATABASE_SCHEMA.md`;
6. review backup/restore compatibility;
7. update privacy/data-lifecycle documentation for new data categories;
8. run exact-source verification before production promotion.

## 30. Encryption format change process

Before changing encrypted document/backup framing, key derivation, package layout, or key ownership:

- update the security and threat models;
- define read/write compatibility explicitly;
- retain canonical historical fixtures where possible;
- add tamper/truncation/trailing-data tests;
- test wrong-password/key behavior;
- test clean-install restore and existing-data access;
- document migration/rollback behavior;
- do not silently remove legacy read support without proven migration/recovery.

## 31. Logging/error change process

When adding logs:

- prefer fixed operation/category text;
- avoid user-entered health content;
- avoid raw document/backup data;
- avoid secrets/PINs/passwords/keys;
- avoid record identifiers where they are not essential;
- do not log full sensitive-path exception objects/messages/stack traces unless explicitly reviewed and privacy-safe;
- preserve cancellation propagation.

## 32. Contributing

Before submitting a change:

- follow `CONTRIBUTING.md`;
- preserve architecture direction;
- preserve medical-safety boundaries;
- preserve local-first/privacy guarantees;
- add regression coverage at the lowest suitable layer;
- update related documentation in the same work;
- keep signing/private credentials out of Git;
- use synthetic/fictional data in tests/screenshots/examples;
- run local quality gates and applicable platform verification.

## 33. Repository governance

Important root governance files include:

- `README.md`;
- `LICENSE`;
- `NOTICE`;
- `CONTRIBUTING.md`;
- `CODE_OF_CONDUCT.md`;
- `SECURITY.md`;
- `PRIVACY.md`;
- `TERMS.md`;
- `SUPPORT.md`;
- `CHANGELOG.md`;
- `PROJECT_STATUS.md`;
- `DECISIONS.md`;
- `what_changed.md`.

GitHub issue/PR templates, Dependabot configuration, funding metadata, and Actions workflows live under `.github/`.

## 34. Documentation map

Start with `docs/README.md`.

Key references:

### Users

- `docs/USER_GUIDE.md`
- `docs/FEATURE_REFERENCE.md`
- `docs/REPORTS_AND_EXPORTS.md`
- `docs/GLOSSARY.md`

### Architecture

- `docs/architecture/ARCHITECTURE.md`
- `docs/architecture/APPLICATION_FLOWS.md`
- `docs/architecture/SERVICE_BOUNDARIES.md`
- `docs/architecture/DATABASE_SCHEMA.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/architecture/DOCUMENT_VAULT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`
- ADR files under `docs/architecture/`

### Privacy/security

- `docs/privacy/PRIVACY_MODEL.md`
- `docs/privacy/DATA_LIFECYCLE.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/security/THREAT_MODEL.md`
- `docs/security/LOGGING_PRIVACY.md`
- `docs/security/DEPENDENCY_RISK_REGISTER.md`

### Design/accessibility

- `docs/design/DESIGN_SYSTEM.md`
- `docs/design/ACCESSIBILITY.md`
- `docs/design/LOCALIZATION.md`
- `docs/design/STORE_ASSETS.md`

### Setup/operations

- `docs/setup/DEVELOPMENT.md`
- `docs/setup/PLATFORM_SETUP.md`
- `docs/setup/TROUBLESHOOTING.md`
- `docs/setup/MAINTAINER_OPERATIONS.md`

### Testing

- `docs/testing/TESTING_GUIDE.md`
- `docs/testing/TEST_PLAN.md`
- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`
- `docs/testing/SETTINGS_LIFECYCLE_CONTRACT.md`
- audit regression matrices under `docs/testing/`

### Release

- `docs/releases/RELEASE_PROCESS.md`
- `docs/releases/RELEASE_CHECKLIST.md`
- `docs/releases/QUALITY_GATE.md`
- `docs/releases/MANUAL_TEST_MATRIX.md`
- `docs/releases/SECURITY_RELEASE_REVIEW.md`
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md`
- `docs/releases/RELEASE_EVIDENCE.md`
- `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`
- `docs/releases/NEXT_STEPS.md`
- `docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md`

### History/handoff

- `what_changed.md`
- `docs/history/`
- dated audit/release evidence files under `docs/releases/`, `docs/security/`, and `docs/testing/`.

## 35. Troubleshooting entry points

For build/workload/project issues use `docs/setup/TROUBLESHOOTING.md`.

For reminder issues check, in order:

1. explicit schedule/profile/medicine state;
2. time zone and UTC boundaries;
3. `SnoozedUntilUtc` when snoozed;
4. notification permission/capability;
5. persisted occurrence state;
6. platform request reconciliation;
7. platform-specific battery/alarm/background/force-stop limitations;
8. startup/rebuild recovery.

For encrypted document/backup issues verify key/password/format/version/topology state and preserve the original file before attempting destructive recovery.

## 36. Data used in development/testing

Use only fictional/synthetic data in:

- automated tests;
- public issues;
- screenshots;
- documentation examples;
- store assets;
- packaged migration fixtures.

Never commit real health records, real backups, app-lock PINs, backup passwords, encryption keys, signing material, or production secret files.

## 37. Definition of source completeness

The current v1 source is considered implementation-complete for the documented local-first RC scope when:

- required source projects exist and build under supported target jobs;
- required entities/services/views/platform integrations are implemented;
- runtime placeholder/source-policy tests pass;
- core tests and release-policy contracts pass;
- security/dependency gates pass;
- documentation matches implemented behavior.

This is not the same as production-publication completeness. Manual/device/store/signing/package evidence remains separate.

## 38. Definition of production completeness

Public production promotion requires all applicable automated, manual, packaged-data, accessibility, store-policy, signing, provenance, and exact-tag evidence to be complete. Do not check a manual row merely because source/CI is green.

## 39. Current authoritative status

The current automated release-engineering source baseline is PR #56 with 285/285 core tests, all four platform Release builds, CodeQL, and unsuppressed Dependency Audit green. The project remains `1.0.0-rc.1` until the real-device/accessibility/store/signing/packaged-data/final-tag release gates are completed.

For exact evidence use `docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md`. For the operational checklist use `PROJECT_STATUS.md`, `docs/releases/RELEASE_CHECKLIST.md`, and `docs/releases/NEXT_STEPS.md`.
