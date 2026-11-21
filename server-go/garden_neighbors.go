package main

import (
	"maps"
	"slices"
	"strings"
	"time"
)

func gardenFriends(actorID, otherID string, actor, other Record) bool {
	_, sent := friendMap(actor, friendLinks)[otherID]
	_, accepted := friendMap(other, friendLinks)[actorID]
	return sent && accepted
}

func gardenRipe(role Record, now time.Time) int {
	n := 0
	for _, v := range gardenPlots(role) {
		plot, _ := v.(Record)
		if gardenVariety(roleString(plot, "SeedId")) != nil && now.UnixMilli() >= roleInt(plot, "HarvestAt") && roleInt(plot, "Picked") == 0 {
			n++
		}
	}
	return n
}

func gardenNeighbors(tx *StoreTx, actorID string, actor Record, scope string, now time.Time) Record {
	rows := []Record{}
	friends := scope == "friends"
	ids := tx.store.order
	start := 0
	if friends {
		ids = slices.Sorted(maps.Keys(friendMap(actor, friendLinks)))
	} else if len(ids) > 0 {
		// Rotate through registered gardens so early accounts do not absorb every visit.
		start = int((now.Unix() / 60) % int64(len(ids)))
	}
	for i := range len(ids) {
		id := ids[(start+i)%len(ids)]
		if id == actorID || strings.HasPrefix(id, "$") {
			continue
		}
		other := tx.View(id)
		if other == nil || friends != gardenFriends(actorID, id, actor, other) {
			continue
		}
		if !friends && len(gardenPlots(other)) == 0 {
			continue
		}
		rows = append(rows, Record{"id": id, "name": roleString(other, "Name"), "friend": friends, "ripe": gardenRipe(other, now)})
		if !friends && len(rows) >= 16 {
			break
		}
	}
	return Record{"neighbors": rows}
}
