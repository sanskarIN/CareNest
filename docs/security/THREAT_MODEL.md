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

## Primary threats and controls

| Threat | Control | Residual risk |
|---|---|---|
| Casual access to an unlocked device | Optional app lock; generic notifications | OS snapshots, device compromise |
| App-lock PIN guessing | Numeric PIN policy, random salt, PBKDF2-HMAC-SHA256 verifier, fixed-time comparison, verifier-buffer zeroing after checks | A short/weak PIN has limited entropy; a compromised device/secure store may permit offline guessing |
| Stolen app files | OS sandbox; encrypted document bytes | SQLite database is not transparently encrypted |
| Backup theft | Password-derived AES-256-GCM encryption | Weak user passwords can reduce practical protection |
| Tampered backup | AEAD authentication + package/version/topology validation | Denial of service remains possible |
| Encrypted-stream prefix truncation | New chunked AEAD framing v2 authenticates terminal record against next chunk counter | Existing legacy framing-v1 ciphertext remains readable and is not retroactively upgraded |
| Trailing bytes after encrypted payload | Reader requires end-of-stream after terminal | Denial of service through malformed file remains possible |
| Malicious backup ZIP topology | Strict manifest/database/key/top-level `.cndoc` allowlist, duplicate rejection, path containment | A fully valid but intentionally huge backup can still consume local resources within platform limits |
| Incomplete document import | Compensating rollback removes DB record and encrypted payload on ordinary failure | Process/OS termination can interrupt cleanup outside managed exception handling |
| Caller-owned key bytes remain in memory | `CryptographicOperations.ZeroMemory` on known mutable key/verifier/salt/crypto buffers | Runtime/OS/secure-store copies, swap/crash dumps, and compromised process memory are outside this guarantee |
| Leaked document content in logs | Redaction rules; never log file bytes/notes | Third-party OS logs outside app control |
| Duplicate reminders | Stable occurrence keys + idempotent upsert/schedule | OS notification subsystem can still behave differently |
| Missed reminders | diagnostics, permission checks, rebuild, battery/exact-alarm warnings | shutdown, force-stop, policy restrictions |
| Appointment clock-kind confusion | `StartsUtc` must be explicit UTC; local/unspecified values rejected | Incorrect user-entered date/time before UTC conversion can still be wrong; CareNest does not validate clinical consequence |
| Scheduling after denied permission | Save-time permission result checked; rebuild does not prompt/schedule while denied | OS permission can change independently after CareNest checks |
| Malicious imported file | treat as opaque bytes; no interpretation/execution | vulnerable external viewer after export |
| Rooted/jailbroken device | explicit limitation | stronger attacker can bypass sandbox/secure store |
| Shoulder surfing | lock + generic notification title | visible screen remains visible to nearby people |
| External repository/policy/funding link | fixed HTTPS destinations, explicit user action, no health-data query parameters or automatic record upload | external sites have their own privacy, account, cookie, payment and availability risks |
| Vulnerable SQLite native dependency | exact advisory tracked, narrow audit suppression, migration plan, release gate | `GHSA-2m69-gcr7-jv3q` remains open until a verified compatible remediation/release decision exists |

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

A user-selected numeric PIN can still have limited entropy. The app lock therefore supplements platform device authentication and secure storage; it does not replace either one.

## Appointment/reminder integrity boundary

Appointment and medicine reminders are organizational scheduling features, not clinical alarms.

Controls include:

- medicine planner ownership validation;
- explicit UTC planner/rebuild/snooze contracts;
- appointment `StartsUtc` explicit UTC contract;
- no silent relabeling of local/unspecified appointment ticks;
- denied notification permission does not become a platform scheduling attempt;
- background rebuild does not repeatedly request notification permission;
- deterministic DST handling for medicine schedules;
- OS delivery limitations surfaced.

Residual risk: device/OS notification policy, shutdown, force-stop, battery management, permission changes, or incorrect user-entered scheduling data can still prevent/delay a reminder.

## External voluntary-support boundary

The Buy Me a Coffee destination is fixed as `https://buymeacoffee.com/sanskarIN` through `AppConstants.FundingUrl` and is opened only after explicit user interaction. CareNest does not append profile IDs, medicine names, document metadata, reminder history, backup data, app-lock information, or other local health content to the URL.

The funding provider is outside the CareNest trust boundary. Browser/network metadata and any information/payment details the user chooses to provide there are governed by that external service, not by CareNest.

No embedded payment SDK, payment token, API secret, or funding-provider credential is stored in the CareNest source/runtime for this link.

## Automated evidence

Exact source `4f5f9abe9d702fa33d6aba3f15c113febfebf95e` passed marker-only PR #33:

- CareNest CI #332 / `31691592300`: success;
- 106 unit + 30 integration + 54 UI-contract = 190 core tests passed;
- Android/Windows/iOS simulator/Mac Catalyst Release builds passed;
- CodeQL #332 / `31691592435`: success;
- Dependency Audit #13 / `31691592302`: success.

The SQLitePCLRaw advisory remains open despite the successful Dependency Audit workflow under its narrow exact suppression.

## Out of scope for v1

- Server compromise, because no CareNest backend exists.
- Cloud sharing/caregiver synchronization.
- Clinical correctness or medical decision support.
- Security/privacy guarantees for independently opened external websites after the user leaves the CareNest app surface.
- Protection against a fully compromised/rooted/jailbroken device or malicious code already executing with equivalent privileges.

## Security review triggers

A new review is mandatory before adding accounts, remote sync, analytics, crash uploads containing user state, document interpretation, sharing by default, medical decision support, an embedded web view for external services, payment/funding SDKs, purchase entitlements, biometric app-lock bypass/recovery, remote PIN recovery, automatic encrypted-data migration that drops v1 compatibility, or any external-link flow that transmits CareNest user data.
