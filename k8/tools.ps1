# Adds Docker Desktop's per-user CLI directory to PATH when the CLI is not
# already available. Docker Desktop installs docker.exe and kubectl.exe here
# on recent Windows versions.
$dockerCliPaths = @(
    "C:\Program Files\Docker\Docker\resources\bin",
    (Join-Path $env:LOCALAPPDATA "Programs\DockerDesktop\resources\bin")
)

foreach ($dockerCliPath in $dockerCliPaths) {
    if (Test-Path (Join-Path $dockerCliPath "docker.exe")) {
        $env:Path = "$dockerCliPath;$env:Path"
    }
}
