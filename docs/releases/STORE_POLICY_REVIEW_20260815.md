# CareNest Store Support-Link Policy Review — 2026-08-15

## Purpose

This document records the 2026-08-15 release-engineering review of the optional CareNest Buy Me a Coffee project-support link for Apple App Store and Google Play distribution.

It records a conservative package configuration decision. It is not legal advice, a store approval, or a substitute for the policy review performed at the actual submission date.

## Product boundary

CareNest project support is voluntary.

Contributing must not:

- unlock medical advice;
- unlock health-organizer functionality;
- unlock premium health features;
- alter reminder priority, timing, delivery claims, or permission behavior;
- provide access to user health data;
- alter emergency behavior;
- alter treatment, dosage, diagnosis, medication-interaction, or clinical-risk behavior;
- create an account or remote health-data relationship.

The canonical external project-support URL is:

`https://buymeacoffee.com/sanskarIN`

## Source-controlled package switch

CareNest defines:

`CareNestShowFundingLink`

Default:

`true`

For a store package where the external support surface should not appear:

`CareNestShowFundingLink=false`

Release-preflight equivalent:

`CARENEST_SHOW_FUNDING_LINK=false`

The false setting hides the complete About-page Buy Me a Coffee support card without changing organizer functionality.

See:

- `docs/releases/STORE_BUILD_POLICY.md`;
- `docs/releases/PACKAGED_RELEASE_VALIDATION.md`;
- `docs/releases/PACKAGED_RELEASE_HARDENING_VERIFICATION_20260815.md`.

## Apple review — 2026-08-15

Reviewed source:

`https://developer.apple.com/app-store/review/guidelines/`

Relevant current Apple App Review Guideline reviewed:

- Guideline 3.2.1(vii) describes an exception for optional monetary gifts between individuals outside in-app purchase when the gift is completely optional and 100% of the funds go to the receiver; if the gift is connected to digital content or services, in-app purchase requirements apply.

CareNest does not tie project support to digital health functionality or entitlement.

However, the selected third-party support provider itself charges a transaction fee, so the literal 100%-to-receiver condition is not clearly satisfied by the current Buy Me a Coffee flow.

### Apple release decision

For the initial Apple App Store package, use:

`CareNestShowFundingLink=false`

unless a current storefront-specific policy review, approved program, or explicit store-review outcome establishes that the external Buy Me a Coffee support link is permitted for the submitted package.

This decision is intentionally conservative and avoids representing the generic optional-gift exception as broader than the published condition.

## Google Play review — 2026-08-15

Reviewed source:

`https://support.google.com/googleplay/android-developer/answer/10281818`

Relevant current Google Play Payments policy guidance reviewed:

- direct tips or contributions can fall outside Play Billing when 100% of the contribution goes to the creator and the payment grants no digital content or services;
- payments connected to digital content/services remain subject to the applicable Play Payments rules and any available country/program-specific alternatives.

CareNest does not tie project support to digital health functionality or entitlement.

However, the selected third-party support provider charges a transaction fee, so the literal 100%-to-creator condition is not clearly satisfied by the current Buy Me a Coffee flow.

### Google Play release decision

For the initial Google Play package, use:

`CareNestShowFundingLink=false`

unless a current storefront/country/program-specific policy review or explicit store-review outcome establishes that the external Buy Me a Coffee support link is permitted for the submitted package.

This keeps the default open-source/direct-distribution behavior separate from the stricter store-package decision.

## Buy Me a Coffee provider fact reviewed — 2026-08-15

Reviewed source:

`https://help.buymeacoffee.com/en/articles/4539170-frequently-asked-questions`

The provider currently states that it charges a 5% transaction fee and that creators keep 95% of earnings, before considering any separately applicable payment-processing details described by the provider.

Because the official Apple and Google exceptions reviewed above use literal 100%-to-receiver/creator wording, CareNest release engineering must not assume that the Buy Me a Coffee flow qualifies for those exceptions.

## Current package policy

Until a later submission-time review establishes otherwise:

### Normal/open-source/direct development builds

Default may remain:

`CareNestShowFundingLink=true`

subject to the rules of the actual distribution channel.

### Apple App Store production candidate

Use:

`CareNestShowFundingLink=false`

### Google Play production candidate

Use:

`CareNestShowFundingLink=false`

## Verification required on the actual package

The policy decision is not complete merely because source contains the switch.

For each submitted store artifact:

- [ ] record source commit;
- [ ] record target framework;
- [ ] record application identifier/version/build number;
- [ ] record `CareNestShowFundingLink=false`;
- [ ] compute and record package SHA-256 where the artifact is directly handled;
- [ ] install/open the actual candidate package;
- [ ] verify the About page contains no Buy Me a Coffee support image/button/URL/card;
- [ ] verify repository, creator, business email, support email, privacy, terms, security, and third-party notices remain available;
- [ ] verify no health-organizer behavior differs from a normal build;
- [ ] record policy-review date/source/reviewer/conclusion;
- [ ] retain signing/notarization/store provenance without committing secrets.

## Re-review rule

Store policies, country-specific programs, entitlement rules, and payment-system requirements can change.

At the actual submission date:

1. re-open the current official Apple or Google policy;
2. review rules applicable to the target storefront/country/program;
3. record the review date and conclusion;
4. keep `CareNestShowFundingLink=false` if the external link remains disallowed, unclear, or dependent on an unavailable/unapproved program;
5. enable it only when the submitted package is clearly permitted to do so;
6. verify the actual packaged result.

Do not infer approval from an earlier review, another app, a community answer, or a previous release.

## Relationship to automated verification

PR #58 verifies that the source-controlled support-link switch, package metadata/privacy contracts, release-preflight propagation, retained application source, and retained dependency graph pass the automated CareNest matrix.

Frozen verified source:

`826b79925dad4402f65fccfecd4a29b353b6e2f3`

PR #58 automated result:

- 291/291 core tests passed;
- Android Release passed;
- Windows Release passed;
- iOS simulator Release passed;
- Mac Catalyst Release passed;
- CodeQL passed;
- unsuppressed Dependency Audit passed.

Automated verification does not constitute Apple or Google approval and does not replace packaged inspection.

## Remaining status

Source-side mitigation and this dated policy review are complete.

Still open:

- actual App Store candidate build with the external support surface hidden;
- actual Google Play candidate build with the external support surface hidden;
- packaged About-page inspection;
- signing and distribution credentials outside Git;
- store metadata/data-safety/privacy submission review;
- final store review/approval;
- re-review if policy changes before submission.
