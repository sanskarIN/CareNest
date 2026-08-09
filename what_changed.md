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

All reminder schedules come from explicit user input. Medicine strength and instruction text are stored as entered and are not interpreted as dosage rules. `StockChangePerTakenEvent` is also explicitly user-entered and is never inferred from medicine strength or instruction text.

Reminder delivery limitations are surfaced instead of hidden. Device permissions, battery optimization, exact-alarm capability, operating-system restrictions, shutdown/force-stop behavior, daylight-saving changes and time-zone changes can affect delivery.

The application tells users to follow qualified professional instructions and to contact local emergency services in an emergency rather than rely on CareNest.

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
- voluntary Buy Me a Coffee project-support action with no health-feature entitlement;
- GitHub funding metadata for the same voluntary support destination;
- original CareNest SVG app-icon/splash/mark assets;
- unit, integration and UI-contract tests;
- GitHub Actions cross-platform CI;
- CodeQL analysis;
- Dependabot configuration;
- architecture, security, privacy, testing, setup, troubleshooting and release documentation.

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

build/
  scripts/
  verification/                 # verification-only branch markers, never runtime product data

.github/
  ISSUE_TEMPLATE/
  workflows/
```

Required repository files are present, including `README.md`, `LICENSE`, `CONTRIBUTING.md`, `CODE_OF_CONDUCT.md`, `SECURITY.md`, `SUPPORT.md`, `PRIVACY.md`, `TERMS.md`, `CHANGELOG.md`, `.gitignore`, `.editorconfig`, `Directory.Build.props`, `Directory.Packages.props`, `PROJECT_STATUS.md`, `DECISIONS.md`, issue templates, pull-request template, CI, CodeQL, Dependabot configuration and GitHub funding metadata.

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
- localization-ready resources.

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
- Android Release CI build;
- Windows Release CI build;
- iOS simulator Release CI build;
- Mac Catalyst Release CI build;
- CodeQL security analysis;
- release checklist and troubleshooting documentation.

Original phase label:

`test: add quality gates documentation and release readiness`

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

- `ci: add cross-platform CareNest build and test workflow`
- `ci: add CodeQL security analysis`
- `fix: scope analyzer exceptions for shared primitives`
- `ci: isolate platform target frameworks per runner`
- `fix: use valid rule parameter names in profile validation`
- `fix: use valid appointment validation parameter name`
- `fix: use valid medicine rule parameter names`
- `fix: resolve reminder planner performance analyzer findings`

---

## Post-merge verification and hardening work

The first full GitHub-hosted verification exposed issues that static source inspection alone could not prove. Those findings were corrected one logical change at a time with separate commits.

### 1. CA1848 logging analyzer blocked compilation

GitHub Actions initially promoted CA1848 logging-performance guidance to an error in `ReminderCoordinator.cs`.

This did not represent incorrect reminder behavior or unsafe health-data logging. The rule recommends precompiled logger delegates for performance.

Fix:

- commit `7fed6d76ae2407d17bf3b19e8e4b112b3f39e279`
- `ci: keep CA1848 logging optimization non-blocking`

The rule remains visible as a suggestion rather than hiding real compile/test failures.

### 2. SQLitePCLRaw security advisory was exposed by NuGet audit

NuGet audit reported high-severity advisory:

`GHSA-2m69-gcr7-jv3q`

against SQLitePCLRaw native package `2.1.11` resolved through the current `sqlite-net-pcl` dependency chain.

An attempted move to `SQLitePCLRaw.bundle_green` `2.1.12` failed because NuGet.org reported no such bundle version was available. Therefore the repository was corrected back to actual available package versions rather than retaining an impossible pin.

Relevant commits:

- `7489b70f0cf37be7545e1ecb338fec6a7ccf90dd` — initial attempted security update, later corrected after NuGet restore proved the requested version unavailable;
- `eda483e...` — `build: restore available SQLitePCLRaw package versions`;
- `a09fefd...` — `security: document temporary SQLite audit suppression in build`;
- `1c5f569...` — `security: add dependency risk register for SQLite advisory`;
- `c8928ec...` — `docs: link dependency risk register from security policy`.

Current accurate state:

- SQLitePCLRaw native packages remain at available `2.1.11` through the current dependency chain;
- the exact advisory URL is temporarily listed through `NuGetAuditSuppress`;
- no wildcard or severity-wide NuGet audit suppression is used;
- `docs/security/DEPENDENCY_RISK_REGISTER.md` marks the risk **open**;
- the suppression is not represented as a vulnerability fix;
- final production release review must upgrade or replace the dependency path when an available compatible patched version exists.

The local-first architecture, absence of a remote database listener, absence of arbitrary user-supplied SQL execution and parameterized repository operations reduce exposure but do not erase the advisory.

### 3. Test naming analyzer prevented descriptive xUnit method names

CA1707 was being promoted to an error for descriptive underscore-separated test names.

Fix:

- `87116d2...` — `test: scope CA1707 away from descriptive test names`

The exception is scoped to tests.

### 4. Infrastructure recommendation analyzers hid functional failures

Several performance/culture analyzer recommendations were promoted to errors before CI could reach real runtime/compile failures.

Fixes included:

- `45a0095...` — `ci: keep advisory infrastructure analyzers non-blocking`;
- `988fffd85f792cad58c09aaf2286398d0a4294c1` — `ci: keep framework binding and delegate naming analyzers advisory`.

Correctness/security rules were not globally disabled. Platform availability CA1416 was fixed in Android code rather than suppressed globally.

### 5. Per-run TargetFrameworks override leaked into referenced projects

The original CI command supplied `TargetFrameworks=...` globally. MSBuild propagated that property into referenced `net10.0` projects and generated invalid/missing restore assets.

Fixes:

- `4249ddb...` — `build: add non-propagating platform target selector`;
- `d589405...` — `ci: select one MAUI target per runner without leaking to references`.

`CareNest.App.csproj` now accepts `CareNestTargetFramework`, allowing CI/development hosts to narrow the MAUI app before restore/build without changing target frameworks in Domain/Application/Infrastructure/Shared projects.

### 6. SQLite initialization failed because result-producing PRAGMAs used ExecuteAsync

GitHub-hosted integration tests exposed sqlite-net behavior where result-producing PRAGMAs returned `SQLITE_ROW`; treating them as non-query `ExecuteAsync` operations surfaced misleading `not an error` exceptions.

Fixes:

- `8c039ae...` — `fix: read SQLite journal mode result during initialization`;
- `cd7087e...` — `test: assert CareNest enables SQLite WAL mode`;
- `6d1ee4d...` — `fix: read SQLite busy-timeout pragma result`;
- `e9e9795...` — `test: assert SQLite busy-timeout configuration`;
- `b8f4e8e...` — `fix: consume SQLite WAL checkpoint result for backups`;
- `e9d7f28...` — `test: cover WAL-backed database snapshot creation`.

Current behavior:

- `PRAGMA journal_mode = WAL` is read with `ExecuteScalarAsync<string>` and validated;
- `PRAGMA busy_timeout = 5000` is read with `ExecuteScalarAsync<int>` and validated;
- `PRAGMA wal_checkpoint(FULL)` is consumed as a scalar result before `VACUUM INTO` backup snapshot creation;
- regression tests verify WAL mode, busy timeout and snapshot creation.

This corrected all integration failures that had previously occurred before backup/restore test logic could run.

### 7. Report-service analyzer findings were cleaned up

Fix:

- `63dbdab...` — `perf: reuse report serializer settings and invariant export formatting`

The report service now reuses serializer settings and uses explicit invariant formatting where appropriate for machine-readable exported values.

### 8. MAUI package/source isolation was incomplete

GitHub platform builds exposed that the app needed an explicit MAUI Controls package reference and stronger platform-source isolation.

Fixes:

- `53a8a7d...` — `build: pin explicit MAUI Controls package`;
- `cc8c030...` — `build: reference MAUI Controls from app project`;
- `9c8989a...` — `build: compile only the active platform source tree`;
- `97e6707...` — added MAUI application global usings;
- `523239d...` — `build: make MAUI and DI namespaces explicit`.

Inactive `Platforms/Android`, `Platforms/iOS`, `Platforms/MacCatalyst` and `Platforms/Windows` C# source trees are excluded when they do not match the current target.

### 9. Android time-zone intent constant was incorrect

Fix:

- `fab3380...` — `fix: use Android timezone-change intent constant`

The receiver now uses the Android binding constant that actually exists for time-zone change events.

### 10. Apple CI runner/Xcode mismatch

The .NET 10 iOS workload installed by CI required a current Xcode toolchain. The earlier `macos-15` runner selected Xcode 16.4 and failed before application compilation.

Fix:

- `9132ed4...` — `ci: use macOS 26 runner for current Xcode toolchain`

The `macos-26` runner was accepted and subsequently compiled both iOS simulator and Mac Catalyst Release targets successfully.

### 11. Shared MAUI source compile errors

Once restore/toolchain issues were removed, CI reached source compilation and exposed several shared C# errors.

Fixes:

- `b78f0529...` — `fix: give startup destination switch an explicit Page type`;
- `2c0c4e27...` — `fix: make schedule editor nullable values and collections explicit`;
- `e292fc963cc693943566a23052e3342ea31d0d33` — `fix: use scheduled reminder timestamps in redacted diagnostics`;
- `c0b90ce1...` — `perf: avoid LINQ for active window lookup`;
- `b4f8972f...` — `perf: avoid LINQ for navigation window lookup`.

The reminder preview contract exposes `ScheduledUtc`; developer diagnostics now use that real contract member instead of a nonexistent `DueUtc` property.

### 12. Android notification integration required explicit platform hardening

After shared C# fixes, the Android compiler/analyzers exposed nullable Java binding values and API-level availability concerns.

Fix:

- `682aef2aa31981c6be31086aa7af8e1c8e56e94b` — `fix: harden Android notification nullability and API guards`.

The Android implementation now includes:

- explicit application-context validation;
- explicit package-name validation;
- notification-manager validation;
- explicit pending-intent null handling;
- explicit notification-build null handling;
- API 31 exact-alarm checks guarded with `OperatingSystem.IsAndroidVersionAtLeast(31)`;
- API 26 notification-channel creation guarded with `OperatingSystem.IsAndroidVersionAtLeast(26)`;
- reusable static silent-vibration pattern;
- platform availability problems solved in code instead of globally suppressing CA1416.

The Android Release build passed after this change.

### 13. Documentation build commands were synchronized with the verified project model

Documentation fixes:

- `67cab88...` — `docs: document target-specific MAUI build commands`;
- `4dd8ef4c3a5def9e6589a01689ff9555194c3746` — `docs: align README builds with verified target selector`.

README and development setup now use `CareNestTargetFramework` for narrow-workload target builds so contributors do not accidentally evaluate unrelated MAUI targets.

### 14. Release records were corrected after verification

Documentation/status commits include:

- `fc20955dc84929ea6d7ee9aee3adf9760e66e9f3` — `docs: record rc1 hardening and verification fixes`;
- `2915aecbb4c4846a14dc12cac20810861b8f890d` — `docs: correct current rc1 verification status`;
- `76c22b226fe55efb646bbc1cf4010963e1b9ac77` — `docs: record automated rc1 release evidence`;
- `fbae16104c07432d275986b5d215e59ab15b5526` — `docs: mark automated Mac Catalyst verification green`;
- `1479c71378c836d5205c8ef373164fe6bee9e0cc` — `docs: finalize green automated rc1 verification status`.

These documentation commits were made after the verified runtime source head and do not alter product runtime behavior.

---

## Verification pull-request sequence

Temporary verification branches/PRs were intentionally used to obtain fresh pull-request-triggered GitHub Actions evidence without merging verification marker files into production source.

- PR #10 — first post-merge verification; superseded after new failures were discovered.
- PR #11 — analyzer/NuGet correction verification; superseded.
- PR #12 — framework-selection verification; superseded.
- PR #13 — SQLite/MAUI CI verification; superseded.
- PR #14 — MAUI/SQLite/shared-source verification; superseded after additional source-level failures were exposed.
- PR #15 — final cross-platform verification for product source head `682aef2aa31981c6be31086aa7af8e1c8e56e94b` before the later funding-support runtime/UI addition.

PR #15 branch:

`ci/carenest-rc1-verification-6`

Verification marker commit:

`6203225fb3e6608a78f867cc0f30352a3c014745`

The marker changed only a verification text file. It was **not** merged into `main`.

PR #15 was closed after all automated gates completed successfully.

---

## Previous automated verification evidence — PR #15

### CareNest CI

GitHub Actions workflow run:

- workflow: `CareNest CI`
- run number: `67`
- run id: `31300473171`
- final status: `completed`
- final conclusion: `success`
- source head under verification: `682aef2aa31981c6be31086aa7af8e1c8e56e94b`

### Core tests

The Core tests job completed successfully on Ubuntu 24.04 with .NET 10.0.302.

Exact results:

- `CareNest.UnitTests`: 15 passed, 0 failed, 0 skipped;
- `CareNest.IntegrationTests`: 11 passed, 0 failed, 0 skipped;
- `CareNest.UiTests`: 8 passed, 0 failed, 0 skipped.

Total automated test cases in that job:

- 34 passed;
- 0 failed;
- 0 skipped.

The integration suite includes the corrected SQLite initialization/snapshot paths as well as encrypted document/backup/report behavior.

### Android

Android MAUI workload installation succeeded.

Release build succeeded for:

`net10.0-android`

using:

`CareNestTargetFramework=net10.0-android`

### Windows

Windows MAUI workload installation succeeded.

Release build succeeded for:

`net10.0-windows10.0.19041.0`

using the non-propagating `CareNestTargetFramework` selector.

### iOS

Apple MAUI workloads installed successfully on the macOS 26 runner.

Release simulator build succeeded for:

`net10.0-ios`

with:

`RuntimeIdentifier=iossimulator-arm64`

### Mac Catalyst

Release build succeeded for:

`net10.0-maccatalyst`

on the same macOS 26 Apple job.

### CodeQL

GitHub Actions workflow run:

- workflow: `CodeQL`
- run number: `66`
- run id: `31300473160`
- final status: `completed`
- final conclusion: `success`.

---

## Static/source hygiene checks

Static review and repository searches have also been used during implementation/hardening.

Current repository search found no remaining:

- `TODO` implementation markers;
- `FIXME` implementation markers;
- `NotImplementedException` placeholders.

Earlier generation/static review also checked XML/XAML well-formedness, project-reference resolution, XAML `x:Class`/code-behind matching and stale settings-key references before the release branch was assembled.

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
- SQLite records rely on application sandbox/device protections; CareNest does **not** falsely claim transparent whole-database encryption at rest.
- The open SQLitePCLRaw advisory is explicitly tracked instead of being hidden behind a claim that it was fixed.
- The external Buy Me a Coffee support destination is fixed in a shared HTTPS constant and opened only after explicit user action.
- CareNest does not append health data, document metadata, profile identifiers, reminder history, backup data, app-lock data or payment secrets to the funding URL.
- The external funding provider is outside the CareNest trust boundary and is governed by its own privacy/security/payment rules once opened.

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
- Automated quality gate: latest funding-enabled source head passed core tests, Android, Windows, iOS simulator, Mac Catalyst and CodeQL.
- Voluntary project funding: implemented through the About page and GitHub funding metadata without changing CareNest health functionality, safety boundaries or local-data access.

---

## Platform limitations retained intentionally

### Android

Reminder timing can be affected by notification permission, exact-alarm capability, battery optimization, force-stop state and operating-system policy. CareNest reports those limitations rather than guaranteeing delivery.

### iOS / Mac Catalyst

Local notification delivery remains controlled by operating-system notification policy. The CI build proves compilation for current targets, not guaranteed real-device notification delivery.

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

The connected GitHub write API used in this chat does not expose author/committer email fields on create/update commit operations. Connector-created commits therefore use the authenticated GitHub identity. This repository does not falsely claim that the connector forced `sanskarin@outlook.in` into those commit objects.

Local/future maintainer commits can use the requested address through the included setup scripts.

---

## Environment and release limitations

The local execution container used for repository assembly does not contain the .NET SDK/MAUI workloads. Therefore local `dotnet restore`, `dotnet format`, platform compilation, emulator/device smoke tests, signing and store packaging cannot truthfully be claimed as executed inside that container.

Instead, GitHub-hosted CI provided the automated source verification described above and is green for the verified source head.

The following activities remain intentionally **not marked complete** because they require an appropriately provisioned development host, real/emulated devices, signing credentials or store access:

- `dotnet format --verify-no-changes` on a fully provisioned host;
- manual onboarding smoke testing;
- manual profile/medicine/schedule workflows on target devices;
- notification permission denied/granted manual testing;
- Android exact-alarm/battery behavior on representative devices;
- real time-zone-change delivery testing;
- manual document import/export/delete testing on target devices;
- manual calendar export verification;
- manual backup restore on a release build/clean installation;
- cold-start app-lock testing;
- screen-reader/large-text/keyboard/reduced-motion manual accessibility checks;
- current Apple/Google store-policy review for the external voluntary project-support link;
- package signing;
- store packaging/submission;
- final review/decision or verified dependency resolution for the open SQLitePCLRaw advisory.

`docs/releases/RELEASE_CHECKLIST.md` records automated evidence separately from those manual release activities so nothing is silently represented as verified when it is not.

---

## Funding-support and next-step continuation

### Buy Me a Coffee support integration

Requested voluntary support URL:

`https://buymeacoffee.com/sanskarIN`

Implemented runtime/repository work:

- `edeb445eaa9ff3a2bbc66cc771146efbd4e18bdb` — `feat: add CareNest funding URL constant`;
- `c9afd3646559823757897053d9ea745839bfc2a9` — `feat: add in-app project support command`;
- `9339e0382c9f85820ea3415746eb76b60f0a0dba` — `feat: expose voluntary project support link in About`;
- `ec7e86ff818d46df5c92a6497d81ad2dca5c41cf` — `chore: add Buy Me a Coffee funding link`;
- `7c38bc0b1dc7859a8890b94cf360a742d3a6488e` — `docs: add funding and next-step links to README`;
- `6cc6694b8fcff1b6abe87eb4b50e2520059f065a` — `docs: add voluntary project support information`;
- `eb51bc25a8c7540654b4ec6f3dae416cb1c9482f` — `test: cover Buy Me a Coffee support surface`.

The About view model now consumes the shared repository/creator/business/support/funding constants instead of duplicating those values directly in command construction.

The About page displays a `Support CareNest on Buy Me a Coffee` button and explicitly says project support is voluntary and does not unlock medical advice, premium health features or different reminder behavior.

GitHub funding metadata was added at:

`.github/FUNDING.yml`

with the same custom support URL.

### Funding privacy/security/legal boundary

Additional documentation commits:

- `7a44a050a5506410a21b25e4faf1333b5bc54fbf` — `docs: document voluntary support link privacy boundary`;
- `02ab94f13636339499c7e5fef80184ee7f090a6c` — `docs: clarify voluntary funding has no CareNest entitlement`;
- `45504aef88ce2a9b59ae38b9a33659c724283812` — `docs: document external support-link data boundary`;
- `d663921434f6892a19ac2a382f04a03346b393fa` — `security: model external project-support link boundary`;
- `e887a107dda24b9c1ffd5de1eb2119779d80eb1c` — `security: document external link trust boundary`;
- `c925f27ce2ad91f16521ccfd880d8d3c5f55cfcf` — `docs: add funding-link store listing guidance`.

Current rules are explicit:

- the support URL is a fixed HTTPS destination;
- it opens only after explicit user interaction;
- no CareNest health records/documents/backups/profile identifiers/reminder history/app-lock data are appended to the link;
- no funding-provider API key/payment SDK/payment credential is included in CareNest for this link;
- the external service becomes an independent privacy/security/payment boundary after it is opened;
- contributing does not buy medical advice, treatment guidance, emergency help, premium reminder behavior, data access or a different CareNest safety standard.

### Store-policy caution retained

A current Apple/Google store-policy determination was not claimed in this repository work because store rules can change and must be checked against the intended distribution channel at submission time.

The release checklist and next-step roadmap require:

- verifying current Apple App Store rules for the external voluntary support link;
- verifying current Google Play rules for the external voluntary support link;
- conditionally hiding/removing the in-app external link for a channel if that channel's current policy requires it;
- never relabeling the link as a medical purchase or health-feature entitlement to work around store rules.

### Detailed next-step roadmap added

New file:

`docs/releases/NEXT_STEPS.md`

Commit:

- `c814f1365608dacce95cee1c5966f68690198fde` — `docs: add concrete CareNest next-step roadmap`.

The roadmap does not pretend future work is already complete. It separates:

#### Priority 0 — production blockers

1. Resolve the open SQLitePCLRaw dependency advisory and remove the temporary audit exception only after a compatible verified dependency path exists.
2. Run manual real/emulated-device and accessibility smoke tests on Android, Windows, iOS/iPadOS and Mac Catalyst.
3. Verify current app-store policy for the external voluntary support link.
4. Prepare Android/Apple/Windows signing identities and keep signing secrets outside Git.
5. Finish store listings, screenshots, data-safety/privacy disclosures and medical-safety wording.

#### Priority 1 — release promotion

6. Create a final exact-commit verification branch after Priority 0 is complete.
7. Promote version/release metadata and create the final annotated tag only from verified source.
8. Build/archive signed Android, Apple and Windows release artifacts with provenance/checksums where appropriate.

#### Priority 2 — post-release quality

9. Establish an explicit user-submitted feedback/bug-report flow without hidden telemetry.
10. Expand notification, time-zone/DST, backup-compatibility, corruption/low-storage and accessibility test coverage.
11. Improve release engineering with protected artifact workflows, dependency review, SBOMs and attestations where supported.

#### Priority 3 — CareNest 1.x enhancements

12. Expand localization/resource coverage.
13. Improve reminder usability without inferring clinical intent.
14. Improve local document organization/search/duplicate handling.
15. Improve backup usability and future migration fixtures.

#### Priority 4 — separately reviewed future versions

Potential encrypted sync, remote caregiver collaboration and accounts remain deliberately deferred until new threat modeling, privacy design, authentication/key design, abuse analysis and explicit consent controls exist.

No future roadmap item changes the current rule that CareNest does not provide diagnosis, dosage calculation, treatment advice, medication-interaction claims or clinical risk scoring.

### Release/security documents linked to next steps

Relevant commits:

- `b3dfd68461a4084721328e399e277010589a8fa2` — `docs: add funding-link store-policy release gate`;
- `2b8f97525ea8d3b41bf62e20d76e1cc224dab102` — `docs: record voluntary project funding support`;
- `368c26408756f8facc52f30a7868478df237f0be` — `docs: connect SQLite risk to production next steps`;
- `841a5dff8d54881518a78bb913554cc41249febb` — `docs: record green funding-enabled release verification`;
- `11f05c230ef66c07caa33d20341d9485bd309d76` — `docs: finalize funding-enabled green verification status`.

---

## Funding-enabled verification pull request — PR #16

A fresh verification was required because the funding work changed runtime/UI source and added UI-contract tests after PR #15's previously verified runtime head.

Verification source head:

`2b8f97525ea8d3b41bf62e20d76e1cc224dab102`

Verification branch:

`ci/carenest-rc1-funding-verification`

Verification marker commit:

`547845c945da9af5ff5738ebddf12e2370a9b664`

Pull request:

`#16 — Verify CareNest funding support integration`

The marker file existed only to trigger pull-request workflows. PR #16 was closed after successful verification and was **not merged**, so the verification marker was not added to production `main`.

### CareNest CI run #87

- workflow: `CareNest CI`;
- run number: `87`;
- run id: `31301203981`;
- final status: `completed`;
- final conclusion: `success`.

Core test evidence:

- `CareNest.UnitTests`: 15 passed, 0 failed, 0 skipped;
- `CareNest.IntegrationTests`: 11 passed, 0 failed, 0 skipped;
- `CareNest.UiTests`: 10 passed, 0 failed, 0 skipped;
- total: 36 passed, 0 failed, 0 skipped.

The two additional UI-contract tests specifically verify consistency of the Buy Me a Coffee URL and the voluntary-support wording across runtime/repository support surfaces.

Platform build evidence:

- Android Release build: passed;
- Windows Release build: passed;
- iOS simulator Release build: passed;
- Mac Catalyst Release build: passed.

### CodeQL run #86

- workflow: `CodeQL`;
- run number: `86`;
- run id: `31301203985`;
- final status: `completed`;
- final conclusion: `success`.

---

## Current repository state after funding/next-step continuation

- Complete CareNest `1.0.0-rc.1` product source is on `main`.
- Buy Me a Coffee voluntary support URL is `https://buymeacoffee.com/sanskarIN`.
- The support URL is centralized in `AppConstants.FundingUrl`.
- The About page exposes the voluntary support action.
- `.github/FUNDING.yml` exposes the same support URL for GitHub repository funding UI.
- README, SUPPORT, PRIVACY, TERMS, SECURITY, threat model, data lifecycle, store guidance and changelog document the funding boundary.
- `docs/releases/NEXT_STEPS.md` contains the complete ordered next-step roadmap.
- `docs/releases/RELEASE_CHECKLIST.md` records the new verification evidence and remaining manual/store/signing/security gates.
- `PROJECT_STATUS.md` records the funding-enabled verified state.
- Source head `2b8f97525ea8d3b41bf62e20d76e1cc224dab102` has a fully green automated matrix.
- CareNest CI run #87 succeeded.
- CodeQL run #86 succeeded.
- 15 unit tests passed.
- 11 integration tests passed.
- 10 UI-contract tests passed.
- 36 total automated tests passed with 0 failed and 0 skipped.
- Android Release build passed.
- Windows Release build passed.
- iOS simulator Release build passed.
- Mac Catalyst Release build passed.
- Verification PR #16 was closed without merging its marker file.
- Later `main` commits after the verified source head are documentation/security/status guidance changes only; they do not alter product runtime behavior.
- The SQLitePCLRaw advisory remains explicitly open; it is not claimed fixed.
- Final `1.0.0` publication/tagging remains gated on the manual release checklist, current store-policy review for the funding link, signing/store preparation and the SQLite dependency-risk decision/resolution.
- Cloud synchronization, remote caregiver collaboration, accounts/mobile-number authentication, server-side storage, medical interpretation, diagnosis, treatment advice, medication-interaction claims and clinical risk scoring remain deferred to later separately reviewed versions.
