# CareNest Dependency and Toolchain Baseline

**Release line:** `1.0.0-rc.1`  
**Baseline date:** 2026-08-18

This document is the current human-readable reference for centrally managed package versions and maintained GitHub Actions majors. Source configuration remains authoritative if this document and executable configuration ever disagree.

## Authoritative source files

Package versions:

- `Directory.Packages.props`

Build/analyzer policy:

- `Directory.Build.props`

GitHub Actions:

- `.github/workflows/ci.yml`
- `.github/workflows/codeql.yml`
- `.github/workflows/dependency-review.yml`
- `.github/workflows/store-package-verification.yml`
- `.github/workflows/store-inspection-artifacts.yml`
- `.github/workflows/release-gate.yml`
- `.github/workflows/release-evidence.yml`

## Current package baseline

| Package | Version | Purpose |
|---|---:|---|
| `Microsoft.Maui.Controls` | `10.0.90` | .NET MAUI UI/application framework |
| `sqlite-net-pcl` | `1.9.172` | SQLite object/persistence layer |
| `SQLitePCLRaw.bundle_green` | `2.1.11` | SQLitePCLRaw bundle integration |
| `SQLitePCLRaw.lib.e_sqlite3` | `3.53.3` | Maintained native SQLite library floor |
| `SQLitePCLRaw.lib.e_sqlite3.android` | `2.1.12` | Android SQLite native leaf where pinned |
| `SQLitePCLRaw.provider.e_sqlite3` | `2.1.12` | e_sqlite3 provider leaf |
| `SQLitePCLRaw.provider.sqlite3` | `2.1.12` | sqlite3 provider leaf |
| `SQLitePCLRaw.provider.dynamic_cdecl` | `2.1.12` | dynamic provider leaf |
| `Microsoft.Extensions.Logging.Debug` | `10.0.0` | Debug logging provider |
| `Microsoft.Extensions.Logging.Abstractions` | `10.0.0` | Logging abstractions |
| `Microsoft.Extensions.DependencyInjection.Abstractions` | `10.0.0` | Dependency-injection abstractions |
| `Microsoft.NET.Test.Sdk` | `18.9.0` | .NET test host/SDK |
| `xunit` | `2.9.3` | Unit/integration/UI contract test framework |
| `xunit.runner.visualstudio` | `4.0.0` | Visual Studio / `dotnet test` runner integration |
| `coverlet.collector` | `10.0.1` | Coverage data collector |

Central package management and transitive pinning are enabled in `Directory.Packages.props`.

The former exact SQLite advisory suppression remains removed. Do not weaken dependency audit policy merely to make a package update pass.

## Current GitHub Actions baseline

Current maintained majors used by CareNest workflows include:

- `actions/checkout@v7`;
- `actions/setup-dotnet@v6`;
- `github/codeql-action/init@v4`;
- `github/codeql-action/analyze@v4`;
- `actions/upload-artifact@v7` where workflow artifacts are uploaded.

Hosted GitHub runners are used by repository workflows. Any future self-hosted runner introduction must separately validate the runtime/runner minimums required by these action majors.

## Upgrade policy

A dependency/action update is not accepted only because a newer version exists.

For package changes:

1. update the centralized source configuration;
2. retain unsuppressed NuGet audit behavior;
3. run the applicable unit/integration/UI source-policy tests;
4. run platform builds when MAUI/platform dependencies are affected;
5. run Store Package Configuration and Store Inspection Artifacts when packaging behavior may change;
6. repeat packaged SQLite/encrypted-data compatibility evidence when persistence/provider changes require it;
7. update this baseline and other user-facing configuration references.

For GitHub Action changes:

1. update every applicable workflow consistently;
2. preserve least-required workflow permissions;
3. preserve fail-closed release/dependency/package gates;
4. validate pull-request automation on an exact frozen source;
5. do not treat an old green run from a different source as proof for the combined updated workflow set.

## Dependabot

Dependabot may propose individual updates. CareNest can integrate compatible updates directly into `main`, but the final accepted combination still requires the repository's exact-head verification matrix.

A Dependabot PR whose only failure came from an unrelated source defect on an older base is not automatically evidence that the dependency itself is broken. Conversely, a historically green isolated update does not prove that several combined updates are green together.

## Verification boundary

The package/action versions above are **candidate current source configuration** until the latest verification-relevant `main` head completes the exact-head matrix defined in:

- `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`;
- `docs/releases/AUTOMATED_BASELINE.md`.

Do not copy an older test count onto a newer package/toolchain source. Record the counts and workflow outcomes actually produced by the final verification run.

## External/manual release boundary

Even a fully green dependency/toolchain matrix does not prove:

- real notification delivery on representative devices;
- production signing/notarization;
- packaged existing-data compatibility;
- assistive-technology behavior;
- live store-console declarations;
- submission-date store-policy compliance;
- store approval/publication.

Those remain separate release evidence requirements.
