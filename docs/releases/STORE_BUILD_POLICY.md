# Store Build Policy — Voluntary Project Support

## Purpose

CareNest is open source and may display an optional Buy Me a Coffee link for voluntary project support. Store policies can change, so release engineering must be able to hide that external support surface for a specific packaged build without forking the application or changing any health-organizer functionality.

This document defines the source-controlled build switch and the evidence required before a store submission.

## Product rule

Project support is voluntary.

It must never:

- unlock medical advice;
- unlock health features;
- alter reminder priority, timing, reliability, or permissions;
- expose user health data;
- alter support priority for health-related use;
- create an account or remote health-data relationship;
- be represented as a purchase of clinical, diagnostic, treatment, or emergency functionality.

The canonical support URL remains:

`https://buymeacoffee.com/sanskarIN`

Repository funding metadata may continue to reference the same URL where GitHub permits it.

## Build switch

`src/CareNest.App/CareNest.App.csproj` defines:

`CareNestShowFundingLink`

Default:

`true`

When the value is `true`, the project defines `CARENEST_FUNDING_LINK` and the About page exposes the voluntary support card.

When the value is `false`, that compile symbol is absent and `AboutViewModel.IsProjectSupportVisible` returns `false`, hiding the complete support card.

This changes only visibility of the voluntary external support surface. It must not change data, reminders, permissions, reports, encryption, backups, app lock, appointments, profiles, documents, or medical-safety wording.

## Direct build examples

Android build with the support surface visible:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android \
  -c Release \
  -p:CareNestTargetFramework=net10.0-android \
  -p:CareNestShowFundingLink=true \
  --nologo
```

Android build with the support surface hidden:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android \
  -c Release \
  -p:CareNestTargetFramework=net10.0-android \
  -p:CareNestShowFundingLink=false \
  --nologo
```

The same property applies to iOS, Mac Catalyst, and Windows targets.

## Release-preflight examples

Bash:

```bash
CARENEST_TARGET=net10.0-android \
CARENEST_SHOW_FUNDING_LINK=false \
./build/scripts/release-preflight.sh
```

PowerShell:

```powershell
$env:CARENEST_TARGET = 'net10.0-windows10.0.19041.0'
$env:CARENEST_SHOW_FUNDING_LINK = 'false'
./build/scripts/release-preflight.ps1
```

Accepted values are exactly `true` and `false`. The preflight scripts fail closed for any other value.

## Store-review decision

Before each Apple App Store or Google Play production submission:

1. Review the current store rule that applies to external voluntary support/donation/project-funding links.
2. Record the rule review date, source, reviewer, and conclusion in release evidence.
3. If the in-app external support link is permitted, build with `CareNestShowFundingLink=true`.
4. If the link is not permitted or its status is uncertain, build with `CareNestShowFundingLink=false`.
5. Verify the resulting packaged build on the actual target/store configuration.
6. Record the selected property value with the package checksum and source commit.

Do not infer store approval from an earlier release. Policy review is a per-release external gate.

## Required packaged verification

For a build with the link enabled:

- About shows the voluntary project-support card.
- The link opens only after explicit user action.
- The destination is the canonical `https://buymeacoffee.com/sanskarIN` URL.
- The surrounding text states that support is voluntary and unlocks no medical/health functionality.

For a build with the link disabled:

- About does not show the support image, button, URL, or explanatory support card.
- GitHub repository, creator, business email, support email, privacy, terms, security, and third-party-notice surfaces remain available.
- No health-organizer feature changes.

## Automated contracts

`tests/CareNest.UiTests/CriticalFlowContractTests.cs` protects the build-configurable visibility contract.

`tests/CareNest.UiTests/PackageMetadataContractTests.cs` protects package identity, platform permission/privacy metadata, local-first Android network boundaries, and required branding assets.

These tests reduce accidental source regressions but do not replace manual store-policy review or packaged target inspection.

## Release evidence fields

Record at minimum:

- source commit SHA;
- release tag when applicable;
- target framework;
- package/application identifier;
- application display version/build number;
- `CareNestShowFundingLink` value;
- store-policy review date and conclusion;
- package filename and SHA-256 checksum;
- signing/notarization provenance where applicable;
- manual About-page result;
- reviewer/date.
