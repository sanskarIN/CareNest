# Contributing to CareNest

Thank you for improving CareNest.

CareNest is a local-first .NET MAUI health-organization project. Contributions must preserve the project's medical-safety, privacy, security, reminder-consistency, and release-evidence boundaries.

## Read first

Before changing source, read:

- `README.md`;
- `docs/README.md`;
- `docs/architecture/ARCHITECTURE.md`;
- `docs/setup/DEVELOPMENT.md`;
- `docs/setup/MAINTAINER_OPERATIONS.md`;
- `docs/testing/TESTING_GUIDE.md`;
- `PRIVACY.md`;
- `SECURITY.md`;
- `docs/security/SECURITY_MODEL.md`;
- `docs/security/THREAT_MODEL.md`;
- `PROJECT_STATUS.md`;
- `docs/releases/NEXT_STEPS.md`.

## Medical-safety boundary

Do not introduce features that:

- diagnose conditions;
- calculate/infer dosage;
- recommend treatment;
- check medication interactions as a clinical safety claim;
- create clinical risk scores;
- verify adherence as fact;
- substitute for emergency services;
- guarantee reminder delivery.

Medicine strength/instruction fields remain opaque user-entered text.

If a proposal would cross this boundary, it is not a normal contribution and requires explicit project-scope review before implementation.

## Local-first/privacy boundary

Do not add, without a separate reviewed architecture/privacy/security proposal:

- required accounts;
- CareNest backend/server storage;
- cloud synchronization;
- analytics/telemetry SDKs;
- automatic upload;
- remote caregiver sharing;
- hidden network requests;
- server-side health processing.

A future network feature must define consent, authentication, authorization, key ownership, deletion/export, retention, breach response, and store disclosures before merge.

## Secrets and real data

Never commit:

- signing keys/certificates/keystores;
- `.p12`, `.pfx`, `.jks` private signing files;
- API/service credentials;
- secret `.env` files;
- app-lock PINs;
- backup passwords;
- encryption keys;
- real user SQLite databases;
- real health documents;
- real CareNest backups;
- private health screenshots/fixtures.

Use synthetic/fictional test data.

## Architecture

Project roles:

- `CareNest.Shared` — small cross-layer primitives/constants;
- `CareNest.Domain` — entities/enums/domain validation;
- `CareNest.Application` — contracts/use cases/reminder planning/coordinators;
- `CareNest.Infrastructure` — SQLite/crypto/backup/reports;
- `CareNest.App` — MAUI UI/composition/platform implementations.

Do not put SQL/platform logic directly into ViewModels.

Do not add MAUI dependencies to platform-neutral projects.

Architecture contract tests enforce these boundaries.

## Reminder contributions

If changing reminders, review:

- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`;
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`;
- `docs/security/THREAT_MODEL.md`.

Preserve:

- explicit user-entered scheduling;
- ownership validation;
- UTC planning-window rules;
- half-open window `[fromUtc, toUtc)`;
- explicit time zone;
- state/date boundaries;
- as-needed no-automatic-reminder behavior;
- deterministic identity;
- DST gap/overlap rules;
- future-UTC snooze contract;
- snoozed `SnoozedUntilUtc` effective due time;
- cancellation of an existing OS request before replacement, suppression, invalidation, or handled-state persistence;
- retryable state when platform cancellation fails;
- retention of stale occurrence identity until old OS requests can be reconciled;
- medicine/profile/appointment database ↔ OS-scheduler compensation where those surfaces can fail independently;
- cancellation-first Taken/Skipped/Delayed/Missed/Snoozed/Cancelled actions;
- non-cancelled previous-state/rebuild compensation when a later handled-action step fails;
- no clinical inference.

Add regression tests for behavior changes, including failure-injection/ordering tests when persistence and OS scheduling cross separate state surfaces.

## Persistence/schema contributions

For a database change:

1. add a new ordered migration/version;
2. update entity/repository behavior;
3. add migration/integrity tests;
4. verify relationship cleanup;
5. review backup/restore compatibility;
6. update `docs/architecture/DATABASE_SCHEMA.md`;
7. update privacy/data lifecycle/store docs if data categories change.

Do not silently rewrite historical migration semantics.

## Backup/document cryptography changes

Read:

- `docs/architecture/BACKUP_AND_RESTORE.md`;
- `docs/security/SECURITY_MODEL.md`;
- `docs/security/THREAT_MODEL.md`.

Use platform-supported/.NET authenticated cryptographic primitives rather than inventing custom crypto.

Preserve wrong-password/tamper/truncation/trailing-data rejection, strict backup topology validation, and document-key portability.

New encrypted writes use authenticated chunked framing v2 while legacy v1 remains readable for compatibility until an explicit migration/deprecation decision is verified.

## App-lock contributions

Preserve:

- no plaintext PIN persistence;
- random salt;
- PBKDF2-HMAC-SHA256 verifier;
- fixed-time comparison;
- verifier-buffer clearing where practical;
- exact expected stored salt/verifier shape;
- fail-closed malformed/missing secure material;
- rollback around multi-key update/disable changes;
- deletion of stored material when disabled;
- explicit limitation that app lock is not whole-database/device encryption.

Biometric/recovery/remote unlock work requires separate security design.

## Logging changes

Do not log private health data or secrets.

Read `docs/security/LOGGING_PRIVACY.md`.

Routine logs should not contain:

- document contents;
- private free-text health notes;
- backup passwords;
- PINs;
- encryption keys;
- full exception messages/stack traces from sensitive user-data workflows;
- reminder record identifiers in scheduling/cancellation/recovery failure logs where avoidable.

## Local development

Follow `docs/setup/DEVELOPMENT.md` and `docs/setup/PLATFORM_SETUP.md`.

Basic checks:

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release

dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

Repository-local gates are also available:

```bash
build/scripts/quality-gate.sh
build/scripts/release-preflight.sh
```

PowerShell equivalents live beside those scripts. The gates run blocking unsuppressed NuGet audit checks; do not change them back to warning-only behavior.

For platform changes, build/smoke-test the affected MAUI target.

## Formatting

Use project-specific formatting checks matching CI:

```bash
dotnet format <project.csproj> --verify-no-changes
```

Do not rely on a full multi-target solution format/build on a host that lacks required workloads.

## Git identity

Project maintainer local setup:

```bash
git config --local user.name "Sanskar"
git config --local user.email "sanskarin@outlook.in"
```

Helper scripts:

- `build/scripts/setup-git.sh`;
- `build/scripts/setup-git.ps1`.

The helper scripts anchor themselves at the repository root, fail on native Git errors, set repository-local identity, and verify the configured values.

GitHub web/API commits may use the authenticated account's commit identity. Do not rewrite other contributors' authorship or falsely claim a connector/API commit used an arbitrary local email.

## Commit messages

Prefer small logical commits.

Prefixes commonly used in the repository:

- `feat:` product behavior;
- `fix:` defect correction;
- `security:` security hardening;
- `test:` regression/coverage;
- `docs:` documentation;
- `ci:` CI workflows/policies;
- `build:` build/release tooling;
- `chore:` maintenance.

Avoid mixing unrelated source, refactor, and documentation changes into one opaque commit.

## Tests

Every behavior fix should add/update the lowest appropriate test layer.

Use:

- unit tests for deterministic domain/application logic;
- integration tests for SQLite/crypto/files/backup/report integration;
- UI/source contract tests for XAML, architecture, ViewModel, security/privacy/repository/release workflow and script policies;
- manual test matrix for real target-device behavior.

See `docs/testing/TESTING_GUIDE.md`.

## Pull requests

A PR description should include:

- problem/user impact;
- implementation approach;
- medical-safety impact;
- privacy/security impact;
- schema/migration impact;
- backup/export impact;
- platform impact;
- dependency impact;
- workflow/release impact;
- tests run;
- manual checks run;
- known limitations;
- documentation changed.

Keep PRs reviewable and never include real user data.

## CI failures

Do not broadly suppress analyzers/tests merely to obtain green status.

Historical verification intentionally exposed and fixed real analyzer/path/privacy/nullability/reminder/release-policy defects.

When CI fails:

1. inspect the failing log;
2. fix source/test/workflow when legitimate;
3. rerun targeted checks;
4. rerun the full applicable exact-source matrix;
5. update evidence if the verified source changed.

## Exact-head verification

Runtime, test, workflow, package, platform configuration, or release-script hardening that changes verification-relevant source uses the marker-only protocol in:

`docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

The verification branch adds one marker file beyond an exact `main` source SHA. The marker PR is closed without merge.

Documentation-only changes may be recorded separately only when they truly do not change runtime/test/project/workflow/package/platform/release-script behavior.

## Dependency updates

After package changes:

- restore;
- run tests;
- run platform builds;
- run unsuppressed Dependency Audit;
- run CodeQL where applicable;
- inspect the transitive graph;
- record packaged compatibility evidence when native persistence/provider behavior changes.

For SQLite-related changes follow `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

The formerly tracked `GHSA-2m69-gcr7-jv3q` source exception has been remediated: maintained native/provider leaves are centrally pinned, the exact audit suppression is removed, and `SqliteDependencySecurityContractTests` guards the package floor/suppression absence. Do not reintroduce the old suppression merely because packaged existing-data/encrypted-data compatibility work is still pending; that remaining work is a separate production-release gate.

## Release workflow changes

Production tags matching `v*` are designed to run the exact tagged source through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Do not remove tag/manual entry points, weaken the Release Gate, make dependency audit warning-only, or make failed Release Evidence disappear without adding/updating executable regression contracts and re-running exact-source verification.

Release Evidence should preserve failed-run evidence, identify commit/run/attempt, and apply aggregate success only after evidence upload.

## Documentation requirements

Behavior changes should update the relevant documentation in the same PR/commit series.

Start at `docs/README.md`.

Update:

- user guide for observable user changes;
- feature reference;
- architecture/data docs;
- security/privacy docs;
- test guide/contract;
- release checklists/status where required;
- `what_changed.md` when a detailed handoff is requested.

Do not leave old dependency/verification facts in active setup/security/architecture documents after a new authoritative source baseline supersedes them.

## Accessibility

New/changed UI should follow `docs/design/ACCESSIBILITY.md`.

Preserve:

- semantic labels;
- text scaling;
- color-independent status;
- keyboard/focus behavior;
- contrast;
- reduced-motion usability.

Automated XAML contracts do not replace real screen-reader testing.

## Store/distribution changes

Any change affecting external support links, permissions, privacy disclosure, capabilities, or signing/package identity must update the store/release documentation.

Current store-policy rules are time-sensitive and must be verified at submission time.

## Code of conduct

All project participation is governed by `CODE_OF_CONDUCT.md`.

## Security reports

Use `SECURITY.md` for security reporting rather than publishing exploitable/private details in a normal public issue.
