# CareNest Documentation

This directory is the documentation hub for CareNest `1.0.0-rc.1`.

CareNest is a local-first .NET MAUI family health organizer. It is an organizational product, not a diagnostic/treatment/dosage/interaction/clinical-risk system and not an emergency service.

## Start here

### Users

- [`USER_GUIDE.md`](USER_GUIDE.md) — complete user-facing behavior, privacy, reminder, backup, app-lock, and limitation guide.
- [`FEATURE_REFERENCE.md`](FEATURE_REFERENCE.md) — feature-by-feature implementation reference.
- [`REPORTS_AND_EXPORTS.md`](REPORTS_AND_EXPORTS.md) — JSON/PDF/CSV/document/calendar export semantics and privacy boundaries.
- [`GLOSSARY.md`](GLOSSARY.md) — project terminology.
- [`SUPPORT_CARENEST.md`](SUPPORT_CARENEST.md) — support/funding information.
- [`../PRIVACY.md`](../PRIVACY.md) — root privacy policy.
- [`../TERMS.md`](../TERMS.md) — root terms.
- [`../SECURITY.md`](../SECURITY.md) — security/reporting policy.

### Developers and maintainers

- [`setup/DEVELOPMENT.md`](setup/DEVELOPMENT.md) — primary development commands/prerequisites.
- [`setup/PLATFORM_SETUP.md`](setup/PLATFORM_SETUP.md) — Android/Windows/iOS/Mac Catalyst setup and manual platform checks.
- [`setup/MAINTAINER_OPERATIONS.md`](setup/MAINTAINER_OPERATIONS.md) — maintainer workflow, commits, CI, verification, dependencies, security, releases.
- [`setup/TROUBLESHOOTING.md`](setup/TROUBLESHOOTING.md) — common development/build/runtime troubleshooting.
- [`DOCUMENTATION_STANDARDS.md`](DOCUMENTATION_STANDARDS.md) — documentation accuracy, evidence, safety, and maintenance rules.
- [`../CONTRIBUTING.md`](../CONTRIBUTING.md) — contribution policy.
- [`testing/TESTING_GUIDE.md`](testing/TESTING_GUIDE.md) — complete automated/manual testing reference.
- [`testing/TEST_PLAN.md`](testing/TEST_PLAN.md) — concise test plan.

### Architecture

- [`architecture/ARCHITECTURE.md`](architecture/ARCHITECTURE.md) — complete system overview/layering.
- [`architecture/APPLICATION_FLOWS.md`](architecture/APPLICATION_FLOWS.md) — end-to-end runtime flows.
- [`architecture/SERVICE_BOUNDARIES.md`](architecture/SERVICE_BOUNDARIES.md) — project/service/infrastructure responsibility boundaries.
- [`architecture/DATABASE_SCHEMA.md`](architecture/DATABASE_SCHEMA.md) — SQLite relationships, entities, migrations, WAL/snapshot model.
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md) — storage/export/share/deletion boundaries.
- [`architecture/BACKUP_AND_RESTORE.md`](architecture/BACKUP_AND_RESTORE.md) — encrypted backup/restore architecture.
- [`architecture/DOCUMENT_VAULT.md`](architecture/DOCUMENT_VAULT.md) — encrypted imported-document storage/key/import/export/delete model.
- [`architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md) — deterministic occurrence vs Android/iOS/Mac/Windows notification-delivery behavior.
- [`architecture/ADR-0001-local-first.md`](architecture/ADR-0001-local-first.md) — local-first architecture decision.
- [`architecture/ADR-0002-reminder-occurrences.md`](architecture/ADR-0002-reminder-occurrences.md) — reminder materialization decision.
- [`architecture/ADR-0003-encrypted-backup-format.md`](architecture/ADR-0003-encrypted-backup-format.md) — encrypted backup-format decision.
- [`../DECISIONS.md`](../DECISIONS.md) — consolidated architectural/engineering decisions.

### Reminder scheduling and platform notifications

- [`testing/REMINDER_SCHEDULING_CONTRACT.md`](testing/REMINDER_SCHEDULING_CONTRACT.md) — deterministic planner contract covering explicit schedules, UTC windows, ownership, date/state rules, DST, deduplication, and occurrence identity.
- [`architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md) — platform permission, delivery, recovery, Android exact-alarm/battery, Apple local notification, and Windows fallback limitations.
- [`FEATURE_REFERENCE.md`](FEATURE_REFERENCE.md#reminder-planner) — feature-level reminder reference.
- [`architecture/APPLICATION_FLOWS.md`](architecture/APPLICATION_FLOWS.md#reminder-materialization-flow) — planner/coordinator runtime flow.

### Privacy and data lifecycle

- [`privacy/PRIVACY_MODEL.md`](privacy/PRIVACY_MODEL.md) — complete local-first privacy architecture.
- [`privacy/DATA_LIFECYCLE.md`](privacy/DATA_LIFECYCLE.md) — data lifecycle from user entry/import through local use/export/backup/deletion.
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md) — where data lives and how it leaves CareNest.
- [`architecture/DOCUMENT_VAULT.md`](architecture/DOCUMENT_VAULT.md) — encrypted document payload boundary.
- [`REPORTS_AND_EXPORTS.md`](REPORTS_AND_EXPORTS.md) — plaintext/portable export boundary.
- [`../PRIVACY.md`](../PRIVACY.md) — user-facing privacy policy.

### Security

- [`security/SECURITY_MODEL.md`](security/SECURITY_MODEL.md) — complete technical security reference.
- [`security/THREAT_MODEL.md`](security/THREAT_MODEL.md) — threats, controls, residual risks.
- [`security/LOGGING_PRIVACY.md`](security/LOGGING_PRIVACY.md) — allowed/prohibited diagnostic content.
- [`security/DEPENDENCY_RISK_REGISTER.md`](security/DEPENDENCY_RISK_REGISTER.md) — tracked dependency risks.
- [`../SECURITY.md`](../SECURITY.md) — root security/reporting policy.
- [`releases/SECURITY_RELEASE_REVIEW.md`](releases/SECURITY_RELEASE_REVIEW.md) — final-release security review checklist.

### Backup and data portability

- [`architecture/BACKUP_AND_RESTORE.md`](architecture/BACKUP_AND_RESTORE.md) — protected backup format/path and restore behavior.
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md) — JSON/PDF/CSV/document/calendar/export boundaries.
- [`architecture/DOCUMENT_VAULT.md`](architecture/DOCUMENT_VAULT.md) — encrypted document payload/key portability.
- [`REPORTS_AND_EXPORTS.md`](REPORTS_AND_EXPORTS.md) — report/export contracts.
- [`privacy/PRIVACY_MODEL.md`](privacy/PRIVACY_MODEL.md) — privacy implications of portable copies.

### Design, branding, accessibility, localization

- [`design/DESIGN_SYSTEM.md`](design/DESIGN_SYSTEM.md) — complete design tokens/visual/interaction rules.
- [`design/ACCESSIBILITY.md`](design/ACCESSIBILITY.md) — accessibility specification and manual checks.
- [`design/LOCALIZATION.md`](design/LOCALIZATION.md) — resource architecture, translation/safety/RTL/testing strategy.
- [`design/STORE_ASSETS.md`](design/STORE_ASSETS.md) — store screenshots/assets/claim/privacy guidance.
- [`releases/BMC_HIGHLIGHT.md`](releases/BMC_HIGHLIGHT.md) — highlighted project-support presentation guidance.
- [`../BUY_ME_A_COFFEE.md`](../BUY_ME_A_COFFEE.md) — root project-support page.

### Testing and CI

- [`testing/TESTING_GUIDE.md`](testing/TESTING_GUIDE.md) — test project roles, commands, verified counts, CI/security gates, exact-head protocol.
- [`testing/TEST_PLAN.md`](testing/TEST_PLAN.md) — current concise test plan.
- [`testing/REMINDER_SCHEDULING_CONTRACT.md`](testing/REMINDER_SCHEDULING_CONTRACT.md) — reminder test invariants.
- [`releases/PHASE8_VERIFICATION_EVIDENCE.md`](releases/PHASE8_VERIFICATION_EVIDENCE.md) — current runtime/test hardening verification evidence.

### Release engineering

- [`releases/RELEASE_PROCESS.md`](releases/RELEASE_PROCESS.md) — end-to-end public release process.
- [`releases/RELEASE_CHECKLIST.md`](releases/RELEASE_CHECKLIST.md) — release evidence and blocking checklist.
- [`releases/QUALITY_GATE.md`](releases/QUALITY_GATE.md) — production quality requirements.
- [`releases/MANUAL_TEST_MATRIX.md`](releases/MANUAL_TEST_MATRIX.md) — device/accessibility/manual behavior evidence matrix.
- [`releases/STORE_SUBMISSION_CHECKLIST.md`](releases/STORE_SUBMISSION_CHECKLIST.md) — distribution-channel checks.
- [`releases/SECURITY_RELEASE_REVIEW.md`](releases/SECURITY_RELEASE_REVIEW.md) — security approval record.
- [`releases/RELEASE_EVIDENCE.md`](releases/RELEASE_EVIDENCE.md) — evidence/provenance workflow.
- [`releases/RELEASE_NOTES_TEMPLATE.md`](releases/RELEASE_NOTES_TEMPLATE.md) — release-notes template.
- [`releases/VERIFICATION_BRANCH_PROTOCOL.md`](releases/VERIFICATION_BRANCH_PROTOCOL.md) — exact-source marker-only CI protocol.
- [`releases/NEXT_STEPS.md`](releases/NEXT_STEPS.md) — production blockers and post-release roadmap.
- [`releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`](releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md) — SQLite provider/package remediation verification plan.
- [`releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md`](releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md) — documentation inventory and remaining operational release gates.

### Project state and history

- [`../PROJECT_STATUS.md`](../PROJECT_STATUS.md) — current release status and real blockers.
- [`../CHANGELOG.md`](../CHANGELOG.md) — version/change history.
- [`../what_changed.md`](../what_changed.md) — active detailed continuation handoff.
- [`history/what_changed_full_through_phase8.md`](history/what_changed_full_through_phase8.md) — exact preserved full pre-documentation handoff containing the complete earlier implementation/hardening/verification record.
- [`../DECISIONS.md`](../DECISIONS.md) — engineering decisions.

## Current automated baseline

Latest exact runtime/test source verified through PR #30:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

Evidence:

- CareNest CI #248 / `31382194805`: success;
- 74 unit tests;
- 13 integration tests;
- 54 UI-contract/policy tests;
- 141 total core tests;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #248 / `31382194687`: success;
- Dependency Audit #10 / `31382194683`: success.

PR #30 was verification-only and closed without merging its marker.

Documentation-only commits after that source head do not change the runtime/test source represented by the above evidence.

## Current production blockers

The repository remains `1.0.0-rc.1`, not a final public production release.

Real remaining gates include:

- manual device/emulator verification;
- manual accessibility verification;
- notification permission/delivery/Android alarm/battery/reboot/time-zone checks;
- current Apple/Google store-policy review, including external voluntary project-support link;
- signing/package identities and secrets outside Git;
- store screenshots/listings/privacy/data-safety disclosures;
- final Release Evidence for the exact promoted commit;
- explicit resolution/decision for the tracked SQLitePCLRaw dependency advisory.

Use `PROJECT_STATUS.md`, `releases/NEXT_STEPS.md`, and `releases/RELEASE_CHECKLIST.md` as the authoritative blocker trackers.

## Documentation maintenance rule

When behavior changes:

1. update the lowest-level technical document describing the behavior;
2. update user-facing documentation if observable behavior changed;
3. update security/privacy docs if data/trust boundaries changed;
4. update tests/contracts;
5. update release checklist/status/evidence when verification changes;
6. update this index for new major documents;
7. update `what_changed.md` when a detailed handoff is requested.

See `DOCUMENTATION_STANDARDS.md` for the complete documentation-governance rules.

Documentation must not claim a manual test, store-policy decision, dependency remediation, or final release is complete unless it actually occurred.