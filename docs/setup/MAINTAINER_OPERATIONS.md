# CareNest Maintainer Operations Guide

This guide is for maintainers working on CareNest source, CI, release evidence, dependencies, documentation, and production preparation.

## Maintainer identity

Requested local Git identity:

```bash
git config user.name "Sanskar"
git config user.email "sanskarin@outlook.in"
```

Use the repository setup scripts:

```bash
build/scripts/setup-git.sh
```

or:

```powershell
./build/scripts/setup-git.ps1
```

GitHub connector/API-generated commits may use the authenticated GitHub identity because GitHub's contents API does not accept an arbitrary author email through the connector used for repository maintenance. Do not falsely claim otherwise.

## Main-branch rule

Normal source/documentation state lives on `main`.

Temporary exact-head verification branches are intentionally marker-only and are closed without merging.

Do not leave verification marker files in `main`.

## Before making changes

Review:

- `README.md`;
- `PROJECT_STATUS.md`;
- `DECISIONS.md`;
- `what_changed.md`;
- `docs/README.md`;
- `docs/releases/NEXT_STEPS.md`;
- `docs/security/DEPENDENCY_RISK_REGISTER.md`.

Identify whether the work changes:

- runtime behavior;
- persisted schema;
- security/privacy boundary;
- reminder semantics;
- platform permissions;
- backup format;
- dependencies;
- only documentation.

## Commit discipline

Prefer small logical commits with descriptive messages.

Examples:

```text
feat: add ...
fix: correct ...
security: harden ...
test: cover ...
docs: document ...
ci: verify ...
build: configure ...
chore: maintain ...
```

A commit should be reviewable as one coherent change.

## Runtime source changes

For runtime behavior:

1. inspect existing domain/application/infrastructure/UI boundary;
2. implement at the lowest correct layer;
3. add regression tests;
4. update related documentation;
5. run formatting/tests;
6. run exact-head cross-platform verification before claiming a new green baseline.

Do not bypass architecture boundaries for convenience.

## Medical-safety boundary

Never add runtime behavior that:

- diagnoses;
- recommends treatment;
- calculates/infers dosage;
- checks medication interactions as a clinical feature;
- creates clinical risk scores;
- presents reminder delivery as guaranteed;
- substitutes for emergency services.

Strength/instruction text remains opaque user-entered data.

## Reminder changes

Any reminder-planner/coordinator change must consider:

- entity ownership;
- explicit schedule kind;
- date boundaries;
- UTC planning-window contract;
- time-zone identity;
- DST gap/overlap behavior;
- stable occurrence identity;
- duplicate handling;
- state suppression;
- as-needed behavior;
- snooze future-UTC requirement;
- platform delivery limitations.

Update `docs/testing/REMINDER_SCHEDULING_CONTRACT.md` if the deterministic contract changes.

## Schema changes

For a SQLite schema change:

1. add a new ordered migration/version;
2. never rewrite historical migration meaning casually;
3. update `docs/architecture/DATABASE_SCHEMA.md`;
4. add migration/integrity tests;
5. review cascade/relationship cleanup;
6. review backup/restore compatibility;
7. review export/report behavior;
8. update privacy/data lifecycle/store disclosures if data categories change.

## Dependency updates

For NuGet/package changes:

- update central package definitions where applicable;
- run restore/test/build matrix;
- run Dependency Audit;
- run CodeQL for source/security-relevant changes;
- inspect transitive dependency changes;
- update third-party notices if required;
- update dependency risk register if advisory status changes.

### SQLite-specific rule

Do not change sqlite-net/SQLitePCLRaw provider/bundle versions without following `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

Current tracked advisory remains open until actually resolved/approved.

## Security changes

Read:

- `SECURITY.md`;
- `docs/security/SECURITY_MODEL.md`;
- `docs/security/THREAT_MODEL.md`;
- `docs/security/LOGGING_PRIVACY.md`.

Security-sensitive changes should add automated regression coverage where practical.

Do not weaken analyzer/security gates to make a real finding disappear.

## Logging changes

Do not log raw user health data for convenience.

Prefer safe metadata such as exception type/category only when needed and only when the log level is enabled.

Do not log:

- document content;
- private free text;
- backup passwords;
- app-lock PIN;
- cryptographic keys;
- raw exception messages/stack traces from sensitive workflows.

## App-lock changes

Preserve:

- no plaintext PIN persistence;
- random salt;
- PBKDF2-HMAC-SHA256 verifier;
- fixed-time comparison;
- verifier-buffer clearing where possible;
- deletion of lock material when disabled;
- explicit limitation that app lock is not whole-database encryption.

Biometric/recovery/remote unlock features require a separate threat-model decision.

## Document-vault changes

Any document-storage change must review:

- encryption/authentication;
- key storage;
- import/export/share boundary;
- temporary plaintext files;
- deletion cleanup;
- backup portability;
- logging;
- platform file picker behavior.

## Backup changes

Any backup-format change must:

- increment/define format compatibility explicitly;
- add compatibility tests;
- preserve wrong-password/tamper rejection;
- verify SQLite snapshot correctness;
- verify document-key portability;
- update `docs/architecture/BACKUP_AND_RESTORE.md`;
- update release/manual test matrices.

## Documentation changes

For documentation-only changes:

- keep factual claims tied to implemented behavior;
- do not mark manual tests complete without evidence;
- do not describe advisories as fixed unless the dependency graph actually changed and passed verification;
- keep links consistent;
- update `docs/README.md` for major new documents;
- update `what_changed.md` when the user requests a detailed handoff.

Documentation-only commits after a verified source head should be explicitly described as documentation-only rather than a new runtime verification baseline.

## CI workflows

Key workflows include:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- Release Evidence.

CI verifies formatting/core tests/platform Release builds.

CodeQL and Dependency Audit are independent security/dependency evidence.

Release Gate intentionally prevents production promotion while tracked release blockers remain.

## Exact-head verification

Use `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

Do not merge the marker PR.

A source change after verification makes the old verification stale for that new source.

## Failure handling

If CI exposes an analyzer/compiler/test failure:

1. read the actual log;
2. determine whether it is source, test, toolchain, or infrastructure;
3. fix source/test when the finding is legitimate;
4. avoid broad suppression;
5. close stale marker PR;
6. verify corrected exact source again.

Historical verification PRs intentionally exposed multiple analyzer/privacy/path/nullability defects and were closed without merge after corrections.

## Release evidence records

When a verification succeeds, record:

- exact source SHA;
- marker SHA;
- PR number;
- CI run number/ID;
- test counts;
- platform build results;
- CodeQL run number/ID;
- Dependency Audit run number/ID;
- whether marker was closed without merge.

Update:

- `PROJECT_STATUS.md`;
- `docs/releases/RELEASE_CHECKLIST.md`;
- `docs/releases/QUALITY_GATE.md`;
- `docs/releases/NEXT_STEPS.md`;
- `CHANGELOG.md` when appropriate;
- `what_changed.md`.

## Documentation-only head verification

If the only commits after a verified source SHA are `.md` documentation files, use a source-to-head compare and record that no runtime/test/project/workflow/package/platform source changed.

Do not imply platform compilation was rerun for documentation-only commits unless it actually was.

## Manual testing records

`docs/releases/MANUAL_TEST_MATRIX.md` is evidence, not a wishlist.

Fill rows only after performing the test on the named platform/build.

Record device/OS/build and result notes.

## Store submissions

Use `docs/releases/STORE_SUBMISSION_CHECKLIST.md`.

Store policy is time-sensitive. Review current rules at submission time, especially external funding links and health/privacy declarations.

## Signing

Signing keys/certificates/profiles remain outside Git.

Use secure CI secret stores or controlled local signing systems.

Never place signing secrets in issue comments, Actions logs, or repository files.

## Project-support link

Canonical URL:

`https://buymeacoffee.com/sanskarIN`

It is voluntary support only.

Do not make funding unlock medical functionality, change reminder priority, expose local data, or imply medical service/support.

## Support contacts

Business: `sanskarin@outlook.in`

Support: `supportramsandesh@gmail.com`

Creator: `https://www.github.com/sanskarIN`

Public support requests should use synthetic/redacted data.

## Final public-release rule

Do not publish/tag final production merely because repository source is complete.

Final promotion requires the real release blockers in `PROJECT_STATUS.md`, `NEXT_STEPS.md`, and `RELEASE_CHECKLIST.md` to be resolved and evidenced.