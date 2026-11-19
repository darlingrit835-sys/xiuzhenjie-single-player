package main

import (
	"context"
	"crypto/tls"
	"encoding/hex"
	"errors"
	"flag"
	"fmt"
	"log"
	"net"
	"net/http"
	"os"
	"os/signal"
	"strconv"
	"strings"
	"syscall"
	"time"

	"software.sslmate.com/src/go-pkcs12"

	"xiuzhenjie/server/internal/opslog"
)

func main() {
	listen := flag.String("listen", ":35443", "HTTPS listener address")
	p12 := flag.String("cert-p12", "", "TLS PKCS#12 certificate file")
	account := flag.String("account", "127.0.0.1:34443", "loopback account HTTPS address")
	game := flag.String("game", "127.0.0.1:34666", "loopback game TCP address")
	battle := flag.String("battle", "127.0.0.1:34667", "loopback battle TCP address")
	fingerprint := flag.String("account-sha256", "", "SHA-256 fingerprint of the account HTTPS leaf certificate")
	limitsFile := flag.String("limits-file", "", "optional JSON limits profile reloaded every two seconds")
	logDir := flag.String("log-dir", "", "directory for rotated ingress logs")
	flag.Parse()

	if *logDir != "" {
		writer, err := opslog.Open(*logDir, "ingress")
		if err != nil {
			log.Fatal(err)
		}
		defer writer.Close()
		log.SetOutput(writer)
	}
	log.SetFlags(log.LstdFlags | log.LUTC | log.Lmicroseconds)

	key := os.Getenv("XZJ_INGRESS_KEY")
	if err := validateIngressKey(key); err != nil {
		log.Fatal(err)
	}
	for _, addr := range []string{*account, *game, *battle} {
		if err := validateLoopbackAddress(addr); err != nil {
			log.Fatal(err)
		}
	}
	pin, err := parseAccountFingerprint(*fingerprint)
	if err != nil {
		log.Fatal(err)
	}
	cert, err := readIngressCertificate(*p12, os.Getenv("XZJ_TLS_PASSWORD"))
	if err != nil {
		log.Fatal(err)
	}
	ctx, stop := signal.NotifyContext(context.Background(), os.Interrupt, syscall.SIGTERM)
	defer stop()
	handler := newIngress(ctx, key, *account, pin, *game, *battle)
	if *limitsFile != "" {
		profile, err := readIngressLimits(*limitsFile)
		if err != nil {
			log.Fatalf("load ingress limits: %v", err)
		}
		handler.limits.replace(profile)
		go handler.limits.watch(ctx, *limitsFile, ingressLimitPoll)
	}
	server := &http.Server{
		Handler:           handler,
		ReadHeaderTimeout: 5 * time.Second,
		ReadTimeout:       15 * time.Second,
		WriteTimeout:      30 * time.Second,
		IdleTimeout:       time.Minute,
		MaxHeaderBytes:    16 << 10,
		Protocols:         new(http.Protocols),
		TLSConfig: &tls.Config{
			Certificates: []tls.Certificate{cert},
			MinVersion:   tls.VersionTLS12,
			NextProtos:   []string{"http/1.1"},
		},
	}
	server.Protocols.SetHTTP1(true)
	listener, err := net.Listen("tcp", *listen)
	if err != nil {
		log.Fatal(err)
	}
	defer listener.Close()
	stopped := make(chan struct{})
	go func() {
		defer close(stopped)
		<-ctx.Done()
		shutdown, cancel := context.WithTimeout(context.Background(), 5*time.Second)
		defer cancel()
		if err := server.Shutdown(shutdown); err != nil {
			_ = server.Close()
		}
	}()
	log.Printf("READY HTTPS ingress on %s", listener.Addr())
	err = server.ServeTLS(listener, "", "")
	stop()
	<-stopped
	if err != nil && !errors.Is(err, http.ErrServerClosed) {
		log.Fatal(err)
	}
}

func validateIngressKey(key string) error {
	if len(key) < 32 || len(key) > 256 {
		return errors.New("XZJ_INGRESS_KEY must contain 32–256 visible ASCII bytes")
	}
	for _, ch := range key {
		if ch <= ' ' || ch > '~' {
			return errors.New("XZJ_INGRESS_KEY must contain 32–256 visible ASCII bytes")
		}
	}
	return nil
}

func validateLoopbackAddress(address string) error {
	host, port, err := net.SplitHostPort(address)
	number, portErr := strconv.Atoi(port)
	if err != nil || !net.ParseIP(host).IsLoopback() || portErr != nil || number <= 0 || number > 65535 {
		return fmt.Errorf("upstream must have a numeric loopback IP and valid port: %q", address)
	}
	return nil
}

func parseAccountFingerprint(value string) ([32]byte, error) {
	var pin [32]byte
	decoded, err := hex.DecodeString(strings.ReplaceAll(strings.TrimSpace(value), ":", ""))
	if err != nil || len(decoded) != len(pin) {
		return pin, errors.New("-account-sha256 must contain the account certificate's 32-byte SHA-256 fingerprint in hexadecimal")
	}
	copy(pin[:], decoded)
	return pin, nil
}

func readIngressCertificate(path, password string) (tls.Certificate, error) {
	if path == "" || password == "" {
		return tls.Certificate{}, errors.New("-cert-p12 and XZJ_TLS_PASSWORD are required")
	}
	data, err := os.ReadFile(path)
	if err != nil {
		return tls.Certificate{}, err
	}
	key, leaf, chain, err := pkcs12.DecodeChain(data, password)
	if err != nil {
		return tls.Certificate{}, errors.New("could not decode the ingress PKCS#12 certificate")
	}
	certificate := tls.Certificate{Certificate: [][]byte{leaf.Raw}, PrivateKey: key, Leaf: leaf}
	for _, intermediate := range chain {
		certificate.Certificate = append(certificate.Certificate, intermediate.Raw)
	}
	return certificate, nil
}
