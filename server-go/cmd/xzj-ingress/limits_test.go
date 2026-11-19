package main

import (
	"context"
	"crypto/sha256"
	"encoding/json"
	"io"
	"net/http"
	"net/http/httptest"
	"os"
	"path/filepath"
	"strings"
	"testing"
	"time"
)

func TestIngressLimitsMustBeCompleteAndWithinBounds(t *testing.T) {
	valid := `{"account":16,"auth":4,"otp":2,"game":64,"battle":64,"otpPerMinute":24}`
	if profile, err := decodeIngressLimits([]byte(valid)); err != nil || profile != defaultIngressLimits() {
		t.Fatalf("valid profile rejected: %+v (%v)", profile, err)
	}
	for _, bad := range []string{
		"null", "[]", valid + " {}", strings.TrimSuffix(valid, "}"), `{"game":0}`,
		strings.Replace(valid, `"game":64`, `"game":null`, 1),
		strings.Replace(valid, `"game":64`, `"game":64,"game":0`, 1),
		strings.Replace(valid, `"game":64`, `"extra":64`, 1),
		strings.Replace(valid, `"game":64`, `"game":-1`, 1),
		strings.Replace(valid, `"game":64`, `"game":513`, 1),
		strings.Replace(valid, `"game":64`, `"game":1.5`, 1),
	} {
		if _, err := decodeIngressLimits([]byte(bad)); err == nil {
			t.Fatalf("invalid profile accepted: %s", bad)
		}
	}
}

func TestHotLimitsPreserveLivePBWhileDrainingAndRaising(t *testing.T) {
	gameUpstream := laneTestListen(t)
	battleUpstream := laneTestListen(t)
	ctx, cancel := context.WithCancel(t.Context())
	handler := newIngress(ctx, testIngressKey, "", [32]byte{}, gameUpstream.Addr().String(), battleUpstream.Addr().String())
	server := httptest.NewTLSServer(handler)
	defer server.Close()
	defer cancel()
	server.Client().Timeout = 3 * time.Second
	initial := defaultIngressLimits()
	initial.Game, initial.Battle = 1, 1
	path := filepath.Join(t.TempDir(), "limits.json")
	writeLimitTestProfile(t, path, initial)
	if changed, err := handler.limits.reload(path); err != nil || !changed {
		t.Fatalf("initial profile not applied: changed=%v err=%v", changed, err)
	}
	go handler.limits.watch(ctx, path, 10*time.Millisecond)
	unauthorized, err := server.Client().Get(server.URL + ingressMetricsPath)
	if err != nil {
		t.Fatal(err)
	}
	_ = unauthorized.Body.Close()
	if unauthorized.StatusCode != http.StatusForbidden {
		t.Fatal("metrics were exposed without the ingress capability")
	}
	assertLaneReply(t, requestIngressLane(server, http.MethodPost, ingressMetricsPath, false), 405, "method not allowed\n")
	oldGame, gameReader := connectTestTunnel(t, server, ingressGamePath, nil)
	peer, err := gameUpstream.Accept()
	if err != nil {
		t.Fatal(err)
	}
	defer peer.Close()
	_ = peer.SetDeadline(time.Now().Add(3 * time.Second))

	drained := initial
	drained.Game, drained.Account = 0, 0
	writeLimitTestProfile(t, path, drained)
	got := waitLimitTestProfile(t, server, drained)
	if got.Lanes["game"].InFlight != 1 || got.Lanes["game"].Admitted != 1 {
		t.Fatalf("profile drain closed or lost a live connection: %+v", got)
	}
	assertLaneReply(t, requestIngressLane(server, http.MethodGet, ingressGamePath, true), 503, "tunnel capacity unavailable\n")
	assertLaneReply(t, requestIngressLane(server, http.MethodGet, "/gameService/statusSearch", false), 200, accountBusyReply)
	connectTestTunnel(t, server, ingressBattlePath, nil)
	if _, err := oldGame.Write([]byte("\x06s\x00tale")); err != nil {
		t.Fatal(err)
	}
	read := make([]byte, 7)
	if _, err := io.ReadFull(peer, read); err != nil || string(read) != "\x06s\x00tale" {
		t.Fatalf("old game stream stopped after draining: %q (%v)", read, err)
	}
	if _, err := peer.Write([]byte("reply")); err != nil {
		t.Fatal(err)
	}
	if _, err := io.ReadFull(gameReader, read[:5]); err != nil || string(read[:5]) != "reply" {
		t.Fatalf("old game reply lost after draining: %q (%v)", read[:5], err)
	}
	if err := os.WriteFile(path, []byte(`{"account":4,"game":512`), 0o600); err != nil {
		t.Fatal(err)
	}
	if changed, err := handler.limits.reload(path); err == nil || changed || readLimitTestMetrics(t, server).Limits != drained {
		t.Fatalf("malformed reload did not keep the complete previous profile: changed=%v err=%v", changed, err)
	}

	raised := initial
	raised.Game = 2
	writeLimitTestProfile(t, path, raised)
	waitLimitTestProfile(t, server, raised)
	connectTestTunnel(t, server, ingressGamePath, nil)
	assertLaneReply(t, requestIngressLane(server, http.MethodGet, ingressGamePath, true), 503, "tunnel capacity unavailable\n")
	got = readLimitTestMetrics(t, server)
	if got.Lanes["game"] != (ingressLaneMetrics{InFlight: 2, Admitted: 2, Rejected: 2}) || got.Lanes["battle"].InFlight != 1 || got.Lanes["account"].Rejected != 1 {
		t.Fatalf("wrong lane counters: %+v", got.Lanes)
	}
}

func TestHotAuthRaiseHasNoReverseProxyQueueAtOldLimit(t *testing.T) {
	ctx, cancel := context.WithCancel(t.Context())
	started := make(chan struct{}, ingressCredentialRequests+1)
	release := make(chan struct{})
	upstream := httptest.NewTLSServer(http.HandlerFunc(func(w http.ResponseWriter, _ *http.Request) {
		started <- struct{}{}
		select {
		case <-release:
			_, _ = io.WriteString(w, "ok")
		case <-ctx.Done():
		}
	}))
	defer upstream.Close()
	handler := newIngress(ctx, testIngressKey, upstream.Listener.Addr().String(), sha256.Sum256(upstream.Certificate().Raw), "", "")
	server := httptest.NewTLSServer(handler)
	defer server.Close()
	defer cancel()
	server.Client().Timeout = 4 * time.Second
	start := func() <-chan ingressLaneReply {
		result := make(chan ingressLaneReply, 1)
		go func() { result <- requestIngressLane(server, http.MethodPost, "/login", false) }()
		return result
	}
	profile := defaultIngressLimits()
	profile.Auth = 1
	handler.limits.replace(profile)
	requests := []<-chan ingressLaneReply{start()}
	waitLaneStarted(t, started)
	for _, path := range []string{"/login", "/user/session/resume", "/user/session/logout"} {
		assertLaneReply(t, requestIngressLane(server, http.MethodPost, path, false), 200, accountBusyReply)
	}
	profile.Auth = ingressCredentialRequests + 1
	handler.limits.replace(profile)
	for range ingressCredentialRequests {
		requests = append(requests, start())
		waitLaneStarted(t, started)
	}
	if readLimitTestMetrics(t, server).Lanes["auth"].InFlight != profile.Auth {
		t.Fatal("raised auth capacity did not reach the pinned upstream")
	}
	close(release)
	for _, request := range requests {
		assertLaneReply(t, <-request, 200, "ok")
	}
}

func TestHotOTPRateDoesNotClearCurrentWindow(t *testing.T) {
	var policy ingressLimitPolicy
	profile := defaultIngressLimits()
	profile.OTPPerMinute = 2
	policy.replace(profile)
	now := time.Unix(1_700_000_000, 0)
	for range 2 {
		if !policy.admit(laneOTP, now) {
			t.Fatal("unused OTP capacity rejected")
		}
		policy.release(laneOTP)
	}
	profile.OTPPerMinute = 0
	policy.replace(profile)
	if policy.admit(laneOTP, now.Add(time.Second)) {
		t.Fatal("zero rate admitted an OTP request")
	}
	profile.OTPPerMinute = 3
	policy.replace(profile)
	if !policy.admit(laneOTP, now.Add(time.Second)) {
		t.Fatal("OTP rate increase did not take effect in the current window")
	}
	policy.release(laneOTP)
	if policy.admit(laneOTP, now.Add(time.Second)) {
		t.Fatal("reload cleared previously spent OTP quota")
	}
}

func writeLimitTestProfile(t *testing.T, path string, profile ingressLimits) {
	t.Helper()
	data, err := json.Marshal(profile)
	if err != nil {
		t.Fatal(err)
	}
	if err := os.WriteFile(path+".next", data, 0o600); err != nil {
		t.Fatal(err)
	}
	if err := os.Rename(path+".next", path); err != nil {
		t.Fatal(err)
	}
}

func readLimitTestMetrics(t *testing.T, server *httptest.Server) ingressMetrics {
	t.Helper()
	reply := requestIngressLane(server, http.MethodGet, ingressMetricsPath, false)
	var metrics ingressMetrics
	if reply.err != nil || reply.status != 200 {
		t.Fatalf("metrics unavailable: %+v", reply)
	}
	if err := json.Unmarshal([]byte(reply.body), &metrics); err != nil {
		t.Fatal(err)
	}
	return metrics
}

func waitLimitTestProfile(t *testing.T, server *httptest.Server, expected ingressLimits) ingressMetrics {
	t.Helper()
	deadline := time.Now().Add(3 * time.Second)
	for {
		got := readLimitTestMetrics(t, server)
		if got.Limits == expected {
			return got
		}
		if time.Now().After(deadline) {
			t.Fatalf("hot profile did not take effect: want %+v, got %+v", expected, got.Limits)
		}
		time.Sleep(15 * time.Millisecond)
	}
}
