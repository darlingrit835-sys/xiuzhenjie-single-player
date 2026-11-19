package opslog

import (
	"errors"
	"io"
	"os"
	"path/filepath"
	"runtime"
	"testing"
	"time"
)

func TestWriterRotatesByUTCAndSizeWithoutOverwriting(t *testing.T) {
	dir := filepath.Join(t.TempDir(), "logs")
	now := time.Date(2026, time.November, 20, 7, 59, 59, 0, time.FixedZone("UTC+08", 8*60*60))
	clock := func() time.Time { return now }
	writer, err := open(dir, "server", clock, 8)
	if err != nil {
		t.Fatal(err)
	}
	defer writer.Close()
	for _, text := range []string{"first", "next"} {
		if n, err := io.WriteString(writer, text); err != nil || n != len(text) {
			t.Fatalf("write %q: %d %v", text, n, err)
		}
	}
	now = now.Add(time.Second)
	if n, err := io.WriteString(writer, "0123456789"); err != nil || n != 10 {
		t.Fatalf("oversized write: %d %v", n, err)
	}
	if err := writer.Close(); err != nil {
		t.Fatal(err)
	}
	if _, err := io.WriteString(writer, "closed"); !errors.Is(err, os.ErrClosed) {
		t.Fatalf("closed writer accepted data: %v", err)
	}
	reopened, err := open(dir, "server", clock, 8)
	if err != nil {
		t.Fatal(err)
	}
	defer reopened.Close()
	if _, err := io.WriteString(reopened, "reopen"); err != nil {
		t.Fatal(err)
	}
	for name, expected := range map[string]string{
		"server-2026-11-19.000.log": "first", "server-2026-11-19.001.log": "next",
		"server-2026-11-20.000.log": "01234567", "server-2026-11-20.001.log": "89", "server-2026-11-20.002.log": "reopen",
	} {
		path := filepath.Join(dir, name)
		contents, err := os.ReadFile(path)
		if err != nil || string(contents) != expected {
			t.Fatalf("%s: got %q (%v)", name, contents, err)
		}
		info, err := os.Stat(path)
		if err != nil || runtime.GOOS != "windows" && info.Mode().Perm()&0o077 != 0 {
			t.Fatalf("%s is not private: %v", name, err)
		}
	}
}
