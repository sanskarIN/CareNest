# CareNest Testing Guide

CareNest uses layered automated tests plus required manual release verification. A green automated matrix is necessary evidence but does not prove real-device notification delivery, accessibility, signing, store compliance, or packaged existing-data compatibility.

## Test projects

### `tests/CareNest.UnitTests`

Primary purpose:

- domain validation;
- deterministic reminder planning;
- schedule recurrence/date/state boundaries;
- ownership integrity;
- UTC/time-zone/DST semantics;
- property-style deterministic recurrence invariants;
- direct application-service behavior using deterministic test doubles;
- profile/medicine/appointment/document/backup-reminder orchestration without MAUI or SQLite.

Current authoritative PR #56 baseline: **122 passed**.

### `tests/CareNest.IntegrationTests`

Primary purpose:

- SQLite migrations/repository behavior;
- relationship cleanup;
- WAL configuration;
- snapshot content/integrity/cancellation;
- reminder effective-due/stale-occurrence behavior;
- encrypted document round-trip/tamper/key-buffer behavior;
- chunked AEAD framing/truncation/legacy compatibility;
- encrypted backup restore/wrong-password/tamper/topology/key-buffer behavior;
- report/export integration.

Current authoritative PR #56 baseline: **39 passed**.

### `tests/CareNest.UiTests`

This project primarily contains source/XAML/repository contract tests rather than full target-device UI automation.

Coverage includes:

- XAML semantic/accessibility expectations;
- route/navigation contracts;
- repository safety/completeness policies;
- architecture dependency rules;
- ViewModel boundaries;
- data-model requirements;
- branding/localization/support surfaces;
- async non-blocking source policies;
- logging-privacy contracts;
- app-lock cryptographic/source contracts;
- reminder coordinator UTC/snooze safety contracts;
- reminder platform-reconciliation and compensation contracts;
- report export/cache-lifecycle contracts;
- SQLite dependency-security pin/suppression contracts;
- exact release workflow tag/manual entry points;
- Dependency Audit event-safety contracts;
- Release Evidence provenance/failure-preservation/rerun-identity contracts;
- release-preflight blocking audit contracts;
- deterministic/fail-closed local quality-gate contracts;
- repository-local Git identity setup contracts;
- fail-closed production Release Gate contracts.

Current authoritative PR #56 baseline: **124 passed**.

### Verified total

Authoritative marker-only PR #56 passed **285 core tests** with 0 failed / 0 skipped, plus all four platform Release builds, CodeQL and unsuppressed Dependency Audit.

PR #56 froze source/base SHA:

`4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`

Its marker head was:

`e3bc621cea05364a69abee0dadbd71a67c17bddb`

Final evidence:

- CareNest CI #571 / `31770929379`: success;
- CodeQL #571 / `31770929382`: success;
- Dependency Audit #41 / `31770929383`: success.

PR #56 was closed without merge. Its marker is not part of `main`.

PR #54 remains the historical authoritative runtime bug-audit baseline. PR #55 is a superseded release-engineering checkpoint that passed 277/277 core tests, Android, Windows, CodeQL and unsuppressed audit before additional confirmed fixes required PR #56.

## Running tests locally

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

## Formatting gate

Platform-neutral projects are formatted independently:

```bash
dotnet format src/CareNest.Shared/CareNest.Shared.csproj --verify-no-changes
dotnet format src/CareNest.Domain/CareNest.Domain.csproj --verify-no-changes
dotnet format src/CareNest.Application/CareNest.Application.csproj --verify-no-changes
dotnet format src/CareNest.Infrastructure/CareNest.Infrastructure.csproj --verify-no-changes
dotnet format tests/CareNest.UnitTests/CareNest.UnitTests.csproj --verify-no-changes
dotnet format tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj --verify-no-changes
dotnet format tests/CareNest.UiTests/CareNest.UiTests.csproj --verify-no-changes
```

CI treats applicable correctness/security analyzer findings as blocking rather than suppressing real failures.

The verification history demonstrates that rule repeatedly:

- PR #31 exposed CA1861 in new profile-cleanup test source; it was fixed without suppression.
- PR #37 exposed CA1068 in the repository transaction-helper signature; the helper/call sites were corrected.
- PR #39 exposed CA1001 plus a formatter defect; source was corrected.
- PR #49 exposed CA1861 in new reminder-reconciliation expectation arrays; the tests were corrected rather than weakening analyzer policy.

## Reminder planner test strategy

Reminder tests protect a non-clinical deterministic contract.

Covered concepts include:

- daily schedules;
- multiple explicit times;
- selected weekdays;
- cycle schedules;
- custom date ranges;
- medicine/schedule end dates;
- every-N-hours;
- follow-ups;
- disabled schedules;
- as-needed no-automatic-reminder behavior;
- paused/completed/archived medicine suppression;
- archived-profile suppression;
- cross-entity ownership mismatch rejection;
- schedule-time ownership mismatch rejection;
- half-open UTC planning windows;
- rejection of local/unspecified planning-window kinds;
- chronological output;
- duplicate explicit time deduplication;
- stable occurrence identity;
- DST invalid spring-forward times;
- deterministic ambiguous fall-back times;
- representative DST zones in US/UK/Australia/New Zealand;
- deterministic randomized recurrence/window property cases.

See `REMINDER_SCHEDULING_CONTRACT.md`.

## Schedule validation tests

Domain validation tests cover:

- known enum/schedule kind;
- valid start/end ordering;
- known time-zone identifier;
- valid hour/minute ranges;
- selected weekday mask non-empty;
- every-N-hours explicit interval 1–168;
- exactly one every-N-hours starting time;
- cycle positive on/off day values;
- as-needed no automatic time requirement;
- medicine opaque strength/instruction behavior;
- stock value boundaries.

Tests validate configuration shape, not clinical suitability.

## Application-service tests

The unit suite directly tests platform-neutral services instead of relying only on repository/UI-contract coverage.

### Profile service

- new profile → Created audit;
- existing profile → Updated audit;
- update timestamp comes from deterministic UTC `TimeProvider`;
- profile cascade deletion coordinates encrypted document/profile-photo cleanup;
- future platform reminder requests are reconciled/cancelled around profile lifecycle transitions;
- failed profile deletion after platform cancellation attempts non-cancelled reminder rebuild compensation;
- deletion audit is recorded without embedding health content in the safe summary.

### Medicine service

- create/update audit distinction;
- reminder rebuild/reconciliation after medicine changes;
- schedule persistence while retaining old occurrence identities long enough to cancel stale platform requests;
- schedule rebuild after save;
- stock adjustment uses only explicit stored/user-entered quantities;
- a change that would produce negative estimated stock fails before persistence;
- medicine cascade deletion cancels/reconciles future platform requests and compensates on persistence failure.

### Appointment service

- `StartsUtc` must be actual `DateTimeKind.Utc`;
- local/unspecified values are rejected instead of relabeled with `DateTime.SpecifyKind`;
- time-zone identifiers are trimmed/validated;
- reminder due time derives from the explicit stored UTC instant and user-entered lead minutes;
- denied permission followed by a rejected permission request produces no platform schedule;
- permission-granted response allows scheduling;
- background/rebuild path does not prompt and does not schedule while permission is denied;
- a stored non-UTC appointment fails closed during rebuild;
- save/delete paths reconcile platform reminder state around persistence failures;
- delete cancels the platform reminder before deleting the record.

### Document service

- encrypted metadata is persisted only after an encrypted payload exists;
- database-save failure removes the encrypted payload;
- audit failure after a database save rolls back both the database record and encrypted payload;
- cleanup failure is surfaced together with the original import failure rather than silently hidden;
- explicit export uses a safe leaf filename and records an Exported audit action;
- successful decrypted temporary output remains under managed cache ownership;
- delete is idempotent for missing records.

### Backup reminder coordinator

- disabled reminder setting cancels the existing reminder;
- background sync does not request permission;
- denied permission produces no schedule;
- an explicitly requested but still denied permission produces no schedule;
- next reminder derives from the last successful backup or current UTC time;
- overdue reminder is moved to a near-future time instead of scheduling in the past;
- sound/vibration preferences are honored.

Reusable deterministic test doubles live under `tests/CareNest.UnitTests/TestDoubles/`.

## Reminder coordinator and reconciliation coverage

Source/UI-contract and direct integration tests protect:

- explicit rebuild `DateTime` values must be UTC;
- snoozed occurrence must have a value;
- snooze must be UTC;
- snooze must be in the future;
- future snooze remains upcoming even after its original scheduled time has passed;
- overdue snooze is evaluated by `SnoozedUntilUtc` rather than stale original time;
- stale future occurrence is cancelled/reconciled instead of remaining represented as a live platform request;
- existing OS request is cancelled before replacement/suppression/invalidation;
- quiet-hours rebuild can cancel a previously scheduled request rather than only skipping a replacement;
- platform cancellation failure leaves state retryable;
- caller cancellation propagates;
- medicine/profile delete flows compensate if persistence fails after platform cancellation;
- medicine/profile save flows reconcile before later non-critical audit bookkeeping;
- appointment persistence has platform reminder compensation coverage;
- notification scheduling/cancellation failures are injectable in tests;
- logging remains privacy-redacted;
- user-configured stock adjustment boundary remains explicit.

### Reminder action cancellation/recovery tests

Handled reminder actions have dedicated ordering/failure coverage.

Tests protect the sequence:

1. cancel the old platform request;
2. persist Taken/Skipped/Delayed/Missed/Snoozed/Cancelled state only after cancellation succeeds;
3. schedule a replacement snooze only after state persistence;
4. if a later step fails, restore the previous occurrence state with non-cancelled compensation;
5. attempt non-cancelled rebuild so a still-actionable platform request can be restored;
6. aggregate primary/recovery failures instead of reporting false consistency.

Post-success audit bookkeeping and user-configured stock bookkeeping are tested/contracted so they do not incorrectly falsify an already completed reminder action.

## SQLite integration strategy

Integration tests use isolated temporary storage and clean up files.

Key invariants:

- migration sequence is deterministic;
- schema relationships persist correctly;
- relationship deletion behavior is correct;
- WAL mode is enabled as expected;
- busy timeout is configured;
- result-producing PRAGMA calls are consumed correctly;
- snapshots preserve committed data;
- copied snapshot passes SQLite integrity check;
- pre-cancelled snapshot leaves no output file.

## SQLite dependency-security tests

`SqliteDependencySecurityContractTests` protects the remediation for the previously tracked `GHSA-2m69-gcr7-jv3q` native dependency path.

Current source requirements include:

- `SQLitePCLRaw.lib.e_sqlite3` at or above `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` at or above `2.1.12`;
- selected provider packages at or above `2.1.12`;
- no restoration of the former advisory `NuGetAuditSuppress` entry.

The direct `sqlite-net-pcl`/bundle API path remains compatible with the application while central transitive pinning selects the maintained native/provider leaves.

This automated contract proves package-policy intent; it does not replace packaged existing-database/backup/encrypted-document compatibility testing on representative devices/builds.

## Chunked AEAD framing tests

CareNest uses a shared chunked AES-256-GCM stream helper for document and backup payload protection.

New writes use framing version 2. Tests protect:

- multi-chunk v2 round-trip;
- each data chunk authenticated with counter/length-bound AAD;
- final zero-length record authenticated using the next chunk counter;
- a valid authenticated prefix cannot be made to look complete merely by substituting a terminal from a later counter;
- bytes after the terminal record are rejected;
- 32-byte AES-256 key requirement;
- legacy v1 stream remains readable for backward compatibility.

Important compatibility statement: v2 protects newly encrypted streams. Existing v1 ciphertext is not retroactively rewritten or described as receiving v2 terminal authentication.

## Encrypted document tests

Tests cover:

- encrypt/store/decrypt round-trip;
- tamper/authentication failure;
- new encrypted document metadata records stream encryption version 2;
- caller-owned copies of the 32-byte document master key are cleared after import/export where managed-memory control permits;
- generated key buffer is cleared if secret-store persistence fails;
- missing/corrupt master key with existing encrypted payload fails closed instead of creating an unrelated replacement key;
- separation from unencrypted normal document persistence.

Manual target-device file picker/share/delete behavior remains separately required.

## Backup tests

Backup tests cover:

- encrypted round-trip;
- correct-password restore;
- wrong-password rejection;
- tamper rejection;
- protected portability of required recovery material;
- caller-owned document-key copy clearing after backup use;
- password-derived encryption key/salt clearing after crypto paths;
- strict decrypted-archive topology before extraction;
- duplicate entry rejection;
- nested document-entry rejection;
- non-`.cndoc` document-entry rejection;
- unexpected entry rejection;
- manifest document-count consistency;
- valid 32-byte document-key requirement when documents exist;
- invalid schema/document-count metadata rejection;
- completed backup/restore is distinguished from later best-effort local bookkeeping;
- failed restore restores exact prior secure-store key bytes where prior bytes existed.

The backup package/container format version is distinct from the internal chunked AEAD framing version. The package can remain compatible while its encrypted payload framing evolves.

Future schema versions should add compatibility fixtures for historical backup package formats.

## Reporting tests

Tests verify:

- expected output type/content contract;
- privacy/non-clinical disclaimer presence;
- no clinical score/treatment conclusion;
- invariant formatting where machine-readable output requires it;
- CSV formula-like user text is neutralized in portable spreadsheet output;
- CSV/PDF/JSON writers use staging + atomic final move;
- incomplete plaintext staging is cleaned after failures/cancellation;
- application-owned shared report cache files are removed after share handoff where CareNest still owns the temporary copy.

## App-lock tests

Source contracts protect:

- random salt generation;
- PBKDF2-HMAC-SHA256;
- configured iteration policy;
- fixed-time compare;
- no plaintext PIN persistence;
- clearing candidate/retrieved verifier buffers;
- exact salt/verifier length handling;
- fail-closed corrupt/missing secure material;
- rollback around multi-key update/disable transitions;
- deletion of stored material when lock is disabled;
- PIN format policy.

Manual secure-storage/cold-start behavior remains a target-device test.

## Repository policy tests

Policy tests intentionally act as executable project rules.

They detect regressions such as:

- committed runtime TODO/FIXME/NotImplemented placeholders;
- accidental runtime network/telemetry client introduction;
- named diagnosis/dosage/treatment/interaction/risk-scoring feature regressions;
- common signing/secret files;
- removal of required governance/release files;
- generated `bin`/`obj` content being mistaken for committed source.

## Release workflow/script policy tests

The final UI-contract suite includes dedicated executable release-engineering contracts.

### `ReleaseWorkflowContractTests`

Protects:

- exact `v*` tag/manual execution for CI, CodeQL and Dependency Audit;
- exact `v*` tag/manual execution for Release Gate and Release Evidence;
- PR-only Dependency Audit diff metadata guard;
- Release Evidence tracked-source provenance;
- independent outcome capture for unit/integration/UI/dependency/workspace evidence;
- evidence upload before aggregate failure;
- 90-day retention;
- artifact identity containing commit SHA + run ID + run attempt.

### `ReleasePreflightContractTests`

Protects:

- blocking `NuGetAudit=true` / `NuGetAuditMode=all` behavior;
- absence of the former warning-only ignored SQLite-audit behavior;
- PowerShell failure on platform-neutral and MAUI dependency-audit errors.

### `QualityGateScriptContractTests`

Protects:

- clean-checkout-safe test execution;
- all three core test projects;
- blocking unsuppressed dependency audit;
- Bash fail-fast semantics;
- PowerShell native-command exit-code checks.

### `GitSetupScriptContractTests`

Protects:

- repository-local `user.name` / `user.email` configuration;
- requested `Sanskar` / `sanskarin@outlook.in` values;
- repository-root anchoring;
- Bash/PowerShell fail-closed native Git behavior.

### `ReleaseGateContractTests`

Protects:

- nested unchecked release-checklist detection;
- case/indentation-safe open-risk detection;
- required release/security/evidence documents;
- explicit job timeout presence.

## Architecture tests

Architecture contracts verify:

- project reference direction;
- Shared/Domain/Application/Infrastructure separation;
- no MAUI dependency in platform-neutral projects;
- MAUI app remains the composition/platform project;
- path parsing works cross-platform.

## ViewModel tests

Contracts verify concrete ViewModels do not:

- directly access SQLite;
- create network clients;
- use common synchronous task-blocking techniques;
- request notification permission during onboarding;
- bypass explicit as-needed reminder rules.

## Logging privacy tests

Tests scan committed runtime source rather than generated output.

They protect against:

- full exception-object logger calls in sensitive operation paths;
- logging user-data record IDs in reminder failures;
- generated source causing false positives;
- removal of required safe logging patterns/guards.

## Accessibility/XAML tests

Automated checks can verify source-level semantic intent but cannot certify screen-reader usability.

Use automated checks for:

- semantic labels;
- required warning/resource keys;
- accessible support action intent;
- route/control presence.

Use manual tests for:

- screen readers;
- focus order;
- keyboard navigation;
- text scaling;
- contrast;
- reduced motion.

## Platform build verification

CI builds:

- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release.

Each platform job installs the necessary target workload rather than requiring all workloads on one runner.

Authoritative PR #56: all four Release builds succeeded.

## CodeQL

CodeQL runs separately from the build/test matrix.

A successful CodeQL run is required automated evidence for the exact source baseline used in a release decision.

Authoritative PR #56: CodeQL #571 / `31770929382` — **success**.

## Dependency Audit

Dependency Audit checks platform-neutral/test and Android MAUI dependency graphs.

Authoritative PR #56: Dependency Audit #41 / `31770929383` — **success without the former SQLite advisory suppression**.

The earlier narrow exact-advisory suppression was temporary evidence-management, not remediation. It has been removed from current source after the compatible maintained native/provider path was established.

Do not interpret this successful audit as proof of packaged existing-database or encrypted-data compatibility; those remain manual release evidence.

## Local quality/preflight verification

`build/scripts/quality-gate.sh` and `quality-gate.ps1` are intended to work from a clean checkout and run formatting/build/tests plus blocking unsuppressed dependency audit.

`build/scripts/release-preflight.sh` and `release-preflight.ps1` add release source hygiene, core Release builds, all three test projects, blocking dependency audit, and optional selected MAUI target audit/build through `CARENEST_TARGET`.

Dependency audit failure is not warning-only and must not be ignored with `|| true` or equivalent logic.

## Production-tag workflow behavior

Tags matching `v*` are configured to run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

A tag is not approved for production publication merely because it exists. Every required tagged workflow plus manual/device/accessibility/store/signing/packaged-data evidence must be complete.

Release Evidence preserves available failure evidence before applying its aggregate pass/fail gate. Artifact existence alone is not a successful release decision.

## Exact-head verification protocol

For verification-relevant source hardening:

1. finish source/test/workflow/package/build-script changes on `main`;
2. freeze exact source SHA;
3. create temporary verification branch from that SHA;
4. add only one marker file under `build/verification/`;
5. open PR to `main`;
6. confirm marker-only diff;
7. run CI/CodeQL/Dependency Audit;
8. fix failures on `main` rather than weakening quality gates;
9. recreate verification from corrected exact head if needed;
10. record green run IDs/test totals;
11. close marker PR without merge.

See `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

## 2026-08-14 verification sequence

The final audit intentionally retained failed/superseded checkpoints as evidence rather than reusing partial green results.

- PR #37 exposed CA1068 in the new transaction helper; fixed without suppression.
- PR #39 exposed CA1001 plus formatting failure; source corrected and the accidentally merged failed marker was explicitly removed.
- PR #40 had four platform builds/CodeQL/audit green but core formatting failed; it was not promoted.
- PRs #41/#42 were intentionally superseded while behavior work was still changing source.
- PR #43 was **not green**: core integration tests failed and UI-contract tests were skipped, despite platform/CodeQL/audit success.
- PR #44 independently reproduced future-snooze, overdue-snooze and stale-occurrence defects; source corrected.
- PR #46 exposed broader OS-reminder reconciliation lifecycle contracts; source corrected.
- PR #47 proved an unsuppressed SQLite dependency graph could audit successfully, but source moved afterward.
- PR #48 passed unsuppressed audit and CodeQL but exposed a transient moving-base reminder-interface compile mismatch; source corrected/simplified.
- PR #49 exposed CA1861 in new reminder-reconciliation expectations; test source corrected.
- PR #50 again passed unsuppressed SQLite audit but predated later analyzer-safe tests.
- PRs #51/#52 were superseded when later runtime/test source changed.
- PR #53 completed duplicate fully green bug-audit verification.
- PR #54 completed the authoritative runtime bug-audit source baseline: 261/261 core tests + all platforms + CodeQL + unsuppressed audit.
- PR #55 completed 277/277 core tests + Android + Windows + CodeQL + unsuppressed audit before being superseded by further confirmed release-tooling/documentation corrections.
- PR #56 is the authoritative current release-engineering baseline: 285/285 core tests + all four platform builds + CodeQL + unsuppressed audit.

### PR #56 — authoritative current baseline

Source/base SHA:

`4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`

Marker head:

`e3bc621cea05364a69abee0dadbd71a67c17bddb`

Evidence:

- CareNest CI #571 / `31770929379`: success;
- formatting: success;
- UnitTests: **122 passed**;
- IntegrationTests: **39 passed**;
- UiTests: **124 passed**;
- total: **285 passed**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #571 / `31770929382`: success;
- Dependency Audit #41 / `31770929383`: success without the former SQLite advisory suppression.

PR #56 changed only the verification marker beyond its frozen source boundary and was closed without merge.

See `docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md` for the authoritative final evidence record and `docs/releases/BUG_AUDIT_VERIFICATION_20260814.md` for the detailed failure-driven runtime audit history.

## Manual release testing

Automated tests do not complete final release verification.

Use `docs/releases/MANUAL_TEST_MATRIX.md` for:

- onboarding/profile/medicine/schedule/log flows;
- appointment permission denied/granted and reminder behavior;
- real notification behavior/limitations;
- cancellation-first reminder action behavior against actual platform scheduling and restart/recovery;
- Android alarm/battery/reboot/time-zone behavior;
- document import/export/delete;
- calendar export;
- report share/cache lifecycle;
- encrypted document v1/v2 real-target read/write behavior where historical fixtures are available;
- backup/restore on clean install;
- representative packaged SQLite upgrade/install with fictional pre-remediation data;
- structured-data readability after the SQLite native/provider update;
- existing encrypted document/backup compatibility after the package update;
- app-lock cold start;
- accessibility;
- themes;
- privacy/log review.

## Testing a bug fix

For each defect:

1. reproduce with the smallest safe case;
2. add a regression test at the lowest appropriate layer;
3. implement the fix;
4. run targeted test;
5. run full relevant project tests;
6. run formatting;
7. run platform builds if runtime/MAUI/platform source changed;
8. update documentation/checklists if user-visible behavior or release evidence changed.

## Test data

Use fictional/synthetic data in automated tests, screenshots, and public reproduction cases.

Never commit real health records, backups, PINs, credentials, or signing material.

## Flaky-test rule

Do not solve deterministic test failures by retries or broad sleeps without understanding the cause.

Reminder/time-zone tests should use explicit dates/time zones/fixed time providers rather than relying on the runner's current wall-clock state.

## Future automated testing roadmap

Still valuable for later versions:

- full target UI automation on stable emulator/device infrastructure;
- notification permission state automation on real/emulated platform APIs;
- historical backup-package fixtures across future schema/package versions;
- retained legacy encrypted-document v1 fixture tests from a released build once a canonical fixture exists;
- packaged existing-database upgrade fixtures for future SQLite/provider changes;
- corruption/low-storage filesystem failure injection;
- deeper accessibility semantics checks;
- signed-artifact smoke tests after signing infrastructure exists;
- SBOM/attestation verification when release-artifact generation is introduced.
