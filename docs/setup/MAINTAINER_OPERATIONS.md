# CareNest Maintainer Operations Guide

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

This guide is for maintainers working on source, CI, release evidence, dependencies, documentation and production preparation.

## 1. Maintainer identity

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

Use actual Git commit metadata when describing connector/API-created commits.

## 2. Start every task from current authority

Review:

- `README.md`;
- `PROJECT_STATUS.md`;
- `docs/DOCUMENTATION_CATALOG.md`;
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`;
- `docs/releases/NEXT_STEPS.md`;
- latest exact-source verification record;
- relevant architecture/security/testing documents.

Do not assume an old dated verification file is the current source baseline.

## 3. Main branch and verification branches

Normal repository state lives on `main`.

Marker-only exact-source verification branches are temporary evidence mechanisms where defined by the verification protocol. Do not leave marker files in `main` unless the protocol explicitly changes.

## 4. Before making changes

Classify whether work affects:

- runtime behavior;
- persisted schema;
- encryption/key/backup format;
- reminder semantics;
- platform permissions/integration;
- dependencies;
- build/project/workflow scripts;
- tests/source-policy;
- documentation only.

Tests, workflows and build scripts are verification-relevant even when user-facing runtime source is unchanged.

## 5. Commit discipline

Prefer small logical commits:

```text
feat: ...
fix: ...
security: ...
test: ...
docs: ...
ci: ...
build: ...
chore: ...
```

Keep each commit reviewable as one coherent change.

## 6. Medical-safety boundary

Never introduce behavior that:

- diagnoses;
- calculates/infers dosage;
- recommends treatment;
- provides clinical interaction/risk scoring;
- guarantees reminder delivery;
- substitutes for emergency services.

Medicine strength/instruction text remains opaque user input.

## 7. Local-first/privacy boundary

Current v1 has no required CareNest account/backend, automatic CareNest cloud synchronization or hidden runtime analytics/telemetry client.

A new network feature requires explicit authentication, authorization, consent, privacy, key-management, deletion/export, offline/conflict, threat-model and store-policy design.

## 8. Runtime source changes

For behavior changes:

1. inspect current architecture;
2. implement at the lowest correct layer;
3. add regression coverage;
4. update documentation;
5. run formatting/tests/audit;
6. run affected platform/store workflows;
7. create fresh exact-source evidence before claiming a new baseline.

## 9. Reminder changes

Consider:

- ownership/state;
- explicit schedule kind/time zone;
- UTC windows;
- DST gap/overlap;
- stable occurrence identity;
- state suppression;
- as-needed behavior;
- explicit future snooze;
- effective snooze due time;
- stale request cancellation;
- cancellation before replacement/suppression/invalidation;
- cancellation-first handled actions;
- persistence/platform compensation;
- lifecycle reconciliation;
- OS delivery limits.

Update the reminder contract when behavior changes.

## 10. Schema changes

For SQLite schema work:

1. add ordered migration/version;
2. preserve historical migration semantics;
3. update `DATABASE_SCHEMA.md`;
4. add migration/integrity tests;
5. review cascades/cleanup;
6. review backup/restore/export;
7. update privacy lifecycle if data categories change;
8. run packaged compatibility before production.

## 11. Dependency changes

- update central package definitions deliberately;
- restore/build/test;
- run unsuppressed Dependency Audit;
- inspect transitive graph;
- run affected target builds;
- update third-party notices/risk docs when required;
- perform packaged compatibility when persistence/native behavior can change.

For SQLite follow `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

Do not restore the former exact audit suppression.

## 12. Security/privacy changes

Read:

- `SECURITY.md`;
- `docs/security/SECURITY_MODEL.md`;
- `docs/security/THREAT_MODEL.md`;
- `docs/security/LOGGING_PRIVACY.md`.

Add automated regression coverage where practical. Do not weaken security/analyzer gates to hide legitimate findings.

## 13. Logging changes

Do not log raw user health text, document/backup contents, PINs/passwords, crypto keys, signing credentials or unnecessary sensitive exception contents.

Prefer privacy-minimized metadata/category/exception type.

## 14. App-lock changes

Preserve no plaintext PIN persistence, random salt, PBKDF2-HMAC-SHA256 verifier, fixed-time comparison, secure-store ownership, strict material validation, rollback/fail-closed behavior and the limitation that app lock is not whole-database encryption.

## 15. Document/backup changes

Review authenticated encryption, key ownership, legacy compatibility, import/export/share boundary, temporary plaintext cleanup, backup topology, restore rollback and logging/privacy.

Format/key changes require updated architecture/security/threat-model/release evidence.

## 16. XAML/UI changes

Current binding policy requires:

- accurate root `x:DataType`;
- DataTemplate item `x:DataType`;
- typed picker display bindings;
- typed explicit Source/ancestor bindings;
- Source binding compilation;
- strict XAML compilation;
- `XC0022`–`XC0025` as errors;
- no matching suppression/type-safety bypass.

## 17. Documentation changes

Documentation must remain tied to implemented/source-verified behavior.

For major canonical rewrites:

- preserve exact prior active files under `docs/history/`;
- update `docs/DOCUMENTATION_CATALOG.md` and `docs/README.md`;
- record a dated audit/handoff;
- keep historical evidence historical;
- do not mark manual tests complete without evidence.

`what_changed.md` remains a chronological handoff surface; large documentation passes can additionally use a dated release handoff to avoid discarding prior content.

## 18. Current CI/workflow set

Key workflows include:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

### Pull requests

PRs to `main` can run CI/security/dependency/store-package/store-inspection workflows according to their current triggers.

### Manual runs

Use `workflow_dispatch` only where a workflow defines it; manual execution does not override evidence requirements.

### Production-style tags

Tags matching `v*` are expected to participate in the applicable full matrix:

- CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

## 19. Local quality gate

```bash
build/scripts/quality-gate.sh
```

or:

```powershell
./build/scripts/quality-gate.ps1
```

This does not replace MAUI platform builds or manual device testing.

## 20. Release preflight

```bash
build/scripts/release-preflight.sh
```

or PowerShell equivalent.

When `CARENEST_TARGET` is set, use a currently supported target framework.

The current application package has no external BMC funding surface and no application funding-link build toggle.

## 21. Store-package preflight

Use the target-specific wrapper with an explicit supported TFM. It delegates normal release preflight and does not sign or publish a production package.

## 22. Store Inspection Artifacts

The workflow provides internal engineering evidence by:

- checking exact source identity;
- self-testing the forbidden-marker scanner;
- creating Android/Windows/Apple inspection output;
- scanning/staging payloads;
- recording checksums/provenance;
- uploading internal artifacts;
- avoiding production signing secrets.

Internal artifacts are not production/store-ready packages.

## 23. Release Evidence

Release Evidence captures exact source/ref/run identity, tracked-source manifests/checksums, tests, dependency information, workspace integrity and evidence checksums.

Artifact existence alone does not imply approval; review the run conclusion/provenance.

## 24. Exact-source verification

Use `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` when verification-relevant source changes.

If a verification run fails:

1. inspect actual logs;
2. identify source/test/workflow/toolchain cause;
3. fix legitimate defects;
4. avoid broad suppression;
5. verify the corrected exact source;
6. record new evidence.

## 25. Release evidence records

Record:

- exact source SHA;
- verification/PR/run identities;
- test counts;
- platform results;
- CodeQL/dependency results;
- store-package/inspection results where applicable;
- whether a marker branch entered main;
- package checksums/signing provenance for production candidates.

Update current status/next steps only after evidence exists.

## 26. Funding/package boundary

The distributed application source/package contains no external Buy Me a Coffee destination/card/command/artwork.

Repository voluntary support metadata/docs remain separate and do not unlock health functionality, reminder reliability/priority or clinical services.

## 27. Production preparation

Before production release complete applicable:

- current exact-source automated matrix;
- real-device platform matrices;
- notification delivery/lifecycle;
- packaged SQLite/encrypted-data compatibility;
- accessibility;
- production signing outside Git;
- signed package inspection/checksums/provenance;
- current store policy/metadata/assets;
- exact immutable production tag;
- tagged release gates;
- publication evidence.

## 28. Hotfixes

For a production-blocking hotfix:

1. reproduce from released source;
2. apply smallest safe correction;
3. add regression test;
4. run affected automated/manual gates;
5. exact-source verify;
6. update status/release notes;
7. create a new version/tag rather than rewriting historical evidence.

## 29. Incident response

For privacy/security incidents:

- stop promotion when appropriate;
- scope affected versions/surfaces;
- avoid public sensitive reproduction data;
- fix/add regression coverage;
- rotate/revoke exposed credentials if needed;
- update security/risk docs;
- verify corrected source;
- publish a new version without rewriting history.

## 30. Current verified baseline

PR #74 frozen head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Verified 331/331 core tests plus all configured normal platform, store-candidate, inspection-artifact, CodeQL and Dependency Audit gates.

See `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

## 31. Current release blockers

Real-device behavior, package compatibility, accessibility, signing, final signed-package inspection, current store review/metadata, exact production tag/gates and publication remain open.

Use `docs/releases/NEXT_STEPS.md`.