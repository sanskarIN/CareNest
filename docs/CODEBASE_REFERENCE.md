# CareNest Codebase and API Reference

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

This reference maps the current repository layers and major source units to their responsibilities. Use the source tree itself as the final authority for exact filenames if later changes add/rename files.

## 1. Dependency direction

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

Tests depend on the layers they validate. Platform-neutral projects must not gain MAUI dependencies.

## 2. `CareNest.Shared`

Path: `src/CareNest.Shared/`

Important shared units include:

- `CareNest.Shared.csproj` — shared project definition;
- `AppConstants.cs` — current product/repository/contact/support/medical-limitation constants and general application defaults;
- `Guard.cs` — guard helpers;
- `Result.cs` — shared result primitive where used;
- `SecretKeys.cs` — secure-storage slot identifiers, not secret values;
- `SettingKeys.cs` — settings keys;
- `TimeProviderExtensions.cs` — time-provider helper behavior.

`AppConstants` currently contains no external Buy Me a Coffee destination. The application funding surface was removed from runtime/package source.

Shared code should remain dependency-light and free from MAUI/persistence implementation details.

## 3. `CareNest.Domain`

Path: `src/CareNest.Domain/`

### Common

- `Common/EntityBase.cs` — common identity/timestamp-style entity state.

### Core entities

- `Entities/AppSetting.cs` — persisted app setting record;
- `Entities/Appointment.cs` — appointment data including UTC scheduling fields;
- `Entities/AuditEntry.cs` — privacy-minimized audit/event record;
- `Entities/BackupMetadata.cs` — backup metadata/history;
- `Entities/CareDocument.cs` — encrypted-document metadata; payload bytes live outside normal metadata storage;
- `Entities/DocumentTag.cs` — document/tag relationship;
- `Entities/EmergencyContact.cs` — user-entered contact information;
- `Entities/MedicationLogEntry.cs` — medication/reminder history state;
- `Entities/Medicine.cs` — user-entered medicine information; strength/instruction text remains opaque;
- `Entities/MedicineSchedule.cs` — explicit schedule configuration;
- `Entities/PersonProfile.cs` — local profile/person;
- `Entities/ReminderOccurrence.cs` — materialized reminder state/platform-request association;
- `Entities/ScheduleTime.cs` — explicit schedule clock time;
- `Entities/StockAdjustment.cs` — user-entered stock/refill adjustment;
- `Entities/Tag.cs` — reusable local tag.

### Enums/rules

- `Enums/DomainEnums.cs` — recognized lifecycle/schedule/reminder/log enum values;
- `Rules/AppointmentRules.cs` — appointment structural validation including UTC/time-zone shape;
- `Rules/MedicineRules.cs` — medicine/schedule structural validation;
- `Rules/ProfileRules.cs` — profile validation.

Domain rules must reject structurally invalid input without becoming a diagnostic/dosage/treatment engine.

## 4. `CareNest.Application`

Path: `src/CareNest.Application/`

### Contracts

Important interfaces include:

- `Contracts/ICareNestRepository.cs` — structured repository abstraction;
- `Contracts/IInfrastructureServices.cs` — infrastructure/platform service abstractions;
- `Contracts/IReminderCoordinator.cs` — reminder coordination contract;
- `Contracts/IUseCaseServices.cs` — higher-level use-case contracts.

Contracts expose behavior rather than SQLite/MAUI implementation details.

### Services

Major application services include:

- `AppointmentService` — appointment save/delete/rebuild, UTC/permission validation, DB/platform compensation;
- `BackupReminderCoordinator` — backup reminder scheduling/cancellation;
- `DocumentService` — encrypted document import/export/delete orchestration and rollback/audit boundaries;
- `MedicineService` — medicine/schedule/stock/reminder orchestration;
- `ProfileService` — profile lifecycle, reminder reconciliation, document/photo cleanup and audit coordination;
- `ReminderCoordinator` — persisted occurrence ↔ OS request reconciliation, handled actions, snooze/effective-due behavior and recovery;
- `ReminderPlanner` — deterministic platform-neutral occurrence materialization from explicit schedules.

### Application invariants

- planner windows use true UTC;
- appointments use true UTC starts;
- snooze uses explicit future UTC;
- valid `SnoozedUntilUtc` is effective due time;
- stale platform requests are cancelled before replacement/suppression/invalidation;
- handled actions use cancellation-first ordering;
- platform cancellation failure remains retryable;
- DB/platform operations use compensation/recovery;
- no clinical inference is introduced.

## 5. `CareNest.Infrastructure`

Path: `src/CareNest.Infrastructure/`

Major responsibility folders:

```text
Backup/
Configuration/
Documents/
Persistence/
Reports/
Security/
```

### Persistence

Contains SQLite database initialization/migrations/repository implementation and consistency behavior.

Responsibilities include:

- schema creation/migration;
- repository operations;
- transactions;
- WAL/busy-timeout/snapshot/integrity support;
- relationship cleanup;
- safe persistence boundaries for application services.

### Documents

Contains encrypted application-owned document storage implementation and related filesystem/encryption handling.

### Backup

Contains manual backup creation/inspection/restore implementation, encrypted framing/topology validation and rollback/recovery behavior.

### Reports

Contains report/export implementations and safe output generation/cleanup behavior.

### Security

Contains infrastructure cryptographic helpers/services used by documents/backups and related secure behavior.

### Configuration

Contains infrastructure configuration/support code where required.

Infrastructure is platform-neutral relative to MAUI UI, even though it can depend on application/domain/shared abstractions and platform-neutral libraries.

## 6. `CareNest.App`

Path: `src/CareNest.App/`

Responsibilities:

- MAUI single-project composition;
- dependency injection;
- startup/shell/navigation;
- ViewModels;
- XAML pages/resources;
- platform adapters;
- notification/alarm integrations;
- file/share/calendar/browser integrations;
- secure storage/preferences/app-lock host integrations;
- Android/iOS/Mac Catalyst/Windows target code.

### Application project

`CareNest.App.csproj` declares:

- .NET MAUI single-project;
- Android/iOS/Mac Catalyst/Windows target frameworks;
- application ID/version;
- MAUI resources;
- strict compiled XAML binding policy;
- target isolation through `CareNestTargetFramework`.

## 7. App startup/composition

Important top-level units include:

- `App.xaml` / `App.xaml.cs` — application resources/lifecycle;
- `AppShell.xaml` / code-behind — shell/navigation structure;
- `MauiProgram.cs` — dependency injection/service/ViewModel/page registration;
- startup/lock/shell routing code as defined by current source.

Keep dependency registration centralized and avoid service-location logic inside ViewModels.

## 8. ViewModels

Path: `src/CareNest.App/ViewModels/`

Current ViewModels include major surfaces for:

- About;
- appointments list/editor;
- dashboard;
- documents;
- lock;
- medication log;
- medicines list/editor;
- onboarding;
- profiles list/editor;
- reports;
- schedule editor;
- settings.

`ObservableViewModel` supplies common observable/command/async state behavior where used.

ViewModel rules:

- use application/infrastructure abstractions;
- no direct `SQLiteAsyncConnection`/repository implementation reach-through;
- no casual network client creation;
- avoid prohibited `async void`/`Task.Run` patterns;
- propagate cancellation where appropriate;
- keep medical text opaque/non-clinical.

## 9. XAML views

Path: `src/CareNest.App/Views/`

Binding-bearing pages include:

- `AboutPage.xaml`;
- `AppointmentEditorPage.xaml`;
- `AppointmentsPage.xaml`;
- `DashboardPage.xaml`;
- `DocumentsPage.xaml`;
- `LockPage.xaml`;
- `MedicationLogPage.xaml`;
- `MedicineEditorPage.xaml`;
- `MedicinesPage.xaml`;
- `OnboardingPage.xaml`;
- `ProfileEditorPage.xaml`;
- `ProfilesPage.xaml`;
- `ReportsPage.xaml`;
- `ScheduleEditorPage.xaml`;
- `SettingsPage.xaml`.

`StartupPage.xaml` and shell/resource surfaces have separate roles and do not require a page ViewModel binding type when they contain no runtime bindings.

## 10. XAML binding contract

All binding-bearing pages/templates are compiled with strict type metadata:

- root page ViewModel `x:DataType`;
- item-specific DataTemplate types;
- typed picker display binding contexts;
- typed explicit Source/ancestor bindings;
- Source binding compilation enabled;
- strict XAML compilation enabled;
- `XC0022`–`XC0025` treated as errors.

`CompiledBindingContractTests` protects this dynamically.

## 11. Platform folders

`src/CareNest.App/Platforms/Android/`

- Android manifest/application/activity integration;
- notification/alarm/broadcast integration;
- platform-specific services/resources.

`src/CareNest.App/Platforms/iOS/`

- app delegate/program/info plist;
- iOS notification/platform service integration.

`src/CareNest.App/Platforms/MacCatalyst/`

- app delegate/program/info plist;
- Mac Catalyst platform service integration.

`src/CareNest.App/Platforms/Windows/`

- Windows app/package manifest/integration;
- in-process reminder fallback/platform service.

## 12. Resources

`src/CareNest.App/Resources/` contains app icon, splash, images, raw files, styles and other MAUI resources.

The external BMC destination/artwork that previously entered Windows package bytes has been removed from application resources. Do not reintroduce it casually.

## 13. Test projects

### `tests/CareNest.UnitTests`

Deterministic domain/application tests using fakes/test doubles.

### `tests/CareNest.IntegrationTests`

SQLite/encryption/filesystem/backup/report integration tests.

### `tests/CareNest.UiTests`

XAML/source/repository policy tests rather than full physical-device automation.

Current PR #74 counts: 122 + 39 + 170 = 331.

## 14. Build scripts

`build/scripts/` includes repository helpers for:

- Git identity setup;
- local quality gate;
- release preflight;
- store-package preflight;
- store-safe payload verification/scanning;
- other release/evidence support defined by the current tree.

Do not weaken fail-closed behavior to obtain green output.

## 15. GitHub workflows

`.github/workflows/` includes:

- CI;
- CodeQL;
- dependency audit/review;
- store package verification;
- store inspection artifacts;
- Release Gate;
- Release Evidence.

Production-style `v*` tag flow is documented in the release docs.

## 16. Where new work belongs

### New domain invariant

`CareNest.Domain` plus unit tests.

### New application use case/orchestration

`CareNest.Application` contracts/services plus unit tests.

### New SQLite/filesystem/crypto/report implementation

`CareNest.Infrastructure` plus integration tests.

### New MAUI screen/platform integration

`CareNest.App` plus UI/source-policy and platform/manual validation.

### New cross-cutting primitive

`CareNest.Shared` only if truly shared and dependency-light.

## 17. New network/cloud work

Do not place an HTTP client in a convenient layer and call the feature complete. Current v1 is local-first/account-free.

A networked feature requires explicit authentication, authorization, consent, key management, privacy, deletion/export, offline/conflict, threat-model and store-policy design.

## 18. Medical-safety boundary

No layer should infer or recommend dosage/treatment/clinical interaction/risk from user medicine/profile/document data.

The architecture exists to organize explicit user input, not transform it into medical advice.

## 19. Current source verification

PR #74 source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Verified 331/331 tests plus all configured target/store/security/dependency/inspection gates.

See `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.