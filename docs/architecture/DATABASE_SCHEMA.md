# CareNest Database Schema

CareNest v1 stores structured application records in local SQLite. Imported document payloads are encrypted separately and referenced by metadata records.

Current release-candidate schema version: **5**.

> CareNest does not claim transparent whole-database encryption. SQLite is protected primarily by the application sandbox/device security; imported document payloads and manual backups have separate encryption protections.

## Schema management

`SchemaInfo` records the applied schema version.

Migrations:

- run in ascending order;
- preserve deterministic upgrade behavior;
- are covered by integration tests;
- must not be silently skipped/reinterpreted;
- must be reviewed for backup/restore compatibility.

## Identifier/time conventions

Primary domain entities use GUID-string identifiers.

Records that track lifecycle/audit timing use UTC timestamps where appropriate.

Status/lifecycle fields are explicit when business behavior depends on state.

## Core relationships

```text
PersonProfile
  ├─< EmergencyContact
  ├─< Medicine
  │    ├─< MedicineSchedule
  │    │    ├─< ScheduleTime
  │    │    └─< ReminderOccurrence
  │    ├─< MedicationLogEntry
  │    └─< StockAdjustment
  ├─< Appointment
  └─< CareDocument >─< DocumentTag >─ Tag

AuditEntry -> entity type/id metadata
Settings / SchemaInfo / backup metadata -> installation-level records
```

## `PersonProfile`

Purpose:

- local organizational container for one person/family member.

Associated data can include:

- medicines;
- appointments;
- documents;
- emergency contacts;
- medication-log records through related medicines;
- stock adjustments through related medicines.

Lifecycle considerations:

- archived profiles suppress automatic reminder materialization;
- destructive deletion must clean expected relationships/files.

## `EmergencyContact`

Purpose:

- local contact details associated with a profile.

CareNest stores organizational contact information only and does not become an emergency-dispatch service.

## `Medicine`

Purpose:

- user-entered medicine organizational record.

Important semantic fields include:

- owning `ProfileId`;
- name;
- strength text;
- instruction text;
- start/end dates;
- lifecycle state;
- optional stock/refill values;
- optional user-configured stock change per Taken event.

Safety rule:

`Strength` and `Instructions` are opaque strings. Persistence/domain code does not parse them to calculate dosage or treatment.

Medicine states include active/paused/completed/archived behavior; non-active states suppress applicable automatic reminders.

## `MedicineSchedule`

Purpose:

- explicit user-entered recurrence configuration for one medicine.

Key fields include:

- `MedicineId`;
- `Kind`;
- `StartDate`;
- optional `EndDate`;
- optional interval hours;
- optional cycle on/off days;
- weekday bit mask;
- explicit `TimeZoneId`;
- optional follow-up minutes;
- enabled flag.

Validation rules are documented in `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`.

## `ScheduleTime`

Purpose:

- explicit local clock time owned by one medicine schedule.

Key fields:

- `MedicineScheduleId`;
- `Hour` (0–23);
- `Minute` (0–59).

Planner ownership validation rejects schedule-time rows belonging to a different schedule.

## `ReminderOccurrence`

Purpose:

- materialized deterministic instance of a future/past reminder event.

Conceptual data includes:

- profile/medicine/schedule identity;
- scheduled UTC time;
- local scheduled representation/time-zone identity;
- reminder state;
- stable occurrence key/identity;
- optional platform notification ID;
- optional snooze UTC value;
- follow-up marker;
- state-change timing.

The planner uses deterministic keys so repeated rebuilds can upsert the same occurrence rather than generate duplicate identities.

## `MedicationLogEntry`

Purpose:

- user-recorded outcome/history associated with a medicine/reminder.

Typical states include user-recorded Taken, Skipped, Delayed, or Missed outcomes.

A log entry is organizational history and does not prove adherence.

## `StockAdjustment`

Purpose:

- quantity delta used to calculate a local stock estimate.

Key principles:

- quantities come from explicit user values/configuration;
- CareNest does not infer quantity from strength/instructions;
- Taken-event automatic adjustment can reference the medication-log entry;
- safeguards prevent an automatic configured change from driving the estimate below zero.

## `Appointment`

Purpose:

- local appointment organization/history.

Can be associated with a profile and include user-entered appointment details/notes plus supported reminder/export metadata.

Calendar export occurs outside SQLite through explicit user action.

## `CareDocument`

Purpose:

- metadata for an imported encrypted document.

The original document bytes are not simply stored unencrypted in the metadata row.

Metadata can include:

- owning profile;
- display/name/category metadata;
- encryption/storage metadata;
- optional folder metadata (schema v5);
- timestamps/lifecycle information.

Actual encrypted payload storage is handled by the document-vault infrastructure.

## `Tag`

Purpose:

- reusable local document-organization label.

## `DocumentTag`

Purpose:

- join table implementing the many-to-many relationship between `CareDocument` and `Tag`.

Relationship cleanup must avoid orphaned join records.

## `AuditEntry`

Purpose:

- safe/high-level record of selected changes/events.

Audit data references entity type/id and change metadata without copying encrypted document bytes.

Safe summaries must not become a backdoor for logging sensitive free-text content.

## Settings

Installation-level settings support behavior such as:

- onboarding state;
- notification preferences;
- quiet hours;
- generic notification labels;
- sound/vibration/persistence preferences;
- theme/accessibility preferences;
- backup reminder/settings where applicable;
- developer diagnostics values.

Secrets such as app-lock verifier material/document encryption keys belong in platform secure secret storage, not normal plaintext settings rows.

## Schema versions

### Version 1 — core model

Introduced core:

- profiles;
- medicines;
- schedules;
- occurrences;
- medication log;
- appointments;
- documents;
- tags;
- stock;
- emergency contacts;
- settings;
- backup metadata;
- audit;
- schema metadata.

### Version 2 — time-zone/recovery/document encryption metadata

Added schedule time-zone/recovery fields and document-encryption metadata needed for reliable local encrypted document handling and reminder recovery.

### Version 3 — follow-up/quiet-hours/index hardening

Added reminder follow-up, quiet-hours settings, and migration indexes.

### Version 4 — explicit stock/refill configuration

Added user-entered stock-change-per-Taken-event configuration and optional refill date fields to medicine records.

The quantity value is explicitly user configured and never inferred from medicine text.

### Version 5 — document folder metadata

Added optional local folder metadata for organized documents.

## Relationship integrity

Important ownership chain:

```text
PersonProfile.Id == Medicine.ProfileId
Medicine.Id == MedicineSchedule.MedicineId
MedicineSchedule.Id == ScheduleTime.MedicineScheduleId
```

Reminder planner now validates this chain before materializing occurrences so incorrect caller-loaded records cannot silently cross profiles/medicines/schedules.

## Deletion/cascade expectations

Repository/migration tests protect relationship cleanup.

Manual release testing must also confirm filesystem cleanup for encrypted document payloads because database cascade rules alone cannot delete external encrypted files unless the application workflow does so.

Previously exported files/backups outside CareNest are not removed by deleting local rows.

## WAL mode and busy timeout

CareNest configures SQLite WAL mode and a busy timeout.

Result-producing PRAGMA operations are read/consumed as results rather than sent through a non-query execution path.

Regression coverage verifies configured behavior.

## Backup snapshot interaction

Because WAL can contain committed data not yet folded into the main database file, backup snapshot creation performs/consumes a full WAL checkpoint before copying the database.

Integration tests verify:

- copied snapshot exists;
- committed profile record is present;
- copied database opens read-only;
- `PRAGMA integrity_check` returns `ok`;
- pre-cancelled snapshot leaves no output file.

See `docs/architecture/BACKUP_AND_RESTORE.md`.

## Query/security principles

Repository/infrastructure owns SQLite access.

UI/ViewModels do not construct SQL directly.

The architecture does not expose an arbitrary user SQL execution interface.

## Migration requirements for contributors

When adding a persisted field/entity:

1. add a new migration/schema version rather than rewriting deployed history;
2. define default/backfill behavior;
3. update domain/entity model;
4. update repository queries/saves;
5. add migration/integration tests;
6. verify relationship deletion behavior;
7. verify backup/restore compatibility;
8. update exports/reports if needed;
9. update privacy/data lifecycle/store disclosures;
10. update this file and `what_changed.md` when requested.

## Future schema compatibility

Future releases should add backup/schema compatibility fixtures representing historical supported versions.

Unknown future schema/backup versions should not be silently accepted/reinterpreted.

## Related documentation

- `docs/architecture/ARCHITECTURE.md`
- `docs/architecture/APPLICATION_FLOWS.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/testing/TESTING_GUIDE.md`