# CareNest Next Steps

This document tracks work after the source-complete `1.0.0-rc.1` milestone. It intentionally separates release blockers from completed hardening and optional future versions so unfinished future ideas are not confused with missing RC1 implementation.

## Current source and last completed exact automated baseline

The current `main` source is newer than the last completed marker-only exact-head verification because the 2026-08-15 continuation changed application project configuration, About presentation source, UI/source-policy tests and release-preflight scripts for store/package release readiness.

PR #56 remains the last completed exact automated baseline for its frozen source boundary:

`Verify complete CareNest release-engineering source`

Frozen source/base SHA:

`4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`

Final evidence:

- CareNest CI #571 / run `31770929379`: **success**;
- platform-neutral formatting: **success**;
- **122 unit tests**: passed;
- **39 integration tests**: passed;
- **124 UI-contract/policy tests**: passed;
- **285 total automated tests**: passed;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- CodeQL #571 / run `31770929382`: **success**;
- unsuppressed Dependency Audit #41 / run `31770929383`: **success**.

PR #56 is closed without merge. Its verification marker `build/verification/release-engineering-final-v2-20260814.txt` is not part of `main`.

`docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md` records that frozen-source evidence. PR #54 remains the historical authoritative runtime bug-audit baseline for the earlier runtime/test/dependency graph.

The PR #56 source includes final reminder/document/backup/SQLite hardening plus release-engineering controls for:

- exact `v*` tag execution of CareNest CI, CodeQL, Dependency Audit, Release Gate and Release Evidence;
- manual workflow entry points where configured;
- PR-only dependency metadata guarded from tag/manual runs;
- failure-preserving Release Evidence with tracked-source provenance/checksums and rerun-safe artifact identity;
- blocking unsuppressed dependency audit in local quality/preflight scripts;
- repository-local Git setup that verifies `Sanskar` / `sanskarin@outlook.in` and fails on Git errors;
- fail-closed production Release Gate matching;
- executable workflow/script/release-policy contracts;
- active architecture/security/setup/release docs aligned with the remediated SQLite graph and cancellation-first reminder model.

The newer 2026-08-15 source additionally completes these source-side release-readiness items:

- [x] Add `CareNestShowFundingLink`, default `true`, so the external voluntary support surface can be disabled for a specific store build without a source fork.
- [x] Hide the complete About support card when `CareNestShowFundingLink=false` while leaving the organizer/legal/support surfaces unchanged.
- [x] Add source-policy regression coverage for the funding-link build switch and voluntary/no-health-entitlement wording.
- [x] Add package metadata/privacy contracts for application identity/version, target/minimum OS declarations, Android permissions/backup/cleartext/local-first network posture, Apple purpose strings/transport posture, Windows package metadata and branding assets.
- [x] Wire `CARENEST_SHOW_FUNDING_LINK=true|false` through Bash and PowerShell release-preflight scripts with fail-closed validation.
- [x] Add `docs/releases/STORE_BUILD_POLICY.md`.
- [x] Add `docs/releases/PACKAGED_RELEASE_VALIDATION.md`.
- [x] Synchronize `what_changed.md`, `PROJECT_STATUS.md`, `CHANGELOG.md`, the documentation hub and this next-steps file to the newer source boundary.

Because verification-relevant source changed after PR #56, a new exact-head marker verification is now required after this continuation stabilizes. PR #56 is not relabeled as proof of the newer head.

Verification history remains intentionally failure-driven. PR #43 was not green; PRs #44, #46 and #49 exposed additional integration/UI/analyzer defects; PRs #47/#48/#50 exercised the SQLite remediation while source was moving; PRs #51/#52 were superseded; PR #54 completed the runtime bug-audit baseline; PR #55 verified the first release-engineering snapshot but was superseded when the complete-file audit found further legitimate fixes; PR #56 completed the last frozen-source release-engineering baseline.

This automated/source work does not complete the production-release blockers below.

## Priority 0 — production-release blockers

These items must be completed before promoting the release candidate to a public production release.

### 1. SQLite dependency remediation — source completed, release compatibility evidence remains

The previous `GHSA-2m69-gcr7-jv3q` source dependency exception is remediated in the verified dependency graph.

Completed source work:

- [x] Re-check the `sqlite-net-pcl` / `SQLitePCLRaw` dependency graph through repository Dependency Audit workflows.
- [x] Keep `sqlite-net-pcl` `1.9.172` and `SQLitePCLRaw.bundle_green` `2.1.11` while centrally pinning maintained native/provider leaves.
- [x] Pin `SQLitePCLRaw.lib.e_sqlite3` to `3.53.3`.
- [x] Pin `SQLitePCLRaw.lib.e_sqlite3.android` to `2.1.12`.
- [x] Pin the selected SQLitePCLRaw providers to `2.1.12`.
- [x] Remove the narrow `NuGetAuditSuppress` entry for `GHSA-2m69-gcr7-jv3q`.
- [x] Add `SqliteDependencySecurityContractTests` so the old vulnerable pin floor/suppression cannot silently return.
- [x] Re-run unsuppressed Dependency Audit successfully during multiple remediation checkpoints.
- [x] Complete unsuppressed Dependency Audit #41 / run `31770929383` on PR #56's frozen source.
- [x] Complete all 285 automated tests and all four platform Release builds on that same verified source boundary.
- [x] Update `docs/security/DEPENDENCY_RISK_REGISTER.md` with the resolved-in-source graph and evidence.
- [x] Update `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` with the selected migration path.
- [x] Add `docs/releases/PACKAGED_RELEASE_VALIDATION.md` to define the packaged compatibility evidence process without pretending it has been executed.

Current source commits for dependency remediation:

- `66cd701f84afd5021a28e7e3327b7da4fad249aa` — dependency pins;
- `e939d5bd912d09ffa150c804519c15e2506b7bd7` — audit-suppression removal;
- `04868965c43d8a6d09b40075d92f20da9b26e32a` — dependency regression contract.

Still required before public promotion because dependency security and user-data compatibility are separate release concerns:

- [ ] Upgrade/install a representative packaged build containing fictional RC1 data and verify the database remains readable.
- [ ] Verify profiles, medicines, schedules, reminder rows, logs, appointments, documents, stock and tags after upgrade.
- [ ] Verify pre-remediation and post-remediation encrypted backups on packaged targets where canonical fixtures are available.
- [ ] Verify encrypted document payloads continue to decrypt through the existing key path.
- [ ] Verify reminder rebuild/reconciliation after upgrade.
- [ ] Record manual compatibility evidence in the release matrix/evidence documents.

Use `docs/releases/PACKAGED_RELEASE_VALIDATION.md` together with `docs/releases/MANUAL_TEST_MATRIX.md`.

**Current state:** dependency remediation and unsuppressed automated verification are complete for the frozen PR #56 source; the maintained dependency source remains in current `main`. Do not re-open the old exception merely because manual compatibility evidence is still outstanding; track those checks as release-validation work.

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

Use `docs/releases/MANUAL_TEST_MATRIX.md` as the evidence record and `docs/releases/PACKAGED_RELEASE_VALIDATION.md` as the execution runbook.

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

CareNest's default open-source build can expose `https://buymeacoffee.com/sanskarIN` as optional project support and GitHub funding metadata also references it.

Source-side mitigation is complete:

- [x] Add a store-specific build switch instead of requiring a source fork.
- [x] Default `CareNestShowFundingLink` to `true` for normal/open-source builds.
- [x] Make `CareNestShowFundingLink=false` hide the complete About support card.
- [x] Add regression coverage for that behavior.
- [x] Pass `CARENEST_SHOW_FUNDING_LINK` through both release-preflight scripts.
- [x] Document exact enabled/disabled build commands and evidence requirements in `docs/releases/STORE_BUILD_POLICY.md`.

Store rules can change. Before submitting each store build:

- [ ] Verify current Apple App Store rules for external project-support/donation links.
- [ ] Verify current Google Play rules for external project-support/donation links.
- [ ] Record policy source/date/reviewer/conclusion.
- [ ] Confirm the link is presented only as voluntary project support when enabled.
- [ ] Confirm no medical feature, health functionality, reminder behavior, support priority, or premium entitlement is unlocked by contributing.
- [ ] Select `CareNestShowFundingLink=true` only where the current store policy permits it.
- [ ] Use `CareNestShowFundingLink=false` when the store disallows the link or the conclusion remains uncertain.
- [ ] Inspect the actual packaged About page and record the selected property next to the package checksum.

Use `docs/releases/STORE_BUILD_POLICY.md` as the source-controlled policy/runbook.

### 5. Prepare production signing and package identity

Source-side package metadata regression coverage is now present, but actual production identities/signing remain external work.

- [x] Add automated source contracts for application title/identifier/display-version/build-number shape and target/minimum OS declarations.
- [x] Add source contracts for Android privacy/permission posture, Apple purpose strings/transport posture, Windows package identity, and required app/splash/support assets.
- [ ] Create Android signing key/keystore outside the repository and store secrets securely.
- [ ] Configure Android release signing in CI/release tooling without committing credentials.
- [ ] Configure Apple signing certificates, provisioning profiles, App Store Connect bundle identity, and entitlements outside the repository.
- [ ] Configure Windows signing/package identity if publishing through Microsoft Store or signed sideloading.
- [ ] Verify application identifiers, version numbers, display names, icons, splash assets, capabilities, and package metadata on the **actual packaged artifact**, not source alone.
- [ ] Document certificate/key backup and rotation procedures.

### 6. Finish store listing and privacy disclosures

- [ ] Produce final screenshots for phone/tablet/desktop targets using fictional data only.
- [ ] Produce required store icon, feature graphic, promotional graphic, and platform-specific screenshots.
- [ ] Write concise and long descriptions that match actual functionality.
- [ ] Complete privacy/data-safety questionnaires from implemented local-first behavior rather than marketing assumptions.
- [ ] Publish links to `PRIVACY.md`, `TERMS.md`, `SECURITY.md`, and support information.
- [ ] Re-check health-category policy wording for each target store.
- [ ] Ensure screenshots/listings match the actual support-link visibility selected for each store package.

## Priority 1 — release promotion

### 7. Final exact-head verification, production commit/tag verification and evidence

PR #56 is the last completed exact automated RC1 release-engineering baseline for its frozen source, but the 2026-08-15 project/test/release-script changes require a new exact-head marker verification before production tagging.

- [x] Exact-head marker-only verification protocol is documented in `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.
- [x] PR #56 completed the full formatting/test/platform/CodeQL/unsuppressed-audit matrix for its frozen source.
- [x] PR #56 was closed without merging its marker after evidence capture.
- [x] Exact `v*` tags are configured to trigger CareNest CI, CodeQL, Dependency Audit, Release Gate and Release Evidence.
- [x] Release Evidence preserves available failed-run evidence and uses source/run/attempt provenance.
- [x] Record that verification-relevant source changed after PR #56 instead of misrepresenting PR #56 as exact-current-head evidence.
- [ ] Let the 2026-08-15 continuation stabilize with no further verification-relevant changes.
- [ ] Create a new marker-only exact-head verification from the final continuation head.
- [ ] Require formatting, all core test suites, Android/Windows/iOS simulator/Mac Catalyst Release builds, CodeQL and unsuppressed Dependency Audit on that exact source boundary.
- [ ] Record the new frozen source SHA, marker SHA, workflow run IDs and exact test totals in a new dated verification evidence document.
- [ ] Close the marker PR without merging after successful evidence capture.
- [ ] Complete Priority 0 blockers and resolve any source/configuration changes they require.
- [ ] If Priority 0 work changes source/test/workflow/package/project/platform/build-script files after the new verification, repeat exact-head verification.
- [ ] Select the exact intended production-release commit.
- [ ] Create the approved `v*` tag pointing to that exact commit.
- [ ] Require tagged CareNest CI success.
- [ ] Require tagged CodeQL success.
- [ ] Require tagged unsuppressed Dependency Audit success.
- [ ] Require tagged Release Gate success.
- [ ] Require tagged CareNest Release Evidence success.
- [ ] Capture final workflow run IDs, artifact identity/checksums, signing/package provenance, and manual evidence in `what_changed.md`, `PROJECT_STATUS.md`, release checklist, security review, and release notes.

### 8. Promote version metadata

- [ ] Decide final first public version (`1.0.0` or another pre-release).
- [ ] Update app version/build values consistently for Android, Apple, and Windows targets.
- [ ] Update `CHANGELOG.md` and release notes.
- [ ] Update `PROJECT_STATUS.md` from release-candidate status to actual shipped status.
- [ ] Ensure final production tag points to the exact approved commit and is not reused/moved after a failed tag gate.

### 9. Build and archive release artifacts

- [ ] Android: generate intended signed AAB/APK artifact.
- [ ] iOS: archive signed app for App Store/TestFlight distribution.
- [ ] Mac Catalyst: create intended signed/notarized package or store-ready archive.
- [ ] Windows: create intended signed MSIX/package.
- [ ] Generate SHA-256 checksums for directly distributed artifacts.
- [ ] Record `CareNestShowFundingLink` beside each store package checksum.
- [ ] Keep release artifacts/provenance separate from source-control secrets.
- [ ] Confirm signed package provenance points to the exact approved/tagged commit.

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
- [x] SQLite dependency-security pin/suppression contract;
- [x] exact release workflow trigger/event-safety contracts;
- [x] Release Evidence provenance/failure-preservation/rerun-identity contracts;
- [x] blocking release-preflight dependency-audit contracts;
- [x] deterministic/fail-closed local quality-gate contracts;
- [x] repository-local Git identity setup contracts;
- [x] fail-closed production Release Gate contracts;
- [x] build-configurable voluntary project-support surface contract;
- [x] package identity/version/minimum-target regression contracts;
- [x] Android local-first permission/backup/cleartext package contracts;
- [x] Apple purpose-string/transport posture package contracts;
- [x] Windows package identity/minimum-platform contracts;
- [x] required CareNest branding asset contracts.

Still useful later when stable target infrastructure exists:

- [ ] Add platform UI automation on real/emulated targets.
- [ ] Add deeper notification permission denial/retry state-transition automation where platform APIs can be reliably driven.
- [ ] Add canonical historical backup/document compatibility fixtures across future formats.
- [ ] Add file-corruption and low-storage target failure-path tests.
- [ ] Expand semantic/accessibility XAML contract coverage where meaningful without replacing manual assistive-technology testing.

### 12. Improve release engineering

Completed:

- [x] Dependency Audit workflow for pull requests, manual execution and exact `v*` tags.
- [x] Release Gate workflow blocks unresolved tracked dependency risk and incomplete release checklist.
- [x] Release Gate matching is hardened against indentation/case/nested-checkbox bypasses.
- [x] Release Evidence workflow records exact source/ref/run/attempt/toolchain/test/dependency/workspace/checksum evidence.
- [x] Release Evidence retains available failed-run evidence before aggregate failure and names artifacts with commit/run/attempt identity.
- [x] Exact-head marker-only verification protocol documented and proven through multiple verification cycles.
- [x] Platform-neutral formatting enforced in CI.
- [x] CodeQL and multi-platform build matrix remain required automated gates.
- [x] CareNest CI, CodeQL, Dependency Audit, Release Gate and Release Evidence are configured for exact `v*` release tags.
- [x] Bash/PowerShell local quality and release-preflight scripts run blocking unsuppressed dependency audits.
- [x] Repository-local Git setup scripts fail closed and verify `Sanskar` / `sanskarin@outlook.in`.
- [x] Narrow SQLite audit exception removed after a compatible dependency graph was established.
- [x] Dependency regression contract prevents silent restoration of the old SQLite pin/suppression baseline.
- [x] PR #56 completed the entire formatting/test/platform/CodeQL/unsuppressed-audit matrix for its frozen source.
- [x] Store-sensitive voluntary funding-link visibility can be selected through a reproducible build property.
- [x] Release-preflight scripts propagate and validate that store policy.
- [x] Packaged release validation and store-build policy are documented as explicit release procedures.

Remaining optional/production improvements:

- [ ] Complete the new exact-head marker verification after the 2026-08-15 source stabilizes.
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

The default open-source build may show the support surface. Store builds must follow `docs/releases/STORE_BUILD_POLICY.md` and can set `CareNestShowFundingLink=false` without changing health-organizer behavior.

## Definition of done for public `1.0.0`

CareNest should be promoted from release candidate only when all applicable items are true:

- [x] no known production-blocking SQLite advisory is being hidden by the former `GHSA-2m69-gcr7-jv3q` exception; the verified source graph removes that suppression and guards the maintained native/provider floor;
- [x] complete automated formatting/test/build/CodeQL/Dependency Audit matrix is green for the PR #56 frozen release-engineering source boundary;
- [ ] new exact-head automated matrix is green for the final 2026-08-15-or-later source boundary after all verification-relevant changes stabilize;
- [ ] manual supported-platform smoke tests are complete;
- [ ] existing-database/SQLite compatibility checks are complete on packaged representative builds;
- [ ] notification/appointment limitations and cancellation-first reminder actions are manually verified/documented;
- [ ] new v2 document/backup workflows are tested in packaged builds;
- [ ] retained v1 encrypted-data compatibility is verified with canonical historical fixtures when available;
- [ ] backup/restore is tested on clean installations;
- [ ] accessibility checks are complete;
- [ ] current Apple/Google store policy review is complete;
- [ ] actual packaged support-link visibility matches current store policy and is recorded with package evidence;
- [ ] signing keys/certificates are secured outside Git;
- [ ] privacy/data-safety disclosures match actual behavior;
- [ ] release notes/changelog/status/handoff documents are updated;
- [ ] signed release artifacts are archived with exact version/provenance information;
- [ ] exact approved production `v*` tag completes CareNest CI, CodeQL, Dependency Audit, Release Gate and Release Evidence successfully;
- [ ] final Release Evidence artifact/checksums and production package provenance are recorded.

Current RC1 implementation and source-side release hardening are complete for the documented feature set. The remaining exact-head verification, manual/device/accessibility/store/signing/distribution and packaged-data compatibility conditions intentionally keep final `1.0.0` publication blocked until actual evidence exists.
