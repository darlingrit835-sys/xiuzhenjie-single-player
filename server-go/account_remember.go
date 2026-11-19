package main

import (
	"crypto/rand"
	"crypto/sha256"
	"encoding/base64"
	"encoding/hex"
	"errors"
	"fmt"
	"net/http"
	"time"
)

const (
	accountResumePath          = "/user/session/resume"
	accountLogoutPath          = "/user/session/logout"
	accountRememberSessionsKey = "rememberSessions"
	rememberLoginDuration      = 30 * 24 * time.Hour
	rememberTokenBytes         = 32
	rememberMaxDevices         = 5
)

var errRememberAccountChanged = errors.New("account changed after authentication")

type rememberLoginCredential struct {
	token     string
	expiresAt int64
}

type rememberAccountBinding struct {
	userID, email, mobile, password, passwordHash string
	verified                                      bool
}

func bindRememberAccount(user Record) rememberAccountBinding {
	return rememberAccountBinding{
		userID:       roleString(user, "userId"),
		email:        roleString(user, "email"),
		mobile:       roleString(user, "mobilePhone"),
		password:     roleString(user, "password"),
		passwordHash: roleString(user, "passwordHash"),
		verified:     roleBool(user, "emailVerified"),
	}
}

// Binding the digest to the current stored credentials rejects an old token
// even if another account maintenance tool updates the password directly.
func (account rememberAccountBinding) tokenDigest(token []byte) string {
	hash := sha256.New()
	_, _ = hash.Write(token)
	_, _ = fmt.Fprintf(hash, "%q %q %q %q %q %t", account.userID, account.email, account.mobile,
		account.password, account.passwordHash, account.verified)
	return hex.EncodeToString(hash.Sum(nil))
}

func (s *Server) canRememberAccount(user Record) bool {
	bound := bindRememberAccount(user)
	if bound.userID == "" || len(bound.userID) > maxEmailBytes || emailAccountNeedsVerification(user) {
		return false
	}
	if s.emailRegistration {
		return bound.password != "" || bound.passwordHash != ""
	}
	return bound.mobile != "" && bound.password != ""
}

func (s *Server) issueRememberCredential(user Record, now time.Time) (rememberLoginCredential, error) {
	var secret [rememberTokenBytes]byte
	if _, err := rand.Read(secret[:]); err != nil {
		return rememberLoginCredential{}, err
	}
	bound := bindRememberAccount(user)
	key, _ := s.users.find(func(record Record) bool { return record["userId"] == bound.userID })
	expires := now.Add(rememberLoginDuration).Unix()
	digest := bound.tokenDigest(secret[:])
	if err := s.users.Update(key, func(record Record) error {
		if bindRememberAccount(record) != bound {
			return errRememberAccountChanged
		}
		sessions, _ := record[accountRememberSessionsKey].(Record)
		if sessions == nil {
			sessions = make(Record)
		}
		for key := range sessions {
			if roleInt(sessions, key) <= now.Unix() {
				delete(sessions, key)
			}
		}
		for len(sessions) >= rememberMaxDevices {
			var oldest string
			var oldestExpiry int64
			for key := range sessions {
				expiry := roleInt(sessions, key)
				if oldest == "" || expiry < oldestExpiry || (expiry == oldestExpiry && key < oldest) {
					oldest, oldestExpiry = key, expiry
				}
			}
			delete(sessions, oldest)
		}
		sessions[digest] = expires
		record[accountRememberSessionsKey] = sessions
		return nil
	}); err != nil {
		return rememberLoginCredential{}, err
	}
	return rememberLoginCredential{base64.RawURLEncoding.EncodeToString(secret[:]), expires}, nil
}

func readRememberLoginForm(w http.ResponseWriter, r *http.Request) (userID, token string, decoded []byte) {
	form, err := parseEmailAccountForm(w, r)
	if err != nil || len(form["userId"]) != 1 || len(form["rememberToken"]) != 1 {
		return "", "", nil
	}
	userID, token = form.Get("userId"), form.Get("rememberToken")
	if userID == "" || len(userID) > maxEmailBytes || len(token) != base64.RawURLEncoding.EncodedLen(rememberTokenBytes) {
		return "", "", nil
	}
	decoded, err = base64.RawURLEncoding.Strict().DecodeString(token)
	if err != nil || len(decoded) != rememberTokenBytes {
		return "", "", nil
	}
	return userID, token, decoded
}

func (s *Server) accountResume(w http.ResponseWriter, r *http.Request) error {
	w.Header().Set("Cache-Control", "no-store")
	userID, token, decoded := readRememberLoginForm(w, r)
	if decoded == nil {
		writeAccount(w, "2105", "本地登录已失效，请重新登录", "")
		return nil
	}
	user := s.users.Find(func(record Record) bool { return record["userId"] == userID })
	bound := bindRememberAccount(user)
	sessions, _ := user[accountRememberSessionsKey].(Record)
	expires := roleInt(sessions, bound.tokenDigest(decoded))
	if expires <= time.Now().Unix() || !s.canRememberAccount(user) {
		writeAccount(w, "2105", "本地登录已失效，请重新登录", "")
		return nil
	}
	displayID := bound.mobile
	if s.emailRegistration && bound.email != "" {
		displayID = bound.email
	}
	if s.emailRegistration {
		if email, valid := normalizeAccountEmail(displayID); valid {
			displayID = email
		}
	}
	return s.writeAccountLogin(w, user, displayID, false, rememberLoginCredential{token, expires})
}

func (s *Server) accountLogout(w http.ResponseWriter, r *http.Request) error {
	w.Header().Set("Cache-Control", "no-store")
	userID, _, decoded := readRememberLoginForm(w, r)
	if decoded != nil {
		key, _ := s.users.find(func(record Record) bool { return record["userId"] == userID })
		if key != "" {
			err := s.users.Transact(func(tx *StoreTx) error {
				current := tx.View(key)
				if current["userId"] != userID {
					return nil
				}
				digest := bindRememberAccount(current).tokenDigest(decoded)
				sessions, _ := current[accountRememberSessionsKey].(Record)
				if _, exists := sessions[digest]; !exists {
					return nil
				}
				record := tx.Get(key)
				sessions = record[accountRememberSessionsKey].(Record)
				delete(sessions, digest)
				if len(sessions) == 0 {
					delete(record, accountRememberSessionsKey)
				}
				return nil
			})
			if err != nil {
				return err
			}
		}
	}
	writeAccount(w, "2000", "success", "true")
	return nil
}
