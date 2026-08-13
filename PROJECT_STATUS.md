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
- Medicine schedule validation covers explicit interval/start-time rules, selected weekdays, cycle on/off values, date ordering, clock ranges, recognized schedule enum values, supported weekday-mask bits, and trimmed/valid time-zone identifiers.
- Reminder planning validates profile → medicine → schedule → persisted schedule-time ownership before materializing occurrences, while allowing intentionally unbound editor times before persistence.
- Archived profiles are suppressed defensively inside the planner in addition to the coordinator's archive filter.
- Reminder planning windows require actual UTC `DateTime` values and remain half-open (`fromUtc` inclusive, `toUtc` exclusive).
- Reminder coordinator rebuild overrides require UTC, and snooze actions require an explicit future UTC timestamp before persistence/platform scheduling.
- Reminder planner tests cover daily, selected-weekday, cycle, custom date range, every-N-hours, follow-up, disabled, archived-profile, paused, completed, archived-medicine, and as-needed behavior.
- Deterministic property-style recurrence coverage uses a fixed seed and verifies arbitrary half-open windows, stable uniqueness/order, all supported weekday masks, cycle matrices, and representative every-N-hours intervals.
- DST gap/overlap coverage exercises representative North America, Europe, Australia, and New Zealand zones when available on the test host; invalid local times are not replaced with invented reminder times and ambiguous times remain deterministic.
- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` records the deterministic non-clinical scheduling, ownership, UTC, snooze, state, window, and DST contracts.
- App-lock verification clears both derived and retrieved verifier buffers after checks; contract tests protect salted PBKDF2-HMAC-SHA256, fixed-time comparison, no plaintext PIN persistence, verifier clearing, lock-material deletion, and PIN policy.
- Security/threat-model documentation explicitly describes app lock as a local privacy barrier rather than whole-database/device encryption and records residual weak-PIN/device-compromise risk.
- Complete documentation hub exists at `docs/README.md` with user, feature, architecture, database, service-boundary, application-flow, notification/platform, encrypted document-vault, backup/restore, reports/exports, privacy, security, accessibility, localization, setup, troubleshooting, maintainer, testing, release, glossary, and documentation-governance references.
- Existing architecture/schema/design/development/troubleshooting/localization/store/data-lifecycle/contribution documents have been expanded into full references rather than left as short placeholders/overviews.
- `docs/DOCUMENTATION_STANDARDS.md` defines implementation-evidence, safety, privacy, dependency-risk, manual-evidence, and documentation-only-source-baseline rules.
- `docs/releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md` inventories the complete documentation set while explicitly preserving manual/store/signing/dependency/release blockers as incomplete operational work.

## Service and appointment hardening completed

- Appointment domain validation now requires `StartsUtc.Kind == DateTimeKind.Utc` and validates a trimmed time-zone identifier separately.
- Appointment reminder scheduling no longer uses `DateTime.SpecifyKind` to reinterpret local/unspecified clock ticks as UTC.
- An appointment save that requests notification permission and remains denied does not attempt platform scheduling.
- Appointment reminder rebuild does not repeatedly prompt for permission and does not schedule while permission is denied.
- A stored appointment with non-UTC `StartsUtc` fails closed during reminder rebuild.
- Direct application-service tests cover profile create/update/delete coordination, medicine create/update/schedule/stock/delete behavior, appointment save/permission/rebuild/delete behavior, document import/export/delete/rollback behavior, and backup-reminder scheduling/permission behavior.
- Reusable repository/time/reminder/notification/document-store test doubles provide deterministic platform-neutral service tests.

## Document-vault hardening completed

- Document import now performs compensating cleanup across the encrypted payload and SQLite metadata path.
- Metadata-save failure removes the newly created encrypted payload.
- Audit failure after metadata save attempts rollback of both the metadata record and encrypted payload.
- Rollback cleanup uses non-cancelled cleanup attempts and incomplete cleanup is surfaced explicitly.
- Explicit document export constrains the temporary filename to a safe leaf name.
- Caller-owned document master-key copies are zeroed after import/export where managed-memory control permits.
- An invalid retrieved key copy is cleared; a newly generated key buffer is cleared if secure-store persistence fails.
- New encrypted documents record stream encryption version **2**.

## Backup and cryptographic hardening completed

- New chunked encrypted document/backup payload streams use framing **version 2** with an authenticated terminal record.
- Data-chunk AAD binds counter and plaintext length; the terminal tag binds the next counter and zero length.
- V2 readers reject chunk-boundary prefix truncation and trailing bytes after terminal.
- Legacy chunked framing version 1 remains readable for backward compatibility; existing v1 ciphertext is not represented as retroactively upgraded.
- The decrypted backup ZIP topology is allowlisted before extraction.
- Duplicate, unexpected, nested, non-`.cndoc`, manifest-count-mismatched, and invalid document-key archive layouts are rejected.
- Backup extraction retains full-path containment checks as defense in depth.
- Backup password-derived AES key/salt buffers are cleared after crypto paths where practical.
- Document master-key copies used during backup creation/restore are cleared after use where practical.
- Chunked AEAD work buffers are cleared where managed-memory control permits.
- Integration tests cover v2 multi-chunk round-trip, prefix truncation rejection, trailing-data rejection, legacy v1 decryption, strict backup topology, document/backup key-buffer hygiene, and new document encryption metadata.

## Security dependency status

NuGet audit reports `GHSA-2m69-gcr7-jv3q` for the SQLitePCLRaw native `2.1.11` package resolved by the current `sqlite-net-pcl` dependency chain.

The repository does **not** claim this advisory is fixed. Instead:

- the exact advisory URL is temporarily suppressed through `NuGetAuditSuppress` so unrelated compile/test failures remain visible;
- no wildcard or severity-wide audit suppression is used;
- `docs/security/DEPENDENCY_RISK_REGISTER.md` records the open risk, mitigation context and review trigger;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` defines the upgrade/provider-migration regression gate;
- final production release review must upgrade/replace the dependency path or explicitly block release until the risk is acceptably resolved.

## Current fully verified source head

Exact runtime/test source head verified through PR #33:

`4f5f9abe9d702fa33d6aba3f15c113febfebf95e`

Verification marker head:

`62a0050a2622e12a31d00842778af0bc96355482`

The marker changed only `build/verification/rc1-aead-v2-hardening-20260813.txt`. PR #33 was closed without merge after the full matrix succeeded.

Automated evidence:

- CareNest CI run #332 / `31691592300`: **success**.
- Platform-neutral formatting gate: **success**.
- Unit tests: **106 passed, 0 failed, 0 skipped**.
- Integration tests: **30 passed, 0 failed, 0 skipped**.
- UI-contract/policy tests: **54 passed, 0 failed, 0 skipped**.
- Total automated test cases in the core job: **190 passed, 0 failed, 0 skipped**.
- Android Release build: **success**.
- Windows Release build: **success**.
- iOS simulator Release build: **success**.
- Mac Catalyst Release build: **success**.
- CodeQL run #332 / `31691592435`: **success**.
- Dependency Audit run #13 / `31691592302`: **success**.

## Current verification sequence

- PR #31 was intentionally superseded after formatting passed but unit-test compilation exposed CA1861 in a newly added profile-service assertion. The analyzer finding was fixed on `main` rather than suppressed; PR #31 was closed without merge.
- PR #32 verified corrected service/document/backup hardening source `8a28bbf30692b2b0e98ec801dac1531d50d65db1` with 106 unit + 26 integration + 54 UI = 186 core tests, all four platform builds, CodeQL #326, and Dependency Audit #12 green.
- Later authenticated-stream-v2 source changes required a new exact-head run rather than reusing PR #32 evidence.
- PR #33 verified source `4f5f9abe...` with 190 core tests and all platform/security/dependency gates green, then closed without merging its marker.

Documentation-only commits after source head `4f5f9abe...` do not change runtime/test/product source and therefore do not replace the PR #33 runtime baseline.

## Documentation status

The current documentation set covers:

- end-user behavior and limitations;
- complete feature reference;
- architecture, service boundaries, application flows, database schema;
- appointment/notification UTC and permission behavior;
- encrypted document vault including v2 framing/rollback/key hygiene;
- data storage/export/deletion;
- encrypted backup/restore including strict topology/v1-v2 compatibility;
- reports/exports;
- privacy model/data lifecycle;
- security architecture/threat model/logging/dependency risk;
- design system/accessibility/localization/store assets;
- development/platform setup/troubleshooting/maintainer operations;
- testing strategy/reminder contract;
- release process/quality/security/manual/store/evidence/verification procedures;
- terminology, contribution rules, support/funding, and documentation maintenance standards.

Documentation completeness does not mean production release readiness. Operational/manual blockers below remain real.

## Release blockers that remain real

- Complete manual device/emulator matrix on Android, Windows, iOS/iPadOS, and Mac Catalyst.
- Complete manual screen-reader, large-text, keyboard, contrast, and reduced-motion checks.
- Manually verify notification permission denied/granted, appointment permission behavior, Android exact-alarm/battery/reboot/time/time-zone behavior, and real-device reminder delivery limitations.
- Manually verify new v2 encrypted document and backup workflows on packaged targets.
- Verify legacy v1 encrypted document/backup compatibility using canonical historical fixtures when available before any decision to drop v1 support.
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

The local execution container used for repository assembly does not include the `dotnet` command or MAUI workloads. Local restore, formatting, compilation, emulator/device smoke tests, signing, and store packaging therefore cannot be truthfully claimed as executed inside that container. GitHub-hosted CI is the authoritative automated build/test verification surface for the runtime/test baseline.

Manual device checks, accessibility checks, signing, store-policy review and store packaging remain separate release activities and are not marked complete unless they are actually performed.

See `docs/README.md` for the documentation hub and `what_changed.md` for the detailed implementation, recovery, hardening, documentation, and verification record.
