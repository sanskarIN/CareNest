# CareNest Notifications and Platform Behavior

This document explains how deterministic reminder occurrences relate to platform notification delivery on Android, iOS, Mac Catalyst, and Windows.

## Core distinction

CareNest separates two concepts:

1. **Reminder occurrence materialization** — deterministic application logic based on explicit user-entered schedules.
2. **Notification delivery** — operating-system behavior subject to platform permissions/policies/capabilities.

A successfully materialized occurrence does not guarantee that the OS will display a notification.

## Common notification flow

```text
Explicit user schedule
  -> ReminderPlanner
  -> persisted ReminderOccurrence
  -> ReminderCoordinator
  -> notification policy/quiet hours
  -> INotificationService
  -> platform notification/alarm API
  -> OS-controlled delivery/display
```

## Permission timing

CareNest does not request notification permission merely because onboarding is running.

Permission is requested when the user explicitly creates/saves/enables reminder-capable behavior that needs notification access.

If permission is denied:

- schedule data can still be saved;
- CareNest reports that notifications are not currently permitted;
- the user can review notification diagnostics/settings.

## Notification content privacy

Default notification labels are generic.

CareNest does not intentionally place:

- health-document contents;
- private notes;
- backup data;
- app-lock data;
- detailed sensitive medicine/profile information

into routine generic notification payloads.

Users still control OS lock-screen preview settings.

## Quiet hours

Quiet hours are a user-controlled notification policy.

If an occurrence is due inside configured quiet hours, supported scheduling may be suppressed according to the implementation.

Quiet hours do not rewrite the underlying user-entered medicine schedule.

## Follow-ups

Follow-up reminders are separate deterministic occurrences created from explicit user-configured follow-up minutes.

They have their own occurrence identity and do not change the original scheduled time.

## Snooze

A snoozed occurrence requires an explicit future UTC timestamp.

Coordinator validation rejects:

- missing snooze time;
- local/unspecified `DateTime.Kind`;
- a snooze time that is not in the future.

After valid snooze:

- existing platform notification registration is cancelled;
- supported future snooze notification is registered if not suppressed by quiet hours.

## Rebuild behavior

CareNest rebuilds/recovers future reminder registrations at appropriate application/platform recovery points.

Reasons include:

- app startup;
- schedule changes;
- supported boot/restart events;
- time/time-zone changes;
- explicit diagnostic/rebuild actions;
- restore/recovery flows.

Occurrence identity is deterministic to avoid duplicate application records for the same user schedule instance.

# Android

## Integration model

Android uses platform alarm/notification mechanisms with API-level guards and capability checks.

CareNest distinguishes exact-alarm availability from general reminder intent.

## Exact/inexact behavior

Exact alarm capability depends on Android version, permissions/capabilities, device policy, and target configuration.

When exact behavior is unavailable, CareNest surfaces limitation/fallback diagnostics rather than claiming exact delivery.

## Notification permission

Modern Android notification permission state can block notification display.

The app should:

- request at explicit reminder-capable action;
- handle denial without losing schedule data;
- expose diagnostics.

## Battery optimization

Android battery optimization/manufacturer background restrictions can delay/prevent background behavior.

CareNest can surface battery-optimization diagnostics but cannot override all OEM/OS policy.

## Boot/time/time-zone changes

Android receiver/integration handles supported system events such as:

- reboot/boot completion;
- time changes;
- time-zone changes.

The goal is to rebuild future registrations from stored schedule intent.

Stored schedule times are not silently rewritten to the new device zone.

## Force-stop

A force-stopped Android application can be prevented from receiving/scheduling expected background events until the user/system allows it again.

CareNest must not claim to defeat force-stop behavior.

## Android manual release tests

Required checks include:

- permission denied/granted;
- exact/inexact capability;
- battery optimization;
- reboot;
- time/time-zone change;
- notification tap/open behavior if applicable;
- synthetic reminder delivery under representative device states.

# iOS

## Integration model

CareNest uses iOS local notification APIs through the platform notification implementation.

## Permission

iOS user permission controls whether CareNest can present local notifications.

Denial does not delete the user's stored schedule intent.

## Delivery limitations

iOS controls final scheduling/delivery/display behavior.

CareNest cannot guarantee delivery during:

- device shutdown;
- revoked permission;
- OS policy changes;
- notification settings that suppress/hide alerts.

## Rebuild

Application startup/recovery can rebuild supported future reminder registrations from persisted occurrences/schedules.

## iOS manual tests

Verify:

- first permission request timing;
- denied/granted states;
- local notification scheduling;
- app restart;
- time-zone change behavior;
- lock-screen privacy presentation;
- tap/open behavior if implemented.

# Mac Catalyst

Mac Catalyst uses Apple local notification APIs under Mac Catalyst platform behavior.

Manual tests should cover:

- notification permission;
- delivery while application state changes;
- window/app restart;
- time-zone handling;
- notification privacy;
- system notification settings.

Mac Catalyst delivery remains OS-controlled.

# Windows

## Current limitation

CareNest's current Windows fallback does not claim guaranteed reminder delivery when CareNest is not running.

The app exposes this limitation through diagnostics/documentation rather than pretending to have a background scheduling guarantee it does not implement.

## Manual tests

Verify:

- in-process/open-app reminder behavior;
- diagnostic wording;
- no misleading background-delivery promise;
- app restart behavior;
- time-zone handling;
- keyboard/accessibility interaction with reminder screens.

# Time-zone and DST behavior

Platform notification registration receives UTC occurrence times produced from the deterministic planner.

Planner rules are platform-neutral:

- explicit schedule time zone;
- invalid spring-forward local time creates no invented replacement occurrence;
- ambiguous fall-back local time resolves deterministically;
- UTC planning window is half-open;
- stored local intent is not rewritten because device zone changed.

Platform APIs receive the resulting occurrence time; delivery remains OS-dependent.

## Missed/overdue reconciliation

CareNest can reconcile overdue scheduled occurrences into Missed organizational state according to application logic.

A Missed state is local organizational history, not a clinical assessment of adherence or harm.

## Notification failure logging

Platform scheduling exceptions are privacy-redacted.

CareNest logs safe operational metadata such as exception type/category when enabled and does not include occurrence/medicine identifiers in reminder scheduling failure logs.

## Test reminders

A test reminder/diagnostic action can verify that the platform path works in the current environment.

Success of one test does not guarantee future delivery under different permission/battery/background states.

## Release evidence

Automated CI verifies platform source compilation.

It does **not** prove real notification delivery.

Final production release requires manual evidence in `docs/releases/MANUAL_TEST_MATRIX.md`.

## Troubleshooting

See `docs/setup/TROUBLESHOOTING.md`.

## Related documents

- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`
- `docs/architecture/APPLICATION_FLOWS.md`
- `docs/USER_GUIDE.md`
- `docs/releases/MANUAL_TEST_MATRIX.md`
- `docs/releases/RELEASE_PROCESS.md`
- `docs/security/LOGGING_PRIVACY.md`