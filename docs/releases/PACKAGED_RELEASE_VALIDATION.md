# CareNest Packaged Release Validation

**Release line:** `1.0.0-rc.1`

This runbook covers release-candidate behavior that hosted source tests cannot fully prove: real package/install paths, existing SQLite data, encrypted documents/backups, real notification delivery, accessibility, signing and final package provenance.

Use fictional/synthetic data only.

## 1. Current source baseline

Verified executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

PR #74 head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Current source evidence:

- 331/331 core tests;
- Android/Windows/iOS simulator/Mac Catalyst Release builds green;
- all four store-candidate configurations green;
- Store Inspection Artifacts green;
- CodeQL green;
- unsuppressed Dependency Audit green.

This is not packaged production evidence.

## 2. Validation principles

- never use real health records in public/shared validation evidence;
- record exact source SHA/tag and package checksum;
- distinguish source/build success from installed-package behavior;
- distinguish dependency security from data compatibility;
- distinguish simulator builds from real-device notification behavior;
- keep production signing secrets outside Git;
- do not mark a row complete without actual evidence.

## 3. Representative prior data set

Create a synthetic earlier-RC data set containing, as applicable:

- multiple profiles;
- emergency contacts;
- medicines in different lifecycle states;
- schedule kinds/times/time zones;
- reminder occurrences including handled/snoozed state;
- medication-log entries;
- appointments;
- stock adjustments;
- encrypted documents/tags;
- settings/app-lock state where safe to simulate;
- encrypted backup files produced by genuine prior builds where available.

Do not manufacture new data and call it historical evidence.

## 4. Packaged SQLite upgrade validation

For each intended production target/package path:

1. install representative earlier RC package/data;
2. confirm baseline data is readable before upgrade;
3. install/upgrade intended candidate through realistic platform path;
4. launch candidate;
5. confirm database opens;
6. run/record integrity validation;
7. confirm schema version/migrations;
8. verify representative records remain readable;
9. verify editable records remain editable;
10. verify relationship cleanup/cascades still behave;
11. rebuild/reconcile reminders;
12. confirm no duplicate/stale OS requests;
13. record source/package/device/OS/checksum/result evidence.

A clean NuGet audit cannot substitute for these steps.

## 5. Current encrypted document lifecycle

With synthetic documents verify:

- import creates encrypted application-owned payload;
- metadata/tag/folder behavior works;
- open works with correct key state;
- explicit export creates expected plaintext/portable copy;
- failed export does not leave unintended CareNest-owned partial plaintext output;
- delete removes intended metadata/encrypted payload;
- missing/corrupt required key fails closed;
- application does not silently generate unrelated replacement key for existing ciphertext.

## 6. Current backup lifecycle

Verify:

- create password-encrypted backup;
- inspect/recognize current version;
- restore into existing install where intended;
- restore into clean install;
- wrong password rejected;
- tampered backup rejected;
- truncated backup rejected;
- trailing data rejected;
- invalid/duplicate/unexpected archive topology rejected;
- restored encrypted documents remain usable;
- reminder/platform derived state rebuilds correctly.

## 7. Historical encrypted compatibility

Where genuine previous CareNest encrypted document/backup fixtures exist:

- record exact producing version/source if known;
- keep canonical bytes unchanged;
- verify current candidate reads/restores according to documented compatibility;
- record result/checksum.

Do not generate a current fixture and label it as historical.

## 8. Android package/device validation

Validate representative supported Android targets for:

- fresh install/onboarding;
- upgrade path;
- notification permission denied/granted;
- actual medicine/appointment notification delivery;
- exact/inexact alarm diagnostics;
- battery optimization/vendor restrictions;
- force-stop limitation/recovery;
- reboot/restart/time-zone/DST recovery;
- create/edit/delete reminder lifecycle;
- Taken/Skipped/Delayed/Missed cancellation-first behavior;
- snooze cancellation/replacement/effective due time;
- stale request cleanup;
- document picker/share;
- backup/restore;
- app lock;
- theme/accessibility.

## 9. Windows package validation

Validate:

- install/launch/update/uninstall;
- core CRUD/navigation;
- running-app reminder behavior;
- closed-app limitation behavior/messaging;
- same-ID timer replacement/cancellation;
- handled actions/snooze/reconciliation;
- restart/recovery;
- documents/share;
- backup/restore;
- app lock;
- keyboard/focus;
- themes/accessibility;
- existing-data upgrade.

## 10. iPhone/iPad validation

Use a signed/provisioned real-device candidate for:

- install/upgrade;
- permission denied/granted;
- real notification delivery;
- reminder actions/snooze/reconciliation;
- lifecycle/restart/time-zone behavior;
- documents/share;
- backup/restore;
- app lock;
- Dynamic Type;
- VoiceOver;
- notification-preview privacy;
- existing-data upgrade where applicable.

Simulator compilation is not a substitute.

## 11. Mac Catalyst validation

Validate:

- install/launch/update;
- notification permission/delivery;
- reminder actions/reconciliation;
- restart/lifecycle;
- files/share;
- backup/restore;
- app lock;
- keyboard/focus;
- theme/contrast/accessibility;
- existing-data upgrade;
- signed/notarized behavior when available.

## 12. Accessibility package validation

On representative final candidate packages verify:

- screen-reader names/order;
- large text/scaling;
- keyboard/focus where applicable;
- system/light/dark contrast;
- color-independent status/error meaning;
- reduced motion;
- destructive confirmation readability;
- medical/privacy/reminder limitation accessibility.

## 13. Application funding/package validation

Current invariant: no external Buy Me a Coffee destination/card/command/artwork in distributed application source/package.

For internal/final candidates:

- run/equivalently apply the forbidden-marker payload scan;
- inspect About UI for absence of BMC funding card/action;
- verify repository/creator/business/support/privacy/terms/security links remain available;
- verify no health feature changes based on repository funding;
- verify store screenshots/listing do not imply removed in-app funding behavior.

There is no current per-package funding-link build toggle.

## 14. Final production signing

Outside Git configure actual production signing for intended channels.

Record only safe public provenance/fingerprints/identifiers in repository evidence; never commit private keys, keystores, certificate passwords or provisioning secrets.

## 15. Final signed-package inspection

For every signed/notarized/store candidate record:

- exact source SHA/tag;
- version/build;
- application/package identity;
- filename;
- SHA-256;
- signing/notarization/store provenance;
- forbidden-marker scan result;
- install/launch smoke result;
- About/legal/support-contact check;
- relevant platform manual matrix result.

## 16. Store-policy/metadata validation

At submission time review current store rules for the exact candidate.

Validate:

- organizer/non-clinical wording;
- notification limitation wording;
- privacy/data-safety declarations;
- permission/capability descriptions;
- screenshots with fictional data;
- support/privacy/terms/security URLs;
- application ID/version/build;
- no removed in-app funding surface in screenshots/listing.

## 17. Exact production tag

Only after applicable package/manual/accessibility/signing/store preparation is complete, create the approved immutable production `v*` tag and require:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

Do not move a failed/rejected production tag to different source.

## 18. Evidence template

For each manual/package run capture:

```text
Date/time:
Tester:
Source SHA/tag:
Version/build:
Package filename:
Package SHA-256:
Signing/notarization provenance:
Platform/device/OS:
Install/upgrade path:
Test scenario:
Expected:
Actual:
Result: PASS / FAIL / N/A
Notes/issue:
```

Use `N/A` only with a defensible reason.

## 19. Current remaining status

The source-controlled RC1 scope is heavily automated-verified, but the packaged/manual rows above remain open until actual evidence exists.

Use `docs/releases/NEXT_STEPS.md` as the authoritative remaining-work checklist.