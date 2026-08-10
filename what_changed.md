# what_changed.md

## CareNest implementation record

This file is the detailed repository handoff requested in place of long chat messages. The uploaded **Master Build Prompt — CareNest** remains the source of truth for product scope, safety boundaries, architecture, testing, branding, repository standards, and the multi-phase continuation protocol.

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

# Safety boundary implemented

CareNest remains a local-first organizational application. It does not diagnose conditions, determine dosage, infer doses, recommend treatment, perform medication-interaction checking, create clinical risk scores, replace a doctor/pharmacist, or provide emergency services.

All reminder schedules originate from explicit user input. Medicine strength and instruction text are stored as opaque text exactly for organizational display/use and are not interpreted as dosage rules. `StockChangePerTakenEvent` is explicitly user-entered and is never inferred from medicine strength or instruction text.

Reminder delivery limitations are surfaced rather than hidden. Notification permissions, battery optimization, exact-alarm capability, operating-system restrictions, force-stop/shutdown behavior, daylight-saving changes, time-zone changes, reboot behavior, and platform scheduling policy can affect delivery.

CareNest tells users to follow qualified professional instructions and to contact local emergency services in an emergency rather than rely on CareNest.

The reminder hardening continuations do **not** add clinical interpretation. They protect deterministic handling of the schedule values that users enter. In particular, an invalid local clock time during a daylight-saving spring-forward gap is not silently moved to another time by the planner. CareNest does not invent a replacement reminder time.

The latest ownership/UTC hardening also does not infer medical intent: it validates that the local profile, medicine, schedule, and persisted schedule-time objects passed to the planner belong together, requires planner/rebuild timestamps to be actual UTC values, and requires snooze to be an explicit future UTC time before persistence/platform scheduling.

Buy Me a Coffee support is voluntary project support only. It does not unlock app functionality, medical advice, priority health support, different reminder behavior, emergency assistance, a CareNest account, or access to local CareNest data. The funding destination is an external trust boundary opened only after explicit user action.

---

# Product scope implemented

The release-candidate source on `main` includes:

- local-first onboarding and privacy disclosure;
- no required account, CareNest server, cloud backend, or network connection;
- multiple local family profiles;
- optional app lock and secure secret storage;
- optional profile photos stored through the encrypted document-storage path;
- emergency contacts and profile notes;
- medicine records with opaque user-entered strength/instruction text;
- active, paused, completed, and archived medicine states;
- daily, selected-weekday, specific-time, every-N-hours, cycle, custom-date-range, and as-needed schedule behavior;
- deterministic and idempotent reminder occurrence materialization;
- planner entity-ownership validation across local profile → medicine → schedule → persisted schedule-time relationships;
- defensive archived-profile suppression inside the planner;
- actual-UTC planner-window validation;
- half-open reminder-planning windows to avoid boundary duplication;
- duplicate user-entered clock-time deduplication by stable occurrence identity;
- deterministic daylight-saving overlap handling;
- representative multi-zone daylight-saving gap/overlap regression coverage;
- explicit no-invented-time behavior for invalid daylight-saving gap local times;
- deterministic fixed-seed property-style recurrence-boundary coverage;
- scheduled, snoozed, taken, skipped, delayed, and missed reminder states;
- future-UTC snooze validation before persistence/platform notification scheduling;
- follow-up reminders and quiet hours;
- medication log and edit history;
- appointment organization with notes, attachments, reminders, and explicit calendar export;
- encrypted local health-document vault;
- document folders, tags, import, camera/file paths, selected export, and deletion;
- user-entered stock/refill tracking and correction flow;
- local caregiver/family dashboard without background sharing;
- profile JSON export;
- PDF profile summary;
- CSV upcoming schedule, medication log, missed reminder, stock/refill, appointment-history, and document-list reports;
- manual password-encrypted backup/restore;
- schema-versioned restore validation and rollback behavior;
- portable recovery of encrypted document data through the encrypted backup payload;
- WAL-backed database snapshot creation with committed-content/integrity/cancellation regression coverage;
- system/light/dark theme handling;
- large-interface and reduced-motion preferences;
- notification diagnostics;
- redacted schedule inspector;
- time-zone simulation that does not rewrite stored schedules;
- sanitized diagnostic export that excludes health-document contents;
- database migration version display;
- storage usage and cache controls;
- About, license, privacy, terms, security, support, business contact, creator, and open-source surfaces;
- voluntary Buy Me a Coffee project-support action;
- GitHub funding metadata;
- custom CareNest BMC vector artwork plus original compact project-support badge;
- adaptive app icon, splash, standard mark, light mark, dark mark, and monochrome mark assets;
- unit, integration, UI-contract, repository-policy, architecture, ViewModel, data-model, branding/localization, async-safety, logging-privacy, app-lock-security, reminder-boundary, reminder-ownership, UTC/snooze, deterministic-property, DST-matrix, and snapshot-integrity tests;
- GitHub Actions cross-platform CI;
- platform-neutral formatting gate;
- CodeQL analysis;
- NuGet dependency-audit workflow;
- explicit production release gate;
- release-evidence/provenance workflow;
- Dependabot configuration;
- privacy-safe structured bug report form;
- architecture, security, privacy, testing, setup, troubleshooting, release, BMC, and store documentation;
- deterministic reminder scheduling contract documentation;
- Bash and PowerShell release-preflight scripts;
- manual cross-platform release test matrix;
- store-submission checklist;
- SQLite dependency migration/verification plan;
- release-quality gate, security review, release evidence, release notes template, and exact-head verification protocol.

---

# Repository structure

The requested multi-project solution separation is present:

```text
src/
  CareNest.App/                 # .NET MAUI UI, resources, composition and platform integrations
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
  verification/                # verification-marker files exist only on temporary branches

.github/
  ISSUE_TEMPLATE/
  workflows/
  FUNDING.yml

BUY_ME_A_COFFEE.md
```

Required root/repository files include `README.md`, `LICENSE`, `NOTICE`, `CONTRIBUTING.md`, `CODE_OF_CONDUCT.md`, `SECURITY.md`, `SUPPORT.md`, `PRIVACY.md`, `TERMS.md`, `CHANGELOG.md`, `.gitignore`, `.editorconfig`, `Directory.Build.props`, `Directory.Packages.props`, `PROJECT_STATUS.md`, `DECISIONS.md`, issue templates, pull-request template, CI, CodeQL, Dependency Audit, Release Gate, Release Evidence, Dependabot, and funding metadata.

---

# Delivery phases completed

## Phase 0 — repository, architecture, privacy and design foundation

Completed:

- multi-project solution and dependency boundaries;
- repository standards, analyzers, central packages, editor configuration and ignores;
- Apache-2.0 license and notices;
- contribution, conduct, security, support, privacy, and terms documentation;
- architecture decision records;
- database schema documentation;
- threat model and data-lifecycle documentation;
- design system and localization readiness;
- store asset guidance;
- original CareNest branding vector assets;
- issue templates, PR template, Dependabot, CI, and CodeQL.

Original phase label:

`chore: establish CareNest architecture and repository standards`

## Phase 1 — domain, persistence, encryption and application services

Completed:

- requested domain entities and enums;
- validation preserving strength/instruction text as opaque user-entered text;
- SQLite persistence and migrations through schema version 5;
- repository, settings, and audit-entry infrastructure;
- deterministic reminder-occurrence keys;
- idempotent reminder materialization;
- explicit time-zone-aware reminder planning;
- AES-256-GCM encrypted document vault;
- password-encrypted schema-versioned backup format with authenticated encryption;
- restore validation and rollback flow;
- portable encrypted-document key recovery inside the password-protected backup payload;
- JSON/PDF/CSV export/report services;
- profile, medicine, appointment, document, reminder, and backup-reminder services.

Original phase label:

`feat: implement local-first domain persistence and safety services`

## Phase 2 — complete MAUI user workflows

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
- voluntary Buy Me a Coffee project-support action.

Original phase label:

`feat: add CareNest MAUI workflows and accessible navigation`

## Phase 3 — platform reminder integrations and reliability

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
- notification permission requested when the user first explicitly creates/saves a reminder-capable feature rather than during onboarding;
- stored schedule times are not silently rewritten after time-zone changes.

Original phase label:

`feat: add reminders encryption backup reports and platform integrations`

## Phase 4 — tests and release engineering

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

## Phase 5 — funding/support presentation and release-preparation hardening

Completed:

- centralized `AppConstants.FundingUrl`;
- in-app Buy Me a Coffee command;
- About-page support action;
- `.github/FUNDING.yml`;
- custom CareNest project-support vector artwork;
- highlighted README and SUPPORT funding surfaces;
- root and documentation clickable BMC pages;
- voluntary-support privacy/security/legal boundaries;
- store-policy review gate for the external link;
- Bash and PowerShell release-preflight scripts;
- manual device test matrix;
- store-submission checklist;
- SQLite dependency migration plan;
- next-step roadmap;
- funding-support UI-contract coverage;
- repeated exact-head cross-platform verification.

## Phase 6 — privacy logging, repository policy and production evidence hardening

Completed in the earlier 2026-08-10 continuation:

- platform-neutral formatting verification in CI;
- repository safety/completeness policy tests;
- architecture-boundary tests;
- ViewModel boundary tests;
- required data-model contract tests;
- branding/localization contract tests;
- async non-blocking source tests;
- logging-privacy policy tests;
- global exception observer;
- full-exception redaction from UI/startup/reminder logging;
- explicit logger-level guards to satisfy eager-argument analyzers;
- logging privacy documentation;
- light, dark, and monochrome CareNest mark assets;
- Release Evidence workflow;
- release evidence documentation;
- production quality gate;
- security release-review checklist;
- release-notes template;
- exact-head verification-branch protocol;
- restoration of missing valid files discovered during repository-history audit;
- four exact-head verification passes, with every discovered failure corrected rather than suppressed;
- fully green automated verification at PR #27.

## Phase 7 — reminder-boundary, WAL-snapshot and app-lock memory hardening

Completed:

- expanded medicine schedule validation boundaries;
- cycle-schedule planner behavior coverage;
- custom date-range and medicine end-date planner coverage;
- paused/completed/archived medicine suppression coverage;
- invalid daylight-saving spring-forward local-time coverage;
- deterministic daylight-saving fall-back overlap coverage retained;
- half-open planner-window coverage;
- duplicate explicit clock-time deduplication coverage;
- chronological occurrence ordering coverage;
- committed WAL-snapshot content verification;
- snapshot SQLite integrity verification;
- pre-cancelled snapshot no-output-file regression coverage;
- app-lock security source contracts;
- runtime app-lock verifier-memory clearing;
- app-lock residual-risk/security documentation;
- deterministic reminder scheduling contract documentation;
- test-plan expansion;
- exact-head PR #28 verification with 101/101 core tests green and all four platform builds green.

## Phase 8 — reminder ownership, UTC, snooze, property and multi-zone DST hardening

Completed in the current continuation:

- reminder planner null/ownership validation for supplied profile, medicine, schedule, and times;
- persisted schedule-time ownership validation while still allowing intentionally unbound editor times before persistence;
- defensive archived-profile suppression inside the planner;
- schedule validation rejects unrecognized `ScheduleKind` values;
- selected-weekday validation rejects unsupported bits outside the seven weekday positions;
- time-zone identifiers are trimmed and validated;
- planner `fromUtc`/`toUtc` require `DateTimeKind.Utc`;
- coordinator rebuild overrides require UTC;
- snooze requires an explicit future UTC timestamp before persistence/platform scheduling;
- deterministic fixed-seed recurrence property tests added;
- property tests cover arbitrary half-open windows, occurrence uniqueness/order, cycle on/off matrices, all supported weekday masks, and representative every-N-hours intervals;
- representative DST invalid/ambiguous coverage expanded to `America/New_York`, `Europe/Berlin`, and `Australia/Sydney` when those zones are available on the host;
- reminder scheduling contract, test plan, ADR decisions, quality gate, security review, roadmap, README, changelog, project status, release checklist, and this handoff aligned to the new behavior;
- first exact-head verification PR #29 correctly exposed CA2263 in the newly added non-generic enum-validation call;
- the analyzer finding was fixed in source rather than suppressed;
- fresh marker-only PR #30 verified the corrected exact head with 141/141 core tests and all platform/security/dependency gates green.

---

# Initial GitHub delivery

The release-candidate source was originally assembled on branch:

`release/carenest-1.0.0-rc.1`

and merged through pull request #3:

`CareNest 1.0.0-rc.1 complete implementation`

Merge commit:

`1244ed7fead73821f768f5119230dd6b8c24113f`

The implementation was deliberately delivered as a coherent source tree rather than running release CI against half-created phase snapshots.

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

# Earlier post-merge verification and hardening history

## CA1848 logging analyzer

GitHub Actions initially promoted CA1848 logging-performance guidance to an error in reminder coordination.

Commit:

- `7fed6d76ae2407d17bf3b19e8e4b112b3f39e279` — `ci: keep CA1848 logging optimization non-blocking`.

The rule remains visible as advisory guidance instead of hiding correctness failures.

## SQLitePCLRaw advisory

NuGet audit reported:

`GHSA-2m69-gcr7-jv3q`

against SQLitePCLRaw native package `2.1.11` resolved through the current `sqlite-net-pcl` dependency chain.

An attempted `SQLitePCLRaw.bundle_green` `2.1.12` move failed because that bundle version was not available from NuGet.org. The repository was therefore corrected back to a real restorable dependency graph instead of retaining an impossible pin.

Historical commits include:

- `7489b70f0cf37be7545e1ecb338fec6a7ccf90dd` — initial attempted security update, later corrected;
- `build: restore available SQLitePCLRaw package versions`;
- `security: document temporary SQLite audit suppression in build`;
- `security: add dependency risk register for SQLite advisory`;
- `docs: link dependency risk register from security policy`.

Current accurate state remains:

- native SQLitePCLRaw `2.1.11` is still in the dependency path;
- the exact advisory URL only is temporarily listed through `NuGetAuditSuppress`;
- there is no wildcard or severity-wide suppression;
- `docs/security/DEPENDENCY_RISK_REGISTER.md` marks the risk **open**;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` defines the upgrade/provider migration validation gate;
- the suppression is not described as a vulnerability fix;
- final production release requires an explicit dependency-risk resolution/decision.

Local-first architecture, no remote DB listener, no arbitrary user SQL execution, and parameterized repository operations reduce exposure but do not erase the advisory.

## Test naming analyzer

CA1707 was being promoted to an error for descriptive underscore-separated xUnit method names. The exception was scoped to test projects only.

## Advisory infrastructure analyzers

Several performance/culture analyzer recommendations were previously promoted to errors before CI reached functional failures. Advisory-only rules were reclassified without globally disabling correctness/security analyzers. Android platform-availability issues were fixed in code rather than hidden behind a CA1416 blanket suppression.

## MAUI target-framework propagation

The original CI command supplied `TargetFrameworks=...` globally, causing MSBuild to leak the application target framework into referenced `net10.0` projects.

The app now accepts `CareNestTargetFramework`, allowing CI/developer hosts to narrow only the MAUI app target before restore/build.

## SQLite result-producing PRAGMA failures

GitHub-hosted integration tests exposed that result-producing PRAGMAs cannot be treated as non-query `ExecuteAsync` operations with sqlite-net.

Corrected behavior:

- `PRAGMA journal_mode = WAL` is read and validated as a scalar;
- `PRAGMA busy_timeout = 5000` is read and validated as a scalar;
- `PRAGMA wal_checkpoint(FULL)` is consumed as a result before backup snapshot creation;
- regression tests verify WAL mode, busy timeout, and WAL-backed snapshot creation.

## MAUI package/source isolation

GitHub platform builds exposed the need for:

- explicit MAUI Controls package reference;
- platform-source isolation for inactive `Platforms/*` trees;
- explicit MAUI/DI namespaces;
- corrected Android time-zone intent constant.

## Apple runner/Xcode compatibility

The .NET 10 Apple workload required a newer Xcode toolchain than the older macOS runner selected. Apple CI was moved to `macos-26`, after which iOS simulator and Mac Catalyst Release compilation succeeded.

## Shared MAUI source compile fixes

Earlier hardening also corrected:

- startup destination switch typing;
- nullable schedule-editor values/collections;
- developer diagnostics to use the actual `ScheduledUtc` reminder contract;
- indexed window access without unnecessary LINQ;
- Android notification binding nullability/API guards.

---

# Previous green funding-enabled baseline

Before the 2026-08-10 hardening continuation, the fully green funding-enabled baseline was source head:

`52abe54cfc771c411b78332d78217a5876ebc4c8`

Verification PR #21 recorded:

- CareNest CI #115 / `31302769113`: success;
- Unit tests: 15 passed;
- Integration tests: 11 passed;
- UI-contract tests: 10 passed;
- Android Release: passed;
- Windows Release: passed;
- iOS simulator Release: passed;
- Mac Catalyst Release: passed;
- CodeQL #114 / `31302769108`: success;
- Dependency Audit #4 / `31302769112`: success.

This baseline later became important during the repository-history recovery audit.

---

# 2026-08-10 privacy/policy continuation — complete hardening record

## Platform-neutral formatting gate

Commit:

- `a63bd413ed0e1760e3391552962ca19128be93eb` — `ci: verify formatting for platform-neutral projects`.

The core CI job verifies formatting separately for Shared, Domain, Application, Infrastructure, UnitTests, IntegrationTests, and UiTests. The MAUI app remains in platform-specific jobs because the core runner does not install every MAUI target workload.

## Repository safety/completeness policy tests

Initial commit:

- `3e3e70779b4156ffcf5daa34c79f113511db84fc` — `test: enforce repository safety and completeness policies`.

The tests protect against runtime TODO/FIXME/NotImplemented placeholders, runtime network/telemetry client introduction, named diagnosis/dosage/treatment/interaction/risk-scoring feature regressions, common secret/signing files, and accidental deletion of required governance/security/release files.

The first source scan was too broad and included generated `obj` files. CI exposed that false positive. The test was fixed to inspect committed source and ignore `bin`, `obj`, and `.git` segments.

Correction:

- `ed84c0998c38b87f03c5474a3522b4357d45c073` — `test: scope repository policy scans to committed source`.

## Additional CareNest branding variants

Added original vector variants:

- `8404ba13bb94ea06fcc0c41bf1b5e787e555a667` — monochrome CareNest system icon;
- `c998f0108770d4399e734fe0ba7efd3c852b4b17` — light CareNest mark variant;
- `ed6ddbd242ba6be27daeb41710234858cb78e623` — dark CareNest mark variant.

The original app icon, foreground adaptive icon, splash, standard CareNest mark, compact support badge, and custom BMC artwork remain present.

## Branding/localization contracts

Initial commit:

- `7cc58c93ca0e65c71e59d049c5b22826c18070b3` — branding/localization contract tests.

Coverage validates MAUI adaptive icon declaration, foreground icon, splash, MAUI image resources, SVG well-formedness, English safety/branding resource keys, BMC destination/artwork consistency, clickable About support artwork, and voluntary-support copy.

Expanded after restoration:

- `15f3b94f9a5a02ffc5090290b7de06a5cd006996` — `test: expand restored support branding contracts`.

## ViewModel boundary contracts

Initial commit:

- `b47ba485c2b8edb77d19c01140b7adaac02c152d`.

The first version incorrectly treated internal ICommand adapters as ordinary ViewModel bodies. The rule was narrowed to concrete ViewModels:

- `303d89606f746c9480bbcb19cb018c7e1a7d98e0` — `test: scope async-void rule to concrete ViewModels`.

Current contracts assert concrete ViewModels do not use `async void`, do not use `Task.Run` to hide blocking work, do not reach directly into SQLite persistence, do not create network clients, use centralized public constants in About, do not request notification permission during onboarding, and preserve explicit as-needed/no-reminder behavior.

## Architecture-boundary contracts

Initial commit:

- `f5a8827d86023d50a57254d97dec36895b25eb64`.

Contracts enforce Shared/Domain/Application/Infrastructure dependency direction, no MAUI in platform-neutral layers, and the MAUI app as composition root.

CI exposed Windows-style project-reference separators and a nullable filename return. Corrections:

- `182533922f491f5c122e9cacd6f0c8c12d43c493` — cross-platform separator normalization;
- `8417513db36c72b0ec2cfaccadb6ac47ba361f11` — explicit non-null project-reference filename contract.

That source became the PR #27 exact-head baseline.

## Required data-model contracts

Commit:

- `d1a0c5682b8e1f0919e95459488d367d88746806`.

Coverage requires the prompt-defined domain entities and protects medicine strength/instructions as strings/opaque text plus explicit user-stored stock change values.

## Privacy-aware global exception observer

Commits:

- `aaad2e1e3abc8ff99d63e177c377d62027402fab` — add `GlobalExceptionHandler`;
- `37f93bcf273655af8c6cf1b900fe5ccd792d1795` — dependency-injection registration;
- `01fb02620472b882f3d620b291b6adb6bbf2deaf` — attach during app construction;
- `915f4a45dc687eed1be7506292568782d2ceff3f` — global exception privacy contracts.

Behavior: attach once with `Interlocked.Exchange`, observe app-domain unhandled and task-scheduler unobserved exceptions, mark unobserved task exceptions observed after safe logging, and avoid messages/stack/full exception objects.

## UI error logging privacy fix

Existing `SafeUiErrorService` passed a full exception object to the structured logger.

Commits:

- `bba6615a224b87aec31f5c033fe2b214595c193f` — redact exception details;
- `a95936d0bcd665531b6a78326bcd8726271a471f` — regression coverage.

## Async non-blocking policy contracts

Commit:

- `907e5a71796a15b78dec75e26b6af00ba692257c`.

Runtime source policy rejects common `.GetAwaiter().GetResult()`, `.Wait()`, `Thread.Sleep`, `Task.WaitAll`, `Task.WaitAny`, and common `.Result` blocking patterns.

## Release Evidence workflow

The repository includes `.github/workflows/release-evidence.yml` plus `docs/releases/RELEASE_EVIDENCE.md`, `QUALITY_GATE.md`, `SECURITY_RELEASE_REVIEW.md`, `RELEASE_NOTES_TEMPLATE.md`, and `VERIFICATION_BRANCH_PROTOCOL.md`.

The workflow records exact source/ref/toolchain identity, TRX test evidence, transitive dependency inventories, SHA-256 checksums, and an Actions artifact for the eventual final promoted commit. It has not been falsely represented as final-public-release approval.

## Reminder logging privacy hardening

Commits:

- `3e05f6ccd5965c29eaaa11b9cff5ba018a585a2a` — redact reminder exception details/record IDs;
- `f07a0dede776bbfca16163a26b1a99a35ee7694b` — reminder logging privacy tests.

## Logging privacy source policy

Commit:

- `28b5e220ac661123abceb576c28218b36846bb12`.

Generated-source scanning was corrected by:

- `853d3d8254fd0b30a386a42d2d1fde316bc46a43` — committed-source-only logging privacy scan.

## Logger eager-argument analyzer fixes

GitHub MAUI compilation surfaced CA1873. Explicit log-level guards were added instead of suppressing the quality rule:

- `8209ed49eeaee5bd2341e4f5a108f126f7c73d06` — GlobalExceptionHandler guards;
- `ebb5e1b66e574552dddab3fe3252cd230fc175f8` — SafeUiErrorService guard;
- `850355e618d206ef1276ede5b28c5c925f47a9d1` — reminder logging guards.

## Existing StartupCoordinator privacy issue found by new policy

Fix:

- `78657718aab236456bb95a33e5f57c00649f9c73` — `security: redact startup recovery exception logging`.

## Repository-history recovery audit

A compare against previously green source head `52abe54...` found an earlier repository ref recovery had not replayed nine valid files/changes. All nine were restored:

1. privacy-safe structured bug report;
2. Dependency Audit workflow;
3. production Release Gate workflow;
4. highlighted BMC README presentation;
5. highlighted SUPPORT presentation;
6. `docs/releases/BMC_HIGHLIGHT.md`;
7. `docs/releases/BMC_HIGHLIGHT_RELEASE_CHECK.md`;
8. compact `carenest_support.svg`;
9. highlighted clickable in-app About support card.

Restoration commits:

- `1fd80561509524dd4dd6d25bc6a3658f3c681cd2`;
- `2d55eadc86aee43a3c930044a4bb8d98e38e941b`;
- `d39f74779d89747293d4a829d1c38299af865b8c`;
- `bff9138503d1cdfd950dcf8beb98726cef35dc2d`;
- `6f699f1708a8a49cfc118a88b29fbf357b0b067b`;
- `ad0d92a7c62341503258cabc587ab96cb2a112d1`;
- `c18a83f1b44cb3ef43b4559bd7abd09eb3f1415a`;
- `d5a4ae9cd01fc8d2ba0a6ee59f088058f6d6920a`;
- `df10abc5758490850d282ab3e085db90bcab0e26`.

## BMC/support current state

Current support URL:

`https://buymeacoffee.com/sanskarIN`

Support surfaces include centralized `AppConstants.FundingUrl`, clickable compact badge in README, highlighted README/SUPPORT links, clickable About `ImageButton`, dedicated About button, visible URL, `.github/FUNDING.yml`, `BUY_ME_A_COFFEE.md`, `docs/SUPPORT_CARENEST.md`, and BMC release guidance.

The original CareNest support badge is not represented as an official Buy Me a Coffee trademark/logo asset. CareNest does not append health data to the support URL.

---

# Exact-head verification protocol

Protocol used throughout hardening:

1. finish intended source/test changes on `main`;
2. create a branch from the exact source SHA;
3. add one marker file under `build/verification/`;
4. open a PR to `main`;
5. verify the PR diff is marker-only;
6. run CI/CodeQL/Dependency Audit;
7. if a real failure appears, fix `main`, close the stale marker PR without merge, and recreate exact-head verification;
8. once all gates pass, record evidence;
9. close the marker PR without merge.

This prevents verification markers from entering production source and prevents stale CI evidence from being attributed to a newer source head.

---

# Privacy/policy verification sequence

## PR #24

Source head: `47234b65c2060e0417a7e7cd6b005d286594df3a`  
Marker head: `45260933d286a60b6d0de66d9f0fddc225bbdf48`

- CodeQL #175: success;
- CareNest CI #175: failure.

Findings: CA1873 eager exception metadata and CA1861 test allocation guidance. Source/tests were corrected. PR #24 closed without merge.

## PR #25

Source head: `15f3b94f9a5a02ffc5090290b7de06a5cd006996`  
Marker head: `450e83d38d9febfbd1d9988b33ed84467dc71737`

- Dependency Audit #5 / `31374433350`: success;
- CodeQL #190 / `31374433235`: success;
- formatting: success;
- 15 unit tests passed;
- 11 integration tests passed;
- UI-contract run: 41 passed, 5 failed;
- CareNest CI #190 / `31374433469`: failure.

Findings: Linux project-reference separator handling, StartupCoordinator full-exception logging, generated `obj` source scan false positive, and required logger guards. All fixed. PR #25 closed without merge.

## PR #26

Source head: `853d3d8254fd0b30a386a42d2d1fde316bc46a43`  
Marker head: `d7c8c19b014f3cfece50a88f6b1c616c6a9fe354`

- Dependency Audit #6 / `31374928518`: success;
- CodeQL #198 / `31374928520`: success;
- formatting: success;
- 15 unit tests passed;
- 11 integration tests passed;
- UI project compile found one nullable contract error;
- CareNest CI #198 / `31374928536`: failure.

Fix: `8417513db36c72b0ec2cfaccadb6ac47ba361f11` explicit non-null project-reference filename contract. PR #26 closed without merge.

## PR #27

Exact source head: `8417513db36c72b0ec2cfaccadb6ac47ba361f11`  
Marker head: `aefd53869b7eaf54815de446fc83373c7977d04d`  
Marker file: `build/verification/rc1-hardening-20260810-4.txt`

- CareNest CI #200 / `31375336226`: success;
- formatting: success;
- UnitTests: 15 passed;
- IntegrationTests: 11 passed;
- UiTests: 46 passed;
- total: 72 passed;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #200 / `31375336083`: success;
- Dependency Audit #7 / `31375336088`: success.

PR #27 closed without merge and became the superseded pre-Phase-7 automated source baseline.

---

# Reminder/snapshot/app-lock hardening continuation — detailed commit record

The continuation requested full work, maximum logical commits, and an updated handoff. The work was split into separate commits so each behavior/security change remains independently reviewable.

## 1. Medicine schedule validation boundaries

Commit:

`951caa1aefad6374b1ae2a440a9886a26e48a920` — `test: expand medicine schedule validation boundaries`

Updated:

`tests/CareNest.UnitTests/MedicineRulesTests.cs`

New/expanded coverage:

- every-N-hours requires an explicit interval;
- every-N-hours requires exactly one explicit starting clock time;
- as-needed remains valid without automatic times;
- selected-weekday schedules require at least one selected day;
- cycle schedules require explicit positive on-days and off-days;
- end date before start date is rejected;
- hour values below 0 or above 23 are rejected;
- minute values below 0 or above 59 are rejected;
- unknown time-zone identifiers are rejected;
- medicine strength/instruction text remains unchanged/opaque;
- negative user stock change remains rejected.

No clinical frequency recommendation is introduced. The tests only validate user-entered configuration shape.

## 2. Reminder recurrence/date/state/DST hardening

Commit:

`7893812a61a380fe95e176e0519cb9ed27e00411` — `test: harden reminder recurrence and boundary coverage`

Updated:

`tests/CareNest.UnitTests/ReminderPlannerTests.cs`

New/expanded coverage includes:

- explicit 2-days-on/1-day-off cycle schedule behavior;
- custom date-range schedule stop boundary;
- medicine end-date stop boundary;
- paused medicine creates no automatic occurrences;
- completed medicine creates no automatic occurrences;
- archived medicine creates no automatic occurrences;
- invalid spring-forward local time produces no invented replacement reminder;
- ambiguous fall-back local time remains deterministic;
- existing daily/multi-time/as-needed/selected-weekday/every-N-hours/follow-up/disabled/stable-key behavior retained.

The spring-forward test uses a representative `America/New_York` transition when available on the runner. If a local 02:30 does not exist, the planner produces no occurrence for that invalid local time instead of moving it to another time.

## 3. Initial app-lock cryptographic source contracts

Commit:

`6c962f39ac2873f5b17dbcb653b260ce850839e3` — `test: enforce CareNest app-lock cryptographic contract`

Created:

`tests/CareNest.UiTests/AppLockSecurityContractTests.cs`

Contracts protect:

- cryptographic random salt generation;
- PBKDF2 key derivation;
- configured 210,000 iterations;
- SHA-256 hash algorithm;
- fixed-time verifier comparison;
- no direct plaintext PIN persistence through the secret store;
- removal of enabled/salt/verifier values when app lock is disabled;
- numeric six-to-thirty-two-digit PIN policy.

These source contracts supplement, rather than replace, target-device secure-storage/app-lock testing.

## 4. WAL snapshot committed-content verification

Commit:

`4f839b377018fc33f6699e3b3e5a3e9b9621ce53` — `test: verify WAL snapshot preserves committed profile data`

Updated:

`tests/CareNest.IntegrationTests/DatabaseMigrationTests.cs`

The test now:

1. creates a local test store;
2. commits a profile record;
3. creates a WAL-backed snapshot through production snapshot code;
4. opens the copied SQLite file read-only;
5. queries for the exact committed profile ID/name;
6. runs `PRAGMA integrity_check` on the copied database;
7. requires the record count to be one and integrity result to be `ok`;
8. closes and deletes the temporary snapshot.

This is stronger than asserting only that a copied file exists and has nonzero length.

## 5. Snapshot pre-cancellation behavior

Commit:

`a176fe68cba190a467203add09229855cf4392d1` — `test: verify snapshot cancellation leaves no output file`

Added integration coverage that passes an already-cancelled token to `CreateSnapshotAsync`, requires `OperationCanceledException`, and verifies no output file is left behind.

## 6. App-lock verifier memory hardening

Commit:

`c0ad7a7022fce7d8312af4dbc40fa6a384a10f60` — `security: clear app-lock verifier after PIN checks`

Updated runtime file:

`src/CareNest.App/Services/AppLockService.cs`

Previous behavior already cleared the freshly derived `actual` verifier. The verifier bytes read from secure storage as `expected` remained in managed memory after comparison.

Current behavior:

- if secure-storage salt is absent but a verifier was retrieved, the retrieved verifier is zeroed before returning false;
- after a normal comparison, both `actual` and `expected` byte arrays are cleared in a `finally` block;
- fixed-time comparison remains in place;
- random salt/PBKDF2/SHA-256 behavior remains unchanged;
- PIN policy remains unchanged;
- no plaintext PIN persistence is added.

This improves managed-memory hygiene but does not claim protection against a compromised operating system, memory dump with arbitrary timing, rooted/jailbroken device, or weak user PIN.

## 7. Verifier-zeroing contract

Commit:

`a1df017060c8ad60ab3ce968a6737039c3ff12ee` — `test: require verifier zeroing after app-lock checks`

App-lock source contracts now explicitly require both:

- `CryptographicOperations.ZeroMemory(actual)`;
- `CryptographicOperations.ZeroMemory(expected)`.

## 8. Planner window/dedup/order invariants

Commit:

`f4e287efa5774b92b2ea8a699a4fbc407da7848e` — `test: cover reminder window and deduplication invariants`

Created:

`tests/CareNest.UnitTests/ReminderPlannerBoundaryTests.cs`

Coverage protects:

- planning window is half-open: `fromUtc` included, `toUtc` excluded;
- duplicate explicit user clock times collapse to one occurrence/key;
- returned occurrences are chronological even if clock-time inputs are out of order.

The half-open rule is particularly important for adjacent rebuild windows because an occurrence exactly on the boundary belongs to only one window.

## 9. App-lock threat-model clarification

Commit:

`7d05765226f684cba34c920ae6b8559fd665ca4a` — `docs: clarify app-lock residual risk and verifier handling`

Updated `docs/security/THREAT_MODEL.md` to add:

- explicit app-lock PIN guessing threat;
- random salt/PBKDF2/fixed-time/verifier-zeroing controls;
- residual weak-PIN/offline-guessing risk on compromised devices/secret stores;
- explicit statement that app lock is a local privacy barrier, not whole-database/device encryption;
- security-review triggers for biometric bypass/recovery and remote PIN recovery.

## 10. SECURITY.md app-lock limitation

Commit:

`640cb6ccac0dae2638d40dabf196fcbb9db721fd` — `docs: document CareNest app-lock security limits`

Aligned root security policy with the threat model and verifier handling.

## 11. Deterministic reminder scheduling contract

Commit:

`692e8b37fb58d5fda966e06a7f8170ba033fa288` — `docs: define deterministic CareNest reminder scheduling contract`

Created:

`docs/testing/REMINDER_SCHEDULING_CONTRACT.md`

The contract documents explicit-user-input-only scheduling, no dose/frequency inference, no automatic as-needed occurrences, half-open planning windows, stable occurrence identity, duplicate-time deduplication, chronological output ordering, daily/custom date boundaries, selected weekdays, cycle on/off behavior, every-N-hours explicit inputs, follow-up separation, disabled/inactive medicine suppression, DST gap no-invented-time behavior, DST overlap determinism, and delivery limitations.

## 12. Test-plan expansion and PR #28 source freeze

Commit:

`69c4dd9319f7dc47edea1786e683f7d90c656e1e` — `docs: expand automated schedule and snapshot test plan`

Updated `docs/testing/TEST_PLAN.md` to record the new schedule validation/planner, snapshot, and app-lock coverage and link to the deterministic scheduling contract.

This commit became the exact source/test/documentation head used as the PR #28 verification base.

---

# PR #28 — exact-head reminder/snapshot/app-lock verification

Verification branch:

`ci/carenest-rc1-reminder-applock-hardening-20260810`

Exact verified source head:

`69c4dd9319f7dc47edea1786e683f7d90c656e1e`

Verification marker head:

`a1362b551749762ae816e8b4366c8f1eb97538fa`

Marker-only file:

`build/verification/rc1-reminder-applock-hardening-20260810.txt`

Pull request:

`#28 — Verify CareNest reminder, snapshot, and app-lock hardening`

PR URL:

`https://github.com/sanskarIN/CareNest/pull/28`

The PR changed only the verification marker beyond the exact source head. After all workflows completed successfully, PR #28 was closed **without merge**.

## CareNest CI #220

Run ID: `31378000135`  
Final conclusion: **success**

Core job evidence:

- platform-neutral formatting: **success**;
- `CareNest.UnitTests`: **37 passed, 0 failed, 0 skipped**;
- `CareNest.IntegrationTests`: **13 passed, 0 failed, 0 skipped**;
- `CareNest.UiTests`: **51 passed, 0 failed, 0 skipped**;
- total automated core test cases: **101 passed, 0 failed, 0 skipped**.

Platform evidence:

- Android Release build: **success**;
- Windows Release build: **success**;
- iOS simulator Release build: **success**;
- Mac Catalyst Release build: **success**.

CodeQL #220 / `31378000143`: **success**.  
Dependency Audit #8 / `31378000134`: **success**.

The dependency audit remains compatible with the repository's explicit narrow SQLite advisory suppression. A green Dependency Audit does not mean `GHSA-2m69-gcr7-jv3q` is fixed; the dependency risk register remains authoritative and open.

PR #28 became the superseded pre-Phase-8 source baseline.

---

# Phase 8 continuation — complete detailed commit record

The current continuation was intentionally split into separate logical commits to keep behavior, validation, tests, and documentation independently reviewable.

## 1. Planner entity-ownership enforcement

Commit:

`0f22de1240d28c8011c3c4f41d0a084211706a8d` — `fix: enforce reminder planner entity ownership`

Updated:

`src/CareNest.Application/Services/ReminderPlanner.cs`

Behavior added:

- explicit null checks for supplied medicine, schedule, times, and profile;
- schedule `MedicineId` must match the supplied medicine `Id`;
- medicine `ProfileId` must match the supplied profile `Id`;
- a persisted `ScheduleTime` with a nonblank `MedicineScheduleId` must match the supplied schedule `Id`;
- unbound editor `ScheduleTime` values remain allowed before persistence;
- ownership mismatch throws instead of silently materializing an occurrence under another local entity.

This is a data-integrity boundary. It does not introduce networking, medical interpretation, or cross-profile sharing.

## 2. Schedule enum, weekday-mask, and time-zone validation

Commit:

`098644c73d7025142ef29f213933f01e8ba52959` — `fix: validate schedule enum and weekday mask`

Updated:

`src/CareNest.Domain/Rules/MedicineRules.cs`

Behavior added:

- unrecognized `ScheduleKind` values are rejected;
- selected-weekday schedules reject bits outside the seven supported weekday positions;
- blank time-zone identifiers are rejected;
- valid time-zone identifiers are trimmed before `TimeZoneInfo` lookup;
- existing interval/cycle/date/time/follow-up validation remains intact.

The first implementation used the non-generic `Enum.IsDefined(Type, object)` overload. Later exact-head CI correctly exposed CA2263; that analyzer finding is recorded below and was fixed without suppression.

## 3. Planner ownership regression tests

Commit:

`e565c90cfc89fedb72127be520f71110e1307c11` — `test: cover reminder planner ownership boundaries`

Created:

`tests/CareNest.UnitTests/ReminderPlannerOwnershipTests.cs`

Coverage:

- reject a schedule belonging to another medicine;
- reject a medicine belonging to another profile;
- reject a persisted time belonging to another schedule;
- accept a persisted time belonging to the supplied schedule;
- accept an intentionally unbound editor time;
- verify produced occurrence IDs remain tied to the supplied valid graph.

## 4. Schedule-validation hardening tests

Commit:

`0c838901d3d90e37c23d29f77de49864e9cac080` — `test: cover schedule validation hardening`

Created:

`tests/CareNest.UnitTests/ScheduleValidationHardeningTests.cs`

Coverage:

- unknown schedule enum rejected;
- unsupported weekday-mask bit 7 rejected;
- mixed valid/unsupported masks rejected;
- negative mask rejected;
- all seven supported weekday bits accepted;
- blank and whitespace-only time zones rejected;
- valid time-zone identifiers surrounded by whitespace are trimmed and accepted.

## 5. UTC planner-window enforcement

Commit:

`d505b76d178f71cd01ecb6bd7d3daac2f01e76ef` — `fix: require UTC reminder planning windows`

Updated:

`src/CareNest.Application/Services/ReminderPlanner.cs`

Behavior:

- `fromUtc.Kind` must equal `DateTimeKind.Utc`;
- `toUtc.Kind` must equal `DateTimeKind.Utc`;
- local/unspecified values are rejected instead of silently reinterpreting their clock ticks as UTC;
- after validation, `TimeZoneInfo.ConvertTimeFromUtc` uses the actual validated UTC values directly;
- half-open window semantics remain unchanged.

## 6. UTC planner-window tests

Commit:

`d222f9855b7ceb65cf1209886e51385ad22af064` — `test: cover UTC reminder planning window contract`

Created:

`tests/CareNest.UnitTests/ReminderPlannerUtcWindowTests.cs`

Coverage:

- local-kind start rejected;
- unspecified-kind start rejected;
- local-kind end rejected;
- unspecified-kind end rejected;
- valid UTC window still includes `fromUtc` and excludes `toUtc`.

## 7. Deterministic recurrence property coverage

Commit:

`c4be9647b1393d3d978a8bc10643c3cfb3ccf91d` — `test: add deterministic recurrence property coverage`

Created:

`tests/CareNest.UnitTests/ReminderPlannerPropertyTests.cs`

Coverage uses fixed random seed `20260810` so failures are reproducible.

Tests include:

- 64 deterministic randomized daily planning windows;
- every produced occurrence must satisfy `ScheduledUtc >= fromUtc` and `ScheduledUtc < toUtc`;
- occurrence keys remain unique in each build result;
- returned occurrences remain chronologically ordered;
- cycle on/off matrices for on-days 1–5 × off-days 1–5 match explicit calendar arithmetic;
- every valid selected-weekday mask from 1 through 127 emits only selected days;
- every-N-hours values 1, 2, 3, 6, 8, 12, 24, 48, and 168 preserve exact elapsed UTC spacing.

All values are synthetic explicit schedule inputs. The tests do not infer treatment frequency or dosage.

## 8. Multi-zone DST gap/overlap matrix

Commit:

`72b4f18c6580164b50dbce5f5b70a512cb82da07` — `test: expand DST gap and overlap coverage`

Created:

`tests/CareNest.UnitTests/ReminderPlannerDstMatrixTests.cs`

Representative zone IDs:

- `America/New_York`;
- `Europe/Berlin`;
- `Australia/Sydney`.

For each available/valid host zone, the test scans 2026 for an invalid and ambiguous local time:

- invalid spring-forward local time must not be materialized;
- ambiguous fall-back local time must produce exactly one occurrence for that local time;
- repeated builds must produce the same `ScheduledUtc` and `OccurrenceKey` for the ambiguous time.

Hosts missing a named time zone return from that individual case rather than inventing replacement zone behavior.

## 9. Coordinator UTC rebuild and future-UTC snooze validation

Commit:

`d2250615b4937846b363b3fe6873fc9d64c958eb` — `fix: validate snooze timestamps before scheduling`

Updated:

`src/CareNest.Application/Services/ReminderCoordinator.cs`

Behavior:

- a supplied `RebuildAsync(fromUtc)` override must be UTC;
- `HandleOccurrenceAsync` obtains one authoritative current UTC time from `TimeProvider` for the state transition;
- snooze still requires a non-null explicit timestamp;
- snooze timestamp must have `DateTimeKind.Utc`;
- snooze timestamp must be strictly later than current UTC time;
- invalid snooze values fail before occurrence persistence or platform notification scheduling;
- validated snooze timestamp is used for occurrence state and notification request;
- existing privacy-redacted logging remains unchanged.

## 10. Coordinator snooze/UTC contracts

Commit:

`5e7a2a1f6fd83a31ba2a57622e0865e6b0df5815` — `test: protect reminder coordinator UTC and snooze guards`

Created:

`tests/CareNest.UiTests/ReminderCoordinatorSafetyContractTests.cs`

Source-contract coverage protects:

- rebuild UTC-kind check;
- non-null snooze requirement;
- snooze UTC-kind check;
- snooze future-time check;
- use of validated snooze timestamp for persisted occurrence and notification scheduling.

## 11. Archived-profile defense in depth

Commit:

`ede2cd0790fcfc52547157b751ff09d148af32bc` — `fix: suppress reminders for archived profiles`

Updated:

`src/CareNest.Application/Services/ReminderPlanner.cs`

The coordinator already skipped archived profiles before invoking the planner. This adds a second defensive gate so a future direct planner caller cannot automatically materialize occurrences for an archived profile.

## 12. Archived-profile planner test

Commit:

`c8896cf41e72ebaf7a995c1020ddfcd965250998` — `test: cover archived profile reminder suppression`

Created:

`tests/CareNest.UnitTests/ReminderPlannerArchivedProfileTests.cs`

The test supplies an otherwise valid active medicine/daily schedule for an archived profile and requires an empty occurrence result.

## 13. Reminder scheduling contract expansion

Commit:

`1439dd7f6208c5f5a9b202cc9b4da498c1cdada6` — `docs: harden reminder scheduling contract`

Updated:

`docs/testing/REMINDER_SCHEDULING_CONTRACT.md`

Now documents:

- entity ownership validation;
- archived-profile suppression;
- actual UTC planner windows;
- selected-weekday supported bits;
- explicit future-UTC snooze handling;
- UTC rebuild override;
- representative multi-zone DST matrix;
- deterministic fixed-seed property coverage;
- continued non-clinical/no-inference boundary.

## 14. Test-plan expansion

Commit:

`864b41f1838485ea89a9ef7b03e4da0964cc274e` — `docs: expand reminder integrity test plan`

Updated:

`docs/testing/TEST_PLAN.md`

The test plan now explicitly includes ownership checks, UTC-kind validation, representative multi-zone DST behavior, fixed-seed property coverage, snooze future-UTC requirements, and archived-profile suppression in addition to the previously recorded snapshot/app-lock/reminder tests.

## 15. Architectural decisions

Commit:

`94af97c0af62661a0eadb3644db6d865e06efc1b` — `docs: record reminder ownership and UTC decisions`

Added ADR-summary decisions:

- reminder ownership is validated before materialization;
- planner/rebuild/snooze timestamps use explicit UTC contracts;
- randomized recurrence coverage must remain reproducible and non-clinical.

## 16. Quality-gate expansion before verification

Commit:

`04057299fe6d13012734ba235e6fa92604753948` — `docs: add reminder ownership and UTC quality gates`

Updated:

`docs/releases/QUALITY_GATE.md`

This commit became the first Phase-8 exact-head verification base and recorded the new ownership, UTC, snooze, DST, property, and archived-profile automated gates.

---

# PR #29 — superseded exact-head verification that exposed CA2263

Verification branch:

`ci/carenest-rc1-ownership-utc-dst-hardening-20260810`

Source head:

`04057299fe6d13012734ba235e6fa92604753948`

Marker head:

`16e303a1fe285faee35743bb8207c4aa8c63d335`

Marker file:

`build/verification/rc1-ownership-utc-dst-hardening-20260810.txt`

Pull request:

`#29 — Verify reminder ownership, UTC, snooze, and DST hardening`

PR URL:

`https://github.com/sanskarIN/CareNest/pull/29`

The PR diff was verified as marker-only.

CareNest CI #246 / run `31382027314` exposed a real analyzer/compile problem on the newly added schedule enum validation:

- CA2263: the non-generic `Enum.IsDefined(Type, object)` overload should be replaced by the generic overload;
- the finding appeared during Apple compilation under the repository warnings-as-errors policy;
- the quality gate was not weakened and the analyzer was not suppressed;
- PR #29 was closed without merge and explicitly marked superseded;
- its marker never entered `main`;
- PR #29 is **not** green release evidence.

---

# CA2263 correction

Commit:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b` — `fix: use generic schedule enum validation`

Updated:

`src/CareNest.Domain/Rules/MedicineRules.cs`

Change:

- replaced `Enum.IsDefined(typeof(ScheduleKind), schedule.Kind)` with `Enum.IsDefined(schedule.Kind)`;
- behavior remains the same: unrecognized schedule enum values are rejected;
- the implementation now satisfies the enabled .NET analyzer instead of adding a suppression.

This commit became the exact corrected Phase-8 source head used for fresh verification.

---

# PR #30 — corrected exact-head ownership/UTC/snooze/DST verification

Verification branch:

`ci/carenest-rc1-ownership-utc-dst-hardening-20260810-2`

Exact verified source head:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

Verification marker head:

`59016b7e2b13d5ac1c93cf0db973f275c6e7eb19`

Marker file:

`build/verification/rc1-ownership-utc-dst-hardening-20260810-2.txt`

Pull request:

`#30 — Reverify reminder ownership, UTC, snooze, and DST hardening`

PR URL:

`https://github.com/sanskarIN/CareNest/pull/30`

The PR changed exactly one marker file beyond `main`. It was closed **without merge** after the complete matrix succeeded.

## CareNest CI #248

Run ID:

`31382194805`

Final conclusion:

**success**

Core job evidence from job `93434630410`:

- platform-neutral formatting: **success**;
- `CareNest.UnitTests`: **74 passed, 0 failed, 0 skipped**;
- `CareNest.IntegrationTests`: **13 passed, 0 failed, 0 skipped**;
- `CareNest.UiTests`: **54 passed, 0 failed, 0 skipped**;
- total automated core test cases: **141 passed, 0 failed, 0 skipped**.

Platform job evidence:

- Android Release job `93434630440`: **success**;
- Windows Release job `93434630484`: **success**;
- Apple job `93434630334`: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**.

## CodeQL #248

Run ID:

`31382194687`

Final conclusion:

**success**

## Dependency Audit #10

Run ID:

`31382194683`

Final conclusion:

**success**

The audit result does not change the open status of `GHSA-2m69-gcr7-jv3q`; the narrow suppression remains a visibility mechanism rather than a vulnerability fix.

## PR #30 result

For exact source head `c61f3c31...`:

- formatting green;
- 141/141 core automated tests green;
- Android green;
- Windows green;
- iOS simulator green;
- Mac Catalyst green;
- CodeQL green;
- Dependency Audit green;
- marker closed without merge.

This supersedes PR #28 as the latest exact automated source baseline.

---

# Current automated policy and safety coverage

## Critical product flows

- onboarding safety/local-first text;
- profile deletion/export contracts;
- medicine/schedule UI contracts;
- as-needed no-automatic-reminder behavior;
- reminder/status terminology;
- About support/safety boundaries;
- schedule state/date/cycle/every-N-hours boundaries;
- planner entity ownership;
- planner UTC window semantics;
- rebuild UTC override contract;
- future-UTC snooze validation;
- planner window/dedup/order invariants;
- representative multi-zone daylight-saving gap/overlap behavior;
- deterministic property-style recurrence boundaries.

## Architecture

- project dependency direction;
- MAUI isolation;
- no direct persistence reach-through from ViewModels;
- no runtime network clients in ViewModels.

## Safety

- no named dosage calculator/inference feature;
- no named diagnosis feature;
- no treatment recommendation feature;
- no interaction checker;
- no clinical risk scoring;
- opaque strength/instruction fields;
- explicit user-entered stock change;
- invalid local clock times do not cause inferred alternate reminder times;
- ownership mismatches fail rather than silently materializing reminder data under another local entity;
- local/unspecified time values are not silently reinterpreted as UTC planner/rebuild/snooze values.

## Privacy/security

- no full exception-object logging patterns;
- no common signing/private-key file types;
- no committed runtime telemetry/network client introduction;
- required privacy/security/release files exist;
- BMC link is fixed/explicit;
- no implicit notification permission request during onboarding;
- app-lock PIN is not directly persisted;
- app-lock verifier uses PBKDF2-HMAC-SHA256 and fixed-time comparison;
- derived/retrieved verifier buffers are cleared after verification paths;
- disabling app lock removes lock material;
- app-lock security limitation is explicit.

## Persistence/backup

- SQLite migration/integrity checks;
- WAL mode and busy timeout;
- WAL checkpoint result handling;
- snapshot file creation;
- snapshot committed-record content;
- copied-snapshot integrity;
- pre-cancelled snapshot leaves no output;
- encrypted backup restore/wrong-password/tamper behavior;
- encrypted document round-trip/tamper behavior.

## Engineering quality

- no committed runtime TODO/FIXME/NotImplemented placeholders;
- no common synchronous task-blocking patterns;
- branding SVGs remain well-formed;
- resource keys remain present;
- architecture references remain portable;
- platform-neutral formatting enforced;
- cross-platform MAUI Release builds enforced;
- analyzers remain active and real findings such as CA2263 are fixed rather than hidden.

These preventive controls do not replace manual application behavior, security, accessibility, store-policy, or device testing.

---

# Privacy-aware logging implementation

## SafeUiErrorService

- accepts caller-supplied safe UI text;
- does not display raw exception text;
- does not pass a full exception object to the structured logger;
- only computes/logs exception type when Error logging is enabled.

## GlobalExceptionHandler

- registered in DI;
- attached during application construction;
- idempotent attachment with `Interlocked.Exchange`;
- observes unhandled application-domain exceptions;
- observes unobserved task exceptions;
- logs only type/category metadata;
- uses explicit enabled-level guards;
- sets unobserved task exceptions observed after safe handling.

## ReminderCoordinator

- reminder scheduling failures do not log occurrence/medicine IDs;
- low-stock reminder scheduling failures do not expose health-record identifiers;
- full exceptions/messages/stack traces are not passed;
- safe exception type metadata is evaluated only when Warning logging is enabled;
- rebuild override timestamps are required to be UTC;
- snooze values are validated before state persistence/platform scheduling.

## StartupCoordinator

- non-fatal reminder recovery does not pass a full exception object;
- logs only safe exception type metadata when Warning is enabled.

## Documentation

`docs/security/LOGGING_PRIVACY.md` explicitly prohibits medicine/profile data, appointment notes, document data/paths, reminder notes, PIN/backup secrets, keys, raw health-record identifiers, full exception objects/messages/stack traces, and URLs containing user data.

---

# App-lock security implementation and limitations

App lock remains optional and local.

Current implementation properties:

- no plaintext PIN persistence;
- numeric 6–32 digit PIN policy;
- random 16-byte salt;
- PBKDF2-HMAC-SHA256 verifier derivation;
- 210,000 derivation iterations;
- secure platform secret-store persistence of salt/verifier/enabled state;
- fixed-time verifier comparison;
- derived verifier buffer zeroing;
- retrieved verifier buffer zeroing after verification paths;
- lock material removed when disabled.

Explicit limitations:

- app lock is not whole-database encryption;
- app lock is not device encryption;
- app lock does not replace device-level authentication;
- weak numeric PINs have limited entropy;
- a compromised/rooted/jailbroken device or compromised secure store remains outside the protection promise;
- future biometric recovery/bypass or remote PIN recovery requires a separate security review.

---

# Deterministic reminder reliability rules

The current planner contract is documented in `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`.

Rules include:

- schedules originate only from explicit user input;
- the supplied schedule must belong to the supplied medicine;
- the supplied medicine must belong to the supplied profile;
- persisted schedule times must belong to the supplied schedule;
- intentionally unbound editor times remain supported before persistence;
- archived profiles produce no automatic occurrences;
- reminder occurrence keys are stable/deterministic;
- rebuilds are idempotent;
- planner windows require actual UTC values;
- windows include `fromUtc` and exclude `toUtc`;
- duplicate clock times collapse to one stable occurrence;
- output is chronological;
- daily/custom date boundaries are enforced;
- selected weekdays use only the seven supported mask bits;
- unknown schedule kinds are rejected;
- cycles use only explicit on/off days;
- every-N-hours uses explicit start time/interval;
- as-needed produces no automatic occurrences;
- disabled schedules produce no automatic occurrences;
- paused/completed/archived medicines produce no automatic occurrences;
- invalid spring-forward local times are not silently shifted;
- ambiguous fall-back local times produce a deterministic occurrence;
- representative North America/Europe/Australia DST transitions are covered when available;
- future reminders are rebuilt at startup;
- explicit rebuild overrides must be UTC;
- snooze requires a future UTC timestamp;
- overdue occurrences are reconciled;
- Android responds to reboot/time/time-zone rebuild signals;
- stored schedule times are not silently rewritten after time-zone changes;
- notification permission denial is surfaced;
- exact-alarm/battery limitations are surfaced on Android;
- iOS/Mac Catalyst use OS-managed local notifications;
- Windows fallback limitations are reported rather than hidden;
- quiet hours are user-controlled;
- follow-up reminders are user-controlled;
- stock changes after Taken use only user-configured quantity change;
- stock estimates explicitly warn users to check actual supply;
- reminder scheduling failures are privacy-redacted in logs.

Deterministic property tests use a fixed seed and explicit synthetic inputs. These are organizational scheduling rules, not treatment or dosage advice.

---

# WAL snapshot/backup reliability rules

`SqliteDatabase.CreateSnapshotAsync` uses the existing SQLite WAL checkpoint path before copying the database snapshot.

Automated regression evidence checks:

- WAL mode enabled;
- busy timeout configured;
- full WAL checkpoint result consumed;
- nonempty snapshot file created;
- a committed profile record is present in the copied database;
- copied database passes `PRAGMA integrity_check`;
- a pre-cancelled operation throws cancellation;
- pre-cancelled operation does not leave a snapshot output file.

This does not replace the required manual clean-install encrypted backup/restore test on final packaged builds.

---

# Release-engineering state

## CI

`.github/workflows/ci.yml` verifies:

- platform-neutral formatting;
- unit tests;
- integration tests;
- UI-contract/policy tests;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build.

Latest verified source head `c61f3c31...` passed CI #248 / `31382194805`.

## CodeQL

CodeQL is an independent security-analysis workflow and passed PR #30 through CodeQL #248 / `31382194687`.

## Dependency Audit

`.github/workflows/dependency-review.yml` audits platform-neutral and Android MAUI dependency graphs using NuGet audit mode and passed PR #30 through Dependency Audit #10 / `31382194683`.

The green audit does not mean the tracked SQLitePCLRaw advisory is fixed.

## Production Release Gate

`.github/workflows/release-gate.yml` intentionally blocks final production release while the dependency risk register has an open production risk, release checklist items remain incomplete, required release documents are missing, or source tests fail.

It is expected that final `1.0.0` remains blocked because manual/store/signing/SQLite-risk decisions are not complete.

## Release Evidence

`.github/workflows/release-evidence.yml` can be manually or tag-triggered to produce source/toolchain/test/dependency/checksum evidence for the eventual promoted commit. It has not been falsely marked as final release evidence for a public package.

---

# Security model implemented

- Imported documents/profile photos use encrypted `.cndoc` storage.
- Document encryption uses authenticated AES-256-GCM primitives rather than custom cryptography.
- Backups use password-derived authenticated encryption and a schema-versioned format.
- The encrypted backup payload carries document key material needed for portable restore without storing it in plaintext.
- App-lock PINs are not stored directly; a salted PBKDF2-HMAC-SHA256 verifier is stored through secure platform storage.
- App-lock verification uses fixed-time comparison and clears verifier buffers after checks.
- Reminder entity ownership is validated before materialization to fail closed on inconsistent local object graphs.
- Planner/rebuild/snooze time-kind contracts reject silent local/unspecified-to-UTC reinterpretation.
- No API keys, signing keys, certificates, passwords, or production secrets are committed.
- No analytics/telemetry client is part of v1.
- No CareNest backend/cloud sync/account system/automatic upload exists in v1.
- Exported/decrypted files leave CareNest protection only after explicit user action.
- SQLite records rely on application sandbox/device protections; CareNest does **not** claim transparent whole-database encryption at rest.
- The open SQLitePCLRaw advisory is explicitly tracked and is not hidden by claiming it fixed.
- External Buy Me a Coffee destination is fixed in a shared HTTPS constant and opens only after explicit user action.
- CareNest does not append health data, document metadata, profile identifiers, reminder history, backup data, app-lock data, or payment secrets to the funding URL.
- External funding provider is outside the CareNest trust boundary after launch.
- Runtime exception logging is intentionally redacted.

---

# Acceptance-criteria mapping

- No account/network required: implemented.
- No CareNest backend/login/cloud sync: implemented for v1.
- No diagnosis/treatment/dosage decisions: enforced through scope, domain behavior, UI text, policy tests, and documentation.
- Reminder recovery: startup rebuild plus Android boot/time/time-zone integration implemented.
- Deterministic reminder boundaries/DST behavior: implemented and contract tested.
- Planner entity ownership: implemented and unit tested.
- Archived-profile planner suppression: implemented and unit tested.
- UTC planner/rebuild/snooze boundaries: implemented and contract tested.
- Deterministic randomized/property recurrence boundaries: implemented with a fixed reproducible seed.
- Representative multi-zone DST gap/overlap coverage: implemented when host zone IDs are available.
- Permission/battery limitations: surfaced.
- Profile export/delete: implemented.
- Document export/delete: implemented.
- Logs exclude health-document contents and raw sensitive exception details: implemented and contract tested.
- Medical limitations in onboarding/About: implemented.
- Manual encrypted backup/restore: implemented with version/integrity validation and rollback handling.
- WAL-backed snapshot content/integrity/cancellation: regression tested.
- No automatic cloud upload: implemented by architecture.
- Local caregiver mode: implemented without silent sharing.
- Theme/accessibility/localization readiness: implemented.
- App-lock privacy barrier: implemented with verifier-memory hardening and explicit limitations.
- Automated quality gate: exact source head `c61f3c31...` passed formatting, 141 tests, Android/Windows/iOS/Mac, CodeQL, and Dependency Audit.
- Voluntary funding: implemented without changing CareNest health behavior or local-data access.
- Branding variants: adaptive/splash/standard/light/dark/monochrome/support artwork present and contract tested.
- Release evidence/provenance process: workflow/docs present; final public-release evidence run intentionally remains pending until production blockers are cleared.

---

# Platform limitations retained intentionally

## Android

Reminder timing can be affected by notification permission, exact-alarm capability, battery optimization, force-stop state, reboot state, and operating-system policy. CareNest reports these limitations rather than guaranteeing delivery.

## iOS / Mac Catalyst

Local notification delivery remains controlled by operating-system policy. CI compilation proves source/build compatibility, not guaranteed real-device delivery.

## Windows

The current fallback cannot guarantee reminder delivery while CareNest is not running. The application reports this limitation.

## All platforms

Device shutdown, permission revocation, platform scheduling policy, system updates, battery management, and target-specific background restrictions can affect reminder delivery.

---

# Current production blockers

Automated exact-head verification is green, but public `1.0.0` remains intentionally blocked until these real tasks are completed:

1. Complete `docs/releases/MANUAL_TEST_MATRIX.md` across Android, Windows, iOS/iPadOS, and Mac Catalyst on representative real/emulated targets.
2. Manually verify notification permission denied/granted flows.
3. Manually verify Android exact-alarm/battery/reboot/time/time-zone behavior on representative devices.
4. Manually verify real-target reminder behavior including documented limitations and snooze behavior against actual platform notification scheduling.
5. Manually verify document import/export/delete.
6. Manually verify calendar export.
7. Manually verify encrypted backup/restore on a clean installation/release build.
8. Manually verify cold-start app lock.
9. Complete screen-reader, large-text, keyboard, contrast, and reduced-motion accessibility review.
10. Review current Apple App Store and Google Play policy for the external voluntary support link.
11. Conditionally remove/hide the in-app external support action for a distribution channel if current rules require it.
12. Prepare signing identities/credentials outside Git.
13. Build/inspect signed packages on fully provisioned hosts.
14. Complete store screenshots, listing text, privacy/data-safety disclosures, support/privacy/terms/security links, and package identity checks.
15. Resolve or make an explicit production release decision for the open SQLitePCLRaw advisory.
16. Run `CareNest Release Evidence` for the exact commit ultimately promoted to public `1.0.0`.
17. Create final release notes/tag/GitHub release only after all applicable gates are complete.

No manual item above is marked complete merely because the automated matrix is green.

---

# Deferred future-version scope

The following remain deliberately deferred and require a new privacy/security/abuse/threat-model review before implementation:

- encrypted cloud synchronization;
- remote caregiver collaboration;
- accounts;
- mobile-number authentication;
- server-side storage;
- remote sharing;
- telemetry/analytics;
- any medical interpretation;
- diagnosis;
- treatment recommendations;
- dosage calculation/inference;
- medication-interaction claims;
- clinical risk scoring.

The medical decision boundary is not a roadmap item to remove.

---

# Future automated hardening still worth considering

These are not missing RC1 core features; they are additional quality improvements for later iterations:

- platform UI automation on stable real/emulated target infrastructure;
- deeper notification-permission denial/retry state-transition automation;
- backup compatibility fixtures across future schema versions;
- file-corruption and low-storage target failure-path tests;
- expanded semantic/accessibility XAML contracts while retaining manual assistive-technology testing;
- SBOM generation for release artifacts;
- artifact attestations/provenance where supported;
- GitHub Dependency Review action if repository Dependency Graph becomes available;
- protected signed-artifact workflow after signing identities are provisioned securely.

The earlier roadmap items for additional representative DST zones and deterministic randomized/property recurrence-boundary tests are now complete in Phase 8 and have been removed from the future-work list.

---

# Commit identity note

The requested maintainer Git identity remains documented/configured through repository setup scripts:

```bash
git config user.email "sanskarin@outlook.in"
git config user.name "Sanskar"
```

Relevant setup files include:

- `build/scripts/setup-git.sh`;
- `build/scripts/setup-git.ps1`;
- `docs/setup/DEVELOPMENT.md`.

The connected GitHub write API used in this chat does not expose author/committer email parameters for contents-API create/update operations. Connector-created commits therefore use the authenticated GitHub identity. This repository does not falsely claim the connector forced `sanskarin@outlook.in` into those commit objects.

Local/future maintainer commits can use the requested address through the provided setup scripts.

---

# Local environment limitation

The local execution container used in this conversation does not provide a full .NET MAUI development host with all required workloads/device tooling. Therefore the repository does not claim local MAUI restore/build/emulator/device/signing/store-package execution from that container.

GitHub-hosted Actions supplied the exact automated evidence recorded above.

Manual target-device tests, signing identities, current store-policy decisions, and store submissions require external provisioned environments/credentials and remain explicitly open.

---

# Documentation-only commits after PR #28 source head

Exact PR #28 verified runtime/test/source head:

`69c4dd9319f7dc47edea1786e683f7d90c656e1e`

After PR #28 completed green, documentation-only commits aligned the status/evidence before Phase 8 source work began:

- `7262b7c8f4e62b569d590d7ceaeaedbb2a2f4b5a` — `docs: record green reminder and app-lock hardening baseline`;
- `4be7d0496b0a04a0e595e54b25f490a11ee3a79a` — `docs: record PR28 automated release evidence`;
- `928c4d3a141cc844edb62dbc5cb45896d607a2c0` — `docs: record reminder snapshot and app-lock hardening`;
- `c1631cbf6816aeda952074d5f21007d0ca848350` — `docs: record deterministic reminder and app-lock memory decisions`;
- `cffec1df155efe824cc93e36b86be154950001b1` — `docs: align next steps with PR28 hardening evidence`;
- `26914b553b26fa0ff6986b23a8846de13999ff36` — `docs: link deterministic scheduling and PR28 quality baseline`;
- `b729a117feb5381f6574e8b018cdcdc4dd04f1fb` — `docs: add reminder snapshot and app-lock quality gates`;
- `044c0f91bfde15b1f85474656e2ca9faedb05085` — `docs: expand app-lock and snapshot security release review`.

These commits did not change the PR #28 verified runtime/test source.

---

# Phase 8 commits before the final source freeze

Runtime/test/documentation commits included:

- `0f22de1240d28c8011c3c4f41d0a084211706a8d` — planner ownership enforcement;
- `098644c73d7025142ef29f213933f01e8ba52959` — schedule enum/weekday/time-zone validation;
- `e565c90cfc89fedb72127be520f71110e1307c11` — ownership tests;
- `0c838901d3d90e37c23d29f77de49864e9cac080` — validation hardening tests;
- `d505b76d178f71cd01ecb6bd7d3daac2f01e76ef` — UTC planner windows;
- `d222f9855b7ceb65cf1209886e51385ad22af064` — UTC window tests;
- `c4be9647b1393d3d978a8bc10643c3cfb3ccf91d` — deterministic recurrence property tests;
- `72b4f18c6580164b50dbce5f5b70a512cb82da07` — multi-zone DST tests;
- `d2250615b4937846b363b3fe6873fc9d64c958eb` — coordinator UTC/snooze validation;
- `5e7a2a1f6fd83a31ba2a57622e0865e6b0df5815` — coordinator safety contracts;
- `ede2cd0790fcfc52547157b751ff09d148af32bc` — archived-profile suppression;
- `c8896cf41e72ebaf7a995c1020ddfcd965250998` — archived-profile test;
- `1439dd7f6208c5f5a9b202cc9b4da498c1cdada6` — scheduling contract expansion;
- `864b41f1838485ea89a9ef7b03e4da0964cc274e` — test-plan expansion;
- `94af97c0af62661a0eadb3644db6d865e06efc1b` — ownership/UTC ADR decisions;
- `04057299fe6d13012734ba235e6fa92604753948` — quality gate expansion and PR #29 source freeze;
- `c61f3c31c4ba33419c7b348fc8ee63a58eaa637b` — CA2263 correction and final PR #30 source freeze.

The exact final source/test/documentation head used by PR #30 is `c61f3c31...`.

---

# Documentation-only commits after the latest verified source head

Exact latest verified runtime/test/source head:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

After PR #30 completed green, the following documentation-only commits recorded/aligned the new evidence without changing the runtime/test source that passed CI #248:

- `a3a55404f0703f2614a89db86cbb48feaf5dc69f` — `docs: promote verified reminder ownership hardening baseline` (`PROJECT_STATUS.md`);
- `d64a4a84c43d81078928ec70accd3c1cb3f69284` — `docs: record green reminder ownership verification evidence` (`docs/releases/RELEASE_CHECKLIST.md`);
- `03f44fb07276e2ce7daa161f9875916bba0bf2a5` — `docs: advance verified reminder hardening roadmap` (`docs/releases/NEXT_STEPS.md`);
- `c56188ba007a1e22dae8072622fbda6621d2d709` — `docs: promote green reminder integrity quality baseline` (`docs/releases/QUALITY_GATE.md`);
- `8c62e626db219c2fe90e61adc832f62f08fe68f2` — `docs: extend reminder integrity security review` (`docs/releases/SECURITY_RELEASE_REVIEW.md`);
- `5af5d12d7b5a617bdbd9414bffd754a7e10d038b` — `docs: publish verified reminder integrity baseline` (`README.md`);
- `9f43bbe4c1f6369a50bf366b30e5839b4714868d` — `docs: record verified reminder ownership and UTC hardening` (`CHANGELOG.md`).

This `what_changed.md` commit is documentation-only as well. A final compare is performed after this handoff update so the repository can prove that every commit after exact source head `c61f3c31...` changes documentation only.

---

# Current repository state

- Complete CareNest `1.0.0-rc.1` source remains on `main`.
- Latest exact verified runtime/test/source head is `c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`.
- Superseded verification PR #29 is closed without merge and is not green evidence.
- PR #29 correctly exposed CA2263 in the new non-generic enum-validation call.
- The CA2263 finding was fixed on `main` instead of suppressed.
- Final Phase-8 verification PR #30 is closed without merge.
- PR #30 marker head `59016b7e2b13d5ac1c93cf0db973f275c6e7eb19` did not enter `main`.
- CareNest CI #248 / `31382194805`: **success**.
- CodeQL #248 / `31382194687`: **success**.
- Dependency Audit #10 / `31382194683`: **success**.
- Platform-neutral formatting: **success**.
- Unit tests: **74/74 passed**.
- Integration tests: **13/13 passed**.
- UI-contract/policy tests: **54/54 passed**.
- Total core automated test cases: **141/141 passed**.
- Android Release: **success**.
- Windows Release: **success**.
- iOS simulator Release: **success**.
- Mac Catalyst Release: **success**.
- Planner entity ownership, UTC windows, archived-profile suppression, stable half-open boundaries, dedup/order, explicit recurrence patterns, and multi-zone DST behavior are automated and documented.
- Reminder coordinator rebuild/snooze UTC boundaries are protected by runtime validation/source contracts.
- Deterministic fixed-seed recurrence property coverage is active.
- WAL snapshot committed-content/integrity/cancellation regression coverage is active.
- App-lock verifier-memory clearing is implemented and source-contract protected.
- App lock remains explicitly documented as a local privacy barrier, not whole-database/device encryption.
- Global/UI/startup/reminder exception logging is privacy-redacted and analyzer-compliant.
- Repository policy, architecture, ViewModel, data-model, branding, async, logging privacy, app-lock security, reminder-integrity, and coordinator-safety contracts are active.
- Dependency Audit, Release Gate, Release Evidence, CI, CodeQL, and Dependabot configuration are present.
- README/SUPPORT/About funding surfaces and both CareNest support artwork variants are present.
- The Buy Me a Coffee URL remains `https://buymeacoffee.com/sanskarIN`, voluntary, external, and non-entitlement based.
- The SQLitePCLRaw advisory remains explicitly open and is not claimed fixed.
- Final public `1.0.0` tagging/publication remains gated on manual device/accessibility/notification testing, current store-policy review, signing/store preparation, final Release Evidence for the exact promoted commit, and the SQLite dependency-risk decision/resolution.
- Cloud sync, remote caregiver collaboration, accounts/server storage, diagnosis, treatment advice, dosage calculation/inference, interaction checking, and clinical risk scoring remain deferred/out of scope.
