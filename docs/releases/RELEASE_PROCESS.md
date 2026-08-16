# CareNest Release Process

**Release line:** `1.0.0-rc.1`  
**Current verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`  
**Verified PR #74 head:** `8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

This document defines the end-to-end path from a source-complete release candidate to a public production release. Automated evidence, real-device evidence, package compatibility, accessibility, signing and store approval are separate gates.

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
- current store-policy/metadata review;
- exact immutable release tag;
- tagged Release Gate/Release Evidence;
- publication evidence.

## 2. Current automated baseline

PR #74 frozen source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Verified:

- 122/122 unit tests;
- 39/39 integration tests;
- 170/170 UI/source-policy tests;
- **331/331 total core tests**;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- Store Package Configuration on all four targets;
- Store Inspection Artifacts;
- CodeQL;
- unsuppressed Dependency Audit.

Permanent evidence: `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

Older PR #68/#67/#61/#59/#58/#56/#54 records remain historical evidence for their own source boundaries.

## 3. Freeze intended scope

Before production work:

1. choose intended version/build;
2. freeze product scope;
3. stop unrelated feature work;
4. review `PROJECT_STATUS.md`;
5. review `docs/releases/NEXT_STEPS.md`;
6. identify every applicable manual/package/accessibility/signing/store blocker;
7. confirm the funding-free application-package boundary remains intact;
8. confirm the former SQLite audit suppression remains absent.

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

There is no current application funding-link build toggle.

## 5. Exact-source automated verification

If verification-relevant runtime/test/project/package/workflow/build-script/platform source changes after the accepted baseline:

1. finish the intended source;
2. freeze exact candidate SHA;
3. follow `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`;
4. require all configured PR-head gates;
5. fix actual failures instead of weakening policies;
6. record exact source/run/test evidence;
7. keep verification markers out of `main` when the protocol requires marker-only closure.

Documentation-only commits can sit above a verified executable source if comparison proves no verification-relevant executable delta.

## 6. Required automated gates

For a verification-relevant candidate require, as applicable:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts.

Production-style `v*` tags additionally require:

- Release Gate;
- Release Evidence.

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
- `docs/releases/SECURITY_RELEASE_REVIEW.md`.

Confirm no required account/cloud/telemetry was introduced without design review and no medical/clinical claims crossed the product boundary.

## 12. Application funding/package boundary

Current product invariant:

- no external Buy Me a Coffee destination/card/command/artwork in distributed application source/package;
- repository-only voluntary project support may remain;
- project funding never creates health/medical/reminder entitlement;
- Store Inspection payload scanning remains defense-in-depth.

Do not resurrect obsolete `CareNestShowFundingLink`/funding-disabled build instructions in the release process.

## 13. Production signing

Signing material stays outside Git.

### Android

Configure production keystore/signing service securely, build intended signed AAB/APK, verify identity/permissions and record safe signing provenance/fingerprint information.

### Apple

Configure certificates/provisioning securely, verify bundle/entitlements, archive/sign/notarize as applicable and record provenance.

### Windows

Configure production signing identity outside Git, produce intended signed package and record provenance.

Never commit private keys/certificates containing private material/keystores/passwords.

## 14. Final signed-package inspection

For every production candidate record:

- exact source SHA/tag;
- version/build;
- package identity;
- filename;
- SHA-256;
- signing/notarization/store provenance;
- forbidden external-funding marker scan;
- About/legal/support-contact inspection;
- install/launch smoke test;
- platform-specific smoke/manual result.

Internal CI inspection artifacts are not automatically production packages.

## 15. Store metadata/policy review

At actual submission time review current Apple/Google/Microsoft rules as applicable.

Validate:

- organizer/non-clinical claims;
- reminder limitation wording;
- privacy/data-safety declarations;
- screenshots with fictional data;
- permission/capability descriptions;
- support/privacy/terms/security links;
- final package identity/version;
- no listing screenshot/copy implies removed in-app funding surface.

## 16. Release notes/version metadata

Only after candidate selection:

- finalize display version/build;
- update `CHANGELOG.md`;
- prepare release notes from `RELEASE_NOTES_TEMPLATE.md`;
- update current status/next steps/evidence references;
- avoid executable changes after final verification.

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

Release Evidence should record exact source/ref/run/attempt identity, source manifests/checksums, test/dependency evidence, workspace integrity and evidence checksums according to the current workflow.

Artifact existence alone is not approval; the workflow conclusion and provenance must be accepted.

## 19. Publication

After all required tag/manual/package/accessibility/signing/store gates pass:

- confirm signed artifacts originate from exact tagged source;
- confirm checksums/provenance;
- publish GitHub release as appropriate;
- submit/promote store packages;
- record final publication evidence;
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
8. create a new approved version/tag;
9. require tagged gates;
10. publish patch release.

## 22. Release blocker rule

Any required gate that is failed, unknown, stale or not actually performed blocks production promotion unless explicitly documented as non-applicable with a defensible reason.

CareNest must never be described as globally bug-free, medically authoritative, production-signed or store-approved merely because CI is green.