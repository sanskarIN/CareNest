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
- Expanded medicine-schedule validation tests for explicit intervals/start times, selected weekdays, cycle on/off values, date ordering, clock ranges, recognized schedule enum values, supported weekday-mask bits, and blank/unknown/trimmed time-zone identifiers.
- Expanded reminder-planner tests for cycle schedules, custom/schedule/medicine end boundaries, archived profiles, paused/completed/archived medicines, and daylight-saving gaps.
- Reminder planning boundary tests for half-open UTC windows, UTC-kind validation, duplicate-time deduplication, stable occurrence identity, and chronological ordering.
- Reminder entity-ownership tests covering profile → medicine → schedule → persisted schedule-time relationships plus intentionally unbound editor times.
- Reminder coordinator contract tests protecting UTC rebuild overrides and explicit future-UTC snooze validation.
- Deterministic fixed-seed property-style recurrence tests covering arbitrary half-open windows, cycle on/off matrices, all supported weekday masks, stable uniqueness/order, and representative every-N-hours intervals.
- Representative multi-zone DST gap/overlap coverage for North America, Europe, and Australia when those time-zone identifiers are available on the test host.
- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` documenting deterministic, non-clinical schedule materialization, ownership, UTC, snooze, state, and DST rules.
- WAL snapshot integration coverage that opens the copied database read-only, verifies committed profile content, and executes `PRAGMA integrity_check`.
- Snapshot cancellation regression coverage ensuring a pre-cancelled request creates no output file.
- App-lock security contract coverage for salted PBKDF2-HMAC-SHA256 verification, fixed-time comparison, verifier-buffer clearing, plaintext-PIN non-persistence, stored lock-material removal, and numeric PIN policy.

### Changed

- Restored the previously verified Dependency Audit and production Release Gate workflows after a repository-history recovery audit identified files that had not been replayed.
- Restored the privacy-safe structured bug report form and all previously verified highlighted Buy Me a Coffee README/SUPPORT/About assets and release guidance.
- Expanded required-repository policy checks to include CI, CodeQL, Dependency Audit, Release Gate, Release Evidence, logging privacy, dependency risk, and security/quality review files.
- Repository/logging source-policy scans now ignore generated `bin`/`obj` content so checks apply to committed source instead of generated SDK files.
- Architecture project-reference parsing is separator-normalized for Linux/Windows compatibility and explicitly non-null under nullable analysis.
- App-lock verification now clears both the newly derived verifier and the verifier retrieved from secure storage after a PIN comparison; malformed/missing-salt paths also clear a retrieved verifier before returning.
- Threat-model/security documentation now explicitly treats app lock as a local privacy barrier rather than whole-database/device encryption and records weak-PIN/device-compromise residual risk.
- Reminder planning now rejects inconsistent local entity ownership instead of materializing an occurrence under a mismatched profile, medicine, schedule, or persisted schedule-time relationship.
- Planner windows and coordinator rebuild overrides now require actual UTC `DateTime` values rather than silently reinterpreting local/unspecified clock ticks.
- Snooze actions now require an explicit future UTC timestamp before occurrence persistence or platform notification scheduling.
- Archived profiles are suppressed defensively in the planner as well as in the coordinator's existing archive filter.
- Test-plan, security-review, quality-gate, release-checklist, and roadmap documentation now record the expanded reminder integrity invariants.

### Fixed

- Removed full exception-object logging from `SafeUiErrorService`, `StartupCoordinator`, and reminder scheduling/recovery paths.
- Global, UI, startup, and reminder error logging now uses explicit `ILogger.IsEnabled(...)` guards and logs only safe metadata such as exception type names instead of exception messages, stack traces, health-record identifiers, or user-entered health content.
- Corrected CA1873 eager logger-argument evaluation findings surfaced by GitHub-hosted MAUI builds.
- Corrected CA1861 constant-array allocation guidance in architecture tests.
- Corrected cross-platform project-reference path parsing and a nullable filename return caught by exact-head CI.
- Corrected policy-test false positives caused by generated `obj` global-using files.
- Added regression evidence that invalid spring-forward local schedule times do not cause CareNest to invent a replacement reminder time.
- Added explicit regression evidence that duplicate user-entered clock times do not create duplicate reminder occurrences.
- Corrected CA2263 surfaced by verification PR #29 by replacing the new non-generic `Enum.IsDefined(Type, object)` schedule-kind check with generic `Enum.IsDefined(schedule.Kind)` instead of weakening analyzer policy.

### Verification

Latest exact production source head verified: `c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`.

Verification PR #30 used marker head `59016b7e2b13d5ac1c93cf0db973f275c6e7eb19`, changed only `build/verification/rc1-ownership-utc-dst-hardening-20260810-2.txt`, and was closed without merge after success.

- CareNest CI #248 / `31382194805`: success.
- Platform-neutral formatting: success.
- Unit tests: 74 passed, 0 failed, 0 skipped.
- Integration tests: 13 passed, 0 failed, 0 skipped.
- UI-contract/policy tests: 54 passed, 0 failed, 0 skipped.
- Total core automated tests: 141 passed, 0 failed, 0 skipped.
- Android Release: success.
- Windows Release: success.
- iOS simulator Release: success.
- Mac Catalyst Release: success.
- CodeQL #248 / `31382194687`: success.
- Dependency Audit #10 / `31382194683`: success.

PR #29 / source head `04057299fe6d13012734ba235e6fa92604753948` was a superseded marker-only verification. CI #246 / `31382027314` exposed CA2263 in the new schedule-kind validation. `main` was corrected in commit `c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`, PR #29 was closed without merge, and PR #30 reran the complete matrix on that exact corrected source head.

The previous green source baseline was PR #28 / source head `69c4dd9319f7dc47edea1786e683f7d90c656e1e` with 101 core tests and all automated platform/security gates green. PR #30 supersedes that baseline after the additional ownership/UTC/snooze/DST/property hardening.

Earlier superseded verification PRs #24–#26 exposed and drove fixes for analyzer, privacy-logging, path-normalization, generated-source scanning, and nullable-contract problems instead of weakening those gates.

### Security

- `GHSA-2m69-gcr7-jv3q` remains explicitly open for the SQLitePCLRaw `2.1.11` dependency path. This continuation does not claim it fixed.
- The exact advisory suppression remains narrowly scoped and is still governed by `docs/security/DEPENDENCY_RISK_REGISTER.md` and `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.
- Reminder ownership validation fails closed on inconsistent local entity relationships; it does not transmit data or add a server/account dependency.
- App-lock verifier memory handling is hardened, but app lock remains a local privacy barrier and does not claim protection against a compromised/rooted/jailbroken device or weak-PIN offline guessing.
- Public production promotion remains blocked on an explicit dependency-risk decision/resolution plus manual device/accessibility/store/signing work and final release evidence.

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
