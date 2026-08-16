# CareNest Store Submission Checklist

**Release line:** `1.0.0-rc.1`

This checklist separates source completeness from platform/store work requiring current policy review, developer accounts, signing credentials, package identity, packaged-data compatibility and real-device evidence.

## 1. Before packaging

- [ ] Select final production version/build/date.
- [ ] Complete applicable `RELEASE_CHECKLIST.md` rows.
- [ ] Complete applicable `MANUAL_TEST_MATRIX.md` rows.
- [ ] Complete packaged SQLite/encrypted-data compatibility evidence.
- [ ] Complete accessibility evidence.
- [ ] Run release preflight on an appropriately provisioned host.
- [ ] Confirm exact-source CareNest CI green.
- [ ] Confirm exact-source CodeQL green.
- [ ] Confirm exact-source unsuppressed Dependency Audit green.
- [ ] Confirm exact-source Store Package Configuration green.
- [ ] Confirm exact-source Store Inspection Artifacts green.
- [ ] Review dependency/security risk register.
- [ ] Review third-party notices/licenses.
- [ ] Verify final package/application identifiers.
- [ ] Keep signing credentials/private material outside Git.

## 2. Current automated reference

Current accepted PR #74 source evidence:

- 122 unit + 39 integration + 170 UI/source-policy = **331/331**;
- Android/Windows/iOS simulator/Mac Catalyst Release builds green;
- all four store-candidate configurations green;
- Store Inspection Artifacts green;
- CodeQL green;
- unsuppressed Dependency Audit green.

This does not replace the store/manual/package rows below.

## 3. Packaged SQLite/data compatibility

The former SQLite source exception is remediated and the old audit suppression is removed. Store readiness still requires package compatibility evidence.

With fictional prior data:

- [ ] Upgrade/open representative earlier RC data.
- [ ] Verify database integrity.
- [ ] Verify profiles/medicines/schedules/occurrences/logs/appointments/documents/stock/tags/settings.
- [ ] Verify records remain readable/editable.
- [ ] Verify reminder rebuild/reconciliation.
- [ ] Verify no duplicate/stale platform requests.
- [ ] Verify existing/current encrypted documents.
- [ ] Verify current encrypted backup/restore.
- [ ] Verify genuine historical fixture compatibility when real prior bytes exist.
- [ ] Record platform/package/source/checksum/result evidence.

## 4. Store listing claims

- [ ] Describe CareNest as organizational/local-first, not medical/diagnostic.
- [ ] Do not claim dosage calculation/inference.
- [ ] Do not claim treatment recommendation.
- [ ] Do not claim clinical interaction/risk scoring.
- [ ] Do not claim verified adherence.
- [ ] Do not claim guaranteed reminder delivery.
- [ ] Explain relevant OS permission/battery/background limitations.
- [ ] State no required CareNest account/backend in current v1.
- [ ] Do not claim whole-database encryption.
- [ ] Use fictional/synthetic screenshots/videos.
- [ ] Avoid certification/accreditation imagery/claims.

## 5. Privacy/data-safety review

- [ ] Forms match actual shipping runtime behavior.
- [ ] No analytics/telemetry declared unless actually introduced and reviewed.
- [ ] Explicit export/share/calendar/browser boundaries described accurately.
- [ ] External copies are not represented as remaining under CareNest control.
- [ ] Privacy policy link is current/reachable.
- [ ] Terms/security/support links are current/reachable.
- [ ] Support contact: `supportramsandesh@gmail.com`.
- [ ] Business/privacy contact: `sanskarin@outlook.in`.

## 6. Project funding/store boundary

Current distributed application source/package contains **no external Buy Me a Coffee destination/card/command/artwork**.

Repository-only voluntary support destination:

`https://buymeacoffee.com/sanskarIN`

Before submission:

- [ ] Confirm submitted binary still contains no external BMC funding surface.
- [ ] Run/equivalently repeat forbidden-marker scan on final signed package.
- [ ] Confirm screenshots/listing do not imply a removed in-app funding button/card.
- [ ] Confirm repository support does not unlock features, reminder priority, medical/emergency/clinical services or health-data access.
- [ ] Review current store policy for any repository/listing support references actually used.

Do not reintroduce an obsolete per-target funding-link toggle merely for submission.

## 7. Android packaging

- [ ] Final application/package ID verified.
- [ ] Production signing configured outside source control.
- [ ] Signed Release AAB/APK produced as intended.
- [ ] Final manifest/permissions/minimum/target platform reviewed.
- [ ] Notification permission flow tested.
- [ ] Exact/inexact alarm/battery diagnostics tested.
- [ ] Reminder actions/snooze/reconciliation tested on real representative device.
- [ ] Reboot/time-zone/force-stop behavior tested.
- [ ] Upgrade from representative prior package/data tested.
- [ ] Clean install/backup restore tested.
- [ ] Icon/splash/store graphics verified.
- [ ] Pre-launch/device testing reviewed where available without exposing private data.

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

## 11. Store assets

Use `docs/design/STORE_ASSETS.md`.

- [ ] Screenshots use fictional data.
- [ ] Screenshots match exact shipping build.
- [ ] No real prescriptions/documents/contacts.
- [ ] No unsupported medical claims.
- [ ] No guaranteed reminder claim.
- [ ] No whole-database encryption claim.
- [ ] No screenshot implies in-app BMC funding surface.
- [ ] Listing language matches actual features/local-first boundary.

## 12. Exact production tag gates

Create final immutable `v*` tag only after applicable pre-tag manual/signing/store preparation is complete.

Require:

- [ ] Tagged CareNest CI.
- [ ] Tagged CodeQL.
- [ ] Tagged Dependency Audit.
- [ ] Tagged Store Package Configuration.
- [ ] Tagged Store Inspection Artifacts.
- [ ] Tagged Release Gate.
- [ ] Tagged Release Evidence.

If a tag fails, preserve evidence, fix source/config on a new commit, repeat required checks and use a corrected approved version/tag. Do not move the failed tag.

## 13. Final signed artifact evidence

For every release artifact:

- [ ] Exact source SHA/tag recorded.
- [ ] Version/build/package identity recorded.
- [ ] Filename recorded.
- [ ] SHA-256 recorded.
- [ ] Signing/notarization/store provenance recorded.
- [ ] Forbidden-marker scan passed.
- [ ] Installed package smoke test passed.
- [ ] About contains no BMC funding card/action.
- [ ] Repository/creator/business/support/privacy/terms/security/notices verified.

## 14. Release metadata/evidence

- [ ] Release notes/changelog finalized.
- [ ] CI/CodeQL/Dependency Audit/Store Package/Store Inspection/Release Gate/Release Evidence run IDs recorded.
- [ ] Release Evidence artifact/checksums recorded.
- [ ] Rollback/hotfix instructions retained.
- [ ] Final publication/store approval evidence recorded.

## Final publication rule

Do not describe a store build as final production merely because source compilation/CI is green. Publication requires applicable automated exact-tag gates, manual device/accessibility/privacy/security/signing/store checks, packaged compatibility and signed-package provenance.

CareNest remains `1.0.0-rc.1` until those rows are actually evidenced.