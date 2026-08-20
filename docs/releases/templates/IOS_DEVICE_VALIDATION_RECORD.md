# CareNest iPhone/iPad Device Validation Record

Use fictional/synthetic data only. Simulator compilation is not real-device notification evidence.

## Identity

- Result status: `NOT RUN`
- CareNest version/build:
- Source SHA:
- Source tag:
- Signed package/build identity:
- Package SHA-256 where available:
- Package-evidence JSON:
- Device model:
- iOS/iPadOS version:
- Provisioning/distribution channel:
- Validation date/time/time zone:
- Operator:

## Install and onboarding

- [ ] Signed/provisioned install succeeds on a real device.
- [ ] First launch succeeds without an account/network requirement.
- [ ] Notification permission is not requested before a reminder-capable action.
- [ ] Medical/reminder limitations are visible.

Evidence/notes:

## Notification permission and reminders

- [ ] Denied notification permission is detected and explained.
- [ ] Granted notification permission enables intended scheduling.
- [ ] Medicine reminder delivery is observed on the real device.
- [ ] Appointment reminder delivery is observed on the real device.
- [ ] Edit/delete cancellation behavior is verified.
- [ ] Taken/Skipped/Delayed/Missed behavior is verified.
- [ ] Snooze replacement/cancellation is verified.
- [ ] Notification preview privacy is verified against intended content.

Evidence/notes:

## Lifecycle and time behavior

- [ ] App close/reopen preserves expected state.
- [ ] Restart/reopen reconciliation is verified.
- [ ] Background/terminated behavior matches documented platform limitations.
- [ ] Time-zone change behavior is verified without silent schedule-time rewriting.
- [ ] DST behavior is verified where practical.

Evidence/notes:

## Data/security workflows

- [ ] Document picker/import/open/export/delete works.
- [ ] Failed export cleanup works.
- [ ] Backup create/inspect/restore works.
- [ ] Clean-install restore works.
- [ ] Restored encrypted documents remain usable.
- [ ] App lock enable/lock/unlock/disable works.

Evidence/notes:

## Accessibility

- [ ] VoiceOver validation completed for representative critical flows.
- [ ] Dynamic Type at representative large sizes remains usable.
- [ ] Focus/reading order is sensible.
- [ ] Destructive confirmations remain readable.
- [ ] Light/dark/system appearance remains usable.
- [ ] Status meaning is not color-only.

Evidence/notes:

## Failures/blockers

List every `FAIL`, `BLOCKED` or `N/A` row with reason and issue/PR reference where applicable.

## Final result

- Overall result: `NOT RUN`
- Blocking issue references:
- Retest package/source if required:
- Reviewer/sign-off:
