# CareNest Windows Validation Record

Use fictional/synthetic data only. This template is not evidence until completed against the intended Windows package/runtime boundary.

## Identity

- Result status: `NOT RUN`
- CareNest version/build:
- Source SHA:
- Source tag:
- Package filename/path:
- Package SHA-256:
- Package/application identity:
- Package-evidence JSON:
- Windows edition/version/build:
- Device/architecture:
- Validation date/time/time zone:
- Operator:

## Install and launch

- [ ] Intended install/execution path succeeds.
- [ ] First launch succeeds without an account/network requirement.
- [ ] Upgrade path from representative earlier package succeeds where applicable.
- [ ] Medical/reminder limitations are visible and accurate.

Evidence/notes:

## Core workflows

- [ ] Profiles CRUD.
- [ ] Medicines and schedules CRUD.
- [ ] Medication-log actions/history.
- [ ] Appointments CRUD/export.
- [ ] Documents import/open/export/delete.
- [ ] Reports/export.
- [ ] Settings/theme/accessibility preferences.

Evidence/notes:

## Reminder behavior

- [ ] Running-app reminder behavior verified.
- [ ] Same-ID replacement/cancellation verified.
- [ ] Taken/Skipped/Delayed/Missed behavior verified.
- [ ] Snooze behavior verified.
- [ ] Restart/reopen reconciliation verified.
- [ ] Closed-app limitation behavior/messaging matches actual platform behavior.
- [ ] Time-zone change behavior verified without silent schedule-time rewriting.

Evidence/notes:

## Backup, documents and app lock

- [ ] Encrypted document lifecycle works.
- [ ] Failed export cleanup works.
- [ ] Backup create/inspect/restore works.
- [ ] Wrong password fails closed.
- [ ] Tampered/truncated/trailing-data backup fails closed.
- [ ] Clean-install restore works.
- [ ] Restored encrypted documents remain usable.
- [ ] App lock enable/lock/unlock/disable works.

Evidence/notes:

## Desktop accessibility/input

- [ ] Keyboard-only navigation reaches representative critical controls.
- [ ] Focus is visible and ordered sensibly.
- [ ] Narrator validation completed for representative critical flows.
- [ ] Text/display scaling remains usable.
- [ ] Light/dark/system themes remain usable.
- [ ] Status meaning is not color-only.

Evidence/notes:

## Failures/blockers

List every `FAIL`, `BLOCKED` or `N/A` row with reason and issue/PR reference where applicable.

## Final result

- Overall result: `NOT RUN`
- Blocking issue references:
- Retest package/source if required:
- Reviewer/sign-off:
