# Changelog

All notable CareNest changes are recorded here using Keep a Changelog-style categories and semantic-versioning intent.

The exact pre-documentation-completion changelog is preserved at:

`docs/history/pre-complete-docs-20260814/CHANGELOG.md`

Historical evidence is retained rather than rewritten. This active changelog records the current authoritative state and the latest continuation.

## [Unreleased] - 2026-08-15

### Added — store/package release-readiness controls

- Added `CareNestShowFundingLink`, defaulting to `true`, so the voluntary Buy Me a Coffee surface can be hidden for a specific store package without maintaining a source fork.
- Added `AboutViewModel.IsProjectSupportVisible` and bound the complete About support card to it.
- Added a UI/source-policy contract protecting the configurable support-link surface and its voluntary/no-health-entitlement wording.
- Added `PackageMetadataContractTests` covering CareNest app identity/version shape, target frameworks/minimum OS declarations, Android local-first permission/backup/cleartext posture, Apple purpose strings/transport posture, Windows package metadata and required brand assets.
- Added `RepositoryLocator.PathOf` for reusable repository-path assertions in source-policy tests.
- Added `docs/releases/STORE_BUILD_POLICY.md` with enabled/disabled package commands, per-store policy-decision rules and release evidence fields.
- Added `docs/releases/PACKAGED_RELEASE_VALIDATION.md` with source freeze, package identity/checksum, packaged SQLite/encrypted-data, reminder, accessibility, store-policy, signing and final-tag evidence procedures.

### Changed — release preflight

- Bash and PowerShell release-preflight scripts now read `CARENEST_SHOW_FUNDING_LINK` with default `true`.
- Accepted values are fail-closed to `true`/`false`; invalid values stop preflight.
- Optional MAUI restore/build receives `CareNestShowFundingLink`, making a store-specific funding-link package reproducible.

### Changed — release evidence boundary

- PR #56 remains valid authoritative evidence for its frozen 2026-08-14 source boundary, but it is no longer described as exact-head verification of the newer 2026-08-15 `main` source.
- `PROJECT_STATUS.md` and `what_changed.md` now require a new exact-head verification after the store/package hardening stabilizes.
- Current Apple/Google support-link policy review, actual packaged variant inspection, device/accessibility testing, signing and production-tag evidence remain open and are not inferred from source changes.

### Continuation commits — 2026-08-15

- `35690d2f1fbe8bb56d91e718dab688fe4de6cc0d` — `feat: make voluntary funding link store-configurable`;
- `7ccea4ff5367b3c4e94b156f989799d91d6f52ff` — `test: enforce package metadata and privacy contracts`;
- `1fe68a73aaa41622391d8ff6e53171ca98dce055` — `build: pass store funding policy into release preflight`;
- `0a9d994ea310f00d715684c993ee2d954dc0f081` — `docs: define store-specific funding-link build policy`;
- `fe17e1ad752250d81d502ef7615fc1e652842e47` — `docs: add packaged release validation runbook`;
- `db8536d9de125ae73f895ca1d1d6cbdb4de0ded0` — `docs: record packaged release hardening handoff`;
- `dadcc8f3a6c4098bf9277e647206f75f59e98261` — `docs: align project status with new source boundary`.

## [Unreleased] - 2026-08-14

### Added — complete project documentation

- `docs/COMPLETE_PROJECT_DOCUMENTATION.md` as the canonical end-to-end project reference covering product identity, scope, architecture, data, reminders, security/privacy, encryption, backup, setup, testing, release and documentation map.
- `docs/CODEBASE_REFERENCE.md` with concrete Shared/Domain/Application/Infrastructure/MAUI/test project and file responsibilities.
- `docs/CONFIGURATION_REFERENCE.md` documenting current central package versions, build/analyzer policy, NuGet audit behavior, target frameworks, MAUI target isolation, workflows, Git identity, secrets and provenance.
- `docs/MAINTENANCE_AND_OPERATIONS.md` covering routine maintenance, triage, dependency/schema/crypto/reminder changes, documentation, exact-head verification, release, signing, hotfix and incident operations.
- `docs/releases/DOCUMENTATION_AUDIT_20260814.md` recording the repository-wide documentation inventory and separating documentation completeness from production release completeness.
- Exact historical snapshots under `docs/history/pre-complete-docs-20260814/` before replacing stale active security/threat/setup/architecture/notification/documentation-standard/changelog/handoff files.

### Changed — documentation hub and public project entry

- Root `README.md` now promotes PR #56 as the authoritative automated release-engineering baseline while retaining PR #54 as historical runtime bug-audit evidence.
- `docs/README.md` now indexes the whole-project, codebase, configuration, maintenance, architecture, privacy/security, testing, release and history documentation set.
- `docs/releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md` now covers source/API/configuration/automation/maintainer documentation and the PR #56 285-test baseline.
- `docs/DOCUMENTATION_STANDARDS.md` now defines current evidence wording, historical preservation, PR #56 verification citation, SQLite remediation wording, reminder reconciliation language and production/manual evidence rules.
- `docs/setup/DEVELOPMENT.md` now uses the current toolchain/package/release-script/PR #56 source truth and repository-local Git identity behavior.
- `docs/architecture/ARCHITECTURE.md` now describes current reminder cross-surface compensation, SQLite/provider state, document/backup security, platform boundaries and exact-tag release architecture.
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md` now documents effective snooze due time, cancellation-first actions, medicine/profile/appointment compensation and the real Android/Windows/Apple release matrix.
- `docs/security/SECURITY_MODEL.md` and `docs/security/THREAT_MODEL.md` now use PR #56 as the active automated baseline and no longer say a final post-PR #55 verification is pending.

### Added — release workflow policy contracts

Release-engineering hardening added source-policy tests covering:

- `v*` production tag/manual workflow entry points;
- Dependency Audit event safety for pull-request-only metadata;
- Release Evidence source provenance and run-attempt identity;
- independent unit/integration/UI/dependency/workspace evidence capture;
- failure-preserving evidence artifact upload before aggregate failure;
- release evidence retention expectations;
- blocking release-preflight NuGet audit behavior;
- clean-checkout local quality-gate behavior;
- PowerShell native command failure handling;
- repository-local Git setup identity/root/failure behavior;
- Release Gate nested unchecked checklist/open-risk matching and required security/evidence surfaces.

These contracts increased the authoritative UI-contract/policy test count from 100 at PR #54 to 124 at PR #56.

### Changed — exact production-tag verification

Production tags matching `v*` are configured to run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

A tag is not production approval until all required automated and manual/device/accessibility/store/signing/package compatibility gates have completed successfully.

### Changed — Release Evidence

CareNest Release Evidence now records and/or retains:

- exact source commit/ref;
- GitHub Actions run ID;
- run attempt;
- .NET toolchain information;
- tracked-source file manifest;
- tracked-source SHA-256 checksums;
- unit test TRX;
- integration test TRX;
- UI-contract/policy TRX;
- transitive dependency inventories;
- workspace integrity state;
- evidence checksums.

Evidence components are attempted independently. Available evidence is uploaded with failure-preserving behavior before the final aggregate success/failure gate.

Artifact identity contains source/run/attempt information so reruns do not become ambiguous.

### Changed — local quality/release scripts

- Bash and PowerShell release-preflight scripts treat unsuppressed NuGet audit failure as blocking.
- Selected MAUI target dependency graphs are audited before optional release target builds.
- Bash/PowerShell quality-gate scripts run clean-checkout-safe core test/audit sequences.
- PowerShell scripts explicitly fail on required native command errors.
- Git setup scripts locate the repository root, use repository-local configuration, set `Sanskar` / `sanskarin@outlook.in`, verify the values and fail on Git errors.

### Changed — production Release Gate

Release Gate now fails closed for:

- unresolved/open dependency risk state;
- nested unchecked applicable release checklist rows;
- required security/evidence document absence;
- core test failure.

Matching is hardened against ordinary indentation/case variations that previously could make a textual gate fragile.

### Fixed — reminder effective-due behavior

- Future snoozes no longer disappear from upcoming results when the original scheduled instant has passed.
- Overdue snoozes are evaluated from `SnoozedUntilUtc` rather than the stale original time.
- `SnoozedUntilUtc` is the effective due time while a snooze is valid.

### Fixed — stale platform reminder reconciliation

- Existing OS requests are cancelled before replacement, quiet-hours suppression or invalidation.
- Schedule edits retain enough old occurrence identity to cancel stale OS requests before final cleanup.
- Platform cancellation failure remains retryable instead of falsely marking the state reconciled.

### Fixed — reminder action ordering/recovery

Handled Taken/Skipped/Delayed/Missed/Snoozed/Cancelled actions now use cancellation-first ordering:

1. cancel old platform request;
2. persist handled state only after cancellation succeeds;
3. for snooze, schedule the replacement after state persistence;
4. if later essential persistence/scheduling fails, restore previous state and attempt non-cancelled rebuild;
5. aggregate recovery failure instead of claiming consistency.

### Fixed — medicine/profile reminder compensation

- Future platform requests are cancelled before medicine/profile database cascade deletion.
- If persistence fails after platform cancellation, a non-cancelled rebuild compensation is attempted for records that still exist.
- Save flows reconcile reminders before later non-critical audit bookkeeping can make an already-applied primary change appear failed.

### Fixed — appointment reminder persistence

- Appointment database/platform reminder state is reconciled/compensated around persistence failures.
- Appointment deletion cancels the platform request before record deletion.
- `StartsUtc` continues to require actual UTC; local/unspecified ticks are rejected rather than relabeled.
- Background appointment rebuild does not repeatedly prompt for notification permission and does not schedule while permission remains denied.

### Fixed — report cache lifecycle

Application-owned shared report cache files are removed after successful share handoff where CareNest still owns the temporary copy.

External copies already controlled by another application/location remain outside CareNest’s deletion guarantee.

### Security — SQLite dependency remediation

The former `GHSA-2m69-gcr7-jv3q` source exception is remediated.

Current verified package intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- central transitive pinning enabled;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- selected SQLitePCLRaw providers `2.1.12`;
- former exact advisory `NuGetAuditSuppress` removed.

`SqliteDependencySecurityContractTests` prevents silent restoration of the old package/suppression baseline.

Source remediation is complete; representative packaged existing-database/encrypted-data compatibility remains a separate manual production gate.

### Security — encryption/backup/document protections retained

- New encrypted document/backup streams use chunked AES-256-GCM framing v2 with authenticated terminal state.
- Legacy framing v1 remains readable for compatibility.
- Trailing bytes after v2 terminal are rejected.
- Strict decrypted backup archive topology is validated before extraction.
- Missing/corrupt document master key with existing ciphertext fails closed rather than silently creating a replacement key.
- Known mutable application-owned key/verifier/salt/crypto buffers are cleared where practical.
- Document import and backup restore use compensating cleanup/rollback across independent state surfaces.

### Security — local-first/logging boundaries retained

- No CareNest account/backend/cloud-sync requirement was introduced.
- No hidden runtime analytics/telemetry client was introduced.
- Logging privacy contracts continue to prevent routine health content/secrets/raw sensitive exception data from leaking into normal application logs.
- Voluntary project support remains an explicit external browser action and is separate from health behavior/access.

## Verification — PR #56 authoritative release-engineering baseline

PR #56: `Verify complete CareNest release-engineering source`.

Frozen source/base:

`4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`

Marker head:

`e3bc621cea05364a69abee0dadbd71a67c17bddb`

Evidence:

- CareNest CI #571 / `31770929379`: **success**;
- formatting: **success**;
- UnitTests: **122 passed, 0 failed, 0 skipped**;
- IntegrationTests: **39 passed, 0 failed, 0 skipped**;
- UiTests/source-policy: **124 passed, 0 failed, 0 skipped**;
- total core tests: **285 passed, 0 failed, 0 skipped**;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- CodeQL #571 / `31770929382`: **success**;
- unsuppressed Dependency Audit #41 / `31770929383`: **success**.

PR #56 was closed without merge. Its marker is not part of `main`.

PR #54 remains the historical authoritative runtime bug-audit baseline. PR #55 remains superseded intermediate release-engineering evidence.

## Documentation-completion commits — 2026-08-14

The documentation completion pass added or aligned the following logical commits on `main`:

- `06f6ae6968d01e272ab1c0b37190442df867c637` — `docs: add complete project documentation`;
- `2796e2852c659f88e64666c7894c13cc08cda2e1` — `docs: promote PR56 in root README`;
- `20649ff30bc1fb8b8c6321d725e492209e1dae9` — `docs: add complete codebase reference`;
- `37a179aaa2ad3d9a7ac944712cacb2e0d01a0183` — `docs: add configuration and automation reference`;
- `d7ca9b8400caf20ac506a9bfb81c8c3d58bc5da7` — `docs: add maintenance and operations manual`;
- `198c8355348aaee76c30781d51214ae355e1dae9` — `docs: record complete documentation audit`;
- `332f95610c80000c7f5f3ae01074877fb438cab6` — `docs: complete documentation hub index`;
- `22116ebc4057d1eab33fb123593072b17c7bb115` — `docs: complete documentation checklist for PR56`;
- `e7a7dde60a710ffc1fe25ce28a15aad1b72f0e3d` — `docs: preserve pre-completion documentation snapshots`;
- `7a783dd7f9edf15e2f0f0b9943d7289c209f051c` — `docs: finalize current security architecture`;
- `d30d707e2fbcf7e98bd2372cf6b3865debd41bd6` — `docs: finalize current threat model`;
- `24b621c114cf877d603c315bcd64b9e9e9c8d301` — `docs: finalize PR56 development setup`;
- `998d03f784b6ec85d18991596df45012c89b4d79` — `docs: finalize current architecture reference`;
- `04cb7563949ba4a9f5d8cac46c08a84d94c844bd` — `docs: finalize notification platform behavior`;
- `fb07250ab61d9ddcdb1760c862dd231d49100107` — `docs: finalize documentation standards`.

GitHub commit metadata for this continuation uses `Sanskar <sanskarin@outlook.in>`.

## Production work intentionally still open

Documentation completeness and PR #56 automation do not complete:

- new exact-head verification for the 2026-08-15 source/test/release-script boundary;
- real Android/Windows/iOS/iPadOS/Mac Catalyst manual matrices;
- actual notification permission/delivery/recovery behavior;
- Android alarm/battery/reboot/time/time-zone checks;
- packaged SQLite existing-data upgrade/integrity/readability;
- encrypted document/backup historical compatibility;
- clean-install restore;
- accessibility testing;
- current Apple/Google store-policy review;
- packaged verification of the selected funding-link visibility per store;
- signing credentials/configuration outside Git;
- signed artifact generation/inspection;
- store listing/screenshots/privacy/data-safety metadata;
- exact final production tag Release Gate/Release Evidence;
- final version/build/checksums/publication.

These remain release-blocking until actual evidence exists.

## [1.0.0-rc.1] - 2026-08-09

### Added

- Complete local-first CareNest project structure and first RC implementation.
- Profiles, medicine records, schedules, reminder occurrences and medication log.
- Appointments, encrypted document organization, stock/refill tracking and reports.
- Password-encrypted manual backup/restore and portable encrypted-document recovery.
- App lock, notification diagnostics, accessibility/theme settings and developer tools.
- Android, iOS, Mac Catalyst and Windows platform integration structure.
- Security, privacy, threat-model, setup, release and contribution documentation.
- Automated unit/integration/source-contract tests and GitHub Actions.
- Voluntary project-support link and support metadata.

### Safety

- Non-diagnostic/non-treatment boundaries included across onboarding, About, reports and documentation.
- Reminder/stock limitations surfaced instead of silently inferred.
- Project support explicitly separated from medical advice, reminder behavior, emergency assistance and access to local data.

## Historical changelog

The complete detailed changelog that existed before the current documentation-completion alignment—including 2026-08-12, 2026-08-13, earlier 2026-08-14 runtime/security work and historical dependency statements—is preserved byte-for-byte at:

`docs/history/pre-complete-docs-20260814/CHANGELOG.md`

Use that file together with `what_changed.md`, `docs/history/`, and dated verification documents when investigating historical source/evidence states.
