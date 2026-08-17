# CareNest Gumroad Rollout Verification — 2026-08-17

**Release line:** `1.0.0-rc.1`  
**Verified implementation/source-policy SHA:** `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`

This record is the authoritative automated verification evidence for the repository-first Gumroad rollout and the associated package-isolation/source-policy changes.

## Scope verified

The verified source includes:

- Gumroad repository branding and canonical storefront documentation;
- highlighted repository support/documentation/marketing links;
- `.github/FUNDING.yml` Gumroad custom link;
- repository-only `docs/assets/gumroad_store_badge.svg`;
- Gumroad/BMC health-entitlement separation wording;
- Gumroad/BMC absence from CareNest runtime/application resources;
- two-marker store-payload scanning;
- expanded Gumroad source-policy tests;
- source-line defect-pattern audit;
- structured runtime-file syntax validation;
- refreshed current project/build/testing/store/release documentation.

## Candidate failure that was corrected

The first documentation-finalization candidate `b5a57186af60e8b42bb917dfa85de24c3c9c1e9a` exposed one newly added test-contract wording mismatch.

The documentation correctly stated that “Gumroad purchases **do not unlock** medical advice...”, while the new test searched for the singular phrase `does not unlock`.

This was a false-positive wording assertion, not an application/runtime defect and not a weakening of the health-safety rule.

Correction commit:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

Commit message:

`test: align Gumroad entitlement wording contract`

The corrected test requires the actual documented plural statement `do not unlock medical advice` and retains diagnosis/dosage/health-data assertions.

## Core test result

CareNest CI run:

`32032436061`

Results on exact SHA `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`:

- formatting: **success**;
- unit tests: **122/122 passed**;
- integration tests: **39/39 passed**;
- UI/source-policy tests: **175/175 passed**;
- total core tests: **336/336 passed**.

The UI/source-policy count increased from 173 to 175 because the Gumroad rollout added independent repository-placement/accessibility/package-isolation coverage.

## Normal platform builds

Same CareNest CI run `32032436061`:

- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**.

## Store-candidate configuration builds

Store Package Configuration run:

`32032436093`

Results on exact SHA `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`:

- Android store-candidate configuration: **success**;
- Windows store-candidate configuration: **success**;
- iOS simulator store-candidate configuration: **success**;
- Mac Catalyst store-candidate configuration: **success**.

## CodeQL

CodeQL run:

`32032436037`

Result:

- C# CodeQL analysis: **success**.

## External-commerce package policy verified by source contracts

The verified source keeps these destinations repository-only:

```text
https://buymeacoffee.com/sanskarIN
https://ramsandesh.gumroad.com
```

The application runtime/source-policy tests require both destinations to remain absent from CareNest application text-like runtime source/resources and shared runtime URL constants.

`build/scripts/verify-store-safe-payload.py` defaults to both markers and retains UTF-8, UTF-16 LE, UTF-16 BE, regular-file, ZIP/AAB-entry and fail-closed inspection behavior.

The repository Gumroad SVG remains outside `src/CareNest.App`.

## Health/privacy boundary

The verified repository documentation and tests preserve that Gumroad purchases or project funding do not unlock or alter:

- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- clinical medication-interaction/risk behavior;
- reminder priority or delivery guarantees;
- emergency assistance;
- clinical support entitlement;
- CareNest account/cloud behavior;
- user health-data access.

CareNest does not automatically transmit local health records to Gumroad or Buy Me a Coffee.

## What this verification does not prove

This automated evidence does not replace:

- representative Android real-device/emulator notification behavior;
- Windows installed/lifecycle behavior;
- real iPhone/iPad notification behavior;
- Mac Catalyst signed/manual behavior;
- packaged existing-data SQLite upgrade compatibility;
- packaged encrypted-document compatibility;
- packaged encrypted-backup compatibility/tamper/wrong-password testing;
- real assistive-technology/accessibility validation;
- production signing identities;
- final signed-package payload scans/checksums/provenance;
- submission-time Apple/Google/Microsoft policy review;
- immutable production tag/publication evidence.

CareNest therefore remains `1.0.0-rc.1`.

## Documentation-only commits after this verified implementation SHA

Current documentation may receive follow-up commits that promote this exact result into README/status/catalog/handoff surfaces. Those commits do not change the verified runtime/test/scanner behavior unless explicitly stated, but any final repository-head claim should still use the workflows associated with the exact final head.

## Authority

Use this file as the automated Gumroad-rollout evidence record. Use:

- `PROJECT_STATUS.md` for active project status;
- `docs/releases/NEXT_STEPS.md` for remaining production work;
- `what_changed.md` for the complete continuation/commit history;
- `GUMROAD.md` for the canonical storefront guide;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` for placement/package policy.
