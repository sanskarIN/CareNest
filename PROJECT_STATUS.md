# CareNest — Current Project Status

**Date:** 2026-08-25  
**Active preparation line:** `2.18.13`  
**Package/build code:** `21813`  
**Release state:** PREPARED IN SOURCE — NOT PUBLISHED  
**Repository:** `https://github.com/sanskarIN/CareNest`  
**Preparation branch:** `continue/prepare-2.18.13-20260825`  
**Starting `main`:** `b2db4821047dbfb7fe223961fc237afcdfc8371e`  
**Last accepted automated-source authority:** `docs/releases/AUTOMATED_BASELINE.md`

CareNest `2.18.13` is a maintenance continuation from the verified `2.18.12` repository baseline. Version/source preparation is in progress. No `2.18.13` production release, store approval, production signing, real-device validation or full cross-platform parity is claimed.

Historical evidence remains authoritative only for the exact source that produced it. The successful `2.18.12` and PR #86 workflow results are starting context, not verification evidence for a newer `2.18.13` head.

---

## 1. Current product boundary

CareNest is a local-first organizational health application. The current codebase uses:

- .NET MAUI for Android, iOS/iPadOS, Mac Catalyst and Windows;
- Avalonia for a Linux-capable desktop host and a WebAssembly browser host.

CareNest does **not** diagnose conditions, calculate or infer dosage, recommend treatment, perform clinical medication-interaction checking, calculate clinical risk scores, independently prove adherence, replace clinicians/pharmacists, provide emergency services, or guarantee operating-system/browser notification delivery.

The application remains account-free and local-first within the documented scope. It does not require a CareNest cloud backend and does not silently upload local health records.

---

## 2. Active 2.18.13 source metadata

The active source is being prepared with:

- central semantic version: `2.18.13`;
- assembly/file version: `2.18.13.0`;
- informational version: `2.18.13`;
- MAUI application display version: `2.18.13`;
- MAUI package/build code: `21813`;
- `Microsoft.Maui.Controls`: `10.0.100`;
- Avalonia package family: `12.1.1`.

`tests/CareNest.UiTests/VersionConsistencyContractTests.cs` now protects the `2.18.13` source/package metadata and non-publication state of the version-specific release documents.

Prepared metadata is not publication evidence.

---

## 3. 2.18.13 starting repository boundary

The preparation branch starts from merged `main` commit:

`b2db4821047dbfb7fe223961fc237afcdfc8371e`

That commit includes PR #86 (`docs: promote CareNest 2.18.12 verification evidence`). PR #86 was merged only after its exact head:

`e14a40d095a6f39993a0f62e497f15ec4668701f`

completed successfully in:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts.

Those results prove the PR #86 source boundary only. The final `2.18.13` preparation head must complete its own configured verification matrix before merge/promotion.

---

## 4. Last accepted 2.18.12 automated baseline

The last accepted feature/source baseline before this version roll-forward remains recorded in:

`docs/releases/AUTOMATED_BASELINE.md`

For the accepted `2.18.12` source boundary, the observed core suite was:

- unit tests: **122/122**;
- integration tests: **54/54**;
- UI/source-policy tests: **215/215**;
- total core tests: **391/391**.

The accepted `2.18.12` matrix also recorded successful Android, Windows, iOS simulator, Mac Catalyst, Linux desktop and WebAssembly/browser build/publish paths, plus CodeQL, unsuppressed dependency audit and store inspection/configuration workflows.

The Windows job history includes a retained transient MAUI-workload `ResponseEnded` download failure followed by a same-source successful job-only retry. That history remains evidence rather than being rewritten.

These numbers and workflow results must not be relabeled as `2.18.13` results. New counts/conclusions are recorded only after the final new source actually runs.

---

## 5. Configured platform reach

### .NET MAUI targets

- Android: `net10.0-android`;
- iOS/iPadOS: `net10.0-ios`;
- Mac Catalyst: `net10.0-maccatalyst`;
- Windows: `net10.0-windows10.0.19041.0`.

### Avalonia presentation/build hosts

- Linux-capable desktop: `CareNest.CrossPlatform.Desktop`, targeting `net10.0`;
- modern WebAssembly-capable browsers: `CareNest.CrossPlatform.Browser`, targeting `net10.0-browser`;
- shared Avalonia application/views: `CareNest.CrossPlatform`.

Configured build reach is not production feature parity. Linux/browser capabilities remain evidence-driven and must use `PASS`, `FAIL`, `BLOCKED`, `N/A` or `NOT RUN` according to actual validation.

Architecture/capability guide: `docs/setup/CROSS_PLATFORM.md`.

---

## 6. Established source-complete application scope

The current source retains the intended non-clinical organizer scope, including:

- multiple local person/family profiles;
- medicine records with user-entered strength/instruction text;
- explicit schedules and deterministic reminder occurrences;
- reminder lifecycle/history/status/reconciliation behavior;
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
- CodeQL, dependency, store and release gates;
- Linux desktop and browser presentation/build hosts.

No speculative clinical/emergency feature has been added to inflate scope.

---

## 7. Cross-platform verification boundary

The repository retains fail-closed cross-platform verification through:

- `build/scripts/verify-cross-platform-targets.py`;
- `build/scripts/test-verify-cross-platform-targets.py`.

The verifier checks MAUI targets, Avalonia declarations, desktop/browser startup wiring, solution registration, Avalonia XAML, CI/audit/release-gate integration, public platform claims, setup documentation and fail-closed Linux/browser evidence templates.

Regression self-tests intentionally require broken configurations to fail detection.

---

## 8. Production evidence semantics

Canonical authority: `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`.

Allowed states remain:

- `PASS`;
- `FAIL`;
- `BLOCKED`;
- `N/A`;
- `NOT RUN`.

Unknown, stale, queued, superseded, blocked or unperformed work must never be represented as a pass.

Public evidence must use fictional/synthetic application data and must not contain real health records, prescription documents, PINs, backup passwords, private signing keys, access tokens, recovery codes or other secrets.

---

## 9. Backup security/resource boundary retained

The accepted source retains the documented authenticated-backup ceilings:

- decrypted ZIP container: **2304 MiB** maximum;
- manifest: **1 MiB** maximum;
- SQLite database: **1 GiB** maximum;
- each encrypted document: **512 MiB** maximum;
- total uncompressed ZIP payload: **2 GiB** maximum;
- documents: **5,000** maximum;
- archive-entry count bounded by document ceiling plus required fixed entries;
- explicit directory-only ZIP entries rejected.

Automated protection does not replace packaged/historical backup compatibility validation.

---

## 10. External-commerce/package boundary retained

Funding/storefront promotion remains outside the shipped health application package. Release governance continues to require final distributed app payloads to avoid in-app promotion/purchase surfaces for external funding/storefront destinations.

No health feature or local-data access may depend on funding/purchase state.

---

## 11. 2.18.13 source-side work completed so far

On `continue/prepare-2.18.13-20260825`:

- central version metadata rolled to `2.18.13`;
- MAUI display/package metadata rolled to `2.18.13` / `21813`;
- version-consistency contract rolled to `2.18.13`;
- `VERSION_2_18_13_PREPARATION.md` added;
- `RELEASE_NOTES_2_18_13_DRAFT.md` added;
- `RELEASE_CHECKLIST_2_18_13.md` added;
- dynamic project/release handoff documents are being aligned to the new preparation boundary.

The final branch head still requires exact-head automation before merge.

---

## 12. Remaining production work

The remaining blockers are genuine external/manual work rather than items that may be truthfully completed from source inspection alone:

- packaged SQLite/encrypted-document/backup compatibility;
- Android installed-device notification/reminder/recovery/accessibility validation;
- Windows installed-package/update/reminder/accessibility validation;
- signed/provisioned iPhone/iPad real-device validation;
- installed Mac Catalyst behavior and notarized-candidate evidence where applicable;
- representative Linux runtime evidence;
- hosted browser/WebAssembly runtime/storage/permission/accessibility evidence;
- cross-platform accessibility validation using applicable assistive technology;
- production signing/provisioning/notarization provenance;
- final package/deployment hashes/provenance;
- store-safe final payload inspection;
- live submission-day policy/metadata/declaration review;
- actual submission, review, approval and publication/deployment outcomes.

---

## 13. Release decision

CareNest `2.18.13` is **prepared/in preparation at the source boundary and NOT PUBLISHED**.

Do not create or treat `v2.18.13` as an approved production tag until applicable production evidence is complete and tagged release gates permit promotion.

Do not claim production signing without evidence, real-device behavior from simulator/build success, Linux/browser full parity from build success, store approval before actual approval, publication before actual publication, accessibility completion without assistive-technology evidence, or a global bug-free guarantee.

The next actionable work is maintained in `docs/releases/NEXT_STEPS.md`.
