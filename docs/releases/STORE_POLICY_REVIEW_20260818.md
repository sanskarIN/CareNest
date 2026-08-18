# CareNest Store Policy Review — 2026-08-18

**Review date:** 2026-08-18  
**Release line:** `1.0.0-rc.1`  
**Repository head at review start:** `2777c6079e6b8cfba7e6ad1a961e17fb3d01dd8b`  
**Latest verified Gumroad implementation/source-policy baseline:** `94e867dce9519a8c1c71f1c4f1e5f833d6a3211f`

This is a current pre-submission policy review for the CareNest release boundary. It is **not** store approval, legal advice, medical-device classification, production-signing evidence, or a substitute for the final policy check performed against the exact production package and listing at submission time.

## 1. Product boundary reviewed

CareNest remains an account-free, local-first organizational health application. The current release does not diagnose conditions, calculate or infer dosage, recommend treatment, perform clinical medication-interaction checking, calculate clinical risk, verify adherence, or provide emergency services.

Current source and documentation also keep Gumroad and Buy Me a Coffee promotion outside the distributed CareNest application package. Repository promotion does not unlock health functionality and does not give the storefront/funding service access to local CareNest health records.

The current app does not require a CareNest-owned cloud backend and does not automatically upload local health records to a CareNest server.

## 2. Official policy sources reviewed

### Apple

Primary source:

- App Review Guidelines: `https://developer.apple.com/app-store/review/guidelines/`

Relevant review areas include:

- Section 1.4 medical/physical-harm rules;
- the heightened scrutiny applied to apps that could be used for diagnosis or treatment;
- restrictions around drug dosage calculators;
- privacy-policy and consent requirements for sensitive health/medical data;
- the requirement that App Store submissions be complete and tested on-device;
- current purchase-link/external-commerce rules, which vary by storefront and entitlement context.

### Google Play

Primary sources:

- Health Content and Services: `https://support.google.com/googleplay/android-developer/answer/16679511`
- Health apps declaration: `https://support.google.com/googleplay/android-developer/answer/14738291`
- Data safety section: `https://support.google.com/googleplay/android-developer/answer/10787469`

Relevant review areas include:

- health apps must complete the Health apps declaration in Play Console;
- health/medical apps must provide an appropriate privacy policy;
- health-related functionality must not be misleading or harmful;
- non-regulated health/medical apps must use the required non-medical-device disclaimer language in the store description where applicable;
- users must be directed to qualified healthcare professionals for medical advice/diagnosis/treatment rather than relying on the app;
- Data safety answers must match actual collection/sharing behavior;
- unnecessary sensitive permissions must not be requested.

### Microsoft Store

Primary source:

- Microsoft Store Policies: `https://learn.microsoft.com/en-us/windows/apps/publish/store-policies`

Relevant review areas include:

- products handling Personal Information must maintain an accurate privacy policy;
- personal information must be handled securely;
- highly sensitive information such as health data must be related to product functionality and handled with appropriate consent and disclosures;
- privacy statements must explain what is accessed/collected, how it is used/stored/secured, and user controls where applicable.

## 3. Apple review against current CareNest boundary

### Current alignment

- CareNest does not implement diagnosis or treatment recommendations.
- CareNest does not implement a drug-dosage calculator.
- CareNest already carries a medical limitation statement and directs users to qualified professionals for medical decisions.
- CareNest maintains an accessible in-app path to `PRIVACY.md` through the About surface.
- Current application source does not include an in-app Gumroad/Buy Me a Coffee purchase or promotional call to action.
- Current local-first design avoids a CareNest cloud upload path for health records.

### Submission actions still required

- Re-check the App Review Guidelines on the actual submission date.
- Test the exact signed iPhone/iPad/Mac Catalyst packages on representative real devices.
- Confirm App Store Connect privacy answers match the exact binary and any enabled platform capabilities.
- Confirm the privacy-policy URL is publicly reachable from the listing and from the app.
- Confirm screenshots/listing copy do not imply diagnosis, dosage calculation, treatment recommendations, guaranteed reminders, medical-device status, or emergency-service behavior.
- Re-check external-commerce rules for every intended Apple storefront even though the current CareNest package intentionally contains no Gumroad/Buy Me a Coffee purchase CTA.

## 4. Google Play review against current CareNest boundary

### Current alignment

- CareNest is clearly a health-organizational app and should be treated as in-scope for the Health apps declaration rather than declaring that it has no health features.
- The current product intentionally avoids diagnosis, treatment, clinical decision support, dosage calculation and prescription-drug sales.
- The current product has a privacy notice describing local health information, local SQLite storage, encrypted document storage, explicit export/share boundaries, backups, notifications, diagnostics and deletion.
- The distributed package intentionally contains no Gumroad/Buy Me a Coffee promotion or purchase flow.
- No Health Connect/body-sensor integration is part of the current source boundary reviewed for this release.

### Submission actions still required

- Complete the Health apps declaration accurately in Play Console for the exact published feature set.
- Use the required non-medical-device disclaimer language in the Google Play description where applicable.
- Include a clear reminder to consult a healthcare professional for medical advice, diagnosis or treatment.
- Provide a publicly accessible, non-geofenced privacy-policy URL accepted by Play Console and make the privacy policy accessible from the app.
- Complete Data safety from the exact production binary and actual SDK/permission behavior rather than from assumptions.
- Verify the exact Android manifest and packaged binary request only permissions needed for the documented features.
- Re-check current health, privacy, permissions and payments/external-commerce policy immediately before submission.

## 5. Microsoft Store review against current CareNest boundary

### Current alignment

- Health information stored by CareNest is directly related to the app's organizing functionality.
- `PRIVACY.md` describes the sensitive local data categories, local storage, encrypted document vault, backup behavior, explicit export/share boundary, deletion behavior and current no-hidden-analytics boundary.
- The current product does not silently publish local health records to an outside service.
- The current product keeps repository storefront/funding promotion outside the Windows application package.

### Submission actions still required

- Re-check the Microsoft Store Policies immediately before submission.
- Verify Partner Center privacy-policy metadata uses a publicly reachable current privacy URL.
- Verify the exact Windows package, capabilities and permissions match the privacy notice and store declaration.
- Complete real installed-package privacy, reminder, keyboard/focus and accessibility checks.
- Record production signing identity/provenance outside Git and the final package SHA-256.

## 6. Store-listing wording boundary

For every store listing, keep the product positioned as an **organizational health application**, not a clinical or medical-decision product.

Listing copy must not claim or imply that CareNest:

- diagnoses a condition;
- determines, calculates or infers dosage;
- recommends a treatment;
- performs clinical interaction checking;
- calculates clinical risk;
- guarantees medication adherence;
- guarantees reminder delivery;
- replaces a clinician/pharmacist;
- provides emergency services;
- is a regulated medical device unless a future release actually receives the required classification/approval and the repository is deliberately updated for that new scope.

## 7. Google Play health declaration preparation

The exact Play Console form must be completed by the release owner, but the current source boundary supports the following preparation rules:

- do **not** select “My app doesn’t provide any health features”;
- describe CareNest as an organizational medicine/reminder/appointment/document-management application;
- do not select clinical decision support, medical-device functionality or diagnosis/treatment behavior unless the source scope changes and a separate review justifies it;
- ensure the declaration remains consistent with the final store description, privacy policy, permissions and exact production binary.

No declaration answer in this repository should be treated as a substitute for reviewing the live Play Console form at submission time.

## 8. Privacy/data-safety preparation

Current source-level facts to carry into store forms, subject to final binary verification:

- local health records are user-entered and stored on-device;
- the app does not require a CareNest account or CareNest-owned health backend;
- imported document payloads use the encrypted CareNest document-vault path;
- SQLite structured data relies on the app sandbox/device protection and is not claimed to be transparently whole-database encrypted;
- backups are manual and password-encrypted;
- exports/shares are explicit user actions and can move copies outside CareNest protection;
- the current release does not include hidden analytics/telemetry;
- repository Gumroad/Buy Me a Coffee promotion is separate from the application package.

Final Apple privacy nutrition labels, Google Data safety answers and Microsoft privacy declarations must be completed against the exact production binary and enabled SDK/capability set.

## 9. External-commerce decision

For `1.0.0-rc.1`, keep the existing conservative package policy:

- no in-app Gumroad destination;
- no in-app Buy Me a Coffee destination;
- no Gumroad/BMC purchase button or promotional card in the health app;
- no health feature entitlement based on purchase/funding state;
- repository documentation may continue to promote the external storefront/funding destinations separately.

This avoids making a release dependent on storefront-specific external-purchase-link exceptions and keeps the current health application package simpler to review.

## 10. Status of this review

Completed on 2026-08-18:

- current Apple App Review Guidelines reviewed for CareNest medical/privacy/completeness/external-commerce boundaries;
- current Google Play health-app, declaration and Data safety guidance reviewed;
- current Microsoft Store personal/sensitive-information policy reviewed;
- existing CareNest local-first, non-clinical and repository-only external-commerce boundaries compared with those requirements;
- release actions that still require the exact production package/store console/real device were separated from source-level findings.

Still blocking production:

- exact production signed packages;
- real-device/platform validation;
- packaged data/encryption compatibility validation;
- accessibility validation;
- exact store-console declarations/metadata;
- submission-day policy re-check;
- store review/approval/publication evidence.

## 11. Review rule

Store policy is time-sensitive. This dated review is evidence of a 2026-08-18 pre-submission check only. Re-open the official sources and repeat the review immediately before submitting the exact production package, and again whenever a store rejection, policy notice, new SDK/capability, in-app commerce change, cloud/analytics change or clinical-scope change occurs.
