# CareNest 2.18.13 Release Preparation

**Prepared:** 2026-08-26  
**Target version:** `2.18.13`  
**Package/build code:** `21813`  
**State:** SOURCE VERIFIED — NOT PUBLISHED  
**Repository:** `https://github.com/sanskarIN/CareNest`  
**Accepted exact source:** `323ea67281cbbdb3e7ca149fabd7135e8ad8a377`  
**Verification PR:** `#87`  
**Verification merge ref:** `421fead1e82c3d470ebf72417049d80b104c5516`  
**Merged `main`:** `b3efa71ab412da65f08d78ad1c4d913e302ff901`

This document records source preparation and observed automated verification for CareNest `2.18.13`. It does not claim store approval, publication, production signing, real-device validation, accessibility completion, or full Linux/browser feature parity.

## Why this patch line exists

CareNest `2.18.12` reached an accepted automated-source boundary, followed by release-governance work that was verified and merged through PR #86. The `2.18.13` line is a maintenance continuation from that verified repository state.

The patch rolls the active source version forward, protects release-line metadata with automated contracts, and keeps unresolved production evidence fail-closed.

## Version-specific release package

Use these version-specific documents together:

- `VERSION_2_18_13_PREPARATION.md` — source/version boundary and promotion rules;
- `RELEASE_NOTES_2_18_13_DRAFT.md` — publication draft that remains non-final;
- `RELEASE_CHECKLIST_2_18_13.md` — exact-source, production-evidence, signing and store checklist.

The stable authorities remain `RELEASE_CHECKLIST.md`, `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`, `PRODUCTION_EVIDENCE_INDEX.md` and the canonical validation templates.

## Source version baseline

The active release target is defined centrally in `Directory.Build.props`:

- `Version`: `2.18.13`;
- `AssemblyVersion`: `2.18.13.0`;
- `FileVersion`: `2.18.13.0`;
- `InformationalVersion`: `2.18.13`.

The .NET MAUI application in `src/CareNest.App/CareNest.App.csproj` uses:

- `ApplicationDisplayVersion`: `2.18.13`;
- `ApplicationVersion`: `21813`.

The central dependency baseline currently includes:

- `Microsoft.Maui.Controls`: `10.0.100`;
- Avalonia package family: `12.1.1`.

`tests/CareNest.UiTests/VersionConsistencyContractTests.cs` protects the release metadata, application build code, MAUI baseline and non-published version-document state from accidental drift.

## Accepted automated verification

PR #87 independently verified exact source `323ea67281cbbdb3e7ca149fabd7135e8ad8a377` through merge ref `421fead1e82c3d470ebf72417049d80b104c5516` before merging to `main` at `b3efa71ab412da65f08d78ad1c4d913e302ff901`.

Observed results:

- CareNest CI: **success** — run `32842319249`;
- CodeQL: **success** — run `32842319316`;
- unsuppressed Dependency Audit: **success** — run `32842319254`;
- Store Package Configuration: **success** — run `32842319272`;
- Store Inspection Artifacts: **success** — run `32842319256`;
- unit tests: **122/122**;
- integration tests: **54/54**;
- UI/source-policy tests: **218/218**;
- total core tests: **394/394**;
- Android Release build: **success**;
- Windows Release build: **success**;
- iOS simulator Release build: **success**;
- Mac Catalyst Release build: **success**;
- Linux desktop Release build: **success**;
- WebAssembly browser Release publish: **success**;
- stable documentation-link verification: **success** — 210 live local links across 131 stable active Markdown files.

The exact-source CareNest CI run required no retry. These results are authoritative for the exact source and merge boundary named above.

## Verification continuation rule

The repository now contains a small follow-up source-policy contract change that updates the active handoff from pre-verification wording to observed-verification wording. That continuation branch must complete a fresh exact-head matrix before its new source becomes the next accepted automated baseline.

Historical workflow success is never transferred to a newer source.

## Production evidence still required

Before CareNest `2.18.13` can be represented as production released, actual evidence remains required where applicable for:

- installed Android behavior and real notification/reminder behavior;
- installed Windows package/update behavior;
- signed/provisioned iPhone/iPad validation;
- installed Mac Catalyst validation;
- representative Linux runtime behavior;
- hosted browser/WebAssembly runtime behavior;
- accessibility with applicable assistive technology;
- packaged existing-data/SQLite/encrypted-document/backup compatibility;
- production signing/provisioning/notarization provenance;
- exact final package/deployment hashes and provenance;
- store-safe final payload inspection;
- live distribution metadata/declaration review;
- actual submission, review, approval and publication/deployment outcomes.

Unknown or unperformed rows must remain `NOT RUN`, `BLOCKED`, `N/A` or `FAIL` as actually justified.

## Safety and privacy boundary

This patch preparation does not add diagnosis, dosage calculation, treatment recommendation, clinical-risk scoring or emergency-service behavior. Public evidence must use fictional/synthetic application data and must not expose real health records, prescription documents, backup passwords, PINs, signing keys, access tokens, recovery codes or other secrets.

## Promotion rule

Do not describe CareNest `2.18.13` as published, store-approved, production-signed, fully platform-parity verified, accessibility-complete or globally bug-free until the corresponding evidence exists for the exact source/package/deployment being promoted.
