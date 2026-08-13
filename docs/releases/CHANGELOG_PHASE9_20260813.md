# CareNest Phase 9 change record — 2026-08-13

## Added

- `SettingsLifecycleContractTests` with two platform-neutral UI contracts covering current Settings secure-store dependency wiring and local-state lifecycle ordering.
- Exact PR #36 verification evidence in `SETTINGS_LIFECYCLE_VERIFICATION_20260813.md` and `PHASE9_VERIFICATION_EVIDENCE.md`.
- Dedicated Settings lifecycle security, testing, and privacy references.
- Exact preserved PR #33-era active handoff and project-status snapshots under `docs/history/`.

## Changed

- Restored the verified PR #33 `ObservableViewModel` Settings architecture after an obsolete implementation was exposed by PR #35 CI.
- Settings local-state lifecycle now captures encrypted document filenames before structured repository transition, processes encrypted payload storage after the repository transition, retains document key material until payload processing succeeds, then clears document-key and app-lock secure material before returning to onboarding.
- Canonical `PROJECT_STATUS.md` now reports PR #36 and the 192-test automated baseline.

## Verification

Exact verified source:

`3b19ce08f509f27aca823469abc5b8a03ed2465a`

PR #36:

- CareNest CI #362 / `31701943543`: success;
- 106 unit tests;
- 30 integration tests;
- 56 UI-contract tests;
- 192 total core tests;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #362 / `31701943506`: success;
- Dependency Audit #16 / `31701943476`: success.

PR #36 was closed without merging its marker.

## Superseded attempts

- PR #34 was closed before promotion when lifecycle review identified an additional secure-storage requirement.
- PR #35 is not release evidence. Android/Apple compile failures exposed the obsolete Settings implementation and triggered restoration of the previously verified architecture before PR #36.

## Release status

CareNest remains `1.0.0-rc.1`. Manual device/accessibility checks, current store-policy review, signing/store preparation, final Release Evidence, and the open SQLitePCLRaw advisory disposition remain required before final public `1.0.0` promotion.
