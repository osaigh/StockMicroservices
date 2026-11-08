[CmdletBinding()]
param(
    [switch]$RebuildClient
)

$ErrorActionPreference = "Stop"
. (Join-Path $PSScriptRoot "tools.ps1")

$root = Split-Path -Parent $PSScriptRoot
$client = Join-Path $root "StockMicroservices.WebClient\ClientApp"
$clientBuild = Join-Path $client "build"

Push-Location $root
try {
    $npmAvailable = $null -ne (Get-Command npm -ErrorAction SilentlyContinue)
    if ($npmAvailable) {
        Write-Host "Building the React client on the host..." -ForegroundColor Cyan
        Push-Location $client
        try {
            if ($RebuildClient -or -not (Test-Path (Join-Path $client "node_modules"))) {
                npm ci --no-audit --no-fund
                if ($LASTEXITCODE -ne 0) { throw "npm ci failed." }
            }

            npm run build
            if ($LASTEXITCODE -ne 0) { throw "npm run build failed." }
        }
        finally {
            Pop-Location
        }
    }
    elseif (-not (Test-Path (Join-Path $clientBuild "index.html"))) {
        throw "Node.js/npm is required to build the web client. Install Node 20+ or build the image with 'docker compose build'."
    }
    else {
        Write-Host "npm is not installed; reusing the existing React build in ClientApp/build." -ForegroundColor Yellow
    }

    Write-Host "Building the web client image from the prebuilt bundle..." -ForegroundColor Cyan
    docker build -f (Join-Path $client "Dockerfile.prebuilt") -t stockwebclient:latest $client
    if ($LASTEXITCODE -ne 0) { throw "Web client image build failed." }

    Write-Host "Building the .NET service images..." -ForegroundColor Cyan
    docker compose build stockidentityserver stockapi stockapigateway stockmarketupdater
    if ($LASTEXITCODE -ne 0) { throw ".NET image build failed." }
}
finally {
    Pop-Location
}
