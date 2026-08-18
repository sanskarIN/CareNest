## What changed

Describe the smallest complete change and why it is needed.

## Scope and safety

- [ ] This change stays within CareNest's organizational/non-clinical product boundary.
- [ ] No diagnosis, dosage inference, treatment advice, medication-interaction claim, clinical-risk score, emergency-service claim, or guaranteed-reminder claim was added.
- [ ] No new remote data flow, account requirement, cloud sync, analytics, or telemetry was added without the required architecture/privacy/security review.
- [ ] No real health records, prescriptions, backups, PINs/passwords, cryptographic keys, signing secrets, or other private data are included in source, tests, screenshots, logs, or fixtures.
- [ ] Repository-only Gumroad/Buy Me a Coffee promotion has not leaked into the distributed application package/runtime.

## Data, migration, and compatibility impact

Describe database/schema, backup, encrypted-document, migration, package, or platform compatibility impact. Write `None` only when it is genuinely not applicable.

## Tests and automated checks

- [ ] Added or updated the lowest appropriate regression coverage.
- [ ] Platform-neutral formatting passes.
- [ ] Unit tests pass where applicable.
- [ ] Integration tests pass where applicable.
- [ ] UI/source-policy tests pass where applicable.
- [ ] `python3 build/scripts/test-create-package-evidence.py` passes when release/package tooling is affected.
- [ ] `python3 build/scripts/test-verify-documentation-links.py` passes when documentation tooling is affected.
- [ ] `python3 build/scripts/verify-documentation-links.py` passes for active documentation.
- [ ] Dependency Audit remains unsuppressed and green when package/dependency files change.

Record commands, workflow run IDs, and actual results below. Do not predict test counts.

## Platform validation

List Android, Windows, iOS/iPadOS, and Mac Catalyst validation actually performed. Distinguish compilation/simulator evidence from real-device behavior.

## Documentation and release impact

- [ ] User/developer/release documentation was updated when behavior, configuration, dependencies, tooling, privacy, security, packaging, or limitations changed.
- [ ] `CHANGELOG.md`, `PROJECT_STATUS.md`, release evidence, or `what_changed.md` was updated when the change affects the active release boundary.
- [ ] No historical evidence under `docs/history/` was rewritten to look current.

## Remaining limitations or blockers

List anything that still needs real-device, accessibility, production-signing, packaged-compatibility, live store-console, current store-policy, or publication evidence. Do not mark unperformed external validation as complete.
