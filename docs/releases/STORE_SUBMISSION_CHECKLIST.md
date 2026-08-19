# CareNest Store Submission Checklist

**Release line:** `1.0.0-rc.1`  
**Current automated evidence authority:** `docs/releases/AUTOMATED_BASELINE.md`  
**Production evidence standard:** `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`  
**Production evidence index:** `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

This stable checklist separates source completeness from platform/store work requiring current policy review, developer accounts, signing credentials, package identity, packaged-data compatibility, structured package provenance and real-device evidence.

Do not pin a moving accepted SHA/test total here. Use `docs/releases/AUTOMATED_BASELINE.md` for the latest actually observed exact-source automated result.

Use a release-specific copy of `docs/releases/templates/STORE_SUBMISSION_RECORD.md` for actual submission/review/approval/publication evidence.

## 1. Before packaging

- [ ] Select final production version/build/date.
- [ ] Complete applicable `RELEASE_CHECKLIST.md` rows.
- [ ] Create release-specific production evidence records using `PRODUCTION_EVIDENCE_INDEX.md`.
- [ ] Complete packaged SQLite/encrypted-data compatibility evidence.
- [ ] Complete accessibility evidence.
- [ ] Run release preflight on an appropriately provisioned host.
- [ ] Confirm exact-source CareNest CI green.
- [ ] Confirm exact-source CodeQL green.
- [ ] Confirm exact-source unsuppressed Dependency Audit green.
- [ ] Confirm exact-source Store Package Configuration green.
- [ ] Confirm exact-source Store Inspection Artifacts green.
- [ ] Confirm automation belongs to the exact intended candidate, not an older source.
- [ ] Review dependency/security risk register.
- [ ] Review third-party notices/licenses.
- [ ] Verify final package/application identifiers.
- [ ] Keep signing credentials/private material outside Git.

## 2. Automated reference

Current accepted exact-source automation is owned by:

`docs/releases/AUTOMATED_BASELINE.md`

If verification-relevant source changes after that boundary, fresh exact-source verification is required before packaging/promotion.

Automation does not replace manual/package/accessibility/signing/store evidence.

## 3. Packaged SQLite/data compatibility

With fictional/synthetic representative prior data:

- [ ] Upgrade/open representative earlier data.
- [ ] Verify database integrity.
- [ ] Verify profiles/medicines/schedules/occurrences/logs/appointments/documents/stock/tags/settings.
- [ ] Verify records remain readable/editable.
- [ ] Verify reminder rebuild/reconciliation.
- [ ] Verify no duplicate/stale platform requests.
- [ ] Verify encrypted documents.
- [ ] Verify current encrypted backup/restore.
- [ ] Verify genuine historical fixture compatibility only where genuine prior bytes safely exist.
- [ ] Record platform/package/source/checksum/device/result evidence.

Use `docs/releases/templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`.

## 4. Store listing claims

- [ ] Describe CareNest as organizational/local-first, not medical/diagnostic.
- [ ] Do not claim dosage calculation/inference.
- [ ] Do not claim treatment recommendation.
- [ ] Do not claim clinical interaction/risk scoring.
- [ ] Do not claim verified adherence.
- [ ] Do not claim guaranteed reminder delivery.
- [ ] Do not claim emergency-service functionality.
- [ ] Do not claim regulated medical-device status unless a future approved release actually has that status.
- [ ] Explain relevant OS permission/battery/background limitations.
- [ ] State no required CareNest account/backend in current v1.
- [ ] Do not claim whole-database encryption.
- [ ] Use fictional/synthetic screenshots/videos.
- [ ] Avoid unsupported certification/accreditation imagery or claims.

## 5. Privacy/data-safety review

- [ ] Forms match actual shipping runtime behavior.
- [ ] No analytics/telemetry declared unless actually introduced and reviewed.
- [ ] Explicit export/share/calendar/browser boundaries described accurately.
- [ ] External copies are not represented as remaining under CareNest control.
- [ ] Privacy policy link is current/reachable.
- [ ] Terms/security/support links are current/reachable.
- [ ] Support contact is correct.
- [ ] Business/privacy contact is correct.
- [ ] Google Play Data safety completed against exact production binary/SDK behavior where applicable.
- [ ] Apple privacy metadata completed against exact production capabilities/binary where applicable.
- [ ] Microsoft/Partner Center privacy metadata completed where applicable.

## 6. External-commerce/store boundary

Current distributed application source/package contains **no external Buy Me a Coffee or Gumroad destination/card/command/artwork**.

Repository-only destinations:

- Buy Me a Coffee: `https://buymeacoffee.com/sanskarIN`;
- Gumroad: `https://ramsandesh.gumroad.com`.

Before submission:

- [ ] Confirm submitted binary contains no external Buy Me a Coffee funding surface.
- [ ] Confirm submitted binary contains no external Gumroad storefront surface.
- [ ] Scan/equivalently inspect final package for `buymeacoffee.com/sanskarIN`.
- [ ] Scan/equivalently inspect final package for `ramsandesh.gumroad.com`.
- [ ] Confirm screenshots/listing do not imply an in-app BMC/Gumroad button/card.
- [ ] Confirm support/storefront promotion does not unlock features, reminder priority, medical/emergency/clinical services or health-data access.
- [ ] Review current store policy for repository/listing support/storefront references actually used.

Do not reintroduce obsolete per-target external-commerce toggles merely for submission.

## 7. Android packaging

- [ ] Final application/package ID verified.
- [ ] Production signing configured outside source control.
- [ ] Signed Release AAB/APK produced as intended.
- [ ] Final manifest/permissions/platform targets reviewed.
- [ ] Notification permission flow tested.
- [ ] Exact/inexact alarm/battery diagnostics tested.
- [ ] Reminder actions/snooze/reconciliation tested on representative device.
- [ ] Reboot/time-zone/force-stop behavior tested.
- [ ] Upgrade from representative prior package/data tested.
- [ ] Clean install/backup restore tested.
- [ ] Icon/splash/store graphics verified.
- [ ] Live Google Play Health apps declaration completed for exact feature set.
- [ ] Live Google Play Data safety answers completed for exact production binary/SDK behavior.
- [ ] Submission-date Google Play policies re-checked.

## 8. Windows packaging

- [ ] Final identity/publisher verified.
- [ ] Production signing configured outside Git.
- [ ] Intended signed package produced.
- [ ] Install/launch/update/uninstall tested.
- [ ] Closed-app reminder limitation remains accurate.
- [ ] Timer replacement/cancellation and reminder actions tested.
- [ ] Files/share/export tested.
- [ ] Packaged data/encryption compatibility tested.
- [ ] Icons/display name/support/privacy/legal metadata verified.
- [ ] Submission-date Microsoft Store requirements re-checked where applicable.

## 9. iOS/iPadOS packaging

- [ ] Final bundle ID/team/signing configuration verified.
- [ ] Distribution credentials kept outside Git.
- [ ] Production archive/package created.
- [ ] Real-device notification permission/delivery tested.
- [ ] Reminder actions/snooze/reconciliation tested.
- [ ] Lifecycle/time-zone behavior tested.
- [ ] Documents/share/export tested.
- [ ] Packaged data/encryption compatibility tested.
- [ ] App Privacy answers match actual runtime.
- [ ] Icons/launch screen/screenshots/category metadata verified.
- [ ] Dynamic Type/VoiceOver tested.
- [ ] Submission-date Apple App Review Guidelines and privacy/store metadata re-checked.

## 10. Mac Catalyst packaging

- [ ] Final bundle ID/team/signing/notarization configuration verified.
- [ ] Production signing credentials kept outside Git.
- [ ] Intended signed/notarized candidate created.
- [ ] Install/launch/notifications tested.
- [ ] Reminder actions/reconciliation tested.
- [ ] Files/share/backup/app-lock tested.
- [ ] Packaged data/encryption compatibility tested.
- [ ] Keyboard/focus/themes/accessibility tested.
- [ ] Icons/screenshots/privacy/support metadata verified.
- [ ] Submission-date Apple policy/privacy/store requirements re-checked.

## 11. Store assets

Use `docs/design/STORE_ASSETS.md`.

- [ ] Screenshots use fictional data.
- [ ] Screenshots match exact shipping build.
- [ ] No real prescriptions/documents/contacts.
- [ ] No unsupported medical claims.
- [ ] No guaranteed reminder claim.
- [ ] No whole-database encryption claim.
- [ ] No screenshot implies in-app BMC funding surface.
- [ ] No screenshot implies in-app Gumroad storefront/purchase surface.
- [ ] Listing language matches actual features/local-first boundary.

## 12. Submission-date policy review

Preliminary dated review:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

The preliminary review is not store approval and does not replace the final gate.

- [ ] Re-open current Apple policy sources for exact Apple package/listing where applicable.
- [ ] Re-open current Google Play policy sources for exact Android package/listing where applicable.
- [ ] Re-open current Microsoft Store policy sources where applicable.
- [ ] Complete live store-console declarations/metadata.
- [ ] Record review date, official sources, conclusions and required changes.
- [ ] If source/package changes, repeat affected exact-source and package/manual verification.

## 13. Exact production tag gates

Create final immutable `v*` tag only after applicable pre-tag manual/signing/store preparation is complete.

Require:

- [ ] Tagged CareNest CI.
- [ ] Tagged CodeQL.
- [ ] Tagged Dependency Audit.
- [ ] Tagged Store Package Configuration.
- [ ] Tagged Store Inspection Artifacts.
- [ ] Tagged Release Gate.
- [ ] Tagged Release Evidence.

If a tag fails, preserve evidence, fix source/configuration on a new commit, repeat required checks and use a corrected approved version/tag. Do not move the failed tag.

## 14. Structured final package evidence

For every final production artifact follow:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

Generate evidence with `build/scripts/create-package-evidence.py --stage production` or the shell/PowerShell wrapper.

Required production conditions include immutable tag/source/HEAD agreement, clean tracked workspace, non-secret real signing/notarization/store provenance, successful BMC/Gumroad store-safe scan, SHA-256 evidence and output outside the package payload.

- [ ] Package evidence JSON created for each intended production artifact.
- [ ] Package evidence retained with release record.
- [ ] Payload SHA-256 independently cross-checked.

The tool does not sign packages or prove store approval.

## 15. Final artifact evidence

For every release artifact:

- [ ] Exact source SHA/tag recorded.
- [ ] Version/build/package identity recorded.
- [ ] Filename recorded.
- [ ] SHA-256 recorded.
- [ ] Signing/notarization/store provenance recorded without secrets.
- [ ] Package evidence JSON recorded.
- [ ] Package evidence payload SHA-256 recorded.
- [ ] Buy Me a Coffee forbidden-marker scan passed.
- [ ] Gumroad forbidden-marker scan passed.
- [ ] Installed package smoke test passed.
- [ ] Installed runtime contains no BMC/Gumroad promotional card/action.
- [ ] Intended support/privacy/terms/security/notices surfaces verified.

## 16. Release metadata/evidence

- [ ] Release notes/changelog finalized.
- [ ] CI/CodeQL/Dependency Audit/Store Package/Store Inspection/Release Gate/Release Evidence run IDs recorded.
- [ ] Release Evidence artifact/checksums recorded.
- [ ] Final package provenance/checksums recorded.
- [ ] Structured package evidence JSON paths/checksums recorded.
- [ ] Store-policy review date/sources recorded.
- [ ] Live store declaration completion recorded.
- [ ] Rollback/hotfix instructions retained.
- [ ] Final submission/review/approval/publication evidence recorded.

## Final publication rule

Do not describe a store build as final production merely because source compilation/CI is green. Publication requires applicable automated exact-tag gates, manual device/accessibility/privacy/security/signing/store checks, packaged compatibility, structured package provenance, live store declarations and current submission-date policy review.

A missing, stale, blocked or unperformed required item is not `PASS` under `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`.

CareNest remains `1.0.0-rc.1` until applicable production evidence is real.
