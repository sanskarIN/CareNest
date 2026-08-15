# CareNest final store-payload and bug-audit verification — 2026-08-15

## Scope

This record captures the final automated/source/package-inspection baseline after resolving the Windows packaged funding-marker defect discovered during the 2026-08-15 release-hardening continuation.

CareNest remains `1.0.0-rc.1`. This evidence does **not** claim production publication, production signing, real-device validation, store approval, or absolute absence of all possible bugs.

## Authoritative executable source

Merged `main` source SHA:

`9ec7b4e7d2150d9cc50be19f30464080318b16e8`

This merge contains PR #67, `Fix final store-safe funding payload isolation`.

PR #67 final source head before merge:

`0fa552ca824f034ce7426513a7d3e50eaa0ef7aa`

PR #67 passed the full configured automated matrix before merge.

## Root cause fixed

Earlier store-safe configurations correctly evaluated the external funding surface as disabled, but Windows inspection still found `buymeacoffee.com/sanskarIN` inside `CareNest.App.dll`.

The actual root cause was the MAUI image resource:

`src/CareNest.App/Resources/Images/buy_me_a_coffee_carenest.svg`

The SVG itself contained the full external funding destination in its accessibility/text content. Windows MAUI resource processing embedded that resource content into the managed application payload. This made code-level compile flags insufficient to remove the marker from the final DLL.

The final design therefore removes the external funding destination from the application runtime/package entirely:

- no Buy Me a Coffee destination under `src/CareNest.App`;
- no in-app funding command;
- no in-app funding card;
- no funding-policy compile units;
- no packaged funding artwork;
- no obsolete funding build toggle;
- voluntary project funding remains repository-documentation-only;
- artifact byte scanning remains a defense-in-depth gate for Android, Windows and Apple inspection outputs.

Source-policy tests recursively guard the app tree against reintroducing the forbidden destination or deleted funding surface.

## Failure-driven chronology

The failed verification checkpoints are intentionally retained as evidence that the final fix was driven by actual package inspection rather than by assumptions:

- PR #64 exposed stale source-policy contracts after the initial payload-scanner hardening;
- PR #65 passed 324/324 core tests and Android payload inspection but Windows package staging correctly failed because `CareNest.App.dll` still contained the funding marker;
- PR #66 passed 325/325 core tests and proved nested funding properties were false, but Windows package staging still failed, ruling out the simple nested-property-loss theory;
- PR #67 reproduced the issue during intermediate physical-source/effective-property experiments, then identified the URL-bearing SVG resource as the root cause and removed the entire external funding surface from the app package;
- PR #68 independently reverified the exact merged `main` source after PR #67.

None of the marker-only verification PRs were merged.

## PR #67 pre-merge evidence

Final PR #67 source head:

`0fa552ca824f034ce7426513a7d3e50eaa0ef7aa`

- CareNest CI #717 / run `31880445293`: success;
- formatting: success;
- unit tests: 122 passed, 0 failed, 0 skipped;
- integration tests: 39 passed, 0 failed, 0 skipped;
- UI/source-policy tests: 164 passed, 0 failed, 0 skipped;
- total core tests: 325 passed, 0 failed, 0 skipped;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- Store Package Configuration #106 / run `31880445239`: Android, Windows, iOS simulator and Mac Catalyst store-candidate builds all success;
- Store Inspection Artifacts #40 / run `31880445403`: scanner self-test success; Android AAB, Windows self-contained bundle, iOS simulator app and Mac Catalyst app payload scans/staging/uploads success;
- CodeQL #717 / run `31880445284`: success;
- Dependency Audit #84 / run `31880445286`: success on platform-neutral and MAUI graphs.

PR #67 was then merged to `main` as:

`9ec7b4e7d2150d9cc50be19f30464080318b16e8`

## Authoritative exact merged-source verification — PR #68

PR:

`https://github.com/sanskarIN/CareNest/pull/68`

Frozen executable source/base SHA:

`9ec7b4e7d2150d9cc50be19f30464080318b16e8`

Verification marker SHA:

`c752815c311e7e443f1d71df8a9197cf706a14b6`

Marker path:

`build/verification/final-bug-audit-20260815.txt`

PR #68 contained exactly one marker file, 14 additions, 0 deletions, and was closed without merge.

### Automated test and build results

CareNest CI #719 / run `31880955724`:

- formatting: success;
- unit tests: **122 passed, 0 failed, 0 skipped**;
- integration tests: **39 passed, 0 failed, 0 skipped**;
- UI/source-policy tests: **164 passed, 0 failed, 0 skipped**;
- total core tests: **325 passed, 0 failed, 0 skipped**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success.

### Store-candidate configuration results

CareNest Store Package Configuration #108 / run `31880955723`:

- Android store-candidate Release build: success;
- Windows store-candidate Release build: success;
- iOS simulator store-candidate Release build: success;
- Mac Catalyst store-candidate Release build: success.

### Package-inspection results

CareNest Store Inspection Artifacts #41 / run `31880955734`:

- store-safe payload scanner self-test: success;
- Android unsigned AAB publish: success;
- Android forbidden-marker payload scan: success;
- Android checksum/provenance staging and artifact upload: success;
- Windows self-contained publish: success;
- Windows forbidden-marker payload scan: **success**;
- Windows checksum/provenance staging and artifact upload: success;
- iOS simulator inspection app build and payload scan: success;
- unsigned Mac Catalyst inspection app publish and payload scan: success;
- Apple checksum/provenance staging and artifact upload: success.

The Windows scan is the decisive regression proof for the bug that failed PR #65 and PR #66.

### Security/dependency results

- CodeQL #719 / run `31880955720`: success;
- unsuppressed Dependency Audit #85 / run `31880955731`: success on platform-neutral and MAUI dependency graphs.

No restored SQLite audit suppression was required.

## Final repository bug/error sweep

After the merge and exact marker verification:

- open GitHub issues: none found;
- indexed `TODO` / `FIXME` / `HACK` / `NotImplementedException`: none found;
- indexed `DateTime.Now`: none found;
- indexed `GetAwaiter`: none found;
- automated view-model contracts continue to reject `async void`, `Task.Run`, direct SQLite infrastructure access and direct network-client creation in concrete view models;
- reminder, backup/restore, document encryption, platform metadata, release workflow and payload-isolation regression contracts remain part of the 164-test UI/source-policy suite.

This supports the statement that **no known automated defect remains at executable source SHA `9ec7b4e7...` under the configured test/build/security/package-inspection matrix**. It is not a claim that software can be proven globally bug-free.

## Non-blocking warnings observed

MAUI/XamlC emitted existing `XC0022` / `XC0025` compiled-binding optimization warnings during some platform builds. They did not fail compilation or the configured quality gates and are treated as non-blocking performance/maintainability cleanup rather than a functional release defect.

## External/manual gates that remain open

The following must not be represented as completed without real evidence:

- production Apple/Google/Windows signing identities and secrets outside Git;
- production-signed package generation and provenance;
- real Android device/emulator reminder permission, delivery, restart, reboot, exact-alarm, battery-optimization and time-zone/DST behavior;
- real Windows reminder behavior and lifecycle matrix;
- real iPhone/iPad device behavior;
- real Mac Catalyst device behavior;
- assistive-technology accessibility testing;
- packaged SQLite existing-data upgrade/integrity/readability validation on actual release candidates;
- historical encrypted document/backup compatibility where genuine prior fixtures exist;
- final store screenshots/listing/privacy/data-safety metadata;
- submission-time Apple/Google policy review;
- exact approved production `v*` tag;
- tagged Release Gate / Release Evidence / CI / CodeQL / unsuppressed Dependency Audit;
- final signed-package checksums and publication.

Until those gates are completed, CareNest should be described as a source-complete, heavily automated-verified release candidate—not as production-published or absolutely bug-free.
