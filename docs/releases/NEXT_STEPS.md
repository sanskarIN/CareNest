# CareNest Next Steps

**Date:** 2026-08-23  
**Release line:** `1.0.0-rc.1`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`  
**Current automated evidence authority:** `AUTOMATED_BASELINE.md`  
**Current verification-relevant continuation:** PR `#84` — `continue/cross-platform-current-main-20260823`

The active checklist that preceded this cross-platform current-main continuation is preserved exactly at:

`docs/history/cross-platform-before-next-steps-20260823/NEXT_STEPS.md`

The established MAUI RC1 runtime feature scope remains source-complete. The current continuation adds cross-platform presentation/build reach plus fail-closed governance for Linux desktop and browser/WebAssembly without pretending that build support is full production feature parity.

## Current authorities

- `AUTOMATED_BASELINE.md` — latest accepted actually observed exact-source automation;
- `RELEASE_CHECKLIST.md` — stable release checklist, now including Linux/browser rows;
- `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` — fail-closed evidence semantics;
- `PRODUCTION_EVIDENCE_INDEX.md` — canonical evidence workflow/templates;
- `templates/LINUX_DESKTOP_VALIDATION_RECORD.md` — Linux manual production evidence record;
- `templates/BROWSER_VALIDATION_RECORD.md` — browser/WebAssembly manual production evidence record;
- `../setup/CROSS_PLATFORM.md` — Linux/browser build, architecture and capability boundaries;
- `VERIFICATION_BRANCH_PROTOCOL.md` — exact-head verification procedure;
- `PACKAGED_RELEASE_VALIDATION.md` — package/manual validation runbook;
- `PACKAGE_EVIDENCE_TOOLING.md` — final package evidence tooling;
- `../../PROJECT_STATUS.md` — current dynamic project status;
- `../../what_changed.md` — current continuation handoff.

---

## 1. Current continuation — implemented source/governance work

PR #84 reconstructs the useful Linux/WebAssembly work directly on the current `main` after PR #82 merged, rather than relying on the stale/diverged base of PR #83.

Completed on PR #84:

- [x] central Avalonia package baseline;
- [x] shared `CareNest.CrossPlatform` Avalonia application/views;
- [x] `CareNest.CrossPlatform.Desktop` host for Linux-capable desktop builds;
- [x] `CareNest.CrossPlatform.Browser` WebAssembly/browser host;
- [x] browser bootstrap assets;
- [x] solution registration for all three Avalonia projects;
- [x] Linux desktop CI Release build;
- [x] browser WebAssembly CI Release publish;
- [x] Avalonia desktop/browser dependency-audit paths;
- [x] tagged release-gate Linux/browser build/publish job;
- [x] preservation of merged PR #82 production-evidence release-gate requirements;
- [x] fail-closed `build/scripts/verify-cross-platform-targets.py`;
- [x] isolated `build/scripts/test-verify-cross-platform-targets.py` regression self-tests;
- [x] verifier mutation case for missing desktop startup wiring;
- [x] verifier mutation case for malformed Avalonia XAML;
- [x] verifier mutation case for unsafe pre-completed browser evidence;
- [x] Linux desktop canonical production-validation template;
- [x] browser/WebAssembly canonical production-validation template;
- [x] production evidence index expanded to Linux/browser;
- [x] Release Gate requires both Linux/browser canonical evidence templates;
- [x] xUnit production-evidence template set expanded to Linux/browser;
- [x] dedicated `CrossPlatformEvidenceContractTests.cs` added;
- [x] stable release-governance tests expanded to cross-platform evidence/tooling;
- [x] root README updated for configured six-family platform reach and parity boundaries;
- [x] `docs/README.md` updated as the current documentation hub;
- [x] `docs/DOCUMENTATION_CATALOG.md` updated for current cross-platform authority/navigation;
- [x] `PROJECT_STATUS.md` updated to the actual PR #84 continuation;
- [x] `RELEASE_CHECKLIST.md` expanded with Linux/browser automated and manual gates;
- [x] previous active handoff/status/catalog/next-steps versions preserved under `docs/history/`.

None of those checked source/governance rows means Linux/browser production feature parity has been proven.

---

## 2. Immediate Priority 0 — finish PR #84 exact-head automation

Before PR #84 can be merged or used as a newer accepted automated baseline:

- [ ] freeze the final verification-relevant PR #84 source head;
- [ ] confirm CareNest CI completes successfully for that exact final head;
- [ ] confirm cross-platform verifier passes;
- [ ] confirm cross-platform verifier self-tests pass;
- [ ] confirm stable documentation-link verification passes;
- [ ] confirm unit tests pass and record the actual observed count;
- [ ] confirm integration tests pass and record the actual observed count;
- [ ] confirm UI/source-policy tests pass and record the actual observed count;
- [ ] confirm Android Release build passes;
- [ ] confirm Windows Release build passes;
- [ ] confirm iOS simulator Release build passes;
- [ ] confirm Mac Catalyst Release build passes;
- [ ] confirm Linux Avalonia Desktop Release build passes;
- [ ] confirm Avalonia Browser WebAssembly publish passes;
- [ ] confirm Dependency Audit passes, including Avalonia desktop/browser graphs;
- [ ] confirm CodeQL passes;
- [ ] confirm Store Package Configuration passes for its configured targets;
- [ ] confirm Store Inspection Artifacts passes for its configured targets;
- [ ] reject any required run that is failed, stale, cancelled, skipped, superseded, queued or from an older PR head;
- [ ] correct any real failure instead of suppressing or documenting it as success;
- [ ] merge PR #84 only after exact-head required evidence is green;
- [ ] keep/close PR #83 as superseded after PR #84 safely replaces it.

Do not copy the accepted test inventory from `AUTOMATED_BASELINE.md` onto PR #84 before the PR #84 matrix actually reports its own counts.

---

## 3. Automated baseline promotion — only after real final-head success

After a newer exact source actually completes all required automated checks:

- [ ] record the exact verified source SHA and merge identity where applicable;
- [ ] record actual CareNest CI workflow/run IDs;
- [ ] record actual unit/integration/UI test totals from that source;
- [ ] record Android/Windows/iOS simulator/Mac Catalyst build results;
- [ ] record Linux desktop build result;
- [ ] record browser WebAssembly publish result;
- [ ] record Dependency Audit result;
- [ ] record CodeQL result;
- [ ] record Store Package Configuration result;
- [ ] record Store Inspection Artifacts result;
- [ ] update `AUTOMATED_BASELINE.md` only from those actually observed values;
- [ ] update dynamic `PROJECT_STATUS.md`, `NEXT_STEPS.md` and `what_changed.md` without modifying stable source merely to record dynamic evidence.

A green automated matrix still does not complete production/manual rows below.

---

# Priority 0 — production evidence still required

## 4. Packaged existing-data / SQLite compatibility

Use fictional/synthetic data and a release-specific copy of `templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`.

- [ ] prepare representative earlier-candidate data;
- [ ] record origin version/build/source/schema where known;
- [ ] install/upgrade through each intended production package path;
- [ ] confirm SQLite opens;
- [ ] run/record integrity validation;
- [ ] verify representative records remain readable/editable;
- [ ] verify expected schema migrations;
- [ ] verify reminder rebuild/reconciliation;
- [ ] verify no duplicate/stale platform request remains in the tested boundary;
- [ ] record exact package/source/checksum/device evidence.

## 5. Encrypted document / backup compatibility

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

## 6. Android validation

Use `templates/ANDROID_DEVICE_VALIDATION_RECORD.md` for every representative device/build boundary.

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

## 7. Windows validation

Use `templates/WINDOWS_VALIDATION_RECORD.md`.

- [ ] intended install/execution path;
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
- [ ] Narrator/large-text/theme checks.

## 8. iPhone/iPad validation

Use `templates/IOS_DEVICE_VALIDATION_RECORD.md`.

- [ ] signed/provisioned real-device install;
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

## 9. Mac Catalyst validation

Use `templates/MACCATALYST_VALIDATION_RECORD.md`.

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

## 10. Linux desktop validation

Use a release-specific copy of `templates/LINUX_DESKTOP_VALIDATION_RECORD.md` for every Linux distribution/environment represented as production-supported.

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

## 11. Browser/WebAssembly validation

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

## 12. Accessibility validation

Use `templates/ACCESSIBILITY_VALIDATION_RECORD.md` for representative platform/assistive-technology combinations, including Linux/browser combinations if those hosts are intended for release.

- [ ] screen-reader validation;
- [ ] reading/focus order;
- [ ] names/roles/states/hints;
- [ ] large text/display scaling/browser zoom;
- [ ] desktop keyboard/focus;
- [ ] light/dark/system contrast;
- [ ] color-independent meaning;
- [ ] reduced-motion behavior;
- [ ] destructive confirmation readability;
- [ ] privacy-safe actionable errors.

## 13. Production signing/notarization/deployment provenance

Use `templates/SIGNING_PROVENANCE_RECORD.md`. Keep secrets outside Git.

- [ ] Android production signing configured outside Git;
- [ ] Apple signing/provisioning configured outside Git;
- [ ] Windows signing configured outside Git where applicable;
- [ ] Linux package provenance/signing recorded where the chosen channel requires it;
- [ ] browser hosting origin/TLS/deployment ownership/provenance recorded where applicable;
- [ ] safe public signing identifiers/fingerprints recorded where appropriate;
- [ ] post-signing/deployment artifact hashes recorded;
- [ ] no private signing material committed.

## 14. Final package/deployment evidence

For each intended production artifact/deployment:

- [ ] use production-stage package evidence tooling where applicable;
- [ ] require immutable `v*` tag;
- [ ] require tag/source/checked-out HEAD identity agreement;
- [ ] require clean tracked workspace;
- [ ] record non-secret signing/notarization/store/deployment provenance;
- [ ] pass store-safe payload scanner for distributed app payloads;
- [ ] record per-file and top-level SHA-256 where supported;
- [ ] keep evidence JSON outside package payload;
- [ ] independently cross-check payload SHA-256;
- [ ] retain evidence with release records.

## 15. Submission/deployment-day policy and metadata

Use `templates/STORE_SUBMISSION_RECORD.md` where a store/channel is applicable.

- [ ] re-open official Apple rules on actual submission date where applicable;
- [ ] re-open official Google Play rules on actual submission date where applicable;
- [ ] re-open official Microsoft/Windows rules on actual submission date where applicable;
- [ ] review the actual Linux distribution channel requirements where applicable;
- [ ] review browser hosting/privacy/security requirements for the actual deployment environment;
- [ ] complete live Google Play Health apps declaration where applicable;
- [ ] complete live Google Play Data safety answers where applicable;
- [ ] complete Apple privacy/store metadata where applicable;
- [ ] complete Microsoft/Partner Center privacy/store metadata where applicable;
- [ ] verify final health-organizer claims/disclaimers;
- [ ] verify reminder limitation wording;
- [ ] verify fictional-data screenshots match exact package/deployment;
- [ ] verify support/privacy/terms/security destinations;
- [ ] record submission/deployment, approval and publication as separate states.

Preliminary review remains at `STORE_POLICY_REVIEW_20260818.md`; it is not store approval.

## 16. Freeze exact production source/tag

Only after applicable production findings are resolved:

- [ ] select exact approved production commit;
- [ ] repeat exact-source automated verification after any verification-relevant change;
- [ ] verify final version/build/release notes;
- [ ] verify final package/deployment hashes/provenance/evidence JSON where applicable;
- [ ] verify no unresolved production blocker remains;
- [ ] create immutable approved `v*` tag;
- [ ] run required tagged release gates.

Do not move a failed/rejected tag to another commit.

## 17. Final production approval/publication

Use a release-specific copy of `templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`.

- [ ] applicable package compatibility evidence passes;
- [ ] applicable MAUI device/platform evidence passes;
- [ ] applicable Linux desktop evidence passes when Linux is represented as production-supported;
- [ ] applicable browser evidence passes for each browser/deployment represented as production-supported;
- [ ] applicable accessibility evidence passes;
- [ ] applicable signing/notarization/deployment evidence passes;
- [ ] applicable final-package/deployment evidence passes;
- [ ] applicable store/distribution-policy blockers are resolved;
- [ ] exact approved source/tag/package/deployment hashes are recorded;
- [ ] approval decision is recorded;
- [ ] GitHub release published where intended;
- [ ] store/distribution/deployment publication evidence recorded;
- [ ] final status/changelog/next-steps updated.

---

## Continuation rule

CareNest remains `1.0.0-rc.1` until applicable production rows have real evidence.

If a real defect is reproduced, fix the smallest correct boundary, add regression coverage, freeze a replacement source and run the required exact-source matrix again. Do not create speculative health functionality or fabricate production evidence merely to increase commit count.