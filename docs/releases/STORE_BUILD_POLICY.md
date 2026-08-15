# Store Build Policy — External Project Funding

## Purpose

CareNest repository documentation may contain an optional Buy Me a Coffee link for voluntary project support. The CareNest application runtime/package does **not** contain or expose that external funding destination.

This is now a source-level product boundary rather than a per-package visibility switch. It avoids store-package drift and protects all app targets consistently.

## Product rule

Project support is voluntary and repository-only.

It must never:

- unlock medical advice;
- unlock health features;
- alter reminder priority, timing, reliability, or permissions;
- expose user health data;
- alter health-related support priority;
- create an account or remote health-data relationship;
- be represented as purchase of clinical, diagnostic, treatment, or emergency functionality.

The repository support URL is:

`https://buymeacoffee.com/sanskarIN`

It may appear in repository support documents and `.github/FUNDING.yml` where permitted. It must not appear under `src/CareNest.App` or in built CareNest application payloads.

## Final application boundary

There is no `CareNestShowFundingLink` build property and no `CARENEST_*FUNDING*` app build switch.

The application contains:

- no external funding command;
- no external funding card in About;
- no funding-policy source unit;
- no Buy Me a Coffee destination under the MAUI app source tree;
- no packaged funding/support promotional artwork carrying the destination.

CareNest About continues to expose normal product/support surfaces such as the repository, creator profile, business email, application support email, privacy policy, terms, security policy, and bundled third-party notices.

Health-organizer behavior is independent of repository funding.

## Why the previous build switch was removed

During 2026-08-15 package inspection, Windows store-safe publishing still contained `buymeacoffee.com/sanskarIN` in `CareNest.App.dll` even when the code/build funding switch evaluated false.

The root cause was `src/CareNest.App/Resources/Images/buy_me_a_coffee_carenest.svg`: the SVG itself contained the full destination in accessibility/text content, and Windows MAUI resource processing embedded that content into the managed application payload.

The final fix removed the runtime funding surface and the URL-bearing artwork instead of depending on compile flags. See `FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md`.

## Store-package preflight

Store-package preflight requires an explicit supported MAUI target and delegates to the standard release preflight.

Supported targets:

- `net10.0-android`;
- `net10.0-ios`;
- `net10.0-maccatalyst`;
- `net10.0-windows10.0.19041.0`.

Bash:

```bash
CARENEST_TARGET=net10.0-android ./build/scripts/store-package-preflight.sh
```

PowerShell:

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
./build/scripts/store-package-preflight.ps1
```

The wrappers preserve formatting, core builds, automated tests, unsuppressed dependency audit, target restore/audit, and selected MAUI Release build coverage.

They do not configure production signing, create store identities, submit an application, or prove installed real-device behavior.

## Automated store-candidate build verification

`.github/workflows/store-package-verification.yml` compiles store-candidate configurations for every supported MAUI target.

It covers:

- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release.

The workflow does not rely on a funding visibility property. The application source itself is funding-surface-free.

The workflow does not inject production signing credentials or publish production releases.

## Automated payload inspection

`.github/workflows/store-inspection-artifacts.yml` creates internal non-production inspection artifacts and runs:

`build/scripts/verify-store-safe-payload.py`

against built payloads before upload.

The scanner fails if the canonical BMC marker is found as UTF-8, UTF-16 little-endian, or UTF-16 big-endian text. It scans ordinary files/directories and ZIP/AAB entries and fails closed for missing/unreadable payloads.

It scans:

- the unsigned Android AAB candidate;
- the Windows self-contained publish directory;
- the iOS simulator `.app` bundle;
- the unsigned Mac Catalyst `.app` bundle.

Successful provenance records include:

`external_funding_surface=absent_by_source_policy`

and:

`funding_url_payload_scan=passed`

The workflow also self-tests the scanner with clean, UTF-8-marker, UTF-16-marker, ZIP/AAB-marker, and missing-path cases.

## Automated contracts

Current source-policy tests protect the final boundary:

- `StoreFundingPayloadContractTests.cs` recursively guards the MAUI app text/resource tree against the external funding destination and deleted funding surface;
- `CriticalFlowContractTests.cs` protects the funding-free About runtime while preserving product/support surfaces;
- `FundingLinkContractTests.cs` keeps voluntary funding repository-only and non-medical;
- `BrandingAndLocalizationContractTests.cs` and `PackageMetadataContractTests.cs` require core CareNest branding while requiring funding artwork to remain absent;
- `StorePackageWorkflowContractTests.cs` protects target coverage and the absence of the obsolete funding toggle;
- `StorePackagePreflightContractTests.cs` protects explicit target selection and release-preflight delegation without the obsolete funding property;
- `StoreInspectionArtifactWorkflowContractTests.cs` protects exact-source artifact generation, payload scanning, provenance, checksums, scanner self-test, and non-production boundaries.

## Store-review decision

The app binary no longer exposes the external voluntary funding destination, so there is no per-store `true/false` funding build choice.

Before each production submission, still review current Apple/Google/Microsoft storefront rules for the complete app/store listing, metadata, payments, privacy, data-safety declarations, and external links that remain in the product.

Do not infer future store approval from an earlier review or automated build result.

## Required packaged verification

For every final signed candidate:

- verify the About experience contains no Buy Me a Coffee funding destination/card;
- verify repository, creator, business email, application support email, privacy, terms, security, and third-party notices remain available as intended;
- verify health-organizer behavior is unchanged;
- run or equivalently reproduce the forbidden-marker payload scan on the signed package;
- record package filename, SHA-256, source SHA, target, version/build, signing/notarization provenance, scan result, manual reviewer and date.

A successful internal automated scan proves only that the configured marker was absent from the inspected payload. It does not prove store approval, final signed-package identity, real-device behavior, accessibility, data-upgrade compatibility, or production readiness by itself.

## Current evidence

The authoritative automated merged-source record is:

`docs/releases/FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md`

Frozen executable source:

`9ec7b4e7d2150d9cc50be19f30464080318b16e8`

Marker-only PR #68 verified all normal/store-candidate builds, all Android/Windows/Apple payload scans, 325/325 tests, CodeQL, and unsuppressed Dependency Audit, then closed without merge.
