# Changelog

All notable changes follow Keep a Changelog principles and semantic versioning.

## [Unreleased] - 2026-08-13

### Added

- Reusable platform-neutral test doubles for repository, deterministic time, reminder coordination, notifications, and encrypted document storage.
- Direct `ProfileService` tests for create/update audits, UTC touch timestamps, cascading encrypted-document/profile-photo cleanup, and deletion audit behavior.
- Direct `MedicineService` tests for create/update audits, reminder rebuilds, schedule persistence/future-occurrence invalidation, explicit stock adjustments, negative estimated-stock rejection, and cascade deletion.
- Direct `AppointmentService` tests for UTC scheduling, denied/granted notification permission behavior, rebuild behavior without permission prompts, non-UTC stored-data rejection, and reminder cancellation/deletion.
- Direct `DocumentService` tests for encrypted import metadata, save/audit failure rollback, safe temporary export filenames, export audits, and idempotent deletion.
- Direct `BackupReminderCoordinator` tests for disabled reminders, denied permission behavior, no background permission prompts, current/last-backup scheduling, overdue recovery, and sound/vibration preferences.
- `BackupArchiveValidator` with strict allowlisted backup ZIP topology checks.
- Integration coverage for duplicate/unexpected/nested/non-`.cndoc` backup entries, manifest count mismatches, invalid/missing document-key material, and invalid schema/document-count metadata.
- Integration coverage that caller-owned document-master-key copies are cleared after document/backup cryptographic operations where managed-memory control permits.
- Direct `ChunkedAead` integration tests for version-2 multi-chunk round-trip, authenticated-terminal prefix-truncation rejection, trailing-data rejection, and legacy version-1 read compatibility.

### Changed

- Appointment start timestamps now require explicit `DateTimeKind.Utc`; local/unspecified values are rejected instead of being relabeled with `DateTime.SpecifyKind`.
- Appointment time-zone identifiers are trimmed and validated separately from the explicit UTC start instant.
- Appointment save-time notification scheduling now stops when a permission request remains denied.
- Appointment reminder rebuild does not prompt for notification permission and does not schedule while permission remains denied.
- Document import now uses compensating rollback across SQLite metadata and encrypted payload storage when save/audit steps fail.
- Rollback cleanup for a failed document import uses non-cancelled cleanup attempts so a cancelled main operation does not knowingly strand the newly created artifacts.
- Encrypted document master-key copies are zeroed after import/export paths where application-owned mutable buffers are available.
- Newly generated document-key material is cleared if secret-store persistence fails.
- Backup creation/restore now clears caller-owned document-key copies, password-derived AES key material, and salt buffers after use where practical.
- Backup inspection/restore validates strict archive topology before extraction; path-containment validation remains as defense in depth.
- New encrypted document and backup payload streams use chunked AEAD framing **version 2** with an authenticated terminal record.
- The chunked encrypted-stream reader rejects bytes after the terminal record.
- Existing chunked framing version 1 remains decryptable for backward compatibility; historical v1 ciphertext is not represented as retroactively upgraded.
- Newly imported encrypted document metadata records encryption stream version `2`.
- Shared chunked AEAD working buffers are cleared where managed-memory control permits.

### Fixed

- Removed silent appointment clock-kind reinterpretation that could turn local/unspecified ticks into a different UTC reminder instant.
- Prevented appointment services from attempting platform scheduling after notification permission remains denied.
- Prevented normal document-import audit failures from leaving a database record pointing to an encrypted payload that rollback already removed.
- Added explicit aggregate failure reporting when document import rollback itself cannot fully clean both local persistence surfaces.
- Prevented decrypted backup archives with duplicate, unexpected, nested, or manifest-inconsistent entries from reaching the extraction/replacement stage.
- Prevented new chunked encrypted streams from accepting a chunk-boundary authenticated prefix as a complete stream through an unauthenticated terminator; v2 now authenticates termination against the next chunk counter and zero length.
- Corrected CA1861 exposed by verification PR #31 in a newly added profile-service test assertion instead of suppressing the analyzer.

### Security

- New encrypted-stream framing v2 authenticates the terminal record and binds it to the next chunk counter/zero plaintext length.
- V2 rejects prefix truncation and trailing data while retaining v1 decryption compatibility for existing data.
- Backup topology is allowlisted to `manifest.json`, `database/carenest.db`, optional/required `secrets/document-master-key.bin`, and top-level `documents/*.cndoc` files.
- Document-bearing backups require a valid 32-byte document master key before restore proceeds.
- Known mutable caller-owned verifier/key/salt/nonce/AAD/tag/plain/cipher buffers are cleared after use where practical; this is not represented as erasure of every runtime/OS/secure-store copy.
- `GHSA-2m69-gcr7-jv3q` remains explicitly open for the SQLitePCLRaw `2.1.11` dependency path. Successful Dependency Audit runs do not claim remediation.

### Verification

Latest exact runtime/test source head verified:

`4f5f9abe9d702fa33d6aba3f15c113febfebf95e`

Verification PR #33 used marker head `62a0050a2622e12a31d00842778af0bc96355482`, changed only `build/verification/rc1-aead-v2-hardening-20260813.txt`, and was closed without merge after success.

- CareNest CI #332 / `31691592300`: success.
- Platform-neutral formatting: success.
- Unit tests: **106 passed, 0 failed, 0 skipped**.
- Integration tests: **30 passed, 0 failed, 0 skipped**.
- UI-contract/policy tests: **54 passed, 0 failed, 0 skipped**.
- Total core automated tests: **190 passed, 0 failed, 0 skipped**.
- Android Release: success.
- Windows Release: success.
- iOS simulator Release: success.
- Mac Catalyst Release: success.
- CodeQL #332 / `31691592435`: success.
- Dependency Audit #13 / `31691592302`: success.

Verification sequence for this continuation:

- PR #31 verified source `8e2607f287ca5777d9edbab445042f96c6bcfcec`; formatting passed, but unit-test compilation exposed CA1861 in a new constant-array assertion. The test was fixed on `main`, PR #31 was closed without merge, and the analyzer was not suppressed.
- PR #32 verified corrected source `8a28bbf30692b2b0e98ec801dac1531d50d65db1` with 106 unit + 26 integration + 54 UI = 186 tests, all four platform builds, CodeQL #326, and Dependency Audit #12 green.
- Later authenticated-stream-v2 source changes required new exact-head PR #33 instead of reusing PR #32 evidence.
- PR #33 is the current exact automated baseline.

Public production promotion remains blocked on manual device/accessibility/notification/document/backup checks, current store-policy review, signing/store preparation, final Release Evidence for the exact promoted commit, and an explicit decision/resolution for the open SQLite dependency risk.

## [Unreleased] - 2026-08-12

### Documentation

- Added `docs/README.md` as the canonical CareNest documentation hub.
- Added complete end-user documentation in `docs/USER_GUIDE.md` and feature-by-feature behavior/contracts in `docs/FEATURE_REFERENCE.md`.
- Added `docs/REPORTS_AND_EXPORTS.md` for JSON/PDF/CSV/document/calendar export semantics and privacy boundaries.
- Added `docs/GLOSSARY.md` for shared product/engineering terminology.
- Added end-to-end application-flow, service-boundary, data-storage/export, encrypted backup/restore, encrypted document-vault, and platform-notification architecture references under `docs/architecture/`.
- Expanded `docs/architecture/ARCHITECTURE.md` and `DATABASE_SCHEMA.md` from concise overviews into complete architecture/schema/WAL/migration references.
- Added `docs/privacy/PRIVACY_MODEL.md` and expanded `docs/privacy/DATA_LIFECYCLE.md` to document local-first storage, outbound boundaries, OS/external-copy limitations, backup/export/deletion behavior, and future-network review requirements.
- Added `docs/security/SECURITY_MODEL.md` to consolidate trust boundaries, SQLite/document/backup/app-lock protection, logging, dependency, CI, secret-management, and residual-risk rules.
- Added `docs/design/ACCESSIBILITY.md`; expanded the design system, localization architecture, and store-asset guidance with accessibility, responsive-layout, safety wording, localization/RTL, screenshot, privacy, and distribution requirements.
- Expanded `docs/setup/DEVELOPMENT.md` and `TROUBLESHOOTING.md`, and added `PLATFORM_SETUP.md` plus `MAINTAINER_OPERATIONS.md` for Android/Windows/iOS/Mac Catalyst setup, Git identity, CI, dependency, signing, troubleshooting, and release operations.
- Added `docs/testing/TESTING_GUIDE.md` covering the unit/integration/UI-contract suites, current 141-test verified baseline, formatting, reminder/property/DST/WAL/app-lock/security contracts, platform builds, CodeQL, Dependency Audit, and manual testing.
- Added `docs/releases/RELEASE_PROCESS.md` covering the complete production release lifecycle from scope freeze through exact-head verification, manual matrix, accessibility, store-policy review, signing, Release Evidence, tagging, and hotfixes.
- Added `docs/DOCUMENTATION_STANDARDS.md` and `docs/releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md` to define documentation evidence/maintenance rules and inventory the completed documentation package without marking manual production work complete.
- Expanded `CONTRIBUTING.md` with architecture, medical-safety, privacy, security, reminder, schema, dependency, test, accessibility, documentation, and exact-head verification contribution rules.
- Updated root `README.md`, `PROJECT_STATUS.md`, and `DECISIONS.md` to link/record the complete documentation package while keeping the exact runtime/test source baseline at `c61f3c31...`.
- The documentation pass is intentionally Markdown-only and does not claim a new runtime/platform verification baseline beyond PR #30.

### Added

- Platform-neutral `dotnet format --verify-no-changes` verification in the core CI job.
- Repository policy tests covering implementation placeholders, local-first network/telemetry boundaries, clinical-decision feature-name regressions, common signing/secret files, and required governance/release artifacts.
- Architecture dependency tests enforcing Shared → Domain → Application → Infrastructure boundaries while keeping MAUI isolated to the application composition project.
- ViewModel contract tests preventing direct SQLite/network-client access and protecting onboarding notification-permission and as-needed reminder rules.
- Data-model contract tests for every entity required by the CareNest master prompt and for opaque medicine strength/instruction storage.
- Branding/localization contract tests for app icon, splash, BMC support artwork, safety resource keys, and clickable support surfaces.
- Original monochrome, light-surface, and dark-surface CareNest mark variants.
- Privacy-aware global unhandled/unobserved-task exception observation registered once during application startup.
- `docs/security/LOGGING_PRIVACY.md` describing the allowed/prohibited runtime diagnostic boundary.
- `CareNest Release Evidence` workflow that records exact source/ref/toolchain identity, TRX results, transitive dependency inventories, SHA-256 evidence checksums, and an immutable Actions artifact for a manual/tag-triggered release candidate.
- `docs/releases/RELEASE_EVIDENCE.md`, `QUALITY_GATE.md`, `SECURITY_RELEASE_REVIEW.md`, `RELEASE_NOTES_TEMPLATE.md`, and `VERIFICATION_BRANCH_PROTOCOL.md`.
- Automated async-safety contract coverage preventing common synchronous task-blocking patterns in runtime source.
- Expanded medicine-schedule validation tests for explicit intervals/start times, selected weekdays, cycle on/off values, date ordering, clock ranges, recognized schedule enum values, supported weekday-mask bits, and blank/unknown/trimmed time-zone identifiers.
- Expanded reminder-planner tests for cycle schedules, custom/schedule/medicine end boundaries, archived profiles, paused/completed/archived medicines, and daylight-saving gaps.
- Reminder planning boundary tests for half-open UTC windows, UTC-kind validation, duplicate-time deduplication, stable occurrence identity, and chronological ordering.
- Reminder entity-ownership tests covering profile → medicine → schedule → persisted schedule-time relationships plus intentionally unbound editor times.
- Reminder coordinator contract tests protecting UTC rebuild overrides and explicit future-UTC snooze validation.
- Deterministic fixed-seed property-style recurrence tests covering arbitrary half-open windows, cycle on/off matrices, all supported weekday masks, stable uniqueness/order, and representative every-N-hours intervals.
- Representative multi-zone DST gap/overlap coverage for North America, Europe, Australia, and New Zealand when those time-zone identifiers are available on the test host.
- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` documenting deterministic, non-clinical schedule materialization, ownership, UTC, snooze, state, and DST rules.
- WAL snapshot integration coverage that opens the copied database read-only, verifies committed profile content, and executes `PRAGMA integrity_check`.
- Snapshot cancellation regression coverage ensuring a pre-cancelled request creates no output file.
- App-lock security contract coverage for salted PBKDF2-HMAC-SHA256 verification, fixed-time comparison, verifier-buffer clearing, plaintext-PIN non-persistence, stored lock-material removal, and numeric PIN policy.

### Changed

- Restored the previously verified Dependency Audit and production Release Gate workflows after a repository-history recovery audit identified files that had not been replayed.
- Restored the privacy-safe structured bug report form and all previously verified highlighted Buy Me a Coffee README/SUPPORT/About assets and release guidance.
- Expanded required-repository policy checks to include CI, CodeQL, Dependency Audit, Release Gate, Release Evidence, logging privacy, dependency risk, and security/quality review files.
- Repository/logging source-policy scans now ignore generated `bin`/`obj` content so checks apply to committed source instead of generated SDK files.
- Architecture project-reference parsing is separator-normalized for Linux/Windows compatibility and explicitly non-null under nullable analysis.
- App-lock verification now clears both the newly derived verifier and the verifier retrieved from secure storage after a PIN comparison; malformed/missing-salt paths also clear a retrieved verifier before returning.
- Threat-model/security documentation now explicitly treats app lock as a local privacy barrier rather than whole-database/device encryption and records weak-PIN/device-compromise residual risk.
- Reminder planning now rejects inconsistent local entity ownership instead of materializing an occurrence under a mismatched profile, medicine, schedule, or persisted schedule-time relationship.
- Planner windows and coordinator rebuild overrides now require actual UTC `DateTime` values rather than silently reinterpreting local/unspecified clock ticks.
- Snooze actions now require an explicit future UTC timestamp before occurrence persistence or platform notification scheduling.
- Archived profiles are suppressed defensively in the planner as well as in the coordinator's existing archive filter.
- Test-plan, security-review, quality-gate, release-checklist, and roadmap documentation now record the expanded reminder integrity invariants.

### Fixed

- Removed full exception-object logging from `SafeUiErrorService`, `StartupCoordinator`, and reminder scheduling/recovery paths.
- Global, UI, startup, and reminder error logging now uses explicit `ILogger.IsEnabled(...)` guards and logs only safe metadata such as exception type names instead of exception messages, stack traces, health-record identifiers, or user-entered health content.
- Corrected CA1873 eager logger-argument evaluation findings surfaced by GitHub-hosted MAUI builds.
- Corrected CA1861 constant-array allocation guidance in architecture tests.
- Corrected cross-platform project-reference path parsing and a nullable filename return caught by exact-head CI.
- Corrected policy-test false positives caused by generated `obj` global-using files.
- Added regression evidence that invalid spring-forward local schedule times do not cause CareNest to invent a replacement reminder time.
- Added explicit regression evidence that duplicate user-entered clock times do not create duplicate reminder occurrences.
- Corrected CA2263 surfaced by verification PR #29 by replacing the new non-generic `Enum.IsDefined(Type, object)` schedule-kind check with generic `Enum.IsDefined(schedule.Kind)` instead of weakening analyzer policy.

### Verification

Exact production runtime/test source head for that continuation: `c61f3c31c4ba33419c7b348fc8ee63a58eaa637b` through PR #30, later superseded by the 2026-08-13 PR #33 baseline above.

### Security

- `GHSA-2m69-gcr7-jv3q` remains explicitly open for the SQLitePCLRaw `2.1.11` dependency path.
- The exact advisory suppression remains narrowly scoped and is governed by `docs/security/DEPENDENCY_RISK_REGISTER.md` and `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.
- Public production promotion remains blocked on an explicit dependency-risk decision/resolution plus manual device/accessibility/store/signing work and final release evidence.

## [1.0.0-rc.1] - 2026-08-09

### Added

- Complete local-first CareNest application architecture and source tree.
- Profiles, medicine records, schedules, reminder occurrences and medication log.
- Appointments, encrypted document organization, stock/refill tracking and reports.
- Manual password-encrypted backup/restore (format v2), per-profile structured export/delete workflows, and portable encrypted-document recovery.
- App lock, notification diagnostics, accessibility/theme settings and developer tools.
- Android, iOS, Mac Catalyst and Windows platform integration structure.
- Security, privacy, threat-model, setup, release and contribution documentation.
- Automated unit/integration/contract test projects and GitHub Actions workflow.
- Dependency risk register for security advisories that cannot yet be resolved by an available compatible package release.
- Voluntary project-support link for `https://buymeacoffee.com/sanskarIN` in the About page, README/support documentation, and GitHub funding metadata.
- `docs/releases/NEXT_STEPS.md` with production-release blockers, manual device checks, signing/store work, release promotion tasks, and future-version ideas.
- UI-contract coverage that verifies the funding URL and voluntary-support wording stay consistent across runtime and repository support surfaces.

### Fixed

- Corrected SQLite result-producing PRAGMA handling for WAL journal mode, busy timeout, and WAL checkpoint operations so sqlite-net no longer treats returned rows as non-query failures.
- Added regression coverage for WAL mode, busy-timeout configuration, and WAL-backed database snapshot creation used by encrypted backups.
- Corrected MAUI CI target selection so a platform-specific target no longer leaks into referenced `net10.0` projects.
- Added an explicit MAUI Controls package reference and isolated platform source trees to their corresponding target frameworks.
- Corrected startup-page switch typing, nullable schedule-editor values, and redacted reminder-diagnostic timestamp usage.
- Corrected Android time-zone-change intent handling.
- Hardened Android notification scheduling for nullable platform values, pending-intent construction, notification construction, API-level guards, and notification-manager access.
- Updated Apple CI to use a runner/toolchain compatible with the installed .NET 10 iOS and Mac Catalyst workloads.
- Reclassified non-correctness analyzer recommendations so they remain visible without hiding functional build/test failures.
- Centralized repository, creator, funding, and contact values through shared CareNest constants in the About view model.

### Security

- Added a narrowly scoped NuGet audit exception for `GHSA-2m69-gcr7-jv3q` because the current `sqlite-net-pcl` dependency chain resolves SQLitePCLRaw `2.1.11` and the attempted `2.1.12` bundle version is not available on NuGet.org.
- The SQLite advisory remains open and explicitly tracked in `docs/security/DEPENDENCY_RISK_REGISTER.md`; the audit exception is not represented as a vulnerability fix.
- No wildcard or severity-wide NuGet audit suppression was added.
- The external voluntary-support action opens only after explicit user interaction and is documented as an independent third-party service; CareNest does not automatically send health records, documents, backups, or profile data to the funding provider.

### Safety

- Added explicit non-diagnostic/non-treatment medical boundaries throughout onboarding, About, reports and documentation.
- Reminder and stock limitations are surfaced instead of silently inferred.
- Project support is explicitly separated from medical advice, health functionality, reminder behavior, emergency assistance, support priority, and access to local CareNest data.
