# Project Status

## Release target

`1.0.0-rc.1` source-complete release candidate.

CareNest remains a local-first organizational application. It does not diagnose conditions, determine or infer dosage, recommend treatment, perform clinical interaction checking, create clinical risk scores, replace qualified professionals, or provide emergency services.

## Current `main` source after 2026-08-15 release-readiness continuation

The current `main` source is newer than the last completed marker-only exact-head verification baseline, PR #56.

The 2026-08-15 continuation added source/test/release-script changes for store-specific packaging and package metadata/privacy contracts:

- build-configurable voluntary project-support visibility through `CareNestShowFundingLink`;
- default open-source builds keep the support surface enabled;
- store packages can set `CareNestShowFundingLink=false` without a source fork;
- package metadata/privacy contracts now protect application identity, target/minimum OS declarations, Android local-first permission boundaries, Apple purpose strings/transport posture, Windows package metadata, and required branding assets;
- release-preflight scripts accept fail-closed `CARENEST_SHOW_FUNDING_LINK=true|false` and propagate it to optional MAUI restore/build;
- `docs/releases/STORE_BUILD_POLICY.md` defines per-store support-link packaging/evidence policy;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md` defines repeatable packaged/device/encrypted-data/accessibility/signing evidence procedures.

Continuation commits through the first handoff update:

- `35690d2f1fbe8bb56d91e718dab688fe4de6cc0d` — `feat: make voluntary funding link store-configurable`;
- `7ccea4ff5367b3c4e94b156f989799d91d6f52ff` — `test: enforce package metadata and privacy contracts`;
- `1fe68a73aaa41622391d8ff6e53171ca98dce055` — `build: pass store funding policy into release preflight`;
- `0a9d994ea310f00d715684c993ee2d954dc0f081` — `docs: define store-specific funding-link build policy`;
- `fe17e1ad752250d81d502ef7615fc1e652842e47` — `docs: add packaged release validation runbook`;
- `db8536d9de125ae73f895ca1d1d6cbdb4de0ded0` — `docs: record packaged release hardening handoff`.

Because application project configuration, presentation source, tests and release-preflight scripts changed after PR #56, PR #56 must not be described as exact-head verification of the current source. A new exact-head verification is required after this continuation stabilizes and before production promotion.

This source-side mitigation does **not** mark current Apple/Google policy review, packaged target inspection, device/accessibility testing, signing, or production-tag evidence complete.

## Last completed exact automated source baseline

The last completed marker-only exact automated baseline is PR #56:

`Verify complete CareNest release-engineering source`

PR:

`https://github.com/sanskarIN/CareNest/pull/56`

Frozen source/base SHA:

`4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`

Verification marker head:

`e3bc621cea05364a69abee0dadbd71a67c17bddb`

Marker path:

`build/verification/release-engineering-final-v2-20260814.txt`

Final PR #56 evidence:

- CareNest CI #571 / run `31770929379`: **success**;
- platform-neutral formatting: **success**;
- unit tests: **122 passed, 0 failed, 0 skipped**;
- integration tests: **39 passed, 0 failed, 0 skipped**;
- UI-contract/policy tests: **124 passed, 0 failed, 0 skipped**;
- total automated core tests: **285 passed, 0 failed, 0 skipped**;
- Android Release build: **success**;
- Windows Release build: **success**;
- iOS simulator Release build: **success**;
- Mac Catalyst Release build: **success**;
- CodeQL #571 / run `31770929382`: **success**;
- unsuppressed Dependency Audit #41 / run `31770929383`: **success**.

PR #56 was closed without merge after all required automated gates succeeded. Its verification marker is not part of `main`.

`docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md` is the authoritative release-engineering evidence record for that frozen source boundary. PR #54 remains the authoritative historical runtime bug-audit baseline for the earlier runtime/test/dependency source, while PR #56 verifies that runtime graph together with the later workflow/test/build-script/release-policy hardening that existed at its frozen source SHA.

## Release-engineering hardening verified at PR #56

The PR #56 source adds or hardens the following release controls without changing CareNest's medical/local-first product boundary:

- exact `v*` tag execution for CareNest CI, CodeQL, Dependency Audit, Release Gate, and Release Evidence;
- manual execution paths for CI/security/dependency workflows where configured;
- PR-only dependency-diff metadata guarded from tag/manual runs;
- failure-preserving Release Evidence with tracked-file manifests/checksums, source/ref/run identity, all three core TRX suites, dependency inventories, workspace-integrity evidence, and aggregate success after artifact upload;
- rerun-safe Release Evidence artifact identity using commit SHA + Actions run ID + run attempt;
- blocking unsuppressed NuGet audit in Bash/PowerShell release-preflight and quality-gate scripts;
- optional MAUI target audit before target Release build in release preflight;
- fail-closed PowerShell native-command handling;
- repository-local Git setup rooted at the CareNest checkout, using and verifying `Sanskar` / `sanskarin@outlook.in`;
- fail-closed production Release Gate matching for open dependency risk and nested unchecked checklist rows;
- executable regression contracts for tag/manual workflow entry points, Release Evidence behavior, preflight/quality scripts, Git setup, Release Gate, and SQLite dependency policy;
- architecture/security/setup/release documentation aligned with the verified reminder-reconciliation and SQLite-remediation behavior.

## 2026-08-14 correctness and failure-safety audit completed in source

The current source includes the following additional hardening beyond the previous PR #36 baseline.

### App lock

- transactional-style secure-store snapshot/rollback for PIN replacement;
- rollback for partial app-lock disable failures;
- clearing of application-owned salt/verifier/derived buffers;
- exact salt/verifier length checks;
- fail-closed verification for invalid PIN shape or corrupt secure material;
- no plaintext PIN persistence.

### Document vault and exports

- missing/corrupt document master key fails closed when encrypted payloads already exist;
- read/export paths never create a replacement key;
- incomplete plaintext exports are removed after failed decrypt/export/audit operations;
- successful decrypted document exports use the managed `Exports` cache directory;
- Settings Clear Cache covers those successful temporary exports;
- application-owned shared report cache files are removed after successful share handoff where CareNest still owns the temporary file.

### Profiles and photos

- profile deletion attempts every associated encrypted payload cleanup after the database cascade;
- cleanup/audit failures are aggregated instead of stopping at the first orphan;
- staged/persisted/obsolete photo references are separated;
- profile preview files use partial-file staging plus atomic move;
- failed staged replacement compensates the newly imported payload;
- profile-photo staging uses an app-lifetime/static synchronization gate;
- page disappearance performs best-effort staged cleanup without crashing navigation.

### Onboarding

- optional PIN is validated before profile creation;
- completion state is written last;
- failed setup compensates app-lock/profile/completion state with non-cancelled cleanup;
- incomplete rollback is surfaced as aggregate failure.

### SQLite migrations and repository writes

- migration DDL + schema-version update is transactional;
- primary-profile write, cascade deletes, schedule/time replacement, occurrence batches, document/tag operations, emergency-contact cleanup and full structured-record clear use transaction boundaries where multi-step consistency matters;
- transaction helper follows analyzer-required `CancellationToken` ordering;
- critical full-data clear no longer depends on `VACUUM` succeeding after the delete transaction commits.

### ViewModel refresh/input integrity

- mutation paths use non-reentrant core refresh methods instead of nesting busy-guarded `LoadAsync` calls;
- fresh profile/medicine selections are rebound by ID;
- unsupported reminder action enum values are rejected before mutation;
- undefined manual medication-log statuses are rejected before repository access.

### Android lifecycle

- boot/time/time-zone recovery uses `BroadcastReceiver.GoAsync()` and guaranteed `Finish()`;
- asynchronous receiver failures are contained so later foreground/startup recovery can retry.

### Windows lifecycle

- fallback reminder timers are not linked to short-lived caller cancellation tokens;
- cancellation and disposal ownership no longer race;
- an old timer cannot remove a newer replacement with the same occurrence ID;
- background notification display failures are contained.

### Backup and restore

- successful backup creation is not reported as failed solely because post-success local metadata recording fails;
- successful restore is not reported as failed solely because post-success audit recording fails;
- post-success bookkeeping is non-cancelled and best effort;
- bookkeeping logs only safe operation text + exception type;
- failed restore rolls the document key back to the exact prior byte state when prior bytes existed.

### Reports

- CSV string cells that look like spreadsheet formulas are neutralized before CSV escaping;
- CSV, PDF and profile JSON final paths use partial-file staging plus atomic move;
- incomplete plaintext reports are cleaned after failed writes/cancellation;
- report profile selection is rebound against freshly loaded profile records;
- shared report cache files are removed after external share returns when CareNest still owns the temporary cache copy.

### Reminder planning

- invalid daylight-saving interval anchors no longer shift forward to invented clock times;
- cycle arithmetic uses widened integer math;
- maximum date boundaries do not overflow interval scheduling.

### Startup recovery

- overdue reconciliation, medicine reminder rebuild, appointment reminder rebuild and backup reminder sync run through independent recovery boundaries;
- caller cancellation still propagates;
- one non-cancellation recovery failure does not stop later recovery steps.

### Reminder platform reconciliation

- snoozed reminders use `SnoozedUntilUtc` as their effective due time;
- future snoozes remain upcoming even after the original due time passes;
- overdue snoozes can transition to missed based on their actual snooze due time;
- rebuild cancels existing platform requests before replacement/suppression/invalidation;
- stale schedule alarms are reconciled instead of only deleting SQLite rows;
- quiet-hours rebuild can cancel previously scheduled platform requests;
- cancellation failures remain retryable;
- schedule save retains prior future occurrence identities until OS-request reconciliation can cancel stale platform requests;
- medicine/profile save flows reconcile reminders before later non-critical audit bookkeeping;
- medicine/profile delete flows cancel future platform requests before cascade deletion and attempt non-cancelled rebuild compensation if the cascade fails;
- appointment save/delete persistence has platform-reminder reconciliation/compensation coverage;
- direct integration tests cover future snooze, overdue snooze and stale future-occurrence reconciliation behavior.

### Reminder action cancellation/recovery

Handled reminder actions now use cancellation-first ordering:

- cancel the old platform request before persisting Taken/Skipped/Delayed/Missed/Snoozed/Cancelled state;
- persist action state only after cancellation succeeds;
- for snooze, schedule the replacement only after state persistence;
- if persistence/replacement scheduling fails, restore the prior occurrence state with non-cancelled compensation;
- attempt a non-cancelled rebuild so the cancelled platform request can be restored for still-actionable data;
- aggregate recovery failure instead of falsely claiming consistency;
- keep post-success audit bookkeeping from undoing an already completed user action;
- contain/privacy-safely log user-configured stock bookkeeping failure after a completed Taken action.

Related commits:

- `1459d24314de4a2f2f4fa232deb4285bb8e33b23` — action ordering/recovery source;
- `508adeb805d604274be8b069668429b6935f3fa6` — notification failure-injection support;
- `da2aed19ee9224b8d8661f11520ab9396e2c005e` — cancellation/recovery ordering tests.

## SQLite dependency remediation

The previously tracked `GHSA-2m69-gcr7-jv3q` repository dependency exception is resolved in the verified source graph.

The earlier narrow audit suppression was explicitly temporary and was **not** represented as a vulnerability fix. The current source uses a compatible maintained native/provider path and removes the exception.

Current dependency strategy:

- `sqlite-net-pcl` remains `1.9.172`;
- `SQLitePCLRaw.bundle_green` remains `2.1.11`;
- central transitive pinning selects maintained native/provider leaves;
- `SQLitePCLRaw.lib.e_sqlite3` is pinned to `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` is pinned to `2.1.12`;
- `SQLitePCLRaw.provider.e_sqlite3`, `SQLitePCLRaw.provider.sqlite3`, and `SQLitePCLRaw.provider.dynamic_cdecl` are pinned to `2.1.12`;
- the exact `NuGetAuditSuppress` entry for the advisory has been removed;
- `SqliteDependencySecurityContractTests` guards the maintained package floor and absence of the old suppression.

Relevant `main` commits:

- `66cd701f84afd5021a28e7e3327b7da4fad249aa` — `fix: pin patched SQLite native dependency path`;
- `e939d5bd912d09ffa150c804519c15e2506b7bd7` — `security: remove resolved SQLite audit suppression`;
- `04868965c43d8a6d09b40075d92f20da9b26e32a` — `test: guard patched SQLite dependency baseline`.

PR #56 passed unsuppressed Dependency Audit #41 / run `31770929383`, all 285 automated tests, and all four target Release builds. This keeps the formerly vulnerable resolved-graph path closed for the verified source.

This does **not** mark manual packaged existing-database/backup/encrypted-document compatibility complete. Those remain production-release validation gates.

Authoritative dependency documents:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

## Verification checkpoint history

The audit used failure-driven marker-only checkpoints.

### PR #37

- formatting succeeded;
- unit suite reached 111 passing tests at that checkpoint;
- CA1068 exposed invalid `CancellationToken` parameter ordering in the new transaction helper;
- fixed directly; no analyzer suppression;
- PR closed unmerged.

### PR #39

- exposed CA1001 on the profile-photo `SemaphoreSlim` ownership;
- exposed a missing final newline in `ReminderPlanner.cs`;
- its marker was accidentally merged before the failed evidence was fully acted on;
- marker was explicitly removed from `main` by `549c77120c2ff792337cb842bf7a0912483816ed`;
- PR #39 is not release evidence.

### PR #40

- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL: success;
- Dependency Audit: success;
- core formatting failed only because `EncryptedBackupService.cs` lacked its final newline;
- source was corrected and PR closed unmerged rather than partially promoted.

### PR #41

- reminder-reconciliation checkpoint intentionally superseded by additional delete-flow work;
- closed unmerged.

### PR #42

- bug-audit checkpoint intentionally superseded while the behavior audit was still changing source;
- closed unmerged.

### PR #43

- **not green**;
- CareNest CI #448 / `31764449533` failed during integration testing;
- UI-contract suite was skipped after the failure;
- platform Release builds, CodeQL and Dependency Audit passed;
- closed unmerged and retained only as historical failure evidence.

### PR #44

- reproduced future-snooze, overdue-snooze and stale-future-occurrence defects;
- source fixed in `4cf2aec989233d213ac7b1099a50d44e1acc3ca0`;
- closed unmerged.

### PR #46

- exposed broader platform-reminder reconciliation contract failures;
- drove explicit cancellation, row-preservation and delete-compensation changes;
- closed unmerged.

### PR #47

- unsuppressed SQLite Dependency Audit #28 / `31765223239` succeeded;
- source advanced while verification ran;
- closed unmerged and retained as dependency-remediation evidence only.

### PR #48

- unsuppressed Dependency Audit #29 / `31765388861`: success;
- CodeQL #469 / `31765388858`: success;
- combined CI exposed a moving-base reminder-interface compile mismatch;
- source corrected/simplified and PR closed unmerged.

### PR #49

- exposed CA1861 in new medicine/profile reconciliation assertions;
- test source corrected rather than suppressing analyzer policy;
- closed unmerged.

### PR #50

- unsuppressed Dependency Audit #31 / `31765668949`: success;
- base predated later analyzer-safe reminder tests;
- closed unmerged.

### PR #51

- superseded by later appointment/reminder-action and SQLite-remediation source;
- closed unmerged.

### PR #52

- included SQLite remediation but was superseded by later cancellation-first reminder actions/failure-injection coverage;
- closed unmerged.

### PR #53

- duplicate marker-only verification for the same final bug-audit source boundary;
- completed a fully green duplicate final-source matrix;
- closed unmerged; PR #54 retained as the authoritative bug-audit baseline.

### PR #54

- **authoritative completed runtime bug-audit baseline**;
- CareNest CI #503 / `31766059137`: success;
- 122 unit + 39 integration + 100 UI-contract = 261/261 tests passed;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #503 / `31766059215`: success;
- unsuppressed Dependency Audit #35 / `31766059132`: success;
- closed unmerged after evidence capture;
- marker is not part of `main`.

### PR #55

- first release-engineering hardening checkpoint;
- formatting: success;
- 122 unit + 39 integration + 116 UI-contract = 277/277 tests passed;
- Android Release: success;
- Windows Release: success;
- CodeQL #547 / `31769940053`: success;
- unsuppressed Dependency Audit #38 / `31769940039`: success;
- closed unmerged as superseded while Apple was still running because the complete-file audit found additional legitimate release-tooling/documentation fixes;
- not the final baseline.

### PR #56

- **authoritative completed release-engineering baseline for its frozen source boundary**;
- frozen source/base `4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`;
- marker head `e3bc621cea05364a69abee0dadbd71a67c17bddb`;
- CareNest CI #571 / `31770929379`: success;
- 122 unit + 39 integration + 124 UI-contract = 285/285 tests passed;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #571 / `31770929382`: success;
- unsuppressed Dependency Audit #41 / `31770929383`: success;
- closed unmerged after evidence capture;
- marker is not part of `main`;
- superseded as exact-current-head evidence by later 2026-08-15 source/test/release-script changes, while remaining valid evidence for the frozen PR #56 source.

## Current documentation entry points

- `what_changed.md` — complete active handoff, including the 2026-08-15 continuation.
- `docs/releases/STORE_BUILD_POLICY.md` — build-configurable voluntary project-support/store policy.
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md` — packaged/manual evidence runbook.
- `docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md` — authoritative PR #56 release-engineering evidence for its frozen source.
- `docs/releases/FINAL_BUG_AUDIT_VERIFICATION_20260814.md` — historical authoritative PR #54 runtime bug-audit evidence.
- `docs/releases/BUG_AUDIT_VERIFICATION_20260814.md` — complete 2026-08-14 bug-audit evidence and correction history.
- `docs/testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md` — defect-to-test map.
- `docs/security/BUG_AUDIT_SECURITY_NOTES_20260814.md` — security/privacy bug-audit boundaries.
- `docs/releases/POST_BUG_AUDIT_COMPARE_20260814.md` — corrected source-boundary history.
- `docs/releases/PHASE9_VERIFICATION_EVIDENCE.md` — previous PR #36 Settings lifecycle evidence.
- `docs/releases/SETTINGS_LIFECYCLE_VERIFICATION_20260813.md` — previous Settings lifecycle verification details.
- `docs/testing/SETTINGS_LIFECYCLE_CONTRACT.md` — Settings lifecycle regression contract.
- `docs/security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md` — full local-data clear security model.
- `docs/security/DEPENDENCY_RISK_REGISTER.md` — dependency risk/remediation source of truth.
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` — SQLite migration/remediation and manual compatibility plan.
- `docs/releases/RELEASE_CHECKLIST.md` — release gates.
- `docs/releases/NEXT_STEPS.md` — operational remaining work.

Historical handoffs remain under `docs/history/` and in Git history. Earlier automated baselines are retained as history instead of being overwritten or silently promoted after later source changes.

## Production blockers that remain real

Source hardening and automated verification do not make external/manual work magically complete.

Still required:

- new exact-head automated verification after the 2026-08-15 verification-relevant source changes stabilize;
- manual Android device/emulator matrix;
- manual Windows matrix;
- manual iOS/iPadOS matrix;
- manual Mac Catalyst matrix;
- notification permission denied/granted and real-delivery checks;
- cancellation-first reminder-action behavior against real platform notification scheduling and restart/recovery;
- Android exact/inexact alarm, battery optimization, reboot, clock and time-zone checks;
- packaged-target existing-database upgrade/SQLite compatibility checks with fictional data;
- packaged-target document import/export and profile-photo checks;
- packaged-target backup create/inspect/restore/wrong-password/tamper checks;
- existing encrypted-document compatibility after the SQLite native/provider remediation;
- legacy encrypted-format fixture verification where canonical historical fixtures are available;
- screen-reader verification;
- large-text/text-scaling verification;
- desktop keyboard/focus verification;
- contrast/theme/reduced-motion verification;
- current Apple App Store policy review for the optional external project-support link;
- current Google Play policy review for the optional external project-support link;
- packaged verification of the selected `CareNestShowFundingLink` value per distribution channel;
- signing identities and credentials outside Git;
- signed package generation and inspection;
- store screenshots/listing/privacy/data-safety metadata;
- exact approved production `v*` tag with successful CareNest CI, CodeQL, Dependency Audit, Release Gate, and Release Evidence runs;
- final version/build metadata, release notes, checksums, production tag and GitHub/store release.

None of these manual/external gates is marked complete merely because automated CI/security analysis is green.

## Deferred scope

Still outside current v1:

- cloud synchronization;
- remote caregiver collaboration;
- required accounts/mobile-number authentication;
- server-side health-record storage;
- silent remote sharing;
- hidden analytics/telemetry;
- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- medication-interaction claims;
- clinical risk scoring.

Any future networked feature requires a new consent/authentication/key/privacy/threat/export/store review.

## Environment truth

The repository assembly environment does not provide local MAUI device simulators, signing credentials or store submission sessions. GitHub-hosted Actions is the authoritative automated compilation/test surface for source changes, while completed marker-only exact-head verification remains the release evidence protocol.

Manual device/accessibility/store/signing/packaged-data/release activities remain separate and are not claimed complete until actually performed.
