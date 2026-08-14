# CareNest Security Release Review

Complete this review against the exact commit proposed for a public release.

## Local-first boundary

- [ ] No required account or CareNest backend was added.
- [ ] Runtime source contains no `HttpClient`, gRPC client or telemetry client introduction.
- [ ] External links remain fixed destinations opened only after explicit user action.
- [ ] The Buy Me a Coffee URL contains no health/profile/document/reminder identifiers or query payload.

## Health-data and scheduling boundary

- [ ] No diagnosis feature was added.
- [ ] No dosage calculation or inference was added.
- [ ] No treatment recommendation was added.
- [ ] No medication-interaction checker or clinical risk score was added.
- [ ] Medicine strength and instruction text remain opaque user-entered strings.
- [ ] Stock math uses only explicit user-entered stock quantities/change values.
- [ ] Reminder schedule frequency/time/date/cycle values come only from explicit user-entered schedule values.
- [ ] As-needed schedules do not create automatic occurrences.
- [ ] Archived profiles and inactive medicine states do not automatically materialize reminders.
- [ ] Planner ownership checks reject profile/medicine/schedule/persisted schedule-time mismatches before materialization.
- [ ] Unknown schedule kinds and unsupported weekday-mask bits are rejected rather than silently interpreted.
- [ ] Planner windows and coordinator rebuild overrides require UTC timestamps.
- [ ] Snooze requires an explicit future UTC timestamp before persistence or platform scheduling.
- [ ] Snoozed reminders use `SnoozedUntilUtc` as effective due time for upcoming/overdue handling.
- [ ] Existing platform requests are cancelled before reminder replacement, suppression, invalidation, or handled-state transition.
- [ ] Reminder cancellation failures remain retryable instead of falsely reporting reconciliation success.
- [ ] Medicine/profile delete flows reconcile platform requests before database cascade and compensate if persistence later fails.
- [ ] Medicine/profile save flows reconcile platform reminders before non-critical audit bookkeeping can fail the operation.
- [ ] Appointment reminder persistence has compensation/reconciliation coverage.
- [ ] Reminder actions use cancellation-first ordering and restore/rebuild compensation when later persistence/scheduling fails.
- [ ] Appointment `StartsUtc` requires `DateTimeKind.Utc`; local/unspecified clock values are not relabeled as UTC.
- [ ] Appointment notification scheduling stops when permission remains denied.
- [ ] Background appointment rebuild does not repeatedly request notification permission.
- [ ] Invalid daylight-saving local times are not silently replaced with inferred alternative reminder times.
- [ ] Ambiguous daylight-saving times remain deterministic across repeated rebuilds.
- [ ] Reminder delivery limitations remain visible and are not represented as guaranteed.

## Secrets, app lock and cryptography

- [ ] No `.p12`, `.pfx`, `.jks`, keystore, `.env`, service credential, API key or signing secret is committed.
- [ ] App-lock PINs are not stored in plaintext.
- [ ] App-lock PIN verification uses a random salt, PBKDF2-HMAC-SHA256 and fixed-time comparison.
- [ ] App-lock verification clears derived and retrieved verifier byte buffers on verification paths where managed-memory control permits.
- [ ] Disabling app lock removes the enabled flag, salt and verifier from the secret store.
- [ ] App lock is documented as a local privacy barrier and not whole-database/device encryption.
- [ ] New encrypted document payloads use AES-256-GCM chunked framing v2.
- [ ] New encrypted backup payload streams use chunked framing v2.
- [ ] V2 terminal record is authenticated against the next chunk counter/zero length.
- [ ] V2 tests reject chunk-boundary prefix truncation.
- [ ] Encrypted-stream reader rejects trailing bytes after terminal.
- [ ] Legacy framing-v1 decryption remains intentional/documented for compatibility.
- [ ] Existing v1 ciphertext is not represented as retroactively upgraded.
- [ ] New document metadata records encryption stream version 2.
- [ ] Caller-owned document/backup key buffers are cleared after use where practical.
- [ ] Backup password-derived key/salt buffers are cleared after crypto paths where practical.
- [ ] Chunked AEAD work buffers are cleared where managed-memory control permits.
- [ ] Cryptographic keys/passwords are not written to diagnostics.

## Document-vault consistency

- [ ] Database-save failure during document import removes the just-created encrypted payload.
- [ ] Audit failure after document metadata save attempts rollback of both metadata and encrypted payload.
- [ ] Import rollback cleanup does not become intentionally cancelled with the original failed operation.
- [ ] Incomplete rollback is surfaced rather than silently hidden.
- [ ] Explicit document export constrains output to a safe leaf filename.
- [ ] Decrypted temporary exports remain under managed cache ownership until explicitly shared/exported.
- [ ] Application-owned shared report cache files are removed after the share handoff returns where CareNest still owns the temporary copy.
- [ ] Delete of a missing document record remains idempotent.

## Logging and diagnostics

- [ ] `docs/security/LOGGING_PRIVACY.md` is still accurate.
- [ ] Runtime logger calls do not receive full exception objects from user-data operation paths.
- [ ] Exception messages/stack traces are not included in CareNest diagnostic logs.
- [ ] Reminder scheduling/cancellation/recovery failures do not log medicine/profile/occurrence identifiers.
- [ ] Diagnostic exports exclude health-document contents and user-entered sensitive notes.

## Persistence and backup

- [ ] SQLite migrations pass from supported schema states.
- [ ] Foreign-key/cascade cleanup tests pass.
- [ ] WAL mode and busy-timeout regression tests pass.
- [ ] WAL-backed snapshot creation passes.
- [ ] WAL snapshot content test verifies committed records are present in the copied database.
- [ ] Copied WAL snapshot passes SQLite integrity checking.
- [ ] Pre-cancelled snapshot operation leaves no output file.
- [ ] Restore integrity/tamper validation passes.
- [ ] Decrypted backup archive topology is validated before extraction.
- [ ] Duplicate backup entries are rejected.
- [ ] Unexpected/nested/non-`.cndoc` document entries are rejected.
- [ ] Manifest document count must match archive contents.
- [ ] Document-bearing backups require a valid 32-byte document master key.
- [ ] Extraction still enforces destination-root path containment.
- [ ] The repository does not claim whole-database encryption at rest.
- [ ] Representative packaged upgrade/install with fictional pre-remediation SQLite data succeeds.
- [ ] Existing profiles/medicines/schedules/reminders/logs/appointments/documents/stock/tags remain readable after the SQLite native/provider update.
- [ ] Pre-remediation and current encrypted backups restore correctly on representative packaged targets where canonical fixtures are available.
- [ ] Existing encrypted document payloads remain decryptable through the unchanged key path.

## Dependency security

- [ ] CodeQL passes for the exact release commit.
- [ ] Dependency Audit passes for the exact release commit without the former `GHSA-2m69-gcr7-jv3q` suppression.
- [ ] `docs/security/DEPENDENCY_RISK_REGISTER.md` was reviewed.
- [ ] `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` was reviewed.
- [x] `GHSA-2m69-gcr7-jv3q` source remediation was re-evaluated using available compatible native/provider paths.
- [x] `SQLitePCLRaw.lib.e_sqlite3` is centrally pinned at or above `3.53.3` in the current source.
- [x] `SQLitePCLRaw.lib.e_sqlite3.android` and the selected providers are centrally pinned at or above `2.1.12` in the current source.
- [x] The exact advisory `NuGetAuditSuppress` entry was removed rather than broadened.
- [x] `SqliteDependencySecurityContractTests` protects the maintained native/provider floor and absence of the old suppression.
- [x] Unsuppressed Dependency Audit checkpoints succeeded during remediation and the authoritative PR #54 Dependency Audit #35 / `31766059132` succeeded on the final verified source.
- [ ] Final release evidence records the actual resolved dependency graph and manual existing-data compatibility results; it must not confuse a successful audit with packaged data/device validation.

## Platform/distribution

- [ ] Android requested permissions match reminder/file behavior.
- [ ] Apple entitlements/permissions match actual behavior.
- [ ] Windows capabilities match actual behavior.
- [ ] Signing credentials remain outside Git and logs.
- [ ] Current Apple/Google rules for the voluntary external support link were reviewed.
- [ ] Store privacy/data-safety disclosures match local-first behavior.

## Current RC1 automated baseline

The earlier PR #33 reference is historical. The 2026-08-14 audit continued through reminder reconciliation, failure compensation, report-cache cleanup, cancellation-first reminder action recovery, appointment persistence compensation and SQLite dependency remediation.

Authoritative marker-only verification: PR #54 — `Verify final CareNest bug-audit source`.

Verified runtime/test/dependency source/base SHA:

`4490f3f86752841d436e981b29279970c90c947b`

Marker head:

`929168a0a319b15d9e89997d86436d59ae731ad1`

Final automated evidence:

- CareNest CI #503 / `31766059137` — **success**;
- platform-neutral formatting — **success**;
- **122 unit tests** — passed;
- **39 integration tests** — passed;
- **100 UI-contract/policy tests** — passed;
- **261 total core tests** — passed;
- Android Release — **success**;
- Windows Release — **success**;
- iOS simulator Release — **success**;
- Mac Catalyst Release — **success**;
- CodeQL #503 / `31766059215` — **success**;
- Dependency Audit #35 / `31766059132` — **success** without the former SQLite advisory suppression, including the platform-neutral and Android MAUI app graphs.

PR #54 was closed without merge after the complete matrix passed. Its verification marker is not part of `main`. PR #53 is duplicate corroborating same-source evidence and was also closed without merge.

Verification history retained for auditability:

- PR #31 was superseded after CA1861 was exposed in new test source; the analyzer finding was fixed instead of suppressed.
- PR #32 verified the corrected service/document/backup hardening before later AEAD-v2 changes.
- PR #33 was a green 2026-08-13 baseline but is no longer the latest runtime/test source.
- PR #43 was incorrectly described as green; actual CareNest CI #448 failed in integration testing and the UI suite was skipped.
- PR #44 reproduced future-snooze/overdue-snooze/stale-occurrence defects that were subsequently fixed.
- PR #46 exposed broader platform-reminder lifecycle contract failures.
- PR #49 exposed CA1861 in new reminder reconciliation assertions; the tests were corrected rather than suppressing the analyzer.
- PRs #47/#48/#50 provided useful unsuppressed SQLite-audit evidence while `main` was moving, but were not final combined-source baselines.
- PRs #51/#52 were superseded when later runtime/test source changed.
- PR #53 independently corroborated the final graph but was closed as duplicate after PR #54 completed the full matrix.

`docs/releases/FINAL_BUG_AUDIT_VERIFICATION_20260814.md` is the authoritative automated evidence record for this audit.

This automated reference still does not pre-approve a later production commit or complete the manual/device/accessibility/store/signing/SQLite-existing-data compatibility gates.

## Approval record

```text
Version:
Commit SHA:
Reviewer:
Review date:
CareNest CI run:
CodeQL run:
Dependency Audit run:
Release Evidence run:
Chunked AEAD framing decision:
Legacy v1 compatibility decision:
SQLite dependency source-remediation decision:
SQLite packaged existing-data compatibility decision:
Open security blockers:
Approved for signing/package creation: yes/no
Notes:
```
