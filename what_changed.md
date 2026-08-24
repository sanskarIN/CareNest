# CareNest — 2.18.12 Preparation Handoff

**Date:** 2026-08-24  
**Target version:** `2.18.12`  
**Package/build code:** `21812`  
**Release state:** PREPARED IN SOURCE — NOT PUBLISHED  
**Repository:** `https://github.com/sanskarIN/CareNest`  
**Continuation branch:** `continue/cross-platform-current-main-20260823`  
**Pull request:** `#84` — `feat: complete cross-platform hosts on current main`

The earlier cross-platform handoff remains available in Git history at the pre-preparation boundary and is referenced under:

`docs/history/version-2.18.12-before-preparation-20260824/`

This active handoff records only the current continuation state. Historical workflow results remain valid only for their exact source commits.

---

## 1. CI formatting blocker corrected

The latest previously observed CareNest CI failure on PR #84 was caused by missing final newlines in three UI/source-policy test files. No runtime failure was identified by that run; formatting stopped the core-test job before the .NET test stages executed.

Corrected as separate commits:

- `4933d6e8e6216c10e78510622644117f469e9e38` — `style: add final newline to cross-platform evidence tests`;
- `3964b4381a27bf08b772b450052c8a2a8ee4fb7b` — `style: add final newline to production evidence tests`;
- `84404b75e0a3a552fe897ea47df83882ce5cd89f` — `style: add final newline to release documentation tests`.

The affected files are:

- `tests/CareNest.UiTests/CrossPlatformEvidenceContractTests.cs`;
- `tests/CareNest.UiTests/ProductionEvidenceDocumentationContractTests.cs`;
- `tests/CareNest.UiTests/ReleaseDocumentationConsistencyContractTests.cs`.

A fresh exact-head verification result is still required after all current version-preparation changes. Older green jobs must not be reused as proof for the newer head.

---

## 2. CareNest 2.18.12 central version baseline

Commit:

`e8ea101765a314f8b39ca09e77d3a17725c2256c` — `build: set CareNest 2.18.12 assembly version baseline`

`Directory.Build.props` now defines:

- `Version`: `2.18.12`;
- `AssemblyVersion`: `2.18.12.0`;
- `FileVersion`: `2.18.12.0`;
- `InformationalVersion`: `2.18.12`.

The central version applies consistently across the source projects that consume the repository build properties.

---

## 3. MAUI package metadata prepared

Commit:

`99ceec91b81d64234973d9cec57328fd506eee1f` — `build: prepare MAUI package metadata for 2.18.12`

`src/CareNest.App/CareNest.App.csproj` now uses:

- `ApplicationDisplayVersion`: `2.18.12`;
- `ApplicationVersion`: `21812`.

This prepares package metadata only. It is not evidence that Android, Apple or Windows stores have received, approved or published the build.

---

## 4. Version consistency regression protection

Commit:

`c87559feda5339faff1bd5c64a86aa83351f8fab` — `test: guard CareNest 2.18.12 version consistency`

Added:

`tests/CareNest.UiTests/VersionConsistencyContractTests.cs`

The contract verifies that:

- the central semantic version is `2.18.12`;
- central assembly/file versions are `2.18.12.0`;
- MAUI display version is `2.18.12`;
- MAUI package/build code is `21812`.

Future version changes must intentionally update this contract instead of silently leaving package metadata inconsistent.

---

## 5. Dedicated 2.18.12 release-preparation record

Commit:

`965c6177a61596130f7e343bdd19e655408076b4` — `docs: add CareNest 2.18.12 release preparation record`

Added:

`docs/releases/VERSION_2_18_12_PREPARATION.md`

The record defines:

- the exact target version/build code;
- configured Android, iOS/iPadOS, Mac Catalyst, Windows, Linux and browser build reach;
- the automated verification categories that must pass on the exact final head;
- the real-device, accessibility, package-compatibility, signing, provenance and store evidence still required;
- the rule that source preparation must not be described as publication or store approval.

---

## 6. Cross-platform implementation retained

PR #84 continues to contain the previously completed cross-platform foundation:

- established .NET MAUI hosts for Android, iOS/iPadOS, Mac Catalyst and Windows;
- shared Avalonia presentation project;
- Linux-capable Avalonia desktop host;
- Avalonia WebAssembly/browser host;
- browser bootstrap assets;
- solution registration;
- fail-closed cross-platform target verifier;
- verifier regression self-tests;
- Linux/browser CI builds;
- Avalonia dependency auditing;
- tagged-release Linux/browser build gates;
- Linux and browser production-evidence templates;
- cross-platform setup/capability documentation.

Configured build support does not imply full production feature parity. Platform-specific persistence, notifications/background execution, secure storage, file/camera integration, sharing, accessibility, package behavior and store approval require actual evidence.

---

## 7. Current required automated verification

The final exact PR #84 head must pass the current repository matrix before merge or release promotion:

- formatting;
- version-consistency contract;
- unit tests;
- integration tests;
- UI/source-policy tests;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build;
- Linux desktop Release build;
- WebAssembly browser Release publish;
- CodeQL;
- unsuppressed Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts.

Queued, failed, cancelled, skipped, superseded and older-head results are not exact-head success evidence.

---

## 8. Remaining repository workflow after PR #84 verification

After PR #84 has a complete exact-head green matrix:

1. merge PR #84 into `main` without discarding its granular commit history;
2. retire/supersede stale PR #83;
3. rebase and validate Dependabot PR #85 (`Microsoft.Maui.Controls` `10.0.100`) against the new `main`;
4. merge dependency updates only after the full applicable MAUI verification matrix passes;
5. update the dynamic automated baseline only from actually observed exact-source results;
6. prepare immutable `v2.18.12` tagging only after automated and required production evidence is complete;
7. generate final package checksum/provenance evidence from the exact production artifacts;
8. complete signing, notarization/provisioning and live store metadata/declaration review;
9. record submission/review/approval/publication outcomes separately.

---

## 9. Real production validation still open

Source preparation does not replace actual platform validation. The remaining production-evidence work includes, where applicable:

### Android

- representative installed-device validation;
- notification permission denied/granted behavior;
- reminder delivery, cancellation, snooze and actions;
- exact/inexact alarm behavior;
- battery/vendor restriction behavior;
- reboot/restart/time-zone recovery;
- documents/share/backup/app-lock/accessibility validation.

### Windows

- installed package/update behavior;
- running/closed-app reminder boundaries;
- timer replacement/cancellation and recovery;
- documents/share/backup/app-lock;
- keyboard/focus/theme/accessibility;
- packaged existing-data upgrade behavior.

### iPhone/iPad

- real signed/provisioned device installation and upgrade;
- notification permission/delivery/actions/snooze/reconciliation;
- lifecycle/restart/time-zone behavior;
- documents/share/backup/app-lock;
- Dynamic Type/VoiceOver/privacy validation.

### Mac Catalyst

- installed application behavior;
- notifications/actions/recovery;
- documents/share/backup/app-lock;
- keyboard/focus/accessibility;
- signing/notarization evidence where applicable.

### Linux desktop

- representative distribution/runtime validation;
- persistence and filesystem behavior;
- secure-storage capability boundary;
- notification/background capability boundary;
- accessibility and packaging behavior.

### Browser/WebAssembly

- browser storage/persistence behavior;
- reload/offline/multiple-tab behavior;
- file/camera/permission behavior;
- notification/background limitations;
- accessibility and supported-browser validation.

---

## 10. Safety, privacy and evidence boundary retained

The 2.18.12 preparation does not add diagnosis, dosage calculation, treatment recommendation, clinical-risk scoring or emergency-service functionality.

Production evidence must use fictional/synthetic application data and must not publish real health records, prescription documents, PINs, backup passwords, private signing keys, access tokens, recovery codes or other secrets.

Green automation alone cannot be represented as:

- production signing;
- real-device validation;
- accessibility completion;
- store approval;
- publication;
- global feature parity;
- a guarantee that the application is defect-free.

---

## 11. Current continuation rule

Continue from the latest exact branch head, not from an older workflow result. Correct real failures rather than suppressing checks. Promote `2.18.12` only after the corresponding automated and production evidence exists for the exact source/package being promoted.
