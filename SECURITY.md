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
- optional local app lock using salted PBKDF2-HMAC-SHA256 verifier material, fixed-time comparison and platform secure storage where applicable;
- privacy-minimized logging;
- blocking dependency audit and CodeQL;
- source-policy/architecture/privacy/security tests;
- fail-closed package forbidden-marker scanner;
- production signing secrets kept outside source control.

The app lock is a local privacy barrier, not a substitute for device authentication or protection against a fully compromised device/OS.

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

## Current automated security reference

PR #74 frozen source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Current evidence includes:

- 331/331 core tests;
- Android/Windows/iOS-simulator/Mac-Catalyst Release builds;
- all four store-candidate configurations;
- Store Inspection Artifacts;
- CodeQL;
- unsuppressed Dependency Audit;
- strict compiled XAML binding enforcement.

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