# CareNest bug-audit regression matrix — 2026-08-14

This matrix maps the major defects/failure modes discovered during the 2026-08-14 source audit to their production fix surface and automated regression evidence.

CareNest remains an organizational product. These tests validate software correctness and privacy boundaries; they do not validate clinical outcomes.

| Area | Failure mode protected | Primary source | Regression evidence |
|---|---|---|---|
| App lock | Partial PIN secure-store write leaves mixed old/new material | `src/CareNest.App/Services/AppLockService.cs` | `tests/CareNest.UiTests/AppLockSecurityContractTests.cs` |
| App lock | Partial disable leaves inconsistent enabled/salt/verifier state | `AppLockService.cs` | `AppLockSecurityContractTests.cs` |
| App lock | Corrupt verifier length controls PBKDF output or bypasses validation | `AppLockService.cs` | `AppLockSecurityContractTests.cs` |
| App lock | Mutable sensitive buffers remain uncleared in application-owned memory | `AppLockService.cs` | `AppLockSecurityContractTests.cs` |
| Document vault | Read/export silently creates a replacement key | `EncryptedDocumentStore.cs` | document-key integration/contract coverage |
| Document vault | Existing encrypted payload + missing/corrupt key silently forks the vault | `EncryptedDocumentStore.cs` | document-key integration coverage |
| Document export | Failed decrypt/export/audit leaves plaintext temp file | `DocumentService.cs` | document service unit coverage |
| Document export | Successful decrypted temp file is outside managed Clear Cache location | `DocumentsViewModel.cs` | `DocumentExportCacheContractTests.cs` |
| Profile delete | First encrypted-file cleanup error prevents later cleanup attempts | `ProfileService.cs` | profile service deletion tests |
| Profile photo | Persisted encrypted image deleted before profile save commits replacement | `ProfileEditorViewModel.cs` | `ProfilePhotoLifecycleContractTests.cs` |
| Profile photo | Failed staged replacement strands newly imported encrypted payload | `ProfileEditorViewModel.cs` | `ProfilePhotoLifecycleContractTests.cs` |
| Profile photo | Partial plaintext preview becomes final preview | `ProfileEditorViewModel.cs` | `ProfilePhotoLifecycleContractTests.cs` |
| Profile photo | Instance semaphore triggers CA1001/disposal ambiguity | `ProfileEditorViewModel.cs` | `ProfilePhotoLifecycleContractTests.cs` + CI analyzers |
| Onboarding | Invalid optional PIN creates profile before validation fails | `OnboardingViewModel.cs` | `OnboardingRollbackContractTests.cs` |
| Onboarding | Failure leaves profile/lock/completion in partial state | `OnboardingViewModel.cs` | `OnboardingRollbackContractTests.cs` |
| Migrations | DDL succeeds/fails independently from `SchemaInfo` version | `SqliteDatabase.cs` | migration transaction contracts/integration tests |
| Repository | Primary-profile write partly updates multiple rows | `CareNestRepository.cs` | repository transaction integration tests |
| Repository | Schedule write leaves schedule/time mismatch | `CareNestRepository.cs` | repository transaction integration tests |
| Repository | Cascade/tag/contact multi-step delete partly commits | `CareNestRepository.cs` | `RepositoryTransactionContractTests.cs` |
| Repository | Transaction helper violates analyzer cancellation-token convention | `CareNestRepository.cs` | CI analyzers + `RepositoryTransactionContractTests.cs` |
| Reset | Structured clear commits then `VACUUM` failure blocks later privacy cleanup | `CareNestRepository.cs` | `RepositoryTransactionContractTests.cs` |
| Medication log | Mutation calls busy-guarded `LoadAsync` while already busy | `MedicationLogViewModel.cs` | ViewModel refresh contracts |
| Medication log | Undefined enum persisted by manual edit | `MedicationLogViewModel.cs` | `MedicationLogInputContractTests.cs` |
| Documents | Mutation calls nested busy-guarded refresh | `DocumentsViewModel.cs` | ViewModel source contracts |
| Reminder action | `Scheduled`/undefined enum accepted as user action | `ReminderCoordinator.cs` | reminder action validation unit tests |
| Android | `BroadcastReceiver` returns before async rebuild completes | Android notification service | `AndroidReceiverLifecycleContractTests.cs` + Android Release build |
| Windows | Scheduled timer linked to short-lived caller token | Windows notification service | `WindowsNotificationTimerContractTests.cs` + Windows Release build |
| Windows | Old timer removes newer same-ID timer from dictionary | Windows notification service | `WindowsNotificationTimerContractTests.cs` |
| Windows | CTS disposed before not-yet-running task captures token | Windows notification service | `WindowsNotificationTimerContractTests.cs` |
| Backup | Finished encrypted backup reported failed due metadata bookkeeping | `EncryptedBackupService.cs` | `BackupCompletionSemanticsContractTests.cs` |
| Backup | Finished restore reported failed due post-restore audit | `EncryptedBackupService.cs` | `BackupCompletionSemanticsContractTests.cs` |
| Backup | Failed restore does not restore exact pre-existing key bytes | `EncryptedBackupService.cs` | `BackupCompletionSemanticsContractTests.cs` |
| Backup logging | Post-success bookkeeping logs full exception/details | `EncryptedBackupService.cs` | `BackupCompletionSemanticsContractTests.cs` + logging policies |
| CSV | User text executes as spreadsheet formula after export | `CsvWriter.cs` | `ReportExportTests.cs` |
| CSV | Failed/cancelled write leaves partial final CSV | `CsvWriter.cs` | `ReportExportSafetyContractTests.cs` |
| PDF | Failed/cancelled write leaves partial final PDF | `SimplePdfWriter.cs` | `ReportExportSafetyContractTests.cs` |
| JSON | Failed/cancelled profile export leaves partial final JSON | `ReportService.cs` | `ReportExportSafetyContractTests.cs` |
| Reports | Selected profile object remains stale after refresh | `ReportsViewModel.cs` | `ReportExportSafetyContractTests.cs` |
| Recurrence | Every-N-hours DST-gap anchor silently shifts +1 hour | `ReminderPlanner.cs` | `ReminderPlannerEdgeCaseTests.cs` |
| Recurrence | Extreme cycle day values overflow integer arithmetic | `ReminderPlanner.cs` | `ReminderPlannerEdgeCaseTests.cs` |
| Recurrence | `DateTime.MaxValue` end boundary overflows interval scheduling | `ReminderPlanner.cs` | `ReminderPlannerEdgeCaseTests.cs` |
| Startup | First recovery failure prevents all later recovery operations | `StartupCoordinator.cs` | `StartupRecoveryContractTests.cs` |
| Startup logging | Recovery logs health content/full exception detail | `StartupCoordinator.cs` | `StartupRecoveryContractTests.cs` + logging privacy contracts |
| Reminder reconciliation | Future snooze disappears when original scheduled time is past | `ReminderCoordinator.cs` | `ReminderReconciliationBehaviorTests.cs` |
| Reminder reconciliation | Overdue snooze is never marked missed | `ReminderCoordinator.cs` | `ReminderReconciliationBehaviorTests.cs` |
| Reminder reconciliation | Schedule edit deletes DB rows but leaves stale OS alarm | `ReminderCoordinator.cs` + `MedicineService.cs` | `ReminderReconciliationContractTests.cs`, behavior integration tests |
| Reminder reconciliation | Quiet-hour/state change declines new schedule but leaves old OS alarm | `ReminderCoordinator.cs` | `ReminderReconciliationContractTests.cs` |
| Reminder reconciliation | Cancellation failure marks occurrence cancelled anyway | `ReminderCoordinator.cs` | `ReminderReconciliationContractTests.cs` |
| Medicine delete | DB cascade removes rows without cancelling future OS alarms | `MedicineService.cs` | `ReminderReconciliationContractTests.cs` |
| Profile delete | DB cascade removes rows without cancelling future OS alarms | `ProfileService.cs` | `ReminderReconciliationContractTests.cs` |
| Delete compensation | Platform cancellation succeeds but cascade fails and alarms are not restored | medicine/profile services | `ReminderReconciliationContractTests.cs` |

## CI/analyzer failures that became regression evidence

### CA1068

The transaction helper originally placed `CancellationToken` before its action parameter. PR #37 exposed CA1068. The method signature was corrected; no suppression was introduced.

### CA1001

The profile editor originally owned an instance `SemaphoreSlim` after the photo-staging race fix. PR #39 exposed CA1001. The gate was intentionally changed to app-lifetime/static ownership; no suppression was introduced.

### Formatting failures

PR #39 and PR #40 each exposed a missing final newline in newly edited C# source. Both were corrected directly. The final source passed formatting on PR #43.

## Required promotion rule

If a future change modifies any runtime, test, workflow, package, project, app resource, or platform source covered by these contracts:

1. run formatter;
2. run unit/integration/UI-contract suites;
3. build Android Release;
4. build Windows Release;
5. build iOS simulator Release;
6. build Mac Catalyst Release;
7. run CodeQL;
8. run Dependency Audit;
9. keep the SQLitePCLRaw advisory separately open until its actual dependency path is remediated;
10. never merge a verification marker into `main`.

PR #43 is the final exact-head verification reference for this audit.
