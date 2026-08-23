# CareNest Linux Desktop Validation Record

Use fictional/synthetic data only. This canonical template is not evidence until a release-specific copy is completed against an actual Linux build/runtime boundary.

The current Avalonia Linux host establishes configured desktop presentation/build reach. Do not mark feature-parity rows `PASS` unless the corresponding behavior is actually implemented and exercised on the named Linux package/runtime.

## Identity

- Result status: `NOT RUN`
- CareNest version/build:
- Source SHA:
- Source tag:
- Desktop publish filename/path:
- Package/archive SHA-256:
- Publish/runtime identifier:
- Package-evidence JSON if applicable:
- Linux distribution/version:
- Desktop environment/display server:
- Device/architecture:
- .NET/runtime mode:
- Validation date/time/time zone:
- Operator:

## Build and package boundary

- [ ] Source SHA/tag exactly matches the candidate being evaluated.
- [ ] `CareNest.CrossPlatform.Desktop` Release build/publish succeeds for the intended Linux runtime.
- [ ] Produced files are attributable to the recorded source and toolchain.
- [ ] Launch dependencies/runtime prerequisites are documented accurately.
- [ ] No repository-only Gumroad/Buy Me a Coffee promotional payload is introduced into the distributed CareNest host.

Evidence/notes:

## Launch and presentation

- [ ] Application starts on the named Linux distribution/desktop environment.
- [ ] Main window renders without startup exception or visibly broken layout.
- [ ] Window resize/minimize/restore/close behavior is usable.
- [ ] Keyboard focus is visible on representative interactive controls.
- [ ] Light/dark/system presentation behavior is recorded accurately where supported.
- [ ] The UI does not claim production feature parity that has not been implemented/validated.
- [ ] Medical/reminder limitations remain visible and accurate where surfaced.

Evidence/notes:

## Capability and parity boundary

For each capability, record `PASS`, `FAIL`, `BLOCKED`, `N/A`, or `NOT RUN`. `PASS` means the behavior exists and was actually exercised on this Linux candidate; compilation or UI text alone is not sufficient.

- Profiles/data persistence:
- Medicines/schedules:
- Reminder delivery/actions/background behavior:
- Appointments:
- Document vault/import/export:
- Backup/restore:
- Reports/export/share:
- Local app lock/secure secret storage:
- File picker/camera integration:
- Accessibility/assistive technology:

Evidence/notes:

## Linux-specific behavior

- [ ] Filesystem paths and permissions behave correctly for the selected publish/install model.
- [ ] Read-only/unwritable destination failures are handled without silent data loss where applicable.
- [ ] Display scaling/high-DPI behavior remains usable.
- [ ] X11/Wayland behavior is recorded for the environment actually tested.
- [ ] Suspend/resume/session restart behavior is recorded where applicable.
- [ ] Native/background notification capability is not inferred from the MAUI implementation.
- [ ] Secure-storage behavior is not claimed unless a Linux-specific implementation was exercised.

Evidence/notes:

## Accessibility

- [ ] Keyboard-only navigation is tested for the flows present in this host.
- [ ] Focus order and focus visibility are usable.
- [ ] Text/display scaling is usable.
- [ ] Status meaning is not color-only.
- [ ] Applicable Linux assistive-technology behavior is recorded in a release-specific accessibility record.

Evidence/notes:

## Privacy and safety

- [ ] Validation uses fictional/synthetic health-organizer data only.
- [ ] No password, PIN, private key, signing secret, token, recovery code, real prescription, real medical record or private backup is committed as evidence.
- [ ] Unsupported platform capabilities fail clearly rather than silently pretending to succeed.
- [ ] Local-first/account-free claims match the behavior actually observed.

Evidence/notes:

## Failures/blockers

List every `FAIL`, `BLOCKED` or `N/A` row with reason and issue/PR reference where applicable.

## Final result

- Overall result: `NOT RUN`
- Blocking issue references:
- Retest package/source if required:
- Reviewer/sign-off:
