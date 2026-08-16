# CareNest Data Storage, Export, and Deletion Model

CareNest v1 is local-first. This document explains where major data categories live, how they leave the application boundary, and what maintainers/users must understand about deletion and portability.

## Trust boundary

The primary CareNest trust boundary is the installed application plus the operating-system application sandbox and platform secure-storage facilities.

No CareNest backend/account is required in v1.

## Structured records

Structured records are stored in a local SQLite database.

Examples include:

- profiles;
- emergency contacts;
- medicines;
- medicine schedules;
- schedule times;
- reminder occurrences;
- medication-log entries;
- appointments;
- document metadata;
- tags/document-tag relationships;
- stock adjustments;
- settings;
- backup metadata;
- audit entries;
- schema information.

The application does **not** claim transparent whole-database encryption at rest. The database relies primarily on the application sandbox/device protections.

## Sensitive imported documents

Imported health documents are stored through the encrypted document-vault path.

The document payload is encrypted independently from the SQLite metadata record. The per-installation document key is stored through platform secure secret storage.

Database metadata can identify/organize a document without storing the original document bytes directly in the SQLite row.

## App-lock secrets

App-lock material is kept through platform secure secret storage.

CareNest stores a salt/verifier/enabled state rather than a plaintext PIN.

App lock is a local privacy barrier and is not a claim that every SQLite field is encrypted.

## Backup files

Manual CareNest backups are user-created portable files protected using password-derived authenticated encryption.

A backup may contain enough protected recovery material to recreate structured local records and restore encrypted document access on a clean installation.

After a backup is written to a user-selected location, that location becomes an external storage/privacy boundary.

## Notification registrations

Platform notification systems receive the minimum request data needed for supported reminder delivery.

Notification labels are generic by default. Document contents and sensitive free-text health details are not intended to be embedded in routine notification payloads.

The OS controls final notification delivery/display behavior.

## Logs and diagnostics

Routine CareNest diagnostic logging is privacy-redacted.

Do not log:

- health-document contents;
- backup passwords;
- plaintext app-lock PINs;
- encryption keys;
- private free-text health notes;
- full exception messages/stack traces from user-data operation paths;
- health-record identifiers in reminder scheduling failure messages where avoidable.

See `docs/security/LOGGING_PRIVACY.md`.

## Profile export

CareNest supports explicit per-profile structured JSON export.

An export is a user-controlled copy of local data.

Once written/shared:

- it is no longer protected solely by the CareNest application sandbox;
- destination application/service/storage policies apply;
- the user should inspect the destination before sharing sensitive information.

## CSV exports

Supported report/export categories include organizational CSV outputs such as:

- upcoming schedule;
- medication log;
- missed reminders;
- stock/refill;
- appointment history;
- document list.

CSV files may be readable by many applications and therefore should be treated as potentially sensitive plaintext exports.

## PDF reports

CareNest can generate informational PDF summaries.

Reports carry non-clinical/privacy limitations and must not present diagnosis, treatment recommendation, dosage inference, or clinical risk scoring.

A PDF saved/shared outside CareNest is governed by its destination.

## Document export/share

Document export is explicit.

Typical boundary transition:

```text
Encrypted CareNest document
  -> explicit user export/share
  -> decrypted/export copy
  -> platform share/file destination
```

The exported copy is no longer protected by the CareNest encrypted document-vault key unless the destination applies its own protection.

## Calendar export

Appointment calendar export is explicit user action.

After export, calendar data may be stored/synchronized by the target calendar application/provider. That behavior is outside the CareNest local-first storage boundary.

## External project-support link

The fixed project-support destination is:

`https://buymeacoffee.com/sanskarIN`

CareNest does not intentionally append profile, medicine, reminder, document, backup, or app-lock data to that URL.

After the user opens the destination, the browser/external provider is a separate trust/privacy boundary.

## Profile deletion

Profile deletion is destructive and should remove/clean related local records according to repository relationship/cascade behavior.

Because the application may also have encrypted document files associated with records, destructive-flow testing must verify both database relationship cleanup and expected file cleanup behavior.

Users who want a copy should export or create an encrypted backup before deletion.

## Document deletion

Deleting a document should remove the intended local document record and protected document file according to the application workflow.

Manual release testing must verify there are no unintended orphaned plaintext exports/caches created by normal operations.

Previously exported copies outside CareNest cannot be remotely recalled by deleting the CareNest record.

## App reset

A local reset/destructive clear operation affects the installed application's local state. It cannot erase copies the user previously exported to other apps/services/locations.

## Backup deletion

Deleting CareNest local records does not automatically delete manually created backup files stored elsewhere.

Users must manage backup retention/deletion at the destination where those backups were saved.

## OS-level backups and device copies

Operating systems or device-management tools may independently back up application data depending on platform/user/device policy.

CareNest documentation should not claim that deleting a local record necessarily erases every historical copy that an OS or external backup system may have created.

## Screenshots and notification previews

CareNest cannot guarantee control over screenshots, screen recording, accessibility services, or OS notification-preview behavior on a compromised/misconfigured device.

Use generic notification labels where privacy is important and configure device-level notification privacy appropriately.

## Data lifecycle summary

Data can move through these stages:

```text
User input/import
  -> local structured/encrypted storage
  -> local editing/reminder/report processing
  -> optional explicit export/share/backup/calendar/support action
  -> external destination boundary
  -> local deletion/archive/reset where requested
```

CareNest v1 does not add an automatic CareNest cloud-upload stage.

## Developer requirements

Any new feature that handles user data must answer before merge:

- What data is created?
- Where is it stored?
- Is it encrypted or sandbox-protected only?
- Can it appear in logs/notifications?
- How is it exported?
- How is it deleted?
- Does it cross the local-device boundary?
- Does it require new permissions?
- Does it alter backup/restore compatibility?
- Does it alter privacy/store disclosures?

Features that add remote transfer/synchronization require a new architecture and threat/privacy review.

## Related documents

- `PRIVACY.md`
- `docs/privacy/DATA_LIFECYCLE.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/security/THREAT_MODEL.md`
- `docs/security/LOGGING_PRIVACY.md`
- `docs/architecture/BACKUP_AND_RESTORE.md`
- `docs/architecture/DATABASE_SCHEMA.md`
- `docs/releases/MANUAL_TEST_MATRIX.md`