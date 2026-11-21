package main

import (
	"errors"
	"math"
	"math/rand/v2"
	"strconv"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const (
	spellPracticeCap = 60
	spellInsightAt   = 10
	spellRerollCost  = 6
)

type spellPractice struct {
	effort, force, affix, pending, revision int
}

func practiceFor(role Record, id string) spellPractice {
	book, _ := role["SpellPractice"].(Record)
	entry, _ := book[id].(Record)
	return spellPractice{
		effort: int(roleInt(entry, "Effort")), force: int(roleInt(entry, "Force")),
		affix: int(roleInt(entry, "Affix")), pending: int(roleInt(entry, "Pending")), revision: int(roleInt(entry, "Revision")),
	}
}

func (p spellPractice) stage() int {
	switch {
	case p.effort >= 60:
		return 5
	case p.effort >= 40:
		return 4
	case p.effort >= 25:
		return 3
	case p.effort >= spellInsightAt:
		return 2
	default:
		return 1
	}
}

func (p spellPractice) stats() (power, castReduction int) {
	power = 100 + p.effort + p.force/2
	castReduction = (p.effort - p.force) / 2
	bonus := 8 + 2*max(0, p.stage()-2)
	switch p.affix {
	case 1:
		power += bonus
	case 2:
		castReduction += bonus
	case 3:
		power += bonus / 2
		castReduction += bonus / 2
	}
	return
}

func (p spellPractice) fill(b protoreflect.Message) {
	putPB(b, Record{"Proficiency": 5 * p.effort, "MaxProficiency": 5 * spellPracticeCap, "ForceEffort": p.force,
		"PracticeAffix": p.affix, "PendingAffix": p.pending, "PracticeRevision": p.revision})
}

func (s *Server) openSpellPractice(peer *Peer, msg Message) {
	role := s.roles.Get(peer.currentRoleID())
	out := s.gameResponse("EnterBattleHomeResp_1005038", role != nil, 2000)
	if role == nil {
		statusPB(out, false, 4007)
	} else {
		set(out, "CurrentVigor", roleInt(role, "CurrentVigor"))
		for _, skill := range battleSkills {
			if skill.kind != 1 {
				continue
			}
			p := practiceFor(role, skill.id)
			entry := addMessage(out, "Skill")
			putPB(entry, Record{"SkillId": skill.id, "Name": skill.name, "Type": skill.skillType, "FiveElement": skill.element,
				"Level": max(1, roleInt(role, "Level")), "Rank": 1, "ProficiencyLevel": p.stage(), "BaseCastMS": skill.castMS})
			p.fill(entry)
		}
	}
	s.sendGame(peer, msg, out)
}

// New clients echo the last observed revision. A timed-out or concurrent request
// can never charge the same practice or discard a newly generated candidate twice.
func (s *Server) practiceSpell(peer *Peer, msg Message) {
	out := s.gameResponse("PracticeSkillResp_1005040", false, 4008)
	req, err := s.decode("PracticeSkillReq_1005040", msg.Data)
	if err != nil {
		s.sendGame(peer, msg, out)
		return
	}
	id := getString(req, "MethodSkillId")
	skill, ok := battleSkillByID(id)
	if !ok || skill.kind != 1 {
		statusPB(out, false, 4516)
		s.sendGame(peer, msg, out)
		return
	}
	roleID := peer.currentRoleID()
	legacy := getInt(req, "PracticeAction") == 0 && msg.Msid > 0
	if legacy && msg.Msid <= peer.lastLegacySpellPractice {
		s.sendGame(peer, msg, out)
		return
	}
	err = s.roles.Update(roleID, func(r Record) error {
		p := practiceFor(r, id)
		action := int(getInt(req, "PracticeAction"))
		if action != 0 && p.revision != int(getInt(req, "ExpectedRevision")) {
			return previewReject(4008)
		}
		units := int(getInt(req, "PracticeNum"))
		var cost int
		switch action {
		case 0, 1, 2: // Unversioned clients train the force path.
			if units < 1 || units > spellPracticeCap-p.effort {
				return previewReject(4008)
			}
			cost = units
			if action != 2 {
				p.force += units
			}
			p.effort += units
			if p.effort >= spellInsightAt && p.affix == 0 {
				p.affix = 1
				if action == 2 {
					p.affix = 2
				}
			}
		case 3:
			if units != 0 || p.affix == 0 || p.pending != 0 {
				return previewReject(4008)
			}
			cost = spellRerollCost
			// Uniform among the other two inscriptions. Persist before exposing it.
			p.pending = (p.affix+rand.IntN(2))%3 + 1
		case 4, 5:
			if units != 0 || p.pending == 0 {
				return previewReject(4008)
			}
			if action == 4 {
				p.affix = p.pending
			}
			p.pending = 0
		default:
			return previewReject(4008)
		}
		if !spendArtisanVigor(r, int64(cost)) {
			return previewReject(4290)
		}
		p.revision++
		book, _ := r["SpellPractice"].(Record)
		if book == nil {
			book = Record{}
			r["SpellPractice"] = book
		}
		book[id] = Record{"Effort": p.effort, "Force": p.force, "Affix": p.affix, "Pending": p.pending, "Revision": p.revision}
		vigor := roleInt(r, "CurrentVigor")
		recomputeGamePower(r)
		p.fill(out)
		putPB(out, Record{"CurrentVigor": vigor, "Level": p.stage()})
		for i := 1; i <= 3; i++ {
			live := "Live" + strconv.Itoa(i)
			if roleInt(r, live+"Type") == 4 {
				putPB(out, Record{"LiveLevel": max(1, roleInt(r, live+"Level")), "LiveProficiency": roleInt(r, live+"Value")})
				break
			}
		}
		return nil
	})
	if err != nil {
		var denied previewReject
		switch {
		case errors.As(err, &denied):
			statusPB(out, false, int(denied))
		case errors.Is(err, ErrRecordNotFound):
			statusPB(out, false, 4007)
		default:
			statusPB(out, false, 5000)
		}
	} else {
		if legacy {
			peer.lastLegacySpellPractice = msg.Msid
		}
		statusPB(out, true, 2000)
	}
	s.sendGame(peer, msg, out)
}

func (p spellPractice) castMS(base int) int {
	_, reduction := p.stats()
	return max(1, int(math.Round(float64(base*(100-reduction))/100)))
}
