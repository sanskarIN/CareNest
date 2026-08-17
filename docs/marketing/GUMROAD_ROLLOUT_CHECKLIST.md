# Gumroad Rollout Checklist

**Canonical URL:** https://ramsandesh.gumroad.com

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
- [ ] Final exact-revision normal platform builds green.
- [ ] Final exact-revision store-candidate builds green.
- [ ] Final exact-revision CodeQL green.

The unchecked verification items must only be checked after the exact final revision completes its configured GitHub Actions workflows.

## Messaging safety

- [x] No Gumroad purchase is described as a medical/clinical entitlement.
- [x] No Gumroad purchase is described as changing reminder delivery or priority.
- [x] Documentation states that health data is not sent to Gumroad by CareNest.
- [x] Plain-text URL fallback accompanies visual CTA usage.

## Historical evidence

- [x] Existing dated verification documents are not rewritten merely to backfill Gumroad promotion.
- [x] Active continuation details are recorded in `what_changed.md`.
