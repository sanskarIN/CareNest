# CareNest Threat Model

This is the current threat model for CareNest `1.0.0-rc.1`. It covers local structured records, encrypted documents, manual backups, optional app lock, reminders/appointments, exports, dependency/security automation, store-safe source configuration, and production release provenance.

CareNest is organizational software. This threat model does not claim clinical correctness, emergency-service behavior, guaranteed reminders, or protection against a fully compromised device/OS.

## Current automated evidence

Authoritative exact automated source: marker-only PR #59.

- source/base: `8489d19734d6142054156d5b57f2713195c16b65`;
- marker head: `ca58294fb7f7a56ee87da16d938f0f691c3a3c7e`;
- CareNest CI #622 / `31869214132`: success;
- 122 unit + 39 integration + 149 UI-contract/policy = **310/310** tests;
- default Android, Windows, iOS simulator and Mac Catalyst Release builds: success;
- CareNest Store Package Configuration #11 / `31869214047`: success;
- funding-disabled Android, Windows, iOS simulator and Mac Catalyst Release builds: success;
- Bash store-package preflight executable-mode guard: success;
- CodeQL #622 / `31869214042`: success;
- unsuppressed Dependency Audit #44 / `31869214093`: success.

PR #59 was closed without merge. Its marker is not production source.

PR #58 remains historical package/store-policy hardening evidence, PR #56 remains historical release-engineering evidence, and PR #54 remains historical runtime bug-audit evidence.

See `docs/releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md`.

## Assets

- local profile/person data;
- medicine/schedule data;
- reminder occurrences and medication logs;
- appointments;
- stock/refill records;
- document metadata/tags;
- encrypted imported document payloads;
- document master key;
- backup archives;
- backup password-derived key material during an operation;
- app-lock salt/verifier state;
- local settings/audit data;
- notification/appointment platform request state;
- release source identity, dependency state, workflow evidence and signing provenance.

## Trust boundaries

### Primary trusted boundary

- CareNest application process;
- app sandbox/private local files;
- app-owned SQLite database;
- encrypted document payload storage;
- app-owned temporary files while still owned by CareNest;
- platform secure storage used through CareNest abstractions.

### Separate/external boundary

- operating-system notification service;
- calendar/file/share/browser APIs;
- user-selected external destinations;
- third-party cloud drives/apps;
- stores/distribution systems;
- source hosting/build runners;
- rooted/jailbroken/compromised device environments.

## Threat/control matrix

| Threat | Current controls | Residual risk |
|---|---|---|
| Casual access to an unlocked device | Optional app lock; generic notification labels | Visible unlocked UI, OS snapshots and device compromise remain possible |
| App-lock PIN guessing | Numeric PIN policy, random salt, PBKDF2-HMAC-SHA256, fixed-time comparison, verifier-buffer clearing | Weak numeric PIN entropy and compromised secure storage remain risks |
| Partial app-lock secure-store update | Snapshot/rollback of multi-key state | Abrupt process/OS termination can interrupt compensation |
| Raw app-private file theft | OS sandbox; encrypted imported document payloads | Structured SQLite is not transparently whole-database encrypted |
| Stolen backup | Password-derived AES-256-GCM encryption | Weak user password reduces practical protection |
| Tampered backup | AEAD authentication, strict package/topology validation | Malformed input can still cause denial of service within resource limits |
| Encrypted stream prefix truncation | New framing v2 authenticates terminal record against next chunk counter | Legacy v1 remains readable and does not retroactively gain v2 terminal protection |
| Trailing bytes after encrypted payload | Reader requires valid terminal and end-of-stream | Malformed files remain denial-of-service inputs |
| Malicious backup ZIP topology | Strict allowlist, duplicate/nesting/extension/count checks, path containment | Structurally valid huge archives can consume local resources |
| Missing/corrupt document key with existing ciphertext | Fail closed; no silent unrelated replacement key | Legitimate data can become unrecoverable if real key is lost |
| Partial document import | DB/filesystem compensating rollback and aggregate failure | Process/OS termination can interrupt compensation |
| Plaintext export/cache retention | Managed cache/staging, failure cleanup, report share cleanup | Copies already handed to another app/cloud/screenshot/backup are outside CareNest control |
| Spreadsheet formula-like exported user text | Neutralize formula-like CSV string prefixes | Destination software can still transform/import data independently |
| Key bytes remain in memory | Zero mutable caller-owned buffers where practical | Runtime/OS/secure-store/swap/crash-dump copies cannot be universally erased |
| Sensitive logs | Logging privacy policy/contracts; safe category/type metadata | Third-party OS/platform logs are outside full app control |
| Runtime network/telemetry creep | Source policy tests prohibit casual network/telemetry client introduction in local-first v1 | Future approved network features would create new threat surfaces and require new design |
| Duplicate reminder materialization | Stable deterministic occurrence identity/upsert behavior | OS scheduler behavior can still differ from app expectations |
| Stale OS request after schedule/state change | Cancel old platform request before replacement/suppression/invalidation; retain stale occurrence identity until reconciliation | OS cancellation itself can fail; retry/recovery remains necessary |
| Handled state committed while old OS request remains active | Cancellation-first Taken/Skipped/Delayed/Missed/Snoozed/Cancelled transitions | Abrupt termination between independent state surfaces is still possible |
| Reminder action fails after old request cancellation | Restore previous state and non-cancelled rebuild attempt; aggregate failure | Recovery can also fail or process can terminate before recovery |
| Medicine/profile delete diverges from OS scheduler | Cancel future requests before DB cascade; rebuild compensation after persistence failure | Recovery can fail; database and OS scheduler are not one transaction |
| Appointment DB/platform divergence | Explicit compensation/reconciliation | Same non-transactional cross-surface residual risk |
| Future snooze disappears after original time | `SnoozedUntilUtc` used as effective due time | Incorrect user-entered snooze time can still be wrong; no clinical validation |
| Scheduling after notification permission denial | Explicit permission result checked; rebuild does not prompt/schedule while denied | Permission can change externally between checks |
| Appointment clock-kind reinterpretation | Require actual `DateTimeKind.Utc`; reject local/unspecified | User-entered wall-clock conversion can still be wrong before reaching service |
| Missed notifications | diagnostics, permission/capability checks, recovery/rebuild, platform limitation messaging | Shutdown, force-stop, battery policy, OS restrictions can delay/prevent delivery |
| Reintroduced vulnerable SQLite native path | Central maintained native/provider pins, suppression absence contract, unsuppressed audit | New future advisories/packages can introduce new risk |
| SQLite native/provider update corrupts existing data | automated persistence/backup tests + mandatory packaged existing-data compatibility | Hosted CI cannot represent every installed database/device/provider combination |
| Dependency audit bypass/suppression | blocking local/CI audit, release policy, source contracts | Maintainer could intentionally change policy; review and exact-source verification required |
| Release tag bypasses candidate gates | exact `v*` tag triggers CI, CodeQL, Dependency Audit, Store Package Configuration, Release Gate, Release Evidence | Manual/store/signing gates can still be ignored by a human if release policy is not followed |
| Store-safe source accidentally reenables external funding surface | `CareNestShowFundingLink=false`, fail-closed store-package wrappers, dedicated false-configuration CI, source-policy contracts | Signed/package tooling outside the verified source path can still be misconfigured; actual artifact inspection remains required |
| Store-safe wrapper loses executable mode | Git mode `100755` plus `test -x` in Store Package Configuration | Non-Git transfer/archive tooling can still alter permissions outside repository evidence |
| Release checklist formatting bypass | fail-closed nested unchecked-row and open-risk detection | Any future parser change must remain covered by source contracts |
| Failed release evidence disappears | evidence components attempted independently; upload runs before aggregate failure | GitHub artifact retention is finite and external archive discipline remains needed |
| Release evidence rerun ambiguity | artifact identity includes commit SHA, run ID and run attempt | Human can still cite the wrong attempt without review |
| Signing key exposure through source | repository secret/signing-file policy; signing kept outside Git | External CI/store/maintainer secret system compromise remains possible |
| Store/funding link leaks health identifiers | fixed support URL, explicit user action, no health-data query parameters | Browser/provider receives normal network/account/payment metadata under its own policy |

## Local-first threat boundary

No CareNest-owned backend exists in current v1. Therefore normal v1 threats do not include a CareNest server storing health records.

If accounts, synchronization, remote caregiver collaboration, analytics or server storage are added later, this threat model must be replaced/expanded before implementation.

## Structured SQLite data

Structured SQLite content is local and not advertised as whole-database encrypted.

Protection depends on:

- application sandbox;
- device security/encryption;
- repository access boundaries;
- input validation;
- transactional operations/migrations;
- integrity-aware backup snapshots.

A rooted/jailbroken/compromised device can bypass many of these controls.

## SQLite dependency/provider boundary

Current verified graph intent:

- `sqlite-net-pcl` `1.9.172`;
- `SQLitePCLRaw.bundle_green` `2.1.11`;
- `SQLitePCLRaw.lib.e_sqlite3` `3.53.3`;
- Android native/provider and selected provider leaves `2.1.12`;
- central transitive pinning enabled;
- no former exact `GHSA-2m69-gcr7-jv3q` audit suppression.

`SqliteDependencySecurityContractTests` protects this policy.

The green dependency graph and packaged existing-data compatibility are separate release requirements.

## Encrypted stream framing boundary

New encrypted document/backup streams use chunked AES-256-GCM framing v2.

Each data record authenticates:

- chunk counter;
- plaintext length;
- ciphertext/tag.

The terminal record is authenticated with the next counter and zero length. Trailing bytes after the terminal are rejected.

Legacy framing v1 remains readable for historical compatibility. V1 is not represented as retroactively strengthened.

A future v1 migration/removal requires canonical historical fixtures, backup/recovery/rollback planning and explicit compatibility verification.

## Backup topology boundary

Decryption success alone is insufficient for restore safety.

Before extraction, CareNest validates the archive topology against the expected manifest/database/key/top-level-document layout.

Controls reject:

- duplicate entries;
- missing required manifest/database;
- unsupported package versions;
- invalid schema/document counts;
- unexpected files;
- nested document paths;
- non-`.cndoc` document entries;
- missing/invalid required document key.

Extraction also uses containment checks as defense in depth.

## Document import consistency boundary

Encrypted payload and SQLite metadata cannot share one filesystem+database ACID transaction.

Normal flow uses compensating cleanup:

1. create encrypted payload;
2. save metadata;
3. save audit;
4. rollback payload on DB failure;
5. rollback metadata + payload on later audit failure;
6. use non-cancelled cleanup attempts where caller cancellation should not knowingly strand the new artifact;
7. surface aggregate cleanup failure.

Abrupt process/OS termination remains a residual consistency risk.

## Sensitive memory boundary

`CryptographicOperations.ZeroMemory` is used on known mutable application-owned sensitive arrays where practical.

This is a lifetime-reduction control, not proof of full erasure from:

- runtime internal copies;
- platform secure-store internals;
- OS/hardware cache;
- swap/hibernation;
- crash dumps;
- debugger/malware with process access.

## App-lock boundary

The app lock protects against casual local access. It does not replace device authentication/security and does not encrypt the entire SQLite database.

A weak user-selected PIN has limited entropy. A compromised OS/secure store can defeat the intended boundary.

## Reminder/platform integrity boundary

Medicine reminders and appointments are organizational, not clinical alarms.

Planner controls include explicit ownership/time-zone/UTC/date/state/DST rules.

Platform reconciliation controls include:

- effective snooze due time;
- cancellation before replacement/suppression/invalidation;
- retryable cancellation failure;
- stale ID retention until cancellation;
- medicine/profile delete compensation;
- appointment DB/platform compensation;
- cancellation-first handled actions;
- previous-state/rebuild recovery after later failures.

OS permission, shutdown, force-stop, battery policy and vendor restrictions remain outside deterministic planner guarantees.

## External export/funding boundary

Explicit exports, calendar actions, browser actions and project-support links cross outside CareNest.

CareNest does not claim control over data after external handoff.

The voluntary support URL must remain separate from health functionality and must not contain CareNest health identifiers.

`CareNestShowFundingLink=false` hides the complete in-app support card. The current 2026-08-15 policy review selects that store-safe configuration for initial Apple App Store and Google Play candidates unless submission-time current policy clearly permits the external link.

The source build decision does not itself prove that the final signed/installed package has the expected UI. Actual package inspection remains required.

## Release automation boundary

Marker-only PR verification validates candidate source through formatting, tests, platform builds, CodeQL and Dependency Audit.

Exact production tags matching `v*` run the exact tagged commit through:

- CareNest CI;
- CodeQL;
- Dependency Audit;
- CareNest Store Package Configuration;
- Release Gate;
- CareNest Release Evidence.

CareNest Store Package Configuration compiles Android, Windows, iOS simulator and Mac Catalyst with the external funding surface disabled. It intentionally does not configure production signing or publish unsigned artifacts.

Release Evidence records exact source/run identity and retains available evidence even when a component fails.

Automation does not replace manual device/accessibility/store/signing/package/data-compatibility approval.

## Out of scope for current v1 threat model

- CareNest-owned server compromise because there is no required CareNest backend;
- remote caregiver authorization/revocation because collaboration is not current v1 functionality;
- clinical correctness because CareNest is not medical decision support;
- security/privacy guarantees for external destinations after explicit user handoff;
- protection from a fully compromised equivalent-privilege environment.

## Security review triggers

A new security/privacy architecture review is mandatory before adding or materially changing:

- accounts/authentication;
- cloud synchronization;
- remote caregiver collaboration;
- analytics/crash state upload;
- document interpretation;
- medical decision support;
- embedded external web/payment/funding SDKs;
- biometric app-lock bypass/recovery;
- remote PIN/key recovery;
- automatic encrypted-data migration that drops historical compatibility;
- raw SQL/import execution paths;
- release tag/audit/evidence weakening;
- store-package configuration paths that could silently alter the selected external-funding visibility.

## Current remaining production security evidence

Before final public `1.0.0`:

- complete supported-platform manual testing;
- verify actual notification permission/delivery/recovery limitations;
- complete packaged SQLite existing-data compatibility;
- complete encrypted document/backup compatibility;
- complete accessibility/privacy presentation checks;
- re-review current Apple/Google store policy/disclosures at submission time;
- build and inspect actual signed/installed store-safe artifacts;
- secure signing credentials outside Git;
- inspect signed artifact provenance and checksums;
- pass exact production-tag CI, CodeQL, audit, Store Package Configuration, Release Gate and Release Evidence.

## Related documents

- `SECURITY.md`
- `docs/security/SECURITY_MODEL.md`
- `docs/security/LOGGING_PRIVACY.md`
- `docs/security/DEPENDENCY_RISK_REGISTER.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/architecture/DOCUMENT_VAULT.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/architecture/NOTIFICATIONS_AND_PLATFORM_BEHAVIOR.md`
- `docs/releases/SECURITY_RELEASE_REVIEW.md`
- `docs/releases/STORE_SAFE_CONFIGURATION_VERIFICATION_20260815.md`
- `docs/releases/STORE_POLICY_REVIEW_20260815.md`
- `docs/releases/STORE_BUILD_POLICY.md`