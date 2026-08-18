# CareNest Release Process

**Release line:** `1.0.0-rc.1`  
**Latest verified Gumroad implementation/source-policy source:** `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

This document defines the end-to-end path from a source-complete release candidate to a public production release. Automated evidence, real-device evidence, package compatibility, accessibility, signing, structured package provenance, store-console declarations, current policy review and store approval are separate gates.

## 1. Release principle

A build is not production-approved merely because it compiles or passes tests.

Public promotion requires the exact candidate source to satisfy applicable:

- formatting/tests/builds;
- CodeQL;
- unsuppressed Dependency Audit;
- store-candidate configuration builds;
- package inspection/provenance;
- real-device/platform behavior;
- packaged existing-data/encrypted-data compatibility;
- accessibility;
- security/privacy review;
- production signing;
- final signed-package inspection;
- structured final-package checksum/provenance evidence;
- current store-policy/metadata/declaration review;
- exact immutable release tag;
- tagged Release Gate/Release Evidence;
- publication evidence.

## 2. Current automated baseline

Latest exact verified Gumroad implementation/source-policy source:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

Verified on that exact source:

- 122/122 unit tests;
- 39/39 integration tests;
- 175/175 UI/source-policy tests;
- **336/336 total core tests**;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- Store Package Configuration on all four targets;
- CodeQL.

Authoritative current automated evidence:

`docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`

Permanent compiled-binding evidence remains:

`docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`

Older PR/source records remain historical evidence for their own exact boundaries and must not replace the current verified Gumroad baseline.

The repository now contains later verification-relevant release-documentation contracts, package-evidence tooling, and CI/release workflow changes. Those changes require a fresh exact-source verification before a newer source can replace the verified baseline above.

Do not infer a new test count from source inspection. Record the count actually produced by the exact verification run.

## 3. Freeze intended scope

Before production work:

1. choose intended version/build;
2. freeze product scope;
3. stop unrelated feature work;
4. review `PROJECT_STATUS.md`;
5. review `docs/releases/NEXT_STEPS.md`;
6. review `docs/releases/STORE_POLICY_REVIEW_20260818.md`;
7. review `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`;
8. identify every applicable automated/manual/package/accessibility/signing/store blocker;
9. confirm the Gumroad/Buy Me a Coffee-free application-package boundary remains intact;
10. confirm the former SQLite audit suppression remains absent.

## 4. Development preflight

On a fully provisioned host run:

```bash
build/scripts/release-preflight.sh
```

or:

```powershell
./build/scripts/release-preflight.ps1
```

The current preflight is intended to fail closed for required source hygiene, formatting, builds/tests and unsuppressed dependency audit.

For an explicit MAUI target, use the current `CARENEST_TARGET` mechanism documented in `docs/CONFIGURATION_REFERENCE.md`.

There is no current application funding/storefront build toggle. Gumroad and Buy Me a Coffee are repository/documentation-only under the current RC1 policy.

## 5. Exact-source automated verification

If verification-relevant runtime/test/project/package/workflow/build-script/platform/release-policy source changes after the accepted baseline:

1. finish the intended source;
2. freeze exact candidate SHA;
3. follow `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` where applicable;
4. require all configured exact-source gates;
5. fix actual failures instead of weakening policies;
6. record exact source/run/test evidence;
7. preserve failed evidence rather than re-labelling an unverified source as approved.

Documentation-only commits can sit above a verified executable source only when the documentation clearly distinguishes that verified source from later documentation-only heads and the changed docs are not verification-contract inputs.

## 6. Required automated gates

For a verification-relevant candidate require, as applicable:

- CareNest CI, including package-evidence Python syntax/self-test;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts.

Production-style `v*` tags additionally require:

- Release Gate, including package-evidence tooling validation;
- Release Evidence, including retained package-evidence tooling self-test output.

No single workflow substitutes for the other required release gates.

## 7. Manual platform matrix

Use `docs/releases/MANUAL_TEST_MATRIX.md` and `docs/PLATFORM_BEHAVIOR_MATRIX.md`.

### Android

Validate representative real/emulated targets for permission, actual reminder delivery, exact/inexact alarm behavior, battery/vendor restrictions, reboot/restart/time-zone recovery, reminder actions/snooze/reconciliation, files/backups/app lock and accessibility.

### Windows

Validate installed package behavior, core flows, running/closed-app reminder limitations, timer replacement/cancellation, files/backups/app lock, keyboard/focus and themes/accessibility.

### iPhone/iPad

Use real devices for permission/delivery, lifecycle/time-zone behavior, reminder actions, files/backups/app lock, Dynamic Type, VoiceOver and notification-preview privacy.

Simulator compilation is not real-device evidence.

### Mac Catalyst

Validate notifications/lifecycle, files/backups/app lock, keyboard/focus, themes/accessibility and signed/notarized behavior when available.

## 8. Packaged SQLite compatibility

With representative fictional prior data:

1. install/upgrade through realistic package path;
2. open database;
3. run integrity validation;
4. verify profiles/medicines/schedules/occurrences/logs/appointments/stock/documents/tags/settings;
5. verify records remain readable/editable;
6. verify schema version/migrations;
7. rebuild/reconcile reminders;
8. verify no duplicate/stale platform requests;
9. record package/source/checksum/result evidence.

A green dependency audit does not replace this gate.

## 9. Encrypted document/backup compatibility

Using fictional data:

- current encrypted document import/open/export/delete;
- failed export cleanup;
- missing/corrupt key fail-closed behavior;
- current backup create/inspect/restore;
- wrong-password rejection;
- tamper/truncation/trailing-data rejection;
- restored encrypted documents remain usable;
- clean-install restore;
- genuine historical fixtures when real prior bytes exist.

Do not manufacture a new artifact and label it historical evidence.

## 10. Accessibility qualification

Use representative assistive technology for:

- screen-reader names/order;
- large text/scaling;
- keyboard/focus;
- light/dark/system contrast;
- reduced motion;
- color-independent meaning;
- destructive confirmations;
- privacy-safe errors.

Source/XAML semantics are not enough.

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

Confirm no required account/cloud/telemetry was introduced without design review and no medical/clinical claims crossed the product boundary.

## 12. External-commerce application-package boundary

Current product invariant:

- no external Buy Me a Coffee destination/card/command/artwork in distributed application source/package;
- no external Gumroad destination/card/command/artwork in distributed application source/package;
- repository-only voluntary support/storefront surfaces may remain in repository documentation/metadata;
- purchase/funding never creates health/medical/reminder entitlement or local-health-data access;
- Store Inspection payload scanning remains defense-in-depth;
- final signed packages must be scanned again/equivalently inspected for both repository-only markers.

Current repository-only markers:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

Do not resurrect obsolete `CareNestShowFundingLink` or per-target external-commerce switches in the release process.

## 13. Production signing

Signing material stays outside Git.

### Android

Configure production keystore/signing service securely, build intended signed AAB/APK, verify identity/permissions and record safe signing provenance/fingerprint information.

### Apple

Configure certificates/provisioning securely, verify bundle/entitlements, archive/sign/notarize as applicable and record provenance.

### Windows

Configure production signing identity outside Git, produce intended signed package and record provenance.

Never commit private keys, certificates containing private material, keystores, passwords or signing-service credentials.

## 14. Final signed-package inspection and structured evidence

For every production candidate record:

- exact source SHA/tag;
- version/build;
- package identity;
- filename;
- SHA-256;
- signing/notarization/store provenance;
- final scan for `buymeacoffee.com/sanskarIN`;
- final scan for `ramsandesh.gumroad.com`;
- confirmation that no Gumroad/Buy Me a Coffee card/button/command/artwork exists in the installed app;
- About/legal/support-contact inspection;
- install/launch smoke test;
- platform-specific smoke/manual result.

Then generate a structured JSON record using:

```text
build/scripts/create-package-evidence.py
```

or the platform wrappers:

```text
build/scripts/create-package-evidence.sh
build/scripts/create-package-evidence.ps1
```

For final production evidence use `--stage production`. The tool requires:

- immutable `v*` source tag;
- tag SHA equals recorded source SHA;
- checked-out HEAD equals recorded source SHA;
- clean tracked workspace;
- non-secret signing/notarization/store provenance;
- successful store-safe scan;
- evidence output outside the package payload.

Retain the generated JSON and verify its payload SHA-256 against independently recorded package evidence.

The tool does not sign packages and cannot prove store approval. Internal CI inspection artifacts are not automatically production packages.

## 15. Store metadata/policy review

A dated preliminary review is recorded at:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

At actual submission time re-open the official current Apple/Google/Microsoft rules as applicable and complete the live store-console forms against the exact production binary/listing.

Validate:

- organizer/non-clinical claims;
- no diagnosis/treatment/dosage/clinical-risk claim;
- no unapproved medical-device claim;
- reminder limitation wording;
- privacy/data-safety declarations;
- screenshots with fictional data;
- permission/capability descriptions;
- support/privacy/terms/security links;
- final package identity/version;
- no listing screenshot/copy implies in-app Gumroad/Buy Me a Coffee functionality under the current package policy.

For Google Play specifically, complete the live Health apps declaration and Data safety form for the exact production feature/binary set.

For Apple, complete the current App Store privacy/store metadata for the exact production capabilities and package.

For Microsoft distribution, complete the current privacy/store metadata where applicable.

Record the submission-date policy sources, conclusions and required changes.

## 16. Release notes/version metadata

Only after candidate selection:

- finalize display version/build;
- update `CHANGELOG.md`;
- prepare release notes from `RELEASE_NOTES_TEMPLATE.md`;
- update current status/next steps/evidence references;
- avoid executable or verification-contract changes after final verification.

If verification-relevant source changes, repeat exact-source verification.

## 17. Create exact production tag

Only after applicable pre-tag blockers are complete:

1. freeze exact approved commit;
2. create intended immutable `v*` tag;
3. require tagged CareNest CI;
4. require tagged CodeQL;
5. require tagged Dependency Audit;
6. require tagged Store Package Configuration;
7. require tagged Store Inspection Artifacts;
8. require tagged Release Gate;
9. require tagged Release Evidence.

Do not move/reuse a failed/rejected production tag to point at different source.

## 18. Release Evidence

Release Evidence must follow `docs/releases/RELEASE_EVIDENCE.md` and should record exact source/ref/run/attempt identity, source manifests/checksums, test/dependency evidence, package-evidence tooling self-test evidence, workspace integrity and evidence checksums according to the current workflow.

Artifact existence alone is not approval; the workflow conclusion and provenance must be accepted.

Final signed-package structured provenance belongs in the release record in addition to CI evidence.

## 19. Publication

After all required tag/manual/package/accessibility/signing/store gates pass:

- confirm signed artifacts originate from exact tagged source;
- confirm package evidence JSON/checksums/provenance;
- publish GitHub release as appropriate;
- submit/promote store packages;
- record final publication/store-approval evidence;
- update `PROJECT_STATUS.md`, `NEXT_STEPS.md`, `CHANGELOG.md` and handoff documentation.

## 20. Post-release monitoring

CareNest v1 has no hidden telemetry feedback loop.

Use explicit channels such as GitHub Issues, support email and the security-reporting process.

Never ask users to publicly upload real health records, backups, PINs/passwords/keys or other secrets.

## 21. Hotfix process

For a production defect:

1. reproduce safely;
2. add regression coverage;
3. make smallest correct fix;
4. update documentation;
5. run affected/full exact-source gates;
6. repeat applicable manual/package checks;
7. build/sign corrected candidate;
8. regenerate structured package evidence for the corrected package;
9. create a new approved version/tag;
10. require tagged gates;
11. publish patch release.

## 22. Release blocker rule

Any required gate that is failed, unknown, stale or not actually performed blocks production promotion unless explicitly documented as non-applicable with a defensible reason.

CareNest must never be described as globally bug-free, medically authoritative, production-signed, store-approved or production-published merely because CI is green.
