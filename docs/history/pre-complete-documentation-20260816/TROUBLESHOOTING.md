# CareNest Troubleshooting Guide

This guide covers development and end-user troubleshooting for CareNest `1.0.0-rc.1` while preserving privacy and medical/reminder limitations.

> CareNest is an organizational tool. Troubleshooting reminder delivery must not be treated as medical advice or a guarantee that a reminder will fire under every operating-system/device condition.

## Notifications do not arrive

Check in this order:

1. Open CareNest Settings / notification diagnostics.
2. Confirm the relevant medicine/profile/schedule is active.
3. Confirm the schedule is enabled.
4. Confirm the schedule has the intended time zone.
5. Confirm the explicit user-entered times/interval/cycle/weekday values.
6. Confirm notification permission is granted.
7. Check quiet hours.
8. Check snooze/follow-up state if applicable.
9. On Android, check exact-alarm capability and battery optimization diagnostics.
10. Rebuild future reminders from the applicable developer/diagnostic option.
11. Use a test reminder if the app exposes the test action.

CareNest cannot guarantee notification delivery when a device is powered off, force-stopped, heavily battery restricted, denied permission, blocked by platform policy, or otherwise prevented by the OS.

## A reminder still appears after a schedule/state change

Persisted CareNest occurrence state and the operating-system scheduled request are separate surfaces.

Current reconciliation rules attempt to cancel an existing platform request before replacement, suppression, or invalidation.

Check:

- whether the old occurrence still records a platform notification identifier;
- whether platform cancellation failed;
- whether the schedule/medicine/profile state still makes the occurrence valid;
- quiet-hour changes;
- whether the app has completed a startup/rebuild recovery pass;
- target OS notification settings/capabilities.

A platform cancellation failure intentionally leaves the state retryable instead of falsely reporting successful cleanup.

## A Taken/Skipped/Delayed/Missed action failed

Handled reminder actions use cancellation-first ordering.

CareNest first attempts to cancel the existing platform request, then persists the handled state. If a later persistence step fails after cancellation, CareNest attempts non-cancelled previous-state restoration and reminder rebuild.

If both the original action and recovery fail, an aggregate error can be surfaced rather than falsely reporting a consistent result.

Troubleshoot using synthetic data and privacy-safe logs. Do not log medicine/profile names, private notes, reminder identifiers, exception messages, or stack traces merely to diagnose platform cancellation/recovery.

## Snoozed reminder disappears or fires at the original time

For a valid snoozed occurrence, `SnoozedUntilUtc` is the effective due time.

Check:

- the snooze value exists;
- the snooze value is UTC at the application boundary;
- the snooze value is in the future when the action is created;
- the schedule/medicine/profile remains eligible;
- platform cancellation of the old request succeeded;
- replacement scheduling succeeded and is not suppressed by quiet hours;
- startup/rebuild recovery ran after an interrupted action.

The old `ScheduledUtc` is historical schedule identity; it should not make a future snooze disappear simply because the original time has passed.

## A schedule exists but no automatic reminder is expected

Confirm whether the schedule is `AsNeeded`.

As-needed schedules intentionally create **no automatic occurrences**.

Also confirm the medicine/profile/schedule state:

- disabled schedule;
- paused medicine;
- completed medicine;
- archived medicine;
- archived profile.

These states suppress applicable automatic materialization.

## Reminder time looks wrong after time-zone/DST change

CareNest stores explicit local schedule intent with a time-zone ID.

Check:

- stored schedule time-zone ID;
- device current time zone;
- whether the local time is in a daylight-saving gap/overlap;
- developer diagnostics/time-zone simulation if available.

Important behavior:

- invalid spring-forward local time is not silently moved to another guessed time;
- ambiguous fall-back time resolves deterministically;
- stored schedule intent should not be silently rewritten merely because device time zone changed.

See `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`.

## Snooze is rejected

Snooze must resolve internally to an explicit future UTC time.

It is rejected if:

- no snooze time was supplied;
- the value is not UTC at the coordinator boundary;
- the value is not later than current UTC time.

## Android reminders are inconsistent

Review:

- notification permission;
- exact-alarm capability;
- battery optimization;
- manufacturer-specific background restrictions;
- force-stop state;
- reboot behavior;
- time/time-zone changes;
- platform cancellation/replacement behavior after schedule edits or handled actions.

A successful CI build cannot prove real delivery on every Android device/vendor policy.

Use `docs/releases/MANUAL_TEST_MATRIX.md` for release evidence.

## Windows reminder limitation

The current Windows fallback does not claim reliable notification delivery while CareNest is not running.

If reminders work while the app is open but not when closed, review the documented Windows limitation rather than presenting it as a guaranteed background service.

For same-ID replacement issues, verify that an older timer does not remove a newer replacement owner and that cancellation/disposal lifetime follows the current in-process notification implementation.

## Build fails before MAUI compilation

Run:

```bash
dotnet --info
dotnet workload list
```

Then build platform-neutral projects:

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
```

If these fail, address shared source/restore/analyzer problems before diagnosing platform workloads.

## MAUI workload errors

Inspect installed workloads:

```bash
dotnet workload list
```

Repair when appropriate:

```bash
dotnet workload repair
```

Install the specific target workload required by the host.

Examples:

```bash
dotnet workload install maui-android
dotnet workload install maui-ios
dotnet workload install maui-maccatalyst
```

For current Windows CI, the repository installs:

```powershell
dotnet workload install maui
```

Do not require unrelated workloads on a target-limited host.

## Target-framework propagation errors

Use `CareNestTargetFramework` rather than globally overriding target frameworks.

Example Android build:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

This avoids leaking the app target framework into referenced `net10.0` libraries.

## Apple build/Xcode mismatch

If .NET Apple workload reports unsupported Xcode:

1. run `dotnet --info`;
2. run `dotnet workload list`;
3. run `xcodebuild -version`;
4. verify the selected Xcode path/version is supported by installed workload;
5. install/select compatible Xcode.

Do not bypass the compatibility check as a release solution.

GitHub Apple CI currently uses a macOS 26 runner compatible with the installed .NET 10 Apple workload.

## Android API/platform analyzer errors

Do not solve Android availability/nullability analyzer failures with a blanket CA1416 suppression.

Use explicit API-level guards/null checks/platform-correct code.

## Analyzer fails build

Treat analyzer output as a real finding until understood.

Historical CI has exposed legitimate issues such as non-generic enum validation, eager logging argument evaluation, transaction-helper cancellation-token ordering, semaphore ownership, and constant-array allocation in test contracts.

Preferred response:

- fix source;
- add regression test if useful;
- narrow only truly advisory rule configuration;
- re-run exact-head verification after runtime/test/workflow/release-script changes.

## `dotnet format` fails

Run formatting on the exact failing project:

```bash
dotnet format <project.csproj>
```

Then verify:

```bash
dotnet format <project.csproj> --verify-no-changes
```

CI checks platform-neutral projects individually.

## Unit tests fail around reminder times

Check:

- `DateTime.Kind` is UTC for planner window values;
- `toUtc > fromUtc`;
- explicit schedule time zone exists on the runner;
- ownership IDs match;
- expected date is within schedule + medicine boundaries;
- profile/medicine/schedule state is eligible;
- DST date corresponds to the intended zone transition.

Reminder tests should be deterministic and not depend on current wall clock.

## Integration tests fail on SQLite WAL/backup

Important implementation facts:

- journal-mode/busy-timeout/full-checkpoint PRAGMAs return results;
- they must be consumed as result-producing operations;
- snapshot tests verify committed data and `PRAGMA integrity_check`.

If a package/provider update changes behavior, follow `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

## Dependency Audit reports the old SQLite advisory again

The intended current RC1 graph no longer relies on the former `GHSA-2m69-gcr7-jv3q` audit suppression.

Expected source policy includes:

- `SQLitePCLRaw.lib.e_sqlite3` at `3.53.3` or later compatible reviewed floor;
- Android native/provider leaves and selected providers at `2.1.12` or later compatible reviewed floor;
- central transitive pinning enabled;
- no old `NuGetAuditSuppress` entry.

If the advisory reappears:

1. inspect `Directory.Packages.props` and the resolved transitive graph;
2. run `SqliteDependencySecurityContractTests`;
3. run unsuppressed `dotnet restore -p:NuGetAudit=true -p:NuGetAuditMode=all`;
4. audit the Android MAUI graph as well as platform-neutral/test graphs;
5. compare with `docs/security/DEPENDENCY_RISK_REGISTER.md`;
6. do **not** restore the old suppression just to make CI green;
7. if a new compatible remediation changes native/provider behavior, repeat packaged existing-database/encrypted-document/backup/reminder compatibility testing.

Read:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

## Local quality/preflight audit fails

Both `quality-gate` and `release-preflight` scripts treat dependency audit as blocking.

Do not replace the failure with `|| true`, warning-only output, or a wildcard audit suppression.

For a selected MAUI target, `CARENEST_TARGET` causes release preflight to audit that app graph before the optional Release build.

## Release Evidence workflow fails

The Release Evidence workflow intentionally attempts all core evidence components, uploads available evidence with `if: always()`, and then applies an aggregate failure gate.

A failed run can therefore still have a useful evidence artifact.

Check:

- unit/integration/UI TRX files;
- dependency inventories;
- tracked workspace integrity result;
- source/ref/run metadata;
- SHA-256 manifests;
- the final aggregate failure message.

Artifact existence does not mean release approval. The run itself must be successful for release evidence to be accepted.

Artifact names include commit SHA, GitHub Actions run ID, and run attempt so reruns can be distinguished.

## Release tag fails a required workflow

Tags matching `v*` run the exact tagged source through CareNest CI, CodeQL, Dependency Audit, Release Gate, and Release Evidence.

If one fails:

- do not publish/promote the failing tag as a successful production release;
- preserve the failed evidence;
- fix source/configuration on a new commit;
- re-run exact-source/manual checks as applicable;
- use the corrected approved commit/tag.

Do not weaken the tag gate to make an already failing release appear successful.

## Restore is rejected

Restore can be rejected when:

- format/magic is unsupported;
- version is unsupported;
- password is wrong;
- authentication/tamper validation fails;
- package/schema/topology validation fails.

CareNest should validate before overwriting current local data.

If a real backup fails:

- do not post the backup/password publicly;
- reproduce using synthetic data if possible;
- record CareNest version/platform/OS and safe error category.

See `docs/architecture/BACKUP_AND_RESTORE.md`.

## Backup file exists but restore data is incomplete

For release testing verify:

- structured records restored;
- encrypted documents restored/open correctly;
- document key portability worked;
- reminder rebuild occurred as expected;
- snapshot contained committed WAL data;
- target storage permissions were available.

Use a clean installation for meaningful restore qualification.

## Existing data fails after SQLite native/provider update

Treat this as a production-blocking compatibility defect even if NuGet audit is green.

Using synthetic data:

- verify SQLite integrity;
- verify profiles/medicines/schedules/reminders/logs/appointments/documents/stock/tags/settings;
- verify reminder rebuild/reconciliation;
- verify existing encrypted document access through the unchanged key path;
- verify current and canonical pre-remediation backup restore where available;
- record exact package/build/source/device evidence.

Do not solve a data-compatibility regression by silently downgrading/restoring a vulnerable dependency path without security review.

## Document cannot open

Check:

- metadata record still exists;
- encrypted payload still exists in app-owned storage;
- secure document key material remains available;
- device has temporary storage for explicit open/export;
- target file/share operation has permission.

CareNest treats imported files as opaque content and does not medically interpret them.

## Document export works but file is now plaintext outside CareNest

This is an expected privacy-boundary transition.

The exported copy is no longer protected by the CareNest encrypted document vault unless the destination applies its own protection.

See `docs/architecture/DATA_STORAGE_AND_EXPORT.md`.

## App lock rejects a correct-looking PIN

Check:

- same installation/profile context;
- PIN format policy;
- secure-storage availability;
- whether lock was disabled/reset previously;
- whether device migration/secure-storage behavior changed.

Do not inspect/log/store the plaintext PIN for debugging.

Use synthetic PINs for reproduction.

## App lock does not encrypt SQLite

Correct: current app lock is a local UI privacy barrier, not whole-database encryption.

See `docs/security/SECURITY_MODEL.md`.

## Support link does not open

Canonical URL:

`https://buymeacoffee.com/sanskarIN`

Check:

- device/browser can open HTTPS links;
- app has a usable launcher/browser handler;
- channel build has not intentionally removed the link due to store-policy requirements.

CareNest does not append health data to the link.

## Export/share/calendar action fails

Check:

- destination handler exists;
- user selected a valid destination;
- target platform file/share/calendar permission/capability;
- sufficient storage;
- no invalid/removed source file.

Once export succeeds, destination privacy rules apply.

## UI clips under large text

This is an accessibility defect, not a user configuration problem.

Record:

- platform/device;
- text scaling setting;
- screen/control;
- screenshot using synthetic data;
- whether primary action became unreachable.

Fix layout rather than telling users to reduce accessibility settings.

See `docs/design/ACCESSIBILITY.md`.

## Theme/readability issue

Test system/light/dark modes.

Check:

- text/background contrast;
- validation text;
- focus indicators;
- state labels;
- support/legal links;
- safety warnings.

Do not rely on color alone for status.

## Public bug reports

Safe information to include:

- CareNest version;
- platform/OS;
- device/emulator model when relevant;
- time zone when reminder issue depends on it;
- notification permission/capability state;
- exact reproduction steps using synthetic data;
- privacy-safe logs/diagnostics.

Do **not** include:

- health documents;
- real backups;
- backup passwords;
- app-lock PINs;
- private health notes;
- encryption/signing keys;
- screenshots containing unredacted private health data.

## Further references

- `docs/setup/DEVELOPMENT.md`
- `docs/setup/PLATFORM_SETUP.md`
- `docs/setup/MAINTAINER_OPERATIONS.md`
- `docs/testing/TESTING_GUIDE.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/releases/RELEASE_PROCESS.md`
