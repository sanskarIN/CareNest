# CareNest Release Evidence

This document defines the evidence that must exist before a CareNest source commit is promoted from release candidate to a public production release.

For the canonical result-state and evidence-quality rules, use:

- `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`;
- `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`.

Those documents define the `PASS`, `FAIL`, `BLOCKED`, `N/A`, and `NOT RUN` semantics used by production validation records. Unknown, stale, blocked, or unperformed work must never be represented as a pass.

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
- Store Package Configuration;
- Store Inspection Artifacts;
- the production Release Gate;
- the `CareNest Release Evidence` workflow.

The normal CI, CodeQL, Dependency Audit, package/store and release-evidence workflows support the repository's exact-source verification process. A release tag must not bypass the same source/test/platform/security gates that protected the release candidate.

## Current accepted automated baseline

The latest accepted exact automated source before the current production-evidence-readiness continuation is:

`30ee6c265104c64ec5a1a4013f592f7f058750e8`

Verified pull-request merge-ref:

`84fda5bb8ced9f4c487110e43652f51ba2d8d495`

Merged executable-source commit:

`2549c08b25145f20c59b7e73ca227c35d5bbf0ec`

Observed accepted evidence:

- CareNest CI run `32205946013`: **success**;
- unit tests: **122/122**;
- integration tests: **54/54**;
- UI/source-policy tests: **194/194**;
- total core tests: **370/370**;
- Android Release build: **success**;
- Windows Release build: **success**;
- iOS simulator Release build: **success**;
- Mac Catalyst Release build: **success**;
- Store Package Configuration run `32205946003`: **success**;
- Store Inspection Artifacts run `32205946001`: **success**;
- CodeQL run `32205946030`: **success**;
- unsuppressed Dependency Audit run `32205946026`: **success**.

Dynamic authority:

`docs/releases/AUTOMATED_BASELINE.md`

Backup hardening record frozen into that accepted source:

`docs/releases/BACKUP_RESOURCE_HARDENING_20260819.md`

A newer verification-relevant branch/head must not inherit these results merely because its changes are documentation-only. The current production-evidence-readiness continuation must complete its own required fresh matrix before it can replace the accepted source boundary.

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
- the evidence artifact is uploaded with failure-preservation behavior so a failed run retains evidence produced before/after the failure;
- artifact upload occurs before the final aggregate outcome gate;
- the workflow fails if required unit, integration, UI-contract, dependency-inventory, or tracked-workspace-integrity evidence did not complete successfully;
- evidence artifacts use the retention configured by the workflow.

A failed evidence workflow is not release approval merely because an artifact exists. A successful evidence workflow does not replace platform builds, device testing, accessibility testing, store-policy review, signing, package inspection, or packaged existing-data compatibility testing.

## Production validation records

Production validation must use the canonical evidence standard and reusable records under:

`docs/releases/templates/`

Current canonical records are:

- `ANDROID_DEVICE_VALIDATION_RECORD.md`;
- `WINDOWS_VALIDATION_RECORD.md`;
- `IOS_DEVICE_VALIDATION_RECORD.md`;
- `MACCATALYST_VALIDATION_RECORD.md`;
- `ACCESSIBILITY_VALIDATION_RECORD.md`;
- `PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`;
- `SIGNING_PROVENANCE_RECORD.md`;
- `STORE_SUBMISSION_RECORD.md`;
- `PRODUCTION_RELEASE_APPROVAL_RECORD.md`.

The templates are evidence containers, not evidence by themselves. Keep canonical templates unperformed. Create release-specific copies according to `docs/releases/PRODUCTION_EVIDENCE_INDEX.md` and record actual results there.

Use fictional or synthetic application data for public validation evidence. Do not commit real user health records, prescription documents, PINs, backup passwords, private signing keys, access tokens, recovery codes, or other secrets.

## Manual platform evidence

The release owner must complete representative platform validation on appropriately provisioned targets and retain at least:

- tested platform/OS version;
- device/emulator/simulator identity where applicable;
- exact package/app version and build;
- exact source commit;
- test date and time zone;
- individual result state;
- notes/evidence references for limitations, failures, blocked cases, or justified `N/A` items.

Simulator compilation is not a substitute for signed real-device iPhone/iPad notification evidence. Hosted compilation is not a substitute for actual installed-package behavior.

Use the platform records linked from `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`.

## Packaged compatibility evidence

Before production approval, validate the intended packages with fictional/synthetic representative existing data and record:

- SQLite open/integrity/migration behavior;
- profiles, medicines, schedules, occurrences, logs, appointments, stock, settings and related data behavior;
- encrypted-document compatibility;
- reminder reconciliation after upgrade/restore;
- current backup create/inspect/restore behavior;
- clean-install restore;
- wrong-password, tamper, truncation and trailing-data behavior;
- genuine historical encrypted backup compatibility where genuine prior bytes safely exist.

Current backup resource ceilings are documented by the backup architecture/hardening records and the canonical packaged compatibility template. Do not manufacture a current backup and label it historical evidence. Do not silently weaken a current security/resource boundary merely to make an unverified historical scenario pass.

## Accessibility evidence

Automated semantics/source checks are necessary but not sufficient. Retain actual representative evidence for:

- screen-reader behavior;
- focus/reading order;
- large text/text scaling;
- keyboard/input behavior on applicable desktop targets;
- light/dark/system contrast;
- color-independent state communication;
- reduced-motion behavior where applicable;
- privacy-safe error and validation messaging.

Use `docs/releases/templates/ACCESSIBILITY_VALIDATION_RECORD.md`.

## Security evidence

Before release:

- review `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- review `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`;
- confirm the SQLite native/provider dependency graph still satisfies the maintained package floor;
- confirm no new unsuppressed dependency advisory blocks the intended production graph;
- confirm `GHSA-2m69-gcr7-jv3q` has not been reintroduced through the old dependency path or hidden by a restored `NuGetAuditSuppress` entry;
- confirm no wildcard/severity-wide audit suppression was introduced;
- confirm no signing key, certificate, `.pfx`, `.p12`, `.jks`, keystore, `.env`, service credential, access token, recovery code or API secret was committed;
- confirm CodeQL completed successfully for the exact release head/tag;
- confirm Dependency Audit completed successfully for the exact release head/tag;
- confirm packaged existing-database, encrypted-document, backup and reminder compatibility evidence was recorded after dependency/provider changes.

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

Use `docs/releases/templates/STORE_SUBMISSION_RECORD.md` to separate policy review, metadata completion, submission, review, rejection, approval and publication states.

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

## Structured package checksum/provenance evidence

Use:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

Source-controlled tooling:

- `build/scripts/create-package-evidence.py`;
- `build/scripts/create-package-evidence.sh`;
- `build/scripts/create-package-evidence.ps1`;
- `build/scripts/test-create-package-evidence.py`.

For every final production package, generate and retain package evidence JSON using `--stage production`.

Production mode requires:

- an immutable `v*` source tag;
- the tag to resolve to the recorded full source SHA;
- checked-out HEAD to equal that source SHA;
- a clean tracked Git workspace;
- non-secret real signing/notarization/store-managed provenance text;
- successful store-safe scanning;
- SHA-256 evidence for the entire file or deterministic directory payload plus every contained file;
- evidence output outside the package payload.

The evidence tool does not sign artifacts and does not prove store approval. Its generated JSON is one part of the final release record and must be paired with actual platform signing/notarization/store evidence, manual package/device evidence and current store-review evidence.

The synthetic package-evidence self-test is run by CareNest CI and exercises success/fail-closed behavior without real user data or signing secrets.

## Store and signing evidence

For each distribution channel:

- current store-policy review is complete for the exact submission package/listing;
- production signing credentials are provided only through the intended secure signing mechanism and remain outside Git;
- the signed package is produced from the exact verified commit;
- package identifier, version and build number match release metadata;
- requested permissions/capabilities match documented product behavior;
- screenshots/listing text do not claim diagnosis, dosage decisions, treatment recommendations, guaranteed reminders, medical-device status without applicable approval, or emergency-service behavior;
- privacy/data-safety disclosures match the local-first implementation and exact binary;
- final package filename, SHA-256 and non-secret signing/notarization/store-managed provenance are recorded;
- structured package evidence JSON is generated and retained;
- final installed-package smoke/manual validation is complete.

Do not put private signing material, passwords, access tokens or recovery codes in the repository evidence record. Use `docs/releases/templates/SIGNING_PROVENANCE_RECORD.md` for safe provenance fields.

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

A tag must not be considered release-approved until every required tag-triggered workflow is successful and the manual/signing/store evidence is complete.

If a required tag-triggered workflow fails:

1. preserve the failed evidence;
2. do not publish/promote the failing tag as a successful production release;
3. fix the source/configuration on a new commit;
4. repeat exact-source verification and manual checks as applicable;
5. create the corrected release tag only after approval.

## Production release approval

The final production approval record is:

`docs/releases/templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`

A release-specific copy must aggregate the actual automated, package, device, accessibility, compatibility, security, signing, policy, submission and publication evidence. Green automation alone cannot mark the production release approved.

## Release record template

For a promoted release, retain at least:

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
UI/source-policy tests:
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
Accessibility evidence:
Store-policy review date/sources:
Google Play Health apps declaration:
Google Play Data safety:
Apple privacy metadata:
Microsoft privacy metadata:
SQLite dependency decision:
Signing/package review:
BMC package-marker scan:
Gumroad package-marker scan:
Package evidence JSON:
Package evidence payload SHA-256:
Final package SHA-256/provenance:
Store submission state:
Store approval/publication evidence:
Release owner:
Release date:
```

A blank, blocked, unknown or `NOT RUN` field remains a release blocker unless the release checklist and evidence standard explicitly document why that item is legitimately `N/A`.