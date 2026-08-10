# Release Checklist

## Automated verification evidence

Latest exact-head verification PR: `#28`  
Source head verified: `69c4dd9319f7dc47edea1786e683f7d90c656e1e`  
Verification marker head: `a1362b551749762ae816e8b4366c8f1eb97538fa`  
CareNest CI run: `#220` / `31378000135`  
CodeQL run: `#220` / `31378000143`  
Dependency Audit run: `#8` / `31378000134`

- [x] Platform-neutral `dotnet format --verify-no-changes` gate.
- [x] Unit tests — 37 passed, 0 failed, 0 skipped.
- [x] Integration tests — 13 passed, 0 failed, 0 skipped.
- [x] UI-contract/policy tests — 51 passed, 0 failed, 0 skipped.
- [x] Total automated test cases in the core job — 101 passed, 0 failed, 0 skipped.
- [x] Android Release build.
- [x] Windows Release build.
- [x] iOS simulator Release build.
- [x] Mac Catalyst Release build.
- [x] CodeQL analysis.
- [x] Dependency Audit.

PR #28 was a verification-only branch containing only `build/verification/rc1-reminder-applock-hardening-20260810.txt` beyond source head `69c4dd...`. It was closed without merging after the full matrix completed successfully, so the marker is not production source.

The verified source includes the previous repository/privacy/architecture hardening plus the new medicine schedule validation coverage, recurrence/date/state/DST boundary coverage, reminder window/dedup/order coverage, WAL snapshot content/cancellation coverage, app-lock verifier-buffer clearing, app-lock security contracts, and aligned security/testing documentation.

The checkmarks above record GitHub-hosted automated evidence for source head `69c4dd...`. Later status/changelog/handoff documentation commits do not change runtime/test/product source and are not represented as a new platform-verification head.

Automated green status does not substitute for manual device, signing, accessibility, notification-delivery, current store-policy, or dependency-risk checks.

## Verification hardening sequence

The current green result follows marker-only exact-head verification cycles that expose defects instead of weakening quality gates:

- PR #24 / CI #175: found CA1873 eager logger-argument evaluation and CA1861 test-allocation analyzer failures; CodeQL succeeded.
- PR #25 / CI #190: formatting, unit and integration tests passed; Dependency Audit #5 and CodeQL #190 passed; UI-contract execution exposed project-reference path normalization, generated-file scanning and an existing StartupCoordinator exception-object logging issue; MAUI compile also confirmed explicit logger-level guards were required.
- PR #26 / CI #198: Dependency Audit #6 and CodeQL #198 passed; formatting, unit and integration tests passed; UI compilation found one remaining nullable project-reference filename contract error.
- PR #27 / CI #200: all automated gates passed for the privacy/policy hardening baseline.
- PR #28 / CI #220: all automated gates passed after reminder schedule/DST/window hardening, WAL snapshot integrity/cancellation coverage, and app-lock verifier clearing/security contracts were added.

## Release-preparation additions now present

- custom scalable CareNest Buy Me a Coffee vector artwork and original compact support badge;
- clickable support surfaces in README, SUPPORT, in-app About, `BUY_ME_A_COFFEE.md`, and `docs/SUPPORT_CARENEST.md`;
- Bash and PowerShell release-preflight scripts;
- manual device test matrix;
- store submission checklist;
- SQLite dependency migration plan;
- privacy-safe structured bug report form;
- Dependency Audit workflow;
- production Release Gate workflow;
- Release Evidence workflow for source/toolchain/test/dependency/checksum evidence;
- logging privacy contract;
- deterministic reminder scheduling contract;
- production quality gate;
- security release-review checklist;
- release-notes template;
- exact-head verification-branch protocol;
- automated repository/architecture/ViewModel/data-model/branding/async/logging/app-lock policy contracts;
- original light, dark, and monochrome CareNest mark variants.

## Release preparation and manual verification

### Automated/preflight

- [ ] Decide final `1.0.0` version/build metadata and release date.
- [ ] Run `build/scripts/release-preflight.sh` or `build/scripts/release-preflight.ps1` on a fully provisioned development host.
- [x] Platform-neutral `dotnet format --verify-no-changes` succeeds on the exact verified source head.
- [x] Required project restores used by automated tests/platform builds succeed on GitHub-hosted runners.
- [x] NuGet dependency vulnerability audit ran and completed successfully under the repository's narrowly scoped advisory policy.
- [x] Automated repository policy confirms no `TODO`, `FIXME`, or `NotImplementedException` implementation markers in committed runtime source.
- [x] CareNest CI is green for the exact verified source head.
- [x] CodeQL is green for the exact verified source head.
- [x] Dependency Audit is green for the exact verified source head.
- [ ] Run the manual/tag-triggered `CareNest Release Evidence` workflow for the exact commit ultimately promoted to public `1.0.0`.

### Automated reminder, snapshot, and app-lock safety coverage

- [x] Every-N-hours requires explicit valid interval and exactly one explicit starting time.
- [x] Selected-weekday schedules require at least one explicit selected day.
- [x] Cycle schedules require explicit positive on/off day values.
- [x] Schedule end-before-start and out-of-range clock times are rejected.
- [x] Unknown time-zone identifiers are rejected by schedule validation.
- [x] As-needed schedules create no automatic occurrences.
- [x] Paused/completed/archived medicines and disabled schedules create no automatic occurrences.
- [x] Custom/schedule/medicine date boundaries are enforced.
- [x] Half-open planning windows include `fromUtc` and exclude `toUtc`.
- [x] Duplicate explicit times are deduplicated by stable occurrence identity.
- [x] Out-of-order explicit times return chronologically ordered occurrences.
- [x] DST-invalid spring-forward local times do not cause an invented alternate reminder time.
- [x] DST-overlap local times produce a deterministic occurrence.
- [x] WAL snapshots contain committed profile data and pass SQLite integrity check.
- [x] Pre-cancelled WAL snapshot requests throw cancellation and leave no output file.
- [x] App-lock verifier uses salted PBKDF2-HMAC-SHA256 and fixed-time comparison.
- [x] Derived and retrieved verifier buffers are cleared after verification paths.
- [x] Plaintext PIN persistence is rejected by source contracts; disabling app lock removes stored lock material.

### Core product behavior

- [ ] Complete applicable rows in `docs/releases/MANUAL_TEST_MATRIX.md`.
- [ ] Manual onboarding smoke test.
- [ ] Create/edit/delete profiles on real/emulated target devices.
- [ ] Create/pause/resume/complete/archive medicine schedules.
- [ ] Verify daily, selected-weekday, every-N-hours, cycle/custom-range and as-needed behaviors on supported targets.
- [ ] Verify notification permission denied and granted flows.
- [ ] Verify Android battery/exact-alarm diagnostics on a device/appropriate emulator.
- [ ] Verify reboot/time/time-zone rebuild behavior on applicable platforms.
- [ ] Verify stored schedule intent is not silently rewritten after a time-zone change.
- [ ] Mark taken/skipped/delayed/missed and edit medication log.
- [ ] Verify quiet hours and follow-up reminder behavior.
- [ ] Import/export/delete encrypted documents.
- [ ] Create appointment and calendar export.
- [ ] Export CSV, JSON and PDF reports; verify disclaimers/privacy boundaries.
- [ ] Create encrypted backup; restore on clean data; reject wrong password and tampered backup.
- [ ] Enable/disable app lock and verify cold-start lock on target devices.
- [ ] Verify local reset/profile deletion destructive confirmations and expected cleanup.

### Accessibility and presentation

- [ ] Large-text/manual scaling checks.
- [ ] Screen-reader traversal and accessible names.
- [ ] Keyboard navigation on applicable desktop targets.
- [ ] Reduced-motion preference checks.
- [ ] Light/dark/system theme checks.
- [ ] Confirm error/validation text remains readable and actionable.
- [ ] Confirm color is not the only status/validation signal.
- [ ] Validate app icon/splash/store screenshots using fictional data only.
- [ ] Verify light/dark/monochrome CareNest brand assets on intended system/store surfaces.

### Privacy/security

- [x] Automated logging policy prevents full exception-object logger calls in committed runtime source.
- [x] Global, UI, startup and reminder exception paths log only safe metadata such as exception type names.
- [x] Automated policy checks reject common signing/secret files from the committed workspace.
- [x] Automated policy checks reject runtime network/telemetry client introduction for the local-first v1 scope.
- [x] Automated policy checks reject named diagnosis/dosage/treatment/interaction/risk-scoring feature regressions.
- [x] App-lock security contract protects salted PBKDF2-HMAC-SHA256, fixed-time verification, verifier-buffer clearing, no plaintext PIN persistence, and lock-material removal.
- [ ] Confirm on target devices that no document content, backup passwords, plaintext PINs, sensitive notes or private file paths appear in device/platform logs.
- [ ] Confirm export/share operations occur only after explicit user action.
- [ ] Confirm no CareNest account/backend/network requirement appears in normal local-first flows.
- [ ] Review `docs/security/THREAT_MODEL.md` for the exact public-release candidate.
- [ ] Review `docs/security/LOGGING_PRIVACY.md` for the exact public-release candidate.
- [ ] Complete `docs/releases/SECURITY_RELEASE_REVIEW.md`.
- [ ] Review `docs/security/DEPENDENCY_RISK_REGISTER.md`.
- [ ] Review `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` if changing SQLite-related packages.
- [ ] Resolve or explicitly block release for the open SQLitePCLRaw advisory; do not treat `NuGetAuditSuppress` as a fix.
- [ ] Review third-party notices and licenses.

### Buy Me a Coffee / funding link

Project-support URL:

`https://buymeacoffee.com/sanskarIN`

- [x] URL centralized in CareNest shared constants.
- [x] About/support action exists.
- [x] In-app support artwork is clickable.
- [x] GitHub funding metadata exists.
- [x] Custom vector project-support artwork exists.
- [x] Original compact CareNest support badge exists.
- [x] Clickable README/SUPPORT/root/documentation support pages exist.
- [x] Support is documented as voluntary and not a CareNest feature entitlement or medical service.
- [ ] Review current rules for external funding/tipping/donation links on every store/distribution channel used for the final package.
- [ ] If a target store disallows the link for the submitted configuration, remove/disable the in-app external funding action for that target before packaging while retaining repository funding links where permitted.
- [ ] Confirm on packaged builds that no CareNest health data is sent merely by displaying/opening the external funding link.
- [ ] Confirm custom badge is not represented as official Buy Me a Coffee brand artwork.

### Signing and distribution

- [ ] Complete `docs/releases/STORE_SUBMISSION_CHECKLIST.md` for every intended store/channel.
- [ ] Sign packages using secrets/certificates/profiles stored outside the repository.
- [ ] Verify final package IDs/bundle IDs/publisher identities.
- [ ] Verify store privacy/data-safety disclosures match the shipping runtime behavior.
- [ ] Verify support/privacy/terms/security URLs and contacts in final listings.
- [ ] Record exact source commit SHA for each signed package.
- [ ] Record exact final CI/CodeQL/Dependency Audit/Release Evidence run IDs.
- [ ] Generate final release notes from `docs/releases/RELEASE_NOTES_TEMPLATE.md`.
- [ ] Create final tag/GitHub release only after all applicable gates above are satisfied.

## Release rule

Do not tag or publish a final `1.0.0` build while an automated platform gate is failing/incomplete, while required manual checks are incomplete, while current store-policy review for the BMC link is unresolved, while signing/store identity is unfinished, or while the tracked SQLite dependency advisory has not received an explicit release decision/resolution.

Automated green status is necessary but not sufficient for public release.
