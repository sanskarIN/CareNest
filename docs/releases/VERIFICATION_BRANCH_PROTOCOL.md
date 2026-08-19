# Exact-Head Verification Branch Protocol

CareNest uses exact-head verification when fresh PR-triggered automated evidence is needed for an intended source state.

## Why this exists

A previously green CI run does not prove a later verification-relevant commit is green.

Verification-relevant changes include:

- runtime behavior;
- tests/contracts;
- project/build configuration;
- dependencies/package resolution;
- platform source/configuration;
- GitHub Actions workflows;
- release scripts/quality/preflight logic;
- package-evidence tooling;
- documentation-integrity tooling;
- repository policy/release gates;
- stable release-policy/evidence documents parsed by executable/source-policy tests.

## Dynamic evidence versus stable policy

CareNest separates mutable verification evidence from stable release policy.

Canonical dynamic automated-baseline pointer:

`docs/releases/AUTOMATED_BASELINE.md`

Dynamic evidence/status documents include, as applicable:

- `docs/releases/AUTOMATED_BASELINE.md`;
- dated verification records under `docs/releases/`;
- `PROJECT_STATUS.md`;
- `docs/releases/NEXT_STEPS.md`;
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`;
- `docs/DOCUMENTATION_CATALOG.md`;
- `docs/README.md`;
- `CHANGELOG.md`;
- `what_changed.md`.

These files may be updated **after** successful exact-source verification to record the observed result as documentation-only evidence promotion, provided the promotion does not also change runtime/test/project/workflow/build-script/stable-policy source.

Executable documentation/source-policy tests must not pin mutable SHA, workflow-run ID or test-count values inside dynamic evidence files. They may verify stable invariants such as required authority links, privacy/safety wording or file existence as long as those assertions do not force a post-verification result-recording commit to rerun solely because a moving evidence value changed.

This prevents a self-referential loop where recording successful verification changes a tested evidence value and therefore requires another verification only to record the first result.

## Stable release-policy inputs

Stable policy documents parsed for invariant contracts include:

- `docs/releases/RELEASE_CHECKLIST.md`;
- `docs/releases/STORE_BUILD_POLICY.md`;
- `docs/releases/RELEASE_EVIDENCE.md`;
- `docs/releases/RELEASE_PROCESS.md`;
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md`;
- `docs/releases/QUALITY_GATE.md`;
- `docs/releases/SECURITY_RELEASE_REVIEW.md`;
- `docs/releases/MANUAL_TEST_MATRIX.md`;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`;
- `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`;
- `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`;
- canonical templates under `docs/releases/templates/`;
- this protocol.

If one of these stable policy inputs changes, the exact source requires fresh verification.

Stable policy documents must not require editing a moving accepted SHA/test count after every successful verification. They should reference `docs/releases/AUTOMATED_BASELINE.md` and dated verification records for the current accepted automated result.

## Current accepted automated baseline

Use:

`docs/releases/AUTOMATED_BASELINE.md`

That dynamic record identifies the latest accepted exact-source automated baseline and its authoritative dated evidence.

Do not predict or publish a newer test total merely by counting test methods. Record only the count actually reported by the exact verification run.

## Verification options

### Verification of an open implementation/policy PR

When the intended verification-relevant work already lives on a pull-request branch:

1. finish the complete intended branch change set;
2. update pre-verification dynamic handoff/status material if needed;
3. freeze the final PR head SHA;
4. make no further verification-relevant change while that checkpoint is being accepted;
5. require the full applicable PR automation for that exact head;
6. record actual run IDs/test counts/results after completion;
7. merge only if the required matrix is successful and branch protection/review requirements are satisfied.

If a later commit is added, the earlier head’s result is superseded for the new head.

### Marker-only verification of an intended `main` state

Use a temporary marker-only PR when fresh PR-triggered evidence is needed for an exact intended `main` source without merging a verification artifact.

1. Finish all intended verification-relevant changes on `main`.
2. Update dynamic handoff/status documentation needed before the verification freeze.
3. Record the exact `main` SHA to verify.
4. Do not make further verification-relevant changes while that checkpoint runs.
5. Create a temporary branch from that exact SHA.
6. Add exactly one marker file under `build/verification/`.
7. Open a pull request from the marker branch to `main`.
8. Confirm the PR changes only the marker beyond the frozen base SHA.
9. Require the full applicable PR automation.
10. Record exact run IDs, actual test counts, platform outcomes, source/base SHA, marker/head SHA and PR number in a dated verification record.
11. Close the marker PR without merge after evidence is recorded.
12. Ensure the marker never enters `main`.
13. Promote the result only through dynamic evidence/status documents unless a real verification-relevant source change is required.

## Required PR automation

For a verification-relevant source require, as applicable:

- repository Python tooling syntax/self-tests through CareNest CI;
- documentation-integrity self-test and stable active-link check;
- platform-neutral formatting;
- unit tests;
- integration tests;
- UI/source-policy tests;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- Store Package Configuration on every configured target;
- Store Inspection Artifacts;
- CodeQL;
- unsuppressed Dependency Audit.

Production `v*` tags additionally require the configured Release Gate and Release Evidence workflows.

## If a gate fails

Do not suppress or ignore a legitimate failure merely to preserve a checkpoint.

Instead:

1. inspect the exact failing job/log;
2. determine whether the defect is source, test, workflow, package, toolchain, stable documentation contract or infrastructure;
3. preserve completed/failed evidence accurately;
4. fix legitimate verification-relevant defects;
5. update dynamic handoff/status material as needed before the replacement freeze;
6. freeze the corrected exact head;
7. rerun the complete required matrix.

Partial green evidence from a failed/superseded checkpoint is historical debugging evidence, not the final baseline.

## If the source changes while verification runs

Determine whether newer commits are verification-relevant.

Examples include:

- `.cs`, `.xaml`, `.csproj`, `.props`, `.targets`;
- package version/configuration files;
- tests;
- workflows;
- `build/scripts/*`;
- platform configuration/resources affecting build/runtime;
- stable release/evidence policy source parsed by executable tests.

If verification-relevant source changes, the running checkpoint is stale for the newer source and a new exact-head matrix is required.

## Post-verification dynamic evidence promotion

After a checkpoint completes successfully, dynamic evidence/status documents may be updated to record the completed result without treating the evidence-only commit as a new executable baseline, provided an exact comparison confirms there are no changes to runtime/test/project/workflow/package/platform/build-script/stable-policy source.

In that case:

- keep the verified source SHA explicit;
- identify the later evidence/documentation head separately when useful;
- do not claim the evidence-only head itself ran platform workflows;
- keep `AUTOMATED_BASELINE.md` pointed at the exact source that actually ran;
- do not change a stable policy merely to insert the new SHA/test count;
- if there is any verification-relevant change or ambiguity, verify a new exact source.

## Marker naming

Use a descriptive path such as:

```text
build/verification/<purpose>-YYYYMMDD.txt
```

The marker should record the frozen source SHA and state that it is verification-only and must not be merged.

## PR description

A verification PR body should identify:

- exact frozen source/head SHA;
- purpose of verification;
- required gates;
- source boundary it supersedes if applicable;
- whether the PR itself is intended to merge or is marker-only.

For marker-only PRs, include the marker path and state that the marker must not be merged.

## Evidence requirements

Record at least:

```text
Verification PR:
Exact verified source/head SHA:
CareNest CI run:
Package-evidence tooling self-test:
Documentation-integrity checks:
Formatting:
Unit tests:
Integration tests:
UI/source-policy tests:
Android Release:
Windows Release:
iOS simulator Release:
Mac Catalyst Release:
Store Package Configuration run:
Store Inspection Artifacts run:
CodeQL run:
Dependency Audit run:
PR disposition:
```

For marker-only verification also record frozen base/source SHA and marker/head SHA.

## Production tag verification is separate

PR verification proves the exact candidate under PR automation. It is not the final production tag gate.

For an approved immutable `v*` tag, require the configured:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- CareNest Release Evidence.

The Release Gate must require current production evidence policy/templates/tooling to exist, but the canonical templates themselves do not prove real manual validation.

Production publication must also wait for applicable:

- final signed-package structured evidence JSON;
- real-device/manual evidence;
- accessibility evidence;
- packaged SQLite/document/backup compatibility evidence;
- production signing/notarization provenance;
- live store-console declarations/metadata;
- submission-date store-policy review;
- final store approval/publication evidence.

If release/tag workflows, tests, stable policies or release scripts change after PR verification, verify a new exact source before production use.

## Historical evidence rule

Never rewrite a failed/superseded verification checkpoint as successful release evidence.

Historical records may retain:

- successes;
- failures/skips/queued gates;
- root cause;
- fixing commit;
- replacement verification.

They remain authoritative only for their own exact source boundary.
