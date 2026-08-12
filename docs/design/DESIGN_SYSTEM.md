# CareNest Design System

CareNest aims for calm, clear, accessible, non-clinical organization across Android, iOS, Mac Catalyst, and Windows.

The design must not imply medical certification, diagnosis, treatment authority, or emergency-service capability.

## Design principles

1. **Calm over alarming.** Health organization can already be stressful; normal states should not look like emergencies.
2. **Clarity over decoration.** Primary actions, dates, reminder state, and limitations should be easy to scan.
3. **Accessible by default.** Text scaling, semantics, contrast, keyboard/focus, and reduced motion are release concerns.
4. **Privacy-aware.** Avoid unnecessary sensitive text in notification-like/public surfaces.
5. **Non-clinical framing.** UI organizes user-entered information without presenting clinical conclusions.
6. **Cross-platform consistency.** Keep CareNest identity stable while respecting platform conventions.

## Spacing tokens

Base spacing scale:

- 4 — micro spacing;
- 8 — compact internal spacing;
- 12 — small control/group spacing;
- 16 — standard content spacing;
- 24 — section spacing;
- 32 — large separation/hero spacing.

Prefer token values rather than one-off arbitrary spacing.

## Corner radii

- 10 — controls/compact interactive surfaces;
- 16 — cards/standard grouped surfaces;
- 24 — hero/large highlighted surfaces.

Avoid excessive nested rounded containers that reduce information hierarchy.

## Touch/click targets

Minimum intended touch target: **44×44 logical units**.

Icon-only actions should still expose a target large enough for touch and meaningful accessibility text.

Destructive actions require clear labeling and should not be placed so closely to common primary actions that accidental activation becomes likely.

## Typography

Use platform-default/scalable typography rather than fixed pixel assumptions.

Reference scale:

- title: 28;
- section heading: 20;
- body: 16;
- caption/supporting text: 13.

Text must wrap when localized or scaled.

Avoid fixed-height layout assumptions around safety/validation text.

## Text hierarchy

Recommended order:

1. screen/page title;
2. short contextual description if needed;
3. grouped content;
4. supporting/limitation text;
5. primary action;
6. secondary/destructive action.

Medication instruction/strength text is user content and must not be visually transformed into a CareNest dosage recommendation.

## Color roles

Use semantic roles instead of hard-coded meaning tied to one color:

- background;
- surface;
- elevated/highlighted surface;
- primary;
- on-primary;
- primary text;
- muted text;
- divider/border;
- success;
- warning;
- danger;
- link/action;
- focus indicator.

Never rely on color alone to convey reminder/validation/stock state.

## Status presentation

Reminder states can include:

- Scheduled;
- Snoozed;
- Taken;
- Skipped;
- Delayed;
- Missed.

Every state must include text or accessible semantics in addition to any color/icon.

Stock/refill state must also include text describing the local estimate/threshold rather than only green/amber/red color.

## Themes

CareNest supports:

- system;
- light;
- dark.

Theme changes must not:

- reset local data;
- rewrite reminder schedule intent;
- hide medical/reminder limitations;
- make validation/focus indicators unreadable.

Manual release testing checks light/dark/system behavior.

## Motion

Motion should be short and optional.

Motion must never be required to understand:

- reminder state;
- validation error;
- destructive action result;
- privacy/medical warning;
- navigation availability.

Reduced-motion preference disables/minimizes decorative transitions where implemented.

## Cards

Cards are appropriate for grouped information such as:

- profile summaries;
- medicine summary;
- upcoming reminder summary;
- appointment summary;
- document metadata;
- settings group;
- About/support surfaces.

A card should represent one coherent unit rather than acting as a decorative container for every line.

## Forms/editors

Forms should:

- clearly label required/optional fields;
- keep label close to control;
- allow multi-line wrapping;
- provide inline actionable validation;
- avoid raw exception text;
- separate destructive actions from save;
- keep user-entered medicine text clearly distinct from application-generated limitation text.

## Schedule editor design

Schedule editor should make explicit user intent visible.

Users should be able to tell which values they entered:

- schedule kind;
- start/end dates;
- explicit time zone;
- reminder times;
- weekday selection;
- every-N-hours interval/start;
- cycle on/off days;
- follow-up minutes;
- enabled state.

Do not use UI wording that implies CareNest chose a medically appropriate frequency.

As-needed must clearly indicate that no automatic reminder occurrences are created.

## Reminder state controls

Taken/Skipped/Delayed/Snooze/Missed actions should be clearly distinguishable and not represented as clinical recommendations.

Snooze UI must result in an explicit future time.

## Destructive actions

Delete/reset/restore-replacement actions should:

- use explicit action names;
- describe what local data is affected;
- use confirmation where appropriate;
- avoid accidental placement near routine save controls;
- mention backup/export option when that helps avoid unintended data loss.

## Empty states

Empty states should explain what the user can do without creating urgency or medical advice.

Examples of intent:

- no medicines recorded;
- no upcoming reminders;
- no documents imported;
- no appointments recorded.

Avoid wording that implies missing records are medically unsafe.

## Validation/error states

Errors should be:

- human-readable;
- actionable;
- privacy-safe;
- accessible;
- independent of color alone.

Do not expose:

- raw stack traces;
- database paths;
- encryption keys;
- backup password details;
- internal record IDs unnecessarily.

## Notification copy

Notification content is privacy-minimized.

Generic labels are preferred by default.

Notification wording must not claim:

- verified adherence;
- dosage recommendation;
- clinical urgency;
- guaranteed delivery.

## Medical limitation surfaces

Medical/reminder limitations appear in onboarding/About/reports/documentation and should remain readable/accessibility-reachable.

Core concepts:

- organizational only;
- no diagnosis/treatment/dosage inference;
- no emergency service;
- reminders can be affected by OS/permission/battery restrictions.

## Brand identity

The CareNest logo combines a gentle nest/shield concept with a small calendar/check organization cue.

Design intent:

- family care/organization;
- privacy/safety;
- scheduling;
- calm utility.

Avoid medical-accreditation imagery such as a red cross or symbols that imply professional certification.

## Logo variants

Repository assets include:

- adaptive app icon/foreground;
- splash artwork;
- standard CareNest mark;
- light-surface mark;
- dark-surface mark;
- monochrome/system mark;
- compact project-support badge;
- custom CareNest Buy Me a Coffee project-support artwork.

Use the correct variant for the surface/contrast requirement.

## Watermark

Required creator watermark:

`Made by the Sanskar`

Appropriate locations:

- splash/about/footer/creator surfaces.

Do not overlay the watermark on top of user health content in a way that hurts readability/privacy.

## Project-support design

Canonical destination:

`https://buymeacoffee.com/sanskarIN`

Support surfaces should describe voluntary project support.

Do not style them as:

- medical purchase;
- premium diagnosis/treatment;
- paid emergency assistance;
- priority health support;
- paid access to user records.

Custom CareNest support artwork must not be represented as an official Buy Me a Coffee trademark asset.

## Responsive design

Layouts should tolerate:

- narrow phones;
- larger phones/tablets;
- resizable desktop windows;
- text scaling;
- translated text expansion.

Prefer flexible Grid/Stack layouts and scrolling where content can grow.

## Desktop interaction

Windows/Mac Catalyst should support expected pointer/keyboard use.

Primary flows must not require touch-only gestures.

## Accessibility

The complete accessibility specification is in `docs/design/ACCESSIBILITY.md`.

Core requirements include:

- semantic labels;
- screen-reader usability;
- logical focus order;
- keyboard navigation;
- large text;
- contrast;
- reduced motion;
- color-independent state communication.

## Localization

Strings should be localization-ready and avoid unnecessary concatenated fragments.

Dates/times shown to users should respect locale where appropriate, while machine-readable exports use stable/invariant formats where required.

See `docs/design/LOCALIZATION.md`.

## Store assets

Use fictional/synthetic data in screenshots.

Store screenshots must not expose real user health information.

See `docs/design/STORE_ASSETS.md`.

## Design review checklist

For each major UI change verify:

- hierarchy remains clear;
- text scales/wraps;
- semantic labels exist;
- state is not color-only;
- destructive actions are distinct;
- theme contrast works;
- reduced motion is respected;
- medical/reminder boundary wording remains accurate;
- privacy-sensitive values are not added to public/notification surfaces unnecessarily;
- mobile + desktop layouts remain usable.

## Related documents

- `docs/design/ACCESSIBILITY.md`
- `docs/design/LOCALIZATION.md`
- `docs/design/STORE_ASSETS.md`
- `docs/USER_GUIDE.md`
- `docs/FEATURE_REFERENCE.md`
- `docs/releases/MANUAL_TEST_MATRIX.md`