# CareNest Settings lifecycle verification — 2026-08-13

## Purpose

This evidence records the final automated verification of the CareNest `1.0.0-rc.1` source after hardening the Settings local-data lifecycle and recovering from an obsolete `SettingsViewModel` replacement that was correctly exposed by CI.

## Exact source under test

- Source SHA: `3b19ce08f509f27aca823469abc5b8a03ed2465a`
- Verification PR: #36 — `Verify final CareNest rc1 source head`
- Verification branch: `ci/carenest-rc1-final-verification-20260813`
- Marker SHA: `b89d4289172f1d4004f3b7017b7ebb90d5471b13`
- Marker path: `build/verification/rc1-final-verification-20260813.txt`
- PR #36 was closed without merge after all required checks passed. The marker is not on `main`.

## Verified Settings lifecycle behavior

The source keeps the previously verified `ObservableViewModel` Settings architecture and adds only the intended local-data lifecycle hardening:

1. Cancel CareNest notification registrations.
2. Capture encrypted document filenames while the repository is still readable.
3. Clear structured local SQLite records before deleting encrypted payload files.
4. Delete encrypted document payloads while the document master key is still available.
5. Remove `SecretKeys.DocumentMasterKey` through the registered `ISecretStore` only after payload cleanup succeeds.
6. Disable app lock, which removes its secure-storage material.
7. Clear the in-memory app-lock state.
8. Navigate to onboarding only after cleanup completes.

This order prefers a recoverable encrypted orphan over a live database row that references an already-missing encrypted payload. It also retains the document key until encrypted-file cleanup succeeds, allowing a failed cleanup to be retried.

## Automated verification results

### CareNest CI #362

Run ID: `31701943543`

- platform-neutral formatting: success;
- UnitTests: 106 passed, 0 failed, 0 skipped;
- IntegrationTests: 30 passed, 0 failed, 0 skipped;
- UiTests: 56 passed, 0 failed, 0 skipped;
- total core tests: 192 passed, 0 failed, 0 skipped;
- Android Release build: success;
- Windows Release build: success;
- iOS simulator Release build: success;
- Mac Catalyst Release build: success.

Relevant job IDs:

- Core tests: `94452963864`
- Android build: `94452963638`
- Windows build: `94452963566`
- Apple build: `94452963689`

### CodeQL #362

Run ID: `31701943506`

Result: success.

### Dependency Audit #16

Run ID: `31701943476`

Result: success.

The green Dependency Audit does **not** mean the separately tracked SQLitePCLRaw advisory is remediated. `GHSA-2m69-gcr7-jv3q` remains an open production-release decision in `docs/security/DEPENDENCY_RISK_REGISTER.md` and `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

## Superseded verification attempts

### PR #34

PR #34 was closed without merge before promotion because the Settings lifecycle review found that the document master key still remained in platform secure storage after local-data cleanup. Its marker was not merged.

### PR #35

PR #35 was closed without merge and is not release evidence. Its CI run correctly exposed that an obsolete `SettingsViewModel` implementation shape had replaced the previously verified MAUI/MVVM implementation. Android and Apple compilation failed with missing obsolete types including `ViewModelBase`, `IBackupReminderCoordinator`, `INavigationService`, `IUserDialogService`, `IFileShareService`, and `IAsyncCommand`.

The source was recovered by restoring the verified PR #33 Settings architecture and then reapplying only the intended lifecycle hardening. PR #36 validates that repaired source.

## Scope boundary

This hardening changes local cleanup/privacy integrity only. It does not add diagnosis, dosage calculation or inference, treatment advice, medication-interaction checking, clinical scoring, cloud sync, remote caregiver sharing, or emergency-service functionality.

## Remaining production gates

This evidence completes the automated exact-head source verification for this candidate. It does not complete manual device testing, accessibility qualification, store-policy review, signing, store submission, final dependency-risk disposition, or final production Release Evidence for the eventual promoted `1.0.0` commit.
