# CareNest Dependency Risk Register

**Release line:** `1.0.0-rc.1`  
**Current verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

This register tracks dependency advisories/remediation and separates source dependency security from packaged existing-data compatibility.

## 1. Current status

The previously tracked SQLite advisory path `GHSA-2m69-gcr7-jv3q` is **resolved in the current source dependency graph**.

The former exact `NuGetAuditSuppress` entry has been removed and remains absent.

Current source is audited without that suppression.

## 2. Current SQLite package strategy

Central package intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
- `SQLitePCLRaw.provider.e_sqlite3` `2.1.12`;
- `SQLitePCLRaw.provider.sqlite3` `2.1.12`;
- `SQLitePCLRaw.provider.dynamic_cdecl` `2.1.12`;
- central transitive pinning enabled.

The bundle can remain at its compatible available version while maintained native/provider leaves are centrally pinned.

## 3. Source remediation history

Key remediation commits retained in history include:

- `66cd701f84afd5021a28e7e3327b7da4fad249aa` — pin patched SQLite native dependency path;
- `e939d5bd912d09ffa150c804519c15e2506b7bd7` — remove resolved SQLite audit suppression;
- `04868965c43d8a6d09b40075d92f20da9b26e32a` — guard patched SQLite dependency baseline.

Earlier PR #47/#48/#50/#54 evidence remains useful history for the remediation process, but it is not the current overall CareNest source baseline.

## 4. Regression guard

`tests/CareNest.UiTests/SqliteDependencySecurityContractTests.cs` protects:

- maintained native/provider version floors;
- absence of the old exact audit suppression;
- central dependency-security intent.

Do not weaken this contract merely to make a package update restore.

## 5. Current authoritative automated evidence

PR #74 frozen source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Dependency Audit #91 / run `31938301172` succeeded on both configured platform-neutral and MAUI graphs with the former suppression absent.

The same source also passed:

- CareNest CI #735 / `31938301209` with **331/331** core tests;
- Android, Windows, iOS simulator and Mac Catalyst Release builds;
- Store Package Configuration #124 / `31938301146`;
- Store Inspection Artifacts #47 / `31938301275`;
- CodeQL #735 / `31938301252`.

Permanent current evidence: `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

## 6. What the remediation did not intentionally change

The package pin remediation did not intentionally change:

- schema/entity meaning;
- migration semantics;
- backup format;
- encrypted document key model;
- reminder schedule semantics;
- user health-data transformation;
- local-first account/network boundary.

That intent does not replace packaged compatibility testing.

## 7. Packaged compatibility remains a separate gate

Before production promotion with the current dependency path, validate representative fictional existing data through realistic packages:

- database opens/integrity passes;
- profiles/medicines/schedules/reminders/logs/appointments/documents/stock/tags/settings remain readable/editable;
- reminder reconciliation succeeds;
- no duplicate/stale OS requests;
- existing encrypted documents remain decryptable through unchanged key path;
- current backups restore;
- genuine historical backups/documents restore where real prior fixtures exist and documented compatibility applies.

A clean dependency audit is not proof of these behaviors.

## 8. Future dependency exception rules

1. Prefer upgrade/replacement over suppression.
2. If a temporary exception is unavoidable, record exact advisory/package/path/reason/owner/expiry here.
3. Never use wildcard/package-family/severity-wide suppression merely to obtain green CI.
4. Verify the resolved transitive graph, not only direct declarations.
5. Add regression contracts for remediated pins/suppressions when practical.
6. Re-run unit/integration/UI-source-policy tests, affected platform builds, CodeQL and Dependency Audit.
7. For persistence/crypto/native provider changes, perform packaged compatibility.
8. Remove an exception as soon as a verified compatible path exists.
9. Block/revert an update that corrupts data or breaks compatibility even when audit is clean.
10. Keep changes consistent with local-first/privacy/non-clinical product boundaries.

## 9. Current open dependency risks

No current exact SQLite advisory suppression remains in source.

This register must be updated if a new dependency advisory, compatible remediation decision or approved temporary exception affects the release line.

## 10. Review triggers

Review this register for:

- every SQLite/sqlite-net-pcl/SQLitePCLRaw change;
- every release candidate;
- new GitHub/NuGet advisory affecting resolved graph;
- MAUI/runtime/tooling dependency changes with security implications;
- new network/telemetry/crypto dependency;
- production release approval.

## Related documents

- `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`
- `docs/releases/SECURITY_RELEASE_REVIEW.md`
- `PROJECT_STATUS.md`
- `docs/releases/NEXT_STEPS.md`