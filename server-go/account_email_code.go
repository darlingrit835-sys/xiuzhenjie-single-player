package main

import (
	"context"
	"crypto/hmac"
	"crypto/rand"
	"crypto/sha256"
	"errors"
	"fmt"
	"log"
	"math/big"
	"net/http"
	"sync"
	"time"
)

const (
	emailCodeLifetime     = 10 * time.Minute
	emailCodeResendWait   = time.Minute
	emailCodeSendTimeout  = 8 * time.Second
	emailCodeMaxAttempts  = 5
	emailCodePendingLimit = 1024
)

var errEmailCodeThrottled = errors.New("email code request throttled")

type emailCodeChallenge struct {
	digest      [sha256.Size]byte
	expiresAt   time.Time
	resendAfter time.Time
	attempts    int
	sending     bool
	claimed     bool
}

// Challenges are process-local. A restart invalidates outstanding codes and
// the key never accompanies the persisted account data.
type emailCodeStore struct {
	mu      sync.Mutex
	key     [sha256.Size]byte
	pending map[string]*emailCodeChallenge
}

func (s *emailCodeStore) digest(email, code string) [sha256.Size]byte {
	mac := hmac.New(sha256.New, s.key[:])
	_, _ = mac.Write([]byte(email + "\x00" + code))
	var result [sha256.Size]byte
	mac.Sum(result[:0])
	return result
}

func (s *emailCodeStore) beginSend(email string, now time.Time) (*emailCodeChallenge, string, error) {
	s.mu.Lock()
	defer s.mu.Unlock()
	for key, challenge := range s.pending {
		if !challenge.sending && !challenge.claimed && !now.Before(challenge.expiresAt) && !now.Before(challenge.resendAfter) {
			delete(s.pending, key)
		}
	}
	challenge := s.pending[email]
	if challenge == nil && len(s.pending) >= emailCodePendingLimit ||
		challenge != nil && (challenge.sending || challenge.claimed || now.Before(challenge.resendAfter)) {
		return nil, "", errEmailCodeThrottled
	}
	if s.pending == nil {
		if _, err := rand.Read(s.key[:]); err != nil {
			return nil, "", err
		}
		s.pending = make(map[string]*emailCodeChallenge)
	}
	if challenge == nil {
		challenge = &emailCodeChallenge{}
	}
	for {
		number, err := rand.Int(rand.Reader, big.NewInt(1_000_000))
		if err != nil {
			return nil, "", err
		}
		code := fmt.Sprintf("%06d", number.Int64())
		if s.digest(email, code) == challenge.digest {
			continue
		}
		challenge.sending = true
		challenge.resendAfter = now.Add(emailCodeResendWait)
		s.pending[email] = challenge
		return challenge, code, nil
	}
}

func (s *emailCodeStore) finishSend(email string, challenge *emailCodeChallenge, code string, now time.Time, sent bool) {
	s.mu.Lock()
	defer s.mu.Unlock()
	challenge.sending = false
	if sent {
		challenge.digest = s.digest(email, code)
		challenge.expiresAt = now.Add(emailCodeLifetime)
		challenge.resendAfter = now.Add(emailCodeResendWait)
		challenge.attempts = 0
	}
}

func (s *emailCodeStore) claim(email, code string, now time.Time) *emailCodeChallenge {
	s.mu.Lock()
	defer s.mu.Unlock()
	challenge := s.pending[email]
	if challenge == nil || challenge.sending || challenge.claimed || !now.Before(challenge.expiresAt) || challenge.attempts >= emailCodeMaxAttempts {
		return nil
	}
	digest := s.digest(email, code)
	if !hmac.Equal(challenge.digest[:], digest[:]) {
		challenge.attempts++
		return nil
	}
	challenge.claimed = true
	return challenge
}

func (s *emailCodeStore) finishClaim(email string, challenge *emailCodeChallenge, registered bool) {
	s.mu.Lock()
	defer s.mu.Unlock()
	if registered {
		delete(s.pending, email)
	} else {
		challenge.claimed = false
	}
}

func validRegistrationEmailCode(code string) bool {
	if len(code) != 6 {
		return false
	}
	for _, digit := range code {
		if digit < '0' || digit > '9' {
			return false
		}
	}
	return true
}

func (s *Server) accountSendEmailCode(w http.ResponseWriter, r *http.Request) error {
	if !s.emailRegistration {
		http.NotFound(w, r)
		return nil
	}
	if s.sendRegistrationEmail == nil {
		writeAccount(w, "4006", "邮箱验证码服务暂不可用", "")
		return nil
	}
	form, err := parseEmailAccountForm(w, r)
	if err != nil {
		writeAccount(w, "5001", "邮箱验证码请求无效", "")
		return nil
	}
	email, valid := normalizeAccountEmail(form.Get("email"))
	if !valid || form.Has("mobilePhone") {
		writeAccount(w, "5001", "请输入有效的邮箱地址", "")
		return nil
	}
	if existing := s.findEmailAccount(email, ""); existing != nil && !emailAccountNeedsVerification(existing) {
		writeAccount(w, "2201", "邮箱已注册", "")
		return nil
	}
	challenge, code, err := s.emailCodes.beginSend(email, time.Now())
	if errors.Is(err, errEmailCodeThrottled) {
		writeAccount(w, "429", "发送请求过于频繁，请稍后再试", "")
		return nil
	}
	if err != nil {
		return err
	}
	ctx, cancel := context.WithTimeout(r.Context(), emailCodeSendTimeout)
	defer cancel()
	err = s.sendRegistrationEmail(ctx, email, code)
	sent := err == nil && ctx.Err() == nil
	s.emailCodes.finishSend(email, challenge, code, time.Now(), sent)
	if !sent {
		log.Print("registration email delivery failed")
		writeAccount(w, "4006", "验证码邮件发送失败，请稍后再试", "")
		return nil
	}
	writeAccount(w, "2000", "验证码已发送，请查收邮箱", "true")
	return nil
}
