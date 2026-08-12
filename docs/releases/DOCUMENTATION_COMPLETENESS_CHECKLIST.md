# CareNest Documentation Completeness Checklist

This checklist inventories the documentation expected for the CareNest `1.0.0-rc.1` repository and defines maintenance expectations for future changes.

## Project-level documentation

- [x] Root `README.md` — product overview, build entry point, verified baseline, support.
- [x] `docs/README.md` — central documentation hub.
- [x] `PROJECT_STATUS.md` — current release state and blockers.
- [x] `CHANGELOG.md` — change history.
- [x] `DECISIONS.md` — engineering/architecture decisions.
- [x] `what_changed.md` — detailed implementation/handoff record.
- [x] `CONTRIBUTING.md` — contributor workflow/boundaries.
- [x] `CODE_OF_CONDUCT.md` — participation expectations.
- [x] `LICENSE` / `NOTICE` — open-source licensing notices.

## User documentation

- [x] `docs/USER_GUIDE.md` — complete user workflow/limitations guide.
- [x] `docs/FEATURE_REFERENCE.md` — feature-by-feature reference.
- [x] `docs/REPORTS_AND_EXPORTS.md` — report/export behavior and privacy boundary.
- [x] `docs/GLOSSARY.md` — terminology.
- [x] `SUPPORT.md` / `docs/SUPPORT_CARENEST.md` — support channels.
- [x] `BUY_ME_A_COFFEE.md` — voluntary project support.

## Legal/privacy/security user-facing documents

- [x] `PRIVACY.md`.
- [x] `TERMS.md`.
- [x] `SECURITY.md`.
- [x] Medical/reminder limitations are represented in user/release docs.
- [x] External project-support trust boundary documented.

## Architecture documentation

- [x] `docs/architecture/ARCHITECTURE.md` — full solution architecture.
- [x] `docs/architecture/APPLICATION_FLOWS.md` — end-to-end runtime flows.
- [x] `docs/architecture/SERVICE_BOUNDARIES.md` — project/service responsibilities.
- [x] `docs/architecture/DATABASE_SCHEMA.md` — schema/entities/migrations/WAL.
- [x] `docs/architecture/DATA_STORAGE_AND_EXPORT.md` — storage/export/delete boundaries.
- [x] `docs/architecture/BACKUP_AND_RESTORE.md` — protected backup architecture.
- [x] `docs/architecture/DOCUMENT_VAULT.md` — encrypted document-vault architecture.
- [x] `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md` — platform notification model.
- [x] Local-first ADR.
- [x] Reminder-occurrence ADR.
- [x] Encrypted-backup ADR.

## Privacy documentation

- [x] `docs/privacy/PRIVACY_MODEL.md` — complete privacy architecture.
- [x] `docs/privacy/DATA_LIFECYCLE.md` — lifecycle from entry to deletion/external copies.
- [x] Explicit distinction between local CareNest data and external exports/backups/calendar/browser destinations.
- [x] No automatic CareNest cloud upload claim documented accurately.
- [x] OS/device backup residual boundary documented.

## Security documentation

- [x] `docs/security/SECURITY_MODEL.md` — technical controls/limitations.
- [x] `docs/security/THREAT_MODEL.md` — threats/residual risk.
- [x] `docs/security/LOGGING_PRIVACY.md` — diagnostic data policy.
- [x] `docs/security/DEPENDENCY_RISK_REGISTER.md` — open dependency risk.
- [x] App-lock limitations documented.
- [x] SQLite database-at-rest limitation documented.
- [x] Document/backup encryption distinctions documented.
- [x] Secret/signing material exclusion documented.

## Reminder documentation

- [x] `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`.
- [x] Explicit-user-input-only scheduling documented.
- [x] Ownership validation documented.
- [x] UTC window contract documented.
- [x] Half-open planning window documented.
- [x] Schedule-state/profile-state suppression documented.
- [x] DST gap/overlap behavior documented.
- [x] Every-N-hours/cycle/selected-weekday rules documented.
- [x] As-needed no-automatic-reminder behavior documented.
- [x] Future-UTC snooze rule documented.
- [x] OS delivery limitations documented separately from planner determinism.

## Design/accessibility/localization documentation

- [x] `docs/design/DESIGN_SYSTEM.md` — full design system.
- [x] `docs/design/ACCESSIBILITY.md` — accessibility specification/manual evidence.
- [x] `docs/design/LOCALIZATION.md` — resource/translation/RTL/safety strategy.
- [x] `docs/design/STORE_ASSETS.md` — visual/store screenshot/claim guidance.
- [x] Brand variants/watermark/support-artwork rules documented.

## Developer/maintainer documentation

- [x] `docs/setup/DEVELOPMENT.md` — build/test/setup.
- [x] `docs/setup/PLATFORM_SETUP.md` — Android/Windows/iOS/Mac Catalyst details.
- [x] `docs/setup/TROUBLESHOOTING.md` — comprehensive troubleshooting.
- [x] `docs/setup/MAINTAINER_OPERATIONS.md` — repository/CI/release operations.
- [x] Maintainer Git identity documented (`Sanskar`, `sanskarin@outlook.in`).
- [x] GitHub API commit-identity limitation documented honestly.
- [x] `CareNestTargetFramework` build isolation documented.
- [x] No-secret/real-health-data contribution rule documented.

## Testing documentation

- [x] `docs/testing/TESTING_GUIDE.md` — layered automated/manual testing guide.
- [x] `docs/testing/TEST_PLAN.md` — concise current plan.
- [x] Unit/integration/UI-contract roles documented.
- [x] Current verified test counts documented.
- [x] Exact-head verification protocol linked.
- [x] Randomized deterministic recurrence testing documented.
- [x] WAL snapshot integrity/cancellation tests documented.
- [x] App-lock source/crypto contracts documented.
- [x] Manual accessibility/device testing distinction documented.

## Release documentation

- [x] `docs/releases/RELEASE_PROCESS.md` — end-to-end release process.
- [x] `docs/releases/RELEASE_CHECKLIST.md`.
- [x] `docs/releases/QUALITY_GATE.md`.
- [x] `docs/releases/MANUAL_TEST_MATRIX.md`.
- [x] `docs/releases/STORE_SUBMISSION_CHECKLIST.md`.
- [x] `docs/releases/SECURITY_RELEASE_REVIEW.md`.
- [x] `docs/releases/RELEASE_EVIDENCE.md`.
- [x] `docs/releases/RELEASE_NOTES_TEMPLATE.md`.
- [x] `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.
- [x] `docs/releases/NEXT_STEPS.md`.
- [x] `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.
- [x] Phase 8 exact-head verification evidence documented.
- [x] BMC/store-channel release guidance documented.

## Documentation governance

- [x] `docs/DOCUMENTATION_STANDARDS.md` defines accuracy/evidence/maintenance rules.
- [x] New major docs are linked from `docs/README.md`.
- [x] Runtime verified source SHA is kept separate from later documentation-only head.
- [x] Manual checks are not represented as complete unless performed.
- [x] Dependency suppression is not represented as remediation.
- [x] Store policies are described as time-sensitive and subject to submission-time review.

## Current automated source baseline

Exact runtime/test source verified through PR #30:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

Evidence:

- CareNest CI #248 / `31382194805`: success;
- formatting: success;
- UnitTests: 74 passed;
- IntegrationTests: 13 passed;
- UiTests: 54 passed;
- total core: 141 passed;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #248 / `31382194687`: success;
- Dependency Audit #10 / `31382194683`: success.

Documentation-only commits after this SHA do not establish a new runtime source baseline.

## Real release work not completed by documentation

The documentation package being complete does **not** complete these production blockers:

- [ ] manual Android device/emulator matrix;
- [ ] manual Windows matrix;
- [ ] manual iOS/iPadOS matrix;
- [ ] manual Mac Catalyst matrix;
- [ ] real notification permission/delivery checks;
- [ ] Android alarm/battery/reboot/time-zone checks;
- [ ] screen-reader/large-text/keyboard/contrast/reduced-motion checks;
- [ ] current Apple/Google external-support-link/store-policy review;
- [ ] signing identities/secrets/package configuration;
- [ ] signed artifact inspection;
- [ ] final screenshots/store listings/privacy-data-safety submission data;
- [ ] explicit acceptable SQLitePCLRaw advisory resolution/decision;
- [ ] final Release Evidence for the exact promoted production commit;
- [ ] final production tag/release.

Do not mark these items complete solely because their procedures are now fully documented.

## Maintenance rule

When a future source change adds/removes/changes behavior, update this checklist if the documentation set or required documentation categories change. A documentation checkbox means the documentation exists and covers the intended area; it does not mean every operational release test described by that documentation has been performed.