# Store Asset Guidance

Use vector sources in `src/CareNest.App/Resources/AppIcon` and `Resources/Splash`.

- Store icon: render `appicon.svg` at each store-required size with no text.
- Feature graphic: calm background, CareNest shield/nest mark, product name, and short phrase such as “Local-first health organization”.
- Screenshots must use fictional test data only.
- Never show real prescriptions or health documents.
- Avoid red-cross or official-accreditation symbolism.
- Keep “Made by the Sanskar” subtle in the About/splash branding, not on screenshots containing user content.
- Do not advertise the Buy Me a Coffee link as a purchase of medical functionality, premium reminder behavior, emergency support, or access to CareNest health data.
- Before including any external project-support link in a store listing or store-distributed binary, verify the current rules for that specific distribution channel. Store policies can change.
- If a store disallows the in-app external voluntary-support link, use a compliant platform-specific build configuration rather than misrepresenting the purpose of the link.
- Keep store privacy/data-safety answers aligned with the actual local-first runtime behavior and `PRIVACY.md`.

See `docs/releases/NEXT_STEPS.md` and `docs/releases/RELEASE_CHECKLIST.md` before preparing production store assets/submissions.
