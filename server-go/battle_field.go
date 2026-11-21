package main

import (
	"hash/fnv"
	"math"
	"math/rand"
)

// Tile numbers, cell size and bit order form the WorldVersion=1 wire contract.
const (
	fieldWall byte = iota
	fieldPath
	fieldGrove
	fieldRidge
	fieldWater
	fieldCamp
	fieldRuin
)

const fieldChunkSize = 9
const fieldSight = 4

type battleField struct {
	width, height    int
	region           string
	tiles, explored  []byte
	landmarks        []byte
	playerX, playerY float64
	nodes            map[string]*fieldNode
}

func dungeonRand(id string) *rand.Rand {
	hash := fnv.New64a()
	hash.Write([]byte(id))
	return rand.New(rand.NewSource(int64(hash.Sum64())))
}

func fieldCounty(locations ...string) string {
	for _, id := range locations {
		for node := gameMapFind(id); node != nil; node = gameMapFind(node.parent) {
			if node.kind == 3 {
				return node.id
			}
		}
	}
	return "1111"
}

func (w *battleField) center(cell int) (float64, float64) {
	return float64(2*(cell%w.width) + 1 - w.width), float64(2*(cell/w.width) + 1 - w.height)
}

func (w *battleField) cell(x, y float64) int {
	if !(x >= -float64(w.width) && x < float64(w.width) && y >= -float64(w.height) && y < float64(w.height)) {
		return -1
	}
	return int((y+float64(w.height))/2)*w.width + int((x+float64(w.width))/2)
}

func (w *battleField) passable(cell int) bool {
	return cell >= 0 && cell < len(w.tiles) && w.tiles[cell] != fieldWall && w.tiles[cell] != fieldWater
}

func magnitude(a int) int {
	if a < 0 {
		return -a
	}
	return a
}

func (w *battleField) sees(from, to int) bool {
	x, y := from%w.width, from/w.width
	tx, ty := to%w.width, to/w.width
	dx, dy := magnitude(tx-x), -magnitude(ty-y)
	sx, sy := -1, -1
	if x < tx {
		sx = 1
	}
	if y < ty {
		sy = 1
	}
	e := dx + dy
	for x != tx || y != ty {
		e2 := 2 * e
		if e2 >= dy {
			e += dy
			x += sx
		}
		if e2 <= dx {
			e += dx
			y += sy
		}
		if (x != tx || y != ty) && !w.passable(y*w.width+x) {
			return false
		}
	}
	return true
}

func (w *battleField) reveal(origin int) {
	ox, oy := origin%w.width, origin/w.width
	for y := max(0, oy-fieldSight); y <= min(w.height-1, oy+fieldSight); y++ {
		for x := max(0, ox-fieldSight); x <= min(w.width-1, ox+fieldSight); x++ {
			dx, dy := x-ox, y-oy
			cell := y*w.width + x
			if dx*dx+dy*dy <= fieldSight*fieldSight && w.sees(origin, cell) {
				w.explored[cell>>3] |= 1 << uint(cell&7)
			}
		}
	}
}

// Walk the continuous segment through grid boundaries. Cell-centre rays used for
// visibility cannot model a moving actor grazing a corner.
func (w *battleField) clearSegment(x0, y0, x1, y1 float64) bool {
	start, end := w.cell(x0, y0), w.cell(x1, y1)
	if !w.passable(start) || !w.passable(end) {
		return false
	}
	x, y, tx, ty := start%w.width, start/w.width, end%w.width, end/w.width
	dx, dy := x1-x0, y1-y0
	var sx, sy int
	nextX, nextY, deltaX, deltaY := math.Inf(1), math.Inf(1), math.Inf(1), math.Inf(1)
	if dx != 0 {
		sx = 1
		edge := 2*(x+1) - w.width
		if dx < 0 {
			sx = -1
			edge -= 2
		}
		nextX = (float64(edge) - x0) / dx
		deltaX = 2 / math.Abs(dx)
	}
	if dy != 0 {
		sy = 1
		edge := 2*(y+1) - w.height
		if dy < 0 {
			sy = -1
			edge -= 2
		}
		nextY = (float64(edge) - y0) / dy
		deltaY = 2 / math.Abs(dy)
	}
	for x != tx || y != ty {
		switch {
		case x == tx || (y != ty && nextY < nextX):
			y += sy
			nextY += deltaY
		case y == ty || nextX < nextY:
			x += sx
			nextX += deltaX
		default:
			if !w.passable(y*w.width+x+sx) || !w.passable((y+sy)*w.width+x) {
				return false
			}
			x += sx
			y += sy
			nextX += deltaX
			nextY += deltaY
		}
		if !w.passable(y*w.width + x) {
			return false
		}
	}
	return true
}

func (w *battleField) move(x, y float64) bool {
	if !w.clearSegment(w.playerX, w.playerY, x, y) {
		return false
	}
	old, cell := w.cell(w.playerX, w.playerY), w.cell(x, y)
	w.playerX, w.playerY = x, y
	if cell != old {
		w.reveal(cell)
	}
	return true
}

func (w *battleField) withinReach(x, y float64) bool {
	return math.Hypot(w.playerX-x, w.playerY-y) <= 3 && w.clearSegment(w.playerX, w.playerY, x, y)
}
