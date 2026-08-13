# what_changed.md

## CareNest complete continuation handoff — 2026-08-12

This file is the active detailed handoff for the current CareNest continuation. It records the full documentation pass requested after the source-complete `1.0.0-rc.1` implementation and the Phase 8 reminder-integrity hardening.

The complete preceding Phase 0–8 implementation/hardening handoff is preserved unchanged on current `main` at:

`docs/history/what_changed_full_through_phase8.md`

It is also permanently available at its historical commit:

`https://github.com/sanskarIN/CareNest/blob/4571cf7e7149b09102690459c437b3ca844b7efa/what_changed.md`

That preserved handoff contains the complete Phase 0–8 implementation record, PR #24–#30 verification history, SQLite/WAL/app-lock/reminder/privacy hardening history, and previous commit-level details. It was preserved using its exact existing Git blob so those details were not discarded or rewritten. This active file continues from that complete historical state and records the documentation completion work pushed directly to `main`.

Repository: `https://github.com/sanskarIN/CareNest`  
Branch: `main`  
Release target: `1.0.0-rc.1`  
Framework: .NET MAUI / .NET 10  
Primary language: C#  
License: Apache-2.0  
Business email: `sanskarin@outlook.in`  
Support email: `supportramsandesh@gmail.com`  
Creator profile: `https://www.github.com/sanskarIN`  
Voluntary project support: `https://buymeacoffee.com/sanskarIN`  
Watermark: `Made by the Sanskar`

---

# Safety and product boundary retained

CareNest remains a local-first organizational application.

CareNest does **not**:

- diagnose conditions;
- determine, calculate, or infer medicine dosage;
- recommend treatment;
- perform medication-interaction checking as a clinical feature;
- create clinical risk scores;
- independently verify medication adherence;
- replace a clinician or pharmacist;
- provide emergency services;
- guarantee notification delivery.

Medicine `Strength` and `Instructions` remain opaque user-entered text. Reminder schedules originate from explicit user-entered values. Stock/refill calculations use only explicit user-entered quantities/configuration.

The documentation pass does not change this boundary and does not add runtime medical interpretation.

---

# Local-first/privacy boundary retained

CareNest v1 still has:

- no required CareNest account;
- no required CareNest server/backend;
- no automatic CareNest cloud synchronization;
- no silent caregiver sharing;
- no hidden analytics/telemetry client;
- local SQLite structured records;
- separately encrypted imported document payloads;
- manual password-encrypted backups;
- explicit user-controlled document/report/profile/calendar export/share boundaries;
- optional app lock as a local privacy barrier;
- privacy-minimized notification/logging behavior.

The SQLite database is not described as transparently whole-database encrypted. Imported document payloads and manual backup payloads have separate authenticated encryption protections.

---

# Latest exact verified runtime/test source baseline

Exact runtime/test source SHA:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

Verification PR:

`#30 — Reverify reminder ownership, UTC, snooze, and DST hardening`

Verification marker SHA:

`59016b7e2b13d5ac1c93cf0db973f275c6e7eb19`

Marker file:

`build/verification/rc1-ownership-utc-dst-hardening-20260810-2.txt`

PR #30 was closed without merge. The marker did not enter `main`.

## Automated evidence

CareNest CI #248 / run `31382194805`:

- platform-neutral formatting: success;
- UnitTests: 74 passed, 0 failed, 0 skipped;
- IntegrationTests: 13 passed, 0 failed, 0 skipped;
- UiTests: 54 passed, 0 failed, 0 skipped;
- total core tests: 141 passed, 0 failed, 0 skipped;
- Android Release build: success;
- Windows Release build: success;
- iOS simulator Release build: success;
- Mac Catalyst Release build: success.

CodeQL #248 / run `31382194687`: success.

Dependency Audit #10 / run `31382194683`: success.

The green Dependency Audit does not mean the tracked SQLitePCLRaw advisory is fixed.

## Superseded PR #29

PR #29 / CI #246 intentionally exposed CA2263 in the newly added non-generic enum validation. The source was fixed on `main` by using generic `Enum.IsDefined(schedule.Kind)` rather than weakening analyzer policy. PR #29 was closed without merge and is not green release evidence.

---

# Runtime/test hardening already included in the verified baseline

The PR #30 verified source includes the previously completed Phase 8 hardening:

- profile → medicine → schedule → persisted schedule-time ownership validation;
- support for intentionally unbound editor times before persistence;
- defensive archived-profile suppression in the planner;
- recognized schedule-kind validation;
- selected-weekday supported-bit validation;
- trimmed/valid time-zone identifiers;
- actual UTC `DateTime` requirements for planner windows;
- half-open planner window `[fromUtc, toUtc)`;
- UTC rebuild override requirement;
- explicit future-UTC snooze requirement;
- deterministic occurrence identity;
- duplicate-time deduplication;
- chronological occurrence ordering;
- explicit date/state boundaries;
- as-needed no-automatic-reminder behavior;
- selected-weekday/cycle/every-N-hours recurrence validation;
- deterministic DST gap/overlap behavior;
- representative US/UK/Australia/New Zealand DST test coverage when zones are available on the host;
- deterministic fixed-seed property-style recurrence tests;
- WAL snapshot committed-content/integrity/cancellation tests;
- app-lock PBKDF2/fixed-time/verifier-buffer security contracts;
- privacy-redacted global/UI/startup/reminder logging;
- repository/architecture/ViewModel/data-model/branding/async/logging/security policies.

No runtime/test/project/workflow/package/platform source has been changed by the 2026-08-12 documentation completion pass described below.

---

# Complete documentation pass goal

The current continuation specifically completed the project's documentation so users, contributors, maintainers, security reviewers, and release owners have dedicated references for every major subsystem.

The documentation package now covers:

- end-user workflows;
- every major feature and its safety/privacy boundary;
- layered architecture;
- service responsibilities;
- application flows;
- database schema/migrations/WAL behavior;
- local data storage/export/deletion boundaries;
- encrypted backup/restore;
- encrypted document vault;
- notification/platform behavior;
- reports/exports;
- privacy model/data lifecycle;
- security model/threats/logging/dependency risk;
- design system;
- accessibility;
- localization;
- store assets/claims/screenshots;
- development setup;
- platform setup;
- troubleshooting;
- maintainer operations;
- testing strategy;
- release process;
- release/manual/store/security/evidence checklists;
- project terminology;
- contribution standards;
- documentation-governance standards;
- documentation completeness inventory.

Documentation completeness does not falsely mark operational production-release work complete.

---

# Documentation commit record — all pushed directly to `main`

The following logical commits were created during this documentation pass.

## 1. Complete user guide

`82d08305c1e0bc6bb01cbf3f4030c573be94e5bc` — `docs: add complete CareNest user guide`

Created `docs/USER_GUIDE.md` covering onboarding, local-first model, profiles, medicines, schedules, DST, notification limitations, reminder states, quiet hours/follow-ups, medication log, appointments, document vault, reports, backup/restore, app lock, diagnostics, accessibility, privacy, external support, help, and release-candidate limitations.

## 2. Complete feature reference

`ec22d5ac34af3e76f5e2ee9c6b0fe53869029a7d` — `docs: add complete CareNest feature reference`

Created `docs/FEATURE_REFERENCE.md` mapping every major product feature to its behavior and medical/privacy/security boundary, including reminder ownership/UTC/DST/snooze semantics, reports, backup, app lock, documents, diagnostics, funding, and deferred scope.

## 3. Application-flow reference

`0ec2ec1f5a63e7f88fa6f5a3db072518b5a4a1b4` — `docs: document CareNest application flows`

Created `docs/architecture/APPLICATION_FLOWS.md` documenting startup, onboarding, profile, medicine, schedule, reminder materialization/state, stock, appointment, document, report, backup, restore, app-lock, diagnostics, external support, and failure handling.

## 4. Backup/restore architecture

`efcb0ec56bf191f5c2bb4079de032d64dec76e47` — `docs: document encrypted backup and restore architecture`

Created `docs/architecture/BACKUP_AND_RESTORE.md` documenting WAL checkpoint/snapshot behavior, PBKDF2-HMAC-SHA256 + AES-GCM protected backup model, document-key portability, wrong-password/tamper rejection, restore validation, rollback/failure safety, privacy boundaries, and manual release requirements.

## 5. Storage/export/deletion model

`7afee682b4b6ee4a044c5e08fbac575c175b03bb` — `docs: document CareNest storage export and deletion model`

Created `docs/architecture/DATA_STORAGE_AND_EXPORT.md` documenting SQLite, encrypted documents, secure secrets, platform notification state, logging, JSON/CSV/PDF/document/calendar exports, deletion/reset, backup retention, OS copies/screenshots, and contributor lifecycle requirements.

## 6. Service/infrastructure boundaries

`3a3c67afc1e4db244cf73d70b6a231e90e638eb7` — `docs: document application service and infrastructure boundaries`

Created `docs/architecture/SERVICE_BOUNDARIES.md` documenting Shared/Domain/Application/Infrastructure/App responsibilities and repository/notification/navigation/error/time/secure-storage/async boundaries.

## 7. Cross-platform setup

`c4d13ab594b54e6441ce605a12e25a07ef9d0a65` — `docs: add cross-platform CareNest setup guide`

Created `docs/setup/PLATFORM_SETUP.md` documenting Android/Windows/iOS/Mac Catalyst toolchains, build commands, `CareNestTargetFramework`, manual platform checks, signing-secret rules, and maintainer Git identity.

## 8. Accessibility specification

`8fd52c6eba814e49aa6aafa80bfe802d89d7f0de` — `docs: add CareNest accessibility specification`

Created `docs/design/ACCESSIBILITY.md` covering text scaling, screen readers, semantics, keyboard/focus, contrast, color-independent state, reduced motion, themes, errors, safety text, destructive actions, and required manual evidence.

## 9. Complete privacy model

`34c24addc7d872292779252a8141f2b2e596ecef` — `docs: add complete CareNest privacy model`

Created `docs/privacy/PRIVACY_MODEL.md` documenting local-first data categories, no-account/backend boundary, storage/notification/logging privacy, outbound boundaries, deletion/archive, OS copies, no hidden telemetry, store disclosures, and future network requirements.

## 10. Security architecture reference

`92d3c31307f4c027d9af51784130fc906bed7117` — `docs: add CareNest security architecture reference`

Created `docs/security/SECURITY_MODEL.md` documenting trust boundaries, SQLite/document/backup/app-lock protections, notification/reminder integrity, logging/global exception controls, network policy, secrets, dependency security, source hygiene, export/backup/device threats, release review, and future network requirements.

## 11. Complete testing guide

`e1fda936cbf743eb7c459a4c01db83ff215b77d0` — `docs: add complete CareNest testing guide`

Created `docs/testing/TESTING_GUIDE.md` covering unit/integration/UI-contract roles, verified 74/13/54 test counts, formatting, reminder ownership/UTC/property/DST, SQLite/WAL/backup/document/app-lock/report/policy tests, CodeQL, Dependency Audit, exact-head verification, manual testing, and future coverage.

## 12. End-to-end release process

`7715226d7d7ad681ae295b435b42db124689bb09` — `docs: add end-to-end CareNest release process`

Created `docs/releases/RELEASE_PROCESS.md` defining scope freeze, dependency/security review, preflight, exact-head verification, device matrix, backup/accessibility qualification, store-policy review, signing, store metadata, security review, Release Evidence, versioning, checksums, tagging, monitoring, and hotfixes.

## 13. Maintainer operations

`bf83c0bca2e22c83ca6621d006bac614009e0d5d` — `docs: add CareNest maintainer operations guide`

Created `docs/setup/MAINTAINER_OPERATIONS.md` documenting Git identity, branch/commit/source/schema/dependency/security/logging/app-lock/document/backup/docs/CI/verification/release operations.

## 14. Glossary

`e2649fa5235184d8f2a24f6e2cab3740f6be9916` — `docs: add CareNest glossary`

Created `docs/GLOSSARY.md` defining CareNest product/engineering terminology.

## 15. Documentation hub

`54ffc0d3e87e01c59f2b407a55ad5d1b827d2e46` — `docs: add complete CareNest documentation index`

Created `docs/README.md` as the central documentation navigation hub.

## 16. Expanded architecture overview

`3b1d2b8cb831a2e179a1ff4c8e262b086faf42bc` — `docs: expand CareNest architecture reference`

Expanded `docs/architecture/ARCHITECTURE.md` into the complete solution/context/layer/reminder/time-zone/persistence/WAL/document/backup/app-lock/privacy/logging/navigation/accessibility/build/CI/dependency/future architecture reference.

## 17. Expanded database schema

`5769c8e218ab5e2fb6b3d6880734934475ef0813` — `docs: expand CareNest database schema reference`

Expanded `docs/architecture/DATABASE_SCHEMA.md` with entities, ownership, schema versions 1–5, deletion/cascade, WAL/busy timeout/snapshot, migration, and compatibility rules.

## 18. Expanded design system

`6899e2516dd4bf6ec480b647f3446d8be50f05f4` — `docs: expand CareNest design system`

Expanded `docs/design/DESIGN_SYSTEM.md` with design principles/tokens, status/theme/motion/forms/schedule/reminder/destructive/error/safety/responsive/branding/funding/accessibility/localization/store rules.

## 19. Expanded development setup

`270904d89e39c256b2213a2c15c016928c57358c` — `docs: expand CareNest development setup`

Expanded `docs/setup/DEVELOPMENT.md` with clone/toolchain/Git identity/workloads/build/tests/formatting/preflight/dependency/analyzer/architecture/secret/exact-head instructions.

## 20. Expanded troubleshooting

`7db8500a60e4d09236f46a6e40ec5ae9280ae56b` — `docs: expand CareNest troubleshooting guide`

Expanded `docs/setup/TROUBLESHOOTING.md` for notification/schedule/DST/snooze/platform/workload/analyzer/SQLite/dependency/backup/document/app-lock/export/accessibility/public-report issues.

## 21. Expanded data lifecycle

`6ccc5c9734737a3f95552dd58fc949ba553d5d73` — `docs: expand CareNest data lifecycle documentation`

Expanded `docs/privacy/DATA_LIFECYCLE.md` across entry, storage, local processing, reminders, notifications, diagnostics, exports, backup/restore, archive/delete/reset/uninstall, OS copies/screenshots, development, and future network lifecycle.

## 22. Expanded localization architecture

`445db7c644ff15fd80044f26db6cf793d505d2fd` — `docs: expand CareNest localization architecture`

Expanded `docs/design/LOCALIZATION.md` with resource architecture, safety translation, opaque user text, layout/RTL/accessibility, dates/numbers/time zones, reports/notifications, branding, workflow/fallback/testing, and current English-only limitation.

## 23. Expanded store asset guidance

`f4781b084dc58d9937decb5c7f220aede008f9ca` — `docs: expand CareNest store asset guidance`

Expanded `docs/design/STORE_ASSETS.md` with icon/splash/graphic/screenshot, fictional-data, medical/reminder/privacy/app-lock/funding claim, policy, permission, localization/accessibility, and final asset requirements.

## 24. Expanded contribution guide

`8fb0f326f7708602de02feadf36f14c6236d2573` — `docs: expand CareNest contributing guide`

Expanded `CONTRIBUTING.md` with medical/local-first, secrets/data, architecture/reminder/schema/backup/app-lock/logging/setup/Git/commits/tests/PR/CI/dependency/docs/accessibility/store/security rules.

## 25. Notification/platform behavior

`5c27558582441f67b38a7bf34f9f986b97c298e6` — `docs: document notification and platform behavior`

Created `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md` separating deterministic occurrences from Android/iOS/Mac/Windows OS delivery behavior and limitations.

## 26. Encrypted document vault

`8c676427aba448eb6a46ff630ccf8a95b9ea9a41` — `docs: document encrypted CareNest document vault`

Created `docs/architecture/DOCUMENT_VAULT.md` documenting metadata/payload/key/import/export/delete/backup/logging/security/manual-test behavior.

## 27. Reports and exports

`26c8e8f97735af3044c198d4e2bcce8074ca7741` — `docs: document CareNest reports and exports`

Created `docs/REPORTS_AND_EXPORTS.md` documenting JSON/PDF/CSV/calendar/document export contracts and privacy/safety/testing rules.

## 28. Documentation standards

`b4121d2580e9cad6e8c72cde357c4caa1eb44986` — `docs: define CareNest documentation standards`

Created `docs/DOCUMENTATION_STANDARDS.md` defining canonical hub, evidence, medical/reminder/local-first/encryption/dependency/manual/store/funding wording, synthetic-data, command/link, feature/schema/security, and handoff rules.

## 29. Documentation completeness checklist

`1bacfbced6dbd1916188ab85d99cb4d5e088d085` — `docs: add CareNest documentation completeness checklist`

Created `docs/releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md`, inventorying the complete documentation set while leaving real operational release work unchecked.

## 30. Final documentation-hub navigation

`66bcc85cc486339631bcfef36421489db0a414cb` — `docs: complete CareNest documentation hub navigation`

Updated `docs/README.md` to link every newly added subsystem document.

## 31. Root README documentation navigation

`b7cfc300423547b14c9ed5e052438cb5d986a2b6` — `docs: link complete CareNest documentation from README`

Updated root `README.md` with the complete documentation entry point, subsystem links, current PR #30 baseline, and release/security/privacy references.

## 32. Project status documentation package

`789df205fd3387dc58e04a7b596564b4034ff455` — `docs: record complete CareNest documentation package`

Updated `PROJECT_STATUS.md` with documentation completeness while preserving all real release blockers.

## 33. Documentation evidence decision

`72a44ffcfd2496ae2356641007372b5b1579502d` — `docs: record CareNest documentation evidence policy`

Updated `DECISIONS.md` with decision 28: documentation-only commits do not establish a newer runtime verification baseline and documentation cannot mark unperformed manual/store/dependency work complete.

## 34. Changelog documentation pass

`7ac7ee8bfd1e49ea4462449e0a8415240fee834d` — `docs: record complete CareNest documentation pass`

Updated `CHANGELOG.md` with the 2026-08-12 documentation completion record.

## 35. Active documentation handoff

`a9549c1feb1b5391b70b86bdf7cc8615dbaa479e` — `docs: update complete CareNest documentation handoff`

Replaced the active handoff with this documentation-continuation record while referencing the immutable earlier complete handoff.

## 36. Exact preservation of the earlier full handoff

`6c0620e7a529d2763045f5546565f9188cbee98a` — `docs: preserve complete pre-documentation CareNest handoff`

Added:

`docs/history/what_changed_full_through_phase8.md`

The new history file points to the exact prior `what_changed.md` Git blob:

`6e33e76f5f9bf8a9f7c2ef9a76b9ab0088237d57`

The preserved file contains 2,016 lines of the complete earlier handoff. No historical detail was regenerated or shortened for that archive; the exact prior blob was reused.

## 37. Documentation hub links preserved history

`6f4cf40c7d147ba1912539df14619f8cf9d021e3` — `docs: link preserved full CareNest historical handoff`

Updated `docs/README.md` to expose both:

- active `what_changed.md`; and
- the exact preserved Phase 0–8 full handoff under `docs/history/`.

---

# Existing documentation-only evidence immediately before this pass

Before the 2026-08-12 complete documentation work, documentation-only commits after verified source `c61f3c31...` already included:

- `a3a55404f0703f2614a89db86cbb48feaf5dc69f` — project-status baseline promotion;
- `d64a4a84c43d81078928ec70accd3c1cb3f69284` — release checklist evidence;
- `03f44fb07276e2ce7daa161f9875916bba0bf2a5` — next-steps roadmap alignment;
- `c56188ba007a1e22dae8072622fbda6621d2d709` — quality-gate baseline;
- `8c62e626db219c2fe90e61adc832f62f08fe68f2` — security release-review expansion;
- `5af5d12d7b5a617bdbd9414bffd754a7e10d038b` — README verified reminder baseline;
- `9f43bbe4c1f6369a50bf366b30e5839b4714868d` — changelog reminder-hardening evidence;
- `4571cf7e7149b09102690459c437b3ca844b7efa` — detailed Phase 8 `what_changed.md` handoff;
- `2695f0951ecd43cd489655059ab94152101f8b68` — `docs/releases/PHASE8_VERIFICATION_EVIDENCE.md`.

---

# Documentation-only source comparisons

## Compare through the main documentation/changelog pass

Base:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

Head:

`7ac7ee8bfd1e49ea4462449e0a8415240fee834d`

Result:

- 43 commits ahead;
- 30 changed files;
- every changed path Markdown documentation;
- no runtime/test/project/workflow/package/platform source change.

## Compare after active handoff update

Base:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

Head:

`a9549c1feb1b5391b70b86bdf7cc8615dbaa479e`

Result:

- 44 commits ahead;
- every changed path remained Markdown documentation;
- no runtime/test/project/workflow/package/platform source change.

## Compare after exact historical handoff preservation and docs-hub link

Base:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

Head:

`6f4cf40c7d147ba1912539df14619f8cf9d021e3`

Result:

- 46 commits ahead;
- every changed path remained Markdown documentation;
- the preserved historical handoff was added as a 2,016-line Markdown file using the exact old blob;
- no C#, XAML, `.csproj`, solution, package, workflow, build script, runtime resource, signing/configuration, test source, or platform source changed.

This current `what_changed.md` finalization commit is itself Markdown-only. A definitive source-to-final-head compare is performed after this commit to confirm that the documentation-only boundary still holds.

---

# Git commit identity note

Requested local maintainer Git identity:

```bash
git config user.name "Sanskar"
git config user.email "sanskarin@outlook.in"
```

This identity is documented in:

- `build/scripts/setup-git.sh`;
- `build/scripts/setup-git.ps1`;
- `docs/setup/DEVELOPMENT.md`;
- `docs/setup/PLATFORM_SETUP.md`;
- `docs/setup/MAINTAINER_OPERATIONS.md`;
- `CONTRIBUTING.md`.

The connected GitHub contents/Git-data APIs used for these commits do not expose arbitrary author/committer-email parameters through the available connector operations. API-created commits therefore use the authenticated GitHub identity. The repository does not falsely claim that the connector forced `sanskarin@outlook.in` into those commit objects.

When committing locally through Git, the requested email is configured through the repository setup scripts.

---

# Documentation completeness status

The repository now has dedicated documentation for every major current subsystem and operational area.

## User/product

- `docs/USER_GUIDE.md`
- `docs/FEATURE_REFERENCE.md`
- `docs/REPORTS_AND_EXPORTS.md`
- `docs/GLOSSARY.md`
- `SUPPORT.md`
- `docs/SUPPORT_CARENEST.md`
- `BUY_ME_A_COFFEE.md`

## Architecture/data

- `docs/architecture/ARCHITECTURE.md`
- `docs/architecture/APPLICATION_FLOWS.md`
- `docs/architecture/SERVICE_BOUNDARIES.md`
- `docs/architecture/DATABASE_SCHEMA.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/architecture/DOCUMENT_VAULT.md`
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`
- ADRs 0001–0003

## Privacy/security

- `PRIVACY.md`
- `SECURITY.md`
- `TERMS.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/privacy/DATA_LIFECYCLE.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/security/THREAT_MODEL.md`
- `docs/security/LOGGING_PRIVACY.md`
- `docs/security/DEPENDENCY_RISK_REGISTER.md`

## Design/accessibility/localization

- `docs/design/DESIGN_SYSTEM.md`
- `docs/design/ACCESSIBILITY.md`
- `docs/design/LOCALIZATION.md`
- `docs/design/STORE_ASSETS.md`

## Development/maintenance

- `docs/setup/DEVELOPMENT.md`
- `docs/setup/PLATFORM_SETUP.md`
- `docs/setup/TROUBLESHOOTING.md`
- `docs/setup/MAINTAINER_OPERATIONS.md`
- `CONTRIBUTING.md`
- `CODE_OF_CONDUCT.md`

## Testing

- `docs/testing/TESTING_GUIDE.md`
- `docs/testing/TEST_PLAN.md`
- `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`

## Release

- `docs/releases/RELEASE_PROCESS.md`
- `docs/releases/RELEASE_CHECKLIST.md`
- `docs/releases/QUALITY_GATE.md`
- `docs/releases/MANUAL_TEST_MATRIX.md`
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md`
- `docs/releases/SECURITY_RELEASE_REVIEW.md`
- `docs/releases/RELEASE_EVIDENCE.md`
- `docs/releases/RELEASE_NOTES_TEMPLATE.md`
- `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`
- `docs/releases/NEXT_STEPS.md`
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`
- `docs/releases/PHASE8_VERIFICATION_EVIDENCE.md`
- `docs/releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md`
- BMC highlight/release check documents.

## Governance/navigation/history

- `docs/README.md`
- `docs/DOCUMENTATION_STANDARDS.md`
- `docs/history/what_changed_full_through_phase8.md`
- `README.md`
- `PROJECT_STATUS.md`
- `CHANGELOG.md`
- `DECISIONS.md`
- `what_changed.md`

---

# Open SQLite dependency risk remains unchanged

Tracked advisory:

`GHSA-2m69-gcr7-jv3q`

Current dependency path:

SQLitePCLRaw native `2.1.11` through the existing sqlite-net-pcl chain.

The exact `NuGetAuditSuppress` entry is not remediation.

Authoritative files:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

The documentation pass does not claim this risk is fixed.

---

# Production release blockers remain real

Complete documentation does **not** complete the public production release.

Still required before final `1.0.0` promotion:

1. Complete Android manual device/emulator matrix.
2. Complete Windows manual matrix.
3. Complete iOS/iPadOS manual matrix.
4. Complete Mac Catalyst manual matrix.
5. Manually verify notification permission denied/granted behavior.
6. Manually verify real reminder delivery limitations.
7. Manually verify Android exact/inexact alarm, battery optimization, reboot, time, and time-zone behavior.
8. Manually verify document import/open/export/delete.
9. Manually verify calendar export.
10. Manually verify encrypted backup creation and clean-install restore.
11. Manually verify wrong-password/tamper behavior in packaged target workflows where applicable.
12. Manually verify app-lock cold-start behavior.
13. Complete screen-reader checks.
14. Complete large-text/text-scaling checks.
15. Complete keyboard/focus checks on desktop targets.
16. Complete contrast/theme/reduced-motion checks.
17. Review current Apple App Store policy for the external voluntary project-support link.
18. Review current Google Play policy for the external voluntary project-support link.
19. Configure signing identities/credentials outside Git.
20. Build and inspect signed release artifacts.
21. Complete store screenshots using fictional data.
22. Complete store descriptions/privacy/data-safety disclosures.
23. Resolve or make an explicit acceptable final release decision for the SQLitePCLRaw advisory.
24. Run `CareNest Release Evidence` for the exact promoted production commit.
25. Update final version/build metadata/release notes/status.
26. Create the final tag/GitHub release only after all applicable blockers are complete.

No item above is marked complete merely because its procedure is now fully documented.

---

# Deferred future scope remains unchanged

Still outside current v1 scope:

- cloud synchronization;
- remote caregiver collaboration;
- required accounts;
- server-side storage;
- mobile-number authentication;
- automatic remote sharing;
- hidden analytics/telemetry;
- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- medication-interaction claims;
- clinical risk scoring.

Any future networked feature requires a new consent/authentication/key/privacy/threat/deletion/export/store architecture review.

---

# Current repository interpretation

- Complete CareNest `1.0.0-rc.1` runtime/test source remains on `main`.
- Exact verified runtime/test SHA remains `c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`.
- PR #30 remains the latest exact source verification baseline.
- Automated baseline remains 141/141 core tests plus four platform Release builds, CodeQL, and Dependency Audit green.
- The 2026-08-12 continuation is a comprehensive documentation-only pass.
- Root README and docs hub expose the complete documentation set.
- Project status/changelog/decisions reflect the documentation completion.
- The complete pre-documentation Phase 0–8 handoff is preserved on current `main` at `docs/history/what_changed_full_through_phase8.md` using the exact previous Git blob.
- The open SQLitePCLRaw advisory remains open and accurately documented.
- Manual/store/signing/final Release Evidence tasks remain blocking.
- No cloud/account/clinical-decision functionality was added by this pass.
