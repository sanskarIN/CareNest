# CareNest Security Policy

## Supported versions

Security fixes are prioritized for the newest maintained CareNest release line.

Current source release line: `1.0.0-rc.1`.

## Reporting

Do not open a public issue for a vulnerability that could expose health records, backups, lock credentials, documents, notification contents, cryptographic material, signing credentials or other sensitive information.

Report privately to:

- Business/security contact: `sanskarin@outlook.in`
- Support: `supportramsandesh@gmail.com`

Include the affected version/source if known, platform, synthetic reproduction conditions, impact and suggested mitigation. Do not include real user health data, real backups, PINs/passwords/keys or production signing material.

## Security design

Current v1 security boundaries include:

- no required CareNest account/backend;
- no hidden runtime analytics/telemetry client;
- local SQLite structured records protected primarily by application sandbox/device security;
- no claim of transparent whole-database encryption;
- imported application-owned health documents encrypted with authenticated AES-256-GCM-based framing;
- password-encrypted/authenticated manual backups;
- bounded backup archive entry counts and uncompressed payload sizes before manifest parsing/extraction;
- optional local app lock using salted PBKDF2-HMAC-SHA256 verifier material, fixed-time comparison and platform secure storage where applicable;
- privacy-minimized logging;
- blocking dependency audit and CodeQL;
- source-policy/architecture/privacy/security tests;
- fail-closed package forbidden-marker scanner;
- production signing secrets kept outside source control.

The app lock is a local privacy barrier, not a substitute for device authentication or protection against a fully compromised device/OS.

## Backup resource-safety boundary

CareNest backup restore/inspection validates archive metadata before deserializing the manifest or extracting files. Current default limits are:

- manifest: 1 MiB maximum;
- SQLite database: 1 GiB maximum;
- each encrypted document: 512 MiB maximum;
- total uncompressed backup payload: 2 GiB maximum;
- documents: 5,000 maximum.

Backup creation validates the generated archive against the same limits before encryption so CareNest does not intentionally create a backup that its current restore path would reject. These bounds are availability/resource-exhaustion controls; they do not replace authenticated encryption, archive topology validation, database integrity checks or package/device compatibility testing.

## External links

Normal repository/creator/legal/support destinations are fixed and opened only after explicit user action. They must not carry local health/profile/document/reminder/backup/app-lock information in query parameters or payloads.

The distributed CareNest application package currently does **not** include or expose the external Buy Me a Coffee project-funding destination/card/command/artwork.

Repository-only voluntary support can be documented at:

`https://buymeacoffee.com/sanskarIN`

That external provider remains outside the CareNest trust boundary and does not gain access to local CareNest records merely because a user separately visits it.

## Logging privacy

The detailed logging boundary is documented in `docs/security/LOGGING_PRIVACY.md`. Runtime/diagnostic changes must preserve that contract.

Routine CareNest diagnostics should not require raw health content, document/backup contents, PIN/password/key material or unnecessary full sensitive exception messages/stack traces.

## Dependency security

Known dependency advisories/remediation are tracked in `docs/security/DEPENDENCY_RISK_REGISTER.md`.

The previously tracked SQLite native dependency path for `GHSA-2m69-gcr7-jv3q` is remediated in the current source graph through maintained central native/provider pins and removal of the former exact `NuGetAuditSuppress` entry.

A suppression is not remediation. Future exceptions must be exact, temporary, documented and reviewed. A clean vulnerability audit also does not replace packaged existing-database/encrypted-data compatibility testing after persistence/native-provider changes.

## Accepted automated security reference

Accepted final-candidate source before the backup resource-limit hardening:

`b6eecae66f74bd72bcb20d93508355542f9f3442`

Observed evidence for that accepted source includes:

- 355/355 core tests;
- Android/Windows/iOS-simulator/Mac-Catalyst Release builds;
- all four store-candidate configurations;
- Store Inspection Artifacts;
- CodeQL;
- unsuppressed Dependency Audit;
- strict compiled XAML binding enforcement.

Any verification-relevant source change after that accepted source, including backup security hardening, requires a fresh exact-source automated matrix before it can replace the accepted production-candidate reference.

This is source automation evidence, not final production signing/store/device security approval.

## Production security requirements

Before public production release complete applicable:

- real-device/manual platform validation;
- packaged SQLite/encrypted-data compatibility;
- accessibility/privacy presentation review;
- current store privacy/policy review;
- production signing outside Git;
- final signed-package checksum/provenance/forbidden-marker scan;
- exact immutable production tag with all required tagged workflows.

See:

- `docs/security/SECURITY_MODEL.md`;
- `docs/security/THREAT_MODEL.md`;
- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SECURITY_RELEASE_REVIEW.md`;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`.
