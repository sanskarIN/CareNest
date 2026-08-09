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

### Safety

- Added explicit non-diagnostic/non-treatment medical boundaries throughout onboarding, About, reports and documentation.
- Reminder and stock limitations are surfaced instead of silently inferred.
