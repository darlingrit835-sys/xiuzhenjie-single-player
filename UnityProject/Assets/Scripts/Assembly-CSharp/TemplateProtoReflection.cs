using System;
using Google.Protobuf.Reflection;

public static class TemplateProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static TemplateProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChNUZW1wbGF0ZVByb3RvLnByb3RvIjUKJ1RlbXBsYXRlTWF0ZXJpYWxBbmlt" + "YWxEZXRhaWxSZXFfMTAxMTAwMRIKCgJpZBgBIAEoCSKGAgooVGVtcGxhdGVN" + "YXRlcmlhbEFuaW1hbERldGFpbFJlc3BfMTAxMTAwMRIMCgRjb2RlGAEgASgF" + "EhEKCWlzU3VjY2VzcxgCIAEoCBIPCgdtYXBOYW1lGAMgASgJEg0KBXN0YWdl" + "GAQgASgFEhMKC2ZpdmVFbGVtZW50GAUgASgFEg8KB2ZvdXJHYXMYBiABKAES" + "EwoLZml2ZUZsYXZvdXIYByABKAESHAoUcmVpbmZvcmNpbmdEaWFycmhvZWEY" + "CCABKAESGwoTYWZmaWxpYXRpb25BY3Vwb2ludBgJIAEoBRIRCglpbnRlbnNp" + "dHkYCiABKAESEAoIdGVuYWNpdHkYCyABKAEiNgooVGVtcGxhdGVNYXRlcmlh" + "bE1pbmVyYWxEZXRhaWxSZXFfMTAxMTAwMhIKCgJpZBgBIAEoCSKmAQopVGVt" + "cGxhdGVNYXRlcmlhbE1pbmVyYWxEZXRhaWxSZXNwXzEwMTEwMDISDAoEY29k" + "ZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSDwoHbWFwTmFtZRgDIAEoCRIN" + "CgVzdGFnZRgEIAEoBRITCgtmaXZlRWxlbWVudBgFIAEoBRIRCglpbnRlbnNp" + "dHkYBiABKAESEAoIdGVuYWNpdHkYByABKAEiNAomVGVtcGxhdGVNYXRlcmlh" + "bFBsYW50RGV0YWlsUmVxXzEwMTEwMDMSCgoCaWQYASABKAkimAIKJ1RlbXBs" + "YXRlTWF0ZXJpYWxQbGFudERldGFpbFJlc3BfMTAxMTAwMxIMCgRjb2RlGAEg" + "ASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIPCgdtYXBOYW1lGAMgASgJEg0KBXN0" + "YWdlGAQgASgFEhMKC2ZpdmVFbGVtZW50GAUgASgFEg8KB2ZvdXJHYXMYBiAB" + "KAESEwoLZml2ZUZsYXZvdXIYByABKAESHAoUcmVpbmZvcmNpbmdEaWFycmhv" + "ZWEYCCABKAESGwoTYWZmaWxpYXRpb25BY3Vwb2ludBgJIAEoBRIRCgljcmVh" + "dGVkQnkYCiABKAkSEQoJaW50ZW5zaXR5GAsgASgBEhAKCHRlbmFjaXR5GAwg" + "ASgBIjAKIk1ldGhvZEFjaGlldmVtZW50RGV0YWlsUmVxXzEwMTEwMDQSCgoC" + "aWQYASABKAki/QIKI01ldGhvZEFjaGlldmVtZW50RGV0YWlsUmVzcF8xMDEx" + "MDA0EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEhMKC2FjdGlv" + "blNjb3BlGAMgASgFEg0KBWxldmVsGAQgASgFEhIKCnN0dWR5TGV2ZWwYBSAB" + "KAUSGgoSYmFzZWRDb25jZW50cmF0aW9uGAYgASgFEhYKDmNvbnZlcnNpb25S" + "YXRlGAcgASgBEhAKCHJ1blNwZWVkGAggASgFEhEKCWNyZWF0ZWRCeRgJIAEo" + "CRITCgtlZmZlY3RUeXBlMRgKIAEoBRIUCgxlZmZlY3RWYWx1ZTEYCyABKAUS" + "EwoLZWZmZWN0VHlwZTIYDCABKAUSFAoMZWZmZWN0VmFsdWUyGA0gASgFEhMK" + "C2VmZmVjdFR5cGUzGA4gASgFEhQKDGVmZmVjdFZhbHVlMxgPIAEoBRIRCgl1" + "c2VkQ291bnQYECABKAUSEAoIbWF4Q291bnQYESABKAUiKwodTWV0aG9kRWxp" + "eGlyRGV0YWlsUmVxXzEwMTEwMDUSCgoCaWQYASABKAkikgIKHk1ldGhvZEVs" + "aXhpckRldGFpbFJlc3BfMTAxMTAwNRIMCgRjb2RlGAEgASgFEhEKCWlzU3Vj" + "Y2VzcxgCIAEoCBITCgtlbGl4aXJMZXZlbBgDIAEoBRISCgplZmZlY3RUeXBl" + "GAQgASgFEhMKC2VmZmVjdFZhbHVlGAUgASgDEhkKEXNwaXJpdHVhbGl0eVZh" + "bHVlGAYgASgDEgwKBHJhbmsYByABKAUSEwoLcG9pc29uVmFsdWUYCCABKAUS" + "EQoJc3RlcENvdW50GAkgASgFEhUKDW1hdGVyaWFsTmFtZXMYCiADKAkSEQoJ" + "Y3JlYXRlZEJ5GAsgASgJEhYKDmNvbnZlcnNpb25SYXRlGAwgASgBIi4KIE1l" + "dGhvZEVxdWlwbWVudERldGFpbFJlcV8xMDExMDA2EgoKAmlkGAEgASgJIuEC" + "CiFNZXRob2RFcXVpcG1lbnREZXRhaWxSZXNwXzEwMTEwMDYSDAoEY29kZRgB" + "IAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSFgoOZXF1aXBtZW50TGV2ZWwYAyAB" + "KAUSHAoUbWluU3Bpcml0dWFsaXR5VmFsdWUYBCABKAMSHAoUbWF4U3Bpcml0" + "dWFsaXR5VmFsdWUYBSABKAMSDwoHbWluUmFuaxgGIAEoBRIPCgdtYXhSYW5r" + "GAcgASgFEhMKC21pbkNvb2xEb3duGAggASgDEhMKC21heENvb2xEb3duGAkg" + "ASgDEhQKDHVzZWRQaHlzaXF1ZRgKIAEoAxIVCg10b3VnaFRlbmFjaXR5GAsg" + "ASgFEhYKDnVzZWRQZXJjZXB0aW9uGAwgASgDEhUKDW1hdGVyaWFsTmFtZXMY" + "DSADKAkSEQoJY3JlYXRlZEJ5GA4gASgJEgwKBHR5cGUYDyABKAUiKwodTWV0" + "aG9kTWF0cml4RGV0YWlsUmVxXzEwMTEwMDcSCgoCaWQYASABKAkiiQIKHk1l" + "dGhvZE1hdHJpeERldGFpbFJlc3BfMTAxMTAwNxIMCgRjb2RlGAEgASgFEhEK" + "CWlzU3VjY2VzcxgCIAEoCBIMCgR0eXBlGAMgASgFEg8KB3R5cGVWYWwYBCAB" + "KAESFgoOY29udmVyc2lvblJhdGUYBSABKAESEwoLdW5pdENvbnN1bWUYBiAB" + "KAESEAoIZHVyYXRpb24YByABKAUSDQoFc2NvcGUYCCABKAESEQoJY3JlYXRl" + "ZEJ5GAkgASgJEg8KB3dhaVF1YW4YCiABKAkSEQoJemhvbmdRdWFuGAsgASgJ" + "Eg8KB25laVF1YW4YDCABKAkSEQoJemhvbmdEaWFuGA0gASgJIikKG01ldGhv" + "ZFNpZ25EZXRhaWxSZXFfMTAxMTAwOBIKCgJpZBgBIAEoCSLAAQocTWV0aG9k" + "U2lnbkRldGFpbFJlc3BfMTAxMTAwOBIMCgRjb2RlGAEgASgFEhEKCWlzU3Vj" + "Y2VzcxgCIAEoCBIRCglza2lsbFR5cGUYAyABKAUSGQoRc3Bpcml0dWFsaXR5" + "VmFsdWUYBCABKAMSFgoOY29udmVyc2lvblJhdGUYBSABKAESFAoMY29udGlu" + "dWVUaW1lGAYgASgFEhAKCGNhc3RUaW1lGAcgASgFEhEKCWNyZWF0ZWRCeRgI" + "IAEoCSIqChxNZXRob2RTa2lsbERldGFpbFJlcV8xMDExMDA5EgoKAmlkGAEg" + "ASgJIuIBCh1NZXRob2RTa2lsbERldGFpbFJlc3BfMTAxMTAwORIMCgRjb2Rl" + "GAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIRCglza2lsbFR5cGUYAyABKAUS" + "HAoUbWluU3Bpcml0dWFsaXR5VmFsdWUYBCABKAMSHAoUbWF4U3Bpcml0dWFs" + "aXR5VmFsdWUYBSABKAMSFAoMY29udGludWVUaW1lGAYgASgFEhAKCGNhc3RU" + "aW1lGAcgASgFEhYKDmNvbnZlcnNpb25SYXRlGAggASgBEhEKCWNyZWF0ZWRC" + "eRgJIAEoCUIwCh9jb20ubG9uZ21vLmdhbWVzZXJ2ZXIubmV0LnByb3RvQg1U" + "ZW1wbGF0ZVByb3RvYgZwcm90bzM="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[18]
		{
			new GeneratedClrTypeInfo(typeof(TemplateMaterialAnimalDetailReq_1011001), TemplateMaterialAnimalDetailReq_1011001.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TemplateMaterialAnimalDetailResp_1011001), TemplateMaterialAnimalDetailResp_1011001.Parser, new string[11]
			{
				"Code", "IsSuccess", "MapName", "Stage", "FiveElement", "FourGas", "FiveFlavour", "ReinforcingDiarrhoea", "AffiliationAcupoint", "Intensity",
				"Tenacity"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(TemplateMaterialMineralDetailReq_1011002), TemplateMaterialMineralDetailReq_1011002.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TemplateMaterialMineralDetailResp_1011002), TemplateMaterialMineralDetailResp_1011002.Parser, new string[7] { "Code", "IsSuccess", "MapName", "Stage", "FiveElement", "Intensity", "Tenacity" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TemplateMaterialPlantDetailReq_1011003), TemplateMaterialPlantDetailReq_1011003.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TemplateMaterialPlantDetailResp_1011003), TemplateMaterialPlantDetailResp_1011003.Parser, new string[12]
			{
				"Code", "IsSuccess", "MapName", "Stage", "FiveElement", "FourGas", "FiveFlavour", "ReinforcingDiarrhoea", "AffiliationAcupoint", "CreatedBy",
				"Intensity", "Tenacity"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodAchievementDetailReq_1011004), MethodAchievementDetailReq_1011004.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodAchievementDetailResp_1011004), MethodAchievementDetailResp_1011004.Parser, new string[17]
			{
				"Code", "IsSuccess", "ActionScope", "Level", "StudyLevel", "BasedConcentration", "ConversionRate", "RunSpeed", "CreatedBy", "EffectType1",
				"EffectValue1", "EffectType2", "EffectValue2", "EffectType3", "EffectValue3", "UsedCount", "MaxCount"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodElixirDetailReq_1011005), MethodElixirDetailReq_1011005.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodElixirDetailResp_1011005), MethodElixirDetailResp_1011005.Parser, new string[12]
			{
				"Code", "IsSuccess", "ElixirLevel", "EffectType", "EffectValue", "SpiritualityValue", "Rank", "PoisonValue", "StepCount", "MaterialNames",
				"CreatedBy", "ConversionRate"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodEquipmentDetailReq_1011006), MethodEquipmentDetailReq_1011006.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodEquipmentDetailResp_1011006), MethodEquipmentDetailResp_1011006.Parser, new string[15]
			{
				"Code", "IsSuccess", "EquipmentLevel", "MinSpiritualityValue", "MaxSpiritualityValue", "MinRank", "MaxRank", "MinCoolDown", "MaxCoolDown", "UsedPhysique",
				"ToughTenacity", "UsedPerception", "MaterialNames", "CreatedBy", "Type"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodMatrixDetailReq_1011007), MethodMatrixDetailReq_1011007.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodMatrixDetailResp_1011007), MethodMatrixDetailResp_1011007.Parser, new string[13]
			{
				"Code", "IsSuccess", "Type", "TypeVal", "ConversionRate", "UnitConsume", "Duration", "Scope", "CreatedBy", "WaiQuan",
				"ZhongQuan", "NeiQuan", "ZhongDian"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodSignDetailReq_1011008), MethodSignDetailReq_1011008.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodSignDetailResp_1011008), MethodSignDetailResp_1011008.Parser, new string[8] { "Code", "IsSuccess", "SkillType", "SpiritualityValue", "ConversionRate", "ContinueTime", "CastTime", "CreatedBy" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodSkillDetailReq_1011009), MethodSkillDetailReq_1011009.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodSkillDetailResp_1011009), MethodSkillDetailResp_1011009.Parser, new string[9] { "Code", "IsSuccess", "SkillType", "MinSpiritualityValue", "MaxSpiritualityValue", "ContinueTime", "CastTime", "ConversionRate", "CreatedBy" }, null, null, null)
		}));
	}
}
