# CareNest

CareNest is an open-source, local-first health organizer built with .NET MAUI and C#. It helps people organize medicine reminders, appointments, health documents, stock/refill notes, reports, backups, and multiple local family profiles without requiring a CareNest account or CareNest network service.

[![Support CareNest on Buy Me a Coffee](src/CareNest.App/Resources/Images/carenest_support.svg)](https://buymeacoffee.com/sanskarIN)

> **Medical limitation:** CareNest is an organizational tool. It does not diagnose conditions, determine or infer dosage, recommend treatment, perform clinical medication-interaction checking, create clinical risk scores, verify adherence, replace a clinician/pharmacist, or provide emergency services. Follow instructions from qualified professionals. In an emergency, use the appropriate local emergency service rather than relying on this app.

## Current source status

CareNest is currently tracked as:

`1.0.0-rc.1`

The earlier README statement that PR #43 was a fully green automated baseline was incorrect. GitHub Actions records show that PR #43 passed formatting, platform builds, CodeQL, and Dependency Audit, but its core CI failed during integration testing and the UI-contract suite was skipped. PR #43 is therefore **not** release evidence.

The reminder-reconciliation defects exposed by that failed run have been corrected on `main`, together with follow-up architecture, compensation, test, and analyzer fixes. A fresh marker-only exact-head verification is required before this README will name a new fully green source baseline.

See:

- [`PROJECT_STATUS.md`](PROJECT_STATUS.md)
- [`what_changed.md`](what_changed.md)
- [`docs/releases/BUG_AUDIT_VERIFICATION_20260814.md`](docs/releases/BUG_AUDIT_VERIFICATION_20260814.md)
- [`docs/testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`](docs/testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md)
- [`docs/security/BUG_AUDIT_SECURITY_NOTES_20260814.md`](docs/security/BUG_AUDIT_SECURITY_NOTES_20260814.md)

The successful dependency audit does **not** mean every tracked advisory is fixed. `GHSA-2m69-gcr7-jv3q` remains open in the dependency risk register.

## Highlights

- Local-first SQLite records; no CareNest account/server required.
- Multiple local profiles with optional app lock.
- User-defined medicine schedules without dosage inference.
- Reminder lifecycle including scheduled, snoozed, taken, skipped, delayed, missed, and cancelled states.
- Deterministic reminder planning with explicit entity ownership, UTC-window, date/state and DST boundaries.
- Invalid DST-gap times are not replaced with guessed reminder clock times.
- `EveryNHours` invalid DST-gap anchors now fail closed instead of being silently shifted.
- Archived profiles and inactive medicines do not automatically materialize reminders.
- Snooze timestamps must be explicit future UTC values.
- Snoozed rows use snooze due time for upcoming/overdue handling.
- Rebuild explicitly reconciles SQLite reminder rows with existing operating-system scheduled requests.
- Medicine/profile delete flows cancel future platform requests before database cascade and compensate if the cascade fails.
- Appointment `StartsUtc` requires explicit UTC; local/unspecified ticks are not silently relabeled.
- Appointment rebuild does not repeatedly prompt for notification permission.
- Encrypted local document vault with failure-compensating import behavior.
- Missing/corrupt document master key plus existing encrypted payload fails closed instead of silently creating a replacement key.
- Decrypted temporary document exports use the managed `Exports` cache directory.
- New encrypted document/backup payloads use authenticated chunked AEAD framing v2; legacy v1 remains readable for compatibility.
- Strict decrypted-backup archive topology validation before extraction.
- Backup completion is distinguished from later best-effort local bookkeeping.
- Sensitive application-owned verifier/key/salt/crypto buffers are cleared where managed-memory control permits.
- Stock/refill tracking based only on user-entered quantities.
- Per-profile JSON export plus PDF/CSV reports with privacy and clinical-limit warnings.
- CSV formula-like user text is neutralized in the portable spreadsheet representation.
- CSV/PDF/JSON writers use partial-file staging plus atomic final move.
- Manual password-encrypted backup/restore, including portable recovery of locally encrypted documents.
- Light, dark, system theme and accessibility-ready layouts.
- Android, iOS, Mac Catalyst, and Windows targets.
- Privacy-aware developer diagnostics and exception-log redaction contracts.
- Transactional multi-step SQLite operations and schema migrations.
- Failure-safe onboarding/app-lock/profile-photo workflows.
- Android `BroadcastReceiver.GoAsync()` recovery lifetime protection.
- Windows in-process reminder fallback protected against replacement/cancellation/disposal timer races.
- Independent startup recovery boundaries for medicine, appointment and backup reminder recovery.
- Automated formatting, architecture, repository-policy, data-model, ViewModel, branding, async-safety, logging-privacy, app-lock, reminder-integrity, direct-service, backup-topology, authenticated-stream, recurrence, snapshot-integrity, report-export, transaction and platform-lifecycle quality gates.

## Technology

- .NET 10 / .NET MAUI
- C# / XAML
- MVVM-style presentation separation
- SQLite (`sqlite-net-pcl`)
- built-in .NET cryptography for encrypted document/backup payloads
- xUnit
- GitHub Actions CI
- CodeQL
- Dependency Audit
- release-evidence/release-gate workflows

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
.github/
```

## Documentation

The documentation hub is [`docs/README.md`](docs/README.md).

Important current references:

- [`PROJECT_STATUS.md`](PROJECT_STATUS.md) — current automated baseline and real production blockers.
- [`what_changed.md`](what_changed.md) — complete active continuation handoff.
- [`docs/releases/BUG_AUDIT_VERIFICATION_20260814.md`](docs/releases/BUG_AUDIT_VERIFICATION_20260814.md) — 2026-08-14 bug-audit evidence and corrections.
- [`docs/testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`](docs/testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md) — defect-to-test map.
- [`docs/security/BUG_AUDIT_SECURITY_NOTES_20260814.md`](docs/security/BUG_AUDIT_SECURITY_NOTES_20260814.md) — security/privacy-relevant audit notes.
- [`docs/USER_GUIDE.md`](docs/USER_GUIDE.md) — complete user guide.
- [`docs/FEATURE_REFERENCE.md`](docs/FEATURE_REFERENCE.md) — feature-by-feature behavior/boundaries.
- [`docs/architecture/ARCHITECTURE.md`](docs/architecture/ARCHITECTURE.md) — system architecture.
- [`docs/architecture/APPLICATION_FLOWS.md`](docs/architecture/APPLICATION_FLOWS.md) — runtime flows.
- [`docs/architecture/DATABASE_SCHEMA.md`](docs/architecture/DATABASE_SCHEMA.md) — schema/migrations/WAL model.
- [`docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md) — platform notification behavior/limitations.
- [`docs/architecture/DOCUMENT_VAULT.md`](docs/architecture/DOCUMENT_VAULT.md) — encrypted document vault.
- [`docs/architecture/BACKUP_AND_RESTORE.md`](docs/architecture/BACKUP_AND_RESTORE.md) — encrypted backup/restore.
- [`docs/REPORTS_AND_EXPORTS.md`](docs/REPORTS_AND_EXPORTS.md) — report/export semantics.
- [`docs/privacy/PRIVACY_MODEL.md`](docs/privacy/PRIVACY_MODEL.md) — privacy architecture.
- [`docs/security/SECURITY_MODEL.md`](docs/security/SECURITY_MODEL.md) — security architecture.
- [`docs/security/THREAT_MODEL.md`](docs/security/THREAT_MODEL.md) — threats/controls/residual risk.
- [`docs/security/DEPENDENCY_RISK_REGISTER.md`](docs/security/DEPENDENCY_RISK_REGISTER.md) — dependency risk source of truth.
- [`docs/design/ACCESSIBILITY.md`](docs/design/ACCESSIBILITY.md) — accessibility specification/manual checks.
- [`docs/testing/TESTING_GUIDE.md`](docs/testing/TESTING_GUIDE.md) — automated/manual testing reference.
- [`docs/releases/RELEASE_PROCESS.md`](docs/releases/RELEASE_PROCESS.md) — production release process.
- [`docs/releases/RELEASE_CHECKLIST.md`](docs/releases/RELEASE_CHECKLIST.md) — release gate checklist.
- [`docs/releases/NEXT_STEPS.md`](docs/releases/NEXT_STEPS.md) — current operational work.

## Quick start

See [`docs/setup/DEVELOPMENT.md`](docs/setup/DEVELOPMENT.md) and [`docs/setup/PLATFORM_SETUP.md`](docs/setup/PLATFORM_SETUP.md) for complete prerequisites and target-specific commands.

Platform-neutral build/test examples:

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

Android example on a machine provisioned for the Android MAUI workload:

```bash
dotnet workload install maui-android
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

`CareNestTargetFramework` narrows the active multi-target MAUI framework before restore/build so a platform-specific machine does not have to evaluate unrelated target workloads and does not propagate the app target framework into the platform-neutral projects.

## Deterministic reminder scheduling

CareNest never chooses a medicine dose or infers how often a medicine should be used. Occurrences are generated only from explicit user-entered schedule values.

The scheduling contract is documented in [`docs/testing/REMINDER_SCHEDULING_CONTRACT.md`](docs/testing/REMINDER_SCHEDULING_CONTRACT.md).

Important invariants include:

- profile/medicine/schedule ownership validation;
- UTC planning windows;
- half-open planning boundaries;
- stable occurrence keys;
- duplicate-time deduplication;
- state/date limits;
- selected-weekday/cycle/every-N-hours rules;
- explicit future-UTC snoozes;
- DST gap/overlap handling;
- no invented replacement time for an invalid local clock time;
- reconciliation of stale platform requests after schedule/state/policy changes.

## Encrypted stream compatibility

New encrypted document/backup payloads use shared chunked AES-256-GCM framing version 2.

V2 authenticates terminal state so an authenticated chunk prefix cannot be accepted as a complete new stream merely because bytes end at a chunk boundary. Trailing data after the terminal is rejected.

Legacy framing version 1 remains readable for compatibility with existing CareNest data. Historical v1 ciphertext is not represented as retroactively upgraded.

## Privacy and security

Read:

- [`PRIVACY.md`](PRIVACY.md)
- [`SECURITY.md`](SECURITY.md)
- [`docs/privacy/PRIVACY_MODEL.md`](docs/privacy/PRIVACY_MODEL.md)
- [`docs/security/SECURITY_MODEL.md`](docs/security/SECURITY_MODEL.md)
- [`docs/security/THREAT_MODEL.md`](docs/security/THREAT_MODEL.md)
- [`docs/security/LOGGING_PRIVACY.md`](docs/security/LOGGING_PRIVACY.md)
- [`docs/security/DEPENDENCY_RISK_REGISTER.md`](docs/security/DEPENDENCY_RISK_REGISTER.md)
- [`docs/security/BUG_AUDIT_SECURITY_NOTES_20260814.md`](docs/security/BUG_AUDIT_SECURITY_NOTES_20260814.md)

The optional app lock is a local privacy barrier. It is not represented as transparent whole-SQLite-database encryption or a replacement for device security.

## Open dependency risk

`GHSA-2m69-gcr7-jv3q` remains tracked for the current SQLitePCLRaw native dependency path.

A narrowly scoped audit suppression is not remediation. See:

- [`docs/security/DEPENDENCY_RISK_REGISTER.md`](docs/security/DEPENDENCY_RISK_REGISTER.md)
- [`docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`](docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md)

## Release engineering

Automated source verification is necessary but not sufficient for public production promotion.

Still required include real-device/accessibility checks, current store-policy review, signing/package work, store metadata, final Release Evidence, and explicit dependency-risk disposition.

See:

- [`docs/releases/RELEASE_PROCESS.md`](docs/releases/RELEASE_PROCESS.md)
- [`docs/releases/RELEASE_CHECKLIST.md`](docs/releases/RELEASE_CHECKLIST.md)
- [`docs/releases/RELEASE_EVIDENCE.md`](docs/releases/RELEASE_EVIDENCE.md)
- [`docs/releases/SECURITY_RELEASE_REVIEW.md`](docs/releases/SECURITY_RELEASE_REVIEW.md)
- [`docs/releases/MANUAL_TEST_MATRIX.md`](docs/releases/MANUAL_TEST_MATRIX.md)
- [`docs/releases/STORE_SUBMISSION_CHECKLIST.md`](docs/releases/STORE_SUBMISSION_CHECKLIST.md)
- [`docs/releases/NEXT_STEPS.md`](docs/releases/NEXT_STEPS.md)

## ☕ Support CareNest

**[Buy Me a Coffee → https://buymeacoffee.com/sanskarIN](https://buymeacoffee.com/sanskarIN)**

If you want to voluntarily support CareNest, that support helps continued open-source design, testing, documentation, accessibility, platform maintenance, and future releases.

Project support does not unlock medical advice, premium health behavior, different reminder behavior, support priority, or access to user health data.

## Branding

- Product: **CareNest**
- Watermark: **Made by the Sanskar**
- Business: `sanskarin@outlook.in`
- Support: `supportramsandesh@gmail.com`
- Creator: `https://www.github.com/sanskarIN`
- Voluntary support: `https://buymeacoffee.com/sanskarIN`

## Open source

Licensed under Apache License 2.0. See [`LICENSE`](LICENSE).

Contributions are welcome. Start with [`CONTRIBUTING.md`](CONTRIBUTING.md), [`docs/setup/DEVELOPMENT.md`](docs/setup/DEVELOPMENT.md), and the code of conduct.
