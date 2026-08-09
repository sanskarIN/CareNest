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
- GitHub workflow, CodeQL, Dependabot, release checklist, troubleshooting and contribution documentation.
- Branding vector sources and store guidance.
- Initial release implementation merged to `main` through PR #3.
- CodeQL verification succeeded for the release branch.
- Post-merge CI blockers were identified from GitHub Actions logs and corrected on `main`:
  - CA1848 logging-optimization analyzer no longer blocks release builds.
  - SQLitePCLRaw native packages were moved past vulnerable `2.1.11` and centrally pinned consistently.

## Current

- Pull request #10 exercises the complete pull-request CI matrix against the post-merge fixes.
- Final release tagging should occur only after the verification matrix is green.

## Deferred to later versions

- Cloud synchronization.
- Remote caregiver collaboration or silent background sharing.
- Accounts, mobile-number authentication, or server-side storage.
- Medical interpretation, diagnosis, treatment advice, interaction checking, or clinical risk scoring.
- Any analytics/telemetry until explicit consent and a privacy review exist.

## Environment limitation

The local execution container used for repository assembly does not include the `dotnet` command or MAUI workloads. Local restore, formatting, compilation, emulator/device smoke tests, signing, and store packaging therefore cannot be truthfully claimed as executed inside that container. GitHub-hosted CI is the authoritative automated build/test verification surface for this delivery.

See `what_changed.md` for the implementation and verification record.
