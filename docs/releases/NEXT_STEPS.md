# CareNest Next Steps

**Date:** 2026-08-18  
**Release line:** `1.0.0-rc.1`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`

The complete active checklist from before the Gumroad rollout is preserved exactly at:

`docs/history/pre-gumroad-rollout-20260817/NEXT_STEPS.md`

This file tracks only the work that remains after the source-line quality hardening, complete documentation refresh, repository-first Gumroad rollout, successful exact-source Gumroad rollout verification, the dated 2026-08-18 preliminary store-policy review, and the current package-evidence/release-governance engineering pass.

---

## 1. Latest fully verified automated boundary

Latest fully verified Gumroad implementation/source-policy source:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

That exact revision passed:

- 122/122 unit tests;
- 39/39 integration tests;
- 175/175 UI/source-policy tests;
- **336/336 total core tests**;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- Android/Windows/iOS-simulator/Mac-Catalyst store-candidate configurations;
- CodeQL.

Authoritative evidence:

`docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`

The current repository head contains newer verification-relevant tests, build scripts and workflow/release-policy changes. Therefore the 336-test result belongs only to the exact source above until a newer exact-source matrix is actually completed and recorded.

Do not predict a newer test total from source inspection.

---

## 2. Source/product work complete before this continuation

Do not repeat these implementation passes unless a real defect or changed requirement is discovered:

- [x] local-first/account-free RC1 product scope;
- [x] profile/medicine/schedule/log/appointment/document/report/settings/app-lock source scope;
- [x] deterministic reminder planning/reconciliation/compensation hardening;
- [x] encrypted document and password-encrypted backup protections;
- [x] SQLite transactional/migration/dependency-security hardening;
- [x] strict compiled XAML binding enforcement;
- [x] `XC0022`–`XC0025` promoted to errors;
- [x] repository-wide runtime C# line-level defect-pattern audit;
- [x] structured XAML/XML/project/JSON syntax validation contract;
- [x] CodeQL/dependency/release/store workflow policies;
- [x] external Buy Me a Coffee application-package removal;
- [x] repository-first Gumroad storefront documentation and branding;
- [x] canonical Gumroad URL surfaced across current repository support/documentation/marketing surfaces;
- [x] repository-only Gumroad SVG badge with accessibility metadata;
- [x] Gumroad/BMC no-medical-entitlement documentation;
- [x] Gumroad/BMC absence from CareNest runtime/application resources;
- [x] store-payload scanner upgraded to reject both external-commerce markers;
- [x] Gumroad repository-placement/package-isolation regression contracts;
- [x] current documentation catalog/status/governance/developer/testing/configuration/FAQ/limitations/changelog refresh;
- [x] superseded major active documents preserved exactly in `docs/history/pre-gumroad-rollout-20260817/`;
- [x] exact-source Gumroad rollout verification completed and recorded;
- [x] dated preliminary Apple/Google/Microsoft store-policy review completed on 2026-08-18;
- [x] stale release-policy/checklist/evidence references corrected to the verified Gumroad baseline.

---

## 3. Current package-evidence/release-governance engineering — source work complete, verification pending

Implemented in current `main`:

- [x] release-documentation consistency regression tests;
- [x] package-evidence tooling regression tests;
- [x] `build/scripts/create-package-evidence.py`;
- [x] Bash package-evidence wrapper;
- [x] PowerShell package-evidence wrapper;
- [x] synthetic package-evidence self-test;
- [x] CareNest CI Python syntax verification for package evidence tooling;
- [x] CareNest CI synthetic package-evidence self-test;
- [x] Release Gate package-evidence syntax/self-test;
- [x] Release Gate current evidence/tooling presence checks;
- [x] Release Evidence package-tooling self-test capture and aggregate failure handling;
- [x] structured final-package SHA-256/provenance guide;
- [x] release process/checklist/security/manual/package-validation documentation integration;
- [x] exact-head verification protocol updated to classify these changes as verification-relevant.

This source work is **not yet a replacement verified baseline** merely because it is committed.

---

# Priority 0 — immediate automated gate for the new current source

## 4. Freeze and verify the new exact source

Because current `main` now includes tests, build scripts and workflow changes after `94e867...`, fresh exact-source automation is required before further production promotion.

- [ ] finish all intended verification-relevant source/doc-contract changes in this continuation;
- [ ] record the exact final `main` SHA;
- [ ] create a marker-only verification branch/PR from that exact SHA according to `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md` when PR-triggered evidence is required;
- [ ] verify package-evidence Python syntax/self-test;
- [ ] verify platform-neutral formatting;
- [ ] record actual unit-test count/result;
- [ ] record actual integration-test count/result;
- [ ] record actual UI/source-policy test count/result;
- [ ] record actual total core-test count/result;
- [ ] verify Android Release;
- [ ] verify Windows Release;
- [ ] verify iOS simulator Release;
- [ ] verify Mac Catalyst Release;
- [ ] verify Store Package Configuration on all four targets;
- [ ] verify Store Inspection Artifacts;
- [ ] verify CodeQL;
- [ ] verify unsuppressed Dependency Audit;
- [ ] record exact workflow/run IDs and source SHA;
- [ ] close marker-only PR without merge where that protocol is used;
- [ ] promote the newer baseline in current docs only after the complete matrix is successful.

If any gate fails, preserve the failure, fix the actual defect on `main`, and verify a new exact source. Do not weaken the gate to preserve a desired test count.

---

## 5. Preliminary store-policy review — complete; final submission review remains

Dated record:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

Completed on 2026-08-18:

- [x] current Apple App Review Guidelines reviewed against the current non-clinical CareNest product/package boundary;
- [x] current Google Play Health Content and Services guidance reviewed;
- [x] current Google Play Health apps declaration guidance reviewed;
- [x] current Google Play Data safety guidance reviewed;
- [x] current Microsoft Store sensitive-personal-information/privacy requirements reviewed;
- [x] current repository-only Gumroad/Buy Me a Coffee package-exclusion policy retained;
- [x] current listing wording boundary documented: no diagnosis, dosage inference/calculation, treatment recommendation, clinical interaction/risk claim, guaranteed reminder delivery, emergency-service claim, or unapproved medical-device claim.

Still required against the exact production package/listing:

- [ ] re-open the official Apple policies on the actual submission date;
- [ ] re-open the official Google Play policies on the actual submission date;
- [ ] re-open the official Microsoft/Windows policies on the actual submission date where applicable;
- [ ] complete the live Google Play Health apps declaration;
- [ ] complete the live Google Play Data safety answers;
- [ ] complete Apple privacy/store metadata against the exact production capabilities/binary;
- [ ] complete Microsoft/Partner Center privacy/store metadata where applicable;
- [ ] record final review date, official sources, conclusions and any required changes;
- [ ] repeat affected exact-source verification if final policy review requires a product/package change.

A dated pre-submission review is useful release evidence but is not store approval.

---

## 6. Packaged existing-data and SQLite compatibility

Use fictional/synthetic data only.

- [ ] prepare a representative earlier candidate data set containing profiles, medicines, schedules, occurrences, logs, appointments, stock notes, documents/tags and settings;
- [ ] install/upgrade through each realistic target package/update path used for production;
- [ ] confirm SQLite opens successfully;
- [ ] run and record integrity validation;
- [ ] verify representative records remain readable and editable;
- [ ] verify expected schema version/migrations;
- [ ] verify reminder rebuild/reconciliation after upgrade;
- [ ] verify no duplicate/stale platform request is stranded;
- [ ] record package, source SHA, checksum and results.

Runbook: `docs/releases/PACKAGED_RELEASE_VALIDATION.md`.

---

## 7. Encrypted document and backup compatibility

With fictional data:

- [ ] packaged encrypted-document import/open/export/delete;
- [ ] failed export cleanup;
- [ ] missing/corrupt document-key fail-closed behavior;
- [ ] packaged encrypted backup creation;
- [ ] backup inspection/restore;
- [ ] wrong-password rejection;
- [ ] tamper rejection;
- [ ] truncation rejection;
- [ ] trailing-data rejection;
- [ ] restored encrypted-document usability;
- [ ] clean-install restore;
- [ ] genuine historical v1 fixture validation if genuine historical bytes safely exist.

Never manufacture a current artifact and label it historical evidence.

---

## 8. Android real-device/emulator validation

On representative supported Android versions/vendors:

- [ ] fresh install/onboarding;
- [ ] notification permission denied and granted;
- [ ] medicine reminder create/edit/delete;
- [ ] appointment reminder create/edit/delete;
- [ ] actual reminder delivery;
- [ ] Taken/Skipped/Delayed/Missed cancellation-first behavior;
- [ ] Snooze cancellation/replacement and future-snooze edge cases;
- [ ] stale-request cleanup after schedule edits;
- [ ] medicine/profile deletion cleanup;
- [ ] app restart/reopen recovery;
- [ ] reboot rebuild;
- [ ] exact/inexact alarm behavior;
- [ ] battery-optimization/vendor restrictions;
- [ ] clock/time-zone/DST recovery;
- [ ] force-stop limitation messaging;
- [ ] document picker/share;
- [ ] backup/restore;
- [ ] app lock;
- [ ] accessibility.

---

## 9. Windows validation

On representative Windows 11 targets:

- [ ] fresh execution/install path;
- [ ] core CRUD/navigation;
- [ ] running-app reminder behavior;
- [ ] closed-app limitation behavior/messaging;
- [ ] same-ID timer replacement/cancellation;
- [ ] reminder actions/snooze;
- [ ] restart/recovery;
- [ ] document picker/share;
- [ ] backup/restore;
- [ ] app lock;
- [ ] keyboard/focus;
- [ ] light/dark/system themes.

---

## 10. iPhone/iPad real-device validation

Simulator compilation is not real-device notification evidence.

- [ ] fresh install;
- [ ] permission denied/granted;
- [ ] medicine reminders;
- [ ] appointment reminders;
- [ ] reminder actions/snooze;
- [ ] restart/recovery;
- [ ] time-zone/DST behavior;
- [ ] backup/restore;
- [ ] document picker/share;
- [ ] app lock;
- [ ] Dynamic Type;
- [ ] VoiceOver;
- [ ] notification-preview privacy.

---

## 11. Mac Catalyst validation

- [ ] fresh execution/install path;
- [ ] notification permission/delivery;
- [ ] reminder actions/snooze/reconciliation;
- [ ] restart behavior;
- [ ] file picker/share;
- [ ] backup/restore;
- [ ] app lock;
- [ ] keyboard/focus;
- [ ] theme/contrast;
- [ ] signed/notarized candidate behavior when available.

---

## 12. Accessibility validation

Automated source checks do not replace real assistive-technology testing.

- [ ] representative screen readers;
- [ ] reading order/names/hints;
- [ ] large text/approximately 200% representative scaling;
- [ ] destructive confirmation readability;
- [ ] desktop keyboard/focus;
- [ ] light/dark/system contrast;
- [ ] color-independent meaning;
- [ ] reduced-motion behavior;
- [ ] privacy-safe actionable errors.

Repository promotional graphics should also retain meaningful alt text/plain-text URL fallback.

---

## 13. Production signing outside Git

Never commit private signing material.

- [ ] Android production keystore/signing service configured outside Git;
- [ ] Apple certificate/provisioning/store signing configured outside Git;
- [ ] Windows production signing identity configured outside Git;
- [ ] safe public signing fingerprints/identifiers recorded where appropriate;
- [ ] signing timestamp/source SHA/package checksum recorded.

---

## 14. Structured final-package evidence

Guide:

`docs/releases/PACKAGE_EVIDENCE_TOOLING.md`

For every final production artifact:

- [ ] run `build/scripts/create-package-evidence.py --stage production` or the Bash/PowerShell wrapper;
- [ ] require immutable `v*` source tag;
- [ ] require source tag to resolve to recorded source SHA;
- [ ] require checked-out HEAD to equal recorded source SHA;
- [ ] require clean tracked workspace;
- [ ] provide only non-secret real signing/notarization/store-managed provenance text;
- [ ] require the store-safe scanner to pass;
- [ ] record per-file SHA-256 evidence;
- [ ] record top-level package/directory payload SHA-256;
- [ ] keep generated JSON outside the package payload;
- [ ] independently cross-check the package evidence payload SHA-256;
- [ ] retain the JSON in final release evidence.

The tool does not sign artifacts or prove store approval.

---

## 15. Final signed-package inspection

For every intended production package:

- [ ] record exact source SHA/version/build/application identity;
- [ ] record package filename and SHA-256;
- [ ] record signing/notarization/store-managed provenance;
- [ ] record package evidence JSON path and payload SHA-256;
- [ ] scan the final payload for `buymeacoffee.com/sanskarIN`;
- [ ] scan the final payload for `ramsandesh.gumroad.com`;
- [ ] verify no Gumroad/Buy Me a Coffee promotional artwork/card/command exists in the app;
- [ ] verify repository/support links that are intentionally part of the app remain available as documented;
- [ ] verify privacy/terms/security/notices;
- [ ] verify no health feature changes according to Gumroad purchase/funding state;
- [ ] install/start the package and run platform smoke tests.

The official repository storefront remains:

**https://ramsandesh.gumroad.com**

It is intentionally promoted outside the app package under the current policy.

---

## 16. Store metadata and policy review at submission

The preliminary 2026-08-18 policy review is complete. At actual submission, use the live platform policies and exact production binary/listing rather than this dated snapshot alone.

- [ ] Apple submission-date policy/listing review;
- [ ] Google Play submission-date policy/listing review;
- [ ] Microsoft/Windows submission-date review if used;
- [ ] live Google Play Health apps declaration;
- [ ] live Google Play Data safety declaration;
- [ ] Apple privacy/store metadata;
- [ ] Microsoft/Partner Center privacy/store metadata where applicable;
- [ ] health-organizer claims/disclaimers;
- [ ] reminder/notification wording;
- [ ] privacy/data-safety declarations;
- [ ] fictional-data screenshots;
- [ ] support/privacy/terms/security links;
- [ ] external-commerce/storefront policy review;
- [ ] confirm repository Gumroad promotion remains separate from the submitted app package unless an explicitly reviewed policy change is approved;
- [ ] record final review date/source/conclusion.

Store policies are time-sensitive.

---

## 17. Freeze exact production source

Only after applicable automated/manual/package/signing/store findings are resolved:

- [ ] select the exact approved production commit;
- [ ] repeat exact-source automated verification if any verification-relevant source changed;
- [ ] verify release version/build metadata;
- [ ] verify release notes;
- [ ] verify signed-package hashes/provenance/package evidence JSON;
- [ ] ensure no unresolved production blocker remains.

Do not move a failed/rejected production tag to a different source just to reuse its version identity.

---

## 18. Production `v*` tag and final gates

For the exact approved production tag require all applicable configured gates:

- [ ] tagged CareNest CI;
- [ ] tagged CodeQL;
- [ ] tagged unsuppressed dependency audit;
- [ ] tagged Store Package Configuration;
- [ ] tagged Store Inspection Artifacts;
- [ ] tagged Release Gate;
- [ ] tagged Release Evidence;
- [ ] release-evidence package-tool self-test evidence;
- [ ] artifact/checksum/provenance evidence;
- [ ] final signed-package structured evidence JSON;
- [ ] final signed-package provenance;
- [ ] final publication/submission evidence.

Only then describe the product as production-published/store-approved for the relevant platform.

---

# Priority 1 — after RC1 production validation

- [ ] accessibility improvements driven by real assistive-technology findings;
- [ ] canonical packaged upgrade fixtures from genuine released artifacts;
- [ ] expanded signed-test package inspection when safe signing infrastructure exists;
- [ ] release-evidence automation improvements that retain exact-source/fail-closed semantics;
- [ ] performance measurement only where it has a concrete product purpose;
- [ ] evaluate future store-policy changes separately if in-app commerce is ever proposed.

---

# Deferred future scope

Still outside RC1 without a separate design/review:

- cloud synchronization;
- remote caregiver collaboration;
- required accounts/phone authentication;
- server-side health-record storage;
- silent remote sharing;
- hidden analytics/telemetry;
- diagnosis;
- dosage calculation/inference;
- treatment recommendations;
- clinical interaction/risk claims;
- in-app Gumroad/Buy Me a Coffee commerce under the current store policy.

Any future networked, remote-data, commerce-in-app or clinical feature needs a new consent/authentication/privacy/key-management/threat-model/safety/store/legal/testing design.

---

## Continuation rule

The intended RC runtime product scope remains source-complete. The package-evidence/release-governance source work in this continuation is also implemented, but it is now the reason a **fresh exact-source automated verification is the immediate next gate**.

Do not continue adding unrelated verification-relevant changes after freezing the intended verification source.

If automated/manual/package/security/accessibility testing finds a defect:

1. reproduce it safely with synthetic data where applicable;
2. fix the smallest correct source boundary;
3. add the lowest appropriate regression coverage;
4. run the full applicable exact-source matrix again;
5. update current evidence only after the new run completes.

Current state: **CareNest `1.0.0-rc.1` remains source-complete for its intended runtime RC scope, with repository-only Gumroad/BMC promotion, package-level external-commerce exclusion, a previously verified 336/336 Gumroad rollout baseline, a dated preliminary Apple/Google/Microsoft store-policy review, and newly implemented fail-closed structured package-evidence/release-governance tooling; the newer current source now requires fresh exact-head automation before it can become the next verified baseline.**
