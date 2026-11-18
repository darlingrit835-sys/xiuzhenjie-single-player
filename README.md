# 修真界 · 服务端源码（单机化改造）

把一个「安卓客户端 APK + 远程服务端」的游戏改成**只在自己电脑上跑的离线单机**：
服务端跑在本机，客户端连 `127.0.0.1`，手机通过 USB + `adb reverse` 接到电脑上的服务端，全程不联网。

> 这个公开仓库放的是**改造说明、启动脚本和客户端改包工具**。
> 完整服务端源码（Java，含 `server/src`、`proto`、配置表）体积较大且含游戏资源，
> 需要的话用作者打包的 `修真界单机版.bundle` 克隆：`git clone 修真界单机版.bundle 修真界单机版`

## 改造做了什么

| # | 位置 | 改动 |
| --- | --- | --- |
| 1 | 客户端 `XiuZhenJie.Constants.LoginServerIP` / `LoginServerPort` | `10.0.2.2:9443` → `127.0.0.1:9443`，热更 URL 同步改，重签名 v1+v2+v3 |
| 2 | 服务端 `Config.PUBLIC_IP` | 支持 `-Dxzj.publicIp=` 覆盖，默认 `127.0.0.1`；`data/config/server.json` 的 `publicIp` 同步改，否则登录后下发的选服地址还是旧 IP |
| 3 | 服务端 `RoleLoginHandler` + `ServerConfig.completeBattleGuide` | 修「进战斗地图就出不来」：客户端要求 `BattleGuideVal == 100` 或引导步数 ≥ 16，私服没有引导链会永久卡死，现在登录直接下发 100 |

细节见 [`docs/单机化改造.md`](docs/单机化改造.md)。

## 怎么玩（玩家视角）

1. 手机 USB 连电脑，开「开发者选项 → USB 调试」，弹窗点允许。
2. 双击 `启动游戏.bat`：起服务端 → 等设备 → `adb reverse` 映射 9443/17666/17667 → 装 APK → 拉起游戏。
3. 别关那个「修真界服务端」黑窗口；拔插数据线后重跑一次脚本即可。

前置条件：手机支持 32 位应用（APK 只有 `armeabi-v7a`）。

## 目录

```
启动游戏.bat / start.ps1      一键启动（ps1 为 UTF-8 BOM，供 PowerShell 5.1 正确解析中文）
停止服务端.bat / stop.ps1     停服
重装客户端.bat / reinstall.ps1 重装 APK
使用说明.txt                  给玩家的说明
tools/patch_apk.ps1           客户端改包：改 IP、重打包、zipalign、apksigner 签名
docs/单机化改造.md            三处改动的具体代码与踩坑
data/server.json              服务端配置样例
```

## 已知限制

- 「单机」目前是**电脑当服务器、手机当客户端**；手机要完全脱机独立运行，需把 Java 服务端编译成
  dex 内嵌进 APK，尚未实现。
- 服务端协议是按客户端逐个补齐的，部分活动 / 充值 / 排行榜系统还没实现。
