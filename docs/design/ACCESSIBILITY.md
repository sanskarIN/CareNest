# CareNest Accessibility Specification

Accessibility is part of CareNest release quality, not a post-release decoration. Automated source/XAML contracts are preventive checks; real assistive-technology evidence remains a manual production gate.

## Scope

Accessibility requirements apply to onboarding, dashboard, profiles, medicine/schedule/reminder/log flows, appointments, documents, reports, settings/diagnostics, app lock and About/legal/support-contact surfaces.

## Core principles

CareNest UI should:

- remain understandable without color alone;
- expose meaningful accessible names/semantics;
- preserve readable text under increased scaling;
- maintain keyboard/focus usability on desktop targets;
- avoid unnecessary motion/respect reduced motion where implemented;
- provide actionable validation/error text;
- keep medical/reminder limitations reachable;
- avoid hover-only/tool-tip-only safety information.

## Text scaling

Manual testing should verify representative large-text settings.

Check that titles/labels/buttons/validation/navigation/cards/lists/limitations/legal/support content wrap and remain reachable rather than clipping behind fixed dimensions.

## Screen readers

Representative testing should include platform tools such as TalkBack, VoiceOver and Narrator where applicable.

Verify:

- controls have meaningful names;
- decorative images do not duplicate announcements;
- icon-only actions are described;
- inputs/validation are discoverable;
- state changes/reading order are logical;
- medical/privacy/reminder limitation content is reachable;
- repository/legal/support-contact actions are understandable.

The current app has **no in-app Buy Me a Coffee funding action/card**, so accessibility testing must not expect or advertise one.

## Semantic labels

Prefer purpose-based descriptions:

- `Save medicine` rather than `check icon`;
- `Open CareNest repository` rather than `GitHub image`;
- `Delete document` rather than `trash`.

Do not add semantics for features that do not exist in the shipped UI.

## Keyboard/focus

Windows/Mac Catalyst flows should support expected keyboard interaction where platform controls permit.

Test Tab/Shift+Tab progression, activation, visible focus, modal focus containment, list/editor transitions and destructive-action placement.

## Touch/click targets

Interactive controls should remain comfortably usable across touch/pointer input; avoid tiny icon-only areas, especially for destructive/reminder/document actions.

## Contrast/themes

Verify system/light/dark presentation for text, disabled state, validation, focus indicators, links, card/status meaning and destructive/warning states.

Status must never rely on color alone.

## Reminder/status semantics

States such as Scheduled, Snoozed, Taken, Skipped, Delayed, Missed and Cancelled need text/semantic meaning in addition to color/iconography.

Stock/refill state also needs explicit text.

## Reduced motion

Motion must not be required to understand navigation, state, validation, privacy or medical/reminder limitations. Reduced-motion preference should suppress nonessential CareNest-controlled motion where implemented.

## Errors/validation

Errors should be plain, actionable, privacy-safe, readable with large text and not represented only by a red border.

Do not expose raw stack traces, internal paths, keys, passwords/PINs or sensitive record data.

## Reminder safety messaging

Assistive technologies must be able to discover that:

- reminder delivery is not guaranteed;
- OS permission/battery/background restrictions can affect delivery;
- CareNest does not calculate dosage/treatment;
- as-needed schedules do not create automatic reminders;
- invalid DST-gap times are not silently replaced.

## Destructive actions

Screen-reader/keyboard users must understand what will be deleted/replaced, whether it can be undone and what backup/export option exists where applicable.

## App lock

App-lock UI should have accessible input/failure feedback without exposing verifier/salt/crypto details. It must remain keyboard-usable on desktop targets.

## Documents/reports

Document import/export/delete actions need clear names. Avoid reading unrelated sensitive internal paths.

The application UI around reports/exports must be accessible. Accessible/tagged PDF output is not automatically claimed unless explicitly implemented/tested.

## Automated coverage

Current source-policy/UI tests help protect XAML semantics, strict compiled bindings, route/navigation expectations, safety/resource wording and other stable source requirements.

They cannot prove real screen-reader usability.

## Required manual evidence

Before final production record representative evidence for:

- large text/scaling;
- screen readers;
- keyboard/focus;
- themes/contrast;
- reduced motion;
- color-independent meaning;
- validation/error messaging;
- destructive confirmations;
- medical/privacy/reminder wording availability.

Use `docs/releases/MANUAL_TEST_MATRIX.md`.

## Contributor regression checklist

When changing XAML/ViewModels:

- preserve/add semantic labels;
- test long/localized text assumptions;
- avoid clipping fixed dimensions;
- pair color/icons with text;
- preserve keyboard focus order;
- avoid gesture-only primary actions;
- keep strict compiled-binding requirements valid;
- update source-policy/manual matrices for new interactions.

## Localization interaction

Localized visible text and accessibility labels must remain semantically aligned. Repeat large-text/screen-reader review for each shipping locale.

See `docs/design/LOCALIZATION.md`.

## Current limitation

Full real-device assistive-technology validation remains a manual production-release gate. A green PR #74 source matrix is not accessibility certification.