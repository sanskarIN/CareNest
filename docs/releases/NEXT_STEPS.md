# CareNest Next Steps

This document tracks work after the source-complete `1.0.0-rc.1` milestone. It intentionally separates release blockers from completed hardening and optional future versions so unfinished future ideas are not confused with missing RC1 implementation.

## Current automated hardening candidate

The earlier PR #33 baseline is historical. The 2026-08-14 repository-wide correctness audit continued far beyond that source and intentionally used failure-driven exact-source checkpoints rather than hiding test/analyzer defects.

Current exact runtime/test source through:

`da2aed19ee9224b8d8661f11520ab9396e2c005e`

is being verified by marker-only PR #53:

`Final CareNest bug-audit verification`

Required automated groups are:

- platform-neutral formatting;
- complete unit-test suite;
- complete integration-test suite;
- complete UI-contract/policy suite;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- CodeQL;
- unsuppressed Dependency Audit.

At the time of this handoff update:

- formatting is green;
- **122 unit tests** are green;
- **39 integration tests** are green;
- **100 UI-contract/policy tests** are green;
- **261 total core automated tests** are green;
- CodeQL #501 / run `31766026573` is green;
- Dependency Audit #34 / run `31766026570` is green with the old SQLite advisory suppression removed;
- platform Release jobs are still the remaining PR #53 CI gates and must be read from GitHub Actions before PR #53 is declared the final green automated baseline.

The marker file is verification-only and must not be merged into `main`.

This source includes the earlier hardening plus the later 2026-08-14 corrections for:

- snoozed/stale reminder effective-due reconciliation;
- platform cancellation before reminder replacement/suppression/invalidation;
- medicine/profile delete compensation;
- schedule occurrence-row preservation until platform reconciliation;
- medicine/profile save-time reminder reconciliation;
- appointment reminder persistence compensation;
- cancellation-first reminder actions with recovery;
- notification failure-injection coverage;
- report-cache cleanup after sharing;
- analyzer-safe direct reminder-reconciliation tests;
- SQLite native/provider dependency remediation and audit-suppression removal.

Verification history remains intentionally failure-driven. PR #43 was not green; PRs #44, #46 and #49 exposed additional integration/UI/analyzer defects; PRs #47/#48/#50 exercised the SQLite remediation while source was moving; PRs #51/#52 were superseded when later source changed. None of their marker files should be merged or reused as final release evidence.

This automated source work does not complete the production-release blockers below.

## Priority 0 — production-release blockers

These items must be completed before promoting the release candidate to a public production release.

### 1. SQLite dependency remediation — source completed, release compatibility evidence remains

The previous `GHSA-2m69-gcr7-jv3q` source blocker is remediated in the current dependency graph.

Completed source work:

- [x] Re-check the `sqlite-net-pcl` / `SQLitePCLRaw` dependency graph through repository Dependency Audit workflows.
- [x] Keep `sqlite-net-pcl` `1.9.172` and `SQLitePCLRaw.bundle_green` `2.1.11` while centrally pinning maintained native/provider leaves.
- [x] Pin `SQLitePCLRaw.lib.e_sqlite3` to `3.53.3`.
- [x] Pin `SQLitePCLRaw.lib.e_sqlite3.android` to `2.1.12`.
- [x] Pin the selected SQLitePCLRaw providers to `2.1.12`.
- [x] Remove the narrow `NuGetAuditSuppress` entry for `GHSA-2m69-gcr7-jv3q`.
- [x] Add `SqliteDependencySecurityContractTests` so the old vulnerable pin floor/suppression cannot silently return.
- [x] Re-run unsuppressed Dependency Audit successfully during multiple remediation checkpoints.
- [x] Update `docs/security/DEPENDENCY_RISK_REGISTER.md` with the resolved-in-source graph and evidence.
- [x] Update `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` with the selected migration path.

Current source commits:

- `66cd701f84afd5021a28e7e3327b7da4fad249aa` — dependency pins;
- `e939d5bd912d09ffa150c804519c15e2506b7bd7` — audit-suppression removal;
- `04868965c43d8a6d09b40075d92f20da9b26e32a` — dependency regression contract.

Still required before public promotion because dependency security and user-data compatibility are separate release concerns:

- [ ] Finish the complete PR #53 final-source platform matrix.
- [ ] Upgrade/install a representative build containing fictional RC1 data and verify the database remains readable.
- [ ] Verify profiles, medicines, schedules, reminder rows, logs, appointments, documents, stock and tags after upgrade.
- [ ] Verify pre-remediation and post-remediation encrypted backups on packaged targets where canonical fixtures are available.
- [ ] Verify encrypted document payloads continue to decrypt through the existing key path.
- [ ] Verify reminder rebuild/reconciliation after upgrade.
- [ ] Record manual compatibility evidence in the release matrix/evidence documents.

**Current state:** dependency remediation is complete in source and the exact NuGet audit suppression is removed. Do not re-open the old exception merely because manual compatibility evidence is still outstanding; track those checks as release-validation work.

### 2. Run manual device, encrypted-data, and accessibility smoke testing

Automated CI proves compilation/contracts, but it does not replace real-device behavior testing.

- [ ] Android phone: fresh install, onboarding, notification permission denied/granted, appointment permission denied/granted, exact/inexact alarm behavior, reboot rebuild, time-zone change, battery diagnostics, v2 document import/export, v2 encrypted backup/restore, app lock.
- [ ] Windows 11: fresh install, navigation, in-process notification limitation messaging, appointment reminder behavior, document picker/share, backup/restore, keyboard navigation, theme changes.
- [ ] iPhone/iPad: fresh install, notification permission flow, appointment reminder permission behavior, notification delivery, backup/restore, document picker/share, app lock, Dynamic Type/VoiceOver checks.
- [ ] macOS/Mac Catalyst: fresh install, notifications, appointment reminders, file operations, keyboard navigation, backup/restore, theme changes.
- [ ] Verify snooze behavior against actual platform notification scheduling.
- [ ] Verify cancellation-first reminder actions against actual platform scheduling and app restart/recovery.
- [ ] Verify large-interface mode, reduced motion, screen-reader labels, focus order, contrast, and text scaling on representative devices.
- [ ] Verify all medical-safety disclaimers remain visible and no workflow implies diagnosis, dosage calculation, treatment recommendations, or guaranteed reminder delivery.

Use `docs/releases/MANUAL_TEST_MATRIX.md` as the evidence record.

### 3. Verify encrypted-data backward compatibility with canonical fixtures

New encrypted streams use framing v2 while the reader retains v1 compatibility.

- [ ] Preserve/create a canonical encrypted document generated by a historical released/verified v1 build using synthetic data.
- [ ] Verify that canonical v1 encrypted document opens/exports correctly in the intended production build.
- [ ] Preserve/create a canonical backup payload generated by a historical released/verified v1 framing path using synthetic data.
- [ ] Verify that canonical v1 backup can still be inspected/restored by the intended production build.
- [ ] Verify new v2 encrypted document import/export in packaged target builds.
- [ ] Verify new v2 encrypted backup create/inspect/restore in packaged target builds.
- [ ] Do not remove v1 read support until an explicit migration/deprecation plan is reviewed and historical compatibility evidence exists.

Automated tests already prove a handcrafted legacy-v1 stream remains decryptable and that v2 rejects truncation/trailing data. Canonical historical fixtures are still useful release evidence because they exercise real previously generated file bytes rather than a test-only fixture builder.

### 4. Verify current app-store policy for the voluntary support link

CareNest exposes `https://buymeacoffee.com/sanskarIN` as optional project support and also publishes it through GitHub funding metadata.

Store rules can change. Before submitting a store build:

- [ ] Verify current Apple App Store rules for external project-support/donation links.
- [ ] Verify current Google Play rules for external project-support/donation links.
- [ ] Confirm the link is presented only as voluntary project support.
- [ ] Confirm no medical feature, health functionality, reminder behavior, support priority, or premium entitlement is unlocked by contributing.
- [ ] If a store disallows the in-app external support link, conditionally hide/remove that button for the affected store build while retaining repository funding links where permitted.

### 5. Prepare production signing and package identity

- [ ] Create Android signing key/keystore outside the repository and store secrets securely.
- [ ] Configure Android release signing in CI/release tooling without committing credentials.
- [ ] Configure Apple signing certificates, provisioning profiles, App Store Connect bundle identity, and entitlements outside the repository.
- [ ] Configure Windows signing/package identity if publishing through Microsoft Store or signed sideloading.
- [ ] Verify application identifiers, version numbers, display names, icons, splash assets, capabilities, and package metadata per platform.
- [ ] Document certificate/key backup and rotation procedures.

### 6. Finish store listing and privacy disclosures

- [ ] Produce final screenshots for phone/tablet/desktop targets using fictional data only.
- [ ] Produce required store icon, feature graphic, promotional graphic, and platform-specific screenshots.
- [ ] Write concise and long descriptions that match actual functionality.
- [ ] Complete privacy/data-safety questionnaires from implemented local-first behavior rather than marketing assumptions.
- [ ] Publish links to `PRIVACY.md`, `TERMS.md`, `SECURITY.md`, and support information.
- [ ] Re-check health-category policy wording for each target store.

## Priority 1 — release promotion

### 7. Create the final production-candidate verification branch

PR #53 is the current automated bug-audit verification candidate, but the final **production-candidate** verification must happen after all applicable Priority 0 manual/security/distribution work and any source/configuration changes that result from it.

- [x] Exact-head marker-only verification protocol is documented in `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.
- [x] The 2026-08-14 bug audit uses failure-driven marker-only verification rather than reusing stale successful subsets.
- [ ] Confirm every required PR #53 gate is green before naming it the current automated RC1 baseline.
- [ ] Close PR #53 without merging its marker after evidence is recorded.
- [ ] After Priority 0 blockers are complete, branch from the exact intended production-release commit.
- [ ] Trigger the complete GitHub Actions matrix.
- [ ] Require green formatting/core tests, Android, Windows, iOS simulator, Mac Catalyst, CodeQL, and Dependency Audit.
- [ ] Run `CareNest Release Evidence` for the exact promoted commit.
- [ ] Capture workflow run IDs in `what_changed.md`, `PROJECT_STATUS.md`, release checklist, and release notes.
- [ ] Close verification-only marker PR without merging its marker file.

### 8. Promote version metadata

- [ ] Decide final first public version (`1.0.0` or another pre-release).
- [ ] Update app version/build values consistently for Android, Apple, and Windows targets.
- [ ] Update `CHANGELOG.md` and release notes.
- [ ] Update `PROJECT_STATUS.md` from release-candidate status to actual shipped status.
- [ ] Create an annotated Git tag from the exact verified commit.

### 9. Build and archive release artifacts

- [ ] Android: generate intended signed AAB/APK artifact.
- [ ] iOS: archive signed app for App Store/TestFlight distribution.
- [ ] Mac Catalyst: create intended signed/notarized package or store-ready archive.
- [ ] Windows: create intended signed MSIX/package.
- [ ] Generate SHA-256 checksums for directly distributed artifacts.
- [ ] Keep release artifacts/provenance separate from source-control secrets.

## Priority 2 — post-release quality

### 10. Establish release-feedback loop without hidden telemetry

- [x] Use GitHub Issues and support email for explicit user-submitted bug reports.
- [x] Privacy-safe structured bug report form exists under `.github/ISSUE_TEMPLATE/bug_report.yml`.
- [x] Bug form requests version/platform/OS/time-zone/notification state/reproduction steps while warning users not to attach medical documents, credentials, backups, or private health information.
- [x] Sanitized diagnostics export remains opt-in/user-controlled in the application design.
- [ ] Triage real crashes/reminder/encrypted-data reliability reports by platform/version after public release.
- [ ] Publish patch releases for confirmed defects after release.

### 11. Expand automated coverage

Completed hardening now includes:

- [x] repository safety/completeness policy contracts;
- [x] architecture dependency contracts;
- [x] ViewModel boundary contracts;
- [x] required data-model safety contracts;
- [x] branding/localization resource contracts;
- [x] async non-blocking source contracts;
- [x] logging-privacy source contracts;
- [x] global/UI/startup/reminder exception-log privacy regression contracts;
- [x] deterministic reminder recurrence/window/date/state contracts;
- [x] selected-weekday/cycle/every-N-hours validation boundaries;
- [x] planner ownership validation and archived-profile suppression;
- [x] UTC-kind validation for planner/rebuild/snooze boundaries;
- [x] DST gap/overlap coverage for representative North America, Europe, Australia and New Zealand zones when available;
- [x] deterministic randomized/property-style recurrence coverage with a fixed seed;
- [x] direct profile/medicine/appointment/document/backup-reminder service tests;
- [x] appointment explicit-UTC and denied-permission fail-safe tests;
- [x] appointment reminder persistence compensation tests;
- [x] reminder action cancellation/recovery ordering tests;
- [x] notification scheduling/cancellation failure-injection support;
- [x] medicine/profile reminder reconciliation and compensation tests;
- [x] analyzer-safe reminder reconciliation expectations;
- [x] document import rollback and safe export tests;
- [x] WAL snapshot creation/content/integrity/pre-cancellation coverage;
- [x] app-lock cryptographic/source security contracts and verifier-buffer clearing;
- [x] document/backup caller-owned key-buffer hygiene tests;
- [x] strict backup ZIP topology tests;
- [x] chunked AEAD v2 round-trip/prefix-truncation/trailing-data tests;
- [x] legacy chunked AEAD v1 read-compatibility test;
- [x] existing encryption/backup/report integration coverage;
- [x] SQLite dependency-security pin/suppression contract.

Still useful later when stable target infrastructure exists:

- [ ] Add platform UI automation on real/emulated targets.
- [ ] Add deeper notification permission denial/retry state-transition automation where platform APIs can be reliably driven.
- [ ] Add canonical historical backup/document compatibility fixtures across future formats.
- [ ] Add file-corruption and low-storage target failure-path tests.
- [ ] Expand semantic/accessibility XAML contract coverage where meaningful without replacing manual assistive-technology testing.

### 12. Improve release engineering

Completed:

- [x] Dependency Audit workflow for pull requests.
- [x] Release Gate workflow blocks unresolved tracked dependency risk and incomplete release checklist.
- [x] Release Evidence workflow records exact source/ref/toolchain/test/dependency/checksum evidence.
- [x] Exact-head marker-only verification protocol documented and proven through multiple verification cycles, including analyzer-failure supersession rather than stale evidence reuse.
- [x] Platform-neutral formatting enforced in CI.
- [x] CodeQL and multi-platform build matrix remain required automated gates.
- [x] Narrow SQLite audit exception removed after a compatible dependency graph was established.
- [x] Dependency regression contract prevents silent restoration of the old SQLite pin/suppression baseline.

Remaining optional/production improvements:

- [ ] Cache supported workloads/packages where this does not make verification stale or unsafe.
- [ ] Produce signed artifacts only from a protected release workflow after signing is configured.
- [ ] Add GitHub Dependency Review action if/when repository Dependency Graph is enabled; current NuGet Dependency Audit is the available gate.
- [ ] Add SBOM generation for release artifacts.
- [ ] Add artifact attestations/provenance where supported by the chosen distribution pipeline.

## Priority 3 — CareNest 1.x enhancements

These preserve the local-first, non-diagnostic boundary unless a future architecture decision explicitly expands infrastructure while retaining safety constraints.

### 13. Localization

- [ ] Move remaining hard-coded UI strings into resources.
- [ ] Add locale-aware date/time formatting while keeping machine-readable exports invariant where required.
- [ ] Add languages based on actual user demand.
- [ ] Add right-to-left layout testing before shipping an RTL locale.

### 14. Reminder usability

- [ ] Add clearer upcoming-reminder grouping/filtering.
- [ ] Add safe duplicate-schedule detection without inferring clinical intent.
- [ ] Add optional user-entered labels/colors for schedules.
- [ ] Improve explanation of OS delivery limitations per platform.
- [ ] Preserve explicit user-entered times and never silently calculate dosage.

### 15. Document organization

- [ ] Improve folder/tag filtering and search.
- [ ] Add duplicate-file detection based on local cryptographic hashes without uploading files.
- [ ] Add optional local thumbnails/previews with encrypted-source handling and cache cleanup.
- [ ] Add bulk export/delete actions with explicit confirmation.

### 16. Backup usability

- [ ] Add clearer backup-age status/reminders.
- [ ] Add optional local backup-history metadata without storing backup password.
- [ ] Add restore-preview metadata that remains non-sensitive.
- [ ] Add migration fixtures for each future schema/package/encrypted-stream version.

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

## Definition of done for public `1.0.0`

CareNest should be promoted from release candidate only when all applicable items are true:

- [x] no known production-blocking SQLite advisory is being hidden by the former `GHSA-2m69-gcr7-jv3q` exception; the current source graph removes that suppression and guards the maintained native/provider floor;
- [ ] complete automated formatting/test/build/CodeQL/Dependency Audit matrix is green on the exact final release commit;
- [ ] `CareNest Release Evidence` is generated for the exact promoted commit;
- [ ] manual supported-platform smoke tests are complete;
- [ ] existing-database/SQLite compatibility checks are complete on packaged representative builds;
- [ ] notification/appointment limitations are manually verified/documented;
- [ ] new v2 document/backup workflows are tested in packaged builds;
- [ ] retained v1 encrypted-data compatibility is verified with canonical historical fixtures when available;
- [ ] backup/restore is tested on clean installations;
- [ ] accessibility checks are complete;
- [ ] store policy review is complete, including external voluntary-support link;
- [ ] signing keys/certificates are secured outside Git;
- [ ] privacy/data-safety disclosures match actual behavior;
- [ ] release notes/changelog/status/handoff documents are updated;
- [ ] signed release artifacts are archived with exact version/provenance information.

Current automated RC1 source hardening is substantially complete, but the remaining manual/device/accessibility/store/signing/distribution conditions intentionally keep final `1.0.0` publication blocked until actual evidence exists.
