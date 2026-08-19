# CareNest Production Evidence Index

**Release line:** `1.0.0-rc.1`  
**Accepted automated source before production validation:** `30ee6c265104c64ec5a1a4013f592f7f058750e8`  
**Accepted automated result:** **370/370 core tests passed**

This index links the repository preparation for the remaining production-only CareNest evidence. None of the templates below claims that the corresponding validation has already been performed.

## Evidence rules

Read first:

- `PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`
- `PACKAGED_RELEASE_VALIDATION.md`
- `PACKAGE_EVIDENCE_TOOLING.md`
- `RELEASE_CHECKLIST.md`
- `NEXT_STEPS.md`

## Platform validation templates

- Android: `templates/ANDROID_DEVICE_VALIDATION_RECORD.md`
- Windows: `templates/WINDOWS_VALIDATION_RECORD.md`
- iPhone/iPad: `templates/IOS_DEVICE_VALIDATION_RECORD.md`
- Mac Catalyst: `templates/MACCATALYST_VALIDATION_RECORD.md`

## Cross-platform production templates

- Accessibility: `templates/ACCESSIBILITY_VALIDATION_RECORD.md`
- Existing-data/document/backup compatibility: `templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`
- Signing and provenance: `templates/SIGNING_PROVENANCE_RECORD.md`
- Store policy/submission/approval/publication: `templates/STORE_SUBMISSION_RECORD.md`
- Final production approval: `templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md`

## Suggested evidence directory for a real release

When real validation begins, create a release-specific directory such as:

`docs/releases/evidence/v1.0.0/`

Copy only the applicable templates into that directory and fill them with real non-sensitive evidence. Do not modify the canonical templates to make them appear completed.

Recommended naming examples:

- `android-pixel-<date>.md`
- `android-samsung-<date>.md`
- `windows-<build>-<date>.md`
- `ios-iphone-<version>-<date>.md`
- `maccatalyst-<version>-<date>.md`
- `accessibility-<platform>-<assistive-tech>-<date>.md`
- `compatibility-<origin>-to-<target>-<date>.md`
- `signing-<platform>-<date>.md`
- `store-<store>-<submission-id>.md`
- `production-approval.md`

Do not commit real medical data, passwords, PINs, private keys, signing secrets, access tokens or recovery codes.

## Current state

Automated source verification is complete for the accepted 2026-08-19 baseline. The remaining release blockers still require actual package/device/accessibility/signing/store evidence and must remain open until performed.
