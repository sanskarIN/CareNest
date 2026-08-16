# CareNest Design System

CareNest aims for calm, clear, accessible, privacy-aware and non-clinical organization across Android, iOS/iPadOS, Mac Catalyst and Windows.

The design must not imply medical certification, diagnosis, treatment authority, dosage selection, clinical interaction checking or emergency-service capability.

## 1. Design principles

1. **Calm over alarming.** Normal organizational states should not look like emergencies.
2. **Clarity over decoration.** Primary actions, dates, reminder state and limitations should be easy to scan.
3. **Accessible by default.** Text scaling, semantics, contrast, keyboard/focus and reduced motion are release concerns.
4. **Privacy-aware.** Avoid unnecessary sensitive text in notifications/public surfaces.
5. **Non-clinical framing.** UI organizes explicit user input without generating clinical conclusions.
6. **Cross-platform consistency.** Keep CareNest identity stable while respecting platform conventions.
7. **Explicit boundaries.** External exports/support links/store actions should be visually distinguishable from local health organization.

## 2. Spacing tokens

Reference scale:

- 4 — micro;
- 8 — compact internal;
- 12 — small control/group;
- 16 — standard content;
- 24 — section;
- 32 — large/hero separation.

Prefer consistent token values over arbitrary per-screen spacing.

## 3. Corner radii

Reference roles:

- 10 — controls/compact interactive surfaces;
- 16 — cards/grouped surfaces;
- 24 — large highlighted surfaces.

Avoid excessive nested rounded containers that obscure hierarchy.

## 4. Touch/click targets

Minimum intended interactive target: about **44×44 logical units** where platform conventions allow.

Icon-only actions still require meaningful accessible naming and sufficient hit area.

Destructive actions should be separated from routine primary actions.

## 5. Typography

Use platform-default/scalable typography rather than fixed pixel assumptions.

Reference scale:

- title: 28;
- section heading: 20;
- body: 16;
- supporting/caption: 13.

Text must wrap under localization and scaling. Avoid fixed-height assumptions around validation, safety or privacy text.

## 6. Text hierarchy

Recommended order:

1. page title;
2. short context/limitation when needed;
3. grouped content;
4. supporting text;
5. primary action;
6. secondary/destructive action.

Medicine strength/instruction text is user content and must never be visually reframed as a CareNest dosage recommendation.

## 7. Semantic color roles

Use semantic roles rather than one-off colors:

- background;
- surface;
- elevated/highlighted surface;
- primary / on-primary;
- primary text;
- muted text;
- divider/border;
- success;
- warning;
- danger;
- link/action;
- focus indicator.

Never rely on color alone for state/validation/stock meaning.

## 8. Status presentation

Reminder states can include Scheduled, Snoozed, Taken, Skipped, Delayed, Missed and Cancelled where applicable.

Every state must include text/accessibility semantics in addition to color/iconography.

Stock/refill state also needs text describing the local estimate/threshold.

## 9. Themes

CareNest supports system, light and dark presentation.

Theme changes must not:

- reset local data;
- alter reminder schedule intent;
- hide limitations/errors;
- make focus/validation unreadable.

Manual release testing covers representative theme behavior.

## 10. Motion

Motion should be short, optional and nonessential.

Motion must never be required to understand reminder state, errors, destructive outcomes, privacy/medical warnings or navigation.

Reduced-motion preference should minimize decorative movement where implemented.

## 11. Cards

Use cards for coherent information groups such as profile summaries, medicine summaries, reminders, appointments, document metadata and settings groups.

Do not wrap every line in decorative cards.

The current application About surface contains normal project/legal/support contact information but **no external BMC funding card/action**.

## 12. Forms/editors

Forms should:

- clearly label required/optional fields;
- keep label/control association clear;
- allow wrapping;
- provide actionable validation;
- avoid raw exception output;
- separate destructive actions from save;
- distinguish user-entered medicine text from application limitations.

## 13. Schedule editor

The schedule editor should clearly show values the user explicitly selected:

- schedule kind;
- start/end dates;
- time zone;
- reminder times;
- weekdays;
- every-N-hours interval/start;
- cycle on/off days;
- follow-up minutes;
- enabled state.

Do not imply CareNest chose a medically appropriate frequency.

As-needed must clearly indicate that no automatic occurrences are created.

## 14. Reminder state actions

Taken/Skipped/Delayed/Snooze/Missed actions should be distinct and non-clinical.

Snooze UI must produce an explicit future destination time.

Actions should not imply verified adherence.

## 15. Destructive actions

Delete/reset/restore-replacement actions should:

- use explicit action names;
- explain affected local data;
- require confirmation where appropriate;
- avoid accidental proximity to routine actions;
- mention backup/export options when useful.

## 16. Empty states

Explain the next organizational action without urgency/medical advice.

Examples:

- no medicines recorded;
- no upcoming reminders;
- no documents imported;
- no appointments recorded.

## 17. Errors/validation

Errors should be human-readable, actionable, privacy-safe and accessible.

Do not expose raw stack traces, database paths, encryption keys, backup passwords or internal IDs unnecessarily.

## 18. Notification copy

Prefer privacy-minimized generic wording.

Do not claim:

- verified adherence;
- dosage recommendation;
- clinical urgency;
- guaranteed delivery.

Lock-screen privacy must be considered during manual release testing.

## 19. Medical limitation surfaces

Onboarding/About/reports/documentation should keep these concepts accessible:

- organizational only;
- no diagnosis/treatment/dosage inference;
- no clinical interaction/risk scoring;
- no emergency service;
- OS/device settings can affect reminders.

## 20. Brand identity

The CareNest mark should communicate family organization, privacy, scheduling and calm utility.

Avoid symbols that imply official medical accreditation/certification.

## 21. Current logo/resource variants

Use the actual version-controlled resources present in `src/CareNest.App/Resources/` for app icon, splash and CareNest mark variants.

Do **not** document or create a packaged BMC/project-funding badge as part of the current application design system. The prior URL-bearing funding artwork was intentionally removed from application resources after package inspection found the destination embedded in Windows payload bytes.

Repository support documentation may use ordinary text/links outside the distributed application package.

## 22. Watermark

Project branding can use:

`Made by the Sanskar`

Appropriate contexts include project/splash/About/footer/creator surfaces where it does not interfere with user health content, accessibility or store policy.

## 23. Repository project-support presentation

Canonical repository-only voluntary support destination:

`https://buymeacoffee.com/sanskarIN`

Rules:

- repository documentation/metadata only for current application boundary;
- not a medical purchase;
- not premium reminder delivery;
- not emergency/clinical support;
- not access to user records;
- not an app feature entitlement.

See `BUY_ME_A_COFFEE.md` and `docs/SUPPORT_CARENEST.md`.

## 24. Responsive design

Layouts should tolerate:

- narrow phones;
- larger phones/tablets;
- resizable desktop windows;
- text scaling;
- translated text expansion.

Prefer flexible Grid/Stack layouts and scrolling for growable content.

## 25. Desktop interaction

Windows/Mac Catalyst primary flows should support expected pointer/keyboard interaction and must not require touch-only gestures.

## 26. Accessibility

See `docs/design/ACCESSIBILITY.md`.

Core requirements include semantic labels, logical focus order, keyboard navigation, large text, contrast, reduced motion and color-independent meaning.

Source semantics do not replace real assistive-technology testing.

## 27. Localization

Strings should be localization-ready and avoid unnecessary concatenated fragments.

Dates/times shown to users should respect locale where appropriate; machine-readable exports use stable/invariant formats where required.

See `docs/design/LOCALIZATION.md`.

## 28. Store assets

Use synthetic/fictional data in screenshots.

Store assets must not expose real health information, imply clinical capability or depict a removed in-app funding feature.

See `docs/design/STORE_ASSETS.md`.

## 29. Strict XAML design/development rule

Binding-bearing UI is compiled with strict type information:

- root `x:DataType`;
- item-specific DataTemplate `x:DataType`;
- typed picker display bindings;
- typed explicit Source/ancestor bindings;
- `XC0022`–`XC0025` as errors.

Design examples/documentation should not recommend patterns that conflict with this build policy.

## 30. Design review checklist

For each major UI change verify:

- hierarchy remains clear;
- text scales/wraps;
- semantic labels exist;
- state is not color-only;
- destructive actions are distinct;
- theme contrast works;
- reduced motion is respected;
- medical/reminder wording remains accurate;
- privacy-sensitive values are minimized;
- mobile/desktop layouts remain usable;
- no removed external funding app surface is reintroduced;
- strict XAML compiled-binding policy remains valid.

## Related documents

- `docs/design/ACCESSIBILITY.md`
- `docs/design/LOCALIZATION.md`
- `docs/design/STORE_ASSETS.md`
- `docs/USER_GUIDE.md`
- `docs/FEATURE_REFERENCE.md`
- `docs/KNOWN_LIMITATIONS.md`
- `docs/releases/MANUAL_TEST_MATRIX.md`