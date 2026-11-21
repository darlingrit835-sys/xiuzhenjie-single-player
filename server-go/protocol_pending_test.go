package main

import "testing"

func TestPendingCallbacksReceiveTypedRejection(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	for _, route := range pendingGameRoutes() {
		for _, enabled := range []bool{true, false} {
			peer, remote := auctionTestRole(t, s, "player", nil)
			registry := s.preview
			if !enabled {
				s.preview = nil
			}
			const seq = 9
			s.handleGame(peer, Message{Cmd: route.cmd, Msid: seq})
			s.preview = registry
			reply := remote.receive(t, s, route.cmd, seq, route.response)
			if getBool(reply, "IsSuccess") || getInt(reply, "Code") != 4008 {
				t.Fatalf("cmd %d returned %v", route.cmd, reply)
			}
		}
	}
}
