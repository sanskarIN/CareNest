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

- AES-GCM authenticated encryption;
- a random per-installation document-encryption key;
- platform secure secret storage for the key material.

Integration tests cover round-trip decryption and tamper rejection.

## Key storage

The document key is sensitive and should not be stored as normal plaintext SQLite/application settings.

It is kept through platform secure secret storage.

Residual risk:

- CareNest cannot guarantee protection if the OS/secure store/device is fully compromised.

## Import flow

```text
User explicitly chooses/captures file
  -> CareNest reads source bytes
  -> encrypt/authenticate payload
  -> write encrypted application-owned file
  -> save CareDocument metadata
  -> optionally link folder/tags/profile
```

The source file selected by the user may remain at its original external location. Importing into CareNest does not automatically delete the user's original copy.

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
  -> decrypt requested content
  -> create/hand off export copy
  -> platform share/file destination
```

After handoff, the exported copy is outside the CareNest encrypted-vault boundary.

CareNest cannot recall/delete a copy that another app/service has retained.

## Delete flow

Deleting a document should remove:

- intended metadata record;
- applicable tag relationships;
- encrypted application-owned payload;
- temporary application-owned copies where appropriate.

Manual release testing should check for orphaned files after destructive workflows.

Previously exported/source copies outside CareNest remain independent.

## Backup integration

A clean-install restore needs both:

- encrypted document payload data; and
- protected recovery key material.

The backup architecture carries the required document recovery key material inside the password-protected backup payload.

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
- tampered encrypted payload;
- leaked key through logging/storage error;
- leftover temporary plaintext export;
- exported copy retained by external application;
- compromised/rooted/jailbroken device;
- user original source file remaining elsewhere.

## Controls

Controls include:

- authenticated encryption;
- secure key storage;
- explicit export action;
- privacy-safe logging;
- tamper/round-trip integration tests;
- application-owned metadata/payload separation;
- encrypted backup portability;
- manual release testing of file workflows.

## Manual release tests

Before final public release verify on supported platforms:

- import synthetic file;
- open/view if supported;
- tag/folder organization;
- export/share;
- exported copy content correct;
- delete metadata + encrypted payload;
- clean restart;
- backup/restore document access;
- insufficient/low-storage behavior where practical;
- no document contents/private paths in logs.

## Future improvements

Potential future local-only enhancements can include:

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
- `docs/releases/MANUAL_TEST_MATRIX.md`