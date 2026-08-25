# Changelog

All notable CareNest changes are recorded here using Keep a Changelog-style categories and semantic-versioning intent.

The complete changelog that was active immediately before the 2026-08-17 Gumroad rollout is preserved exactly at:

`docs/history/pre-gumroad-rollout-20260817/CHANGELOG.md`

Earlier exact snapshots remain under `docs/history/` and Git history.

## [Unreleased] - 2026-08-25

### Changed — CareNest 2.18.13 maintenance preparation

Started the next maintenance patch line from merged `main` commit:

`b2db4821047dbfb7fe223961fc237afcdfc8371e`

The active source metadata is now prepared for:

- semantic/display version `2.18.13`;
- assembly/file version `2.18.13.0`;
- MAUI package/build code `21813`;
- `Microsoft.Maui.Controls` `10.0.100`;
- Avalonia `12.1.1` package family.

This is a source/version roll-forward only. It does not claim production publication, signing, real-device validation, accessibility completion, store approval or Linux/browser full feature parity.

### Added — 2.18.13 release package

Added:

- `docs/releases/VERSION_2_18_13_PREPARATION.md`;
- `docs/releases/RELEASE_NOTES_2_18_13_DRAFT.md`;
- `docs/releases/RELEASE_CHECKLIST_2_18_13.md`.

The version-specific package records the exact starting repository boundary, requires fresh exact-head automation for the final `2.18.13` source, and keeps all unresolved production/manual evidence fail-closed.

### Changed — active status and handoff

Updated:

- `PROJECT_STATUS.md`;
- `docs/releases/NEXT_STEPS.md`;
- `what_changed.md`.

The active documentation now identifies `2.18.13` / `21813` as the preparation line while retaining the accepted `2.18.12` workflow/test results only as historical exact-source evidence.

### Changed — version consistency protection

`tests/CareNest.UiTests/VersionConsistencyContractTests.cs` now protects:

- central `2.18.13` version metadata;
- MAUI `2.18.13` / `21813` metadata;
- `Microsoft.Maui.Controls` `10.0.100`;
- the non-published `2.18.13` version-specific preparation package.

### Added — active release-line alignment contract

Added:

`tests/CareNest.UiTests/ActiveReleaseLineContractTests.cs`

The contract derives the active version/build from source metadata and requires the dynamic status, next-steps and handoff documents to follow that active line. It also requires the matching version-specific release package to exist and remain explicitly non-published/non-released.

The contract additionally protects the handoff rule that fresh exact-head verification must be observed before promotion and that historical workflow success cannot be transferred to a newer source.

### Verification status — fresh exact-head automation required

The branch changes version metadata, tests and verification-sensitive release documentation after the last accepted source boundary.

Therefore no `2.18.13` test count or platform/workflow success is predicted here. The final intended `2.18.13` source must complete fresh CareNest CI, CodeQL, unsuppressed Dependency Audit, Store Package Configuration and Store Inspection Artifacts before merge/promotion.

The last accepted `2.18.12` result remains historical evidence only.

### Production status

CareNest `2.18.13` is **NOT PUBLISHED**.

Production promotion still requires genuine packaged compatibility, real-device/runtime behavior, accessibility, signing/provenance, final package/deployment inspection, live distribution metadata/policy review and actual approval/publication evidence as applicable.

---

## [Unreleased] - 2026-08-18

### Added — current pre-submission store-policy review

Added:

`docs/releases/STORE_POLICY_REVIEW_20260818.md`

The dated review records a current pre-submission check of CareNest's existing non-clinical/local-first/package boundary against official policy areas for:

- Apple App Review Guidelines;
- Google Play Health Content and Services;
- Google Play Health apps declaration;
- Google Play Data safety;
- Microsoft Store sensitive-personal-information/privacy requirements.

This record is intentionally not presented as store approval. The official current policies and live store-console declarations must still be reviewed against the exact production package/listing on the actual submission date.

### Fixed — stale release-policy evidence references

Updated current release documentation that still described older pre-Gumroad or intermediate automated baselines.

The current release-policy/checklist/evidence documents now consistently identify the latest fully verified Gumroad implementation/source-policy baseline as:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

Verified results on that exact source remain:

- 122/122 unit tests;
- 39/39 integration tests;
- 175/175 UI/source-policy tests;
- **336/336 total core tests**;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- all four store-candidate configurations;
- CodeQL.

The correction is evidence alignment only. The 336-test result belongs to that exact source and must not be assigned to later verification-relevant heads without a fresh exact run.

### Changed — release/store evidence boundary

Current release documents consistently require final production packages to preserve the established repository-only external-commerce boundary:

- no in-app `buymeacoffee.com/sanskarIN` promotion/purchase surface;
- no in-app `ramsandesh.gumroad.com` promotion/purchase surface;
- final package marker scans for both destinations;
- no health feature or data access linked to purchase/funding state.

The preliminary store-policy review is marked complete, while real-device testing, packaged compatibility, accessibility, production signing, final package inspection, live store-console metadata/declarations, submission-day policy review, production tagging and publication remain production blockers.

### Added — release-documentation consistency contracts

Added:

`tests/CareNest.UiTests/ReleaseDocumentationConsistencyContractTests.cs`

The contract protects current active release governance from drifting back to superseded evidence. It checks applicable current documents for:

- the latest fully verified Gumroad source SHA until a newer baseline is actually proven;
- the exact **336/336** result only as that source's recorded baseline;
- absence of the superseded 331-test/current-PR #74 release claims from active current documents;
- both Buy Me a Coffee and Gumroad final-package marker requirements;
- current store-policy review linkage without claiming store approval;
- live Google Play Health apps/Data safety and submission-day policy review remaining open;
- package-evidence guide integration;
- Release Gate evidence/tooling requirements.

These tests mean current release documents can now be verification-sensitive source inputs rather than merely passive prose.

### Added — structured package checksum/provenance tooling

Added:

- `build/scripts/create-package-evidence.py`;
- `build/scripts/create-package-evidence.sh`;
- `build/scripts/create-package-evidence.ps1`;
- `build/scripts/test-create-package-evidence.py`;
- `docs/releases/PACKAGE_EVIDENCE_TOOLING.md`;
- `tests/CareNest.UiTests/PackageEvidenceToolContractTests.cs`.

The package-evidence generator records:

- stage/platform/version/build/package identity;
- exact full source SHA;
- source tag when supplied;
- tracked-workspace state;
- non-secret signing/notarization/store provenance description;
- per-file SHA-256;
- top-level package-file or deterministic directory payload SHA-256;
- mandatory store-safe payload scanner result;
- optional non-sensitive notes.

Production mode fails closed unless:

- an immutable `v*` source tag is supplied;
- that tag resolves to the recorded source SHA;
- checked-out HEAD equals that source SHA;
- tracked Git files are clean;
- signing provenance is not empty/unsigned/not-applicable;
- store-safe payload scanning passes;
- the evidence JSON is written outside the package payload.

The tool does not create signatures, validate private signing credentials by itself, submit packages, prove store approval, replace real-device/accessibility testing, or replace packaged SQLite/encrypted-data compatibility evidence.

### Added — synthetic package-evidence self-test

`build/scripts/test-create-package-evidence.py` uses temporary synthetic payloads only and verifies:

- successful single-file SHA-256 evidence;
- deterministic sorted directory evidence;
- Gumroad marker fail-closed behavior;
- rejection of evidence output inside a hashed payload directory;
- rejection of production evidence without a `v*` source tag.

No real user health data or signing secret is needed for this self-test.

### Changed — CI, Release Gate and Release Evidence

CareNest CI now:

- syntax-checks the store-safe scanner, package-evidence generator and package-evidence self-test with `python3 -m py_compile`;
- runs the synthetic package-evidence self-test before the existing .NET formatting/test steps.

Release Gate now:

- requires current release evidence/runbooks/package-evidence tooling to exist and be non-empty;
- repeats Python syntax verification and the package-evidence synthetic self-test before release source tests.

Release Evidence now:

- records Python version;
- runs the package-evidence syntax/self-test as an independently captured outcome;
- stores self-test output under the release-evidence artifact;
- treats package-tooling failure as a Release Evidence failure.

### Changed — release process and package evidence documentation

Updated current active release documents including:

- `PROJECT_STATUS.md`;
- `docs/releases/NEXT_STEPS.md`;
- `docs/releases/RELEASE_PROCESS.md`;
- `docs/releases/RELEASE_CHECKLIST.md`;
- `docs/releases/QUALITY_GATE.md`;
- `docs/releases/SECURITY_RELEASE_REVIEW.md`;
- `docs/releases/MANUAL_TEST_MATRIX.md`;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`;
- `docs/releases/STORE_BUILD_POLICY.md`;
- `docs/releases/STORE_SUBMISSION_CHECKLIST.md`;
- `docs/releases/EXECUTABLE_BUILD_CHECKLIST.md`;
- `docs/releases/RELEASE_EVIDENCE.md`;
- `docs/releases/RELEASE_NOTES_TEMPLATE.md`;
- `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`;
- `docs/DOCUMENTATION_CATALOG.md`.

The documents now use one consistent route from build output to final evidence:

1. build/sign through the platform's secure signing process;
2. inspect the exact final package;
3. scan for both repository-only external-commerce markers;
4. generate structured package evidence JSON;
5. cross-check SHA-256/provenance;
6. retain real-device/package/accessibility/store evidence separately;
7. publish only after all required exact-tag/manual/store gates pass.

### Verification status — fresh exact-head automation now required

The package-evidence/release-governance continuation changes tests, build scripts and GitHub Actions workflows after the last fully verified baseline.

Therefore:

- `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f` remains the latest fully verified baseline for now;
- **336/336 is not claimed as the result of the newer current head**;
- a new test total must not be predicted from source inspection;
- the final intended current source must complete fresh exact-head CareNest CI, Store Package Configuration, Store Inspection Artifacts, CodeQL and unsuppressed Dependency Audit before a newer baseline is promoted;
- final production-tag Release Gate/Release Evidence remain separate later gates.

No CareNest runtime health-organizer feature was added by this release-engineering continuation.

---

## [Unreleased] - 2026-08-17

### Added — Ram Sandesh Gumroad storefront integration

Canonical storefront:

**https://ramsandesh.gumroad.com**

Added a repository-first Gumroad rollout that highlights the storefront without embedding external commerce into the CareNest health application package.

Added:

- `GUMROAD.md` canonical storefront guide;
- `docs/assets/gumroad_store_badge.svg` repository-only storefront badge;
- `docs/assets/README.md` asset/accessibility guidance;
- `docs/marketing/README.md` marketing documentation hub;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`;
- `docs/marketing/GUMROAD_ROLLOUT_CHECKLIST.md`;
- `docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`;
- Gumroad custom repository link in `.github/FUNDING.yml`.

### Changed — highlighted repository documentation

The Gumroad storefront is now prominently surfaced in current reader-facing and maintainer documentation, including:

- `README.md`;
- `SUPPORT.md`;
- `BUY_ME_A_COFFEE.md`;
- `docs/README.md`;
- `docs/SUPPORT_CARENEST.md`;
- `docs/DOCUMENTATION_CATALOG.md`;
- `docs/COMPLETE_PROJECT_DOCUMENTATION.md`;
- `docs/DEVELOPER_REFERENCE.md`;
- `docs/REPOSITORY_GOVERNANCE.md`;
- `CONTRIBUTING.md`;
- `PROJECT_STATUS.md`;
- active `what_changed.md`.

The exact Gumroad URL is kept visible as a plain-text fallback anywhere the repository badge is used.

### Added — repository-only storefront branding

`docs/assets/gumroad_store_badge.svg` includes:

- custom storefront/shopping artwork;
- exact canonical URL;
- accessible SVG `<title>`;
- accessible SVG `<desc>`;
- explicit repository-only/package-boundary wording.

The generated promotional concept from the chat is represented in source control by a maintainable SVG rather than being silently copied into app package resources.

### Changed — external-commerce package isolation

The CareNest application runtime/package continues to exclude repository-only external commercial/funding destinations.

Current forbidden package markers:

- `ramsandesh.gumroad.com`;
- `buymeacoffee.com/sanskarIN`.

Repository/storefront/funding documentation is separate from health functionality and does not unlock diagnosis, dosage guidance, treatment recommendations, reminder priority/reliability, emergency assistance, clinical support, accounts/cloud behavior, or access to user health data.

CareNest does not automatically transmit local health records to Gumroad.

### Changed — package payload scanner

Updated:

`build/scripts/verify-store-safe-payload.py`

The scanner defaults to both repository-only markers and inspects:

- UTF-8;
- UTF-16 little-endian;
- UTF-16 big-endian;
- regular payload files;
- ZIP-compatible package entries such as AABs.

The scanner fails closed for unreadable/missing inspection paths and returns failure when a forbidden marker is found.

The `--forbidden` option is repeatable for explicit one-or-more marker scans.

### Added/Changed — Gumroad regression contracts

Updated `FundingLinkContractTests.cs` to protect:

- repository Gumroad visibility;
- support/metadata placement;
- no in-app About surface;
- no medical/health entitlement claims;
- repository SVG accessibility metadata;
- absence of the Gumroad badge from app resources.

Updated `StoreFundingPayloadContractTests.cs` to protect:

- no Gumroad/Buy Me a Coffee URL in application runtime text-like files;
- no external-commerce URL constant in the shared runtime assembly;
- no obsolete external-commerce build switches;
- both package-scanner markers;
- UTF-8/UTF-16/ZIP scanning behavior;
- fail-closed scanner semantics.

### Changed — current documentation/evidence governance

The active `PROJECT_STATUS.md` and `docs/COMPLETE_PROJECT_DOCUMENTATION.md` were modernized for the 2026-08-17 Gumroad/source-line baseline after preserving their complete prior versions exactly under:

`docs/history/pre-gumroad-rollout-20260817/`

The previous active `what_changed.md` was also preserved in that history directory before the Gumroad handoff replaced it.

Historical dated verification files are not rewritten merely to backfill newer storefront links.

### Verified — Gumroad rollout automated baseline

Exact verified implementation/source-policy SHA:

`94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

That exact source passed:

- 122/122 unit tests;
- 39/39 integration tests;
- 175/175 UI/source-policy tests;
- **336/336 total core tests**;
- Android Release;
- Windows Release;
- iOS simulator Release;
- Mac Catalyst Release;
- Android/Windows/iOS/Mac Catalyst store-candidate configurations;
- CodeQL.

Authoritative evidence:

`docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`

The UI/source-policy count increased from 173 to 175 because the rollout adds independent Gumroad repository-placement/accessibility/package-isolation coverage.

### Fixed — Gumroad entitlement wording contract false positive

The first documentation-finalization candidate `b5a57186af60e8b42bb917dfa85de24c3c9c1e9a` exposed one newly added test assertion that searched for singular `does not unlock` even though `GUMROAD.md` correctly stated “Gumroad purchases do not unlock medical advice...”.

The assertion was corrected without weakening the health-safety requirement. The replacement source `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f` then passed the full configured matrix above.

### Production status

CareNest remains `1.0.0-rc.1`.

The intended RC source scope and Gumroad repository/package-isolation rollout are automated-verified at the exact named baseline, but production promotion still requires newer source verification when verification-relevant changes occur, real-device notification/lifecycle validation, accessibility evidence, packaged existing-data/encrypted-data compatibility, production signing, structured/final signed-package inspection, current store metadata/policy review, an exact approved immutable production tag and publication evidence.

Do not describe CareNest as globally bug-free, production-signed, store-approved or production-published until those external gates are actually completed.

---

## Historical changelog

For the complete 2026-08-16 compiled-binding entry, the complete 2026-08-15 funding-package/final-bug-audit entry, and all earlier details, use:

`docs/history/pre-gumroad-rollout-20260817/CHANGELOG.md`

That preserved file remains the exact prior active changelog rather than a shortened reconstruction.
