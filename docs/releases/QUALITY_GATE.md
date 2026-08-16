# CareNest Production Quality Gate

**Release line:** `1.0.0-rc.1`  
**Verified executable source:** `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

CareNest must not be described as globally bug-free. Production promotion is acceptable only when the applicable automated, manual, package, accessibility, signing and store evidence below is complete for the exact candidate.

## 1. Source quality

Required:

- nullable/analyzer policy remains enabled;
- platform-neutral formatting passes;
- project dependency direction remains correct;
- ViewModels do not directly issue SQLite operations or casually create network clients;
- prohibited sync-over-async/async misuse remains guarded by source-policy tests;
- new behavior has appropriate lowest-layer regression tests;
- analyzer/compiler failures are fixed rather than broadly suppressed;
- strict XAML compiled-binding policy remains enabled.

## 2. Strict XAML quality

Required:

- Source binding compilation enabled;
- strict XAML compilation enabled;
- `XC0022`, `XC0023`, `XC0024`, `XC0025` remain errors;
- binding-bearing pages have accurate root `x:DataType`;
- binding-bearing DataTemplates have item-specific types;
- picker display bindings are typed where context changes;
- explicit Source/ancestor bindings are typed;
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
- restoration/rebuild compensation when later persistence/platform work fails;
- appointment true-UTC/permission/compensation behavior.

## 5. Persistence quality

Required:

- ordered/idempotent migrations;
- transactional multi-step consistency where needed;
- repository abstraction instead of direct ViewModel SQL;
- WAL/snapshot/integrity behavior remains tested;
- destructive cleanup/rollback is explicit;
- source dependency security is not confused with packaged data compatibility.

## 6. Document-vault quality

Required:

- authenticated encrypted payloads;
- current v2 framing for new writes;
- documented legacy v1 read compatibility;
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
- SQLite snapshot/integrity validation;
- encrypted-document recovery material rules;
- rollback/cleanup after failed restore;
- legacy compatibility where documented.

## 8. App-lock quality

Required:

- no plaintext PIN persistence;
- random salt;
- PBKDF2-HMAC-SHA256 verifier;
- fixed-time comparison;
- secure-store ownership;
- strict material validation;
- rollback/fail-closed behavior;
- app lock described as privacy barrier, not whole-database encryption.

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
- former SQLite exact suppression remains absent;
- dependency floors remain protected by source-policy tests.

## 10. Application funding/package quality

Required:

- no external Buy Me a Coffee destination/card/command/artwork under distributed application runtime/package source;
- repository-only voluntary support remains separate;
- funding never creates health/reminder/medical entitlement;
- forbidden-marker package scanner remains fail closed;
- no obsolete application funding-link toggle is required for store builds.

## 11. Automated test/build quality

Current accepted PR #74 evidence:

- 122/122 unit;
- 39/39 integration;
- 170/170 UI/source-policy;
- **331/331 total**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- Store Package Configuration: success on all four targets;
- Store Inspection Artifacts: success;
- CodeQL: success;
- Dependency Audit: success.

This evidence belongs to the exact PR #74 source head `8908fa9f5f6d2b47123627e91f5aa5925d34a3c9` and merged executable source `e8f4aa0a2d95c15500fa59b83c5fc715fb202273`.

## 12. Release-engineering quality

Required configuration includes:

- CI, CodeQL and Dependency Audit for applicable PR/tag/manual paths;
- Store Package Configuration;
- Store Inspection Artifacts;
- fail-closed Release Gate;
- Release Evidence with exact source/ref/run identity and checksums;
- quality/release preflight that treats required dependency audit failures as blocking;
- repository-local Git identity helpers that fail closed;
- production-style `v*` tag coverage for all seven applicable workflows.

## 13. Manual platform evidence

Before production complete applicable rows in:

- `docs/releases/MANUAL_TEST_MATRIX.md`;
- `docs/PLATFORM_BEHAVIOR_MATRIX.md`.

This includes real notification permission/delivery/lifecycle behavior, Android alarm/battery/reboot behavior, Windows limitation behavior, Apple real-device behavior, reminder actions/reconciliation, files/backups/app lock and themes/accessibility.

## 14. Packaged compatibility evidence

Before production verify with fictional representative data:

- packaged SQLite upgrade/open/integrity/readability/editability;
- schema version/migrations;
- reminder reconciliation;
- existing/current encrypted documents;
- current backup/restore;
- wrong-password/tamper/truncation behavior;
- genuine historical fixtures where real prior bytes exist.

A green NuGet audit is not a substitute.

## 15. Accessibility evidence

Manual evidence must cover representative:

- screen readers;
- large text/scaling;
- keyboard/focus;
- contrast/themes;
- reduced motion;
- color-independent meaning;
- destructive confirmation readability.

## 16. Production signing/package evidence

Required:

- signing identities/secrets configured outside Git;
- exact source/package identity recorded;
- final signed package SHA-256 recorded;
- signing/notarization/store provenance recorded;
- forbidden-marker scan on final signed package;
- install/smoke test of final candidate.

## 17. Store/policy evidence

At submission time verify current channel rules and actual candidate behavior for:

- health-organizer claims;
- notification limitations;
- privacy/data-safety;
- permissions/capabilities;
- screenshots with fictional data;
- support/privacy/terms/security links;
- final package identity;
- absence of removed in-app BMC funding surface from listing/screenshots.

## 18. Exact production tag evidence

The approved immutable `v*` tag must complete:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

Do not move a failed/rejected tag to different source.

## 19. Final production rule

Any required quality gate that is failed, stale, unknown or not actually performed blocks production promotion unless explicitly documented as non-applicable with a defensible reason.

Current status remains `1.0.0-rc.1` until the applicable production evidence is complete.