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

## Historical changelog continuity

The full exact changelog that was active before this continuation—including the complete 2026-08-15 packaged-funding root-cause investigation, package-policy changes, PR #67/PR #68 verification, SQLite security state, documentation promotion, and production status—is preserved unchanged at:

`docs/history/pre-xaml-compiled-bindings-20260816/CHANGELOG.md`

Use that exact file for the complete earlier entry rather than reconstructing or shortening its historical wording.
