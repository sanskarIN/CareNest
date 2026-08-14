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
- honest about known risks and limitations;
- explicit when a former risk has been remediated in source but still has separate packaged-compatibility evidence outstanding.

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

For workflows that span SQLite/filesystem/secure storage/OS scheduling, document compensation/reconciliation behavior instead of implying a single transaction exists across those independent state surfaces.

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

These documents must distinguish automated checks, manual checks, store-policy decisions, signing, exact-tag gates, evidence retention, and final release approval.

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

- deterministic reminder occurrence materialization;
- persisted CareNest reminder state; and
- operating-system notification scheduling/delivery.

Accurate wording can describe deterministic occurrence generation, explicit schedules, time-zone/DST behavior, effective snooze due time, cancellation/replacement ordering, compensation/recovery, and platform limitations.

Current consistency language should reflect that:

- `SnoozedUntilUtc` is the effective due time for a valid snooze;
- CareNest cancels an old platform request before replacement, suppression, invalidation, or handled-state persistence;
- platform cancellation failure remains retryable;
- medicine/profile/appointment flows use compensation where database and OS scheduler state can fail independently;
- handled reminder actions attempt non-cancelled previous-state/rebuild recovery if a later step fails.

Avoid unsupported promises such as `never miss a dose` or wording that treats database state as proof the OS request changed successfully.

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
- new encrypted document/backup streams use authenticated framing v2;
- legacy framing v1 remains readable for compatibility;
- app-lock verifier material is protected through secure storage;
- SQLite database is **not** claimed to have transparent whole-database encryption.

Do not use `fully encrypted` for the entire product unless a future implementation actually supports and verifies that claim.

## Dependency-risk wording

The former `GHSA-2m69-gcr7-jv3q` CareNest source exception is remediated in the current RC1 graph.

Accurate current wording:

- `sqlite-net-pcl` remains on the existing application API path;
- `SQLitePCLRaw.bundle_green` remains `2.1.11`;
- central transitive pinning selects maintained native/provider leaves;
- `SQLitePCLRaw.lib.e_sqlite3` is pinned to `3.53.3`;
- Android native/provider leaves and selected providers are pinned to `2.1.12`;
- the exact old `NuGetAuditSuppress` entry is removed;
- `SqliteDependencySecurityContractTests` protects the package floor/suppression absence;
- unsuppressed Dependency Audit is a required gate.

Rules:

- `NuGetAuditSuppress` is not a fix;
- do not call a dependency upgraded unless the resolved graph actually changed;
- do not describe source remediation as proof of packaged existing-database/encrypted-data compatibility;
- do not reintroduce the old suppression merely because manual packaged compatibility work remains incomplete;
- record exact package/provider evidence;
- keep `DEPENDENCY_RISK_REGISTER.md` authoritative.

## Verification wording

When citing automated verification include:

- exact source/base SHA;
- verification marker/head SHA when applicable;
- PR number;
- CI run number/ID;
- test counts;
- platform build results;
- CodeQL run;
- Dependency Audit run;
- marker-only/closed-without-merge status when using the exact-head protocol.

Do not attribute old verification to newer runtime/test/workflow/package/build-script source that has changed.

Workflow, test, build-script, package, and platform-configuration changes are verification-relevant even when application runtime code did not change.

## Documentation-only commits after a verified source

If changes after an exact verified source SHA are truly documentation-only:

- say they are documentation-only;
- do not claim the later documentation head itself was platform-build verified unless it was;
- use a commit comparison to prove no runtime/test/project/workflow/package/platform/build-script files changed;
- keep the verified source SHA separate from the documentation head SHA.

This documentation-only exception does **not** apply when tests, workflows, release scripts, package files, project files, or platform configuration changed.

## Manual evidence wording

Do not mark a manual device/accessibility/store/signing/packaged-data task complete unless it was actually performed.

`MANUAL_TEST_MATRIX.md` is an evidence record, not a statement of intent.

A clean NuGet audit is not packaged SQLite upgrade evidence. A green platform compile is not real notification-delivery evidence.

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
- match current CI-backed workload commands where practical;
- treat unsuppressed dependency audit as blocking in release/quality-gate examples;
- keep signing secrets outside command examples checked into Git.

## Git identity wording

Requested local maintainer identity:

- name: `Sanskar`;
- email: `sanskarin@outlook.in`.

Document this as **repository-local** Git configuration (`git config --local`).

Do not claim GitHub web/API/connector commits used that local email unless their commit metadata actually proves it. Authenticated connector commits can use the GitHub account identity.

## Release workflow wording

Production tags matching `v*` are intended to run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Release Evidence should be documented as:

- source/ref/run provenance;
- tracked-file manifest/checksums;
- all three core TRX suites;
- transitive dependency inventories;
- workspace integrity;
- evidence checksums;
- upload of available evidence even when a component fails;
- aggregate failure after the evidence upload;
- artifact identity containing commit SHA, run ID, and run attempt.

A failed Release Evidence run can have an artifact; artifact existence alone is never approval.

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
14. known limitations;
15. release workflow/evidence impact when applicable.

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
- tests/contracts;
- release workflow/evidence documentation if the security gate changes.

## Documentation review checklist

Before merging/pushing documentation:

- implementation claims are supported by source/evidence;
- no manual task is falsely checked complete;
- medical-safety wording is preserved;
- privacy/encryption wording is precise;
- dependency risk/remediation state is current;
- source remediation and packaged compatibility are not conflated;
- reminder persistence/OS scheduler reconciliation wording is current;
- exact-tag/release-evidence behavior matches workflow source;
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

## Current verification lineage

### Authoritative completed bug-audit baseline

PR #54 verified source/base SHA:

`4490f3f86752841d436e981b29279970c90c947b`

Evidence:

- CareNest CI #503 / `31766059137`: success;
- 122 unit + 39 integration + 100 UI-contract = 261 core tests;
- Android/Windows/iOS simulator/Mac Catalyst Release builds: success;
- CodeQL #503 / `31766059215`: success;
- unsuppressed Dependency Audit #35 / `31766059132`: success.

PR #54 was marker-only and closed without merge.

### Later release-engineering hardening

Release workflow, tests, quality/preflight scripts, and documentation changed after PR #54, so PR #54 cannot be used as verification for the newer release-engineering source.

Superseded PR #55 demonstrated 122 unit + 39 integration + 116 UI-contract = 277 core tests, Android/Windows, CodeQL #547 / `31769940053`, and unsuppressed Dependency Audit #38 / `31769940039` before further confirmed release-tooling/documentation fixes required a new exact-source verification.

The final current `main` head must receive a complete marker-only matrix before it becomes the new authoritative release-engineering baseline.
