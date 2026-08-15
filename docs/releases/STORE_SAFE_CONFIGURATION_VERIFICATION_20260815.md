# CareNest Store-Safe Configuration Verification — 2026-08-15

## Purpose

This document records the exact-head automated verification for the CareNest store-safe package-configuration continuation completed on 2026-08-15.

It supersedes PR #58 only as the latest exact automated source baseline. PR #58, PR #56, and PR #54 remain valid historical evidence for their own frozen source boundaries.

This document records source/build/test evidence. It does not claim completion of signed package generation, installed store-candidate inspection, device testing, accessibility testing, packaged existing-data compatibility, store approval, or final production publication.

## Frozen source boundary

Repository:

`https://github.com/sanskarIN/CareNest`

Frozen `main` source/base SHA:

`8489d19734d6142054156d5b57f2713195c16b65`

Verification branch:

`ci/carenest-store-safe-final-20260815`

Verification marker/head SHA:

`ca58294fb7f7a56ee87da16d938f0f691c3a3c7e`

Marker file:

`build/verification/store-safe-package-final-20260815.txt`

Pull request:

`https://github.com/sanskarIN/CareNest/pull/59`

PR title:

`Verify store-safe CareNest package configuration`

PR #59 contained exactly one changed file beyond the frozen source: the verification marker. GitHub reported 23 additions, 0 deletions, and one commit.

PR #59 was closed without merge after all required gates completed successfully. The marker is therefore not part of `main`.

## Source changes covered by PR #59

The frozen source includes all earlier RC1 runtime, privacy, reminder, document, backup, SQLite dependency, release-engineering, package metadata, and store-support-link hardening plus this continuation:

- `.github/workflows/store-package-verification.yml`;
- funding-disabled Release compilation for Android, Windows, iOS simulator, and Mac Catalyst;
- `v*`, pull-request, `main`/`release/**`, and manual workflow entry points for the store-safe configuration workflow;
- explicit `CARENEST_STORE_FUNDING_LINK=false` propagation into `CareNestShowFundingLink`;
- no unsigned artifact upload/publish behavior in that verification workflow;
- `build/scripts/store-package-preflight.sh`;
- `build/scripts/store-package-preflight.ps1`;
- fail-closed explicit target allow-list in both wrappers;
- forced `CARENEST_SHOW_FUNDING_LINK=false` in both wrappers;
- delegation to the existing release preflight rather than duplicating its test/audit logic;
- executable Git mode (`100755`) for the Bash wrapper;
- CI verification that the Bash wrapper remains executable;
- `StorePackageWorkflowContractTests`;
- `StorePackagePreflightContractTests`;
- release-workflow contract coverage requiring the store-safe workflow on exact `v*`/manual verification paths;
- updated `docs/releases/STORE_BUILD_POLICY.md` describing the automated and local store-safe paths.

## CareNest CI evidence

Workflow:

`CareNest CI`

Run number:

`#622`

Run ID:

`31869214132`

Conclusion:

**success**

### Platform-neutral formatting

Result:

**success**

### Unit tests

- Passed: **122**
- Failed: **0**
- Skipped: **0**

### Integration tests

- Passed: **39**
- Failed: **0**
- Skipped: **0**

### UI/source-policy tests

- Passed: **149**
- Failed: **0**
- Skipped: **0**

The UI/source-policy suite increased from 130 at PR #58 to 149 at PR #59 because the continuation added store-package workflow/preflight contracts and release-workflow coverage.

### Total core tests

- Passed: **310**
- Failed: **0**
- Skipped: **0**

### Default/open-source Release configuration

The normal/default application configuration passed:

- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**.

The default project behavior keeps `CareNestShowFundingLink=true` unless another value is supplied.

## Store-safe Release configuration evidence

Workflow:

`CareNest Store Package Configuration`

Run number:

`#11`

Run ID:

`31869214047`

Conclusion:

**success**

The workflow forced:

`CARENEST_STORE_FUNDING_LINK=false`

which was passed to:

`CareNestShowFundingLink`

### Funding-disabled target results

- Android Release with `CareNestShowFundingLink=false`: **success**;
- Windows Release with `CareNestShowFundingLink=false`: **success**;
- iOS simulator Release with `CareNestShowFundingLink=false`: **success**;
- Mac Catalyst Release with `CareNestShowFundingLink=false`: **success**.

The Android job also verified:

`test -x build/scripts/store-package-preflight.sh`

Result:

**success**

This protects the documented direct Bash wrapper command from accidental executable-bit loss.

## CodeQL evidence

Workflow:

`CodeQL`

Run number:

`#622`

Run ID:

`31869214042`

Conclusion:

**success**

## Dependency Audit evidence

Workflow:

`Dependency Audit`

Run number:

`#44`

Run ID:

`31869214093`

Conclusion:

**success**

The audit completed both the platform-neutral dependency graph and the Android MAUI application dependency graph without restoring the former SQLite advisory suppression.

## Verification conclusion

For frozen source SHA `8489d19734d6142054156d5b57f2713195c16b65`:

- formatting is green;
- 310/310 core tests are green;
- default Android/Windows/iOS simulator/Mac Catalyst Release builds are green;
- funding-disabled Android/Windows/iOS simulator/Mac Catalyst Release builds are green;
- the Bash store-package preflight executable-mode guard is green;
- CodeQL is green;
- unsuppressed Dependency Audit is green.

PR #59 is therefore the latest authoritative exact automated source baseline.

## What this proves

This exact-source evidence proves that:

- both the normal/default source configuration and the store-safe funding-disabled source configuration compile on every currently supported MAUI target used by CI;
- store-safe workflow configuration cannot silently use the default funding-link value under the verified source contracts;
- local store-package wrappers force the external funding surface off for their selected target;
- the Bash wrapper is executable in the verified Git tree;
- current source contracts and dependency/security gates remain green.

## What this does not prove

It does not prove that:

- a signed Apple App Store package has been produced;
- a signed Google Play package has been produced;
- a signed Windows or Mac distribution package has been produced;
- the About page has been manually inspected in an installed store artifact;
- store review will approve the application;
- real-device notification lifecycle behavior is complete;
- accessibility testing is complete;
- packaged SQLite upgrade compatibility is complete;
- historical encrypted document/backup fixtures are compatible;
- signing credentials/provenance are configured;
- final production `v*` tag workflows have run successfully.

## Relationship to current store policy decision

`docs/releases/STORE_POLICY_REVIEW_20260815.md` currently selects `CareNestShowFundingLink=false` for the initial Apple App Store and Google Play candidates unless a submission-time storefront/country/program-specific review clearly permits the external support link.

PR #59 now provides automated four-platform source compilation evidence for that funding-disabled configuration.

The policy must still be re-reviewed at actual submission time.

## Documentation-only movement after verification

Documentation-only commits made after frozen source SHA `8489d19734d6142054156d5b57f2713195c16b65` may record this already-completed evidence without changing the executable/project/test/workflow/build-script source verified by PR #59.

If any runtime, test, project, package, platform, workflow, or build/release-script source changes after this boundary, complete a fresh marker-only exact-head verification before production promotion.

## Remaining production blockers

Still open unless separately evidenced:

- installed Apple App Store candidate built with `CareNestShowFundingLink=false`;
- installed Google Play candidate built with `CareNestShowFundingLink=false`;
- packaged About-page inspection proving the external support card is absent;
- package identifiers/version/build metadata inspection on actual artifacts;
- package checksums/provenance;
- representative packaged SQLite existing-data upgrade/integrity/readability;
- canonical historical encrypted document/backup compatibility where fixtures exist;
- Android manual device/emulator matrix;
- Windows manual matrix;
- iOS/iPadOS real-device matrix;
- Mac Catalyst manual matrix;
- actual notification permission/delivery/restart/reboot/time-zone behavior;
- accessibility with representative assistive technologies;
- production signing identities/credentials outside Git;
- signed artifacts and signing/notarization provenance;
- store screenshots/listing/privacy/data-safety metadata;
- submission-time Apple/Google policy re-review;
- exact approved production `v*` tag;
- tagged CareNest CI, CodeQL, unsuppressed Dependency Audit, Store Package Configuration, Release Gate, and Release Evidence success;
- final publication evidence.

Do not call CareNest bug-free or production-published solely from this automated verification.