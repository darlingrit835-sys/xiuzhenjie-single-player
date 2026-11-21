using System;
using Google.Protobuf.Reflection;

public static class BattleSettingProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static BattleSettingProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChhCYXR0bGVTZXR0aW5nUHJvdG8ucHJvdG8iMAoZRHJlc3NFcXVpcG1lbnRS" + "ZXFfMTAwOTAwMRITCgtlcXVpcG1lbnRJZBgBIAEoCSK3AgoaRHJlc3NFcXVp" + "cG1lbnRSZXNwXzEwMDkwMDESDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MY" + "AiABKAgSHgoWY3VycmVudFBoeXNpcXVlRGVmZW5zZRgDIAEoAxIfChdjdXJy" + "ZW50UHVyZUJsb29kRGVmZW5zZRgEIAEoBRIYChBjdXJyZW50TW92ZVNwZWVk" + "GAUgASgBEhwKFHN1cGVyQWlyUmVjb3ZlclZhbHVlGAYgASgBEhsKE2RlZmVu" + "c2VSZWNvdmVyVmFsdWUYByABKAESJAocc3VwZXJBaXJSZWNvdmVyVmFsdWVP" + "bkJhdHRsZRgIIAEoARIjChtkZWZlbnNlUmVjb3ZlclZhbHVlT25CYXR0bGUY" + "CSABKAESFwoPY2FuVXNlZFBoeXNpcXVlGAogASgBIjIKG1VuZHJlc3NFcXVp" + "cG1lbnRSZXFfMTAwOTAwMhITCgtlcXVpcG1lbnRJZBgBIAEoCSK5AgocVW5k" + "cmVzc0VxdWlwbWVudFJlc3BfMTAwOTAwMhIMCgRjb2RlGAEgASgFEhEKCWlz" + "U3VjY2VzcxgCIAEoCBIeChZjdXJyZW50UGh5c2lxdWVEZWZlbnNlGAMgASgD" + "Eh8KF2N1cnJlbnRQdXJlQmxvb2REZWZlbnNlGAQgASgFEhgKEGN1cnJlbnRN" + "b3ZlU3BlZWQYBSABKAESHAoUc3VwZXJBaXJSZWNvdmVyVmFsdWUYBiABKAES" + "GwoTZGVmZW5zZVJlY292ZXJWYWx1ZRgHIAEoARIkChxzdXBlckFpclJlY292" + "ZXJWYWx1ZU9uQmF0dGxlGAggASgBEiMKG2RlZmVuc2VSZWNvdmVyVmFsdWVP" + "bkJhdHRsZRgJIAEoARIXCg9jYW5Vc2VkUGh5c2lxdWUYCiABKAEiUgoTU2V0" + "U2tpbGxSZXFfMTAwOTAwMxIRCglza2lsbFR5cGUYASABKAUSDwoHc2tpbGxJ" + "ZBgCIAEoCRIXCg9jb21iYXRTZXR0aW5nSWQYAyABKAUibgoUU2V0U2tpbGxS" + "ZXNwXzEwMDkwMDMSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgS" + "GQoRY3VycmVudFBlcmNlcHRpb24YAyABKAUSGgoSZnVsbExvYWRQZXJjZXB0" + "aW9uGAQgASgFIjwKFlJlbW92ZVNraWxsUmVxXzEwMDkwMDQSEQoJc2tpbGxU" + "eXBlGAEgASgFEg8KB3NraWxsSWQYAiABKAkicQoXUmVtb3ZlU2tpbGxSZXNw" + "XzEwMDkwMDQSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSGQoR" + "Y3VycmVudFBlcmNlcHRpb24YAyABKAUSGgoSZnVsbExvYWRQZXJjZXB0aW9u" + "GAQgASgFIvsEChpFbnRlckVxdWlwbWVudFJlc3BfMTAwOTAwNRIMCgRjb2Rl" + "GAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBJQChFkcmVzc2VkRXF1aXBtZW50" + "cxgDIAMoCzI1LkVudGVyRXF1aXBtZW50UmVzcF8xMDA5MDA1LkVxdWlwbWVu" + "dEluZm9SZXNwXzEwMDkwMDYSHgoWY3VycmVudFBoeXNpcXVlRGVmZW5zZRgE" + "IAEoAxIfChdjdXJyZW50UHVyZUJsb29kRGVmZW5zZRgFIAEoBRIaChJmdWxs" + "TG9hZFBlcmNlcHRpb24YBiABKAUSGAoQY3VycmVudE1vdmVTcGVlZBgHIAEo" + "ARIcChRzdXBlckFpclJlY292ZXJWYWx1ZRgIIAEoARIbChNkZWZlbnNlUmVj" + "b3ZlclZhbHVlGAkgASgBEiQKHHN1cGVyQWlyUmVjb3ZlclZhbHVlT25CYXR0" + "bGUYCiABKAESIwobZGVmZW5zZVJlY292ZXJWYWx1ZU9uQmF0dGxlGAsgASgB" + "EhUKDXN1cGVyQWlyUG93ZXIYDCABKAESFgoOY2FuVXNlUGh5c2lxdWUYDSAB" + "KAUavQEKGUVxdWlwbWVudEluZm9SZXNwXzEwMDkwMDYSDwoHZ29vZHNJZBgB" + "IAEoCRIMCgRuYW1lGAIgASgJEhMKC2ZpdmVFbGVtZW50GAMgASgFEgwKBHJh" + "bmsYBCABKAUSDQoFbGV2ZWwYBSABKAUSDAoEdHlwZRgGIAEoBRIMCgRpY29u" + "GAcgASgFEg8KB2JsYW5rSWQYCCABKAkSEgoKdGVtcGxhdGVJZBgJIAEoCRIO" + "CgZpc0xvY2sYCiABKAgi6wIKFkVudGVyU2tpbGxSZXNwXzEwMDkwMDcSDAoE" + "Y29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSRAoNc2V0YWJsZVNraWxs" + "cxgDIAMoCzItLkVudGVyU2tpbGxSZXNwXzEwMDkwMDcuU2tpbGxJbmZvUmVz" + "cF8xMDA5MDA4EkMKDGNhbnVzZVNraWxscxgEIAMoCzItLkVudGVyU2tpbGxS" + "ZXNwXzEwMDkwMDcuU2tpbGxJbmZvUmVzcF8xMDA5MDA4GqQBChVTa2lsbElu" + "Zm9SZXNwXzEwMDkwMDgSDwoHZ29vZHNJZBgBIAEoCRIMCgRuYW1lGAIgASgJ" + "EhMKC2ZpdmVFbGVtZW50GAMgASgFEgwKBHJhbmsYBCABKAUSDQoFbGV2ZWwY" + "BSABKAUSDAoEdHlwZRgGIAEoBRITCgtzZXR0aW5nVHlwZRgHIAEoBRIXCg9j" + "b21iYXRTZXR0aW5nSWQYCCABKAVCNQofY29tLmxvbmdtby5nYW1lc2VydmVy" + "Lm5ldC5wcm90b0ISQmF0dGxlU2V0dGluZ1Byb3RvYgZwcm90bzM="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[10]
		{
			new GeneratedClrTypeInfo(typeof(DressEquipmentReq_1009001), DressEquipmentReq_1009001.Parser, new string[1] { "EquipmentId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(DressEquipmentResp_1009001), DressEquipmentResp_1009001.Parser, new string[10] { "Code", "IsSuccess", "CurrentPhysiqueDefense", "CurrentPureBloodDefense", "CurrentMoveSpeed", "SuperAirRecoverValue", "DefenseRecoverValue", "SuperAirRecoverValueOnBattle", "DefenseRecoverValueOnBattle", "CanUsedPhysique" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(UndressEquipmentReq_1009002), UndressEquipmentReq_1009002.Parser, new string[1] { "EquipmentId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(UndressEquipmentResp_1009002), UndressEquipmentResp_1009002.Parser, new string[10] { "Code", "IsSuccess", "CurrentPhysiqueDefense", "CurrentPureBloodDefense", "CurrentMoveSpeed", "SuperAirRecoverValue", "DefenseRecoverValue", "SuperAirRecoverValueOnBattle", "DefenseRecoverValueOnBattle", "CanUsedPhysique" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SetSkillReq_1009003), SetSkillReq_1009003.Parser, new string[3] { "SkillType", "SkillId", "CombatSettingId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SetSkillResp_1009003), SetSkillResp_1009003.Parser, new string[4] { "Code", "IsSuccess", "CurrentPerception", "FullLoadPerception" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RemoveSkillReq_1009004), RemoveSkillReq_1009004.Parser, new string[2] { "SkillType", "SkillId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RemoveSkillResp_1009004), RemoveSkillResp_1009004.Parser, new string[4] { "Code", "IsSuccess", "CurrentPerception", "FullLoadPerception" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterEquipmentResp_1009005), EnterEquipmentResp_1009005.Parser, new string[13]
			{
				"Code", "IsSuccess", "DressedEquipments", "CurrentPhysiqueDefense", "CurrentPureBloodDefense", "FullLoadPerception", "CurrentMoveSpeed", "SuperAirRecoverValue", "DefenseRecoverValue", "SuperAirRecoverValueOnBattle",
				"DefenseRecoverValueOnBattle", "SuperAirPower", "CanUsePhysique"
			}, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterEquipmentResp_1009005.Types.EquipmentInfoResp_1009006), EnterEquipmentResp_1009005.Types.EquipmentInfoResp_1009006.Parser, new string[10] { "GoodsId", "Name", "FiveElement", "Rank", "Level", "Type", "Icon", "BlankId", "TemplateId", "IsLock" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(EnterSkillResp_1009007), EnterSkillResp_1009007.Parser, new string[4] { "Code", "IsSuccess", "SetableSkills", "CanuseSkills" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterSkillResp_1009007.Types.SkillInfoResp_1009008), EnterSkillResp_1009007.Types.SkillInfoResp_1009008.Parser, new string[8] { "GoodsId", "Name", "FiveElement", "Rank", "Level", "Type", "SettingType", "CombatSettingId" }, null, null, null)
			})
		}));
	}
}
