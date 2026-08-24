# CareNest — Current Project Status

**Date:** 2026-08-23  
**Release line:** `1.0.0-rc.1`  
**Repository:** `https://github.com/sanskarIN/CareNest`  
**Current automated evidence authority:** `docs/releases/AUTOMATED_BASELINE.md`  
**Latest accepted automated source:** `30ee6c265104c64ec5a1a4013f592f7f058750e8`  
**Current verification-relevant continuation:** PR `#84`, `continue/cross-platform-current-main-20260823`

The complete project status that was active immediately before this cross-platform current-main continuation is preserved exactly at:

`docs/history/cross-platform-before-current-status-20260823/PROJECT_STATUS.md`

Historical verification records remain authoritative only for their exact source boundaries. The current dynamic automated authority remains:

`docs/releases/AUTOMATED_BASELINE.md`

The accepted automated baseline is still the PR #81 backup-resource-hardening source until a newer exact source is fully verified and explicitly promoted. No older test count or workflow result is copied onto PR #84 merely because the branch contains later source.

---

## 1. Current product boundary

CareNest is a local-first organizational health application using two presentation-host families over the same .NET 10 codebase:

- the established .NET MAUI application for Android, iOS/iPadOS, Mac Catalyst and Windows;
- an Avalonia presentation layer with a desktop host for Linux-capable desktop execution and a WebAssembly browser host.

The Linux/browser work establishes configured build and presentation reach. It does **not** by itself establish full production feature parity with the established MAUI application.

CareNest does **not**:

- diagnose conditions;
- calculate or infer medicine dosage;
- recommend treatment;
- perform clinical medication-interaction checking;
- calculate clinical risk scores;
- independently prove adherence;
- replace clinicians or pharmacists;
- provide emergency services;
- guarantee operating-system or browser notification delivery.

The current release remains account-free and local-first. It does not require a CareNest cloud backend and does not silently upload local health records.

---

## 2. Current configured platform reach

### Established MAUI targets

- Android: `net10.0-android`;
- iOS/iPadOS: `net10.0-ios`;
- Mac Catalyst: `net10.0-maccatalyst`;
- Windows: `net10.0-windows10.0.19041.0`.

### Cross-platform presentation hosts in PR #84

- Linux desktop: `CareNest.CrossPlatform.Desktop`, targeting `net10.0` through Avalonia Desktop;
- modern WebAssembly-capable browsers: `CareNest.CrossPlatform.Browser`, targeting `net10.0-browser` through Avalonia Browser;
- shared Avalonia application/views: `CareNest.CrossPlatform`.

The Avalonia desktop host can also execute on supported Windows/macOS environments, but it does not replace the established MAUI release boundary for those platforms.

Source setup and architecture guide:

`docs/setup/CROSS_PLATFORM.md`

---

## 3. Current implementation state

The source-controlled `1.0.0-rc.1` MAUI runtime scope remains source-complete for the intended established RC feature set, including:

- multiple local person/family profiles;
- medicine records with user-entered strength/instruction text;
- explicit schedules and deterministic reminder occurrences;
- reminder lifecycle/history/status/reconciliation/compensation behavior;
- appointments and optional reminders;
- stock/refill organization;
- encrypted imported-document vault;
- password-encrypted manual backup/restore;
- bounded authenticated backup archive/decrypted-container processing;
- optional local app lock;
- reports and explicit exports;
- privacy-aware diagnostics;
- light/dark/system themes;
- accessibility-oriented source contracts;
- strict compiled MAUI XAML bindings;
- automated C#/structured-file quality contracts;
- documentation-integrity tooling;
- package-evidence/provenance tooling;
- CodeQL, dependency, store and release gates.

PR #84 adds the cross-platform presentation/build foundation rather than falsely cloning MAUI behavior behind unsupported APIs. Linux/browser implementations must preserve explicit capability semantics for persistence, reminders/background execution, secure storage, file/camera integration, sharing, accessibility and packaging.

---

## 4. Cross-platform source added in PR #84

Current branch source includes:

- `src/CareNest.CrossPlatform/CareNest.CrossPlatform.csproj`;
- `src/CareNest.CrossPlatform/App.axaml`;
- `src/CareNest.CrossPlatform/App.axaml.cs`;
- `src/CareNest.CrossPlatform/Views/MainView.axaml`;
- `src/CareNest.CrossPlatform/Views/MainView.axaml.cs`;
- `src/CareNest.CrossPlatform.Desktop/CareNest.CrossPlatform.Desktop.csproj`;
- `src/CareNest.CrossPlatform.Desktop/Program.cs`;
- `src/CareNest.CrossPlatform.Browser/CareNest.CrossPlatform.Browser.csproj`;
- `src/CareNest.CrossPlatform.Browser/Program.cs`;
- browser bootstrap assets under `src/CareNest.CrossPlatform.Browser/wwwroot/`.

`CareNest.sln` registers all three cross-platform projects.

Central package management includes Avalonia, Avalonia Desktop, Avalonia Browser, Fluent theme and Inter font packages at the branch's configured version.

---

## 5. Fail-closed cross-platform configuration verification

PR #84 adds:

- `build/scripts/verify-cross-platform-targets.py`;
- `build/scripts/test-verify-cross-platform-targets.py`.

The verifier checks required:

- MAUI target declarations;
- Avalonia package declarations;
- desktop/browser project targets;
- desktop and browser lifetime/bootstrap wiring;
- solution registration;
- well-formed Avalonia XAML;
- CI and dependency-audit wiring;
- release-gate wiring;
- public README cross-platform claims;
- cross-platform setup documentation;
- Linux/browser production-evidence records and fail-closed defaults.

The self-test uses isolated temporary fixtures and requires failure for intentionally broken desktop startup wiring, malformed Avalonia XAML and an unsafe pre-completed browser evidence template.

This provides executable regression protection rather than relying only on prose documentation.

---

## 6. Continuous-integration state for PR #84

The branch configuration extends CareNest CI with:

- cross-platform verifier syntax validation;
- direct cross-platform target verification;
- verifier regression self-tests;
- shared Avalonia/desktop formatting checks;
- Linux desktop Release build;
- WebAssembly workload installation;
- Avalonia browser Release publish.

Existing jobs remain for:

- unit tests;
- integration tests;
- UI/source-policy tests;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build.

Dependency Audit additionally restores/audits the Avalonia desktop and browser graphs.

The tagged Release Gate additionally builds the Linux desktop host and publishes the browser host.

A green source build proves only the configured automated source boundary for that exact commit. It is not manual production evidence for Linux/browser feature parity.

---

## 7. Linux/browser production evidence system

PR #84 extends the production-evidence system with canonical templates:

- `docs/releases/templates/LINUX_DESKTOP_VALIDATION_RECORD.md`;
- `docs/releases/templates/BROWSER_VALIDATION_RECORD.md`.

Both canonical templates intentionally start `NOT RUN`.

The Linux template separates build/launch evidence from platform-specific persistence, notifications/background execution, secret storage, filesystem behavior, accessibility and full feature-parity evidence.

The browser template separates WebAssembly publish/startup evidence from browser persistence/storage, notifications/background execution, file/camera permissions, reload/offline/multiple-tab behavior, accessibility and full feature-parity evidence.

`docs/releases/PRODUCTION_EVIDENCE_INDEX.md` links both templates and explicitly states that a Linux build or WebAssembly publish is not manual production evidence.

`.github/workflows/release-gate.yml` requires both templates to exist.

`tests/CareNest.UiTests/ProductionEvidenceDocumentationContractTests.cs` now includes both templates in the canonical unperformed-template/index contract.

`tests/CareNest.UiTests/CrossPlatformEvidenceContractTests.cs` adds cross-platform-specific evidence/parity contracts.

---

## 8. Production validation result semantics

Canonical authority:

`docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`

Allowed result states remain:

- `PASS`;
- `FAIL`;
- `BLOCKED`;
- `N/A`;
- `NOT RUN`.

Unknown, stale, queued, superseded, blocked or unperformed work must never be represented as a pass.

Canonical templates are evidence containers, not evidence themselves. Create release-specific copies only when actual validation occurs.

Public evidence must use fictional/synthetic application data and must not contain real health records, prescription documents, PINs, backup passwords, private signing keys, access tokens, recovery codes or other secrets.

---

## 9. Why PR #83 is superseded

The earlier cross-platform PR #83 had workflow success for its own older head, but its feature branch diverged after PR #82 merged into `main`.

The replacement work was therefore rebuilt directly from current `main` as PR #84 instead of treating stale-base success as valid for an unverified merge result.

PR #84 preserves the merged PR #82 production-evidence release-gate requirements while reconstructing the useful Linux/browser implementation on top of that source.

PR #83 should remain historical/superseded rather than becoming the preferred merge path after PR #84 completes verification.

---

## 10. Current accepted automated baseline

Canonical file:

`docs/releases/AUTOMATED_BASELINE.md`

Current accepted exact verified source:

`30ee6c265104c64ec5a1a4013f592f7f058750e8`

Accepted baseline results remain:

- unit tests: **122/122**;
- integration tests: **54/54**;
- UI/source-policy tests: **194/194**;
- total core tests: **370/370**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- Store Package Configuration: success for its recorded targets;
- Store Inspection Artifacts: success for its recorded artifacts;
- CodeQL: success;
- Dependency Audit: success.

Those results belong only to the exact source recorded in `AUTOMATED_BASELINE.md`. They do not prove PR #84.

PR #84 must complete a fresh exact-head required matrix before its source can replace that accepted boundary.

---

## 11. Backup resource-hardening state

The accepted baseline continues to protect authenticated backup handling with bounded resource/topology rules before manifest parsing/extraction and during decrypted-container creation.

Current documented defaults include:

- decrypted ZIP container: 2304 MiB maximum;
- manifest: 1 MiB maximum;
- SQLite database: 1 GiB maximum;
- each encrypted document: 512 MiB maximum;
- total uncompressed ZIP payload: 2 GiB maximum;
- document count: 5,000 maximum;
- bounded archive-entry count;
- explicit directory-only ZIP entries rejected.

Cross-platform presentation work must not weaken these domain/infrastructure safety boundaries.

---

## 12. Repository/open-source completeness

The repository retains the expected public/community surfaces, including:

- Apache License 2.0 `LICENSE`;
- `NOTICE`;
- `README.md`;
- `CHANGELOG.md`;
- `CODE_OF_CONDUCT.md`;
- `CONTRIBUTING.md`;
- `SECURITY.md`;
- privacy, terms and support documentation;
- `.github/CODEOWNERS`;
- Dependabot configuration;
- issue forms;
- pull-request template;
- funding metadata;
- CI, CodeQL, Dependency Audit, Store Package Configuration, Store Inspection Artifacts and Release Gate workflows;
- setup, architecture, testing, security, privacy, accessibility, localization, packaging, release and governance documentation;
- production evidence standard/index/templates;
- Linux/browser production validation templates;
- source-policy regression tests protecting release-evidence and cross-platform documentation.

The continuation does not add decorative files merely to increase file count where a maintained equivalent already exists.

---

## 13. Gumroad/external-commerce package boundary

The canonical repository storefront remains:

`https://ramsandesh.gumroad.com`

Repository support/storefront promotion remains separate from CareNest health functionality.

The distributed CareNest application/source package must not gain external-commerce behavior that changes health functionality, reminder priority/reliability, medical advice, clinical services or access to local health records.

Cross-platform hosts must preserve the same package boundary rather than introducing repository-only promotion into Linux/browser runtime payloads.

---

## 14. Remaining release work

The major remaining work is evidence-driven rather than speculative feature expansion:

1. complete PR #84 exact-head automated verification;
2. fix any real failures exposed by that matrix;
3. merge only after required current-head checks succeed;
4. keep PR #83 superseded after PR #84 replaces it;
5. promote a newer automated baseline only from actually observed exact-source results;
6. perform applicable Android/Windows/iOS/Mac Catalyst real-package/device validation;
7. perform applicable Linux desktop validation using a release-specific Linux record;
8. perform applicable browser validation using release-specific browser records for actually tested browsers;
9. perform accessibility validation with real assistive technology;
10. perform packaged existing-data/document/backup compatibility validation;
11. complete production signing/notarization/provenance as applicable;
12. perform final package inspection and structured package evidence;
13. complete live store declarations/policy review/submission where applicable;
14. freeze the exact production source/tag only after applicable blockers are resolved.

A build, simulator compile, browser publish, canonical template, queued workflow or stale workflow is not a substitute for these evidence rows.

---

## 15. Current governance rule

CareNest remains `1.0.0-rc.1` until applicable production rows are actually evidenced.

Do not add speculative medical/runtime features merely to increase commit count. Continue with concrete defects, cross-platform capability implementations backed by real requirements, regression protection, documentation correctness and release evidence.
