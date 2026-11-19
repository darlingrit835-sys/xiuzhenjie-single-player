package main

import (
	"bytes"
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"io"
	"log"
	"os"
	"sync"
	"sync/atomic"
	"time"
)

const (
	ingressLimitPoll = 2 * time.Second
	maxLimitFileSize = 4 << 10
)

type ingressLane int

const (
	laneAccount ingressLane = iota
	laneAuth
	laneOTP
	laneGame
	laneBattle
	ingressLaneCount
)

var ingressLaneNames = [ingressLaneCount]string{"account", "auth", "otp", "game", "battle"}

type ingressLimits struct {
	Account      int `json:"account"`
	Auth         int `json:"auth"`
	OTP          int `json:"otp"`
	Game         int `json:"game"`
	Battle       int `json:"battle"`
	OTPPerMinute int `json:"otpPerMinute"`
}

func defaultIngressLimits() ingressLimits {
	return ingressLimits{
		Account: ingressGeneralRequests, Auth: ingressCredentialRequests, OTP: ingressOTPRequests,
		Game: ingressGameTunnels, Battle: ingressBattleTunnels, OTPPerMinute: ingressOTPPerMin,
	}
}

func (p ingressLimits) concurrency(lane ingressLane) int {
	return [ingressLaneCount]int{p.Account, p.Auth, p.OTP, p.Game, p.Battle}[lane]
}

type ingressLaneMetrics struct {
	InFlight int    `json:"inFlight"`
	Admitted uint64 `json:"admitted"`
	Rejected uint64 `json:"rejected"`
}

type ingressMetrics struct {
	Limits ingressLimits                 `json:"limits"`
	Lanes  map[string]ingressLaneMetrics `json:"lanes"`
}

type ingressLaneState struct {
	mu         sync.Mutex
	metrics    ingressLaneMetrics
	windowEnd  time.Time
	windowUsed int
}

type ingressLimitPolicy struct {
	profile atomic.Pointer[ingressLimits]
	lanes   [ingressLaneCount]ingressLaneState
}

func (p *ingressLimitPolicy) replace(profile ingressLimits) {
	p.profile.Store(&profile)
}

func (p *ingressLimitPolicy) admit(lane ingressLane, now time.Time) bool {
	state := &p.lanes[lane]
	state.mu.Lock()
	defer state.mu.Unlock()
	profile := p.profile.Load()
	if state.metrics.InFlight >= profile.concurrency(lane) {
		state.metrics.Rejected++
		return false
	}
	if lane == laneOTP {
		if !now.Before(state.windowEnd) {
			state.windowEnd = now.Add(time.Minute)
			state.windowUsed = 0
		}
		if state.windowUsed >= profile.OTPPerMinute {
			state.metrics.Rejected++
			return false
		}
		state.windowUsed++
	}
	state.metrics.InFlight++
	state.metrics.Admitted++
	return true
}

func (p *ingressLimitPolicy) release(lane ingressLane) {
	state := &p.lanes[lane]
	state.mu.Lock()
	state.metrics.InFlight--
	state.mu.Unlock()
}

func (p *ingressLimitPolicy) metrics() ingressMetrics {
	snapshot := ingressMetrics{Limits: *p.profile.Load(), Lanes: make(map[string]ingressLaneMetrics, ingressLaneCount)}
	for index, name := range ingressLaneNames {
		state := &p.lanes[index]
		state.mu.Lock()
		snapshot.Lanes[name] = state.metrics
		state.mu.Unlock()
	}
	return snapshot
}

func readIngressLimits(path string) (ingressLimits, error) {
	info, err := os.Stat(path)
	if err != nil {
		return ingressLimits{}, err
	}
	if !info.Mode().IsRegular() || info.Size() > maxLimitFileSize {
		return ingressLimits{}, errors.New("limit profile must be a regular file of at most 4 KiB")
	}
	file, err := os.Open(path)
	if err != nil {
		return ingressLimits{}, err
	}
	defer file.Close()
	data, err := io.ReadAll(io.LimitReader(file, maxLimitFileSize+1))
	if err != nil {
		return ingressLimits{}, err
	}
	if len(data) > maxLimitFileSize {
		return ingressLimits{}, errors.New("limit profile exceeds 4 KiB")
	}
	return decodeIngressLimits(data)
}

func decodeIngressLimits(data []byte) (ingressLimits, error) {
	var profile ingressLimits
	fields := map[string]struct {
		value *int
		max   int
	}{
		"account": {&profile.Account, 128}, "auth": {&profile.Auth, 32}, "otp": {&profile.OTP, 8},
		"game": {&profile.Game, 512}, "battle": {&profile.Battle, 512}, "otpPerMinute": {&profile.OTPPerMinute, 240},
	}
	decoder := json.NewDecoder(bytes.NewReader(data))
	if token, err := decoder.Token(); err != nil || token != json.Delim('{') {
		return ingressLimits{}, errors.New("limit profile must be a JSON object")
	}
	for decoder.More() {
		token, err := decoder.Token()
		key, ok := token.(string)
		if err != nil || !ok {
			return ingressLimits{}, errors.New("invalid limit profile field")
		}
		field, ok := fields[key]
		if !ok {
			return ingressLimits{}, errors.New("unknown or repeated limit profile field")
		}
		var value *int
		if err := decoder.Decode(&value); err != nil || value == nil || *value < 0 || *value > field.max {
			return ingressLimits{}, fmt.Errorf("%s must be an integer between 0 and %d", key, field.max)
		}
		*field.value = *value
		delete(fields, key)
	}
	if token, err := decoder.Token(); err != nil || token != json.Delim('}') || len(fields) != 0 {
		return ingressLimits{}, errors.New("limit profile must contain all six fields")
	}
	if _, err := decoder.Token(); err != io.EOF {
		return ingressLimits{}, errors.New("unexpected data after limit profile")
	}
	return profile, nil
}

func (p *ingressLimitPolicy) reload(path string) (bool, error) {
	profile, err := readIngressLimits(path)
	if err != nil || *p.profile.Load() == profile {
		return false, err
	}
	p.replace(profile)
	return true, nil
}

func (p *ingressLimitPolicy) watch(ctx context.Context, path string, interval time.Duration) {
	ticker := time.NewTicker(interval)
	defer ticker.Stop()
	failed := false
	for {
		select {
		case <-ctx.Done():
			return
		case <-ticker.C:
			changed, err := p.reload(path)
			if err != nil {
				if !failed {
					log.Printf("could not reload ingress limits; keeping previous profile: %v", err)
				}
				failed = true
				continue
			}
			failed = false
			if changed {
				log.Print("ingress limits reloaded")
			}
		}
	}
}
