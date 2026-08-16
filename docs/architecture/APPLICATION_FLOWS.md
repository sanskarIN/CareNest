# CareNest Application Flows

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

This document describes the principal end-to-end CareNest flows and the boundaries each flow crosses. CareNest is local-first organizational software; no flow performs diagnosis, dosage calculation/inference, treatment recommendation, clinical interaction/risk scoring, emergency-service behavior or guaranteed notification delivery.

## 1. Startup and onboarding

```text
Application launch
  -> local settings/state load
  -> app-lock gate when enabled
  -> onboarding when required
  -> initial/local profile setup
  -> shell/dashboard
```

No CareNest account/backend is required. Onboarding presents privacy/medical/reminder limitations and does not treat notification permission as a clinical requirement.

## 2. Profile flow

```text
Profiles
  -> create/edit local PersonProfile
  -> validate structural data
  -> persist through application/repository boundary
  -> refresh dependent UI
```

Archive preserves data while changing active behavior. Destructive delete coordinates local structured data, document/photo cleanup and future reminder reconciliation as required.

## 3. Medicine flow

```text
Profile
  -> medicines list
  -> medicine editor
  -> user enters name/strength/instructions/status/stock data
  -> domain/application validation
  -> SQLite persistence
  -> reminder eligibility reconciliation
```

Strength/instructions remain opaque user-entered text. CareNest does not infer dose, frequency or treatment from medicine text.

## 4. Schedule creation/edit flow

```text
Medicine
  -> schedule editor
  -> explicit kind/date/time/time-zone values
  -> validation
  -> save schedule + schedule-time data
  -> reminder coordinator rebuild/reconcile
```

Supported concepts include daily, selected weekdays, explicit times, every-N-hours, cycle, custom range and as-needed. `AsNeeded` intentionally creates no automatic occurrences.

## 5. Reminder planning flow

```text
Explicit schedule intent
  + profile/medicine/schedule state
  + UTC planning window
  -> ReminderPlanner
  -> deterministic ReminderOccurrence identities/times
```

Planner rules include ownership validation, true UTC boundaries, half-open windows, explicit time-zone/DST rules, state suppression, deterministic deduplication and no clinical inference.

## 6. Reminder persistence/platform flow

```text
Planned occurrence
  -> persist/update CareNest occurrence
  -> schedule/cancel OS request through INotificationService
  -> record platform request state
```

Database state and OS request state are separate surfaces. Reconciliation/compensation is used because they cannot be committed atomically.

## 7. Reminder rebuild/reconciliation flow

```text
Load eligible schedules + persisted occurrences
  -> plan desired future occurrences
  -> compare desired state with persisted/OS state
  -> cancel stale/invalid requests first where required
  -> persist/schedule replacements
  -> retain retryable state on platform failure
```

This flow runs after relevant edits/deletes/startup/recovery operations as designed.

## 8. Reminder action flow

For Taken/Skipped/Delayed/Missed and related handled actions:

```text
User action
  -> validate occurrence/state
  -> cancel existing OS request first where required
  -> persist handled state / medication log / configured stock effect
  -> on later failure, attempt previous-state restoration/rebuild
  -> surface privacy-safe result/error
```

Handled state is an organizational record, not proof of medication ingestion/adherence.

## 9. Snooze flow

```text
User selects future snooze time
  -> validate explicit future UTC
  -> cancel old request
  -> persist SnoozedUntilUtc/state
  -> schedule replacement for snooze due time
  -> recovery/rebuild if later step fails
```

While validly snoozed, `SnoozedUntilUtc` is the effective due time. `ScheduledUtc` remains historical schedule identity.

## 10. Appointment flow

```text
Appointments
  -> create/edit explicit appointment details
  -> validate genuine StartsUtc + optional lead time
  -> persist appointment
  -> schedule/cancel optional platform reminder
  -> compensation if DB/platform operations diverge
```

Notification permission denial is not reported as successful scheduling. Calendar export is a separate explicit external handoff.

## 11. Document import flow

```text
User selects file
  -> CareNest reads source stream
  -> encrypt into application-owned document vault
  -> persist document metadata/tag/folder state
  -> persist privacy-minimized audit state
  -> rollback encrypted payload/metadata when later step fails
```

The app does not automatically upload document contents.

## 12. Document open/export/share flow

```text
Encrypted vault payload
  -> validate required key/material
  -> decrypt to controlled temporary/output destination
  -> explicit user open/export/share handoff
  -> cleanup CareNest-owned temporary plaintext best effort
```

After handoff, the external destination owns its copy/security/retention. Missing/corrupt required key with existing ciphertext fails closed rather than silently generating an unrelated replacement key.

## 13. Document delete flow

```text
User confirms delete
  -> coordinate metadata/encrypted payload removal
  -> record safe result/audit state
  -> report incomplete cleanup if a separate surface fails
```

Previously exported copies remain external.

## 14. Report/export flow

```text
User selects report/export
  -> application loads local records
  -> infrastructure renders CSV/PDF/JSON/other supported output
  -> formula-like CSV input neutralized where applicable
  -> staged/atomic final-file behavior
  -> explicit save/share handoff
  -> app-owned cache/temp cleanup best effort
```

Reports remain informational/organizational and do not produce clinical conclusions.

## 15. Backup creation flow

```text
User selects backup + password + destination
  -> SQLite snapshot/integrity preparation
  -> gather required document recovery state
  -> build validated backup package
  -> derive key from password
  -> authenticated encrypted write
  -> explicit external file destination
```

Backup password/file custody becomes the user's responsibility after handoff.

## 16. Restore flow

```text
User selects backup + password
  -> validate magic/version/authentication
  -> decrypt/stage
  -> validate archive topology/manifest/database/key state
  -> validate SQLite integrity
  -> replace/recover local state with rollback protection
  -> rebuild derived reminder/platform state
```

Wrong-password, tampered, truncated, trailing-data or malformed topology fails closed.

## 17. App-lock enable/verify/disable flow

Enable:

```text
User selects PIN
  -> validate PIN policy
  -> random salt
  -> PBKDF2-HMAC-SHA256 verifier
  -> secure-store material update with rollback handling
```

Verify:

```text
Entered PIN
  -> derive verifier
  -> fixed-time compare
  -> allow/deny local UI access
```

Disable/update preserves fail-closed/rollback behavior. App lock is not whole-database encryption.

## 18. Local reset/data-clear flow

```text
User explicitly confirms reset
  -> cancel/reconcile CareNest-owned reminder requests as applicable
  -> clear current CareNest-owned structured/files/secure settings state
  -> restart/onboard as designed
```

External exports, screenshots, calendar copies, device backups and manually retained backup files are outside CareNest deletion control.

## 19. External repository/legal/support flow

```text
User selects fixed repository/creator/privacy/terms/security/support destination
  -> platform browser/external navigation
  -> external provider boundary
```

Ordinary fixed links do not automatically attach local health/profile/document/reminder/backup/app-lock data.

The distributed application does **not** expose the external Buy Me a Coffee funding destination. Repository-only voluntary project support is outside the current app runtime.

## 20. Store/package inspection flow

```text
Exact source head
  -> normal/store-candidate platform builds
  -> internal Android/Windows/Apple inspection outputs
  -> fail-closed forbidden-marker payload scan
  -> checksums/provenance
  -> internal artifact upload
```

Inspection outputs are engineering evidence, not production-signed/store-approved packages.

## 21. Production release flow

```text
Source-complete candidate
  -> exact-source CI/CodeQL/Dependency Audit
  -> Store Package Configuration
  -> Store Inspection Artifacts
  -> real-device/manual + accessibility + packaged compatibility
  -> production signing outside Git
  -> final signed-package inspection/checksums/provenance
  -> current store-policy/metadata review
  -> exact immutable v* tag
  -> tagged CI + CodeQL + Dependency Audit
     + Store Package + Store Inspection
     + Release Gate + Release Evidence
  -> publication
```

A green source build or tag creation alone is not production approval.

## 22. Current automated reference

PR #74 frozen source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Verified 331/331 core tests plus all configured normal target, store-candidate, inspection, CodeQL and unsuppressed Dependency Audit gates.

## Related documents

- `docs/architecture/ARCHITECTURE.md`
- `docs/architecture/SERVICE_BOUNDARIES.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`
- `docs/privacy/DATA_LIFECYCLE.md`
- `docs/releases/RELEASE_PROCESS.md`