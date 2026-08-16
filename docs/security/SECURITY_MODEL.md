# CareNest Security Architecture Reference

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`  
**Verified PR #74 head:** `8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

CareNest is a local-first organizational health app. This security model describes source-controlled protections and residual risk. It does not claim protection against a fully compromised device/OS, clinical correctness, guaranteed reminder delivery, or completed production signing/store review.

## 1. Current automated security baseline

PR #74 verified:

- 122 unit + 39 integration + 170 UI/source-policy = **331/331** core tests;
- Android, Windows, iOS simulator and Mac Catalyst Release builds;
- all four store-candidate configurations;
- Store Inspection Artifacts, including fail-closed payload-scanner self-test;
- CodeQL;
- unsuppressed Dependency Audit;
- strict XAML compiled-binding policy.

Permanent current evidence: `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

Older PR #68/#67/#61/#59/#58/#56/#54 security/package evidence remains historical for older source boundaries.

## 2. Security objectives

CareNest aims to:

- minimize unnecessary data movement;
- keep normal structured records local in v1;
- encrypt imported document payloads;
- encrypt manual backups;
- store app-lock/document key material through platform secure storage where applicable;
- avoid sensitive application logging;
- fail closed for invalid cryptographic/key state;
- validate backup topology before extraction;
- reconcile reminder persisted/platform state conservatively;
- keep dependency/security/release gates fail closed;
- preserve documented encrypted-data compatibility rather than silently making old data unreadable.

## 3. Trust boundaries

### Primary CareNest-controlled boundary

- application process;
- app sandbox/private files;
- local SQLite database;
- encrypted document-vault payloads;
- application-owned cache/staging while still under CareNest ownership;
- secure-storage APIs used through CareNest abstractions;
- validated application configuration/services.

### Separate/external boundaries

- OS notification/alarm subsystem;
- OS calendar/file/share/browser services;
- external files/cloud drives/apps;
- distribution/store systems;
- device/OS backups/snapshots;
- build/source-hosting infrastructure;
- rooted/jailbroken/otherwise compromised environments.

Copies handed to another destination are outside CareNest control.

## 4. Local-first network boundary

Current v1 requires no CareNest account/backend and includes no hidden runtime analytics/telemetry client.

A future account/sync/remote caregiver/analytics subsystem requires explicit authentication, authorization, consent, key management, privacy, deletion/export, abuse/threat-model and store-disclosure review.

## 5. Structured SQLite protection

Structured records live in local SQLite storage.

Security statement:

- protected primarily by application sandbox/device security;
- no transparent whole-database encryption claim;
- UI/ViewModels do not issue direct SQL;
- persistence is isolated to infrastructure/repositories;
- migrations are ordered/versioned;
- consistency-sensitive operations use transactions where appropriate;
- WAL/snapshot/integrity behavior is tested;
- backup snapshots validate committed content/integrity.

A compromised device or raw private-file extraction can expose structured SQLite content.

## 6. SQLite dependency security

Current graph intent includes:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- central transitive pinning;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- Android/provider leaves `2.1.12` where pinned;
- no former `GHSA-2m69-gcr7-jv3q` exact audit suppression.

Source-policy tests protect these floors/suppression absence and Dependency Audit is blocking.

A clean dependency graph does not prove packaged existing-database/encrypted-data compatibility; both are required before production.

## 7. Encrypted document protection

Imported application-owned document payloads use authenticated encryption.

Current design includes:

- 32-byte document master key;
- platform secure storage for that key where applicable;
- AES-256-GCM;
- chunked authenticated framing v2 for new writes;
- counter/length-bound associated data;
- authenticated terminal record;
- rejection of trailing data;
- retained legacy framing v1 read compatibility;
- tamper/truncation/trailing-data tests;
- sensitive mutable buffer clearing where practical.

V2 does not retroactively strengthen historical v1 ciphertext.

## 8. Document-key failure behavior

If encrypted payloads exist and the required document master key is missing/corrupt, CareNest fails closed.

It does not silently generate an unrelated replacement key and then claim existing ciphertext remains recoverable.

## 9. Document import/export consistency

Encrypted filesystem payload and SQLite metadata are separate state surfaces.

Current compensation model creates encrypted payload, persists metadata/audit state and attempts rollback/cleanup when later steps fail. Cleanup is not intentionally abandoned merely because the initiating caller cancellation token is cancelled.

Explicit export/decrypt creates plaintext/portable output outside the encrypted vault boundary. CareNest cannot revoke copies already controlled by another app/service/filesystem/screenshot/backup.

## 10. Backup protection

Manual backup uses:

- user password;
- PBKDF2-HMAC-SHA256 derivation;
- random salt;
- AES-256-GCM authenticated encryption;
- chunked v2 framing for new writes;
- versioned package metadata;
- document-recovery material where required;
- strict decrypted ZIP topology validation;
- wrong-password/tamper/truncation/trailing-data rejection;
- database snapshot/integrity checks.

Weak user passwords remain vulnerable to offline guessing within their entropy limits.

## 11. Restore rollback/key integrity

Restore validates package/authentication/topology before acceptance and attempts to preserve/restore prior key/database/filesystem state when a later operation fails.

Process/OS termination can still interrupt compensation; that remains residual risk.

## 12. Sensitive buffer handling

Known mutable application-owned cryptographic buffers are cleared with `CryptographicOperations.ZeroMemory` where practical.

This reduces lifetime of known arrays but does not guarantee erasure from runtime internals, OS secure storage, swap/hibernation, crash dumps or a compromised process.

## 13. App-lock protection

Optional app lock uses:

- numeric PIN policy;
- random salt;
- PBKDF2-HMAC-SHA256 verifier derivation;
- fixed-time comparison;
- platform secure storage for enabled/salt/verifier material;
- strict material validation;
- rollback across multi-key updates/disable transitions;
- fail-closed missing/corrupt material;
- clearing of mutable verifier buffers where practical.

App lock is a local privacy barrier, not whole-database/device encryption or protection against a compromised OS/secure store.

## 14. Notification privacy/integrity

Notification content is privacy-minimized/generic by default and must not contain document contents, passwords, PINs or keys.

OS policy controls final display/history/lock-screen preview.

Scheduling integrity includes:

- true UTC appointment/snooze/planner boundaries;
- deterministic ownership/time-zone/DST rules;
- stable occurrence identity;
- as-needed/inactive-state suppression;
- persisted occurrence ↔ OS request reconciliation;
- cancellation before replacement/suppression/invalidation;
- cancellation-first handled states;
- retryable cancellation failure;
- restoration/rebuild compensation.

OS permission, shutdown, force-stop, battery/vendor restrictions can still prevent delivery.

## 15. Logging protection

Routine sensitive-path logs must not contain:

- raw medicine/health notes;
- document/backup contents;
- backup passwords;
- app-lock PINs;
- encryption/signing keys;
- unnecessary sensitive record identifiers;
- raw sensitive exception messages/stack traces where avoidable.

Use privacy-minimized category/operation and exception type where sufficient.

## 16. Export/share risks

Exports intentionally create external copies.

Risks include plaintext report/document exposure, insecure share targets, cloud synchronization, destination vulnerabilities and retained copies after local deletion.

Mitigation is explicit user action, safe staging/cleanup while CareNest owns the file and clear documentation—not remote revocation claims.

## 17. Application funding/package boundary

The distributed CareNest application runtime/source/package contains **no external Buy Me a Coffee destination/card/command/artwork**.

Repository-only voluntary support URL:

`https://buymeacoffee.com/sanskarIN`

It is not a health entitlement and does not alter medical functionality, reminder priority/reliability, emergency assistance, support priority or access to local records.

There is no current `CareNestShowFundingLink` build property. Store builds do not depend on a funding visibility fork.

The package payload scanner remains defense-in-depth against accidental reintroduction of the canonical external marker.

## 18. Internal inspection-artifact boundary

`Store Inspection Artifacts` creates non-production engineering evidence for the exact source head:

- unsigned Android AAB inspection artifact;
- unpackaged/self-contained Windows inspection output;
- iOS simulator inspection output;
- unsigned Mac Catalyst inspection output;
- payload scan;
- checksums/provenance;
- `store_submission_ready=false`/equivalent non-production boundary;
- no production signing-secret injection.

These artifacts are not proof of final production signing/store approval.

## 19. Release automation boundary

Production-style `v*` tags are expected to participate in:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

Automation does not replace manual device/accessibility/package/signing/store approval.

## 20. Secret management

Never commit:

- Android keystores/private keys;
- Apple private keys/certificates/provisioning secrets;
- Windows signing private keys;
- API/service credentials;
- real PINs/passwords;
- encryption keys;
- production secret environment files;
- real CareNest databases/backups/documents.

## 21. Static/automated controls

Current controls include:

- CodeQL;
- blocking unsuppressed NuGet audit;
- source hygiene/architecture tests;
- no common signing-secret file contracts;
- local-first network/telemetry policy tests;
- logging privacy tests;
- app-lock crypto tests;
- reminder time/ownership/reconciliation/action tests;
- SQLite migration/integrity tests;
- document/backup tamper/framing tests;
- strict compiled XAML tests;
- funding-free app/source/package contracts;
- package payload scanner self-test;
- exact-source release workflow/evidence contracts.

## 22. Residual risks

Residual risk includes:

- fully compromised/rooted/jailbroken device;
- malicious accessibility/overlay/process inspection;
- unlocked-device physical access;
- weak backup/app-lock secrets;
- OS/cloud backup copies;
- exported/shared copies;
- OS notification failure;
- process termination during cross-surface compensation;
- future dependency/toolchain advisories;
- human release-policy mistakes.

## 23. Required production security evidence

Before final production:

- supported-platform manual tests;
- actual notification permission/delivery/recovery limitations;
- packaged SQLite compatibility;
- encrypted document/backup compatibility;
- accessibility/privacy presentation checks;
- current store-policy/privacy review;
- production signing outside Git;
- final signed-package checksum/provenance/forbidden-marker scan;
- exact immutable production tag and all tagged gates.

## 24. Security review triggers

A fresh security/privacy review is required before materially changing:

- accounts/authentication;
- cloud sync/remote caregiver collaboration;
- analytics/crash-state upload;
- document interpretation/medical decision support;
- biometric/remote app-lock recovery;
- encryption framing/key ownership/legacy compatibility;
- raw SQL/import execution paths;
- dependency/release audit/evidence policy;
- package signing/provenance;
- external payment/funding SDKs or in-app funding surfaces.

## Related documents

- `SECURITY.md`
- `PRIVACY.md`
- `docs/security/THREAT_MODEL.md`
- `docs/security/LOGGING_PRIVACY.md`
- `docs/security/DEPENDENCY_RISK_REGISTER.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/releases/SECURITY_RELEASE_REVIEW.md`
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`