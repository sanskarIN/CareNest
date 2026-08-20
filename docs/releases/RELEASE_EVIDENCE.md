# CareNest Release Evidence

**Release line:** `1.0.0-rc.1`  
**Current automated evidence authority:** `docs/releases/AUTOMATED_BASELINE.md`  
**Production evidence standard:** `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`  
**Production evidence index:** `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

This document defines the evidence that must exist before a CareNest source commit is promoted from release candidate to a public production release.

Do not hard-code a moving accepted source SHA, workflow run ID, or test total in this stable policy document. The latest actually observed exact-source automated result belongs in `docs/releases/AUTOMATED_BASELINE.md` and its dated verification record.

## 1. Evidence result semantics

Production records use the canonical result states defined by `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`:

- `PASS`;
- `FAIL`;
- `BLOCKED`;
- `N/A`;
- `NOT RUN`.

Unknown, stale, blocked, skipped, cancelled, superseded, or unperformed work is not a pass. `N/A` requires a defensible reason.

Canonical templates are evidence containers, not evidence by themselves. Release-specific copies record actual results.

## 2. Automated evidence

The exact release commit/tag must have successful GitHub-hosted evidence for the applicable configured gates:

- CareNest CI core verification;
- Android Release compilation;
- Windows Release compilation;
- iOS simulator Release compilation;
- Mac Catalyst Release compilation;
- CodeQL;
- unsuppressed Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- CareNest Release Evidence.

Use `docs/releases/AUTOMATED_BASELINE.md` for the latest accepted exact-source result. If verification-relevant source changes after that boundary, follow `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` and record the new result only after it is actually observed.

A previous green run does not prove a later source is green.

## 3. Release Evidence artifact contents

The Release Evidence workflow records source/test/dependency/workspace provenance for its exact run.

### Source identity/provenance

Retain:

- exact Git commit SHA;
- Git ref/tag;
- GitHub Actions run ID and attempt;
- .NET SDK/runtime information;
- pre-test Git status;
- post-test tracked-workspace Git status;
- tracked-file path manifest;
- SHA-256 checksum manifest for tracked repository files according to the workflow.

### Test evidence

Retain the configured unit, integration, and UI/source-policy test output.

The workflow should preserve useful evidence even when one suite fails. A failed workflow remains failed even if an artifact was uploaded.

### Dependency evidence

Retain the configured transitive dependency inventories. The separate Dependency Audit workflow remains the authoritative vulnerability-audit gate and must stay unsuppressed according to current repository policy.

A dependency inventory is provenance; it does not replace vulnerability audit, packaged SQLite compatibility, or encrypted-data compatibility testing.

### Artifact integrity and failure preservation

Evidence artifacts should preserve:

- generated evidence files;
- checksums for those files;
- independent suite outcomes where configured;
- tracked-workspace integrity state;
- failure evidence needed to diagnose a blocked release.

Artifact existence alone is not release approval.

## 4. Canonical production validation records

Use release-specific copies of the templates linked by `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`:

- `docs/releases/templates/ANDROID_DEVICE_VALIDATION_RECORD.md`;
- `docs/releases/templates/WINDOWS_VALIDATION_RECORD.md`;
- `docs/releases/templates/IOS_DEVICE_VALIDATION_RECORD.md`;
- `docs/releases/templates/MACCATALYST_VALIDATION_RECORD.md`;
- `docs/releases/templates/ACCESSIBILITY_VALIDATION_RECORD.md`;
- `docs/releases/templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`;
- `docs/releases/templates/SIGNING_PROVENANCE_RECORD.md`;
- `docs/releases/templates/STORE_SUBMISSION_RECORD.md`;
- `docs/releases/templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`.

Canonical templates must remain visibly unperformed. Do not edit a template to look like release evidence.

## 5. Public evidence data boundary

Use fictional or synthetic application data in public/shared evidence.

Do not commit:

- real user health records;
- real prescription documents;
- real private contact data copied from a user;
- PINs;
- backup passwords;
- cryptographic keys;
- private signing keys;
- keystores/certificate private material;
- access tokens;
- recovery codes;
- store/service credentials;
- other secrets.

Screenshots, logs, reports, package evidence notes, and issue references must follow the same privacy boundary.

## 6. Manual platform evidence

Record representative installed/package behavior on appropriately provisioned targets.

Every completed result should include at least:

- platform and OS version;
- device/emulator/simulator/host identity where applicable;
- app version/build;
- exact source SHA/tag;
- exact package identity/checksum when applicable;
- date and time zone;
- result state;
- non-sensitive evidence/notes;
- issue/fix reference for failure or blockage.

Simulator compilation is not real iPhone/iPad notification evidence. Hosted compilation is not installed-package behavior.

## 7. Packaged compatibility evidence

Using representative fictional/synthetic data, validate as applicable:

- SQLite open/integrity/migration behavior;
- profiles, medicines, schedules, reminder occurrences/logs, appointments, stock, tags, settings, and related records;
- reminder reconciliation after packaged upgrade or restore;
- encrypted-document compatibility;
- current encrypted backup creation/inspection/restore;
- clean-install restore;
- wrong-password rejection;
- tamper rejection;
- truncation rejection;
- trailing-data rejection;
- genuine historical encrypted backup compatibility where genuine prior bytes safely exist.

Do not manufacture a current artifact and label it historical evidence.

Current backup resource boundaries are defined by the backup architecture/security records and must not be silently weakened merely to make an unverified historical scenario pass.

## 8. Accessibility evidence

Automated XAML/source semantics are necessary but not sufficient.

Retain representative evidence for:

- screen-reader behavior;
- reading/focus order;
- meaningful names/roles/states/hints;
- large text/display scaling;
- keyboard/input behavior where applicable;
- light/dark/system contrast;
- color-independent state meaning;
- reduced motion where applicable;
- privacy-safe actionable error/validation messaging.

Use `docs/releases/templates/ACCESSIBILITY_VALIDATION_RECORD.md`.

## 9. Security evidence

Before production promotion:

- review `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- review `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` where applicable;
- confirm maintained SQLite/provider package floors remain satisfied;
- confirm no unsuppressed dependency advisory blocks the intended production graph;
- confirm the former SQLite advisory suppression has not been restored merely to force a green audit;
- confirm no wildcard/severity-wide audit suppression was introduced;
- confirm no signing key, certificate private material, keystore, `.env`, service credential, access token, recovery code, or API secret was committed;
- require CodeQL for the exact production source/tag;
- require unsuppressed Dependency Audit for the exact production source/tag;
- retain packaged existing-data/encrypted-document/backup compatibility evidence separately.

Green source dependency evidence is not packaged historical-data proof.

## 10. Current external-commerce package boundary

Repository support/storefront promotion remains separate from the CareNest health application package.

The final distributed application package must contain no external application promotion or purchase surface for:

- `https://buymeacoffee.com/sanskarIN`;
- `https://ramsandesh.gumroad.com`.

For every final production package, retain evidence that:

- the exact payload was scanned for `buymeacoffee.com/sanskarIN`;
- the exact payload was scanned for `ramsandesh.gumroad.com`;
- no Gumroad/Buy Me a Coffee promotional card, command, button, destination, or artwork is present in the installed app under the current policy;
- purchase/funding state does not change diagnosis, dosage, treatment, reminder priority/reliability, emergency behavior, clinical support, account/cloud behavior, or local-health-data access.

A future policy change requires explicit product/security/store review and fresh verification.

## 11. Structured package checksum/provenance evidence

Use `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`.

Source-controlled tooling:

- `build/scripts/create-package-evidence.py`;
- `build/scripts/create-package-evidence.sh`;
- `build/scripts/create-package-evidence.ps1`;
- `build/scripts/test-create-package-evidence.py`;
- `build/scripts/verify-store-safe-payload.py`.

For every final production package, generate package evidence with `--stage production`.

Production mode requires the source/tag/workspace/provenance/store-safe conditions documented by the package-evidence guide, including:

- immutable `v*` tag;
- source tag resolves to the recorded source SHA;
- checked-out HEAD equals that source SHA;
- clean tracked workspace;
- non-secret real signing/notarization/store-managed provenance;
- successful store-safe payload scanning;
- SHA-256 evidence;
- evidence output outside the payload being hashed.

The tool does not sign artifacts or prove store approval.

## 12. Store-policy evidence

Preliminary review:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

The dated review is not store approval and does not replace submission-day review.

Immediately before submitting the exact production package/listing:

- re-open current official Apple policy sources where applicable;
- re-open current official Google Play policy sources where applicable;
- re-open current Microsoft/Windows store policy sources where applicable;
- complete live store-console declarations/metadata for the exact production feature/package set;
- record review date, sources, conclusions, and required changes;
- repeat affected exact-source/package verification when a source/package change is required.

Use `docs/releases/templates/STORE_SUBMISSION_RECORD.md` to keep policy review, metadata completion, submission, review, rejection, approval, and publication distinct.

## 13. Store and signing evidence

For each intended distribution channel:

- production signing credentials remain outside Git;
- signed package originates from the exact approved source/tag;
- package identifier/version/build match release metadata;
- permissions/capabilities match documented behavior;
- listing claims remain organizational/non-clinical;
- privacy/data-safety declarations match the exact runtime/package;
- final filename and SHA-256 are recorded;
- non-secret signing/notarization/store-managed provenance is recorded;
- structured package evidence JSON is retained;
- installed-package smoke/manual validation is complete.

Use `docs/releases/templates/SIGNING_PROVENANCE_RECORD.md` for safe provenance fields.

## 14. Exact production tag behavior

For an approved immutable `v*` source tag, require every configured tag-triggered gate, including:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- CareNest Release Evidence.

If a required tag gate fails:

1. preserve the failed evidence;
2. do not promote the failing tag as successful production evidence;
3. fix the source/configuration on a new commit;
4. repeat applicable verification/manual/package checks;
5. use a corrected approved version/tag rather than moving the failed tag.

## 15. Production release approval

Use a release-specific copy of:

`docs/releases/templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`

Production approval must aggregate actual current evidence for:

- automated source verification;
- package compatibility;
- platform/device behavior;
- accessibility;
- security/dependency review;
- signing/notarization;
- final package checksum/provenance and external-commerce scans;
- current store policy/metadata/declarations;
- submission/review/approval/publication where applicable.

Green CI alone cannot mark production approved.

## 16. Release record fields

For a promoted release, retain at least:

```text
Version/build:
Tag:
Commit SHA:
CareNest CI run:
CodeQL run:
Dependency Audit run:
Store Package Configuration run:
Store Inspection Artifacts run:
Release Gate run:
Release Evidence run/artifact/checksums:
Unit tests:
Integration tests:
UI/source-policy tests:
Android build:
Windows build:
iOS simulator build:
Mac Catalyst build:
Manual Android evidence:
Manual Windows evidence:
Manual iOS/iPadOS evidence:
Manual Mac Catalyst evidence:
Packaged SQLite compatibility:
Encrypted document compatibility:
Backup compatibility:
Accessibility evidence:
Store-policy review date/sources:
Google Play Health apps declaration:
Google Play Data safety:
Apple privacy metadata:
Microsoft privacy metadata:
Signing/notarization evidence:
BMC package-marker scan:
Gumroad package-marker scan:
Package evidence JSON:
Final package SHA-256/provenance:
Store submission state:
Store approval/publication evidence:
Release owner:
Release date:
```

A blank, blocked, unknown, failed, stale, or `NOT RUN` required field remains a release blocker unless the evidence standard explicitly justifies the item as `N/A`.
