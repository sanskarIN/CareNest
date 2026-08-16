# CareNest Next Steps

This document tracks only the work that remains after the 2026-08-16 compiled-binding hardening continuation and full PR #74 verification.

The exact active version immediately before this continuation is preserved at:

`docs/history/pre-xaml-compiled-bindings-20260816/NEXT_STEPS.md`

Earlier final-bug-audit history remains at:

`docs/history/pre-final-bug-audit-20260815/NEXT_STEPS.md`

Historical PR #54/#56/#58/#59/#61/#67/#68 evidence remains available in dated release records and Git history.

CareNest remains `1.0.0-rc.1` until the external/manual production gates below are completed.

---

## Current exact automated baseline

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Verified PR #74 source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

PR:

`https://github.com/sanskarIN/CareNest/pull/74`

Permanent evidence:

`docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`

Automated results on the frozen PR head:

- CareNest CI #735 / run `31938301209`: success;
- formatting: success;
- unit tests: **122/122**;
- integration tests: **39/39**;
- UI/source-policy tests: **170/170**;
- total: **331/331**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- Store Package Configuration #124 / run `31938301146`: all four target configurations success;
- Store Inspection Artifacts #47 / run `31938301275`: scanner self-test plus Android/Windows/Apple inspection artifact workflows success;
- CodeQL #735 / run `31938301252`: success;
- unsuppressed Dependency Audit #91 / run `31938301172`: success on both dependency graphs;
- strict XAML binding compilation: success on all configured target builds;
- `XC0022`, `XC0023`, `XC0024`, `XC0025`: warnings-as-errors, no suppression.

---

## Completed source-side work

The following items are complete in source and do not need another implementation pass unless a new real defect is found:

- [x] local-first/account-free RC1 feature implementation;
- [x] profile/medicine/schedule/medication-log/appointment/document/report/settings/app-lock source scope;
- [x] reminder reconciliation and cancellation-first action recovery hardening;
- [x] Android/Windows/iOS/Mac Catalyst source builds;
- [x] encrypted document and encrypted manual backup framing/tamper/truncation protections;
- [x] SQLite multi-step transactional consistency hardening;
- [x] SQLite dependency advisory remediation and removal of the old audit suppression;
- [x] package identity/privacy/platform metadata regression contracts;
- [x] Release Gate / Release Evidence / exact-tag workflow coverage;
- [x] unsuppressed dependency audit in CI/release paths;
- [x] exact-source internal Android/Windows/Apple inspection artifacts with checksums/provenance;
- [x] fail-closed forbidden-marker payload scanner and scanner self-test;
- [x] discovery and removal of the Windows packaged external-funding marker defect;
- [x] removal of the external project-funding destination/card/command/artwork from the application runtime/package;
- [x] removal of obsolete per-package funding build switches;
- [x] repository-only voluntary funding documentation with no health/medical entitlement;
- [x] recursive source-policy regression guard preventing the funding destination from re-entering `src/CareNest.App`;
- [x] final 2026-08-15 merged-source package verification;
- [x] automated bug/error repository sweep with no known automated functional defect under that matrix;
- [x] accurate root `x:DataType` on every binding-bearing page;
- [x] accurate item `x:DataType` on every binding-bearing DataTemplate;
- [x] typed picker `ItemDisplayBinding` contexts;
- [x] typed explicit Source/RelativeSource ViewModel command bindings;
- [x] `MauiEnableXamlCBindingWithSourceCompilation=true`;
- [x] `MauiStrictXamlCompilation=true`;
- [x] `XC0022` / `XC0023` / `XC0024` / `XC0025` promoted to errors;
- [x] six dynamic compiled-binding source-policy tests;
- [x] PR #74 full CI/store-package/store-inspection/CodeQL/dependency-audit verification;
- [x] PR #74 merged to `main` while preserving all 17 focused commits.

The previous Priority 1 item to incrementally reduce `XC0022` / `XC0025` warnings is now **closed**, not deferred.

The application package remains funding-surface-free by source policy for every target.

---

# Priority 0 — required before a public production release

These are the actual remaining blockers. Do not replace them with broad source refactoring unless validation exposes a real defect.

## 1. Packaged existing-data and SQLite compatibility

Use fictional/synthetic data only.

- [ ] Create/install a representative earlier RC candidate data set containing profiles, medicines, schedules, reminder occurrences, medication logs, appointments, stock adjustments, documents/tags and settings.
- [ ] Upgrade/install the intended production candidate through the target platform's realistic package/update path.
- [ ] Confirm the SQLite database opens.
- [ ] Run and record integrity validation.
- [ ] Confirm all representative records remain readable.
- [ ] Confirm all representative editable records remain editable.
- [ ] Confirm schema version is correct.
- [ ] Confirm reminder rebuild/reconciliation succeeds after upgrade.
- [ ] Confirm no duplicate/stale platform reminder is silently stranded.
- [ ] Record package/source/checksum/result evidence.

Source dependency remediation is already complete. Do not restore the former audit suppression because this manual compatibility evidence is pending.

Runbook:

`docs/releases/PACKAGED_RELEASE_VALIDATION.md`

---

## 2. Encrypted document and backup compatibility

With fictional data:

- [ ] Verify a current packaged encrypted document import/open/export/delete lifecycle.
- [ ] Verify failed document export does not leave an unintended CareNest-owned partial plaintext file.
- [ ] Verify missing/corrupt document-key behavior fails closed.
- [ ] Verify current packaged encrypted backup create/inspect/restore.
- [ ] Verify wrong backup password is rejected.
- [ ] Verify tampered backup is rejected.
- [ ] Verify truncated backup is rejected.
- [ ] Verify trailing-data backup is rejected.
- [ ] Verify restored encrypted documents remain usable.
- [ ] Verify clean-install restore.
- [ ] Verify canonical historical v1 document/backup bytes if genuine historical fixtures exist.
- [ ] Do not manufacture a new test artifact and label it as historical evidence.

---

## 3. Android manual matrix

On representative supported Android hardware/emulators:

- [ ] fresh install and onboarding;
- [ ] notification permission denied;
- [ ] notification permission granted;
- [ ] medicine reminder create/edit/delete;
- [ ] appointment reminder create/edit/delete;
- [ ] Taken cancellation-first behavior;
- [ ] Skipped cancellation-first behavior;
- [ ] Delayed cancellation-first behavior;
- [ ] Missed cancellation-first behavior;
- [ ] Snooze cancellation + replacement;
- [ ] future snooze crossing original due time;
- [ ] overdue snooze evaluated from snooze due time;
- [ ] schedule-edit stale-request cleanup;
- [ ] medicine delete cleanup;
- [ ] profile delete cleanup;
- [ ] restart/reopen recovery;
- [ ] reboot rebuild;
- [ ] exact/inexact alarm diagnostics;
- [ ] battery-optimization disclosure/behavior;
- [ ] clock/time-zone/DST recovery;
- [ ] force-stop/vendor limitation messaging;
- [ ] document picker/share;
- [ ] backup/restore;
- [ ] app lock.

---

## 4. Windows manual matrix

On representative Windows 11 targets:

- [ ] fresh install/package execution;
- [ ] navigation and core CRUD;
- [ ] running-app notification behavior;
- [ ] closed-app limitation messaging;
- [ ] same-ID timer replacement/cancellation;
- [ ] reminder actions and snooze;
- [ ] restart/recovery;
- [ ] document picker/share;
- [ ] backup/restore;
- [ ] app lock;
- [ ] keyboard navigation/focus;
- [ ] light theme;
- [ ] dark theme;
- [ ] system theme.

---

## 5. iPhone/iPad real-device matrix

Simulator compilation is automated evidence, not a substitute for real-device notification behavior.

- [ ] fresh install;
- [ ] notification permission denied;
- [ ] notification permission granted;
- [ ] medicine reminders;
- [ ] appointment reminders;
- [ ] reminder actions/snooze;
- [ ] restart behavior;
- [ ] time-zone/DST behavior;
- [ ] backup/restore;
- [ ] document picker/share;
- [ ] app lock;
- [ ] Dynamic Type;
- [ ] VoiceOver;
- [ ] notification preview privacy.

---

## 6. Mac Catalyst manual matrix

- [ ] fresh install/package execution;
- [ ] notification permission/delivery;
- [ ] reminder actions/snooze/reconciliation;
- [ ] restart behavior;
- [ ] file picker/share;
- [ ] backup/restore;
- [ ] app lock;
- [ ] keyboard/focus;
- [ ] theme/contrast;
- [ ] signed/notarized candidate behavior when available.

---

## 7. Accessibility validation

Use representative assistive technologies, not source inspection alone.

- [ ] screen-reader names and reading order;
- [ ] large text / 200% or representative scaling;
- [ ] destructive confirmation readability;
- [ ] desktop keyboard focus/navigation;
- [ ] contrast in light theme;
- [ ] contrast in dark theme;
- [ ] contrast in system theme;
- [ ] color-independent status meaning;
- [ ] reduced-motion behavior;
- [ ] privacy-safe actionable errors.

Compiled XAML binding warnings are no longer a pending accessibility/quality item. They are closed and enforced by build/test policy. Real accessibility behavior still requires this manual matrix.

---

## 8. Production signing outside Git

Never commit private signing material.

Required:

- [ ] Android production keystore/signing service configured outside Git;
- [ ] Apple certificates/provisioning/store signing configured outside Git;
- [ ] Windows production signing identity configured outside Git;
- [ ] safe public signing fingerprints/identifiers recorded where appropriate;
- [ ] signing timestamp recorded;
- [ ] exact source SHA recorded;
- [ ] package checksum recorded.

---

## 9. Final signed-package inspection

For every intended production package:

- [ ] record exact source SHA;
- [ ] record version/build number;
- [ ] record application/package identity;
- [ ] record package filename;
- [ ] record SHA-256;
- [ ] record signing/notarization/store-managed provenance;
- [ ] repeat/equivalently run the forbidden funding-marker payload scan on the final signed package;
- [ ] manually verify About contains no Buy Me a Coffee funding destination/card;
- [ ] verify repository link remains available;
- [ ] verify creator profile link remains available;
- [ ] verify business email remains available;
- [ ] verify application support email remains available;
- [ ] verify privacy/terms/security/notices remain available;
- [ ] verify no health feature changes based on project funding;
- [ ] verify installed package starts;
- [ ] execute platform smoke tests.

---

## 10. Store metadata and policy review

At actual submission time, review current rules rather than relying on older snapshots.

- [ ] review current Apple rules applicable to the app/listing;
- [ ] review current Google Play rules applicable to the app/listing;
- [ ] review current Microsoft/Windows distribution requirements if used;
- [ ] validate health-organizer claims/disclaimers;
- [ ] validate notification/reminder wording;
- [ ] validate privacy/data-safety declarations;
- [ ] validate screenshots with fictional data;
- [ ] validate support link;
- [ ] validate privacy link;
- [ ] validate terms link;
- [ ] validate security link;
- [ ] record review date/source/conclusion.

The app binary itself has no external project-funding destination. Repository funding metadata remains separate.

---

## 11. Select the exact production source

Only after all applicable manual/package/signing/store findings are resolved:

- [ ] freeze the exact approved production commit;
- [ ] ensure no verification-relevant source changed after the last accepted exact-head verification, or repeat exact-source verification if it did;
- [ ] verify release version/build metadata;
- [ ] verify final release notes;
- [ ] verify final package checksums/provenance.

Do not move a failed/rejected production tag to a different commit.

---

## 12. Create the production `v*` tag and require all tagged gates

For the exact approved tag require:

- [ ] tagged CareNest CI success;
- [ ] tagged CodeQL success;
- [ ] tagged unsuppressed Dependency Audit success;
- [ ] tagged CareNest Store Package Configuration success;
- [ ] tagged CareNest Store Inspection Artifacts success;
- [ ] tagged Release Gate success;
- [ ] tagged Release Evidence success;
- [ ] release-evidence artifact/checksums recorded;
- [ ] final signed-package provenance recorded.

Only then proceed to public publication.

---

# Priority 1 — post-RC quality improvements

These are not current RC1 functional blockers unless later testing proves otherwise.

- [ ] Continue accessibility polish based on actual assistive-technology findings.
- [ ] Add canonical packaged upgrade fixtures as genuine released-build artifacts become available.
- [ ] Expand non-production package inspection to signed test candidates when safe signing infrastructure exists.
- [ ] Improve release-evidence automation only if it preserves the exact-source/fail-closed model.
- [ ] Consider performance measurements for compiled versus runtime XAML bindings only if measurement has a concrete product/release purpose; do not weaken strict compilation to perform the comparison.

Removed from this section because it is complete:

- [x] add compiled XAML binding metadata and eliminate `XC0022` / `XC0025` warning debt;
- [x] compile explicit Source bindings;
- [x] enforce `XC0022`–`XC0025` against regression.

---

# Deferred future scope

Still intentionally outside RC1 unless separately designed/reviewed:

- cloud synchronization;
- remote caregiver collaboration;
- required accounts or phone-number authentication;
- server-side health-record storage;
- silent remote sharing;
- hidden analytics/telemetry;
- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- clinical interaction claims;
- clinical risk scoring.

Any future networked/remote-data feature requires a new consent, authentication, key-management, privacy, threat-model, export/deletion and store-policy review.

---

## Continuation rule

Do not perform more broad source refactoring merely to keep development active.

The current executable source is verified under the configured automated matrix and the previously pending compiled-binding cleanup is complete.

The next work is production validation.

If a real manual/package/security/accessibility defect is found:

1. reproduce it safely;
2. fix the smallest correct source boundary;
3. add regression coverage;
4. run the full exact-head automated matrix again;
5. update factual evidence only after the run completes.

Current status:

**Source-complete CareNest `1.0.0-rc.1` with strict compiled XAML binding enforcement and no known automated defect under the configured PR #74 matrix; production validation remains open.**
