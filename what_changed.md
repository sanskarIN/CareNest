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
Creator profile: `https://www.github.com/sanskarIN`  
Voluntary project support: `https://buymeacoffee.com/sanskarIN`  
Watermark: `Made by the Sanskar`

---

## Safety boundary implemented

CareNest remains a local-first organizational application. It does not diagnose conditions, determine dosage, infer doses, recommend treatment, perform medication-interaction checking, produce clinical risk scores, replace a doctor/pharmacist, or provide emergency services.

All reminder schedules come from explicit user input. Medicine strength and instruction text are stored as entered and are not interpreted as dosage rules. `StockChangePerTakenEvent` is explicitly user-entered and is never inferred from medicine strength or instruction text.

Reminder delivery limitations are surfaced instead of hidden. Device permissions, battery optimization, exact-alarm capability, operating-system restrictions, shutdown/force-stop behavior, daylight-saving changes and time-zone changes can affect delivery.

The application tells users to follow qualified professional instructions and to contact local emergency services in an emergency rather than rely on CareNest.

Buy Me a Coffee support is voluntary project support only. It does not unlock app functionality, provide medical services, create priority health support, create a CareNest account, or cause CareNest to transmit health records to the funding site.

---

## Implemented product scope

The release-candidate source includes:

- local-first onboarding and privacy disclosure;
- no required account, server or network connection;
- multiple local family profiles;
- optional app lock and secure secret storage;
- optional profile photos stored through the encrypted document storage path;
- emergency contacts and profile notes;
- medicine records with opaque user-entered strength/instruction text;
- active, paused, completed and archived medicine states;
- explicit daily, selected-weekday, specific-time, every-N-hours, cycle, custom date-range and as-needed schedule behavior;
- idempotent reminder occurrence materialization;
- scheduled, snoozed, taken, skipped, delayed and missed reminder states;
- follow-up reminders;
- quiet hours;
- medication log and edit history;
- appointment organizer with notes, attachments, reminders and explicit calendar export;
- encrypted local health-document vault;
- document folders, tags, import, camera/file paths, selected export and deletion;
- user-entered stock/refill tracking and correction flow;
- local caregiver/family dashboard without background sharing;
- profile JSON export;
- PDF profile summary;
- CSV upcoming schedule, medication log, missed reminder, stock/refill, appointment-history and document-list reports;
- manual password-encrypted backup/restore;
- schema-versioned restore validation and rollback behavior;
- portable recovery of encrypted document data through the encrypted backup payload;
- system/light/dark theme handling;
- large-interface and reduced-motion preferences;
- notification diagnostics;
- redacted schedule inspector;
- time-zone simulation that does not rewrite stored schedules;
- sanitized diagnostic export that excludes health-document contents;
- database migration version display;
- storage usage and cache controls;
- About, license, privacy, terms, support, business contact and open-source surfaces;
- voluntary Buy Me a Coffee project-support action;
- GitHub funding metadata;
- custom CareNest BMC vector artwork and clickable repository support pages;
- original CareNest SVG app-icon/splash/mark assets;
- unit, integration and UI-contract tests;
- GitHub Actions cross-platform CI;
- CodeQL analysis;
- Dependabot configuration;
- architecture, security, privacy, testing, setup, troubleshooting and release documentation;
- repeatable Bash and PowerShell release-preflight scripts;
- manual cross-platform release test matrix;
- store-submission checklist;
- SQLite dependency migration/verification plan.

---

## Repository structure

The requested multi-project solution separation is present:

```text
src/
  CareNest.App/                 # .NET MAUI UI, resources and platform integrations
  CareNest.Domain/              # entities, enums and domain rules
  CareNest.Application/         # use cases, contracts, planners and coordinators
  CareNest.Infrastructure/      # SQLite, encrypted files, backup and reports
  CareNest.Shared/              # shared constants and primitives

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
  SUPPORT_CARENEST.md

build/
  scripts/

.github/
  ISSUE_TEMPLATE/
  workflows/
  FUNDING.yml

BUY_ME_A_COFFEE.md
```

Required repository files are present, including `README.md`, `LICENSE`, `CONTRIBUTING.md`, `CODE_OF_CONDUCT.md`, `SECURITY.md`, `SUPPORT.md`, `PRIVACY.md`, `TERMS.md`, `CHANGELOG.md`, `.gitignore`, `.editorconfig`, `Directory.Build.props`, `Directory.Packages.props`, `PROJECT_STATUS.md`, `DECISIONS.md`, issue templates, pull-request template, CI, CodeQL and Dependabot configuration.

---

## Delivery phases completed

### Phase 0 — repository, architecture, privacy and design foundation

Completed:

- multi-project solution and dependency boundaries;
- repository standards, analyzers, central packages, editor configuration and ignores;
- Apache-2.0 license and notices;
- contribution, conduct, security, support, privacy and terms documentation;
- architecture decision records;
- database schema documentation;
- threat model and data-lifecycle documentation;
- design system and localization readiness;
- store asset guidance;
- original CareNest branding vector assets;
- issue templates, PR template, Dependabot, CI and CodeQL.

Original phase label:

`chore: establish CareNest architecture and repository standards`

### Phase 1 — domain, persistence, encryption and application services

Completed:

- requested domain entities and enums;
- validation preserving strength/instruction text as opaque user-entered text;
- SQLite persistence and migrations through schema version 5;
- repository, settings and audit-entry infrastructure;
- deterministic reminder-occurrence keys;
- idempotent reminder materialization;
- explicit time-zone-aware reminder planning;
- AES-256-GCM encrypted document vault;
- password-encrypted schema-versioned backup format with authenticated encryption;
- restore validation and rollback flow;
- portable encrypted-document key recovery inside the password-protected backup payload;
- JSON/PDF/CSV export/report services;
- profile, medicine, appointment, document, reminder and backup-reminder services.

Original phase label:

`feat: implement local-first domain persistence and safety services`

### Phase 2 — complete MAUI user workflows

Completed:

- startup/onboarding flow;
- no-account local-first setup;
- optional app lock;
- dashboard;
- profiles and profile editor;
- medicines and medicine editor;
- schedule editor;
- medication log;
- appointments and appointment editor;
- document organizer;
- reports;
- settings and developer diagnostics;
- lock screen;
- About/open-source/support/legal surfaces;
- theme handling;
- accessibility-oriented semantics and scalable controls;
- localization-ready resources;
- voluntary Buy Me a Coffee support action on the project/support surface.

Original phase label:

`feat: add CareNest MAUI workflows and accessible navigation`

### Phase 3 — platform reminder integrations and reliability

Completed:

- Android alarm scheduling;
- exact/inexact fallback diagnostics;
- Android reboot/time/time-zone rebuild receiver;
- Android battery-optimization warning;
- iOS local notifications;
- Mac Catalyst local notifications;
- Windows fallback diagnostics with explicit limitation language;
- startup reminder rebuild;
- overdue reconciliation;
- appointment-reminder rebuilding;
- backup-reminder rebuilding;
- notification permission requested when a user first explicitly creates/saves a reminder-capable feature rather than during onboarding;
- stored schedule times are not silently rewritten after time-zone changes.

Original phase label:

`feat: add reminders encryption backup reports and platform integrations`

### Phase 4 — tests and release engineering

Completed:

- domain validation tests;
- reminder-planner unit tests;
- explicit multi-time schedule tests;
- as-needed behavior tests;
- interval schedule tests;
- stable occurrence-key tests;
- ambiguous local-time handling tests;
- SQLite migration/integrity tests;
- repository round-trip tests;
- cascade-delete tests;
- encrypted document round-trip/tamper tests;
- encrypted backup restore/wrong-password/tamper tests;
- WAL journal-mode regression test;
- SQLite busy-timeout regression test;
- WAL-backed snapshot regression test;
- report/export safety tests;
- XAML/UI-contract tests;
- BMC support-surface consistency tests;
- Android Release CI build;
- Windows Release CI build;
- iOS simulator Release CI build;
- Mac Catalyst Release CI build;
- CodeQL security analysis;
- release checklist and troubleshooting documentation.

Original phase label:

`test: add quality gates documentation and release readiness`

### Phase 5 — release-preparation hardening and project-support presentation

Completed in the latest continuation:

- custom scalable CareNest Buy Me a Coffee vector badge;
- root-level clickable BMC support page;
- documentation-level clickable BMC support page;
- explicit third-party/external-link privacy and medical-entitlement boundaries;
- Bash release-preflight script;
- PowerShell release-preflight script;
- cross-platform manual release test matrix;
- store-submission checklist covering Android, Windows, iOS and Mac Catalyst;
- store-policy review gate for the external BMC link;
- SQLite dependency migration/upgrade verification plan;
- expanded project status and release checklist;
- expanded handoff documentation in this file.

---

## Initial GitHub delivery

The release-candidate source was assembled on branch:

`release/carenest-1.0.0-rc.1`

and merged through pull request #3:

`CareNest 1.0.0-rc.1 complete implementation`

Merge commit:

`1244ed7fead73821f768f5119230dd6b8c24113f`

The implementation was intentionally delivered as a coherent source tree rather than allowing CI to run against half-created phase snapshots.

Important commits from the initial implementation/hardening history include:

- `ci: add cross-platform CareNest build and test workflow`;
- `ci: add CodeQL security analysis`;
- `fix: scope analyzer exceptions for shared primitives`;
- `ci: isolate platform target frameworks per runner`;
- `fix: use valid rule parameter names in profile validation`;
- `fix: use valid appointment validation parameter name`;
- `fix: use valid medicine rule parameter names`;
- `fix: resolve reminder planner performance analyzer findings`.

---

## Post-merge verification and hardening work

The first full GitHub-hosted verification exposed issues that static source inspection alone could not prove. Those findings were corrected one logical change at a time with separate commits.

### 1. CA1848 logging analyzer blocked compilation

GitHub Actions initially promoted CA1848 logging-performance guidance to an error in `ReminderCoordinator.cs`.

This did not represent incorrect reminder behavior or unsafe health-data logging. The rule recommends precompiled logger delegates for performance.

Fix:

- `7fed6d76ae2407d17bf3b19e8e4b112b3f39e279` — `ci: keep CA1848 logging optimization non-blocking`.

The rule remains visible as a suggestion rather than hiding real compile/test failures.

### 2. SQLitePCLRaw security advisory was exposed by NuGet audit

NuGet audit reported high-severity advisory:

`GHSA-2m69-gcr7-jv3q`

against SQLitePCLRaw native package `2.1.11` resolved through the current `sqlite-net-pcl` dependency chain.

An attempted move to `SQLitePCLRaw.bundle_green` `2.1.12` failed because the GitHub-hosted restore reported no such bundle version was available. Therefore the repository was corrected back to an actually restorable dependency graph rather than retaining an impossible pin.

Relevant history includes:

- `7489b70f0cf37be7545e1ecb338fec6a7ccf90dd` — initial attempted security update, later corrected after NuGet restore proved the requested bundle version unavailable;
- `build: restore available SQLitePCLRaw package versions`;
- `security: document temporary SQLite audit suppression in build`;
- `security: add dependency risk register for SQLite advisory`;
- `docs: link dependency risk register from security policy`.

Current accurate state:

- SQLitePCLRaw native package `2.1.11` remains in the currently tracked dependency path;
- the exact advisory URL is temporarily listed through `NuGetAuditSuppress`;
- no wildcard or severity-wide NuGet audit suppression is used;
- `docs/security/DEPENDENCY_RISK_REGISTER.md` marks the risk **open**;
- the suppression is not represented as a vulnerability fix;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` defines the upgrade/provider replacement validation path;
- final production release remains gated on an explicit dependency-risk decision/resolution.

The local-first architecture, absence of a remote database listener, absence of arbitrary user-supplied SQL execution and parameterized repository operations reduce exposure but do not erase the advisory.

### 3. Test naming analyzer prevented descriptive xUnit method names

CA1707 was being promoted to an error for descriptive underscore-separated test names.

Fix:

- `test: scope CA1707 away from descriptive test names`.

The exception is scoped to tests.

### 4. Infrastructure recommendation analyzers hid functional failures

Several performance/culture analyzer recommendations were promoted to errors before CI could reach real runtime/compile failures.

Fixes included:

- `ci: keep advisory infrastructure analyzers non-blocking`;
- `988fffd85f792cad58c09aaf2286398d0a4294c1` — `ci: keep framework binding and delegate naming analyzers advisory`.

Correctness/security rules were not globally disabled. Platform availability CA1416 was fixed in Android code rather than suppressed globally.

### 5. Per-run TargetFrameworks override leaked into referenced projects

The original CI command supplied `TargetFrameworks=...` globally. MSBuild propagated that property into referenced `net10.0` projects and generated invalid/missing restore assets.

Fixes included:

- `build: add non-propagating platform target selector`;
- `ci: select one MAUI target per runner without leaking to references`.

`CareNest.App.csproj` accepts `CareNestTargetFramework`, allowing CI/development hosts to narrow the MAUI app before restore/build without changing target frameworks in Domain/Application/Infrastructure/Shared projects.

### 6. SQLite initialization failed because result-producing PRAGMAs used ExecuteAsync

GitHub-hosted integration tests exposed sqlite-net behavior where result-producing PRAGMAs returned `SQLITE_ROW`; treating them as non-query `ExecuteAsync` operations surfaced misleading failures.

Fixes included:

- `fix: read SQLite journal mode result during initialization`;
- `test: assert CareNest enables SQLite WAL mode`;
- `fix: read SQLite busy-timeout pragma result`;
- `test: assert SQLite busy-timeout configuration`;
- `fix: consume SQLite WAL checkpoint result for backups`;
- `test: cover WAL-backed database snapshot creation`.

Current behavior:

- `PRAGMA journal_mode = WAL` is read with a scalar result and validated;
- `PRAGMA busy_timeout = 5000` is read with a scalar result and validated;
- `PRAGMA wal_checkpoint(FULL)` is consumed as a result before backup snapshot creation;
- regression tests verify WAL mode, busy timeout and snapshot creation.

This corrected all integration failures that had previously occurred before backup/restore test logic could run.

### 7. Report-service analyzer findings were cleaned up

Fix:

- `perf: reuse report serializer settings and invariant export formatting`.

The report service reuses serializer settings and uses explicit invariant formatting where appropriate for machine-readable exported values.

### 8. MAUI package/source isolation was incomplete

GitHub platform builds exposed that the app needed an explicit MAUI Controls package reference and stronger platform-source isolation.

Fixes included:

- `build: pin explicit MAUI Controls package`;
- `build: reference MAUI Controls from app project`;
- `build: compile only the active platform source tree`;
- explicit MAUI application global usings;
- `build: make MAUI and DI namespaces explicit`.

Inactive platform source trees are excluded when they do not match the current target.

### 9. Android time-zone intent constant was incorrect

Fix:

- `fix: use Android timezone-change intent constant`.

The receiver now uses the Android binding constant that actually exists for time-zone change events.

### 10. Apple CI runner/Xcode mismatch

The .NET 10 Apple workload installed by CI required a current Xcode toolchain. The earlier runner selected an incompatible Xcode version and failed before application compilation.

Fix:

- `ci: use macOS 26 runner for current Xcode toolchain`.

The macOS 26 runner was accepted and subsequently compiled both iOS simulator and Mac Catalyst Release targets successfully.

### 11. Shared MAUI source compile errors

Once restore/toolchain issues were removed, CI reached source compilation and exposed several shared C# errors.

Fixes included:

- `fix: give startup destination switch an explicit Page type`;
- `fix: make schedule editor nullable values and collections explicit`;
- `e292fc963cc693943566a23052e3342ea31d0d33` — `fix: use scheduled reminder timestamps in redacted diagnostics`;
- `c0b90ce1...` — `perf: avoid LINQ for active window lookup`;
- `b4f8972f...` — `perf: avoid LINQ for navigation window lookup`.

The reminder preview contract exposes the actual scheduled timestamp used by developer diagnostics instead of referencing a nonexistent property.

### 12. Android notification integration required explicit platform hardening

After shared C# fixes, Android compiler/analyzers exposed nullable Java binding values and API-level availability concerns.

Fix:

- `682aef2aa31981c6be31086aa7af8e1c8e56e94b` — `fix: harden Android notification nullability and API guards`.

The Android implementation includes:

- explicit application-context validation;
- explicit package-name validation;
- notification-manager validation;
- explicit pending-intent null handling;
- explicit notification-build null handling;
- API 31 exact-alarm checks guarded by platform version;
- API 26 notification-channel creation guarded by platform version;
- reusable static silent-vibration pattern;
- platform availability problems solved in code instead of globally suppressing CA1416.

The Android Release build passed after these changes.

### 13. Documentation build commands synchronized with verified project model

README and development setup use `CareNestTargetFramework` for narrow-workload target builds so contributors do not accidentally evaluate unrelated MAUI targets.

### 14. Release records corrected after verification

Project status, release checklist, README/build guidance and the handoff were updated after actual CI evidence was available rather than claiming unverified results.

---

## Funding integration completed before the latest artwork pass

The project-support destination is:

`https://buymeacoffee.com/sanskarIN`

The implementation already included:

- `AppConstants.FundingUrl` as the shared runtime source for the URL;
- an About/support command that opens the external destination only after explicit user action;
- GitHub `.github/FUNDING.yml` metadata;
- README/SUPPORT/PRIVACY/TERMS/security/store guidance explaining the funding boundary;
- UI-contract coverage to prevent support URL/wording drift;
- no feature unlock, account creation, premium medical service, diagnosis, treatment or reminder advantage tied to financial support.

The funding-enabled verification pass added two UI-contract tests, increasing the UI-contract suite from 8 to 10 tests.

---

## Custom BMC artwork and highlighting added in the latest continuation

### Vector artwork

Added:

`src/CareNest.App/Resources/Images/buy_me_a_coffee_carenest.svg`

The asset is a custom CareNest project-support graphic with:

- a warm yellow/coffee-brown visual treatment;
- a custom coffee-cup illustration;
- prominent `BUY ME A COFFEE` heading;
- visible `buymeacoffee.com/sanskarIN` text;
- `SUPPORT CARENEST` callout;
- scalable vector format suitable for repository/app resource use;
- descriptive SVG `<title>` and `<desc>` metadata.

The asset is custom CareNest project artwork and is not represented as an official Buy Me a Coffee brand logo.

### Clickable support pages

Added:

- `BUY_ME_A_COFFEE.md` at repository root;
- `docs/SUPPORT_CARENEST.md`.

Both pages make the vector artwork clickable and point to:

`https://buymeacoffee.com/sanskarIN`

Both pages also state that support is voluntary and does not create CareNest feature/medical/privacy entitlements.

### Existing repository highlighting retained

The existing GitHub funding metadata and textual support surfaces remain in place, so the BMC destination is discoverable through both native repository funding UI and project documentation.

---

## Release-preflight automation added

### Bash

Added:

`build/scripts/release-preflight.sh`

The script:

- requires a .NET SDK;
- prints the installed .NET environment;
- scans `src/` and `tests/` for `TODO`, `FIXME` and `NotImplementedException` implementation markers;
- runs `dotnet format CareNest.sln --verify-no-changes`;
- builds Domain/Application/Infrastructure in Release mode;
- runs unit, integration and UI-contract tests;
- emits a dependency vulnerability report for the infrastructure dependency graph;
- optionally builds a selected MAUI target when `CARENEST_TARGET` is set;
- explicitly reminds maintainers that manual/device/accessibility/signing/store-policy/dependency-risk checks remain separate.

### PowerShell

Added:

`build/scripts/release-preflight.ps1`

It provides the same release-preflight intent for Windows/PowerShell hosts with explicit `$LASTEXITCODE` checks around build/test/format commands.

The scripts do not claim to replace GitHub-hosted platform builds or real-device testing.

---

## Manual release matrix added

Added:

`docs/releases/MANUAL_TEST_MATRIX.md`

The matrix covers:

- fresh install/onboarding;
- profiles and app lock;
- medicine/schedule variants;
- reminder state transitions;
- quiet hours/follow-ups;
- appointments/calendar export;
- encrypted documents;
- stock/refill behavior;
- PDF/CSV/JSON reports;
- encrypted backup/restore/wrong-password behavior;
- reset/deletion;
- themes, large text, screen reader, keyboard and reduced motion;
- Buy Me a Coffee external-action behavior;
- offline/local-first use;
- Android notification permission/exact alarm/battery/reboot/time/time-zone scenarios;
- iOS/Mac Catalyst notification scenarios;
- Windows reminder limitation scenarios;
- privacy/security checks;
- release evidence rules using fictional data only.

This matrix deliberately leaves device-dependent rows unchecked until they are actually executed on the intended release devices/simulators.

---

## Store submission checklist added

Added:

`docs/releases/STORE_SUBMISSION_CHECKLIST.md`

It covers:

- final version/build metadata;
- exact-source CI/CodeQL evidence;
- dependency/advisory review;
- package identities and signing secrets;
- non-medical store claims;
- reminder limitation wording;
- privacy/data-safety disclosures;
- fictional screenshot/test data requirements;
- current-policy review for BMC/external funding links;
- Android signing/AAB/device checks;
- Windows signing/package identity checks;
- iOS signing/privacy/notification checks;
- Mac Catalyst signing/distribution/accessibility checks;
- release artifact/source SHA/run-ID recording;
- explicit rule not to publish final `1.0.0` solely because compilation is green.

The checklist does not guess current store rules. It requires a current review at submission time because platform policies can change.

---

## SQLite dependency migration plan added

Added:

`docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`

The plan defines two safe paths:

1. upgrade to a compatible patched dependency graph when one is actually available;
2. replace the SQLite provider/native dependency path if necessary, while preserving the Infrastructure boundary and local-first design.

Required post-change evidence includes:

- unit/integration/UI-contract tests;
- clean database initialization;
- migration/foreign-key/repository/cascade behavior;
- WAL/busy-timeout or intentionally redesigned equivalent behavior;
- backup snapshot and encrypted backup/restore tests;
- document/reference survival;
- Android/Windows/iOS/Mac Catalyst builds;
- CodeQL;
- upgrade of a fictional pre-existing device database;
- pre/post migration backup tests;
- reminder rebuild verification;
- exact dependency graph and CI/run evidence in the risk register and handoff.

The plan explicitly forbids claiming the advisory fixed merely because a suppression exists.

---

## Verification pull-request sequence

Temporary verification branches/PRs were used to obtain fresh pull-request-triggered GitHub Actions evidence without merging marker files into production source.

- PR #10 — first post-merge verification; superseded after new failures were discovered.
- PR #11 — analyzer/NuGet correction verification; superseded.
- PR #12 — framework-selection verification; superseded.
- PR #13 — SQLite/MAUI CI verification; superseded.
- PR #14 — MAUI/SQLite/shared-source verification; superseded after additional source-level failures were exposed.
- PR #15 — full cross-platform verification for runtime source head `682aef2aa31981c6be31086aa7af8e1c8e56e94b`.
- PR #16 — funding-enabled verification for source head `2b8f97525ea8d3b41bf62e20d76e1cc224dab102`.

Verification markers changed only verification text files and were not merged into production `main`.

---

## Latest completed automated verification evidence

### CareNest CI

GitHub Actions workflow:

- workflow: `CareNest CI`;
- run number: `87`;
- run id: `31301203981`;
- final status: `completed`;
- final conclusion: `success`;
- source head under verification: `2b8f97525ea8d3b41bf62e20d76e1cc224dab102`.

### Core tests

Exact results:

- `CareNest.UnitTests`: 15 passed, 0 failed, 0 skipped;
- `CareNest.IntegrationTests`: 11 passed, 0 failed, 0 skipped;
- `CareNest.UiTests`: 10 passed, 0 failed, 0 skipped.

Total automated tests in that run:

- 36 passed;
- 0 failed;
- 0 skipped.

The two additional UI-contract tests cover the BMC support URL/surface consistency.

### Android

Release build succeeded for:

`net10.0-android`

using the non-propagating `CareNestTargetFramework` selection model.

### Windows

Release build succeeded for:

`net10.0-windows10.0.19041.0`.

### iOS

Release simulator build succeeded for:

`net10.0-ios`

with the simulator runtime used by the GitHub Apple job.

### Mac Catalyst

Release build succeeded for:

`net10.0-maccatalyst`.

### CodeQL

GitHub Actions workflow:

- workflow: `CodeQL`;
- run number: `86`;
- run id: `31301203985`;
- final status: `completed`;
- final conclusion: `success`.

---

## Why another final exact-commit build is still required

The latest completed CI evidence above verifies the funding-enabled runtime source before the custom vector badge/release-preparation files were added.

The new BMC SVG is stored inside `CareNest.App/Resources/Images`. Even though it does not change reminder/persistence/medical logic, it is part of the MAUI resource tree. Therefore the exact final packaging commit should receive another Android/Windows/iOS/Mac Catalyst build and CodeQL pass before signed/public release.

That requirement is intentionally left open in `docs/releases/RELEASE_CHECKLIST.md` until a fresh verification is completed for the exact final source head.

---

## Static/source hygiene

Repository hardening previously checked for and removed release-blocking placeholders.

Release preflight now contains an explicit source scan for:

- `TODO`;
- `FIXME`;
- `NotImplementedException`.

Earlier static review also checked XML/XAML well-formedness, project-reference resolution, XAML `x:Class`/code-behind matching and stale settings-key references before the release branch was assembled.

A generation-script string-quoting error occurred during an early local assembly pass; that pass did not write repository source. The generator input was corrected before the release tree was delivered.

---

## Security model implemented

- Imported documents and profile photos use encrypted `.cndoc` storage.
- Document encryption uses authenticated AES-256-GCM primitives rather than custom cryptography.
- Backups use password-derived authenticated encryption and a schema-versioned format.
- The encrypted backup payload carries the document key material needed for portable restore without storing it in plaintext.
- App-lock PINs are not stored directly; a salted password-derived verifier is stored through secure platform storage.
- Diagnostics/logs intentionally omit health-document contents, PINs, backup passwords and sensitive note values.
- No API keys, signing keys, certificates, passwords or production secrets are committed.
- No analytics or telemetry were added.
- No CareNest backend, cloud sync, account system or automatic upload exists in this release.
- Exported/decrypted files leave CareNest protection only after explicit user action.
- SQLite records rely on application sandbox/device protections; CareNest does **not** claim transparent whole-database encryption at rest.
- The open SQLitePCLRaw advisory is explicitly tracked instead of being hidden behind a claim that it was fixed.
- External links such as GitHub/BMC are explicit user actions and leave the CareNest trust boundary.

---

## Reminder reliability rules implemented

- reminder schedules originate only from explicit user input;
- reminder occurrences have stable deterministic keys;
- rebuilds are idempotent;
- future reminders are rebuilt at startup;
- overdue occurrences are reconciled;
- Android responds to reboot/time/time-zone rebuild signals;
- stored schedule times are not silently rewritten after time-zone changes;
- notification permission denial is surfaced;
- exact-alarm/battery limitations are surfaced on Android;
- iOS/Mac Catalyst use OS-managed local notifications;
- Windows fallback limitations are reported rather than hidden;
- quiet hours are user-controlled;
- follow-up reminders are user-controlled;
- stock changes after a Taken event use only the user-configured quantity change;
- stock estimates explicitly warn that users should check actual supply.

---

## Acceptance-criteria mapping

- No account/network required: implemented; no CareNest backend/login/cloud sync exists.
- No diagnosis/treatment/dosage decisions: enforced through scope, UI language and domain/application behavior.
- Reminder recovery: startup rebuild plus Android boot/time/time-zone rebuild integration implemented.
- Permission/battery limitations: surfaced through diagnostics and user-facing warnings.
- Profile export/delete: implemented.
- Document export/delete: implemented.
- Logs exclude health-document contents: implemented through sanitized diagnostics and privacy-aware logging.
- Medical limitations in onboarding/About: implemented.
- Manual encrypted backup/restore: implemented with version/integrity validation and rollback handling.
- No automatic cloud upload: implemented by architecture; no cloud service exists in v1.
- Local caregiver mode: implemented without silent sharing.
- Theme/accessibility/localization readiness: implemented with system/light/dark preferences, scalable UI tokens, reduced-motion behavior and English resource architecture ready for additional languages.
- Automated quality gate: latest completed funding-enabled source head passed unit/integration/UI-contract tests, Android, Windows, iOS simulator, Mac Catalyst and CodeQL.
- Voluntary project support: BMC URL is centralized, documented, tested and separated from medical/feature entitlements.
- Visual project-support highlight: custom vector badge and clickable support pages are implemented.
- Release-preparation tooling: preflight scripts, manual matrix, store checklist and dependency migration plan are implemented.

---

## Platform limitations retained intentionally

### Android

Reminder timing can be affected by notification permission, exact-alarm capability, battery optimization, force-stop state and operating-system policy. CareNest reports those limitations rather than guaranteeing delivery.

### iOS / Mac Catalyst

Local notification delivery remains controlled by operating-system notification policy. CI build success proves compilation, not guaranteed real-device notification delivery.

### Windows

The current fallback cannot guarantee reminder delivery while CareNest is not running. The application reports this limitation instead of pretending background delivery is guaranteed.

### All platforms

Device shutdown, permission revocation, platform scheduling policy, system updates and battery-management behavior can affect reminder delivery.

---

## Commit identity note

The requested maintainer commit email is configured in repository setup documentation/scripts:

- `build/scripts/setup-git.sh`;
- `build/scripts/setup-git.ps1`;
- `docs/setup/DEVELOPMENT.md`.

Configured command:

```bash
git config user.email "sanskarin@outlook.in"
git config user.name "Sanskar"
```

The connected GitHub write API used in this chat does not expose author/committer email fields on the create/update file operations. Connector-created commits therefore use the authenticated GitHub identity. This repository does not falsely claim that the connector forced `sanskarin@outlook.in` into those commit objects.

Local/future maintainer commits can use the requested address through the included setup scripts.

---

## Documentation-write recovery note

During this latest continuation, an attempted handoff-file update wrote a temporary placeholder to `what_changed.md`. The repository was immediately restored to the last known-good handoff commit before replaying the valid BMC/release-hardening changes.

The recovery intentionally removed the temporary placeholder state from `main` before the valid continuation commits were replayed. No CareNest runtime, health data model, reminder logic, encryption logic, database logic or user data was affected by that documentation-only recovery.

This section is included for traceability rather than hiding the repository maintenance correction.

---

## Environment and release limitations

The local execution container used for repository assembly does not contain the .NET SDK/MAUI workloads. Therefore local `dotnet restore`, `dotnet format`, platform compilation, emulator/device smoke tests, signing and store packaging cannot truthfully be claimed as executed inside that container.

GitHub-hosted CI provided the automated verification described above for the latest completed verified runtime source.

The following activities remain intentionally **not marked complete** because they require an appropriately provisioned development host, real/emulated devices, signing credentials, current store access/policy review, or a verified dependency update:

- run the new release-preflight script on a fully provisioned host;
- fresh exact-final-commit Android/Windows/iOS/Mac Catalyst builds after the BMC SVG resource addition;
- fresh exact-final-commit CodeQL;
- manual onboarding/profile/medicine/schedule workflows on target devices;
- notification permission denied/granted manual testing;
- Android exact-alarm/battery behavior on representative devices;
- real time-zone-change delivery testing;
- manual document import/export/delete testing on target devices;
- manual calendar export verification;
- manual backup restore on a release build/clean installation;
- cold-start app-lock testing;
- screen-reader/large-text/keyboard/reduced-motion manual accessibility checks;
- current app-store policy review for the external funding link;
- package signing;
- store packaging/submission;
- final review/resolution for the open SQLitePCLRaw advisory.

`docs/releases/RELEASE_CHECKLIST.md`, `docs/releases/MANUAL_TEST_MATRIX.md`, `docs/releases/STORE_SUBMISSION_CHECKLIST.md` and `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` keep automated evidence separate from manual/store/security activities so nothing is silently represented as verified when it is not.

---

## Current repository state after this continuation

- Complete CareNest `1.0.0-rc.1` product source is on `main`.
- Buy Me a Coffee support URL is `https://buymeacoffee.com/sanskarIN`.
- Funding URL remains centralized in shared constants and existing runtime support action.
- GitHub funding metadata remains present.
- Custom BMC vector artwork is present in the MAUI image resource tree.
- Root and documentation support pages make the BMC artwork clickable.
- Bash and PowerShell release-preflight scripts are present.
- Manual release test matrix is present.
- Store submission/funding-policy checklist is present.
- SQLite dependency migration/verification plan is present.
- Latest completed funding-enabled verification: CareNest CI run #87 and CodeQL run #86, both successful.
- Latest completed test counts: 15 unit + 11 integration + 10 UI-contract = 36 passed, 0 failed, 0 skipped.
- Latest completed platform verification: Android, Windows, iOS simulator and Mac Catalyst Release builds passed.
- A new exact-final-commit platform/CodeQL verification is still required because a new MAUI SVG resource was added after run #87.
- The SQLitePCLRaw advisory remains explicitly open and is not claimed fixed.
- Final `1.0.0` publication/tagging remains gated on fresh exact-commit automated verification, manual release matrix, accessibility/device checks, current BMC/store-policy review, signing/packaging and the dependency-risk decision/resolution.
- Cloud synchronization, remote caregiver collaboration, accounts/mobile-number authentication, server-side storage, medical interpretation, diagnosis, treatment advice, medication-interaction claims and clinical risk scoring remain deferred to later separately reviewed versions.
