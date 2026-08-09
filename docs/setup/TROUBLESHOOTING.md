# Troubleshooting

## Notifications do not arrive

1. Open Settings → Notification diagnostics.
2. Confirm notification permission.
3. On Android, check battery optimization and exact-alarm capability.
4. Confirm the medicine schedule is active and its time zone is correct.
5. Use Test reminder.
6. Rebuild future reminders from developer options.

CareNest cannot guarantee delivery while a device is powered off, force-stopped, heavily restricted by battery policies, or prevented by operating-system policy.

## Build errors

- Verify .NET 10 SDK: `dotnet --info`.
- Verify workloads: `dotnet workload list`.
- Run `dotnet workload repair` if the MAUI workload is damaged.
- Delete `bin/` and `obj/` then restore.
- Confirm Xcode/Android SDK versions are compatible with the installed MAUI workload.

## Restore rejected

A restore is rejected if its format is unsupported, authentication fails, the password is wrong, or integrity validation fails. CareNest does not overwrite live data before validation.

## Document cannot open

Ensure the document still exists in encrypted app storage and there is sufficient temporary storage for an explicit export/open operation. CareNest treats imported files as opaque and does not medically interpret them.
