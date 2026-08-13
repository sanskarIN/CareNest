# what_changed.md

## CareNest complete continuation handoff — final 2026-08-13 Phase 9 state

This is the active detailed handoff for the latest CareNest continuation.

Repository: `https://github.com/sanskarIN/CareNest`  
Branch: `main`  
Release target: `1.0.0-rc.1`  
Framework: .NET 10 / .NET MAUI  
Primary language: C#  
License: Apache-2.0  
Business email: `sanskarin@outlook.in`  
Support email: `supportramsandesh@gmail.com`  
Creator: `https://github.com/sanskarIN`  
Voluntary project support: `https://buymeacoffee.com/sanskarIN`  
Watermark: `Made by the Sanskar`

---

# No-loss handoff/history preservation

No earlier CareNest implementation, documentation, verification, or handoff detail was discarded to create this active file.

Complete earlier records remain on current `main`:

- `docs/history/what_changed_full_through_phase8.md` — complete Phase 0–8 implementation/hardening/verification history.
- `docs/history/what_changed_documentation_through_20260812.md` — complete 2026-08-12 documentation-completion handoff.
- `docs/history/what_changed_through_pr33_20260813.md` — the exact previous active `what_changed.md` Git blob, 1,333 lines, covering the entire appointment/service/document/backup/AEAD-v2 continuation through verified PR #33.
- `docs/history/PROJECT_STATUS_through_PR33.md` — exact previous PR #33-era `PROJECT_STATUS.md` snapshot.

The previous active handoff blob was:

`2ae7966e815046b517f8985df9016f6caabc54f5`

It was preserved exactly on `main` by:

`566e2df4dae9d56c406539cd6bd9df3db19e76b4` — `docs: preserve CareNest handoff through PR33`

The previous project-status blob was:

`7bc7cfe7dd348892197ef4006e7d6861bb00dc03`

It was preserved by:

`8bb384fc1aba1b7e9329bc6da144bd3decad13fc` — `docs: preserve project status through PR33`

This active file therefore continues the complete record rather than replacing history with a shorter reconstruction.

---

# Product and medical-safety boundary retained

CareNest remains a local-first organizational application.

CareNest does **not**:

- diagnose conditions;
- determine, calculate, or infer medicine dosage;
- recommend treatment;
- perform medication-interaction checking as a clinical feature;
- create clinical risk scores;
- independently verify medication adherence;
- replace a clinician or pharmacist;
- provide emergency services;
- guarantee reminder/notification delivery.

Medicine strength and instruction fields remain opaque user-entered text. Reminder times and stock calculations continue to originate only from explicit user-entered values/configuration.

The latest continuation changes local-state integrity, privacy, tests, verification evidence, and release documentation only. It does not add medical interpretation.

---

# Local-first boundary retained

Current v1 still has:

- no required CareNest account;
- no required CareNest backend/server;
- no automatic CareNest cloud synchronization;
- no silent caregiver sharing;
- no hidden analytics/telemetry client;
- local SQLite structured records;
- separately encrypted imported document payloads;
- manual password-encrypted backups;
- explicit user-controlled report/document/calendar sharing and export;
- optional local app lock;
- privacy-minimized logging and diagnostics.

The SQLite database is not described as transparently whole-database encrypted. Document and backup payloads retain their separate authenticated-encryption boundaries.

---

# Starting automated baseline for this latest phase

The immediately preceding fully green automated source baseline was PR #33 source:

`4f5f9abe9d702fa33d6aba3f15c113febfebf95e`

PR #33 evidence:

- CareNest CI #332: success;
- 106 unit tests;
- 30 integration tests;
- 54 UI-contract tests;
- 190 total core tests;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #332: success;
- Dependency Audit #13: success.

PR #33 was marker-only and closed without merge.

All detailed appointment/service/document/backup/crypto work through that baseline remains in `docs/history/what_changed_through_pr33_20260813.md` and is not repeated here solely to avoid duplicating 1,333 already-preserved lines.

---

# Phase 9 objective

The latest continuation performed one more Settings/local-state integrity review after the PR #33 cryptographic/service baseline.

The intended goal was narrow:

1. Ensure the full local-state lifecycle does not create active structured metadata that points to an already-unavailable encrypted document payload if a later operation fails.
2. Ensure the document-vault master key is handled through the existing `ISecretStore` abstraction and remains available until encrypted payload processing completes.
3. Ensure app-lock secure material is handled after document-vault local-state processing.
4. Preserve the already verified Settings MAUI/MVVM architecture rather than introducing a parallel/obsolete implementation shape.
5. Protect the resulting dependency wiring and lifecycle ordering with platform-neutral UI-contract tests.
6. Re-run exact-head formatting, tests, four platform Release builds, CodeQL, and dependency audit.

---

# Initial Settings lifecycle finding

The original PR #33 Settings lifecycle processed encrypted document files before the structured repository transition.

That order could create an undesirable partial state if encrypted payload processing succeeded but the later repository operation failed: structured metadata could remain while its corresponding payload was already unavailable to CareNest.

The intended hardening therefore moved the structured repository transition before encrypted payload processing, while still collecting the target document filenames beforehand.

The document key must remain available during payload processing. Its secure-storage entry is transitioned only after the targeted encrypted payload work succeeds.

---

# Superseded Settings implementation sequence

A source-selection/replay mistake occurred during this continuation and is documented explicitly rather than hidden.

An obsolete/stale `SettingsViewModel` implementation shape was accidentally written to `main` while the lifecycle work was being applied. That obsolete shape used types from an older architecture such as:

- `ViewModelBase`;
- `IBackupReminderCoordinator`;
- `INavigationService`;
- `IUserDialogService`;
- `IFileShareService`;
- `IAsyncCommand`.

Those are not the types used by the verified PR #33 Settings implementation.

The relevant superseded commits in that path were:

- `c83ca013588ebb022b4ce30de99e51bcd69c9f6e` — initial local-state ordering edit on the stale Settings source.
- `48ed3782d7756458c189f2cb594e672343781eb3` — first stale Settings source contract.
- `7b942187d1d7d77ae149e5895d5f221697825111` — stale Settings source with document-key lifecycle addition.
- `de6e0a05ef177ffbb7def07c9e3c91c65a6ac091` — stale Settings test update.
- `543916d9d0ed4640617e9c2890a8af4e0daf8fb1` — additional stale-source Settings edit.
- `218db7e9ce7e6890fece430e909fa22c364634b1` — stale contract/source candidate used for PR #35.

These commits are retained in Git history for traceability but are **not** the final source interpretation.

---

# Verification PR #34 — superseded before promotion

A marker-only verification branch was created from source `48ed3782...`.

PR:

`#34 — Verify CareNest reset data integrity hardening`

Marker head:

`6dc4bd4cb2739a1b528bf8efee741313113e6740`

PR #34 was closed without merge when the continuing lifecycle review found one more secure-storage requirement: the CareNest document master key still needed to be included in the full local-state lifecycle.

PR #34 was therefore not promoted as release evidence and its marker did not enter `main`.

---

# Verification PR #35 — CI exposed the obsolete source

A later marker-only verification PR was created from stale source:

`218db7e9ce7e6890fece430e909fa22c364634b1`

PR:

`#35 — Verify final CareNest settings integrity hardening`

Marker head:

`01671e0b08d01a85e67311d4e76ef7023d521410`

Workflow runs:

- CareNest CI #358 / run `31701221708`;
- CodeQL #358 / run `31701221693`;
- Dependency Audit #15 / run `31701221717`.

Dependency Audit #15 completed successfully and the core test step reached green, but Android/Apple application compilation failed.

The Android job exposed compile errors for the obsolete Settings architecture, including missing types such as:

- `ViewModelBase`;
- `IBackupReminderCoordinator`;
- `INavigationService`;
- `IUserDialogService`;
- `IFileShareService`;
- `IAsyncCommand`.

That failure was treated correctly as source evidence. The project did **not** suppress the compiler errors, weaken CI, or represent PR #35 as successful.

PR #35 was closed without merge and is explicitly **not** release evidence.

---

# Recovery of the verified Settings architecture

The exact PR #33 Settings implementation was retrieved from verified source:

`4f5f9abe9d702fa33d6aba3f15c113febfebf95e`

That correct implementation uses:

- `SettingsViewModel : ObservableViewModel`;
- `ICareNestRepository _repository`;
- `AppStateService _state`;
- `INotificationService _notifications`;
- `IReminderCoordinator _reminders`;
- `IAppointmentService _appointments`;
- `BackupReminderCoordinator _backupReminder`;
- `IBackupService _backup`;
- `IDocumentStore _documents`;
- `IAppFileGateway _files`;
- `IAppLockService _lock`;
- `IAppNavigator _navigator`;
- `ICommand` command surfaces.

The current `TestReminderAsync` permission behavior from the verified source was retained rather than replaced: it requests/confirms notification permission and stops before requesting a platform test notification when permission is not granted.

Recovery commit:

`b2549fd58c69f721c0cd600984a79f30c91912a2` — `fix: restore verified settings architecture with safe full reset`

That commit restored the PR #33 architecture and reapplied only the intended lifecycle changes.

---

# Final Settings lifecycle behavior

The final verified `SettingsViewModel.ResetAllDataAsync` flow uses this conceptual order:

1. Request cancellation of CareNest notification registrations.
2. Capture the encrypted document filenames that must be processed.
3. Transition the structured local repository to its fresh state.
4. Process the targeted encrypted document payload files while the document key remains available.
5. Transition `SecretKeys.DocumentMasterKey` through the registered `ISecretStore` after payload processing succeeds.
6. Disable app lock through the existing `IAppLockService` abstraction.
7. Clear the Settings in-memory app-lock state.
8. Return navigation to onboarding.

Failure-safety intent:

- if repository processing fails, encrypted payloads and key material have not already been made unavailable by CareNest;
- if encrypted payload processing stops part way through, the document key still exists for a later retry;
- only after encrypted payload work succeeds does CareNest transition the document key;
- app-lock secure material is handled afterward through its existing service;
- navigation occurs last.

This remains an application-level privacy lifecycle. It is not represented as physical secure erasure of flash storage or removal of user/OS copies outside CareNest's control.

---

# Secure-storage audit

The current secure-store abstraction is:

`ISecretStore`

MAUI registration:

`AddSingleton<ISecretStore, SecureSecretStore>()`

`SecureSecretStore.RemoveAsync` maps to platform `SecureStorage.Default.Remove(key)` while respecting cancellation.

Document-vault key:

`SecretKeys.DocumentMasterKey = "documents.master-key.v1"`

App-lock material is handled through `AppLockService.DisableAsync`, which removes the CareNest app-lock enabled flag, PBKDF2 salt, and verifier values from the same secure-storage abstraction.

The final Settings source therefore uses existing platform/security boundaries instead of directly calling a platform secure-storage API from the view-model.

---

# Stale contract removal and replacement

The obsolete Settings contract left from the stale-source sequence was removed:

`0b3dbfb880c057391c04dafe47e9366d539db504` — `test: remove obsolete settings contract`

A new current-architecture contract was then added:

`3b19ce08f509f27aca823469abc5b8a03ed2465a` — `test: add settings lifecycle contract`

File:

`tests/CareNest.UiTests/SettingsLifecycleContractTests.cs`

The contract contains two tests.

## Contract 1 — registered secret-store lifecycle

Checks that:

- Settings depends on `ISecretStore`;
- constructor injection is present;
- the injected secret store is retained;
- the document master-key constant participates in the Settings lifecycle;
- `MauiProgram.cs` registers `ISecretStore` → `SecureSecretStore`.

## Contract 2 — lifecycle ordering

Checks the current source order of:

- notification cancellation;
- encrypted-file discovery;
- structured repository transition;
- encrypted payload processing;
- secret-store document-key transition;
- app-lock transition;
- onboarding navigation.

This increased the UI-contract suite from 54 to 56 tests.

---

# Exact repaired-source comparison to PR #33

Before final verification, source:

`3b19ce08f509f27aca823469abc5b8a03ed2465a`

was compared directly against verified PR #33 source:

`4f5f9abe9d702fa33d6aba3f15c113febfebf95e`

At that comparison point, the only non-Markdown differences were:

- `src/CareNest.App/ViewModels/SettingsViewModel.cs` — 15 additions, 2 deletions;
- `tests/CareNest.UiTests/SettingsLifecycleContractTests.cs` — new 45-line file.

All other differences were documentation.

This comparison was used to prove that the obsolete Settings implementation was no longer present in the final candidate.

---

# Final exact-head verification — PR #36

Final verified source:

`3b19ce08f509f27aca823469abc5b8a03ed2465a`

Verification branch:

`ci/carenest-rc1-final-verification-20260813`

Verification marker:

`b89d4289172f1d4004f3b7017b7ebb90d5471b13`

Marker path:

`build/verification/rc1-final-verification-20260813.txt`

PR:

`#36 — Verify final CareNest rc1 source head`

PR #36 changed only the marker file beyond the exact base source.

After all required checks passed, PR #36 was closed **without merge**. The marker is not part of `main`.

---

# PR #36 automated evidence

## CareNest CI #362

Run ID:

`31701943543`

Result:

**success**

Core job:

- platform-neutral formatting: success;
- UnitTests: 106 passed, 0 failed, 0 skipped;
- IntegrationTests: 30 passed, 0 failed, 0 skipped;
- UiTests: 56 passed, 0 failed, 0 skipped;
- total core tests: 192 passed, 0 failed, 0 skipped.

Job IDs:

- Core tests: `94452963864`
- Android build: `94452963638`
- Windows build: `94452963566`
- Apple build: `94452963689`

Platform builds:

- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success.

## CodeQL #362

Run ID:

`31701943506`

Result:

**success**

## Dependency Audit #16

Run ID:

`31701943476`

Result:

**success**

The successful dependency audit does **not** close the tracked SQLitePCLRaw advisory described below.

---

# New Phase 9 documentation commits

The verified source was frozen before the following documentation-only commits.

## Verification evidence

`754b84628ccf01e53782f0501c1ddbb6be7287db` — `docs: add final Settings lifecycle verification evidence`

Added:

`docs/releases/SETTINGS_LIFECYCLE_VERIFICATION_20260813.md`

Contains exact PR #36 source/marker/run/job/test evidence plus PR #34/#35 recovery history.

## Local-state security model

`b66886da3b30403cddb755721c5828a0533a3402` — `docs: document full local-data clear security model`

Added:

`docs/security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md`

Documents ordering rationale, failure behavior, secure-store boundaries, notification limitations, external-copy limits, automated controls, and residual risks.

## Settings lifecycle testing contract

`269b380efb9ad5750e1e65db271c0cdc287444db` — `docs: document Settings lifecycle regression contract`

Added:

`docs/testing/SETTINGS_LIFECYCLE_CONTRACT.md`

Documents the two new UI contracts, their architectural purpose, and regression-response rules.

## Privacy lifecycle

`c5499828a217a32add62e652788dfeee546a45b3` — `docs: document local privacy cleanup lifecycle`

Added:

`docs/privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md`

Documents the CareNest-controlled state boundary versus backups/exports/screenshots/OS copies outside the application boundary.

## Phase 9 evidence

`7120fb3b1f40094ee9bf386bc5d6ad81641b5635` — `docs: add Phase 9 exact-head verification evidence`

Added:

`docs/releases/PHASE9_VERIFICATION_EVIDENCE.md`

Provides the compact exact-source PR #36 evidence record.

## Preserve previous active handoff

`566e2df4dae9d56c406539cd6bd9df3db19e76b4` — `docs: preserve CareNest handoff through PR33`

Added exact old `what_changed.md` blob at:

`docs/history/what_changed_through_pr33_20260813.md`

## Preserve previous project status

`8bb384fc1aba1b7e9329bc6da144bd3decad13fc` — `docs: preserve project status through PR33`

Added exact old `PROJECT_STATUS.md` blob at:

`docs/history/PROJECT_STATUS_through_PR33.md`

## Promote current project status

`0ec80aa644f3fbe7666cb5afd2b260c0bab1d6ce` — `docs: promote PR36 automated baseline`

Updated canonical:

`PROJECT_STATUS.md`

It now records PR #36, 192 tests, all platform/security/dependency gates, recovery history, Phase 9 references, and unchanged real production blockers.

## Phase 9 change record

`da2979af1cbeda81b20f13a00f13995e1b308334` — `docs: add Phase 9 change record`

Added:

`docs/releases/CHANGELOG_PHASE9_20260813.md`

This supplements the existing root changelog without rewriting older entries.

---

# Current test baseline

Exact verified source:

`3b19ce08f509f27aca823469abc5b8a03ed2465a`

Current automated core test totals:

- Unit: 106
- Integration: 30
- UI-contract: 56
- Total: 192

All passed in PR #36.

The two-test increase from PR #33 is entirely the new `SettingsLifecycleContractTests.cs` file.

---

# Current documentation/evidence entry points

Primary current status/evidence:

- `PROJECT_STATUS.md`
- `docs/releases/SETTINGS_LIFECYCLE_VERIFICATION_20260813.md`
- `docs/releases/PHASE9_VERIFICATION_EVIDENCE.md`
- `docs/releases/CHANGELOG_PHASE9_20260813.md`
- `docs/security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md`
- `docs/testing/SETTINGS_LIFECYCLE_CONTRACT.md`
- `docs/privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md`
- `what_changed.md`

Complete earlier records:

- `docs/history/what_changed_full_through_phase8.md`
- `docs/history/what_changed_documentation_through_20260812.md`
- `docs/history/what_changed_through_pr33_20260813.md`
- `docs/history/PROJECT_STATUS_through_PR33.md`

---

# Buy Me a Coffee / support status retained

Voluntary CareNest project support remains:

`https://buymeacoffee.com/sanskarIN`

Existing support integration includes:

- centralized app constant;
- in-app About support action;
- `.github/FUNDING.yml`;
- `BUY_ME_A_COFFEE.md`;
- `docs/SUPPORT_CARENEST.md`;
- custom CareNest BMC vector artwork;
- funding-link contract tests;
- privacy/security/store-boundary documentation.

Funding remains voluntary, is separate from health data, and does not unlock medical functionality.

Current Apple/Google store-policy review for the external support link remains an operational release gate.

---

# Open SQLite dependency risk remains unchanged

Tracked advisory:

`GHSA-2m69-gcr7-jv3q`

Current dependency path still resolves SQLitePCLRaw native `2.1.11` through the current `sqlite-net-pcl` chain.

The repository does **not** claim this is fixed.

The exact audit suppression is narrow and is not represented as remediation.

Authoritative files:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`

Final production review must follow that documented migration/risk process.

---

# Production release blockers remain real

PR #36 completes the current automated exact-head source verification. It does not complete public `1.0.0` promotion.

Still required:

1. Android manual device/emulator matrix.
2. Windows manual matrix.
3. iOS/iPadOS manual matrix.
4. Mac Catalyst manual matrix.
5. Manual notification permission and real-delivery checks.
6. Android alarm/battery/reboot/time/time-zone checks.
7. Packaged-target appointment/reminder/document/calendar/report checks.
8. Packaged-target v2 encrypted document/backup checks.
9. Clean-install backup restore, wrong-password, and tamper checks.
10. Current Settings local-state lifecycle checks on intended devices.
11. Screen-reader checks.
12. Large-text/text-scaling checks.
13. Desktop keyboard/focus checks.
14. Contrast/theme/reduced-motion checks.
15. Current Apple App Store external-support-link policy review.
16. Current Google Play external-support-link policy review.
17. Signing identities/credentials outside Git.
18. Signed package build and inspection.
19. Store screenshots using fictional data.
20. Store descriptions/privacy/data-safety metadata.
21. Final SQLitePCLRaw advisory disposition.
22. `CareNest Release Evidence` for the exact production commit.
23. Final version/build metadata, release notes, checksums, production tag, and GitHub release only after applicable gates pass.

No item above is marked complete merely because the code/documentation exists or automated CI is green.

---

# Deferred scope remains unchanged

Still outside current v1:

- cloud synchronization;
- remote caregiver collaboration;
- required accounts/mobile-number authentication;
- server-side health-record storage;
- silent remote sharing;
- hidden analytics/telemetry;
- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- medication-interaction claims;
- clinical risk scoring.

Any future networked feature requires a new consent/authentication/key/privacy/threat/export/store architecture review.

---

# Environment truth

The repository-assembly environment used in this work does not provide local `dotnet`/MAUI workloads, device simulators, signing credentials, or store submission sessions.

GitHub-hosted Actions is the authoritative automated verification surface for the exact source SHA recorded above.

Manual device/accessibility/store/signing/release tasks remain separate and are not claimed complete unless actually performed.

---

# Current repository interpretation

- CareNest `1.0.0-rc.1` source remains complete for the current v1 scope.
- Exact latest automated source baseline is `3b19ce08f509f27aca823469abc5b8a03ed2465a`.
- PR #36 is the latest green exact-head verification evidence.
- Automated baseline is 192/192 core tests plus Android/Windows/iOS simulator/Mac Catalyst Release builds, CodeQL, and Dependency Audit green.
- PR #35 is intentionally retained as failed/superseded evidence that exposed the obsolete Settings replacement; it is not release evidence.
- The verified PR #33 Settings architecture was restored before the final lifecycle hardening was verified.
- Previous active handoff/status files are preserved exactly under `docs/history/`.
- The SQLitePCLRaw advisory remains open.
- Manual device/accessibility/store/signing/final-release work remains blocking.
- No cloud/account/clinical-decision functionality was introduced by Phase 9.
