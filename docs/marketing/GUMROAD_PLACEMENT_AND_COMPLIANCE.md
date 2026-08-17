# Gumroad Placement and Compliance Guide

**Canonical storefront:** https://ramsandesh.gumroad.com  
**Repository badge:** `docs/assets/gumroad_store_badge.svg`  
**Applies to:** CareNest repository/documentation/marketing surfaces

## Purpose

This document defines how the Ram Sandesh Gumroad storefront is highlighted across CareNest materials without turning external commerce into a CareNest health feature or silently embedding a storefront destination inside store-distributed application binaries.

## Required repository placement

Current, reader-facing repository material should surface the canonical Gumroad URL when a support, publisher, author, documentation-navigation, project-resource, or marketing section is already present.

High-value surfaces include:

1. root `README.md`;
2. root `SUPPORT.md`;
3. root `GUMROAD.md`;
4. `.github/FUNDING.yml` custom links;
5. `docs/README.md`;
6. `docs/DOCUMENTATION_CATALOG.md`;
7. contributor/maintainer guidance where external project resources are described;
8. release/continuation handoff documentation;
9. repository-only promotional artwork.

Historical snapshots and dated evidence are not rewritten merely to add a modern storefront URL.

## Runtime/package boundary

The packaged CareNest application remains free of the Gumroad destination and Gumroad promotional artwork.

Do not place the storefront URL or repository Gumroad badge under:

- `src/CareNest.App/`;
- runtime ViewModels;
- runtime XAML pages;
- shared constants compiled into the application;
- packaged resources;
- platform manifests/plists for promotional purposes.

This keeps CareNest focused on local-first organization and avoids creating an external-commerce surface inside the health organizer without a dedicated store-policy review.

## Automated enforcement

The store-safe payload scanner should reject both repository commercial/funding destinations if their marker bytes appear in packaged output:

- `buymeacoffee.com/sanskarIN`;
- `ramsandesh.gumroad.com`.

The scanner checks UTF-8 and UTF-16 encodings and inspects regular files plus ZIP-compatible package entries.

Repository contract tests should verify:

- Gumroad appears in required repository documentation/metadata;
- Gumroad does not appear under `src/CareNest.App`;
- the payload scanner includes the Gumroad marker;
- no Gumroad purchase is documented as a health entitlement.

## Messaging rules

Approved concepts:

- “Shop on Gumroad.”
- “Visit the Ram Sandesh Gumroad store.”
- “Digital products, books, learning resources, and project material.”
- “The storefront is separate from CareNest health functionality.”

Do not imply that a Gumroad purchase provides:

- diagnosis;
- dosage decisions;
- treatment recommendations;
- clinical medication-interaction checking;
- medical-risk scoring;
- guaranteed reminder delivery;
- emergency assistance;
- access to another person’s health data;
- priority medical support.

## Visual treatment

Use `docs/assets/gumroad_store_badge.svg` for repository/documentation promotion.

The badge:

- uses a custom storefront illustration;
- displays the exact canonical URL;
- includes accessible SVG title/description text;
- is repository-only;
- is not part of application resources.

When the image cannot be rendered, always provide the plain-text canonical URL nearby.

## Link integrity

Canonical form:

`https://ramsandesh.gumroad.com`

Do not add tracking parameters to the canonical documentation link unless a future privacy review explicitly approves them. Do not use URL shorteners for the canonical repository documentation.

## Maintenance checklist

When updating Gumroad integration:

- [ ] keep the URL exact;
- [ ] keep the repository badge readable and accessible;
- [ ] keep a plain-text fallback link;
- [ ] keep the application runtime free of the destination;
- [ ] keep the payload scanner marker current;
- [ ] keep contract tests current;
- [ ] do not rewrite historical evidence;
- [ ] update `what_changed.md`;
- [ ] run UI/source-policy tests;
- [ ] run normal and store-safe platform builds;
- [ ] run CodeQL and applicable dependency/security gates.
