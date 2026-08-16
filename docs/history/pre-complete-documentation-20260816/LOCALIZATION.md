# CareNest Localization Architecture

CareNest `1.0.0-rc.1` ships English (`en`) first and is structured for future resource-based localization.

Localization must preserve the product's local-first, privacy, medical-safety, and reminder-delivery meanings. A translated string must not accidentally turn organizational wording into a diagnosis, dosage recommendation, treatment recommendation, emergency promise, or guaranteed reminder-delivery claim.

## Current language status

Enabled application language in the current release candidate:

- English (`en`).

The app follows device UI culture/resource lookup behavior while English remains the only shipping resource set.

## Resource architecture

User-facing product/safety strings are backed by:

`src/CareNest.App/Resources/Strings/AppResources.resx`

and exposed through the application's resource/text abstraction such as `AppText`.

Future languages should use sibling satellite resource files, for example:

```text
AppResources.hi.resx
AppResources.es.resx
AppResources.fr.resx
```

Do not fork persistence models or domain values merely to add a display language.

## What should be localized

Examples:

- navigation labels;
- buttons/actions;
- onboarding text;
- validation messages;
- settings labels;
- medical/reminder limitations;
- privacy/support/about labels;
- status display text;
- report headings/disclaimers where output localization is intentionally supported;
- accessibility semantic labels.

## What must not be automatically translated

Do not automatically translate user-entered content such as:

- medicine names;
- strength text;
- instruction text;
- clinician names;
- appointment notes;
- profile notes;
- document titles/files;
- emergency-contact names;
- custom labels.

These values are user data and may need to remain exactly as entered.

## Opaque medicine-text rule

Localization must not parse/transform medicine strength/instruction text into application-generated dosage/frequency wording.

CareNest stores these values as opaque user-entered strings.

## Safety-critical strings

Translations must preserve the exact intent of concepts such as:

- CareNest is organizational only;
- it does not diagnose;
- it does not calculate/infer dosage;
- it does not recommend treatment;
- it does not check interactions/clinical risk;
- it is not an emergency service;
- reminder delivery can be affected by OS permissions/battery/background policy;
- as-needed schedules do not create automatic reminders;
- invalid DST-gap times are not silently moved to an invented replacement time.

Safety translations should receive dedicated review rather than casual machine translation.

## Layout rules

Localized UI must:

- allow text wrapping;
- allow dynamic height;
- avoid fixed widths/heights for long safety/validation text;
- keep primary actions reachable under string expansion;
- support large system text sizes;
- avoid overlapping icons/text;
- preserve readable cards/forms in narrow phone layouts and desktop resizing.

## Avoid sentence-fragment concatenation

Do not construct translated sentences from multiple independently translated fragments when word order/grammar can vary.

Prefer one complete resource string with placeholders.

Bad pattern:

```text
"Reminder for " + medicine + " is " + state
```

Prefer a localized full template whose placeholder order can vary by language.

## Placeholders

Use named/positional placeholders only when they do not expose hidden private data or force English word order.

Review placeholders for:

- grammatical gender/plural forms;
- date/time formatting;
- screen-reader output;
- privacy-sensitive information in notification text.

## Dates and times

Presentation can be locale-aware.

Persistence/reminder identity rules must not mutate because of localization.

Keep:

- stored UTC values as UTC;
- explicit schedule time-zone IDs unchanged;
- machine-readable export formats invariant where required;
- user-entered local schedule intent unchanged.

Display formatting may use locale conventions without rewriting stored timestamps.

## Numbers

User-facing numbers can use locale formatting where appropriate.

Machine-readable CSV/JSON fields requiring stable interchange should use deliberately defined invariant formatting rather than whatever UI culture is active.

## Time-zone names

Do not translate/replace stored time-zone IDs.

The UI can show friendly localized descriptions if a future feature adds them, but the stable stored identifier remains the scheduling contract.

## Right-to-left languages

Before shipping an RTL locale:

- test mirrored layout;
- test navigation/back affordances;
- test icons whose direction conveys meaning;
- test schedule editor weekday/time layouts;
- test document/report controls;
- test keyboard/focus order;
- test charts/visuals if future versions add them;
- test project-support/about surfaces;
- run screen-reader review in the target locale.

Do not enable RTL language support based only on successful resource compilation.

## Accessibility and localization

Localized semantic labels must remain natural and concise.

Avoid translating visible text while leaving English-only accessibility labels.

Large-text and screen-reader testing should be repeated for new shipping locales because translated text length/pronunciation can expose issues not visible in English.

See `docs/design/ACCESSIBILITY.md`.

## Notifications

Notification translation must preserve privacy minimization.

Do not localize by inserting sensitive medicine/profile content into a previously generic notification string.

Generic notification labels should remain generic unless the user explicitly configures otherwise according to implemented product behavior.

## Reports/exports

If reports become localized:

- keep medical/privacy disclaimer meaning equivalent;
- keep machine-readable formats stable;
- document which outputs are localized vs invariant;
- add regression tests for headers/disclaimers;
- ensure receiving software can still parse expected CSV/JSON representations.

## Branding

Product name **CareNest** and creator watermark `Made by the Sanskar` are brand identifiers and normally remain unchanged unless an explicit branding decision says otherwise.

Canonical links/emails must not be translated:

- `https://github.com/sanskarIN/CareNest`
- `https://www.github.com/sanskarIN`
- `https://buymeacoffee.com/sanskarIN`
- `sanskarin@outlook.in`
- `supportramsandesh@gmail.com`

## Resource key stability

Use meaningful resource keys so tests and future translations remain maintainable.

Repository branding/localization contract tests protect key safety/branding resources from accidental deletion.

## Translation workflow

Recommended workflow for a new locale:

1. select target locale based on real user demand;
2. create sibling `.resx` resource file;
3. translate UI strings with context;
4. separately review safety/privacy/reminder limitation text;
5. verify placeholders/plurals/date/time formatting;
6. build target platforms;
7. test phone/tablet/desktop layouts;
8. test large text/accessibility semantics;
9. test RTL if applicable;
10. update store listing/screenshots if localized;
11. add localization contract tests where useful;
12. update privacy/release documentation if translated policies/listings are published.

## Machine translation

Machine translation may help produce a draft for non-safety copy, but must not be treated as final review for medical/privacy/legal/reminder limitation text.

Never machine-translate user-entered health content automatically as part of normal CareNest v1 behavior.

## Fallback behavior

If a resource is missing, the application should fall back according to .NET resource lookup rather than inventing or silently removing safety text.

A missing safety resource in a shipping locale is a release defect.

## Testing

Localization-related automated checks should protect:

- resource file validity;
- required safety keys;
- brand/support URL consistency;
- no accidental removal of key disclaimers.

Manual checks remain necessary for:

- truncation/wrapping;
- layout direction;
- screen readers;
- translated meaning/context;
- store listing quality.

## Current limitation

English is the only enabled shipping language in `1.0.0-rc.1`. Additional `.resx` satellite resources are future work and should not be described as already supported languages until they are actually included, reviewed, tested, and shipped.

## Related documentation

- `docs/design/DESIGN_SYSTEM.md`
- `docs/design/ACCESSIBILITY.md`
- `docs/USER_GUIDE.md`
- `docs/releases/MANUAL_TEST_MATRIX.md`
- `docs/privacy/PRIVACY_MODEL.md`