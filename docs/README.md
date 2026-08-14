# CareNest Documentation

This directory is the documentation hub for CareNest `1.0.0-rc.1`.

CareNest is a local-first .NET MAUI family health organizer. It is an organizational product, not a diagnostic/treatment/dosage/interaction/clinical-risk system and not an emergency service.

## Current verified source

The current exact-head automated source reference is:

- PR #43 — `Verify final CareNest 2026-08-14 bug audit source`
- verification branch — `ci/carenest-final-bug-audit-20260814`
- PR closed without merge after all required automated gates succeeded;
- marker file did not enter `main`.

Successful final gate groups:

- platform-neutral formatting;
- complete unit suite;
- complete integration suite;
- complete UI-contract/policy suite;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- CodeQL;
- Dependency Audit.

See:

- [`../PROJECT_STATUS.md`](../PROJECT_STATUS.md)
- [`../what_changed.md`](../what_changed.md)
- [`releases/BUG_AUDIT_VERIFICATION_20260814.md`](releases/BUG_AUDIT_VERIFICATION_20260814.md)
- [`testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`](testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md)
- [`security/BUG_AUDIT_SECURITY_NOTES_20260814.md`](security/BUG_AUDIT_SECURITY_NOTES_20260814.md)

The successful Dependency Audit does not resolve `GHSA-2m69-gcr7-jv3q`; the dependency risk register remains authoritative.

## Start here

### Users

- [`USER_GUIDE.md`](USER_GUIDE.md) — complete user-facing behavior, privacy, reminder, backup, app-lock and limitation guide.
- [`FEATURE_REFERENCE.md`](FEATURE_REFERENCE.md) — feature-by-feature implementation reference.
- [`REPORTS_AND_EXPORTS.md`](REPORTS_AND_EXPORTS.md) — JSON/PDF/CSV/document/calendar export semantics and privacy boundaries.
- [`GLOSSARY.md`](GLOSSARY.md) — shared terminology.
- [`SUPPORT_CARENEST.md`](SUPPORT_CARENEST.md) — voluntary support/funding information.
- [`../PRIVACY.md`](../PRIVACY.md) — root privacy policy.
- [`../TERMS.md`](../TERMS.md) — root terms.
- [`../SECURITY.md`](../SECURITY.md) — root security/reporting policy.

### Developers and maintainers

- [`setup/DEVELOPMENT.md`](setup/DEVELOPMENT.md) — primary commands/prerequisites.
- [`setup/PLATFORM_SETUP.md`](setup/PLATFORM_SETUP.md) — Android/Windows/iOS/Mac Catalyst setup and manual platform checks.
- [`setup/MAINTAINER_OPERATIONS.md`](setup/MAINTAINER_OPERATIONS.md) — maintainer workflow, Git identity, CI, dependencies, security and releases.
- [`setup/TROUBLESHOOTING.md`](setup/TROUBLESHOOTING.md) — development/build/runtime troubleshooting.
- [`DOCUMENTATION_STANDARDS.md`](DOCUMENTATION_STANDARDS.md) — documentation accuracy/evidence rules.
- [`../CONTRIBUTING.md`](../CONTRIBUTING.md) — contribution policy.

## Architecture

- [`architecture/ARCHITECTURE.md`](architecture/ARCHITECTURE.md) — complete system overview/layering.
- [`architecture/APPLICATION_FLOWS.md`](architecture/APPLICATION_FLOWS.md) — end-to-end runtime flows.
- [`architecture/SERVICE_BOUNDARIES.md`](architecture/SERVICE_BOUNDARIES.md) — project/service/infrastructure responsibilities.
- [`architecture/DATABASE_SCHEMA.md`](architecture/DATABASE_SCHEMA.md) — SQLite relationships, entities, migrations, WAL and snapshot model.
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md) — storage/export/share boundaries.
- [`architecture/BACKUP_AND_RESTORE.md`](architecture/BACKUP_AND_RESTORE.md) — encrypted backup/restore, strict topology and v1/v2 compatibility.
- [`architecture/DOCUMENT_VAULT.md`](architecture/DOCUMENT_VAULT.md) — encrypted imported-document storage, key handling and export model.
- [`architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md) — deterministic occurrence vs platform delivery and permission limitations.
- [`architecture/ADR-0001-local-first.md`](architecture/ADR-0001-local-first.md) — local-first decision.
- [`architecture/ADR-0002-reminder-occurrences.md`](architecture/ADR-0002-reminder-occurrences.md) — reminder materialization decision.
- [`architecture/ADR-0003-encrypted-backup-format.md`](architecture/ADR-0003-encrypted-backup-format.md) — backup-format decision.
- [`../DECISIONS.md`](../DECISIONS.md) — consolidated architectural/engineering decisions.

## Reminder scheduling and platform notifications

- [`testing/REMINDER_SCHEDULING_CONTRACT.md`](testing/REMINDER_SCHEDULING_CONTRACT.md) — deterministic planner contract.
- [`testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`](testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md) — final stale-alarm/snooze/platform reconciliation regression map.
- [`architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md) — permission/delivery/recovery/platform limitations.
- [`FEATURE_REFERENCE.md`](FEATURE_REFERENCE.md) — feature-level reminder reference.
- [`architecture/APPLICATION_FLOWS.md`](architecture/APPLICATION_FLOWS.md) — reminder materialization runtime flow.

Current audited reminder invariants additionally include:

- snoozed rows use `SnoozedUntilUtc` as effective due time;
- future snoozes remain upcoming after original due time passes;
- overdue snoozes can transition to missed;
- rebuild cancels an existing platform request before replacement/suppression/invalidation;
- stale schedule rows retain their IDs until OS-level cancellation can be attempted;
- medicine/profile delete flows cancel future platform requests before cascade and compensate if the cascade fails;
- invalid DST-gap interval anchors do not invent a shifted time.

## Privacy and data lifecycle

- [`privacy/PRIVACY_MODEL.md`](privacy/PRIVACY_MODEL.md) — complete local-first privacy architecture.
- [`privacy/DATA_LIFECYCLE.md`](privacy/DATA_LIFECYCLE.md) — lifecycle from user entry/import through local use/export/backup/deletion.
- [`privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md`](privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md) — CareNest-controlled cleanup boundary.
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md) — where data lives/how it leaves CareNest.
- [`architecture/DOCUMENT_VAULT.md`](architecture/DOCUMENT_VAULT.md) — encrypted payload/explicit export boundary.
- [`REPORTS_AND_EXPORTS.md`](REPORTS_AND_EXPORTS.md) — plaintext/portable export boundary.
- [`../PRIVACY.md`](../PRIVACY.md) — user-facing policy.

The 2026-08-14 audit additionally routes successful decrypted document exports through the managed `Exports` cache and uses partial-file/atomic-move handling for generated plaintext reports/previews.

## Security

- [`security/SECURITY_MODEL.md`](security/SECURITY_MODEL.md) — technical security reference.
- [`security/THREAT_MODEL.md`](security/THREAT_MODEL.md) — threats, controls and residual risks.
- [`security/LOGGING_PRIVACY.md`](security/LOGGING_PRIVACY.md) — allowed/prohibited diagnostic content.
- [`security/DEPENDENCY_RISK_REGISTER.md`](security/DEPENDENCY_RISK_REGISTER.md) — tracked dependency risks.
- [`security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md`](security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md) — full local-data clear failure-safety model.
- [`security/BUG_AUDIT_SECURITY_NOTES_20260814.md`](security/BUG_AUDIT_SECURITY_NOTES_20260814.md) — app-lock/key/plaintext/platform-reconciliation audit notes.
- [`../SECURITY.md`](../SECURITY.md) — root security/reporting policy.
- [`releases/SECURITY_RELEASE_REVIEW.md`](releases/SECURITY_RELEASE_REVIEW.md) — final-release security review checklist.

## Backup and data portability

- [`architecture/BACKUP_AND_RESTORE.md`](architecture/BACKUP_AND_RESTORE.md) — protected backup format and restore behavior.
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md) — JSON/PDF/CSV/document/calendar boundaries.
- [`architecture/DOCUMENT_VAULT.md`](architecture/DOCUMENT_VAULT.md) — document payload/key portability.
- [`REPORTS_AND_EXPORTS.md`](REPORTS_AND_EXPORTS.md) — report/export contracts.
- [`privacy/PRIVACY_MODEL.md`](privacy/PRIVACY_MODEL.md) — privacy implications of portable copies.

The bug audit distinguishes primary backup/restore completion from later best-effort local bookkeeping and restores exact previous document-key bytes during failed restore rollback.

## Design, branding, accessibility and localization

- [`design/DESIGN_SYSTEM.md`](design/DESIGN_SYSTEM.md) — design tokens/visual/interaction rules.
- [`design/ACCESSIBILITY.md`](design/ACCESSIBILITY.md) — accessibility specification/manual checks.
- [`design/LOCALIZATION.md`](design/LOCALIZATION.md) — resource architecture, translation/safety/RTL/testing strategy.
- [`design/STORE_ASSETS.md`](design/STORE_ASSETS.md) — store screenshots/assets/claim/privacy guidance.
- [`releases/BMC_HIGHLIGHT.md`](releases/BMC_HIGHLIGHT.md) — project-support presentation guidance.
- [`../BUY_ME_A_COFFEE.md`](../BUY_ME_A_COFFEE.md) — root project-support page.

## Testing and CI

- [`testing/TESTING_GUIDE.md`](testing/TESTING_GUIDE.md) — automated/manual testing reference.
- [`testing/TEST_PLAN.md`](testing/TEST_PLAN.md) — concise test plan.
- [`testing/REMINDER_SCHEDULING_CONTRACT.md`](testing/REMINDER_SCHEDULING_CONTRACT.md) — reminder invariants.
- [`testing/SETTINGS_LIFECYCLE_CONTRACT.md`](testing/SETTINGS_LIFECYCLE_CONTRACT.md) — Settings lifecycle contract.
- [`testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`](testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md) — final bug-class regression inventory.
- [`releases/PHASE8_VERIFICATION_EVIDENCE.md`](releases/PHASE8_VERIFICATION_EVIDENCE.md) — preserved earlier Phase 8 evidence.
- [`releases/PHASE9_VERIFICATION_EVIDENCE.md`](releases/PHASE9_VERIFICATION_EVIDENCE.md) — preserved PR #36 evidence.
- [`releases/BUG_AUDIT_VERIFICATION_20260814.md`](releases/BUG_AUDIT_VERIFICATION_20260814.md) — latest exact-head evidence.

Final PR #43 is the current automated source reference.

## Release engineering

- [`releases/RELEASE_PROCESS.md`](releases/RELEASE_PROCESS.md) — end-to-end public release process.
- [`releases/RELEASE_CHECKLIST.md`](releases/RELEASE_CHECKLIST.md) — automated/manual promotion checklist.
- [`releases/QUALITY_GATE.md`](releases/QUALITY_GATE.md) — production quality requirements.
- [`releases/MANUAL_TEST_MATRIX.md`](releases/MANUAL_TEST_MATRIX.md) — device/accessibility/manual evidence matrix.
- [`releases/STORE_SUBMISSION_CHECKLIST.md`](releases/STORE_SUBMISSION_CHECKLIST.md) — distribution-channel checks.
- [`releases/SECURITY_RELEASE_REVIEW.md`](releases/SECURITY_RELEASE_REVIEW.md) — security approval record.
- [`releases/RELEASE_EVIDENCE.md`](releases/RELEASE_EVIDENCE.md) — evidence/provenance workflow.
- [`releases/RELEASE_NOTES_TEMPLATE.md`](releases/RELEASE_NOTES_TEMPLATE.md) — release-notes template.
- [`releases/VERIFICATION_BRANCH_PROTOCOL.md`](releases/VERIFICATION_BRANCH_PROTOCOL.md) — marker-only exact-head CI protocol.
- [`releases/NEXT_STEPS.md`](releases/NEXT_STEPS.md) — production blockers and roadmap.
- [`releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`](releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md) — SQLite dependency remediation plan.
- [`releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md`](releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md) — documentation inventory/operational gates.
- [`releases/BUG_AUDIT_VERIFICATION_20260814.md`](releases/BUG_AUDIT_VERIFICATION_20260814.md) — latest source audit verification.

## Project state and history

- [`../PROJECT_STATUS.md`](../PROJECT_STATUS.md) — current PR #43 automated baseline and real blockers.
- [`../CHANGELOG.md`](../CHANGELOG.md) — version/change history.
- [`../what_changed.md`](../what_changed.md) — active detailed handoff.
- [`history/what_changed_full_through_phase8.md`](history/what_changed_full_through_phase8.md) — complete earlier implementation/hardening history.
- [`history/what_changed_documentation_through_20260812.md`](history/what_changed_documentation_through_20260812.md) — documentation pass history.
- [`history/what_changed_through_pr33_20260813.md`](history/what_changed_through_pr33_20260813.md) — preserved long PR #33 handoff.
- [`../DECISIONS.md`](../DECISIONS.md) — engineering decisions.

## Current production blockers

CareNest remains `1.0.0-rc.1`, not a final public production release.

Real remaining gates include:

- manual Android/Windows/iOS/iPadOS/Mac Catalyst verification;
- manual notification permission/real-delivery/platform lifecycle checks;
- packaged-target document/photo/report/backup checks;
- legacy encrypted-format fixture checks when canonical fixtures are available;
- manual accessibility verification;
- current Apple/Google store-policy review, including optional external project-support link;
- signing/package identities and secrets outside Git;
- signed package generation/inspection;
- store screenshots/listings/privacy/data-safety disclosures;
- final Release Evidence for the exact promoted production commit;
- explicit resolution/decision for the tracked SQLitePCLRaw advisory.

Use `PROJECT_STATUS.md`, `releases/NEXT_STEPS.md`, and `releases/RELEASE_CHECKLIST.md` as operational trackers.

## Documentation maintenance rule

When behavior changes:

1. update the lowest-level technical document describing the behavior;
2. update user-facing documentation if observable behavior changes;
3. update security/privacy docs if data/trust boundaries change;
4. update tests/contracts;
5. update release status/evidence when verification changes;
6. update this index for new major documents;
7. update `what_changed.md` when a detailed handoff is requested;
8. if runtime/test/workflow/package/resource source changes after an exact-head verification, run a fresh marker-only verification instead of reusing old evidence.

Documentation must not claim a manual test, store-policy decision, dependency remediation, signing step, or final production release is complete unless it actually occurred.
