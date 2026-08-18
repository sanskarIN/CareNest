# CareNest Security Release Review

**Release line:** `1.0.0-rc.1`  
**Latest verified Gumroad implementation/source-policy source:** `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`  
**Current store-policy review:** `docs/releases/STORE_POLICY_REVIEW_20260818.md`

Complete this review against the exact commit/tag/package proposed for public release.

## 1. Candidate identity

- [ ] Exact source SHA/tag recorded.
- [ ] Version/build/application identity recorded.
- [ ] Final package filename/SHA-256 recorded.
- [ ] Signing/notarization/store provenance recorded.
- [ ] Structured package evidence JSON recorded.
- [ ] Candidate corresponds to the source actually reviewed.

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
- [ ] Handled actions use cancellation-first ordering and compensation.
- [ ] Profile/medicine delete/save reminder reconciliation remains correct.
- [ ] Appointment `StartsUtc` is true UTC.
- [ ] Notification denial does not become successful scheduling.

## 5. Secrets/app lock/cryptography

- [ ] No private signing/service/API credentials committed.
- [ ] App-lock PIN not stored plaintext.
- [ ] PBKDF2-HMAC-SHA256/salt/fixed-time verifier behavior intact.
- [ ] App-lock material validation/rollback/fail-closed behavior intact.
- [ ] App lock described as privacy barrier, not database encryption.
- [ ] New documents/backups use current authenticated chunked framing v2.
- [ ] Authenticated terminal/truncation/trailing-data protections intact.
- [ ] Legacy v1 read compatibility remains intentional/documented.
- [ ] Existing v1 ciphertext not described as retroactively upgraded.
- [ ] Cryptographic key/password material excluded from diagnostics.
- [ ] Sensitive mutable buffers cleared where practical.

## 6. Document-vault consistency

- [ ] Import DB failure removes just-created encrypted payload.
- [ ] Later audit failure attempts metadata/payload rollback.
- [ ] Cleanup is not intentionally abandoned because caller cancellation fired.
- [ ] Incomplete rollback is surfaced.
- [ ] Export uses safe output filename/path behavior.
- [ ] Application-owned temporary plaintext/cache files cleaned best effort.
- [ ] Missing/corrupt required document key fails closed.
- [ ] No unrelated replacement key silently generated for existing ciphertext.

## 7. Logging/diagnostics

- [ ] `docs/security/LOGGING_PRIVACY.md` remains accurate.
- [ ] No raw health/document content logged.
- [ ] No backup password/PIN/key/signing secret logged.
- [ ] Sensitive operation logs avoid unnecessary exception messages/stack traces/identifiers.
- [ ] Diagnostic exports remain privacy-minimized.

## 8. Persistence/backup

- [ ] SQLite migrations/integrity tests pass.
- [ ] Relationship/cascade cleanup tests pass.
- [ ] WAL/snapshot/integrity behavior passes.
- [ ] Backup topology rejects duplicate/unexpected/invalid entries.
- [ ] Wrong-password/tamper/truncation/trailing-data restore fails closed.
- [ ] Document-bearing backup key material validates correctly.
- [ ] Packaged existing-data upgrade/readability/editability test completed with fictional data.
- [ ] Existing encrypted documents remain decryptable through unchanged key path.
- [ ] Current/genuine historical backup compatibility tested where real prior fixtures exist.

## 9. Dependency security

Current source remediation state:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- Android/provider leaves `2.1.12` where pinned;
- former `GHSA-2m69-gcr7-jv3q` exact suppression removed;
- source-policy tests protect maintained floors/suppression absence.

Release checks:

- [ ] CodeQL passes for exact production tag.
- [ ] Dependency Audit passes for exact production tag without restored suppression.
- [ ] `DEPENDENCY_RISK_REGISTER.md` reviewed.
- [ ] SQLite packaged compatibility evidence reviewed separately from audit result.
- [ ] Final Release Evidence records actual resolved dependency graph.

## 10. XAML/UI source security/quality

- [ ] Source binding compilation remains enabled.
- [ ] Strict XAML compilation remains enabled.
- [ ] `XC0022`–`XC0025` remain errors.
- [ ] Binding-bearing views/templates remain accurately typed.
- [ ] No warning/type-safety bypass introduced.
- [ ] Accessibility/privacy/medical limitation surfaces remain reachable.
- [ ] Release-documentation consistency contracts pass.
- [ ] Package-evidence tooling contracts pass.

## 11. External-commerce application-package boundary

Current invariant:

- no external Buy Me a Coffee destination/card/command/artwork in distributed application source/package;
- no external Gumroad destination/card/command/artwork in distributed application source/package;
- no `CareNestShowFundingLink` application build property;
- repository-only support/storefront documentation remains separate;
- funding/purchase never changes health/reminder/medical behavior or local-health-data access.

Release checks:

- [ ] Source-policy guard passes.
- [ ] Store payload scanner self-test passes.
- [ ] Package-evidence synthetic self-test passes.
- [ ] Final signed package scan passes for `buymeacoffee.com/sanskarIN`.
- [ ] Final signed package scan passes for `ramsandesh.gumroad.com`.
- [ ] About/runtime has no Buy Me a Coffee funding action/card.
- [ ] About/runtime has no Gumroad storefront/purchase action/card.
- [ ] Store listing/screenshots do not imply in-app Gumroad/Buy Me a Coffee behavior under the current package policy.

## 12. Structured package provenance evidence

Use:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

Release checks:

- [ ] `build/scripts/create-package-evidence.py --stage production` used for every final production package.
- [ ] Production evidence source tag begins with `v` and resolves to the recorded source SHA.
- [ ] Checked-out HEAD equals the recorded source SHA.
- [ ] Tracked workspace is clean.
- [ ] Package evidence signing/notarization provenance contains no secrets.
- [ ] Package evidence payload SHA-256 matches independently recorded final package evidence.
- [ ] Store-safe scan result is `passed`.
- [ ] Generated JSON is retained outside the package payload.

The tool does not sign packages and does not prove store approval; independent platform/store signing/notarization evidence remains required.

## 13. Release-engineering security controls

- [ ] CareNest CI passes exact production tag.
- [ ] CodeQL passes exact production tag.
- [ ] Dependency Audit passes exact production tag.
- [ ] Store Package Configuration passes exact production tag.
- [ ] Store Inspection Artifacts passes exact production tag.
- [ ] Release Gate passes exact production tag.
- [ ] Release Evidence passes exact production tag.
- [ ] Release Evidence provenance/checksums reviewed.
- [ ] Release-preflight/quality-gate audit remains fail closed.
- [ ] Package-evidence Python syntax/self-test remains part of CareNest CI.
- [ ] Signing secrets remain outside Git/workflow logs.

## 14. Platform/distribution

- [ ] Android permissions/alarm/battery behavior matches product/docs.
- [ ] Apple entitlements/notification behavior matches product/docs.
- [ ] Windows capabilities/reminder limitations match product/docs.
- [ ] Real-device/manual platform matrix complete.
- [ ] Accessibility matrix complete.
- [ ] Submission-date store privacy/data-safety/policy review complete.
- [ ] Live Google Play Health apps/Data safety declarations complete where applicable.
- [ ] Apple privacy/store metadata complete where applicable.
- [ ] Microsoft/Partner Center privacy/store metadata complete where applicable.

## 15. Current automated reference

Latest exact verified Gumroad implementation/source-policy source:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

Evidence on that exact source:

- 122 unit + 39 integration + 175 UI/source-policy = **336/336**;
- all four Release target builds: success;
- all four Store Package Configuration targets: success;
- CodeQL: success.

Permanent current Gumroad record:

`docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`

The repository now contains later verification-relevant release-documentation/package-evidence tests/scripts. Those later changes require their own exact-source workflow evidence before they replace the verified baseline above.

This automated baseline does not pre-approve a production tag/package or complete manual/package/signing/store gates.

## 16. Store-policy review reference

Preliminary dated review:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

The dated review is not store approval and does not replace final live policy/store-console review for the exact production package/listing.

## 17. Approval record

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
Accessibility/manual platform result:
Store policy review date/sources:
Google Play Health apps declaration:
Google Play Data safety:
Apple privacy metadata:
Microsoft privacy metadata:
BMC package-marker scan:
Gumroad package-marker scan:
Open security blockers:
Approved for signing/package creation: yes/no
Approved for publication: yes/no
Notes:
```
