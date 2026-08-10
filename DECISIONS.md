# Architectural Decisions

## ADR summary

1. **Local-first only for v1.** No account, network dependency, automatic cloud upload, or remote caregiver sharing.
2. **Layered solution.** Domain is framework-independent; Application defines use cases/contracts; Infrastructure owns SQLite, encryption and exports; App owns MAUI/platform UI.
3. **SQLite with schema migrations.** Versioned migrations and integrity checks keep restore and upgrades deterministic.
4. **Sensitive documents encrypted individually.** Imported health documents are AES-256-GCM encrypted with a per-installation random key stored through the platform secure secret store.
5. **Database-at-rest limits are explicit.** The SQLite database remains protected primarily by each platform application sandbox; full database encryption is not claimed.
6. **No dosage model.** Medicine strength and instruction are opaque user-entered text. Scheduling never computes a dose.
7. **Occurrences are materialized.** Future reminder occurrences are generated idempotently from user schedules so state changes and recovery are auditable.
8. **Notification payloads are privacy-minimized.** Default notification labels are generic; document contents and sensitive notes are never logged.
9. **Manual encrypted backups.** Backup archives use PBKDF2-HMAC-SHA256 + AES-256-GCM and require a user password; there is no background cloud upload.
10. **PDF reports are informational.** Exported reports carry a privacy and non-clinical disclaimer and avoid scoring or treatment conclusions.
11. **MVVM without a mandatory UI toolkit dependency.** Small internal observable/command primitives keep the app testable and reduce dependency surface.
12. **Platform notification limitations are surfaced.** The app reports permission, battery/exact-alarm limitations, and rebuilds schedules on startup and observed system changes where platform APIs permit.
13. **Runtime exception logging is metadata-only.** Global, UI, startup and reminder error paths do not pass full exception objects, messages, stack traces, health-record identifiers, document paths, or user-entered health text to the CareNest logger. Safe exception type/category metadata is evaluated only when the corresponding log level is enabled.
14. **Repository policy is executable.** Local-first, non-clinical, no-placeholder, no-common-secret-file, architecture, ViewModel, data-model, branding, async-safety and logging-privacy boundaries are protected by automated contract/policy tests in addition to documentation and review.
15. **Formatting is a platform-neutral CI gate.** Shared, Domain, Application, Infrastructure and all test projects must pass `dotnet format --verify-no-changes`; MAUI target compilation remains isolated to platform jobs with the appropriate workloads.
16. **Exact-head verification uses marker-only branches.** When a fresh PR-triggered matrix is required, a temporary branch is created from the exact intended `main` source SHA and adds only a marker under `build/verification/`. Any source change makes that PR stale; stale marker PRs are closed without merge and recreated from the corrected head.
17. **Dependency audit suppression is not remediation.** The narrowly scoped `GHSA-2m69-gcr7-jv3q` NuGet audit exception exists only to keep the remaining build/test pipeline observable while the compatible SQLite dependency path remains unavailable. The risk stays open until a verified patched/replacement path or explicit release decision exists.
18. **Release evidence is separate from release permission.** Automated CI, CodeQL, Dependency Audit and Release Evidence artifacts are necessary evidence but do not authorize production release by themselves. Manual device/accessibility testing, current store-policy review, signing/package validation and open-risk review remain independent release gates.
19. **Voluntary funding is outside the health-product entitlement model.** The fixed external Buy Me a Coffee destination opens only after explicit user action, receives no CareNest health payload, and never changes health behavior, reminder priority, emergency handling, support priority, or access to local data.
