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
5. reconcile overdue reminder state where applicable;
6. rebuild eligible medicine reminders;
7. rebuild appointment reminders;
8. synchronize backup reminder state;
9. surface non-fatal recovery failures through safe/redacted diagnostics rather than raw exception data.

Startup recovery steps are isolated so one non-cancellation failure does not intentionally stop every later recovery category. Caller cancellation still propagates.

Startup recovery is designed to be repeatable. It must not create duplicate reminder identities.

## Onboarding flow

1. show local-first/privacy and medical/reminder limitations;
2. validate profile/PIN input before partial setup begins;
3. create the initial local profile;
4. optionally configure app-lock protection;
5. persist default settings;
6. write onboarding-complete state last;
7. navigate to the normal application shell.

Notification permission is not requested solely because onboarding is running.

If onboarding fails after partial state is created, CareNest attempts non-cancelled compensation for the created app-lock/profile/completion state and surfaces incomplete rollback rather than silently claiming success.

## Profile creation/edit flow

```text
Profile editor
  -> ViewModel validates required user-entered values
  -> profile application service/repository
  -> SQLite transaction/write
  -> reminder reconciliation where profile state affects eligibility
  -> audit metadata where applicable
  -> navigation refresh
```

No remote identity/account is created.

Non-critical audit bookkeeping must not falsely turn an already-applied primary profile state transition into a failed data operation.

## Profile archive/delete flow

Archive:

- preserves local data while suppressing applicable automatic reminder materialization;
- triggers reminder reconciliation where eligibility changes.

Delete/reset:

- requires explicit destructive user action;
- cancels future platform reminder requests before the structured profile cascade;
- if the database cascade fails after cancellation, attempts a non-cancelled reminder rebuild for records that still exist;
- relies on repository/migration relationship rules for structured related cleanup;
- attempts encrypted document/profile-photo cleanup after successful structured deletion;
- should be preceded by an encrypted backup/export if the user wants a copy.

## Medicine flow

Medicine editor stores user-entered fields including name, strength text, instruction text, dates, lifecycle state, and optional stock/refill values.

Critical rule:

`Strength` and `Instructions` remain opaque text. They are never parsed into dosage/frequency logic.

Lifecycle changes such as pause/complete/archive affect automatic reminder eligibility but do not rewrite user-entered medicine text.

Medicine save reconciles reminder state before later non-critical audit bookkeeping. Medicine deletion cancels future platform requests before structured cascade and attempts non-cancelled rebuild compensation if persistence fails after cancellation.

## Schedule creation/edit flow

1. user chooses a schedule kind;
2. user enters explicit dates, times, interval/cycle/weekday values as applicable;
3. UI/ViewModel builds `MedicineSchedule` + `ScheduleTime` records;
4. domain rules validate shape and time-zone identity;
5. schedule is saved through application/repository services;
6. old future occurrence identity is retained long enough for platform-request reconciliation rather than being deleted before stale OS requests can be cancelled;
7. reminder permission can be requested when a reminder-capable schedule is explicitly enabled;
8. reminder materialization/reconciliation is rebuilt.

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
  -> coordinator reads actionable scheduled/snoozed rows by effective due time
  -> coordinator reconciles any existing platform request
  -> coordinator schedules a replacement only when still valid/eligible
```

Ownership rules:

- medicine belongs to profile;
- schedule belongs to medicine;
- each persisted schedule time belongs to schedule.

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

For `EveryNHours`, an invalid DST-gap anchor also fails closed rather than being silently shifted to a guessed clock time.

## Effective due-time flow

For reminder actions/upcoming/overdue behavior:

- normal Scheduled row → `ScheduledUtc`;
- Snoozed row with explicit snooze → `SnoozedUntilUtc`.

The original scheduled time remains part of schedule/occurrence identity, but a future snooze is not dropped simply because its original scheduled time is already in the past.

## Reminder rebuild/reconciliation flow

For an actionable future row:

1. determine whether it is still valid under current schedule/medicine/profile/date state;
2. calculate effective due time;
3. if a platform request identifier exists, attempt cancellation first;
4. if cancellation fails, keep state retryable and do not falsely mark reconciliation complete;
5. if the occurrence is no longer valid, mark it Cancelled after successful platform cleanup;
6. if quiet hours now suppress it, do not create a replacement;
7. otherwise schedule the current replacement request;
8. persist the current platform notification identifier only after successful scheduling.

SQLite occurrence state and OS scheduling are separate surfaces; rebuild is compensation/reconciliation, not a single cross-platform transaction.

## Reminder state-change flow

Handled actions use cancellation-first ordering:

```text
User action
  -> ReminderCoordinator.HandleOccurrenceAsync
  -> validate requested state transition inputs
  -> cancel existing platform notification registration
  -> persist requested handled occurrence state/time
  -> for Snoozed: optionally schedule replacement request
  -> create medication-log entry when applicable
  -> apply only explicit user-configured stock change for Taken
  -> write safe audit metadata best effort after primary success
```

Snooze requires an explicit future UTC time.

### Action failure recovery

If the old platform request was successfully cancelled but a later occurrence persistence or snooze replacement step fails:

1. attempt non-cancelled restoration of the previous occurrence state;
2. attempt a non-cancelled reminder rebuild so a still-actionable platform request can be restored;
3. if recovery itself fails, surface aggregate failure instead of claiming database/platform consistency.

If platform cancellation itself fails, the handled state is not knowingly committed while the old request is still considered live.

## Taken/stock flow

1. a user marks an occurrence Taken using the cancellation-first action flow;
2. medication log entry is recorded after the handled state transition succeeds;
3. if the medicine has explicit user-configured stock-change-per-Taken value, calculate current local estimate;
4. prevent an automatic change that would make the estimate negative;
5. save a stock adjustment;
6. if a user-configured threshold is crossed, schedule a local stock reminder where supported.

No quantity is inferred from strength or instruction text.

A later stock-bookkeeping failure does not intentionally roll back/falsify an already completed Taken action/log transition; it is contained and privacy-safely diagnosed.

## Appointment flow

Appointment editor stores local user-entered appointment information.

`StartsUtc` must be an explicit UTC `DateTime`; local/unspecified values are rejected rather than relabeled.

Appointment reminder flow:

1. persist validated appointment data;
2. evaluate explicit reminder lead time and notification permission;
3. request permission only from an explicit user action when appropriate;
4. if permission remains denied, do not schedule a platform request;
5. schedule/cancel the platform appointment reminder when eligible;
6. if later persistence/scheduling fails after one surface changed, attempt compensation/reconciliation rather than assuming SQLite and the OS scheduler are atomic;
7. background rebuild does not repeatedly request notification permission.

Calendar export is a separate privacy-boundary crossing controlled by user action.

## Document import flow

```text
User chooses import/capture source
  -> app reads selected bytes
  -> encrypted document storage creates protected local payload
  -> metadata record is persisted
  -> audit is recorded
  -> document appears in local organizer
```

Failure compensation:

- database-save failure removes the just-created encrypted payload;
- audit failure after metadata save attempts to remove both metadata and encrypted payload;
- rollback cleanup uses non-cancelled attempts;
- incomplete rollback is surfaced explicitly.

Document contents are not placed into routine diagnostic logs.

## Document master-key read/export flow

Existing encrypted payloads depend on the installation's stored document master key.

Read/export does not generate an unrelated replacement key if the existing key is missing/corrupt. Existing ciphertext plus missing/corrupt key fails closed.

## Document export/share flow

1. user explicitly selects an export/share action;
2. CareNest decrypts/creates the intended export copy under managed temporary ownership where applicable;
3. target platform share/file APIs receive the copy;
4. failed export cleans application-owned incomplete plaintext best effort;
5. successful exported/shared copies outside CareNest become the destination's responsibility.

CareNest does not automatically upload documents in v1.

## Report flow

1. user selects profile/report type;
2. application/repository reads applicable local data;
3. report service renders CSV/PDF/structured output to an incomplete staging path;
4. final output is atomically moved into place only after successful generation;
5. safety/privacy limitation text is included where applicable;
6. user explicitly chooses where to save/share the output;
7. after share handoff returns, CareNest removes the application-owned temporary report cache file where it still owns that copy.

Reports do not create clinical conclusions.

CSV string cells that begin with formula-like prefixes after optional whitespace are neutralized in the portable spreadsheet representation; the stored source record is not modified.

## Backup creation flow

```text
User chooses backup + password
  -> SQLite WAL checkpoint/snapshot
  -> package required local database/document recovery material
  -> derive encryption key from password
  -> authenticated chunked encryption
  -> write manual backup file
  -> best-effort local backup-history bookkeeping
```

New encrypted payload writes use authenticated framing v2; legacy v1 remains readable for compatibility.

The backup password is not uploaded to a CareNest service.

A completely written encrypted backup is not falsely reported as failed solely because later local backup-history metadata could not be recorded.

## Restore flow

1. user chooses a backup file and provides password;
2. validate backup magic/version/format;
3. derive key and authenticate/decrypt protected payload;
4. validate strict archive topology and schema/package integrity;
5. stage restore data;
6. replace/recover local database/document-key/document storage according to restore implementation;
7. rebuild derived reminder registrations/state as needed;
8. preserve rollback/failure safety if validation/replacement fails;
9. record post-success audit metadata best effort after the primary restore commits.

Wrong password/tamper/topology failure must not be treated as a valid backup.

If restore fails after changing the document key, rollback restores the exact previous secure-store byte state where previous bytes existed rather than silently normalizing unrelated prior state.

## App-lock enable/update flow

1. validate numeric PIN policy;
2. snapshot current secure-store enabled/salt/verifier state before replacing an existing lock;
3. generate random salt;
4. derive PBKDF2-HMAC-SHA256 verifier;
5. write enabled flag + salt + verifier to platform secure secret storage;
6. if a later write fails, attempt non-cancelled restoration of the previous state;
7. clear temporary verifier/salt buffers where practical.

No plaintext PIN is persisted.

## App-lock verification flow

1. read salt/verifier from secure storage;
2. require expected material lengths and valid entered PIN shape;
3. derive fixed-length candidate verifier;
4. fixed-time compare;
5. clear derived/retrieved verifier byte buffers where managed-memory control permits;
6. allow/deny local UI access.

Malformed/missing stored material fails closed. App lock is not whole-database encryption.

## App-lock disable flow

1. snapshot prior secure-store state;
2. remove enabled/salt/verifier values;
3. if a later removal fails, attempt non-cancelled restoration of the previous state;
4. surface aggregate failure if rollback is incomplete.

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

## Development quality/release flow

Local quality/preflight scripts:

- verify platform-neutral formatting;
- build/test the applicable source/test projects;
- run unsuppressed dependency audit as a blocking check;
- optionally audit/build a selected MAUI target in release preflight.

Exact-source marker PRs verify verification-relevant runtime/test/workflow/package/build-script source through CI, CodeQL, Dependency Audit, and all four platform Release builds.

Production tags matching `v*` run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Release Evidence attempts all core evidence components, records tracked-source provenance/checksums/run identity, uploads available evidence even on component failure, then applies an aggregate pass/fail gate.

A release tag still requires completed manual/device/accessibility/store/signing/packaged-data compatibility evidence before publication.

## Failure-handling principles

- validate before destructive writes;
- prefer cancellation-aware async operations;
- use non-cancelled compensation when cancellation should not knowingly strand inconsistent cross-surface state;
- avoid synchronous task blocking in runtime source;
- keep reminder planning/rebuild idempotent;
- reconcile database/filesystem/secure-store/OS state explicitly when one transaction cannot span them;
- keep sensitive diagnostics redacted;
- do not hide real analyzer/test/workflow/dependency failures by broad suppressions;
- keep manual platform limitations explicit.

## Future networked flows

Cloud sync, remote caregiver collaboration, remote identity, or server-side health storage are deliberately not part of this v1 flow model. Any future addition requires separate consent, authentication, key management, deletion/export, threat-model, abuse, conflict-resolution, and privacy design before implementation.
