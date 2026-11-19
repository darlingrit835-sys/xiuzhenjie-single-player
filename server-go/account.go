package main

import (
	"bytes"
	"crypto/tls"
	"encoding/json"
	"errors"
	"fmt"
	"io"
	"log"
	"net/http"
	"net/url"
	"os"
	"path/filepath"
	"strconv"
	"strings"
	"sync/atomic"
	"time"

	"software.sslmate.com/src/go-pkcs12"
)

func loadCertificate(dataDir, password string) (tls.Certificate, error) {
	path := filepath.Join(dataDir, "server.p12")
	data, err := os.ReadFile(path)
	if err != nil {
		return tls.Certificate{}, err
	}
	key, leaf, chain, err := pkcs12.DecodeChain(data, password)
	if err != nil {
		return tls.Certificate{}, fmt.Errorf("read PKCS#12 %s: %w", path, err)
	}
	certs := make([][]byte, 1, len(chain)+1)
	certs[0] = leaf.Raw
	for _, cert := range chain {
		certs = append(certs, cert.Raw)
	}
	return tls.Certificate{Certificate: certs, PrivateKey: key, Leaf: leaf}, nil
}

type accountRoute struct {
	path   string
	method string
	handle func(http.ResponseWriter, *http.Request) error
}

func (s *Server) accountHandler() http.Handler {
	routes := []accountRoute{
		{"/login/roleCdKey/cdkeyExchange", http.MethodGet, accountStatic("2501", "兑换码无效", "")},
		{"/client/update/local/download", http.MethodGet, accountHotfix},
		{"/client/data/queryClientDataModel", http.MethodGet, accountStatic("2000", "success", "{}")},
		{"/client/log/error", http.MethodPost, accountStatic("2000", "success", "true")},
		{"/gameService/statusSearch", http.MethodGet, s.accountStatus},
		{"/announcement/queryInfo", http.MethodGet, accountStatic("2000", "success", "[]")},
		{"/announcement/isShow", http.MethodGet, accountStatic("2000", "success", "false")},
		{"/user/sendCheckCode", http.MethodGet, s.accountCheckCode},
		{"/user/sendEmailCode", http.MethodPost, s.accountSendEmailCode},
		{"/user/register", http.MethodPost, s.accountRegister},
		{"/user/pwdUpdate", http.MethodPost, s.accountPassword},
		{"/user/roleSuicide", http.MethodPost, accountStatic("2000", "success", "true")},
		{accountResumePath, http.MethodPost, s.accountResume},
		{accountLogoutPath, http.MethodPost, s.accountLogout},
		{"/login", http.MethodPost, s.accountLogin},
	}
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		// Java matches context prefixes. The longer /login context comes first.
		for _, route := range routes {
			if !strings.HasPrefix(r.URL.Path, route.path) ||
				((route.path == accountResumePath || route.path == accountLogoutPath) && r.URL.Path != route.path) {
				continue
			}
			if !strings.EqualFold(r.Method, route.method) {
				writeAccount(w, "1001", "method not allowed", "")
				return
			}
			if err := route.handle(w, r); err != nil {
				log.Printf("account %s: %v", route.path, err)
				writeAccount(w, "1001", "server error", "")
			}
			return
		}
		http.NotFound(w, r)
	})
}

func accountStatic(code, message, data string) func(http.ResponseWriter, *http.Request) error {
	return func(w http.ResponseWriter, _ *http.Request) error {
		writeAccount(w, code, message, data)
		return nil
	}
}

func writeAccount(w http.ResponseWriter, code, message, data string) {
	envelope := Record{
		"msgBody": Record{
			"sysInfo": Record{"httpCode": http.StatusOK},
			"bizInfo": Record{"code": code, "msg": message, "dataString": data},
		},
	}
	w.Header().Set("Content-Type", "application/json; charset=utf-8")
	encoder := json.NewEncoder(w)
	encoder.SetEscapeHTML(false)
	_ = encoder.Encode(envelope)
}

func writeAccountData(w http.ResponseWriter, data any) error {
	var body bytes.Buffer
	encoder := json.NewEncoder(&body)
	encoder.SetEscapeHTML(false)
	if err := encoder.Encode(data); err != nil {
		return err
	}
	writeAccount(w, "2000", "success", strings.TrimSuffix(body.String(), "\n"))
	return nil
}

func parseAccountForm(w http.ResponseWriter, r *http.Request) (map[string]string, error) {
	body, err := io.ReadAll(http.MaxBytesReader(w, r.Body, 1<<20))
	if err != nil {
		return nil, err
	}
	fields := make(map[string]string)
	for _, part := range strings.Split(string(body), "&") {
		name, value, found := strings.Cut(part, "=")
		if !found || name == "" {
			continue
		}
		name, err = url.QueryUnescape(name)
		if err != nil {
			return nil, errors.New("invalid account form field encoding")
		}
		value, err = url.QueryUnescape(value)
		if err != nil {
			return nil, errors.New("invalid account form value encoding")
		}
		fields[name] = value
	}
	if _, ok := fields["mobilePhone"]; !ok {
		return nil, errors.New("account form is missing mobilePhone")
	}
	return fields, nil
}

func formPassword(form map[string]string) any {
	if value, found := form["password"]; found {
		return value
	}
	return nil
}

var lastAccountID atomic.Int64

func newAccountID() string {
	for {
		previous := lastAccountID.Load()
		now := time.Now().UnixMilli()
		if now <= previous {
			now = previous + 1
		}
		if lastAccountID.CompareAndSwap(previous, now) {
			return "u_" + strconv.FormatInt(now, 10)
		}
	}
}

func (s *Server) findAccount(phone string) (string, Record) {
	return s.users.find(func(record Record) bool { return record["mobilePhone"] == phone })
}

func (s *Server) createAccount(phone string, password any) (Record, error) {
	account := Record{"mobilePhone": phone, "userId": newAccountID()}
	if password != nil {
		account["password"] = password
	}
	err := s.users.InsertUnique("u_"+phone, account, func(record Record) bool {
		return record["mobilePhone"] == phone
	})
	return account, err
}

func (s *Server) accountLogin(w http.ResponseWriter, r *http.Request) error {
	if s.emailRegistration {
		return s.accountLoginWithEmail(w, r)
	}
	form, err := parseAccountForm(w, r)
	if err != nil {
		return err
	}
	phone, password := form["mobilePhone"], formPassword(form)
	_, user := s.findAccount(phone)
	newUser := false
	if user == nil {
		user, err = s.createAccount(phone, password)
		if errors.Is(err, ErrRecordExists) {
			_, user = s.findAccount(phone)
			if user == nil {
				return err
			}
		} else if err != nil {
			return err
		} else {
			newUser = true
		}
	}
	if user["password"] != password {
		writeAccount(w, "2101", "密码错误", "")
		return nil
	}
	return s.writeAuthenticatedAccountLogin(w, user, phone, newUser, form["rememberLogin"] == "1")
}

func (s *Server) writeAuthenticatedAccountLogin(w http.ResponseWriter, user Record, displayID string, newUser, remember bool) error {
	if s.emailRegistration && emailAccountNeedsVerification(user) {
		writeAccount(w, "4008", "邮箱尚未验证，请使用验证码完成注册", "")
		return nil
	}
	var credential rememberLoginCredential
	if remember && s.canRememberAccount(user) {
		w.Header().Set("Cache-Control", "no-store")
		var err error
		credential, err = s.issueRememberCredential(user, time.Now())
		if errors.Is(err, errRememberAccountChanged) {
			writeAccount(w, "2101", "账号信息已变更，请重新登录", "")
			return nil
		}
		if err != nil {
			return err
		}
	}
	return s.writeAccountLogin(w, user, displayID, newUser, credential)
}

func (s *Server) writeAccountLogin(w http.ResponseWriter, user Record, displayID string, newUser bool, credential rememberLoginCredential) error {
	userID, ok := user["userId"].(string)
	if !ok {
		return errors.New("account has invalid userId")
	}
	server := s.accountServerInfo()
	if s.accountSessionsRequired() {
		server["gameServerIpManageId"] = s.issuePreviewSession(userID, time.Now())
	}
	role := s.roles.Find(func(record Record) bool { return record["UserId"] == userID })
	if role != nil {
		server["currentRoleVO"] = Record{
			"id": role["Id"], "userId": userID, "serversRoleId": role["Id"], "name": role["Name"],
			"level": accountRoleLevel(role["Level"]), "gameServersId": "1",
		}
	}
	login := Record{
		"userId": userID, "mobilePhone": displayID, "gameServersInfoList": []Record{server},
		"lastGameServersId": "1", "whetherNewUser": newUser,
	}
	if credential.token != "" {
		login["rememberToken"] = credential.token
		login["rememberTokenExpiresAt"] = credential.expiresAt
	}
	return writeAccountData(w, login)
}

func accountRoleLevel(value any) int64 {
	switch level := value.(type) {
	case int:
		return int64(level)
	case int32:
		return int64(level)
	case int64:
		return level
	case float64:
		return int64(level)
	case json.Number:
		if integer, err := level.Int64(); err == nil {
			return integer
		}
		fractional, _ := level.Float64()
		return int64(fractional)
	default:
		return 0
	}
}

func (s *Server) accountRegister(w http.ResponseWriter, r *http.Request) error {
	if s.emailRegistration {
		return s.accountRegisterEmail(w, r)
	}
	form, err := parseAccountForm(w, r)
	if err != nil {
		return err
	}
	_, err = s.createAccount(form["mobilePhone"], formPassword(form))
	if errors.Is(err, ErrRecordExists) {
		writeAccount(w, "2201", "手机号已注册", "")
		return nil
	}
	if err != nil {
		return err
	}
	writeAccount(w, "2000", "success", "true")
	return nil
}

func (s *Server) accountCheckCode(w http.ResponseWriter, _ *http.Request) error {
	if s.emailRegistration {
		writeAccount(w, "4008", "暂未开通验证码服务", "")
		return nil
	}
	writeAccount(w, "2000", "success", "true")
	return nil
}

func (s *Server) accountPassword(w http.ResponseWriter, r *http.Request) error {
	if s.emailRegistration {
		writeAccount(w, "4008", "暂未开通密码找回", "")
		return nil
	}
	if s.previewWrites {
		writeAccount(w, "4008", "试用服未启用账号找回", "")
		return nil
	}
	form, err := parseAccountForm(w, r)
	if err != nil {
		return err
	}
	key, user := s.findAccount(form["mobilePhone"])
	if user == nil {
		writeAccount(w, "2301", "账号不存在", "")
		return nil
	}
	if password, ok := form["password"]; ok {
		if err := s.users.Update(key, func(record Record) error {
			if record["password"] != password {
				delete(record, accountRememberSessionsKey)
			}
			record["password"] = password
			return nil
		}); err != nil {
			return err
		}
	}
	writeAccount(w, "2000", "success", "true")
	return nil
}

func (s *Server) accountServerInfo() Record {
	return Record{
		"gameServerId": "1", "name": "修真界-私服1", "osType": 1, "serverIp": s.config.PublicIP,
		"port": s.config.GamePort, "registerCount": 0, "onlineCount": 0, "maxCount": 5000,
		"gameServerIpManageId": "1", "status": 1, "serverVersion": serverVersion,
	}
}

func (s *Server) accountStatus(w http.ResponseWriter, _ *http.Request) error {
	server := s.accountServerInfo()
	server["registerCount"] = 10
	return writeAccountData(w, []Record{server})
}

func accountHotfix(w http.ResponseWriter, _ *http.Request) error {
	w.Header().Set("Content-Type", "application/xml; charset=utf-8")
	_, _ = fmt.Fprintf(w, "<?xml version=\"1.0\" encoding=\"utf-8\"?><ServerInfo><GameVersion Version=\"%s\"></GameVersion></ServerInfo>", serverVersion)
	return nil
}
