# CareNest Security Architecture Reference

This document provides a single technical view of CareNest v1 security controls and limitations. It complements `SECURITY.md`, `docs/security/THREAT_MODEL.md`, `docs/security/LOGGING_PRIVACY.md`, and `docs/security/DEPENDENCY_RISK_REGISTER.md`.

## Security objective

CareNest is designed as a local-first organizer with strong separation between structured local records, encrypted document payloads, manual encrypted backups, optional app-lock access control, privacy-minimized diagnostics, and explicit outbound user actions.

The application is not a medical device security system and does not claim protection against a fully compromised operating system/device.

## Trust boundaries

Primary trusted components:

- CareNest application process;
- operating-system application sandbox;
- platform secure-secret storage;
- local filesystem areas assigned to the application;
- platform notification/file/share APIs used after explicit application actions.

External/untrusted-or-separate boundaries include:

- user-selected export destinations;
- calendar providers;
- browsers/web destinations;
- cloud drives chosen by the user;
- public GitHub/support systems;
- store/distribution systems;
- compromised/rooted/jailbroken environments.

## SQLite protection

Structured records live in local SQLite storage.

Current security statement:

- database is protected primarily by application sandbox/device security;
- CareNest does not claim transparent whole-database encryption;
- SQL/repository access is kept in infrastructure rather than UI;
- migrations are versioned and migration/version writes are transactionally coordinated;
- integrity tests cover persistence behavior;
- WAL mode and busy-timeout configuration are regression tested;
- backup snapshot tests validate copied committed content and `PRAGMA integrity_check`;
- native/provider package changes are treated as both dependency-security work and data-compatibility work.

## Encrypted document protection

Imported document payloads use authenticated encryption with .NET cryptographic primitives.

Design properties:

- document payloads are separate from structured metadata;
- a per-installation random 32-byte encryption key is stored through platform secure storage;
- new payloads use chunked AES-256-GCM framing version 2;
- each chunk authenticates counter and length through AAD;
- v2 includes an authenticated terminal record bound to the next chunk counter;
- trailing data after the terminal is rejected;
- legacy framing v1 remains readable so existing local files are not made inaccessible;
- encrypted document round-trip/tamper/truncation/trailing-data tests are part of the integration suite;
- decrypted/exported copies leave the CareNest vault boundary after explicit export/share.

Security limitation: v2 does not retroactively rewrite or strengthen already-existing v1 ciphertext. Legacy v1 read compatibility remains a deliberate compatibility tradeoff until an explicit migration policy exists.

## Document import consistency

Document import spans two local persistence surfaces: encrypted filesystem payload and SQLite metadata/audit state.

Controls:

- encrypted payload is created before metadata is committed;
- database-save failure removes the new encrypted payload;
- audit failure after metadata save attempts to remove both the metadata record and encrypted payload;
- rollback cleanup is not cancelled merely because the original user operation has become cancelled;
- incomplete rollback is surfaced as an aggregate failure rather than silently hidden.

This is compensating cleanup, not a claim of a single cross-filesystem/SQLite ACID transaction. Process termination or OS failure can still interrupt cleanup and therefore remains a manual/recovery consideration.

## Plaintext export/cache lifecycle

Explicit document/report export necessarily creates plaintext or portable output.

Current controls include:

- failed decrypted document export cleans application-owned incomplete/plaintext output best effort;
- successful decrypted temporary document exports stay under the managed `Exports` cache until explicitly shared/exported;
- CSV/PDF/JSON writers use staged partial files and atomic final moves;
- failed/cancelled report generation removes incomplete staging files best effort;
- shared report flows remove the application-owned temporary report after share handoff returns where CareNest still owns that copy;
- CareNest does not claim deletion of copies already controlled by another app, cloud destination, OS share service, screenshot, backup, or filesystem snapshot.

CSV string output also neutralizes formula-like user-entered prefixes in the portable spreadsheet representation without mutating the stored source record.

## Backup protection

Manual backups use:

- user password;
- PBKDF2-HMAC-SHA256 password-based key derivation;
- random salt;
- AES-256-GCM authenticated chunk encryption;
- versioned application package metadata;
- chunked AEAD framing v2 for new encrypted payloads;
- wrong-password/tamper/truncation/trailing-data rejection;
- protected document-recovery key material inside the encrypted payload;
- strict decrypted ZIP topology validation before extraction.

Allowed archive files are limited to the manifest, database, optional/required document key, and top-level `.cndoc` document entries. Duplicate, nested, unexpected, count-mismatched, or invalid-key layouts fail validation.

The backup password is not recoverable through a CareNest backend because no such backend exists in v1.

Primary encrypted backup/restore completion is distinguished from later local bookkeeping. A later audit/history write failure does not falsely turn a completely written backup or fully committed restore into an unsuccessful cryptographic operation.

## Sensitive buffer handling

Where application code owns mutable key-material buffers, CareNest clears them with `CryptographicOperations.ZeroMemory` after use where practical.

Current examples include:

- app-lock derived/retrieved verifier buffers;
- caller-owned document-master-key copies after import/export;
- generated document key if secret-store persistence fails;
- backup password-derived AES key and salt;
- copied document master keys during backup creation/restore;
- chunked AEAD plaintext/ciphertext/tag/nonce/AAD working buffers.

Limitations:

- managed-memory zeroing reduces lifetime of known caller-owned buffers;
- it does not prove erasure of copies inside the runtime, OS, platform secure store, crash dumps, swap, hardware, or a compromised process/device.

## App-lock protection

The optional app lock uses:

- numeric PIN policy;
- random salt;
- PBKDF2-HMAC-SHA256 verifier derivation;
- fixed-time verifier comparison;
- platform secure secret storage for enabled/salt/verifier material;
- clearing candidate/retrieved verifier buffers where managed-memory control permits;
- removal of stored lock material when disabled;
- rollback around multi-key update/disable transitions;
- fail-closed behavior when stored salt/verifier material is missing or malformed.

Limitations:

- no plaintext PIN is intentionally persisted;
- numeric PIN entropy depends on user choice;
- app lock is not whole-database encryption;
- a compromised secure store/OS can defeat the intended boundary;
- app lock does not replace device-level authentication/security.

## Notification protection and time integrity

CareNest minimizes notification payload sensitivity.

- generic labels are used by default;
- document contents/private free-text are not intended in notification requests;
- platform notification systems control final storage/display/delivery;
- user device lock-screen preview settings remain important.

Time/permission integrity controls include:

- appointment `StartsUtc` must be actual `DateTimeKind.Utc`;
- local/unspecified appointment ticks are rejected instead of silently relabeled;
- denied notification permission is not treated as successful appointment scheduling;
- background/rebuild paths do not repeatedly prompt and do not schedule while permission remains denied;
- medicine planner/rebuild boundaries require explicit UTC transport timestamps;
- snooze requires an explicit future UTC timestamp.

## Reminder integrity protections

Reminder planning and platform-state integrity are separate but coordinated concerns.

Planning controls include:

- explicit entity-ownership validation;
- known schedule kind;
- valid explicit time-zone identifier;
- UTC planning-window validation;
- half-open window semantics;
- deterministic occurrence keys;
- duplicate-time deduplication;
- deterministic DST overlap handling;
- no invented DST-gap replacement time;
- archived/paused/completed/disabled/as-needed suppression rules;
- explicit future-UTC snooze requirement.

Platform reconciliation controls include:

- snoozed occurrences use `SnoozedUntilUtc` as their effective due time;
- rebuild attempts cancellation of an existing OS request before replacement, quiet-hour suppression, or invalidation;
- cancellation failure leaves state retryable instead of falsely reconciled;
- schedule edits retain enough old occurrence identity to cancel stale platform requests;
- medicine/profile deletion cancels future platform requests before database cascade and attempts non-cancelled rebuild compensation if persistence fails;
- medicine/profile save flows reconcile reminders before later non-critical audit bookkeeping;
- appointment persistence/platform scheduling uses compensation/reconciliation rather than assuming database and OS scheduler are one transaction.

Handled reminder actions are cancellation-first:

1. cancel the old platform request;
2. persist Taken/Skipped/Delayed/Missed/Snoozed/Cancelled only after cancellation succeeds;
3. schedule a snooze replacement only after state persistence;
4. if a later persistence/scheduling step fails, attempt non-cancelled restoration of the previous occurrence state and reminder rebuild;
5. if recovery also fails, surface aggregate failure rather than claiming consistency.

These controls protect organizational data integrity. They do not validate clinical appropriateness or guarantee OS delivery.

## Logging protection

Runtime diagnostic logging is intentionally restricted.

The codebase uses source contracts and explicit logging-level guards to avoid:

- full exception-object logging from user-data operation paths;
- raw exception messages/stack traces;
- medicine/profile/reminder record identifiers in reminder scheduling/cancellation/recovery failures;
- document contents;
- credentials/PINs/backup passwords/keys.

See `docs/security/LOGGING_PRIVACY.md`.

## Global exception observation

CareNest attaches privacy-aware global exception observation at startup.

The handler:

- attaches once;
- observes supported application-domain/unobserved-task exceptions;
- logs only safe exception type/category metadata when the level is enabled;
- marks unobserved task exceptions observed after safe handling.

It is not intended to serialize private application state for remote telemetry.

## Local-first network boundary

Current runtime policy tests protect against accidental addition of network/telemetry clients to the local-first v1 scope.

A future HTTP/gRPC/sync/analytics subsystem requires explicit review rather than being introduced as an incidental dependency.

## External browser/support boundary

Fixed project-support destination:

`https://buymeacoffee.com/sanskarIN`

The application should open it only after explicit user action and without appending health/profile/document/reminder identifiers.

External store policy is reviewed separately before distribution.

## Secret management

Repository rules prohibit committing common secret/signing material.

Never commit:

- Android keystore/private keys;
- Apple signing certificates/private keys/provisioning secrets;
- Windows signing private keys;
- API/service credentials;
- real app-lock PINs;
- backup passwords;
- encryption keys;
- production `.env` secrets.

Signing/configuration secrets belong outside Git.

## Dependency security

NuGet dependency auditing is a blocking repository/release control.

The formerly tracked SQLite native dependency exception for `GHSA-2m69-gcr7-jv3q` is remediated in the current RC1 source graph:

- central transitive pinning remains enabled;
- `sqlite-net-pcl` remains `1.9.172`;
- `SQLitePCLRaw.bundle_green` remains `2.1.11` as the compatible bundle API path;
- `SQLitePCLRaw.lib.e_sqlite3` is pinned to `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` is pinned to `2.1.12`;
- selected provider packages are pinned to `2.1.12`;
- the exact `GHSA-2m69-gcr7-jv3q` `NuGetAuditSuppress` entry has been removed;
- `SqliteDependencySecurityContractTests` prevents restoration of the old native/provider floor or suppression.

This source remediation was verified by unsuppressed Dependency Audit and the full platform/core matrix on PR #54, and again through later release-engineering checkpoints. The security decision is not permission to skip packaged existing-database, encrypted-document, backup, reminder, and real-device compatibility checks after changing native persistence dependencies.

Any future audit exception must be exact, temporary, documented in `DEPENDENCY_RISK_REGISTER.md`, and reviewed as a release risk. Wildcard/severity-wide suppression is prohibited.

## Static and automated security controls

Repository automation includes:

- CodeQL;
- unsuppressed Dependency Audit;
- architecture contracts;
- repository policy contracts;
- no common signing-secret file contracts;
- no runtime network/telemetry client policy;
- logging privacy source contracts;
- app-lock cryptographic source contracts;
- direct application-service tests;
- backup/document encryption integration tests;
- strict backup topology tests;
- chunked AEAD v2 truncation/trailing-data/legacy-v1 tests;
- sensitive caller-buffer hygiene tests;
- SQLite migration/integrity tests;
- SQLite dependency-security contracts;
- reminder/appointment ownership/time/state/permission/reconciliation contracts;
- release workflow/preflight/quality-gate contracts;
- warnings-as-errors CI posture for correctness/security analyzers except explicitly documented advisory exceptions.

## Source hygiene

Committed runtime source policy rejects implementation placeholders such as TODO/FIXME/`NotImplementedException` patterns covered by the repository tests.

Generated `bin`/`obj` content is excluded from committed-source policy scans.

## Async/cancellation safety

Runtime source avoids common synchronous task-blocking patterns.

Cancellation-aware operations are used where appropriate for I/O/application workflows.

Security-sensitive cleanup/reconciliation sometimes intentionally uses non-cancelled compensation after the main operation has failed so cancellation cannot knowingly strand newly created encrypted payloads/metadata or leave a known cancelled reminder request without a best-effort restore attempt.

This improves reliability and consistency but is not itself a confidentiality guarantee against process termination.

## Backup/restore attack considerations

Threats include:

- tampered backup;
- wrong password;
- encrypted-stream prefix truncation;
- trailing data;
- malicious/unsupported format version;
- duplicate/unexpected/nested archive entries;
- manifest/document-count mismatch;
- invalid/missing document key;
- partial/corrupt SQLite snapshot;
- leaked backup file/password;
- insecure destination.

Controls include authenticated encryption, v2 authenticated stream termination for new writes, strict archive topology validation, format/version validation, snapshot integrity checks, rollback handling, buffer hygiene, and manual release restore testing.

## Export attack considerations

Exports intentionally create copies outside the CareNest protected boundary.

Risks:

- plaintext CSV/PDF/JSON exposure;
- decrypted document export exposure;
- insecure share destination;
- cloud synchronization by destination app;
- retained historical copies after local deletion.

Mitigation is explicit user action plus clear privacy documentation; CareNest cannot remotely recall exported copies.

## Physical/device compromise

Residual risks outside CareNest's guarantee include:

- unlocked device access;
- rooted/jailbroken device;
- malicious overlay/accessibility tooling;
- OS compromise;
- memory/process inspection;
- screenshots/screen recording;
- compromised secure storage;
- device/OS backups.

Device-level encryption, secure lock screen, OS updates, and trusted software remain part of the overall security posture.

## Automated security baselines

### Authoritative bug-audit baseline

Marker-only PR #54 verified the 2026-08-14 runtime/test/dependency graph:

- CareNest CI #503 / `31766059137`: success;
- 122 unit + 39 integration + 100 UI-contract = 261 tests passed;
- Android/Windows/iOS simulator/Mac Catalyst Release builds: success;
- CodeQL #503 / `31766059215`: success;
- unsuppressed Dependency Audit #35 / `31766059132`: success.

PR #54 was closed without merge after evidence capture.

### Release-engineering hardening after PR #54

Later workflow/test/build-script changes added exact `v*` tag triggers, failure-preserving release evidence, blocking local dependency audits, and executable release-policy contracts. These changes are verification-relevant even though they do not intentionally change the application runtime.

PR #55 was a successful but superseded checkpoint: formatting, 122 unit tests, 39 integration tests, 116 UI-contract tests, Android, Windows, CodeQL #547 / `31769940053`, and unsuppressed Dependency Audit #38 / `31769940039` succeeded before further confirmed release-tooling/documentation corrections required a newer exact-source verification.

The final current `main` head must receive a fresh complete exact-source matrix before it replaces these historical checkpoints as the release-engineering baseline.

## Exact-tag release security behavior

Tags matching `v*` are configured to run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Release Evidence records source/ref/run identity, tracked-source manifests/checksums, all three core TRX suites, dependency inventories, workspace integrity, and evidence checksums. It uploads available evidence even when a component fails, then applies an aggregate failure gate.

A tag is not a production approval until every required tag workflow plus manual/device/accessibility/store/signing/packaged-data compatibility evidence is complete.

## Security release review

Before final public promotion:

- rerun the complete exact-source matrix for the intended production commit/tag;
- rerun CodeQL/unsuppressed Dependency Audit for exact promoted source;
- review threat model;
- review logging privacy;
- review v1/v2 encrypted-stream compatibility plan;
- review strict backup topology assumptions;
- review dependency risk/remediation state;
- complete packaged SQLite existing-data/encrypted-data compatibility evidence;
- complete `docs/releases/SECURITY_RELEASE_REVIEW.md`;
- verify no real secrets were committed;
- verify signed artifacts come from exact reviewed source;
- verify store/privacy disclosures match runtime behavior;
- manually inspect logs and export/backup/reminder workflows on target devices.

## Incident response

Security reports should use the process in `SECURITY.md`.

Do not request real health data by default. Prefer synthetic reproduction inputs and sanitized diagnostics.

## Future networked features

Any future sync/account/remote caregiver feature requires at minimum:

- authentication design;
- authorization/consent/revocation model;
- encryption/key ownership;
- network endpoint security;
- server retention/deletion/export;
- abuse/threat analysis;
- conflict recovery;
- device revocation;
- privacy/store disclosure changes;
- incident-response expansion;
- dedicated automated security tests.

Those features are not implicitly covered by the current local-first security model.
