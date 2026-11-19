package main

import (
	"bufio"
	"bytes"
	"context"
	"crypto/sha256"
	"crypto/tls"
	"fmt"
	"io"
	"net"
	"net/http"
	"net/http/httptest"
	"strings"
	"sync/atomic"
	"testing"
	"time"
)

const testIngressKey = "only-a-local-fixture-key-for-ingress-tests"

func TestAccountProxyAuthenticatesPinsAndStripsPrivateHeader(t *testing.T) {
	var calls atomic.Int32
	account := httptest.NewTLSServer(http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		calls.Add(1)
		body, err := io.ReadAll(r.Body)
		if err != nil || r.Method != http.MethodPost || r.RequestURI != "/login?client=trial" || string(body) != "key=value" {
			t.Errorf("unexpected upstream account request: %s %s (%v)", r.Method, r.RequestURI, err)
		}
		if r.Header.Get(ingressKeyHeader) != "" {
			t.Error("private ingress capability reached the account backend")
		}
		w.Header().Set("Content-Type", "application/json")
		_, _ = io.WriteString(w, `{"msgBody":{"bizInfo":{"code":"2000"}}}`)
	}))
	defer account.Close()
	address := account.Listener.Addr().String()
	pin := sha256.Sum256(account.Certificate().Raw)
	approved := httptest.NewTLSServer(newIngress(t.Context(), testIngressKey, address, pin, "", ""))
	defer approved.Close()
	for _, item := range []struct {
		keys []string
		code int
	}{
		{nil, 403}, {[]string{"bad"}, 403}, {[]string{testIngressKey, testIngressKey}, 403}, {[]string{testIngressKey}, 200},
	} {
		body := requestAccount(t, approved, item.keys, item.code)
		if item.code == 200 && body != `{"msgBody":{"bizInfo":{"code":"2000"}}}` {
			t.Fatalf("account protocol reply was altered: %q", body)
		}
	}
	if calls.Load() != 1 {
		t.Fatal("failed capability validation opened an upstream request")
	}
	wrongPin := httptest.NewTLSServer(newIngress(t.Context(), testIngressKey, address, [32]byte{}, "", ""))
	defer wrongPin.Close()
	requestAccount(t, wrongPin, []string{testIngressKey}, 502)
	if calls.Load() != 1 {
		t.Fatal("incorrect certificate pin reached the account handler")
	}
}

func requestAccount(t *testing.T, server *httptest.Server, keys []string, expected int) string {
	t.Helper()
	request, err := http.NewRequest(http.MethodPost, server.URL+"/login?client=trial", strings.NewReader("key=value"))
	if err != nil {
		t.Fatal(err)
	}
	for _, key := range keys {
		request.Header.Add(ingressKeyHeader, key)
	}
	response, err := server.Client().Do(request)
	if err != nil {
		t.Fatal(err)
	}
	defer response.Body.Close()
	if response.StatusCode != expected {
		t.Fatalf("got %d, expected %d", response.StatusCode, expected)
	}
	data, err := io.ReadAll(response.Body)
	if err != nil {
		t.Fatal(err)
	}
	return string(data)
}

func TestGameAndBattleUpgradePreserveRawBytesAndHalfClose(t *testing.T) {
	payload := []byte{0x07, 0xaa, 0x00, 0xff, 0x1a, 0x02, 0x0d, 0x0a}
	game := startTestPBBackend(t, "game", payload)
	battle := startTestPBBackend(t, "battle", payload)
	ctx, cancel := context.WithCancel(t.Context())
	defer cancel()
	server := httptest.NewTLSServer(newIngress(ctx, testIngressKey, "", [32]byte{}, game, battle))
	defer server.Close()
	for _, item := range []struct{ target, name string }{{ingressGamePath, "game"}, {ingressBattlePath, "battle"}} {
		t.Run(item.name, func(t *testing.T) {
			conn, reader := connectTestTunnel(t, server, item.target, payload)
			defer conn.Close()
			expected := append([]byte(item.name), payload...)
			got := make([]byte, len(expected))
			if _, err := io.ReadFull(reader, got); err != nil || !bytes.Equal(got, expected) {
				t.Fatalf("upgraded %s bytes mismatch: %x (%v)", item.name, got, err)
			}
			if err := conn.CloseWrite(); err != nil {
				t.Fatal(err)
			}
			tail, err := io.ReadAll(reader)
			if string(tail) != "closed" || err != nil {
				t.Fatalf("upstream final bytes lost after TLS half-close: %q (%v)", tail, err)
			}
		})
	}
}

func startTestPBBackend(t *testing.T, prefix string, payload []byte) string {
	t.Helper()
	listener, err := net.Listen("tcp", "127.0.0.1:0")
	if err != nil {
		t.Fatal(err)
	}
	t.Cleanup(func() { _ = listener.Close() })
	go func() {
		conn, err := listener.Accept()
		if err != nil {
			return
		}
		defer conn.Close()
		_ = conn.SetDeadline(time.Now().Add(5 * time.Second))
		_, _ = io.WriteString(conn, prefix)
		fromClient := make([]byte, len(payload))
		if _, err := io.ReadFull(conn, fromClient); err != nil {
			return
		}
		_, _ = conn.Write(fromClient)
		if _, err := io.Copy(io.Discard, conn); err == nil {
			_, _ = io.WriteString(conn, "closed")
		}
	}()
	return listener.Addr().String()
}

func connectTestTunnel(t *testing.T, server *httptest.Server, path string, pipelinedPayload []byte) (*tls.Conn, *bufio.Reader) {
	t.Helper()
	address := server.Listener.Addr().String()
	config := server.Client().Transport.(*http.Transport).TLSClientConfig.Clone()
	config.MinVersion, config.MaxVersion = tls.VersionTLS12, tls.VersionTLS12
	conn, err := tls.DialWithDialer(&net.Dialer{Timeout: 3 * time.Second}, "tcp", address, config)
	if err != nil {
		t.Fatal(err)
	}
	t.Cleanup(func() { _ = conn.Close() })
	_ = conn.SetDeadline(time.Now().Add(4 * time.Second))
	request := fmt.Sprintf("GET %s HTTP/1.1\r\nHost: %s\r\nX-XZJ-Trial: %s\r\nConnection: keep-alive, Upgrade\r\nUpgrade: xzj-pb\r\n\r\n", path, address, testIngressKey)
	if _, err := conn.Write(append([]byte(request), pipelinedPayload...)); err != nil {
		t.Fatal(err)
	}
	reader := bufio.NewReader(conn)
	response, err := http.ReadResponse(reader, &http.Request{Method: http.MethodGet})
	if err != nil || response.StatusCode != http.StatusSwitchingProtocols ||
		!strings.EqualFold(response.Header.Get("Connection"), "Upgrade") || response.Header.Get("Upgrade") != ingressUpgrade {
		t.Fatalf("invalid upgrade response: %v (%v)", response, err)
	}
	return conn, reader
}

func TestIngressCancellationClosesOpenTunnelAndOTPBudgetRecovers(t *testing.T) {
	listener, err := net.Listen("tcp", "127.0.0.1:0")
	if err != nil {
		t.Fatal(err)
	}
	defer listener.Close()
	ctx, cancel := context.WithCancel(t.Context())
	defer cancel()
	handler := newIngress(ctx, testIngressKey, "", [32]byte{}, listener.Addr().String(), "")
	server := httptest.NewTLSServer(handler)
	defer server.Close()
	_, reader := connectTestTunnel(t, server, ingressGamePath, nil)
	upstream, err := listener.Accept()
	if err != nil {
		t.Fatal(err)
	}
	defer upstream.Close()
	_ = upstream.SetReadDeadline(time.Now().Add(3 * time.Second))
	cancel()
	if _, err := reader.ReadByte(); err != io.EOF {
		t.Fatalf("TLS stream did not close: %v", err)
	}
	if _, err := upstream.Read(make([]byte, 1)); err != io.EOF {
		t.Fatalf("loopback stream did not close: %v", err)
	}
	now := time.Unix(1_700_000_000, 0)
	for range ingressOTPPerMin {
		if !handler.limits.admit(laneOTP, now) {
			t.Fatal("unused OTP budget rejected")
		}
		handler.limits.release(laneOTP)
	}
	if handler.limits.admit(laneOTP, now.Add(time.Second)) || !handler.limits.admit(laneOTP, now.Add(time.Minute)) {
		t.Fatal("OTP request budget did not expire as expected")
	}
	handler.limits.release(laneOTP)
}
