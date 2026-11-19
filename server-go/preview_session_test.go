package main

import (
	"encoding/json"
	"net"
	"net/http"
	"net/http/httptest"
	"net/url"
	"strings"
	"testing"
	"time"

	"google.golang.org/protobuf/proto"
)

func TestPreviewSessionBindsAccountAndExpires(t *testing.T) {
	now := time.Unix(1_700_000_000, 0)
	s := &Server{dataDir: t.TempDir()}
	if err := s.enablePreviewWrites(); err != nil {
		t.Fatal(err)
	}
	token := s.issuePreviewSession("account", now)
	if !s.verifyPreviewSession(token, "account", now.Add(time.Minute)) {
		t.Fatal("valid reconnect rejected")
	}
	for _, input := range []struct {
		token string
		owner string
		now   time.Time
	}{
		{"1", "account", now},
		{token, "other", now},
		{token, "account", now.Add(previewSessionDuration)},
	} {
		if s.verifyPreviewSession(input.token, input.owner, input.now) {
			t.Fatal("unexpected valid session")
		}
	}
}

func TestWritableTrialRequiresTicketForRoleOwner(t *testing.T) {
	s, err := openServer(Config{}, t.TempDir())
	if err != nil {
		t.Fatal(err)
	}
	if err := s.enablePreviewWrites(); err != nil {
		t.Fatal(err)
	}
	if err := s.users.Put("owner", Record{"userId": "owner", "mobilePhone": "test", "password": "fixture"}); err != nil {
		t.Fatal(err)
	}
	for id, owner := range map[string]string{"own": "owner", "other": "another-owner"} {
		if err := s.roles.Put(id, Record{"Id": id, "UserId": owner, "Level": 31}); err != nil {
			t.Fatal(err)
		}
	}
	body := url.Values{"mobilePhone": {"test"}, "password": {"fixture"}}
	login := httptest.NewRecorder()
	s.accountHandler().ServeHTTP(login, httptest.NewRequest(http.MethodPost, "/login", strings.NewReader(body.Encode())))
	var response struct {
		MsgBody struct {
			BizInfo struct {
				Code, DataString string
			}
		}
	}
	if err := json.Unmarshal(login.Body.Bytes(), &response); err != nil || response.MsgBody.BizInfo.Code != "2000" {
		t.Fatalf("account login failed: %v", err)
	}
	var account struct {
		GameServersInfoList []struct{ GameServerIpManageId string }
	}
	if err := json.Unmarshal([]byte(response.MsgBody.BizInfo.DataString), &account); err != nil || len(account.GameServersInfoList) != 1 {
		t.Fatalf("missing account server information: %v", err)
	}
	token := account.GameServersInfoList[0].GameServerIpManageId
	client, server := net.Pipe()
	defer client.Close()
	defer server.Close()
	peer := &Peer{conn: server}
	for i, attempt := range []struct {
		role, ticket string
		want         bool
	}{
		{"own", "1", false},
		{"other", token, false},
		{"own", token, true},
	} {
		req := s.pb("RoleLoginReq_1002002")
		putPB(req, Record{"RoleId": attempt.role, "GameServerIpManageId": attempt.ticket})
		encoded, err := proto.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
		ok, code := dispatchAndReadPreview(t, s, peer, client, Message{Cmd: 1002002, Msid: i + 1, Data: encoded}, "RoleLoginResp_1002002")
		if ok != attempt.want {
			t.Fatalf("role login %d, got %v/%d", i, ok, code)
		}
	}
	if id := peer.currentRoleID(); id != "own" {
		t.Fatalf("role binding = %q", id)
	}
}
