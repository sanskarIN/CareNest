# CareNest 2026-08-14 bug-audit verification

## Scope

This document records the repository-wide correctness, failure-safety, privacy, persistence, export, reminder, backup, platform-lifecycle, and regression-test audit completed on 2026-08-14 for the CareNest `1.0.0-rc.1` source.

CareNest remains a local-first organizational product. Nothing in this audit adds diagnosis, dosage calculation/inference, treatment recommendations, medication-interaction checking, clinical risk scoring, or an emergency-service substitute.

## Final verification

Final exact-head verification PR:

- PR #43 — `Verify final CareNest 2026-08-14 bug audit source`
- verification branch: `ci/carenest-final-bug-audit-20260814`
- verification marker: `build/verification/final-bug-audit-20260814.txt`
- PR #43 was closed without merge after the required workflow groups completed successfully.
- The marker is verification-only and is not part of `main`.

Successful required gates on the frozen PR #43 source:

- platform-neutral `dotnet format --verify-no-changes`;
- complete `CareNest.UnitTests` suite;
- complete `CareNest.IntegrationTests` suite;
- complete `CareNest.UiTests` contract/policy suite;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build;
- CodeQL;
- Dependency Audit.

The exact execution counts, job IDs, run IDs, runner logs, and timestamps remain preserved by GitHub Actions on PR #43. This document intentionally does not invent values that are already available in the immutable Actions evidence.

The successful Dependency Audit does **not** close `GHSA-2m69-gcr7-jv3q`. The SQLitePCLRaw dependency risk remains open and separately tracked.

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

The final source was frozen only after the remaining behavior fixes and direct reminder reconciliation integration tests were added.

All required gate groups completed successfully and PR #43 was closed without merging its marker.

## Correctness fixes covered by the final source

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

Direct integration tests cover future snoozes whose original scheduled time is already past, overdue snoozes, and stale future occurrence cancellation/replacement behavior.

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

The successful source audit does not make the public `1.0.0` production release automatic.

Still required outside this automated source pass:

- real-device/emulator manual matrix on Android, Windows, iOS/iPadOS, and Mac Catalyst;
- manual notification permission/real-delivery checks;
- Android alarm/battery/reboot/time/time-zone checks;
- manual document/import/export/backup/restore checks on packaged targets;
- screen-reader, large-text, keyboard, contrast, theme, and reduced-motion checks;
- current Apple App Store and Google Play policy review, including the voluntary external support link;
- signing identities/credentials maintained outside Git;
- signed package build/inspection;
- store screenshots/listings/privacy/data-safety metadata;
- final promoted-commit Release Evidence;
- final version/build metadata and release notes;
- explicit disposition of the open SQLitePCLRaw advisory.

## Dependency-risk status

`GHSA-2m69-gcr7-jv3q` remains OPEN for the currently resolved SQLitePCLRaw native dependency path.

Authoritative dependency files:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`

No successful CI or audit run should be interpreted as silently resolving that tracked advisory.
