# CareNest Architecture

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

CareNest is a local-first .NET MAUI organizational health application. Its architecture separates domain rules, application orchestration, persistence/encryption, UI and platform integrations so reminder/data/privacy behavior can be tested without turning the product into clinical decision support.

## 1. Architectural goals

- local-first operation without required CareNest backend/account;
- deterministic organizational reminder planning from explicit user input;
- clear persisted-state versus OS-scheduler boundaries;
- SQLite repository isolation from UI/ViewModels;
- separately encrypted document payloads;
- password-encrypted manual backups;
- optional app-lock privacy barrier;
- explicit export/share/calendar/browser handoff boundaries;
- privacy-minimized diagnostics;
- multi-target MAUI UI/platform adapters;
- fail-closed dependency/security/release automation;
- no dosage/treatment/clinical interaction/risk inference.

## 2. Solution layers

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

### Shared

Cross-cutting constants/primitives/helpers without MAUI/persistence implementation coupling.

### Domain

Framework-independent entities/enums/structural rules. Domain validation checks shape/ownership/state but must not become medical advice.

### Application

Use-case contracts/services, reminder planning/coordinating, ownership/time validation and compensation/recovery logic. Designed to remain testable without MAUI host/SQLite implementation.

### Infrastructure

SQLite database/migrations/repositories, encrypted document storage, backup/restore, reports/exports and platform-neutral cryptographic/persistence implementation.

### App

MAUI composition, XAML/ViewModels/navigation, platform services, notifications, secure storage/preferences, file/share/calendar/browser integrations and Android/iOS/Mac Catalyst/Windows adapters.

## 3. Application targets

- Android `net10.0-android`, minimum API 24;
- iOS `net10.0-ios`, minimum iOS 15;
- Mac Catalyst `net10.0-maccatalyst`, minimum 15;
- Windows `net10.0-windows10.0.19041.0`, minimum 10.0.19041.0.

`CareNestTargetFramework` isolates one app TFM on target-specific hosts/jobs.

## 4. UI/presentation boundary

XAML/ViewModels own presentation state/navigation/commands and delegate business/persistence/platform work through services.

Current policy prevents ViewModels from issuing direct SQL or casually creating runtime network clients.

Strict compiled XAML binding policy requires:

- root page `x:DataType`;
- item-specific DataTemplate `x:DataType`;
- typed picker display bindings;
- typed explicit Source/ancestor bindings;
- Source binding compilation;
- strict XAML compilation;
- `XC0022`–`XC0025` as errors.

## 5. Domain/data model

Core concepts include:

- PersonProfile;
- EmergencyContact;
- Medicine;
- MedicineSchedule;
- ScheduleTime;
- ReminderOccurrence;
- MedicationLogEntry;
- Appointment;
- CareDocument;
- Tag/DocumentTag;
- StockAdjustment;
- AppSetting/AuditEntry/BackupMetadata.

Exact schema/relationships/migrations are documented in `DATABASE_SCHEMA.md`.

## 6. Reminder architecture

Reminder scheduling crosses three independent state surfaces:

1. explicit user schedule intent;
2. persisted CareNest occurrence state;
3. operating-system scheduled request state.

Because DB and OS scheduler cannot share one transaction, CareNest uses deterministic planning, persisted occurrence identity, reconciliation, cancellation-first ordering and compensation/rebuild rather than a single `scheduled=true` flag.

## 7. Reminder planner

Platform-neutral planner inputs include explicit schedule/profile/medicine state, UTC planning window and stored time-zone context.

Planner invariants include:

- true UTC boundaries;
- half-open windows;
- ownership validation;
- known schedule/weekday values;
- active/archive suppression;
- deterministic DST gap/overlap rules;
- stable occurrence identity/deduplication;
- AsNeeded creates no automatic occurrence;
- no medical schedule inference.

## 8. Reminder coordinator/reconciliation

Coordinator bridges persistence and platform scheduler.

Current invariants include:

- persisted ↔ platform request reconciliation;
- stale request cancellation;
- cancellation before replacement/suppression/invalidation;
- cancellation-first Taken/Skipped/Delayed/Missed transitions;
- valid snooze uses explicit future UTC;
- `SnoozedUntilUtc` is effective due time;
- cancellation failures remain retryable;
- previous-state restoration/rebuild attempts after later failure;
- profile/medicine/schedule/appointment lifecycle cleanup.

## 9. Date/time architecture

- application planner/appointment/snooze UTC boundaries require real UTC values;
- schedules retain explicit local-time/time-zone intent;
- invalid DST-gap local time is not silently invented/replaced;
- ambiguous fall-back handling is deterministic;
- device time-zone changes do not silently rewrite stored schedule intent.

## 10. Persistence architecture

SQLite implementation is isolated in Infrastructure behind repository/application contracts.

Design includes:

- ordered schema migrations;
- version/integrity handling;
- parameterized repository operations;
- transactions for consistency-sensitive multi-step writes;
- WAL/busy-timeout/snapshot behavior;
- cleanup/compensation where DB state interacts with filesystem/platform state.

CareNest does not claim transparent whole-database encryption.

## 11. SQLite dependency architecture

Current maintained package path is centrally pinned/audited with no former exact advisory suppression.

Dependency security and packaged existing-data compatibility are separate release gates.

See `docs/security/DEPENDENCY_RISK_REGISTER.md`.

## 12. Document-vault architecture

Document metadata is structured local data; imported payload bytes use separate application-owned encrypted storage.

Current protections include:

- secure-stored master key where applicable;
- AES-256-GCM;
- chunked framing v2 for new writes;
- authenticated terminal/truncation/trailing-data checks;
- retained documented v1 read compatibility;
- fail-closed missing/corrupt required key;
- import/export/delete compensation/cleanup.

Explicit export creates a copy outside the encrypted vault boundary.

## 13. Backup/restore architecture

Manual backup packages local data/document recovery state into a password-encrypted authenticated format.

Restore validates authentication/version/topology/database integrity/key state before accepting data and rebuilds derived platform state as required.

Wrong-password/tamper/truncation/trailing-data/malformed topology fails closed.

## 14. App-lock architecture

Optional local app lock stores derived verifier/salt in secure storage where applicable, uses PBKDF2-HMAC-SHA256/fixed-time comparison and fail-closed state validation/rollback.

It is a UI privacy barrier, not database/device encryption.

## 15. Reports/export architecture

Reports/exports are explicit user actions.

Infrastructure generates portable output with safe staging/cleanup/formula-like CSV neutralization as documented. Once a destination receives a copy, it leaves CareNest-controlled storage/security.

## 16. Platform-services boundary

Application contracts abstract services such as:

- notification scheduling/cancellation/permission/diagnostics;
- secure storage/preferences;
- file picker/share;
- calendar export;
- browser/external navigation;
- platform app-data paths/time/restart capabilities as implemented.

Platform adapters own target-specific APIs; application/domain remain platform-neutral.

## 17. Android boundary

Android adapter handles manifest/application/activity/notification/alarm/broadcast integration.

Delivery remains subject to permission, alarm capability, battery/vendor policy, force-stop, reboot and clock/time-zone behavior.

## 18. Windows boundary

Windows adapter provides desktop integration and current in-process reminder fallback. Source tests protect timer replacement/cancellation ownership, but reliable closed-app delivery is not claimed.

## 19. Apple boundary

iOS/Mac Catalyst adapters use platform notification/file/share behavior. Simulator/unsigned compilation is automated evidence; real-device/signed/notarized behavior remains manual/external release evidence.

## 20. Local-first network boundary

Normal v1 runtime has no required CareNest account/backend/automatic sync/hidden analytics client.

Future networked features require explicit authentication/authorization/consent/privacy/key-management/deletion/export/offline/conflict/threat-model/store design.

## 21. External link boundary

Application can explicitly open normal repository/creator/legal/support destinations as implemented.

The distributed app does **not** include/expose the external Buy Me a Coffee funding destination. Repository-only project support is outside current app runtime and does not receive local health data automatically.

## 22. Logging boundary

Diagnostics must minimize health/document/credential/crypto content. Sensitive paths avoid raw exception messages/stack traces/paths when unnecessary.

See `docs/security/LOGGING_PRIVACY.md`.

## 23. Security/release architecture

Automated controls include:

- unit/integration/UI-source-policy tests;
- strict XAML build policy;
- CodeQL;
- unsuppressed Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts with fail-closed marker scan/provenance;
- Release Gate;
- Release Evidence.

Production signing secrets remain outside Git.

## 24. Application funding/package invariant

Current distributed application invariant:

- no BMC destination/card/command/artwork in app source/package;
- no `CareNestShowFundingLink` build property;
- repository-only optional support;
- funding never changes health/reminder/medical behavior;
- package scanner remains defense-in-depth.

## 25. Testing architecture

- Unit: deterministic domain/application/service behavior.
- Integration: SQLite/encryption/filesystem/backup/report behavior.
- UI/source-policy: XAML/architecture/privacy/async/release/package source contracts.
- Manual: real OS/device/accessibility/package/signing/store behavior.

Current PR #74 automated count: 331/331.

## 26. Current release evidence

Current verified source:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

PR #74 passed all configured normal platform, store-candidate, inspection, CodeQL and Dependency Audit gates.

Production release still requires real-device/package/accessibility/signing/store/tag evidence.

## Related documents

- `docs/architecture/APPLICATION_FLOWS.md`
- `docs/architecture/SERVICE_BOUNDARIES.md`
- `docs/architecture/DATABASE_SCHEMA.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/architecture/DOCUMENT_VAULT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/privacy/PRIVACY_MODEL.md`