# CareNest 2.18.12 — Draft Release Notes

**Status:** DRAFT — NOT PUBLISHED  
**Prepared:** 2026-08-24  
**Target:** `2.18.12` / build `21812`

These notes describe the source prepared for CareNest `2.18.12`. Final notes must be reconciled with the exact tagged source and actual production evidence before publication.

## Highlights

### Expanded cross-platform foundation

CareNest now has configured presentation/build reach for:

- Android;
- iOS/iPadOS;
- Mac Catalyst;
- Windows;
- Linux desktop;
- modern WebAssembly-capable browsers.

The established Android, iOS/iPadOS, Mac Catalyst and Windows application remains .NET MAUI based. Linux desktop and browser reach is provided by Avalonia presentation hosts.

Linux/browser build support must not be interpreted as automatic full feature parity with the established MAUI application. Platform-specific capabilities remain subject to explicit runtime evidence.

### Stronger cross-platform verification

The source adds fail-closed checks for:

- required MAUI target declarations;
- Avalonia package and project registration;
- desktop/browser startup wiring;
- well-formed Avalonia XAML;
- Linux/browser CI integration;
- dependency-audit integration;
- release-gate integration;
- production-evidence template presence and unperformed defaults.

The verifier has isolated regression self-tests covering intentionally broken startup wiring, malformed XAML and unsafe evidence-state changes.

### Linux and browser production-evidence boundaries

Canonical validation records now exist for Linux desktop and browser/WebAssembly behavior. They intentionally begin `NOT RUN` and separate build/publish success from real runtime, persistence, notification, secure-storage, filesystem, browser-sandbox, accessibility and parity evidence.

### Version consistency protection

CareNest `2.18.12` is defined centrally and protected by automated source-policy tests. The MAUI package metadata uses display version `2.18.12` and package/build code `21812`.

### .NET MAUI servicing update

The central package baseline now uses `Microsoft.Maui.Controls` `10.0.100`, replacing `10.0.90`. The standalone Dependabot PR for this update had a successful CI/security/store-check matrix on its own source before integration. The dependency is still subject to the complete final PR #84 exact-head matrix together with the cross-platform and 2.18.12 changes.

### CI formatting correction

The previous PR head exposed three missing final-newline formatting errors in source-policy test files. Those formatting defects were corrected without weakening the formatter or skipping tests.

## Security, privacy and product boundary

This release preparation does not add diagnosis, dosage calculation, treatment recommendation, clinical-risk scoring, emergency-service behavior or hidden cloud upload.

CareNest remains local-first within the documented product scope. Production evidence must use fictional/synthetic application data and must not expose health records, prescription documents, PINs, backup passwords, private signing keys, access tokens or recovery codes.

## Before publication

These draft notes must not be published as final until the exact `v2.18.12` source/package has the required automated and production evidence, including applicable real-device/runtime validation, accessibility, package compatibility, signing/provenance, store-safe inspection, live store declarations and actual submission/approval/publication outcomes.
