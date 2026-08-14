# CareNest Manual Release Test Matrix

This matrix covers release checks that cannot be proven by compilation or unit/integration/UI-contract tests alone. Use fictional test data only. Never use a real prescription, health document, backup password, PIN, emergency contact, or other private health information in release evidence.

## Evidence rules

For every completed row, record:

- app version/build;
- exact source commit;
- device or emulator/simulator model;
- operating-system version;
- date/time zone;
- notification/battery permission state when relevant;
- package/install/upgrade path when relevant;
- pass/fail result;
- a short non-sensitive observation;
- issue/commit reference for any failure.

Screenshots and logs must contain fictional data. Do not upload decrypted user documents or private health records to GitHub issues.

## Cross-platform functional matrix

| Area | Android | Windows | iOS | Mac Catalyst | Expected result |
|---|---:|---:|---:|---:|---|
| Fresh install/onboarding | ☐ | ☐ | ☐ | ☐ | Opens without account/network requirement; medical/privacy limitations visible. |
| Create/edit/delete profile | ☐ | ☐ | ☐ | ☐ | Local profile lifecycle works and deletion confirms destructive action. |
| App lock enable/cold start/disable | ☐ | ☐ | ☐ | ☐ | Lock protects app entry without storing plaintext PIN. |
| Add medicine | ☐ | ☐ | ☐ | ☐ | User-entered name/strength/instructions are preserved; no dosage inference. |
| Daily schedule | ☐ | ☐ | ☐ | ☐ | Explicit selected times preview/materialize correctly. |
| Selected weekdays | ☐ | ☐ | ☐ | ☐ | Only selected weekdays appear. |
| Every-N-hours schedule | ☐ | ☐ | ☐ | ☐ | Uses explicit user interval; no strength/instruction interpretation. |
| Cycle/custom date range | ☐ | ☐ | ☐ | ☐ | Boundaries are honored. |
| As-needed medicine | ☐ | ☐ | ☐ | ☐ | Does not create automatic reminders unless explicitly requested. |
| Pause/resume/complete/archive | ☐ | ☐ | ☐ | ☐ | Future reminder state reflects status changes and stale platform requests are reconciled. |
| Future snooze after original due time | ☐ | ☐ | ☐ | ☐ | Snoozed reminder remains actionable/upcoming until explicit snooze due time. |
| Overdue snooze | ☐ | ☐ | ☐ | ☐ | Overdue handling uses the snoozed due time rather than stale original schedule time. |
| Taken/skipped/delayed/missed | ☐ | ☐ | ☐ | ☐ | Old platform request is cancelled before handled state commits; log/status transitions are accurate. |
| Snooze replacement | ☐ | ☐ | ☐ | ☐ | Old request cancels before state transition; replacement uses explicit future snooze time. |
| Reminder action recovery | ☐ | ☐ | ☐ | ☐ | If later persistence/replacement scheduling fails, previous state/request can be reconciled rather than left contradictory. |
| Schedule change stale-request cleanup | ☐ | ☐ | ☐ | ☐ | Obsolete OS request is cancelled before the stale occurrence is invalidated/replaced. |
| Medicine/profile delete reminder cleanup | ☐ | ☐ | ☐ | ☐ | Future OS requests are cancelled before cascade; failed persistence does not silently strand contradictory reminder state. |
| Quiet hours | ☐ | ☐ | ☐ | ☐ | User-defined quiet-hour behavior is respected and an already scheduled request is reconciled when newly suppressed. |
| Follow-up reminder | ☐ | ☐ | ☐ | ☐ | Follow-up is explicit and removable. |
| Appointment create/edit/delete | ☐ | ☐ | ☐ | ☐ | Appointment lifecycle works without medical inference and reminder state remains reconciled with persistence. |
| Calendar export | ☐ | ☐ | ☐ | ☐ | Export happens only after explicit user action. |
| Document import | ☐ | ☐ | ☐ | ☐ | File imports into encrypted CareNest storage. |
| Document export | ☐ | ☐ | ☐ | ☐ | Decrypted copy is created only for user-selected destination/share action. |
| Document delete | ☐ | ☐ | ☐ | ☐ | CareNest-owned encrypted payload is removed. |
| Shared report cache cleanup | ☐ | ☐ | ☐ | ☐ | CareNest removes its owned temporary report after share handoff where still under app control. |
| Stock adjustment | ☐ | ☐ | ☐ | ☐ | Uses only user-entered quantities; supply warning remains visible. |
| PDF report | ☐ | ☐ | ☐ | ☐ | Opens/exports and includes privacy/medical limitation language. |
| CSV reports | ☐ | ☐ | ☐ | ☐ | Machine-readable output is correct, selected-profile scoped, and formula-like user strings remain text. |
| Profile JSON export | ☐ | ☐ | ☐ | ☐ | Selected local profile data exports explicitly. |
| Encrypted backup | ☐ | ☐ | ☐ | ☐ | Backup is password-protected and user chooses destination. |
| Wrong backup password | ☐ | ☐ | ☐ | ☐ | Restore is rejected without partial data replacement. |
| Tampered backup | ☐ | ☐ | ☐ | ☐ | Authentication/topology failure blocks restore before destructive replacement. |
| Clean-install restore | ☐ | ☐ | ☐ | ☐ | Database and encrypted documents restore consistently. |
| Reset all data | ☐ | ☐ | ☐ | ☐ | Requires explicit confirmation and removes app-owned local data. |
| Theme system/light/dark | ☐ | ☐ | ☐ | ☐ | Text/control contrast remains usable. |
| Large text | ☐ | ☐ | ☐ | ☐ | Core actions remain visible/reachable; no critical clipping. |
| Keyboard navigation | N/A/☐ | ☐ | N/A/☐ | ☐ | Desktop-capable surfaces are operable without mouse where applicable. |
| Screen reader semantics | ☐ | ☐ | ☐ | ☐ | Important controls have meaningful accessible names/roles. |
| Reduced motion | ☐ | ☐ | ☐ | ☐ | Preference is honored by CareNest-controlled motion. |
| Buy Me a Coffee action | ☐ | ☐ | ☐ | ☐ | Opens `https://buymeacoffee.com/sanskarIN`; clearly voluntary; no feature entitlement. |
| Offline core use | ☐ | ☐ | ☐ | ☐ | Profiles, medicines, logs, documents and reports work without CareNest backend/account. |

## SQLite/native-provider packaged compatibility matrix

This section is mandatory for the first production candidate after the SQLite native/provider remediation. A clean NuGet audit and green unit/integration tests do not substitute for packaged upgrade evidence.

Use only fictional/synthetic pre-remediation data.

| Scenario | Android | Windows | iOS | Mac Catalyst | Expected result |
|---|---:|---:|---:|---:|---|
| Upgrade/open existing RC1 database | ☐ | ☐ | ☐ | ☐ | Existing database opens without corruption, unexpected schema rewrite, or record loss. |
| Existing profiles/medicines/schedules | ☐ | ☐ | ☐ | ☐ | Structured records remain readable with unchanged meaning. |
| Existing reminder occurrences/logs | ☐ | ☐ | ☐ | ☐ | Rows remain readable and reminder reconciliation can rebuild current platform state. |
| Existing appointments/stock/tags/settings | ☐ | ☐ | ☐ | ☐ | Structured data remains readable and editable. |
| Existing encrypted document payload | ☐ | ☐ | ☐ | ☐ | Existing `.cndoc` data decrypts through the unchanged document-key path. |
| New encrypted document after upgrade | ☐ | ☐ | ☐ | ☐ | Import/export/delete still works on the remediated dependency graph. |
| Pre-remediation synthetic backup restore | ☐ | ☐ | ☐ | ☐ | Restores when a canonical fixture is available and format compatibility permits. |
| New backup/clean restore after upgrade | ☐ | ☐ | ☐ | ☐ | Current encrypted backup round-trip succeeds without data inconsistency. |
| Reminder rebuild after dependency upgrade | ☐ | ☐ | ☐ | ☐ | Rebuild/reconciliation succeeds and does not duplicate or strand stale requests. |
| SQLite integrity check after upgrade | ☐ | ☐ | ☐ | ☐ | Database passes integrity validation on representative packaged data. |

Any corruption, migration failure, backup incompatibility, encrypted-document failure, or reminder-state regression blocks production promotion even when vulnerability audit is green.

## Android reminder reliability matrix

Test on at least one recent physical Android device in addition to an emulator when preparing a public release.

| Scenario | Result | Expected behavior |
|---|---:|---|
| Notification permission not yet requested | ☐ | Permission is requested only when user creates/saves a reminder-capable feature. |
| Permission denied | ☐ | CareNest reports limitation; app remains usable. |
| Permission granted | ☐ | Scheduled notification can be delivered subject to OS policy. |
| Exact alarm unavailable | ☐ | CareNest surfaces fallback/limitation rather than claiming guaranteed timing. |
| Battery optimization active | ☐ | Diagnostic warning/guidance is visible. |
| Reboot | ☐ | Future reminder state is rebuilt after boot signal when platform permits. |
| Manual system time change | ☐ | Reminder rebuild/reconciliation occurs without rewriting stored schedule intent. |
| Time-zone change | ☐ | Future occurrence times are recalculated from stored local schedule semantics. |
| Force-stop then reopen | ☐ | Limitations are acknowledged; startup rebuild occurs on reopen. |
| Future snooze crosses original due time | ☐ | Snooze remains scheduled for explicit snoozed due time, not dropped by original time. |
| Taken/skipped/delayed/missed | ☐ | Existing OS request is cancelled before the handled state is committed. |
| Snooze replacement | ☐ | Existing OS request is cancelled before the replacement request is scheduled. |
| Schedule change with old alarm | ☐ | Obsolete alarm is cancelled during reconciliation before stale state is invalidated. |
| Delete medicine/profile with future alarm | ☐ | Future alarms are cancelled before persistent cascade; failed cascade remains recoverable/reconcilable. |

## iOS / Mac Catalyst notification matrix

| Scenario | iOS | Mac Catalyst | Expected behavior |
|---|---:|---:|---|
| Permission not yet requested | ☐ | ☐ | No onboarding-time permission pressure. |
| Permission denied | ☐ | ☐ | Limitation is surfaced; app remains usable. |
| Permission granted | ☐ | ☐ | OS local notification is registered for eligible occurrences. |
| Foreground/background transition | ☐ | ☐ | App state remains consistent. |
| Time-zone change then reopen | ☐ | ☐ | Rebuild uses stored schedule intent; no silent schedule rewrite. |
| Future snooze after original due time | ☐ | ☐ | Explicit snooze due time remains the effective due time. |
| Taken/skipped/delayed/missed action | ☐ | ☐ | Old OS request is cancelled before handled-state persistence. |
| Snooze replacement | ☐ | ☐ | Old OS request is cancelled before replacement scheduling. |
| Schedule/state change reconciliation | ☐ | ☐ | Stale OS requests are removed before replacement/suppression/invalidation. |
| Notification tap/action | ☐ | ☐ | Opens expected CareNest context without exposing sensitive content unnecessarily. |

## Windows reminder limitation matrix

| Scenario | Result | Expected behavior |
|---|---:|---|
| CareNest running | ☐ | Due reminder behavior/diagnostic path works. |
| CareNest closed | ☐ | App does not claim guaranteed background delivery if current implementation cannot guarantee it. |
| Startup after missed time | ☐ | Overdue reconciliation records appropriate missed state. |
| Same-ID timer replacement/cancellation | ☐ | Old in-process timer cannot remove or outlive the newer replacement owner incorrectly. |
| Taken/skipped/delayed/missed action | ☐ | Old in-process request is cancelled before handled-state persistence. |
| Snooze replacement | ☐ | Old request cancels before replacement timer is owned/scheduled. |
| Notification/settings diagnostic | ☐ | Current limitation text is visible and accurate. |

## Security/privacy manual checks

- [ ] App logs contain no backup password, plaintext PIN, imported document bytes, or full sensitive note text.
- [ ] Reminder cancellation/scheduling/recovery logs contain no health-record identifiers, exception messages, or stack traces.
- [ ] Generic notification content does not expose more information than intended by current privacy settings.
- [ ] Export/share destinations are always user-selected.
- [ ] Application-owned temporary report/export files are removed according to the documented managed-cache lifecycle.
- [ ] Backup destination is always user-selected.
- [ ] Wrong/tampered backup fails safely.
- [ ] Encrypted document payload is not readable as original plaintext in app storage.
- [ ] Existing encrypted documents remain decryptable after the SQLite native/provider package update.
- [ ] App lock cold start is verified after process termination.
- [ ] No network requirement appears during normal local-first flows.
- [ ] Opening GitHub/BMC/legal links is an explicit user action and clearly leaves CareNest.
- [ ] BMC support does not unlock functionality or collect CareNest health data.

## Accessibility/manual UX checks

- [ ] 200% text scaling does not hide destructive-action confirmations or primary save/cancel actions.
- [ ] Screen reader order follows visual/task order.
- [ ] Buttons/inputs expose meaningful accessible names.
- [ ] Focus indicators are visible on keyboard-capable targets.
- [ ] Color is not the only indicator for reminder status or validation errors.
- [ ] Light/dark modes preserve readable contrast.
- [ ] Reduced-motion preference avoids unnecessary CareNest-controlled animation.
- [ ] Error messages are actionable and do not expose internal/sensitive values.

## Release sign-off

A manual matrix is complete only when the intended distribution platforms have recorded evidence for all applicable rows. `N/A` must include a short reason. Do not convert an untested row into `N/A` merely to unblock a release.

The dependency source remediation and automated NuGet audit are already separate evidence. This matrix must record the packaged compatibility evidence rather than re-opening the old audit suppression.
