# CareNest Documentation Catalog

**Release line:** `1.0.0-rc.1`  
**Documentation baseline date:** 2026-08-19  
**Current automated baseline record:** `docs/releases/AUTOMATED_BASELINE.md`  
**Production evidence standard:** `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`  
**Production evidence index:** `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`  
**Current dependency/toolchain baseline:** `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`  
**Documentation integrity guide:** `docs/testing/DOCUMENTATION_INTEGRITY.md`

This catalog is the navigation and ownership map for the complete CareNest documentation set. It does not replace the detailed documents; it tells readers which document is authoritative for each question.

## 1. Documentation precedence

When documents appear to disagree, use this order:

1. `PROJECT_STATUS.md` for current release state, verification boundary and blockers.
2. `docs/releases/AUTOMATED_BASELINE.md` for the latest actually observed exact-source automated verification record.
3. `docs/releases/NEXT_STEPS.md` for remaining production work and the immediate exact-head verification gate.
4. `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` for production evidence quality and result-state semantics.
5. `docs/releases/PRODUCTION_EVIDENCE_INDEX.md` for release-specific production evidence workflow and canonical templates.
6. latest dated verification record under `docs/releases/` for permanent exact automated evidence.
7. `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` for exact-head verification rules when current source or verification-relevant documentation has moved beyond the verified baseline.
8. `docs/releases/RELEASE_EVIDENCE.md` for the full automated/manual/security/store/signing/package evidence contract.
9. `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` for structured final-package checksum/provenance evidence.
10. `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md` for the current human-readable package/GitHub Actions source baseline.
11. `docs/testing/DOCUMENTATION_INTEGRITY.md` for stable/dynamic/historical documentation-link verification boundaries.
12. `docs/releases/STORE_POLICY_REVIEW_20260818.md` for the dated pre-submission store-policy review; official live store policies remain authoritative at actual submission time.
13. `docs/COMPLETE_PROJECT_DOCUMENTATION.md` for the end-to-end product/engineering reference.
14. specialized architecture/security/testing/setup documents for implementation details.
15. `GUMROAD.md` and `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` for the current external storefront placement/package boundary.
16. `what_changed.md` and `docs/history/` for chronological and historical context.

Historical evidence is intentionally retained and may describe older source boundaries. A dated historical file must not be interpreted as the current product state merely because it contains a detailed test count or source SHA.

## 2. Start here by audience

### End users and evaluators

- `README.md` — product overview, scope, limitations, current release status and highlighted Gumroad storefront link.
- `docs/GETTING_STARTED.md` — first-use and first-build orientation.
- `docs/USER_GUIDE.md` — complete use workflows.
- `docs/FEATURE_REFERENCE.md` — feature-by-feature behavior.
- `docs/USER_FAQ.md` — common questions and limitations.
- `docs/KNOWN_LIMITATIONS.md` — intentionally unsupported or externally constrained behavior.
- `docs/REPORTS_AND_EXPORTS.md` — exports, reports and privacy boundaries.
- `docs/GLOSSARY.md` — terminology.
- `GUMROAD.md` — official Ram Sandesh storefront guide and CareNest separation boundary.

### Contributors and developers

- `CONTRIBUTING.md` — contribution rules.
- `.github/PULL_REQUEST_TEMPLATE.md` — safety/privacy/migration/testing/release contribution checklist.
- `.github/CODEOWNERS` — default repository ownership.
- `docs/DEVELOPER_REFERENCE.md` — development conventions and current technical baseline.
- `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md` — package versions and maintained GitHub Actions majors.
- `docs/setup/DEVELOPMENT.md` — environment and build/test setup.
- `docs/setup/PLATFORM_SETUP.md` — target-specific requirements.
- `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md` — authoritative executable/package creation guide for Windows, Android, iOS and Mac Catalyst.
- `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — final-package evidence generator and fail-closed production requirements.
- `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` — production evidence result and redaction rules.
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
- `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md` — exact current dependency/action baseline to compare with executable configuration.
- `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md` — complete executable/package build guide.
- `docs/releases/EXECUTABLE_BUILD_CHECKLIST.md` — concise operator checklist and package-evidence integration.
- `docs/releases/RELEASE_PROCESS.md` — production release process.
- `docs/releases/RELEASE_CHECKLIST.md` — release gate checklist.
- `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` — fail-closed evidence semantics.
- `docs/releases/PRODUCTION_EVIDENCE_INDEX.md` — canonical evidence workflow and templates.
- `docs/releases/RELEASE_EVIDENCE.md` — automated/manual/security/store/signing/package evidence contract.
- `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — deterministic final-package checksum/provenance JSON tooling.
- `docs/testing/DOCUMENTATION_INTEGRITY.md` — stable documentation integrity gate and dynamic evidence exception.
- `docs/releases/MANUAL_TEST_MATRIX.md` — manual platform validation.
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md` — packaged compatibility validation.
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md` — exact store-submission preparation and final package evidence.
- `docs/releases/AUTOMATED_BASELINE.md` — current exact-source automated evidence authority.
- `docs/releases/NEXT_STEPS.md` — current remaining work.
- `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` — exact-head verification procedure.
- `docs/releases/BACKUP_RESOURCE_HARDENING_20260819.md` — current accepted backup resource-hardening record.
- `docs/releases/FINAL_CANDIDATE_VERIFICATION_20260818.md` — previous accepted exact-head evidence record.
- `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md` — permanent Gumroad rollout automated evidence.
- `docs/releases/STORE_POLICY_REVIEW_20260818.md` — dated pre-submission Apple/Google/Microsoft policy review.
- `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md` — permanent compiled-binding evidence.
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — external-commerce package-boundary rules.

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
- `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — package provenance and no-secret evidence rules.
- `docs/releases/STORE_POLICY_REVIEW_20260818.md` — current dated sensitive-health-data/store-policy review.
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — no-health-entitlement and no-runtime-storefront policy.

### QA and verification

- `docs/testing/TESTING_GUIDE.md` — testing strategy and commands.
- `docs/testing/TEST_PLAN.md` — test plan.
- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` — reminder correctness contract.
- `docs/testing/SETTINGS_LIFECYCLE_CONTRACT.md` — settings lifecycle contract.
- `docs/testing/DOCUMENTATION_INTEGRITY.md` — offline local-link checker behavior and exact-source boundary.
- `docs/PLATFORM_BEHAVIOR_MATRIX.md` — automated versus manual platform evidence.
- `docs/releases/EXECUTABLE_BUILD_CHECKLIST.md` — executable/package validation checklist.
- `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` — result-state/evidence-quality contract.
- `docs/releases/PRODUCTION_EVIDENCE_INDEX.md` — platform/cross-platform evidence templates.
- `docs/releases/RELEASE_EVIDENCE.md` — exact release evidence requirements.
- `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` — current exact-head verification procedure.
- `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — package evidence self-test/production behavior.
- `docs/releases/AUTOMATED_BASELINE.md` — latest actual workflow run/test-count record.
- `docs/releases/STORE_POLICY_REVIEW_20260818.md` — dated policy review evidence and submission-day recheck boundary.
- `tests/CareNest.UiTests/ProductionEvidenceDocumentationContractTests.cs` — production evidence documentation contracts.
- `tests/CareNest.UiTests/FundingLinkContractTests.cs` — repository/storefront placement contracts.
- `tests/CareNest.UiTests/StoreFundingPayloadContractTests.cs` — store-payload external-commerce exclusion contracts.
- `tests/CareNest.UiTests/ReleaseDocumentationConsistencyContractTests.cs` — stable release-governance consistency contracts.
- `tests/CareNest.UiTests/PackageEvidenceToolContractTests.cs` — package-evidence tooling/source/workflow contracts.
- `tests/CareNest.UiTests/DocumentationIntegrityToolContractTests.cs` — documentation-integrity source/workflow contracts.
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

## 3. Architecture set

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

## 4. Executable, package, production-evidence and provenance ownership

`docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md` owns end-to-end executable/package creation and documents:

- repository/build inputs;
- the MAUI application project;
- Windows executable publication;
- Android APK/AAB publication;
- iOS IPA publication;
- Mac Catalyst `.app`/`.pkg` publication;
- signing boundaries;
- restore/quality/release preflight;
- output locations and troubleshooting.

`docs/releases/EXECUTABLE_BUILD_CHECKLIST.md` is the concise operator companion.

`docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` owns evidence quality, result states, data/secret exclusions, failure handling and promotion semantics.

`docs/releases/PRODUCTION_EVIDENCE_INDEX.md` owns the release-specific evidence directory workflow and canonical templates for platform, accessibility, compatibility, signing, store and final approval evidence.

`docs/releases/RELEASE_EVIDENCE.md` owns the combined automated/manual/security/store/signing/package release evidence contract.

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md` owns the structured final-package evidence procedure. It documents:

- `create-package-evidence.py`;
- Bash and PowerShell wrappers;
- inspection versus production modes;
- exact source/tag/HEAD requirements;
- clean tracked workspace requirement;
- no-secret signing-provenance rule;
- store-safe payload scanning;
- per-file SHA-256;
- deterministic directory payload hashing;
- atomic JSON evidence output;
- synthetic self-test behavior;
- evidence limitations.

Repository-only Gumroad promotional files are documentation inputs, not CareNest application package inputs. Store-safe payload scanning rejects `ramsandesh.gumroad.com` and `buymeacoffee.com/sanskarIN` if either appears in built application output.

## 5. Current source facts that documentation must preserve

The application project targets:

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

Current central package versions are maintained in `Directory.Packages.props` and documented in `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`. The current candidate source includes:

- Microsoft.Maui.Controls `10.0.90`;
- sqlite-net-pcl `1.9.172`;
- SQLitePCLRaw.bundle_green `2.1.11`;
- SQLitePCLRaw.lib.e_sqlite3 `3.53.3`;
- SQLitePCLRaw Android/provider leaves `2.1.12` where pinned;
- xUnit `2.9.3`;
- Microsoft.NET.Test.Sdk `18.9.0`;
- xunit.runner.visualstudio `4.0.0`;
- coverlet.collector `10.0.1`.

Current maintained workflow majors include checkout v7, setup-dotnet v6, CodeQL v4 and upload-artifact v7 where used.

## 6. Automated evidence authority

Current mutable automated evidence authority:

`docs/releases/AUTOMATED_BASELINE.md`

It must record only workflow/test results actually observed for the exact source named there.

The latest accepted exact automated source before the current production-evidence-readiness continuation is:

`30ee6c265104c64ec5a1a4013f592f7f058750e8`

Accepted recorded result:

- unit tests: **122/122**;
- integration tests: **54/54**;
- UI/source-policy tests: **194/194**;
- total core tests: **370/370**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- Store Package Configuration: success;
- Store Inspection Artifacts: success;
- CodeQL: success;
- unsuppressed Dependency Audit: success.

Current recorded workflow IDs and merge/source boundaries belong in `docs/releases/AUTOMATED_BASELINE.md`, `PROJECT_STATUS.md`, `docs/releases/NEXT_STEPS.md`, and the accepted dated verification/hardening records.

Permanent historical verification records include older boundaries such as:

- `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md` — verified `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f` with 336/336 core tests for that historical source;
- `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md` — permanent strict XAML compiled-binding evidence for its own exact source.

Historical counts do not transfer automatically to newer heads.

## 7. Current continuation verification boundary

The accepted runtime/source behavior is already complete for the intended RC1 scope. The current production-evidence-readiness continuation changes release-governance documentation and source-policy tests by adding the production evidence standard/index/templates and aligning active release authorities.

Because those changes are verification-relevant:

- the accepted automated source remains `30ee6c265104c64ec5a1a4013f592f7f058750e8` until the final continuation head completes its required fresh exact-head/pull-request matrix;
- previous 370/370 results must not be copied forward as if generated by a newer head;
- failed checks must be corrected rather than suppressed or described as successful;
- `docs/releases/NEXT_STEPS.md`, `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`, and `docs/releases/AUTOMATED_BASELINE.md` own the verification/evidence procedure.

This verification requirement does not mean the accepted 370-test baseline is obsolete; it prevents a newer unverified documentation/test head from falsely inheriting it.

## 8. Current production evidence workflow

Canonical standard:

`docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`

Canonical index:

`docs/releases/PRODUCTION_EVIDENCE_INDEX.md`

Canonical templates under `docs/releases/templates/` cover:

- Android device validation;
- Windows validation;
- iOS/iPadOS device validation;
- Mac Catalyst validation;
- accessibility validation;
- packaged compatibility validation;
- signing provenance;
- store submission/review/publication;
- final production release approval.

Canonical templates intentionally remain `NOT RUN` and unperformed. They are copied into release-specific evidence directories and populated only with actual results.

## 9. Current store-policy evidence

Pre-submission review date: `2026-08-18`.

Record:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

That record compares the current CareNest product/package boundary with official Apple, Google Play and Microsoft Store policy areas relevant to health functionality, sensitive-data privacy, application completeness and external-commerce placement.

It is evidence of a dated policy review, **not** store approval and **not** a substitute for reviewing official policies and live store-console forms against the exact production package/listing on the actual submission date.

Current release decision retained by that review:

- Gumroad remains repository/documentation-only;
- Buy Me a Coffee remains repository/documentation-only;
- neither external destination is a CareNest health entitlement;
- neither external destination belongs in the submitted CareNest application package under the current RC1 policy.

Use `docs/releases/templates/STORE_SUBMISSION_RECORD.md` for the final submission-day evidence and state transitions.

## 10. Gumroad storefront documentation ownership

Canonical storefront URL:

`https://ramsandesh.gumroad.com`

Authority map:

- `GUMROAD.md` — canonical reader-facing storefront guide and separation statement;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — placement/package policy;
- `docs/assets/gumroad_store_badge.svg` — repository-only promotional graphic;
- `README.md`, `SUPPORT.md`, `.github/FUNDING.yml` — highlighted repository entry points;
- `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md` — exact rollout automated evidence;
- `tests/CareNest.UiTests/FundingLinkContractTests.cs` — repository-placement regression contract;
- `tests/CareNest.UiTests/StoreFundingPayloadContractTests.cs` — application/package exclusion regression contract;
- `build/scripts/verify-store-safe-payload.py` — byte-level package scanner.

The Gumroad storefront is not a CareNest health entitlement and must not be represented as medical advice, diagnosis, treatment, dosage guidance, reminder reliability, emergency service, or health-data access.

## 11. Documentation-integrity ownership

`docs/testing/DOCUMENTATION_INTEGRITY.md` owns the offline local-link integrity design.

Source-controlled tooling:

- `build/scripts/verify-documentation-links.py`;
- `build/scripts/test-verify-documentation-links.py`;
- `tests/CareNest.UiTests/DocumentationIntegrityToolContractTests.cs`.

The default exact-source check validates stable active Markdown while excluding `docs/history/` and the explicitly dynamic post-verification evidence/status files. `--include-history` and `--include-dynamic` provide explicit wider audits without changing the stable exact-source evidence boundary.

## 12. Historical documentation

`docs/history/` contains exact snapshots of previously active handoff/status/documentation files. Dated release documents intentionally retain older verification boundaries. Do not rewrite historical evidence merely to make old files look current or to retroactively insert Gumroad promotion/newer test counts.

## 13. Production status boundary

Documentation/tooling/source completeness does not mean production release completeness.

CareNest remains `1.0.0-rc.1` until the applicable release evidence is real. Remaining external/manual gates include, as applicable:

- real-device notification/lifecycle behavior;
- accessibility validation;
- packaged existing-data/encrypted-data compatibility;
- production signing/notarization;
- structured final-package evidence;
- final signed-package external-commerce inspection;
- live store-console declarations/metadata;
- submission-day policy re-check;
- exact approved production source/tag;
- tagged release gates;
- store submission/approval/publication evidence.

Use `docs/releases/AUTOMATED_BASELINE.md` for the current automated source boundary, `docs/releases/NEXT_STEPS.md` as the operational checklist, `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` for evidence semantics, and `docs/releases/PRODUCTION_EVIDENCE_INDEX.md` for release-specific evidence records.
