using System;
using Google.Protobuf.Reflection;

public static class BattleProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static BattleProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChFCYXR0bGVQcm90by5wcm90byI5ChdCdWlsZER1bmdlb25SZXFfMTAxMDAw" + "MRINCgVtYXBJZBgBIAEoCRIPCgdtYXBUeXBlGAIgASgFIpIBChhCdWlsZER1" + "bmdlb25SZXNwXzEwMTAwMDESDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MY" + "AiABKAgSEQoJZHVuZ2VvbklkGAMgASgJEg0KBW1hcElkGAQgASgJEg8KB2Fk" + "ZHJlc3MYBSABKAkSDAoEcG9ydBgGIAEoBRIUCgxjdXJyZW50UG93ZXIYByAB" + "KAUiPgoaUm9sZUpvaW5EdW5nZW9uUmVxXzEwMTAwMDISEQoJZHVuZ2Vvbklk" + "GAEgASgJEg0KBW1hcElkGAIgASgJIvIJChtSb2xlSm9pbkR1bmdlb25SZXNw" + "XzEwMTAwMDISDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSEQoJ" + "ZHVuZ2VvbklkGAMgASgJEg4KBnJvbGVJZBgEIAEoCRIMCgRuYW1lGAUgASgJ" + "Eh4KFmN1cnJlbnRQaHlzaXF1ZURlZmVuc2UYBiABKAMSHwoXY3VycmVudFB1" + "cmVCbG9vZERlZmVuc2UYByABKAUSFwoPY3VycmVudFN1cGVyQWlyGAggASgD" + "EhkKEWN1cnJlbnRQZXJjZXB0aW9uGAkgASgFEhgKEGN1cnJlbnRNb3ZlU3Bl" + "ZWQYCiABKAESDQoFbGV2ZWwYCyABKAUSGwoTcGh5c2lxdWVGaXZlRWxlbWVu" + "dBgMIAEoBRIbChNzdXBlckFpckZpdmVFbGVtZW50GA0gASgFEh0KFXBlcmNl" + "cHRpb25GaXZlRWxlbWVudBgOIAEoBRIMCgRpY29uGA8gASgFEhwKFHN1cGVy" + "QWlyUmVjb3ZlclZhbHVlGBAgASgBEhsKE2RlZmVuc2VSZWNvdmVyVmFsdWUY" + "ESABKAESJAocc3VwZXJBaXJSZWNvdmVyVmFsdWVPbkJhdHRsZRgSIAEoARIj" + "ChtkZWZlbnNlUmVjb3ZlclZhbHVlT25CYXR0bGUYEyABKAESRAoGc2tpbGxz" + "GBQgAygLMjQuUm9sZUpvaW5EdW5nZW9uUmVzcF8xMDEwMDAyLkJhdHRsZVNr" + "aWxsUmVzcF8xMDEwMDAzEhsKE2dvbGREYW1hZ2VBdm9pZGFuY2UYFSABKAES" + "GwoTd29vZERhbWFnZUF2b2lkYW5jZRgWIAEoARIcChR3YXRlckRhbWFnZUF2" + "b2lkYW5jZRgXIAEoARIbChNmaXJlRGFtYWdlQXZvaWRhbmNlGBggASgBEhwK" + "FGVhcnRoRGFtYWdlQXZvaWRhbmNlGBkgASgBEhYKDmF0dGFja0Rpc3RhbmNl" + "GBogASgBGoQEChdCYXR0bGVTa2lsbFJlc3BfMTAxMDAwMxIMCgRpY29uGAEg" + "ASgFEgwKBHR5cGUYAiABKAUSEQoJc2tpbGxUeXBlGAwgASgFEgwKBG5hbWUY" + "AyABKAkSEgoKdGVtcGxhdGVJZBgEIAEoCRIQCghjYXN0VGltZRgFIAEoBRIU" + "Cgxjb250aW51ZVRpbWUYBiABKAUSEAoIY29vbERvd24YByABKAUSEwoLZml2" + "ZUVsZW1lbnQYCCABKAUSFgoOY29udmVyc2lvblJhdGUYCSABKAESEgoKYmFz" + "ZURhbWFnZRgKIAEoARIWCg51c2VkUGVyY2VwdGlvbhgLIAEoBRINCgVsZXZl" + "bBgNIAEoBRISCgpza2lsbEluZGV4GA4gASgFEhQKDHVzZWRTdXBlckFpchgP" + "IAEoAxITCgtjYW5Vc2VDb3VudBgQIAEoBRIMCgRyYW5rGBEgASgFEhAKCHVu" + "aXRVc2VkGBIgASgBEhUKDWZpcmVEYW1hZ2VBZGQYEyABKAESFQoNd29vZERh" + "bWFnZUFkZBgUIAEoARIWCg53YXRlckRhbWFnZUFkZBgVIAEoARIVCg1nb2xk" + "RGFtYWdlQWRkGBYgASgBEhYKDmVhcnRoRGFtYWdlQWRkGBcgASgBEhMKC3Vu" + "aXRDb25zdW1lGBggASgFEg0KBXNjb3BlGBkgASgBIiQKE0NsZWFuTWFwUmVx" + "XzEwMTAwMDQSDQoFbWFwSWQYASABKAkilwMKFENsZWFuTWFwUmVzcF8xMDEw" + "MDA0EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEkIKCWRyb3BH" + "b29kcxgDIAMoCzIvLkNsZWFuTWFwUmVzcF8xMDEwMDA0LkRyb3BHb29kc0lu" + "Zm9SZXNwXzEwMTAwMDUSEQoJZHJvcFN0b25lGAQgASgFEhkKEWN1cnJlbnRT" + "aGlueVN0b25lGAUgASgDEhQKDGN1cnJlbnRQb3dlchgGIAEoBRIQCghiYXR0" + "bGVJZBgHIAEoCRrDAQoZRHJvcEdvb2RzSW5mb1Jlc3BfMTAxMDAwNRIPCgdn" + "b29kc0lkGAEgASgJEgwKBG5hbWUYAiABKAkSEwoLZml2ZUVsZW1lbnQYAyAB" + "KAUSDAoEcmFuaxgEIAEoBRINCgVsZXZlbBgFIAEoBRIMCgR0eXBlGAYgASgF" + "Eg4KBnZvbHVtZRgHIAEoARINCgVjb3VudBgIIAEoBRISCgp0ZW1wbGF0ZUlk" + "GAkgASgJEhQKDHJlY3ljbGVQcmljZRgKIAEoBUIuCh9jb20ubG9uZ21vLmdh" + "bWVzZXJ2ZXIubmV0LnByb3RvQgtCYXR0bGVQcm90b2IGcHJvdG8z"), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[6]
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
				new GeneratedClrTypeInfo(typeof(RoleJoinDungeonResp_1010002.Types.BattleSkillResp_1010003), RoleJoinDungeonResp_1010002.Types.BattleSkillResp_1010003.Parser, new string[25]
				{
					"Icon", "Type", "SkillType", "Name", "TemplateId", "CastTime", "ContinueTime", "CoolDown", "FiveElement", "ConversionRate",
					"BaseDamage", "UsedPerception", "Level", "SkillIndex", "UsedSuperAir", "CanUseCount", "Rank", "UnitUsed", "FireDamageAdd", "WoodDamageAdd",
					"WaterDamageAdd", "GoldDamageAdd", "EarthDamageAdd", "UnitConsume", "Scope"
				}, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(CleanMapReq_1010004), CleanMapReq_1010004.Parser, new string[1] { "MapId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(CleanMapResp_1010004), CleanMapResp_1010004.Parser, new string[7] { "Code", "IsSuccess", "DropGoods", "DropStone", "CurrentShinyStone", "CurrentPower", "BattleId" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(CleanMapResp_1010004.Types.DropGoodsInfoResp_1010005), CleanMapResp_1010004.Types.DropGoodsInfoResp_1010005.Parser, new string[10] { "GoodsId", "Name", "FiveElement", "Rank", "Level", "Type", "Volume", "Count", "TemplateId", "RecyclePrice" }, null, null, null)
			})
		}));
	}
}
