# CareNest Documentation Standards

This document defines how CareNest documentation should be created, updated, reviewed, and tied to implementation evidence.

## Goals

CareNest documentation should be:

- accurate to the implemented source;
- explicit about local-first/privacy/security boundaries;
- explicit about the non-clinical product scope;
- clear about what is automated evidence vs manual release evidence;
- discoverable from `docs/README.md` and root `README.md`;
- maintained alongside behavior changes;
- honest about known risks and limitations.

## Canonical documentation hub

`docs/README.md` is the primary navigation index for the documentation set.

Every major new user/developer/security/release document should be linked there.

## Documentation layers

### User documentation

Examples:

- `docs/USER_GUIDE.md`;
- `docs/FEATURE_REFERENCE.md`;
- `docs/REPORTS_AND_EXPORTS.md`;
- root privacy/terms/support files.

User documentation should describe observable behavior without implementation-only jargon unless it helps explain a limitation.

### Architecture documentation

Examples:

- `docs/architecture/ARCHITECTURE.md`;
- service boundaries;
- application flows;
- database schema;
- backup/restore;
- document vault;
- notification/platform behavior.

Architecture documentation should explain ownership, trust boundaries, data flow, failure modes, and project-layer responsibilities.

### Security/privacy documentation

Examples:

- `docs/security/SECURITY_MODEL.md`;
- `docs/security/THREAT_MODEL.md`;
- `docs/security/LOGGING_PRIVACY.md`;
- `docs/privacy/PRIVACY_MODEL.md`;
- dependency risk register.

Security/privacy documents must distinguish controls from residual risk and never imply guarantees beyond the implementation.

### Testing/release documentation

Examples:

- `docs/testing/TESTING_GUIDE.md`;
- reminder scheduling contract;
- release process;
- quality gate;
- manual matrix;
- security release review;
- store submission checklist;
- release evidence/protocol.

These documents must distinguish automated checks, manual checks, store-policy decisions, signing, and final release approval.

## Medical-safety wording

CareNest is organizational only.

Documentation must not claim or imply that CareNest:

- diagnoses;
- calculates/infers dosage;
- recommends treatment;
- checks medication interactions as a clinical guarantee;
- creates clinical risk scores;
- independently verifies adherence;
- provides emergency services;
- guarantees notification delivery.

Medicine strength/instruction text remains user-entered opaque text.

## Reminder wording

Documentation should distinguish:

- deterministic reminder occurrence materialization; and
- operating-system notification delivery.

Accurate wording can describe deterministic occurrence generation, explicit schedules, time-zone/DST behavior, and platform limitations.

Avoid unsupported promises such as `never miss a dose`.

## Local-first wording

Accurate v1 wording:

- no required CareNest account/backend;
- no automatic CareNest cloud sync/upload;
- local structured storage;
- encrypted imported documents;
- manual encrypted backups;
- explicit outbound export/share boundaries.

Do not claim that the OS never creates backups/caches or that exported copies remain local after the user sends them elsewhere.

## Encryption wording

Be precise:

- imported document payloads are encrypted;
- manual backups are encrypted;
- app-lock verifier material is protected through secure storage;
- SQLite database is **not** claimed to have transparent whole-database encryption.

Do not use `fully encrypted` for the entire product unless a future implementation actually supports and verifies that claim.

## Dependency-risk wording

Known advisory `GHSA-2m69-gcr7-jv3q` remains open for the current SQLitePCLRaw native path until actually resolved/approved.

Rules:

- `NuGetAuditSuppress` is not a fix;
- do not call a dependency upgraded unless the graph actually changed;
- record exact package/provider evidence;
- keep `DEPENDENCY_RISK_REGISTER.md` authoritative.

## Verification wording

When citing automated verification include:

- exact source SHA;
- PR number;
- CI run number/ID;
- test counts;
- platform build results;
- CodeQL run;
- Dependency Audit run;
- marker-only/closed-without-merge status when using the exact-head protocol.

Do not attribute old verification to newer runtime source that has changed.

## Documentation-only commits after a verified source

If changes after an exact verified source SHA are documentation-only:

- say they are documentation-only;
- do not claim the later documentation head itself was platform-build verified unless it was;
- use a commit comparison to prove no runtime/test/project/workflow/package/platform files changed;
- keep the verified runtime source SHA separate from the documentation head SHA.

## Manual evidence wording

Do not mark a manual device/accessibility/store/signing task complete unless it was actually performed.

`MANUAL_TEST_MATRIX.md` is an evidence record, not a statement of intent.

## Store policy wording

Store policies can change.

Documentation should say current policy must be reviewed at submission time instead of freezing a possibly stale rule as permanent truth.

This is especially important for:

- health-app categorization;
- external funding/project-support links;
- permissions;
- privacy/data-safety forms;
- payment/funding rules.

## Funding wording

Canonical project-support URL:

`https://buymeacoffee.com/sanskarIN`

Funding is voluntary project support only.

Do not imply funding:

- buys medical functionality;
- changes reminder delivery/priority;
- unlocks local user data;
- provides emergency support;
- creates a medical support entitlement.

## Security/privacy examples

Use synthetic/fictional data in:

- screenshots;
- docs examples;
- tests;
- public bug reports;
- release-store graphics.

Never add real health documents/backups/PINs/passwords/signing credentials to documentation.

## Commands

Commands in documentation should:

- match the current project layout;
- use the repository's `CareNestTargetFramework` pattern for MAUI target builds;
- avoid assuming all platform workloads are installed on every host;
- keep signing secrets outside command examples checked into Git.

## Links

Prefer relative repository links for repository-local documentation.

External canonical URLs/emails should remain consistent:

- repository: `https://github.com/sanskarIN/CareNest`;
- creator: `https://www.github.com/sanskarIN`;
- support: `supportramsandesh@gmail.com`;
- business: `sanskarin@outlook.in`;
- funding: `https://buymeacoffee.com/sanskarIN`.

## New feature documentation checklist

For a new feature, document where applicable:

1. user-visible purpose;
2. local/remote data flow;
3. persisted fields/schema;
4. service/layer ownership;
5. permissions/platform behavior;
6. security/privacy considerations;
7. backup/restore behavior;
8. export/delete behavior;
9. notification behavior;
10. accessibility/localization;
11. automated tests;
12. manual release checks;
13. store disclosure changes;
14. known limitations.

## Schema changes

A schema change should update:

- `DATABASE_SCHEMA.md`;
- architecture/application flow if relevant;
- backup/restore compatibility docs;
- privacy/data lifecycle docs if new data category;
- testing guide/fixtures;
- release notes/status/checklists.

## Security-sensitive changes

A security-sensitive change should review/update:

- `SECURITY_MODEL.md`;
- `THREAT_MODEL.md`;
- `LOGGING_PRIVACY.md` if logging changes;
- dependency risk register if dependencies change;
- security release review;
- tests/contracts.

## Documentation review checklist

Before merging/pushing documentation:

- implementation claims are supported by source/evidence;
- no manual task is falsely checked complete;
- medical-safety wording is preserved;
- privacy/encryption wording is precise;
- known dependency risk remains accurate;
- links work conceptually/paths exist;
- current baseline evidence numbers are consistent;
- new docs are linked from `docs/README.md`;
- root README points to the documentation hub;
- changelog/status/handoff are updated when appropriate.

## Handoff record

`what_changed.md` is the detailed implementation/handoff record used for long-running project continuation.

It should record:

- important commits;
- source fixes;
- verification failures/successes;
- exact run evidence;
- documentation/release changes;
- remaining blockers;
- environment limitations.

It must remain factual and must not describe unperformed work as completed.

## Current verified source baseline

Latest exact runtime/test source baseline:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

Verified through PR #30 with CI #248, CodeQL #248, Dependency Audit #10, 141 core tests, and Android/Windows/iOS simulator/Mac Catalyst Release builds successful.

Documentation changes after that baseline do not replace the runtime source SHA unless runtime/test/project/workflow/package/platform files are changed and reverified.