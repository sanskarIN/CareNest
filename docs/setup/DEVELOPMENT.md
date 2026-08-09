# Development Setup

## Required

- .NET 10 SDK with the .NET MAUI workload for the platform you build.
- Visual Studio 2026 or compatible IDE on Windows, or current supported Visual Studio Code/Rider tooling.
- Android SDK/JDK for Android.
- Xcode compatible with the installed .NET iOS/Mac Catalyst workload on macOS.
- Windows App SDK prerequisites for Windows.

Install the full MAUI workload when the machine is intended to build every supported target available on that operating system:

```bash
dotnet workload install maui
dotnet restore CareNest.sln
```

For narrower CI/development machines, install only the required workload, for example `maui-android`, `maui-ios`, or `maui-maccatalyst`.

## Build

Build the platform-neutral layers first:

```bash
dotnet build src/CareNest.Domain/CareNest.Domain.csproj -c Release
dotnet build src/CareNest.Application/CareNest.Application.csproj -c Release
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj -c Release
```

CareNest is a multi-target MAUI app. The `CareNestTargetFramework` property intentionally narrows the app project before restore, so a runner does not need unrelated platform workloads and the selected target does not leak into referenced `net10.0` libraries.

Android:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

Windows:

```powershell
dotnet build src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0
```

iOS simulator:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-ios -c Release \
  -p:CareNestTargetFramework=net10.0-ios \
  -p:RuntimeIdentifier=iossimulator-arm64
```

Mac Catalyst:

```bash
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-maccatalyst -c Release \
  -p:CareNestTargetFramework=net10.0-maccatalyst
```

Supported platform targets:

- `net10.0-android`
- `net10.0-ios`
- `net10.0-maccatalyst`
- `net10.0-windows10.0.19041.0`

The current GitHub-hosted Apple verification uses a macOS 26 runner because the .NET 10 Apple workload installed by CI requires a matching current Xcode toolchain. If a local workload reports an Xcode-version mismatch, update/select a supported Xcode version rather than bypassing the workload compatibility check.

## Tests

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj -c Release
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj -c Release
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj -c Release
```

## Git identity for maintainer

```bash
git config user.email "sanskarin@outlook.in"
git config user.name "Sanskar"
```

## Do not commit

Signing keys, certificates, keystores, passwords, exported backups, real documents, SQLite databases, or real user health data.
