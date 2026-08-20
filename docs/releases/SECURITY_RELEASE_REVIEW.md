# CareNest Security Release Review

**Release line:** `1.0.0-rc.1`  
**Current automated evidence authority:** `docs/releases/AUTOMATED_BASELINE.md`  
**Production evidence standard:** `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`  
**Production evidence index:** `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`  
**Current backup-hardening record:** `docs/releases/BACKUP_RESOURCE_HARDENING_20260819.md`  
**Package evidence guide:** `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

Complete this review against the exact commit/tag/package proposed for public release.

Do not pin a moving accepted source SHA or test total in this stable review. Use `docs/releases/AUTOMATED_BASELINE.md` for the latest actually observed automated boundary.

## 1. Candidate identity

- [ ] Exact source SHA/tag recorded.
- [ ] Version/build/application identity recorded.
- [ ] Final package filename/SHA-256 recorded.
- [ ] Signing/notarization/store provenance recorded without secrets.
- [ ] Structured package evidence JSON recorded.
- [ ] Candidate corresponds to the source actually reviewed.
- [ ] Exact-source automated evidence is current for this candidate.

## 2. Local-first/privacy boundary

- [ ] No required CareNest account/backend introduced without approved architecture review.
- [ ] No hidden runtime analytics/telemetry client introduced.
- [ ] External export/share/calendar/browser actions remain explicit.
- [ ] External fixed links do not attach health/profile/document/reminder identifiers.
- [ ] Whole-database encryption is not falsely claimed.
- [ ] External-copy/deletion limitations remain documented.

## 3. Health/scheduling boundary

- [ ] No diagnosis feature.
- [ ] No dosage calculation/inference.
- [ ] No treatment recommendation.
- [ ] No clinical interaction/risk score.
- [ ] Medicine strength/instruction text remains opaque.
- [ ] Stock math uses explicit user quantities only.
- [ ] Reminder schedules derive only from explicit user input.
- [ ] As-needed schedules create no automatic occurrence.
- [ ] Inactive/archive states suppress automatic reminders as documented.
- [ ] Reminder delivery limitations remain visible and non-guaranteed.

## 4. Reminder integrity

- [ ] Planner ownership/state checks pass.
- [ ] Unknown schedule/weekday values fail safely.
- [ ] Planning windows/rebuild overrides require true UTC.
- [ ] Invalid DST-gap times are not silently replaced.
- [ ] Ambiguous DST times are deterministic.
- [ ] Snooze requires explicit future UTC.
- [ ] `SnoozedUntilUtc` is effective due time.
- [ ] Existing platform requests are cancelled before replacement/suppression/invalidation/handled-state persistence where required.
- [ ] Cancellation failures remain retryable.
- [ ] Handled actions use current cancellation/compensation contract.
- [ ] Profile/medicine delete/save reminder reconciliation remains correct.
- [ ] Appointment `StartsUtc` is true UTC.
- [ ] Notification denial does not become successful scheduling.

## 5. Secrets/app lock/cryptography

- [ ] No private signing/service/API credentials committed.
- [ ] App-lock PIN is not stored plaintext.
- [ ] PBKDF2-HMAC-SHA256/salt/fixed-time verifier behavior intact.
- [ ] App-lock material validation/rollback/fail-closed behavior intact.
- [ ] App lock remains described as a privacy barrier, not database encryption.
- [ ] New protected payloads use current authenticated framing.
- [ ] Terminal/truncation/trailing-data protections remain intact.
- [ ] Configured encrypted-stream plaintext bounds fail closed.
- [ ] Legacy read compatibility remains intentional/documented.
- [ ] Legacy reads obey configured plaintext bounds where required.
- [ ] Existing legacy ciphertext is not described as retroactively upgraded.
- [ ] Cryptographic key/password material excluded from diagnostics.
- [ ] Sensitive mutable buffers cleared where practical.

## 6. Document-vault consistency

- [ ] Import DB failure removes just-created encrypted payload.
- [ ] Later audit failure attempts metadata/payload rollback.
- [ ] Cleanup is not intentionally abandoned because caller cancellation fired.
- [ ] Incomplete rollback is surfaced.
- [ ] Export uses safe output filename/path behavior.
- [ ] Application-owned temporary plaintext/cache files are cleaned best effort.
- [ ] Missing/corrupt required document key fails closed.
- [ ] No unrelated replacement key is silently generated for existing ciphertext.

## 7. Logging/diagnostics

- [ ] `docs/security/LOGGING_PRIVACY.md` remains accurate.
- [ ] No raw health/document content logged.
- [ ] No backup password/PIN/key/signing secret logged.
- [ ] Sensitive operation logs avoid unnecessary sensitive exception data.
- [ ] Diagnostic exports remain privacy-minimized.

## 8. Persistence/backup

- [ ] SQLite migrations/integrity tests pass.
- [ ] Relationship/cascade cleanup tests pass.
- [ ] WAL/snapshot/integrity behavior passes.
- [ ] Backup topology rejects duplicate/unexpected/invalid entries.
- [ ] Backup rejects explicit directory-only archive entries.
- [ ] Backup validates entry count/resource bounds before unsafe manifest/extraction work.
- [ ] Decrypted ZIP container output is bounded during authenticated decryption.
- [ ] Generated backups are checked against current restore/resource limits before encryption.
- [ ] Current default ceilings remain reviewed: 2304 MiB decrypted ZIP, 1 MiB manifest, 1 GiB database, 512 MiB/document, 2 GiB total uncompressed payload, 5,000 documents.
- [ ] Wrong-password/tamper/truncation/trailing-data restore fails closed.
- [ ] Document-bearing backup key material validates correctly.
- [ ] Packaged existing-data upgrade/readability/editability test completed with fictional/synthetic data.
- [ ] Existing encrypted documents remain decryptable through unchanged required key path.
- [ ] Current/genuine historical backup compatibility tested where genuine prior fixtures exist.
- [ ] Any genuine historical fixture above a current resource ceiling is treated as an explicit compatibility/security decision.

Use `docs/releases/templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md` for actual release-specific compatibility evidence.

## 9. Dependency security

Current source dependency floors/decisions are owned by executable package configuration and `docs/DEPENDENCY_AND_TOOLCHAIN_BASELINE.md`.

Release checks:

- [ ] CodeQL passes for exact production source/tag.
- [ ] Dependency Audit passes for exact production source/tag without restored suppression.
- [ ] `docs/security/DEPENDENCY_RISK_REGISTER.md` reviewed.
- [ ] Maintained SQLite/provider package floors remain satisfied.
- [ ] Former SQLite advisory suppression remains absent.
- [ ] No wildcard/severity-wide audit suppression introduced merely to unblock release.
- [ ] SQLite packaged compatibility evidence reviewed separately from audit result.
- [ ] Final Release Evidence records actual resolved dependency graph as configured.

## 10. XAML/UI source security/quality

- [ ] Source binding compilation remains enabled.
- [ ] Strict XAML compilation remains enabled.
- [ ] `XC0022`–`XC0025` remain errors.
- [ ] Binding-bearing views/templates remain accurately typed.
- [ ] No warning/type-safety bypass introduced.
- [ ] Accessibility/privacy/medical limitation surfaces remain reachable.
- [ ] Release-documentation consistency contracts pass.
- [ ] Production-evidence documentation contracts pass.
- [ ] Package-evidence tooling contracts pass.

## 11. External-commerce package boundary

Current invariant:

- no external Buy Me a Coffee destination/card/command/artwork in distributed application source/package;
- no external Gumroad destination/card/command/artwork in distributed application source/package;
- no obsolete `CareNestShowFundingLink` application build property;
- repository-only support/storefront documentation remains separate;
- funding/purchase never changes health/reminder/medical behavior or local-health-data access.

Release checks:

- [ ] Source-policy guard passes.
- [ ] Store payload scanner self-test passes.
- [ ] Package-evidence synthetic self-test passes.
- [ ] Final package scan passes for `buymeacoffee.com/sanskarIN`.
- [ ] Final package scan passes for `ramsandesh.gumroad.com`.
- [ ] Installed runtime has no Buy Me a Coffee funding action/card.
- [ ] Installed runtime has no Gumroad storefront/purchase action/card.
- [ ] Store listing/screenshots do not imply in-app Gumroad/BMC behavior under current policy.

## 12. Structured package provenance evidence

Use:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

Release checks:

- [ ] `build/scripts/create-package-evidence.py --stage production` used for each final production package.
- [ ] Production evidence source tag begins with `v` and resolves to recorded source SHA.
- [ ] Checked-out HEAD equals recorded source SHA.
- [ ] Tracked workspace is clean.
- [ ] Package evidence signing/notarization provenance contains no secrets.
- [ ] Package evidence payload SHA-256 matches independently recorded final package evidence.
- [ ] Store-safe scan result passes.
- [ ] Generated JSON retained outside package payload.

The tool does not sign packages and does not prove store approval.

## 13. Release-engineering security controls

- [ ] CareNest CI passes exact production source/tag.
- [ ] CodeQL passes exact production source/tag.
- [ ] Dependency Audit passes exact production source/tag.
- [ ] Store Package Configuration passes exact production source/tag.
- [ ] Store Inspection Artifacts passes exact production source/tag.
- [ ] Release Gate passes exact production tag.
- [ ] Release Evidence passes exact production tag.
- [ ] Release Evidence provenance/checksums reviewed.
- [ ] Release Gate requires the production evidence standard/index/templates to remain present.
- [ ] Release-preflight/quality-gate audit remains fail closed.
- [ ] Package-evidence Python syntax/self-test remains part of CI.
- [ ] Signing secrets remain outside Git/workflow logs.

## 14. Platform/distribution

- [ ] Android permissions/alarm/battery behavior matches product/docs.
- [ ] Apple entitlements/notification behavior matches product/docs.
- [ ] Windows capabilities/reminder limitations match product/docs.
- [ ] Real-device/manual platform evidence complete.
- [ ] Accessibility evidence complete.
- [ ] Submission-date store privacy/data-safety/policy review complete.
- [ ] Live Google Play Health apps/Data safety declarations complete where applicable.
- [ ] Apple privacy/store metadata complete where applicable.
- [ ] Microsoft/Partner Center privacy/store metadata complete where applicable.

Use the release-specific templates indexed by `docs/releases/PRODUCTION_EVIDENCE_INDEX.md`.

## 15. Automated reference

Read the latest accepted exact-source automated evidence from:

`docs/releases/AUTOMATED_BASELINE.md`

A previous source’s green result does not pre-approve a later source, production tag/package, device matrix, accessibility result, signing state or store outcome.

If verification-relevant source changed, perform fresh exact-source verification according to `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`.

## 16. Store-policy review reference

Preliminary dated review:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

The dated review is not store approval and does not replace final live policy/store-console review for the exact production package/listing.

## 17. Approval record

Use a release-specific copy of `docs/releases/templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md` for the final decision. At minimum retain:

```text
Version/build:
Exact source SHA/tag:
Reviewer:
Review date:
CareNest CI run:
CodeQL run:
Dependency Audit run:
Store Package run:
Store Inspection run:
Release Gate run:
Release Evidence run/artifact:
Signed package filename/SHA-256:
Package evidence JSON:
Package evidence payload SHA-256:
Signing/notarization provenance:
SQLite packaged compatibility result:
Encrypted document/backup compatibility result:
Backup resource-limit compatibility result:
Accessibility/manual platform result:
Store policy review date/sources:
Google Play Health apps declaration:
Google Play Data safety:
Apple privacy metadata:
Microsoft privacy metadata:
BMC package-marker scan:
Gumroad package-marker scan:
Open security blockers:
Approved for publication: yes/no
Notes:
```

A failed, stale, blocked, unknown or `NOT RUN` required item is not `PASS` under `docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md`.
