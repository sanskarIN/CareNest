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
- [ ] Invalid daylight-saving local times are not silently replaced with inferred alternative reminder times.
- [ ] Reminder delivery limitations remain visible and are not represented as guaranteed.

## Secrets, app lock and cryptography

- [ ] No `.p12`, `.pfx`, `.jks`, keystore, `.env`, service credential, API key or signing secret is committed.
- [ ] App-lock PINs are not stored in plaintext.
- [ ] App-lock PIN verification uses a random salt, PBKDF2-HMAC-SHA256 and fixed-time comparison.
- [ ] App-lock verification clears derived and retrieved verifier byte buffers on verification paths where managed-memory control permits.
- [ ] Disabling app lock removes the enabled flag, salt and verifier from the secret store.
- [ ] App lock is documented as a local privacy barrier and not whole-database/device encryption.
- [ ] Encrypted document storage still uses authenticated platform-supported .NET cryptography.
- [ ] Backup encryption/tamper/wrong-password tests pass.
- [ ] Cryptographic keys/passwords are not written to diagnostics.

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

For comparison during the next final-release review, source head `69c4dd9319f7dc47edea1786e683f7d90c656e1e` passed PR #28 with CareNest CI #220 / `31378000135`, CodeQL #220 / `31378000143`, Dependency Audit #8 / `31378000134`, formatting, 37 unit tests, 13 integration tests, 51 UI-contract tests, and Android/Windows/iOS-simulator/Mac-Catalyst Release builds. This reference does not pre-approve a later production commit.

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
SQLite advisory decision:
Open security blockers:
Approved for signing/package creation: yes/no
Notes:
```
