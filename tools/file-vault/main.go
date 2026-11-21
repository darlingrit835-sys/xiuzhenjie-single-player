package main

import (
	"crypto/rand"
	"crypto/sha256"
	"crypto/subtle"
	"embed"
	"encoding/json"
	"errors"
	"flag"
	"io"
	"io/fs"
	"log"
	"mime"
	"net"
	"net/http"
	"os"
	"path"
	"path/filepath"
	"slices"
	"strings"
	"syscall"
	"time"
	"unicode"
	"unicode/utf8"

	"xiuzhenjie/file-vault/internal/releases"
)

//go:embed index.html style.css app.js
var assets embed.FS

const (
	gib          = 1 << 30
	maxFile      = 4 * gib
	maxStorage   = 15 * gib
	reserveSpace = 10 * gib
	tempPrefix   = ".upload-"
)

type vault struct {
	root       *os.Root
	password   [sha256.Size]byte
	upload     chan struct{}
	releaseDir string
}

type storedFile struct {
	Name     string    `json:"name"`
	Size     int64     `json:"size"`
	Modified time.Time `json:"modified"`
}

func main() {
	listen := flag.String("listen", "127.0.0.1:18192", "loopback IPv4 listener")
	data := flag.String("data", "", "directory for uploaded files")
	secret := flag.String("password-file", "", "file containing the login password")
	releaseDir := flag.String("release-dir", "", "directory for approved Android releases")
	publish := flag.String("publish-existing", "", "publish an existing APK and exit")
	flag.Parse()

	host, _, err := net.SplitHostPort(*listen)
	if err != nil || host != "127.0.0.1" {
		log.Fatal("listen must use 127.0.0.1")
	}
	password, err := os.ReadFile(*secret)
	if err != nil {
		log.Fatal(err)
	}
	value := strings.TrimSpace(string(password))
	if len(value) < 24 {
		log.Fatal("password must contain at least 24 characters")
	}
	root, err := os.OpenRoot(*data)
	if err != nil {
		log.Fatal(err)
	}
	defer root.Close()
	site := &vault{root: root, password: sha256.Sum256([]byte(value)), upload: make(chan struct{}, 1), releaseDir: *releaseDir}
	if *publish != "" {
		version, err := site.publish(*publish)
		if err != nil {
			log.Fatal(err)
		}
		log.Printf("published Android version %d", version)
		return
	}
	if err := site.clearPartialUploads(); err != nil {
		log.Fatal(err)
	}

	routes := http.NewServeMux()
	routes.HandleFunc("GET /{$}", page("index.html", "text/html; charset=utf-8"))
	routes.HandleFunc("GET /style.css", page("style.css", "text/css; charset=utf-8"))
	routes.HandleFunc("GET /app.js", page("app.js", "text/javascript; charset=utf-8"))
	routes.HandleFunc("GET /api/files", site.list)
	routes.HandleFunc("POST /api/files", site.save)
	routes.HandleFunc("GET /download", site.download)

	server := &http.Server{
		Addr:              *listen,
		Handler:           site.authorize(routes),
		ReadHeaderTimeout: 10 * time.Second,
		IdleTimeout:       time.Minute,
		ReadTimeout:       3 * time.Hour,
		WriteTimeout:      4 * time.Hour,
		MaxHeaderBytes:    32 << 10,
	}
	log.Printf("listening on %s", *listen)
	log.Fatal(server.ListenAndServe())
}

func page(name, contentType string) http.HandlerFunc {
	contents, err := assets.ReadFile(name)
	if err != nil {
		panic(err)
	}
	return func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Content-Type", contentType)
		if r.Method != http.MethodHead {
			_, _ = w.Write(contents)
		}
	}
}

func (v *vault) authorize(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Cache-Control", "no-store")
		w.Header().Set("X-Content-Type-Options", "nosniff")
		w.Header().Set("Referrer-Policy", "no-referrer")
		w.Header().Set("Cross-Origin-Resource-Policy", "same-origin")
		w.Header().Set("Content-Security-Policy", "default-src 'none'; script-src 'self'; style-src 'self'; connect-src 'self'; base-uri 'none'; frame-ancestors 'none'; form-action 'none'")
		host, _, err := net.SplitHostPort(r.Host)
		if err != nil || (host != "127.0.0.1" && host != "localhost") {
			http.Error(w, "地址无效", http.StatusBadRequest)
			return
		}
		user, password, ok := r.BasicAuth()
		digest := sha256.Sum256([]byte(password))
		if !ok || user != "vault" || subtle.ConstantTimeCompare(v.password[:], digest[:]) != 1 {
			w.Header().Set("WWW-Authenticate", `Basic realm="XZJ File Vault", charset="UTF-8"`)
			http.Error(w, "需要登录", http.StatusUnauthorized)
			return
		}
		next.ServeHTTP(w, r)
	})
}

func validName(name string) bool {
	return name != "" && len(name) <= 240 && utf8.ValidString(name) && !strings.HasPrefix(name, ".") &&
		!strings.ContainsAny(name, `/\`) && strings.TrimSpace(name) == name &&
		strings.IndexFunc(name, func(r rune) bool { return unicode.IsControl(r) || unicode.Is(unicode.Cf, r) }) == -1
}

func (v *vault) clearPartialUploads() error {
	entries, err := fs.ReadDir(v.root.FS(), ".")
	if err != nil {
		return err
	}
	for _, entry := range entries {
		if strings.HasPrefix(entry.Name(), tempPrefix) && entry.Type().IsRegular() {
			if err := v.root.Remove(entry.Name()); err != nil {
				return err
			}
		}
	}
	return nil
}

func (v *vault) files() ([]storedFile, int64, error) {
	entries, err := fs.ReadDir(v.root.FS(), ".")
	if err != nil {
		return nil, 0, err
	}
	files := make([]storedFile, 0, len(entries))
	var bytes int64
	for _, entry := range entries {
		if !validName(entry.Name()) || !entry.Type().IsRegular() {
			continue
		}
		info, err := entry.Info()
		if err != nil {
			return nil, 0, err
		}
		files = append(files, storedFile{entry.Name(), info.Size(), info.ModTime()})
		bytes += info.Size()
	}
	return files, bytes, nil
}

func (v *vault) list(w http.ResponseWriter, _ *http.Request) {
	files, bytes, err := v.files()
	if err != nil {
		log.Printf("list: %v", err)
		http.Error(w, "读取文件列表失败", http.StatusInternalServerError)
		return
	}
	slices.SortStableFunc(files, func(a, b storedFile) int { return b.Modified.Compare(a.Modified) })
	var publicVersion int32
	if v.releaseDir != "" {
		if release, err := releases.Latest(v.releaseDir); err == nil {
			publicVersion = release.Version
		}
	}
	w.Header().Set("Content-Type", "application/json; charset=utf-8")
	_ = json.NewEncoder(w).Encode(struct {
		Files         []storedFile `json:"files"`
		Used          int64        `json:"used"`
		Limit         int64        `json:"limit"`
		Max           int64        `json:"max"`
		PublicVersion int32        `json:"publicVersion,omitempty"`
	}{files, bytes, maxStorage, maxFile, publicVersion})
}

func (v *vault) save(w http.ResponseWriter, r *http.Request) {
	if r.Header.Get("X-File-Vault") != "1" {
		http.Error(w, "请从文件站页面上传", http.StatusForbidden)
		return
	}
	name := r.URL.Query().Get("name")
	if !validName(name) {
		http.Error(w, "文件名无效，请重命名后重试", http.StatusBadRequest)
		return
	}
	size := r.ContentLength
	if size < 0 {
		http.Error(w, "需要提供文件大小", http.StatusLengthRequired)
		return
	}
	if size > maxFile {
		http.Error(w, "单文件上限为 4 GiB", http.StatusRequestEntityTooLarge)
		return
	}

	select {
	case v.upload <- struct{}{}:
		defer func() { <-v.upload }()
	default:
		http.Error(w, "有文件正在上传，请稍后重试", http.StatusTooManyRequests)
		return
	}
	_, used, err := v.files()
	var disk syscall.Statfs_t
	if err == nil {
		err = syscall.Statfs(v.root.Name(), &disk)
	}
	if err != nil {
		log.Printf("capacity: %v", err)
		http.Error(w, "读取存储空间失败", http.StatusInternalServerError)
		return
	}
	free := uint64(disk.Bavail) * uint64(disk.Bsize)
	if size > maxStorage-used || free < uint64(size+reserveSpace) {
		http.Error(w, "可用存储空间不足", http.StatusInsufficientStorage)
		return
	}
	if err := v.store(w, r, name, size); err != nil {
		log.Printf("upload: %v", err)
		http.Error(w, "上传未完成，请重试", http.StatusInternalServerError)
	}
}

func (v *vault) store(w http.ResponseWriter, r *http.Request, name string, size int64) error {
	staging := tempPrefix + rand.Text()
	file, err := v.root.OpenFile(staging, os.O_CREATE|os.O_EXCL|os.O_WRONLY, 0600)
	if err != nil {
		return err
	}
	defer v.root.Remove(staging)
	written, writeErr := io.Copy(file, http.MaxBytesReader(w, r.Body, size))
	if writeErr == nil && written != size {
		writeErr = io.ErrUnexpectedEOF
	}
	if writeErr == nil {
		writeErr = file.Sync()
	}
	if err := errors.Join(writeErr, file.Close()); err != nil {
		return err
	}

	// Keep earlier files intact when a build uses the same filename again.
	destination := name
	for {
		err = v.root.Link(staging, destination)
		if !errors.Is(err, fs.ErrExist) {
			break
		}
		ext := path.Ext(name)
		if len(ext) > 30 {
			ext = ""
		}
		stem := strings.TrimSuffix(name, ext)
		suffix := " (" + rand.Text()[:10] + ")" + ext
		for len(stem)+len(suffix) > 240 {
			_, width := utf8.DecodeLastRuneInString(stem)
			stem = stem[:len(stem)-width]
		}
		destination = stem + suffix
	}
	if err != nil {
		return err
	}
	folder, err := v.root.Open(".")
	if err != nil {
		return err
	}
	if err := errors.Join(folder.Sync(), folder.Close()); err != nil {
		return err
	}
	w.Header().Set("Content-Type", "application/json; charset=utf-8")
	version, publishErr := v.publish(destination)
	if publishErr != nil {
		log.Printf("publish APK: %v", publishErr)
	}
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(struct {
		Name         string `json:"name"`
		Version      int32  `json:"publishedVersion,omitempty"`
		PublishError bool   `json:"publishError,omitempty"`
	}{destination, version, publishErr != nil})
	return nil
}

func (v *vault) publish(name string) (int32, error) {
	if v.releaseDir == "" || !validName(name) || !strings.EqualFold(path.Ext(name), ".apk") {
		return 0, nil
	}
	file, err := v.root.Open(name)
	if err != nil {
		return 0, err
	}
	defer file.Close()
	info, err := file.Stat()
	if err != nil {
		return 0, err
	}
	if !info.Mode().IsRegular() {
		return 0, fs.ErrInvalid
	}
	return releases.Publish(v.releaseDir, file, filepath.Join(v.root.Name(), name), name, info.Size())
}

func (v *vault) download(w http.ResponseWriter, r *http.Request) {
	name := r.URL.Query().Get("name")
	if !validName(name) {
		http.NotFound(w, r)
		return
	}
	info, err := v.root.Lstat(name)
	if err != nil || !info.Mode().IsRegular() {
		http.NotFound(w, r)
		return
	}
	file, err := v.root.Open(name)
	if err != nil {
		http.NotFound(w, r)
		return
	}
	defer file.Close()
	w.Header().Set("Content-Type", "application/octet-stream")
	w.Header().Set("Content-Disposition", mime.FormatMediaType("attachment", map[string]string{"filename": name}))
	w.Header().Set("Content-Security-Policy", "sandbox")
	http.ServeContent(w, r, name, info.ModTime(), file)
}
