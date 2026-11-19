package main

import (
	"bytes"
	"context"
	"crypto/tls"
	"errors"
	"fmt"
	"mime"
	"mime/quotedprintable"
	"net"
	"net/mail"
	"net/smtp"
	"os"
	"strconv"
	"strings"
	"time"
)

type registrationSMTP struct {
	host, port, username, password string
	from                           mail.Address
	startTLS                       bool
	tlsConfig                      *tls.Config
}

func registrationSMTPSenderFromEnv() (func(context.Context, string, string) error, error) {
	host := strings.TrimSpace(os.Getenv("XZJ_SMTP_HOST"))
	if host == "" {
		return nil, nil
	}
	port := strings.TrimSpace(os.Getenv("XZJ_SMTP_PORT"))
	if port == "" {
		port = "465"
	}
	n, err := strconv.Atoi(port)
	if err != nil || n < 1 || n > 65535 {
		return nil, errors.New("XZJ_SMTP_PORT must be a valid TCP port")
	}
	mode := strings.ToLower(strings.TrimSpace(os.Getenv("XZJ_SMTP_SECURITY")))
	if mode == "" {
		mode = "tls"
		if n == 587 {
			mode = "starttls"
		}
	}
	if mode != "tls" && mode != "starttls" {
		return nil, errors.New("XZJ_SMTP_SECURITY must be tls or starttls")
	}
	username := os.Getenv("XZJ_SMTP_USERNAME")
	password := os.Getenv("XZJ_SMTP_PASSWORD")
	if (username == "") != (password == "") {
		return nil, errors.New("XZJ_SMTP_USERNAME and XZJ_SMTP_PASSWORD must be configured together")
	}
	from, err := mail.ParseAddress(os.Getenv("XZJ_SMTP_FROM"))
	if err != nil {
		return nil, errors.New("XZJ_SMTP_FROM must be a valid sender address")
	}
	if from.Name == "" {
		from.Name = "修真界"
	}
	sender := registrationSMTP{
		host: host, port: port, from: *from, username: username, password: password,
		startTLS:  mode == "starttls",
		tlsConfig: &tls.Config{ServerName: host, MinVersion: tls.VersionTLS12},
	}
	return sender.send, nil
}

func (s registrationSMTP) send(ctx context.Context, recipient, code string) error {
	to, err := mail.ParseAddress(recipient)
	if err != nil || to.Address != recipient || len(code) != 6 || strings.Trim(code, "0123456789") != "" {
		return errors.New("invalid registration email")
	}
	dialer := net.Dialer{Timeout: 8 * time.Second}
	conn, err := dialer.DialContext(ctx, "tcp", net.JoinHostPort(s.host, s.port))
	if err != nil {
		return err
	}
	defer conn.Close()
	stop := context.AfterFunc(ctx, func() { _ = conn.Close() })
	defer stop()
	deadline := time.Now().Add(8 * time.Second)
	if limit, ok := ctx.Deadline(); ok && limit.Before(deadline) {
		deadline = limit
	}
	if err := conn.SetDeadline(deadline); err != nil {
		return err
	}
	var transport net.Conn = conn
	if !s.startTLS {
		secure := tls.Client(conn, s.tlsConfig)
		if err := secure.HandshakeContext(ctx); err != nil {
			return err
		}
		transport = secure
	}
	client, err := smtp.NewClient(transport, s.host)
	if err != nil {
		return err
	}
	defer client.Close()
	if s.startTLS {
		if err := client.StartTLS(s.tlsConfig); err != nil {
			return err
		}
	}
	if s.username != "" {
		if err := client.Auth(smtp.PlainAuth("", s.username, s.password, s.host)); err != nil {
			return err
		}
	}
	if err := client.Mail(s.from.Address); err != nil {
		return err
	}
	if err := client.Rcpt(to.Address); err != nil {
		return err
	}
	data, err := client.Data()
	if err != nil {
		return err
	}
	if _, err := data.Write(registrationEmailMessage(s.from, *to, code)); err != nil {
		return err
	}
	if err := data.Close(); err != nil {
		return err
	}
	// The DATA response confirmed the relay accepted the message.
	_ = client.Quit()
	return nil
}

func registrationEmailMessage(from, to mail.Address, code string) []byte {
	var message bytes.Buffer
	fmt.Fprintf(&message, "From: %s\r\nTo: %s\r\nDate: %s\r\n", from.String(), to.String(), time.Now().Format(time.RFC1123Z))
	fmt.Fprintf(&message, "Subject: %s\r\n", mime.QEncoding.Encode("UTF-8", "修真界注册验证码"))
	message.WriteString("MIME-Version: 1.0\r\nContent-Type: text/plain; charset=utf-8\r\nContent-Transfer-Encoding: quoted-printable\r\n\r\n")
	body := quotedprintable.NewWriter(&message)
	fmt.Fprintf(body, "你的注册验证码是 %s，有效期 10 分钟。\r\n", code)
	_ = body.Close()
	return message.Bytes()
}
