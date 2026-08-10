# CareNest Release Notes Template

Use this template only after the exact release commit has completed the automated and manual gates in `docs/releases/RELEASE_CHECKLIST.md`.

# CareNest <version>

## Release identity

- Version: `<version>`
- Build: `<build>`
- Tag: `<tag>`
- Commit: `<full commit SHA>`
- Release date: `<YYYY-MM-DD>`

## What this release includes

Describe only behavior that exists in the tagged source. Keep claims factual and non-clinical.

- Local-first organizer improvements:
- Reminder reliability improvements:
- Appointment/document organization improvements:
- Backup/export improvements:
- Accessibility/UI improvements:
- Security/privacy improvements:
- Developer/release-engineering improvements:

## Medical and reminder limitations

CareNest is an organizational tool. It does not diagnose conditions, determine dosage, recommend treatment, perform medication-interaction checking, replace a doctor/pharmacist, or provide emergency services.

Reminder delivery can be affected by notification permissions, battery optimization, exact-alarm capability, device shutdown/force-stop behavior, time-zone changes and operating-system policy. In an emergency, contact local emergency services rather than relying on CareNest.

## Privacy

CareNest v1 is local-first and does not require an account. It does not automatically upload health records or documents to a CareNest backend. Manual exports and the external voluntary project-support link leave the CareNest trust boundary only after explicit user action.

## Security/dependency notes

- CodeQL run: `<run/link>`
- Dependency-audit run: `<run/link>`
- Open dependency risks: `<none or exact tracked items>`
- SQLitePCLRaw advisory decision: `<resolved / release blocked / documented approved decision>`

Never describe a suppressed dependency advisory as fixed.

## Verification evidence

- CareNest CI: `<run/link>`
- Release Evidence workflow: `<run/link>`
- Unit tests: `<result>`
- Integration tests: `<result>`
- UI-contract tests: `<result>`
- Android Release build: `<result>`
- Windows Release build: `<result>`
- iOS simulator Release build: `<result>`
- Mac Catalyst Release build: `<result>`
- Manual device matrix: `<completed evidence location>`
- Accessibility review: `<completed evidence location>`
- Store-policy review: `<completed evidence location>`

## Known limitations

List platform/release limitations that remain true. Do not call the application bug-free or imply guaranteed reminder delivery.

## Open source and support

- Repository: https://github.com/sanskarIN/CareNest
- Creator: https://www.github.com/sanskarIN
- Support: `supportramsandesh@gmail.com`
- Business: `sanskarin@outlook.in`
- Voluntary project support: https://buymeacoffee.com/sanskarIN

Financial support is optional and does not unlock medical advice, premium health behavior, different reminder behavior, emergency assistance, support priority or access to local CareNest data.
