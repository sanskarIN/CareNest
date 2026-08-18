# CareNest Release Notes Template

Use this template only after the exact release commit has completed the applicable automated, manual, package, accessibility, signing and store gates in `docs/releases/RELEASE_CHECKLIST.md`.

# CareNest <version>

## Release identity

- Version: `<version>`
- Build: `<build>`
- Tag: `<tag>`
- Commit: `<full commit SHA>`
- Release date: `<YYYY-MM-DD>`
- Package/application identity: `<identity>`

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

CareNest is an organizational tool. It does not diagnose conditions, determine or infer dosage, recommend treatment, perform clinical medication-interaction checking, calculate clinical risk, replace a doctor/pharmacist, or provide emergency services.

Reminder delivery can be affected by notification permissions, battery optimization, exact-alarm capability, device shutdown/force-stop behavior, time-zone changes and operating-system policy. In an emergency, contact local emergency services rather than relying on CareNest.

## Privacy

CareNest v1 is local-first and does not require a CareNest account or CareNest-owned backend. It does not automatically upload local health records or documents to a CareNest cloud service.

Manual exports/shares/calendar actions can create copies outside the CareNest trust boundary only after explicit user action.

Repository-only Gumroad and Buy Me a Coffee destinations are intentionally excluded from the distributed CareNest application package under the current release policy. Visiting those external services separately is outside CareNest and does not give them access to local CareNest health data through the app.

## Repository storefront/support boundary

- Gumroad storefront: https://ramsandesh.gumroad.com
- Voluntary project support: https://buymeacoffee.com/sanskarIN

These destinations are repository/documentation surfaces, not in-app health functionality.

A Gumroad purchase or voluntary contribution does not unlock or change:

- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- clinical interaction/risk behavior;
- reminder priority/reliability;
- emergency assistance;
- support priority;
- accounts/cloud behavior;
- access to local CareNest health data.

## Security/dependency notes

- CodeQL run: `<run/link>`
- Dependency-audit run: `<run/link>`
- Open dependency risks: `<none or exact tracked items>`
- SQLitePCLRaw advisory decision: `<resolved / release blocked / documented approved decision>`

Never describe a suppressed dependency advisory as fixed.

## Verification evidence

- CareNest CI: `<run/link>`
- CodeQL: `<run/link>`
- Dependency Audit: `<run/link>`
- Store Package Configuration: `<run/link>`
- Store Inspection Artifacts: `<run/link>`
- Release Gate: `<run/link>`
- Release Evidence workflow: `<run/link>`
- Unit tests: `<result>`
- Integration tests: `<result>`
- UI/source-policy tests: `<result>`
- Android Release build: `<result>`
- Windows Release build: `<result>`
- iOS simulator Release build: `<result>`
- Mac Catalyst Release build: `<result>`
- Manual Android evidence: `<location/result>`
- Manual Windows evidence: `<location/result>`
- Manual iOS/iPadOS evidence: `<location/result>`
- Manual Mac Catalyst evidence: `<location/result>`
- Packaged SQLite compatibility: `<location/result>`
- Encrypted document/backup compatibility: `<location/result>`
- Accessibility review: `<location/result>`
- Store-policy review date/sources: `<location/result>`
- Live Google Play Health apps declaration: `<result/N/A>`
- Live Google Play Data safety: `<result/N/A>`
- Apple privacy/store metadata: `<result/N/A>`
- Microsoft privacy/store metadata: `<result/N/A>`

## Final package evidence

For every published production artifact include:

- Package filename: `<filename>`
- Package SHA-256: `<sha256>`
- Package evidence JSON: `<path/link>`
- Package evidence payload SHA-256: `<sha256>`
- Signing/notarization/store provenance: `<non-secret provenance>`
- Buy Me a Coffee payload-marker scan: `<pass/fail>`
- Gumroad payload-marker scan: `<pass/fail>`
- Installed-package smoke test: `<pass/fail>`

Generate the structured package evidence according to:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

Do not put private signing keys, keystore passwords, account credentials, real health data, backup passwords, PINs or encryption keys into release notes or package evidence.

## Known limitations

List platform/release limitations that remain true. Do not call the application bug-free or imply guaranteed reminder delivery.

## Open source and support

- Repository: https://github.com/sanskarIN/CareNest
- Creator: https://www.github.com/sanskarIN
- Support: `supportramsandesh@gmail.com`
- Business: `sanskarin@outlook.in`
- Gumroad storefront: https://ramsandesh.gumroad.com
- Voluntary project support: https://buymeacoffee.com/sanskarIN

Repository storefront/support links are optional and separate from the distributed health-app package under the current policy.
