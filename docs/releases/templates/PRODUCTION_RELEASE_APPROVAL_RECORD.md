# CareNest Production Release Approval Record

This is the final release-candidate approval record. It must not be completed until the applicable automated, package, device, accessibility, signing and store blockers have real evidence.

## Release identity

- Overall status: `NOT RUN`
- CareNest version/build:
- Approved source SHA:
- Immutable production tag:
- Release date/time/time zone:
- Release owner/reviewer:

## Automated verification

- Accepted automated source SHA:
- CareNest CI run/result:
- Unit tests:
- Integration tests:
- UI/source-policy tests:
- Total core tests:
- Android Release build:
- Windows Release build:
- iOS simulator Release build:
- Mac Catalyst Release build:
- Store Package Configuration:
- Store Inspection Artifacts:
- CodeQL:
- Dependency Audit:

- [ ] Every verification-relevant source change after the previous accepted baseline has a fresh exact-source matrix.

## Production package evidence

For each intended platform, record package filename, SHA-256, package-evidence JSON and signing/notarization/store-managed provenance.

### Android

- Package:
- SHA-256:
- Evidence JSON:
- Signing provenance record:
- Device validation record:
- Compatibility validation record:

### Windows

- Package:
- SHA-256:
- Evidence JSON:
- Signing provenance record:
- Device validation record:
- Compatibility validation record:

### iOS/iPadOS

- Package/build identity:
- SHA-256 where applicable:
- Evidence JSON:
- Signing provenance record:
- Device validation record:
- Compatibility validation record:

### Mac Catalyst

- Package:
- SHA-256:
- Evidence JSON:
- Signing/notarization provenance record:
- Device validation record:
- Compatibility validation record:

## Accessibility evidence

- Accessibility validation records:
- [ ] Representative screen-reader validation completed for intended platforms.
- [ ] Representative large-text/display scaling completed.
- [ ] Desktop keyboard/focus validation completed where applicable.
- [ ] Light/dark/system and color-independent meaning validated.

## Final package inspection

- [ ] Exact package/source/tag identity recorded.
- [ ] Final package SHA-256 recorded after signing/notarization transformations.
- [ ] Store-safe payload scan passed for intended final payloads.
- [ ] `buymeacoffee.com/sanskarIN` is absent from the distributed app payload.
- [ ] `ramsandesh.gumroad.com` is absent from the distributed app payload.
- [ ] Intended repository/support/legal links remain available as designed.
- [ ] Installed package starts and representative platform smoke tests pass.

## Store/policy evidence

- Store submission records:
- [ ] Submission-day policy review completed for each intended store.
- [ ] Required privacy/data-safety/health declarations completed.
- [ ] Store submission status recorded separately from approval/publication.
- [ ] Any rejection/change request is resolved or explicitly blocks release.

## Blocking defects/issues

List every unresolved production blocker. If there are none, write `None` only after verifying the repository issue/PR and evidence records.

## Approval decision

- Decision: `NOT APPROVED / APPROVED`
- Approved source SHA:
- Approved immutable tag:
- Approved package hashes:
- Approval rationale/evidence references:
- Reviewer/sign-off:

Do not move an approved/rejected immutable production tag to another source commit.
