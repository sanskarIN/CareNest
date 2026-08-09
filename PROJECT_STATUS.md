# Project Status

## Release target

`1.0.0-rc.1` source-complete release candidate.

## Completed

- Product scope, medical safety boundary, privacy model, threat model, architecture, design system.
- Multi-project .NET MAUI solution structure.
- Local SQLite schema, migrations, repositories, audit entries, settings.
- Profiles, medicines, schedules, reminder occurrences, medication log, appointments, documents, stock adjustments, tags.
- Encrypted document storage.
- Manual password-encrypted, schema-versioned backup/restore package with portable encrypted-document key recovery.
- Per-profile structured JSON export plus PDF/CSV report services.
- MAUI navigation, onboarding, dashboard, profiles, medicines, log, appointments, documents, reports, settings, and About.
- Android/iOS/Mac Catalyst notification integrations and Windows fallback diagnostics.
- App lock primitives and secure secret storage.
- Unit/integration/UI-contract tests.
- GitHub workflow, release checklist, troubleshooting and contribution documentation.
- Branding vector sources and store guidance.

## Current

- External build verification on a machine with the .NET 10 SDK and MAUI workloads.

## Deferred to later versions

- Cloud synchronization.
- Remote caregiver collaboration or silent background sharing.
- Accounts, mobile-number authentication, or server-side storage.
- Medical interpretation, diagnosis, treatment advice, interaction checking, or clinical risk scoring.
- Any analytics/telemetry until explicit consent and a privacy review exist.

## Blocked in the current execution environment

The repository source can be created and reviewed here, but this execution environment does not include the `dotnet` command or MAUI workloads. Therefore local `dotnet restore`, formatting, compilation, emulator/device smoke tests, signing, and store packaging cannot be truthfully claimed as executed here. CI is configured to perform automated restore/build/test checks on GitHub-hosted runners.

See `what_changed.md` for implementation and verification details.
