package main

import (
	"context"
	"net/http/httptest"
	"os"
	"path/filepath"
	"strings"
	"testing"
)

func TestPublicHotUpdateChannelKeepsAccountAndTunnelsPrivate(t *testing.T) {
	dir := t.TempDir()
	for name, content := range map[string][]byte{
		"manifest.json": {123, 125}, "manifest.sig": make([]byte, 1025), "gateway.env": []byte("private"),
	} {
		if err := os.WriteFile(filepath.Join(dir, name), content, 0600); err != nil {
			t.Fatal(err)
		}
	}
	server := newIngress(context.Background(), "test-key-for-private-ingress-testing", "127.0.0.1:1", [32]byte{}, "127.0.0.1:2", "127.0.0.1:3")
	channel, err := newHotUpdateChannel(dir)
	if err != nil {
		t.Fatal(err)
	}
	server.hotUpdate = channel
	for _, test := range []struct {
		method, url string
		code        int
		body        string
	}{
		{"GET", "https://example.test" + hotUpdateAndroidPath + "manifest.json", 200, "{}"},
		{"HEAD", "https://example.test" + hotUpdateAndroidPath + "manifest.json", 200, ""},
		{"GET", "https://example.test" + hotUpdateAndroidPath + "manifest.sig", 404, ""},
		{"GET", "https://example.test" + hotUpdateAndroidPath + "gateway.env", 403, ""},
		{"GET", "https://example.test" + ingressGamePath, 403, ""},
		{"POST", "https://example.test/login", 403, ""},
		{"POST", "https://example.test" + hotUpdateAndroidPath + "manifest.json", 405, ""},
		{"GET", "http://example.test" + hotUpdateAndroidPath + "manifest.json", 403, ""},
	} {
		w := httptest.NewRecorder()
		server.ServeHTTP(w, httptest.NewRequest(test.method, test.url, nil))
		if w.Code != test.code || (test.code == 200 && w.Body.String() != test.body) {
			t.Errorf("%s %s returned %d %q", test.method, test.url, w.Code, w.Body.String())
		}
	}
}

func TestFullHotUpdateServesByDigestAndKeepsLegacySeparate(t *testing.T) {
	root := t.TempDir()
	legacy := filepath.Join(root, "legacy")
	full := filepath.Join(root, "full")
	digest := strings.Repeat("ab", 32)
	version := filepath.Join(full, "releases", digest)
	seeded := filepath.Join(full, "channels", "30")
	seededVersion := filepath.Join(seeded, "releases", digest)
	objects := filepath.Join(seeded, "objects")
	for _, path := range []string{legacy, version, seededVersion, objects} {
		if err := os.MkdirAll(path, 0700); err != nil {
			t.Fatal(err)
		}
	}
	for path, value := range map[string]string{
		filepath.Join(legacy, "manifest.json"):             "legacy",
		filepath.Join(full, "manifest.json"):               "full",
		filepath.Join(full, "manifest.sig"):                "signature",
		filepath.Join(version, "client-release.zip"):       "abcdefgh",
		filepath.Join(seeded, "manifest.json"):             "seeded",
		filepath.Join(seededVersion, "client-release.zip"): "ijklmnop",
		filepath.Join(objects, digest):                     "download",
	} {
		if err := os.WriteFile(path, []byte(value), 0600); err != nil {
			t.Fatal(err)
		}
	}
	server := newIngress(context.Background(), "test-key-for-private-ingress-testing", "127.0.0.1:1", [32]byte{}, "127.0.0.1:2", "127.0.0.1:3")
	var err error
	if server.hotUpdate, err = newHotUpdateChannel(legacy); err != nil {
		t.Fatal(err)
	}
	if server.fullHotUpdate, err = newFullHotUpdateChannel(full); err != nil {
		t.Fatal(err)
	}
	for _, test := range []struct {
		path, body, byteRange string
		code                  int
	}{
		{hotUpdateAndroidPath + "manifest.json", "legacy", "", 200},
		{fullHotUpdateAndroidPath + "manifest.json", "full", "", 200},
		{fullHotUpdateAndroidPath + "manifest.sig", "signature", "", 200},
		{fullHotUpdateAndroidPath + "channels/30/manifest.json", "seeded", "", 200},
		{fullHotUpdateAndroidPath + "channels/30/releases/" + digest + "/client-release.zip", "jk", "bytes=1-2", 206},
		{fullHotUpdateAndroidPath + "channels/30/objects/" + digest, "own", "bytes=1-3", 206},
		{fullHotUpdateAndroidPath + "channels/31/objects/" + digest, "", "", 404},
		{fullHotUpdateAndroidPath + "channels/30/objects/../manifest.sig", "", "", 403},
		{fullHotUpdateAndroidPath + "channels/31/manifest.json", "", "", 404},
		{fullHotUpdateAndroidPath + "channels/../manifest.json", "", "", 403},
		{fullHotUpdateAndroidPath + "releases/" + digest + "/client-release.zip", "bc", "bytes=1-2", 206},
		{fullHotUpdateAndroidPath + "releases/" + strings.Repeat("cd", 32) + "/client-release.zip", "", "", 404},
		{fullHotUpdateAndroidPath + "releases/../client-release.zip", "", "", 403},
		{fullHotUpdateAndroidPath + "releases/" + digest + "/manifest.json", "", "", 403},
		{fullHotUpdateAndroidPath + "client-release.zip", "", "", 403},
	} {
		w := httptest.NewRecorder()
		r := httptest.NewRequest("GET", "https://example.test"+test.path, nil)
		if test.byteRange != "" {
			r.Header.Set("Range", test.byteRange)
		}
		server.ServeHTTP(w, r)
		if w.Code != test.code || (w.Code < 300 && w.Body.String() != test.body) {
			t.Errorf("%s returned %d %q", test.path, w.Code, w.Body.String())
		}
	}
}
