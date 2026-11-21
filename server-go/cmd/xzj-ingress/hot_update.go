package main

import (
	"fmt"
	"net/http"
	"os"
	"path/filepath"
	"strings"
	"time"
)

const hotUpdateAndroidPath = "/_xzj/hot/android/"
const fullHotUpdateAndroidPath = hotUpdateAndroidPath + "full/"

type hotUpdateChannel struct {
	dir   string
	slots chan struct{}
	full  bool
}

func newHotUpdateChannel(dir string) (*hotUpdateChannel, error) {
	if !filepath.IsAbs(dir) {
		return nil, fmt.Errorf("hot-update-dir must be an absolute path")
	}
	state, err := os.Stat(dir)
	if err != nil {
		return nil, fmt.Errorf("open hot-update-dir: %w", err)
	}
	if !state.IsDir() {
		return nil, fmt.Errorf("hot-update-dir must be a directory")
	}
	return &hotUpdateChannel{dir: dir, slots: make(chan struct{}, 8)}, nil
}

func newFullHotUpdateChannel(dir string) (*hotUpdateChannel, error) {
	channel, err := newHotUpdateChannel(dir)
	if err != nil {
		return nil, err
	}
	channel.full = true
	channel.slots = make(chan struct{}, 2)
	return channel, nil
}

func (s *hotUpdateChannel) serve(w http.ResponseWriter, r *http.Request) bool {
	base := hotUpdateAndroidPath
	if s.full {
		base = fullHotUpdateAndroidPath
	}
	path, found := strings.CutPrefix(r.URL.Path, base)
	if !found {
		return false
	}
	root := s.dir
	if channelPath, named := strings.CutPrefix(path, "channels/"); s.full && named {
		channel, resource, ok := strings.Cut(channelPath, "/")
		if !ok || !validChannelName(channel) {
			return false
		}
		root = filepath.Join(root, "channels", channel)
		path = resource
	}
	var name, contentType string
	var maxSize int64
	var payload bool
	switch path {
	case "manifest.json":
		name, contentType, maxSize = "manifest.json", "application/json; charset=utf-8", 16<<10
		if s.full {
			maxSize = 320 << 10
		}
	case "manifest.sig":
		name, contentType, maxSize = "manifest.sig", "application/octet-stream", 1<<10
	case "XZJ.HotUpdate.dll.bytes":
		if s.full {
			return false
		}
		name, contentType, maxSize = "XZJ.HotUpdate.dll.bytes", "application/octet-stream", 8<<20
	default:
		if !s.full {
			return false
		}
		if digest, found := strings.CutPrefix(path, "objects/"); found && validReleaseDigest(digest) {
			name, contentType = filepath.Join("objects", digest), "application/octet-stream"
		} else {
			directory, found := strings.CutPrefix(path, "releases/")
			digest, file, ok := strings.Cut(directory, "/")
			if !found || !ok || file != "client-release.zip" || !validReleaseDigest(digest) {
				return false
			}
			name, contentType = filepath.Join("releases", digest, file), "application/zip"
		}
		maxSize, payload = 1<<30, true
	}
	w.Header().Set("Cache-Control", "no-store")
	w.Header().Set("X-Content-Type-Options", "nosniff")
	if r.Method != http.MethodGet && r.Method != http.MethodHead {
		w.Header().Set("Allow", "GET, HEAD")
		http.Error(w, "method not allowed", http.StatusMethodNotAllowed)
		return true
	}
	if !s.full || payload {
		select {
		case s.slots <- struct{}{}:
			defer func() { <-s.slots }()
		default:
			http.Error(w, "update channel busy", http.StatusServiceUnavailable)
			return true
		}
	}
	file, err := os.Open(filepath.Join(root, name))
	if err != nil {
		http.NotFound(w, r)
		return true
	}
	defer file.Close()
	status, err := file.Stat()
	if err != nil || !status.Mode().IsRegular() || status.Size() == 0 || status.Size() > maxSize {
		http.NotFound(w, r)
		return true
	}
	w.Header().Set("Content-Type", contentType)
	if payload {
		_ = http.NewResponseController(w).SetWriteDeadline(time.Now().Add(6 * time.Minute))
		w.Header().Set("Cache-Control", "public, max-age=31536000, immutable")
	}
	http.ServeContent(w, r, filepath.Base(name), status.ModTime(), file)
	return true
}

func validChannelName(channel string) bool {
	return len(channel) > 0 && len(channel) <= 10 && channel[0] != '0' &&
		strings.IndexFunc(channel, func(c rune) bool { return c < '0' || c > '9' }) == -1
}

func validReleaseDigest(digest string) bool {
	return len(digest) == 64 && strings.IndexFunc(digest, func(c rune) bool {
		return !(c >= '0' && c <= '9' || c >= 'a' && c <= 'f')
	}) == -1
}
