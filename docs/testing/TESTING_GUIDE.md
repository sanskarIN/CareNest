# CareNest Testing Guide

**Release line:** `1.0.0-rc.1`  
**Current automated baseline record:** `docs/releases/AUTOMATED_BASELINE.md`  
**Exact-head procedure:** `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`  
**Documentation integrity:** `docs/testing/DOCUMENTATION_INTEGRITY.md`

CareNest uses layered automated testing plus mandatory manual release validation. Automated success is necessary but does not prove real-device notification delivery, accessibility, signing, store approval, live store declarations or packaged existing-data compatibility.

## 1. Automated evidence authority

Use:

`docs/releases/AUTOMATED_BASELINE.md`

for the latest actually observed exact-source workflow IDs and test counts.

Do not hard-code an older total as the expected result of a newer source. Permanent historical evidence remains useful for its own exact SHA, including `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md` and `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

The current candidate includes verification-relevant source/test/dependency/workflow/tooling changes and therefore must complete a fresh exact-head matrix before it replaces the recorded baseline.

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

Use the actual exact-source run for the current count rather than copying a historical value.

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

Use the actual exact-source run for the current count.

## 4. UI/source-policy tests

Project:

`tests/CareNest.UiTests`

This suite is primarily repository/source/XAML/tooling policy testing rather than full device UI automation.

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
- store-payload Gumroad/Buy Me a Coffee marker enforcement;
- package-evidence tooling/source/workflow contracts;
- stable release-documentation governance contracts;
- documentation-integrity tooling/source/workflow contracts.

Do not predict the current count from source inspection; record the count actually reported by exact-source CI.

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

`SourceLineQualityContractTests.cs` deterministically scans runtime C# files under `src/` and reports repository-relative file/line failures.

The broad line audit rejects known defect patterns including:

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

## 9. Package-evidence tooling tests

`PackageEvidenceToolContractTests.cs` protects:

- package-evidence implementation/wrappers/self-test/guide existence;
- production tag/source/checked-out-HEAD/clean-workspace requirements;
- real non-secret signing/notarization provenance requirement;
- store-safe scanner integration;
- payload/per-file SHA-256 contracts;
- evidence-output placement;
- synthetic success and fail-closed self-test paths;
- CareNest CI package-tool wiring;
- package-evidence documentation boundaries.

Synthetic behavior self-test:

```bash
python3 build/scripts/test-create-package-evidence.py
```

The self-test uses only temporary synthetic data and does not replace final production package evidence.

## 10. Documentation-integrity tests

`DocumentationIntegrityToolContractTests.cs` protects:

- documentation checker/self-test/guide existence;
- fail-closed missing-link behavior;
- repository-escaping path rejection;
- stable/default dynamic evidence exclusion;
- history exclusion;
- explicit `--include-dynamic` and `--include-history` audit behavior;
- CI, Release Gate and Release Evidence integration.

Run:

```bash
python3 build/scripts/test-verify-documentation-links.py
python3 build/scripts/verify-documentation-links.py
```

The default checker is intentionally offline and verifies stable active local links without making post-verification dynamic evidence values into an executable exact-source loop.

See `DOCUMENTATION_INTEGRITY.md`.

## 11. Release-governance tests

`ReleaseDocumentationConsistencyContractTests.cs` protects stable release policy from:

- re-promotion of superseded intermediate verification language;
- loss of both repository-only external-commerce package markers;
- disappearance/misrepresentation of the dated store-policy review;
- premature completion of live store declarations;
- loss of package-evidence tooling;
- loss of documentation-integrity tooling;
- weakening of the production Release Gate required-file/tooling set.

The mutable `docs/releases/AUTOMATED_BASELINE.md` record exists but its changing source SHA/count values are deliberately not hard-coded into executable C# assertions.

## 12. Running tests locally

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

Use a clean checkout when validating release behavior.

## 13. Formatting gate

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

## 14. Reminder planner test strategy

Reminder tests protect a deterministic organizational contract, not a medical recommendation engine.

Covered concepts include daily schedules, multiple explicit times, selected weekdays, cycle schedules, custom date ranges, end dates, every-N-hours, follow-ups, disabled schedules, archived/inactive suppression, as-needed/no automatic materialization, ownership mismatch rejection, invalid state rejection, half-open UTC planning windows, duplicate-time deduplication, chronological output, deterministic time-zone/DST behavior, spring-forward rejection, fall-back ambiguity handling, stable occurrence identity and explicit future UTC snooze validation.

See `REMINDER_SCHEDULING_CONTRACT.md`.

## 15. Reminder coordinator/reconciliation tests

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

## 16. Appointment tests

Assertions include:

- `StartsUtc` requires true UTC;
- local/unspecified timestamps are rejected rather than relabeled;
- explicit reminder lead time;
- denied notification permission does not become successful scheduling;
- background rebuild does not repeatedly prompt;
- persistence/platform scheduling compensation.

## 17. Document-vault tests

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

## 18. Authenticated stream tests

Chunked AEAD testing includes:

- multi-chunk round trip;
- authenticated terminal record;
- truncation rejection;
- trailing-data rejection;
- legacy v1 read compatibility;
- key length enforcement;
- buffer clearing where managed-memory control permits.

## 19. Backup/restore tests

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

## 20. SQLite/persistence tests

Coverage includes:

- migration idempotency/order;
- relationship cleanup;
- WAL/busy-timeout behavior;
- transactional multi-step operations;
- snapshot/integrity behavior;
- package dependency policy at source level.

A green integration suite does not replace a realistic packaged upgrade with representative prior data.

## 21. App-lock tests

Source/behavior contracts cover:

- no plaintext PIN persistence;
- salted PBKDF2-HMAC-SHA256 verifier flow;
- fixed-time comparison;
- material validation;
- secure-store ownership;
- disable/update rollback;
- verifier-buffer clearing where possible;
- fail-closed corrupt/missing material.

## 22. Report/export tests

Coverage includes required disclaimers, CSV formula-like input neutralization, staged/atomic output behavior, cleanup of app-owned temporary report files and JSON/PDF/CSV integration behavior.

## 23. Architecture/source-policy tests

Repository policies protect against:

- MAUI dependency leaking into platform-neutral layers;
- direct SQL from ViewModels;
- casual network-client creation in local-first runtime;
- prohibited async blocking/misuse patterns;
- common signing/private-key artifacts;
- sensitive logging regressions;
- incomplete release workflow/preflight behavior;
- reintroduction of external-commerce application surfaces;
- unresolved runtime source placeholders/merge defects;
- malformed structured runtime files;
- release/documentation/package tooling regression.

## 24. File/camera gateway behavior

`MauiFileGateway` honors its cancellation-token contract at application-controlled boundaries:

- before picker/camera/share entry;
- after picker/camera completion;
- before opening a selected file stream;
- after a file stream opens, disposing that stream before throwing if cancellation arrived during the platform open.

This does not claim the operating system's picker/camera UI can always be force-cancelled by a managed token. Final platform smoke tests still cover real file/camera behavior.

## 25. Dependency security tests

SQLite dependency-security contracts protect maintained package/native/provider floors and removal of the former exact audit suppression.

Dependency Audit remains separate and blocking.

Current candidate package versions are documented in `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`; the exact combined candidate still requires current verification.

## 26. GitHub Actions matrix

Current exact-source PR verification can include:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts.

Production-style tags additionally use Release Gate and Release Evidence as configured.

Current maintained action majors are documented in `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`.

## 27. Store inspection tests/evidence

The inspection workflow self-tests the forbidden-marker scanner and generates internal Android/Windows/Apple artifacts with checksums/provenance.

Default forbidden repository-only markers include:

- `buymeacoffee.com/sanskarIN`;
- `ramsandesh.gumroad.com`.

Inspection artifacts are engineering evidence, not production-signed/store-approved packages.

## 28. Manual Android matrix

Production validation still requires representative testing for notification permission, actual delivery, exact/inexact alarm/battery restrictions, reboot/restart/time-zone recovery, force-stop/vendor behavior, reminder lifecycle/actions/snooze, file/camera/share, backup/restore, app lock and accessibility.

## 29. Manual Windows matrix

Still requires installed package behavior, running/closed-app reminder behavior, timer replacement/cancellation, reminder actions/snooze, restart/recovery, file/share, backup/restore, app lock, keyboard/focus and themes/accessibility.

## 30. Manual Apple matrix

Simulator compilation does not replace real-device/manual evidence.

Still requires appropriate iPhone/iPad/Mac Catalyst testing for permission/delivery, lifecycle, time zone, reminder actions, file/camera/share where supported, backup, app lock, Dynamic Type/VoiceOver/keyboard/focus/themes and signed/notarized behavior.

## 31. Accessibility testing

Automated XAML semantics are necessary but insufficient. Manual validation should cover representative screen readers, large text/scaling, keyboard focus, contrast/theme, reduced motion and color-independent meaning.

Repository promotional assets also require meaningful text alternatives and plain-text link fallbacks.

## 32. Packaged compatibility testing

Before production release test representative fictional prior data through realistic package/update paths:

- SQLite opens/integrity/readability/editability;
- schema version/migrations;
- reminder reconciliation;
- encrypted document access/export;
- backup create/inspect/restore;
- wrong-password/tamper/truncation/trailing-data rejection;
- genuine historical fixtures where actual prior bytes exist.

Do not manufacture a new artifact and label it historical evidence.

## 33. Test data policy

Use fictional/synthetic data only in automated tests, screenshots, public issues and packaged migration fixtures.

Never commit real health data, real backups, PINs/passwords, keys, tokens or signing material.

Do not place private health information in external storefront/payment notes or test examples.

## 34. Exact-source evidence rule

A test count is meaningful only with the exact source it verified.

Use `docs/releases/AUTOMATED_BASELINE.md` for the current promoted automated result and `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` for a new candidate.

If verification-relevant source moves, the running/older checkpoint is stale for that newer source. Fix genuine failures, freeze the corrected source and rerun the complete applicable matrix.

Dynamic post-verification evidence/status files can record successful run IDs/counts without becoming mutable-value C# assertion inputs. Stable documentation integrity remains part of exact-source CI.

## 35. Release interpretation

CareNest remains `1.0.0-rc.1` until required production evidence is complete.

A green exact-source automated matrix still does not finish manual/device/package/accessibility/signing/store/tag/publication work.

Use:

- `PROJECT_STATUS.md`;
- `what_changed.md`;
- `docs/releases/AUTOMATED_BASELINE.md`;
- `docs/releases/NEXT_STEPS.md`;
- `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`;
- `docs/testing/DOCUMENTATION_INTEGRITY.md`;
- `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`.
