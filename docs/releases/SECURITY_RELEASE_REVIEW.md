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
- [ ] Delete of a missing document record remains idempotent.

## Logging and diagnostics

- [ ] `docs/security/LOGGING_PRIVACY.md` is still accurate.
- [ ] Runtime logger calls do not receive full exception objects from user-data operation paths.
- [ ] Exception messages/stack traces are not included in CareNest diagnostic logs.
- [ ] Reminder scheduling failures do not log medicine/occurrence identifiers.
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

## Dependency security

- [ ] CodeQL passes for the exact commit.
- [ ] Dependency audit passes for the exact commit.
- [ ] `docs/security/DEPENDENCY_RISK_REGISTER.md` was reviewed.
- [ ] `GHSA-2m69-gcr7-jv3q` status was re-evaluated using available compatible packages/provider paths.
- [ ] The exact advisory suppression was not broadened.
- [ ] The release record states the real advisory decision; it does not call a suppression a fix.

## Platform/distribution

- [ ] Android requested permissions match reminder/file behavior.
- [ ] Apple entitlements/permissions match actual behavior.
- [ ] Windows capabilities match actual behavior.
- [ ] Signing credentials remain outside Git and logs.
- [ ] Current Apple/Google rules for the voluntary external support link were reviewed.
- [ ] Store privacy/data-safety disclosures match local-first behavior.

## Current RC1 automated reference

Latest exact automated runtime/test baseline:

`4f5f9abe9d702fa33d6aba3f15c113febfebf95e`

Marker-only PR #33 used marker head `62a0050a2622e12a31d00842778af0bc96355482` and was closed without merge after success.

Evidence:

- CareNest CI #332 / `31691592300` — success;
- platform-neutral formatting — success;
- 106 unit tests — passed;
- 30 integration tests — passed;
- 54 UI-contract/policy tests — passed;
- **190 total core tests — passed**;
- Android Release — success;
- Windows Release — success;
- iOS simulator Release — success;
- Mac Catalyst Release — success;
- CodeQL #332 / `31691592435` — success;
- Dependency Audit #13 / `31691592302` — success.

Verification history retained for auditability:

- PR #31 was superseded after CA1861 was exposed in new test source; the analyzer finding was fixed instead of suppressed.
- PR #32 verified the corrected service/document/backup hardening at 186 tests before the later AEAD-v2 source changes required PR #33.

This reference does not pre-approve a later production commit and does not resolve the open SQLite dependency risk or manual/distribution gates.

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
SQLite advisory decision:
Open security blockers:
Approved for signing/package creation: yes/no
Notes:
```
