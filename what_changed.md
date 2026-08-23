# CareNest — Cross-Platform Current-Main Continuation Handoff

**Date:** 2026-08-23  
**Release line:** `1.0.0-rc.1`  
**Repository:** `https://github.com/sanskarIN/CareNest`  
**Continuation branch:** `continue/cross-platform-current-main-20260823`  
**Pull request:** `#84` — `feat: complete cross-platform hosts on current main`  
**Current base used for this continuation:** `f58aaca1d1d7a3fef68cb30b8b9a68fa0f94bf09`  
**Head before this handoff replacement:** `49bb93bfdd5b90f128dbde5a95b0b594c6723305`

The complete production-evidence handoff that was active before this continuation is preserved byte-for-byte at:

`docs/history/cross-platform-before-current-main-20260823/what_changed.md`

That archive is retained instead of deleting or shortening the previous active handoff.

---

## 1. Continuation decision

The repository had no open issues. The active remaining implementation work was PR #83, which added Linux desktop and WebAssembly/browser presentation hosts.

PR #83 had successful workflow runs for its own head, but it had diverged from current `main` after PR #82 merged. A direct compare showed the old feature head was **27 commits ahead and 47 commits behind** current `main` with merge base `e9043a23de1e3aa5b46e7faf88d0e1201f9a2db6`.

The cross-platform work was therefore rebuilt directly on current `main` rather than treating stale-base success evidence as valid for a new merge result.

The replacement branch began from:

`f58aaca1d1d7a3fef68cb30b8b9a68fa0f94bf09`

and is not behind `main` at the point this handoff was written.

---

## 2. Cross-platform package baseline

Commit:

`12bdd17851316bdc0c395cc639cae8fbd96ad1e8` — `build: add Avalonia package baseline`

Added centrally managed Avalonia packages at `12.1.1`:

- `Avalonia`;
- `Avalonia.Desktop`;
- `Avalonia.Browser`;
- `Avalonia.Themes.Fluent`;
- `Avalonia.Fonts.Inter`.

Existing MAUI and data/security package baselines were retained.

---

## 3. Shared Avalonia presentation host

Commit:

`ca5de04c601791c03d7ab39ccde3991fd7bc2d27` — `feat: add shared Avalonia presentation host`

Added:

- `src/CareNest.CrossPlatform/CareNest.CrossPlatform.csproj`;
- `src/CareNest.CrossPlatform/App.axaml`;
- `src/CareNest.CrossPlatform/App.axaml.cs`;
- `src/CareNest.CrossPlatform/Views/MainView.axaml`;
- `src/CareNest.CrossPlatform/Views/MainView.axaml.cs`.

The host explicitly supports Avalonia classic-desktop and single-view lifetimes. The landing surface describes configured platform reach without claiming production feature parity.

---

## 4. Linux-capable desktop host

Commit:

`9aff88f6f2e1445337c236bc2f3fc9faf5102eee` — `feat: add Avalonia desktop host`

Added:

- `src/CareNest.CrossPlatform.Desktop/CareNest.CrossPlatform.Desktop.csproj`;
- `src/CareNest.CrossPlatform.Desktop/Program.cs`.

The desktop entry point uses Avalonia platform detection and classic desktop lifetime startup. It provides the Linux desktop build path while remaining capable of native Avalonia desktop execution on supported Windows/macOS environments.

---

## 5. WebAssembly/browser host

Commit:

`0820343d5f677438d8cf7a6174b75a40a7858a5c` — `feat: add Avalonia WebAssembly browser host`

Added:

- `src/CareNest.CrossPlatform.Browser/CareNest.CrossPlatform.Browser.csproj`;
- `src/CareNest.CrossPlatform.Browser/Program.cs`;
- `src/CareNest.CrossPlatform.Browser/wwwroot/index.html`;
- `src/CareNest.CrossPlatform.Browser/wwwroot/app.css`;
- `src/CareNest.CrossPlatform.Browser/wwwroot/main.js`.

The browser host targets `net10.0-browser` through `Microsoft.NET.Sdk.WebAssembly` and starts the shared Avalonia application in the browser output element.

---

## 6. Solution registration

Commit:

`27897cd658c5197d13665c5ff5205687a125f679` — `build: register cross-platform hosts in solution`

`CareNest.sln` now registers:

- `CareNest.CrossPlatform`;
- `CareNest.CrossPlatform.Desktop`;
- `CareNest.CrossPlatform.Browser`.

Debug and Release solution configurations are present for all three projects.

---

## 7. Fail-closed cross-platform configuration verifier

Initial commit:

`062ebe35f5cded2eeb0914246ae045e2750cdff3` — `test: add fail-closed cross-platform target verifier`

Testability hardening:

`58034afea2a811aa07851e15dc17a7e7ee9e92fe` — `test: make cross-platform verifier fixture-testable`

Added:

`build/scripts/verify-cross-platform-targets.py`

The verifier fails when required platform projects, target-framework declarations, package references, host entry-point wiring, solution registration, CI/dependency/release integration, or required Avalonia XAML are missing or malformed.

The verifier now accepts `--root` so isolated regression fixtures can be checked without mutating the live checkout.

---

## 8. Verifier regression self-tests

Commit:

`ed566e81e47c72587c2fb2f1b3354c1b30911f4c` — `test: cover cross-platform verifier failure modes`

Added:

`build/scripts/test-verify-cross-platform-targets.py`

The self-test:

1. copies only the required verification fixture files;
2. requires a valid fixture to pass;
3. removes required desktop startup wiring and requires a fail-closed result with the missing token identified;
4. injects malformed Avalonia XAML and requires a fail-closed XML/XAML error.

This is additional regression protection beyond the superseded PR #83 implementation.

---

## 9. CI integration

Initial cross-platform CI commit:

`20ba47243148356ea9d6611e223cd9ac372d2994` — `ci: build Linux and browser hosts`

Verifier-self-test integration:

`3729bdded3d61eb00ade53715ac11c09ae4fe52f` — `ci: self-test cross-platform target verification`

CareNest CI now includes:

- Python syntax validation for cross-platform verifier tooling;
- direct cross-platform target verification;
- verifier regression self-tests;
- platform-neutral formatting for the shared Avalonia and desktop projects;
- Linux desktop Release build;
- WebAssembly workload installation and browser Release publish;
- all pre-existing unit, integration, UI/source-policy, Android, Windows and Apple jobs.

---

## 10. Dependency audit integration

Commit:

`05e5bd0ae2154ef93cc2e0897899fecbea7a11af` — `ci: audit Avalonia desktop and browser dependencies`

Dependency Audit now restores with unsuppressed NuGet audit for:

- the existing platform-neutral tests;
- Avalonia desktop;
- Avalonia browser/WebAssembly;
- the existing MAUI application dependency graph.

---

## 11. Release-gate merge with PR #82 governance

Current-main merge commit:

`1ef827f7d38d1a19897efcc2cd1168dd82e501ae` — `ci: preserve production evidence and gate cross-platform hosts`

Verifier-self-test release commit:

`a334eb278eedc9f4f78943f43a3982cca24b5404` — `ci: require verifier self-tests in release gate`

This was the only file-level overlap between PR #82 and the old cross-platform branch.

The reconstructed release gate retains all PR #82 production-evidence requirements, including:

- production validation evidence standard/index;
- Android/Windows/iOS/Mac Catalyst validation templates;
- accessibility validation template;
- packaged compatibility template;
- signing provenance template;
- store submission template;
- final production approval template.

It additionally requires:

- `docs/setup/CROSS_PLATFORM.md`;
- the cross-platform verifier;
- the verifier self-test;
- direct target verification during release source tests;
- verifier self-tests during release source tests;
- Linux desktop Release build;
- WebAssembly browser Release publish.

No production-evidence protection from merged PR #82 was intentionally discarded.

---

## 12. Cross-platform setup and architecture guide

Commit:

`1f5f206f5f222cdce2c43a2fcda33916408f1131` — `docs: add current cross-platform build guide`

Added:

`docs/setup/CROSS_PLATFORM.md`

The guide covers:

- Android, iOS/iPadOS, Mac Catalyst and Windows MAUI targets;
- Linux desktop through Avalonia Desktop;
- modern WebAssembly-capable browsers through Avalonia Browser;
- Linux restore/build/run/publish commands;
- browser WebAssembly workload/build/publish commands;
- existing MAUI build commands;
- presentation-host dependency direction;
- browser/native capability boundaries;
- CI verification semantics;
- the distinction between configured build reach and production/manual feature-parity evidence.

---

## 13. Previous handoff preserved

Commit:

`49bb93bfdd5b90f128dbde5a95b0b594c6723305` — `docs: archive production evidence handoff before cross-platform continuation`

The previous root `what_changed.md` blob was reused exactly at:

`docs/history/cross-platform-before-current-main-20260823/what_changed.md`

so this active handoff can move forward without deleting the earlier production-evidence history.

---

## 14. Current configured platform reach

The source now configures these build targets/hosts on PR #84:

- Android — .NET MAUI `net10.0-android`;
- iOS/iPadOS — .NET MAUI `net10.0-ios`;
- macOS — .NET MAUI Mac Catalyst `net10.0-maccatalyst`;
- Windows — .NET MAUI `net10.0-windows10.0.19041.0`;
- Linux desktop — Avalonia Desktop `net10.0` host;
- modern WebAssembly-capable browsers — Avalonia Browser `net10.0-browser` host.

Configured build reach must not be interpreted as complete production feature parity. Native notifications, secure storage, file/camera behavior, background execution, accessibility, packaging/signing and browser sandbox capabilities remain host/platform-specific evidence obligations.

---

## 15. Verification boundary

PR #84 was opened from current `main` after the reconstructed branch reached 14 commits and 20 changed files.

At PR creation, the exact head was:

`a334eb278eedc9f4f78943f43a3982cca24b5404`

GitHub workflows were queued/in progress for that head. Those runs are not claimed as success until they actually complete successfully.

Additional handoff/documentation commits after that head invalidate any older-head result as final PR evidence. The final exact head must complete its own required workflow set.

Required final verification includes at least:

- CareNest CI, including Linux desktop and browser jobs;
- Dependency Audit, including Avalonia desktop/browser dependency graphs;
- CodeQL;
- Store Package Configuration;
- Store Inspection Artifacts.

A green automated matrix still does not replace real production device/browser/accessibility/signing/store evidence.

---

## 16. Superseded PR #83

PR #83 remains historical implementation work from the stale/diverged base and should not be merged after PR #84 replaces it.

Its useful implementation content has been reconstructed on current `main`, with additional verifier self-test hardening and the PR #82 production-evidence release-gate changes preserved.

The preferred merge path is PR #84 after exact-head verification succeeds.
