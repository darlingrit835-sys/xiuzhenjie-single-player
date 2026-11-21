package main

import (
	"bytes"
	"fmt"
	"math"
	"slices"
	"strings"
	"testing"
	"time"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

// BFS also returns a route usable by real 8001004 movement messages.
func routesFromSpawn(w *battleField) map[int]int {
	start := w.cell(w.playerX, w.playerY)
	parent := map[int]int{start: -1}
	queue := []int{start}
	for head := 0; head < len(queue); head++ {
		at := queue[head]
		for _, step := range []int{-1, 1, -w.width, w.width} {
			to := at + step
			if _, ok := parent[to]; ok || !w.passable(to) {
				continue
			}
			parent[to] = at
			queue = append(queue, to)
		}
	}
	return parent
}

func TestStitchedFieldHabitatAndConnectivity(t *testing.T) {
	ecology, err := loadBattleEcology("../server/data")
	if err != nil {
		t.Fatal(err)
	}
	for region, eco := range ecology {
		if region == "1121" && !slices.ContainsFunc(eco.fauna, func(s fieldSpecies) bool { return s.name == "浊沼鳄" && s.icon == 9 }) {
			t.Fatal("missing regional crocodile portrait")
		}
		if got := fieldCounty(region + "W13"); got != region {
			t.Fatalf("region lookup %s: %s", region, got)
		}
		for seed := range 32 {
			id := fmt.Sprintf("d_%s_%d", region, seed)
			r := dungeonRand(id)
			w := generateBattleField(region, 6, r)
			if !bytes.Equal(w.tiles, generateBattleField(region, 6, dungeonRand(id)).tiles) {
				t.Fatal("same seed changed geometry")
			}
			monsters := make([]battleMonster, 6)
			for i := range monsters {
				monsters[i] = battleMonster{index: 1001 + i, objectType: 2002, unusual: 1, name: "幼年·样品"}
			}
			monsters[2].objectType = 2001
			monsters[5].unusual = 4
			w.populate(id, monsters, eco, r)
			reached := routesFromSpawn(w)
			for i := range w.tiles {
				_, ok := reached[i]
				if w.passable(i) != ok {
					t.Fatalf("%s/%d unreachable ground %d", region, seed, i)
				}
			}
			occupied := map[int]bool{w.cell(w.playerX, w.playerY): true}
			for off := 0; off < len(w.landmarks); off += 3 {
				occupied[int(w.landmarks[off])|int(w.landmarks[off+1])<<8] = true
			}
			mark := func(x, y float64) byte {
				cell := w.cell(x, y)
				if _, ok := reached[cell]; !ok || occupied[cell] {
					t.Fatalf("%s invalid or overlapping spawn %d", id, cell)
				}
				occupied[cell] = true
				return w.tiles[cell]
			}
			for _, m := range monsters {
				if math.Hypot(m.x-w.playerX, m.y-w.playerY) <= 16 {
					t.Fatalf("%s enemy in safe entrance: %+v", id, m)
				}
				ground := mark(m.x, m.y)
				pool := eco.fauna
				if m.unusual == 4 {
					pool = eco.bosses
				} else if m.objectType == 2001 {
					pool = eco.people
				}
				_, name, _ := strings.Cut(m.name, "·")
				if !slices.ContainsFunc(pool, func(s fieldSpecies) bool { return s.name == name && s.kind == m.objectType }) {
					t.Fatalf("foreign species %s", m.name)
				}
				if m.objectType == 2001 && ground != fieldCamp && ground != fieldRuin {
					t.Fatalf("person on terrain %d", ground)
				}
				if m.objectType == 2002 && ground != fieldGrove && ground != fieldRidge {
					t.Fatalf("%s beast on terrain %d", id, ground)
				}
			}
			if len(w.nodes) < 4 {
				t.Fatal("missing resource classes")
			}
			visibleHerb := false
			for _, n := range w.nodes {
				ground := mark(n.x, n.y)
				cell := w.cell(n.x, n.y)
				if n.item.kind == 12 && w.explored[cell>>3]&(1<<uint(cell&7)) != 0 {
					visibleHerb = true
				}
				pool := eco.plants
				required := fieldGrove
				if n.item.kind == 13 {
					pool, required = eco.ores, fieldRidge
				}
				if ground != required || !slices.Contains(pool, n.item) || n.count < n.item.minQty || n.count > n.item.maxQty {
					t.Fatalf("bad resource site %+v", n)
				}
			}
			if !visibleHerb {
				t.Fatal("no forage visible from spawn")
			}
		}
	}
}

func TestFieldBlocksSightAndMovement(t *testing.T) {
	w := &battleField{width: 7, height: 5, tiles: bytes.Repeat([]byte{fieldPath}, 35), explored: make([]byte, 5)}
	start := 2*w.width + 1
	w.playerX, w.playerY = w.center(start)
	for _, barrier := range []byte{fieldWall, fieldWater} {
		clear(w.explored)
		w.tiles[start+1] = barrier
		w.reveal(start)
		lit := func(i int) bool { return w.explored[i>>3]&(1<<uint(i&7)) != 0 }
		if !lit(start+1) || lit(start+2) {
			t.Fatal("sight did not stop after first opaque cell")
		}
		if w.move(w.center(start+2)) || w.move(w.center(start+1)) || w.move(math.NaN(), 0) {
			t.Fatal("invalid move accepted")
		}
	}
	if !w.move(w.center(start+w.width)) || w.explored[start>>3]&(1<<uint(start&7)) == 0 {
		t.Fatal("lost explored history")
	}
}

func TestFieldWireGatherReconnectAndLegacy(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.dataDir = "../server/data"
	s.config = Config{BattleDurationSeconds: 120, MonsterCount: 6, MonsterDifficulty: 1, DropMultiplier: 1}
	peer, conn := auctionTestRole(t, s, "explorer", nil)
	role := s.roles.Get(peer.roleID)
	d := s.createDungeon(role, "1211W13")
	msg := func(cmd int, name string, values Record) Message {
		m := s.pb(name)
		putPB(m, values)
		body, err := proto.Marshal(m)
		if err != nil {
			t.Fatal(err)
		}
		return Message{Cmd: cmd, Msid: 9, Data: body}
	}
	s.handleDungeonGame(peer, msg(1010002, "RoleJoinDungeonReq_1010002", Record{"DungeonId": d.id, "MapId": d.mapID}))
	joined := conn.receive(t, s, 1010002, 9, "RoleJoinDungeonResp_1010002")
	skills := joined.Get(field(joined, "Skills")).List()
	if skills.Len() != len(d.skills) {
		t.Fatal("join did not return the role's skills")
	}
	for i, skill := range d.skills {
		if got := getInt(skills.Get(i).Message(), "CoolDown"); got != int64(skill.cooldownS)*1000 {
			t.Fatalf("role cooldown %s: %d", skill.id, got)
		}
	}
	enter := func(id string, version int) protoreflect.Message {
		s.handleBattle(peer, msg(8001001, "BattleRoleEnterReq_8001001", Record{"DungeonId": id, "RoleId": "explorer", "WorldVersion": version}))
		return conn.receive(t, s, 8001001, 9, "BattleRoleEnterResp_8001001")
	}
	resp := enter(d.id, 1)
	if !getBool(resp, "IsSuccess") || len(resp.Get(field(resp, "WorldTiles")).Bytes()) != d.world.width*d.world.height ||
		!bytes.Equal(resp.Get(field(resp, "Landmarks")).Bytes(), d.world.landmarks) || len(d.world.landmarks) != 12 || getString(resp, "RegionId") != "1211" {
		t.Fatal("missing field payload")
	}
	objects := resp.Get(field(resp, "Objects")).List()
	for i := 0; i < objects.Len(); i++ {
		object := objects.Get(i).Message()
		monster := d.monsters[int(getInt(object, "ObjectIndex"))-1001]
		scale := int64(1)
		if monster.objectType == 2001 {
			scale = 1000
		}
		abilities := object.Get(field(object, "Skills")).List()
		if abilities.Len() == 0 {
			t.Fatal("missing monster skills")
		}
		for j := 0; j < abilities.Len(); j++ {
			ability := abilities.Get(j).Message()
			idx := int(getInt(ability, "SkillIndex"))
			if monster.objectType == 2001 && (getInt(ability, "Type") != 1 || getInt(ability, "SkillType") != int64(battleSkills[idx-1].skillType)) {
				t.Fatalf("human AI cannot dispatch skill %v", ability)
			}
			if monster.objectType == 2002 && getInt(ability, "SkillType") != int64(idx) {
				t.Fatalf("beast AI cannot dispatch skill %v", ability)
			}
			want := int64(monster.skill(idx).cooldownS) * scale
			if got := getInt(ability, "CoolDown"); got != want {
				t.Fatalf("enemy %d skill %d cooldown %d expected %d", monster.objectType, idx, got, want)
			}
		}
	}
	if getBool(enter(d.id, 0), "IsSuccess") {
		t.Fatal("mixed versions in same dungeon")
	}
	w := d.world
	parent := routesFromSpawn(w)
	for _, group := range []string{"Plants", "Minerals"} {
		entries := resp.Get(field(resp, group)).List()
		for i := 0; i < entries.Len(); i++ {
			entry := entries.Get(i).Message()
			if field(entry, "Count").Number() != 11 || getInt(entry, "Count") != int64(w.nodes[getString(entry, "GoodsId")].count) {
				t.Fatalf("wrong %s quantity on wire: %v", group, entry)
			}
		}
	}
	nodes := resp.Get(field(resp, "Minerals")).List()
	advertised := getInt(nodes.Get(0).Message(), "Count")
	nodeID := getString(nodes.Get(0).Message(), "GoodsId")
	n := w.nodes[nodeID]
	dest := w.cell(n.x, n.y)
	path := []int{}
	for i := dest; i != -1; i = parent[i] {
		path = append(path, i)
	}
	pick := func() protoreflect.Message {
		s.handleDungeonGame(peer, msg(1004026, "PickUpGoodsReq_1004026", Record{"BattleId": d.id, "GoodsId": nodeID, "Count": 999}))
		return conn.receive(t, s, 1004026, 9, "PickUpGoodsResp_1004026")
	}
	if getBool(pick(), "IsSuccess") {
		t.Fatal("remote resource collected")
	}
	for _, cell := range slices.Backward(path) {
		x, y := w.center(cell)
		s.handleBattle(peer, msg(8001004, "BattleRoleMoveReq_8001004", Record{"RoleIndex": 1, "PosX": x, "PosY": y}))
	}
	award := pick()
	if !getBool(award, "IsSuccess") || getString(award, "GoodsId") != getString(pick(), "GoodsId") {
		t.Fatal("gather/retry failed")
	}
	goods := locateGoods(s.roles.Get("explorer"), getString(award, "GoodsId"))
	if roleInt(goods, "Count") != advertised || roleString(goods, "TemplateId") != n.item.id {
		t.Fatal("incorrect material, quantity or double credit")
	}
	seen := slices.Clone(w.explored)
	resp = enter(d.id, 1)
	if getFloat(resp, "PosX") != n.x || getFloat(resp, "PosY") != n.y || !bytes.Equal(resp.Get(field(resp, "Explored")).Bytes(), seen) || resp.Get(field(resp, "Minerals")).List().Len() != nodes.Len()-1 {
		t.Fatal("field state lost on reentry")
	}
	now := time.Now()
	if d.castFieldSkill(1, 1001, now) != 4534 || d.castFieldSkill(1, 1001, now) != 4535 || d.castFieldSkill(100, 0, now) != 4516 || d.castFieldSkill(1, 2000, now) != 4535 {
		t.Fatal("cast slot, target or cooldown not enforced")
	}
	s.handleBattle(peer, msg(8002003, "MonsterDeadReq_8002003", Record{"DeadMonsterIndex": 1001, "PosX": n.x, "PosY": n.y}))
	if _, ok := d.dead[1001]; !ok {
		t.Fatal("kill was not credited")
	}
	if d.castFieldSkill(1, 1001, now.Add(time.Minute)) != 4535 {
		t.Fatal("dead target accepted")
	}
	drops := len(conn.frames)
	conn.frames = nil
	resp = enter(d.id, 1)
	if resp.Get(field(resp, "Objects")).List().Len() != 5 || drops == 0 || len(conn.frames) != drops {
		t.Fatal("kill/drop state not reconstructed")
	}
	conn.frames = nil
	// The report opens on the game lane before the final battle-lane hits arrive.
	game := &Peer{roleID: "explorer", conn: conn}
	leaveGame := func() {
		s.handleDungeonGame(game, msg(1004037, "BattleEndLeaveReq_1004037", Record{"BattleId": d.id}))
		if !getBool(conn.receive(t, s, 1004037, 9, "BattleEndLeaveResp_1004037"), "IsSuccess") {
			t.Fatal("game leave rejected")
		}
	}
	leaveGame()
	before := roleInt(s.roles.Get("explorer"), "ShinyStone")
	for index := 1002; index <= 1006; index++ {
		s.handleBattle(peer, msg(8002003, "MonsterDeadReq_8002003", Record{"DeadMonsterIndex": index, "PosX": n.x, "PosY": n.y}))
		conn.frames = nil
	}
	reward := 30 + int64(d.playerLevel)*12
	if len(d.dead) != 6 || roleInt(s.roles.Get("explorer"), "ShinyStone") != before+reward {
		t.Fatal("leave raced last hit or credited unclaimed ground coins")
	}
	if err := s.roles.Update("explorer", func(r Record) error {
		r["BattleLoot"] = append(r["BattleLoot"].([]any), Record{"DungeonId": "another", "DropId": "other"})
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	leaveBattle := func() bool {
		s.handleBattle(peer, msg(8001008, "RoleLeaveDungeonReq_8001008", Record{"DungeonId": d.id}))
		return getBool(conn.receive(t, s, 8001008, 9, "RoleLeaveDungeonResp_8001008"), "IsSuccess")
	}
	savePath := s.roles.path
	s.roles.path = t.TempDir()
	failed := leaveBattle()
	s.roles.path = savePath
	if failed || d.closed {
		t.Fatal("exit accepted a failed settlement save")
	}
	if !leaveBattle() || !leaveBattle() {
		t.Fatal("battle leave retry failed")
	}
	leaveGame()
	saved := s.roles.Get("explorer")
	pending := battleRecords(saved["BattleLoot"])
	if !d.closed || len(pending) != 1 || roleString(pending[0], "DungeonId") != "another" || len(goodsList(saved)) != 1 || roleInt(saved, "ShinyStone") != before+reward {
		t.Fatal("leave discarded another dungeon's loot, auto-collected materials or duplicated credit")
	}
	legacy := s.createDungeon(role, "1211W13")
	resp = enter(legacy.id, 0)
	if !getBool(resp, "IsSuccess") || len(resp.Get(field(resp, "WorldTiles")).Bytes()) != 0 || len(resp.Get(field(resp, "Landmarks")).Bytes()) != 0 || getFloat(resp, "MapMinX") != -10 {
		t.Fatal("legacy field changed")
	}
}

func BenchmarkBattleFieldGeneration(b *testing.B) {
	ecology, err := loadBattleEcology("../server/data")
	if err != nil {
		b.Fatal(err)
	}
	for _, count := range []int{6, 64, 256} {
		b.Run(fmt.Sprintf("%d_enemies", count), func(b *testing.B) {
			monsters := make([]battleMonster, count)
			for i := range monsters {
				monsters[i] = battleMonster{index: i + 1001, objectType: 2002, unusual: 1, name: "成年·样品"}
			}
			rng := dungeonRand("benchmark")
			b.ReportAllocs()
			for b.Loop() {
				field := generateBattleField("1211", count, rng)
				field.populate("benchmark", monsters, ecology["1211"], rng)
			}
		})
	}
}

func TestFieldSkillLoadoutMessages(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	s.config.BattleDurationSeconds = 120
	peer, conn := auctionTestRole(t, s, "loadout", nil)
	call := func(cmd int, fields Record) protoreflect.Message {
		t.Helper()
		route := battleEquipmentProtocols[cmd]
		if cmd == 1010002 {
			route = gameMapProtocol{"RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002"}
		}
		msg := Message{Cmd: cmd, Msid: 9}
		if route.req != "" {
			req := s.pb(route.req)
			putPB(req, fields)
			var err error
			if msg.Data, err = proto.Marshal(req); err != nil {
				t.Fatal(err)
			}
		}
		if cmd == 1010002 {
			s.handleDungeonGame(peer, msg)
		} else {
			s.handleEquipment(peer, msg)
		}
		return conn.receive(t, s, cmd, msg.Msid, route.resp)
	}
	check := func(want ...string) {
		t.Helper()
		prep := call(1009007, nil)
		join := call(1010002, Record{"MapId": "1211W13"})
		active := join.Get(field(join, "Skills")).List()
		set := prep.Get(field(prep, "SetableSkills")).List()
		available := prep.Get(field(prep, "CanuseSkills")).List()
		if !getBool(join, "IsSuccess") || !getBool(prep, "IsSuccess") || active.Len() != len(want) || available.Len()+set.Len() != len(battleSkills) {
			t.Fatalf("incorrect loadout length active=%d prepared=%d available=%d, wanted %v", active.Len(), set.Len(), available.Len(), want)
		}
		equipped := make(map[string]bool)
		for slot, id := range want {
			battle := active.Get(slot).Message()
			if got := getString(battle, "TemplateId"); got != id {
				t.Fatalf("slot %d got %q, want %q", slot, got, id)
			}
			if id == "" {
				if getInt(battle, "Type") != 0 {
					t.Fatal("visible skill in empty slot")
				}
				continue
			}
			if len(equipped) >= set.Len() {
				t.Fatal("missing prepared skill", id)
			}
			skill, _ := battleSkillByID(id)
			row := set.Get(len(equipped)).Message()
			if getInt(row, "CombatSettingId") != int64(slot) || getString(row, "GoodsId") != id ||
				getInt(row, "Type") != int64(skill.skillType) || getInt(row, "SettingType") != int64(skill.kind) ||
				getInt(battle, "Type") != int64(skill.kind) || getInt(battle, "SkillType") != int64(skill.skillType) ||
				getInt(battle, "SkillIndex") != int64(slot+1) {
				t.Fatalf("slot %d mismatch preparation=%v battle=%v", slot, row, battle)
			}
			equipped[id] = true
		}
		if len(equipped) != set.Len() {
			t.Fatal("extra prepared skills")
		}
		for i := 0; i < available.Len(); i++ {
			row := available.Get(i).Message()
			id := getString(row, "GoodsId")
			skill, ok := battleSkillByID(id)
			if !ok || equipped[id] || getInt(row, "Type") != int64(skill.skillType) || getInt(row, "SettingType") != int64(skill.kind) {
				t.Fatal("invalid available skill", row)
			}
			equipped[id] = true
		}
	}
	check("skill_yujian", "skill_liehuo", "skill_qingfeng")
	if !getBool(call(1009004, Record{"SkillId": "skill_liehuo", "SkillType": 1}), "IsSuccess") {
		t.Fatal("remove failed")
	}
	check("skill_yujian", "", "skill_qingfeng")
	if !getBool(call(1009003, Record{"SkillId": "skill_qingfeng", "SkillType": 2, "CombatSettingId": 6}), "IsSuccess") {
		t.Fatal("move failed")
	}
	check("skill_yujian", "", "", "", "", "", "skill_qingfeng")
	if getBool(call(1009003, Record{"SkillId": "skill_liehuo", "CombatSettingId": 9}), "IsSuccess") {
		t.Fatal("out-of-bounds slot accepted")
	}
	for _, id := range []string{"skill_qingfeng", "skill_yujian"} {
		if !getBool(call(1009004, Record{"SkillId": id}), "IsSuccess") {
			t.Fatal("clear failed", id)
		}
	}
	check()
	reopened, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	s.roles = reopened
	check()
}

// Count physical room-to-room doors from the final tiles, independently of the
// planned graph: a misplaced opening can merge a cul-de-sac or lose a cycle.
func fieldRoutes(w *battleField) (loops, branches int) {
	cols, rows := w.width/fieldChunkSize, w.height/fieldChunkSize
	degree := make([]int, cols*rows)
	edges := 0
	open := func(a, b, seam, across, along int) {
		for t := range fieldChunkSize {
			i := seam + t*along
			if w.passable(i) && w.passable(i+across) {
				edges++
				degree[a]++
				degree[b]++
				return
			}
		}
	}
	for room := range degree {
		base := (room/cols*fieldChunkSize)*w.width + room%cols*fieldChunkSize
		if room%cols > 0 {
			open(room-1, room, base-1, 1, w.width)
		}
		if room >= cols {
			open(room-cols, room, base-w.width, w.width, 1)
		}
	}
	for _, d := range degree {
		if d == 1 {
			branches++
		}
	}
	return edges - len(degree) + 1, branches
}

// A joining pass can cut rock or bridge water. The original dry interior of the
// landscape, including its landmark anchor, must survive the stitch. The entrance
// changes at most the player tile and a neighboring herb tile.
func fieldMatchesLandmark(w *battleField, cell int, kind byte, spawn int) bool {
	ox := cell % w.width / fieldChunkSize * fieldChunkSize
	oy := cell / w.width / fieldChunkSize * fieldChunkSize
	for _, stamp := range fieldRoomStyles[kind].stamps {
		for mirror := 0; mirror < 2; mirror++ {
			for turns := range 4 {
				matches := true
				for y, row := range stamp {
					for x, ch := range []byte(row) {
						u, v := x, y
						for range turns {
							u, v = 6-v, u
						}
						if mirror == 1 {
							u = 6 - u
						}
						i := (oy+v+1)*w.width + ox + u + 1
						if ch == '@' {
							if i != cell {
								matches = false
							}
						} else if ch != '#' && ch != '~' && w.tiles[i] != fieldGlyph[ch] {
							nextToSpawn := magnitude(i%w.width-spawn%w.width)+magnitude(i/w.width-spawn/w.width) == 1
							if i != spawn && !(nextToSpawn && w.tiles[i] == fieldGrove) {
								matches = false
							}
						}
					}
				}
				if matches {
					return true
				}
			}
		}
	}
	return false
}

func TestFieldCountyScenery(t *testing.T) {
	for _, region := range []string{"1111", "1112", "1121", "1211", "1311", "1411"} {
		kinds := [7]int{}
		signature := countyLandmark(region)
		companions := fieldCompanions[signature]
		combinations := make(map[byte]bool)
		water, canopy := 0, 0
		minLoops, maxLoops, minBranches, maxBranches := 99, 0, 99, 0
		for seed := range 32 {
			id := fmt.Sprintf("d_%s_%d", region, seed)
			w := generateBattleField(region, 6, dungeonRand(id))
			replayed := generateBattleField(region, 6, dungeonRand(id))
			if !bytes.Equal(w.landmarks, replayed.landmarks) || len(w.landmarks) != 12 {
				t.Fatal(id, "scenery is missing or changes on reentry")
			}
			var seen byte
			spawn := w.cell(w.playerX, w.playerY)
			for off := 0; off < len(w.landmarks); off += 3 {
				cell := int(w.landmarks[off]) | int(w.landmarks[off+1])<<8
				kind := w.landmarks[off+2]
				if kind == 0 || int(kind) >= len(kinds) || !w.passable(cell) || cell == spawn || w.tiles[cell] != fieldRoomStyles[kind].ground {
					t.Fatalf("%s invalid scenery %d tile %d kind %d", id, cell, w.tiles[cell], kind)
				}
				if visible := w.explored[cell>>3]&(1<<uint(cell&7)) != 0; visible != (off == 0) {
					t.Fatalf("%s entry visibility %v at landmark %d", id, visible, off/3)
				}
				if off == 0 && kind != signature || off > 0 && !slices.Contains(companions, kind) {
					t.Fatalf("%s unexpected site %d at %d", id, kind, off/3)
				}
				if seen&(1<<kind) != 0 {
					t.Fatalf("%s repeated landscape %d", id, kind)
				}
				if !fieldMatchesLandmark(w, cell, kind, spawn) {
					t.Fatalf("%s scenery %d has the wrong room", id, kind)
				}
				kinds[kind]++
				seen |= 1 << kind
			}
			combinations[seen] = true
			l, b := fieldRoutes(w)
			if l < 1 || b < 1 {
				t.Fatalf("%s loops %d branches %d", id, l, b)
			}
			minLoops, maxLoops, minBranches, maxBranches = min(l, minLoops), max(l, maxLoops), min(b, minBranches), max(b, maxBranches)
			water += bytes.Count(w.tiles, []byte{fieldWater})
			canopy += bytes.Count(w.tiles, []byte{fieldGrove})
		}
		if len(combinations) != 4 {
			t.Fatalf("%s generated %d of the four regional three-site combinations", region, len(combinations))
		}
		for _, k := range companions {
			if kinds[k] == 0 {
				t.Fatal(region, "missing companion landscape", k)
			}
		}
		t.Logf("county %s: 32 maps, landmark counts %v, avg water/wood %d/%d cells, %d–%d cycles, %d–%d branch ends", region, kinds[1:], water/32, canopy/32, minLoops, maxLoops, minBranches, maxBranches)
	}
}
