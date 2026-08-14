# CareNest security notes — 2026-08-14 bug audit

## Purpose

This document records security/privacy-relevant engineering changes made during the 2026-08-14 correctness audit. It supplements, rather than replaces:

- `SECURITY.md`;
- `PRIVACY.md`;
- `docs/security/SECURITY_MODEL.md`;
- `docs/security/THREAT_MODEL.md`;
- `docs/security/LOGGING_PRIVACY.md`;
- `docs/security/DEPENDENCY_RISK_REGISTER.md`.

CareNest remains a local-first organizer. These changes improve software failure safety; they do not add clinical interpretation or make CareNest an emergency/medical-decision system.

## Secure-store consistency

### App lock

App-lock updates and disable operations touch multiple secure-storage keys. The audit treats those writes as one logical state:

- enabled flag;
- PBKDF2 salt;
- verifier.

Before changing those values, CareNest snapshots the previous state. If a later write/removal fails, CareNest performs non-cancelled rollback attempts so a caller cancellation cannot intentionally stop consistency repair.

Rollback failure is not hidden. It becomes aggregate failure containing the primary operation failure plus rollback failures.

Verification fails closed when:

- entered PIN shape violates the configured policy;
- stored salt is missing or not exactly 16 bytes;
- stored verifier is missing or not exactly 32 bytes.

No corrupted stored length is used to select PBKDF output length.

### Document master key

Document-vault read/export paths do not create replacement key material.

If encrypted payloads already exist but the master key is missing/corrupt, CareNest stops rather than generating a new unrelated key that would make the existing payloads permanently unreadable through the application.

Backup restore rollback restores the exact previous byte state when previous bytes existed. Failed restore therefore does not normalize or replace pre-existing secure-store state as an unrelated side effect.

## Plaintext lifecycle

CareNest encrypts imported document payloads at rest within its document vault, but explicit export/report/share operations necessarily create plaintext or portable output.

The audit tightened temporary plaintext handling:

- failed decrypted document export removes incomplete/final plaintext created by that failed operation;
- successful decrypted document exports are kept under the managed `Exports` cache directory;
- CSV/PDF/JSON report writers stage to a `.partial` path before atomic final move;
- failed/cancelled report generation removes incomplete staging files best effort;
- profile photo preview uses the same partial-file then atomic-move principle;
- Settings Clear Cache includes managed report/export/preview directories;
- shared report flows remove the application-owned temporary report file after the share handoff returns, while not claiming deletion of copies already controlled by another application or destination.

These controls do not erase copies already handed to another application, system share destination, cloud provider chosen by the user, screenshot, backup, filesystem snapshot, or other OS/external surface.

## Spreadsheet formula neutralization

CSV quoting alone does not prevent a spreadsheet application from evaluating text beginning with formula prefixes.

For string values only, CareNest prefixes exported formula-like cells when their first non-whitespace character is:

- `=`;
- `+`;
- `-`;
- `@`.

This is an export representation change. It does not modify the original user-entered CareNest record.

## Async platform lifetime

### Android

System broadcast reminder recovery now holds a `PendingResult` from `GoAsync()` until asynchronous rebuild work completes, with `Finish()` guaranteed in `finally`.

This prevents the receiver lifetime from ending before the asynchronous recovery operation is finished.

### Windows

The unpackaged in-process timer fallback separates scheduled-reminder lifetime from a short-lived caller cancellation token. Cancellation ownership and disposal ownership are also separated to remove races around immediate cancel/replacement.

An older timer may remove the dictionary entry only when it is still the current owner for that occurrence ID.

## Platform reminder reconciliation

SQLite reminder rows and operating-system scheduled requests are separate state surfaces. The audit now explicitly reconciles them.

Before replacement/suppression/invalidation, CareNest attempts to cancel the old platform request.

If platform cancellation fails:

- the row is left retryable;
- CareNest does not falsely mark reconciliation complete;
- the failure is logged using safe fixed operation text and exception type only.

Snoozed rows use their explicit `SnoozedUntilUtc` as effective due time.

Medicine/profile deletion cancels future platform requests before the database cascade. If the database cascade then fails, CareNest attempts a non-cancelled reminder rebuild to restore platform scheduling for the records that still exist.

Medicine/profile save flows reconcile reminders after the structured record changes but before later audit bookkeeping can incorrectly make the primary state transition appear failed.

Appointment reminder persistence now follows the same failure-safety principle: if the structured appointment changes and later platform scheduling/persistence work fails, CareNest attempts reconciliation instead of silently leaving the database and OS request surfaces inconsistent.

### Reminder action ordering and compensation

Handled reminder actions now cancel the existing operating-system request before committing the new handled state.

This prevents CareNest from recording a reminder as taken/skipped/delayed/missed/snoozed while an old platform request is still known to be scheduled.

If a later state/snooze scheduling operation fails after cancellation:

- the previous occurrence state is restored with non-cancelled persistence;
- CareNest attempts a non-cancelled reminder rebuild to restore the platform request state;
- if both the primary action and recovery fail, the operation surfaces aggregate failure rather than claiming consistency;
- post-success reminder-action audit bookkeeping is best effort and privacy-safe rather than undoing an already completed user action;
- user-configured stock adjustment failure after a Taken log is contained and privacy-safely logged instead of falsifying the completed reminder action.

Failure-injection tests exercise cancellation/scheduling ordering and recovery behavior.

## Logging boundary

New failure logging added during the audit follows the existing privacy policy:

Allowed:

- fixed operation category;
- fixed recovery step identifier;
- exception type name.

Not logged:

- exception message;
- stack trace;
- medicine/profile/document names;
- notes;
- contact details;
- health-record IDs;
- document content;
- PIN/password/key bytes.

## Backup completion semantics

A security-sensitive operation must distinguish primary success from later bookkeeping failure.

The audit therefore treats these as post-success best-effort metadata:

- local backup-history metadata after encrypted backup bytes are completely written;
- restore audit entry after document/key/database replacement is fully committed.

A failure in those bookkeeping writes does not cause CareNest to falsely tell the user that the already-completed encrypted backup/restore itself failed.

Actual cryptographic validation, password/tamper validation, filesystem staging, secure-store transition, and database replacement failures remain fatal.

## Repository and migration atomicity

Multi-step SQLite consistency operations use transaction boundaries so a failure cannot leave a partially updated logical entity set.

Migration DDL and schema-version recording are one logical transaction.

Full structured-data clear is transactional, while database compaction is not a prerequisite for later privacy cleanup.

## SQLite dependency remediation

The earlier version of this document correctly treated the audit suppression as **not** being a security fix. That temporary state has since been replaced by an actual dependency-graph remediation.

Previously tracked advisory:

`GHSA-2m69-gcr7-jv3q`

Current source controls:

- `SQLitePCLRaw.lib.e_sqlite3` is centrally pinned to `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` is centrally pinned to `2.1.12`;
- `SQLitePCLRaw.provider.e_sqlite3`, `SQLitePCLRaw.provider.sqlite3`, and `SQLitePCLRaw.provider.dynamic_cdecl` are centrally pinned to `2.1.12`;
- `sqlite-net-pcl` remains `1.9.172` and `SQLitePCLRaw.bundle_green` remains `2.1.11` while central transitive pinning selects the maintained native/provider leaves;
- the exact `NuGetAuditSuppress` entry was removed from `Directory.Build.props`;
- `SqliteDependencySecurityContractTests` rejects restoration of the old native/provider floor or the advisory suppression.

Remediation commits:

- `66cd701f84afd5021a28e7e3327b7da4fad249aa` — native/provider pins;
- `e939d5bd912d09ffa150c804519c15e2506b7bd7` — audit-suppression removal;
- `04868965c43d8a6d09b40075d92f20da9b26e32a` — regression contract.

Unsuppressed Dependency Audit succeeded during multiple remediation checkpoints, including PR #47, PR #48, PR #50, and the current PR #53 dependency-audit run. Superseded PRs are retained as historical evidence only; the complete latest-source build/test/CodeQL matrix remains the release-level automated gate.

The package remediation intentionally does not change CareNest's schema, health-record semantics, encrypted-document framing, backup archive format, network boundary, or account model.

Manual existing-database upgrade, backup/restore, encrypted-document and packaged-device checks remain required release evidence.

See:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

## Automated verification correction

PR #43 was originally described as the final green exact-head reference for source `6e920f38972155ba9eaa6693f6ad948ebf6d1db7`. That description was incorrect and is superseded by the actual GitHub Actions records.

Actual PR #43 evidence:

- CareNest CI #448 / run `31764449533`: **failure**;
- formatting: success;
- unit tests: success;
- integration tests: failure;
- UI-contract/policy tests: skipped after integration failure;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #448 / run `31764449600`: success;
- Dependency Audit #23 / run `31764449574`: success.

PR #44 independently reproduced three reminder-reconciliation integration failures on the same source:

- a future snooze was omitted when its original scheduled time was already past;
- an expired snooze remained `Snoozed` instead of transitioning to `Missed`;
- a stale future occurrence remained scheduled instead of being cancelled before replacement scheduling.

Those runtime defects were fixed in commit:

`4cf2aec989233d213ac7b1099a50d44e1acc3ca0` — `fix: reconcile snoozed and stale reminder occurrences`

Later checkpoints intentionally continued finding/fixing platform reconciliation and analyzer issues rather than reusing PR #43 as proof. PRs #46 and #49 exposed additional lifecycle/analyzer failures; PRs #47/#48/#50 exercised the SQLite remediation while `main` was still moving; PRs #51/#52 were superseded exact-source markers.

The current marker-only automated candidate is PR #53. Its source includes the subsequent reminder action cancellation/recovery work and the SQLite remediation. It becomes an authoritative automated baseline only if every required formatting/test/platform/CodeQL/unsuppressed-audit gate is green on that source. Its marker file must not be merged.

Manual real-device, encrypted-data compatibility, accessibility, store-policy, signing and production-promotion gates remain separate even after a fully green automated source verification.
