# CareNest User FAQ

This FAQ answers common product, privacy, reminder, document, backup, Gumroad/storefront and release-status questions. For step-by-step workflows use `docs/USER_GUIDE.md`.

## What is CareNest?

CareNest is a local-first organizer for user-entered family/person profiles, medicines, schedules, reminder history, appointments, health documents, stock/refill notes, reports and manual backups.

## Is CareNest a medical or diagnostic app?

No. CareNest is organizational software. It does not diagnose conditions, calculate or infer dosage, recommend treatment, perform clinical medication-interaction checking, calculate clinical risk, replace a clinician/pharmacist, or provide emergency services.

## Does CareNest decide when or how much medicine I should take?

No. Medicine strength/instruction text is stored as user-entered text. Reminder schedules come from explicit user-entered schedule values. CareNest does not infer dosage or a medical schedule from a medicine name, strength, diagnosis or document.

## What is the official Ram Sandesh Gumroad store?

The official storefront is:

**https://ramsandesh.gumroad.com**

It may contain separate digital products, books, learning resources, templates, project material or documentation bundles.

See `GUMROAD.md` for the canonical storefront guide.

## Is Gumroad part of the CareNest health app?

No. Under the current release/store policy, Gumroad is a repository/documentation/marketing surface, not an in-app health feature.

The CareNest application package intentionally excludes the Gumroad destination and repository Gumroad promotional badge.

## Does buying something on Gumroad unlock CareNest health features?

No. A Gumroad purchase does not unlock or change:

- diagnosis;
- dosage guidance;
- treatment recommendations;
- clinical medication-interaction checking;
- reminder priority or delivery guarantees;
- emergency assistance;
- CareNest account/cloud behavior;
- access to user health data.

## Does CareNest send my health records to Gumroad?

No automatic transmission of CareNest local health records to Gumroad is part of the current product design.

If you independently visit or purchase from an external site, that third party has its own terms/privacy practices. Do not place private health information in purchase notes, payment notes or third-party messages.

## Does CareNest require an account?

Current v1 does not require a CareNest account or CareNest-owned backend.

## Does CareNest automatically upload my records to a CareNest cloud service?

No automatic CareNest cloud synchronization/upload is part of the current v1 scope.

Exports, shares, calendar operations, browser links, device backups or user-selected external destinations can still move copies outside CareNest's local application boundary.

## Is the whole SQLite database encrypted?

CareNest does not claim transparent whole-database encryption. Structured SQLite data relies primarily on the application sandbox and device/OS security.

Imported document payloads are separately encrypted, and manual backups are password-encrypted.

## Are imported health documents encrypted?

CareNest's application-owned document-vault payloads are encrypted with authenticated encryption. Metadata and structured database records have different storage protections; see `docs/architecture/DOCUMENT_VAULT.md` and `docs/security/SECURITY_MODEL.md`.

## What happens if I export or share a document/report?

The exported/shared copy leaves the CareNest-controlled encrypted/local boundary. The destination application, cloud provider, filesystem, screenshot, email service or OS may retain it. CareNest cannot reliably revoke a copy after control has been handed to another system.

## Are backups encrypted?

Manual CareNest backups are password-protected and authenticated. Keep the backup file and password secure. Losing the password can make the backup unrecoverable.

## Can CareNest recover a forgotten backup password?

The project does not provide a server-side password recovery service for locally encrypted backups.

## What happens if a backup is tampered with or truncated?

The backup format is designed to reject wrong-password, tampered, truncated or malformed payloads rather than silently restoring corrupted content.

## Does CareNest guarantee reminder delivery?

No. CareNest can plan, persist, schedule and reconcile reminder requests, but operating systems control notification/alarm delivery. Permissions, exact-alarm capability, battery restrictions, force-stop behavior, vendor policies, reboot/time changes and platform lifecycle can affect delivery.

## Why can a reminder appear late or not at all?

Common external causes include notification permission denial, Android alarm/battery restrictions, force-stop/background limitations, OS scheduling policy, device power state, clock/time-zone changes, or platform lifecycle behavior.

CareNest also performs internal consistency checks so stale platform requests can be reconciled after edits/restarts.

## What does Snooze mean in CareNest?

A valid snoozed reminder uses the explicit future `SnoozedUntilUtc` time as its effective due time. The original scheduled time is not used to incorrectly classify the snoozed occurrence as overdue while the snooze due time is still in the future.

## What are Taken, Skipped, Delayed and Missed states?

They are organizational history states selected/recorded through CareNest reminder workflows. They are not clinical verification that a medicine was actually ingested or medically appropriate.

## Can CareNest detect medication interactions?

No clinical medication-interaction engine is part of the documented v1 scope.

## Can CareNest automatically refill medicines?

CareNest can organize user-entered stock/refill information. It does not place pharmacy orders or infer medically appropriate quantities.

## Can I manage multiple people?

Yes. CareNest supports multiple local profiles so a household can organize information separately.

## Does CareNest support appointments?

Yes. Appointments can be organized and can use optional reminders based on the explicit stored appointment time and configured lead time.

## Can I export reports?

CareNest supports user-controlled reports/exports. See `docs/REPORTS_AND_EXPORTS.md` for current formats, data boundaries and privacy considerations.

## Does CareNest have an app lock?

Yes, an optional local app lock is part of the current scope. It is a privacy barrier and is not equivalent to whole-database encryption or protection against a fully compromised device.

## What happens if I delete local data?

CareNest can clear application-owned local data according to its documented cleanup lifecycle. Copies already exported, shared, backed up by the OS, captured in screenshots or stored by another application may remain outside CareNest's control.

## Does CareNest collect analytics?

The current local-first v1 boundary does not include a hidden runtime analytics/telemetry client.

## What platforms does the project target?

Current source targets Android, iOS/iPadOS, Mac Catalyst and Windows through .NET MAUI.

Automated builds currently verify Android, Windows, iOS simulator and Mac Catalyst. Real-device/manual production validation remains a separate release gate.

## Is CareNest already published in production stores?

The repository status is `1.0.0-rc.1`. The source is heavily automated-verified, but production signing, final signed-package validation, real-device/accessibility evidence, store metadata/policy review, tagged release evidence and publication are still separate gates.

## What is the latest fully verified automated baseline?

The latest fully verified pre-Gumroad exact source is:

`7cbe5568b6cffa06c279b29f3cb1b107ea988791`

It passed:

- 122 unit tests;
- 39 integration tests;
- 173 UI/source-policy tests;
- **334/334 total core tests**;
- Android, Windows, iOS simulator and Mac Catalyst Release builds;
- all four store-candidate configurations;
- CodeQL.

The Gumroad rollout adds verification-relevant source-policy tests and package-scanner changes. A newer baseline is only authoritative after the exact final Gumroad revision completes its own workflow matrix. See `PROJECT_STATUS.md` and `what_changed.md`.

## What does “334/334 tests passed” mean?

It means 122 unit tests, 39 integration tests and 173 UI/source-policy tests passed for the exact source `7cbe5568b6cffa06c279b29f3cb1b107ea988791`. It does not prove that no undiscovered defect can exist or that manual production testing is complete.

## Is Buy Me a Coffee inside the app?

The current application package intentionally does not include an in-app external Buy Me a Coffee destination/card/command/artwork. Voluntary project-support information can exist in repository documentation separately from health functionality.

## Is Gumroad inside the app?

No. The current application package intentionally does not include the Gumroad destination/card/command/repository promotional artwork. Gumroad is highlighted in the repository and documentation instead.

## Do Gumroad purchases or funding unlock health features or better reminders?

No. Gumroad purchases and voluntary project funding do not unlock medical advice, health functionality, reminder reliability/priority, emergency assistance, health-data access or clinical services.

## Where can I report a bug?

Use the repository issue templates when appropriate and follow `SECURITY.md` for security-sensitive reports. Do not include real health records, passwords, PINs, keys, tokens or private backups in public reports.

## Where can I get support?

See `SUPPORT.md` and `docs/SUPPORT_CARENEST.md`.

## Where can I find the Gumroad link?

Use the canonical URL:

**https://ramsandesh.gumroad.com**

It is also highlighted in `README.md`, `SUPPORT.md`, `GUMROAD.md`, `docs/README.md` and other current repository support/marketing documentation.

## Where is the complete documentation?

Start with `docs/DOCUMENTATION_CATALOG.md` and `docs/COMPLETE_PROJECT_DOCUMENTATION.md`.
