# CareNest Current Automated Baseline

**Release line:** `2.18.13` source preparation  
**Record type:** dynamic verification evidence summary  
**Last updated:** 2026-08-25  
**Release state:** PREPARED IN SOURCE — NOT PUBLISHED

This file is the canonical **dynamic** pointer to the latest accepted exact-source automated verification baseline.

It is intentionally kept separate from stable release-policy/documentation contracts so completed verification evidence can be promoted without representing production-device, signing, store-approval or publication work as complete.

## Current accepted baseline

**Exact verified branch source SHA:** `323ea67281cbbdb3e7ca149fabd7135e8ad8a377`  
**Verified PR merge ref SHA:** `421fead1e82c3d470ebf72417049d80b104c5516`  
**Merged `main` commit:** `b3efa71ab412da65f08d78ad1c4d913e302ff901`  
**Verification PR:** `#87` — `chore: prepare CareNest 2.18.13 maintenance line`  
**PR base used by the verified merge ref:** `b2db4821047dbfb7fe223961fc237afcdfc8371e`

The accepted pull-request workflows checked out GitHub's merge ref `421fead1e82c3d470ebf72417049d80b104c5516`, which merged exact branch source `323ea67281cbbdb3e7ca149fabd7135e8ad8a377` into base `b2db4821047dbfb7fe223961fc237afcdfc8371e`. PR #87 remained mergeable and was merged with an expected-head lock only after every required top-level workflow group reported success for that corrected exact head.

The resulting `main` merge commit is `b3efa71ab412da65f08d78ad1c4d913e302ff901`.

## Verified automated results

Repository/tooling checks:

- repository Python tooling syntax: **success**;
- cross-platform target verifier: **success**;
- cross-platform verifier regression self-tests: **success**;
- package-evidence self-test: **success**;
- documentation-link checker self-test: **success**;
- stable active documentation local-link check: **success** — 210 live local links across 131 stable active Markdown files;
- platform-neutral formatting: **success**.

Observed test inventory:

- unit tests: **122/122**;
- integration tests: **54/54**;
- UI/source-policy tests: **218/218**;
- total core tests: **394/394**.

Configured build verification:

- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- Linux desktop Avalonia Release build: **success**;
- WebAssembly Avalonia browser Release publish: **success**.

Store/security/dependency verification:

- Store Package Configuration: Android, Windows, iOS simulator and Mac Catalyst **success**;
- Store Inspection Artifacts: scanner self-test plus Android, Windows and Apple inspection artifacts **success**;
- CodeQL: **success**;
- unsuppressed Dependency Audit, including platform-neutral, Avalonia desktop/browser and MAUI application graphs: **success**.

## Observed workflow runs

Accepted corrected exact head `323ea67281cbbdb3e7ca149fabd7135e8ad8a377`:

- CareNest CI: `32842319249` — **success**;
- Store Package Configuration: `32842319272` — **success**;
- Store Inspection Artifacts: `32842319256` — **success**;
- CodeQL: `32842319316` — **success**;
- Dependency Audit: `32842319254` — **success**.

## Pre-acceptance correction record

The first PR #87 candidate head was:

`b88a51c23eb12bdfb5d6c07573850ceef9b0ff24`

Its CareNest CI core job exposed a compile-time defect in the newly added `ActiveReleaseLineContractTests.cs`: the test called a nonexistent `string.Replace(char, char, StringComparison)` overload while normalizing `2.18.13` into a version-specific document filename.

Before that source was superseded:

- repository Python/tooling checks passed;
- documentation-link checks passed;
- platform-neutral formatting passed;
- unit tests passed **122/122**;
- integration tests passed **54/54**;
- the UI/source-policy project failed to compile at the new release-line contract;
- Linux desktop and WebAssembly builds had already passed;
- CodeQL and Dependency Audit had passed on that old source.

The defect was fixed without weakening any gate by changing normalization to the valid char-to-char overload. Corrected commit:

`323ea67281cbbdb3e7ca149fabd7135e8ad8a377`

GitHub cancelled the superseded heavy CareNest CI / Store Package / Store Inspection runs after the new head was pushed. A fresh five-workflow acceptance matrix was then run for the corrected head and all required groups succeeded.

The failed/cancelled earlier source is retained here because release evidence must not erase genuine pre-acceptance failures.

## Accepted source changes represented by this baseline

The verified `2.18.13` source retains the established local-first CareNest health-organizer scope and the cross-platform build/presentation baseline from `2.18.12` while adding the maintenance release-line preparation:

- central semantic version `2.18.13`;
- central assembly/file version `2.18.13.0`;
- MAUI display version `2.18.13`;
- MAUI application package/build code `21813`;
- `Microsoft.Maui.Controls` `10.0.100` retained;
- Avalonia `12.1.1` package family retained;
- `VERSION_2_18_13_PREPARATION.md`;
- `RELEASE_NOTES_2_18_13_DRAFT.md`;
- `RELEASE_CHECKLIST_2_18_13.md`;
- updated `VersionConsistencyContractTests` for the `2.18.13` package;
- active release-line alignment coverage in `ActiveReleaseLineContractTests`;
- active project-status, next-step, changelog and continuation handoff preparation.

No runtime clinical feature was added by this maintenance source roll-forward.

## Cross-platform evidence boundary

The accepted automated matrix proves that the configured source builds/publishes through its declared automated targets. It does **not** promote Linux desktop or browser/WebAssembly to full production feature parity merely because those hosts build successfully.

The canonical production records remain:

- `templates/ANDROID_DEVICE_VALIDATION_RECORD.md`;
- `templates/WINDOWS_VALIDATION_RECORD.md`;
- `templates/IOS_DEVICE_VALIDATION_RECORD.md`;
- `templates/MACCATALYST_VALIDATION_RECORD.md`;
- `templates/LINUX_DESKTOP_VALIDATION_RECORD.md`;
- `templates/BROWSER_VALIDATION_RECORD.md`;
- `templates/ACCESSIBILITY_VALIDATION_RECORD.md`;
- `templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`;
- `templates/SIGNING_PROVENANCE_RECORD.md`;
- `templates/STORE_SUBMISSION_RECORD.md`;
- `templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`.

Canonical templates are evidence containers, not successful evidence by themselves, and must remain visibly unperformed until actual validation occurs.

## Accepted backup security/resource boundary retained

The verified source preserves the established authenticated-backup resource protections. Current defaults remain:

- decrypted ZIP container: **2304 MiB** maximum;
- manifest: **1 MiB** maximum;
- SQLite database: **1 GiB** maximum;
- each encrypted document: **512 MiB** maximum;
- total uncompressed ZIP payload: **2 GiB** maximum;
- documents: **5,000** maximum;
- archive-entry count: document ceiling plus required fixed entries;
- explicit directory-only ZIP entries: rejected.

Automated protection does not replace packaged or genuine historical-backup compatibility validation.

## Previous accepted automated baseline

Immediately before this promotion, the accepted exact feature/source branch SHA was:

`1d9de89fbc7de69696c9d4276991f07bcdce1027`

for CareNest `2.18.12`, with:

- 122 unit tests;
- 54 integration tests;
- 215 UI/source-policy tests;
- **391/391 total core tests**;
- Android, Windows, iOS simulator, Mac Catalyst, Linux desktop and WebAssembly/browser build success;
- Store Package Configuration success;
- Store Inspection Artifacts success;
- CodeQL success;
- Dependency Audit success.

The newer accepted `2.18.13` baseline advances the UI/source-policy inventory to **218** and the total core inventory to **394/394** while retaining the 122-unit and 54-integration suites.

Historical workflow success is never transferred to a newer source; every result remains evidence only for its recorded exact source/merge boundary.

## Post-verification dynamic evidence files

These active files are designated for normal evidence/status promotion:

- `docs/releases/AUTOMATED_BASELINE.md`;
- `PROJECT_STATUS.md`;
- `docs/releases/NEXT_STEPS.md`;
- `what_changed.md`.

Mutable run IDs, source SHAs and test counts in these dynamic evidence files must not be treated as fixed values by executable release-policy tests. Stable source-policy contracts may protect invariant semantics such as active version/build alignment and fail-closed publication state.

## Update rule

After a newer exact feature/source is successfully verified:

1. record the exact frozen source SHA, verification PR/merge identity, workflow/run IDs, actual test counts and platform/security/store results;
2. update this file to point to that exact verified source and evidence record;
3. update `PROJECT_STATUS.md`, `docs/releases/NEXT_STEPS.md`, and `what_changed.md` with the resulting state/evidence;
4. preserve failed/superseded candidate history where it materially explains the accepted replacement source;
5. do not modify runtime/test/project/workflow/build-script/stable-policy source merely to manufacture a cleaner evidence story;
6. if a verification-relevant correction is required, freeze and verify the corrected replacement source instead.

## Production boundary

The accepted automated baseline still does not prove:

- real Android installed-device reminder/notification behavior;
- installed Windows package/update/runtime behavior;
- real signed/provisioned iPhone/iPad behavior;
- installed Mac Catalyst behavior;
- representative Linux runtime behavior or full feature parity;
- browser persistence/storage/file/permission/reload/offline behavior or full feature parity;
- accessibility validation with real applicable assistive technology;
- packaged SQLite/encrypted-document/backup compatibility;
- genuine historical backup compatibility against current resource ceilings;
- production signing/provisioning/notarization;
- final production package/deployment SHA-256/provenance evidence;
- live store-console declarations and metadata reconciliation;
- submission-date store/distribution policy compliance;
- store approval;
- publication/deployment.

CareNest `2.18.13` therefore remains **prepared in source, not published** until the applicable production evidence is actually completed.
