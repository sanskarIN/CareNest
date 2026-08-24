# CareNest Release Checklist

**Release line:** `1.0.0-rc.1`  
**Current automated evidence authority:** `AUTOMATED_BASELINE.md`  
**Current store-policy review:** `STORE_POLICY_REVIEW_20260818.md`  
**Production evidence standard:** `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`  
**Production evidence index:** `PRODUCTION_EVIDENCE_INDEX.md`  
**Package evidence guide:** `PACKAGE_EVIDENCE_TOOLING.md`  
**Cross-platform guide:** `../setup/CROSS_PLATFORM.md`

This checklist is the stable production release checklist for CareNest `1.0.0-rc.1`. It separates source/product readiness from production work that requires exact-source automation, real packages, devices/browsers, assistive technology, signing identities, live store accounts and current policy evidence.

Do not hard-code a moving accepted SHA, workflow run ID or test total in this stable checklist. Read the latest actually observed accepted source/result from `AUTOMATED_BASELINE.md`.

A checkbox may be marked complete only from real evidence. A build, simulator compile, Linux build, WebAssembly publish or canonical template is not a substitute for a manual production result.

## 1. Exact-source automated verification — required for final candidate

Before production approval:

- [ ] Record the exact approved source SHA/tag.
- [ ] Confirm `AUTOMATED_BASELINE.md` describes automation that actually ran for the exact candidate, or run a fresh exact-source matrix.
- [ ] CareNest CI passes.
- [ ] Repository Python tooling syntax/self-tests pass.
- [ ] Cross-platform target/evidence verifier passes.
- [ ] Cross-platform verifier regression self-tests pass.
- [ ] Documentation-integrity tooling self-test and stable active-link check pass.
- [ ] Platform-neutral formatting passes.
- [ ] Unit tests pass; record actual count.
- [ ] Integration tests pass; record actual count.
- [ ] UI/source-policy tests pass; record actual count.
- [ ] Android Release build passes.
- [ ] Windows Release build passes.
- [ ] iOS simulator Release build passes.
- [ ] Mac Catalyst Release build passes.
- [ ] Linux Avalonia desktop Release build passes.
- [ ] Avalonia Browser WebAssembly Release publish passes.
- [ ] Store Package Configuration passes on every configured store target.
- [ ] Store Inspection Artifacts passes for every configured inspection target.
- [ ] CodeQL passes.
- [ ] Unsuppressed Dependency Audit passes, including Avalonia desktop/browser graphs when present.
- [ ] No required check is failed, stale, cancelled, skipped, superseded or merely queued.

If verification-relevant source changes after the accepted automated boundary, follow `VERIFICATION_BRANCH_PROTOCOL.md` and record only the result actually observed for the replacement exact source.

A successful Linux/browser automated build is evidence of build reach only. It does not prove production persistence, reminders/background execution, secure storage, file/camera behavior, accessibility or full feature parity.

## 2. Source/product scope — complete for established RC1 MAUI runtime

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

Cross-platform presentation/build foundation in the current continuation:

- [x] Shared Avalonia application/view project configured.
- [x] Avalonia Desktop host configured for Linux-capable desktop execution.
- [x] Avalonia Browser WebAssembly host configured.
- [x] Linux/browser build reach is explicitly separated from full production feature parity.

Do not mark established MAUI feature rows as Linux/browser parity merely because the shared presentation host exists.

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
- [x] Android device validation template: `templates/ANDROID_DEVICE_VALIDATION_RECORD.md`.
- [x] Windows validation template: `templates/WINDOWS_VALIDATION_RECORD.md`.
- [x] iPhone/iPad validation template: `templates/IOS_DEVICE_VALIDATION_RECORD.md`.
- [x] Mac Catalyst validation template: `templates/MACCATALYST_VALIDATION_RECORD.md`.
- [x] Linux desktop validation template: `templates/LINUX_DESKTOP_VALIDATION_RECORD.md`.
- [x] Browser/WebAssembly validation template: `templates/BROWSER_VALIDATION_RECORD.md`.
- [x] Accessibility validation template: `templates/ACCESSIBILITY_VALIDATION_RECORD.md`.
- [x] Packaged compatibility validation template: `templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`.
- [x] Signing/provenance template: `templates/SIGNING_PROVENANCE_RECORD.md`.
- [x] Store submission/policy/publication template: `templates/STORE_SUBMISSION_RECORD.md`.
- [x] Final production release approval template: `templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`.

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

## 9A. Linux desktop validation — required when Linux distribution is intended

Use a release-specific copy of `templates/LINUX_DESKTOP_VALIDATION_RECORD.md`.

- [ ] Record source SHA/tag, publish path/runtime identifier and artifact hash/provenance.
- [ ] Record Linux distribution/version, desktop environment/display server and architecture.
- [ ] Launch the actual candidate and verify main-window rendering/lifecycle.
- [ ] Verify resize/high-DPI/display-scaling behavior.
- [ ] Verify filesystem/package/runtime prerequisite behavior for the intended distribution model.
- [ ] Record X11/Wayland behavior for the environment actually tested.
- [ ] Validate only implemented persistence/workflows; leave unsupported rows `NOT RUN`, `BLOCKED` or `N/A` as appropriate.
- [ ] Do not infer native/background notification behavior from MAUI source.
- [ ] Do not claim Linux secure storage unless a Linux-specific implementation was exercised.
- [ ] Perform applicable keyboard/focus/assistive-technology validation.
- [ ] Confirm local-first/privacy/external-commerce package boundaries remain accurate.

A Linux build success is not a Linux production-validation `PASS`.

## 9B. Browser/WebAssembly validation — required when browser distribution is intended

Use release-specific copies of `templates/BROWSER_VALIDATION_RECORD.md` for actually tested browsers.

- [ ] Record exact source, published artifact/provenance and hosting origin.
- [ ] Confirm WebAssembly/static assets load without startup-blocking console/network failures.
- [ ] Record browser name/version/engine and OS/device for every tested row.
- [ ] Verify representative viewport/zoom/keyboard behavior.
- [ ] Record refresh/reload/navigation-away lifecycle behavior.
- [ ] Identify the actual persistence mechanism before making persistence claims.
- [ ] Record storage quota/denial/clearing/private-mode behavior where applicable.
- [ ] Record notification/background capability behavior without copying native MAUI assumptions.
- [ ] Record file/camera permission/denial behavior only where those capabilities are actually implemented.
- [ ] Verify unsupported capabilities fail clearly or remain unavailable rather than silently pretending to succeed.
- [ ] Verify no hidden analytics/telemetry/network upload is introduced by browser code/hosting.
- [ ] Perform applicable screen-reader/zoom/focus validation.
- [ ] Keep untested browsers explicitly `NOT RUN`.

A WebAssembly publish success is not a browser production-validation `PASS`.

## 10. Accessibility validation — required

Use `templates/ACCESSIBILITY_VALIDATION_RECORD.md` for representative platform/assistive-technology combinations, including Linux/browser combinations when those hosts are release targets.

- [ ] Representative screen-reader validation.
- [ ] Reading order/names/roles/states/hints.
- [ ] Large text/display scaling/browser zoom as applicable.
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
- [ ] Linux package signing/provenance configured where the chosen distribution channel requires it.
- [ ] Browser deployment provenance/TLS/hosting ownership recorded where browser distribution is intended.
- [ ] Safe public fingerprints/identifiers recorded where appropriate.
- [ ] Signing timestamp/source SHA/package checksum recorded.
- [ ] Final post-signing/deployed-artifact SHA-256 recorded where applicable.
- [ ] No private signing material committed.

## 12. Structured final-package evidence — required

Guide: `PACKAGE_EVIDENCE_TOOLING.md`.

For every intended production artifact supported by the evidence tooling:

- [ ] Use `build/scripts/create-package-evidence.py --stage production` or the Bash/PowerShell wrapper.
- [ ] Require immutable `v*` source tag.
- [ ] Require source tag to resolve to recorded source SHA.
- [ ] Require checked-out HEAD to equal recorded source SHA.
- [ ] Require clean tracked workspace.
- [ ] Provide only non-secret real signing/notarization/store-managed provenance text.
- [ ] Require store-safe payload scanner to pass where applicable.
- [ ] Record per-file SHA-256 evidence.
- [ ] Record top-level package/directory payload SHA-256.
- [ ] Keep generated JSON outside the package payload.
- [ ] Independently cross-check package-evidence payload SHA-256.
- [ ] Retain JSON with final release evidence.

The package-evidence tool does not sign artifacts or prove store approval.

## 13. Final signed/deployed-package inspection — required

For every intended production package/deployment:

- [ ] Exact source SHA/tag recorded.
- [ ] Version/build/application identity recorded.
- [ ] Package/deployment artifact identity and SHA-256/provenance recorded.
- [ ] Signing/notarization/store/hosting provenance recorded as applicable.
- [ ] Package-evidence JSON and payload SHA-256 recorded where applicable.
- [ ] `buymeacoffee.com/sanskarIN` forbidden-marker scan passed for distributed application payloads.
- [ ] `ramsandesh.gumroad.com` forbidden-marker scan passed for distributed application payloads.
- [ ] No Gumroad/Buy Me a Coffee promotional artwork/card/command exists in the distributed CareNest runtime payload.
- [ ] Intended repository/support/legal links remain accurate.
- [ ] Installed/deployed candidate starts.
- [ ] Representative platform/browser smoke tests pass.

The official repository storefront remains `https://ramsandesh.gumroad.com`; it is intentionally outside the distributed health-app package under the current policy.

## 14. Store/distribution metadata, policy and submission — required where applicable

Use `templates/STORE_SUBMISSION_RECORD.md` per intended store/channel where applicable.

Preliminary review: `STORE_POLICY_REVIEW_20260818.md`.

- [x] Preliminary Apple policy review completed on 2026-08-18.
- [x] Preliminary Google Play health/Data safety guidance review completed on 2026-08-18.
- [x] Preliminary Microsoft sensitive-personal-information/privacy review completed on 2026-08-18.
- [ ] Re-open official Apple policy on actual submission date where applicable.
- [ ] Re-open official Google Play policy on actual submission date where applicable.
- [ ] Re-open official Microsoft/Windows policy on actual submission date where applicable.
- [ ] Review the actual Linux distribution channel requirements where Linux publication is intended.
- [ ] Review browser hosting/privacy/security requirements for the actual deployment environment where browser publication is intended.
- [ ] Complete live Google Play Health apps declaration where applicable.
- [ ] Complete live Google Play Data safety answers where applicable.
- [ ] Complete Apple privacy/store metadata where applicable.
- [ ] Complete Microsoft/Partner Center privacy/store metadata where applicable.
- [ ] Verify final listing/deployment claims/disclaimers/reminder limitations.
- [ ] Verify screenshots use fictional data and match the exact package/deployment.
- [ ] Verify support/privacy/terms/security destinations.
- [ ] Record submission/deployment separately from approval/publication.
- [ ] Resolve every rejection/change request before production approval.

Store/distribution policies are time-sensitive; preliminary review is not store approval.

## 15. Freeze exact production source and tag — required

Only after applicable manual/package/browser/accessibility/signing/store findings are resolved:

- [ ] Select exact approved production commit.
- [ ] Confirm the selected source has current exact-source automation in `AUTOMATED_BASELINE.md`, or run fresh verification.
- [ ] Verify final version/build metadata.
- [ ] Verify final release notes/changelog.
- [ ] Verify package/deployment hashes/provenance/package-evidence JSON where applicable.
- [ ] Ensure no unresolved production blocker remains.
- [ ] Create immutable approved `v*` tag.

Do not move a failed/rejected production tag to a different source merely to reuse its version identity.

## 16. Tagged production gates — required

For the immutable approved `v*` tag:

- [ ] Tagged CareNest CI succeeds.
- [ ] Tagged CodeQL succeeds.
- [ ] Tagged Dependency Audit succeeds.
- [ ] Tagged Store Package Configuration succeeds for configured store targets.
- [ ] Tagged Store Inspection Artifacts succeeds for configured inspection targets.
- [ ] Tagged Release Gate succeeds, including Linux/browser release-host builds when configured.
- [ ] Tagged Release Evidence succeeds where configured.
- [ ] Exact tagged run IDs/artifacts/checksums are retained in the release-specific evidence record.

## 17. Production approval — required

Use a release-specific copy of `templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`.

- [ ] Applicable automated evidence is current.
- [ ] Applicable packaged compatibility evidence passes.
- [ ] Applicable MAUI platform/device evidence passes.
- [ ] Applicable Linux desktop evidence passes when Linux is a production target.
- [ ] Applicable browser evidence passes for every browser/deployment represented as supported.
- [ ] Applicable accessibility evidence passes.
- [ ] Applicable signing/notarization/deployment provenance evidence passes.
- [ ] Applicable final-package/deployment evidence passes.
- [ ] Applicable store/distribution-policy blockers are resolved.
- [ ] Approved source/tag/package/deployment hashes are recorded explicitly.
- [ ] Final approval decision is recorded.

## 18. Publication — required

- [ ] GitHub release published where intended.
- [ ] Store packages submitted/promoted where intended.
- [ ] Linux artifacts published only through intended validated channels where applicable.
- [ ] Browser site deployed only from the approved artifact/source where applicable.
- [ ] Store/distribution approval/publication evidence recorded where applicable.
- [ ] Final public version/build matches approved package/deployment hashes.
- [ ] Final status/changelog/next-steps updated.
- [ ] Support/security monitoring channels confirmed.

## Final rule

A failed, unknown, stale, blocked or unperformed required gate blocks production promotion unless it is explicitly and defensibly recorded as not applicable under `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`.

CareNest remains `1.0.0-rc.1` until applicable production rows are actually evidenced.