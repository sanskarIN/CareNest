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
- GitHub Actions CI, CodeQL, Dependency Audit, Release Gate, Release Evidence, Dependabot, release checklist, troubleshooting and contribution documentation.
- Branding vector sources and store guidance.
- Initial release implementation merged to `main` through PR #3.
- SQLite result-producing PRAGMAs for WAL mode, busy timeout and WAL checkpoint are handled correctly through scalar reads.
- WAL-backed backup snapshot regression coverage is included.
- MAUI per-target CI restore/build isolation is implemented without propagating app target frameworks into referenced `net10.0` projects.
- Android notification integration has explicit API-level guards and nullability checks.
- Apple verification uses a macOS 26 runner compatible with the current .NET 10 Apple workload.
- Voluntary Buy Me a Coffee project-support link is centralized at `https://buymeacoffee.com/sanskarIN` and exposed through the About/support surfaces and GitHub funding metadata.
- Custom CareNest BMC vector artwork and original compact support-badge artwork are version-controlled.
- Clickable visual support pages are available at `BUY_ME_A_COFFEE.md`, `SUPPORT.md`, `README.md`, `docs/SUPPORT_CARENEST.md`, and the in-app About page.
- Release preflight scripts are available for Bash and PowerShell.
- Manual cross-platform/device test matrix and store-submission checklist are documented.
- SQLite dependency migration/verification plan is documented for the open advisory path.
- Platform-neutral formatting verification is part of the core CI job.
- Repository policy tests enforce no TODO/FIXME/NotImplemented placeholders in committed runtime source, no runtime network/telemetry client introduction, no clinical decision feature-name regressions, no common signing/secret files, and presence of required governance/release files.
- Architecture contract tests enforce Shared/Domain/Application/Infrastructure dependency direction and keep MAUI isolated to the app composition project.
- ViewModel contract tests enforce no direct SQLite/network-client access from concrete ViewModels and preserve the notification-permission and as-needed reminder boundaries.
- Data-model contract tests cover all entities required by the CareNest master prompt and preserve medicine strength/instruction values as opaque text.
- Branding/localization contract tests validate the adaptive icon, splash, BMC artwork, English safety resource keys, and highlighted support destination.
- Original monochrome, light-surface, and dark-surface CareNest mark variants are present for system/brand surfaces.
- Runtime asynchronous-policy tests prevent common synchronous task-blocking patterns.
- Global exception observation is registered once at app startup.
- Global/UI/startup/reminder error logging records only safe operational metadata such as exception type names, not full exception messages, stack traces, health-record identifiers, or user-entered health content.
- `docs/security/LOGGING_PRIVACY.md` documents and automated tests enforce the diagnostic redaction boundary.
- `docs/releases/QUALITY_GATE.md`, `SECURITY_RELEASE_REVIEW.md`, `RELEASE_EVIDENCE.md`, `RELEASE_NOTES_TEMPLATE.md`, and `VERIFICATION_BRANCH_PROTOCOL.md` define reproducible promotion/evidence requirements.
- An earlier recovery-history audit restored every valid BMC/dependency/release-gate file that existed in the previously green source baseline.

## Security dependency status

NuGet audit reports `GHSA-2m69-gcr7-jv3q` for the SQLitePCLRaw native `2.1.11` package resolved by the current `sqlite-net-pcl` dependency chain.

An attempted `2.1.12` bundle pin was rejected because that version is not available on NuGet.org. The repository therefore does **not** claim this advisory is fixed. Instead:

- the exact advisory URL is temporarily suppressed through `NuGetAuditSuppress` so unrelated compile/test failures remain visible;
- no wildcard or severity-wide audit suppression is used;
- `docs/security/DEPENDENCY_RISK_REGISTER.md` records the open risk, mitigation context and review trigger;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` defines the upgrade/provider-migration regression gate;
- final production release review must upgrade/replace the dependency path or explicitly block release until the risk is acceptably resolved.

## Current fully verified source head

Exact production source head verified through PR #27:

`8417513db36c72b0ec2cfaccadb6ac47ba361f11`

Verification marker head:

`aefd53869b7eaf54815de446fc83373c7977d04d`

The marker changed only `build/verification/rc1-hardening-20260810-4.txt`. PR #27 was closed without merge after the full matrix succeeded.

Automated evidence:

- CareNest CI run #200 / `31375336226`: **success**.
- Platform-neutral formatting gate: **success**.
- Unit tests: **15 passed, 0 failed, 0 skipped**.
- Integration tests: **11 passed, 0 failed, 0 skipped**.
- UI-contract/policy tests: **46 passed, 0 failed, 0 skipped**.
- Total automated test cases in the core job: **72 passed, 0 failed, 0 skipped**.
- Android Release build: **success**.
- Windows Release build: **success**.
- iOS simulator Release build: **success**.
- Mac Catalyst Release build: **success**.
- CodeQL run #200 / `31375336083`: **success**.
- Dependency Audit run #7 / `31375336088`: **success**.

The final source verification followed three superseded verification passes that intentionally exposed and corrected real issues instead of weakening the gates:

- PR #24 / CI #175: found CA1873 eager exception-metadata logging and CA1861 test-allocation analyzer failures; CodeQL succeeded.
- PR #25 / CI #190: formatting, unit and integration tests passed; Dependency Audit #5 and CodeQL #190 passed; UI-contract execution found path-normalization, generated-file scanning and existing StartupCoordinator exception-object logging issues; MAUI builds also confirmed explicit logger-level guards were required.
- PR #26 / CI #198: Dependency Audit #6 and CodeQL #198 passed; formatting, unit and integration tests passed; UI compilation found one remaining nullable project-reference filename contract error.
- PR #27 / CI #200: all automated gates passed.

Later status/changelog/handoff documentation commits after verified source head `8417513...` do not change runtime/test/product source and are not represented as a separate platform-verification head.

## Release blockers that remain real

- Complete manual device/emulator matrix on Android, Windows, iOS/iPadOS, and Mac Catalyst.
- Complete manual screen-reader, large-text, keyboard, contrast, and reduced-motion checks.
- Verify current Apple App Store and Google Play policy for the external voluntary project-support link before submission.
- Prepare signing identities/credentials outside Git.
- Build and inspect signed release packages on appropriately provisioned hosts.
- Complete store listing screenshots/data-safety/privacy disclosures.
- Resolve or make an explicit final release decision for the tracked SQLitePCLRaw advisory.
- Run the tag/manual `CareNest Release Evidence` workflow for the exact promoted release commit when the production blockers above are cleared.

## Deferred to later versions

- Cloud synchronization.
- Remote caregiver collaboration or silent background sharing.
- Accounts, mobile-number authentication, or server-side storage.
- Medical interpretation, diagnosis, treatment advice, interaction checking, or clinical risk scoring.
- Any analytics/telemetry until explicit consent and a privacy review exist.

## Environment limitation

The local execution container used for repository assembly does not include the `dotnet` command or MAUI workloads. Local restore, formatting, compilation, emulator/device smoke tests, signing, and store packaging therefore cannot be truthfully claimed as executed inside that container. GitHub-hosted CI is the authoritative automated build/test verification surface for this delivery.

Manual device checks, accessibility checks, signing, store-policy review and store packaging remain separate release activities and are not marked complete unless they are actually performed.

See `what_changed.md` for the detailed implementation, recovery, hardening and verification record.
