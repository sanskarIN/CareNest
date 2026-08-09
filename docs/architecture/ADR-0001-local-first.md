# ADR-0001: Local-first v1

Status: Accepted

CareNest v1 stores application data locally and requires no account or network. This minimizes remote attack surface and supports offline operation. Remote caregiver collaboration is intentionally deferred because it requires identity, consent, revocation, encryption-key management, conflict handling, abuse prevention and a substantially different privacy model.
