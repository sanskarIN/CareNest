# CareNest — Active Completion Handoff

**Date:** 2026-08-16  
**Release candidate:** `1.0.0-rc.1`  
**Repository:** `sanskarIN/CareNest`

This is the active continuation handoff after the 2026-08-16 MAUI XAML compiled-binding hardening pass, full PR-head verification, and merge of PR #74.

The exact previous active handoff and matching active status documents are preserved byte-for-byte at:

- `docs/history/pre-xaml-compiled-bindings-20260816/what_changed.md`
- `docs/history/pre-xaml-compiled-bindings-20260816/PROJECT_STATUS.md`
- `docs/history/pre-xaml-compiled-bindings-20260816/NEXT_STEPS.md`
- `docs/history/pre-xaml-compiled-bindings-20260816/CHANGELOG.md`
- `docs/history/pre-xaml-compiled-bindings-20260816/docs_README.md`

Earlier 2026-08-15 final-bug-audit snapshots remain under:

`docs/history/pre-final-bug-audit-20260815/`

No earlier handoff/status detail was discarded from the repository.

---

# 1. Current authoritative source boundary

The current merged executable/project/test source after this continuation is:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

This is the `main` merge commit for PR #74:

`https://github.com/sanskarIN/CareNest/pull/74`

PR title:

`Compile and enforce MAUI XAML bindings`

The exact PR implementation/test head verified by the complete automated matrix before merge was:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Base before the continuation:

`7f7bcab25d92e11783d396edc82d2981eeadadb1`

PR #74 changed exactly 17 files through 17 focused implementation/test commits, with 250 additions and 27 deletions at the PR boundary.

The merge commit adds no separate executable behavior change beyond the verified PR head.

Permanent verification record:

`docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`

---

# 2. Why this continuation was performed

The previous RC1 automated baseline was functionally green, but MAUI/XamlC still reported compiled-binding optimization/type-information warnings including `XC0022` and `XC0025`.

Those warnings were previously classified correctly as non-blocking maintainability/performance cleanup rather than known runtime defects.

This continuation closed that remaining source-side cleanup item instead of suppressing it.

The final policy is stronger than the previous baseline:

- every binding-bearing page has a real root binding type;
- every binding-bearing DataTemplate has its own item type;
- picker item-display bindings declare their item type where the context changes;
- explicit Source/RelativeSource command bindings declare their source type;
- source-binding compilation is enabled;
- strict XAML compilation is enabled;
- `XC0022`, `XC0023`, `XC0024`, and `XC0025` are promoted to errors;
- repository tests reject future regression or type-safety escape hatches.

No `NoWarn`, `x:Object`, or `x:Null` workaround was introduced.

---

# 3. Binding-bearing views completed

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

The inspected non-binding XAML surfaces do not require a runtime binding-context type migration:

- `AppShell.xaml`
- `StartupPage.xaml`
- `App.xaml` is the application resource root rather than a binding-bearing page.

No binding-bearing view was skipped.

---

# 4. Root ViewModel compiled bindings

Each binding-bearing page now declares the actual root ViewModel via `x:DataType`.

Examples of the completed page/ViewModel contracts include:

- About → `AboutViewModel`;
- appointments list → `AppointmentsViewModel`;
- appointment editor → `AppointmentEditorViewModel`;
- dashboard → `DashboardViewModel`;
- documents → `DocumentsViewModel`;
- lock → `LockViewModel`;
- medication log → `MedicationLogViewModel`;
- medicine editor → `MedicineEditorViewModel`;
- medicines list → `MedicinesViewModel`;
- onboarding → `OnboardingViewModel`;
- profile editor → `ProfileEditorViewModel`;
- profiles list → `ProfilesViewModel`;
- reports → `ReportsViewModel`;
- schedule editor → `ScheduleEditorViewModel`;
- settings → `SettingsViewModel`.

This moves ordinary page property/command lookup into XamlC's compile-time binding checks instead of leaving those expressions as reflection-only runtime lookups.

---

# 5. DataTemplate item typing

Every binding-bearing DataTemplate now declares the type of the item it actually receives.

Relevant typed row/item contexts include:

- `AppointmentRow`;
- `MedicineRow`;
- `DocumentRow`;
- `MedicationLogRow`;
- `ProfileCareSummary`;
- `RedactedScheduleItem`;
- `PersonProfile`;
- `EmergencyContact`;
- `ReminderPreview`.

This prevents DataTemplates from accidentally inheriting a page ViewModel `x:DataType` and closes the `XC0024` class of wrong-context compilation warnings.

---

# 6. Picker item-display typing

Picker `ItemDisplayBinding` expressions that run against collection items instead of the page ViewModel now carry their own binding-specific item type.

Completed examples include:

- profile picker display through `PersonProfile.Name`;
- medicine picker display through `Medicine.Name`.

This keeps the display expression type-correct even though its binding context differs from the root page.

---

# 7. Explicit Source / RelativeSource command compilation

Buttons inside list/template rows that need a command owned by the parent page ViewModel now use typed ancestor binding-context lookup and a binding-specific `x:DataType`.

This is applied to relevant profile, appointment, medicine, document, and medication-log row commands.

The implementation follows MAUI's ViewModel `AncestorType` binding-context pattern instead of suppressing `XC0025`.

---

# 8. Project-level XamlC enforcement

`src/CareNest.App/CareNest.App.csproj` now contains the binding hardening switches:

```xml
<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
<WarningsAsErrors>$(WarningsAsErrors);XC0022;XC0023;XC0024;XC0025</WarningsAsErrors>
```

The intended meaning of this policy is fail-closed:

- `XC0022` must not return because a normal binding lacks usable type information;
- `XC0023` must not return through explicit null binding typing;
- `XC0024` must not return because a template inherited the wrong type;
- `XC0025` must not return because an explicit Source binding remained uncompiled.

The previous compiled-binding-warning cleanup item is therefore not merely reduced; it is enforced against regression by the build.

---

# 9. New permanent source-policy tests

Added:

`tests/CareNest.UiTests/CompiledBindingContractTests.cs`

The new tests dynamically inspect binding-bearing XAML and enforce:

- a real root `x:DataType` on every binding-bearing view;
- item `x:DataType` on every binding-bearing DataTemplate;
- binding-specific source `x:DataType` on explicit Source bindings;
- binding-specific item type on picker `ItemDisplayBinding`;
- `MauiEnableXamlCBindingWithSourceCompilation=true`;
- `MauiStrictXamlCompilation=true`;
- `XC0022`, `XC0023`, `XC0024`, and `XC0025` in warnings-as-errors;
- no matching `NoWarn` suppression;
- no `x:Object` or `x:Null` binding type-safety bypass.

The scan discovers XAML files from the views directory instead of depending on a hard-coded list, so future binding-bearing pages enter the contract automatically.

---

# 10. Focused source commit history

The source work was intentionally split into 17 focused commits:

1. `131342ef...` — `perf: compile about page bindings`
2. `6f4bddf...` — `perf: compile lock page bindings`
3. `062a259...` — `perf: compile onboarding page bindings`
4. `1a641f37...` — `perf: compile appointment editor bindings`
5. `b080865e...` — `perf: compile medicine editor bindings`
6. `8ecd3a39...` — `perf: compile report page bindings`
7. `21b5a401...` — `perf: compile schedule editor bindings`
8. `d8a35e8b...` — `perf: compile profile editor bindings`
9. `ff44188c...` — `perf: compile profile list bindings`
10. `2806b8f1...` — `perf: compile appointment list bindings`
11. `e9f67ebd...` — `perf: compile medicine list bindings`
12. `ca38a9fe...` — `perf: compile document organizer bindings`
13. `89d464db...` — `perf: compile medication log bindings`
14. `a7a91adb...` — `perf: compile dashboard bindings`
15. `bd4640ef...` — `perf: compile settings page bindings`
16. `245b004e...` — `build: enforce compiled XAML bindings`
17. `8908fa9f...` — `test: protect compiled XAML binding contracts`

The focused commits were authored as:

`Sanskar <sanskarin@outlook.in>`

PR #74 was merged using a merge commit so the focused commits remain visible instead of being squashed into one source commit.

---

# 11. Complete frozen-head automated verification

All verification below ran against the frozen PR #74 source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

## CareNest CI #735

Run:

`31938301209`

Result: **success**.

Core quality/test evidence:

- platform-neutral formatting verification: success;
- unit tests: **122 passed, 0 failed, 0 skipped**;
- integration tests: **39 passed, 0 failed, 0 skipped**;
- UI/source-policy tests: **170 passed, 0 failed, 0 skipped**;
- total core tests: **331 passed, 0 failed, 0 skipped**.

The UI/source-policy suite increased from 164 to 170 because six compiled-binding contract tests were added.

Platform builds:

- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success.

These builds were performed with `XC0022`–`XC0025` promoted to errors.

## Store Package Configuration #124

Run:

`31938301146`

Result: **success**.

- Android store-candidate configuration: success;
- Windows store-candidate configuration: success;
- iOS simulator store-candidate configuration: success;
- Mac Catalyst store-candidate configuration: success.

## Store Inspection Artifacts #47

Run:

`31938301275`

Result: **success**.

- payload scanner self-test: success;
- Android unsigned AAB inspection publish/staging/provenance/upload: success;
- Windows self-contained inspection publish/staging/provenance/upload: success;
- iOS simulator inspection app build: success;
- unsigned Mac Catalyst inspection app publish: success;
- Apple inspection staging/checksums/provenance: success;
- Apple inspection artifact upload: success.

The earlier funding-free package boundary and forbidden-marker scanner remain green after this continuation.

## CodeQL #735

Run:

`31938301252`

Result: **success**.

## Dependency Audit #91

Run:

`31938301172`

Result: **success**.

- platform-neutral dependency graph: success;
- MAUI app dependency graph: success;
- no restoration of the former SQLite audit suppression.

---

# 12. Product behavior intentionally unchanged

This continuation does not intentionally change any user health-organizer behavior.

It does not change:

- profile storage semantics;
- medicine storage semantics;
- medicine strength/instruction opacity;
- dosage policy;
- schedule creation semantics;
- reminder reconciliation semantics;
- reminder action/snooze semantics;
- appointments;
- encrypted documents;
- encryption framing;
- manual backup format;
- database schema;
- SQLite transaction behavior;
- app lock;
- reports/exports;
- package identity;
- external-funding package policy;
- privacy wording;
- medical limitations;
- network/account/cloud scope.

The executable delta is limited to XAML binding metadata, XamlC build enforcement, and regression tests.

---

# 13. External funding/BMC boundary remains unchanged

The 2026-08-15 package-policy decision remains in force.

The CareNest distributed application runtime/package does not include the external Buy Me a Coffee destination or in-app funding card/command/artwork.

Repository-only funding documentation may remain separate from the application binary, subject to the already documented no-medical/no-health-entitlement boundary.

This compiled-binding continuation did not reintroduce any packaged BMC/funding marker. Store Inspection Artifacts #47 completed successfully after the changes.

---

# 14. Current automated interpretation

At the PR #74 frozen source head and merged source boundary:

- no compiled-binding warning suppression is required;
- strict XAML binding compilation is enabled;
- source bindings are configured for compilation;
- `XC0022`–`XC0025` are errors;
- 331/331 core tests pass;
- Android/Windows/iOS simulator/Mac Catalyst normal builds pass;
- Android/Windows/iOS simulator/Mac Catalyst store-candidate builds pass;
- Android/Windows/Apple inspection artifact workflows pass;
- CodeQL passes;
- unsuppressed dependency audit passes.

The previously documented compiled-binding optimization-warning debt is closed.

This supports the precise statement that no known automated defect remains under the configured PR #74 test/build/security/package-inspection matrix.

It is not a claim that every possible software bug is impossible.

---

# 15. Production blockers that remain open

CareNest remains `1.0.0-rc.1` and is not yet a production-published/store-approved release.

The remaining blockers are still real external/manual release gates.

## Real-device/platform behavior

- [ ] representative Android device/emulator matrix;
- [ ] notification permission denied/granted behavior;
- [ ] actual Android reminder delivery;
- [ ] exact/inexact alarm and battery-optimization behavior;
- [ ] Android reboot/restart/clock/time-zone/DST recovery;
- [ ] Windows manual reminder/lifecycle matrix;
- [ ] iPhone/iPad real-device notification/permission/recovery matrix;
- [ ] Mac Catalyst manual notification/lifecycle matrix;
- [ ] cancellation-first reminder actions and snooze against real platform schedulers.

## Packaged existing-data/encryption compatibility

- [ ] representative fictional existing-data packaged upgrade/install;
- [ ] SQLite integrity/readability/editability after upgrade;
- [ ] reminder rebuild/reconciliation after packaged upgrade;
- [ ] packaged encrypted-document import/export compatibility;
- [ ] packaged encrypted-backup create/inspect/restore/wrong-password/tamper behavior;
- [ ] genuine historical encrypted document/backup fixtures where real prior bytes exist.

## Accessibility

- [ ] screen-reader testing;
- [ ] large-text/text-scaling validation;
- [ ] keyboard/focus validation on desktop targets;
- [ ] light/dark/system contrast;
- [ ] reduced-motion behavior;
- [ ] color-independent state verification.

Compiled bindings are now complete, but they are not a substitute for real accessibility testing.

## Signing, store, release and publication

- [ ] production Android signing identity outside Git;
- [ ] Apple signing/provisioning outside Git;
- [ ] Windows production signing identity outside Git;
- [ ] final production-signed packages;
- [ ] signed-package checksums/provenance;
- [ ] final screenshots/listing/privacy/data-safety metadata;
- [ ] submission-time Apple/Google/Microsoft policy review where applicable;
- [ ] exact approved production source commit;
- [ ] immutable approved `v*` tag;
- [ ] tagged CI/CodeQL/Dependency Audit/Store Package/Store Inspection/Release Gate/Release Evidence success;
- [ ] final publication evidence.

Use:

`docs/releases/NEXT_STEPS.md`

and:

`docs/releases/PACKAGED_RELEASE_VALIDATION.md`

for the remaining production-validation workflow.

---

# 16. Documentation changes in this continuation

The documentation continuation is intentionally separate from the frozen executable PR head.

Completed documentation work includes:

- exact archive of the previous active `what_changed.md`;
- exact archive of the previous `PROJECT_STATUS.md`;
- exact archive of the previous `docs/releases/NEXT_STEPS.md`;
- exact archive of the previous `CHANGELOG.md`;
- exact archive of the previous `docs/README.md`;
- new permanent `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`;
- active `what_changed.md` promotion to the PR #74 merged source boundary;
- active status/next-steps/changelog/docs-index promotion in the same documentation continuation.

Documentation commits do not alter the verified executable implementation.

---

# 17. Continuation rule from here

Do not perform broad source refactoring merely to keep development active.

The source-controlled RC1 feature scope plus the compiled-binding cleanup is complete under the current automated matrix.

The next meaningful work is the manual/package/accessibility/signing/store validation listed above.

If those validations expose a real defect:

1. reproduce the defect with fictional/safe data where applicable;
2. fix the smallest correct source boundary;
3. add regression coverage;
4. run the complete exact-head automated matrix again;
5. update release evidence only after the new frozen head is green.

Current interpretation:

**CareNest `1.0.0-rc.1` remains source-complete with strict compiled XAML binding enforcement and no known automated defect under the PR #74 matrix; production validation remains open.**
