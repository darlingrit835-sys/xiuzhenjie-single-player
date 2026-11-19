package main

import (
	"context"
	"encoding/json"
	"errors"
	"net/url"
	"strings"
	"testing"
	"time"

	"golang.org/x/crypto/bcrypt"
)

func otherEmailTestCode(code string) string {
	if code == "000000" {
		return "000001"
	}
	return "000000"
}

func TestEmailCodeAwaitingDeliveryAndFailedDeliveryCannotRegister(t *testing.T) {
	s := emailTestServer(t, t.TempDir(), true)
	const email = "delivery@example.com"
	delivery := make(chan string, 1)
	reject := make(chan struct{})
	s.sendRegistrationEmail = func(ctx context.Context, _, code string) error {
		delivery <- code
		select {
		case <-reject:
			return errors.New("test delivery failure")
		case <-ctx.Done():
			return ctx.Err()
		}
	}
	request := url.Values{"email": {email}}
	finished := make(chan accountTestReply, 1)
	go func() {
		reply, err := emailTestRequest(s, "POST", "/user/sendEmailCode", request)
		if err != nil {
			t.Error(err)
		}
		finished <- reply
	}()
	code := <-delivery
	register := url.Values{"email": {email}, "password": {"correct-password"}, "checkCode": {code}}
	emailTestReply(t, s, "POST", "/user/register", "4010", register)
	emailTestReply(t, s, "POST", "/user/sendEmailCode", "429", request)
	close(reject)
	if got := <-finished; got.Code != "4006" || got.DataString != "" {
		t.Fatalf("failed mail got positive acknowledgement: %+v", got)
	}
	emailTestReply(t, s, "POST", "/user/register", "4010", register)
	if s.findEmailAccount(email, "") != nil {
		t.Fatal("failed delivery created an account")
	}
}

func TestEmailCodeExpiryAttemptsResendAndClaim(t *testing.T) {
	var store emailCodeStore
	now := time.Unix(1_700_000_000, 0)
	const email = "expiry@example.com"
	entry, first, err := store.beginSend(email, now)
	if err != nil {
		t.Fatal(err)
	}
	store.finishSend(email, entry, first, now, true)
	if _, _, err := store.beginSend(email, now.Add(time.Second)); !errors.Is(err, errEmailCodeThrottled) {
		t.Fatalf("resend cooldown not enforced: %v", err)
	}
	for range emailCodeMaxAttempts {
		if store.claim(email, otherEmailTestCode(first), now) != nil {
			t.Fatal("wrong code was accepted")
		}
	}
	if store.claim(email, first, now) != nil {
		t.Fatal("code was accepted after exhausting its attempts")
	}
	now = now.Add(emailCodeResendWait)
	entry, next, err := store.beginSend(email, now)
	if err != nil || next == first {
		t.Fatalf("resend did not replace its previous code: %v", err)
	}
	store.finishSend(email, entry, next, now, true)
	if store.claim(email, next, now.Add(emailCodeLifetime)) != nil || store.claim("other@example.com", next, now) != nil {
		t.Fatal("expired or unrelated-email code was accepted")
	}
	if store.claim(email, first, now) != nil || store.claim(email, next, now) != entry || store.claim(email, next, now) != nil {
		t.Fatal("superseded code or concurrent reuse was accepted")
	}
	store.finishClaim(email, entry, true)
	if store.claim(email, next, now) != nil {
		t.Fatal("committed code was accepted twice")
	}
}

func TestEmailCodeRetriesFailedWriteAndActivatesExistingRole(t *testing.T) {
	dir := t.TempDir()
	s := emailTestServer(t, dir, true)
	const (
		email  = "activate@example.com"
		userID = "existing-email-owner"
		roleID = "existing-email-role"
	)
	oldHash, err := bcrypt.GenerateFromPassword([]byte("old password"), bcrypt.MinCost)
	if err != nil {
		t.Fatal(err)
	}
	if err := s.users.Put("e_"+email, Record{"email": email, "userId": userID, "passwordHash": string(oldHash)}); err != nil {
		t.Fatal(err)
	}
	if err := s.roles.Put(roleID, Record{"Id": roleID, "UserId": userID, "Name": "原角色", "Level": 31}); err != nil {
		t.Fatal(err)
	}
	oldLogin := url.Values{"email": {email}, "password": {"old password"}}
	emailTestReply(t, s, "POST", "/login", "4008", oldLogin)
	code := emailTestDeliveredCode(t, s, email, email)
	newLogin := url.Values{"email": {email}, "password": {"new password"}}
	register := url.Values{"email": {email}, "password": {newLogin.Get("password")}, "checkCode": {code}}
	originalPath := s.users.path
	s.users.path = t.TempDir()
	emailTestReply(t, s, "POST", "/user/register", "1001", register)
	s.users.path = originalPath
	if account := s.findEmailAccount(email, ""); roleBool(account, "emailVerified") || roleString(account, "passwordHash") != string(oldHash) {
		t.Fatal("failed save changed the existing account")
	}
	emailTestReply(t, s, "POST", "/user/register", "2000", register)
	reopened := emailTestServer(t, dir, true)
	emailTestReply(t, reopened, "POST", "/login", "2101", oldLogin)
	response := emailTestReply(t, reopened, "POST", "/login", "2000", newLogin)
	var login struct{ UserId string }
	if err := json.Unmarshal([]byte(response.DataString), &login); err != nil || login.UserId != userID || !strings.Contains(response.DataString, roleID) {
		t.Fatalf("activation replaced the account or lost its role: %v", err)
	}
	if account := reopened.findEmailAccount(email, ""); !roleBool(account, "emailVerified") || !accountPasswordMatches(account, newLogin.Get("password")) {
		t.Fatal("verified account did not persist")
	}
}
