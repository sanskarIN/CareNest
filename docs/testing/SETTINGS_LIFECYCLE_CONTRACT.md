# Settings lifecycle regression contract

## Purpose

The Settings lifecycle contract protects a small but security-sensitive source invariant without requiring a MAUI device runner in the platform-neutral test project.

Current contract file:

`tests/CareNest.UiTests/SettingsLifecycleContractTests.cs`

## What it verifies

### Registered secret-store dependency

The contract requires the current Settings view-model to:

- depend on `ISecretStore`;
- receive it through constructor injection;
- retain the injected instance;
- reference `SecretKeys.DocumentMasterKey`; and
- have `ISecretStore` registered to `SecureSecretStore` in `MauiProgram.cs`.

This keeps document-key cleanup inside the existing secure-storage abstraction rather than introducing a platform-specific shortcut in the view-model.

### Lifecycle ordering

The contract locates the `ResetAllDataAsync` source block and checks that the relevant operations remain ordered as follows:

1. notification cancellation;
2. encrypted-document filename capture;
3. structured repository clear;
4. encrypted payload cleanup;
5. secure document-key removal;
6. app-lock cleanup;
7. onboarding navigation.

The contract is intentionally narrow. It protects ordering and dependency wiring, while runtime behavior of the underlying repository, document store, secret store, and app-lock services remains covered by their own unit/integration/security contracts.

## Why this is a source contract

`CareNest.UiTests` is deliberately platform-neutral and does not load the MAUI application at runtime. Source contracts are used for architecture, privacy, branding, and lifecycle invariants that must remain visible in the application source but do not require device automation to verify their structure.

This does not replace manual device testing. The release matrix must still verify the user-facing full local-data clearing flow on intended Android, Windows, iOS/iPadOS, and Mac Catalyst targets.

## Verified baseline

PR #36 verified exact source SHA:

`3b19ce08f509f27aca823469abc5b8a03ed2465a`

CareNest CI #362 / run `31701943543` passed:

- 106 unit tests;
- 30 integration tests;
- 56 UI-contract tests;
- 192 total core tests;
- platform-neutral formatting;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build.

CodeQL #362 and Dependency Audit #16 also passed.

## Regression response

If this contract fails after an intentional lifecycle redesign:

1. review the failure-safety model in `docs/security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md`;
2. update implementation and documentation together;
3. do not weaken or delete the contract solely to make CI green;
4. create a new exact-head verification PR after the source change;
5. run the manual local-data clear flow on the supported device matrix before production promotion.
