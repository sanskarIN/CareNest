# CareNest Maintenance and Operations Manual

This manual defines how CareNest should be maintained after implementation work is complete. It applies to routine development, bug fixes, dependency changes, persistence/crypto changes, platform changes, documentation changes, verification, release preparation, and hotfixes.

CareNest is a local-first organizational health app. Maintenance must preserve its non-clinical boundary and must not silently introduce accounts, cloud synchronization, telemetry, diagnosis, dosage inference, treatment recommendations, interaction checking, risk scoring, or guaranteed notification-delivery claims.

## 1. Maintainer identity

Requested repository-local Git identity:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Preferred helpers:

```bash
build/scripts/setup-git.sh
```

```powershell
./build/scripts/setup-git.ps1
```

The helper scripts validate the repository root and configured values and fail on native Git errors.

Authenticated GitHub web/API/connector commits can use the GitHub account identity. Do not claim an arbitrary author email unless the actual commit metadata supports it.

## 2. Daily/routine maintenance cycle

For normal repository maintenance:

1. inspect current `main` and open pull requests/issues;
2. identify whether a change affects runtime, tests, project/package/workflow/platform/build-script source, or documentation only;
3. reproduce a reported defect with fictional/synthetic data;
4. add the smallest appropriate regression test when possible;
5. implement the correction at the lowest correct architectural layer;
6. run targeted tests;
7. run the full relevant test projects;
8. run formatting;
9. run dependency audit when package/restore behavior can change;
10. run affected platform builds;
11. update documentation and changelog/status evidence;
12. create an exact-head verification checkpoint when the verified source boundary changed;
13. never mark manual/device/store/signing evidence complete unless it was actually performed.

## 3. Issue triage

Classify incoming reports before changing code.

### Correctness

Examples:

- reminder state inconsistent with platform request;
- stale data after edit/delete;
- invalid UTC/time-zone behavior;
- failed rollback/cleanup;
- persistence corruption or migration issue;
- export/backup restore failure;
- UI state not refreshing after successful mutation.

Correctness defects should receive a regression test at the lowest useful layer.

### Security/privacy

Examples:

- secret/health content in logs;
- dependency advisory;
- plaintext cache lifetime;
- backup/document authentication issue;
- key handling problem;
- unsafe external data transmission;
- committed credential/signing artifact.

Treat these as release-blocking until scoped and resolved or explicitly tracked with an approved temporary risk decision.

### Platform limitation

Examples:

- Android battery/alarm restriction;
- Windows closed-app notification limitation;
- Apple permission/OS behavior;
- device-specific background scheduling.

Distinguish a product defect from an OS/platform limitation. Document the limitation rather than promising delivery the platform cannot guarantee.

### Documentation

Documentation-only corrections do not change runtime behavior, but policy/source contract tests may still depend on documentation content. Significant documentation alignment can therefore justify a marker-only verification.

### Feature request

Check whether the request violates current v1 boundaries. Networked accounts/sync/caregiver collaboration, analytics, clinical interpretation, or medical decision support require a new architecture/privacy/security design before implementation.

## 4. Bug-fix workflow

For a confirmed defect:

1. record the exact failing scenario using synthetic data;
2. decide whether the lowest meaningful regression belongs in UnitTests, IntegrationTests, or UiTests/source contracts;
3. add the regression test first or in the same logical change;
4. implement the fix without weakening analyzers/audit/quality gates;
5. run the targeted test;
6. run all affected suites;
7. inspect cancellation/failure paths, not only the success path;
8. update architecture/security/testing docs if behavior changed;
9. update `CHANGELOG.md` and `what_changed.md` for substantial work;
10. perform exact-head verification before promoting the new source baseline.

## 5. Reminder/platform changes

Reminder work is unusually sensitive because SQLite state and operating-system scheduled requests are independent surfaces.

Before changing reminder logic, preserve these current invariants:

- schedule intent comes only from explicit user-entered values;
- no medicine text is parsed into dosage/frequency advice;
- planner/rebuild transport boundaries use explicit UTC;
- snooze requires explicit future UTC;
- `SnoozedUntilUtc` is the effective due time for a valid snooze;
- old platform request cancellation occurs before replacement/suppression/invalidation when a previous request exists;
- handled Taken/Skipped/Delayed/Missed/Snoozed/Cancelled transitions are cancellation-first;
- cancellation failure remains retryable;
- later essential failure attempts non-cancelled restoration/rebuild;
- medicine/profile deletion cancels future platform requests before database cascade and compensates if persistence fails;
- appointment database/platform scheduling uses compensation rather than pretending to be one transaction;
- logging remains privacy-minimized.

Required review after a reminder change:

- unit/service tests;
- reminder reconciliation integration tests;
- UI/source contracts;
- Android/Windows/Apple platform compile;
- manual target checks if platform behavior changed;
- notification/platform architecture docs.

## 6. Database/schema changes

CareNest currently uses versioned local SQLite storage.

For every schema change:

1. assign the next ordered schema version;
2. implement migration without destroying supported prior-version data;
3. coordinate migration DDL and schema-version update transactionally;
4. add migration/repository integration coverage;
5. preserve WAL/busy-timeout/snapshot behavior;
6. review backup/restore compatibility;
7. update `docs/architecture/DATABASE_SCHEMA.md`;
8. update privacy/data-lifecycle docs if a new data category is introduced;
9. update test/release compatibility matrices;
10. perform packaged upgrade testing with fictional prior-version data before production promotion.

Do not use a clean fresh-install database as the only evidence for a migration change.

## 7. SQLite native/provider dependency changes

SQLite package security and stored-data compatibility are separate properties.

Current verified graph intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- Android native/provider leaves and selected providers at `2.1.12`;
- central transitive pinning enabled;
- no former exact `GHSA-2m69-gcr7-jv3q` suppression.

For a provider/native update:

1. review upstream package/release/security information;
2. update central package pins deliberately;
3. restore and inspect resolved graphs;
4. run unsuppressed dependency audit;
5. run all core tests;
6. build all four platform targets;
7. verify packaged upgrade of representative fictional existing data;
8. verify SQLite integrity and all structured data categories;
9. verify reminder rebuild/reconciliation;
10. verify encrypted document access through existing key state;
11. verify current backup create/restore and compatible historical synthetic backup if available;
12. update dependency risk/migration/release docs;
13. exact-head verify the final source.

Do not restore an old audit suppression merely because packaged compatibility testing is still incomplete.

## 8. Dependency updates in general

Dependabot/open package PRs are proposals, not automatic upgrades.

Before merging a dependency change:

- read the package release/security notes;
- identify runtime/platform/persistence/crypto impact;
- update central package version;
- run restore/build/test/audit;
- run affected platform builds;
- update documentation if behavior/toolchain changes;
- run relevant packaged/manual checks;
- use a new exact-head verification baseline.

If a dependency update changes MAUI SDK/runtime behavior, test all supported platform builds and the relevant manual interaction paths before release.

## 9. Encryption/document-vault changes

For changes to document encryption, key storage, stream framing, export or cache behavior:

- review `DOCUMENT_VAULT.md`, `SECURITY_MODEL.md`, and `THREAT_MODEL.md`;
- preserve fail-closed behavior when existing ciphertext depends on missing/corrupt key material;
- do not silently create an unrelated replacement key for existing encrypted payloads;
- preserve authenticated encryption and tamper rejection;
- retain supported historical read compatibility unless a proven migration/recovery plan replaces it;
- clear application-owned mutable key material where practical;
- ensure failed import/export cleans newly created artifacts best effort;
- ensure successful plaintext export is clearly outside the encrypted vault boundary;
- add integration and source-contract tests;
- perform packaged historical-fixture checks before production promotion.

## 10. Backup format changes

Before changing backup package format, password derivation, encrypted framing, archive topology, key portability, or restore transaction boundaries:

1. define the new package/stream version explicitly;
2. preserve or intentionally migrate supported historical formats;
3. add round-trip/wrong-password/tamper/truncation/trailing-data tests;
4. validate strict archive topology before extraction;
5. test missing/invalid document-key material;
6. preserve exact prior secure-store key restoration on failed restore where prior bytes existed;
7. keep primary cryptographic/restore completion distinct from later non-critical bookkeeping;
8. add canonical synthetic historical fixtures where possible;
9. update architecture/security/testing/release docs;
10. perform clean-install packaged restore and historical compatibility checks.

## 11. Logging/privacy changes

Every new log statement must be reviewed for private data.

Normal sensitive-path logs should not include:

- medicine names/instructions/health notes;
- document contents;
- backup contents/passwords;
- app-lock PIN;
- encryption keys;
- raw exception message or stack trace from health-data operations;
- record identifiers unless truly necessary and explicitly reviewed.

Prefer fixed operation/category text and exception type name when diagnostic context is needed.

Run logging-privacy source contracts after changes.

## 12. External links/funding changes

Current voluntary project support URL:

`https://buymeacoffee.com/sanskarIN`

External support must remain:

- explicit user action;
- separate from health functionality;
- free of health/profile/document/reminder identifiers in the URL;
- not a medical/support entitlement;
- subject to current store policy review at submission time.

Adding embedded payment/funding SDKs requires new privacy/security/store-policy architecture review.

## 13. Accessibility/design changes

For UI changes, review:

- semantic labels;
- focus order;
- text scaling;
- contrast;
- dark/light/system themes;
- keyboard navigation on desktop;
- reduced motion;
- color-independent status cues;
- responsive layout.

Automated XAML/source semantics checks are helpful but do not replace real assistive-technology testing.

## 14. Localization changes

For a new locale:

- move/verify strings in resources;
- verify culture-aware presentation dates/times;
- preserve machine-readable invariant formats where required;
- test text expansion;
- test plural/grammar needs;
- test accessibility;
- test RTL layout before shipping an RTL locale;
- update store text/screenshots if applicable.

## 15. Documentation maintenance

Behavior, architecture, security, dependency, release or setup changes must update the corresponding documentation in the same work.

Primary documentation entry points:

- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`;
- `docs/CODEBASE_REFERENCE.md`;
- `docs/CONFIGURATION_REFERENCE.md`;
- `docs/README.md`.

Do not remove historical evidence merely because a newer baseline exists. Mark historical material as historical and add a current authoritative correction/addendum.

## 16. Local verification

Before pushing significant changes:

```bash
build/scripts/quality-gate.sh
```

or:

```powershell
./build/scripts/quality-gate.ps1
```

For release-oriented work:

```bash
build/scripts/release-preflight.sh
```

or:

```powershell
./build/scripts/release-preflight.ps1
```

Dependency audit failures are blocking.

## 17. Exact-head verification protocol

When runtime, tests, project/package/workflow/platform/build-script source changes:

1. finish all source/test/docs changes required by the behavior;
2. freeze the exact `main` source SHA;
3. create a temporary verification branch from that SHA;
4. add only one marker under `build/verification/`;
5. open a PR against `main`;
6. verify the diff is marker-only beyond frozen source;
7. require CareNest CI, CodeQL and Dependency Audit success;
8. if a gate fails, fix the defect on `main` rather than weakening policy;
9. create a new exact-head checkpoint from corrected source;
10. record exact test totals and run IDs;
11. close the marker PR without merge.

See `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

## 18. Documentation-only changes after a verified source

Documentation-only commits may be made after a verified runtime/source boundary if they truly do not modify runtime/test/project/workflow/package/platform/build-script files.

For a release decision, prove that relationship with commit comparison and do not imply the newer documentation commit itself was platform compiled if it was not.

If documentation is consumed by executable repository policy tests, a final documentation-policy marker verification is recommended before freezing a release candidate.

## 19. Pull-request review checklist

A reviewer should confirm, as applicable:

- intended behavior is clear;
- medical-safety boundary remains intact;
- local-first/privacy boundary remains intact;
- no secret/signing/user data committed;
- architecture direction preserved;
- no SQL in ViewModels;
- no casual runtime network/telemetry client;
- async/cancellation behavior is safe;
- failure/rollback paths handled;
- reminder platform reconciliation ordering preserved;
- persistence/crypto compatibility considered;
- regression tests added;
- formatting/analyzer/audit gates not weakened;
- docs/changelog/status updated;
- manual release work is not falsely marked complete.

## 20. Release candidate preparation

Before selecting the production commit:

- complete all source changes;
- complete required exact-head automated verification;
- complete manual platform/device matrix;
- complete notification checks;
- complete packaged SQLite/encrypted-data/backup compatibility checks;
- complete accessibility checks;
- review current store policies/disclosures;
- configure signing outside Git;
- generate/inspect signed packages;
- prepare store assets/listings;
- update release notes/changelog/status;
- ensure release checklist has no applicable unchecked blocker.

## 21. Production tag process

Production tags matching `v*` are configured to run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Tag process:

1. select the exact approved commit;
2. create the intended immutable version tag;
3. wait for every required tagged workflow to succeed;
4. inspect Release Evidence provenance and checksums;
5. verify signed artifacts correspond to the same approved source;
6. publish only after all applicable manual/store/signing gates are satisfied.

Do not move a failed release tag to make it look successful. Fix on a new commit, repeat required verification, and use a corrected approved version/tag according to release policy.

## 22. Release Evidence operation

The Release Evidence workflow captures:

- source/ref/run identity;
- run attempt;
- toolchain information;
- tracked-source manifest/checksums;
- unit/integration/UI TRX;
- transitive dependency inventories;
- workspace-integrity evidence;
- evidence checksums.

Available evidence is uploaded before aggregate failure evaluation. A failed run can therefore have an artifact; artifact existence alone is not approval.

## 23. Signing operations

Signing credentials never belong in Git.

Maintain separately:

- Android production keystore/private key;
- Apple certificates/private keys/provisioning configuration;
- Windows signing certificate/private key;
- store/CI credentials.

Release documentation can record non-secret certificate/profile identifiers and provenance but must not contain private key material or passwords.

## 24. Store submission operations

Before submission:

- verify app identifiers/version/build numbers;
- verify signed package contents;
- verify current platform permission descriptions;
- verify privacy/data-safety forms against actual behavior;
- verify health-organizer wording does not imply clinical decision support;
- verify support/privacy/terms/security links;
- verify voluntary external support link against current store policy;
- use fictional data in screenshots;
- archive approved store text/assets and package checksums.

## 25. Hotfix process

For a production-blocking hotfix:

1. reproduce the issue from the released tag/source;
2. make the smallest safe correction on a new branch/commit from the appropriate maintained line;
3. add regression coverage;
4. run all affected tests and platform builds;
5. run CodeQL/dependency audit as applicable;
6. repeat relevant manual/device/compatibility checks;
7. exact-head verify the corrected source;
8. update changelog/release notes/status;
9. create a new version tag; never rewrite the old release tag;
10. run tagged Release Gate/Release Evidence;
11. publish corrected packages only after approval.

## 26. Rollback/recovery planning

CareNest is local-first, so rollback cannot assume server-side data repair.

Before a risky persistence/crypto release:

- ensure users can make a backup using the prior version where practical;
- define backward/forward data compatibility;
- retain compatible readers/migrations as needed;
- keep canonical synthetic old-version fixtures;
- never publish a rollback plan that would make newer encrypted/database state unreadable by the chosen rollback version without clear recovery steps.

## 27. Incident response

If a privacy/security defect is discovered:

1. stop production promotion if not yet released;
2. scope affected versions/data surfaces;
3. avoid posting sensitive reproduction data publicly;
4. fix the source and add regression coverage;
5. review whether keys/passwords/signing credentials could be exposed;
6. rotate/revoke credentials when necessary;
7. update `SECURITY.md`/security model/risk register as appropriate;
8. perform exact-head verification;
9. release a new version without rewriting historical evidence.

For a dependency advisory, determine actual resolved package graph/exposure and do not equate suppression with remediation.

## 28. Current automated baseline

Authoritative release-engineering source verification: PR #56.

- source/base: `4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`
- marker head: `e3bc621cea05364a69abee0dadbd71a67c17bddb`
- CareNest CI #571 / `31770929379`: success
- 122 unit + 39 integration + 124 UI-contract/policy = 285/285
- Android Release: success
- Windows Release: success
- iOS simulator Release: success
- Mac Catalyst Release: success
- CodeQL #571 / `31770929382`: success
- unsuppressed Dependency Audit #41 / `31770929383`: success

PR #56 was closed without merge and its marker is not part of `main`.

## 29. Current production-blocking work

Automated source verification is complete for the PR #56 baseline. Production `1.0.0` still requires actual evidence for:

- supported-platform manual matrices;
- real notification delivery and recovery;
- packaged SQLite existing-data compatibility;
- encrypted document/backup compatibility;
- accessibility;
- current store policy/disclosures;
- signing and signed package inspection;
- store assets/listings;
- exact final production-tag workflow/evidence;
- final version/build/checksum/release publication.

Do not mark these complete through documentation alone.
