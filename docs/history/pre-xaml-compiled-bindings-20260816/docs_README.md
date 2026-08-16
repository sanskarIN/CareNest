# CareNest Documentation

This directory is the canonical documentation hub for CareNest `1.0.0-rc.1`.

CareNest is a local-first .NET MAUI family health organizer. It is an organizational product, not a diagnostic, treatment, dosage-calculation, medication-interaction, clinical-risk, or emergency-service system.

---

## Current executable source and exact automated verification

Authoritative executable source:

`9ec7b4e7d2150d9cc50be19f30464080318b16e8`

Authoritative exact marker verification:

- PR #68 — `Verify final CareNest bug-audit source`;
- marker SHA — `c752815c311e7e443f1d71df8a9197cf706a14b6`;
- marker file — `build/verification/final-bug-audit-20260815.txt`;
- marker PR closed without merge.

Final automated evidence:

- CareNest CI #719 / run `31880955724`: success;
- formatting: success;
- 122 unit tests: passed;
- 39 integration tests: passed;
- 164 UI/source-policy tests: passed;
- **325/325 total core tests**: passed;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- Store Package Configuration #108 / run `31880955723`: all four target configurations success;
- Store Inspection Artifacts #41 / run `31880955734`: scanner self-test plus Android/Windows/Apple payload scans success;
- CodeQL #719 / run `31880955720`: success;
- unsuppressed Dependency Audit #85 / run `31880955731`: success.

Permanent final evidence:

- [`releases/FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md`](releases/FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md)

Earlier PR #54/#56/#58/#59/#61 records remain historical evidence for their own frozen source boundaries.

---

## Final external-funding package boundary

The CareNest application runtime/package does **not** contain or expose the Buy Me a Coffee project-funding destination.

During the final audit, Windows package scanning repeatedly found the funding destination inside `CareNest.App.dll` even when previous source/build flags evaluated false. The root cause was the MAUI SVG resource `buy_me_a_coffee_carenest.svg`, whose accessibility/text content contained the full destination and was embedded into the Windows managed payload.

The final source removes:

- the app funding destination;
- the About funding command/card;
- funding-policy source units;
- funding-specific build toggles;
- packaged funding/support artwork carrying the destination.

Voluntary project funding remains repository-documentation-only where appropriate. Package byte scanning remains a defense-in-depth release gate.

References:

- [`releases/STORE_BUILD_POLICY.md`](releases/STORE_BUILD_POLICY.md)
- [`releases/PACKAGED_RELEASE_VALIDATION.md`](releases/PACKAGED_RELEASE_VALIDATION.md)
- [`../SUPPORT.md`](../SUPPORT.md)
- [`../BUY_ME_A_COFFEE.md`](../BUY_ME_A_COFFEE.md)

---

## Primary project references

Start here:

- [`COMPLETE_PROJECT_DOCUMENTATION.md`](COMPLETE_PROJECT_DOCUMENTATION.md) — end-to-end project reference.
- [`CODEBASE_REFERENCE.md`](CODEBASE_REFERENCE.md) — source-project/file map and responsibilities.
- [`CONFIGURATION_REFERENCE.md`](CONFIGURATION_REFERENCE.md) — packages, build/analyzer/audit/platform/workflow configuration.
- [`MAINTENANCE_AND_OPERATIONS.md`](MAINTENANCE_AND_OPERATIONS.md) — maintenance, bug-fix, dependency, release, hotfix and incident operations.
- [`../PROJECT_STATUS.md`](../PROJECT_STATUS.md) — current final automated source boundary and real production blockers.
- [`../what_changed.md`](../what_changed.md) — active final continuation handoff.
- [`releases/NEXT_STEPS.md`](releases/NEXT_STEPS.md) — only the production-validation work still required.

---

## User documentation

- [`USER_GUIDE.md`](USER_GUIDE.md)
- [`FEATURE_REFERENCE.md`](FEATURE_REFERENCE.md)
- [`REPORTS_AND_EXPORTS.md`](REPORTS_AND_EXPORTS.md)
- [`GLOSSARY.md`](GLOSSARY.md)
- [`SUPPORT_CARENEST.md`](SUPPORT_CARENEST.md)
- [`../PRIVACY.md`](../PRIVACY.md)
- [`../TERMS.md`](../TERMS.md)
- [`../SUPPORT.md`](../SUPPORT.md)
- [`../SECURITY.md`](../SECURITY.md)

---

## Architecture

- [`architecture/ARCHITECTURE.md`](architecture/ARCHITECTURE.md)
- [`architecture/APPLICATION_FLOWS.md`](architecture/APPLICATION_FLOWS.md)
- [`architecture/SERVICE_BOUNDARIES.md`](architecture/SERVICE_BOUNDARIES.md)
- [`architecture/DATABASE_SCHEMA.md`](architecture/DATABASE_SCHEMA.md)
- [`architecture/DATA_STORAGE_AND_EXPORT.md`](architecture/DATA_STORAGE_AND_EXPORT.md)
- [`architecture/BACKUP_AND_RESTORE.md`](architecture/BACKUP_AND_RESTORE.md)
- [`architecture/DOCUMENT_VAULT.md`](architecture/DOCUMENT_VAULT.md)
- [`architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md)
- [`architecture/ADR-0001-local-first.md`](architecture/ADR-0001-local-first.md)
- [`architecture/ADR-0002-reminder-occurrences.md`](architecture/ADR-0002-reminder-occurrences.md)
- [`architecture/ADR-0003-encrypted-backup-format.md`](architecture/ADR-0003-encrypted-backup-format.md)

Intended dependency direction:

```text
CareNest.Shared <- CareNest.Domain <- CareNest.Application <- CareNest.Infrastructure <- CareNest.App
```

Platform-neutral projects do not depend on MAUI. ViewModels do not issue SQL directly. Current local-first v1 runtime code does not casually add network/telemetry clients.

---

## Product safety boundary

CareNest does **not**:

- diagnose conditions;
- determine, calculate, or infer medicine dosage;
- recommend treatment;
- perform clinical medication-interaction checking;
- create clinical risk scores;
- replace a clinician/pharmacist;
- provide emergency services;
- guarantee notification delivery.

Medicine strength/instruction values remain opaque user-entered text. Reminder schedules come only from explicit user-entered schedule values.

---

## Local-first and privacy boundary

Current v1 intentionally has:

- no required CareNest account/backend;
- no automatic CareNest cloud synchronization/upload;
- no hidden runtime analytics/telemetry client;
- local structured SQLite records;
- separately encrypted imported document payloads;
- password-encrypted manual backups;
- explicit outbound export/share/calendar/browser actions;
- privacy-minimized application logs.

Exported/shared copies can leave the CareNest-controlled boundary and may be retained by the chosen destination, OS, cloud service, screenshots, or backups.

References:

- [`privacy/PRIVACY_MODEL.md`](privacy/PRIVACY_MODEL.md)
- [`privacy/DATA_LIFECYCLE.md`](privacy/DATA_LIFECYCLE.md)
- [`privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md`](privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md)
- [`../PRIVACY.md`](../PRIVACY.md)

---

## Reminder/platform contracts

Primary references:

- [`testing/REMINDER_SCHEDULING_CONTRACT.md`](testing/REMINDER_SCHEDULING_CONTRACT.md)
- [`architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`](architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md)
- [`testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`](testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md)

Current source includes effective snooze due time, stale OS-request reconciliation, cancellation-before-replacement/suppression/invalidation, persistence compensation, cancellation-first handled actions, appointment reminder compensation, and restart/recovery hardening.

Real platform notification behavior still requires manual device evidence before production promotion.

---

## SQLite and data security

References:

- [`architecture/DATABASE_SCHEMA.md`](architecture/DATABASE_SCHEMA.md)
- [`security/DEPENDENCY_RISK_REGISTER.md`](security/DEPENDENCY_RISK_REGISTER.md)
- [`releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md`](releases/SQLITE_DEPENDENCY_MIGRATION_PLAN.md)

The former `GHSA-2m69-gcr7-jv3q` source dependency exception is remediated, maintained SQLite native/provider leaves are pinned, and the old exact audit suppression is removed.

A green dependency audit does not replace packaged existing-database upgrade/readability/integrity validation.

---

## Encryption and backup

- [`architecture/DOCUMENT_VAULT.md`](architecture/DOCUMENT_VAULT.md)
- [`architecture/BACKUP_AND_RESTORE.md`](architecture/BACKUP_AND_RESTORE.md)
- [`security/SECURITY_MODEL.md`](security/SECURITY_MODEL.md)
- [`security/THREAT_MODEL.md`](security/THREAT_MODEL.md)

New encrypted document/backup streams use authenticated chunked AEAD framing v2 with truncation/trailing-data protections while retaining required v1 read compatibility.

Canonical historical fixture evidence remains a production-validation item when genuine prior fixture bytes are available.

---

## Testing and verification

- [`testing/TESTING_GUIDE.md`](testing/TESTING_GUIDE.md)
- [`testing/TEST_PLAN.md`](testing/TEST_PLAN.md)
- [`testing/REMINDER_SCHEDULING_CONTRACT.md`](testing/REMINDER_SCHEDULING_CONTRACT.md)
- [`testing/SETTINGS_LIFECYCLE_CONTRACT.md`](testing/SETTINGS_LIFECYCLE_CONTRACT.md)
- [`testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md`](testing/BUG_AUDIT_REGRESSION_MATRIX_20260814.md)
- [`releases/VERIFICATION_BRANCH_PROTOCOL.md`](releases/VERIFICATION_BRANCH_PROTOCOL.md)

Exact marker verification is required again if verification-relevant runtime/test/project/workflow/build/release-script source changes after the frozen source boundary.

---

## Release documentation

Current:

- [`releases/FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md`](releases/FINAL_STORE_PAYLOAD_AND_BUG_AUDIT_VERIFICATION_20260815.md)
- [`releases/STORE_BUILD_POLICY.md`](releases/STORE_BUILD_POLICY.md)
- [`releases/PACKAGED_RELEASE_VALIDATION.md`](releases/PACKAGED_RELEASE_VALIDATION.md)
- [`releases/NEXT_STEPS.md`](releases/NEXT_STEPS.md)
- [`releases/RELEASE_PROCESS.md`](releases/RELEASE_PROCESS.md)
- [`releases/RELEASE_CHECKLIST.md`](releases/RELEASE_CHECKLIST.md)
- [`releases/QUALITY_GATE.md`](releases/QUALITY_GATE.md)
- [`releases/MANUAL_TEST_MATRIX.md`](releases/MANUAL_TEST_MATRIX.md)
- [`releases/SECURITY_RELEASE_REVIEW.md`](releases/SECURITY_RELEASE_REVIEW.md)
- [`releases/STORE_SUBMISSION_CHECKLIST.md`](releases/STORE_SUBMISSION_CHECKLIST.md)
- [`releases/RELEASE_EVIDENCE.md`](releases/RELEASE_EVIDENCE.md)

Historical evidence:

- [`releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md`](releases/STORE_INSPECTION_ARTIFACTS_VERIFICATION_20260815.md) — PR #61.
- [`releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md`](releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md) — PR #59.
- [`releases/PACKAGED_RELEASE_HARDENING_VERIFICATION_20260815.md`](releases/PACKAGED_RELEASE_HARDENING_VERIFICATION_20260815.md) — PR #58.
- [`releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md`](releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md) — PR #56.
- [`releases/FINAL_BUG_AUDIT_VERIFICATION_20260814.md`](releases/FINAL_BUG_AUDIT_VERIFICATION_20260814.md) — PR #54.
- [`releases/BUG_AUDIT_VERIFICATION_20260814.md`](releases/BUG_AUDIT_VERIFICATION_20260814.md) — failure-driven audit history.

Exact pre-final active handoff/status files are preserved under:

`history/pre-final-bug-audit-20260815/`

---

## Production state

The source-complete RC1 has **no known automated defect under the configured PR #68 test/build/security/package-inspection matrix**.

It is not yet production-published because real-device/accessibility/package-compatibility/signing/store/tag evidence remains incomplete.

Do not call CareNest globally bug-free. Use [`releases/NEXT_STEPS.md`](releases/NEXT_STEPS.md) for the exact remaining production work.
