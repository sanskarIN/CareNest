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

- `CareNest.UnitTests`: domain/application/reminder deterministic tests.
- `CareNest.IntegrationTests`: SQLite/crypto/backup/document/report integration.
- `CareNest.UiTests`: source/XAML/repository/architecture/ViewModel/security policy contracts.

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

`ReminderCoordinator`:

1. loads enabled schedules;
2. loads related medicine/profile/times;
3. invokes planner;
4. upserts materialized occurrences;
5. reads future eligible occurrences;
6. applies notification policy/quiet hours;
7. calls platform `INotificationService`;
8. processes reminder state changes;
9. creates medication-log entries;
10. applies explicit user-configured stock changes after Taken events.

### Snooze

Snooze requires a future UTC value. Local or unspecified `DateTime.Kind` is rejected rather than silently reinterpreted.

### Platform delivery

CareNest distinguishes deterministic planner state from OS notification delivery.

OS permission/battery/background/force-stop/shutdown/policy can affect delivery. The app reports limitations instead of guaranteeing reminders.

See `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` and `docs/architecture/APPLICATION_FLOWS.md`.

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

Explicit export/decryption creates a copy outside the CareNest vault boundary.

## Backup architecture

Manual backups combine required portable local state into a versioned password-protected package.

Security properties include:

- PBKDF2-HMAC-SHA256 password derivation;
- AES-GCM authenticated encryption;
- format/version validation;
- wrong-password/tamper rejection;
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
- derived/retrieved verifier-buffer clearing where managed memory control permits;
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

## CI/security architecture

Repository automation includes:

- platform-neutral formatting;
- unit/integration/UI-policy tests;
- Android/Windows/iOS/Mac Catalyst Release builds;
- CodeQL;
- Dependency Audit;
- Release Gate;
- Release Evidence.

Major source hardening uses marker-only exact-head PR verification. Verification marker files are never merged into `main`.

## Dependency risk boundary

The current sqlite-net dependency chain resolves SQLitePCLRaw native `2.1.11`, tracked under `GHSA-2m69-gcr7-jv3q`.

The narrow exact audit suppression is not remediation. The risk register remains authoritative until a compatible patched/replacement path is actually validated or release is explicitly blocked/decided.

## External support boundary

Canonical support destination:

`https://buymeacoffee.com/sanskarIN`

It is opened only after explicit user action and is not supplied with health-record query payloads.

Funding is not an entitlement system and does not change health behavior, reminder priority, emergency assistance, or access to local records.

## Failure strategy

Architecture principles:

- validate before destructive writes;
- preserve async/cancellation behavior;
- avoid synchronous task blocking;
- keep reminder rebuild idempotent;
- keep diagnostics privacy-minimized;
- treat tool/analyzer failures as defects to understand rather than broadly suppress;
- keep manual/store/signing limitations explicit;
- do not claim a release gate complete unless it actually ran.

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
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/testing/TESTING_GUIDE.md`
- `docs/releases/RELEASE_PROCESS.md`
- `DECISIONS.md`