# CareNest Notifications and Platform Behavior

This document defines the current CareNest reminder/notification model for Android, Windows, iOS/iPadOS, and Mac Catalyst.

CareNest notifications are local organizational reminders based only on explicit user-entered schedule/appointment settings. They are not clinical alarms and do not determine dosage, treatment, medication appropriateness, or medical urgency.

## Current automated baseline

Marker-only PR #56 is the current release-engineering source baseline:

- source/base: `4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`;
- marker head: `e3bc621cea05364a69abee0dadbd71a67c17bddb`;
- CareNest CI #571 / `31770929379`: success;
- 122 unit + 39 integration + 124 UI-contract/policy = **285/285** tests;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #571 / `31770929382`: success;
- unsuppressed Dependency Audit #41 / `31770929383`: success.

PR #56 was closed without merge; its marker is not part of `main`.

Hosted compilation and source contracts do **not** prove real notification delivery under every device/OS state.

## Three distinct state surfaces

CareNest distinguishes:

1. **User schedule intent** — explicit medicine schedule/appointment values.
2. **Persisted CareNest state** — reminder occurrence/appointment records in local storage.
3. **Operating-system request state** — notification/alarm requests owned by the platform.

These surfaces are coordinated but are not one transaction.

A persisted reminder row does not prove the OS currently has a matching request, and an OS request does not replace CareNest’s persisted state/audit model.

## Common medicine reminder flow

```text
Explicit user schedule
  -> ReminderPlanner
  -> persisted ReminderOccurrence
  -> ReminderCoordinator
  -> reconciliation / quiet-hours / permission policy
  -> INotificationService
  -> platform notification/alarm API
  -> OS-controlled delivery/display
```

## Explicit schedule intent only

CareNest never parses medicine strength, instructions, symptoms, diagnosis or health notes to infer schedule frequency.

Automatic medicine occurrences come only from explicit supported schedule configuration such as:

- Daily;
- Selected weekdays;
- Every N hours;
- Cycle/custom date range;
- explicit local clock time(s);
- explicit interval/day/date values;
- explicit schedule time-zone ID.

As-needed schedules do not generate automatic occurrences.

Disabled schedules, archived profiles and paused/completed/archived medicines do not automatically materialize reminders.

## Deterministic planner

`ReminderPlanner` is platform-neutral and deterministic.

It validates:

- profile → medicine ownership;
- medicine → schedule ownership;
- persisted schedule-time → schedule ownership;
- known schedule kind;
- explicit valid time zone;
- date/state boundaries;
- actual UTC planning bounds;
- half-open `[fromUtc, toUtc)` window;
- explicit interval/cycle/weekdays/time shape.

Duplicate clock times collapse to stable occurrence identity.

The planner never calls OS notification APIs directly.

## DST behavior

CareNest preserves explicit user clock intent rather than inventing a replacement time.

- nonexistent spring-forward local time is not silently shifted;
- ambiguous fall-back local time resolves deterministically;
- invalid Every-N-Hours DST-gap anchors fail closed rather than shifting the user-entered anchor.

This is deterministic scheduling behavior, not medical guidance.

## Effective due time

For upcoming/overdue/reconciliation behavior:

- normal Scheduled occurrence → `ScheduledUtc`;
- valid Snoozed occurrence → `SnoozedUntilUtc`.

A future snooze remains upcoming even after the original scheduled time has passed.

An overdue snooze is evaluated using the snooze due time, not the stale original schedule instant.

## Permission timing

CareNest does not request notification permission just because onboarding or startup runs.

Permission is associated with explicit reminder-capable user actions where a prompt is appropriate.

If permission is denied:

- local organizational data can still be stored;
- the app reports the limitation;
- the application does not represent the platform notification as scheduled;
- background/rebuild paths do not repeatedly prompt;
- platform diagnostics/settings remain available to the user.

## Rebuild and platform reconciliation

Rebuild reconciles persisted occurrence rows with existing OS requests.

For an occurrence that currently records a platform request:

1. determine current eligibility and effective due time;
2. attempt old platform-request cancellation before replacement, quiet-hour suppression, or invalidation;
3. if cancellation fails, keep state retryable instead of falsely marking reconciliation complete;
4. if the occurrence is no longer valid, persist cancellation/invalid state only after platform cleanup succeeds where applicable;
5. if quiet-hours/current policy suppresses it, do not create a replacement request;
6. otherwise schedule the current request;
7. persist the new platform request identifier only after successful scheduling.

This ordering prevents stale OS requests from surviving merely because SQLite rows were replaced first.

## Schedule changes

When schedule details change, CareNest intentionally preserves enough old future occurrence identity for OS cancellation/reconciliation.

Old occurrence rows are not blindly removed before obsolete platform requests can be cancelled.

After successful reconciliation, current planner output represents the current explicit future schedule.

## Medicine/profile save behavior

Changes affecting reminder eligibility trigger reconciliation, including examples such as:

- profile archive/unarchive;
- medicine pause/resume/complete/archive;
- schedule/date/time changes.

Reconciliation happens before later non-critical audit bookkeeping can incorrectly turn an already-applied primary data change into an apparent failure.

## Medicine/profile delete behavior

Database cascade deletion and OS request cancellation are separate state surfaces.

CareNest therefore:

1. cancels future platform requests for the relevant medicine/profile;
2. attempts the structured database cascade;
3. if the cascade fails after platform cancellation, attempts non-cancelled reminder rebuild compensation for records that still exist;
4. surfaces recovery failure rather than silently claiming consistency.

## Handled reminder actions

Current handled actions include:

- Taken;
- Skipped;
- Delayed;
- Missed;
- Snoozed;
- Cancelled.

Normal ordering:

1. validate action and explicit snooze input where applicable;
2. cancel the old platform request;
3. only after cancellation succeeds, persist handled state;
4. for Snoozed, schedule a replacement only after state persistence;
5. create/update medication-log state where applicable;
6. apply only explicit user-configured stock bookkeeping after Taken;
7. record later non-critical audit metadata best effort.

CareNest does not knowingly commit a handled state while a known old platform request remains live because its cancellation failed.

## Reminder action compensation

If the old OS request was cancelled but a later essential persistence/scheduling step fails:

1. attempt non-cancelled restoration of the previous occurrence state;
2. attempt a non-cancelled reminder rebuild so a still-actionable platform request can be restored;
3. if recovery also fails, surface aggregate failure rather than claiming the database/platform state is consistent.

Post-success audit/stock bookkeeping does not intentionally roll back an already completed user action.

## Snooze contract

Snooze requires:

- an explicit value;
- actual UTC kind at the application boundary;
- a value later than current UTC.

`SnoozedUntilUtc` becomes effective due time while snoozed.

Quiet-hours/current scheduling policy may suppress the platform replacement without changing the underlying user-entered medicine schedule intent.

## Quiet hours

Quiet hours are an organizational preference, not clinical guidance.

During reconciliation, an existing OS request can be cancelled when current quiet-hours policy now suppresses that occurrence. CareNest does not merely skip creation of a duplicate while knowingly leaving an old request active.

## Follow-up reminders

Follow-up state/category is based on current explicit application configuration and stable occurrence/request identity.

It is not inferred from medicine strength, diagnosis, symptoms or adherence interpretation.

## Appointment reminder model

Appointments follow a separate application-service path from medicine `ReminderOccurrence` materialization.

`Appointment.StartsUtc` has a strict contract:

- `StartsUtc.Kind` must be `DateTimeKind.Utc`;
- local/unspecified values are rejected;
- CareNest does not use `DateTime.SpecifyKind` to relabel local clock ticks as UTC;
- `TimeZoneId` is validated/trimmed separately for intent/presentation context;
- reminder due time derives only from validated UTC start plus user-entered `ReminderMinutesBefore`.

## Appointment permission behavior

When an explicit appointment save needs a future notification and permission is currently denied:

1. the explicit user action may trigger a permission request;
2. if permission remains denied, the appointment stays saved locally but no platform schedule is attempted;
3. later background/rebuild work does not prompt again and does not schedule while denied.

A stored non-UTC appointment encountered during rebuild fails closed rather than being silently reinterpreted.

## Appointment persistence compensation

Appointment database persistence and platform scheduling are separate surfaces.

The service uses reconciliation/compensation so failure after one surface changes does not silently leave contradictory appointment/platform state.

Deleting an appointment cancels its CareNest platform request before deleting the record.

## Notification content privacy

Default notification content is intentionally generic.

Do not include:

- imported document content;
- private notes;
- passwords/PINs/keys;
- diagnostic stack traces;
- unnecessary health-record identifiers.

The OS controls lock-screen presentation, notification history and platform persistence.

Users should review device notification-preview settings for their privacy needs.

## Startup recovery

Startup recovery treats major categories independently where possible:

- overdue reminder reconciliation;
- medicine reminder rebuild;
- appointment reminder rebuild;
- backup-reminder synchronization.

Caller cancellation propagates.

A non-cancellation failure in one recovery category is privacy-safely contained so later recovery categories can still be attempted where designed.

Later startup/foreground passes can retry prior failures.

# Platform behavior

## Android

Android notification behavior can depend on:

- notification permission;
- Android/target SDK behavior;
- exact/inexact alarm capability/policy;
- battery optimization;
- manufacturer background restrictions;
- force-stop state;
- reboot;
- clock/time-zone changes;
- OS scheduling policy.

### Android recovery lifecycle

Android boot/time/time-zone recovery uses `BroadcastReceiver.GoAsync()` and keeps the `PendingResult` alive until asynchronous recovery completes, with `Finish()` performed in `finally`.

This prevents the receiver lifetime from ending before its asynchronous recovery attempt finishes.

A receiver failure is contained so later normal startup can retry.

### Android alarm/battery behavior

CareNest surfaces platform limitations instead of promising exact delivery on every Android device.

If exact scheduling capability or background execution is restricted, diagnostics/limitations should be reported and the implemented fallback behavior used.

### Android manual release matrix

Verify on representative real/emulated targets:

- fresh install/onboarding;
- notification permission denied/granted;
- actual scheduled notification delivery;
- future snooze after original due time;
- overdue snooze handling;
- cancellation-first Taken/Skipped/Delayed/Missed/Snoozed/Cancelled;
- stale request cancellation after schedule changes;
- medicine/profile delete request cleanup;
- exact/inexact alarm diagnostics;
- battery optimization behavior;
- reboot recovery;
- clock/time-zone change recovery;
- force-stop limitation behavior/messaging;
- packaged SQLite existing-data compatibility.

Hosted Android compilation is not proof of all vendor/device behaviors.

## Windows

The current Windows implementation includes an in-process fallback and does not claim guaranteed closed-app background delivery.

Timer ownership protections include:

- timer lifetime independent of the caller’s short-lived cancellation token;
- cancellation/disposal ownership separation;
- old timer removes the dictionary entry only if it still owns that occurrence ID;
- rapid same-ID replacement cannot be undone by an older timer’s cleanup;
- display failures are contained.

Manual Windows testing must cover:

- delivery while app is running;
- documented closed-app limitation;
- same-ID replacement/cancellation race;
- cancellation-first handled actions;
- snooze replacement;
- restart/recovery behavior;
- packaged SQLite existing-data compatibility.

## iOS / iPadOS

Apple notification behavior is subject to user permission and OS policy.

CareNest does not claim guaranteed delivery.

Manual release testing includes:

- permission denied/granted;
- local notification scheduling/delivery;
- effective future snooze due time;
- cancellation-first handled actions;
- snooze replacement;
- stale request reconciliation after state/schedule changes;
- foreground/background/restart behavior;
- time-zone changes;
- packaged SQLite existing-data compatibility;
- notification-preview privacy/accessibility checks.

Production device/App Store builds additionally require signing/provisioning outside Git.

## Mac Catalyst

Mac Catalyst follows Apple notification/permission behavior and additionally requires desktop interaction checks.

Manual release checks include:

- notification permission/delivery;
- cancellation-first handled actions;
- snooze/stale-request reconciliation;
- app restart behavior;
- keyboard/focus behavior;
- packaged SQLite existing-data compatibility;
- signing/package verification.

# Backup reminder behavior

Backup reminders are organizational reminders driven by explicit settings and backup timestamps.

The backup reminder coordinator:

- cancels existing request when disabled;
- does not prompt for permission during background synchronization;
- schedules only when permission is already granted or an explicit user flow successfully obtains it;
- derives due time from configured interval/current/last-backup state;
- avoids deliberately scheduling in the past;
- honors configured sound/vibration where supported.

Backup reminders never upload a backup or password.

# Logging and diagnostics

Reminder/notification logging follows `docs/security/LOGGING_PRIVACY.md`.

Allowed safe diagnostic context can include:

- operation/recovery category;
- permission/capability state;
- exception type name.

Routine sensitive failures must not log:

- medicine/profile private content;
- document contents;
- backup password;
- app-lock PIN;
- cryptographic keys;
- raw sensitive exception messages/stack traces.

# Automated coverage

Automated tests/contracts protect behavior including:

- planner ownership/UTC/date/state/DST boundaries;
- snooze validation/effective due time;
- stale request reconciliation;
- cancellation-first action ordering;
- failure injection/recovery;
- medicine/profile compensation;
- appointment permission/UTC/compensation;
- Android receiver async lifetime;
- Windows timer race ownership;
- logging privacy;
- release workflow/script policy.

PR #56 passed 285 core tests plus all four platform Release builds, CodeQL and unsuppressed Dependency Audit.

# Production release boundary

A fully green hosted matrix cannot prove real notification behavior on every target/device/OS state.

Before final public production promotion, complete the applicable manual matrix for:

- permissions;
- actual delivery;
- cancellation-first action behavior;
- snooze/stale-request reconciliation;
- restart/reboot/time/time-zone behavior;
- Android alarm/battery restrictions;
- Windows limitation behavior;
- packaged SQLite existing-data compatibility;
- accessibility/privacy previews.

Exact production tags matching `v*` also run CareNest CI, CodeQL, Dependency Audit, Release Gate and Release Evidence against the exact tagged commit. Publication remains blocked until the tagged workflows and all applicable manual/store/signing/package evidence are complete.

## Related documents

- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`
- `docs/testing/TESTING_GUIDE.md`
- `docs/testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`
- `docs/architecture/APPLICATION_FLOWS.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/releases/MANUAL_TEST_MATRIX.md`
- `docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md`
