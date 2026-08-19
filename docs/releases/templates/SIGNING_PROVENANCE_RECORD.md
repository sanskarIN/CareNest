# CareNest Signing and Provenance Record

Do not commit private keys, keystore passwords, certificate private material, access tokens, recovery codes or other signing secrets.

## Identity

- Result status: `NOT RUN`
- Platform:
- CareNest version/build:
- Source SHA:
- Immutable source tag:
- Package filename/path:
- Package SHA-256:
- Package/application identifier:
- Package-evidence JSON:
- Validation date/time/time zone:
- Operator:

## Signing boundary

- Signing method/service:
- Safe public certificate/signing identity description:
- Safe public certificate fingerprint/serial/team ID where appropriate:
- Timestamping/notarization/store-managed signing service:
- Signing timestamp:
- Signing environment/channel:

## Verification

- [ ] Package/source identity matches the intended release candidate.
- [ ] Signing occurred outside Git with no private material committed.
- [ ] Signature verification succeeds using platform-supported tooling.
- [ ] Package remains installable/launchable after signing.
- [ ] Package SHA-256 is recorded after the final signing/notarization transformation.
- [ ] Package-evidence JSON references the exact final payload.
- [ ] Recorded source tag resolves to the recorded source SHA.
- [ ] Production-stage evidence uses an immutable `v*` tag.
- [ ] Non-secret provenance values are sufficient to identify the signing boundary.

Evidence/commands/results:

## Platform-specific notes

### Android

- Keystore/private key location recorded only outside repository: `YES/NO/N/A`
- APK/AAB signature verification result:
- Play App Signing involvement, if applicable:

### Apple

- Distribution method:
- Team ID / safe public identity:
- Provisioning/distribution profile type:
- Notarization result for Mac Catalyst where applicable:

### Windows

- Signing identity/provider:
- Signature verification result:
- Timestamp verification result:

## Secret-safety review

- [ ] No private key material is present in this record.
- [ ] No keystore/certificate password is present.
- [ ] No access token/service-account private credential is present.
- [ ] No recovery code is present.
- [ ] No real health/user data is present.

## Failures/blockers

List every `FAIL`, `BLOCKED` or `N/A` row with reason and issue/PR reference where applicable.

## Final result

- Overall result: `NOT RUN`
- Blocking issue references:
- Retest/re-sign package if required:
- Reviewer/sign-off:
