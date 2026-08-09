# CareNest Architecture

## Context

CareNest v1 is a local-first .NET MAUI application. The trusted computing boundary is the user's device and operating-system application sandbox. No backend is required.

## Projects

- `CareNest.Domain`: entities, enums, schedule semantics and domain validation.
- `CareNest.Application`: DTOs, service contracts, use cases and reminder planning.
- `CareNest.Infrastructure`: SQLite persistence, migrations, encrypted document storage, backup/restore, CSV/PDF export and privacy-aware logging helpers.
- `CareNest.Shared`: small cross-layer primitives with no MAUI dependency.
- `CareNest.App`: MAUI UI, dependency injection, navigation, SecureStorage, file/media picker, sharing and platform notification implementations.
- Test projects: domain/application unit tests, SQLite/crypto integration tests, XAML/navigation contract tests.

## Data flow

```text
View -> ViewModel -> Application service -> Repository/Infrastructure -> SQLite/files
                       |
                       +-> Reminder coordinator -> Platform notification service
```

ViewModels do not issue SQL or access platform APIs directly.

## Reminder model

Schedules describe user intent. `ReminderPlanner` materializes future `ReminderOccurrence` rows over a rolling horizon. A stable occurrence key derived from schedule/time prevents duplicates. The coordinator compares persisted future occurrences with platform registrations and can rebuild them after app startup, upgrade, time-zone change, or explicit diagnostics.

No dosage is computed or inferred.

## Privacy boundaries

- Database: sandbox-protected SQLite; no whole-database encryption claim.
- Documents: AES-GCM encrypted individually using a random per-install installation key in platform secure storage.
- Backups: PBKDF2-HMAC-SHA256 + AES-GCM with a user password.
- Notifications: generic by default; no health-document contents.
- Logs: redacted structured events.

## Failure strategy

Application services return actionable failures; UI catches unexpected exceptions through a centralized safe error presenter. Migrations are transactional where supported. Backup restore validates magic/version/authentication before overwrite. Reminder scheduling is idempotent.

## Later versions

Cloud sync, remote caregiver access, identity, consent receipts and conflict resolution require a new threat model and are intentionally excluded from v1.
