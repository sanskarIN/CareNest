# CareNest Production Validation Evidence Standard

**Release line:** `1.0.0-rc.1`  
**Current automated evidence authority:** `AUTOMATED_BASELINE.md`

This stable document defines the minimum evidence quality required for CareNest production validation. It does not claim that any real-device, signing, notarization, store or publication task has been completed.

Do not pin a moving accepted source SHA, workflow run ID or test total here. Read current exact-source automation from `AUTOMATED_BASELINE.md`.

## 1. Evidence principles

Every production validation record must be:

- attributable to one exact CareNest source SHA and, when available, one immutable `v*` tag;
- attributable to one exact package filename or package directory and SHA-256 where package evidence applies;
- attributable to a specific platform, OS version and device/model or virtualized environment;
- dated with local date/time and time zone;
- explicit about whether data is fictional/synthetic;
- explicit about the result state;
- reproducible from documented steps without undocumented operator memory;
- free of private signing material, passwords, PINs, real health information and document contents;
- accompanied by issue/PR references when a failure causes source changes.

Unknown, stale or unperformed work must never be recorded as passed.

## 2. Required source/package identity

Record all applicable fields:

- release version and build number;
- exact Git commit SHA;
- immutable source tag, if approved;
- package filename/path;
- package SHA-256;
- package-evidence JSON filename/path;
- package-evidence payload SHA-256;
- application/package identifier;
- signing/notarization/store-managed provenance safe to publish;
- validation operator name or role;
- validation date/time/time zone.

For production package evidence, use `PACKAGE_EVIDENCE_TOOLING.md` and keep generated evidence outside the package payload.

## 3. Data-safety rule

Use fictional or synthetic data for validation.

Do not place in repository/public evidence:

- real medical records;
- real prescriptions/lab reports/diagnosis information;
- passwords;
- app-lock PINs;
- backup passwords;
- private signing keys;
- keystores/certificate private material;
- access tokens;
- recovery codes;
- service-account private credentials;
- real sensitive document contents.

Screenshots and logs must be reviewed before retention. Redact sensitive user-entered text, document names/contents, contacts and similar values.

## 4. Status vocabulary

Use only these result states:

- `PASS` — the exact recorded step was performed and met its expected result;
- `FAIL` — the step was performed and did not meet its expected result;
- `BLOCKED` — the step could not be performed because a dependency, permission, device, account, environment or signing requirement was unavailable;
- `N/A` — the step is genuinely not applicable and the reason is documented;
- `NOT RUN` — the step has not yet been performed.

Do not convert `BLOCKED`, `N/A` or `NOT RUN` into `PASS` merely to complete a checklist. `N/A` requires a defensible reason.

## 5. Failure handling

When a production validation step fails:

1. preserve non-sensitive reproduction details;
2. record the exact package/source/device boundary;
3. determine whether the cause is source, packaging, signing, platform policy, store metadata or environment;
4. open/reference a GitHub issue when repository source work is required;
5. implement the smallest correct fix with regression coverage where applicable;
6. run the required exact-source automated matrix again for verification-relevant changes;
7. rebuild the affected package from the corrected exact source;
8. repeat the failed validation;
9. never move an already published/rejected immutable production tag to another commit.

## 6. Reminder evidence

Reminder validation must distinguish:

- notification permission state;
- exact/inexact scheduling capability where applicable;
- battery/background restrictions;
- app running/background/terminated/force-stopped state;
- reboot/restart lifecycle;
- clock/time-zone/DST changes;
- create/edit/delete cancellation and replacement behavior;
- Taken/Skipped/Delayed/Missed/Snooze behavior;
- known operating-system delivery limitations.

Do not claim guaranteed notification delivery from a successful build, simulator compilation or one successful notification.

## 7. Existing-data compatibility evidence

For packaged migration/upgrade checks record:

- originating CareNest version/build/source where known;
- target version/build/source;
- database schema version before/after;
- integrity-check result;
- representative entity counts/observations before/after;
- reminder rebuild/reconciliation observations;
- encrypted-document accessibility after upgrade;
- backup create/inspect/restore observations.

Never manufacture a current backup and label it historical evidence.

If a genuine historical artifact violates a current security/resource boundary, record a compatibility/security decision rather than silently weakening the boundary.

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

Submission records must distinguish policy review, metadata completion, submission, review, rejection, approval and publication as separate states.

## 10. Required record set before production promotion

As applicable to the intended platform set, complete release-specific records based on:

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

Canonical templates must remain visibly unperformed. Create release-specific copies for actual evidence.

## 11. Automated evidence boundary

Current exact-source automated evidence is owned by:

`AUTOMATED_BASELINE.md`

A previous green source does not prove a later verification-relevant source. A newer source replaces the accepted automated boundary only after its required exact-source matrix has actually completed successfully.

Do not predict a test total from source inspection.

## 12. Production promotion rule

CareNest remains a release candidate until all applicable automated, production validation, compatibility, accessibility, signing, final-package, policy, submission and publication blockers have real evidence.

A green automated matrix is required but not sufficient for production approval.
