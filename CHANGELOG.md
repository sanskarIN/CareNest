# Changelog

All notable CareNest changes are recorded here using Keep a Changelog-style categories and semantic-versioning intent.

The complete changelog that was active immediately before the 2026-08-17 Gumroad rollout is preserved exactly at:

`docs/history/pre-gumroad-rollout-20260817/CHANGELOG.md`

Earlier exact snapshots remain under `docs/history/` and Git history.

## [Unreleased] - 2026-08-17

### Added — Ram Sandesh Gumroad storefront integration

Canonical storefront:

**https://ramsandesh.gumroad.com**

Added a repository-first Gumroad rollout that highlights the storefront without embedding external commerce into the CareNest health application package.

Added:

- `GUMROAD.md` canonical storefront guide;
- `docs/assets/gumroad_store_badge.svg` repository-only storefront badge;
- `docs/assets/README.md` asset/accessibility guidance;
- `docs/marketing/README.md` marketing documentation hub;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`;
- `docs/marketing/GUMROAD_ROLLOUT_CHECKLIST.md`;
- Gumroad custom repository link in `.github/FUNDING.yml`.

### Changed — highlighted repository documentation

The Gumroad storefront is now prominently surfaced in current reader-facing and maintainer documentation, including:

- `README.md`;
- `SUPPORT.md`;
- `BUY_ME_A_COFFEE.md`;
- `docs/README.md`;
- `docs/SUPPORT_CARENEST.md`;
- `docs/DOCUMENTATION_CATALOG.md`;
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`;
- `docs/DEVELOPER_REFERENCE.md`;
- `docs/REPOSITORY_GOVERNANCE.md`;
- `CONTRIBUTING.md`;
- `PROJECT_STATUS.md`;
- active `what_changed.md`.

The exact Gumroad URL is kept visible as a plain-text fallback anywhere the repository badge is used.

### Added — repository-only storefront branding

`docs/assets/gumroad_store_badge.svg` includes:

- custom storefront/shopping artwork;
- exact canonical URL;
- accessible SVG `<title>`;
- accessible SVG `<desc>`;
- explicit repository-only/package-boundary wording.

The generated promotional concept from the chat is represented in source control by a maintainable SVG rather than being silently copied into app package resources.

### Changed — external-commerce package isolation

The CareNest application runtime/package continues to exclude repository-only external commercial/funding destinations.

Current forbidden package markers:

- `ramsandesh.gumroad.com`;
- `buymeacoffee.com/sanskarIN`.

Repository/storefront/funding documentation is separate from health functionality and does not unlock diagnosis, dosage guidance, treatment recommendations, reminder priority/reliability, emergency assistance, clinical support, accounts/cloud behavior, or access to user health data.

CareNest does not automatically transmit local health records to Gumroad.

### Changed — package payload scanner

Updated:

`build/scripts/verify-store-safe-payload.py`

The scanner now defaults to both repository-only markers and continues to inspect:

- UTF-8;
- UTF-16 little-endian;
- UTF-16 big-endian;
- regular payload files;
- ZIP-compatible package entries such as AABs.

The scanner continues to fail closed for unreadable/missing inspection paths and returns failure when a forbidden marker is found.

The `--forbidden` option is repeatable for explicit one-or-more marker scans.

### Added/Changed — Gumroad regression contracts

Updated `FundingLinkContractTests.cs` to protect:

- repository Gumroad visibility;
- support/metadata placement;
- no in-app About surface;
- no medical/health entitlement claims;
- repository SVG accessibility metadata;
- absence of the Gumroad badge from app resources.

Updated `StoreFundingPayloadContractTests.cs` to protect:

- no Gumroad/Buy Me a Coffee URL in application runtime text-like files;
- no external-commerce URL constant in the shared runtime assembly;
- no obsolete external-commerce build switches;
- both package-scanner markers;
- UTF-8/UTF-16/ZIP scanning behavior;
- fail-closed scanner semantics.

### Changed — current documentation/evidence governance

The active `PROJECT_STATUS.md` and `docs/COMPLETE_PROJECT_DOCUMENTATION.md` were modernized for the 2026-08-17 Gumroad/source-line baseline after preserving their complete prior versions exactly under:

`docs/history/pre-gumroad-rollout-20260817/`

The previous active `what_changed.md` was also preserved in that history directory before the Gumroad handoff replaced it.

Historical dated verification files are not rewritten merely to backfill newer storefront links.

### Previous automated baseline before Gumroad rollout

Latest fully verified pre-Gumroad source:

`7cbe5568b6cffa06c279b29f3cb1b107ea988791`

That exact source passed:

- 122/122 unit tests;
- 39/39 integration tests;
- 173/173 UI/source-policy tests;
- **334/334 total core tests**;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- Android/Windows/iOS/Mac Catalyst store-candidate configurations;
- CodeQL.

### Verification rule for this continuation

The Gumroad rollout changes verification-relevant tests and the package scanner, so the earlier 334-test baseline is not automatically applied to newer source.

The exact final Gumroad source must pass the applicable current matrix before a new authoritative baseline is declared:

- formatting;
- unit tests;
- integration tests;
- UI/source-policy tests;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- all four store-candidate configurations;
- CodeQL.

The final exact source and workflow results are recorded in `what_changed.md` and, after successful completion, in a dated release verification record.

### Production status

CareNest remains `1.0.0-rc.1`.

The intended RC source scope is implemented and heavily automated, but production promotion still requires real-device notification/lifecycle validation, accessibility evidence, packaged existing-data/encrypted-data compatibility, production signing, signed-package inspection, current store metadata/policy review, an exact approved immutable production tag and publication evidence.

Do not describe CareNest as globally bug-free, production-signed, store-approved or production-published until those external gates are actually completed.

---

## Historical changelog

For the complete 2026-08-16 compiled-binding entry, the complete 2026-08-15 funding-package/final-bug-audit entry, and all earlier details, use:

`docs/history/pre-gumroad-rollout-20260817/CHANGELOG.md`

That preserved file remains the exact prior active changelog rather than a shortened reconstruction.
