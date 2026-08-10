# Logging Privacy Contract

CareNest handles user-entered health-organizer information. Application diagnostics must therefore remain useful without becoming another copy of sensitive records.

## Allowed diagnostic metadata

Runtime logs may include only low-sensitivity operational metadata that is necessary to understand product behavior, such as:

- fixed component/category names;
- safe state categories;
- boolean capability/availability status;
- exception type names without message or stack trace;
- migration/schema version numbers;
- non-user-entered build/version metadata.

## Prohibited log content

Do not log:

- medicine names, strength text, instruction text, prescriber/pharmacy notes;
- profile names, dates of birth, blood group, allergies/sensitivities or emergency-contact values;
- appointment titles/notes;
- document names, document bytes, decrypted contents or local/export file paths;
- reminder notes or medication-log notes;
- app-lock PINs, verifiers, backup passwords or cryptographic keys;
- backup contents;
- full exception objects, exception messages or stack traces from operations that can touch user data;
- CareNest record identifiers when an operational category is sufficient;
- URLs containing user data or identifiers.

## Exception handling

`SafeUiErrorService` shows caller-supplied safe user-facing messages. When an exception exists, only its type name is emitted to the logger.

`GlobalExceptionHandler` observes process-domain and unobserved-task failures and also logs only an exception type/category. It does not log raw exception messages or stack traces.

Reminder scheduling error paths follow the same rule and do not include occurrence or medicine record identifiers.

## Diagnostics export

Developer diagnostics must remain redacted. Do not add document contents or user-entered health text to exported diagnostics for convenience.

Before adding a new log statement, ask whether the same troubleshooting goal can be met with a fixed category, state flag or exception type. Prefer the least revealing form.

## Automated enforcement

UI-contract/policy tests scan runtime source for unsafe exception-object logger calls and protect known reminder/error logging boundaries. Those tests are preventive controls, not permission to log any content that merely passes a text scan.
