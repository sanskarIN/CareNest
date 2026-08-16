# CareNest Store Build Policy

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

This document defines the current source/package boundary for store-oriented builds. It is not evidence of production signing or store approval.

## 1. Current application funding boundary

The distributed CareNest application runtime/source/package contains **no external Buy Me a Coffee destination/card/command/artwork**.

Repository-only voluntary support destination:

`https://buymeacoffee.com/sanskarIN`

Project support does not unlock health functionality, reminder priority/reliability, medical advice, emergency service or access to local records.

## 2. No current funding build toggle

The old `CareNestShowFundingLink` / store-funding visibility architecture is removed.

Store builds do not require a special funding-disabled property because the external destination is absent from application source/package by product policy for every target.

Historical release evidence can describe the earlier toggle investigation but must not be treated as current configuration.

## 3. Why package scanning remains

The 2026-08-15 investigation proved that source/build flags alone can miss payload content: a URL-bearing SVG resource caused the external funding marker to enter Windows application bytes.

The current stronger invariant is:

- funding surface absent by source policy;
- actual built payload scanned for the canonical external marker before inspection artifact upload.

The scanner is defense-in-depth and must fail closed.

## 4. Store-candidate configuration targets

Current Store Package Configuration verifies Release configurations for:

- Android;
- Windows;
- iOS simulator;
- Mac Catalyst.

These builds exercise current project configuration and strict XAML compilation. They do not create production-signed store packages.

## 5. Store-package preflight

Store-package wrapper scripts require an explicit supported target and delegate to standard release preflight.

Examples:

```bash
CARENEST_TARGET=net10.0-android \
./build/scripts/store-package-preflight.sh
```

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
./build/scripts/store-package-preflight.ps1
```

The current wrapper does not accept/use a funding-link property.

## 6. Store Inspection Artifacts

The inspection workflow generates non-production exact-source evidence:

### Android

- unsigned AAB inspection candidate;
- excludes/rejects signed companion metadata as configured;
- payload scan before staging;
- checksum/provenance;
- artifact upload.

### Windows

- self-contained unpackaged inspection output;
- payload scan before staging;
- checksum/provenance;
- artifact upload.

### Apple

- iOS simulator inspection build;
- unsigned Mac Catalyst inspection publish;
- payload scan/staging/checksums/provenance;
- artifact upload.

Production signing secrets are intentionally absent from this workflow.

## 7. Internal artifact boundary

Inspection artifacts are engineering evidence only and can be unsigned, unpackaged or simulator-targeted by design.

They must not be described as:

- production signed;
- notarized;
- store submitted;
- store approved;
- production installable for every target.

Final production packages require separate signing/provenance/smoke/manual validation.

## 8. Current automated evidence

PR #74 frozen source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Verified:

- CareNest CI #735 / `31938301209`: success;
- 331/331 core tests;
- Android/Windows/iOS simulator/Mac Catalyst Release builds: success;
- Store Package Configuration #124 / `31938301146`: all four targets success;
- Store Inspection Artifacts #47 / `31938301275`: success;
- CodeQL #735 / `31938301252`: success;
- Dependency Audit #91 / `31938301172`: success.

Permanent evidence: `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

Older PR #68/#67/#61/#59/#58 evidence remains historical for earlier boundaries.

## 9. Strict XAML behavior

Store-candidate and inspection builds use the current app project policy:

- Source binding compilation enabled;
- strict XAML compilation enabled;
- `XC0022`–`XC0025` as errors.

Do not weaken XAML warning policy to make store builds pass.

## 10. Store privacy/medical boundary

Every candidate/listing must preserve:

- organizational/non-clinical positioning;
- no dosage calculation/inference;
- no treatment/interaction/risk claims;
- no guaranteed reminder delivery;
- no required CareNest account/backend in current v1;
- no whole-database encryption claim;
- explicit external export/share boundaries.

## 11. Submission-time policy review

Store policy changes over time. At actual submission:

- review current Apple rules for the exact package/listing;
- review current Google Play rules for the exact package/listing;
- review Microsoft/Windows requirements where applicable;
- record date/source/conclusion;
- adjust listing/package only through an explicit reviewed source change, followed by new verification.

## 12. Production signing

Production signing remains outside Git and outside internal inspection workflows.

Final packages must record exact source SHA/tag, identity/version, filename, SHA-256 and signing/notarization/store provenance.

## 13. Final signed-package funding inspection

Even though source policy removes the external funding surface, final signed packages must repeat/equivalently perform the forbidden-marker scan and manually verify About contains no BMC funding destination/card.

This protects against packaging/tooling/regression differences after internal inspection.

## 14. Change policy

Do not reintroduce an application funding link, payment SDK or external support card as a routine store-specific switch.

Any future in-app external funding/payment surface requires fresh product, privacy, security, UX and current store-policy review plus source/package tests and exact-source verification.