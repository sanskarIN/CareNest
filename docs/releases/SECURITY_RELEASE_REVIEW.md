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

For comparison during the next final-release review, source head `c61f3c31c4ba33419c7b348fc8ee63a58eaa637b` passed marker-only PR #30 with:

- CareNest CI #248 / `31382194805` — success;
- platform-neutral formatting — success;
- 74 unit tests — passed;
- 13 integration tests — passed;
- 54 UI-contract/policy tests — passed;
- 141 total core tests — passed;
- Android Release — success;
- Windows Release — success;
- iOS simulator Release — success;
- Mac Catalyst Release — success;
- CodeQL #248 / `31382194687` — success;
- Dependency Audit #10 / `31382194683` — success.

PR #29 / CI #246 is retained as a superseded failure record because it exposed CA2263 in a new non-generic `Enum.IsDefined` call. The source was fixed on `main` and reverified through PR #30 rather than weakening the analyzer policy. This reference does not pre-approve a later production commit and does not resolve the open SQLite dependency risk or manual/distribution gates.

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
