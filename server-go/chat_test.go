package main

import (
	"bytes"
	"encoding/binary"
	"os"
	"path/filepath"
	"slices"
	"strings"
	"testing"
	"time"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/proto"
)

type privatePushCapture struct {
	*auctionTestConnection
	pushed chan struct{}
}

func (capture *privatePushCapture) Write(frame []byte) (int, error) {
	n, err := capture.auctionTestConnection.Write(frame)
	capture.pushed <- struct{}{}
	return n, err
}

func TestFriendChatWireAndRestart(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	a, aConn := auctionTestRole(t, s, "a", nil)
	b, bConn := auctionTestRole(t, s, "b", nil)
	c, cConn := auctionTestRole(t, s, "c", nil)
	capture := &privatePushCapture{auctionTestConnection: bConn, pushed: make(chan struct{}, 4)}
	b.conn = capture
	t.Cleanup(func() { s.privateChat.close() })
	request := func(p *Peer, cmd int, target, text string, before int64) {
		req := s.pb("SendMessageRoleReq_4001007")
		putPB(req, Record{"RoleId": target, "Msg": text})
		payload, err := proto.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
		if before > 0 {
			payload = protowire.AppendVarint(protowire.AppendTag(payload, 3, protowire.VarintType), uint64(before))
		}
		s.handleGame(p, Message{Cmd: cmd, Msid: 5, Data: payload})
	}
	history := func(conn *auctionTestConnection, wantCode int64) (times []int64, msgs, senders []string, more bool) {
		t.Helper()
		if len(conn.frames) != 1 {
			t.Fatalf("got %d frames", len(conn.frames))
		}
		frame := conn.frames[0]
		conn.frames = nil
		_, n := binary.Uvarint(frame)
		env, err := s.decode("Msg", frame[n:])
		if err != nil || getInt(env, "Cmd") != privateChatHistory || getInt(env, "Msid") != 5 {
			t.Fatalf("invalid history envelope %v, %v", env, err)
		}
		raw := env.Get(field(env, "Data")).Bytes()
		code, success := int64(0), false
		for len(raw) > 0 {
			num, typ, n := protowire.ConsumeTag(raw)
			if n < 0 {
				t.Fatal("bad tag")
			}
			raw = raw[n:]
			switch typ {
			case protowire.VarintType:
				v, n := protowire.ConsumeVarint(raw)
				if n < 0 {
					t.Fatal("bad varint")
				}
				raw = raw[n:]
				switch num {
				case 1:
					code = int64(v)
				case 2:
					success = v == 1
				case 4:
					more = v == 1
				default:
					t.Fatalf("bad field %d", num)
				}
			case protowire.BytesType:
				item, n := protowire.ConsumeBytes(raw)
				if n < 0 || num != 3 {
					t.Fatal("bad item")
				}
				row, err := s.decode("ReceiveMessageRoleResp_4001008", item)
				if err != nil {
					t.Fatal(err)
				}
				times = append(times, getInt(row, "MsgTime"))
				msgs = append(msgs, getString(row, "Msg"))
				senders = append(senders, getString(row, "RoleId"))
				raw = raw[n:]
			default:
				t.Fatalf("bad wire type %d", typ)
			}
		}
		if code != wantCode || success != (wantCode == 2000) {
			t.Fatalf("history status %d/%v", code, success)
		}
		return
	}

	request(a, privateChatSend, "b", "unauthorized", 0)
	if got := aConn.receive(t, s, privateChatSend, 5, "SendMessageRoleResp_4001007"); getBool(got, "IsSuccess") || getInt(got, "Code") != 4407 {
		t.Fatal(got)
	}
	if _, err := os.Stat(filepath.Join(dir, "chat")); !os.IsNotExist(err) {
		t.Fatal("rejected requests created a message archive")
	}
	if err := s.roles.Transact(func(tx *StoreTx) error {
		putFriendEntry(tx.Get("a"), friendLinks, "b", 0)
		putFriendEntry(tx.Get("b"), friendLinks, "a", 0)
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	request(a, privateChatSend, "b", strings.Repeat("x", maxPrivateChatBytes+1), 0)
	if got := aConn.receive(t, s, privateChatSend, 5, "SendMessageRoleResp_4001007"); getBool(got, "IsSuccess") {
		t.Fatal(got)
	}
	request(a, privateChatSend, "b", "论道", 0)
	ack := aConn.receive(t, s, privateChatSend, 5, "SendMessageRoleResp_4001007")
	select {
	case <-capture.pushed:
	case <-time.After(time.Second):
		t.Fatal("online push timed out")
	}
	pushed := bConn.receive(t, s, privateChatPush, 0, "ReceiveMessageRoleResp_4001008")
	firstAt := getInt(ack, "MsgTime")
	if !getBool(ack, "IsSuccess") || getString(pushed, "RoleId") != "a" || getString(pushed, "Msg") != "论道" || getInt(pushed, "MsgTime") != firstAt || firstAt <= 0 {
		t.Fatal("incorrect delivery or timestamp", ack, pushed)
	}
	// Protobuf fields can legitimately arrive in either order on a retry.
	bodyField := protowire.AppendString(protowire.AppendTag(nil, 1, protowire.BytesType), "论道")
	targetField := protowire.AppendString(protowire.AppendTag(nil, 2, protowire.BytesType), "b")
	for _, retry := range [][]byte{bytes.Join([][]byte{bodyField, targetField}, nil), bytes.Join([][]byte{targetField, bodyField}, nil)} {
		s.handleGame(a, Message{Cmd: privateChatSend, Msid: 5, Data: retry})
		dup := aConn.receive(t, s, privateChatSend, 5, "SendMessageRoleResp_4001007")
		if !getBool(dup, "IsSuccess") || getInt(dup, "MsgTime") != firstAt {
			t.Fatal("equivalent retried request not acknowledged")
		}
	}
	s.online.Delete("a")
	request(b, privateChatSend, "a", "稍后见", 0)
	reply := bConn.receive(t, s, privateChatSend, 5, "SendMessageRoleResp_4001007")
	lastAt := getInt(reply, "MsgTime")
	if len(aConn.frames) != 0 || lastAt <= firstAt {
		t.Fatal("offline push or time regression")
	}
	request(c, privateChatHistory, "b", "", 0)
	history(cConn, 4407)
	if err := s.privateChat.close(); err != nil {
		t.Fatal(err)
	}
	s = auctionTestServer(t, dir)
	a, aConn = auctionTestRole(t, s, "a", nil)
	request(a, privateChatHistory, "b", "", 0)
	times, msgs, senders, more := history(aConn, 2000)
	if !slices.Equal(times, []int64{firstAt, lastAt}) || !slices.Equal(msgs, []string{"论道", "稍后见"}) || !slices.Equal(senders, []string{"a", "b"}) || more {
		t.Fatalf("history after reopen: %v %v %v %v", times, msgs, senders, more)
	}
	request(a, privateChatHistory, "b", "", lastAt)
	times, _, _, _ = history(aConn, 2000)
	if !slices.Equal(times, []int64{firstAt}) {
		t.Fatal("exclusive cursor", times)
	}
	if err := s.roles.Update("b", func(r Record) error { delete(friendMap(r, friendLinks), "a"); return nil }); err != nil {
		t.Fatal(err)
	}
	request(a, privateChatHistory, "b", "", 0)
	history(aConn, 4407)
}

func TestPrivateChatPagesSurviveClockChanges(t *testing.T) {
	var store privateChatStore
	db, err := store.open(t.TempDir())
	if err != nil {
		t.Fatal(err)
	}
	defer store.close()
	conv := privateChatKey("x", "y")
	if conv != privateChatKey("y", "x") || conv == privateChatKey("xy", "") {
		t.Fatal("invalid conversation key")
	}
	makeValue := func(at int64) ([]byte, error) { return binary.BigEndian.AppendUint64(nil, uint64(at)), nil }
	for i := 0; i < privateChatPageSize+2; i++ {
		at, _, err := appendPrivateChat(db, conv, int64(10000-i), makeValue)
		if err != nil || at != int64(10000+i) {
			t.Fatalf("clock rollback %d: %v", at, err)
		}
	}
	page, more, err := readPrivateChat(db, conv, 0)
	if err != nil || !more || len(page) != privateChatPageSize {
		t.Fatalf("bad first page: %d %v %v", len(page), more, err)
	}
	before := int64(binary.BigEndian.Uint64(page[len(page)-1]))
	older, more, err := readPrivateChat(db, conv, before)
	if err != nil || more || len(older) != 2 || bytes.Equal(older[0], page[len(page)-1]) || binary.BigEndian.Uint64(older[1]) != 10000 {
		t.Fatalf("bad second page %v %v", more, err)
	}
}
