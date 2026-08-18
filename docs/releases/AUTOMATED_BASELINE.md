# CareNest Current Automated Baseline

**Release line:** `1.0.0-rc.1`  
**Record type:** dynamic verification evidence summary  
**Last updated:** 2026-08-18

This file is the canonical **dynamic** pointer to the latest accepted exact-source automated verification baseline.

It is intentionally kept separate from stable release-policy/documentation contracts so a completed verification run can be recorded without creating a self-referential requirement to re-run the same source merely because its evidence summary changed.

## Current accepted baseline

**Exact source SHA:** `b6eecae66f74bd72bcb20d93508355542f9f3442`

Verified results on that exact source through marker-only PR #80:

- repository Python tooling syntax: **success**;
- package-evidence self-test: **success**;
- documentation-link checker self-test: **success**;
- stable active documentation local-link check: **success** — 182 live local links across 109 stable active Markdown files;
- platform-neutral formatting: **success**;
- unit tests: **122/122**;
- integration tests: **39/39**;
- UI/source-policy tests: **194/194**;
- total core tests: **355/355**;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- Store Package Configuration: Android, Windows, iOS simulator and Mac Catalyst **success**;
- Store Inspection Artifacts: scanner self-test and Android/Windows/Apple inspection artifacts **success**;
- CodeQL: **success**;
- Dependency Audit: **success**.

Authoritative dated evidence:

`docs/releases/FINAL_CANDIDATE_VERIFICATION_20260818.md`

Observed workflow runs:

- CareNest CI: `32141539179`;
- Store Package Configuration: `32141539246`;
- Store Inspection Artifacts: `32141539169`;
- CodeQL: `32141539253`;
- Dependency Audit: `32141539349`.

Verification marker/head SHA:

`ef1e8cea30108f1f3a4dca3158d9b862121e33fe`

PR #80 was closed without merge after success. The marker file is not part of `main`.

## Verification history immediately before the accepted baseline

The final source was reached through fail-closed verification rather than by suppressing failures:

- PR #78 exposed a documentation-link checker false positive on fenced example code;
- PR #79 exposed two stale UI/source-policy assertions after intentional workflow/Markdown maintenance;
- both defects were corrected on `main`;
- PR #80 then passed the complete required automated matrix.

The failed/superseded checkpoints remain historical evidence and are not promoted as successful baselines.

## Post-verification dynamic evidence files

Only these active files are designated for normal post-verification evidence/status promotion without changing stable source/policy documentation:

- `docs/releases/AUTOMATED_BASELINE.md`;
- `PROJECT_STATUS.md`;
- `docs/releases/NEXT_STEPS.md`;
- `what_changed.md`.

They are excluded from mutable-value executable release-documentation assertions, and the default stable documentation-link checker excludes them as documented in `docs/testing/DOCUMENTATION_INTEGRITY.md`.

Stable documents such as `README.md`, `CHANGELOG.md`, `docs/README.md`, `docs/DOCUMENTATION_CATALOG.md`, `docs/COMPLETE_PROJECT_DOCUMENTATION.md`, `docs/CONFIGURATION_REFERENCE.md`, testing guides and stable release-policy documents were finalized before freezing the exact source candidate and are not routinely rewritten merely to insert new dynamic run IDs/test counts.

## Update rule

After a newer exact source is successfully verified:

1. create a dated verification record containing the exact frozen source SHA, verification marker/PR identity where applicable, workflow/run IDs, actual test counts and platform/security/store results;
2. update this file to point to that exact verified source and dated record;
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
- production signing/notarization;
- final signed-package structured evidence;
- live store-console declarations;
- submission-date store-policy compliance;
- store approval/publication.

Those remain separate production gates in `docs/releases/RELEASE_CHECKLIST.md` and `docs/releases/NEXT_STEPS.md`.
