# SQLite Dependency Migration Plan

## Purpose

This document began as the remediation plan for `GHSA-2m69-gcr7-jv3q` while CareNest `1.0.0-rc.1` still resolved the affected SQLitePCLRaw native `2.1.11` path. The repository now contains a compatible dependency-graph remediation, so this file records both the completed source migration and the remaining release-level regression work.

The remediation does **not** replace CareNest's local-first persistence architecture, database schema, domain/application contracts, encrypted-document model, or backup format. It changes package resolution only.

## Non-negotiable rules

1. Do not claim a dependency advisory is fixed solely because a direct package declaration changed; verify the resolved direct/transitive graph.
2. Do not widen `NuGetAuditSuppress` or use severity-wide/wildcard suppression.
3. Do not change database libraries without testing migrations, existing databases, backup/restore, encrypted-document references, cascade behavior and concurrency/WAL behavior.
4. Do not silently alter stored health records to accommodate a provider migration.
5. Keep the database local-first; a dependency migration must not introduce a backend/account/network requirement.
6. If security remediation and data-integrity evidence conflict, block release and investigate rather than shipping a known migration/data regression.

## Selected path A — compatible package upgrade

CareNest retained the existing `sqlite-net-pcl` API/persistence path and used central transitive pinning for the maintained native/provider leaves.

### Old package graph relevant to the advisory

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.11`;
- narrow `NuGetAuditSuppress` entry for `GHSA-2m69-gcr7-jv3q`.

### Current source package graph

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- `SQLitePCLRaw.provider.e_sqlite3` `2.1.12`;
- `SQLitePCLRaw.provider.sqlite3` `2.1.12`;
- `SQLitePCLRaw.provider.dynamic_cdecl` `2.1.12`;
- no `NuGetAuditSuppress` entry for the advisory.

The bundle package remaining at `2.1.11` is intentional. The earlier assumption that a missing `SQLitePCLRaw.bundle_green 2.1.12` prevented remediation was incorrect because central transitive pinning can select the newer compatible native/provider leaves independently.

### Source commits

- [x] `66cd701f84afd5021a28e7e3327b7da4fad249aa` — pin patched SQLite native/provider path.
- [x] `e939d5bd912d09ffa150c804519c15e2506b7bd7` — remove the exact NuGet audit suppression.
- [x] `04868965c43d8a6d09b40075d92f20da9b26e32a` — add dependency-security regression contract.

### Source migration checklist

- [x] Update `Directory.Packages.props` with the verified native/provider pins.
- [x] Keep central transitive pinning enabled.
- [x] Remove the obsolete exact advisory suppression from `Directory.Build.props`.
- [x] Add a regression contract that rejects restoration of the vulnerable pin floor.
- [x] Add a regression contract that rejects restoration of the advisory suppression.
- [x] Re-run unsuppressed NuGet vulnerability reporting on platform-neutral projects.
- [x] Re-run unsuppressed Android MAUI dependency audit/restore.
- [x] Update `docs/security/DEPENDENCY_RISK_REGISTER.md` with the selected graph and evidence.

Unsuppressed audit checkpoints already succeeded on PR #47 (`31765223239`), PR #48 (`31765388861`) and PR #50 (`31765668949`). Those PRs were superseded for unrelated moving-source reasons and are dependency-remediation evidence, not final release baselines.

## Fallback path B — replace sqlite-net-pcl/native dependency path

This fallback is **not currently selected**. Retain it for future use if a later advisory cannot be resolved through a compatible package graph.

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

## Required regression suite after the SQLite dependency change

### Automated core tests

The final exact-source verification must record the actual pass counts for the source that includes the dependency remediation and all later runtime fixes.

- [ ] Unit tests all pass on the final source.
- [ ] Integration tests all pass on the final source.
- [ ] UI-contract tests all pass on the final source.
- [ ] Database initialization succeeds on a clean database through the automated integration suite.
- [ ] Migration/version contracts remain green.
- [ ] Foreign-key contracts remain green.
- [ ] Repository CRUD round-trips remain green.
- [ ] Cascade behavior remains green.
- [ ] WAL/snapshot behavior remains green.
- [ ] Busy-timeout/concurrency contracts remain green.
- [ ] Backup snapshot creation tests remain green.
- [ ] Encrypted backup restore tests remain green.
- [ ] Wrong-password and tampered-backup tests remain green.
- [ ] Encrypted-document metadata/reference backup/restore tests remain green.
- [ ] Report/export tests remain green.
- [ ] `SqliteDependencySecurityContractTests` passes.

### Platform builds/security automation

- [ ] Android Release on the final source.
- [ ] Windows Release on the final source.
- [ ] iOS simulator Release on the final source.
- [ ] Mac Catalyst Release on the final source.
- [ ] CodeQL on the final source.
- [ ] Dependency Audit on the final source with no matching suppression.

These boxes are intentionally tied to the final source rather than retroactively checking them from a superseded checkpoint.

### Manual migration/device tests

These cannot be manufactured by source code or GitHub-hosted compilation and remain production-release work:

- [ ] Upgrade a representative device/emulator installation containing fictional `1.0.0-rc.1` data.
- [ ] Verify profiles, medicines, schedules, reminder occurrences, medication logs, appointments, documents, stock and tags after upgrade.
- [ ] Verify old encrypted document payloads still decrypt through the existing key path.
- [ ] Verify a pre-remediation backup restores correctly after the dependency update when the backup schema is unchanged.
- [ ] Verify a new backup restores on a clean install.
- [ ] Verify reminder rebuild/reconciliation after upgrade.
- [ ] Verify reset/delete operations remove expected application-owned data.
- [ ] Verify real notification behavior on the supported packaged targets.

## Database-behavior decision

No intentional database schema or stored-record semantic change is part of this dependency remediation.

Expected invariants:

- existing schema migrations remain the source of schema evolution;
- SQLite foreign-key behavior remains enabled through the existing repository initialization path;
- existing transaction boundaries remain unchanged;
- existing WAL/snapshot/backup semantics remain unchanged unless a test explicitly proves otherwise;
- encrypted document payloads and their key material remain outside the SQLite native-package decision;
- backup archive format/framing remains unchanged by the dependency pins;
- no health record is rewritten merely because the native SQLite package version changed.

Any contrary behavior is a regression and triggers the rollback rule below.

## Evidence to record

The active release/status documents must record:

- old direct/transitive package graph;
- new direct/transitive package graph;
- source commit SHAs;
- exact final CI/CodeQL/Dependency Audit run IDs;
- final automated test totals;
- target build results;
- manual migration/device evidence when performed;
- confirmation that `NuGetAuditSuppress` was removed;
- any database behavior intentionally changed (currently: none).

Primary evidence locations:

- `docs/security/DEPENDENCY_RISK_REGISTER.md`;
- `PROJECT_STATUS.md`;
- `docs/releases/NEXT_STEPS.md`;
- `what_changed.md`.

## Rollback rule

If the dependency update causes data corruption, migration failure, backup incompatibility, platform build regressions, reminder-state inconsistencies, or other persistence regressions, do not ship it merely to clear the advisory.

Required response:

1. preserve the failing automated/manual evidence;
2. determine whether the defect is package-resolution, native-provider, application, migration, or test-contract related;
3. correct the source or revert to a safer known graph;
4. if reverting reintroduces an advisory, restore an explicit narrowly scoped risk entry rather than hiding it;
5. re-run the complete final-source matrix before promotion.

The goal is both security and data integrity, not a green vulnerability report at the cost of user data reliability.
