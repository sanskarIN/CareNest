# Test Plan

## Latest exact automated baseline

Exact runtime/test source verified through marker-only PR #33:

`4f5f9abe9d702fa33d6aba3f15c113febfebf95e`

Automated evidence:

- CareNest CI #332 / `31691592300`: success;
- platform-neutral formatting: success;
- unit tests: **106 passed, 0 failed, 0 skipped**;
- integration tests: **30 passed, 0 failed, 0 skipped**;
- UI-contract/policy tests: **54 passed, 0 failed, 0 skipped**;
- total core automated tests: **190 passed**;
- Android Release build: success;
- Windows Release build: success;
- iOS simulator Release build: success;
- Mac Catalyst Release build: success;
- CodeQL #332 / `31691592435`: success;
- Dependency Audit #13 / `31691592302`: success.

PR #33 added only `build/verification/rc1-aead-v2-hardening-20260813.txt` beyond that source head and was closed without merge after all gates passed.

The Dependency Audit result does not mean the tracked SQLitePCLRaw advisory is fixed. `docs/security/DEPENDENCY_RISK_REGISTER.md` remains authoritative.

## Domain and scheduling automation

- Domain validation and schedule planner edge cases.
- Explicit validation for selected weekdays, cycle on/off values, end-date ordering, clock-time ranges, every-N-hours starting-time count, unknown schedule enum values, unsupported weekday-mask bits, blank/unknown time-zone identifiers, and every-N-hours interval limits.
- Reminder planner ownership checks for profile → medicine → schedule → persisted schedule-time relationships.
- Explicit UTC-kind enforcement for reminder planning windows and coordinator rebuild overrides.
- DST/time-zone planner behavior using explicit time-zone identifiers where available.
- Representative North America, Europe, Australia, and New Zealand DST gap/overlap coverage when the test host exposes those zone identifiers.
- Stable reminder occurrence keys/idempotency.
- Half-open reminder planning windows (`fromUtc` inclusive, `toUtc` exclusive).
- Duplicate explicit reminder-time deduplication and chronological output ordering.
- Deterministic randomized/property-style recurrence coverage using a fixed seed.
- Daily, selected-weekday, cycle, custom date-range, every-N-hours, follow-up, disabled, archived-profile, paused, completed, archived-medicine, and as-needed scheduling behavior.
- Invalid spring-forward local times do not cause CareNest to invent an alternate reminder time.
- Ambiguous fall-back local times create one deterministic occurrence for the same schedule/window.
- Snooze actions require an explicit future UTC timestamp before persistence/platform scheduling.
- Stock arithmetic uses only user-entered adjustments.

See [`REMINDER_SCHEDULING_CONTRACT.md`](REMINDER_SCHEDULING_CONTRACT.md) for the exact non-clinical reminder-planning invariants protected by the unit suite.

## Application-service automation

Direct platform-neutral service tests now cover behavior independently from MAUI and SQLite integration:

- `ProfileService`: create/update audit action, UTC touch time, cascading profile cleanup coordination, encrypted document/profile-photo cleanup.
- `MedicineService`: create/update audit action, reminder rebuild after medicine changes, schedule persistence/rebuild, future occurrence invalidation, explicit stock adjustments, prevention of negative estimated stock, cascade deletion.
- `AppointmentService`: explicit UTC start requirement, scheduling from the stored UTC instant, create audit behavior, platform reminder cancellation/deletion, denied-notification-permission behavior, rebuild behavior without permission prompts, stored non-UTC data fails closed.
- `DocumentService`: encrypted import metadata, audit creation, rollback when database save fails, rollback of both database record and encrypted payload when audit persistence fails, explicit export audit, safe exported filename handling, idempotent missing-record deletion.
- `BackupReminderCoordinator`: disabled-state cancellation, denied permission behavior, no background permission prompt during rebuild/sync, reminder scheduling from last backup/current time, overdue backup reminder recovery, sound/vibration preference handling.

Reusable test doubles under `tests/CareNest.UnitTests/TestDoubles/` provide deterministic repository, clock, reminder, notification, and encrypted-document-store behavior.

## Appointment time and permission assertions

- `Appointment.StartsUtc` must have `DateTimeKind.Utc`.
- Local/unspecified appointment clock values are rejected instead of being relabeled as UTC.
- Time-zone identifiers are trimmed and validated.
- Appointment reminder scheduling does not continue when permission remains denied.
- Rebuild does not trigger a new permission prompt and does not attempt platform scheduling while permission is denied.

## Encrypted document-vault automation

- Document encryption round-trip.
- Ciphertext tamper rejection.
- New encrypted documents record encryption stream format version **2**.
- Caller-owned copies of the 32-byte document master key are zeroed after import/export where managed-memory control permits.
- Generated document-key buffers are zeroed if secure-store persistence fails.
- Document-service import failure cleanup prevents a database record from remaining after its encrypted payload has been rolled back.
- Explicit export uses a safe leaf filename and creates an audit event.

## Chunked authenticated-encryption automation

The shared `ChunkedAead` framing is covered directly:

- version 2 multi-chunk round-trip;
- authenticated terminal record validation;
- chunk-boundary prefix truncation rejection;
- trailing-data rejection after the terminal record;
- legacy version 1 stream read compatibility;
- 32-byte AES-256 key requirement;
- nonce/AAD/tag/plaintext/ciphertext buffer clearing where managed-memory control permits.

The v2 framing upgrade applies to newly encrypted document and backup payload streams. Existing v1 streams remain readable; v1 ciphertext is not retroactively rewritten merely by upgrading the application.

## Encrypted backup automation

- Backup encryption round-trip, wrong-password rejection and tamper rejection.
- SQLite WAL snapshot creation, committed-data preservation, integrity checking, and pre-cancelled snapshot rejection without an output file.
- Portable recovery of the document master key when encrypted documents are present.
- Caller-owned document-master-key copies used during backup creation/restore are zeroed after use where managed-memory control permits.
- Password-derived AES key and salt buffers are zeroed after backup encryption/decryption paths.
- Strict archive topology validator rejects duplicate entries.
- Strict archive topology validator rejects unexpected files.
- Nested document entries are rejected.
- Non-`.cndoc` document entries are rejected.
- Manifest document-count mismatch is rejected.
- A document-bearing backup without a valid 32-byte document master key is rejected.
- Invalid backup schema/document-count metadata is rejected.
- The existing backup container/package version remains distinct from the internal chunked AEAD framing version.

## Persistence and app-lock automation

- SQLite migration idempotency and relationship cleanup.
- WAL journal mode/busy-timeout/checkpoint behavior.
- App-lock source contracts for salted PBKDF2-HMAC-SHA256 verification, fixed-time comparison, verifier-buffer clearing, no plaintext-PIN persistence, and removal of stored lock material when disabled.

## Reports/UI/repository-policy automation

- Report disclaimer presence.
- XAML semantic/accessibility contract checks.
- Shell route uniqueness and expected navigation targets.
- Architecture dependency rules.
- ViewModel persistence/network boundaries.
- No runtime TODO/FIXME/`NotImplementedException` placeholders.
- No named diagnosis/dosage/treatment/interaction/risk-scoring implementation regression.
- No common signing/private-key artifacts committed.
- Privacy-redacted exception logging contracts.
- Runtime async-safety contracts.
- Branding/localization/funding surface contracts.

## Manual device matrix

Automated tests do not replace these release gates:

- Android phone/tablet: notification permission, exact/inexact alarm behavior, battery optimization, reboot/time/time-zone changes, force-stop limitation, reminder delivery.
- iPhone/iPad: notification permission, scheduled delivery, app lock, document import/export, backup/restore.
- Mac Catalyst: resizing, keyboard navigation, notification scheduling, app lock, document flows, backup/restore.
- Windows: resizing, keyboard navigation, in-app reminder fallback/diagnostics, document flows, backup/restore.
- All targets: screen reader, large text, theme/contrast, reduced motion, destructive-action confirmation, clean-install restore.

## Safety assertions

- No screen asks CareNest to choose a dose.
- No clinical score or diagnosis appears.
- Stock text explains estimate limitations.
- Reports call data user-entered/unverified.
- Medical disclaimer appears onboarding and About.
- Invalid local times are not silently converted into clinically inferred alternatives.
- Planner ownership mismatches fail rather than silently moving reminder data across local profile/medicine boundaries.
- Local/unspecified `DateTime` values are not silently reinterpreted as UTC planning, snooze, or appointment times.
- Notification permission denial does not become an attempted platform schedule from CareNest application services.
- App lock is presented as a local privacy barrier rather than database/device encryption.
- Backup/document cryptography remains an organizational privacy/security control and is not presented as a clinical guarantee.
