# CareNest Documentation Hub

<p align="center">
  <a href="https://ramsandesh.gumroad.com">
    <img src="assets/gumroad_store_badge.svg" alt="Shop on Gumroad — https://ramsandesh.gumroad.com" width="850" />
  </a>
</p>

**Release line:** `1.0.0-rc.1`  
**Documentation baseline:** 2026-08-17  
**Latest fully verified pre-storefront source:** `7cbe5568b6cffa06c279b29f3cb1b107ea988791`  
**Compiled-binding verification head:** `8908fa9f5f6d2b47123627e91f5aa5925d34a3c9` (PR #74)

CareNest is a local-first .NET MAUI family health organizer. It is organizational software, not a diagnostic, treatment, dosage-calculation, clinical-interaction, clinical-risk or emergency-service system.

This directory is the canonical documentation hub. For a complete map of authority and audience-specific reading paths, start with [`DOCUMENTATION_CATALOG.md`](DOCUMENTATION_CATALOG.md).

## 🛍️ Ram Sandesh Gumroad Store

**[Shop on Gumroad → https://ramsandesh.gumroad.com](https://ramsandesh.gumroad.com)**

The storefront is promoted throughout current repository/documentation support and marketing surfaces but remains separate from CareNest health functionality and from packaged CareNest runtime resources.

- [`../GUMROAD.md`](../GUMROAD.md) — canonical storefront guide.
- [`marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`](marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md) — placement, safety, package-boundary and maintenance rules.
- [`assets/gumroad_store_badge.svg`](assets/gumroad_store_badge.svg) — repository-only promotional badge.

## Current authoritative automated evidence

The latest fully verified pre-storefront source `7cbe5568b6cffa06c279b29f3cb1b107ea988791` passed:

- 122/122 unit tests;
- 39/39 integration tests;
- 173/173 UI/source-policy tests;
- **334/334 total core tests**;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- all four store-candidate configurations;
- CodeQL.

The earlier PR #74 compiled-binding verification remains permanent evidence for strict XAML compilation: [`releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`](releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md).

Current status: [`../PROJECT_STATUS.md`](../PROJECT_STATUS.md).  
Remaining production work: [`releases/NEXT_STEPS.md`](releases/NEXT_STEPS.md).

## Primary documentation

- [`DOCUMENTATION_CATALOG.md`](DOCUMENTATION_CATALOG.md) — complete navigation, audience and authority map.
- [`COMPLETE_PROJECT_DOCUMENTATION.md`](COMPLETE_PROJECT_DOCUMENTATION.md) — end-to-end whole-project reference.
- [`EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`](EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md) — complete Windows/Android/iOS/Mac Catalyst executable, package, signing, validation and repository-file coverage guide.
- [`releases/EXECUTABLE_BUILD_CHECKLIST.md`](releases/EXECUTABLE_BUILD_CHECKLIST.md) — concise release-operator checklist and copy/paste build commands.
- [`GETTING_STARTED.md`](GETTING_STARTED.md) — quickest safe entry point for users/developers.
- [`USER_GUIDE.md`](USER_GUIDE.md) — complete user workflows.
- [`FEATURE_REFERENCE.md`](FEATURE_REFERENCE.md) — feature-by-feature reference.
- [`USER_FAQ.md`](USER_FAQ.md) — common questions.
- [`KNOWN_LIMITATIONS.md`](KNOWN_LIMITATIONS.md) — intentional/external/RC limitations.
- [`DEVELOPER_REFERENCE.md`](DEVELOPER_REFERENCE.md) — current developer rules and source baseline.
- [`PLATFORM_BEHAVIOR_MATRIX.md`](PLATFORM_BEHAVIOR_MATRIX.md) — automated versus manual platform evidence.
- [`CODEBASE_REFERENCE.md`](CODEBASE_REFERENCE.md) — source/project/file map.
- [`CONFIGURATION_REFERENCE.md`](CONFIGURATION_REFERENCE.md) — package/build/configuration reference.
- [`MAINTENANCE_AND_OPERATIONS.md`](MAINTENANCE_AND_OPERATIONS.md) — maintainer operations.
- [`REPOSITORY_GOVERNANCE.md`](REPOSITORY_GOVERNANCE.md) — evidence/documentation governance.
- [`DOCUMENTATION_STANDARDS.md`](DOCUMENTATION_STANDARDS.md) — writing/maintenance standards.
- [`../GUMROAD.md`](../GUMROAD.md) — canonical Gumroad storefront documentation.
- [`marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`](marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md) — repository/storefront placement and package-compliance guide.
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

- [`architecture/ARCHITECTURE.md`](architecture/ARCHITECTURE.md) — complete solution architecture.
- [`architecture/APPLICATION_FLOWS.md`](architecture/APPLICATION_FLOWS.md) — runtime/user flows.
- [`architecture/SERVICE_BOUNDARIES.md`](architecture/SERVICE_BOUNDARIES.md) — project/service ownership.
- [`architecture/DATABASE_SCHEMA.md`](architecture/DATABASE_SCHEMA.md) — schema, migrations and indexes.
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md) — storage/export/delete boundaries.
- [`architecture/DOCUMENT_VAULT.md`](architecture/DOCUMENT_VAULT.md) — encrypted documents.
- [`architecture/BACKUP_AND_RESTORE.md`](architecture/BACKUP_AND_RESTORE.md) — backup/restore architecture.
- [`architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md) — reminder/notification integration.
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

All binding-bearing pages/templates are required to carry accurate binding-context types. See [`releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`](releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md) and [`DEVELOPER_REFERENCE.md`](DEVELOPER_REFERENCE.md).

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
- [`security/BUG_AUDIT_SECURITY_NOTES_20260814.md`](security/BUG_AUDIT_SECURITY_NOTES_20260814.md) — historical audit evidence.

Structured SQLite data is local/sandboxed but not claimed as transparently whole-database encrypted. Imported document payloads and manual backups use separate encryption protections.

## SQLite/dependency security

Current central intent includes:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- Android/provider leaves `2.1.12` where pinned;
- former exact advisory suppression removed.

A green dependency audit does not replace packaged existing-database compatibility testing.

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

## Testing

- [`testing/TESTING_GUIDE.md`](testing/TESTING_GUIDE.md)
- [`testing/TEST_PLAN.md`](testing/TEST_PLAN.md)
- [`testing/REMINDER_SCHEDULING_CONTRACT.md`](testing/REMINDER_SCHEDULING_CONTRACT.md)
- [`testing/SETTINGS_LIFECYCLE_CONTRACT.md`](testing/SETTINGS_LIFECYCLE_CONTRACT.md)
- [`testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`](testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md) — historical defect/test mapping.

Latest fully verified pre-storefront baseline: 122 unit + 39 integration + 173 UI/source-policy = 334 tests.

## Release and production validation

Current operational documents:

- [`EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`](EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md)
- [`releases/EXECUTABLE_BUILD_CHECKLIST.md`](releases/EXECUTABLE_BUILD_CHECKLIST.md)
- [`releases/NEXT_STEPS.md`](releases/NEXT_STEPS.md)
- [`releases/RELEASE_PROCESS.md`](releases/RELEASE_PROCESS.md)
- [`releases/RELEASE_CHECKLIST.md`](releases/RELEASE_CHECKLIST.md)
- [`releases/QUALITY_GATE.md`](releases/QUALITY_GATE.md)
- [`releases/MANUAL_TEST_MATRIX.md`](releases/MANUAL_TEST_MATRIX.md)
- [`releases/PACKAGED_RELEASE_VALIDATION.md`](releases/PACKAGED_RELEASE_VALIDATION.md)
- [`releases/SECURITY_RELEASE_REVIEW.md`](releases/SECURITY_RELEASE_REVIEW.md)
- [`releases/STORE_SUBMISSION_CHECKLIST.md`](releases/STORE_SUBMISSION_CHECKLIST.md)
- [`releases/RELEASE_EVIDENCE.md`](releases/RELEASE_EVIDENCE.md)
- [`releases/RELEASE_NOTES_TEMPLATE.md`](releases/RELEASE_NOTES_TEMPLATE.md)
- [`releases/VERIFICATION_BRANCH_PROTOCOL.md`](releases/VERIFICATION_BRANCH_PROTOCOL.md)
- [`releases/STORE_BUILD_POLICY.md`](releases/STORE_BUILD_POLICY.md)

Current verification references:

- latest fully verified pre-storefront source: `7cbe5568b6cffa06c279b29f3cb1b107ea988791`;
- [`releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`](releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md) — permanent compiled-binding verification;
- [`releases/FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md`](releases/FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md) — earlier package-boundary verification.

Older PR #68/#67/#61/#59/#58/#56/#54 records remain historical evidence for the exact source they verified.

## Application-package external-commerce boundary

The current distributed application source/package contains no external Buy Me a Coffee or Gumroad destination/card/command/promotional artwork. Repository-only funding and storefront information remain separate and do not unlock health functionality or clinical services.

The store-safe payload scanner is required to reject both repository-only markers if they appear in packaged output:

- `buymeacoffee.com/sanskarIN`;
- `ramsandesh.gumroad.com`.

Historical files may document the earlier funding-toggle investigation, but that architecture is not the current product boundary.

## Documentation completeness and governance

- [`releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md`](releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md)
- [`releases/DOCUMENTATION_AUDIT_20260816.md`](releases/DOCUMENTATION_AUDIT_20260816.md)
- [`DOCUMENTATION_STANDARDS.md`](DOCUMENTATION_STANDARDS.md)
- [`REPOSITORY_GOVERNANCE.md`](REPOSITORY_GOVERNANCE.md)
- [`marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`](marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md)

## Historical snapshots

`history/` contains exact snapshots of previously active handoff/status/documentation surfaces. Do not treat an older snapshot as current merely because it contains detailed source/test evidence.

## Production state

CareNest remains `1.0.0-rc.1`.

The source-controlled feature scope and current strict-binding cleanup are automated-verified, but production publication still requires real-device/accessibility/package-compatibility/signing/store/tag evidence.

Do not describe CareNest as globally bug-free, production-signed, store-approved or production-published until those external gates are actually completed.
