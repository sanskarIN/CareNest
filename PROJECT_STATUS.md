# CareNest — Current Project Status

**Date:** 2026-08-17  
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

The Ram Sandesh Gumroad storefront is now a first-class **repository/documentation** surface:

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

## 5. Latest verified Gumroad rollout automated baseline

Exact verified implementation/source-policy SHA:

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

The two-test increase from 173 to 175 UI/source-policy tests is the intended Gumroad repository-placement/accessibility/package-isolation expansion.

---

## 6. Gumroad verification correction history

The first finalization candidate `b5a57186af60e8b42bb917dfa85de24c3c9c1e9a` exposed one wording-contract mismatch in the newly added Gumroad test. Documentation correctly said Gumroad purchases “do not unlock” medical advice, while the test searched for singular `does not unlock`.

This false-positive test assertion was corrected without weakening the health-safety rule. The replacement exact source `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f` then passed the complete configured matrix listed above.

Documentation-only commits after that verified implementation source do not change the tested runtime/scanner behavior unless explicitly stated. Final repository-head workflow status should still be checked for the exact latest head.

---

## 7. Source-line and structured-file quality state

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

## 8. Strict XAML binding state

The application retains:

- `MauiEnableXamlCBindingWithSourceCompilation=true`;
- `MauiStrictXamlCompilation=true`;
- `XC0022`, `XC0023`, `XC0024`, `XC0025` as warnings-as-errors;
- typed binding-bearing pages/templates;
- no intended `NoWarn`/type-safety bypass for the compiled-binding policy.

Permanent historical verification for the compiled-binding migration remains:

`docs/releases/XAML_COMPILED_BINDINGS_VERIFICATION_20260816.md`

---

## 9. Security/dependency state

Current policy retains:

- CodeQL scanning;
- blocking dependency auditing;
- no restoration of the former exact SQLite advisory suppression merely to make audit green;
- privacy-aware logging contracts;
- encrypted imported-document and manual-backup protections;
- source/package external-commerce isolation contracts;
- release/store verification mechanisms.

A green source dependency graph does not prove packaged existing-data/encrypted-data upgrade compatibility; that remains a separate production gate.

---

## 10. Documentation state

Current documentation entry points:

- `README.md` — public project overview and highlighted Gumroad storefront;
- `docs/README.md` — documentation hub;
- `docs/DOCUMENTATION_CATALOG.md` — authority/ownership map;
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md` — whole-project reference;
- `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md` — verified Gumroad rollout evidence;
- `docs/DEVELOPER_REFERENCE.md` — developer rules;
- `docs/REPOSITORY_GOVERNANCE.md` — source/evidence/marketing governance;
- `GUMROAD.md` — canonical storefront guide;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — storefront/package policy;
- `docs/assets/README.md` — repository visual-asset rules;
- `SUPPORT.md` and `docs/SUPPORT_CARENEST.md` — support/storefront separation;
- `what_changed.md` — active detailed continuation record;
- `docs/history/` — immutable historical snapshots.

Dated historical evidence is not rewritten to retroactively insert current Gumroad promotion.

---

## 11. Production blockers still open

Automated source/build verification does not complete production release evidence.

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

### Signing/store/publication

- [ ] production Android signing identity/secrets outside Git;
- [ ] Apple signing/provisioning outside Git;
- [ ] Windows production signing identity outside Git;
- [ ] final production-signed packages;
- [ ] signed-package checksums/provenance;
- [ ] current store screenshots/listing/privacy/data-safety metadata;
- [ ] submission-time Apple/Google/Microsoft policy review as applicable;
- [ ] exact approved production source commit;
- [ ] immutable approved `v*` tag;
- [ ] tagged CI/CodeQL/dependency/store/release-gate evidence;
- [ ] final publication evidence.

---

## 12. Current interpretation

- CareNest remains `1.0.0-rc.1`.
- The intended RC feature scope remains source-complete.
- The Gumroad storefront is strongly highlighted across current repository/documentation surfaces.
- Gumroad and Buy Me a Coffee remain absent from the packaged CareNest health app under the current policy.
- The verified Gumroad rollout implementation/source-policy baseline is `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f` with **336/336 core tests**, all normal platform builds, all four store-candidate builds, and CodeQL green.
- Production validation still requires real-device, accessibility, packaged compatibility, signing, current store-policy and publication evidence.
- CareNest is not yet production-signed, store-approved, production-published, manually proven on every target/device condition, or globally guaranteed bug-free.

Use `what_changed.md` for the exact active Gumroad continuation and commit history.
