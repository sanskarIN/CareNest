# CareNest Security Release Review

Complete this review against the exact commit/tag/package proposed for public release.

## 1. Candidate identity

- [ ] Exact source SHA/tag recorded.
- [ ] Version/build/application identity recorded.
- [ ] Final package filename/SHA-256 recorded.
- [ ] Signing/notarization/store provenance recorded.
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

Current accepted automated source evidence is PR #74, not the older PR #54/#56 checkpoints.

## 10. XAML/UI source security/quality

- [ ] Source binding compilation remains enabled.
- [ ] Strict XAML compilation remains enabled.
- [ ] `XC0022`–`XC0025` remain errors.
- [ ] Binding-bearing views/templates remain accurately typed.
- [ ] No warning/type-safety bypass introduced.
- [ ] Accessibility/privacy/medical limitation surfaces remain reachable.

## 11. Application funding/package boundary

Current invariant:

- no external Buy Me a Coffee destination/card/command/artwork in distributed application source/package;
- no `CareNestShowFundingLink` application build property;
- repository-only support documentation remains separate;
- funding never changes health/reminder/medical behavior.

Release checks:

- [ ] Source-policy guard passes.
- [ ] Store payload scanner self-test passes.
- [ ] Final signed package forbidden-marker scan passes.
- [ ] About has no BMC funding action/card.
- [ ] Store listing/screenshots do not imply removed in-app funding behavior.

## 12. Release-engineering security controls

- [ ] CareNest CI passes exact production tag.
- [ ] CodeQL passes exact production tag.
- [ ] Dependency Audit passes exact production tag.
- [ ] Store Package Configuration passes exact production tag.
- [ ] Store Inspection Artifacts passes exact production tag.
- [ ] Release Gate passes exact production tag.
- [ ] Release Evidence passes exact production tag.
- [ ] Release Evidence provenance/checksums reviewed.
- [ ] Release-preflight/quality-gate audit remains fail closed.
- [ ] Signing secrets remain outside Git/workflow logs.

## 13. Platform/distribution

- [ ] Android permissions/alarm/battery behavior matches product/docs.
- [ ] Apple entitlements/notification behavior matches product/docs.
- [ ] Windows capabilities/reminder limitations match product/docs.
- [ ] Real-device/manual platform matrix complete.
- [ ] Accessibility matrix complete.
- [ ] Current store privacy/data-safety/policy review complete.

## 14. Current automated reference

PR #74 frozen source head:

`8908fa9f5f6d2b47123627e91f5aa5925d34a3c9`

Merged executable source:

`e8f4aa0a2d95c15500fa59b83c5fc715fb202273`

Evidence:

- CareNest CI #735 / `31938301209`: success;
- 122 unit + 39 integration + 170 UI/source-policy = **331/331**;
- all four Release target builds: success;
- Store Package Configuration #124 / `31938301146`: success;
- Store Inspection Artifacts #47 / `31938301275`: success;
- CodeQL #735 / `31938301252`: success;
- Dependency Audit #91 / `31938301172`: success.

Permanent record: `docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`.

This does not pre-approve a production tag/package or complete manual/package/signing/store gates.

## 15. Approval record

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
Signing/notarization provenance:
SQLite packaged compatibility result:
Encrypted document/backup compatibility result:
Accessibility/manual platform result:
Store policy/privacy review result:
Open security blockers:
Approved for signing/package creation: yes/no
Approved for publication: yes/no
Notes:
```