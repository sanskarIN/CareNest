# what_changed.md

## CareNest implementation record

This file is the detailed handoff requested in place of long chat messages. The implementation follows the uploaded **Master Build Prompt — CareNest** as the source of truth.

Repository: `https://github.com/sanskarIN/CareNest`  
Release target: `1.0.0-rc.1`  
Framework: .NET MAUI / .NET 10  
Primary language: C#  
Source model: Apache-2.0 open source  
Business email: `sanskarin@outlook.in`  
Support email: `supportramsandesh@gmail.com`  
Watermark: `Made by the Sanskar`

## Safety boundary implemented

CareNest remains an organizational application. It does not diagnose, determine dosage, infer a dose, recommend treatment, perform interaction checking, produce clinical risk scores, replace clinicians/pharmacists, or provide emergency services. Reminder and stock limitations are visible in product copy, reports, onboarding, About, settings, and documentation.

All reminder schedules come from explicit user input. `StockChangePerTakenEvent` is also user-entered and is never derived from medicine strength or instruction text.

## Delivery phases

### Phase 0 — repository, architecture, privacy and design foundation

Implemented:
- Multi-project solution and dependency boundaries.
- Repository standards, analyzers, central packages, editor configuration and ignores.
- Apache-2.0 project notices, contribution/security/support/privacy/terms documentation.
- ADRs, threat model, schema documentation, setup, troubleshooting, test/release plans.
- Calm original CareNest SVG branding, splash source and design-system resources.
- GitHub issue/PR templates, Dependabot, CI and CodeQL workflows.

Phase label:
`chore: establish CareNest architecture and repository standards`

### Phase 1 — domain, persistence, encryption and application services

Implemented:
- Requested domain entities and enums.
- Validation that keeps strength/instruction text opaque and never calculates dosage.
- SQLite repository and migrations through schema version 5.
- Stable reminder-occurrence keys and idempotent materialization.
- Time-zone-aware explicit schedule planning.
- Encrypted document vault using AES-256-GCM.
- Password-encrypted portable backup format v2 with authenticated encryption and restore rollback.
- The document master key is included only inside the password-encrypted backup payload so restored encrypted documents remain usable on a clean installation.
- JSON/PDF/CSV report services with non-clinical disclaimers.
- Profile, medicine, appointment, document, reminder and backup-reminder application services.

Phase label:
`feat: implement local-first domain persistence and safety services`

### Phase 2 — complete MAUI user workflows

Implemented:
- Onboarding, no-account local-first setup and optional app lock.
- Local family profiles, optional encrypted profile photo and emergency contacts.
- Medicine records and states, explicit multi-time schedules, selected weekdays, cycles, intervals, date ranges, as-needed records and follow-up reminders.
- Taken/skipped/delayed/missed/snooze workflow, manual log edits and audit history.
- Appointment organization, attachments and explicit `.ics` export.
- Encrypted document import/camera capture, folders, tags, search, selected export and deletion.
- Caregiver/local-profile dashboard with no sharing.
- Stock/refill estimates driven only by user-entered quantities.
- Per-profile complete structured JSON export, profile PDF summary and list reports.
- Settings, theme, large-interface preference, reduced motion, quiet hours, sound/vibration, app lock, backup/restore, storage maintenance and reset.
- Privacy-redacted schedule inspector, time-zone simulation and sanitized diagnostics.
- About/open-source/support/legal surfaces.

Phase label:
`feat: add CareNest MAUI workflows and accessible navigation`

### Phase 3 — platform reminder integrations and reliability

Implemented:
- Android alarm scheduling with exact/inexact fallback diagnostics, reboot/time/time-zone rebuild receiver and battery-optimization warning.
- iOS and Mac Catalyst OS-managed local notifications.
- Windows notification/fallback implementation with explicit platform limitation diagnostics.
- Startup reminder rebuild, overdue-state reconciliation and appointment/backup reminder rebuilding.
- Permission is requested when the user first explicitly creates/saves a reminder-capable feature, not during onboarding.
- Stored schedule times are not rewritten when the device time zone changes.

Phase label:
`feat: add reminders encryption backup reports and platform integrations`

### Phase 4 — tests and release gate

Implemented:
- Domain/planner unit tests including explicit multi-time scheduling, as-needed behavior, interval behavior, stable occurrence keys and ambiguous local-time handling.
- SQLite migration/cascade integration tests.
- Encrypted document round-trip/tamper tests.
- Encrypted backup restore/wrong-password/tamper tests.
- Report/export safety tests.
- XAML/UI contract tests for critical safety/navigation flows.
- GitHub Actions core, Android, Windows and Apple compilation/test matrix.
- CodeQL and Dependabot configuration.
- Release checklist and manual device matrix.

Phase label:
`test: add quality gates documentation and release readiness`


## GitHub delivery commit

The completed source tree is delivered atomically to `main` after all phases are assembled and statically validated, so GitHub Actions does not run against half-built phase snapshots.

Commit message:
`feat: build complete CareNest local-first health organizer`

The phase labels above remain the logical implementation breakdown and can be used for future history/changelog work.

## Acceptance-criteria mapping

- No account/network required: implemented; no CareNest backend, login or cloud sync exists.
- No diagnosis/treatment/dosage decisions: enforced in scope, UI language and rules.
- Reminder recovery: startup, Android boot/time/time-zone rebuild and explicit rebuild diagnostics are implemented.
- Permission/battery limitations: surfaced through settings diagnostics and product warnings.
- Profile data export/delete: JSON export and cascade deletion are implemented.
- Document export/delete: explicit selected export/deletion are implemented.
- Logs: sanitized diagnostics omit medicine/profile names, notes, contacts and document contents.
- Onboarding/About medical limitations: implemented.
- Manual encrypted backup/restore: implemented with schema and integrity validation; no automatic cloud upload.
- Local caregiver mode: implemented without silent sharing.
- Theme/accessibility/localization readiness: implemented with system/light/dark, scalable tokens, reduced-motion navigation and English resource architecture.

## Security notes

- Imported documents and profile photos use encrypted `.cndoc` storage.
- SQLite records rely on the platform application sandbox and device protection. **CareNest does not claim whole-database encryption at rest.**
- App-lock PINs use a salted PBKDF2 verifier stored through the platform secure-storage surface; plaintext PINs are not stored.
- Backups use PBKDF2-HMAC-SHA-256 plus chunked AES-256-GCM authenticated encryption.
- No API keys, signing keys, passwords, certificates or production secrets are committed.
- Exported/decrypted files leave CareNest protection when the user explicitly shares/saves them.
- No analytics or telemetry are added.

## Platform limitations documented

- Android reminder timing can be affected by notification permission, exact-alarm capability, force-stop state and battery optimization.
- iOS/Mac Catalyst delivery remains controlled by operating-system notification policy.
- The Windows implementation explicitly reports that its current fallback cannot guarantee reminder delivery when CareNest is not running; it does not pretend otherwise.
- Device shutdown, permission revocation and operating-system policy can always affect reminder delivery.

## Verification performed in this execution environment

Completed static checks:
- Every generated XAML/XML/RESX/project file parses as well-formed XML.
- Every project reference resolves to a generated project path.
- C# brace-balance scan completed across generated sources.
- Source scan found no `TODO`, `FIXME` or `NotImplementedException` placeholders.
- XAML `x:Class` declarations were matched to code-behind classes.
- Stale internal setting-key references were scanned and removed.
- A duplicated restore block that would have referenced an out-of-scope variable was found during static review and corrected before GitHub upload.
- Test projects explicitly import xUnit through project configuration.

Environment limitation:
- `dotnet --info` is unavailable in the current execution container because the .NET SDK/MAUI workloads are not installed. Therefore restore, `dotnet format`, compilation, tests, emulator/device smoke tests, signing and store packaging cannot truthfully be claimed as locally executed.
- GitHub Actions are included to run restore/build/tests on supported hosted runners. Any platform toolchain issue exposed by CI should be fixed before tagging a final `1.0.0` release.

## Build-generation incident

An early local file-generation pass had a Python string-quoting `SyntaxError`. That pass did not produce repository writes. The generator input was corrected and the project was regenerated before static validation.

## Commit identity note

The requested maintainer email is configured in:
- `build/scripts/setup-git.sh`
- `build/scripts/setup-git.ps1`
- `docs/setup/DEVELOPMENT.md`

with:

`git config user.email "sanskarin@outlook.in"`

The connected GitHub write API used for this chat does not expose author/committer email fields on its create-commit operation. Repository commits created through that connector therefore use the authenticated GitHub identity; it would be inaccurate to claim the connector forced `sanskarin@outlook.in` into commit metadata. Local/future maintainer commits can use the requested address via the included setup scripts.

## Repository file tree

```text
.editorconfig
.github/ISSUE_TEMPLATE/bug_report.yml
.github/ISSUE_TEMPLATE/feature_request.yml
.github/PULL_REQUEST_TEMPLATE.md
.github/dependabot.yml
.github/workflows/ci.yml
.github/workflows/codeql.yml
.gitignore
CHANGELOG.md
CODE_OF_CONDUCT.md
CONTRIBUTING.md
CareNest.sln
DECISIONS.md
Directory.Build.props
Directory.Packages.props
NOTICE
NuGet.config
PRIVACY.md
PROJECT_STATUS.md
README.md
SECURITY.md
SUPPORT.md
TERMS.md
build/scripts/quality-gate.ps1
build/scripts/quality-gate.sh
build/scripts/setup-git.ps1
build/scripts/setup-git.sh
docs/architecture/ADR-0001-local-first.md
docs/architecture/ADR-0002-reminder-occurrences.md
docs/architecture/ADR-0003-encrypted-backup-format.md
docs/architecture/ARCHITECTURE.md
docs/architecture/DATABASE_SCHEMA.md
docs/design/DESIGN_SYSTEM.md
docs/design/LOCALIZATION.md
docs/design/STORE_ASSETS.md
docs/privacy/DATA_LIFECYCLE.md
docs/releases/RELEASE_CHECKLIST.md
docs/security/THREAT_MODEL.md
docs/setup/DEVELOPMENT.md
docs/setup/TROUBLESHOOTING.md
docs/testing/TEST_PLAN.md
global.json
src/CareNest.App/App.xaml
src/CareNest.App/App.xaml.cs
src/CareNest.App/CareNest.App.csproj
src/CareNest.App/Converters/CommonConverters.cs
src/CareNest.App/MauiProgram.cs
src/CareNest.App/Navigation/RouteNames.cs
src/CareNest.App/Platforms/Android/AndroidManifest.xml
src/CareNest.App/Platforms/Android/MainActivity.cs
src/CareNest.App/Platforms/Android/MainApplication.cs
src/CareNest.App/Platforms/Android/PlatformNotificationService.Android.cs
src/CareNest.App/Platforms/Android/Resources/values/colors.xml
src/CareNest.App/Platforms/MacCatalyst/AppDelegate.cs
src/CareNest.App/Platforms/MacCatalyst/Info.plist
src/CareNest.App/Platforms/MacCatalyst/PlatformNotificationService.MacCatalyst.cs
src/CareNest.App/Platforms/MacCatalyst/Program.cs
src/CareNest.App/Platforms/Windows/App.xaml
src/CareNest.App/Platforms/Windows/App.xaml.cs
src/CareNest.App/Platforms/Windows/Package.appxmanifest
src/CareNest.App/Platforms/Windows/PlatformNotificationService.Windows.cs
src/CareNest.App/Platforms/iOS/AppDelegate.cs
src/CareNest.App/Platforms/iOS/Info.plist
src/CareNest.App/Platforms/iOS/PlatformNotificationService.iOS.cs
src/CareNest.App/Platforms/iOS/Program.cs
src/CareNest.App/Resources/AppIcon/appicon.svg
src/CareNest.App/Resources/AppIcon/appiconfg.svg
src/CareNest.App/Resources/Images/carenest_mark.svg
src/CareNest.App/Resources/Images/empty_state.svg
src/CareNest.App/Resources/Raw/third_party_notices.txt
src/CareNest.App/Resources/Splash/splash.svg
src/CareNest.App/Resources/Strings/AppResources.resx
src/CareNest.App/Resources/Strings/AppText.cs
src/CareNest.App/Resources/Styles/Colors.xaml
src/CareNest.App/Resources/Styles/Styles.xaml
src/CareNest.App/Services/AppLockService.cs
src/CareNest.App/Services/AppStateService.cs
src/CareNest.App/Services/MauiFileGateway.cs
src/CareNest.App/Services/MauiNavigator.cs
src/CareNest.App/Services/PlatformNotificationService.cs
src/CareNest.App/Services/SafeUiErrorService.cs
src/CareNest.App/Services/SecureSecretStore.cs
src/CareNest.App/Services/StartupCoordinator.cs
src/CareNest.App/ViewModels/AboutViewModel.cs
src/CareNest.App/ViewModels/AppointmentEditorViewModel.cs
src/CareNest.App/ViewModels/AppointmentsViewModel.cs
src/CareNest.App/ViewModels/DashboardViewModel.cs
src/CareNest.App/ViewModels/DocumentsViewModel.cs
src/CareNest.App/ViewModels/LockViewModel.cs
src/CareNest.App/ViewModels/MedicationLogViewModel.cs
src/CareNest.App/ViewModels/MedicineEditorViewModel.cs
src/CareNest.App/ViewModels/MedicinesViewModel.cs
src/CareNest.App/ViewModels/ObservableViewModel.cs
src/CareNest.App/ViewModels/OnboardingViewModel.cs
src/CareNest.App/ViewModels/ProfileEditorViewModel.cs
src/CareNest.App/ViewModels/ProfilesViewModel.cs
src/CareNest.App/ViewModels/ReportsViewModel.cs
src/CareNest.App/ViewModels/ScheduleEditorViewModel.cs
src/CareNest.App/ViewModels/SettingsViewModel.cs
src/CareNest.App/Views/AboutPage.xaml
src/CareNest.App/Views/AboutPage.xaml.cs
src/CareNest.App/Views/AppShell.xaml
src/CareNest.App/Views/AppShell.xaml.cs
src/CareNest.App/Views/AppointmentEditorPage.xaml
src/CareNest.App/Views/AppointmentEditorPage.xaml.cs
src/CareNest.App/Views/AppointmentsPage.xaml
src/CareNest.App/Views/AppointmentsPage.xaml.cs
src/CareNest.App/Views/DashboardPage.xaml
src/CareNest.App/Views/DashboardPage.xaml.cs
src/CareNest.App/Views/DocumentsPage.xaml
src/CareNest.App/Views/DocumentsPage.xaml.cs
src/CareNest.App/Views/LockPage.xaml
src/CareNest.App/Views/LockPage.xaml.cs
src/CareNest.App/Views/MedicationLogPage.xaml
src/CareNest.App/Views/MedicationLogPage.xaml.cs
src/CareNest.App/Views/MedicineEditorPage.xaml
src/CareNest.App/Views/MedicineEditorPage.xaml.cs
src/CareNest.App/Views/MedicinesPage.xaml
src/CareNest.App/Views/MedicinesPage.xaml.cs
src/CareNest.App/Views/OnboardingPage.xaml
src/CareNest.App/Views/OnboardingPage.xaml.cs
src/CareNest.App/Views/ProfileEditorPage.xaml
src/CareNest.App/Views/ProfileEditorPage.xaml.cs
src/CareNest.App/Views/ProfilesPage.xaml
src/CareNest.App/Views/ProfilesPage.xaml.cs
src/CareNest.App/Views/ReportsPage.xaml
src/CareNest.App/Views/ReportsPage.xaml.cs
src/CareNest.App/Views/ScheduleEditorPage.xaml
src/CareNest.App/Views/ScheduleEditorPage.xaml.cs
src/CareNest.App/Views/SettingsPage.xaml
src/CareNest.App/Views/SettingsPage.xaml.cs
src/CareNest.App/Views/StartupPage.xaml
src/CareNest.App/Views/StartupPage.xaml.cs
src/CareNest.Application/CareNest.Application.csproj
src/CareNest.Application/Contracts/ICareNestRepository.cs
src/CareNest.Application/Contracts/IInfrastructureServices.cs
src/CareNest.Application/Contracts/IReminderCoordinator.cs
src/CareNest.Application/Contracts/IUseCaseServices.cs
src/CareNest.Application/Services/AppointmentService.cs
src/CareNest.Application/Services/BackupReminderCoordinator.cs
src/CareNest.Application/Services/DocumentService.cs
src/CareNest.Application/Services/MedicineService.cs
src/CareNest.Application/Services/ProfileService.cs
src/CareNest.Application/Services/ReminderCoordinator.cs
src/CareNest.Application/Services/ReminderPlanner.cs
src/CareNest.Domain/CareNest.Domain.csproj
src/CareNest.Domain/Common/EntityBase.cs
src/CareNest.Domain/Entities/AppSetting.cs
src/CareNest.Domain/Entities/Appointment.cs
src/CareNest.Domain/Entities/AuditEntry.cs
src/CareNest.Domain/Entities/BackupMetadata.cs
src/CareNest.Domain/Entities/CareDocument.cs
src/CareNest.Domain/Entities/DocumentTag.cs
src/CareNest.Domain/Entities/EmergencyContact.cs
src/CareNest.Domain/Entities/MedicationLogEntry.cs
src/CareNest.Domain/Entities/Medicine.cs
src/CareNest.Domain/Entities/MedicineSchedule.cs
src/CareNest.Domain/Entities/PersonProfile.cs
src/CareNest.Domain/Entities/ReminderOccurrence.cs
src/CareNest.Domain/Entities/ScheduleTime.cs
src/CareNest.Domain/Entities/StockAdjustment.cs
src/CareNest.Domain/Entities/Tag.cs
src/CareNest.Domain/Enums/DomainEnums.cs
src/CareNest.Domain/Rules/AppointmentRules.cs
src/CareNest.Domain/Rules/MedicineRules.cs
src/CareNest.Domain/Rules/ProfileRules.cs
src/CareNest.Infrastructure/Backup/BackupManifest.cs
src/CareNest.Infrastructure/Backup/EncryptedBackupService.cs
src/CareNest.Infrastructure/CareNest.Infrastructure.csproj
src/CareNest.Infrastructure/Configuration/CareNestStorageOptions.cs
src/CareNest.Infrastructure/Documents/EncryptedDocumentStore.cs
src/CareNest.Infrastructure/Persistence/CareNestRepository.cs
src/CareNest.Infrastructure/Persistence/SchemaInfo.cs
src/CareNest.Infrastructure/Persistence/SqliteDatabase.cs
src/CareNest.Infrastructure/Reports/CsvWriter.cs
src/CareNest.Infrastructure/Reports/ReportService.cs
src/CareNest.Infrastructure/Reports/SimplePdfWriter.cs
src/CareNest.Infrastructure/Security/ChunkedAead.cs
src/CareNest.Shared/AppConstants.cs
src/CareNest.Shared/CareNest.Shared.csproj
src/CareNest.Shared/Guard.cs
src/CareNest.Shared/Result.cs
src/CareNest.Shared/SecretKeys.cs
src/CareNest.Shared/SettingKeys.cs
src/CareNest.Shared/TimeProviderExtensions.cs
tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj
tests/CareNest.IntegrationTests/DatabaseMigrationTests.cs
tests/CareNest.IntegrationTests/EncryptedBackupTests.cs
tests/CareNest.IntegrationTests/EncryptedDocumentStoreTests.cs
tests/CareNest.IntegrationTests/ReportExportTests.cs
tests/CareNest.IntegrationTests/TestInfrastructure.cs
tests/CareNest.UiTests/CareNest.UiTests.csproj
tests/CareNest.UiTests/CriticalFlowContractTests.cs
tests/CareNest.UiTests/RepositoryLocator.cs
tests/CareNest.UnitTests/AppointmentAndProfileRulesTests.cs
tests/CareNest.UnitTests/CareNest.UnitTests.csproj
tests/CareNest.UnitTests/MedicineRulesTests.cs
tests/CareNest.UnitTests/ReminderPlannerTests.cs
what_changed.md
```

## Release rule

Do not call CareNest “bug-free.” A release is acceptable only after the automated GitHub quality gates pass and the manual platform checks in `docs/releases/RELEASE_CHECKLIST.md` are completed on actual target devices/OS versions.
