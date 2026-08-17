# Changelog

All notable CareNest changes are recorded here using Keep a Changelog-style categories and semantic-versioning intent.

The exact active changelog immediately before the 2026-08-16 compiled-binding continuation is preserved byte-for-byte at:

`docs/history/pre-xaml-compiled-bindings-20260816/CHANGELOG.md`

The earlier exact 2026-08-15 pre-final changelog remains at:

`docs/history/pre-final-bug-audit-20260815/CHANGELOG.md`

Earlier snapshots remain under `docs/history/` and Git history.

## [Unreleased] - 2026-08-16

### Changed — compile all binding-bearing MAUI XAML surfaces

- Added accurate root `x:DataType` declarations to all 15 binding-bearing pages under `src/CareNest.App/Views`.
- Added item-level `x:DataType` declarations to every binding-bearing DataTemplate.
- Added binding-specific item types to picker `ItemDisplayBinding` expressions where the binding context switches from a page ViewModel to a domain item.
- Reworked template-to-parent command bindings to use typed ViewModel ancestor binding contexts.
- Added binding-specific source type information to explicit Source/RelativeSource command bindings.
- Left non-binding `AppShell.xaml`, `StartupPage.xaml`, and application resource `App.xaml` unchanged after inspection because they do not require a runtime binding-context type migration.

### Changed — strict XamlC build policy

- Enabled `MauiEnableXamlCBindingWithSourceCompilation`.
- Enabled `MauiStrictXamlCompilation`.
- Promoted `XC0022`, `XC0023`, `XC0024`, and `XC0025` to errors.
- Did not add matching `NoWarn` suppressions.
- Did not use `x:Object` or `x:Null` to escape binding type checking.
- Closed the previously documented non-blocking `XC0022` / `XC0025` compiled-binding optimization-warning cleanup item.

### Added — compiled-binding regression contracts

Added `tests/CareNest.UiTests/CompiledBindingContractTests.cs` with dynamic repository-policy coverage that requires:

- a real root `x:DataType` on every binding-bearing view;
- an item `x:DataType` on every binding-bearing DataTemplate;
- a binding-specific source type on explicit Source bindings;
- an item type on picker `ItemDisplayBinding`;
- strict/source XAML compilation project switches;
- `XC0022`–`XC0025` warnings-as-errors;
- no matching `NoWarn` suppression;
- no `x:Object` / `x:Null` compiled-binding type-safety bypass.

The UI/source-policy suite increased from 164 tests to 170 tests.

### Source commit structure

PR #74 intentionally preserved 17 focused implementation/test commits:

1. `perf: compile about page bindings`
2. `perf: compile lock page bindings`
3. `perf: compile onboarding page bindings`
4. `perf: compile appointment editor bindings`
5. `perf: compile medicine editor bindings`
6. `perf: compile report page bindings`
7. `perf: compile schedule editor bindings`
8. `perf: compile profile editor bindings`
9. `perf: compile profile list bindings`
10. `perf: compile appointment list bindings`
11. `perf: compile medicine list bindings`
12. `perf: compile document organizer bindings`
13. `perf: compile medication log bindings`
14. `perf: compile dashboard bindings`
15. `perf: compile settings page bindings`
16. `build: enforce compiled XAML bindings`
17. `test: protect compiled XAML binding contracts`

Focused source commits were authored as `Sanskar <sanskarin@outlook.in>`.

### Verification — PR #74 frozen source head

Frozen PR source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

CareNest CI #735 / run `31938301209`:

- result: success;
- formatting: success;
- unit tests: **122/122**;
- integration tests: **39/39**;
- UI/source-policy tests: **170/170**;
- total core tests: **331/331**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success.

Store Package Configuration #124 / run `31938301146`:

- result: success;
- Android store-candidate configuration: success;
- Windows store-candidate configuration: success;
- iOS simulator store-candidate configuration: success;
- Mac Catalyst store-candidate configuration: success.

Store Inspection Artifacts #47 / run `31938301275`:

- result: success;
- payload scanner self-test: success;
- Android unsigned AAB inspection artifact: success;
- Windows self-contained inspection artifact: success;
- iOS simulator inspection build: success;
- unsigned Mac Catalyst inspection publish: success;
- Android/Windows/Apple staging, checksums/provenance and artifact upload: success.

Security/dependency:

- CodeQL #735 / run `31938301252`: success;
- Dependency Audit #91 / run `31938301172`: success on platform-neutral and MAUI graphs;
- former SQLite advisory suppression remains removed.

### Merged source

PR #74 was merged with a merge commit, preserving the focused commits.

Merged `main` source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Permanent verification record:

`docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`

### Behavior boundary

No health-organizer feature semantics were intentionally changed by this continuation.

The pass does not change:

- medicine strength/instruction semantics;
- dosage policy;
- reminder scheduling/reconciliation/action semantics;
- appointments;
- encrypted document or backup formats;
- database schema or transaction policy;
- reports/exports;
- app-lock behavior;
- package identities;
- external-funding package policy;
- account/network/cloud scope;
- medical limitations or privacy wording.

The executable delta is XAML binding metadata, XamlC project enforcement, and tests.

### Documentation

- Preserved the previous active `what_changed.md`, `PROJECT_STATUS.md`, `docs/releases/NEXT_STEPS.md`, `CHANGELOG.md`, and `docs/README.md` under `docs/history/pre-xaml-compiled-bindings-20260816/`.
- Added `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.
- Promoted active `what_changed.md` to the PR #74 merged source boundary.
- Promoted active `PROJECT_STATUS.md` to the PR #74 automated baseline.
- Removed compiled-binding warning reduction from the remaining-work list because it is complete and enforced.
- Kept real-device, packaged-upgrade/encryption compatibility, accessibility, signing, store and publication work open.

### Production status

CareNest remains `1.0.0-rc.1`.

The source-controlled RC1 feature scope and compiled-binding cleanup are complete under the configured automated matrix, but public production release still requires real-device/accessibility/package-upgrade/encrypted-compatibility/signing/store-policy/metadata/final-tag/release-evidence work.

Do not describe CareNest as production-published, store-approved, production-signed, or globally bug-free until those external gates are actually completed.

---

## Complete previous active changelog entry — preserved verbatim

> **Historical snapshot notice:** Everything below this notice is the complete previous active `CHANGELOG.md` preserved verbatim. It describes the 2026-08-15 source boundary and remains historical evidence; the 2026-08-16 entry above is the current continuation.

# Changelog

All notable CareNest changes are recorded here using Keep a Changelog-style categories and semantic-versioning intent.

The exact pre-final 2026-08-15 changelog is preserved at:

`docs/history/pre-final-bug-audit-20260815/CHANGELOG.md`

Earlier snapshots remain under `docs/history/` and Git history.

## [Unreleased] - 2026-08-15

### Fixed — packaged external funding marker root cause

- Fixed the final known automated package defect in which Windows store-safe publishing still contained `buymeacoffee.com/sanskarIN` inside `CareNest.App.dll` despite earlier funding controls evaluating false.
- Identified the actual cause as `src/CareNest.App/Resources/Images/buy_me_a_coffee_carenest.svg`, whose SVG accessibility/text content contained the full external funding destination and was embedded into the Windows MAUI managed payload.
- Removed the URL-bearing funding artwork from the app package.
- Removed the obsolete packaged support artwork.
- Removed the in-app project-funding command and About-page funding card.
- Removed funding-policy source units and obsolete app funding build switches.
- Kept voluntary project funding in repository documentation only, with explicit no-medical/no-health-entitlement language.
- Removed broken repository documentation image references after the app funding artwork was deleted.

### Changed — final funding-free app package policy

- Store/package policy now treats the absence of the external funding destination as a source-level product boundary rather than a per-store build toggle.
- `CareNestShowFundingLink` and related store funding environment/effective-property machinery are no longer part of the final app build configuration.
- Store Package Configuration continues to build Android, Windows, iOS simulator and Mac Catalyst store-candidate configurations without a funding-property fork.
- Store Inspection Artifacts continues to scan the actual Android/Windows/Apple payloads for the forbidden funding marker before artifact upload.
- Inspection provenance now records `external_funding_surface=absent_by_source_policy` together with `funding_url_payload_scan=passed`.
- Bash/PowerShell release preflight no longer forwards an obsolete funding property.
- Bash/PowerShell store-package preflight now requires an explicit supported target and delegates to the normal release preflight.

### Added — package payload regression protection

- Added/retained byte-level funding-marker inspection for UTF-8, UTF-16 LE and UTF-16 BE payload content.
- Added/retained ZIP/AAB entry scanning.
- Added/retained fail-closed handling for missing/unreadable payload paths.
- Added/retained scanner self-tests proving clean pass plus UTF-8, UTF-16, nested archive and missing-path failure behavior.
- Added recursive source-policy coverage preventing the external funding destination or removed funding surfaces from re-entering `src/CareNest.App`.
- Added funding-free About/ViewModel/branding/package/workflow/preflight regression contracts.
- Added `docs/releases/FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md` as the final exact automated evidence record.

### Verification — PR #67 pre-merge

Final PR #67 source head:

`0fa552ca824f034ce7426513a7d3e50eaa0ef7aa`

Results:

- CareNest CI #717 / run `31880445293`: success;
- formatting: success;
- unit tests: 122 passed, 0 failed, 0 skipped;
- integration tests: 39 passed, 0 failed, 0 skipped;
- UI/source-policy tests: 164 passed, 0 failed, 0 skipped;
- total core tests: **325/325**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- Store Package Configuration #106 / run `31880445239`: all four target configurations success;
- Store Inspection Artifacts #40 / run `31880445403`: scanner self-test plus Android/Windows/Apple payload scans success;
- CodeQL #717 / run `31880445284`: success;
- unsuppressed Dependency Audit #84 / run `31880445286`: success.

PR #67 was merged to `main` as:

`9ec7b4e7d2150d9cc50be19f30464080318b16e8`

### Verification — PR #68 authoritative exact merged-source baseline

Frozen executable source/base:

`9ec7b4e7d2150d9cc50be19f30464080318b16e8`

Marker SHA:

`c752815c311e7e443f1d71df8a9197cf706a14b6`

PR #68 changed one marker file only, 14 additions and 0 deletions, and was closed without merge.

Results:

- CareNest CI #719 / run `31880955724`: success;
- formatting: success;
- unit tests: **122/122**;
- integration tests: **39/39**;
- UI/source-policy tests: **164/164**;
- total core tests: **325/325**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- Store Package Configuration #108 / run `31880955723`: Android/Windows/iOS simulator/Mac Catalyst success;
- Store Inspection Artifacts #41 / run `31880955734`: scanner self-test plus Android/Windows/Apple inspection artifacts and payload scans success;
- CodeQL #719 / run `31880955720`: success;
- unsuppressed Dependency Audit #85 / run `31880955731`: success.

The exact merged-source Windows payload scan is the decisive regression proof for the defect found by earlier PR #65/#66 checkpoints.

### Final automated bug/error sweep

After merge and exact verification:

- no open GitHub issues were found;
- no indexed `TODO`, `FIXME`, `HACK` or `NotImplementedException` placeholders were found;
- no indexed `DateTime.Now` or `GetAwaiter` patterns were found;
- existing ViewModel contracts continue to reject `async void`, `Task.Run`, direct SQLite infrastructure access and direct network-client creation.

This supports the statement that no known automated defect remains at executable source `9ec7b4e7...` under the configured test/build/security/package-inspection matrix. It is not a claim that all possible bugs are impossible.

### Security — SQLite dependency remediation remains closed

- The former `GHSA-2m69-gcr7-jv3q` repository dependency exception remains remediated.
- Maintained SQLite native/provider leaves remain centrally pinned.
- The former exact NuGet audit suppression remains removed.
- PR #68 unsuppressed Dependency Audit #85 succeeded on platform-neutral and MAUI graphs.
- Packaged existing-database/encrypted-data compatibility remains a separate manual release gate.

### Documentation — final active handoff/status

- Added final automated evidence record.
- Updated store build policy to the funding-free app-package design.
- Updated packaged release validation for the final source boundary.
- Preserved exact pre-final `PROJECT_STATUS.md`, `NEXT_STEPS.md`, `what_changed.md` and `CHANGELOG.md` snapshots under `docs/history/pre-final-bug-audit-20260815/`.
- Promoted active `PROJECT_STATUS.md` to PR #68/current blockers.
- Promoted `docs/releases/NEXT_STEPS.md` to only real remaining production-validation work.
- Promoted `docs/README.md` to the final PR #68 evidence baseline.
- Rebuilt active `what_changed.md` as the final completion handoff while retaining the exact earlier file in history.

### Production status

CareNest remains `1.0.0-rc.1`.

The source-controlled RC1 feature boundary is complete and heavily automated-verified, but public production release still requires real-device/accessibility/package-upgrade/encrypted-compatibility/signing/store-policy/metadata/final-tag/release-evidence work.

Do not describe CareNest as production-published, store-approved, production-signed, or globally bug-free until those external gates are actually completed.
