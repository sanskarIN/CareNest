# CareNest Documentation Hub

<p align="center">
  <a href="https://ramsandesh.gumroad.com">
    <img src="assets/gumroad_store_badge.svg" alt="Shop on Gumroad — https://ramsandesh.gumroad.com" width="850" />
  </a>
</p>

**Release line:** `1.0.0-rc.1`  
**Documentation baseline:** 2026-08-18  
**Current automated baseline:** `releases/AUTOMATED_BASELINE.md`  
**Dependency/toolchain baseline:** `DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`  
**Current store-policy review:** `releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `releases/PACKAGE_EVIDENCE_TOOLING.md`  
**Documentation integrity guide:** `testing/DOCUMENTATION_INTEGRITY.md`

CareNest is a local-first .NET MAUI family health organizer. It is organizational software, not a diagnostic, treatment, dosage-calculation, clinical-interaction, clinical-risk or emergency-service system.

This directory is the canonical documentation hub. For the complete authority and audience map, start with [`DOCUMENTATION_CATALOG.md`](DOCUMENTATION_CATALOG.md).

## 🛍️ Ram Sandesh Gumroad Store

**[Shop on Gumroad → https://ramsandesh.gumroad.com](https://ramsandesh.gumroad.com)**

The storefront is promoted throughout repository/documentation support and marketing surfaces but remains separate from CareNest health functionality and packaged CareNest runtime resources.

- [`../GUMROAD.md`](../GUMROAD.md) — canonical storefront guide.
- [`marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`](marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md) — placement, safety, package-boundary and maintenance rules.
- [`assets/gumroad_store_badge.svg`](assets/gumroad_store_badge.svg) — repository-only promotional badge.

## Current verification authority

Use [`releases/AUTOMATED_BASELINE.md`](releases/AUTOMATED_BASELINE.md) for the latest **actually observed** exact-source automated result: source SHA, verification PR/marker, workflow IDs, test counts and conclusions.

Use [`releases/VERIFICATION_BRANCH_PROTOCOL.md`](releases/VERIFICATION_BRANCH_PROTOCOL.md) whenever verification-relevant source changes after the recorded baseline.

Permanent historical verification records remain useful for their own exact source boundaries, including:

- [`releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`](releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md);
- [`releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`](releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md).

Do not copy an older test count onto a newer source revision.

## Current dependency and toolchain authority

See [`DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`](DEPENDENCY_AND_TOOLCHAIN_BASELINE.md).

Executable source remains authoritative in `Directory.Packages.props`, `Directory.Build.props` and `.github/workflows/`.

The current candidate includes maintained .NET MAUI/test/coverage dependencies and maintained GitHub Actions majors; those combined changes still require exact-head verification before promotion as a new automated baseline.

## Current store-policy evidence

Dated pre-submission review: [`releases/STORE_POLICY_REVIEW_20260818.md`](releases/STORE_POLICY_REVIEW_20260818.md).

The review compares the current product/package boundary with relevant Apple, Google Play and Microsoft Store policy areas. It does **not** mean CareNest is store-approved and does not replace final policy/store-console review against the exact production binary/listing on the submission date.

## Structured final-package evidence

Guide: [`releases/PACKAGE_EVIDENCE_TOOLING.md`](releases/PACKAGE_EVIDENCE_TOOLING.md).

Source-controlled tooling:

- `../build/scripts/create-package-evidence.py`;
- `../build/scripts/create-package-evidence.sh`;
- `../build/scripts/create-package-evidence.ps1`;
- `../build/scripts/test-create-package-evidence.py`.

Production evidence requires an immutable `v*` source tag, tag/source/checked-out-HEAD agreement, clean tracked workspace, non-secret signing/notarization provenance, successful store-safe payload scanning, SHA-256 evidence and JSON output outside the package payload.

The tool does not sign artifacts, prove store approval, replace real-device/accessibility testing or replace packaged SQLite/encrypted-data compatibility evidence.

## Documentation integrity

Guide: [`testing/DOCUMENTATION_INTEGRITY.md`](testing/DOCUMENTATION_INTEGRITY.md).

Offline stable local-link verification:

```bash
python3 build/scripts/test-verify-documentation-links.py
python3 build/scripts/verify-documentation-links.py
```

The default exact-source gate excludes immutable `docs/history/` snapshots and the four dynamic post-verification status/evidence files. Use `--include-dynamic` for an explicit documentation-only audit after evidence promotion.

## Primary documentation

- [`DOCUMENTATION_CATALOG.md`](DOCUMENTATION_CATALOG.md) — complete navigation, audience and authority map.
- [`COMPLETE_PROJECT_DOCUMENTATION.md`](COMPLETE_PROJECT_DOCUMENTATION.md) — end-to-end project reference.
- [`DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`](DEPENDENCY_AND_TOOLCHAIN_BASELINE.md) — current package/action baseline and upgrade policy.
- [`EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`](EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md) — complete Windows/Android/iOS/Mac Catalyst executable/package guide.
- [`releases/EXECUTABLE_BUILD_CHECKLIST.md`](releases/EXECUTABLE_BUILD_CHECKLIST.md) — concise release-operator build checklist.
- [`releases/PACKAGE_EVIDENCE_TOOLING.md`](releases/PACKAGE_EVIDENCE_TOOLING.md) — final-package checksum/provenance tooling.
- [`releases/VERIFICATION_BRANCH_PROTOCOL.md`](releases/VERIFICATION_BRANCH_PROTOCOL.md) — exact-head verification procedure.
- [`releases/AUTOMATED_BASELINE.md`](releases/AUTOMATED_BASELINE.md) — mutable current automated evidence authority.
- [`GETTING_STARTED.md`](GETTING_STARTED.md) — quickest safe user/developer entry point.
- [`USER_GUIDE.md`](USER_GUIDE.md) — complete user workflows.
- [`FEATURE_REFERENCE.md`](FEATURE_REFERENCE.md) — feature-by-feature reference.
- [`USER_FAQ.md`](USER_FAQ.md) — common questions.
- [`KNOWN_LIMITATIONS.md`](KNOWN_LIMITATIONS.md) — intentional/external/RC limitations.
- [`DEVELOPER_REFERENCE.md`](DEVELOPER_REFERENCE.md) — current developer rules.
- [`PLATFORM_BEHAVIOR_MATRIX.md`](PLATFORM_BEHAVIOR_MATRIX.md) — automated versus manual platform evidence.
- [`CODEBASE_REFERENCE.md`](CODEBASE_REFERENCE.md) — source/project/file map.
- [`CONFIGURATION_REFERENCE.md`](CONFIGURATION_REFERENCE.md) — package/build/workflow configuration reference.
- [`MAINTENANCE_AND_OPERATIONS.md`](MAINTENANCE_AND_OPERATIONS.md) — maintainer operations.
- [`REPOSITORY_GOVERNANCE.md`](REPOSITORY_GOVERNANCE.md) — evidence/documentation governance.
- [`DOCUMENTATION_STANDARDS.md`](DOCUMENTATION_STANDARDS.md) — writing/maintenance standards.
- [`../what_changed.md`](../what_changed.md) — detailed active continuation handoff.

## Product/user documentation

- [`USER_GUIDE.md`](USER_GUIDE.md)
- [`FEATURE_REFERENCE.md`](FEATURE_REFERENCE.md)
- [`USER_FAQ.md`](USER_FAQ.md)
- [`KNOWN_LIMITATIONS.md`](KNOWN_LIMITATIONS.md)
- [`REPORTS_AND_EXPORTS.md`](REPORTS_AND_EXPORTS.md)
- [`GLOSSARY.md`](GLOSSARY.md)
- [`SUPPORT_CARENEST.md`](SUPPORT_CARENEST.md)
- [`../GUMROAD.md`](../GUMROAD.md)
- [`../PRIVACY.md`](../PRIVACY.md)
- [`../TERMS.md`](../TERMS.md)
- [`../SUPPORT.md`](../SUPPORT.md)
- [`../SECURITY.md`](../SECURITY.md)

## Product safety boundary

CareNest does **not**:

- diagnose conditions;
- calculate or infer medicine dosage;
- recommend treatment;
- perform clinical medication-interaction checking;
- calculate clinical risk scores;
- independently verify adherence;
- replace a clinician/pharmacist;
- provide emergency services;
- guarantee notification delivery.

Medicine strength/instruction values remain user-entered organizational text. Schedules originate from explicit user input.

## Architecture

- [`architecture/ARCHITECTURE.md`](architecture/ARCHITECTURE.md)
- [`architecture/APPLICATION_FLOWS.md`](architecture/APPLICATION_FLOWS.md)
- [`architecture/SERVICE_BOUNDARIES.md`](architecture/SERVICE_BOUNDARIES.md)
- [`architecture/DATABASE_SCHEMA.md`](architecture/DATABASE_SCHEMA.md)
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md)
- [`architecture/DOCUMENT_VAULT.md`](architecture/DOCUMENT_VAULT.md)
- [`architecture/BACKUP_AND_RESTORE.md`](architecture/BACKUP_AND_RESTORE.md)
- [`architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md)
- [`architecture/ADR-0001-local-first.md`](architecture/ADR-0001-local-first.md)
- [`architecture/ADR-0002-reminder-occurrences.md`](architecture/ADR-0002-reminder-occurrences.md)
- [`architecture/ADR-0003-encrypted-backup-format.md`](architecture/ADR-0003-encrypted-backup-format.md)

Intended dependency direction:

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

Platform-neutral projects must not accidentally depend on MAUI. ViewModels should not issue direct SQL or casually create network clients.

## Current platform targets

- Android: `net10.0-android`, minimum API 24.
- iOS/iPadOS: `net10.0-ios`, minimum iOS 15.
- Mac Catalyst: `net10.0-maccatalyst`, minimum 15.
- Windows: `net10.0-windows10.0.19041.0`, minimum Windows 10.0.19041.0.

See [`setup/PLATFORM_SETUP.md`](setup/PLATFORM_SETUP.md), [`EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`](EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md) and [`PLATFORM_BEHAVIOR_MATRIX.md`](PLATFORM_BEHAVIOR_MATRIX.md).

## Strict XAML compiled-binding policy

The application project enables Source binding compilation and strict XAML compilation and promotes `XC0022`, `XC0023`, `XC0024`, `XC0025` to errors.

Binding-bearing pages/templates are required to carry accurate binding-context types. See [`releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`](releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md) and [`DEVELOPER_REFERENCE.md`](DEVELOPER_REFERENCE.md).

## Privacy

- [`privacy/PRIVACY_MODEL.md`](privacy/PRIVACY_MODEL.md)
- [`privacy/DATA_LIFECYCLE.md`](privacy/DATA_LIFECYCLE.md)
- [`privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md`](privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md)
- [`../PRIVACY.md`](../PRIVACY.md)

Current v1 intentionally has no required CareNest account/backend, no automatic CareNest cloud upload and no hidden runtime analytics/telemetry client. Explicit exports/shares can create copies outside CareNest control.

## Security

- [`security/SECURITY_MODEL.md`](security/SECURITY_MODEL.md)
- [`security/THREAT_MODEL.md`](security/THREAT_MODEL.md)
- [`security/LOGGING_PRIVACY.md`](security/LOGGING_PRIVACY.md)
- [`security/DEPENDENCY_RISK_REGISTER.md`](security/DEPENDENCY_RISK_REGISTER.md)
- [`security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md`](security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md)
- [`releases/SECURITY_RELEASE_REVIEW.md`](releases/SECURITY_RELEASE_REVIEW.md)

Structured SQLite data is local/sandboxed but is not claimed as transparently whole-database encrypted. Imported document payloads and manual backups use separate encryption protections.

## Dependency security

Current source versions and action majors: [`DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`](DEPENDENCY_AND_TOOLCHAIN_BASELINE.md).

The former exact SQLite audit suppression remains removed. A green dependency audit does not replace packaged existing-database compatibility testing.

## Reminder documentation

- [`testing/REMINDER_SCHEDULING_CONTRACT.md`](testing/REMINDER_SCHEDULING_CONTRACT.md)
- [`architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md)
- [`PLATFORM_BEHAVIOR_MATRIX.md`](PLATFORM_BEHAVIOR_MATRIX.md)

The model separates explicit user intent, persisted occurrence state and OS request state. Reconciliation/compensation is required because database and platform scheduling cannot be committed atomically.

## Design, accessibility and localization

- [`design/DESIGN_SYSTEM.md`](design/DESIGN_SYSTEM.md)
- [`design/ACCESSIBILITY.md`](design/ACCESSIBILITY.md)
- [`design/LOCALIZATION.md`](design/LOCALIZATION.md)
- [`design/STORE_ASSETS.md`](design/STORE_ASSETS.md)

Automated source/semantics checks do not replace real assistive-technology evidence.

## Setup and developer operations

- [`setup/DEVELOPMENT.md`](setup/DEVELOPMENT.md)
- [`setup/PLATFORM_SETUP.md`](setup/PLATFORM_SETUP.md)
- [`setup/TROUBLESHOOTING.md`](setup/TROUBLESHOOTING.md)
- [`setup/MAINTAINER_OPERATIONS.md`](setup/MAINTAINER_OPERATIONS.md)
- [`EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`](EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md)
- [`DEVELOPER_REFERENCE.md`](DEVELOPER_REFERENCE.md)
- [`CONFIGURATION_REFERENCE.md`](CONFIGURATION_REFERENCE.md)

## Testing

- [`testing/TESTING_GUIDE.md`](testing/TESTING_GUIDE.md)
- [`testing/TEST_PLAN.md`](testing/TEST_PLAN.md)
- [`testing/REMINDER_SCHEDULING_CONTRACT.md`](testing/REMINDER_SCHEDULING_CONTRACT.md)
- [`testing/SETTINGS_LIFECYCLE_CONTRACT.md`](testing/SETTINGS_LIFECYCLE_CONTRACT.md)
- [`testing/DOCUMENTATION_INTEGRITY.md`](testing/DOCUMENTATION_INTEGRITY.md)
- [`../tests/CareNest.UiTests/ReleaseDocumentationConsistencyContractTests.cs`](../tests/CareNest.UiTests/ReleaseDocumentationConsistencyContractTests.cs)
- [`../tests/CareNest.UiTests/PackageEvidenceToolContractTests.cs`](../tests/CareNest.UiTests/PackageEvidenceToolContractTests.cs)
- [`../tests/CareNest.UiTests/DocumentationIntegrityToolContractTests.cs`](../tests/CareNest.UiTests/DocumentationIntegrityToolContractTests.cs)

Current test counts belong in [`releases/AUTOMATED_BASELINE.md`](releases/AUTOMATED_BASELINE.md) only after the exact source has actually run.

## Release and production validation

Current operational documents:

- [`EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`](EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md)
- [`releases/EXECUTABLE_BUILD_CHECKLIST.md`](releases/EXECUTABLE_BUILD_CHECKLIST.md)
- [`releases/PACKAGE_EVIDENCE_TOOLING.md`](releases/PACKAGE_EVIDENCE_TOOLING.md)
- [`releases/AUTOMATED_BASELINE.md`](releases/AUTOMATED_BASELINE.md)
- [`releases/NEXT_STEPS.md`](releases/NEXT_STEPS.md)
- [`releases/VERIFICATION_BRANCH_PROTOCOL.md`](releases/VERIFICATION_BRANCH_PROTOCOL.md)
- [`releases/RELEASE_PROCESS.md`](releases/RELEASE_PROCESS.md)
- [`releases/RELEASE_CHECKLIST.md`](releases/RELEASE_CHECKLIST.md)
- [`releases/QUALITY_GATE.md`](releases/QUALITY_GATE.md)
- [`releases/MANUAL_TEST_MATRIX.md`](releases/MANUAL_TEST_MATRIX.md)
- [`releases/PACKAGED_RELEASE_VALIDATION.md`](releases/PACKAGED_RELEASE_VALIDATION.md)
- [`releases/SECURITY_RELEASE_REVIEW.md`](releases/SECURITY_RELEASE_REVIEW.md)
- [`releases/STORE_BUILD_POLICY.md`](releases/STORE_BUILD_POLICY.md)
- [`releases/STORE_SUBMISSION_CHECKLIST.md`](releases/STORE_SUBMISSION_CHECKLIST.md)
- [`releases/STORE_POLICY_REVIEW_20260818.md`](releases/STORE_POLICY_REVIEW_20260818.md)
- [`releases/RELEASE_EVIDENCE.md`](releases/RELEASE_EVIDENCE.md)

Automated source completeness is not equivalent to production approval. Real-device/platform, accessibility, packaged compatibility, production signing, final package provenance, live store declarations, submission-day policy review, immutable tagged gates and publication/store approval remain distinct evidence categories.

## Historical evidence

`history/` and dated release documents preserve older source/test/configuration boundaries. Do not rewrite historical evidence merely to make old snapshots look current.

## Repository/community files

At repository root / `.github/`:

- `../CONTRIBUTING.md`
- `../CODE_OF_CONDUCT.md`
- `../SECURITY.md`
- `../SUPPORT.md`
- `../.github/ISSUE_TEMPLATE/bug_report.yml`
- `../.github/ISSUE_TEMPLATE/feature_request.yml`
- `../.github/ISSUE_TEMPLATE/config.yml`
- `../.github/PULL_REQUEST_TEMPLATE.md`
- `../.github/CODEOWNERS`

Public issues/tests/screenshots must use fictional/synthetic information. Security reports and sensitive data belong in private reporting channels.
