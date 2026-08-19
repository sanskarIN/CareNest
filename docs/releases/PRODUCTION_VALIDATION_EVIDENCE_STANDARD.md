# CareNest Production Validation Evidence Standard

**Release line:** `1.0.0-rc.1`  
**Accepted automated source before production validation:** `30ee6c265104c64ec5a1a4013f592f7f058750e8`  
**Accepted automated result:** **370/370 core tests passed**

This document defines the minimum evidence quality required for the remaining production-only CareNest validation. It does not claim that any real-device, signing, notarization, store or publication task has been completed.

## 1. Evidence principles

Every production validation record must be:

- attributable to one exact CareNest source SHA and, when available, one immutable `v*` tag;
- attributable to one exact package filename or package directory and SHA-256;
- attributable to a specific platform, OS version and device/model or virtualized environment;
- dated with the local date/time and time zone of the validation;
- explicit about whether data is fictional/synthetic;
- explicit about pass, fail, blocked or not-applicable status;
- reproducible from documented steps without relying on undocumented operator memory;
- free of private signing material, passwords, PINs, real health information or document contents;
- accompanied by issue/PR references when a failure causes source changes.

Unknown, stale or unperformed work must never be recorded as passed.

## 2. Required source/package identity

Record all applicable fields:

- release version and build number;
- exact Git commit SHA;
- immutable source tag, if one has been approved;
- package filename/path;
- package SHA-256;
- package-evidence JSON filename/path;
- package-evidence payload SHA-256;
- application/package identifier;
- signing/notarization/store-managed provenance that is safe to publish;
- validation operator name or role;
- validation date/time/time zone.

For production stage package evidence, use `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` and keep generated evidence outside the package payload.

## 3. Data-safety rule

Use fictional or synthetic data for validation. Do not place real medical records, prescriptions, lab reports, diagnosis information, passwords, app-lock PINs, private keys or signing secrets in repository evidence.

Screenshots and logs must be reviewed before retention. Redact user-entered health text, document names, document contents, contacts and other sensitive values.

## 4. Status vocabulary

Use only these result states:

- `PASS` — the exact recorded step was performed and met its expected result;
- `FAIL` — the step was performed and did not meet its expected result;
- `BLOCKED` — the step could not be performed because a dependency, permission, device, account or signing requirement was unavailable;
- `N/A` — the step is genuinely not applicable and the reason is documented;
- `NOT RUN` — the step has not yet been performed.

Do not convert `BLOCKED`, `N/A` or `NOT RUN` into `PASS` for checklist completion.

## 5. Failure handling

When a production validation step fails:

1. preserve non-sensitive reproduction details;
2. record the exact package/source/device boundary;
3. determine whether the cause is source, packaging, signing, platform policy, store metadata or environment;
4. open or reference a GitHub issue when source/repository work is required;
5. implement the smallest correct fix with regression coverage where applicable;
6. run the required exact-source automated matrix again for verification-relevant changes;
7. repeat the failed production validation against a replacement package;
8. never move an already published/rejected immutable production tag to another commit.

## 6. Reminder evidence

Reminder validation must distinguish:

- notification permission state;
- exact/inexact scheduling capability where applicable;
- battery/background restrictions;
- app running/background/terminated/force-stopped state;
- reboot/restart lifecycle;
- clock/time-zone/DST changes;
- create/edit/delete cancellation and replacement behavior;
- Taken/Skipped/Delayed/Missed/Snooze state behavior;
- known operating-system delivery limitations.

Do not claim guaranteed notification delivery from a successful build or simulator compilation.

## 7. Existing-data compatibility evidence

For packaged migration/upgrade checks, record:

- originating CareNest version/build/source where known;
- target version/build/source;
- database schema version before/after;
- integrity-check result;
- representative entity counts before/after;
- reminder rebuild/reconciliation observations;
- encrypted-document accessibility after upgrade;
- backup create/inspect/restore observations.

Never manufacture a current backup and label it historical evidence.

## 8. Accessibility evidence

Accessibility records must identify the actual assistive technology or input method used, for example VoiceOver, TalkBack, Narrator, keyboard-only navigation or large-text/display scaling.

Source semantics and automated tests are supporting evidence only; they do not replace actual assistive-technology validation.

## 9. Signing and store evidence

Repository evidence may contain safe public identifiers/fingerprints, timestamps and package hashes. It must never contain:

- private signing keys;
- keystore passwords;
- certificate private material;
- Apple private keys;
- store-account recovery codes;
- access tokens;
- service-account private credentials.

Submission records must distinguish policy review, submission, approval and publication as separate states.

## 10. Required record set before production promotion

As applicable to the intended platform set, complete records from:

- `templates/ANDROID_DEVICE_VALIDATION_RECORD.md`;
- `templates/WINDOWS_VALIDATION_RECORD.md`;
- `templates/IOS_DEVICE_VALIDATION_RECORD.md`;
- `templates/MACCATALYST_VALIDATION_RECORD.md`;
- `templates/ACCESSIBILITY_VALIDATION_RECORD.md`;
- `templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`;
- `templates/SIGNING_PROVENANCE_RECORD.md`;
- `templates/STORE_SUBMISSION_RECORD.md`;
- `templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`.

The templates are evidence containers, not evidence by themselves.

## 11. Production promotion rule

CareNest remains a release candidate until all applicable production validation, compatibility, accessibility, signing, final-package, policy and submission blockers have real evidence.

A green automated matrix is required but not sufficient for production approval.
