package opslog

import (
	"errors"
	"fmt"
	"io"
	"os"
	"path/filepath"
	"sync"
	"time"
)

const maxFileSize = 16 << 20

// Writer stores operational logs in size-limited segments grouped by UTC date.
// Opening a writer never truncates or removes existing segments.
type Writer struct {
	mu          sync.Mutex
	dir, prefix string
	now         func() time.Time
	maxSize     int64
	file        *os.File
	day         string
	size        int64
	nextSegment int
}

// Open creates a private operational log writer in dir. Prefix is a file stem.
func Open(dir, prefix string) (*Writer, error) {
	return open(dir, prefix, time.Now, maxFileSize)
}

func open(dir, prefix string, now func() time.Time, maxSize int64) (*Writer, error) {
	if dir == "" || prefix == "" || prefix == "." || prefix == ".." || len(prefix) > 64 {
		return nil, errors.New("operational log requires a directory and a simple file prefix")
	}
	for _, ch := range prefix {
		if (ch < 'a' || ch > 'z') && (ch < 'A' || ch > 'Z') &&
			(ch < '0' || ch > '9') && ch != '-' && ch != '_' && ch != '.' {
			return nil, errors.New("operational log prefix contains an invalid character")
		}
	}
	if err := os.MkdirAll(dir, 0o700); err != nil {
		return nil, err
	}
	w := &Writer{dir: dir, prefix: prefix, now: now, maxSize: maxSize}
	if err := w.rotate(now().UTC().Format(time.DateOnly)); err != nil {
		return nil, err
	}
	return w, nil
}

func (w *Writer) rotate(day string) error {
	next := w.nextSegment
	if day != w.day {
		next = 0
	}
	for segment := next; ; segment++ {
		name := fmt.Sprintf("%s-%s.%03d.log", w.prefix, day, segment)
		file, err := os.OpenFile(filepath.Join(w.dir, name), os.O_CREATE|os.O_EXCL|os.O_WRONLY, 0o600)
		if errors.Is(err, os.ErrExist) {
			continue
		}
		if err != nil {
			return err
		}
		previous := w.file
		w.file, w.day, w.size, w.nextSegment = file, day, 0, segment+1
		if previous != nil {
			return previous.Close()
		}
		return nil
	}
}

func (w *Writer) Write(p []byte) (int, error) {
	w.mu.Lock()
	defer w.mu.Unlock()
	if w.file == nil {
		return 0, os.ErrClosed
	}
	day := w.now().UTC().Format(time.DateOnly)
	written := 0
	for len(p) != 0 {
		remaining := w.maxSize - w.size
		length := int64(len(p))
		// Keep ordinary log entries together. An unusually large write is
		// split across segments so it cannot bypass the file size limit.
		if day != w.day || remaining == 0 || length <= w.maxSize && length > remaining {
			if err := w.rotate(day); err != nil {
				return written, err
			}
			remaining = w.maxSize
		}
		amount := min(length, remaining)
		n, err := w.file.Write(p[:amount])
		w.size += int64(n)
		written += n
		if err != nil {
			return written, err
		}
		if int64(n) != amount {
			return written, io.ErrShortWrite
		}
		p = p[n:]
	}
	return written, nil
}

func (w *Writer) Close() error {
	w.mu.Lock()
	defer w.mu.Unlock()
	if w.file == nil {
		return nil
	}
	err := w.file.Close()
	w.file = nil
	return err
}
