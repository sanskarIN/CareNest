# CareNest 2026-08-14 bug-audit verification

## Scope

This document records the repository-wide correctness, failure-safety, privacy, persistence, export, reminder, backup, platform-lifecycle, dependency-security, and regression-test audit completed on 2026-08-14 for the CareNest `1.0.0-rc.1` source.

CareNest remains a local-first organizational product. Nothing in this audit adds diagnosis, dosage calculation/inference, treatment recommendations, medication-interaction checking, clinical risk scoring, or an emergency-service substitute.

## Verification status correction and final authoritative baseline

PR #43 was originally documented as the final fully green verification. That statement was incorrect and is superseded by the actual GitHub Actions evidence.

Actual PR #43 required-gate evidence:

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

PR #43 was closed without merging its marker, but it is not release evidence and must not be represented as a successful exact-source baseline.

The continuing audit fixed the reminder failures and later platform-lifecycle/analyzer/dependency issues instead of suppressing or ignoring them.

The authoritative final automated bug-audit verification is marker-only PR #54:

- PR #54 — `Verify final CareNest bug-audit source`;
- branch: `ci/carenest-final-bug-audit-2-20260814`;
- source/base SHA frozen for runtime/test/dependency verification: `4490f3f86752841d436e981b29279970c90c947b`;
- marker head: `929168a0a319b15d9e89997d86436d59ae731ad1`;
- marker: `build/verification/bug-audit-final-20260814-2.txt`;
- PR closed without merge after successful evidence capture.

Final PR #54 evidence:

- CareNest CI #503 / run `31766059137`: **success**;
- platform-neutral formatting: **success**;
- `CareNest.UnitTests`: **122 passed, 0 failed, 0 skipped**;
- `CareNest.IntegrationTests`: **39 passed, 0 failed, 0 skipped**;
- `CareNest.UiTests`: **100 passed, 0 failed, 0 skipped**;
- total core tests: **261 passed, 0 failed, 0 skipped**;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- CodeQL #503 / run `31766059215`: **success**;
- Dependency Audit #35 / run `31766059132`: **success** with the former SQLite advisory suppression removed.

PR #53 independently completed a duplicate fully green verification of the same final runtime/test graph. It is useful corroborating evidence, but PR #54 is the recorded authoritative checkpoint. Both marker-only PRs were closed without merge, so neither verification marker is production source.

The verification marker is never intended for `main`; exact-source verification evidence is recorded, then the marker PR is closed unmerged.

## Failure-driven checkpoint history

The audit intentionally used multiple marker-only checkpoints. Failed checkpoints were treated as evidence and fixed rather than hidden or suppressed.

### PR #37

Checkpoint source exposed CA1068 on the new repository transaction helper because the `CancellationToken` parameter was not last.

Action:

- no analyzer suppression;
- helper changed to `RunAtomicAsync(Action<SQLiteConnection>, CancellationToken)`;
- all call sites updated;
- PR closed without merge.

The core job had already demonstrated formatting success and 111 unit tests passing before the analyzer/compiler failure was reached.

### PR #39

Checkpoint exposed:

- CA1001 because `ProfileEditorViewModel` owned an instance `SemaphoreSlim` without disposal semantics;
- a missing final newline in `ReminderPlanner.cs`.

The verification marker from this failed checkpoint was accidentally merged before the failed evidence was fully acted on. It was explicitly removed from `main` by commit `549c77120c2ff792337cb842bf7a0912483816ed` and PR #39 is not release evidence.

Action:

- profile photo staging now uses an app-lifetime/static gate;
- formatter defect corrected;
- no analyzer/formatter suppression.

### PR #40

Checkpoint demonstrated successful Release compilation for Android, Windows, iOS simulator, and Mac Catalyst, plus successful CodeQL and Dependency Audit.

The core job failed only because `EncryptedBackupService.cs` lacked a final newline, so the PR was closed without merge and not promoted.

Action:

- final newline restored while applying the later backup rollback correction;
- a new exact-head verification was required instead of reusing partial evidence.

### PR #41

Marker-only reminder-reconciliation checkpoint was intentionally superseded before promotion because the continuing audit identified additional medicine/profile delete-flow work.

It was closed without merge and is not release evidence.

### PR #42

Marker-only bug-audit checkpoint was intentionally superseded because appointment/reminder regression work was still being reviewed. It was closed without merge and not promoted.

### PR #43

PR #43 is historical failure evidence, not the final green baseline.

Its integration failures proved that the then-current source still mishandled snoozed and stale reminder occurrences. The platform builds, CodeQL, and dependency audit being green did not compensate for a failed required core suite.

### PR #44

PR #44 independently reproduced three reminder-reconciliation defects:

1. a future snooze disappeared from upcoming reminders after its original scheduled time passed;
2. an expired snooze remained `Snoozed` rather than becoming `Missed`;
3. a stale future occurrence remained scheduled instead of being cancelled/reconciled.

Fix:

`4cf2aec989233d213ac7b1099a50d44e1acc3ca0` — `fix: reconcile snoozed and stale reminder occurrences`

PR #44 was closed unmerged and is not release evidence.

### PR #46

The next checkpoint progressed far enough to expose broader UI-contract failures around OS-request reconciliation, including cancellation before replacement/invalidation and delete compensation.

Those failures drove the later medicine/profile/schedule reminder lifecycle changes rather than being suppressed.

PR #46 was closed unmerged and is not release evidence.

### PR #47

PR #47 began the SQLite dependency remediation on a side branch. Its unsuppressed Dependency Audit #28 / run `31765223239` succeeded.

`main` advanced with runtime source while the checkpoint was running, so it was closed without merge instead of being misrepresented as final combined-source evidence.

### PR #48

PR #48 replayed the SQLite remediation from newer source.

Observed:

- Dependency Audit #29 / run `31765388861`: success without the old suppression;
- CodeQL #469 / run `31765388858`: success;
- CareNest CI #469 / run `31765388909`: failed on a transient moving-base reminder-interface implementation mismatch.

The interface/source was corrected/simplified on `main`; PR #48 was closed unmerged.

### PR #49

PR #49 exposed CA1861 in two newly added medicine-reconciliation assertion arrays.

The analyzer was not disabled. Corrections include:

- `cc9465136bd7de0e55e14386c19fa849a3e56067`;
- `834b2980167c41bc7e9c1ad69dc54ad5ccc7e53e`.

The matching profile assertion was proactively made analyzer-safe as well.

PR #49 was closed unmerged and is not release evidence.

### PR #50

PR #50 again demonstrated successful unsuppressed Dependency Audit #31 / run `31765668949` for the SQLite remediation, but its source snapshot predated the later analyzer-safe reminder test fixes.

It was closed unmerged rather than reusing stale evidence.

### PR #51

Analyzer-clean reminder reconciliation verification was superseded by later appointment/reminder-action source and by the SQLite remediation moving directly onto `main`.

PR #51 was closed unmerged and its original stale SQLite-open wording was corrected.

### PR #52

PR #52 verified a unified source snapshot that already contained the SQLite remediation, but runtime source advanced afterward with cancellation-first reminder actions and failure-injection tests.

It was closed unmerged and is not final release evidence.

### PR #53

PR #53 independently completed a fully green verification of the same final runtime/test graph later recorded authoritatively by PR #54.

Its completed evidence included 122 unit tests, 39 integration tests, 100 UI-contract/policy tests, all four platform Release builds, CodeQL #501 / `31766026573`, and unsuppressed Dependency Audit #34 / `31766026570`.

PR #53 was closed without merging its marker and is retained as duplicate corroborating evidence.

### PR #54

PR #54 is the authoritative final automated bug-audit verification.

Final evidence:

- CareNest CI #503 / `31766059137`: success;
- formatting: success;
- 122 unit tests: success;
- 39 integration tests: success;
- 100 UI-contract/policy tests: success;
- 261 total core tests: success;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #503 / `31766059215`: success;
- unsuppressed Dependency Audit #35 / `31766059132`: success.

PR #54 was closed without merge and its marker is not part of `main`.

## Correctness fixes covered by the current source

### App lock

- PIN updates snapshot the existing enabled flag, salt, and verifier before writing replacement secure-storage values.
- Partial secure-storage write failure attempts non-cancelled rollback to the previous state.
- App-lock disable similarly snapshots and restores prior secure state if removals fail part way through.
- New and retrieved mutable salt/verifier/derived buffers are cleared where application-owned managed memory permits.
- Verification fails closed for invalid PIN shape, missing/corrupt salt, or missing/corrupt verifier material.
- Salt length is fixed at 16 bytes and verifier length at 32 bytes.
- PBKDF2 output length is not derived from potentially corrupt stored verifier length.
- PIN policy remains numeric, 6–32 digits.
- Plaintext PIN storage remains prohibited.

### Document-vault key handling

- Read/export paths never create a replacement document master key.
- Existing encrypted `.cndoc` payloads plus missing/corrupt master key fail closed.
- Import creates a new key only when no encrypted payload already depends on an existing key.
- Caller-owned key copies remain cleared where managed-memory control permits.

### Decrypted document export lifecycle

- Incomplete plaintext export files are removed if decryption/export/audit fails.
- Cleanup failure is surfaced rather than silently ignored when the operation itself failed.
- Successful decrypted document exports now use the managed `Exports` cache directory instead of the cache root.
- Settings → Clear Cache therefore covers successful temporary decrypted document exports.

### Profile deletion and encrypted-file cleanup

- The database cascade completes before CareNest attempts encrypted-file cleanup.
- Every associated encrypted document payload and encrypted profile-photo payload is attempted with `CancellationToken.None` after the database transition.
- One cleanup failure does not stop later cleanup attempts or deletion bookkeeping.
- Incomplete cleanup/bookkeeping is surfaced as aggregate failure after the structured records have already been removed.

### Profile photo staging

- Persisted, staged, and obsolete encrypted photo references are tracked separately.
- Replacing a staged image compensates the newly imported payload if old staged cleanup fails.
- Preview plaintext is written to a partial file and atomically moved into place.
- Preview cleanup is best effort and cannot block profile lifecycle operations.
- The photo staging gate is app-lifetime/static, eliminating the CA1001 disposable-instance problem while still serializing concurrent staging operations.

### Onboarding failure safety

- Optional PIN format is validated before creating the primary profile.
- Completion order is profile → optional app lock → defaults → onboarding-complete flag.
- Failure compensates app-lock/profile/completion state with non-cancelled cleanup attempts.
- Rollback failures are aggregated rather than hiding a partial setup.

### SQLite migrations

- Migration DDL and corresponding `SchemaInfo` version update execute inside a shared transaction boundary.
- A failed migration cannot leave the database partly migrated while claiming the newer schema version.

### Repository multi-step operations

A shared transaction boundary now protects multi-step operations including:

- primary-profile replacement/write;
- profile cascade deletion;
- medicine cascade deletion;
- schedule + schedule-time replacement;
- occurrence upsert batches;
- document + tag-link deletion;
- document-tag replacement;
- emergency-contact deletion/reference cleanup;
- full local structured-record clear.

The transaction helper follows analyzer-required cancellation-token parameter ordering.

`ClearAllAsync` performs the critical data transition transactionally. `VACUUM` is no longer part of that critical clear, so a compaction failure cannot interrupt the later encrypted-file/key/app-lock cleanup after structured records have already been cleared.

### ViewModel refresh reentrancy

- medication-log mutations use a non-reentrant `LoadCoreAsync` path instead of calling a second busy-guarded `LoadAsync` inside `RunAsync`;
- document mutations use the same non-reentrant refresh pattern;
- refreshed profile/medicine selections are re-bound to fresh collection objects by ID.

### Reminder action input validation

`HandleOccurrenceAsync` rejects unsupported action states before repository/platform mutation.

Accepted user/action states are limited to:

- Snoozed;
- Taken;
- Skipped;
- Delayed;
- Missed;
- Cancelled.

`Scheduled` and undefined enum values are not accepted as an action.

### Reminder action cancellation-first recovery

The later audit identified a stronger consistency rule for action handling: the old operating-system request must be cancelled before CareNest persists the handled action state.

Current behavior:

- cancel the existing platform request first;
- only then persist the requested action state;
- schedule the replacement snooze only after state persistence;
- if persistence or later snooze scheduling fails, restore the previous occurrence state using non-cancelled compensation;
- attempt a non-cancelled reminder rebuild so a cancelled request can be restored for still-actionable data;
- aggregate primary and recovery failures rather than claiming consistency;
- post-success audit bookkeeping is best effort so an already completed reminder action is not reported as failed only because audit persistence failed;
- a user-configured stock-adjustment bookkeeping failure after a Taken action is contained/logged privacy-safely instead of rolling back a completed reminder state/log transition.

Related source/test commits include:

- `1459d24314de4a2f2f4fa232deb4285bb8e33b23` — cancellation-first/recoverable action handling;
- `508adeb805d604274be8b069668429b6935f3fa6` — notification failure injection;
- `da2aed19ee9224b8d8661f11520ab9396e2c005e` — cancellation/recovery ordering verification.

### Medication-log input validation

Manual medication-log edits reject undefined `MedicationLogStatus` values before repository access.

### Android receiver lifetime

Boot/time/time-zone reminder recovery now uses `BroadcastReceiver.GoAsync()` and always calls `Finish()` in `finally` after its asynchronous recovery work.

A foreground/startup recovery pass can retry a failed background recovery; failures are contained instead of producing unobserved background faults.

### Windows reminder timer ownership

The unpackaged Windows in-process fallback now avoids several timer races:

- scheduled reminder CTS lifetime is independent from the short-lived caller cancellation token;
- timer token is captured before the background task runs;
- cancellation only cancels; the background owner performs disposal;
- an old timer removes the dictionary entry only if it is still the current owner for that occurrence ID;
- notification display failures are contained;
- replacement timers with the same occurrence ID cannot be removed by an older timer's `finally` block.

### Backup completion semantics

- a completely written encrypted backup is not reported as failed solely because local backup-history metadata could not be recorded afterward;
- a fully committed restore is not reported as failed solely because its post-restore audit entry could not be recorded afterward;
- those bookkeeping steps use non-cancelled best-effort writes after the primary operation has completed;
- bookkeeping diagnostics record only fixed operation text and exception type, not full exception messages/stack traces/health data;
- actual encryption, validation, filesystem, secret-store, and database replacement failures remain fatal.

### Backup rollback exact-state restoration

If restore fails after the document-key transition, rollback restores the exact prior secure-store byte state whenever prior bytes existed—even if those bytes were already malformed—instead of silently normalizing/removing prior state during a failed operation.

### CSV spreadsheet safety

String cells beginning, after optional leading whitespace, with spreadsheet formula prefixes are neutralized in exported CSV:

- `=`;
- `+`;
- `-`;
- `@`.

The stored CareNest value is unchanged; only the portable CSV cell representation is prefixed so common spreadsheet software treats user-entered text as text rather than a formula.

Numeric values remain numeric.

### Atomic report generation

Plaintext report writers use incomplete-file staging plus atomic replacement:

- CSV;
- PDF;
- profile JSON export.

Cancellation/serialization/write failure cannot leave a partially generated final report path. Incomplete plaintext files are removed best effort.

### Shared report-cache cleanup

After a report has been handed to the system share flow and control returns to CareNest, the application removes the temporary report file it still owns from the managed cache.

This does not claim deletion of copies already created by another application, selected cloud destination, OS share service, screenshot, backup, or filesystem snapshot.

Commit:

`c844acdb63b5320344ff0d771d1365eaf7471f4a` — `security: remove shared report cache files after export`

### Report profile refresh

Report profile selection is re-bound to the freshly loaded profile collection by ID, with primary/first fallback, instead of retaining a stale object reference indefinitely.

### Reminder planner edge cases

- `EveryNHours` no longer invents a +1 hour replacement when the user-selected local anchor falls inside a DST gap.
- A DST-gap interval anchor generates no reminder rather than silently changing the user's selected clock time.
- cycle on/off arithmetic uses `long` to avoid overflow for extreme user-entered integer values.
- interval schedule date-boundary checks avoid `DateTime.MaxValue` overflow.

### Startup recovery independence

Startup recovery steps are isolated:

- overdue reminder reconciliation;
- medicine reminder rebuild;
- appointment reminder rebuild;
- backup-reminder synchronization.

Cancellation still propagates. A non-cancellation failure in one recovery step is logged with fixed step/type metadata and does not prevent the remaining recovery steps from running.

### Reminder platform reconciliation

Reminder reconciliation now uses effective due time:

- normal scheduled row → `ScheduledUtc`;
- snoozed row → `SnoozedUntilUtc` when present.

Consequences:

- a future snooze remains visible even if the original scheduled time has passed;
- an overdue snooze can transition to missed based on its snoozed due time;
- rebuild cancels an existing platform request before deciding whether to replace, suppress, or invalidate it;
- a stale scheduled occurrence no longer remains as an obsolete platform alarm after its schedule changes;
- quiet-hours rebuild can cancel a previously scheduled request rather than merely declining to add another one;
- invalid future occurrences are marked Cancelled only after platform cancellation succeeds;
- cancellation failures remain retryable instead of falsely claiming reconciliation succeeded;
- snoozed reminders remain valid only while the associated schedule/medicine/profile context is still active and within current date bounds;
- caller cancellation is propagated instead of being swallowed by platform scheduling error handling.

Medicine schedule saves retain old occurrence rows until reconciliation has had a chance to cancel their existing platform identifiers.

Medicine/profile deletion cancels future platform reminder requests before the database cascade. If the cascade then fails, CareNest performs a non-cancelled best-effort rebuild to restore platform requests for the still-existing records.

Medicine/profile saves reconcile reminders before non-critical audit bookkeeping can incorrectly make the primary record transition appear failed.

Direct integration tests cover future snoozes whose original scheduled time is already past, overdue snoozes, and stale future occurrence cancellation/replacement behavior.

### Appointment reminder persistence compensation

Appointment persistence/platform state was added to the same reconciliation audit.

Current behavior ensures appointment reminder state is reconciled around persistence failures rather than assuming SQLite and the OS scheduler succeed/fail atomically.

Related commits:

- `61772f968d8686e472b5849e77e0a3156936701d` — `fix: reconcile appointment reminders around persistence`;
- `633b6bbca587fbc5030b940132b3112d7a73b458` — `test: cover appointment reminder persistence compensation`.

## SQLite dependency remediation

The earlier source resolved the tracked native dependency through the `2.1.11` path and temporarily used a narrow exact-advisory NuGet audit suppression. That exception was explicitly documented as not being remediation.

The current source now contains an actual compatible dependency-graph remediation.

Relevant `main` commits:

- `66cd701f84afd5021a28e7e3327b7da4fad249aa` — `fix: pin patched SQLite native dependency path`;
- `e939d5bd912d09ffa150c804519c15e2506b7bd7` — `security: remove resolved SQLite audit suppression`;
- `04868965c43d8a6d09b40075d92f20da9b26e32a` — `test: guard patched SQLite dependency baseline`.

Current package strategy:

- retain `sqlite-net-pcl` `1.9.172`;
- retain `SQLitePCLRaw.bundle_green` `2.1.11`;
- use central transitive pinning for maintained native/provider leaves;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- relevant provider packages at `2.1.12`;
- no remaining `NuGetAuditSuppress` entry for `GHSA-2m69-gcr7-jv3q`.

`SqliteDependencySecurityContractTests` requires the patched pin floor and requires that the old audit suppression not return.

Unsuppressed Dependency Audit succeeded repeatedly during remediation and finally on authoritative PR #54 Dependency Audit #35 / run `31766059132`.

This package resolution intentionally does not change:

- CareNest's local-first/no-required-backend architecture;
- SQLite schema semantics;
- health-record meaning;
- encrypted document framing;
- backup archive format;
- account model.

Manual existing-database/backup/encrypted-document/device compatibility checks remain production-release evidence and are not inferred from NuGet audit alone.

Authoritative dependency documents:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

## Repository-wide policy scan

A refreshed public `main` snapshot was scanned separately from GitHub Actions for:

- common sync-over-async patterns;
- `Thread.Sleep` in runtime source;
- implementation placeholders;
- TODO/FIXME markers in runtime source;
- direct runtime network clients;
- telemetry clients;
- common full-exception logger overloads;
- obsolete/stale PR #35 Settings architecture symbols;
- common signing/secret artifacts;
- failed/superseded bug-audit marker files accidentally remaining on `main`;
- missing final newlines in committed source/workflow files.

The final CI/CodeQL/platform verification remains the authoritative compile/test surface.

## Remaining production gates

A successful source audit does not make the public `1.0.0` production release automatic.

Still required outside this automated source pass:

- real-device/emulator manual matrix on Android, Windows, iOS/iPadOS, and Mac Catalyst;
- manual notification permission/real-delivery checks;
- cancellation-first reminder action checks against actual platform scheduling/restart recovery;
- Android alarm/battery/reboot/time/time-zone checks;
- manual existing-database upgrade/SQLite compatibility checks;
- manual document/import/export/backup/restore checks on packaged targets;
- canonical historical encrypted-format compatibility fixtures where available;
- screen-reader, large-text, keyboard, contrast, theme, and reduced-motion checks;
- current Apple App Store and Google Play policy review, including the voluntary external support link;
- signing identities/credentials maintained outside Git;
- signed package build/inspection;
- store screenshots/listings/privacy/data-safety metadata;
- final promoted-commit Release Evidence;
- final version/build metadata and release notes/checksums;
- production tag/GitHub release only after applicable gates pass.

## Dependency-risk status

`GHSA-2m69-gcr7-jv3q` is no longer being hidden by the former narrow source audit exception: the compatible current source graph pins the maintained native/provider leaves and the matching NuGet audit suppression has been removed.

The dependency source remediation and unsuppressed automated verification are complete, while packaged existing-database/backup/device compatibility remains a separate production validation gate.

No successful CI/audit run is interpreted as proof of manual data/device compatibility.
