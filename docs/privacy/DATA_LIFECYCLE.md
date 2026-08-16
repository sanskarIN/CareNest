# CareNest Data Lifecycle

CareNest v1 is local-first. This document follows data from explicit user entry/import through local processing, operating-system reminder registration, export/backup and deletion.

## 1. Collection

CareNest stores information users explicitly enter/import, including local profile/contact data, medicine names and opaque strength/instruction text, schedules/reminder history, appointments, documents, stock/refill values and settings.

CareNest does not infer dosage/treatment from medicine text.

## 2. Initial local storage

- Structured records → SQLite in application-owned local storage.
- Imported document payloads → authenticated encrypted document-vault storage.
- App-lock/document-key secret material → platform secure storage where applicable.
- Backup/export output → only when explicitly created by the user.

CareNest does not claim transparent whole-database encryption for SQLite.

## 3. Local processing

Local data supports:

- profile/family organization;
- deterministic reminder occurrence generation;
- reminder/log history;
- appointment organization;
- document organization;
- user-entered stock/refill estimates;
- local reports/exports;
- manual encrypted backups;
- privacy-minimized diagnostics.

No required CareNest backend is involved in normal v1 processing.

## 4. Reminder materialization

Explicit schedule data can create future local reminder occurrences after validating ownership/state/date/time-zone rules.

Operating-system notification/alarm registration is a separate platform boundary. Delivery can be affected by permission/device/OS policy.

## 5. Platform notification registration

Eligible occurrences can be registered with the target OS.

CareNest stores/reconciles persisted occurrence state separately from OS request state because those surfaces are not one transaction.

Notification content remains privacy-minimized where possible; the OS controls final display/history.

## 6. Logs/diagnostics

Routine diagnostics must not include raw health-document content, backup passwords, PINs, encryption/signing keys or unnecessary sensitive health text/exception payloads.

See `docs/security/LOGGING_PRIVACY.md`.

## 7. Document export/share

When a user explicitly exports/shares a document:

1. CareNest reads/decrypts its protected local payload;
2. creates/hands off the requested plaintext/portable copy;
3. platform file/share APIs transfer control to the selected destination;
4. the external copy is no longer protected by the CareNest document-vault boundary.

## 8. Reports/profile export

PDF/CSV/JSON and other supported exports can contain sensitive plaintext organizational data. External destination protections apply after handoff.

## 9. Calendar export

Appointment information can be explicitly transferred to an OS/third-party calendar. Any remote synchronization performed by that provider is outside CareNest's local-first boundary.

## 10. External web links

Current app can expose normal explicit repository/creator/legal/support destinations as implemented.

The distributed app does **not** contain/expose the external Buy Me a Coffee funding destination. That URL is repository-documentation-only.

CareNest does not attach local health/profile/document/reminder/backup/app-lock data to ordinary fixed external links.

## 11. Manual backup creation

User explicitly selects backup action/password/destination.

CareNest snapshots required data, packages recovery state, derives encryption material from the password, authenticates/encrypts the backup payload and writes the resulting portable file to the selected destination.

## 12. Backup retention

Users control external backup copies, passwords and destination retention. CareNest cannot automatically delete every copy stored outside its application-owned boundary.

## 13. Restore

Restore is explicit and validates format/version/authentication/topology/integrity before accepting data.

Wrong-password/tampered/truncated/malformed input is rejected. After successful restore, derived platform state such as reminders is rebuilt/reconciled as required.

## 14. Archive

Archive preserves local data but changes active behavior such as reminder eligibility. Archive is not deletion.

## 15. Record/profile deletion

Explicit deletion removes intended current CareNest-owned records/files according to repository/workflow rules and attempts related platform/request cleanup.

Copies already exported or retained by OS/device systems are not recalled automatically.

## 16. Document deletion

Deleting a CareNest document removes intended metadata/encrypted application-owned payload through the current workflow. Previous exported copies remain external.

## 17. Reset/uninstall

Full local reset/uninstall can remove CareNest-owned current local state according to platform behavior.

It does not guarantee deletion of:

- external backups;
- exported reports/documents;
- calendar-provider copies;
- screenshots/recordings;
- OS/device/cloud backups outside CareNest control.

## 18. OS-level copies

Operating systems/device management/backups/snapshots can independently retain application data according to device policy. Local-first architecture does not imply the OS never creates copies.

## 19. Development/test data

Use fictional/synthetic data. Never commit real databases/documents/backups, PINs/passwords/keys/tokens or production signing credentials.

## 20. Future networked lifecycle

Current lifecycle contains no automatic CareNest cloud upload/sync stage.

Accounts/sync/remote caregiver access would require explicit collection purpose, consent, authentication/authorization, transfer/storage/retention, encryption/key ownership, deletion/export, conflict/offline, incident-response and store-disclosure design.

## 21. Lifecycle diagram

```text
User entry/import
   |
   v
Local SQLite + encrypted document storage + secure secrets
   |
   +--> local reminder/report/organization processing
   |
   +--> OS reminder registration ----------------------------> OS boundary
   |
   +--> explicit document/report/profile/calendar export ---> external destination
   |
   +--> explicit encrypted backup ---------------------------> external destination
   |
   +--> explicit repository/legal/support web link ----------> browser/provider
   |
   +--> archive (preserve local data, change active behavior)
   |
   `--> delete/reset/uninstall (CareNest-owned current state)

External copies remain governed by their destinations.
```

## 22. Current release evidence

PR #74 verifies the current source-level local-first/privacy/repository policies with 331/331 core tests plus configured platform/store/security/dependency gates. It does not complete real-device privacy/accessibility/store review.

## Related documentation

- `PRIVACY.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/security/THREAT_MODEL.md`
- `docs/security/LOGGING_PRIVACY.md`