# CareNest Next Steps

**Date:** 2026-08-24  
**Release line:** `2.18.12`  
**Release state:** PREPARED IN SOURCE — NOT PUBLISHED  
**Current automated evidence authority:** `AUTOMATED_BASELINE.md`  
**Merged source:** `ca80bd554296363d71a6008cac73c819be77b39b`

The checklist that preceded the current verified cross-platform baseline remains preserved at:

`docs/history/cross-platform-before-next-steps-20260823/NEXT_STEPS.md`

CareNest's current repository implementation and automated merge-readiness work are complete for the prepared `2.18.12` baseline. The remaining work is dominated by genuine production validation, signing/provenance, package/deployment evidence and store/publication outcomes. Those items must not be marked complete from source inspection or CI alone.

## Current authorities

- `AUTOMATED_BASELINE.md` — latest accepted actually observed exact-source automation;
- `RELEASE_CHECKLIST.md` — stable release checklist;
- `RELEASE_CHECKLIST_2_18_12.md` — version-specific preparation checklist;
- `VERSION_2_18_12_PREPARATION.md` — prepared source/version boundary;
- `RELEASE_NOTES_2_18_12_DRAFT.md` — draft notes, not published notes;
- `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` — fail-closed evidence semantics;
- `PRODUCTION_EVIDENCE_INDEX.md` — canonical evidence workflow/templates;
- `PACKAGED_RELEASE_VALIDATION.md` — package/manual validation runbook;
- `PACKAGE_EVIDENCE_TOOLING.md` — final package provenance/checksum tooling;
- `VERIFICATION_BRANCH_PROTOCOL.md` — exact-source verification rules;
- `../setup/CROSS_PLATFORM.md` — Linux/browser architecture and capability boundaries;
- `../../PROJECT_STATUS.md` — current dynamic project status;
- `../../what_changed.md` — current continuation handoff.

---

## 1. Repository and automated acceptance — complete

Observed exact-source acceptance for PR #84 is recorded in `AUTOMATED_BASELINE.md`.

- [x] PR #84 final branch head frozen for acceptance.
- [x] CareNest CI completed successfully for the accepted exact source/base combination.
- [x] Cross-platform target verifier passed.
- [x] Cross-platform verifier self-tests passed.
- [x] Stable documentation-link verification passed.
- [x] Platform-neutral formatting passed.
- [x] Unit tests passed: **122/122**.
- [x] Integration tests passed: **54/54**.
- [x] UI/source-policy tests passed: **215/215**.
- [x] Total core tests passed: **391/391**.
- [x] Android Release build passed.
- [x] Windows Release build passed after a same-source job-only retry for a transient workload-download `ResponseEnded` error.
- [x] iOS simulator Release build passed.
- [x] Mac Catalyst Release build passed.
- [x] Linux Avalonia Desktop Release build passed.
- [x] Avalonia Browser WebAssembly publish passed.
- [x] Dependency Audit passed, including platform-neutral, Avalonia desktop/browser and MAUI application graphs.
- [x] CodeQL passed.
- [x] Store Package Configuration passed.
- [x] Store Inspection Artifacts passed.
- [x] PR #83 closed as superseded.
- [x] `Microsoft.Maui.Controls` `10.0.100` integrated from the superseded Dependabot PR #85 and verified through PR #84.
- [x] PR #85 closed as superseded.
- [x] PR #84 merged into `main` at `ca80bd554296363d71a6008cac73c819be77b39b`.
- [x] Dynamic automated baseline promoted from observed values only.

The Windows first-attempt network failure remains part of the evidence history. It was not reclassified as a source success; the unchanged job was rerun and then actually passed.

---

# Priority 0 — production evidence still required

## 2. Packaged existing-data / SQLite compatibility

Use fictional/synthetic data and a release-specific copy of `templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`.

- [ ] prepare representative earlier-candidate data;
- [ ] record origin version/build/source/schema where known;
- [ ] install/upgrade through each intended production package path;
- [ ] confirm SQLite opens;
- [ ] run and record integrity validation;
- [ ] verify representative records remain readable/editable;
- [ ] verify expected schema migrations;
- [ ] verify reminder rebuild/reconciliation;
- [ ] verify no duplicate/stale platform request remains in the tested boundary;
- [ ] record exact package/source/checksum/device evidence.

## 3. Encrypted document / backup compatibility

Use fictional/synthetic data and the packaged compatibility record.

- [ ] packaged document import/open/export/delete;
- [ ] failed export cleanup;
- [ ] missing/corrupt key fail-closed behavior where safely testable;
- [ ] packaged backup create/inspect/restore;
- [ ] wrong-password rejection;
- [ ] tamper rejection;
- [ ] truncation rejection;
- [ ] trailing-data rejection;
- [ ] clean-install restore;
- [ ] restored encrypted-document usability;
- [ ] representative normal backups remain comfortably below current resource ceilings;
- [ ] genuine historical encrypted backup validation only where genuine prior bytes safely exist.

Never manufacture a current artifact and label it historical evidence.

---

## 4. Android production validation

Use a release-specific copy of `templates/ANDROID_DEVICE_VALIDATION_RECORD.md` for every representative device/build boundary.

- [ ] fresh install/onboarding;
- [ ] notification permission denied/granted;
- [ ] medicine/appointment reminder create-edit-delete;
- [ ] actual reminder delivery;
- [ ] Taken/Skipped/Delayed/Missed behavior;
- [ ] Snooze replacement/cancellation;
- [ ] stale-request cleanup;
- [ ] profile/medicine deletion cleanup;
- [ ] restart/reopen recovery;
- [ ] reboot rebuild;
- [ ] exact/inexact alarm behavior;
- [ ] battery/vendor restrictions;
- [ ] clock/time-zone/DST recovery;
- [ ] force-stop limitation messaging;
- [ ] document picker/share;
- [ ] backup/restore;
- [ ] app lock;
- [ ] TalkBack/large-text checks.

---

## 5. Windows production validation

Use a release-specific copy of `templates/WINDOWS_VALIDATION_RECORD.md`.

- [ ] intended install/execution/update path;
- [ ] core CRUD/navigation;
- [ ] running-app reminder behavior;
- [ ] closed-app limitation behavior/messaging;
- [ ] same-ID replacement/cancellation;
- [ ] reminder actions/snooze/reconciliation;
- [ ] restart/recovery;
- [ ] document picker/share;
- [ ] backup/restore;
- [ ] app lock;
- [ ] keyboard/focus;
- [ ] Narrator/large-text/theme checks;
- [ ] existing-data packaged upgrade behavior.

Automated Windows build success is not installed-package/manual-runtime evidence.

---

## 6. iPhone/iPad production validation

Use a release-specific copy of `templates/IOS_DEVICE_VALIDATION_RECORD.md`.

- [ ] signed/provisioned real-device install and upgrade;
- [ ] notification permission denied/granted;
- [ ] actual medicine/appointment notification delivery;
- [ ] reminder actions/snooze/reconciliation;
- [ ] restart/lifecycle behavior;
- [ ] time-zone/DST behavior;
- [ ] document picker/share;
- [ ] backup/restore;
- [ ] app lock;
- [ ] Dynamic Type;
- [ ] VoiceOver;
- [ ] notification-preview privacy.

Simulator compilation is not real-device notification evidence.

---

## 7. Mac Catalyst production validation

Use a release-specific copy of `templates/MACCATALYST_VALIDATION_RECORD.md`.

- [ ] intended install/execution path;
- [ ] notification permission/delivery;
- [ ] reminder actions/snooze/reconciliation;
- [ ] restart/lifecycle/time-zone behavior;
- [ ] file picker/share;
- [ ] backup/restore;
- [ ] app lock;
- [ ] keyboard/focus;
- [ ] VoiceOver/large-text/theme/contrast checks;
- [ ] signed/notarized candidate behavior when available.

---

## 8. Linux desktop production validation

Use release-specific copies of `templates/LINUX_DESKTOP_VALIDATION_RECORD.md` for every Linux distribution/environment represented as production-supported.

- [ ] record exact source/tag, publish path/runtime identifier and artifact checksum/provenance;
- [ ] record Linux distribution/version, desktop environment/display server and architecture;
- [ ] verify actual application launch/window lifecycle;
- [ ] verify resize/display-scaling/high-DPI behavior;
- [ ] verify filesystem/runtime/package prerequisite behavior;
- [ ] record X11/Wayland behavior for environments actually tested;
- [ ] record persistence behavior only if a Linux implementation exists;
- [ ] record reminder/background behavior only if a Linux implementation exists;
- [ ] record secure-storage behavior only if a Linux-specific implementation exists;
- [ ] record file/camera/share behavior only if implemented;
- [ ] perform applicable keyboard/focus/assistive-technology validation;
- [ ] keep unsupported/unimplemented rows `NOT RUN`, `BLOCKED` or defensible `N/A` rather than pretending parity.

A successful Avalonia Desktop build is not a manual Linux `PASS`.

---

## 9. Browser/WebAssembly production validation

Use release-specific copies of `templates/BROWSER_VALIDATION_RECORD.md` for each actually tested browser/deployment boundary.

- [ ] record exact source, published artifact/provenance and hosting origin;
- [ ] record browser name/version/engine and OS/device;
- [ ] verify startup/static asset/WebAssembly loading;
- [ ] verify representative viewport, zoom and keyboard behavior;
- [ ] record refresh/reload/navigation-away behavior;
- [ ] identify and validate the actual persistence mechanism if implemented;
- [ ] test storage quota/denial/clearing/private-mode behavior where applicable;
- [ ] record notifications/background behavior only if browser-specific capability is implemented;
- [ ] record file/camera permissions only if implemented;
- [ ] verify unsupported capabilities fail clearly or remain unavailable;
- [ ] verify no hidden analytics/telemetry/network upload was added;
- [ ] test screen-reader/focus/zoom behavior where applicable;
- [ ] keep every untested browser explicitly `NOT RUN`.

A successful WebAssembly publish is not a manual browser `PASS`.

---

## 10. Accessibility validation

Use release-specific copies of `templates/ACCESSIBILITY_VALIDATION_RECORD.md` for representative platform/assistive-technology combinations.

- [ ] TalkBack where applicable;
- [ ] VoiceOver where applicable;
- [ ] Narrator where applicable;
- [ ] Linux assistive technology where the represented environment supports it;
- [ ] browser screen-reader validation for represented browser/OS combinations;
- [ ] reading/focus order;
- [ ] names/roles/states/hints;
- [ ] large text/display scaling/browser zoom;
- [ ] desktop keyboard/focus;
- [ ] light/dark/system contrast;
- [ ] color-independent meaning;
- [ ] reduced-motion behavior;
- [ ] destructive confirmation readability;
- [ ] privacy-safe actionable errors.

---

## 11. Production signing, notarization and deployment provenance

Use a release-specific copy of `templates/SIGNING_PROVENANCE_RECORD.md`. Keep secrets outside Git.

- [ ] Android production signing configured through secure external tooling;
- [ ] Apple signing/provisioning configured through secure external tooling;
- [ ] Windows signing configured where the chosen distribution path requires it;
- [ ] Mac notarization evidence recorded where applicable;
- [ ] Linux package provenance/signing recorded where the chosen channel requires it;
- [ ] browser hosting origin/TLS/deployment ownership/provenance recorded where applicable;
- [ ] safe public signing identifiers/fingerprints recorded where appropriate;
- [ ] post-signing/deployment artifact hashes recorded;
- [ ] no private signing material committed.

---

## 12. Final package/deployment evidence

For every intended production artifact/deployment:

- [ ] use production-stage package evidence tooling where applicable;
- [ ] require immutable `v*` tag only after production approval permits tagging;
- [ ] require tag/source/checked-out HEAD identity agreement;
- [ ] require clean tracked workspace;
- [ ] record non-secret signing/notarization/store/deployment provenance;
- [ ] pass store-safe payload scanner for distributed app payloads;
- [ ] record per-file and top-level SHA-256 where supported;
- [ ] keep evidence JSON outside package payload;
- [ ] independently cross-check payload SHA-256;
- [ ] retain evidence with release records.

The existing unsigned inspection artifacts are automated inspection evidence, not signed production packages.

---

## 13. Submission/deployment-day policy and metadata

Use a release-specific copy of `templates/STORE_SUBMISSION_RECORD.md` where a store/channel is applicable.

- [ ] re-open official Apple rules on the actual submission date where applicable;
- [ ] re-open official Google Play rules on the actual submission date where applicable;
- [ ] re-open official Microsoft/Windows rules on the actual submission date where applicable;
- [ ] review actual Linux distribution-channel requirements where applicable;
- [ ] review browser hosting/privacy/security requirements for the actual deployment environment;
- [ ] complete live Google Play Health apps declaration where applicable;
- [ ] complete live Google Play Data safety answers where applicable;
- [ ] complete Apple privacy/store metadata where applicable;
- [ ] complete Microsoft/Partner Center privacy/store metadata where applicable;
- [ ] verify final health-organizer claims/disclaimers;
- [ ] verify reminder limitation wording;
- [ ] verify fictional-data screenshots match the exact package/deployment;
- [ ] verify support/privacy/terms/security destinations;
- [ ] record submission, review, rejection/remediation, approval and publication as separate states.

The dated preliminary review in `STORE_POLICY_REVIEW_20260818.md` remains useful background but is not store approval or a substitute for submission-day review.

---

## 14. Freeze the exact production source/tag

Only after applicable production findings are resolved:

- [ ] select the exact approved production commit;
- [ ] repeat exact-source automated verification after any verification-relevant correction;
- [ ] verify final version/build/release notes against the approved source;
- [ ] verify final package/deployment hashes/provenance/evidence JSON where applicable;
- [ ] verify no unresolved production blocker remains;
- [ ] create immutable approved tag `v2.18.12`;
- [ ] run required tagged release gates;
- [ ] do not move a failed or rejected tag to another commit.

The tag must not be created merely because source preparation and PR automation are green.

---

## 15. Final production approval/publication

Use a release-specific copy of `templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`.

- [ ] applicable package compatibility evidence passes;
- [ ] applicable MAUI device/platform evidence passes;
- [ ] applicable Linux desktop evidence passes if Linux is represented as production-supported;
- [ ] applicable browser evidence passes for each browser/deployment represented as production-supported;
- [ ] applicable accessibility evidence passes;
- [ ] applicable signing/notarization/deployment evidence passes;
- [ ] applicable final-package/deployment evidence passes;
- [ ] applicable store/distribution-policy blockers are resolved;
- [ ] actual store/channel approval is recorded where required;
- [ ] actual publication/deployment is recorded separately from approval;
- [ ] `RELEASE_NOTES_2_18_12_DRAFT.md` is reconciled with the exact approved package/source before becoming final release notes.

---

## Final rule

CareNest `2.18.12` is currently **automated-source accepted but not production released**.

Do not convert any unresolved production row to `PASS` from CI, source inspection, assumption or intended future work. Unknown or unperformed work must remain `NOT RUN`, `BLOCKED`, `N/A` or `FAIL` as actually justified.