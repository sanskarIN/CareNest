# CareNest — Current Project Status

**Date:** 2026-08-18  
**Release line:** `1.0.0-rc.1`  
**Repository:** `https://github.com/sanskarIN/CareNest`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`  
**Latest accepted automated source:** `b6eecae66f74bd72bcb20d93508355542f9f3442`

The complete project status that was active before the Gumroad rollout is preserved exactly at:

`docs/history/pre-gumroad-rollout-20260817/PROJECT_STATUS.md`

Historical verification records remain authoritative only for their exact source boundaries. The current dynamic automated authority is:

`docs/releases/AUTOMATED_BASELINE.md`

The dated final-candidate verification record is:

`docs/releases/FINAL_CANDIDATE_VERIFICATION_20260818.md`

---

## 1. Current product boundary

CareNest is a local-first organizational health application built with .NET MAUI for Android, Windows, iOS/iPadOS and Mac Catalyst.

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

The source-controlled `1.0.0-rc.1` scope is source-complete for the intended RC feature set, including:

- multiple local person/family profiles;
- medicine records with user-entered strength/instruction text;
- explicit schedules and deterministic reminder occurrences;
- reminder lifecycle/history/status/reconciliation/compensation behavior;
- appointments and optional reminders;
- stock/refill organization;
- encrypted imported-document vault;
- password-encrypted manual backup/restore;
- optional local app lock;
- reports and explicit exports;
- privacy-aware diagnostics;
- light/dark/system themes;
- accessibility-oriented source contracts;
- strict compiled XAML bindings;
- automated C#/structured-file quality contracts;
- documentation-integrity tooling;
- package-evidence/provenance tooling;
- CodeQL, dependency, store and release gates.

No unresolved GitHub issue was present in the repository issue backlog at finalization time. That is not a claim that undiscovered bugs are impossible.

---

## 3. Repository/open-source completeness

The public repository includes the core open-source/community surfaces expected for this project:

- Apache License 2.0 `LICENSE`;
- `NOTICE`;
- public `README.md`;
- `CHANGELOG.md`;
- `CODE_OF_CONDUCT.md`;
- `CONTRIBUTING.md`;
- `SECURITY.md`;
- privacy, terms and support documentation;
- `.github/CODEOWNERS`;
- `.github/dependabot.yml`;
- bug-report issue form;
- feature-request issue form;
- issue-template configuration;
- pull-request template;
- GitHub funding metadata;
- CI, CodeQL, Dependency Audit, Store Package Configuration, Store Inspection Artifacts, Release Gate and Release Evidence workflows;
- setup, architecture, testing, security, privacy, accessibility, localization, packaging, release and governance documentation.

The final audit did not add decorative files simply to increase repository file count where an equivalent maintained surface already existed.

---

## 4. Gumroad and external-commerce package boundary

The Ram Sandesh Gumroad storefront remains a first-class **repository/documentation** surface:

**https://ramsandesh.gumroad.com**

Repository support/storefront promotion is separate from CareNest health functionality.

The distributed application source/package intentionally contains no external:

- `ramsandesh.gumroad.com` destination;
- `buymeacoffee.com/sanskarIN` destination;
- Gumroad/Buy Me a Coffee promotional runtime command;
- Gumroad/Buy Me a Coffee promotional XAML surface;
- repository promotional Gumroad badge in packaged app resources.

A purchase or contribution does not unlock diagnosis, dosage decisions, treatment recommendations, reminder priority/reliability, emergency assistance, clinical support, accounts/cloud services, or access to local health records.

The store-safe payload scanner defaults to both repository-only markers and checks regular payloads plus ZIP-compatible package entries using UTF-8/UTF-16 marker encodings. It fails closed for inspection errors.

---

## 5. Final accepted exact-source automated baseline

Frozen verified source:

`b6eecae66f74bd72bcb20d93508355542f9f3442`

Verification marker/head:

`ef1e8cea30108f1f3a4dca3158d9b862121e33fe`

Verification PR:

`#80` — closed without merge after success.

Observed CareNest CI run `32141539179`:

- repository Python tooling syntax: **success**;
- package-evidence self-test: **success**;
- documentation-link checker self-test: **success**;
- stable documentation local-link integrity: **success** — 182 live local links across 109 stable active Markdown files;
- platform-neutral formatting: **success**;
- unit tests: **122/122 passed**;
- integration tests: **39/39 passed**;
- UI/source-policy tests: **194/194 passed**;
- total core tests: **355/355 passed**;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**.

Store Package Configuration run `32141539246`:

- Android: **success**;
- Windows: **success**;
- iOS simulator: **success**;
- Mac Catalyst: **success**.

Store Inspection Artifacts run `32141539169`:

- scanner positive/negative/archive/fail-closed self-test: **success**;
- Android inspection artifact: **success**;
- Windows inspection artifact: **success**;
- Apple inspection artifacts: **success**.

CodeQL run `32141539253`: **success**.

Dependency Audit run `32141539349`: **success**.

Authoritative dated record:

`docs/releases/FINAL_CANDIDATE_VERIFICATION_20260818.md`

---

## 6. Failed verification checkpoints were fixed, not hidden

The accepted final baseline followed two superseded verification checkpoints:

- PR #78 exposed a documentation-link checker false positive on fenced example code. The checker was fixed to ignore fenced/inline/comment example-only links while preserving fail-closed checking for live local links.
- PR #79 reached 122/122 unit and 39/39 integration tests but UI/source-policy reported 192 passed and 2 failed. The two failures were stale verification contracts: one still required `actions/upload-artifact@v4` after the workflow moved to v7, and one raw Markdown substring assertion did not account for emphasis around the existing `not store approval` wording. The contracts were corrected without weakening the underlying safety/policy rules.

PR #80 then passed the complete required matrix. Superseded failures remain historical debugging evidence.

---

## 7. Source quality and regression protection

Current automated source/policy coverage includes checks for:

- unresolved merge-conflict markers;
- unfinished `TODO`/`FIXME`/`HACK` placeholders in runtime source;
- `NotImplementedException` placeholders;
- common sync-over-async forms;
- `Thread.Sleep`/`Task.WaitAll`/`Task.WaitAny` runtime patterns;
- `throw ex;` stack-trace destruction;
- malformed XAML/XML/project/JSON inputs;
- strict XAML compiled-binding requirements;
- package external-commerce isolation;
- package-evidence tooling behavior;
- release-documentation policy invariants;
- stable documentation local-link integrity.

The broad scanner does not classify every direct clock read as a generic defect; time semantics remain protected by more specific scheduling/time-zone tests.

---

## 8. Strict XAML binding state

The application retains:

- `MauiEnableXamlCBindingWithSourceCompilation=true`;
- `MauiStrictXamlCompilation=true`;
- `XC0022`, `XC0023`, `XC0024`, `XC0025` as warnings-as-errors;
- typed binding-bearing pages/templates;
- no intended warning/type-safety bypass for the compiled-binding policy.

Permanent migration evidence remains at:

`docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`

---

## 9. Structured final-package evidence tooling

Current source includes:

- `build/scripts/create-package-evidence.py`;
- `build/scripts/create-package-evidence.sh`;
- `build/scripts/create-package-evidence.ps1`;
- `build/scripts/test-create-package-evidence.py`;
- `build/scripts/verify-store-safe-payload.py`.

Guide:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

Production evidence mode fails closed unless it receives an immutable `v*` tag matching the recorded SHA, checked-out HEAD matches that SHA, tracked files are clean, real non-secret signing/notarization/store-managed provenance is supplied, the store-safe scanner passes, and evidence output is outside the payload being hashed.

The tool records deterministic SHA-256/package provenance evidence. It does not sign a package, verify a private signing identity by itself, submit to a store, or prove store approval.

---

## 10. Security/dependency state

Current source retains:

- CodeQL scanning;
- blocking unsuppressed dependency auditing;
- no restoration of the former SQLite advisory suppression merely to make audit green;
- privacy-minimized logging rules;
- encrypted imported-document protections;
- authenticated password-encrypted backup protections;
- app-lock privacy barrier protections;
- package external-commerce isolation;
- fail-closed package evidence generation;
- release/store workflow gates.

A green dependency graph does not prove packaged historical-data/encryption compatibility; those remain separate manual/package gates.

---

## 11. Store-policy state

A dated pre-submission review was completed on 2026-08-18:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

It covers current Apple, Google Play and Microsoft policy areas relevant to the product boundary, health claims, sensitive-data privacy and external commerce.

The conservative current decision remains: Gumroad and Buy Me a Coffee stay repository/documentation-only and outside the distributed health-app package.

The dated review is not store approval. Submission-day policies and live store-console declarations must still be re-checked against the exact signed production package/listing.

---

## 12. Documentation authority

Use these current entry points:

1. `docs/releases/AUTOMATED_BASELINE.md` — latest accepted dynamic automated baseline;
2. `docs/releases/FINAL_CANDIDATE_VERIFICATION_20260818.md` — final exact-head automated evidence;
3. `PROJECT_STATUS.md` — active product/release status;
4. `docs/releases/NEXT_STEPS.md` — remaining production work;
5. `README.md` — public project overview;
6. `docs/README.md` — documentation hub;
7. `docs/DOCUMENTATION_CATALOG.md` — documentation authority/ownership map;
8. `docs/COMPLETE_PROJECT_DOCUMENTATION.md` — end-to-end project reference;
9. `docs/releases/RELEASE_EVIDENCE.md` — production evidence contract;
10. `docs/releases/PACKAGED_RELEASE_VALIDATION.md` — package/manual validation runbook;
11. `docs/releases/STORE_SUBMISSION_CHECKLIST.md` — store submission checklist;
12. `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — package evidence guide;
13. `docs/releases/STORE_POLICY_REVIEW_20260818.md` — preliminary store-policy review;
14. `what_changed.md` — detailed continuation/handoff record;
15. `docs/history/` — preserved historical snapshots.

Stable documentation was finalized before the exact source freeze. Dynamic status/evidence files can record completed runs without creating an executable verification loop.

---

## 13. Production blockers still open

The in-repository source/tooling/documentation and automated verification work is complete for the current RC1 candidate. Production promotion still requires evidence that cannot be fabricated from source CI alone.

### Real-device/platform behavior

- [ ] representative Android device/emulator matrix;
- [ ] Android notification permission denied/granted behavior;
- [ ] actual Android reminder delivery/cancellation/snooze behavior;
- [ ] exact/inexact alarm and battery/vendor behavior;
- [ ] Android reboot/restart/clock/time-zone/DST recovery;
- [ ] Windows installed/manual reminder and lifecycle behavior;
- [ ] real iPhone/iPad notification permission/delivery/recovery;
- [ ] Mac Catalyst manual notification/lifecycle behavior.

### Packaged data/encryption compatibility

Use fictional/synthetic data only.

- [ ] representative existing-data packaged upgrade/install;
- [ ] SQLite integrity/readability/editability after upgrade;
- [ ] reminder reconciliation after packaged upgrade;
- [ ] packaged encrypted-document compatibility;
- [ ] packaged encrypted-backup create/restore/wrong-password/tamper/truncation/trailing-data validation;
- [ ] genuine historical encrypted fixtures where genuine prior bytes actually exist and can be tested safely.

### Accessibility

- [ ] representative screen-reader testing;
- [ ] large-text/text-scaling validation;
- [ ] keyboard/focus validation on desktop targets;
- [ ] light/dark/system contrast validation;
- [ ] reduced-motion validation;
- [ ] color-independent state verification.

### Signing/final package evidence

- [ ] production Android signing outside Git;
- [ ] Apple signing/provisioning outside Git;
- [ ] Windows production signing outside Git where applicable;
- [ ] final signed/notarized production artifacts;
- [ ] `--stage production` package evidence JSON for every final artifact;
- [ ] independent package SHA-256/provenance cross-check;
- [ ] final signed-package BMC marker scan;
- [ ] final signed-package Gumroad marker scan;
- [ ] installed-package smoke tests.

### Store/publication

- [ ] final store screenshots/listing/privacy/data-safety metadata;
- [ ] live Google Play Health apps declaration;
- [ ] live Google Play Data safety answers;
- [ ] Apple privacy/store metadata;
- [ ] Microsoft/Partner Center privacy/store metadata where applicable;
- [ ] submission-day Apple/Google/Microsoft policy re-check;
- [ ] exact approved production source/tag decision;
- [ ] immutable approved `v*` production tag;
- [ ] tagged CI/CodeQL/Dependency Audit/Store Package/Store Inspection/Release Gate/Release Evidence success;
- [ ] final store submission/approval/publication evidence.

---

## 14. Current interpretation

- CareNest remains `1.0.0-rc.1`.
- The intended RC runtime feature scope is source-complete.
- The repository/open-source documentation and community surfaces have been audited and no material missing standard repository file was identified in the final pass.
- No open GitHub issues were present at finalization time.
- Gumroad and Buy Me a Coffee remain repository/documentation-only and excluded from the distributed health-app package under the current policy.
- The accepted exact automated source is `b6eecae66f74bd72bcb20d93508355542f9f3442` with **355/355 core tests** and the recorded platform/store/security matrix green.
- PR #80 is closed without merge; its verification marker did not enter `main`.
- The remaining work is production validation requiring actual packages/devices/signing/store accounts and current live policy/store evidence.
- CareNest is not claimed globally bug-free, medically authoritative, production-signed, store-approved or production-published merely because automated verification is green.

Use `what_changed.md` for the detailed final continuation history.