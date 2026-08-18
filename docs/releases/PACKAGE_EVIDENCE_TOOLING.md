# CareNest Package Evidence Tooling

**Release line:** `1.0.0-rc.1`  
**Tool:** `build/scripts/create-package-evidence.py`  
**Shell wrapper:** `build/scripts/create-package-evidence.sh`  
**PowerShell wrapper:** `build/scripts/create-package-evidence.ps1`

This guide documents the source-controlled tooling used to create checksum/provenance evidence for CareNest inspection artifacts and final production packages.

The tooling does **not**:

- sign an application;
- create or recover signing credentials;
- prove that a package was submitted to a store;
- prove store approval;
- replace real-device testing;
- replace packaged database/encryption compatibility testing;
- replace accessibility validation;
- replace submission-day store-policy review;
- prove that a signing-provenance description is truthful by itself.

It provides a consistent, fail-closed evidence record that can be attached to the larger release record.

## 1. What the evidence tool records

For a package file or published package directory, the JSON evidence contains:

- evidence schema version;
- generation timestamp in UTC;
- evidence stage (`inspection` or `production`);
- target platform;
- display/release version;
- build/version number;
- package/application identity;
- full source commit SHA;
- release tag when supplied;
- tracked-workspace clean/dirty state;
- non-secret signing/notarization/store-managed provenance description;
- payload filename/directory name;
- payload kind (`file` or `directory`);
- payload file count;
- payload byte count;
- SHA-256 for every payload file;
- top-level payload SHA-256;
- store-safe payload scan result;
- optional non-sensitive operator notes.

For a single package file, the top-level payload SHA-256 is that file's SHA-256.

For a directory, the tool creates a deterministic aggregate digest from the sorted relative file path, file SHA-256 and file size for every contained file. The per-file manifest remains the primary detailed evidence.

## 2. Store-safe scan is mandatory

Evidence generation invokes:

`build/scripts/verify-store-safe-payload.py`

The current default forbidden repository-only markers are:

```text
buymeacoffee.com/sanskarIN
ramsandesh.gumroad.com
```

If the scanner cannot inspect the payload or finds either default marker, evidence generation fails and the JSON evidence file is not written.

This is defense-in-depth for the current CareNest product policy that keeps Gumroad and Buy Me a Coffee promotion outside the distributed health-application package.

## 3. Inspection mode

Inspection mode is intended for unsigned/internal/non-production build evidence.

It:

- validates the supplied source SHA when supplied;
- otherwise records the checked-out repository HEAD;
- records whether tracked repository files are clean;
- runs the store-safe payload scanner;
- hashes the payload;
- writes the JSON evidence outside the payload path.

Inspection mode does not require a release tag and may describe an unsigned/internal artifact in `--signing-provenance`.

### Linux/macOS example

```bash
./build/scripts/create-package-evidence.sh \
  artifacts/android/CareNest.aab \
  --platform android \
  --version 1.0.0-rc.1 \
  --build 1 \
  --package-id com.sanskar.carenest \
  --stage inspection \
  --signing-provenance "unsigned CI inspection artifact" \
  --output artifacts/evidence/android-inspection.json
```

### PowerShell example

```powershell
./build/scripts/create-package-evidence.ps1 `
  artifacts/windows/CareNest `
  --platform windows `
  --version 1.0.0-rc.1 `
  --build 1 `
  --package-id com.sanskar.carenest `
  --stage inspection `
  --signing-provenance "unpackaged CI inspection output" `
  --output artifacts/evidence/windows-inspection.json
```

## 4. Production mode

Production mode is intentionally stricter.

It requires:

- `--stage production`;
- a full source SHA or the checked-out HEAD;
- `--source-tag` beginning with `v`;
- the supplied `v*` tag to resolve to the recorded source SHA;
- the checked-out repository HEAD to equal the recorded source SHA;
- no tracked workspace changes;
- a non-empty signing/notarization provenance description that is not labelled unsigned/not-applicable;
- successful store-safe payload scanning;
- an output location outside the package payload.

This prevents the evidence tool from creating a production-labelled manifest while checked out at a different source than the release tag.

It still cannot cryptographically prove that the human-readable signing-provenance description itself is truthful. Signing verification or store-managed provenance must be retained through the appropriate platform/store mechanism as additional evidence.

### Android production example

```bash
./build/scripts/create-package-evidence.sh \
  release/CareNest-1.0.0.aab \
  --platform android \
  --version 1.0.0 \
  --build 1 \
  --package-id com.sanskar.carenest \
  --stage production \
  --source-tag v1.0.0 \
  --signing-provenance "Google Play App Signing; certificate fingerprint recorded in private release log" \
  --output release/evidence/CareNest-1.0.0-android.json
```

Do not put keystore passwords, private keys, service credentials or other secrets in `--signing-provenance` or `--notes`.

### Windows production example

```powershell
./build/scripts/create-package-evidence.ps1 `
  release/CareNest-1.0.0.msix `
  --platform windows `
  --version 1.0.0 `
  --build 1 `
  --package-id com.sanskar.carenest `
  --stage production `
  --source-tag v1.0.0 `
  --signing-provenance "Production code-signing certificate public thumbprint recorded separately" `
  --output release/evidence/CareNest-1.0.0-windows.json
```

### iOS production example

```bash
./build/scripts/create-package-evidence.sh \
  release/CareNest-1.0.0.ipa \
  --platform ios \
  --version 1.0.0 \
  --build 1 \
  --package-id com.sanskar.carenest \
  --stage production \
  --source-tag v1.0.0 \
  --signing-provenance "App Store distribution signing/provisioning identifiers recorded separately" \
  --output release/evidence/CareNest-1.0.0-ios.json
```

### Mac Catalyst production example

```bash
./build/scripts/create-package-evidence.sh \
  release/CareNest-1.0.0.pkg \
  --platform maccatalyst \
  --version 1.0.0 \
  --build 1 \
  --package-id com.sanskar.carenest \
  --stage production \
  --source-tag v1.0.0 \
  --signing-provenance "Developer ID/App Store signing and notarization record retained separately" \
  --output release/evidence/CareNest-1.0.0-maccatalyst.json
```

## 5. Source SHA behavior

If `--source-sha` is omitted, the tool uses:

```text
git rev-parse HEAD
```

If `--source-sha` is supplied, it must:

- be a full 40-character hexadecimal SHA;
- resolve to a real commit in the current repository.

Production mode additionally requires the checked-out HEAD and supplied `v*` tag to resolve to that same SHA.

## 6. Output placement rule

The evidence JSON must not be written inside the payload directory being hashed.

For a package file, the evidence JSON must not replace the package file itself.

This prevents the evidence generator from changing the payload after/during hashing and creating unstable self-referential evidence.

## 7. Directory hashing rule

For published directory payloads, each file is processed in sorted relative-path order.

The aggregate digest receives, for each file:

```text
relative-path NUL file-sha256 NUL file-size LF
```

The JSON also stores every individual relative path, byte size and SHA-256 so the aggregate digest can be independently reconstructed.

## 8. Failure behavior

The tool returns non-zero and does not create successful evidence when, among other conditions:

- the payload path does not exist;
- the payload directory contains no files;
- a file cannot be stat'ed or read;
- the source SHA is invalid or cannot be resolved;
- the store-safe scanner is missing;
- the store-safe scan fails;
- a forbidden external-commerce marker is found;
- the evidence output would be inside the payload;
- production mode has no `v*` source tag;
- the production tag does not resolve to the source SHA;
- production HEAD does not match the source SHA;
- the production tracked workspace is dirty;
- production signing provenance is empty/unsigned/not-applicable;
- the JSON evidence cannot be written atomically.

## 9. Synthetic self-test

Run:

```bash
python3 build/scripts/test-create-package-evidence.py
```

The self-test uses temporary synthetic payloads only and verifies:

- a safe single-file package produces the expected SHA-256 evidence;
- a safe directory produces deterministic sorted file evidence;
- a Gumroad marker fails closed;
- evidence output inside a payload directory is rejected;
- production mode without a `v*` tag is rejected.

CareNest CI runs Python syntax validation and this self-test before the .NET formatting/test steps.

## 10. Evidence storage and privacy

The generated JSON is designed to contain technical provenance, not private user data.

Do not put into evidence:

- real health records;
- prescription document content;
- names/contacts from real users;
- app-lock PINs;
- backup passwords;
- encryption keys;
- private signing keys/certificates;
- keystore passwords;
- Apple/Google/Microsoft account credentials;
- service tokens.

If a package filename or operator note could reveal sensitive information, rename/sanitize it before generating public evidence.

## 11. Relationship to final release evidence

The package evidence JSON should be retained alongside:

- exact tagged CI/CodeQL/dependency/store/release workflow run IDs;
- Release Evidence artifact/checksums;
- platform signing/notarization/store-managed provenance;
- manual device test records;
- packaged SQLite/encrypted-document/backup compatibility records;
- accessibility evidence;
- submission-date store-policy review;
- live store-console health/privacy/data-safety declarations;
- final store approval/publication evidence.

A package evidence JSON by itself is never sufficient to mark CareNest production-published or store-approved.
