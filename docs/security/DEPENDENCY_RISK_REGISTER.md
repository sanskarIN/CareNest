# Dependency Risk Register

This register tracks dependency advisories that affect the CareNest release line and documents any temporary exception that prevents CI from hiding the risk.

## Open dependency exception

### GHSA-2m69-gcr7-jv3q — SQLite native package resolved by sqlite-net-pcl

- **Status:** Open; temporary audit suppression only.
- **Observed by:** GitHub Actions / NuGet audit during CareNest `1.0.0-rc.1` verification.
- **Affected resolved package:** `SQLitePCLRaw.lib.e_sqlite3` / Android variant `2.1.11` through the current `sqlite-net-pcl` dependency chain.
- **Attempted upgrade:** `2.1.12` was tested and NuGet.org reported that no such `SQLitePCLRaw.bundle_green` version exists; the nearest available release was `2.1.11`.
- **Build behavior:** `Directory.Build.props` suppresses this exact advisory URL so the CI pipeline can continue to compile and test the rest of the application. No other NuGet advisory is suppressed.
- **Release impact:** This exception must remain visible in release review. A final production release should either consume an available dependency version that resolves the advisory or migrate the persistence/native-SQLite dependency path after compatibility testing.
- **Data-boundary mitigation:** CareNest remains local-first, does not expose a remote database listener, does not ingest arbitrary SQL from users, and uses application-controlled parameterized repository operations. These properties reduce exposure but do not make the advisory disappear.
- **Review trigger:** Every dependency update, every release candidate, and any NuGet/SQLite dependency change.

## Rules for exceptions

1. Never suppress an advisory without recording the exact advisory identifier and dependency path here.
2. Never use a wildcard or severity-wide suppression.
3. Prefer upgrading or replacing the dependency over retaining an exception.
4. Re-run unit, integration, platform build, backup/restore, migration, and document tests after changing SQLite-related dependencies.
5. Remove the matching `NuGetAuditSuppress` entry as soon as the dependency chain can restore and build with a verified patched version.
