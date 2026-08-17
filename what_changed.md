# CareNest — Active Completion Handoff

**Date:** 2026-08-17  
**Release line:** `1.0.0-rc.1`  
**Repository:** `sanskarIN/CareNest`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`  
**Continuation focus:** complete Gumroad rollout, repository branding, package isolation, current-documentation refresh, and exact-source verification preparation

The complete active handoff from before the Gumroad rollout is preserved exactly at:

`docs/history/pre-gumroad-rollout-20260817/what_changed.md`

Other major active documents replaced during this continuation were also preserved under the same history directory before being modernized.

---

## 1. What this continuation completed

The Ram Sandesh Gumroad storefront is now a first-class **repository/documentation** surface while remaining outside the CareNest health-application package under the current store/product policy.

Canonical URL:

**https://ramsandesh.gumroad.com**

The continuation completed:

- repository Gumroad branding;
- prominent Gumroad links across current support/documentation/marketing entry points;
- GitHub custom repository metadata;
- canonical storefront documentation;
- marketing/compliance documentation;
- repository visual-asset documentation;
- no-medical-entitlement and no-health-data-transfer language;
- Gumroad runtime/package exclusion contracts;
- package scanner support for both Gumroad and Buy Me a Coffee markers;
- source-line/structured-file quality documentation integration;
- active project status modernization;
- complete project documentation modernization;
- contributor/developer/governance/configuration/testing documentation modernization;
- getting-started/FAQ/limitations/support modernization;
- release next-steps/store-policy/package-validation/executable-checklist modernization;
- canonical executable-build guide modernization;
- exact preservation of superseded major active documentation.

---

## 2. Repository-only Gumroad branding

Added:

`docs/assets/gumroad_store_badge.svg`

The badge:

- displays the exact canonical storefront URL;
- has custom storefront/shopping artwork;
- includes SVG `<title>` and `<desc>` accessibility metadata;
- is documented for Markdown/HTML usage;
- is intentionally repository-only;
- is intentionally absent from `src/CareNest.App/Resources`.

Supporting asset documentation:

`docs/assets/README.md`

The generated chat concept was translated into this maintainable source-controlled SVG rather than silently placing a promotional raster asset inside the CareNest application package.

---

## 3. Canonical storefront documentation

Added:

- `GUMROAD.md`;
- `docs/marketing/README.md`;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`;
- `docs/marketing/GUMROAD_ROLLOUT_CHECKLIST.md`.

These documents define:

- canonical URL usage;
- repository placement;
- accessible image-link usage;
- plain-text link fallback;
- health-feature separation;
- no health-data transfer to Gumroad by CareNest;
- package/runtime exclusion;
- maintainer review/checklist rules.

---

## 4. Highlighted current repository surfaces

Gumroad is now highlighted or explicitly documented across current surfaces including:

- `README.md`;
- `.github/FUNDING.yml`;
- `SUPPORT.md`;
- `BUY_ME_A_COFFEE.md`;
- `GUMROAD.md`;
- `CONTRIBUTING.md`;
- `PROJECT_STATUS.md`;
- `CHANGELOG.md`;
- `docs/README.md`;
- `docs/GETTING_STARTED.md`;
- `docs/USER_FAQ.md`;
- `docs/KNOWN_LIMITATIONS.md`;
- `docs/SUPPORT_CARENEST.md`;
- `docs/DOCUMENTATION_CATALOG.md`;
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`;
- `docs/DEVELOPER_REFERENCE.md`;
- `docs/CONFIGURATION_REFERENCE.md`;
- `docs/REPOSITORY_GOVERNANCE.md`;
- `docs/testing/TESTING_GUIDE.md`;
- `docs/releases/NEXT_STEPS.md`;
- `docs/releases/STORE_BUILD_POLICY.md`;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`;
- `docs/releases/EXECUTABLE_BUILD_CHECKLIST.md`;
- `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`;
- repository marketing/asset documentation.

Historical dated evidence is not rewritten merely to insert newer marketing links.

---

## 5. Application/package external-commerce boundary

Current CareNest runtime/package intentionally contains no external:

- `ramsandesh.gumroad.com` destination;
- `buymeacoffee.com/sanskarIN` destination;
- Gumroad/BMC promotional runtime card;
- Gumroad/BMC promotional runtime command;
- Gumroad repository promotional artwork in app resources;
- shared runtime external-commerce URL constant.

This keeps repository promotion separate from a health-organizer application package.

A Gumroad purchase or financial contribution does not unlock or modify:

- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- clinical interaction/risk behavior;
- reminder priority/reliability;
- emergency assistance;
- clinical support entitlement;
- CareNest account/cloud behavior;
- user health-data access.

CareNest does not automatically transmit local health records to Gumroad or Buy Me a Coffee.

---

## 6. Store-safe payload scanner upgrade

Updated:

`build/scripts/verify-store-safe-payload.py`

Default repository-only markers are now:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

The scanner continues to inspect:

- UTF-8;
- UTF-16 little-endian;
- UTF-16 big-endian;
- regular payload files;
- ZIP-compatible entries such as AAB contents.

It continues to fail closed when inspection cannot be performed.

`--forbidden` is repeatable for explicit additional marker lists.

---

## 7. Gumroad regression coverage

Updated:

`tests/CareNest.UiTests/FundingLinkContractTests.cs`

It now protects:

- repository Buy Me a Coffee visibility;
- repository Gumroad visibility;
- Gumroad presence in required support/metadata/canonical documentation;
- no Gumroad/BMC About runtime surface;
- no purchase/funding medical entitlement;
- Gumroad SVG accessibility metadata;
- Gumroad badge absence from app resources.

Updated:

`tests/CareNest.UiTests/StoreFundingPayloadContractTests.cs`

It now protects:

- Gumroad/BMC absence from text-like application runtime source;
- external-commerce URL absence from shared runtime constants;
- obsolete external-commerce build-switch absence;
- both default package-scanner markers;
- UTF-8/UTF-16 scan behavior;
- ZIP/AAB scan behavior;
- scanner fail-closed behavior.

The Gumroad rollout adds independent UI/source-policy test coverage. Do not assume the old 173 UI-test total remains the final current total until CI reports the exact final revision.

---

## 8. Source-line and structured-file quality coverage retained

The runtime source quality contract continues to report file/line failures for known defect patterns including:

- unresolved merge markers;
- `TODO`/`FIXME`/`HACK` placeholders;
- `NotImplementedException`;
- common sync-over-async forms;
- `Thread.Sleep`;
- `Task.WaitAll`/`Task.WaitAny`;
- `throw ex;`.

Structured runtime inputs including XAML, project/XML-family files and JSON are parsed for syntax validity.

The generic audit intentionally does not ban every clock read; date/time correctness belongs to specific semantic/time-zone tests.

---

## 9. Latest fully verified baseline before this rollout

Exact source:

`7cbe5568b6cffa06c279b29f3cb1b107ea988791`

That exact revision passed:

- 122/122 unit tests;
- 39/39 integration tests;
- 173/173 UI/source-policy tests;
- **334/334 total core tests**;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- all four store-candidate configurations;
- CodeQL.

This remains the authoritative automated baseline until the exact final Gumroad rollout source completes its own workflow matrix.

---

## 10. Major documentation preserved exactly before replacement

Exact pre-Gumroad versions were preserved under:

`docs/history/pre-gumroad-rollout-20260817/`

Preserved major active files include:

- `what_changed.md`;
- `PROJECT_STATUS.md`;
- `COMPLETE_PROJECT_DOCUMENTATION.md`;
- `CHANGELOG.md`;
- `CONFIGURATION_REFERENCE.md`;
- `NEXT_STEPS.md`;
- `EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md`.

Earlier dated release/history evidence remains untouched.

---

## 11. Current documentation authority map

Use:

1. `PROJECT_STATUS.md` — active product/release state;
2. `docs/releases/NEXT_STEPS.md` — remaining operational work;
3. latest exact-source verification record — automated evidence;
4. `docs/COMPLETE_PROJECT_DOCUMENTATION.md` — complete current project reference;
5. `docs/EXECUTABLE_BUILD_AND_PACKAGING_GUIDE.md` — executable/package build guide;
6. `docs/releases/STORE_BUILD_POLICY.md` — current store/package external-commerce boundary;
7. `docs/releases/PACKAGED_RELEASE_VALIDATION.md` — package/manual evidence runbook;
8. `GUMROAD.md` — canonical storefront guide;
9. `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md` — storefront placement/package policy;
10. this `what_changed.md` — active continuation record;
11. `docs/history/` — immutable prior snapshots.

---

## 12. Focused commits created in this continuation

### Initial Gumroad rollout

1. `b9a9c2b8849e17fd32914b23290113314ea91586` — `assets: add Gumroad storefront badge`
2. `87da5df5be4d3a9885812747fbd85be083b63e68` — `docs: add Gumroad storefront guide`
3. `6139d5a8a6531817fc9b6305d1f912d570ba8340` — `docs: define Gumroad placement policy`
4. `3e682fe11b110dd0daeb8a8bd71a0613d229f460` — `meta: add Gumroad to repository links`
5. `b4af3f78dfb340502d49ca1933531dd0e9ec0a15` — `docs: highlight Gumroad in support guide`
6. `55a1e782fda4ddda1cc0bf91190ce8b126ea18ec` — `docs: feature Gumroad across main README`
7. `d5de414f28222b45bbd995f263f9f71588aa46a7` — `docs: cross-link Gumroad from support page`
8. `549b5a569732a7cce42e3fd270b61744bc4c36fc` — `build: scan Gumroad from store payloads`
9. `dfdcad96a1f1e498a692a342cf9fd2f0d11f4db6` — `test: enforce repository-only Gumroad placement`
10. `30623f4c81e45a483a8f40d05f5abb4dece75af6` — `test: protect store payload from Gumroad marker`
11. `ae908dc94c4ee5c63d48f9eb3d915db626f51bf6` — `docs: highlight Gumroad in documentation hub`
12. `5738a09ffb299d12b25d1e52c75d581827ebea55` — `docs: catalog Gumroad branding and package policy`
13. `a12e23361595d1427c6ae160bc56636bd1e56f1d` — `docs: document repository branding assets`
14. `489a424de434ddda0e203746dd58ddd035ef581c` — `docs: add marketing documentation hub`
15. `6508cdb39d24cb7aa5c5ffb944089b40aff9e6f4` — `docs: add Gumroad rollout checklist`
16. `fdf744db7bfe71b56d6b1b84f1308d1b44981dd1` — `docs: preserve pre-Gumroad handoff`
17. `9e18b774aba959b4b7fe02eb0d6077d5809dd8df` — `docs: record complete Gumroad rollout handoff`

### Complete current-documentation refresh

18. `0a4a764b217f87a9ca5422a43ea599e80b09d8f6` — `docs: add Gumroad rules to contributor guide`
19. `7c55f93167abea3785c068865dc2dd40d02c3c66` — `docs: refresh developer reference for Gumroad boundary`
20. `41078500206a21c3286bbfa4780dba56a8333dab` — `docs: govern Gumroad and current verification boundaries`
21. `eac2ca32440be3e362f920dca842cc6079f55344` — `docs: highlight Gumroad in CareNest support page`
22. `5735c303825bce8f0fbd739bd853045dc25e6fb3` — `docs: preserve pre-Gumroad project status`
23. `81ad1a9d595d6c4fa88d0e266220c7bb8ec611ea` — `docs: refresh active project status for Gumroad rollout`
24. `b75dd36984f610995ae5748e8386207cceba9f39` — `docs: preserve pre-Gumroad complete project guide`
25. `c24dd1f5fb5e788655739dfa49c657dc62e46f38` — `docs: rebuild complete project documentation for Gumroad rollout`
26. `5a4a9b522533239aa548a28d6bf3d2fce3178e53` — `docs: preserve pre-Gumroad changelog`
27. `ef64e9d8d06c4e34ba919b8ee6dba860e00711ef` — `docs: record Gumroad rollout in active changelog`
28. `eabd374801859a691fbfa07b75d673b4006c0365` — `docs: refresh getting started for Gumroad rollout`
29. `8693061e99c69559ca9a85fd902b529f6fd13670` — `docs: add Gumroad and current baseline to user FAQ`
30. `d5290decb75a55bebcc65c8fde816fa7a7732501` — `docs: document Gumroad and current RC limitations`
31. `b4b6c299340ea5bac617218f60f60edd9b4c4f92` — `docs: preserve pre-Gumroad configuration reference`
32. `cd507906e305f02d1ab6842d0d80bcdb8b218422` — `docs: rebuild configuration reference for Gumroad isolation`
33. `610c5b8a914781c617229e8061ad90d34e165ef3` — `docs: refresh testing guide for Gumroad contracts`
34. `74ca0f03acc2fcd9a4f3fffb538b767b354df096` — `docs: preserve pre-Gumroad next steps`
35. `0ceb2945f4db58c42683e8cae44e1dac3983f7f3` — `docs: refresh next steps after Gumroad rollout`
36. `eeb9890cb9f27ebaf7b98cc83e30f3ed1d0ff2ce` — `docs: extend store policy to Gumroad isolation`
37. `559b54f373bb09cb3e0cacbe0b239eaebbe17003` — `docs: validate Gumroad-free production packages`
38. `61435805eeb0cedc0c98ff241496f224e5cbe90e` — `docs: add Gumroad checks to executable checklist`
39. `74fe8da904c4f929d9b3077725c54ed5e106ebf4` — `docs: preserve pre-Gumroad executable guide`
40. `b80c902f1a2ca178c168ec747c654d5b775ec583` — `docs: rebuild executable guide for Gumroad-safe packages`

This handoff update is the final planned content change before exact-source workflow verification. Use the actual resulting `main` SHA from Git/GitHub Actions as the verification target rather than assuming the handoff can know its own commit SHA before creation.

---

## 13. Exact-source verification required now

Only the workflows associated with the exact final handoff revision are authoritative for this continuation.

Required current gates:

- formatting;
- 122-unit-test suite;
- 39-integration-test suite;
- current UI/source-policy suite including new Gumroad contracts;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- all four store-candidate configurations;
- CodeQL.

Store Inspection Artifacts runs on its configured triggers rather than every ordinary `main` push. Its source wiring/scanner behavior remains protected by source-policy tests, and production/tag/inspection runs must exercise both default external-commerce markers.

If any exact-final-source gate fails, fix the real issue in the smallest correct commit and rerun the resulting new exact source. Do not suppress legitimate failures.

---

## 14. Real production work still remaining after automation

Even a completely green final workflow set does not finish production release evidence.

Still open:

- representative Android real-device/emulator behavior;
- real notification permission/delivery/cancellation/snooze behavior;
- Android alarm/battery/vendor/reboot/time-zone/DST behavior;
- Windows installed/lifecycle/reminder behavior;
- real iPhone/iPad notification behavior;
- Mac Catalyst manual/signed behavior;
- packaged existing-data SQLite upgrade compatibility;
- packaged encrypted-document compatibility;
- packaged encrypted-backup create/restore/wrong-password/tamper/truncation/trailing-data behavior;
- genuine historical encrypted fixtures where genuine prior bytes safely exist;
- screen-reader/large-text/keyboard/focus/contrast/reduced-motion validation;
- production signing identities outside Git;
- final signed-package Gumroad/BMC payload scans;
- final signed-package checksums/provenance;
- current Apple/Google/Microsoft store policy review as applicable;
- store metadata/screenshots/privacy/data-safety declarations;
- exact approved immutable production source/tag;
- tagged CI/security/dependency/store/release evidence;
- final publication evidence.

---

## 15. Continuation rule after this pass

After the exact final Gumroad/documentation source is green, the next meaningful CareNest work is **production validation**, not another broad speculative source refactor.

If real manual/package/security/accessibility testing finds a defect:

1. reproduce it safely with synthetic data;
2. fix the smallest correct source boundary;
3. add the lowest appropriate regression coverage;
4. run the full applicable exact-source matrix again;
5. rebuild/retest the affected final package;
6. update current evidence only after results are known.

Current project interpretation: **CareNest `1.0.0-rc.1` remains source-complete for its intended RC scope, now with strongly highlighted repository-only Gumroad promotion, explicit Gumroad/BMC package exclusion, refreshed complete current documentation, and final exact-source automation pending before production validation.**
