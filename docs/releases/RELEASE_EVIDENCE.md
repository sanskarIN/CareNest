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
- the repository dependency-audit workflow;
- the platform-neutral formatting gate.

The manual/tag-triggered `CareNest Release Evidence` workflow also records:

- exact Git commit SHA;
- Git ref;
- .NET SDK/runtime information;
- unit-test TRX output;
- integration-test TRX output;
- UI-contract-test TRX output;
- transitive dependency inventories for Shared, Domain, Application and Infrastructure;
- SHA-256 checksums for the evidence files.

A successful evidence workflow does not replace platform builds, device testing, accessibility testing, store-policy review, signing or package inspection.

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
- confirm the exact SQLitePCLRaw advisory state;
- confirm no new high/critical unsuppressed dependency advisory is present;
- confirm the narrowly scoped SQLite audit suppression has not been broadened;
- confirm no signing key, certificate, `.pfx`, `.p12`, `.jks`, keystore, `.env`, service credential or API secret was committed;
- confirm CodeQL completed successfully for the exact release head.

The known SQLitePCLRaw advisory must never be described as fixed unless a compatible patched dependency/provider path has actually been adopted and verified.

## Store and signing evidence

For each distribution channel:

- current store-policy review is complete;
- the external voluntary Buy Me a Coffee link is allowed for that channel or is conditionally hidden/removed;
- signing credentials are provided only through the intended secure signing mechanism;
- the signed package is produced from the exact verified commit;
- package identifier, version and build number match release metadata;
- requested permissions match documented product behavior;
- screenshots/listing text do not claim diagnosis, dosage decisions, treatment recommendations, guaranteed reminders or emergency-service behavior;
- privacy/data-safety disclosures match the local-first implementation.

## Release record template

For a promoted release, record:

```text
Version:
Tag:
Commit SHA:
CareNest CI run:
CodeQL run:
Dependency audit run:
Release Evidence run:
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
Accessibility review:
Store-policy review:
SQLite advisory decision:
Signing/package review:
Release owner:
Release date:
```

A blank or blocked field remains a release blocker unless the release checklist explicitly documents why that field does not apply.
