# Release Checklist

## Automated verification evidence

Latest verification PR: `#16`  
Source head verified: `2b8f97525ea8d3b41bf62e20d76e1cc224dab102`  
CareNest CI run: `#87` / `31301203981`  
CodeQL run: `#86` / `31301203985`

- [x] Unit tests — 15 passed, 0 failed, 0 skipped.
- [x] Integration tests — 11 passed, 0 failed, 0 skipped.
- [x] UI-contract tests — 10 passed, 0 failed, 0 skipped.
- [x] Android Release build.
- [x] Windows Release build.
- [x] iOS simulator Release build.
- [x] Mac Catalyst Release build.
- [x] CodeQL analysis.

Total automated tests in the latest core job: 36 passed, 0 failed, 0 skipped.

PR #16 was a verification-only pull request. Its marker file was not merged into `main`; the PR was closed after the complete CI and CodeQL matrix passed.

The checkmarks above record GitHub-hosted automated evidence for the stated source head. Documentation-only commits after that source head do not change product runtime behavior. Any later runtime/UI/dependency change must receive a new complete verification pass before final release.

## Release preparation and manual verification

- [ ] Update version, changelog and release notes for the final release tag.
- [ ] `dotnet format --verify-no-changes` on a fully provisioned development host.
- [ ] Restore with locked/known package sources.
- [x] Build Domain/Application/Infrastructure through the automated test/build pipeline for the latest verified source head.
- [x] Run unit, integration and UI-contract tests for the latest verified source head.
- [x] Build Android release for the latest verified source head.
- [x] Build Windows release for the latest verified source head.
- [x] On macOS, build iOS simulator and Mac Catalyst release targets for the latest verified source head.
- [ ] Re-run the complete automated verification matrix on the exact final release commit after any later runtime/UI/dependency changes.
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
- [ ] Review `docs/security/DEPENDENCY_RISK_REGISTER.md`; the SQLitePCLRaw advisory remains open until an available compatible patched dependency path exists or an explicit production release decision is recorded.
- [ ] Verify current Apple App Store rules for the external voluntary project-support link before submission.
- [ ] Verify current Google Play rules for the external voluntary project-support link before submission.
- [ ] Confirm Buy Me a Coffee support remains optional and does not unlock medical advice, health features, reminder behavior, support priority, or access to CareNest data.
- [ ] If a distribution channel disallows the in-app external support link, remove or conditionally hide that button for the affected store build while retaining permitted repository funding metadata.
- [ ] Sign packages using secrets outside the repository.
- [ ] Verify store privacy disclosures match actual behavior.
- [ ] Review [`NEXT_STEPS.md`](NEXT_STEPS.md) and record which Priority 0 items are complete.

## Release rule

Do not tag or publish a final `1.0.0` build while an automated platform gate is failing or incomplete, while later runtime/UI/dependency changes have not been re-verified, while the tracked SQLite dependency advisory has not received an explicit release decision/resolution, while the external funding-link policy has not been reviewed for the intended store, or before required manual checks for the intended distribution platforms are completed.
