package main

import (
	"bytes"
	"encoding/binary"
	"testing"

	"google.golang.org/protobuf/proto"
)

func TestWireEncoderMatchesPBEnvelope(t *testing.T) {
	p, err := loadProtocol()
	if err != nil {
		t.Fatal(err)
	}
	for _, test := range []Message{
		{},
		{Cmd: 9000000},
		{Cmd: 1002002, Msid: 11, Data: []byte{0x12, 0x34}},
		{Cmd: 8002003, Msid: -1, Data: bytes.Repeat([]byte{0x23}, 500)},
		{Cmd: -1, Msid: 2147483647},
	} {
		envelope := p.message("Msg")
		set(envelope, "Cmd", test.Cmd)
		if test.Msid > 0 {
			set(envelope, "Msid", test.Msid)
		}
		if len(test.Data) > 0 {
			set(envelope, "Data", test.Data)
		}
		body, err := (proto.MarshalOptions{Deterministic: true}).Marshal(envelope)
		if err != nil {
			t.Fatal(err)
		}
		want := append(binary.AppendUvarint(nil, uint64(len(body))), body...)
		if frame := encodeFrame(test.Cmd, test.Msid, test.Data); !bytes.Equal(want, frame) {
			t.Fatalf("wire differs from protobuf for Cmd=%d Msid=%d", test.Cmd, test.Msid)
		}
	}
}
