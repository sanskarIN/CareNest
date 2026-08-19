# CareNest Backup Resource Hardening — 2026-08-19

**Release line:** `1.0.0-rc.1`  
**PR:** `#81`  
**Accepted automated baseline before this work:** `b6eecae66f74bd72bcb20d93508355542f9f3442`  
**Status:** implementation and regression coverage complete on the PR branch; exact-source automated verification required before promotion

## 1. Finding

The existing encrypted-backup path authenticated backup bytes and strictly validated archive topology, but a deliberately crafted password-valid backup could still declare very large ZIP entries or a very large decrypted ZIP payload before normal topology/extraction checks completed.

That is an availability/resource-exhaustion issue rather than a confidentiality bypass: authenticated encryption, wrong-password rejection and topology validation still applied, but local disk/CPU consumption was not explicitly bounded at every stage.

No real health data or production backup was used to reproduce or address this finding.

## 2. Implemented controls

Current PR behavior adds fail-closed limits for:

- decrypted ZIP container: **2304 MiB** maximum;
- manifest: **1 MiB** maximum;
- SQLite database: **1 GiB** maximum;
- each encrypted document entry: **512 MiB** maximum;
- total uncompressed ZIP payload: **2 GiB** maximum;
- document count: **5,000** maximum;
- total archive entry count: bounded from the document limit plus required fixed entries;
- explicit directory-only ZIP entries: rejected.

The encrypted chunked-decryption primitive now supports an optional maximum plaintext byte count. Backup decryption supplies the decrypted-container ceiling so a malicious archive cannot first expand to an unbounded temporary ZIP and only then reach topology validation.

## 3. Validation ordering

The backup path now applies controls in this order:

1. validate outer CareNest backup header/version;
2. derive the password key;
3. decrypt authenticated chunked payload while enforcing the decrypted-container byte ceiling;
4. open the bounded ZIP;
5. validate archive entry count/directory-entry policy and manifest size before manifest deserialization;
6. validate per-entry and total uncompressed limits;
7. deserialize and validate the manifest;
8. validate strict database/key/document topology/count agreement;
9. only then extract for restore;
10. validate restored SQLite integrity/schema before replacement.

Existing cryptographic tamper/truncation/trailing-data behavior remains separate and intact.

## 4. Creation/restore symmetry

CareNest now validates a newly generated ZIP against the same container/topology/resource boundary before encrypting it.

This prevents the current application from intentionally creating a backup that its current restore/inspection path would reject because of the newly introduced resource ceilings.

If genuine historical backup bytes are later found to exceed one of these ceilings, that must be treated as an explicit compatibility finding. Do not silently weaken a security limit or manufacture replacement bytes and label them historical evidence.

## 5. Regression coverage added

Focused integration coverage now includes:

- oversized manifest rejection before parsing;
- oversized database rejection;
- oversized document rejection;
- excessive total uncompressed payload rejection;
- excessive entry/document-count rejection;
- directory-only archive-entry rejection;
- manifest document-count ceiling rejection;
- decrypted-container over-limit rejection;
- decrypted-container exact-limit acceptance;
- encrypted plaintext over-limit rejection before first chunk write;
- encrypted plaintext exact-limit acceptance;
- invalid non-positive decrypt-limit rejection;
- cumulative limit enforcement across multiple encrypted chunks;
- legacy framing v1 enforcement of the same optional plaintext limit.

The accepted pre-change baseline had 39 integration tests and 355 total core tests. This PR adds 14 integration tests, so the expected inventory is 53 integration tests and 369 total core tests if the exact-source matrix confirms all tests and existing suites unchanged.

Do not record those expected counts as passed until GitHub Actions reports success for the final PR head.

## 6. Required automated verification before promotion

Because this changes security-relevant runtime source, require the final PR head to pass at minimum:

- CareNest CI, including formatting and all core tests;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build;
- Store Package Configuration;
- Store Inspection Artifacts;
- CodeQL;
- unsuppressed Dependency Audit;
- documentation-link validation.

Only after the exact final PR head is green may the accepted automated baseline be advanced.

## 7. Required packaged/manual follow-up

Automated small-fixture tests intentionally avoid creating multi-gigabyte public artifacts. Production/package validation must still confirm with fictional data that:

- representative normal backups remain below the configured ceilings;
- backup create/inspect/restore works on each intended package/platform path;
- clean-install restore works;
- encrypted documents remain usable after restore;
- wrong-password/tamper/truncation/trailing-data rejection remains correct;
- any genuine historical backup fixture remains compatible;
- an over-limit historical fixture, if one genuinely exists, is recorded as a compatibility/security decision rather than silently bypassed.

## 8. Baseline rule

Until final PR #81 verification succeeds, the accepted automated production-candidate reference remains:

`b6eecae66f74bd72bcb20d93508355542f9f3442`

This document records a security hardening continuation. It is not production signing, real-device validation, store approval or publication evidence.
