[CmdletBinding()]
param(
    [switch]$SkipBuild,
    [switch]$ForwardPorts
)

$ErrorActionPreference = "Stop"
. (Join-Path $PSScriptRoot "tools.ps1")

$root = Split-Path -Parent $PSScriptRoot
Push-Location $root

try {
    if (-not $SkipBuild) {
        & (Join-Path $PSScriptRoot "build-images.ps1")
        if ($LASTEXITCODE -ne 0) { throw "Image build failed." }
    }

    Write-Host "Applying Kubernetes manifests from k8/..." -ForegroundColor Cyan
    kubectl apply -k k8
    if ($LASTEXITCODE -ne 0) { throw "kubectl apply failed." }

    $appDeployments = @(
        "stockidentityserver",
        "stockapi",
        "stockapigateway",
        "stockmarketupdater",
        "stockwebclient"
    )

    if (-not $SkipBuild) {
        foreach ($deployment in $appDeployments) {
            Write-Host "Restarting deployment/$deployment to pick up the new image..." -ForegroundColor Cyan
            kubectl -n stock rollout restart "deployment/$deployment"
            if ($LASTEXITCODE -ne 0) { throw "Could not restart deployment/$deployment." }
        }
    }

    $workloads = @(
        "statefulset/mongo",
        "statefulset/rabbitmq",
        "deployment/mongo-express",
        "deployment/stockidentityserver",
        "deployment/stockapi",
        "deployment/stockapigateway",
        "deployment/stockmarketupdater",
        "deployment/stockwebclient"
    )

    foreach ($workload in $workloads) {
        Write-Host "Waiting for $workload..." -ForegroundColor Cyan
        kubectl -n stock rollout status $workload --timeout=180s
        if ($LASTEXITCODE -ne 0) { throw "$workload did not become ready." }
    }

    Write-Host ""
    Write-Host "Deployment complete." -ForegroundColor Green
    Write-Host "  Web client : http://localhost:44100"
    Write-Host "  Identity   : http://localhost:44401"
    Write-Host "  API gateway: http://localhost:44405"
    Write-Host "  API (direct): http://localhost:44300/swagger"
    Write-Host "  Mongo UI   : http://localhost:8081"
    Write-Host ""
    Write-Host "Port forwarding is only running if you passed -ForwardPorts." -ForegroundColor Yellow
    Write-Host "Otherwise run: .\k8\port-forward.ps1"

    if ($ForwardPorts) {
        & (Join-Path $PSScriptRoot "port-forward.ps1")
    }
}
finally {
    Pop-Location
}
