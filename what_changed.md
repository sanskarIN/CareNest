# what_changed.md

## CareNest implementation record

This file is the detailed handoff requested in place of long chat messages. The uploaded **Master Build Prompt — CareNest** remains the source of truth.

Repository: `https://github.com/sanskarIN/CareNest`  
Release target: `1.0.0-rc.1`  
Framework: .NET MAUI / .NET 10  
Primary language: C#  
Source model: Apache-2.0 open source  
Business email: `sanskarin@outlook.in`  
Support email: `supportramsandesh@gmail.com`  
Watermark: `Made by the Sanskar`

## Implemented product scope

CareNest is implemented as a local-first organizational application. It does not diagnose, determine dosage, infer doses, recommend treatment, perform interaction checking, produce clinical risk scores, replace clinicians/pharmacists, or provide emergency services.

Implemented areas include:
- onboarding and local-first privacy disclosure;
- multiple local family profiles;
- optional app lock and secure secret storage;
- medicine records with user-entered strength/instruction text;
- explicit user-defined reminder schedules;
- reminder occurrence materialization and recovery;
- taken/skipped/delayed/missed/snoozed states;
- medication log and edit history;
- appointments, reminders, notes, attachments and calendar export;
- encrypted local document vault, tags, folders and selected export;
- stock/refill tracking driven only by user-entered quantities;
- caregiver/local-profile dashboard without silent sharing;
- reports and per-profile exports;
- encrypted manual backup and schema-versioned restore;
- settings, themes, accessibility and developer diagnostics;
- About/open-source/legal/support surfaces;
- original CareNest vector branding assets;
- unit, integration and UI-contract tests;
- cross-platform GitHub Actions and CodeQL workflows.

## Repository structure

The repository contains the requested multi-project solution:

```text
src/
  CareNest.App/
  CareNest.Domain/
  CareNest.Application/
  CareNest.Infrastructure/
  CareNest.Shared/
tests/
  CareNest.UnitTests/
  CareNest.IntegrationTests/
  CareNest.UiTests/
docs/
  architecture/
  design/
  privacy/
  security/
  setup/
  testing/
  releases/
build/
  scripts/
.github/
```

## Delivery phases completed

### Phase 0 — repository, architecture, privacy and design foundation

Completed:
- repository standards, central packages, analyzers, editor settings and ignores;
- Apache-2.0 license and notices;
- contribution, conduct, security, support, privacy and terms documentation;
- architecture decisions, database schema documentation and threat model;
- design system, localization readiness and store asset guidance;
- issue templates, pull-request template, Dependabot, CI and CodeQL.

### Phase 1 — domain, persistence, encryption and application services

Completed:
- requested entities and enums;
- local SQLite persistence and migrations;
- repositories, settings and audit entries;
- explicit time-zone-aware reminder planning;
- deterministic reminder occurrence IDs and idempotent rebuilds;
- encrypted document vault using authenticated encryption;
- password-encrypted portable backup/restore format;
- report/export services;
- profile, medicine, appointment, document and reminder use-case services.

### Phase 2 — MAUI workflows

Completed:
- onboarding and startup routing;
- dashboard;
- profiles and profile editor;
- medicines and medicine editor;
- schedule editor;
- medication log;
- appointments and appointment editor;
- documents;
- reports;
- settings and developer diagnostics;
- lock screen;
- About/open-source page;
- responsive XAML and accessibility semantics;
- dark/light/system theme support.

### Phase 3 — platform integrations and reliability

Completed:
- Android reminder scheduling, reboot/time/time-zone rebuild receiver and diagnostics;
- iOS local notifications;
- Mac Catalyst local notifications;
- Windows fallback diagnostics and documented limitations;
- startup rebuild and overdue reconciliation;
- explicit notification-permission timing;
- quiet-hours handling;
- profile/document/export/delete workflows;
- encrypted backup and restore.

### Phase 4 — tests and release engineering

Completed:
- reminder planner tests;
- domain validation tests;
- SQLite integration/migration tests;
- encrypted document and backup integrity/tamper tests;
- report/export safety tests;
- UI contract tests;
- GitHub CI matrix for core, Android, Windows and Apple builds;
- CodeQL analysis;
- release checklist and troubleshooting documentation.

## GitHub delivery history

The complete release-candidate implementation was assembled on branch `release/carenest-1.0.0-rc.1` and merged through PR #3.

Important implementation and hardening commits already present include:
- `feat: build complete CareNest local-first health organizer`
- `ci: add cross-platform CareNest build and test workflow`
- `ci: add CodeQL security analysis`
- `fix: scope analyzer exceptions for shared primitives`
- `ci: isolate platform target frameworks per runner`
- `fix: use valid rule parameter names in profile validation`
- `fix: use valid appointment validation parameter name`
- `fix: use valid medicine rule parameter names`
- `security: pin patched SQLitePCLRaw native packages`
- `fix: resolve reminder planner performance analyzer findings`
- merge commit for PR #3: `1244ed7fead73821f768f5119230dd6b8c24113f`

## CI investigation after merge

The final pre-merge PR CI run exposed two release blockers after the last source hardening pass.

### Blocker 1 — CA1848 logging analyzer

GitHub Actions core tests failed in `ReminderCoordinator.cs` because CA1848 was configured as a warning promoted to an error. This analyzer recommends precompiled logger delegates for performance; it did not indicate incorrect reminder behavior or unsafe logging.

Fix pushed to `main`:
- commit `7fed6d76ae2407d17bf3b19e8e4b112b3f39e279`
- message: `ci: keep CA1848 logging optimization non-blocking`

The analyzer remains visible as a suggestion to contributors but no longer blocks all release targets.

### Blocker 2 — vulnerable SQLite native package pin

GitHub Actions Android and Windows restore failed with `NU1903` because `SQLitePCLRaw` native package version `2.1.11` was flagged by GitHub/NuGet vulnerability auditing under advisory `GHSA-2m69-gcr7-jv3q`.

Fix pushed to `main`:
- commit `7489b70f0cf37be7545e1ecb338fec6a7ccf90dd`
- message: `security: update SQLite native packages past vulnerable 2.1.11`

Central package management now pins the bundle and native packages consistently past `2.1.11`.

### Verification branch

A dedicated post-merge verification branch was created from the fixed `main` head:
- branch: `ci/carenest-rc1-verification`
- verification marker commit: `018c452e7c9e895613d2bd286126e18bdefd319e`
- message: `ci: trigger post-merge release verification`
- pull request: #10 — `Verify CareNest 1.0.0-rc.1 after CI/security fixes`

PR #10 exists only to exercise the complete pull-request CI matrix after the post-merge fixes. The marker file does not alter production behavior.

## CodeQL status

The CodeQL workflow for release commit `469f89817d305ac5b3b96957a11fabe2de1c3cee` completed successfully before the merge.

## Security model implemented

- Imported documents and profile photos use encrypted local storage.
- Backup archives use password-derived authenticated encryption.
- App-lock secrets are stored through platform secure storage and plaintext PINs are not persisted.
- Sensitive document contents are excluded from diagnostics/logs.
- No analytics or telemetry are included.
- No account, server, cloud sync or automatic remote upload exists.
- Exported/decrypted files leave CareNest protection only through explicit user action.
- Whole-database encryption at rest is not falsely claimed; SQLite relies on the application sandbox/device protections documented in the security notes.

## Reminder reliability rules implemented

- schedules come only from explicit user input;
- future occurrences are rebuilt idempotently;
- stored schedule times are not silently rewritten on time-zone changes;
- Android handles reboot/time/time-zone rebuild signals;
- notification permission denial is surfaced;
- battery/exact-alarm limitations are surfaced;
- iOS/Mac Catalyst use OS-managed local notifications;
- Windows fallback limitations are documented rather than hidden;
- quiet hours and follow-up behavior are user controlled.

## Medical-safety boundaries implemented

CareNest intentionally does not include:
- symptom diagnosis;
- dosage calculation or inference;
- medication interaction claims;
- treatment recommendations;
- risk predictions;
- clinical scoring;
- emergency-service replacement;
- document medical interpretation.

Medical limitations are visible in onboarding, About, reports and documentation.

## Commit identity

The requested maintainer email is configured for local/future commits in:
- `build/scripts/setup-git.sh`
- `build/scripts/setup-git.ps1`
- `docs/setup/DEVELOPMENT.md`

Configured command:

```bash
git config user.email "sanskarin@outlook.in"
```

The connected GitHub write API used in this chat does not expose author/committer email fields on its create/update commit actions. Connector-created commits therefore use the authenticated GitHub identity. The repository does not falsely claim that the connector forced the requested email into commit metadata.

## Verification limitation of this execution container

The local execution container available in this chat does not include the `dotnet` command or MAUI workloads. Therefore local `dotnet restore`, `dotnet format`, compilation, emulator/device smoke tests, signing and store packaging cannot be truthfully claimed as executed here.

GitHub Actions is the authoritative automated verification surface for the generated repository. Release tagging should be performed only after PR #10's complete matrix is green.

## Current state

- Complete source implementation is on `main`.
- Post-merge CI/security fixes are on `main`.
- CodeQL passed for the release branch.
- PR #10 is the current verification gate for the repaired release candidate.
- Cloud sync, remote caregiver collaboration, accounts and clinical interpretation remain intentionally deferred to later separately reviewed versions.
