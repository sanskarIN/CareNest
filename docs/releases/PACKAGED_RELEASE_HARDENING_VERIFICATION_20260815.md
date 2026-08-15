# CareNest Packaged Release Hardening Verification — 2026-08-15

## Purpose

This document records the exact-head automated verification for the 2026-08-15 CareNest packaged-release and store-policy hardening continuation.

It supersedes PR #56 only as the latest exact automated source baseline. PR #56 remains valid historical evidence for its frozen 2026-08-14 source boundary.

This document does not claim completion of real-device, accessibility, store-submission, signing, packaged existing-data, encrypted-data compatibility, or production-tag work.

## Frozen source boundary

Repository:

`https://github.com/sanskarIN/CareNest`

Frozen `main` source/base SHA:

`826b79925dad4402f65fccfecd4a29b353b6e2f3`

Verification branch:

`ci/carenest-packaged-release-final-20260815`

Verification marker/head SHA:

`b92e3b79857db2f6cb8346fb881fe65b43f8453b`

Marker file:

`build/verification/packaged-release-store-policy-final-20260815.txt`

Pull request:

`https://github.com/sanskarIN/CareNest/pull/58`

PR title:

`Verify 2026-08-15 packaged release hardening source`

PR #58 contained exactly one changed file: the verification marker. GitHub reported 9 additions, 0 deletions and 1 commit.

PR #58 was closed without merge after all required gates completed successfully. The verification marker is therefore not part of `main`.

## Source changes covered by this verification

The frozen source includes the complete earlier CareNest RC1 runtime/test/dependency/release-engineering graph plus the 2026-08-15 continuation through `826b79925dad4402f65fccfecd4a29b353b6e2f3`.

The continuation includes:

- build-configurable voluntary project-support visibility through `CareNestShowFundingLink`;
- default support visibility for normal/open-source builds;
- fail-closed store packaging through `CareNestShowFundingLink=false`;
- About-page visibility binding for the complete support card;
- UI/source-policy regression coverage for the store-specific funding switch;
- package metadata/privacy contracts for product identity, target frameworks, minimum OS versions, Android local-first permission/backup/cleartext posture, Apple purpose strings/transport posture, Windows package metadata and branding assets;
- release-preflight propagation of `CARENEST_SHOW_FUNDING_LINK=true|false`;
- `docs/releases/STORE_BUILD_POLICY.md`;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`;
- current source-boundary/handoff/changelog/documentation/next-step alignment through the frozen base SHA.

## CareNest CI evidence

Workflow:

`CareNest CI`

Run number:

`#608`

Run ID:

`31867245796`

Conclusion:

**success**

### Platform-neutral formatting

Result:

**success**

### Unit tests

Project:

`tests/CareNest.UnitTests/CareNest.UnitTests.csproj`

Result:

- Passed: **122**
- Failed: **0**
- Skipped: **0**

### Integration tests

Project:

`tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj`

Result:

- Passed: **39**
- Failed: **0**
- Skipped: **0**

### UI/source-policy tests

Project:

`tests/CareNest.UiTests/CareNest.UiTests.csproj`

Result:

- Passed: **130**
- Failed: **0**
- Skipped: **0**

The six-test increase over PR #56 comes from the new funding-link build-configuration contract plus package metadata/privacy contract coverage.

### Total core tests

- Passed: **291**
- Failed: **0**
- Skipped: **0**

## Platform Release build evidence

The same PR #58 CareNest CI run completed:

- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**.

These are compile/build gates. They do not replace real packaged signing, installation, notification delivery, accessibility or existing-data compatibility checks.

## CodeQL evidence

Workflow:

`CodeQL`

Run number:

`#608`

Run ID:

`31867245799`

Conclusion:

**success**

## Dependency Audit evidence

Workflow:

`Dependency Audit`

Run number:

`#43`

Run ID:

`31867245800`

Conclusion:

**success**

The audit completed both the platform-neutral dependency graph and the MAUI dependency graph without restoring the former SQLite advisory suppression.

## Verification conclusion

For frozen source SHA `826b79925dad4402f65fccfecd4a29b353b6e2f3`:

- formatting is green;
- 291/291 core tests are green;
- Android Release is green;
- Windows Release is green;
- iOS simulator Release is green;
- Mac Catalyst Release is green;
- CodeQL is green;
- unsuppressed Dependency Audit is green.

PR #58 is therefore the authoritative latest exact automated source baseline for the 2026-08-15 packaged-release/store-policy hardening continuation.

## Documentation-only movement after verification

Documentation commits made after the frozen source SHA may record this evidence or current policy decisions without changing the executable/project/test/workflow/build-script source that PR #58 verified.

If any runtime, test, project, package, platform, workflow or build/release-script source changes after this verified boundary, complete a fresh marker-only exact-head verification before production promotion.

## Remaining production gates

Still open unless separately evidenced:

- representative packaged SQLite existing-data upgrade/integrity/readability;
- canonical historical encrypted document/backup compatibility where fixtures exist;
- Android manual device/emulator matrix;
- Windows manual matrix;
- iOS/iPadOS real-device matrix;
- Mac Catalyst manual matrix;
- actual notification permission/delivery/restart/reboot/time-zone behavior;
- accessibility with representative assistive technologies;
- final per-store support-link policy/package decision and packaged inspection;
- production signing identities/credentials outside Git;
- signed package generation and provenance;
- store screenshots/listing/privacy/data-safety metadata;
- exact approved production `v*` tag;
- successful tagged CareNest CI, CodeQL, unsuppressed Dependency Audit, Release Gate and Release Evidence;
- final artifact checksums and publication evidence.

Do not call CareNest bug-free or production-published solely from this automated verification.