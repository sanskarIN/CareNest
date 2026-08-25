# CareNest Next Steps

**Date:** 2026-08-25  
**Active preparation line:** `2.18.13`  
**Package/build code:** `21813`  
**Release state:** PREPARED IN SOURCE — NOT PUBLISHED  
**Preparation branch:** `continue/prepare-2.18.13-20260825`  
**Starting `main`:** `b2db4821047dbfb7fe223961fc237afcdfc8371e`  
**Last accepted automated evidence authority:** `AUTOMATED_BASELINE.md`

CareNest `2.18.13` is a maintenance continuation from the verified `2.18.12` baseline and the exact-head-green post-merge governance work merged through PR #86. The active branch must earn its own verification results; older green workflows are not reused as proof for newer source.

The remaining work is split into two categories:

1. source-side preparation and exact-head verification that can be completed in the repository;
2. genuine production/manual evidence that cannot be truthfully completed from CI or source inspection.

---

## Current authorities

- `VERSION_2_18_13_PREPARATION.md` — active source/version boundary;
- `RELEASE_NOTES_2_18_13_DRAFT.md` — draft notes, not published notes;
- `RELEASE_CHECKLIST_2_18_13.md` — active version-specific checklist;
- `AUTOMATED_BASELINE.md` — last accepted observed exact-source automation;
- `RELEASE_CHECKLIST.md` — stable release checklist;
- `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` — fail-closed evidence semantics;
- `PRODUCTION_EVIDENCE_INDEX.md` — evidence workflow/templates;
- `PACKAGED_RELEASE_VALIDATION.md` — package/manual validation runbook;
- `PACKAGE_EVIDENCE_TOOLING.md` — package provenance/checksum tooling;
- `VERIFICATION_BRANCH_PROTOCOL.md` — exact-source verification rules;
- `../setup/CROSS_PLATFORM.md` — Linux/browser architecture/capability boundaries;
- `../../PROJECT_STATUS.md` — current dynamic project status;
- `../../what_changed.md` — current continuation handoff.

---

# Priority 0 — finish the 2.18.13 source preparation

## 1. Repository metadata and version package

- [x] central semantic/assembly/file/informational version set to `2.18.13`;
- [x] MAUI display version set to `2.18.13`;
- [x] MAUI package/build code set to `21813`;
- [x] `Microsoft.Maui.Controls` retained at `10.0.100`;
- [x] version-consistency contract rolled to `2.18.13`;
- [x] version preparation record added;
- [x] draft release notes added;
- [x] version-specific release checklist added;
- [x] `PROJECT_STATUS.md` aligned;
- [x] `NEXT_STEPS.md` aligned;
- [ ] `what_changed.md` aligned;
- [ ] changelog updated with the new preparation line.

## 2. Exact-head verification and merge

The final branch head must independently complete the configured matrix before merge.

- [ ] open the `2.18.13` preparation pull request;
- [ ] freeze/record the final candidate head for acceptance;
- [ ] CareNest CI succeeds on that exact head;
- [ ] CodeQL succeeds on that exact head;
- [ ] unsuppressed Dependency Audit succeeds on that exact head;
- [ ] Store Package Configuration succeeds on that exact head;
- [ ] Store Inspection Artifacts succeeds on that exact head;
- [ ] record actual unit/integration/UI-source-policy counts from the run;
- [ ] record actual platform build/publish conclusions from the run;
- [ ] preserve any retry/failure history rather than hiding it;
- [ ] merge only with an expected-head lock after required workflows are green;
- [ ] promote dynamic automated evidence/status only from actually observed post-merge values.

Queued, skipped, cancelled, failed, superseded or older-source workflows are not success evidence for the final head.

---

# Priority 1 — packaged compatibility evidence

Use fictional/synthetic data and release-specific copies of the canonical records.

## 3. Existing-data / SQLite compatibility

- [ ] prepare representative earlier-candidate data;
- [ ] record origin version/build/source/schema where known;
- [ ] install/upgrade through intended production package paths;
- [ ] confirm SQLite opens;
- [ ] run and record integrity validation;
- [ ] verify representative records remain readable/editable;
- [ ] verify expected schema migrations;
- [ ] verify reminder rebuild/reconciliation;
- [ ] verify no duplicate/stale platform request remains in the tested boundary;
- [ ] record exact package/source/checksum/device evidence.

## 4. Encrypted document / backup compatibility

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
- [ ] representative normal backups remain below current resource ceilings;
- [ ] genuine historical encrypted backup validation only where genuine prior bytes safely exist.

Never manufacture a current artifact and label it historical evidence.

---

# Priority 2 — platform production validation

## 5. Android

Use a release-specific copy of `templates/ANDROID_DEVICE_VALIDATION_RECORD.md`.

- [ ] fresh install/onboarding;
- [ ] notification permission denied/granted;
- [ ] medicine/appointment reminder create-edit-delete;
- [ ] actual reminder delivery;
- [ ] Taken/Skipped/Delayed/Missed behavior;
- [ ] snooze replacement/cancellation;
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

## 6. Windows

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

## 7. iPhone/iPad

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

## 8. Mac Catalyst

Use a release-specific copy of `templates/MACCATALYST_VALIDATION_RECORD.md`.

- [ ] intended install/execution path;
- [ ] notification permission/delivery;
- [ ] reminder actions/snooze/reconciliation;
- [ ] restart/lifecycle/time-zone behavior;
- [ ] file picker/share;
- [ ] backup/restore;
- [ ] app lock;
- [ ] keyboard/focus;
- [ ] VoiceOver/large-text/theme/contrast;
- [ ] signed/notarized candidate behavior where available.

## 9. Linux desktop

Use release-specific copies of `templates/LINUX_DESKTOP_VALIDATION_RECORD.md`.

- [ ] record exact source/artifact/checksum/runtime environment;
- [ ] record distribution/version, desktop/display server and architecture;
- [ ] verify launch/window lifecycle;
- [ ] verify resize/display-scaling/high-DPI behavior;
- [ ] verify filesystem/runtime/package prerequisites;
- [ ] record X11/Wayland behavior where actually tested;
- [ ] record persistence/reminder/secure-storage/file/share behavior only where implemented;
- [ ] perform applicable keyboard/focus/assistive-technology validation;
- [ ] keep unsupported/unimplemented rows explicitly non-PASS.

## 10. Browser/WebAssembly

Use release-specific copies of `templates/BROWSER_VALIDATION_RECORD.md`.

- [ ] record exact source/published artifact/provenance/hosting origin;
- [ ] record browser/version/engine and OS/device;
- [ ] verify startup/static asset/WebAssembly loading;
- [ ] verify viewport/zoom/keyboard behavior;
- [ ] record refresh/reload/navigation-away behavior;
- [ ] identify/validate persistence mechanism if implemented;
- [ ] test storage quota/denial/clearing/private mode where applicable;
- [ ] record notifications/background behavior only if implemented;
- [ ] record file/camera permissions only if implemented;
- [ ] verify unsupported capabilities fail clearly or remain unavailable;
- [ ] verify no hidden analytics/telemetry/network upload was added;
- [ ] test screen-reader/focus/zoom behavior where applicable;
- [ ] keep untested browsers explicitly `NOT RUN`.

---

# Priority 3 — accessibility, signing and distribution

## 11. Accessibility

Use release-specific copies of `templates/ACCESSIBILITY_VALIDATION_RECORD.md`.

- [ ] TalkBack where applicable;
- [ ] VoiceOver where applicable;
- [ ] Narrator where applicable;
- [ ] Linux assistive technology where represented;
- [ ] browser screen reader where represented;
- [ ] reading/focus order;
- [ ] names/roles/states/hints;
- [ ] large text/display scaling/browser zoom;
- [ ] desktop keyboard/focus;
- [ ] light/dark/system contrast;
- [ ] color-independent meaning;
- [ ] reduced-motion behavior;
- [ ] destructive-confirmation readability;
- [ ] privacy-safe actionable errors.

## 12. Signing, notarization and deployment provenance

Use a release-specific copy of `templates/SIGNING_PROVENANCE_RECORD.md`. Keep secrets outside Git.

- [ ] Android production signing;
- [ ] Apple signing/provisioning;
- [ ] Windows signing where required;
- [ ] Mac notarization where applicable;
- [ ] Linux package/channel provenance where applicable;
- [ ] browser origin/TLS/deployment ownership provenance where applicable;
- [ ] safe public signing identifiers/fingerprints where appropriate;
- [ ] post-signing/deployment artifact hashes;
- [ ] no private signing material committed.

## 13. Final package/deployment evidence

- [ ] use production-stage evidence tooling where applicable;
- [ ] require immutable `v2.18.13` tag only after production approval permits tagging;
- [ ] require tag/source/checked-out HEAD identity agreement;
- [ ] require clean tracked workspace;
- [ ] record non-secret signing/notarization/store/deployment provenance;
- [ ] pass store-safe payload scanner for distributed app payloads;
- [ ] record per-file/top-level SHA-256 where supported;
- [ ] keep evidence JSON outside package payload;
- [ ] independently cross-check payload SHA-256;
- [ ] retain evidence with release records.

---

# Priority 4 — submission and publication

## 14. Submission/deployment-day review

- [ ] re-open current Apple rules on the actual submission date where applicable;
- [ ] re-open current Google Play rules on the actual submission date where applicable;
- [ ] re-open current Microsoft/Windows rules on the actual submission date where applicable;
- [ ] review actual Linux distribution-channel requirements where applicable;
- [ ] review browser hosting/privacy/security requirements for the actual deployment;
- [ ] complete live health/privacy declarations where applicable;
- [ ] verify final organizer claims/disclaimers and reminder limitation wording;
- [ ] verify fictional-data screenshots match the exact package/deployment;
- [ ] verify support/privacy/terms/security destinations;
- [ ] record submission, review, rejection/remediation, approval and publication as separate states.

## 15. Freeze and approve the exact production source

Only after applicable production findings are resolved:

- [ ] select the exact approved production commit;
- [ ] repeat exact-source automated verification after any correction;
- [ ] verify final version/build/release notes against approved source;
- [ ] verify final package/deployment hashes/provenance/evidence;
- [ ] verify no unresolved production blocker remains;
- [ ] create immutable approved tag `v2.18.13`;
- [ ] run required tagged release gates;
- [ ] do not move a failed/rejected tag to another commit.

## Final rule

CareNest `2.18.13` is currently **source-prepared/in preparation and NOT PUBLISHED**.

Do not convert unresolved production rows to `PASS` from CI, source inspection, assumption or intended future work. Unknown/unperformed work must remain `NOT RUN`, `BLOCKED`, `N/A` or `FAIL` as actually justified.
