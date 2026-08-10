# Changelog

All notable changes follow Keep a Changelog principles and semantic versioning.

## [Unreleased] - 2026-08-10

### Added

- Platform-neutral `dotnet format --verify-no-changes` verification in the core CI job.
- Repository policy tests covering implementation placeholders, local-first network/telemetry boundaries, clinical-decision feature-name regressions, common signing/secret files, and required governance/release artifacts.
- Architecture dependency tests enforcing Shared → Domain → Application → Infrastructure boundaries while keeping MAUI isolated to the application composition project.
- ViewModel contract tests preventing direct SQLite/network-client access and protecting onboarding notification-permission and as-needed reminder rules.
- Data-model contract tests for every entity required by the CareNest master prompt and for opaque medicine strength/instruction storage.
- Branding/localization contract tests for app icon, splash, BMC support artwork, safety resource keys, and clickable support surfaces.
- Original monochrome, light-surface, and dark-surface CareNest mark variants.
- Privacy-aware global unhandled/unobserved-task exception observation registered once during application startup.
- `docs/security/LOGGING_PRIVACY.md` describing the allowed/prohibited runtime diagnostic boundary.
- `CareNest Release Evidence` workflow that records exact source/ref/toolchain identity, TRX results, transitive dependency inventories, SHA-256 evidence checksums, and an immutable Actions artifact for a manual/tag-triggered release candidate.
- `docs/releases/RELEASE_EVIDENCE.md`, `QUALITY_GATE.md`, `SECURITY_RELEASE_REVIEW.md`, `RELEASE_NOTES_TEMPLATE.md`, and `VERIFICATION_BRANCH_PROTOCOL.md`.
- Automated async-safety contract coverage preventing common synchronous task-blocking patterns in runtime source.

### Changed

- Restored the previously verified Dependency Audit and production Release Gate workflows after a repository-history recovery audit identified files that had not been replayed.
- Restored the privacy-safe structured bug report form and all previously verified highlighted Buy Me a Coffee README/SUPPORT/About assets and release guidance.
- Expanded required-repository policy checks to include CI, CodeQL, Dependency Audit, Release Gate, Release Evidence, logging privacy, dependency risk, and security/quality review files.
- Repository/logging source-policy scans now ignore generated `bin`/`obj` content so checks apply to committed source instead of generated SDK files.
- Architecture project-reference parsing is separator-normalized for Linux/Windows compatibility and explicitly non-null under nullable analysis.

### Fixed

- Removed full exception-object logging from `SafeUiErrorService`, `StartupCoordinator`, and reminder scheduling/recovery paths.
- Global, UI, startup, and reminder error logging now uses explicit `ILogger.IsEnabled(...)` guards and logs only safe metadata such as exception type names instead of exception messages, stack traces, health-record identifiers, or user-entered health content.
- Corrected CA1873 eager logger-argument evaluation findings surfaced by GitHub-hosted MAUI builds.
- Corrected CA1861 constant-array allocation guidance in architecture tests.
- Corrected cross-platform project-reference path parsing and a nullable filename return caught by exact-head CI.
- Corrected policy-test false positives caused by generated `obj` global-using files.

### Verification

Exact production source head verified: `8417513db36c72b0ec2cfaccadb6ac47ba361f11`.

Final verification PR #27 used marker head `aefd53869b7eaf54815de446fc83373c7977d04d` and was closed without merge after success.

- CareNest CI #200 / `31375336226`: success.
- Platform-neutral formatting: success.
- Unit tests: 15 passed, 0 failed, 0 skipped.
- Integration tests: 11 passed, 0 failed, 0 skipped.
- UI-contract/policy tests: 46 passed, 0 failed, 0 skipped.
- Total core automated tests: 72 passed, 0 failed, 0 skipped.
- Android Release: success.
- Windows Release: success.
- iOS simulator Release: success.
- Mac Catalyst Release: success.
- CodeQL #200 / `31375336083`: success.
- Dependency Audit #7 / `31375336088`: success.

The final green pass followed superseded verification PRs #24–#26 that exposed and drove fixes for analyzer, privacy-logging, path-normalization, generated-source scanning, and nullable-contract problems instead of weakening those gates.

### Security

- `GHSA-2m69-gcr7-jv3q` remains explicitly open for the SQLitePCLRaw `2.1.11` dependency path. This continuation does not claim it fixed.
- The exact advisory suppression remains narrowly scoped and is still governed by `docs/security/DEPENDENCY_RISK_REGISTER.md` and `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.
- Public production promotion remains blocked on an explicit dependency-risk decision/resolution plus manual device/accessibility/store/signing work.

## [1.0.0-rc.1] - 2026-08-09

### Added

- Complete local-first CareNest application architecture and source tree.
- Profiles, medicine records, schedules, reminder occurrences and medication log.
- Appointments, encrypted document organization, stock/refill tracking and reports.
- Manual password-encrypted backup/restore (format v2), per-profile structured export/delete workflows, and portable encrypted-document recovery.
- App lock, notification diagnostics, accessibility/theme settings and developer tools.
- Android, iOS, Mac Catalyst and Windows platform integration structure.
- Security, privacy, threat-model, setup, release and contribution documentation.
- Automated unit/integration/contract test projects and GitHub Actions workflow.
- Dependency risk register for security advisories that cannot yet be resolved by an available compatible package release.
- Voluntary project-support link for `https://buymeacoffee.com/sanskarIN` in the About page, README/support documentation, and GitHub funding metadata.
- `docs/releases/NEXT_STEPS.md` with production-release blockers, manual device checks, signing/store work, release promotion tasks, and future-version ideas.
- UI-contract coverage that verifies the funding URL and voluntary-support wording stay consistent across runtime and repository support surfaces.

### Fixed

- Corrected SQLite result-producing PRAGMA handling for WAL journal mode, busy timeout, and WAL checkpoint operations so sqlite-net no longer treats returned rows as non-query failures.
- Added regression coverage for WAL mode, busy-timeout configuration, and WAL-backed database snapshot creation used by encrypted backups.
- Corrected MAUI CI target selection so a platform-specific target no longer leaks into referenced `net10.0` projects.
- Added an explicit MAUI Controls package reference and isolated platform source trees to their corresponding target frameworks.
- Corrected startup-page switch typing, nullable schedule-editor values, and redacted reminder-diagnostic timestamp usage.
- Corrected Android time-zone-change intent handling.
- Hardened Android notification scheduling for nullable platform values, pending-intent construction, notification construction, API-level guards, and notification-manager access.
- Updated Apple CI to use a runner/toolchain compatible with the installed .NET 10 iOS and Mac Catalyst workloads.
- Reclassified non-correctness analyzer recommendations so they remain visible without hiding functional build/test failures.
- Centralized repository, creator, funding, and contact values through shared CareNest constants in the About view model.

### Security

- Added a narrowly scoped NuGet audit exception for `GHSA-2m69-gcr7-jv3q` because the current `sqlite-net-pcl` dependency chain resolves SQLitePCLRaw `2.1.11` and the attempted `2.1.12` bundle version is not available on NuGet.org.
- The SQLite advisory remains open and explicitly tracked in `docs/security/DEPENDENCY_RISK_REGISTER.md`; the audit exception is not represented as a vulnerability fix.
- No wildcard or severity-wide NuGet audit suppression was added.
- The external voluntary-support action opens only after explicit user interaction and is documented as an independent third-party service; CareNest does not automatically send health records, documents, backups, or profile data to the funding provider.

### Safety

- Added explicit non-diagnostic/non-treatment medical boundaries throughout onboarding, About, reports and documentation.
- Reminder and stock limitations are surfaced instead of silently inferred.
- Project support is explicitly separated from medical advice, health functionality, reminder behavior, emergency assistance, support priority, and access to local CareNest data.
