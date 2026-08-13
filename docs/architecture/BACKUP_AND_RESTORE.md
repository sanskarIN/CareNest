# CareNest Backup and Restore Architecture

CareNest v1 provides manual password-encrypted backup and restore without automatic cloud upload.

This document describes the implemented architecture and operational boundaries. It is not a cryptographic guarantee beyond the behavior actually implemented and tested.

## Goals

The backup system is designed to:

- let a user manually create a portable protected backup;
- protect backup contents with authenticated encryption;
- keep the backup package format versioned;
- validate archive topology before extraction/replacement;
- validate before replacing local data;
- carry the recovery material needed for CareNest-encrypted documents inside the protected backup payload;
- avoid any required CareNest server/account;
- support regression testing of snapshot integrity, wrong-password/tamper rejection, archive topology, and key-buffer handling.

## Non-goals

CareNest v1 does not provide:

- automatic background cloud backup;
- remote password recovery;
- server-side escrow of backup passwords/keys;
- automatic synchronization between devices;
- remote conflict resolution;
- a guarantee that a user will retain the backup file/password.

## Version layers

Three version concepts must not be confused:

1. **CareNest backup package format version** — application package/manifest compatibility, currently governed by `AppConstants.BackupFormatVersion`.
2. **Backup encryption header version** — the outer CareNest backup header used to recognize/decrypt the protected payload.
3. **Chunked AEAD stream framing version** — the internal streaming authenticated-encryption framing used by the protected payload.

The 2026-08-13 hardening changed **new chunked encrypted streams to framing v2** while preserving v1 decryption. It did not silently redefine the backup package/schema version.

## Data sources

A portable CareNest backup can require two categories of local state:

1. the SQLite database snapshot containing structured local records;
2. encrypted document recovery material/files required to restore imported encrypted documents.

The backup format protects the portable recovery payload with password-derived authenticated encryption.

## SQLite snapshot path

CareNest uses SQLite WAL mode. A safe backup cannot assume that copying only the main database file at an arbitrary instant will include all recently committed WAL content.

The production snapshot path therefore consumes the result of a full WAL checkpoint before creating the copied database snapshot.

Result-producing PRAGMAs are treated as result-producing operations, not as non-query commands.

Relevant behavior includes:

- `PRAGMA journal_mode = WAL` is read/validated;
- `PRAGMA busy_timeout = 5000` is read/validated;
- `PRAGMA wal_checkpoint(FULL)` result is consumed before snapshot copying.

## Snapshot automated evidence

Integration coverage verifies more than file existence:

- committed profile data is present in the copied database;
- the copied SQLite database can be opened read-only;
- `PRAGMA integrity_check` returns `ok`;
- an already-cancelled snapshot request throws cancellation;
- a pre-cancelled operation does not leave an output snapshot file.

These tests do not replace final packaged-build restore testing on real/supported targets.

## Password encryption model

The backup system uses:

- a user-supplied backup password;
- PBKDF2-HMAC-SHA256 password-based key derivation;
- a random salt;
- AES-256-GCM authenticated chunk encryption;
- CareNest magic/version metadata required to recognize supported backups.

The password-derived AES key and salt byte buffers owned by CareNest are cleared after encryption/decryption paths where managed-memory control permits.

This does not imply that every copy inside the runtime, OS, secure storage, swap, or a compromised machine can be erased by application code.

## Chunked AEAD framing v2

New backup payload encryption uses shared `ChunkedAead` framing **version 2**.

Version 2 authenticates:

- every data chunk;
- chunk counter;
- plaintext chunk length;
- a final zero-length terminal record bound to the next chunk counter.

The authenticated terminal prevents an authenticated chunk prefix from being presented as a complete v2 encrypted stream merely by terminating at a chunk boundary.

The reader also rejects trailing data after the terminal record.

### Legacy v1 compatibility

The reader continues to support framing version 1 so existing CareNest backups can remain readable.

Important limitation:

- existing v1 backup ciphertext is not retroactively rewritten or described as receiving v2 terminal authentication;
- v2 protects newly created encrypted payload streams;
- future removal of v1 support requires an explicit migration/deprecation policy and historical compatibility fixtures.

The stable CareNest AAD context label is not itself the framing version number.

## Password handling

The backup password is user-controlled.

CareNest does not provide a CareNest-hosted password recovery mechanism in v1.

Operational consequences:

- forgetting the password can make the backup unusable;
- storing the password with the backup can weaken protection;
- sending the password or backup through public issue trackers is unsafe;
- maintainers/support must never ask users to post real backup passwords publicly.

## Document-key portability

CareNest imported documents are encrypted locally using a per-installation 32-byte key kept through platform secure storage.

A backup intended to restore encrypted documents to a clean installation needs protected recovery material for those documents. The backup architecture therefore includes the required document-recovery key material inside the password-protected backup payload rather than exposing that key in plaintext.

A copied document-master-key byte array retrieved from the secret-store abstraction is cleared after backup use where managed-memory control permits. Old/restored key copies used during restore rollback/replacement are likewise cleared after the operation.

This is one reason the backup file itself must be treated as sensitive even though its payload is encrypted.

## Backup package topology

After authenticated decryption, the ZIP payload is validated **before extraction**.

Allowed file topology is intentionally narrow:

```text
manifest.json
database/carenest.db
secrets/document-master-key.bin       # optional when no documents; required/32 bytes when documents exist
documents/<top-level-name>.cndoc     # zero or more
```

The validator rejects:

- duplicate file entries;
- missing manifest;
- missing database entry;
- unsupported backup format version;
- invalid/non-positive schema version;
- negative document count;
- unexpected files;
- nested document paths;
- document entries that are not top-level `.cndoc` files;
- document count that does not match the manifest;
- document-bearing backup without a 32-byte document master key;
- a present document-key entry with invalid length.

This strict topology reduces ambiguity between what the archive contains and what the restore path actually consumes.

The extraction path still performs full-path containment checks as defense in depth.

## Backup creation sequence

```text
User chooses backup destination/password
  -> validate request
  -> checkpoint WAL
  -> create database snapshot
  -> gather top-level encrypted .cndoc payloads
  -> retrieve required document-key copy
  -> construct versioned strict backup package
  -> derive key from password
  -> encrypt package with chunked AEAD framing v2
  -> authenticate final terminal record
  -> write backup file
  -> clear caller-owned key/salt buffers where possible
  -> clean temporary data
```

If encrypted documents exist but the 32-byte document key cannot be retrieved, backup creation fails instead of creating a knowingly incomplete portable backup.

## Restore validation sequence

A restore does not overwrite current local state merely because a file has a CareNest-like filename.

Validation includes:

1. recognize backup magic/outer encryption version;
2. derive the password key;
3. authenticate/decrypt the protected payload, supporting framing v1/v2;
4. reject wrong-password, tampered, truncated-v2, or trailing-data payloads;
5. deserialize manifest;
6. validate strict package topology before extraction;
7. extract only validated entries through path-containment checks;
8. validate restored SQLite database integrity/schema presence;
9. stage restored encrypted documents;
10. validate/recover document master key;
11. replace current local documents/key/database with rollback handling;
12. clear caller-owned old/restored key buffers where practical;
13. rebuild derived runtime state/reminders after restore as necessary.

## Wrong-password behavior

A wrong password fails authenticated recovery. It must not produce partially trusted plaintext that is then installed as valid local state.

Integration tests cover wrong-password rejection.

## Tamper/truncation/trailing-data behavior

Authenticated encryption is intended to detect modification to protected ciphertext/authentication data.

For newly created framing-v2 backups, integration tests also cover:

- authenticated terminal verification;
- chunk-boundary prefix truncation rejection;
- trailing data rejection.

Legacy framing v1 remains readable for compatibility and is not represented as having the new v2 terminal property.

## Restore and schema versions

The SQLite schema is versioned independently through `SchemaInfo` and ordered migrations.

Current release-candidate schema version: **5**.

A restore path must not silently reinterpret unknown future versions. Future backup/schema migrations require explicit compatibility fixtures and tests.

## Rollback/failure safety

Restore validates as much as possible before replacing current local data.

Document files are staged before replacement. Previous document storage and document key are retained long enough to attempt rollback if replacing the database/key/documents fails.

The release process still requires manual clean-install backup/restore testing because filesystem permissions, platform share/picker behavior, secure-storage behavior, process interruption, and packaged application identity can affect the real restore path in ways unit/integration tests cannot fully simulate.

## Privacy boundary

A CareNest backup can represent a portable copy of sensitive local health-organizational information.

CareNest v1 does not automatically upload it. However, after the user saves/shares the backup, the destination controls that file.

Users should consider:

- destination encryption/security;
- cloud-drive provider behavior;
- shared-device access;
- removable-media loss;
- backup password storage;
- retention/deletion of old backups.

## Support boundary

For support/bug reports:

Do not attach real backups to public GitHub issues.

Do not include:

- backup password;
- app-lock PIN;
- encryption keys;
- health documents;
- private profile/medicine notes;
- signing credentials.

Use synthetic test data when reproducing backup bugs whenever possible.

## Automated evidence

Exact source `4f5f9abe9d702fa33d6aba3f15c113febfebf95e` passed marker-only PR #33:

- CareNest CI #332 / `31691592300`: success;
- 106 unit tests;
- 30 integration tests;
- 54 UI-contract tests;
- Android/Windows/iOS simulator/Mac Catalyst Release builds: success;
- CodeQL #332 / `31691592435`: success;
- Dependency Audit #13 / `31691592302`: success.

The Dependency Audit result does not resolve the separately tracked SQLitePCLRaw advisory.

## Release verification requirements

Before final public production promotion, manually verify at minimum:

- backup creation in a packaged release build;
- restore into a clean installation;
- wrong-password rejection;
- tamper rejection where practical;
- restored structured records;
- restored encrypted document usability;
- retained legacy v1 backup fixture compatibility once a canonical released-build fixture is available;
- new v2 backup creation/readback;
- reminder/runtime rebuild after restore;
- no sensitive secret appearing in logs;
- platform picker/share destination behavior.

These items are tracked in `docs/releases/MANUAL_TEST_MATRIX.md` and `docs/releases/RELEASE_CHECKLIST.md`.

## Future compatibility work

Future releases should add stored compatibility fixtures for each supported historical backup/schema/framing format so migration behavior is executable evidence rather than documentation alone.

Any future automatic/cloud backup feature would require a new privacy/threat model, provider trust boundary, consent model, credential strategy, deletion/retention behavior, and key/recovery design.
