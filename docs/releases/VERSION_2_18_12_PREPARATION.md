# CareNest 2.18.12 Release Preparation

**Prepared:** 2026-08-24  
**Target version:** `2.18.12`  
**Package/build code:** `21812`  
**State:** PREPARED IN SOURCE — NOT PUBLISHED  
**Repository:** `https://github.com/sanskarIN/CareNest`

This document records source preparation for CareNest `2.18.12`. It is not store approval, publication evidence, production-signing evidence, real-device validation, or proof of platform feature parity.

## Version-specific release package

Use these version-specific documents together:

- `VERSION_2_18_12_PREPARATION.md` — source/version boundary and promotion rules;
- `RELEASE_NOTES_2_18_12_DRAFT.md` — publication draft that remains non-final;
- `RELEASE_CHECKLIST_2_18_12.md` — exact-head, production-evidence, signing and store checklist.

The stable release authorities remain `RELEASE_CHECKLIST.md`, `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`, `PRODUCTION_EVIDENCE_INDEX.md` and the canonical templates. Version-specific documents supplement them rather than replacing them.

## Source version baseline

The release target is defined centrally in `Directory.Build.props`:

- `Version`: `2.18.12`;
- `AssemblyVersion`: `2.18.12.0`;
- `FileVersion`: `2.18.12.0`;
- `InformationalVersion`: `2.18.12`.

The established .NET MAUI application in `src/CareNest.App/CareNest.App.csproj` uses:

- `ApplicationDisplayVersion`: `2.18.12`;
- `ApplicationVersion`: `21812`.

`tests/CareNest.UiTests/VersionConsistencyContractTests.cs` protects these values and the non-published version-document state from accidental drift.

## Configured platform scope

The current source is configured for:

- Android through .NET MAUI;
- iOS/iPadOS through .NET MAUI;
- Mac Catalyst through .NET MAUI;
- Windows through .NET MAUI;
- Linux desktop through the Avalonia desktop host;
- modern WebAssembly-capable browsers through the Avalonia browser host.

Configured build reach is not the same as verified production feature parity. Linux and browser capability boundaries remain explicitly documented in `docs/setup/CROSS_PLATFORM.md` and the production-evidence templates.

## Required automated acceptance before merge/release promotion

The exact final release-preparation head must complete the repository verification matrix without reusing success from an older source:

- CareNest CI formatting and version-consistency checks;
- unit tests;
- integration tests;
- UI/source-policy tests;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build;
- Linux desktop Release build;
- WebAssembly browser Release publish;
- CodeQL;
- unsuppressed Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts.

Queued, cancelled, skipped, failed, superseded, or older-head runs are not success evidence for a newer source.

## Production evidence still required

Before `2.18.12` can be represented as production released, actual evidence must be recorded for applicable targets, including:

- installed Android behavior and reminder/notification permission behavior;
- installed Windows package/update behavior;
- signed/provisioned iPhone/iPad validation;
- installed Mac Catalyst validation;
- Linux runtime behavior on representative distributions;
- browser runtime/storage/file/permission/reload/offline behavior;
- accessibility testing using applicable assistive technology;
- packaged existing-data/SQLite/encrypted-document/backup compatibility;
- production signing/provisioning/notarization provenance;
- final package SHA-256/provenance;
- store-safe payload inspection;
- live store metadata/declaration review;
- submission, review, approval and publication outcomes.

Canonical evidence rules remain in `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` and `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`.

## Safety and privacy boundary

Version preparation does not add diagnosis, dosage calculation, treatment recommendation, clinical-risk scoring or emergency-service behavior. Public validation evidence must use fictional/synthetic application data and must not expose health records, prescription documents, backup passwords, PINs, signing keys, access tokens, recovery codes or other secrets.

## Promotion rule

Do not describe CareNest `2.18.12` as published, store-approved, production-signed, fully feature-parity verified or globally bug-free until the corresponding real evidence exists for the exact package/source being promoted.
