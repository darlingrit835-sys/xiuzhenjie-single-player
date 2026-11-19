# 停止此仓库在默认端口监听的后端。
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'tools\backend.ps1')
$owners = @(Get-XzjLoopbackListeners | Select-Object -ExpandProperty OwningProcess -Unique)
$stopped = 0
foreach ($owner in $owners) {
  $backend = Get-XzjBackendName $owner
  if ($backend) {
    Stop-Process -Id $owner
    Write-Host ("已停止 {0} 服务端，PID {1}。" -f $backend, $owner) -ForegroundColor Green
    $stopped++
  }
}
if ($stopped -eq 0 -and $owners.Count -gt 0) {
  Write-Host '默认端口由其他程序占用。' -ForegroundColor Yellow
} elseif ($stopped -eq 0) {
  Write-Host '本地服务端没有运行。' -ForegroundColor Green
}
Read-Host '按回车关闭' | Out-Null
