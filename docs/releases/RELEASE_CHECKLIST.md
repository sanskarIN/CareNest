# CareNest Release Checklist

**Release line:** `1.0.0-rc.1`  
**Latest fully verified Gumroad implementation/source-policy source:** `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

This checklist separates completed source/automated evidence from the newer verification-relevant release-engineering source and from production work that still requires real package/device/accessibility/signing/store evidence.

## 1. Latest fully verified automated source baseline — completed

For exact source `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`:

- [x] CareNest CI run `32032436061` succeeded.
- [x] Platform-neutral formatting succeeded.
- [x] Unit tests: **122/122**.
- [x] Integration tests: **39/39**.
- [x] UI/source-policy tests: **175/175**.
- [x] Total core tests: **336/336**.
- [x] Android Release build succeeded.
- [x] Windows Release build succeeded.
- [x] iOS simulator Release build succeeded.
- [x] Mac Catalyst Release build succeeded.
- [x] Store Package Configuration run `32032436093` succeeded on all four targets.
- [x] CodeQL run `32032436037` succeeded.
- [x] Strict XAML Source binding compilation is enabled.
- [x] `XC0022`, `XC0023`, `XC0024`, `XC0025` are warnings-as-errors.
- [x] Application source/package contains no external Buy Me a Coffee destination/card/command/artwork at that verified boundary.
- [x] Application source/package contains no external Gumroad destination/card/command/artwork at that verified boundary.
- [x] Store payload scanner covers both repository-only external-commerce markers.

Authoritative automated record: `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`.

Permanent compiled-binding evidence: `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

The 336-test result belongs to the exact source above. It must not be copied forward as the result of a newer verification-relevant head without a real run.

## 2. New verification-relevant release engineering — implemented, fresh exact-source evidence required

Current `main` now contains additional tests/scripts/workflow/release-policy changes after the latest verified baseline.

Implemented:

- [x] release-documentation consistency contracts;
- [x] package-evidence tooling contracts;
- [x] fail-closed package-evidence generator;
- [x] Bash and PowerShell package-evidence wrappers;
- [x] synthetic package-evidence self-test;
- [x] CI package-evidence Python syntax/self-test;
- [x] Release Gate package-evidence syntax/self-test and evidence/tooling existence checks;
- [x] Release Evidence package-tooling self-test capture;
- [x] structured final-package evidence documentation;
- [x] exact-head verification protocol updated for the new source boundary.

Required before this newer source can replace `94e867...` as the accepted automated baseline:

- [ ] Freeze exact final verification-relevant `main` SHA.
- [ ] Verify package-evidence syntax/self-test on that exact source.
- [ ] Verify platform-neutral formatting.
- [ ] Record actual unit-test total/result.
- [ ] Record actual integration-test total/result.
- [ ] Record actual UI/source-policy total/result.
- [ ] Record actual overall core-test total/result.
- [ ] Verify Android Release build.
- [ ] Verify Windows Release build.
- [ ] Verify iOS simulator Release build.
- [ ] Verify Mac Catalyst Release build.
- [ ] Verify Store Package Configuration on all four targets.
- [ ] Verify Store Inspection Artifacts.
- [ ] Verify CodeQL.
- [ ] Verify unsuppressed Dependency Audit.
- [ ] Record exact source SHA/run IDs/results in a new verification record.

Do not infer a new test count from source inspection.

## 3. Source/product boundary — completed

- [x] Local-first/account-free RC1 feature scope implemented.
- [x] Profiles, medicines, schedules, logs, appointments, documents, reports, settings and app lock implemented for current source scope.
- [x] No dosage calculation/inference feature.
- [x] No diagnosis/treatment recommendation feature.
- [x] No clinical interaction/risk scoring feature.
- [x] No hidden runtime analytics/telemetry client.
- [x] No required CareNest backend/cloud sync.
- [x] Reminder planner/coordinator deterministic/reconciliation contracts implemented.
- [x] Snooze effective-due behavior implemented.
- [x] Cancellation-first reminder-action/reconciliation behavior implemented.
- [x] Encrypted document/backup v2 framing with retained documented v1 read compatibility implemented.
- [x] SQLite dependency source remediation implemented.
- [x] Gumroad/Buy Me a Coffee-free distributed application package/source policy implemented.

## 4. Documentation — current source scope

- [x] Root README current for product/storefront boundary.
- [x] Complete project documentation present.
- [x] Documentation catalog/authority map present.
- [x] User guide/FAQ/feature reference/known limitations present.
- [x] Developer/configuration/codebase/maintenance/setup/troubleshooting references present.
- [x] Architecture/privacy/security/testing/design documentation present.
- [x] Platform behavior matrix distinguishes automated/manual evidence.
- [x] Release process/checklists/runbooks present.
- [x] Previous canonical documentation snapshots preserved under `docs/history/`.
- [x] Documentation audit recorded for 2026-08-16.
- [x] Preliminary store-policy review recorded for 2026-08-18.
- [x] Package-evidence tooling guide present.
- [x] Active release documentation has consistency regression coverage.

The current documentation consumed by the new source-policy contracts participates in the fresh exact-source verification required in section 2.

## 5. Packaged existing-data/SQLite compatibility — required

Use fictional/synthetic data only.

- [ ] Prepare representative earlier RC data.
- [ ] Upgrade/install through realistic package path.
- [ ] Confirm SQLite database opens.
- [ ] Run/record integrity validation.
- [ ] Confirm profiles/medicines/schedules/occurrences/logs/appointments/stock/documents/tags/settings remain readable.
- [ ] Confirm editable records remain editable.
- [ ] Confirm schema version/migrations.
- [ ] Confirm reminder rebuild/reconciliation.
- [ ] Confirm no duplicate/stale platform requests.
- [ ] Record source/package/checksum/device/result evidence.

## 6. Encrypted document/backup compatibility — required

- [ ] Current packaged document import/open/export/delete lifecycle.
- [ ] Failed export leaves no unintended CareNest-owned partial plaintext file.
- [ ] Missing/corrupt key fails closed.
- [ ] Current packaged backup create/inspect/restore.
- [ ] Wrong password rejected.
- [ ] Tampered backup rejected.
- [ ] Truncated backup rejected.
- [ ] Trailing-data backup rejected.
- [ ] Restored encrypted documents remain usable.
- [ ] Clean-install restore works.
- [ ] Genuine historical v1 fixtures verified where real prior bytes exist.
- [ ] No newly manufactured fixture is mislabeled as historical evidence.

## 7. Android manual matrix — required

- [ ] Fresh install/onboarding.
- [ ] Notification permission denied.
- [ ] Notification permission granted.
- [ ] Actual medicine reminder delivery.
- [ ] Actual appointment reminder delivery.
- [ ] Create/edit/delete reminder lifecycle.
- [ ] Taken/Skipped/Delayed/Missed cancellation-first behavior.
- [ ] Snooze cancellation/replacement.
- [ ] Schedule-edit stale-request cleanup.
- [ ] Medicine/profile delete cleanup.
- [ ] Restart/reopen recovery.
- [ ] Reboot recovery.
- [ ] Exact/inexact alarm diagnostics.
- [ ] Battery/vendor background restrictions.
- [ ] Clock/time-zone/DST behavior.
- [ ] Force-stop limitation messaging.
- [ ] Document picker/share.
- [ ] Backup/restore.
- [ ] App lock.
- [ ] Accessibility.

## 8. Windows manual matrix — required

- [ ] Install/package execution.
- [ ] Navigation/core CRUD.
- [ ] Running-app notification behavior.
- [ ] Closed-app limitation messaging/behavior.
- [ ] Same-ID timer replacement/cancellation.
- [ ] Reminder actions/snooze/reconciliation.
- [ ] Restart/recovery.
- [ ] Documents/share.
- [ ] Backup/restore.
- [ ] App lock.
- [ ] Keyboard/focus.
- [ ] System/light/dark themes.
- [ ] Accessibility.

## 9. iPhone/iPad real-device matrix — required

- [ ] Signed/provisioned real-device install.
- [ ] Notification permission denied/granted.
- [ ] Actual medicine/appointment notifications.
- [ ] Reminder actions/snooze/reconciliation.
- [ ] Restart/lifecycle/time-zone behavior.
- [ ] Documents/share.
- [ ] Backup/restore.
- [ ] App lock.
- [ ] Dynamic Type.
- [ ] VoiceOver.
- [ ] Notification preview privacy.

Simulator compilation is not a substitute.

## 10. Mac Catalyst manual matrix — required

- [ ] Install/package execution.
- [ ] Notification permission/delivery.
- [ ] Reminder actions/reconciliation.
- [ ] Restart/lifecycle.
- [ ] File picker/share.
- [ ] Backup/restore.
- [ ] App lock.
- [ ] Keyboard/focus.
- [ ] Theme/contrast.
- [ ] Accessibility.
- [ ] Signed/notarized behavior when available.

## 11. Accessibility — required

- [ ] Representative screen-reader validation.
- [ ] Large text/text scaling.
- [ ] Desktop keyboard/focus order.
- [ ] Light/dark/system contrast.
- [ ] Color-independent status meaning.
- [ ] Reduced motion.
- [ ] Destructive confirmation readability.
- [ ] Privacy-safe actionable errors.

## 12. Production signing — required outside Git

- [ ] Android production signing configured outside Git.
- [ ] Apple certificates/provisioning configured outside Git.
- [ ] Windows production signing configured outside Git where applicable.
- [ ] Safe public fingerprints/identifiers recorded where appropriate.
- [ ] Signing timestamp/source SHA/package checksum recorded.
- [ ] No private signing material committed.

## 13. Structured final package evidence — required

Guide: `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`.

For every production artifact:

- [ ] Use `build/scripts/create-package-evidence.py --stage production` or the Bash/PowerShell wrapper.
- [ ] Immutable `v*` source tag recorded.
- [ ] Source tag resolves to recorded source SHA.
- [ ] Checked-out HEAD equals recorded source SHA.
- [ ] Tracked workspace is clean.
- [ ] Non-secret real signing/notarization/store provenance recorded.
- [ ] Package evidence JSON written outside the package payload.
- [ ] Per-file SHA-256 manifest recorded.
- [ ] Top-level package/directory payload SHA-256 recorded.
- [ ] Package evidence payload SHA-256 independently cross-checked.
- [ ] Package evidence JSON retained with release evidence.

The package evidence tool does not sign packages or prove store approval.

## 14. Final signed-package inspection — required

For every production package:

- [ ] Exact source SHA/tag recorded.
- [ ] Version/build recorded.
- [ ] Package identity recorded.
- [ ] Filename recorded.
- [ ] SHA-256 recorded.
- [ ] Signing/notarization/store provenance recorded.
- [ ] Package evidence JSON recorded.
- [ ] Package evidence payload SHA-256 recorded.
- [ ] `buymeacoffee.com/sanskarIN` forbidden-marker scan passed.
- [ ] `ramsandesh.gumroad.com` forbidden-marker scan passed.
- [ ] Installed About/runtime contains no Gumroad/Buy Me a Coffee promotion or purchase CTA.
- [ ] Repository/creator/business/support/privacy/terms/security/notices remain available as intentionally designed.
- [ ] Installed package starts.
- [ ] Platform smoke tests passed.

## 15. Store metadata/policy — preliminary review complete; submission-time evidence still required

Pre-submission policy review: `docs/releases/STORE_POLICY_REVIEW_20260818.md`.

- [x] Apple App Review Guidelines reviewed on 2026-08-18 against current CareNest source/product boundary.
- [x] Google Play health-app, Health apps declaration and Data safety guidance reviewed on 2026-08-18.
- [x] Microsoft Store sensitive-personal-information/privacy rules reviewed on 2026-08-18.
- [x] Current CareNest non-clinical health-organizer wording boundary reviewed.
- [x] Current Gumroad/Buy Me a Coffee package-exclusion decision reviewed.
- [ ] Re-check current Apple rules on the actual submission date for the exact package/listing.
- [ ] Re-check current Google Play rules on the actual submission date for the exact package/listing.
- [ ] Re-check current Microsoft/Windows rules on the actual submission date where applicable.
- [ ] Complete live Google Play Health apps declaration against the exact production feature set.
- [ ] Complete live Google Play Data safety answers against the exact production binary/SDK behavior.
- [ ] Complete Apple privacy metadata against the exact production binary/capabilities.
- [ ] Complete Microsoft/Partner Center privacy metadata where applicable.
- [ ] Notification limitation wording reviewed against final listing text.
- [ ] Screenshots use fictional data and match exact production package.
- [ ] No screenshot/listing implies in-app Gumroad/Buy Me a Coffee purchase/funding functionality.
- [ ] Support/privacy/terms/security links verified from final store metadata.
- [ ] Final submission review date/source/conclusion recorded.

## 16. Select exact production source — required

- [ ] Freeze exact approved production commit.
- [ ] Repeat exact-source verification if verification-relevant source changed after accepted baseline.
- [ ] Verify final version/build metadata.
- [ ] Verify final release notes/changelog.
- [ ] Verify final package checksums/provenance/package evidence JSON.

## 17. Create exact production `v*` tag — required

For the approved immutable tag require:

- [ ] Tagged CareNest CI success.
- [ ] Tagged CodeQL success.
- [ ] Tagged Dependency Audit success.
- [ ] Tagged Store Package Configuration success.
- [ ] Tagged Store Inspection Artifacts success.
- [ ] Tagged Release Gate success.
- [ ] Tagged Release Evidence success.
- [ ] Release Evidence package-tool self-test evidence recorded.
- [ ] Release Evidence artifact/checksums recorded.
- [ ] Final signed-package structured evidence/provenance recorded.

Do not move a failed/rejected production tag to different source.

## 18. Publication — required

- [ ] GitHub release published where intended.
- [ ] Store packages submitted/promoted where intended.
- [ ] Store approval/publication evidence recorded.
- [ ] Final status/changelog/next-steps updated.
- [ ] Support/security monitoring channels confirmed.

## Final rule

A failed, unknown, stale or unperformed required gate blocks production promotion unless explicitly documented as non-applicable with a defensible reason.

CareNest remains `1.0.0-rc.1` until applicable production rows are actually evidenced.
