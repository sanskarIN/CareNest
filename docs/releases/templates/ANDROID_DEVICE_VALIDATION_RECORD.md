# CareNest Android Device Validation Record

Use fictional/synthetic data only. This template is not evidence until completed against an actual intended package/device boundary.

## Identity

- Result status: `NOT RUN`
- CareNest version/build:
- Source SHA:
- Source tag:
- Package filename:
- Package SHA-256:
- Package/application ID:
- Package-evidence JSON:
- Device manufacturer/model:
- Android version/API level:
- Security patch level:
- Validation date/time/time zone:
- Operator:

## Installation and onboarding

- [ ] Fresh install succeeds.
- [ ] First launch succeeds without an account/network requirement.
- [ ] Medical limitations are visible.
- [ ] Backup responsibility is visible.
- [ ] Notification permission is not requested during onboarding before a reminder-capable action.

Evidence/notes:

## Notification permission and scheduling

- [ ] Denied notification permission is detected and explained.
- [ ] Granted notification permission enables intended scheduling path.
- [ ] Exact/inexact alarm capability is reported accurately where applicable.
- [ ] Battery/background restriction guidance is accurate for this device.
- [ ] Test reminder path behaves as documented.

Evidence/notes:

## Medicine reminder lifecycle

- [ ] Create reminder.
- [ ] Edit reminder and verify stale platform request cleanup.
- [ ] Delete reminder and verify cancellation.
- [ ] Actual reminder delivery observed.
- [ ] Taken action cancels/reconciles as expected.
- [ ] Skipped action cancels/reconciles as expected.
- [ ] Delayed action cancels/reconciles as expected.
- [ ] Missed-state reconciliation behaves as expected.
- [ ] Snooze replaces/cancels the intended request.
- [ ] Future-snooze edge case behaves as documented.

Evidence/notes:

## Appointment reminders

- [ ] Create appointment reminder.
- [ ] Edit appointment reminder.
- [ ] Delete appointment reminder.
- [ ] Actual appointment reminder delivery observed.

Evidence/notes:

## Lifecycle and recovery

- [ ] App close/reopen preserves expected state.
- [ ] Process restart/reopen rebuilds/reconciles reminders.
- [ ] Device reboot rebuilds intended future reminders.
- [ ] Clock change behavior is verified.
- [ ] Time-zone change behavior is verified without silently rewriting user schedule times.
- [ ] DST behavior is verified where the locale/date supports it.
- [ ] Force-stop limitation messaging is accurate.

Evidence/notes:

## Data and security workflows

- [ ] Profile/medicine deletion cleans related reminder state.
- [ ] Document import/open/export/delete works.
- [ ] Failed export leaves no unintended CareNest-owned plaintext.
- [ ] Manual encrypted backup create/inspect/restore works.
- [ ] Clean-install restore works.
- [ ] Restored encrypted documents remain usable.
- [ ] App lock enable/lock/unlock/disable works.

Evidence/notes:

## Accessibility and UI

- [ ] TalkBack validation completed for representative critical flows.
- [ ] Large text/display scaling remains usable.
- [ ] Light/dark/system themes are usable.
- [ ] Destructive confirmations remain readable.
- [ ] Status meaning is not color-only.

Evidence/notes:

## Failures/blockers

List every `FAIL`, `BLOCKED` or `N/A` row with reason and issue/PR reference where applicable.

## Final result

- Overall result: `NOT RUN`
- Blocking issue references:
- Retest package/source if required:
- Reviewer/sign-off:
