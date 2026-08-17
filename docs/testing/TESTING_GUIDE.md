# CareNest Testing Guide

**Release line:** `1.0.0-rc.1`  
**Latest fully verified pre-Gumroad source:** `7cbe5568b6cffa06c279b29f3cb1b107ea988791`  
**Current Gumroad rollout:** exact latest source and workflow state are tracked in `what_changed.md`

CareNest uses layered automated testing plus mandatory manual release validation. Automated success is necessary but does not prove real-device notification delivery, accessibility, signing, store approval or packaged existing-data compatibility.

## 1. Latest fully verified pre-Gumroad totals

Exact source:

`7cbe5568b6cffa06c279b29f3cb1b107ea988791`

That exact source passed:

- unit: **122/122**;
- integration: **39/39**;
- UI/source-policy: **173/173**;
- total: **334/334**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- all four store-candidate configurations: success;
- CodeQL: success.

These counts belong only to that exact source boundary.

The Gumroad rollout adds independent repository-placement/accessibility source-policy coverage and modifies the store-payload scanner contract. A new count becomes authoritative only after the exact final Gumroad revision completes CI.

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

Latest fully verified count: **122**.

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

Latest fully verified count: **39**.

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
- runtime line-level defect-pattern scanning;
- structured runtime file syntax parsing;
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
- repository-only external-commerce placement;
- Gumroad badge accessibility and package isolation;
- store-payload Gumroad/Buy Me a Coffee marker enforcement.

Latest fully verified pre-Gumroad count: **173**.

The current branch contains additional Gumroad source-policy coverage, so do not hard-code 173 as the expected latest count after the rollout.

## 5. Compiled XAML binding tests

The compiled-binding contract suite requires:

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

## 6. Runtime source-line quality tests

`SourceLineQualityContractTests.cs` deterministically scans every runtime C# file under `src/` and reports repository-relative file/line failures.

The current broad line audit rejects known defect patterns including:

- unresolved merge-conflict markers;
- `TODO`;
- `FIXME`;
- `HACK`;
- `NotImplementedException`;
- `.GetAwaiter().GetResult()`;
- common `Task.Result` access forms;
- `Thread.Sleep(`;
- `Task.WaitAll(`;
- `Task.WaitAny(`;
- `throw ex;`.

The line audit does not blindly classify all current-clock reads as defects. Time correctness is protected by more specific date/time/reminder contracts.

## 7. Structured runtime file syntax tests

The source quality contract also parses structured runtime files under `src/`, including:

- `.xaml`;
- `.csproj`;
- `.props`;
- `.targets`;
- `.xml`;
- `.plist`;
- `.resx`;
- `.json`.

XML-family inputs use `XDocument`; JSON uses `JsonDocument`. This produces focused repository-policy failures in addition to compiler/platform build checks.

## 8. Gumroad/repository commerce tests

### `FundingLinkContractTests.cs`

Protects:

- Buy Me a Coffee visibility in repository support material;
- Gumroad visibility in README/support/funding metadata/canonical guide;
- canonical Gumroad URL `https://ramsandesh.gumroad.com`;
- no Gumroad/Buy Me a Coffee About runtime surface;
- no medical/health entitlement wording;
- repository Gumroad SVG existence and accessible `<title>`/`<desc>`;
- no Gumroad badge under app resources.

### `StoreFundingPayloadContractTests.cs`

Protects:

- no external-commerce destinations in text-like `src/CareNest.App` files;
- no runtime shared URL constants;
- no obsolete external-commerce build switches;
- default scanner markers for Buy Me a Coffee and Gumroad;
- UTF-8/UTF-16 marker scanning;
- ZIP/AAB inspection behavior;
- fail-closed scanner handling.

The repository can promote Gumroad strongly while the health-app package remains external-commerce-free under the current policy.

## 9. Running tests locally

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

Use a clean checkout when validating release behavior.

## 10. Formatting gate

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

## 11. Reminder planner test strategy

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

## 12. Reminder coordinator/reconciliation tests

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

## 13. Appointment tests

Assertions include:

- `StartsUtc` requires true UTC;
- local/unspecified timestamps are rejected rather than relabeled;
- explicit reminder lead time;
- denied notification permission does not become successful scheduling;
- background rebuild does not repeatedly prompt;
- persistence/platform scheduling compensation.

## 14. Document-vault tests

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

## 15. Authenticated stream tests

Chunked AEAD testing includes:

- multi-chunk round trip;
- authenticated terminal record;
- truncation rejection;
- trailing-data rejection;
- legacy v1 read compatibility;
- key length enforcement;
- buffer clearing where managed-memory control permits.

## 16. Backup/restore tests

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

## 17. SQLite/persistence tests

Coverage includes:

- migration idempotency/order;
- relationship cleanup;
- WAL/busy-timeout behavior;
- transactional multi-step operations;
- snapshot/integrity behavior;
- package dependency policy at source level.

A green integration suite does not replace a realistic packaged upgrade with representative prior data.

## 18. App-lock tests

Source/behavior contracts cover:

- no plaintext PIN persistence;
- salted PBKDF2-HMAC-SHA256 verifier flow;
- fixed-time comparison;
- material validation;
- secure-store ownership;
- disable/update rollback;
- verifier-buffer clearing where possible;
- fail-closed corrupt/missing material.

## 19. Report/export tests

Coverage includes:

- required disclaimers;
- CSV formula-like input neutralization;
- staged/atomic output behavior;
- cleanup of app-owned temporary report files;
- JSON/PDF/CSV integration behavior.

## 20. Architecture/source-policy tests

Repository policies protect against:

- MAUI dependency leaking into platform-neutral layers;
- direct SQL from ViewModels;
- casual network-client creation in local-first runtime;
- `async void`/`Task.Run` misuse in ViewModels where prohibited;
- common signing/private-key artifacts;
- sensitive logging regressions;
- incomplete release workflow/preflight behavior;
- reintroduction of external-commerce application surfaces;
- unresolved runtime source placeholders/merge defects;
- malformed structured runtime files.

## 21. Dependency security tests

SQLite dependency-security contracts protect maintained package/native/provider floors and removal of the former exact audit suppression.

Dependency Audit remains separate and blocking.

## 22. GitHub Actions matrix

Current exact-source verification can include:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts on their configured triggers.

Production-style tags additionally use Release Gate and Release Evidence as configured.

## 23. Store inspection tests/evidence

The inspection workflow self-tests the forbidden-marker scanner and generates internal Android/Windows/Apple artifacts with checksums/provenance.

Default forbidden repository-only markers include:

- `buymeacoffee.com/sanskarIN`;
- `ramsandesh.gumroad.com`.

Inspection artifacts are engineering evidence, not production-signed/store-approved packages.

## 24. Manual Android matrix

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

## 25. Manual Windows matrix

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

## 26. Manual Apple matrix

Simulator compilation does not replace real-device/manual evidence.

Still requires appropriate iPhone/iPad/Mac Catalyst testing for permission/delivery, lifecycle, time zone, backup, documents, app lock, Dynamic Type/VoiceOver/keyboard/focus/themes and signed/notarized behavior.

## 27. Accessibility testing

Automated XAML semantics are necessary but insufficient. Manual validation should cover representative screen readers, large text/scaling, keyboard focus, contrast/theme, reduced motion and color-independent meaning.

Repository promotional assets also require meaningful text alternatives and plain-text link fallbacks.

## 28. Packaged compatibility testing

Before production release test representative fictional prior data through realistic package/update paths:

- SQLite opens/integrity/readability/editability;
- schema version/migrations;
- reminder reconciliation;
- encrypted document access/export;
- backup create/inspect/restore;
- wrong-password/tamper rejection;
- genuine historical fixtures where actual prior bytes exist.

Do not manufacture a new artifact and label it historical evidence.

## 29. Test data policy

Use fictional/synthetic data only in automated tests, screenshots, public issues and packaged migration fixtures.

Never commit real health data, real backups, PINs/passwords, keys, tokens or signing material.

Do not place private health information in external storefront/payment notes or test examples.

## 30. Exact-source evidence rule

A test count is meaningful only with the exact source it verified.

Latest fully verified pre-Gumroad source:

`7cbe5568b6cffa06c279b29f3cb1b107ea988791`

Older PR #74/#68/etc. counts remain historical evidence for their named source boundaries.

The Gumroad rollout changes source-policy tests and package scanning, so it requires a new exact-source result before its new count is promoted.

## 31. Release interpretation

CareNest remains `1.0.0-rc.1`.

The latest fully verified pre-Gumroad automated matrix is green, but manual/device/package/accessibility/signing/store/tag/publication evidence remains open. The current rollout must earn its own exact-source green result before replacing the prior baseline.

Use:

- `PROJECT_STATUS.md`;
- `what_changed.md`;
- `docs/releases/NEXT_STEPS.md`;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`.
