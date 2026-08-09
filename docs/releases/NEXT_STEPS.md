# CareNest Next Steps

This document tracks the work to consider after the source-complete `1.0.0-rc.1` milestone. It intentionally separates release blockers from optional future versions so unfinished future ideas are not confused with missing RC1 implementation.

## Priority 0 — production-release blockers

These items should be completed before promoting the release candidate to a public production release.

### 1. Resolve the open SQLite dependency advisory

- [ ] Re-check the current `sqlite-net-pcl` / `SQLitePCLRaw` dependency graph against NuGet audit.
- [ ] Upgrade to a compatible patched native SQLite package path when one is actually available.
- [ ] Remove the narrow `NuGetAuditSuppress` entry after the dependency graph no longer resolves the affected package/advisory.
- [ ] Run unit, integration, UI-contract, Android, Windows, iOS simulator, Mac Catalyst, and CodeQL verification again.
- [ ] Update `docs/security/DEPENDENCY_RISK_REGISTER.md` with the exact resolution and evidence.

**Done when:** no unresolved high-severity dependency advisory remains in the release dependency graph and CI is green without the temporary audit exception.

### 2. Run manual device and accessibility smoke testing

Automated CI proves compilation and contracts, but it does not replace real-device behavior testing.

- [ ] Android phone: fresh install, onboarding, notification permission denied/granted, exact/inexact alarm behavior, reboot rebuild, time-zone change, battery-optimization diagnostics, document import/export, encrypted backup/restore, app lock.
- [ ] Windows 11: fresh install, navigation, in-process notification limitation messaging, document picker/share, backup/restore, keyboard navigation, theme changes.
- [ ] iPhone/iPad: fresh install, notification permission flow, notification delivery, backup/restore, document picker/share, app lock, Dynamic Type/VoiceOver checks.
- [ ] macOS/Mac Catalyst: fresh install, notifications, file operations, keyboard navigation, backup/restore, theme changes.
- [ ] Verify large-interface mode, reduced motion, screen-reader labels, focus order, contrast, and text scaling on representative devices.
- [ ] Verify all medical-safety disclaimers remain visible and no workflow implies diagnosis, dosage calculation, treatment recommendations, or guaranteed reminder delivery.

**Done when:** the release checklist has device-specific evidence for every supported platform and no release-blocking defect remains.

### 3. Verify current app-store policy for the voluntary support link

CareNest now exposes `https://buymeacoffee.com/sanskarIN` as an optional project-support link and also publishes it through GitHub funding metadata.

Store rules for external funding/payment links can change. Before submitting a store build:

- [ ] Verify the current Apple App Store rules for external project-support/donation links.
- [ ] Verify the current Google Play rules for external project-support/donation links.
- [ ] Confirm the link is presented only as voluntary project support.
- [ ] Confirm no medical feature, health functionality, reminder behavior, support priority, or premium entitlement is unlocked by contributing.
- [ ] If a store disallows the in-app external support link, conditionally hide/remove that button for the affected store build while retaining the repository funding link where permitted.

**Done when:** store-review guidance for every distribution channel is documented and the shipped UI complies with that channel's current rules.

### 4. Prepare production signing and package identity

- [ ] Create Android signing key/keystore outside the repository and store secrets securely.
- [ ] Configure Android release signing in CI/release tooling without committing credentials.
- [ ] Configure Apple signing certificates, provisioning profiles, App Store Connect bundle identity, and entitlements outside the repository.
- [ ] Configure Windows signing/package identity if publishing through Microsoft Store or signed sideloading.
- [ ] Verify application identifiers, version numbers, display names, icons, splash assets, capabilities, and package metadata per platform.
- [ ] Document certificate/key backup and rotation procedures.

**Done when:** reproducible signed release artifacts can be produced without placing private signing material in Git.

### 5. Finish store listing and privacy disclosures

- [ ] Produce final screenshots for phone/tablet/desktop targets.
- [ ] Produce required store icon, feature graphic, promotional graphic, and platform-specific screenshots.
- [ ] Write concise and long descriptions that match actual functionality.
- [ ] Complete privacy/data-safety questionnaires from the implemented local-first behavior rather than marketing assumptions.
- [ ] Confirm no analytics/telemetry claim is made unless analytics are actually added later with explicit consent and privacy review.
- [ ] Publish links to `PRIVACY.md`, `TERMS.md`, `SECURITY.md`, and support information.
- [ ] Re-check medical/health-category policy wording for each target store.

**Done when:** store metadata accurately describes the binary being submitted and matches CareNest's privacy/safety boundaries.

## Priority 1 — release promotion

### 6. Create a final release verification branch

After all Priority 0 blockers are complete:

- [ ] Branch from the exact intended release commit.
- [ ] Trigger the complete GitHub Actions matrix.
- [ ] Require green core tests, Android, Windows, iOS simulator, Mac Catalyst, and CodeQL.
- [ ] Capture the workflow run IDs in `what_changed.md` and the release notes.
- [ ] Do not merge verification-only marker files into `main`.

### 7. Promote version metadata

- [ ] Decide whether the first public version is `1.0.0` or another pre-release build.
- [ ] Update app version/build values consistently for Android, Apple, and Windows targets.
- [ ] Update `CHANGELOG.md` and release notes.
- [ ] Update `PROJECT_STATUS.md` from release-candidate status to the actual shipped status.
- [ ] Create an annotated Git tag from the exact verified commit.

### 8. Build and archive release artifacts

- [ ] Android: generate the intended signed AAB/APK artifact.
- [ ] iOS: archive the signed app for App Store/TestFlight distribution.
- [ ] Mac Catalyst: create the intended signed/notarized package if distributed outside the Mac App Store, or the store-ready archive if using the store.
- [ ] Windows: create the intended signed MSIX/package.
- [ ] Generate SHA-256 checksums for directly distributed artifacts.
- [ ] Keep release artifacts and provenance metadata separate from source-control secrets.

## Priority 2 — post-release quality

### 9. Establish a release-feedback loop without hidden telemetry

- [ ] Use GitHub Issues and the support email for explicit user-submitted bug reports.
- [ ] Keep sanitized diagnostics export opt-in and user-controlled.
- [ ] Add a structured bug-report template asking for app version, OS version, time zone, notification permission state, and reproduction steps while explicitly telling users not to attach medical documents or sensitive health details.
- [ ] Triage crashes and reminder reliability reports by platform and version.
- [ ] Publish patch releases for confirmed defects.

### 10. Expand automated coverage

- [ ] Add platform UI automation where stable device/emulator infrastructure is available.
- [ ] Add tests for notification permission denial/retry state transitions.
- [ ] Add tests for daylight-saving gaps/overlaps across additional time zones.
- [ ] Add randomized/fuzz-style schedule-planner tests for recurrence boundaries.
- [ ] Add backup compatibility fixtures across schema versions.
- [ ] Add file-corruption and low-storage failure-path tests.
- [ ] Add accessibility contract checks for important XAML pages.

### 11. Improve release engineering

- [ ] Cache supported workloads/packages in CI where this does not make verification stale or unsafe.
- [ ] Produce signed artifacts only from protected release workflows.
- [ ] Add dependency-review automation for pull requests.
- [ ] Add SBOM generation for release artifacts.
- [ ] Add artifact attestations/provenance where supported by the chosen distribution pipeline.
- [ ] Add a release workflow that fails if the dependency risk register contains an unresolved production blocker.

## Priority 3 — CareNest 1.x enhancements

These should preserve the local-first, non-diagnostic design unless a later architecture decision explicitly changes that boundary.

### 12. Localization

- [ ] Move remaining hard-coded UI strings into resources.
- [ ] Add locale-aware date/time formatting while keeping machine-readable exports invariant where required.
- [ ] Start with languages selected from actual user demand.
- [ ] Add right-to-left layout testing before shipping an RTL locale.

### 13. Reminder usability

- [ ] Add clearer upcoming-reminder grouping and filtering.
- [ ] Add safe duplicate-schedule detection without inferring clinical intent.
- [ ] Add optional user-entered labels/colors for schedules.
- [ ] Improve explanation of operating-system delivery limitations per platform.
- [ ] Preserve explicit user-entered times and never silently calculate dosage.

### 14. Document organization

- [ ] Improve folder/tag filtering and search.
- [ ] Add duplicate-file detection based on local cryptographic hashes without uploading files.
- [ ] Add optional local thumbnails/previews with encrypted-source handling and cache cleanup.
- [ ] Add bulk export/delete actions with explicit confirmation.

### 15. Backup usability

- [ ] Add clearer backup-age status and reminders.
- [ ] Add optional local backup-history metadata without storing the backup password.
- [ ] Add restore-preview metadata that remains non-sensitive.
- [ ] Add migration fixtures for every future schema version.

## Priority 4 — separately reviewed future versions

The following are intentionally not part of the current local-only release and require new threat modeling, privacy design, authentication design, abuse analysis, and explicit user consent before implementation.

### Optional encrypted synchronization

Consider only after a separate architecture/security review:

- end-to-end encrypted multi-device synchronization;
- user-controlled backup destination integrations;
- conflict handling and recovery;
- key rotation and device revocation;
- clear offline behavior and deletion semantics.

### Optional remote caregiver collaboration

Consider only with explicit invitation/consent and revocation controls:

- no silent sharing;
- per-profile/per-data-category permissions;
- clear audit history;
- explicit expiration/revocation;
- no clinical interpretation or treatment recommendations.

### Optional accounts/authentication

If accounts are ever added, define first:

- what data must remain local;
- what data, if any, leaves the device;
- account deletion and export behavior;
- encryption/key ownership;
- recovery model;
- breach-response process;
- jurisdiction/privacy obligations.

## Funding and sustainability

Current voluntary support URL:

`https://buymeacoffee.com/sanskarIN`

Funding should remain separate from health behavior. Contributions must not change CareNest's medical-safety boundary, silently enable data sharing, or imply that paying users receive medical advice.

Potential sustainable project paths to consider later:

- voluntary sponsorship/donations;
- paid convenience features that do not alter medical claims or core safety behavior, only after store-policy review;
- paid support for organizations without access to user medical data unless explicitly and separately designed;
- consulting/custom-development work linked from the maintainer profile rather than embedding sensitive service workflows into CareNest.

## Definition of done for a public `1.0.0`

CareNest should be promoted from release candidate only when all of the following are true:

- [ ] no known unresolved production-blocking dependency vulnerability remains;
- [ ] complete automated test/build/CodeQL matrix is green on the exact release commit;
- [ ] manual supported-platform smoke tests are complete;
- [ ] notification limitations are verified and documented;
- [ ] backup/restore is tested on clean installations;
- [ ] accessibility checks are complete;
- [ ] store policy review is complete, including the external voluntary-support link;
- [ ] signing keys/certificates are secured outside Git;
- [ ] privacy/data-safety disclosures match actual behavior;
- [ ] release notes/changelog/status documents are updated;
- [ ] signed release artifacts are archived with reproducible version/provenance information.
