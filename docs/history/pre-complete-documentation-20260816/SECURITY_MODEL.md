# CareNest Security Architecture Reference

This document is the current technical security reference for CareNest `1.0.0-rc.1`. It complements `SECURITY.md`, `THREAT_MODEL.md`, `LOGGING_PRIVACY.md`, `DEPENDENCY_RISK_REGISTER.md`, the privacy model, and the release security review.

CareNest is a local-first organizational health app. It does not claim protection against a fully compromised device/OS and does not provide clinical decision support.

## Current authoritative automated security baseline

Marker-only PR #61 is the current exact automated/source-inspection baseline.

- frozen source/base: `4c60f90ac33a321d12a6f9b3a8c097e4e4a4e5f2`;
- marker head: `19c82b813c375047cf1166487bc18a1bd2cd0e52`;
- PR merge/event SHA during verification: `c8ea9fef89d7b773f19bf13c64f349495be706ad`;
- CareNest CI #650 / `31872610834`: success;
- 122 unit + 39 integration + 157 UI-contract/policy = **318/318** core tests;
- default Android/Windows/iOS simulator/Mac Catalyst Release builds: success;
- CareNest Store Package Configuration #39 / `31872610789`: success;
- funding-disabled Android/Windows/iOS simulator/Mac Catalyst Release builds: success;
- Bash store-package preflight executable-mode guard: success;
- CareNest Store Inspection Artifacts #2 / `31872610786`: success;
- verified-unsigned Android AAB internal artifact with downloaded checksum/provenance inspection: success;
- self-contained unpackaged Windows internal artifact with downloaded checksum/provenance inspection: success;
- iOS simulator and unsigned Mac Catalyst internal artifacts with downloaded checksum/provenance inspection: success;
- CodeQL #650 / `31872610815`: success;
- unsuppressed Dependency Audit #46 / `31872610791`: success.

PR #61 was closed without merge; its verification marker is not part of `main`.

PR #60 remains a superseded failure-driven artifact checkpoint. PR #59 remains historical exact store-safe compilation evidence, PR #58 remains historical exact package/store-policy hardening evidence, PR #56 remains historical exact release-engineering evidence, and PR #54 remains the historical authoritative runtime bug-audit checkpoint.

See `docs/releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md`.

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

It is opened only after explicit user interaction in configurations where the in-app support surface is enabled and should not receive CareNest health/profile/document/reminder identifiers in query parameters.

Funding is not a health entitlement and does not change medical functionality, reminder priority, emergency assistance, support priority or access to local user records.

`CareNestShowFundingLink=false` hides the complete About-page external support card and makes the funding command non-executable without changing health-organizer behavior.

The current 2026-08-15 Apple/Google policy review selects the funding-disabled configuration for initial store candidates unless submission-time policy clearly permits the external support link. Store policy must be reviewed again at actual submission time.

## Internal store-inspection artifact boundary

`CareNest Store Inspection Artifacts` creates reproducible internal artifacts with the external funding surface disabled. This is a source/configuration/provenance control, not production signing evidence.

Security properties protected by workflow and source-policy contracts include:

- artifact generation checks out and names output from the exact source head;
- pull-request event/merge SHA is retained separately from the inspected source SHA;
- Android staging requires exactly one non-`-Signed.aab` candidate;
- the Android AAB is rejected if JAR-signature metadata is present;
- Android provenance records `signing=verified-unsigned` and that the debug-signed companion was not staged;
- Windows output is an unpackaged self-contained inspection bundle, not a signed package;
- iOS output is a simulator bundle;
- Mac Catalyst output is published with code signing/package creation disabled;
- every artifact records SHA-256/provenance and `store_submission_ready=false`;
- production signing credentials are not injected by the inspection workflow.

PR #60 demonstrated why downloaded-artifact inspection is necessary: the initial workflow uploaded MAUI's debug-signed Android companion while the job itself was green. The corrected workflow was then verified under PR #61.

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
- store-package workflow/preflight contracts;
- store-inspection artifact/signing/provenance contracts;
- package metadata/privacy and Windows publish-RID contracts;
- CI warnings-as-errors for applicable analyzer findings.

## Exact production-tag security behavior

Tags matching `v*` are configured to run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- CareNest Store Package Configuration;
- CareNest Store Inspection Artifacts;
- Release Gate;
- CareNest Release Evidence.

Release Evidence records source/ref/run/attempt provenance, tracked-source manifests/checksums, core TRX results, dependency inventories, workspace integrity and evidence checksums.

Available evidence is uploaded before aggregate failure evaluation. Therefore a failed Release Evidence run can have an artifact; artifact existence alone is not approval.

Store Package Configuration adds funding-disabled source compilation across Android, Windows, iOS simulator and Mac Catalyst. Store Inspection Artifacts adds reproducible internal package-shape/checksum/provenance evidence. Neither signs, submits, approves, or proves installed production-store behavior.

## Release Gate security behavior

Release Gate fails closed when required release/security/evidence files are missing, dependency risk is open, required checklist rows remain unchecked, or core tests fail.

Matching is hardened against normal Markdown nesting/indentation/case drift so a release blocker is not accidentally bypassed by formatting.

## Verification-relevant source

Changes to runtime, tests, project files, package files, workflows, platform configuration, artifact generation, or build/release scripts require a new exact-head verification before the newer source becomes the production baseline.

Documentation-only changes can remain layered after a verified source boundary if a comparison proves they do not alter verification-relevant source, though documentation policy tests may justify a final policy check.

## Security release review

Before public production promotion:

- complete exact-source automated verification;
- run CodeQL and unsuppressed Dependency Audit for exact source;
- run funding-disabled store-safe compilation when current store packaging requires it;
- run and inspect internal store artifacts for exact source when the release workflow requires them;
- review security/threat/logging/dependency state;
- complete packaged SQLite existing-data compatibility;
- complete encrypted document/backup compatibility;
- complete real notification/device/accessibility checks;
- re-review current store policy/privacy disclosures at submission time;
- configure signing outside Git;
- inspect signed artifacts/provenance and actual funding-link visibility;
- run exact production-tag CI, CodeQL, Dependency Audit, Store Package Configuration, Store Inspection Artifacts, Release Gate and Release Evidence.

See `docs/releases/SECURITY_RELEASE_REVIEW.md`.

## Remaining production security evidence

PR #61 completes the current automated/source-inspection baseline, but public `1.0.0` still requires real evidence for:

- packaged existing-database/encrypted-data compatibility;
- real platform notification behavior/recovery;
- accessibility/privacy presentation;
- submission-time store policy/disclosures;
- signed and installed store-safe package inspection;
- signing/signed artifact provenance;
- exact production-tag automated evidence, including tagged Store Inspection Artifacts.

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
- `docs/releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md`
- `docs/releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md`
- `docs/releases/STORE_POLICY_REVIEW_20260815.md`
- `docs/releases/STORE_BUILD_POLICY.md`
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`