# CareNest Architecture

CareNest `1.0.0-rc.1` is a local-first .NET MAUI application for organizing family profiles, user-entered medicine reminders, appointments, health documents, stock/refill notes, reports, and encrypted manual backups.

> **Safety boundary:** CareNest is organizational software. It does not diagnose, calculate/infer dosage, recommend treatment, check medication interactions, create clinical risk scores, replace qualified professionals, or provide emergency services.

## Architectural goals

The v1 architecture is designed to provide:

- no required CareNest account or backend;
- local structured storage with explicit schema migrations;
- encrypted imported document storage;
- password-encrypted manual backup/restore;
- deterministic reminder materialization from explicit user-entered schedules;
- explicit reconciliation between persisted reminder state and platform scheduled requests;
- platform notification abstractions with limitation reporting;
- MVVM-style UI separation;
- testable platform-neutral domain/application/infrastructure layers;
- privacy-minimized diagnostic logging;
- explicit user-controlled export/share boundaries;
- clear room for future versions without silently introducing cloud sync/account behavior into v1.

## System context

```text
+-------------------------- User Device ---------------------------+
|                                                                  |
|  CareNest MAUI UI / ViewModels                                  |
|             |                                                    |
|             v                                                    |
|  Application services / coordinators / planner                  |
|             |                         |                          |
|             v                         v                          |
|  Repository + Infrastructure       Platform abstractions        |
|       |          |          |       notifications/share/files    |
|       v          v          v                                    |
|    SQLite   encrypted     reports                                |
|             documents     backups                                |
|                                                                  |
|  Platform secure secret storage: app-lock/document key material |
+------------------------------------------------------------------+
                   |
                   | explicit user action only
                   v
        External file/calendar/browser/share destination
```

No normal CareNest v1 flow requires a CareNest-owned server.

## Solution projects

### `CareNest.Shared`

Small cross-layer primitives/constants/guards with no MAUI or persistence dependency.

Responsibilities include:

- shared product/contact/support constants;
- simple guard helpers;
- values safe for platform-neutral reuse.

### `CareNest.Domain`

Framework-independent entities, enums, and validation rules.

Responsibilities include:

- person/profile entities;
- medicine entities/lifecycle;
- schedule configuration;
- reminder/log entities;
- appointment/document/tag/stock/contact/audit entities;
- validation of user-entered schedule/data shape.

Domain validation never turns medicine text into dosage/treatment instructions.

### `CareNest.Application`

Platform-neutral application orchestration and contracts.

Responsibilities include:

- repository/service contracts;
- reminder planning;
- reminder coordination;
- reminder platform-state reconciliation/compensation through abstractions;
- use-case orchestration;
- notification abstraction;
- profile/medicine/appointment/document/report/backup-facing application operations;
- time-provider-aware workflows where applicable.

`ReminderPlanner` belongs here because it is deterministic business/application logic, not platform notification code.

### `CareNest.Infrastructure`

Local persistence, cryptography, and export implementations.

Responsibilities include:

- SQLite connection/configuration;
- schema migrations;
- repositories;
- WAL checkpoint/snapshot logic;
- encrypted document storage;
- backup/restore format/encryption;
- CSV/PDF/structured export implementation;
- filesystem/crypto operations that can remain UI-independent.

### `CareNest.App`

.NET MAUI UI/composition/platform layer.

Responsibilities include:

- XAML views;
- ViewModels;
- dependency injection;
- navigation;
- theme/accessibility presentation;
- platform secure-storage adapter;
- file/media picker;
- share/browser/calendar integration;
- Android/iOS/Mac Catalyst/Windows notification implementations;
- platform permission/capability diagnostics;
- startup composition/recovery.

### Tests

- `CareNest.UnitTests`: domain/application/reminder deterministic/direct service tests.
- `CareNest.IntegrationTests`: SQLite/crypto/backup/document/report/reminder behavior integration.
- `CareNest.UiTests`: source/XAML/repository/architecture/ViewModel/security/release-policy contracts.

See `docs/testing/TESTING_GUIDE.md`.

## Dependency direction

Intended project direction:

```text
Shared <- Domain <- Application <- Infrastructure <- App
```

More precisely, lower layers must not depend upward on MAUI/platform composition. Architecture contract tests enforce this direction and verify that platform-neutral projects do not reference MAUI.

## MVVM/UI boundary

Views bind to ViewModels.

ViewModels:

- do not issue SQL directly;
- do not construct network clients for the local-first v1 scope;
- do not hide blocking I/O inside `Task.Run` as a default pattern;
- do not directly own platform-specific SQLite/notification internals;
- use application contracts/navigation abstractions;
- route unexpected errors through privacy-aware safe UI handling.

## Core data flow

```text
View
  -> ViewModel
    -> Application service/coordinator
      -> repository/service abstraction
        -> infrastructure/platform implementation
          -> SQLite/files/secure storage/OS service
```

A feature should live at the lowest layer that can own it without breaking dependency direction.

## Reminder architecture

### Schedule intent

`MedicineSchedule` + `ScheduleTime` describe explicit user intent.

CareNest never parses `Medicine.Strength` or `Medicine.Instructions` to choose a reminder frequency.

### Planner

`ReminderPlanner` materializes deterministic future `ReminderOccurrence` values.

Planner protections include:

- profile → medicine ownership consistency;
- medicine → schedule ownership consistency;
- schedule → schedule-time ownership consistency;
- valid explicit time zone;
- UTC planning bounds;
- half-open planning window `[fromUtc, toUtc)`;
- state/date/schedule-kind validation;
- stable occurrence key;
- duplicate explicit-time deduplication;
- chronological result ordering;
- DST gap/overlap deterministic handling;
- no automatic as-needed occurrences;
- suppression for disabled schedules/archived profiles/paused-completed-archived medicines.

### Coordinator

`ReminderCoordinator` coordinates two distinct state surfaces:

1. persisted CareNest reminder occurrences/log state;
2. operating-system scheduled notification/alarm requests.

Core responsibilities include:

1. load eligible schedules/medicine/profile/times;
2. invoke `ReminderPlanner`;
3. upsert materialized occurrences;
4. use `ScheduledUtc` or explicit `SnoozedUntilUtc` as the effective due time;
5. reconcile existing platform requests before replacement, quiet-hour suppression, or invalidation;
6. schedule eligible platform notifications through `INotificationService`;
7. process reminder actions with cancellation-first ordering;
8. create medication-log entries where applicable;
9. apply only explicit user-configured stock changes after Taken events;
10. reconcile overdue reminders;
11. keep cancellation failures retryable rather than falsely declaring synchronization complete.

### Persisted state ↔ OS request reconciliation

The SQLite row and the platform request are not one transaction.

CareNest therefore applies explicit ordering/compensation:

- cancel an old platform request before replacement/suppression/invalidation;
- retain enough stale occurrence identity after schedule edits to cancel obsolete OS requests;
- cancel future requests before medicine/profile cascade deletion;
- if persistence fails after platform cancellation, attempt non-cancelled rebuild compensation for still-existing records;
- reconcile medicine/profile saves before later non-critical audit bookkeeping;
- reconcile appointment persistence with its platform reminder request;
- leave cancellation failure retryable.

### Handled reminder actions

Taken, Skipped, Delayed, Missed, Snoozed, and Cancelled use cancellation-first ordering:

1. cancel the old platform request;
2. persist the requested handled state only after cancellation succeeds;
3. for Snoozed, schedule a replacement only after state persistence;
4. if later persistence/scheduling fails, attempt non-cancelled restoration of previous occurrence state plus reminder rebuild;
5. surface aggregate recovery failure rather than claiming contradictory state is consistent.

Post-success audit/stock bookkeeping is not allowed to falsely undo an already completed user action.

### Snooze

Snooze requires a future UTC value. Local or unspecified `DateTime.Kind` is rejected rather than silently reinterpreted.

For a valid snooze, `SnoozedUntilUtc` is the effective due time for upcoming/overdue behavior. The original scheduled time remains historical schedule identity.

### Platform delivery

CareNest distinguishes deterministic planner/persisted state from OS notification delivery.

OS permission/battery/background/force-stop/shutdown/policy can affect delivery. The app reports limitations instead of guaranteeing reminders.

See:

- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`;
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`;
- `docs/architecture/APPLICATION_FLOWS.md`.

## Appointment reminder architecture

Appointments store an explicit UTC instant. `Appointment.StartsUtc` must have `DateTimeKind.Utc`; local/unspecified values are rejected rather than relabeled.

Appointment save/rebuild respects notification permission and does not repeatedly prompt from background recovery.

Database appointment persistence and the platform reminder request remain separate surfaces. Appointment services use compensation/reconciliation when one succeeds and a later step fails.

## Time-zone architecture

Schedules store explicit time-zone identity.

For a user-entered local reminder time:

- normal local time converts to UTC;
- DST-invalid gap time is not replaced with a guessed alternative;
- DST-ambiguous overlap chooses a deterministic offset;
- resulting UTC value is filtered by the half-open planning window.

Stored schedule intent is not silently rewritten simply because the device time zone changes.

## Persistence architecture

Structured records are stored in SQLite.

Current schema version: **5**.

Persistence design includes:

- ordered migrations;
- `SchemaInfo` version tracking;
- relationships/cascades/indexes;
- transactional migration DDL + schema-version recording;
- transaction boundaries for multi-step repository changes where consistency matters;
- WAL mode;
- busy timeout;
- parameterized repository operations;
- snapshot/integrity regression tests.

CareNest does not claim whole-database encryption.

See `docs/architecture/DATABASE_SCHEMA.md`.

## SQLite WAL/snapshot architecture

Result-producing SQLite PRAGMAs are consumed correctly as query/scalar results.

Important operations include:

- read/validate WAL journal mode;
- read/validate busy timeout;
- consume full WAL checkpoint result before backup snapshot.

Snapshot integration tests verify committed content and `PRAGMA integrity_check`, not only output file existence.

## SQLite dependency/provider architecture

The application retains the established `sqlite-net-pcl` API path while central transitive package pinning selects maintained native/provider leaves.

Current RC1 package intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- selected SQLitePCLRaw providers `2.1.12`;
- no former `GHSA-2m69-gcr7-jv3q` audit suppression.

`SqliteDependencySecurityContractTests` protects the package floor/suppression absence.

A security-clean dependency graph does not by itself prove packaged existing-database/encrypted-document/backup compatibility. Those remain manual release gates after native/provider changes.

See `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

## Encrypted document architecture

Imported sensitive document bytes are stored separately using authenticated encryption.

High-level model:

```text
User-selected file
  -> encrypted document service
  -> AES-GCM protected local payload
  -> metadata row in SQLite
  -> key material kept via platform secure storage
```

New encrypted payloads use chunked authenticated framing v2; legacy v1 remains readable for compatibility.

Read/export paths do not silently create a replacement key when existing encrypted payloads depend on a missing/corrupt key.

Explicit export/decryption creates a copy outside the CareNest vault boundary. Temporary application-owned plaintext is managed/cleaned according to export/share lifecycle rules.

## Backup architecture

Manual backups combine required portable local state into a versioned password-protected package.

Security properties include:

- PBKDF2-HMAC-SHA256 password derivation;
- AES-GCM authenticated encryption;
- chunked authenticated framing v2 for new writes;
- legacy v1 read compatibility;
- strict decrypted archive topology validation;
- format/version validation;
- wrong-password/tamper/truncation/trailing-data rejection;
- protected inclusion of encrypted-document recovery key material;
- database snapshot path compatible with WAL.

See `docs/architecture/BACKUP_AND_RESTORE.md`.

## App-lock architecture

App lock is optional.

Protection model:

- numeric PIN policy;
- random salt;
- PBKDF2-HMAC-SHA256 verifier;
- secure platform secret-store persistence;
- fixed-time comparison;
- exact salt/verifier shape validation;
- derived/retrieved verifier-buffer clearing where managed memory control permits;
- rollback around multi-key update/disable transitions;
- fail-closed missing/corrupt material;
- delete stored lock material when disabled.

It is not whole-database/device encryption.

## Privacy architecture

Primary v1 privacy properties:

- no required account/server;
- no automatic CareNest cloud upload;
- no hidden analytics/telemetry client;
- local structured storage;
- encrypted imported documents;
- manual encrypted backups;
- explicit export/share/calendar/browser boundaries;
- generic notification labels by default;
- privacy-redacted logging.

See `docs/privacy/PRIVACY_MODEL.md` and `docs/architecture/DATA_STORAGE_AND_EXPORT.md`.

## Logging/error architecture

CareNest centralizes safe UI/global/startup/reminder error handling.

Routine logs use safe operational metadata rather than raw health information or full exception details.

Global exception observation attaches once and observes supported unhandled/unobserved exceptions without introducing remote telemetry.

See `docs/security/LOGGING_PRIVACY.md`.

## Navigation architecture

ViewModels use an app-navigation abstraction rather than directly coupling application behavior to platform shell operations.

Benefits:

- testability;
- route consistency;
- no direct platform navigation dependency in application/domain layers.

## Theme/accessibility architecture

The app supports system/light/dark presentation plus large-interface/reduced-motion preferences.

Accessibility semantics are partly contract-tested in source/XAML, but real assistive-technology verification remains a manual release gate.

See `docs/design/ACCESSIBILITY.md`.

## Build architecture

The MAUI project targets Android, iOS, Mac Catalyst, and Windows.

`CareNestTargetFramework` narrows the app target before restore/build so target-specific runners do not need unrelated workloads and target framework values do not leak into platform-neutral referenced projects.

CI uses separate target jobs for platform builds.

## CI/security/release architecture

Repository automation includes:

- platform-neutral formatting;
- unit/integration/UI-policy tests;
- Android/Windows/iOS/Mac Catalyst Release builds;
- CodeQL;
- unsuppressed Dependency Audit;
- Release Gate;
- Release Evidence.

Major verification-relevant source hardening uses marker-only exact-head PR verification. Verification marker files are never merged into `main`.

Production tags matching `v*` run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Release Evidence records source/ref/run identity, tracked-file manifests/checksums, all three TRX suites, dependency inventories, workspace integrity, and evidence checksums. It uploads available evidence even when an evidence component fails, then applies an aggregate success/failure gate.

Release workflow/test/build-script changes are verification-relevant source even when application runtime behavior is unchanged.

## External support boundary

Canonical support destination:

`https://buymeacoffee.com/sanskarIN`

It is opened only after explicit user action and is not supplied with health-record query payloads.

Funding is not an entitlement system and does not change health behavior, reminder priority, emergency assistance, or access to local records.

## Failure strategy

Architecture principles:

- validate before destructive writes;
- preserve async/cancellation behavior;
- use non-cancelled compensation deliberately when cancellation should not strand inconsistent cross-surface state;
- avoid synchronous task blocking;
- keep reminder planning/rebuild idempotent;
- reconcile persisted state and external OS requests explicitly;
- keep diagnostics privacy-minimized;
- treat tool/analyzer/workflow failures as defects to understand rather than broadly suppress;
- keep manual/store/signing/data-compatibility limitations explicit;
- do not claim a release gate complete unless it actually ran.

## Current verification lineage

Authoritative completed bug-audit baseline PR #54 verified 261 core tests, all four platform Release builds, CodeQL, and unsuppressed Dependency Audit for the runtime/test/dependency graph.

Later release-engineering source changes added tag triggers, failure-preserving Release Evidence, blocking local dependency audit, hardened Git setup and Release Gate checks, and executable policy tests. Superseded PR #55 verified 277 core tests plus Android/Windows/CodeQL/Dependency Audit before the complete-file audit identified further release-tooling/documentation corrections.

The current final `main` head requires a fresh complete exact-source verification before becoming the new release-engineering baseline.

## Future architecture

The following are deliberately outside the current v1 design:

- cloud synchronization;
- required accounts;
- server-side storage;
- remote caregiver collaboration;
- silent background sharing;
- remote mobile-number authentication;
- hidden analytics/telemetry;
- clinical interpretation/decision systems.

Any networked/collaboration feature requires new authentication, consent/revocation, key management, deletion/export, conflict handling, privacy/store disclosures, abuse/threat analysis, and security testing.

## Related documents

- `docs/README.md`
- `docs/architecture/APPLICATION_FLOWS.md`
- `docs/architecture/SERVICE_BOUNDARIES.md`
- `docs/architecture/DATABASE_SCHEMA.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/testing/TESTING_GUIDE.md`
- `docs/releases/RELEASE_PROCESS.md`
- `DECISIONS.md`
