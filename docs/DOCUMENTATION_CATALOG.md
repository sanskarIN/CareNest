# CareNest Documentation Catalog

**Release line:** `1.0.0-rc.1`  
**Documentation baseline date:** 2026-08-23  
**Current automated baseline record:** `docs/releases/AUTOMATED_BASELINE.md`  
**Production evidence standard:** `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`  
**Production evidence index:** `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`  
**Cross-platform guide:** `docs/setup/CROSS_PLATFORM.md`  
**Current dependency/toolchain baseline:** `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`  
**Documentation integrity guide:** `docs/testing/DOCUMENTATION_INTEGRITY.md`

This catalog is the navigation and ownership map for the complete CareNest documentation set. It does not replace detailed documents; it identifies which document is authoritative for each question.

The pre-cross-platform catalog is preserved exactly at `docs/history/cross-platform-before-catalog-20260823/DOCUMENTATION_CATALOG.md`.

## 1. Documentation precedence

When documents appear to disagree, use this order:

1. `PROJECT_STATUS.md` for current release state, current continuation and blockers.
2. `docs/releases/AUTOMATED_BASELINE.md` for the latest actually observed accepted exact-source automated verification record.
3. `docs/releases/NEXT_STEPS.md` for remaining production work and the immediate exact-head verification gate.
4. `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` for production evidence quality and result-state semantics.
5. `docs/releases/PRODUCTION_EVIDENCE_INDEX.md` for release-specific production evidence workflow and canonical templates.
6. `docs/setup/CROSS_PLATFORM.md` for Linux/browser build hosts, architecture and capability/parity boundaries.
7. latest dated verification record under `docs/releases/` for permanent exact automated evidence.
8. `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` for exact-head verification rules when current source moves beyond the accepted baseline.
9. `docs/releases/RELEASE_EVIDENCE.md` for the combined automated/manual/security/store/signing/package evidence contract.
10. `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` for structured final-package checksum/provenance evidence.
11. `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md` for the current human-readable package/GitHub Actions source baseline.
12. `docs/testing/DOCUMENTATION_INTEGRITY.md` for stable/dynamic/historical documentation-link verification boundaries.
13. `docs/releases/STORE_POLICY_REVIEW_20260818.md` for the dated pre-submission store-policy review; official live policies remain authoritative at actual submission time.
14. `docs/COMPLETE_PROJECT_DOCUMENTATION.md` for the broad product/engineering reference.
15. specialized architecture/security/testing/setup documents for implementation details.
16. `GUMROAD.md` and `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` for external storefront placement/package boundaries.
17. `what_changed.md` and `docs/history/` for chronological and historical context.

Historical evidence intentionally describes older source boundaries. A dated historical file must not be interpreted as the current product state merely because it contains detailed test counts or source SHAs.

## 2. Current platform documentation boundary

CareNest currently has two presentation-host families:

### Established .NET MAUI application

- Android: `net10.0-android`;
- iOS/iPadOS: `net10.0-ios`;
- Mac Catalyst: `net10.0-maccatalyst`;
- Windows: `net10.0-windows10.0.19041.0`.

### Cross-platform presentation hosts

- Linux-capable desktop: `CareNest.CrossPlatform.Desktop`, `net10.0`, Avalonia Desktop;
- modern WebAssembly-capable browsers: `CareNest.CrossPlatform.Browser`, `net10.0-browser`, Avalonia Browser;
- shared Avalonia application/views: `CareNest.CrossPlatform`.

Canonical Linux/browser setup and architecture document:

`docs/setup/CROSS_PLATFORM.md`

Configured build/presentation reach does not imply complete production feature parity. Linux/browser capability evidence is intentionally separate from the established MAUI behavior documentation.

## 3. Start here by audience

### End users and evaluators

- `README.md` — product overview, current platform reach, limitations and highlighted Gumroad storefront link.
- `docs/GETTING_STARTED.md` — first-use and first-build orientation.
- `docs/USER_GUIDE.md` — established application workflows.
- `docs/FEATURE_REFERENCE.md` — feature-by-feature established behavior.
- `docs/USER_FAQ.md` — common questions and limitations.
- `docs/KNOWN_LIMITATIONS.md` — intentionally unsupported or externally constrained behavior.
- `docs/REPORTS_AND_EXPORTS.md` — exports, reports and privacy boundaries.
- `docs/GLOSSARY.md` — terminology.
- `docs/setup/CROSS_PLATFORM.md` — Linux/browser host scope and capability boundaries.
- `GUMROAD.md` — official Ram Sandesh storefront guide and CareNest separation boundary.

### Contributors and developers

- `CONTRIBUTING.md` — contribution rules.
- `.github/PULL_REQUEST_TEMPLATE.md` — safety/privacy/migration/testing/release contribution checklist.
- `.github/CODEOWNERS` — default repository ownership.
- `docs/DEVELOPER_REFERENCE.md` — development conventions and current technical baseline.
- `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md` — package versions and maintained GitHub Actions majors.
- `docs/setup/DEVELOPMENT.md` — environment and build/test setup.
- `docs/setup/PLATFORM_SETUP.md` — established MAUI target-specific requirements.
- `docs/setup/CROSS_PLATFORM.md` — Linux/browser build/run/publish setup and host boundaries.
- `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md` — established Windows/Android/iOS/Mac Catalyst package guide.
- `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — final-package evidence generator and fail-closed production requirements.
- `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` — production evidence result and redaction rules.
- `docs/releases/PRODUCTION_EVIDENCE_INDEX.md` — platform evidence records including Linux/browser.
- `docs/testing/DOCUMENTATION_INTEGRITY.md` — stable active documentation local-link verification.
- `docs/setup/TROUBLESHOOTING.md` — build/runtime troubleshooting.
- `docs/CODEBASE_REFERENCE.md` — source/project/file map.
- `docs/CONFIGURATION_REFERENCE.md` — package/build/configuration reference.
- `docs/architecture/ARCHITECTURE.md` — solution architecture.
- `docs/architecture/SERVICE_BOUNDARIES.md` — dependency/service ownership.
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — storefront placement and runtime-package exclusion rules.

### Maintainers and release operators

- `docs/MAINTENANCE_AND_OPERATIONS.md` — maintenance and incident operations.
- `docs/setup/MAINTAINER_OPERATIONS.md` — setup/repository operations.
- `docs/REPOSITORY_GOVERNANCE.md` — documentation/source/evidence governance.
- `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md` — dependency/action baseline to compare with executable configuration.
- `docs/setup/CROSS_PLATFORM.md` — Linux/browser operator build boundary.
- `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md` — established executable/package build guide.
- `docs/releases/EXECUTABLE_BUILD_CHECKLIST.md` — concise operator checklist and package-evidence integration.
- `docs/releases/RELEASE_PROCESS.md` — production release process.
- `docs/releases/RELEASE_CHECKLIST.md` — release gate checklist.
- `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` — fail-closed evidence semantics.
- `docs/releases/PRODUCTION_EVIDENCE_INDEX.md` — canonical evidence workflow and templates.
- `docs/releases/templates/LINUX_DESKTOP_VALIDATION_RECORD.md` — canonical Linux validation record.
- `docs/releases/templates/BROWSER_VALIDATION_RECORD.md` — canonical browser/WebAssembly validation record.
- `docs/releases/RELEASE_EVIDENCE.md` — automated/manual/security/store/signing/package evidence contract.
- `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — deterministic final-package checksum/provenance JSON tooling.
- `docs/testing/DOCUMENTATION_INTEGRITY.md` — stable documentation integrity gate and dynamic evidence exception.
- `docs/releases/MANUAL_TEST_MATRIX.md` — manual platform validation.
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md` — packaged compatibility validation.
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md` — store-submission preparation and final package evidence.
- `docs/releases/AUTOMATED_BASELINE.md` — current accepted exact-source automated evidence authority.
- `docs/releases/NEXT_STEPS.md` — current remaining work.
- `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` — exact-head verification procedure.
- `docs/releases/BACKUP_RESOURCE_HARDENING_20260819.md` — accepted backup resource-hardening record.
- dated verification documents under `docs/releases/` — permanent evidence for their own exact source boundaries.

### Security/privacy reviewers

- `PRIVACY.md` — user-facing privacy statement.
- `SECURITY.md` — security policy/reporting.
- `.github/ISSUE_TEMPLATE/config.yml` — private vulnerability routing and safe public-support links.
- `docs/privacy/PRIVACY_MODEL.md` — privacy architecture.
- `docs/privacy/DATA_LIFECYCLE.md` — data lifecycle.
- `docs/security/SECURITY_MODEL.md` — technical security model.
- `docs/security/THREAT_MODEL.md` — threat model and residual risk.
- `docs/security/LOGGING_PRIVACY.md` — diagnostic/logging policy.
- `docs/security/DEPENDENCY_RISK_REGISTER.md` — dependency security status.
- `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md` — current dependency/action versions and upgrade policy.
- `docs/releases/SECURITY_RELEASE_REVIEW.md` — exact-candidate security review checklist.
- `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` — public-evidence data/secret exclusion rules.
- `docs/releases/templates/SIGNING_PROVENANCE_RECORD.md` — non-secret signing/provenance evidence record.
- `docs/releases/templates/BROWSER_VALIDATION_RECORD.md` — browser storage/network/privacy evidence boundary.
- `docs/releases/templates/LINUX_DESKTOP_VALIDATION_RECORD.md` — Linux filesystem/secure-storage/capability evidence boundary.
- `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — package provenance and no-secret evidence rules.
- `docs/releases/STORE_POLICY_REVIEW_20260818.md` — dated sensitive-health-data/store-policy review.
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — no-health-entitlement and no-runtime-storefront policy.

### QA and verification

- `docs/testing/TESTING_GUIDE.md` — testing strategy and commands.
- `docs/testing/TEST_PLAN.md` — test plan.
- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` — reminder correctness contract.
- `docs/testing/SETTINGS_LIFECYCLE_CONTRACT.md` — settings lifecycle contract.
- `docs/testing/DOCUMENTATION_INTEGRITY.md` — offline local-link checker behavior and exact-source boundary.
- `docs/PLATFORM_BEHAVIOR_MATRIX.md` — automated versus manual established-platform evidence.
- `docs/setup/CROSS_PLATFORM.md` — Linux/browser build and manual-evidence boundary.
- `docs/releases/EXECUTABLE_BUILD_CHECKLIST.md` — executable/package validation checklist.
- `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` — result-state/evidence-quality contract.
- `docs/releases/PRODUCTION_EVIDENCE_INDEX.md` — platform/cross-platform evidence templates.
- `docs/releases/templates/LINUX_DESKTOP_VALIDATION_RECORD.md` — Linux manual validation record.
- `docs/releases/templates/BROWSER_VALIDATION_RECORD.md` — browser manual validation record.
- `docs/releases/RELEASE_EVIDENCE.md` — exact release evidence requirements.
- `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` — current exact-head verification procedure.
- `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — package evidence self-test/production behavior.
- `docs/releases/AUTOMATED_BASELINE.md` — latest accepted actual workflow/test record.
- `tests/CareNest.UiTests/ProductionEvidenceDocumentationContractTests.cs` — production evidence documentation contracts.
- `tests/CareNest.UiTests/CrossPlatformEvidenceContractTests.cs` — Linux/browser evidence/parity contracts.
- `tests/CareNest.UiTests/FundingLinkContractTests.cs` — repository/storefront placement contracts.
- `tests/CareNest.UiTests/StoreFundingPayloadContractTests.cs` — store-payload external-commerce exclusion contracts.
- `tests/CareNest.UiTests/ReleaseDocumentationConsistencyContractTests.cs` — stable release-governance consistency contracts.
- `tests/CareNest.UiTests/PackageEvidenceToolContractTests.cs` — package-evidence tooling/source/workflow contracts.
- `tests/CareNest.UiTests/DocumentationIntegrityToolContractTests.cs` — documentation-integrity source/workflow contracts.
- `build/scripts/verify-cross-platform-targets.py` — fail-closed cross-platform configuration/evidence verifier.
- `build/scripts/test-verify-cross-platform-targets.py` — isolated cross-platform verifier regression tests.
- `build/scripts/test-create-package-evidence.py` — synthetic package-evidence behavior self-test.
- `build/scripts/test-verify-documentation-links.py` — synthetic documentation-link behavior self-test.
- dated verification documents under `docs/releases/` — exact workflow evidence.

### Design, accessibility and localization

- `docs/design/DESIGN_SYSTEM.md` — visual/component principles.
- `docs/design/ACCESSIBILITY.md` — accessibility requirements and manual evidence needs.
- `docs/design/LOCALIZATION.md` — localization/RTL strategy.
- `docs/design/STORE_ASSETS.md` — screenshots and store-asset guidance.
- `docs/releases/templates/ACCESSIBILITY_VALIDATION_RECORD.md` — production accessibility evidence record.
- `docs/assets/gumroad_store_badge.svg` — repository-only storefront badge with accessible title/description text.

## 4. Architecture set

The architecture documentation is intentionally layered:

- `docs/architecture/ARCHITECTURE.md` — system overview and dependency direction.
- `docs/architecture/APPLICATION_FLOWS.md` — runtime/user flows.
- `docs/architecture/SERVICE_BOUNDARIES.md` — project/service ownership.
- `docs/architecture/DATABASE_SCHEMA.md` — schema, migrations and indexes.
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md` — storage/export/delete boundaries.
- `docs/architecture/DOCUMENT_VAULT.md` — encrypted imported-document storage.
- `docs/architecture/BACKUP_AND_RESTORE.md` — backup/restore architecture.
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md` — reminder/notification integration.
- `docs/architecture/ADR-0001-local-first.md` — local-first decision.
- `docs/architecture/ADR-0002-reminder-occurrences.md` — reminder-occurrence decision.
- `docs/architecture/ADR-0003-encrypted-backup-format.md` — backup format decision.
- `docs/setup/CROSS_PLATFORM.md` — current presentation-host architecture and capability boundaries.

Platform-neutral business logic must remain outside presentation hosts. Domain/application projects must not acquire MAUI, Avalonia, browser or OS-specific UI dependencies.

## 5. Executable, package, production-evidence and provenance ownership

`docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md` owns established MAUI executable/package creation and documents Windows, Android, iOS and Mac Catalyst package paths.

`docs/setup/CROSS_PLATFORM.md` owns Linux desktop and WebAssembly/browser build/run/publish instructions and the distinction between configured build support and production feature parity.

`docs/releases/EXECUTABLE_BUILD_CHECKLIST.md` is the concise established-package operator companion.

`docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` owns evidence quality, result states, data/secret exclusions, failure handling and promotion semantics.

`docs/releases/PRODUCTION_EVIDENCE_INDEX.md` owns the release-specific evidence directory workflow and canonical templates for platform, accessibility, compatibility, signing, store and final approval evidence.

Linux/browser platform records are:

- `docs/releases/templates/LINUX_DESKTOP_VALIDATION_RECORD.md`;
- `docs/releases/templates/BROWSER_VALIDATION_RECORD.md`.

Both canonical files must remain visibly unperformed (`NOT RUN`). They do not prove Linux/browser parity simply by existing.

`docs/releases/RELEASE_EVIDENCE.md` owns the combined automated/manual/security/store/signing/package release evidence contract.

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md` owns structured final-package evidence procedures, including exact source/tag/HEAD requirements, clean tracked workspace, no-secret provenance, external-commerce payload scanning, deterministic hashing and atomic JSON evidence output.

Repository-only Gumroad promotional files are documentation inputs, not CareNest application package inputs. Store-safe payload scanning rejects `ramsandesh.gumroad.com` and `buymeacoffee.com/sanskarIN` if either appears in built application output.

## 6. Current source facts that documentation must preserve

### Application identity

- title: `CareNest`;
- application ID: `com.sanskar.carenest`;
- display version: `1.0.0-rc.1`;
- build/application version: `1`.

### Established MAUI targets

- `net10.0-android`, minimum Android API 24;
- `net10.0-ios`, minimum iOS 15;
- `net10.0-maccatalyst`, minimum Mac Catalyst 15;
- `net10.0-windows10.0.19041.0`, minimum Windows target 10.0.19041.0.

### Cross-platform targets

- `CareNest.CrossPlatform` shared Avalonia application targeting `net10.0`;
- `CareNest.CrossPlatform.Desktop` Avalonia Desktop host targeting `net10.0`;
- `CareNest.CrossPlatform.Browser` Avalonia Browser WebAssembly host targeting `net10.0-browser`.

### MAUI XAML compiler policy

- `MauiEnableXamlCBindingWithSourceCompilation=true`;
- `MauiStrictXamlCompilation=true`;
- `XC0022`, `XC0023`, `XC0024`, `XC0025` are warnings-as-errors;
- no matching warning suppression is part of the intended baseline.

### Cross-platform verifier policy

`build/scripts/verify-cross-platform-targets.py` checks required target/package/host/solution/CI/dependency/release/evidence wiring and XML-parses required Avalonia XAML.

`build/scripts/test-verify-cross-platform-targets.py` must remain an isolated fail-closed self-test rather than mutating the live checkout.

### Dependency baseline

Current central package versions are maintained in `Directory.Packages.props`; `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md` is the human-readable baseline. Executable source remains authoritative when prose drifts.

## 7. Automated evidence authority

Current mutable automated evidence authority:

`docs/releases/AUTOMATED_BASELINE.md`

It records only workflow/test results actually observed for the exact source named there.

Stable documents must not pin moving accepted SHA/test totals as if they automatically apply to newer source. Use the dynamic baseline file for the current accepted source and result inventory.

Permanent historical verification records remain authoritative only for the exact source named in each record. Historical counts do not transfer automatically to newer heads.

## 8. Current continuation verification boundary

Current verification-relevant continuation:

PR `#84` — `continue/cross-platform-current-main-20260823`

It rebuilds the useful Linux desktop/WebAssembly host work directly on the current `main` that already contains the merged production-evidence governance work, instead of relying on the stale/diverged base of PR #83.

PR #84 changes project/workflow/test/stable documentation source, so its final exact head must complete a fresh required matrix before merge or promotion.

Required branch-level evidence includes, as configured:

- CareNest CI, including Linux desktop and browser jobs;
- unit/integration/UI/source-policy tests;
- Android/Windows/iOS simulator/Mac Catalyst builds;
- unsuppressed Dependency Audit including Avalonia desktop/browser graphs;
- CodeQL;
- Store Package Configuration;
- Store Inspection Artifacts.

Queued, superseded, cancelled, skipped, failed or older-head runs are not final current-head success evidence.

A successful Linux build/browser publish still does not prove manual production parity.

## 9. Current production evidence workflow

Canonical standard:

`docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`

Canonical index:

`docs/releases/PRODUCTION_EVIDENCE_INDEX.md`

Canonical templates under `docs/releases/templates/` cover:

- Android device validation;
- Windows validation;
- iOS/iPadOS device validation;
- Mac Catalyst validation;
- Linux desktop validation;
- Browser/WebAssembly validation;
- accessibility validation;
- packaged compatibility validation;
- signing provenance;
- store submission/review/publication;
- final production release approval.

Canonical templates intentionally remain `NOT RUN` and unperformed. They are copied into release-specific evidence directories and populated only with actual results.

A build, simulator compile, Linux build or WebAssembly publish is automated source evidence only, not a manual production result.

## 10. Current store-policy evidence

Pre-submission review date: `2026-08-18`.

Record:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

That record compares the relevant product/package boundary with official Apple, Google Play and Microsoft Store policy areas. It is evidence of a dated review, **not** store approval and **not** a substitute for live submission-day policy/store-console review.

Linux/browser distribution/hosting channels require their own applicable current evidence rather than inheriting Apple/Google/Microsoft store approval assumptions.

Use `docs/releases/templates/STORE_SUBMISSION_RECORD.md` for final submission-day evidence and state transitions where a store is applicable.

## 11. Gumroad storefront documentation ownership

Canonical storefront URL:

`https://ramsandesh.gumroad.com`

Authority map:

- `GUMROAD.md` — canonical reader-facing storefront guide and separation statement;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — placement/package policy;
- `docs/assets/gumroad_store_badge.svg` — repository-only promotional graphic;
- `README.md`, `SUPPORT.md`, `.github/FUNDING.yml` — highlighted repository entry points;
- dated rollout evidence under `docs/releases/` — exact rollout automated evidence;
- `tests/CareNest.UiTests/FundingLinkContractTests.cs` — repository-placement regression contract;
- `tests/CareNest.UiTests/StoreFundingPayloadContractTests.cs` — application/package exclusion regression contract;
- `build/scripts/verify-store-safe-payload.py` — byte-level package scanner.

The Gumroad storefront is not a CareNest health entitlement and must not be represented as medical advice, diagnosis, treatment, dosage guidance, reminder reliability, emergency service or health-data access.

Cross-platform hosts must preserve the same runtime-package separation boundary.

## 12. Documentation-integrity ownership

`docs/testing/DOCUMENTATION_INTEGRITY.md` owns the offline local-link integrity design.

Source-controlled tooling:

- `build/scripts/verify-documentation-links.py`;
- `build/scripts/test-verify-documentation-links.py`;
- `tests/CareNest.UiTests/DocumentationIntegrityToolContractTests.cs`.

Cross-platform source/evidence integrity additionally uses:

- `build/scripts/verify-cross-platform-targets.py`;
- `build/scripts/test-verify-cross-platform-targets.py`;
- `tests/CareNest.UiTests/CrossPlatformEvidenceContractTests.cs`;
- `tests/CareNest.UiTests/ProductionEvidenceDocumentationContractTests.cs`;
- `tests/CareNest.UiTests/ReleaseDocumentationConsistencyContractTests.cs`.

The default documentation check validates stable active Markdown while excluding `docs/history/` and explicitly dynamic post-verification evidence/status files. `--include-history` and `--include-dynamic` provide explicit wider audits without changing the stable exact-source evidence boundary.

## 13. Historical documentation

`docs/history/` contains exact snapshots of previously active handoff/status/catalog/documentation files. Dated release documents intentionally retain older verification boundaries.

Do not rewrite historical evidence merely to make old files look current or to retroactively insert newer platforms/test counts.

Current 2026-08-23 cross-platform continuation archives include:

- the previous active `what_changed.md`;
- the previous active `PROJECT_STATUS.md`;
- the previous active `docs/DOCUMENTATION_CATALOG.md`.

## 14. Production status boundary

Documentation/tooling/source completeness does not mean production release completeness.

CareNest remains `1.0.0-rc.1` until applicable release evidence is real. Remaining external/manual gates include, as applicable:

- exact-head automated verification for the current candidate;
- Android/Windows/iOS/Mac Catalyst real package/device behavior;
- Linux desktop runtime/manual validation;
- browser/WebAssembly runtime/manual validation across actually tested browsers;
- accessibility validation;
- packaged existing-data/encrypted-data compatibility;
- production signing/notarization;
- structured final-package evidence;
- final signed-package external-commerce inspection;
- live store-console declarations/metadata where applicable;
- submission-day policy re-check;
- exact approved production source/tag;
- tagged release gates;
- store submission/approval/publication evidence where applicable.

Use `docs/releases/AUTOMATED_BASELINE.md` for the current accepted automated source boundary, `docs/releases/NEXT_STEPS.md` as the operational checklist, `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` for evidence semantics, `docs/releases/PRODUCTION_EVIDENCE_INDEX.md` for release-specific evidence records and `docs/setup/CROSS_PLATFORM.md` for Linux/browser capability boundaries.
