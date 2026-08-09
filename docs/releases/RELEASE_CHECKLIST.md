# Release Checklist

## Automated verification evidence

Verification PR: `#15`  
Source head verified: `682aef2aa31981c6be31086aa7af8e1c8e56e94b`  
CareNest CI run: `#67` / `31300473171`  
CodeQL run: `#66` / `31300473160`

- [x] Unit tests — 15 passed, 0 failed, 0 skipped.
- [x] Integration tests — 11 passed, 0 failed, 0 skipped.
- [x] UI-contract tests — 8 passed, 0 failed, 0 skipped.
- [x] Android Release build.
- [x] Windows Release build.
- [x] iOS simulator Release build.
- [x] Mac Catalyst Release build.
- [x] CodeQL analysis.

The checkmarks above record only GitHub-hosted automated evidence for the stated source head. They do not substitute for the manual device, signing, accessibility, notification-delivery, or store-readiness checks below.

## Release preparation and manual verification

- [ ] Update version, changelog and release notes for the final release tag.
- [ ] `dotnet format --verify-no-changes` on a fully provisioned development host.
- [ ] Restore with locked/known package sources.
- [x] Build Domain/Application/Infrastructure through the automated test/build pipeline.
- [x] Run unit, integration and UI-contract tests.
- [x] Build Android release.
- [x] Build Windows release.
- [x] On macOS, build iOS simulator and Mac Catalyst release targets.
- [ ] Manual onboarding smoke test.
- [ ] Create/edit/delete profiles on real/emulated target devices.
- [ ] Create/pause/resume medicine schedules.
- [ ] Verify notification permission denied and granted flows.
- [ ] Verify Android battery/exact-alarm diagnostics on a device/appropriate emulator.
- [ ] Verify time-zone change rebuild on supported devices.
- [ ] Mark taken/skipped/delayed/missed and edit log.
- [ ] Import/export/delete encrypted documents.
- [ ] Create appointment and calendar export.
- [ ] Export CSV and PDF reports; verify disclaimers.
- [ ] Create encrypted backup; restore on clean data; reject wrong password on a release build.
- [ ] Enable/disable app lock and verify cold-start lock.
- [ ] Large-text, screen reader, keyboard and reduced-motion checks.
- [ ] Light/dark/system theme checks.
- [ ] Confirm no document content or credentials in device logs.
- [ ] Review third-party notices and licenses.
- [ ] Review `docs/security/DEPENDENCY_RISK_REGISTER.md`; the SQLitePCLRaw advisory remains open until an available compatible patched dependency path exists.
- [ ] Sign packages using secrets outside the repository.
- [ ] Verify store privacy disclosures match actual behavior.

## Release rule

Do not tag or publish a final `1.0.0` build while an automated platform gate is failing or incomplete, while the tracked SQLite dependency advisory has not received an explicit release decision, or before required manual checks for the intended distribution platforms are completed.
