# Test Plan

## Automated

- Domain validation and schedule planner edge cases.
- Explicit validation for selected weekdays, cycle on/off values, end-date ordering, clock-time ranges, every-N-hours starting-time count, unknown schedule enum values, unsupported weekday-mask bits, blank/unknown time-zone identifiers, and every-N-hours interval limits.
- Reminder planner ownership checks for profile → medicine → schedule → persisted schedule-time relationships.
- Explicit UTC-kind enforcement for reminder planning windows and coordinator rebuild overrides.
- DST/time-zone planner behavior using explicit time-zone identifiers where available.
- Representative North America, Europe, and Australia DST gap/overlap coverage when the test host exposes those zone identifiers.
- Stable reminder occurrence keys/idempotency.
- Half-open reminder planning windows (`fromUtc` inclusive, `toUtc` exclusive).
- Duplicate explicit reminder-time deduplication and chronological output ordering.
- Deterministic randomized/property-style recurrence coverage using a fixed seed.
- Daily, selected-weekday, cycle, custom date-range, every-N-hours, follow-up, disabled, archived-profile, paused, completed, archived-medicine, and as-needed scheduling behavior.
- Invalid spring-forward local times do not cause CareNest to invent an alternate reminder time.
- Ambiguous fall-back local times create one deterministic occurrence for the same schedule/window.
- Snooze actions require an explicit future UTC timestamp before persistence/platform scheduling.
- Stock arithmetic uses only user-entered adjustments.
- Backup encryption round-trip, wrong-password rejection and tamper rejection.
- SQLite WAL snapshot creation, committed-data preservation, integrity checking, and pre-cancelled snapshot rejection without an output file.
- Document encryption round-trip.
- SQLite migration idempotency and relationship cleanup.
- App-lock source contracts for salted PBKDF2-HMAC-SHA256 verification, fixed-time comparison, verifier-buffer clearing, no plaintext-PIN persistence, and removal of stored lock material when disabled.
- Report disclaimer presence.
- XAML semantic/accessibility contract checks.
- Shell route uniqueness and expected navigation targets.

See [`REMINDER_SCHEDULING_CONTRACT.md`](REMINDER_SCHEDULING_CONTRACT.md) for the exact non-clinical reminder-planning invariants protected by the unit suite.

## Manual device matrix

- Android phone/tablet: permission, exact-alarm fallback, battery optimization, reboot.
- iPhone/iPad: notification permission, scheduled delivery, app lock.
- Mac Catalyst: resizing, keyboard navigation, notification scheduling.
- Windows: resizing, keyboard navigation, in-app reminder fallback/diagnostics.

## Safety assertions

- No screen asks CareNest to choose a dose.
- No clinical score or diagnosis appears.
- Stock text explains estimate limitations.
- Reports call data user-entered/unverified.
- Medical disclaimer appears onboarding and About.
- Invalid local times are not silently converted into clinically inferred alternatives.
- Planner ownership mismatches fail rather than silently moving reminder data across local profile/medicine boundaries.
- Local/unspecified `DateTime` values are not silently reinterpreted as UTC planning or snooze times.
- App lock is presented as a local privacy barrier rather than database/device encryption.
