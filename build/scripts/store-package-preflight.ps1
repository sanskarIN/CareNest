$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$Target = $env:CARENEST_TARGET
$SupportedTargets = @(
    'net10.0-android',
    'net10.0-ios',
    'net10.0-maccatalyst',
    'net10.0-windows10.0.19041.0'
)

if ([string]::IsNullOrWhiteSpace($Target)) {
    throw 'CARENEST_TARGET is required for store-package preflight.'
}

if ($Target -notin $SupportedTargets) {
    throw "Unsupported CARENEST_TARGET: $Target"
}

Write-Host 'CareNest store-package preflight'
Write-Host "Target: $Target"
Write-Host 'External funding surface: absent from app runtime by source policy'

& (Join-Path $PSScriptRoot 'release-preflight.ps1')
if ($LASTEXITCODE -ne 0) {
    throw "CareNest store-package preflight failed with exit code $LASTEXITCODE."
}
