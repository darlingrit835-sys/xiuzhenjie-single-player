package main

import (
	"context"
	"crypto/tls"
	"encoding/json"
	"errors"
	"flag"
	"fmt"
	"log"
	"net"
	"net/http"
	"os"
	"os/signal"
	"path/filepath"
	"strconv"
	"sync"
	"syscall"
	"time"

	"google.golang.org/protobuf/proto"

	"xiuzhenjie/server/internal/opslog"
)

const serverVersion = "1.3.5"

type Config struct {
	AccountsDir               string                         `json:"-"`
	AuctionCalendar           map[string]auctionCalendarRule `json:"auctionCalendar,omitempty"`
	TrialExchangeStoneReserve int64                          `json:"trialExchangeStoneReserve"`
	PublicIP                  string                         `json:"publicIp"`
	GamePort                  int                            `json:"-"`
	AccountPort               int                            `json:"-"`
	BattlePort                int                            `json:"battlePort"`
	BattleDurationSeconds     int                            `json:"battleDurationSeconds"`
	MonsterCount              int                            `json:"monsterCount"`
	MonsterDifficulty         float64                        `json:"monsterDifficulty"`
	DropMultiplier            float64                        `json:"dropMultiplier"`
	CultivationSpeed          float64                        `json:"cultivationSpeed"`
	OfflineCapHours           int                            `json:"offlineCapHours"`
	CompleteBattleGuide       bool                           `json:"completeBattleGuide"`
	SkipAcupointGuide         bool                           `json:"skipAcupointGuide"`
}

type Server struct {
	alchemy               *alchemyRoom
	smithing              *smithingWorkshop
	privateChat           privateChatStore
	auctionChatCache      sync.Map // tier -> immutable public message window
	config                Config
	dataDir               string
	protocol              *Protocol
	users                 *Store
	roles                 *Store
	game                  gameTables
	battleState           battleState
	preview               previewRegistry
	previewWrites         bool
	emailRegistration     bool
	emailCodes            emailCodeStore
	sendRegistrationEmail func(context.Context, string, string) error
	previewSessionKey     [32]byte
	previewAuction        previewAuctionRuntime
	guilds                *Store
	history               *gameHistory
	conns                 sync.Map
	online                sync.Map
}

func readConfig(dataDir string) (Config, error) {
	defaults := Config{
		TrialExchangeStoneReserve: 100_000,
		PublicIP:                  "127.0.0.1", GamePort: 17666, AccountPort: 9443, BattlePort: 17667,
		BattleDurationSeconds: 600, MonsterCount: 6, MonsterDifficulty: 0.85,
		DropMultiplier: 1, CultivationSpeed: 1, OfflineCapHours: 12, CompleteBattleGuide: true,
	}
	config := defaults
	content, err := os.ReadFile(filepath.Join(dataDir, "config", "server.json"))
	if errors.Is(err, os.ErrNotExist) {
		return config, nil
	}
	if err != nil {
		return config, err
	}
	if err := json.Unmarshal(content, &config); err != nil {
		return config, fmt.Errorf("parse server.json: %w", err)
	}
	if err := validateAuctionCalendar(config.AuctionCalendar); err != nil {
		return config, err
	}
	if config.PublicIP == "" {
		config.PublicIP = defaults.PublicIP
	}
	if config.BattlePort <= 0 {
		config.BattlePort = defaults.BattlePort
	}
	if config.BattleDurationSeconds <= 0 {
		config.BattleDurationSeconds = defaults.BattleDurationSeconds
	}
	if config.MonsterCount <= 0 {
		config.MonsterCount = defaults.MonsterCount
	}
	if config.MonsterDifficulty <= 0 {
		config.MonsterDifficulty = 1
	}
	if config.DropMultiplier <= 0 {
		config.DropMultiplier = defaults.DropMultiplier
	}
	if config.CultivationSpeed <= 0 {
		config.CultivationSpeed = defaults.CultivationSpeed
	}
	if config.OfflineCapHours <= 0 {
		config.OfflineCapHours = defaults.OfflineCapHours
	}
	return config, nil
}

func openServer(config Config, data string) (*Server, error) {
	protocol, err := loadProtocol()
	if err != nil {
		return nil, err
	}
	accountsDir := data
	if config.AccountsDir != "" {
		accountsDir = config.AccountsDir
		if _, err := os.Stat(filepath.Join(accountsDir, "users.json")); err != nil {
			return nil, fmt.Errorf("open existing account directory: %w", err)
		}
	}
	users, err := OpenStore(accountsDir, "users")
	if err != nil {
		return nil, err
	}
	roles, err := OpenStore(data, "roles")
	if err != nil {
		return nil, err
	}
	s := &Server{config: config, dataDir: data, protocol: protocol, users: users, roles: roles}
	if err := s.loadGameTables(); err != nil {
		return nil, err
	}
	if err := s.loadSmithingWorkshop(); err != nil {
		return nil, err
	}
	if err := s.loadAlchemy(); err != nil {
		return nil, err
	}
	return s, nil
}

func (s *Server) bindGameRole(peer *Peer, id string) {
	if peer.roleID != "" {
		s.online.CompareAndDelete(peer.roleID, peer)
		if s.previewWrites {
			s.previewAuction.leave(peer)
		}
	}
	peer.writes.Lock()
	peer.worldChatJoined.Store(false)
	if peer.roleID != id {
		peer.worldChatNextSend = time.Time{}
		peer.lastLegacySpellPractice = 0
	}
	peer.roleID = id
	clear(peer.inventory[:])
	peer.onlineRole.Store(id)
	peer.writes.Unlock()
	s.online.Store(id, peer)
}

func (s *Server) pushGame(roleID string, cmd int, data proto.Message) {
	s.pushGameIf(roleID, cmd, data, nil)
}

func (s *Server) pushGameIf(roleID string, cmd int, data proto.Message, current func() bool) {
	if peer, ok := s.online.Load(roleID); ok {
		payload, err := proto.Marshal(data)
		if err == nil {
			peer.(*Peer).writeFrameForRole(roleID, encodeFrame(cmd, 0, payload), current)
		}
	}
}

func (s *Server) gamePeers() []*Peer {
	var peers []*Peer
	s.online.Range(func(_, value any) bool {
		peers = append(peers, value.(*Peer))
		return true
	})
	return peers
}

func listen(host string, port int) (net.Listener, error) {
	if port < 1 || port > 65535 {
		return nil, fmt.Errorf("invalid port %d", port)
	}
	return net.Listen("tcp", net.JoinHostPort(host, strconv.Itoa(port)))
}

func (s *Server) run(ctx context.Context, host string, cert tls.Certificate) error {
	account, err := listen(host, s.config.AccountPort)
	if err != nil {
		return fmt.Errorf("account listener: %w", err)
	}
	defer account.Close()
	game, err := listen(host, s.config.GamePort)
	if err != nil {
		return fmt.Errorf("game listener: %w", err)
	}
	defer game.Close()
	battle, err := listen(host, s.config.BattlePort)
	if err != nil {
		return fmt.Errorf("battle listener: %w", err)
	}
	defer battle.Close()

	if s.previewWrites {
		if err := s.settlePreviewAuctions(time.Now()); err != nil {
			log.Printf("启动时拍卖结算失败，将重试: %v", err)
		}
	}
	web := &http.Server{
		Handler: s.accountHandler(), ReadHeaderTimeout: 5 * time.Second, ReadTimeout: 15 * time.Second,
		IdleTimeout: 60 * time.Second,
		TLSConfig:   &tls.Config{Certificates: []tls.Certificate{cert}, MinVersion: tls.VersionTLS12},
	}
	failures := make(chan error, 3)
	var accepting, peers sync.WaitGroup
	accepting.Add(2)
	go func() {
		defer accepting.Done()
		failures <- s.accept(game, false, &peers)
	}()
	go func() {
		defer accepting.Done()
		failures <- s.accept(battle, true, &peers)
	}()
	go func() { failures <- web.ServeTLS(account, "", "") }()
	ticks, stopTicks := context.WithCancel(ctx)
	var workers sync.WaitGroup
	workers.Add(1)
	go func() {
		defer workers.Done()
		s.advanceCultivation(ticks)
	}()
	if s.previewWrites {
		workers.Add(1)
		go func() {
			defer workers.Done()
			s.runScheduledAuctions(ticks)
		}()
		workers.Add(1)
		go func() {
			defer workers.Done()
			s.runSectCalendar(ticks)
		}()
		workers.Add(1)
		go func() {
			defer workers.Done()
			s.advancePreviewAuctions(ticks)
		}()
	}
	if s.history != nil {
		workers.Add(1)
		go func() {
			defer workers.Done()
			s.history.run(ticks, s.roles, s.guilds)
		}()
	}
	log.Printf("READY account=https://%s game=tcp://%s battle=tcp://%s", account.Addr(), game.Addr(), battle.Addr())

	select {
	case err = <-failures:
	case <-ctx.Done():
	}
	stopTicks()
	_ = game.Close()
	_ = battle.Close()
	accepting.Wait()
	s.conns.Range(func(key, _ any) bool {
		_ = key.(net.Conn).Close()
		return true
	})
	shutdown, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()
	_ = web.Shutdown(shutdown)
	peers.Wait()
	if closeErr := s.privateChat.close(); closeErr != nil {
		log.Printf("关闭私聊存储失败: %v", closeErr)
	}
	workers.Wait()
	if s.history != nil {
		if err := s.history.flush(s.roles, s.guilds); err != nil {
			log.Printf("停服归档失败，待归档事件已留在存档: %v", err)
		}
	}
	return err
}

func main() {
	data := flag.String("data", "../server/data", "Java-compatible data directory")
	accountsDir := flag.String("accounts-data", "", "use an existing users.json from this directory; only one game process may write these accounts")
	host := flag.String("bind", "127.0.0.1", "listen address")
	public := flag.String("public-ip", "", "address advertised to the client")
	account := flag.Int("account-port", 0, "override the account HTTPS port")
	game := flag.Int("game-port", 0, "override the game TCP port")
	battle := flag.Int("battle-port", 0, "override the battle TCP port")
	preview := flag.Bool("interface-preview", false, "enable trial-only frontend replies")
	previewWrites := flag.Bool("interface-writes", false, "enable persistent auction, guild, friend, mentorship and acupoint actions in interface preview")
	skipAcupointGuide := flag.Bool("skip-acupoint-guide", false, "omit the required meridian tutorial on login without changing saved progress")
	emailRegistration := flag.Bool("email-registration", false, "enable email/password registration and sign-in for accounts")
	catalog := flag.Bool("interface-catalog", false, "print interface metadata without starting the server")
	historyDir := flag.String("history-dir", "", "game event archive directory, defaults to <data>/history")
	historyMode := flag.String("history-query", "", "read matching events or firsts without starting the server")
	historyKind := flag.String("history-kind", "", "filter history by event kind")
	historyRoleID := flag.String("history-role", "", "filter history by actor or subject role ID")
	historyScope := flag.String("history-scope", "", "filter firsts by world, account, role, world-entity or role-entity")
	historyLimit := flag.Int("history-limit", 100, "maximum number of matching history entries")
	logDir := flag.String("log-dir", "", "directory for rotated server logs")
	flag.Parse()

	if *historyDir == "" {
		*historyDir = filepath.Join(*data, "history")
	}
	if *historyMode != "" {
		query := historyQuery{mode: *historyMode, kind: *historyKind, roleID: *historyRoleID, scope: *historyScope, limit: *historyLimit}
		if err := query.write(*historyDir, os.Stdout); err != nil {
			log.Fatal(err)
		}
		return
	}

	if *catalog {
		if err := writeInterfaceCatalog(os.Stdout); err != nil {
			log.Fatal(err)
		}
		return
	}
	if *previewWrites && !*preview {
		log.Fatal("-interface-writes requires -interface-preview")
	}
	if *logDir != "" {
		writer, err := opslog.Open(*logDir, "server")
		if err != nil {
			log.Fatal(err)
		}
		defer writer.Close()
		log.SetOutput(writer)
	}
	log.SetFlags(log.LstdFlags | log.LUTC | log.Lmicroseconds)
	config, err := readConfig(*data)
	if err != nil {
		log.Fatal(err)
	}
	config.SkipAcupointGuide = config.SkipAcupointGuide || *skipAcupointGuide
	config.AccountsDir = *accountsDir
	if *public != "" {
		config.PublicIP = *public
	}
	if *account != 0 {
		config.AccountPort = *account
	}
	if *game != 0 {
		config.GamePort = *game
	}
	if *battle != 0 {
		config.BattlePort = *battle
	}
	password := os.Getenv("XZJ_TLS_PASSWORD")
	if password == "" {
		log.Fatal("set XZJ_TLS_PASSWORD before starting the server")
	}
	certificate, err := loadCertificate(*data, password)
	if err != nil {
		log.Fatal(err)
	}
	s, err := openServer(config, *data)
	if err != nil {
		log.Fatal(err)
	}
	s.emailRegistration = *emailRegistration
	if *emailRegistration {
		s.sendRegistrationEmail, err = registrationSMTPSenderFromEnv()
		if err != nil {
			log.Fatal(err)
		}
	}
	if *preview {
		s.preview, err = loadPreview(s.protocol)
		if err != nil {
			log.Fatal(err)
		}
		log.Printf("Interface preview enabled (%d mapped commands)", len(s.preview))
	}
	if *previewWrites {
		if err := s.enablePreviewWrites(); err != nil {
			log.Fatal(err)
		}
		log.Print("Interface preview writes enabled")
	} else if *emailRegistration {
		if err := s.initAccountSessions(); err != nil {
			log.Fatal(err)
		}
	}
	if *emailRegistration {
		log.Printf("Email registration enabled (SMTP configured: %t)", s.sendRegistrationEmail != nil)
	}
	s.history, err = openGameHistory(*historyDir)
	if err != nil {
		log.Fatalf("open game history: %v", err)
	}
	if err := s.history.checkStores(s.roles, s.guilds); err != nil {
		log.Fatal(err)
	}
	s.roles.enableHistory()
	if s.guilds != nil {
		s.guilds.enableHistory()
	}
	ctx, stop := signal.NotifyContext(context.Background(), os.Interrupt, syscall.SIGTERM)
	defer stop()
	if err := s.run(ctx, *host, certificate); err != nil && !errors.Is(err, http.ErrServerClosed) {
		log.Fatal(err)
	}
}
