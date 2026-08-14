# Threat Model

## Assets

- Local profile information.
- User-entered medicine and appointment information.
- Medication logs.
- Imported health documents.
- Backup archives.
- App-lock verifier material.
- Document encryption master key.
- Backup password-derived key material while an operation is active.
- Local reminder/appointment scheduling state.
- Release/source provenance and signing/distribution evidence.

## Primary threats and controls

| Threat | Control | Residual risk |
|---|---|---|
| Casual access to an unlocked device | Optional app lock; generic notifications | OS snapshots, device compromise |
| App-lock PIN guessing | Numeric PIN policy, random salt, PBKDF2-HMAC-SHA256 verifier, fixed-time comparison, verifier-buffer zeroing after checks, fail-closed malformed material | A short/weak PIN has limited entropy; a compromised device/secure store may permit offline guessing |
| Partial secure-store app-lock update | Snapshot + non-cancelled rollback of enabled/salt/verifier state | Process/OS termination can interrupt compensation |
| Stolen app files | OS sandbox; encrypted document bytes | SQLite database is not transparently encrypted |
| Backup theft | Password-derived AES-256-GCM encryption | Weak user passwords can reduce practical protection |
| Tampered backup | AEAD authentication + package/version/topology validation | Denial of service remains possible |
| Encrypted-stream prefix truncation | New chunked AEAD framing v2 authenticates terminal record against next chunk counter | Existing legacy framing-v1 ciphertext remains readable and is not retroactively upgraded |
| Trailing bytes after encrypted payload | Reader requires end-of-stream after terminal | Denial of service through malformed file remains possible |
| Malicious backup ZIP topology | Strict manifest/database/key/top-level `.cndoc` allowlist, duplicate rejection, path containment | A fully valid but intentionally huge backup can still consume local resources within platform limits |
| Incomplete document import | Compensating rollback removes DB record and encrypted payload on ordinary failure | Process/OS termination can interrupt cleanup outside managed exception handling |
| Missing/corrupt document master key with existing ciphertext | Read/export fails closed; replacement key is not silently generated | Manual recovery may be impossible if the legitimate key is lost |
| Plaintext export/cache persistence | Managed cache ownership, partial-file staging, failed-operation cleanup, report share cleanup | Copies already handed to external apps/locations/screenshots/backups are outside CareNest control |
| Spreadsheet formula execution from exported user text | Formula-like CSV string prefixes are neutralized in portable output | Destination software can still transform/import data in its own ways |
| Caller-owned key bytes remain in memory | `CryptographicOperations.ZeroMemory` on known mutable key/verifier/salt/crypto buffers | Runtime/OS/secure-store copies, swap/crash dumps, and compromised process memory are outside this guarantee |
| Leaked document content in logs | Redaction rules; never log file bytes/notes | Third-party OS logs outside app control |
| Duplicate reminders | Stable occurrence keys + idempotent upsert/schedule | OS notification subsystem can still behave differently |
| Stale OS reminder after schedule/state change | Existing platform request cancelled before replacement/suppression/invalidation; old occurrence identity retained until reconciliation | OS cancellation can fail; state remains retryable and real device policy still applies |
| Handled reminder state committed while old OS request remains live | Cancellation-first Taken/Skipped/Delayed/Missed/Snoozed/Cancelled transitions | Process/OS termination can occur between cross-surface steps; recovery is compensating, not globally atomic |
| Reminder action fails after platform cancellation | Previous occurrence state restoration + non-cancelled rebuild attempt; aggregate failure on failed recovery | Abrupt process termination or OS scheduler failure can still require later startup/rebuild recovery |
| Medicine/profile cascade fails after platform cancellation | Cancel before cascade + non-cancelled reminder rebuild compensation | Rebuild itself can fail and remains visible/retryable |
| Appointment DB/platform reminder divergence | Appointment persistence compensation/reconciliation | Database and OS scheduler remain separate non-transactional surfaces |
| Missed reminders | diagnostics, permission checks, rebuild, battery/exact-alarm warnings | shutdown, force-stop, policy restrictions |
| Appointment clock-kind confusion | `StartsUtc` must be explicit UTC; local/unspecified values rejected | Incorrect user-entered date/time before UTC conversion can still be wrong; CareNest does not validate clinical consequence |
| Scheduling after denied permission | Save-time permission result checked; rebuild does not prompt/schedule while denied | OS permission can change independently after CareNest checks |
| Malicious imported file | treat as opaque bytes; no interpretation/execution | vulnerable external viewer after export |
| Rooted/jailbroken device | explicit limitation | stronger attacker can bypass sandbox/secure store |
| Shoulder surfing | lock + generic notification title | visible screen remains visible to nearby people |
| External repository/policy/funding link | fixed HTTPS destinations, explicit user action, no health-data query parameters or automatic record upload | external sites have their own privacy, account, cookie, payment and availability risks |
| Reintroduction of vulnerable SQLite native path | central transitive maintained native/provider pins, no former suppression, regression contract, unsuppressed Dependency Audit | A future package/advisory can create a new risk requiring fresh review |
| SQLite native/provider update corrupts existing user data | automated persistence/backup tests + mandatory packaged existing-data/encrypted-data compatibility matrix | Hosted CI cannot prove every real installed database/device/provider combination |
| Release tag bypasses candidate security/build gates | exact `v*` tag triggers CI, CodeQL, Dependency Audit, Release Gate, Release Evidence | Store/manual/signing approval can still be incomplete if maintainers ignore release policy |
| Failed release evidence disappears before diagnosis | evidence components run independently, artifact upload uses failure-preserving `always()`, aggregate failure occurs after upload | GitHub artifact retention is finite; external archival policy remains maintainer responsibility |
| Release evidence rerun ambiguity | artifact name includes commit SHA + Actions run ID + run attempt; evidence records run metadata | Human record-keeping can still cite the wrong run unless release records are reviewed |

## Encrypted-stream framing boundary

CareNest document and backup payload encryption uses chunked AES-256-GCM.

### Framing version 2

New encrypted streams use framing v2. Each data record authenticates:

- chunk counter;
- chunk plaintext length;
- ciphertext through AES-GCM tag.

The final terminal record has zero plaintext and its own authentication tag bound to the **next** chunk counter and zero length.

Security purpose: a valid prefix ending on a chunk boundary cannot be made to look like a complete v2 stream by merely inserting/reusing a zero-length terminator without possessing a valid terminal tag for that counter.

The reader also rejects trailing bytes after the authenticated terminal.

### Legacy framing version 1

CareNest keeps v1 decryption for existing local documents/backups.

Residual compatibility risk:

- v1 used an unauthenticated zero-length terminal;
- v1 ciphertext is not retroactively strengthened by the application update;
- retaining v1 read support is a compatibility decision to avoid data loss;
- a future v1-to-v2 migration requires explicit backup/recovery/rollback and historical-fixture verification before v1 support can be removed.

This limitation is documented rather than representing all historical encrypted data as v2-protected.

## Backup archive topology boundary

Authenticated decryption alone does not mean every ZIP member should be accepted by the restore path.

Before extraction, CareNest validates the decrypted package against the expected topology:

```text
manifest.json
database/carenest.db
secrets/document-master-key.bin
documents/<top-level>.cndoc
```

Controls reject:

- duplicate entries;
- missing manifest/database;
- unsupported package format;
- invalid schema/document count;
- unexpected files;
- nested document paths;
- non-`.cndoc` document entries;
- document-count mismatch;
- missing/invalid 32-byte document key when required.

Extraction then performs full-path containment checks as defense in depth.

Residual risk includes resource-exhaustion attempts using otherwise structurally valid large files. Platform/application size/resource limits and manual recovery behavior remain relevant.

## Document-import consistency boundary

Encrypted document import is not one filesystem+SQLite ACID transaction.

Normal failure controls:

1. encrypted payload is written;
2. metadata is saved;
3. audit is saved;
4. database-save failure removes the new encrypted payload;
5. audit failure after metadata save removes both the metadata row and encrypted payload;
6. rollback cleanup uses non-cancelled cleanup attempts;
7. incomplete rollback is surfaced explicitly.

Residual risk: abrupt process termination, storage-device failure, OS crash, or permissions changing at the wrong instant can still leave artifacts requiring recovery/cleanup.

## Sensitive memory boundary

CareNest explicitly clears mutable caller-owned cryptographic buffers when practical:

- app-lock derived/retrieved verifier bytes;
- document master-key copies returned by the secret-store abstraction;
- generated document-key buffer if persistence fails;
- backup password-derived key and random salt;
- old/restored document-key copies used during backup restore;
- chunked AEAD plaintext/ciphertext/tag/nonce/AAD work buffers.

`ZeroMemory` reduces known buffer lifetime. It does **not** claim complete erasure from:

- garbage collector/runtime internal copies;
- platform secure-store internals;
- OS/hardware caches;
- swap/hibernation;
- crash dumps;
- a debugger/malware already controlling the process/device.

## App-lock boundary

CareNest app lock is a local privacy barrier for casual access. It is not represented as full-database encryption, device encryption, or protection against a rooted/jailbroken/otherwise compromised operating system.

The PIN itself is not stored. CareNest stores a random salt and a PBKDF2-HMAC-SHA256 verifier through the platform secret store, compares derived values with a fixed-time primitive, and clears derived/stored verifier buffers after verification where the managed runtime permits.

Multi-key update/disable flows snapshot previous secure-store state and attempt non-cancelled rollback if a later key write/removal fails. Missing or malformed salt/verifier data fails closed instead of controlling derived-key length.

A user-selected numeric PIN can still have limited entropy. The app lock therefore supplements platform device authentication and secure storage; it does not replace either one.

## Appointment/reminder integrity boundary

Appointment and medicine reminders are organizational scheduling features, not clinical alarms.

Planning controls include:

- medicine planner ownership validation;
- explicit UTC planner/rebuild/snooze contracts;
- appointment `StartsUtc` explicit UTC contract;
- no silent relabeling of local/unspecified appointment ticks;
- denied notification permission does not become a platform scheduling attempt;
- background rebuild does not repeatedly request notification permission;
- deterministic DST handling for medicine schedules;
- OS delivery limitations surfaced.

Cross-surface reconciliation controls include:

- snoozed rows use `SnoozedUntilUtc` as effective due time;
- platform cancellation precedes replacement/suppression/invalidation;
- cancellation failure leaves state retryable;
- stale occurrence identity is retained until old OS requests can be cancelled;
- medicine/profile deletes compensate with non-cancelled rebuild when persistence fails after cancellation;
- appointment persistence reconciles database/platform state;
- handled reminder actions cancel the old OS request before committing state;
- later action failures attempt previous-state restoration and rebuild.

Residual risk: device/OS notification policy, shutdown, force-stop, battery management, permission changes, incorrect user-entered scheduling data, or abrupt termination between database/OS operations can still prevent/delay a reminder or require later recovery.

## SQLite dependency/provider boundary

The previously tracked native SQLite advisory path was remediated in source rather than hidden.

Current maintained graph intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- central transitive pinning enabled;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- Android native/provider leaves `2.1.12`;
- selected providers `2.1.12`;
- no `NuGetAuditSuppress` entry for `GHSA-2m69-gcr7-jv3q`.

`SqliteDependencySecurityContractTests` rejects restoring the old package floor/suppression.

Automated risk evidence includes unsuppressed NuGet audits and the full core/platform matrix. Manual packaged upgrade tests remain required because dependency security and data compatibility are separate properties.

If a future SQLite/provider change fixes a vulnerability but corrupts/misreads existing databases or breaks encrypted document/backup workflows, production promotion is blocked until both security and data integrity are acceptable.

## Release automation boundary

CareNest distinguishes candidate verification from production permission.

Marker-only PR verification protects exact source changes through:

- formatting;
- unit/integration/UI-contract tests;
- Android/Windows/iOS simulator/Mac Catalyst Release builds;
- CodeQL;
- Dependency Audit.

Exact `v*` tags trigger the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Release Evidence captures source/ref/run identity, tracked-file manifests/checksums, all core TRX suites, dependency inventories, workspace integrity, and evidence checksums. Components are attempted independently; available evidence is uploaded with failure-preserving behavior before an aggregate pass/fail step.

This automation does not replace manual device/accessibility/store/signing/package/data-compatibility approval.

## External voluntary-support boundary

The Buy Me a Coffee destination is fixed as `https://buymeacoffee.com/sanskarIN` through `AppConstants.FundingUrl` and is opened only after explicit user interaction. CareNest does not append profile IDs, medicine names, document metadata, reminder history, backup data, app-lock information, or other local health content to the URL.

The funding provider is outside the CareNest trust boundary. Browser/network metadata and any information/payment details the user chooses to provide there are governed by that external service, not by CareNest.

No embedded payment SDK, payment token, API secret, or funding-provider credential is stored in the CareNest source/runtime for this link.

## Automated evidence

### Authoritative bug-audit baseline

PR #54 completed the fully green 2026-08-14 runtime/test/dependency baseline:

- CareNest CI #503 / `31766059137`: success;
- 122 unit + 39 integration + 100 UI-contract = 261 tests passed;
- Android/Windows/iOS simulator/Mac Catalyst Release builds passed;
- CodeQL #503 / `31766059215`: success;
- unsuppressed Dependency Audit #35 / `31766059132`: success.

### Release-engineering checkpoint

PR #55 verified the first release-engineering hardening snapshot far enough to prove:

- formatting success;
- 122 unit tests passed;
- 39 integration tests passed;
- 116 UI-contract/policy tests passed;
- 277 total core tests passed;
- Android Release passed;
- Windows Release passed;
- CodeQL #547 / `31769940053`: success;
- unsuppressed Dependency Audit #38 / `31769940039`: success.

PR #55 was closed unmerged as superseded after the complete-file audit found further legitimate release-tooling/documentation fixes. A new exact-source verification is required for the final current head.

## Out of scope for v1

- Server compromise, because no CareNest backend exists.
- Cloud sharing/caregiver synchronization.
- Clinical correctness or medical decision support.
- Security/privacy guarantees for independently opened external websites after the user leaves the CareNest app surface.
- Protection against a fully compromised/rooted/jailbroken device or malicious code already executing with equivalent privileges.

## Security review triggers

A new review is mandatory before adding accounts, remote sync, analytics, crash uploads containing user state, document interpretation, sharing by default, medical decision support, an embedded web view for external services, payment/funding SDKs, purchase entitlements, biometric app-lock bypass/recovery, remote PIN recovery, automatic encrypted-data migration that drops v1 compatibility, or any external-link flow that transmits CareNest user data.

A new review is also mandatory before weakening release tag gates, dependency-audit behavior, evidence retention/provenance, or existing-data compatibility requirements.
