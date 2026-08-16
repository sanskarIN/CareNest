# CareNest Maintenance and Operations Manual

**Release line:** `1.0.0-rc.1`  
**Current verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

This manual governs routine maintenance, defect correction, dependency changes, persistence/crypto changes, platform changes, documentation work, verification, release preparation, incident response and hotfixes.

CareNest is a local-first organizational health app. Maintenance must preserve the non-clinical boundary and must not silently introduce accounts, cloud synchronization, telemetry, diagnosis, dosage inference, treatment recommendations, clinical interaction/risk scoring or guaranteed notification-delivery claims.

## 1. Maintainer identity

Repository-local convention:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Helpers:

```bash
build/scripts/setup-git.sh
```

```powershell
./build/scripts/setup-git.ps1
```

GitHub/API/connector commits should be described using actual commit metadata.

## 2. Maintenance cycle

For normal work:

1. inspect current `main`, open PRs/issues and current status;
2. classify whether the change affects runtime, persistence, crypto, platform, project/package/workflow/build scripts, tests or documentation only;
3. reproduce defects with fictional/synthetic data;
4. add the lowest-suitable regression test;
5. implement at the lowest correct architecture layer;
6. run targeted tests;
7. run full affected suites;
8. run formatting/quality gate;
9. run unsuppressed dependency audit when restore/package behavior changes;
10. run affected MAUI targets;
11. update all related documentation;
12. create fresh exact-source verification when the executable/test/build/workflow boundary changes;
13. never mark manual/device/store/signing evidence complete without performing it.

## 3. Sources of truth

Use:

- `PROJECT_STATUS.md` — current release state;
- `docs/releases/NEXT_STEPS.md` — remaining production work;
- latest exact-source verification record — current automated evidence;
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md` — whole-project reference;
- `docs/DOCUMENTATION_CATALOG.md` — navigation/authority map;
- specialized subsystem docs — implementation detail;
- `docs/history/` — historical snapshots.

## 4. Issue triage

### Correctness

Examples:

- reminder persisted/platform inconsistency;
- stale data after edit/delete;
- invalid UTC/time-zone behavior;
- failed rollback/cleanup;
- database migration/corruption issue;
- backup/export restore failure;
- stale UI after successful mutation.

Correctness defects should receive regression tests.

### Security/privacy

Examples:

- secret/health content in logs;
- dependency advisory;
- plaintext cache lifetime;
- backup/document authentication issue;
- key handling problem;
- unsafe external data transmission;
- committed signing/credential material.

Treat these as release-blocking until resolved or explicitly risk-tracked.

### Platform limitation

Examples:

- Android battery/alarm restriction;
- Windows closed-app reminder limitation;
- Apple notification permission/OS policy;
- device/vendor background scheduling.

Distinguish product defect from platform constraint; do not promise delivery the OS cannot guarantee.

### Documentation

Documentation-only changes do not intentionally alter runtime but can affect source-policy tests/links/contracts. Keep factual claims tied to exact source evidence.

### Feature request

Networked accounts/sync/caregiver collaboration, analytics, clinical interpretation or medical decision support require new architecture/privacy/security/safety design before implementation.

## 5. Bug-fix workflow

1. reproduce with safe synthetic data;
2. identify the failing invariant;
3. write a regression test where practical;
4. fix the smallest correct boundary;
5. verify failure and success paths;
6. run related integration/source-policy tests;
7. run affected platform builds;
8. update docs/status/evidence as appropriate;
9. if verification-relevant source changed, verify the corrected exact source.

Do not broaden refactoring during a release-blocking fix unless required for correctness.

## 6. Reminder maintenance

Preserve:

- explicit user-entered schedules only;
- profile/medicine/schedule ownership;
- active/archive state handling;
- explicit time-zone identifiers;
- UTC planning windows;
- deterministic DST rules;
- stable occurrence identity;
- valid future UTC snooze;
- `SnoozedUntilUtc` as effective due time;
- stale OS request reconciliation;
- cancellation before replacement/suppression/invalidation;
- cancellation-first handled actions;
- retryable cancellation failure;
- persistence/platform compensation and restoration/rebuild.

Update `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` if the contract changes.

## 7. Date/time maintenance

- do not relabel local/unspecified ticks as UTC;
- appointments require true UTC starts;
- schedule planning keeps explicit time-zone context;
- invalid DST-gap input must not silently become another local time;
- snooze deadlines are explicit UTC values.

## 8. Schema/database changes

For a schema change:

1. add an ordered migration;
2. coordinate DDL/version state transactionally where required;
3. preserve supported prior upgrade paths;
4. update `DATABASE_SCHEMA.md`;
5. add integration tests;
6. review relationship/cascade cleanup;
7. review backup/restore/export implications;
8. update privacy/data lifecycle if categories change;
9. run packaged compatibility before production.

## 9. SQLite dependency changes

Current source security path is unsuppressed. Do not restore the old `GHSA-2m69-gcr7-jv3q` suppression.

For SQLite package/provider/native changes:

- follow `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`;
- audit both platform-neutral and MAUI graphs;
- run tests/platform builds;
- inspect resolved transitive graph;
- perform packaged existing-data/encrypted-data compatibility;
- update risk/configuration docs.

Dependency security and packaged compatibility are separate concerns.

## 10. Document-vault changes

Review:

- authenticated encryption/framing;
- key ownership/secure storage;
- legacy read compatibility;
- import/export/share boundaries;
- temporary plaintext cleanup;
- rollback after metadata/audit failures;
- backup portability;
- logging/privacy.

Missing/corrupt key state with existing ciphertext must fail closed rather than silently generating unrelated replacement key material.

## 11. Backup-format changes

Before changing backup framing/topology/derivation:

- define format version and compatibility;
- update architecture/security/threat model;
- add wrong-password/tamper/truncation/trailing-data tests;
- validate strict archive topology;
- verify SQLite snapshot integrity;
- verify encrypted-document recovery material;
- verify clean-install restore;
- retain genuine historical fixtures where they exist;
- document rollback/migration behavior.

## 12. App-lock changes

Preserve:

- no plaintext PIN persistence;
- random salt;
- PBKDF2-HMAC-SHA256 verifier;
- fixed-time comparison;
- secure-store material;
- strict material validation;
- update/disable rollback;
- fail-closed corrupt/missing state;
- explicit statement that app lock is not whole-database encryption.

Biometric/remote recovery requires a separate threat-model decision.

## 13. Reports/exports changes

Preserve:

- explicit user action;
- formula-like CSV content neutralization;
- staged/atomic final output where documented;
- cleanup of app-owned temporary files;
- no claim that external copies can be revoked after handoff;
- required safety/privacy wording.

## 14. Logging changes

Do not log:

- user-entered health content;
- document/backup contents;
- PIN/password/key material;
- signing credentials;
- unnecessary sensitive exception messages/stack traces.

Prefer safe operation/category context and exception type when sufficient.

## 15. Network/cloud changes

Current v1 is local-first/account-free.

A networked feature requires explicit design covering authentication, authorization, consent, key management, privacy, deletion/export, threat model, offline/conflict behavior and store disclosures.

Do not casually add HTTP/telemetry clients to current local-first runtime.

## 16. XAML/UI maintenance

Current build policy requires strict compiled bindings:

- real root `x:DataType`;
- item `x:DataType` in binding templates;
- typed picker display bindings;
- typed explicit Source/ancestor bindings;
- `XC0022`–`XC0025` as errors;
- no matching warning/type-safety bypass.

New UI work must preserve this policy and update accessibility/manual matrices as needed.

## 17. Documentation maintenance

Behavior, architecture, data, security, dependencies, release/setup/platform changes must update documentation in the same work.

For major documentation work:

- update `docs/DOCUMENTATION_CATALOG.md`;
- preserve exact prior active files under `docs/history/` when replacing major canonical references;
- keep historical evidence source-boundary-specific;
- never rewrite old verification counts as though they were current;
- never mark manual work complete because a runbook exists.

## 18. Local quality verification

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

## 19. Exact-source verification

When runtime, tests, project/package/workflow/platform/build-script source changes:

1. finish required source/test/docs changes;
2. freeze exact candidate source;
3. use the repository verification protocol;
4. require the configured CI/security/dependency/store-package/store-inspection gates as applicable;
5. fix failures in source rather than weakening gates;
6. record exact source/test/run evidence;
7. keep marker-only verification artifacts out of `main` when the protocol says so.

See `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

## 20. Documentation-only heads

A documentation-only commit can sit above a verified executable source if comparison proves no source/test/project/workflow/build/package/runtime files changed.

Do not claim the newer documentation SHA itself was a new executable baseline unless the workflows actually verified it.

## 21. Pull-request review checklist

Review, as applicable:

- behavior intent;
- medical-safety boundary;
- local-first/privacy boundary;
- no secrets/private user data;
- architecture direction;
- no direct SQL in ViewModels;
- no casual runtime network/telemetry;
- async/cancellation safety;
- failure/rollback paths;
- reminder reconciliation ordering;
- persistence/crypto compatibility;
- strict XAML compliance;
- regression tests;
- formatter/analyzer/audit gates unchanged;
- docs/status/evidence updated;
- manual work not falsely completed.

## 22. Current workflow matrix

Configured repository automation includes:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

Production-style `v*` tags are expected to participate in the applicable full release matrix.

## 23. Package funding boundary

The distributed application source/package contains no external Buy Me a Coffee destination/card/command/artwork.

Do not reintroduce an app funding build toggle/surface as a routine maintenance change. Repository funding metadata/docs remain separate and do not create health/medical entitlement.

## 24. Release candidate preparation

Before production selection complete applicable:

- exact-source automated verification;
- Android/Windows/iPhone/iPad/Mac Catalyst manual matrices;
- real notification delivery/lifecycle;
- packaged SQLite compatibility;
- packaged encrypted document/backup compatibility;
- accessibility;
- current store policy/disclosures;
- production signing outside Git;
- final signed-package checksums/provenance/inspection;
- store assets/listings;
- final source/tag/release evidence.

## 25. Production tag process

1. select exact approved production commit;
2. create intended immutable version tag;
3. require tagged CI/CodeQL/Dependency Audit/Store Package/Store Inspection/Release Gate/Release Evidence success;
4. verify signed package provenance/checksums match the approved source;
5. publish only after manual/store/signing evidence is complete.

Do not move a failed/rejected production tag to another commit.

## 26. Release Evidence operation

Release Evidence records exact candidate identity, tracked-file manifests/checksums, tests/dependencies/workspace information and evidence checksums.

Artifact existence alone is not approval; the run outcome and provenance must be reviewed.

## 27. Signing operations

Signing material remains outside Git:

- Android keystore/private keys;
- Apple private keys/certificates/provisioning;
- Windows signing private keys;
- store/CI credentials.

Documentation can record non-secret identifiers/fingerprints/provenance.

## 28. Store submission operations

Before submission verify:

- identity/version/build;
- signed package contents/checksum;
- current platform permission descriptions;
- privacy/data-safety declarations;
- non-clinical health-organizer wording;
- support/privacy/terms/security links;
- screenshots use fictional data;
- current store policies;
- package contains no prohibited/unexpected external funding marker.

## 29. Hotfix process

1. reproduce from released/candidate source;
2. make smallest safe fix;
3. add regression coverage;
4. run affected tests/platform/security/dependency gates;
5. repeat required manual/compatibility checks;
6. exact-source verify corrected source;
7. update changelog/status/release notes;
8. create a new version/tag rather than rewriting historical tag/evidence.

## 30. Incident response

For privacy/security incidents:

1. stop promotion if appropriate;
2. scope affected versions/data surfaces;
3. avoid posting sensitive reproduction data publicly;
4. fix and add regression coverage;
5. assess key/credential exposure and rotate/revoke when needed;
6. update security/risk documentation;
7. verify corrected source;
8. release a new version without rewriting history.

## 31. Current automated baseline

PR #74 frozen source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Verified:

- 331/331 core tests;
- all four normal Release targets;
- all four store-candidate targets;
- Android/Windows/Apple inspection artifacts;
- CodeQL;
- unsuppressed Dependency Audit.

See `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

## 32. Current production-blocking work

Production `1.0.0` remains blocked on actual evidence for real-device behavior, notification lifecycle, packaged data/encryption compatibility, accessibility, signing, final signed-package inspection, current store policy/metadata, exact production tag/release gates and publication.

Use `docs/releases/NEXT_STEPS.md` as the authoritative checklist.