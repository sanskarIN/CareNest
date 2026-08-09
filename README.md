# CareNest

CareNest is an open-source, local-first health organizer built with .NET MAUI and C#. It helps people organize medicine reminders, appointments, health documents, stock/refill notes, and multiple local family profiles without requiring an account or network connection.

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
- Privacy-aware developer diagnostics.

## Technology

- .NET 10 / .NET MAUI
- C# / XAML
- MVVM-style presentation separation
- SQLite (`sqlite-net-pcl`)
- Built-in .NET cryptography for encrypted documents/backups
- xUnit tests
- GitHub Actions quality gates

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

## Privacy and security

Read [`PRIVACY.md`](PRIVACY.md), [`SECURITY.md`](SECURITY.md), the threat model in [`docs/security/THREAT_MODEL.md`](docs/security/THREAT_MODEL.md), and the dependency risk register in [`docs/security/DEPENDENCY_RISK_REGISTER.md`](docs/security/DEPENDENCY_RISK_REGISTER.md).

The current release has no automatic cloud sync and no silent caregiver sharing. Known dependency advisories are not represented as fixed merely because CI contains a narrowly scoped audit exception; the dependency risk register is the source of truth for those open items.

## Branding

- Product: **CareNest**
- Watermark: **Made by the Sanskar**
- Business: `sanskarin@outlook.in`
- Support: `supportramsandesh@gmail.com`
- Creator: https://www.github.com/sanskarIN

## Open source

Licensed under the Apache License 2.0. See [`LICENSE`](LICENSE).

Contributions are welcome. Start with [`CONTRIBUTING.md`](CONTRIBUTING.md) and the code of conduct.
