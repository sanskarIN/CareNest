# CareNest Browser/WebAssembly Validation Record

Use fictional/synthetic data only. This canonical template is not evidence until a release-specific copy is completed against an actual published WebAssembly site in named browsers.

The current Avalonia Browser host establishes configured WebAssembly presentation/build reach. Do not mark native or full-feature parity `PASS` unless the behavior is actually implemented for the browser sandbox and exercised against the published candidate.

## Identity

- Result status: `NOT RUN`
- CareNest version/build:
- Source SHA:
- Source tag:
- Published site path/artifact:
- Published artifact SHA-256/provenance reference:
- Hosting origin/environment:
- HTTPS status:
- Browser name/version:
- Browser engine:
- Operating system/device:
- Validation date/time/time zone:
- Operator:

## Publish and hosting boundary

- [ ] `CareNest.CrossPlatform.Browser` Release publish succeeds from the recorded source.
- [ ] Static publish output is attributable to the recorded source/toolchain.
- [ ] The site is served over an intended origin with correct WebAssembly/static-file behavior.
- [ ] `.wasm` resources load successfully without console/network failures that block startup.
- [ ] Browser cache/service-worker behavior, if introduced, is documented and validated rather than assumed.
- [ ] Repository-only Gumroad/Buy Me a Coffee promotion is not introduced into the distributed CareNest browser host.

Evidence/notes:

## Launch and presentation

- [ ] Application starts in the named browser/version.
- [ ] Main view renders without startup exception or visibly broken layout.
- [ ] Keyboard focus is visible on representative interactive controls.
- [ ] Browser zoom and representative viewport sizes remain usable.
- [ ] Refresh/reload behavior is recorded accurately.
- [ ] The UI does not claim native or production feature parity that has not been implemented/validated.
- [ ] Medical/reminder limitations remain visible and accurate where surfaced.

Evidence/notes:

## Browser capability and parity boundary

For each capability, record `PASS`, `FAIL`, `BLOCKED`, `N/A`, or `NOT RUN`. `PASS` means a browser-specific implementation exists and was actually exercised on this published candidate; native MAUI behavior cannot be copied forward as browser evidence.

- Profiles/data persistence:
- Medicines/schedules:
- Reminder/notification delivery and actions:
- Background execution:
- Appointments:
- Document vault/import/export:
- Backup/restore/download/upload:
- Reports/export/share:
- App lock/secret storage:
- File picker/camera integration:
- Accessibility/assistive technology:

Evidence/notes:

## Browser storage and privacy

- [ ] Actual persistence mechanism is identified and its browser-clearing/private-mode behavior is recorded.
- [ ] Storage quota/denial behavior is handled clearly where applicable.
- [ ] Cross-origin/network behavior matches the documented local-first/account-free boundary.
- [ ] No hidden analytics/telemetry/network upload is introduced by the browser host or hosting configuration.
- [ ] Secret-storage claims are not made unless the browser-specific implementation and threat boundary were validated.
- [ ] Sensitive content is not exposed through URLs, query strings, console logging or unintended browser storage.

Evidence/notes:

## Browser lifecycle and failure behavior

- [ ] Reload/navigation-away behavior is recorded for data that is actually implemented.
- [ ] Offline/network-loss behavior is recorded rather than inferred.
- [ ] Unsupported notification/background/file-system capabilities fail clearly or remain unavailable.
- [ ] Permission denial behavior is recorded for any browser permission actually requested.
- [ ] Multiple-tab behavior is recorded where it can affect persistence/state.
- [ ] Browser storage clearing produces understandable behavior without false recovery claims.

Evidence/notes:

## Accessibility and responsive behavior

- [ ] Keyboard-only navigation is tested for the flows present in this host.
- [ ] Focus order and focus visibility are usable.
- [ ] Browser zoom at representative levels is usable without critical content loss.
- [ ] Screen-reader behavior is tested for representative controls where applicable.
- [ ] Status meaning is not color-only.
- [ ] Applicable results are linked to a release-specific accessibility record.

Evidence/notes:

## Privacy and safety

- [ ] Validation uses fictional/synthetic health-organizer data only.
- [ ] No password, PIN, private key, signing secret, token, recovery code, real prescription, real medical record or private backup is committed as evidence.
- [ ] Unsupported browser capabilities are not represented as working native capabilities.
- [ ] Local-first/account-free claims match the behavior actually observed.

Evidence/notes:

## Browser matrix

Add one row per actually tested browser. Do not mark untested browsers as pass.

| Browser/version | OS/device | Startup | Layout/zoom | Persistence | Permissions/capabilities | Overall |
| --- | --- | --- | --- | --- | --- | --- |
| Not run | Not run | `NOT RUN` | `NOT RUN` | `NOT RUN` | `NOT RUN` | `NOT RUN` |

## Failures/blockers

List every `FAIL`, `BLOCKED` or `N/A` row with reason and issue/PR reference where applicable.

## Final result

- Overall result: `NOT RUN`
- Blocking issue references:
- Retest artifact/source if required:
- Reviewer/sign-off:
