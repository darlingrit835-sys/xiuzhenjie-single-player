$XzjBackendPorts = @(9443, 17666, 17667)
$XzjRoot = Split-Path -Parent $PSScriptRoot
$XzjGoExe = Join-Path $XzjRoot 'server-go\.build\xzj-server.exe'
$XzjJavaExe = Join-Path $XzjRoot 'server\jdk8\bin\java.exe'
$XzjJavawExe = Join-Path $XzjRoot 'server\jdk8\bin\javaw.exe'

function Get-XzjListeners {
  Get-NetTCPConnection -State Listen -LocalPort $XzjBackendPorts -ErrorAction SilentlyContinue
}

function Get-XzjLoopbackListeners {
  Get-XzjListeners | Where-Object { $_.LocalAddress -in @('127.0.0.1', '0.0.0.0', '::') }
}

function Get-XzjBackendName([int]$ProcessId) {
  $process = Get-CimInstance Win32_Process -Filter "ProcessId=$ProcessId" -ErrorAction SilentlyContinue
  if ($process.ExecutablePath -eq $XzjGoExe) {
    return 'go'
  }
  if (($process.ExecutablePath -in @($XzjJavaExe, $XzjJavawExe)) -and
      ($process.CommandLine -match '(?:^|\s)server\.Main(?:\s|$)')) {
    return 'java'
  }
}

function Test-XzjBackendReady([string]$Backend) {
  $connections = @(Get-XzjLoopbackListeners)
  $ports = @($connections | Select-Object -ExpandProperty LocalPort -Unique)
  $owners = @($connections | Select-Object -ExpandProperty OwningProcess -Unique)
  if ($ports.Count -ne $XzjBackendPorts.Count -or $owners.Count -ne 1) {
    return $false
  }
  return (Get-XzjBackendName $owners[0]) -eq $Backend
}
