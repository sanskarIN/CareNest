# CareNest Data Lifecycle

CareNest v1 is local-first. This document follows data from user entry/import through local use, explicit export/backup, and deletion.

For the complete privacy architecture see `docs/privacy/PRIVACY_MODEL.md` and `docs/architecture/DATA_STORAGE_AND_EXPORT.md`.

## 1. Collection

CareNest stores information that the user explicitly enters/imports through product workflows.

Examples:

- local profile information;
- emergency contacts;
- medicine names;
- opaque strength/instruction text;
- reminder schedule values;
- reminder outcomes/log entries;
- appointments/notes;
- imported documents;
- tags/folders;
- stock/refill values;
- preferences/settings.

CareNest does not infer a dose/treatment plan from medicine text.

## 2. Initial storage

Structured data:

- SQLite inside the application data/sandbox area.

Sensitive imported document payloads:

- encrypted document-vault storage using authenticated encryption.

App-lock/document-key secret material:

- platform secure secret storage.

CareNest does not claim transparent whole-database encryption for SQLite.

## 3. Local processing/use

Local data is used for:

- family/profile organization;
- deterministic reminder occurrence generation;
- reminder state/history;
- appointment organization;
- document organization;
- stock/refill estimates based on user-entered values;
- local reports/exports;
- manual encrypted backups;
- privacy-safe diagnostics.

No required CareNest backend is involved in normal v1 processing.

## 4. Reminder materialization

User-entered schedule records can be materialized into future local reminder occurrences.

The planner:

- validates profile/medicine/schedule/time ownership;
- requires UTC planning windows;
- uses explicit schedule time zone;
- respects dates/states;
- handles DST deterministically;
- creates no automatic as-needed reminders.

OS notification delivery is a separate platform boundary and can be affected by permission/device policy.

## 5. Notification registration

Eligible future occurrences can be registered with the platform notification system.

Notification content is privacy-minimized/generic by default.

The operating system may store/display notification data according to OS/device policy.

## 6. Logs/diagnostics

CareNest uses redacted operational diagnostics.

Routine logs are not intended to contain:

- health-document contents;
- backup passwords;
- app-lock PINs;
- encryption keys;
- raw health free text;
- full exception messages/stack traces from sensitive operation paths.

See `docs/security/LOGGING_PRIVACY.md`.

## 7. Explicit document export/share

When a user explicitly exports/shares a document:

1. CareNest accesses/decrypts the protected local payload;
2. creates/hands off the requested export copy;
3. platform file/share APIs transfer it to the user-selected destination;
4. the copy is outside CareNest encrypted-vault protection.

CareNest cannot control retention/security of the destination copy.

## 8. Explicit report/profile export

CareNest can create user-controlled outputs such as:

- JSON profile export;
- PDF summaries;
- CSV reports.

These files can contain sensitive plaintext organizational data.

After saving/sharing, destination protections apply.

## 9. Explicit calendar export

Appointment information can be explicitly handed to an OS/third-party calendar system.

If that calendar syncs to a remote provider, the remote copy is outside the CareNest local-first boundary.

## 10. External links

Repository, policy, creator, and voluntary project-support links open only after explicit user action.

CareNest does not intentionally attach/transmit local profiles, medicine data, documents, backups, reminder history, or app-lock data to those fixed links.

Canonical voluntary support URL:

`https://buymeacoffee.com/sanskarIN`

External services may use cookies/accounts/payment/retention practices governed by their own policies.

## 11. Manual backup creation

User explicitly selects backup action/password/destination.

CareNest:

- checkpoints/snapshots SQLite;
- packages required portable recovery state;
- derives a key from user password;
- encrypts/authenticates the protected backup payload;
- writes the backup to the selected destination.

The backup file leaves normal application-only storage once saved externally.

## 12. Backup retention

CareNest does not automatically manage every external backup copy.

Users must control:

- destination access;
- retention period;
- old backup deletion;
- password storage;
- cloud-drive/device backup behavior.

Deleting CareNest local data does not automatically delete backup files stored elsewhere.

## 13. Restore

Restore is explicit.

The application validates format/version/authentication before accepting protected content.

Wrong-password/tampered/unsupported data is rejected.

After successful restore, local structured/encrypted state is recreated and derived reminder/runtime registrations are rebuilt as needed.

## 14. Profile archive

Archive preserves local data but changes lifecycle behavior, including suppressing applicable automatic reminders.

Archive is not deletion and must not be presented as deletion.

## 15. Profile/record deletion

Explicit deletion removes intended current local records according to repository/workflow relationship rules.

Related encrypted files must also be cleaned by the application workflow where appropriate.

SQLite deleted/free pages may physically persist until database/OS cleanup; CareNest should not promise forensic secure erasure beyond implemented behavior.

## 16. Document deletion

Deleting a CareNest document removes the intended metadata/protected file through the application workflow.

Copies that the user previously exported remain at their external destinations until separately removed.

## 17. Full local reset/uninstall

Reset/uninstall can remove application-owned current local storage according to platform behavior.

It does not guarantee deletion of:

- user-created external backups;
- exported reports/documents;
- calendar-provider copies;
- screenshots;
- OS/device/cloud backups outside CareNest control.

## 18. OS-level copies

Operating systems/device-management tools can independently create backups/caches/previews according to system configuration.

CareNest's local-first statement does not mean the operating system never backs up application data.

Store/privacy disclosures should account for the exact platform configuration of the shipping build.

## 19. Screenshots/screen recording

Information visible on screen can be captured by screenshots/screen recording/accessibility tooling according to OS/device state.

CareNest cannot guarantee protection against a compromised/untrusted device environment.

## 20. Development/test data

Developers should use synthetic/fictional data.

Never commit:

- real user databases;
- health documents;
- backups;
- PINs/passwords;
- private signing credentials.

## 21. Future networked features

Current lifecycle deliberately contains no automatic CareNest cloud-upload/sync stage.

If future versions add accounts/sync/remote caregiver access, the lifecycle must be redesigned/documented for:

- collection purpose;
- remote transfer;
- authentication/authorization;
- consent/revocation;
- encryption/key ownership;
- server storage/retention;
- deletion/export;
- conflict handling;
- breach/incident response;
- store privacy disclosures.

## Lifecycle diagram

```text
User entry/import
   |
   v
Local SQLite + encrypted document storage + secure secrets
   |
   +--> local reminder/report/organization processing
   |
   +--> explicit document/report/profile/calendar export ----> external destination
   |
   +--> explicit encrypted backup ---------------------------> external destination
   |
   +--> explicit external web/support link ------------------> browser/provider
   |
   +--> archive (preserve local data, change active behavior)
   |
   `--> delete/reset/uninstall (current local state)

External copies remain governed by their destinations.
```

## Related documentation

- `PRIVACY.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/security/THREAT_MODEL.md`
- `docs/security/LOGGING_PRIVACY.md`