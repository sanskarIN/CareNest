# CareNest Store Asset and Listing Guidance

**Release line:** `1.0.0-rc.1`

This document defines how to prepare CareNest icons, screenshots, promotional images and store-listing content while preserving privacy, medical-safety, accessibility and package-policy boundaries.

Store requirements can change. Always review the current requirements for the exact distribution channel at submission time.

## 1. Current application-package boundary

The distributed CareNest application source/package intentionally contains **no external Buy Me a Coffee destination/card/command/artwork**.

Do not create store screenshots, listing copy or promotional graphics that imply a Buy Me a Coffee button exists inside the app.

Voluntary project support remains repository documentation/metadata only:

`https://buymeacoffee.com/sanskarIN`

Repository project support does not unlock health functionality, reminder priority/reliability, medical advice, emergency assistance or access to local records.

## 2. Source assets

Use the actual version-controlled application assets under `src/CareNest.App/Resources/`, including the current app icon, splash and CareNest mark variants present in source.

Do not reintroduce removed packaged project-funding artwork merely to make a promotional image. The URL-bearing funding artwork was intentionally removed from the application package after package inspection found the external destination embedded in Windows payload bytes.

Do not share development font files or use unlicensed third-party assets.

## 3. Store icon

Render the CareNest app icon at every store-required size.

Guidelines:

- no small text inside the icon;
- preserve safe margins;
- verify masked/adaptive behavior where required;
- verify appearance on light/dark launcher surfaces;
- avoid medical-accreditation imagery such as a red cross or symbols implying professional certification.

The visual identity should communicate organization, care, privacy and scheduling—not clinical authority.

## 4. Splash

Splash content should be simple, fast and privacy-safe.

Allowed content can include:

- CareNest mark/name;
- approved creator watermark such as `Made by the Sanskar` where appropriate.

Never place user health information on splash artwork.

## 5. Promotional/feature graphic

A suitable direction is calm local-first organization using CareNest brand elements.

Accurate phrases can include:

- `Local-first health organization`;
- `Organize reminders, appointments and documents locally`;
- `Your schedules. Your device. Your records.`

Avoid unsupported claims such as:

- medically approved;
- never miss a dose;
- guaranteed medicine reminders;
- AI doctor;
- dosage calculator;
- treatment advisor;
- medication-interaction safety checker;
- emergency assistant.

## 6. Screenshot data policy

All public/store screenshots must use fictional/synthetic data.

Never show:

- real prescriptions;
- real health documents;
- identifiable medicine notes;
- real appointments/clinician contact information;
- real emergency contacts;
- private backup filenames or paths;
- private email/message content;
- real user profile photos without appropriate authorization.

Use obviously fictional profile/medicine/document values that do not resemble a real person's health record.

## 7. Recommended screenshot set

A representative set can include:

- onboarding/local-first and medical-limitation screen;
- dashboard;
- multiple local profiles;
- medicines list/editor;
- schedule editor;
- upcoming reminders;
- medication log;
- appointment organizer;
- encrypted document organizer with synthetic filenames;
- reports/export screen;
- settings/privacy/reminder diagnostics;
- About/legal/open-source/support-contact screen.

Do **not** include an in-app BMC/project-funding screenshot because that feature is not part of the current distributed app package.

## 8. Screenshot copy

Accurate phrases include:

- `Organize family health information locally`;
- `Create reminders from your own schedule`;
- `Keep imported documents encrypted locally`;
- `Export your own reports and backups`;
- `No required CareNest account`.

Do not imply CareNest chooses medicine timing, dosage, treatment or clinical priority.

## 9. Notification screenshots

If showing a notification example:

- use fictional data;
- minimize health details;
- do not imply delivery is guaranteed;
- do not encourage sensitive lock-screen previews as a default;
- reflect the actual notification wording shipped by the candidate build.

Marketing must distinguish deterministic CareNest schedule planning from OS-controlled notification delivery.

## 10. Reminder marketing boundary

Operating-system permission, battery/background restrictions, force-stop/shutdown, clock/time-zone changes and platform policy can affect delivery.

Avoid slogans such as `Never miss a medicine again` or `Guaranteed reminders`.

## 11. Medical-safety listing boundary

Store assets/descriptions must not describe CareNest as providing:

- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- clinical medication-interaction checking;
- clinical risk scoring;
- emergency services;
- verified adherence.

CareNest is an organizational tool based on explicit user-entered information.

## 12. Privacy/local-first claims

Accurate current v1 claims include:

- no required CareNest account/backend;
- no automatic CareNest cloud synchronization/upload;
- local SQLite structured records;
- separately encrypted imported document payloads;
- password-encrypted manual backups;
- explicit user-controlled exports/shares.

Do not claim:

- transparent whole-database encryption;
- that the OS/device can never back up application data;
- that exported/shared copies remain under CareNest control.

## 13. App-lock claims

Accurate wording:

`Optional local app lock`

Do not advertise it as:

- whole-database encryption;
- device encryption;
- protection against a rooted/jailbroken/fully compromised device.

## 14. Repository project support

Repository support metadata/documentation can mention:

`https://buymeacoffee.com/sanskarIN`

This is not a current in-app store surface.

Do not describe project support as:

- purchase of medical functionality;
- premium reminder delivery;
- emergency support;
- priority health support;
- access to user records;
- a subscription required for core organizational features.

## 15. Store-policy review

At submission time:

1. review current Apple App Store requirements relevant to the actual binary/listing;
2. review current Google Play requirements relevant to the actual binary/listing;
3. review current Microsoft/Windows distribution requirements where used;
4. review health-organizer wording and privacy/data-safety declarations;
5. verify screenshots/listing match the exact candidate package;
6. record the review date/source/conclusion in release evidence.

The current application binary does not need an in-app funding-link toggle because the external project-funding destination is absent from application source/package by product policy.

## 16. Store descriptions

Short/long descriptions must match the shipping binary.

Do not advertise deferred features such as:

- automatic cloud synchronization;
- remote caregiver collaboration;
- required accounts;
- diagnosis;
- clinical decision support;
- clinical interaction/risk scoring.

## 17. Privacy/data-safety forms

Complete forms from actual runtime behavior.

Re-review if a future build adds:

- analytics;
- crash reporting/telemetry;
- cloud synchronization;
- accounts/authentication;
- remote support;
- server storage;
- new third-party SDKs;
- new export/share destinations.

Do not blindly reuse current v1 declarations after a network/data-flow change.

## 18. Permission/capability descriptions

Store text must match actual target configuration.

Review, as applicable:

- notification permission;
- Android alarm capabilities;
- file/document picker access;
- calendar export/integration;
- Apple entitlements;
- Windows capabilities.

Do not request permissions solely to support marketing screenshots.

## 19. Platform screenshot sizing

Generate required store dimensions from representative final candidate UI/device classes rather than stretching a single screenshot.

Verify:

- required phone orientations/sizes;
- tablet/iPad sizes where applicable;
- desktop window sizes where applicable;
- no clipped text;
- correct system UI/status bar framing;
- large-text/localization layouts where represented.

## 20. Localization in listings

If a store listing is localized:

- translate marketing text with the same medical/privacy boundaries;
- use localized screenshots only for shipped/tested locales where appropriate;
- do not advertise a language not actually supported;
- review date/time formatting and text expansion.

Current documented app language for `1.0.0-rc.1`: English unless the source/release candidate explicitly adds another tested locale.

## 21. Accessibility in store imagery

Screenshots should show readable, uncrowded UI.

Do not claim complete accessibility certification merely because source semantics exist. Real screen-reader, large-text, keyboard/focus, contrast and reduced-motion validation remains a release gate.

## 22. Creator/support metadata

Product: **CareNest**

Creator: `https://www.github.com/sanskarIN`

Repository: `https://github.com/sanskarIN/CareNest`

Business: `sanskarin@outlook.in`

Support: `supportramsandesh@gmail.com`

Watermark: `Made by the Sanskar`

Repository-only voluntary support: `https://buymeacoffee.com/sanskarIN`

## 23. Current application identity

- Application title: `CareNest`;
- Application ID: `com.sanskar.carenest`;
- Display version: `1.0.0-rc.1`;
- Application version/build: `1`.

Verify final store metadata against the exact signed package rather than assuming documentation is sufficient.

## 24. Package inspection

Before production submission record:

- exact source SHA;
- package filename/identity/version;
- SHA-256;
- signing/notarization/store provenance;
- forbidden external-funding marker scan result;
- installed About/legal/support-contact inspection;
- smoke-test result.

Internal CI inspection artifacts are not automatically store-ready production packages.

## 25. Release asset checklist

Before submission verify:

- final icon/splash match current source;
- screenshots use fictional data;
- screenshots match the exact shipping build/version;
- no private health data;
- no unsupported medical claims;
- no guaranteed reminder claim;
- no whole-database encryption claim;
- no screenshot/listing implying a removed in-app BMC funding surface;
- privacy/data-safety forms match runtime;
- support/privacy/terms/security links are current;
- package/bundle identity matches the signed artifact;
- accessibility/store-policy review is documented;
- final package checksums/provenance are recorded.

## Related documentation

- `docs/design/DESIGN_SYSTEM.md`
- `docs/design/ACCESSIBILITY.md`
- `docs/design/LOCALIZATION.md`
- `docs/PLATFORM_BEHAVIOR_MATRIX.md`
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md`
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`
- `docs/releases/RELEASE_PROCESS.md`
- `PRIVACY.md`
- `PROJECT_STATUS.md`