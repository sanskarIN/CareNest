# CareNest

CareNest is an open-source, local-first health organizer built with .NET MAUI and C#. It helps people organize medicine reminders, appointments, health documents, stock/refill notes, and multiple local family profiles without requiring an account or network connection.

[![Support CareNest on Buy Me a Coffee](src/CareNest.App/Resources/Images/carenest_support.svg)](https://buymeacoffee.com/sanskarIN)

> **Medical limitation:** CareNest is an organizational tool. It does not diagnose conditions, determine dosage, recommend treatment, verify adherence, replace a clinician or pharmacist, or provide emergency services. Follow instructions from qualified professionals. In an emergency, contact local emergency services instead of relying on this app.

## Highlights

- Local-first SQLite data; no account or server required.
- Multiple local profiles with optional app lock.
- User-defined medicine schedules without dosage inference.
- Reminder lifecycle: scheduled, snoozed, taken, skipped, delayed, and missed.
- Appointment planning and history.
- Encrypted local health-document vault.
- Stock/refill tracking based only on user-entered quantities.
- Per-profile JSON export plus PDF/CSV reports with privacy and clinical-limit warnings.
- Manual password-encrypted backup/restore, including portable recovery of locally encrypted documents.
- Light, dark, system theme and accessibility-ready layouts.
- Android, iOS, Mac Catalyst, and Windows targets.
- Privacy-aware developer diagnostics and exception-log redaction contracts.
- Automated formatting, architecture, repository-policy, data-model, ViewModel, branding, async-safety, and logging-privacy quality gates.

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

## Quick start

Prerequisites, platform setup, and the full target-specific command set are in [`docs/setup/DEVELOPMENT.md`](docs/setup/DEVELOPMENT.md).

Build and test the platform-neutral layers first:

```bash
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

## Verified automated quality baseline

Exact source head `8417513db36c72b0ec2cfaccadb6ac47ba361f11` passed the final hardening verification through PR #27:

- CareNest CI #200 / `31375336226` — success;
- platform-neutral formatting — success;
- 15 unit tests — passed;
- 11 integration tests — passed;
- 46 UI-contract/policy tests — passed;
- Android Release — success;
- Windows Release — success;
- iOS simulator Release — success;
- Mac Catalyst Release — success;
- CodeQL #200 / `31375336083` — success;
- Dependency Audit #7 / `31375336088` — success.

That automated evidence is necessary but not sufficient for final `1.0.0` publication. Manual device/accessibility testing, current store-policy review, signing/package work, and the tracked SQLite dependency-risk decision remain production gates.

See [`PROJECT_STATUS.md`](PROJECT_STATUS.md), [`docs/releases/RELEASE_CHECKLIST.md`](docs/releases/RELEASE_CHECKLIST.md), and [`docs/releases/QUALITY_GATE.md`](docs/releases/QUALITY_GATE.md).

## Privacy and security

Read [`PRIVACY.md`](PRIVACY.md), [`SECURITY.md`](SECURITY.md), the threat model in [`docs/security/THREAT_MODEL.md`](docs/security/THREAT_MODEL.md), the logging privacy contract in [`docs/security/LOGGING_PRIVACY.md`](docs/security/LOGGING_PRIVACY.md), and the dependency risk register in [`docs/security/DEPENDENCY_RISK_REGISTER.md`](docs/security/DEPENDENCY_RISK_REGISTER.md).

The current release has no automatic cloud sync and no silent caregiver sharing. Known dependency advisories are not represented as fixed merely because CI contains a narrowly scoped audit exception; the dependency risk register is the source of truth for those open items.

## Release engineering

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

Project support does not unlock medical advice, premium health behavior, different reminder behavior, or access to user health data.

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

Contributions are welcome. Start with [`CONTRIBUTING.md`](CONTRIBUTING.md) and the code of conduct.
