package main

import (
	"errors"
	"io"
	"net/http"
	"net/mail"
	"net/url"
	"strings"
	"time"
	"unicode/utf8"

	"golang.org/x/crypto/bcrypt"
)

const (
	maxEmailBytes           = 254
	minEmailPasswordBytes   = 8
	maxEmailPasswordBytes   = 72 // bcrypt only supports passwords up to 72 bytes.
	maxLegacyPasswordBytes  = 1024
	maxEmailAccountFormSize = 8 << 10
)

func parseEmailAccountForm(w http.ResponseWriter, r *http.Request) (url.Values, error) {
	body, err := io.ReadAll(http.MaxBytesReader(w, r.Body, maxEmailAccountFormSize))
	if err != nil {
		return nil, err
	}
	form, err := url.ParseQuery(string(body))
	if err != nil {
		return nil, err
	}
	for _, field := range []string{"email", "mobilePhone", "password", "checkCode"} {
		if len(form[field]) > 1 {
			return nil, errors.New("duplicate account form field")
		}
	}
	return form, nil
}

func normalizeAccountEmail(input string) (string, bool) {
	if !utf8.ValidString(input) {
		return "", false
	}
	email := strings.ToLower(strings.TrimSpace(input))
	local, domain, found := strings.Cut(email, "@")
	if !found || len(email) > maxEmailBytes || len(local) == 0 || len(local) > 64 {
		return "", false
	}
	address, err := mail.ParseAddress(email)
	if err != nil || address.Address != email || !strings.Contains(domain, ".") {
		return "", false
	}
	for _, label := range strings.Split(domain, ".") {
		if len(label) == 0 || len(label) > 63 || label[0] == '-' || label[len(label)-1] == '-' {
			return "", false
		}
		for _, ch := range label {
			if (ch < 'a' || ch > 'z') && (ch < '0' || ch > '9') && ch != '-' {
				return "", false
			}
		}
	}
	return email, true
}

func accountMatchesEmail(account Record, email string) bool {
	registered, _ := account["email"].(string)
	legacy, _ := account["mobilePhone"].(string)
	return registered == email || strings.EqualFold(strings.TrimSpace(legacy), email)
}

func emailAccountNeedsVerification(account Record) bool {
	return roleString(account, "email") != "" && roleString(account, "passwordHash") != "" && !roleBool(account, "emailVerified")
}

func (s *Server) findEmailAccount(email, legacyIdentifier string) Record {
	if account := s.users.Get("e_" + email); account != nil {
		return account
	}
	// Old clients and servers accepted any nonempty identifier in mobilePhone.
	if legacyIdentifier != "" {
		if _, account := s.findAccount(legacyIdentifier); account != nil {
			return account
		}
	}
	return s.users.Find(func(account Record) bool { return accountMatchesEmail(account, email) })
}

func (s *Server) accountRegisterEmail(w http.ResponseWriter, r *http.Request) error {
	form, err := parseEmailAccountForm(w, r)
	if err != nil {
		writeAccount(w, "5001", "注册信息无效", "")
		return nil
	}
	email, valid := normalizeAccountEmail(form.Get("email"))
	if !valid || form.Has("mobilePhone") {
		writeAccount(w, "5001", "请输入有效的邮箱地址", "")
		return nil
	}
	password := form.Get("password")
	if len(password) < minEmailPasswordBytes || len(password) > maxEmailPasswordBytes ||
		!utf8.ValidString(password) || strings.TrimSpace(password) == "" {
		writeAccount(w, "5001", "密码长度须为 8–72 字节，且至少包含一个非空白字符", "")
		return nil
	}
	if existing := s.findEmailAccount(email, ""); existing != nil && !emailAccountNeedsVerification(existing) {
		writeAccount(w, "2201", "邮箱已注册", "")
		return nil
	}
	code := form.Get("checkCode")
	if !validRegistrationEmailCode(code) {
		writeAccount(w, "5001", "请输入 6 位邮箱验证码", "")
		return nil
	}
	if s.sendRegistrationEmail == nil {
		writeAccount(w, "4006", "邮箱验证码服务暂不可用", "")
		return nil
	}
	challenge := s.emailCodes.claim(email, code, time.Now())
	if challenge == nil {
		writeAccount(w, "4010", "邮箱验证码不正确或已失效，请重新获取", "")
		return nil
	}
	registered := false
	defer func() { s.emailCodes.finishClaim(email, challenge, registered) }()
	hash, err := bcrypt.GenerateFromPassword([]byte(password), bcrypt.DefaultCost)
	if err != nil {
		return err
	}
	err = s.saveVerifiedEmailAccount(email, string(hash))
	if errors.Is(err, ErrRecordExists) {
		writeAccount(w, "2201", "邮箱已注册", "")
		return nil
	}
	if err != nil {
		return err
	}
	registered = true
	writeAccount(w, "2000", "success", "true")
	return nil
}

func (s *Server) saveVerifiedEmailAccount(email, hash string) error {
	key := "e_" + email
	// Preserve the user ID and roles of email accounts created before verification was required.
	err := s.users.Update(key, func(account Record) error {
		if !emailAccountNeedsVerification(account) || roleString(account, "email") != email {
			return ErrRecordExists
		}
		account["passwordHash"] = hash
		account["emailVerified"] = true
		delete(account, accountRememberSessionsKey)
		return nil
	})
	if !errors.Is(err, ErrRecordNotFound) {
		return err
	}
	account := Record{"userId": newAccountID(), "email": email, "passwordHash": hash, "emailVerified": true}
	return s.users.InsertUnique(key, account, func(current Record) bool { return accountMatchesEmail(current, email) })
}

func (s *Server) accountLoginWithEmail(w http.ResponseWriter, r *http.Request) error {
	form, err := parseEmailAccountForm(w, r)
	if err != nil || form.Has("email") == form.Has("mobilePhone") {
		writeAccount(w, "5001", "请填写邮箱或原手机号和密码", "")
		return nil
	}
	password := form.Get("password")
	if password == "" || len(password) > maxLegacyPasswordBytes {
		writeAccount(w, "5001", "密码长度无效", "")
		return nil
	}
	if form.Has("email") {
		identifier := strings.TrimSpace(form.Get("email"))
		email, valid := normalizeAccountEmail(identifier)
		if !valid {
			writeAccount(w, "5001", "请输入有效的邮箱地址", "")
			return nil
		}
		user := s.findEmailAccount(email, identifier)
		if !accountPasswordMatches(user, password) {
			writeAccount(w, "2101", "邮箱或密码错误", "")
			return nil
		}
		return s.writeAuthenticatedAccountLogin(w, user, email, false, form.Get("rememberLogin") == "1")
	}
	phone := form.Get("mobilePhone")
	if phone == "" || len(phone) > maxEmailBytes {
		writeAccount(w, "5001", "请输入原手机号", "")
		return nil
	}
	_, user := s.findAccount(phone)
	if !accountPasswordMatches(user, password) {
		writeAccount(w, "2101", "账号或密码错误", "")
		return nil
	}
	return s.writeAuthenticatedAccountLogin(w, user, phone, false, form.Get("rememberLogin") == "1")
}

func accountPasswordMatches(account Record, password string) bool {
	if hash, ok := account["passwordHash"].(string); ok {
		return len(password) <= maxEmailPasswordBytes && bcrypt.CompareHashAndPassword([]byte(hash), []byte(password)) == nil
	}
	legacy, ok := account["password"].(string)
	return ok && legacy == password
}
