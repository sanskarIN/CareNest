# CareNest Testing Guide

CareNest uses layered automated tests plus required manual release verification. A green automated matrix is necessary evidence but does not prove real-device notification delivery, accessibility, signing, or store compliance.

## Test projects

### `tests/CareNest.UnitTests`

Primary purpose:

- domain validation;
- deterministic reminder planning;
- schedule recurrence/date/state boundaries;
- ownership integrity;
- UTC/time-zone/DST semantics;
- property-style deterministic recurrence invariants.

Current verified PR #30 baseline: **74 passed**.

### `tests/CareNest.IntegrationTests`

Primary purpose:

- SQLite migrations/repository behavior;
- relationship cleanup;
- WAL configuration;
- snapshot content/integrity/cancellation;
- encrypted document round-trip/tamper behavior;
- encrypted backup restore/wrong-password/tamper behavior;
- report/export integration.

Current verified PR #30 baseline: **13 passed**.

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
- app-lock cryptographic/source contracts.

Current verified PR #30 baseline: **54 passed**.

### Verified total

PR #30 exact source head `c61f3c31c4ba33419c7b348fc8ee63a58eaa637b` passed **141 core tests** with 0 failed / 0 skipped.

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

## Reminder coordinator contract coverage

Source/UI-contract tests protect:

- explicit rebuild `DateTime` values must be UTC;
- snoozed occurrence must have a value;
- snooze must be UTC;
- snooze must be in the future;
- notification scheduling failure logging remains privacy-redacted;
- user-configured stock adjustment boundary remains explicit.

## SQLite integration strategy

Integration tests should use isolated temporary storage and clean up files.

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

## Backup tests

Backup tests cover:

- encrypted round-trip;
- correct-password restore;
- wrong-password rejection;
- tamper rejection;
- protected portability of required recovery material.

Future schema versions should add compatibility fixtures for historical formats.

## Encrypted document tests

Tests cover:

- encrypt/store/decrypt round-trip;
- tamper/authentication failure;
- separation from unencrypted normal document persistence.

Manual target-device file picker/share/delete behavior remains separately required.

## Reporting tests

Tests should verify:

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

## Dependency Audit

Dependency Audit checks platform-neutral and MAUI dependency graphs.

Current tracked SQLitePCLRaw advisory remains explicitly open even when the workflow succeeds under the narrow exact advisory suppression.

Do not interpret workflow success as a vulnerability fix.

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

## Current exact verified source evidence

PR #30 verified source head:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

Evidence:

- CareNest CI #248 / `31382194805`: success;
- formatting: success;
- UnitTests: 74 passed;
- IntegrationTests: 13 passed;
- UiTests: 54 passed;
- total: 141 passed;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #248 / `31382194687`: success;
- Dependency Audit #10 / `31382194683`: success.

PR #30 was closed without merging the verification marker.

## Manual release testing

Automated tests do not complete final release verification.

Use `docs/releases/MANUAL_TEST_MATRIX.md` for:

- onboarding/profile/medicine/schedule/log flows;
- permission denied/granted;
- real notification behavior/limitations;
- Android alarm/battery/reboot/time-zone behavior;
- document import/export/delete;
- calendar export;
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
- notification permission state automation;
- future schema backup compatibility fixtures;
- corruption/low-storage filesystem failure injection;
- deeper accessibility semantics checks;
- signed-artifact smoke tests after signing infrastructure exists;
- SBOM/attestation verification when release-artifact generation is introduced.