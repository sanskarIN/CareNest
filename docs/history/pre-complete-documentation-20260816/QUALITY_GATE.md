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

## Release-engineering quality controls

- CareNest CI, CodeQL and Dependency Audit support exact `v*` tag execution.
- CareNest CI, CodeQL and Dependency Audit expose manual execution paths where configured.
- Dependency Audit does not dereference pull-request-only metadata during tag/manual runs.
- Release Gate and Release Evidence support exact `v*` tags and manual execution.
- Release Gate detects open dependency-risk status independent of indentation/case and detects nested unchecked checklist rows.
- Release Gate requires the core release/status/security/evidence documents and all three core test suites.
- Release Evidence records exact commit/ref/run/attempt identity.
- Release Evidence records tracked-file manifests and SHA-256 source checksums.
- Release Evidence attempts unit, integration, UI-contract, dependency-inventory, and workspace-integrity evidence independently.
- Release Evidence uploads available evidence before the aggregate failure gate so a failed run remains diagnosable.
- Release Evidence artifact names include commit SHA, run ID, and run attempt.
- Release-preflight Bash/PowerShell scripts treat unsuppressed dependency audit failures as blocking.
- Quality-gate Bash/PowerShell scripts work from a clean checkout and fail on required native-command errors.
- Repository Git setup scripts use repository-local identity, verify `Sanskar` / `sanskarin@outlook.in`, and fail closed on native Git errors.
- Executable UI-contract tests guard these workflow/script/release-policy expectations.

## Cross-platform automated evidence

- Unit tests pass.
- Integration tests pass.
- UI/repository/release-policy contract tests pass.
- Android Release build passes.
- Windows Release build passes.
- iOS simulator Release build passes.
- Mac Catalyst Release build passes.
- CodeQL passes.
- Unsuppressed Dependency Audit passes.
- Release Evidence artifacts are generated for the exact final production release commit/tag before publication.

## Current exact automated baseline

Authoritative marker-only verification: PR #56 — `Verify complete CareNest release-engineering source`.

Frozen source/base SHA:

`4f1a0a14abb8f3405a2387317a89e8a2988a3eaa`

Verification marker head:

`e3bc621cea05364a69abee0dadbd71a67c17bddb`

Final evidence:

- CareNest CI #571 / `31770929379`: **success**;
- platform-neutral formatting: **success**;
- **122 unit tests**: passed;
- **39 integration tests**: passed;
- **124 UI-contract/policy tests**: passed;
- **285 total core tests**: passed;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**;
- CodeQL #571 / `31770929382`: **success**;
- unsuppressed Dependency Audit #41 / `31770929383`: **success**.

PR #56 was closed without merge. Its verification marker is not part of `main`.

`docs/releases/RELEASE_ENGINEERING_VERIFICATION_20260814.md` records the authoritative final release-engineering evidence. PR #54 remains the historical completed runtime bug-audit baseline.

### Verification failures/supersession retained as evidence of gate behavior

- PR #31 exposed CA1861 in new profile-service test source; it was fixed without analyzer suppression.
- PR #39 exposed CA1001 and a formatter defect; the accidentally merged failed marker was explicitly removed from `main`.
- PR #40 demonstrated platform/CodeQL/audit success but core formatting failed, so it was not promoted.
- PR #43 was incorrectly described as green in earlier documentation; actual CareNest CI #448 failed integration tests and skipped the UI suite.
- PR #44 reproduced future-snooze, overdue-snooze and stale-occurrence defects; source was fixed instead of reusing PR #43 evidence.
- PR #46 exposed broader OS-reminder reconciliation contracts.
- PR #49 exposed CA1861 in new reminder reconciliation assertions; tests were corrected instead of suppressing the analyzer.
- PRs #47/#48/#50 supplied useful unsuppressed SQLite-audit evidence while source was moving, but were not final combined-source baselines.
- PRs #51/#52 were superseded when later runtime/test source changed.
- PR #53 independently corroborated the final bug-audit source; PR #54 was retained as that audit's authoritative checkpoint.
- PR #55 passed 277/277 core tests, Android, Windows, CodeQL and unsuppressed Dependency Audit but was intentionally superseded when the complete-file audit found further legitimate release-tooling/documentation fixes.
- PR #56 is the authoritative completed release-engineering baseline.

This automated baseline is necessary but not sufficient for final public release. The exact promoted production commit/tag still needs successful exact-tag Release Gate/Release Evidence after all applicable manual/store/signing/packaged-data blockers are complete.

## Manual evidence still required

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

## Distribution evidence still required

- Current Apple/Google policy review for the voluntary project-support link is complete.
- Channel-specific support-link visibility follows current store rules.
- Signing identities are supplied outside Git.
- Signed packages are built from the exact verified production commit.
- Store listing/privacy/data-safety claims match actual implementation.
- Release notes include known limitations and do not promise guaranteed reminder delivery.
- Final package/release notes distinguish v2 new-write hardening from retained v1 read compatibility if that distinction is relevant to users/support.
- Exact approved production `v*` tag completes CareNest CI, CodeQL, Dependency Audit, Release Gate, and Release Evidence successfully.

Any failed, unknown or stale required gate blocks final production promotion until it is resolved or explicitly documented as not applicable by the release owner.
