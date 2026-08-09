# Data Lifecycle

1. **Collection:** only data the user enters/imports locally.
2. **Storage:** SQLite in app sandbox; encrypted document vault for imported files.
3. **Use:** local organization, reminders, exports and reports.
4. **External links:** repository, policy, creator and voluntary project-support links open only after explicit user action. CareNest does not attach or transmit local profiles, medicine data, documents, backups, reminder history or app-lock data to those links.
5. **Export:** explicit user action; decrypted copies may leave the CareNest sandbox.
6. **Backup:** explicit user action; encrypted archive.
7. **Deletion:** app deletes selected records and encrypted document files. SQLite free pages may persist until vacuum/OS cleanup; full reset removes application-owned storage.
8. **External copies/services:** CareNest cannot delete files already exported to another app/location and does not control the privacy, cookies, accounts, payment processing or retention practices of independently opened external services such as `https://buymeacoffee.com/sanskarIN`.
