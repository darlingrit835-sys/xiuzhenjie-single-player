package main

import (
	"bytes"
	"encoding/json"
	"errors"
	"fmt"
	"io"
	"maps"
	"os"
	"path/filepath"
	"sync"
	"unicode/utf8"

	"golang.org/x/text/encoding/simplifiedchinese"
)

type Record = map[string]any

const previewAuctionStoreID = "$preview:auction"

var (
	ErrRecordNotFound = errors.New("record not found")
	ErrRecordExists   = errors.New("record already exists")
)

// Store serializes mutations to a Java-compatible JSON object of keyed records.
// Durable mutations are published only after replacement of the save file succeeds.
type Store struct {
	path           string
	mu             sync.RWMutex
	records        map[string]Record
	order          []string
	roleNames      map[string]string
	historyEnabled bool
}

func readLegacyFile(path string) ([]byte, error) {
	data, err := os.ReadFile(path)
	if err != nil {
		return nil, err
	}
	data = bytes.TrimPrefix(data, []byte{0xef, 0xbb, 0xbf})
	if utf8.Valid(data) {
		return data, nil
	}
	return simplifiedchinese.GBK.NewDecoder().Bytes(data)
}

func OpenStore(dataDir, name string) (*Store, error) {
	path := filepath.Join(dataDir, name+".json")
	store := &Store{path: path, records: make(map[string]Record)}
	data, err := readLegacyFile(path)
	if errors.Is(err, os.ErrNotExist) {
		return store, nil
	}
	if err != nil {
		return nil, fmt.Errorf("read %s: %w", path, err)
	}
	decoder := json.NewDecoder(bytes.NewReader(data))
	decoder.UseNumber()
	if err := store.decodeRecords(decoder); err != nil {
		return nil, fmt.Errorf("decode %s: %w", path, err)
	}
	var trailing any
	if err := decoder.Decode(&trailing); !errors.Is(err, io.EOF) {
		return nil, fmt.Errorf("unexpected data after JSON in %s", path)
	}
	return store, nil
}

func (s *Store) decodeRecords(decoder *json.Decoder) error {
	start, err := decoder.Token()
	if err != nil || start == nil {
		return err
	}
	if start != json.Delim('{') {
		return errors.New("expected a JSON object")
	}
	for decoder.More() {
		key, err := decoder.Token()
		if err != nil {
			return err
		}
		var record Record
		if err := decoder.Decode(&record); err != nil {
			return err
		}
		id := key.(string)
		if _, exists := s.records[id]; !exists {
			s.order = append(s.order, id)
		}
		s.records[id] = record
	}
	_, err = decoder.Token()
	return err
}

func cloneRecord(record Record) Record {
	if record == nil {
		return nil
	}
	cloned := make(Record, len(record))
	for key, value := range record {
		cloned[key] = cloneRecordValue(value)
	}
	return cloned
}

func cloneRecordValue(value any) any {
	switch typed := value.(type) {
	case Record:
		return cloneRecord(typed)
	case []any:
		cloned := make([]any, len(typed))
		for index, item := range typed {
			cloned[index] = cloneRecordValue(item)
		}
		return cloned
	case []string:
		return append(typed[:0:0], typed...)
	case []Record:
		cloned := make([]Record, len(typed))
		for index, item := range typed {
			cloned[index] = cloneRecord(item)
		}
		return cloned
	default:
		return value
	}
}

func (s *Store) Get(id string) Record {
	s.mu.RLock()
	defer s.mu.RUnlock()
	return cloneRecord(s.records[id])
}

func (s *Store) project(id string, fields []string) Record {
	s.mu.RLock()
	defer s.mu.RUnlock()
	record := s.records[id]
	if record == nil {
		return nil
	}
	out := make(Record, len(fields))
	for _, key := range fields {
		if value, ok := record[key]; ok {
			out[key] = cloneRecordValue(value)
		}
	}
	return out
}

// roleIDByName follows the save's original ordering for legacy duplicate names.
func (s *Store) roleIDByName(name string) string {
	if name == "" {
		return ""
	}
	s.mu.RLock()
	if s.roleNames != nil {
		id := s.roleNames[name]
		s.mu.RUnlock()
		return id
	}
	s.mu.RUnlock()
	s.mu.Lock()
	defer s.mu.Unlock()
	if s.roleNames == nil {
		s.roleNames = make(map[string]string)
		for _, id := range s.order {
			roleName, _ := s.records[id]["Name"].(string)
			if roleName != "" && !reservedStoreID(id) && s.roleNames[roleName] == "" {
				s.roleNames[roleName] = id
			}
		}
	}
	return s.roleNames[name]
}

func (s *Store) invalidateRoleName(before, after Record) {
	oldName, _ := before["Name"].(string)
	newName, _ := after["Name"].(string)
	if s.roleNames != nil && oldName != newName {
		s.roleNames = nil
	}
}

// Find returns a copy of the first record matching a read-only predicate.
func (s *Store) Find(predicate func(Record) bool) Record {
	_, record := s.find(predicate)
	return record
}

func (s *Store) find(predicate func(Record) bool) (string, Record) {
	s.mu.RLock()
	defer s.mu.RUnlock()
	// The Java account server selects the first role in save-file order.
	for _, id := range s.order {
		if reservedStoreID(id) {
			continue
		}
		record := s.records[id]
		if record != nil && predicate(record) {
			return id, cloneRecord(record)
		}
	}
	return "", nil
}

// StoreTx stages related edits under the same store lock. Records remain valid
// for the duration of the callback only.
type StoreTx struct {
	store   *Store
	pending map[string]Record
	newIDs  []string
}

// View returns a read-only view of the current record without staging a write.
func (tx *StoreTx) View(id string) Record {
	if record, ok := tx.pending[id]; ok {
		return record
	}
	return tx.store.records[id]
}

func (tx *StoreTx) Get(id string) Record {
	if record, ok := tx.pending[id]; ok {
		return record
	}
	record := cloneRecord(tx.store.records[id])
	if record != nil {
		tx.pending[id] = record
	}
	return record
}

func (tx *StoreTx) GetOrCreate(id string) Record {
	if record := tx.Get(id); record != nil {
		return record
	}
	if _, exists := tx.store.records[id]; !exists {
		tx.newIDs = append(tx.newIDs, id)
	}
	record := make(Record)
	tx.pending[id] = record
	return record
}

// Transact publishes one atomic file replacement for all staged records.
func (s *Store) Transact(edit func(*StoreTx) error) error {
	s.mu.Lock()
	defer s.mu.Unlock()
	tx := &StoreTx{store: s, pending: make(map[string]Record)}
	if err := edit(tx); err != nil {
		return err
	}
	return s.commitLocked(tx)
}

func (s *Store) commitLocked(tx *StoreTx) error {
	if len(tx.pending) == 0 {
		return nil
	}
	orderLen := len(s.order)
	s.order = append(s.order, tx.newIDs...)
	before := make(map[string]Record, len(tx.pending))
	for id, record := range tx.pending {
		before[id] = s.records[id]
		s.records[id] = record
	}
	if err := s.saveLocked(); err != nil {
		for id, record := range before {
			s.records[id] = record
		}
		for _, id := range tx.newIDs {
			delete(s.records, id)
		}
		s.order = s.order[:orderLen]
		return fmt.Errorf("save %s: %w", s.path, err)
	}
	for id, record := range tx.pending {
		s.invalidateRoleName(before[id], record)
	}
	return nil
}

func (s *Store) Update(id string, mutate func(Record) error) error {
	return s.UpdateProgress(id, func(record Record) (bool, error) {
		return true, mutate(record)
	})
}

// UpdateProgress permits transient cultivation ticks between periodic durable saves.
func (s *Store) UpdateProgress(id string, mutate func(Record) (persist bool, err error)) error {
	return s.updateProgress(id, cloneRecord, mutate)
}

// Passive cultivation edits only top-level scalar progress. Its shared
// inventory and other nested collections must remain read-only.
func (s *Store) updateCultivationProgress(id string, mutate func(Record) (persist bool, err error)) error {
	return s.updateProgress(id, maps.Clone[Record], mutate)
}

func (s *Store) updateProgress(id string, clone func(Record) Record, mutate func(Record) (bool, error)) error {
	s.mu.Lock()
	defer s.mu.Unlock()
	record := clone(s.records[id])
	if record == nil {
		return ErrRecordNotFound
	}
	persist, err := mutate(record)
	if err != nil {
		return err
	}
	if !persist {
		s.invalidateRoleName(s.records[id], record)
		s.records[id] = record
		return nil
	}
	return s.putLocked(id, record)
}

func (s *Store) flushProgress() error {
	s.mu.Lock()
	defer s.mu.Unlock()
	return s.saveLocked()
}

// InsertUnique checks the identifier and a read-only conflict predicate under the same lock.
func (s *Store) InsertUnique(id string, record Record, conflict func(Record) bool, events ...historyEvent) error {
	s.mu.Lock()
	defer s.mu.Unlock()
	if _, exists := s.records[id]; exists || reservedStoreID(id) {
		return ErrRecordExists
	}
	if conflict != nil {
		for key, current := range s.records {
			if !reservedStoreID(key) && current != nil && conflict(current) {
				return ErrRecordExists
			}
		}
	}
	if !s.historyEnabled || len(events) == 0 {
		return s.putLocked(id, cloneRecord(record))
	}
	tx := &StoreTx{store: s, pending: map[string]Record{id: cloneRecord(record)}, newIDs: []string{id}}
	if err := tx.RecordHistory(events...); err != nil {
		return err
	}
	return s.commitLocked(tx)
}

func (s *Store) Put(id string, record Record) error {
	s.mu.Lock()
	defer s.mu.Unlock()
	return s.putLocked(id, cloneRecord(record))
}

func (s *Store) putLocked(id string, record Record) error {
	previous, existed := s.records[id]
	if !existed {
		s.order = append(s.order, id)
	}
	s.records[id] = record
	if err := s.saveLocked(); err != nil {
		if existed {
			s.records[id] = previous
		} else {
			delete(s.records, id)
			s.order = s.order[:len(s.order)-1]
		}
		return fmt.Errorf("save %s: %w", s.path, err)
	}
	s.invalidateRoleName(previous, record)
	return nil
}

func (s *Store) saveLocked() error {
	dir := filepath.Dir(s.path)
	if err := os.MkdirAll(dir, 0o700); err != nil {
		return err
	}
	file, err := os.CreateTemp(dir, filepath.Base(s.path)+".*.tmp")
	if err != nil {
		return err
	}
	defer os.Remove(file.Name())
	defer file.Close()

	if err := s.encodeRecords(file); err != nil {
		return err
	}
	if err := file.Sync(); err != nil {
		return err
	}
	if err := file.Close(); err != nil {
		return err
	}
	return os.Rename(file.Name(), s.path)
}

func (s *Store) encodeRecords(dest io.Writer) error {
	var out bytes.Buffer
	out.WriteByte('{')
	encoder := json.NewEncoder(&out)
	encoder.SetIndent("  ", "  ")
	encoder.SetEscapeHTML(false)
	for i, id := range s.order {
		if i > 0 {
			out.WriteByte(',')
		}
		out.WriteString("\n  ")
		key, _ := json.Marshal(id)
		out.Write(key)
		out.WriteString(": ")
		if err := encoder.Encode(s.records[id]); err != nil {
			return err
		}
		out.Truncate(out.Len() - 1) // Encoder adds a newline after each record.
	}
	if len(s.order) > 0 {
		out.WriteByte('\n')
	}
	out.WriteString("}\n")
	_, err := dest.Write(out.Bytes())
	return err
}
