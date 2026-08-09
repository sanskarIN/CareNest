# Test Plan

## Automated

- Domain validation and schedule planner edge cases.
- DST/time-zone planner behavior using explicit time-zone identifiers where available.
- Stable reminder occurrence keys/idempotency.
- Stock arithmetic uses only user-entered adjustments.
- Backup encryption round-trip, wrong-password rejection and tamper rejection.
- Document encryption round-trip.
- SQLite migration idempotency and relationship cleanup.
- Report disclaimer presence.
- XAML semantic/accessibility contract checks.
- Shell route uniqueness and expected navigation targets.

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
