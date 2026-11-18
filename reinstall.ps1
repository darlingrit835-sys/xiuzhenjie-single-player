# 重装客户端：卸载旧版本后重新安装 APK
$ErrorActionPreference = 'Continue'
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$adb  = Join-Path $root 'tools\adb\adb.exe'
$apk  = Join-Path $root 'client\修真界单机版.apk'
$pkg  = 'com.LMGame.XiuXianJie'

Write-Host '卸载旧版本...'
& $adb uninstall $pkg | Out-Null
Write-Host '安装中（约 1 分钟）...'
& $adb install -r -d $apk
Write-Host ''
Write-Host '装好了，接着运行“启动游戏.bat”。' -ForegroundColor Green
Read-Host '按回车关闭' | Out-Null
