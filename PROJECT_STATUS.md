# Project Status

## Release target

`1.0.0-rc.1` source-complete release candidate.

CareNest remains a local-first organizational application. It does not diagnose conditions, determine or infer dosage, recommend treatment, perform clinical interaction checking, create clinical risk scores, replace qualified professionals, or provide emergency services.

## Current automated source baseline

The earlier statement that PR #43 was the final fully green 2026-08-14 source baseline was incorrect. Actual PR #43 CareNest CI #448 / run `31764449533` failed in integration testing and the UI-contract/policy suite was skipped. PR #43 is historical failure evidence, not release evidence.

The completed authoritative automated baseline is marker-only PR #54:

`Verify final CareNest bug-audit source`

PR:

`https://github.com/sanskarIN/CareNest/pull/54`

Source/base SHA frozen for runtime/test/dependency verification:

`4490f3f86752841d436e981b29279970c90c947b`

Verification marker head:

`929168a0a319b15d9e89997d86436d59ae731ad1`

Marker path:

`build/verification/bug-audit-final-20260814-2.txt`

Later base movement visible in the PR merge ref was documentation-only and did not alter the verified runtime/test/dependency graph.

Final PR #54 evidence:

- CareNest CI #503 / run `31766059137`: **success**;
- platform-neutral formatting: **success**;
- unit tests: **122 passed, 0 failed, 0 skipped**;
- integration tests: **39 passed, 0 failed, 0 skipped**;
- UI-contract/policy tests: **100 passed, 0 failed, 0 skipped**;
- total automated tests: **261 passed, 0 failed, 0 skipped**;
- Android Release build: **success**;
- Windows Release build: **success**;
- iOS simulator Release build: **success**;
- Mac Catalyst Release build: **success**;
- CodeQL #503 / run `31766059215`: **success**;
- unsuppressed Dependency Audit #35 / run `31766059132`: **success**, including platform-neutral and Android MAUI app dependency graphs.

PR #54 was closed without merge after all required automated gates succeeded. Its verification marker is not part of `main`.

`docs/releases/FINAL_BUG_AUDIT_VERIFICATION_20260814.md` is the concise authoritative evidence record.

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

The final source passed unsuppressed Dependency Audit #35 / run `31766059132`, all 261 automated tests, and all four target Release builds. This closes the previously tracked vulnerable resolved-graph path for this source. It does not claim external advisory metadata itself was changed.

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

- duplicate marker-only verification for the same final source boundary;
- core, Android, Windows, CodeQL and unsuppressed Dependency Audit succeeded while Apple was still running;
- closed unmerged after PR #54 became the authoritative completed matrix.

### PR #54

- **authoritative final automated bug-audit baseline**;
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

## Current documentation entry points

- `what_changed.md` — complete active handoff.
- `docs/releases/FINAL_BUG_AUDIT_VERIFICATION_20260814.md` — authoritative final PR #54 evidence.
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

- manual Android device/emulator matrix;
- manual Windows matrix;
- manual iOS/iPadOS matrix;
- manual Mac Catalyst matrix;
- notification permission denied/granted and real-delivery checks;
- Android exact/inexact alarm, battery optimization, reboot, clock and time-zone checks;
- packaged-target existing-database upgrade/SQLite compatibility checks with fictional data;
- packaged-target document import/export and profile-photo checks;
- packaged-target backup create/inspect/restore/wrong-password/tamper checks;
- legacy encrypted-format fixture verification where canonical historical fixtures are available;
- screen-reader verification;
- large-text/text-scaling verification;
- desktop keyboard/focus verification;
- contrast/theme/reduced-motion verification;
- current Apple App Store policy review for the optional external project-support link;
- current Google Play policy review for the optional external project-support link;
- signing identities and credentials outside Git;
- signed package generation and inspection;
- store screenshots/listing/privacy/data-safety metadata;
- exact promoted-commit Release Evidence workflow;
- final version/build metadata, release notes, checksums, production tag and GitHub release.

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

The repository assembly environment does not provide local MAUI device simulators, signing credentials or store submission sessions. GitHub-hosted Actions is the authoritative automated compilation/test surface for the verified source.

Manual device/accessibility/store/signing/release activities remain separate and are not claimed complete until actually performed.