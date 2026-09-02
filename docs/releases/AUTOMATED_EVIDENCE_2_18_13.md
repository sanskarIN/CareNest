# CareNest 2.18.13 Automated Verification Evidence

**Version:** `2.18.13`
**Build/package code:** `21813`
**Accepted source head:** `e3f57d9514edc61b15ff699051f93847d225a085`
**Verification date:** 2026-09-02
**State:** AUTOMATED VERIFICATION OBSERVED — PRODUCTION NOT PUBLISHED

This record captures only GitHub Actions results observed for the exact pre-merge `2.18.13` candidate head. It does not transfer historical evidence from another source and does not claim real-device, signing, store approval, publication, or other production/manual evidence.

## Required workflow results

| Workflow | Run | Result |
|---|---:|---|
| CareNest CI | `33619406293` | PASS |
| CodeQL | `33619406365` | PASS |
| Dependency Audit | `33619406326` | PASS |
| CareNest Store Package Configuration | `33619406392` | PASS |
| CareNest Store Inspection Artifacts | `33619406295` | PASS |

## CareNest CI coverage

The exact candidate completed successfully for:

- WebAssembly browser build/publish;
- Linux desktop build;
- Windows build;
- Core tests and repository verification tooling;
- Apple build, including iOS simulator and Mac Catalyst;
- Android build.

The Core tests job also completed its configured Python tooling syntax, cross-platform target verification, package-evidence self-test, documentation-link self-test, active documentation-link verification, formatting, unit-test, integration-test, and UI-contract-test steps successfully.

## Store/package coverage

The exact candidate completed successfully for:

- Windows store-safe build;
- Android store-safe build;
- Apple store-safe builds, including iOS simulator and Mac Catalyst;
- Android unsigned AAB inspection artifact;
- Windows self-contained inspection artifact;
- store-safe payload scanner self-test;
- Apple unsigned inspection artifacts, including iOS simulator and Mac Catalyst publication/staging steps.

## Security coverage

CodeQL and Dependency Audit both completed successfully for the exact candidate head.

## Evidence boundary

These automated results prove only the configured repository automation executed successfully for this exact source. They do not prove:

- real-device notification delivery;
- installed-package upgrade/restore compatibility;
- accessibility validation with assistive technology;
- production signing/provisioning/notarization;
- final production package hashes or deployment provenance;
- store submission, review, approval, or publication;
- global bug-free behavior.

Those requirements remain `NOT RUN`, `BLOCKED`, `N/A`, or otherwise unresolved until genuine evidence exists.

## Merge identity

The candidate was merged only after the five required workflow groups above completed successfully. The exact pre-merge source identity is retained as:

`e3f57d9514edc61b15ff699051f93847d225a085`

The resulting merge commit is:

`8259d937250ef2d4fc0f71341b2b16f780014a52`

A later source commit is a new verification candidate and must earn its own exact-head evidence.
