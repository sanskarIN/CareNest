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
- time/time-zone changes.

A successful CI build cannot prove real delivery on every Android device/vendor policy.

Use `docs/releases/MANUAL_TEST_MATRIX.md` for release evidence.

## Windows reminder limitation

The current Windows fallback does not claim reliable notification delivery while CareNest is not running.

If reminders work while the app is open but not when closed, review the documented Windows limitation rather than presenting it as a guaranteed background service.

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

Historical CI has exposed legitimate issues such as non-generic enum validation and eager logging argument evaluation.

Preferred response:

- fix source;
- add regression test if useful;
- narrow only truly advisory rule configuration;
- re-run exact-head verification after runtime/test changes.

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

## Dependency Audit shows SQLitePCLRaw advisory

Known tracked advisory:

`GHSA-2m69-gcr7-jv3q`

Current dependency path resolves native `2.1.11`.

The exact `NuGetAuditSuppress` entry is not a fix.

Read:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`.

Do not randomly pin a package version that is unavailable or incompatible just to silence audit output.

## Restore is rejected

Restore can be rejected when:

- format/magic is unsupported;
- version is unsupported;
- password is wrong;
- authentication/tamper validation fails;
- package/schema validation fails.

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