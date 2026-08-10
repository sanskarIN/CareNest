# CareNest Production Quality Gate

CareNest must not be described as bug-free. A production release is acceptable only when the preventive controls and evidence below are complete for the exact release commit.

## Source quality

- Runtime source contains no TODO/FIXME/NotImplemented implementation placeholders.
- Nullable reference types and analyzers remain enabled.
- CI warnings-as-errors policy remains enabled for CI builds except explicitly documented advisory analyzer exceptions.
- Platform-neutral projects pass `dotnet format --verify-no-changes`.
- Shared/Domain/Application/Infrastructure project dependency direction passes architecture contract tests.
- Concrete ViewModels do not directly access SQLite infrastructure or create network clients.
- Runtime source does not synchronously block on tasks through `.Wait()`, `.Result`, `GetAwaiter().GetResult()`, `Thread.Sleep`, `Task.WaitAll` or `Task.WaitAny` patterns.

## Product safety

- No diagnosis, treatment recommendation, dosage calculation/inference, medication-interaction checking or clinical risk scoring is introduced.
- Medicine strength and instruction values remain opaque user-entered text.
- As-needed schedules do not automatically create reminders.
- Archived profiles, paused/completed/archived medicines and disabled schedules do not automatically materialize reminders.
- Daily, selected-weekday, cycle, custom-range and every-N-hours behavior is derived only from explicit user-entered schedule values.
- Planning windows remain half-open (`fromUtc` inclusive, `toUtc` exclusive) so adjacent rebuild windows do not duplicate boundary occurrences.
- Planner window start/end and reminder rebuild overrides require actual UTC `DateTime` values.
- Duplicate explicit clock times do not create duplicate occurrence identities.
- Reminder ownership is verified across profile → medicine → schedule → persisted schedule-time relationships before materialization.
- Unknown schedule kinds, unsupported weekday-mask bits, invalid explicit intervals and invalid time-zone identifiers are rejected rather than silently reinterpreted.
- Invalid daylight-saving spring-forward local times do not cause CareNest to invent an alternate reminder time.
- Ambiguous daylight-saving fall-back local times remain deterministic across rebuilds.
- Representative DST gap/overlap coverage spans North America, Europe and Australia when those identifiers exist on the test host.
- Deterministic property-style recurrence tests use fixed seeds/explicit synthetic schedules and remain reproducible.
- Snooze actions require an explicit future UTC timestamp before persistence or platform scheduling.
- Notification permission is not requested during onboarding; it is requested at the first explicit reminder-capable action.
- Stock changes use only user-configured values.
- Medical/reminder limitations remain visible in onboarding and About.

## Privacy/security

- No required CareNest account/server/network client is introduced in v1.
- No analytics/telemetry client is introduced.
- No common signing/credential files are committed.
- Error/reminder logging does not pass full exception objects or health-record identifiers to the structured logger.
- Planner ownership mismatches fail closed instead of silently creating occurrences under another local entity.
- Document and backup encryption tests pass.
- WAL snapshot tests verify copied committed data and SQLite integrity rather than only file existence.
- A pre-cancelled snapshot operation leaves no output file.
- App-lock source contracts verify salted PBKDF2-HMAC-SHA256, fixed-time comparison, no plaintext PIN persistence, verifier-buffer clearing and stored lock-material removal.
- App lock remains described as a local privacy barrier, not whole-database/device encryption.
- SQLite migration/integrity tests pass.
- CodeQL passes.
- Dependency audit passes apart from the explicitly tracked/narrowly suppressed SQLite advisory.
- The SQLite advisory has an explicit release decision and is never represented as fixed unless a patched path is actually verified.

## Cross-platform automated evidence

- Unit tests pass.
- Integration tests pass.
- UI/repository contract tests pass.
- Android Release build passes.
- Windows Release build passes.
- iOS simulator Release build passes.
- Mac Catalyst Release build passes.
- Release evidence artifacts are generated for the exact final release commit.

The latest RC1 hardening source baseline is `c61f3c31c4ba33419c7b348fc8ee63a58eaa637b`, verified by marker-only PR #30 with:

- CareNest CI #248 / `31382194805`: success;
- platform-neutral formatting: success;
- 74 unit tests: passed;
- 13 integration tests: passed;
- 54 UI-contract/policy tests: passed;
- 141 total core tests: passed;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #248 / `31382194687`: success;
- Dependency Audit #10 / `31382194683`: success.

PR #29 / CI #246 intentionally remains recorded as a superseded failure: it exposed CA2263 in the non-generic `Enum.IsDefined` overload added during schedule validation hardening. The source was fixed on `main` instead of suppressing the analyzer, and PR #30 verified the corrected exact head.

This baseline is automated evidence only. The final public release still needs a fresh exact promoted-commit Release Evidence run after all release blockers are cleared.

## Manual evidence

- Android device/emulator matrix complete.
- Windows manual matrix complete.
- iOS/iPadOS manual matrix complete.
- Mac Catalyst manual matrix complete.
- Notification permission and delivery limitations tested.
- Android exact-alarm/battery/reboot behavior tested on representative devices.
- Time-zone change behavior tested.
- Snooze behavior tested against real platform notification scheduling.
- Document import/export/delete tested.
- Calendar export tested.
- Encrypted backup/restore tested on clean installation/release build.
- App lock cold-start flow tested.
- Screen-reader, keyboard, large-text, reduced-motion and contrast checks complete.

## Distribution evidence

- Current Apple/Google policy review for the voluntary project-support link is complete.
- Channel-specific support-link visibility follows current store rules.
- Signing identities are supplied outside Git.
- Signed packages are built from the exact verified commit.
- Store listing/privacy/data-safety claims match actual implementation.
- Release notes include known limitations and do not promise guaranteed reminder delivery.

Any failed, unknown or stale required gate blocks final production promotion until it is resolved or explicitly documented as not applicable by the release owner.
