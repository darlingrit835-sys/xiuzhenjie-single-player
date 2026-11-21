package main

import (
	"encoding/json"
	"time"
)

const fixedPurchaseHistoryCommand = 4901041
const fixedPurchasePageSize = 10
const fixedPurchaseRetention = 50

// The row holds display information as it stood at purchase. No private
// inventory, account or seller identifiers leave the transaction.
func addFixedPurchase(buyer, goods Record, seller string, total int64, now time.Time) {
	if seller == "" {
		seller = "坊市道友"
	}
	records := goodsListForField(buyer, "FixedPurchases")
	if len(records) >= fixedPurchaseRetention {
		records = records[len(records)-fixedPurchaseRetention+1:]
	}
	buyer["FixedPurchases"] = append(records, Record{"name": roleString(goods, "Name"), "count": max(1, transferStackCount(goods)),
		"kind": roleInt(goods, "Type"), "rank": max(1, roleInt(goods, "Rank")), "price": total, "at": now.UnixMilli(), "seller": seller})
}

func (s *Store) fixedPurchasesPage(roleID string, page int) Record {
	s.mu.RLock()
	defer s.mu.RUnlock()
	role := s.records[roleID]
	if role == nil {
		return nil
	}
	records := goodsListForField(role, "FixedPurchases")
	pages := max(1, (len(records)+fixedPurchasePageSize-1)/fixedPurchasePageSize)
	page = min(pages, max(1, page))
	end := len(records) - (page-1)*fixedPurchasePageSize
	rows := make([]Record, 0, fixedPurchasePageSize)
	for i := end - 1; i >= max(0, end-fixedPurchasePageSize); i-- {
		row, _ := records[i].(Record)
		rows = append(rows, Record{"name": roleString(row, "name"), "count": max(1, roleInt(row, "count")), "kind": roleInt(row, "kind"), "rank": roleInt(row, "rank"),
			"price": roleInt(row, "price"), "at": roleInt(row, "at"), "seller": roleString(row, "seller")})
	}
	return Record{"page": page, "pages": pages, "rows": rows}
}

func (s *Server) handleFixedPurchaseHistory(peer *Peer, msg Message) {
	id := peer.currentRoleID()
	active, online := s.online.Load(id)
	if id == "" || reservedStoreID(id) || !online || active != peer {
		sendAuctionJSON(peer, msg, 4008, Record{"error": "请先登入角色，再查买入簿"})
		return
	}
	var req struct {
		Page int `json:"page"`
	}
	if len(msg.Data) > 1024 || json.Unmarshal([]byte(offerToken(msg.Data)), &req) != nil {
		sendAuctionJSON(peer, msg, 4008, Record{"error": "买入簿页码无效"})
		return
	}
	result := s.roles.fixedPurchasesPage(id, req.Page)
	if result == nil {
		sendAuctionJSON(peer, msg, 4008, Record{"error": "角色不存在"})
		return
	}
	sendAuctionJSON(peer, msg, 2000, result)
}
