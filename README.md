# CareNest

<p align="center">
  <a href="https://ramsandesh.gumroad.com">
    <img src="docs/assets/gumroad_store_badge.svg" alt="Shop on Gumroad — https://ramsandesh.gumroad.com" width="900" />
  </a>
</p>

> **Latest fully verified pre-storefront-integration source baseline — 2026-08-17:** `7cbe5568b6cffa06c279b29f3cb1b107ea988791`. The configured matrix passed **334/334 core tests** (122 unit + 39 integration + 173 UI/source-policy), Android/Windows/iOS-simulator/Mac-Catalyst Release builds, all four store-candidate configurations, and CodeQL. The current repository may contain newer repository-only Gumroad/documentation commits; those do not become a new verified baseline until their exact final workflow set is green.

CareNest is an open-source, local-first family health organizer built with .NET MAUI and C#. It helps users organize medicine reminders, appointments, encrypted health documents, stock/refill notes, reports, backups and multiple local profiles without requiring a CareNest account or CareNest-owned cloud service.

> **Medical limitation:** CareNest is organizational software. It does not diagnose conditions, calculate or infer dosage, recommend treatment, perform clinical medication-interaction checking, calculate clinical risk, verify adherence, replace a clinician/pharmacist, provide emergency services, or guarantee operating-system notification delivery.

## 🛍️ Ram Sandesh Gumroad Store

**[SHOP ON GUMROAD → https://ramsandesh.gumroad.com](https://ramsandesh.gumroad.com)**

The Ram Sandesh Gumroad storefront may contain separate digital products, books, learning resources, templates, project material, and documentation bundles.

The storefront is separate from CareNest health functionality. A purchase does not unlock diagnosis, treatment recommendations, dosage decisions, reminder priority/reliability, emergency assistance, or user health data.

Full storefront and placement policy:

- [`GUMROAD.md`](GUMROAD.md)
- [`docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`](docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md)

## Release status

Current release line:

`1.0.0-rc.1`

The source-controlled RC1 scope is implemented and heavily automated-verified. Production promotion still requires real-device/platform validation, accessibility evidence, packaged existing-data/encrypted-data compatibility, production signing, final signed-package inspection, current store-policy/metadata review, exact production tagging and publication evidence.

Use [`PROJECT_STATUS.md`](PROJECT_STATUS.md) and [`docs/releases/NEXT_STEPS.md`](docs/releases/NEXT_STEPS.md) for the exact current state.

## Highlights

- Local-first SQLite structured records.
- No required CareNest account/backend.
- Multiple local family/person profiles.
- Medicine records with user-entered strength/instruction text.
- Explicit schedules; no dosage or medical schedule inference.
- Deterministic reminder planning with time-zone/DST rules.
- Reminder states including scheduled, snoozed, taken, skipped, delayed, missed and cancelled.
- Stale OS-request reconciliation and cancellation-first recovery logic.
- Appointments with optional reminders.
- User-entered stock/refill notes.
- Encrypted imported-document vault.
- Password-encrypted manual backup/restore.
- Optional local app lock.
- CSV/PDF/JSON/report/export workflows with privacy boundaries.
- Light/dark/system theme support and accessibility-oriented source contracts.
- Android, iOS/iPadOS, Mac Catalyst and Windows targets.
- Strict compiled XAML binding policy with `XC0022`–`XC0025` as errors.
- CodeQL, blocking dependency audit, release gates and package-inspection workflows.

## Current platform targets

- Android: `net10.0-android`, minimum API 24.
- iOS/iPadOS: `net10.0-ios`, minimum iOS 15.
- Mac Catalyst: `net10.0-maccatalyst`, minimum 15.
- Windows: `net10.0-windows10.0.19041.0`, minimum 10.0.19041.0.

Application identity:

- title: `CareNest`;
- ID: `com.sanskar.carenest`;
- display version: `1.0.0-rc.1`.

## Technology

- .NET 10 / .NET MAUI
- C# / XAML
- MVVM-style presentation separation
- SQLite / `sqlite-net-pcl`
- authenticated .NET cryptography for document/backup payloads
- xUnit
- GitHub Actions
- CodeQL
- unsuppressed dependency audit

## Repository layout

```text
src/
  CareNest.Shared/
  CareNest.Domain/
  CareNest.Application/
  CareNest.Infrastructure/
  CareNest.App/
tests/
  CareNest.UnitTests/
  CareNest.IntegrationTests/
  CareNest.UiTests/
docs/
  assets/
  marketing/
build/scripts/
.github/workflows/
```

Intended dependency direction:

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

## Quick start

```bash
git clone https://github.com/sanskarIN/CareNest.git
cd CareNest

dotnet restore CareNest.sln

dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

For MAUI platform workloads and target-specific commands, use [`docs/setup/DEVELOPMENT.md`](docs/setup/DEVELOPMENT.md) and [`docs/setup/PLATFORM_SETUP.md`](docs/setup/PLATFORM_SETUP.md).

## Strict XAML compiled bindings

The app project currently enables:

```xml
<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
<WarningsAsErrors>$(WarningsAsErrors);XC0022;XC0023;XC0024;XC0025</WarningsAsErrors>
```

All binding-bearing pages/templates are typed for XamlC, and repository tests protect the policy from regression.

## Privacy model

Current v1 intentionally has:

- no required CareNest account/backend;
- no automatic CareNest cloud synchronization/upload;
- no hidden runtime analytics/telemetry client;
- local SQLite structured data;
- separately encrypted imported document payloads;
- password-encrypted manual backups;
- explicit user-controlled export/share/calendar/browser boundaries.

CareNest does not claim transparent whole-database encryption. See [`PRIVACY.md`](PRIVACY.md) and [`docs/privacy/PRIVACY_MODEL.md`](docs/privacy/PRIVACY_MODEL.md).

## Reminder model

CareNest separates:

1. explicit user schedule intent;
2. persisted reminder-occurrence state;
3. operating-system request state.

Because database and OS scheduling are not one atomic transaction, the implementation uses deterministic planning, reconciliation, cancellation-first ordering and compensation/recovery. See [`docs/testing/REMINDER_SCHEDULING_CONTRACT.md`](docs/testing/REMINDER_SCHEDULING_CONTRACT.md).

## Security model

CareNest uses separate controls for structured data, encrypted documents, backups, secure-store secrets and optional app lock. Exported copies and compromised devices remain outside some protections. See [`SECURITY.md`](SECURITY.md), [`docs/security/SECURITY_MODEL.md`](docs/security/SECURITY_MODEL.md) and [`docs/security/THREAT_MODEL.md`](docs/security/THREAT_MODEL.md).

## Latest fully verified automated baseline

Source:

`7cbe5568b6cffa06c279b29f3cb1b107ea988791`

Verified on that exact revision:

- 122/122 unit tests;
- 39/39 integration tests;
- 173/173 UI/source-policy tests;
- **334/334 total core tests**;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- all four store-candidate configurations;
- CodeQL.

This means no known automated defect remained under that configured matrix for that exact verified source. It does **not** mean the product is guaranteed globally bug-free or that manual production testing is complete.

## External storefront/funding package boundary

The distributed CareNest application source/package intentionally contains no external Buy Me a Coffee or Gumroad destination/card/command/promotional artwork. Repository support and storefront promotion remain separate from the health-application package and do not unlock health functionality, reminder priority/reliability, medical advice, clinical services, or access to user health data.

Repository links:

- **Gumroad:** https://ramsandesh.gumroad.com
- **Buy Me a Coffee:** https://buymeacoffee.com/sanskarIN
- [`GUMROAD.md`](GUMROAD.md)
- [`BUY_ME_A_COFFEE.md`](BUY_ME_A_COFFEE.md)

## Complete documentation

Start with:

- [`docs/DOCUMENTATION_CATALOG.md`](docs/DOCUMENTATION_CATALOG.md) — complete navigation/authority map.
- [`docs/COMPLETE_PROJECT_DOCUMENTATION.md`](docs/COMPLETE_PROJECT_DOCUMENTATION.md) — full end-to-end project reference.
- [`docs/GETTING_STARTED.md`](docs/GETTING_STARTED.md) — first steps.
- [`docs/USER_GUIDE.md`](docs/USER_GUIDE.md) — user guide.
- [`docs/DEVELOPER_REFERENCE.md`](docs/DEVELOPER_REFERENCE.md) — developer reference.
- [`docs/KNOWN_LIMITATIONS.md`](docs/KNOWN_LIMITATIONS.md) — limitations.
- [`docs/PLATFORM_BEHAVIOR_MATRIX.md`](docs/PLATFORM_BEHAVIOR_MATRIX.md) — automated/manual platform evidence.
- [`GUMROAD.md`](GUMROAD.md) — storefront guide.
- [`docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`](docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md) — storefront placement/package policy.
- [`PROJECT_STATUS.md`](PROJECT_STATUS.md) — current status.
- [`what_changed.md`](what_changed.md) — detailed continuation handoff.

The full documentation hub is [`docs/README.md`](docs/README.md).

## Contributing

Read [`CONTRIBUTING.md`](CONTRIBUTING.md). Use fictional/synthetic data only. Never commit real health records, PINs/passwords, encryption keys, private backups, access tokens or production signing material.

Maintainer Git identity convention:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

## Support and links

- **Gumroad storefront:** https://ramsandesh.gumroad.com
- **Buy Me a Coffee:** https://buymeacoffee.com/sanskarIN
- Support guide: [`SUPPORT.md`](SUPPORT.md)
- Security reports: [`SECURITY.md`](SECURITY.md)
- Privacy: [`PRIVACY.md`](PRIVACY.md)
- Terms: [`TERMS.md`](TERMS.md)

## License

CareNest is licensed under the Apache License 2.0. See [`LICENSE`](LICENSE) and [`NOTICE`](NOTICE).
