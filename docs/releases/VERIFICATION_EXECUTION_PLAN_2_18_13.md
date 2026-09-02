# CareNest 2.18.13 Exact-Head Verification Execution Plan

**Target:** `2.18.13`
**Build/package code:** `21813`
**Created:** 2026-09-02
**State:** EXECUTION PLAN — NO RESULTS CLAIMED

This document defines the acceptance boundary for the next `2.18.13` verification candidate. It intentionally records no test count, workflow result, package hash, signing result, or production approval that has not actually been observed.

## Candidate identity

The candidate must be identified by all of the following before acceptance:

- exact Git commit SHA;
- pull-request number and head SHA;
- resulting merge commit SHA when merged;
- active semantic version `2.18.13`;
- MAUI package/build code `21813`.

A later commit creates a new candidate and invalidates acceptance evidence from the earlier SHA.

## Required automated evidence

The final candidate must independently complete the repository's configured release verification matrix:

1. CareNest CI;
2. CodeQL;
3. unsuppressed Dependency Audit;
4. Store Package Configuration;
5. Store Inspection Artifacts.

The accepted candidate must also record the observed results for:

- unit tests;
- integration tests;
- UI/source-policy tests;
- formatting/documentation checks;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- Linux desktop Release;
- WebAssembly browser Release publish.

No historical result is copied forward merely because the source is a maintenance roll-forward.

## Exact-head acceptance procedure

1. Open the preparation PR from the current candidate branch into `main`.
2. Record the PR head SHA before acceptance.
3. Wait for all required workflows to finish for that exact source.
4. Investigate every failure, cancellation, skipped required check, or infrastructure retry.
5. If source changes are required, treat the resulting SHA as a new candidate and repeat the complete acceptance procedure.
6. Merge only when the required exact-head checks are green.
7. Use an expected-head lock so a moved PR head cannot be merged accidentally.
8. Record the resulting merge commit and retain the exact pre-merge head identity.

## Evidence rules

- `PASS` requires observed evidence for the exact source/package/deployment being assessed.
- `FAIL` records an observed failed requirement.
- `BLOCKED` records a genuine external/manual blocker.
- `N/A` is used only when a requirement does not apply to the represented distribution boundary.
- `NOT RUN` is used when evidence has not yet been collected.
- Queued, stale, superseded, cancelled, or older-source results are not final acceptance evidence.

## Production boundary

This automated plan does not prove:

- installed-device behavior;
- notification delivery on real devices;
- packaged upgrade/restore compatibility;
- accessibility completion with assistive technology;
- production signing, provisioning or notarization;
- final deployment/package provenance;
- store approval or publication.

Those requirements remain governed by the version-specific release checklist and production evidence standard.

## Security boundary

No secrets belong in this record or in the repository. Evidence must not contain real health records, prescription documents, backup passwords, private signing keys, access tokens, recovery codes, or other sensitive credentials.

## Acceptance status

**No `2.18.13` exact-head verification result is claimed by this plan.**

The plan becomes evidence only when the corresponding exact-source workflow and/or production records are actually observed and recorded in their canonical evidence documents.
