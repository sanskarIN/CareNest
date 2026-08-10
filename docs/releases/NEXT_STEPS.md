# CareNest Next Steps

This document tracks work after the source-complete `1.0.0-rc.1` milestone. It intentionally separates release blockers from completed hardening and optional future versions so unfinished future ideas are not confused with missing RC1 implementation.

## Automated hardening baseline completed

Exact source head `8417513db36c72b0ec2cfaccadb6ac47ba361f11` passed:

- CareNest CI #200 / `31375336226`;
- platform-neutral formatting;
- 15 unit tests;
- 11 integration tests;
- 46 UI-contract/policy tests;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- CodeQL #200 / `31375336083`;
- Dependency Audit #7 / `31375336088`.

This automated baseline does not complete the production-release blockers below.

## Priority 0 — production-release blockers

These items must be completed before promoting the release candidate to a public production release.

### 1. Resolve the open SQLite dependency advisory

- [x] Re-check the current `sqlite-net-pcl` / `SQLitePCLRaw` dependency graph through the repository Dependency Audit workflow.
- [ ] Upgrade to a compatible patched native SQLite package path when one is actually available, or adopt a separately verified replacement provider/path.
- [ ] Remove the narrow `NuGetAuditSuppress` entry only after the dependency graph no longer resolves the affected package/advisory.
- [ ] Run unit, integration, UI-contract, Android, Windows, iOS simulator, Mac Catalyst, CodeQL and Dependency Audit verification again after any SQLite dependency/provider change.
- [ ] Update `docs/security/DEPENDENCY_RISK_REGISTER.md` with the exact final resolution/decision and evidence.

**Current state:** `GHSA-2m69-gcr7-jv3q` remains open for SQLitePCLRaw native `2.1.11`. The attempted `2.1.12` bundle path was unavailable. The narrow audit suppression is not a vulnerability fix.

**Done when:** the release dependency path has an acceptable documented resolution/decision and the applicable regression/verification gates are green.

### 2. Run manual device and accessibility smoke testing

Automated CI proves compilation/contracts, but it does not replace real-device behavior testing.

- [ ] Android phone: fresh install, onboarding, notification permission denied/granted, exact/inexact alarm behavior, reboot rebuild, time-zone change, battery-optimization diagnostics, document import/export, encrypted backup/restore, app lock.
- [ ] Windows 11: fresh install, navigation, in-process notification limitation messaging, document picker/share, backup/restore, keyboard navigation, theme changes.
- [ ] iPhone/iPad: fresh install, notification permission flow, notification delivery, backup/restore, document picker/share, app lock, Dynamic Type/VoiceOver checks.
- [ ] macOS/Mac Catalyst: fresh install, notifications, file operations, keyboard navigation, backup/restore, theme changes.
- [ ] Verify large-interface mode, reduced motion, screen-reader labels, focus order, contrast, and text scaling on representative devices.
- [ ] Verify all medical-safety disclaimers remain visible and no workflow implies diagnosis, dosage calculation, treatment recommendations, or guaranteed reminder delivery.

Use `docs/releases/MANUAL_TEST_MATRIX.md` as the evidence record.

**Done when:** the release checklist has device-specific evidence for every supported platform and no release-blocking defect remains.

### 3. Verify current app-store policy for the voluntary support link

CareNest exposes `https://buymeacoffee.com/sanskarIN` as optional project support and also publishes it through GitHub funding metadata.

Store rules for external funding/payment links can change. Before submitting a store build:

- [ ] Verify the current Apple App Store rules for external project-support/donation links.
- [ ] Verify the current Google Play rules for external project-support/donation links.
- [ ] Confirm the link is presented only as voluntary project support.
- [ ] Confirm no medical feature, health functionality, reminder behavior, support priority, or premium entitlement is unlocked by contributing.
- [ ] If a store disallows the in-app external support link, conditionally hide/remove that button for the affected store build while retaining repository funding links where permitted.

Automated tests already protect the fixed URL and voluntary/no-health-entitlement wording; they cannot determine current store policy.

**Done when:** store-review guidance for every distribution channel is documented and the shipped UI complies with that channel's current rules.

### 4. Prepare production signing and package identity

- [ ] Create Android signing key/keystore outside the repository and store secrets securely.
- [ ] Configure Android release signing in CI/release tooling without committing credentials.
- [ ] Configure Apple signing certificates, provisioning profiles, App Store Connect bundle identity, and entitlements outside the repository.
- [ ] Configure Windows signing/package identity if publishing through Microsoft Store or signed sideloading.
- [ ] Verify application identifiers, version numbers, display names, icons, splash assets, capabilities, and package metadata per platform.
- [ ] Document certificate/key backup and rotation procedures.

Automated repository policy tests reject common committed signing/secret file types, but credentials themselves must be provisioned externally.

**Done when:** reproducible signed release artifacts can be produced without placing private signing material in Git.

### 5. Finish store listing and privacy disclosures

- [ ] Produce final screenshots for phone/tablet/desktop targets using fictional data only.
- [ ] Produce required store icon, feature graphic, promotional graphic, and platform-specific screenshots.
- [ ] Write concise and long descriptions that match actual functionality.
- [ ] Complete privacy/data-safety questionnaires from the implemented local-first behavior rather than marketing assumptions.
- [ ] Confirm no analytics/telemetry claim is made unless analytics are actually added later with explicit consent and privacy review.
- [ ] Publish links to `PRIVACY.md`, `TERMS.md`, `SECURITY.md`, and support information.
- [ ] Re-check medical/health-category policy wording for each target store.

**Done when:** store metadata accurately describes the binary being submitted and matches CareNest's privacy/safety boundaries.

## Priority 1 — release promotion

### 6. Create the final production-candidate verification branch

The current hardening head has a green exact-head matrix, but the final production-candidate verification must happen **after** all Priority 0 work and any resulting source/configuration changes.

- [x] Exact-head marker-only verification protocol is documented in `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.
- [x] Current RC1 hardening source has a green exact-head automated baseline through PR #27.
- [ ] After Priority 0 blockers are complete, branch from the exact intended production-release commit.
- [ ] Trigger the complete GitHub Actions matrix.
- [ ] Require green formatting/core tests, Android, Windows, iOS simulator, Mac Catalyst, CodeQL, and Dependency Audit.
- [ ] Run `CareNest Release Evidence` for the exact promoted commit.
- [ ] Capture workflow run IDs in `what_changed.md`, `PROJECT_STATUS.md`, release checklist, and release notes.
- [ ] Close verification-only marker PR without merging its marker file.

### 7. Promote version metadata

- [ ] Decide final first public version (`1.0.0` or another pre-release).
- [ ] Update app version/build values consistently for Android, Apple, and Windows targets.
- [ ] Update `CHANGELOG.md` and release notes.
- [ ] Update `PROJECT_STATUS.md` from release-candidate status to actual shipped status.
- [ ] Create an annotated Git tag from the exact verified commit.

### 8. Build and archive release artifacts

- [ ] Android: generate intended signed AAB/APK artifact.
- [ ] iOS: archive signed app for App Store/TestFlight distribution.
- [ ] Mac Catalyst: create intended signed/notarized package or store-ready archive.
- [ ] Windows: create intended signed MSIX/package.
- [ ] Generate SHA-256 checksums for directly distributed artifacts.
- [ ] Keep release artifacts/provenance separate from source-control secrets.

## Priority 2 — post-release quality

### 9. Establish release-feedback loop without hidden telemetry

- [x] Use GitHub Issues and support email for explicit user-submitted bug reports.
- [x] Privacy-safe structured bug report form exists under `.github/ISSUE_TEMPLATE/bug_report.yml`.
- [x] Bug form requests version/platform/OS/time-zone/notification state/reproduction steps while warning users not to attach medical documents, credentials, backups, or private health information.
- [x] Sanitized diagnostics export remains opt-in/user-controlled in the application design.
- [ ] Triage real crashes/reminder reliability reports by platform/version after public release.
- [ ] Publish patch releases for confirmed defects after release.

### 10. Expand automated coverage

Completed hardening now includes:

- [x] repository safety/completeness policy contracts;
- [x] architecture dependency contracts;
- [x] ViewModel boundary contracts;
- [x] required data-model safety contracts;
- [x] branding/localization resource contracts;
- [x] async non-blocking source contracts;
- [x] logging-privacy source contracts;
- [x] global/UI/startup/reminder exception-log privacy regression contracts;
- [x] existing reminder recurrence/time-zone/SQLite/encryption/backup/report integration coverage.

Still useful later when stable target infrastructure exists:

- [ ] Add platform UI automation on real/emulated targets.
- [ ] Add deeper notification permission denial/retry state-transition automation where platform APIs can be reliably driven.
- [ ] Add additional daylight-saving gap/overlap zones.
- [ ] Add randomized/fuzz-style schedule-planner recurrence-boundary coverage.
- [ ] Add backup compatibility fixtures across future schema versions.
- [ ] Add file-corruption and low-storage target failure-path tests.
- [ ] Expand semantic/accessibility XAML contract coverage where meaningful without replacing manual assistive-technology testing.

### 11. Improve release engineering

Completed:

- [x] Dependency Audit workflow for pull requests.
- [x] Release Gate workflow blocks unresolved tracked dependency risk and incomplete release checklist.
- [x] Release Evidence workflow records exact source/ref/toolchain/test/dependency/checksum evidence.
- [x] Exact-head marker-only verification protocol documented and proven through multiple verification cycles.
- [x] Platform-neutral formatting enforced in CI.
- [x] CodeQL and multi-platform build matrix remain required automated gates.

Remaining optional/production improvements:

- [ ] Cache supported workloads/packages where this does not make verification stale or unsafe.
- [ ] Produce signed artifacts only from a protected release workflow after signing is configured.
- [ ] Add GitHub Dependency Review action if/when repository Dependency Graph is enabled; current NuGet Dependency Audit is the available gate.
- [ ] Add SBOM generation for release artifacts.
- [ ] Add artifact attestations/provenance where supported by the chosen distribution pipeline.

## Priority 3 — CareNest 1.x enhancements

These preserve the local-first, non-diagnostic boundary unless a future architecture decision explicitly expands infrastructure while retaining safety constraints.

### 12. Localization

- [ ] Move remaining hard-coded UI strings into resources.
- [ ] Add locale-aware date/time formatting while keeping machine-readable exports invariant where required.
- [ ] Add languages based on actual user demand.
- [ ] Add right-to-left layout testing before shipping an RTL locale.

### 13. Reminder usability

- [ ] Add clearer upcoming-reminder grouping/filtering.
- [ ] Add safe duplicate-schedule detection without inferring clinical intent.
- [ ] Add optional user-entered labels/colors for schedules.
- [ ] Improve explanation of OS delivery limitations per platform.
- [ ] Preserve explicit user-entered times and never silently calculate dosage.

### 14. Document organization

- [ ] Improve folder/tag filtering and search.
- [ ] Add duplicate-file detection based on local cryptographic hashes without uploading files.
- [ ] Add optional local thumbnails/previews with encrypted-source handling and cache cleanup.
- [ ] Add bulk export/delete actions with explicit confirmation.

### 15. Backup usability

- [ ] Add clearer backup-age status/reminders.
- [ ] Add optional local backup-history metadata without storing backup password.
- [ ] Add restore-preview metadata that remains non-sensitive.
- [ ] Add migration fixtures for each future schema version.

## Priority 4 — separately reviewed future versions

The following are intentionally not part of current local-only release and require new threat modeling, privacy design, authentication design, abuse analysis, and explicit user consent before implementation.

### Optional encrypted synchronization

Consider only after separate architecture/security review:

- end-to-end encrypted multi-device synchronization;
- user-controlled backup destination integrations;
- conflict handling/recovery;
- key rotation/device revocation;
- clear offline/deletion semantics.

### Optional remote caregiver collaboration

Consider only with explicit invitation/consent/revocation controls:

- no silent sharing;
- per-profile/per-data-category permissions;
- clear audit history;
- explicit expiration/revocation;
- no clinical interpretation or treatment recommendations.

### Optional accounts/authentication

If accounts are ever added, define first:

- what data must remain local;
- what data, if any, leaves device;
- account deletion/export behavior;
- encryption/key ownership;
- recovery model;
- breach-response process;
- jurisdiction/privacy obligations.

## Funding and sustainability

Current voluntary support URL:

`https://buymeacoffee.com/sanskarIN`

Funding remains separate from health behavior. Contributions must not change CareNest's medical-safety boundary, silently enable data sharing, imply medical advice, change reminder priority, or provide access to local user data.

Potential sustainable project paths to consider later:

- voluntary sponsorship/donations;
- paid convenience features that do not alter medical claims/core safety behavior, only after store-policy review;
- paid organizational support without access to user medical data unless separately designed and consented;
- consulting/custom-development work linked from maintainer profile rather than embedding sensitive service workflows into CareNest.

## Definition of done for public `1.0.0`

CareNest should be promoted from release candidate only when all applicable items are true:

- [ ] no unresolved production-blocking dependency risk remains without an explicit approved resolution/decision;
- [ ] complete automated formatting/test/build/CodeQL/Dependency Audit matrix is green on the exact final release commit;
- [ ] `CareNest Release Evidence` is generated for the exact promoted commit;
- [ ] manual supported-platform smoke tests are complete;
- [ ] notification limitations are manually verified/documented;
- [ ] backup/restore is tested on clean installations;
- [ ] accessibility checks are complete;
- [ ] store policy review is complete, including external voluntary-support link;
- [ ] signing keys/certificates are secured outside Git;
- [ ] privacy/data-safety disclosures match actual behavior;
- [ ] release notes/changelog/status/handoff documents are updated;
- [ ] signed release artifacts are archived with exact version/provenance information.

Current automated RC1 hardening is green, but those remaining manual/security/distribution conditions intentionally keep final `1.0.0` publication blocked.
