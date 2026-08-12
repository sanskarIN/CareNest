# CareNest Application Flows

This document describes the major runtime flows in CareNest `1.0.0-rc.1` and the architectural boundaries each flow must preserve.

## Layering model

```text
MAUI View
  -> ViewModel
    -> Application service / coordinator
      -> Repository / infrastructure contract
        -> SQLite / encrypted files / platform service
```

Platform-specific services are composed in `CareNest.App`. Platform-neutral projects do not depend on MAUI.

## Startup flow

Typical startup responsibilities:

1. initialize dependency injection and application services;
2. initialize/migrate local SQLite storage;
3. attach privacy-aware global exception observation;
4. determine onboarding/app-lock/navigation state;
5. rebuild eligible reminders and supporting reminder categories where applicable;
6. reconcile overdue reminder state where applicable;
7. surface non-fatal recovery failures through safe/redacted diagnostics rather than raw exception data.

Startup recovery is designed to be repeatable. It must not create duplicate reminder identities.

## Onboarding flow

1. show local-first/privacy and medical/reminder limitations;
2. collect initial local profile information;
3. optionally configure app-lock protection;
4. persist local settings/profile state;
5. navigate to the normal application shell.

Notification permission is not requested solely because onboarding is running.

## Profile creation/edit flow

```text
Profile editor
  -> ViewModel validates required user-entered values
  -> profile application service/repository
  -> SQLite transaction/write
  -> audit metadata where applicable
  -> navigation refresh
```

No remote identity/account is created.

## Profile archive/delete flow

Archive:

- preserves local data while suppressing applicable automatic reminder materialization.

Delete/reset:

- requires explicit destructive user action;
- relies on repository/migration relationship rules for related local cleanup;
- should be preceded by an encrypted backup/export if the user wants a copy.

## Medicine flow

Medicine editor stores user-entered fields including name, strength text, instruction text, dates, lifecycle state, and optional stock/refill values.

Critical rule:

`Strength` and `Instructions` remain opaque text. They are never parsed into dosage/frequency logic.

Lifecycle changes such as pause/complete/archive affect automatic reminder eligibility but do not rewrite user-entered medicine text.

## Schedule creation/edit flow

1. user chooses a schedule kind;
2. user enters explicit dates, times, interval/cycle/weekday values as applicable;
3. UI/ViewModel builds `MedicineSchedule` + `ScheduleTime` records;
4. domain rules validate shape and time-zone identity;
5. schedule is saved through application/repository services;
6. reminder permission can be requested when a reminder-capable schedule is explicitly enabled;
7. reminder materialization/scheduling is rebuilt.

Validation protects configuration integrity; it does not decide clinically appropriate timing.

## Reminder materialization flow

```text
Repository returns enabled schedules
  -> coordinator loads medicine
  -> coordinator loads owning profile
  -> coordinator loads schedule times
  -> ReminderPlanner validates ownership/window/time zone
  -> planner produces stable future ReminderOccurrence values
  -> repository upserts occurrences
  -> coordinator schedules supported platform notifications
```

Ownership rules:

- medicine belongs to profile;
- schedule belongs to medicine;
- each schedule time belongs to schedule.

Window rules:

- bounds are UTC;
- lower bound inclusive;
- upper bound exclusive;
- upper bound later than lower bound.

## Reminder identity and idempotency

Occurrence identity is deterministic from schedule/local scheduled value/time zone/follow-up status.

This allows rebuild operations after startup, time-zone changes, platform recovery events, or diagnostics without intentionally generating a new identity for the same occurrence.

Duplicate explicit clock times collapse to one stable occurrence key.

## Daylight-saving flow

For each local user-entered clock time:

1. build local scheduled date/time under the schedule time zone;
2. if the local time is invalid because of a DST gap, do not invent a replacement occurrence;
3. if the local time is ambiguous because of a DST overlap, choose the deterministic offset rule;
4. convert to UTC;
5. apply half-open UTC planning-window filtering.

## Reminder state-change flow

```text
User action
  -> ReminderCoordinator.HandleOccurrenceAsync
  -> validate requested state transition inputs
  -> update occurrence state/time
  -> cancel existing platform notification registration
  -> optionally reschedule snooze
  -> optionally create medication-log entry
  -> optionally apply user-configured stock change for Taken
  -> write safe audit metadata
```

Snooze requires an explicit future UTC time.

## Taken/stock flow

1. a user marks an occurrence Taken;
2. medication log entry is recorded;
3. if the medicine has explicit user-configured stock-change-per-Taken value, calculate current local estimate;
4. prevent an automatic change that would make the estimate negative;
5. save a stock adjustment;
6. if a user-configured threshold is crossed, schedule a local stock reminder where supported.

No quantity is inferred from strength or instruction text.

## Appointment flow

Appointment editor:

- stores local user-entered appointment information;
- can rebuild appointment reminders where implemented;
- can explicitly export calendar information.

Calendar export is a privacy-boundary crossing controlled by user action.

## Document import flow

```text
User chooses import/capture source
  -> app reads selected bytes
  -> encrypted document storage creates protected local payload
  -> metadata record is persisted
  -> document appears in local organizer
```

Document contents are not placed into routine diagnostic logs.

## Document export/share flow

1. user explicitly selects an export/share action;
2. CareNest decrypts/creates the intended export copy;
3. target platform share/file APIs receive the copy;
4. the exported copy is outside CareNest's encrypted-vault boundary.

CareNest does not automatically upload documents in v1.

## Report flow

1. user selects profile/report type;
2. application/repository reads applicable local data;
3. report service renders CSV/PDF/structured output;
4. safety/privacy limitation text is included where applicable;
5. user explicitly chooses where to save/share the output.

Reports do not create clinical conclusions.

## Backup creation flow

```text
User chooses backup + password
  -> SQLite WAL checkpoint/snapshot
  -> package required local database/document recovery material
  -> derive encryption key from password
  -> authenticated encryption
  -> write manual backup file
```

The backup password is not uploaded to a CareNest service.

## Restore flow

1. user chooses a backup file and provides password;
2. validate backup magic/version/format;
3. derive key and authenticate/decrypt protected payload;
4. validate schema/package integrity;
5. stage restore data;
6. replace/recover local storage according to restore implementation;
7. rebuild derived reminder registrations/state as needed;
8. preserve rollback/failure safety if validation fails before replacement.

A failed authentication/wrong password must not be treated as a valid backup.

## App-lock enable flow

1. validate numeric PIN policy;
2. generate random salt;
3. derive PBKDF2-HMAC-SHA256 verifier;
4. save enabled flag + salt + verifier to platform secure secret storage;
5. clear temporary verifier buffers where practical.

No plaintext PIN is persisted.

## App-lock verification flow

1. read salt/verifier from secure storage;
2. derive candidate verifier from user-entered PIN;
3. fixed-time compare;
4. clear derived/retrieved verifier byte buffers where managed-memory control permits;
5. allow/deny local UI access.

App lock is not whole-database encryption.

## Diagnostics flow

Diagnostic surfaces must prefer operational metadata over user data.

Allowed examples:

- exception type/category;
- permission state;
- schema version;
- safe time-zone/reminder capability status;
- aggregate storage information.

Prohibited routine log content includes:

- health-document bytes/content;
- medicine/profile sensitive free text;
- raw health-record identifiers where avoidable;
- PINs;
- backup passwords;
- encryption keys;
- full exception messages/stack traces from user-data operation paths.

See `docs/security/LOGGING_PRIVACY.md`.

## External project-support flow

1. user explicitly presses project-support action;
2. CareNest opens fixed HTTPS destination `https://buymeacoffee.com/sanskarIN`;
3. no CareNest health record is intentionally appended to the destination URL;
4. the browser/external service becomes a separate trust/privacy boundary.

Funding does not alter CareNest health behavior or entitlements.

## Failure-handling principles

- validate before destructive writes;
- prefer cancellation-aware async operations;
- avoid synchronous task blocking in runtime source;
- keep reminder rebuild idempotent;
- keep sensitive diagnostics redacted;
- do not hide real analyzer/test failures by broad suppressions;
- keep manual platform limitations explicit.

## Future networked flows

Cloud sync, remote caregiver collaboration, remote identity, or server-side health storage are deliberately not part of this v1 flow model. Any future addition requires separate consent, authentication, key management, deletion/export, threat-model, abuse, conflict-resolution, and privacy design before implementation.