$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$Root = (Resolve-Path (Join-Path $PSScriptRoot '../..')).Path
Set-Location $Root

if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    throw 'git is required.'
}

function Invoke-Git {
    param(
        [Parameter(Mandatory = $true)]
        [string[]]$Arguments,

        [Parameter(Mandatory = $true)]
        [string]$Description
    )

    & git @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "$Description failed with exit code $LASTEXITCODE."
    }
}

Invoke-Git -Arguments @('rev-parse', '--is-inside-work-tree') -Description 'Repository validation'
Invoke-Git -Arguments @('config', '--local', 'user.name', 'Sanskar') -Description 'Git user.name configuration'
Invoke-Git -Arguments @('config', '--local', 'user.email', 'sanskarin@outlook.in') -Description 'Git user.email configuration'

$configuredName = (& git config --local --get user.name).Trim()
if ($LASTEXITCODE -ne 0 -or $configuredName -ne 'Sanskar') {
    throw 'Repository-local Git user.name verification failed.'
}

$configuredEmail = (& git config --local --get user.email).Trim()
if ($LASTEXITCODE -ne 0 -or $configuredEmail -ne 'sanskarin@outlook.in') {
    throw 'Repository-local Git user.email verification failed.'
}

Write-Host 'Configured repository-local Git identity.'
