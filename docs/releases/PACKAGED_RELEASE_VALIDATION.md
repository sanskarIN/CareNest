# Packaged Release Validation Runbook

## Purpose

This runbook converts CareNest's remaining manual production blockers into a repeatable evidence process. It supplements `MANUAL_TEST_MATRIX.md`; it does not mark any device, store-policy, signing, accessibility, encrypted-data, or package-distribution check complete by itself.

Use fictional test data only.

## 1. Freeze the candidate source

Record before building:

- exact `main` commit SHA;
- intended release version;
- intended target framework;
- operating system and toolchain used to build;
- `CareNestShowFundingLink` value;
- whether the package is debug, release, signed test, TestFlight/internal, or production candidate.

Do not reuse a failed production tag. Do not move an approved production tag to a different commit.

## 2. Run source preflight

Bash example:

```bash
CARENEST_TARGET=net10.0-android \
CARENEST_SHOW_FUNDING_LINK=false \
./build/scripts/release-preflight.sh
```

PowerShell example:

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
$env:CARENEST_SHOW_FUNDING_LINK = 'false'
./build/scripts/release-preflight.ps1
```

The preflight must remain green before creating the candidate package. A green preflight does not replace the manual matrix.

## 3. Confirm package identity

For every candidate, capture:

- application title: `CareNest`;
- application identifier: `com.sanskar.carenest` unless an explicitly reviewed store identity requires a documented change;
- display version;
- build/version code;
- target OS/minimum OS;
- package filename;
- exact source commit;
- signing identity fingerprint or store-managed signing provenance without committing secrets.

If any identity differs from the source-controlled release plan, stop and explain the difference before distribution.

## 4. Compute artifact checksum

For directly handled artifacts, record SHA-256.

Bash:

```bash
sha256sum path/to/artifact
```

PowerShell:

```powershell
Get-FileHash path/to/artifact -Algorithm SHA256
```

Store the checksum in release evidence. Never place private signing keys, keystores, certificate private keys, backup passwords, or app-lock PINs in the repository.

## 5. Fresh-install smoke test

On every intended target:

1. Install the exact candidate package.
2. Launch CareNest without a CareNest account or backend.
3. Complete onboarding with synthetic profile data.
4. Confirm the medical limitations and backup responsibility remain visible.
5. Confirm notification permission is not requested merely by onboarding.
6. Create an explicit reminder-capable feature and verify the permission surface appears at the intended time.
7. Verify core navigation: Home, Profiles, Medicines, Medication log, Appointments, Documents, Reports, Settings, About.
8. Verify no workflow implies diagnosis, dosage calculation, treatment recommendation, emergency-service replacement, or guaranteed notification delivery.

Record results in `MANUAL_TEST_MATRIX.md`.

## 6. Store-support-link package test

Follow `STORE_BUILD_POLICY.md`.

If `CareNestShowFundingLink=true`:

- About shows the complete voluntary project-support card;
- the destination is `https://buymeacoffee.com/sanskarIN`;
- opening it requires explicit user action;
- the copy states that support unlocks no medical/health feature.

If `CareNestShowFundingLink=false`:

- About hides the support image, button, URL, and support explanation;
- the funding command is non-executable in the compiled store-safe configuration;
- repository, creator, business/support email, privacy, terms, security, and notices remain available;
- no organizer feature differs from the enabled build.

Record the chosen property value next to the package checksum.

## 7. Existing-data upgrade and SQLite compatibility

For the first production candidate after the SQLite native/provider remediation, use synthetic existing data that exercises:

- profiles;
- medicines;
- schedules and schedule times;
- reminder occurrences;
- medication logs;
- appointments;
- stock adjustments;
- documents and tags;
- app settings;
- backup metadata where applicable.

Upgrade/install the candidate using the platform's intended real distribution path where possible.

Verify:

- database opens successfully;
- records remain readable and editable;
- SQLite integrity validation passes;
- reminder rebuild/reconciliation succeeds;
- no duplicate or stale platform reminder is silently stranded;
- no schema version is silently rewritten outside intended migration behavior.

A clean NuGet audit does not substitute for this package compatibility evidence.

## 8. Encrypted-document compatibility

Using synthetic files only:

1. Open/export an existing encrypted `.cndoc` payload from a pre-remediation/earlier compatible build when a canonical fixture exists.
2. Import a new document with the candidate package.
3. Export it and compare the plaintext only in the temporary controlled test location.
4. Delete the document and verify CareNest-owned encrypted storage cleanup.
5. Verify missing/corrupt key behavior fails closed rather than silently replacing the key.
6. Verify failed export does not leave an unintended partial plaintext file under CareNest ownership.

Do not upload decrypted fixtures to GitHub release evidence.

## 9. Backup compatibility and tamper checks

With synthetic data:

- create a current encrypted backup;
- inspect metadata through supported app behavior;
- restore on a clean installation;
- verify wrong password is rejected;
- verify tampered backup is rejected;
- verify restored encrypted documents remain usable;
- verify no destructive partial restore is reported as success;
- verify a canonical historical compatible backup when real historical fixture bytes are available.

Record only non-sensitive evidence and checksums of synthetic fixtures where appropriate.

## 10. Reminder lifecycle on real platform scheduling

At minimum verify:

- permission denied and granted states;
- schedule creation;
- schedule change with stale-request cleanup;
- Taken/Skipped/Delayed/Missed cancellation-first ordering from observed platform behavior;
- Snooze cancellation and replacement;
- future snooze crossing the original due time;
- overdue snooze evaluated from its snooze due time;
- medicine/profile delete cleanup;
- appointment reminder create/edit/delete reconciliation;
- restart/reopen recovery;
- time-zone change recovery;
- Android reboot/exact-alarm/battery-optimization behavior where applicable;
- Windows limitation messaging when guaranteed background delivery is unavailable.

A platform failure that leaves persisted state and OS-request state contradictory blocks production promotion.

## 11. Accessibility pass

On representative targets verify:

- screen-reader names and reading order;
- 200% or representative large text scaling;
- destructive confirmation readability;
- keyboard navigation/focus on desktop-capable targets;
- light/dark contrast;
- color-independent status meaning;
- reduced-motion behavior;
- actionable, privacy-safe errors.

Automated XAML contracts do not replace assistive-technology testing.

## 12. Store-policy review

Before submission, review current rules for:

- health/medical claims and disclaimers;
- local health-data disclosure requirements;
- notification/reminder claims;
- privacy/data-safety declarations;
- external voluntary project-support links;
- screenshots and metadata accuracy.

Record review date and the store policy source. If the external support link is not clearly allowed, package with `CareNestShowFundingLink=false` until the policy question is resolved.

## 13. Signing and secret handling

Production signing material must remain outside Git.

Record only safe provenance such as:

- certificate/public fingerprint;
- signing service identity;
- keystore alias without secret material when appropriate;
- notarization/store submission identifier;
- signing timestamp;
- source commit and package checksum.

Never commit:

- private keys;
- signing passwords;
- keystore passwords;
- provisioning secrets;
- API tokens;
- backup passwords;
- app-lock PINs.

## 14. Final release evidence gate

Before a production `v*` tag is approved, the release record must identify:

- exact source commit;
- completed applicable manual matrix rows;
- package identity/version;
- package checksums;
- signing provenance;
- current store-policy review;
- `CareNestShowFundingLink` value per store package;
- packaged SQLite/encrypted-data compatibility results;
- accessibility results;
- exact production tag;
- tagged CareNest CI result;
- tagged CodeQL result;
- tagged unsuppressed Dependency Audit result;
- tagged Store Package Configuration result;
- tagged Store Inspection Artifacts result;
- tagged Release Gate result;
- tagged Release Evidence result.

Do not call CareNest bug-free. Production promotion means the defined automated and manual gates are satisfied for the approved source/package boundary.

## 15. Automated internal store inspection artifacts

CareNest provides `.github/workflows/store-inspection-artifacts.yml` to create reproducible **internal inspection** artifacts with `CareNestShowFundingLink=false`.

The workflow intentionally separates this source-side evidence from production signing/distribution:

- Android: exactly one verified-unsigned `.aab` is staged. A MAUI `*-Signed.aab` debug-signed companion is explicitly excluded, and the selected AAB is rejected if JAR-signature metadata is present.
- Windows: a self-contained `win-x64` unpackaged bundle is archived as ZIP using `RuntimeIdentifierOverride=win-x64` and `WindowsPackageType=None`.
- iOS: a simulator `.app` bundle is archived for inspection only.
- Mac Catalyst: an unsigned `.app` bundle is archived with package creation/code signing disabled.
- every artifact includes SHA-256 checksum material and `provenance.txt`;
- pull-request provenance records the exact PR head/source SHA separately from GitHub's temporary PR merge/event SHA;
- artifact names use the exact source SHA;
- uploads fail if expected files are missing;
- artifacts are explicitly marked `artifact_purpose=internal-inspection-only` and `store_submission_ready=false`.

The latest exact runtime exercise is recorded in `STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md` (PR #61).

These artifacts are useful for source/configuration inspection and reproducibility, but they **do not** satisfy:

- production signing/provisioning;
- real-device installation;
- store-delivery testing;
- packaged existing-data compatibility;
- accessibility certification;
- final store submission/approval.

For an actual release, create and inspect the signed candidate separately, retain safe signing provenance/checksums, then complete the manual matrix and submission-time policy review before production promotion.
