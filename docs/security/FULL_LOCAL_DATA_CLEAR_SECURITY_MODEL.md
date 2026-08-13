# Full local-data clear security model

## Scope

This document defines the security and failure-safety contract for CareNest's full local-data clearing flow. It is about local privacy cleanup only; it does not change CareNest's medical or organizational scope.

## Data involved

A full local-data clear can affect:

- local SQLite structured records and settings;
- scheduled CareNest notifications;
- encrypted imported-document payload files;
- the document-vault master key stored through `ISecretStore`;
- app-lock secure-storage values;
- in-memory app-lock state;
- current application navigation state.

It does not automatically delete user-created copies outside CareNest, such as shared reports, exported calendar files, manually saved backups, screenshots, or files copied by the operating system or another application.

## Required cleanup order

The current Settings flow uses this order:

1. Cancel CareNest notification registrations.
2. Read and retain the encrypted-document filenames that must be cleaned up.
3. Clear the structured local repository.
4. Remove encrypted document payload files.
5. Remove the document-vault master key from secure storage.
6. Disable app lock and remove its stored verifier material.
7. Clear in-memory app-lock state.
8. Reset navigation to onboarding.

## Why SQLite is cleared before payload files

Deleting encrypted files first can leave live database metadata pointing to files that no longer exist if the database clear subsequently fails.

CareNest therefore clears structured records before deleting the files. If later file cleanup fails, the residual state is an encrypted orphan rather than an active database row referencing a missing payload. That orphan remains encrypted and can be removed on a later cleanup attempt.

## Why the document key is retained during payload deletion

The document master key is intentionally retained until encrypted-file cleanup succeeds. Removing the key first could make a remaining encrypted orphan permanently inaccessible even to CareNest's own cleanup/recovery path.

After all targeted document payload cleanup succeeds, `SecretKeys.DocumentMasterKey` is removed through `ISecretStore`.

## App-lock material

`IAppLockService.DisableAsync` removes CareNest's app-lock secure-storage entries. The current implementation removes the enabled flag, PBKDF2 salt, and verifier material. The in-memory Settings state is cleared only after that secure cleanup call completes.

## Notification registrations

CareNest requests cancellation of its scheduled notification registrations before destructive local-state changes. Operating systems control final delivery semantics; cancellation is a best-effort platform request and cannot guarantee that no already-delivered or OS-cached notification is visible.

## Failure model

### Notification cancellation fails

The repository and encrypted local data remain untouched because subsequent destructive work has not begun.

### Filename enumeration fails

The database and payloads remain untouched.

### Repository clear fails

Encrypted payload files and secure document key remain available. This avoids active records that point to files CareNest already removed.

### Encrypted-file cleanup fails

Structured records may already be gone, but the document master key is still retained. The residual payload remains encrypted and can be targeted again on a future cleanup attempt.

### Document-key removal fails

The structured records and targeted encrypted files are already gone. A residual unused secure-storage key may remain until cleanup succeeds or the operating system removes application secure storage.

### App-lock cleanup fails

Document-vault cleanup has already succeeded, but app-lock secure-storage material may remain until the operation can be retried.

### Navigation fails

Storage cleanup has already completed. The user interface may remain on the current surface, but the removed local data is not recreated by a navigation failure.

## Trust boundaries

The flow cannot guarantee deletion of:

- external backups saved by the user;
- exported reports or CSV/PDF/calendar files;
- documents copied outside the CareNest sandbox;
- operating-system backups or snapshots outside application control;
- screenshots;
- data retained by external share targets.

These are separate user/OS-controlled copies and must be handled at their respective storage locations.

## Automated controls

`tests/CareNest.UiTests/SettingsLifecycleContractTests.cs` checks that:

- Settings uses the registered `ISecretStore` dependency for the document-key lifecycle; and
- the current source keeps notification cancellation, filename capture, structured clearing, payload cleanup, document-key removal, app-lock cleanup, and onboarding navigation in the required order.

The exact source was verified through PR #36 with 192 core tests and all four platform Release builds green.

## Residual risks

- Device or OS compromise can bypass application-level privacy controls.
- Files copied outside the app sandbox are not under CareNest's deletion control.
- OS notification delivery/cancellation is not guaranteed.
- Secure-storage behavior ultimately depends on the platform implementation.
- The separately tracked SQLitePCLRaw dependency advisory remains open for production release review.
