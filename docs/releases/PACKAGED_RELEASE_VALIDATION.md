# CareNest Packaged Release Validation

**Release line:** `1.0.0-rc.1`  
**Current automated evidence authority:** `docs/releases/AUTOMATED_BASELINE.md`  
**Production evidence standard:** `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`  
**Production evidence index:** `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`

This runbook covers release-candidate behavior that hosted source tests cannot fully prove: real package/install paths, existing SQLite data, encrypted documents/backups, real notification delivery, accessibility, external-commerce payload isolation, signing and final package provenance.

Use fictional/synthetic data only.

Do not pin a moving automated source SHA/test total in this stable runbook. Read the latest actually observed accepted source/result from `docs/releases/AUTOMATED_BASELINE.md`.

## 1. Automated source boundary

Before testing packages, confirm the intended package source has current exact-source automation in `docs/releases/AUTOMATED_BASELINE.md` or run a fresh exact-source matrix according to `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

Source automation is not packaged production evidence.

## 2. Validation principles

- use only fictional/synthetic application data in public/shared evidence;
- record exact source SHA/tag and package checksum;
- distinguish source/build success from installed-package behavior;
- distinguish dependency security from data compatibility;
- distinguish simulator builds from real-device notification behavior;
- keep production signing secrets outside Git;
- verify repository marketing does not enter the app package;
- generate structured package evidence for final production artifacts;
- use `PASS`, `FAIL`, `BLOCKED`, `N/A` and `NOT RUN` as defined by `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`;
- do not mark a row complete without actual evidence.

Use a release-specific copy of `docs/releases/templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md` for the canonical result record.

## 3. Representative prior data set

Create a synthetic earlier-candidate data set containing, as applicable:

- multiple profiles;
- emergency contacts;
- medicines in different lifecycle states;
- schedule kinds/times/time zones;
- reminder occurrences including handled/snoozed state;
- medication logs;
- appointments;
- stock adjustments;
- encrypted documents/tags;
- settings/app-lock state where safe to simulate;
- encrypted backups produced by genuine prior builds where genuine bytes safely exist.

Do not manufacture new data and call it historical evidence.

## 4. Packaged SQLite upgrade validation

For each intended production target/package path:

1. install representative earlier package/data;
2. confirm baseline data is readable before upgrade;
3. install/upgrade intended candidate through a realistic platform path;
4. launch candidate;
5. confirm database opens;
6. run/record integrity validation;
7. confirm schema version/migrations;
8. verify representative records remain readable/editable;
9. verify relationship cleanup/cascades;
10. rebuild/reconcile reminders;
11. confirm no duplicate/stale OS requests;
12. record source/package/device/OS/checksum/result evidence.

A clean dependency audit cannot substitute for these steps.

## 5. Encrypted document lifecycle

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

- password-encrypted backup creation;
- current-version inspection;
- restore into intended existing install;
- clean-install restore;
- wrong-password rejection;
- tamper rejection;
- truncation rejection;
- trailing-data rejection;
- invalid/duplicate/unexpected archive topology rejection;
- resource ceilings enforced before unsafe parsing/extraction;
- generated backup checked against current restore/resource rules before encryption;
- restored encrypted documents remain usable;
- reminder/platform-derived state rebuilds correctly.

Current default resource boundaries include:

- decrypted ZIP container: **2304 MiB** maximum;
- manifest: **1 MiB** maximum;
- SQLite database: **1 GiB** maximum;
- each encrypted document: **512 MiB** maximum;
- total uncompressed payload: **2 GiB** maximum;
- document count: **5,000** maximum;
- bounded archive-entry count;
- explicit directory-only entries rejected.

Use deliberately small synthetic fixtures for rejection behavior where possible. Do not create huge public fixtures merely to restate parameterized source-test coverage.

## 7. Historical encrypted compatibility

Where genuine previous CareNest encrypted document/backup fixtures exist:

- record producing version/source if known;
- keep canonical bytes unchanged;
- verify current candidate reads/restores according to documented compatibility;
- record checksum/result;
- if a genuine historical backup exceeds a current resource ceiling, treat it as an explicit compatibility/security decision rather than silently weakening the limit.

Do not generate a current fixture and label it historical.

## 8. Android package/device validation

Validate representative supported Android targets for:

- fresh install/onboarding and upgrade;
- notification permission denied/granted;
- actual medicine/appointment notification delivery;
- exact/inexact alarm diagnostics;
- battery/vendor restrictions;
- force-stop limitation/recovery;
- reboot/restart/time-zone/DST recovery;
- create/edit/delete reminder lifecycle;
- handled actions and snooze behavior;
- stale-request cleanup;
- document picker/share;
- backup/restore;
- app lock;
- theme/accessibility.

Use `docs/releases/templates/ANDROID_DEVICE_VALIDATION_RECORD.md`.

## 9. Windows package validation

Validate install/launch/update/uninstall, core flows, running/closed-app reminder behavior, timer replacement/cancellation, reminder actions/recovery, documents/share, backup/restore, app lock, keyboard/focus, themes/accessibility and existing-data upgrade.

Use `docs/releases/templates/WINDOWS_VALIDATION_RECORD.md`.

## 10. iPhone/iPad validation

Use a signed/provisioned real-device candidate for permission, real notification delivery, reminder actions/snooze/reconciliation, lifecycle/time-zone behavior, documents/share, backup/restore, app lock, Dynamic Type, VoiceOver, notification-preview privacy and existing-data upgrade where applicable.

Simulator compilation is not a substitute for real-device evidence.

Use `docs/releases/templates/IOS_DEVICE_VALIDATION_RECORD.md`.

## 11. Mac Catalyst validation

Validate installed behavior, notifications, reminder actions/reconciliation, lifecycle, files/share, backup/restore, app lock, keyboard/focus, theme/contrast/accessibility, existing-data upgrade and signed/notarized behavior when applicable.

Use `docs/releases/templates/MACCATALYST_VALIDATION_RECORD.md`.

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

Use `docs/releases/templates/ACCESSIBILITY_VALIDATION_RECORD.md`.

## 13. Repository storefront versus package validation

Official repository storefront:

`https://ramsandesh.gumroad.com`

Current invariant: the distributed CareNest application source/package contains no external Gumroad or Buy Me a Coffee destination/card/command/promotional artwork.

For internal and final candidates:

- scan/equivalently inspect the payload;
- confirm `buymeacoffee.com/sanskarIN` is absent;
- confirm `ramsandesh.gumroad.com` is absent;
- inspect installed runtime for absence of Gumroad/BMC promotion;
- verify repository-only Gumroad badge is not packaged;
- verify intended repository/creator/business/support/privacy/terms/security links remain as designed;
- verify no health feature changes according to purchase/funding state;
- verify store screenshots/listing do not imply in-app Gumroad/BMC behavior.

There is no current per-package external-commerce build toggle.

## 14. Payload scanner behavior

`build/scripts/verify-store-safe-payload.py` defaults to:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

Preserve UTF-8/UTF-16 scanning, regular-file inspection, ZIP/AAB entry inspection and fail-closed behavior for unreadable/missing payloads.

## 15. Structured package evidence

Use `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` and the source-controlled Python/Bash/PowerShell entry points.

For final production artifacts use `--stage production` and require:

- immutable `v*` source tag;
- tag SHA equals recorded source SHA;
- checked-out HEAD equals recorded source SHA;
- clean tracked workspace;
- non-secret real signing/notarization/store provenance;
- successful store-safe scan;
- evidence output outside the package payload.

Retain the generated JSON with the final release record. The tool does not sign packages or prove store approval.

## 16. Production signing

Configure actual production signing outside Git. Record only safe public provenance/fingerprints/identifiers in release evidence.

Never commit private keys, keystores, certificate passwords, provisioning secrets, service credentials, tokens or recovery codes.

Use `docs/releases/templates/SIGNING_PROVENANCE_RECORD.md`.

## 17. Final signed-package inspection

For every final package record:

- exact source SHA/tag;
- version/build/application identity;
- filename and SHA-256;
- signing/notarization/store provenance;
- package evidence JSON and payload SHA-256;
- BMC marker result;
- Gumroad marker result;
- installed smoke result;
- intended support/legal check;
- relevant platform manual result.

The final signed package—not only an unsigned internal artifact—must be checked.

## 18. Store-policy/metadata validation

Preliminary review:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

At submission time review current store rules for the exact candidate and complete applicable live health/privacy/data-safety/store metadata.

The dated preliminary review does not replace submission-day review.

Use `docs/releases/templates/STORE_SUBMISSION_RECORD.md`.

## 19. Exact production tag

Only after applicable package/manual/accessibility/signing/store preparation is complete, create the approved immutable production `v*` tag and require:

- CareNest CI;
- CodeQL;
- unsuppressed Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

Do not move a failed production tag to a different source revision.

## 20. Failure handling

If validation exposes a defect:

1. retain failing evidence safely;
2. fix the smallest correct source boundary;
3. add/update regression coverage;
4. run the full applicable exact-source automated matrix;
5. rebuild the package from that exact source;
6. repeat affected package/manual checks;
7. regenerate package evidence;
8. update current evidence only after the replacement result is known.

Do not suppress a valid package/test/security failure merely to complete a checklist.
