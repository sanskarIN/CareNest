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
- WAL-backed backup snapshot regression coverage verifies snapshot creation, committed-data preservation, integrity, and cancellation-before-copy behavior.
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
- Medicine schedule validation covers explicit interval/start-time rules, selected weekdays, cycle on/off values, date ordering, clock ranges, recognized schedule enum values, supported weekday-mask bits, and trimmed/valid time-zone identifiers.
- Reminder planning validates profile → medicine → schedule → persisted schedule-time ownership before materializing occurrences, while allowing intentionally unbound editor times before persistence.
- Archived profiles are suppressed defensively inside the planner in addition to the coordinator's archive filter.
- Reminder planning windows require actual UTC `DateTime` values and remain half-open (`fromUtc` inclusive, `toUtc` exclusive).
- Reminder coordinator rebuild overrides require UTC, and snooze actions require an explicit future UTC timestamp before persistence/platform scheduling.
- Reminder planner tests cover daily, selected-weekday, cycle, custom date range, every-N-hours, follow-up, disabled, archived-profile, paused, completed, archived-medicine, and as-needed behavior.
- Deterministic property-style recurrence coverage uses a fixed seed and verifies arbitrary half-open windows, stable uniqueness/order, all supported weekday masks, cycle matrices, and representative every-N-hours intervals.
- DST gap/overlap coverage now exercises representative North America, Europe, and Australia zones when available on the test host; invalid local times are not replaced with invented reminder times and ambiguous times remain deterministic.
- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` records the deterministic non-clinical scheduling, ownership, UTC, snooze, state, window, and DST contracts.
- App-lock verification clears both derived and retrieved verifier buffers after checks; contract tests protect salted PBKDF2-HMAC-SHA256, fixed-time comparison, no plaintext PIN persistence, verifier clearing, lock-material deletion, and PIN policy.
- Security/threat-model documentation explicitly describes app lock as a local privacy barrier rather than whole-database/device encryption and records residual weak-PIN/device-compromise risk.

## Security dependency status

NuGet audit reports `GHSA-2m69-gcr7-jv3q` for the SQLitePCLRaw native `2.1.11` package resolved by the current `sqlite-net-pcl` dependency chain.

An attempted `2.1.12` bundle pin was rejected because that version is not available on NuGet.org. The repository therefore does **not** claim this advisory is fixed. Instead:

- the exact advisory URL is temporarily suppressed through `NuGetAuditSuppress` so unrelated compile/test failures remain visible;
- no wildcard or severity-wide audit suppression is used;
- `docs/security/DEPENDENCY_RISK_REGISTER.md` records the open risk, mitigation context and review trigger;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` defines the upgrade/provider-migration regression gate;
- final production release review must upgrade/replace the dependency path or explicitly block release until the risk is acceptably resolved.

## Current fully verified source head

Exact source head verified through PR #30:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

Verification marker head:

`59016b7e2b13d5ac1c93cf0db973f275c6e7eb19`

The marker changed only `build/verification/rc1-ownership-utc-dst-hardening-20260810-2.txt`. PR #30 was closed without merge after the full matrix succeeded.

Automated evidence:

- CareNest CI run #248 / `31382194805`: **success**.
- Platform-neutral formatting gate: **success**.
- Unit tests: **74 passed, 0 failed, 0 skipped**.
- Integration tests: **13 passed, 0 failed, 0 skipped**.
- UI-contract/policy tests: **54 passed, 0 failed, 0 skipped**.
- Total automated test cases in the core job: **141 passed, 0 failed, 0 skipped**.
- Android Release build: **success**.
- Windows Release build: **success**.
- iOS simulator Release build: **success**.
- Mac Catalyst Release build: **success**.
- CodeQL run #248 / `31382194687`: **success**.
- Dependency Audit run #10 / `31382194683`: **success**.

PR #29 / source head `04057299fe6d13012734ba235e6fa92604753948` was intentionally superseded after CI #246 exposed analyzer error CA2263 in the newly added non-generic `Enum.IsDefined(Type, object)` call. The quality gate was not weakened: `main` was corrected in commit `c61f3c31c4ba33419c7b348fc8ee63a58eaa637b` to use generic `Enum.IsDefined(schedule.Kind)`, then a new marker-only PR #30 was created from that exact corrected head and passed the complete matrix.

The preceding fully green baseline was PR #28 / source head `69c4dd9319f7dc47edea1786e683f7d90c656e1e`, which passed CI #220, CodeQL #220, Dependency Audit #8, formatting, 37 unit tests, 13 integration tests, 51 UI-contract tests, and all four platform builds. PR #30 supersedes that automated source baseline because it includes the additional ownership/UTC/snooze/DST/property hardening.

Earlier superseded verification PRs #24–#26 intentionally exposed and drove fixes for analyzer, privacy-logging, path-normalization, generated-source scanning, and nullable-contract problems instead of weakening quality gates.

Documentation-only status/changelog/handoff commits after source head `c61f3c31...` do not change the runtime/test source that passed PR #30 and are not represented as separate platform-verification heads.

## Release blockers that remain real

- Complete manual device/emulator matrix on Android, Windows, iOS/iPadOS, and Mac Catalyst.
- Complete manual screen-reader, large-text, keyboard, contrast, and reduced-motion checks.
- Manually verify notification permission denied/granted, Android exact-alarm/battery/reboot/time/time-zone behavior, and real-device reminder delivery limitations.
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
