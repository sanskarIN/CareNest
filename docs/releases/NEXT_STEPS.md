# CareNest Next Steps

**Date:** 2026-08-19  
**Release line:** `1.0.0-rc.1`  
**Canonical Gumroad storefront:** `https://ramsandesh.gumroad.com`  
**Accepted automated source:** `30ee6c265104c64ec5a1a4013f592f7f058750e8`  
**Merged source commit:** `2549c08b25145f20c59b7e73ca227c35d5bbf0ec`

The complete active checklist from before the Gumroad rollout is preserved exactly at:

`docs/history/pre-gumroad-rollout-20260817/NEXT_STEPS.md`

The in-repository RC1 feature, hardening, documentation, open-source/community-file, package-evidence and automated exact-head verification work is complete for the current candidate. This file now tracks only work that still requires actual packages/devices/signing/store access or a newly reproduced defect.

Current automated authority:

- `docs/releases/AUTOMATED_BASELINE.md`;
- `docs/releases/BACKUP_RESOURCE_HARDENING_20260819.md`;
- `docs/releases/FINAL_CANDIDATE_VERIFICATION_20260818.md` for the immediately previous accepted baseline.

---

## 1. Final accepted automated boundary — complete

Verified exact source:

`30ee6c265104c64ec5a1a4013f592f7f058750e8`

Verified PR merge ref:

`84fda5bb8ced9f4c487110e43652f51ba2d8d495`

Merged `main` commit preserving all PR commits:

`2549c08b25145f20c59b7e73ca227c35d5bbf0ec`

Verification/merge PR:

`#81` — merged after the complete required matrix succeeded.

Observed automated evidence:

- [x] repository Python tooling syntax;
- [x] package-evidence self-test;
- [x] documentation-link checker self-test;
- [x] stable active documentation local-link integrity — 182 links / 111 stable active Markdown files;
- [x] platform-neutral formatting;
- [x] unit tests — **122/122**;
- [x] integration tests — **54/54**;
- [x] UI/source-policy tests — **194/194**;
- [x] total core tests — **370/370**;
- [x] Android Release;
- [x] Windows Release;
- [x] iOS simulator Release;
- [x] Mac Catalyst Release;
- [x] Store Package Configuration — all four configured targets;
- [x] Store Inspection Artifacts — scanner self-test plus Android/Windows/Apple artifacts;
- [x] CodeQL;
- [x] unsuppressed Dependency Audit;
- [x] PR #81 merged with merge commit after success;
- [x] accepted baseline promoted to designated dynamic evidence/status files.

Workflow runs:

- CareNest CI `32205946013`;
- Store Package Configuration `32205946003`;
- Store Inspection Artifacts `32205946001`;
- CodeQL `32205946030`;
- Dependency Audit `32205946026`.

---

## 2. Source/product work complete

Do not repeat these passes unless a real defect or changed requirement is discovered:

- [x] local-first/account-free RC1 product scope;
- [x] profile/medicine/schedule/log/appointment/document/report/settings/app-lock source scope;
- [x] deterministic reminder planning/reconciliation/compensation hardening;
- [x] encrypted document and password-encrypted backup protections;
- [x] bounded authenticated backup decrypted-container/archive resource handling;
- [x] generated-backup validation against current restore limits before encryption;
- [x] legacy/current encrypted-stream compatibility under caller-provided plaintext limits;
- [x] SQLite transactional/migration/dependency-security hardening;
- [x] strict compiled XAML binding enforcement;
- [x] `XC0022`–`XC0025` promoted to errors;
- [x] runtime C# defect-pattern audit;
- [x] structured XAML/XML/project/JSON validation;
- [x] CodeQL/dependency/release/store workflow policies;
- [x] external Buy Me a Coffee application-package removal;
- [x] repository-first Gumroad storefront documentation/branding;
- [x] repository-only Gumroad badge accessibility metadata;
- [x] Gumroad/BMC no-medical-entitlement boundary;
- [x] Gumroad/BMC absence from runtime/application resources;
- [x] store-payload scanner covering both external-commerce markers;
- [x] package-evidence generator/wrappers/self-test;
- [x] release-documentation consistency contracts;
- [x] documentation-link checker with example-only exclusion regression coverage;
- [x] stable/dynamic documentation evidence boundary;
- [x] open-source/community repository-file audit;
- [x] dated preliminary Apple/Google/Microsoft store-policy review;
- [x] final exact-head automated verification after backup resource hardening.

The repository issue search at the start of this continuation returned no open GitHub issues. Do not infer from this that undiscovered defects are impossible.

---

## 3. Backup resource-hardening follow-up — source complete, packaged compatibility pending

The 2026-08-19 continuation reproduced a concrete availability/resource-exhaustion gap in authenticated backup processing. It is now fixed and verified in source.

Current default limits:

- decrypted ZIP container: **2304 MiB** maximum;
- manifest: **1 MiB** maximum;
- SQLite database: **1 GiB** maximum;
- each encrypted document: **512 MiB** maximum;
- total uncompressed ZIP payload: **2 GiB** maximum;
- documents: **5,000** maximum;
- archive-entry count: document limit plus fixed required entries;
- explicit directory-only ZIP entries: rejected.

Completed source work:

- [x] bound decrypted authenticated backup output before ZIP parsing;
- [x] validate archive entry count before manifest parsing;
- [x] reject directory-only archive entries;
- [x] reject oversized manifest/database/document entries;
- [x] reject excessive total uncompressed ZIP payload;
- [x] safely validate configured document-count ceilings without integer overflow;
- [x] validate newly generated backups against the same current restore boundary before encryption;
- [x] retain existing tamper/truncation/trailing-data handling;
- [x] retain legacy framing compatibility while honoring caller-provided plaintext limits;
- [x] add 15 focused integration regressions;
- [x] run and pass the complete exact-head CI/store/security matrix.

Still required with actual intended packages and fictional data:

- [ ] verify representative normal packaged backups remain comfortably below current ceilings;
- [ ] verify packaged create/inspect/restore on each intended platform path;
- [ ] verify clean-install restore;
- [ ] verify restored encrypted documents remain usable;
- [ ] verify wrong-password/tamper/truncation/trailing-data behavior in packaged builds;
- [ ] test genuine historical encrypted backup bytes where genuine prior fixtures safely exist;
- [ ] if a genuine historical backup exceeds a current limit, record and resolve it as an explicit compatibility/security decision rather than silently weakening the boundary.

Runbook: `docs/releases/PACKAGED_RELEASE_VALIDATION.md`.

---

# Priority 0 — remaining production validation

## 4. Preliminary store-policy review is complete; submission-day review remains

Dated preliminary record:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

Completed:

- [x] Apple policy review against current non-clinical product/package boundary;
- [x] Google Play Health Content and Services review;
- [x] Google Play Health apps declaration guidance review;
- [x] Google Play Data safety guidance review;
- [x] Microsoft sensitive-personal-information/privacy review;
- [x] repository-only Gumroad/Buy Me a Coffee package-exclusion policy retained.

Still required against the exact production package/listing:

- [ ] re-open official Apple policies on the actual submission date;
- [ ] re-open official Google Play policies on the actual submission date;
- [ ] re-open official Microsoft/Windows policies on the actual submission date where applicable;
- [ ] complete live Google Play Health apps declaration;
- [ ] complete live Google Play Data safety answers;
- [ ] complete Apple privacy/store metadata;
- [ ] complete Microsoft/Partner Center privacy/store metadata where applicable;
- [ ] record final review date, official sources, conclusions and required changes;
- [ ] repeat affected exact-source verification if final policy review requires source/package changes.

A preliminary policy review is evidence, not store approval.

---

## 5. Packaged existing-data and SQLite compatibility

Use fictional/synthetic data only.

- [ ] prepare representative earlier-candidate data containing profiles, medicines, schedules, occurrences, logs, appointments, stock, documents/tags and settings;
- [ ] install/upgrade through each realistic target package/update path used for production;
- [ ] confirm SQLite opens successfully;
- [ ] run and record integrity validation;
- [ ] verify representative records remain readable/editable;
- [ ] verify expected schema version/migrations;
- [ ] verify reminder rebuild/reconciliation after upgrade;
- [ ] verify no duplicate/stale platform request is stranded;
- [ ] record package, source SHA, checksum, platform and results.

Runbook: `docs/releases/PACKAGED_RELEASE_VALIDATION.md`.

---

## 6. Encrypted document and backup compatibility

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
- [ ] resource-ceiling rejection behavior where practical with deliberately small test-only fixtures or controlled packaged tests;
- [ ] restored encrypted-document usability;
- [ ] clean-install restore;
- [ ] genuine historical fixture validation only where genuine historical bytes safely exist.

Never manufacture a current artifact and label it historical evidence.

---

## 7. Android real-device/emulator validation

On representative supported Android versions/vendors:

- [ ] fresh install/onboarding;
- [ ] notification permission denied/granted;
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

## 8. Windows validation

On representative Windows targets:

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

## 9. iPhone/iPad real-device validation

Simulator compilation is not real-device notification evidence.

- [ ] fresh install;
- [ ] notification permission denied/granted;
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

## 10. Mac Catalyst validation

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

## 11. Accessibility validation

Automated source checks do not replace real assistive-technology testing.

- [ ] representative screen readers;
- [ ] reading order/names/hints;
- [ ] large text/representative ~200% scaling;
- [ ] destructive confirmation readability;
- [ ] desktop keyboard/focus;
- [ ] light/dark/system contrast;
- [ ] color-independent meaning;
- [ ] reduced-motion behavior;
- [ ] privacy-safe actionable errors.

---

## 12. Production signing outside Git

Never commit private signing material.

- [ ] Android production keystore/signing service configured outside Git;
- [ ] Apple certificate/provisioning/store signing configured outside Git;
- [ ] Windows production signing identity configured outside Git where applicable;
- [ ] safe public signing fingerprints/identifiers recorded where appropriate;
- [ ] signing timestamp/source SHA/package checksum recorded.

---

## 13. Structured final-package evidence

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
- [ ] independently cross-check package evidence payload SHA-256;
- [ ] retain the JSON in final release evidence.

The tool does not sign artifacts or prove store approval.

---

## 14. Final signed-package inspection

For every intended production package:

- [ ] record exact source SHA/version/build/application identity;
- [ ] record package filename and SHA-256;
- [ ] record signing/notarization/store-managed provenance;
- [ ] record package evidence JSON path and payload SHA-256;
- [ ] scan final payload for `buymeacoffee.com/sanskarIN`;
- [ ] scan final payload for `ramsandesh.gumroad.com`;
- [ ] verify no Gumroad/Buy Me a Coffee promotional artwork/card/command exists in the app;
- [ ] verify intended repository/support/legal links remain accurate;
- [ ] verify no health feature changes according to Gumroad purchase/funding state;
- [ ] install/start the package and run platform smoke tests.

Official repository storefront:

**https://ramsandesh.gumroad.com**

It remains intentionally outside the app package under the current policy.

---

## 15. Store metadata and policy review at submission

- [ ] Apple submission-date policy/listing review;
- [ ] Google Play submission-date policy/listing review;
- [ ] Microsoft/Windows submission-date review if used;
- [ ] live Google Play Health apps declaration;
- [ ] live Google Play Data safety declaration;
- [ ] Apple privacy/store metadata;
- [ ] Microsoft/Partner Center privacy/store metadata where applicable;
- [ ] health-organizer claims/disclaimers verified;
- [ ] reminder/notification limitation wording verified;
- [ ] privacy/data-safety declarations match exact binary;
- [ ] fictional-data screenshots match exact package;
- [ ] support/privacy/terms/security links verified;
- [ ] external-commerce/storefront policy reviewed for exact listing;
- [ ] final review date/source/conclusion recorded.

Store policies are time-sensitive.

---

## 16. Freeze exact production source and tag

Only after applicable manual/package/accessibility/signing/store findings are resolved:

- [ ] select exact approved production commit;
- [ ] repeat exact-source automated verification if any verification-relevant source changes after `30ee6c265104c64ec5a1a4013f592f7f058750e8`;
- [ ] verify release version/build metadata;
- [ ] verify release notes;
- [ ] verify signed-package hashes/provenance/package evidence JSON;
- [ ] ensure no unresolved production blocker remains;
- [ ] create immutable approved `v*` tag.

The designated dynamic status/evidence files may advance beyond the frozen verified executable source without redefining that source boundary. Runtime/test/project/workflow/build-script/stable-policy/stable-documentation changes require a replacement exact-source verification.

Do not move a failed/rejected production tag to a different source merely to reuse its version identity.

---

## 17. Production `v*` tag and final gates

For the exact approved production tag require:

- [ ] tagged CareNest CI;
- [ ] tagged CodeQL;
- [ ] tagged unsuppressed Dependency Audit;
- [ ] tagged Store Package Configuration;
- [ ] tagged Store Inspection Artifacts;
- [ ] tagged Release Gate;
- [ ] tagged Release Evidence;
- [ ] Release Evidence package-tool self-test evidence;
- [ ] artifact/checksum/provenance evidence;
- [ ] final signed-package structured evidence JSON;
- [ ] final signed-package provenance;
- [ ] final store submission/approval/publication evidence.

Only then describe the product as production-published/store-approved for the relevant platform.

---

# Priority 1 — after RC1 production validation

- [ ] accessibility improvements driven by actual assistive-technology findings;
- [ ] canonical packaged upgrade fixtures from genuine released artifacts;
- [ ] expanded signed-test package inspection when safe signing infrastructure exists;
- [ ] release-evidence automation improvements that preserve exact-source/fail-closed semantics;
- [ ] performance work only where measurement identifies a concrete product need;
- [ ] evaluate future store-policy changes separately if in-app commerce is ever proposed.

---

# Deferred future scope

Still outside RC1 without a separate product/privacy/security/safety/store design:

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
- in-app Gumroad/Buy Me a Coffee commerce under the current policy.

Any future networked, remote-data, commerce-in-app or clinical feature requires a new consent/authentication/privacy/key-management/threat-model/safety/store/legal/testing review.

---

## Continuation rule

The intended RC runtime product scope, package-evidence/release-governance tooling, documentation-integrity tooling and current automated exact-head verification are complete.

Do **not** add unrelated verification-relevant source merely to create more commits after this candidate is green.

If actual device/package/security/accessibility/store validation finds a defect:

1. reproduce it safely with synthetic data where applicable;
2. fix the smallest correct source boundary;
3. add the lowest appropriate regression coverage;
4. run the full applicable exact-source matrix again;
5. rebuild/retest the affected final package;
6. update evidence only after the replacement result is known.

Current state: **CareNest `1.0.0-rc.1` is source-complete for its intended RC scope. Exact source `30ee6c265104c64ec5a1a4013f592f7f058750e8` passed 370/370 core tests, all configured normal platform builds, all store-candidate builds, Store Inspection Artifacts, CodeQL and unsuppressed Dependency Audit, and PR #81 was merged while preserving its 19 meaningful commits. Remaining work is real production package/device/accessibility/signing/store/publication evidence, including genuine packaged/historical backup compatibility against the new resource ceilings—not another speculative source-feature pass.**
