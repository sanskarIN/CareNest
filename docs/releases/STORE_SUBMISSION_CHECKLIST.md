# CareNest Store Submission Checklist

This checklist separates source completeness from the platform/store work that requires current policy review, signing credentials, developer accounts, package identity, and real-device evidence.

## Before packaging

- [ ] Decide the final `1.0.0` release date and version/build numbers.
- [ ] Complete `docs/releases/RELEASE_CHECKLIST.md`.
- [ ] Complete applicable rows in `docs/releases/MANUAL_TEST_MATRIX.md`.
- [ ] Run `build/scripts/release-preflight.sh` or `build/scripts/release-preflight.ps1` on a fully provisioned host.
- [ ] Confirm CareNest CI and CodeQL are green for the exact source commit being packaged.
- [ ] Re-run dependency audit and review `docs/security/DEPENDENCY_RISK_REGISTER.md`.
- [ ] Resolve or explicitly block release for any high-severity dependency advisory that remains applicable.
- [ ] Review third-party notices/licenses.
- [ ] Verify package/application identifiers are final and owned by the publisher.
- [ ] Verify signing secrets/certificates/provisioning profiles are stored outside the repository.

## Store listing claims

- [ ] Describe CareNest as an organizer/reminder/document app, not a medical device or diagnostic service.
- [ ] Do not claim CareNest determines dosage, recommends treatment, verifies adherence, checks interactions, predicts outcomes, or guarantees reminder delivery.
- [ ] State that reminders can be affected by permissions, battery optimization, force-stop/shutdown, time-zone changes, and OS policy.
- [ ] State that the current release is local-first and does not require a CareNest account.
- [ ] Do not claim whole-database encryption; imported document payloads and manual backups are encrypted while SQLite records rely on app sandbox/device protections.
- [ ] Use fictional data in screenshots and videos.
- [ ] Do not display real prescriptions, private health documents, real phone numbers, real backup passwords, or real PINs.
- [ ] Do not use red-cross/official-accreditation imagery or wording that implies certification/clinical endorsement.

## Privacy disclosure review

- [ ] Store privacy/data-safety forms match the actual shipping build.
- [ ] No analytics/telemetry is declared unless such code is actually introduced and separately privacy-reviewed.
- [ ] External GitHub, legal, email, share/export and Buy Me a Coffee actions are described as explicit user actions where required.
- [ ] The app does not claim that CareNest controls data after the user exports/shares it to another app/service.
- [ ] Privacy policy URL/document is reachable from the listing and app where required.
- [ ] Support contact is current: `supportramsandesh@gmail.com`.
- [ ] Business/privacy contact is current: `sanskarin@outlook.in`.

## Buy Me a Coffee / external funding link

Current project-support destination:

`https://buymeacoffee.com/sanskarIN`

Before each store submission:

- [ ] Review the current store/platform rules that apply to external funding/donation/tipping links for this app category and region.
- [ ] Confirm the BMC action is voluntary project support only.
- [ ] Confirm it does not unlock digital features/content, subscriptions, medical services, priority health support, or other CareNest entitlements.
- [ ] Confirm no health information is transmitted by CareNest when the user opens the link.
- [ ] If a target store disallows the link in the submitted configuration, remove/disable the in-app external funding action for that target before packaging while retaining repository funding links if permitted.
- [ ] Keep the custom badge identified as CareNest project artwork, not an official Buy Me a Coffee brand asset.

## Android packaging

- [ ] Use final application/package ID.
- [ ] Build signed Release AAB/APK with production keystore outside source control.
- [ ] Verify minimum/target Android versions and permissions in the final manifest.
- [ ] Confirm notification permission flow on supported Android versions.
- [ ] Confirm exact-alarm/battery diagnostics remain accurate for the shipping target SDK/device behavior.
- [ ] Test upgrade from the previous public build if one exists.
- [ ] Test fresh install and restore from a user-created CareNest backup.
- [ ] Validate launcher icon, splash, adaptive icon and store graphics.
- [ ] Run Play pre-launch/device testing where available and review crashes/ANRs without uploading real health data.

## Windows packaging

- [ ] Finalize package identity/publisher values.
- [ ] Produce signed MSIX/package with trusted certificate outside repository.
- [ ] Verify install, launch, update and uninstall on supported Windows versions.
- [ ] Verify the app does not promise guaranteed reminders while closed if the current Windows implementation cannot guarantee them.
- [ ] Verify file picker/share/export flows with standard Windows permissions.
- [ ] Validate icons, display name, privacy/support links and architecture targets.

## iOS packaging

- [ ] Finalize bundle identifier/team/signing configuration.
- [ ] Build/archive with production distribution credentials outside source control.
- [ ] Test notification permission denial/grant and delivery on physical iPhone/iPad as applicable.
- [ ] Test background/foreground transitions and time-zone changes.
- [ ] Verify document picker/share/export behavior.
- [ ] Review App Store external-link/funding policy for the exact submitted build and storefronts.
- [ ] Complete App Privacy answers from actual runtime behavior.
- [ ] Validate app icon, launch screen, screenshots and age/category metadata.

## Mac Catalyst packaging

- [ ] Finalize bundle identifier/team/signing/notarization requirements.
- [ ] Build/archive with distribution credentials outside source control.
- [ ] Test install/launch and notification permission behavior on supported macOS hardware.
- [ ] Test keyboard navigation, file picker/export and app lock cold start.
- [ ] Review external funding-link policy for the selected Mac distribution channel.
- [ ] Validate icons/screenshots/privacy/support metadata.

## Release artifacts

- [ ] Generate release notes from `CHANGELOG.md` and `docs/releases/NEXT_STEPS.md` status.
- [ ] Record exact Git commit SHA used for every package.
- [ ] Record CI run IDs used as automated evidence.
- [ ] Record signing/package checksums in the private release process where appropriate; do not commit private keys.
- [ ] Create GitHub release/tag only after the release decision is complete.
- [ ] Preserve rollback/recovery instructions for rejected store submissions or post-release defects.

## Final publication rule

Do not describe a store build as final `1.0.0` merely because source compilation is green. Publication requires the applicable manual/device/accessibility/privacy/security/signing/store-policy checks above, plus an explicit decision on all open dependency advisories.
