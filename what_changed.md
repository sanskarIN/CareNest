# what_changed.md

## CareNest complete continuation handoff — 2026-08-13

This is the active detailed CareNest handoff for the continuation completed on 2026-08-13.

The user requested continued implementation with complete code, maximum logical commits, no skipped files, GitHub delivery on `main`, and an updated `what_changed.md`. This file records the complete continuation from the previously documentation-complete release-candidate state through the latest service, appointment, document, backup, cryptographic, testing, security, verification, and documentation hardening.

Repository: `https://github.com/sanskarIN/CareNest`  
Branch: `main`  
Release target: `1.0.0-rc.1`  
Framework: .NET 10 / .NET MAUI  
Primary language: C#  
License: Apache-2.0  
Business email: `sanskarin@outlook.in`  
Support email: `supportramsandesh@gmail.com`  
Creator: `https://github.com/sanskarIN`  
Voluntary project support: `https://buymeacoffee.com/sanskarIN`  
Watermark: `Made by the Sanskar`

---

# Preserved earlier handoffs

No earlier implementation/history record was discarded when this active handoff was replaced.

The complete earlier Phase 0–8 implementation/hardening/verification handoff remains on current `main` at:

`docs/history/what_changed_full_through_phase8.md`

That file reuses the exact historical Git blob and contains the complete earlier repository assembly, privacy/logging hardening, reminder-integrity hardening, PR #24–#30 verification history, SQLite/WAL/app-lock work, BMC work, and earlier commit-level record.

The complete 2026-08-12 documentation-completion handoff is also preserved on current `main` at:

`docs/history/what_changed_documentation_through_20260812.md`

Preservation commit:

`d9271e24aa8b9884fc99ade11636a16e77aad9ea` — `docs: preserve complete 2026-08-12 CareNest handoff`

The active file therefore continues the record rather than replacing history with a shorter summary.

---

# Product and medical-safety boundary retained

CareNest remains a local-first organizational application.

CareNest does **not**:

- diagnose conditions;
- determine or infer medicine dosage;
- recommend treatment;
- perform medication-interaction checking as a clinical feature;
- create clinical risk scores;
- independently verify medication adherence;
- replace a clinician or pharmacist;
- provide emergency services;
- guarantee notification delivery.

Medicine `StrengthText` and `InstructionText` remain opaque user-entered text. Reminder schedules and stock changes are derived only from explicit user-entered configuration values.

The current continuation adds correctness/security/reliability controls only. It does not introduce clinical interpretation.

---

# Local-first/privacy boundary retained

Current v1 still has:

- no required CareNest account;
- no required CareNest backend/server;
- no automatic CareNest cloud sync;
- no silent caregiver sharing;
- no hidden analytics/telemetry client;
- local SQLite structured records;
- encrypted imported document payloads;
- manual password-encrypted backups;
- explicit user-controlled export/share/calendar actions;
- optional local app lock;
- privacy-minimized notification/logging behavior.

The SQLite database is still **not** represented as transparently whole-database encrypted. The application sandbox/device security protects SQLite metadata, while document payloads and manual backup payloads have separate authenticated-encryption protections.

---

# Starting point for this continuation

The prior documentation-complete `main` head before new runtime/test work was:

`d1d62aaa9a4e8579badf0e89ee740bf39d0f4605`

At that point the latest exact verified runtime/test source was still PR #30 source:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

PR #30 baseline:

- CareNest CI #248 / `31382194805`: success;
- 74 unit tests;
- 13 integration tests;
- 54 UI-contract/policy tests;
- 141 total core tests;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #248 / `31382194687`: success;
- Dependency Audit #10 / `31382194683`: success.

The current continuation supersedes that automated source baseline with PR #33 as recorded below.

---

# Continuation goals completed

The current continuation focused on remaining automatable release-quality work after the repository became source complete and documentation complete.

Completed areas:

- appointment UTC integrity;
- appointment notification-permission fail-safe behavior;
- direct application-service unit coverage;
- deterministic reusable test doubles;
- document import rollback consistency across SQLite + encrypted filesystem payload;
- encrypted document key-buffer hygiene;
- strict decrypted-backup archive topology;
- backup cryptographic key/salt/key-copy hygiene;
- authenticated chunked AEAD stream termination v2;
- legacy encrypted-stream v1 read compatibility;
- prefix-truncation resistance for new v2 streams;
- trailing-data rejection for new v2 streams;
- encrypted document metadata updated for new framing version;
- exact-head PR verification cycles #31, #32, and #33;
- complete architecture/security/testing/release/status/documentation alignment.

---

# Detailed runtime/test commit ledger

The continuation was intentionally split into many logical commits so behavior/security/test changes remain reviewable.

## 1. Require explicit UTC appointment start timestamps

Commit:

`4263d18c16eeca8da13a9efc7efe344490c41ec3` — `fix: require UTC appointment start timestamps`

Updated:

`src/CareNest.Domain/Rules/AppointmentRules.cs`

Behavior:

- validates non-null appointment;
- validates profile ID/title;
- trims/validates `TimeZoneId`;
- requires `appointment.StartsUtc.Kind == DateTimeKind.Utc`;
- rejects local/unspecified appointment start values;
- retains the explicit reminder-lead range boundary.

Reason:

The property is explicitly named/persisted as UTC and must not silently accept ambiguous clock kinds.

## 2. Stop silently relabeling appointment clock ticks as UTC

Commit:

`175180eba49f51a5a18204e4857f66151a9adb1f` — `fix: stop reinterpreting appointment start time as UTC`

Updated:

`src/CareNest.Application/Services/AppointmentService.cs`

Behavior:

- removes silent `DateTime.SpecifyKind(..., DateTimeKind.Utc)` reinterpretation;
- requires stored appointment start to already satisfy the UTC contract before reminder scheduling;
- calculates reminder due time from the validated explicit UTC start instant plus the user-entered reminder lead.

This is a time-integrity fix, not a medical scheduling decision.

## 3. Appointment UTC/time-zone rule tests

Commit:

`9a3b160feca2413c64b579989b807f6e28490881` — `test: cover appointment UTC and timezone validation`

Updated:

`tests/CareNest.UnitTests/AppointmentAndProfileRulesTests.cs`

Coverage:

- local appointment start rejected;
- unspecified appointment start rejected;
- valid UTC accepted;
- valid time-zone identifier is trimmed;
- existing reminder-lead boundary remains covered.

## 4. Reusable repository test double

Commit:

`899ad86f875c18eef3606cfaee3189078c44cd1c` — `test: add reusable repository test double`

Added:

`tests/CareNest.UnitTests/TestDoubles/RepositoryStub.cs`

Provides a complete no-op/default implementation of `ICareNestRepository` that individual service tests can override narrowly.

## 5. Deterministic TimeProvider test double

Commit:

`a2dd856f9f2a22360d98f505592737ded3c7365e` — `test: add deterministic CareNest time provider`

Added:

`tests/CareNest.UnitTests/TestDoubles/FixedTimeProvider.cs`

Allows application-service tests to assert exact UTC timestamps without depending on the runner clock.

## 6. Reminder coordinator spy

Commit:

`034ba02a225b11d98fa102f0601be8b6398455cd` — `test: add reminder coordinator spy`

Added:

`tests/CareNest.UnitTests/TestDoubles/ReminderCoordinatorSpy.cs`

Tracks reminder rebuild calls while preserving a no-op platform-neutral service boundary.

## 7. Notification service spy

Commit:

`d0475ef0a99d3bc2b753eed73142ac38f464b258` — `test: add notification service spy`

Added:

`tests/CareNest.UnitTests/TestDoubles/NotificationServiceSpy.cs`

Tracks:

- permission requests;
- diagnostics;
- scheduled notification requests;
- cancellation calls;
- test notification calls.

## 8. Encrypted document-store spy

Commit:

`2062546eb2192357420ce63cdfbb4193f5ac5179` — `test: add encrypted document store spy`

Added:

`tests/CareNest.UnitTests/TestDoubles/DocumentStoreSpy.cs`

Supports deterministic application-level import/export/delete tests without invoking real cryptography.

## 9. ProfileService direct tests

Commit:

`a1aabf5209ccd5b833ca88cbf4972ee8fa8d91d4` — `test: cover profile service save and deletion flows`

Added:

`tests/CareNest.UnitTests/ProfileServiceTests.cs`

Coverage includes:

- new profile → Created audit;
- existing profile → Updated audit;
- `UpdatedUtc` comes from deterministic UTC time provider;
- profile deletion coordinates encrypted document cleanup;
- profile-photo encrypted payload cleanup;
- profile cascade deletion;
- deletion audit.

## 10. Respect denied appointment notification permission

Commit:

`fa7e935b8f1da4f87cfa1751db6e86e07985466c` — `fix: respect denied appointment notification permission`

Updated:

`src/CareNest.Application/Services/AppointmentService.cs`

Behavior:

- if diagnostics show permission denied during an explicit reminder-capable save, CareNest requests permission;
- if permission remains denied, no platform notification schedule is attempted;
- rebuild path does not prompt for permission;
- rebuild path does not attempt scheduling while permission remains denied.

The appointment record itself remains local and saved even when notification permission is unavailable.

## 11. AppointmentService direct tests

Commit:

`c16b1bdffb8264f0bac572dd72ade8283cbb6025` — `test: cover appointment reminder service behavior`

Added:

`tests/CareNest.UnitTests/AppointmentServiceTests.cs`

Coverage:

- new appointment save + Created audit;
- exact reminder due time from explicit UTC start;
- denied permission + rejected request → no schedule;
- denied permission + granted request → schedule;
- rebuild with denied permission → no prompt/no schedule;
- stored non-UTC appointment → fail closed;
- delete cancels platform reminder then deletes record.

## 12. MedicineService direct tests

Commit:

`8a5a9c0ae87e3b27ec2d073318cb31c07bd83b4d` — `test: cover medicine service persistence and stock flows`

Added:

`tests/CareNest.UnitTests/MedicineServiceTests.cs`

Coverage:

- new medicine → Created audit;
- existing medicine → Updated audit;
- reminder rebuild after save;
- schedule persistence;
- future occurrence invalidation from exact current UTC;
- reminder rebuild after schedule save;
- stock adjustment uses repository/user-entered values;
- fallback to explicit medicine stock value;
- negative estimated stock rejected before persistence;
- medicine cascade delete + audit + reminder rebuild.

No stock change is inferred from medicine strength/instructions.

## 13. Roll back document DB record when import audit fails

Commit:

`ae2c09be7774a6a846ed4ad9c0ec0cf6bd98be35` — `fix: rollback document record when import audit fails`

Updated:

`src/CareNest.Application/Services/DocumentService.cs`

Previous gap:

If encrypted payload creation and DB metadata save succeeded but audit persistence failed, the catch path removed the encrypted payload but could leave the DB record pointing to a now-missing encrypted file.

Current behavior:

1. encrypted payload is created;
2. DB document metadata is saved;
3. audit entry is attempted;
4. if DB save fails, encrypted payload is removed;
5. if audit fails after DB save, DB record and encrypted payload are both rolled back;
6. rollback uses `CancellationToken.None` so a cancelled main operation does not intentionally strand newly created artifacts;
7. cleanup failures are collected;
8. if cleanup cannot fully complete, an `AggregateException` surfaces original + cleanup failures instead of hiding partial cleanup.

This is compensating rollback, not a claim of a cross-filesystem/SQLite ACID transaction.

## 14. DocumentService rollback/export tests

Commit:

`65a9917ead7db985b97eafeea39554b6cce2ca45` — `test: cover encrypted document service rollback and export flows`

Added:

`tests/CareNest.UnitTests/DocumentServiceTests.cs`

Coverage:

- successful import metadata/audit;
- DB save failure removes encrypted payload;
- audit failure removes DB record + encrypted payload;
- explicit export constrains original filename to safe leaf path;
- explicit export audits `Exported` action;
- existing document delete removes DB record + encrypted payload;
- missing document delete is idempotent.

## 15. Backup reminder coordinator tests

Commit:

`067076425cba56471117465f5d94acb12bb6693a` — `test: cover backup reminder scheduling and permission flows`

Added:

`tests/CareNest.UnitTests/BackupReminderCoordinatorTests.cs`

Coverage:

- disabled backup reminder cancels registration;
- denied permission without explicit prompt → no schedule;
- explicit prompt still denied → no schedule;
- no previous backup → schedules from current UTC + configured days;
- overdue backup → schedules near future instead of past;
- sound/vibration preferences respected.

## 16. Simplify denied backup-permission regression

Commit:

`b6eb37b0475139fe7a2b0e588a97a7e998d8828d` — `test: simplify denied backup permission regression`

Refined the permission-denied test to use the reusable notification spy cleanly rather than hiding base members in a derived helper.

## 17. Clear document master-key copy after vault operations

Commit:

`6ad32950b60ac6e439bcb1634603e4b8d4fd63ee` — `security: clear document master key after vault operations`

Updated:

`src/CareNest.Infrastructure/Documents/EncryptedDocumentStore.cs`

Behavior:

- caller-owned 32-byte key copy retrieved from `ISecretStore` is zeroed in `finally` after import;
- caller-owned key copy is zeroed after export;
- invalid retrieved key copy is zeroed before generating a replacement.

## 18. Clear generated key if secure-store persistence fails

Commit:

`0dc941449661cb39b90de8163b73f05b22bbcdbf` — `security: clear generated document key when persistence fails`

If a newly generated document key cannot be persisted to secure storage, its caller-owned buffer is zeroed before the failure propagates.

## 19. Verify document key-buffer clearing

Commit:

`93b70421bc231dbee2740808caea45fb7264dfe1` — `test: verify document master key buffers are cleared`

Updated:

`tests/CareNest.IntegrationTests/EncryptedDocumentStoreTests.cs`

Adds a tracking `ISecretStore` to prove caller-owned key buffers passed into/out of the abstraction are zeroed by CareNest after import/export.

## 20. Make profile cleanup assertion explicit

Commit:

`fe4b1c8780e3c4d431bba0da3a9180502ed59ad4` — `test: make profile cleanup assertion explicit`

Refined the profile cleanup assertion before exact-head verification.

## 21. Expose infrastructure internals to integration tests

Commit:

`53b61862b4661459e12bec7967d76065c4412b85` — `test: expose infrastructure internals to integration tests`

Added:

`src/CareNest.Infrastructure/Properties/AssemblyInfo.cs`

with `InternalsVisibleTo("CareNest.IntegrationTests")` so the strict archive validator and shared crypto framing can be tested directly without making them public runtime APIs.

## 22. Add strict backup archive topology validator

Commit:

`1d67e883608bdfb95237bf994a89533bed38b52f` — `security: validate encrypted backup archive topology`

Added:

`src/CareNest.Infrastructure/Backup/BackupArchiveValidator.cs`

Allowed file topology after authenticated decryption:

```text
manifest.json
database/carenest.db
secrets/document-master-key.bin
documents/<top-level-name>.cndoc
```

Rules:

- package format version must match supported CareNest format;
- schema version must be positive;
- document count must be non-negative;
- duplicate file entries rejected;
- manifest required;
- database required;
- unexpected files rejected;
- nested document paths rejected;
- non-`.cndoc` document files rejected;
- manifest document count must equal actual document entries;
- document-bearing backup requires valid 32-byte document key;
- any present document-key entry must be exactly 32 bytes.

## 23. Portable archive-separator validation

Commit:

`20a3d35b604a989d86b9f46a73e8db0f3c8a0cfc` — `fix: use portable backup entry separator checks`

Ensures archive topology validation is cross-platform and explicitly rejects both slash/backslash nesting without relying on a platform-specific overload.

## 24. Enforce strict backup archive/key cleanup in production service

Commit:

`3b4d28e77f01e297420dd7582b10b2d749ac6204` — `security: enforce strict backup archive and key cleanup`

Updated:

`src/CareNest.Infrastructure/Backup/EncryptedBackupService.cs`

Behavior:

- `InspectAsync` validates strict archive topology after authenticated decryption;
- restore validates topology before extraction;
- path containment remains a defense-in-depth extraction control;
- backup creation clears copied document-master-key buffer after use;
- PBKDF2-derived AES key is zeroed after encrypt/decrypt paths;
- random salt buffer is zeroed after encrypt/decrypt paths;
- old/restored document-key copies are zeroed after restore handling;
- document-bearing backup cannot be created/restored without the required valid key material.

## 25. Strict backup-topology integration tests

Commit:

`61e83a977ea607c310701bce7e767d12d8bb142b` — `test: cover strict encrypted backup archive topology`

Added:

`tests/CareNest.IntegrationTests/BackupArchiveValidatorTests.cs`

Coverage:

- valid archive with no docs;
- valid archive with document + 32-byte key;
- nested document entry rejected;
- non-`.cndoc` entry rejected;
- unexpected file rejected;
- duplicate file entry rejected;
- manifest count mismatch rejected;
- missing document key rejected when documents exist;
- invalid key length rejected;
- invalid schema version rejected;
- negative document count rejected.

## 26. Expose copied secret buffers in integration test infrastructure

Commit:

`26f9c9ed9cb020f46ff8fd0ce0a62d4371c6f99c` — `test: expose copied secret buffers for hygiene assertions`

Updated:

`tests/CareNest.IntegrationTests/TestInfrastructure.cs`

The in-memory secret store retains references to the caller-owned copies it returns/receives so integration tests can assert CareNest zeroed those specific buffers while independently retaining its own stored copy.

## 27. Verify backup clears copied document key

Commit:

`8e2607f287ca5777d9edbab445042f96c6bcfcec` — `test: verify backup clears copied document key buffer`

Updated:

`tests/CareNest.IntegrationTests/EncryptedBackupTests.cs`

This became the first exact source head for PR #31.

Coverage proves backup creation clears the caller-owned document-master-key copy after it has been used to create the protected portable backup.

---

# PR #31 — superseded verification that exposed CA1861

Exact source:

`8e2607f287ca5777d9edbab445042f96c6bcfcec`

Verification marker:

`f98c3cc3458e0e42b6336111b7fc4f400ec75d92`

PR:

`#31 — Verify CareNest service, document, and backup hardening`

Marker-only diff:

`build/verification/rc1-service-backup-hardening-20260813.txt`

Result:

- platform-neutral formatting passed;
- unit-test compilation exposed CA1861 in a new constant-array assertion in `ProfileServiceTests`;
- the analyzer finding was treated as a real quality-gate failure;
- no analyzer suppression was added;
- PR #31 was closed without merge;
- marker never entered `main`;
- PR #31 is **not** release evidence.

## CA1861 correction

Commit:

`8a28bbf30692b2b0e98ec801dac1531d50d65db1` — `fix: satisfy static array analyzer in profile cleanup test`

The expected cleanup array became a static test field rather than allocating a constant array inside the assertion.

This corrected source became the PR #32 verification base.

---

# PR #32 — green service/document/backup baseline

Exact source:

`8a28bbf30692b2b0e98ec801dac1531d50d65db1`

Marker:

`aa751f8f84cc2ef3fa0dd93bfcd8db9e5d2288d4`

PR:

`#32 — Reverify CareNest service, document, and backup hardening`

Marker-only file:

`build/verification/rc1-service-backup-hardening-20260813-2.txt`

Results:

- CareNest CI #326 / `31690726676`: **success**;
- formatting: **success**;
- unit tests: **106 passed, 0 failed, 0 skipped**;
- integration tests: **26 passed, 0 failed, 0 skipped**;
- UI-contract/policy tests: **54 passed, 0 failed, 0 skipped**;
- total core tests: **186 passed**;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- CodeQL #326 / `31690726675`: **success**;
- Dependency Audit #12 / `31690726700`: **success**.

PR #32 was closed without merge after success. Its marker did not enter production source.

The open SQLitePCLRaw advisory remained open; a green Dependency Audit does not mean remediation occurred.

---

# Deeper cryptographic audit and authenticated stream v2

After the PR #32 source was green, a deeper review of the shared chunked authenticated-encryption format identified a meaningful integrity hardening opportunity.

Legacy framing v1 authenticated each data chunk, but the final zero-length marker was not itself authenticated. A valid authenticated prefix ending exactly at a chunk boundary could therefore theoretically be presented as complete if a parser accepted a zero-length terminator at that point.

Rather than remove compatibility with existing CareNest encrypted data, the solution was:

- write new streams as framing **v2**;
- authenticate the terminal record in v2;
- keep legacy v1 decryption support;
- explicitly document that old v1 ciphertext is not retroactively upgraded.

## 28. Authenticate chunked AEAD stream termination

Commit:

`f1df67def1bb4ac311eeed3bd9f9661ebad772cc` — `security: authenticate chunked AEAD stream termination`

Updated:

`src/CareNest.Infrastructure/Security/ChunkedAead.cs`

New write format:

- magic bytes;
- framing version `2`;
- 12-byte base nonce;
- zero or more authenticated data chunk records;
- authenticated terminal record.

Each data chunk binds through AAD:

- stable application context prefix;
- chunk counter;
- plaintext chunk length.

V2 terminal:

- 4-byte zero length;
- 16-byte AES-GCM authentication tag;
- tag is computed with the **next** chunk counter and length `0` over empty plaintext.

Security effect:

A valid prefix cannot be accepted as a complete v2 stream merely by ending on an authenticated chunk boundary unless the terminator tag for that exact next counter is also valid.

Reader behavior:

- supports v1 and v2;
- validates expected magic;
- validates supported version;
- validates chunk length;
- verifies each AES-GCM chunk;
- verifies v2 terminal tag;
- rejects trailing bytes after terminal;
- checks counter overflow;
- requires a 32-byte AES-256 key.

Memory hygiene:

Known mutable buffers are zeroed where practical, including base nonce, plaintext chunk buffer, ciphertext chunk buffer, data tag, terminal tag, per-record nonce, AAD, header/magic/length buffers.

## 29. Mark new encrypted documents as stream format v2

Commit:

`4b3a7984dfdf590373e29cee0b3e40ae7fc5641e` — `security: mark new encrypted documents with stream format v2`

Updated:

`src/CareNest.Infrastructure/Documents/EncryptedDocumentStore.cs`

New document imports now store:

`EncryptionVersion = 2`

Existing v1 encrypted document files remain readable and are not automatically rewritten merely because the app is upgraded.

## 30. Direct authenticated-stream v2 + legacy v1 tests

Commit:

`5a1de4260bb3579b0d8dcef289beb4175dd369b3` — `test: cover authenticated stream termination and v1 compatibility`

Added:

`tests/CareNest.IntegrationTests/ChunkedAeadTests.cs`

Coverage:

- v2 multi-chunk round-trip;
- output header records version 2;
- chunk-boundary prefix truncation is rejected because a terminal tag from a later counter cannot authenticate at the earlier counter;
- trailing data after valid terminal is rejected;
- handcrafted legacy v1 stream remains decryptable.

## 31. Avoid constant-array allocations in legacy fixture

Commit:

`2469445dcb8551a54f99fb092d69837a2de15af3` — `test: avoid constant array allocations in legacy stream fixture`

Refined the v1 compatibility fixture to avoid triggering the same constant-array allocation analyzer family seen during PR #31.

## 32. Require new encrypted document metadata version 2

Commit:

`4f5f9abe9d702fa33d6aba3f15c113febfebf95e` — `test: require encrypted document stream format v2`

Updated:

`tests/CareNest.IntegrationTests/EncryptedDocumentStoreTests.cs`

The integration suite now explicitly requires new imports to report `EncryptionVersion == 2` while preserving read compatibility for v1 framing through the direct shared framing test.

This commit became the exact source for PR #33.

---

# PR #33 — current fully green exact automated runtime/test baseline

Verification branch:

`ci/carenest-rc1-aead-v2-hardening-20260813`

Exact runtime/test source:

`4f5f9abe9d702fa33d6aba3f15c113febfebf95e`

Verification marker head:

`62a0050a2622e12a31d00842778af0bc96355482`

Marker file:

`build/verification/rc1-aead-v2-hardening-20260813.txt`

Pull request:

`#33 — Verify CareNest authenticated stream v2 hardening`

The PR contained only the verification marker beyond the exact source head and was closed without merge after all gates passed.

## CareNest CI #332

Run:

`31691592300`

Conclusion:

**success**

Core evidence:

- platform-neutral formatting: **success**;
- `CareNest.UnitTests`: **106 passed, 0 failed, 0 skipped**;
- `CareNest.IntegrationTests`: **30 passed, 0 failed, 0 skipped**;
- `CareNest.UiTests`: **54 passed, 0 failed, 0 skipped**;
- total core tests: **190 passed, 0 failed, 0 skipped**.

Platform evidence:

- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**.

## CodeQL #332

Run:

`31691592435`

Conclusion:

**success**

## Dependency Audit #13

Run:

`31691592302`

Conclusion:

**success**

Important dependency statement:

The successful Dependency Audit does **not** close `GHSA-2m69-gcr7-jv3q`. The exact advisory remains narrowly suppressed so unrelated failures stay visible. `docs/security/DEPENDENCY_RISK_REGISTER.md` remains the authoritative open-risk record.

## Current automated baseline

PR #33 supersedes PR #30 and PR #32 as the latest exact automated runtime/test source baseline.

Current counts:

- unit: 106;
- integration: 30;
- UI-contract/policy: 54;
- total: 190.

---

# Cryptographic version distinction

The repository now explicitly distinguishes several version layers.

## Backup package/application format

The versioned CareNest backup package/manifest format is governed separately by `AppConstants.BackupFormatVersion`.

## Backup outer encryption header

The outer CareNest backup recognizer/header has its own version boundary.

## Chunked AEAD stream framing

The internal shared streaming encryption framing is now:

- v1: legacy readable format;
- v2: current new-write format with authenticated terminal record.

These are intentionally not conflated.

## Stable AAD context strings

Strings such as `CareNest.Document.v1` are stable application-context AAD labels, not a declaration that the chunked stream framing is still version 1.

---

# V1 compatibility boundary

CareNest does not claim historical v1 ciphertext has magically received v2 protection.

Current compatibility policy:

- all new shared encrypted streams use framing v2;
- existing framing-v1 encrypted document/backup streams remain readable;
- v1 is retained to avoid making existing user data inaccessible;
- v1 ciphertext is not automatically rewritten in the background;
- removing v1 support requires an explicit migration/deprecation/security/recovery review;
- canonical historical fixtures should be preserved/used before production deprecation decisions.

Automated evidence currently includes a handcrafted v1-compatible stream fixture.

Manual/release evidence still worth obtaining:

- a canonical encrypted document created by a previously verified/released v1 build using synthetic data;
- a canonical encrypted backup created by a previously verified/released v1 path using synthetic data;
- successful read/restore of those exact historical fixtures in the intended production package.

---

# Sensitive-memory boundary

CareNest now explicitly clears known application-owned mutable buffers where practical.

Covered examples:

- app-lock derived verifier;
- app-lock retrieved verifier;
- retrieved document-master-key copy;
- generated document key if persistence fails;
- backup copied document key;
- backup old/restored document-key copies;
- PBKDF2-derived backup AES key;
- backup random salt;
- chunked-AEAD plaintext buffer;
- chunked-AEAD ciphertext buffer;
- authentication tags;
- per-record nonce;
- AAD buffer;
- terminal tag.

Limitations remain explicit:

`CryptographicOperations.ZeroMemory` reduces lifetime of known mutable arrays owned by CareNest. It does not prove erasure of copies inside the garbage-collected runtime, platform secure store, OS, swap, crash dumps, hardware caches, or a compromised process/device.

---

# Strict backup topology boundary

After authenticated decryption, a backup archive is validated against exactly what the restore implementation consumes.

Allowed file layout:

```text
manifest.json
database/carenest.db
secrets/document-master-key.bin
documents/<top-level-name>.cndoc
```

`secrets/document-master-key.bin` may be absent for a no-document backup, but if present must be 32 bytes. If documents exist, the 32-byte key is required.

Rejected layouts include:

- duplicate file entries;
- missing manifest;
- missing database;
- unsupported package format;
- invalid/non-positive schema version;
- negative document count;
- unexpected files;
- nested document files;
- backslash/slash nested document paths;
- non-`.cndoc` document files;
- document-count mismatch;
- document-bearing archive without required key;
- invalid-length document key.

Extraction also retains the existing full-path containment validation as defense in depth.

---

# Application-service test coverage added

The platform-neutral unit suite now directly tests major application orchestration instead of relying only on UI/source contracts and integration persistence tests.

## ProfileService

- create/update audit distinction;
- deterministic UTC touch time;
- cascading profile deletion coordination;
- encrypted document cleanup;
- profile-photo encrypted cleanup;
- deletion audit.

## MedicineService

- create/update audit distinction;
- reminder rebuild after medicine save;
- schedule save;
- future occurrence invalidation;
- reminder rebuild after schedule change;
- explicit user/repository stock arithmetic;
- negative estimated stock rejection;
- cascade delete/rebuild.

## AppointmentService

- explicit UTC start;
- due-time calculation;
- create audit;
- denied notification permission;
- granted permission after explicit request;
- rebuild without prompt while denied;
- stored non-UTC fail-closed behavior;
- cancellation before delete.

## DocumentService

- encrypted import metadata;
- DB save failure rollback;
- audit failure rollback;
- cleanup failure surfacing;
- safe export leaf filename;
- export audit;
- existing delete;
- idempotent missing delete.

## BackupReminderCoordinator

- disabled state cancellation;
- denied permission behavior;
- no background permission prompt;
- scheduling from current/last backup time;
- overdue correction to near-future;
- sound/vibration preferences.

---

# Post-PR33 documentation alignment

After exact source `4f5f9abe...` was fully green, `main` was advanced only with documentation so runtime/test evidence remains exact and attributable.

## Documentation commits after PR #33 source

1. `ecd190f28fd5e07ab2cdb1d17e14d825f30f986d` — `docs: expand test plan for service backup and AEAD hardening`
2. `8340797afe8ec56f5f837f5bb2f1f1a1ca207491` — `docs: promote 190-test CareNest verification baseline`
3. `7a809eb3443056d2df0b6f9cf3711bce3cc6b24b` — `docs: document appointment UTC and permission fail-safe behavior`
4. `6d7b56787b9f89258b0a7c2592b7893ee1e65fae` — `docs: document document-vault v2 framing and rollback guarantees`
5. `dace7f371daa65480e0bbdb712b91303ba99aca4` — `docs: document strict backup topology and authenticated framing v2`
6. `1d08a3c4f6310880815f5e3c50d46a39772f4f45` — `docs: align security model with AEAD v2 and rollback hardening`
7. `afa1cd5d2579853e737df0f7bf3a6eb2464d9422` — `docs: expand threat model for stream truncation and backup topology`
8. `5299d5337f9ba44168bb833d228b7fdf8a7acd02` — `docs: record appointment rollback backup and AEAD v2 decisions`
9. `5384b3472bc0119506c0109a4514eda408a9da95` — `docs: add service backup and AEAD v2 production quality gates`
10. `010b675d5fb83c4e1f498be3708815f2a235414b` — `docs: expand security release review for v2 encryption hardening`
11. `705263fd169dcabc7b1c606d7c825581acb8666c` — `docs: record PR33 190-test release verification evidence`
12. `075dabf7a3e68531a8f4f0740e6118d30ea10bd4` — `docs: promote PR33 service backup and AEAD v2 baseline`
13. `4c184d0ab5453522fffa8b9573b7cb6da66262ae` — `docs: record service backup and authenticated-stream hardening`
14. `341f0e3081d71f430307a945eb3bb8b603db50be` — `docs: publish 190-test service backup and crypto baseline`
15. `985204bcc01992622971c10af9a0164ce63fad9e` — `docs: promote PR33 baseline in documentation hub`
16. `ff318bed42400f9bb53a4ea4d6bd8815f4a0feaf` — `docs: advance roadmap after PR33 crypto and service hardening`
17. `d9271e24aa8b9884fc99ade11636a16e77aad9ea` — `docs: preserve complete 2026-08-12 CareNest handoff`

The preservation commit adds only the exact prior handoff blob under `docs/history/`.

---

# Documentation aligned to current verified source

The following current files now describe the new runtime truth:

- `docs/testing/TEST_PLAN.md`;
- `docs/testing/TESTING_GUIDE.md`;
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`;
- `docs/architecture/DOCUMENT_VAULT.md`;
- `docs/architecture/BACKUP_AND_RESTORE.md`;
- `docs/security/SECURITY_MODEL.md`;
- `docs/security/THREAT_MODEL.md`;
- `DECISIONS.md`;
- `docs/releases/QUALITY_GATE.md`;
- `docs/releases/SECURITY_RELEASE_REVIEW.md`;
- `docs/releases/RELEASE_CHECKLIST.md`;
- `PROJECT_STATUS.md`;
- `CHANGELOG.md`;
- `README.md`;
- `docs/README.md`;
- `docs/releases/NEXT_STEPS.md`;
- this `what_changed.md`.

---

# New architectural/security decisions recorded

`DECISIONS.md` now includes:

## Decision 29 — appointment UTC and permission fail-safe

- `StartsUtc` requires actual UTC kind;
- local/unspecified ticks are rejected, not relabeled;
- notification permission denial stops scheduling;
- background rebuild does not repeatedly prompt while denied.

## Decision 30 — document import compensating rollback

- encrypted payload, DB metadata, and audit span multiple persistence surfaces;
- DB save failure removes payload;
- audit failure after metadata save removes both DB record and payload;
- rollback cleanup is non-cancelled;
- incomplete rollback is surfaced.

## Decision 31 — clear caller-owned cryptographic buffers

- mutable verifier/key/salt/work buffers are zeroed where practical;
- no claim of total OS/runtime erasure.

## Decision 32 — strict backup topology before extraction

- restore accepts only the exact expected file layout;
- duplicate/nested/unexpected/count/key-invalid layouts fail;
- path containment remains defense in depth.

## Decision 33 — authenticated stream v2 + v1 compatibility

- new streams use authenticated terminal v2;
- v2 rejects prefix truncation/trailing data;
- v1 remains readable;
- v1 data is not falsely represented as retroactively upgraded;
- removing v1 requires explicit migration/deprecation review.

---

# Open SQLite dependency risk remains unchanged

Tracked advisory:

`GHSA-2m69-gcr7-jv3q`

Current affected path:

SQLitePCLRaw native `2.1.11` through the current sqlite-net-pcl dependency graph.

Current truthful state:

- exact advisory is narrowly present in `NuGetAuditSuppress`;
- no wildcard/severity-wide suppression exists;
- the suppression is not remediation;
- `docs/security/DEPENDENCY_RISK_REGISTER.md` marks the risk open;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` defines the migration/upgrade regression gate;
- final production release needs an explicit acceptable resolution/decision.

The current continuation did not guess or invent an unavailable patched package version.

---

# Current production blockers

Automated source hardening is green, but final public `1.0.0` remains intentionally blocked.

## Manual platform/device behavior

- Android phone/emulator matrix;
- Windows matrix;
- iPhone/iPad matrix;
- Mac Catalyst matrix;
- notification permission denied/granted;
- appointment notification permission denied/granted;
- real notification delivery limitations;
- Android exact/inexact alarm behavior;
- Android battery optimization behavior;
- Android reboot/time/time-zone behavior;
- real snooze behavior against platform scheduling;
- document import/export/delete;
- calendar export;
- app-lock cold start;
- encrypted backup/restore on clean installation.

## Encrypted-data compatibility

- packaged-target test of new v2 document import/export;
- packaged-target test of new v2 backup create/inspect/restore;
- canonical historical v1 encrypted-document fixture verification when available;
- canonical historical v1 backup fixture verification when available;
- do not remove v1 support before explicit migration/deprecation evidence.

## Accessibility

- screen reader;
- large text/text scaling;
- keyboard/focus;
- contrast/themes;
- reduced motion.

## Distribution

- current Apple App Store rule review for voluntary external project-support link;
- current Google Play rule review for voluntary external project-support link;
- signing identities/credentials outside Git;
- signed package creation/inspection;
- package identities/version/build metadata;
- screenshots with fictional data only;
- store listing text;
- privacy/data-safety disclosures.

## Security/release evidence

- explicit SQLitePCLRaw advisory resolution/decision;
- final security review for exact production candidate;
- final `CareNest Release Evidence` run for the exact promoted commit;
- final release notes/tag/GitHub release only after applicable blockers clear.

No item above is marked complete merely because automated tests are green.

---

# Deferred future scope remains unchanged

The following remain deliberately outside current v1 and require new privacy/security/threat/consent architecture before implementation:

- cloud synchronization;
- remote caregiver collaboration;
- accounts;
- mobile-number authentication;
- server-side storage;
- automatic remote sharing;
- analytics/telemetry;
- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- medication-interaction claims;
- clinical risk scoring.

The non-clinical boundary is not a temporary defect to remove.

---

# Git identity note

Requested maintainer identity for local Git commits:

```bash
git config user.name "Sanskar"
git config user.email "sanskarin@outlook.in"
```

Repository setup/documentation includes this identity in the local setup workflow.

The connected GitHub APIs used in this conversation do not expose arbitrary author/committer-email parameters through the available write operations. Connector-created commits therefore use the authenticated GitHub identity. The repository does not falsely claim those API commits were forced to `sanskarin@outlook.in`.

Local/future Git commits can use the requested email through the provided setup scripts.

---

# Local environment limitation

The local execution environment used for repository assembly does not provide a complete .NET MAUI host with all platform workloads/device tooling.

Therefore the repository does not falsely claim local execution of:

- MAUI restore/build for all targets;
- emulator/device smoke testing;
- signing;
- store packaging.

GitHub-hosted Actions are the authoritative automated evidence for the exact source baselines recorded above.

---

# Current exact repository interpretation

- CareNest remains `1.0.0-rc.1`.
- Complete runtime/test source is on `main`.
- Latest exact automated runtime/test source is `4f5f9abe9d702fa33d6aba3f15c113febfebf95e`.
- PR #33 is the current exact automated source baseline.
- PR #33 marker `62a0050a2622e12a31d00842778af0bc96355482` was closed without merge.
- CareNest CI #332 / `31691592300`: success.
- Unit tests: 106 passed.
- Integration tests: 30 passed.
- UI-contract/policy tests: 54 passed.
- Total core tests: 190 passed.
- Android Release: success.
- Windows Release: success.
- iOS simulator Release: success.
- Mac Catalyst Release: success.
- CodeQL #332 / `31691592435`: success.
- Dependency Audit #13 / `31691592302`: success.
- Appointment UTC and permission fail-safe behavior is implemented/tested.
- Direct application-service tests are implemented.
- Document import rollback consistency is implemented/tested.
- Document/backup key-buffer hygiene is implemented/tested.
- Strict backup archive topology is implemented/tested.
- New encrypted streams use authenticated-terminal framing v2.
- V2 prefix truncation/trailing data are rejected and tested.
- Legacy v1 encrypted streams remain readable and tested.
- Existing v1 ciphertext is not represented as retroactively upgraded.
- New encrypted document metadata records stream version 2.
- The SQLitePCLRaw advisory remains open and is not claimed fixed.
- Manual/accessibility/store/signing/final Release Evidence work remains open.
- Earlier complete Phase 0–8 and 2026-08-12 documentation handoffs are preserved under `docs/history/`.

---

# Documentation-only boundary after PR #33

Before this active handoff update, exact verified source `4f5f9abe9d702fa33d6aba3f15c113febfebf95e` was compared to documentation head `ff318bed42400f9bb53a4ea4d6bd8815f4a0feaf`.

Result:

- status: ahead;
- 16 commits after the verified runtime/test source;
- all changed files were Markdown documentation;
- no C# runtime source changed;
- no XAML changed;
- no test source changed;
- no project/solution/package file changed;
- no workflow changed;
- no platform source changed;
- no runtime resource changed.

The later `d9271e24...` preservation commit also adds only a Markdown historical handoff using the exact prior blob.

This `what_changed.md` update is itself documentation-only. A final compare from `4f5f9abe...` to the resulting `main` head is performed after this commit so the documentation-only boundary can be confirmed through the final handoff state.
