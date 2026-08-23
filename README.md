# CareNest

<p align="center">
  <a href="https://ramsandesh.gumroad.com">
    <img src="docs/assets/gumroad_store_badge.svg" alt="Shop on Gumroad — https://ramsandesh.gumroad.com" width="900" />
  </a>
</p>

> **Current automated verification authority:** [`docs/releases/AUTOMATED_BASELINE.md`](docs/releases/AUTOMATED_BASELINE.md). It records only exact-source results that actually ran. Permanent dated verification records remain under `docs/releases/`; historical test counts are not automatically assigned to newer source.

CareNest is an open-source, local-first family health organizer built with .NET 10, C# and two presentation-host families: .NET MAUI for the established Android/iOS/iPadOS/Mac Catalyst/Windows application and Avalonia for Linux desktop and WebAssembly/browser reach. It helps users organize medicine reminders, appointments, encrypted health documents, stock/refill notes, reports, backups and multiple local profiles without requiring a CareNest account or CareNest-owned cloud service.

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

The intended source-controlled RC1 feature scope is implemented and heavily automated. PR #84 is the current verification-relevant continuation for Linux desktop and WebAssembly/browser presentation hosts, cross-platform configuration verification and associated dependency/release gates. Those new hosts prove configured build/presentation reach only; production feature parity remains evidence-driven and platform-specific.

Production promotion still requires applicable real-device/platform/browser validation, accessibility evidence, packaged existing-data/encrypted-data compatibility, production signing, final signed-package inspection/provenance, live store declarations/current policy review, exact production tagging and publication evidence.

Use [`PROJECT_STATUS.md`](PROJECT_STATUS.md), [`docs/releases/AUTOMATED_BASELINE.md`](docs/releases/AUTOMATED_BASELINE.md), [`docs/releases/NEXT_STEPS.md`](docs/releases/NEXT_STEPS.md) and [`docs/setup/CROSS_PLATFORM.md`](docs/setup/CROSS_PLATFORM.md) for the exact current state and platform boundaries.

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
- Established .NET MAUI targets for Android, iOS/iPadOS, Mac Catalyst and Windows.
- Avalonia Desktop host for Linux-capable desktop builds and Avalonia Browser host for modern WebAssembly-capable browsers.
- Explicit capability/parity boundary: configured Linux/browser build reach is not presented as native notification, secure-store, background-execution or full feature-parity evidence.
- Strict compiled MAUI XAML binding policy with `XC0022`–`XC0025` as errors.
- Fail-closed Avalonia XAML/host-wiring configuration verification with mutation-style self-tests.
- CodeQL, blocking dependency audit, release gates and package-inspection workflows.
- Deterministic structured final-package evidence tooling.
- Offline stable documentation-link integrity checks.
- Safe issue routing, a release-aware PR checklist and CODEOWNERS metadata.

## Current platform targets

- Android: .NET MAUI `net10.0-android`, minimum API 24.
- iOS/iPadOS: .NET MAUI `net10.0-ios`, minimum iOS 15.
- Mac Catalyst: .NET MAUI `net10.0-maccatalyst`, minimum 15.
- Windows: .NET MAUI `net10.0-windows10.0.19041.0`, minimum 10.0.19041.0.
- Linux desktop: Avalonia Desktop host targeting `net10.0`.
- Modern WebAssembly-capable browsers: Avalonia Browser host targeting `net10.0-browser`.

The Avalonia desktop host can also execute on supported Windows/macOS environments, but the MAUI application remains the established primary host for the original four platform families. See [`docs/setup/CROSS_PLATFORM.md`](docs/setup/CROSS_PLATFORM.md) for build commands and capability boundaries.

Application identity:

- title: `CareNest`;
- ID: `com.sanskar.carenest`;
- display version: `1.0.0-rc.1`.

## Technology

- .NET 10 / .NET MAUI
- Avalonia Desktop / Avalonia Browser WebAssembly
- C# / XAML / Avalonia XAML
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
  CareNest.App/                       # MAUI Android/iOS/Mac Catalyst/Windows
  CareNest.CrossPlatform/             # shared Avalonia application/views
  CareNest.CrossPlatform.Desktop/     # Linux/Windows/macOS Avalonia entry point
  CareNest.CrossPlatform.Browser/     # WebAssembly Avalonia entry point
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

Intended dependency direction keeps platform-neutral business rules outside presentation hosts:

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure
                                                        ^
                                                        |
                  CareNest.App (MAUI) ------------------+
                  CareNest.CrossPlatform (Avalonia) ----+
                    |- CareNest.CrossPlatform.Desktop
                    `- CareNest.CrossPlatform.Browser
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
python3 build/scripts/verify-cross-platform-targets.py
python3 build/scripts/test-verify-cross-platform-targets.py
```

For MAUI platform workloads and target-specific commands, use [`docs/setup/DEVELOPMENT.md`](docs/setup/DEVELOPMENT.md) and [`docs/setup/PLATFORM_SETUP.md`](docs/setup/PLATFORM_SETUP.md). For Linux desktop and WebAssembly/browser commands and capability boundaries, use [`docs/setup/CROSS_PLATFORM.md`](docs/setup/CROSS_PLATFORM.md).

## Strict XAML compiled bindings

The MAUI app project currently enables:

```xml
<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
<WarningsAsErrors>$(WarningsAsErrors);XC0022;XC0023;XC0024;XC0025</WarningsAsErrors>
```

Binding-bearing MAUI pages/templates are typed for XamlC, and repository tests protect the policy from regression. Avalonia XAML used by the cross-platform host is additionally XML-parsed by `build/scripts/verify-cross-platform-targets.py` so malformed XAML fails before expensive platform builds.

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

The current cross-platform landing hosts do not claim that native MAUI reminder/background behavior is already equivalent on Linux or in a browser. Any host-specific implementation must preserve explicit capability semantics and be validated on that platform.

## File, camera and share behavior

The established MAUI application supports user-initiated document/file selection and supported camera capture through an application file gateway. The MAUI implementation honors cancellation before/after application-controlled picker/camera boundaries and disposes a newly opened stream if cancellation arrives during stream opening.

This does not claim that every operating-system picker can be programmatically force-cancelled by a .NET cancellation token, or that browser/Linux adapters already provide identical file, camera, share or secure-storage behavior.

## Security model

CareNest uses separate controls for structured data, encrypted documents, backups, secure-store secrets and optional app lock. Exported copies and compromised devices remain outside some protections. Browser and native operating systems also expose different storage/security primitives; cross-platform adapters must preserve the documented local-first/privacy boundary rather than silently weakening it. See [`SECURITY.md`](SECURITY.md), [`docs/security/SECURITY_MODEL.md`](docs/security/SECURITY_MODEL.md) and [`docs/security/THREAT_MODEL.md`](docs/security/THREAT_MODEL.md).

## Automated verification

Latest promoted exact-source evidence:

[`docs/releases/AUTOMATED_BASELINE.md`](docs/releases/AUTOMATED_BASELINE.md)

Exact-head procedure for a newer verification-relevant source:

[`docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`](docs/releases/VERIFICATION_BRANCH_PROTOCOL.md)

PR #84 extends the CI source configuration with Linux desktop Release build and WebAssembly browser Release publish jobs plus cross-platform configuration/self-test gates. Those checks must succeed for the final exact head before the new source can replace an older accepted automated baseline.

A green automated matrix means the configured automated gates passed for the named source. It does **not** guarantee global bug-freedom, complete feature parity or complete manual production qualification.

## Dependency and toolchain maintenance

Current source package versions and GitHub Actions majors:

[`docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`](docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md)

Dependency upgrades remain subject to unsuppressed audit and the applicable exact-source build/test/platform/store matrix. Avalonia desktop and browser dependency graphs are included in the current cross-platform Dependency Audit configuration. A historically green isolated Dependabot PR does not prove the final combined candidate; the combined candidate must pass.

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
- [`docs/setup/CROSS_PLATFORM.md`](docs/setup/CROSS_PLATFORM.md) — Linux/browser host setup, architecture and capability boundaries.
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
