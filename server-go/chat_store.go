package main

import (
	"bytes"
	"crypto/sha256"
	"encoding/binary"
	"errors"
	"math"
	"os"
	"path/filepath"
	"sync"
	"time"

	bolt "go.etcd.io/bbolt"
	"google.golang.org/protobuf/encoding/protowire"
)

const privateChatPageSize = 100

type privateChatStore struct {
	mu sync.Mutex
	db *bolt.DB
}

func (store *privateChatStore) open(dataDir string) (*bolt.DB, error) {
	store.mu.Lock()
	defer store.mu.Unlock()
	if store.db != nil {
		return store.db, nil
	}
	dir := filepath.Join(dataDir, "chat")
	if err := os.MkdirAll(dir, 0700); err != nil {
		return nil, err
	}
	db, err := bolt.Open(filepath.Join(dir, "messages.db"), 0600, &bolt.Options{Timeout: time.Second})
	if err != nil {
		return nil, err
	}
	store.db = db
	return db, nil
}

func (store *privateChatStore) close() error {
	store.mu.Lock()
	defer store.mu.Unlock()
	if store.db == nil {
		return nil
	}
	err := store.db.Close()
	store.db = nil
	return err
}

func privateChatKey(a, b string) [32]byte {
	if a > b {
		a, b = b, a
	}
	return sha256.Sum256(protowire.AppendString(protowire.AppendString(nil, a), b))
}

// Each conversation is an ordered bucket. The key is both a monotonically
// increasing millisecond timestamp and the exclusive cursor used by the client.
func appendPrivateChat(db *bolt.DB, conversation [32]byte, now int64, encode func(int64) ([]byte, error)) (int64, []byte, error) {
	var timestamp int64
	var payload []byte
	err := db.Update(func(tx *bolt.Tx) error {
		bucket, err := tx.CreateBucketIfNotExists(conversation[:])
		if err != nil {
			return err
		}
		timestamp = now
		if last, _ := bucket.Cursor().Last(); last != nil {
			if len(last) != 8 || binary.BigEndian.Uint64(last) >= math.MaxInt64 {
				return errors.New("invalid private message timestamp")
			}
			timestamp = max(timestamp, int64(binary.BigEndian.Uint64(last))+1)
		}
		payload, err = encode(timestamp)
		if err != nil {
			return err
		}
		return bucket.Put(binary.BigEndian.AppendUint64(nil, uint64(timestamp)), payload)
	})
	return timestamp, payload, err
}

func readPrivateChat(db *bolt.DB, conversation [32]byte, before int64) ([][]byte, bool, error) {
	var page [][]byte
	var more bool
	err := db.View(func(tx *bolt.Tx) error {
		bucket := tx.Bucket(conversation[:])
		if bucket == nil {
			return nil
		}
		cursor := bucket.Cursor()
		var key, value []byte
		if before > 0 {
			key, value = cursor.Seek(binary.BigEndian.AppendUint64(nil, uint64(before)))
		}
		if key == nil {
			key, value = cursor.Last()
		} else {
			key, value = cursor.Prev()
		}
		for key != nil {
			if len(page) == privateChatPageSize {
				more = true
				break
			}
			page = append(page, bytes.Clone(value))
			key, value = cursor.Prev()
		}
		return nil
	})
	return page, more, err
}
