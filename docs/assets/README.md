# Repository Documentation Assets

This directory contains documentation- and repository-only visual assets.

## Gumroad storefront badge

Asset:

`gumroad_store_badge.svg`

Canonical destination:

**https://ramsandesh.gumroad.com**

The badge is designed for GitHub Markdown, documentation pages, project-resource pages, and other repository-owned promotional surfaces.

### Accessibility

The SVG includes:

- a `<title>` describing the storefront CTA;
- a `<desc>` identifying the destination and repository-only package boundary;
- the canonical URL as visible text.

When embedding the SVG in Markdown/HTML, also provide meaningful `alt` text and keep a plain-text URL nearby.

### Package rule

Do **not** copy repository promotional assets into `src/CareNest.App`.

CareNest store-distributed application output is protected by the store-safe payload scanner, which rejects repository-only external-commerce markers including:

- `buymeacoffee.com/sanskarIN`;
- `ramsandesh.gumroad.com`.

### Recommended embed

```html
<a href="https://ramsandesh.gumroad.com">
  <img src="docs/assets/gumroad_store_badge.svg"
       alt="Shop on Gumroad — https://ramsandesh.gumroad.com"
       width="850" />
</a>
```

For the full policy, see:

- `GUMROAD.md`;
- `docs/marketing/GUMROAD_PLACEMENT_AND_COMPLIANCE.md`.
