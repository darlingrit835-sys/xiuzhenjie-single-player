package main

import (
	"fmt"
	"slices"
	"strings"
)

// Each running server uses the catalogue from its own data directory.
type methodCatalog struct {
	all    []gameMethod
	byID   map[string]*gameMethod
	byName map[string]*gameMethod
}

func indexMethods(all []gameMethod) *methodCatalog {
	c := &methodCatalog{all: all, byID: make(map[string]*gameMethod, len(all)), byName: make(map[string]*gameMethod, len(all))}
	for i := range c.all {
		m := &c.all[i]
		c.byID[m.id], c.byName[m.name] = m, m
	}
	return c
}

var legacyMethods = indexMethods(buildGameMethods())

func (s *Server) methods() *methodCatalog {
	if s.game.methods != nil {
		return s.game.methods
	}
	return legacyMethods
}

func (c *methodCatalog) equipped(r Record, kind int) *gameMethod {
	def := c.byID[roleString(r, gameEquipField(kind))]
	if def == nil || def.kind != kind {
		return nil
	}
	return def
}

func (c *methodCatalog) book(g Record) *gameMethod {
	if def := c.byID[roleString(g, "TemplateId")]; def != nil {
		return def
	}
	return c.byName[roleString(g, "Name")]
}

type systemMethodRow struct {
	ID, Name                    string
	Polarity, Element, Qi, Kind int
	Rank, LearningLevel         int
	BaseCircle                  float64
	Effect1, Effect2            int
	EffectValue1, EffectValue2  int
	MoveSpeedPercent            int
}

func (s *Server) loadSystemMethods() error {
	var table *struct{ Methods []systemMethodRow }
	if err := s.readGameTable("system_cultivation_methods.json", &table); err != nil {
		return err
	}
	if table == nil {
		return nil // Older data directories still contain the original library.
	}
	if len(table.Methods) != 2*5*12 {
		return fmt.Errorf("系统传承表共 %d 门，应包含全部 120 种阴阳、五行与主气组合", len(table.Methods))
	}
	all := slices.Clone(legacyMethods.all)
	seenID := make(map[string]bool)
	seenName := make(map[string]bool)
	for _, row := range table.Methods {
		id := fmt.Sprintf("sys_%d_%d_%d", row.Polarity, row.Element, row.Qi)
		if row.ID != id || row.Polarity < 1 || row.Polarity > 2 || row.Element < 1 || row.Element > 5 ||
			row.Qi < 1 || row.Qi > 12 || row.Kind < 1 || row.Kind > 3 || row.Rank < 1 || row.Rank > 15 ||
			row.Rank%4 == 0 || row.LearningLevel != 1+10*((row.Rank-1)/4) || row.BaseCircle <= 0 ||
			row.MoveSpeedPercent < 0 || row.MoveSpeedPercent > 35 ||
			strings.TrimSpace(row.Name) == "" || seenID[id] || seenName[row.Name] || legacyMethods.byName[row.Name] != nil {
			return fmt.Errorf("系统传承的属性、名称或编号无效: %q (%s)", row.Name, row.ID)
		}
		seenID[id], seenName[row.Name] = true, true
		all = append(all, gameMethod{id: id, name: row.Name, kind: row.Kind, rank: row.Rank,
			element: row.Element, learningLevel: row.LearningLevel, baseCircle: row.BaseCircle,
			effect1: row.Effect1, effect2: row.Effect2, effectValue1: row.EffectValue1, effectValue2: row.EffectValue2,
			moveSpeedPercent: row.MoveSpeedPercent, system: true})
	}
	s.game.methods = indexMethods(all)
	return nil
}
