# CareNest Accessibility Specification

Accessibility is part of the CareNest release quality model, not a post-release decoration. This document defines intended UI behavior and the manual/automated checks required before public production promotion.

## Scope

Accessibility requirements apply to:

- onboarding;
- dashboard;
- profiles;
- medicine editor/list;
- schedule editor;
- reminder/log screens;
- appointments;
- document organizer;
- reports;
- settings/diagnostics;
- app-lock screen;
- About/legal/support surfaces.

## Core principles

CareNest UI should:

- remain understandable without relying on color alone;
- expose meaningful semantic/accessibility labels;
- preserve readable text under increased scaling;
- maintain keyboard/focus usability on desktop targets;
- avoid unnecessary motion and respect reduced-motion preference where implemented;
- provide actionable validation/error text;
- keep medical/reminder limitations readable;
- avoid hiding safety-critical limitations in hover-only/tool-tip-only interactions.

## Text scaling

Manual testing should verify representative large-text settings on each applicable platform.

Check:

- titles do not overlap controls;
- labels remain associated with inputs;
- buttons remain tappable/clickable;
- long validation text wraps;
- navigation remains usable;
- cards/lists expand rather than clipping essential text;
- medical/reminder warning text remains visible;
- support/legal links remain reachable.

Avoid fixed-height containers for content that can grow with user font settings unless scrolling/expansion safely accommodates the content.

## Screen readers

Representative screen-reader testing should include:

- Android TalkBack;
- iOS/iPadOS VoiceOver;
- macOS VoiceOver where applicable;
- Windows Narrator where applicable.

Verify:

- interactive controls have meaningful names;
- decorative images do not create confusing duplicate announcements;
- icon-only controls have accessible descriptions;
- input purpose is understandable;
- validation errors can be discovered;
- state changes are communicated where practical;
- navigation order is logical;
- support artwork/link exposes understandable action text;
- medical warning content is reachable.

## Semantic labels

XAML semantic contract tests supplement manual screen-reader testing.

Semantic text should describe action/purpose, not only visual appearance.

Good intent examples:

- `Save medicine` rather than `check icon`;
- `Open CareNest project support` rather than `coffee image`;
- `Delete document` rather than `trash`.

## Keyboard navigation

Windows and Mac Catalyst flows should be usable with keyboard where controls/platform support permits.

Manual checks include:

- Tab/Shift+Tab focus progression;
- activation with expected keyboard controls;
- visible focus indication;
- no unreachable primary action;
- modal/dialog focus does not disappear behind the dialog;
- Escape/back behavior remains predictable where platform conventions support it;
- list/editor transitions do not trap focus.

## Focus order

Focus should follow task order rather than visual implementation order.

Typical editor sequence:

1. heading/context;
2. primary required fields;
3. optional fields;
4. schedule/configuration controls;
5. explanatory/safety text;
6. save/cancel actions.

Destructive actions should not be the accidental first focus target when safer primary actions exist.

## Touch/click target size

Interactive targets should remain comfortably usable across touch devices and desktop pointer input.

Avoid tiny icon-only hit areas, especially for destructive actions, reminder state changes, and document export/delete controls.

## Contrast

Verify light/dark/system themes under representative platform settings.

Requirements:

- primary/secondary text remains legible;
- disabled state remains distinguishable without becoming unreadable;
- validation text contrasts with background;
- focus indicators are visible;
- links remain identifiable;
- card boundaries/status chips remain understandable;
- warning/error/success states are not communicated only by color.

## Color-independent status

Reminder states such as Scheduled, Snoozed, Taken, Skipped, Delayed, and Missed must be communicated by text/semantics in addition to any color treatment.

Stock/refill status must also include text, not only a color threshold.

## Reduced motion

CareNest includes a reduced-motion preference in its design model.

When reduced motion is enabled:

- avoid decorative animation where it is not necessary;
- avoid motion that is required to understand state;
- preserve all actions/content without animation;
- do not delay safety/validation text behind animated sequences.

Manual testing must verify that the preference does not break navigation or leave content hidden.

## Theme behavior

Supported theme concepts:

- system;
- light;
- dark.

Changing theme should not reset local health-organizational data or rewrite schedule configuration.

Verify each major workflow in at least light/dark presentation and ensure system theme transitions are readable.

## Validation and error messaging

Errors should:

- use plain actionable language;
- avoid raw exception messages/stack traces;
- identify the user-correctable field/rule where appropriate;
- not expose sensitive local health content unnecessarily;
- remain readable with large text;
- not rely solely on a red border.

## Reminder safety messaging

Accessibility applies to limitation text too.

The following concepts must remain available to assistive technologies:

- reminder delivery is not guaranteed;
- permission/battery/OS restrictions can affect notifications;
- CareNest does not calculate dosage or treatment;
- as-needed records do not generate automatic reminders;
- invalid DST-gap times are not silently replaced.

## Destructive actions

Delete/reset/restore operations should use explicit labels and confirmation patterns.

Screen-reader users must be able to determine:

- what will be deleted/replaced;
- whether the action can be undone;
- what backup/export option exists before destructive action where applicable.

## App lock

App-lock entry should:

- provide an accessible input description;
- avoid announcing stored verifier/salt details;
- provide understandable failure feedback without revealing sensitive cryptographic data;
- preserve keyboard/accessibility behavior on desktop targets.

## Document workflows

Document import/export/delete controls must expose clear semantic actions.

File names can themselves be sensitive; avoid reading unrelated internal paths/diagnostic information into accessibility descriptions.

## Reports

Generated PDF/CSV exports are primarily data/report artifacts. The application UI used to select/generate/export them must be accessible.

If future versions add tagged/accessible PDF requirements, that should be tracked as a separate explicit export-format capability and tested accordingly rather than assumed.

## Buy Me a Coffee support surface

The project-support image/button should have accessible text describing the voluntary project-support action.

It must not be described as purchasing health functionality or medical assistance.

## Automated coverage

Current automated UI/repository contract tests help protect:

- XAML semantic/accessibility presence;
- route/navigation expectations;
- safety/resource wording;
- support-surface accessibility intent;
- no accidental removal of required safety content.

Automated source/XAML inspection cannot prove real screen-reader usability.

## Required manual evidence

Before final public `1.0.0`, record evidence in `docs/releases/MANUAL_TEST_MATRIX.md` for:

- large text;
- representative screen readers;
- keyboard navigation;
- focus order;
- light/dark/system themes;
- reduced motion;
- contrast/readability;
- validation/error messaging;
- destructive confirmation readability;
- support/safety wording availability.

Do not mark these checks complete solely because CI is green.

## Accessibility regression checklist for contributors

When changing XAML/ViewModels:

- preserve/add semantic labels for new controls;
- test long localized text assumptions;
- avoid fixed dimensions that clip text;
- include text with status colors/icons;
- maintain keyboard focus order;
- do not hide primary actions behind gestures only;
- update UI-contract tests where they protect a stable requirement;
- update `MANUAL_TEST_MATRIX.md` if a new interaction needs manual assistive-technology verification.

## Localization interaction

Accessible labels and visible strings should be localization-ready. Do not concatenate translated fragments in a way that creates unnatural screen-reader output.

See `docs/design/LOCALIZATION.md`.

## Known limitation

The repository currently treats full real-device assistive-technology testing as a manual production-release gate. Automated tests are preventive contracts, not certification of accessibility compliance.