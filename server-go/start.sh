#!/bin/sh
set -eu
cd "$(CDPATH='' cd -- "$(dirname -- "$0")" && pwd)"

go_command="${XZJ_GO:-go}"
if command -v "$go_command" >/dev/null 2>&1; then
    mkdir -p .build
    "$go_command" build -trimpath -o .build/xzj-server .
elif [ ! -x .build/xzj-server ]; then
    printf '%s\n' '请安装 Go 1.26+，或把已编译的 xzj-server 放入 server-go/.build。' >&2
    exit 1
fi
exec ./.build/xzj-server -data "${XZJ_DATA_DIR:-$PWD/../server/data}" "$@"
