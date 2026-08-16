# CareNest Platform Behavior and Evidence Matrix

This document separates **source/CI evidence** from **manual production evidence** for each supported target.

## Evidence legend

- **Automated verified** — covered by the PR #74 configured automated matrix.
- **Source contract** — protected by code/source-policy tests but not necessarily exercised on real hardware.
- **Manual required** — must be validated on a representative real target before production promotion.
- **External** — controlled partly or entirely by OS/store/signing infrastructure.

## Current automated source boundary

Verified PR head: `8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`.

Merged executable source: `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`.

Automated totals: 331/331 core tests.

## Android

Target: `net10.0-android`  
Minimum supported OS platform declaration: Android API 24.

### Automated verified

- Release compilation;
- store-candidate compilation;
- unsigned Android AAB inspection publish;
- forbidden external-funding marker payload scan;
- inspection provenance/checksum staging;
- platform metadata/source contracts;
- reminder planner/reconciliation source behavior;
- Android receiver async-lifetime source contract;
- permission/manifest-related source policies where covered.

### Manual required

- fresh install/onboarding;
- notification permission denied/granted;
- real reminder delivery;
- exact/inexact alarm behavior;
- battery optimization behavior;
- device/vendor background restrictions;
- force-stop behavior and user messaging;
- reboot recovery;
- app restart/reopen recovery;
- clock/time-zone/DST recovery;
- create/edit/delete reminder scheduling;
- Taken/Skipped/Delayed/Missed cancellation-first behavior;
- snooze cancellation/replacement;
- stale-request cleanup after schedule edits/deletes;
- document picker/share;
- backup/restore;
- app lock;
- accessibility on representative Android devices.

## Windows

Target: `net10.0-windows10.0.19041.0`  
Minimum Windows target declaration: 10.0.19041.0.

### Automated verified

- Release compilation;
- store-candidate compilation;
- self-contained Windows inspection publish;
- forbidden-marker payload scan;
- provenance/checksum staging;
- same-ID timer race source/test contracts;
- strict XAML compiled-binding policy.

### Important limitation

The current Windows reminder fallback is in-process. Automated tests can protect timer ownership/replacement/cancellation/disposal logic but cannot prove closed-app delivery on every Windows lifecycle state.

### Manual required

- package/install execution;
- core navigation/CRUD;
- running-app notifications;
- closed-app behavior/limitation messaging;
- timer replacement/cancellation;
- reminder actions and snooze;
- restart/recovery;
- document picker/share;
- backup/restore;
- app lock;
- keyboard/focus order;
- light/dark/system theme;
- Windows accessibility tooling.

## iOS / iPadOS

Target: `net10.0-ios`  
Minimum OS platform declaration: iOS 15.

### Automated verified

- iOS simulator Release compilation;
- iOS simulator store-candidate compilation;
- iOS simulator inspection build;
- strict XAML compiled-binding policy;
- platform/source contracts that do not require physical hardware.

### Manual/external required

- real-device install;
- Apple signing/provisioning;
- notification permission denied/granted;
- real local notification delivery;
- reminder actions/snooze where supported by current implementation;
- restart/lifecycle behavior;
- time-zone/DST behavior;
- backup/restore;
- document picker/share;
- app lock;
- Dynamic Type;
- VoiceOver;
- notification preview privacy;
- store/TestFlight behavior as applicable.

Simulator success must never be described as real-device notification proof.

## Mac Catalyst

Target: `net10.0-maccatalyst`  
Minimum platform declaration: Mac Catalyst 15.

### Automated verified

- Release compilation;
- store-candidate compilation;
- unsigned Mac Catalyst inspection publish;
- payload/provenance workflow;
- strict XAML binding compilation.

### Manual/external required

- signed/notarized install when available;
- notification permission/delivery;
- reminder actions/snooze/reconciliation;
- restart/lifecycle behavior;
- file picker/share;
- backup/restore;
- app lock;
- keyboard/focus;
- theme/contrast;
- VoiceOver/desktop accessibility;
- notarization/store behavior.

## Cross-platform reminder rules

The deterministic application layer protects shared reminder intent/state, but platform scheduling is not identical across targets.

Cross-platform source rules include:

- explicit user-entered schedules only;
- ownership/state validation;
- UTC planning windows;
- deterministic time-zone/DST handling;
- effective snooze due time;
- stale request reconciliation;
- cancellation-before-replacement/suppression/handled-state transitions where required;
- persistence/platform compensation.

Real OS delivery remains manual evidence.

## Cross-platform storage rules

Automated/integration evidence covers source-level behavior for:

- SQLite schema/migrations/transactions;
- document encryption;
- backup encryption/restore validation;
- reports/exports;
- cleanup/rollback contracts.

Production release still requires packaged compatibility checks on representative targets, particularly existing SQLite data and encrypted documents/backups.

## Cross-platform accessibility

Source and XAML contracts are necessary but insufficient. Manual evidence must cover representative screen reader, large text/scaling, keyboard/focus where applicable, contrast/theme, reduced motion and color-independent meaning.

## Store/package interpretation

Automated Store Package Configuration and Store Inspection Artifacts are internal engineering evidence. They do not replace:

- production signing;
- notarization/provisioning;
- actual store submission;
- store review/approval;
- submission-time policy review;
- final signed-package checksums/provenance;
- installation smoke testing of production candidates.

## Current release conclusion

All four target families compile under the current verified source and all configured store-candidate/inspection workflows are green, but CareNest remains `1.0.0-rc.1` until the manual/external rows in this matrix and `docs/releases/NEXT_STEPS.md` are evidenced.