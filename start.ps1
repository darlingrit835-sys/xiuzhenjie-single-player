# 修真界 单机版 一键启动
# 作用：起本地服务端 -> 等手机连上 -> 建端口映射 -> 装客户端 -> 启动游戏
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
. (Join-Path $root 'tools\backend.ps1')
$adb  = Join-Path $root 'tools\adb\adb.exe'
$srv  = Join-Path $root 'server'
$goStart = Join-Path $root 'server-go\start.ps1'
$apk  = Join-Path $root 'client\修真界单机版.apk'
$pkg  = 'com.LMGame.XiuXianJie'
$act  = 'com.unity3d.player.UnityPlayerActivity'
$backend = if ($env:XZJ_BACKEND) { $env:XZJ_BACKEND.ToLowerInvariant() } else { 'go' }
if ($backend -notin @('go', 'java')) {
  Write-Host '[错误] XZJ_BACKEND 只接受 go 或 java。' -ForegroundColor Red
  Read-Host '按回车退出' | Out-Null
  exit 1
}

Write-Host '==================================================' -ForegroundColor Cyan
Write-Host '             修真界 · 单机版 一键启动' -ForegroundColor Cyan
Write-Host '==================================================' -ForegroundColor Cyan

if (-not (Test-Path -LiteralPath $apk))  { Write-Host '[错误] 找不到客户端 APK：' $apk -ForegroundColor Red; Read-Host '按回车退出' | Out-Null; exit 1 }
if (-not (Test-Path -LiteralPath $adb))  { Write-Host '[错误] 找不到 adb：' $adb -ForegroundColor Red; Read-Host '按回车退出' | Out-Null; exit 1 }

# ---------- 1. 服务端 ----------
Write-Host ''
Write-Host "[1/5] 本地服务端 ($backend)" -ForegroundColor Yellow
if (Test-XzjBackendReady $backend) {
  Write-Host '      已在运行，直接复用' -ForegroundColor Green
} else {
  if (@(Get-XzjListeners).Count -gt 0) {
    Write-Host '[错误] 默认端口有其他服务占用，或所选后端未完整启动。请检查端口或先停止原服务。' -ForegroundColor Red
    Read-Host '按回车退出' | Out-Null
    exit 1
  }
  Write-Host '      正在启动服务端，请保留新打开的服务端窗口。'
  if ($backend -eq 'go') {
    $child = Start-Process -FilePath 'powershell.exe' -ArgumentList ('-NoProfile -ExecutionPolicy Bypass -File "{0}"' -f $goStart) -WorkingDirectory $root -PassThru
  } else {
    if (-not (Test-Path -LiteralPath $XzjJavaExe)) {
      Write-Host '[错误] 找不到内置 Java。' -ForegroundColor Red
      Read-Host '按回车退出' | Out-Null
      exit 1
    }
    $javaOptions = @()
    if ($env:XZJ_JAVA_PROFILE -ne 'default') {
      $javaOptions = @('-Xms32m', '-Xmx256m', '-XX:+UseSerialGC', '-XX:ActiveProcessorCount=2')
    }
    $child = Start-Process -FilePath $XzjJavaExe -ArgumentList ($javaOptions + @('-cp', 'out;lib/*', 'server.Main')) -WorkingDirectory $srv -PassThru
  }
  $ok = $false
  $deadline = (Get-Date).AddSeconds(120)
  while ((Get-Date) -lt $deadline -and -not $child.HasExited) {
    Start-Sleep -Seconds 1
    if (Test-XzjBackendReady $backend) { $ok = $true; break }
  }
  if (-not $ok) {
    Write-Host '服务端未就绪。首次启动 Go 可能需要编译，请查看服务端窗口中的输出。' -ForegroundColor Red
    Read-Host '按回车退出' | Out-Null
    exit 1
  }
  Write-Host '      服务端已就绪（账号服 9443 / 游戏服 17666 / 战斗服 17667）' -ForegroundColor Green
}

# ---------- 2. 设备 ----------
Write-Host ''
Write-Host '[2/5] 等待手机连接（请插 USB，并在手机上允许 USB 调试）' -ForegroundColor Yellow
$dev = $null
for ($i = 0; $i -lt 60; $i++) {
  $lines = & $adb devices | Where-Object { $_ -match '\tdevice' }
  if ($lines) { $dev = ($lines -split "`t")[0].Trim(); break }
  Start-Sleep -Seconds 1
}
if (-not $dev) {
  Write-Host '      没检测到已授权的设备。请检查：' -ForegroundColor Red
  Write-Host '        · 数据线是能传数据的（不是只能充电的）'
  Write-Host '        · 开发者选项里“USB 调试”已打开，手机弹出的授权框点了“允许”'
  Read-Host '按回车退出' | Out-Null
  exit 1
}
Write-Host ("      已连接：{0}" -f $dev) -ForegroundColor Green

# ---------- 3. 端口映射 ----------
Write-Host ''
Write-Host '[3/5] 建立端口映射（USB 反向代理）' -ForegroundColor Yellow
foreach ($port in $XzjBackendPorts) {
  & $adb -s $dev reverse "tcp:$port" "tcp:$port" | Out-Null
  if ($LASTEXITCODE -ne 0) { throw "端口 $port 映射失败。" }
}
Write-Host '      已完成：手机的 127.0.0.1 已指向电脑上的服务端' -ForegroundColor Green

# ---------- 4. 客户端 ----------
Write-Host ''
Write-Host '[4/5] 客户端' -ForegroundColor Yellow
$installed = (& $adb -s $dev shell pm path $pkg 2>$null) -join ''
if ([string]::IsNullOrWhiteSpace($installed)) {
  Write-Host '      首次安装，约 1 分钟...'
  & $adb -s $dev install -r -d $apk | Select-Object -Last 2 | ForEach-Object { Write-Host ('      ' + $_) }
  if ($LASTEXITCODE -ne 0) { throw '客户端安装失败。' }
} else {
  Write-Host '      已安装，跳过（要重装请运行“重装客户端.bat”）' -ForegroundColor Green
}

# ---------- 5. 启动 ----------
Write-Host ''
Write-Host '[5/5] 启动游戏' -ForegroundColor Yellow
& $adb -s $dev shell am force-stop $pkg | Out-Null
& $adb -s $dev shell am start -n "$pkg/$act" | Select-Object -First 1 | ForEach-Object { Write-Host ('      ' + $_) }

Write-Host ''
Write-Host '==================================================' -ForegroundColor Cyan
Write-Host ' 完成。游戏已在手机上启动。' -ForegroundColor Green
Write-Host ' 注意：不要关闭“修真界服务端”那个窗口；' -ForegroundColor Gray
Write-Host '      拔插过数据线后，重新运行本脚本即可恢复连接。' -ForegroundColor Gray
Write-Host '==================================================' -ForegroundColor Cyan
Read-Host '按回车关闭本窗口（不影响游戏）' | Out-Null
