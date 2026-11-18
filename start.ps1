# 修真界 单机版 一键启动
# 作用：起本地服务端 -> 等手机连上 -> 建端口映射 -> 装客户端 -> 启动游戏
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$adb  = Join-Path $root 'tools\adb\adb.exe'
$srv  = Join-Path $root 'server'
$java = Join-Path $srv 'jdk8\bin\java.exe'
$apk  = Join-Path $root 'client\修真界单机版.apk'
$pkg  = 'com.LMGame.XiuXianJie'
$act  = 'com.unity3d.player.UnityPlayerActivity'

function Test-Port([int]$p) {
  $null -ne (Get-NetTCPConnection -State Listen -LocalPort $p -ErrorAction SilentlyContinue)
}

Write-Host '==================================================' -ForegroundColor Cyan
Write-Host '             修真界 · 单机版 一键启动' -ForegroundColor Cyan
Write-Host '==================================================' -ForegroundColor Cyan

if (-not (Test-Path -LiteralPath $java)) { Write-Host '[错误] 找不到内置 Java：' $java -ForegroundColor Red; Read-Host '按回车退出' | Out-Null; exit 1 }
if (-not (Test-Path -LiteralPath $apk))  { Write-Host '[错误] 找不到客户端 APK：' $apk -ForegroundColor Red; Read-Host '按回车退出' | Out-Null; exit 1 }
if (-not (Test-Path -LiteralPath $adb))  { Write-Host '[错误] 找不到 adb：' $adb -ForegroundColor Red; Read-Host '按回车退出' | Out-Null; exit 1 }

Write-Host ''
Write-Host '[1/5] 本地服务端' -ForegroundColor Yellow
if (Test-Port 9443) {
  Write-Host '      已在运行，直接复用' -ForegroundColor Green
} else {
  Write-Host '      正在启动（会弹出一个“修真界服务端”窗口，关掉它就等于停服）...'
  Start-Process -FilePath $java -ArgumentList '-cp','out;lib/*','server.Main' -WorkingDirectory $srv | Out-Null
  $ok = $false
  for ($i = 0; $i -lt 30; $i++) {
    Start-Sleep -Seconds 1
    if (Test-Port 9443) { $ok = $true; break }
  }
  if (-not $ok) { Write-Host '      服务端启动超时，请看那个黑窗口里的报错' -ForegroundColor Red }
  else { Write-Host '      服务端已就绪（账号服 9443 / 游戏服 17666 / 战斗服 17667）' -ForegroundColor Green }
}

Write-Host ''
Write-Host '[2/5] 等待手机连接（请插 USB，并在手机上允许 USB 调试）' -ForegroundColor Yellow
$dev = $null
for ($i = 0; $i -lt 60; $i++) {
  $lines = & $adb devices | Where-Object { $_ -match "`tdevice" }
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

Write-Host ''
Write-Host '[3/5] 建立端口映射（USB 反向代理）' -ForegroundColor Yellow
& $adb reverse tcp:9443  tcp:9443  | Out-Null
& $adb reverse tcp:17666 tcp:17666 | Out-Null
& $adb reverse tcp:17667 tcp:17667 | Out-Null
Write-Host '      已完成：手机的 127.0.0.1 已指向电脑上的服务端' -ForegroundColor Green

Write-Host ''
Write-Host '[4/5] 客户端' -ForegroundColor Yellow
$installed = (& $adb shell pm path $pkg 2>$null) -join ''
if ([string]::IsNullOrWhiteSpace($installed)) {
  Write-Host '      首次安装，约 1 分钟...'
  & $adb install -r -d $apk | Select-Object -Last 2 | ForEach-Object { Write-Host ('      ' + $_) }
} else {
  Write-Host '      已安装，跳过（要重装请运行“重装客户端.bat”）' -ForegroundColor Green
}

Write-Host ''
Write-Host '[5/5] 启动游戏' -ForegroundColor Yellow
& $adb shell am force-stop $pkg | Out-Null
& $adb shell am start -n "$pkg/$act" | Select-Object -First 1 | ForEach-Object { Write-Host ('      ' + $_) }

Write-Host ''
Write-Host '==================================================' -ForegroundColor Cyan
Write-Host ' 完成。游戏已在手机上启动。' -ForegroundColor Green
Write-Host ' 注意：不要关闭“修真界服务端”那个窗口；' -ForegroundColor Gray
Write-Host '      拔插过数据线后，重新运行本脚本即可恢复连接。' -ForegroundColor Gray
Write-Host '==================================================' -ForegroundColor Cyan
Read-Host '按回车关闭本窗口（不影响游戏）' | Out-Null
