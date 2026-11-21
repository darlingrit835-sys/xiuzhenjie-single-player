#!/bin/sh
set -eu
umask 077
ip=82.156.174.166
acme=/var/lib/xzj-apk-acme
if [ "${1:-}" != --install-existing ]; then
    /opt/xzj-apk-share/lego run --path "$acme" --accept-tos --server letsencrypt --profile shortlived --domains "$ip" --http --http.webroot /var/lib/xzj-apk-acme-http --http-timeout 25
fi
tls_dir=/var/lib/xzj-apk-edge
next=$(mktemp "$tls_dir/.site.XXXXXXXX")
trap 'rm -f "$next"' EXIT
cat "$acme/certificates/$ip.crt" "$acme/certificates/$ip.key" > "$next"
chgrp xzj-apk-edge "$next"
chmod 0640 "$next"
mv -f "$next" "$tls_dir/site.pem"
