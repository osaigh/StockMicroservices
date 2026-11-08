$ErrorActionPreference = "Stop"
. (Join-Path $PSScriptRoot "tools.ps1")

$forwards = @(
    @{ Service = "stockwebclient"; LocalPort = 44100; RemotePort = 80 },
    @{ Service = "stockidentityserver"; LocalPort = 44401; RemotePort = 80 },
    @{ Service = "stockapigateway"; LocalPort = 44405; RemotePort = 80 },
    @{ Service = "stockapi"; LocalPort = 44300; RemotePort = 80 },
    @{ Service = "mongo-express"; LocalPort = 8081; RemotePort = 8081 }
)

$processes = @()
try {
    foreach ($forward in $forwards) {
        Write-Host "Forwarding localhost:$($forward.LocalPort) -> $($forward.Service):$($forward.RemotePort)" -ForegroundColor Cyan
        $processes += Start-Process -FilePath "kubectl" `
            -ArgumentList @("-n", "stock", "port-forward", "svc/$($forward.Service)", "$($forward.LocalPort):$($forward.RemotePort)") `
            -PassThru -NoNewWindow
    }

    Write-Host ""
    Write-Host "Open http://localhost:44100. Press Ctrl+C to stop forwarding." -ForegroundColor Green
    Wait-Process -Id ($processes | ForEach-Object { $_.Id })
}
finally {
    foreach ($process in $processes) {
        if (-not $process.HasExited) {
            Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
        }
    }
}
