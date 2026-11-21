// apk-share exposes published Android releases with a dedicated password.
package main

import (
	"crypto/rand"
	"crypto/sha256"
	"crypto/subtle"
	"flag"
	"html/template"
	"log"
	"mime"
	"net"
	"net/http"
	"os"
	"strconv"
	"strings"
	"time"

	"xiuzhenjie/file-vault/internal/releases"
)

const cookieName = "__Host-xzj_apk"

var page = template.Must(template.New("apk").Parse(`<!doctype html>
<html lang="zh-CN">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>修真界 · APK 下载</title>
<style>
* { box-sizing: border-box; }
body { margin: 0; min-height: 100vh; display: grid; place-items: center; padding: 1rem; background: #eef2fa; color: #1c243a; font: 1rem/1.55 -apple-system, "PingFang SC", "Microsoft YaHei", sans-serif; }
main { width: min(100%, 28rem); padding: 1.65rem; border: 1px solid #dbe1ee; border-radius: 1rem; background: white; }
h1 { margin: 0 0 .5rem; font-size: 1.65rem; }
p { margin: .5rem 0 1.25rem; color: #58647b; overflow-wrap: anywhere; }
label { display: block; margin-bottom: .45rem; font-weight: 600; }
input, button, a { width: 100%; padding: .7rem .85rem; border-radius: .55rem; font: inherit; }
input { border: 1px solid #9daac2; margin-bottom: .8rem; }
button, a { display: block; border: 0; background: #2445bf; color: white; text-align: center; font-weight: 700; cursor: pointer; text-decoration: none; }
.error { color: #a12130; margin-bottom: .7rem; }
</style>
</head>
<body><main>
<h1>修真界 APK 下载</h1>
<p>{{.Release.Name}} · 版本号 {{.Release.Version}} · {{.Size}} MiB</p>
{{if .Unlocked}}
<a href="/download/{{.Release.Version}}">下载 APK</a>
{{else}}
{{if .Error}}<p class="error" role="alert">密码有误，请重试。</p>{{end}}
<form method="post" action="/unlock">
<label for="password">下载密码</label>
<input type="password" id="password" name="password" autocomplete="current-password" required maxlength="128">
<button type="submit">解锁下载</button>
</form>
{{end}}
</main></body>
</html>`))

func main() {
	address := flag.String("listen", "127.0.0.1:18193", "loopback listener")
	releaseDir := flag.String("release-dir", "", "directory for approved Android releases")
	passwordFile := flag.String("password-file", "", "file containing the download password")
	flag.Parse()
	if host, _, err := net.SplitHostPort(*address); err != nil || host != "127.0.0.1" {
		log.Fatal("listen must use 127.0.0.1")
	}
	pass, err := os.ReadFile(*passwordFile)
	if err != nil {
		log.Fatal(err)
	}
	password := strings.TrimSpace(string(pass))
	if len(password) < 12 {
		log.Fatal("download password must contain at least 12 characters")
	}
	if _, err := releases.Latest(*releaseDir); err != nil {
		log.Fatalf("no APK release: %v", err)
	}
	digest := sha256.Sum256([]byte(password))
	token := rand.Text()
	session := sha256.Sum256([]byte(token))
	allowed := func(r *http.Request) bool {
		cookie, err := r.Cookie(cookieName)
		if err != nil {
			return false
		}
		supplied := sha256.Sum256([]byte(cookie.Value))
		return subtle.ConstantTimeCompare(session[:], supplied[:]) == 1
	}
	render := func(w http.ResponseWriter, unlocked, invalid bool) {
		release, err := releases.Latest(*releaseDir)
		if err != nil {
			http.Error(w, "暂无可下载的安装包", http.StatusServiceUnavailable)
			return
		}
		size := strconv.FormatFloat(float64(release.Size)/(1<<20), 'f', 1, 64)
		w.Header().Set("Content-Type", "text/html; charset=utf-8")
		if invalid {
			w.WriteHeader(http.StatusForbidden)
		}
		if err := page.Execute(w, struct {
			Release         releases.Release
			Size            string
			Unlocked, Error bool
		}{release, size, unlocked, invalid}); err != nil {
			log.Printf("render: %v", err)
		}
	}

	routes := http.NewServeMux()
	routes.HandleFunc("GET /{$}", func(w http.ResponseWriter, r *http.Request) {
		render(w, allowed(r), false)
	})
	routes.HandleFunc("POST /unlock", func(w http.ResponseWriter, r *http.Request) {
		r.Body = http.MaxBytesReader(w, r.Body, 4096)
		if err := r.ParseForm(); err != nil {
			http.Error(w, "请求无效", http.StatusBadRequest)
			return
		}
		supplied := sha256.Sum256([]byte(r.PostForm.Get("password")))
		if subtle.ConstantTimeCompare(digest[:], supplied[:]) != 1 {
			render(w, false, true)
			return
		}
		http.SetCookie(w, &http.Cookie{
			Name: cookieName, Value: token, Path: "/", Secure: true, HttpOnly: true,
			SameSite: http.SameSiteStrictMode, MaxAge: int((12 * time.Hour).Seconds()),
		})
		http.Redirect(w, r, "/", http.StatusSeeOther)
	})
	download := func(w http.ResponseWriter, r *http.Request) {
		if !allowed(r) {
			http.Redirect(w, r, "/", http.StatusSeeOther)
			return
		}
		version := r.PathValue("version")
		var release releases.Release
		var err error
		if version == "" {
			release, err = releases.Latest(*releaseDir)
		} else {
			code, parseErr := strconv.ParseInt(version, 10, 32)
			if parseErr != nil || code <= 0 {
				http.NotFound(w, r)
				return
			}
			release, err = releases.Read(*releaseDir, int32(code))
		}
		if err != nil {
			http.NotFound(w, r)
			return
		}
		file, err := os.Open(release.Path(*releaseDir))
		if err != nil {
			http.NotFound(w, r)
			return
		}
		defer file.Close()
		info, err := file.Stat()
		if err != nil || !info.Mode().IsRegular() || info.Size() != release.Size {
			http.NotFound(w, r)
			return
		}
		w.Header().Set("Content-Type", "application/vnd.android.package-archive")
		w.Header().Set("Content-Disposition", mime.FormatMediaType("attachment", map[string]string{"filename": release.Name}))
		http.ServeContent(w, r, release.Name, info.ModTime(), file)
	}
	routes.HandleFunc("GET /download", download)
	routes.HandleFunc("GET /download/{version}", download)

	server := &http.Server{
		ReadHeaderTimeout: 10 * time.Second, ReadTimeout: 30 * time.Second,
		WriteTimeout: 2 * time.Hour, IdleTimeout: time.Minute, MaxHeaderBytes: 32 << 10,
		Handler: http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
			w.Header().Set("Cache-Control", "private, no-store")
			w.Header().Set("X-Content-Type-Options", "nosniff")
			w.Header().Set("Referrer-Policy", "no-referrer")
			w.Header().Set("Content-Security-Policy", "default-src 'none'; style-src 'unsafe-inline'; frame-ancestors 'none'; form-action 'self'; base-uri 'none'")
			routes.ServeHTTP(w, r)
		}),
	}
	listener, err := net.Listen("tcp", *address)
	if err != nil {
		log.Fatal(err)
	}
	log.Fatal(server.Serve(listener))
}
