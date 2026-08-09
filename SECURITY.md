# Security Policy

## Supported versions

Security fixes are prioritized for the newest release line.

## Reporting

Do not open a public issue for a vulnerability that could expose health records, backups, lock credentials, documents, or notification contents. Report privately to:

- Business/security contact: `sanskarin@outlook.in`
- Support: `supportramsandesh@gmail.com`

Include affected version, platform, reproduction conditions, impact, and suggested mitigation. Do not include real user health data.

## Security design

- No required account or server in v1.
- Imported health documents are encrypted locally with AES-256-GCM.
- Backup files are encrypted with password-derived AES-256-GCM keys.
- PINs are not stored directly; the app stores a salted password-derived verifier in secure platform storage.
- Structured logs redact sensitive content.
- SQLite records rely on platform application sandbox protections; CareNest does **not** claim transparent database encryption at rest.
- No production secrets belong in source control.

See `docs/security/THREAT_MODEL.md` for boundaries and residual risks.
