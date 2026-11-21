#!/bin/bash
set -euo pipefail

config="$HOME/.config/xzj-file-vault"
remote="xzj-vault-tunnel@82.156.174.166"
url="http://127.0.0.1:18192/"
socket="$config/ctl"

if ! /usr/bin/ssh -S "$socket" -O check "$remote" >/dev/null 2>&1; then
  if ! /usr/bin/ssh -fNT -M -S "$socket" \
    -i "$config/tunnel_ed25519" -o IdentitiesOnly=yes -o BatchMode=yes \
    -o UserKnownHostsFile="$config/known_hosts" -o StrictHostKeyChecking=yes \
    -o ConnectTimeout=8 -o ExitOnForwardFailure=yes \
    -o ServerAliveInterval=25 -o ServerAliveCountMax=3 \
    -L 127.0.0.1:18192:127.0.0.1:18192 "$remote"; then
    printf '\n无法连接文件站。请检查网络后重试。按回车关闭。\n'
    if [[ -t 0 ]]; then read -r; fi
    exit 1
  fi
fi

if [[ "${1:-}" == "--connect-only" ]]; then
  exit
fi

/usr/bin/open "$url"
printf '\n修真界文件站已打开。首次登录的账号为 vault。\n'
printf '输入 p 后按回车可复制密码。回车可直接结束。\n'
if [[ -t 0 ]]; then
  read -r -n 1 action
  if [[ "$action" == p ]]; then
    /usr/bin/tr -d '\n' <"$config/access-token" | /usr/bin/pbcopy
    printf '\n密码已复制，切回网页粘贴即可。\n'
  fi
fi
