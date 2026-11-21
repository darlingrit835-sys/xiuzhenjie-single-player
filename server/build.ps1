$ErrorActionPreference = 'Stop'
Set-Location -LiteralPath $PSScriptRoot
$cache = [IO.Directory]::CreateDirectory((Join-Path $PSScriptRoot '.local-build'))
$output = Join-Path $PSScriptRoot 'out'
$buildId = [Guid]::NewGuid().ToString('N')
$staging = Join-Path $cache.FullName "classes-$buildId"
$previous = Join-Path $cache.FullName "previous-$buildId"
$lock = [IO.File]::Open((Join-Path $cache.FullName 'windows-build.lock'), 'OpenOrCreate', 'ReadWrite', 'None')
try {
    [IO.Directory]::CreateDirectory($staging) | Out-Null
    $sources = @(Get-ChildItem -LiteralPath 'src' -Recurse -File -Filter '*.java' | ForEach-Object { Resolve-Path -Relative -LiteralPath $_.FullName })
    & javac -source 8 -target 8 -encoding UTF-8 -J-Xmx2g -cp 'lib/*' -d $staging @sources
    if ($LASTEXITCODE -ne 0) { throw "javac exited with code $LASTEXITCODE" }
    if (-not (Test-Path -LiteralPath (Join-Path $staging 'server/Main.class'))) { throw 'Missing server/Main.class' }

    if ([IO.Directory]::Exists($output)) { [IO.Directory]::Move($output, $previous) }
    try {
        [IO.Directory]::Move($staging, $output)
    } catch {
        if ([IO.Directory]::Exists($previous)) { [IO.Directory]::Move($previous, $output) }
        throw
    }
    if ([IO.Directory]::Exists($previous)) { [IO.Directory]::Delete($previous, $true) }
} finally {
    $lock.Dispose()
    if ([IO.Directory]::Exists($staging)) { [IO.Directory]::Delete($staging, $true) }
}
