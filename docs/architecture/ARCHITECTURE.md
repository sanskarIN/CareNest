# CareNest Architecture

CareNest `1.0.0-rc.1` is a local-first .NET MAUI application for organizing multiple local profiles, user-entered medicine schedules/reminders, appointments, encrypted health documents, stock/refill notes, reports/exports, and password-encrypted manual backups.

> **Safety boundary:** CareNest is organizational software. It does not diagnose, calculate/infer dosage, recommend treatment, perform clinical interaction checking, create clinical risk scores, replace qualified professionals, provide emergency services, or guarantee notification delivery.

## Current authoritative automated architecture baseline

Marker-only PR #56 verified the current release-engineering source:

- source/base: `4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`;
- marker head: `e3bc621cea05364a69abee0dadbd71a67c17bddb`;
- CareNest CI #571 / `31770929379`: success;
- 122 unit + 39 integration + 124 UI-contract/policy = **285/285** tests;
- Android/Windows/iOS simulator/Mac Catalyst Release builds: success;
- CodeQL #571 / `31770929382`: success;
- unsuppressed Dependency Audit #41 / `31770929383`: success.

PR #56 was closed without merge and its marker is not part of `main`.

PR #54 remains the historical runtime bug-audit baseline for the earlier 261-test source boundary.

## Architecture goals

CareNest v1 architecture prioritizes:

- no required CareNest account/backend;
- no automatic CareNest cloud synchronization;
- local structured persistence;
- encrypted imported document payloads;
- password-encrypted manual backups;
- deterministic reminder materialization from explicit user values;
- explicit reconciliation between persisted reminder state and OS scheduled requests;
- platform notification abstraction/limitations;
- MVVM-style presentation separation;
- platform-neutral/testable domain/application/infrastructure projects;
- privacy-minimized logging;
- explicit export/share/browser/calendar boundaries;
- versioned persistence and encrypted-data compatibility;
- exact-source CI/security/release evidence.

## System context

```text
+---------------------------- User Device ----------------------------+
|                                                                     |
| CareNest MAUI UI / ViewModels                                       |
|              |                                                      |
|              v                                                      |
| Application services / reminder planner & coordinator               |
|              |                            |                          |
|              v                            v                          |
| Repository/infrastructure          Platform abstractions             |
|     |       |       |              notifications/files/share/etc.    |
|     v       v       v                                                |
|   SQLite  encrypted reports/backup                                   |
|           documents                                                  |
|                                                                     |
| Platform secure storage: app-lock and document-key material          |
+---------------------------------------------------------------------+
                 |
                 | explicit user action
                 v
        External file/calendar/browser/share destination
```

No normal current-v1 flow requires a CareNest-owned server.

## Project structure

### `CareNest.Shared`

Small dependency-light reusable primitives and canonical constants.

Responsibilities include:

- product/repository/contact/support constants;
- settings/secret key identifiers;
- guards/results/time-provider helpers.

It must remain free of MAUI and persistence implementation details.

### `CareNest.Domain`

Framework-independent entities, enums and validation rules.

Responsibilities include:

- profiles;
- medicines;
- schedules/times;
- reminder occurrences;
- medication logs;
- appointments;
- documents/tags;
- stock/refill adjustments;
- settings/audit/supporting local records;
- structural validation.

Medicine strength/instruction values remain opaque. Domain rules do not convert health text into medical decisions.

### `CareNest.Application`

Platform-neutral use-case contracts/orchestration.

Responsibilities include:

- repository/service contracts;
- deterministic reminder planning;
- reminder reconciliation/action handling;
- profile/medicine/appointment/document orchestration;
- backup reminder coordination;
- compensation/recovery across independent state surfaces;
- time-provider aware workflows.

### `CareNest.Infrastructure`

Local implementation concerns.

Responsibilities include:

- SQLite connection/configuration/migrations;
- repositories;
- transaction helpers;
- WAL checkpoint/snapshot/integrity operations;
- encrypted document storage;
- backup archive/encryption/restore;
- reports and portable output;
- shared chunked AEAD framing.

### `CareNest.App`

MAUI UI/composition/platform project.

Responsibilities include:

- XAML views;
- ViewModels;
- navigation;
- dependency injection;
- theme/accessibility presentation;
- secure-storage adapter;
- file/media picker;
- browser/share/calendar integration;
- platform notification implementations;
- notification permission/capability diagnostics;
- startup recovery;
- Android/iOS/Mac Catalyst/Windows source/resources.

### Test projects

- `CareNest.UnitTests` — deterministic domain/application/direct service tests.
- `CareNest.IntegrationTests` — SQLite/crypto/backup/document/report/reminder integration.
- `CareNest.UiTests` — XAML/source/repository/architecture/privacy/security/release-policy contracts; not a claim of full real-device UI automation.

Concrete files are mapped in `docs/CODEBASE_REFERENCE.md`.

## Dependency direction

Intended direction:

```text
Shared <- Domain <- Application <- Infrastructure <- App
```

Architecture contracts enforce that lower/platform-neutral layers do not depend upward on MAUI composition.

## MVVM/presentation boundary

Views bind to ViewModels.

ViewModels:

- do not issue SQL directly;
- do not create network clients for local-first v1;
- avoid synchronous task-blocking patterns;
- do not own platform notification internals;
- use application/navigation/platform abstractions;
- route unexpected errors through privacy-aware UI error handling.

## Core data flow

```text
View
  -> ViewModel
    -> Application service/coordinator
      -> repository/service abstraction
        -> infrastructure/platform implementation
          -> SQLite/files/secure store/OS service
```

A capability belongs in the lowest architectural layer that can own it without violating dependency direction.

## Domain/data model

Core local structured concepts include:

- `PersonProfile`;
- `Medicine`;
- `MedicineSchedule`;
- `ScheduleTime`;
- `ReminderOccurrence`;
- `MedicationLogEntry`;
- `Appointment`;
- `CareDocument`;
- `Tag` / `DocumentTag`;
- `StockAdjustment`;
- `EmergencyContact`;
- `AppSetting`;
- `AuditEntry`;
- `BackupMetadata`.

See `DATABASE_SCHEMA.md` for schema/relationship details.

## Reminder architecture

Reminder scheduling separates:

1. explicit user schedule intent;
2. persisted CareNest occurrence state;
3. operating-system scheduled request state.

These are not one atomic state surface.

### Planner

`ReminderPlanner` materializes deterministic future occurrence intent from explicit values.

Planner validation includes:

- profile → medicine ownership;
- medicine → schedule ownership;
- schedule → persisted schedule-time ownership;
- recognized schedule kind;
- explicit valid time zone;
- UTC planning bounds;
- half-open planning window `[fromUtc, toUtc)`;
- schedule/state/date boundaries;
- stable occurrence identity;
- duplicate-time deduplication;
- deterministic DST handling;
- no automatic as-needed occurrences;
- suppression for archived/paused/completed/disabled state.

Invalid DST-gap local clock values do not cause CareNest to invent a replacement reminder time.

### Coordinator

`ReminderCoordinator` coordinates persisted occurrences and platform requests.

Responsibilities include:

- load eligible profiles/medicines/schedules/times;
- materialize/upsert occurrences;
- compute effective due time;
- reconcile existing OS requests;
- apply quiet-hours/platform eligibility;
- schedule/cancel platform requests;
- process handled actions;
- record medication-log state where applicable;
- apply explicit user-configured stock bookkeeping;
- reconcile overdue state;
- keep failed cancellation/recovery retryable/visible.

### Effective due time

- normal Scheduled occurrence → `ScheduledUtc`;
- valid Snoozed occurrence → `SnoozedUntilUtc`.

The original scheduled instant remains historical schedule identity after snooze.

### Reconciliation ordering

CareNest explicitly coordinates SQLite and platform request state:

- cancel old OS request before replacement/suppression/invalidation;
- retain stale occurrence identity long enough to cancel obsolete requests after schedule edits;
- leave failed cancellation retryable;
- cancel future requests before medicine/profile cascade deletion;
- if database deletion fails after cancellation, attempt non-cancelled rebuild compensation;
- reconcile medicine/profile saves before non-critical audit bookkeeping;
- reconcile/compensate appointment persistence/platform scheduling.

### Handled action ordering

Taken, Skipped, Delayed, Missed, Snoozed and Cancelled use cancellation-first behavior:

1. validate action/snooze input;
2. cancel old platform request;
3. persist handled state only after cancellation succeeds;
4. schedule snooze replacement after state persistence;
5. if a later essential step fails, attempt non-cancelled previous-state restoration and rebuild;
6. surface aggregate recovery failure instead of claiming contradictory state is consistent.

These are data-consistency rules, not medical decisions.

## Appointment architecture

Appointments store an explicit UTC instant.

`Appointment.StartsUtc` must be actual `DateTimeKind.Utc`; local/unspecified ticks are rejected rather than relabeled.

Appointment reminder due time derives from:

- explicit stored UTC start;
- user-entered `ReminderMinutesBefore`.

Notification permission denial is not considered successful scheduling. Background rebuild does not repeatedly prompt for permission.

Appointment database state and platform notification state use compensation/reconciliation because they are separate surfaces.

## Time-zone architecture

Schedules store explicit time-zone identity.

For a user-entered local clock time:

- normal local time converts deterministically to UTC;
- DST-gap invalid time is not silently shifted;
- DST-overlap ambiguous time resolves deterministically;
- resulting UTC occurrence is filtered against half-open planning windows.

A device time-zone change does not silently rewrite stored user schedule intent.

## SQLite persistence architecture

Structured records are stored in local SQLite.

Current schema version is documented in `DATABASE_SCHEMA.md` and source migration definitions.

Persistence includes:

- ordered/versioned migrations;
- schema version tracking;
- indexes/relationships/cascades;
- transactional migration DDL + version writes;
- transactional repository operations where consistency requires it;
- WAL mode;
- busy timeout;
- parameterized operations;
- snapshot/integrity tests.

CareNest does not claim whole-database encryption.

## SQLite WAL/snapshot architecture

SQLite result-producing PRAGMAs are treated as query/scalar result operations rather than non-query commands.

Snapshot/backup preparation coordinates WAL state and validates copied database integrity/content.

## SQLite dependency/provider architecture

CareNest retains the established `sqlite-net-pcl` application API while central package pinning selects maintained SQLite native/provider leaves.

Current verified intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- selected providers `2.1.12`;
- central transitive pinning enabled;
- no former exact `GHSA-2m69-gcr7-jv3q` audit suppression.

`SqliteDependencySecurityContractTests` protects this source policy.

Package security and packaged existing-data compatibility are separate release properties.

## Encrypted document architecture

Imported sensitive document bytes are stored separately from structured metadata using authenticated encryption.

High-level flow:

```text
User-selected file
  -> DocumentService
  -> EncryptedDocumentStore
  -> authenticated encrypted local payload
  -> SQLite metadata
  -> document master key in platform secure storage
```

New streams use chunked AEAD framing v2. Legacy v1 read support remains for compatibility.

Missing/corrupt key plus existing ciphertext fails closed; read/export does not silently create a replacement key.

Explicit decrypted export creates a copy outside the encrypted vault boundary.

## Document import compensation

Encrypted payload and SQLite metadata are separate surfaces.

Import uses compensating rollback around payload creation, metadata persistence and audit persistence. Cleanup attempts can deliberately ignore caller cancellation when cancelling cleanup would knowingly strand newly created artifacts.

This is compensation, not a cross-filesystem/SQLite global transaction.

## Backup architecture

Manual backups package the local state into a password-protected versioned container.

Security/compatibility properties include:

- PBKDF2-HMAC-SHA256 password derivation;
- AES-256-GCM authenticated encryption;
- chunked framing v2 for new writes;
- legacy v1 read compatibility;
- strict decrypted archive topology validation;
- snapshot/integrity behavior;
- protected inclusion of document recovery key material;
- wrong-password/tamper/truncation/trailing-data rejection;
- rollback/key restoration on failed restore.

See `BACKUP_AND_RESTORE.md`.

## App-lock architecture

Optional app lock uses:

- numeric PIN policy;
- random salt;
- PBKDF2-HMAC-SHA256 verifier;
- fixed-time comparison;
- platform secure storage;
- exact verifier/salt shape validation;
- buffer clearing where practical;
- rollback around multi-key transitions;
- fail-closed corrupt/missing material.

It is a local privacy barrier, not whole-database/device encryption.

## Reports/export architecture

Portable output is explicit user-controlled data movement.

Current controls include:

- staged partial files + atomic final move for report writers;
- CSV formula-like user text neutralization;
- failure/cancellation staging cleanup best effort;
- managed decrypted document/report cache ownership;
- report-cache cleanup after external share handoff where CareNest still owns the temporary copy.

CareNest cannot delete external copies after handoff.

## Privacy architecture

Current v1 privacy properties:

- no required account/server;
- no automatic CareNest cloud upload;
- no hidden runtime telemetry client;
- local SQLite records;
- encrypted imported documents;
- encrypted manual backups;
- explicit external actions;
- generic notifications by default;
- privacy-minimized logs.

## Logging/error architecture

Runtime diagnostic paths avoid normal logging of health content, raw document/backup content, secrets, PINs/passwords/keys, and sensitive full exception messages/stack traces.

Global exception observation is privacy-aware and does not introduce remote telemetry.

See `docs/security/LOGGING_PRIVACY.md`.

## Navigation architecture

ViewModels use app-navigation abstractions/routes rather than directly coupling domain/application logic to Shell/platform navigation internals.

## Theme/accessibility architecture

CareNest supports system/light/dark presentation and accessibility-oriented semantics/preferences.

Source/XAML contracts cannot certify real assistive-technology behavior. Manual screen-reader, text-scaling, keyboard/focus, contrast and reduced-motion tests remain release gates.

## Platform architecture

### Android

Android notification scheduling/recovery must account for permission, alarm capability, battery restrictions, reboot/time/time-zone changes, force-stop/vendor behavior and async receiver lifetime.

### Windows

Current notification fallback includes in-process limitations and timer ownership/race protections. Closed-app delivery is not guaranteed.

### iOS/Mac Catalyst

Apple notification permission/OS behavior and production signing/provisioning are platform-controlled. Real-device/manual release checks remain required.

## Build architecture

`CareNest.App` is multi-targeted.

`CareNestTargetFramework` narrows the active app target before restore/build so platform-specific runners do not require unrelated workloads and app target values do not leak into platform-neutral referenced projects.

## CI/security/release architecture

Repository automation includes:

- formatting;
- 3 core test projects;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- CodeQL;
- unsuppressed Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Major verification-relevant source changes use marker-only exact-head PR verification. Marker files are closed without merge and do not enter production source.

## Exact production-tag architecture

Production tags matching `v*` run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Release Evidence records source/ref/run/attempt identity, tracked source manifests/checksums, TRX results, dependency inventories, workspace integrity and evidence checksums.

Available evidence is retained before aggregate failure evaluation.

## External project-support boundary

Canonical voluntary project-support URL:

`https://buymeacoffee.com/sanskarIN`

It is explicit user action and must not receive health-record identifiers through URL parameters.

Project support does not change health behavior, reminder priority, emergency support or access to local records.

## Failure strategy

Architecture principles:

- validate before destructive writes;
- preserve async/cancellation behavior;
- deliberately use non-cancelled compensation when cancellation would knowingly strand inconsistent state;
- avoid synchronous task blocking;
- keep reminder planner/rebuild deterministic/idempotent;
- reconcile persisted state and external OS requests explicitly;
- keep logs privacy-minimized;
- fix analyzer/audit/workflow failures rather than broadly suppressing them;
- preserve historical persistence/encryption compatibility;
- keep manual/store/signing/data-compatibility limitations explicit.

## Future architecture

The following are outside the current local-first v1 design unless a future architecture decision explicitly introduces them:

- CareNest cloud synchronization;
- required accounts;
- server-side health storage;
- remote caregiver collaboration;
- silent background sharing;
- analytics/telemetry;
- clinical interpretation/decision systems.

Any such feature requires new authentication, consent/revocation, deletion/export, key management, privacy/store disclosure, abuse/threat and security testing design.

## Related documents

- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`
- `docs/CODEBASE_REFERENCE.md`
- `docs/CONFIGURATION_REFERENCE.md`
- `docs/architecture/APPLICATION_FLOWS.md`
- `docs/architecture/SERVICE_BOUNDARIES.md`
- `docs/architecture/DATABASE_SCHEMA.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/architecture/DOCUMENT_VAULT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/testing/TESTING_GUIDE.md`
- `docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md`
- `DECISIONS.md`
