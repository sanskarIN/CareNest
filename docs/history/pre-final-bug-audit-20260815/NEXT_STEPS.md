# CareNest Next Steps

This document tracks work after the source-complete `1.0.0-rc.1` milestone. It intentionally separates release blockers from completed hardening and optional future versions so unfinished future ideas are not confused with missing RC1 implementation.

## Current source and exact automated baseline

The latest verification-relevant executable/project/test/workflow/build-script/artifact-generation source was frozen at:

`4c60f90ac33a321d12a6f9b3a8c097e4e4a4e5f2`

PR #61 is the current exact automated/source-inspection baseline:

`Verify corrected CareNest store inspection artifacts`

Frozen source/base SHA:

`4c60f90ac33a321d12a6f9b3a8c097e4e4a4e5f2`

Verification marker/head:

`19c82b813c375047cf1166487bc18a1bd2cd0e52`

GitHub PR merge/event SHA during verification:

`c8ea9fef89d7b773f19bf13c64f349495be706ad`

Final evidence:

- CareNest CI #650 / run `31872610834`: **success**;
- platform-neutral formatting: **success**;
- **122 unit tests**: passed;
- **39 integration tests**: passed;
- **157 UI-contract/policy tests**: passed;
- **318 total automated tests**: passed;
- default Android Release: **success**;
- default Windows Release: **success**;
- default iOS simulator Release: **success**;
- default Mac Catalyst Release: **success**;
- CareNest Store Package Configuration #39 / run `31872610789`: **success**;
- funding-disabled Android Release: **success**;
- funding-disabled Windows Release: **success**;
- funding-disabled iOS simulator Release: **success**;
- funding-disabled Mac Catalyst Release: **success**;
- Bash store-package preflight executable-mode guard: **success**;
- CareNest Store Inspection Artifacts #2 / run `31872610786`: **success**;
- verified-unsigned Android AAB inspection artifact: **success**;
- self-contained unpackaged Windows `win-x64` inspection artifact: **success**;
- iOS simulator and unsigned Mac Catalyst inspection artifacts: **success**;
- source-head/event-SHA provenance separation and payload checksums: **verified**;
- CodeQL #650 / run `31872610815`: **success**;
- unsuppressed Dependency Audit #46 / run `31872610791`: **success**.

PR #61 was marker-only and closed without merge. Its verification marker `build/verification/store-inspection-artifacts-final-v2-20260815.txt` is not part of `main`.

`docs/releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md` records the exact runs, artifact IDs, API digests, payload checksums and downloaded-artifact inspection evidence.

PR #60 remains a deliberately superseded failure-driven checkpoint: it exposed that the first Android artifact upload included MAUI's debug-signed `-Signed.aab` companion and that pull-request provenance was conflating GitHub's merge/event SHA with the branch head. Those defects were corrected before PR #61. PR #59 remains historical exact store-safe compilation evidence, PR #58 remains historical exact packaged-release/store-policy hardening evidence, PR #56 remains historical exact release-engineering evidence, and PR #54 remains the historical authoritative runtime bug-audit baseline.

The current source includes final reminder/document/backup/SQLite hardening plus release-engineering controls for:

- exact `v*` execution of CareNest CI, CodeQL, Dependency Audit, CareNest Store Package Configuration, CareNest Store Inspection Artifacts, Release Gate and Release Evidence;
- manual workflow entry points where configured;
- PR-only dependency metadata guarded from tag/manual runs;
- failure-preserving Release Evidence with tracked-source provenance/checksums and rerun-safe artifact identity;
- blocking unsuppressed dependency audit in local quality/preflight scripts;
- repository-local Git setup that verifies `Sanskar` / `sanskarin@outlook.in` and fails on Git errors;
- fail-closed production Release Gate matching;
- executable workflow/script/release-policy contracts;
- build-configurable voluntary project-support visibility;
- a store-safe funding command that cannot execute when the compile-time funding surface is disabled;
- package metadata/privacy contracts;
- Windows portable publish RID mapping through `RuntimeIdentifierOverride`;
- fail-closed store-package wrappers that force `CARENEST_SHOW_FUNDING_LINK=false`;
- a dedicated four-platform store-safe workflow compiling with `CareNestShowFundingLink=false`;
- executable-mode verification for the Bash store-package wrapper;
- store-package workflow/preflight source-policy contracts;
- a dedicated store-inspection workflow producing checksum/provenance-bearing internal artifacts without production signing secrets;
- unsigned-only Android artifact staging that excludes and refuses debug-signed/signature-bearing AAB candidates;
- exact source-head checkout/artifact naming with PR merge/event SHA retained separately in provenance;
- current architecture/security/setup/release docs aligned with the remediated SQLite graph and cancellation-first reminder model.

Completed 2026-08-15 source-side release-readiness items include:

- [x] Add `CareNestShowFundingLink`, default `true`, so the external voluntary support surface can be disabled for a specific store build without a source fork.
- [x] Hide the complete About support card when `CareNestShowFundingLink=false` while leaving organizer/legal/support surfaces unchanged.
- [x] Make the hidden funding command non-executable in the store-safe compile configuration.
- [x] Add source-policy regression coverage for the funding-link build switch, disabled command and voluntary/no-health-entitlement wording.
- [x] Add package metadata/privacy contracts for application identity/version, target/minimum OS declarations, Android permissions/backup/cleartext/local-first network posture, Apple purpose strings/transport posture, Windows package metadata and branding assets.
- [x] Add and regression-protect the Windows `RuntimeIdentifierOverride` mapping used by portable internal inspection publishing.
- [x] Wire `CARENEST_SHOW_FUNDING_LINK=true|false` through Bash and PowerShell release-preflight scripts with fail-closed validation.
- [x] Add fail-closed Bash and PowerShell `store-package-preflight` wrappers that require an explicit supported target and force the support surface off.
- [x] Track the Bash store-package wrapper as executable (`100755`).
- [x] Add CI verification that the Bash store-package wrapper remains executable.
- [x] Add `.github/workflows/store-package-verification.yml`.
- [x] Compile Android, Windows, iOS simulator and Mac Catalyst Release builds with `CareNestShowFundingLink=false` on PR #61.
- [x] Require the store-safe workflow on `v*` and manual verification paths through source-policy contracts.
- [x] Add `StorePackageWorkflowContractTests`.
- [x] Add `StorePackagePreflightContractTests`.
- [x] Add `.github/workflows/store-inspection-artifacts.yml` for reproducible internal inspection artifacts.
- [x] Add `StoreInspectionArtifactWorkflowContractTests` and require the artifact workflow on exact `v*`/manual release paths.
- [x] Runtime-exercise the initial artifact workflow on PR #60 and reject it as final evidence after downloaded-artifact inspection exposed the debug-signed Android companion and ambiguous PR provenance.
- [x] Exclude `*-Signed.aab`, require exactly one Android AAB candidate and reject AAB JAR-signature metadata before upload.
- [x] Record `signing=verified-unsigned` and `debug_signed_companion_staged=false` in Android provenance.
- [x] Separate exact source SHA/ref from GitHub event/PR merge SHA/ref and checkout/name artifacts from the exact source head.
- [x] Complete PR #61 exact-head verification with 318/318 tests, both four-platform build configurations, CodeQL and unsuppressed Dependency Audit.
- [x] Produce and independently inspect PR #61 Android, Windows and Apple internal artifacts and verify payload SHA-256 values/provenance.
- [x] Add `docs/releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md`.
- [x] Add `docs/releases/STORE_BUILD_POLICY.md`.
- [x] Add/update `docs/releases/PACKAGED_RELEASE_VALIDATION.md` with the automated inspection-artifact boundary.
- [x] Complete the dated 2026-08-15 Apple/Google external support-link policy review.
- [x] Add `docs/releases/STORE_POLICY_REVIEW_20260815.md`.
- [x] Preserve PR #59's historical store-safe verification in `docs/releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md`.

Verification history remains intentionally failure-driven. PR #43 was not green; PRs #44, #46 and #49 exposed additional integration/UI/analyzer defects; PRs #47/#48/#50 exercised SQLite remediation while source was moving; PRs #51/#52 were superseded; PR #54 completed the runtime bug-audit baseline; PR #55 verified an intermediate release-engineering snapshot; PR #56 completed the 2026-08-14 release-engineering baseline; PR #58 completed the first packaged-release/store-policy source baseline; PR #59 completed the default-plus-store-safe compilation baseline; PR #60 exposed artifact-signing/provenance defects and was superseded; PR #61 completed the corrected exact source plus internal artifact baseline.

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
- [x] Complete unsuppressed Dependency Audit #46 / run `31872610791` on PR #61's frozen source.
- [x] Complete all 318 automated tests, all four default Release builds, and all four store-safe Release builds on that same verified source boundary.
- [x] Produce non-production internal inspection artifacts for Android/Windows/Apple targets with checksums/provenance; these do not replace packaged existing-data compatibility.
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

**Current state:** dependency remediation and unsuppressed automated verification are complete for PR #61's frozen source. Do not re-open the old exception merely because manual compatibility evidence is still outstanding; track those checks as release-validation work.

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

### 4. Store policy and voluntary project-support packaging

CareNest's default open-source build can expose `https://buymeacoffee.com/sanskarIN` as optional project support and GitHub funding metadata also references it.

Source-side mitigation and the current dated policy review are complete:

- [x] Add a store-specific build switch instead of requiring a source fork.
- [x] Default `CareNestShowFundingLink` to `true` for normal/open-source builds.
- [x] Make `CareNestShowFundingLink=false` hide the complete About support card and disable the funding command.
- [x] Add regression coverage for that behavior.
- [x] Pass `CARENEST_SHOW_FUNDING_LINK` through both release-preflight scripts.
- [x] Add dedicated fail-closed store-package preflight wrappers.
- [x] Add dedicated store-safe four-platform CI.
- [x] Verify current Apple App Store guidance for external gifts/support on 2026-08-15.
- [x] Verify current Google Play payments guidance for tips/contributions on 2026-08-15.
- [x] Record policy sources/date/conclusion in `docs/releases/STORE_POLICY_REVIEW_20260815.md`.
- [x] Record the conservative initial Apple/Google package decision: `CareNestShowFundingLink=false` unless submission-time policy clearly permits the external link.
- [x] Compile all four supported targets with `CareNestShowFundingLink=false` on PR #61.
- [x] Generate funding-disabled internal inspection artifacts on PR #61 and verify their source identity/checksums/provenance.
- [x] Verify the Android internal artifact contains exactly one unsigned AAB and no staged debug-signed companion.
- [x] Document exact enabled/disabled build commands and evidence requirements in `docs/releases/STORE_BUILD_POLICY.md` and `docs/releases/PACKAGED_RELEASE_VALIDATION.md`.

Still required before each actual store submission because policies/programs and package behavior can change:

- [ ] Re-check the current Apple App Store rules at actual submission time.
- [ ] Re-check the current Google Play rules at actual submission time.
- [ ] Record submission-time policy source/date/reviewer/conclusion.
- [ ] Build the actual **signed** Apple App Store candidate with `CareNestShowFundingLink=false` under the current conservative decision.
- [ ] Build the actual **signed** Google Play candidate with `CareNestShowFundingLink=false` under the current conservative decision.
- [ ] Inspect the installed packaged About page and confirm the BMC image/button/URL/card is absent and the hidden command cannot be reached through the actual package UI.
- [ ] Verify repository, creator, business/support email, privacy, terms, security and notices remain available in the store-safe package.
- [ ] Confirm no medical feature, health functionality, reminder behavior, support priority or premium entitlement changes between normal and store-safe packages.
- [ ] Record the selected property beside package checksum/source/signing provenance.

Use `docs/releases/STORE_BUILD_POLICY.md` and `docs/releases/PACKAGED_RELEASE_VALIDATION.md`.

### 5. Prepare production signing and package identity

Source-side package metadata regression coverage and internal artifact-shape evidence are present, but actual production identities/signing remain external work.

- [x] Add automated source contracts for application title/identifier/display-version/build-number shape and target/minimum OS declarations.
- [x] Add source contracts for Android privacy/permission posture, Apple purpose strings/transport posture, Windows package identity, and required app/splash/support assets.
- [x] Compile both default and funding-disabled source configurations for all supported targets under PR #61.
- [x] Generate unsigned/internal Android, Windows and Apple inspection artifacts with exact source/checksum provenance under PR #61.
- [ ] Create Android signing key/keystore outside the repository and store secrets securely.
- [ ] Configure Android release signing in CI/release tooling without committing credentials.
- [ ] Configure Apple signing certificates, provisioning profiles, App Store Connect bundle identity, and entitlements outside the repository.
- [ ] Configure Windows signing/package identity if publishing through Microsoft Store or signed sideloading.
- [ ] Verify application identifiers, version numbers, display names, icons, splash assets, capabilities, funding-link visibility and package metadata on the **actual signed packaged artifact**, not source/internal inspection artifacts alone.
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

### 7. Exact-head source verification, production commit/tag verification and evidence

Current source-side exact verification is complete:

- [x] Exact-head marker-only verification protocol is documented in `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.
- [x] PR #56 completed its historical release-engineering source matrix.
- [x] PR #58 completed the first 2026-08-15 package/store-policy source matrix.
- [x] PR #59 completed its historical default-plus-store-safe source matrix.
- [x] PR #60 exercised the first store-inspection workflow, exposed debug-signed Android companion/provenance defects, and was explicitly superseded/closed without merge.
- [x] PR #61 completed the corrected current exact source and internal artifact matrix.
- [x] PR #61 passed formatting, 318/318 core tests, all four default Release builds, CodeQL and unsuppressed Dependency Audit.
- [x] PR #61 passed all four funding-disabled store-safe Release builds.
- [x] PR #61 passed Android/Windows/Apple Store Inspection Artifact jobs and downloaded payload checksums/provenance were independently verified.
- [x] PR #61 verification marker was closed without merge after evidence capture.
- [x] `docs/releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md` records the frozen source SHA, marker SHA, PR event SHA, workflow IDs, exact test totals, artifact IDs/digests and payload checksums.
- [x] Exact `v*` tags are configured to trigger CareNest CI, CodeQL, Dependency Audit, CareNest Store Package Configuration, CareNest Store Inspection Artifacts, Release Gate and Release Evidence.
- [x] Release Evidence preserves available failed-run evidence and uses source/run/attempt provenance.

Still required for final production promotion:

- [ ] Complete Priority 0 packaged/device/accessibility/signing/store-metadata blockers and resolve any source/configuration changes they require.
- [ ] If any runtime/test/workflow/package/project/platform/build-script/artifact-generation source changes after PR #61's frozen source, repeat marker-only exact-head verification.
- [ ] Select the exact intended production-release commit.
- [ ] Create the approved non-movable `v*` tag pointing to that exact commit.
- [ ] Require tagged CareNest CI success.
- [ ] Require tagged CodeQL success.
- [ ] Require tagged unsuppressed Dependency Audit success.
- [ ] Require tagged CareNest Store Package Configuration success.
- [ ] Require tagged CareNest Store Inspection Artifacts success and inspect the exact tagged internal artifacts/checksums/provenance.
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

Internal inspection artifacts are now reproducible and checksum/provenance-bearing, but they are deliberately not production artifacts.

- [x] Android: generate and inspect a verified-unsigned internal AAB with `CareNestShowFundingLink=false` and exact source/checksum provenance.
- [x] Windows: generate and inspect a self-contained unpackaged `win-x64` internal bundle with `CareNestShowFundingLink=false` and exact source/checksum provenance.
- [x] iOS simulator: generate and inspect an internal simulator `.app` archive with `CareNestShowFundingLink=false` and exact source/checksum provenance.
- [x] Mac Catalyst: generate and inspect an unsigned internal `.app` archive with `CareNestShowFundingLink=false` and exact source/checksum provenance.
- [ ] Android: generate intended **signed** AAB/APK production artifact with the selected store funding-link configuration.
- [ ] iOS: archive signed app for App Store/TestFlight distribution with the selected store funding-link configuration.
- [ ] Mac Catalyst: create intended signed/notarized package or store-ready archive.
- [ ] Windows: create intended signed MSIX/package.
- [ ] Generate/retain SHA-256 checksums for directly distributed production artifacts.
- [ ] Record `CareNestShowFundingLink` beside each store production package checksum.
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
- [x] non-executable funding command contract for store-safe compilation;
- [x] package identity/version/minimum-target regression contracts;
- [x] Windows portable publish RID mapping contract;
- [x] Android local-first permission/backup/cleartext package contracts;
- [x] Apple purpose-string/transport posture package contracts;
- [x] Windows package identity/minimum-platform contracts;
- [x] required CareNest branding asset contracts;
- [x] store-package workflow trigger/funding-disabled/target/non-publication contracts;
- [x] store-package preflight forced-false/target-allow-list/delegation contracts;
- [x] Bash store-package executable-mode CI contract;
- [x] store-inspection workflow trigger/funding-disabled/internal-only/checksum/provenance/signing-secret contracts;
- [x] Android inspection-artifact unsigned-only/debug-signed-companion rejection contract;
- [x] inspection-artifact exact source-head/event-SHA provenance contract;
- [x] release workflow contract requiring both store-safe compilation and inspection-artifact workflows on exact `v*`/manual verification paths.

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
- [x] CodeQL and default multi-platform build matrix remain required automated gates.
- [x] CareNest CI, CodeQL, Dependency Audit, CareNest Store Package Configuration, CareNest Store Inspection Artifacts, Release Gate and Release Evidence are configured for exact `v*` release tags.
- [x] Bash/PowerShell local quality and release-preflight scripts run blocking unsuppressed dependency audits.
- [x] Repository-local Git setup scripts fail closed and verify `Sanskar` / `sanskarin@outlook.in`.
- [x] Narrow SQLite audit exception removed after a compatible dependency graph was established.
- [x] Dependency regression contract prevents silent restoration of the old SQLite pin/suppression baseline.
- [x] Store-sensitive voluntary funding-link visibility can be selected through a reproducible build property.
- [x] Release-preflight scripts propagate and validate that store policy.
- [x] Fail-closed store-package wrappers force the funding link off for an explicit supported target.
- [x] The Bash store-package wrapper is executable and CI verifies its executable bit.
- [x] Dedicated Store Package Configuration workflow compiles all four supported targets with the external support surface disabled.
- [x] Store Package Configuration does not upload unsigned artifacts or configure production signing.
- [x] Store-package workflow/preflight behavior is regression-protected by source-policy tests.
- [x] Dedicated Store Inspection Artifacts workflow produces checksum/provenance-bearing internal artifacts without production signing secrets.
- [x] Android inspection staging rejects the debug-signed MAUI companion and verifies the selected AAB has no JAR-signature metadata.
- [x] Pull-request inspection artifacts distinguish the exact source head from the GitHub merge/event SHA.
- [x] Packaged release validation and store-build policy are documented as explicit release procedures.
- [x] PR #61 completed the full default-plus-store-safe formatting/test/platform/CodeQL/unsuppressed-audit and corrected internal-artifact matrix.

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

The default open-source build may show the support surface. Store builds must follow `docs/releases/STORE_BUILD_POLICY.md`; the current conservative Apple/Google decision uses `CareNestShowFundingLink=false` for initial store candidates unless submission-time policy clearly permits the external link.

## Definition of done for public `1.0.0`

CareNest should be promoted from release candidate only when all applicable items are true:

- [x] no known production-blocking SQLite advisory is being hidden by the former `GHSA-2m69-gcr7-jv3q` exception; the verified source graph removes that suppression and guards the maintained native/provider floor;
- [x] exact automated formatting/test/default-build/CodeQL/Dependency Audit matrix is green for PR #61's frozen source boundary;
- [x] funding-disabled Android/Windows/iOS simulator/Mac Catalyst Release source builds are green for PR #61's frozen source boundary;
- [x] checksum/provenance-bearing internal Android/Windows/Apple store-safe inspection artifacts are green and independently inspected for PR #61's frozen source boundary;
- [x] dated 2026-08-15 Apple/Google external support-link policy review is complete and records the conservative store-safe decision;
- [ ] submission-time Apple/Google policy re-review is complete;
- [ ] actual signed/installed store candidates reflect the selected `CareNestShowFundingLink` value and packaged About-page inspection is recorded;
- [ ] manual supported-platform smoke tests are complete;
- [ ] existing-database/SQLite compatibility checks are complete on packaged representative builds;
- [ ] notification/appointment limitations and cancellation-first reminder actions are manually verified/documented;
- [ ] new v2 document/backup workflows are tested in packaged builds;
- [ ] retained v1 encrypted-data compatibility is verified with canonical historical fixtures when available;
- [ ] backup/restore is tested on clean installations;
- [ ] accessibility checks are complete;
- [ ] signing keys/certificates are secured outside Git;
- [ ] privacy/data-safety disclosures match actual behavior;
- [ ] release notes/changelog/status/handoff documents are updated for the final production boundary;
- [ ] signed release artifacts are archived with exact version/provenance information;
- [ ] exact approved production `v*` tag completes CareNest CI, CodeQL, unsuppressed Dependency Audit, CareNest Store Package Configuration, CareNest Store Inspection Artifacts, Release Gate and Release Evidence successfully;
- [ ] final Release Evidence artifact/checksums and production package provenance are recorded.

Current RC1 implementation and source-side release hardening are complete for the documented feature set. The remaining manual/device/accessibility/store/signing/distribution and packaged-data compatibility conditions intentionally keep final `1.0.0` publication blocked until actual evidence exists.