#!/bin/sh
# USB 重连后，恢复这台手机的本地账号服、游戏服和战斗服转发。
set -eu

if [ "$#" -ne 1 ] || [ -z "$1" ]; then
    printf '用法：%s 手机序列号\n' "$0" >&2
    exit 2
fi
serial=$1
adb=${XZJ_ADB:-adb}
if ! command -v "$adb" >/dev/null 2>&1; then
    printf '%s\n' '找不到 adb，请通过 XZJ_ADB 指定可执行文件。' >&2
    exit 1
fi

previous=
while :; do
    if mappings=$("$adb" -s "$serial" reverse --list 2>/dev/null); then
        status=ready
        restored=
        for port in 9443 17666 17667; do
            if printf '%s\n' "$mappings" | awk -v port="tcp:$port" '
                NF >= 2 && $(NF-1) == port && $NF == port { found = 1 }
                END { exit !found }
            '; then
                continue
            fi
            if "$adb" -s "$serial" reverse "tcp:$port" "tcp:$port" >/dev/null 2>&1; then
                restored="${restored:+$restored }$port"
            else
                status=retry
            fi
        done
        if [ -n "$restored" ]; then
            printf '[%s] 已恢复手机端口转发 %s\n' "$(date '+%H:%M:%S')" "$restored"
        fi
    else
        status=disconnected
    fi
    if [ "$status" != "$previous" ]; then
        case "$status" in
            ready) message='三个本地服务端口已完成手机转发' ;;
            retry) message='手机转发未配置齐，正在重试' ;;
            disconnected) message='等待手机连接并授权 USB 调试' ;;
        esac
        printf '[%s] %s\n' "$(date '+%H:%M:%S')" "$message"
        previous=$status
    fi
    sleep 2
done
