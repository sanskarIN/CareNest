# CareNest

<p align="center">
  <a href="https://ramsandesh.gumroad.com">
    <img src="docs/assets/gumroad_store_badge.svg" alt="Shop on Gumroad — https://ramsandesh.gumroad.com" width="900" />
  </a>
</p>

> **Current automated verification authority:** [`docs/releases/AUTOMATED_BASELINE.md`](docs/releases/AUTOMATED_BASELINE.md). It records only exact-source results that actually ran. Permanent dated verification records remain under `docs/releases/`; historical test counts are not automatically assigned to newer source.

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

The intended source-controlled RC1 feature scope is implemented and heavily automated. The current candidate also includes release/documentation/package-evidence tooling, dependency/action maintenance and final bug-hardening changes that require a fresh exact-head verification before a newer automated baseline is promoted.

Production promotion still requires applicable real-device/platform validation, accessibility evidence, packaged existing-data/encrypted-data compatibility, production signing, final signed-package inspection/provenance, live store declarations/current policy review, exact production tagging and publication evidence.

Use [`PROJECT_STATUS.md`](PROJECT_STATUS.md), [`docs/releases/AUTOMATED_BASELINE.md`](docs/releases/AUTOMATED_BASELINE.md) and [`docs/releases/NEXT_STEPS.md`](docs/releases/NEXT_STEPS.md) for the exact current state.

## Highlights

- Local-first SQLite structured records.
- No required CareNest account/backend.
- Multiple local family/person profiles and local caregiver-style profile organization.
- Profile images from file selection or supported camera capture.
- Emergency contact organization.
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
- Per-profile and aggregate CSV/PDF/JSON/report/export workflows with privacy boundaries.
- Quiet-hours/reminder diagnostics/developer support settings.
- Light/dark/system theme support, reduced-motion/large-interface settings and accessibility-oriented source contracts.
- Android, iOS/iPadOS, Mac Catalyst and Windows targets.
- Strict compiled XAML binding policy with `XC0022`–`XC0025` as errors.
- CodeQL, blocking dependency audit, release gates and package-inspection workflows.
- Deterministic structured final-package evidence tooling.
- Offline stable documentation-link integrity checks.
- Safe issue routing, a release-aware PR checklist and CODEOWNERS metadata.

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

Current package/action versions are documented in [`docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`](docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md); executable source configuration remains authoritative.

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
  architecture/
  assets/
  design/
  history/
  marketing/
  privacy/
  releases/
  security/
  setup/
  testing/
build/scripts/
.github/
  ISSUE_TEMPLATE/
  workflows/
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

Repository tooling checks:

```bash
python3 build/scripts/test-create-package-evidence.py
python3 build/scripts/test-verify-documentation-links.py
python3 build/scripts/verify-documentation-links.py
```

For MAUI platform workloads and target-specific commands, use [`docs/setup/DEVELOPMENT.md`](docs/setup/DEVELOPMENT.md) and [`docs/setup/PLATFORM_SETUP.md`](docs/setup/PLATFORM_SETUP.md).

## Strict XAML compiled bindings

The app project currently enables:

```xml
<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
<WarningsAsErrors>$(WarningsAsErrors);XC0022;XC0023;XC0024;XC0025</WarningsAsErrors>
```

Binding-bearing pages/templates are typed for XamlC, and repository tests protect the policy from regression.

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

## File, camera and share behavior

CareNest supports user-initiated document/file selection and supported camera capture through an application file gateway. The MAUI implementation honors cancellation before/after application-controlled picker/camera boundaries and disposes a newly opened stream if cancellation arrives during stream opening.

This does not claim that every operating-system picker can be programmatically force-cancelled by a .NET cancellation token.

## Security model

CareNest uses separate controls for structured data, encrypted documents, backups, secure-store secrets and optional app lock. Exported copies and compromised devices remain outside some protections. See [`SECURITY.md`](SECURITY.md), [`docs/security/SECURITY_MODEL.md`](docs/security/SECURITY_MODEL.md) and [`docs/security/THREAT_MODEL.md`](docs/security/THREAT_MODEL.md).

## Automated verification

Latest promoted exact-source evidence:

[`docs/releases/AUTOMATED_BASELINE.md`](docs/releases/AUTOMATED_BASELINE.md)

Exact-head procedure for a newer verification-relevant source:

[`docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`](docs/releases/VERIFICATION_BRANCH_PROTOCOL.md)

A green automated matrix means the configured automated gates passed for the named source. It does **not** guarantee global bug-freedom or complete manual production qualification.

## Dependency and toolchain maintenance

Current source package versions and GitHub Actions majors:

[`docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`](docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md)

Dependency upgrades remain subject to unsuppressed audit and the applicable exact-source build/test/platform/store matrix. A historically green isolated Dependabot PR does not prove the final combined candidate; the combined candidate must pass.

## Documentation integrity

Stable active local documentation links are verified offline by:

```bash
python3 build/scripts/verify-documentation-links.py
```

The checker fails closed for missing/repository-escaping local targets, excludes immutable `docs/history/` snapshots and the explicit post-verification dynamic evidence/status files by default, and supports `--include-dynamic` / `--include-history` for wider audits.

See [`docs/testing/DOCUMENTATION_INTEGRITY.md`](docs/testing/DOCUMENTATION_INTEGRITY.md).

## Structured package evidence

Final package checksum/provenance tooling is documented in [`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`](docs/releases/PACKAGE_EVIDENCE_TOOLING.md).

Production mode requires exact immutable tag/source/HEAD agreement, clean tracked workspace, real non-secret signing/notarization/store provenance and successful external-commerce payload scanning. It does not sign packages or prove store approval.

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
- [`docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`](docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md) — package/action baseline.
- [`docs/GETTING_STARTED.md`](docs/GETTING_STARTED.md) — first steps.
- [`docs/USER_GUIDE.md`](docs/USER_GUIDE.md) — user guide.
- [`docs/DEVELOPER_REFERENCE.md`](docs/DEVELOPER_REFERENCE.md) — developer reference.
- [`docs/CONFIGURATION_REFERENCE.md`](docs/CONFIGURATION_REFERENCE.md) — build/configuration/tooling reference.
- [`docs/testing/TESTING_GUIDE.md`](docs/testing/TESTING_GUIDE.md) — testing strategy.
- [`docs/testing/DOCUMENTATION_INTEGRITY.md`](docs/testing/DOCUMENTATION_INTEGRITY.md) — documentation link gate.
- [`docs/KNOWN_LIMITATIONS.md`](docs/KNOWN_LIMITATIONS.md) — limitations.
- [`docs/PLATFORM_BEHAVIOR_MATRIX.md`](docs/PLATFORM_BEHAVIOR_MATRIX.md) — automated/manual platform evidence.
- [`docs/releases/AUTOMATED_BASELINE.md`](docs/releases/AUTOMATED_BASELINE.md) — current exact automated evidence.
- [`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`](docs/releases/PACKAGE_EVIDENCE_TOOLING.md) — package evidence tooling.
- [`docs/releases/STORE_POLICY_REVIEW_20260818.md`](docs/releases/STORE_POLICY_REVIEW_20260818.md) — dated preliminary store-policy review.
- [`GUMROAD.md`](GUMROAD.md) — storefront guide.
- [`docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`](docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md) — storefront placement/package policy.
- [`PROJECT_STATUS.md`](PROJECT_STATUS.md) — current status.
- [`what_changed.md`](what_changed.md) — detailed continuation handoff.

The full documentation hub is [`docs/README.md`](docs/README.md).

## Contributing

Read [`CONTRIBUTING.md`](CONTRIBUTING.md) and [`.github/PULL_REQUEST_TEMPLATE.md`](.github/PULL_REQUEST_TEMPLATE.md). Use fictional/synthetic data only. Never commit real health records, PINs/passwords, encryption keys, private backups, access tokens or production signing material.

Issue routing:

- public bugs/features: repository issue forms;
- security vulnerabilities: private security-advisory/reporting path from [`.github/ISSUE_TEMPLATE/config.yml`](.github/ISSUE_TEMPLATE/config.yml);
- support/privacy questions: repository support/privacy documentation.

Default code ownership is declared in [`.github/CODEOWNERS`](.github/CODEOWNERS).

Maintainer Git identity convention:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

API/connector-created commits must be evaluated by actual commit metadata; local Git configuration cannot be assumed to apply when a connector does not expose author-email controls.

## Support and links

- **Gumroad storefront:** https://ramsandesh.gumroad.com
- **Buy Me a Coffee:** https://buymeacoffee.com/sanskarIN
- Support guide: [`SUPPORT.md`](SUPPORT.md)
- Security reports: [`SECURITY.md`](SECURITY.md)
- Privacy: [`PRIVACY.md`](PRIVACY.md)
- Terms: [`TERMS.md`](TERMS.md)

## License

CareNest is licensed under the Apache License 2.0. See [`LICENSE`](LICENSE) and [`NOTICE`](NOTICE).
