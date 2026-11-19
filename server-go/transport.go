package main

import (
	"bufio"
	"encoding/binary"
	"errors"
	"fmt"
	"io"
	"log"
	"net"
	"runtime/debug"
	"sync"
	"sync/atomic"
	"time"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/proto"
)

const maxFrameSize = 8 << 20

type Message struct {
	Cmd  int
	Msid int
	Data []byte
}

type Peer struct {
	privateReceipts          *privateChatReceipts
	privatePushes            privateChatPushes
	conn                     net.Conn
	writes                   sync.Mutex
	onlineRole               atomic.Value
	roleID                   string
	dungeonID                string
	lastHomeUpgradeMessageID int
	lastPracticeGuideID      int
	gameRequestID            int
	transferReceipts         *friendTransferReceipts
	acupointReceipts         *acupointReceipts
}

func (p *Peer) currentRoleID() string {
	id, _ := p.onlineRole.Load().(string)
	return id
}

func (p *Peer) send(cmd, msid int, data proto.Message) error {
	var payload []byte
	if data != nil {
		var err error
		payload, err = proto.Marshal(data)
		if err != nil {
			return err
		}
	}
	return p.sendPayload(cmd, msid, payload)
}

func (p *Peer) sendPayload(cmd, msid int, payload []byte) error {
	frame := encodeFrame(cmd, msid, payload)
	p.writes.Lock()
	defer p.writes.Unlock()
	return p.writeFrameLocked(frame)
}

func (p *Peer) writeFrameLocked(frame []byte) error {
	if err := p.conn.SetWriteDeadline(time.Now().Add(5 * time.Second)); err != nil {
		return err
	}
	if _, err := p.conn.Write(frame); err != nil {
		_ = p.conn.Close()
		return err
	}
	return nil
}

func encodeFrame(cmd, msid int, payload []byte) []byte {
	command := uint64(int64(int32(cmd)))
	var sequence uint64
	if msid > 0 {
		sequence = uint64(int64(int32(msid)))
	}
	size := 0
	if command != 0 {
		size += 1 + protowire.SizeVarint(command)
	}
	if sequence != 0 {
		size += 1 + protowire.SizeVarint(sequence)
	}
	if len(payload) != 0 {
		size += 1 + protowire.SizeBytes(len(payload))
	}
	frame := make([]byte, 0, protowire.SizeVarint(uint64(size))+size)
	frame = protowire.AppendVarint(frame, uint64(size))
	if command != 0 {
		frame = protowire.AppendVarint(protowire.AppendTag(frame, 1, protowire.VarintType), command)
	}
	if sequence != 0 {
		frame = protowire.AppendVarint(protowire.AppendTag(frame, 2, protowire.VarintType), sequence)
	}
	if len(payload) != 0 {
		frame = protowire.AppendBytes(protowire.AppendTag(frame, 3, protowire.BytesType), payload)
	}
	return frame
}

func (s *Server) accept(listener net.Listener, battle bool, active *sync.WaitGroup) error {
	for {
		conn, err := listener.Accept()
		if err != nil {
			return err
		}
		s.conns.Store(conn, true)
		active.Add(1)
		go func() {
			defer active.Done()
			defer s.conns.Delete(conn)
			defer conn.Close()
			peer := &Peer{conn: conn}
			if !battle {
				defer func() {
					if s.previewWrites {
						s.previewAuction.leave(peer)
					}
					s.online.CompareAndDelete(peer.roleID, peer)
					s.closeGame(peer)
				}()
			}
			if err := s.readMessages(peer, battle); err != nil && !errors.Is(err, io.EOF) && !errors.Is(err, net.ErrClosed) {
				log.Printf("TCP read: %v", err)
			}
		}()
	}
}

func (s *Server) readMessages(peer *Peer, battle bool) error {
	reader := bufio.NewReader(peer.conn)
	for {
		size, err := binary.ReadUvarint(reader)
		if err != nil {
			return err
		}
		if size > maxFrameSize {
			return fmt.Errorf("PB frame too large: %d", size)
		}
		body := make([]byte, int(size))
		if _, err := io.ReadFull(reader, body); err != nil {
			return err
		}
		in, err := s.decode("Msg", body)
		if err != nil {
			return err
		}
		message := Message{
			Cmd: int(getInt(in, "Cmd")), Msid: int(getInt(in, "Msid")), Data: in.Get(field(in, "Data")).Bytes(),
		}
		s.dispatch(peer, message, battle)
	}
}

func (s *Server) dispatch(peer *Peer, msg Message, battle bool) {
	defer func() {
		if problem := recover(); problem != nil {
			log.Printf("PB cmd=%d: %v\n%s", msg.Cmd, problem, debug.Stack())
		}
	}()
	if battle {
		s.handleBattle(peer, msg)
		return
	}
	s.handleGame(peer, msg)
}
