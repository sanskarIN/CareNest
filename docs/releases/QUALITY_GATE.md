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
- Notification permission is not requested during onboarding; it is requested at the first explicit reminder-capable action.
- Stock changes use only user-configured values.
- Medical/reminder limitations remain visible in onboarding and About.

## Privacy/security

- No required CareNest account/server/network client is introduced in v1.
- No analytics/telemetry client is introduced.
- No common signing/credential files are committed.
- Error/reminder logging does not pass full exception objects or health-record identifiers to the structured logger.
- Document and backup encryption tests pass.
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
- Release evidence artifacts are generated for the exact release commit.

## Manual evidence

- Android device/emulator matrix complete.
- Windows manual matrix complete.
- iOS/iPadOS manual matrix complete.
- Mac Catalyst manual matrix complete.
- Notification permission and delivery limitations tested.
- Android exact-alarm/battery/reboot behavior tested on representative devices.
- Time-zone change behavior tested.
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
