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

---

# Complete previous active handoff — preserved verbatim

> **Historical snapshot notice:** Everything below this notice is the complete previous active `what_changed.md` preserved verbatim for continuity. It describes the 2026-08-15 source boundary and is superseded for current status by the 2026-08-16 sections above. It is retained here specifically so the active handoff is not shortened and no earlier detail is skipped.

# CareNest — Final Active Completion Handoff

**Date:** 2026-08-15  
**Release candidate:** `1.0.0-rc.1`  
**Repository:** `sanskarIN/CareNest`

This is the active final continuation handoff after the 2026-08-15 source completion, store-package hardening, package-level bug investigation, final bug/error sweep, and exact merged-source verification.

The exact previous active `what_changed.md` is preserved byte-for-byte at:

`docs/history/pre-final-bug-audit-20260815/what_changed.md`

The matching pre-final status/next-steps files are preserved at:

- `docs/history/pre-final-bug-audit-20260815/PROJECT_STATUS.md`
- `docs/history/pre-final-bug-audit-20260815/NEXT_STEPS.md`

Those files retain all earlier PR #54/#56/#58/#59/#61 and prior continuation detail. Nothing from the old active handoff was discarded from the repository.

---

# 1. Final source boundary

The authoritative verification-relevant executable/project/test/workflow/build-script source is:

`9ec7b4e7d2150d9cc50be19f30464080318b16e8`

This is the merge commit for PR #67:

`Fix final store-safe funding payload isolation`

Final PR #67 feature-branch source head before merge:

`0fa552ca824f034ce7426513a7d3e50eaa0ef7aa`

Documentation-only commits after `9ec7b4e7...` record evidence/status and do not alter the executable source proven by final marker PR #68.

CareNest remains `1.0.0-rc.1` because production signing, real-device/accessibility/package-compatibility/store/tag/publication evidence is still external work.

---

# 2. Starting point of this final continuation

This final continuation began from `main` source:

`ab3844a5fa07efd66d78fcf604defc312f5f719b`

At that point the repository had already reached a strong automated baseline, but marker PR #66 had exposed a real unresolved store-package defect:

- core source/tests were green;
- Android funding-disabled inspection was green;
- Windows funding-disabled compilation was green;
- the generated Windows self-contained package still contained `buymeacoffee.com/sanskarIN` inside `CareNest.App.dll`;
- the payload scanner correctly failed the artifact staging step and prevented the affected Windows artifact from being uploaded.

The final work therefore concentrated on **finding the actual package-level root cause**, rather than disabling the scanner or pretending that source/build flags were enough.

---

# 3. Store payload scanner and package-inspection hardening retained

Before the final root cause was found, the repository added/retained defense-in-depth package inspection:

- a reusable `build/scripts/verify-store-safe-payload.py` scanner;
- canonical forbidden-marker inspection for `buymeacoffee.com/sanskarIN`;
- UTF-8 scanning;
- UTF-16 little-endian scanning;
- UTF-16 big-endian scanning;
- ordinary file/directory scanning;
- ZIP/AAB entry scanning;
- fail-closed behavior for missing/unreadable payloads;
- a workflow self-test proving:
  - clean payload passes;
  - UTF-8 marker fails;
  - UTF-16 marker fails;
  - marker inside ZIP/AAB fails;
  - missing payload path fails;
- Android AAB payload scan before artifact staging/upload;
- Windows self-contained publish-tree scan before archive/upload;
- iOS simulator `.app` scan;
- Mac Catalyst `.app` scan;
- artifact checksum/provenance recording;
- exact source SHA/ref separation from GitHub PR event/merge identity;
- internal-artifact `store_submission_ready=false` boundaries.

Important commits from the package-scanner phase include:

- `485d789cf324887902130e123a5551e20158489f` — `test: guard store funding payload boundary`;
- `9261488773f1f1a9b6003690d95cfe2859ac85a6` — `ci: self-test store-safe payload scanner`;
- `db9c750db9270f7c5b43a2f5a3ecbb2b65dd8ce0` — `test: guard store payload scanner self-test`;
- `e7da23f0bd85afde356d257feca42bca7a173b10` — `docs: define store-safe payload URL inspection`;
- `c8a35e25feb46fb0c8e62eac8444b302ba4a1315` — `docs: require funding scan in package validation`;
- `f6872910a8f4150816564bd4c8ff5c8a1693f501` — `test: align funding surface contract with payload isolation`.

The scanner was deliberately kept strict throughout the investigation. It was never weakened to make a red Windows job pass.

---

# 4. Failure-driven verification chronology

The final correction was reached through several explicit failed/superseded checkpoints. These failures are part of the engineering evidence, not hidden history.

## PR #64

The initial scanner/payload-boundary source exposed stale source-policy contracts.

Result:

- source-policy/UI tests failed because older tests still required the previous shared funding constant/shape;
- runtime source direction was not reverted;
- stale contracts were corrected;
- PR #64 remained failure-driven evidence and was closed without merge.

## PR #65

Corrected source-policy checkpoint.

Important evidence:

- unit tests: 122 passed;
- integration tests: 39 passed;
- UI/source-policy: 163 passed at that source boundary;
- total: 324/324;
- Android unsigned AAB payload scan: success;
- Windows publish: success;
- Windows payload staging: **failure** because `CareNest.App.dll` still contained the forbidden funding marker.

This proved that merely moving/removing the shared constant was insufficient.

PR #65 was closed without merge.

## PR #66

Nested-build/property-propagation hypothesis checkpoint.

The project was hardened so store funding policy propagated into inner Windows builds and invalid values failed closed.

Exact core result at that source boundary:

- unit: 122;
- integration: 39;
- UI/source-policy: 164;
- total: **325/325**;
- CodeQL: success;
- unsuppressed Dependency Audit: success;
- scanner self-test: success;
- Android AAB inspection: success;
- Windows funding-disabled compilation: success;
- Windows package payload scan: **failure** again.

The completed Windows log later proved the relevant values really evaluated false. This ruled out the simple "property was dropped in an inner build" explanation.

PR #66 was closed without merge.

## PR #67 intermediate physical-source/effective-property attempts

Additional deliberate attempts were made before the actual resource cause was known:

- `399e67910468f35791c538146765da741ff2bcc2` — physically separated enabled/disabled funding compile units;
- `6ab0a00bb71f1d3db3afb47eb56299a44e106dbc` — aligned ViewModel contracts to that physical split;
- `1aa453db5e7fa986c0342953ba174c2805d12f33` — made store funding environment authoritative through an effective property;
- `7a68753c3eba3df2f753f85d7e39636dbb023d10` — guarded effective store funding precedence.

Windows logs showed:

- requested funding value: false;
- store funding value: false;
- effective funding value: false.

Yet the Windows package still contained the marker.

That evidence eliminated the remaining C#/MSBuild-toggle theories and forced a repository resource audit.

---

# 5. Actual root cause found

The real root cause was:

`src/CareNest.App/Resources/Images/buy_me_a_coffee_carenest.svg`

The SVG itself contained the full external destination:

`https://buymeacoffee.com/sanskarIN`

inside its accessibility/text content.

Windows MAUI resource processing embedded this resource content into the managed application payload, which is why:

- C# funding code could be conditionally disabled;
- the requested/store/effective properties could all be false;
- Windows compilation could still succeed;
- and `CareNest.App.dll` could still contain the external funding marker.

This is why the package scanner was essential: source-level inspection alone would have produced a false sense of success.

---

# 6. Final product fix: application package is funding-surface-free

Once the root cause was identified, the design was simplified instead of adding another fragile build switch.

The final app runtime/package does **not** include or expose the external project-funding destination.

## Runtime changes

- `f5ba0eefa7c7fa548120743431f2f0dcd25b8172` — `fix: remove funding command from app runtime`
  - removed `SupportProjectCommand`;
  - removed runtime funding command wiring;
  - About keeps normal repository/creator/business/support/privacy/terms/security/notices links.

- `d82ff77540b2652ab3fd3c911c6080b6d9e6369e` — `fix: remove external funding surface from about page`
  - removed Buy Me a Coffee card/button/image/copy from the packaged About UI.

- `cc1694180b0a2e58d30848792eca961f91b7a19e` — `fix: remove URL-bearing funding policy source`.

- `04b98cd270a4aa2b353384cb3da0d0567baa37b1` — `fix: remove obsolete funding policy shim`.

## Resource changes

- `c5517b2088d3868b57c7c52592b12df328c6f8f6` — `fix: remove packaged funding destination artwork`
  - removed the actual URL-bearing SVG that caused the Windows payload leak.

- `12107d86705d4e6533a25a49c01d32ed94a17dde` — `fix: remove obsolete packaged support artwork`.

## Build/project simplification

- `fe35b1cab40e6d468e984c23467a1339848404bf` — `build: remove obsolete app funding build switch`
  - removed `CareNestShowFundingLink`;
  - removed effective funding property machinery;
  - removed funding conditional compile machinery.

There is now no per-store app funding toggle because the app package itself has no external project-funding destination.

---

# 7. Final source-policy regression coverage

The final tests protect the simplified product boundary rather than the superseded build-toggle architecture.

Important commits:

- `5f12dfb3b283d62c847e35591f6d0dd371087d89` — `test: forbid funding destination across app runtime tree`
  - recursively inspects text-like files under `src/CareNest.App`;
  - rejects the canonical BMC destination;
  - rejects deleted funding symbols/surfaces;
  - requires deleted funding policy/artwork files to remain absent.

- `c81c21d62461c3caa0ca0119397c4a2e9e440359` — `test: keep voluntary funding outside app runtime`.

- `fe72d21d95c999fe96fb21e2b6f94c946e6de85b` — `test: match repository funding entitlement wording`.

- `46d001af7951236a8fcc296fadd9d0c89b66d522` — `test: enforce funding-free about runtime`.

- `bb88d0ad75218794d98bcc05a545366696bf828a` — `test: enforce funding-free about view model`.

- `bbd43a1786ef8f4f0e7f9f42ace7e2e314ade7b3` — `test: keep package branding free of funding artwork`.

- `0fa552ca824f034ce7426513a7d3e50eaa0ef7aa` — `test: align branding contracts with funding-free package`.

The final UI/source-policy suite is 164 tests at the exact verified source boundary.

---

# 8. Repository funding documentation remains optional and non-medical

Voluntary project funding remains repository-only where appropriate. It is not part of the app package.

Documentation corrections include:

- `491b6faf0ab911f46467a61ba7f9d1b0bed5e86c` — `docs: keep voluntary funding link repository-only`;
- `3a38dbd5c4447ea599bcfe396bb10c02ab108d2e` — `docs: remove deleted funding artwork reference`;
- `3bc60d307be700c9cf966ed384ff9ab425fb7d19` — `docs: keep funding support documentation-only`.

Repository support text explicitly states that funding does not unlock:

- medical advice;
- health functionality;
- reminder priority/reliability;
- emergency assistance;
- account behavior;
- health-data access;
- premium clinical service.

Broken image references to deleted app funding artwork were removed from support documentation.

---

# 9. Store/release workflow simplification

Once the app package became funding-surface-free by source policy, obsolete build toggles were removed from release engineering.

## Store package workflow

- `730b1e3376f2b4914353aadc957bbaa95b1c92a3` — `ci: remove obsolete funding switch from store builds`.

Store Package Configuration now validates store-candidate Release configurations for:

- Android;
- Windows;
- iOS simulator;
- Mac Catalyst;

without a funding-property fork.

## Store inspection workflow

- `d0f99afc4ec39ceb2baaaf6bb5cb7f05f8cdbd72` — `ci: make payload scan the store funding invariant`.

The stronger invariant is now:

`external_funding_surface=absent_by_source_policy`

plus actual built-payload evidence:

`funding_url_payload_scan=passed`

## Workflow contracts

- `a2ce04b9954ff1284379b7a298f2ddc27bdfcc68` — `test: enforce funding-free store build workflow`;
- `bb3937e08b55b2711e8d08b6cd944c5d4f71fabd` — `test: enforce source-policy funding absence in artifacts`.

## Release preflight

- `e58b1cc4faec525335b8e7b4902c635db8aedc34` — `build: remove obsolete funding toggle from bash preflight`;
- `798b41087077c5e8a914e4b24e33212e9da09ebd` — `build: remove obsolete funding toggle from powershell preflight`.

The normal release preflight still performs:

- source hygiene;
- formatting;
- core Release builds;
- unit tests;
- integration tests;
- UI/source-policy tests;
- unsuppressed dependency audit;
- optional MAUI target restore/audit/build.

## Store package preflight

- `27d5d5cd64cc6a97e673d6c1b1aa45f9fcd13e85` — `build: simplify bash store package preflight`;
- `7589921fd0968f68be881ca0435a87bd6c868970` — `build: simplify powershell store package preflight`;
- `3eaa1da39cf273f8138ff2110dd79516ddf9eaef` — `test: enforce funding-free store package preflight`.

Store preflight now:

- requires an explicit supported target;
- rejects unsupported/missing target values;
- delegates to release preflight;
- reports that external funding surface is absent from app runtime by source policy.

---

# 10. PR #67 final pre-merge evidence

Final PR #67 source head:

`0fa552ca824f034ce7426513a7d3e50eaa0ef7aa`

## CareNest CI #717

Run:

`31880445293`

Result: **success**.

- formatting: success;
- unit: **122 passed, 0 failed, 0 skipped**;
- integration: **39 passed, 0 failed, 0 skipped**;
- UI/source-policy: **164 passed, 0 failed, 0 skipped**;
- total: **325 passed, 0 failed, 0 skipped**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success.

## Store Package Configuration #106

Run:

`31880445239`

All success:

- Android store-candidate configuration;
- Windows store-candidate configuration;
- iOS simulator store-candidate configuration;
- Mac Catalyst store-candidate configuration.

## Store Inspection Artifacts #40

Run:

`31880445403`

All success:

- scanner self-test;
- Android unsigned AAB publish/scan/provenance/upload;
- Windows self-contained publish/**payload scan**/provenance/upload;
- iOS simulator inspection app scan;
- unsigned Mac Catalyst inspection app scan;
- Apple checksums/provenance/upload.

This was the first completely green Windows package scan after the real URL-bearing SVG root cause was removed.

## Security/dependency

- CodeQL #717 / run `31880445284`: success;
- unsuppressed Dependency Audit #84 / run `31880445286`: success on platform-neutral and MAUI graphs.

PR #67 evidence was recorded in its conversation before merge.

---

# 11. PR #67 merged executable source

PR #67 was merged to `main` as:

`9ec7b4e7d2150d9cc50be19f30464080318b16e8`

Merge subject:

`fix: eliminate packaged external funding surface`

This SHA is the authoritative executable source for the final RC1 automated baseline.

No verification marker file entered `main`.

---

# 12. Final exact merged-source verification — PR #68

A fresh marker-only branch was created from exact merged `main` source `9ec7b4e7d2150d9cc50be19f30464080318b16e8`.

Verification branch:

`ci/carenest-final-bug-audit-20260815`

Marker file:

`build/verification/final-bug-audit-20260815.txt`

Marker commit:

`c752815c311e7e443f1d71df8a9197cf706a14b6`

PR:

`https://github.com/sanskarIN/CareNest/pull/68`

PR #68 properties:

- base source: `9ec7b4e7d2150d9cc50be19f30464080318b16e8`;
- one changed file;
- 14 additions;
- 0 deletions;
- marker contains no executable product change;
- closed without merge.

## CareNest CI #719

Run:

`31880955724`

Result: **success**.

- formatting: success;
- unit: **122/122**;
- integration: **39/39**;
- UI/source-policy: **164/164**;
- total: **325/325**;
- failed: 0;
- skipped: 0;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success.

## Store Package Configuration #108

Run:

`31880955723`

All success:

- Android;
- Windows;
- iOS simulator;
- Mac Catalyst.

## Store Inspection Artifacts #41

Run:

`31880955734`

All success:

- payload scanner self-test;
- Android AAB build/publish/scan/provenance/upload;
- Windows self-contained publish/**forbidden-marker scan**/provenance/upload;
- iOS simulator inspection bundle scan;
- unsigned Mac Catalyst inspection bundle scan;
- Apple provenance/checksums/upload.

The exact merged-source Windows scan is the decisive regression proof for the package defect that had failed PR #65/#66 and intermediate PR #67 attempts.

## Security/dependency

- CodeQL #719 / run `31880955720`: **success**;
- Dependency Audit #85 / run `31880955731`: **success** on both platform-neutral and MAUI graphs.

PR #68 was closed without merge after evidence was recorded.

---

# 13. Final bug/error sweep

After PR #67 was merged and PR #68 completed, the repository received a final non-mutating bug/error sweep.

Results:

- open GitHub issues: none found;
- indexed `TODO`: none found;
- indexed `FIXME`: none found;
- indexed `HACK`: none found;
- indexed `NotImplementedException`: none found;
- indexed `DateTime.Now`: none found;
- indexed `GetAwaiter`: none found.

The source-policy suite additionally guards concrete ViewModels against:

- `async void`;
- `Task.Run` misuse;
- direct `SQLiteAsyncConnection`/repository persistence implementation reach-through;
- direct `HttpClient`/`WebClient` creation.

Existing reminder, SQLite, backup, document-encryption, package-metadata, platform-permission, release-workflow, app-lock and data-lifecycle regression coverage remains green.

The exact truthful interpretation is:

**No known automated defect remains at executable source `9ec7b4e7...` under the configured test/build/CodeQL/dependency-audit/package-inspection matrix.**

This is deliberately not written as “CareNest is guaranteed bug-free,” because no nontrivial software can be proven to have no possible undiscovered defect, and real-device/production-package evidence is still pending.

---

# 14. Non-blocking warnings observed

Some MAUI platform builds continue to emit XamlC compiled-binding optimization warnings such as:

- `XC0022`;
- `XC0025`.

They did not fail:

- normal Release builds;
- store-candidate builds;
- package publishing;
- payload scans;
- core tests;
- CodeQL;
- dependency audit.

They are classified as non-blocking performance/maintainability opportunities, not known functional RC1 defects.

They can be reduced post-RC by adding/strengthening compiled binding metadata while preserving the exact-source verification protocol.

---

# 15. SQLite security state remains green

The previously tracked `GHSA-2m69-gcr7-jv3q` dependency exception remains remediated.

The verified source keeps:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- maintained centrally pinned native/provider leaves;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- selected providers at `2.1.12`;
- the former exact audit suppression removed.

PR #68 Dependency Audit #85 is green without restoring that exception.

Packaged existing-user-data compatibility remains a manual/package release gate and must not be conflated with a green dependency graph.

---

# 16. Documentation-only finalization after source freeze

After executable source `9ec7b4e7...` was exact-head verified, only documentation/history files were changed.

## Final evidence and policy/runbook

- `bfe553b0968359f9c32755784fa35187ba0d8f91` — `docs: record final store payload and bug audit verification`
  - added `docs/releases/FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md`.

- `36ad1c264dc62c09f7bd576d0f8d08af015c14dc` — `docs: align store policy with funding-free app packages`
  - updated `docs/releases/STORE_BUILD_POLICY.md`;
  - removed stale per-store funding-toggle instructions;
  - documented the actual URL-bearing SVG root cause;
  - documented source-policy + payload-scan boundary.

- `17c2318a45acf53ccd7bd2ce6df0e37f3936f193` — `docs: update packaged validation for funding-free app`
  - updated `docs/releases/PACKAGED_RELEASE_VALIDATION.md`;
  - retained full device/SQLite/encryption/accessibility/signing/store/tag manual gates.

## Historical active-file preservation

Before replacing stale PR #61/current-toggle wording, exact previous active files were preserved byte-for-byte via PR #69:

- `docs/history/pre-final-bug-audit-20260815/PROJECT_STATUS.md`;
- `docs/history/pre-final-bug-audit-20260815/NEXT_STEPS.md`;
- `docs/history/pre-final-bug-audit-20260815/what_changed.md`.

History-preservation commits:

- `fa50cded0b2f8370e68d6e066b3b6467da2d6214` — branch commit;
- `002f4c6d95b9b9f2cc2c9d0c567e559cbb3984a5` — merged PR #69 preservation commit.

PR #69 added 2,998 lines and 0 deletions.

## Active documentation promoted to final baseline

- `d669b885b16fd4178d7e4e6a8234f5ee64cecd7d` — `docs: promote final PR68 project status`
  - `PROJECT_STATUS.md` now reports `9ec7b4e7...` / PR #68 and real remaining blockers.

- `1ce0ce20182963429f4aba574201639da108f17e` — `docs: finalize post-PR68 production next steps`
  - `docs/releases/NEXT_STEPS.md` now contains only actual post-source production work;
  - obsolete “build with funding=false” actions are removed;
  - source-wide refactoring is explicitly discouraged unless a real defect is found.

- `2c9e51096a6d0df67e30372b12296ef59dc3d903` — `docs: promote final PR68 documentation baseline`
  - `docs/README.md` now points first to final evidence/status/remaining gates.

This active `what_changed.md` finalization is documentation-only and does not alter the verified executable source.

---

# 17. Current canonical documentation entry points

Use these files first:

- `PROJECT_STATUS.md` — final active source/evidence/release-blocker status;
- `what_changed.md` — this active final handoff;
- `docs/README.md` — documentation hub;
- `docs/releases/FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md` — authoritative PR #67/#68 final automated evidence;
- `docs/releases/STORE_BUILD_POLICY.md` — final funding-free app-package policy;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md` — packaged/manual validation runbook;
- `docs/releases/NEXT_STEPS.md` — exact production-validation work still open;
- `docs/releases/RELEASE_CHECKLIST.md` — production release gates;
- `docs/releases/MANUAL_TEST_MATRIX.md` — manual target evidence;
- `docs/security/DEPENDENCY_RISK_REGISTER.md` — dependency security status;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` — SQLite security/manual compatibility boundary.

Historical exact pre-final active handoffs:

`docs/history/pre-final-bug-audit-20260815/`

Earlier PR-specific release evidence remains under `docs/releases/` and Git history.

---

# 18. Final product boundary

CareNest RC1 remains deliberately local-first and organizational.

It does **not**:

- diagnose health conditions;
- infer or calculate medication dosage;
- recommend treatment;
- perform clinical drug-interaction decision making;
- calculate clinical risk scores;
- independently verify adherence;
- replace a clinician/pharmacist;
- replace emergency services;
- guarantee reminder/notification delivery.

Current v1 intentionally has:

- no required CareNest account;
- no required backend;
- no automatic CareNest cloud synchronization/upload;
- no silent remote health-data sharing;
- no hidden runtime analytics/telemetry client;
- local structured SQLite storage;
- separately encrypted imported document payloads;
- password-encrypted manual backups;
- explicit export/share/calendar/browser boundaries;
- optional repository funding separate from the app package.

---

# 19. What is actually complete

The following are complete for the current source-controlled RC1 boundary:

- documented RC1 source feature scope;
- repository architecture/layering;
- local-first/privacy product boundary;
- medicine/schedule/log/reminder source behavior;
- appointment source behavior;
- encrypted document vault source behavior;
- reports/export source behavior;
- encrypted manual backup/restore source behavior;
- Settings/app-lock source behavior;
- SQLite transaction/correctness hardening;
- SQLite dependency advisory remediation;
- reminder reconciliation/cancellation-first recovery hardening;
- Android/Windows/iOS/Mac Catalyst compile verification;
- store-candidate compile verification;
- Android/Windows/Apple internal package inspection;
- forbidden external-funding marker byte scan;
- scanner self-test;
- CodeQL;
- unsuppressed Dependency Audit;
- final exact merged-source marker verification;
- final source/repository automated bug sweep;
- final active release documentation alignment.

The final exact automated baseline is **325/325 tests** plus all configured build/security/package-inspection gates green.

---

# 20. What is not complete and must remain open

Source completion and automated inspection do not magically complete physical device/signing/store work.

## Real device/platform behavior

Still required:

- Android representative device/emulator matrix;
- notification permission denied/granted behavior;
- actual reminder delivery;
- exact/inexact alarm behavior;
- battery-optimization behavior;
- reboot/restart recovery;
- clock/time-zone/DST behavior;
- cancellation-first Taken/Skipped/Delayed/Missed handling against real OS scheduling;
- snooze cancellation/replacement against real OS scheduling;
- Windows reminder/lifecycle matrix;
- iPhone/iPad real-device matrix;
- Mac Catalyst manual matrix.

## Packaged existing-data/SQLite compatibility

Still required with fictional representative data:

- realistic packaged upgrade/install path;
- database opens;
- SQLite integrity passes;
- representative records remain readable/editable;
- schema version correct;
- reminder reconciliation after upgrade;
- no duplicate/stale OS requests.

## Encrypted compatibility

Still required:

- final packaged document import/open/export/delete;
- packaged backup create/inspect/restore;
- wrong-password rejection;
- tamper/truncation rejection;
- clean-install restore;
- genuine historical document/backup fixture compatibility when canonical earlier bytes exist.

Do not manufacture a new fixture and call it historical evidence.

## Accessibility

Still required with actual assistive technology:

- screen reader;
- large text/text scaling;
- keyboard/focus;
- contrast/theme;
- reduced motion;
- color-independent state;
- privacy-safe errors.

## Signing and production packages

Still required outside Git:

- Android production signing;
- Apple signing/provisioning;
- Windows signing if applicable;
- final signed packages;
- source/package identity inspection;
- final package SHA-256;
- signing/notarization/store-managed provenance;
- final signed-package forbidden-marker scan;
- installed signed-package smoke tests.

## Store and publication

Still required:

- current submission-time store-policy review;
- store screenshots/listing/privacy/data-safety metadata;
- exact approved production source commit;
- exact immutable `v*` tag;
- tagged CareNest CI;
- tagged CodeQL;
- tagged unsuppressed Dependency Audit;
- tagged Store Package Configuration;
- tagged Store Inspection Artifacts;
- tagged Release Gate;
- tagged Release Evidence;
- final publication checksums/provenance/evidence.

---

# 21. Final safe continuation order

The project should **not** receive more broad source changes merely to continue coding. The executable RC1 source is already exact-head verified.

The correct continuation is:

1. perform packaged existing-data SQLite compatibility using fictional data;
2. perform packaged encrypted document/backup validation;
3. complete Android manual reminder/lifecycle matrix;
4. complete Windows manual matrix;
5. complete iPhone/iPad real-device matrix;
6. complete Mac Catalyst manual matrix;
7. complete assistive-technology accessibility matrix;
8. configure production signing outside Git;
9. build final signed candidate packages;
10. scan final signed packages for the forbidden external funding marker;
11. manually inspect final About/support/legal surfaces;
12. complete store metadata/privacy/data-safety/policy review;
13. freeze the exact approved production commit;
14. if verification-relevant source changed after `9ec7b4e7...`, repeat the marker-only exact-head verification protocol;
15. create the approved non-movable `v*` tag;
16. require every tagged automated/release evidence gate to pass;
17. publish only after all applicable manual/package/signing/store rows are actually evidenced.

If any manual/package/security test finds a real defect:

1. reproduce it;
2. make the smallest correct source fix;
3. add regression coverage;
4. rerun the complete exact-head verification matrix;
5. update evidence only after the run is actually green.

---

# 22. Final truthful status

CareNest is now a **source-complete, heavily automated-verified `1.0.0-rc.1` release candidate**.

At executable source:

`9ec7b4e7d2150d9cc50be19f30464080318b16e8`

there are:

- 325/325 exact automated core tests passing;
- all four normal Release platform builds passing;
- all four store-candidate configurations passing;
- Android/Windows/Apple internal payload scans passing;
- scanner self-test passing;
- CodeQL passing;
- unsuppressed Dependency Audit passing;
- no open GitHub issues found in the final sweep;
- no indexed unfinished implementation markers found;
- no known automated defect remaining under the configured matrix.

The formerly failing Windows packaged funding-marker defect is fixed at the actual resource root cause and proven clean on the **merged** source by PR #68.

CareNest is **not yet** a production-published/store-approved/signed release and must not be described as globally bug-free until the remaining manual/external production gates are actually completed.
