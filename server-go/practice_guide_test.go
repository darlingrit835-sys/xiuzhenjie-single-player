package main

import (
	"slices"
	"testing"
	"time"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestPracticeGuideRecoversReadBookOnce(t *testing.T) {
	for _, tc := range []struct {
		name          string
		hasBook, read bool
		stage, want   int64
	}{
		{"new role", true, false, 1, 1},
		{"older bag missing book", false, false, 1, 2},
		{"book read with a remaining copy", true, true, 1, 2},
		{"already meditating", false, true, 2, 2},
		{"already finished", false, false, 100, 100},
	} {
		t.Run(tc.name, func(t *testing.T) {
			r := Record{"PracticeGuideVal": tc.stage, "StarterKitVersion": 2, "PracticeStarterRead": tc.read}
			if tc.hasBook {
				// Legacy bags can identify the book by name alone.
				r["Goods"] = []any{Record{"Type": 1, "Name": "启灵经"}}
			}
			resumePracticeGuide(r)
			resumePracticeGuide(r)
			if roleInt(r, "PracticeGuideVal") != tc.want {
				t.Fatal("incorrect login checkpoint", r)
			}
			if tc.stage != tc.want {
				if roleInt(r, "ShinyStone") != 100 || !slices.Equal(stringRecords(r["GuideRewardClaimed"]), []string{"1_2"}) {
					t.Fatal("incorrect reward on repeat login", r)
				}
			} else if roleInt(r, "ShinyStone") != 0 {
				t.Fatal("unearned login reward", r)
			}
		})
	}
}

func TestPracticeGuideBookAndChapterRequestsThroughRelog(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	peer, client := auctionTestRole(t, s, "pupil", []any{Record{
		"Type": 1, "Name": "启灵经", "TemplateId": practiceStarterMethodID, "GoodsId": "starter", "Count": 2,
	}})
	if err := s.roles.Update("pupil", func(r Record) error {
		r["PracticeGuideVal"], r["StarterKitVersion"] = 1, 2
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	call := func(cmd, id int, request, response string, values Record) protoreflect.Message {
		t.Helper()
		req := s.pb(request)
		putPB(req, values)
		body, err := proto.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
		client.frames = nil // The tutorial also emits unsolicited reward notifications.
		s.handleGame(peer, Message{Cmd: cmd, Msid: id, Data: body})
		return client.receive(t, s, cmd, id, response)
	}
	if !getBool(call(1004024, 1, "GoodsUseReq_1004024", "GoodsUseResp_1004024", Record{"GoodsId": "starter"}), "IsSuccess") {
		t.Fatal("book use failed")
	}
	peer, client = auctionTestRole(t, s, "pupil", nil)
	login := call(1002002, 1, "RoleLoginReq_1002002", "RoleLoginResp_1002002", Record{
		"RoleId": "pupil", "GameServerIpManageId": s.issuePreviewSession("account-pupil", time.Now()),
	})
	if getInt(login, "PracticeGuideVal") != 2 || roleInt(locateGoods(s.roles.Get("pupil"), "starter"), "Count") != 1 {
		t.Fatal("did not resume after reading one copy")
	}
	chapter := func(seq int) bool {
		t.Helper()
		return getBool(call(1002013, seq, "GuideInfoUpdateReq_1002013", "GuideInfoUpdateResp_1002013", Record{"Type": 1}), "IsSuccess")
	}
	if chapter(2) {
		t.Fatal("advanced before starter meditation")
	}
	if !getBool(call(1001005, 3, "UnderstandMethodReq_1001005", "ActionBoolResp_1001001", Record{"Type": 2, "Id": practiceStarterMethodID}), "IsSuccess") {
		t.Fatal("starter meditation failed")
	}
	if !chapter(4) || !chapter(4) || roleInt(s.roles.Get("pupil"), "PracticeGuideVal") != 3 {
		t.Fatal("repeated chapter request advanced the stage")
	}
	if !chapter(5) || !chapter(6) {
		t.Fatal("could not finish")
	}
	call(1002014, 7, "GuideInfoSkipReq_1002014", "GuideInfoSkipResp_1002014", Record{"Type": 1})
	r := s.roles.Get("pupil")
	if roleInt(r, "PracticeGuideVal") != 100 || roleInt(r, "ShinyStone") != 1500 || !slices.Equal(stringRecords(r["GuideRewardClaimed"]), []string{"1_2", "1_3", "1_100"}) {
		t.Fatal("unexpected final progress or rewards", r)
	}
}

func TestPracticeGuideSwitchToNaQiUsesRequestedLocation(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	peer, client := auctionTestRole(t, s, "pupil", nil)
	for i, tc := range []struct {
		level, oldMask, location, wantMask int64
		success                            bool
	}{
		{1, 1, 3, 4, true}, {1, 4, 3, 4, true},
		{1, 2, 4, 2, false}, {31, 2, 4, 6, true}, {31, 6, 2, 2, true},
		{31, 1, 6, 5, true}, {31, 2, 5, 3, true}, {31, 2, 7, 2, false},
	} {
		if err := s.roles.Update("pupil", func(r Record) error {
			r["Level"], r["RunMask"], r["RunLocation"] = tc.level, tc.oldMask, gameLocation(tc.oldMask)
			return nil
		}); err != nil {
			t.Fatal(err)
		}
		req := s.pb("ChangeRunLocationReq_1001007")
		set(req, "NewLocation", tc.location)
		body, err := proto.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
		msg := Message{Cmd: 1001007, Msid: i + 1, Data: body}
		s.handleGame(peer, msg)
		reply := client.receive(t, s, msg.Cmd, msg.Msid, "ActionBoolResp_1001001")
		role := s.roles.Get("pupil")
		if getBool(reply, "IsSuccess") != tc.success || roleInt(role, "RunMask") != tc.wantMask || roleInt(role, "RunLocation") != gameLocation(tc.wantMask) {
			t.Fatalf("level %d, mask %d, request %d: %v, saved %v", tc.level, tc.oldMask, tc.location, reply, role)
		}
	}
}
