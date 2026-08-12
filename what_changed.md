# what_changed.md

## CareNest complete continuation handoff — 2026-08-12

This file is the active detailed handoff for the current CareNest continuation. It records the full documentation pass requested after the source-complete `1.0.0-rc.1` implementation and the Phase 8 reminder-integrity hardening.

The immediately preceding full implementation/hardening handoff remains preserved unchanged in Git history at:

`https://github.com/sanskarIN/CareNest/blob/4571cf7e7149b09102690459c437b3ca844b7efa/what_changed.md`

That historical handoff contains the complete Phase 0–8 implementation record, PR #24–#30 verification history, SQLite/WAL/app-lock/reminder/privacy hardening history, and previous commit-level details. It is intentionally preserved rather than discarded. This active file continues from that complete historical state and records the documentation completion work pushed directly to `main`.

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

Commit:

`82d08305c1e0bc6bb01cbf3f4030c573be94e5bc` — `docs: add complete CareNest user guide`

Created:

`docs/USER_GUIDE.md`

Covers onboarding, local-first model, profiles, medicines, schedules, DST, notification limitations, reminder states, quiet hours/follow-ups, medication log, appointments, document vault, reports, backup/restore, app lock, diagnostics, accessibility, privacy, external support, help, and release-candidate limitations.

## 2. Complete feature reference

Commit:

`ec22d5ac34af3e76f5e2ee9c6b0fe53869029a7d` — `docs: add complete CareNest feature reference`

Created:

`docs/FEATURE_REFERENCE.md`

Maps every major product feature to its behavior and medical/privacy/security boundary, including explicit reminder ownership/UTC/DST/snooze semantics, reports, backup, app lock, documents, diagnostics, funding, and deferred scope.

## 3. Application-flow reference

Commit:

`0ec2ec1f5a63e7f88fa6f5a3db072518b5a4a1b4` — `docs: document CareNest application flows`

Created:

`docs/architecture/APPLICATION_FLOWS.md`

Documents startup, onboarding, profile, medicine, schedule, reminder materialization, reminder state, stock, appointment, document, report, backup, restore, app-lock, diagnostics, external support, and failure-handling flows.

## 4. Backup/restore architecture

Commit:

`efcb0ec56bf191f5c2bb4079de032d64dec76e47` — `docs: document encrypted backup and restore architecture`

Created:

`docs/architecture/BACKUP_AND_RESTORE.md`

Documents WAL checkpoint/snapshot behavior, PBKDF2-HMAC-SHA256 + AES-GCM protected backup model, document-key portability, wrong-password/tamper rejection, restore validation, rollback/failure safety, privacy boundaries, and manual release requirements.

## 5. Storage/export/deletion model

Commit:

`7afee682b4b6ee4a044c5e08fbac575c175b03bb` — `docs: document CareNest storage export and deletion model`

Created:

`docs/architecture/DATA_STORAGE_AND_EXPORT.md`

Documents SQLite, encrypted documents, secure secrets, platform notification state, logging, JSON/CSV/PDF/document/calendar exports, profile/document deletion, reset, backup retention, OS copies, screenshots, and contributor data-lifecycle requirements.

## 6. Service/infrastructure boundaries

Commit:

`3a3c67afc1e4db244cf73d70b6a231e90e638eb7` — `docs: document application service and infrastructure boundaries`

Created:

`docs/architecture/SERVICE_BOUNDARIES.md`

Documents Shared/Domain/Application/Infrastructure/App responsibilities, repository/notification/navigation/error/time/secure-storage boundaries, async/cancellation expectations, and rules for adding new services.

## 7. Cross-platform setup

Commit:

`c4d13ab594b54e6441ce605a12e25a07ef9d0a65` — `docs: add cross-platform CareNest setup guide`

Created:

`docs/setup/PLATFORM_SETUP.md`

Documents Android, Windows, iOS, and Mac Catalyst toolchain/build commands, `CareNestTargetFramework`, platform manual checks, signing-secret rules, and the requested maintainer Git identity.

## 8. Accessibility specification

Commit:

`8fd52c6eba814e49aa6aafa80bfe802d89d7f0de` — `docs: add CareNest accessibility specification`

Created:

`docs/design/ACCESSIBILITY.md`

Documents text scaling, TalkBack/VoiceOver/Narrator, semantic labels, keyboard/focus, target size, contrast, color-independent status, reduced motion, themes, errors, reminder safety text, destructive actions, document/report/support surfaces, and required manual evidence.

## 9. Complete privacy model

Commit:

`34c24addc7d872292779252a8141f2b2e596ecef` — `docs: add complete CareNest privacy model`

Created:

`docs/privacy/PRIVACY_MODEL.md`

Documents local-first data categories, no-account/no-backend boundary, SQLite/document/app-lock/notification/logging privacy, explicit outbound boundaries, deletion/archive distinctions, OS-level copies, no hidden telemetry, store disclosure rules, and future network-feature requirements.

## 10. Security architecture reference

Commit:

`92d3c31307f4c027d9af51784130fc906bed7117` — `docs: add CareNest security architecture reference`

Created:

`docs/security/SECURITY_MODEL.md`

Documents trust boundaries, SQLite/document/backup/app-lock protections, notification/reminder integrity, logging/global exception controls, local-first network policy, secret management, dependency security, source hygiene, export/backup/device threats, release review, and future network security requirements.

## 11. Complete testing guide

Commit:

`e1fda936cbf743eb7c459a4c01db83ff215b77d0` — `docs: add complete CareNest testing guide`

Created:

`docs/testing/TESTING_GUIDE.md`

Documents unit/integration/UI-contract roles, current verified 74/13/54 test counts, formatting, reminder ownership/UTC/property/DST coverage, SQLite/WAL/backup/document/app-lock/report/policy tests, CodeQL, Dependency Audit, exact-head verification, manual testing, bug-fix testing, synthetic-data rules, and future testing roadmap.

## 12. End-to-end release process

Commit:

`7715226d7d7ad681ae295b435b42db124689bb09` — `docs: add end-to-end CareNest release process`

Created:

`docs/releases/RELEASE_PROCESS.md`

Defines the full release process from scope freeze and dependency/security review through preflight, exact-head verification, device matrix, backup/accessibility qualification, store-policy review, signing, store metadata, security review, Release Evidence, versioning, checksums, tagging, post-release monitoring, and hotfixes.

## 13. Maintainer operations

Commit:

`bf83c0bca2e22c83ca6621d006bac614009e0d5d` — `docs: add CareNest maintainer operations guide`

Created:

`docs/setup/MAINTAINER_OPERATIONS.md`

Documents Git identity, main/verification branch rules, commit discipline, source/schema/dependency/security/logging/app-lock/document/backup/documentation changes, CI/exact-head verification, failure handling, release evidence, manual/store/signing operations, and final release rules.

## 14. Glossary

Commit:

`e2649fa5235184d8f2a24f6e2cab3740f6be9916` — `docs: add CareNest glossary`

Created:

`docs/GLOSSARY.md`

Defines CareNest-specific terms including app lock, as-needed, CareNestTargetFramework, clinical inference, cycle schedule, DST gap/overlap, local-first, occurrence key, opaque medicine text, reminder planner/coordinator, release evidence/gate, snooze, stock estimate, UTC contract, verification marker, WAL, and whole-database encryption.

## 15. Documentation hub

Commit:

`54ffc0d3e87e01c59f2b407a55ad5d1b827d2e46` — `docs: add complete CareNest documentation index`

Created:

`docs/README.md`

Established the central documentation navigation hub.

## 16. Expanded architecture overview

Commit:

`3b1d2b8cb831a2e179a1ff4c8e262b086faf42bc` — `docs: expand CareNest architecture reference`

Expanded:

`docs/architecture/ARCHITECTURE.md`

Converted the previous concise overview into a complete solution/context/layer/reminder/time-zone/persistence/WAL/document/backup/app-lock/privacy/logging/navigation/accessibility/build/CI/dependency/future architecture reference.

## 17. Expanded database schema

Commit:

`5769c8e218ab5e2fb6b3d6880734934475ef0813` — `docs: expand CareNest database schema reference`

Expanded:

`docs/architecture/DATABASE_SCHEMA.md`

Documents entities, ownership relationships, schema versions 1–5, deletion/cascade expectations, WAL/busy timeout/snapshot interaction, migration rules, and future compatibility requirements.

## 18. Expanded design system

Commit:

`6899e2516dd4bf6ec480b647f3446d8be50f05f4` — `docs: expand CareNest design system`

Expanded:

`docs/design/DESIGN_SYSTEM.md`

Documents design principles, spacing/radii, typography, color/status, themes/motion, cards/forms/schedule/reminder/destructive/empty/error/notification/safety surfaces, responsive desktop/mobile behavior, branding/watermark/funding, accessibility/localization/store assets, and design review checklist.

## 19. Expanded development setup

Commit:

`270904d89e39c256b2213a2c15c016928c57358c` — `docs: expand CareNest development setup`

Expanded:

`docs/setup/DEVELOPMENT.md`

Documents clone/toolchain/Git identity, project structure, workload/restore/build commands, `CareNestTargetFramework`, platform targets, tests/formatting/preflight, local data, dependencies/analyzers/architecture rules, documentation, secret exclusion, troubleshooting, and exact-head verification.

## 20. Expanded troubleshooting

Commit:

`7db8500a60e4d09236f46a6e40ec5ae9280ae56b` — `docs: expand CareNest troubleshooting guide`

Expanded:

`docs/setup/TROUBLESHOOTING.md`

Documents user/developer troubleshooting for notifications, as-needed/state suppression, DST/time zone, snooze, Android, Windows, workloads, target-framework propagation, Apple Xcode, analyzers, formatting, reminder tests, SQLite/WAL, dependency audit, restore, document vault, app lock, support links, exports, accessibility/theme issues, and safe public bug reports.

## 21. Expanded data lifecycle

Commit:

`6ccc5c9734737a3f95552dd58fc949ba553d5d73` — `docs: expand CareNest data lifecycle documentation`

Expanded:

`docs/privacy/DATA_LIFECYCLE.md`

Documents collection, local storage/processing, reminder materialization, notifications, diagnostics, export/share/calendar/external links, backup/restore, archive/delete/reset/uninstall, OS-level copies/screenshots, development data, and future networked lifecycle requirements.

## 22. Expanded localization architecture

Commit:

`445db7c644ff15fd80044f26db6cf793d505d2fd` — `docs: expand CareNest localization architecture`

Expanded:

`docs/design/LOCALIZATION.md`

Documents English-first resource architecture, what should/should-not be localized, opaque medicine text, safety-critical translations, layout/RTL/accessibility, dates/numbers/time zones, reports/notifications, branding, translation workflow, fallback/testing, and current language limitation.

## 23. Expanded store asset guidance

Commit:

`f4781b084dc58d9937decb5c7f220aede008f9ca` — `docs: expand CareNest store asset guidance`

Expanded:

`docs/design/STORE_ASSETS.md`

Documents app/splash/store graphic/screenshot creation, fictional-data requirements, accurate reminder/medical/privacy/app-lock/funding claims, time-sensitive store-policy review, permissions/privacy forms, localization/accessibility, creator/support metadata, and final asset checklist.

## 24. Expanded contribution guide

Commit:

`8fb0f326f7708602de02feadf36f14c6236d2573` — `docs: expand CareNest contributing guide`

Expanded:

`CONTRIBUTING.md`

Documents medical/local-first boundaries, secrets/real-data rules, architecture/reminder/schema/backup/app-lock/logging rules, setup/formatting/Git identity/commit/PR/test/CI/dependency/documentation/accessibility/store/security-reporting requirements.

## 25. Notification/platform behavior

Commit:

`5c27558582441f67b38a7bf34f9f986b97c298e6` — `docs: document notification and platform behavior`

Created:

`docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`

Separates deterministic CareNest occurrences from OS delivery and documents permission timing, generic notification privacy, quiet hours/follow-ups/snooze/rebuild, Android exact/inexact/battery/boot/time-zone/force-stop behavior, Apple local notifications, Windows fallback, DST, overdue reconciliation, failure logging, test reminders, release evidence, and troubleshooting.

## 26. Encrypted document vault

Commit:

`8c676427aba448eb6a46ff630ccf8a95b9ea9a41` — `docs: document encrypted CareNest document vault`

Created:

`docs/architecture/DOCUMENT_VAULT.md`

Documents metadata/payload separation, AES-GCM/key storage, import/picker/metadata/tags/folders/profile-photo/open/export/delete/backup/logging/security/manual-test behavior and future local-only improvements.

## 27. Reports and exports

Commit:

`26c8e8f97735af3044c198d4e2bcce8074ca7741` — `docs: document CareNest reports and exports`

Created:

`docs/REPORTS_AND_EXPORTS.md`

Documents JSON profile export, PDF summaries, all CSV report categories, calendar/document export, backup-vs-export distinction, plaintext/export privacy, formatting/localization/disclaimer/error/cancellation/share/store/test/support rules.

## 28. Documentation standards

Commit:

`b4121d2580e9cad6e8c72cde357c4caa1eb44986` — `docs: define CareNest documentation standards`

Created:

`docs/DOCUMENTATION_STANDARDS.md`

Defines canonical hub, documentation layers, medical/reminder/local-first/encryption/dependency/verification/manual/store/funding wording rules, synthetic-data requirements, command/link standards, feature/schema/security review checklists, and handoff requirements.

## 29. Documentation completeness checklist

Commit:

`1bacfbced6dbd1916188ab85d99cb4d5e088d085` — `docs: add CareNest documentation completeness checklist`

Created:

`docs/releases/DOCUMENTATION_COMPLETENESS_CHECKLIST.md`

Inventories project, user, legal/privacy/security, architecture, reminder, design/accessibility/localization, developer, testing, release, and governance documentation while explicitly leaving real manual/store/signing/dependency/release tasks unchecked.

## 30. Final documentation-hub navigation

Commit:

`66bcc85cc486339631bcfef36421489db0a414cb` — `docs: complete CareNest documentation hub navigation`

Updated:

`docs/README.md`

Added the notification, document-vault, report/export, documentation-standards, and completeness-checklist references and reorganized navigation for users, developers, architecture, reminders/platforms, privacy, security, backup/portability, design, testing, release, and history.

## 31. Root README documentation navigation

Commit:

`b7cfc300423547b14c9ed5e052438cb5d986a2b6` — `docs: link complete CareNest documentation from README`

Updated:

`README.md`

Adds the full documentation entry point, key subsystem references, current PR #30 runtime/test baseline, reminder-integrity description, privacy/security/release links, and preserves the voluntary project-support boundary.

## 32. Project status documentation package

Commit:

`789df205fd3387dc58e04a7b596564b4034ff455` — `docs: record complete CareNest documentation package`

Updated:

`PROJECT_STATUS.md`

Records the complete documentation package and a dedicated documentation-status section while keeping all manual/accessibility/store/signing/dependency/Release-Evidence blockers real and unchecked.

## 33. Documentation evidence decision

Commit:

`72a44ffcfd2496ae2356641007372b5b1579502d` — `docs: record CareNest documentation evidence policy`

Updated:

`DECISIONS.md`

Adds decision 28: documentation is implementation evidence, not permission to overclaim; documentation-only commits do not create a new runtime verification baseline and manual/store/dependency claims require real evidence.

## 34. Changelog documentation pass

Commit:

`7ac7ee8bfd1e49ea4462449e0a8415240fee834d` — `docs: record complete CareNest documentation pass`

Updated:

`CHANGELOG.md`

Adds a dedicated 2026-08-12 documentation section covering the entire documentation package while retaining PR #30 as the exact runtime/test baseline.

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

The full preceding handoff at `4571cf7...` remains the historical detailed Phase 0–8 record referenced at the top of this file.

---

# Documentation-only source comparison

After commit `7ac7ee8bfd1e49ea4462449e0a8415240fee834d`, the repository was compared from exact verified runtime/test source SHA:

`c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`

to documentation head:

`7ac7ee8bfd1e49ea4462449e0a8415240fee834d`

Comparison result:

- status: ahead;
- 43 commits after the verified runtime/test source;
- 30 changed files;
- every changed path was Markdown documentation (`.md`);
- no `src/` runtime C#/XAML/resource file changed;
- no test source/project changed;
- no `.csproj`, solution, package, workflow, build script, platform source, signing/configuration, or runtime resource changed.

Changed documentation paths in that compare included:

- `CHANGELOG.md`;
- `CONTRIBUTING.md`;
- `DECISIONS.md`;
- `PROJECT_STATUS.md`;
- `README.md`;
- `docs/DOCUMENTATION_STANDARDS.md`;
- `docs/FEATURE_REFERENCE.md`;
- `docs/GLOSSARY.md`;
- `docs/README.md`;
- `docs/REPORTS_AND_EXPORTS.md`;
- `docs/USER_GUIDE.md`;
- architecture documentation files;
- design/accessibility/localization/store documentation files;
- privacy/security documentation files;
- release documentation files;
- setup/maintainer/troubleshooting files;
- testing documentation files;
- the prior `what_changed.md` handoff.

Therefore the PR #30 runtime/test evidence remains the authoritative automated source baseline while the current documentation head advances independently.

This active `what_changed.md` update is itself Markdown-only. A final compare after this commit is used to confirm the same property through the final `main` head.

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

The connected GitHub contents API used for these commits does not expose arbitrary author/committer-email parameters. These API-created commits therefore use the authenticated GitHub identity. The repository does not falsely claim that the connector forced `sanskarin@outlook.in` into those commit objects.

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
- Root README and docs hub now expose the complete documentation set.
- Project status/changelog/decisions reflect the documentation completion.
- The historical Phase 0–8 detailed handoff remains permanently available at commit `4571cf7e7149b09102690459c437b3ca844b7efa`.
- The open SQLitePCLRaw advisory remains open and accurately documented.
- Manual/store/signing/final Release Evidence tasks remain blocking.
- No cloud/account/clinical-decision functionality was added by this pass.
