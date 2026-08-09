# SQLite Dependency Migration Plan

## Purpose

CareNest `1.0.0-rc.1` currently tracks `GHSA-2m69-gcr7-jv3q` against SQLitePCLRaw native `2.1.11` resolved through the current `sqlite-net-pcl` dependency chain. The exact advisory is temporarily suppressed in NuGet audit only so unrelated CI failures remain observable. The advisory is still open.

This document defines the required migration/verification process when a compatible patched dependency path becomes available or when the team chooses a different SQLite provider.

## Non-negotiable rules

1. Do not claim the advisory is fixed until the actual resolved dependency graph no longer contains the affected package/version for every shipping target.
2. Do not widen `NuGetAuditSuppress` or use severity-wide suppression.
3. Do not change database libraries without testing migrations, existing databases, backup/restore, encrypted-document references, cascade behavior and concurrency/WAL behavior.
4. Do not silently alter stored health records to accommodate a provider migration.
5. Keep the database local-first; a dependency migration must not introduce a backend/account/network requirement.

## Preferred path A — compatible package upgrade

When a compatible version becomes available:

- [ ] Update the top-level SQLite package/version in `Directory.Packages.props`.
- [ ] Remove any explicit obsolete native pin that is no longer needed.
- [ ] Restore each platform independently using `CareNestTargetFramework` for MAUI targets.
- [ ] Inspect resolved direct/transitive package versions.
- [ ] Run NuGet vulnerability reporting and confirm `GHSA-2m69-gcr7-jv3q` no longer applies to the resolved graph.
- [ ] Remove the matching `NuGetAuditSuppress` entry only after the resolved graph is verified.
- [ ] Update `docs/security/DEPENDENCY_RISK_REGISTER.md` from Open to Resolved with exact package versions and evidence.

## Fallback path B — replace sqlite-net-pcl/native dependency path

If no compatible patched path exists and the advisory blocks production release, evaluate a provider migration without changing the domain/application contracts unnecessarily.

Candidate design constraints:

- preserve `CareNest.Infrastructure` as the persistence boundary;
- keep Domain/Application unaware of provider-specific APIs;
- support Android, Windows, iOS simulator/device and Mac Catalyst targets used by CareNest;
- support parameterized queries and transactions;
- support schema migrations and foreign-key enforcement;
- support deterministic backup snapshots or an equivalent safe backup export strategy;
- provide predictable concurrency/locking semantics;
- avoid introducing server/network requirements;
- use maintained packages with a reviewable security/update story.

Do not select a replacement solely because it makes one CI advisory disappear. Compare maintenance status, platform support, native-binary provenance, migration risk, performance, API stability and licensing.

## Required regression suite after any SQLite dependency change

### Automated core tests

- [ ] Unit tests all pass.
- [ ] Integration tests all pass.
- [ ] UI-contract tests all pass.
- [ ] Database initialization succeeds on a clean database.
- [ ] Migration version advances correctly from every supported schema version.
- [ ] Foreign keys are enforced.
- [ ] Repository CRUD round-trips preserve values.
- [ ] Cascade-delete behavior is unchanged.
- [ ] WAL/selected journal behavior is explicitly verified or intentionally redesigned/documented.
- [ ] Busy-timeout/concurrency behavior is explicitly verified or intentionally redesigned/documented.
- [ ] Backup snapshot creation succeeds with active database state.
- [ ] Encrypted backup restore succeeds on a clean database.
- [ ] Wrong-password and tampered-backup restore fail safely.
- [ ] Imported encrypted-document metadata/references survive backup/restore.
- [ ] Report/export tests still pass.

### Platform builds

- [ ] Android Release.
- [ ] Windows Release.
- [ ] iOS simulator Release.
- [ ] Mac Catalyst Release.
- [ ] CodeQL.

### Manual migration/device tests

- [ ] Upgrade a device/emulator installation containing fictional `1.0.0-rc.1` data.
- [ ] Verify profiles, medicines, schedules, logs, appointments, documents, stock and tags after upgrade.
- [ ] Verify old encrypted document payloads still decrypt through the existing key path.
- [ ] Verify a pre-migration backup restores correctly after the dependency update when the backup schema is unchanged.
- [ ] Verify new backup restores on a clean install.
- [ ] Verify reminder rebuild after migration.
- [ ] Verify reset/delete operations remove expected application-owned data.

## Evidence to record

When the issue is resolved, record in both `docs/security/DEPENDENCY_RISK_REGISTER.md` and `what_changed.md`:

- old direct/transitive package graph;
- new direct/transitive package graph;
- exact commit SHA;
- exact CI/CodeQL run IDs;
- test totals;
- target build results;
- migration/manual test evidence;
- whether `NuGetAuditSuppress` was removed;
- any database behavior intentionally changed.

## Rollback rule

If a dependency upgrade/provider migration causes data corruption, migration failure, backup incompatibility, platform build regressions, or reminder-state inconsistencies, do not ship it merely to clear the advisory. Revert to the last known source state, keep the advisory explicitly open, and choose either a safer patched package path or a better-tested provider migration.
