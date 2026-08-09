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

## Security dependency status

NuGet audit reports `GHSA-2m69-gcr7-jv3q` for the SQLitePCLRaw native `2.1.11` package resolved by the current `sqlite-net-pcl` dependency chain.

An attempted `2.1.12` bundle pin was rejected because that version is not available on NuGet.org. The repository therefore does **not** claim this advisory is fixed. Instead:

- the exact advisory URL is temporarily suppressed through `NuGetAuditSuppress` so unrelated compile/test failures remain visible;
- no wildcard or severity-wide audit suppression is used;
- `docs/security/DEPENDENCY_RISK_REGISTER.md` records the open risk, mitigation context and review trigger;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` defines the upgrade/provider-migration regression gate;
- final production release review must upgrade/replace the dependency path or explicitly block release until the risk is acceptably resolved.

## Automated verification completed

Funding-enabled verification PR #16 exercised source head `2b8f97525ea8d3b41bf62e20d76e1cc224dab102`. The verification marker branch was not merged into `main`.

GitHub Actions CareNest CI run #87 (`31301203981`) completed successfully:

- Unit tests: 15 passed, 0 failed, 0 skipped.
- Integration tests: 11 passed, 0 failed, 0 skipped.
- UI-contract tests: 10 passed, 0 failed, 0 skipped.
- Total tests: 36 passed, 0 failed, 0 skipped.
- Android Release build: passed.
- Windows Release build: passed.
- iOS simulator Release build: passed.
- Mac Catalyst Release build: passed.

CodeQL run #86 (`31301203985`) also completed successfully.

PR #16 was closed after verification succeeded because it contained only a verification marker and did not need to be merged.

## Current

- Product runtime source at funding-enabled verified head `2b8f97525ea8d3b41bf62e20d76e1cc224dab102` has a fully green automated test/build/security-analysis matrix.
- Later BMC vector/support documentation/release-preparation commits do not alter CareNest medical, reminder, persistence, encryption, or scheduling behavior.
- Because the new SVG resides in the MAUI image resource tree, the exact final packaging commit should receive a fresh platform build before signed/public release.
- Final `1.0.0` tagging remains intentionally blocked on the manual release checklist, current store-policy review for the external funding link, signing/store preparation, final exact-commit CI/CodeQL evidence, and an explicit resolution/release decision for the tracked SQLite dependency advisory.

## Next work prepared

- `docs/releases/NEXT_STEPS.md` contains the ordered release path.
- `build/scripts/release-preflight.sh` and `build/scripts/release-preflight.ps1` provide repeatable preflight checks on a fully provisioned host.
- `docs/releases/MANUAL_TEST_MATRIX.md` defines device/manual functional, reminder, privacy and accessibility evidence.
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md` defines packaging/store/funding-link review gates.
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` defines the dependency upgrade/replacement test strategy.
- `BUY_ME_A_COFFEE.md` and `docs/SUPPORT_CARENEST.md` expose the highlighted clickable BMC support artwork.

## Deferred to later versions

- Cloud synchronization.
- Remote caregiver collaboration or silent background sharing.
- Accounts, mobile-number authentication, or server-side storage.
- Medical interpretation, diagnosis, treatment advice, interaction checking, or clinical risk scoring.
- Any analytics/telemetry until explicit consent and a privacy review exist.

## Environment limitation

The local execution container used for repository assembly does not include the `dotnet` command or MAUI workloads. Local restore, formatting, compilation, emulator/device smoke tests, signing, and store packaging therefore cannot be truthfully claimed as executed inside that container. GitHub-hosted CI is the authoritative automated build/test verification surface for this delivery.

Manual device checks, `dotnet format --verify-no-changes` on a fully provisioned development host, signing, and store packaging remain separate release activities and are not marked complete unless they are actually performed.

See `what_changed.md` for the implementation and verification record.
