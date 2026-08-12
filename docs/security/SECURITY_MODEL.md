# CareNest Security Architecture Reference

This document provides a single technical view of CareNest v1 security controls and limitations. It complements `SECURITY.md`, `docs/security/THREAT_MODEL.md`, `docs/security/LOGGING_PRIVACY.md`, and `docs/security/DEPENDENCY_RISK_REGISTER.md`.

## Security objective

CareNest is designed as a local-first organizer with strong separation between structured local records, encrypted document payloads, manual encrypted backups, optional app-lock access control, privacy-minimized diagnostics, and explicit outbound user actions.

The application is not a medical device security system and does not claim protection against a fully compromised operating system/device.

## Trust boundaries

Primary trusted components:

- CareNest application process;
- operating-system application sandbox;
- platform secure-secret storage;
- local filesystem areas assigned to the application;
- platform notification/file/share APIs used after explicit application actions.

External/untrusted-or-separate boundaries include:

- user-selected export destinations;
- calendar providers;
- browsers/web destinations;
- cloud drives chosen by the user;
- public GitHub/support systems;
- store/distribution systems;
- compromised/rooted/jailbroken environments.

## SQLite protection

Structured records live in local SQLite storage.

Current security statement:

- database is protected primarily by application sandbox/device security;
- CareNest does not claim transparent whole-database encryption;
- SQL/repository access is kept in infrastructure rather than UI;
- migrations are versioned;
- integrity tests cover persistence behavior;
- WAL mode and busy-timeout configuration are regression tested.

## Encrypted document protection

Imported document payloads use authenticated encryption with .NET cryptographic primitives.

Design properties:

- document payloads are separate from structured metadata;
- a per-installation random encryption key is stored through platform secure storage;
- encrypted document round-trip/tamper tests are part of the integration suite;
- decrypted/exported copies leave the CareNest vault boundary after explicit export/share.

## Backup protection

Manual backups use:

- user password;
- PBKDF2-HMAC-SHA256 password-based key derivation;
- AES-GCM authenticated encryption;
- versioned format metadata;
- authentication/tamper checks;
- wrong-password rejection;
- protected document-recovery key material inside the encrypted payload.

The backup password is not recoverable through a CareNest backend because no such backend exists in v1.

## App-lock protection

The optional app lock uses:

- numeric PIN policy;
- random salt;
- PBKDF2-HMAC-SHA256 verifier derivation;
- fixed-time verifier comparison;
- platform secure secret storage for enabled/salt/verifier material;
- clearing candidate/retrieved verifier buffers where managed-memory control permits;
- removal of stored lock material when disabled.

Limitations:

- no plaintext PIN is intentionally persisted;
- numeric PIN entropy depends on user choice;
- app lock is not whole-database encryption;
- a compromised secure store/OS can defeat the intended boundary;
- app lock does not replace device-level authentication/security.

## Notification protection

CareNest minimizes notification payload sensitivity.

- generic labels are used by default;
- document contents/private free-text are not intended in notification requests;
- platform notification systems control final storage/display/delivery;
- user device lock-screen preview settings remain important.

## Reminder integrity protections

Reminder-planning security/reliability controls include:

- explicit entity-ownership validation;
- known schedule kind;
- valid explicit time-zone identifier;
- UTC planning-window validation;
- half-open window semantics;
- deterministic occurrence keys;
- duplicate-time deduplication;
- deterministic DST overlap handling;
- no invented DST-gap replacement time;
- archived/paused/completed/disabled/as-needed suppression rules;
- explicit future-UTC snooze requirement.

These controls protect organizational data integrity. They do not validate clinical appropriateness.

## Logging protection

Runtime diagnostic logging is intentionally restricted.

The codebase uses source contracts and explicit logging-level guards to avoid:

- full exception-object logging from user-data operation paths;
- raw exception messages/stack traces;
- medicine/profile/reminder record identifiers in reminder scheduling failures;
- document contents;
- credentials/PINs/backup passwords/keys.

See `docs/security/LOGGING_PRIVACY.md`.

## Global exception observation

CareNest attaches privacy-aware global exception observation at startup.

The handler:

- attaches once;
- observes supported application-domain/unobserved-task exceptions;
- logs only safe exception type/category metadata when the level is enabled;
- marks unobserved task exceptions observed after safe handling.

It is not intended to serialize private application state for remote telemetry.

## Local-first network boundary

Current runtime policy tests protect against accidental addition of network/telemetry clients to the local-first v1 scope.

A future HTTP/gRPC/sync/analytics subsystem requires explicit review rather than being introduced as an incidental dependency.

## External browser/support boundary

Fixed project-support destination:

`https://buymeacoffee.com/sanskarIN`

The application should open it only after explicit user action and without appending health/profile/document/reminder identifiers.

External store policy is reviewed separately before distribution.

## Secret management

Repository rules prohibit committing common secret/signing material.

Never commit:

- Android keystore/private keys;
- Apple signing certificates/private keys/provisioning secrets;
- Windows signing private keys;
- API/service credentials;
- real app-lock PINs;
- backup passwords;
- encryption keys;
- production `.env` secrets.

Signing/configuration secrets belong outside Git.

## Dependency security

NuGet dependency auditing is part of repository verification.

Current known tracked risk:

`GHSA-2m69-gcr7-jv3q` against SQLitePCLRaw native `2.1.11` through the current dependency chain.

Important:

- the exact audit suppression exists only to keep other validation observable;
- it is not remediation;
- no blanket/severity-wide suppression is used;
- release risk remains open in `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- provider/package changes must follow `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

## Static and automated security controls

Repository automation includes:

- CodeQL;
- Dependency Audit;
- architecture contracts;
- repository policy contracts;
- no common signing-secret file contracts;
- no runtime network/telemetry client policy;
- logging privacy source contracts;
- app-lock cryptographic source contracts;
- backup/document encryption integration tests;
- SQLite migration/integrity tests;
- reminder ownership/time/state contracts;
- warnings-as-errors CI posture for correctness/security analyzers except explicitly documented advisory exceptions.

## Source hygiene

Committed runtime source policy rejects implementation placeholders such as TODO/FIXME/`NotImplementedException` patterns covered by the repository tests.

Generated `bin`/`obj` content is excluded from committed-source policy scans.

## Async/cancellation safety

Runtime source avoids common synchronous task-blocking patterns.

Cancellation-aware operations are used where appropriate for I/O/application workflows.

This improves reliability and reduces UI-thread/deadlock risk but is not itself a confidentiality mechanism.

## Backup/restore attack considerations

Threats include:

- tampered backup;
- wrong password;
- malicious/unsupported format version;
- partial/corrupt SQLite snapshot;
- leaked backup file/password;
- insecure destination.

Controls include authenticated encryption, format/version validation, snapshot integrity checks, and manual release restore testing.

## Export attack considerations

Exports intentionally create copies outside the CareNest protected boundary.

Risks:

- plaintext CSV/PDF/JSON exposure;
- insecure share destination;
- cloud synchronization by destination app;
- retained historical copies after local deletion.

Mitigation is explicit user action plus clear privacy documentation; CareNest cannot remotely recall exported copies.

## Physical/device compromise

Residual risks outside CareNest's guarantee include:

- unlocked device access;
- rooted/jailbroken device;
- malicious overlay/accessibility tooling;
- OS compromise;
- memory/process inspection;
- screenshots/screen recording;
- compromised secure storage;
- device/OS backups.

Device-level encryption, secure lock screen, OS updates, and trusted software remain part of the overall security posture.

## Security release review

Before final public promotion:

- rerun CodeQL/Dependency Audit for exact source;
- review threat model;
- review logging privacy;
- review dependency risk;
- complete `docs/releases/SECURITY_RELEASE_REVIEW.md`;
- verify no real secrets were committed;
- verify signed artifacts come from exact reviewed source;
- verify store/privacy disclosures match runtime behavior;
- manually inspect logs and export/backup workflows on target devices.

## Incident response

Security reports should use the process in `SECURITY.md`.

Do not request real health data by default. Prefer synthetic reproduction inputs and sanitized diagnostics.

## Future networked features

Any future sync/account/remote caregiver feature requires at minimum:

- authentication design;
- authorization/consent/revocation model;
- encryption/key ownership;
- network endpoint security;
- server retention/deletion/export;
- abuse/threat analysis;
- conflict recovery;
- device revocation;
- privacy/store disclosure changes;
- incident-response expansion;
- dedicated automated security tests.

Those features are not implicitly covered by the current local-first security model.