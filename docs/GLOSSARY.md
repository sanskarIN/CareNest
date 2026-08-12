# CareNest Glossary

This glossary defines terms used throughout CareNest source, tests, UI, and documentation.

## App lock

Optional local privacy barrier that requires a user PIN before normal CareNest access. It stores derived verifier material rather than plaintext PIN. It is not whole-database/device encryption.

## Appointment

Local organizational record for an appointment, including user-entered details/notes and supported reminder/export information.

## As-needed schedule

A medicine schedule kind that creates no automatic reminder occurrences. CareNest does not infer when an as-needed medicine should be used.

## Audit entry

Local record describing a safe/high-level change event for an entity. Audit entries are not intended to duplicate sensitive document contents.

## Backup

Manual password-encrypted portable CareNest package containing the local data/recovery material needed for supported restore.

## Care document

Metadata record representing an imported local health document stored through the encrypted document-vault path.

## CareNestTargetFramework

MSBuild property used to narrow the multi-target MAUI app to a specific platform target before restore/build so unrelated workloads do not need to be evaluated.

## Clinical inference

Any behavior that attempts to derive diagnosis, dosage, treatment, medication interaction meaning, or clinical risk from user data. Clinical inference is outside CareNest v1 scope.

## Cycle schedule

User-defined recurrence with explicit positive on-days and off-days counted from the schedule start date.

## Delayed

Reminder state indicating a user-recorded delayed event. It is organizational state, not a clinical assessment.

## Dependency Audit

GitHub Actions workflow that audits NuGet dependency graphs. A successful workflow does not mean a narrowly suppressed tracked advisory is fixed.

## DocumentTag

Join relationship implementing the many-to-many association between local document metadata and tags.

## Document vault

CareNest encrypted local storage path for imported sensitive document payloads.

## DST gap

Local clock time that does not exist because clocks move forward for daylight saving. CareNest planner does not invent a replacement reminder time.

## DST overlap

Local clock time that occurs twice because clocks move backward. CareNest resolves it deterministically for stable occurrence identity.

## Every-N-hours schedule

User-entered recurrence defined by one explicit starting time and an explicit interval from 1 to 168 hours. CareNest does not derive the interval from medicine instructions.

## Explicit user input

A schedule/stock/profile/document/appointment value entered or chosen by the user rather than inferred from medical text.

## Follow-up reminder

Separate reminder occurrence created at an explicit user-entered minute offset from an original occurrence.

## Half-open planning window

UTC interval where `fromUtc` is included and `toUtc` is excluded: `[fromUtc, toUtc)`. This avoids duplicate occurrence ownership at adjacent window boundaries.

## Local-first

Architecture where normal CareNest v1 operation stores/processes records on the device without requiring a CareNest account/backend/network connection.

## Medication log

Local history of user-recorded reminder outcomes such as Taken, Skipped, Delayed, or Missed. It is not proof of adherence.

## Medicine

Local organizational record with user-entered name, opaque strength/instruction text, lifecycle state, dates, and optional stock/refill values.

## Medicine schedule

User-entered recurrence configuration associated with a medicine.

## Missed

Reminder state used for an occurrence that CareNest/user records as missed under the application's organizational lifecycle. It is not a clinical conclusion.

## NuGetAuditSuppress

Narrow package-audit suppression mechanism. In CareNest it must never be described as vulnerability remediation.

## Occurrence key

Stable deterministic identity for a reminder occurrence derived from schedule/local time/time-zone/follow-up context.

## Opaque medicine text

Strength/instruction string stored/displayed without CareNest interpreting it to calculate dosage or treatment.

## Person profile

Local family/person organizational container grouping medicines, appointments, documents, emergency contacts, and related records.

## Platform notification

Notification registration handed to Android/iOS/Mac Catalyst/Windows integration. Delivery is controlled by OS capability/policy and is not guaranteed.

## PRAGMA

SQLite control/query statement. Result-producing PRAGMAs used by CareNest WAL/backup setup are consumed as result-producing operations.

## Profile archive

Lifecycle action that preserves the local profile but suppresses applicable automatic reminder behavior. Archive is not deletion.

## Project support

Voluntary external Buy Me a Coffee contribution. It does not unlock medical functionality, reminder priority, local data access, or medical assistance.

## Reminder coordinator

Application service that loads eligible records, invokes reminder planning, persists occurrences, schedules/cancels platform notifications, processes reminder states, and applies explicit stock adjustment behavior.

## Reminder occurrence

Materialized scheduled reminder instance generated deterministically from explicit schedule data.

## Reminder planner

Platform-neutral deterministic application component that converts validated explicit user schedule intent into reminder occurrences.

## Reminder state

Lifecycle state such as Scheduled, Snoozed, Taken, Skipped, Delayed, or Missed.

## Release Evidence

Workflow/process capturing exact source/ref/toolchain/test/dependency/checksum evidence for an intended release candidate. It does not replace manual/store/security approval.

## Release Gate

Workflow/policy designed to block final production promotion while required release conditions remain unresolved.

## Release candidate

Source version considered feature/source complete enough for verification but not yet publicly approved/signed/published as production.

## ScheduleTime

Explicit hour/minute record owned by one `MedicineSchedule`.

## SchemaInfo

SQLite metadata recording CareNest schema migration version.

## Selected weekdays

Schedule mode using an explicit weekday bit mask. At least one weekday is required.

## Snoozed

Reminder state with an explicit future UTC time. Snooze is user action and does not alter medical instructions.

## SQLitePCLRaw advisory

Current tracked dependency-risk issue `GHSA-2m69-gcr7-jv3q` affecting the native SQLitePCLRaw `2.1.11` path. It remains open until actually resolved/approved.

## Stock adjustment

Local quantity delta associated with a medicine stock estimate. Automatic Taken-event changes use only explicit user-configured quantity values.

## Stock estimate

CareNest organizational estimate of local supply based on user-entered values. Users must check actual supply.

## Taken

User-recorded reminder outcome. It can trigger a user-configured local stock adjustment but is not independently verified adherence.

## Time zone ID

Explicit schedule time-zone identifier used to convert stored local clock intent into UTC occurrences.

## UTC contract

Public/internal reminder planning/snooze parameters documented as UTC must use `DateTimeKind.Utc`; local/unspecified values are rejected rather than silently reinterpreted.

## Verification marker

Temporary file under `build/verification/` added on a branch created from an exact source SHA to trigger PR workflows. It must not be merged into production `main`.

## WAL

SQLite Write-Ahead Logging mode. CareNest backup snapshot logic checkpoints WAL before copying the database.

## Whole-database encryption

Encryption of the entire SQLite database file. CareNest v1 does not claim this capability; imported documents/backups have separate encryption protections.