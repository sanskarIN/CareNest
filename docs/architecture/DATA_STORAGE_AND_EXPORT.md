# CareNest Data Storage, Export, and Deletion Model

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

This document defines where CareNest data lives, when it is encrypted, how copies leave the application boundary and what deletion can/cannot guarantee.

## 1. Storage categories

CareNest uses multiple storage protections rather than one universal storage mechanism.

### Structured SQLite data

Examples:

- profiles/contacts;
- medicines/schedules/times;
- reminder occurrences/logs;
- appointments;
- document metadata/tags/folders;
- stock adjustments;
- settings/audit/backup metadata.

Protection: application sandbox/device/OS security plus application/repository access controls. CareNest does **not** claim transparent whole-database encryption.

### Encrypted document payloads

Imported application-owned document bytes are stored separately from metadata using authenticated encryption.

### Secure-storage material

Small secret/configuration material such as the document master key and app-lock derived material uses platform secure storage where applicable.

### Application cache/staging

CareNest can create app-owned temporary/staging files during report/document/backup operations. Cleanup is best effort and lifecycle-limited while CareNest still owns the path.

### External copies

Exports, shares, calendar entries, user-selected backups, screenshots and OS/device backups are outside CareNest-controlled storage after handoff/capture.

## 2. SQLite schema/storage boundary

SQLite stores structured organizational records and metadata, not the encrypted imported document bytes themselves.

Database design includes explicit migrations, relationships/indexes, transactions where required, WAL/snapshot/integrity behavior and repository abstraction.

Current source dependency security is unsuppressed/green, but packaged existing-data compatibility remains a separate production gate.

## 3. Document-vault boundary

Import flow:

```text
User-selected source file
  -> application/import service
  -> authenticated encryption
  -> CareNest-owned encrypted vault payload
  -> SQLite metadata/tag/folder record
```

Original external source file remains governed by its original location; CareNest does not claim to delete it.

## 4. Document export boundary

```text
Encrypted CareNest payload
  -> decrypt to controlled output/staging
  -> explicit user-selected save/share/open handoff
  -> external plaintext/portable copy
```

After handoff, CareNest cannot remotely revoke or enforce retention/security of that external copy.

## 5. Report/profile export boundary

Supported portable outputs such as CSV/PDF/JSON are created only after explicit user action.

Safety/privacy controls include:

- informational/non-clinical wording;
- formula-like spreadsheet content neutralization where applicable;
- staged/atomic final-file behavior where documented;
- cleanup of app-owned temporary output best effort;
- external destination becomes responsible after handoff.

## 6. Appointment calendar export

Calendar export transfers explicit appointment information to the OS/provider selected by the user.

That provider can synchronize/store the calendar entry under its own privacy/security policy. CareNest cannot delete every provider-side copy by deleting the local appointment.

## 7. Backup storage boundary

Manual backup flow:

```text
Local SQLite snapshot + required document recovery state
  -> validated package
  -> password-derived authenticated encryption
  -> user-selected external backup file
```

The resulting backup file is encrypted, but the user controls its destination/password/retention. CareNest has no server-side password recovery.

## 8. Restore boundary

Restore reads an external encrypted backup, validates version/authentication/topology/database/key state and stages/replaces local data through documented rollback logic.

Wrong password, tamper, truncation, trailing data or malformed topology fails closed.

## 9. App-lock data

App-lock PIN plaintext is not intended to be stored. Derived verifier/salt/enabled state uses secure storage where applicable.

App lock protects UI access but does not convert SQLite into whole-database encryption.

## 10. Reminder/OS state boundary

Reminder data exists both as persisted CareNest occurrences and as OS scheduled-request state.

OS request state is not CareNest database storage and cannot be atomically committed with SQLite. Reconciliation/cancellation/rebuild handles drift.

## 11. Logging/diagnostic data

CareNest diagnostics should avoid raw health text, document/backup contents, PIN/password/key material and unnecessary sensitive exception details.

Diagnostic exports remain subject to user-selected external-copy risk after handoff.

## 12. Local-first network boundary

Current v1 does not automatically upload ordinary health-organizer data to a CareNest backend/cloud service and does not include a hidden analytics/telemetry client.

Network/cloud features would create new storage/retention/deletion/authentication boundaries and require a new design review.

## 13. Fixed external web links

The application can explicitly open fixed normal repository/creator/privacy/terms/security/support destinations as implemented.

These actions must not attach local health/profile/document/reminder/backup/app-lock data automatically.

The external Buy Me a Coffee destination is **not** present in the current distributed application runtime/package. It exists only in repository support documentation/metadata.

## 14. Archive behavior

Archive preserves local records while changing active behavior. It is not deletion.

Examples include archived profiles/medicines becoming ineligible for automatic reminder materialization according to current rules.

## 15. Delete behavior

Explicit record deletion removes intended CareNest-owned current records/files and coordinates related reminder/platform cleanup where required.

Deletion cannot guarantee removal of external copies already exported/shared/synchronized/captured/backed up outside CareNest.

## 16. Full local reset

Full reset is intended to remove current CareNest-owned local application data such as structured database/files/settings/secure material according to the documented cleanup lifecycle.

It cannot reliably erase:

- manual backup files stored externally;
- exported reports/documents;
- calendar-provider copies;
- screenshots/screen recordings;
- OS/device/cloud backups outside CareNest control;
- forensic remnants beyond implemented/OS behavior.

## 17. Uninstall behavior

Platform uninstall can remove app-owned data according to OS behavior, but external copies and OS/device backups can remain.

Do not claim uninstall securely erases every physical or remotely synchronized copy.

## 18. OS/device backup boundary

The operating system, enterprise management, cloud/device backup or snapshot features can independently retain application data.

Local-first means no required CareNest server—not that the OS can never back up local files.

## 19. Privacy classification summary

| Data | Primary location | CareNest encryption claim | External-copy risk |
|---|---|---|---|
| Structured profile/medicine/schedule/etc. | Local SQLite | No whole-DB encryption claim | OS/device backup, screenshots, exports |
| Imported document payload | CareNest vault file | Authenticated encrypted payload | Original/import source and explicit exports |
| Document metadata | SQLite | No whole-DB encryption claim | Exports/screenshots/OS backup |
| App-lock verifier/key settings | Secure storage where applicable | Derived/secret material protected by platform store | Compromised device/secure store |
| Manual backup | User-selected external file | Password-authenticated encryption | Destination/provider/password custody |
| Reports/exports | User-selected external destination | Generally plaintext portable output | Destination owns copy |
| OS reminder request | OS notification/alarm subsystem | OS-managed | OS history/preview/device state |

## 20. Data-change checklist

When adding/changing a stored category:

1. define owning layer/store;
2. define sensitivity and encryption claim;
3. update schema/migration where needed;
4. define backup/restore behavior;
5. define export/share behavior;
6. define deletion/reset behavior;
7. define logging restrictions;
8. update privacy/security/store documentation;
9. add tests;
10. perform packaged compatibility if persistence format/provider behavior changes.

## 21. Current release evidence

PR #74 source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Current automated verification includes 331/331 tests plus configured platform/store/security/dependency/inspection gates. Real packaged data compatibility remains open until actual evidence is recorded.

## Related documents

- `docs/privacy/PRIVACY_MODEL.md`
- `docs/privacy/DATA_LIFECYCLE.md`
- `docs/architecture/DATABASE_SCHEMA.md`
- `docs/architecture/DOCUMENT_VAULT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`