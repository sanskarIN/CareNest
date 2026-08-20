# CareNest Production Quality Gate

**Release line:** `1.0.0-rc.1`  
**Current automated evidence authority:** `docs/releases/AUTOMATED_BASELINE.md`  
**Production evidence standard:** `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`  
**Production evidence index:** `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

CareNest must not be described as globally bug-free. Production promotion is acceptable only when the applicable automated, manual, package, accessibility, signing and store evidence is current for the exact candidate.

Do not pin a moving accepted SHA/test total in this stable policy file. Use `docs/releases/AUTOMATED_BASELINE.md` for the latest actually observed exact-source automation.

## 1. Source quality

Required:

- nullable/analyzer policy remains enabled;
- platform-neutral formatting passes;
- project dependency direction remains correct;
- ViewModels do not directly issue SQLite operations or casually create network clients;
- prohibited sync-over-async/async misuse remains guarded;
- new behavior has appropriate lowest-layer regression tests;
- analyzer/compiler failures are fixed rather than broadly suppressed;
- strict XAML compiled-binding policy remains enabled;
- release-documentation consistency contracts remain green;
- production-evidence documentation contracts remain green;
- package-evidence tooling contracts remain green;
- documentation-integrity tooling remains green.

## 2. Strict XAML quality

Required:

- Source binding compilation enabled;
- strict XAML compilation enabled;
- `XC0022`, `XC0023`, `XC0024`, `XC0025` remain errors;
- binding-bearing pages/templates remain accurately typed;
- picker/display bindings remain typed where context changes;
- explicit Source/ancestor bindings remain typed;
- no matching `NoWarn`, `x:Object` or `x:Null` bypass.

## 3. Product safety

Required:

- no diagnosis;
- no dosage calculation/inference;
- no treatment recommendation;
- no clinical interaction/risk scoring;
- no emergency-service replacement;
- no guaranteed reminder-delivery claim;
- medicine strength/instructions remain opaque user text;
- schedules derive only from explicit user input;
- as-needed schedules create no automatic occurrence;
- inactive/archived states suppress automatic reminder materialization as documented.

## 4. Reminder integrity

Required source/test behavior includes:

- true UTC planning boundaries;
- half-open planning windows;
- deterministic time-zone/DST handling;
- ownership/state validation;
- stable occurrence identity/deduplication;
- valid future UTC snooze;
- `SnoozedUntilUtc` as effective due time;
- stale OS request reconciliation;
- cancellation before replacement/suppression/invalidation;
- cancellation-first handled actions;
- retryable platform cancellation failure;
- compensation when later persistence/platform work fails;
- appointment true-UTC/permission/compensation behavior.

## 5. Persistence quality

Required:

- ordered/idempotent migrations;
- transactional multi-step consistency where needed;
- repository abstraction instead of direct ViewModel SQL;
- WAL/snapshot/integrity behavior remains tested;
- destructive cleanup/rollback is explicit;
- dependency security is not confused with packaged data compatibility.

## 6. Document-vault quality

Required:

- authenticated encrypted payloads;
- current framing for new writes;
- documented legacy read compatibility;
- tamper/truncation/trailing-data rejection;
- missing/corrupt key fail closed;
- no silent unrelated replacement key for existing ciphertext;
- import/export/delete cleanup/rollback;
- explicit plaintext export boundary.

## 7. Backup/restore quality

Required:

- authenticated password-encrypted backups;
- versioned format;
- wrong-password/tamper/truncation/trailing-data rejection;
- strict archive topology;
- bounded decrypted-container/archive resource use;
- SQLite snapshot/integrity validation;
- encrypted-document recovery material rules;
- rollback/cleanup after failed restore;
- legacy compatibility where documented;
- current backup creation checked against current restore/resource limits.

Current source resource ceilings remain documented by the backup architecture/security records and the packaged compatibility evidence template.

## 8. App-lock quality

Required:

- no plaintext PIN persistence;
- random salt;
- PBKDF2-HMAC-SHA256 verifier;
- fixed-time comparison;
- secure-store ownership;
- strict material validation;
- rollback/fail-closed behavior;
- app lock described as a privacy barrier, not whole-database encryption.

## 9. Privacy/security quality

Required:

- no required CareNest account/backend in current v1;
- no automatic CareNest cloud sync/upload;
- no hidden runtime analytics/telemetry client;
- no committed signing/credential secrets;
- privacy-minimized logging;
- no raw health/document/PIN/password/key data in normal diagnostics;
- CodeQL passes;
- unsuppressed Dependency Audit passes;
- former SQLite advisory suppression remains absent;
- dependency floors remain protected by source-policy tests.

## 10. External-commerce package quality

Required:

- no external Buy Me a Coffee destination/card/command/artwork in the distributed application package;
- no external Gumroad destination/card/command/artwork in the distributed application package;
- repository-only support/storefront surfaces remain separate;
- purchase/funding never creates health/reminder/medical entitlement or local-health-data access;
- forbidden-marker scanning remains fail closed;
- no obsolete application external-commerce toggle is required for store builds.

Repository-only markers:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

## 11. Automated test/build quality

Read the current accepted exact-source result from:

`docs/releases/AUTOMATED_BASELINE.md`

The final candidate must have current successful evidence for the applicable configured matrix, including:

- unit tests;
- integration tests;
- UI/source-policy tests;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- Store Package Configuration;
- Store Inspection Artifacts;
- CodeQL;
- unsuppressed Dependency Audit.

If verification-relevant source changed after the accepted automated boundary, run fresh exact-source verification. Do not reuse an older test count as proof for a newer head.

## 12. Release-engineering quality

Required configuration includes:

- CI, CodeQL and Dependency Audit for applicable PR/tag/manual paths;
- Store Package Configuration;
- Store Inspection Artifacts;
- fail-closed Release Gate;
- Release Evidence with exact source/ref/run identity and checksums;
- production evidence standard/index/templates present and protected by source-policy tests;
- fail-closed package evidence tooling with synthetic self-test coverage;
- release/documentation integrity tooling;
- production-style `v*` tag coverage for applicable workflows.

## 13. Manual platform evidence

Before production complete actual release-specific records using `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`.

Required evidence covers real notification permission/delivery/lifecycle behavior, Android alarm/battery/reboot behavior, Windows limitation behavior, Apple real-device behavior, reminder actions/reconciliation, files/backups/app lock and themes/accessibility.

## 14. Packaged compatibility evidence

Before production verify with fictional/synthetic representative data:

- packaged SQLite upgrade/open/integrity/readability/editability;
- schema version/migrations;
- reminder reconciliation;
- encrypted documents;
- current backup/restore;
- wrong-password/tamper/truncation/trailing-data behavior;
- clean-install restore;
- genuine historical fixtures only where genuine prior bytes exist.

A green dependency audit is not a substitute.

Use `docs/releases/templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md`.

## 15. Accessibility evidence

Manual evidence must cover representative:

- screen readers;
- large text/scaling;
- keyboard/focus;
- contrast/themes;
- reduced motion;
- color-independent meaning;
- destructive confirmation readability;
- privacy-safe errors.

Use `docs/releases/templates/ACCESSIBILITY_VALIDATION_RECORD.md`.

## 16. Production signing/package evidence

Required:

- signing identities/secrets configured outside Git;
- exact source/package identity recorded;
- final signed package SHA-256 recorded;
- signing/notarization/store provenance recorded without secrets;
- final package scan for `buymeacoffee.com/sanskarIN`;
- final package scan for `ramsandesh.gumroad.com`;
- structured package evidence JSON generated with `build/scripts/create-package-evidence.py --stage production`;
- source tag, checked-out HEAD and recorded source SHA agree;
- tracked workspace is clean;
- installed/smoke result recorded.

Package evidence guide:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

## 17. Store/policy evidence

Preliminary policy review:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

At submission time verify current channel rules and actual candidate behavior for:

- health-organizer claims;
- reminder limitations;
- privacy/data safety;
- permissions/capabilities;
- fictional screenshots;
- support/privacy/terms/security links;
- package identity;
- external-commerce policy;
- live Google Play Health apps declaration/Data safety where applicable;
- current Apple privacy/store metadata where applicable;
- current Microsoft/Partner Center privacy/store metadata where applicable.

The dated preliminary review does not replace the final submission-date gate.

## 18. Exact production tag evidence

The approved immutable `v*` tag must complete every configured required tag gate:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

Do not move a failed/rejected tag to different source.

## 19. Final production rule

Use `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md` for evidence semantics and `docs/releases/templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md` for the final release-specific approval record.

Any required quality gate that is failed, stale, unknown, blocked or not actually performed blocks production promotion unless explicitly justified as `N/A`.

Current status remains `1.0.0-rc.1` until applicable production evidence is complete.
