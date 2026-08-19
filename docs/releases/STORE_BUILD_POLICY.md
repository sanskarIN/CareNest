# CareNest Store Build Policy

**Release line:** `1.0.0-rc.1`  
**Current automated evidence authority:** `docs/releases/AUTOMATED_BASELINE.md`  
**Production evidence standard:** `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`  
**Production evidence index:** `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`

This document defines the stable source/package boundary for store-oriented builds. It is not evidence of production signing or store approval.

Do not pin a moving accepted source SHA or test total here. Current exact-source automation is owned by `docs/releases/AUTOMATED_BASELINE.md`.

## 1. Current external-commerce boundary

The distributed CareNest application runtime/source/package intentionally contains **no external Gumroad or Buy Me a Coffee destination/card/command/promotional artwork**.

Repository-only destinations:

- Gumroad: `https://ramsandesh.gumroad.com`;
- Buy Me a Coffee: `https://buymeacoffee.com/sanskarIN`.

Repository promotion/support does not unlock health functionality, reminder priority/reliability, medical advice, diagnosis, dosage decisions, treatment recommendations, emergency assistance, clinical services, accounts/cloud functionality or access to local health records.

CareNest does not automatically transmit local health records to either external destination.

## 2. No current commerce/funding build toggle

The old `CareNestShowFundingLink` / store-funding visibility architecture is removed.

Store builds do not require a special Gumroad/funding-disabled property because both external destinations are absent from application source/package by current product policy for every target.

Historical release evidence may describe the earlier funding-toggle investigation but must not be treated as current configuration.

## 3. Why package scanning remains mandatory

Source/build rules alone can miss payload content. The current defense-in-depth invariant is:

- external-commerce runtime surfaces absent by source policy;
- built payload scanned for repository-only external-commerce markers before inspection artifact upload;
- final signed packages scanned again/equivalently inspected before production promotion;
- final package scan/hash/provenance results captured in structured package evidence JSON.

The scanner and production evidence path must fail closed.

## 4. Default forbidden package markers

`build/scripts/verify-store-safe-payload.py` defaults to:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

The scanner covers UTF-8/UTF-16 marker encodings, regular files and ZIP-compatible package entries such as Android AABs according to current source tooling.

Normal CareNest package verification should retain both default markers.

## 5. Repository-only Gumroad branding

The repository promotional badge is:

`docs/assets/gumroad_store_badge.svg`

It is documentation/marketing material and must not be copied into `src/CareNest.App/Resources/Images/` or another packaged application resource path under the current policy.

Repository placement rules are documented in:

- `GUMROAD.md`;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`.

## 6. Store-candidate configuration targets

Store Package Configuration verifies the configured Release targets for:

- Android;
- Windows;
- iOS simulator;
- Mac Catalyst.

These builds exercise current project configuration and strict XAML compilation. They do not create production-signed store packages.

## 7. Store-package preflight

Store-package wrappers require an explicit supported target and delegate to standard release preflight.

Examples:

```bash
CARENEST_TARGET=net10.0-android ./build/scripts/store-package-preflight.sh
```

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
./build/scripts/store-package-preflight.ps1
```

The current wrapper does not accept/use a Gumroad or funding-link property.

## 8. Store Inspection Artifacts

The inspection workflow generates non-production exact-source evidence.

Configured jobs may produce unsigned/unpackaged/simulator inspection artifacts by design. Before staging/upload, the applicable payload is scanned and provenance/checksum evidence is produced according to the workflow.

Production signing secrets are intentionally absent from these internal inspection jobs.

## 9. Internal artifact boundary

Inspection artifacts are engineering evidence only and must not be described as:

- production signed;
- notarized;
- store submitted;
- store approved;
- production installable for every target.

Final production packages require separate signing/provenance/smoke/manual validation and structured final-package evidence.

## 10. Current automated evidence

Read the current accepted exact-source automation from:

`docs/releases/AUTOMATED_BASELINE.md`

If store/build/workflow/stable-policy source changes after that exact boundary, run fresh exact-source verification according to `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

Do not copy an older test total onto a newer source.

## 11. Strict XAML behavior

Store-candidate and inspection builds must preserve:

- Source binding compilation enabled;
- strict XAML compilation enabled;
- `XC0022`–`XC0025` as errors;
- accurate typed binding contexts;
- no warning/type-safety bypass merely to make a store build pass.

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

Pre-submission review:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

The dated review is not store approval. At actual submission:

- re-open current Apple rules for the exact package/listing where applicable;
- re-open current Google Play rules for the exact package/listing where applicable;
- re-open current Microsoft/Windows requirements where applicable;
- review current external-commerce/support rules;
- complete live health/privacy/data-safety/store declarations against the exact production binary;
- record date/source/conclusion;
- route required source/package changes through fresh review and verification.

Use `docs/releases/templates/STORE_SUBMISSION_RECORD.md` for actual submission/review/approval/publication evidence.

## 14. Production signing

Production signing remains outside Git and outside internal inspection workflows.

For final packages record exact source SHA/tag, identity/version, filename, SHA-256 and non-secret signing/notarization/store provenance.

Never place private signing material, passwords, service credentials, access tokens or recovery codes in repository evidence or package-evidence notes.

Use `docs/releases/templates/SIGNING_PROVENANCE_RECORD.md`.

## 15. Structured final-package evidence

Source-controlled package evidence tooling:

- `build/scripts/create-package-evidence.py`;
- `build/scripts/create-package-evidence.sh`;
- `build/scripts/create-package-evidence.ps1`.

For a final production artifact use `--stage production` according to:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

Production mode must enforce the exact tag/source/HEAD/workspace/provenance/store-safe conditions documented by that guide.

The resulting evidence records SHA-256/provenance and store-safe scan state. The tool does not sign the package or prove store approval.

## 16. Final signed-package external-commerce inspection

For every final signed/notarized/store candidate:

- scan/equivalently inspect for `buymeacoffee.com/sanskarIN`;
- scan/equivalently inspect for `ramsandesh.gumroad.com`;
- manually verify installed app contains no Gumroad/BMC promotional card/action/artwork;
- verify no health feature changes based on purchase/funding state;
- retain package evidence JSON, independently checked SHA-256, signing/notarization/store provenance and installed-package smoke/manual evidence.

## 17. Production evidence semantics

Use `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` and release-specific records linked by `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`.

A configured store build or inspection artifact is not a substitute for real package/device/accessibility/signing/store evidence.

## 18. Change policy

Do not reintroduce an application Gumroad link, payment SDK, funding link, external support card or promotional storefront asset as a routine store-specific switch.

Any future in-app external-commerce surface requires fresh product, privacy, security, medical-safety, UX, current store-policy and release review plus source/package regression tests and exact-source verification.
