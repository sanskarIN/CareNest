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
- explicit about the exact source SHA behind verification claims;
- precise about normal/default versus store-safe source configuration evidence;
- precise about internal inspection artifacts versus signed/store-ready artifacts.

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

When current automated/source-inspection evidence is needed, use PR #61:

- source/base: `4c60f90ac33a321d12a6f9b3a8c097e4e4a4e5f2`;
- marker head: `19c82b813c375047cf1166487bc18a1bd2cd0e52`;
- PR merge/event SHA: `c8ea9fef89d7b773f19bf13c64f349495be706ad`;
- CareNest CI #650 / `31872610834`: success;
- 122 unit + 39 integration + 157 UI-contract/policy = 318 total;
- default Android/Windows/iOS simulator/Mac Catalyst Release builds: success;
- CareNest Store Package Configuration #39 / `31872610789`: success;
- funding-disabled Android/Windows/iOS simulator/Mac Catalyst Release builds: success;
- Bash store-package preflight executable-mode guard: success;
- CareNest Store Inspection Artifacts #2 / `31872610786`: success;
- Android verified-unsigned AAB, Windows unpackaged bundle, iOS simulator bundle and unsigned Mac Catalyst bundle: downloaded checksum/provenance inspection success;
- CodeQL #650 / `31872610815`: success;
- unsuppressed Dependency Audit #46 / `31872610791`: success.

PR #61 was closed without merge and its marker is not part of `main`.

PR #60 is historical superseded artifact evidence because downloaded Android artifact inspection exposed the debug-signed companion and ambiguous PR merge/source provenance. PR #59 is historical exact store-safe compilation evidence, PR #58 is historical exact package/store-policy hardening evidence, PR #56 is historical exact release-engineering evidence, and PR #54 is historical authoritative runtime bug-audit evidence. Do not incorrectly attribute an older PR to later source.

PR #43 is historical failed-core evidence and must not be described as fully green.

The current exact evidence record is `docs/releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md`.

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
- Release Evidence/verification protocol;
- store-build policy;
- packaged release validation;
- exact store-safe configuration verification evidence;
- exact store-inspection artifact verification evidence.

These documents must distinguish automated source/internal-artifact verification from manual/device/store/signing/package evidence.

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
- PR event/merge SHA when artifact provenance distinguishes it from the source head;
- PR number;
- CI run number/ID;
- unit/integration/UI-contract test counts;
- default platform build results;
- store-safe platform build results when the source contains a store-safe path;
- CodeQL run;
- Dependency Audit run;
- store-package configuration run ID when applicable;
- Store Inspection Artifacts run ID and artifact IDs/digests/checksums when applicable;
- marker-only and closed-without-merge status.

Do not reuse verification evidence after runtime/test/project/workflow/package/platform/build-script/artifact-generation source changes.

Workflow, test, build-script, package, platform configuration and artifact-generation changes are verification-relevant even when app runtime behavior is not intentionally changed.

Do not represent funding-disabled source compilation as signed/installed package inspection or store approval. Do not represent an internal unsigned/simulator/unpackaged artifact as production signing evidence.

## 11. Internal artifact wording

When referring to `CareNest Store Inspection Artifacts`:

- call them internal inspection artifacts;
- identify the exact source SHA separately from a PR merge/event SHA;
- state that Android output is verified unsigned;
- state that Windows output is unpackaged;
- state that iOS output is simulator-only;
- state that Mac Catalyst output is unsigned;
- include payload checksum/provenance evidence when making an exact artifact claim;
- retain `internal-inspection-only` / `store_submission_ready=false` language;
- never describe them as signed production packages, store submissions or store approvals.

PR #60 must remain visible as the evidence that green workflow status alone was insufficient: downloaded artifact inspection exposed MAUI's debug-signed Android companion. PR #61 is the corrected exact baseline.

## 12. Documentation-only commits after verified source

If commits after a verified source are truly documentation-only:

- call them documentation-only;
- do not claim the later doc commit itself was platform compiled unless it was;
- use commit comparison when making a release decision;
- keep the verified source SHA distinct from the documentation head;
- consider a final documentation-policy check when repository policy tests consume docs.

This exception does not apply to tests, workflows, artifact-generation logic, package/project files, platform configuration or build/release scripts.

## 13. Historical evidence

Do not erase failure history to make the project look cleaner.

Historical artifacts can describe old dependency exceptions, test counts, PR failures and source limitations if their historical context is explicit.

When replacing a stale active document, preserve the old exact version under `docs/history/` when practical.

Current active docs must clearly identify the latest authoritative state.

## 14. Manual evidence wording

Do not mark device/accessibility/store/signing/packaged-data work complete unless it actually occurred.

`MANUAL_TEST_MATRIX.md` is an evidence record, not an aspiration checklist that can be checked from CI.

Examples:

- green Android build ≠ real Android notification delivery;
- green NuGet audit ≠ packaged existing SQLite database upgrade proof;
- green funding-disabled source build ≠ installed store artifact has the support card hidden;
- green internal inspection artifact workflow ≠ signed production package;
- source-level semantic accessibility checks ≠ screen-reader certification;
- Release Evidence artifact existence ≠ successful Release Evidence gate if the workflow failed.

## 15. Store-policy wording

Store policies change.

Documentation should require current submission-time review rather than freezing possibly stale rules as permanent truth.

This applies particularly to:

- health categorization;
- privacy/data-safety declarations;
- permissions;
- external voluntary project-support links;
- payment/funding rules;
- screenshots/listing claims.

The dated 2026-08-15 Apple/Google support-link review is current evidence for its review date, not a permanent approval. It selects `CareNestShowFundingLink=false` for initial store candidates unless submission-time policy clearly permits the external link.

## 16. Funding wording

Canonical voluntary project-support URL:

`https://buymeacoffee.com/sanskarIN`

Funding must not be represented as:

- medical functionality;
- higher reminder priority;
- emergency assistance;
- access to local records;
- paid health advice;
- a clinical/support entitlement.

Document `CareNestShowFundingLink` as a visibility/build-policy switch only, not as a feature entitlement switch. In store-safe builds the hidden funding command is non-executable.

## 17. Security/privacy examples and test data

Use synthetic/fictional data in:

- documentation examples;
- screenshots;
- automated tests;
- public bug reports;
- store graphics;
- migration/backup fixtures.

Never put real health documents/backups/PINs/passwords/keys/signing credentials in documentation or Git.

## 18. Command accuracy

Commands in documentation should:

- match current repository paths;
- use current target frameworks;
- use `CareNestTargetFramework` for target-specific MAUI build isolation;
- use fail-closed store-package wrappers where store-safe funding-disabled preflight is intended;
- use `RuntimeIdentifierOverride=win-x64` only for the documented Windows internal inspection publish path;
- not assume every platform workload exists on every host;
- reflect current CI/workload patterns where practical;
- treat unsuppressed dependency audit as blocking;
- keep signing secrets outside Git.

The Bash store-package wrapper is intended to be directly executable and its Git executable mode is CI-checked.

## 19. Git identity wording

Requested local maintainer identity:

- name: `Sanskar`;
- email: `sanskarin@outlook.in`.

Document this as repository-local Git configuration with `git config --local`.

Do not claim GitHub web/API/connector commits used that email unless actual commit metadata supports it.

## 20. Release workflow wording

Production tags matching `v*` run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- CareNest Store Package Configuration;
- CareNest Store Inspection Artifacts;
- Release Gate;
- CareNest Release Evidence.

CareNest Store Package Configuration should be documented as source compilation with `CareNestShowFundingLink=false` across the supported targets, not as signing or publication.

CareNest Store Inspection Artifacts should be documented as reproducible non-production package-shape/checksum/provenance evidence, not as signing or submission.

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

## 21. Release Gate wording

Release Gate is fail-closed for:

- required documents;
- open dependency risk;
- unchecked applicable release checklist items;
- core tests.

Do not describe formatting changes to Markdown checklist rows as a way to bypass the gate. Source contracts protect nested/case/indentation behavior.

## 22. Links

Prefer relative links for repository-local documentation.

Canonical external identity values:

- repository: `https://github.com/sanskarIN/CareNest`;
- creator: `https://www.github.com/sanskarIN`;
- business: `sanskarin@outlook.in`;
- support: `supportramsandesh@gmail.com`;
- voluntary support: `https://buymeacoffee.com/sanskarIN`.

## 23. New feature documentation checklist

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

## 24. Schema-change documentation

A schema change should update:

- `DATABASE_SCHEMA.md`;
- architecture/application flow where relevant;
- backup/restore compatibility docs;
- privacy/data lifecycle for new data categories;
- tests/fixtures;
- release compatibility/checklist/status;
- `CHANGELOG.md` and handoff.

## 25. Security-sensitive change documentation

Review/update as applicable:

- `SECURITY_MODEL.md`;
- `THREAT_MODEL.md`;
- `LOGGING_PRIVACY.md`;
- `DEPENDENCY_RISK_REGISTER.md`;
- security release review;
- tests/contracts;
- release workflow/evidence docs.

## 26. Configuration/package/artifact change documentation

When build/package/workflow/platform/artifact-generation configuration changes:

- update `CONFIGURATION_REFERENCE.md`;
- update setup/troubleshooting if commands/toolchain changed;
- update store-build/package validation docs when funding/store-safe configuration changed;
- update dependency risk/migration docs if package graph changed;
- update tests/contracts;
- inspect generated artifacts when artifact-generation logic changed;
- create new exact-head verification before using the newer source as a release baseline.

## 27. Documentation review checklist

Before committing documentation:

- implementation claims match source/evidence;
- current PR/test/run numbers are correct;
- default versus store-safe build evidence is distinguished;
- internal artifact evidence is distinguished from production signing/store submission;
- source SHA versus PR merge/event SHA is clear for artifact evidence;
- no manual task is falsely checked;
- medical-safety wording remains intact;
- privacy/encryption wording is precise;
- dependency security and packaged compatibility are separated;
- reminder persisted/OS reconciliation wording is current;
- exact-tag/Store Package Configuration/Store Inspection Artifacts/Release Evidence wording matches workflows;
- links/paths exist conceptually;
- new major docs are indexed from `docs/README.md`;
- root README points to the documentation hub/complete reference;
- changelog/status/handoff are updated when appropriate;
- historical evidence is retained.

## 28. Handoff record

`what_changed.md` is the detailed active continuation record.

It should contain:

- important commit SHAs/messages;
- source fixes;
- verification failures/successes;
- exact run evidence;
- artifact IDs/digests/payload checksums and inspection findings when relevant;
- documentation/release changes;
- remaining blockers;
- environment/tool limitations.

When old content is replaced for clarity, preserve the prior exact content under `docs/history/`.

## 29. Documentation completeness audit

The repository-wide documentation inventory is recorded in:

`docs/releases/DOCUMENTATION_AUDIT_20260814.md`

The operational checklist is:

`docs/releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md`

These documents establish documentation completeness, not production release completion.

## 30. Current production blockers

Even with complete documentation, green PR #61 source, and verified internal inspection artifacts, production `1.0.0` remains blocked on actual evidence for:

- supported-platform manual testing;
- real notification delivery/recovery;
- packaged SQLite/encrypted-data compatibility;
- accessibility;
- submission-time store policy/disclosures;
- actual signed/installed store-safe package inspection;
- signing/signed artifact provenance;
- exact production-tag CareNest CI/CodeQL/Dependency Audit/Store Package Configuration/Store Inspection Artifacts/Release Gate/Release Evidence;
- final version/build/checksum/publication work.

No documentation checkbox can substitute for those operations.