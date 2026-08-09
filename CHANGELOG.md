# Changelog

All notable changes follow Keep a Changelog principles and semantic versioning.

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
