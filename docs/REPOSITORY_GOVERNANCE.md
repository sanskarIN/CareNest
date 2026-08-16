# CareNest Repository Governance and Evidence Rules

This document defines how source, documentation, verification evidence, history and release status should be maintained so the repository does not drift into contradictory claims.

## 1. Sources of truth

### Current product/release state

`PROJECT_STATUS.md` is the primary current-state summary.

### Remaining work

`docs/releases/NEXT_STEPS.md` is the primary current operational checklist.

### Current automated evidence

Use the latest dated verification record that explicitly names the exact source boundary. For the current executable source, use `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

### Whole-project reference

Use `docs/COMPLETE_PROJECT_DOCUMENTATION.md`.

### Specialized technical detail

Use the architecture/privacy/security/testing/setup/design documents for their respective topics.

### Chronological continuation history

Use `what_changed.md`, `CHANGELOG.md`, Git history and `docs/history/`.

## 2. Current verified source boundary

PR #74 source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

The current documentation head can move beyond that SHA if changes are documentation-only. Documentation-only commits do not become new executable verification boundaries.

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

Do not rewrite old verification files merely because a newer source exists. Old files should continue describing the source they actually verified.

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

All user-facing and release-facing documentation must preserve the product boundary:

- organizational, not diagnostic;
- no dosage calculation/inference;
- no treatment recommendation;
- no clinical interaction/risk claims;
- no emergency-service replacement;
- no notification-delivery guarantee.

New features must not quietly cross this boundary through UI copy, analytics, automation or inferred scheduling.

## 8. Local-first/privacy language

For current v1, documentation must not imply a CareNest cloud account/backend that does not exist.

Preserve:

- no required CareNest account;
- no automatic CareNest cloud synchronization/upload;
- no hidden runtime analytics/telemetry client;
- explicit outbound export/share/calendar/browser boundaries;
- external copies may remain outside CareNest control.

## 9. Funding/application-package boundary

The distributed application source/package currently contains no external Buy Me a Coffee destination/card/command/artwork.

Repository funding documentation/metadata is separate from the application binary and must not imply medical/health feature entitlement.

Historical documents describing earlier funding build toggles may remain as history but must not be linked as the current package design.

## 10. Security statements

Distinguish:

- structured SQLite sandbox/device protection;
- separately encrypted document payloads;
- password-encrypted backups;
- optional app lock;
- production signing;
- external exported copies.

Do not describe the whole database as encrypted when the implementation does not provide transparent database encryption.

## 11. Dependency security versus compatibility

A green dependency audit means the source dependency graph passes configured audit policy.

It does not prove:

- existing SQLite database upgrade compatibility;
- historical encrypted document readability;
- historical backup restore compatibility;
- real production package behavior.

Those require separate packaged/manual validation.

## 12. Test-count governance

Test counts are evidence for a source boundary, not permanent product constants.

When tests are added:

- update current verification/status documents after a green run;
- do not rewrite old evidence counts;
- investigate unexpected test-count decreases.

Current PR #74 evidence: 122 unit + 39 integration + 170 UI/source-policy = 331.

## 13. XAML governance

Binding-bearing XAML is subject to strict compiled-binding policy:

- real root `x:DataType`;
- item-specific `x:DataType` in templates;
- typed Source/RelativeSource bindings;
- typed picker display bindings where context changes;
- `XC0022`–`XC0025` as errors;
- no suppression/type-safety bypass.

Documentation and examples must not recommend patterns that violate this policy.

## 14. Secrets and private data

Never place in Git, documentation examples, issues or screenshots:

- real health records;
- real personal backups;
- PINs/passwords;
- encryption keys;
- access tokens;
- signing private keys/keystores/certificates;
- production credentials.

Use synthetic/fictional examples.

## 15. Change-documentation coupling

When a source change alters behavior, update applicable documents in the same work:

- feature/user guide;
- architecture/service boundary;
- schema/data lifecycle;
- security/threat model;
- testing contract;
- release/manual matrix;
- configuration/setup;
- current status/evidence after verification.

## 16. Major change triggers

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

## 17. Pull request documentation expectations

A PR description should identify:

- user-visible behavior change;
- architecture/data/security impact;
- tests added/changed;
- manual checks required;
- documentation updated;
- release/evidence implications.

## 18. Release evidence rules

Production promotion must use the exact approved source/tag. Do not move a failed production tag to a different commit to reuse the same version identity.

Final production evidence should include package identity/version, source SHA, package filenames, SHA-256 values, signing/notarization/store provenance and required smoke/manual results.

## 19. Current documentation audit

The 2026-08-16 full documentation pass is recorded in `docs/releases/DOCUMENTATION_AUDIT_20260816.md`.

## 20. Documentation catalog

Use `docs/DOCUMENTATION_CATALOG.md` as the complete navigation and ownership map.