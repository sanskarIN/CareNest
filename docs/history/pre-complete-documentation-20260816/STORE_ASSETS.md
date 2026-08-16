# CareNest Store Asset Guidance

This document defines how to prepare CareNest visual assets and screenshots for public distribution while keeping privacy, branding, medical-safety, and store-policy boundaries accurate.

Final store requirements can change. Always verify the current requirements for the exact distribution channel at submission time.

## Source assets

Use version-controlled vector sources under the CareNest MAUI resources, including app icon/splash/brand variants.

Relevant asset families include:

- adaptive app icon/foreground;
- splash asset;
- standard CareNest mark;
- light-surface mark;
- dark-surface mark;
- monochrome/system mark;
- compact CareNest project-support badge;
- custom CareNest Buy Me a Coffee project-support vector artwork.

Do not share font files from development environments or use unlicensed third-party assets.

## Store icon

Render the CareNest app icon at each store-required size.

Guidelines:

- no small text inside the store icon;
- preserve safe margins;
- verify appearance on light/dark launcher surfaces;
- verify adaptive/masked icon behavior on Android;
- verify monochrome/system icon requirements where applicable;
- avoid a red cross/official medical-accreditation symbol.

The mark should communicate organization/care/privacy rather than medical certification.

## Splash

Splash branding should be simple and fast-loading.

Allowed branding:

- CareNest mark/name;
- subtle `Made by the Sanskar` creator watermark where appropriate.

Do not place user data on splash surfaces.

## Feature/promotional graphic

Suggested visual direction:

- calm background;
- CareNest shield/nest/calendar-check visual language;
- product name;
- short accurate phrase such as `Local-first health organization`.

Avoid claims such as:

- medically approved;
- never miss a dose;
- guaranteed medicine reminders;
- AI doctor;
- dosage calculator;
- treatment advisor;
- interaction safety checker;
- emergency assistant.

These claims exceed the implemented product boundary.

## Screenshots

All public screenshots must use fictional/synthetic data.

Never show:

- real prescriptions;
- real health documents;
- real medicine notes for an identifiable person;
- real appointments/clinician contact details;
- real emergency contacts;
- real backup filenames containing private data;
- private email/message content;
- real user profile photos unless fully authorized/appropriate.

Recommended screenshot set can cover:

- onboarding/local-first statement;
- dashboard;
- multiple family profiles;
- medicine list/editor with clearly fictional values;
- schedule editor;
- upcoming reminders;
- medication log;
- appointment organizer;
- encrypted document organizer with synthetic filenames;
- reports/export screen;
- settings/privacy/reminder diagnostics;
- About/open-source/support screen.

## Screenshot copy

Use accurate descriptions such as:

- `Organize family health information locally`;
- `Create reminders from your own schedule`;
- `Keep imported documents encrypted locally`;
- `Export your own reports and backups`.

Do not imply CareNest chooses medicine timing/dosage for the user.

## Notification screenshot safety

If showing a notification example:

- use fictional data;
- prefer generic notification labels;
- do not imply guaranteed delivery;
- do not show lock-screen private health content as a recommended configuration.

## Reminder wording

Marketing/store text must preserve the distinction between:

- deterministic CareNest schedule materialization; and
- operating-system notification delivery.

OS permissions, battery/background policy, force-stop/shutdown, and platform rules can affect delivery.

Avoid slogans such as `Never miss a medicine again` because they create an unsupported guarantee.

## Medical boundary

Store assets/listings must not describe CareNest as providing:

- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- medication-interaction checks;
- clinical risk scores;
- emergency services;
- verified medication adherence.

CareNest is an organizational tool based on explicit user-entered information.

## Privacy/local-first claims

Accurate v1 claims can describe:

- no required CareNest account;
- no required CareNest backend;
- no automatic CareNest cloud sync/upload;
- local SQLite structured records;
- encrypted imported document payloads;
- manual password-encrypted backups;
- explicit user-controlled exports.

Do not claim whole-database encryption because current SQLite storage does not provide that guarantee.

Do not claim that the OS never backs up application data; platform/device settings can independently affect backups.

## App-lock claims

Accurate wording:

`Optional local app lock` or equivalent.

Do not advertise app lock as:

- full database encryption;
- device encryption;
- protection against a rooted/jailbroken/fully compromised device.

## Buy Me a Coffee / project-support surfaces

Canonical URL:

`https://buymeacoffee.com/sanskarIN`

Support must be presented as voluntary project support.

Do not present it as:

- purchase of medical functionality;
- premium reminder delivery;
- emergency support;
- priority health support;
- access to local CareNest data;
- a subscription required to use core health-organizational functionality.

Custom CareNest support artwork must not be represented as official Buy Me a Coffee brand artwork.

## Store policy review for external support link

Before including an external support/donation link in a store-distributed binary or listing:

1. verify current Apple App Store rules for the exact category/channel;
2. verify current Google Play rules for the exact category/channel;
3. verify Windows/other channel requirements as applicable;
4. document the decision in release evidence/checklists.

If a channel disallows the in-app link:

- remove/disable the in-app action for that channel;
- keep repository funding links where permitted;
- do not relabel the funding action as a medical purchase to bypass policy.

## Store descriptions

The long/short descriptions must match the shipping binary.

Include accurate capabilities only.

If a feature is deferred (cloud sync, remote caregiver collaboration, accounts), do not advertise it as available.

## Privacy/data-safety forms

Complete forms based on actual runtime behavior.

Re-review if a later build adds:

- analytics;
- crash reporting;
- cloud sync;
- account/authentication;
- remote support;
- server storage;
- new third-party SDKs.

Current v1 documentation should not be reused blindly for a future network-enabled build.

## Permission declarations

Store permission/capability explanations must match actual target configuration.

Examples to review:

- notification permission;
- Android alarm capabilities;
- file/media/document picker access;
- calendar export/interaction;
- Apple entitlements;
- Windows capabilities.

Do not request/store permissions solely for marketing screenshots.

## Platform screenshot sizes

Generate platform/store-required dimensions from the final packaged UI/device classes rather than stretching one source screenshot.

Verify:

- phone portrait/landscape if required;
- tablet/iPad where required;
- desktop window sizes where relevant;
- no clipped text;
- correct status bar/system UI presentation.

## Localization

If a store listing is localized:

- translate marketing text with the same medical/privacy limits;
- use localized screenshots only after that app locale is actually supported/tested where required;
- do not advertise a language that is not shipped.

Current app language in `1.0.0-rc.1`: English.

## Accessibility in store imagery

Screenshots should demonstrate legible UI rather than extremely dense text.

Do not hide accessibility settings/large-text compatibility in the product claim if manual verification remains incomplete.

## Creator/support metadata

Product: **CareNest**

Creator profile: `https://www.github.com/sanskarIN`

Business: `sanskarin@outlook.in`

Support: `supportramsandesh@gmail.com`

Watermark: `Made by the Sanskar`

Voluntary support: `https://buymeacoffee.com/sanskarIN`

## Release asset checklist

Before submission verify:

- final app icon rendered from current source;
- splash correct;
- screenshots use fictional data;
- screenshots match exact shipping build/version;
- no private health data;
- no unsupported medical claims;
- no guaranteed reminder claim;
- no whole-database encryption claim;
- support-link policy reviewed;
- privacy/data-safety forms match runtime;
- listing links point to current privacy/terms/security/support docs;
- package/bundle identity matches signed artifact.

## Related documentation

- `docs/design/DESIGN_SYSTEM.md`
- `docs/design/ACCESSIBILITY.md`
- `docs/design/LOCALIZATION.md`
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md`
- `docs/releases/RELEASE_PROCESS.md`
- `PRIVACY.md`
- `PROJECT_STATUS.md`