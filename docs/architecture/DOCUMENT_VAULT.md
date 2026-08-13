# CareNest Encrypted Document Vault

CareNest v1 includes an encrypted local document-organizer path for imported sensitive files. This document describes the storage/privacy/security model and operational boundaries.

## Purpose

The document vault lets users organize imported health-related files locally while avoiding storage of the original payload as ordinary unprotected application content.

CareNest treats imported files as opaque user content. It does not medically interpret their contents.

## Data split

Document handling separates:

1. **metadata** in structured SQLite records; and
2. **document payload bytes** in encrypted application-owned storage.

Conceptual model:

```text
CareDocument (SQLite metadata)
   |
   +--> encrypted payload path/id
   |
   +--> profile/folder/tag metadata

Encrypted payload file
   |
   +--> authenticated encrypted original bytes
```

## Encryption model

The document payload uses authenticated .NET cryptographic primitives.

Architecture uses:

- AES-256-GCM authenticated encryption;
- a random per-installation 32-byte document-encryption key;
- platform secure secret storage for the persisted key material;
- chunked encrypted-stream framing so large files do not require one giant plaintext/ciphertext buffer.

### Encrypted stream framing versions

New document imports use **chunked AEAD framing version 2**.

Version 2 keeps authenticated per-chunk encryption and adds an **authenticated terminal record**. The terminal tag is bound to the next chunk counter and a zero plaintext length. This prevents a valid authenticated chunk prefix from being presented as a complete v2 stream merely by ending at a chunk boundary.

The reader remains backward compatible with legacy framing version 1 so existing CareNest encrypted documents can still be opened after upgrade.

Important compatibility boundary:

- v2 improves **newly encrypted streams**;
- existing v1 ciphertext is not automatically rewritten merely because the app was upgraded;
- v1 read support is retained to avoid making previously encrypted local files inaccessible;
- a future migration that rewrites v1 payloads would require explicit transactional migration/backup/manual-device validation.

`CareDocument.EncryptionVersion` records version **2** for newly imported encrypted document payloads.

The application-level AAD label remains a stable CareNest document context label and is not the same thing as the stream-framing version number.

## Chunk integrity behavior

For v2 streams:

- each data chunk is authenticated;
- chunk counter and plaintext length are part of authenticated associated data;
- the terminal record is authenticated;
- bytes after the terminal record are rejected;
- malformed/truncated/tag-modified streams fail rather than returning a silently shortened plaintext.

Integration tests cover multi-chunk round-trip, ciphertext tampering, prefix truncation, trailing-data rejection, and legacy v1 read compatibility.

## Key storage

The document key is sensitive and is not stored as normal plaintext SQLite/application settings.

It is kept through platform secure secret storage.

When infrastructure requests a byte-array copy of the document master key from the secret-store abstraction, CareNest clears that caller-owned copy with `CryptographicOperations.ZeroMemory` after import/export or failure handling where managed-memory control permits.

If a stored key copy has an invalid length, that caller-owned buffer is cleared before a new key is generated. If persistence of a newly generated key fails, the generated caller buffer is cleared before the failure propagates.

This memory hygiene does **not** claim that CareNest can erase every copy inside the operating system, secure-storage implementation, garbage-collected runtime, swap, crash dump, or a compromised device.

Residual risk:

- CareNest cannot guarantee protection if the OS/secure store/device is fully compromised.

## Import flow

```text
User explicitly chooses/captures file
  -> CareNest reads source bytes
  -> compute local integrity metadata
  -> encrypt/authenticate payload to application-owned .cndoc file
  -> save CareDocument metadata
  -> record safe audit event
  -> optionally link folder/tags/profile
```

The source file selected by the user may remain at its original external location. Importing into CareNest does not automatically delete the user's original copy.

## Import rollback guarantee

Import crosses both encrypted filesystem storage and SQLite metadata, so failure handling is intentionally compensating/transaction-like.

Current behavior:

1. encrypted payload is created first;
2. CareDocument metadata is saved;
3. audit entry is saved;
4. if metadata save fails, the encrypted payload is removed;
5. if audit persistence fails after metadata save, CareNest attempts to remove both the just-created CareDocument record and the encrypted payload;
6. rollback cleanup uses a non-cancelled cleanup token so an already-cancelled user operation does not intentionally strand the newly created artifacts;
7. if rollback itself cannot fully clean up, the failure is surfaced as an aggregate failure rather than hiding that local cleanup was incomplete.

This prevents the normal failure path from leaving a SQLite document row that points at an encrypted payload CareNest already deleted.

Manual device testing still needs to cover low-storage/filesystem/OS interruption cases because process termination can occur outside ordinary managed exception handling.

## Camera/media/file picker boundary

The App platform layer owns user-facing picker/media interactions.

The encrypted vault/infrastructure owns protected storage.

ViewModels should not manually implement cryptography or raw filesystem persistence.

## Metadata

Metadata may include organizational fields such as:

- profile ownership;
- display/document name;
- dates/category information;
- encrypted-storage metadata;
- folder metadata;
- tag relationships.

Metadata itself can be sensitive and remains part of the local SQLite data model.

## Tags

Documents can be associated many-to-many with `Tag` through `DocumentTag` join records.

Deleting relationships should clean join rows without modifying unrelated tags/documents unexpectedly.

## Folders

Schema version 5 includes optional local folder metadata for organized documents.

Folder organization is local metadata; it does not create remote/cloud folders.

## Profile photos

Where profile photos use the encrypted document-storage path, they inherit the same local protected-payload boundary rather than being treated as arbitrary unprotected remote media.

## Open/view flow

Opening a protected document may require CareNest to produce/read a temporary decrypted representation depending on platform API behavior.

Security/privacy requirements:

- use only explicit user action;
- minimize temporary plaintext lifetime;
- do not log plaintext contents/paths unnecessarily;
- clean temporary files where the implementation owns them;
- ensure sufficient storage before creating temporary copies.

Manual target testing is required because picker/share/open APIs vary by platform.

## Export/share flow

```text
Encrypted local payload
  -> explicit user export/share
  -> authenticate/decrypt requested content
  -> create/hand off export copy
  -> platform share/file destination
```

CareNest uses only the safe leaf filename when creating its temporary explicit export path, rather than allowing the stored original filename to escape the intended temporary directory.

After handoff, the exported copy is outside the CareNest encrypted-vault boundary.

CareNest cannot recall/delete a copy that another app/service has retained.

## Delete flow

Deleting a document should remove:

- intended metadata record;
- applicable tag relationships;
- encrypted application-owned payload;
- temporary application-owned copies where appropriate.

Deleting a missing document record is treated idempotently by the application service.

Manual release testing should check for orphaned files after destructive workflows.

Previously exported/source copies outside CareNest remain independent.

## Backup integration

A clean-install restore needs both:

- encrypted document payload data; and
- protected recovery key material.

The backup architecture carries the required document recovery key material inside the password-protected backup payload.

Backup validation now also requires document-bearing archives to contain a valid 32-byte document master key and a strict top-level `.cndoc` document topology before extraction.

See `docs/architecture/BACKUP_AND_RESTORE.md`.

## Logging privacy

Never place into routine CareNest logs:

- document bytes/content;
- private document text;
- decrypted payload;
- encryption key;
- backup password;
- unnecessary private file path.

See `docs/security/LOGGING_PRIVACY.md`.

## Reports/document list

A document-list report/export can contain document metadata without embedding document payload bytes.

Any exported CSV/PDF/JSON remains outside the CareNest protected boundary after user-controlled export.

## Whole-database encryption distinction

Document encryption does not mean the entire SQLite database is encrypted.

CareNest documentation must continue to distinguish:

- encrypted document payloads;
- encrypted manual backups;
- sandbox-protected SQLite metadata.

## App-lock distinction

App lock protects access to the CareNest UI through a local PIN verifier.

It is separate from document encryption and does not replace the document-vault cryptographic protection.

## Threats considered

Relevant threats include:

- access to application files without decryption key;
- modified encrypted payload;
- encrypted-stream prefix truncation;
- trailing ciphertext/garbage after terminal;
- leaked key through logging/storage error;
- caller-owned key bytes remaining in memory longer than necessary;
- database record/encrypted-file inconsistency after partial import failure;
- leftover temporary plaintext export;
- exported copy retained by external application;
- compromised/rooted/jailbroken device;
- user original source file remaining elsewhere.

## Controls

Controls include:

- AES-256-GCM authenticated encryption;
- v2 authenticated stream termination for new writes;
- legacy v1 read compatibility without pretending old ciphertext was upgraded;
- secure key storage;
- caller-owned key-buffer clearing where practical;
- compensating import rollback;
- explicit export action;
- safe leaf filename handling for temporary export;
- privacy-safe logging;
- tamper/round-trip/truncation/trailing-data integration tests;
- application-owned metadata/payload separation;
- encrypted backup portability;
- manual release testing of file workflows.

## Automated evidence

Exact source `4f5f9abe9d702fa33d6aba3f15c113febfebf95e` passed PR #33 with:

- 106 unit tests;
- 30 integration tests;
- 54 UI-contract tests;
- all Android/Windows/iOS simulator/Mac Catalyst Release builds;
- CodeQL #332;
- Dependency Audit #13.

## Manual release tests

Before final public release verify on supported platforms:

- import synthetic file and confirm new metadata version;
- open/view a new v2 document;
- open/view a retained legacy v1 fixture when a canonical released-build fixture is available;
- tag/folder organization;
- export/share;
- exported copy content correct;
- delete metadata + encrypted payload;
- import failure/low-storage cleanup where practical;
- clean restart;
- backup/restore document access;
- insufficient/low-storage behavior where practical;
- no document contents/private paths in logs.

## Future improvements

Potential future local-only enhancements can include:

- explicit transactional migration of legacy v1 document payloads after backup/recovery testing;
- duplicate-file detection via local cryptographic hashes;
- encrypted/local thumbnail handling;
- bulk export/delete with explicit confirmation;
- cache cleanup controls.

Any OCR/content interpretation or remote document processing would require a new privacy/security/scope review and must not introduce clinical interpretation by default.

## Related documentation

- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/security/THREAT_MODEL.md`
- `docs/testing/TESTING_GUIDE.md`
- `docs/releases/MANUAL_TEST_MATRIX.md`
