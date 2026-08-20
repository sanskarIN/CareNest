# CareNest Next Steps

**Date:** 2026-08-19  
**Release line:** `1.0.0-rc.1`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`  
**Accepted automated source before this continuation:** `30ee6c265104c64ec5a1a4013f592f7f058750e8`  
**Merged executable-source commit:** `2549c08b25145f20c59b7e73ca227c35d5bbf0ec`  
**Accepted automated result before this continuation:** **370/370 core tests passed**

The complete active checklist from before the Gumroad rollout remains preserved at:

`docs/history/pre-gumroad-rollout-20260817/NEXT_STEPS.md`

The current RC1 runtime feature scope is source-complete. This file tracks only production validation/evidence work, release preparation that reduces that manual burden, or newly reproduced defects.

## Current authorities

- `AUTOMATED_BASELINE.md` — accepted automated baseline;
- `BACKUP_RESOURCE_HARDENING_20260819.md` — current backup resource hardening;
- `RELEASE_CHECKLIST.md` — current release checklist;
- `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` — evidence-quality rules;
- `PRODUCTION_EVIDENCE_INDEX.md` — production evidence templates and workflow;
- `PACKAGED_RELEASE_VALIDATION.md` — package/manual validation runbook;
- `PACKAGE_EVIDENCE_TOOLING.md` — final package evidence tooling.

---

## 1. Current continuation: production-evidence readiness

The accepted executable behavior is unchanged. This continuation addresses a concrete release-process gap: production-only validation had checklists but no uniform platform/evidence record set, and `RELEASE_CHECKLIST.md` still referenced an older 336-test baseline.

Implemented on the continuation branch:

- [x] define `PASS`, `FAIL`, `BLOCKED`, `N/A` and `NOT RUN` evidence semantics;
- [x] define required source/package/device/time-zone identity for manual evidence;
- [x] define secret/health-data exclusions for evidence files;
- [x] add Android device validation record;
- [x] add Windows validation record;
- [x] add iPhone/iPad device validation record;
- [x] add Mac Catalyst validation record;
- [x] add accessibility validation record;
- [x] add packaged existing-data/document/backup compatibility record;
- [x] add signing/provenance record;
- [x] add store policy/submission/approval/publication record;
- [x] add final production release approval record;
- [x] add production evidence index;
- [x] refresh `RELEASE_CHECKLIST.md` to the accepted 370-test baseline without marking manual work complete.

These repository/documentation changes are verification-relevant until a fresh pull-request matrix validates the final continuation head. Do not promote them as a replacement accepted exact-source baseline before that evidence exists.

---

## 2. Source/product work — complete for intended RC1 scope

Do not repeat broad speculative feature passes unless a real defect or changed requirement is discovered.

- [x] local-first/account-free product boundary;
- [x] profiles/family organization;
- [x] medicine records preserving user-entered strength/instruction text;
- [x] explicit schedules and deterministic reminder occurrences;
- [x] reminder action/reconciliation/compensation behavior;
- [x] appointment organization and reminders;
- [x] stock/refill organization from explicit user-entered values;
- [x] encrypted imported-document storage;
- [x] password-encrypted manual backup/restore;
- [x] bounded authenticated backup resource processing;
- [x] optional local app lock;
- [x] reports/exports with non-clinical limitations;
- [x] privacy-aware diagnostics;
- [x] strict XAML compiled-binding policy;
- [x] package external-commerce isolation;
- [x] package evidence tooling;
- [x] CodeQL/dependency/store/release gates;
- [x] repository/community/documentation baseline.

## 3. Accepted automated baseline before this continuation — complete

Exact source:

`30ee6c265104c64ec5a1a4013f592f7f058750e8`

Observed accepted evidence:

- [x] CareNest CI `32205946013` — success;
- [x] unit tests — **122/122**;
- [x] integration tests — **54/54**;
- [x] UI/source-policy tests — **194/194**;
- [x] total core tests — **370/370**;
- [x] Android Release — success;
- [x] Windows Release — success;
- [x] iOS simulator Release — success;
- [x] Mac Catalyst Release — success;
- [x] Store Package Configuration `32205946003` — success;
- [x] Store Inspection Artifacts `32205946001` — success;
- [x] CodeQL `32205946030` — success;
- [x] unsuppressed Dependency Audit `32205946026` — success.

Do not copy these results onto a newer verification-relevant head unless that head has real workflow evidence.

---

# Priority 0 — production evidence still required

## 4. Packaged existing-data / SQLite compatibility

Use fictional/synthetic data and `templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`.

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

Use fictional/synthetic data and the same packaged compatibility record.

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

## 10. Accessibility validation

Use `templates/ACCESSIBILITY_VALIDATION_RECORD.md` for representative platform/assistive-technology combinations.

- [ ] screen-reader validation;
- [ ] reading/focus order;
- [ ] names/roles/states/hints;
- [ ] large text/display scaling;
- [ ] desktop keyboard/focus;
- [ ] light/dark/system contrast;
- [ ] color-independent meaning;
- [ ] reduced-motion behavior;
- [ ] destructive confirmation readability;
- [ ] privacy-safe actionable errors.

## 11. Production signing/notarization

Use `templates/SIGNING_PROVENANCE_RECORD.md`. Keep secrets outside Git.

- [ ] Android production signing configured outside Git;
- [ ] Apple signing/provisioning configured outside Git;
- [ ] Windows signing configured outside Git where applicable;
- [ ] safe public signing identifiers/fingerprints recorded where appropriate;
- [ ] post-signing package hashes recorded;
- [ ] no private signing material committed.

## 12. Final package evidence

For each intended final package:

- [ ] use production-stage package evidence tooling;
- [ ] require immutable `v*` tag;
- [ ] require tag/source/checked-out HEAD identity agreement;
- [ ] require clean tracked workspace;
- [ ] record non-secret signing/notarization/store-managed provenance;
- [ ] pass store-safe payload scanner;
- [ ] record per-file and top-level SHA-256;
- [ ] keep evidence JSON outside package payload;
- [ ] independently cross-check payload SHA-256;
- [ ] retain evidence with release records.

## 13. Submission-day policy/store metadata

Use `templates/STORE_SUBMISSION_RECORD.md`.

- [ ] re-open official Apple rules on actual submission date where applicable;
- [ ] re-open official Google Play rules on actual submission date where applicable;
- [ ] re-open official Microsoft/Windows rules on actual submission date where applicable;
- [ ] complete live Google Play Health apps declaration where applicable;
- [ ] complete live Google Play Data safety answers where applicable;
- [ ] complete Apple privacy/store metadata where applicable;
- [ ] complete Microsoft/Partner Center privacy/store metadata where applicable;
- [ ] verify final health-organizer claims/disclaimers;
- [ ] verify reminder limitation wording;
- [ ] verify fictional-data screenshots match exact package;
- [ ] verify support/privacy/terms/security destinations;
- [ ] record submission, approval and publication as separate states.

Preliminary review remains at `STORE_POLICY_REVIEW_20260818.md`; it is not store approval.

## 14. Freeze exact production source/tag

Only after production findings are resolved:

- [ ] select exact approved production commit;
- [ ] repeat exact-source automated verification after any verification-relevant change;
- [ ] verify final version/build/release notes;
- [ ] verify final package hashes/provenance/evidence JSON;
- [ ] verify no unresolved production blocker remains;
- [ ] create immutable approved `v*` tag;
- [ ] run required tagged release gates.

Do not move a failed/rejected tag to another commit.

## 15. Final production approval/publication

Use `templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`.

- [ ] applicable package compatibility evidence passes;
- [ ] applicable device/platform evidence passes;
- [ ] applicable accessibility evidence passes;
- [ ] applicable signing/notarization evidence passes;
- [ ] applicable final-package evidence passes;
- [ ] applicable store/policy blockers are resolved;
- [ ] exact approved source/tag/package hashes are recorded;
- [ ] approval decision is recorded;
- [ ] GitHub release published where intended;
- [ ] store submission/approval/publication evidence recorded;
- [ ] final status/changelog/next-steps updated.

---

## Continuation rule

CareNest remains `1.0.0-rc.1` until applicable production rows have real evidence.

If a real defect is reproduced, fix the smallest correct boundary, add regression coverage, freeze a replacement source and run the required exact-source matrix again. Do not create speculative health functionality or fabricate production evidence merely to increase commit count.
