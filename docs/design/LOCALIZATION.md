# CareNest Localization Architecture

CareNest `1.0.0-rc.1` ships English (`en`) first and is structured for future resource-based localization.

Localization must preserve local-first, privacy, medical-safety and reminder-delivery meanings. A translation must not turn organizational wording into diagnosis, dosage/treatment recommendation, emergency promise or guaranteed notification delivery.

## 1. Current language status

Enabled shipping language:

- English (`en`).

Additional locales must not be described as supported until translated, reviewed, built, accessibility-tested and shipped.

## 2. Resource architecture

User-facing strings are backed by application resource files such as:

`src/CareNest.App/Resources/Strings/AppResources.resx`

Future locale resources can use sibling `.resx` files, for example:

```text
AppResources.hi.resx
AppResources.es.resx
AppResources.fr.resx
```

Do not fork domain/persistence values merely to add display language.

## 3. What should be localized

Examples:

- navigation/buttons/actions;
- onboarding;
- validation/errors;
- settings;
- medical/reminder/privacy limitations;
- status display text;
- About/legal/support-contact labels;
- report headings/disclaimers when an output is intentionally localized;
- accessibility semantic labels.

## 4. What must not be automatically translated

Do not automatically translate user-entered medicine names, strength/instruction text, clinician names, appointment/profile notes, document titles/files, emergency-contact names or custom labels.

Medicine text remains opaque user data and must not be transformed into application-generated dosage/frequency language.

## 5. Safety-critical translation

Translations must preserve that CareNest:

- is organizational only;
- does not diagnose;
- does not calculate/infer dosage;
- does not recommend treatment;
- does not perform clinical interaction/risk scoring;
- is not an emergency service;
- cannot guarantee reminder delivery;
- creates no automatic reminder for as-needed schedules;
- does not silently replace invalid DST-gap local times.

Safety/privacy/reminder limitation strings require dedicated human/context review rather than unreviewed machine translation.

## 6. Layout rules

Localized UI must allow wrapping/dynamic height, keep actions reachable under string expansion, support large text and remain usable on narrow phones, tablets and resizable desktop windows.

Avoid sentence-fragment concatenation that assumes English grammar/order. Prefer complete resource strings with placeholders whose order can vary.

## 7. Placeholders

Review placeholders for grammar/plurals/date-time formatting, accessibility output and privacy-sensitive notification content.

Do not introduce sensitive data into a generic notification merely because a localized template supports placeholders.

## 8. Dates/times/numbers

Presentation may use locale conventions while persistence/reminder identity remains stable.

Preserve:

- stored UTC values as UTC;
- explicit time-zone IDs;
- stable/invariant machine-readable export formats where defined;
- user-entered schedule intent.

## 9. Time-zone IDs

Do not translate stored time-zone identifiers. A future UI can show friendly localized descriptions while retaining the stable scheduling identifier.

## 10. RTL languages

Before shipping RTL support test layout mirroring, navigation/back affordances, directional icons, schedule editor layouts, document/report controls, keyboard/focus order, About/legal/support surfaces and screen-reader behavior.

Do not enable an RTL locale based only on resource compilation.

## 11. Accessibility and localization

Visible localized strings and semantic labels must remain aligned and natural. Repeat large-text and screen-reader testing for new shipping locales.

See `docs/design/ACCESSIBILITY.md`.

## 12. Notifications

Notification translation must preserve privacy minimization and the non-guaranteed-delivery boundary.

Do not insert sensitive medicine/profile content into a previously generic notification unless an explicitly designed/tested product behavior requires it.

## 13. Reports/exports

If reports become localized:

- preserve medical/privacy disclaimer meaning;
- keep machine-readable formats stable;
- document localized versus invariant outputs;
- add tests for headings/disclaimers;
- ensure CSV/JSON consumers still receive defined representations.

## 14. Branding and fixed identifiers

Product name **CareNest** and creator watermark `Made by the Sanskar` normally remain unchanged unless an explicit branding decision says otherwise.

Do not translate fixed identifiers such as:

- repository `https://github.com/sanskarIN/CareNest`;
- creator `https://www.github.com/sanskarIN`;
- business `sanskarin@outlook.in`;
- support `supportramsandesh@gmail.com`.

Repository-only voluntary project support URL `https://buymeacoffee.com/sanskarIN` is not a current in-app localized action/resource. The distributed application package contains no BMC funding surface.

## 15. Translation workflow

For a new locale:

1. select locale based on real product need;
2. add sibling resource file;
3. translate UI strings with context;
4. separately review safety/privacy/reminder text;
5. verify placeholders/plurals/date/time;
6. build all intended target platforms;
7. test mobile/tablet/desktop layouts;
8. test large text/accessibility;
9. test RTL where applicable;
10. localize store listing/screenshots only when the app actually ships that locale;
11. add resource/localization contracts where useful;
12. update privacy/release docs if translated policies/listings are published.

## 16. Machine translation

Machine translation can assist drafts for non-safety copy but is not final review for medical/privacy/legal/reminder limitation text.

Never automatically translate user-entered health content as normal CareNest v1 behavior.

## 17. Fallback behavior

Missing resources should fall back through normal .NET resource lookup rather than inventing or silently removing safety text.

A missing safety resource in a shipping locale is a release defect.

## 18. Testing

Automated checks can protect resource validity, required safety keys and brand/support identifiers.

Manual checks remain required for wrapping/truncation, directionality, screen readers, translated meaning/context and store listing quality.

## Related documentation

- `docs/design/DESIGN_SYSTEM.md`
- `docs/design/ACCESSIBILITY.md`
- `docs/USER_GUIDE.md`
- `docs/releases/MANUAL_TEST_MATRIX.md`
- `docs/privacy/PRIVACY_MODEL.md`