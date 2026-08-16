# CareNest XAML Compiled-Binding Verification — 2026-08-16

## Release context

- Release candidate: `1.0.0-rc.1`
- Repository: `sanskarIN/CareNest`
- Source hardening PR: #74 — `Compile and enforce MAUI XAML bindings`
- Frozen implementation/test head: `8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`
- Merged `main` source SHA: `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`
- Base before this continuation: `7f7bcab25d92e11783d396edc82d2981eeadadb1`
- Implementation delta: 17 commits, 17 files
- Intended behavior change: none. This continuation is a compile-time binding correctness, maintainability, and performance hardening pass.

PR #74 was merged only after the full frozen-head GitHub Actions matrix completed successfully.

---

## Problem closed by this continuation

The previous RC1 baseline was functionally green but still emitted MAUI/XamlC compiled-binding optimization warnings such as `XC0022` and `XC0025`.

Those warnings meant that some XAML bindings still fell back to reflection-based runtime binding because their binding context or explicit `Source` was not fully typed for XamlC.

This continuation removes that remaining source-side warning debt by making the binding contracts explicit and then making regression of those warnings a build failure.

No warning suppression was introduced.

---

## Binding-bearing views migrated

All binding-bearing XAML views under `src/CareNest.App/Views` were inspected and migrated:

1. `AboutPage.xaml`
2. `AppointmentEditorPage.xaml`
3. `AppointmentsPage.xaml`
4. `DashboardPage.xaml`
5. `DocumentsPage.xaml`
6. `LockPage.xaml`
7. `MedicationLogPage.xaml`
8. `MedicineEditorPage.xaml`
9. `MedicinesPage.xaml`
10. `OnboardingPage.xaml`
11. `ProfileEditorPage.xaml`
12. `ProfilesPage.xaml`
13. `ReportsPage.xaml`
14. `ScheduleEditorPage.xaml`
15. `SettingsPage.xaml`

The following inspected XAML files do not contain runtime `{Binding ...}` expressions and therefore do not require a ViewModel `x:DataType` migration:

- `AppShell.xaml`
- `StartupPage.xaml`
- `App.xaml` contains application resources rather than a runtime page binding context.

---

## Root binding contexts

Each binding-bearing page now declares an accurate root `x:DataType` for its actual ViewModel.

This moves ordinary page bindings from reflection-oriented runtime lookup toward XamlC compile-time checking and prevents misspelled/missing ViewModel members from remaining silent until runtime.

No page uses `x:Object` or `x:Null` to escape type checking.

---

## DataTemplate item contexts

Every binding-bearing `DataTemplate` now declares its own row/item `x:DataType` instead of inheriting the parent page ViewModel type.

Typed template contexts include the relevant row/domain/application-contract types, including:

- `AppointmentRow`
- `MedicineRow`
- `DocumentRow`
- `MedicationLogRow`
- `ProfileCareSummary`
- `RedactedScheduleItem`
- `PersonProfile`
- `EmergencyContact`
- `ReminderPreview`

This closes the `XC0024` class of inherited-template type ambiguity as well as ordinary untyped template lookup.

---

## Picker display-binding contexts

Picker `ItemDisplayBinding` expressions that switch from a page ViewModel to a collection item now declare their item type explicitly.

Examples include:

- `PersonProfile.Name`
- `Medicine.Name`

This prevents the picker display expression from being compiled against the wrong inherited context.

---

## Explicit Source / RelativeSource command bindings

List/template buttons that invoke commands owned by a parent ViewModel now use a typed ancestor binding context.

The migration uses the ViewModel as `AncestorType`; MAUI therefore resolves the binding against the matching ancestor binding context. The binding itself also declares its source `x:DataType`.

This pattern is used for parent commands in profile, appointment, medicine, document, and medication-log template surfaces.

The result is that explicit `Source` command bindings are eligible for source-binding compilation instead of remaining reflection-based.

---

## App project enforcement

`src/CareNest.App/CareNest.App.csproj` now enables:

```xml
<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
<WarningsAsErrors>$(WarningsAsErrors);XC0022;XC0023;XC0024;XC0025</WarningsAsErrors>
```

The policy is intentionally fail-closed:

- `XC0022` — binding could be compiled with an appropriate `x:DataType`;
- `XC0023` — binding could be compiled if `x:DataType` is not explicitly null;
- `XC0024` — binding may be compiled against an incorrect inherited `x:DataType`, commonly in a template;
- `XC0025` — binding was not compiled because it uses an explicit `Source` and source-binding compilation/type information is missing.

No matching `NoWarn` suppression was added.

---

## Permanent regression tests

Added:

`tests/CareNest.UiTests/CompiledBindingContractTests.cs`

The suite dynamically scans binding-bearing XAML views and enforces all of the following:

1. every binding-bearing view has a real root `x:DataType`;
2. every binding-bearing `DataTemplate` has its own item `x:DataType`;
3. every explicit `Source` binding declares a binding-specific source `x:DataType`;
4. every picker `ItemDisplayBinding` declares the item `x:DataType`;
5. the app project keeps source-binding compilation and strict XAML compilation enabled;
6. `XC0022`, `XC0023`, `XC0024`, and `XC0025` remain warnings-as-errors;
7. matching warnings are not hidden through `NoWarn`;
8. binding type safety is not disabled with `x:Object` or `x:Null`.

Because the tests enumerate the XAML directory dynamically, a future page/template using runtime bindings is covered by policy without requiring a manually maintained page-name allowlist.

---

## Focused source commits

The implementation was intentionally split into focused commits so any platform/compiler regression remains easy to isolate:

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

All commits were authored as `Sanskar <sanskarin@outlook.in>`.

---

## Verified automated results on the frozen source head

### Core tests

CareNest CI #735 / run `31938301209` completed successfully:

- formatting verification: success;
- unit tests: **122 passed, 0 failed, 0 skipped**;
- integration tests: **39 passed, 0 failed, 0 skipped**;
- UI/source-policy tests: **170 passed, 0 failed, 0 skipped**;
- total core tests: **331 passed, 0 failed, 0 skipped**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success.

The UI/source-policy count increased from 164 to 170 because this continuation adds six compiled-binding contract tests.

These platform builds execute with `XC0022`–`XC0025` promoted to errors, so success is direct evidence that the migrated XAML passes strict compiled-binding enforcement on every configured target family.

### Store-candidate configurations

CareNest Store Package Configuration #124 / run `31938301146` completed successfully on all configured targets:

- Android store-candidate configuration: success;
- Windows store-candidate configuration: success;
- iOS simulator store-candidate configuration: success;
- Mac Catalyst store-candidate configuration: success.

### Store inspection artifacts

CareNest Store Inspection Artifacts #47 / run `31938301275` completed successfully:

- store-safe payload scanner self-test: success;
- unsigned Android AAB inspection publish/staging/provenance/upload: success;
- Windows self-contained inspection publish/staging/provenance/upload: success;
- iOS simulator inspection app build: success;
- unsigned Mac Catalyst inspection app publish: success;
- Apple inspection staging/checksums/provenance: success;
- Apple inspection artifact upload: success.

The package-inspection protections therefore remain green after the compiled-binding migration.

### Security and dependency gates

- CodeQL #735 / run `31938301252`: **success**;
- Dependency Audit #91 / run `31938301172`: **success**;
- platform-neutral dependency graph audit: success;
- MAUI app dependency graph audit: success.

---

## Merge result

PR #74 was merged with a merge commit so the 17 focused implementation/test commits remain individually visible in repository history.

Merged `main` source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

PR source head verified by the complete matrix:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

The merge introduces no additional executable delta beyond the verified PR head.

---

## Product behavior boundary

This continuation intentionally does not alter:

- health-data semantics;
- medicine strength/instruction storage semantics;
- dosage policy;
- reminder schedule semantics;
- reminder reconciliation semantics;
- appointment behavior;
- encrypted document format;
- backup format;
- SQLite schema or transaction semantics;
- app-lock behavior;
- package identities;
- funding/package policy;
- privacy/medical limitation wording;
- network/account/cloud scope.

The change is confined to XAML binding metadata, XamlC project enforcement, and source-policy tests.

---

## Release interpretation

The previous `XC0022` / `XC0025` compiled-binding cleanup item is now closed. The project also fails future configured builds for `XC0022`, `XC0023`, `XC0024`, and `XC0025` rather than silently accepting regression.

Closing compiled-binding warnings improves compile-time correctness checking and binding execution efficiency, but it does not replace real-device, accessibility, package-upgrade, encryption-compatibility, signing, or store validation.

CareNest therefore remains `1.0.0-rc.1`.

The remaining production blockers stay external/manual unless those tests expose a real source defect.

Active status/handoff files point to this evidence record and the merged source boundary. Exact pre-binding versions remain under `docs/history/pre-xaml-compiled-bindings-20260816/`.
