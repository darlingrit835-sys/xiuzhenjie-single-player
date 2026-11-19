package main

import (
	"errors"
	"os"
	"path/filepath"
	"testing"
)

func TestSocialRoleNameIndexFollowsCommittedRoleNames(t *testing.T) {
	s, err := OpenStore(t.TempDir(), "roles")
	if err != nil {
		t.Fatal(err)
	}
	for _, id := range []string{historyOutboxID, "older", "newer"} {
		if err := s.Put(id, Record{"Name": "旧名"}); err != nil {
			t.Fatal(err)
		}
	}
	if s.roleIDByName("旧名") != "older" {
		t.Fatal("lookup did not preserve legacy save order")
	}
	rename := func(tx *StoreTx) error {
		tx.Get("older")["Name"] = "新名"
		return nil
	}
	if err := s.Transact(func(tx *StoreTx) error {
		_ = rename(tx)
		return ErrRecordExists
	}); !errors.Is(err, ErrRecordExists) {
		t.Fatal(err)
	}
	if s.roleIDByName("新名") != "" || s.roleIDByName("旧名") != "older" {
		t.Fatal("an aborted rename changed the index")
	}
	if err := s.Transact(rename); err != nil {
		t.Fatal(err)
	}
	if s.roleIDByName("新名") != "older" || s.roleIDByName("旧名") != "newer" {
		t.Fatal("the committed rename was not visible")
	}
	if err := s.InsertUnique("third", Record{"Name": "第三人"}, nil); err != nil {
		t.Fatal(err)
	}
	if s.roleIDByName("第三人") != "third" {
		t.Fatal("a newly created role was missing from the index")
	}
}

func TestLegacyRoleOrderSurvivesSaveAndReopen(t *testing.T) {
	dir := t.TempDir()
	save := []byte("{\"z-old\":{\"UserId\":\"same\",\"Id\":\"z-old\"},\"a-new\":{\"UserId\":\"same\",\"Id\":\"a-new\"}}")
	if err := os.WriteFile(filepath.Join(dir, "roles.json"), save, 0o600); err != nil {
		t.Fatal(err)
	}
	first := func() {
		t.Helper()
		store, err := OpenStore(dir, "roles")
		if err != nil {
			t.Fatal(err)
		}
		role := store.Find(func(r Record) bool { return r["UserId"] == "same" })
		if role["Id"] != "z-old" {
			t.Fatalf("expected the first legacy role, got %v", role["Id"])
		}
		if err := store.Update("a-new", func(r Record) error {
			r["LastOnline"] = int64(123)
			return nil
		}); err != nil {
			t.Fatal(err)
		}
	}
	first()
	first()
}

func TestStoreTransactionCommitsWalletsAndAuctionTogether(t *testing.T) {
	dir := t.TempDir()
	store, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	if err := store.Put("buyer", Record{"ShinyStone": 100}); err != nil {
		t.Fatal(err)
	}
	if err := store.Put("previous", Record{"ShinyStone": 50}); err != nil {
		t.Fatal(err)
	}
	apply := func(tx *StoreTx) error {
		tx.Get("buyer")["ShinyStone"] = 60
		tx.Get("previous")["ShinyStone"] = 80
		tx.GetOrCreate(previewAuctionStoreID)["Bidder"] = "buyer"
		return nil
	}
	originalPath := store.path
	store.path = dir // Renaming a regular file onto a directory must fail.
	if err := store.Transact(apply); err == nil {
		t.Fatal("wanted failed save")
	}
	store.path = originalPath
	if roleInt(store.Get("buyer"), "ShinyStone") != 100 ||
		roleInt(store.Get("previous"), "ShinyStone") != 50 ||
		store.Get(previewAuctionStoreID) != nil {
		t.Fatal("failed save leaked staged records")
	}
	if err := store.Transact(func(tx *StoreTx) error {
		_ = apply(tx)
		return ErrRecordNotFound
	}); !errors.Is(err, ErrRecordNotFound) {
		t.Fatal(err)
	}
	if err := store.Transact(apply); err != nil {
		t.Fatal(err)
	}
	reopened, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	if roleInt(reopened.Get("buyer"), "ShinyStone") != 60 ||
		roleInt(reopened.Get("previous"), "ShinyStone") != 80 ||
		roleString(reopened.Get(previewAuctionStoreID), "Bidder") != "buyer" {
		t.Fatal("committed transaction incomplete after reopening")
	}
	if reopened.Find(func(r Record) bool { return r["Bidder"] != nil }) != nil {
		t.Fatal("auction state visible through normal role selection")
	}
}
