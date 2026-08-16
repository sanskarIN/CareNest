# CareNest Feature Reference

**Release line:** `1.0.0-rc.1`

This reference maps the implemented product surface to intended behavior and safety/privacy boundaries.

CareNest is a local-first organizational application. It does not diagnose, recommend treatment, calculate/infer dosage, perform clinical medication-interaction checking, calculate clinical risk, verify adherence or provide emergency services.

## Onboarding

Purpose:

- explain local-first storage;
- establish medical/reminder limitations;
- create an initial local profile;
- optionally configure local app lock.

Rules:

- no required CareNest account/backend;
- no automatic cloud synchronization;
- reminder permission requested only from applicable user workflows;
- no medical/clinical inference.

## Dashboard

Purpose:

- summarize local family/profile organizational information;
- surface upcoming reminders/appointments and local care records.

Boundary:

- uses local records;
- no automatic remote caregiver sharing;
- no clinical prioritization/risk score.

## Profiles

Purpose:

- organize multiple people on one local installation.

Related data can include medicines, schedules, logs, appointments, documents, contacts, stock/refill records and reports.

Archived profiles do not materialize automatic reminders.

## Medicine records

Purpose:

- store user-entered organizational medicine information.

Safety contract:

- `Strength` and `Instructions` remain opaque strings;
- no dose/frequency/treatment is parsed from them;
- stock changes use explicit user-entered values/configuration.

Lifecycle states can include active, paused, completed and archived. Automatic reminder materialization is suppressed when applicable for paused/completed/archived medicine states.

## Medicine schedules

Supported concepts include:

- Daily;
- SelectedWeekdays;
- SpecificTimes;
- EveryNHours;
- Cycle;
- CustomDateRange;
- AsNeeded.

Validation covers known schedule kind, date ordering, time-zone identity, clock-time ranges, required weekday selections, every-N-hours interval/start, cycle counts and ownership relationships.

`AsNeeded` creates no automatic occurrences.

## Reminder planner

Purpose:

- deterministically convert explicit schedule intent into future organizational occurrences.

Core invariants:

- planning boundaries are true UTC;
- half-open planning window (`fromUtc` inclusive / `toUtc` exclusive);
- stable occurrence identity;
- deterministic ordering/deduplication;
- explicit time-zone validation;
- profile/medicine/schedule state boundaries;
- no clinical inference.

## Daylight-saving/time-zone behavior

Schedule-local intent retains an explicit time-zone identifier.

- invalid spring-forward local time is not silently shifted to a guessed time;
- ambiguous fall-back local time resolves deterministically;
- rebuilding the same schedule remains stable.

## Reminder coordinator

Purpose:

- rebuild future occurrences;
- register/cancel platform requests;
- process user reminder states;
- reconcile stale/overdue occurrences;
- coordinate user-configured stock effects for Taken actions where configured.

The coordinator treats persisted CareNest state and OS scheduled-request state as separate surfaces and uses reconciliation/compensation rather than pretending they are one transaction.

## Reminder reconciliation

Current source protects:

- stale request cancellation;
- cancellation before replacement/suppression/invalidation;
- cancellation-first handled actions;
- retryable platform cancellation failure;
- restoration/rebuild attempts after later persistence failure;
- lifecycle cleanup after schedule/medicine/profile changes.

## Reminder states

Organizational states include Scheduled, Snoozed, Taken, Skipped, Delayed, Missed and Cancelled where applicable.

These states do not independently prove ingestion/adherence.

## Snooze

Rules:

- snooze requires a value;
- value is true UTC at the coordinator boundary;
- value must be later than current UTC when created;
- valid `SnoozedUntilUtc` becomes effective due time.

The original `ScheduledUtc` remains schedule identity/history.

## Quiet hours

Purpose:

- user-controlled notification suppression period.

Quiet hours alter supported notification scheduling, not dosage or underlying medical meaning.

## Follow-up reminders

Purpose:

- create a separate occurrence at an explicit user-entered delay.

No medical follow-up timing is inferred.

## Medication log

Purpose:

- record local user-marked reminder/medicine events.

Boundary:

- records user/app interaction state;
- not proof of adherence;
- not a clinical assessment.

## Stock/refill tracking

Purpose:

- maintain an organizational stock estimate.

Rules:

- initial stock and adjustments are explicit;
- negative estimates are guarded;
- threshold is user-configured;
- actual physical supply must be checked;
- no quantity is derived from medicine strength/instructions.

## Appointments

Purpose:

- organize appointment date/time/details/notes/attachments/reminder information.

Appointment source rules include true UTC storage at the application boundary and explicit reminder lead-time behavior.

Calendar export is explicit; destination copies leave CareNest privacy control.

## Document vault

Purpose:

- organize imported local sensitive documents.

Features include encrypted application-owned payload storage, metadata, folders/tags, import/open/export/share/delete.

CareNest does not automatically upload document contents.

Explicit plaintext export/open creates a copy outside the encrypted vault boundary.

## Tags/folders

Provide local document organization. Current schema supports the documented tag relationship/folder metadata model.

## Emergency contacts

Store local user-entered contact information associated with a profile.

CareNest does not become an emergency service; emergencies require local emergency services.

## Reports

User-controlled outputs include supported PDF, CSV and structured exports.

Reports use local/user-entered records and do not produce diagnosis, treatment recommendations, verified adherence or clinical scores.

## Structured profile export

Provides user-controlled portable structured data for a profile.

Exported files leave the CareNest-owned sandbox/protection boundary once saved/shared.

## Encrypted backup

Purpose:

- manual portable local backup/restore without automatic cloud upload.

Properties include password-derived key material, authenticated encryption, versioned format, database snapshot/integrity validation and document recovery material where required.

Wrong password/tamper/truncation/malformed topology is rejected.

No remote password-recovery service exists.

## App lock

Purpose:

- optional local privacy barrier before CareNest UI access.

Security contract includes no plaintext PIN persistence, random salt, PBKDF2-HMAC-SHA256 verifier, fixed-time comparison, secure storage and fail-closed invalid material.

Limitation: app lock is not whole-database/device encryption.

## Notification diagnostics

Purpose:

- explain relevant platform permission/capability limits.

Android can surface permission/alarm/battery constraints where supported. Windows exposes fallback limitations rather than implying guaranteed background reliability.

## Developer diagnostics

Purpose:

- inspect privacy-safe operational state without exposing raw health data.

Examples include redacted schedule state, time-zone simulation, schema version/storage diagnostics and sanitized diagnostic output as implemented.

## Theme/presentation

Supports system/light/dark presentation and documented large-interface/reduced-motion/accessibility-oriented behavior where configured.

Real assistive-technology validation remains a manual release gate.

## About / legal / open source

Application/repository surfaces include product identity, creator/repository links, business/support contacts, Apache-2.0 license, privacy, terms, security, notices and medical/reminder limitations.

Watermark/creator wording: `Made by the Sanskar` where used by project branding.

## Repository project support

Voluntary project support destination:

`https://buymeacoffee.com/sanskarIN`

**Current product boundary:** the distributed application source/package does **not** include or expose this external funding destination/card/command/artwork.

Repository support:

- is optional;
- does not unlock app/health features;
- does not change reminder reliability/priority;
- does not provide medical/emergency service;
- does not grant access to local health records;
- is governed by third-party terms/privacy when opened from repository documentation.

See `BUY_ME_A_COFFEE.md` and `docs/SUPPORT_CARENEST.md`.

## Local privacy cleanup

CareNest supports documented application-owned local cleanup/reset workflows.

Deletion of CareNest-owned data cannot guarantee removal of copies previously exported/shared, captured in screenshots, stored in device backups or retained by external applications/services.

## Strict compiled XAML bindings

All binding-bearing pages/templates use typed compiled-binding metadata. The app project enables Source binding compilation and strict XAML compilation and treats `XC0022`–`XC0025` as errors.

This is a build/source quality feature, not a user medical feature.

## Platform targets

Current target frameworks:

- Android `net10.0-android`;
- iOS/iPadOS `net10.0-ios`;
- Mac Catalyst `net10.0-maccatalyst`;
- Windows `net10.0-windows10.0.19041.0`.

Automated builds are not a substitute for real-device/manual production evidence.

## Not part of CareNest v1

Deliberately excluded:

- required accounts;
- CareNest backend storage;
- automatic CareNest cloud sync;
- silent remote caregiver sharing/collaboration;
- server-side health record storage;
- hidden analytics/telemetry networking;
- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- clinical medication-interaction checking;
- clinical risk scoring;
- emergency-service replacement;
- guaranteed notification delivery.

Future networked/clinical scope requires new architecture, consent, privacy, security, threat-model, deletion/export, safety and store-policy review.

## Current release status

PR #74 verified 331/331 core tests plus all configured normal platform builds, store-candidate builds, inspection artifacts, CodeQL and unsuppressed Dependency Audit.

CareNest remains `1.0.0-rc.1` because manual/device/package/accessibility/signing/store/tag/publication evidence remains open.

Use `PROJECT_STATUS.md` and `docs/releases/NEXT_STEPS.md`.