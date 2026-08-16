# CareNest Privacy Model

**Release line:** `1.0.0-rc.1`

CareNest v1 is intentionally local-first. This document describes the implemented data boundaries for users, developers, reviewers and store-submission work. It complements `PRIVACY.md` and `docs/privacy/DATA_LIFECYCLE.md` and is not jurisdiction-specific legal advice.

## 1. Privacy objective

CareNest minimizes required remote data processing by keeping normal application records on the user's device and requiring explicit user action for export/share/calendar/browser operations.

Current v1 has no required CareNest account/backend, no automatic CareNest cloud sync/upload and no hidden runtime analytics/telemetry client.

## 2. Data categories

CareNest can locally organize sensitive data such as:

- profile names/details/photos;
- emergency contacts;
- medicine names;
- user-entered strength/instruction text;
- schedule/reminder values;
- reminder/log history;
- appointments/notes;
- document metadata/tags/folders;
- imported health documents;
- stock/refill estimates;
- settings/preferences;
- audit/backup metadata;
- app-lock configuration material.

Being organizational rather than diagnostic does not make this data non-sensitive.

## 3. No required account/backend

Current release does not require:

- CareNest registration;
- phone-number authentication;
- CareNest cloud database;
- automatic CareNest synchronization;
- remote caregiver account;
- server-side analytics profile.

A future change to this boundary requires explicit architecture/privacy/security/threat-model/store-disclosure review.

## 4. Structured SQLite data

Structured records are stored in SQLite within application-owned local storage.

CareNest does **not** claim transparent whole-database encryption. Protection relies primarily on app sandbox/device/OS security plus application access controls.

Imported document payloads use a separate encrypted storage path.

## 5. Encrypted documents

Application-owned imported document payloads use authenticated encryption with document key material stored through platform secure storage where applicable.

Document metadata remains in structured local records so CareNest can organize/search/link documents.

CareNest does not automatically upload document payloads.

## 6. Manual backups

Manual backups are password-encrypted/authenticated portable files.

Privacy depends on:

- backup password strength/storage;
- selected destination;
- destination provider/device security;
- retention of old backups;
- external cloud/device backup behavior.

CareNest has no server-side backup-password recovery service.

## 7. App lock

Optional app lock stores derived verifier material rather than plaintext PIN.

It is a local UI privacy barrier, not whole-database/device encryption and not protection against a fully compromised OS/device.

## 8. Notification privacy

Notifications can be visible on lock screens or connected devices.

CareNest uses privacy-minimized/generic notification wording by default and avoids document contents, passwords/PINs/keys and unnecessary sensitive free text in normal payloads.

The OS controls final display/history/preview behavior.

## 9. Logging/diagnostics

Routine CareNest logs should not contain:

- health-document contents;
- raw private health notes/instructions;
- backup passwords;
- plaintext app-lock PINs;
- encryption keys;
- signing credentials;
- unnecessary raw sensitive exception messages/stack traces.

Use safe operation/category and exception-type information where sufficient.

See `docs/security/LOGGING_PRIVACY.md`.

## 10. Explicit outbound boundaries

CareNest can hand data/control to external systems only through explicit user actions such as:

- document export/share;
- report/profile export/share;
- appointment calendar export;
- manual encrypted backup save/share;
- opening repository/creator/legal/support web destinations.

After handoff, the receiving app/service/location has its own privacy/security policy.

## 11. Application funding boundary

The distributed CareNest application source/package does **not** include or expose the external Buy Me a Coffee project-support destination.

Repository documentation can contain the voluntary support URL:

`https://buymeacoffee.com/sanskarIN`

Opening that link occurs from repository/browser context, not as a current CareNest health-data application feature. Project support does not unlock health functionality or access local records.

## 12. Reports/exports

PDF/CSV/JSON/document exports can contain sensitive plaintext organizational data.

Once a user saves/shares a copy outside application-controlled storage, CareNest cannot enforce its later retention/security or remotely revoke it.

## 13. Calendar privacy

Appointment calendar export can transfer information to an OS or third-party calendar provider. That provider may sync remotely according to its own policies.

## 14. Backup privacy after export

Deleting local CareNest data does not delete backup files stored in external locations. Users must manage those copies separately.

## 15. Archive versus delete

Archive preserves local data but changes active behavior such as reminder eligibility.

Delete is destructive local removal. UI/documentation must not describe archive as deletion.

## 16. Deletion limitations

Deleting/resetting CareNest-owned local data cannot reliably remove copies previously handed to:

- external files/apps;
- cloud drives;
- calendars;
- email/messaging;
- screenshots/screen recordings;
- OS/device backups;
- manually retained encrypted backups.

SQLite/OS physical remnants can also be subject to storage/device behavior; CareNest does not claim forensic secure erasure beyond implemented behavior.

## 17. OS/device copies

Operating systems, enterprise management, device backup and snapshot systems can independently retain application data according to their configuration.

“Local-first” does not mean “the operating system can never back up data.”

## 18. Screenshots/accessibility tools

Visible content can be captured by screenshots, screen recording, accessibility/overlay software or other privileged tooling according to device state.

A fully compromised device is outside the guaranteed privacy boundary.

## 19. No hidden analytics/telemetry

Current local-first runtime does not include an analytics/telemetry client as part of v1 architecture.

Future telemetry would require explicit purpose, data minimization, consent/retention/deletion policy, network/privacy/store disclosures, threat-model review and regression tests.

## 20. Contributor data-minimization rules

When adding a field/feature:

1. store only what the feature needs;
2. prefer local processing for v1-compatible features;
3. keep sensitive data out of logs;
4. minimize notification payloads;
5. require explicit export/share;
6. define deletion and backup behavior;
7. update schema/privacy/store docs if persistence changes;
8. add source tests preventing accidental remote/network behavior when the feature remains local-first.

## 21. Future cloud/remote features

Accounts/sync/remote caregiver access/server storage require new design for:

- purpose/consent;
- authentication/authorization;
- remote transfer/storage/retention;
- encryption/key ownership;
- revocation/deletion/export;
- conflict/offline behavior;
- incident response;
- privacy/store disclosures.

Do not treat such a feature as a minor infrastructure addition.

## 22. Security/reporting privacy

Security reports follow `SECURITY.md`.

Do not request or publicly post real user health records as a default debugging strategy. Prefer synthetic reproduction data and privacy-safe diagnostics.

## 23. Current automated evidence boundary

Current executable source verification is PR #74:

- 331/331 core tests;
- all configured Android/Windows/iOS-simulator/Mac-Catalyst Release builds;
- all four store-candidate builds;
- Store Inspection Artifacts;
- CodeQL;
- unsuppressed Dependency Audit.

This evidence protects source policies but does not replace real-device/privacy presentation/store review.

## 24. Related documentation

- `PRIVACY.md`
- `TERMS.md`
- `SECURITY.md`
- `docs/privacy/DATA_LIFECYCLE.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/security/THREAT_MODEL.md`
- `docs/security/LOGGING_PRIVACY.md`
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md`