# Release Checklist

## Automated verification evidence

Authoritative verification PR: `#59`  
Frozen source/base SHA: `8489d19734d6142054156d5b57f2713195c16b65`  
Verification marker head: `ca58294fb7f7a56ee87da16d938f0f691c3a3c7e`  
CareNest CI run: `#622` / `31869214132`  
Store Package Configuration run: `#11` / `31869214047`  
CodeQL run: `#622` / `31869214042`  
Dependency Audit run: `#44` / `31869214093`

Completed evidence:

- [x] Platform-neutral `dotnet format --verify-no-changes` gate.
- [x] Unit tests — **122 passed, 0 failed, 0 skipped**.
- [x] Integration tests — **39 passed, 0 failed, 0 skipped**.
- [x] UI-contract/policy tests — **149 passed, 0 failed, 0 skipped**.
- [x] Total automated test cases in the core job — **310 passed, 0 failed, 0 skipped**.
- [x] Default Android Release build.
- [x] Default Windows Release build.
- [x] Default iOS simulator Release build.
- [x] Default Mac Catalyst Release build.
- [x] Store-safe Android Release build with `CareNestShowFundingLink=false`.
- [x] Store-safe Windows Release build with `CareNestShowFundingLink=false`.
- [x] Store-safe iOS simulator Release build with `CareNestShowFundingLink=false`.
- [x] Store-safe Mac Catalyst Release build with `CareNestShowFundingLink=false`.
- [x] Bash store-package preflight executable-mode guard.
- [x] CodeQL analysis.
- [x] Unsuppressed Dependency Audit, including the Android MAUI application graph.
- [x] Exact-tag/manual release workflow entry-point contracts.
- [x] Store-package workflow/preflight forced-false/target/non-publication contracts.
- [x] Failure-preserving Release Evidence provenance/rerun-identity contracts.
- [x] Blocking local preflight/quality dependency-audit contracts.
- [x] Repository-local Git identity setup contracts.
- [x] Fail-closed Release Gate contracts.

PR #59 is a verification-only branch containing only `build/verification/store-safe-package-final-20260815.txt` beyond its frozen source boundary. It was closed without merge after the full matrix completed. The marker is not part of `main`.

`docs/releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md` records the authoritative current automated evidence. PR #58 remains historical packaged-release/store-policy hardening evidence, PR #56 remains historical release-engineering evidence, and PR #54 remains the historical authoritative runtime bug-audit baseline.

The verified source includes all previous repository/privacy/architecture/reminder/snapshot/app-lock/service/document/backup/AEAD-v2 hardening plus:

- app-lock multi-key rollback/fail-closed corrupt-material handling;
- document-master-key fail-closed behavior;
- plaintext export/cache cleanup;
- profile photo staging/cleanup synchronization;
- onboarding rollback;
- SQLite migration/repository transaction boundaries;
- non-reentrant ViewModel refresh/input validation;
- Android `GoAsync()` receiver lifetime correction;
- Windows reminder timer ownership/race correction;
- backup completion/rollback exact-state semantics;
- CSV formula-like text neutralization;
- atomic CSV/PDF/JSON report generation;
- DST-gap/overflow/max-date reminder-planner fixes;
- startup recovery isolation;
- effective-due snooze handling;
- OS-reminder cancellation before replacement/suppression/invalidation;
- medicine/profile delete compensation and save-time reconciliation;
- appointment reminder persistence compensation;
- cancellation-first handled reminder actions with state/rebuild recovery;
- notification scheduling/cancellation failure-injection tests;
- analyzer-safe reconciliation expectations;
- shared report-cache cleanup after share handoff;
- SQLite native/provider package remediation;
- removal of the tracked SQLite NuGet audit suppression;
- dependency-security regression contract protecting the patched package floor;
- exact `v*` tag execution for CareNest CI, CodeQL, Dependency Audit, CareNest Store Package Configuration, Release Gate and Release Evidence;
- failure-preserving Release Evidence with tracked-source provenance/checksums and rerun-safe artifact identity;
- blocking unsuppressed dependency audit in local quality/preflight scripts;
- fail-closed repository-local Git identity setup;
- hardened production Release Gate matching;
- build-configurable voluntary external funding surface;
- package metadata/privacy regression contracts;
- fail-closed store-package Bash/PowerShell wrappers that require an explicit supported target and force `CARENEST_SHOW_FUNDING_LINK=false`;
- executable Git mode for the Bash store-package wrapper plus CI executable-mode verification;
- dedicated four-platform store-safe Release compilation with `CareNestShowFundingLink=false`;
- source-policy contracts preventing store-safe workflow/preflight drift;
- dated 2026-08-15 Apple/Google external support-link policy review and conservative store package decision.

Automated green status does not substitute for manual device, signing, accessibility, notification-delivery, submission-time store-policy re-review, encrypted-data compatibility, installed package inspection, or packaged existing-database checks.

## Verification hardening sequence

The repository uses marker-only exact-head verification cycles that expose defects instead of weakening quality gates.

Historical sequence includes:

- PR #24 / CI #175: found CA1873 eager logger-argument evaluation and CA1861 test-allocation analyzer failures; CodeQL succeeded.
- PR #25 / CI #190: formatting, unit and integration tests passed; Dependency Audit #5 and CodeQL #190 passed; UI-contract execution exposed project-reference path normalization, generated-file scanning and an existing StartupCoordinator exception-object logging issue; MAUI compile also confirmed explicit logger-level guards were required.
- PR #26 / CI #198: Dependency Audit #6 and CodeQL #198 passed; formatting, unit and integration tests passed; UI compilation found one remaining nullable project-reference filename contract error.
- PR #27 / CI #200: all automated gates passed for the privacy/policy hardening baseline.
- PR #28 / CI #220: all automated gates passed after reminder schedule/DST/window hardening, WAL snapshot integrity/cancellation coverage, and app-lock verifier clearing/security contracts were added.
- PR #29 / CI #246: marker-only verification exposed CA2263 in new schedule-kind validation; source fixed without weakening analyzer policy.
- PR #30 / CI #248: all automated gates passed on the corrected reminder-integrity source.
- PR #31: service/document/backup hardening exposed CA1861 in a new test assertion; source fixed, no suppression.
- PR #32 / CI #326: corrected service/document/backup hardening passed.
- PR #33 / CI #332: authenticated-stream-v2 source passed with 190 core tests and all platform/security/dependency gates.
- PR #37: exposed CA1068 in a new transaction-helper cancellation-token signature; source fixed.
- PR #39: exposed CA1001 in profile-photo synchronization ownership plus a formatter defect; source fixed, and the accidentally merged failed marker was explicitly removed.
- PR #40: four platform Release builds, CodeQL and Dependency Audit passed; core formatting failed on a final newline, so the checkpoint was not promoted.
- PR #41 and #42: intentionally superseded while reminder/behavior audit source was still changing.
- PR #43 / CareNest CI #448: **not green**; integration tests failed and the UI suite was skipped even though platform builds, CodeQL and Dependency Audit passed.
- PR #44: reproduced future-snooze, overdue-snooze and stale-future-occurrence defects; source fixed.
- PR #46: exposed broader platform-reminder reconciliation lifecycle failures; source fixed.
- PR #47: unsuppressed SQLite Dependency Audit #28 / `31765223239` succeeded, but source moved afterward.
- PR #48: unsuppressed Dependency Audit #29 and CodeQL succeeded; combined CI exposed a transient moving-base reminder-interface compile mismatch; source corrected/simplified.
- PR #49: exposed CA1861 in new reminder-reconciliation assertions; tests corrected rather than suppressing the analyzer.
- PR #50: unsuppressed SQLite Dependency Audit #31 succeeded, but its source predated later analyzer-safe tests.
- PR #51/#52: superseded as later runtime/test source changed.
- PR #53: duplicate final bug-audit verification; all required groups ultimately completed successfully.
- PR #54 / CI #503: authoritative completed runtime bug-audit baseline; 261/261 core tests, all four Release builds, CodeQL, and unsuppressed Dependency Audit succeeded.
- PR #55: first release-engineering checkpoint; 277/277 core tests, Android, Windows, CodeQL and unsuppressed Dependency Audit succeeded before further confirmed release-tooling/documentation fixes superseded it.
- PR #56 / CI #571: historical release-engineering baseline; 285/285 core tests, all four default Release builds, CodeQL, and unsuppressed Dependency Audit succeeded.
- PR #58 / CI #608: historical packaged-release/store-policy hardening baseline; 291/291 core tests, all four default Release builds, CodeQL, and unsuppressed Dependency Audit succeeded.
- PR #59 / CI #622: **authoritative current automated baseline**; 310/310 core tests, all four default Release builds, all four funding-disabled store-safe Release builds, CodeQL, and unsuppressed Dependency Audit succeeded.

No failed/superseded verification marker is final release evidence, and no verification marker file is intended to enter `main`.

## Release-preparation additions now present

- custom scalable CareNest Buy Me a Coffee vector artwork and original compact support badge;
- clickable support surfaces in README, SUPPORT, in-app About, `BUY_ME_A_COFFEE.md`, and `docs/SUPPORT_CARENEST.md` for builds where the external support surface is enabled;
- build-configurable funding surface through `CareNestShowFundingLink`;
- Bash and PowerShell release-preflight scripts;
- Bash and PowerShell local quality-gate scripts;
- Bash and PowerShell fail-closed store-package preflight scripts;
- executable-mode guard for the Bash store-package preflight;
- dedicated CareNest Store Package Configuration workflow;
- repository-local Git identity setup scripts;
- manual device test matrix;
- packaged release validation runbook;
- store build policy;
- dated store support-link policy review;
- store-safe configuration exact-source verification evidence;
- store submission checklist;
- SQLite dependency migration plan;
- privacy-safe structured bug report form;
- Dependency Audit workflow;
- production Release Gate workflow;
- Release Evidence workflow with source provenance/failure preservation/rerun identity;
- exact `v*` tag execution for all required release workflows, including Store Package Configuration;
- logging privacy contract;
- deterministic reminder scheduling contract;
- complete testing guide and test plan;
- production quality gate;
- security release-review checklist;
- release-notes template;
- exact-head verification-branch protocol;
- automated repository/architecture/ViewModel/data-model/branding/async/logging/app-lock/reminder/service/backup/crypto/dependency/release-policy/store-package coverage;
- original light, dark, and monochrome CareNest mark variants.

## Release preparation and manual verification

### Automated/preflight

- [ ] Decide final `1.0.0` version/build metadata and release date.
- [ ] Run `build/scripts/release-preflight.sh` or `build/scripts/release-preflight.ps1` on a fully provisioned development host for the intended normal configuration where applicable.
- [ ] Run `build/scripts/store-package-preflight.sh` or `build/scripts/store-package-preflight.ps1` for each intended store-safe target on a fully provisioned development host where applicable.
- [x] Platform-neutral `dotnet format --verify-no-changes` succeeds on the authoritative PR #59 source boundary.
- [x] Required project restores used by completed automated tests/platform builds succeed on GitHub-hosted runners.
- [x] Unsuppressed NuGet dependency vulnerability audit succeeds on PR #59.
- [x] Automated repository policy confirms no release-blocking implementation markers in committed runtime source.
- [x] CareNest CI is fully green for the current exact source boundary.
- [x] CareNest Store Package Configuration is fully green for the current exact source boundary.
- [x] CodeQL is green for the current exact source boundary.
- [x] Dependency Audit is green for the current exact source boundary.
- [x] Release workflow/script/Git setup/store-package/Release Gate contracts are green in the 149-test UI-contract suite.
- [x] PR #59 marker-only exact-head verification is completed and recorded in `STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md`.
- [ ] Re-run the complete automated matrix if runtime/test/workflow/package/project/platform/build-script source changes before production promotion.
- [ ] Run the exact production `v*` tag and require CareNest CI, CodeQL, Dependency Audit, CareNest Store Package Configuration, Release Gate and Release Evidence all green.
- [ ] Record the final Release Evidence artifact/checksums for the exact production tag.

### Automated reminder, appointment, service, snapshot and app-lock coverage

- [x] Every-N-hours requires explicit valid interval and exactly one explicit starting time.
- [x] Selected-weekday schedules require at least one explicit selected day.
- [x] Selected-weekday masks reject unsupported bits outside the seven weekday positions.
- [x] Unknown schedule enum values are rejected.
- [x] Blank/unknown time-zone identifiers are rejected and valid identifiers are trimmed before lookup.
- [x] Cycle schedules require explicit positive on/off day values.
- [x] Schedule end-before-start and out-of-range clock times are rejected.
- [x] As-needed schedules create no automatic occurrences.
- [x] Archived profiles, paused/completed/archived medicines and disabled schedules create no automatic occurrences.
- [x] Planner validates profile → medicine → schedule → persisted schedule-time ownership before materialization.
- [x] Custom/schedule/medicine date boundaries are enforced.
- [x] Planning window inputs must be UTC and half-open windows include `fromUtc` while excluding `toUtc`.
- [x] Reminder rebuild overrides require UTC.
- [x] Snooze actions require an explicit future UTC timestamp before persistence/platform scheduling.
- [x] Snoozed rows use their explicit snooze due time for upcoming/overdue classification.
- [x] Duplicate explicit times are deduplicated by stable occurrence identity.
- [x] Out-of-order explicit times return chronologically ordered occurrences.
- [x] DST-invalid spring-forward local times do not cause an invented alternate reminder time.
- [x] DST-overlap local times produce a deterministic occurrence.
- [x] DST gap/overlap matrix covers representative North America, Europe, Australia and New Zealand zones when available on the test host.
- [x] Deterministic fixed-seed property tests validate random planning windows, cycle matrices, all supported weekday masks, uniqueness/order, and representative every-N-hours spacing.
- [x] Rebuild cancels existing platform requests before replacement/suppression/invalidation.
- [x] Schedule changes retain stale occurrence IDs until platform cancellation/reconciliation can occur.
- [x] Medicine/profile deletes cancel future platform requests before database cascade and compensate on cascade failure.
- [x] Medicine/profile saves reconcile reminders before non-critical audit bookkeeping.
- [x] Reminder actions cancel the old platform request before handled-state persistence and use non-cancelled recovery when later work fails.
- [x] Notification scheduling/cancellation failure injection covers action/reconciliation recovery paths.
- [x] Appointment `StartsUtc` rejects local/unspecified kinds instead of relabeling them as UTC.
- [x] Appointment time-zone identifiers are trimmed/validated.
- [x] Appointment save does not schedule when notification permission remains denied.
- [x] Appointment rebuild does not prompt/schedule while permission is denied.
- [x] Appointment reminder persistence compensation is covered.
- [x] Direct service tests cover profile, medicine, appointment, document and backup-reminder orchestration.
- [x] WAL snapshots contain committed profile data and pass SQLite integrity check.
- [x] Pre-cancelled WAL snapshot requests throw cancellation and leave no output file.
- [x] App-lock verifier uses salted PBKDF2-HMAC-SHA256 and fixed-time comparison.
- [x] Derived and retrieved verifier buffers are cleared after verification paths.
- [x] Plaintext PIN persistence is rejected by source contracts; disabling app lock removes stored lock material.

### Automated document/backup cryptographic coverage

- [x] Document import database-save failure removes the encrypted payload.
- [x] Document audit failure after metadata save rolls back both metadata and encrypted payload.
- [x] Incomplete document rollback is surfaced explicitly.
- [x] Document master-key caller-owned copies are cleared after import/export where practical.
- [x] New encrypted documents record stream format version 2.
- [x] Chunked AEAD v2 multi-chunk round-trip passes.
- [x] V2 authenticated terminal rejects chunk-boundary prefix truncation.
- [x] Trailing data after terminal is rejected.
- [x] Legacy framing-v1 stream remains decryptable.
- [x] Backup creation/restore clears caller-owned document-key copies where practical.
- [x] Backup password-derived key/salt buffers are cleared after crypto paths where practical.
- [x] Strict backup topology rejects duplicate entries.
- [x] Strict backup topology rejects unexpected/nested/non-`.cndoc` entries.
- [x] Strict backup topology validates manifest document count and document-key length/presence.
- [x] Restore retains path-containment validation after topology validation.
- [x] Report generation uses staged/atomic final-file semantics.
- [x] CSV formula-like user strings are neutralized in portable spreadsheet output.
- [x] Application-owned shared report cache files are cleaned after successful share handoff where CareNest still owns them.

### SQLite dependency source remediation

- [x] Central transitive pinning remains enabled.
- [x] `SQLitePCLRaw.lib.e_sqlite3` is pinned to `3.53.3`.
- [x] `SQLitePCLRaw.lib.e_sqlite3.android` is pinned to `2.1.12`.
- [x] Selected SQLitePCLRaw providers are pinned to `2.1.12`.
- [x] The exact `GHSA-2m69-gcr7-jv3q` `NuGetAuditSuppress` entry is removed.
- [x] `SqliteDependencySecurityContractTests` protects the package floor and suppression absence.
- [x] Unsuppressed Dependency Audit #44 / `31869214093` succeeds on authoritative PR #59.
- [x] All 310 automated tests, four default platform Release builds, and four funding-disabled store-safe Release builds succeed on the same verified source boundary.
- [ ] Representative packaged upgrade/install with fictional pre-remediation data is manually verified.
- [ ] Existing structured records are manually verified after the package update.
- [ ] Existing encrypted document and backup workflows are manually verified on packaged targets.
- [ ] Reminder rebuild/reconciliation is manually verified after package upgrade.

### Core product behavior

- [ ] Complete applicable rows in `docs/releases/MANUAL_TEST_MATRIX.md`.
- [ ] Manual onboarding smoke test.
- [ ] Create/edit/delete profiles on real/emulated target devices.
- [ ] Create/pause/resume/complete/archive medicine schedules.
- [ ] Verify daily, selected-weekday, every-N-hours, cycle/custom-range and as-needed behaviors on supported targets.
- [ ] Verify notification permission denied and granted flows.
- [ ] Verify appointment permission-denied/granted reminder flow on supported targets.
- [ ] Verify Android battery/exact-alarm diagnostics on a device/appropriate emulator.
- [ ] Verify reboot/time/time-zone rebuild behavior on applicable platforms.
- [ ] Verify stored schedule intent is not silently rewritten after a time-zone change.
- [ ] Mark taken/skipped/delayed/missed and edit medication log.
- [ ] Verify snooze rejects invalid/past clock values and behaves correctly with real platform notification scheduling.
- [ ] Verify cancellation-first reminder actions behave correctly with real platform notification scheduling and recovery/restart.
- [ ] Verify quiet hours and follow-up reminder behavior.
- [ ] Import/export/delete new v2 encrypted documents.
- [ ] Verify retained legacy v1 encrypted-document compatibility using a canonical historical fixture when available.
- [ ] Create appointment and calendar export.
- [ ] Export CSV, JSON and PDF reports; verify disclaimers/privacy boundaries.
- [ ] Create new v2 encrypted backup; restore on clean data; reject wrong password and tampered backup.
- [ ] Verify retained legacy v1 backup compatibility using a canonical historical fixture when available.
- [ ] Enable/disable app lock and verify cold-start lock on target devices.
- [ ] Verify local reset/profile deletion destructive confirmations and expected cleanup.

### Accessibility and presentation

- [ ] Large-text/manual scaling checks.
- [ ] Screen-reader traversal and accessible names.
- [ ] Keyboard navigation on applicable desktop targets.
- [ ] Reduced-motion preference checks.
- [ ] Light/dark/system theme checks.
- [ ] Confirm error/validation text remains readable and actionable.
- [ ] Confirm color is not the only status/validation signal.
- [ ] Validate app icon/splash/store screenshots using fictional data only.
- [ ] Verify light/dark/monochrome CareNest brand assets on intended system/store surfaces.

### Privacy/security

- [x] Automated logging policy prevents full exception-object logger calls in committed runtime source.
- [x] Global, UI, startup and reminder exception paths log only safe metadata such as exception type names.
- [x] Automated policy checks reject common signing/secret files from the committed workspace.
- [x] Automated policy checks reject runtime network/telemetry client introduction for the local-first v1 scope.
- [x] Automated policy checks reject named diagnosis/dosage/treatment/interaction/risk-scoring feature regressions.
- [x] App-lock security contract protects salted PBKDF2-HMAC-SHA256, fixed-time verification, verifier-buffer clearing, no plaintext PIN persistence, and lock-material removal.
- [x] Planner ownership checks fail closed rather than materializing reminder occurrences under mismatched local entities.
- [x] New encrypted stream v2 terminal/truncation/trailing-data behavior is integration tested.
- [x] Strict backup topology is integration tested.
- [x] Caller-owned key-buffer hygiene has integration coverage.
- [x] SQLite dependency source remediation removes the tracked advisory audit suppression and guards the maintained native/provider floor.
- [x] Release workflow/script/security-policy/store-package contracts pass in PR #59.
- [ ] Confirm on target devices that no document content, backup passwords, plaintext PINs, sensitive notes or private file paths appear in device/platform logs.
- [ ] Confirm export/share operations occur only after explicit user action.
- [ ] Confirm no CareNest account/backend/network requirement appears in normal local-first flows.
- [ ] Review `docs/security/THREAT_MODEL.md` for the exact public-release candidate.
- [ ] Review `docs/security/LOGGING_PRIVACY.md` for the exact public-release candidate.
- [ ] Complete `docs/releases/SECURITY_RELEASE_REVIEW.md`.
- [ ] Review `docs/security/DEPENDENCY_RISK_REGISTER.md`.
- [ ] Review `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` and record packaged existing-data compatibility evidence.
- [ ] Review third-party notices and licenses.

### Buy Me a Coffee / funding link

Project-support URL:

`https://buymeacoffee.com/sanskarIN`

- [x] URL centralized in CareNest shared constants.
- [x] About/support action exists for configurations where the support surface is enabled.
- [x] In-app support artwork is clickable when enabled.
- [x] GitHub funding metadata exists.
- [x] Custom vector project-support artwork exists.
- [x] Original compact CareNest support badge exists.
- [x] Clickable README/SUPPORT/root/documentation support pages exist.
- [x] Support is documented as voluntary and not a CareNest feature entitlement or medical service.
- [x] `CareNestShowFundingLink=false` hides the complete in-app support card without changing organizer functionality.
- [x] Current Apple App Store external gift/support guidance was reviewed on 2026-08-15.
- [x] Current Google Play tip/contribution guidance was reviewed on 2026-08-15.
- [x] The current dated review/conservative decision is recorded in `STORE_POLICY_REVIEW_20260815.md`.
- [x] Initial Apple App Store/Google Play source configuration is conservatively selected as `CareNestShowFundingLink=false` unless submission-time policy clearly permits the link.
- [x] PR #59 compiles Android, Windows, iOS simulator and Mac Catalyst Release source with `CareNestShowFundingLink=false`.
- [ ] Re-review the current rules for external funding/tipping/donation links on every store/distribution channel at actual submission time.
- [ ] Build the actual Apple App Store candidate with the selected store-safe setting.
- [ ] Build the actual Google Play candidate with the selected store-safe setting.
- [ ] Inspect installed packaged builds and confirm the BMC image/button/URL/card is absent where disabled.
- [ ] Confirm repository/legal/support surfaces remain available in the store-safe package.
- [ ] Confirm on packaged builds that no CareNest health data is sent merely by displaying/opening the external funding link in configurations where it is enabled.
- [ ] Confirm custom badge is not represented as official Buy Me a Coffee brand artwork.

### Signing and distribution

- [ ] Complete `docs/releases/STORE_SUBMISSION_CHECKLIST.md` for every intended store/channel.
- [ ] Sign packages using secrets/certificates/profiles stored outside the repository.
- [ ] Verify final package IDs/bundle IDs/publisher identities.
- [ ] Verify selected `CareNestShowFundingLink` value on each actual packaged artifact.
- [ ] Verify store privacy/data-safety disclosures match the shipping runtime behavior.
- [ ] Verify support/privacy/terms/security URLs and contacts in final listings.
- [ ] Record exact source commit SHA for each signed package.
- [ ] Record package SHA-256/checksum where the artifact is directly handled.
- [ ] Record signing/notarization/store provenance without committing secrets.
- [ ] Create the exact approved production `v*` tag.
- [ ] Require tagged CareNest CI to succeed.
- [ ] Require tagged CodeQL to succeed.
- [ ] Require tagged Dependency Audit to succeed.
- [ ] Require tagged CareNest Store Package Configuration to succeed.
- [ ] Require tagged Release Gate to succeed.
- [ ] Require tagged Release Evidence to succeed and record its artifact/checksums.
- [ ] Generate final release notes from `docs/releases/RELEASE_NOTES_TEMPLATE.md`.
- [ ] Create/publish the final GitHub/store release only after all applicable gates above are satisfied.

## Release rule

Do not tag/publish/promote a final `1.0.0` build while a required automated gate for the exact production commit/tag is failing or incomplete, while required manual checks are incomplete, while submission-time store-policy re-review for the voluntary support link is unresolved, while signing/store identity is unfinished, while actual packaged funding-link visibility has not been inspected, while packaged SQLite existing-data compatibility has not been manually evidenced, or before exact-tag Release Gate/Release Evidence exists.

The PR #59 automated RC1 source baseline is fully green for both normal/default and funding-disabled store-safe configurations. Automated green status is necessary but not sufficient for public release.