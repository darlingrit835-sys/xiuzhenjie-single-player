package main

import (
	"encoding/json"
	"errors"
	"fmt"
	"io"
	"log"
	"sort"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
	"google.golang.org/protobuf/types/dynamicpb"
)

const (
	previewDeniedCode = 4008
	maxPreviewRequest = 64 << 10
)

// Preview builders receive only their declared save fields and protobuf objects.
// A route with no request or save fields must produce a constant response.
type previewRoute struct {
	cmd        int
	module     string
	request    string
	response   string
	summary    string
	blocked    bool
	roleFields []string
	build      func(role Record, req, out protoreflect.Message)
	live       func(s *Server, peer *Peer, req, out protoreflect.Message) (afterReply func(), err error)
}

type previewReject int

func (code previewReject) Error() string { return fmt.Sprintf("preview rejected (%d)", code) }

type compiledPreview struct {
	previewRoute
	reqDesc  protoreflect.MessageDescriptor
	respDesc protoreflect.MessageDescriptor
	failure  []byte
	cached   []byte
}

type previewRegistry map[int]compiledPreview

func loadPreview(p *Protocol) (previewRegistry, error) {
	registry := make(previewRegistry)
	for _, module := range [][]previewRoute{
		commercePreviewRoutes(), socialPreviewRoutes(), friendPreviewRoutes(), mentorshipPreviewRoutes(), mentorshipPracticeRoutes(), mentorshipGraduationRoutes(),
		craftingPreviewRoutes(), guildGovernanceRoutes(), roleExtraRoutes(),
		pendingGameRoutes(), storageRoutes(),
	} {
		for _, route := range module {
			if _, exists := registry[route.cmd]; exists || p.java[route.cmd] {
				return nil, fmt.Errorf("preview command %d conflicts with an existing handler", route.cmd)
			}
			req, resp := p.messages[route.request], p.messages[route.response]
			if route.cmd <= 0 || resp == nil || (route.request != "" && req == nil) {
				return nil, fmt.Errorf("preview command %d has invalid PB descriptors", route.cmd)
			}
			if route.module == "" || route.summary == "" || route.blocked == (route.build != nil) {
				return nil, fmt.Errorf("preview command %d has invalid module metadata or builder", route.cmd)
			}
			if !hasResponseStatus(resp) {
				return nil, fmt.Errorf("preview command %d has no error status field", route.cmd)
			}
			compiled := compiledPreview{previewRoute: route, reqDesc: req, respDesc: resp}
			var err error
			compiled.failure, err = proto.Marshal(responseStatus(resp, false, previewDeniedCode))
			if err != nil {
				return nil, err
			}
			if route.blocked {
				compiled.cached = compiled.failure
			} else if req == nil && len(route.roleFields) == 0 {
				compiled.cached, err = compiled.buildPayload(nil, nil)
				if err != nil {
					return nil, fmt.Errorf("encode static preview command %d: %w", route.cmd, err)
				}
			}
			registry[route.cmd] = compiled
		}
	}
	return registry, nil
}

func hasResponseStatus(desc protoreflect.MessageDescriptor) bool {
	fields := desc.Fields()
	return fields.ByName("IsSuccess") != nil || fields.ByName("Code") != nil
}

func responseStatus(desc protoreflect.MessageDescriptor, success bool, code int) *dynamicpb.Message {
	out := dynamicpb.NewMessage(desc)
	if desc.Fields().ByName("IsSuccess") != nil {
		set(out, "IsSuccess", success)
	}
	if desc.Fields().ByName("Code") != nil {
		set(out, "Code", code)
	}
	return out
}

func (route compiledPreview) buildPayload(role Record, req protoreflect.Message) ([]byte, error) {
	out := responseStatus(route.respDesc, true, 2000)
	route.build(role, req, out)
	return proto.Marshal(out)
}

func (s *Server) handlePreview(peer *Peer, msg Message) bool {
	route, ok := s.preview[msg.Cmd]
	if !ok {
		return false
	}
	live := s.previewWrites && route.live != nil
	if !live && route.cached != nil {
		_ = peer.sendPayload(msg.Cmd, msg.Msid, route.cached)
		return true
	}
	var req protoreflect.Message
	if route.reqDesc != nil {
		in := dynamicpb.NewMessage(route.reqDesc)
		if len(msg.Data) > maxPreviewRequest || proto.Unmarshal(msg.Data, in) != nil {
			_ = peer.sendPayload(msg.Cmd, msg.Msid, route.failure)
			return true
		}
		req = in
	}
	if live {
		s.runLiveRoute(peer, msg, route.previewRoute, req)
		return true
	}
	var role Record
	if len(route.roleFields) > 0 {
		role = s.roles.project(peer.currentRoleID(), route.roleFields)
		if role == nil {
			_ = peer.sendPayload(msg.Cmd, msg.Msid, route.failure)
			return true
		}
	}
	payload, err := route.buildPayload(role, req)
	if err != nil {
		payload = route.failure
	}
	_ = peer.sendPayload(msg.Cmd, msg.Msid, payload)
	return true
}

func (s *Server) runLiveRoute(peer *Peer, msg Message, route previewRoute, req protoreflect.Message) {
	desc := s.protocol.messages[route.response]
	if peer.currentRoleID() == "" {
		s.sendGame(peer, msg, responseStatus(desc, false, previewDeniedCode))
		return
	}
	out := responseStatus(desc, true, 2000)
	peer.gameRequestID = msg.Msid
	afterReply, err := route.live(s, peer, req, out)
	if err != nil {
		var code previewReject
		if errors.As(err, &code) {
			out = responseStatus(desc, false, int(code))
		} else {
			log.Printf("试用处理 cmd=%d: %v", msg.Cmd, err)
			out = responseStatus(desc, false, previewDeniedCode)
		}
	}
	s.sendGame(peer, msg, out)
	if err == nil && afterReply != nil {
		afterReply()
	}
}

type previewCatalogEntry struct {
	Cmd      int    `json:"cmd"`
	Module   string `json:"module"`
	Request  string `json:"request"`
	Response string `json:"response"`
	Summary  string `json:"summary"`
	Blocked  bool   `json:"blocked"`
	Live     bool   `json:"live,omitempty"`
}

func writeInterfaceCatalog(dest io.Writer) error {
	p, err := loadProtocol()
	if err != nil {
		return err
	}
	routes, err := loadPreview(p)
	if err != nil {
		return err
	}
	var catalog struct {
		GameNative    []int                 `json:"game_native"`
		DefaultDenied []int                 `json:"default_denied"`
		Preview       []previewCatalogEntry `json:"preview"`
		Fallback      map[int]string        `json:"fallback"`
	}
	for cmd := range p.java {
		catalog.GameNative = append(catalog.GameNative, cmd)
	}
	catalog.GameNative = append(catalog.GameNative, privateChatSend, privateChatHistory, achievementListCmd, achievementClaimCmd)
	for _, route := range chamberStorageRoutes {
		catalog.GameNative = append(catalog.GameNative, route.cmd)
	}
	sort.Ints(catalog.GameNative)
	for _, route := range pendingGameRoutes() {
		catalog.DefaultDenied = append(catalog.DefaultDenied, route.cmd)
	}
	for _, route := range routes {
		catalog.Preview = append(catalog.Preview, previewCatalogEntry{
			Cmd: route.cmd, Module: route.module, Request: route.request, Response: route.response,
			Summary: route.summary, Blocked: route.blocked, Live: route.live != nil,
		})
	}
	sort.Slice(catalog.Preview, func(i, j int) bool { return catalog.Preview[i].Cmd < catalog.Preview[j].Cmd })
	catalog.Fallback = make(map[int]string, len(p.fallback))
	for cmd, desc := range p.fallback {
		catalog.Fallback[cmd] = string(desc.Name())
	}
	encoder := json.NewEncoder(dest)
	encoder.SetIndent("", "  ")
	return encoder.Encode(catalog)
}
