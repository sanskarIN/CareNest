# CareNest Repository Governance and Evidence Rules

This document defines how source, documentation, verification evidence, repository marketing, history and release status should be maintained so the repository does not drift into contradictory claims.

## 1. Sources of truth

### Current product/release state

`PROJECT_STATUS.md` is the primary current-state summary.

### Remaining work

`docs/releases/NEXT_STEPS.md` is the primary current operational checklist.

### Current automated evidence

Use the latest dated verification record that explicitly names the exact source boundary. The latest fully verified pre-Gumroad source is `7cbe5568b6cffa06c279b29f3cb1b107ea988791`; a newer Gumroad rollout source becomes authoritative only after its exact workflow set is green and recorded.

The permanent strict-XAML evidence remains `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md` for the source it verified.

### Whole-project reference

Use `docs/COMPLETE_PROJECT_DOCUMENTATION.md` together with `docs/README.md` and `docs/DOCUMENTATION_CATALOG.md` for current navigation.

### Repository storefront policy

Use:

- `GUMROAD.md` for the canonical storefront guide;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` for placement/package rules;
- `docs/assets/gumroad_store_badge.svg` for the repository-only visual CTA.

Canonical storefront:

`https://ramsandesh.gumroad.com`

### Specialized technical detail

Use the architecture/privacy/security/testing/setup/design documents for their respective topics.

### Chronological continuation history

Use `what_changed.md`, `CHANGELOG.md`, Git history and `docs/history/`.

## 2. Current verified source boundary

Latest fully verified pre-Gumroad source:

`7cbe5568b6cffa06c279b29f3cb1b107ea988791`

That exact source passed:

- 122 unit tests;
- 39 integration tests;
- 173 UI/source-policy tests;
- 334 total core tests;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- all four store-candidate configurations;
- CodeQL.

The current repository head can move beyond that SHA. A newer verification-relevant head must not inherit those results automatically.

The Gumroad rollout changes repository documentation, source-policy tests and the store-payload scanner, so it requires its own exact-head verification before becoming a new authoritative automated baseline.

## 3. Evidence must name an exact source

A statement such as “all tests pass” is incomplete without a source boundary.

Verification records should include:

- exact source/head SHA;
- base/merge SHA where relevant;
- PR/run number;
- workflow name;
- test counts;
- platform build results;
- security/dependency results;
- package/inspection results where applicable;
- whether the verification branch/marker entered `main`.

## 4. Historical evidence is immutable context

Do not rewrite old verification files merely because a newer source exists or a new Gumroad storefront is being promoted. Old files should continue describing the source and repository state they actually verified.

When an old file becomes confusing as an entry point:

- keep it as historical evidence;
- update the current catalog/index/status to point to newer authority;
- archive exact prior active documents under `docs/history/` when replacing a major active handoff/status surface.

## 5. Documentation completeness does not equal release completeness

A checkbox indicating that a runbook exists means the procedure is documented, not that it has been performed.

Manual/device/package/signing/store rows remain open until evidenced.

## 6. No false “bug-free” claim

Allowed wording:

> No known automated defect remains under the configured verification matrix for the named exact source.

Avoid absolute statements that the software is globally bug-free or that all platform conditions have been proven.

## 7. Medical-safety language

All user-facing, repository-marketing and release-facing documentation must preserve the product boundary:

- organizational, not diagnostic;
- no dosage calculation/inference;
- no treatment recommendation;
- no clinical interaction/risk claims;
- no emergency-service replacement;
- no notification-delivery guarantee.

New features or promotional copy must not quietly cross this boundary through UI text, analytics, automation, inferred scheduling, fundraising language or storefront claims.

## 8. Local-first/privacy language

For current v1, documentation must not imply a CareNest cloud account/backend that does not exist.

Preserve:

- no required CareNest account;
- no automatic CareNest cloud synchronization/upload;
- no hidden runtime analytics/telemetry client;
- explicit outbound export/share/calendar/browser boundaries;
- external copies may remain outside CareNest control.

CareNest does not automatically transmit local health records to Gumroad or Buy Me a Coffee.

## 9. Repository storefront/funding and application-package boundary

Current repository-only destinations:

- Gumroad: `https://ramsandesh.gumroad.com`;
- Buy Me a Coffee: `https://buymeacoffee.com/sanskarIN`.

These may appear prominently in current repository documentation, GitHub funding/project metadata and repository-only marketing assets.

The distributed application source/package currently contains no external Gumroad or Buy Me a Coffee destination/card/command/promotional artwork.

Repository storefront/funding documentation is separate from the application binary and must not imply medical/health feature entitlement.

A Gumroad purchase or contribution does not unlock:

- diagnosis;
- dosage guidance;
- treatment recommendations;
- clinical support;
- reminder priority or reliability;
- emergency assistance;
- user health-data access.

The package scanner defaults to both repository-only destination markers and the source-policy tests enforce runtime absence.

Historical documents describing earlier funding build toggles may remain as history but must not be linked as the current package design.

## 10. Repository marketing asset governance

Repository-only promotional assets live outside `src/CareNest.App`.

For the current Gumroad badge:

`docs/assets/gumroad_store_badge.svg`

Requirements:

- exact canonical URL;
- meaningful SVG title/description;
- plain-text URL fallback in surrounding documentation;
- no private health data;
- no medical claims;
- no placement in packaged CareNest application resources under the current product/store boundary.

Generated chat artwork can inspire a maintainable repository asset, but repository source should retain a reviewable, accessible and package-safe asset rather than silently copying promotional binaries into the application.

## 11. Security statements

Distinguish:

- structured SQLite sandbox/device protection;
- separately encrypted document payloads;
- password-encrypted backups;
- optional app lock;
- production signing;
- external exported copies.

Do not describe the whole database as encrypted when the implementation does not provide transparent database encryption.

## 12. Dependency security versus compatibility

A green dependency audit means the source dependency graph passes configured audit policy.

It does not prove:

- existing SQLite database upgrade compatibility;
- historical encrypted document readability;
- historical backup restore compatibility;
- real production package behavior.

Those require separate packaged/manual validation.

## 13. Test-count governance

Test counts are evidence for a source boundary, not permanent product constants.

When tests are added:

- update current verification/status documents after a green run;
- do not rewrite old evidence counts;
- investigate unexpected test-count decreases.

Latest fully verified pre-Gumroad evidence: 122 unit + 39 integration + 173 UI/source-policy = 334.

The Gumroad rollout adds additional independent repository-placement/accessibility coverage, but its resulting count must not be promoted until the exact final revision is green.

## 14. XAML governance

Binding-bearing XAML is subject to strict compiled-binding policy:

- real root `x:DataType`;
- item-specific `x:DataType` in templates;
- typed Source/RelativeSource bindings;
- typed picker display bindings where context changes;
- `XC0022`–`XC0025` as errors;
- no suppression/type-safety bypass.

Documentation and examples must not recommend patterns that violate this policy.

## 15. Secrets and private data

Never place in Git, documentation examples, marketing assets, storefront notes, issues or screenshots:

- real health records;
- real personal backups;
- PINs/passwords;
- encryption keys;
- access tokens;
- signing private keys/keystores/certificates;
- production credentials.

Use synthetic/fictional examples.

## 16. Change-documentation coupling

When a source change alters behavior, update applicable documents in the same work:

- feature/user guide;
- architecture/service boundary;
- schema/data lifecycle;
- security/threat model;
- testing contract;
- release/manual matrix;
- configuration/setup;
- storefront/marketing policy when external commerce changes;
- current status/evidence after verification.

## 17. Major change triggers

### Database/schema change

Update schema, migration, backup compatibility, privacy lifecycle and tests.

### Encryption format/key ownership change

Update document/backup architecture, security/threat model, compatibility tests and release validation.

### Reminder behavior/platform scheduling change

Update reminder contract, platform behavior docs, test matrix and manual validation.

### New network/cloud feature

Require explicit authentication/authorization/consent/privacy/deletion/export/key-management/threat-model/offline/store-policy design.

### New health/clinical behavior

Requires separate product/safety/legal review; it cannot be introduced as an incidental enhancement to the current organizational scope.

### New or changed external-commerce surface

Require review of:

- `GUMROAD.md`;
- `SUPPORT.md`;
- `.github/FUNDING.yml` where applicable;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`;
- repository promotional assets;
- `FundingLinkContractTests`;
- `StoreFundingPayloadContractTests`;
- `build/scripts/verify-store-safe-payload.py`;
- current store/release policy.

## 18. Pull request documentation expectations

A PR description should identify:

- user-visible behavior change;
- architecture/data/security impact;
- storefront/external-commerce impact where applicable;
- tests added/changed;
- manual checks required;
- documentation updated;
- release/evidence implications.

## 19. Release evidence rules

Production promotion must use the exact approved source/tag. Do not move a failed production tag to a different commit to reuse the same version identity.

Final production evidence should include package identity/version, source SHA, package filenames, SHA-256 values, signing/notarization/store provenance and required smoke/manual results.

Repository-only storefront promotion must remain distinguishable from application package contents in release evidence.

## 20. Current documentation audit

The 2026-08-16 full documentation pass is recorded in `docs/releases/DOCUMENTATION_AUDIT_20260816.md`.

The 2026-08-17 Gumroad continuation adds current repository marketing/storefront documentation without rewriting that historical audit.

## 21. Documentation catalog

Use `docs/DOCUMENTATION_CATALOG.md` as the complete navigation and ownership map.

For active continuation details, use `what_changed.md`.
