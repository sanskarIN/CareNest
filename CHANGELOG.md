# Changelog

All notable CareNest changes are recorded here using Keep a Changelog-style categories and semantic-versioning intent.

The exact pre-documentation-completion changelog is preserved at:

`docs/history/pre-complete-docs-20260814/CHANGELOG.md`

Historical evidence is retained rather than rewritten. This active changelog records the current authoritative state and the latest continuation.

## [Unreleased] - 2026-08-15

### Added — corrected store inspection artifact automation

- Added `.github/workflows/store-inspection-artifacts.yml` for reproducible non-production Android, Windows, iOS-simulator and Mac Catalyst inspection artifacts with `CareNestShowFundingLink=false`.
- Added exact source-head checkout/artifact naming and separate PR merge/event SHA provenance so a temporary GitHub pull-request merge ref is not confused with the inspected source.
- Added SHA-256/provenance files to every inspection artifact and explicit `artifact_purpose=internal-inspection-only` / `store_submission_ready=false` markers.
- Added a verified-unsigned Android AAB path that stages exactly one non-`-Signed.aab` candidate and rejects JAR-signature metadata before upload.
- Added a self-contained unpackaged Windows `win-x64` inspection publish using the Windows-only `RuntimeIdentifierOverride` mapping.
- Added iOS simulator and unsigned Mac Catalyst inspection bundles without production provisioning/signing credentials.
- Added `StoreInspectionArtifactWorkflowContractTests` covering triggers, funding-disabled configuration, unsigned/internal status, secret absence, checksums, exact source/event provenance and Android debug-signed-companion rejection.
- Added release-workflow contract coverage requiring Store Inspection Artifacts on exact `v*` and manual release-verification paths.
- Added `docs/releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md` with PR #60 failure evidence and authoritative PR #61 runs/artifact IDs/API digests/payload checksums/download inspection.

### Changed — store-safe About command and Windows publish isolation

- Store-safe builds now make the hidden funding command non-executable instead of merely hiding the support card.
- `PackageMetadataContractTests` now protect the Windows-only `RuntimeIdentifierOverride` publish mapping while preserving minimum-OS package metadata assertions.
- `CareNest.App.csproj` maps `RuntimeIdentifierOverride` to `RuntimeIdentifier` only for the Windows target when explicitly supplied.
- Exact production tags matching `v*` now cover **seven** release workflows: CareNest CI, CodeQL, Dependency Audit, CareNest Store Package Configuration, CareNest Store Inspection Artifacts, Release Gate and CareNest Release Evidence.

### Fixed — first inspection artifact workflow defects exposed by PR #60

PR #60 was deliberately rejected as final evidence even though its Android and Windows artifact jobs completed.

Downloaded Android artifact inspection exposed:

- an unsigned AAB and a MAUI-generated debug-signed `-Signed.aab` companion staged together while provenance said signing was disabled;
- the signed companion carried the standard Android Debug certificate identity (`CN=Android Debug, O=Android, C=US`);
- PR artifact naming/provenance used GitHub's temporary PR merge/event SHA rather than the marker branch head.

Corrective source now:

- excludes `*-Signed.aab` from staging;
- requires exactly one unsigned Android AAB candidate;
- rejects AAB JAR-signature metadata;
- records `signing=verified-unsigned` and `debug_signed_companion_staged=false`;
- checks out and names artifacts from the exact PR/source head;
- records GitHub event/merge SHA/ref separately for auditability.

PR #60 was closed without merge and remains historical failure-driven evidence only.

### Verification — PR #61 authoritative current automated/source-inspection baseline

PR #61: `Verify corrected CareNest store inspection artifacts`.

Frozen source/base:

`4c60f90ac33a321d12a6f9b3a8c097e4e4a4e5f2`

Marker head:

`19c82b813c375047cf1166487bc18a1bd2cd0e52`

PR merge/event SHA:

`c8ea9fef89d7b773f19bf13c64f349495be706ad`

Evidence:

- CareNest CI #650 / `31872610834`: **success**;
- formatting: **success**;
- UnitTests: **122 passed, 0 failed, 0 skipped**;
- IntegrationTests: **39 passed, 0 failed, 0 skipped**;
- UiTests/source-policy: **157 passed, 0 failed, 0 skipped**;
- total core tests: **318 passed, 0 failed, 0 skipped**;
- default Android Release: **success**;
- default Windows Release: **success**;
- default iOS simulator Release: **success**;
- default Mac Catalyst Release: **success**;
- CareNest Store Package Configuration #39 / `31872610789`: **success**;
- store-safe Android/Windows/iOS simulator/Mac Catalyst Release builds with `CareNestShowFundingLink=false`: **success**;
- Bash store-package preflight executable-mode guard: **success**;
- CareNest Store Inspection Artifacts #2 / `31872610786`: **success**;
- CodeQL #650 / `31872610815`: **success**;
- unsuppressed Dependency Audit #46 / `31872610791`: **success**.

Downloaded artifact evidence:

- Android artifact ID `9243915053`, API digest `sha256:ac0039136e3608319df2927fbb38acf383445b022596ce4f86633b39f882c164`, AAB SHA-256 `fea87ddc9e790d4c88f4de382f70a121c57f308e9f476bc52b57f3bd091ce080`;
- Windows artifact ID `9243904498`, API digest `sha256:c0c7dd46ad8ec38e2295da0e1e0c8c69ece690f024c248b82ee09a0721a999f6`, nested ZIP SHA-256 `08b4de53dcebc7d88031f4ae3f243e6579e8ad556bcf1e299c6294399b978ac0`;
- Apple artifact ID `9244085155`, API digest `sha256:e82e6fe2022a7a5cf6ead34744876561c4c93e550e5d34fe192098455ea6ebd2`, iOS simulator tar SHA-256 `6ad6077fff0ac0f9b5bd5d8a03b73c0e2abf7fb6c825e7db2408204c58f02d65`, Mac Catalyst tar SHA-256 `fb98371db1c54cfac766d126f3eebace53269ee3c150b49c077f1637115d67d8`.

Independent downloaded-artifact inspection confirmed the corrected Android artifact contains exactly one unsigned AAB with no staged debug-signed companion/signature metadata; Windows is unpackaged/internal-only; Apple has no embedded production provisioning and Mac Catalyst has no `_CodeSignature`; all provenance separates source head from PR event/merge SHA.

PR #61 was closed without merge. Its marker is not part of `main`.

Internal inspection artifacts remain non-production evidence. They do not complete actual production signing, installed-package inspection, packaged SQLite/encrypted-data compatibility, accessibility, device notification behavior, submission-time store-policy review or store approval.

### Added — store/package release-readiness controls

- Added `CareNestShowFundingLink`, defaulting to `true`, so the voluntary Buy Me a Coffee surface can be hidden for a specific store package without maintaining a source fork.
- Added `AboutViewModel.IsProjectSupportVisible` and bound the complete About support card to it.
- Added a UI/source-policy contract protecting the configurable support-link surface and its voluntary/no-health-entitlement wording.
- Added `PackageMetadataContractTests` covering CareNest app identity/version shape, target frameworks/minimum OS declarations, Android local-first permission/backup/cleartext posture, Apple purpose strings/transport posture, Windows package metadata and required brand assets.
- Added `RepositoryLocator.PathOf` for reusable repository-path assertions in source-policy tests.
- Added `docs/releases/STORE_BUILD_POLICY.md` with enabled/disabled package commands, per-store policy-decision rules, automated/local store-safe paths, and release evidence fields.
- Added `docs/releases/PACKAGED_RELEASE_VALIDATION.md` with source freeze, package identity/checksum, packaged SQLite/encrypted-data, reminder, accessibility, store-policy, signing and final-tag evidence procedures.
- Added `docs/releases/STORE_POLICY_REVIEW_20260815.md` recording the current Apple/Google external support-link review and conservative initial store-package decision.
- Added `.github/workflows/store-package-verification.yml` to compile Android, Windows, iOS simulator and Mac Catalyst Release source with `CareNestShowFundingLink=false`.
- Added `build/scripts/store-package-preflight.sh` and `build/scripts/store-package-preflight.ps1` as fail-closed store-package wrappers that require an explicit supported target and force the external support surface off before delegating the standard release preflight.
- Added `StorePackageWorkflowContractTests` covering workflow entry points, funding-disabled propagation, supported targets, executable-mode verification, unsigned simulator behavior and non-publication boundaries.
- Added `StorePackagePreflightContractTests` covering forced-false behavior, explicit target allow-list, standard-preflight delegation and caller-override rejection.
- Added exact `v*`/manual workflow contract coverage requiring the store-package workflow.
- Added `docs/releases/PACKAGED_RELEASE_HARDENING_VERIFICATION_20260815.md` recording PR #58 evidence.
- Added `docs/releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md` recording the historical PR #59 default-plus-store-safe exact-source evidence.

### Changed — release preflight and store-safe source compilation

- Bash and PowerShell release-preflight scripts read `CARENEST_SHOW_FUNDING_LINK` with default `true`.
- Accepted general preflight values are fail-closed to `true`/`false`; invalid values stop preflight.
- Optional MAUI restore/build receives `CareNestShowFundingLink`, making a store-specific funding-link source configuration reproducible.
- Dedicated store-package wrappers force `CARENEST_SHOW_FUNDING_LINK=false`; callers cannot override the wrapper back to `true`.
- Store-package wrappers accept only the supported Android/iOS/Mac Catalyst/Windows target framework allow-list.
- The Bash store-package wrapper is tracked with executable Git mode `100755`.
- Store Package Configuration CI runs `test -x build/scripts/store-package-preflight.sh` so executable-bit loss fails automatically.
- Store Package Configuration CI runs on pull requests to `main`, pushes to `main`/`release/**`, exact `v*` tags, and manual execution.
- Store Package Configuration CI does not upload unsigned build outputs, does not run `dotnet publish`, and does not configure production signing credentials.

### Changed — exact release workflow coverage

Production tags matching `v*` are now expected to run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- CareNest Store Package Configuration;
- CareNest Store Inspection Artifacts;
- Release Gate;
- CareNest Release Evidence.

A successful Store Package Configuration run proves funding-disabled source compilation only. A successful Store Inspection Artifacts run proves reproducible internal package-shape/checksum/provenance evidence only. Neither proves signed artifact generation, installed package behavior, store approval, accessibility, device notification behavior, or packaged existing-data compatibility.

### Verification — PR #58 packaged-release hardening baseline

PR #58 froze source/base:

`826b79925dad4402f65fccfecd4a29b353b6e2f3`

Marker head:

`b92e3b79857db2f6cb8346fb881fe65b43f8453b`

Evidence:

- CareNest CI #608 / `31867245796`: success;
- formatting: success;
- UnitTests: 122 passed;
- IntegrationTests: 39 passed;
- UiTests/source-policy: 130 passed;
- total: 291/291 passed;
- default Android Release: success;
- default Windows Release: success;
- default iOS simulator Release: success;
- default Mac Catalyst Release: success;
- CodeQL #608 / `31867245799`: success;
- unsuppressed Dependency Audit #43 / `31867245800`: success.

PR #58 was closed without merge and remains historical exact evidence for its frozen source boundary.

### Verification — PR #59 historical store-safe source baseline

PR #59: `Verify store-safe CareNest package configuration`.

Frozen source/base:

`8489d19734d6142054156d5b57f2713195c16b65`

Marker head:

`ca58294fb7f7a56ee87da16d938f0f691c3a3c7e`

Evidence:

- CareNest CI #622 / `31869214132`: **success**;
- formatting: **success**;
- UnitTests: **122 passed, 0 failed, 0 skipped**;
- IntegrationTests: **39 passed, 0 failed, 0 skipped**;
- UiTests/source-policy: **149 passed, 0 failed, 0 skipped**;
- total core tests: **310 passed, 0 failed, 0 skipped**;
- default Android Release: **success**;
- default Windows Release: **success**;
- default iOS simulator Release: **success**;
- default Mac Catalyst Release: **success**;
- CareNest Store Package Configuration #11 / `31869214047`: **success**;
- store-safe Android Release with `CareNestShowFundingLink=false`: **success**;
- store-safe Windows Release with `CareNestShowFundingLink=false`: **success**;
- store-safe iOS simulator Release with `CareNestShowFundingLink=false`: **success**;
- store-safe Mac Catalyst Release with `CareNestShowFundingLink=false`: **success**;
- Bash store-package preflight executable-mode guard: **success**;
- CodeQL #622 / `31869214042`: **success**;
- unsuppressed Dependency Audit #44 / `31869214093`: **success**.

PR #59 was closed without merge. Its marker is not part of `main`.

PR #61 supersedes PR #59 only as the current exact automated/source-inspection baseline. PR #59, PR #58, PR #56, and PR #54 remain valid historical evidence for their own frozen source boundaries.

### Changed — active status/documentation alignment

- Root `README.md` now promotes PR #61 and documents default/store-safe source compilation plus internal inspection artifacts.
- `PROJECT_STATUS.md` now uses PR #61 as the authoritative current source boundary and preserves PR #60/#59/#58/#56/#54 as historical evidence.
- `docs/README.md` now indexes PR #61 verification, the dated store-policy review, store-safe workflow/preflight documentation, internal artifact evidence, and current blockers.
- `docs/CONFIGURATION_REFERENCE.md` now documents `CareNestShowFundingLink`, non-executable store-safe funding command, Windows inspection RID mapping, store-package wrappers, Store Package Configuration, Store Inspection Artifacts, exact `v*` coverage, and PR #61.
- `docs/releases/NEXT_STEPS.md` now checks only source/internal-artifact work actually completed and leaves signed package/device/accessibility/data/store-submission work open.
- `docs/releases/RELEASE_CHECKLIST.md` now distinguishes source compilation/internal artifact evidence from installed/signed package evidence and requires tagged Store Package Configuration plus Store Inspection Artifacts success for final production promotion.
- `docs/security/SECURITY_MODEL.md`, `docs/security/THREAT_MODEL.md`, `docs/setup/DEVELOPMENT.md`, `docs/architecture/ARCHITECTURE.md`, and `docs/DOCUMENTATION_STANDARDS.md` now use PR #61/current artifact boundaries.

### Current store-policy decision

The dated 2026-08-15 review records:

- normal/open-source/direct builds may retain `CareNestShowFundingLink=true` where the distribution channel permits it;
- initial Apple App Store candidate should use `CareNestShowFundingLink=false` unless submission-time policy clearly permits the external support link;
- initial Google Play candidate should use `CareNestShowFundingLink=false` unless submission-time policy clearly permits the external support link;
- actual submission-time policy must be re-reviewed;
- actual signed/installed package visibility must still be verified.

The current source provides the store-safe switch, non-executable hidden funding command, wrappers, tests, four-platform compilation, and internal inspection artifacts. It does not claim Apple/Google approval.

### Selected continuation commits — 2026-08-15

Earlier store/package hardening commits:

- `35690d2f1fbe8bb56d91e718dab688fe4de6cc0d` — `feat: make voluntary funding link store-configurable`;
- `7ccea4ff5367b3c4e94b156f989799d91d6f52ff` — `test: enforce package metadata and privacy contracts`;
- `1fe68a73aaa41622391d8ff6e53171ca98dce055` — `build: pass store funding policy into release preflight`;
- `0a9d994ea310f00d715684c993ee2d954dc0f081` — `docs: define store-specific funding-link build policy`;
- `fe17e1ad752250d81d502ef7615fc1e652842e47` — `docs: add packaged release validation runbook`;
- `db8536d9de125ae73f895ca1d1d6cbdb4de0ded0` — `docs: record packaged release hardening handoff`;
- `dadcc8f3a6c4098bf9277e647206f75f59e98261` — `docs: align project status with new source boundary`;
- `39c2cdce359ff18c44bc3d4743d7f8ca55ee1294` — `docs: record 2026-08-15 release readiness changes`;
- `c78e080ede0d997082a2bd68b2baf521f8ac8534` — `docs: align documentation hub with current source`;
- `826b79925dad4402f65fccfecd4a29b353b6e2f3` — `docs: advance release next steps after package hardening`;
- `7ad45c82e6cf2877d693fd8481591f9969082eba` — `docs: record PR58 packaged release verification`;
- `0488c68899eb8c6b5ef0de1753d3d3552fd97871` — `docs: record 2026-08-15 store support policy review`;
- `157c904114dca152b92a15ef9b77e1d8f440e6c4` — `docs: finalize PR58 evidence in handoff`.

Store-safe workflow/preflight continuation commits include focused CI, test, build-mode and documentation commits culminating in historical frozen executable source `8489d19734d6142054156d5b57f2713195c16b65`.

PR #59 evidence/status documentation commits include:

- `dd9c4cc69c7f5e4371566e7ea11787f1726f142b` — `docs: record PR59 store-safe configuration verification`;
- `aec6fbf559af2dec6f5734992302d7e0e28d3461` — `docs: promote PR59 store-safe verification baseline`;
- `b7a494004c8b35fa1c54eac82b4df33849c23ae1` — `docs: promote PR59 in project status`;
- `91f9ee53dca0cd5ea1306b315b3beecaea524f42` — `docs: promote PR59 in documentation hub`;
- `2df6b26a56fb877ae40f549c8c5d1bc5abfa5e40` — `docs: document PR59 store-safe automation baseline`;
- `1997c37da8d2b04e3f93c879afe6840d9ef1d37e` — `docs: advance next steps to PR59 baseline`;
- `531bebd512151b6a3c68cc1004384ec10b082637` — `docs: promote PR59 in release checklist`.

Current PR #61 source and evidence commits are recorded in `what_changed.md` and `docs/releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md`.

GitHub commit metadata for this continuation uses `Sanskar <sanskarin@outlook.in>`.

### Production work intentionally still open

PR #61 automation and internal artifact evidence do not complete:

- actual signed Apple App Store candidate generation;
- actual signed Google Play candidate generation;
- installed packaged About-page verification of funding-link visibility;
- real Android/Windows/iOS/iPadOS/Mac Catalyst manual matrices;
- actual notification permission/delivery/recovery behavior;
- Android alarm/battery/reboot/time/time-zone checks;
- packaged SQLite existing-data upgrade/integrity/readability;
- encrypted document/backup historical compatibility;
- clean-install restore;
- accessibility testing;
- submission-time Apple/Google store-policy re-review;
- signing credentials/configuration outside Git;
- signed artifact generation/inspection/provenance;
- store listing/screenshots/privacy/data-safety metadata;
- exact final production tag CareNest CI/CodeQL/audit/Store Package Configuration/Store Inspection Artifacts/Release Gate/Release Evidence;
- final version/build/checksums/publication.

These remain release-blocking until actual evidence exists.

## [Unreleased] - 2026-08-14

### Added — complete project documentation

- `docs/COMPLETE_PROJECT_DOCUMENTATION.md` as the canonical end-to-end project reference covering product identity, scope, architecture, data, reminders, security/privacy, encryption, backup, setup, testing, release and documentation map.
- `docs/CODEBASE_REFERENCE.md` with concrete Shared/Domain/Application/Infrastructure/MAUI/test project and file responsibilities.
- `docs/CONFIGURATION_REFERENCE.md` documenting central package versions, build/analyzer policy, NuGet audit behavior, target frameworks, MAUI target isolation, workflows, Git identity, secrets and provenance.
- `docs/MAINTENANCE_AND_OPERATIONS.md` covering routine maintenance, triage, dependency/schema/crypto/reminder changes, documentation, exact-head verification, release, signing, hotfix and incident operations.
- `docs/releases/DOCUMENTATION_AUDIT_20260814.md` recording the repository-wide documentation inventory and separating documentation completeness from production release completeness.
- Exact historical snapshots under `docs/history/pre-complete-docs-20260814/` before replacing stale active security/threat/setup/architecture/notification/documentation-standard/changelog/handoff files.

### Changed — documentation hub and public project entry

- Root `README.md` promoted PR #56 as the then-current automated release-engineering baseline while retaining PR #54 as historical runtime bug-audit evidence.
- `docs/README.md` indexed the whole-project, codebase, configuration, maintenance, architecture, privacy/security, testing, release and history documentation set.
- `docs/releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md` covered source/API/configuration/automation/maintainer documentation and the PR #56 285-test baseline.
- `docs/DOCUMENTATION_STANDARDS.md` defined evidence wording, historical preservation, PR #56 verification citation, SQLite remediation wording, reminder reconciliation language and production/manual evidence rules.
- `docs/setup/DEVELOPMENT.md` used the then-current toolchain/package/release-script/PR #56 source truth and repository-local Git identity behavior.
- `docs/architecture/ARCHITECTURE.md` described reminder cross-surface compensation, SQLite/provider state, document/backup security, platform boundaries and exact-tag release architecture.
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md` documented effective snooze due time, cancellation-first actions, medicine/profile/appointment compensation and the real Android/Windows/Apple release matrix.
- `docs/security/SECURITY_MODEL.md` and `docs/security/THREAT_MODEL.md` used PR #56 as the then-active automated baseline.

### Added — release workflow policy contracts

Release-engineering hardening added source-policy tests covering:

- `v*` production tag/manual workflow entry points;
- Dependency Audit event safety for pull-request-only metadata;
- Release Evidence source provenance and run-attempt identity;
- independent unit/integration/UI/dependency/workspace evidence capture;
- failure-preserving evidence artifact upload before aggregate failure;
- release evidence retention expectations;
- blocking release-preflight NuGet audit behavior;
- clean-checkout local quality-gate behavior;
- PowerShell native command failure handling;
- repository-local Git setup identity/root/failure behavior;
- Release Gate nested unchecked checklist/open-risk matching and required security/evidence surfaces.

These contracts increased the authoritative UI-contract/policy test count from 100 at PR #54 to 124 at PR #56.

### Changed — exact production-tag verification

At that 2026-08-14 boundary, production tags matching `v*` were configured to run the exact tagged commit through CareNest CI, CodeQL, Dependency Audit, Release Gate, and CareNest Release Evidence. The 2026-08-15 continuation later added CareNest Store Package Configuration and CareNest Store Inspection Artifacts to that matrix.

A tag is not production approval until all required automated and manual/device/accessibility/store/signing/package compatibility gates have completed successfully.

### Changed — Release Evidence

CareNest Release Evidence records and/or retains:

- exact source commit/ref;
- GitHub Actions run ID;
- run attempt;
- .NET toolchain information;
- tracked-source file manifest;
- tracked-source SHA-256 checksums;
- unit test TRX;
- integration test TRX;
- UI-contract/policy TRX;
- transitive dependency inventories;
- workspace integrity state;
- evidence checksums.

Evidence components are attempted independently. Available evidence is uploaded with failure-preserving behavior before the final aggregate success/failure gate.

Artifact identity contains source/run/attempt information so reruns do not become ambiguous.

### Changed — local quality/release scripts

- Bash and PowerShell release-preflight scripts treat unsuppressed NuGet audit failure as blocking.
- Selected MAUI target dependency graphs are audited before optional release target builds.
- Bash/PowerShell quality-gate scripts run clean-checkout-safe core test/audit sequences.
- PowerShell scripts explicitly fail on required native command errors.
- Git setup scripts locate the repository root, use repository-local configuration, set `Sanskar` / `sanskarin@outlook.in`, verify the values and fail on Git errors.

### Changed — production Release Gate

Release Gate fails closed for:

- unresolved/open dependency risk state;
- nested unchecked applicable release checklist rows;
- required security/evidence document absence;
- core test failure.

Matching is hardened against ordinary indentation/case variations that previously could make a textual gate fragile.

### Fixed — reminder effective-due behavior

- Future snoozes no longer disappear from upcoming results when the original scheduled instant has passed.
- Overdue snoozes are evaluated from `SnoozedUntilUtc` rather than the stale original time.
- `SnoozedUntilUtc` is the effective due time while a snooze is valid.

### Fixed — stale platform reminder reconciliation

- Existing OS requests are cancelled before replacement, quiet-hours suppression or invalidation.
- Schedule edits retain enough old occurrence identity to cancel stale OS requests before final cleanup.
- Platform cancellation failure remains retryable instead of falsely marking the state reconciled.

### Fixed — reminder action ordering/recovery

Handled Taken/Skipped/Delayed/Missed/Snoozed/Cancelled actions use cancellation-first ordering:

1. cancel old platform request;
2. persist handled state only after cancellation succeeds;
3. for snooze, schedule the replacement after state persistence;
4. if later essential persistence/scheduling fails, restore previous state and attempt non-cancelled rebuild;
5. aggregate recovery failure instead of claiming consistency.

### Fixed — medicine/profile reminder compensation

- Future platform requests are cancelled before medicine/profile database cascade deletion.
- If persistence fails after platform cancellation, a non-cancelled rebuild compensation is attempted for records that still exist.
- Save flows reconcile reminders before later non-critical audit bookkeeping can make an already-applied primary change appear failed.

### Fixed — appointment reminder persistence

- Appointment database/platform reminder state is reconciled/compensated around persistence failures.
- Appointment deletion cancels the platform request before record deletion.
- `StartsUtc` continues to require actual UTC; local/unspecified ticks are rejected rather than relabeled.
- Background appointment rebuild does not repeatedly prompt for notification permission and does not schedule while permission remains denied.

### Fixed — report cache lifecycle

Application-owned shared report cache files are removed after successful share handoff where CareNest still owns the temporary copy.

External copies already controlled by another application/location remain outside CareNest’s deletion guarantee.

### Security — SQLite dependency remediation

The former `GHSA-2m69-gcr7-jv3q` source exception is remediated.

Current verified package intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- central transitive pinning enabled;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- selected SQLitePCLRaw providers `2.1.12`;
- former exact advisory `NuGetAuditSuppress` removed.

`SqliteDependencySecurityContractTests` prevents silent restoration of the old package/suppression baseline.

Source remediation is complete; representative packaged existing-database/encrypted-data compatibility remains a separate manual production gate.

### Security — encryption/backup/document protections retained

- New encrypted document/backup streams use chunked AES-256-GCM framing v2 with authenticated terminal state.
- Legacy framing v1 remains readable for compatibility.
- Trailing bytes after v2 terminal are rejected.
- Strict decrypted backup archive topology is validated before extraction.
- Missing/corrupt document master key with existing ciphertext fails closed rather than silently creating a replacement key.
- Known mutable application-owned key/verifier/salt/crypto buffers are cleared where practical.
- Document import and backup restore use compensating cleanup/rollback across independent state surfaces.

### Security — local-first/logging boundaries retained

- No CareNest account/backend/cloud-sync requirement was introduced.
- No hidden runtime analytics/telemetry client was introduced.
- Logging privacy contracts continue to prevent routine health content/secrets/raw sensitive exception data from leaking into normal application logs.
- Voluntary project support remains an explicit external browser action and is separate from health behavior/access.

## Verification — PR #56 historical release-engineering baseline

PR #56: `Verify complete CareNest release-engineering source`.

Frozen source/base:

`4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`

Marker head:

`e3bc621cea05364a69abee0dadbd71a67c17bddb`

Evidence:

- CareNest CI #571 / `31770929379`: **success**;
- formatting: **success**;
- UnitTests: **122 passed, 0 failed, 0 skipped**;
- IntegrationTests: **39 passed, 0 failed, 0 skipped**;
- UiTests/source-policy: **124 passed, 0 failed, 0 skipped**;
- total core tests: **285 passed, 0 failed, 0 skipped**;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- CodeQL #571 / `31770929382`: **success**;
- unsuppressed Dependency Audit #41 / `31770929383`: **success**.

PR #56 was closed without merge. Its marker is not part of `main`.

PR #54 remains the historical authoritative runtime bug-audit baseline. PR #55 remains superseded intermediate release-engineering evidence.

## Documentation-completion commits — 2026-08-14

The documentation completion pass added or aligned the following logical commits on `main`:

- `06f6ae6968d01e272ab1c0b37190442df867c637` — `docs: add complete project documentation`;
- `2796e2852c659f88e64666c7894c13cc08cda2e1` — `docs: promote PR56 in root README`;
- `20649ff30bc1fb8b8c6321d725e492209e1dae9` — `docs: add complete codebase reference`;
- `37a179aaa2ad3d9a7ac944712cacb2e0d01a0183` — `docs: add configuration and automation reference`;
- `d7ca9b8400caf20ac506a9bfb81c8c3d58bc5da7` — `docs: add maintenance and operations manual`;
- `198c8355348aaee76c30781d51214ae355e1dae9` — `docs: record complete documentation audit`;
- `332f95610c80000c7f5f3ae01074877fb438cab6` — `docs: complete documentation hub index`;
- `22116ebc4057d1eab33fb123593072b17c7bb115` — `docs: complete documentation checklist for PR56`;
- `e7a7dde60a710ffc1fe25ce28a15aad1b72f0e3d` — `docs: preserve pre-completion documentation snapshots`;
- `7a783dd7f9edf15e2f0f0b9943d7289c209f051c` — `docs: finalize current security architecture`;
- `d30d707e2fbcf7e98bd2372cf6b3865debd41bd6` — `docs: finalize current threat model`;
- `24b621c114cf877d603c315bcd64b9e9e9c8d301` — `docs: finalize PR56 development setup`;
- `998d03f784b6ec85d18991596df45012c89b4d79` — `docs: finalize current architecture reference`;
- `04cb7563949ba4a9f5d8cac46c08a84d94c844bd` — `docs: finalize notification platform behavior`;
- `fb07250ab61d9ddcdb1760c862dd231d49100107` — `docs: finalize documentation standards`.

GitHub commit metadata for this continuation uses `Sanskar <sanskarin@outlook.in>`.

## [1.0.0-rc.1] - 2026-08-09

### Added

- Complete local-first CareNest project structure and first RC implementation.
- Profiles, medicine records, schedules, reminder occurrences and medication log.
- Appointments, encrypted document organization, stock/refill tracking and reports.
- Password-encrypted manual backup/restore and portable encrypted-document recovery.
- App lock, notification diagnostics, accessibility/theme settings and developer tools.
- Android, iOS, Mac Catalyst and Windows platform integration structure.
- Security, privacy, threat-model, setup, release and contribution documentation.
- Automated unit/integration/source-contract tests and GitHub Actions.
- Voluntary project-support link and support metadata.

### Safety

- Non-diagnostic/non-treatment boundaries included across onboarding, About, reports and documentation.
- Reminder/stock limitations surfaced instead of silently inferred.
- Project support explicitly separated from medical advice, reminder behavior, emergency assistance and access to local data.

## Historical changelog

The complete detailed changelog that existed before the current documentation-completion alignment—including 2026-08-12, 2026-08-13, earlier 2026-08-14 runtime/security work and historical dependency statements—is preserved byte-for-byte at:

`docs/history/pre-complete-docs-20260814/CHANGELOG.md`

Use that file together with `what_changed.md`, `docs/history/`, and dated verification documents when investigating historical source/evidence states.