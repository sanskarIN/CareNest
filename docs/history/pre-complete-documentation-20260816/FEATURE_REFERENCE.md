# CareNest Feature Reference

This reference maps the implemented `1.0.0-rc.1` product surface to its intended behavior and safety/privacy boundary.

CareNest is a local-first organizational application. It does not diagnose, recommend treatment, calculate/infer dosage, check medication interactions, create clinical risk scores, verify adherence, or provide emergency services.

## Onboarding

Purpose:

- explain local-first storage;
- establish medical/reminder limitations;
- create an initial local profile;
- optionally configure the local app lock.

Rules:

- no required CareNest account;
- no required CareNest backend;
- no automatic notification-permission request merely from onboarding;
- reminder permission is requested at explicit reminder-capable actions.

## Dashboard

Purpose:

- provide a local overview of family/profile organizational information;
- surface upcoming reminder information and relevant local records.

Boundary:

- dashboard content is based on local records;
- no remote caregiver sharing occurs automatically;
- no clinical scoring or prioritization is performed.

## Profiles

Purpose:

- organize records for multiple people on one local installation.

Related data:

- medicines;
- appointments;
- documents;
- emergency contacts;
- medication logs;
- stock adjustments;
- reports/exports.

Lifecycle:

- active local profile;
- archived profile behavior;
- explicit destructive deletion/reset flows.

Reminder rule:

- archived profiles do not materialize automatic reminder occurrences.

## Medicine records

Purpose:

- store user-entered organizational medicine information.

Important fields:

- name;
- strength text;
- instruction text;
- start/end date;
- lifecycle state;
- optional stock/refill values.

Safety contract:

- `Strength` and `Instructions` remain opaque strings;
- no dose/frequency is parsed from those strings;
- stock changes are based only on explicit user-entered quantities/configuration.

States:

- active;
- paused;
- completed;
- archived.

Automatic reminder materialization is suppressed for paused, completed, or archived medicines.

## Medicine schedules

Supported schedule concepts:

- Daily;
- SelectedWeekdays;
- SpecificTimes/user-entered explicit times;
- EveryNHours;
- Cycle;
- CustomDateRange;
- AsNeeded.

Validation includes:

- known schedule kind;
- start/end ordering;
- valid explicit time-zone identifier;
- hour 0–23;
- minute 0–59;
- selected-weekday schedule must have at least one selected weekday;
- every-N-hours requires an explicit interval from 1 to 168 hours and one explicit starting time;
- cycle requires positive on/off day values;
- as-needed creates no automatic occurrence.

Ownership integrity:

- schedule must belong to the medicine supplied to the planner;
- medicine must belong to the profile supplied to the planner;
- each schedule-time record must belong to the schedule being materialized.

## Reminder planner

Purpose:

- convert explicit user schedule intent into deterministic future organizational occurrences.

Core invariants:

- `fromUtc` and `toUtc` are UTC;
- planning window is half-open: `fromUtc` inclusive, `toUtc` exclusive;
- `toUtc` must be later than `fromUtc`;
- stable occurrence identity;
- duplicate explicit times deduplicate by stable key;
- returned occurrences are chronological;
- schedule/medicine/profile lifecycle boundaries are respected;
- schedule time zone is explicit and validated.

No clinical inference is performed.

## Daylight-saving and time zones

CareNest stores schedule-local intent with an explicit time-zone identifier.

Spring-forward gap:

- an invalid local time is not silently shifted to an invented replacement time.

Fall-back overlap:

- ambiguous local time resolves deterministically so rebuilds remain stable.

Automated coverage includes representative DST-observing zones across United States, United Kingdom, Australia, and New Zealand transitions when available on the runner.

## Reminder coordinator

Purpose:

- rebuild future occurrences;
- register supported future notifications;
- process state changes;
- reconcile overdue reminders;
- apply user-configured stock adjustments after Taken events.

Rebuild contract:

- explicit rebuild time must be UTC;
- default rebuild time comes from `TimeProvider.GetUtcNow()`;
- active medicine and non-archived profile checks occur before planner materialization.

Notification scheduling failures are privacy-redacted and do not log health-record identifiers.

## Reminder states

Supported organizational states include:

- Scheduled;
- Snoozed;
- Taken;
- Skipped;
- Delayed;
- Missed.

Snooze contract:

- Snoozed requires a value;
- value must be UTC;
- value must be later than the current UTC time.

Taken/skipped/delayed/missed events can create medication-log entries.

## Quiet hours

Purpose:

- user-controlled period where supported notification scheduling is suppressed.

Rules:

- quiet-hours enablement and start/end values are settings;
- behavior is organizational notification policy only;
- quiet hours do not modify medical schedule intent or dosage.

## Follow-up reminders

Purpose:

- create an additional occurrence at an explicit user-entered offset.

Rules:

- follow-up minutes are explicit;
- follow-up is separate from the original occurrence;
- follow-up identity is deterministic;
- as-needed records do not create automatic follow-up occurrences.

## Medication log

Purpose:

- record user-marked events such as Taken, Skipped, Delayed, or Missed.

Boundary:

- a log entry records user interaction/local state;
- it is not proof of adherence;
- it is not a clinical assessment.

## Stock/refill tracking

Purpose:

- provide an organizational stock estimate.

Rules:

- initial stock is user-entered;
- per-Taken quantity change is user-entered;
- negative resulting estimates are guarded;
- low-stock threshold is user-configured;
- user must check actual supply.

CareNest never derives quantity from strength/instruction text.

## Appointments

Purpose:

- organize appointment details, notes, reminder information, attachments, and history.

Calendar export:

- explicit user action;
- exported data leaves the CareNest privacy boundary and is governed by the destination application/service.

## Document vault

Purpose:

- organize locally imported sensitive documents.

Features:

- encrypted document storage;
- folders;
- tags;
- import;
- explicit export/share;
- deletion;
- optional profile-photo path through encrypted storage where applicable.

Boundary:

- CareNest does not automatically upload document contents;
- explicitly exported/decrypted copies are outside CareNest vault protection.

## Tags and folders

Purpose:

- local document organization.

Implementation model:

- document/tag many-to-many relationship through `DocumentTag`;
- optional local folder metadata is available in schema version 5.

## Emergency contacts

Purpose:

- store local contact information associated with a profile.

Boundary:

- CareNest does not become an emergency service;
- users must contact local emergency services directly in emergencies.

## Reports

Supported outputs include user-controlled informational exports such as:

- PDF profile summary;
- CSV upcoming schedule data;
- CSV medication log;
- CSV missed reminder report;
- CSV stock/refill report;
- CSV appointment history;
- CSV document list.

Report boundary:

- values are based on local/user-entered records;
- reports contain privacy/non-clinical limitations;
- no diagnosis, treatment recommendation, adherence verification, or clinical score is produced.

## Structured profile export

CareNest supports per-profile structured JSON export.

Purpose:

- user-controlled portability/review of local records.

Boundary:

- exported files are outside CareNest's protected application sandbox once saved/shared;
- user is responsible for the destination.

## Encrypted backup

Purpose:

- manual portable backup/restore without automatic cloud upload.

Properties:

- password-derived key;
- PBKDF2-HMAC-SHA256;
- authenticated AES-GCM encryption;
- versioned backup format;
- integrity/authentication checks before restore;
- portable encrypted-document key recovery in the protected payload;
- database snapshot uses WAL checkpoint and copied-database integrity verification in tests.

No remote password-recovery system exists.

## App lock

Purpose:

- optional local privacy barrier before access to CareNest UI.

Security contract:

- no plaintext PIN persistence;
- numeric PIN policy;
- random salt;
- PBKDF2-HMAC-SHA256 verifier;
- fixed-time comparison;
- verifier-buffer clearing where managed memory control permits;
- disable operation removes stored lock material.

Limitation:

- app lock is not whole-database/device encryption.

## Notification diagnostics

Purpose:

- explain relevant platform capability/permission limitations.

Android can surface permission/exact-alarm/battery-related limitations where APIs permit.

Windows explicitly surfaces fallback limitations rather than implying background reliability that does not exist.

## Developer diagnostics

Purpose:

- help maintainers/users inspect safe operational state without exposing raw health data.

Includes concepts such as:

- redacted schedule inspection;
- time-zone simulation without rewriting stored schedule intent;
- schema/database migration version display;
- storage usage/cache controls;
- sanitized diagnostic export.

Logging follows `docs/security/LOGGING_PRIVACY.md`.

## Theme and presentation

Supported presentation preferences include:

- system theme;
- light theme;
- dark theme;
- large-interface preference;
- reduced-motion preference.

Manual accessibility checks remain a release gate.

## About / legal / open source

The app/repository surfaces:

- product identity;
- creator profile;
- business/support contacts;
- Apache-2.0 license;
- privacy;
- terms;
- security;
- project support;
- medical/reminder limitations.

Watermark/creator wording: `Made by the Sanskar`.

## Voluntary project support

Destination:

`https://buymeacoffee.com/sanskarIN`

Rules:

- explicit external action;
- no health-data query payload;
- no medical feature entitlement;
- no reminder-priority change;
- no access to local records;
- no emergency/support-priority entitlement;
- store-channel policy review required before final distribution.

## Not part of CareNest v1

Deliberately excluded from the current local-first release:

- required accounts;
- CareNest backend storage;
- automatic cloud sync;
- silent remote caregiver sharing;
- remote caregiver collaboration;
- server-side health record storage;
- analytics/telemetry without a future explicit consent/privacy design;
- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- medication-interaction checking;
- clinical risk scoring.

Any future networked/synchronization/collaboration expansion requires new architecture, consent, privacy, threat-model, security, deletion/export, and abuse-review work.