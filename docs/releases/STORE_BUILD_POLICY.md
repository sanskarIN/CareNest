# Store Build Policy — Voluntary Project Support

## Purpose

CareNest is open source and may display an optional Buy Me a Coffee link for voluntary project support. Store policies can change, so release engineering must be able to hide that external support surface for a specific packaged build without forking the application or changing any health-organizer functionality.

This document defines the source-controlled build switch, compiled-payload funding boundary, automated store-safe compilation/inspection paths, local fail-closed preflight wrappers, and the evidence required before a store submission.

## Product rule

Project support is voluntary.

It must never:

- unlock medical advice;
- unlock health features;
- alter reminder priority, timing, reliability, or permissions;
- expose user health data;
- alter support priority for health-related use;
- create an account or remote health-data relationship;
- be represented as a purchase of clinical, diagnostic, treatment, or emergency functionality.

The canonical support URL remains:

`https://buymeacoffee.com/sanskarIN`

Repository funding metadata may continue to reference the same URL where GitHub permits it.

## Build switch and compiled URL boundary

`src/CareNest.App/CareNest.App.csproj` defines:

`CareNestShowFundingLink`

Default:

`true`

When the value is `true`, the project defines `CARENEST_FUNDING_LINK` and the About page exposes the voluntary support card.

When the value is `false`, that compile symbol is absent and `AboutViewModel.IsProjectSupportVisible` returns `false`, hiding the complete support card. The disabled command also has a false `CanExecute` predicate.

The canonical BMC URL is intentionally **not** stored in `CareNest.Shared.AppConstants`. It exists only inside the `CARENEST_FUNDING_LINK` compile branch in `AboutViewModel`, so a funding-disabled app build does not inherit the URL merely because a shared assembly always contains it.

This changes only the voluntary external support surface. It must not change data, reminders, permissions, reports, encryption, backups, app lock, appointments, profiles, documents, or medical-safety wording.

## Direct build examples

Android build with the support surface visible:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android \
  -c Release \
  -p:CareNestTargetFramework=net10.0-android \
  -p:CareNestShowFundingLink=true \
  --nologo
```

Android build with the support surface hidden:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android \
  -c Release \
  -p:CareNestTargetFramework=net10.0-android \
  -p:CareNestShowFundingLink=false \
  --nologo
```

The same property applies to iOS, Mac Catalyst, and Windows targets.

## General release-preflight examples

Bash:

```bash
CARENEST_TARGET=net10.0-android \
CARENEST_SHOW_FUNDING_LINK=false \
./build/scripts/release-preflight.sh
```

PowerShell:

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
$env:CARENEST_SHOW_FUNDING_LINK = 'false'
./build/scripts/release-preflight.ps1
```

Accepted funding-link values are exactly `true` and `false`. The general release-preflight scripts fail closed for any other value.

## Fail-closed store-package preflight

For store candidates where the external support surface must be disabled, prefer the dedicated wrappers. They require an explicit supported target and force `CARENEST_SHOW_FUNDING_LINK=false`; a caller cannot override the wrapper back to `true`.

Supported targets:

- `net10.0-android`;
- `net10.0-ios`;
- `net10.0-maccatalyst`;
- `net10.0-windows10.0.19041.0`.

Bash example:

```bash
CARENEST_TARGET=net10.0-android \
./build/scripts/store-package-preflight.sh
```

PowerShell example:

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
./build/scripts/store-package-preflight.ps1
```

The wrappers delegate to the existing release preflight, so formatting, core builds, automated tests, unsuppressed dependency audit, target dependency restore, and the selected MAUI Release build remain governed by one underlying preflight implementation.

These wrappers compile/test a source configuration. They do not configure signing, submit an app, create a production identity, or prove behavior on an installed store artifact.

## Automated store-safe build verification

`.github/workflows/store-package-verification.yml` continuously compiles the funding-disabled configuration for every supported MAUI target.

It runs on:

- pull requests to `main`;
- pushes to `main` and `release/**`;
- exact production-style `v*` tags;
- manual `workflow_dispatch` runs.

The workflow sets:

`CARENEST_STORE_FUNDING_LINK=false`

and passes that value into:

`CareNestShowFundingLink`

for:

- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release.

The iOS job intentionally uses the simulator runtime for unsigned source compilation. The workflow does not upload unsigned binaries, configure signing credentials, or publish release artifacts.

The standard CareNest CI continues to compile the normal/default configuration. Together, the two workflows provide automated compilation coverage for both the normal source configuration and the store-safe funding-disabled configuration.

## Automated store-safe payload inspection

`.github/workflows/store-inspection-artifacts.yml` creates internal, non-production inspection artifacts and runs:

`build/scripts/verify-store-safe-payload.py`

against the built payload before upload.

The scanner fails if the canonical BMC marker is found as UTF-8, UTF-16 little-endian, or UTF-16 big-endian text. It scans ordinary files/directories and ZIP/AAB entries. Missing/unreadable payloads fail closed rather than being treated as a clean result.

The workflow applies this scan to:

- the unsigned Android AAB candidate before staging;
- the Windows self-contained publish directory before ZIP creation;
- the iOS simulator `.app` bundle before tar creation;
- the unsigned Mac Catalyst `.app` bundle before tar creation.

Every successful inspection artifact records:

`funding_url_payload_scan=passed`

in its provenance file.

The same workflow contains a small scanner self-test proving that:

- a clean payload passes;
- a UTF-8 marker fails;
- a UTF-16 marker fails;
- a marker inside a ZIP/AAB entry fails;
- a missing payload path fails closed.

This is stronger than merely checking `CareNestShowFundingLink=false`, but it is still automated package-shape evidence—not store approval and not a substitute for installed UI inspection on the signed candidate.

## Store-review decision

Before each Apple App Store or Google Play production submission:

1. Review the current store rule that applies to external voluntary support/donation/project-funding links.
2. Record the rule review date, source, reviewer, and conclusion in release evidence.
3. If the in-app external support link is permitted, build with `CareNestShowFundingLink=true`.
4. If the link is not permitted or its status is uncertain, build with `CareNestShowFundingLink=false`.
5. Verify the resulting packaged build on the actual target/store configuration.
6. Record the selected property value with the package checksum and source commit.

Do not infer store approval from an earlier release. Policy review is a per-release external gate.

The dated 2026-08-15 review in `STORE_POLICY_REVIEW_20260815.md` currently selects `CareNestShowFundingLink=false` for the initial Apple App Store and Google Play candidates, subject to submission-time re-review.

## Required packaged verification

For a build with the link enabled:

- About shows the voluntary project-support card.
- The link opens only after explicit user action.
- The destination is the canonical `https://buymeacoffee.com/sanskarIN` URL.
- The surrounding text states that support is voluntary and unlocks no medical/health functionality.

For a build with the link disabled:

- About does not show the support image, button, URL, or explanatory support card.
- GitHub repository, creator, business email, support email, privacy, terms, security, and third-party-notice surfaces remain available.
- No health-organizer feature changes.
- Automated internal inspection should report `funding_url_payload_scan=passed` for the exact source candidate.

A successful automated payload scan proves that the forbidden marker was not found in the inspected unsigned/internal payload. It does not prove the final signed package is identical or that the installed UI/store submission is approved. Re-run or equivalent-inspect the final signed candidate and perform the manual About-page check.

## Automated contracts

`tests/CareNest.UiTests/CriticalFlowContractTests.cs` protects the build-configurable visibility contract.

`tests/CareNest.UiTests/PackageMetadataContractTests.cs` protects package identity, platform permission/privacy metadata, local-first Android network boundaries, and required branding assets.

`tests/CareNest.UiTests/StorePackageWorkflowContractTests.cs` protects the funding-disabled multi-platform workflow, target coverage, unsigned simulator behavior, and non-publication boundary.

`tests/CareNest.UiTests/StorePackagePreflightContractTests.cs` protects the fail-closed local wrappers, supported target allow-list, and forced funding-disabled setting.

`tests/CareNest.UiTests/StoreInspectionArtifactWorkflowContractTests.cs` protects exact-source artifact generation, unsigned/non-production boundaries, payload scanner execution/provenance, and scanner self-test coverage.

`tests/CareNest.UiTests/StoreFundingPayloadContractTests.cs` protects the compile-time URL boundary plus scanner encodings/ZIP behavior/fail-closed source contract.

`tests/CareNest.UiTests/ReleaseWorkflowContractTests.cs` requires the store-package and store-inspection workflows to remain part of exact `v*` release-tag/manual workflow coverage.

These tests reduce accidental source regressions but do not replace manual store-policy review, signing, final signed-package inspection, accessibility testing, or real-device behavior testing.

## Release evidence fields

Record at minimum:

- source commit SHA;
- release tag when applicable;
- target framework;
- package/application identifier;
- application display version/build number;
- `CareNestShowFundingLink` value;
- store-package configuration workflow run ID/conclusion where applicable;
- store-inspection workflow run ID/conclusion where applicable;
- `funding_url_payload_scan` result for funding-disabled candidates;
- store-policy review date and conclusion;
- package filename and SHA-256 checksum;
- signing/notarization provenance where applicable;
- manual About-page result;
- reviewer/date.
