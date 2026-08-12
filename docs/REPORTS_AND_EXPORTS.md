# CareNest Reports and Exports

CareNest v1 provides explicit user-controlled reports/exports for reviewing and moving local organizational data. These outputs are informational and must not be described as diagnosis, treatment advice, dosage calculation, clinical scoring, or verified adherence.

## Export principles

Every export/share operation should preserve these principles:

- explicit user action;
- no automatic background upload;
- clear destination boundary;
- privacy-safe handling;
- no clinical inference;
- stable machine-readable formatting where required;
- fictional/synthetic data in public examples/screenshots.

## Structured profile export

CareNest supports per-profile structured JSON export.

Purpose:

- user-controlled data portability/review;
- local troubleshooting with synthetic data;
- backup-adjacent portability without pretending JSON is the encrypted full backup format.

JSON export is not equivalent to the encrypted CareNest backup because the backup also protects portable recovery state/documents according to the backup format.

## PDF profile summary

CareNest can generate a PDF summary based on local profile records.

The report should include or preserve non-clinical/privacy limitation wording.

It must not produce:

- diagnosis;
- treatment plan;
- medicine-dose calculation;
- medication interaction conclusion;
- risk score;
- verified adherence statement.

## CSV upcoming schedule report

Purpose:

- provide machine-readable upcoming explicit schedule/reminder organization data.

The report represents CareNest schedule state, not guaranteed future OS notification delivery.

## CSV medication log

Purpose:

- export user-recorded Taken/Skipped/Delayed/Missed organizational history.

A Taken record is user-recorded and should not be represented as independently verified adherence.

## CSV missed reminder report

Purpose:

- export occurrences recorded as Missed under the CareNest reminder lifecycle.

The report should not infer health consequences from a missed record.

## CSV stock/refill report

Purpose:

- export the local stock estimate based on explicit user-entered values/adjustments.

The report must not claim the estimate equals actual physical supply. Users must verify actual stock.

## CSV appointment history

Purpose:

- export local appointment organizational history.

The output can contain sensitive personal data and should be treated accordingly.

## CSV document list

Purpose:

- export document metadata/listing without automatically embedding encrypted document payload bytes.

Document titles/metadata can still be sensitive.

## Calendar export

CareNest can explicitly export appointment information to a platform/third-party calendar.

After export:

- the destination may store/sync data independently;
- CareNest cannot enforce deletion/retention at the destination;
- the external calendar is outside the CareNest local-first boundary.

## Document export/share

Encrypted document export is an explicit privacy-boundary transition.

```text
CareNest encrypted payload
  -> user export/share
  -> decrypted/export copy
  -> platform destination
```

The resulting copy is not protected by the CareNest encrypted document vault unless the destination provides its own protection.

## Backup vs export

Use the correct artifact for the correct purpose.

### Encrypted backup

Designed for:

- protected recovery;
- restoring structured records;
- restoring encrypted-document access;
- clean-install recovery.

### JSON/CSV/PDF/document export

Designed for:

- user-readable/machine-readable portability;
- sharing/review;
- individual data categories.

Exports are not automatically encrypted just because CareNest stores the source data locally.

## Privacy warning

JSON/CSV/PDF/decrypted document copies can contain sensitive plaintext data.

Once exported they can be:

- copied;
- emailed;
- uploaded;
- backed up by another service;
- retained after local deletion.

CareNest cannot remotely recall them.

## File naming

Export filenames should avoid unnecessary sensitive information.

Do not include private notes, PINs, backup passwords, cryptographic keys, or raw internal database paths in filenames.

## Machine-readable formatting

Where CSV/JSON fields are designed for machine interchange:

- use stable/invariant date/number representations as required;
- do not let UI localization mutate stored UTC/time-zone identity;
- document any schema/header changes;
- add regression tests when the contract changes.

## Localization

Visible report labels can be localized in future versions, but machine-readable fields must remain deliberately stable where required.

Safety/privacy disclaimers must retain meaning across translations.

See `docs/design/LOCALIZATION.md`.

## Report disclaimer contract

Reports should communicate that:

- data is user-entered/local;
- CareNest is organizational;
- no diagnosis/treatment/dosage recommendation is being made;
- reminder/log records are not independent clinical verification;
- users should verify medical information with qualified professionals.

## Error handling

If export generation fails:

- show actionable privacy-safe UI text;
- do not display raw stack traces/database paths;
- do not log sensitive record content;
- clean partial temporary output where appropriate.

## Cancellation

Long-running export/file operations should honor cancellation where the service contract supports it and avoid leaving misleading partially completed output.

## Sharing

Use platform share/file APIs only after explicit user action.

CareNest should not silently choose a remote destination.

## Store/privacy disclosures

Store listings/forms must accurately describe explicit export/share capabilities without claiming that all exported data stays local after the user sends it elsewhere.

## Testing

Automated tests should verify:

- expected report generation;
- required disclaimer text;
- stable formatting/contracts;
- no clinical conclusions;
- correct data selection.

Manual target tests should verify:

- file picker/save/share behavior;
- destination permissions;
- readable output;
- export after clean install/restore;
- no sensitive data in logs;
- cancellation/low-storage failure behavior where practical.

## Security/support rule

Do not ask users to upload real reports/exports to public GitHub issues unless they have safely removed all sensitive information.

Prefer synthetic reproductions.

## Related documentation

- `docs/USER_GUIDE.md`
- `docs/FEATURE_REFERENCE.md`
- `docs/architecture/DATA_STORAGE_AND_EXPORT.md`
- `docs/architecture/DOCUMENT_VAULT.md`
- `docs/privacy/PRIVACY_MODEL.md`
- `docs/testing/TESTING_GUIDE.md`
- `docs/releases/MANUAL_TEST_MATRIX.md`