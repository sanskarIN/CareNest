# CareNest release-engineering verification — 2026-08-14

## Purpose

This file is the authoritative automated evidence record for the CareNest release-engineering source that followed the completed 2026-08-14 runtime bug audit.

The verified source keeps the previously hardened CareNest runtime behavior and adds release-process correctness controls around exact production tags, dependency auditing, release evidence, local preflight/quality gates, Git identity setup, and production release policy.

CareNest remains a local-first organizational application. Nothing in this release-engineering work adds diagnosis, dosage calculation/inference, treatment recommendations, clinical interaction checking, clinical risk scores, guaranteed reminder delivery, a required CareNest account/backend, automatic cloud synchronization, or hidden telemetry.

## Frozen source

Source/base SHA:

`4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`

Verification PR:

`#56 — Verify complete CareNest release-engineering source`

Verification branch:

`ci/carenest-release-engineering-final-v2-20260814`

Verification marker head:

`e3bc621cea05364a69abee0dadbd71a67c17bddb`

Verification marker:

`build/verification/release-engineering-final-v2-20260814.txt`

The PR changed only the verification marker beyond the frozen source. It was closed without merge after all required gates succeeded. The marker is not part of `main`.

## Final automated evidence

### CareNest CI

CareNest CI #571 / run `31770929379`: **success**.

- platform-neutral formatting: **success**;
- `CareNest.UnitTests`: **122 passed, 0 failed, 0 skipped**;
- `CareNest.IntegrationTests`: **39 passed, 0 failed, 0 skipped**;
- `CareNest.UiTests`: **124 passed, 0 failed, 0 skipped**;
- total core tests: **285 passed, 0 failed, 0 skipped**;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**.

### CodeQL

CodeQL #571 / run `31770929382`: **success**.

### Dependency Audit

Dependency Audit #41 / run `31770929383`: **success**.

The audit is unsuppressed for the formerly tracked SQLite advisory and covers the platform-neutral/test dependency graph plus the Android MAUI application graph.

## Release-engineering changes covered

The verified source includes the following controls in addition to the previously green CareNest runtime/test/dependency graph.

### Exact production-tag verification

Tags matching `v*` run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

CI, CodeQL, and Dependency Audit also support manual execution where configured.

### Dependency Audit event safety

The PR-only dependency-diff step is explicitly guarded with:

`if: github.event_name == 'pull_request'`

so manual/tag runs do not dereference pull-request-only metadata.

### Failure-preserving Release Evidence

The Release Evidence workflow records:

- commit SHA;
- Git ref;
- GitHub run ID and run attempt;
- .NET toolchain information;
- tracked-file manifest;
- SHA-256 manifest for tracked repository files;
- pre/post tracked-workspace status;
- unit/integration/UI-contract TRX evidence;
- platform-neutral source/test transitive dependency inventories;
- evidence-file checksums.

Unit, integration, UI-contract, dependency-inventory, and workspace-integrity components are attempted independently. Evidence upload runs with failure-preserving `always()` behavior before an aggregate pass/fail step, so a failed release-evidence run remains diagnosable.

Artifact names include:

- commit SHA;
- workflow run ID;
- workflow run attempt.

This keeps rerun artifacts distinguishable.

### Blocking local dependency audits

Both Bash and PowerShell release-preflight scripts now treat unsuppressed NuGet audit failure as blocking rather than warning-only.

Both local quality-gate scripts:

- work from a clean checkout;
- restore/run all three core test projects;
- perform blocking unsuppressed NuGet audit;
- fail on required native-command errors.

When `CARENEST_TARGET` is set, release preflight audits the selected MAUI target before the optional target Release build.

### Repository-local Git identity setup

Git setup scripts now:

- resolve the repository root;
- require a valid Git work tree;
- use `git config --local`;
- set `user.name` to `Sanskar`;
- set `user.email` to `sanskarin@outlook.in`;
- verify both configured values;
- fail on native Git command errors.

This is a local maintainer setup rule. GitHub web/API commits can still use the authenticated GitHub account identity and must not be falsely represented as arbitrary local-email commits.

### Hardened Release Gate

The production Release Gate now:

- detects open dependency-risk status without indentation/case bypass;
- detects nested unchecked release-checklist rows;
- requires core status/security/evidence documents;
- runs all three core test projects;
- uses explicit job timeouts.

### Executable release-policy contracts

The UI-contract/policy suite now includes automated protection for:

- release workflow exact-tag/manual triggers;
- Dependency Audit PR-only metadata guard;
- Release Evidence provenance/failure preservation/rerun identity;
- blocking release-preflight audit behavior;
- deterministic/fail-closed local quality-gate scripts;
- repository-local Git identity setup;
- fail-closed production Release Gate matching.

These additions increased the UI-contract/policy suite from 100 tests at PR #54 to 124 tests in this final PR #56 source.

## SQLite dependency status

The formerly tracked `GHSA-2m69-gcr7-jv3q` source exception remains remediated in this verified graph.

Current source intent includes:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- central transitive pinning enabled;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- Android native/provider leaves and selected providers at `2.1.12`;
- no former advisory `NuGetAuditSuppress` entry;
- `SqliteDependencySecurityContractTests` protecting the package floor and suppression absence.

Dependency security and packaged existing-data compatibility are separate properties. The green audit does not replace the manual packaged upgrade/database/encrypted-document/backup/reminder compatibility matrix.

## Reminder/platform integrity retained

The release-engineering source retains the previously verified reminder behavior:

- `SnoozedUntilUtc` is the effective due time for snoozed reminders;
- existing platform requests are cancelled before replacement, suppression, invalidation, or handled-state persistence;
- cancellation failures remain retryable;
- stale occurrence identity is retained long enough to cancel obsolete OS requests;
- medicine/profile deletion uses cancellation-before-cascade plus non-cancelled rebuild compensation on persistence failure;
- appointment persistence/platform scheduling uses reconciliation/compensation;
- Taken/Skipped/Delayed/Missed/Snoozed/Cancelled actions use cancellation-first ordering;
- later action failure attempts previous-state restoration plus non-cancelled reminder rebuild;
- reminder failure logs remain privacy-minimized.

## Historical checkpoint

PR #55 verified an earlier release-engineering snapshot and produced useful evidence:

- formatting success;
- 122 unit tests passed;
- 39 integration tests passed;
- 116 UI-contract/policy tests passed;
- 277 total core tests passed;
- Android Release success;
- Windows Release success;
- CodeQL #547 / `31769940053`: success;
- unsuppressed Dependency Audit #38 / `31769940039`: success.

PR #55 was intentionally closed unmerged before its Apple job completed because the complete-file audit found additional legitimate release-tooling/documentation corrections. It is superseded and is not the authoritative final baseline.

## Remaining production gates

PR #56 completes the automated release-engineering source verification. It does **not** complete work that requires real devices, store accounts, signing credentials, or actual packaged upgrade evidence.

Still required before final public production promotion:

- Android real-device/emulator manual matrix;
- Windows manual matrix;
- iOS/iPadOS manual matrix;
- Mac Catalyst manual matrix;
- real notification permission/delivery checks;
- cancellation-first reminder-action behavior against real platform scheduling/restart recovery;
- Android alarm/battery/reboot/time/time-zone checks;
- packaged SQLite existing-data upgrade/integrity checks using fictional data;
- existing encrypted-document compatibility checks;
- current/pre-remediation encrypted-backup compatibility checks where canonical synthetic fixtures are available;
- accessibility checks including screen readers, large text, keyboard/focus, contrast/themes, and reduced motion;
- current Apple App Store / Google Play policy review for the voluntary external support link;
- signing identities/credentials outside Git;
- signed package generation and inspection;
- final store screenshots/listing/privacy/data-safety metadata;
- final exact production commit/tag Release Gate and Release Evidence;
- final version/build metadata, release notes, checksums, tag, and GitHub/store release publication.

No manual/external row is considered complete merely because PR #56 is green.
