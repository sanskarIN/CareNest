# CareNest Documentation

This directory is the canonical documentation hub for CareNest `1.0.0-rc.1`.

CareNest is a local-first .NET MAUI family health organizer. It is an organizational product, not a diagnostic, treatment, dosage-calculation, medication-interaction, clinical-risk, or emergency-service system.

## Current source and exact automated verification

The latest verification-relevant executable/project/test/workflow/build-script/artifact-generation source was frozen at:

`4c60f90ac33a321d12a6f9b3a8c097e4e4a4e5f2`

Marker-only PR #61 is the **current authoritative exact automated/source-inspection baseline**:

- PR #61 — `Verify corrected CareNest store inspection artifacts`;
- verified source/base SHA — `4c60f90ac33a321d12a6f9b3a8c097e4e4a4e5f2`;
- verification marker head — `19c82b813c375047cf1166487bc18a1bd2cd0e52`;
- PR merge/event SHA during verification — `c8ea9fef89d7b773f19bf13c64f349495be706ad`;
- CareNest CI #650 / `31872610834`: success;
- platform-neutral formatting: success;
- 122 unit tests;
- 39 integration tests;
- 157 UI-contract/policy tests;
- 318 total core tests;
- default Android Release: success;
- default Windows Release: success;
- default iOS simulator Release: success;
- default Mac Catalyst Release: success;
- CareNest Store Package Configuration #39 / `31872610789`: success;
- funding-disabled Android Release: success;
- funding-disabled Windows Release: success;
- funding-disabled iOS simulator Release: success;
- funding-disabled Mac Catalyst Release: success;
- Bash store-package preflight executable-mode guard: success;
- CareNest Store Inspection Artifacts #2 / `31872610786`: success;
- corrected Android verified-unsigned AAB artifact: success;
- Windows self-contained unpackaged inspection artifact: success;
- iOS simulator + unsigned Mac Catalyst inspection artifacts: success;
- downloaded payload checksum/provenance inspection: success;
- CodeQL #650 / `31872610815`: success;
- unsuppressed Dependency Audit #46 / `31872610791`: success.

PR #61 was closed without merge; its verification marker did not enter `main`.

The verified store-safe jobs compile with `CareNestShowFundingLink=false`. The inspection workflow also produces reproducible internal artifacts with exact source/checksum provenance. These artifacts are deliberately unsigned/simulator/unpackaged evidence and do not replace production signing, installed-artifact inspection, packaged-data compatibility, device testing, or store approval.

PR #60 remains a superseded failure-driven checkpoint because downloaded Android artifact inspection exposed a debug-signed MAUI companion and ambiguous PR merge/source provenance. PR #59 remains historical exact evidence for the earlier default-plus-store-safe compilation boundary, PR #58 remains historical exact evidence for the earlier package/store-policy hardening boundary, PR #56 remains historical release-engineering evidence, and PR #54 remains the historical authoritative runtime bug-audit baseline.

The formerly tracked `GHSA-2m69-gcr7-jv3q` repository dependency exception is remediated in source. Maintained SQLite native/provider leaves are centrally pinned, the former exact audit suppression was removed, and PR #61 passed the unsuppressed audit. Packaged existing-database/encrypted-data compatibility remains a separate production release gate.

The dated 2026-08-15 Apple/Google support-link review is recorded in `releases/STORE_POLICY_REVIEW_20260815.md`. Under that current conservative decision, initial Apple App Store and Google Play candidates should use `CareNestShowFundingLink=false` unless submission-time storefront/country/program-specific review clearly permits the external support link.

## Primary project references

Start with these four documents for a complete engineering view:

- [`COMPLETE_PROJECT_DOCUMENTATION.md`](COMPLETE_PROJECT_DOCUMENTATION.md) — end-to-end project reference: identity, features, boundaries, architecture, data, reminders, encryption, backup, setup, testing, release and documentation map.
- [`CODEBASE_REFERENCE.md`](CODEBASE_REFERENCE.md) — concrete source-project/file map, responsibilities, test layers, workflows/scripts and change-placement rules.
- [`CONFIGURATION_REFERENCE.md`](CONFIGURATION_REFERENCE.md) — central packages, build/analyzer/audit configuration, target frameworks, local commands, platform configuration, workflows, secrets and provenance.
- [`MAINTENANCE_AND_OPERATIONS.md`](MAINTENANCE_AND_OPERATIONS.md) — routine maintenance, triage, dependency/schema/crypto/reminder changes, exact-head verification, release, signing, hotfix and incident operations.

Documentation-completeness evidence:

- [`releases/DOCUMENTATION_AUDIT_20260814.md`](releases/DOCUMENTATION_AUDIT_20260814.md) — repository-wide documentation inventory and completeness audit.
- [`releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md`](releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md) — operational documentation checklist.

## Current status and handoff

- [`../PROJECT_STATUS.md`](../PROJECT_STATUS.md) — current source boundary, exact automated/internal-artifact baseline and real production blockers.
- [`../what_changed.md`](../what_changed.md) — detailed active continuation/handoff ledger including the 2026-08-15 store/package/artifact hardening and exact verification.
- [`../CHANGELOG.md`](../CHANGELOG.md) — chronological change history.
- [`../DECISIONS.md`](../DECISIONS.md) — consolidated architecture/engineering decisions.
- [`releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md`](releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md) — authoritative PR #61 exact-source/internal-artifact evidence.
- [`releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md`](releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md) — historical PR #59 default-plus-store-safe exact-source evidence.
- [`releases/PACKAGED_RELEASE_HARDENING_VERIFICATION_20260815.md`](releases/PACKAGED_RELEASE_HARDENING_VERIFICATION_20260815.md) — historical PR #58 package/store-policy hardening evidence.
- [`releases/STORE_POLICY_REVIEW_20260815.md`](releases/STORE_POLICY_REVIEW_20260815.md) — dated support-link store-policy review and conservative package decision.
- [`releases/STORE_BUILD_POLICY.md`](releases/STORE_BUILD_POLICY.md) — build-configurable voluntary support surface and automated/local store-safe build policy.
- [`releases/PACKAGED_RELEASE_VALIDATION.md`](releases/PACKAGED_RELEASE_VALIDATION.md) — packaged/device/SQLite/encrypted-data/accessibility/signing validation runbook plus internal-artifact boundary.
- [`releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md`](releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md) — historical PR #56 automated evidence for its frozen source.
- [`releases/FINAL_BUG_AUDIT_VERIFICATION_20260814.md`](releases/FINAL_BUG_AUDIT_VERIFICATION_20260814.md) — historical authoritative PR #54 runtime bug-audit evidence.
- [`releases/BUG_AUDIT_VERIFICATION_20260814.md`](releases/BUG_AUDIT_VERIFICATION_20260814.md) — full 2026-08-14 failure-driven audit/checkpoint history.
- [`testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`](testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md) — defect-to-test/contract map.
- [`security/BUG_AUDIT_SECURITY_NOTES_20260814.md`](security/BUG_AUDIT_SECURITY_NOTES_20260814.md) — security/privacy-relevant audit record.

## User documentation

- [`USER_GUIDE.md`](USER_GUIDE.md) — complete user-facing usage guide.
- [`FEATURE_REFERENCE.md`](FEATURE_REFERENCE.md) — feature-by-feature behavior and limitations.
- [`REPORTS_AND_EXPORTS.md`](REPORTS_AND_EXPORTS.md) — JSON/PDF/CSV/document/calendar export semantics and privacy boundaries.
- [`GLOSSARY.md`](GLOSSARY.md) — project terminology.
- [`SUPPORT_CARENEST.md`](SUPPORT_CARENEST.md) — CareNest support/project-funding information.
- [`../PRIVACY.md`](../PRIVACY.md) — public privacy policy.
- [`../TERMS.md`](../TERMS.md) — public terms/limitations.
- [`../SUPPORT.md`](../SUPPORT.md) — support channels.
- [`../SECURITY.md`](../SECURITY.md) — security/reporting policy.
- [`../BUY_ME_A_COFFEE.md`](../BUY_ME_A_COFFEE.md) — voluntary project-support information.

## Product safety boundary

CareNest does **not**:

- diagnose conditions;
- determine, calculate, or infer medicine dosage;
- recommend treatment;
- perform clinical medication-interaction checking;
- create clinical risk scores;
- independently verify adherence;
- replace a clinician/pharmacist;
- provide emergency services;
- guarantee notification delivery.

Medicine strength/instruction values remain opaque user-entered text. Reminder schedules are derived only from explicit user-entered schedule values.

## Local-first/privacy boundary

Current v1 documentation consistently describes:

- no required CareNest account/backend;
- no automatic CareNest cloud synchronization/upload;
- no hidden runtime analytics/telemetry client;
- local structured SQLite records;
- separately encrypted imported document payloads;
- password-encrypted manual backups;
- explicit outbound export/share/calendar/browser actions;
- generic notification labels by default;
- privacy-minimized application logs.

Exported/shared copies leave the CareNest protected boundary and can be retained by the chosen destination, OS, cloud service, screenshots or backups.

## Architecture

- [`architecture/ARCHITECTURE.md`](architecture/ARCHITECTURE.md) — system overview and project dependency direction.
- [`architecture/APPLICATION_FLOWS.md`](architecture/APPLICATION_FLOWS.md) — end-to-end runtime flows.
- [`architecture/SERVICE_BOUNDARIES.md`](architecture/SERVICE_BOUNDARIES.md) — layer/service responsibilities.
- [`architecture/DATABASE_SCHEMA.md`](architecture/DATABASE_SCHEMA.md) — SQLite entities, relationships, indexes, schema versions, migrations, WAL and snapshots.
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md) — storage/export/share boundaries.
- [`architecture/BACKUP_AND_RESTORE.md`](architecture/BACKUP_AND_RESTORE.md) — encrypted backup/restore package and compatibility model.
- [`architecture/DOCUMENT_VAULT.md`](architecture/DOCUMENT_VAULT.md) — encrypted document storage/key/export model.
- [`architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md) — persisted reminder state versus platform notification delivery.
- [`architecture/ADR-0001-local-first.md`](architecture/ADR-0001-local-first.md) — local-first architecture decision.
- [`architecture/ADR-0002-reminder-occurrences.md`](architecture/ADR-0002-reminder-occurrences.md) — reminder occurrence materialization decision.
- [`architecture/ADR-0003-encrypted-backup-format.md`](architecture/ADR-0003-encrypted-backup-format.md) — encrypted backup format decision.

### Intended dependency direction

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

Platform-neutral projects do not depend on MAUI. ViewModels do not issue SQL directly. Local-first v1 runtime code does not casually add network/telemetry clients.

## Reminder scheduling and platform notifications

Primary references:

- [`testing/REMINDER_SCHEDULING_CONTRACT.md`](testing/REMINDER_SCHEDULING_CONTRACT.md)
- [`architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md)
- [`architecture/APPLICATION_FLOWS.md`](architecture/APPLICATION_FLOWS.md)
- [`architecture/SERVICE_BOUNDARIES.md`](architecture/SERVICE_BOUNDARIES.md)
- [`testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`](testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md)

Current audited invariants include:

- explicit user-entered schedule values only;
- ownership/UTC/date/state/DST validation;
- invalid DST-gap times are not silently shifted to invented reminder times;
- explicit future-UTC snooze requirement;
- `SnoozedUntilUtc` is effective due time for a valid snooze;
- future snoozes remain upcoming after original due time passes;
- overdue snoozes use snooze due time;
- existing platform requests are cancelled before replacement/suppression/invalidation;
- cancellation failure remains retryable;
- stale occurrence identity is retained long enough to cancel obsolete platform requests;
- medicine/profile delete flows cancel future platform requests before database cascade and compensate when persistence fails;
- medicine/profile save flows reconcile reminder state before non-critical audit bookkeeping;
- appointment persistence/platform scheduling uses compensation;
- Taken/Skipped/Delayed/Missed/Snoozed/Cancelled actions use cancellation-first ordering and non-cancelled recovery when later essential work fails.

## SQLite and data storage

Primary references:

- [`architecture/DATABASE_SCHEMA.md`](architecture/DATABASE_SCHEMA.md)
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md)
- [`security/DEPENDENCY_RISK_REGISTER.md`](security/DEPENDENCY_RISK_REGISTER.md)
- [`releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`](releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md)
- [`CONFIGURATION_REFERENCE.md`](CONFIGURATION_REFERENCE.md)

Current dependency intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- central transitive pinning enabled;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- selected providers `2.1.12`;
- former exact advisory audit suppression removed.

A successful unsuppressed dependency audit is security evidence, not proof of packaged existing-database compatibility.

## Encrypted document vault

- [`architecture/DOCUMENT_VAULT.md`](architecture/DOCUMENT_VAULT.md)
- [`security/SECURITY_MODEL.md`](security/SECURITY_MODEL.md)
- [`security/THREAT_MODEL.md`](security/THREAT_MODEL.md)

New encrypted document payloads use authenticated chunked AEAD framing v2. Legacy v1 remains readable where required for compatibility. Missing/corrupt document key plus existing encrypted payload fails closed; CareNest does not silently create an unrelated replacement key for existing ciphertext.

Explicit decrypted export creates plaintext outside the encrypted vault boundary.

## Backup and restore

- [`architecture/BACKUP_AND_RESTORE.md`](architecture/BACKUP_AND_RESTORE.md)
- [`security/SECURITY_MODEL.md`](security/SECURITY_MODEL.md)
- [`testing/TESTING_GUIDE.md`](testing/TESTING_GUIDE.md)

Manual backups use password-derived authenticated encryption, versioned package metadata, strict decrypted archive topology validation, SQLite snapshot integrity, protected document-recovery key material, and wrong-password/tamper/truncation/trailing-data rejection.

Historical format compatibility is retained/documented rather than silently removed.

## Reports and exports

- [`REPORTS_AND_EXPORTS.md`](REPORTS_AND_EXPORTS.md)
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md)
- [`privacy/PRIVACY_MODEL.md`](privacy/PRIVACY_MODEL.md)

CSV formula-like user strings are neutralized in the portable spreadsheet representation. CSV/PDF/JSON generation uses staging plus atomic final move. Application-owned shared report cache files are removed after share handoff where CareNest still controls that temporary copy.

## Privacy and data lifecycle

- [`privacy/PRIVACY_MODEL.md`](privacy/PRIVACY_MODEL.md)
- [`privacy/DATA_LIFECYCLE.md`](privacy/DATA_LIFECYCLE.md)
- [`privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md`](privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md)
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md)
- [`../PRIVACY.md`](../PRIVACY.md)

These documents distinguish local application-owned data from user-exported/external copies and document deletion/cleanup limitations honestly.

## Security

- [`security/SECURITY_MODEL.md`](security/SECURITY_MODEL.md)
- [`security/THREAT_MODEL.md`](security/THREAT_MODEL.md)
- [`security/LOGGING_PRIVACY.md`](security/LOGGING_PRIVACY.md)
- [`security/DEPENDENCY_RISK_REGISTER.md`](security/DEPENDENCY_RISK_REGISTER.md)
- [`security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md`](security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md)
- [`security/BUG_AUDIT_SECURITY_NOTES_20260814.md`](security/BUG_AUDIT_SECURITY_NOTES_20260814.md)
- [`releases/SECURITY_RELEASE_REVIEW.md`](releases/SECURITY_RELEASE_REVIEW.md)
- [`../SECURITY.md`](../SECURITY.md)

Security controls are documented separately from residual risk. App lock is a local privacy barrier, not whole-database encryption. A compromised/rooted/jailbroken OS/device remains outside the app’s strongest guarantees.

## Logging privacy

See [`security/LOGGING_PRIVACY.md`](security/LOGGING_PRIVACY.md).

Normal sensitive operation logs must not contain health notes/document contents/PINs/passwords/keys or routine full sensitive exception messages/stack traces. Fixed operation/category metadata and exception type names are preferred where diagnostics are necessary.

## Design, branding, accessibility and localization

- [`design/DESIGN_SYSTEM.md`](design/DESIGN_SYSTEM.md)
- [`design/ACCESSIBILITY.md`](design/ACCESSIBILITY.md)
- [`design/LOCALIZATION.md`](design/LOCALIZATION.md)
- [`design/STORE_ASSETS.md`](design/STORE_ASSETS.md)
- [`releases/BMC_HIGHLIGHT.md`](releases/BMC_HIGHLIGHT.md)
- [`../BUY_ME_A_COFFEE.md`](../BUY_ME_A_COFFEE.md)

Automated XAML/source semantics tests do not replace real screen-reader, text-scaling, keyboard/focus, contrast/theme and reduced-motion verification.

## Developer setup

- [`setup/DEVELOPMENT.md`](setup/DEVELOPMENT.md)
- [`setup/PLATFORM_SETUP.md`](setup/PLATFORM_SETUP.md)
- [`setup/TROUBLESHOOTING.md`](setup/TROUBLESHOOTING.md)
- [`CONFIGURATION_REFERENCE.md`](CONFIGURATION_REFERENCE.md)

Repository-local maintainer identity:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Helpers:

```bash
build/scripts/setup-git.sh
```

```powershell
./build/scripts/setup-git.ps1
```

GitHub web/API commits can use authenticated GitHub account metadata; do not misrepresent them as arbitrary local-email commits unless the commit metadata proves it.

## Maintainer operations

- [`MAINTENANCE_AND_OPERATIONS.md`](MAINTENANCE_AND_OPERATIONS.md) — primary current maintainer manual.
- [`setup/MAINTAINER_OPERATIONS.md`](setup/MAINTAINER_OPERATIONS.md) — setup-oriented maintainer operations reference.
- [`DOCUMENTATION_STANDARDS.md`](DOCUMENTATION_STANDARDS.md) — documentation accuracy/evidence rules.
- [`../CONTRIBUTING.md`](../CONTRIBUTING.md) — contribution policy.

## Testing and CI

- [`testing/TESTING_GUIDE.md`](testing/TESTING_GUIDE.md)
- [`testing/TEST_PLAN.md`](testing/TEST_PLAN.md)
- [`testing/REMINDER_SCHEDULING_CONTRACT.md`](testing/REMINDER_SCHEDULING_CONTRACT.md)
- [`testing/SETTINGS_LIFECYCLE_CONTRACT.md`](testing/SETTINGS_LIFECYCLE_CONTRACT.md)
- [`testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`](testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md)

PR #61 core totals for the current frozen source:

- UnitTests: 122;
- IntegrationTests: 39;
- UiTests/source-policy: 157;
- total: 318.

The UiTests/source-policy suite includes architecture, repository, ViewModel, XAML/accessibility intent, logging/privacy, app-lock, reminder/platform, dependency-security, workflow, release-preflight, store-package workflow/preflight, store-inspection artifact/signing/provenance, quality-gate, Git-setup, package-metadata/privacy and production Release Gate contracts.

## Build/configuration automation

- [`CONFIGURATION_REFERENCE.md`](CONFIGURATION_REFERENCE.md)
- [`setup/DEVELOPMENT.md`](setup/DEVELOPMENT.md)
- [`setup/PLATFORM_SETUP.md`](setup/PLATFORM_SETUP.md)
- [`releases/STORE_BUILD_POLICY.md`](releases/STORE_BUILD_POLICY.md)

Local quality gate:

```bash
build/scripts/quality-gate.sh
```

```powershell
./build/scripts/quality-gate.ps1
```

Release preflight:

```bash
build/scripts/release-preflight.sh
```

```powershell
./build/scripts/release-preflight.ps1
```

Fail-closed store-package preflight:

```bash
CARENEST_TARGET=net10.0-android \
./build/scripts/store-package-preflight.sh
```

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
./build/scripts/store-package-preflight.ps1
```

The dedicated wrappers force `CARENEST_SHOW_FUNDING_LINK=false` and delegate the standard release preflight. Dependency audit remains blocking; it is not warning-only.

For reproducible non-production store-safe artifact generation, `.github/workflows/store-inspection-artifacts.yml` creates checksum/provenance-bearing Android/Windows/Apple internal artifacts. It does not configure production signing or replace installed-device/package validation.

## Release engineering

- [`releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md`](releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md) — current authoritative PR #61 exact automated/internal-artifact evidence.
- [`releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md`](releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md) — historical PR #59 exact automated store-safe compilation evidence.
- [`releases/STORE_POLICY_REVIEW_20260815.md`](releases/STORE_POLICY_REVIEW_20260815.md) — dated Apple/Google external support-link policy review.
- [`releases/STORE_BUILD_POLICY.md`](releases/STORE_BUILD_POLICY.md) — per-store voluntary support-link build/evidence policy and automated/local store-safe paths.
- [`releases/PACKAGED_RELEASE_VALIDATION.md`](releases/PACKAGED_RELEASE_VALIDATION.md) — packaged release evidence runbook and internal-artifact boundary.
- [`releases/PACKAGED_RELEASE_HARDENING_VERIFICATION_20260815.md`](releases/PACKAGED_RELEASE_HARDENING_VERIFICATION_20260815.md) — historical PR #58 evidence.
- [`releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md`](releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md) — historical PR #56 frozen-source evidence.
- [`releases/RELEASE_PROCESS.md`](releases/RELEASE_PROCESS.md) — end-to-end release process.
- [`releases/RELEASE_CHECKLIST.md`](releases/RELEASE_CHECKLIST.md) — automated/manual promotion checklist.
- [`releases/QUALITY_GATE.md`](releases/QUALITY_GATE.md) — production quality requirements.
- [`releases/MANUAL_TEST_MATRIX.md`](releases/MANUAL_TEST_MATRIX.md) — platform/manual/accessibility/SQLite compatibility evidence matrix.
- [`releases/SECURITY_RELEASE_REVIEW.md`](releases/SECURITY_RELEASE_REVIEW.md) — release security approval record.
- [`releases/STORE_SUBMISSION_CHECKLIST.md`](releases/STORE_SUBMISSION_CHECKLIST.md) — distribution/store preparation.
- [`releases/RELEASE_EVIDENCE.md`](releases/RELEASE_EVIDENCE.md) — exact-source evidence/provenance workflow behavior.
- [`releases/RELEASE_NOTES_TEMPLATE.md`](releases/RELEASE_NOTES_TEMPLATE.md) — release-note template.
- [`releases/VERIFICATION_BRANCH_PROTOCOL.md`](releases/VERIFICATION_BRANCH_PROTOCOL.md) — marker-only exact-head verification protocol.
- [`releases/NEXT_STEPS.md`](releases/NEXT_STEPS.md) — current blockers/roadmap.
- [`releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`](releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md) — SQLite remediation and packaged compatibility plan.
- [`releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md`](releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md) — documentation checklist.
- [`releases/DOCUMENTATION_AUDIT_20260814.md`](releases/DOCUMENTATION_AUDIT_20260814.md) — final documentation audit.

Production tags matching `v*` are configured to run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- CareNest Store Package Configuration;
- CareNest Store Inspection Artifacts;
- Release Gate;
- CareNest Release Evidence.

A tag is not production approval until every required automated and manual gate is actually complete.

## Verification and historical evidence

Latest completed exact automated/source-inspection verification:

- [`releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md`](releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md) — PR #61 frozen source with both build configurations plus corrected checksum/provenance-bearing internal artifacts.

Earlier retained evidence includes:

- PR #60 in Git/PR history — superseded first inspection-artifact checkpoint that exposed debug-signed Android companion/provenance defects.
- [`releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md`](releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md) — PR #59 default-plus-store-safe compilation baseline.
- [`releases/PACKAGED_RELEASE_HARDENING_VERIFICATION_20260815.md`](releases/PACKAGED_RELEASE_HARDENING_VERIFICATION_20260815.md) — PR #58 packaged-release/store-policy hardening baseline.
- [`releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md`](releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md) — PR #56 release-engineering baseline.
- [`releases/FINAL_BUG_AUDIT_VERIFICATION_20260814.md`](releases/FINAL_BUG_AUDIT_VERIFICATION_20260814.md) — PR #54 runtime bug-audit baseline.
- [`releases/BUG_AUDIT_VERIFICATION_20260814.md`](releases/BUG_AUDIT_VERIFICATION_20260814.md) — detailed bug-audit checkpoint/failure history.
- [`releases/PHASE8_VERIFICATION_EVIDENCE.md`](releases/PHASE8_VERIFICATION_EVIDENCE.md)
- [`releases/PHASE9_VERIFICATION_EVIDENCE.md`](releases/PHASE9_VERIFICATION_EVIDENCE.md)
- historical handoffs under [`history/`](history/).

Failed/superseded checkpoints remain historical evidence and are not silently relabeled as release baselines.

## Current production blockers

CareNest remains `1.0.0-rc.1`, not final public production `1.0.0`.

Real remaining evidence includes:

- actual signed Apple App Store candidate built with `CareNestShowFundingLink=false` under the current policy decision;
- actual signed Google Play candidate built with `CareNestShowFundingLink=false` under the current policy decision;
- installed packaged About-page inspection proving the external support card is absent;
- Android real-device/emulator manual matrix;
- Windows manual matrix;
- iOS/iPadOS manual matrix;
- Mac Catalyst manual matrix;
- real notification permission/delivery/restart/platform lifecycle checks;
- cancellation-first reminder action behavior against actual platform scheduling;
- Android alarm/battery/reboot/time/time-zone checks;
- packaged-target document/photo/report/backup behavior;
- representative packaged SQLite existing-data upgrade/integrity/readability after native/provider remediation;
- encrypted document and backup compatibility;
- canonical historical encrypted-format/backup fixtures where available;
- clean-install restore;
- real accessibility testing;
- submission-time Apple/Google store-policy re-review;
- package identifiers/version/build metadata inspection on actual production artifacts;
- signing identities/credentials outside Git;
- signed package generation/inspection;
- production package checksums and signing/notarization provenance;
- store screenshots/listing/privacy/data-safety metadata;
- successful exact production tag CareNest CI/CodeQL/audit/Store Package Configuration/Store Inspection Artifacts/Release Gate/Release Evidence;
- final version/build metadata, release notes, checksums and publication.

The dated 2026-08-15 Apple/Google support-link policy review is complete. It must still be repeated at actual submission time because policies/programs can change.

No documentation or CI commit should mark these remaining external/manual gates complete without actual evidence.

## Documentation maintenance rule

When behavior or release engineering changes:

1. update the lowest-level technical document describing the behavior;
2. update user-facing docs when observable behavior changes;
3. update architecture/privacy/security docs when data/trust/failure boundaries change;
4. update tests/contracts;
5. update package/build/config references if relevant;
6. update release status/evidence when verification changes;
7. update this index for any new major reference;
8. update `what_changed.md` for detailed continuation work;
9. run a fresh marker-only exact-head verification whenever runtime/test/project/workflow/package/platform/build-script/artifact-generation source changes after the current verified boundary.

Historical evidence should be preserved rather than silently rewritten. Current authoritative addenda/status documents should make the present state unambiguous.