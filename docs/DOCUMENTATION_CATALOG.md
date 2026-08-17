# CareNest Documentation Catalog

**Release line:** `1.0.0-rc.1`  
**Documentation baseline date:** 2026-08-17  
**Latest verified Gumroad rollout source:** `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`  
**Compiled-binding verification source head:** `8908fa9f5f6d2b47123627e91f5aa5925d34a3c9` (PR #74)

This catalog is the navigation and ownership map for the complete CareNest documentation set. It does not replace the detailed documents; it tells readers which document is authoritative for each question.

## 1. Documentation precedence

When documents appear to disagree, use this order:

1. `PROJECT_STATUS.md` for current release state and blockers.
2. `docs/releases/NEXT_STEPS.md` for remaining production work.
3. latest dated verification record under `docs/releases/` for automated evidence.
4. `docs/COMPLETE_PROJECT_DOCUMENTATION.md` for the current end-to-end product/engineering reference.
5. specialized architecture/security/testing/setup documents for implementation details.
6. `GUMROAD.md` and `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` for the current external storefront placement/package boundary.
7. `what_changed.md` and `docs/history/` for chronological and historical context.

Historical evidence is intentionally retained and may describe older source boundaries. A dated historical file must not be interpreted as the current product state merely because it contains a detailed test count or source SHA.

## 2. Start here by audience

### End users and evaluators

- `README.md` — product overview, scope, limitations, current release status and highlighted Gumroad storefront link.
- `docs/GETTING_STARTED.md` — first-use and first-build orientation.
- `docs/USER_GUIDE.md` — complete use workflows.
- `docs/FEATURE_REFERENCE.md` — feature-by-feature behavior.
- `docs/USER_FAQ.md` — common questions and limitations.
- `docs/KNOWN_LIMITATIONS.md` — intentionally unsupported or externally constrained behavior.
- `docs/REPORTS_AND_EXPORTS.md` — exports, reports, privacy boundaries.
- `docs/GLOSSARY.md` — terminology.
- `GUMROAD.md` — official Ram Sandesh storefront guide and CareNest separation boundary.

### Contributors and developers

- `CONTRIBUTING.md` — contribution rules.
- `docs/DEVELOPER_REFERENCE.md` — development conventions and current technical baseline.
- `docs/setup/DEVELOPMENT.md` — environment and build/test setup.
- `docs/setup/PLATFORM_SETUP.md` — target-specific requirements.
- `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md` — authoritative executable/package creation guide for Windows, Android, iOS and Mac Catalyst, including complete repository-file coverage.
- `docs/setup/TROUBLESHOOTING.md` — build/runtime troubleshooting.
- `docs/CODEBASE_REFERENCE.md` — file/project responsibility map.
- `docs/CONFIGURATION_REFERENCE.md` — package/build/configuration reference.
- `docs/architecture/ARCHITECTURE.md` — solution architecture.
- `docs/architecture/SERVICE_BOUNDARIES.md` — dependency/service boundaries.
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — storefront placement and runtime-package exclusion rules.

### Maintainers and release operators

- `docs/MAINTENANCE_AND_OPERATIONS.md` — maintenance and incident operations.
- `docs/setup/MAINTAINER_OPERATIONS.md` — setup/repository operations.
- `docs/REPOSITORY_GOVERNANCE.md` — documentation/source/evidence governance.
- `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md` — complete commands, outputs, signing boundaries, package validation and executable troubleshooting.
- `docs/releases/EXECUTABLE_BUILD_CHECKLIST.md` — concise operator checklist for producing and validating platform artifacts.
- `docs/releases/RELEASE_PROCESS.md` — production release process.
- `docs/releases/RELEASE_CHECKLIST.md` — release gate checklist.
- `docs/releases/MANUAL_TEST_MATRIX.md` — manual platform validation.
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md` — packaged compatibility validation.
- `docs/releases/NEXT_STEPS.md` — current remaining work.
- `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md` — current Gumroad rollout automated evidence.
- `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md` — permanent compiled-binding automated verification record.
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — external-commerce package-boundary rules.

### Security/privacy reviewers

- `PRIVACY.md` — user-facing privacy statement.
- `SECURITY.md` — security policy/reporting.
- `docs/privacy/PRIVACY_MODEL.md` — privacy architecture.
- `docs/privacy/DATA_LIFECYCLE.md` — data lifecycle.
- `docs/security/SECURITY_MODEL.md` — technical security model.
- `docs/security/THREAT_MODEL.md` — threat model and residual risk.
- `docs/security/LOGGING_PRIVACY.md` — diagnostic/logging policy.
- `docs/security/DEPENDENCY_RISK_REGISTER.md` — dependency security status.
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — no-health-entitlement and no-runtime-storefront policy.

### QA and verification

- `docs/testing/TESTING_GUIDE.md` — testing strategy and commands.
- `docs/testing/TEST_PLAN.md` — test plan.
- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` — reminder correctness contract.
- `docs/testing/SETTINGS_LIFECYCLE_CONTRACT.md` — settings lifecycle contract.
- `docs/PLATFORM_BEHAVIOR_MATRIX.md` — automated versus manual platform evidence.
- `docs/releases/EXECUTABLE_BUILD_CHECKLIST.md` — executable/package validation checklist.
- `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md` — exact source/test/build/security result for the Gumroad rollout.
- `tests/CareNest.UiTests/FundingLinkContractTests.cs` — repository/storefront placement contracts.
- `tests/CareNest.UiTests/StoreFundingPayloadContractTests.cs` — store-payload external-commerce exclusion contracts.
- dated verification documents under `docs/releases/` — exact workflow evidence.

### Design, accessibility and localization

- `docs/design/DESIGN_SYSTEM.md` — visual/component principles.
- `docs/design/ACCESSIBILITY.md` — accessibility requirements and manual evidence needs.
- `docs/design/LOCALIZATION.md` — localization/RTL strategy.
- `docs/design/STORE_ASSETS.md` — screenshots and store-asset guidance.
- `docs/assets/gumroad_store_badge.svg` — repository-only storefront badge with accessible title/description text.

## 3. Architecture set

The architecture documentation is intentionally layered:

- `docs/architecture/ARCHITECTURE.md` — system overview and dependency direction.
- `docs/architecture/APPLICATION_FLOWS.md` — user/action/runtime flows.
- `docs/architecture/SERVICE_BOUNDARIES.md` — service and project responsibilities.
- `docs/architecture/DATABASE_SCHEMA.md` — schema, entities, migrations and indexes.
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md` — local storage and external-copy boundaries.
- `docs/architecture/DOCUMENT_VAULT.md` — encrypted imported-document storage.
- `docs/architecture/BACKUP_AND_RESTORE.md` — encrypted backup and restore.
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md` — reminder/notification platform integration.
- `docs/architecture/ADR-0001-local-first.md` — local-first decision.
- `docs/architecture/ADR-0002-reminder-occurrences.md` — reminder-occurrence decision.
- `docs/architecture/ADR-0003-encrypted-backup-format.md` — backup format decision.

## 4. Executable and package documentation ownership

`docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md` owns the end-to-end executable/package creation procedure. It documents:

- every repository area as build input, validation input, package input, documentation-only input or generated output;
- the single MAUI application project that must be published;
- Windows unpackaged `.exe` publication;
- Android APK/AAB publication and signing boundaries;
- iOS IPA publication and Apple signing/provisioning boundaries;
- Mac Catalyst `.app`/`.pkg` publication and signing/notarization boundaries;
- restore, quality gate, release preflight, tests and dependency audit;
- output locations, clean rebuild behavior and common failures;
- artifact hashing, evidence and secret-handling requirements.

`docs/releases/EXECUTABLE_BUILD_CHECKLIST.md` is the concise operator companion. It does not replace the full guide or the release/store/security documents.

Repository-only Gumroad promotional files are documentation inputs, not CareNest application package inputs. Store-safe payload scanning rejects `ramsandesh.gumroad.com` if it appears in built application output.

## 5. Current source facts that documentation must preserve

The current verified application project targets:

- `net10.0-android` with minimum Android API 24;
- `net10.0-ios` with minimum iOS 15;
- `net10.0-maccatalyst` with minimum Mac Catalyst 15;
- `net10.0-windows10.0.19041.0` with minimum Windows target 10.0.19041.0.

Current application identity/version:

- title: `CareNest`;
- application ID: `com.sanskar.carenest`;
- display version: `1.0.0-rc.1`;
- build/application version: `1`.

Current XAML compiler policy:

- `MauiEnableXamlCBindingWithSourceCompilation=true`;
- `MauiStrictXamlCompilation=true`;
- `XC0022`, `XC0023`, `XC0024`, `XC0025` are warnings-as-errors;
- no matching warning suppression is part of the intended baseline.

Current central package versions include:

- Microsoft.Maui.Controls `10.0.20`;
- sqlite-net-pcl `1.9.172`;
- SQLitePCLRaw.bundle_green `2.1.11`;
- SQLitePCLRaw.lib.e_sqlite3 `3.53.3`;
- SQLitePCLRaw Android/provider leaves `2.1.12` where pinned;
- xUnit `2.9.3`;
- Microsoft.NET.Test.Sdk `17.14.1`.

## 6. Current automated evidence

Latest verified Gumroad rollout implementation/source-policy source:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

Verified results on that exact revision:

- 122 unit tests passed;
- 39 integration tests passed;
- 175 UI/source-policy tests passed;
- **336/336 total core tests passed**;
- Android Release build passed;
- Windows Release build passed;
- iOS simulator Release build passed;
- Mac Catalyst Release build passed;
- all four store-candidate configurations passed;
- CodeQL passed.

Verification record: `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`.

The PR #74 compiled-binding evidence remains at `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md` for the exact source it verified.

## 7. Gumroad storefront documentation ownership

Canonical storefront URL:

`https://ramsandesh.gumroad.com`

Authority map:

- `GUMROAD.md` — canonical reader-facing storefront guide and CareNest separation statement;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — maintainer placement/package policy;
- `docs/assets/gumroad_store_badge.svg` — repository-only promotional graphic;
- `README.md`, `SUPPORT.md`, `.github/FUNDING.yml` — highlighted repository entry points;
- `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md` — exact rollout automated evidence;
- `tests/CareNest.UiTests/FundingLinkContractTests.cs` — repository-placement regression contract;
- `tests/CareNest.UiTests/StoreFundingPayloadContractTests.cs` — application/package exclusion regression contract;
- `build/scripts/verify-store-safe-payload.py` — byte-level package scanner.

The Gumroad storefront is not a CareNest health entitlement and must not be represented as medical advice, diagnosis, treatment, dosage guidance, reminder reliability, emergency service, or health-data access.

## 8. Historical documentation

`docs/history/` contains exact snapshots of previously active handoff/status/documentation files. Dated release documents intentionally retain older verification boundaries. Do not rewrite historical evidence merely to make old files look current or to retroactively insert Gumroad promotion.

## 9. Production status boundary

Documentation completeness does not mean production release completeness.

CareNest remains `1.0.0-rc.1`. The Gumroad rollout is automated-verified at the named implementation/source-policy baseline, while remaining production gates include real-device behavior, accessibility validation, packaged existing-data/encrypted-data compatibility, production signing, final signed-package inspection, current store-policy review, production metadata, exact approved production source/tag, tagged release gates and publication evidence.

Use `docs/releases/NEXT_STEPS.md` as the authoritative operational checklist.
