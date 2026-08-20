# CareNest Cross-Platform Guide

CareNest uses two presentation-host families over the same .NET 10 domain/application foundation.

## Platform coverage

| Platform | Host | Target | Status |
| --- | --- | --- | --- |
| Android | .NET MAUI | `net10.0-android` | existing application target |
| iPhone / iPad | .NET MAUI | `net10.0-ios` | existing application target |
| macOS | .NET MAUI Mac Catalyst | `net10.0-maccatalyst` | existing application target |
| Windows 10/11 | .NET MAUI | `net10.0-windows10.0.19041.0` | existing application target |
| Linux desktop | Avalonia Desktop | `net10.0` | cross-platform host |
| Modern browsers | Avalonia Browser | `net10.0-browser` | WebAssembly host |

The Avalonia desktop host can also run on Windows and macOS. The MAUI application remains the primary established host for the original four platform families while the shared Avalonia presentation layer provides Linux and browser reach without replacing the existing verified MAUI source.

## Projects

```text
src/
  CareNest.Shared/                    common helpers
  CareNest.Domain/                    health-organizer entities/rules
  CareNest.Application/               use cases and contracts
  CareNest.Infrastructure/            SQLite, documents, reports, backup
  CareNest.App/                       MAUI Android/iOS/Mac Catalyst/Windows host
  CareNest.CrossPlatform/             shared Avalonia application + views
  CareNest.CrossPlatform.Desktop/     Windows/macOS/Linux Avalonia entry point
  CareNest.CrossPlatform.Browser/     WebAssembly Avalonia entry point
```

## Linux development

Prerequisites:

- .NET 10 SDK
- a supported Linux graphical environment
- internet access for the first NuGet restore

Build:

```bash
dotnet restore src/CareNest.CrossPlatform.Desktop/CareNest.CrossPlatform.Desktop.csproj
dotnet build src/CareNest.CrossPlatform.Desktop/CareNest.CrossPlatform.Desktop.csproj -c Release
dotnet run --project src/CareNest.CrossPlatform.Desktop/CareNest.CrossPlatform.Desktop.csproj
```

Publish framework-dependent binaries:

```bash
dotnet publish src/CareNest.CrossPlatform.Desktop/CareNest.CrossPlatform.Desktop.csproj \
  -c Release \
  -r linux-x64 \
  --self-contained false
```

For ARM64 Linux, replace `linux-x64` with `linux-arm64`.

## Browser development

Install the WebAssembly build workload once for the active .NET 10 SDK:

```bash
dotnet workload install wasm-tools
```

Build and publish:

```bash
dotnet build src/CareNest.CrossPlatform.Browser/CareNest.CrossPlatform.Browser.csproj -c Release
dotnet publish src/CareNest.CrossPlatform.Browser/CareNest.CrossPlatform.Browser.csproj -c Release
```

The publish output contains static WebAssembly site assets under the browser project's Release publish tree. Serve the generated `wwwroot` directory with a web server that returns `.wasm` files with the `application/wasm` MIME type.

## Existing MAUI builds

Android:

```bash
dotnet workload install maui-android
dotnet build src/CareNest.App/CareNest.App.csproj \
  -f net10.0-android \
  -c Release \
  -p:CareNestTargetFramework=net10.0-android
```

Windows:

```powershell
dotnet workload install maui
dotnet build src/CareNest.App/CareNest.App.csproj `
  -f net10.0-windows10.0.19041.0 `
  -c Release `
  -p:CareNestTargetFramework=net10.0-windows10.0.19041.0
```

iOS and Mac Catalyst must be built on a compatible macOS/Xcode environment with the applicable .NET workloads.

## Architecture rule

Platform-neutral business rules must remain outside presentation hosts. New features should prefer this dependency direction:

```text
CareNest.Shared
    ↑
CareNest.Domain
    ↑
CareNest.Application
    ↑
CareNest.Infrastructure

CareNest.App (MAUI) --------------------┐
CareNest.CrossPlatform (Avalonia) -----┼─ presentation/host boundary
  ├─ CareNest.CrossPlatform.Desktop ---┤
  └─ CareNest.CrossPlatform.Browser ---┘
```

A presentation project may reference shared/domain/application components, but domain/application projects must never reference MAUI, Avalonia, browser APIs, or operating-system-specific UI APIs.

## Capability boundaries

Operating systems and browsers differ in notification delivery, camera/file-picker behavior, secure-storage facilities, background execution and filesystem access. Cross-platform code must therefore expose those capabilities through host-specific adapters instead of pretending every platform has identical APIs.

The browser host must not silently claim native background-reminder, unrestricted filesystem, or native secure-store behavior. Any browser-specific implementation must use explicit browser capability semantics and preserve CareNest's local-first/privacy model.

## Continuous integration

`.github/workflows/ci.yml` verifies:

- core unit/integration/UI-contract tests on Linux;
- Android MAUI build;
- Windows MAUI build;
- iOS simulator + Mac Catalyst builds;
- Avalonia Linux desktop build;
- Avalonia WebAssembly publish.

A green matrix proves that the configured source builds/tests passed for that exact commit. It does not replace real-device validation, signing/notarization, accessibility testing, store review or browser-specific manual validation.
