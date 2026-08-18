# CareNest Production Quality Gate

**Release line:** `1.0.0-rc.1`  
**Latest verified Gumroad implementation/source-policy source:** `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`

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
- strict XAML compiled-binding policy remains enabled;
- release-documentation consistency contracts remain green;
- package-evidence tooling contracts remain green.

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

## 10. External-commerce application-package quality

Required:

- no external Buy Me a Coffee destination/card/command/artwork under distributed application runtime/package source;
- no external Gumroad destination/card/command/artwork under distributed application runtime/package source;
- repository-only voluntary support/storefront surfaces remain separate;
- purchase/funding never creates health/reminder/medical entitlement or local-health-data access;
- forbidden-marker package scanner remains fail closed for both repository-only markers;
- no obsolete application external-commerce toggle is required for store builds.

Repository-only markers:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

## 11. Automated test/build quality

Latest accepted exact verified Gumroad implementation/source-policy evidence:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

Verified on that exact source:

- 122/122 unit;
- 39/39 integration;
- 175/175 UI/source-policy;
- **336/336 total**;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- Store Package Configuration: success on all four targets;
- CodeQL: success.

Authoritative record:

`docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`

The repository now also contains additional release-documentation and package-evidence-tooling tests/scripts after that verified source. Those later verification-relevant changes require their own exact-source CI evidence before they can replace the verified baseline above.

## 12. Release-engineering quality

Required configuration includes:

- CI, CodeQL and Dependency Audit for applicable PR/tag/manual paths;
- Store Package Configuration;
- Store Inspection Artifacts;
- fail-closed Release Gate;
- Release Evidence with exact source/ref/run identity and checksums;
- fail-closed package evidence tooling with synthetic self-test coverage;
- quality/release preflight that treats required dependency audit failures as blocking;
- repository-local Git identity helpers that fail closed;
- production-style `v*` tag coverage for all applicable workflows.

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
- final signed package scan for `buymeacoffee.com/sanskarIN`;
- final signed package scan for `ramsandesh.gumroad.com`;
- structured package evidence JSON generated with `build/scripts/create-package-evidence.py --stage production`;
- production package evidence source tag, checked-out HEAD and source SHA all match;
- production package evidence tracked workspace is clean;
- install/smoke test of final candidate.

Package evidence guide:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

## 17. Store/policy evidence

Preliminary policy review:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

At submission time verify current channel rules and actual candidate behavior for:

- health-organizer claims;
- notification limitations;
- privacy/data-safety;
- permissions/capabilities;
- screenshots with fictional data;
- support/privacy/terms/security links;
- final package identity;
- absence of in-app Gumroad/Buy Me a Coffee surface from listing/screenshots under the current package policy;
- live Google Play Health apps declaration and Data safety where applicable;
- current Apple privacy/store metadata where applicable;
- current Microsoft/Partner Center privacy/store metadata where applicable.

The dated preliminary review does not replace this final submission-date gate.

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
