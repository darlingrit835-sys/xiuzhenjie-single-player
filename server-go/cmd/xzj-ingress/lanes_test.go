package main

import (
	"context"
	"crypto/sha256"
	"io"
	"net"
	"net/http"
	"net/http/httptest"
	"strings"
	"testing"
	"time"
)

func TestAccountLanesDoNotQueueBehindSlowOTPOrCredentials(t *testing.T) {
	ctx, cancel := context.WithCancel(t.Context())
	otpStarted := make(chan struct{}, ingressOTPRequests)
	credentialStarted := make(chan struct{}, ingressCredentialRequests)
	releaseOTP, releaseCredentials := make(chan struct{}), make(chan struct{})
	upstream := httptest.NewTLSServer(http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		if r.URL.Query().Get("hold") == "1" {
			started, release := credentialStarted, releaseCredentials
			if strings.HasPrefix(r.URL.Path, ingressOTPPath) {
				started, release = otpStarted, releaseOTP
			}
			started <- struct{}{}
			select {
			case <-release:
			case <-ctx.Done():
				return
			}
		}
		_, _ = io.WriteString(w, "upstream "+r.URL.Path)
	}))
	defer upstream.Close()
	server := httptest.NewTLSServer(newIngress(ctx, testIngressKey, upstream.Listener.Addr().String(), sha256.Sum256(upstream.Certificate().Raw), "", ""))
	defer server.Close()
	defer cancel()
	server.Client().Timeout = 4 * time.Second

	start := func(path string) <-chan ingressLaneReply {
		result := make(chan ingressLaneReply, 1)
		go func() { result <- requestIngressLane(server, http.MethodPost, path+"?hold=1", false) }()
		return result
	}
	otp := make([]<-chan ingressLaneReply, 0, ingressOTPRequests)
	for range ingressOTPRequests {
		otp = append(otp, start(ingressOTPPath))
		waitLaneStarted(t, otpStarted)
	}
	assertLaneReply(t, requestIngressLane(server, http.MethodPost, ingressOTPPath+"/alias", false), 200, otpBusyReply)
	assertLaneReply(t, requestIngressLane(server, http.MethodPost, "/login", false), 200, "upstream /login")

	credentials := make([]<-chan ingressLaneReply, 0, ingressCredentialRequests)
	for index := range ingressCredentialRequests {
		path := "/login"
		if index%2 == 1 {
			path = "/user/register"
		}
		credentials = append(credentials, start(path))
		waitLaneStarted(t, credentialStarted)
	}
	assertLaneReply(t, requestIngressLane(server, http.MethodPost, "/user/register/legacy", false), 200, accountBusyReply)
	assertLaneReply(t, requestIngressLane(server, http.MethodPost, "/login", false), 200, accountBusyReply)
	assertLaneReply(t, requestIngressLane(server, http.MethodGet, "/gameService/statusSearch", false), 200, "upstream /gameService/statusSearch")

	close(releaseOTP)
	for _, request := range otp {
		assertLaneReply(t, <-request, 200, "upstream "+ingressOTPPath)
	}
	assertLaneReply(t, requestIngressLane(server, http.MethodPost, ingressOTPPath, false), 200, "upstream "+ingressOTPPath)
	close(releaseCredentials)
	for index, request := range credentials {
		path := "/login"
		if index%2 == 1 {
			path = "/user/register"
		}
		assertLaneReply(t, <-request, 200, "upstream "+path)
	}
	assertLaneReply(t, requestIngressLane(server, http.MethodPost, "/login", false), 200, "upstream /login")
}

func TestGameAndBattleHaveIndependentTunnelBudgets(t *testing.T) {
	gameUpstream := laneTestListen(t)
	battleUpstream := laneTestListen(t)
	ctx, cancel := context.WithCancel(t.Context())
	handler := newIngress(ctx, testIngressKey, "", [32]byte{}, gameUpstream.Addr().String(), battleUpstream.Addr().String())
	// Exercise real TLS upgrades while using one occupied slot per lane.
	profile := defaultIngressLimits()
	profile.Game, profile.Battle = 1, 1
	handler.limits.replace(profile)
	server := httptest.NewTLSServer(handler)
	defer server.Close()
	defer cancel()
	server.Client().Timeout = 3 * time.Second
	game, gameReader := connectTestTunnel(t, server, ingressGamePath, nil)
	peer, err := gameUpstream.Accept()
	if err != nil {
		t.Fatal(err)
	}
	defer peer.Close()
	assertLaneReply(t, requestIngressLane(server, http.MethodGet, ingressGamePath, true), 503, "tunnel capacity unavailable\n")
	connectTestTunnel(t, server, ingressBattlePath, nil)
	assertLaneReply(t, requestIngressLane(server, http.MethodGet, ingressBattlePath, true), 503, "tunnel capacity unavailable\n")

	if err := game.CloseWrite(); err != nil {
		t.Fatal(err)
	}
	_ = peer.SetReadDeadline(time.Now().Add(2 * time.Second))
	if _, err := peer.Read(make([]byte, 1)); err != io.EOF {
		t.Fatalf("game upstream did not receive EOF: %v", err)
	}
	_ = peer.Close()
	if _, err := gameReader.ReadByte(); err != io.EOF {
		t.Fatalf("game TLS closure failed: %v", err)
	}
	// The connection sends its final TLS notification before the handler returns its slot.
	deadline := time.Now().Add(time.Second)
	for handler.limits.metrics().Lanes["game"].InFlight != 0 {
		if time.Now().After(deadline) {
			t.Fatal("game tunnel did not release its slot")
		}
		time.Sleep(time.Millisecond)
	}
	connectTestTunnel(t, server, ingressGamePath, nil)
	assertLaneReply(t, requestIngressLane(server, http.MethodGet, ingressBattlePath, true), 503, "tunnel capacity unavailable\n")
}

type ingressLaneReply struct {
	status int
	body   string
	err    error
}

func requestIngressLane(server *httptest.Server, method, path string, upgrade bool) ingressLaneReply {
	request, err := http.NewRequest(method, server.URL+path, nil)
	if err != nil {
		return ingressLaneReply{err: err}
	}
	request.Header.Set(ingressKeyHeader, testIngressKey)
	if upgrade {
		request.Header.Set("Connection", "Upgrade")
		request.Header.Set("Upgrade", ingressUpgrade)
	}
	response, err := server.Client().Do(request)
	if err != nil {
		return ingressLaneReply{err: err}
	}
	defer response.Body.Close()
	body, err := io.ReadAll(response.Body)
	return ingressLaneReply{status: response.StatusCode, body: string(body), err: err}
}

func assertLaneReply(t *testing.T, got ingressLaneReply, status int, body string) {
	t.Helper()
	if got.err != nil || got.status != status || got.body != body {
		t.Fatalf("expected %d %q, got %d %q (%v)", status, body, got.status, got.body, got.err)
	}
}

func waitLaneStarted(t *testing.T, started <-chan struct{}) {
	t.Helper()
	select {
	case <-started:
	case <-time.After(2 * time.Second):
		t.Fatal("expected request did not enter the upstream slot")
	}
}

func laneTestListen(t *testing.T) net.Listener {
	t.Helper()
	listener, err := net.Listen("tcp", "127.0.0.1:0")
	if err != nil {
		t.Fatal(err)
	}
	t.Cleanup(func() { _ = listener.Close() })
	return listener
}
