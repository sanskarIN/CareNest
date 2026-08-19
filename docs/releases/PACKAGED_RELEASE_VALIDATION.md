# CareNest Packaged Release Validation

**Release line:** `1.0.0-rc.1`  
**Accepted automated baseline before current backup hardening:** `b6eecae66f74bd72bcb20d93508355542f9f3442`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`

This runbook covers release-candidate behavior that hosted source tests cannot fully prove: real package/install paths, existing SQLite data, encrypted documents/backups, real notification delivery, accessibility, external-commerce payload isolation, signing and final package provenance.

Use fictional/synthetic data only.

## 1. Automated source boundary

Accepted exact-source automated baseline before the current backup resource-hardening branch:

`b6eecae66f74bd72bcb20d93508355542f9f3442`

That exact source passed:

- 122/122 unit tests;
- 39/39 integration tests;
- 194/194 UI/source-policy tests;
- **355/355 core tests**;
- Android/Windows/iOS simulator/Mac Catalyst Release builds;
- all four store-candidate configurations;
- Store Inspection Artifacts;
- CodeQL;
- unsuppressed Dependency Audit.

Authoritative record:

`docs/releases/FINAL_CANDIDATE_VERIFICATION_20260818.md`

Any later verification-relevant source, including backup security hardening, requires its own exact-source workflow evidence before it replaces the accepted baseline above.

This source automation is still not packaged production evidence.

## 2. Validation principles

- never use real health records in public/shared evidence;
- record exact source SHA/tag and package checksum;
- distinguish source/build success from installed-package behavior;
- distinguish dependency security from data compatibility;
- distinguish simulator builds from real-device notification behavior;
- keep production signing secrets outside Git;
- verify repository marketing does not leak into the app package;
- generate structured package evidence for final production artifacts;
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
- application does not silently generate an unrelated replacement key for existing ciphertext.

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
- current default archive resource ceilings are enforced before manifest parsing/extraction;
- a newly created backup is checked against the same resource/topology limits before encryption;
- restored encrypted documents remain usable;
- reminder/platform derived state rebuilds correctly.

For current defaults, explicitly verify boundary behavior around:

- 1 MiB manifest maximum;
- 1 GiB SQLite database maximum;
- 512 MiB per encrypted document maximum;
- 2 GiB total uncompressed payload maximum;
- 5,000-document maximum.

Use deliberately small synthetic fixtures when testing rejection paths where possible; do not create multi-gigabyte public fixtures merely to prove a boundary already covered by parameterized source tests. Real packaged compatibility should instead confirm normal representative backups remain comfortably within the configured ceilings.

## 7. Historical encrypted compatibility

Where genuine previous CareNest encrypted document/backup fixtures exist:

- record exact producing version/source if known;
- keep canonical bytes unchanged;
- verify current candidate reads/restores according to documented compatibility;
- record result/checksum;
- if a genuine historical backup exceeds a newly enforced resource ceiling, treat that as a compatibility finding requiring explicit design review rather than weakening the limit silently.

Do not generate a current fixture and label it historical.

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

## 13. Repository storefront versus application-package validation

Official repository storefront:

**https://ramsandesh.gumroad.com**

Current invariant: the distributed CareNest application source/package contains no external Gumroad or Buy Me a Coffee destination/card/command/promotional artwork.

For internal and final candidates:

- run/equivalently apply the forbidden-marker payload scanner;
- confirm `buymeacoffee.com/sanskarIN` is absent;
- confirm `ramsandesh.gumroad.com` is absent;
- inspect About/runtime UI for absence of Gumroad/BMC promotion;
- verify the repository-only Gumroad badge is not packaged;
- verify intentional repository/creator/business/support/privacy/terms/security application links remain available as documented;
- verify no health feature changes according to Gumroad purchase/funding state;
- verify store screenshots/listing do not imply in-app Gumroad/BMC behavior.

There is no current per-package external-commerce build toggle.

## 14. Payload scanner behavior to preserve

`build/scripts/verify-store-safe-payload.py` defaults to:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

Candidate-package validation must preserve:

- UTF-8 scanning;
- UTF-16 LE scanning;
- UTF-16 BE scanning;
- regular-file inspection;
- ZIP/AAB entry inspection;
- fail-closed errors for unreadable/missing payloads.

## 15. Structured package evidence tooling

Use:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

Source-controlled entry points:

- `build/scripts/create-package-evidence.py`;
- `build/scripts/create-package-evidence.sh`;
- `build/scripts/create-package-evidence.ps1`.

For internal inspection artifacts, `--stage inspection` can record package hashes and store-safe scan output without claiming production signing.

For each final production artifact, use `--stage production` and verify the tool requires:

- immutable `v*` source tag;
- tag SHA equals recorded source SHA;
- checked-out HEAD equals recorded source SHA;
- clean tracked workspace;
- non-secret real signing/notarization/store-managed provenance description;
- successful store-safe scan;
- output outside the package payload.

Retain the generated JSON with the final release record.

CareNest CI includes syntax validation and a synthetic self-test for the package-evidence tool, but that self-test does not replace running the tool against the real final production package.

## 16. Final production signing

Outside Git configure actual production signing for intended channels.

Record only safe public provenance/fingerprints/identifiers in repository evidence; never commit private keys, keystores, certificate passwords or provisioning secrets.

## 17. Final signed-package inspection

For every signed/notarized/store candidate record:

- exact source SHA/tag;
- version/build;
- application/package identity;
- filename;
- SHA-256;
- signing/notarization/store provenance;
- package evidence JSON;
- package evidence payload SHA-256;
- Buy Me a Coffee forbidden-marker result;
- Gumroad forbidden-marker result;
- install/launch smoke result;
- About/legal/support-contact check;
- relevant platform manual matrix result.

The final signed package—not only an unsigned internal candidate—must be checked.

## 18. Store-policy/metadata validation

Preliminary review:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

At submission time review current store rules for the exact candidate.

Validate:

- organizer/non-clinical wording;
- notification limitation wording;
- privacy/data-safety declarations;
- permission/capability descriptions;
- screenshots with fictional data;
- support/privacy/terms/security URLs;
- application ID/version/build;
- external-commerce policy applicable at submission time;
- live Google Play Health apps declaration/Data safety where applicable;
- current Apple privacy/store metadata where applicable;
- current Microsoft/Partner Center privacy/store metadata where applicable;
- no repository-only Gumroad/BMC surface in the submitted app unless a future explicitly reviewed policy change approves it.

The dated preliminary review does not replace the submission-day review.

## 19. Exact production tag

Only after applicable package/manual/accessibility/signing/store preparation is complete, create the approved immutable production `v*` tag and require all configured tagged gates, including:

- CareNest CI;
- CodeQL;
- unsuppressed Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

Do not move a failed production tag to a different source revision.

## 20. Evidence record template

For every validated package record:

```text
Source SHA/tag:
CareNest version/build:
Platform:
OS/device:
Artifact filename:
SHA-256:
Package evidence JSON:
Package evidence payload SHA-256:
Signing/notarization provenance:
SQLite upgrade result:
Encrypted document result:
Backup result:
Backup resource-limit compatibility result:
Reminder/notification result:
Accessibility result:
Buy Me a Coffee marker scan:
Gumroad marker scan:
Installed app Gumroad/BMC surface check:
Store-policy review date/source:
Live store declaration/metadata result:
Overall result:
Notes:
```

## 21. Failure handling

If validation exposes a defect:

1. retain the failing evidence safely;
2. fix the smallest correct source boundary;
3. add/update regression coverage;
4. run the full applicable exact-source automated matrix;
5. rebuild the package from that exact source;
6. repeat the affected package/manual checks;
7. regenerate final package evidence JSON from the replacement package;
8. update current evidence only after the replacement result is known.

Do not suppress a valid package/test/security failure merely to complete a checklist.
