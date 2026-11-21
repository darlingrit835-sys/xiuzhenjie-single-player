package main

import (
	"errors"
	"sync"
	"sync/atomic"
	"testing"
	"time"
)

func TestTrialSignInCalendarAndLegacyLogin(t *testing.T) {
	before := time.Date(2026, time.November, 18, 15, 59, 59, 0, time.UTC)
	after := before.Add(time.Second)
	role := Record{"SignInDay": 2, "IsGetSignIn": true, "LastOnline": before.UnixMilli()}
	syncTrialSignInLogin(role, before)
	if roleString(role, "TrialSignInDate") != "2026-11-18" || !roleBool(role, "IsGetSignIn") {
		t.Fatal(role)
	}
	syncTrialSignInLogin(role, after)
	if roleBool(role, "IsGetSignIn") || roleInt(role, "SignInDay") != 2 {
		t.Fatal(role)
	}
}

func TestTrialSignInWritesOneLetterAndCapsAtSeven(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	peer, conn := auctionTestRole(t, s, "recipient", nil)
	now := time.Now()
	view := auctionTestMessage(t, s, 1002018, nil)
	s.handleGame(peer, view)
	out := auctionTestReply(t, s, conn, view, 2000)
	if out.Get(field(out, "SignInAward")).List().Len() != len(trialSignInStones) || len(out.GetUnknown()) == 0 {
		t.Fatal(out)
	}
	claim := auctionTestMessage(t, s, 1002021, nil)
	s.handleGame(peer, claim)
	if !getBool(auctionTestReply(t, s, conn, claim, 4368), "IsSuccess") {
		t.Fatal("first claim did not acknowledge")
	}
	conn.receive(t, s, 1003007, 0, "LetterNotifyResp_1003007")
	s.handleGame(peer, claim)
	if getBool(auctionTestReply(t, s, conn, claim, 4368), "IsSuccess") {
		t.Fatal("repeat claim would advance the original client a second time")
	}
	// Advancing one calendar day, including skipping a day, cannot skip or
	// duplicate a reward. Concurrent tabs must issue exactly one letter.
	for day := 1; day < len(trialSignInStones); day++ {
		current := now.AddDate(0, 0, 2*day)
		var issued atomic.Int32
		var group sync.WaitGroup
		for range 5 {
			group.Go(func() {
				err := s.issueTrialSignInLetter("recipient", current)
				if err == nil {
					issued.Add(1)
				} else if !errors.Is(err, previewReject(4368)) {
					t.Errorf("claim failed: %v", err)
				}
			})
		}
		group.Wait()
		if issued.Load() != 1 {
			t.Fatalf("day %d issued %d rewards", day+1, issued.Load())
		}
	}
	if err := s.issueTrialSignInLetter("recipient", now.AddDate(0, 0, 20)); !errors.Is(err, previewReject(4368)) {
		t.Fatalf("eighth reward: %v", err)
	}
	s = auctionTestServer(t, dir)
	role := s.roles.Get("recipient")
	if roleInt(role, "SignInDay") != 7 || len(trialLetters(role)) != 7 || roleInt(role, "ShinyStone") != 1000 {
		t.Fatal("reopened progress, mail or unclaimed wallet mismatch", role)
	}
	for _, id := range []string{"trial-signin-1", "trial-signin-7"} {
		callMail(t, s, "recipient", 1003006, id, nil, 4206)
		callMail(t, s, "recipient", 1003006, id, nil, 4206)
	}
	if balance := roleInt(s.roles.Get("recipient"), "ShinyStone"); balance != 1350 {
		t.Fatal(balance)
	}
}

func TestTrialSignInVIPAndFailedPersistence(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	owner := "vip"
	if err := s.roles.Put(owner, Record{"Id": owner, "VipLevel": 6}); err != nil {
		t.Fatal(err)
	}
	now := time.Now()
	path := s.roles.path
	s.roles.path = t.TempDir() // Rename over a directory fails.
	if err := s.issueTrialSignInLetter(owner, now); err == nil {
		t.Fatal("acknowledged unwritten sign-in")
	}
	s.roles.path = path
	if role := s.roles.Get(owner); roleInt(role, "SignInDay") != 0 || len(trialLetters(role)) != 0 {
		t.Fatal(role)
	}
	if err := s.issueTrialSignInLetter(owner, now); err != nil {
		t.Fatal(err)
	}
	award := mailAwards(trialLetter(s.roles.Get(owner), "trial-signin-1"))[0].(Record)
	if roleInt(award, "GoodsCount") != 100 {
		t.Fatal(award)
	}
}
