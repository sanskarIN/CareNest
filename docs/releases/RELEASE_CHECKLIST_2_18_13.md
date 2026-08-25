# CareNest 2.18.13 Release Checklist

**Target:** `2.18.13`  
**Build/package code:** `21813`  
**MAUI Controls:** `10.0.100`  
**State:** PREPARATION — NOT RELEASED

This checklist supplements `RELEASE_CHECKLIST.md`, `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`, `PRODUCTION_EVIDENCE_INDEX.md` and the canonical evidence templates. It does not replace them.

## Source and metadata

- [x] Central semantic version set to `2.18.13`.
- [x] Assembly/file version set to `2.18.13.0`.
- [x] MAUI display version set to `2.18.13`.
- [x] MAUI package/build code set to `21813`.
- [x] `Microsoft.Maui.Controls` baseline retained at `10.0.100`.
- [x] Version-consistency contract rolled forward to `2.18.13`.
- [x] Active release-line alignment contract added.
- [x] Version preparation record added.
- [x] Draft release notes added.
- [x] Active `PROJECT_STATUS.md` aligned to the `2.18.13` preparation boundary.
- [x] Active `docs/releases/NEXT_STEPS.md` aligned to the `2.18.13` preparation boundary.
- [x] Active `what_changed.md` handoff aligned to the `2.18.13` preparation boundary.
- [x] `CHANGELOG.md` records the `2.18.13` maintenance preparation.

## Starting repository boundary

Preparation branch: `continue/prepare-2.18.13-20260825`.

Starting `main`: `b2db4821047dbfb7fe223961fc237afcdfc8371e`.

That starting source includes PR #86, whose exact head `e14a40d095a6f39993a0f62e497f15ec4668701f` passed:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts.

Those checks are historical evidence for the prior exact source only. They do not satisfy the final `2.18.13` exact-head acceptance rows below.

## Exact-head automated acceptance

Complete these only from observed results on the final `2.18.13` pull-request head/merge ref.

- [ ] Final exact-head CareNest CI succeeds.
- [ ] Final exact-head CodeQL succeeds.
- [ ] Final exact-head unsuppressed Dependency Audit succeeds.
- [ ] Final exact-head Store Package Configuration succeeds.
- [ ] Final exact-head Store Inspection Artifacts succeeds.
- [ ] Unit tests pass on the accepted exact source.
- [ ] Integration tests pass on the accepted exact source.
- [ ] UI/source-policy tests pass on the accepted exact source.
- [ ] Platform-neutral formatting passes.
- [ ] Stable documentation-link verification passes.
- [ ] Android Release build passes.
- [ ] Windows Release build passes.
- [ ] iOS simulator Release build passes.
- [ ] Mac Catalyst Release build passes.
- [ ] Linux desktop Release build passes.
- [ ] WebAssembly browser Release publish passes.

Do not pre-fill counts, workflow IDs or run conclusions before they are actually observed on the final source.

## Repository follow-through

- [ ] Merge the `2.18.13` preparation PR only after required exact-head checks are green.
- [ ] Use an expected-head lock for the merge so a moved head cannot be accepted accidentally.
- [ ] Record the accepted branch source, PR merge ref and resulting `main` commit.
- [ ] Promote dynamic automated baseline/status records only from actually observed results.
- [ ] Preserve any transient infrastructure failure/retry history rather than rewriting it as an uninterrupted pass.

## Production compatibility validation

These rows remain open because source/CI success is not runtime evidence.

- [ ] Packaged existing-data/SQLite compatibility recorded with representative synthetic data.
- [ ] Packaged encrypted-document compatibility recorded.
- [ ] Packaged backup create/inspect/restore compatibility recorded.
- [ ] Genuine historical-backup compatibility recorded only where genuine prior bytes safely exist.
- [ ] Reminder rebuild/reconciliation behavior validated after packaged upgrade/restore.

## Android production validation

- [ ] Representative installed-device validation recorded.
- [ ] Notification permission denied/granted behavior recorded.
- [ ] Actual medicine/appointment reminder delivery recorded.
- [ ] Reminder actions, snooze, replacement and cancellation recorded.
- [ ] Restart/reboot/time-zone/DST recovery recorded.
- [ ] Exact/inexact alarm and battery/vendor behavior recorded.
- [ ] Documents/share/backup/app-lock behavior recorded.
- [ ] TalkBack/large-text checks recorded.

## Windows production validation

- [ ] Intended installed package/update path validated.
- [ ] Core CRUD/navigation validated in installed form.
- [ ] Running-app reminder behavior validated.
- [ ] Closed-app limitations/messaging validated.
- [ ] Reminder replacement/cancellation/actions/snooze/recovery validated.
- [ ] Documents/share/backup/app-lock validated.
- [ ] Keyboard/focus/Narrator/large-text/theme checks recorded.
- [ ] Existing-data packaged upgrade behavior recorded.

## iPhone/iPad production validation

- [ ] Signed/provisioned real-device install and upgrade validated.
- [ ] Notification permission and actual delivery validated.
- [ ] Reminder actions/snooze/reconciliation validated.
- [ ] Lifecycle/restart/time-zone behavior validated.
- [ ] Documents/share/backup/app-lock validated.
- [ ] Dynamic Type/VoiceOver/notification-preview privacy validated.

Simulator compilation is not real-device notification evidence.

## Mac Catalyst production validation

- [ ] Intended installed application path validated.
- [ ] Notification permission/delivery/actions/snooze validated.
- [ ] Lifecycle/restart/time-zone behavior validated.
- [ ] File picker/share/backup/app-lock validated.
- [ ] Keyboard/focus/VoiceOver/large-text/theme/contrast validated.
- [ ] Signed/notarized candidate behavior recorded where applicable.

## Linux desktop production validation

- [ ] Exact source/artifact/checksum and runtime environment recorded.
- [ ] Representative distribution/desktop/display-server behavior recorded.
- [ ] Launch/window lifecycle and scaling validated.
- [ ] Filesystem/runtime/package prerequisites validated.
- [ ] Persistence/reminder/secure-storage/file/share behavior recorded only where implemented.
- [ ] Keyboard/focus/assistive-technology checks recorded where applicable.
- [ ] Unsupported/unimplemented capabilities remain explicitly non-PASS.

## Browser/WebAssembly production validation

- [ ] Exact source/deployment/provenance and hosting origin recorded.
- [ ] Representative browser/engine/OS boundaries recorded.
- [ ] Startup/static-asset/WebAssembly loading validated.
- [ ] Viewport/zoom/keyboard/reload/navigation behavior validated.
- [ ] Storage/persistence/quota/private-mode behavior recorded where implemented.
- [ ] Notification/background/file/camera behavior recorded only where implemented.
- [ ] Unsupported capabilities fail clearly or remain unavailable.
- [ ] Screen-reader/focus/zoom behavior recorded where applicable.
- [ ] No hidden analytics/telemetry/network upload introduced.

## Accessibility validation

- [ ] TalkBack validation recorded where applicable.
- [ ] VoiceOver validation recorded where applicable.
- [ ] Narrator validation recorded where applicable.
- [ ] Linux assistive-technology validation recorded where applicable.
- [ ] Browser screen-reader validation recorded for represented boundaries.
- [ ] Reading/focus order and names/roles/states/hints validated.
- [ ] Large text/display scaling/browser zoom validated.
- [ ] Light/dark/system contrast and color-independent meaning validated.
- [ ] Reduced-motion and destructive-confirmation readability validated.
- [ ] Privacy-safe actionable errors validated.

## Signing, provenance and final artifacts

- [ ] Android production signing configured through secure external tooling.
- [ ] Apple signing/provisioning configured through secure external tooling.
- [ ] Windows signing configured where required by the selected distribution path.
- [ ] Mac notarization evidence recorded where applicable.
- [ ] Linux package/channel provenance recorded where applicable.
- [ ] Browser origin/TLS/deployment ownership provenance recorded where applicable.
- [ ] Exact final production package/deployment SHA-256/provenance generated.
- [ ] Store-safe scanner passes on exact final distributed app packages.
- [ ] No private signing material or secrets committed.

Unsigned automated inspection artifacts are not signed production packages.

## Store, distribution and publication

- [ ] Current Google Play requirements/declarations reviewed on actual submission day where applicable.
- [ ] Current Apple requirements reviewed on actual submission day where applicable.
- [ ] Current Microsoft/Windows requirements reviewed on actual submission day where applicable.
- [ ] Actual Linux distribution-channel requirements reviewed where applicable.
- [ ] Actual browser hosting/privacy/security requirements reviewed where applicable.
- [ ] Store/deployment metadata, screenshots and privacy text reconciled with the exact build.
- [ ] Submission/deployment state recorded.
- [ ] Review/rejection/remediation state recorded where applicable.
- [ ] Approval state recorded only after actual approval.
- [ ] Publication/deployment state recorded only after actual publication/deployment.
- [ ] Draft release notes reconciled with the exact production-approved source/package.
- [ ] Immutable `v2.18.13` tag created only when production gates permit it.
- [ ] Tagged release gates pass for the approved immutable tag.

## Final rule

CareNest `2.18.13` is currently **NOT RELEASED**.

Do not describe it as production released, fully platform-parity verified, accessibility-complete, production signed, store approved or globally defect-free until the corresponding evidence actually exists for the exact source/package/deployment being promoted.
