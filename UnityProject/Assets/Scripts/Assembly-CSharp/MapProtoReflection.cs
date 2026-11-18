using System;
using Google.Protobuf.Reflection;

public static class MapProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static MapProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("Cg5NYXBQcm90by5wcm90byImChVTaWJpbmdzTWFwUmVxXzEwMDcwMDESDQoF" + "bWFwSWQYASABKAkinQMKFlNpYmluZ3NNYXBSZXNwXzEwMDcwMDESDAoEY29k" + "ZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSPQoGb2JqZWN0GAMgAygLMi0u" + "U2liaW5nc01hcFJlc3BfMTAwNzAwMS5NYXBPYmplY3RSZXNwXzEwMDcwMDIa" + "ogIKFU1hcE9iamVjdFJlc3BfMTAwNzAwMhIKCgJpZBgBIAEoCRIQCghwYXJl" + "bnRJZBgCIAEoCRISCgpwYXJlbnROYW1lGAMgASgJEgwKBG5hbWUYBCABKAkS" + "EQoJc2liaW5nc0lkGAUgASgJEhMKC2NoaWxkcmVuSWRzGAYgASgJEgwKBHR5" + "cGUYByABKAUSEAoIbWluTGV2ZWwYCCABKAUSEAoIbWF4TGV2ZWwYCSABKAUS" + "DgoGc3RhdHVzGAogASgFEhAKCHJlc3RyaWN0GAsgASgJEhoKEmJhc2VkQ29u" + "Y2VudHJhdGlvbhgMIAEoARIUCgxmaXZlRWxlbWVudHMYDSABKAUSDAoEc29y" + "dBgOIAEoBRINCgVsZXZlbBgPIAEoBSInChZDaGlsZHJlbk1hcFJlcV8xMDA3" + "MDAzEg0KBW1hcElkGAEgASgJIp8DChdDaGlsZHJlbk1hcFJlc3BfMTAwNzAw" + "MxIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBI+CgZvYmplY3QY" + "AyADKAsyLi5DaGlsZHJlbk1hcFJlc3BfMTAwNzAwMy5NYXBPYmplY3RSZXNw" + "XzEwMDcwMDQaogIKFU1hcE9iamVjdFJlc3BfMTAwNzAwNBIKCgJpZBgBIAEo" + "CRIQCghwYXJlbnRJZBgCIAEoCRISCgpwYXJlbnROYW1lGAMgASgJEgwKBG5h" + "bWUYBCABKAkSEQoJc2liaW5nc0lkGAUgASgJEhMKC2NoaWxkcmVuSWRzGAYg" + "ASgJEgwKBHR5cGUYByABKAUSEAoIbWluTGV2ZWwYCCABKAUSEAoIbWF4TGV2" + "ZWwYCSABKAUSDgoGc3RhdHVzGAogASgFEhAKCHJlc3RyaWN0GAsgASgJEhoK" + "EmJhc2VkQ29uY2VudHJhdGlvbhgMIAEoARIUCgxmaXZlRWxlbWVudHMYDSAB" + "KAUSDAoEc29ydBgOIAEoBRINCgVsZXZlbBgPIAEoBSInChZQYXJlbnRNYXBJ" + "ZFJlcV8xMDA3MDA1Eg0KBW1hcElkGAEgASgJIkYKF1BhcmVudE1hcElkUmVz" + "cF8xMDA3MDA1EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEgoK" + "AmlkGAMgAygJIiUKFFBhcmVudE1hcFJlcV8xMDA3MDA2Eg0KBW1hcElkGAEg" + "ASgJIpsDChVQYXJlbnRNYXBSZXNwXzEwMDcwMDYSDAoEY29kZRgBIAEoBRIR" + "Cglpc1N1Y2Nlc3MYAiABKAgSPAoGb2JqZWN0GAMgAygLMiwuUGFyZW50TWFw" + "UmVzcF8xMDA3MDA2Lk1hcE9iamVjdFJlc3BfMTAwNzAwNxqiAgoVTWFwT2Jq" + "ZWN0UmVzcF8xMDA3MDA3EgoKAmlkGAEgASgJEhAKCHBhcmVudElkGAIgASgJ" + "EhIKCnBhcmVudE5hbWUYAyABKAkSDAoEbmFtZRgEIAEoCRIRCglzaWJpbmdz" + "SWQYBSABKAkSEwoLY2hpbGRyZW5JZHMYBiABKAkSDAoEdHlwZRgHIAEoBRIQ" + "CghtaW5MZXZlbBgIIAEoBRIQCghtYXhMZXZlbBgJIAEoBRIOCgZzdGF0dXMY" + "CiABKAUSEAoIcmVzdHJpY3QYCyABKAkSGgoSYmFzZWRDb25jZW50cmF0aW9u" + "GAwgASgBEhQKDGZpdmVFbGVtZW50cxgNIAEoBRIMCgRzb3J0GA4gASgFEg0K" + "BWxldmVsGA8gASgFIlQKF0FuY2VzdG9yTWFwUmVzcF8xMDA3MDA4EgwKBGNv" + "ZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEgoKAmlkGAMgASgJEgwKBG5h" + "bWUYBCABKAkiIwoSTWFwSW5mb1JlcV8xMDA3MDA5Eg0KBW1hcElkGAEgASgJ" + "IsECChNNYXBJbmZvUmVzcF8xMDA3MDA5EgwKBGNvZGUYASABKAUSEQoJaXNT" + "dWNjZXNzGAIgASgIEgoKAmlkGAMgASgJEhAKCHBhcmVudElkGAQgASgJEhIK" + "CnBhcmVudE5hbWUYBSABKAkSDAoEbmFtZRgGIAEoCRIRCglzaWJpbmdzSWQY" + "ByABKAkSEwoLY2hpbGRyZW5JZHMYCCABKAkSDAoEdHlwZRgJIAEoBRIQCght" + "aW5MZXZlbBgKIAEoBRIQCghtYXhMZXZlbBgLIAEoBRIOCgZzdGF0dXMYDCAB" + "KAUSEAoIcmVzdHJpY3QYDSABKAkSGgoSYmFzZWRDb25jZW50cmF0aW9uGA4g" + "ASgBEhQKDGZpdmVFbGVtZW50cxgPIAEoBRIMCgRzb3J0GBAgASgFEg0KBWxl" + "dmVsGBEgASgFIicKFk1hcFRyYW5zbWl0UmVxXzEwMDcwMTASDQoFbWFwSWQY" + "ASABKAkiTgoXTWFwVHJhbnNtaXRSZXNwXzEwMDcwMTASDAoEY29kZRgBIAEo" + "BRIRCglpc1N1Y2Nlc3MYAiABKAgSEgoKc2hpbnlTdG9uZRgDIAEoAyIqChlN" + "YXBDaXR5TWlncmF0ZVJlcV8xMDA3MDExEg0KBW1hcElkGAEgASgJImcKGk1h" + "cENpdHlNaWdyYXRlUmVzcF8xMDA3MDExEgwKBGNvZGUYASABKAUSEQoJaXNT" + "dWNjZXNzGAIgASgIEhIKCmN1cnJlbnRBZ2UYAyABKAUSFAoMY3VycmVudFBv" + "d2VyGAQgASgFIjIKIVBhcmVudE1hcEFuZE9uZXNlbGZNYXBSZXFfMTAwNzAx" + "MhINCgVtYXBJZBgBIAEoCSJTCiJQYXJlbnRNYXBBbmRPbmVzZWxmTWFwUmVz" + "cF8xMDA3MDEyEgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEgwK" + "BG5hbWUYAyADKAkiLQocTGlMaWFuQ2hpbGRyZW5NYXBSZXFfMTAwNzAxMxIN" + "CgVtYXBJZBgBIAEoCSLLAwodTGlMaWFuQ2hpbGRyZW5NYXBSZXNwXzEwMDcw" + "MTMSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSSgoGb2JqZWN0" + "GAMgAygLMjouTGlMaWFuQ2hpbGRyZW5NYXBSZXNwXzEwMDcwMTMuTGlMaWFu" + "TWFwT2JqZWN0UmVzcF8xMDA3MDE0GrwCChtMaUxpYW5NYXBPYmplY3RSZXNw" + "XzEwMDcwMTQSCgoCaWQYASABKAkSEAoIcGFyZW50SWQYAiABKAkSEgoKcGFy" + "ZW50TmFtZRgDIAEoCRIMCgRuYW1lGAQgASgJEhEKCXNpYmluZ3NJZBgFIAEo" + "CRITCgtjaGlsZHJlbklkcxgGIAEoCRIMCgR0eXBlGAcgASgFEhAKCG1pbkxl" + "dmVsGAggASgFEhAKCG1heExldmVsGAkgASgFEg4KBnN0YXR1cxgKIAEoBRIQ" + "CghyZXN0cmljdBgLIAEoCRIaChJiYXNlZENvbmNlbnRyYXRpb24YDCABKAES" + "FAoMZml2ZUVsZW1lbnRzGA0gASgFEgwKBHNvcnQYDiABKAUSDQoFbGV2ZWwY" + "DyABKAUSEgoKaXNDbGVhbk1hcBgQIAEoCEIrCh9jb20ubG9uZ21vLmdhbWVz" + "ZXJ2ZXIubmV0LnByb3RvQghNYXBQcm90b2IGcHJvdG8z"), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[19]
		{
			new GeneratedClrTypeInfo(typeof(SibingsMapReq_1007001), SibingsMapReq_1007001.Parser, new string[1] { "MapId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SibingsMapResp_1007001), SibingsMapResp_1007001.Parser, new string[3] { "Code", "IsSuccess", "Object" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(SibingsMapResp_1007001.Types.MapObjectResp_1007002), SibingsMapResp_1007001.Types.MapObjectResp_1007002.Parser, new string[15]
				{
					"Id", "ParentId", "ParentName", "Name", "SibingsId", "ChildrenIds", "Type", "MinLevel", "MaxLevel", "Status",
					"Restrict", "BasedConcentration", "FiveElements", "Sort", "Level"
				}, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(ChildrenMapReq_1007003), ChildrenMapReq_1007003.Parser, new string[1] { "MapId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChildrenMapResp_1007003), ChildrenMapResp_1007003.Parser, new string[3] { "Code", "IsSuccess", "Object" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(ChildrenMapResp_1007003.Types.MapObjectResp_1007004), ChildrenMapResp_1007003.Types.MapObjectResp_1007004.Parser, new string[15]
				{
					"Id", "ParentId", "ParentName", "Name", "SibingsId", "ChildrenIds", "Type", "MinLevel", "MaxLevel", "Status",
					"Restrict", "BasedConcentration", "FiveElements", "Sort", "Level"
				}, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(ParentMapIdReq_1007005), ParentMapIdReq_1007005.Parser, new string[1] { "MapId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ParentMapIdResp_1007005), ParentMapIdResp_1007005.Parser, new string[3] { "Code", "IsSuccess", "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ParentMapReq_1007006), ParentMapReq_1007006.Parser, new string[1] { "MapId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ParentMapResp_1007006), ParentMapResp_1007006.Parser, new string[3] { "Code", "IsSuccess", "Object" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(ParentMapResp_1007006.Types.MapObjectResp_1007007), ParentMapResp_1007006.Types.MapObjectResp_1007007.Parser, new string[15]
				{
					"Id", "ParentId", "ParentName", "Name", "SibingsId", "ChildrenIds", "Type", "MinLevel", "MaxLevel", "Status",
					"Restrict", "BasedConcentration", "FiveElements", "Sort", "Level"
				}, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(AncestorMapResp_1007008), AncestorMapResp_1007008.Parser, new string[4] { "Code", "IsSuccess", "Id", "Name" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MapInfoReq_1007009), MapInfoReq_1007009.Parser, new string[1] { "MapId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MapInfoResp_1007009), MapInfoResp_1007009.Parser, new string[17]
			{
				"Code", "IsSuccess", "Id", "ParentId", "ParentName", "Name", "SibingsId", "ChildrenIds", "Type", "MinLevel",
				"MaxLevel", "Status", "Restrict", "BasedConcentration", "FiveElements", "Sort", "Level"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(MapTransmitReq_1007010), MapTransmitReq_1007010.Parser, new string[1] { "MapId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MapTransmitResp_1007010), MapTransmitResp_1007010.Parser, new string[3] { "Code", "IsSuccess", "ShinyStone" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MapCityMigrateReq_1007011), MapCityMigrateReq_1007011.Parser, new string[1] { "MapId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MapCityMigrateResp_1007011), MapCityMigrateResp_1007011.Parser, new string[4] { "Code", "IsSuccess", "CurrentAge", "CurrentPower" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ParentMapAndOneselfMapReq_1007012), ParentMapAndOneselfMapReq_1007012.Parser, new string[1] { "MapId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ParentMapAndOneselfMapResp_1007012), ParentMapAndOneselfMapResp_1007012.Parser, new string[3] { "Code", "IsSuccess", "Name" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(LiLianChildrenMapReq_1007013), LiLianChildrenMapReq_1007013.Parser, new string[1] { "MapId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(LiLianChildrenMapResp_1007013), LiLianChildrenMapResp_1007013.Parser, new string[3] { "Code", "IsSuccess", "Object" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(LiLianChildrenMapResp_1007013.Types.LiLianMapObjectResp_1007014), LiLianChildrenMapResp_1007013.Types.LiLianMapObjectResp_1007014.Parser, new string[16]
				{
					"Id", "ParentId", "ParentName", "Name", "SibingsId", "ChildrenIds", "Type", "MinLevel", "MaxLevel", "Status",
					"Restrict", "BasedConcentration", "FiveElements", "Sort", "Level", "IsCleanMap"
				}, null, null, null)
			})
		}));
	}
}
