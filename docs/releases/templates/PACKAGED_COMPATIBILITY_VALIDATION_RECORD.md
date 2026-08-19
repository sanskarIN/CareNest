# CareNest Packaged Compatibility Validation Record

Use fictional/synthetic data. Never manufacture a current backup and label it historical evidence.

## Identity

- Result status: `NOT RUN`
- Originating CareNest version/build/source:
- Target CareNest version/build/source:
- Target source tag:
- Origin package filename/SHA-256:
- Target package filename/SHA-256:
- Target package-evidence JSON:
- Platform/device/OS:
- Validation date/time/time zone:
- Operator:

## Representative dataset

Record synthetic dataset composition before upgrade/restore:

- profiles:
- medicines:
- schedules:
- reminder occurrences:
- medication log entries:
- appointments:
- stock adjustments:
- documents/tags:
- settings:
- encrypted documents:

Do not include real health information or document contents.

## Existing-data / SQLite upgrade

- [ ] Origin package/data opens before upgrade.
- [ ] Origin schema version recorded.
- [ ] Target package installs/upgrades through the intended production path.
- [ ] Target database opens successfully.
- [ ] Target schema version recorded.
- [ ] SQLite integrity validation passes.
- [ ] Representative entity counts remain consistent with intended migrations.
- [ ] Representative records remain readable.
- [ ] Representative editable records remain editable.
- [ ] Reminder rebuild/reconciliation completes.
- [ ] No duplicate/stale platform request is observed in the tested boundary.

Evidence/notes:

## Encrypted document compatibility

- [ ] Existing encrypted documents open after upgrade.
- [ ] Import/open/export/delete lifecycle works on target package.
- [ ] Failed export cleanup leaves no unintended CareNest-owned plaintext.
- [ ] Missing/corrupt key fails closed where safely testable.

Evidence/notes:

## Current backup compatibility

- [ ] Target package creates encrypted backup.
- [ ] Backup inspection succeeds.
- [ ] Restore into an existing installation succeeds.
- [ ] Clean-install restore succeeds.
- [ ] Restored encrypted documents remain usable.
- [ ] Wrong password is rejected.
- [ ] Tampered backup is rejected.
- [ ] Truncated backup is rejected.
- [ ] Trailing-data backup is rejected.
- [ ] Representative normal backup remains comfortably below current resource ceilings.

Current default resource ceilings for the accepted source:

- decrypted ZIP container: 2304 MiB;
- manifest: 1 MiB;
- SQLite database: 1 GiB;
- each encrypted document entry: 512 MiB;
- total uncompressed ZIP payload: 2 GiB;
- document count: 5,000;
- archive-entry count: document limit plus fixed required entries;
- explicit directory-only ZIP entries: rejected.

Evidence/notes:

## Historical backup evidence

- Genuine prior backup bytes available: `NO/YES`
- Provenance of genuine prior bytes:
- Origin version/source if known:
- [ ] Genuine historical backup inspection performed.
- [ ] Genuine historical backup restore performed.
- [ ] Result recorded without weakening current limits silently.

If no genuine historical bytes exist, record `NOT RUN` or `N/A` with the reason. Do not create replacement bytes and call them historical evidence.

## Failures/blockers

List every `FAIL`, `BLOCKED` or `N/A` row with reason and issue/PR reference where applicable.

## Final result

- Overall result: `NOT RUN`
- Blocking issue references:
- Retest package/source if required:
- Reviewer/sign-off:
