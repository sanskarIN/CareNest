# CareNest Release Evidence

This document defines the evidence that must exist before a CareNest source commit is promoted from release candidate to a public production release.

## Automated evidence

The exact release commit must have successful GitHub-hosted evidence for:

- `CareNest CI` core tests;
- Android Release compilation;
- Windows Release compilation;
- iOS simulator Release compilation;
- Mac Catalyst Release compilation;
- CodeQL;
- the repository Dependency Audit workflow;
- the platform-neutral formatting gate;
- the production Release Gate;
- the `CareNest Release Evidence` workflow.

The normal CI, CodeQL, and Dependency Audit workflows support:

- pull-request verification where applicable;
- manual `workflow_dispatch` execution;
- exact release-tag execution for tags matching `v*`.

This is intentional: a release tag must not bypass the same source/test/platform/security gates that protected the release candidate.

## Current verified automated baseline

The latest verified Gumroad implementation/source-policy baseline is:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

On that exact source:

- unit tests: **122/122**;
- integration tests: **39/39**;
- UI/source-policy tests: **175/175**;
- total core tests: **336/336**;
- Android Release build passed;
- Windows Release build passed;
- iOS simulator Release build passed;
- Mac Catalyst Release build passed;
- all four store-candidate configurations passed;
- CodeQL passed.

Authoritative record:

`docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`

Documentation-only commits after that exact implementation/source-policy source must not be described as exact-head automated evidence unless the applicable workflows have also completed successfully for the newer head.

## Release Evidence artifact contents

The manual/tag-triggered `CareNest Release Evidence` workflow records:

### Source identity/provenance

- exact Git commit SHA;
- Git ref;
- GitHub Actions run ID;
- GitHub Actions run attempt;
- .NET SDK/runtime information;
- pre-test Git status;
- post-test tracked-workspace Git status;
- tracked-file path manifest;
- SHA-256 checksum manifest for every tracked repository file at the evidence commit.

### Test evidence

- unit-test TRX output;
- integration-test TRX output;
- UI-contract/policy-test TRX output.

The three test steps use independent captured outcomes. A failure in one suite does not intentionally prevent later suites from attempting to produce their own evidence.

### Dependency evidence

Transitive dependency inventories are captured for:

- Shared;
- Domain;
- Application;
- Infrastructure;
- UnitTests;
- IntegrationTests;
- UiTests.

The separate Dependency Audit workflow remains the authoritative vulnerability-audit gate, including the Android MAUI application graph. The Release Evidence dependency inventory is provenance and inspection evidence; it does not replace the blocking audit workflow.

### Artifact integrity and failure preservation

- evidence files receive a SHA-256 evidence-manifest checksum;
- the evidence artifact is uploaded with `if: always()` so a failed release-evidence run retains the evidence that was successfully produced before/after the failure;
- the artifact upload occurs before the final aggregate outcome gate;
- the workflow fails after upload if unit, integration, UI-contract, dependency-inventory, or tracked-workspace-integrity evidence did not complete successfully;
- evidence artifacts are retained for 90 days by the workflow.

A failed evidence workflow is not release approval merely because an artifact exists. The artifact is retained specifically to make failed release verification diagnosable and auditable.

A successful evidence workflow does not replace platform builds, device testing, accessibility testing, store-policy review, signing, package inspection, or packaged existing-data compatibility testing.

## Manual evidence

The release owner must complete `docs/releases/MANUAL_TEST_MATRIX.md` on appropriately provisioned targets and retain at least:

- tested platform/OS version;
- device or emulator/simulator identity;
- app version/build;
- exact source commit;
- date of test;
- pass/fail result;
- notes for any limitation, workaround or blocked case.

Do not store user health records, real prescription documents, PINs, backup passwords or other sensitive test data in public release evidence.

## Security evidence

Before release:

- review `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- review `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`;
- confirm the SQLite native/provider dependency graph still satisfies the maintained package floor;
- confirm no new unsuppressed dependency advisory blocks the intended production graph;
- confirm `GHSA-2m69-gcr7-jv3q` has not been reintroduced through the old dependency path or hidden by a restored `NuGetAuditSuppress` entry;
- confirm no wildcard/severity-wide audit suppression was introduced;
- confirm no signing key, certificate, `.pfx`, `.p12`, `.jks`, keystore, `.env`, service credential or API secret was committed;
- confirm CodeQL completed successfully for the exact release head;
- confirm Dependency Audit completed successfully for the exact release head/tag;
- confirm packaged existing-database, encrypted-document, backup, and reminder compatibility evidence was recorded after dependency/provider changes.

The source remediation for the formerly tracked SQLite advisory is complete in the current RC1 graph. That source remediation must not be confused with the still-manual packaged existing-data compatibility gate.

## Store-policy evidence

A dated pre-submission policy review is recorded at:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

It records a 2026-08-18 review of the current CareNest product/package boundary against Apple, Google Play and Microsoft Store policy areas relevant to health functionality, sensitive-data privacy, application completeness and external-commerce placement.

This review is not store approval and is not the final submission-time policy check. Immediately before submitting the exact production package/listing, the release owner must:

- re-open the official current policy sources;
- complete the live store-console declarations for the exact production binary/capabilities/SDK behavior;
- record review date, source, conclusion and any required product/listing change;
- repeat affected exact-source verification if a source/package change is required.

## Current external-commerce evidence boundary

For `1.0.0-rc.1`, repository storefront/funding promotion remains separate from the CareNest application package.

The final application packages must contain no external application promotion or purchase surface for:

- `https://buymeacoffee.com/sanskarIN`;
- `https://ramsandesh.gumroad.com`.

This is stronger than deciding at release time whether a funding link is conditionally visible. Under the current policy, both destinations are repository/documentation-only.

For every final production package, retain evidence that:

- the exact package payload was scanned for `buymeacoffee.com/sanskarIN`;
- the exact package payload was scanned for `ramsandesh.gumroad.com`;
- no Gumroad/Buy Me a Coffee promotional card, command, button or artwork is present in the installed app;
- no health feature, reminder behavior, medical limitation or health-data access changes according to purchase/funding state;
- the package still exposes the intentional repository/creator/business/support/privacy/terms/security/notices surfaces documented by the current product design.

## Store and signing evidence

For each distribution channel:

- current store-policy review is complete for the exact submission package/listing;
- production signing credentials are provided only through the intended secure signing mechanism and remain outside Git;
- the signed package is produced from the exact verified commit;
- package identifier, version and build number match release metadata;
- requested permissions/capabilities match documented product behavior;
- screenshots/listing text do not claim diagnosis, dosage decisions, treatment recommendations, guaranteed reminders, medical-device status without applicable approval, or emergency-service behavior;
- privacy/data-safety disclosures match the local-first implementation and exact binary;
- final package filename, SHA-256 and signing/notarization/store-managed provenance are recorded;
- final installed-package smoke/manual validation is complete.

For Google Play, complete the live Health apps declaration and Data safety form for the exact production feature/binary set. For Apple and Microsoft distribution, complete the current privacy/store metadata required by the applicable submission channel.

## Exact-tag release behavior

When the approved `v*` tag is created, repository automation is expected to run against that exact tagged commit:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- CareNest Release Evidence.

A tag should not be considered release-approved until every required tag-triggered workflow is successful and the manual/signing/store evidence is complete.

If a tag-triggered required workflow fails:

1. preserve the failed evidence;
2. do not publish/promote the failing tag as a successful production release;
3. fix the source/configuration on a new commit;
4. repeat exact-source verification and manual checks as applicable;
5. create the corrected release tag only after approval.

## Release record template

For a promoted release, record:

```text
Version:
Tag:
Commit SHA:
CareNest CI run:
CodeQL run:
Dependency Audit run:
Store Package Configuration run:
Store Inspection Artifacts run:
Release Gate run:
Release Evidence run:
Release Evidence artifact:
Release Evidence SHA256SUMS:
Unit tests:
Integration tests:
UI-contract tests:
Android build:
Windows build:
iOS simulator build:
Mac Catalyst build:
Manual Android evidence:
Manual Windows evidence:
Manual iOS/iPadOS evidence:
Manual Mac Catalyst evidence:
Packaged SQLite existing-data compatibility:
Encrypted document compatibility:
Backup compatibility:
Accessibility review:
Store-policy review date/sources:
Google Play Health apps declaration:
Google Play Data safety:
Apple privacy metadata:
Microsoft privacy metadata:
SQLite dependency decision:
Signing/package review:
BMC package-marker scan:
Gumroad package-marker scan:
Final package SHA-256/provenance:
Release owner:
Release date:
```

A blank or blocked field remains a release blocker unless the release checklist explicitly documents why that field does not apply.
