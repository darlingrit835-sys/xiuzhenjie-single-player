package main

import (
	"testing"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestPreviewSocialResponses(t *testing.T) {
	protocol, err := loadProtocol()
	if err != nil {
		t.Fatal(err)
	}
	routes, err := loadPreview(protocol)
	if err != nil {
		t.Fatal(err)
	}
	response := func(cmd int, values Record) protoreflect.Message {
		t.Helper()
		route := routes[cmd]
		var request protoreflect.Message
		if route.request != "" {
			request = protocol.message(route.request)
			putPB(request, values)
		}
		out := protocol.message(route.response)
		statusPB(out, true, 2000)
		route.build(nil, request, out)
		return out
	}

	t.Run("mail list and detail", func(t *testing.T) {
		list := response(1003001, nil)
		letters := list.Get(field(list, "Letters")).List()
		if letters.Len() != 1 {
			t.Fatalf("expected one demo letter, got %d", letters.Len())
		}
		letter := letters.Get(0).Message()
		id := getString(letter, "Id")
		if id == "" || getString(letter, "Title") == "" || getString(letter, "Content") == "" ||
			!getBool(letter, "Status") || !getBool(letter, "IsGetAll") || getBool(letter, "HasAward") ||
			letter.Get(field(letter, "Awards")).List().Len() != 0 {
			t.Fatalf("incomplete or claimable preview letter %v", letter)
		}
		if open := response(1003005, Record{"Id": id}); !getBool(open, "IsSuccess") {
			t.Fatal("listing returned a letter which cannot be opened")
		}
		if missing := response(1003005, Record{"Id": "unknown"}); getBool(missing, "IsSuccess") {
			t.Fatal("opening an unknown letter unexpectedly succeeded")
		}
	})

	t.Run("guild list and detail", func(t *testing.T) {
		list := response(1015001, Record{"MapId": previewGuildMap})
		guilds := list.Get(field(list, "GuildList")).List()
		if guilds.Len() != 1 || list.Has(field(list, "SelfGuild")) {
			t.Fatal("expected one demo guild without assigning membership")
		}
		item := guilds.Get(0).Message()
		detail := response(1015003, Record{"GuildId": getInt(item, "GuildId")})
		if !getBool(detail, "IsSuccess") || getString(detail, "GuildName") != getString(item, "Name") ||
			getInt(detail, "GuildLevel") <= 0 {
			t.Fatal("guild listing and details disagree")
		}
		if unrelated := response(1015001, Record{"MapId": "1112W1"}); unrelated.Get(field(unrelated, "GuildList")).List().Len() != 0 {
			t.Fatal("demo guild appeared outside its configured map")
		}
		if missing := response(1015003, Record{"GuildId": 1}); getBool(missing, "IsSuccess") {
			t.Fatal("unknown guild detail unexpectedly succeeded")
		}
	})

	t.Run("mentor reward state", func(t *testing.T) {
		summary := response(1014012, nil)
		if !getBool(summary, "IsGetTmcg") || summary.Has(field(summary, "ShiFu")) {
			t.Fatal("empty mentorship state would expose a fictitious reward or mentor")
		}
	})

	for cmd, responseType := range map[int]string{
		1003004: "ActionBoolResp_1001001", 1003006: "ReceiveLetterResp_1003006", 1015004: "GuildJoinResp_1015004",
	} {
		if route := routes[cmd]; !route.blocked || route.build != nil || route.response != responseType {
			t.Fatalf("cmd %d must use an explicit typed refusal", cmd)
		}
	}
}
