# CareNest Store Inspection Artifacts Verification — 2026-08-15

This document records the exact automated verification and internal inspection-artifact evidence for the CareNest source boundary that introduced store-safe inspection artifact generation.

It does **not** represent production signing, installed-package manual testing, store submission, store approval, accessibility certification, or packaged existing-user-data compatibility.

## Frozen source boundary

Frozen verification-relevant `main` source:

`4c60f90ac33a321d12a6f9b3a8c097e4e4a4e5f2`

Verification branch:

`ci/carenest-store-inspection-artifacts-final-v2-20260815`

Marker/head SHA:

`19c82b813c375047cf1166487bc18a1bd2cd0e52`

Marker path:

`build/verification/store-inspection-artifacts-final-v2-20260815.txt`

Verification PR:

`https://github.com/sanskarIN/CareNest/pull/61`

PR #61 changed exactly one file beyond the frozen source: the verification marker. GitHub reported 8 additions, 0 deletions, and one commit. The PR was closed without merge after successful evidence capture, so the marker never entered `main`.

GitHub's pull-request merge/event SHA during the run was:

`c8ea9fef89d7b773f19bf13c64f349495be706ad`

The inspection workflow deliberately records this event SHA separately from the marker/source head so provenance does not conflate GitHub's temporary PR merge ref with the source branch being inspected.

## Verification-relevant changes in this source boundary

The source lineage after the PR #59 baseline added:

- a store-safe About command that becomes non-executable when `CARENEST_FUNDING_LINK` is absent;
- regression coverage for the hidden/non-executable funding command;
- Windows `RuntimeIdentifierOverride` mapping for portable publish isolation;
- package metadata coverage for that Windows mapping;
- a dedicated `CareNest Store Inspection Artifacts` workflow;
- exact `v*`/manual coverage for the new inspection workflow;
- source-policy contracts for artifact shape, funding-disabled configuration, unsigned/internal status, checksums, provenance and secret absence;
- fail-closed Android staging that excludes MAUI's debug-signed companion and requires exactly one unsigned AAB candidate;
- inspection of Android AAB signing metadata before upload;
- exact PR-head checkout/artifact naming and separate event-SHA provenance.

## Historical PR #60 checkpoint — superseded

PR #60 was the first marker-only runtime exercise of the new artifact workflow. It is **not** final release evidence.

Frozen source/base:

`e9f7ab64dd73d22ee5fe7e608d73d7cfcaf7fcff`

Marker/head:

`6c618aa4ac2440c0718d4d1dc207125494dd9ec1`

PR:

`https://github.com/sanskarIN/CareNest/pull/60`

The Android and Windows artifact jobs completed, but downloading and inspecting the Android artifact exposed two evidence defects that normal workflow status alone did not reveal:

1. MAUI emitted both an unsigned AAB and a debug-signed `-Signed.aab` companion; the workflow staged both while provenance said signing was disabled.
2. PR artifact naming/provenance used `github.sha`, which is GitHub's temporary pull-request merge SHA rather than the marker branch head.

The signed companion was independently inspected and reported the standard Android Debug certificate identity (`CN=Android Debug, O=Android, C=US`).

PR #60 was therefore explicitly superseded and closed without merge. Its marker is not part of `main`.

The corrected source then:

- excluded `*-Signed.aab` from staging;
- required exactly one unsigned AAB candidate;
- rejected AABs containing JAR-signature metadata;
- marked Android provenance `signing=verified-unsigned`;
- recorded `debug_signed_companion_staged=false`;
- separated `CARENEST_SOURCE_SHA`/`CARENEST_SOURCE_REF` from `GITHUB_SHA`/`GITHUB_REF`;
- checked out the exact source head for artifact generation;
- named artifacts with the exact source head.

## PR #61 exact automated evidence

### CareNest CI #650

Run ID:

`31872610834`

Result:

**success**

Core results:

- platform-neutral formatting: **success**;
- UnitTests: **122 passed, 0 failed, 0 skipped**;
- IntegrationTests: **39 passed, 0 failed, 0 skipped**;
- UiTests/source-policy: **157 passed, 0 failed, 0 skipped**;
- total: **318 passed, 0 failed, 0 skipped**.

Default Release configuration:

- Android: **success**;
- Windows: **success**;
- iOS simulator: **success**;
- Mac Catalyst: **success**.

### CodeQL #650

Run ID:

`31872610815`

Result:

**success**.

### Dependency Audit #46

Run ID:

`31872610791`

Result:

**success**.

The unsuppressed audit completed both platform-neutral and MAUI application dependency graphs. The former SQLite advisory suppression remains absent.

### CareNest Store Package Configuration #39

Run ID:

`31872610789`

Result:

**success**.

Funding-disabled Release configuration:

- Android with `CareNestShowFundingLink=false`: **success**;
- Windows with `CareNestShowFundingLink=false`: **success**;
- iOS simulator with `CareNestShowFundingLink=false`: **success**;
- Mac Catalyst with `CareNestShowFundingLink=false`: **success**;
- Bash store-package preflight executable-mode guard: **success**.

### CareNest Store Inspection Artifacts #2

Run ID:

`31872610786`

Result:

**success**.

All three artifact jobs succeeded:

- Android unsigned AAB inspection artifact;
- Windows self-contained unpackaged inspection artifact;
- Apple iOS-simulator + unsigned Mac Catalyst inspection artifacts.

## Android inspection artifact

Artifact ID:

`9243915053`

Artifact name:

`carenest-android-store-safe-inspection-19c82b813c375047cf1166487bc18a1bd2cd0e52`

GitHub artifact API digest:

`sha256:ac0039136e3608319df2927fbb38acf383445b022596ce4f86633b39f882c164`

Downloaded payload contained exactly:

- `com.sanskar.carenest.aab`;
- `SHA256SUMS.txt`;
- `provenance.txt`.

AAB SHA-256:

`fea87ddc9e790d4c88f4de382f70a121c57f308e9f476bc52b57f3bd091ce080`

Independent downloaded-artifact checks confirmed:

- the checksum matched;
- exactly one `.aab` was present;
- no `*-Signed.aab` companion was present;
- no `META-INF` JAR signature metadata with `.RSA`, `.DSA`, `.EC` or `.SF` suffix was present;
- provenance reported `signing=verified-unsigned`;
- provenance reported `debug_signed_companion_staged=false`;
- `CareNestShowFundingLink=false` was recorded;
- `source_sha=19c82b813c375047cf1166487bc18a1bd2cd0e52`;
- `event_sha=c8ea9fef89d7b773f19bf13c64f349495be706ad`;
- artifact purpose was `internal-inspection-only`;
- `store_submission_ready=false`.

This AAB is intentionally unsigned and is **not** the signed Google Play production candidate.

## Windows inspection artifact

Artifact ID:

`9243904498`

Artifact name:

`carenest-windows-store-safe-inspection-19c82b813c375047cf1166487bc18a1bd2cd0e52`

GitHub artifact API digest:

`sha256:c0c7dd46ad8ec38e2295da0e1e0c8c69ece690f024c248b82ee09a0721a999f6`

Downloaded payload contained:

- `CareNest-windows-win-x64-store-safe.zip`;
- `SHA256SUMS.txt`;
- `provenance.txt`.

Nested ZIP SHA-256:

`08b4de53dcebc7d88031f4ae3f243e6579e8ad556bcf1e299c6294399b978ac0`

Independent downloaded-artifact checks confirmed the checksum and provenance.

Recorded properties include:

- source head `19c82b813c375047cf1166487bc18a1bd2cd0e52`;
- event SHA `c8ea9fef89d7b773f19bf13c64f349495be706ad`;
- target `net10.0-windows10.0.19041.0`;
- runtime identifier `win-x64`;
- `CareNestShowFundingLink=false`;
- `WindowsPackageType=None`;
- self-contained Windows App SDK deployment;
- `signing=not_applicable_unpacked_bundle`;
- internal-inspection-only purpose;
- `store_submission_ready=false`.

This is an unpackaged/internal inspection bundle, not a signed Microsoft Store package.

## Apple inspection artifact

Artifact ID:

`9244085155`

Artifact name:

`carenest-apple-store-safe-inspection-19c82b813c375047cf1166487bc18a1bd2cd0e52`

GitHub artifact API digest:

`sha256:e82e6fe2022a7a5cf6ead34744876561c4c93e550e5d34fe192098455ea6ebd2`

Downloaded payload contained:

- `CareNest-iossimulator-arm64-store-safe.tar.gz`;
- `CareNest-maccatalyst-arm64-store-safe.tar.gz`;
- `SHA256SUMS.txt`;
- `provenance.txt`.

iOS simulator tar SHA-256:

`6ad6077fff0ac0f9b5bd5d8a03b73c0e2abf7fb6c825e7db2408204c58f02d65`

Mac Catalyst tar SHA-256:

`fb98371db1c54cfac766d126f3eebace53269ee3c150b49c077f1637115d67d8`

Independent downloaded-artifact checks confirmed both checksums and provenance.

Extracted-bundle inspection found:

- iOS simulator `.app`: present;
- Mac Catalyst `.app`: present;
- embedded iOS `mobileprovision`: absent;
- embedded Mac Catalyst provisioning profile: absent;
- Mac Catalyst `_CodeSignature` directory: absent;
- iOS simulator `_CodeSignature` resources: present, consistent with simulator build behavior rather than production provisioning;
- provenance reported `code_signing=disabled_or_simulator_only`;
- `CareNestShowFundingLink=false` was recorded;
- source/event identities were separated;
- artifact purpose was internal inspection only;
- `store_submission_ready=false`.

These artifacts are not a signed iOS archive or signed/notarized/store-ready Mac Catalyst production package.

## Source evidence versus production package evidence

PR #61 establishes:

- exact source compilation in default and funding-disabled configurations;
- exact source-policy/security/dependency verification;
- reproducible funding-disabled internal inspection artifacts;
- checksum and provenance generation;
- corrected unsigned-only Android artifact staging;
- explicit distinction between source head and PR merge/event SHA.

PR #61 does **not** establish:

- Android production keystore signing;
- Apple production certificates/provisioning;
- Windows package signing identity;
- signed Google Play/App Store/Microsoft Store candidate packages;
- installation on representative real devices/OS environments;
- actual About-page visual inspection on installed production packages;
- packaged SQLite existing-data upgrade compatibility;
- encrypted-document/historical-backup packaged compatibility;
- actual reminder delivery/reboot/time-zone/force-stop behavior;
- accessibility with representative assistive technology;
- submission-time Apple/Google policy approval;
- store listing/privacy/data-safety completion;
- production `v*` release tag approval/publication.

## Current authoritative automated source baseline

PR #61 supersedes PR #59 only as the latest exact automated source and internal inspection-artifact baseline.

PR #59 remains valid historical exact evidence for its own store-safe compilation boundary. PR #58, PR #56 and PR #54 remain valid historical evidence for their own frozen source boundaries.

Any later runtime, test, project, package, platform, workflow, artifact-generation, build or release-script change requires a new exact-head verification before production promotion.
