# CareNest Release Checklist

**Release line:** `1.0.0-rc.1`  
**Current automated evidence authority:** `AUTOMATED_BASELINE.md`  
**Current store-policy review:** `STORE_POLICY_REVIEW_20260818.md`  
**Production evidence standard:** `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`  
**Production evidence index:** `PRODUCTION_EVIDENCE_INDEX.md`  
**Package evidence guide:** `PACKAGE_EVIDENCE_TOOLING.md`

This checklist is the stable production release checklist for CareNest `1.0.0-rc.1`. It separates source/product readiness from production work that requires exact-source automation, real packages, devices, assistive technology, signing identities, live store accounts and current store policy.

Do not hard-code a moving accepted SHA, workflow run ID or test total in this stable checklist. Read the latest actually observed accepted source/result from `AUTOMATED_BASELINE.md`.

A checkbox may be marked complete only from real evidence. A build, simulator compile or canonical template is not a substitute for a manual production result.

## 1. Exact-source automated verification — required for final candidate

Before production approval:

- [ ] Record the exact approved source SHA/tag.
- [ ] Confirm `AUTOMATED_BASELINE.md` describes automation that actually ran for the exact candidate, or run a fresh exact-source matrix.
- [ ] CareNest CI passes.
- [ ] Repository Python tooling syntax/self-tests pass.
- [ ] Documentation-integrity tooling self-test and stable active-link check pass.
- [ ] Platform-neutral formatting passes.
- [ ] Unit tests pass; record actual count.
- [ ] Integration tests pass; record actual count.
- [ ] UI/source-policy tests pass; record actual count.
- [ ] Android Release build passes.
- [ ] Windows Release build passes.
- [ ] iOS simulator Release build passes.
- [ ] Mac Catalyst Release build passes.
- [ ] Store Package Configuration passes on every configured target.
- [ ] Store Inspection Artifacts passes.
- [ ] CodeQL passes.
- [ ] Unsuppressed Dependency Audit passes.
- [ ] No required check is failed, stale, cancelled, skipped, superseded or merely queued.

If verification-relevant source changes after the accepted automated boundary, follow `VERIFICATION_BRANCH_PROTOCOL.md` and record only the result actually observed for the replacement exact source.

## 2. Source/product scope — complete for RC1

- [x] Local-first/account-free CareNest scope implemented.
- [x] Profiles and local family/caregiver organization implemented.
- [x] Medicines preserve user-entered strength/instruction text without dosage inference.
- [x] Explicit schedules, selected weekdays, cycles, intervals, date ranges and as-needed records implemented.
- [x] Reminder occurrence planning/reconciliation/compensation behavior implemented.
- [x] Taken/Skipped/Delayed/Missed/Snooze workflows implemented.
- [x] Appointments and optional reminders implemented.
- [x] Stock/refill organization remains driven by explicit user-entered quantities.
- [x] Encrypted local document storage implemented.
- [x] Password-encrypted manual backup/restore implemented.
- [x] Optional local app lock implemented.
- [x] Reports and explicit exports implemented with non-clinical limitations.
- [x] Settings, notification diagnostics, time-zone diagnostics and sanitized logs implemented.
- [x] Light/dark/system theme support implemented.
- [x] Accessibility-oriented source contracts implemented.
- [x] Localization-ready string architecture implemented.
- [x] No diagnosis feature.
- [x] No treatment recommendation feature.
- [x] No dosage calculation/inference feature.
- [x] No clinical interaction/risk-scoring feature.
- [x] No required CareNest backend/cloud sync.
- [x] No hidden runtime analytics/telemetry client.

Do not add speculative health features merely to increase commit count. A new runtime change requires a real reproduced defect or changed requirement.

## 3. Backup resource hardening — source complete

Current default limits:

- decrypted ZIP container: **2304 MiB** maximum;
- manifest: **1 MiB** maximum;
- SQLite database: **1 GiB** maximum;
- each encrypted document entry: **512 MiB** maximum;
- total uncompressed ZIP payload: **2 GiB** maximum;
- document count: **5,000** maximum;
- archive-entry count: document ceiling plus fixed required-entry allowance;
- explicit directory-only ZIP entries: rejected.

Completed source work:

- [x] Bounded decrypted authenticated output before ZIP parsing.
- [x] Archive-entry count validated before manifest parsing.
- [x] Oversized manifest/database/document entries rejected.
- [x] Excessive total uncompressed payload rejected.
- [x] Unsafe configured document-count ceilings rejected safely.
- [x] Generated backups validated against the same current restore boundary before encryption.
- [x] Current framing v2 tamper/truncation/trailing-data handling retained.
- [x] Legacy framing v1 read compatibility retained under caller-provided plaintext limits.
- [x] Focused integration regression coverage exists for the bounded backup path.

Packaged compatibility remains a separate production requirement.

## 4. Production evidence preparation — complete

The repository contains reusable evidence rules/templates. These files are preparation only and do not prove a release has been tested.

- [x] Production validation evidence standard: `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`.
- [x] Production evidence index: `PRODUCTION_EVIDENCE_INDEX.md`.
- [x] Android device validation template.
- [x] Windows validation template.
- [x] iPhone/iPad validation template.
- [x] Mac Catalyst validation template.
- [x] Accessibility validation template.
- [x] Packaged compatibility validation template.
- [x] Signing/provenance template.
- [x] Store submission/policy/publication template.
- [x] Final production release approval template.

Templates live under `templates/` and must remain visibly unperformed. Create release-specific copies for actual evidence.

## 5. Packaged existing-data/document/backup compatibility — required

Use fictional/synthetic data only. Record results with `templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`.

- [ ] Prepare representative earlier-candidate data.
- [ ] Record origin version/build/source/schema where known.
- [ ] Upgrade/install through each realistic production package path.
- [ ] Confirm SQLite opens successfully.
- [ ] Run and record SQLite integrity validation.
- [ ] Confirm representative profiles/medicines/schedules/occurrences/logs/appointments/stock/documents/tags/settings remain readable.
- [ ] Confirm representative editable records remain editable.
- [ ] Confirm expected schema version/migrations.
- [ ] Confirm reminder rebuild/reconciliation after upgrade.
- [ ] Confirm no duplicate/stale platform request is stranded in the tested boundary.
- [ ] Verify packaged encrypted-document import/open/export/delete.
- [ ] Verify failed export cleanup.
- [ ] Verify missing/corrupt document-key fail-closed behavior where safely testable.
- [ ] Verify packaged encrypted backup creation.
- [ ] Verify backup inspection/restore.
- [ ] Verify wrong-password rejection.
- [ ] Verify tamper rejection.
- [ ] Verify truncation rejection.
- [ ] Verify trailing-data rejection.
- [ ] Verify representative normal packaged backups remain comfortably below current resource ceilings.
- [ ] Verify clean-install restore.
- [ ] Verify restored encrypted documents remain usable.
- [ ] Test genuine historical encrypted backup bytes only where genuine prior bytes safely exist.

Never manufacture a current artifact and label it historical evidence.

Runbook: `PACKAGED_RELEASE_VALIDATION.md`.

## 6. Android real-device validation — required

Record each tested device/build with `templates/ANDROID_DEVICE_VALIDATION_RECORD.md`.

- [ ] Fresh install/onboarding.
- [ ] Notification permission denied/granted.
- [ ] Medicine reminder create/edit/delete.
- [ ] Appointment reminder create/edit/delete.
- [ ] Actual reminder delivery.
- [ ] Taken/Skipped/Delayed/Missed behavior.
- [ ] Snooze cancellation/replacement and future-snooze edge cases.
- [ ] Stale-request cleanup after schedule edits.
- [ ] Medicine/profile deletion cleanup.
- [ ] App restart/reopen recovery.
- [ ] Device reboot rebuild.
- [ ] Exact/inexact alarm behavior.
- [ ] Battery-optimization/vendor restrictions.
- [ ] Clock/time-zone/DST recovery.
- [ ] Force-stop limitation messaging.
- [ ] Document picker/share.
- [ ] Backup/restore.
- [ ] App lock.
- [ ] TalkBack/large-text/accessibility checks.

## 7. Windows validation — required

Record each intended Windows boundary with `templates/WINDOWS_VALIDATION_RECORD.md`.

- [ ] Intended install/execution path.
- [ ] Core CRUD/navigation.
- [ ] Running-app reminder behavior.
- [ ] Closed-app limitation behavior/messaging.
- [ ] Same-ID timer replacement/cancellation.
- [ ] Reminder actions/snooze/reconciliation.
- [ ] Restart/recovery.
- [ ] Document picker/share.
- [ ] Backup/restore.
- [ ] App lock.
- [ ] Keyboard/focus.
- [ ] Narrator where applicable.
- [ ] Light/dark/system themes.

## 8. iPhone/iPad real-device validation — required

Record each real-device boundary with `templates/IOS_DEVICE_VALIDATION_RECORD.md`.

- [ ] Signed/provisioned real-device install.
- [ ] Notification permission denied/granted.
- [ ] Actual medicine reminder delivery.
- [ ] Actual appointment reminder delivery.
- [ ] Reminder actions/snooze/reconciliation.
- [ ] Restart/lifecycle behavior.
- [ ] Time-zone/DST behavior.
- [ ] Document picker/share.
- [ ] Backup/restore.
- [ ] App lock.
- [ ] Dynamic Type.
- [ ] VoiceOver.
- [ ] Notification-preview privacy.

Simulator compilation is not a substitute for real-device notification evidence.

## 9. Mac Catalyst validation — required

Record each intended Mac boundary with `templates/MACCATALYST_VALIDATION_RECORD.md`.

- [ ] Intended install/execution path.
- [ ] Notification permission/delivery.
- [ ] Reminder actions/snooze/reconciliation.
- [ ] Restart/lifecycle behavior.
- [ ] Time-zone behavior.
- [ ] File picker/share.
- [ ] Backup/restore.
- [ ] App lock.
- [ ] Keyboard/focus.
- [ ] VoiceOver/large-text checks.
- [ ] Theme/contrast.
- [ ] Signed/notarized candidate behavior when available.

## 10. Accessibility validation — required

Use `templates/ACCESSIBILITY_VALIDATION_RECORD.md` for representative platform/assistive-technology combinations.

- [ ] Representative screen-reader validation.
- [ ] Reading order/names/roles/states/hints.
- [ ] Large text/display scaling.
- [ ] Destructive confirmation readability.
- [ ] Desktop keyboard/focus.
- [ ] Light/dark/system contrast.
- [ ] Color-independent meaning.
- [ ] Reduced-motion behavior.
- [ ] Privacy-safe actionable errors.

Automated source checks do not replace assistive-technology validation.

## 11. Production signing/notarization — required outside Git

Record only safe public provenance with `templates/SIGNING_PROVENANCE_RECORD.md`.

- [ ] Android production signing configured outside Git.
- [ ] Apple certificate/provisioning/store signing configured outside Git.
- [ ] Windows production signing configured outside Git where applicable.
- [ ] Safe public fingerprints/identifiers recorded where appropriate.
- [ ] Signing timestamp/source SHA/package checksum recorded.
- [ ] Final post-signing package SHA-256 recorded.
- [ ] No private signing material committed.

## 12. Structured final-package evidence — required

Guide: `PACKAGE_EVIDENCE_TOOLING.md`.

For every intended production artifact:

- [ ] Use `build/scripts/create-package-evidence.py --stage production` or the Bash/PowerShell wrapper.
- [ ] Require immutable `v*` source tag.
- [ ] Require source tag to resolve to recorded source SHA.
- [ ] Require checked-out HEAD to equal recorded source SHA.
- [ ] Require clean tracked workspace.
- [ ] Provide only non-secret real signing/notarization/store-managed provenance text.
- [ ] Require store-safe payload scanner to pass.
- [ ] Record per-file SHA-256 evidence.
- [ ] Record top-level package/directory payload SHA-256.
- [ ] Keep generated JSON outside the package payload.
- [ ] Independently cross-check package-evidence payload SHA-256.
- [ ] Retain JSON with final release evidence.

The package-evidence tool does not sign artifacts or prove store approval.

## 13. Final signed-package inspection — required

For every intended production package:

- [ ] Exact source SHA/tag recorded.
- [ ] Version/build/application identity recorded.
- [ ] Package filename and SHA-256 recorded.
- [ ] Signing/notarization/store-managed provenance recorded.
- [ ] Package-evidence JSON and payload SHA-256 recorded.
- [ ] `buymeacoffee.com/sanskarIN` forbidden-marker scan passed.
- [ ] `ramsandesh.gumroad.com` forbidden-marker scan passed.
- [ ] No Gumroad/Buy Me a Coffee promotional artwork/card/command exists in the distributed app payload.
- [ ] Intended repository/support/legal links remain accurate.
- [ ] Installed package starts.
- [ ] Representative platform smoke tests pass.

The official repository storefront remains `https://ramsandesh.gumroad.com`; it is intentionally outside the distributed health-app package under the current policy.

## 14. Store metadata/policy/submission — required

Use `templates/STORE_SUBMISSION_RECORD.md` per intended store.

Preliminary review: `STORE_POLICY_REVIEW_20260818.md`.

- [x] Preliminary Apple policy review completed on 2026-08-18.
- [x] Preliminary Google Play health/Data safety guidance review completed on 2026-08-18.
- [x] Preliminary Microsoft sensitive-personal-information/privacy review completed on 2026-08-18.
- [ ] Re-open official Apple policy on actual submission date where applicable.
- [ ] Re-open official Google Play policy on actual submission date where applicable.
- [ ] Re-open official Microsoft/Windows policy on actual submission date where applicable.
- [ ] Complete live Google Play Health apps declaration where applicable.
- [ ] Complete live Google Play Data safety answers where applicable.
- [ ] Complete Apple privacy/store metadata where applicable.
- [ ] Complete Microsoft/Partner Center privacy/store metadata where applicable.
- [ ] Verify final listing claims/disclaimers/reminder limitations.
- [ ] Verify screenshots use fictional data and match exact package.
- [ ] Verify support/privacy/terms/security destinations.
- [ ] Record submission separately from approval/publication.
- [ ] Resolve every rejection/change request before production approval.

Store policies are time-sensitive; preliminary review is not store approval.

## 15. Freeze exact production source and tag — required

Only after applicable manual/package/accessibility/signing/store findings are resolved:

- [ ] Select exact approved production commit.
- [ ] Confirm the selected source has current exact-source automation in `AUTOMATED_BASELINE.md`, or run fresh verification.
- [ ] Verify final version/build metadata.
- [ ] Verify final release notes/changelog.
- [ ] Verify signed-package hashes/provenance/package-evidence JSON.
- [ ] Ensure no unresolved production blocker remains.
- [ ] Create immutable approved `v*` tag.

Do not move a failed/rejected production tag to a different source merely to reuse its version identity.

## 16. Tagged production gates — required

For the immutable approved `v*` tag:

- [ ] Tagged CareNest CI succeeds.
- [ ] Tagged CodeQL succeeds.
- [ ] Tagged Dependency Audit succeeds.
- [ ] Tagged Store Package Configuration succeeds.
- [ ] Tagged Store Inspection Artifacts succeeds.
- [ ] Tagged Release Gate succeeds.
- [ ] Tagged Release Evidence succeeds.
- [ ] Exact tagged run IDs/artifacts/checksums are retained in the release-specific evidence record.

## 17. Production approval — required

Use a release-specific copy of `templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`.

- [ ] Applicable automated evidence is current.
- [ ] Applicable packaged compatibility evidence passes.
- [ ] Applicable platform/device evidence passes.
- [ ] Applicable accessibility evidence passes.
- [ ] Applicable signing/notarization evidence passes.
- [ ] Applicable final-package evidence passes.
- [ ] Applicable store/policy blockers are resolved.
- [ ] Approved source/tag/package hashes are recorded explicitly.
- [ ] Final approval decision is recorded.

## 18. Publication — required

- [ ] GitHub release published where intended.
- [ ] Store packages submitted/promoted where intended.
- [ ] Store approval/publication evidence recorded.
- [ ] Final public version/build matches approved package hashes.
- [ ] Final status/changelog/next-steps updated.
- [ ] Support/security monitoring channels confirmed.

## Final rule

A failed, unknown, stale, blocked or unperformed required gate blocks production promotion unless it is explicitly and defensibly recorded as not applicable under `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`.

CareNest remains `1.0.0-rc.1` until applicable production rows are actually evidenced.
