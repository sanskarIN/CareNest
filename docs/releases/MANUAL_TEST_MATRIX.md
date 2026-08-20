# CareNest Manual Release Test Matrix

**Release line:** `1.0.0-rc.1`  
**Current automated evidence authority:** `docs/releases/AUTOMATED_BASELINE.md`  
**Production evidence standard:** `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`  
**Production evidence index:** `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

Use fictional/synthetic data only. This matrix covers behavior that source compilation/tests cannot prove on real packages/devices.

Do not pin a moving accepted SHA/test total here. Read the latest exact-source automated result from `docs/releases/AUTOMATED_BASELINE.md`.

For release evidence, prefer release-specific copies of the canonical templates indexed by `PRODUCTION_EVIDENCE_INDEX.md`. This matrix is a coverage map, not proof by itself.

## Evidence for every completed row

Record:

- app version/build;
- exact source SHA/tag;
- package filename/checksum where applicable;
- structured package evidence JSON where applicable;
- device/emulator/simulator/host identity;
- OS version;
- date/time zone;
- permission/battery/alarm state when relevant;
- install/upgrade path;
- result state from the production evidence standard;
- short non-sensitive observation;
- issue/fix reference for failures/blockage.

Never put real prescriptions, health documents, backups, PINs/passwords/keys, signing secrets, tokens, recovery codes or private health information in public evidence.

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
| Future/overdue snooze | ☐ | ☐ | ☐ | ☐ | Effective due time follows explicit snooze state. |
| Taken/Skipped/Delayed/Missed | ☐ | ☐ | ☐ | ☐ | Platform request cancellation/state transition follows current contract. |
| Snooze replacement | ☐ | ☐ | ☐ | ☐ | Old request cancelled; replacement uses future snooze time. |
| Reminder recovery | ☐ | ☐ | ☐ | ☐ | Failure does not silently strand contradictory state. |
| Schedule edit/delete cleanup | ☐ | ☐ | ☐ | ☐ | Obsolete requests are reconciled. |
| Quiet hours | ☐ | ☐ | ☐ | ☐ | User-defined suppression respected. |
| Appointment CRUD/reminder | ☐ | ☐ | ☐ | ☐ | Local lifecycle/reminder state consistent. |
| Calendar export | ☐ | ☐ | ☐ | ☐ | Explicit user action only. |
| Document import/open/export/delete | ☐ | ☐ | ☐ | ☐ | Encrypted local storage; explicit plaintext export boundary. |
| Reports/CSV/PDF/JSON | ☐ | ☐ | ☐ | ☐ | Correct output; safety/privacy wording preserved. |
| Stock/refill | ☐ | ☐ | ☐ | ☐ | Uses user-entered quantities only. |
| Encrypted backup | ☐ | ☐ | ☐ | ☐ | Password-protected backup created to selected destination. |
| Wrong/tampered/truncated/trailing backup | ☐ | ☐ | ☐ | ☐ | Restore rejected safely. |
| Clean-install restore | ☐ | ☐ | ☐ | ☐ | Structured data/documents restore consistently. |
| Reset all local data | ☐ | ☐ | ☐ | ☐ | Explicit confirmation; CareNest-owned data cleared. |
| System/light/dark theme | ☐ | ☐ | ☐ | ☐ | Readable/usable. |
| Large text/scaling | ☐ | ☐ | ☐ | ☐ | Core actions remain reachable. |
| Keyboard/focus | N/A/☐ | ☐ | N/A/☐ | ☐ | Desktop-capable flows usable. |
| Screen-reader semantics | ☐ | ☐ | ☐ | ☐ | Meaningful accessible names/order. |
| Reduced motion | ☐ | ☐ | ☐ | ☐ | CareNest-controlled motion respects preference. |
| Offline core use | ☐ | ☐ | ☐ | ☐ | Core local-first flows work without CareNest backend. |
| About/legal/support contacts | ☐ | ☐ | ☐ | ☐ | Intended repository/creator/business/support/privacy/terms/security surfaces available. |
| No in-app BMC surface | ☐ | ☐ | ☐ | ☐ | No BMC destination/card/action/artwork. |
| No in-app Gumroad surface | ☐ | ☐ | ☐ | ☐ | No Gumroad destination/card/action/artwork. |

## Packaged SQLite/data compatibility

Mandatory before production promotion with representative fictional/synthetic prior data.

| Scenario | Android | Windows | iOS/iPadOS | Mac Catalyst | Expected result |
|---|---:|---:|---:|---:|---|
| Upgrade/open representative existing database | ☐ | ☐ | ☐ | ☐ | Opens without corruption/record loss. |
| Profiles/medicines/schedules | ☐ | ☐ | ☐ | ☐ | Readable/editable with unchanged meaning. |
| Reminder occurrences/logs | ☐ | ☐ | ☐ | ☐ | Readable; reconciliation succeeds. |
| Appointments/stock/tags/settings | ☐ | ☐ | ☐ | ☐ | Readable/editable. |
| Existing encrypted document | ☐ | ☐ | ☐ | ☐ | Opens through unchanged required key path. |
| New encrypted document | ☐ | ☐ | ☐ | ☐ | Import/open/export/delete works. |
| Genuine historical backup where available | ☐ | ☐ | ☐ | ☐ | Restores when documented compatibility permits. |
| New backup/clean restore | ☐ | ☐ | ☐ | ☐ | Current backup round trip succeeds. |
| Reminder rebuild after upgrade | ☐ | ☐ | ☐ | ☐ | No duplicate/stale requests. |
| SQLite integrity check | ☐ | ☐ | ☐ | ☐ | Passes on representative packaged data. |

Use `docs/releases/templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md` for the actual release-specific evidence.

## Android reminder reliability

- [ ] Notification permission timing/denied/granted behavior tested.
- [ ] Exact alarm unavailable behavior/diagnostic accurate.
- [ ] Battery optimization/vendor restriction behavior documented.
- [ ] Reboot rebuild tested.
- [ ] Manual clock/time-zone/DST change tested.
- [ ] Force-stop/reopen limitation/recovery tested.
- [ ] Future snooze behavior tested.
- [ ] Handled-state cancellation/compensation behavior tested.
- [ ] Snooze replacement tested.
- [ ] Schedule edit/delete stale-request cleanup tested.

## Windows reminder/lifecycle

- [ ] Running-app reminder behavior.
- [ ] Closed-app limitation behavior/messaging.
- [ ] Startup after missed time.
- [ ] Same-ID timer replacement/cancellation.
- [ ] Handled-state behavior.
- [ ] Snooze replacement.
- [ ] Settings/diagnostic wording accurate.

## iOS/iPadOS and Mac Catalyst notifications

- [ ] Permission denied/granted.
- [ ] Real notification delivery on representative device/host where applicable.
- [ ] Foreground/background/lifecycle transitions.
- [ ] Time-zone change/reopen.
- [ ] Future snooze effective due time.
- [ ] Handled actions/reconciliation.
- [ ] Notification tap/action privacy/expected context.

Simulator compilation is not a substitute for real iPhone/iPad notification evidence.

## Security/privacy manual checks

- [ ] Logs contain no health document content, backup password, plaintext PIN, key material or unnecessary sensitive data.
- [ ] Notification copy is privacy-minimized.
- [ ] Export/share destinations require explicit user action.
- [ ] CareNest-owned temporary exports/reports are cleaned according to documented lifecycle.
- [ ] Wrong/tampered/truncated/trailing backup fails safely.
- [ ] Encrypted document payload is not stored as original plaintext in app-owned vault storage.
- [ ] App lock cold start works after process termination.
- [ ] No CareNest account/network requirement appears in normal local-first flows.
- [ ] Application package/source exposes no external BMC destination.
- [ ] Application package/source exposes no external Gumroad destination.
- [ ] Purchase/funding state does not change health/reminder behavior or local-health-data access.

Repository-only markers retained for final package scanning:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

## Accessibility/manual UX

- [ ] Representative large text keeps primary/destructive actions reachable.
- [ ] Screen-reader order follows logical task order.
- [ ] Interactive controls have meaningful names/roles/states.
- [ ] Desktop focus indicators are visible.
- [ ] Color is not the only state/error signal.
- [ ] Light/dark/system contrast is readable.
- [ ] Reduced motion is respected where applicable.
- [ ] Errors are actionable/privacy-safe.

Use `docs/releases/templates/ACCESSIBILITY_VALIDATION_RECORD.md` for actual evidence.

## Final package inspection

For each production candidate:

- [ ] Exact source SHA/tag recorded.
- [ ] Package filename/version/identity recorded.
- [ ] SHA-256 recorded.
- [ ] Signing/notarization/store provenance recorded without secrets.
- [ ] `buymeacoffee.com/sanskarIN` marker scan passed.
- [ ] `ramsandesh.gumroad.com` marker scan passed.
- [ ] Structured package evidence JSON generated with `build/scripts/create-package-evidence.py --stage production`.
- [ ] Package evidence payload SHA-256 independently cross-checked.
- [ ] Installed runtime has no BMC/Gumroad promotional surface.
- [ ] Intended support/legal links work.
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

Use `docs/releases/templates/STORE_SUBMISSION_RECORD.md` to distinguish submission, review, rejection, approval and publication.

## Sign-off rule

A row is complete only when actually tested/evidenced. `N/A` requires a defensible reason under `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`.

Current automation is separate from this manual matrix and is owned by `docs/releases/AUTOMATED_BASELINE.md`. CareNest remains `1.0.0-rc.1` until applicable release-specific evidence is complete.
