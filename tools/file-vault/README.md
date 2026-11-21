# 修真界文件站

私有上传与下载页面，支持拖拽、多选、进度和同名文件保留。单文件最大 4 GiB，总容量 15 GiB。上传的数据保存在腾讯云服务器上的 `/var/lib/xzj-file-vault`，并为系统盘预留至少 10 GiB。文件仓库独立于游戏存档和现有热更新目录。

## Mac 使用

双击下载目录里的「打开修真界文件站.command」。浏览器会打开 `http://127.0.0.1:18192/`。首次登录账号为 `vault`，在启动器窗口按 `p` 可复制密码。启动器通过 SSH 加密转发访问服务器，文件站只监听服务器的回环地址。

本机的连接材料保存在 `~/.config/xzj-file-vault/`。其中 `access-token` 和 `tunnel_ed25519` 是私密文件。专用 SSH 账号仅能转发到文件站端口。

## 服务维护

使用 Go 1.26 或更新版本执行 `CGO_ENABLED=0 GOOS=linux GOARCH=amd64 go build -trimpath .`。云端二进制路径为 `/opt/xzj-file-vault/file-vault`，由 `xzj-file-vault.service` 启动，口令文件为 `/etc/xzj-file-vault/access-token`。用 `sudo systemctl status xzj-file-vault` 检查状态，更新二进制后用 `sudo systemctl restart xzj-file-vault` 使变更生效。备份时复制 `/var/lib/xzj-file-vault/` 中已完成的文件。

## 手机下载最新版

手机收藏 `https://82.156.174.166:80/`。网站继续使用原来的下载密码。每次通过文件站上传包名为 `com.lmgame.xiuxianjie.emailtrial`、版本号更高的 APK，上传完成后会自动发布。页面会显示当前发布的版本号，旧版本和其他文件仍可通过私有文件站下载。手机下载按钮使用带版本号的路径，发布后已有的下载可继续。

云端通过 `xzj-apk-edge.service` 提供 HTTPS，`xzj-apk-latest.service` 处理密码和下载，`xzj-apk-renew.timer` 检查证书续期。文件站在私有目录下的 `.published` 归档已发布版本，使用硬链接复用原 APK 数据。公开下载服务由 `xzj-apk-share` 账号读取只读挂载的发布目录。
