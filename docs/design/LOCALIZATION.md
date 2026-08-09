# Localization architecture

CareNest ships English (`en`) first. User-facing strings that define product identity and safety boundaries are backed by `Resources/Strings/AppResources.resx` and exposed through `AppText`.

New languages must use sibling satellite resources such as `AppResources.hi.resx` and must preserve the meaning of safety and privacy statements. Contributors should gradually move all user-facing strings into the resource catalog as features evolve. Never translate medicine names, strength text, instruction text, clinician names, notes, or other user-entered content automatically.

Layout rules:
- allow text wrapping and dynamic height;
- do not concatenate translated sentence fragments;
- keep controls usable at large system text sizes;
- test right-to-left layout before shipping an RTL culture;
- localize dates/numbers only for presentation, never mutate persisted ISO/UTC values.

English is the only enabled application language in release 1.0.0-rc.1. The app follows the device UI culture for resource lookup and is ready for additional `.resx` satellite resources without changing the persistence model.
