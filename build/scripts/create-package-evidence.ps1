$ErrorActionPreference = 'Stop'

$script = Join-Path $PSScriptRoot 'create-package-evidence.py'

$python3 = Get-Command python3 -ErrorAction SilentlyContinue
if ($python3) {
    & $python3.Source $script @args
    exit $LASTEXITCODE
}

$python = Get-Command python -ErrorAction SilentlyContinue
if ($python) {
    & $python.Source $script @args
    exit $LASTEXITCODE
}

$py = Get-Command py -ErrorAction SilentlyContinue
if ($py) {
    & $py.Source -3 $script @args
    exit $LASTEXITCODE
}

throw 'Python 3 is required to create CareNest package evidence.'
