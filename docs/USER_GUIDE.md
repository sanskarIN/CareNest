# CareNest User Guide

**Release line:** `1.0.0-rc.1`

CareNest is a local-first family health organizer for Android, iOS/iPadOS, Mac Catalyst and Windows. It helps organize user-entered medicine reminders, appointments, documents, stock/refill notes, reports/backups and multiple local profiles without requiring a CareNest account or CareNest-owned backend.

> **Medical limitation**
>
> CareNest is an organizational tool. It does not diagnose conditions, calculate or infer medicine dosage, recommend treatment, perform clinical medication-interaction checking, calculate clinical risk, verify adherence, replace a clinician/pharmacist, provide emergency services, or guarantee notification delivery.

## 1. Local-first model

Current v1 works without a required CareNest account/server.

- Structured records are stored locally in application-owned SQLite storage.
- Imported document payloads use the encrypted document-vault path.
- Manual backups are password-encrypted.
- CareNest does not automatically upload health records to a CareNest cloud service.
- Export/share/calendar/browser actions require explicit user action.
- External copies are governed by their receiving destination after handoff.

Deleting app data, losing the device or losing a backup password can make local information unavailable. Maintain encrypted backups appropriate to your needs.

## 2. First launch and onboarding

Typical flow:

1. Read privacy and medical/reminder limitations.
2. Create an initial local profile.
3. Optionally configure app lock.
4. Enter only information you want stored locally.
5. Configure reminders through explicit reminder-capable workflows.

Notification permission is tied to reminder-capable actions rather than being treated as a medical requirement.

## 3. Profiles

CareNest supports multiple local profiles for organizing information for different people.

A profile can group:

- medicines;
- schedules/reminder occurrences;
- medication-log entries;
- appointments;
- documents;
- emergency contacts;
- stock/refill records;
- reports/exports.

A profile is a local organizational container, not a remote account/caregiver relationship.

### Archive/delete

Archived profiles do not materialize automatic reminders.

Destructive deletion/reset requires explicit user action. If information must be retained, create a suitable encrypted backup/export before deletion.

## 4. Medicines

Medicine records store user-entered organizational information.

`Strength` and `Instructions` are opaque text. CareNest stores/displays them but does not derive dosage, frequency or treatment from them.

Medicine lifecycle can include active, paused, completed and archived states. Automatic reminder materialization is suppressed when applicable for paused/completed/archived medicines.

## 5. Stock/refill tracking

CareNest can maintain an organizational stock estimate from explicit user-entered values.

- initial stock is user-entered;
- quantity adjustments are explicit/user-configured;
- low-stock threshold is organizational only;
- CareNest does not infer quantity from medicine strength/instructions;
- always check the actual physical supply.

## 6. Reminder schedules

Schedules originate from explicit user input.

Supported concepts include:

- daily;
- selected weekdays;
- specific times;
- every N hours from an explicit start;
- cycles with explicit on/off day counts;
- custom date ranges;
- as-needed records with no automatic occurrences.

CareNest never converts medicine text into a guessed schedule.

## 7. Time zones and daylight saving

Schedule-local intent retains an explicit time-zone identifier.

- invalid spring-forward local time is not silently moved to a guessed alternate time;
- ambiguous fall-back local time is handled deterministically;
- stored schedule intent is not silently rewritten merely because device time zone changes.

See `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`.

## 8. Notification permission and delivery limits

A saved schedule can exist even when operating-system notification permission is denied.

Delivery can be affected by:

- notification permission;
- Android exact/inexact alarm capability;
- battery optimization/vendor restrictions;
- force-stop/process lifecycle;
- device reboot/shutdown;
- clock/time-zone/DST changes;
- platform updates/policy;
- notification settings changed outside CareNest.

CareNest cannot guarantee delivery under all OS/device conditions.

## 9. Platform notes

### Android

Uses Android notification/alarm integration and supported rebuild triggers. Exact/inexact/battery/background behavior remains OS/vendor controlled.

### iOS/iPadOS

Uses operating-system local notification scheduling. Real-device behavior is controlled by Apple platform policy/permissions.

### Mac Catalyst

Uses platform notification behavior subject to macOS/Catalyst lifecycle/permission restrictions.

### Windows

Current reminder fallback has explicit in-process limitations and does not claim reliable closed-app delivery.

See `docs/PLATFORM_BEHAVIOR_MATRIX.md`.

## 10. Reminder states

Organizational states include:

- Scheduled;
- Snoozed;
- Taken;
- Skipped;
- Delayed;
- Missed;
- Cancelled where applicable.

These states are local workflow/history records, not proof of medication ingestion or clinical adherence.

## 11. Snooze

A valid snooze uses an explicit future UTC time internally.

For a snoozed occurrence, `SnoozedUntilUtc` becomes its effective due time. The original `ScheduledUtc` remains historical schedule identity and should not incorrectly make a future snooze overdue.

## 12. Reminder action/recovery behavior

CareNest coordinates persisted reminder state and OS requests as separate surfaces.

For handled transitions, current source uses cancellation-first behavior where required. If later persistence/platform work fails, recovery/rebuild can be attempted rather than silently claiming a consistent result.

Users normally do not manage this directly; it exists to reduce stale OS requests after edits/actions/restarts.

## 13. Quiet hours

Quiet hours are user-controlled notification policy.

They can suppress supported platform scheduling during configured periods but do not change medical meaning, dosage or the user's underlying schedule intent.

## 14. Follow-up reminders

Follow-up delays are explicit user-entered values. A follow-up is a separate organizational occurrence and does not represent a treatment recommendation.

## 15. Medication log

Taken/Skipped/Delayed/Missed actions can create local medication-log entries.

A log entry records user/app interaction state. It is not independently verified clinical adherence.

## 16. Appointments

Appointments are local organizational records with explicit date/time/details and optional reminder behavior.

Appointment reminder times originate from the stored appointment instant and explicit lead time.

Calendar export is explicit user action. A calendar copy is outside CareNest control after handoff.

## 17. Document vault

Imported documents are stored through CareNest's encrypted application-owned document path.

Organization can include:

- document metadata;
- local folder/tags;
- import;
- open/export/share;
- delete.

CareNest does not automatically upload document contents to a CareNest service.

### Export/open boundary

An explicitly decrypted/exported copy leaves CareNest vault protection. The destination may retain it independently.

## 18. Reports and structured exports

CareNest provides user-controlled informational exports such as supported PDF, CSV and JSON output.

Reports are based on local/user-entered records and do not produce diagnosis, treatment conclusions, clinical scores or verified adherence.

Review personal information before sharing.

## 19. Manual encrypted backup

Backups are user-initiated and password-protected.

Important points:

- keep the password safe;
- CareNest has no server-side recovery service for a forgotten local backup password;
- store backup files according to your threat model;
- backup content is sensitive even when encrypted;
- do not attach real backups/passwords to public issues/support conversations;
- production qualification should include clean-install restore testing.

See `docs/architecture/BACKUP_AND_RESTORE.md`.

## 20. Restore validation

Restore validates format/authentication/topology before replacing current local state as designed.

Wrong-password, tampered, truncated or malformed backup content should fail closed rather than silently restore corrupted data.

## 21. App lock

CareNest has an optional local app lock.

Current security model includes:

- no plaintext PIN persistence;
- random salt;
- PBKDF2-HMAC-SHA256 verifier derivation;
- fixed-time comparison;
- secure platform storage for lock material;
- fail-closed invalid/corrupt material handling.

The app lock is a local privacy barrier. It is **not** whole-database encryption/device encryption.

## 22. Settings and diagnostics

Settings contain product preferences and reminder/storage diagnostics.

Diagnostic/logging policy is privacy-minimized and should exclude raw health text, document contents, backup passwords, PINs, crypto keys and unnecessary sensitive exception details.

See `docs/security/LOGGING_PRIVACY.md`.

## 23. Theme/accessibility

CareNest supports system/light/dark presentation and accessibility-oriented source/design behavior.

Production validation still requires actual checks for:

- screen readers;
- large text/text scaling;
- keyboard/focus on desktop;
- contrast;
- reduced motion;
- color-independent meaning.

See `docs/design/ACCESSIBILITY.md`.

## 24. Privacy and deletion

Consider:

- device lock/security;
- OS/device backups;
- notification previews;
- screenshots;
- shared-device access;
- exported files;
- calendar/share destinations;
- where encrypted backup files are stored.

Clearing CareNest-owned local data cannot reliably delete copies already handed to another application/service/device backup.

Read `PRIVACY.md` and `docs/privacy/`.

## 25. Project support / Buy Me a Coffee

The **distributed CareNest application package does not include or expose the external Buy Me a Coffee project-funding destination**.

Voluntary project support exists in repository documentation/metadata only:

`https://buymeacoffee.com/sanskarIN`

Project funding:

- does not unlock health features;
- does not change reminder reliability/priority;
- does not provide medical advice;
- does not grant access to local records;
- does not create a CareNest account;
- does not provide emergency/clinical service.

See `BUY_ME_A_COFFEE.md` and `docs/SUPPORT_CARENEST.md`.

## 26. Getting help

Repository: `https://github.com/sanskarIN/CareNest`

Business: `sanskarin@outlook.in`

Support: `supportramsandesh@gmail.com`

Creator: `https://www.github.com/sanskarIN`

For public bug reports, provide only the minimum synthetic/non-sensitive information needed to reproduce the issue. Never attach real health documents, backups, PINs, passwords, keys or private health information.

## 27. Current release-candidate status

CareNest remains `1.0.0-rc.1`.

Current PR #74 automated evidence is green at 331/331 core tests plus all configured normal platform, store-candidate, inspection-artifact, CodeQL and unsuppressed Dependency Audit gates.

Production release still requires actual evidence for:

- supported real-device/manual matrices;
- notification permission/delivery/lifecycle;
- packaged SQLite existing-data compatibility;
- encrypted document/backup compatibility;
- accessibility;
- production signing;
- final signed-package inspection/provenance;
- current store policy/metadata;
- exact production tag and tagged gates;
- publication.

The formerly tracked SQLite dependency exception is remediated in the current source graph; packaged data compatibility remains a separate manual gate.

Use `PROJECT_STATUS.md` and `docs/releases/NEXT_STEPS.md` for current release status.