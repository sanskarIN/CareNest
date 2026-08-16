# CareNest Store Submission Checklist

This checklist separates source completeness from the platform/store work that requires current policy review, signing credentials, developer accounts, package identity, packaged-data compatibility, and real-device evidence.

## Before packaging

- [ ] Decide the final `1.0.0` release date and version/build numbers.
- [ ] Complete `docs/releases/RELEASE_CHECKLIST.md`.
- [ ] Complete applicable rows in `docs/releases/MANUAL_TEST_MATRIX.md`.
- [ ] Run `build/scripts/release-preflight.sh` or `build/scripts/release-preflight.ps1` on a fully provisioned host.
- [ ] Confirm the complete exact-source CareNest CI matrix is green for the commit being packaged.
- [ ] Confirm CodeQL is green for the exact source commit being packaged.
- [ ] Confirm unsuppressed Dependency Audit is green for the exact source commit being packaged.
- [ ] Review `docs/security/DEPENDENCY_RISK_REGISTER.md` and `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.
- [ ] Complete representative packaged SQLite existing-data/encrypted-data compatibility evidence after the native/provider remediation.
- [ ] Resolve or explicitly block release for any dependency advisory that remains applicable under the release policy.
- [ ] Review third-party notices/licenses.
- [ ] Verify package/application identifiers are final and owned by the publisher.
- [ ] Verify signing secrets/certificates/provisioning profiles are stored outside the repository.

## Packaged SQLite/data compatibility

The former `GHSA-2m69-gcr7-jv3q` source exception is remediated and the old audit suppression is removed. Store readiness still requires packaged compatibility evidence because dependency security and existing-data integrity are different release properties.

Using fictional/synthetic data:

- [ ] Upgrade/open a representative installation containing pre-remediation RC1 SQLite data.
- [ ] Verify profiles, medicines, schedules, reminder occurrences, logs, appointments, documents, stock, tags and settings remain readable/editable.
- [ ] Verify SQLite integrity after upgrade.
- [ ] Verify reminder rebuild/reconciliation after upgrade.
- [ ] Verify existing encrypted document payloads remain decryptable through the unchanged key path.
- [ ] Verify a new encrypted backup/restore round-trip.
- [ ] Verify a canonical synthetic pre-remediation backup where available and compatible.
- [ ] Record target platform, package/build, source SHA, package graph, date and non-sensitive result notes.

Any corruption, migration failure, encrypted-document failure, backup incompatibility, or reminder-state regression blocks store promotion even when vulnerability audit is green.

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
- [ ] Test cancellation-first Taken/Skipped/Delayed/Missed/Snoozed actions against actual Android scheduled requests.
- [ ] Test stale-request cleanup after schedule/state changes and medicine/profile deletion.
- [ ] Test upgrade from the previous public/pre-remediation representative build using fictional data.
- [ ] Test fresh install and restore from a user-created CareNest backup.
- [ ] Validate launcher icon, splash, adaptive icon and store graphics.
- [ ] Run Play pre-launch/device testing where available and review crashes/ANRs without uploading real health data.

## Windows packaging

- [ ] Finalize package identity/publisher values.
- [ ] Produce signed MSIX/package with trusted certificate outside repository.
- [ ] Verify install, launch, update and uninstall on supported Windows versions.
- [ ] Verify the app does not promise guaranteed reminders while closed if the current Windows implementation cannot guarantee them.
- [ ] Verify in-process reminder cancellation/replacement and cancellation-first action behavior.
- [ ] Verify file picker/share/export flows with standard Windows permissions.
- [ ] Verify packaged existing-data/encrypted-data compatibility after the SQLite native/provider update.
- [ ] Validate icons, display name, privacy/support links and architecture targets.

## iOS packaging

- [ ] Finalize bundle identifier/team/signing configuration.
- [ ] Build/archive with production distribution credentials outside source control.
- [ ] Test notification permission denial/grant and delivery on physical iPhone/iPad as applicable.
- [ ] Test cancellation-first handled actions, snooze replacement and stale request reconciliation.
- [ ] Test background/foreground transitions and time-zone changes.
- [ ] Verify document picker/share/export behavior.
- [ ] Verify packaged existing-data/encrypted-data compatibility after the SQLite native/provider update.
- [ ] Review App Store external-link/funding policy for the exact submitted build and storefronts.
- [ ] Complete App Privacy answers from actual runtime behavior.
- [ ] Validate app icon, launch screen, screenshots and age/category metadata.

## Mac Catalyst packaging

- [ ] Finalize bundle identifier/team/signing/notarization requirements.
- [ ] Build/archive with distribution credentials outside source control.
- [ ] Test install/launch and notification permission behavior on supported macOS hardware.
- [ ] Test cancellation-first handled actions and snooze/stale-request reconciliation.
- [ ] Test keyboard navigation, file picker/export and app lock cold start.
- [ ] Verify packaged existing-data/encrypted-data compatibility after the SQLite native/provider update.
- [ ] Review external funding-link policy for the selected Mac distribution channel.
- [ ] Validate icons/screenshots/privacy/support metadata.

## Exact approved tag gates

Create the final `v*` tag only after applicable pre-tag manual/signing/store preparation is complete and the exact commit is approved for tagging.

The tag is expected to trigger:

- [ ] CareNest CI for the exact tagged commit.
- [ ] CodeQL for the exact tagged commit.
- [ ] Dependency Audit for the exact tagged commit.
- [ ] Release Gate for the exact tagged commit.
- [ ] CareNest Release Evidence for the exact tagged commit.

Do not publish/promote the GitHub/store release until all required tag-triggered workflows are successful.

If a tag workflow fails, preserve evidence, fix source/configuration on a new commit, repeat required verification/manual checks, and create/use the corrected approved tag rather than moving or disguising the failed tag.

## Release artifacts

- [ ] Generate release notes from `CHANGELOG.md` and `docs/releases/NEXT_STEPS.md` status.
- [ ] Record exact Git commit SHA used for every package.
- [ ] Record CI, CodeQL, Dependency Audit, Release Gate and Release Evidence run IDs used as automated evidence.
- [ ] Record the Release Evidence artifact name; it includes commit SHA, run ID and run attempt.
- [ ] Record signing/package checksums in the private release process where appropriate; do not commit private keys.
- [ ] Confirm signed package provenance points to the exact approved/tagged commit.
- [ ] Preserve rollback/recovery instructions for rejected store submissions or post-release defects.

## Final publication rule

Do not describe a store build as final `1.0.0` merely because source compilation is green. Publication requires the applicable automated exact-tag gates, manual device/accessibility/privacy/security/signing/store-policy checks, packaged existing-data/encrypted-data compatibility evidence, and no unresolved release-blocking dependency risk.
