# CareNest Privacy Model

CareNest v1 is intentionally local-first. This privacy model explains the implemented data boundaries for developers, reviewers, store-submission work, and users evaluating how information moves through the product.

This document complements `PRIVACY.md` and `docs/privacy/DATA_LIFECYCLE.md`. It does not replace jurisdiction-specific legal advice.

## Privacy objective

The v1 architecture minimizes required remote data processing by keeping normal CareNest records on the user's device and requiring explicit user action for export/share/external-link operations.

## Data categories

CareNest can locally organize data such as:

- profile names/details/photos;
- emergency contacts;
- medicine names;
- user-entered strength/instruction text;
- reminder schedule values;
- reminder state/history;
- medication-log entries;
- appointments and notes;
- health documents and document metadata;
- tags/folders;
- stock/refill estimates;
- settings/preferences;
- audit information;
- app-lock configuration material;
- backup metadata.

These records can be sensitive even when they are described as organizational rather than clinical.

## No required CareNest account/backend

The current release does not require:

- CareNest account registration;
- phone-number authentication;
- CareNest cloud database;
- automatic cloud sync;
- remote caregiver account;
- server-side analytics profile.

A future version that changes this boundary requires explicit architecture/privacy/threat-model changes.

## Local database

Structured application data is stored in SQLite inside the application data area.

CareNest does not claim transparent whole-database encryption at rest. Device/application sandbox protections remain part of the threat model.

Imported document payloads have a separate encrypted storage path.

## Encrypted documents

Sensitive imported documents use authenticated encryption with a per-installation key stored through platform secure secret storage.

Document metadata remains represented in structured local records so the application can organize/search/link documents.

CareNest does not automatically upload document payloads in v1.

## App lock

The optional app lock stores derived verifier material rather than plaintext PIN.

The app lock is a privacy barrier for opening the CareNest UI. It is not a promise that every database field is encrypted or that a fully compromised device cannot access data.

## Notification privacy

Notifications are privacy-sensitive because the OS may display them on lock screens or connected devices.

CareNest uses generic labels by default and avoids placing document content or sensitive health-record details in normal notification payloads.

Users still control OS notification-preview settings, and the operating system controls final display behavior.

## Diagnostic/logging privacy

CareNest uses privacy-minimized logging contracts.

Routine structured logs should not contain:

- health-document content;
- private free-text health notes;
- backup passwords;
- plaintext app-lock PINs;
- encryption keys;
- full exception messages or stack traces from health-data operations;
- record identifiers in reminder scheduling failure messages where avoidable.

See `docs/security/LOGGING_PRIVACY.md`.

## Explicit outbound boundaries

CareNest can hand data to another destination only through an explicit user-facing operation such as:

- document export/share;
- report export/share;
- profile JSON export;
- appointment calendar export;
- manual encrypted backup save/share;
- opening legal/support/project-support web destinations.

Once data is handed to another application/service/location, that destination has its own privacy/security policies.

## Reports and exports

CSV/PDF/JSON exports may contain sensitive plaintext organizational data.

CareNest cannot enforce retention/security after a user saves or shares those files outside the application sandbox.

Users should review exports before sharing.

## Backup privacy

Manual backups are encrypted, but they remain sensitive portable files.

Privacy considerations include:

- where the backup is stored;
- who can access the destination;
- how the password is stored;
- whether old backups remain after local deletion;
- whether a cloud-drive provider independently synchronizes the file.

CareNest v1 has no remote password-recovery service.

## Calendar privacy

Appointment calendar export can transfer appointment data into an OS calendar or third-party calendar provider.

After export, CareNest does not control whether that calendar provider syncs data remotely.

## External project-support privacy

CareNest can open:

`https://buymeacoffee.com/sanskarIN`

The action is voluntary and external.

CareNest does not intentionally append profile IDs, medicine data, reminder data, document information, backup data, or app-lock data to the funding URL.

The browser/provider becomes a separate privacy boundary after launch.

Funding does not unlock health functionality.

## GitHub/support privacy

Public issue trackers must not be used to upload real health records or secrets.

Users/reporters should not post:

- health documents;
- real backups;
- backup passwords;
- app-lock PINs;
- private keys/signing credentials;
- sensitive profile/medicine/appointment notes;
- screenshots that expose private records unless they have been safely redacted.

The bug-report template includes privacy warnings.

## Deletion model

Deleting local CareNest data can remove the application's current local records/files according to the workflow, but it cannot recall copies previously exported to:

- external file locations;
- cloud drives;
- calendar providers;
- email/messaging apps;
- screenshots;
- OS/device backups;
- manually retained encrypted CareNest backup files.

The user must manage those copies separately.

## Archive vs delete

Archive preserves local data but changes active behavior such as reminder eligibility.

Delete is destructive local removal.

Documentation/UI should not describe archive as deletion.

## No hidden analytics/telemetry

The local-first v1 runtime does not include an analytics/telemetry client as part of the product architecture.

If telemetry is considered later, it requires:

- explicit purpose;
- consent design where appropriate;
- data minimization;
- retention/deletion policy;
- privacy policy changes;
- network permissions/endpoint review;
- store disclosure changes;
- security/threat-model review;
- automated regression tests.

## Data minimization principles for contributors

When adding a field or feature:

1. Store only what the feature needs.
2. Prefer local processing for v1-compatible features.
3. Do not put sensitive data into logs for debugging convenience.
4. Keep notification payloads minimal.
5. Require explicit export/share action.
6. Define deletion behavior.
7. Define backup/restore behavior.
8. Update schema/privacy/store documentation if persistence changes.
9. Add tests preventing accidental remote/network introduction when the feature remains local-first.

## Medical-safety/privacy relationship

CareNest stores health-related organizational data, but it deliberately avoids clinical interpretation.

Privacy documentation must not imply that because the application is non-diagnostic the information is non-sensitive. User-entered medicine, appointment, reminder, and document records should still be treated as private.

## Threat-model assumptions

CareNest cannot protect against every device-level threat.

Residual risks include:

- compromised/rooted/jailbroken device;
- compromised OS secure storage;
- screen capture/recording;
- malicious accessibility/overlay software;
- physical access to an unlocked device;
- external destinations chosen by the user;
- OS/cloud backup behavior;
- weak app-lock PIN selection.

See `docs/security/THREAT_MODEL.md`.

## Store/privacy-disclosure rule

Store privacy/data-safety questionnaires must be completed from the behavior of the exact shipping build, not from aspirational marketing.

If a future distribution build adds analytics, network sync, crash reporting, remote support, or other data transfer, the disclosures and privacy docs must change before submission.

## Security incident/reporting boundary

Security reports should follow `SECURITY.md`.

Do not request real user health data as a default debugging strategy. Prefer synthetic reproduction data and privacy-safe diagnostics.

## Related documentation

- `PRIVACY.md`
- `TERMS.md`
- `SECURITY.md`
- `docs/privacy/DATA_LIFECYCLE.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/security/THREAT_MODEL.md`
- `docs/security/LOGGING_PRIVACY.md`
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md`