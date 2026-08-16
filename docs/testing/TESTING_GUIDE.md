# CareNest Testing Guide

**Current verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`  
**PR #74 frozen test/source head:** `8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

CareNest uses layered automated testing plus mandatory manual release validation. Automated success is necessary but does not prove real-device notification delivery, accessibility, signing, store approval or packaged existing-data compatibility.

## 1. Current verified test totals

PR #74 CareNest CI #735 / run `31938301209` passed:

- unit: **122 passed, 0 failed, 0 skipped**;
- integration: **39 passed, 0 failed, 0 skipped**;
- UI/source-policy: **170 passed, 0 failed, 0 skipped**;
- total: **331/331**.

The same source also passed Android, Windows, iOS simulator and Mac Catalyst Release builds.

Additional gates:

- Store Package Configuration #124 / `31938301146`: success;
- Store Inspection Artifacts #47 / `31938301275`: success;
- CodeQL #735 / `31938301252`: success;
- Dependency Audit #91 / `31938301172`: success.

These counts belong to that exact source boundary. Future test additions can legitimately increase them.

## 2. Unit tests

Project:

`tests/CareNest.UnitTests`

Primary responsibilities:

- domain validation;
- deterministic reminder planning;
- schedule recurrence/date/state boundaries;
- ownership integrity;
- UTC/time-zone/DST semantics;
- deterministic recurrence invariants;
- application-service orchestration with test doubles;
- profile/medicine/appointment/document/backup-reminder behavior without MAUI or SQLite.

Current verified count: **122**.

## 3. Integration tests

Project:

`tests/CareNest.IntegrationTests`

Primary responsibilities:

- SQLite migrations/repositories;
- relationship cleanup/cascades;
- WAL/snapshot/integrity behavior;
- reminder persistence/effective-due state;
- encrypted document round-trip/tamper/key behavior;
- chunked AEAD framing/truncation/trailing-data/legacy compatibility;
- encrypted backup restore/wrong-password/tamper/topology behavior;
- reports/exports;
- persistence/crypto cleanup and compensation.

Current verified count: **39**.

## 4. UI/source-policy tests

Project:

`tests/CareNest.UiTests`

This suite is primarily repository/source/XAML policy testing rather than full device UI automation.

Coverage includes:

- XAML semantics/accessibility expectations;
- strict compiled-binding contracts;
- route/navigation contracts;
- architecture dependency rules;
- ViewModel boundaries;
- data-model/source completeness;
- async non-blocking policies;
- logging/privacy contracts;
- app-lock source/crypto contracts;
- reminder UTC/snooze/reconciliation/compensation contracts;
- report export/cache contracts;
- SQLite dependency-security policy;
- release workflow/tag/manual entry points;
- dependency-audit event safety;
- release-evidence provenance/failure preservation;
- release-preflight/quality-gate fail-closed behavior;
- Git identity setup contracts;
- production Release Gate contracts;
- funding-free application package/source boundary.

Current verified count: **170**.

## 5. Compiled XAML binding tests

PR #74 added six dynamic compiled-binding contract tests.

The policy requires:

- root `x:DataType` on binding-bearing views;
- item-specific `x:DataType` on binding-bearing DataTemplates;
- source type information for explicit Source bindings;
- picker item type for `ItemDisplayBinding` when context changes;
- `MauiEnableXamlCBindingWithSourceCompilation=true`;
- `MauiStrictXamlCompilation=true`;
- `XC0022`, `XC0023`, `XC0024`, `XC0025` as errors;
- no matching `NoWarn` suppression;
- no `x:Object` / `x:Null` type-safety bypass.

See `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

## 6. Running tests locally

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

Use a clean checkout when validating release behavior.

## 7. Formatting gate

```bash
dotnet format src/CareNest.Shared/CareNest.Shared.csproj --verify-no-changes
dotnet format src/CareNest.Domain/CareNest.Domain.csproj --verify-no-changes
dotnet format src/CareNest.Application/CareNest.Application.csproj --verify-no-changes
dotnet format src/CareNest.Infrastructure/CareNest.Infrastructure.csproj --verify-no-changes
dotnet format tests/CareNest.UnitTests/CareNest.UnitTests.csproj --verify-no-changes
dotnet format tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj --verify-no-changes
dotnet format tests/CareNest.UiTests/CareNest.UiTests.csproj --verify-no-changes
```

Fix legitimate analyzer/formatter failures instead of weakening policy.

## 8. Reminder planner test strategy

Reminder tests protect a deterministic organizational contract, not a medical recommendation engine.

Covered concepts include:

- daily schedules;
- multiple explicit times;
- selected weekdays;
- cycle schedules;
- custom date ranges;
- schedule/medicine end dates;
- every-N-hours;
- follow-ups;
- disabled schedules;
- archived profile suppression;
- paused/completed/archived medicine suppression;
- as-needed/no automatic materialization;
- entity ownership mismatch rejection;
- invalid enum/state rejection;
- half-open UTC planning windows;
- duplicate explicit-time deduplication;
- chronological output;
- deterministic time-zone/DST behavior;
- invalid spring-forward local-time rejection;
- deterministic fall-back ambiguity handling;
- stable occurrence identity;
- explicit future UTC snooze validation.

See `REMINDER_SCHEDULING_CONTRACT.md`.

## 9. Reminder coordinator/reconciliation tests

Tests protect:

- persisted occurrence versus OS request reconciliation;
- stale request cleanup;
- cancellation before replacement/suppression/invalidation;
- cancellation-first handled actions;
- effective snooze due time;
- overdue handling;
- retryable cancellation failures;
- restoration/rebuild compensation after later failure;
- medicine/profile/appointment lifecycle cleanup.

The database and platform scheduler are separate state surfaces and must not be treated as one atomic transaction.

## 10. Appointment tests

Assertions include:

- `StartsUtc` requires true UTC;
- local/unspecified timestamps are rejected rather than relabeled;
- explicit reminder lead time;
- denied notification permission does not become successful scheduling;
- background rebuild does not repeatedly prompt;
- persistence/platform scheduling compensation.

## 11. Document-vault tests

Coverage includes:

- encrypted round trip;
- tamper rejection;
- current framing/version metadata;
- missing/corrupt key fail-closed behavior;
- secure-store failure cleanup;
- import metadata rollback;
- explicit export behavior;
- safe filenames;
- application-owned temporary-file cleanup.

## 12. Authenticated stream tests

Chunked AEAD testing includes:

- multi-chunk round trip;
- authenticated terminal record;
- truncation rejection;
- trailing-data rejection;
- legacy v1 read compatibility;
- key length enforcement;
- buffer clearing where managed-memory control permits.

## 13. Backup/restore tests

Coverage includes:

- encrypted backup round trip;
- wrong-password rejection;
- tamper/truncation/trailing-data rejection;
- SQLite snapshot correctness/integrity;
- strict archive topology;
- duplicate/unexpected entry rejection;
- document master-key portability rules;
- invalid metadata rejection;
- clean failure/rollback behavior.

## 14. SQLite/persistence tests

Coverage includes:

- migration idempotency/order;
- relationship cleanup;
- WAL/busy-timeout behavior;
- transactional multi-step operations;
- snapshot/integrity behavior;
- package dependency policy at source level.

A green integration suite does not replace a realistic packaged upgrade with representative prior data.

## 15. App-lock tests

Source/behavior contracts cover:

- no plaintext PIN persistence;
- salted PBKDF2-HMAC-SHA256 verifier flow;
- fixed-time comparison;
- material validation;
- secure-store ownership;
- disable/update rollback;
- verifier-buffer clearing where possible;
- fail-closed corrupt/missing material.

## 16. Report/export tests

Coverage includes:

- required disclaimers;
- CSV formula-like input neutralization;
- staged/atomic output behavior;
- cleanup of app-owned temporary report files;
- JSON/PDF/CSV integration behavior.

## 17. Architecture/source-policy tests

Repository policies protect against:

- MAUI dependency leaking into platform-neutral layers;
- direct SQL from ViewModels;
- casual network-client creation in local-first runtime;
- `async void`/`Task.Run` misuse in ViewModels where prohibited;
- common signing/private-key artifacts;
- sensitive logging regressions;
- incomplete release workflow/preflight behavior;
- reintroduction of removed external funding application surfaces.

## 18. Dependency security tests

`SqliteDependencySecurityContractTests` protects:

- maintained package/native/provider floor;
- removal of the former exact audit suppression;
- source dependency policy.

Dependency Audit remains separate and blocking.

## 19. GitHub Actions matrix

PR/source verification can include:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts.

Production-style tags additionally use Release Gate and Release Evidence as configured.

## 20. Store inspection tests/evidence

The inspection workflow self-tests the forbidden-marker scanner and generates internal Android/Windows/Apple artifacts with checksums/provenance.

These artifacts are engineering evidence, not production-signed/store-approved packages.

## 21. Manual Android matrix

Production validation still requires representative testing for:

- permission denied/granted;
- actual notification delivery;
- exact/inexact alarm/battery restrictions;
- reboot/restart/time-zone recovery;
- force-stop/vendor behavior;
- create/edit/delete reminder lifecycle;
- Taken/Skipped/Delayed/Missed actions;
- snooze replacement;
- document picker/share;
- backup/restore;
- app lock;
- accessibility.

## 22. Manual Windows matrix

Still requires:

- installed package behavior;
- running/closed-app reminder behavior;
- timer replacement/cancellation;
- reminder actions/snooze;
- restart/recovery;
- file/share;
- backup/restore;
- app lock;
- keyboard/focus;
- themes/accessibility.

## 23. Manual Apple matrix

Simulator compilation does not replace real-device/manual evidence.

Still requires appropriate iPhone/iPad/Mac Catalyst testing for permission/delivery, lifecycle, time zone, backup, documents, app lock, Dynamic Type/VoiceOver/keyboard/focus/themes and signed/notarized behavior.

## 24. Accessibility testing

Automated XAML semantics are necessary but insufficient. Manual validation should cover representative screen readers, large text/scaling, keyboard focus, contrast/theme, reduced motion and color-independent meaning.

## 25. Packaged compatibility testing

Before production release test representative fictional prior data through realistic package/update paths:

- SQLite opens/integrity/readability/editability;
- schema version/migrations;
- reminder reconciliation;
- encrypted document access/export;
- backup create/inspect/restore;
- wrong-password/tamper rejection;
- genuine historical fixtures where actual prior bytes exist.

Do not manufacture a new artifact and label it historical evidence.

## 26. Test data policy

Use fictional/synthetic data only in automated tests, screenshots, public issues and packaged migration fixtures.

Never commit real health data, real backups, PINs/passwords, keys, tokens or signing material.

## 27. Exact-source evidence rule

A test count is meaningful only with the source it verified.

Current PR #74 evidence belongs to:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Older PR #56/#54/etc. test counts remain historical evidence for older source boundaries, not current totals.

## 28. Release interpretation

CareNest remains `1.0.0-rc.1`. The configured automated matrix is green for the PR #74 source, but manual/device/package/accessibility/signing/store/tag/publication evidence remains open.

Use `docs/releases/NEXT_STEPS.md` for current release work.