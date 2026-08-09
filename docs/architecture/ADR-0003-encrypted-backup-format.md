# ADR-0003 — Password-encrypted portable backup format

**Status:** Accepted for 1.0.0-rc.1

CareNest backups are explicit user actions. The backup payload contains a SQLite snapshot, a manifest, encrypted `.cndoc` files, and—when documents exist—the 32-byte document master key required to decrypt those files after restoring on another installation. That key is never stored outside the password-encrypted outer backup payload.

The outer stream uses a versioned CareNest header, PBKDF2-HMAC-SHA-256 password derivation (250,000 iterations, random 16-byte salt), and chunked AES-256-GCM authenticated encryption. The current application backup **format version is 2**; the encryption envelope remains independently versioned as 1.

Restore performs authenticated decryption, ZIP path traversal checks, manifest/document-count validation, SQLite integrity validation, staging, document-key restoration, database replacement, and rollback of document/key state if database replacement fails.

CareNest never uploads a backup automatically. Losing the backup password makes the export unrecoverable by CareNest. Database-at-rest limits outside exported backups are documented separately in the threat model.
