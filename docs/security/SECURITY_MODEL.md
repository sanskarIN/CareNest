# CareNest Security Architecture Reference

This document is the current technical security reference for CareNest `1.0.0-rc.1`. It complements `SECURITY.md`, `THREAT_MODEL.md`, `LOGGING_PRIVACY.md`, `DEPENDENCY_RISK_REGISTER.md`, the privacy model, and the release security review.

CareNest is a local-first organizational health app. It does not claim protection against a fully compromised device/OS and does not provide clinical decision support.

## Current authoritative automated security baseline

Marker-only PR #56 is the current release-engineering source baseline.

- frozen source/base: `4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`;
- marker head: `e3bc621cea05364a69abee0dadbd71a67c17bddb`;
- CareNest CI #571 / `31770929379`: success;
- 122 unit + 39 integration + 124 UI-contract/policy = **285/285** core tests;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #571 / `31770929382`: success;
- unsuppressed Dependency Audit #41 / `31770929383`: success.

PR #56 was closed without merge; its verification marker is not part of `main`.

PR #54 remains the historical authoritative runtime bug-audit checkpoint for the earlier 261-test source boundary. PR #55 is a superseded intermediate release-engineering checkpoint and is not the current baseline.

See `docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md`.

## Security objectives

CareNest aims to:

- minimize unnecessary data movement;
- keep structured records local to the app/device in v1;
- encrypt imported document payloads;
- encrypt manual backups;
- protect app-lock/document key material using platform secure storage;
- avoid sensitive application logging;
- reconcile cross-surface reminder state conservatively;
- fail closed when cryptographic/key state is invalid;
- validate backup topology before extraction;
- treat dependency and release evidence as blocking controls;
- preserve historical compatibility instead of silently making old encrypted data unreadable.

## Trust boundaries

### Inside the primary CareNest trust boundary

- CareNest application process;
- app sandbox/private files;
- local SQLite database;
- encrypted document payload directory;
- app-owned cache/staging files while CareNest still owns them;
- platform secure-storage APIs used for application secret material;
- application repository/services and validated configuration.

### Separate/external boundaries

- operating-system notification subsystem;
- operating-system calendar/file picker/share/browser services;
- user-selected external files/directories/cloud drives;
- browsers and external websites;
- store/distribution services;
- device backups/snapshots;
- rooted/jailbroken/compromised device or equivalent-privilege malware.

A copy handed to another application or destination is outside CareNest’s control.

## Local-first network boundary

Current v1 requires no CareNest account or CareNest-owned backend.

Repository policy protects against accidental runtime introduction of network/telemetry clients in the local-first v1 scope.

A future HTTP/gRPC/account/sync/analytics subsystem requires explicit architecture, privacy, authentication, consent, deletion/export, abuse and threat-model review before implementation.

## SQLite structured-data protection

Structured records live in local SQLite storage.

Current security statement:

- SQLite is protected primarily by the application sandbox/device security;
- CareNest does **not** claim transparent whole-database encryption;
- UI/ViewModels do not issue SQL directly;
- repository/persistence logic is isolated to infrastructure;
- migrations are ordered/versioned;
- migration/version writes are transactionally coordinated;
- WAL mode and busy timeout are regression tested;
- backup snapshots validate committed content and `PRAGMA integrity_check`;
- multi-step repository operations use transaction boundaries where appropriate.

Device compromise or raw private-file extraction can expose structured SQLite content because whole-database encryption is not claimed.

## SQLite dependency security

The formerly tracked `GHSA-2m69-gcr7-jv3q` source exception is remediated in the current verified graph.

Current package intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11` as the compatible bundle/API path;
- central transitive pinning enabled;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- selected SQLitePCLRaw providers `2.1.12`;
- no former exact advisory `NuGetAuditSuppress` entry.

`SqliteDependencySecurityContractTests` prevents restoration of the old vulnerable native/provider floor or old suppression.

Dependency audit is blocking in repository/release workflows and local release/quality scripts.

Security-clean dependency resolution does not prove packaged existing-database/encrypted-data compatibility. Native/provider package changes require both security evidence and representative packaged data-compatibility evidence.

## Encrypted document protection

Imported document payloads are encrypted separately from normal structured metadata.

Current properties:

- random 32-byte document master key;
- platform secure storage for that key;
- AES-256-GCM authenticated encryption;
- chunked AEAD framing v2 for new writes;
- each data record authenticated with counter/length-bound associated data;
- v2 authenticated terminal record bound to the next chunk counter;
- trailing bytes after terminal rejected;
- legacy framing v1 remains readable for compatibility;
- tamper/truncation/trailing-data integration tests;
- key-copy buffers cleared where application-owned mutable arrays exist.

V2 does not retroactively strengthen already-existing v1 ciphertext. Retained v1 read support is a compatibility decision.

## Document-key failure behavior

If encrypted payloads already exist and the document master key is missing/corrupt, CareNest fails closed.

It does not silently create an unrelated new key and then pretend existing ciphertext is recoverable.

A newly generated key buffer is cleared if secure-store persistence fails.

## Document import consistency

Document import spans encrypted filesystem payload plus SQLite metadata/audit state; these are not one ACID transaction.

Current compensation behavior:

1. create encrypted payload;
2. save metadata;
3. save audit state;
4. if database save fails, remove the new encrypted payload;
5. if later audit fails after metadata save, attempt removal of metadata and encrypted payload;
6. cleanup compensation is not abandoned merely because the original caller cancellation token is cancelled;
7. incomplete rollback is surfaced rather than silently hidden.

Abrupt process/OS termination can still interrupt compensation.

## Plaintext export/cache lifecycle

Explicit decrypted/exported copies are plaintext or portable user-controlled output.

Controls include:

- failed decrypted document export attempts cleanup of app-owned incomplete/plaintext output;
- successful temporary decrypted output remains in the managed export/cache location until explicitly used/shared;
- report writers use staged partial files plus atomic final move;
- failed/cancelled report generation removes incomplete staging best effort;
- shared application-owned report cache is removed after share handoff returns where CareNest still owns it.

CareNest cannot delete copies already controlled by another app, cloud provider, share service, screenshot, device backup or filesystem snapshot.

## Backup protection

Manual backup uses:

- user password;
- PBKDF2-HMAC-SHA256 password derivation;
- random salt;
- AES-256-GCM authenticated encryption;
- chunked AEAD framing v2 for new writes;
- versioned package metadata;
- protected document-recovery key material when required;
- strict decrypted ZIP topology validation before extraction;
- wrong-password/tamper/truncation/trailing-data rejection;
- database snapshot/integrity checks.

Allowed decrypted archive topology is explicitly validated; duplicate, nested, unexpected, count-mismatched or invalid-key layouts fail.

Primary backup/restore completion is distinguished from later best-effort local audit/history bookkeeping.

## Restore rollback/key integrity

A failed restore attempts to preserve/restore the exact prior secure-store document key bytes where previous key material existed.

Restore does not treat a partially applied key/database/filesystem state as successful.

Process/OS termination can still interrupt compensation and is part of the residual risk model.

## Sensitive buffer handling

Where CareNest owns mutable arrays containing sensitive cryptographic material, code clears them with `CryptographicOperations.ZeroMemory` where practical.

Examples include:

- candidate/derived/retrieved app-lock verifier buffers;
- document master-key copies;
- generated document key after failed persistence;
- backup password-derived key/salt;
- copied document keys used by backup/restore;
- chunked AEAD plaintext/ciphertext/tag/nonce/AAD working buffers.

This reduces lifetime of known managed buffers; it is not a guarantee that the runtime, OS, secure store, swap, hardware, crash dump or compromised process contains no copies.

## App-lock protection

Optional app lock uses:

- numeric PIN policy;
- random salt;
- PBKDF2-HMAC-SHA256 verifier derivation;
- fixed-time comparison;
- platform secure storage for enabled/salt/verifier material;
- exact salt/verifier shape validation;
- verifier-buffer clearing where practical;
- rollback across multi-key update/disable transitions;
- fail-closed missing/corrupt material;
- removal of lock material when disabled.

App lock is a local privacy barrier. It is not whole-database encryption, device encryption, biometric security or protection against a compromised OS/secure store.

## Notification privacy

Default notification labels are intentionally generic.

Notification requests should not contain document contents, private health notes, passwords, PINs or keys.

The platform/OS controls final notification storage/display/history and lock-screen previews.

## Time and permission integrity

Current scheduling integrity includes:

- appointment `StartsUtc` must be actual `DateTimeKind.Utc`;
- local/unspecified appointment ticks are rejected rather than relabeled;
- denied notification permission is not treated as successful scheduling;
- background rebuild does not repeatedly prompt for permission;
- reminder planner/rebuild transport timestamps require UTC;
- snooze requires an explicit future UTC timestamp.

## Reminder planning security/integrity

Reminder planning is deterministic organizational logic, not clinical inference.

Controls include:

- entity ownership validation;
- recognized schedule kind;
- explicit valid time zone;
- UTC planning-window validation;
- half-open windows;
- deterministic occurrence keys;
- duplicate time deduplication;
- deterministic DST overlap handling;
- no invented DST-gap replacement time;
- archived/paused/completed/disabled/as-needed suppression rules;
- explicit future-UTC snooze requirement.

## Persisted reminder state ↔ OS request integrity

SQLite reminder state and OS scheduled requests are separate surfaces.

Current reconciliation controls:

- `SnoozedUntilUtc` is the effective due time for a valid snooze;
- existing OS request is cancelled before replacement, quiet-hour suppression or invalidation;
- cancellation failure leaves state retryable;
- schedule edits retain old occurrence identity long enough to cancel stale requests;
- medicine/profile deletion cancels future platform requests before database cascade;
- failed database cascade after cancellation triggers non-cancelled rebuild compensation where records still exist;
- medicine/profile save flows reconcile reminders before non-critical audit bookkeeping;
- appointment persistence/platform scheduling uses compensation.

## Handled reminder action integrity

Taken, Skipped, Delayed, Missed, Snoozed and Cancelled use cancellation-first ordering:

1. validate action/snooze input;
2. cancel old platform request;
3. only after cancellation succeeds, persist handled state;
4. for snooze, schedule replacement after state persistence;
5. if later essential persistence/scheduling fails, attempt non-cancelled previous-state restoration and reminder rebuild;
6. surface aggregate recovery failure rather than claiming consistency.

Post-success audit/stock bookkeeping is not allowed to falsify an already completed action.

## Logging protection

Sensitive runtime logging is constrained by source contracts and explicit logging-level guards.

Normal sensitive-path logs must not contain:

- raw health notes/medicine instructions;
- document contents;
- backup contents/passwords;
- app-lock PINs;
- encryption keys;
- raw sensitive exception messages/stack traces;
- record identifiers where not required.

Safe operational category and exception type name can be used where needed.

See `LOGGING_PRIVACY.md`.

## Global exception observation

CareNest registers privacy-aware global/unobserved exception observation at startup.

It records only safe category/type metadata where enabled and does not introduce remote telemetry or serialize private application state.

## Backup/restore attack considerations

Threats include:

- stolen encrypted backup;
- weak password;
- wrong password;
- tampered stream;
- prefix truncation;
- trailing data;
- malicious/unsupported package version;
- duplicate/unexpected/nested archive entries;
- manifest count mismatch;
- invalid/missing document key;
- corrupt SQLite snapshot;
- insecure user-selected backup destination.

Controls reduce these risks but cannot protect a weak user password from all offline guessing or an already-compromised device from equivalent-privilege access.

## Export attack considerations

Exports intentionally create external copies.

Risks include:

- plaintext report/document exposure;
- insecure share target;
- cloud synchronization by another app;
- destination application vulnerabilities;
- retained historical copies after local CareNest deletion.

CareNest mitigates through explicit user action, clear boundaries, safe staging and limited app-owned cleanup. It cannot remotely recall exported copies.

## Device compromise

Residual risks include:

- unlocked device access;
- rooted/jailbroken device;
- malicious accessibility/overlay tooling;
- OS compromise;
- memory inspection;
- screenshots/screen recording;
- compromised secure storage;
- device/OS backups.

Users depend on device encryption, lock screen, OS updates and trusted software as part of the overall security posture.

## External project-support boundary

Canonical voluntary support URL:

`https://buymeacoffee.com/sanskarIN`

It is opened only after explicit user interaction and should not receive CareNest health/profile/document/reminder identifiers in query parameters.

Funding is not a health entitlement and does not change medical functionality, reminder priority, emergency assistance, support priority or access to local user records.

Current store policy must be reviewed at submission time.

## Secret management

Never commit:

- Android signing keystore/private keys;
- Apple signing certificates/private keys/provisioning secrets;
- Windows signing private keys;
- API/service credentials;
- real PINs/passwords;
- encryption keys;
- production secret `.env` files;
- real CareNest user database/backups/documents.

Signing and store credentials belong outside Git.

## Static and automated security controls

Repository automation includes:

- CodeQL;
- unsuppressed NuGet Dependency Audit;
- repository/source hygiene contracts;
- architecture dependency contracts;
- no common signing-secret file contracts;
- local-first network/telemetry policy contracts;
- logging privacy contracts;
- app-lock cryptographic contracts;
- reminder time/ownership/reconciliation/action contracts;
- SQLite migration/integrity tests;
- SQLite dependency-security contract;
- backup/document encryption integration tests;
- strict backup-topology tests;
- AEAD v2 truncation/trailing-data/v1-read compatibility tests;
- release workflow/preflight/quality-gate/Git/release-gate contracts;
- CI warnings-as-errors for applicable analyzer findings.

## Exact production-tag security behavior

Tags matching `v*` are configured to run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Release Evidence records source/ref/run/attempt provenance, tracked-source manifests/checksums, core TRX results, dependency inventories, workspace integrity and evidence checksums.

Available evidence is uploaded before aggregate failure evaluation. Therefore a failed Release Evidence run can have an artifact; artifact existence alone is not approval.

## Release Gate security behavior

Release Gate fails closed when required release/security/evidence files are missing, dependency risk is open, required checklist rows remain unchecked, or core tests fail.

Matching is hardened against normal Markdown nesting/indentation/case drift so a release blocker is not accidentally bypassed by formatting.

## Verification-relevant source

Changes to runtime, tests, project files, package files, workflows, platform configuration or build/release scripts require a new exact-head verification before the newer source becomes the production baseline.

Documentation-only changes can remain layered after a verified source boundary if a comparison proves they do not alter verification-relevant source, though documentation policy tests may justify a final marker verification.

## Security release review

Before public production promotion:

- complete exact-source automated verification;
- run CodeQL and unsuppressed Dependency Audit for exact source;
- review security/threat/logging/dependency state;
- complete packaged SQLite existing-data compatibility;
- complete encrypted document/backup compatibility;
- complete real notification/device/accessibility checks;
- review current store policy/privacy disclosures;
- configure signing outside Git;
- inspect signed artifacts/provenance;
- run exact production-tag Release Gate and Release Evidence.

See `docs/releases/SECURITY_RELEASE_REVIEW.md`.

## Remaining production security evidence

PR #56 completes the current automated source baseline, but public `1.0.0` still requires real evidence for:

- packaged existing-database/encrypted-data compatibility;
- real platform notification behavior/recovery;
- accessibility/privacy presentation;
- store policy/disclosures;
- signing/signed artifact provenance;
- exact production-tag automated evidence.

No documentation-only change marks these complete.

## Related documentation

- `SECURITY.md`
- `docs/security/THREAT_MODEL.md`
- `docs/security/LOGGING_PRIVACY.md`
- `docs/security/DEPENDENCY_RISK_REGISTER.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/architecture/DOCUMENT_VAULT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`
- `docs/testing/TESTING_GUIDE.md`
- `docs/releases/SECURITY_RELEASE_REVIEW.md`
- `docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md`
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`
