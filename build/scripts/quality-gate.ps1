$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$Root = (Resolve-Path (Join-Path $PSScriptRoot '../..')).Path
Set-Location $Root

function Invoke-Dotnet {
    param(
        [Parameter(Mandatory = $true)]
        [string[]]$Arguments,

        [Parameter(Mandatory = $true)]
        [string]$Description
    )

    & dotnet @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "$Description failed with exit code $LASTEXITCODE."
    }
}

$projects = @(
    'src/CareNest.Shared/CareNest.Shared.csproj',
    'src/CareNest.Domain/CareNest.Domain.csproj',
    'src/CareNest.Application/CareNest.Application.csproj',
    'src/CareNest.Infrastructure/CareNest.Infrastructure.csproj',
    'tests/CareNest.UnitTests/CareNest.UnitTests.csproj',
    'tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj',
    'tests/CareNest.UiTests/CareNest.UiTests.csproj'
)

$testProjects = @(
    'tests/CareNest.UnitTests/CareNest.UnitTests.csproj',
    'tests/CareNest.IntegrationTests/CareNest.IntegrationTests.csproj',
    'tests/CareNest.UiTests/CareNest.UiTests.csproj'
)

foreach ($project in $projects) {
    Invoke-Dotnet -Arguments @('format', $project, '--verify-no-changes', '--verbosity', 'minimal') -Description "Formatting verification for $project"
}

$coreProjects = @(
    'src/CareNest.Shared/CareNest.Shared.csproj',
    'src/CareNest.Domain/CareNest.Domain.csproj',
    'src/CareNest.Application/CareNest.Application.csproj',
    'src/CareNest.Infrastructure/CareNest.Infrastructure.csproj'
)

foreach ($project in $coreProjects) {
    Invoke-Dotnet -Arguments @('build', $project, '-c', 'Release', '--nologo') -Description "Release build for $project"
}

foreach ($project in $testProjects) {
    Invoke-Dotnet -Arguments @('test', $project, '-c', 'Release', '--nologo') -Description "Tests for $project"
}

foreach ($project in $testProjects) {
    Invoke-Dotnet -Arguments @('restore', $project, '--nologo', '-p:NuGetAudit=true', '-p:NuGetAuditMode=all') -Description "Dependency audit for $project"
}
