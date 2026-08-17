# CareNest — Active Completion Handoff

**Date:** 2026-08-17  
**Release candidate:** `1.0.0-rc.1`  
**Repository:** `sanskarIN/CareNest`  
**Continuation focus:** Gumroad storefront rollout, repository branding, documentation completeness, and store-payload isolation

The complete previous active handoff is preserved at:

`docs/history/pre-gumroad-rollout-20260817/what_changed.md`

The older source-line-audit handoff and all earlier historical evidence remain preserved under `docs/history/` and Git history.

---

## 1. Requested continuation

This continuation implements the requested highlighted Gumroad destination:

`https://ramsandesh.gumroad.com`

The rollout intentionally distinguishes two concerns:

1. **high repository/documentation visibility** for the Gumroad storefront;
2. **continued absence from the packaged CareNest health application** unless a future explicit store-policy redesign approves external commerce in the runtime.

This preserves CareNest's local-first health-organizer scope while making the storefront prominent across current repository-owned support, documentation, marketing and metadata surfaces.

---

## 2. Repository-only Gumroad branding asset

Added:

`docs/assets/gumroad_store_badge.svg`

The SVG:

- displays the exact canonical storefront URL;
- includes an accessible `<title>` and `<desc>`;
- uses a custom storefront/shopping visual treatment;
- is suitable for GitHub/documentation embedding;
- is explicitly repository-only;
- is not copied into `src/CareNest.App`.

Asset documentation:

`docs/assets/README.md`

---

## 3. Canonical storefront documentation

Added root guide:

`GUMROAD.md`

It documents:

- canonical storefront URL;
- supported repository placement;
- plain-text and image-link embed formats;
- health-feature separation;
- no health-data transfer to Gumroad by CareNest;
- no medical/clinical entitlement from a Gumroad purchase;
- application-package exclusion rule;
- maintainer consistency rules.

Added maintainer/compliance guide:

`docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`

Added marketing documentation hub:

`docs/marketing/README.md`

Added rollout checklist:

`docs/marketing/GUMROAD_ROLLOUT_CHECKLIST.md`

---

## 4. Highlighted repository surfaces

Updated the current repository surfaces so the storefront is prominent to readers:

- `README.md` — top-level linked Gumroad badge, dedicated storefront section, support-links section, complete-documentation links, package-boundary statement;
- `SUPPORT.md` — linked Gumroad badge, dedicated storefront section and plain-text URL;
- `BUY_ME_A_COFFEE.md` — cross-link to the Gumroad storefront and repository badge;
- `.github/FUNDING.yml` — Gumroad added as a second repository custom link;
- `docs/README.md` — storefront badge, storefront navigation, current package-boundary documentation;
- `docs/DOCUMENTATION_CATALOG.md` — Gumroad documentation ownership, QA contract mapping, branding asset and package-policy navigation.

Canonical URL everywhere in the new/current surfaces:

`https://ramsandesh.gumroad.com`

Historical snapshots and dated verification evidence are not rewritten merely to backfill modern marketing links.

---

## 5. Store-safe runtime/package boundary

CareNest remains a local-first organizational health application.

The packaged application intentionally does not contain:

- the Gumroad destination;
- the repository Gumroad badge;
- Gumroad promotional ViewModel commands;
- Gumroad promotional XAML;
- Gumroad URL constants in shared runtime assemblies.

The same package boundary remains in effect for the repository-only Buy Me a Coffee destination.

This means repository visibility is high without silently converting CareNest into an in-app external-commerce surface.

---

## 6. Store-safe payload scanner upgraded

Updated:

`build/scripts/verify-store-safe-payload.py`

The scanner now defaults to both repository-only markers:

- `buymeacoffee.com/sanskarIN`;
- `ramsandesh.gumroad.com`.

The scanner continues to:

- inspect UTF-8;
- inspect UTF-16 little-endian;
- inspect UTF-16 big-endian;
- scan regular payload files;
- inspect ZIP-compatible package entries such as AABs;
- fail closed for inspection errors;
- return failure when a forbidden marker is found.

The `--forbidden` option is now repeatable so maintainers can supply one or more explicit markers when needed.

Existing store-inspection workflows invoke the scanner without overriding defaults, so their package scans inherit both default external-commerce markers.

---

## 7. Regression coverage upgraded

Updated:

`tests/CareNest.UiTests/FundingLinkContractTests.cs`

The contract now verifies:

- Buy Me a Coffee remains present in repository support material;
- Gumroad is present in required repository support/material/metadata;
- Gumroad is absent from About ViewModel/XAML runtime surfaces;
- external commercial links are not health entitlements;
- the Gumroad repository badge exists and has accessible SVG metadata;
- the Gumroad badge is absent from packaged app resources.

Updated:

`tests/CareNest.UiTests/StoreFundingPayloadContractTests.cs`

The contract now verifies:

- both Buy Me a Coffee and Gumroad are absent from all text-like application runtime files;
- shared runtime constants do not carry external-commerce URLs;
- obsolete external-commerce build switches remain absent;
- the store payload scanner contains both markers;
- the scanner retains UTF-8/UTF-16 and ZIP inspection behavior;
- the scanner retains fail-closed semantics.

The test-count increase is expected because `FundingLinkContractTests` now contains additional independent Gumroad placement/accessibility checks.

---

## 8. Latest fully verified baseline before this rollout

The last fully verified source before the Gumroad rollout was:

`7cbe5568b6cffa06c279b29f3cb1b107ea988791`

That exact revision passed:

- 122/122 unit tests;
- 39/39 integration tests;
- 173/173 UI/source-policy tests;
- 334/334 total core tests;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- all four store-candidate configurations;
- CodeQL.

The Gumroad rollout creates a newer source/documentation/test boundary and therefore must not inherit that verification claim automatically.

---

## 9. Commits created in this Gumroad continuation

1. `b9a9c2b8849e17fd32914b23290113314ea91586`  
   `assets: add Gumroad storefront badge`

2. `87da5df5be4d3a9885812747fbd85be083b63e68`  
   `docs: add Gumroad storefront guide`

3. `6139d5a8a6531817fc9b6305d1f912d570ba8340`  
   `docs: define Gumroad placement policy`

4. `3e682fe11b110dd0daeb8a8bd71a0613d229f460`  
   `meta: add Gumroad to repository links`

5. `b4af3f78dfb340502d49ca1933531dd0e9ec0a15`  
   `docs: highlight Gumroad in support guide`

6. `55a1e782fda4ddda1cc0bf91190ce8b126ea18ec`  
   `docs: feature Gumroad across main README`

7. `d5de414f28222b45bbd995f263f9f71588aa46a7`  
   `docs: cross-link Gumroad from support page`

8. `549b5a569732a7cce42e3fd270b61744bc4c36fc`  
   `build: scan Gumroad from store payloads`

9. `dfdcad96a1f1e498a692a342cf9fd2f0d11f4db6`  
   `test: enforce repository-only Gumroad placement`

10. `30623f4c81e45a483a8f40d05f5abb4dece75af6`  
    `test: protect store payload from Gumroad marker`

11. `ae908dc94c4ee5c63d48f9eb3d915db626f51bf6`  
    `docs: highlight Gumroad in documentation hub`

12. `5738a09ffb299d12b25d1e52c75d581827ebea55`  
    `docs: catalog Gumroad branding and package policy`

13. `a12e23361595d1427c6ae160bc56636bd1e56f1d`  
    `docs: document repository branding assets`

14. `489a424de434ddda0e203746dd58ddd035ef581c`  
    `docs: add marketing documentation hub`

15. `6508cdb39d24cb7aa5c5ffb944089b40aff9e6f4`  
    `docs: add Gumroad rollout checklist`

16. `fdf744db7bfe71b56d6b1b84f1308d1b44981dd1`  
    `docs: preserve pre-Gumroad handoff`

This active handoff update is intentionally the final content commit of the rollout so its exact resulting revision becomes the single workflow target to evaluate.

---

## 10. Verification rule for the final Gumroad revision

Because repeated pushes to `main` supersede earlier workflow runs, only the workflow set associated with the final handoff commit should be treated as current Gumroad-rollout verification.

Required automated checks include:

- formatting;
- unit tests;
- integration tests;
- UI/source-policy tests including the Gumroad contracts;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build;
- Android store-candidate build;
- Windows store-candidate build;
- iOS simulator store-candidate build;
- Mac Catalyst store-candidate build;
- CodeQL.

Do not mark the final Gumroad rollout as a new verified baseline until those workflows complete successfully for the exact final revision.

Store Inspection Artifacts does not run on ordinary `main` pushes; its source contract remains protected by UI tests and it will exercise the two-marker scanner on pull-request/release/tag runs.

---

## 11. Documentation completeness boundary

The Gumroad integration is documented at multiple levels:

- reader-facing storefront guide;
- highlighted repository entry points;
- documentation hub/catalog;
- marketing-maintainer policy;
- asset usage/accessibility guide;
- rollout checklist;
- automated source/package contract tests;
- active continuation handoff;
- preserved pre-rollout history.

The repository promotional image generated during the chat is represented in source control by the maintainable SVG badge. The generated raster artwork is not required by the CareNest application and is intentionally not copied into the packaged runtime.

---

## 12. Health, privacy and commerce boundary

A Gumroad purchase does not provide or change:

- diagnosis;
- dosage calculation or inference;
- treatment recommendations;
- clinical medication-interaction checking;
- clinical risk scoring;
- reminder priority or delivery guarantees;
- emergency services;
- CareNest health-data access;
- a CareNest cloud account or cloud synchronization.

CareNest does not automatically transmit local health records to Gumroad.

---

## 13. Production work still remaining

The Gumroad repository rollout does not complete the external/manual RC1 production gates.

Still required before production promotion:

- representative Android real-device/emulator validation;
- notification permission and real reminder-delivery testing;
- exact/inexact alarm and battery-optimization validation;
- reboot/restart/time/time-zone/DST recovery validation;
- Windows reminder/lifecycle validation;
- real iPhone/iPad notification validation;
- Mac Catalyst notification/lifecycle validation;
- packaged existing-data upgrade/readability/editability;
- packaged encrypted-document compatibility;
- packaged encrypted-backup create/restore/wrong-password/tamper tests;
- screen-reader testing;
- large-text testing;
- keyboard/focus testing;
- light/dark/system contrast validation;
- reduced-motion validation;
- production Android/Apple/Windows signing identities outside Git;
- final signed packages and checksums/provenance;
- current store listing/privacy/data-safety metadata;
- submission-time store-policy review;
- approved immutable production tag;
- final publication evidence.

---

## 14. Future continuation rule

For future CareNest work:

1. keep Gumroad canonical as `https://ramsandesh.gumroad.com`;
2. keep the storefront highly visible in current repository documentation/support/marketing surfaces where appropriate;
3. keep plain-text fallback links beside visual CTAs;
4. preserve the package/runtime exclusion unless an explicit store-policy redesign is performed;
5. keep both external-commerce markers in package scanning;
6. preserve health-data and medical-scope boundaries;
7. preserve historical evidence instead of rewriting it;
8. make small reviewable commits;
9. update regression contracts with behavior changes;
10. run the final exact-revision workflow matrix before promoting a new automated baseline.
