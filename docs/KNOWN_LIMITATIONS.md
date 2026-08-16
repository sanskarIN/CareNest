# CareNest Known Limitations and Release-Candidate Boundaries

This document records limitations that are intentional, externally constrained, or not yet proven by production evidence. A limitation listed here is not automatically a software defect.

## 1. Medical/clinical limitations

CareNest is an organizational tool and does not:

- diagnose conditions;
- calculate or infer medicine dosage;
- recommend starting, stopping, changing or combining treatment;
- perform clinical medication-interaction checking;
- calculate clinical risk scores;
- interpret documents as clinical advice;
- independently verify that medication was taken;
- replace a clinician, pharmacist or emergency service.

These are product boundaries, not pending features for `1.0.0-rc.1`.

## 2. Notification delivery is not guaranteed

CareNest can deterministically plan reminders and can persist/reconcile operating-system requests, but the operating system ultimately controls delivery.

Potential external constraints include:

- denied notification permission;
- Android exact/inexact alarm capability;
- battery optimization and vendor background policy;
- force-stop behavior;
- reboot/startup timing;
- clock/time-zone/DST changes;
- iOS/macOS notification policies;
- process termination;
- Windows closed-app/in-process fallback limitations.

CareNest must not promise exact delivery as a medical guarantee.

## 3. Windows closed-app reminder behavior

The current Windows implementation includes an in-process reminder fallback. Automated tests protect timer replacement/cancellation/disposal races, but closed-app behavior is platform-constrained and still requires manual release evidence.

## 4. iOS/iPadOS real-device behavior

Automated CI compiles an iOS simulator target. Simulator compilation is not equivalent to real-device notification permission, delivery, background behavior, signing or store deployment evidence.

## 5. Mac Catalyst real-device/signed behavior

Automated builds verify Mac Catalyst compilation and unsigned inspection output. Production signing/notarization and manual notification/desktop interaction evidence remain separate gates.

## 6. Whole-database encryption is not claimed

Structured SQLite data is local and protected primarily by the application sandbox/device security. CareNest does not claim transparent whole-database encryption.

Imported document payloads and manual backups have separate authenticated-encryption protections.

## 7. App lock is not full-device security

The optional app lock is a local privacy barrier. It does not protect against every threat, including a fully compromised/rooted/jailbroken device, privileged forensic access, external exported copies or secrets exposed outside the application boundary.

## 8. External copies are outside CareNest control

After a user explicitly exports/shares information, copies may remain in:

- another application;
- email/messaging systems;
- cloud storage;
- the filesystem;
- calendar providers;
- screenshots;
- OS/device backups;
- print/PDF workflows.

Deleting local CareNest data cannot guarantee deletion of those external copies.

## 9. No CareNest cloud synchronization in v1

The current local-first release does not provide automatic CareNest cloud sync, required accounts, remote caregiver collaboration or server-side health-record storage.

Adding such features would require a new authentication, consent, privacy, threat-model, key-management, deletion/export and store-policy review.

## 10. No silent remote caregiver access

CareNest does not silently share local records with family members, clinicians or caregivers. Any future remote collaboration is outside the current RC1 scope.

## 11. No automatic prescription/pharmacy integration

The current source does not automatically contact pharmacies, refill prescriptions, validate prescriptions or purchase medicine.

## 12. No clinical interaction engine

Medicine names, strength and instructions are organizational user input. The application does not make clinical safety decisions from that text.

## 13. Reminder states are organizational records

Taken, Skipped, Delayed, Missed, Snoozed and related states are workflow/history states. They do not prove ingestion, adherence, clinical correctness or treatment effectiveness.

## 14. Historical encrypted-format compatibility needs packaged evidence

Source includes retained read compatibility for legacy encrypted framing where documented. Final production promotion still requires compatibility checks using genuine/canonical prior artifacts when they exist. A newly manufactured artifact must not be labeled historical evidence.

## 15. Existing SQLite data requires packaged upgrade validation

The source dependency graph and automated integration tests are green, but final production release still requires a realistic packaged upgrade test with representative fictional existing data, database integrity/readability/editability checks and reminder reconciliation.

## 16. Accessibility is not certified by source tests alone

CareNest includes accessibility-oriented design and source contracts, but real assistive-technology evidence remains required for:

- screen readers;
- large text/text scaling;
- keyboard/focus behavior;
- contrast/theme behavior;
- reduced motion;
- color-independent meaning.

## 17. Localization is an architecture/strategy, not proof of every locale

Localization guidance exists, but adding a language requires actual translated resources, layout review, date/time review, accessibility checks and target-specific validation.

## 18. Store policy can change

Apple, Google and Microsoft distribution requirements are time-sensitive. Repository documentation can describe the current release strategy, but submission-time policy review is required.

## 19. Production signing material is intentionally absent from Git

Private signing keys, certificates, keystores and secrets must remain outside the repository. Therefore an unsigned/internal artifact in CI is not automatically a store-ready production package.

## 20. Internal inspection artifacts are not production packages

The Store Inspection workflow creates internal evidence artifacts for payload scanning/provenance. Those artifacts can be unsigned, simulator-targeted or unpackaged by design and must not be presented as production-store deliverables.

## 21. “No known automated defect” has a precise meaning

At the PR #74 verified source boundary, the configured automated matrix is green. This means no known defect is exposed by that matrix. It does **not** mean every possible bug is mathematically impossible or that all manual/platform conditions have been tested.

## 22. Release candidate status

CareNest remains `1.0.0-rc.1` until applicable manual/device/package/accessibility/signing/store/tag/publication evidence is complete.

See `PROJECT_STATUS.md` and `docs/releases/NEXT_STEPS.md` for the current authoritative status.