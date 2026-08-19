# CareNest — Current Project Status

**Date:** 2026-08-19  
**Release line:** `1.0.0-rc.1`  
**Repository:** `https://github.com/sanskarIN/CareNest`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`  
**Latest accepted automated source:** `30ee6c265104c64ec5a1a4013f592f7f058750e8`  
**Merged source commit:** `2549c08b25145f20c59b7e73ca227c35d5bbf0ec`

The complete project status that was active before the Gumroad rollout is preserved exactly at:

`docs/history/pre-gumroad-rollout-20260817/PROJECT_STATUS.md`

Historical verification records remain authoritative only for their exact source boundaries. The current dynamic automated authority is:

`docs/releases/AUTOMATED_BASELINE.md`

The dated final-candidate verification record from the immediately previous accepted source remains:

`docs/releases/FINAL_CANDIDATE_VERIFICATION_20260818.md`

The current backup hardening record frozen in the newly verified source is:

`docs/releases/BACKUP_RESOURCE_HARDENING_20260819.md`

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
- bounded authenticated backup archive/decrypted-container processing;
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

No unresolved GitHub issue was present in the repository issue backlog when the 2026-08-19 continuation began. That is not a claim that undiscovered bugs are impossible.

The 2026-08-19 continuation did not add speculative feature scope merely to increase commit count. A real backup resource-exhaustion gap was identified, hardened, regression-tested and verified before merge.

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

The continuation audit did not add decorative files simply to increase repository file count where an equivalent maintained surface already existed.

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

## 5. Current accepted exact-source automated baseline

Frozen verified source:

`30ee6c265104c64ec5a1a4013f592f7f058750e8`

GitHub PR merge-ref tested by the pull-request workflows:

`84fda5bb8ced9f4c487110e43652f51ba2d8d495`

Merged `main` commit preserving all 19 PR commits:

`2549c08b25145f20c59b7e73ca227c35d5bbf0ec`

Verification/merge PR:

`#81` — merged after complete success.

CareNest CI run `32205946013`:

- repository Python tooling syntax: **success**;
- package-evidence self-test: **success**;
- documentation-link checker self-test: **success**;
- stable documentation local-link integrity: **success** — 182 live local links across 111 stable active Markdown files;
- platform-neutral formatting: **success**;
- unit tests: **122/122 passed**;
- integration tests: **54/54 passed**;
- UI/source-policy tests: **194/194 passed**;
- total core tests: **370/370 passed**;
- Android Release: **success**;
- Windows Release: **success**;
- iOS simulator Release: **success**;
- Mac Catalyst Release: **success**.

Store Package Configuration run `32205946003`:

- Android: **success**;
- Windows: **success**;
- iOS simulator: **success**;
- Mac Catalyst: **success**.

Store Inspection Artifacts run `32205946001`:

- scanner positive/negative/archive/fail-closed self-test: **success**;
- Android inspection artifact: **success**;
- Windows inspection artifact: **success**;
- Apple inspection artifacts: **success**.

CodeQL run `32205946030`: **success**.

Dependency Audit run `32205946026`: **success**.

Dynamic authority:

`docs/releases/AUTOMATED_BASELINE.md`

Hardening record frozen into the verified source:

`docs/releases/BACKUP_RESOURCE_HARDENING_20260819.md`

---

## 6. Backup resource-hardening state

The 2026-08-19 audit found a concrete availability/resource-exhaustion gap: a deliberately crafted password-valid backup could still force excessive decrypted ZIP or uncompressed ZIP resource use before ordinary topology/extraction validation completed.

The accepted source now fails closed across the relevant layers.

Current default ceilings are:

- decrypted ZIP container: **2304 MiB** maximum;
- manifest: **1 MiB** maximum;
- SQLite database: **1 GiB** maximum;
- each encrypted document: **512 MiB** maximum;
- total uncompressed ZIP payload: **2 GiB** maximum;
- documents: **5,000** maximum;
- archive-entry count: document limit plus the fixed required-entry allowance;
- directory-only ZIP entries: rejected.

The hardening also:

- enforces a backup-specific plaintext ceiling while the authenticated encrypted payload is being decrypted, before ZIP parsing;
- validates archive count/resource properties before manifest deserialization;
- preserves strict topology/database/key/document validation before extraction;
- validates newly created backups against the same current restore resource/topology boundary before encryption;
- rejects unsafe configured document-count ceilings that would overflow the archive-entry allowance;
- retains legacy encrypted-stream v1 readability while respecting a caller-supplied plaintext limit;
- leaves other `ChunkedAead` callers unchanged when no maximum is supplied.

The branch added 15 focused integration tests. The integration suite advanced from 39 to 54 tests and the total core inventory advanced from 355 to 370 without reducing existing unit/UI/source-policy coverage.

---

## 7. Failed verification checkpoints are fixed, not hidden

The accepted pre-hardening baseline was:

`b6eecae66f74bd72bcb20d93508355542f9f3442`

with 122 unit + 39 integration + 194 UI/source-policy = **355/355** and all required platform/store/security workflows green.

The current hardening was developed on PR #81 with multiple intermediate heads. Superseded workflow runs were allowed to cancel under repository concurrency policy instead of being treated as final evidence.

The final frozen head `30ee6c265104c64ec5a1a4013f592f7f058750e8` then passed the complete required matrix. No valid test/security failure was suppressed to reach the green state.

Earlier historical PR #78/#79/#80 verification history remains preserved in the existing dated records.

---

## 8. Source quality and regression protection

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
- stable documentation local-link integrity;
- backup archive topology/resource limits;
- bounded encrypted-stream decryption behavior;
- legacy/current encrypted framing compatibility under caller-provided limits.

The broad scanner does not classify every direct clock read as a generic defect; time semantics remain protected by more specific scheduling/time-zone tests.

---

## 9. Strict XAML binding state

The application retains:

- `MauiEnableXamlCBindingWithSourceCompilation=true`;
- `MauiStrictXamlCompilation=true`;
- `XC0022`, `XC0023`, `XC0024`, `XC0025` as warnings-as-errors;
- typed binding-bearing pages/templates;
- no intended warning/type-safety bypass for the compiled-binding policy.

Permanent migration evidence remains at:

`docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`

---

## 10. Structured final-package evidence tooling

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

## 11. Security/dependency state

Current source retains:

- CodeQL scanning;
- blocking unsuppressed dependency auditing;
- no restoration of the former SQLite advisory suppression merely to make audit green;
- privacy-minimized logging rules;
- encrypted imported-document protections;
- authenticated password-encrypted backup protections;
- bounded decrypted-backup and archive-resource handling;
- app-lock privacy barrier protections;
- package external-commerce isolation;
- fail-closed package evidence generation;
- release/store workflow gates.

A green dependency graph or green backup unit/integration test does not prove packaged historical-data/encryption compatibility; those remain separate manual/package gates.

---

## 12. Store-policy state

A dated pre-submission review was completed on 2026-08-18:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

It covers current Apple, Google Play and Microsoft policy areas relevant to the product boundary, health claims, sensitive-data privacy and external commerce.

The conservative current decision remains: Gumroad and Buy Me a Coffee stay repository/documentation-only and outside the distributed health-app package.

The dated review is not store approval. Submission-day policies and live store-console declarations must still be re-checked against the exact signed production package/listing.

---

## 13. Documentation authority

Use these current entry points:

1. `docs/releases/AUTOMATED_BASELINE.md` — latest accepted dynamic automated baseline;
2. `docs/releases/BACKUP_RESOURCE_HARDENING_20260819.md` — current backup hardening design/evidence record frozen in verified source;
3. `docs/releases/FINAL_CANDIDATE_VERIFICATION_20260818.md` — immediately previous exact-head verification record;
4. `PROJECT_STATUS.md` — active product/release status;
5. `docs/releases/NEXT_STEPS.md` — remaining production work;
6. `README.md` — public project overview;
7. `docs/README.md` — documentation hub;
8. `docs/DOCUMENTATION_CATALOG.md` — documentation authority/ownership map;
9. `docs/COMPLETE_PROJECT_DOCUMENTATION.md` — end-to-end project reference;
10. `docs/releases/RELEASE_EVIDENCE.md` — production evidence contract;
11. `docs/releases/PACKAGED_RELEASE_VALIDATION.md` — package/manual validation runbook;
12. `docs/releases/STORE_SUBMISSION_CHECKLIST.md` — store submission checklist;
13. `docs/releases/PACKAGE_EVIDENCE_TOOLING.md` — package evidence guide;
14. `docs/releases/STORE_POLICY_REVIEW_20260818.md` — preliminary store-policy review;
15. `what_changed.md` — detailed continuation/handoff record;
16. `docs/history/` — preserved historical snapshots.

Stable documentation was finalized before the exact source freeze. Designated dynamic status/evidence files can record completed runs without creating an executable verification loop.

---

## 14. Production blockers still open

The in-repository runtime/source/tooling/documentation and automated verification work is complete for the current RC1 candidate. Production promotion still requires evidence that cannot be fabricated from source CI alone.

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
- [ ] packaged encrypted-backup create/inspect/restore/wrong-password/tamper/truncation/trailing-data validation;
- [ ] representative normal packaged backups remain comfortably below current resource ceilings;
- [ ] genuine historical encrypted fixtures where genuine prior bytes actually exist and can be tested safely;
- [ ] any genuine historical backup exceeding a current resource ceiling is recorded as a compatibility/security decision rather than silently bypassed.

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

## 15. Current interpretation

- CareNest remains `1.0.0-rc.1`.
- The intended RC runtime feature scope is source-complete.
- A concrete backup resource-exhaustion gap discovered during the 2026-08-19 continuation is fixed and verified rather than left as a speculative follow-up.
- The repository/open-source documentation and community surfaces have been audited; no reason was found to add decorative/redundant files merely to inflate the repository.
- No open GitHub issues were present when this continuation began.
- Gumroad and Buy Me a Coffee remain repository/documentation-only and excluded from the distributed health-app package under the current policy.
- The accepted exact automated source is `30ee6c265104c64ec5a1a4013f592f7f058750e8` with **370/370 core tests** and the recorded platform/store/security matrix green.
- PR #81 is merged via merge commit `2549c08b25145f20c59b7e73ca227c35d5bbf0ec`, preserving all 19 meaningful branch commits.
- Post-verification commits to the four designated dynamic evidence/status files do not redefine the frozen executable source baseline.
- The remaining work is production validation requiring actual packages/devices/signing/store accounts and current live policy/store evidence.
- CareNest is not claimed globally bug-free, medically authoritative, production-signed, store-approved or production-published merely because automated verification is green.

Use `what_changed.md` for the detailed continuation history.
