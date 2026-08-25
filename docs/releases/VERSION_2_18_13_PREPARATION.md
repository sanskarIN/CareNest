# CareNest 2.18.13 Release Preparation

**Prepared:** 2026-08-25  
**Target version:** `2.18.13`  
**Package/build code:** `21813`  
**State:** PREPARED IN SOURCE — NOT PUBLISHED  
**Repository:** `https://github.com/sanskarIN/CareNest`  
**Preparation branch:** `continue/prepare-2.18.13-20260825`  
**Starting `main`:** `b2db4821047dbfb7fe223961fc237afcdfc8371e`

This document records source preparation for CareNest `2.18.13`. It does not claim store approval, publication, production signing, real-device validation, accessibility completion, or full Linux/browser feature parity.

## Why this patch line exists

CareNest `2.18.12` reached an accepted automated-source boundary, and the post-merge governance PR #86 also completed its own exact-head verification before merging to `main` at `b2db4821047dbfb7fe223961fc237afcdfc8371e`.

The `2.18.13` line is a maintenance continuation from that verified repository state. It rolls the active source version forward without converting unresolved production evidence into a pass. The external/manual release blockers inherited from `2.18.12` remain open until genuine evidence exists.

## Version-specific release package

Use these version-specific documents together:

- `VERSION_2_18_13_PREPARATION.md` — source/version boundary and promotion rules;
- `RELEASE_NOTES_2_18_13_DRAFT.md` — publication draft that remains non-final;
- `RELEASE_CHECKLIST_2_18_13.md` — exact-head, production-evidence, signing and store checklist.

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

## Starting verification authority

The branch starts from merged `main` commit `b2db4821047dbfb7fe223961fc237afcdfc8371e`, which includes PR #86.

PR #86 exact head `e14a40d095a6f39993a0f62e497f15ec4668701f` completed successfully before merge in:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts.

Those results are historical evidence for that exact source only. They are not reused as verification evidence for the newer `2.18.13` branch head.

## Required automated acceptance for 2.18.13

The exact final `2.18.13` preparation head must independently complete the configured verification matrix, including:

- repository formatting and source-policy checks;
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

Queued, skipped, cancelled, failed, superseded or older-head results are not success evidence for a newer source.

## Production evidence inherited as open work

The source roll-forward does not satisfy the production rows that were still open for `2.18.12`. Before `2.18.13` can be represented as production released, actual evidence is still required where applicable for:

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
