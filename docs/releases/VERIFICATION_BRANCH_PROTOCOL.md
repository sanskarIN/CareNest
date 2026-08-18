# Exact-Head Verification Branch Protocol

CareNest uses temporary marker-only pull requests when fresh PR-triggered automated evidence is needed for an exact intended `main` source state without merging verification artifacts into production source.

## Why this exists

A previously green CI run does not prove a later commit is green.

This matters when source changes affect:

- runtime behavior;
- tests/contracts;
- project/build configuration;
- dependencies/package resolution;
- platform source/configuration;
- GitHub Actions workflows;
- release scripts/quality/preflight logic;
- package-evidence tooling;
- repository policy/release gates;
- current documentation consumed by release/source-policy tests.

Documentation-only changes can sometimes be compared separately, but only when no verification-relevant source category above changed and no changed document is consumed by a verification contract.

## Current verified baseline versus new candidate

The latest exact verified Gumroad implementation/source-policy baseline remains:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

Its recorded result is 336/336 core tests plus the platform/store-candidate/CodeQL evidence in:

`docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`

Later release-documentation consistency tests, package-evidence tooling, CI/release workflow changes and related current release documents are verification-relevant. They must receive a fresh exact-source verification before a newer source replaces the baseline above.

Do not predict or publish a newer test total merely by counting added test methods. Record only the count actually reported by the exact verification run.

## Marker-only procedure

1. Finish all intended verification-relevant changes on `main`.
2. Record the exact `main` SHA to verify.
3. Create a temporary branch from that exact SHA.
4. Add exactly one marker file under `build/verification/`.
5. Open a pull request from the marker branch to `main`.
6. Confirm the PR changes only the marker file beyond the frozen base SHA.
7. Require the full applicable PR automation:
   - package-evidence Python syntax/self-test through CareNest CI;
   - platform-neutral formatting;
   - unit tests;
   - integration tests;
   - UI-contract/policy tests;
   - Android Release;
   - Windows Release;
   - iOS simulator Release;
   - Mac Catalyst Release;
   - Store Package Configuration on all four targets;
   - Store Inspection Artifacts;
   - CodeQL;
   - unsuppressed Dependency Audit.
8. Record exact run IDs, test counts, platform outcomes, source/base SHA, marker/head SHA, and PR number.
9. Close the marker PR without merge after evidence is recorded.
10. Ensure the marker file never enters `main`.

## If a gate fails

Do not suppress/ignore a legitimate failure to preserve the checkpoint.

Instead:

1. inspect the exact failing job/log;
2. determine whether the defect is source, test, workflow, package, toolchain, documentation contract or infrastructure;
3. fix legitimate source/test/workflow/package/script/documentation-contract defects on `main`;
4. close the old marker PR as failed/superseded;
5. create a new marker branch from the corrected exact `main` SHA;
6. rerun the complete required matrix.

Partial green evidence from a failed checkpoint can be retained historically, but it is not the final baseline.

## If `main` changes while verification is running

Determine whether the newer commits are verification-relevant.

### Verification-relevant movement

Examples:

- `.cs`, `.xaml`, `.csproj`, `.props`, `.targets`;
- package version/configuration files;
- tests;
- workflows;
- `build/scripts/*`;
- platform configuration/resources affecting build/runtime;
- repository policy source consumed by executable tests;
- release documents parsed by current consistency/source-policy tests.

If these change, the running checkpoint is stale for the newer source. Close/supersede it and verify the new exact head.

### Truly documentation-only movement

A later commit can be treated as documentation-only only when an exact comparison confirms there are no runtime/test/project/workflow/package/platform/build-script changes **and** none of the changed documents are inputs to executable/source-policy tests.

In that case:

- keep the previously verified source SHA explicit;
- identify the later documentation head separately;
- do not claim the documentation head itself ran the platform matrix unless it actually did;
- if there is any ambiguity, prefer a fresh exact-head verification.

## Marker naming

Use a descriptive path such as:

```text
build/verification/<purpose>-YYYYMMDD.txt
```

The marker should record the frozen source SHA and state that it is verification-only and must not be merged.

## PR description

The PR body should identify:

- frozen base/source SHA;
- marker path;
- purpose of verification;
- required gates;
- statement that the marker must not be merged;
- any known source boundary that this PR supersedes.

## Evidence requirements

Record at least:

```text
Verification PR:
Frozen source/base SHA:
Marker/head SHA:
CareNest CI run:
Package-evidence tooling self-test:
Formatting:
Unit tests:
Integration tests:
UI-contract/policy tests:
Android Release:
Windows Release:
iOS simulator Release:
Mac Catalyst Release:
Store Package Configuration run:
Store Inspection Artifacts run:
CodeQL run:
Dependency Audit run:
PR closed without merge: yes/no
```

## Production tag verification is separate

Marker-only PR verification proves the candidate source under PR automation. It is not the final production tag gate.

For an approved `v*` tag, the exact tagged commit is configured to run:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- CareNest Release Evidence.

The Release Gate and Release Evidence workflows also verify the package-evidence tooling source/self-test, but they do not create or sign final production packages.

Production publication must wait for all required tag-triggered workflows plus applicable:

- final signed-package structured evidence JSON;
- real-device/manual evidence;
- accessibility evidence;
- packaged SQLite/document/backup compatibility evidence;
- production signing/notarization provenance;
- live store-console declarations/metadata;
- submission-date store-policy review;
- final store approval/publication evidence.

If release/tag workflows or release scripts change after marker verification, create a new exact-head verification before using those changes for production.

## Historical evidence rule

Never rewrite a failed/superseded marker PR as successful release evidence.

Document what actually happened:

- successes;
- failures/skips;
- root cause;
- fixing commit;
- replacement verification.

This keeps release history useful for future debugging and prevents a green subset from being confused with a fully green matrix.
