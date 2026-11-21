# 重装客户端：卸载旧版本后重新安装 APK
$ErrorActionPreference = 'Continue'
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$adb  = Join-Path $root 'tools\adb\adb.exe'
$apk  = if ($env:XZJ_CLIENT_APK) { $env:XZJ_CLIENT_APK } else { Join-Path $root 'client\修真界单机版.apk' }
$pkg  = 'com.LMGame.XiuXianJie'

if (-not (Test-Path -LiteralPath $apk -PathType Leaf)) {
  Write-Host "[错误] 请设置 XZJ_CLIENT_APK，或将本地版 APK 放在 $apk。" -ForegroundColor Red
  Read-Host '按回车退出' | Out-Null
  exit 1
}
Write-Host '卸载旧版本...'
& $adb uninstall $pkg | Out-Null
Write-Host '安装中（约 1 分钟）...'
& $adb install -r -d $apk
if ($LASTEXITCODE -ne 0) { throw '客户端安装失败。' }
Write-Host ''
Write-Host '装好了，接着运行“启动游戏.bat”。' -ForegroundColor Green
Read-Host '按回车关闭' | Out-Null
