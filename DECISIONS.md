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
