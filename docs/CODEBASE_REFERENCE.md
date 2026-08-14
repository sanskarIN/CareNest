# CareNest Codebase and API Reference

This document maps the implementation source to its responsibilities. It complements `COMPLETE_PROJECT_DOCUMENTATION.md` and the architecture documents by naming the concrete source files and explaining where new work belongs.

## 1. Solution and dependency direction

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

Tests depend on the layers they validate. Platform-neutral projects must not gain MAUI dependencies.

## 2. Shared project

Path: `src/CareNest.Shared/`

- `CareNest.Shared.csproj` — shared project definition.
- `AppConstants.cs` — canonical product/repository/contact/support/funding constants used across appropriate app surfaces.
- `Guard.cs` — reusable argument/state guard helpers.
- `Result.cs` — small shared result primitive where used.
- `SecretKeys.cs` — canonical secure-storage key identifiers; values identify secret slots rather than containing secret material.
- `SettingKeys.cs` — canonical settings keys.
- `TimeProviderExtensions.cs` — shared deterministic time-provider helper behavior.

Shared code should remain small, dependency-light, and free from MAUI/persistence implementation details.

## 3. Domain project

Path: `src/CareNest.Domain/`

### Project/common

- `CareNest.Domain.csproj`
- `Common/EntityBase.cs` — base identity/timestamp-style entity state shared by domain records.

### Entities

- `Entities/AppSetting.cs` — persisted application setting record.
- `Entities/Appointment.cs` — appointment data, including explicit UTC scheduling fields.
- `Entities/AuditEntry.cs` — privacy-minimized audit/event record.
- `Entities/BackupMetadata.cs` — local backup history/metadata record.
- `Entities/CareDocument.cs` — encrypted-document metadata record; encrypted payload bytes live outside normal SQLite metadata.
- `Entities/DocumentTag.cs` — document/tag relationship.
- `Entities/EmergencyContact.cs` — user-entered contact information.
- `Entities/MedicationLogEntry.cs` — user/reminder medication-log state.
- `Entities/Medicine.cs` — user-entered medicine record; strength/instruction text remains opaque and non-clinical.
- `Entities/MedicineSchedule.cs` — explicit schedule configuration.
- `Entities/PersonProfile.cs` — local person/family profile.
- `Entities/ReminderOccurrence.cs` — materialized reminder state and platform-request association.
- `Entities/ScheduleTime.cs` — explicit user-entered schedule clock time.
- `Entities/StockAdjustment.cs` — user-entered stock/refill adjustment information.
- `Entities/Tag.cs` — reusable local tag.

### Enums

- `Enums/DomainEnums.cs` — recognized lifecycle/schedule/reminder/log enum values.

Undefined/unrecognized enum values must fail validation rather than being persisted as if they were known behavior.

### Rules

- `Rules/AppointmentRules.cs` — appointment validation, including explicit UTC/time-zone shape.
- `Rules/MedicineRules.cs` — medicine/schedule structural validation; never infers dosage or medical intent.
- `Rules/ProfileRules.cs` — profile validation.

## 4. Application project

Path: `src/CareNest.Application/`

### Contracts

- `Contracts/ICareNestRepository.cs` — structured local repository abstraction used by application services.
- `Contracts/IInfrastructureServices.cs` — platform/infrastructure-facing abstractions such as encryption, notifications, files/reports/backup-related dependencies where defined.
- `Contracts/IReminderCoordinator.cs` — platform-neutral reminder coordination API.
- `Contracts/IUseCaseServices.cs` — higher-level application use-case service interfaces.

Application contracts should expose behavior rather than SQLite/MAUI implementation details.

### Services

- `Services/AppointmentService.cs` — appointment save/delete/rebuild orchestration, UTC enforcement, permission-aware notification scheduling, and database/platform compensation.
- `Services/BackupReminderCoordinator.cs` — explicit backup-reminder scheduling/cancellation using local backup state and notification permission.
- `Services/DocumentService.cs` — encrypted document import/export/delete orchestration, metadata/payload compensation, and audit boundaries.
- `Services/MedicineService.cs` — medicine/schedule save/delete orchestration, explicit stock behavior, and reminder reconciliation.
- `Services/ProfileService.cs` — profile create/update/delete orchestration, reminder reconciliation, encrypted document/photo cleanup, and audit boundaries.
- `Services/ReminderCoordinator.cs` — persisted occurrence ↔ OS request reconciliation, handled action ordering, snooze/effective-due behavior, overdue handling, cancellation-first transitions, and compensation/recovery.
- `Services/ReminderPlanner.cs` — deterministic, platform-neutral occurrence materialization from explicit user-entered schedules.

### Important application invariants

- planner windows use explicit UTC values;
- appointments require actual UTC instants;
- snooze requires explicit future UTC;
- `SnoozedUntilUtc` is the effective due time for a valid snooze;
- stale platform requests are cancelled before replacement/suppression/invalidation;
- handled actions cancel the old platform request before handled-state persistence;
- cancellation failure remains retryable;
- database/OS scheduling surfaces use compensation rather than pretending to be one transaction;
- no clinical inference is introduced into services.

## 5. Infrastructure project

Path: `src/CareNest.Infrastructure/`

### Backup

- `Backup/BackupArchiveValidator.cs` — validates decrypted backup topology against the expected allowlist before extraction.
- `Backup/BackupManifest.cs` — backup manifest model.
- `Backup/EncryptedBackupService.cs` — database/document backup packaging, password-derived encryption, restore validation/commit/rollback, key portability, and completion semantics.

### Configuration

- `Configuration/CareNestStorageOptions.cs` — local database/document/backup-related storage path options.

### Documents

- `Documents/EncryptedDocumentStore.cs` — encrypted document payload storage, master-key access, encrypt/decrypt/export lifecycle, and failure cleanup.

### Persistence

- `Persistence/CareNestRepository.cs` — concrete structured data repository and atomic multi-step operations.
- `Persistence/SchemaInfo.cs` — schema version record.
- `Persistence/SqliteDatabase.cs` — SQLite connection initialization, migrations, WAL/busy-timeout configuration, checkpoint/snapshot/integrity-related behavior.

### Reports

- `Reports/CsvWriter.cs` — CSV output with portable spreadsheet formula-like string neutralization and safe staging behavior.
- `Reports/ReportService.cs` — profile/report/export construction.
- `Reports/SimplePdfWriter.cs` — project PDF writer used by report generation.

### Security

- `Security/ChunkedAead.cs` — chunked AES-256-GCM authenticated stream framing; new writes use v2 authenticated termination and reads retain v1 compatibility where supported.

### Project metadata

- `CareNest.Infrastructure.csproj`
- `Properties/AssemblyInfo.cs`

## 6. MAUI application project

Path: `src/CareNest.App/`

### Composition/startup

- `CareNest.App.csproj` — multi-target MAUI project definition.
- `App.xaml` / `App.xaml.cs` — application resources and startup app object.
- `MauiProgram.cs` — dependency injection/composition root, infrastructure/application/platform registration.
- `GlobalUsings.cs` — application-level shared imports.

### Navigation

- `Navigation/RouteNames.cs` — canonical app routes.

ViewModels should use navigation abstractions/routes rather than embedding persistence/platform details.

### Converters

- `Converters/CommonConverters.cs` — reusable binding converters.

### Platform implementations

#### Android

- `Platforms/Android/AndroidManifest.xml`
- `Platforms/Android/MainActivity.cs`
- `Platforms/Android/MainApplication.cs`
- `Platforms/Android/PlatformNotificationService.Android.cs`
- Android resources under `Platforms/Android/Resources/`

Android notification/recovery behavior must account for permission, alarm capability/policy, battery restrictions, reboot, time/time-zone changes, force-stop, and receiver lifetime.

#### iOS

- `Platforms/iOS/AppDelegate.cs`
- `Platforms/iOS/Info.plist`
- `Platforms/iOS/PlatformNotificationService.iOS.cs`
- `Platforms/iOS/Program.cs`

#### Mac Catalyst

- `Platforms/MacCatalyst/AppDelegate.cs`
- `Platforms/MacCatalyst/Info.plist`
- `Platforms/MacCatalyst/PlatformNotificationService.MacCatalyst.cs`
- `Platforms/MacCatalyst/Program.cs`

#### Windows

- `Platforms/Windows/App.xaml`
- `Platforms/Windows/App.xaml.cs`
- `Platforms/Windows/Package.appxmanifest`
- `Platforms/Windows/PlatformNotificationService.Windows.cs`

The Windows fallback is documented as an in-process limitation rather than guaranteed closed-app delivery.

### Resources

`src/CareNest.App/Resources/` contains application icon, splash/branding, images, fonts/styles/raw assets as applicable.

Known branding assets include:

- `Resources/AppIcon/appicon.svg`
- `Resources/AppIcon/appiconfg.svg`
- `Resources/Images/buy_me_a_coffee_carenest.svg`
- `Resources/Images/carenest_mark.svg`
- `Resources/Images/carenest_mark_dark.svg`
- `Resources/Images/carenest_mark_light.svg`
- `Resources/Images/carenest_monochrome.svg`

Support/funding artwork must remain a voluntary project-support surface and must not be represented as health entitlement or medical functionality.

### Presentation/services/views/viewmodels

The MAUI tree also contains the concrete UI, ViewModel, app-navigation, secure storage, file/share/browser/calendar, app-lock, onboarding/startup, notification diagnostics, settings and other platform-composition files. Their architectural rule is consistent: UI/ViewModels depend on application/platform abstractions rather than issuing SQL or inventing medical behavior.

For page-by-page behavior see `docs/USER_GUIDE.md`, `docs/FEATURE_REFERENCE.md`, `docs/architecture/APPLICATION_FLOWS.md`, and `docs/architecture/SERVICE_BOUNDARIES.md`.

## 7. Unit-test project

Path: `tests/CareNest.UnitTests/`

Concrete test files include:

- `AppointmentAndProfileRulesTests.cs`
- `AppointmentServiceTests.cs`
- `BackupReminderCoordinatorTests.cs`
- `DocumentServiceTests.cs`
- `MedicineRulesTests.cs`
- `MedicineServiceTests.cs`
- `ProfileServiceTests.cs`
- `ReminderCoordinatorActionRecoveryTests.cs`
- `ReminderCoordinatorActionValidationTests.cs`
- `ReminderPlannerArchivedProfileTests.cs`
- `ReminderPlannerBoundaryTests.cs`
- `ReminderPlannerDstMatrixTests.cs`
- `ReminderPlannerEdgeCaseTests.cs`
- `ReminderPlannerOwnershipTests.cs`
- `ReminderPlannerPropertyTests.cs`
- `ReminderPlannerTests.cs`
- `ReminderPlannerUtcWindowTests.cs`
- `ScheduleValidationHardeningTests.cs`
- reusable test doubles under `TestDoubles/` including document-store, deterministic time, notification, reminder-coordinator and repository doubles.

The authoritative PR #56 baseline contains 122 unit tests.

## 8. Integration-test project

Path: `tests/CareNest.IntegrationTests/`

This suite validates behavior that crosses implementation boundaries, including:

- SQLite migrations/repository behavior;
- WAL/busy timeout/snapshot integrity;
- rollback/transaction behavior;
- encrypted document round-trip/tamper/key state;
- backup create/inspect/restore/wrong-password/tamper/topology/key behavior;
- chunked AEAD v2 framing and v1 read compatibility;
- report/export safety;
- reminder reconciliation behavior.

The authoritative PR #56 baseline contains 39 integration tests.

## 9. UI-contract/policy test project

Path: `tests/CareNest.UiTests/`

This suite contains source/XAML/repository policy contracts rather than claiming full target-device UI automation.

Coverage includes:

- architecture dependency direction;
- repository/source hygiene;
- required data model;
- ViewModel boundaries;
- XAML semantics/accessibility intent;
- branding/support surfaces;
- async-safety rules;
- logging privacy;
- app-lock source/crypto contracts;
- reminder UTC/snooze/reconciliation/compensation rules;
- Android receiver lifecycle;
- Windows notification timer ownership/race rules;
- backup/document/report safety contracts;
- SQLite dependency-security floor/suppression absence;
- release workflow exact-tag/manual triggers;
- Dependency Audit event-safety;
- Release Evidence provenance/failure preservation/rerun identity;
- release-preflight blocking audit behavior;
- local quality-gate clean-checkout/fail-closed behavior;
- repository-local Git setup;
- production Release Gate fail-closed matching.

The authoritative PR #56 baseline contains 124 UI-contract/policy tests.

## 10. Build scripts

Path: `build/scripts/`

- `quality-gate.sh`
- `quality-gate.ps1`
- `release-preflight.sh`
- `release-preflight.ps1`
- `setup-git.sh`
- `setup-git.ps1`

Quality/preflight scripts must fail on required native-command/dependency-audit failures. Dependency audit is not warning-only.

Git setup uses repository-local identity:

- name: `Sanskar`
- email: `sanskarin@outlook.in`

## 11. GitHub automation

Path: `.github/workflows/`

- `ci.yml` — formatting, core tests and platform Release builds.
- `codeql.yml` — CodeQL analysis.
- `dependency-review.yml` — NuGet dependency auditing and PR-specific dependency comparison where applicable.
- `release-gate.yml` — fail-closed production release checklist/risk/core-test gate.
- `release-evidence.yml` — exact-source provenance/test/dependency/checksum evidence artifact.

Production tags matching `v*` are configured to run the exact tagged commit through the required production workflows described in the release documentation.

## 12. Central build/package files

- `CareNest.sln` — solution project graph.
- `Directory.Build.props` — shared build/analyzer/audit behavior.
- `Directory.Packages.props` — centrally managed package versions, including maintained SQLite native/provider pins.
- `NuGet.config` — package source configuration.
- `.editorconfig` — source formatting/style policy.
- `.gitignore` — generated/local/secret artifact exclusions.

## 13. Root governance and project files

- `README.md` — public project entry point.
- `LICENSE` — Apache License 2.0.
- `NOTICE` — required project notice.
- `CONTRIBUTING.md` — contributor rules.
- `CODE_OF_CONDUCT.md` — community conduct.
- `SECURITY.md` — vulnerability/security policy.
- `PRIVACY.md` — user privacy statement.
- `TERMS.md` — project terms/limitations.
- `SUPPORT.md` — support channels.
- `BUY_ME_A_COFFEE.md` — voluntary project support information.
- `CHANGELOG.md` — release/change history.
- `PROJECT_STATUS.md` — current source/release status.
- `DECISIONS.md` — architectural/project decision record.
- `what_changed.md` — detailed continuation ledger.

## 14. Where to add new code

Use the lowest appropriate layer:

- reusable primitive/constant with no domain semantics → Shared;
- entity/enum/structural validation → Domain;
- use-case contract/orchestration/deterministic planner behavior → Application;
- SQLite/filesystem/crypto/report implementation → Infrastructure;
- XAML/ViewModel/navigation/platform API implementation → App;
- deterministic behavior test → UnitTests;
- persistence/crypto/filesystem integration test → IntegrationTests;
- source/XAML/architecture/repository/security/release policy → UiTests.

## 15. Forbidden shortcuts

Do not:

- put SQL directly in a ViewModel;
- make Domain/Application depend on MAUI;
- add a network/telemetry client to local-first v1 without explicit architecture/privacy/security review;
- infer dosage/treatment from medicine text;
- use broad audit/analyzer suppression to hide a real defect;
- make platform notification state and SQLite state look atomic when they are not;
- silently replace a missing encryption key that existing ciphertext depends on;
- remove legacy encrypted-format compatibility without a tested migration/recovery plan;
- commit real health records, backups, PINs, passwords, encryption keys or signing material.

## 16. Current verified source

Authoritative release-engineering verification: PR #56.

- source/base: `4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`
- marker head: `e3bc621cea05364a69abee0dadbd71a67c17bddb`
- CareNest CI #571 / `31770929379`: success
- unit: 122 passed
- integration: 39 passed
- UI-contract/policy: 124 passed
- total: 285 passed
- Android/Windows/iOS simulator/Mac Catalyst Release: success
- CodeQL #571 / `31770929382`: success
- unsuppressed Dependency Audit #41 / `31770929383`: success

See `docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md` for the exact evidence and `docs/testing/TESTING_GUIDE.md` for test-layer details.
