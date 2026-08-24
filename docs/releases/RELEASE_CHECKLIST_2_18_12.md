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
- [x] Version-consistency contract added.
- [x] Draft release notes added.
- [x] Version preparation record added.
- [x] Active `what_changed.md` handoff refreshed.

## Pull request acceptance

- [ ] Exact final PR #84 head has successful CareNest CI.
- [ ] Exact final PR #84 head has successful CodeQL.
- [ ] Exact final PR #84 head has successful unsuppressed Dependency Audit.
- [ ] Exact final PR #84 head has successful Store Package Configuration.
- [ ] Exact final PR #84 head has successful Store Inspection Artifacts.
- [ ] Unit tests pass on the exact final head.
- [ ] Integration tests pass on the exact final head.
- [ ] UI/source-policy tests pass on the exact final head.
- [ ] Android Release build passes on the exact final head.
- [ ] Windows Release build passes on the exact final head.
- [ ] iOS simulator Release build passes on the exact final head.
- [ ] Mac Catalyst Release build passes on the exact final head.
- [ ] Linux desktop Release build passes on the exact final head.
- [ ] WebAssembly browser Release publish passes on the exact final head.

Do not check any item from an older, cancelled, skipped, queued, failed or superseded run.

## Repository follow-through

- [ ] Merge PR #84 only after the exact-head matrix is green.
- [x] Close/supersede stale PR #83 in favor of current-main PR #84.
- [ ] Rebase Dependabot PR #85 onto the new `main`.
- [ ] Validate `Microsoft.Maui.Controls` `10.0.100` with the applicable MAUI matrix.
- [ ] Merge PR #85 only if its exact head is green and compatible.
- [ ] Promote the dynamic automated baseline only from actually observed results.

## Production validation

- [ ] Android representative installed-device validation recorded.
- [ ] Windows installed-package/update validation recorded.
- [ ] iPhone/iPad signed/provisioned-device validation recorded.
- [ ] Mac Catalyst installed application validation recorded.
- [ ] Linux representative runtime validation recorded.
- [ ] Browser/WebAssembly runtime validation recorded.
- [ ] Accessibility validation recorded with applicable assistive technologies.
- [ ] Packaged existing-data/SQLite/encrypted-document/backup compatibility recorded.

## Signing and artifact provenance

- [ ] Production signing/provisioning completed through secure platform tooling.
- [ ] Notarization evidence recorded where applicable.
- [ ] Exact final package SHA-256/provenance generated.
- [ ] Store-safe payload scanner passes on exact final packages.
- [ ] Signing/provenance record completed without committing secrets.

## Store and publication

- [ ] Live Google Play declarations reviewed against exact package/listing.
- [ ] Current Apple policy/listing requirements reviewed on submission day.
- [ ] Current Microsoft Store requirements reviewed on submission day.
- [ ] Store metadata/screenshots/privacy text reconciled with exact build.
- [ ] Submission state recorded.
- [ ] Review/rejection/remediation state recorded where applicable.
- [ ] Approval state recorded only after actual approval.
- [ ] Publication state recorded only after actual publication.
- [ ] Immutable `v2.18.12` tag created only when release gates permit it.

## Final rule

CareNest `2.18.12` must not be described as production released, fully platform-parity verified, store approved or globally defect-free until the corresponding evidence actually exists for the exact source and package being promoted.
