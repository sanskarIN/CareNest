# CareNest Release Process

**Release line:** `1.0.0-rc.1`  
**Current automated evidence authority:** `docs/releases/AUTOMATED_BASELINE.md`  
**Production evidence standard:** `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`  
**Production evidence index:** `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

This document defines the stable end-to-end path from a source-complete release candidate to a public production release.

Do not pin a moving accepted source SHA, workflow run ID or test count here. Current exact-source automation belongs in `docs/releases/AUTOMATED_BASELINE.md` and its dated verification record.

## 1. Release principle

A build is not production-approved merely because it compiles or passes tests.

Public promotion requires applicable evidence for:

- exact-source tests/builds;
- CodeQL;
- unsuppressed dependency audit;
- store-candidate configuration;
- store-safe payload inspection;
- real platform/device behavior;
- packaged existing-data/encrypted-data compatibility;
- accessibility;
- security/privacy review;
- production signing/notarization;
- final package checksum/provenance;
- current store policy/metadata/declarations;
- exact immutable release tag;
- tagged release gates;
- store submission/approval/publication where intended.

Use `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` for `PASS`, `FAIL`, `BLOCKED`, `N/A` and `NOT RUN` semantics. Unknown/stale/unperformed work cannot be treated as passed.

## 2. Current automated baseline

Read:

`docs/releases/AUTOMATED_BASELINE.md`

That dynamic record identifies the latest actually observed accepted exact-source automated boundary.

If verification-relevant source moves beyond it, follow `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` and record only results actually produced by the replacement exact source.

Do not infer a test count from source inspection.

## 3. Freeze intended scope

Before production work:

1. choose intended version/build;
2. freeze product scope;
3. stop unrelated feature work;
4. review `PROJECT_STATUS.md`;
5. review `docs/releases/NEXT_STEPS.md`;
6. review `docs/releases/RELEASE_CHECKLIST.md`;
7. review `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`;
8. review `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`;
9. review `docs/releases/STORE_POLICY_REVIEW_20260818.md`;
10. review `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`;
11. identify every applicable automated/manual/package/accessibility/signing/store blocker;
12. confirm the Gumroad/Buy Me a Coffee-free application-package boundary remains intact;
13. confirm dependency audit remains unsuppressed under current policy.

## 4. Development preflight

On a fully provisioned host run:

```bash
build/scripts/release-preflight.sh
```

or:

```powershell
./build/scripts/release-preflight.ps1
```

Use the documented `CARENEST_TARGET` mechanism for explicit MAUI targets.

There is no current in-app funding/storefront toggle. Gumroad and Buy Me a Coffee remain repository/documentation-only under the RC1 package policy.

## 5. Exact-source automated verification

When runtime, test, project, package, workflow, build script, platform or stable release-policy source changes:

1. finish intended changes;
2. freeze exact candidate SHA;
3. follow `VERIFICATION_BRANCH_PROTOCOL.md` where applicable;
4. require the full applicable exact-source matrix;
5. fix actual failures instead of weakening policy;
6. retain exact source/run/test evidence;
7. preserve failed/cancelled/superseded evidence accurately.

Dynamic evidence/status documentation may record a completed result after successful exact-source verification according to `VERIFICATION_BRANCH_PROTOCOL.md` without changing the frozen executable source boundary.

## 6. Required automated gates

For a verification-relevant candidate require, as applicable:

- CareNest CI;
- platform-neutral formatting;
- unit tests;
- integration tests;
- UI/source-policy tests;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- CodeQL;
- unsuppressed Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts.

Production-style `v*` tags additionally require:

- Release Gate;
- Release Evidence.

No single workflow substitutes for another required gate.

## 7. Production validation records

Use release-specific copies of the templates indexed by `PRODUCTION_EVIDENCE_INDEX.md`.

Canonical templates cover:

- Android;
- Windows;
- iPhone/iPad;
- Mac Catalyst;
- accessibility;
- packaged compatibility;
- signing provenance;
- store submission/review/publication;
- final production approval.

Canonical templates remain unperformed. Release-specific copies contain actual evidence.

## 8. Manual platform validation

Validate representative installed behavior for:

### Android

Permission, actual reminder delivery, exact/inexact alarm behavior, battery/vendor restrictions, reboot/restart/time-zone recovery, reminder actions/snooze/reconciliation, files, backup, app lock and accessibility.

### Windows

Installed package behavior, core flows, running/closed-app reminder limits, timer replacement/cancellation, files, backup, app lock, keyboard/focus and themes/accessibility.

### iPhone/iPad

Real devices for permission/delivery, lifecycle/time-zone behavior, reminder actions, files, backup, app lock, Dynamic Type, VoiceOver and notification-preview privacy.

Simulator compilation is not real-device notification evidence.

### Mac Catalyst

Installed notification/lifecycle behavior, files, backup, app lock, keyboard/focus, themes/accessibility and signed/notarized behavior when applicable.

## 9. Packaged compatibility

Using fictional/synthetic representative prior data:

- verify SQLite opens and passes integrity checks;
- verify schema/migration behavior;
- verify representative data remains readable/editable;
- verify reminder rebuild/reconciliation;
- verify encrypted documents;
- verify current backup create/inspect/restore;
- verify wrong-password/tamper/truncation/trailing-data rejection;
- verify clean-install restore;
- test genuine historical encrypted fixtures only where genuine prior bytes safely exist.

Do not manufacture a new artifact and call it historical evidence.

Use `docs/releases/templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`.

## 10. Accessibility qualification

Use actual representative assistive technology for:

- screen readers;
- reading/focus order;
- large text/scaling;
- keyboard/focus;
- contrast/themes;
- color-independent meaning;
- reduced motion;
- privacy-safe errors.

Automated XAML/source semantics are not accessibility certification.

## 11. Security/privacy review

Review:

- `SECURITY.md`;
- `PRIVACY.md`;
- `docs/security/SECURITY_MODEL.md`;
- `docs/security/THREAT_MODEL.md`;
- `docs/security/LOGGING_PRIVACY.md`;
- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SECURITY_RELEASE_REVIEW.md`;
- `docs/releases/STORE_POLICY_REVIEW_20260818.md`.

Confirm no medical/clinical claims cross the product boundary and no required account/cloud/telemetry behavior is introduced without explicit review.

## 12. External-commerce application-package boundary

Current invariant:

- no external Buy Me a Coffee destination/card/command/artwork in the distributed application package;
- no external Gumroad destination/card/command/artwork in the distributed application package;
- repository-only support/storefront surfaces remain separate;
- purchase/funding never changes health/reminder/medical behavior or local-health-data access.

Repository-only markers:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

Final signed packages must be scanned again/equivalently inspected for both markers.

## 13. Production signing

Signing material stays outside Git.

Record only safe non-secret provenance using `docs/releases/templates/SIGNING_PROVENANCE_RECORD.md`.

For each channel verify package identity/version, signing/notarization state where applicable and final package checksum without committing private keys, keystores, certificate private material, passwords, service credentials, tokens or recovery codes.

## 14. Final package evidence

For every production candidate retain:

- exact source SHA/tag;
- version/build/application identity;
- filename;
- SHA-256;
- non-secret signing/notarization/store provenance;
- BMC/Gumroad forbidden-marker scan results;
- installed smoke/manual result;
- structured package evidence JSON.

Use `build/scripts/create-package-evidence.py --stage production` according to `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`.

The tool does not sign packages or prove store approval.

## 15. Store metadata/policy review

Preliminary review:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

At actual submission time re-open current official Apple/Google/Microsoft rules as applicable and complete live store-console forms against the exact production binary/listing.

Validate organizer/non-clinical claims, reminder limitations, privacy/data safety, permissions/capabilities, fictional screenshots, support/privacy/terms/security links and package identity.

For Google Play complete the live Health apps declaration and Data safety form where applicable.

Record review date, official sources, conclusions and any required changes.

## 16. Release metadata

Only after candidate selection:

- finalize display version/build;
- update `CHANGELOG.md`;
- prepare release notes;
- update current dynamic status/evidence references;
- avoid verification-relevant changes after the final source freeze.

If verification-relevant source changes, repeat exact-source verification.

## 17. Create exact production tag

Only after applicable pre-tag blockers are complete:

1. freeze exact approved commit;
2. create immutable approved `v*` tag;
3. require tagged CareNest CI;
4. require tagged CodeQL;
5. require tagged Dependency Audit;
6. require tagged Store Package Configuration;
7. require tagged Store Inspection Artifacts;
8. require tagged Release Gate;
9. require tagged Release Evidence.

Do not move/reuse a failed production tag for a different source.

## 18. Release Evidence and approval

Follow `docs/releases/RELEASE_EVIDENCE.md` for automated/manual/security/store/signing/package evidence.

Use a release-specific copy of `docs/releases/templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md` for the final decision.

Artifact existence alone is not approval. Green automation is necessary but not sufficient.

## 19. Publication

After all required tag/manual/package/accessibility/signing/store gates pass:

- confirm signed artifacts originate from exact tagged source;
- confirm package evidence/checksums/provenance;
- publish GitHub release where intended;
- submit/promote store packages where intended;
- record submission/review/approval/publication evidence;
- update dynamic status/changelog/next-steps/handoff documentation.

## 20. Post-release and hotfixes

Use explicit support, issue and security-reporting channels. Never ask users to publish real health records, backups, passwords/PINs/keys or secrets.

For a production defect:

1. reproduce safely;
2. add regression coverage;
3. make the smallest correct fix;
4. update affected documentation;
5. run exact-source gates;
6. repeat affected manual/package checks;
7. rebuild/sign corrected candidate;
8. regenerate package evidence;
9. use a new approved version/tag;
10. publish only after required gates pass.

## 21. Release blocker rule

Any required gate that is failed, unknown, stale, blocked or not actually performed blocks production promotion unless explicitly documented as `N/A` with a defensible reason under `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`.

CareNest must never be described as globally bug-free, medically authoritative, production-signed, store-approved or production-published merely because CI is green.
