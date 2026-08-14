# CareNest Release Process

This document defines the end-to-end release process from a green release-candidate source tree to a public signed release. It intentionally keeps automated evidence separate from manual/device/store/signing approval.

## Release principle

A CareNest release is not approved merely because source compiles.

Public promotion requires the exact candidate commit to satisfy:

- source/format/test/build gates;
- CodeQL;
- Dependency Audit;
- dependency-risk review;
- manual device behavior;
- packaged existing-database/encrypted-data compatibility where applicable;
- accessibility checks;
- notification limitation checks;
- security review;
- current store-policy review;
- signing/package identity;
- privacy/data-safety disclosure review;
- final release evidence/provenance.

## Current state

Current product target: `1.0.0-rc.1`.

The authoritative 2026-08-14 bug-audit baseline is PR #54, which completed:

- 122 unit tests;
- 39 integration tests;
- 100 UI-contract/policy tests;
- 261 total core automated tests;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- CodeQL;
- unsuppressed Dependency Audit.

Later release-engineering workflow/script hardening requires a new exact-source verification before the current `main` head is promoted. Historical PR #54 remains evidence for the earlier runtime/test graph; it is not permission to skip verification after workflow/test/configuration changes.

## Stage 1 — freeze intended scope

Before final release work:

1. decide the version to ship;
2. freeze product scope;
3. stop adding unrelated features;
4. review `PROJECT_STATUS.md` and `docs/releases/NEXT_STEPS.md`;
5. identify every open Priority 0 blocker;
6. confirm the SQLite source remediation remains present and the former audit suppression has not returned;
7. identify packaged existing-data/encrypted-data compatibility evidence still required before production promotion.

## Stage 2 — dependency/security review

Review:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`;
- `docs/security/THREAT_MODEL.md`;
- `docs/security/SECURITY_MODEL.md`;
- `docs/security/LOGGING_PRIVACY.md`;
- `SECURITY.md`.

Do not call `NuGetAuditSuppress` remediation and do not restore the former SQLite advisory suppression merely because manual compatibility evidence remains incomplete.

If the SQLite dependency/provider changes:

- rerun migration tests;
- rerun snapshot/backup tests;
- rerun all core tests;
- rebuild Android/Windows/iOS/Mac Catalyst;
- rerun CodeQL/Dependency Audit;
- re-evaluate storage/backup behavior;
- perform representative packaged existing-database/encrypted-data compatibility checks;
- update risk register and migration plan.

## Stage 3 — run development preflight

On a fully provisioned host run the repository preflight script:

```bash
build/scripts/release-preflight.sh
```

or:

```powershell
./build/scripts/release-preflight.ps1
```

The preflight is blocking for:

- source hygiene;
- platform-neutral formatting;
- core Release builds;
- all core test suites;
- unsuppressed NuGet audit for the platform-neutral/test graph;
- optional MAUI target audit before an optional target build when `CARENEST_TARGET` is provided.

The preflight must not ignore dependency-audit failures.

Record toolchain information:

- OS;
- `dotnet --info`;
- `dotnet workload list`;
- Xcode version for Apple builds;
- Android SDK/tooling details;
- Windows toolchain details.

## Stage 4 — exact-head automated verification

For any source/config/test/workflow change after the previous verified baseline, use the marker-only verification protocol.

1. finish intended source on `main`;
2. record exact source SHA;
3. create temporary branch from exact SHA;
4. add one marker under `build/verification/`;
5. open PR to `main`;
6. confirm PR contains only marker beyond base;
7. require green:
   - platform-neutral formatting;
   - unit tests;
   - integration tests;
   - UI-contract/policy tests;
   - Android Release;
   - Windows Release;
   - iOS simulator Release;
   - Mac Catalyst Release;
   - CodeQL;
   - Dependency Audit;
8. close marker PR without merge.

If any gate exposes a defect, fix the real source on `main`, close the stale verification PR, and create a new exact-head verification.

Never merge the marker as production source.

## Stage 5 — manual supported-platform matrix

Use `docs/releases/MANUAL_TEST_MATRIX.md`.

Required categories include:

### Android

- fresh install;
- onboarding;
- permission denied/granted;
- reminder behavior;
- cancellation-first reminder actions and recovery;
- exact/inexact alarm diagnostics;
- battery optimization;
- reboot/time/time-zone recovery;
- document operations;
- backup/restore;
- app lock;
- accessibility.

### Windows

- fresh install;
- navigation/resizing;
- keyboard behavior;
- reminder/fallback limitation messaging;
- cancellation/replacement behavior for in-process reminder timers;
- document operations;
- backup/restore;
- app lock;
- themes/accessibility.

### iOS/iPadOS

- fresh install;
- notification permission/delivery behavior;
- cancellation-first reminder action behavior;
- document share/picker;
- backup/restore;
- app lock;
- Dynamic Type;
- VoiceOver;
- themes.

### Mac Catalyst

- fresh install;
- notifications;
- cancellation/replacement behavior;
- resizing;
- keyboard/focus;
- file operations;
- backup/restore;
- app lock;
- accessibility/themes.

Manual rows must record actual evidence; do not pre-check them based on CI.

## Stage 6 — packaged SQLite/data compatibility qualification

After any SQLite native/provider dependency change and before production promotion:

1. prepare a representative packaged build with the intended dependency graph;
2. start from a controlled installation/database containing fictional pre-remediation RC1 data;
3. upgrade/open with the new build;
4. verify profiles, medicines, schedules, reminder occurrences, medication logs, appointments, documents, stock, tags, and settings;
5. verify SQLite integrity/readability;
6. verify reminder rebuild/reconciliation;
7. verify existing encrypted documents still decrypt through the unchanged key path;
8. verify a new encrypted backup/restore round-trip;
9. when a canonical synthetic pre-remediation backup exists, restore it too;
10. record device/build/source evidence.

A clean NuGet audit does not substitute for this compatibility qualification.

## Stage 7 — backup/restore qualification

On release packaging candidates:

1. create realistic synthetic local records;
2. import synthetic documents;
3. create encrypted backup;
4. uninstall/reset or use clean installation;
5. restore using correct password;
6. confirm records/documents;
7. confirm reminder rebuild/runtime state;
8. test wrong password;
9. test corrupted/tampered backup where practical;
10. inspect logs for secret/private-data exposure.

## Stage 8 — accessibility qualification

Use `docs/design/ACCESSIBILITY.md`.

Verify representative:

- screen readers;
- large text;
- keyboard/focus;
- contrast;
- reduced motion;
- light/dark/system themes;
- destructive confirmation readability;
- medical/reminder limitation text availability.

## Stage 9 — store-policy review

Before submission, review current Apple App Store / Google Play requirements relevant to:

- health/medical organizational apps;
- privacy/data safety;
- notification permissions;
- external voluntary project-support link;
- payment/funding policies;
- account deletion requirements only if accounts are ever added;
- platform signing/capabilities.

CareNest's Buy Me a Coffee link is voluntary project support, not purchase of medical functionality.

If a distribution channel disallows the in-app link, remove/disable that in-app action for that channel rather than misrepresenting it as a medical purchase.

Repository funding links can remain where permitted.

## Stage 10 — signing/package identity

Signing material must stay outside Git.

### Android

- package/application ID verified;
- release keystore stored securely;
- signing config supplied through secure environment/CI secrets;
- build signed AAB/APK as intended;
- inspect permissions/capabilities.

### Apple

- bundle ID verified;
- certificates/provisioning profiles configured securely;
- entitlements reviewed;
- archive/sign for App Store/TestFlight or intended channel;
- Mac Catalyst notarization/signing as applicable.

### Windows

- package/publisher identity verified;
- signing certificate/private key kept outside repository;
- build intended MSIX/package/sideload artifact.

## Stage 11 — store metadata/privacy disclosures

Use `docs/releases/STORE_SUBMISSION_CHECKLIST.md`.

Prepare:

- final app name/version/build;
- descriptions matching implemented behavior;
- screenshots using fictional data;
- icons/feature graphics;
- privacy policy URL;
- terms/security/support links;
- local-first data-safety answers;
- notification/health-category declarations as applicable;
- support contact.

Do not claim analytics collection if none exists.

Do not claim whole-database encryption if the current implementation does not provide it.

Do not claim guaranteed reminder delivery.

## Stage 12 — security release review

Complete `docs/releases/SECURITY_RELEASE_REVIEW.md` for the exact candidate.

Record:

- version;
- commit SHA;
- reviewer/date;
- CI run;
- CodeQL run;
- Dependency Audit run;
- Release Evidence run;
- SQLite dependency/source-remediation decision;
- packaged existing-data compatibility decision;
- open security blockers;
- approval decision.

## Stage 13 — Release Evidence workflow

Run `CareNest Release Evidence` for the exact commit intended for release.

Evidence captures:

- source/ref/run identity;
- tracked-file manifest and SHA-256 source checksums;
- pre/post tracked-workspace status;
- toolchain data;
- TRX results for all three core suites;
- transitive dependency inventories for platform-neutral source and test projects;
- checksums for evidence files.

The workflow attempts all core evidence components independently, uploads the artifact even when one of those components failed, then fails the run if any required evidence component did not succeed. Failed evidence artifacts are retained for diagnosis rather than hidden by fail-fast execution.

Automated release evidence is provenance, not a substitute for manual/store/security approval.

## Stage 14 — version metadata

Only after the candidate is actually selected:

- set final display version/build numbers consistently;
- update `CHANGELOG.md`;
- generate final release notes from `docs/releases/RELEASE_NOTES_TEMPLATE.md`;
- update `PROJECT_STATUS.md`;
- update `what_changed.md`;
- record exact verification/evidence IDs.

Avoid changing runtime/source/workflow/test configuration after final verification. If any such source changes, re-run exact-head verification.

## Stage 15 — artifact checksums/archive

For directly distributed artifacts:

- generate SHA-256 checksums;
- record exact source commit;
- record signing identity/channel;
- archive provenance/evidence;
- retain secrets outside source artifacts/logs.

## Stage 16 — create exact approved tag and run tag gates

Create the final `v*` tag only after applicable blocking manual/signing/store checklist items are complete and the candidate commit is approved for tagging.

The tag must point to the exact approved commit.

A `v*` tag automatically triggers the exact tagged source through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Release Gate;
- CareNest Release Evidence.

Do not publish/promote the GitHub/store release until all required tag-triggered workflows are successful.

If a tag-triggered gate fails:

1. preserve its evidence;
2. do not call the tag a successful production release;
3. correct source/configuration on a new commit;
4. re-run exact-source verification/manual checks as applicable;
5. use the corrected approved commit/tag for release.

Do not move or silently reuse a failed release tag as if the underlying source had not changed.

## Stage 17 — GitHub/store release promotion

After the exact tag gates are green:

- confirm signed artifacts came from the exact tagged commit;
- attach/check artifact checksums and provenance;
- publish the GitHub release;
- submit/promote store packages through the intended channels;
- update final release status/evidence documents.

## Stage 18 — post-release monitoring

CareNest v1 has no hidden telemetry feedback loop.

Use explicit channels:

- GitHub Issues;
- support email;
- security-reporting process.

Triage reports by:

- version;
- platform;
- OS;
- reproduction steps;
- time-zone/notification capability where relevant;
- privacy-safe diagnostics.

Do not ask users to publicly post health records/backups/secrets.

## Hotfix process

For a production defect:

1. reproduce safely;
2. add regression test;
3. implement smallest correct fix;
4. update docs if behavior changed;
5. exact-head verify full matrix;
6. manually re-test affected target flow;
7. rerun security/dependency checks;
8. update changelog/release notes;
9. build/sign from exact verified source;
10. create the approved patch tag;
11. require all tag-triggered CI/CodeQL/Dependency Audit/Release Gate/Release Evidence workflows green;
12. publish patch version.

## Release blockers rule

Any required gate that is failed, unknown, stale, or not actually performed blocks final promotion unless explicitly documented as non-applicable with a defensible reason.

CareNest should never be marketed as bug-free, medically authoritative, or guaranteed to deliver reminders merely because automated CI is green.
