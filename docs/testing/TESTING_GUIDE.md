# CareNest Testing Guide

CareNest uses layered automated tests plus required manual release verification. A green automated matrix is necessary evidence but does not prove real-device notification delivery, accessibility, signing, store compliance, or resolution of an explicitly tracked dependency risk.

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

Current verified PR #33 baseline: **106 passed**.

### `tests/CareNest.IntegrationTests`

Primary purpose:

- SQLite migrations/repository behavior;
- relationship cleanup;
- WAL configuration;
- snapshot content/integrity/cancellation;
- encrypted document round-trip/tamper/key-buffer behavior;
- chunked AEAD framing/truncation/legacy compatibility;
- encrypted backup restore/wrong-password/tamper/topology/key-buffer behavior;
- report/export integration.

Current verified PR #33 baseline: **30 passed**.

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
- reminder coordinator UTC/snooze safety contracts.

Current verified PR #33 baseline: **54 passed**.

### Verified total

PR #33 exact source head `4f5f9abe9d702fa33d6aba3f15c113febfebf95e` passed **190 core tests** with 0 failed / 0 skipped.

Documentation-only commits after that source head do not alter the verified runtime/test source.

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

PR #31 demonstrated that rule in this continuation: formatting passed, but unit-test compilation exposed CA1861 in a new profile-cleanup assertion. The test source was corrected on `main`; PR #31 was closed unmerged and superseded rather than weakening the analyzer policy.

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

The unit suite now directly tests the platform-neutral services instead of relying only on repository/UI-contract coverage.

### Profile service

- new profile → Created audit;
- existing profile → Updated audit;
- update timestamp comes from deterministic UTC `TimeProvider`;
- profile cascade deletion coordinates encrypted document/profile-photo cleanup;
- deletion audit is recorded without embedding health content in the safe summary.

### Medicine service

- create/update audit distinction;
- reminder rebuild after medicine changes;
- schedule persistence and future-occurrence invalidation;
- schedule rebuild after save;
- stock adjustment uses only explicit stored/user-entered quantities;
- a change that would produce negative estimated stock fails before persistence;
- medicine cascade delete triggers reminder rebuild.

### Appointment service

- `StartsUtc` must be actual `DateTimeKind.Utc`;
- local/unspecified values are rejected instead of relabeled with `DateTime.SpecifyKind`;
- time-zone identifiers are trimmed/validated;
- reminder due time derives from the explicit stored UTC instant and user-entered lead minutes;
- denied permission followed by a rejected permission request produces no platform schedule;
- permission-granted response allows scheduling;
- background/rebuild path does not prompt and does not schedule while permission is denied;
- a stored non-UTC appointment fails closed during rebuild;
- delete cancels the platform reminder before deleting the record.

### Document service

- encrypted metadata is persisted only after an encrypted payload exists;
- database-save failure removes the encrypted payload;
- audit failure after a database save rolls back both the database record and encrypted payload;
- cleanup failure is surfaced together with the original import failure rather than silently hidden;
- explicit export uses a safe leaf filename and records an Exported audit action;
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

## Reminder coordinator contract coverage

Source/UI-contract tests protect:

- explicit rebuild `DateTime` values must be UTC;
- snoozed occurrence must have a value;
- snooze must be UTC;
- snooze must be in the future;
- notification scheduling failure logging remains privacy-redacted;
- user-configured stock adjustment boundary remains explicit.

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
- invalid schema/document-count metadata rejection.

The backup package/container format version is distinct from the internal chunked AEAD framing version. The package can remain compatible while its encrypted payload framing evolves.

Future schema versions should add compatibility fixtures for historical backup package formats.

## Reporting tests

Tests verify:

- expected output type/content contract;
- privacy/non-clinical disclaimer presence;
- no clinical score/treatment conclusion;
- invariant formatting where machine-readable output requires it.

## App-lock tests

Source contracts protect:

- random salt generation;
- PBKDF2-HMAC-SHA256;
- configured iteration policy;
- fixed-time compare;
- no plaintext PIN persistence;
- clearing candidate/retrieved verifier buffers;
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

## CodeQL

CodeQL runs separately from the build/test matrix.

A successful CodeQL run is required automated evidence for the exact source baseline used in a release decision.

PR #33: CodeQL #332 / `31691592435` — success.

## Dependency Audit

Dependency Audit checks platform-neutral and MAUI dependency graphs.

PR #33: Dependency Audit #13 / `31691592302` — success.

The tracked SQLitePCLRaw advisory remains explicitly open even when the workflow succeeds under the narrow exact advisory suppression. Do not interpret workflow success as a vulnerability fix.

## Exact-head verification protocol

For major source hardening:

1. finish source/test changes on `main`;
2. freeze exact source SHA;
3. create temporary verification branch from that SHA;
4. add only one marker file under `build/verification/`;
5. open PR to `main`;
6. confirm marker-only diff;
7. run CI/CodeQL/Dependency Audit;
8. fix failures on `main` rather than weakening quality gates;
9. recreate verification from corrected exact head if needed;
10. record green run IDs;
11. close marker PR without merge.

See `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

## Verification sequence for this continuation

### PR #31 — superseded

Source `8e2607f287ca5777d9edbab445042f96c6bcfcec`.

Formatting passed, but unit-test compilation exposed CA1861 in a new constant-array assertion. The test was corrected on `main`; PR #31 was closed without merge and is not release evidence.

### PR #32 — service/document/backup hardening green baseline

Source `8a28bbf30692b2b0e98ec801dac1531d50d65db1`.

- CareNest CI #326 / `31690726676`: success;
- unit 106;
- integration 26;
- UI 54;
- total 186;
- four Release platform builds: success;
- CodeQL #326 / `31690726675`: success;
- Dependency Audit #12 / `31690726700`: success.

PR #32 was marker-only and closed without merge.

### PR #33 — current exact baseline

Source:

`4f5f9abe9d702fa33d6aba3f15c113febfebf95e`

Marker:

`62a0050a2622e12a31d00842778af0bc96355482`

Evidence:

- CareNest CI #332 / `31691592300`: success;
- formatting: success;
- UnitTests: **106 passed**;
- IntegrationTests: **30 passed**;
- UiTests: **54 passed**;
- total: **190 passed**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #332 / `31691592435`: success;
- Dependency Audit #13 / `31691592302`: success.

PR #33 changed only the verification marker beyond the exact source and was closed without merge.

## Manual release testing

Automated tests do not complete final release verification.

Use `docs/releases/MANUAL_TEST_MATRIX.md` for:

- onboarding/profile/medicine/schedule/log flows;
- appointment permission denied/granted and reminder behavior;
- real notification behavior/limitations;
- Android alarm/battery/reboot/time-zone behavior;
- document import/export/delete;
- calendar export;
- encrypted document v1/v2 real-target read/write behavior where historical fixtures are available;
- backup/restore on clean install;
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
- corruption/low-storage filesystem failure injection;
- deeper accessibility semantics checks;
- signed-artifact smoke tests after signing infrastructure exists;
- SBOM/attestation verification when release-artifact generation is introduced.