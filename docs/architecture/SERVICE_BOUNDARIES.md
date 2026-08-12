# CareNest Service and Infrastructure Boundaries

CareNest uses layered contracts so UI, application logic, persistence, cryptography, and platform integrations remain independently understandable and testable.

## Dependency direction

The intended direction is:

```text
CareNest.Shared
   ↑
CareNest.Domain
   ↑
CareNest.Application
   ↑
CareNest.Infrastructure
   ↑
CareNest.App (composition/UI/platform)
```

The MAUI application may reference platform-neutral layers. Platform-neutral projects must not depend on MAUI.

Automated architecture tests protect these boundaries.

## CareNest.Shared

Purpose:

- constants;
- small reusable guards/primitives;
- values that do not require MAUI or persistence.

Examples include project/support/contact constants and guard helpers.

Rules:

- no MAUI dependency;
- no SQLite dependency;
- no platform-specific notification/file APIs;
- no clinical decision logic.

## CareNest.Domain

Purpose:

- entities;
- enums;
- domain validation;
- schedule configuration semantics.

Examples:

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
- `AuditEntry`.

Domain rules validate user-entered configuration shape. They do not infer dosage or treatment.

## CareNest.Application

Purpose:

- use-case orchestration;
- service/repository contracts;
- deterministic reminder planning;
- reminder coordination;
- profile/medicine/appointment/document/report/backup-oriented application operations.

Application code should remain platform-neutral.

### Repository contract boundary

Application services depend on repository abstractions such as the CareNest repository contract rather than direct SQL.

Expected repository responsibilities include:

- load/save domain entities;
- relationship queries;
- reminder-occurrence reads/upserts;
- settings;
- audit entries;
- stock calculations/adjustments;
- migration/schema-related access through infrastructure.

ViewModels must not issue SQL directly.

### ReminderPlanner

`ReminderPlanner` is deterministic application logic.

Responsibilities:

- validate cross-entity ownership consistency;
- validate UTC planning-window contract;
- validate schedule configuration;
- honor profile/medicine/schedule states;
- honor user-entered date/time/time-zone rules;
- produce deterministic occurrence identities;
- handle DST gaps/overlaps deterministically;
- avoid automatic occurrences for as-needed schedules.

It does not schedule OS notifications itself.

### ReminderCoordinator

`ReminderCoordinator` orchestrates:

- loading eligible schedules/related records;
- planner execution;
- occurrence persistence;
- platform notification registration/cancellation through `INotificationService`;
- snooze state;
- medication-log generation;
- user-configured stock changes;
- overdue reconciliation.

It does not infer how often a medicine should be used.

### Notification contract

`INotificationService` abstracts platform notification capability.

Typical responsibilities:

- request/check permission;
- schedule a notification request;
- cancel an existing notification;
- expose diagnostics/capability information through applicable higher-level services.

Platform-specific implementations belong in `CareNest.App` platform composition.

### Navigation contract

ViewModels use navigation abstraction (`IAppNavigator`) rather than directly owning platform navigation mechanics.

Benefits:

- testable ViewModels;
- predictable route ownership;
- no direct platform-shell manipulation inside business logic.

### Error presentation boundary

UI-facing asynchronous operations use safe centralized error presentation/logging patterns.

Raw exception messages/stack traces are not surfaced to users or routine structured logs from health-data operations.

## CareNest.Infrastructure

Purpose:

- SQLite database implementation;
- schema migrations;
- repository implementation;
- encrypted document storage;
- backup/restore;
- report/export generation;
- filesystem/crypto implementation that remains independent of MAUI UI composition where possible.

### SQLite boundary

Infrastructure owns:

- connection configuration;
- WAL mode;
- busy timeout;
- migrations;
- relational cleanup/indexes;
- snapshot/checkpoint mechanics;
- query execution.

Application/UI code does not need to know SQL syntax.

### Encrypted document storage boundary

Infrastructure handles document encryption/decryption and protected storage using authenticated .NET cryptographic primitives.

The App layer supplies platform/user-selected file paths/streams as required by UI workflows.

### Backup boundary

Infrastructure handles:

- database snapshot creation;
- protected package format;
- password-based key derivation;
- authenticated encryption;
- restore validation/staging.

UI supplies explicit user intent/password/destination interaction.

### Report/export boundary

Infrastructure/application report services render output based on local records.

Reports must keep the non-clinical boundary and avoid introducing clinical scoring/inference.

## CareNest.App

Purpose:

- MAUI views/XAML;
- ViewModels;
- dependency injection/composition;
- navigation;
- platform secure storage adapter;
- media/file picker;
- share/calendar/browser integration;
- Android/iOS/Mac Catalyst/Windows platform notification behavior;
- platform permission/capability diagnostics;
- theme/accessibility presentation.

### ViewModel rules

Concrete ViewModels are contract-tested to avoid:

- direct SQLite access;
- direct network-client creation;
- synchronous task blocking;
- `async void` business operations outside event-adapter patterns;
- notification-permission request during onboarding;
- bypassing explicit as-needed/no-reminder behavior.

### Platform-specific source isolation

Platform source under `Platforms/*` is compiled only for its corresponding target framework.

The `CareNestTargetFramework` property is used by CI/developer commands to narrow the MAUI target on hosts that do not have every workload installed.

## TimeProvider boundary

Time-sensitive application logic uses `TimeProvider` where implemented so tests can reason about deterministic current UTC time rather than depending on wall-clock timing.

When a public method accepts a value named/defined as UTC, runtime guards should reject non-UTC `DateTime.Kind` rather than silently reinterpret local/unspecified values.

## Secure secret storage boundary

Platform secure storage is used for secrets such as app-lock verifier material and document-encryption key material.

CareNest does not treat secure storage as a substitute for:

- device security;
- OS trust;
- whole-database encryption;
- protection on a rooted/jailbroken/fully compromised device.

## File/share/browser boundary

File export, sharing, calendar export, and external funding links occur only after explicit user action.

Once content is handed to another app/service, that destination becomes a separate privacy/security boundary.

## No-network v1 boundary

Runtime policy tests protect the current local-first scope from accidental introduction of a network/telemetry client in runtime source.

A future network feature is not a normal refactor. It requires explicit architecture/privacy/security review and documentation updates.

## Logging boundary

Each layer must avoid leaking sensitive records into logs.

Allowed operational metadata should be deliberately limited.

See `docs/security/LOGGING_PRIVACY.md`.

## Cancellation and async boundary

Long-running I/O/application operations should be asynchronous and accept/propagate `CancellationToken` where applicable.

Runtime source policy rejects common synchronous task-blocking patterns such as `.Wait()`, `.Result`, `GetAwaiter().GetResult()`, `Thread.Sleep`, `Task.WaitAll`, and `Task.WaitAny` patterns.

## Adding a new service

Before adding a new service, determine:

1. Is the behavior domain validation, application orchestration, infrastructure I/O, or UI/platform work?
2. What data crosses the boundary?
3. Does the service need cancellation?
4. Does it introduce a new permission?
5. Does it create logs/notifications/exports?
6. Does it affect backup/restore/schema compatibility?
7. Does it introduce networking/telemetry?
8. Does it change the medical-safety boundary?
9. Which unit/integration/UI-contract tests should enforce it?
10. Which documentation and release checklists must change?

Do not place convenience code in a higher layer if doing so breaks architecture direction or makes health-data behavior harder to test/review.