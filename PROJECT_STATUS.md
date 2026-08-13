# Project Status

## Release target

`1.0.0-rc.1` remains the current release candidate. The implementation is source-complete for the current v1 scope. Final public `1.0.0` promotion still requires the manual, store, signing, dependency, and release-evidence gates listed below.

## Preserved detailed history

The complete previous PR #33-era status is preserved unchanged at:

`docs/history/PROJECT_STATUS_through_PR33.md`

Complete earlier handoffs are also preserved at:

- `docs/history/what_changed_full_through_phase8.md`
- `docs/history/what_changed_documentation_through_20260812.md`
- `docs/history/what_changed_through_pr33_20260813.md`

The active status therefore advances the current baseline without discarding earlier detail.

## Current exact automated source baseline

Source SHA:

`3b19ce08f509f27aca823469abc5b8a03ed2465a`

Verification PR:

`#36 — Verify final CareNest rc1 source head`

Verification marker:

`b89d4289172f1d4004f3b7017b7ebb90d5471b13`

The marker-only PR was closed without merge after the full matrix passed.

### CareNest CI #362

Run ID: `31701943543`

- formatting: success;
- UnitTests: 106 passed, 0 failed, 0 skipped;
- IntegrationTests: 30 passed, 0 failed, 0 skipped;
- UiTests: 56 passed, 0 failed, 0 skipped;
- total core tests: 192 passed, 0 failed, 0 skipped;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success.

### Security/dependency workflows

- CodeQL #362 / run `31701943506`: success.
- Dependency Audit #16 / run `31701943476`: success.

The dependency result does not resolve the separately tracked SQLitePCLRaw advisory.

## Phase 9 status

Phase 9 preserves the verified PR #33 `ObservableViewModel` Settings architecture and adds the intended local-state lifecycle integrity controls plus two new UI-contract tests.

The lifecycle now keeps notification registration handling, encrypted-document file discovery, structured repository state, encrypted payload processing, document-key state, app-lock state, and onboarding navigation in the documented failure-safe order.

The exact source-to-PR33 comparison showed only these intended non-documentation differences:

- `src/CareNest.App/ViewModels/SettingsViewModel.cs`: 15 additions, 2 deletions;
- `tests/CareNest.UiTests/SettingsLifecycleContractTests.cs`: new 45-line file.

Authoritative Phase 9 references:

- `docs/releases/SETTINGS_LIFECYCLE_VERIFICATION_20260813.md`
- `docs/releases/PHASE9_VERIFICATION_EVIDENCE.md`
- `docs/security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md`
- `docs/testing/SETTINGS_LIFECYCLE_CONTRACT.md`
- `docs/privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md`

## Verification recovery record

PR #34 was closed without merge when the lifecycle review identified one more secure-storage requirement before final verification.

PR #35 is explicitly not release evidence. Its CI exposed an obsolete Settings implementation replacement through Android/Apple compilation failures. The failure was corrected rather than suppressed: the verified PR #33 Settings architecture was restored, the intended lifecycle delta was reapplied, and PR #36 then passed every required gate.

## Previously verified hardening retained

The current source also retains the complete prior service/appointment/document/backup/crypto/reminder/privacy hardening, including:

- strict appointment UTC handling and permission fail-safe scheduling;
- direct profile/medicine/appointment/document/backup-reminder service coverage;
- document import compensation and key-buffer hygiene;
- strict backup archive topology;
- authenticated chunked AEAD framing v2 with authenticated terminal records;
- legacy v1 read compatibility;
- prefix-truncation and trailing-data rejection for v2 streams;
- backup/document secret-buffer hygiene where managed-memory control permits;
- deterministic reminder ownership, UTC, snooze, window, recurrence, and DST contracts;
- privacy-minimized logging and global exception observation;
- app-lock PBKDF2/fixed-time/verifier-buffer contracts;
- local-first architecture and no hidden telemetry/network-client policy.

Full implementation detail remains in the preserved handoffs and subsystem documentation.

## Product boundary

CareNest remains a local-first organizational application. It has no required CareNest account/backend, no automatic CareNest cloud synchronization, no silent caregiver sharing, and no hidden analytics/telemetry client.

CareNest does not diagnose conditions, determine or infer medicine dosage, recommend treatment, perform clinical medication-interaction checking, create clinical risk scores, independently verify adherence, replace clinicians/pharmacists, provide emergency services, or guarantee notification delivery.

Medicine strength and instruction values remain opaque user-entered text.

## Voluntary project support

Project support remains centralized at:

`https://buymeacoffee.com/sanskarIN`

Funding remains separate from health data and does not unlock medical functionality. Current Apple/Google store-policy review for the external support link remains a release-time requirement.

## Open dependency risk

Tracked advisory:

`GHSA-2m69-gcr7-jv3q`

The current dependency path still includes SQLitePCLRaw native `2.1.11` through the current `sqlite-net-pcl` chain. The repository does not claim remediation.

Authoritative files:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`

## Documentation status

Dedicated documentation covers the current user/product, architecture, storage, reminders/platform behavior, document vault, reports/exports, backup, privacy, security, accessibility, localization, design/store assets, development, troubleshooting, testing, release, funding, and historical-evidence areas. Start at `docs/README.md`.

## Production gates still required

1. Android manual device/emulator matrix.
2. Windows manual matrix.
3. iOS/iPadOS manual matrix.
4. Mac Catalyst manual matrix.
5. Notification permission/delivery and Android alarm/battery/reboot/time-zone checks.
6. Packaged-target appointment, document, calendar, report, v2 encrypted-document, and backup/restore checks.
7. Full local-state lifecycle verification on intended devices.
8. Screen-reader, large-text, keyboard/focus, contrast/theme, and reduced-motion checks.
9. Current Apple App Store policy review for the external voluntary support link.
10. Current Google Play policy review for the external voluntary support link.
11. Signing identities/credentials outside Git.
12. Signed package build/inspection.
13. Fictional-data store screenshots and store privacy/data-safety metadata.
14. Final SQLitePCLRaw advisory disposition.
15. `CareNest Release Evidence` for the exact production commit.
16. Final production version/build metadata, release notes, checksums, tag, and GitHub release after applicable gates pass.

## Deferred future scope

Cloud sync, remote caregiver collaboration, required accounts/mobile authentication, server-side health-record storage, silent remote sharing, hidden analytics/telemetry, diagnosis, dosage inference, treatment recommendations, medication-interaction claims, and clinical scoring remain outside v1.

Any future networked feature requires a fresh consent/authentication/key/privacy/threat/export/store review.

## Environment note

GitHub-hosted CI is the authoritative automated verification surface for the source baseline above. Manual device/accessibility/store/signing work is separate and is not claimed complete unless actually performed.

See `what_changed.md` for the active detailed continuation and `docs/history/` for preserved earlier complete records.
