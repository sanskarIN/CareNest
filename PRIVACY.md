# CareNest Privacy Notice

CareNest `1.0.0-rc.1` is a local-first organizational application and does not require a CareNest account or CareNest-owned backend.

## Data stored locally

Depending on what the user enters, CareNest can store local profiles, emergency contacts, medicine names and user-entered instructions/strength text, schedules/reminder history, appointments/notes, stock adjustments, tags, settings and imported health documents.

Structured records use local SQLite application storage. CareNest does **not** claim transparent whole-database encryption.

Imported application-owned document payloads use a separate encrypted document-vault path.

## Network behavior

The core application does not require a network connection and does not automatically upload health data/documents to a CareNest cloud service.

Explicit actions such as opening repository/creator/legal/support web pages or using operating-system share/export/calendar features can involve external apps/services chosen by the user. CareNest does not automatically attach local health/profile/document/reminder/backup/app-lock data to ordinary fixed web links.

## Voluntary project support

The distributed CareNest application package currently does **not** include or expose the external Buy Me a Coffee project-funding destination/card/command/artwork.

Voluntary project support is available only through repository documentation/metadata:

`https://buymeacoffee.com/sanskarIN`

If a user separately visits that external service, its own privacy, cookies, account/payment processing and terms apply. Supporting the project does not change CareNest health functionality or give the project access to local CareNest data.

## Documents

Imported document bytes are encrypted within CareNest application-owned vault storage. Metadata remains in local structured storage.

Explicit export/open/share creates or hands off a decrypted/portable copy to the destination chosen by the user. That copy is then governed by the destination app/storage/provider and cannot be remotely revoked by CareNest.

## Database protection

SQLite records rely primarily on the app sandbox and device/OS security. Device encryption, screen lock, OS/device backups, malware, rooted/jailbroken state and operating-system compromise affect protection.

## Backups

Backups are manual, user-initiated and password-encrypted/authenticated. Users choose the destination and are responsible for protecting the password/file. CareNest has no server-side backup-password recovery service.

## App lock

Optional app lock is a local privacy barrier. PIN plaintext is not intended to be stored; derived verifier/salt material uses secure platform storage where applicable.

App lock is not whole-database/device encryption or protection against a fully compromised device.

## Notifications

Notification contents are privacy-minimized where practical, but the operating system controls final lock-screen/history display. Users should review OS notification-preview settings for their privacy needs.

CareNest cannot guarantee reminder delivery under every permission, shutdown, force-stop, battery/vendor or OS state.

## Diagnostics

Routine logs are designed not to contain document contents, lock PINs, backup passwords, encryption keys, full sensitive notes/medicine/profile names or raw health-record identifiers.

Sensitive-path logging should use safe operation/category and exception type instead of full raw exception messages/stack traces/paths where avoidable.

Users/maintainers should still review diagnostic exports before sharing them.

## Exports and external copies

Reports, documents, backups and calendar entries created/exported by explicit user action can leave CareNest-controlled storage. External providers/apps may retain or synchronize them according to their own policies.

## Deletion

Users can delete profiles/associated CareNest records with confirmation and can reset application-owned local data through documented workflows.

Deletion does not automatically remove copies already exported/shared, saved in external backups/calendars/cloud drives, captured in screenshots or retained by OS/device backup systems.

## No hidden analytics/telemetry

Current v1 does not include a hidden runtime analytics/telemetry client.

A future network/cloud/analytics feature would require updated consent, privacy, security, retention/deletion, threat-model and store-disclosure review before release.

## Medical boundary

CareNest organizes user-entered information. It does not diagnose conditions, calculate/infer dosage, recommend treatment, perform clinical medication-interaction checking, calculate clinical risk, verify adherence or provide emergency services.

## Contact

Privacy/business questions: `sanskarin@outlook.in`

Support: `supportramsandesh@gmail.com`

Detailed privacy architecture: `docs/privacy/PRIVACY_MODEL.md` and `docs/privacy/DATA_LIFECYCLE.md`.