# CareNest Troubleshooting Guide

This guide covers development and user troubleshooting for CareNest `1.0.0-rc.1` while preserving privacy, security and medical/reminder limitations.

> CareNest is an organizational tool. Troubleshooting reminder delivery is not medical advice and cannot create a guarantee that an operating system will deliver a notification under every device state.

## 1. Notifications do not arrive

Check in this order:

1. Open CareNest Settings/notification diagnostics.
2. Confirm the relevant profile/medicine/schedule is active.
3. Confirm the schedule is enabled.
4. Confirm the stored time zone is intended.
5. Confirm explicit user-entered times/interval/cycle/weekday values.
6. Confirm notification permission.
7. Check quiet hours.
8. Check snooze/follow-up state.
9. On Android, check alarm capability/battery optimization/vendor restrictions.
10. Rebuild future reminders through the applicable diagnostic/recovery action.
11. Use a test notification if the current UI exposes one.

CareNest cannot guarantee delivery when the OS blocks scheduling/delivery, the device is off/force-stopped, permission is denied or background execution is restricted.

## 2. Reminder still appears after schedule/state change

Persisted CareNest occurrence state and the OS request are separate surfaces.

Current reconciliation cancels stale platform requests before replacement/suppression/invalidation where required.

Check:

- occurrence/platform request identifier state;
- whether platform cancellation failed;
- current schedule/medicine/profile eligibility;
- quiet-hour changes;
- startup/rebuild reconciliation;
- target OS settings/capabilities.

Cancellation failure intentionally remains retryable rather than being reported as successful cleanup.

## 3. Taken/Skipped/Delayed/Missed action failed

Handled actions use cancellation-first ordering where required.

CareNest attempts to cancel the existing platform request before persisting handled state. If later persistence fails, recovery/rebuild can be attempted.

Troubleshoot with synthetic data and privacy-safe logs. Do not log raw health text, document contents, passwords/PINs/keys or unnecessary sensitive exception content.

## 4. Snoozed reminder disappears/fires at original time

For a valid snooze, `SnoozedUntilUtc` is effective due time.

Check:

- snooze value exists;
- value is true UTC;
- it was future-dated when created;
- profile/medicine/schedule remains eligible;
- old platform request cancellation succeeded;
- replacement scheduling succeeded;
- quiet hours/platform policy did not suppress delivery;
- startup/rebuild recovery completed after interruption.

The original `ScheduledUtc` remains historical schedule identity and should not make a still-future snooze overdue.

## 5. Schedule exists but no automatic reminder is expected

Check whether it is `AsNeeded`.

As-needed schedules intentionally create no automatic occurrences.

Also check:

- disabled schedule;
- paused medicine;
- completed medicine;
- archived medicine;
- archived profile.

These states suppress automatic materialization as documented.

## 6. Reminder time looks wrong after time-zone/DST change

Check:

- stored schedule time-zone ID;
- device current time zone;
- DST gap/overlap for that local time;
- diagnostic/time-zone simulation if available.

Important rules:

- invalid spring-forward local time is not silently moved to a guessed replacement;
- ambiguous fall-back time resolves deterministically;
- stored schedule intent is not silently rewritten merely because device time zone changes.

See `docs/testing/REMINDER_SCHEDULING_CONTRACT.md`.

## 7. Snooze is rejected

Snooze must resolve to an explicit future UTC time.

It is rejected when:

- no time was supplied;
- the value is not UTC at the application boundary;
- it is not later than current UTC when created.

## 8. Android reminders are inconsistent

Review:

- notification permission;
- exact/inexact alarm capability;
- battery optimization;
- manufacturer background restrictions;
- force-stop state;
- reboot/restart;
- clock/time-zone changes;
- cancellation/replacement after edits/actions.

A green CI build cannot prove delivery on every Android device/vendor policy.

Use `docs/releases/MANUAL_TEST_MATRIX.md`.

## 9. Windows reminder limitation

The current Windows fallback does not claim reliable delivery while CareNest is not running.

If reminders work only while the app is open, review the documented limitation instead of treating the app as a guaranteed background service.

For same-ID issues, verify older timer cleanup cannot remove ownership of a newer replacement timer.

## 10. iOS/Mac reminder issues

Check permission, notification settings, app lifecycle, time zone, target OS version and signing/provisioning for real-device builds.

Simulator compilation is not proof of real-device delivery.

## 11. Build fails before MAUI compilation

Run:

```bash
dotnet --info
dotnet workload list
```

Then isolate platform-neutral builds:

```bash
dotnet build src/CareNest.Shared/CareNest.Shared.csproj -c Release
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
```

Fix restore/compiler/analyzer problems there before diagnosing platform workloads.

## 12. MAUI workload errors

Inspect:

```bash
dotnet workload list
```

Repair when appropriate:

```bash
dotnet workload repair
```

Install only needed supported workloads, for example:

```bash
dotnet workload install maui-android
dotnet workload install maui-ios
dotnet workload install maui-maccatalyst
```

Windows CI currently uses the MAUI workload supported by its runner/toolchain.

## 13. Target-framework propagation errors

Use `CareNestTargetFramework` rather than globally overriding TFMs.

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

## 14. Apple build/Xcode mismatch

1. capture `dotnet --info`;
2. capture `dotnet workload list`;
3. capture `xcodebuild -version`;
4. verify supported Xcode/workload combination;
5. select/install compatible Xcode.

Do not bypass platform compatibility checks as a release solution.

## 15. Android platform analyzer errors

Do not solve legitimate availability/nullability findings with blanket suppression. Use correct API guards/null checks/platform-safe code.

## 16. XAML build fails with XC0022–XC0025

These warnings are intentionally promoted to errors.

Check:

- root page `x:DataType`;
- DataTemplate item `x:DataType`;
- picker item display binding type;
- explicit Source/RelativeSource binding type;
- ancestor binding-context type;
- actual ViewModel/item property name.

Do not use matching `NoWarn`, `x:Object` or `x:Null` to bypass type safety.

See `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

## 17. Analyzer fails build

Treat analyzer output as a real finding until understood.

Preferred response:

- fix source/test;
- add regression coverage where useful;
- narrow only genuinely advisory configuration;
- repeat exact-source verification after verification-relevant changes.

## 18. `dotnet format` fails

```bash
dotnet format <project.csproj>
```

Then:

```bash
dotnet format <project.csproj> --verify-no-changes
```

CI verifies platform-neutral/test projects independently.

## 19. Unit reminder tests fail around time

Check:

- true UTC `DateTime.Kind` at planner boundaries;
- `toUtc > fromUtc`;
- time-zone ID exists on host;
- ownership IDs match;
- date is inside schedule/medicine boundaries;
- profile/medicine/schedule state is eligible;
- intended DST transition date/zone.

Reminder tests should be deterministic and not depend on wall-clock timing.

## 20. SQLite WAL/backup integration tests fail

Review current SQLite provider/native graph and repository PRAGMA/snapshot behavior.

If a dependency update changed behavior, follow `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md` and perform packaged compatibility rather than treating a green restore as sufficient.

## 21. Dependency Audit reports the former SQLite advisory

Current source should not rely on the old `GHSA-2m69-gcr7-jv3q` suppression.

Check:

1. `Directory.Packages.props`;
2. resolved transitive graph;
3. `SqliteDependencySecurityContractTests`;
4. unsuppressed `dotnet restore -p:NuGetAudit=true -p:NuGetAuditMode=all`;
5. MAUI app graph as well as platform-neutral/test graph;
6. `docs/security/DEPENDENCY_RISK_REGISTER.md`.

Do not restore the old suppression just to make CI green.

## 22. Quality/release preflight audit fails

Audit is intentionally blocking.

Do not replace failure with `|| true`, warning-only handling or wildcard suppression.

When `CARENEST_TARGET` is supplied, release preflight audits/builds that target as defined by current scripts.

## 23. Store-package preflight confusion

Current store-package wrappers require an explicit supported target and delegate to standard release preflight.

They do **not** use a funding-link toggle. The external BMC destination is absent from application runtime/package source by policy.

They do not configure production signing or publish to a store.

## 24. Store payload scanner fails

The scanner is fail-closed and should not be weakened to make a package pass.

If it finds the forbidden external funding marker:

- inspect actual package/publish payload;
- identify the source/resource producing it;
- remove/correct the package source;
- add regression coverage;
- rerun inspection.

The 2026-08-15 historical Windows package defect demonstrated why source-only checks are insufficient.

## 25. Store Inspection artifact is unsigned/not installable as production

Expected: internal inspection artifacts are engineering evidence and can be unsigned/unpackaged/simulator-targeted by design.

Do not distribute them as production/store-ready packages.

## 26. Release Evidence workflow fails

The workflow can upload available evidence before final aggregate failure.

Inspect:

- test results;
- dependency inventories;
- workspace integrity;
- source/ref/run identity;
- SHA-256 manifests;
- aggregate outcome.

Artifact existence alone is not approval.

## 27. Production tag fails a required workflow

Production-style `v*` tags are expected to participate in:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

If a required run fails:

- do not publish/promote the failing tag;
- preserve evidence;
- fix on a new commit;
- repeat required automated/manual checks;
- use a corrected approved tag/version according to release policy.

Do not move a failed/rejected tag to another commit.

## 28. Restore is rejected

Possible causes:

- unsupported magic/version;
- wrong password;
- authentication/tamper failure;
- truncation/trailing data;
- invalid package/schema/topology.

Do not post real backups/passwords publicly. Reproduce using synthetic data and record safe version/platform/error category.

## 29. Backup restores incompletely

For release qualification verify:

- structured records;
- encrypted documents/open behavior;
- document-key portability;
- reminder rebuild;
- SQLite committed WAL state;
- target storage permissions.

Use clean-install restore testing with fictional data.

## 30. Existing data fails after SQLite provider/native update

Treat as production-blocking compatibility defect even if dependency audit is green.

With synthetic data verify database integrity, representative records, reminder reconciliation, encrypted document access and current/genuine historical backup compatibility where real prior fixtures exist.

Do not silently downgrade/reintroduce a vulnerable dependency path without security review.

## 31. Document cannot open

Check:

- metadata record;
- encrypted payload existence;
- secure document-key material;
- temporary storage availability;
- target file/share capability.

Missing/corrupt required key state should fail closed rather than silently replacing the key when existing ciphertext depends on it.

## 32. Exported document is plaintext

Expected privacy-boundary transition: explicit export/decrypt creates a copy outside the encrypted CareNest vault.

The destination is responsible for its own protection/retention.

## 33. App lock rejects a correct-looking PIN

Check secure-storage availability/material state and whether lock was reset/disabled/migrated.

Never inspect/log/store the real plaintext PIN for troubleshooting. Use synthetic PINs.

## 34. App lock does not encrypt SQLite

Correct. Current app lock is a local UI privacy barrier, not whole-database encryption.

See `docs/security/SECURITY_MODEL.md`.

## 35. Buy Me a Coffee link is not present in CareNest app

This is **expected current behavior**.

The distributed application source/package intentionally does not include or expose the external Buy Me a Coffee destination/card/command/artwork.

Voluntary project support exists in repository documentation only:

- `BUY_ME_A_COFFEE.md`;
- `docs/SUPPORT_CARENEST.md`.

Do not treat absence of the in-app link as a defect.

## 36. Export/share/calendar action fails

Check:

- target handler exists;
- valid user-selected destination;
- platform permissions/capability;
- storage availability;
- source file still exists.

After successful external handoff, destination privacy rules apply.

## 37. UI clips under large text

Treat this as an accessibility defect.

Record platform/device, scaling, screen/control and a synthetic-data screenshot. Fix layout rather than telling users to disable accessibility settings.

## 38. Theme/readability problem

Test system/light/dark themes, contrast, focus, disabled states and error/status communication. Do not rely on color alone.

## 39. Support/security report contains sensitive data

Remove/redact sensitive data before posting publicly.

Do not include real health records, documents, encrypted backups/passwords, PINs, keys, tokens or production signing material.

Use `SECURITY.md` for security-sensitive reporting.

## 40. Current source verification

PR #74 source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Verified 331/331 core tests plus all configured normal target, store-candidate, inspection, CodeQL and unsuppressed dependency gates.

## 41. Still stuck?

Collect only privacy-safe diagnostics:

- CareNest version/source if known;
- platform/OS;
- .NET/toolchain for development failures;
- target TFM;
- safe error category/type;
- reproducible synthetic steps.

Start with `docs/DOCUMENTATION_CATALOG.md`, `docs/GETTING_STARTED.md`, `docs/DEVELOPER_REFERENCE.md` and `SUPPORT.md`.