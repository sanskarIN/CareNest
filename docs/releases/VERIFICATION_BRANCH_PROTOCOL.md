# Exact-Head Verification Branch Protocol

CareNest uses short-lived verification branches only when a fresh pull-request event is required to obtain GitHub-hosted build/test/security evidence for an exact `main` source head.

## Rules

1. Finish the intended `main` source/test/documentation changes first.
2. Record the exact `main` commit SHA.
3. Create a verification branch from that exact SHA.
4. Add only a small marker file under `build/verification/` on the verification branch.
5. Open a pull request to `main`.
6. Verify that the PR diff contains only the marker file.
7. Wait for the required workflows to complete.
8. If any workflow exposes a real source/test/configuration defect, fix that defect on `main`, close the now-stale verification PR without merging, then create a new exact-head verification branch.
9. When all required gates pass, record run IDs/results in `PROJECT_STATUS.md`, `what_changed.md`, and the release checklist.
10. Close the marker PR without merging. Do not add verification marker files to production `main`.

## Required automated gates

At minimum, the exact head must have successful evidence for:

- Core tests and the platform-neutral formatting gate;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build;
- CodeQL;
- dependency audit.

A release/tag may require additional evidence from `CareNest Release Evidence` and the manual release matrix.

## Why the marker is not merged

The marker exists only to trigger pull-request workflows. It is not application source, release metadata or runtime configuration. Keeping it outside `main` avoids accumulating test-trigger artifacts in production history.

## Evidence integrity

The verification PR base SHA must match the intended `main` head at the time the branch was created. If `main` advances with any change that affects the intended release evidence, the verification result is stale for the newer head and must not be reused as exact-head proof.
