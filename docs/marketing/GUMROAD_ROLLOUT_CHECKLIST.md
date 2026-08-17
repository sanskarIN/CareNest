# Gumroad Rollout Checklist

**Canonical URL:** https://ramsandesh.gumroad.com  
**Verified implementation/source-policy SHA:** `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

Use this checklist whenever Gumroad branding, links, or repository marketing are changed.

## Repository visibility

- [x] Main `README.md` highlights the storefront.
- [x] `SUPPORT.md` highlights the storefront.
- [x] `.github/FUNDING.yml` includes the storefront as a repository custom link.
- [x] `GUMROAD.md` provides a canonical reader-facing guide.
- [x] `docs/README.md` links the storefront documentation.
- [x] `docs/DOCUMENTATION_CATALOG.md` maps ownership.
- [x] Repository-only SVG branding exists.
- [x] Marketing and asset documentation exists.

## Runtime/package isolation

- [x] No Gumroad URL is intentionally added under `src/CareNest.App`.
- [x] No Gumroad promotional image is intentionally added under application resources.
- [x] UI contract tests reject the Gumroad URL in application runtime sources.
- [x] Store-payload contract tests require the scanner to know the Gumroad marker.
- [x] Store-safe payload scanner defaults include `ramsandesh.gumroad.com`.
- [x] Verified implementation/source-policy normal platform builds are green.
- [x] Verified implementation/source-policy store-candidate builds are green.
- [x] Verified implementation/source-policy CodeQL is green.
- [x] Verified implementation/source-policy core tests are 336/336 green.

Authoritative automated evidence:

`docs/releases/GUMROAD_ROLLOUT_VERIFICATION_20260817.md`

Documentation-only promotion commits after the verified implementation/source-policy SHA should still be checked against their own exact-head workflows before describing that repository head as fully green.

## Messaging safety

- [x] No Gumroad purchase is described as a medical/clinical entitlement.
- [x] No Gumroad purchase is described as changing reminder delivery or priority.
- [x] Documentation states that health data is not sent to Gumroad by CareNest.
- [x] Plain-text URL fallback accompanies visual CTA usage.

## Historical evidence

- [x] Existing dated verification documents are not rewritten merely to backfill Gumroad promotion.
- [x] Active continuation details are recorded in `what_changed.md`.

## Production work remains separate

This rollout checklist does not complete real-device, accessibility, packaged upgrade/encryption compatibility, production signing, final signed-package scanning, store-policy review, production tagging, or publication evidence. Those remain tracked in `docs/releases/NEXT_STEPS.md`.
