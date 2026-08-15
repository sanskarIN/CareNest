$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$Root = (Resolve-Path (Join-Path $PSScriptRoot '../..')).Path
Set-Location $Root

function Write-Step([string]$Message) {
    Write-Host "`n==> $Message"
}

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw 'dotnet SDK is required.'
}

Write-Step 'CareNest release preflight'
Write-Host "Repository: $Root"
dotnet --info
if ($LASTEXITCODE -ne 0) { throw 'dotnet --info failed.' }

Write-Step 'Source hygiene'
$markers = Get-ChildItem -Path src, tests -Recurse -File |
    Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' } |
    Select-String -Pattern 'TODO|FIXME|NotImplementedException'
if ($markers) {
    $markers | ForEach-Object { Write-Host $_ }
    throw 'Release-blocking implementation marker found.'
}

Write-Step 'Formatting'
dotnet format CareNest.sln --verify-no-changes
if ($LASTEXITCODE -ne 0) { throw 'dotnet format verification failed.' }

Write-Step 'Core Release builds'
$coreProjects = @(
    'src/CareNest.Domain/CareNest.Domain.csproj',
    'src/CareNest.Application/CareNest.Application.csproj',
    'src/CareNest.Infrastructure/CareNest.Infrastructure.csproj'
)
foreach ($project in $coreProjects) {
    dotnet build $project -c Release --nologo
    if ($LASTEXITCODE -ne 0) { throw "Build failed: $project" }
}

Write-Step 'Automated tests'
$testProjects = @(
    'tests/CareNest.UnitTests/CareNest.UnitTests.csproj',
    'tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj',
    'tests/CareNest.UiTests/CareNest.UiTests.csproj'
)
foreach ($project in $testProjects) {
    dotnet test $project -c Release --nologo
    if ($LASTEXITCODE -ne 0) { throw "Tests failed: $project" }
}

Write-Step 'Blocking dependency audit'
foreach ($project in $testProjects) {
    dotnet restore $project --nologo -p:NuGetAudit=true -p:NuGetAuditMode=all
    if ($LASTEXITCODE -ne 0) { throw "Dependency audit failed: $project" }
}

if ($env:CARENEST_TARGET) {
    Write-Step "Audit optional MAUI target: $($env:CARENEST_TARGET)"
    dotnet restore src/CareNest.App/CareNest.App.csproj `
        -p:CareNestTargetFramework=$env:CARENEST_TARGET `
        -p:NuGetAudit=true `
        -p:NuGetAuditMode=all `
        --nologo
    if ($LASTEXITCODE -ne 0) { throw "MAUI dependency audit failed: $($env:CARENEST_TARGET)" }

    Write-Step "Optional MAUI Release build: $($env:CARENEST_TARGET)"
    dotnet build src/CareNest.App/CareNest.App.csproj `
        -f $env:CARENEST_TARGET `
        -c Release `
        -p:CareNestTargetFramework=$env:CARENEST_TARGET `
        --nologo
    if ($LASTEXITCODE -ne 0) { throw "MAUI build failed: $($env:CARENEST_TARGET)" }
}

Write-Step 'Preflight complete'
Write-Host 'Automated source checks completed. Manual device, accessibility, signing, store-policy, and packaged existing-data/encrypted-data compatibility evidence are still required where applicable.'
