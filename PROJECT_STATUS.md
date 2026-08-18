# CareNest — Current Project Status

**Date:** 2026-08-18  
**Release line:** `1.0.0-rc.1`  
**Repository:** `https://github.com/sanskarIN/CareNest`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`

The complete project status that was active before the Gumroad rollout is preserved exactly at:

`docs/history/pre-gumroad-rollout-20260817/PROJECT_STATUS.md`

Do not treat that preserved historical snapshot as the current status merely because it contains detailed older verification records.

---

## 1. Current product boundary

CareNest is a local-first organizational health application built with .NET MAUI.

CareNest does **not**:

- diagnose conditions;
- calculate or infer medicine dosage;
- recommend treatment;
- perform clinical medication-interaction checking;
- calculate clinical risk scores;
- independently prove adherence;
- replace clinicians or pharmacists;
- provide emergency services;
- guarantee operating-system notification delivery.

The current release remains account-free and local-first. It does not require a CareNest cloud backend and does not silently upload local health records.

---

## 2. Current implementation state

The source-controlled `1.0.0-rc.1` product scope remains source-complete for the intended RC feature set, including:

- multiple local person/family profiles;
- medicine records with user-entered strength/instruction text;
- explicit schedules and deterministic reminder occurrences;
- reminder history/status/reconciliation and compensation behavior;
- appointments and optional reminders;
- stock/refill organization;
- encrypted imported-document vault;
- password-encrypted manual backup/restore;
- optional local app lock;
- reports/exports;
- privacy-aware diagnostics;
- light/dark/system themes;
- accessibility-oriented source contracts;
- Android, Windows, iOS/iPadOS and Mac Catalyst targets;
- strict compiled XAML bindings;
- automated source-line/structured-file quality contracts;
- CodeQL, dependency and store/release gates.

---

## 3. Gumroad storefront rollout

The Ram Sandesh Gumroad storefront is a first-class **repository/documentation** surface:

**https://ramsandesh.gumroad.com**

Current repository integration includes:

- highlighted storefront badge and URL in the main README;
- highlighted storefront in support documentation;
- GitHub repository custom funding/project metadata;
- `GUMROAD.md` canonical storefront guide;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`;
- `docs/marketing/GUMROAD_ROLLOUT_CHECKLIST.md`;
- `docs/assets/gumroad_store_badge.svg`;
- contributor/developer/governance documentation;
- automated repository-placement and package-isolation tests;
- store-payload scanner coverage for the Gumroad marker.

A Gumroad purchase is separate from CareNest health functionality and does not unlock diagnosis, dosage decisions, treatment recommendations, reminder priority/reliability, emergency assistance, clinical support, accounts/cloud services, or access to user health data.

CareNest does not automatically transmit local health records to Gumroad.

---

## 4. External-commerce application-package boundary

The current distributed CareNest application source/package intentionally contains no external:

- `ramsandesh.gumroad.com` destination;
- `buymeacoffee.com/sanskarIN` destination;
- Gumroad/Buy Me a Coffee promotional runtime command;
- Gumroad/Buy Me a Coffee promotional XAML surface;
- repository promotional Gumroad badge in app resources.

Repository support, storefront and marketing surfaces remain separate from the health-app package under the current release/store policy.

The store-safe payload scanner defaults to both repository-only markers:

- `buymeacoffee.com/sanskarIN`;
- `ramsandesh.gumroad.com`.

It checks regular payload files and ZIP-compatible package entries using UTF-8 and UTF-16 marker encodings and fails closed for inspection errors.

---

## 5. Latest fully verified implementation/source-policy baseline

Exact verified Gumroad implementation/source-policy SHA:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

That exact source passed:

- unit tests: **122/122**;
- integration tests: **39/39**;
- UI/source-policy tests: **175/175**;
- total core tests: **336/336**;
- Android Release build;
- Windows Release build;
- iOS simulator Release build;
- Mac Catalyst Release build;
- Android store-candidate configuration;
- Windows store-candidate configuration;
- iOS simulator store-candidate configuration;
- Mac Catalyst store-candidate configuration;
- CodeQL.

Authoritative verification record:

`docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`

The two-test increase from 173 to 175 UI/source-policy tests in that verified baseline was the intended Gumroad repository-placement/accessibility/package-isolation expansion.

---

## 6. Current verification-relevant continuation — new exact-head verification required

After the 336-test baseline was verified, current `main` gained additional **verification-relevant** release-engineering source.

New current source now includes:

- `tests/CareNest.UiTests/ReleaseDocumentationConsistencyContractTests.cs`;
- `tests/CareNest.UiTests/PackageEvidenceToolContractTests.cs`;
- `build/scripts/create-package-evidence.py`;
- `build/scripts/create-package-evidence.sh`;
- `build/scripts/create-package-evidence.ps1`;
- `build/scripts/test-create-package-evidence.py`;
- CareNest CI package-evidence syntax/self-test steps;
- Release Gate package-evidence syntax/self-test and required-evidence checks;
- Release Evidence package-evidence self-test capture;
- current release documents consumed by the new consistency contracts.

Because tests, workflows, build scripts and verification-sensitive release documents changed, these later commits are **not** documentation-only for verification purposes.

Therefore:

- `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f` remains the latest fully verified baseline until a replacement exact source completes the applicable matrix;
- **336/336 must not be presented as the test result of the newer current head**;
- no newer test total should be predicted from the number of added test methods;
- a fresh exact-head verification must record the actual unit/integration/UI totals and workflow results before a newer baseline is promoted.

Protocol:

`docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`

---

## 7. Structured final-package evidence tooling

Current source now includes a fail-closed package checksum/provenance generator:

`build/scripts/create-package-evidence.py`

Cross-platform wrappers:

- `build/scripts/create-package-evidence.sh`;
- `build/scripts/create-package-evidence.ps1`.

Synthetic self-test:

`build/scripts/test-create-package-evidence.py`

Guide:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

The tool records:

- exact source SHA;
- release tag when supplied;
- stage/platform/version/build/package identity;
- tracked-workspace clean state;
- non-secret signing/notarization/store-managed provenance text;
- per-file SHA-256;
- top-level file or deterministic directory payload SHA-256;
- store-safe scanner result;
- optional non-sensitive notes.

Production mode fails closed unless:

- a `v*` tag is supplied;
- the tag resolves to the recorded source SHA;
- checked-out HEAD equals that source SHA;
- tracked Git files are clean;
- signing provenance is not empty/unsigned/not-applicable;
- the existing store-safe payload scanner passes;
- evidence output is outside the payload being hashed.

The tool does not sign packages, verify real signing credentials by itself, perform store submission, prove store approval, replace real-device testing, replace accessibility testing, or replace packaged database/encryption compatibility testing.

---

## 8. Package-evidence automated protection

CareNest CI now:

- syntax-checks the store-safe scanner, package-evidence generator and synthetic self-test with `python3 -m py_compile`;
- runs `python3 build/scripts/test-create-package-evidence.py` before the .NET formatting/test steps.

The synthetic self-test covers:

- safe single-file SHA-256 evidence;
- deterministic directory evidence;
- Gumroad marker fail-closed behavior;
- rejection of evidence output inside the payload;
- rejection of production evidence without a `v*` tag.

The Release Gate repeats package-tool syntax/self-test checks and requires the current package-evidence/release documents to exist and be non-empty.

The Release Evidence workflow also runs the package-tool self-test, stores its output in release evidence, and treats failure as a release-evidence failure.

These workflow changes require fresh exact-source verification before they become a new accepted automated baseline.

---

## 9. Release-documentation consistency protection

Current UI/source-policy tests protect active release documentation from drifting back to superseded release evidence.

The consistency contract requires applicable current documents to preserve:

- the current verified `94e867...` baseline until replaced by real newer evidence;
- the recorded **336/336** result only as that exact baseline's result;
- both repository-only external-commerce markers in final-package evidence rules;
- the 2026-08-18 store-policy review link without misrepresenting it as store approval;
- open live Google Play Health apps/Data safety and submission-day Apple/Google/Microsoft review gates;
- package-evidence guide integration;
- Release Gate requirements for current release evidence/tooling.

This converts previously manual documentation alignment into CI-enforced release governance.

---

## 10. Source-line and structured-file quality state

The current UI/source-policy suite includes a deterministic source quality contract that scans runtime C# lines for known defect patterns such as:

- unresolved merge-conflict markers;
- unfinished `TODO`/`FIXME`/`HACK` placeholders;
- `NotImplementedException` placeholders;
- common sync-over-async forms;
- `Thread.Sleep`/`Task.WaitAll`/`Task.WaitAny` runtime patterns;
- `throw ex;` stack-trace destruction.

It also parses structured runtime files including XAML, project/XML-family files and JSON so malformed structured source inputs fail the repository quality gate.

The broad scanner intentionally does not classify every direct clock read as a defect; time semantics are handled by more specific tests/rules.

---

## 11. Strict XAML binding state

The application retains:

- `MauiEnableXamlCBindingWithSourceCompilation=true`;
- `MauiStrictXamlCompilation=true`;
- `XC0022`, `XC0023`, `XC0024`, `XC0025` as warnings-as-errors;
- typed binding-bearing pages/templates;
- no intended `NoWarn`/type-safety bypass for the compiled-binding policy.

Permanent historical verification for the compiled-binding migration remains:

`docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`

---

## 12. Security/dependency state

Current policy retains:

- CodeQL scanning;
- blocking dependency auditing;
- no restoration of the former exact SQLite advisory suppression merely to make audit green;
- privacy-aware logging contracts;
- encrypted imported-document and manual-backup protections;
- source/package external-commerce isolation contracts;
- release/store verification mechanisms;
- fail-closed structured final-package evidence generation.

A green source dependency graph does not prove packaged existing-data/encrypted-data upgrade compatibility; that remains a separate production gate.

---

## 13. Current store-policy review

A dated pre-submission store-policy review was completed on **2026-08-18** and recorded at:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

The review checked the current CareNest product/package boundary against:

- Apple App Review Guidelines relevant to medical/health functionality, privacy, completeness and external purchase links;
- Google Play Health Content and Services, Health apps declaration and Data safety requirements;
- Microsoft Store rules for personal and highly sensitive information.

The review preserves the current conservative release choice: Gumroad and Buy Me a Coffee remain repository/documentation surfaces and stay outside the distributed CareNest health-app package.

This dated review is not store approval. The official policy pages and live store-console declarations must be re-checked against the exact production binary/listing immediately before submission.

---

## 14. Documentation state

Current documentation entry points include:

- `README.md` — public project overview and highlighted Gumroad storefront;
- `docs/README.md` — documentation hub;
- `docs/DOCUMENTATION_CATALOG.md` — authority/ownership map;
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md` — whole-project reference;
- `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md` — latest fully verified implementation/source-policy evidence;
- `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` — fresh exact-head verification protocol;
- `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — structured package checksum/provenance tool guide;
- `docs/releases/RELEASE_EVIDENCE.md` — release evidence contract;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md` — packaged compatibility runbook;
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md` — final submission checklist;
- `docs/releases/STORE_POLICY_REVIEW_20260818.md` — current dated pre-submission store-policy review;
- `docs/DEVELOPER_REFERENCE.md` — developer rules;
- `docs/REPOSITORY_GOVERNANCE.md` — source/evidence/marketing governance;
- `GUMROAD.md` — canonical storefront guide;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — storefront/package policy;
- `docs/assets/README.md` — repository visual-asset rules;
- `SUPPORT.md` and `docs/SUPPORT_CARENEST.md` — support/storefront separation;
- `what_changed.md` — active detailed continuation record;
- `docs/history/` — immutable historical snapshots.

Dated historical evidence is not rewritten to retroactively insert current Gumroad promotion or newer test counts.

---

## 15. Production blockers still open

Automated source/build verification, package-evidence tooling, and the 2026-08-18 pre-submission policy review do not complete production release evidence.

### Immediate exact-source automated gate

- [ ] freeze the final intended verification-relevant `main` SHA after this continuation;
- [ ] run fresh exact-head CareNest CI including package-evidence syntax/self-test;
- [ ] record actual unit/integration/UI test counts;
- [ ] verify Android Release;
- [ ] verify Windows Release;
- [ ] verify iOS simulator Release;
- [ ] verify Mac Catalyst Release;
- [ ] verify Store Package Configuration on all four targets;
- [ ] verify Store Inspection Artifacts;
- [ ] verify CodeQL;
- [ ] verify unsuppressed Dependency Audit;
- [ ] promote a newer baseline only after the complete required matrix succeeds.

### Real-device/platform behavior

- [ ] representative Android device/emulator matrix;
- [ ] Android notification permission denied/granted behavior;
- [ ] actual Android reminder delivery/cancellation/snooze behavior;
- [ ] exact/inexact alarm and battery-optimization behavior;
- [ ] Android reboot/restart/clock/time-zone/DST recovery;
- [ ] Windows manual reminder/lifecycle behavior;
- [ ] real iPhone/iPad notification permission/delivery/recovery;
- [ ] Mac Catalyst manual notification/lifecycle behavior.

### Packaged data/encryption compatibility

Use fictional/synthetic data only.

- [ ] representative existing-data packaged upgrade/install;
- [ ] SQLite integrity/readability/editability after upgrade;
- [ ] reminder reconciliation after packaged upgrade;
- [ ] packaged encrypted-document compatibility;
- [ ] packaged encrypted-backup create/restore/wrong-password/tamper validation;
- [ ] genuine historical encrypted fixtures where real prior bytes exist and can be safely tested.

### Accessibility

- [ ] representative screen-reader testing;
- [ ] large-text/text-scaling validation;
- [ ] keyboard/focus validation on desktop targets;
- [ ] light/dark/system contrast validation;
- [ ] reduced-motion validation;
- [ ] color-independent state verification.

### Signing/final package evidence

- [ ] production Android signing identity/secrets outside Git;
- [ ] Apple signing/provisioning outside Git;
- [ ] Windows production signing identity outside Git;
- [ ] final production-signed packages;
- [ ] structured package evidence JSON for every final production artifact;
- [ ] package evidence payload SHA-256 cross-check;
- [ ] signed-package BMC marker scan;
- [ ] signed-package Gumroad marker scan;
- [ ] signing/notarization/store provenance;
- [ ] final installed-package smoke tests.

### Store/publication

- [ ] current store screenshots/listing/privacy/data-safety metadata;
- [ ] live Google Play Health apps declaration for the exact production feature set;
- [ ] live Google Play Data safety for the exact production binary/SDK behavior;
- [ ] Apple privacy/store metadata for the exact production package;
- [ ] Microsoft/Partner Center privacy/store metadata where applicable;
- [ ] submission-day Apple/Google/Microsoft policy re-check as applicable;
- [ ] exact approved production source commit;
- [ ] immutable approved `v*` tag;
- [ ] tagged CI/CodeQL/dependency/store/release-gate/release-evidence success;
- [ ] final publication/store-approval evidence.

---

## 16. Current interpretation

- CareNest remains `1.0.0-rc.1`.
- The intended RC runtime feature scope remains source-complete.
- The Gumroad storefront remains strongly highlighted across repository/documentation surfaces only.
- Gumroad and Buy Me a Coffee remain absent from the packaged CareNest health app under the current policy.
- The latest **fully verified** implementation/source-policy baseline remains `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f` with **336/336 core tests**, all normal platform builds, all four store-candidate builds, and CodeQL green.
- Current `main` now contains additional verification-relevant release contracts, package-evidence tooling and workflow changes and therefore requires fresh exact-source verification before it can replace that baseline.
- A dated pre-submission Apple/Google/Microsoft policy review is recorded for 2026-08-18.
- Production validation still requires fresh exact-head automation, real-device, accessibility, packaged compatibility, signing, structured final-package evidence, live store-console metadata/policy re-check and publication evidence.
- CareNest is not yet production-signed, store-approved, production-published, manually proven on every target/device condition, or globally guaranteed bug-free.

Use `what_changed.md` for the exact active continuation and commit history.
