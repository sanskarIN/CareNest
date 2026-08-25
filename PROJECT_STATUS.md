# CareNest — Current Project Status

**Date:** 2026-08-24  
**Release line:** `2.18.12`  
**Release state:** PREPARED IN SOURCE — NOT PUBLISHED  
**Repository:** `https://github.com/sanskarIN/CareNest`  
**Current automated evidence authority:** `docs/releases/AUTOMATED_BASELINE.md`  
**Latest accepted verified branch source:** `1d9de89fbc7de69696c9d4276991f07bcdce1027`  
**Verified PR merge ref:** `0a579f2a1d927173f3c69e8b32d0ac52ced6c944`  
**Merged `main` commit:** `ca80bd554296363d71a6008cac73c819be77b39b`

The project-status snapshot that preceded the cross-platform current-main continuation remains preserved under:

`docs/history/cross-platform-before-current-status-20260823/PROJECT_STATUS.md`

Historical evidence is authoritative only for its own exact source. The current accepted automated result is recorded in `docs/releases/AUTOMATED_BASELINE.md` and must not be confused with production-device, signing, store-approval or publication evidence.

---

## 1. Current product boundary

CareNest is a local-first organizational health application. It currently uses two presentation-host families over a .NET 10 codebase:

- .NET MAUI for Android, iOS/iPadOS, Mac Catalyst and Windows;
- Avalonia for a Linux-capable desktop host and a WebAssembly browser host.

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

The current source remains account-free and local-first within the documented product scope. It does not require a CareNest cloud backend and does not silently upload local health records.

---

## 2. Current version and dependency baseline

CareNest source is prepared for version `2.18.12`:

- central semantic version: `2.18.12`;
- assembly/file version: `2.18.12.0`;
- MAUI application display version: `2.18.12`;
- MAUI application package/build code: `21812`;
- `Microsoft.Maui.Controls`: `10.0.100`;
- Avalonia package family: `12.1.1`.

`tests/CareNest.UiTests/VersionConsistencyContractTests.cs` protects the source/package version, MAUI dependency baseline and release-document non-publication state from accidental drift.

Preparation of these values does not mean `2.18.12` has been published.

---

## 3. Configured platform reach

### Established MAUI targets

- Android: `net10.0-android`;
- iOS/iPadOS: `net10.0-ios`;
- Mac Catalyst: `net10.0-maccatalyst`;
- Windows: `net10.0-windows10.0.19041.0`.

### Avalonia presentation/build hosts

- Linux-capable desktop: `CareNest.CrossPlatform.Desktop`, targeting `net10.0`;
- modern WebAssembly-capable browsers: `CareNest.CrossPlatform.Browser`, targeting `net10.0-browser`;
- shared Avalonia application/views: `CareNest.CrossPlatform`.

The Avalonia desktop host can also execute on supported Windows/macOS environments, but it does not replace the established MAUI production boundary on those platforms.

Configured build/presentation reach is **not** the same as production feature parity. Linux and browser runtime capabilities remain evidence-driven and must be represented as `NOT RUN`, `BLOCKED`, `N/A`, `FAIL` or `PASS` according to actual validation.

Architecture and capability guide:

`docs/setup/CROSS_PLATFORM.md`

---

## 4. Established source-complete application scope

The current source retains the intended non-clinical CareNest organizer scope, including:

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

No speculative clinical or emergency feature was added merely to expand scope.

---

## 5. Cross-platform foundation now merged

PR #84 was merged into `main` at:

`ca80bd554296363d71a6008cac73c819be77b39b`

The merged source includes:

- `src/CareNest.CrossPlatform/CareNest.CrossPlatform.csproj`;
- shared Avalonia application and views;
- `src/CareNest.CrossPlatform.Desktop/CareNest.CrossPlatform.Desktop.csproj` and desktop startup;
- `src/CareNest.CrossPlatform.Browser/CareNest.CrossPlatform.Browser.csproj` and browser startup;
- browser bootstrap assets under `src/CareNest.CrossPlatform.Browser/wwwroot/`;
- solution registration for all three cross-platform projects;
- Linux desktop and browser CI paths;
- Avalonia dependency-audit paths;
- tagged release-gate Linux/browser build/publish paths;
- Linux and browser canonical production-validation templates.

The older PR #83 was closed as superseded. Dependabot PR #85 was also closed after its `Microsoft.Maui.Controls` `10.0.100` update was integrated and verified through PR #84.

---

## 6. Fail-closed cross-platform verification

The merged source includes:

- `build/scripts/verify-cross-platform-targets.py`;
- `build/scripts/test-verify-cross-platform-targets.py`.

The verifier checks required:

- MAUI target declarations;
- Avalonia package declarations;
- desktop/browser project targets;
- application lifetime/bootstrap wiring;
- solution registration;
- well-formed Avalonia XAML;
- CI and dependency-audit wiring;
- release-gate wiring;
- public README platform claims;
- cross-platform setup documentation;
- Linux/browser production-evidence records and fail-closed defaults.

The isolated regression self-tests require intentional failures to be detected, including missing startup wiring, malformed Avalonia XAML and unsafe pre-completed production-evidence state.

---

## 7. Current accepted automated evidence

Canonical details are in:

`docs/releases/AUTOMATED_BASELINE.md`

The exact PR #84 branch source `1d9de89fbc7de69696c9d4276991f07bcdce1027`, evaluated through merge ref `0a579f2a1d927173f3c69e8b32d0ac52ced6c944`, produced:

- repository Python tooling syntax: **success**;
- cross-platform target verification: **success**;
- cross-platform verifier self-tests: **success**;
- package-evidence self-test: **success**;
- documentation-link checker self-test: **success**;
- active stable documentation links: **210** live local links across **128** stable active Markdown files;
- platform-neutral formatting: **success**;
- unit tests: **122/122**;
- integration tests: **54/54**;
- UI/source-policy tests: **215/215**;
- total core tests: **391/391**;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- Linux desktop Release build: **success**;
- WebAssembly browser Release publish: **success**;
- Store Package Configuration: **success**;
- Store Inspection Artifacts: **success**;
- CodeQL: **success**;
- unsuppressed Dependency Audit: **success**.

CareNest CI run `32685906690` initially encountered a transient HTTP `ResponseEnded` failure while downloading the Windows MAUI workload. The Windows build had not started. A job-only retry on the unchanged exact source succeeded in workload installation and the Windows Release build. The final CI conclusion is success; the initial failure remains recorded in the automated baseline rather than being hidden.

---

## 8. Production evidence semantics

Canonical authority:

`docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`

Allowed evidence states remain:

- `PASS`;
- `FAIL`;
- `BLOCKED`;
- `N/A`;
- `NOT RUN`.

Unknown, stale, queued, superseded, blocked or unperformed work must never be represented as a pass.

Canonical templates are evidence containers, not evidence themselves. Release-specific copies should be completed only when actual validation occurs.

Public evidence must use fictional/synthetic application data and must not contain real health records, prescription documents, PINs, backup passwords, private signing keys, access tokens, recovery codes or other secrets.

---

## 9. Backup security/resource boundary retained

The verified 2.18.12 source retains the accepted authenticated-backup resource limits:

- decrypted ZIP container: **2304 MiB** maximum;
- manifest: **1 MiB** maximum;
- SQLite database: **1 GiB** maximum;
- each encrypted document: **512 MiB** maximum;
- total uncompressed ZIP payload: **2 GiB** maximum;
- documents: **5,000** maximum;
- archive-entry count: document ceiling plus required fixed entries;
- explicit directory-only ZIP entries: rejected.

Backup creation validates generated archives against the same topology/resource boundary before encryption. These automated protections do not replace packaged historical-backup compatibility validation.

---

## 10. External-commerce/package boundary retained

CareNest keeps funding/storefront promotion outside the shipped health application package.

The release governance continues to require final distributed app payloads to avoid in-app promotion/purchase surfaces for:

- `buymeacoffee.com/sanskarIN`;
- `ramsandesh.gumroad.com`.

Final package inspection must retain the applicable store-safe marker checks. No health feature or local-data access may depend on purchase/funding state.

---

## 11. Remaining production work

The repository source and automated merge-readiness work are complete for the current prepared baseline. Remaining blockers require genuine external/manual evidence rather than more speculative source churn.

### Android

- representative installed-device validation;
- notification permission denied/granted behavior;
- actual reminder delivery/actions/snooze/cancellation;
- exact/inexact alarm and battery/vendor behavior;
- reboot/restart/clock/time-zone/DST recovery;
- force-stop limitation/recovery messaging;
- documents/share/backup/app-lock/accessibility validation.

### Windows

- actual intended installed-package/update path;
- running/closed-app reminder behavior and limitations;
- reminder replacement/cancellation/actions/snooze/recovery;
- documents/share/backup/app-lock;
- keyboard/focus/theme/Narrator/large-text validation;
- existing-data packaged upgrade validation.

### iPhone/iPad

- real signed/provisioned device install/upgrade;
- notification permission and actual delivery/actions/snooze behavior;
- lifecycle/restart/time-zone behavior;
- documents/share/backup/app-lock;
- Dynamic Type/VoiceOver/notification-preview privacy;
- packaged existing-data behavior where applicable.

Simulator compilation is not real-device notification evidence.

### Mac Catalyst

- installed/manual behavior;
- notification permission/delivery/actions/snooze;
- lifecycle/restart/time-zone behavior;
- file picker/share/backup/app-lock;
- keyboard/focus/VoiceOver/large-text/theme/contrast;
- signed/notarized candidate behavior where applicable.

### Linux desktop

- representative distribution/runtime validation;
- launch/window lifecycle, scaling and X11/Wayland boundaries where represented;
- filesystem/package prerequisites;
- persistence/reminder/secure-storage/file/share capability evidence only where implemented;
- keyboard/focus/assistive-technology checks;
- explicit non-parity state for unsupported capabilities.

### Browser/WebAssembly

- actual hosted startup/static-asset/WebAssembly behavior;
- browser storage/persistence/quota/private-mode boundaries where implemented;
- reload/navigation/offline/multiple-tab behavior;
- file/camera/notification permissions only where implemented;
- unsupported-capability behavior;
- screen-reader/focus/zoom checks;
- verification that no hidden analytics/telemetry/network upload was added.

### Cross-platform production gates

- packaged SQLite/encrypted-document/backup compatibility;
- genuine historical-backup compatibility where genuine prior bytes exist;
- accessibility validation with applicable assistive technologies;
- production signing/provisioning/notarization provenance;
- exact final package/deployment SHA-256/provenance;
- store-safe final payload inspection;
- live store metadata/declaration review;
- submission-day policy review;
- actual submission/review/approval/publication outcomes.

---

## 12. Release decision

CareNest `2.18.12` is **prepared and fully accepted at the current automated source boundary**, but it is **not production published**.

Do not create or treat `v2.18.12` as an approved production tag until applicable production evidence is complete and the tagged release gates permit promotion.

Do not claim:

- production signing without signing evidence;
- real-device behavior from simulator/build results;
- Linux/browser full feature parity from successful builds;
- store approval before actual approval;
- publication before actual publication;
- a global bug-free guarantee.

The next actionable work is maintained in `docs/releases/NEXT_STEPS.md`.