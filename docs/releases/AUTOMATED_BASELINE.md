# CareNest Current Automated Baseline

**Release line:** `1.0.0-rc.1`  
**Record type:** dynamic verification evidence summary  
**Last updated:** 2026-08-19

This file is the canonical **dynamic** pointer to the latest accepted exact-source automated verification baseline.

It is intentionally kept separate from stable release-policy/documentation contracts so a completed verification run can be recorded without creating a self-referential requirement to re-run the same source merely because its evidence summary changed.

## Current accepted baseline

**Exact verified source SHA:** `30ee6c265104c64ec5a1a4013f592f7f058750e8`  
**Verified PR merge ref SHA:** `84fda5bb8ced9f4c487110e43652f51ba2d8d495`  
**Merged main commit:** `2549c08b25145f20c59b7e73ca227c35d5bbf0ec`  
**Verification PR:** `#81` — `security: bound backup archive resource usage`

The PR workflows checked out GitHub's merge ref that merged exact source `30ee6c265104c64ec5a1a4013f592f7f058750e8` into the then-current `main` source `02e63969cc1cf22f0958b0979bb80c33e5e665cf`. `main` did not move while the matrix was running, and PR #81 was merged only after all required workflows succeeded.

Verified results:

- repository Python tooling syntax: **success**;
- package-evidence self-test: **success**;
- documentation-link checker self-test: **success**;
- stable active documentation local-link check: **success** — 182 live local links across 111 stable active Markdown files;
- platform-neutral formatting: **success**;
- unit tests: **122/122**;
- integration tests: **54/54**;
- UI/source-policy tests: **194/194**;
- total core tests: **370/370**;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- Store Package Configuration: Android, Windows, iOS simulator and Mac Catalyst **success**;
- Store Inspection Artifacts: scanner self-test and Android/Windows/Apple inspection artifacts **success**;
- CodeQL: **success**;
- Dependency Audit: **success**.

Observed workflow runs:

- CareNest CI: `32205946013`;
- Store Package Configuration: `32205946003`;
- Store Inspection Artifacts: `32205946001`;
- CodeQL: `32205946030`;
- Dependency Audit: `32205946026`.

Authoritative hardening record frozen in the verified source:

`docs/releases/BACKUP_RESOURCE_HARDENING_20260819.md`

The hardening introduced 15 focused integration regressions. The accepted automated test inventory therefore advanced from 355/355 to **370/370** without reducing the existing unit or UI/source-policy suites.

## Accepted security boundary added in this baseline

The verified source bounds authenticated backup resource consumption before manifest parsing/extraction and during decrypted-container creation. Current defaults are:

- decrypted ZIP container: **2304 MiB** maximum;
- manifest: **1 MiB** maximum;
- SQLite database: **1 GiB** maximum;
- each encrypted document: **512 MiB** maximum;
- total uncompressed ZIP payload: **2 GiB** maximum;
- documents: **5,000** maximum;
- archive-entry count: document ceiling plus required fixed entries;
- explicit directory-only ZIP entries: rejected.

Backup creation validates the generated ZIP against the same current resource/topology boundary before encryption. The shared chunked AEAD decrypt path accepts an optional plaintext limit while retaining existing behavior for callers that do not supply one, and legacy v1 encrypted streams remain compatible while obeying a caller-provided limit.

## Verification history immediately before the accepted baseline

The previously accepted source was:

`b6eecae66f74bd72bcb20d93508355542f9f3442`

Its accepted inventory was 122 unit + 39 integration + 194 UI/source-policy = **355/355** and all required Release/store/security workflows were green.

PR #81 was not created to add speculative feature scope. It addressed a reproduced availability/resource-exhaustion gap in authenticated backup handling, added regression coverage, aligned stable security documentation, passed the complete fail-closed matrix, and was then merged with a merge commit so its 19 meaningful commits remain visible in history.

## Post-verification dynamic evidence files

Only these active files are designated for normal post-verification evidence/status promotion without changing stable source/policy documentation:

- `docs/releases/AUTOMATED_BASELINE.md`;
- `PROJECT_STATUS.md`;
- `docs/releases/NEXT_STEPS.md`;
- `what_changed.md`.

They are excluded from mutable-value executable release-documentation assertions, and the default stable documentation-link checker excludes them as documented in `docs/testing/DOCUMENTATION_INTEGRITY.md`.

Stable documents such as `README.md`, `CHANGELOG.md`, `docs/README.md`, `docs/DOCUMENTATION_CATALOG.md`, `docs/COMPLETE_PROJECT_DOCUMENTATION.md`, `docs/CONFIGURATION_REFERENCE.md`, testing guides and stable release-policy documents are not routinely rewritten merely to insert new dynamic run IDs/test counts.

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

- real-device notification/lifecycle behavior;
- accessibility validation with real assistive technology;
- packaged SQLite/encrypted-document/backup compatibility;
- genuine historical backup compatibility against the new resource ceilings;
- production signing/notarization;
- final signed-package structured evidence;
- live store-console declarations;
- submission-date store-policy compliance;
- store approval/publication.

Those remain separate production gates in `docs/releases/RELEASE_CHECKLIST.md` and `docs/releases/NEXT_STEPS.md`.
