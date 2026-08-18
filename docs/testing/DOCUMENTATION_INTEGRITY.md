# CareNest Documentation Integrity

CareNest treats stable active documentation as part of release quality. Broken repository-local links can hide privacy, security, build, support, or release instructions even when application code compiles successfully.

## Source-controlled checker

Run:

```bash
python3 build/scripts/verify-documentation-links.py
```

The checker validates **live** repository-local destinations referenced from stable tracked Markdown files, including normal Markdown links/images, HTML `href`/`src` attributes, and reference-style link definitions.

It intentionally ignores link-like text inside:

- fenced Markdown code blocks;
- inline code spans;
- HTML comments.

Those forms are examples or non-rendered content rather than live document navigation. This matters for snippets that intentionally demonstrate paths relative to a different embedding location, such as a repository-root README example shown from a nested documentation file.

The checker intentionally does **not** make network requests. External `http`, `https`, `mailto`, `tel`, and similar URI availability belongs to current/manual release review so normal CI does not become dependent on third-party uptime.

## What fails the check

The checker fails closed when a checked live local documentation link:

- points to a missing file or directory;
- resolves outside the repository root;
- cannot be represented as a valid repository-local destination under the supported link forms.

Fragments and query strings are removed before local filesystem validation, so a link such as `USER_GUIDE.md#reminders` verifies the document target without pretending to validate Markdown anchor rendering.

## Example-only versus live-link rule

A code sample such as:

```html
<img src="docs/assets/gumroad_store_badge.svg" />
```

is not interpreted as a live link from the Markdown file that contains the example. The example may intentionally be written for copying into a repository-root document.

By contrast, this rendered Markdown link is live and must resolve relative to the source document:

[Documentation catalog](../DOCUMENTATION_CATALOG.md)

When a real rendered link is broken, fix the document target/path. When only an example is involved, keep the example semantically correct for its documented context rather than changing it merely to satisfy filesystem-relative scanning.

## Dynamic post-verification evidence boundary

These files are deliberately excluded from the default exact-source documentation gate:

- `PROJECT_STATUS.md`;
- `what_changed.md`;
- `docs/releases/AUTOMATED_BASELINE.md`;
- `docs/releases/NEXT_STEPS.md`.

They are dynamic evidence/status records. A successful exact-source verification must be able to record its own source SHA, workflow IDs, observed test counts, and follow-up state without that evidence write becoming a new executable verification input and creating an infinite re-verification loop.

To audit those records explicitly without changing the stable-source boundary:

```bash
python3 build/scripts/verify-documentation-links.py --include-dynamic
```

A dynamic documentation-only audit is useful after evidence promotion, but it does not retroactively change which source revision produced the recorded runtime/test/platform evidence.

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

- valid live local Markdown links pass;
- valid live local images pass;
- live HTML links are checked;
- live reference-style destinations are checked;
- remote/mail/anchor-only links are skipped;
- link-like content inside fenced code, inline code and HTML comments is ignored;
- missing live local targets fail;
- repository-escaping paths fail;
- `docs/history/` is excluded by default but checked with `--include-history`;
- dynamic evidence/status files are excluded by default but checked with `--include-dynamic`.

No health data, network access, signing material, or production package is required.

## CI and release integration

CareNest CI performs:

1. Python syntax validation for the documentation checker and self-test;
2. the synthetic self-test;
3. the stable active documentation live-link check.

The production Release Gate repeats the same requirements. The Release Evidence workflow retains the self-test and stable-link-check output under `artifacts/tooling/` and treats a stable documentation-integrity failure as a failed evidence component.

This boundary is intentionally consistent with `docs/releases/VERIFICATION_BRANCH_PROTOCOL.md`: mutable evidence/status data can be written after successful exact-source verification without becoming an executable baseline assertion.

## Contributor rule

When adding, moving, renaming, or deleting documentation or repository assets:

1. update every applicable stable active live local link in the same change;
2. run the default checker;
3. run `--include-dynamic` when changing dynamic evidence/status links;
4. keep example-only links semantically correct for the context they demonstrate;
5. do not fix current documentation by rewriting historical evidence;
6. use source-document-relative paths for rendered local links;
7. keep external URL verification separate from offline link integrity.
