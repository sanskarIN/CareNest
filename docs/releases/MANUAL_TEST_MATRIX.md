# CareNest Manual Release Test Matrix

**Release line:** `1.0.0-rc.1`  
**Latest verified Gumroad implementation/source-policy source:** `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

Use fictional/synthetic data only. This matrix covers behavior that source compilation/tests cannot prove on real packages/devices.

## Evidence for every completed row

Record:

- app version/build;
- exact source SHA/tag;
- package filename/checksum when applicable;
- structured package evidence JSON when applicable;
- device/emulator/simulator model;
- OS version;
- date/time zone;
- permission/battery/alarm state when relevant;
- install/upgrade path;
- pass/fail;
- short non-sensitive observation;
- issue/fix reference for failures.

Never put real prescriptions, health documents, backups, PINs/passwords/keys, signing secrets or private health information in public evidence.

## Cross-platform functional matrix

| Area | Android | Windows | iOS/iPadOS | Mac Catalyst | Expected result |
|---|---:|---:|---:|---:|---|
| Fresh install/onboarding | ☐ | ☐ | ☐ | ☐ | Starts without CareNest account/backend; limitations visible. |
| Profile CRUD | ☐ | ☐ | ☐ | ☐ | Local lifecycle works; destructive actions confirmed. |
| App lock enable/cold start/disable | ☐ | ☐ | ☐ | ☐ | Local privacy barrier works without plaintext PIN storage. |
| Medicine CRUD | ☐ | ☐ | ☐ | ☐ | User text preserved; no dosage inference. |
| Daily/specific-time schedule | ☐ | ☐ | ☐ | ☐ | Explicit times materialize correctly. |
| Selected weekdays | ☐ | ☐ | ☐ | ☐ | Only selected weekdays appear. |
| Every-N-hours | ☐ | ☐ | ☐ | ☐ | Uses explicit interval/start only. |
| Cycle/custom range | ☐ | ☐ | ☐ | ☐ | Date/cycle boundaries honored. |
| As-needed | ☐ | ☐ | ☐ | ☐ | No automatic occurrences. |
| Pause/complete/archive | ☐ | ☐ | ☐ | ☐ | Automatic reminders suppressed/reconciled. |
| Future snooze | ☐ | ☐ | ☐ | ☐ | Snooze remains upcoming until explicit snooze due time. |
| Overdue snooze | ☐ | ☐ | ☐ | ☐ | Effective due time is snooze due time. |
| Taken/Skipped/Delayed/Missed | ☐ | ☐ | ☐ | ☐ | Existing platform request cancelled before handled state commits. |
| Snooze replacement | ☐ | ☐ | ☐ | ☐ | Old request cancelled; replacement uses future snooze time. |
| Reminder recovery | ☐ | ☐ | ☐ | ☐ | Failure does not silently strand contradictory state. |
| Schedule-edit stale cleanup | ☐ | ☐ | ☐ | ☐ | Obsolete platform request reconciled. |
| Medicine/profile delete cleanup | ☐ | ☐ | ☐ | ☐ | Future requests cancelled/recoverable across failure. |
| Quiet hours | ☐ | ☐ | ☐ | ☐ | User-defined suppression respected. |
| Appointment CRUD/reminder | ☐ | ☐ | ☐ | ☐ | Local lifecycle/reminder state consistent. |
| Calendar export | ☐ | ☐ | ☐ | ☐ | Explicit user action only. |
| Document import/open/export/delete | ☐ | ☐ | ☐ | ☐ | Encrypted local storage; explicit plaintext export boundary. |
| Reports/CSV/PDF/JSON | ☐ | ☐ | ☐ | ☐ | Correct output; safety/privacy wording; formula-like CSV input remains text. |
| Stock/refill | ☐ | ☐ | ☐ | ☐ | Uses user-entered quantities only. |
| Encrypted backup | ☐ | ☐ | ☐ | ☐ | Password-protected backup created to selected destination. |
| Wrong/tampered/truncated backup | ☐ | ☐ | ☐ | ☐ | Restore rejected safely. |
| Clean-install restore | ☐ | ☐ | ☐ | ☐ | Structured data/documents restore consistently. |
| Reset all local data | ☐ | ☐ | ☐ | ☐ | Explicit confirmation; CareNest-owned data cleared. |
| System/light/dark theme | ☐ | ☐ | ☐ | ☐ | Readable/usable. |
| Large text/scaling | ☐ | ☐ | ☐ | ☐ | Core actions remain reachable. |
| Keyboard/focus | N/A/☐ | ☐ | N/A/☐ | ☐ | Desktop-capable flows usable. |
| Screen-reader semantics | ☐ | ☐ | ☐ | ☐ | Meaningful accessible names/order. |
| Reduced motion | ☐ | ☐ | ☐ | ☐ | CareNest-controlled motion respects preference. |
| Offline core use | ☐ | ☐ | ☐ | ☐ | Core local-first flows work without CareNest backend. |
| About/legal/support contacts | ☐ | ☐ | ☐ | ☐ | Repository/creator/business/support/privacy/terms/security available. |
| No in-app BMC funding surface | ☐ | ☐ | ☐ | ☐ | Distributed app contains no BMC destination/card/action/artwork. |
| No in-app Gumroad storefront surface | ☐ | ☐ | ☐ | ☐ | Distributed app contains no Gumroad destination/card/action/artwork. |

## Packaged SQLite/data compatibility

Mandatory before production promotion with representative fictional prior data.

| Scenario | Android | Windows | iOS/iPadOS | Mac Catalyst | Expected result |
|---|---:|---:|---:|---:|---|
| Upgrade/open existing RC database | ☐ | ☐ | ☐ | ☐ | Opens without corruption/record loss. |
| Profiles/medicines/schedules | ☐ | ☐ | ☐ | ☐ | Readable/editable with unchanged meaning. |
| Reminder occurrences/logs | ☐ | ☐ | ☐ | ☐ | Readable; reconciliation succeeds. |
| Appointments/stock/tags/settings | ☐ | ☐ | ☐ | ☐ | Readable/editable. |
| Existing encrypted document | ☐ | ☐ | ☐ | ☐ | Opens via unchanged required key path. |
| New encrypted document | ☐ | ☐ | ☐ | ☐ | Import/open/export/delete works. |
| Genuine prior backup fixture where available | ☐ | ☐ | ☐ | ☐ | Restores when documented compatibility permits. |
| New backup/clean restore | ☐ | ☐ | ☐ | ☐ | Current backup round trip succeeds. |
| Reminder rebuild after upgrade | ☐ | ☐ | ☐ | ☐ | No duplicate/stale requests. |
| SQLite integrity check | ☐ | ☐ | ☐ | ☐ | Passes on representative packaged data. |

## Android reminder reliability

- [ ] Notification permission not yet requested until applicable reminder action.
- [ ] Permission denied keeps app usable and surfaces limitation.
- [ ] Permission granted permits delivery subject to OS policy.
- [ ] Exact alarm unavailable behavior/diagnostic accurate.
- [ ] Battery optimization/vendor restriction behavior documented.
- [ ] Reboot rebuild tested.
- [ ] Manual clock/time-zone change tested.
- [ ] Force-stop/reopen limitation/recovery tested.
- [ ] Future snooze crossing original due time tested.
- [ ] Taken/Skipped/Delayed/Missed cancellation-first behavior tested.
- [ ] Snooze replacement tested.
- [ ] Schedule edit/delete stale-request cleanup tested.

## Windows reminder/lifecycle

- [ ] Running-app reminder behavior.
- [ ] Closed-app limitation behavior/messaging.
- [ ] Startup after missed time.
- [ ] Same-ID timer replacement/cancellation.
- [ ] Handled-state cancellation-first behavior.
- [ ] Snooze replacement.
- [ ] Settings/diagnostic wording accurate.

## iOS/iPadOS and Mac Catalyst notifications

- [ ] Permission denied/granted.
- [ ] Real notification delivery on representative device/host.
- [ ] Foreground/background/lifecycle transitions.
- [ ] Time-zone change/reopen.
- [ ] Future snooze effective due time.
- [ ] Handled actions cancellation-first.
- [ ] Snooze replacement/stale reconciliation.
- [ ] Notification tap/action privacy/expected context.

## Security/privacy manual checks

- [ ] Logs contain no health document content, backup password, plaintext PIN, key material or unnecessary sensitive exception data.
- [ ] Notification copy is privacy-minimized.
- [ ] Export/share destinations require explicit user action.
- [ ] App-owned temporary exports/reports are cleaned according to documented lifecycle.
- [ ] Wrong/tampered backup fails safely.
- [ ] Encrypted document payload is not stored as original plaintext in app-owned vault storage.
- [ ] App lock cold start works after process termination.
- [ ] No CareNest account/network requirement appears in normal local-first flows.
- [ ] Repository/browser/legal external actions are explicit.
- [ ] Application package/source exposes no external BMC funding destination.
- [ ] Application package/source exposes no external Gumroad storefront destination.
- [ ] Purchase/funding state does not change health/reminder behavior or local-health-data access.

## Accessibility/manual UX

- [ ] 200%/representative large text keeps primary/destructive actions reachable.
- [ ] Screen-reader order follows logical task order.
- [ ] Interactive controls have meaningful names.
- [ ] Desktop focus indicators are visible.
- [ ] Color is not the only state/error signal.
- [ ] Light/dark/system contrast is readable.
- [ ] Reduced motion is respected.
- [ ] Errors are actionable/privacy-safe.

## Final package inspection

For each signed production candidate:

- [ ] Exact source SHA/tag recorded.
- [ ] Package filename/version/identity recorded.
- [ ] SHA-256 recorded.
- [ ] Signing/notarization/store provenance recorded.
- [ ] `buymeacoffee.com/sanskarIN` marker scan passed.
- [ ] `ramsandesh.gumroad.com` marker scan passed.
- [ ] Structured package evidence JSON generated with `build/scripts/create-package-evidence.py --stage production`.
- [ ] Package evidence payload SHA-256 matches independently recorded package SHA-256/equivalent directory evidence.
- [ ] Installed About/runtime has no BMC funding action/card.
- [ ] Installed About/runtime has no Gumroad storefront/purchase action/card.
- [ ] Support/legal links work as intended.
- [ ] Smoke test passed.

Package evidence guide:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

## Store submission manual evidence

Preliminary policy review:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

- [ ] Submission-date Apple policy/privacy/store review complete where applicable.
- [ ] Submission-date Google Play health/privacy/permissions/payments review complete where applicable.
- [ ] Submission-date Microsoft Store privacy/sensitive-data review complete where applicable.
- [ ] Live Google Play Health apps declaration complete where applicable.
- [ ] Live Google Play Data safety complete where applicable.
- [ ] Apple privacy/store metadata complete where applicable.
- [ ] Microsoft/Partner Center privacy/store metadata complete where applicable.
- [ ] Final listing/screenshots use fictional data and match the exact production package.
- [ ] Final support/privacy/terms/security links verified.

## Sign-off rule

A row is complete only when actually tested/evidenced. `N/A` requires a defensible reason. Do not use `N/A` merely to unblock a release.

The latest verified automated Gumroad implementation/source-policy evidence at `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f` is separate from this manual matrix. Later release-documentation/package-evidence tooling changes require their own exact-source automation before replacing that verified baseline. CareNest remains `1.0.0-rc.1` until applicable rows and the rest of `docs/releases/NEXT_STEPS.md` are complete.
