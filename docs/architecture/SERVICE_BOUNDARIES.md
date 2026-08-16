# CareNest Service and Infrastructure Boundaries

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

CareNest uses explicit project/service boundaries so local health-organizer behavior remains testable, privacy-aware and platform-independent where possible.

## 1. Project dependency boundary

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

Rules:

- Shared contains dependency-light primitives/constants/helpers.
- Domain owns entities/enums/structural rules and remains framework-independent.
- Application owns use-case contracts/orchestration and remains testable without MAUI/SQLite implementations.
- Infrastructure owns SQLite/filesystem/encryption/backup/report implementation.
- App owns MAUI UI/composition/platform integrations.
- ViewModels do not reach directly into SQLite implementation.
- Platform-neutral projects do not depend on MAUI.

## 2. Domain boundary

Domain code validates structure/ownership/state/date-time shape but must not infer diagnosis, dosage, treatment, clinical interaction or risk.

Medicine strength/instructions remain opaque user text.

## 3. Repository boundary

`ICareNestRepository` and related application contracts isolate structured persistence from use cases/UI.

Repository responsibilities include:

- CRUD/query operations;
- transactions/consistency support;
- migration/schema-backed persistence;
- reminder occurrence/log/appointment/document metadata/stock/settings state;
- database snapshot/integrity behavior needed by backup flows.

Consumers should not depend on `SQLiteAsyncConnection` or concrete persistence implementation directly.

## 4. Reminder planner boundary

`ReminderPlanner` is platform-neutral deterministic logic.

Inputs:

- explicit profile/medicine/schedule data;
- explicit UTC planning window;
- stored time-zone/date/recurrence values.

Outputs:

- desired organizational reminder occurrences.

It does not schedule OS notifications or make medical decisions.

## 5. Reminder coordinator boundary

`IReminderCoordinator`/`ReminderCoordinator` bridges:

- desired reminder plan;
- persisted occurrence/log state;
- platform notification scheduler.

Responsibilities include rebuild/reconciliation, stale request cleanup, handled states, snooze/effective-due behavior and compensation/recovery.

It must treat DB and OS scheduler as separate failure surfaces.

## 6. Notification-service boundary

Platform `INotificationService` implementations own:

- permission/capability checks;
- scheduling/cancellation through target OS;
- test notification/diagnostics where supported;
- platform-specific request identifiers/constraints.

Application/domain code must not assume an OS request is guaranteed to deliver.

Android/Apple/Windows implementations can differ while honoring common application contracts.

## 7. Appointment-service boundary

`AppointmentService` owns application-level validation/orchestration for appointment CRUD and optional reminders.

Platform calendar export is a separate explicit external handoff and is not part of SQLite persistence itself.

## 8. Profile-service boundary

`ProfileService` coordinates profile lifecycle with dependent records, reminders, documents/photos and audit state through abstractions.

Delete/archive behavior must preserve explicit semantic differences and coordinate cleanup/recovery across separate state surfaces.

## 9. Medicine-service boundary

`MedicineService` coordinates medicine/schedule/stock/reminder use cases.

It must not parse medicine strength/instructions to infer a schedule/dose/treatment.

## 10. Document-service boundary

`DocumentService` coordinates:

- file selection/import intent;
- encrypted document storage abstraction;
- metadata persistence;
- export/share/delete;
- rollback/audit behavior.

Encryption/filesystem implementation belongs in Infrastructure/platform abstractions, not ViewModels.

## 11. Document-storage boundary

Infrastructure document storage owns application-controlled encrypted payload bytes and related staging/cleanup.

It is distinct from:

- SQLite metadata;
- user-selected external export destination;
- operating-system share target;
- manually retained external copy.

## 12. Cryptographic boundary

Cryptographic helpers/services own documented key derivation/authenticated framing/key validation behavior.

Callers receive success/failure through service contracts rather than reimplementing crypto in ViewModels/UI.

Format/key changes are compatibility/security architecture changes.

## 13. Backup boundary

Backup services coordinate snapshot/package/encryption/restore validation and rollback.

Responsibilities include:

- SQLite snapshot/integrity;
- required encrypted-document recovery material;
- package topology validation;
- password-derived encryption;
- restore staging/rollback;
- rejection of wrong-password/tampered/truncated/malformed input.

External backup destination/retention is outside CareNest control after handoff.

## 14. Report/export boundary

Report infrastructure turns local records into supported portable output.

It owns safe rendering/staging/cleanup/formula-like CSV neutralization where applicable. UI owns user intent/destination flow; external destination owns the final copy after handoff.

## 15. Secure storage boundary

App/platform secure-storage abstraction owns small secret/configuration material such as document master key and app-lock derived material where applicable.

Do not store raw health documents or plaintext PIN/passwords there.

## 16. Settings/preferences boundary

Preferences/settings abstractions own non-secret application configuration. Sensitive key material belongs in secure storage rather than ordinary preferences.

## 17. File picker/share boundary

Platform file/share abstractions own target OS interaction.

Application services decide what data is safe/ready to hand off. Once external transfer occurs, CareNest cannot enforce remote retention/security.

## 18. Calendar boundary

Calendar export is explicit user action. The OS/provider can sync appointment data remotely under its own policy; CareNest does not silently create a cloud caregiver record.

## 19. Browser/external navigation boundary

The app can explicitly open fixed normal destinations such as repository/creator/privacy/terms/security/support where implemented.

Fixed web actions must not attach local health/profile/document/reminder/backup/app-lock data.

The distributed CareNest application does **not** include/expose the external Buy Me a Coffee project-support destination. That URL is repository-documentation-only.

## 20. Logging boundary

Application/infrastructure/platform logging must use privacy-minimized context and avoid raw health content, documents, backup passwords, PINs, cryptographic/signing keys and unnecessary sensitive exception payloads.

## 21. ViewModel boundary

ViewModels own presentation orchestration:

- observable UI state;
- commands/cancellation;
- navigation prompts/dialog state through abstractions;
- validation-result presentation.

They must not own direct SQL, cryptographic algorithms, raw filesystem persistence or casual HTTP/telemetry creation.

## 22. XAML boundary

Binding-bearing XAML is compile-time typed under current strict policy:

- root `x:DataType`;
- item-specific DataTemplate types;
- typed picker display bindings;
- typed Source/ancestor bindings;
- `XC0022`–`XC0025` as errors.

This is protected by dynamic repository tests.

## 23. Platform adapter boundary

`CareNest.App/Platforms/*` owns OS-specific code for Android, iOS, Mac Catalyst and Windows.

Platform code should adapt application contracts rather than move domain rules into platform-specific classes.

## 24. Network boundary

Current v1 has no required CareNest backend/account/automatic sync/hidden analytics runtime client.

Any new network/cloud subsystem requires a first-class design for authentication, authorization, consent, privacy, key management, deletion/export, offline/conflict behavior, abuse prevention, threat model and store disclosures.

## 25. Dependency/security boundary

Central package configuration and GitHub workflows own dependency audit/version policy. Application services should not embed dependency exceptions/suppressions.

The former SQLite exact advisory suppression is removed; packaged compatibility remains a separate release validation concern.

## 26. Store/package boundary

Store Package Configuration builds candidate configurations. Store Inspection Artifacts creates internal unsigned/unpackaged/simulator evidence, scans for forbidden marker and records provenance/checksums.

These workflows do not own production signing or store approval.

## 27. Release automation boundary

Production-style `v*` tags participate in:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

Manual/device/package/accessibility/signing/store evidence remains outside pure CI automation.

## 28. Funding/package invariant

Current source boundary:

- no application BMC destination/card/command/artwork;
- no `CareNestShowFundingLink` build property;
- repository-only optional project support;
- funding never changes health/reminder/medical behavior;
- package scanner remains defense-in-depth.

## 29. Testing boundaries

- Unit tests target Domain/Application deterministic behavior.
- Integration tests target Infrastructure/persistence/crypto/filesystem behavior.
- UI/source-policy tests target XAML/architecture/privacy/async/release/package source contracts.
- Manual tests target real OS/device/accessibility/package/signing/store behavior.

Current PR #74 automated count: 331/331.

## Related documents

- `docs/architecture/ARCHITECTURE.md`
- `docs/architecture/APPLICATION_FLOWS.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/CODEBASE_REFERENCE.md`
- `docs/DEVELOPER_REFERENCE.md`
- `docs/security/SECURITY_MODEL.md`