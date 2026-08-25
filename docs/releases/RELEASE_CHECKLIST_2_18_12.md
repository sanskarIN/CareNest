# CareNest 2.18.12 Release Checklist

**Target:** `2.18.12`  
**Build/package code:** `21812`  
**State:** PREPARATION — NOT RELEASED

This checklist supplements the stable release authorities. It must not replace `RELEASE_CHECKLIST.md`, `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`, `PRODUCTION_EVIDENCE_INDEX.md` or the canonical evidence templates.

## Source and metadata

- [x] Central semantic version set to `2.18.12`.
- [x] Assembly/file version set to `2.18.12.0`.
- [x] MAUI display version set to `2.18.12`.
- [x] MAUI package/build code set to `21812`.
- [x] `Microsoft.Maui.Controls` baseline set to `10.0.100`.
- [x] Version/dependency consistency contracts added.
- [x] Draft release notes added.
- [x] Version preparation record added.
- [x] Active `what_changed.md` handoff refreshed.

## Pull request acceptance

Accepted PR #84 branch source: `1d9de89fbc7de69696c9d4276991f07bcdce1027`.

- [x] Exact final PR #84 source completed successful CareNest CI.
- [x] Exact final PR #84 source completed successful CodeQL.
- [x] Exact final PR #84 source completed successful unsuppressed Dependency Audit.
- [x] Exact final PR #84 source completed successful Store Package Configuration.
- [x] Exact final PR #84 source completed successful Store Inspection Artifacts.
- [x] Unit tests passed: **122/122**.
- [x] Integration tests passed: **54/54**.
- [x] UI/source-policy tests passed: **215/215**.
- [x] Total core tests passed: **391/391**.
- [x] Android Release build passed with MAUI `10.0.100`.
- [x] Windows Release build passed with MAUI `10.0.100` after a same-source job-only retry for a transient workload-download `ResponseEnded` error.
- [x] iOS simulator Release build passed with MAUI `10.0.100`.
- [x] Mac Catalyst Release build passed with MAUI `10.0.100`.
- [x] Linux desktop Release build passed.
- [x] WebAssembly browser Release publish passed.
- [x] Stable documentation-link verification passed: **210** live local links across **128** stable active Markdown files.
- [x] Platform-neutral formatting passed.

The Windows first-attempt infrastructure failure is retained in `AUTOMATED_BASELINE.md`; it is not erased by the successful unchanged-source retry.

## Repository follow-through

- [x] PR #84 merged only after the exact-head matrix was green.
- [x] PR #84 merged into `main` at `ca80bd554296363d71a6008cac73c819be77b39b`.
- [x] Close/supersede stale PR #83 in favor of current-main PR #84.
- [x] Integrate Dependabot PR #85 `Microsoft.Maui.Controls` `10.0.100` change into PR #84.
- [x] Close superseded PR #85 after preserving its exact-source verification boundary.
- [x] Promote the dynamic automated baseline only from actually observed results.
- [x] Align `PROJECT_STATUS.md`, `NEXT_STEPS.md` and `what_changed.md` with the accepted automated boundary.

## Production validation

These rows remain open because CI/build success is not a substitute for real runtime evidence.

- [ ] Android representative installed-device validation recorded.
- [ ] Windows installed-package/update validation recorded.
- [ ] iPhone/iPad signed/provisioned-device validation recorded.
- [ ] Mac Catalyst installed application validation recorded.
- [ ] Linux representative runtime validation recorded for every environment represented as supported.
- [ ] Browser/WebAssembly runtime validation recorded for every browser/deployment boundary represented as supported.
- [ ] Accessibility validation recorded with applicable assistive technologies.
- [ ] Packaged existing-data/SQLite/encrypted-document/backup compatibility recorded.
- [ ] Genuine historical-backup compatibility recorded only where genuine prior artifacts safely exist.

## Signing and artifact provenance

- [ ] Production signing/provisioning completed through secure platform tooling.
- [ ] Notarization evidence recorded where applicable.
- [ ] Exact final production package/deployment SHA-256/provenance generated.
- [ ] Store-safe payload scanner passes on exact final distributed app packages.
- [ ] Signing/provenance record completed without committing secrets.
- [ ] Browser deployment origin/TLS/ownership provenance recorded if browser deployment is promoted.
- [ ] Linux package/channel provenance recorded if Linux distribution is promoted.

Unsigned automated inspection artifacts are not signed production packages.

## Store, distribution and publication

- [ ] Live Google Play declarations reviewed against exact package/listing where applicable.
- [ ] Current Apple policy/listing requirements reviewed on submission day where applicable.
- [ ] Current Microsoft Store requirements reviewed on submission day where applicable.
- [ ] Actual Linux distribution-channel requirements reviewed where applicable.
- [ ] Actual browser hosting/privacy/security requirements reviewed where applicable.
- [ ] Store/deployment metadata, screenshots and privacy text reconciled with exact build.
- [ ] Submission/deployment state recorded.
- [ ] Review/rejection/remediation state recorded where applicable.
- [ ] Approval state recorded only after actual approval.
- [ ] Publication state recorded only after actual publication/deployment.
- [ ] Draft release notes reconciled with the exact production-approved source/package.
- [ ] Immutable `v2.18.12` tag created only when production gates permit it.
- [ ] Tagged release gates pass for the approved immutable tag.

## Final rule

CareNest `2.18.12` has completed its current automated source-acceptance boundary but remains **NOT RELEASED**.

It must not be described as production released, fully platform-parity verified, production signed, store approved or globally defect-free until the corresponding evidence actually exists for the exact source/package/deployment being promoted.