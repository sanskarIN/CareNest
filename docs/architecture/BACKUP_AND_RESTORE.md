# CareNest Backup and Restore Architecture

CareNest v1 provides manual password-encrypted backup and restore without automatic cloud upload.

This document describes the implemented architecture and operational boundaries. It is not a cryptographic guarantee beyond the behavior actually implemented and tested.

## Goals

The backup system is designed to:

- let a user manually create a portable protected backup;
- protect backup contents with authenticated encryption;
- keep the backup format versioned;
- validate before replacing local data;
- carry the recovery material needed for CareNest-encrypted documents inside the protected backup payload;
- avoid any required CareNest server/account;
- support regression testing of snapshot integrity and wrong-password/tamper rejection.

## Non-goals

CareNest v1 does not provide:

- automatic background cloud backup;
- remote password recovery;
- server-side escrow of backup passwords/keys;
- automatic synchronization between devices;
- remote conflict resolution;
- a guarantee that a user will retain the backup file/password.

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

## Encryption model

The documented/implemented backup format uses:

- a user-supplied backup password;
- PBKDF2-HMAC-SHA256 password-based key derivation;
- authenticated AES-GCM encryption;
- format/version metadata required to recognize supported backups.

The exact format is versioned so future schema/backup changes can be handled explicitly rather than guessed.

## Password handling

The backup password is user-controlled.

CareNest does not provide a CareNest-hosted password recovery mechanism in v1.

Operational consequences:

- forgetting the password can make the backup unusable;
- storing the password with the backup can weaken protection;
- sending the password or backup through public issue trackers is unsafe;
- maintainers/support must never ask users to post real backup passwords publicly.

## Document-key portability

CareNest imported documents are encrypted locally using a per-installation key kept through platform secure storage.

A backup intended to restore encrypted documents to a clean installation needs protected recovery material for those documents. The backup architecture therefore includes the required document-recovery key material inside the password-protected backup payload rather than exposing that key in plaintext.

This is one reason the backup file itself must be treated as sensitive even though its payload is encrypted.

## Backup creation sequence

```text
User chooses backup destination/password
  -> validate request
  -> checkpoint WAL
  -> create database snapshot
  -> gather portable encrypted-document recovery payload
  -> construct versioned backup package
  -> derive key from password
  -> AES-GCM authenticate/encrypt protected content
  -> write backup file
  -> clean temporary data where applicable
```

Cancellation should be honored before/destructive stages where the implementation supports it.

## Restore validation sequence

A restore should not overwrite current local state merely because a file has a CareNest-like filename.

Validation includes, as applicable:

1. recognize backup magic/format metadata;
2. validate supported format version;
3. derive the password key;
4. authenticate/decrypt the protected payload;
5. reject wrong-password or tampered data;
6. validate expected package/schema contents;
7. stage local replacement/recovery data;
8. replace/recover only after validation succeeds;
9. rebuild derived runtime state/reminders after restore as necessary.

## Wrong-password behavior

A wrong password must fail authenticated recovery. It must not produce partially trusted plaintext that is then installed as valid local state.

Integration tests cover wrong-password rejection.

## Tamper behavior

AES-GCM authentication is intended to detect modifications to protected ciphertext/authentication data.

Integration tests cover tampered-backup rejection.

## Restore and schema versions

The SQLite schema is versioned independently through `SchemaInfo` and ordered migrations.

Current release-candidate schema version: **5**.

A restore path must not silently reinterpret unknown future versions. Future backup/schema migrations require explicit compatibility fixtures and tests.

## Rollback/failure safety

Restore should validate as much as possible before replacing current local data.

The release process requires manual clean-install backup/restore testing because filesystem permissions, platform share/picker behavior, secure-storage behavior, and packaged application identity can affect the real restore path in ways unit/integration tests cannot fully simulate.

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

## Release verification requirements

Before final public production promotion, manually verify at minimum:

- backup creation in a packaged release build;
- restore into a clean installation;
- wrong-password rejection;
- tamper rejection where practical;
- restored structured records;
- restored encrypted document usability;
- reminder/runtime rebuild after restore;
- no sensitive secret appearing in logs;
- platform picker/share destination behavior.

These items are tracked in `docs/releases/MANUAL_TEST_MATRIX.md` and `docs/releases/RELEASE_CHECKLIST.md`.

## Future compatibility work

Future releases should add stored compatibility fixtures for each supported historical backup/schema format so migration behavior is executable evidence rather than documentation alone.

Any future automatic/cloud backup feature would require a new privacy/threat model, provider trust boundary, consent model, credential strategy, deletion/retention behavior, and key/recovery design.