package main

import (
	"strings"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func isSmithingEquipment(goods Record) bool {
	switch roleInt(goods, "Type") {
	case 15, 16, 26, 29:
		return strings.HasPrefix(roleString(goods, "TemplateId"), "ugc.")
	}
	return false
}

// Shared by smithing creation and the original 1004018 equipment detail window.
func buildSmithingEquipmentFields(template Record) Record {
	rank := max(1, roleInt(template, "Rank"))
	fields := Record{
		"MinSpiritualityValue": 10 * rank, "MaxSpiritualityValue": 100 * rank,
		"ToughTenacity": roleInt(template, "BaseDefense"), "ConversionRate": 1.0, "Volume": 1.0,
	}
	for _, key := range []string{"CreatedBy", "BlankId", "ClassId", "PolarityId", "QiId", "BaseAttack", "BaseDefense", "Bonuses", "MaterialAffixes", "MoldQuality", "MoldId", "ShapeBalance"} {
		if template[key] != nil {
			fields[key] = cloneRecordValue(template[key])
		}
	}
	return fields
}

// Field 18 carries the public template ID. Updated bag and market windows use
// it to fetch all affixes from the same template detail service.
const smithingEquipmentField = 18

func fillSmithingEquipmentDetail(out protoreflect.Message, goods Record) {
	copyRolePB(out, goods, "CreatedBy", "MinSpiritualityValue", "MaxSpiritualityValue", "ConversionRate", "Volume",
		"ToughTenacity", "UsedPerception", "CoolDown", "RechooseCount")
	out.SetUnknown(protowire.AppendString(protowire.AppendTag(out.GetUnknown(), smithingEquipmentField, protowire.BytesType), roleString(goods, "TemplateId")))
}
