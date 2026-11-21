using System;
using Google.Protobuf.Reflection;

public static class SimpleActionProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static SimpleActionProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChdTaW1wbGVBY3Rpb25Qcm90by5wcm90byIaChhBY3Rpb25Ob1BhcmFtUmVx" + "XzEwMDEwMDEiOQoWQWN0aW9uQm9vbFJlc3BfMTAwMTAwMRIMCgRjb2RlGAEg" + "ASgFEhEKCWlzU3VjY2VzcxgCIAEoCCJTChtMb29rQWR2ZXJ0aXNpbmdSZXNw" + "XzEwMDEwMDMSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSEwoL" + "Y3VycmVudEphZGUYAyABKAUiYgoUQnV5WXVla2FSZXNwXzEwMDEwMDQSDAoE" + "Y29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSEwoLY3VycmVudEphZGUY" + "AyABKAUSFAoMcmVtYWluaW5nRGF5GAQgASgFIjcKG1VuZGVyc3RhbmRNZXRo" + "b2RSZXFfMTAwMTAwNRIKCgJpZBgBIAEoCRIMCgR0eXBlGAIgASgFIlUKHlVz" + "ZWRBY2hpZXZlbWVudFBhcnRSZXFfMTAwMTAwNhIKCgJpZBgBIAEoCRIRCglw" + "YXJ0Q291bnQYAiABKAUSFAoMZGFvSmluZ0NvdW50GAMgASgFIp0BCh9Vc2Vk" + "QWNoaWV2ZW1lbnRQYXJ0UmVzcF8xMDAxMDA2EgwKBGNvZGUYASABKAUSEQoJ" + "aXNTdWNjZXNzGAIgASgIEhQKDGN1cnJlbnRMZXZlbBgDIAEoBRIaChJiYXNl" + "ZENvbmNlbnRyYXRpb24YBCABKAESEQoJcGFydENvdW50GAUgASgFEhQKDGRh" + "b0ppbmdDb3VudBgGIAEoBSIzChxDaGFuZ2VSdW5Mb2NhdGlvblJlcV8xMDAx" + "MDA3EhMKC25ld0xvY2F0aW9uGAEgASgFIk8KGFJvbGVCcmVha0Fza1Jlc3Bf" + "MTAwMTAwOBIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBISCgpo" + "YXNUaHVuZGVyGAMgASgFIokBCiFDaGFuZ2VDb250cm9sbGVyTWF0cml4UmVx" + "XzEwMDEwMDkSEgoKZ29sZElzT3BlbhgBIAEoCBISCgp3b29kSXNPcGVuGAIg" + "ASgIEhMKC3dhdGVySXNPcGVuGAMgASgIEhIKCmZpcmVJc09wZW4YBCABKAgS" + "EwoLZWFydGhJc09wZW4YBSABKAgiZgoiQ2hhbmdlQ29udHJvbGxlck1hdHJp" + "eFJlc3BfMTAwMTAwORIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEo" + "CBIfChdjdXJyZW50U3VwZXJBaXJSdW5WYWx1ZRgDIAEoASJNChlSb2xlQnJl" + "YWtSZWFsbVJlcV8xMDAxMDEwEhgKEGlzVGh1bmRlclN1Y2Nlc3MYASABKAgS" + "FgoOZmFpbFRodW5kZXJOdW0YAiABKAUi7wIKGlJvbGVCcmVha1JlYWxtUmVz" + "cF8xMDAxMDEwEgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEhEK" + "CXB1cmVCbG9vZBgDIAEoBRIUCgxmbGV4aWJsZVJvb3QYBCABKAkSEAoIc3Vw" + "ZXJBaXIYBSABKAMSEgoKcGVyY2VwdGlvbhgGIAEoAxIQCghwaHlzaXF1ZRgH" + "IAEoAxIXCg9jdXJyZW50U3VwZXJBaXIYCCABKAMSGQoRY3VycmVudFBlcmNl" + "cHRpb24YCSABKAMSFwoPY3VycmVudFBoeXNpcXVlGAogASgDEhQKDGN1cnJl" + "bnRWaWdvchgLIAEoBRIVCg1jdXJyZW50Q2xldmVyGAwgASgREhgKEGN1cnJl" + "bnRQb2lzb25WYWwYDSABKAUSGAoQY3VycmVudFB1cmVCbG9vZBgOIAEoBRIL" + "CgNhZ2UYDyABKAUSFAoMY3VycmVudFBvd2VyGBAgASgFIq4CCh9PcGVuVW5k" + "ZXJzdGFuZFBhbmVsUmVzcF8xMDAxMDExEgwKBGNvZGUYASABKAUSEQoJaXNT" + "dWNjZXNzGAIgASgIElMKBm9iamVjdBgDIAMoCzJDLk9wZW5VbmRlcnN0YW5k" + "UGFuZWxSZXNwXzEwMDEwMTEuVW5kZXJzdGFuZE1ldGhvZE9iamVjdFJlc3Bf" + "MTAwMTAxMhqUAQoiVW5kZXJzdGFuZE1ldGhvZE9iamVjdFJlc3BfMTAwMTAx" + "MhIUCgxpc1VuZGVyc3RhbmQYASABKAgSDAoEbmFtZRgCIAEoCRITCgtmaXZl" + "RWxlbWVudBgDIAEoBRINCgVsZXZlbBgEIAEoBRIMCgRyYW5rGAUgASgFEgwK" + "BHR5cGUYBiABKAUSCgoCaWQYByABKAkiPQohVW5kZXJzdGFuZE1ldGhvZEZv" + "cmdldFJlcV8xMDAxMDEzEgoKAmlkGAEgASgJEgwKBHR5cGUYAiABKAUiJAoV" + "QnJvYWRjYXN0UmVzcF8xMDAxMDE0EgsKA21zZxgBIAEoCSIrChpVbmRlcnN0" + "YW5kU2tpbGxSZXFfMTAwMTAxNRINCgVzdGFnZRgBIAEoBSLHAQobVW5kZXJz" + "dGFuZFNraWxsUmVzcF8xMDAxMDE1EgwKBGNvZGUYASABKAUSEQoJaXNTdWNj" + "ZXNzGAIgASgIEkQKBnNraWxscxgDIAMoCzI0LlVuZGVyc3RhbmRTa2lsbFJl" + "c3BfMTAwMTAxNS5Ta2lsbE9iamVjdFJlc3BfMTAwMTAwMhpBChdTa2lsbE9i" + "amVjdFJlc3BfMTAwMTAwMhIKCgJpZBgBIAEoCRIMCgRuYW1lGAIgASgJEgwK" + "BHJhbmsYAyABKAUiiQEKF0J1eVl1ZWthQ255UmVzcF8xMDAxMDE2EgwKBGNv" + "ZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEhMKC2N1cnJlbnRKYWRlGAMg" + "ASgFEhQKDHJlbWFpbmluZ0RheRgEIAEoBRIQCghsZWlDaG9uZxgFIAEoBRIQ" + "Cgh2aXBMZXZlbBgGIAEoBSJyChZCdXlKYWRlQ255UmVzcF8xMDAxMDE3EgwK" + "BGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEhMKC2N1cnJlbnRKYWRl" + "GAMgASgFEhAKCGxlaUNob25nGAQgASgFEhAKCHZpcExldmVsGAUgASgFIiMK" + "E0J1eUxpQmFvUmVxXzEwMDEwMTgSDAoEdHlwZRgBIAEoBSJMChRCdXlMaUJh" + "b1Jlc3BfMTAwMTAxOBIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEo" + "CBITCgtjdXJyZW50SmFkZRgDIAEoBSJsChdCdXlMaUJhb0NueVJlc3BfMTAw" + "MTAxORIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIMCgR0eXBl" + "GAMgASgFEhAKCGxlaUNob25nGAQgASgFEhAKCHZpcExldmVsGAUgASgFIkQK" + "K1VwZ3JhZGVBY2hpZXZlbWVudENvbnZlcnNpb25SYXRlUmVxXzEwMDEwMjAS" + "FQoNYWNoaWV2ZW1lbnRJZBgBIAEoCSKLAgosVXBncmFkZUFjaGlldmVtZW50" + "Q29udmVyc2lvblJhdGVSZXNwXzEwMDEwMjASDAoEY29kZRgBIAEoBRIRCglp" + "c1N1Y2Nlc3MYAiABKAgSDAoEcmFuaxgDIAEoBRIWCg5jb252ZXJzaW9uUmF0" + "ZRgEIAEoARITCgtlZmZlY3RUeXBlMRgFIAEoBRIUCgxlZmZlY3RWYWx1ZTEY" + "BiABKAUSEwoLZWZmZWN0VHlwZTIYByABKAUSFAoMZWZmZWN0VmFsdWUyGAgg" + "ASgFEhMKC2VmZmVjdFR5cGUzGAkgASgFEhQKDGVmZmVjdFZhbHVlMxgKIAEo" + "BRITCgtub3dDYXBhY2l0eRgLIAEoBSJACilVcGdyYWRlRXF1aXBtZW50Q29u" + "dmVyc2lvblJhdGVSZXFfMTAwMTAyMRITCgtlcXVpcG1lbnRJZBgBIAEoCSKJ" + "AgoqVXBncmFkZUVxdWlwbWVudENvbnZlcnNpb25SYXRlUmVzcF8xMDAxMDIx" + "EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEgwKBHJhbmsYAyAB" + "KAUSFgoOY29udmVyc2lvblJhdGUYBCABKAESEwoLZWZmZWN0VHlwZTEYBSAB" + "KAUSFAoMZWZmZWN0VmFsdWUxGAYgASgFEhMKC2VmZmVjdFR5cGUyGAcgASgF" + "EhQKDGVmZmVjdFZhbHVlMhgIIAEoBRITCgtlZmZlY3RUeXBlMxgJIAEoBRIU" + "CgxlZmZlY3RWYWx1ZTMYCiABKAUSEwoLbm93Q2FwYWNpdHkYCyABKAUiOAol" + "VXBncmFkZVNraWxsQ29udmVyc2lvblJhdGVSZXFfMTAwMTAyMhIPCgdza2ls" + "bElkGAEgASgJIoQBCiZVcGdyYWRlU2tpbGxDb252ZXJzaW9uUmF0ZVJlc3Bf" + "MTAwMTAyMhIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIMCgRy" + "YW5rGAMgASgFEhYKDmNvbnZlcnNpb25SYXRlGAQgASgBEhMKC25vd0NhcGFj" + "aXR5GAUgASgFQjQKH2NvbS5sb25nbW8uZ2FtZXNlcnZlci5uZXQucHJvdG9C" + "EVNpbXBsZUFjdGlvblByb3RvYgZwcm90bzM="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[29]
		{
			new GeneratedClrTypeInfo(typeof(ActionNoParamReq_1001001), ActionNoParamReq_1001001.Parser, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ActionBoolResp_1001001), ActionBoolResp_1001001.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(LookAdvertisingResp_1001003), LookAdvertisingResp_1001003.Parser, new string[3] { "Code", "IsSuccess", "CurrentJade" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BuyYuekaResp_1001004), BuyYuekaResp_1001004.Parser, new string[4] { "Code", "IsSuccess", "CurrentJade", "RemainingDay" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(UnderstandMethodReq_1001005), UnderstandMethodReq_1001005.Parser, new string[2] { "Id", "Type" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(UsedAchievementPartReq_1001006), UsedAchievementPartReq_1001006.Parser, new string[3] { "Id", "PartCount", "DaoJingCount" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(UsedAchievementPartResp_1001006), UsedAchievementPartResp_1001006.Parser, new string[6] { "Code", "IsSuccess", "CurrentLevel", "BasedConcentration", "PartCount", "DaoJingCount" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChangeRunLocationReq_1001007), ChangeRunLocationReq_1001007.Parser, new string[1] { "NewLocation" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleBreakAskResp_1001008), RoleBreakAskResp_1001008.Parser, new string[3] { "Code", "IsSuccess", "HasThunder" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChangeControllerMatrixReq_1001009), ChangeControllerMatrixReq_1001009.Parser, new string[5] { "GoldIsOpen", "WoodIsOpen", "WaterIsOpen", "FireIsOpen", "EarthIsOpen" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChangeControllerMatrixResp_1001009), ChangeControllerMatrixResp_1001009.Parser, new string[3] { "Code", "IsSuccess", "CurrentSuperAirRunValue" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleBreakRealmReq_1001010), RoleBreakRealmReq_1001010.Parser, new string[2] { "IsThunderSuccess", "FailThunderNum" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleBreakRealmResp_1001010), RoleBreakRealmResp_1001010.Parser, new string[16]
			{
				"Code", "IsSuccess", "PureBlood", "FlexibleRoot", "SuperAir", "Perception", "Physique", "CurrentSuperAir", "CurrentPerception", "CurrentPhysique",
				"CurrentVigor", "CurrentClever", "CurrentPoisonVal", "CurrentPureBlood", "Age", "CurrentPower"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(OpenUnderstandPanelResp_1001011), OpenUnderstandPanelResp_1001011.Parser, new string[3] { "Code", "IsSuccess", "Object" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(OpenUnderstandPanelResp_1001011.Types.UnderstandMethodObjectResp_1001012), OpenUnderstandPanelResp_1001011.Types.UnderstandMethodObjectResp_1001012.Parser, new string[7] { "IsUnderstand", "Name", "FiveElement", "Level", "Rank", "Type", "Id" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(UnderstandMethodForgetReq_1001013), UnderstandMethodForgetReq_1001013.Parser, new string[2] { "Id", "Type" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BroadcastResp_1001014), BroadcastResp_1001014.Parser, new string[1] { "Msg" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(UnderstandSkillReq_1001015), UnderstandSkillReq_1001015.Parser, new string[1] { "Stage" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(UnderstandSkillResp_1001015), UnderstandSkillResp_1001015.Parser, new string[3] { "Code", "IsSuccess", "Skills" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(UnderstandSkillResp_1001015.Types.SkillObjectResp_1001002), UnderstandSkillResp_1001015.Types.SkillObjectResp_1001002.Parser, new string[3] { "Id", "Name", "Rank" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(BuyYuekaCnyResp_1001016), BuyYuekaCnyResp_1001016.Parser, new string[6] { "Code", "IsSuccess", "CurrentJade", "RemainingDay", "LeiChong", "VipLevel" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BuyJadeCnyResp_1001017), BuyJadeCnyResp_1001017.Parser, new string[5] { "Code", "IsSuccess", "CurrentJade", "LeiChong", "VipLevel" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BuyLiBaoReq_1001018), BuyLiBaoReq_1001018.Parser, new string[1] { "Type" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BuyLiBaoResp_1001018), BuyLiBaoResp_1001018.Parser, new string[3] { "Code", "IsSuccess", "CurrentJade" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BuyLiBaoCnyResp_1001019), BuyLiBaoCnyResp_1001019.Parser, new string[5] { "Code", "IsSuccess", "Type", "LeiChong", "VipLevel" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(UpgradeAchievementConversionRateReq_1001020), UpgradeAchievementConversionRateReq_1001020.Parser, new string[1] { "AchievementId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(UpgradeAchievementConversionRateResp_1001020), UpgradeAchievementConversionRateResp_1001020.Parser, new string[11]
			{
				"Code", "IsSuccess", "Rank", "ConversionRate", "EffectType1", "EffectValue1", "EffectType2", "EffectValue2", "EffectType3", "EffectValue3",
				"NowCapacity"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(UpgradeEquipmentConversionRateReq_1001021), UpgradeEquipmentConversionRateReq_1001021.Parser, new string[1] { "EquipmentId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(UpgradeEquipmentConversionRateResp_1001021), UpgradeEquipmentConversionRateResp_1001021.Parser, new string[11]
			{
				"Code", "IsSuccess", "Rank", "ConversionRate", "EffectType1", "EffectValue1", "EffectType2", "EffectValue2", "EffectType3", "EffectValue3",
				"NowCapacity"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(UpgradeSkillConversionRateReq_1001022), UpgradeSkillConversionRateReq_1001022.Parser, new string[1] { "SkillId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(UpgradeSkillConversionRateResp_1001022), UpgradeSkillConversionRateResp_1001022.Parser, new string[5] { "Code", "IsSuccess", "Rank", "ConversionRate", "NowCapacity" }, null, null, null)
		}));
	}
}
