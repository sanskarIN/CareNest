# PR #36 post-verification source-boundary evidence — 2026-08-13

## Verified source

`3b19ce08f509f27aca823469abc5b8a03ed2465a`

## Documentation head compared before this evidence refresh

`aaf8e06a9e1a1dbf0bccec88951073e997f52b32`

## GitHub comparison result

- status: ahead;
- commits after verified source at the recorded boundary: 12;
- runtime/test source commits after verified source: 0;
- changed paths after verified source: Markdown documentation/history only.

Changed paths at that boundary:

- `PROJECT_STATUS.md`
- `docs/history/PROJECT_STATUS_through_PR33.md`
- `docs/history/README_through_PR33.md`
- `docs/history/what_changed_through_pr33_20260813.md`
- `docs/privacy/LOCAL_PRIVACY_CLEANUP_LIFECYCLE.md`
- `docs/releases/CHANGELOG_PHASE9_20260813.md`
- `docs/releases/PHASE9_VERIFICATION_EVIDENCE.md`
- `docs/releases/POST_VERIFICATION_COMPARE_20260813.md`
- `docs/releases/SETTINGS_LIFECYCLE_VERIFICATION_20260813.md`
- `docs/security/FULL_LOCAL_DATA_CLEAR_SECURITY_MODEL.md`
- `docs/testing/SETTINGS_LIFECYCLE_CONTRACT.md`
- `what_changed.md`

No C#, XAML, `.csproj`, solution, workflow, package-management, signing/configuration, app-resource, runtime-test, or platform source changed after the exact PR #36 source.

The commit that refreshes this evidence file is itself Markdown-only. A final GitHub compare after this update is used to confirm that this remains true through the resulting `main` head.

## PR #36 evidence

- CareNest CI #362 / `31701943543`: success;
- 106 unit + 30 integration + 56 UI-contract = 192 total core tests;
- Android Release: success;
- Windows Release: success;
- iOS simulator Release: success;
- Mac Catalyst Release: success;
- CodeQL #362 / `31701943506`: success;
- Dependency Audit #16 / `31701943476`: success.

PR #36 was closed without merging its verification marker.

The SQLitePCLRaw advisory remains separately open and this comparison does not change that status.
