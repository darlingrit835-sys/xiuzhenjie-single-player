# 停止本地服务端（关闭所有运行中的 server.Main 进程）
$procs = Get-CimInstance Win32_Process -Filter "Name='java.exe' or Name='javaw.exe'" |
         Where-Object { $_.CommandLine -match 'server\.Main' }
if (-not $procs) {
  Write-Host '服务端没有在运行。' -ForegroundColor Green
} else {
  foreach ($p in $procs) {
    Write-Host ("  结束进程 PID {0}" -f $p.ProcessId)
    Stop-Process -Id $p.ProcessId -Force -ErrorAction SilentlyContinue
  }
  Write-Host '服务端已停止。' -ForegroundColor Green
}
Read-Host '按回车关闭' | Out-Null
