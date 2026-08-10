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

## Safety boundary implemented

CareNest remains a local-first organizational application. It does not diagnose conditions, determine dosage, infer doses, recommend treatment, perform medication-interaction checking, create clinical risk scores, replace a doctor/pharmacist, or provide emergency services.

All reminder schedules originate from explicit user input. Medicine strength and instruction text are stored as opaque text exactly for organizational display/use and are not interpreted as dosage rules. `StockChangePerTakenEvent` is explicitly user-entered and is never inferred from medicine strength or instruction text.

Reminder delivery limitations are surfaced rather than hidden. Notification permissions, battery optimization, exact-alarm capability, operating-system restrictions, force-stop/shutdown behavior, daylight-saving changes, time-zone changes, reboot behavior, and platform scheduling policy can affect delivery.

CareNest tells users to follow qualified professional instructions and to contact local emergency services in an emergency rather than rely on CareNest.

Buy Me a Coffee support is voluntary project support only. It does not unlock app functionality, medical advice, priority health support, different reminder behavior, emergency assistance, a CareNest account, or access to local CareNest data. The funding destination is an external trust boundary opened only after explicit user action.

---

## Product scope implemented

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
- scheduled, snoozed, taken, skipped, delayed, and missed reminder states;
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
- unit, integration, UI-contract, repository-policy, architecture, ViewModel, data-model, branding/localization, async-safety, and logging-privacy tests;
- GitHub Actions cross-platform CI;
- platform-neutral formatting gate;
- CodeQL analysis;
- NuGet dependency-audit workflow;
- explicit production release gate;
- release-evidence/provenance workflow;
- Dependabot configuration;
- privacy-safe structured bug report form;
- architecture, security, privacy, testing, setup, troubleshooting, release, BMC, and store documentation;
- Bash and PowerShell release-preflight scripts;
- manual cross-platform release test matrix;
- store-submission checklist;
- SQLite dependency migration/verification plan;
- release-quality gate, security review, release evidence, release notes template, and exact-head verification protocol.

---

## Repository structure

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

Completed before the current privacy/policy continuation:

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

Completed in the 2026-08-10 continuation:

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
- final fully green automated verification at PR #27.

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

Before the 2026-08-10 hardening continuation, the last fully green baseline was source head:

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

This baseline became important during the repository-history recovery audit described below.

---

# 2026-08-10 continuation — complete hardening record

## 1. Platform-neutral formatting gate

Commit:

- `a63bd413ed0e1760e3391552962ca19128be93eb` — `ci: verify formatting for platform-neutral projects`.

The core CI job now verifies formatting separately for:

- `CareNest.Shared`;
- `CareNest.Domain`;
- `CareNest.Application`;
- `CareNest.Infrastructure`;
- `CareNest.UnitTests`;
- `CareNest.IntegrationTests`;
- `CareNest.UiTests`.

The MAUI app is intentionally excluded from this platform-neutral formatting loop because the local/core runner does not install every MAUI target workload. Cross-platform MAUI compilation remains a separate Android/Windows/Apple gate.

Final PR #27 evidence confirms the formatting gate passes.

## 2. Repository safety/completeness policy tests

Initial commit:

- `3e3e70779b4156ffcf5daa34c79f113511db84fc` — `test: enforce repository safety and completeness policies`.

The tests protect against:

- runtime `TODO` implementation markers;
- runtime `FIXME` markers;
- `NotImplementedException` placeholders;
- runtime `System.Net.Http`/`HttpClient` introduction;
- gRPC client introduction;
- analytics/telemetry client introduction;
- named diagnosis/dosage/treatment/interaction/risk-scoring feature regressions;
- common signing/secret files such as `.p12`, `.pfx`, `.jks`, `.keystore`, `.env`, and mobile service credential files;
- accidental deletion of required governance, CI, security, and release files.

The first runtime source scan was too broad and included SDK-generated `obj` files. CI correctly exposed that false positive. The test was fixed to inspect committed workspace source and ignore generated `bin`, `obj`, and `.git` segments.

Final commit for that correction:

- `ed84c0998c38b87f03c5474a3522b4357d45c073` — `test: scope repository policy scans to committed source`.

## 3. Additional CareNest branding variants

Added original vector variants required by the branding/release prompt:

- `8404ba13bb94ea06fcc0c41bf1b5e787e555a667` — monochrome CareNest system icon;
- `c998f0108770d4399e734fe0ba7efd3c852b4b17` — light CareNest mark variant;
- `ed6ddbd242ba6be27daeb41710234858cb78e623` — dark CareNest mark variant.

The original app icon, foreground adaptive icon, splash, standard CareNest mark, compact support badge, and custom BMC artwork remain present.

## 4. Branding/localization contracts

Initial commit:

- `7cc58c93ca0e65c71e59d049c5b22826c18070b3` — branding/localization contract tests.

Coverage validates:

- MAUI adaptive icon declaration;
- foreground icon resource;
- splash declaration;
- MAUI image resources;
- XML/SVG well-formedness of required branding files;
- English safety/branding resource keys;
- BMC support destination and artwork consistency;
- clickable support artwork in About;
- explicit BMC URL and voluntary-support copy.

After restoring the original compact support badge and About card, coverage was expanded by:

- `15f3b94f9a5a02ffc5090290b7de06a5cd006996` — `test: expand restored support branding contracts`.

## 5. ViewModel boundary contracts

Initial commit:

- `b47ba485c2b8edb77d19c01140b7adaac02c152d` — ViewModel contract coverage.

The first version incorrectly treated `ObservableViewModel` ICommand adapter helpers as ordinary ViewModel command bodies. Those adapters legitimately require `async void` because they implement ICommand `Execute` semantics. The rule was narrowed to concrete ViewModels:

- `303d89606f746c9480bbcb19cb018c7e1a7d98e0` — `test: scope async-void rule to concrete ViewModels`.

Current contracts assert:

- concrete ViewModels do not use `async void`;
- concrete ViewModels do not use `Task.Run` to hide blocking work;
- ViewModels do not reach directly into `SQLiteAsyncConnection`/`SqliteDatabase` persistence;
- ViewModels do not construct network clients;
- About uses centralized public constants;
- onboarding does not request notification permission;
- schedule editing preserves explicit `AsNeeded` no-reminder behavior and requests notification permission only in reminder-capable flows.

## 6. Architecture-boundary contracts

Initial commit:

- `f5a8827d86023d50a57254d97dec36895b25eb64` — architecture boundary tests.

Contracts enforce:

- Shared has no project dependencies;
- Domain depends only on Shared;
- Application depends only on Domain + Shared;
- Infrastructure depends only on Application + Domain + Shared;
- platform-neutral projects do not reference MAUI;
- the MAUI app is the runtime composition root.

CI then exposed two test-quality issues and both were fixed:

- Windows-style project-reference backslashes needed normalization on Linux;
- `Path.GetFileNameWithoutExtension` needed an explicit non-null contract under nullable analysis.

Corrections:

- `182533922f491f5c122e9cacd6f0c8c12d43c493` — cross-platform project-reference separator normalization;
- `8417513db36c72b0ec2cfaccadb6ac47ba361f11` — `test: make project reference names explicitly non-null`.

That final commit is the exact runtime/test source head verified green by PR #27.

## 7. Required data-model contracts

Commit:

- `d1a0c5682b8e1f0919e95459488d367d88746806` — required CareNest data-model contracts.

Coverage requires the prompt-defined domain entities, including:

- `PersonProfile`;
- `Medicine`;
- `MedicineSchedule`;
- `ScheduleTime`;
- `ReminderOccurrence`;
- `MedicationLogEntry`;
- `Appointment`;
- `CareDocument`;
- `Tag`;
- `DocumentTag`;
- `StockAdjustment`;
- `EmergencyContact`;
- `AppSetting`;
- `BackupMetadata`;
- `AuditEntry`.

It also protects the critical safety rule that medicine strength and instructions remain strings/opaque text and that stock changes are explicit user-stored values rather than inferred dosage behavior.

## 8. Privacy-aware global exception observer

The prompt requested privacy-aware logging and global exception handling. An explicit global observer was not yet wired, so this continuation added it.

Commits:

- `aaad2e1e3abc8ff99d63e177c377d62027402fab` — add `GlobalExceptionHandler`;
- `37f93bcf273655af8c6cf1b900fe5ccd792d1795` — register the observer in dependency injection;
- `01fb02620472b882f3d620b291b6adb6bbf2deaf` — attach it during app construction;
- `915f4a45dc687eed1be7506292568782d2ceff3f` — global exception privacy contracts.

Current behavior:

- attaches once using `Interlocked.Exchange`;
- observes `AppDomain.CurrentDomain.UnhandledException`;
- observes `TaskScheduler.UnobservedTaskException`;
- marks unobserved task exceptions observed after safe logging;
- does not log exception messages;
- does not log stack traces;
- does not pass full exception objects to the logger;
- logs only low-sensitivity category/type metadata.

## 9. UI error logging privacy fix

Existing `SafeUiErrorService` passed a full exception object to the structured logger. Full exception objects can include messages and paths, which is inappropriate for a privacy-sensitive organizer.

Commits:

- `bba6615a224b87aec31f5c033fe2b214595c193f` — `security: redact exception details from UI error logs`;
- `a95936d0bcd665531b6a78326bcd8726271a471f` — regression coverage.

Current UI error behavior:

- user sees only caller-supplied safe UI text;
- logger receives only safe exception type/category metadata;
- no message/stack/full exception is passed.

## 10. Async non-blocking policy contracts

Commit:

- `907e5a71796a15b78dec75e26b6af00ba692257c` — async non-blocking runtime source tests.

Runtime source policy prevents common synchronous task-blocking patterns such as:

- `.GetAwaiter().GetResult()`;
- `.Wait()`;
- `Thread.Sleep`;
- `Task.WaitAll`;
- `Task.WaitAny`;
- common `.Result` blocking patterns.

This complements compiler/analyzer and code-review discipline rather than replacing runtime device testing.

## 11. Release Evidence workflow

Commit:

- `9b5abf8ed7cf1a11b6e613db4f2af00ba692257c` was planned in the continuation record; the actual repository workflow commit is the `ci: add reproducible release evidence workflow` commit in current history.

The workflow at `.github/workflows/release-evidence.yml` is manual/tag-triggered and records:

- exact Git SHA;
- Git ref;
- .NET runtime/SDK information;
- unit-test TRX;
- integration-test TRX;
- UI-contract-test TRX;
- transitive dependency inventories for platform-neutral projects;
- SHA-256 checksums;
- uploaded run artifact with 30-day retention.

Supporting documentation:

- `docs/releases/RELEASE_EVIDENCE.md`;
- `docs/releases/QUALITY_GATE.md`;
- `docs/releases/SECURITY_RELEASE_REVIEW.md`;
- `docs/releases/RELEASE_NOTES_TEMPLATE.md`;
- `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

The workflow has **not** been falsely marked as a completed final-release artifact run. It is intended for the exact commit that will eventually be promoted after the remaining production blockers are cleared.

## 12. Reminder logging privacy hardening

Existing reminder scheduling catch paths could pass full exception objects and identify reminder/medicine records through structured log properties.

Commits:

- `3e05f6ccd5965c29eaaa11b9cff5ba018a585a2a` — redact reminder exception details and record IDs;
- `f07a0dede776bbfca16163a26b1a99a35ee7694b` — reminder logging privacy tests.

Final behavior logs only exception type/category metadata when Warning logging is enabled.

## 13. Logging privacy source policy

Commit:

- `28b5e220ac661123abceb576c28218b36846bb12` — structured logger exception-object source policy.

The initial scanner also saw generated source after builds. It was corrected to scan committed runtime source only:

- `853d3d8254fd0b30a386a42d2d1fde316bc46a43` — committed-source-only logging privacy scan.

This is a preventive text/source policy and does not grant permission to log sensitive content merely because a string pattern passes the check.

## 14. Privacy/security documentation for logging

Commits/files added or updated:

- `PRIVACY.md` now states exception logging records safe categories/type names and intentionally excludes full exception objects/messages/stack traces/user-entered health text/file paths;
- `docs/security/LOGGING_PRIVACY.md` defines allowed and prohibited log data;
- `SECURITY.md` links and enforces that diagnostic boundary.

## 15. Logger eager-argument analyzer fixes

GitHub MAUI compilation surfaced CA1873 because even safe exception-type lookup should not be eagerly evaluated when the corresponding log level is disabled.

Intermediate precomputation commits existed, but CI correctly demonstrated that explicit level guards were still required.

Final fixes:

- `8209ed49eeaee5bd2341e4f5a108f126f7c73d06` — explicit Critical/Error `ILogger.IsEnabled` guards in `GlobalExceptionHandler`;
- `ebb5e1b66e574552dddab3fe3252cd230fc175f8` — explicit Error guard in `SafeUiErrorService`;
- `850355e618d206ef1276ede5b28c5c925f47a9d1` — explicit Warning guards in reminder logging.

The quality gate was not weakened or suppressed; runtime code was changed to satisfy it.

## 16. Existing StartupCoordinator privacy issue discovered by new tests

The new logging policy discovered that `StartupCoordinator` still used the full-exception overload for non-fatal reminder recovery logging.

Fix:

- `78657718aab236456bb95a33e5f57c00649f9c73` — `security: redact startup recovery exception logging`.

Startup recovery now follows the same privacy boundary as the global/UI/reminder paths.

## 17. Repository-history recovery audit

A compare against the previously green source head `52abe54...` found that an earlier repository ref recovery had not replayed nine valid later files/changes.

Rather than accepting silent regression, this continuation restored all nine:

1. `.github/ISSUE_TEMPLATE/bug_report.yml` — privacy-safe structured bug report;
2. `.github/workflows/dependency-review.yml` — Dependency Audit;
3. `.github/workflows/release-gate.yml` — production Release Gate;
4. `README.md` — highlighted BMC support presentation;
5. `SUPPORT.md` — highlighted BMC support presentation;
6. `docs/releases/BMC_HIGHLIGHT.md`;
7. `docs/releases/BMC_HIGHLIGHT_RELEASE_CHECK.md`;
8. `src/CareNest.App/Resources/Images/carenest_support.svg` — original compact CareNest support badge;
9. `src/CareNest.App/Views/AboutPage.xaml` — highlighted clickable in-app support card.

Restoration commits include:

- `1fd80561509524dd4dd6d25bc6a3658f3c681cd2` — privacy-safe bug report restoration;
- `2d55eadc86aee43a3c930044a4bb8d98e38e941b` — Dependency Audit restoration;
- `d39f74779d89747293d4a829d1c38299af865b8c` — Release Gate restoration;
- `bff9138503d1cdfd950dcf8beb98726cef35dc2d` — BMC README restoration;
- `6f699f1708a8a49cfc118a88b29fbf357b0b067b` — SUPPORT restoration;
- `ad0d92a7c62341503258cabc587ab96cb2a112d1` — compact support badge restoration;
- `c18a83f1b44cb3ef43b4559bd7abd09eb3f1415a` — highlighted About support-card restoration;
- `d5a4ae9cd01fc8d2ba0a6ee59f088058f6d6920a` — BMC highlight guide restoration;
- `df10abc5758490850d282ab3e085db90bcab0e26` — BMC release review restoration.

A subsequent compare no longer showed those historical files as missing. The current repository preserves the prior valid funding/dependency/release work plus the new hardening.

## 18. BMC/support current state

Current support URL:

`https://buymeacoffee.com/sanskarIN`

Support surfaces include:

- centralized `AppConstants.FundingUrl`;
- clickable compact `carenest_support.svg` in README;
- highlighted README text link;
- highlighted SUPPORT page;
- clickable `ImageButton` in the in-app About card;
- dedicated BMC button in About;
- explicit visible BMC URL in About;
- `.github/FUNDING.yml`;
- `BUY_ME_A_COFFEE.md` using the larger custom artwork;
- `docs/SUPPORT_CARENEST.md`;
- BMC highlight/release guidance.

The original CareNest support badge is not represented as an official Buy Me a Coffee trademark/logo asset. CareNest does not append health data to the support URL.

---

# Exact-head verification protocol used

Verification branches are short-lived and exist only to trigger PR workflows for an exact `main` source head.

Protocol followed:

1. finish intended source/test changes on `main`;
2. create a branch from the exact source SHA;
3. add one marker file under `build/verification/`;
4. open PR to `main`;
5. verify the PR diff is marker-only;
6. run CI/CodeQL/Dependency Audit;
7. if a real failure appears, fix `main`, close the stale marker PR without merge, and create a new exact-head marker PR;
8. once all gates pass, record evidence;
9. close the marker PR without merge.

This ensures marker files never become production source and prevents stale CI results from being represented as evidence for a newer head.

---

# 2026-08-10 verification sequence

## PR #24 — first hardening verification

Source head:

`47234b65c2060e0417a7e7cd6b005d286594df3a`

Verification marker head:

`45260933d286a60b6d0de66d9f0fddc225bbdf48`

Results:

- CodeQL #175: success;
- CareNest CI #175: failure.

Real findings:

- CA1873 eager exception-metadata argument evaluation in the new global logger path;
- CA1861 analyzer guidance in architecture test expectation arrays;
- source/test issues were corrected on `main`.

PR #24 was closed without merge as superseded.

## PR #25 — second hardening verification

Exact source head:

`15f3b94f9a5a02ffc5090290b7de06a5cd006996`

Verification marker head:

`450e83d38d9febfbd1d9988b33ed84467dc71737`

Results:

- Dependency Audit #5 / `31374433350`: success;
- CodeQL #190 / `31374433235`: success;
- platform-neutral formatting: success;
- unit tests: 15 passed;
- integration tests: 11 passed;
- UI-contract execution reached 46 tests: 41 passed, 5 failed;
- CareNest CI #190 / `31374433469`: failure.

The five UI/policy failures were actionable:

1. Domain dependency path comparison failed because Windows-style backslashes were not normalized on Linux;
2. Application dependency path comparison had the same problem;
3. Infrastructure dependency path comparison had the same problem;
4. logging privacy source policy discovered `StartupCoordinator` still passed a full exception object to the logger;
5. repository network/telemetry source scan included generated `obj/...GlobalUsings.g.cs`, creating a false positive for `System.Net.Http`.

MAUI compilation also confirmed CA1873 required explicit `ILogger.IsEnabled(...)` guards, not only precomputed type strings.

Every finding was fixed on `main`; PR #25 was closed without merge.

## PR #26 — third hardening verification

Exact source head:

`853d3d8254fd0b30a386a42d2d1fde316bc46a43`

Verification marker head:

`d7c8c19b014f3cfece50a88f6b1c616c6a9fe354`

Results:

- Dependency Audit #6 / `31374928518`: success;
- CodeQL #198 / `31374928520`: success;
- formatting: success;
- unit tests: 15 passed;
- integration tests: 11 passed;
- UI test project compile: one nullable contract error;
- CareNest CI #198 / `31374928536`: failure.

Remaining issue:

`Path.GetFileNameWithoutExtension` is nullable under the project’s nullable analysis and the architecture helper returned `string?[]` where `string[]` was required.

Fix:

- `8417513db36c72b0ec2cfaccadb6ac47ba361f11` — explicit non-null project-reference filename contract.

PR #26 was closed without merge.

## PR #27 — final exact-head hardening verification

Exact verified production source head:

`8417513db36c72b0ec2cfaccadb6ac47ba361f11`

Verification branch:

`ci/carenest-rc1-hardening-verification-20260810-4`

Verification marker head:

`aefd53869b7eaf54815de446fc83373c7977d04d`

Marker-only changed file:

`build/verification/rc1-hardening-20260810-4.txt`

PR:

`#27 — Verify CareNest exact-head hardening after CI fixes`

PR #27 was closed **without merge** after the full automated matrix succeeded.

### CareNest CI #200

Run ID:

`31375336226`

Final conclusion:

**success**

Core job evidence:

- platform-neutral formatting: passed;
- `CareNest.UnitTests`: **15 passed, 0 failed, 0 skipped**;
- `CareNest.IntegrationTests`: **11 passed, 0 failed, 0 skipped**;
- `CareNest.UiTests`: **46 passed, 0 failed, 0 skipped**;
- total automated test cases in core job: **72 passed, 0 failed, 0 skipped**.

Platform build evidence:

- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**.

### CodeQL #200

Run ID:

`31375336083`

Final conclusion:

**success**

### Dependency Audit #7

Run ID:

`31375336088`

Final conclusion:

**success**

Dependency Audit includes:

- platform-neutral NuGet audit restores;
- Android MAUI workload installation;
- MAUI app dependency graph audit;
- explicit documentation that GitHub Dependency Review action is not currently used because the repository Dependency Graph is not enabled.

### Final automated state

For exact source head `8417513...`:

- formatting green;
- 72/72 automated tests green;
- Android green;
- Windows green;
- iOS simulator green;
- Mac Catalyst green;
- CodeQL green;
- Dependency Audit green.

No final production tag or store release is claimed from this evidence alone.

---

# Current automated policy coverage

The `CareNest.UiTests` project now includes contracts/policies across these areas:

## Critical product flows

- onboarding safety/local-first text;
- profile deletion/export behavior contracts;
- medicine/schedule UI contracts;
- as-needed behavior;
- reminder/status terminology;
- About support/safety boundaries.

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
- explicit user-entered stock change.

## Privacy/security

- no full exception-object logging patterns;
- no common signing/private-key file types;
- no committed runtime telemetry/network client introduction;
- required privacy/security/release files exist;
- BMC link is fixed and explicit;
- no implicit notification permission request in onboarding.

## Engineering quality

- no committed runtime TODO/FIXME/NotImplemented placeholders;
- no common synchronous task-blocking patterns;
- branding SVGs remain well-formed;
- resource keys remain present;
- architecture references remain portable across operating systems.

These tests are preventive controls. They do not replace manual application behavior, security, accessibility, store-policy, or device testing.

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
- idempotent attachment through `Interlocked.Exchange`;
- observes unhandled application-domain exceptions;
- observes unobserved task exceptions;
- logs only type/category metadata;
- uses explicit enabled-level guards;
- sets unobserved task exceptions observed after safe handling.

## ReminderCoordinator

- reminder scheduling failures no longer log occurrence IDs or medicine IDs;
- low-stock reminder scheduling failures no longer expose health-record identifiers;
- full exceptions/messages/stack traces are not passed;
- safe exception type metadata is evaluated only when Warning logging is enabled.

## StartupCoordinator

- non-fatal reminder recovery no longer passes a full exception object;
- logs only safe exception type metadata when Warning is enabled.

## Documentation

`docs/security/LOGGING_PRIVACY.md` explicitly prohibits logging medicine/profile data, appointment notes, document data/paths, reminder notes, PIN/backup secrets, keys, raw health-record identifiers, full exception objects/messages/stack traces, and URLs containing user data.

---

# Release-engineering state

## CI

`.github/workflows/ci.yml` currently verifies:

- formatting of platform-neutral projects/tests;
- unit tests;
- integration tests;
- UI-contract/policy tests;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build.

## CodeQL

CodeQL remains an independent security-analysis workflow and passed on final exact-head verification.

## Dependency Audit

`.github/workflows/dependency-review.yml` is restored and green. It audits platform-neutral and Android MAUI dependency graphs using NuGet audit mode.

## Production Release Gate

`.github/workflows/release-gate.yml` is restored. It intentionally blocks public release while:

- the dependency risk register contains an open production risk;
- release checklist items remain incomplete;
- required release documents are missing;
- source tests fail.

It is acceptable and intentional for this gate to block final `1.0.0` today because manual/store/signing/SQLite decisions are still incomplete.

## Release Evidence

`.github/workflows/release-evidence.yml` can be manually or tag triggered to produce source/toolchain/test/dependency/checksum evidence for the eventual promoted commit.

It is not a substitute for platform CI or manual target testing.

---

# Security model implemented

- Imported documents/profile photos use encrypted `.cndoc` storage.
- Document encryption uses authenticated AES-256-GCM primitives rather than custom cryptography.
- Backups use password-derived authenticated encryption and a schema-versioned format.
- The encrypted backup payload carries document key material needed for portable restore without storing it in plaintext.
- App-lock PINs are not stored directly; a salted password-derived verifier is stored through secure platform storage.
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

# Reminder reliability rules implemented

- schedules originate only from explicit user input;
- reminder occurrence keys are stable/deterministic;
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
- stock changes after Taken use only user-configured quantity change;
- stock estimates explicitly warn users to check actual supply;
- reminder scheduling failures are now privacy-redacted in logs.

---

# Acceptance-criteria mapping

- No account/network required: implemented.
- No CareNest backend/login/cloud sync: implemented for v1.
- No diagnosis/treatment/dosage decisions: enforced through scope, domain behavior, UI text, policy tests, and documentation.
- Reminder recovery: startup rebuild plus Android boot/time/time-zone integration implemented.
- Permission/battery limitations: surfaced.
- Profile export/delete: implemented.
- Document export/delete: implemented.
- Logs exclude health-document contents and raw sensitive exception details: implemented and contract tested.
- Medical limitations in onboarding/About: implemented.
- Manual encrypted backup/restore: implemented with version/integrity validation and rollback handling.
- No automatic cloud upload: implemented by architecture.
- Local caregiver mode: implemented without silent sharing.
- Theme/accessibility/localization readiness: implemented.
- Automated quality gate: exact hardening source head passed formatting/core tests/Android/Windows/iOS/Mac/CodeQL/Dependency Audit.
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
4. Manually verify document import/export/delete.
5. Manually verify calendar export.
6. Manually verify encrypted backup/restore on clean installation/release build.
7. Manually verify cold-start app lock.
8. Complete screen-reader, large-text, keyboard, contrast, and reduced-motion accessibility review.
9. Review current Apple App Store and Google Play policy for the external voluntary support link.
10. Conditionally remove/hide the in-app external support action for a distribution channel if current rules require it.
11. Prepare signing identities/credentials outside Git.
12. Build/inspect signed packages on fully provisioned hosts.
13. Complete store screenshots, listing text, privacy/data-safety disclosures, support/privacy/terms/security links, and package identity checks.
14. Resolve or make an explicit production release decision for the open SQLitePCLRaw advisory.
15. Run `CareNest Release Evidence` for the exact commit ultimately promoted to public `1.0.0`.
16. Create final release notes/tag/GitHub release only after all applicable gates are complete.

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

The connected GitHub write API used in this chat does not expose author/committer email parameters for file update/create operations. Connector-created commits therefore use the authenticated GitHub identity. This repository does not falsely claim the connector forced `sanskarin@outlook.in` into those commit objects.

Local/future maintainer commits can use the requested address through the provided setup scripts.

---

# Local environment limitation

The local execution container used in this conversation does not provide a full .NET MAUI development host with all required workloads/device tooling. Therefore the repository does not claim local MAUI restore/build/emulator/device/signing/store-package execution from that container.

GitHub-hosted Actions supplied the exact automated evidence recorded above.

Manual target-device tests, signing identities, current store-policy decisions, and store submissions require external provisioned environments/credentials and remain explicitly open.

---

# Documentation-only commits after verified source head

Exact verified runtime/test source head:

`8417513db36c72b0ec2cfaccadb6ac47ba361f11`

After PR #27 completed green, the following documentation-only commits recorded the evidence without changing runtime/test/product source:

- `fa8b78cb6ffbb41242cbc1d95f7fd696a8303fd5` — `docs: record green exact-head rc1 hardening verification` (`PROJECT_STATUS.md`);
- `212f92b3dbe07e2d8d5bce63af0d1c14059237ce` — `docs: record green rc1 hardening release evidence` (`RELEASE_CHECKLIST.md`);
- `d682e2e58c06c6e4c47c875a892f35a1353fb569` — `docs: record rc1 privacy and release hardening changes` (`CHANGELOG.md`);
- `a02fca94b98657bf6010d76821cd3eab0b9db408` — `docs: link verified CareNest quality and release evidence` (`README.md`).

These commits do not alter the exact runtime/test source that passed CI #200. They are not represented as if each had a separate cross-platform build.

---

# Current repository state

- Complete CareNest `1.0.0-rc.1` source remains on `main`.
- Exact verified runtime/test source head is `8417513db36c72b0ec2cfaccadb6ac47ba361f11`.
- Final exact-head verification PR #27 is closed without merge.
- CareNest CI #200 / `31375336226`: success.
- CodeQL #200 / `31375336083`: success.
- Dependency Audit #7 / `31375336088`: success.
- Platform-neutral formatting: success.
- Unit tests: 15/15 passed.
- Integration tests: 11/11 passed.
- UI-contract/policy tests: 46/46 passed.
- Total core test cases: 72/72 passed.
- Android Release: success.
- Windows Release: success.
- iOS simulator Release: success.
- Mac Catalyst Release: success.
- Global/UI/startup/reminder exception logging is privacy-redacted and analyzer-compliant.
- Repository policy, architecture, ViewModel, data-model, branding, async, and logging privacy contracts are active.
- Restored Dependency Audit and Release Gate workflows are present.
- Release Evidence workflow and release-quality/security/provenance documentation are present.
- Previously missing valid BMC/release/dependency files discovered during recovery audit have been restored.
- README/SUPPORT/About funding surfaces and both CareNest support artwork variants are present.
- The SQLitePCLRaw advisory remains explicitly open and is not claimed fixed.
- Final public `1.0.0` tagging/publication remains gated on manual device/accessibility testing, current store-policy review, signing/store preparation, Release Evidence generation for the eventual promoted commit, and the SQLite dependency-risk decision/resolution.
- Cloud sync, remote caregiver collaboration, accounts/server storage, diagnosis, treatment advice, dosage calculation/inference, interaction checking, and clinical risk scoring remain deferred/out of scope.
