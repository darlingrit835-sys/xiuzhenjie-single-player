package main

import (
	"bufio"
	"bytes"
	"context"
	"crypto/tls"
	"crypto/x509"
	"fmt"
	"io"
	"mime/quotedprintable"
	"net"
	"net/http"
	"net/http/httptest"
	"net/mail"
	"net/textproto"
	"strings"
	"testing"
	"time"
)

func TestRegistrationSMTPSendsCodeOverVerifiedTLS(t *testing.T) {
	web := httptest.NewTLSServer(http.HandlerFunc(func(http.ResponseWriter, *http.Request) {}))
	defer web.Close()
	certs := x509.NewCertPool()
	certs.AddCert(web.Certificate())
	tcp, err := net.Listen("tcp", "127.0.0.1:0")
	if err != nil {
		t.Fatal(err)
	}
	listener := tls.NewListener(tcp, &tls.Config{Certificates: web.TLS.Certificates, MinVersion: tls.VersionTLS12})
	defer listener.Close()
	host, port, err := net.SplitHostPort(listener.Addr().String())
	if err != nil {
		t.Fatal(err)
	}
	var received bytes.Buffer
	finished := make(chan error, 1)
	go func() {
		finished <- receiveRegistrationSMTP(listener, &received)
	}()
	sender := registrationSMTP{
		host: host, port: port,
		username: "test-sender", password: "test-only-credential",
		from:      mail.Address{Name: "修真界", Address: "sender@example.test"},
		tlsConfig: &tls.Config{ServerName: host, RootCAs: certs, MinVersion: tls.VersionTLS12},
	}
	ctx, cancel := context.WithTimeout(context.Background(), 4*time.Second)
	defer cancel()
	if err := sender.send(ctx, "player@example.test", "019204"); err != nil {
		t.Fatal(err)
	}
	if err := <-finished; err != nil {
		t.Fatal(err)
	}
	message, err := mail.ReadMessage(&received)
	if err != nil {
		t.Fatal(err)
	}
	body, err := io.ReadAll(quotedprintable.NewReader(message.Body))
	if err != nil || !bytes.Contains(body, []byte("019204")) || message.Header.Get("To") != "<player@example.test>" {
		t.Fatal("relay did not receive the expected recipient and verification code")
	}
}

func receiveRegistrationSMTP(listener net.Listener, received io.Writer) error {
	conn, err := listener.Accept()
	if err != nil {
		return err
	}
	defer conn.Close()
	if err := conn.SetDeadline(time.Now().Add(4 * time.Second)); err != nil {
		return err
	}
	reader := textproto.NewReader(bufio.NewReader(conn))
	if _, err := io.WriteString(conn, "220 test SMTP\r\n"); err != nil {
		return err
	}
	for _, step := range []struct{ command, reply string }{
		{"EHLO ", "250-test SMTP\r\n250 AUTH PLAIN\r\n"},
		{"AUTH PLAIN ", "235 authenticated\r\n"},
		{"MAIL FROM:<sender@example.test>", "250 OK\r\n"},
		{"RCPT TO:<player@example.test>", "250 OK\r\n"},
		{"DATA", "354 send message\r\n"},
	} {
		line, err := reader.ReadLine()
		if err != nil || !strings.HasPrefix(line, step.command) {
			return fmt.Errorf("expected SMTP %s: %v", step.command, err)
		}
		if _, err := io.WriteString(conn, step.reply); err != nil {
			return err
		}
	}
	if _, err := io.Copy(received, reader.DotReader()); err != nil {
		return err
	}
	if _, err := io.WriteString(conn, "250 queued\r\n"); err != nil {
		return err
	}
	line, err := reader.ReadLine()
	if err != nil || line != "QUIT" {
		return fmt.Errorf("expected SMTP quit: %v", err)
	}
	_, err = io.WriteString(conn, "221 bye\r\n")
	return err
}
