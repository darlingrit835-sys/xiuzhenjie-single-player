#!/bin/sh
# 本机 Unity 预览和 USB adb reverse 使用的服务端入口。
set -eu
cd "$(CDPATH='' cd -- "$(dirname -- "$0")" && pwd)"

case "${XZJ_BACKEND:-go}" in
    go) exec ../server-go/start.sh "$@" ;;
    java) ;;
    *)
        printf '%s\n' 'XZJ_BACKEND 只接受 go 或 java。' >&2
        exit 2
        ;;
esac

if [ -n "${JAVA_HOME:-}" ]; then
    java="$JAVA_HOME/bin/java"
    javac="$JAVA_HOME/bin/javac"
else
    bundled='/Applications/Unity/Hub/Editor/2022.3.62f3/PlaybackEngines/AndroidPlayer/OpenJDK/bin'
    if [ -x "$bundled/java" ] && [ -x "$bundled/javac" ]; then
        java="$bundled/java"
        javac="$bundled/javac"
    else
        java=$(command -v java || true)
        javac=$(command -v javac || true)
    fi
fi
if [ ! -x "$java" ] || [ ! -x "$javac" ]; then
    printf '%s\n' '找不到 Java JDK。请将 JAVA_HOME 指向本机 JDK。' >&2
    exit 1
fi

data=${XZJ_DATA_DIR:-"$PWD/data"}
if [ ! -r "$data/server.p12" ]; then
    printf '找不到本地服务证书：%s/server.p12\n' "$data" >&2
    exit 1
fi

cache=${XZJ_BUILD_DIR:-"$PWD/.local-build"}
mkdir -p "$cache"
marker="$cache/current"
classes=
if [ -r "$marker" ]; then
    IFS= read -r classes < "$marker"
fi
if [ ! -f "$classes/server/Main.class" ] || [ ! -f "$marker" ] || [ -n "$(find src lib -type f -newer "$marker" -print -quit)" ]; then
    printf '%s\n' '正在编译本地服务端…'
    classes=$(mktemp -d "$cache/classes.XXXXXXXX")
    find src -name '*.java' -print > "$classes/sources.txt"
    "$javac" -source 8 -target 8 -Xlint:-options -encoding UTF-8 -J-Xmx2g -cp 'lib/*' -d "$classes" "@$classes/sources.txt"
    printf '%s\n' "$classes" > "$cache/current.$$"
    mv -f "$cache/current.$$" "$marker"
fi
printf '本地存档目录 %s\n' "$data"
set -- '-Dxzj.bindAddress=127.0.0.1' "-Dxzj.dataDir=$data" -cp "$classes:lib/*" server.Main "$@"
if [ "${XZJ_JAVA_PROFILE:-compact}" != default ]; then
    set -- -Xms32m -Xmx256m -XX:+UseSerialGC -XX:ActiveProcessorCount=2 "$@"
fi
exec "$java" "$@"
