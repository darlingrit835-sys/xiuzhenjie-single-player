package main

import (
	"encoding/binary"
	"encoding/json"
	"testing"
	"time"

	"google.golang.org/protobuf/encoding/protowire"
)

// Decode the game envelope and the three fields consumed by Unity's ForgeWire.
func auctionWireReply(t *testing.T, s *Server, p *Peer, order auctionAction, want int) []byte {
	t.Helper()
	req, err := json.Marshal(order)
	if err != nil {
		t.Fatal(err)
	}
	return auctionJSONCommandReply(t, s, p, scheduledAuctionCommand, req, want)
}

func auctionJSONCommandReply(t *testing.T, s *Server, p *Peer, cmd int, req []byte, want int) []byte {
	t.Helper()
	seq := 37
	s.handleGame(p, Message{Cmd: cmd, Msid: seq, Data: offerString(nil, 3, string(req))})
	c := p.conn.(*auctionTestConnection)
	if len(c.frames) != 1 {
		t.Fatalf("expected one response, got %d", len(c.frames))
	}
	frame := c.frames[0]
	c.frames = nil
	size, n := binary.Uvarint(frame)
	if n <= 0 || size != uint64(len(frame)-n) {
		t.Fatal("invalid frame size")
	}
	envelope, err := s.decode("Msg", frame[n:])
	if err != nil || getInt(envelope, "Cmd") != int64(cmd) || getInt(envelope, "Msid") != int64(seq) {
		t.Fatal("lost game message routing", err)
	}
	data := envelope.Get(field(envelope, "Data")).Bytes()
	var code, success uint64
	var payload []byte
	for len(data) > 0 {
		field, typ, n := protowire.ConsumeTag(data)
		if n < 0 {
			t.Fatal("invalid tag")
		}
		data = data[n:]
		switch {
		case field == 1 && typ == protowire.VarintType:
			code, n = protowire.ConsumeVarint(data)
		case field == 2 && typ == protowire.VarintType:
			success, n = protowire.ConsumeVarint(data)
		case field == 3 && typ == protowire.BytesType:
			payload, n = protowire.ConsumeBytes(data)
		default:
			t.Fatal("unexpected response field", field)
		}
		if n < 0 {
			t.Fatal("invalid field")
		}
		data = data[n:]
	}
	if code != uint64(want) || (success == 1) != (want == 2000) || !json.Valid(payload) {
		t.Fatalf("code=%d ok=%d body=%s", code, success, payload)
	}
	return payload
}

func TestAuctionGatewayRequiresCurrentSession(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	t.Cleanup(func() { _ = s.privateChat.close() })
	old, _ := auctionTestRole(t, s, "viewer", nil)
	current, _ := auctionTestRole(t, s, "viewer", nil)
	query := auctionAction{Tier: "daily", Action: "view"}
	auctionWireReply(t, s, old, query, 4008)
	var v auctionView
	if err := json.Unmarshal(auctionWireReply(t, s, current, query, 2000), &v); err != nil || v.Balance != 1000 || v.EventID == "" {
		t.Fatal("missing private projection", err)
	}
	auctionWireReply(t, s, current, auctionAction{Tier: "daily", Action: "bid"}, 4008)
	s.previewWrites = false
	auctionWireReply(t, s, current, query, 4008)
}

func TestFixedMarketCategoryFilters(t *testing.T) {
	s := &Server{}
	listing := s.fixedAuctionSummary("sample", Record{"Shop": 83, "Goods": Record{"Name": "玄铁护心镜", "Type": 26, "Rank": 7, "FiveElement": 0}}, time.Now())
	usable := previewAuctionFilter{kind: 14, subtype: 26, element: -2, rank: -2}
	if !usable.matches(listing) {
		t.Fatal("grade, neutral element, equipment and function should all match")
	}
	usable.rank = -1
	if usable.matches(listing) {
		t.Fatal("wrong grade")
	}
	usable.rank = 7
	if !usable.matches(listing) {
		t.Fatal("legacy exact rank")
	}
	usable.subtype = 15
	if usable.matches(listing) {
		t.Fatal("wrong equipment function")
	}
}

func TestAuctionConditionalRefreshIncludesPrivateChanges(t *testing.T) {
	now := time.Date(2026, 11, 30, 21, 0, 1, 0, auctionZone)
	view := auctionView{Now: now.UnixMilli(), EventID: "monthly-20261130", Tier: "monthly", Phase: "live", Balance: 1200}
	read := func(previous string) (tag string, unchanged bool, bytes int) {
		t.Helper()
		body, err := auctionWireView(view, previous)
		if err != nil {
			t.Fatal(err)
		}
		raw, err := json.Marshal(body)
		if err != nil {
			t.Fatal(err)
		}
		var out struct {
			ViewTag   string
			Unchanged bool
			Now       int64
		}
		if err = json.Unmarshal(raw, &out); err != nil || out.ViewTag == "" || view.Now != out.Now {
			t.Fatal(string(raw), err)
		}
		return out.ViewTag, out.Unchanged, len(raw)
	}
	tag, same, _ := read("")
	if same {
		t.Fatal("missing initial view")
	}
	view.Now += 3000
	next, same, bytes := read(tag)
	if next != tag || !same || bytes > 150 {
		t.Fatal("quiet room sent a full view", bytes)
	}
	t.Logf("quiet monthly room: %d bytes JSON", bytes)
	view.Balance -= 180
	if next, same, _ = read(tag); same || next == tag {
		t.Fatal("wallet change hidden")
	}
}
