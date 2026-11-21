package main

import (
	"math"
	"math/rand"
)

// Numeric values are shared with the Landmarks field and the Unity scenery atlas.
const (
	fieldSpiritTree byte = iota + 1
	fieldSpring
	fieldStarAltar
	fieldCrystal
	fieldSwordTomb
	fieldMountainGate
)

type fieldRoomStyle struct {
	ground byte
	stamps [2][7]string
}

// Each stamp is a landscape silhouette. @ is its scenic anchor. Rotation varies
// the shore, terraces and approach independently of the adjacent rooms.
var fieldRoomStyles = [...]fieldRoomStyle{
	fieldSpiritTree: {fieldGrove, [2][7]string{
		{"#gggg~#", "gggg.~~", "gg@.gg~", "gg..cgg", "r..ccg#", "rr.ggg#", "#rr.g##"},
		{"#ggg###", "gggggc#", "gg@ggcc", "~gg..g.", "~~g.r..", "gg..rr#", "#ggrr##"},
	}},
	fieldSpring: {fieldGrove, [2][7]string{
		{"#gg~~~#", "gg~~~~~", "g@.~~~~", "gg..~~~", "ggg.ru~", "#gg.uur", "##ggrr#"},
		{"##rrgg#", "rruugg~", "guu.~~~", "g..~~~~", "g@gg~~~", "ggggg~#", "#ggg###"},
	}},
	fieldStarAltar: {fieldRuin, [2][7]string{
		{"##gg~~#", "#g.ug~~", "guuuug~", "uu@.uu~", "ruu..gg", "#rr..gg", "##rrgg#"},
		{"#~gg###", "~~gguu#", "~guuuur", "gg.u@ur", "g..uuur", "#gg.rr#", "###gg##"},
	}},
	fieldCrystal: {fieldRidge, [2][7]string{
		{"##rrr##", "#rr#rr#", "grr@.rr", "gg..rrr", "gg.rru#", "g~rruu#", "#~gr###"},
		{"##grr##", "#ggrrur", "gg.ruur", "#g..rrr", "gggr@rr", "~~ggrr#", "#~rr###"},
	}},
	fieldSwordTomb: {fieldRuin, [2][7]string{
		{"##rruu#", "#rruuu#", "rruu@ur", "r..uuur", "gg.ruu#", "ggg.rr#", "#ggg###"},
		{"##rrr##", "#ruruur", "ggur@uu", "ggg.uur", "#g...rr", "#gggrr#", "##gg###"},
	}},
	fieldMountainGate: {fieldCamp, [2][7]string{
		{"##rrr##", "#rccrgg", "rrc@cgg", "rc..ccg", "ccc.gg#", "#gg..g#", "##grr##"},
		{"##ggr##", "#gggcr#", "gg.ccrr", "g..c@cr", "#grccrr", "#grrrr#", "##rrr##"},
	}},
}

func countyLandmark(region string) byte {
	switch region {
	case "1112":
		return fieldSpring
	case "1121":
		return fieldStarAltar
	case "1211":
		return fieldSwordTomb
	case "1311":
		return fieldMountainGate
	case "1411":
		return fieldCrystal
	default:
		return fieldSpiritTree
	}
}

// Each county draws three distinct deep sites from four suitable landscapes.
// The signature stays at the trailhead and is absent from its own draw.
var fieldCompanions = [...][]byte{
	fieldSpiritTree:   {fieldSpring, fieldMountainGate, fieldStarAltar, fieldSwordTomb},
	fieldSpring:       {fieldSpiritTree, fieldStarAltar, fieldCrystal, fieldMountainGate},
	fieldStarAltar:    {fieldSpring, fieldSpiritTree, fieldMountainGate, fieldCrystal},
	fieldCrystal:      {fieldSpring, fieldSwordTomb, fieldStarAltar, fieldMountainGate},
	fieldSwordTomb:    {fieldCrystal, fieldMountainGate, fieldSpiritTree, fieldStarAltar},
	fieldMountainGate: {fieldSwordTomb, fieldCrystal, fieldStarAltar, fieldSpiritTree},
}

var fieldGlyph = map[byte]byte{'#': fieldWall, '.': fieldPath, 'g': fieldGrove, 'r': fieldRidge, '~': fieldWater, 'c': fieldCamp, 'u': fieldRuin}

type fieldSite struct {
	room int
	kind byte
}

type fieldLink struct {
	a, b int
}

// Kruskal's tree connects every site. Open extra passes away from a leaf to
// provide shortcuts without closing the last branch into a cul-de-sac.
func fieldLinks(cols, rows int, rng *rand.Rand) []fieldLink {
	n := cols * rows
	parent, degree := make([]int, n), make([]int, n)
	var edges []fieldLink
	for i := range n {
		parent[i] = i
		if i%cols > 0 {
			edges = append(edges, fieldLink{i - 1, i})
		}
		if i >= cols {
			edges = append(edges, fieldLink{i - cols, i})
		}
	}
	root := func(i int) int {
		for i != parent[i] {
			parent[i] = parent[parent[i]]
			i = parent[i]
		}
		return i
	}
	rng.Shuffle(len(edges), func(i, j int) { edges[i], edges[j] = edges[j], edges[i] })
	var links, extra []fieldLink
	for _, e := range edges {
		a, b := root(e.a), root(e.b)
		if a == b {
			extra = append(extra, e)
			continue
		}
		parent[a] = b
		links = append(links, e)
		degree[e.a]++
		degree[e.b]++
	}
	branch := -1
	for i := n - 1; i >= 0; i-- {
		if degree[i] == 1 {
			for _, e := range extra {
				if e.a != i && e.b != i {
					branch = i
					break
				}
			}
			if branch >= 0 {
				break
			}
		}
	}
	wanted := max(1, (len(extra)+1)/2)
	for _, e := range extra {
		if e.a == branch || e.b == branch {
			continue
		}
		links = append(links, e)
		wanted--
		if wanted == 0 {
			break
		}
	}
	return links
}

func generateBattleField(region string, count int, rng *rand.Rand) *battleField {
	cols := max(3, int(math.Ceil(math.Sqrt(float64(count+8)/2))))
	rows := max(2, (count+8+cols*2-1)/(cols*2))
	w := &battleField{width: cols * fieldChunkSize, height: rows * fieldChunkSize, region: region, nodes: make(map[string]*fieldNode)}
	w.tiles = make([]byte, w.width*w.height)
	w.explored = make([]byte, (len(w.tiles)+7)/8)
	n := cols * rows
	signature := countyLandmark(region)
	// The trailhead previews the county motif. The other three sites have their
	// own clearings and are spread apart rather than drawn from independent rooms.
	sites := []fieldSite{{0, signature}}
	companions := fieldCompanions[signature]
	kinds := rng.Perm(len(companions))
	for _, k := range kinds[:3] {
		best, score := -1, -1
		for _, room := range rng.Perm(n) {
			d := n
			for _, site := range sites {
				d = min(d, magnitude(room%cols-site.room%cols)+magnitude(room/cols-site.room/cols))
			}
			if d > score {
				best, score = room, d
			}
		}
		sites = append(sites, fieldSite{best, companions[k]})
	}
	anchors := make([]int, n)
	for room := range n {
		// Select the nearest scenic region with mild edge noise to avoid hard
		// boundaries. A seed room always keeps its own landscape.
		kind, best := signature, math.MaxInt
		for _, site := range sites {
			d := magnitude(room%cols-site.room%cols) + magnitude(room/cols-site.room/cols)
			score := d*3 + rng.Intn(4)
			if d == 0 {
				kind = site.kind
				break
			}
			if score < best {
				kind, best = site.kind, score
			}
		}
		anchors[room] = w.paintFieldRoom(room, cols, kind, rng)
	}
	// Extend alternating banks across the nominal frame. A closed ridge can
	// wander by a cell without creating a diagonal or orthogonal passage.
	for room := range n {
		x, y := room%cols*fieldChunkSize, room/cols*fieldChunkSize
		if room%cols > 0 {
			w.blendFieldBanks(y*w.width+x-1, 1, w.width, rng)
		}
		if room >= cols {
			w.blendFieldBanks((y-1)*w.width+x, w.width, 1, rng)
		}
	}
	for _, e := range fieldLinks(cols, rows, rng) {
		w.openFieldPass(e, cols, rng)
	}
	for _, site := range sites {
		i := anchors[site.room]
		if i <= math.MaxUint16 {
			w.landmarks = append(w.landmarks, byte(i), byte(i>>8), site.kind)
		}
	}
	// Leave a herb beside the player and a clear view of the trailhead landmark.
	start := anchors[0]
	for _, d := range []int{-w.width, -1, w.width, 1} {
		if w.passable(anchors[0] + d) {
			start = anchors[0] + d
			break
		}
	}
	w.tiles[start] = fieldPath
	for _, d := range []int{1, w.width, -1, -w.width} {
		if i := start + d; i != anchors[0] && w.passable(i) {
			w.tiles[i] = fieldGrove
			break
		}
	}
	w.playerX, w.playerY = w.center(start)
	w.reveal(start)
	return w
}

func (w *battleField) paintFieldRoom(room, cols int, kind byte, rng *rand.Rand) int {
	style := fieldRoomStyles[kind]
	stamp := style.stamps[rng.Intn(len(style.stamps))]
	turns, reflect := rng.Intn(4), rng.Intn(2) == 0
	ox, oy := room%cols*fieldChunkSize, room/cols*fieldChunkSize
	anchor := 0
	for y := 0; y < fieldChunkSize; y++ {
		for x := 0; x < fieldChunkSize; x++ {
			u, v := min(6, max(0, x-1)), min(6, max(0, y-1))
			if reflect {
				u = 6 - u
			}
			for range turns {
				u, v = v, 6-u
			}
			ch := stamp[v][u]
			i := (oy+y)*w.width + ox + x
			if x == 0 || y == 0 || x == fieldChunkSize-1 || y == fieldChunkSize-1 {
				// Continue the lake into its border. The other cliffs are cut by passes.
				if ch == '~' {
					w.tiles[i] = fieldWater
				}
			} else if ch == '@' {
				w.tiles[i] = style.ground
				anchor = i
			} else {
				w.tiles[i] = fieldGlyph[ch]
			}
		}
	}
	return anchor
}

func (w *battleField) blendFieldBanks(seam, across, along int, rng *rand.Rand) {
	far := rng.Intn(2) == 0
	for t := 1; t < fieldChunkSize-1; t++ {
		if rng.Intn(4) == 0 {
			far = !far
		}
		lip, inward := seam+t*along, -across
		if far {
			lip += across
			inward = across
		}
		inner := w.tiles[lip+inward]
		if inner != fieldWall {
			w.tiles[lip] = inner
		}
	}
}

// Grow a pass inward until it meets the local floor. The two banks can approach
// at different angles; wider saddles extend the neighboring biome across seams.
func (w *battleField) openFieldPass(edge fieldLink, cols int, rng *rand.Rand) {
	ox, oy := edge.a%cols*fieldChunkSize, edge.a/cols*fieldChunkSize
	forward, side := 1, w.width
	seam := (oy*w.width + ox) + fieldChunkSize - 1
	if edge.b-edge.a == cols {
		forward, side = w.width, 1
		seam = (oy+fieldChunkSize-1)*w.width + ox
	}
	lane := 2 + rng.Intn(5)
	mouth := seam + lane*side
	for _, bank := range []struct{ start, step int }{{mouth, -forward}, {mouth + forward, forward}} {
		// Find the nearest floor on this bank, including diagonal shores. Long
		// stretches of lake cannot terminate a passage before reaching dry land.
		best, depth, shift := math.MaxInt, 0, 0
		for d := 1; d < fieldChunkSize-1; d++ {
			for t := 1; t < fieldChunkSize-1; t++ {
				delta := t - lane
				score := d + magnitude(delta)
				if score < best && w.passable(bank.start+d*bank.step+delta*side) {
					best, depth, shift = score, d, delta
				}
			}
		}
		ground := w.tiles[bank.start+depth*bank.step+shift*side]
		cur := bank.start
		w.tiles[cur] = fieldPath
		turn := rng.Intn(depth) + 1
		for d := 1; d <= depth; d++ {
			cur += bank.step
			if !w.passable(cur) {
				w.tiles[cur] = fieldPath
			}
			if d != turn {
				continue
			}
			step := side
			if shift < 0 {
				step = -side
			}
			for range magnitude(shift) {
				cur += step
				if !w.passable(cur) {
					w.tiles[cur] = fieldPath
				}
			}
		}
		for _, slope := range []int{-1, 1} {
			span := rng.Intn(3)
			for k := 1; k <= span; k++ {
				t := lane + slope*k
				if t > 0 && t < fieldChunkSize-1 {
					w.tiles[bank.start+slope*k*side] = ground
				}
			}
		}
	}
}
