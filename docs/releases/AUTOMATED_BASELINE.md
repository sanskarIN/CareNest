# CareNest Current Automated Baseline

**Release line:** `1.0.0-rc.1`  
**Record type:** dynamic verification evidence summary  
**Last updated:** 2026-08-18

This file is the canonical **dynamic** pointer to the latest accepted exact-source automated verification baseline.

It is intentionally kept separate from stable release-policy/documentation contracts so a completed verification run can be recorded without creating a self-referential requirement to re-run the same source merely because its evidence summary changed.

## Current accepted baseline

**Exact source SHA:** `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

Verified results on that exact source:

- unit tests: **122/122**;
- integration tests: **39/39**;
- UI/source-policy tests: **175/175**;
- total core tests: **336/336**;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- Store Package Configuration: all four configured targets **success**;
- CodeQL: **success**.

Authoritative dated evidence:

`docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`

## Newer source state

Current `main` contains verification-relevant release-governance, package-evidence, dependency/toolchain, workflow, documentation-integrity and bug-hardening changes after the accepted baseline above.

Those changes do **not** become the accepted baseline until the applicable exact-source verification matrix succeeds and a replacement dated verification record is created.

Do not predict a replacement test count from source inspection.

## Post-verification dynamic evidence files

Only these active files are designated for normal post-verification evidence/status promotion without changing stable source/policy documentation:

- `docs/releases/AUTOMATED_BASELINE.md`;
- `PROJECT_STATUS.md`;
- `docs/releases/NEXT_STEPS.md`;
- `what_changed.md`.

They are excluded from mutable-value executable release-documentation assertions, and the default stable documentation-link checker excludes them as documented in `docs/testing/DOCUMENTATION_INTEGRITY.md`.

Stable documents such as `README.md`, `CHANGELOG.md`, `docs/README.md`, `docs/DOCUMENTATION_CATALOG.md`, `docs/COMPLETE_PROJECT_DOCUMENTATION.md`, `docs/CONFIGURATION_REFERENCE.md`, testing guides and stable release-policy documents must be finalized **before** freezing the exact source candidate. Do not routinely rewrite those stable inputs after verification merely to insert new run IDs/test counts.

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

An accepted automated baseline still does not prove:

- real-device notification/lifecycle behavior;
- accessibility validation;
- packaged SQLite/encrypted-document/backup compatibility;
- production signing/notarization;
- final signed-package structured evidence;
- live store-console declarations;
- submission-date store-policy compliance;
- store approval/publication.

Those remain separate production gates in `docs/releases/RELEASE_CHECKLIST.md` and `docs/releases/NEXT_STEPS.md`.
