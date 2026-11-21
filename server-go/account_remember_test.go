package main

import (
	"bytes"
	"encoding/base64"
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"net/url"
	"os"
	"path/filepath"
	"strings"
	"testing"
	"time"

	"golang.org/x/crypto/bcrypt"
)

type rememberTestLogin struct {
	UserID, MobilePhone, RememberToken string
	RememberTokenExpiresAt             int64
	GameServersInfoList                []struct {
		ServerIp             string
		GameServerIpManageId string
		CurrentRoleVO        struct{ ServersRoleId string }
	}
}

func rememberTestRequest(t *testing.T, s *Server, path, code string, form url.Values) rememberTestLogin {
	t.Helper()
	request := httptest.NewRequest(http.MethodPost, path, strings.NewReader(form.Encode()))
	request.Header.Set("Content-Type", "application/x-www-form-urlencoded")
	response := httptest.NewRecorder()
	s.accountHandler().ServeHTTP(response, request)
	var envelope struct {
		MsgBody struct{ BizInfo accountTestReply }
	}
	if err := json.Unmarshal(response.Body.Bytes(), &envelope); err != nil {
		t.Fatalf("%s did not return an account reply: %v", path, err)
	}
	reply := envelope.MsgBody.BizInfo
	if response.Code != http.StatusOK || reply.Code != code {
		t.Fatalf("%s: want success envelope and code %s, got HTTP %d and code %s", path, code, response.Code, reply.Code)
	}
	var login rememberTestLogin
	if code == "2000" && path != accountLogoutPath {
		if err := json.Unmarshal([]byte(reply.DataString), &login); err != nil {
			t.Fatal(err)
		}
	}
	if (path != "/login" || login.RememberToken != "") && response.Header().Get("Cache-Control") != "no-store" {
		t.Fatalf("remember response was eligible for caching: %s", path)
	}
	return login
}

func (login rememberTestLogin) resumeForm() url.Values {
	return url.Values{"userId": {login.UserID}, "rememberToken": {login.RememberToken}}
}

func TestFreshWorldWithExistingAccounts(t *testing.T) {
	oldDir, newDir := t.TempDir(), t.TempDir()
	if _, err := openServer(Config{AccountsDir: oldDir}, newDir); err == nil {
		t.Fatal("missing account data was accepted")
	}
	old := emailTestServer(t, oldDir, false)
	login := rememberTestRequest(t, old, "/login", "2000", url.Values{
		"mobilePhone": {"13600000091"}, "password": {"fixture password"}, "rememberLogin": {"1"},
	})
	if err := old.roles.Put("old-role", Record{"UserId": login.UserID, "Id": "old-role", "Level": 40}); err != nil {
		t.Fatal(err)
	}
	fresh, err := openServer(Config{AccountsDir: oldDir}, newDir)
	if err != nil {
		t.Fatal(err)
	}
	resumed := rememberTestRequest(t, fresh, accountResumePath, "2000", login.resumeForm())
	if resumed.UserID != login.UserID || resumed.GameServersInfoList[0].CurrentRoleVO.ServersRoleId != "" {
		t.Fatal("old account must resume into an empty world")
	}
	rememberTestRequest(t, fresh, accountLogoutPath, "2000", login.resumeForm())
	restored := emailTestServer(t, oldDir, false)
	rememberTestRequest(t, restored, accountResumePath, "2105", login.resumeForm())
	if restored.roles.Get("old-role") == nil {
		t.Fatal("old world role disappeared")
	}
}

func TestRememberLoginSurvivesRestartAndReturnsCurrentRealm(t *testing.T) {
	dir := t.TempDir()
	s := emailTestServer(t, dir, false)
	loginForm := url.Values{"mobilePhone": {"13600000001"}, "password": {"test pass+"}}
	first := rememberTestRequest(t, s, "/login", "2000", loginForm)
	if first.UserID == "" || first.RememberToken != "" {
		t.Fatal("legacy login unexpectedly issued persistent credentials")
	}
	loginForm.Set("rememberLogin", "1")
	login := rememberTestRequest(t, s, "/login", "2000", loginForm)
	if login.RememberToken == "" || login.RememberTokenExpiresAt < time.Now().Add(rememberLoginDuration-time.Minute).Unix() {
		t.Fatal("missing remember credential or incorrect lifetime")
	}
	contents, err := os.ReadFile(filepath.Join(dir, "users.json"))
	if err != nil || bytes.Contains(contents, []byte(login.RememberToken)) || !bytes.Contains(contents, []byte(accountRememberSessionsKey)) {
		t.Fatalf("persisted token must be stored only as a digest: %v", err)
	}
	const roleID = "remember-role"
	if err := s.roles.Put(roleID, Record{"Id": roleID, "UserId": login.UserID, "Name": "归来", "Level": 2}); err != nil {
		t.Fatal(err)
	}
	reopened := emailTestServer(t, dir, false)
	reopened.config.PublicIP = "192.0.2.10"
	resumed := rememberTestRequest(t, reopened, accountResumePath, "2000", login.resumeForm())
	if resumed.UserID != login.UserID || resumed.MobilePhone != loginForm.Get("mobilePhone") ||
		resumed.RememberToken != login.RememberToken || resumed.RememberTokenExpiresAt != login.RememberTokenExpiresAt || len(resumed.GameServersInfoList) != 1 ||
		resumed.GameServersInfoList[0].CurrentRoleVO.ServersRoleId != roleID || resumed.GameServersInfoList[0].ServerIp != "192.0.2.10" {
		t.Fatal("remembered login did not return the persisted role and current realm data")
	}
	if s.users.Get("u_"+loginForm.Get("mobilePhone")) == nil {
		t.Fatal("original account record disappeared")
	}
}

func TestRememberLoginRevocationExpiryAndPasswordChange(t *testing.T) {
	s := emailTestServer(t, t.TempDir(), false)
	loginForm := url.Values{"mobilePhone": {"13600000002"}, "password": {"test password"}, "rememberLogin": {"1"}}
	first := rememberTestRequest(t, s, "/login", "2000", loginForm)
	second := rememberTestRequest(t, s, "/login", "2000", loginForm)
	if first.RememberToken == second.RememberToken || first.RememberToken == "" || second.RememberToken == "" {
		t.Fatal("one device's credential must be independent of another's")
	}
	other := rememberTestRequest(t, s, "/login", "2000", url.Values{"mobilePhone": {"13600000003"}, "password": {"other"}, "rememberLogin": {"1"}})
	forged := second.resumeForm()
	forged.Set("userId", other.UserID)
	rememberTestRequest(t, s, accountResumePath, "2105", forged)
	forged.Set("userId", first.UserID)
	forged.Set("rememberToken", base64.RawURLEncoding.EncodeToString(make([]byte, rememberTokenBytes)))
	rememberTestRequest(t, s, accountResumePath, "2105", forged)
	rememberTestRequest(t, s, accountLogoutPath, "2000", first.resumeForm())
	rememberTestRequest(t, s, accountLogoutPath, "2000", first.resumeForm())
	rememberTestRequest(t, s, accountResumePath, "2105", first.resumeForm())
	rememberTestRequest(t, s, accountResumePath, "2000", second.resumeForm())
	key, user := s.findAccount(loginForm.Get("mobilePhone"))
	expired, err := s.issueRememberCredential(user, time.Now().Add(-rememberLoginDuration-time.Second))
	if err != nil {
		t.Fatal(err)
	}
	rememberTestRequest(t, s, accountResumePath, "2105", url.Values{"userId": {first.UserID}, "rememberToken": {expired.token}})
	if err := s.users.Update(key, func(record Record) error {
		record["password"] = "changed by account maintenance"
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	rememberTestRequest(t, s, accountResumePath, "2105", second.resumeForm())
	rememberTestRequest(t, s, accountResumePath, "2000", other.resumeForm())
	passwordless := rememberTestRequest(t, s, "/login", "2000", url.Values{"mobilePhone": {"13600000004"}, "rememberLogin": {"1"}})
	if passwordless.RememberToken != "" {
		t.Fatal("passwordless legacy login issued a remember token")
	}
}

func TestRememberEmailAndLegacyPhoneReceiveFreshGameSessions(t *testing.T) {
	dir := t.TempDir()
	s := emailTestServer(t, dir, true)
	const email = "player@example.net"
	const emailPassword = "test Email pass+"
	hash, err := bcrypt.GenerateFromPassword([]byte(emailPassword), bcrypt.MinCost)
	if err != nil {
		t.Fatal(err)
	}
	if err := s.users.Put("e_"+email, Record{"email": email, "userId": "email-id", "passwordHash": string(hash), "emailVerified": true}); err != nil {
		t.Fatal(err)
	}
	if err := s.users.Put("legacy-id", Record{"mobilePhone": "13600000005", "userId": "legacy-id", "password": "legacy"}); err != nil {
		t.Fatal(err)
	}
	emailLogin := rememberTestRequest(t, s, "/login", "2000", url.Values{"email": {" Player@EXAMPLE.net "}, "password": {emailPassword}, "rememberLogin": {"1"}})
	phoneLogin := rememberTestRequest(t, s, "/login", "2000", url.Values{"mobilePhone": {"13600000005"}, "password": {"legacy"}, "rememberLogin": {"1"}})
	reopened := emailTestServer(t, dir, true)
	for _, login := range []rememberTestLogin{emailLogin, phoneLogin} {
		resumed := rememberTestRequest(t, reopened, accountResumePath, "2000", login.resumeForm())
		if len(resumed.GameServersInfoList) != 1 || len(login.GameServersInfoList) != 1 || resumed.MobilePhone != login.MobilePhone ||
			resumed.GameServersInfoList[0].GameServerIpManageId == login.GameServersInfoList[0].GameServerIpManageId ||
			!reopened.verifyPreviewSession(resumed.GameServersInfoList[0].GameServerIpManageId, resumed.UserID, time.Now()) {
			t.Fatal("remembered email or phone lacked current PB game authorization")
		}
	}
	disabled := emailTestServer(t, dir, false)
	rememberTestRequest(t, disabled, accountResumePath, "2105", emailLogin.resumeForm())
}

func TestRememberLoginBoundsActiveDevicesAndMatchesExactPaths(t *testing.T) {
	s := emailTestServer(t, t.TempDir(), false)
	account, err := s.createAccount("13600000006", "test-password")
	if err != nil {
		t.Fatal(err)
	}
	var credentials []rememberLoginCredential
	start := time.Now().Add(-time.Minute)
	for i := range rememberMaxDevices + 1 {
		credential, err := s.issueRememberCredential(account, start.Add(time.Duration(i)*time.Second))
		if err != nil {
			t.Fatal(err)
		}
		credentials = append(credentials, credential)
	}
	_, saved := s.findAccount(roleString(account, "mobilePhone"))
	if sessions, _ := saved[accountRememberSessionsKey].(Record); len(sessions) != rememberMaxDevices {
		t.Fatal("remember token storage exceeded its per-account limit")
	}
	form := url.Values{"userId": {roleString(account, "userId")}, "rememberToken": {credentials[0].token}}
	rememberTestRequest(t, s, accountResumePath, "2105", form)
	form.Set("rememberToken", credentials[1].token)
	rememberTestRequest(t, s, accountResumePath, "2000", form)
	request := httptest.NewRequest(http.MethodPost, accountResumePath+"-unexpected", strings.NewReader(form.Encode()))
	response := httptest.NewRecorder()
	s.accountHandler().ServeHTTP(response, request)
	if response.Code != http.StatusNotFound {
		t.Fatal("remember path was matched by prefix")
	}
}
