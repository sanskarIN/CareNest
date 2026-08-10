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
- SQLite result-producing PRAGMAs for WAL mode, busy timeout and WAL checkpoint are handled correctly through scalar reads.
- WAL-backed backup snapshot regression coverage is included.
- MAUI per-target CI restore/build isolation is implemented without propagating app target frameworks into referenced `net10.0` projects.
- Android notification integration has explicit API-level guards and nullability checks.
- Apple verification uses a macOS 26 runner compatible with the current .NET 10 Apple workload.
- Voluntary Buy Me a Coffee project-support link is centralized at `https://buymeacoffee.com/sanskarIN` and exposed through the About/support surfaces and GitHub funding metadata.
- Custom CareNest Buy Me a Coffee vector artwork is stored at `src/CareNest.App/Resources/Images/buy_me_a_coffee_carenest.svg`.
- Clickable visual support pages are available at `BUY_ME_A_COFFEE.md` and `docs/SUPPORT_CARENEST.md`.
- Release preflight scripts are available for Bash and PowerShell.
- Manual cross-platform/device test matrix and store-submission checklist are documented.
- SQLite dependency migration/verification plan is documented for the open advisory path.
- Platform-neutral formatting verification is now part of the core CI job.
- Repository policy tests enforce no TODO/FIXME/NotImplemented placeholders in runtime source, no runtime network/telemetry client introduction, no clinical decision feature-name regressions, no common signing/secret files, and presence of required governance/release files.
- Architecture contract tests enforce Shared/Domain/Application/Infrastructure dependency direction and keep MAUI isolated to the app composition project.
- ViewModel contract tests enforce no direct SQLite/network-client access from concrete ViewModels and preserve the notification-permission and as-needed reminder boundaries.
- Data-model contract tests cover all entities required by the CareNest master prompt and preserve medicine strength/instruction values as opaque text.
- Branding/localization contract tests validate the adaptive icon, splash, BMC artwork, English safety resource keys, and highlighted support destination.
- Added original monochrome, light-surface, and dark-surface CareNest mark variants for system/brand surfaces.

## Security dependency status

NuGet audit reports `GHSA-2m69-gcr7-jv3q` for the SQLitePCLRaw native `2.1.11` package resolved by the current `sqlite-net-pcl` dependency chain.

An attempted `2.1.12` bundle pin was rejected because that version is not available on NuGet.org. The repository therefore does **not** claim this advisory is fixed. Instead:

- the exact advisory URL is temporarily suppressed through `NuGetAuditSuppress` so unrelated compile/test failures remain visible;
- no wildcard or severity-wide audit suppression is used;
- `docs/security/DEPENDENCY_RISK_REGISTER.md` records the open risk, mitigation context and review trigger;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` defines the upgrade/provider-migration regression gate;
- final production release review must upgrade/replace the dependency path or explicitly block release until the risk is acceptably resolved.

## Last fully verified runtime baseline

Funding-enabled verification PR #21 records the last fully green baseline before the current additional hardening work.

Verified source head: `52abe54cfc771c411b78332d78217a5876ebc4c8`.

Evidence recorded on PR #21:

- CareNest CI run #115 / `31302769113`: success.
- Unit tests: 15 passed.
- Integration tests: 11 passed.
- UI-contract tests: 10 passed.
- Android Release build: passed.
- Windows Release build: passed.
- iOS simulator Release build: passed.
- Mac Catalyst Release build: passed.
- CodeQL run #114 / `31302769108`: success.
- Dependency Audit run #4 / `31302769112`: success.

## Current hardening head

Current `main` now contains additional automated release-hardening changes after the last green baseline:

- platform-neutral `dotnet format --verify-no-changes` CI checks;
- repository policy/security boundary tests;
- architecture-boundary tests;
- ViewModel contract tests;
- required data-model contract tests;
- branding/localization contract tests;
- monochrome/light/dark CareNest mark variants.

Because these changes alter CI/test/resource files, the earlier PR #22 verification head is now stale and must not be treated as evidence for the newest `main` head. A fresh exact-head verification PR is required after this continuation is complete.

## Release blockers that remain real

- Complete manual device/emulator matrix on Android, Windows, iOS/iPadOS, and Mac Catalyst.
- Complete manual screen-reader, large-text, keyboard, contrast, and reduced-motion checks.
- Verify current Apple App Store and Google Play policy for the external voluntary project-support link before submission.
- Prepare signing identities/credentials outside Git.
- Build and inspect signed release packages on appropriately provisioned hosts.
- Complete store listing screenshots/data-safety/privacy disclosures.
- Resolve or make an explicit final release decision for the tracked SQLitePCLRaw advisory.
- Obtain a fresh fully green exact-head CI/CodeQL/dependency-audit verification after current hardening changes.

## Deferred to later versions

- Cloud synchronization.
- Remote caregiver collaboration or silent background sharing.
- Accounts, mobile-number authentication, or server-side storage.
- Medical interpretation, diagnosis, treatment advice, interaction checking, or clinical risk scoring.
- Any analytics/telemetry until explicit consent and a privacy review exist.

## Environment limitation

The local execution container used for repository assembly does not include the `dotnet` command or MAUI workloads. Local restore, formatting, compilation, emulator/device smoke tests, signing, and store packaging therefore cannot be truthfully claimed as executed inside that container. GitHub-hosted CI is the authoritative automated build/test verification surface for this delivery.

Manual device checks, signing, and store packaging remain separate release activities and are not marked complete unless they are actually performed.

See `what_changed.md` for the detailed implementation and verification record.
