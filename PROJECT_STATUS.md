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
