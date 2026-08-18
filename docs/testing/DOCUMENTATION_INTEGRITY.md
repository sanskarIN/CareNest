# CareNest Documentation Integrity

CareNest treats active documentation as part of release quality. Broken repository-local links can hide privacy, security, build, support, or release instructions even when application code compiles successfully.

## Source-controlled checker

Run:

```bash
python3 build/scripts/verify-documentation-links.py
```

The checker validates repository-local destinations referenced from active tracked Markdown files, including normal Markdown links/images, HTML `href`/`src` attributes, and reference-style link definitions.

It intentionally does **not** make network requests. External `http`, `https`, `mailto`, `tel`, and similar URI availability belongs to current/manual release review so normal CI does not become dependent on third-party uptime.

## What fails the check

The checker fails closed when an active local documentation link:

- points to a missing file or directory;
- resolves outside the repository root;
- cannot be represented as a valid repository-local destination under the supported link forms.

Fragments and query strings are removed before local filesystem validation, so a link such as `USER_GUIDE.md#reminders` verifies the document target without pretending to validate Markdown anchor rendering.

## Historical snapshots

`docs/history/` is excluded by default because it contains immutable historical evidence whose old links must not be rewritten merely to look current.

To audit history explicitly:

```bash
python3 build/scripts/verify-documentation-links.py --include-history
```

A historical failure is evidence about that snapshot; do not silently modify old verification records to satisfy the current active-doc contract.

## Synthetic self-test

Run:

```bash
python3 build/scripts/test-verify-documentation-links.py
```

The self-test uses a temporary synthetic documentation tree and proves that:

- valid local Markdown links pass;
- valid local images pass;
- HTML links are checked;
- reference-style destinations are checked;
- remote/mail/anchor-only links are skipped;
- missing local targets fail;
- repository-escaping paths fail;
- `docs/history/` is excluded by default.

No health data, network access, signing material, or production package is required.

## CI and release integration

CareNest CI performs:

1. Python syntax validation for the documentation checker and self-test;
2. the synthetic self-test;
3. the active documentation link check.

The production Release Gate repeats the same requirements. The Release Evidence workflow retains the self-test and active-link-check output under `artifacts/tooling/` and treats a documentation-integrity failure as a failed evidence component.

## Contributor rule

When adding, moving, renaming, or deleting documentation or repository assets:

1. update every active local link in the same change;
2. run the checker;
3. do not fix current documentation by rewriting historical evidence;
4. use repository-relative paths that work from the source Markdown file;
5. keep external URL verification separate from offline link integrity.
