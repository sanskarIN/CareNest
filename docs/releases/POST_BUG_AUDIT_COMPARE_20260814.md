# CareNest post-bug-audit source-boundary evidence — 2026-08-14

## Purpose

This file records the corrected boundary between the 2026-08-14 bug-audit verification checkpoints and the source that followed them. An earlier revision incorrectly described PR #43 as a fully green final baseline and incorrectly stated that only documentation changed afterward. GitHub Actions and later source commits prove otherwise; those earlier statements are superseded here.

## PR #43 — historical correction

Verification PR:

`#43 — Verify final CareNest 2026-08-14 bug audit source`

Verification branch:

`ci/carenest-final-bug-audit-20260814`

Verification marker:

`build/verification/final-bug-audit-20260814.txt`

PR #43 was closed without merging its marker, but it was **not** a fully green source baseline.

Actual workflow evidence:

- CareNest CI #448 / run `31764449533`: **failure**;
- platform-neutral formatting: success;
- unit tests: success;
- integration tests: failure;
- UI-contract/policy tests: skipped after the integration failure;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #448 / run `31764449600`: success;
- Dependency Audit #23 / run `31764449574`: success.

Therefore PR #43 must not be used as release evidence and must not be described as the current exact-head green source.

## PR #44 — reproduction of the reminder defects

PR #44 independently reproduced three integration failures on the PR #43-era source:

1. a snoozed occurrence with a future `SnoozedUntilUtc` disappeared from upcoming reminders when its original `ScheduledUtc` was already in the past;
2. an expired snooze remained `Snoozed` instead of becoming `Missed`;
3. a stale future occurrence remained scheduled instead of being reconciled/cancelled before replacement scheduling.

The defects were fixed on `main` by:

`4cf2aec989233d213ac7b1099a50d44e1acc3ca0` — `fix: reconcile snoozed and stale reminder occurrences`

PR #44 was closed unmerged and is failure evidence, not release evidence.

## Later reminder-reconciliation checkpoints

The audit continued after PR #44 rather than treating one patch as the end of the review.

### PR #46

PR #46 verified the corrected integration behavior far enough to expose six UI-contract/policy failures covering broader platform-reminder lifecycle rules. The findings drove additional source changes for:

- effective snooze due-time handling;
- explicit platform cancellation before replacement/invalidation;
- schedule-row preservation until reconciliation has a chance to cancel existing platform requests;
- medicine/profile future-reminder cancellation before cascade deletion;
- non-cancelled compensation if a cascade fails after platform cancellation;
- a brittle stock-method contract boundary.

PR #46 was closed without merging its marker and is not release evidence.

### PR #49

PR #49 later reached analyzer enforcement and exposed CA1861 in two new medicine-reconciliation assertions. The analyzer was not suppressed. The affected test expectations were moved to analyzer-safe static readonly data, and the same proactive correction was made for the corresponding profile assertion.

Relevant corrections include:

- `cc9465136bd7de0e55e14386c19fa849a3e56067` — medicine reconciliation assertion fix;
- `834b2980167c41bc7e9c1ad69dc54ad5ccc7e53e` — profile reconciliation assertion fix.

PR #49 was closed unmerged and is not release evidence.

## Additional source after the PR #43 boundary

Contrary to the old version of this document, runtime/test/configuration source did change after PR #43. The continued audit added or corrected, among other work:

- reminder effective-due and stale-request reconciliation;
- medicine/profile cancellation compensation;
- profile/medicine save-time reminder reconciliation;
- appointment reminder persistence compensation;
- analyzer-safe reconciliation tests;
- shared report-cache cleanup after export;
- SQLite native/provider dependency remediation;
- removal of the tracked SQLite NuGet audit suppression;
- a regression contract preventing the vulnerable SQLite dependency/audit-suppression baseline from returning.

This means no comparison that ends at PR #43 can describe the current repository runtime/test boundary.

## SQLite dependency remediation now present on `main`

The earlier tracked SQLite exception was subsequently changed in source rather than merely documented.

Relevant commits on `main`:

- `66cd701f84afd5021a28e7e3327b7da4fad249aa` — `fix: pin patched SQLite native dependency path`;
- `e939d5bd912d09ffa150c804519c15e2506b7bd7` — `security: remove resolved SQLite audit suppression`;
- `04868965c43d8a6d09b40075d92f20da9b26e32a` — `test: guard patched SQLite dependency baseline`.

The dependency graph intentionally keeps `SQLitePCLRaw.bundle_green` at the available `2.1.11` bundle version while centrally pinning the native/provider leaves to the newer maintenance path. `Directory.Build.props` no longer contains the exact `GHSA-2m69-gcr7-jv3q` `NuGetAuditSuppress` entry.

The dependency change must be judged by unsuppressed NuGet audit plus the complete CareNest automated matrix and the existing manual migration/device gates; the old narrow suppression is no longer the intended production baseline.

## Correct interpretation of verification markers

Verification marker branches exist only to trigger/check an exact source state. A marker file is not application source and must not be merged merely because its PR produced useful evidence.

For every verification checkpoint:

1. use the PR's actual merge/source snapshot;
2. inspect every required workflow group;
3. treat any failed/ skipped required group as non-green evidence;
4. fix the source rather than suppressing analyzer/test/security failures;
5. create a new exact-source verification after source changes;
6. close the marker PR without merging its marker.

This rule supersedes any earlier prose that inferred a successful baseline from only platform builds, CodeQL, Dependency Audit, or a subset of the core test pipeline.

## Remaining non-source production gates

Corrected automated evidence still does not mark manual/external work complete. The following remain separate release requirements until actually performed and recorded:

- real Android, Windows, iOS/iPadOS and Mac Catalyst smoke matrices;
- real notification permission and delivery checks;
- Android alarm/battery/reboot/clock/time-zone behavior;
- packaged document/photo/report/backup workflows;
- canonical encrypted-format compatibility fixtures where available;
- screen-reader, text-scaling, keyboard/focus, contrast/theme and reduced-motion checks;
- current Apple App Store and Google Play policy review;
- signing identities/credentials outside Git;
- signed package generation and inspection;
- store listing/screenshots/privacy/data-safety metadata;
- Release Evidence for the exact promoted production commit;
- final version/build metadata, checksums, tag and release.

`PROJECT_STATUS.md`, `docs/security/DEPENDENCY_RISK_REGISTER.md`, `docs/releases/NEXT_STEPS.md`, and `what_changed.md` are the active status/handoff surfaces and must be updated whenever a later exact-source verification supersedes the historical checkpoints described here.
