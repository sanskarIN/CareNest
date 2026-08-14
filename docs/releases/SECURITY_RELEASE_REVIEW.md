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

- [ ] CodeQL passes for the exact production release commit/tag.
- [ ] Dependency Audit passes for the exact production release commit/tag without the former `GHSA-2m69-gcr7-jv3q` suppression.
- [ ] `docs/security/DEPENDENCY_RISK_REGISTER.md` was reviewed.
- [ ] `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` was reviewed.
- [x] `GHSA-2m69-gcr7-jv3q` source remediation was re-evaluated using available compatible native/provider paths.
- [x] `SQLitePCLRaw.lib.e_sqlite3` is centrally pinned at or above `3.53.3` in the current source.
- [x] `SQLitePCLRaw.lib.e_sqlite3.android` and the selected providers are centrally pinned at or above `2.1.12` in the current source.
- [x] The exact advisory `NuGetAuditSuppress` entry was removed rather than broadened.
- [x] `SqliteDependencySecurityContractTests` protects the maintained native/provider floor and absence of the old suppression.
- [x] Authoritative PR #56 Dependency Audit #41 / `31770929383` succeeded on the fully verified release-engineering source.
- [ ] Final production Release Evidence records the actual resolved dependency graph and manual existing-data compatibility results; it must not confuse a successful audit with packaged data/device validation.

## Release-engineering security controls

- [x] CareNest CI, CodeQL and Dependency Audit support exact `v*` release-tag execution.
- [x] Dependency Audit guards pull-request-only metadata from manual/tag runs.
- [x] Release Gate and Release Evidence support exact `v*` tag and manual execution.
- [x] Release Gate detects open dependency-risk status without indentation/case bypass.
- [x] Release Gate detects nested unchecked release-checklist items.
- [x] Release Evidence records commit/ref/run/attempt identity and tracked-source SHA-256 provenance.
- [x] Release Evidence attempts all core evidence components and uploads available evidence before its aggregate failure gate.
- [x] Release Evidence artifact identity includes commit SHA + run ID + run attempt.
- [x] Release-preflight/quality-gate scripts treat unsuppressed dependency audit failures as blocking.
- [x] Git setup scripts use verified repository-local `Sanskar` / `sanskarin@outlook.in` identity and fail on native Git errors.
- [x] Executable UI-contract tests guard release workflow/script/security policy.
- [ ] Exact final production `v*` tag completes CareNest CI, CodeQL, Dependency Audit, Release Gate and Release Evidence successfully after all manual/store/signing blockers are resolved.

## Platform/distribution

- [ ] Android requested permissions match reminder/file behavior.
- [ ] Apple entitlements/permissions match actual behavior.
- [ ] Windows capabilities match actual behavior.
- [ ] Signing credentials remain outside Git and logs.
- [ ] Current Apple/Google rules for the voluntary external support link were reviewed.
- [ ] Store privacy/data-safety disclosures match local-first behavior.

## Current RC1 automated baseline

Authoritative marker-only verification: PR #56 — `Verify complete CareNest release-engineering source`.

Frozen source/base SHA:

`4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`

Marker head:

`e3bc621cea05364a69abee0dadbd71a67c17bddb`

Final automated evidence:

- CareNest CI #571 / `31770929379` — **success**;
- platform-neutral formatting — **success**;
- **122 unit tests** — passed;
- **39 integration tests** — passed;
- **124 UI-contract/policy tests** — passed;
- **285 total core tests** — passed;
- Android Release — **success**;
- Windows Release — **success**;
- iOS simulator Release — **success**;
- Mac Catalyst Release — **success**;
- CodeQL #571 / `31770929382` — **success**;
- Dependency Audit #41 / `31770929383` — **success** without the former SQLite advisory suppression.

PR #56 was closed without merge after the complete matrix passed. Its verification marker is not part of `main`.

PR #54 remains the historical authoritative runtime bug-audit baseline. PR #55 is a superseded but useful release-engineering checkpoint that passed 277/277 core tests, Android, Windows, CodeQL and unsuppressed Dependency Audit before additional confirmed release-tooling/documentation fixes required PR #56.

`docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md` is the authoritative automated evidence record for the current release-engineering source.

This automated reference still does not pre-approve a later production commit/tag or complete the manual/device/accessibility/store/signing/SQLite-existing-data compatibility gates.

## Approval record

```text
Version:
Commit SHA:
Tag:
Reviewer:
Review date:
CareNest CI run:
CodeQL run:
Dependency Audit run:
Release Gate run:
Release Evidence run:
Release Evidence artifact:
Chunked AEAD framing decision:
Legacy v1 compatibility decision:
SQLite dependency source-remediation decision:
SQLite packaged existing-data compatibility decision:
Open security blockers:
Approved for signing/package creation: yes/no
Approved for publication: yes/no
Notes:
```
