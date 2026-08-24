# CareNest Current Automated Baseline

**Release line:** `2.18.12` source preparation  
**Record type:** dynamic verification evidence summary  
**Last updated:** 2026-08-24  
**Release state:** PREPARED IN SOURCE — NOT PUBLISHED

This file is the canonical **dynamic** pointer to the latest accepted exact-source automated verification baseline.

It is intentionally kept separate from stable release-policy/documentation contracts so a completed verification run can be recorded without creating a self-referential requirement to re-run the same source merely because its evidence summary changed.

## Current accepted baseline

**Exact verified branch source SHA:** `1d9de89fbc7de69696c9d4276991f07bcdce1027`  
**Verified PR merge ref SHA:** `0a579f2a1d927173f3c69e8b32d0ac52ced6c944`  
**Merged `main` commit:** `ca80bd554296363d71a6008cac73c819be77b39b`  
**Verification PR:** `#84` — `feat: complete cross-platform hosts and prepare 2.18.12`  
**PR base used by the verified merge ref:** `f58aaca1d1d7a3fef68cb30b8b9a68fa0f94bf09`

The pull-request workflows checked out GitHub's merge ref `0a579f2a1d927173f3c69e8b32d0ac52ced6c944`, which merged exact branch source `1d9de89fbc7de69696c9d4276991f07bcdce1027` into the then-current `main` source `f58aaca1d1d7a3fef68cb30b8b9a68fa0f94bf09`. `main` did not move during the acceptance matrix, and PR #84 was merged only after every required top-level workflow group had an accepted successful result for that exact source/base combination.

The resulting `main` merge commit is `ca80bd554296363d71a6008cac73c819be77b39b`. This dynamic evidence promotion records the already-observed PR acceptance evidence; it does not manufacture production-device, signing, store-approval or publication evidence for the merge commit.

## Verified automated results

Repository/tooling checks:

- repository Python tooling syntax: **success**;
- cross-platform target verifier: **success**;
- cross-platform verifier regression self-tests: **success**;
- package-evidence self-test: **success**;
- documentation-link checker self-test: **success**;
- stable active documentation local-link check: **success** — 210 live local links across 128 stable active Markdown files;
- platform-neutral formatting: **success**.

Observed test inventory:

- unit tests: **122/122**;
- integration tests: **54/54**;
- UI/source-policy tests: **215/215**;
- total core tests: **391/391**.

Configured build verification:

- Android Release: **success**;
- Windows Release: **success** after a job-only retry described below;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- Linux desktop Avalonia Release build: **success**;
- WebAssembly Avalonia browser Release publish: **success**.

Store/security/dependency verification:

- Store Package Configuration: Android, Windows, iOS simulator and Mac Catalyst **success**;
- Store Inspection Artifacts: scanner self-test plus Android, Windows and Apple inspection artifacts **success**;
- CodeQL: **success**;
- unsuppressed Dependency Audit, including platform-neutral, Avalonia desktop/browser and MAUI application graphs: **success**.

## Windows CI retry record

CareNest CI run `32685906690` initially failed only in the Windows job while `dotnet workload install maui` was downloading workload packs. The installer reported an HTTP response truncation:

`ResponseEnded`

The Windows application build had not started, so this was not a source compilation failure.

The failed Windows job was rerun without changing the source, PR base or merge ref. On run attempt 2:

- .NET setup succeeded;
- MAUI workload installation succeeded;
- the Windows Release build succeeded;
- every other CI job remained successful.

The final top-level CareNest CI conclusion therefore became **success**. The transient first-attempt failure is retained here because verification evidence must not erase real failures or pretend they never occurred.

## Observed workflow runs

- CareNest CI: `32685906690` — final conclusion **success**, with Windows job-only retry on attempt 2;
- Store Package Configuration: `32685906685` — **success**;
- Store Inspection Artifacts: `32685906678` — **success**;
- CodeQL: `32685906722` — **success**;
- Dependency Audit: `32685906679` — **success**.

## Accepted source changes represented by this baseline

The verified source includes the established local-first CareNest health-organizer scope plus the following release-relevant continuation work:

- .NET 10 MAUI targets for Android, iOS/iPadOS, Mac Catalyst and Windows;
- Avalonia shared presentation host;
- Linux-capable Avalonia desktop host;
- WebAssembly Avalonia browser host;
- solution registration for the cross-platform projects;
- fail-closed cross-platform target verification and isolated verifier self-tests;
- Linux/browser CI build integration;
- Avalonia dependency-audit integration;
- Linux/browser tagged-release build gates;
- Linux/browser production-validation evidence templates that intentionally begin `NOT RUN`;
- explicit separation of configured build/presentation reach from production feature parity;
- CareNest semantic/display version preparation for `2.18.12`;
- MAUI application package/build code `21812`;
- central assembly/file/informational version consistency checks;
- non-publication-state release documentation contracts;
- `Microsoft.Maui.Controls` `10.0.100` integrated into the same exact-head verification matrix;
- correction of the earlier final-newline formatting defects without disabling formatting enforcement.

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

The verified source preserves the previously accepted authenticated-backup resource protections. Current defaults remain:

- decrypted ZIP container: **2304 MiB** maximum;
- manifest: **1 MiB** maximum;
- SQLite database: **1 GiB** maximum;
- each encrypted document: **512 MiB** maximum;
- total uncompressed ZIP payload: **2 GiB** maximum;
- documents: **5,000** maximum;
- archive-entry count: document ceiling plus required fixed entries;
- explicit directory-only ZIP entries: rejected.

Backup creation validates the generated ZIP against the same resource/topology boundary before encryption. Existing chunked authenticated-decryption and legacy compatibility rules remain subject to the documented caller-provided resource limits.

## Previous accepted automated baseline

Immediately before this promotion, the accepted exact branch source was:

`30ee6c265104c64ec5a1a4013f592f7f058750e8`

with:

- 122 unit tests;
- 54 integration tests;
- 194 UI/source-policy tests;
- **370/370 total core tests**;
- Android, Windows, iOS simulator and Mac Catalyst Release success;
- Store Package Configuration success;
- Store Inspection Artifacts success;
- CodeQL success;
- Dependency Audit success.

The newer accepted baseline advances the source-policy/UI inventory to 215 tests and the total core inventory to **391/391** while retaining the 122-unit and 54-integration suites.

Historical workflow results remain evidence only for their own exact source/merge boundaries and are not rewritten to appear as results of newer source.

## Post-verification dynamic evidence files

Only these active files are designated for normal post-verification evidence/status promotion without changing stable source/policy documentation:

- `docs/releases/AUTOMATED_BASELINE.md`;
- `PROJECT_STATUS.md`;
- `docs/releases/NEXT_STEPS.md`;
- `what_changed.md`.

They are excluded from mutable-value executable release-documentation assertions, and the default stable documentation-link checker excludes them as documented in `docs/testing/DOCUMENTATION_INTEGRITY.md`.

Stable documents such as `README.md`, `CHANGELOG.md`, `docs/README.md`, `docs/DOCUMENTATION_CATALOG.md`, `docs/COMPLETE_PROJECT_DOCUMENTATION.md`, `docs/CONFIGURATION_REFERENCE.md`, testing guides and stable release-policy documents are not routinely rewritten merely to insert mutable run IDs or test-count values.

## Update rule

After a newer exact source is successfully verified:

1. record the exact frozen source SHA, verification PR/merge identity where applicable, workflow/run IDs, actual test counts and platform/security/store results;
2. update this file to point to that exact verified source and evidence record;
3. update `PROJECT_STATUS.md`, `docs/releases/NEXT_STEPS.md`, and `what_changed.md` with the resulting state/evidence;
4. optionally run `python3 build/scripts/verify-documentation-links.py --include-dynamic` as a documentation-only integrity audit of those dynamic files;
5. do not modify runtime/test/project/workflow/build-script/stable-policy/stable-documentation source merely to record the completed result;
6. if any verification-relevant or stable-source correction is required, freeze and verify the corrected replacement source instead.

## Stable-policy boundary

Stable release-policy documents and executable source-policy tests may link to this file, but they must not require this dynamic evidence file's current SHA/test-count text as an executable test input.

This avoids an evidence loop where recording a successful verification would itself change an executable test input and require another verification solely to record the first one.

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
- final signed-package SHA-256/provenance evidence;
- live store-console declarations and metadata reconciliation;
- submission-date store-policy compliance;
- store approval;
- publication.

CareNest `2.18.12` therefore remains **prepared in source, not published** until the applicable production evidence is actually completed.