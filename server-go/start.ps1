$ErrorActionPreference = 'Stop'
$Go = if ($env:XZJ_GO) { $env:XZJ_GO } else { 'go' }
$Data = if ($env:XZJ_DATA_DIR) { $env:XZJ_DATA_DIR } else { Join-Path $PSScriptRoot '..\server\data' }
Push-Location -LiteralPath $PSScriptRoot
try {
    if (Get-Command $Go -ErrorAction SilentlyContinue) {
        New-Item -ItemType Directory -Path '.build' -Force | Out-Null
        & $Go build -trimpath -o '.build\xzj-server.exe' .
        if ($LASTEXITCODE) { exit $LASTEXITCODE }
    } elseif (-not (Test-Path -LiteralPath '.build\xzj-server.exe' -PathType Leaf)) {
        throw 'Install Go 1.26+ or place xzj-server.exe in server-go\.build.'
    }
    & '.build\xzj-server.exe' -data $Data @args
    exit $LASTEXITCODE
} finally {
    Pop-Location
}
