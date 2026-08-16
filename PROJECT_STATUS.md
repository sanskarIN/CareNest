# Project Status

## Release target

`1.0.0-rc.1` source-complete release candidate.

CareNest is a local-first organizational health application. It does not diagnose conditions, calculate or infer dosage, recommend treatment, perform clinical medication-interaction checking, create clinical risk scores, replace qualified professionals, provide emergency services, or guarantee notification delivery.

The exact active status immediately before the 2026-08-16 compiled-binding continuation is preserved at:

`docs/history/pre-xaml-compiled-bindings-20260816/PROJECT_STATUS.md`

The earlier final-bug-audit status remains at:

`docs/history/pre-final-bug-audit-20260815/PROJECT_STATUS.md`

---

## Authoritative executable source

Current merged executable/project/test source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

This is the `main` merge commit for PR #74:

`Compile and enforce MAUI XAML bindings`

Verified PR #74 implementation/test head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

The merge was performed after every configured PR-head workflow completed successfully.

Permanent evidence:

`docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`

The previous authoritative executable source `9ec7b4e7d2150d9cc50be19f30464080318b16e8` and PR #68 evidence remain historical proof for the 2026-08-15 package-funding defect closure, but they are no longer the latest executable source boundary.

---

## 2026-08-16 compiled-binding hardening

The previous RC1 baseline still had non-blocking MAUI/XamlC compiled-binding warnings such as `XC0022` and `XC0025`.

PR #74 closes that source-side cleanup item without warning suppression.

Completed changes:

- all 15 binding-bearing XAML views declare an accurate root `x:DataType`;
- all binding-bearing DataTemplates declare their own row/item `x:DataType`;
- picker item-display bindings declare item types where the binding context switches;
- parent ViewModel commands used inside templates use typed ancestor binding contexts;
- explicit Source bindings carry binding-specific source type information;
- `MauiEnableXamlCBindingWithSourceCompilation` is enabled;
- `MauiStrictXamlCompilation` is enabled;
- `XC0022`, `XC0023`, `XC0024`, and `XC0025` are warnings-as-errors;
- matching warnings are not suppressed with `NoWarn`;
- compiled-binding type safety is not disabled through `x:Object` or `x:Null`;
- six permanent repository contract tests protect this policy.

The implementation changed XAML binding metadata, project enforcement, and tests only. It intentionally did not change health-data, reminder, persistence, backup, encryption, app-lock, report, package-identity, funding-policy, network, account, or cloud behavior.

---

## Current exact automated baseline — PR #74

Verification PR:

`https://github.com/sanskarIN/CareNest/pull/74`

Frozen source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

### CareNest CI #735

Run:

`31938301209`

Result: **success**.

- formatting: success;
- unit tests: **122 passed, 0 failed, 0 skipped**;
- integration tests: **39 passed, 0 failed, 0 skipped**;
- UI/source-policy tests: **170 passed, 0 failed, 0 skipped**;
- total core tests: **331 passed, 0 failed, 0 skipped**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success.

The UI/source-policy count increased by six because of the new compiled-binding contract suite.

All configured platform builds ran with `XC0022`–`XC0025` promoted to errors.

### Store Package Configuration #124

Run:

`31938301146`

Result: **success**.

- Android store-candidate configuration: success;
- Windows store-candidate configuration: success;
- iOS simulator store-candidate configuration: success;
- Mac Catalyst store-candidate configuration: success.

### Store Inspection Artifacts #47

Run:

`31938301275`

Result: **success**.

- payload scanner self-test: success;
- Android unsigned AAB inspection artifact: success;
- Windows self-contained inspection artifact: success;
- iOS simulator inspection build: success;
- unsigned Mac Catalyst inspection publish: success;
- Android/Windows/Apple staging, checksum/provenance and artifact upload: success.

The previous funding-free application-package boundary remains intact after the compiled-binding migration.

### Security and dependency gates

- CodeQL #735 / run `31938301252`: **success**;
- Dependency Audit #91 / run `31938301172`: **success**;
- platform-neutral dependency graph: success;
- MAUI dependency graph: success;
- former SQLite advisory suppression remains removed.

---

## Current automated interpretation

The configured PR #74 matrix now establishes all of the following at the current source boundary:

- 331/331 core tests green;
- four normal target builds green;
- four store-candidate target builds green;
- Android/Windows/Apple inspection artifacts green;
- package forbidden-marker scanner self-test green;
- CodeQL green;
- unsuppressed Dependency Audit green;
- strict compiled XAML bindings green on every configured target;
- `XC0022`/`XC0023`/`XC0024`/`XC0025` regression promoted to build failure.

This supports the precise statement:

**No known automated defect remains at the current merged source under the configured PR #74 test, build, security, dependency, strict-XAML, and package-inspection matrix.**

It does not prove that every possible software defect is impossible.

---

## External funding/package boundary

The 2026-08-15 funding-package correction remains unchanged.

The CareNest application runtime/package contains no external Buy Me a Coffee destination/card/command/artwork.

Repository funding documentation remains separate from the application package and does not create medical/health feature entitlement.

PR #74 did not reintroduce the funding marker; Store Inspection Artifacts #47 remained green.

---

## SQLite dependency security state

The former `GHSA-2m69-gcr7-jv3q` repository dependency exception remains remediated.

Current maintained dependency path remains governed by the centralized package configuration and unsuppressed audit gate.

Source dependency security and packaged existing-user-data compatibility remain separate concerns. The source dependency graph is green; actual packaged upgrade/readability evidence remains a manual release gate.

---

## Current release-engineering boundary

Production-style `v*` source remains configured to participate in the release matrix including:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- CareNest Store Package Configuration;
- CareNest Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

Release Gate and Release Evidence remain source-controlled fail-closed mechanisms, but production approval still requires the manual/package/signing evidence listed below.

---

## Production blockers still open

The source-controlled RC1 feature scope and compiled-binding cleanup are complete. The following external/manual gates remain real release blockers and must not be marked complete without evidence.

### Real-device/platform behavior

- [ ] Android representative device/emulator matrix;
- [ ] notification permission denied/granted behavior;
- [ ] actual Android reminder delivery;
- [ ] exact/inexact alarm and battery-optimization behavior;
- [ ] Android reboot, restart, clock and time-zone/DST recovery;
- [ ] Windows manual reminder/lifecycle matrix;
- [ ] iPhone/iPad real-device permission/delivery/recovery matrix;
- [ ] Mac Catalyst manual notification/lifecycle matrix;
- [ ] cancellation-first reminder actions and snooze behavior against real platform scheduling.

### Packaged data/encryption compatibility

Use fictional/synthetic health data for testing.

- [ ] representative existing-data upgrade/install through the intended package path;
- [ ] SQLite integrity/readability/editability after upgrade;
- [ ] reminder rebuild/reconciliation after packaged upgrade;
- [ ] packaged encrypted-document import/export compatibility;
- [ ] packaged encrypted-backup create/inspect/restore/wrong-password/tamper behavior;
- [ ] genuine historical encrypted document/backup fixtures where real prior bytes exist.

### Accessibility

- [ ] representative screen-reader testing;
- [ ] large text / text scaling;
- [ ] keyboard/focus on desktop targets;
- [ ] light/dark/system contrast;
- [ ] reduced-motion behavior;
- [ ] color-independent state verification.

The old compiled-binding-warning item is no longer part of accessibility/post-RC cleanup; it is completed and enforced.

### Signing, store and publication

- [ ] production Android signing identity/secret outside Git;
- [ ] Apple signing/provisioning outside Git;
- [ ] Windows production signing identity outside Git;
- [ ] final production-signed packages;
- [ ] signed-package checksums/provenance;
- [ ] store screenshots/listing/privacy/data-safety metadata;
- [ ] submission-time Apple/Google/Microsoft policy review as applicable;
- [ ] exact approved production source commit;
- [ ] non-movable approved `v*` tag;
- [ ] tagged CI/CodeQL/Dependency Audit/Store Package/Store Inspection/Release Gate/Release Evidence success;
- [ ] final publication evidence.

---

## Current interpretation

- CareNest remains `1.0.0-rc.1`.
- The documented local-first RC1 feature scope is source-complete.
- The MAUI XAML compiled-binding cleanup is complete and enforced.
- PR #74 / source head `8908fa9f...` is the latest complete automated verification boundary.
- Merge commit `e8f4aa0a...` is the current merged executable source.
- 331/331 core tests are green.
- Android, Windows, iOS simulator and Mac Catalyst normal and store-candidate configurations are green.
- Android, Windows and Apple internal inspection artifact workflows are green.
- CodeQL and unsuppressed Dependency Audit are green.
- The external Buy Me a Coffee destination remains repository-documentation-only and absent from the application runtime/package.
- No known automated defect remains under the configured PR #74 matrix.
- CareNest is **not** yet production-published, production-signed, store-approved, or proven globally bug-free.

Use:

- `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md` for the latest automated source evidence;
- `docs/releases/NEXT_STEPS.md` for the exact remaining production work;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md` for packaged/manual validation.

---

## Complete previous active project status — preserved verbatim

> **Historical snapshot notice:** Everything below this notice is the complete previous active `PROJECT_STATUS.md` preserved verbatim. It describes the 2026-08-15 source boundary and is superseded for current status by the sections above. It is retained so no prior status detail is shortened or skipped.

# Project Status

## Release target

`1.0.0-rc.1` source-complete release candidate.

CareNest is a local-first organizational health application. It does not diagnose conditions, calculate or infer dosage, recommend treatment, perform clinical medication-interaction checking, create clinical risk scores, replace qualified professionals, provide emergency services, or guarantee notification delivery.

The exact pre-final status document is preserved at:

`docs/history/pre-final-bug-audit-20260815/PROJECT_STATUS.md`

Earlier verification histories remain in Git history and the dated release evidence documents.

---

## Authoritative executable source

The final verification-relevant executable/project/test/workflow/build-script source is:

`9ec7b4e7d2150d9cc50be19f30464080318b16e8`

This is the `main` merge commit for PR #67:

`Fix final store-safe funding payload isolation`

PR #67 source head before merge:

`0fa552ca824f034ce7426513a7d3e50eaa0ef7aa`

Documentation-only commits after `9ec7b4e7...` record evidence and current status; they do not change the executable source verified by PR #68.

---

## Final packaged-funding defect and fix

Repeated Windows package inspection found `buymeacoffee.com/sanskarIN` inside `CareNest.App.dll` even when earlier funding properties evaluated false.

The final root cause was not a C# flag failure. The MAUI image resource:

`src/CareNest.App/Resources/Images/buy_me_a_coffee_carenest.svg`

contained the full external funding destination in its SVG accessibility/text content. Windows MAUI resource processing embedded that resource content into the managed payload.

The final source therefore removes the external project-funding surface from the application package entirely:

- no Buy Me a Coffee destination under `src/CareNest.App`;
- no funding command in `AboutViewModel`;
- no funding card/button in `AboutPage`;
- no funding-policy compile units;
- no packaged funding/support artwork carrying the destination;
- no `CareNestShowFundingLink` or equivalent app funding build switch;
- repository funding remains optional documentation/GitHub-funding metadata only;
- Android, Windows and Apple internal package outputs remain protected by byte-level forbidden-marker scanning.

Core app support/legal surfaces remain available: repository, creator profile, business email, application support email, privacy, terms, security, and bundled third-party notices.

---

## Authoritative exact automated baseline — PR #68

Verification PR:

`https://github.com/sanskarIN/CareNest/pull/68`

Frozen executable source/base:

`9ec7b4e7d2150d9cc50be19f30464080318b16e8`

Marker SHA:

`c752815c311e7e443f1d71df8a9197cf706a14b6`

Marker path:

`build/verification/final-bug-audit-20260815.txt`

PR #68 changed exactly one marker file, 14 additions, 0 deletions, and was closed without merge.

### CareNest CI #719

Run:

`31880955724`

Result: **success**.

- formatting: success;
- unit tests: **122 passed, 0 failed, 0 skipped**;
- integration tests: **39 passed, 0 failed, 0 skipped**;
- UI/source-policy tests: **164 passed, 0 failed, 0 skipped**;
- total core tests: **325 passed, 0 failed, 0 skipped**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success.

### Store Package Configuration #108

Run:

`31880955723`

Result: **success**.

- Android store-candidate configuration: success;
- Windows store-candidate configuration: success;
- iOS simulator store-candidate configuration: success;
- Mac Catalyst store-candidate configuration: success.

### Store Inspection Artifacts #41

Run:

`31880955734`

Result: **success**.

- payload scanner self-test: success;
- Android unsigned AAB publish + forbidden-marker scan + checksum/provenance + upload: success;
- Windows self-contained publish + forbidden-marker scan + checksum/provenance + upload: **success**;
- iOS simulator inspection bundle scan: success;
- unsigned Mac Catalyst inspection bundle scan: success;
- Apple checksum/provenance + upload: success.

The Windows payload scan is the decisive regression proof for the defect that earlier verification checkpoints exposed.

### Security and dependency gates

- CodeQL #719 / run `31880955720`: **success**;
- unsuppressed Dependency Audit #85 / run `31880955731`: **success** on platform-neutral and MAUI dependency graphs;
- former SQLite advisory suppression remains removed.

Permanent evidence:

`docs/releases/FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md`

---

## Final automated bug/error sweep

After the merge and exact marker verification:

- no open GitHub issues were found;
- no indexed `TODO` / `FIXME` / `HACK` / `NotImplementedException` placeholders were found;
- no indexed `DateTime.Now` use was found;
- no indexed `GetAwaiter` blocking-sync pattern was found;
- ViewModel contracts continue to reject `async void`, `Task.Run`, direct SQLite infrastructure access and direct network-client creation;
- reminder, backup/restore, encrypted-document, package-metadata, release-workflow and payload-boundary contracts remain part of the 164 UI/source-policy tests.

This supports the precise statement:

**No known automated defect remains at executable source `9ec7b4e7...` under the configured test, build, CodeQL, dependency-audit and package-inspection matrix.**

It does not prove that all possible software bugs are impossible.

Existing MAUI/XamlC `XC0022` / `XC0025` messages are compiled-binding optimization warnings. They did not fail the configured builds or quality gates and are currently treated as non-blocking maintainability/performance cleanup, not known functional defects.

---

## SQLite dependency security state

The former `GHSA-2m69-gcr7-jv3q` repository dependency exception is remediated in the verified source graph.

Current maintained path includes:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- selected SQLitePCLRaw providers at `2.1.12`;
- no old exact `NuGetAuditSuppress` entry.

Dependency security and packaged existing-user-data compatibility remain separate concerns. The source dependency graph is green; actual packaged upgrade/readability evidence remains a manual release gate.

---

## Current release-engineering boundary

Production-style `v*` source is configured to participate in the repository release matrix including:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- CareNest Store Package Configuration;
- CareNest Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

Release Evidence and Release Gate remain source-controlled fail-closed mechanisms, but production approval still requires the manual/package/signing evidence listed below.

Store-package preflight requires an explicit supported target and delegates to the standard release preflight. The app no longer has a per-store funding toggle because the external funding destination is absent from the distributed app by source policy.

---

## Production blockers still open

Automated source/package inspection is complete for the frozen RC1 source. The following external/manual gates are still real release blockers and must not be marked complete without evidence:

### Real-device/platform behavior

- [ ] Android representative device/emulator matrix;
- [ ] notification permission denied/granted behavior;
- [ ] actual Android reminder delivery;
- [ ] exact/inexact alarm and battery-optimization behavior;
- [ ] Android reboot, restart, clock and time-zone/DST recovery;
- [ ] Windows manual reminder/lifecycle matrix;
- [ ] iPhone/iPad real-device permission/delivery/recovery matrix;
- [ ] Mac Catalyst manual notification/lifecycle matrix;
- [ ] cancellation-first reminder actions and snooze behavior against real platform scheduling.

### Packaged data/encryption compatibility

- [ ] representative fictional existing-data upgrade/install using the intended packaged path;
- [ ] SQLite integrity/readability/editability after upgrade;
- [ ] reminder rebuild/reconciliation after packaged upgrade;
- [ ] packaged encrypted-document import/export compatibility;
- [ ] packaged encrypted-backup create/inspect/restore/wrong-password/tamper behavior;
- [ ] canonical historical encrypted document/backup fixtures where genuine prior bytes exist.

### Accessibility

- [ ] representative screen-reader testing;
- [ ] large text / text scaling;
- [ ] keyboard/focus on desktop targets;
- [ ] light/dark/system contrast;
- [ ] reduced-motion and color-independent state verification.

### Signing, store and publication

- [ ] production Android signing identity/secret outside Git;
- [ ] Apple signing/provisioning outside Git;
- [ ] Windows production signing identity outside Git;
- [ ] final production-signed packages;
- [ ] signed-package checksums/provenance;
- [ ] store screenshots/listing/privacy/data-safety metadata;
- [ ] submission-time Apple/Google/Microsoft policy review as applicable;
- [ ] exact approved production source commit;
- [ ] non-movable approved `v*` tag;
- [ ] tagged CI/CodeQL/unsuppressed Dependency Audit/Store Package/Store Inspection/Release Gate/Release Evidence success;
- [ ] final publication evidence.

---

## Current interpretation

- CareNest remains `1.0.0-rc.1`.
- The documented local-first RC1 feature scope is source-complete.
- PR #68 is the authoritative exact automated/source/package-inspection baseline.
- PR #67 contains the final merged executable source correction.
- Earlier PR #54/#56/#58/#59/#61 evidence remains historical for those frozen source boundaries.
- The external Buy Me a Coffee destination is repository-documentation-only and absent from the app runtime/package source boundary.
- 325/325 core tests are green on the exact merged executable source.
- Android, Windows, iOS simulator and Mac Catalyst normal and store-candidate configurations are green.
- Android, Windows and Apple internal inspection payload scans are green.
- CodeQL and unsuppressed Dependency Audit are green.
- No known automated defect remains under the configured exact-source matrix.
- CareNest is **not** yet production-published, production-signed, store-approved, or proven globally bug-free.

Use `docs/releases/FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md` and `docs/releases/PACKAGED_RELEASE_VALIDATION.md` as the final automated evidence and remaining production-validation runbook.
