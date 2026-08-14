# what_changed.md

## CareNest complete continuation handoff — 2026-08-14 final bug-audit state

This is the active detailed CareNest handoff after the repository-wide 2026-08-14 correctness, failure-safety, privacy, persistence, export, backup, notification, platform-lifecycle and regression-test audit.

Repository: `https://github.com/sanskarIN/CareNest`  
Branch: `main`  
Release target: `1.0.0-rc.1`  
Framework: .NET 10 / .NET MAUI  
Primary language: C#  
License: Apache-2.0  
Business email: `sanskarin@outlook.in`  
Support email: `supportramsandesh@gmail.com`  
Creator: `https://github.com/sanskarIN`  
Voluntary project support: `https://buymeacoffee.com/sanskarIN`  
Watermark: `Made by the Sanskar`

---

# Historical continuity — no earlier work discarded

The new active handoff does not erase the earlier project record.

Complete preserved/reference material already in the repository includes:

- `docs/history/what_changed_full_through_phase8.md` — complete early implementation/hardening/verification history;
- `docs/history/what_changed_documentation_through_20260812.md` — complete documentation-completion handoff;
- `docs/history/what_changed_through_pr33_20260813.md` — exact previous long handoff through the PR #33 service/document/backup/AEAD-v2 baseline;
- `docs/history/PROJECT_STATUS_through_PR33.md` — exact previous PR #33-era status snapshot;
- `docs/history/README_through_PR33.md` — preserved previous README snapshot;
- `docs/releases/SETTINGS_LIFECYCLE_VERIFICATION_20260813.md` — full Settings lifecycle recovery/verification record;
- `docs/releases/PHASE9_VERIFICATION_EVIDENCE.md` — PR #36 Phase 9 exact-head evidence;
- `docs/releases/POST_VERIFICATION_COMPARE_20260813.md` — previous post-verification documentation-only source-boundary evidence;
- `docs/releases/CHANGELOG_PHASE9_20260813.md` — previous Phase 9 change record;
- `docs/security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md` — local-data clear security model;
- `docs/testing/SETTINGS_LIFECYCLE_CONTRACT.md` — Settings lifecycle regression contract;
- `docs/privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md` — local privacy cleanup boundary.

The pre-audit active handoff also remains recoverable from Git history. This file advances the active record rather than pretending the earlier implementation never existed.

---

# Product and medical-safety boundary retained

CareNest remains a local-first organizational application.

CareNest does **not**:

- diagnose conditions;
- determine, calculate, or infer medicine dosage;
- recommend treatment;
- perform medication-interaction checking as a clinical feature;
- create clinical risk scores;
- independently verify medication adherence;
- replace a clinician or pharmacist;
- provide emergency services;
- guarantee reminder/notification delivery.

Medicine strength and instruction values remain opaque user-entered text.

Reminder times, snoozes, stock estimates and reports remain based on explicit user-entered/configured values. The 2026-08-14 work changes software correctness/failure handling only; it introduces no medical interpretation.

---

# Local-first/privacy boundary retained

Current v1 still has:

- no required CareNest account;
- no required CareNest backend/server;
- no automatic CareNest cloud synchronization;
- no silent caregiver sharing;
- no hidden analytics/telemetry client;
- local SQLite structured records;
- separately encrypted imported document payloads;
- manual password-encrypted backups;
- explicit user-controlled report/document/calendar sharing and export;
- optional local app lock;
- privacy-minimized logging/diagnostics.

The SQLite database is not described as transparently whole-database encrypted. Document payloads and manual backup payloads retain separate authenticated-encryption boundaries.

---

# Previous automated baseline before this bug audit

The immediately preceding exact automated source baseline was PR #36, verified on 2026-08-13.

PR #36 evidence included:

- formatting success;
- 106 unit tests;
- 30 integration tests;
- 56 UI-contract/policy tests;
- 192 total core tests;
- Android Release success;
- Windows Release success;
- iOS simulator Release success;
- Mac Catalyst Release success;
- CodeQL success;
- Dependency Audit success.

PR #36 was marker-only and closed without merge.

The 2026-08-14 source work below supersedes PR #36 as the current automated baseline.

---

# 2026-08-14 bug-audit goals

The user requested all project errors/bugs to be fixed and the complete project continued with maximum logical commits.

The audit therefore reviewed and hardened, rather than merely cosmetically editing:

- secure-store multi-key consistency;
- document master-key behavior;
- encrypted document export cleanup;
- profile deletion cleanup;
- profile photo staging and preview lifetime;
- onboarding rollback;
- SQLite migration consistency;
- repository multi-step transaction boundaries;
- full structured-data clear ordering;
- ViewModel refresh/reentrancy;
- reminder/medication-log enum input validation;
- Android broadcast receiver async lifetime;
- Windows reminder timer races;
- backup/restore completion semantics and rollback;
- CSV spreadsheet safety;
- CSV/PDF/JSON partial-file behavior;
- report selection refresh;
- reminder planner DST/overflow edges;
- startup recovery isolation;
- SQLite-row ↔ OS-notification reconciliation;
- medicine/profile deletion alarm cleanup;
- direct behavioral regression tests;
- repository-wide source policy scan;
- exact-head formatting/tests/platform builds/CodeQL/dependency audit.

---

# Detailed fix ledger

## 1. App-lock PIN replacement rollback

Commit:

`fd0b5dfb5de289c8c8002aff5de45acdf77af043`

App-lock PIN replacement now snapshots the previous:

- enabled flag;
- PBKDF2 salt;
- verifier.

If any new secure-store write fails, CareNest attempts non-cancelled restoration of the previous state.

New/previous mutable salt/verifier buffers are cleared where application-owned managed-memory control permits.

## 2. App-lock rollback/buffer contract

Commit:

`ab2aa0055ab97f85f4815f4854eafd0c87035f8e`

Automated contracts protect secure-store rollback and mutable-buffer clearing.

## 3. App-lock disable rollback

Commit:

`93aaea2d59d6611e9499fc29cb2d422741fced64`

Disable snapshots previous app-lock material and restores it if removal fails part way through.

## 4. App-lock disable rollback contract

Commit:

`9ba5a821610ed79df2da2b41bc1857d60014a2bd`

Regression coverage protects partial-disable compensation.

## 5. App-lock corrupt-material fail-closed verification

Later audit hardening further requires:

- salt exactly 16 bytes;
- verifier exactly 32 bytes;
- invalid entered PIN shape rejected before PBKDF verification;
- corrupt/missing stored material returns false instead of deriving from an attacker/corruption-controlled length;
- PBKDF output remains exactly 32 bytes;
- no plaintext PIN persistence;
- numeric 6–32 digit policy retained.

`tests/CareNest.UiTests/AppLockSecurityContractTests.cs` protects these invariants.

---

# Document-vault key safety

## 6. Fail closed instead of silently replacing a missing document key

Commit:

`69d925b197de281f1c29cfd8d63e4f28687b60a9`

Behavior:

- read/export never creates a replacement master key;
- import creates a key only when no existing encrypted payload depends on one;
- existing `.cndoc` payloads plus a missing/corrupt key fail closed.

This prevents CareNest from creating a new unrelated key that would strand existing encrypted documents.

## 7. Document-key persistence tracking

Commit:

`707286412775d2aefd2ec175baa4afb36b0a7b04`

Regression support tracks secure-store key persistence behavior.

## 8. Document-key integration tests

Commit:

`23dc160b8dba2c6720e5ebb81f394c20ca51709e`

Integration coverage includes missing/invalid key behavior, no silent replacement, tamper handling and mutable key-buffer hygiene.

---

# Plaintext document export cleanup

## 9. Failed export removes plaintext artifacts

Commit:

`94430a17b696bc18b8e6e83825dfd316d30292b1`

`DocumentService.ExportToTemporaryFileAsync` now cleans plaintext output created by a failed decrypt/export/audit operation.

If the primary operation and cleanup both fail, incomplete cleanup is surfaced instead of silently ignored.

## 10. Document-store failure injection

Commit:

`0da854fe74b11fa9a992e770c340a2eed4a61548`

The document-store test double can inject failures for cleanup regression tests.

## 11. Plaintext export cleanup unit coverage

Commit:

`4020edadee7a754bb71138a603509d1dccdcaa85`

Unit tests cover partial/complete plaintext cleanup on failed export paths.

## 12. Successful document exports use managed cache

Later audit work routes successful decrypted temporary exports into:

`FileSystem.Current.CacheDirectory/Exports`

instead of the cache root.

Settings Clear Cache already owns the `Exports` directory, so successful temporary decrypted exports now participate in user-visible cache cleanup.

`tests/CareNest.UiTests/DocumentExportCacheContractTests.cs` protects this boundary.

---

# Profile deletion cleanup

## 13. Attempt all encrypted-file cleanup after database cascade

Commit:

`5e9d2cda8efa551b749e21cde3064f5bbed0b918`

After the structured profile cascade succeeds, CareNest attempts every associated encrypted document/profile-photo cleanup with `CancellationToken.None`.

One cleanup failure does not stop later cleanup attempts or deletion bookkeeping.

## 14. Nullable cleanup correctness

Commit:

`011fa03ba18e5c4d453f388817e4f755aa11b7c0`

Nullable encrypted-file cleanup handling was corrected.

## 15. Profile deletion cleanup regression test

Commit:

`877308e0a0fc93d99ebe5eda9fed5ca34adc8661`

A test verifies that one encrypted-file failure still allows later payload cleanup and audit attempt.

Later reminder reconciliation work also cancels future profile platform requests before cascade deletion and attempts non-cancelled rebuild compensation if the cascade fails.

---

# Profile photo staging lifecycle

A series of logical commits separated persisted/staged/obsolete encrypted photo references, added staging synchronization, atomic preview replacement and best-effort page-disappearance cleanup.

Important final behavior:

- persisted encrypted photo is not deleted before profile save commits the replacement;
- old staged payload cleanup happens before accepting a new staged reference;
- if old staged cleanup fails, the newly imported payload is compensated;
- plaintext preview uses `.partial` then atomic move;
- preview cleanup cannot block profile lifecycle;
- page disappearance attempts staged cleanup;
- final synchronization gate is app-lifetime/static.

Checkpoint PR #39 later proved that instance ownership of the semaphore triggered CA1001. The fix changed the gate to:

`private static readonly SemaphoreSlim PhotoGate = new(1, 1);`

Important related commits include:

- `0b55e27b10a8ff181bb00255f4e99b37c45d78f4` — static/app-lifetime photo staging gate;
- `0edf5a0347f6620decb4e9b1d182488f047948a3` — contract updated for shared gate.

Regression file:

`tests/CareNest.UiTests/ProfilePhotoLifecycleContractTests.cs`

---

# SQLite migration atomicity

## 16. Migration DDL + schema version use one transaction

Commit:

`7d34f4e676cd7c465ae6627ca7f831241a39d2a3`

A migration cannot partly apply DDL and separately claim its schema version.

## 17. Migration transaction contracts

Commit:

`0612af1c317197fc4ae8f107c296077208ac2186`

Regression contracts protect the migration boundary.

---

# Atomic repository multi-step writes

## 18. Shared atomic repository helper

Commit:

`3b7412baab822c6c8aca5dffb81cce05caa003e7`

Multi-step repository changes moved behind a common transaction boundary.

Protected operations include profile/schedule/cascade/tag/contact/full-clear paths where partial persistence would create an inconsistent local model.

## 19. Transaction rollback integration coverage

Commit:

`a665aafa6e1175d04159772d33ca844ed043ed23`

Integration tests exercise rollback for primary-profile and schedule/time multi-step changes.

## 20. CA1068 correction

PR #37 exposed CA1068 because the transaction helper did not place `CancellationToken` last.

Fix commit:

`c2aa8bc74230c17726766534f5bdb01990d1bfba`

Final helper shape:

`RunAtomicAsync(Action<SQLiteConnection> action, CancellationToken cancellationToken)`

No analyzer suppression was added.

## 21. Full clear no longer depends on VACUUM

Commit:

`a4517419b45a59999caef071c64be39d5379747e`

`ClearAllAsync` now performs the critical structured-data transition transactionally but does not make later privacy cleanup depend on `VACUUM` succeeding after the delete transaction has already committed.

Regression coverage lives in:

`tests/CareNest.UiTests/RepositoryTransactionContractTests.cs`

---

# ViewModel refresh/reentrancy fixes

## Medication log

Commit:

`dace1c608ba17aee7bb70d171807536b580ea15c`

Mutation/filter paths use a non-reentrant `LoadCoreAsync` rather than calling a second busy-guarded `LoadAsync` while the current operation is still inside `RunAsync`.

Fresh profile/medicine selections are rebound by ID.

## Documents

Equivalent non-reentrant refresh/reselection behavior was applied to `DocumentsViewModel`.

This prevents successful mutations from failing to refresh merely because the ViewModel is still marked busy by the outer operation.

---

# Reminder action validation

Commit:

`1f46fdf8...` in the bug-audit history introduced strict action-state validation.

`HandleOccurrenceAsync` accepts only:

- Snoozed;
- Taken;
- Skipped;
- Delayed;
- Missed;
- Cancelled.

`Scheduled` and undefined enum values are rejected before state/platform mutation.

Unit coverage protects the boundary.

---

# Medication-log status validation

Manual edit now checks `Enum.IsDefined(status)` before repository access.

Undefined `MedicationLogStatus` values fail with an argument-range error instead of being persisted.

Regression file:

`tests/CareNest.UiTests/MedicationLogInputContractTests.cs`

---

# Onboarding rollback

Commit:

`cd48d1ac...` in the bug-audit history hardened onboarding order and compensation.

Final setup order:

1. validate input/PIN;
2. create primary profile;
3. optional app-lock state;
4. default settings;
5. onboarding-complete flag last.

Failure attempts non-cancelled cleanup of partial state and aggregates rollback failures.

Regression coverage:

`tests/CareNest.UiTests/OnboardingRollbackContractTests.cs`

---

# Android broadcast receiver lifetime

The boot/time/time-zone receiver previously launched asynchronous rebuild work after `OnReceive` without extending receiver lifetime.

Final behavior:

- call `GoAsync()`;
- run reminder/appointment/backup recovery asynchronously;
- contain non-fatal background failures;
- always call `pendingResult?.Finish()` in `finally`.

Regression file:

`tests/CareNest.UiTests/AndroidReceiverLifecycleContractTests.cs`

Later checkpoint platform evidence demonstrated Android Release compilation success before the final PR #43 full-green verification.

---

# Windows reminder timer race fixes

Final in-process fallback behavior:

- scheduled timer owns its own CTS;
- timer lifetime is not linked to the caller's short-lived cancellation token;
- token is captured before background work;
- cancellation cancels but does not prematurely dispose;
- background owner disposes;
- old same-ID timer removes the dictionary entry only if it is still the current owner;
- background notification display failures are contained.

Regression file:

`tests/CareNest.UiTests/WindowsNotificationTimerContractTests.cs`

Windows Release compiled successfully in checkpoint PR #40 and again in final PR #43.

---

# Backup completion semantics

The audit distinguished primary encrypted operation completion from later local bookkeeping.

Final behavior:

- completely written encrypted backup remains a successful backup even if local backup metadata recording fails afterward;
- fully committed restore remains a successful restore even if post-restore audit recording fails afterward;
- those bookkeeping operations use `CancellationToken.None` after primary success;
- failure logging records fixed operation text + exception type only;
- exception message, stack trace and health-record details are not logged;
- actual crypto/tamper/password/filesystem/secure-store/database failures remain fatal.

Regression file:

`tests/CareNest.UiTests/BackupCompletionSemanticsContractTests.cs`

---

# Exact previous key restoration on failed backup restore

A failed restore now rolls the document master key back to the exact previous secure-store byte state whenever previous bytes existed.

The rollback no longer restores only a `{ Length: 32 }` previous value and silently removes a pre-existing malformed value.

The failed operation therefore does not normalize unrelated pre-existing state.

This invariant is protected in `BackupCompletionSemanticsContractTests.cs`.

---

# CSV spreadsheet formula neutralization

`CsvWriter` now treats string cells whose first non-whitespace character is one of:

- `=`;
- `+`;
- `-`;
- `@`

as formula-like portable spreadsheet input and prefixes the exported representation so common spreadsheet software treats it as text.

The stored CareNest value is not modified.

Numeric values remain numeric.

Integration coverage was added to `ReportExportTests.cs`.

---

# Atomic plaintext report writers

Final report generation uses staging + atomic move for:

- CSV;
- PDF;
- profile JSON.

Cancellation/serialization/write failure cannot leave the incomplete staging file presented under the final output filename.

Incomplete plaintext staging is deleted best effort in `finally`.

Regression file:

`tests/CareNest.UiTests/ReportExportSafetyContractTests.cs`

---

# Report profile refresh

`ReportsViewModel` stores the current selected profile ID before refreshing, reloads fresh profile rows, then reselects:

1. same ID if present;
2. primary profile;
3. first profile.

It no longer indefinitely retains a stale profile object after the collection refreshes.

---

# Reminder planner edge cases

## DST-gap interval anchor

`EveryNHours` used to shift an invalid daylight-saving start anchor forward by one hour.

That violated CareNest's existing deterministic rule that an invalid local clock time is not replaced with an invented reminder time.

Final behavior:

- invalid DST-gap anchor → no generated interval occurrence until the user chooses a valid anchor;
- no hidden +1 hour substitution.

## Cycle integer overflow

Cycle on/off arithmetic now widens user-entered integer values to `long` before addition/modulo.

## Maximum date boundary

Interval scheduling compares nullable date bounds without adding beyond `DateTime.MaxValue`.

Unit regression file:

`tests/CareNest.UnitTests/ReminderPlannerEdgeCaseTests.cs`

Coverage includes:

- New York 2026 spring-forward 02:30 interval anchor;
- `int.MaxValue` cycle on/off counts;
- `DateTime.MaxValue.Date` schedule/medicine end boundary.

---

# Startup recovery independence

Startup recovery used to share one failure boundary for multiple independent recovery operations.

Final recovery steps:

- `overdue-reminder-reconciliation`;
- `medicine-reminder-rebuild`;
- `appointment-reminder-rebuild`;
- `backup-reminder-sync`.

Each is run independently.

`OperationCanceledException` propagates; other failures are contained so later recovery work still executes.

Logging remains privacy-safe: fixed step + exception type, no health content/message/stack trace.

Regression file:

`tests/CareNest.UiTests/StartupRecoveryContractTests.cs`

---

# Reminder reconciliation — major final correctness fix

SQLite reminder rows and operating-system scheduled requests are separate state surfaces. The audit added explicit reconciliation between them.

## Effective due time

- Scheduled → `ScheduledUtc`;
- Snoozed with explicit snooze due → `SnoozedUntilUtc`.

This fixes the case where a snoozed reminder's original time is already in the past but its snoozed time is still future.

## Upcoming behavior

Future snoozes remain in upcoming reminders according to their snooze due time.

## Overdue behavior

An overdue snooze can transition to Missed based on the snoozed due time.

## Rebuild behavior

For actionable future rows, rebuild:

1. calculates current schedule validity;
2. attempts to cancel the existing platform request;
3. only after successful cancellation decides whether to cancel/suppress/replace it;
4. leaves cancellation failures retryable;
5. cancels stale occurrences that are no longer produced by the current schedule;
6. cancels previously scheduled platform requests when current quiet-hour policy says not to schedule a replacement;
7. schedules a replacement only when the row is still valid and due outside quiet hours.

## Snooze validity

Snoozed rows remain valid only while their schedule/medicine/profile context remains active and their effective due date remains within current user-entered date bounds.

## Schedule save

`MedicineService.SaveScheduleAsync` intentionally does **not** delete old future occurrence rows before rebuild.

Those old rows retain the occurrence IDs needed to cancel stale OS-level platform requests safely.

## Medicine save

Medicine save runs reminder reconciliation before non-critical audit bookkeeping so a state/date change cannot leave an obsolete platform request solely because later audit persistence failed.

## Medicine delete

- cancel future platform requests for that medicine first;
- attempt database cascade;
- on cascade failure, best-effort non-cancelled rebuild restores platform requests for still-existing records;
- on success, deletion audit runs afterward.

## Profile save/delete

Profile archive/unarchive changes trigger reconciliation when a coordinator is available.

Profile deletion:

- cancels future profile platform requests before cascade;
- compensates with non-cancelled rebuild if cascade fails;
- retains the existing exhaustive encrypted-file cleanup behavior after successful cascade.

## Platform failure logging

Platform scheduling/cancellation failure logging records operation category + exception type only.

Caller cancellation is propagated instead of swallowed.

Regression source-contract file:

`tests/CareNest.UiTests/ReminderReconciliationContractTests.cs`

Direct integration behavior file:

`tests/CareNest.IntegrationTests/ReminderReconciliationBehaviorTests.cs`

Direct behavior scenarios include:

- future snooze whose original due time is already past;
- overdue snooze becoming missed;
- stale future occurrence cancelled/marked Cancelled and not rescheduled.

---

# Repository-wide static policy scan

A refreshed public `main` snapshot was downloaded after the source audit and scanned separately from Actions.

The scan checked runtime/test/workflow files for:

- common sync-over-async patterns;
- `Thread.Sleep`;
- `NotImplementedException`;
- TODO/FIXME placeholders in runtime source;
- direct runtime network clients;
- telemetry clients;
- common full-exception logging overloads;
- obsolete PR #35 Settings architecture symbols;
- common signing/secret artifacts;
- failed/superseded 2026-08-14 verification markers remaining on `main`;
- missing final newlines in committed source/workflow files.

GitHub-hosted formatting/build/test/CodeQL remains the authoritative automated verification surface.

---

# Checkpoint verification history

The audit deliberately used failure-driven exact-head marker branches rather than suppressing failures.

## PR #37 — failed/superseded

Checkpoint source:

`d6d81baa74ba37887129dc137aa6d325f625dcbc`

Observed:

- formatting: success;
- unit tests: 111 passed at this checkpoint;
- CA1068 exposed transaction-helper token ordering during later compilation.

Action:

- helper/call sites corrected;
- no analyzer suppression;
- PR closed unmerged;
- not release evidence.

## PR #39 — failed/superseded, marker cleanup required

Checkpoint source:

`f3fdb123280ca62f1e4aa703e2b47ad4f628590c`

Observed:

- CA1001 on instance `SemaphoreSlim` ownership;
- missing final newline in `ReminderPlanner.cs`.

The verification marker was accidentally merged before the failed checks were fully acted on.

Removal commit:

`549c77120c2ff792337cb842bf7a0912483816ed` — failed checkpoint marker removed from `main`.

Action:

- static/app-lifetime profile-photo gate;
- newline corrected;
- PR #39 explicitly not release evidence.

## PR #40 — partial successful platform evidence, not promoted

Checkpoint source:

`7ac567fb11546db7701f328ae860880ddd5b6eef`

Observed:

- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL: success;
- Dependency Audit: success;
- core formatting failed only because `EncryptedBackupService.cs` lacked its final newline;
- core tests were skipped after formatting failed.

Action:

- newline corrected during later backup rollback edit;
- checkpoint closed unmerged;
- not release evidence.

## PR #41 — superseded

Reminder-reconciliation checkpoint was opened, then intentionally closed because further medicine/profile delete-flow source changes were already known.

No partial promotion.

## PR #42 — superseded

Bug-audit checkpoint was intentionally closed because the audit still had confirmed source/test work remaining.

No partial promotion.

## PR #43 — final green exact-head source

PR:

`https://github.com/sanskarIN/CareNest/pull/43`

Title:

`Verify final CareNest 2026-08-14 bug audit source`

Verification branch:

`ci/carenest-final-bug-audit-20260814`

Marker:

`build/verification/final-bug-audit-20260814.txt`

Result:

- platform-neutral formatting: success;
- complete unit suite: success;
- complete integration suite: success;
- complete UI-contract/policy suite: success;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL: success;
- Dependency Audit: success.

PR #43 was closed without merge after success. Its marker is not part of `main`.

GitHub Actions on PR #43 preserves the exact suite counts, run IDs, job IDs, runner/toolchain versions and timestamps.

---

# New documentation-only bug-audit evidence

After the source was frozen/verified, only Markdown evidence/status files were changed.

Added:

- `docs/releases/BUG_AUDIT_VERIFICATION_20260814.md` — full fix/checkpoint/verification report;
- `docs/testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md` — defect → source → test/contract mapping;
- `docs/security/BUG_AUDIT_SECURITY_NOTES_20260814.md` — security/privacy-relevant audit boundaries.

Updated:

- `PROJECT_STATUS.md` — PR #43 promoted as current automated source baseline;
- `what_changed.md` — this active complete handoff.

Further documentation-only alignment may update README/changelog/documentation-index files without changing the PR #43 runtime/test baseline.

---

# Open SQLite dependency risk — unchanged and still real

Tracked advisory:

`GHSA-2m69-gcr7-jv3q`

The current `sqlite-net-pcl` dependency path still resolves the tracked SQLitePCLRaw native version documented in the risk register.

The repository does **not** claim this advisory is fixed.

The current narrow exact-advisory audit suppression is not remediation.

Authoritative files:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

Final production promotion must resolve or explicitly block on the documented dependency-risk decision.

---

# Buy Me a Coffee / support integration retained

Voluntary project support remains:

`https://buymeacoffee.com/sanskarIN`

Existing support integration remains separate from health data and app behavior:

- centralized app constant;
- in-app About support action;
- `.github/FUNDING.yml`;
- `BUY_ME_A_COFFEE.md`;
- `SUPPORT.md`;
- `docs/SUPPORT_CARENEST.md`;
- CareNest support vector artwork;
- repository/about support contracts.

Support does not unlock medical advice, different reminder behavior, premium health behavior, support priority, or access to user health data.

Current Apple/Google store-policy review for the optional external support link remains an external release gate.

---

# Production blockers that remain real

The source is now automated-release-candidate green under final PR #43.

That does not make external/manual work magically complete.

Still required:

1. Android real-device/emulator manual matrix.
2. Windows manual matrix.
3. iOS/iPadOS manual matrix.
4. Mac Catalyst manual matrix.
5. Notification permission denied/granted checks.
6. Real reminder delivery checks.
7. Android exact/inexact alarm checks.
8. Android battery optimization behavior.
9. Android reboot recovery.
10. Device clock/time-zone change behavior.
11. Appointment reminder packaged-target behavior.
12. Managed document import/export checks.
13. Profile photo import/capture/change/remove checks.
14. Report export/share checks.
15. Encrypted backup create/inspect/restore checks.
16. Wrong backup password checks.
17. Tampered backup checks.
18. Clean-install restore checks.
19. Legacy encrypted-format fixture verification where canonical fixtures are available.
20. Screen-reader checks.
21. Large text/text scaling.
22. Keyboard/focus desktop checks.
23. Contrast/light/dark/system theme checks.
24. Reduced-motion checks.
25. Current Apple App Store external-support-link policy review.
26. Current Google Play external-support-link policy review.
27. Signing identities/credentials maintained outside Git.
28. Signed package build and inspection.
29. Store screenshots using fictional data.
30. Store descriptions/privacy/data-safety disclosures.
31. Final SQLitePCLRaw advisory disposition.
32. `CareNest Release Evidence` workflow for the exact production commit.
33. Final version/build metadata.
34. Final release notes/checksums.
35. Production tag/GitHub release only after applicable gates pass.

No item above is marked complete merely because automated source verification succeeded.

---

# Deferred v1 scope remains unchanged

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

Any future networked feature requires a new explicit consent/authentication/key/privacy/threat/export/store architecture review.

---

# Environment truth

The repository assembly environment does not provide local MAUI device simulators, signing credentials, or store submission sessions.

GitHub-hosted Actions is the authoritative automated compilation/test surface for the final PR #43 source.

Manual device/accessibility/store/signing/release work remains separate and is not claimed complete unless actually performed.

---

# Current repository interpretation

- CareNest `1.0.0-rc.1` remains source-complete for the current v1 scope.
- PR #43 is the latest successful exact-head automated source verification.
- Formatting, all core test suites, all four platform Release builds, CodeQL and Dependency Audit are green on the frozen final bug-audit source.
- No failed/superseded bug-audit marker is intentionally part of `main`; the accidentally merged PR #39 marker was removed explicitly.
- The repository contains direct behavioral and source-contract regression coverage for the major bug classes discovered during this audit.
- The open SQLitePCLRaw advisory remains open.
- Manual device/accessibility/store/signing/final-release work remains blocking.
- No cloud/account/clinical-decision functionality was introduced by this audit.

See `docs/releases/BUG_AUDIT_VERIFICATION_20260814.md` and `docs/testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md` for the final evidence map.
