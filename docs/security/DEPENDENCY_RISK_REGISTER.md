# Dependency Risk Register

This register tracks dependency advisories that affect the CareNest release line and documents any temporary exception, remediation, and release evidence. A successful restore/build is not by itself a security resolution; the actual resolved dependency graph, NuGet audit result, regression matrix, and release state all matter.

## Resolved-in-source dependency remediation

### GHSA-2m69-gcr7-jv3q — SQLite native package resolved by sqlite-net-pcl

- **Status:** Resolved in the current source dependency graph; release-level manual/platform evidence remains part of the normal RC1 production checklist.
- **Originally observed by:** GitHub Actions / NuGet audit during CareNest `1.0.0-rc.1` verification.
- **Old resolved native path:** `SQLitePCLRaw.lib.e_sqlite3` `2.1.11` and the Android native variant `2.1.11` through the `sqlite-net-pcl` / `SQLitePCLRaw.bundle_green` dependency chain.
- **Old exception:** `Directory.Build.props` temporarily suppressed only `GHSA-2m69-gcr7-jv3q`. No severity-wide or wildcard audit suppression was used.
- **Correction to the old investigation:** the earlier repository note that the `2.1.12` maintenance path did not exist was too broad. `SQLitePCLRaw.bundle_green` remains at the available `2.1.11` bundle version, but newer compatible native/provider leaves are available and can be selected through central transitive pinning.
- **Current package strategy:** keep `sqlite-net-pcl` `1.9.172` and `SQLitePCLRaw.bundle_green` `2.1.11`, while centrally pinning the shipping native/provider leaves to the maintained path.
- **Current native/provider pins:**
  - `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
  - `SQLitePCLRaw.lib.e_sqlite3.android` `2.1.12`;
  - `SQLitePCLRaw.provider.e_sqlite3` `2.1.12`;
  - `SQLitePCLRaw.provider.sqlite3` `2.1.12`;
  - `SQLitePCLRaw.provider.dynamic_cdecl` `2.1.12`.
- **Remediation commits on `main`:**
  - `66cd701f84afd5021a28e7e3327b7da4fad249aa` — `fix: pin patched SQLite native dependency path`;
  - `e939d5bd912d09ffa150c804519c15e2506b7bd7` — `security: remove resolved SQLite audit suppression`;
  - `04868965c43d8a6d09b40075d92f20da9b26e32a` — `test: guard patched SQLite dependency baseline`.
- **Audit suppression:** removed. `Directory.Build.props` no longer contains `NuGetAuditSuppress` or the advisory identifier.
- **Regression guard:** `tests/CareNest.UiTests/SqliteDependencySecurityContractTests.cs` requires the patched native/provider floor and requires that the old audit suppression not return.
- **Unsuppressed audit evidence already observed during remediation:**
  - PR #47 Dependency Audit #28 / run `31765223239`: success;
  - PR #48 Dependency Audit #29 / run `31765388861`: success;
  - PR #50 Dependency Audit #31 / run `31765668949`: success.
- **Important evidence boundary:** PRs #47, #48 and #50 were superseded for unrelated moving-source reasons and are not final release baselines. Their successful unsuppressed audits prove the dependency graph can restore/audit without the exception; the latest exact-source CareNest CI/CodeQL/platform verification must still be used for release promotion.
- **Data-boundary continuity:** the remediation does not introduce a server, account, remote database listener, cloud synchronization requirement, telemetry client, or user-controlled raw SQL path. CareNest remains local-first and continues to use the existing repository/application persistence boundary.
- **Database behavior intent:** no schema, entity, migration semantic, backup format, document-key model, or user-visible health-record transformation was intentionally changed by the package pin update.
- **Manual release follow-through:** upgrade/install, existing-database, backup/restore, encrypted-document, reminder-rebuild, and packaged-target checks remain required under the normal release matrix. They must not be inferred from NuGet audit alone.
- **Review trigger:** every SQLite/sqlite-net-pcl/SQLitePCLRaw update, every release candidate, any new advisory, and any persistence-provider change.
- **Owner documents:** `docs/releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`, `docs/releases/NEXT_STEPS.md`, `PROJECT_STATUS.md`, and `what_changed.md` record the release-level evidence and remaining manual work.

## Historical exception timeline

The temporary exception existed only to keep unrelated CI failures observable while no verified repository-compatible remediation had yet been established. It was intentionally narrow and visible.

The remediation process used several superseded checkpoints because `main` was concurrently receiving reminder-reconciliation fixes:

1. PR #47 proved the unsuppressed dependency graph restored/audited successfully, but `main` advanced afterward.
2. PR #48 again passed unsuppressed Dependency Audit and CodeQL, while its combined CI snapshot exposed an unrelated transient reminder-interface compile break on the moving base.
3. The reminder interface/source was corrected/simplified on `main`; PR #48 was closed without merge.
4. PR #50 again passed unsuppressed Dependency Audit, but its source snapshot predated later analyzer-safe reminder test fixes.
5. The three SQLite remediation changes were then committed directly to `main` so all subsequent source verification automatically exercises the patched graph instead of repeatedly rebasing a parallel dependency PR.

No failed/superseded marker PR is represented as final release evidence.

## Rules for dependency exceptions and remediation

1. Never suppress an advisory without recording the exact advisory identifier and dependency path here.
2. Never use a wildcard, package-family-wide, or severity-wide suppression to make CI green.
3. Prefer upgrading or replacing the dependency over retaining an exception.
4. Verify the **resolved** transitive graph, not only the direct package declarations.
5. Re-run unit, integration, UI-contract, platform build, CodeQL, Dependency Audit, backup/restore, migration, and document tests after SQLite-related dependency changes.
6. Remove a matching `NuGetAuditSuppress` entry as soon as a verified compatible dependency path exists.
7. Add a regression contract when practical so a vulnerable pin or obsolete suppression cannot silently return.
8. Do not mark public `1.0.0` production release work complete solely because the dependency advisory is resolved; manual device, encrypted-data, accessibility, store, signing, and release-evidence gates remain separate.
9. If a future SQLite update causes data corruption, migration failure, backup incompatibility, reminder-state inconsistency, or target build regression, block/revert that update even if its vulnerability audit is otherwise clean.
10. Keep dependency decisions consistent with CareNest's local-first privacy and non-clinical safety boundaries.
