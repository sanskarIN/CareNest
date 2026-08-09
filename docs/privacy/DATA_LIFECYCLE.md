# Data Lifecycle

1. **Collection:** only data the user enters/imports locally.
2. **Storage:** SQLite in app sandbox; encrypted document vault for imported files.
3. **Use:** local organization, reminders, exports and reports.
4. **Export:** explicit user action; decrypted copies may leave the CareNest sandbox.
5. **Backup:** explicit user action; encrypted archive.
6. **Deletion:** app deletes selected records and encrypted document files. SQLite free pages may persist until vacuum/OS cleanup; full reset removes application-owned storage.
7. **External copies:** CareNest cannot delete files already exported to another app/location.
