# CareNest Phase 8 Verification Evidence

## Scope

This document records exact automated evidence for the CareNest reminder ownership, UTC, snooze, schedule-validation, deterministic recurrence-property, and multi-zone daylight-saving hardening completed on 2026-08-10.

This is automated source evidence only. It does not complete manual device/accessibility testing, current store-policy review, signing/package creation, final Release Evidence for a promoted production commit, or the open SQLitePCLRaw dependency-risk decision.

## Superseded verification — PR #29

Source head:

`04057299fe6d13012734ba235e6fa92604753948`

Verification marker head:

`16e303a1fe285faee35743bb8207c4aa8c63d335`

Marker-only file:

`build/verification/rc1-ownership-utc-dst-hardening-20260810.txt`

Pull request:

`https://github.com/sanskarIN/CareNest/pull/29`

The PR diff contained only the verification marker beyond the source head.

CareNest CI #246 / run `31382027314` correctly exposed analyzer error CA2263 in the newly added schedule-kind validation because it used `Enum.IsDefined(Type, object)` instead of the generic overload.

The analyzer was not disabled or suppressed. PR #29 was closed without merge and is not considered green evidence.

## Corrective source commit

Commit:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b` — `fix: use generic schedule enum validation`

The corrected code uses:

`Enum.IsDefined(schedule.Kind)`

The behavior remains fail-closed for unrecognized schedule enum values while satisfying the repository analyzer policy.

## Final exact-head verification — PR #30

Exact verified source head:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

Verification branch:

`ci/carenest-rc1-ownership-utc-dst-hardening-20260810-2`

Verification marker head:

`59016b7e2b13d5ac1c93cf0db973f275c6e7eb19`

Marker-only file:

`build/verification/rc1-ownership-utc-dst-hardening-20260810-2.txt`

Pull request:

`https://github.com/sanskarIN/CareNest/pull/30`

The PR changed exactly one verification marker beyond `main` and was closed without merge after all required automated workflows completed successfully.

## CareNest CI #248

Run ID:

`31382194805`

Conclusion:

**success**

Core tests job:

`93434630410`

Evidence:

- platform-neutral formatting: success;
- CareNest.UnitTests: 74 passed, 0 failed, 0 skipped;
- CareNest.IntegrationTests: 13 passed, 0 failed, 0 skipped;
- CareNest.UiTests: 54 passed, 0 failed, 0 skipped;
- total core test cases: 141 passed, 0 failed, 0 skipped.

Platform jobs:

- Android Release job `93434630440`: success;
- Windows Release job `93434630484`: success;
- Apple job `93434630334`: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success.

## CodeQL #248

Run ID:

`31382194687`

Conclusion:

**success**

## Dependency Audit #10

Run ID:

`31382194683`

Conclusion:

**success**

The green audit does not mean `GHSA-2m69-gcr7-jv3q` is fixed. The repository continues to track the SQLitePCLRaw native `2.1.11` advisory as open, with only the exact advisory URL narrowly suppressed to keep the rest of the build/test pipeline visible.

## Verified hardening scope

The exact source head includes automated/runtime protection for:

- profile → medicine → schedule → persisted schedule-time ownership validation;
- unbound editor schedule-time support before persistence;
- archived-profile automatic-reminder suppression;
- unrecognized ScheduleKind rejection;
- unsupported selected-weekday mask-bit rejection;
- trimmed/validated time-zone identifiers;
- planner `fromUtc`/`toUtc` DateTimeKind.Utc enforcement;
- half-open planner windows;
- reminder rebuild UTC override enforcement;
- explicit future-UTC snooze validation before persistence/platform scheduling;
- stable occurrence identity and duplicate-time deduplication;
- chronological planner output;
- deterministic fixed-seed randomized recurrence-boundary tests;
- cycle on/off matrix tests;
- all supported selected-weekday mask tests;
- representative every-N-hours spacing tests;
- representative DST gap/overlap tests for North America, Europe and Australia when host time-zone data is available;
- no invented alternate reminder time for invalid spring-forward local times;
- deterministic ambiguous fall-back occurrence identity.

These behaviors validate organizational schedule data only and do not introduce dosage calculation, treatment recommendations, diagnosis, medication-interaction checking, clinical risk scoring, or other medical decision support.

## Post-verification source boundary

A GitHub compare was run from exact verified source head:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

to documentation head:

`4571cf7e7149b09102690459c437b3ca844b7efa`

The compare reported:

- status: ahead;
- ahead by: 8 commits;
- behind by: 0 commits;
- total commits: 8;
- changed files were documentation only:
  - `CHANGELOG.md`;
  - `PROJECT_STATUS.md`;
  - `README.md`;
  - `docs/releases/NEXT_STEPS.md`;
  - `docs/releases/QUALITY_GATE.md`;
  - `docs/releases/RELEASE_CHECKLIST.md`;
  - `docs/releases/SECURITY_RELEASE_REVIEW.md`;
  - `what_changed.md`.

No runtime, test, project, package, workflow, or platform source file changed in those eight post-verification commits.

This evidence document itself is an additional documentation-only commit after that compare and does not change the verified runtime/test source boundary.

## Production blockers still open

Automated green evidence is necessary but not sufficient for final public `1.0.0`. The following remain intentionally open:

- supported-platform manual device/emulator matrix;
- screen-reader, large-text, keyboard, contrast, reduced-motion, and theme checks;
- real notification permission/delivery/reboot/time-zone/exact-alarm/battery behavior;
- real platform snooze behavior;
- clean-install encrypted backup/restore testing;
- current Apple App Store and Google Play review for the voluntary external support link;
- signing identities and credentials outside Git;
- signed package build/inspection;
- store screenshots/listing/privacy/data-safety/package-identity work;
- explicit resolution/decision for the open SQLitePCLRaw advisory;
- final `CareNest Release Evidence` workflow for the exact commit promoted to production;
- final version/tag/GitHub release only after all applicable gates are complete.
