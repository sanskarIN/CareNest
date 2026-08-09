# Development Setup

## Required

- .NET 10 SDK with .NET MAUI workload.
- Visual Studio 2026 or compatible IDE on Windows, or current supported Visual Studio Code/Rider tooling.
- Android SDK/JDK for Android.
- Xcode on macOS for iOS/Mac Catalyst.
- Windows App SDK prerequisites for Windows.

```bash
dotnet workload install maui
dotnet restore CareNest.sln
```

## Build

```bash
dotnet build src/CareNest.Domain/CareNest.Domain.csproj
dotnet build src/CareNest.Application/CareNest.Application.csproj
dotnet build src/CareNest.Infrastructure/CareNest.Infrastructure.csproj
dotnet build src/CareNest.App/CareNest.App.csproj -f net10.0-android
```

Platform targets:

- `net10.0-android`
- `net10.0-ios`
- `net10.0-maccatalyst`
- `net10.0-windows10.0.19041.0`

## Tests

```bash
dotnet test tests/CareNest.UnitTests/CareNest.UnitTests.csproj
dotnet test tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj
dotnet test tests/CareNest.UiTests/CareNest.UiTests.csproj
```

## Git identity for maintainer

```bash
git config user.email "sanskarin@outlook.in"
git config user.name "Sanskar"
```

## Do not commit

Signing keys, certificates, keystores, passwords, exported backups, real documents, SQLite databases, or real user health data.
