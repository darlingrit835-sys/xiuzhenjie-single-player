package main

import (
	"bytes"
	"context"
	"encoding/json"
	"net/http/httptest"
	"net/url"
	"os"
	"path/filepath"
	"strings"
	"sync"
	"testing"
	"time"

	"golang.org/x/crypto/bcrypt"
)

type accountTestReply struct {
	Code, Msg, DataString string
}

func emailTestRequest(s *Server, method, path string, form url.Values) (accountTestReply, error) {
	request := httptest.NewRequest(method, path, strings.NewReader(form.Encode()))
	request.Header.Set("Content-Type", "application/x-www-form-urlencoded")
	response := httptest.NewRecorder()
	s.accountHandler().ServeHTTP(response, request)
	var envelope struct {
		MsgBody struct{ BizInfo accountTestReply }
	}
	err := json.Unmarshal(response.Body.Bytes(), &envelope)
	return envelope.MsgBody.BizInfo, err
}

func emailTestReply(t *testing.T, s *Server, method, path, code string, form url.Values) accountTestReply {
	t.Helper()
	reply, err := emailTestRequest(s, method, path, form)
	if err != nil || reply.Code != code || reply.Msg == "" {
		t.Fatalf("%s %s: expected code %s, got %+v (%v)", method, path, code, reply, err)
	}
	return reply
}

func emailTestServer(t *testing.T, dir string, enabled bool) *Server {
	t.Helper()
	s, err := openServer(Config{PublicIP: "127.0.0.1", GamePort: 30666}, dir)
	if err != nil {
		t.Fatal(err)
	}
	s.emailRegistration = enabled
	if enabled {
		if err := s.initAccountSessions(); err != nil {
			t.Fatal(err)
		}
	}
	return s
}

func emailTestDeliveredCode(t *testing.T, s *Server, input, canonical string) string {
	t.Helper()
	var recipient, code string
	s.sendRegistrationEmail = func(_ context.Context, address, value string) error {
		recipient, code = address, value
		return nil
	}
	reply := emailTestReply(t, s, "POST", "/user/sendEmailCode", "2000", url.Values{"email": {input}})
	if recipient != canonical || !validRegistrationEmailCode(code) || reply.DataString != "true" || strings.Contains(reply.Msg, code) {
		t.Fatal("code was not privately delivered to the normalized address")
	}
	return code
}

func TestEmailAccountRegistersOnceAndLogsInAfterReopen(t *testing.T) {
	dir := t.TempDir()
	s := emailTestServer(t, dir, true)
	const (
		email    = "trial+new@example.com"
		password = "录入 time pass+"
	)
	form := url.Values{"email": {" Trial+New@Example.com "}, "password": {password}}
	emailTestReply(t, s, "POST", "/login", "2101", form)
	if got := s.findEmailAccount(email, ""); got != nil {
		t.Fatal("email login created an account without registration")
	}
	emailTestReply(t, s, "POST", "/user/sendEmailCode", "4006", url.Values{"email": {email}})
	emailTestReply(t, s, "POST", "/user/register", "4006", url.Values{"email": {email}, "password": {password}, "checkCode": {"123456"}})
	code := emailTestDeliveredCode(t, s, form.Get("email"), email)
	emailTestReply(t, s, "POST", "/user/register", "5001", form)
	emailTestReply(t, s, "POST", "/user/register", "4010", url.Values{"email": {"other@example.com"}, "password": {password}, "checkCode": {code}})
	emailTestReply(t, s, "POST", "/user/register", "4010", url.Values{"email": {email}, "password": {password}, "checkCode": {otherEmailTestCode(code)}})
	var registrations sync.WaitGroup
	responses := make(chan accountTestReply, 2)
	for _, variant := range []string{"TRIAL+NEW@EXAMPLE.COM", "  " + email + " "} {
		registrations.Go(func() {
			reply, err := emailTestRequest(s, "POST", "/user/register", url.Values{"email": {variant}, "password": {password}, "checkCode": {code}})
			if err != nil {
				t.Error(err)
			}
			responses <- reply
		})
	}
	registrations.Wait()
	close(responses)
	counts := make(map[string]int)
	for reply := range responses {
		counts[reply.Code]++
		if reply.Code == "2000" && reply.DataString != "true" {
			t.Fatalf("registered account should return legacy true dataString, got %+v", reply)
		}
	}
	if counts["2000"] != 1 || counts["2201"]+counts["4010"] != 1 || s.emailCodes.claim(email, code, time.Now()) != nil {
		t.Fatalf("concurrent registration did not consume its challenge exactly once: %v", counts)
	}
	const roleID = "fixture-role"
	id := roleString(s.findEmailAccount(email, ""), "userId")
	if err := s.roles.Put(roleID, Record{"Id": roleID, "Name": "试用角色", "UserId": id, "Level": 31}); err != nil {
		t.Fatal(err)
	}
	reopened := emailTestServer(t, dir, true)
	account := reopened.findEmailAccount(email, "")
	if id == "" || roleString(account, "userId") != id || roleString(account, "email") != email || account["password"] != nil || !roleBool(account, "emailVerified") ||
		bcrypt.CompareHashAndPassword([]byte(roleString(account, "passwordHash")), []byte(password)) != nil {
		t.Fatal("reopened account has a missing identity or invalid password hash")
	}
	contents, err := os.ReadFile(filepath.Join(dir, "users.json"))
	if err != nil || bytes.Contains(contents, []byte(password)) {
		t.Fatalf("plaintext password persisted or storage inaccessible: %v", err)
	}
	info := emailTestReply(t, reopened, "POST", "/login", "2000", form)
	var login struct {
		UserID, MobilePhone string
		GameServersInfoList []struct {
			GameServerIpManageId string
			CurrentRoleVO        struct{ ID, ServersRoleId string }
		}
	}
	if err := json.Unmarshal([]byte(info.DataString), &login); err != nil {
		t.Fatal(err)
	}
	if login.UserID != id || login.MobilePhone != email || len(login.GameServersInfoList) != 1 || strings.Contains(info.DataString, `"email":`) {
		t.Fatal("login data did not preserve the client-visible identity")
	}
	game := login.GameServersInfoList[0]
	if game.CurrentRoleVO.ID != roleID || game.CurrentRoleVO.ServersRoleId != roleID ||
		!reopened.verifyPreviewSession(game.GameServerIpManageId, id, time.Now()) {
		t.Fatal("existing role ID or required role session token was lost")
	}
	emailTestReply(t, reopened, "POST", "/login", "2101", url.Values{"email": {email}, "password": {"wrong password"}})
}

func TestEmailAccountLegacyAndValidation(t *testing.T) {
	s := emailTestServer(t, t.TempDir(), true)
	oldPhone := url.Values{"mobilePhone": {"13800000001"}, "password": {"old"}}
	oldEmail := url.Values{"email": {"Old@EXAMPLE.net"}, "password": {"old"}}
	for key, user := range map[string]Record{
		"phone": {"mobilePhone": oldPhone.Get("mobilePhone"), "password": oldPhone.Get("password"), "userId": "phone"},
		"alias": {"mobilePhone": "Old@Example.NET", "password": oldEmail.Get("password"), "userId": "alias"},
	} {
		if err := s.users.Put(key, user); err != nil {
			t.Fatal(err)
		}
	}
	emailTestReply(t, s, "POST", "/login", "2000", oldPhone)
	emailTestReply(t, s, "POST", "/login", "2000", oldEmail)
	if err := s.users.Put("alias-lowercase", Record{"mobilePhone": "old@example.net", "password": "second", "userId": "alias-lowercase"}); err != nil {
		t.Fatal(err)
	}
	emailTestReply(t, s, "POST", "/login", "2000", url.Values{"email": {"Old@Example.NET"}, "password": {"old"}})
	emailTestReply(t, s, "POST", "/login", "2000", url.Values{"email": {"old@example.net"}, "password": {"second"}})
	emailTestReply(t, s, "POST", "/login", "2101", url.Values{"mobilePhone": {"13800000002"}, "password": {"unknown"}})
	emailTestReply(t, s, "POST", "/login", "5001", url.Values{"email": {oldEmail.Get("email")}, "mobilePhone": {oldPhone.Get("mobilePhone")}, "password": {"old"}})
	emailTestReply(t, s, "POST", "/user/register", "2201", url.Values{"email": {"old@example.net"}, "password": {"valid pass"}})
	emailTestReply(t, s, "GET", "/user/sendCheckCode", "4008", nil)
	emailTestReply(t, s, "POST", "/user/pwdUpdate", "4008", oldPhone)
	emailTestReply(t, s, "POST", "/user/register", "5001", oldPhone)
	for _, form := range []url.Values{
		{"email": {"wrong@format"}, "password": {"valid pass"}},
		{"email": {"name <x@example.net>"}, "password": {"valid pass"}},
		{"email": {"trial@example.net"}, "password": {"1234567"}},
		{"email": {"trial@example.net"}, "password": {strings.Repeat("密", 25)}},
		{"email": {"trial@example.net"}, "password": {"        "}},
		{"email": {"trial@example.net", "other@example.net"}, "password": {"valid pass"}},
	} {
		emailTestReply(t, s, "POST", "/user/register", "5001", form)
	}
	if _, unknown := s.findAccount("13800000002"); unknown != nil || s.findEmailAccount("trial@example.net", "") != nil {
		t.Fatal("rejected request unexpectedly modified account data")
	}
	legacy := emailTestServer(t, t.TempDir(), false)
	emailTestReply(t, legacy, "POST", "/login", "2000", oldPhone)
	if _, saved := legacy.findAccount(oldPhone.Get("mobilePhone")); roleString(saved, "password") != oldPhone.Get("password") {
		t.Fatal("disabled email feature changed legacy login auto-registration")
	}
	emailTestReply(t, legacy, "POST", "/user/register", "1001", oldEmail)
	emailTestReply(t, legacy, "GET", "/user/sendCheckCode", "2000", nil)
}
