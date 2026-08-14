# Project Status

## Release target

`1.0.0-rc.1` source-complete release candidate.

CareNest remains a local-first organizational application. It does not diagnose conditions, determine or infer dosage, recommend treatment, perform clinical interaction checking, create clinical risk scores, replace qualified professionals, or provide emergency services.

## Current automated source baseline

The latest exact-head source verification is PR #43:

`Verify final CareNest 2026-08-14 bug audit source`

Verification branch:

`ci/carenest-final-bug-audit-20260814`

Marker path:

`build/verification/final-bug-audit-20260814.txt`

PR #43 was closed without merge after all required workflow groups completed successfully. The verification marker is not part of `main`.

Final required automated gates:

- platform-neutral formatting: **success**;
- complete unit-test suite: **success**;
- complete integration-test suite: **success**;
- complete UI-contract/policy suite: **success**;
- Android Release build: **success**;
- Windows Release build: **success**;
- iOS simulator Release build: **success**;
- Mac Catalyst Release build: **success**;
- CodeQL: **success**;
- Dependency Audit: **success**.

GitHub Actions on PR #43 is the authoritative record for exact run IDs, job IDs, runner versions, timestamps, and final suite counts.

## 2026-08-14 correctness and failure-safety audit completed

The final verified source includes the following additional hardening beyond the previous PR #36 baseline.

### App lock

- transactional-style secure-store snapshot/rollback for PIN replacement;
- rollback for partial app-lock disable failures;
- clearing of application-owned salt/verifier/derived buffers;
- exact salt/verifier length checks;
- fail-closed verification for invalid PIN shape or corrupt secure material;
- no plaintext PIN persistence.

### Document vault and exports

- missing/corrupt document master key fails closed when encrypted payloads already exist;
- read/export paths never create a replacement key;
- incomplete plaintext exports are removed after failed decrypt/export/audit operations;
- successful decrypted document exports use the managed `Exports` cache directory;
- Settings Clear Cache covers those successful temporary exports.

### Profiles and photos

- profile deletion attempts every associated encrypted payload cleanup after the database cascade;
- cleanup/audit failures are aggregated instead of stopping at the first orphan;
- staged/persisted/obsolete photo references are separated;
- profile preview files use partial-file staging plus atomic move;
- failed staged replacement compensates the newly imported payload;
- profile-photo staging uses an app-lifetime/static synchronization gate;
- page disappearance performs best-effort staged cleanup without crashing navigation.

### Onboarding

- optional PIN is validated before profile creation;
- completion state is written last;
- failed setup compensates app-lock/profile/completion state with non-cancelled cleanup;
- incomplete rollback is surfaced as aggregate failure.

### SQLite migrations and repository writes

- migration DDL + schema-version update is transactional;
- primary-profile write, cascade deletes, schedule/time replacement, occurrence batches, document/tag operations, emergency-contact cleanup and full structured-record clear use transaction boundaries where multi-step consistency matters;
- transaction helper follows analyzer-required `CancellationToken` ordering;
- critical full-data clear no longer depends on `VACUUM` succeeding after the delete transaction commits.

### ViewModel refresh/input integrity

- mutation paths use non-reentrant core refresh methods instead of nesting busy-guarded `LoadAsync` calls;
- fresh profile/medicine selections are rebound by ID;
- unsupported reminder action enum values are rejected before mutation;
- undefined manual medication-log statuses are rejected before repository access.

### Android lifecycle

- boot/time/time-zone recovery uses `BroadcastReceiver.GoAsync()` and guaranteed `Finish()`;
- asynchronous receiver failures are contained so later foreground/startup recovery can retry.

### Windows lifecycle

- fallback reminder timers are not linked to short-lived caller cancellation tokens;
- cancellation and disposal ownership no longer race;
- an old timer cannot remove a newer replacement with the same occurrence ID;
- background notification display failures are contained.

### Backup and restore

- successful backup creation is not reported as failed solely because post-success local metadata recording fails;
- successful restore is not reported as failed solely because post-success audit recording fails;
- post-success bookkeeping is non-cancelled and best effort;
- bookkeeping logs only safe operation text + exception type;
- failed restore rolls the document key back to the exact prior byte state when prior bytes existed.

### Reports

- CSV string cells that look like spreadsheet formulas are neutralized before CSV escaping;
- CSV, PDF and profile JSON final paths use partial-file staging plus atomic move;
- incomplete plaintext reports are cleaned after failed writes/cancellation;
- report profile selection is rebound against freshly loaded profile records.

### Reminder planning

- invalid daylight-saving interval anchors no longer shift forward to invented clock times;
- cycle arithmetic uses widened integer math;
- maximum date boundaries do not overflow interval scheduling.

### Startup recovery

- overdue reconciliation, medicine reminder rebuild, appointment reminder rebuild and backup reminder sync run through independent recovery boundaries;
- caller cancellation still propagates;
- one non-cancellation recovery failure does not stop later recovery steps.

### Reminder platform reconciliation

- snoozed reminders use `SnoozedUntilUtc` as their effective due time;
- future snoozes remain upcoming even after the original due time passes;
- overdue snoozes can transition to missed based on their actual snooze due time;
- rebuild cancels existing platform requests before replacement/suppression/invalidation;
- stale schedule alarms are reconciled instead of only deleting SQLite rows;
- quiet-hours rebuild can cancel previously scheduled platform requests;
- cancellation failures remain retryable;
- medicine/profile delete flows cancel future platform requests before cascade deletion and attempt non-cancelled rebuild compensation if the cascade fails;
- direct integration tests cover future snooze, overdue snooze and stale future-occurrence reconciliation behavior.

## Verification checkpoint history

The audit used failure-driven marker-only checkpoints.

### PR #37

- formatting succeeded;
- unit suite reached 111 passing tests at that checkpoint;
- CA1068 exposed invalid `CancellationToken` parameter ordering in the new transaction helper;
- fixed directly; no analyzer suppression;
- PR closed unmerged.

### PR #39

- exposed CA1001 on the profile-photo `SemaphoreSlim` ownership;
- exposed a missing final newline in `ReminderPlanner.cs`;
- its marker was accidentally merged before the failed evidence was fully acted on;
- marker was explicitly removed from `main` by `549c77120c2ff792337cb842bf7a0912483816ed`;
- PR #39 is not release evidence.

### PR #40

- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL: success;
- Dependency Audit: success;
- core formatting failed only because `EncryptedBackupService.cs` lacked its final newline;
- source was corrected and PR closed unmerged rather than partially promoted.

### PR #41

- reminder-reconciliation checkpoint intentionally superseded by additional delete-flow work;
- closed unmerged.

### PR #42

- bug-audit checkpoint intentionally superseded while the behavior audit was still changing source;
- closed unmerged.

### PR #43

- final frozen source;
- all required workflow groups green;
- closed unmerged;
- current automated baseline.

## Security dependency status

The SQLite native dependency advisory remains OPEN:

`GHSA-2m69-gcr7-jv3q`

The current repository does **not** claim remediation.

- suppression is narrowly scoped to the exact advisory URL;
- no blanket severity or wildcard audit suppression is used;
- `docs/security/DEPENDENCY_RISK_REGISTER.md` remains authoritative;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` remains the required remediation/regression plan;
- final production promotion must explicitly resolve or block on this risk decision.

## Current documentation entry points

- `what_changed.md` — complete active handoff.
- `docs/releases/BUG_AUDIT_VERIFICATION_20260814.md` — complete 2026-08-14 bug-audit evidence and fix map.
- `docs/releases/PHASE9_VERIFICATION_EVIDENCE.md` — previous PR #36 Settings lifecycle evidence.
- `docs/releases/SETTINGS_LIFECYCLE_VERIFICATION_20260813.md` — previous Settings lifecycle verification details.
- `docs/testing/SETTINGS_LIFECYCLE_CONTRACT.md` — Settings lifecycle regression contract.
- `docs/security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md` — full local-data clear security model.
- `docs/security/DEPENDENCY_RISK_REGISTER.md` — dependency risk source of truth.
- `docs/releases/RELEASE_CHECKLIST.md` — release gates.
- `docs/releases/NEXT_STEPS.md` — operational remaining work.

Historical handoffs remain under `docs/history/` and in Git history. The complete previous active Phase 9 handoff remains recoverable from the pre-audit `main` history and its referenced preserved snapshots.

## Production blockers that remain real

The source is automated-release-candidate green. Public `1.0.0` production promotion still requires operational evidence that cannot be manufactured by source code alone:

- manual Android device/emulator matrix;
- manual Windows matrix;
- manual iOS/iPadOS matrix;
- manual Mac Catalyst matrix;
- notification permission denied/granted and real-delivery checks;
- Android exact/inexact alarm, battery optimization, reboot, clock and time-zone checks;
- packaged-target document import/export and profile-photo checks;
- packaged-target backup create/inspect/restore/wrong-password/tamper checks;
- legacy encrypted-format fixture verification where canonical historical fixtures are available;
- screen-reader verification;
- large-text/text-scaling verification;
- desktop keyboard/focus verification;
- contrast/theme/reduced-motion verification;
- current Apple App Store policy review for the optional external project-support link;
- current Google Play policy review for the optional external project-support link;
- signing identities and credentials outside Git;
- signed package generation and inspection;
- store screenshots/listing/privacy/data-safety metadata;
- exact promoted-commit Release Evidence workflow;
- final version/build metadata, release notes, checksums, production tag and GitHub release;
- explicit resolution/decision for the open SQLitePCLRaw dependency risk.

None of these manual/external gates is marked complete merely because automated CI is green.

## Deferred scope

Still outside current v1:

- cloud synchronization;
- remote caregiver collaboration;
- required accounts/mobile-number authentication;
- server-side health-record storage;
- silent remote sharing;
- hidden analytics/telemetry;
- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- medication-interaction claims;
- clinical risk scoring.

Any future networked feature requires a new consent/authentication/key/privacy/threat/export/store review.

## Environment truth

The repository assembly environment does not provide local MAUI device simulators, signing credentials or store submission sessions. GitHub-hosted Actions is the authoritative automated compilation/test surface for the verified source.

Manual device/accessibility/store/signing/release activities remain separate and are not claimed complete until actually performed.
