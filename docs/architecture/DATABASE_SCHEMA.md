# Database Schema

SQLite tables mirror the domain entities plus `SchemaInfo`.

Core relationships:

- `PersonProfile` 1 → many `Medicine`
- `PersonProfile` 1 → many `Appointment`
- `PersonProfile` 1 → many `CareDocument`
- `Medicine` 1 → many `MedicineSchedule`
- `MedicineSchedule` 1 → many `ScheduleTime`
- `MedicineSchedule` 1 → many `ReminderOccurrence`
- `Medicine` 1 → many `MedicationLogEntry`
- `Medicine` 1 → many `StockAdjustment`
- `CareDocument` many ↔ many `Tag` through `DocumentTag`
- `PersonProfile` 1 → many `EmergencyContact`
- `AuditEntry` references entity type/id without storing document bytes.

Every primary entity uses a GUID string identifier, UTC creation/update timestamps, and explicit status fields where lifecycle matters.

## Schema versions

1. Core profiles, medicines, schedules, occurrences, logs, appointments, documents, tags, stock, contacts, settings, backup metadata and audit.
2. Adds schedule time-zone/recovery fields and document encryption metadata.
3. Adds reminder follow-up, quiet-hours settings and migration indexes.
4. Adds user-entered stock change-per-taken-event and optional refill date fields to medicine records.
5. Adds optional local folder metadata to organized documents.

Migrations are applied in ascending order and recorded in `SchemaInfo`. The current release-candidate schema version is **5**.
