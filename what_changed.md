# CareNest — Active Complete Continuation Handoff

This file is the current detailed handoff for ongoing CareNest work.

The exact previous `what_changed.md` content that existed before the final documentation-completion pass is preserved byte-for-byte at:

`docs/history/pre-complete-docs-20260814/what_changed.md`

Earlier long-form handoffs are also preserved under `docs/history/`.

This active file supersedes older **current-status** statements while retaining historical evidence through those archived files. It must not be interpreted as proof that unperformed manual/device/store/signing work has been completed.

---

# 1. Project identity and current release line

- Project: **CareNest**
- Repository: `https://github.com/sanskarIN/CareNest`
- Default branch: `main`
- Current release line: `1.0.0-rc.1`
- Creator/GitHub profile: `https://github.com/sanskarIN`
- Business email: `sanskarin@outlook.in`
- Support email: `supportramsandesh@gmail.com`
- Voluntary project support: `https://buymeacoffee.com/sanskarIN`
- Watermark: `Made by the Sanskar`
- License: Apache License 2.0

Requested repository-local Git identity:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Current continuation commits created through the GitHub repository interface show author/committer metadata as `Sanskar <sanskarin@outlook.in>`.

---

# 2. Product boundary that must not change accidentally

CareNest is a local-first organizational health app.

Current v1 intentionally does **not**:

- diagnose conditions;
- calculate or infer medicine dosage;
- recommend treatment;
- perform clinical medication-interaction checking;
- create clinical risk scores;
- replace a clinician/pharmacist;
- provide emergency services;
- guarantee notification delivery;
- require a CareNest account/backend;
- automatically synchronize health records to a CareNest cloud service;
- silently upload analytics/telemetry containing user state;
- silently share health information with caregivers or other users.

Medicine strength/instruction values remain opaque user-entered text.

Reminder planning comes only from explicit user-entered schedule values.

---

# 3. Current source architecture

Solution projects:

```text
src/CareNest.Shared
src/CareNest.Domain
src/CareNest.Application
src/CareNest.Infrastructure
src/CareNest.App

tests/CareNest.UnitTests
tests/CareNest.IntegrationTests
tests/CareNest.UiTests
```

Intended dependency direction:

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

Key rules:

- platform-neutral projects do not depend on MAUI;
- ViewModels do not issue SQL directly;
- current local-first v1 runtime does not casually add network/telemetry clients;
- deterministic reminder planning remains platform-neutral;
- SQLite persisted reminder state and operating-system request state are treated as separate surfaces;
- security/privacy/manual-release limits are documented rather than hidden.

Detailed references:

- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`
- `docs/CODEBASE_REFERENCE.md`
- `docs/architecture/ARCHITECTURE.md`
- `docs/architecture/SERVICE_BOUNDARIES.md`
- `docs/architecture/APPLICATION_FLOWS.md`

---

# 4. Current SQLite dependency security state

The former exact `GHSA-2m69-gcr7-jv3q` source exception is **remediated** in the current verified source graph.

Current package intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11` as the compatible bundle/API path;
- central transitive pinning enabled;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- `SQLitePCLRaw.provider.e_sqlite3` `2.1.12`;
- `SQLitePCLRaw.provider.sqlite3` `2.1.12`;
- `SQLitePCLRaw.provider.dynamic_cdecl` `2.1.12`;
- former exact `NuGetAuditSuppress` entry removed.

Remediation commits from the bug-audit continuation:

- `66cd701f84afd5021a28e7e3327b7da4fad249aa` — `fix: pin patched SQLite native dependency path`;
- `e939d5bd912d09ffa150c804519c15e2506b7bd7` — `security: remove resolved SQLite audit suppression`;
- `04868965c43d8a6d09b40075d92f20da9b26e32a` — `test: guard patched SQLite dependency baseline`.

`SqliteDependencySecurityContractTests` protects the maintained package floor and absence of the old advisory suppression.

Important distinction:

- source dependency remediation: complete and unsuppressed-audit green;
- packaged existing-user-data compatibility: still a manual production release gate.

Do **not** restore the old audit suppression simply because packaged compatibility testing is still pending.

Primary references:

- `Directory.Packages.props`
- `docs/security/DEPENDENCY_RISK_REGISTER.md`
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`
- `docs/CONFIGURATION_REFERENCE.md`

---

# 5. Current reminder/platform consistency model

CareNest separates:

1. explicit user schedule intent;
2. persisted `ReminderOccurrence` state;
3. OS scheduled notification/alarm requests.

These are separate persistence/state surfaces.

## Effective due time

- normal Scheduled occurrence → `ScheduledUtc`;
- valid Snoozed occurrence → `SnoozedUntilUtc`.

A future snooze remains upcoming after its original scheduled instant passes.

An overdue snooze is evaluated from the snooze due time rather than the stale original schedule time.

## Platform reconciliation

Current rules:

- cancel an existing OS request before replacement;
- cancel before quiet-hours suppression;
- cancel before invalidation;
- cancellation failure remains retryable;
- schedule edits retain enough old occurrence identity to cancel stale OS requests before final cleanup;
- current planner output becomes authoritative only after reconciliation can occur safely.

## Handled reminder actions

Taken, Skipped, Delayed, Missed, Snoozed and Cancelled use cancellation-first ordering:

1. validate action/snooze input;
2. cancel old OS request;
3. persist handled state only after cancellation succeeds;
4. for Snoozed, schedule the replacement after state persistence;
5. if a later essential operation fails, attempt non-cancelled previous-state restoration;
6. attempt non-cancelled reminder rebuild;
7. surface aggregate recovery failure instead of claiming contradictory state is consistent.

## Medicine/profile persistence compensation

- future platform requests are cancelled before medicine/profile database cascade deletion;
- if persistence fails after platform cancellation, non-cancelled rebuild compensation is attempted for records that remain;
- save flows reconcile reminders before later non-critical audit bookkeeping.

## Appointment persistence compensation

- `Appointment.StartsUtc` must be actual `DateTimeKind.Utc`;
- local/unspecified values are rejected rather than relabeled;
- notification permission denial is not successful scheduling;
- background rebuild does not repeatedly prompt while denied;
- platform request state is compensated/reconciled around database persistence;
- deletion cancels the platform request before deleting the appointment.

Key reminder-related commits retained in final runtime source lineage include:

- `4cf2aec989233d213ac7b1099a50d44e1acc3ca0` — `fix: reconcile snoozed and stale reminder occurrences`;
- `61772f968d8686e472b5849e77e0a3156936701d` — `fix: reconcile appointment reminders around persistence`;
- `633b6bbca587fbc5030b940132b3112d7a73b458` — `test: cover appointment reminder persistence compensation`;
- `1459d24314de4a2f2f4fa232deb4285bb8e33b23` — `fix: make reminder actions cancellation-first and recoverable`;
- `508adeb805d604274be8b069668429b6935f3fa6` — `test: support notification failure injection`;
- `da2aed19ee9224b8d8661f11520ab9396e2c005e` — `test: verify reminder action cancellation and recovery ordering`.

Analyzer-safe follow-up test fixes included:

- `cc9465136bd7de0e55e14386c19fa849a3e56067`;
- `834b2980167c41bc7e9c1ad69dc54ad5ccc7e53e`.

References:

- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`
- `docs/testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`

---

# 6. Report/plaintext cache hardening

Commit:

- `c844acdb63b5320344ff0d771d1365eaf7471f4a` — `security: remove shared report cache files after export`.

Current behavior:

- report writers use partial-file staging and atomic final move;
- failed/cancelled staging is cleaned best effort;
- application-owned shared report cache is removed after share handoff where CareNest still owns it;
- CareNest does not claim deletion of copies already owned by another app, cloud location, screenshot, backup or OS share service.

---

# 7. Encrypted document and backup security state

New encrypted document/backup streams use chunked AES-256-GCM framing v2.

Current properties:

- authenticated data chunks;
- authenticated terminal state in v2;
- trailing bytes after terminal rejected;
- legacy v1 read compatibility retained;
- v1 historical ciphertext is not represented as retroactively upgraded;
- strict decrypted backup archive topology validated before extraction;
- wrong-password/tamper/truncation/trailing-data rejection;
- protected document-recovery key portability in encrypted backup;
- missing/corrupt document master key plus existing ciphertext fails closed;
- application-owned mutable verifier/key/salt/crypto buffers cleared where practical;
- document import and backup restore use compensating rollback across independent state surfaces.

A future v1 migration/removal requires canonical historical fixtures plus explicit recovery/rollback verification.

References:

- `docs/architecture/DOCUMENT_VAULT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/security/THREAT_MODEL.md`
- `docs/testing/TESTING_GUIDE.md`

---

# 8. Historical verification corrections

PR #43 was previously described incorrectly in some documentation as fully green.

Actual historical PR #43 result:

- formatting/platform/CodeQL/audit had successes;
- core CI failed during integration testing;
- UI-contract suite was skipped;
- PR #43 is **not** release evidence.

Later failure-driven checkpoints exposed/fixed additional issues:

- PR #44 reproduced future-snooze, overdue-snooze and stale-occurrence defects;
- PR #46 exposed broader OS-reminder lifecycle source-contract defects;
- PR #47 proved an unsuppressed SQLite dependency graph could audit successfully;
- PR #48 passed unsuppressed audit/CodeQL but exposed a moving-base reminder interface compile mismatch;
- PR #49 exposed CA1861 in reminder-reconciliation test source and was fixed without suppression;
- PR #50 again passed unsuppressed SQLite audit but predated later source changes;
- PRs #51/#52 were superseded as source changed;
- PR #53 independently completed a fully green final runtime/test graph verification;
- PR #54 became the authoritative recorded runtime bug-audit baseline.

Failed/superseded markers were not treated as production source.

---

# 9. PR #54 — historical authoritative runtime bug-audit baseline

PR #54: `Verify final CareNest bug-audit source`.

Source/base SHA:

`4490f3f86752841d436e981b29279970c90c947b`

Marker head:

`929168a0a319b15d9e89997d86436d59ae731ad1`

Evidence:

- CareNest CI #503 / `31766059137`: success;
- formatting: success;
- UnitTests: 122 passed;
- IntegrationTests: 39 passed;
- UiTests/source-policy: 100 passed;
- total: 261 passed;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #503 / `31766059215`: success;
- unsuppressed Dependency Audit #35 / `31766059132`: success.

PR #54 was marker-only and closed without merge.

PR #53 independently corroborated the same final runtime/test graph.

PR #54 remains the historical authoritative runtime bug-audit source evidence.

---

# 10. Release-engineering hardening after PR #54

Later work changed verification-relevant source including workflows, build/release scripts, tests and policy contracts.

Therefore PR #54 could not be reused as the release-engineering baseline.

Major hardening completed:

## Exact production-tag coverage

Production tags matching `v*` now run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

## Dependency Audit event safety

Pull-request-only dependency comparison logic is guarded so tag/manual runs do not dereference pull-request-only metadata.

## Failure-preserving Release Evidence

Release Evidence records:

- source commit/ref;
- run ID;
- run attempt;
- toolchain information;
- tracked source manifest/checksums;
- unit TRX;
- integration TRX;
- UI-contract/policy TRX;
- transitive dependency inventories;
- workspace integrity;
- evidence checksums.

Evidence components are attempted independently. Available evidence is uploaded before final aggregate pass/fail evaluation.

Artifact identity includes commit SHA + run ID + run attempt.

A failed run can have an artifact; artifact existence alone is not release approval.

## Blocking local audits

Bash and PowerShell quality/preflight scripts now treat unsuppressed NuGet audit failures as blocking.

PowerShell explicitly checks native command exit status.

## Repository-local Git setup

Git helpers:

- locate repository root;
- require valid Git work tree;
- use `git config --local`;
- set `Sanskar` / `sanskarin@outlook.in`;
- verify values;
- fail on native Git errors.

## Hardened Release Gate

Release Gate is fail-closed for:

- open dependency risk;
- nested unchecked applicable checklist rows;
- required security/evidence documents;
- core tests.

Matching is hardened against normal case/indentation/nesting variations.

## Executable release-policy contracts

`CareNest.UiTests` now includes contracts for:

- release workflows/tags/manual triggers;
- Dependency Audit event safety;
- Release Evidence provenance/failure preservation/rerun identity;
- blocking release-preflight audit;
- clean-checkout/fail-closed quality gate;
- repository-local Git setup;
- Release Gate fail-closed matching.

These additions increased UiTests/source-policy from 100 at PR #54 to 124 at PR #56.

---

# 11. PR #55 — superseded release-engineering checkpoint

PR #55 proved an intermediate hardening snapshot:

- formatting: success;
- unit: 122 passed;
- integration: 39 passed;
- UI-contract/policy: 116 passed;
- total: 277 passed;
- Android Release: success;
- Windows Release: success;
- CodeQL #547 / `31769940053`: success;
- unsuppressed Dependency Audit #38 / `31769940039`: success.

The complete-file audit then found additional legitimate release-tooling/documentation corrections.

PR #55 was closed without merge and is not the current baseline.

---

# 12. PR #56 — authoritative current automated release-engineering baseline

PR #56: `Verify complete CareNest release-engineering source`.

Verification branch:

`ci/carenest-release-engineering-final-v2-20260814`

Frozen source/base SHA:

`4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`

Marker head:

`e3bc621cea05364a69abee0dadbd71a67c17bddb`

Marker file:

`build/verification/release-engineering-final-v2-20260814.txt`

Evidence:

- CareNest CI #571 / `31770929379`: **success**;
- formatting: **success**;
- UnitTests: **122 passed, 0 failed, 0 skipped**;
- IntegrationTests: **39 passed, 0 failed, 0 skipped**;
- UiTests/source-policy: **124 passed, 0 failed, 0 skipped**;
- total: **285 passed, 0 failed, 0 skipped**;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- CodeQL #571 / `31770929382`: **success**;
- unsuppressed Dependency Audit #41 / `31770929383`: **success**.

PR #56 was closed without merge after all required gates succeeded.

Its verification marker is not part of `main`.

Authoritative evidence record:

`docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md`

---

# 13. Complete documentation pass — current continuation

The user requested complete project documentation and all project code/documentation on GitHub `main` with commit messages.

A full repository documentation audit was performed across:

- root project/governance docs;
- user docs;
- architecture/ADRs;
- privacy/data lifecycle;
- security/threat/logging/dependency docs;
- design/accessibility/localization/store assets;
- setup/platform/troubleshooting/maintainer docs;
- tests/contracts/plans;
- release process/checklists/evidence;
- source project/file trees;
- central package/build configuration;
- GitHub workflows and local scripts.

## New canonical documentation added

### Complete project reference

Commit:

`06f6ae6968d01e272ab1c0b37190442df867c637`

Message:

`docs: add complete project documentation`

File:

`docs/COMPLETE_PROJECT_DOCUMENTATION.md`

Covers product identity/scope, architecture, features, data, reminders, encryption, backup, security/privacy, setup/build/test, release, governance and documentation map.

### Root README alignment

Commit:

`2796e2852c659f88e64666c7894c13cc08cda2e1`

Message:

`docs: promote PR56 in root README`

README now distinguishes historical PR #54 runtime evidence from current PR #56 release-engineering evidence and links the complete project reference.

### Concrete codebase/API reference

Commit:

`20649ff30bc1fb8b8c6321d725e492209e1dae9`

Message:

`docs: add complete codebase reference`

File:

`docs/CODEBASE_REFERENCE.md`

Maps:

- Shared files;
- Domain entities/rules;
- Application contracts/services;
- Infrastructure backup/document/persistence/report/security files;
- MAUI composition/platform resources;
- unit/integration/UI-policy test projects;
- build scripts;
- workflows;
- central configuration;
- where future code belongs;
- forbidden architecture shortcuts.

### Configuration/build/automation reference

Commit:

`37a179aaa2ad3d9a7ac944712cacb2e0d01a0183`

Message:

`docs: add configuration and automation reference`

File:

`docs/CONFIGURATION_REFERENCE.md`

Documents:

- exact current central package versions;
- build/analyzer/CI properties;
- NuGet audit policy;
- target frameworks;
- `CareNestTargetFramework`;
- restore/build/test/format commands;
- quality/preflight/Git scripts;
- Android/Windows/iOS/Mac Catalyst configuration;
- GitHub workflows;
- `v*` release tag behavior;
- secret/signing policy;
- provenance expectations.

### Maintainer operations manual

Commit:

`d7ca9b8400caf20ac506a9bfb81c8c3d58bc5da7`

Message:

`docs: add maintenance and operations manual`

File:

`docs/MAINTENANCE_AND_OPERATIONS.md`

Covers:

- routine maintenance;
- issue triage;
- bug-fix workflow;
- reminder changes;
- schema/SQLite dependency changes;
- general dependency changes;
- document/backup encryption changes;
- logging/privacy review;
- external support links;
- accessibility/localization;
- exact-head verification;
- release candidate preparation;
- production tags/evidence;
- signing/store operations;
- hotfixes;
- rollback/recovery planning;
- incident response.

### Documentation audit

Commit:

`198c8355348aaee76c30781d51214ae355e1dae9`

Message:

`docs: record complete documentation audit`

File:

`docs/releases/DOCUMENTATION_AUDIT_20260814.md`

Records the full documentation inventory and explicitly distinguishes documentation completeness from production release completeness.

### Documentation hub

Commit:

`332f95610c80000c7f5f3ae01074877fb438cab6`

Message:

`docs: complete documentation hub index`

`docs/README.md` now indexes all complete references, user docs, architecture, data, reminders, privacy/security, setup, testing, release and historical evidence.

### Documentation checklist

Commit:

`22116ebc4057d1eab33fb123593072b17c7bb115`

Message:

`docs: complete documentation checklist for PR56`

Updates the documentation completeness matrix to the PR #56 285-test source and adds codebase/configuration/automation/maintenance documentation categories.

---

# 14. Historical active-document preservation

Before replacing remaining stale active references, exact pre-correction blobs were preserved under:

`docs/history/pre-complete-docs-20260814/`

Commit:

`e7a7dde60a710ffc1fe25ce28a15aad1b72f0e3d`

Message:

`docs: preserve pre-completion documentation snapshots`

Preserved files include:

- `SECURITY_MODEL.md`;
- `THREAT_MODEL.md`;
- `DEVELOPMENT.md`;
- `ARCHITECTURE.md`;
- `NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`;
- `DOCUMENTATION_STANDARDS.md`;
- `CHANGELOG.md`;
- prior `what_changed.md`.

This means replacing stale active versions did not discard historical bytes.

---

# 15. Active reference finalization commits

## Security architecture

Commit:

`7a783dd7f9edf15e2f0f0b9943d7289c209f051c`

Message:

`docs: finalize current security architecture`

Active `docs/security/SECURITY_MODEL.md` now documents PR #56, current SQLite remediation, reminder integrity, encryption/backup/key behavior, Release Gate/Release Evidence, exact `v*` tags and remaining real security evidence.

## Threat model

Commit:

`d30d707e2fbcf7e98bd2372cf6b3865debd41bd6`

Message:

`docs: finalize current threat model`

Active `docs/security/THREAT_MODEL.md` now covers current local-first/data/encryption/reminder/platform/dependency/release-provenance threats and PR #56 evidence.

## Development setup

Commit:

`24b621c114cf877d603c315bcd64b9e9e9c8d301`

Message:

`docs: finalize PR56 development setup`

Active `docs/setup/DEVELOPMENT.md` now contains current package/toolchain/build/test/audit/Git/release commands and PR #56 totals instead of PR #55 pending-verification wording.

## Architecture

Commit:

`998d03f784b6ec85d18991596df45012c89b4d79`

Message:

`docs: finalize current architecture reference`

Active architecture now reflects current reminder compensation, SQLite provider state, encrypted document/backup security, platform/build/release boundaries and PR #56 evidence.

## Notification/platform behavior

Commit:

`04cb7563949ba4a9f5d8cac46c08a84d94c844bd`

Message:

`docs: finalize notification platform behavior`

Active notification docs now match implemented reconciliation/action/permission/recovery behavior and document Android/Windows/Apple manual release requirements.

## Documentation standards

Commit:

`fb07250ab61d9ddcdb1760c862dd231d49100107`

Message:

`docs: finalize documentation standards`

Standards now require accurate PR #56 verification wording, historical evidence preservation, precise SQLite/security/reminder language, and strict separation of documentation/CI from manual release evidence.

## Changelog

Commit:

`926f9d5fe7e3ba8ba4482bb39edac2adc1906b36`

Message:

`docs: finalize release engineering changelog`

Active changelog now includes PR #56 release-engineering hardening and documentation-completion work. The exact previous changelog is preserved in history.

---

# 16. Source tree audit completed during documentation work

The repository source tree was re-audited rather than documenting from assumptions.

## Shared source confirmed

- `AppConstants.cs`
- `Guard.cs`
- `Result.cs`
- `SecretKeys.cs`
- `SettingKeys.cs`
- `TimeProviderExtensions.cs`
- project file.

## Domain source confirmed

Entities include:

- `AppSetting`
- `Appointment`
- `AuditEntry`
- `BackupMetadata`
- `CareDocument`
- `DocumentTag`
- `EmergencyContact`
- `MedicationLogEntry`
- `Medicine`
- `MedicineSchedule`
- `PersonProfile`
- `ReminderOccurrence`
- `ScheduleTime`
- `StockAdjustment`
- `Tag`

Rules include appointment, medicine/schedule and profile validation.

## Application source confirmed

Contracts include:

- `ICareNestRepository`
- `IInfrastructureServices`
- `IReminderCoordinator`
- `IUseCaseServices`

Services include:

- `AppointmentService`
- `BackupReminderCoordinator`
- `DocumentService`
- `MedicineService`
- `ProfileService`
- `ReminderCoordinator`
- `ReminderPlanner`

## Infrastructure source confirmed

Major files include:

- `BackupArchiveValidator`
- `BackupManifest`
- `EncryptedBackupService`
- `CareNestStorageOptions`
- `EncryptedDocumentStore`
- `CareNestRepository`
- `SchemaInfo`
- `SqliteDatabase`
- `CsvWriter`
- `ReportService`
- `SimplePdfWriter`
- `ChunkedAead`

## MAUI source confirmed

App composition/platform tree includes:

- `App.xaml` / `.cs`
- `MauiProgram.cs`
- navigation/converters;
- Android platform files and resources;
- Windows platform files/manifests;
- iOS platform files/plists;
- Mac Catalyst platform files/plists;
- branding/icon/support resources;
- presentation/ViewModel/platform service tree.

## Unit tests confirmed

Concrete tests cover:

- appointment/profile rules;
- appointment service;
- backup reminder coordinator;
- document service;
- medicine rules/service;
- profile service;
- reminder action recovery/validation;
- reminder planner archived/boundary/DST/edge/ownership/property/UTC/main behavior;
- schedule validation hardening;
- deterministic repository/time/notification/document/reminder test doubles.

Integration and UI/source-policy projects were also confirmed in the solution tree.

---

# 17. Central package/build configuration audited

`Directory.Packages.props` currently contains:

- `Microsoft.Maui.Controls` `10.0.20`;
- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- `SQLitePCLRaw.provider.e_sqlite3` `2.1.12`;
- `SQLitePCLRaw.provider.sqlite3` `2.1.12`;
- `SQLitePCLRaw.provider.dynamic_cdecl` `2.1.12`;
- `Microsoft.Extensions.Logging.Debug` `10.0.0`;
- `Microsoft.Extensions.Logging.Abstractions` `10.0.0`;
- `Microsoft.Extensions.DependencyInjection.Abstractions` `10.0.0`;
- `Microsoft.NET.Test.Sdk` `17.14.1`;
- `xunit` `2.9.3`;
- `xunit.runner.visualstudio` `3.1.4`;
- `coverlet.collector` `6.0.4`.

Central transitive pinning is enabled.

`Directory.Build.props` confirms:

- latest C# language version;
- nullable enabled;
- implicit usings;
- analyzers enabled at latest-recommended;
- warnings-as-errors in CI;
- deterministic builds;
- ContinuousIntegrationBuild in CI;
- repository/author metadata.

---

# 18. Current GitHub/release automation documented and retained

Current key workflows:

- `.github/workflows/ci.yml`
- `.github/workflows/codeql.yml`
- `.github/workflows/dependency-review.yml`
- `.github/workflows/release-gate.yml`
- `.github/workflows/release-evidence.yml`

Current local scripts:

- `build/scripts/quality-gate.sh`
- `build/scripts/quality-gate.ps1`
- `build/scripts/release-preflight.sh`
- `build/scripts/release-preflight.ps1`
- `build/scripts/setup-git.sh`
- `build/scripts/setup-git.ps1`

Production tag behavior:

`v*` → exact tagged source through CI + CodeQL + Dependency Audit + Release Gate + Release Evidence.

---

# 19. Current documentation map

Primary references:

- `README.md`
- `docs/README.md`
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`
- `docs/CODEBASE_REFERENCE.md`
- `docs/CONFIGURATION_REFERENCE.md`
- `docs/MAINTENANCE_AND_OPERATIONS.md`
- `docs/releases/DOCUMENTATION_AUDIT_20260814.md`
- `docs/releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md`
- `PROJECT_STATUS.md`
- `CHANGELOG.md`
- this `what_changed.md`.

Architecture:

- `docs/architecture/ARCHITECTURE.md`
- `APPLICATION_FLOWS.md`
- `SERVICE_BOUNDARIES.md`
- `DATABASE_SCHEMA.md`
- `DATA_STORAGE_AND_EXPORT.md`
- `DOCUMENT_VAULT.md`
- `BACKUP_AND_RESTORE.md`
- `NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`
- ADRs.

Privacy/security:

- `PRIVACY.md`
- `SECURITY.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `DATA_LIFECYCLE.md`
- `LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md`
- `docs/security/SECURITY_MODEL.md`
- `THREAT_MODEL.md`
- `LOGGING_PRIVACY.md`
- `DEPENDENCY_RISK_REGISTER.md`
- `FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md`.

Testing:

- `docs/testing/TESTING_GUIDE.md`
- `TEST_PLAN.md`
- `REMINDER_SCHEDULING_CONTRACT.md`
- `SETTINGS_LIFECYCLE_CONTRACT.md`
- `BUG_AUDIT_REGRESSION_MATRIX_20260814.md`.

Release:

- `docs/releases/RELEASE_PROCESS.md`
- `RELEASE_CHECKLIST.md`
- `QUALITY_GATE.md`
- `MANUAL_TEST_MATRIX.md`
- `SECURITY_RELEASE_REVIEW.md`
- `STORE_SUBMISSION_CHECKLIST.md`
- `RELEASE_EVIDENCE.md`
- `VERIFICATION_BRANCH_PROTOCOL.md`
- `NEXT_STEPS.md`
- `SQLITE_DEPENDENCY_MIGRATION_PLAN.md`
- `RELEASE_ENGINEERING_VERIFICATION_20260814.md`
- `DOCUMENTATION_AUDIT_20260814.md`.

---

# 20. Manual/external production blockers — still open

CareNest source/documentation is not the same thing as a published production release.

Do **not** check these as complete until actual evidence exists.

## Android

- [ ] real device/emulator manual matrix;
- [ ] notification permission denied/granted;
- [ ] actual notification delivery;
- [ ] future/overdue snooze behavior;
- [ ] cancellation-first handled actions;
- [ ] stale request cleanup after schedule changes;
- [ ] medicine/profile delete OS-request cleanup;
- [ ] exact/inexact alarm diagnostics;
- [ ] battery optimization behavior;
- [ ] reboot recovery;
- [ ] clock/time-zone recovery;
- [ ] force-stop/vendor limitation behavior;
- [ ] packaged SQLite compatibility.

## Windows

- [ ] manual feature matrix;
- [ ] running-app notification behavior;
- [ ] documented closed-app limitation;
- [ ] same-ID timer replacement/cancellation behavior;
- [ ] cancellation-first actions;
- [ ] restart/recovery;
- [ ] packaged SQLite compatibility.

## iOS / iPadOS

- [ ] real device permission/delivery;
- [ ] snooze/reconciliation/action behavior;
- [ ] restart/time-zone behavior;
- [ ] packaged SQLite compatibility;
- [ ] notification preview privacy/accessibility;
- [ ] production signing/provisioning.

## Mac Catalyst

- [ ] notification permission/delivery;
- [ ] action/reconciliation behavior;
- [ ] restart behavior;
- [ ] keyboard/focus;
- [ ] packaged SQLite compatibility;
- [ ] signing/package behavior.

## Packaged data/encryption compatibility

- [ ] representative fictional pre-remediation SQLite upgrade/install;
- [ ] SQLite integrity after package update;
- [ ] all structured data readable;
- [ ] reminder rebuild/reconciliation after upgrade;
- [ ] existing encrypted document access through existing key path;
- [ ] current backup create/restore;
- [ ] wrong-password/tamper packaged behavior;
- [ ] clean-install restore;
- [ ] canonical pre-remediation backup compatibility where available;
- [ ] canonical historical framing-v1 fixture checks where available;
- [ ] new framing-v2 packaged behavior.

## Accessibility

- [ ] screen reader;
- [ ] large text/text scaling;
- [ ] keyboard/focus;
- [ ] contrast;
- [ ] light/dark/system themes;
- [ ] reduced motion;
- [ ] color-independent state/validation cues.

## Store/policy

- [ ] current Apple App Store support-link policy review;
- [ ] current Google Play support-link policy review;
- [ ] health-organizer/category wording review;
- [ ] privacy/data-safety disclosures;
- [ ] store screenshots using fictional data;
- [ ] listing/support/privacy/terms/security links;
- [ ] channel-specific support-link visibility if needed.

## Signing/distribution

- [ ] Android production keystore outside Git;
- [ ] Apple certificates/provisioning outside Git;
- [ ] Windows signing identity outside Git;
- [ ] signed Android package;
- [ ] signed iOS archive;
- [ ] signed/notarized/store-ready Mac Catalyst package;
- [ ] signed Windows package;
- [ ] package identifiers/version/build metadata;
- [ ] package checksums/provenance.

## Exact final production release

After all applicable pre-tag blockers and any resulting source/config changes:

1. select exact approved production commit;
2. if verification-relevant source changed after PR #56, complete a new exact-head marker verification;
3. create approved `v*` tag pointing to exact commit;
4. require tagged CareNest CI success;
5. require tagged CodeQL success;
6. require tagged unsuppressed Dependency Audit success;
7. require tagged Release Gate success;
8. require tagged CareNest Release Evidence success;
9. record evidence artifact identity/checksums;
10. verify signed package provenance;
11. publish only after every applicable gate succeeds.

A failed production tag must not be moved/relabelled to look successful.

---

# 21. Current interpretation

- CareNest remains `1.0.0-rc.1`.
- The local-first v1 implementation scope is source-complete for the current documented feature set.
- PR #56 is the authoritative current automated release-engineering source baseline.
- PR #54 remains historical runtime bug-audit evidence.
- 285/285 core tests are green on PR #56.
- Android/Windows/iOS simulator/Mac Catalyst Release builds are green on PR #56.
- CodeQL and unsuppressed Dependency Audit are green on PR #56.
- The former SQLite advisory suppression remains removed.
- The maintained SQLite native/provider floor remains regression-protected.
- Exact `v*` production tags now cover CI, CodeQL, Dependency Audit, Release Gate and Release Evidence.
- Release Evidence preserves available failed-run diagnostics and identifies run attempts.
- Local release/quality scripts treat dependency audit as blocking.
- Repository-local Git setup is fail-closed and uses `Sanskar <sanskarin@outlook.in>`.
- Release Gate matching is hardened against nested/case/indentation drift.
- Complete project/codebase/configuration/maintenance documentation now exists.
- Older pre-completion active docs are preserved byte-for-byte under `docs/history/pre-complete-docs-20260814/`.
- No cloud/account/telemetry/clinical-decision functionality was added by this continuation.
- Manual device/accessibility/store/signing/packaged-data/exact-production-tag evidence remains release-blocking until actually performed.

---

# 22. Next safe continuation order

If continuing toward public `1.0.0`, use this order:

1. keep `main` source/documentation stable unless a real defect is found;
2. run final documentation/source-policy exact-head verification after the complete documentation pass;
3. complete representative packaged SQLite/encrypted-data compatibility using fictional data;
4. complete Android/Windows/iOS/Mac Catalyst manual matrices;
5. complete accessibility testing;
6. review current Apple/Google store policy/disclosures;
7. configure signing outside Git;
8. build/inspect signed packages;
9. update only factual checklist/status evidence;
10. if any verification-relevant source changes, re-run exact-head verification;
11. select exact production commit;
12. create `v*` production tag;
13. require all tagged workflows including Release Gate/Release Evidence;
14. publish only after every applicable gate is complete.

Do not reintroduce the former SQLite audit suppression and do not weaken analyzer/audit/release gates to obtain a green result.

---

# 23. 2026-08-15 packaged-release and store-policy hardening continuation

This continuation advances source-side release readiness without falsely completing work that requires actual store review, signing identities, packaged builds, real devices, assistive technology, or manual compatibility evidence.

Because this continuation changes application project configuration, the About presentation/ViewModel, UI/source-policy tests and release-preflight scripts, PR #56 remains authoritative **historical** evidence for its frozen source boundary but is no longer exact-head verification for the current `main` source. The current head requires a new exact-head verification before production promotion after this continuation stabilizes.

## Commit 1 — configurable voluntary project-support surface

Commit:

`35690d2f1fbe8bb56d91e718dab688fe4de6cc0d`

Message:

`feat: make voluntary funding link store-configurable`

Changed:

- `src/CareNest.App/CareNest.App.csproj`;
- `src/CareNest.App/ViewModels/AboutViewModel.cs`;
- `src/CareNest.App/Views/AboutPage.xaml`;
- `tests/CareNest.UiTests/CriticalFlowContractTests.cs`.

Behavior:

- `CareNestShowFundingLink` defaults to `true`;
- default open-source builds continue to expose the voluntary Buy Me a Coffee support card;
- `-p:CareNestShowFundingLink=false` removes the `CARENEST_FUNDING_LINK` compile symbol;
- `AboutViewModel.IsProjectSupportVisible` then evaluates false;
- the entire About support card is hidden rather than leaving a dead external-link button;
- no health, reminder, document, backup, encryption, profile, appointment, report, app-lock, or medical-safety behavior changes with this flag;
- support remains voluntary and does not unlock medical advice, premium health functionality, reminder behavior, or access to health data.

The new UI contract prevents the store-specific visibility escape hatch from silently disappearing.

## Commit 2 — package metadata and privacy regression contracts

Commit:

`7ccea4ff5367b3c4e94b156f989799d91d6f52ff`

Message:

`test: enforce package metadata and privacy contracts`

Added:

- `tests/CareNest.UiTests/PackageMetadataContractTests.cs`.

Updated:

- `tests/CareNest.UiTests/RepositoryLocator.cs` with reusable repository-path resolution.

Contracts now protect:

- app title `CareNest`;
- application identifier `com.sanskar.carenest`;
- semantic release display-version shape and positive build number;
- Android/iOS/Mac Catalyst/Windows target frameworks and minimum supported OS declarations;
- Android reminder/camera permissions;
- absence of Android `INTERNET` permission for the local-first v1 package boundary;
- Android backup disabled and cleartext transport disabled;
- Apple camera/photo purpose strings and absence of arbitrary transport-security opt-out;
- Windows package identity/display name/minimum platform declarations;
- required CareNest app icon, foreground icon, splash, mark and support assets.

These contracts reduce accidental package/privacy drift. They do not replace store package inspection or current store-policy review.

## Commit 3 — release-preflight propagation of the store funding policy

Commit:

`1fe68a73aaa41622391d8ff6e53171ca98dce055`

Message:

`build: pass store funding policy into release preflight`

Updated:

- `build/scripts/release-preflight.sh`;
- `build/scripts/release-preflight.ps1`.

New release-preflight behavior:

- reads `CARENEST_SHOW_FUNDING_LINK`;
- defaults to `true`;
- accepts only exact logical values `true` or `false` after PowerShell normalization;
- fails closed for invalid values;
- prints the selected package policy;
- propagates the value into optional MAUI restore and Release build through `CareNestShowFundingLink`.

This makes a store-specific support-link package reproducible instead of relying on hand-edited source or a store-only fork.

## Commit 4 — store-build policy documentation

Commit:

`0a9d994ea310f00d715684c993ee2d954dc0f081`

Message:

`docs: define store-specific funding-link build policy`

Added:

`docs/releases/STORE_BUILD_POLICY.md`

The document defines:

- the voluntary support product boundary;
- direct build commands for enabled/disabled support surfaces;
- release-preflight examples;
- fail-closed property rules;
- per-release Apple/Google policy-review requirements;
- package checks for enabled and disabled variants;
- evidence fields including source SHA, target, application identity/version, selected flag, policy-review conclusion, package checksum and signing provenance.

The policy explicitly says that if a store disallows the external support link or the policy is uncertain, the affected package should use `CareNestShowFundingLink=false` while the project remains open source and repository funding may remain where separately permitted.

## Commit 5 — packaged release validation runbook

Commit:

`fe17e1ad752250d81d502ef7615fc1e652842e47`

Message:

`docs: add packaged release validation runbook`

Added:

`docs/releases/PACKAGED_RELEASE_VALIDATION.md`

The runbook gives a repeatable evidence process for:

- freezing the exact candidate source;
- release preflight;
- package identity/version capture;
- SHA-256 artifact checksums;
- fresh-install smoke testing;
- funding-link enabled/disabled package inspection;
- packaged SQLite existing-data upgrade compatibility;
- encrypted-document compatibility;
- backup/restore/wrong-password/tamper/historical-fixture checks;
- real reminder lifecycle/recovery;
- accessibility;
- current store-policy review;
- signing provenance and secret handling;
- final exact-tag release evidence.

It supplements `docs/releases/MANUAL_TEST_MATRIX.md` and explicitly does not mark any manual row complete without actual target evidence.

## Automated verification state while this section was prepared

The push of current head `fe17e1ad752250d81d502ef7615fc1e652842e47` triggered:

- CareNest CI run `31866933962`;
- CodeQL run `31866933951`.

At the time this section was first prepared, both runs were still in progress. CareNest CI had entered platform-neutral formatting/core testing and Android/Windows/Apple build jobs without a reported failure. This file must be updated with final results only after GitHub reports them; an in-progress run is not represented as passing evidence.

## Production blockers intentionally still open

This continuation does **not** mark complete:

- Apple App Store current support-link policy review;
- Google Play current support-link policy review;
- packaged support-link variant inspection;
- manual Android/Windows/iOS/Mac Catalyst matrices;
- SQLite packaged existing-data compatibility;
- canonical historical encrypted backup/document fixtures where available;
- accessibility testing with real assistive technologies;
- signing identities/credentials outside Git;
- signed production packages;
- store screenshots/listings/data-safety/privacy metadata;
- exact approved production `v*` tag and tagged workflow evidence.

## Next exact continuation order after these changes

1. allow the current push CI and CodeQL runs to finish and fix any real defect without weakening tests/analyzers;
2. synchronize `PROJECT_STATUS.md`, `NEXT_STEPS.md`, changelog and release documentation to this source boundary;
3. complete a new exact-head marker verification because verification-relevant source changed after PR #56;
4. use `STORE_BUILD_POLICY.md` to decide the BMC visibility independently for each target store after current policy review;
5. use `PACKAGED_RELEASE_VALIDATION.md` plus `MANUAL_TEST_MATRIX.md` for actual packaged/device/accessibility evidence;
6. configure signing outside Git;
7. produce and inspect signed candidate packages;
8. select the exact approved production commit;
9. create a non-movable approved `v*` tag;
10. require tagged CI, CodeQL, unsuppressed Dependency Audit, Release Gate and Release Evidence success before publication.

---

# 24. 2026-08-15 exact-head verification and store-policy review completion

This section supersedes only the in-progress verification/policy status recorded earlier in section 23. It does not rewrite or remove any earlier history.

## Frozen executable/source verification boundary

The complete 2026-08-15 application/project/test/release-script continuation was frozen at:

`826b79925dad4402f65fccfecd4a29b353b6e2f3`

Verification branch:

`ci/carenest-packaged-release-final-20260815`

Marker/head SHA:

`b92e3b79857db2f6cb8346fb881fe65b43f8453b`

Marker path:

`build/verification/packaged-release-store-policy-final-20260815.txt`

Verification PR:

`https://github.com/sanskarIN/CareNest/pull/58`

PR #58 changed exactly one file: the marker. It was closed without merge after all required gates completed successfully. The verification marker therefore never entered `main`.

## PR #58 exact automated evidence

CareNest CI #608 / run `31867245796`: **success**.

Exact core results:

- platform-neutral formatting: **success**;
- UnitTests: **122 passed, 0 failed, 0 skipped**;
- IntegrationTests: **39 passed, 0 failed, 0 skipped**;
- UiTests/source-policy: **130 passed, 0 failed, 0 skipped**;
- total: **291 passed, 0 failed, 0 skipped**.

Exact target Release results from the same PR source boundary:

- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**.

Security/dependency results:

- CodeQL #608 / run `31867245799`: **success**;
- unsuppressed Dependency Audit #43 / run `31867245800`: **success**.

The dependency audit completed both the platform-neutral and MAUI dependency graphs without restoring the former SQLite advisory suppression.

PR #58 is therefore the latest authoritative exact automated source baseline for the 2026-08-15 packaged-release/store-policy continuation. PR #56 remains valid historical exact evidence for its own frozen 2026-08-14 source boundary, and PR #54 remains the historical runtime bug-audit baseline.

Authoritative dated evidence file:

`docs/releases/PACKAGED_RELEASE_HARDENING_VERIFICATION_20260815.md`

Evidence-record commit:

`7ad45c82e6cf2877d693fd8481591f9969082eba` — `docs: record PR58 packaged release verification`.

Documentation-only commits after frozen SHA `826b79925dad4402f65fccfecd4a29b353b6e2f3` may record the already-completed evidence without changing the executable/project/test/workflow/build-script source verified by PR #58. Any later runtime, test, project, package, platform, workflow or build/release-script change requires a fresh exact-head verification before production promotion.

## Apple and Google support-link policy review completed

A dated review of current official Apple App Review Guidelines, current Google Play Payments policy guidance and the current Buy Me a Coffee provider fee model was recorded in:

`docs/releases/STORE_POLICY_REVIEW_20260815.md`

Policy-review commit:

`0488c68899eb8c6b5ef0de1753d3d3552fd97871` — `docs: record 2026-08-15 store support policy review`.

The current official Apple/Google optional gift/tip exceptions reviewed use literal conditions that the contribution be optional, provide no digital entitlement, and direct 100% of the relevant gift/contribution to the receiver/creator. The current Buy Me a Coffee provider model states a 5% platform transaction fee, so CareNest release engineering does not assume that its external BMC flow satisfies the literal 100% condition.

Conservative current production-package decision:

- normal/open-source/direct builds may retain the default `CareNestShowFundingLink=true` where their distribution channel permits it;
- initial Apple App Store production candidate: `CareNestShowFundingLink=false`;
- initial Google Play production candidate: `CareNestShowFundingLink=false`;
- enable the external BMC card for a store package only if a current storefront/country/program-specific review or explicit store-review outcome clearly permits it.

This policy review is source/release-engineering evidence, not Apple/Google approval. The actual submission-time policy must be rechecked because store policies/programs can change.

## Store/package work that remains open

The policy review itself is complete, but the following are still real release blockers:

- [ ] actual Apple App Store candidate package built with `CareNestShowFundingLink=false`;
- [ ] actual Google Play candidate package built with `CareNestShowFundingLink=false`;
- [ ] packaged About-page inspection proving the BMC image/button/URL/card is absent;
- [ ] representative packaged SQLite existing-data upgrade/integrity/readability;
- [ ] canonical historical encrypted document/backup compatibility where fixtures exist;
- [ ] Android manual device/emulator matrix;
- [ ] Windows manual matrix;
- [ ] iOS/iPadOS real-device matrix;
- [ ] Mac Catalyst manual matrix;
- [ ] actual reminder permission/delivery/restart/reboot/time-zone behavior;
- [ ] accessibility with representative assistive technologies;
- [ ] production signing credentials/identities outside Git;
- [ ] signed package generation and provenance;
- [ ] store screenshots/listing/privacy/data-safety metadata;
- [ ] submission-time Apple/Google policy re-review;
- [ ] exact approved production `v*` tag;
- [ ] tagged CareNest CI, CodeQL, unsuppressed Dependency Audit, Release Gate and Release Evidence success;
- [ ] final package checksums and publication evidence.

## Current next safe continuation order

1. keep the verified executable/project/test/workflow/build-script source stable unless a real defect is discovered;
2. use `docs/releases/PACKAGED_RELEASE_VALIDATION.md` for packaged SQLite/encrypted-data and target smoke evidence;
3. create Apple/Google store candidates with `CareNestShowFundingLink=false` under the current conservative decision;
4. perform packaged About-page inspection and record package checksums/source/signing provenance;
5. complete Android, Windows, iOS/iPadOS and Mac Catalyst manual matrices;
6. complete accessibility checks;
7. configure production signing outside Git and inspect signed artifacts;
8. complete store metadata/privacy/data-safety/listing evidence;
9. re-review the official store policy at actual submission time;
10. if any verification-relevant source changes, repeat marker-only exact-head verification;
11. select the exact approved production commit and create the non-movable approved `v*` tag;
12. require tagged CI, CodeQL, unsuppressed Dependency Audit, Release Gate and Release Evidence success;
13. publish only after every applicable production gate has actual evidence.

Do not call CareNest bug-free or production-published solely from the 291-test automated matrix. The automated source baseline is green; the remaining manual, packaged, signing and external-store evidence is intentionally still release-blocking.
