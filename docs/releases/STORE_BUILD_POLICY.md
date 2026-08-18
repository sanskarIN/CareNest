# CareNest Store Build Policy

**Release line:** `1.0.0-rc.1`  
**Latest fully verified Gumroad implementation/source-policy baseline:** `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`

This document defines the current source/package boundary for store-oriented builds. It is not evidence of production signing or store approval.

## 1. Current external-commerce boundary

The distributed CareNest application runtime/source/package intentionally contains **no external Gumroad or Buy Me a Coffee destination/card/command/promotional artwork**.

Repository-only destinations:

- Gumroad: `https://ramsandesh.gumroad.com`;
- Buy Me a Coffee: `https://buymeacoffee.com/sanskarIN`.

Repository promotion/support does not unlock health functionality, reminder priority/reliability, medical advice, diagnosis, dosage decisions, treatment recommendations, emergency assistance, clinical services, accounts/cloud functionality, or access to local health records.

CareNest does not automatically transmit local health records to either external destination.

## 2. No current commerce/funding build toggle

The old `CareNestShowFundingLink` / store-funding visibility architecture is removed.

Store builds do not require a special Gumroad/funding-disabled property because both external destinations are absent from application source/package by product policy for every target.

Historical release evidence may describe the earlier funding-toggle investigation but must not be treated as current configuration.

## 3. Why package scanning remains mandatory

The 2026-08-15 investigation proved that source/build flags alone can miss payload content: a URL-bearing SVG resource caused an external funding marker to enter Windows application bytes.

The stronger current invariant is:

- external-commerce runtime surfaces absent by source policy;
- actual built payload scanned for repository-only external-commerce markers before inspection artifact upload;
- final signed packages scanned again/equivalently inspected before production promotion;
- final production package scan/hash/provenance results captured in structured package evidence JSON.

The scanner and production evidence path are defense-in-depth and must fail closed.

## 4. Default forbidden package markers

`build/scripts/verify-store-safe-payload.py` defaults to:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

The scanner checks:

- UTF-8 bytes;
- UTF-16 little-endian bytes;
- UTF-16 big-endian bytes;
- regular files;
- ZIP-compatible package entries such as Android AABs.

The repeatable `--forbidden` option may be used for explicit marker lists, but normal CareNest package verification should retain both default markers.

## 5. Repository-only Gumroad branding

The repository promotional badge is:

`docs/assets/gumroad_store_badge.svg`

It is documentation/marketing material and must not be copied into `src/CareNest.App/Resources/Images/` or another packaged application resource path under the current policy.

Repository placement rules are documented in:

- `GUMROAD.md`;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`.

## 6. Store-candidate configuration targets

Current Store Package Configuration verifies Release configurations for:

- Android;
- Windows;
- iOS simulator;
- Mac Catalyst.

These builds exercise current project configuration and strict XAML compilation. They do not create production-signed store packages.

## 7. Store-package preflight

Store-package wrappers require an explicit supported target and delegate to standard release preflight.

Examples:

```bash
CARENEST_TARGET=net10.0-android \
./build/scripts/store-package-preflight.sh
```

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
./build/scripts/store-package-preflight.ps1
```

The current wrapper does not accept/use a Gumroad or funding-link property.

## 8. Store Inspection Artifacts

The inspection workflow generates non-production exact-source evidence.

### Android

- unsigned AAB inspection candidate;
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

Production signing secrets are intentionally absent.

## 9. Internal artifact boundary

Inspection artifacts are engineering evidence only and may be unsigned, unpackaged or simulator-targeted by design.

They must not be described as:

- production signed;
- notarized;
- store submitted;
- store approved;
- production installable for every target.

Final production packages require separate signing/provenance/smoke/manual validation and structured final-package evidence.

## 10. Latest fully verified Gumroad rollout automated evidence

Exact verified implementation/source-policy source:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

Verified on that exact revision:

- unit tests: **122/122**;
- integration tests: **39/39**;
- UI/source-policy tests: **175/175**;
- total core tests: **336/336**;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build;
- Android store-candidate configuration;
- Windows store-candidate configuration;
- iOS simulator store-candidate configuration;
- Mac Catalyst store-candidate configuration;
- CodeQL.

Authoritative automated verification record:

`docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`

Current `main` now contains later verification-relevant release contracts, package-evidence tooling and workflow changes. Those newer changes require fresh exact-source automation before they can replace the verified baseline above.

Do not assign the 336-test result to the newer source or predict a replacement total before the exact run is recorded.

## 11. Strict XAML behavior

Store-candidate and inspection builds use:

- Source binding compilation enabled;
- strict XAML compilation enabled;
- `XC0022`–`XC0025` as errors.

Do not weaken XAML warning policy to make store builds pass.

## 12. Store privacy/medical boundary

Every candidate/listing must preserve:

- organizational/non-clinical positioning;
- no dosage calculation/inference;
- no diagnosis/treatment/interaction/risk claims;
- no guaranteed reminder delivery;
- no required CareNest account/backend in current v1;
- no whole-database encryption claim;
- explicit external export/share boundaries;
- no claim that Gumroad purchase or project funding changes health-app behavior.

## 13. Current policy review and submission-time re-check

A dated pre-submission review was completed on 2026-08-18 and is recorded at:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

That review covers current Apple App Review Guidelines, Google Play health-app/declaration/Data safety guidance and Microsoft Store sensitive-personal-information policy as they apply to the current CareNest boundary.

Store policy changes over time. At actual submission:

- re-open and review the current Apple rules for the exact package/listing;
- re-open and review the current Google Play rules for the exact package/listing;
- re-open and review Microsoft/Windows requirements where applicable;
- review current rules applicable to external commerce/support links;
- complete the live store-console health/privacy/data-safety declarations against the exact production binary;
- record date/source/conclusion;
- change listing/package only through an explicit reviewed source change followed by new verification.

The current policy keeps Gumroad and Buy Me a Coffee out of the submitted application package.

## 14. Production signing

Production signing remains outside Git and outside internal inspection workflows.

Final packages must record exact source SHA/tag, identity/version, filename, SHA-256 and signing/notarization/store provenance.

Never put private signing material, passwords, service credentials or account tokens into repository evidence or package-evidence notes.

## 15. Structured final-package evidence

Source-controlled package evidence tooling:

- `build/scripts/create-package-evidence.py`;
- `build/scripts/create-package-evidence.sh`;
- `build/scripts/create-package-evidence.ps1`.

For a final production artifact use `--stage production` according to:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

Production mode requires:

- immutable `v*` source tag;
- tag SHA equals recorded source SHA;
- checked-out HEAD equals recorded source SHA;
- clean tracked workspace;
- non-secret real signing/notarization/store-managed provenance text;
- successful default store-safe scan;
- output outside the payload.

The resulting JSON records per-file SHA-256, top-level package/directory payload SHA-256 and the store-safe scanner result.

The tool does not sign the package or prove store approval; independent signing/notarization/store provenance and manual validation remain required.

## 16. Final signed-package external-commerce inspection

Even though source policy removes external-commerce surfaces, final signed packages must repeat/equivalently perform forbidden-marker scans for both:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

Also manually verify that the installed app contains no Gumroad/Buy Me a Coffee promotional card/action/artwork and that no health feature changes based on purchase/funding state.

For each final package retain:

- structured package evidence JSON;
- independently checked package SHA-256;
- signing/notarization/store provenance;
- installed-package smoke/manual evidence.

This protects against packaging/tooling/regression differences after internal inspection.

## 17. Change policy

Do not reintroduce an application Gumroad link, payment SDK, funding link, external support card or promotional storefront asset as a routine store-specific switch.

Any future in-app external-commerce surface requires fresh product, privacy, security, medical-safety, UX, current store-policy and release review plus source/package regression tests and exact-source verification.
