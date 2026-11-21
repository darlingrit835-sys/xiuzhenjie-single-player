// apk-edge serves the APK page and ACME HTTP challenges on the public port.
package main

import (
	"bufio"
	"crypto/tls"
	"flag"
	"io"
	"log"
	"net"
	"net/http"
	"net/http/httputil"
	"net/url"
	"os"
	"path/filepath"
	"strings"
	"time"
)

func main() {
	address := flag.String("listen", ":80", "public TCP listener")
	publicURL := flag.String("public-url", "", "fixed HTTPS URL")
	pemFile := flag.String("pem", "", "full chain and private key in one PEM file")
	webroot := flag.String("webroot", "", "ACME webroot directory")
	originAddress := flag.String("origin", "127.0.0.1:18193", "loopback APK share listener")
	flag.Parse()
	if host, _, err := net.SplitHostPort(*originAddress); err != nil || host != "127.0.0.1" {
		log.Fatal("origin must use 127.0.0.1")
	}

	getCertificate := func(*tls.ClientHelloInfo) (*tls.Certificate, error) {
		pem, err := os.ReadFile(*pemFile)
		if err != nil {
			return nil, err
		}
		cert, err := tls.X509KeyPair(pem, pem)
		return &cert, err
	}
	if _, err := getCertificate(nil); err != nil {
		log.Fatal(err)
	}

	listener, err := net.Listen("tcp4", *address)
	if err != nil {
		log.Fatal(err)
	}
	plain, secure := makeListener(listener.Addr()), makeListener(listener.Addr())
	acme := http.NewServeMux()
	acme.HandleFunc("GET /.well-known/acme-challenge/{token}", func(w http.ResponseWriter, r *http.Request) {
		token := r.PathValue("token")
		if len(token) == 0 || len(token) > 128 || strings.Trim(token, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-_") != "" {
			http.NotFound(w, r)
			return
		}
		challenge, err := os.Open(filepath.Join(*webroot, ".well-known", "acme-challenge", token))
		if err != nil {
			http.NotFound(w, r)
			return
		}
		defer challenge.Close()
		w.Header().Set("Content-Type", "text/plain; charset=utf-8")
		_, _ = io.Copy(w, io.LimitReader(challenge, 2048))
	})
	acme.HandleFunc("GET /", func(w http.ResponseWriter, r *http.Request) {
		http.Redirect(w, r, *publicURL, http.StatusTemporaryRedirect)
	})

	origin := &url.URL{Scheme: "http", Host: *originAddress}
	proxy := &httputil.ReverseProxy{
		Rewrite: func(r *httputil.ProxyRequest) {
			r.SetURL(origin)
			r.SetXForwarded()
		},
	}
	public := &http.Server{
		Handler:           proxy,
		TLSConfig:         &tls.Config{MinVersion: tls.VersionTLS12, GetCertificate: getCertificate},
		ReadHeaderTimeout: 8 * time.Second,
		ReadTimeout:       20 * time.Second,
		WriteTimeout:      2 * time.Hour,
		IdleTimeout:       time.Minute,
		MaxHeaderBytes:    32 << 10,
	}
	challengeServer := &http.Server{
		Handler: acme, ReadHeaderTimeout: 5 * time.Second, WriteTimeout: 10 * time.Second, IdleTimeout: 15 * time.Second,
	}
	go func() { log.Fatal(challengeServer.Serve(plain)) }()
	go func() { log.Fatal(public.ServeTLS(secure, "", "")) }()

	// ACME must reach plain HTTP on port 80. TLS uses the same port without touching the game's listener.
	probes := make(chan struct{}, 128)
	for {
		conn, err := listener.Accept()
		if err != nil {
			log.Fatal(err)
		}
		select {
		case probes <- struct{}{}:
			go func() {
				defer func() { <-probes }()
				dispatch(conn, plain, secure)
			}()
		default:
			conn.Close()
		}
	}
}

type queuedListener struct {
	address     net.Addr
	connections chan net.Conn
}

func makeListener(address net.Addr) *queuedListener {
	return &queuedListener{address: address, connections: make(chan net.Conn)}
}

func (l *queuedListener) Accept() (net.Conn, error) { return <-l.connections, nil }
func (l *queuedListener) Addr() net.Addr            { return l.address }
func (l *queuedListener) Close() error              { return nil }

type bufferedConn struct {
	net.Conn
	reader *bufio.Reader
}

func (c *bufferedConn) Read(data []byte) (int, error) { return c.reader.Read(data) }

func dispatch(conn net.Conn, plain, secure *queuedListener) {
	_ = conn.SetReadDeadline(time.Now().Add(5 * time.Second))
	reader := bufio.NewReaderSize(conn, 1024)
	first, err := reader.Peek(1)
	if err != nil {
		conn.Close()
		return
	}
	_ = conn.SetReadDeadline(time.Time{})
	target := plain
	if first[0] == 0x16 {
		target = secure
	}
	select {
	case target.connections <- &bufferedConn{Conn: conn, reader: reader}:
	case <-time.After(time.Second):
		conn.Close()
	}
}
