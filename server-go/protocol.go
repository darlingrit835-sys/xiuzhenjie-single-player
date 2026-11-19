package main

import (
	_ "embed"
	"fmt"
	"strconv"
	"strings"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protodesc"
	"google.golang.org/protobuf/reflect/protoreflect"
	"google.golang.org/protobuf/types/descriptorpb"
	"google.golang.org/protobuf/types/dynamicpb"
)

//go:embed schema/xzj.pb
var schemaBytes []byte

//go:embed schema/java-commands.txt
var javaCommands string

type Protocol struct {
	messages map[string]protoreflect.MessageDescriptor
	fallback map[int]protoreflect.MessageDescriptor
	java     map[int]bool
}

func loadProtocol() (*Protocol, error) {
	var set descriptorpb.FileDescriptorSet
	if err := proto.Unmarshal(schemaBytes, &set); err != nil {
		return nil, fmt.Errorf("read PB descriptor: %w", err)
	}
	files, err := protodesc.NewFiles(&set)
	if err != nil {
		return nil, fmt.Errorf("resolve PB descriptor: %w", err)
	}
	p := &Protocol{
		messages: make(map[string]protoreflect.MessageDescriptor),
		fallback: make(map[int]protoreflect.MessageDescriptor),
		java:     make(map[int]bool),
	}
	files.RangeFiles(func(file protoreflect.FileDescriptor) bool {
		messages := file.Messages()
		for i := 0; i < messages.Len(); i++ {
			desc := messages.Get(i)
			name := string(desc.Name())
			p.messages[name] = desc
			// Java picks the first top-level message ending in Resp_<Cmd>.
			_, suffix, found := strings.Cut(name, "Resp_")
			if cmd, err := strconv.Atoi(suffix); found && err == nil && p.fallback[cmd] == nil {
				p.fallback[cmd] = desc
			}
		}
		return true
	})
	if err := validateEnvelope(p.messages["Msg"]); err != nil {
		return nil, err
	}
	for _, text := range strings.Fields(javaCommands) {
		cmd, err := strconv.Atoi(text)
		if err != nil {
			return nil, fmt.Errorf("invalid Java Cmd %q", text)
		}
		p.java[cmd] = true
	}
	return p, nil
}

func validateEnvelope(desc protoreflect.MessageDescriptor) error {
	if desc == nil {
		return fmt.Errorf("PB descriptor is missing Msg")
	}
	for _, expected := range []struct {
		number protoreflect.FieldNumber
		name   protoreflect.Name
		kind   protoreflect.Kind
	}{
		{1, "Cmd", protoreflect.Int32Kind},
		{2, "Msid", protoreflect.Int32Kind},
		{3, "Data", protoreflect.BytesKind},
	} {
		f := desc.Fields().ByNumber(expected.number)
		if f == nil || f.Name() != expected.name || f.Kind() != expected.kind || f.IsList() {
			return fmt.Errorf("PB Msg.%s does not match the wire encoder", expected.name)
		}
	}
	return nil
}

func (p *Protocol) message(name string) *dynamicpb.Message {
	desc := p.messages[name]
	if desc == nil {
		panic("unknown PB message " + name)
	}
	return dynamicpb.NewMessage(desc)
}

func (s *Server) pb(name string) *dynamicpb.Message {
	return s.protocol.message(name)
}

func (s *Server) decode(name string, data []byte) (*dynamicpb.Message, error) {
	message := s.pb(name)
	if err := proto.Unmarshal(data, message); err != nil {
		return nil, fmt.Errorf("decode %s: %w", name, err)
	}
	return message, nil
}

func (s *Server) unknownGame(peer *Peer, msg Message) {
	if s.preview != nil {
		s.sendFallback(peer, msg, false, previewDeniedCode)
		return
	}
	if s.protocol.java[msg.Cmd] {
		s.sendFallback(peer, msg, false, 1001)
		return
	}
	s.sendFallback(peer, msg, true, 2000)
}

func (s *Server) sendFallback(peer *Peer, msg Message, success bool, code int) {
	desc := s.protocol.fallback[msg.Cmd]
	if desc == nil {
		return
	}
	if s.preview != nil && !hasResponseStatus(desc) {
		return
	}
	_ = peer.send(msg.Cmd, msg.Msid, responseStatus(desc, success, code))
}

func field(m protoreflect.Message, name string) protoreflect.FieldDescriptor {
	f := m.Descriptor().Fields().ByName(protoreflect.Name(name))
	if f == nil {
		panic(fmt.Sprintf("field %q is missing from %s", name, m.Descriptor().FullName()))
	}
	return f
}

func set(m protoreflect.Message, name string, value any) {
	f := field(m, name)
	var v protoreflect.Value
	switch f.Kind() {
	case protoreflect.BoolKind:
		v = protoreflect.ValueOfBool(value.(bool))
	case protoreflect.StringKind:
		v = protoreflect.ValueOfString(value.(string))
	case protoreflect.BytesKind:
		v = protoreflect.ValueOfBytes(value.([]byte))
	case protoreflect.Int32Kind, protoreflect.Sint32Kind, protoreflect.Sfixed32Kind:
		v = protoreflect.ValueOfInt32(int32(pbInt(value)))
	case protoreflect.Int64Kind, protoreflect.Sint64Kind, protoreflect.Sfixed64Kind:
		v = protoreflect.ValueOfInt64(pbInt(value))
	case protoreflect.Uint32Kind, protoreflect.Fixed32Kind:
		v = protoreflect.ValueOfUint32(uint32(pbInt(value)))
	case protoreflect.Uint64Kind, protoreflect.Fixed64Kind:
		v = protoreflect.ValueOfUint64(uint64(pbInt(value)))
	case protoreflect.FloatKind:
		v = protoreflect.ValueOfFloat32(float32(pbFloat(value)))
	case protoreflect.DoubleKind:
		v = protoreflect.ValueOfFloat64(pbFloat(value))
	case protoreflect.EnumKind:
		v = protoreflect.ValueOfEnum(protoreflect.EnumNumber(pbInt(value)))
	case protoreflect.MessageKind, protoreflect.GroupKind:
		v = protoreflect.ValueOfMessage(value.(proto.Message).ProtoReflect())
	default:
		panic(fmt.Sprintf("unsupported PB field %s (%s)", f.FullName(), f.Kind()))
	}
	m.Set(f, v)
}

func pbInt(value any) int64 {
	switch v := value.(type) {
	case int:
		return int64(v)
	case int32:
		return int64(v)
	case int64:
		return v
	case uint:
		return int64(v)
	case uint32:
		return int64(v)
	case uint64:
		return int64(v)
	case float32:
		return int64(v)
	case float64:
		return int64(v)
	case protoreflect.EnumNumber:
		return int64(v)
	default:
		panic(fmt.Sprintf("expected PB number, got %T", value))
	}
}

func pbFloat(value any) float64 {
	switch v := value.(type) {
	case float32:
		return float64(v)
	case float64:
		return v
	default:
		return float64(pbInt(value))
	}
}

func getString(m protoreflect.Message, name string) string {
	return m.Get(field(m, name)).String()
}

func getInt(m protoreflect.Message, name string) int64 {
	return pbInt(m.Get(field(m, name)).Interface())
}

func getFloat(m protoreflect.Message, name string) float64 {
	return pbFloat(m.Get(field(m, name)).Interface())
}

func getBool(m protoreflect.Message, name string) bool {
	return m.Get(field(m, name)).Bool()
}

func addMessage(m protoreflect.Message, name string) protoreflect.Message {
	list := m.Mutable(field(m, name)).List()
	item := list.NewElement()
	list.Append(item)
	return item.Message()
}
