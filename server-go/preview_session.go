package main

import (
	"context"
	"crypto/hmac"
	"crypto/rand"
	"crypto/sha256"
	"encoding/base64"
	"log"
	"strconv"
	"strings"
	"time"
)

const previewSessionDuration = 12 * time.Hour

func (s *Server) enablePreviewWrites() error {
	guilds, err := OpenStore(s.dataDir, "guilds")
	if err != nil {
		return err
	}
	if err := s.initAccountSessions(); err != nil {
		return err
	}
	s.guilds = guilds
	s.previewWrites = true
	return nil
}

func (s *Server) accountSessionsRequired() bool {
	return s.previewWrites || s.emailRegistration
}

func (s *Server) initAccountSessions() error {
	_, err := rand.Read(s.previewSessionKey[:])
	return err
}

func (s *Server) previewSessionSignature(expires, userID string) []byte {
	mac := hmac.New(sha256.New, s.previewSessionKey[:])
	_, _ = mac.Write([]byte(expires + "." + userID))
	return mac.Sum(nil)
}

// The original client echoes this HTTPS server field in the PB login, including on reconnect.
func (s *Server) issuePreviewSession(userID string, now time.Time) string {
	expires := strconv.FormatInt(now.Add(previewSessionDuration).Unix(), 10)
	return expires + "." + base64.RawURLEncoding.EncodeToString(s.previewSessionSignature(expires, userID))
}

func (s *Server) verifyPreviewSession(token, userID string, now time.Time) bool {
	if userID == "" || len(token) > 64 {
		return false
	}
	expires, encoded, ok := strings.Cut(token, ".")
	deadline, err := strconv.ParseInt(expires, 10, 64)
	if !ok || err != nil || deadline <= now.Unix() {
		return false
	}
	signature, err := base64.RawURLEncoding.DecodeString(encoded)
	return err == nil && hmac.Equal(signature, s.previewSessionSignature(expires, userID))
}

func (s *Server) advancePreviewAuctions(ctx context.Context) {
	ticker := time.NewTicker(time.Second)
	defer ticker.Stop()
	for {
		select {
		case <-ctx.Done():
			return
		case now := <-ticker.C:
			if err := s.settlePreviewAuctions(now); err != nil {
				log.Printf("拍卖结算失败，将重试: %v", err)
			}
		}
	}
}
