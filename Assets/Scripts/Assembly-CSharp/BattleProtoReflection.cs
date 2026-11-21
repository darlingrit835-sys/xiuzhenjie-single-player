using System;
using Google.Protobuf.Reflection;

public static class BattleProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static BattleProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChFCYXR0bGVQcm90by5wcm90byI5ChdCdWlsZER1bmdlb25SZXFfMTAxMDAwMRINCgVtYXBJ" + "ZBgBIAEoCRIPCgdtYXBUeXBlGAIgASgFIpIBChhCdWlsZER1bmdlb25SZXNwXzEwMTAwMDES" + "DAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSEQoJZHVuZ2VvbklkGAMgASgJEg0K" + "BW1hcElkGAQgASgJEg8KB2FkZHJlc3MYBSABKAkSDAoEcG9ydBgGIAEoBRIUCgxjdXJyZW50" + "UG93ZXIYByABKAUiPgoaUm9sZUpvaW5EdW5nZW9uUmVxXzEwMTAwMDISEQoJZHVuZ2Vvbklk" + "GAEgASgJEg0KBW1hcElkGAIgASgJIqgKChtSb2xlSm9pbkR1bmdlb25SZXNwXzEwMTAwMDIS" + "DAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSEQoJZHVuZ2VvbklkGAMgASgJEg4K" + "BnJvbGVJZBgEIAEoCRIMCgRuYW1lGAUgASgJEh4KFmN1cnJlbnRQaHlzaXF1ZURlZmVuc2UY" + "BiABKAMSHwoXY3VycmVudFB1cmVCbG9vZERlZmVuc2UYByABKAUSFwoPY3VycmVudFN1cGVy" + "QWlyGAggASgDEhkKEWN1cnJlbnRQZXJjZXB0aW9uGAkgASgFEhgKEGN1cnJlbnRNb3ZlU3Bl" + "ZWQYCiABKAESDQoFbGV2ZWwYCyABKAUSGwoTcGh5c2lxdWVGaXZlRWxlbWVudBgMIAEoBRIb" + "ChNzdXBlckFpckZpdmVFbGVtZW50GA0gASgFEh0KFXBlcmNlcHRpb25GaXZlRWxlbWVudBgO" + "IAEoBRIMCgRpY29uGA8gASgFEhwKFHN1cGVyQWlyUmVjb3ZlclZhbHVlGBAgASgBEhsKE2Rl" + "ZmVuc2VSZWNvdmVyVmFsdWUYESABKAESJAocc3VwZXJBaXJSZWNvdmVyVmFsdWVPbkJhdHRs" + "ZRgSIAEoARIjChtkZWZlbnNlUmVjb3ZlclZhbHVlT25CYXR0bGUYEyABKAESRAoGc2tpbGxz" + "GBQgAygLMjQuUm9sZUpvaW5EdW5nZW9uUmVzcF8xMDEwMDAyLkJhdHRsZVNraWxsUmVzcF8x" + "MDEwMDAzEhsKE2dvbGREYW1hZ2VBdm9pZGFuY2UYFSABKAESGwoTd29vZERhbWFnZUF2b2lk" + "YW5jZRgWIAEoARIcChR3YXRlckRhbWFnZUF2b2lkYW5jZRgXIAEoARIbChNmaXJlRGFtYWdl" + "QXZvaWRhbmNlGBggASgBEhwKFGVhcnRoRGFtYWdlQXZvaWRhbmNlGBkgASgBEhYKDmF0dGFj" + "a0Rpc3RhbmNlGBogASgBGroEChdCYXR0bGVTa2lsbFJlc3BfMTAxMDAwMxIMCgRpY29uGAEg" + "ASgFEgwKBHR5cGUYAiABKAUSEQoJc2tpbGxUeXBlGAwgASgFEgwKBG5hbWUYAyABKAkSEgoK" + "dGVtcGxhdGVJZBgEIAEoCRIQCghjYXN0VGltZRgFIAEoBRIUCgxjb250aW51ZVRpbWUYBiAB" + "KAUSEAoIY29vbERvd24YByABKAUSEwoLZml2ZUVsZW1lbnQYCCABKAUSFgoOY29udmVyc2lv" + "blJhdGUYCSABKAESEgoKYmFzZURhbWFnZRgKIAEoARIWCg51c2VkUGVyY2VwdGlvbhgLIAEo" + "BRINCgVsZXZlbBgNIAEoBRISCgpza2lsbEluZGV4GA4gASgFEhQKDHVzZWRTdXBlckFpchgP" + "IAEoAxITCgtjYW5Vc2VDb3VudBgQIAEoBRIMCgRyYW5rGBEgASgFEhAKCHVuaXRVc2VkGBIg" + "ASgBEhUKDWZpcmVEYW1hZ2VBZGQYEyABKAESFQoNd29vZERhbWFnZUFkZBgUIAEoARIWCg53" + "YXRlckRhbWFnZUFkZBgVIAEoARIVCg1nb2xkRGFtYWdlQWRkGBYgASgBEhYKDmVhcnRoRGFt" + "YWdlQWRkGBcgASgBEhMKC3VuaXRDb25zdW1lGBggASgFEg0KBXNjb3BlGBkgASgBEhUKDXBy" + "YWN0aWNlQWZmaXgYGiABKAUSHQoVZm9yZ2VSZXNvbmFuY2VQZXJjZW50GBsgASgFIiQKE0Ns" + "ZWFuTWFwUmVxXzEwMTAwMDQSDQoFbWFwSWQYASABKAkilwMKFENsZWFuTWFwUmVzcF8xMDEw" + "MDA0EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEkIKCWRyb3BHb29kcxgDIAMo" + "CzIvLkNsZWFuTWFwUmVzcF8xMDEwMDA0LkRyb3BHb29kc0luZm9SZXNwXzEwMTAwMDUSEQoJ" + "ZHJvcFN0b25lGAQgASgFEhkKEWN1cnJlbnRTaGlueVN0b25lGAUgASgDEhQKDGN1cnJlbnRQ" + "b3dlchgGIAEoBRIQCghiYXR0bGVJZBgHIAEoCRrDAQoZRHJvcEdvb2RzSW5mb1Jlc3BfMTAx" + "MDAwNRIPCgdnb29kc0lkGAEgASgJEgwKBG5hbWUYAiABKAkSEwoLZml2ZUVsZW1lbnQYAyAB" + "KAUSDAoEcmFuaxgEIAEoBRINCgVsZXZlbBgFIAEoBRIMCgR0eXBlGAYgASgFEg4KBnZvbHVt" + "ZRgHIAEoARINCgVjb3VudBgIIAEoBRISCgp0ZW1wbGF0ZUlkGAkgASgJEhQKDHJlY3ljbGVQ" + "cmljZRgKIAEoBUIuCh9jb20ubG9uZ21vLmdhbWVzZXJ2ZXIubmV0LnByb3RvQgtCYXR0bGVQ" + "cm90b2IGcHJvdG8z"), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[6]
		{
			new GeneratedClrTypeInfo(typeof(BuildDungeonReq_1010001), BuildDungeonReq_1010001.Parser, new string[2] { "MapId", "MapType" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BuildDungeonResp_1010001), BuildDungeonResp_1010001.Parser, new string[7] { "Code", "IsSuccess", "DungeonId", "MapId", "Address", "Port", "CurrentPower" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleJoinDungeonReq_1010002), RoleJoinDungeonReq_1010002.Parser, new string[2] { "DungeonId", "MapId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleJoinDungeonResp_1010002), RoleJoinDungeonResp_1010002.Parser, new string[26]
			{
				"Code", "IsSuccess", "DungeonId", "RoleId", "Name", "CurrentPhysiqueDefense", "CurrentPureBloodDefense", "CurrentSuperAir", "CurrentPerception", "CurrentMoveSpeed",
				"Level", "PhysiqueFiveElement", "SuperAirFiveElement", "PerceptionFiveElement", "Icon", "SuperAirRecoverValue", "DefenseRecoverValue", "SuperAirRecoverValueOnBattle", "DefenseRecoverValueOnBattle", "Skills",
				"GoldDamageAvoidance", "WoodDamageAvoidance", "WaterDamageAvoidance", "FireDamageAvoidance", "EarthDamageAvoidance", "AttackDistance"
			}, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(RoleJoinDungeonResp_1010002.Types.BattleSkillResp_1010003), RoleJoinDungeonResp_1010002.Types.BattleSkillResp_1010003.Parser, new string[27]
				{
					"Icon", "Type", "SkillType", "Name", "TemplateId", "CastTime", "ContinueTime", "CoolDown", "FiveElement", "ConversionRate",
					"BaseDamage", "UsedPerception", "Level", "SkillIndex", "UsedSuperAir", "CanUseCount", "Rank", "UnitUsed", "FireDamageAdd", "WoodDamageAdd",
					"WaterDamageAdd", "GoldDamageAdd", "EarthDamageAdd", "UnitConsume", "Scope", "PracticeAffix", "ForgeResonancePercent"}, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(CleanMapReq_1010004), CleanMapReq_1010004.Parser, new string[1] { "MapId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(CleanMapResp_1010004), CleanMapResp_1010004.Parser, new string[7] { "Code", "IsSuccess", "DropGoods", "DropStone", "CurrentShinyStone", "CurrentPower", "BattleId" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(CleanMapResp_1010004.Types.DropGoodsInfoResp_1010005), CleanMapResp_1010004.Types.DropGoodsInfoResp_1010005.Parser, new string[10] { "GoodsId", "Name", "FiveElement", "Rank", "Level", "Type", "Volume", "Count", "TemplateId", "RecyclePrice" }, null, null, null)
			})
		}));
	}
}
