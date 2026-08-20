# CareNest Production Evidence Index

**Release line:** `1.0.0-rc.1`  
**Current automated evidence authority:** `AUTOMATED_BASELINE.md`

This stable index links the repository preparation for CareNest production evidence. None of the canonical templates below claims that the corresponding validation has already been performed.

Do not pin a moving accepted source SHA, workflow run ID or test total here. Read current exact-source automation from `AUTOMATED_BASELINE.md`.

## Evidence rules

Read first:

- `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`;
- `AUTOMATED_BASELINE.md`;
- `PACKAGED_RELEASE_VALIDATION.md`;
- `PACKAGE_EVIDENCE_TOOLING.md`;
- `RELEASE_CHECKLIST.md`;
- `RELEASE_EVIDENCE.md`;
- `NEXT_STEPS.md`.

## Platform validation templates

- Android: `templates/ANDROID_DEVICE_VALIDATION_RECORD.md`;
- Windows: `templates/WINDOWS_VALIDATION_RECORD.md`;
- iPhone/iPad: `templates/IOS_DEVICE_VALIDATION_RECORD.md`;
- Mac Catalyst: `templates/MACCATALYST_VALIDATION_RECORD.md`.

## Cross-platform production templates

- Accessibility: `templates/ACCESSIBILITY_VALIDATION_RECORD.md`;
- Existing-data/document/backup compatibility: `templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`;
- Signing and provenance: `templates/SIGNING_PROVENANCE_RECORD.md`;
- Store policy/submission/review/approval/publication: `templates/STORE_SUBMISSION_RECORD.md`;
- Final production approval: `templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`.

The templates are evidence containers, not evidence by themselves.

## Release-specific evidence directory

When real validation begins, create a release-specific directory such as:

`docs/releases/evidence/v1.0.0/`

Copy only the applicable templates into that directory and fill them with actual non-sensitive evidence. Do not modify canonical templates to make them appear completed.

Recommended naming examples:

- `android-pixel-<date>.md`;
- `android-samsung-<date>.md`;
- `windows-<build>-<date>.md`;
- `ios-iphone-<version>-<date>.md`;
- `maccatalyst-<version>-<date>.md`;
- `accessibility-<platform>-<assistive-tech>-<date>.md`;
- `compatibility-<origin>-to-<target>-<date>.md`;
- `signing-<platform>-<date>.md`;
- `store-<store>-<submission-id>.md`;
- `production-approval.md`.

## Data and secret boundary

Use fictional/synthetic CareNest data for public/shared evidence.

Do not commit real medical data, prescription/document contents, passwords, PINs, private keys, signing secrets, access tokens, recovery codes or service credentials.

## Result-state rule

Every release-specific record uses the status vocabulary defined by `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`:

- `PASS`;
- `FAIL`;
- `BLOCKED`;
- `N/A`;
- `NOT RUN`.

Unknown, stale or unperformed work is not a pass.

## Current state rule

Use `AUTOMATED_BASELINE.md` for the current accepted exact-source automation and `NEXT_STEPS.md` / `PROJECT_STATUS.md` for current operational status.

The remaining production blockers must stay open until actual package/device/accessibility/signing/store evidence exists. A canonical template, green source build or simulator compile is not production evidence for a manual row.
