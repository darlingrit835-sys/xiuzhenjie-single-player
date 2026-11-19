package main

import (
	"bufio"
	"bytes"
	"encoding/binary"
	"io"
	"net"
	"os"
	"path/filepath"
	"testing"
	"time"

	"google.golang.org/protobuf/proto"
)

func dispatchAndReadPreview(t *testing.T, s *Server, p *Peer, remote net.Conn, msg Message, response string) (bool, int64) {
	t.Helper()
	reader := bufio.NewReader(remote)
	_ = remote.SetReadDeadline(time.Now().Add(2 * time.Second))
	finished := make(chan struct{})
	go func() {
		defer close(finished)
		s.handleGame(p, msg)
	}()
	length, err := binary.ReadUvarint(reader)
	if err != nil {
		t.Fatal(err)
	}
	data := make([]byte, length)
	if _, err := io.ReadFull(reader, data); err != nil {
		t.Fatal(err)
	}
	envelope, err := s.decode("Msg", data)
	if err != nil || getInt(envelope, "Cmd") != int64(msg.Cmd) || getInt(envelope, "Msid") != int64(msg.Msid) {
		t.Fatalf("invalid preview envelope: %v", err)
	}
	<-finished
	out, err := s.decode(response, envelope.Get(field(envelope, "Data")).Bytes())
	if err != nil {
		t.Fatal(err)
	}
	return getBool(out, "IsSuccess"), getInt(out, "Code")
}

func TestPreviewRoutingAndSaveIsolation(t *testing.T) {
	p, err := loadProtocol()
	if err != nil {
		t.Fatal(err)
	}
	registry, err := loadPreview(p)
	if err != nil {
		t.Fatal(err)
	}
	dir := t.TempDir()
	path := filepath.Join(dir, "roles.json")
	before := []byte("{\"preview-account\":{\"Id\":\"preview-account\",\"ShinyStone\":42,\"Goods\":[{\"Id\":\"gear\"}]}}")
	if err := os.WriteFile(path, before, 0o600); err != nil {
		t.Fatal(err)
	}
	store, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	s := &Server{protocol: p, roles: store}
	client, server := net.Pipe()
	defer client.Close()
	defer server.Close()
	peer := &Peer{conn: server}
	s.bindGameRole(peer, "preview-account")

	check := func(msg Message, response string, success bool, code int64) {
		t.Helper()
		if got, gotCode := dispatchAndReadPreview(t, s, peer, client, msg, response); got != success || gotCode != code {
			t.Fatalf("Cmd %d: got success=%v code=%d, want %v/%d", msg.Cmd, got, gotCode, success, code)
		}
	}
	check(Message{Cmd: 1013011, Msid: 1}, "EnterSeedResp_1013011", true, 2000)
	s.preview = registry
	check(Message{Cmd: 1013011, Msid: 2}, "EnterSeedResp_1013011", true, 2000)
	check(Message{Cmd: 1003004, Msid: 3}, "ActionBoolResp_1001001", false, previewDeniedCode)
	check(Message{Cmd: 1013004, Msid: 4, Data: []byte{0x80}}, "EnterSoilResp_1013004", false, previewDeniedCode)
	check(Message{Cmd: 1004036, Msid: 5}, "BattleEndRingOutGoodsResp_1004036", false, previewDeniedCode)

	after, err := os.ReadFile(path)
	if err != nil || !bytes.Equal(before, after) {
		t.Fatalf("trial command changed save file: %v", err)
	}
}

func TestTrialLandSeedsAndSoil(t *testing.T) {
	p, err := loadProtocol()
	if err != nil {
		t.Fatal(err)
	}
	registry, err := loadPreview(p)
	if err != nil {
		t.Fatal(err)
	}
	s := &Server{protocol: p}
	plot, err := s.decode("EnterPlantResp_1013001", registry[1013001].cached)
	if err != nil {
		t.Fatal(err)
	}
	lands := plot.Get(field(plot, "Land")).List()
	if lands.Len() != 1 || getInt(lands.Get(0).Message(), "Id") != previewPlotID {
		t.Fatal("trial field is not selectable")
	}
	soilReq := s.pb("EnterSoilReq_1013004")
	set(soilReq, "LandId", previewPlotID)
	payload, err := registry[1013004].buildPayload(nil, soilReq)
	if err != nil {
		t.Fatal(err)
	}
	soil, err := s.decode("EnterSoilResp_1013004", payload)
	if err != nil {
		t.Fatal(err)
	}
	if !soil.Has(field(soil, "CurrentSoil")) || getString(soil.Get(field(soil, "CurrentSoil")).Message(), "Id") != previewSoilID {
		t.Fatal("response does not match the selected test field")
	}
	seedReq := s.pb("TemplateMaterialPlantDetailReq_1011003")
	for _, id := range []string{previewSeedID, "unrelated-seed"} {
		set(seedReq, "Id", id)
		payload, err := registry[1011003].buildPayload(nil, seedReq)
		if err != nil {
			t.Fatal(err)
		}
		seed, err := s.decode("TemplateMaterialPlantDetailResp_1011003", payload)
		if err != nil {
			t.Fatal(err)
		}
		if getBool(seed, "IsSuccess") != (id == previewSeedID) {
			t.Fatalf("unexpected seed result for %q", id)
		}
		if id == previewSeedID && (getInt(seed, "FiveElement") != 2 || getString(seed, "MapName") == "") {
			t.Fatal("trial seed detail has incomplete client fields")
		}
		if id != previewSeedID && getInt(seed, "Code") != previewDeniedCode {
			t.Fatal("unrelated seed should receive the trial refusal code")
		}
	}
}

type previewSink struct{ net.Conn }

func (previewSink) Write(data []byte) (int, error)   { return len(data), nil }
func (previewSink) SetWriteDeadline(time.Time) error { return nil }

func BenchmarkPreviewPB(b *testing.B) {
	p, err := loadProtocol()
	if err != nil {
		b.Fatal(err)
	}
	registry, err := loadPreview(p)
	if err != nil {
		b.Fatal(err)
	}
	s := &Server{protocol: p, preview: registry}
	peer := &Peer{conn: previewSink{}}
	request := p.message("EnterSoilReq_1013004")
	set(request, "LandId", previewPlotID)
	payload, err := proto.Marshal(request)
	if err != nil {
		b.Fatal(err)
	}
	for _, item := range []struct {
		name string
		msg  Message
	}{
		{"cached", Message{Cmd: 1013011, Msid: 1}},
		{"dynamic", Message{Cmd: 1013004, Msid: 1, Data: payload}},
	} {
		b.Run(item.name, func(b *testing.B) {
			b.ReportAllocs()
			for b.Loop() {
				s.handlePreview(peer, item.msg)
			}
		})
	}
}
