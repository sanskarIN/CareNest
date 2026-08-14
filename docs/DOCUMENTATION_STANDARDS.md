# CareNest Documentation Standards

This document defines how CareNest documentation is created, reviewed, updated, verified, and tied to implementation evidence.

## 1. Goals

CareNest documentation must be:

- accurate to implemented source;
- explicit about local-first/privacy/security boundaries;
- explicit about the non-clinical product scope;
- clear about automated evidence versus manual release evidence;
- discoverable from `docs/README.md`;
- maintained alongside behavior/configuration changes;
- honest about known limitations and historical defects;
- precise about dependency security versus packaged data compatibility;
- precise about platform notification limitations;
- explicit about the exact source SHA behind verification claims.

## 2. Canonical documentation entry points

Primary complete references:

- `docs/COMPLETE_PROJECT_DOCUMENTATION.md` — end-to-end project reference;
- `docs/CODEBASE_REFERENCE.md` — source/API/project/file map;
- `docs/CONFIGURATION_REFERENCE.md` — package/build/workflow/platform configuration;
- `docs/MAINTENANCE_AND_OPERATIONS.md` — maintainer lifecycle;
- `docs/README.md` — canonical documentation hub;
- `docs/releases/DOCUMENTATION_AUDIT_20260814.md` — documentation completeness/evidence audit.

Root public entry point: `README.md`.

## 3. Current authoritative automated baseline

When current automated evidence is needed, use PR #56:

- source/base: `4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`;
- marker head: `e3bc621cea05364a69abee0dadbd71a67c17bddb`;
- CareNest CI #571 / `31770929379`: success;
- 122 unit + 39 integration + 124 UI-contract/policy = 285 total;
- Android/Windows/iOS simulator/Mac Catalyst Release builds: success;
- CodeQL #571 / `31770929382`: success;
- unsuppressed Dependency Audit #41 / `31770929383`: success.

PR #56 was closed without merge and its marker is not part of `main`.

PR #54 is historical authoritative runtime bug-audit evidence for the earlier 261-test source graph. Do not incorrectly attribute PR #54 to later release-engineering source.

PR #43 is historical failed-core evidence and must not be described as fully green.

## 4. Documentation layers

### User documentation

Examples:

- `docs/USER_GUIDE.md`;
- `docs/FEATURE_REFERENCE.md`;
- `docs/REPORTS_AND_EXPORTS.md`;
- root privacy/terms/support files.

User documentation should describe observable behavior in clear language while preserving limitations.

### Architecture documentation

Examples:

- `docs/architecture/ARCHITECTURE.md`;
- `APPLICATION_FLOWS.md`;
- `SERVICE_BOUNDARIES.md`;
- `DATABASE_SCHEMA.md`;
- backup/document/notification architecture;
- ADRs.

Architecture docs should describe ownership, dependency direction, trust/data boundaries, failure modes and compensation rather than implying false atomicity.

### Security/privacy documentation

Examples:

- `docs/security/SECURITY_MODEL.md`;
- `docs/security/THREAT_MODEL.md`;
- `docs/security/LOGGING_PRIVACY.md`;
- `docs/privacy/PRIVACY_MODEL.md`;
- dependency risk register.

Security/privacy docs must distinguish controls from residual risk and must not overstate guarantees.

### Development/configuration documentation

Examples:

- `docs/setup/DEVELOPMENT.md`;
- `docs/setup/PLATFORM_SETUP.md`;
- `docs/CONFIGURATION_REFERENCE.md`;
- `docs/MAINTENANCE_AND_OPERATIONS.md`.

Commands and versions must match the current repository.

### Testing/release documentation

Examples:

- `docs/testing/TESTING_GUIDE.md`;
- release process/checklist;
- quality gate;
- manual matrix;
- security release review;
- store submission checklist;
- Release Evidence/verification protocol.

These documents must distinguish automated source verification from manual/device/store/signing/package evidence.

## 5. Medical-safety wording

CareNest is organizational software.

Documentation must not claim or imply that CareNest:

- diagnoses;
- calculates or infers dosage;
- recommends treatment;
- provides a clinical medication-interaction guarantee;
- creates clinical risk scores;
- independently verifies adherence;
- replaces qualified professionals;
- provides emergency services;
- guarantees reminder delivery.

Medicine strength/instruction text remains user-entered opaque text.

Avoid phrases such as `never miss a dose`, `safe dosage`, `recommended medicine`, or equivalent unsupported clinical claims.

## 6. Reminder wording

Documentation must distinguish:

1. explicit user schedule intent;
2. persisted CareNest occurrence state;
3. operating-system scheduled request/delivery state.

Current wording should reflect:

- deterministic explicit-input-only planning;
- explicit ownership/time-zone/UTC/date/state validation;
- half-open planning windows;
- no invented DST-gap replacement time;
- future-UTC snooze rule;
- `SnoozedUntilUtc` effective due time;
- platform request cancellation before replacement/suppression/invalidation;
- cancellation-first handled state transitions;
- retryable cancellation failure;
- medicine/profile/appointment compensation;
- platform delivery limitations.

Database state alone is not proof of OS scheduler state.

## 7. Local-first wording

Accurate current v1 wording:

- no required CareNest account/backend;
- no automatic CareNest cloud sync/upload;
- no hidden runtime telemetry client;
- local structured storage;
- encrypted imported document payloads;
- manual encrypted backups;
- explicit outbound export/share/calendar/browser boundaries.

Do not claim that the OS never creates caches/backups or that external exported copies remain under CareNest control.

## 8. Encryption wording

Be precise:

- imported document payloads are encrypted;
- manual backups are encrypted;
- new encrypted document/backup streams use chunked authenticated framing v2;
- legacy framing v1 remains readable where compatibility requires it;
- platform secure storage holds app-lock/document key material;
- application-owned mutable cryptographic buffers are cleared where practical;
- SQLite is **not** claimed to have transparent whole-database encryption.

Do not say `everything is fully encrypted` unless a future implementation truly provides and verifies that property.

## 9. Dependency security wording

The former `GHSA-2m69-gcr7-jv3q` source exception is remediated.

Current SQLite package intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- central transitive pinning;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- Android native/provider and selected provider leaves `2.1.12`;
- old exact advisory suppression removed;
- package floor/suppression absence protected by `SqliteDependencySecurityContractTests`;
- unsuppressed Dependency Audit is blocking.

Rules:

- `NuGetAuditSuppress` is not remediation;
- do not call a dependency upgraded unless the resolved graph actually changed;
- source dependency remediation does not prove packaged existing-data compatibility;
- do not restore the old suppression just because manual compatibility work is pending;
- keep `DEPENDENCY_RISK_REGISTER.md` authoritative for dependency risk state.

## 10. Verification wording

When citing an exact-head verification, include as applicable:

- source/base SHA;
- marker/head SHA;
- PR number;
- CI run number/ID;
- test counts;
- platform build results;
- CodeQL run;
- Dependency Audit run;
- marker-only and closed-without-merge status.

Do not reuse verification evidence after runtime/test/project/workflow/package/platform/build-script source changes.

Workflow, test, build-script, package and platform configuration changes are verification-relevant even when app runtime behavior is not intentionally changed.

## 11. Documentation-only commits after verified source

If commits after a verified source are truly documentation-only:

- call them documentation-only;
- do not claim the later doc commit itself was platform compiled unless it was;
- use commit comparison when making a release decision;
- keep the verified source SHA distinct from the documentation head;
- consider a final documentation-policy marker verification when repository policy tests consume docs.

This exception does not apply to tests, workflows, package/project files, platform configuration or build/release scripts.

## 12. Historical evidence

Do not erase failure history to make the project look cleaner.

Historical artifacts can describe old dependency exceptions, test counts, PR failures and source limitations if their historical context is explicit.

When replacing a stale active document, preserve the old exact version under `docs/history/` when practical.

Current active docs must clearly identify the latest authoritative state.

## 13. Manual evidence wording

Do not mark device/accessibility/store/signing/packaged-data work complete unless it actually occurred.

`MANUAL_TEST_MATRIX.md` is an evidence record, not an aspiration checklist that can be checked from CI.

Examples:

- green Android build ≠ real Android notification delivery;
- green NuGet audit ≠ packaged existing SQLite database upgrade proof;
- source-level semantic accessibility checks ≠ screen-reader certification;
- Release Evidence artifact existence ≠ successful Release Evidence gate if the workflow failed.

## 14. Store-policy wording

Store policies change.

Documentation should require current submission-time review rather than freezing possibly stale rules as permanent truth.

This applies particularly to:

- health categorization;
- privacy/data-safety declarations;
- permissions;
- external voluntary project-support links;
- payment/funding rules;
- screenshots/listing claims.

## 15. Funding wording

Canonical voluntary project-support URL:

`https://buymeacoffee.com/sanskarIN`

Funding must not be represented as:

- medical functionality;
- higher reminder priority;
- emergency assistance;
- access to local records;
- paid health advice;
- a clinical/support entitlement.

## 16. Security/privacy examples and test data

Use synthetic/fictional data in:

- documentation examples;
- screenshots;
- automated tests;
- public bug reports;
- store graphics;
- migration/backup fixtures.

Never put real health documents/backups/PINs/passwords/keys/signing credentials in documentation or Git.

## 17. Command accuracy

Commands in documentation should:

- match current repository paths;
- use current target frameworks;
- use `CareNestTargetFramework` for target-specific MAUI build isolation;
- not assume every platform workload exists on every host;
- reflect current CI/workload patterns where practical;
- treat unsuppressed dependency audit as blocking;
- keep signing secrets outside Git.

## 18. Git identity wording

Requested local maintainer identity:

- name: `Sanskar`;
- email: `sanskarin@outlook.in`.

Document this as repository-local Git configuration with `git config --local`.

Do not claim GitHub web/API/connector commits used that email unless actual commit metadata supports it.

## 19. Release workflow wording

Production tags matching `v*` run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Release Evidence should be documented as containing:

- commit/ref/run provenance;
- run attempt;
- tracked-file manifest/checksums;
- all three core TRX suites;
- transitive dependency inventories;
- workspace integrity;
- evidence checksums;
- failure-preserving upload before aggregate failure;
- artifact identity containing commit SHA, run ID and run attempt.

A failed Release Evidence run may have an artifact; that artifact is diagnostic evidence, not release approval.

## 20. Release Gate wording

Release Gate is fail-closed for:

- required documents;
- open dependency risk;
- unchecked applicable release checklist items;
- core tests.

Do not describe formatting changes to Markdown checklist rows as a way to bypass the gate. Source contracts protect nested/case/indentation behavior.

## 21. Links

Prefer relative links for repository-local documentation.

Canonical external identity values:

- repository: `https://github.com/sanskarIN/CareNest`;
- creator: `https://www.github.com/sanskarIN`;
- business: `sanskarin@outlook.in`;
- support: `supportramsandesh@gmail.com`;
- voluntary support: `https://buymeacoffee.com/sanskarIN`.

## 22. New feature documentation checklist

For a new feature, document where applicable:

1. user-visible purpose;
2. non-clinical boundary;
3. local/external data flow;
4. stored fields/schema;
5. layer/service ownership;
6. permissions/platform behavior;
7. security/privacy implications;
8. backup/restore behavior;
9. export/delete behavior;
10. reminder behavior;
11. accessibility/localization;
12. automated tests;
13. manual release checks;
14. store disclosure changes;
15. limitations;
16. release workflow/evidence impact.

## 23. Schema-change documentation

A schema change should update:

- `DATABASE_SCHEMA.md`;
- architecture/application flow where relevant;
- backup/restore compatibility docs;
- privacy/data lifecycle for new data categories;
- tests/fixtures;
- release compatibility/checklist/status;
- `CHANGELOG.md` and handoff.

## 24. Security-sensitive change documentation

Review/update as applicable:

- `SECURITY_MODEL.md`;
- `THREAT_MODEL.md`;
- `LOGGING_PRIVACY.md`;
- `DEPENDENCY_RISK_REGISTER.md`;
- security release review;
- tests/contracts;
- release workflow/evidence docs.

## 25. Configuration/package change documentation

When build/package/workflow/platform configuration changes:

- update `CONFIGURATION_REFERENCE.md`;
- update setup/troubleshooting if commands/toolchain changed;
- update dependency risk/migration docs if package graph changed;
- update tests/contracts;
- create new exact-head verification before using the newer source as a release baseline.

## 26. Documentation review checklist

Before committing documentation:

- implementation claims match source/evidence;
- current PR/test/run numbers are correct;
- no manual task is falsely checked;
- medical-safety wording remains intact;
- privacy/encryption wording is precise;
- dependency security and packaged compatibility are separated;
- reminder persisted/OS reconciliation wording is current;
- exact-tag/Release Evidence wording matches workflows;
- links/paths exist conceptually;
- new major docs are indexed from `docs/README.md`;
- root README points to the documentation hub/complete reference;
- changelog/status/handoff are updated when appropriate;
- historical evidence is retained.

## 27. Handoff record

`what_changed.md` is the detailed active continuation record.

It should contain:

- important commit SHAs/messages;
- source fixes;
- verification failures/successes;
- exact run evidence;
- documentation/release changes;
- remaining blockers;
- environment/tool limitations.

When old content is replaced for clarity, preserve the prior exact content under `docs/history/`.

## 28. Documentation completeness audit

The repository-wide documentation inventory is recorded in:

`docs/releases/DOCUMENTATION_AUDIT_20260814.md`

The operational checklist is:

`docs/releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md`

These documents establish documentation completeness, not production release completion.

## 29. Current production blockers

Even with complete documentation and green PR #56 source, production `1.0.0` remains blocked on actual evidence for:

- supported-platform manual testing;
- real notification delivery/recovery;
- packaged SQLite/encrypted-data compatibility;
- accessibility;
- current store policy/disclosures;
- signing/signed artifact provenance;
- exact production-tag Release Gate/Release Evidence;
- final version/build/checksum/publication work.

No documentation checkbox can substitute for those operations.
