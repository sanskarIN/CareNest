# Exact-Head Verification Branch Protocol

CareNest uses temporary marker-only pull requests when fresh PR-triggered automated evidence is needed for an exact intended `main` source state without merging verification artifacts into production source.

## Why this exists

A previously green CI run does not prove a later verification-relevant commit is green.

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
- stable policy documents whose content is parsed by executable/source-policy tests.

## Dynamic evidence versus stable policy

CareNest deliberately separates mutable verification evidence from stable executable policy inputs.

Canonical dynamic automated-baseline pointer:

`docs/releases/AUTOMATED_BASELINE.md`

Dynamic evidence/status documents may include:

- `docs/releases/AUTOMATED_BASELINE.md`;
- dated verification records under `docs/releases/`;
- `PROJECT_STATUS.md`;
- `docs/releases/NEXT_STEPS.md`;
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`;
- `docs/DOCUMENTATION_CATALOG.md`;
- `docs/README.md`;
- `CHANGELOG.md`;
- `what_changed.md`.

These files may be updated **after** a successful exact-source verification to record that result as documentation-only evidence promotion, provided the promotion does not also change runtime/test/project/workflow/build-script/stable-policy source.

Executable release-documentation consistency tests must not assert mutable SHA/test-count text inside those dynamic evidence files. They may verify stable policy invariants and the existence/path of the canonical dynamic baseline record.

This avoids a self-referential loop where recording successful verification would itself change a tested evidence value and require another verification solely to record the first result.

## Stable release-policy inputs

Stable policy documents parsed for invariant contracts include current release process/store/security/package rules such as:

- `docs/releases/STORE_BUILD_POLICY.md`;
- `docs/releases/RELEASE_EVIDENCE.md`;
- `docs/releases/RELEASE_PROCESS.md`;
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md`;
- `docs/releases/QUALITY_GATE.md`;
- `docs/releases/SECURITY_RELEASE_REVIEW.md`;
- `docs/releases/MANUAL_TEST_MATRIX.md`;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`;
- this protocol.

If one of these stable policy inputs changes, the exact source requires fresh verification.

Stable policy documents should avoid treating a mutable source SHA/test count as a value that must be edited after every successful verification. Use `docs/releases/AUTOMATED_BASELINE.md` and the latest dated verification record for the current accepted automated result.

## Current accepted automated baseline

Use:

`docs/releases/AUTOMATED_BASELINE.md`

That dynamic record identifies the latest accepted exact-source automated baseline and its authoritative dated evidence.

Do not predict or publish a newer test total merely by counting added test methods. Record only the count actually reported by the exact verification run.

## Marker-only procedure

1. Finish all intended verification-relevant changes on `main`.
2. Update dynamic handoff/status documentation needed **before** the verification freeze.
3. Record the exact `main` SHA to verify.
4. Do not make further verification-relevant changes while that checkpoint is running.
5. Create a temporary branch from that exact SHA.
6. Add exactly one marker file under `build/verification/`.
7. Open a pull request from the marker branch to `main`.
8. Confirm the PR changes only the marker file beyond the frozen base SHA.
9. Require the full applicable PR automation:
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
10. Record exact run IDs, actual test counts, platform outcomes, source/base SHA, marker/head SHA, and PR number in a dated verification record.
11. Close the marker PR without merge after evidence is recorded.
12. Ensure the marker file never enters `main`.
13. Promote the completed result only through dynamic evidence/status documents unless a real verification-relevant source change is required.

## If a gate fails

Do not suppress/ignore a legitimate failure to preserve the checkpoint.

Instead:

1. inspect the exact failing job/log;
2. determine whether the defect is source, test, workflow, package, toolchain, stable documentation contract or infrastructure;
3. preserve completed/failed evidence accurately;
4. close the marker PR as failed/superseded when its source must change;
5. fix legitimate verification-relevant defects on `main`;
6. update dynamic handoff/status material as needed before the replacement freeze;
7. create a new marker branch from the corrected exact `main` SHA;
8. rerun the complete required matrix.

Partial green evidence from a failed/superseded checkpoint can be retained historically, but it is not the final baseline.

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
- stable repository policy source parsed by executable tests.

If these change, the running checkpoint is stale for the newer source. Close/supersede it and verify the new exact head.

### Post-verification dynamic evidence promotion

After a checkpoint has completed successfully, the dynamic evidence/status documents named above may be updated to record the completed result without treating the evidence-only commit as a new executable baseline, provided an exact comparison confirms there are no changes to runtime/test/project/workflow/package/platform/build-script/stable-policy source.

In that case:

- keep the verified source SHA explicit;
- identify the later evidence/documentation head separately if useful;
- do not claim that the evidence-only head itself ran platform workflows;
- keep `AUTOMATED_BASELINE.md` pointed at the exact source that actually ran;
- if there is any verification-relevant change or ambiguity, verify a new exact source.

## Marker naming

Use a descriptive path such as:

```text
build/verification/<purpose>-YYYYMMDD.txt
```

The marker should record the frozen source SHA and state that it is verification-only and must not be merged.

## PR description

The PR body should identify:

- frozen base/source SHA;
- marker/head SHA when available;
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

## Superseded checkpoint example — PR #77

Verification PR #77 froze source:

`0e894ef57d3b9bfb9297cd5d3cda37ac7abccc3d`

Marker/head:

`4d8e521dff85ae3f4887782ed313c79985de0295`

Observed before supersession:

- Dependency Audit run `32132538649`: success;
- CareNest CI run `32132538608`: queued;
- CodeQL run `32132538582`: queued;
- Store Package Configuration run `32132538730`: queued;
- Store Inspection Artifacts run `32132538637`: queued.

PR #77 was closed without merge because verification review discovered that mutable post-verification evidence/status files were being parsed for mutable SHA/test-count assertions. Recording successful evidence would therefore have created an unnecessary recursive verification loop.

That governance defect is corrected by separating dynamic evidence from stable policy inputs. PR #77 remains historical/superseded evidence and must not be represented as a completed verification baseline.

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

The Release Gate and Release Evidence workflows also verify package-evidence tooling source/self-test, but they do not create or sign final production packages.

Production publication must wait for all required tag-triggered workflows plus applicable:

- final signed-package structured evidence JSON;
- real-device/manual evidence;
- accessibility evidence;
- packaged SQLite/document/backup compatibility evidence;
- production signing/notarization provenance;
- live store-console declarations/metadata;
- submission-date store-policy review;
- final store approval/publication evidence.

If release/tag workflows, tests, stable policies or release scripts change after marker verification, create a new exact-head verification before using those changes for production.

## Historical evidence rule

Never rewrite a failed/superseded marker PR as successful release evidence.

Document what actually happened:

- successes;
- failures/skips/queued gates;
- root cause;
- fixing commit;
- replacement verification.

This keeps release history useful for future debugging and prevents a green subset from being confused with a fully green matrix.
