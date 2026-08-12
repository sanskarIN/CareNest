# CareNest

CareNest is an open-source, local-first health organizer built with .NET MAUI and C#. It helps people organize medicine reminders, appointments, health documents, stock/refill notes, and multiple local family profiles without requiring an account or network connection.

[![Support CareNest on Buy Me a Coffee](src/CareNest.App/Resources/Images/carenest_support.svg)](https://buymeacoffee.com/sanskarIN)

> **Medical limitation:** CareNest is an organizational tool. It does not diagnose conditions, determine dosage, recommend treatment, verify adherence, replace a clinician or pharmacist, or provide emergency services. Follow instructions from qualified professionals. In an emergency, contact local emergency services instead of relying on this app.

## Highlights

- Local-first SQLite data; no account or server required.
- Multiple local profiles with optional app lock.
- User-defined medicine schedules without dosage inference.
- Reminder lifecycle: scheduled, snoozed, taken, skipped, delayed, and missed.
- Deterministic reminder materialization with explicit entity-ownership, date/time/time-zone, UTC-window, and DST boundaries.
- Invalid DST-gap times are not replaced with guessed reminder times.
- Archived profiles and inactive medicine states do not automatically materialize reminders.
- Snooze timestamps must be explicit future UTC values before platform scheduling.
- Appointment planning and history.
- Encrypted local health-document vault.
- Stock/refill tracking based only on user-entered quantities.
- Per-profile JSON export plus PDF/CSV reports with privacy and clinical-limit warnings.
- Manual password-encrypted backup/restore, including portable recovery of locally encrypted documents.
- Light, dark, system theme and accessibility-ready layouts.
- Android, iOS, Mac Catalyst, and Windows targets.
- Privacy-aware developer diagnostics and exception-log redaction contracts.
- Automated formatting, architecture, repository-policy, data-model, ViewModel, branding, async-safety, logging-privacy, app-lock, reminder-integrity, randomized-recurrence, and snapshot-integrity quality gates.

## Technology

- .NET 10 / .NET MAUI
- C# / XAML
- MVVM-style presentation separation
- SQLite (`sqlite-net-pcl`)
- Built-in .NET cryptography for encrypted documents/backups
- xUnit tests
- GitHub Actions CI, CodeQL, Dependency Audit, Release Gate, and Release Evidence workflows

## Repository layout

```text
src/
  CareNest.App/
  CareNest.Domain/
  CareNest.Application/
  CareNest.Infrastructure/
  CareNest.Shared/
tests/
  CareNest.UnitTests/
  CareNest.IntegrationTests/
  CareNest.UiTests/
docs/
build/scripts/
```

## Documentation

The complete documentation hub is [`docs/README.md`](docs/README.md).

Key references:

- [`docs/USER_GUIDE.md`](docs/USER_GUIDE.md) — complete user guide.
- [`docs/FEATURE_REFERENCE.md`](docs/FEATURE_REFERENCE.md) — feature-by-feature behavior and boundaries.
- [`docs/architecture/ARCHITECTURE.md`](docs/architecture/ARCHITECTURE.md) — full architecture.
- [`docs/architecture/APPLICATION_FLOWS.md`](docs/architecture/APPLICATION_FLOWS.md) — runtime flows.
- [`docs/architecture/DATABASE_SCHEMA.md`](docs/architecture/DATABASE_SCHEMA.md) — schema/entities/migrations/WAL.
- [`docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md) — Android/iOS/Mac/Windows notification behavior and limitations.
- [`docs/architecture/DOCUMENT_VAULT.md`](docs/architecture/DOCUMENT_VAULT.md) — encrypted document-vault model.
- [`docs/architecture/BACKUP_AND_RESTORE.md`](docs/architecture/BACKUP_AND_RESTORE.md) — encrypted backup/restore model.
- [`docs/REPORTS_AND_EXPORTS.md`](docs/REPORTS_AND_EXPORTS.md) — JSON/PDF/CSV/document/calendar export contracts.
- [`docs/privacy/PRIVACY_MODEL.md`](docs/privacy/PRIVACY_MODEL.md) — complete privacy architecture.
- [`docs/security/SECURITY_MODEL.md`](docs/security/SECURITY_MODEL.md) — security architecture and limitations.
- [`docs/design/ACCESSIBILITY.md`](docs/design/ACCESSIBILITY.md) — accessibility specification.
- [`docs/testing/TESTING_GUIDE.md`](docs/testing/TESTING_GUIDE.md) — automated/manual testing guide.
- [`docs/setup/PLATFORM_SETUP.md`](docs/setup/PLATFORM_SETUP.md) — cross-platform development setup.
- [`docs/setup/MAINTAINER_OPERATIONS.md`](docs/setup/MAINTAINER_OPERATIONS.md) — maintainer/CI/release operations.
- [`docs/releases/RELEASE_PROCESS.md`](docs/releases/RELEASE_PROCESS.md) — end-to-end release process.
- [`docs/releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md`](docs/releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md) — documentation inventory and operational gates.
- [`docs/DOCUMENTATION_STANDARDS.md`](docs/DOCUMENTATION_STANDARDS.md) — documentation accuracy/evidence rules.

## Quick start

Prerequisites, platform setup, and the full target-specific command set are in [`docs/setup/DEVELOPMENT.md`](docs/setup/DEVELOPMENT.md) and [`docs/setup/PLATFORM_SETUP.md`](docs/setup/PLATFORM_SETUP.md).

Build and test the platform-neutral layers first:

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

For Android on a machine with the Android MAUI workload:

```bash
dotnet workload install maui-android
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

The `CareNestTargetFramework` property intentionally narrows the multi-target MAUI project before restore/build, so a platform-specific machine does not need to evaluate unrelated target workloads and the app target framework does not propagate into the platform-neutral projects.

For Windows, iOS simulator, and Mac Catalyst commands, use [`docs/setup/DEVELOPMENT.md`](docs/setup/DEVELOPMENT.md).

## Deterministic reminder scheduling

CareNest never chooses a medicine dose or infers how frequently a medicine should be used. Reminder occurrences are derived only from explicit user-entered schedule values.

The exact planner invariants—entity ownership, UTC planning windows, half-open boundaries, stable occurrence keys, duplicate-time deduplication, state/date boundaries, selected-weekday/cycle/every-N-hours rules, snooze UTC requirements, and daylight-saving gap/overlap handling—are documented in [`docs/testing/REMINDER_SCHEDULING_CONTRACT.md`](docs/testing/REMINDER_SCHEDULING_CONTRACT.md).

A local clock time that does not exist during a daylight-saving spring-forward gap is not silently replaced with a guessed alternative time. Automated property-style tests use a fixed seed and synthetic user-entered schedules so recurrence-boundary checks are reproducible and non-clinical.

## Verified automated quality baseline

Exact runtime/test source head `c61f3c31c4ba33419c7b348fc8ee63a58eaa637b` passed the latest hardening verification through marker-only PR #30:

- CareNest CI #248 / `31382194805` — success;
- platform-neutral formatting — success;
- 74 unit tests — passed;
- 13 integration tests — passed;
- 54 UI-contract/policy tests — passed;
- 141 total core automated tests — passed;
- Android Release — success;
- Windows Release — success;
- iOS simulator Release — success;
- Mac Catalyst Release — success;
- CodeQL #248 / `31382194687` — success;
- Dependency Audit #10 / `31382194683` — success.

PR #29 / CI #246 is intentionally recorded as a superseded verification: it exposed CA2263 in a new non-generic `Enum.IsDefined` call. The code was corrected on `main` and reverified through PR #30 rather than suppressing the analyzer.

Documentation-only commits after `c61f3c31...` do not change the runtime/test source represented by that evidence and are not a separate platform-verification baseline.

That automated evidence is necessary but not sufficient for final `1.0.0` publication. Manual device/accessibility/notification testing, current store-policy review, signing/package work, final Release Evidence for the promoted commit, and the tracked SQLite dependency-risk decision remain production gates.

See [`PROJECT_STATUS.md`](PROJECT_STATUS.md), [`docs/releases/RELEASE_CHECKLIST.md`](docs/releases/RELEASE_CHECKLIST.md), [`docs/releases/QUALITY_GATE.md`](docs/releases/QUALITY_GATE.md), and [`docs/releases/RELEASE_PROCESS.md`](docs/releases/RELEASE_PROCESS.md).

## Privacy and security

Read [`PRIVACY.md`](PRIVACY.md), [`SECURITY.md`](SECURITY.md), [`docs/privacy/PRIVACY_MODEL.md`](docs/privacy/PRIVACY_MODEL.md), [`docs/security/SECURITY_MODEL.md`](docs/security/SECURITY_MODEL.md), the threat model in [`docs/security/THREAT_MODEL.md`](docs/security/THREAT_MODEL.md), the logging privacy contract in [`docs/security/LOGGING_PRIVACY.md`](docs/security/LOGGING_PRIVACY.md), and the dependency risk register in [`docs/security/DEPENDENCY_RISK_REGISTER.md`](docs/security/DEPENDENCY_RISK_REGISTER.md).

The current release has no automatic CareNest cloud sync and no silent caregiver sharing. Known dependency advisories are not represented as fixed merely because CI contains a narrowly scoped audit exception; the dependency risk register is the source of truth for those open items.

The optional app lock is a local privacy barrier. CareNest does not claim that app lock transparently encrypts the whole SQLite database or replaces device-level authentication/security.

## Release engineering

- [`docs/releases/RELEASE_PROCESS.md`](docs/releases/RELEASE_PROCESS.md) — end-to-end production release process.
- [`docs/releases/RELEASE_CHECKLIST.md`](docs/releases/RELEASE_CHECKLIST.md) — automated and manual promotion checklist.
- [`docs/releases/RELEASE_EVIDENCE.md`](docs/releases/RELEASE_EVIDENCE.md) — exact source/toolchain/test/dependency evidence process.
- [`docs/releases/SECURITY_RELEASE_REVIEW.md`](docs/releases/SECURITY_RELEASE_REVIEW.md) — pre-release security review.
- [`docs/releases/MANUAL_TEST_MATRIX.md`](docs/releases/MANUAL_TEST_MATRIX.md) — real/emulated-device checks.
- [`docs/releases/STORE_SUBMISSION_CHECKLIST.md`](docs/releases/STORE_SUBMISSION_CHECKLIST.md) — distribution-channel checks.
- [`docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`](docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md) — open SQLite dependency migration gate.
- [`docs/releases/RELEASE_NOTES_TEMPLATE.md`](docs/releases/RELEASE_NOTES_TEMPLATE.md) — evidence-aware release notes.
- [`docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`](docs/releases/VERIFICATION_BRANCH_PROTOCOL.md) — marker-only exact-head CI protocol.

## ☕ Support CareNest

**[Buy Me a Coffee → https://buymeacoffee.com/sanskarIN](https://buymeacoffee.com/sanskarIN)**

CareNest is open source. If you want to voluntarily support continued development, the link above helps fund design, testing, documentation, accessibility work, platform maintenance, and future releases.

Project support does not unlock medical advice, premium health behavior, different reminder behavior, support priority, or access to user health data.

## Next steps

The release-candidate promotion checklist, production blockers, manual device testing, signing/store preparation, and future-version ideas are tracked in [`docs/releases/NEXT_STEPS.md`](docs/releases/NEXT_STEPS.md).

## Branding

- Product: **CareNest**
- Watermark: **Made by the Sanskar**
- Business: `sanskarin@outlook.in`
- Support: `supportramsandesh@gmail.com`
- Creator: https://www.github.com/sanskarIN
- Voluntary support: **https://buymeacoffee.com/sanskarIN**

## Open source

Licensed under the Apache License 2.0. See [`LICENSE`](LICENSE).

Contributions are welcome. Start with [`CONTRIBUTING.md`](CONTRIBUTING.md), [`docs/setup/DEVELOPMENT.md`](docs/setup/DEVELOPMENT.md), and the code of conduct.