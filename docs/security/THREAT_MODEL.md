# Threat Model

## Assets

- Local profile information.
- User-entered medicine and appointment information.
- Medication logs.
- Imported health documents.
- Backup archives.
- App-lock verifier and document encryption key.

## Primary threats and controls

| Threat | Control | Residual risk |
|---|---|---|
| Casual access to an unlocked device | Optional app lock; generic notifications | OS snapshots, device compromise |
| Stolen app files | OS sandbox; encrypted document bytes | SQLite database is not transparently encrypted |
| Backup theft | Password-derived AES-GCM encryption | Weak user passwords can reduce practical protection |
| Tampered backup | AEAD authentication + format/version validation | Denial of service remains possible |
| Leaked document content in logs | Redaction rules; never log file bytes/notes | Third-party OS logs outside app control |
| Duplicate reminders | Stable occurrence keys + idempotent upsert/schedule | OS notification subsystem can still behave differently |
| Missed reminders | diagnostics, permission checks, rebuild, battery/exact-alarm warnings | shutdown, force-stop, policy restrictions |
| Malicious imported file | treat as opaque bytes; no interpretation/execution | vulnerable external viewer after export |
| Rooted/jailbroken device | explicit limitation | stronger attacker can bypass sandbox/secure store |
| Shoulder surfing | lock + generic notification title | visible screen remains visible to nearby people |
| External repository/policy/funding link | fixed HTTPS destinations, explicit user action, no health-data query parameters or automatic record upload | external sites have their own privacy, account, cookie, payment and availability risks |

## External voluntary-support boundary

The Buy Me a Coffee destination is fixed as `https://buymeacoffee.com/sanskarIN` through `AppConstants.FundingUrl` and is opened only after explicit user interaction. CareNest does not append profile IDs, medicine names, document metadata, reminder history, backup data, app-lock information, or other local health content to the URL.

The funding provider is outside the CareNest trust boundary. Browser/network metadata and any information/payment details the user chooses to provide there are governed by that external service, not by CareNest.

No embedded payment SDK, payment token, API secret, or funding-provider credential is stored in the CareNest source/runtime for this link.

## Out of scope for v1

- Server compromise, because no CareNest backend exists.
- Cloud sharing/caregiver synchronization.
- Clinical correctness or medical decision support.
- Security/privacy guarantees for independently opened external websites after the user leaves the CareNest app surface.

## Security review triggers

A new review is mandatory before adding accounts, remote sync, analytics, crash uploads containing user state, document interpretation, sharing by default, medical decision support, an embedded web view for external services, payment/funding SDKs, purchase entitlements, or any external-link flow that transmits CareNest user data.
