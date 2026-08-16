# CareNest Test Plan

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`  
**Verified PR #74 head:** `8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

This plan defines the automated and manual evidence required for CareNest. The app is an organizational health tool; testing must preserve its non-clinical, local-first and privacy boundaries.

## 1. Current automated baseline

PR #74 evidence:

- CareNest CI #735 / `31938301209`: success;
- formatting: success;
- unit: 122/122;
- integration: 39/39;
- UI/source-policy: 170/170;
- total: **331/331**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- Store Package Configuration #124 / `31938301146`: success;
- Store Inspection Artifacts #47 / `31938301275`: success;
- CodeQL #735 / `31938301252`: success;
- Dependency Audit #91 / `31938301172`: success.

Permanent evidence: `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

## 2. Test objectives

The test system should prove, as far as automation can, that:

- deterministic domain/application rules remain correct;
- reminder planning never infers medical intent;
- persisted/OS reminder state is reconciled safely;
- database operations preserve consistency;
- encrypted documents/backups authenticate and fail closed;
- source architecture/privacy/async rules do not regress;
- strict XAML binding policy remains enforced;
- package/dependency/release workflows remain fail closed;
- documentation/status never substitutes for manual production evidence.

## 3. Unit test scope

Unit tests cover platform-neutral deterministic behavior, including:

- profile/medicine/schedule/appointment validation;
- schedule recurrence;
- UTC/time-zone/DST boundaries;
- stable occurrence identities;
- duplicate prevention;
- archive/pause/completion suppression;
- as-needed behavior;
- explicit future snooze rules;
- appointment UTC/permission logic;
- profile/medicine/appointment/document service orchestration;
- backup reminder coordination;
- compensation/recovery using deterministic test doubles.

## 4. Reminder planner cases

Required planner cases include:

- daily;
- multiple explicit times;
- selected weekdays;
- cycle on/off schedules;
- custom date ranges;
- every-N-hours;
- follow-up schedules;
- disabled schedules;
- archived profiles;
- paused/completed/archived medicines;
- as-needed schedules producing no automatic reminder;
- invalid/unknown enum values;
- ownership mismatch;
- unsupported weekday masks;
- blank/invalid time-zone IDs;
- half-open UTC windows;
- duplicate-time deduplication;
- chronological deterministic output;
- spring-forward invalid local times;
- fall-back ambiguous local times;
- fixed-seed randomized/property invariants.

## 5. Reminder coordinator/reconciliation cases

Required cases include:

- new platform request scheduling;
- same occurrence idempotency;
- stale request cancellation;
- cancellation before replacement;
- cancellation before suppression/invalidation;
- cancellation-first Taken/Skipped/Delayed/Missed handling;
- snooze old-request cancellation + replacement;
- valid future snooze crossing original due time;
- overdue snooze based on snooze due time;
- platform cancellation failure remaining retryable;
- persistence failure after cancellation triggering restoration/rebuild attempts;
- schedule edits/deletes;
- medicine/profile lifecycle cleanup;
- appointment reminder compensation;
- startup/rebuild recovery.

## 6. Appointment tests

Required assertions:

- `StartsUtc` must be true UTC;
- local/unspecified values rejected;
- time-zone identifier validation;
- reminder lead time derived only from explicit appointment configuration;
- notification permission denied is not successful scheduling;
- background rebuild does not prompt repeatedly;
- database/platform state compensation.

## 7. Integration test scope

Integration tests use real repository/infrastructure boundaries where practical and cover:

- SQLite migrations;
- relationship cleanup;
- transactions;
- WAL/busy-timeout behavior;
- snapshots/integrity;
- reminder persisted state;
- document encryption/import/export/delete;
- backup create/inspect/restore;
- report/export output;
- failure cleanup/rollback.

## 8. SQLite migration/integrity cases

- fresh database creation;
- ordered migration application;
- migration idempotency;
- schema version correctness;
- relationship/cascade cleanup;
- transactional multi-step operations;
- snapshot includes committed data;
- integrity validation;
- cancellation leaves no unintended final output.

Packaged existing-user data remains a separate manual release test.

## 9. Encrypted document cases

- current format round trip;
- tamper rejection;
- truncation/trailing-data rejection through shared framing where applicable;
- missing/corrupt key fail closed;
- no unrelated replacement key for existing ciphertext;
- import rollback if metadata/audit save fails;
- explicit export creates expected plaintext copy;
- failed export/import cleans app-owned partial output best effort;
- delete removes metadata/payload consistently;
- retained legacy v1 read compatibility where documented.

## 10. Backup/restore cases

- current backup round trip;
- wrong password rejected;
- tamper rejected;
- truncation rejected;
- trailing data rejected;
- strict archive topology;
- duplicate/unexpected/nested invalid entries rejected;
- document-key recovery material validated;
- database snapshot/integrity validated;
- clean-install restore;
- rollback restores previous key state where documented;
- genuine legacy fixtures when real prior bytes exist.

Never manufacture a test artifact and call it historical evidence.

## 11. App-lock cases

- PIN validation policy;
- random salt;
- PBKDF2-HMAC-SHA256 verifier;
- fixed-time comparison;
- secure-store material ownership;
- strict salt/verifier length validation;
- no plaintext PIN persistence;
- update/disable rollback;
- fail-closed corrupt/missing material;
- sensitive-buffer clearing where practical.

## 12. Reports/exports cases

- correct output structure;
- required disclaimer/limitations;
- CSV formula-like content neutralization;
- staged/partial-file behavior;
- atomic final move where documented;
- cleanup after failure/cancellation;
- share cache cleanup while CareNest owns the file;
- external-copy boundary documented.

## 13. UI/XAML/source-policy scope

`CareNest.UiTests` should continue to cover:

- required views/routes/navigation;
- semantic/accessibility source expectations;
- architecture project references;
- no direct SQL in ViewModels;
- no casual runtime HTTP/telemetry creation;
- async/cancellation policy;
- logging privacy;
- security/secret/signing-file hygiene;
- package identity/platform metadata;
- reminder contracts;
- release scripts/workflows;
- funding-free app runtime/package source boundary;
- documentation/repository policy where applicable.

## 14. Strict compiled XAML cases

Every binding-bearing page/template is subject to:

- correct root `x:DataType`;
- item-specific DataTemplate `x:DataType`;
- typed picker `ItemDisplayBinding`;
- typed explicit Source/ancestor bindings;
- Source binding compilation enabled;
- strict XAML compilation enabled;
- `XC0022`–`XC0025` as errors;
- no matching `NoWarn`, `x:Object`, `x:Null` bypass.

## 15. Dependency audit cases

Automated policy should detect:

- restoration of the old exact SQLite advisory suppression;
- regression below maintained SQLite native/provider floors;
- dependency audit failure on platform-neutral or MAUI graph;
- invalid event-specific dependency-review assumptions.

## 16. Package payload scanner cases

The scanner self-test must cover:

- clean payload pass;
- UTF-8 forbidden marker rejection;
- UTF-16 marker rejection;
- nested archive marker rejection;
- missing/unreadable path fail closed.

Actual Android/Windows/Apple inspection output is scanned before artifact staging/upload as configured.

## 17. CI matrix

Pull-request/release-relevant automation includes:

- formatting;
- all three core test projects;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts.

Production-style tags also use Release Gate and Release Evidence.

## 18. Android manual matrix

Validate on representative targets:

- fresh install/onboarding;
- notification permission denied/granted;
- medicine/appointment reminder create/edit/delete;
- actual delivery;
- Taken/Skipped/Delayed/Missed action ordering;
- snooze cancellation/replacement;
- schedule/medicine/profile delete stale cleanup;
- restart/reopen/reboot recovery;
- exact/inexact alarm diagnostics;
- battery optimization;
- clock/time-zone/DST changes;
- force-stop/vendor limitations;
- document picker/share;
- backup/restore;
- app lock;
- accessibility.

## 19. Windows manual matrix

Validate:

- install/execution;
- core CRUD/navigation;
- running-app notifications;
- closed-app limitation behavior;
- same-ID timer replacement/cancellation;
- reminder actions/snooze;
- restart/recovery;
- file/share;
- backup/restore;
- app lock;
- keyboard/focus;
- light/dark/system theme.

## 20. iPhone/iPad manual matrix

Real-device evidence must include:

- install;
- notification permission denied/granted;
- real reminders/actions/snooze;
- lifecycle/restart/time-zone behavior;
- backup/restore;
- file/share;
- app lock;
- Dynamic Type;
- VoiceOver;
- notification preview privacy.

Simulator compile is not a substitute.

## 21. Mac Catalyst manual matrix

Validate:

- install/execution;
- notification permission/delivery;
- reminder actions/reconciliation;
- restart;
- file/share;
- backup/restore;
- app lock;
- keyboard/focus;
- theme/contrast;
- signed/notarized candidate behavior when available.

## 22. Accessibility matrix

Use representative assistive technology for:

- screen-reader names/order;
- large text/scaling;
- destructive confirmation readability;
- desktop keyboard navigation/focus;
- light/dark/system contrast;
- color-independent meaning;
- reduced motion;
- privacy-safe actionable errors.

## 23. Packaged existing-data validation

With fictional representative data:

- install/upgrade using realistic package path;
- database opens;
- integrity passes;
- representative records readable/editable;
- schema version correct;
- reminders reconcile;
- no duplicate/stale OS requests;
- documents/backups remain usable.

## 24. Production signing/package validation

Outside Git:

- configure Android/Apple/Windows signing as applicable;
- record source SHA/version/package identity;
- record SHA-256 and signing/notarization/store provenance;
- scan final signed package for forbidden marker;
- install/smoke-test final candidate;
- verify support/legal/About surfaces.

## 25. Store validation

At actual submission time review current Apple/Google/Microsoft requirements, health-organizer wording, privacy/data-safety, permissions, screenshots, support/privacy/terms/security links and package identity.

## 26. Test-data policy

Use fictional/synthetic data in automation, screenshots, docs, store assets and migration fixtures.

Never commit real health data, production backups, PINs/passwords, crypto keys, tokens or signing material.

## 27. Failure handling

A red analyzer/test/build/security/package scan is evidence to investigate, not a reason to weaken the gate.

Fix the smallest correct source/test/workflow boundary, add regression coverage, and repeat exact-source verification when verification-relevant source changes.

## 28. Current release interpretation

The PR #74 automated matrix is green, but production completeness requires the remaining manual/package/accessibility/signing/store/tag/publication evidence in `docs/releases/NEXT_STEPS.md`.