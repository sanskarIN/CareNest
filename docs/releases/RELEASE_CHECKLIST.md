# Release Checklist

## Automated verification evidence

Latest runtime/funding-enabled verification PR: `#16`  
Source head verified: `2b8f97525ea8d3b41bf62e20d76e1cc224dab102`  
CareNest CI run: `#87` / `31301203981`  
CodeQL run: `#86` / `31301203985`

- [x] Unit tests — 15 passed, 0 failed, 0 skipped.
- [x] Integration tests — 11 passed, 0 failed, 0 skipped.
- [x] UI-contract tests — 10 passed, 0 failed, 0 skipped.
- [x] Total automated tests — 36 passed, 0 failed, 0 skipped.
- [x] Android Release build.
- [x] Windows Release build.
- [x] iOS simulator Release build.
- [x] Mac Catalyst Release build.
- [x] CodeQL analysis.

PR #16 was a verification-only branch containing a marker file and was closed without merging the marker after the matrix completed successfully.

The checkmarks above record only GitHub-hosted automated evidence for the stated source head. They do not substitute for manual device, signing, accessibility, notification-delivery, current store-policy, or dependency-risk checks.

## Post-verification repository presentation additions

The following later commits add repository/support/release-preparation material without changing CareNest medical, scheduling, persistence, encryption, or reminder logic:

- custom vector BMC badge at `src/CareNest.App/Resources/Images/buy_me_a_coffee_carenest.svg`;
- clickable visual support pages at `BUY_ME_A_COFFEE.md` and `docs/SUPPORT_CARENEST.md`;
- shell/PowerShell release-preflight scripts;
- complete manual device test matrix;
- store submission checklist;
- SQLite dependency migration plan.

Because the SVG is stored in the MAUI image resource tree, the exact final packaging commit should still receive a fresh platform build before a signed/public release even though the artwork itself does not alter application logic.

## Release preparation and manual verification

### Automated/preflight

- [ ] Decide final `1.0.0` version/build metadata and release date.
- [ ] Run `build/scripts/release-preflight.sh` or `build/scripts/release-preflight.ps1` on a fully provisioned development host.
- [ ] `dotnet format --verify-no-changes` succeeds on the exact release source.
- [ ] Restore from known package sources succeeds.
- [ ] Re-run NuGet dependency vulnerability reporting and inspect every reported advisory.
- [ ] Confirm no `TODO`, `FIXME`, or `NotImplementedException` implementation markers exist in release source/tests.
- [ ] Confirm latest CareNest CI is green for the exact commit/package candidate.
- [ ] Confirm latest CodeQL is green for the exact commit/package candidate.

### Core product behavior

- [ ] Complete applicable rows in `docs/releases/MANUAL_TEST_MATRIX.md`.
- [ ] Manual onboarding smoke test.
- [ ] Create/edit/delete profiles on real/emulated target devices.
- [ ] Create/pause/resume/complete/archive medicine schedules.
- [ ] Verify daily, selected-weekday, every-N-hours, cycle/custom-range and as-needed behaviors.
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
- [ ] Enable/disable app lock and verify cold-start lock.
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

### Privacy/security

- [ ] Confirm no document content, backup passwords, plaintext PINs, or sensitive notes appear in device logs.
- [ ] Confirm export/share operations occur only after explicit user action.
- [ ] Confirm no CareNest account/backend/network requirement appears in normal local-first flows.
- [ ] Review `docs/security/THREAT_MODEL.md`.
- [ ] Review `docs/security/DEPENDENCY_RISK_REGISTER.md`.
- [ ] Review `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` if changing SQLite-related packages.
- [ ] Resolve or explicitly block release for the open SQLitePCLRaw advisory; do not treat `NuGetAuditSuppress` as a fix.
- [ ] Review third-party notices and licenses.

### Buy Me a Coffee / funding link

Project-support URL:

`https://buymeacoffee.com/sanskarIN`

- [x] URL centralized in CareNest shared constants.
- [x] About/support action exists.
- [x] GitHub funding metadata exists.
- [x] Custom vector project-support artwork exists.
- [x] Clickable root/documentation support pages exist.
- [x] Support is documented as voluntary and not a CareNest feature entitlement or medical service.
- [ ] Review current rules for external funding/tipping/donation links on every store/distribution channel used for the final package.
- [ ] If a target store disallows the link for the submitted configuration, remove/disable the in-app external funding action for that target before packaging while retaining repository funding links where permitted.
- [ ] Confirm no CareNest health data is sent merely by displaying/opening the external funding link.
- [ ] Confirm custom badge is not represented as official Buy Me a Coffee brand artwork.

### Signing and distribution

- [ ] Complete `docs/releases/STORE_SUBMISSION_CHECKLIST.md` for every intended store/channel.
- [ ] Sign packages using secrets/certificates/profiles stored outside the repository.
- [ ] Verify final package IDs/bundle IDs/publisher identities.
- [ ] Verify store privacy/data-safety disclosures match the shipping runtime behavior.
- [ ] Verify support/privacy/terms/security URLs and contacts in final listings.
- [ ] Record exact source commit SHA for each signed package.
- [ ] Record exact final CI/CodeQL run IDs.
- [ ] Generate final release notes/changelog.
- [ ] Create final tag/GitHub release only after all applicable gates above are satisfied.

## Release rule

Do not tag or publish a final `1.0.0` build while an automated platform gate is failing/incomplete, while required manual checks are incomplete, while current store-policy review for the BMC link is unresolved, while signing/store identity is unfinished, or while the tracked SQLite dependency advisory has not received an explicit release decision/resolution.

Automated green status is necessary but not sufficient for public release.
