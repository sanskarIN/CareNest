# CareNest User Guide

CareNest is a local-first family health organizer for Android, iOS, Mac Catalyst, and Windows. It is designed to help users organize user-entered medicine reminders, appointments, documents, stock/refill notes, and local family profiles without requiring a CareNest account or backend service.

> **Medical limitation**
>
> CareNest is an organizational tool. It does not diagnose conditions, calculate or infer a medicine dose, recommend treatment, check medication interactions, create clinical risk scores, verify adherence, replace a clinician or pharmacist, or provide emergency services. Follow instructions from qualified professionals. In an emergency, contact local emergency services instead of relying on CareNest.

## 1. Local-first model

CareNest v1 works without a required account or CareNest server.

- Health records are stored locally in the application's device sandbox.
- Imported documents are stored through the encrypted document-vault path.
- CareNest does not automatically upload records to a CareNest cloud service.
- Exporting or sharing data requires explicit user action.
- Optional external project-support links are independent third-party destinations and do not receive health records from CareNest merely because the link is displayed.

Deleting the app, clearing application data, losing the device, or losing an encrypted backup password can make local information unavailable. Create manual encrypted backups when appropriate.

## 2. First launch and onboarding

On first launch, CareNest presents the local-first and medical-safety boundaries before normal use.

Typical setup flow:

1. Read the privacy and medical limitation information.
2. Create the first local person/profile.
3. Optionally configure the local app lock.
4. Enter only the information you want stored on the device.
5. Configure reminders later from explicit reminder-capable workflows.

CareNest does not request notification permission merely because onboarding started. Notification permission is requested when the user explicitly saves or enables a reminder-capable feature that needs it.

## 3. Family profiles

CareNest can store multiple local profiles for organizing information for different people.

A profile can be used to group:

- medicines;
- reminder schedules;
- medication-log entries;
- appointments;
- health documents;
- emergency contacts;
- profile notes;
- reports and exports.

Profiles are local organizational containers. A profile does not create a remote account or remote caregiver relationship.

### Archive/delete behavior

Archived profiles are excluded from automatic reminder materialization. Destructive deletion/reset operations require explicit user action and should be followed by verification that the intended records were removed.

Before deleting important local data, create an encrypted backup or user-controlled export if you need a copy.

## 4. Medicines

Medicine records hold user-entered organizational information.

Important rule: medicine `Strength` and `Instructions` are opaque text. CareNest stores and displays that text but does not interpret it to calculate dosage or treatment.

Medicine lifecycle states include active, paused, completed, and archived behavior. Automatic reminder materialization is suppressed for paused, completed, or archived medicines.

### Stock/refill notes

CareNest can track a local stock estimate from explicit user-entered values.

- The initial stock count is user-entered.
- Any quantity change associated with a Taken event is user-configured.
- CareNest does not infer tablet quantity from medicine strength or instruction text.
- Low-stock thresholds are organizational reminders only.
- Always check the actual physical supply.

## 5. Reminder schedules

All reminder schedule rules originate from user input.

Supported schedule concepts include:

- daily;
- selected weekdays;
- explicit specific times;
- every N hours from an explicit starting time;
- cycles with explicit on/off day counts;
- custom date ranges;
- as-needed records with no automatic reminder occurrences.

### Explicit time handling

CareNest never converts medicine text into a guessed frequency.

For a reminder-capable schedule:

- start/end dates are user-entered;
- time-zone identity is stored with the schedule;
- specific times are user-entered;
- every-N-hours intervals are user-entered;
- cycle lengths are user-entered;
- selected weekdays are user-entered;
- follow-up delay is user-entered.

### Daylight-saving behavior

CareNest preserves local schedule intent instead of inventing medical meaning.

If a local time does not exist because the clock moves forward, the planner does not silently move that reminder to a guessed replacement time.

If a local time occurs twice because the clock moves backward, CareNest uses deterministic handling so rebuilding the same schedule produces a stable occurrence.

See `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` for the complete deterministic planner contract.

## 6. Notification permission and delivery limitations

A saved schedule can exist even if operating-system notification permission is denied.

Actual notification delivery can be affected by:

- permission settings;
- operating-system scheduling policy;
- Android exact-alarm capability;
- battery optimization;
- force-stop state;
- device shutdown/reboot;
- time/time-zone changes;
- background restrictions;
- platform updates;
- notification settings changed outside CareNest.

CareNest surfaces these limitations and does not claim guaranteed reminder delivery.

### Platform notes

**Android**

CareNest uses Android notification/alarm integration and can rebuild after supported boot/time/time-zone events. Exact/inexact behavior depends on platform capability and policy.

**iOS / Mac Catalyst**

CareNest uses operating-system local notification scheduling. Delivery remains controlled by the OS.

**Windows**

The current fallback has explicit limitations and does not claim reliable delivery while CareNest is not running.

## 7. Reminder states and medication log

Reminder occurrences can move through organizational states such as:

- Scheduled;
- Snoozed;
- Taken;
- Skipped;
- Delayed;
- Missed.

Snooze requires an explicit future UTC destination internally; the UI workflow must represent a future user-selected time.

Taken/skipped/delayed/missed actions can create medication-log entries for later review. These entries represent user-recorded events, not proof of adherence.

## 8. Quiet hours and follow-ups

Quiet hours and follow-up reminders are user-controlled organizational settings.

- Quiet hours suppress supported notification scheduling during the configured period.
- Follow-up minutes are explicit user-entered schedule values.
- A follow-up occurrence remains separate from the original occurrence.

Changing these settings does not create treatment recommendations.

## 9. Appointments

Appointments are local organizational records. They can contain user-entered details, notes, attachments, and reminder/export information.

Calendar export requires explicit user action. Once information is exported to another calendar application or service, that copy is governed by the receiving application/service rather than CareNest.

## 10. Health document vault

Imported health documents are stored through CareNest's encrypted document path.

Supported organization concepts include:

- document records;
- local folders;
- tags;
- import;
- selected export/share;
- deletion.

CareNest does not automatically upload document contents to a CareNest service.

### Export boundary

When the user explicitly exports/decrypts/shares a document, the exported copy leaves CareNest's encrypted document-vault protection. Treat the destination as a separate privacy boundary.

## 11. Reports and structured exports

CareNest provides user-controlled exports such as:

- per-profile structured JSON;
- PDF profile summaries;
- CSV reports for supported organizational data sets.

Reports are informational and based on user-entered/local records. They do not provide diagnosis, treatment conclusions, clinical scores, or verified medical interpretation.

Before sharing a report, review it for personal information.

## 12. Encrypted backup and restore

CareNest supports manual password-encrypted backups.

The backup path uses authenticated encryption and a password-derived key. Backups are schema-versioned and validated during restore.

Important points:

- Keep the backup password safe; CareNest does not provide a remote password-recovery service.
- Store backup files somewhere appropriate for your threat model.
- Test restore on a clean installation before relying on a backup for a public production release.
- A backup can contain sensitive local information even though it is encrypted.
- Do not send backups through public issue trackers or support conversations.

See `docs/architecture/BACKUP_AND_RESTORE.md` for the format and recovery model.

## 13. Optional app lock

CareNest offers an optional local app lock.

Current security properties include:

- no plaintext PIN persistence;
- random salt;
- PBKDF2-HMAC-SHA256 verifier derivation;
- fixed-time verifier comparison;
- secure-platform secret-store persistence for lock material;
- clearing derived/retrieved verifier buffers where managed-memory control permits.

The app lock is a local privacy barrier. It is not whole-database encryption, device encryption, or a substitute for device-level authentication/security.

## 14. Settings and diagnostics

Settings include product preferences and developer/diagnostic information relevant to reminder reliability and local storage.

Diagnostic output is intentionally privacy-minimized. CareNest's logging policy excludes health-document contents, sensitive notes, backup passwords, plaintext PINs, encryption keys, and raw exception details from normal structured diagnostic logging.

See `docs/security/LOGGING_PRIVACY.md`.

## 15. Theme and accessibility

CareNest supports system/light/dark presentation and accessibility-oriented UI behavior.

Release testing covers or requires manual verification for:

- large text/text scaling;
- screen readers;
- semantic labels;
- keyboard navigation on desktop targets;
- reduced motion;
- contrast;
- focus order;
- ensuring color is not the only status signal.

See `docs/design/ACCESSIBILITY.md`.

## 16. Privacy and deletion

CareNest's local-first model reduces required remote data sharing but does not eliminate device-level privacy risks.

Users should consider:

- device lock/security;
- OS backups;
- exported files;
- screenshots;
- notification previews;
- shared-device access;
- external calendar/share destinations;
- where encrypted backup files are stored.

Read `PRIVACY.md`, `docs/privacy/DATA_LIFECYCLE.md`, and `docs/privacy/PRIVACY_MODEL.md`.

## 17. External project support

CareNest may show the voluntary support destination:

`https://buymeacoffee.com/sanskarIN`

Opening it is explicit user action and launches an external service.

Project support:

- does not unlock medical features;
- does not change reminder priority;
- does not provide emergency assistance;
- does not grant access to local health data;
- does not create a CareNest account;
- does not imply medical advice.

Store rules for external funding links can change; final store builds require channel-specific policy review.

## 18. Getting help

Repository: `https://github.com/sanskarIN/CareNest`

Business contact: `sanskarin@outlook.in`

Support contact: `supportramsandesh@gmail.com`

Creator profile: `https://www.github.com/sanskarIN`

For bugs, provide only the minimum technical information needed to reproduce the issue. Do not attach health documents, encrypted backups, credentials, PINs, signing keys, or private health information to public issues.

## 19. Known release-candidate limitations

CareNest `1.0.0-rc.1` remains a release candidate rather than a final public production release.

The repository still tracks real release gates including:

- manual supported-device testing;
- accessibility checks;
- real notification-permission/delivery checks;
- current Apple/Google external-support-link policy review;
- production signing/package identity;
- store listing/privacy/data-safety work;
- final release evidence for the exact promoted commit;
- the open SQLitePCLRaw dependency-risk decision/resolution documented in `docs/security/DEPENDENCY_RISK_REGISTER.md`.

Do not interpret a green automated CI matrix as a guarantee of real-device notification delivery or medical correctness.