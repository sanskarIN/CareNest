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
- Buy Me a Coffee voluntary project-support URL is centralized as `https://buymeacoffee.com/sanskarIN`.
- The About page exposes a dedicated voluntary project-support action.
- `.github/FUNDING.yml` exposes the same URL through GitHub funding metadata.
- README, support, privacy, terms, security, threat-model, data-lifecycle, changelog, store-guidance, release-checklist, and UI-contract surfaces document the voluntary/non-medical funding boundary.
- `docs/releases/NEXT_STEPS.md` tracks production-release blockers, store/signing work, release promotion tasks, post-release quality work, and separately reviewed future-version ideas.

## Security dependency status

NuGet audit reports `GHSA-2m69-gcr7-jv3q` for the SQLitePCLRaw native `2.1.11` package resolved by the current `sqlite-net-pcl` dependency chain.

An attempted `2.1.12` bundle pin was rejected because that version is not available on NuGet.org. The repository therefore does **not** claim this advisory is fixed. Instead:

- the exact advisory URL is temporarily suppressed through `NuGetAuditSuppress` so unrelated compile/test failures remain visible;
- no wildcard or severity-wide audit suppression is used;
- `docs/security/DEPENDENCY_RISK_REGISTER.md` records the open risk, mitigation context and review trigger;
- `docs/releases/NEXT_STEPS.md` Priority 0 item 1 tracks the required upgrade/removal/re-verification work;
- final production release review must upgrade or replace the dependency path when an available compatible patched package exists, or record an explicit release decision after security review.

## Latest automated verification completed

Verification PR #16 exercised source head:

`2b8f97525ea8d3b41bf62e20d76e1cc224dab102`

The verification marker branch was not merged into `main`. PR #16 was closed after verification succeeded.

GitHub Actions CareNest CI run #87 (`31301203981`) completed successfully:

- Unit tests: 15 passed, 0 failed, 0 skipped.
- Integration tests: 11 passed, 0 failed, 0 skipped.
- UI-contract tests: 10 passed, 0 failed, 0 skipped.
- Total automated tests: 36 passed, 0 failed, 0 skipped.
- Android Release build: passed.
- Windows Release build: passed.
- iOS simulator Release build: passed.
- Mac Catalyst Release build: passed.

CodeQL run #86 (`31301203985`) also completed successfully.

This fresh pass verified the funding-support runtime/UI changes, shared funding URL constant, About-page command, GitHub funding metadata, funding-link UI-contract tests, and all existing core/platform behavior present at that source head.

## Funding boundary

CareNest project funding is optional. The Buy Me a Coffee action does not unlock medical advice, premium health behavior, different reminder scheduling/delivery, emergency assistance, support priority, or access to local health data.

The support page is an external third-party service opened only after explicit user action. CareNest does not automatically send local profiles, medicine data, documents, backups, app-lock data, or reminder history to the funding provider.

Before store submission, current Apple and Google rules for external voluntary funding/payment links must be reviewed because store policies can change. That policy review is explicitly tracked in the release checklist and `docs/releases/NEXT_STEPS.md`.

## Current

- Complete CareNest `1.0.0-rc.1` product source is on `main`.
- Source head `2b8f97525ea8d3b41bf62e20d76e1cc224dab102` has a fully green automated core/platform/CodeQL matrix.
- Later `main` commits after that source head are documentation/security/status guidance updates only and do not alter product runtime behavior.
- Final `1.0.0` tagging remains intentionally blocked on the manual release checklist, the tracked SQLite dependency advisory decision/resolution, current store-policy review for the funding link, signing, final store/privacy preparation, and exact-final-commit verification if later runtime/UI/dependency changes occur.

## Deferred to later versions

- Cloud synchronization.
- Remote caregiver collaboration or silent background sharing.
- Accounts, mobile-number authentication, or server-side storage.
- Medical interpretation, diagnosis, treatment advice, interaction checking, or clinical risk scoring.
- Any analytics/telemetry until explicit consent and a privacy review exist.

## Environment limitation

The local execution container used for repository assembly does not include the `dotnet` command or MAUI workloads. Local restore, formatting, compilation, emulator/device smoke tests, signing, and store packaging therefore cannot be truthfully claimed as executed inside that container. GitHub-hosted CI is the authoritative automated build/test verification surface for this delivery.

Manual device checks, `dotnet format --verify-no-changes` on a fully provisioned development host, signing, and store packaging remain separate release activities and are not marked complete unless they are actually performed.

See `what_changed.md` for the implementation and verification record and `docs/releases/NEXT_STEPS.md` for the ordered next-step roadmap.
