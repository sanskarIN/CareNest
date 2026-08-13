# Local privacy cleanup lifecycle

## Purpose

This document describes how CareNest returns its current-device local state to a fresh onboarding state when the user chooses the application's full local cleanup action.

## CareNest-managed state covered

The flow covers CareNest-managed notification registrations, structured SQLite application records, encrypted document payload storage, document-vault secure-storage material, app-lock secure-storage material, and the current in-memory app-lock state.

## State outside the application boundary

CareNest does not control user-created or operating-system-managed copies that have already left the app sandbox. Examples include manually saved encrypted backups, reports saved or shared as PDF/CSV/JSON, calendar exports, document exports, screenshots, device backups, and files retained by another application.

Those copies remain under the control of their storage location and are not represented by CareNest as changed by the local cleanup action.

## Lifecycle order

1. CareNest requests cancellation of its notification registrations.
2. It captures the encrypted document filenames needed for local storage cleanup.
3. It clears the structured local repository.
4. It processes the encrypted document payload storage while the document key remains available.
5. It clears `SecretKeys.DocumentMasterKey` through the registered `ISecretStore` after payload processing succeeds.
6. It disables app lock, which clears its secure-storage verifier material.
7. It clears the in-memory app-lock state.
8. It returns navigation to onboarding.

The ordering is designed so a repository failure does not create active metadata that references a payload already removed from CareNest storage, and so document-key material remains available while encrypted payload cleanup is still in progress.

## Privacy limitations

CareNest can control only its own sandbox, the secure-storage entries available through its platform APIs, and notification requests made by the application. It does not claim physical secure erasure of device storage, nor control over external applications or operating-system backups.

The SQLite database is not represented as transparently whole-database encrypted. Imported document payloads and manual backups retain their separately documented authenticated-encryption boundaries.

## Logging boundary

The lifecycle must not intentionally place health names, medicine names, notes, document contents, contact details, backup passwords, app-lock PINs, document keys, or other private payloads into logs. Existing privacy-minimized logging rules continue to apply.

## Manual release checks

Before production promotion, verify on intended devices that the confirmation wording is clear, CareNest notification registrations are cancelled as far as the platform permits, structured records do not reappear after restart, encrypted document storage is cleared, app lock is no longer enabled, and onboarding is shown after completion.

Also verify that external backups/exports remain outside CareNest's control and that the UI does not imply those external copies were changed by the local cleanup action.

## Automated evidence

PR #36 verified exact source `3b19ce08f509f27aca823469abc5b8a03ed2465a` with 106 unit, 30 integration, and 56 UI-contract tests, plus Android, Windows, iOS simulator, and Mac Catalyst Release builds. CodeQL #362 and Dependency Audit #16 also passed.
