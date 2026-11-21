package main

import (
	"context"
	"crypto/sha256"
	"crypto/subtle"
	"crypto/tls"
	"encoding/json"
	"errors"
	"io"
	"log"
	"net"
	"net/http"
	"net/http/httputil"
	"net/url"
	"strings"
	"sync"
	"time"
)

const (
	ingressKeyHeader          = "X-XZJ-Trial"
	ingressUpgrade            = "xzj-pb"
	ingressGamePath           = "/_xzj/game"
	ingressBattlePath         = "/_xzj/battle"
	ingressMetricsPath        = "/_xzj/metrics"
	ingressOTPPath            = "/user/sendEmailCode"
	ingressGameTunnels        = 64
	ingressBattleTunnels      = 64
	ingressGeneralRequests    = 16
	ingressCredentialRequests = 4
	ingressOTPRequests        = 2
	ingressOTPPerMin          = 24
	ingressOpenTimeout        = 5 * time.Second

	accountBusyReply = `{"msgBody":{"sysInfo":{"httpCode":200},"bizInfo":{"code":"429","msg":"服务器繁忙，请稍后重试","dataString":""}}}`
	otpBusyReply     = `{"msgBody":{"sysInfo":{"httpCode":200},"bizInfo":{"code":"429","msg":"验证码请求过多，请稍后重试","dataString":""}}}`
)

type ingress struct {
	ctx           context.Context
	keyHash       [32]byte
	account       [laneGame]http.Handler
	game, battle  string
	dialer        net.Dialer
	limits        ingressLimitPolicy
	hotUpdate     *hotUpdateChannel
	fullHotUpdate *hotUpdateChannel
}

func newIngress(ctx context.Context, key, account string, accountPin [32]byte, game, battle string) *ingress {
	handler := &ingress{
		ctx: ctx, keyHash: sha256.Sum256([]byte(key)),
		account: [laneGame]http.Handler{
			laneAccount: newAccountProxy(account, accountPin),
			laneAuth:    newAccountProxy(account, accountPin),
			laneOTP:     newAccountProxy(account, accountPin),
		},
		game: game, battle: battle,
		dialer: net.Dialer{Timeout: ingressOpenTimeout, KeepAlive: 30 * time.Second},
	}
	handler.limits.replace(defaultIngressLimits())
	return handler
}

func newAccountProxy(account string, accountPin [32]byte) *httputil.ReverseProxy {
	return &httputil.ReverseProxy{
		Rewrite: func(request *httputil.ProxyRequest) {
			request.SetURL(&url.URL{Scheme: "https", Host: account})
			request.Out.Header.Del(ingressKeyHeader)
			request.Out.Trailer.Del(ingressKeyHeader)
		},
		Transport: &http.Transport{
			DialContext:           (&net.Dialer{Timeout: ingressOpenTimeout, KeepAlive: 30 * time.Second}).DialContext,
			TLSHandshakeTimeout:   ingressOpenTimeout,
			ResponseHeaderTimeout: 20 * time.Second,
			IdleConnTimeout:       time.Minute,
			// Admission bounds active connections. A transport cap would queue
			// already-admitted requests after a hot increase.
			MaxIdleConnsPerHost: 8,
			TLSClientConfig: &tls.Config{
				MinVersion: tls.VersionTLS12,
				// The private upstream uses an exact leaf pin in place of DNS CA validation.
				InsecureSkipVerify: true,
				VerifyConnection: func(state tls.ConnectionState) error {
					if len(state.PeerCertificates) == 0 {
						return errors.New("missing account certificate")
					}
					leaf := state.PeerCertificates[0]
					fingerprint := sha256.Sum256(leaf.Raw)
					if subtle.ConstantTimeCompare(accountPin[:], fingerprint[:]) != 1 {
						return errors.New("account certificate pin mismatch")
					}
					now := time.Now()
					if now.Before(leaf.NotBefore) || now.After(leaf.NotAfter) {
						return errors.New("account certificate outside its validity period")
					}
					return nil
				},
			},
		},
		ErrorHandler: func(w http.ResponseWriter, _ *http.Request, _ error) {
			log.Print("account upstream request failed")
			http.Error(w, "account service unavailable", http.StatusBadGateway)
		},
	}
}

func (s *ingress) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	if r.TLS != nil {
		if s.hotUpdate != nil && s.hotUpdate.serve(w, r) {
			return
		}
		if s.fullHotUpdate != nil && s.fullHotUpdate.serve(w, r) {
			return
		}
	}
	keys := r.Header.Values(ingressKeyHeader)
	if r.TLS == nil || len(keys) != 1 {
		http.Error(w, "forbidden", http.StatusForbidden)
		return
	}
	digest := sha256.Sum256([]byte(keys[0]))
	if subtle.ConstantTimeCompare(s.keyHash[:], digest[:]) != 1 {
		http.Error(w, "forbidden", http.StatusForbidden)
		return
	}
	switch r.URL.Path {
	case ingressGamePath:
		s.serveTunnel(w, r, laneGame, s.game)
	case ingressBattlePath:
		s.serveTunnel(w, r, laneBattle, s.battle)
	case ingressMetricsPath:
		s.serveMetrics(w, r)
	default:
		if r.Method == http.MethodConnect || len(r.Header.Values("Upgrade")) != 0 {
			http.Error(w, "unsupported upgrade", http.StatusBadRequest)
			return
		}
		s.serveAccount(w, r)
	}
}

func (s *ingress) serveMetrics(w http.ResponseWriter, r *http.Request) {
	if r.Method != http.MethodGet {
		w.Header().Set("Allow", http.MethodGet)
		http.Error(w, "method not allowed", http.StatusMethodNotAllowed)
		return
	}
	w.Header().Set("Cache-Control", "no-store")
	w.Header().Set("Content-Type", "application/json; charset=utf-8")
	_ = json.NewEncoder(w).Encode(s.limits.metrics())
}

func (s *ingress) serveAccount(w http.ResponseWriter, r *http.Request) {
	lane, busyReply := accountRequestLane(r)
	if !s.limits.admit(lane, time.Now()) {
		writeAccountBusy(w, busyReply)
		return
	}
	defer s.limits.release(lane)
	s.account[lane].ServeHTTP(w, r)
}

func accountRequestLane(r *http.Request) (ingressLane, string) {
	if r.Method == http.MethodPost {
		// Match the account server's prefix routing, including its earlier CD-key route.
		path := r.URL.Path
		switch {
		case strings.HasPrefix(path, ingressOTPPath):
			return laneOTP, otpBusyReply
		case strings.HasPrefix(path, "/login/roleCdKey/cdkeyExchange"):
			return laneAccount, accountBusyReply
		case strings.HasPrefix(path, "/login"), strings.HasPrefix(path, "/user/register"),
			path == "/user/session/resume", path == "/user/session/logout":
			return laneAuth, accountBusyReply
		}
	}
	return laneAccount, accountBusyReply
}

func writeAccountBusy(w http.ResponseWriter, body string) {
	w.Header().Set("Content-Type", "application/json; charset=utf-8")
	_, _ = io.WriteString(w, body)
}

func validTunnelRequest(r *http.Request) bool {
	upgrade := r.Header.Values("Upgrade")
	if r.Method != http.MethodGet || r.ProtoMajor != 1 || r.ProtoMinor != 1 || r.ContentLength != 0 ||
		len(r.TransferEncoding) != 0 || r.URL.RawQuery != "" ||
		len(upgrade) != 1 || !strings.EqualFold(upgrade[0], ingressUpgrade) {
		return false
	}
	for _, value := range r.Header.Values("Connection") {
		for _, token := range strings.Split(value, ",") {
			if strings.EqualFold(strings.TrimSpace(token), "upgrade") {
				return true
			}
		}
	}
	return false
}

func (s *ingress) serveTunnel(w http.ResponseWriter, r *http.Request, lane ingressLane, target string) {
	if !validTunnelRequest(r) {
		http.Error(w, "expected xzj-pb upgrade", http.StatusBadRequest)
		return
	}
	if !s.limits.admit(lane, time.Now()) {
		http.Error(w, "tunnel capacity unavailable", http.StatusServiceUnavailable)
		return
	}
	defer s.limits.release(lane)
	upstream, err := s.dialer.DialContext(r.Context(), "tcp", target)
	if err != nil {
		http.Error(w, "game service unavailable", http.StatusBadGateway)
		return
	}
	defer upstream.Close()
	client, buffered, err := http.NewResponseController(w).Hijack()
	if err != nil {
		http.Error(w, "cannot open tunnel", http.StatusInternalServerError)
		return
	}
	defer client.Close()
	closeOnShutdown := context.AfterFunc(s.ctx, func() {
		_ = client.Close()
		_ = upstream.Close()
	})
	defer closeOnShutdown()
	if err := client.SetDeadline(time.Now().Add(ingressOpenTimeout)); err != nil {
		return
	}
	if _, err := buffered.WriteString("HTTP/1.1 101 Switching Protocols\r\nConnection: Upgrade\r\nUpgrade: xzj-pb\r\n\r\n"); err != nil {
		return
	}
	if err := buffered.Flush(); err != nil {
		return
	}
	if err := client.SetDeadline(time.Time{}); err != nil {
		return
	}
	bridgeTunnel(client, buffered.Reader, upstream)
}

func bridgeTunnel(client net.Conn, fromClient io.Reader, upstream net.Conn) {
	var uploading sync.WaitGroup
	uploading.Go(func() {
		if _, err := io.Copy(upstream, fromClient); err == nil {
			if tcp, ok := upstream.(*net.TCPConn); ok && tcp.CloseWrite() == nil {
				return
			}
		}
		_ = upstream.Close()
		_ = client.Close()
	})
	_, _ = io.Copy(client, upstream)
	_ = upstream.Close()
	_ = client.Close()
	uploading.Wait()
}
