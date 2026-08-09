# ADR-0002: Materialized reminder occurrences

Status: Accepted

CareNest stores future reminder occurrences rather than treating schedules as ephemeral timers. Materialization supports idempotent scheduling, missed-state reconciliation, auditability and recovery after updates/time-zone changes. Occurrence generation is based only on explicit user schedule input and never derives dosage.
