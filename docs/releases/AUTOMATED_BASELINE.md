# CareNest Current Automated Baseline

**Release line:** `1.0.0-rc.1`  
**Record type:** dynamic verification evidence summary  
**Last updated:** 2026-08-18

This file is the canonical **dynamic** pointer to the latest accepted exact-source automated verification baseline.

It is intentionally kept separate from stable release-policy contracts so a completed verification run can be recorded without creating a self-referential requirement to re-run the same source merely because its evidence summary changed.

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

Current `main` contains verification-relevant package-evidence/release-governance changes after the accepted baseline above.

Those changes do **not** become the accepted baseline until the applicable exact-source verification matrix succeeds and a replacement dated verification record is created.

Do not predict a replacement test count from source inspection.

## Update rule

After a newer exact source is successfully verified:

1. create a dated verification record containing the exact source SHA, marker/PR identity where applicable, workflow/run IDs, actual test counts and platform/security/store results;
2. update this file to point to that exact verified source and dated record;
3. update dynamic status/handoff surfaces such as `PROJECT_STATUS.md`, `docs/releases/NEXT_STEPS.md`, `docs/COMPLETE_PROJECT_DOCUMENTATION.md`, `docs/DOCUMENTATION_CATALOG.md`, `docs/README.md`, `CHANGELOG.md` and `what_changed.md` as documentation-only evidence promotion;
4. do not modify runtime/test/project/workflow/build-script/stable-policy source merely to record the completed result;
5. if a verification-relevant source change is required, freeze and verify the replacement source instead.

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
