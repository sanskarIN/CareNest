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
- Settings Clear Cache includes managed report/export/preview directories.

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

## Dependency risk remains separate

The current source audit does not remediate:

`GHSA-2m69-gcr7-jv3q`

A narrowly scoped audit suppression keeps other audit/build failures visible. It must not be described as a security fix.

See:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

## Final automated evidence

PR #43 is the exact-head verification reference for the final 2026-08-14 audited source.

Required formatter, core tests, all four platform Release builds, CodeQL, and Dependency Audit completed successfully. PR #43 was closed without merging its marker.

Manual real-device, accessibility, store-policy, signing and production-promotion gates remain separate.
