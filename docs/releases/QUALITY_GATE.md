# CareNest Production Quality Gate

CareNest must not be described as bug-free. A production release is acceptable only when the preventive controls and evidence below are complete for the exact release commit.

## Source quality

- Runtime source contains no TODO/FIXME/NotImplemented implementation placeholders.
- Nullable reference types and analyzers remain enabled.
- CI warnings-as-errors policy remains enabled for CI builds except explicitly documented advisory analyzer exceptions.
- Platform-neutral projects pass `dotnet format --verify-no-changes`.
- Shared/Domain/Application/Infrastructure project dependency direction passes architecture contract tests.
- Concrete ViewModels do not directly access SQLite infrastructure or create network clients.
- Runtime source does not synchronously block on tasks through `.Wait()`, `.Result`, `GetAwaiter().GetResult()`, `Thread.Sleep`, `Task.WaitAll` or `Task.WaitAny` patterns.
- New service behavior has direct platform-neutral tests where orchestration can be verified without MAUI/SQLite.
- Analyzer findings exposed by marker-only verification are fixed in source/tests rather than hidden by weakening the analyzer gate.

## Product safety and scheduling integrity

- No diagnosis, treatment recommendation, dosage calculation/inference, medication-interaction checking or clinical risk scoring is introduced.
- Medicine strength and instruction values remain opaque user-entered text.
- As-needed schedules do not automatically create reminders.
- Archived profiles, paused/completed/archived medicines and disabled schedules do not automatically materialize reminders.
- Daily, selected-weekday, cycle, custom-range and every-N-hours behavior is derived only from explicit user-entered schedule values.
- Planning windows remain half-open (`fromUtc` inclusive, `toUtc` exclusive) so adjacent rebuild windows do not duplicate boundary occurrences.
- Planner window start/end and reminder rebuild overrides require actual UTC `DateTime` values.
- Duplicate explicit clock times do not create duplicate occurrence identities.
- Reminder ownership is verified across profile → medicine → schedule → persisted schedule-time relationships before materialization.
- Unknown schedule kinds, unsupported weekday-mask bits, invalid explicit intervals and invalid time-zone identifiers are rejected rather than silently reinterpreted.
- Invalid daylight-saving spring-forward local times do not cause CareNest to invent an alternate reminder time.
- Ambiguous daylight-saving fall-back local times remain deterministic across rebuilds.
- Representative DST gap/overlap coverage spans North America, Europe, Australia and New Zealand when those identifiers exist on the test host.
- Deterministic property-style recurrence tests use fixed seeds/explicit synthetic schedules and remain reproducible.
- Snooze actions require an explicit future UTC timestamp before persistence or platform scheduling.
- Snoozed rows use `SnoozedUntilUtc` as their effective due time for upcoming and overdue behavior.
- Rebuild cancels existing OS requests before replacement, suppression or invalidation.
- Platform cancellation failure leaves reminder state retryable rather than falsely reconciled.
- Schedule edits preserve old future occurrence identities until OS-request reconciliation can cancel stale requests.
- Medicine/profile delete flows cancel future OS requests before cascade and compensate with non-cancelled rebuild if persistence fails.
- Medicine/profile save flows reconcile reminders before non-critical audit bookkeeping can make an already-applied record transition appear failed.
- Appointment persistence has explicit platform-reminder reconciliation/compensation coverage.
- Reminder actions cancel the old OS request before committing handled state and use non-cancelled state/rebuild compensation when later persistence/snooze scheduling fails.
- Appointment `StartsUtc` requires actual `DateTimeKind.Utc`; local/unspecified appointment clock values are rejected instead of relabeled.
- Appointment time-zone identifiers are trimmed/validated separately from the UTC instant.
- Notification permission is not requested during onboarding; it is requested only at an explicit reminder-capable action.
- An appointment save whose permission request remains denied creates no platform notification schedule.
- Appointment/background rebuild does not repeatedly prompt and does not schedule while permission remains denied.
- Stock changes use only user-configured values.
- Medical/reminder limitations remain visible in onboarding and About.

## Document-vault consistency/security

- New document payloads use AES-256-GCM chunked framing v2 with an authenticated terminal record.
- Legacy framing-v1 encrypted documents remain readable for compatibility; existing v1 ciphertext is not represented as retroactively upgraded.
- V2 integration tests reject chunk-boundary prefix truncation and trailing data.
- New `CareDocument.EncryptionVersion` metadata records stream format v2.
- Caller-owned copies of the document master key are zeroed after crypto operations where practical.
- Generated document-key buffers are cleared if secure-store persistence fails.
- A document metadata-save failure removes the newly created encrypted payload.
- An audit failure after metadata save attempts rollback of both metadata record and encrypted payload.
- Rollback cleanup failure is surfaced rather than silently hidden.
- Explicit export constrains the temporary output filename to a safe leaf filename.
- Successful decrypted exports use managed cache ownership.
- Failed/cancelled plaintext exports remove application-owned incomplete files best effort.
- Application-owned report cache files are removed after share handoff where CareNest still controls the temporary copy.

## Backup/restore security

- Backup encryption tests pass for round-trip, wrong-password and tamper rejection.
- New encrypted backup payload streams use authenticated framing v2; legacy framing v1 remains readable for compatibility.
- V2 authenticated terminal/trailing-data behavior is tested through the shared framing tests.
- Decrypted backup ZIP topology is validated before extraction.
- Duplicate archive entries are rejected.
- Unexpected archive entries are rejected.
- Nested document entries are rejected.
- Non-`.cndoc` document entries are rejected.
- Manifest document count must match the archive.
- A document-bearing backup requires a valid 32-byte document master key.
- Invalid schema/document-count metadata is rejected.
- Extraction retains full-path containment checks as defense in depth.
- Password-derived key/salt buffers are zeroed after backup crypto paths where practical.
- Copied document-key buffers used during backup creation/restore are zeroed after use where practical.
- WAL snapshot tests verify copied committed data and SQLite integrity rather than only file existence.
- A pre-cancelled snapshot operation leaves no output file.
- Primary encrypted backup/restore success is not falsely reported as failure only because later local bookkeeping/audit persistence fails.
- Failed restore rolls the document key back to its exact prior byte state where prior bytes existed.

## Privacy/security

- No required CareNest account/server/network client is introduced in v1.
- No analytics/telemetry client is introduced.
- No common signing/credential files are committed.
- Error/reminder logging does not pass full exception objects or health-record identifiers to the structured logger.
- Planner ownership mismatches fail closed instead of silently creating occurrences under another local entity.
- App-lock source contracts verify salted PBKDF2-HMAC-SHA256, fixed-time comparison, no plaintext PIN persistence, verifier-buffer clearing and stored lock-material removal.
- App lock remains described as a local privacy barrier, not whole-database/device encryption.
- Sensitive mutable caller-owned buffers are cleared where practical without claiming total process/OS-memory erasure.
- SQLite migration/integrity tests pass.
- CodeQL passes.
- Dependency Audit passes without the former `GHSA-2m69-gcr7-jv3q` `NuGetAuditSuppress` exception.
- SQLite native/provider package floors are guarded by `SqliteDependencySecurityContractTests`.
- The dependency remediation is not represented as proof of packaged existing-database/backup/device compatibility; those manual release checks remain separate.

## Cross-platform automated evidence

- Unit tests pass.
- Integration tests pass.
- UI/repository contract tests pass.
- Android Release build passes.
- Windows Release build passes.
- iOS simulator Release build passes.
- Mac Catalyst Release build passes.
- CodeQL passes.
- Unsuppressed Dependency Audit passes.
- Release Evidence artifacts are generated for the exact final release commit before production publication.

## Current exact automated baseline

The older PR #33 baseline is historical. The 2026-08-14 bug audit continued through reminder reconciliation/failure recovery, appointment persistence compensation, report-cache cleanup, SQLite dependency remediation, and cancellation-first reminder action recovery.

Authoritative marker-only verification: PR #54 — `Verify final CareNest bug-audit source`.

Verified runtime/test/dependency source/base SHA:

`4490f3f86752841d436e981b29279970c90c947b`

Verification marker head:

`929168a0a319b15d9e89997d86436d59ae731ad1`

Final evidence:

- CareNest CI #503 / `31766059137`: **success**;
- platform-neutral formatting: **success**;
- **122 unit tests**: passed;
- **39 integration tests**: passed;
- **100 UI-contract/policy tests**: passed;
- **261 total core tests**: passed;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- CodeQL #503 / `31766059215`: **success**;
- unsuppressed Dependency Audit #35 / `31766059132`: **success**, including platform-neutral and Android MAUI app dependency graphs.

PR #54 was closed without merge. Its verification marker is not part of `main`. Documentation-only commits after the source boundary do not alter the runtime/test/dependency graph exercised by PR #54.

`docs/releases/FINAL_BUG_AUDIT_VERIFICATION_20260814.md` records the final evidence in one place.

### Verification failures retained as evidence of gate behavior

- PR #31 exposed CA1861 in new profile-service test source; it was fixed without analyzer suppression.
- PR #39 exposed CA1001 and a formatter defect; the accidentally merged failed marker was explicitly removed from `main`.
- PR #40 demonstrated platform/CodeQL/audit success but core formatting failed, so it was not promoted.
- PR #43 was incorrectly described as green in earlier documentation; actual CareNest CI #448 failed integration tests and skipped the UI suite.
- PR #44 reproduced future-snooze, overdue-snooze and stale-occurrence defects; source was fixed instead of reusing PR #43 evidence.
- PR #46 exposed broader OS-reminder reconciliation contracts.
- PR #49 exposed CA1861 in new reminder reconciliation assertions; tests were corrected instead of suppressing the analyzer.
- PRs #47/#48/#50 supplied useful unsuppressed SQLite-audit evidence while source was moving, but were not final combined-source baselines.
- PRs #51/#52 were superseded when later runtime/test source changed.
- PR #53 independently corroborated the final source but was closed as duplicate verification after PR #54 completed the full matrix.

This automated baseline is necessary but not sufficient for final public release. The exact promoted production commit still needs Release Evidence after all manual/store/signing/existing-data compatibility blockers are complete.

## Manual evidence

- Android device/emulator matrix complete.
- Windows manual matrix complete.
- iOS/iPadOS manual matrix complete.
- Mac Catalyst manual matrix complete.
- Notification permission and delivery limitations tested.
- Appointment permission-denied/granted behavior tested on target platforms.
- Android exact-alarm/battery/reboot behavior tested on representative devices.
- Time-zone change behavior tested.
- Snooze and cancellation-first reminder-action behavior tested against real platform notification scheduling.
- Representative upgrade/install containing fictional pre-remediation SQLite data tested.
- Existing structured records remain readable after the SQLite native/provider update.
- Existing encrypted documents remain decryptable after the dependency update.
- Pre-remediation/current encrypted backup compatibility tested on packaged builds where canonical fixtures are available.
- Document import/export/delete tested.
- New v2 encrypted document read/write tested in packaged builds.
- Legacy v1 document compatibility tested with a canonical historical fixture when available.
- Calendar export tested.
- Encrypted backup/restore tested on clean installation/release build.
- New v2 encrypted backup creation/readback tested in packaged builds.
- Legacy v1 backup compatibility tested with a canonical historical fixture when available.
- App lock cold-start flow tested.
- Screen-reader, keyboard, large-text, reduced-motion and contrast checks complete.

## Distribution evidence

- Current Apple/Google policy review for the voluntary project-support link is complete.
- Channel-specific support-link visibility follows current store rules.
- Signing identities are supplied outside Git.
- Signed packages are built from the exact verified commit.
- Store listing/privacy/data-safety claims match actual implementation.
- Release notes include known limitations and do not promise guaranteed reminder delivery.
- Final package/release notes distinguish v2 new-write hardening from retained v1 read compatibility if that distinction is relevant to users/support.

Any failed, unknown or stale required gate blocks final production promotion until it is resolved or explicitly documented as not applicable by the release owner.
