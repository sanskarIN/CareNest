# CareNest 2.18.13 — Draft Release Notes

**Status:** DRAFT — NOT PUBLISHED  
**Prepared:** 2026-08-25  
**Target:** `2.18.13` / build `21813`  
**MAUI Controls:** `10.0.100`

These notes describe the source being prepared for CareNest `2.18.13`. They are not final publication notes and must be reconciled with the exact approved tag/package and actual production evidence before release.

## Highlights

### Maintenance roll-forward from the verified 2.18.12 baseline

CareNest `2.18.13` starts from `main` commit `b2db4821047dbfb7fe223961fc237afcdfc8371e`, which includes the verified `2.18.12` cross-platform baseline and the exact-head-green post-merge governance work from PR #86.

The active source metadata is rolled forward to:

- semantic/display version `2.18.13`;
- assembly/file version `2.18.13.0`;
- package/build code `21813`.

### Release-version drift protection

The version-consistency source-policy test now targets the `2.18.13` release package. It verifies central assembly metadata, MAUI display/build metadata, the `Microsoft.Maui.Controls` `10.0.100` baseline, and the required non-published version-specific preparation documents.

### Evidence remains exact-source and fail-closed

The successful workflows from the earlier `2.18.12` and PR #86 sources are retained as historical evidence only. The final `2.18.13` branch head must complete its own CI, CodeQL, dependency-audit, store-configuration and store-inspection workflow matrix before merge/promotion.

No source-preparation commit is treated as real-device, signing, accessibility, store-approval or publication evidence.

## Platform boundary

The configured presentation/build reach remains:

- Android, iOS/iPadOS, Mac Catalyst and Windows through .NET MAUI;
- Linux-capable desktop through the Avalonia desktop host;
- modern WebAssembly-capable browsers through the Avalonia browser host.

Linux/browser build support does not imply full runtime feature parity with the established MAUI application. Platform-specific capabilities remain evidence-driven.

## Dependency baseline

The current central dependency baseline retains:

- `Microsoft.Maui.Controls` `10.0.100`;
- Avalonia `12.1.1` package family.

No dependency success claim is made for the final `2.18.13` source until its exact-head audit/build matrix completes.

## Security, privacy and product boundary

This maintenance line does not add diagnosis, dosage calculation, treatment recommendation, clinical-risk scoring, emergency-service behavior, or hidden health-data upload.

CareNest remains local-first within its documented scope. Public validation evidence must use fictional/synthetic application data and must not expose health records, prescription documents, PINs, backup passwords, private signing keys, access tokens, recovery codes or other secrets.

## Still required before publication

The following remain external/manual production requirements rather than source-complete claims:

- representative Android installed-device validation;
- Windows installed-package/update validation;
- signed/provisioned iPhone/iPad validation;
- installed Mac Catalyst validation;
- representative Linux runtime validation;
- hosted browser/WebAssembly runtime validation;
- accessibility validation with applicable assistive technologies;
- packaged compatibility for SQLite, encrypted documents and backups;
- production signing/provisioning/notarization provenance;
- exact final package/deployment hashes and provenance;
- store-safe final payload inspection;
- live distribution metadata/declaration review;
- actual submission, review, approval and publication/deployment outcomes.

## Before publication

Do not publish these notes as final until the exact `v2.18.13` source/package/deployment has the required automated and production evidence and the release checklist permits promotion.
