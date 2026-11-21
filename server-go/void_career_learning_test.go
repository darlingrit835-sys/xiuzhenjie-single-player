package main

import (
	"strconv"
	"testing"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestResidenceCareerLearnReplaceAndRestore(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	r := Record{"Level": 31, "DongFuRank": 0, "ShinyStone": 11000, "ShinyJade": 20, "Live3Type": 7, "Live3Level": 0, "Live4Type": 4, "Live4Level": 5}
	if err := s.roles.Put("owner", r); err != nil {
		t.Fatal(err)
	}
	p, c := auctionTestRole(t, s, "owner", nil)
	apply := func(cmd, room, career, seen, expect int) protoreflect.Message {
		t.Helper()
		route := gameCultivationProtocols[cmd]
		req := s.pb(route.req)
		field := "LiveType"
		if cmd == 1005004 {
			field = "NewLiveType"
		}
		putPB(req, Record{"Location": room, field: career})
		if seen >= 0 {
			req.SetUnknown(offerNumber(nil, 3, uint64(seen)))
		}
		wire, err := proto.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
		s.handleGame(p, Message{Cmd: cmd, Msid: 77, Data: wire})
		out := c.receive(t, s, cmd, 77, route.resp)
		if getInt(out, "Code") != int64(expect) || getBool(out, "IsSuccess") != (expect == 4114 || expect == 4119) {
			t.Fatalf("command %d room %d profession %d: want %d, received %v", cmd, room, career, expect, out)
		}
		return out
	}
	apply(1005003, 1, 7, 0, 4331)
	if err := s.roles.Update("owner", func(r Record) error { r["DongFuRank"] = 1; return nil }); err != nil {
		t.Fatal(err)
	}
	apply(1005003, 2, 9, 0, 4331)
	first := apply(1005003, 1, 7, 0, 4114)
	if getInt(first, "ShinyStone") != 10000 || voidRank(s.roles.Get("owner"), 7) != 1 {
		t.Fatal(first)
	}
	apply(1005003, 1, 7, 0, 4008)
	if err := s.roles.Update("owner", func(r Record) error {
		r["DongFuRank"] = 2
		r["ArtisanResearch"] = Record{"7": Record{"Rank": 4}}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	apply(1005003, 2, 4, 70, 4118)
	second := apply(1005003, 2, 9, -1, 4114)
	if getInt(second, "ShinyStone") != 0 || voidRank(s.roles.Get("owner"), 9) != 1 {
		t.Fatal(second)
	}
	apply(1005004, 1, 5, 70, 4008)
	replaced := apply(1005004, 1, 5, 79, 4119)
	if getInt(replaced, "ShinyJade") != 0 {
		t.Fatal(replaced)
	}
	apply(1005004, 1, 6, 59, 4117)
	after := auctionTestServer(t, dir).roles.Get("owner")
	if !roleBool(farmMap(after, "ResidenceRetiredCareers"), "7") || voidRank(after, 7) != 0 || voidRank(after, 9) != 1 || voidRank(after, 4) != 5 || roleInt(after, "Live1Type") != 5 || roleInt(after, "Live1Level") != 1 ||
		roleInt(after, "Live1Value") != 0 || roleInt(after, "ShinyStone") != 0 || roleInt(after, "ShinyJade") != 0 {
		t.Fatal(after)
	}

}

func TestResidenceCareerRetireAndRelearnWorkshops(t *testing.T) {
	for _, subject := range []struct {
		kind   int
		prefix string
	}{{1, "Alchemy"}, {2, "Forge"}, {3, "Talisman"}, {8, "Garden"}} {
		t.Run(subject.prefix, func(t *testing.T) {
			dir := t.TempDir()
			s := auctionTestServer(t, dir)
			id := strconv.Itoa(subject.kind)
			r := Record{"Level": 31, "DongFuRank": 2, "ShinyJade": 60, "Live1Type": subject.kind, "Live1Level": 5, "Live1Value": 63,
				"Live3Type": subject.kind, "Live3Level": 0, "Live4Type": 6, "Live4Level": 3, "Live4Value": 18,
				subject.prefix + "Rank": 5, subject.prefix + "Experience": 63,
				"ArtisanResearch": Record{id: Record{"Rank": 4}, "6": Record{"Rank": 3}}}
			if err := s.roles.Put("owner", r); err != nil {
				t.Fatal(err)
			}
			p, c := auctionTestRole(t, s, "owner", nil)
			rebuild := func(kind, expected int) {
				t.Helper()
				req := s.pb("RebuildLiveTypeReq_1005004")
				putPB(req, Record{"Location": 1, "NewLiveType": kind})
				req.SetUnknown(offerNumber(nil, 3, uint64(expected*10)))
				wire, err := proto.Marshal(req)
				if err != nil {
					t.Fatal(err)
				}
				s.handleGame(p, Message{Cmd: 1005004, Msid: 1, Data: wire})
				out := c.receive(t, s, 1005004, 1, "RebuildLiveTypeResp_1005004")
				if !getBool(out, "IsSuccess") || getInt(out, "Code") != 4119 {
					t.Fatal(out)
				}
			}
			rebuild(7, subject.kind)
			left := s.roles.Get("owner")
			if !roleBool(farmMap(left, "ResidenceRetiredCareers"), id) || left[subject.prefix+"Rank"] != nil || left[subject.prefix+"Experience"] != nil ||
				farmMap(farmMap(left, "ArtisanResearch"), id) != nil || roleInt(left, "Live3Type") != 0 || roleInt(left, "Live4Value") != 18 {
				t.Fatalf("retirement did not clear its stale room and mirror: %v", left)
			}
			if err := s.roles.Update("owner", func(r Record) error {
				r[subject.prefix+"Rank"], r[subject.prefix+"Experience"] = 7, 61
				return nil
			}); err != nil {
				t.Fatal(err)
			}
			if voidRank(s.roles.Get("owner"), subject.kind) != 0 {
				t.Fatal("retired stamp revived from later workshop practice")
			}
			rebuild(subject.kind, 7)
			got := auctionTestServer(t, dir).roles.Get("owner")
			if voidRank(got, subject.kind) != 1 || roleBool(farmMap(got, "ResidenceRetiredCareers"), id) || got[subject.prefix+"Rank"] != nil || got[subject.prefix+"Experience"] != nil ||
				roleInt(got, "Live1Value") != 0 || roleInt(got, "ShinyJade") != 20 || voidRank(got, 6) != 3 {
				t.Fatalf("expected a fresh one-rank room and preserved independent career after restart: %v", got)
			}
			// A pre-existing, built room with zero rank can formally start its own profession.
			if err := s.roles.Update("owner", func(r Record) error {
				r["Live1Level"] = 0
				return nil
			}); err != nil {
				t.Fatal(err)
			}
			rebuild(subject.kind, subject.kind)
			if voidRank(s.roles.Get("owner"), subject.kind) != 1 {
				t.Fatal("zero-rank room could not be recommissioned")
			}
		})
	}
}
