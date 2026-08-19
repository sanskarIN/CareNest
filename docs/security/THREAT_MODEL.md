# CareNest Threat Model

**Release line:** `1.0.0-rc.1`  
**Accepted automated baseline before current backup hardening:** `b6eecae66f74bd72bcb20d93508355542f9f3442`

This threat model covers local structured records, encrypted documents, manual backups, app lock, reminders/appointments, exports, dependency/build automation, funding-free package policy, internal inspection artifacts and production provenance.

CareNest is organizational software. It does not claim clinical correctness, emergency-service behavior, guaranteed reminders or protection against a fully compromised device/OS.

## 1. Current automated evidence

Accepted source `b6eecae66f74bd72bcb20d93508355542f9f3442` passed:

- 355/355 core tests;
- all four configured Release platform builds;
- all four store-candidate configurations;
- Android/Windows/Apple inspection-artifact workflows;
- CodeQL;
- unsuppressed Dependency Audit;
- strict compiled XAML binding policy.

Any verification-relevant source change after that accepted source must pass a fresh exact-source matrix before replacing this baseline.

This is source/automation evidence, not manual production security approval.

## 2. Assets

Protected/sensitive assets include:

- profile/person/contact data;
- medicine/schedule/reminder/log data;
- appointments;
- stock/refill records;
- document metadata/tags;
- encrypted imported document payloads;
- document master key;
- backup archives/password-derived key material during operations;
- app-lock salt/verifier state;
- settings/audit data;
- OS reminder/appointment request state;
- release source identity/dependency/workflow/package/signing provenance.

## 3. Trust boundaries

### Primary CareNest boundary

- CareNest process;
- app sandbox/private local files;
- SQLite database;
- encrypted document-vault storage;
- app-owned cache/staging while CareNest owns it;
- platform secure storage through CareNest abstractions.

### External/separate boundaries

- OS notification/alarm service;
- calendar/file/share/browser APIs;
- external files/apps/cloud drives;
- store/distribution services;
- source/build infrastructure;
- OS/device backups;
- rooted/jailbroken/compromised environments.

## 4. Threat/control matrix

| Threat | Current controls | Residual risk |
|---|---|---|
| Casual access to unlocked app | Optional app lock; generic notifications | Unlocked/compromised device can expose data |
| App-lock guessing | Salted PBKDF2-HMAC-SHA256; fixed-time comparison | Numeric PIN entropy can be limited |
| Partial app-lock update | Multi-key rollback/fail closed | Process/OS termination can interrupt compensation |
| Raw private-file theft | OS sandbox; encrypted document payloads | SQLite is not transparently whole-database encrypted |
| Stolen backup | Password-derived AES-256-GCM | Weak password reduces protection |
| Tampered/truncated backup | AEAD + authenticated v2 terminal + strict package validation | Malformed authenticated inputs can still consume bounded local resources before rejection |
| Malicious backup topology/resource expansion | Entry allowlist/count/path validation plus pre-manifest per-entry/total uncompressed limits | Inputs within configured limits still consume local disk/CPU during validation |
| Missing/corrupt document key | Fail closed; no unrelated replacement key | Real data becomes unrecoverable if genuine key is lost |
| Partial document import | DB/filesystem compensation | Abrupt termination can interrupt rollback |
| Plaintext export retention | Explicit handoff; app-owned staging cleanup | External copies cannot be remotely revoked |
| Formula-like CSV user text | Neutralization before output | Destination software may transform data independently |
| Sensitive memory copies | Zero known mutable app-owned buffers where practical | Runtime/OS copies cannot be universally erased |
| Sensitive logging | Privacy-minimized logging/source contracts | OS/third-party logs remain external |
| Network/telemetry creep | Local-first source-policy tests | Future approved network features create new threat surfaces |
| Duplicate/stale reminders | Deterministic IDs + reconciliation | OS scheduler can fail/cancel asynchronously |
| Handled state while old request active | Cancellation-first transitions | Abrupt termination across state surfaces remains possible |
| Action failure after cancellation | Previous-state restore/rebuild attempt | Recovery can also fail |
| Delete diverges from OS scheduler | Cancel before DB cascade + rebuild compensation | DB and OS are never one transaction |
| Future snooze incorrectly overdue | `SnoozedUntilUtc` as effective due time | User-entered time can still be wrong |
| Permission denied but scheduling claimed | Explicit permission result/rebuild behavior | Permission can change externally |
| UTC clock-kind reinterpretation | True UTC validation | Earlier user/system conversion can still be incorrect |
| Missed notifications | diagnostics/reconciliation/platform messaging | shutdown/force-stop/battery/vendor policy can prevent delivery |
| Vulnerable SQLite path reintroduced | central pins, source contracts, blocking audit | future advisories remain possible |
| SQLite update breaks data | tests + mandatory packaged compatibility | CI cannot represent every real installed database/device |
| Audit bypass | fail-closed scripts/workflows/contracts | maintainer can intentionally weaken policy if review fails |
| Release tag bypass | seven configured tagged workflows | humans can still ignore manual gates |
| External funding marker reintroduced into app | source-policy guard + package scanner | new packaging/toolchain can require renewed inspection |
| Internal artifact mistaken for production | non-production provenance/no prod secrets | humans can still mislabel redistributed artifacts |
| Signing secret committed | source hygiene/policy + external signing requirement | external secret systems can still be compromised |
| Export/browser link leaks health identifiers | fixed explicit links; no health query data | provider receives ordinary browser/network metadata |

## 5. Local-first boundary

No required CareNest backend stores normal v1 health-organizer records.

Accounts, synchronization, remote caregiver collaboration, analytics or server storage require a new/expanded threat model before implementation.

## 6. Structured SQLite boundary

SQLite data is local but not advertised as whole-database encrypted. Protection depends on sandbox/device security, application validation/repository boundaries, transactions/migrations and integrity-aware backups.

A compromised device can bypass much of this boundary.

## 7. SQLite dependency/provider boundary

Current graph intent:

- sqlite-net-pcl `1.9.172`;
- bundle_green `2.1.11`;
- lib.e_sqlite3 `3.53.3`;
- Android/provider leaves `2.1.12` where pinned;
- central transitive pinning;
- no former exact advisory suppression.

Security-clean dependencies and real packaged compatibility are separate requirements.

## 8. Encrypted stream boundary

New document/backup streams use chunked AES-256-GCM framing v2 with authenticated counters/lengths/terminal and rejection of trailing data.

Legacy v1 remains readable for documented compatibility. V1 is not described as retroactively strengthened.

Removing legacy support would require genuine fixtures, migration/recovery design and explicit validation.

## 9. Backup topology and resource boundary

After decryption CareNest validates archive metadata before manifest deserialization, then validates package topology/manifest/database/key/document layout before extraction.

Invalid/duplicate/unexpected/nested/count-mismatched content fails rather than being accepted merely because AEAD authentication succeeded.

Default resource ceilings are also enforced before manifest parsing/extraction: 1 MiB manifest, 1 GiB SQLite database, 512 MiB per encrypted document, 2 GiB total uncompressed payload and 5,000 documents. Backup creation applies the same topology/resource validator before encryption so current CareNest does not intentionally emit a backup outside its restore boundary.

These ceilings bound, rather than eliminate, local CPU/disk consumption. Changing them is security- and compatibility-relevant and requires regression plus packaged compatibility review.

## 10. Document import consistency boundary

Filesystem ciphertext and SQLite metadata/audit state cannot share one ACID transaction. CareNest uses compensating cleanup and surfaces incomplete rollback rather than silently claiming consistency.

Abrupt process/OS termination remains residual risk.

## 11. App-lock boundary

App lock is intended for casual local privacy, not device compromise/whole-database encryption. Weak PINs and compromised secure storage remain risks.

## 12. Reminder/platform integrity boundary

Reminder planning is deterministic organizational logic, not clinical inference.

CareNest protects ownership/time-zone/UTC/date/state/DST rules and reconciles OS requests through effective snooze due time, cancellation-before-replacement/suppression, cancellation-first handled actions and rebuild/restoration compensation.

OS permission/battery/shutdown/force-stop/vendor policy remains outside deterministic guarantees.

## 13. External export/browser boundary

Document/report/profile/calendar exports and normal repository/legal/support browser actions are explicit user handoffs. CareNest does not control data after external transfer.

The distributed app contains no external Buy Me a Coffee funding destination. The repository support URL is repository-only and must remain separate from health functionality/data.

## 14. Internal inspection-artifact boundary

The dedicated workflow generates non-production Android/Windows/iOS-simulator/Mac-Catalyst inspection output for exact-source package scanning/provenance.

Artifacts are deliberately unsigned/unpackaged/simulator-targeted where applicable, carry checksums/provenance and do not cross the production signing boundary.

They are not evidence of store approval.

## 15. Release automation boundary

Production-style `v*` tags use:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- Store Package Configuration;
- Store Inspection Artifacts;
- Release Gate;
- Release Evidence.

These automation controls do not replace real-device/accessibility/package/signing/store approval.

## 16. Application funding/package boundary

The old per-package funding visibility architecture is removed.

Current invariant:

- no `CareNestShowFundingLink` application property;
- no BMC destination/card/command/artwork in app runtime/source/package;
- repository-only voluntary project support;
- package scanner prevents accidental canonical-marker regression;
- funding never changes health/reminder/medical functionality.

## 17. Out of scope for current v1 threat model

- CareNest server compromise, because no required CareNest backend exists;
- remote caregiver authorization/revocation, because remote collaboration is not current v1;
- clinical correctness, because CareNest is not medical decision support;
- security guarantees for external destinations after explicit handoff;
- protection against an equivalent-privilege fully compromised environment.

## 18. Security review triggers

A new security/privacy architecture review is mandatory before adding/materially changing:

- accounts/authentication;
- cloud sync/remote collaboration;
- analytics/crash-state upload;
- document interpretation/medical decision support;
- in-app external payment/funding SDK/surface;
- biometric/remote PIN/key recovery;
- encrypted format/key ownership/legacy migration;
- backup topology/resource ceilings or extraction behavior;
- raw SQL/import execution paths;
- release/audit/evidence weakening;
- package signing/provenance logic.

## 19. Current remaining production security evidence

Before public production:

- complete supported-platform manual matrices;
- verify actual notification permission/delivery/recovery limitations;
- complete packaged SQLite/encrypted-data compatibility;
- complete accessibility/privacy presentation checks;
- review current store privacy/policy requirements;
- configure production signing outside Git;
- inspect final signed packages/checksums/provenance;
- require exact immutable production tag and all tagged gates.

## Related documents

- `SECURITY.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/security/LOGGING_PRIVACY.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/releases/SECURITY_RELEASE_REVIEW.md`
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`
