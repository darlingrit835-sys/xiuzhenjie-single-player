using System;
using Google.Protobuf.Reflection;

public static class PlantProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static PlantProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChBQbGFudFByb3RvLnByb3RvIo0CChZFbnRlclBsYW50UmVzcF8xMDEzMDAx" + "EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEjYKBGxhbmQYAyAD" + "KAsyKC5FbnRlclBsYW50UmVzcF8xMDEzMDAxLkxhbmRSZXNwXzEwMTMwMDIS" + "FgoOaXNPbmVLZXlLYWlLZW4YBCABKAgagQEKEExhbmRSZXNwXzEwMTMwMDIS" + "CgoCaWQYASABKAUSDgoGc3RhdHVzGAIgASgFEhUKDXRpbWVSZW1haW5pbmcY" + "AyABKAMSFgoOdG90YWxXYXN0ZVRpbWUYBCABKAMSDwoHcGxhbnRJZBgFIAEo" + "CRIRCglwbGFudE5hbWUYBiABKAkiKgoVS2FpS2VuTGFuZFJlcV8xMDEzMDAz" + "EhEKCWxhbmRJbmRleBgBIAEoBSKSAQoWS2FpS2VuTGFuZFJlc3BfMTAxMzAw" + "MxIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBISCgpsYW5kU3Rh" + "dHVzGAMgASgFEhUKDXRpbWVSZW1haW5pbmcYBCABKAMSFgoOdG90YWxXYXN0" + "ZVRpbWUYBSABKAMSFAoMY3VycmVudFZpZ29yGAYgASgFIiYKFEVudGVyU29p" + "bFJlcV8xMDEzMDA0Eg4KBmxhbmRJZBgBIAEoBSK9AgoVRW50ZXJTb2lsUmVz" + "cF8xMDEzMDA0EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEjwK" + "C2N1cnJlbnRTb2lsGAMgASgLMicuRW50ZXJTb2lsUmVzcF8xMDEzMDA0LlNv" + "aWxSZXNwXzEwMTMwMDUSOAoHc29pbEFsbBgEIAMoCzInLkVudGVyU29pbFJl" + "c3BfMTAxMzAwNC5Tb2lsUmVzcF8xMDEzMDA1GooBChBTb2lsUmVzcF8xMDEz" + "MDA1EgoKAmlkGAEgASgJEhMKC2ZpdmVFbGVtZW50GAIgASgFEg0KBWxldmVs" + "GAMgASgFEgwKBG5hbWUYBCABKAkSDAoEcmFuaxgFIAEoBRISCgpyZWNlaXZl" + "VmFsGAYgASgBEhYKDmNvbnZlcnNpb25SYXRlGAcgASgBIjcKFUNob29zZVNv" + "aWxSZXFfMTAxMzAwNhIOCgZsYW5kSWQYASABKAUSDgoGc29pbElkGAIgASgJ" + "Ik0KFkNob29zZVNvaWxSZXNwXzEwMTMwMDYSDAoEY29kZRgBIAEoBRIRCglp" + "c1N1Y2Nlc3MYAiABKAgSEgoKbGFuZFN0YXR1cxgDIAEoBSI8ChVTb2lsRGVw" + "bG95UmVxXzEwMTMwMDcSDwoHZ29vZHNJZBgBIAMoCRISCgp4aVJhbmdUeXBl" + "GAIgASgFItABChZTb2lsRGVwbG95UmVzcF8xMDEzMDA3EgwKBGNvZGUYASAB" + "KAUSEQoJaXNTdWNjZXNzGAIgASgIEg4KBnNvaWxJZBgDIAEoCRITCgtmaXZl" + "RWxlbWVudBgEIAEoBRINCgVsZXZlbBgFIAEoBRIMCgRuYW1lGAYgASgJEgwK" + "BHJhbmsYByABKAUSEgoKcmVjZWl2ZVZhbBgIIAEoARIWCg5jb252ZXJzaW9u" + "UmF0ZRgJIAEoARIZChFjdXJyZW50U2hpbnlTdG9uZRgKIAEoAyI1CiBEZXBs" + "b3lSaW5nR29vZHNTZWxlY3RSZXFfMTAxMzAwOBIRCglnb29kc1R5cGUYASAB" + "KAUixQIKIURlcGxveVJpbmdHb29kc1NlbGVjdFJlc3BfMTAxMzAwOBIMCgRj" + "b2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBJLCglnb29kc0luZm8YAyAD" + "KAsyOC5EZXBsb3lSaW5nR29vZHNTZWxlY3RSZXNwXzEwMTMwMDguR29vZHNJ" + "bmZvUmVzcF8xMDEzMDA5GrEBChVHb29kc0luZm9SZXNwXzEwMTMwMDkSDwoH" + "Z29vZHNJZBgBIAEoCRIMCgRuYW1lGAIgASgJEhMKC2ZpdmVFbGVtZW50GAMg" + "ASgFEgwKBHJhbmsYBCABKAUSDQoFbGV2ZWwYBSABKAUSDAoEdHlwZRgGIAEo" + "BRISCgpyZWNlaXZlVmFsGAcgASgBEhYKDmNvbnZlcnNpb25SYXRlGAggASgB" + "Eg0KBWNvdW50GAkgASgFIigKFkRpc2NhcmRTb2lsUmVxXzEwMTMwMTASDgoG" + "c29pbElkGAEgASgJIjoKF0Rpc2NhcmRTb2lsUmVzcF8xMDEzMDEwEgwKBGNv" + "ZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIItsBChVFbnRlclNlZWRSZXNw" + "XzEwMTMwMTESDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSPQoI" + "c2VlZEluZm8YAyADKAsyKy5FbnRlclNlZWRSZXNwXzEwMTMwMTEuU2VlZElu" + "Zm9SZXNwXzEwMTMwMTIaYgoUU2VlZEluZm9SZXNwXzEwMTMwMTISCgoCaWQY" + "ASABKAkSDAoEbmFtZRgCIAEoCRITCgtmaXZlRWxlbWVudBgDIAEoBRIMCgRy" + "YW5rGAQgASgFEg0KBWxldmVsGAUgASgFIjcKFVNlZWRDaG9vc2VSZXFfMTAx" + "MzAxMxIOCgZsYW5kSWQYASABKAUSDgoGc2VlZElkGAIgASgJInwKFlNlZWRD" + "aG9vc2VSZXNwXzEwMTMwMTMSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MY" + "AiABKAgSEgoKbGFuZFN0YXR1cxgDIAEoBRIVCg10aW1lUmVtYWluaW5nGAQg" + "ASgDEhYKDnRvdGFsV2FzdGVUaW1lGAUgASgDIioKGEVudGVySXJyaWdhdGVS" + "ZXFfMTAxMzAxNBIOCgZsYW5kSWQYASABKAUipwIKGUVudGVySXJyaWdhdGVS" + "ZXNwXzEwMTMwMTQSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgS" + "QwoJd2F0ZXJJbmZvGAMgAygLMjAuRW50ZXJJcnJpZ2F0ZVJlc3BfMTAxMzAx" + "NC5XYXRlckluZm9SZXNwXzEwMTMwMTUaowEKFVdhdGVySW5mb1Jlc3BfMTAx" + "MzAxNRIKCgJpZBgBIAEoCRIMCgRuYW1lGAIgASgJEhMKC2ZpdmVFbGVtZW50" + "GAMgASgFEgwKBHJhbmsYBCABKAUSDQoFbGV2ZWwYBSABKAUSEgoKcmVjZWl2" + "ZVZhbBgGIAEoARIWCg5jb252ZXJzaW9uUmF0ZRgHIAEoARISCgpyZWR1Y2VU" + "aW1lGAggASgDIjkKFldhdGVyQ2hvb3NlUmVxXzEwMTMwMTYSDgoGbGFuZElk" + "GAEgASgFEg8KB3dhdGVySWQYAiABKAkifQoXV2F0ZXJDaG9vc2VSZXNwXzEw" + "MTMwMTYSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSEgoKbGFu" + "ZFN0YXR1cxgDIAEoBRIVCg10aW1lUmVtYWluaW5nGAQgASgDEhYKDnRvdGFs" + "V2FzdGVUaW1lGAUgASgDIkQKFldhdGVyRGVwbG95UmVxXzEwMTMwMTcSDwoH" + "Z29vZHNJZBgBIAMoCRIZChF3YXRlck1hdGVyaWFsVHlwZRgCIAEoBSLSAQoX" + "V2F0ZXJEZXBsb3lSZXNwXzEwMTMwMTcSDAoEY29kZRgBIAEoBRIRCglpc1N1" + "Y2Nlc3MYAiABKAgSDwoHd2F0ZXJJZBgDIAEoCRIMCgRuYW1lGAQgASgJEhMK" + "C2ZpdmVFbGVtZW50GAUgASgFEgwKBHJhbmsYBiABKAUSDQoFbGV2ZWwYByAB" + "KAUSEgoKcmVjZWl2ZVZhbBgIIAEoARIWCg5jb252ZXJzaW9uUmF0ZRgJIAEo" + "ARIZChFjdXJyZW50U2hpbnlTdG9uZRgKIAEoAyIpChdIYXJ2ZXN0UGxhbnRS" + "ZXFfMTAxMzAxOBIOCgZsYW5kSWQYASABKAUingIKGEhhcnZlc3RQbGFudFJl" + "c3BfMTAxMzAxOBIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBJC" + "CglwbGFudEluZm8YAyADKAsyLy5IYXJ2ZXN0UGxhbnRSZXNwXzEwMTMwMTgu" + "UGxhbnRJbmZvUmVzcF8xMDEzMDE5EhEKCWxpdmVMZXZlbBgEIAEoBRIXCg9s" + "aXZlUHJvZmljaWVuY3kYBSABKAUacQoVUGxhbnRJbmZvUmVzcF8xMDEzMDE5" + "EgoKAmlkGAEgASgJEgwKBG5hbWUYAiABKAkSEwoLZml2ZUVsZW1lbnQYAyAB" + "KAUSDAoEcmFuaxgEIAEoBRINCgVsZXZlbBgFIAEoBRIMCgR0eXBlGAYgASgF" + "IicKFUZvcmdldFNlZWRSZXFfMTAxMzAyMBIOCgZzZWVkSWQYASABKAkiOQoW" + "Rm9yZ2V0U2VlZFJlc3BfMTAxMzAyMBIMCgRjb2RlGAEgASgFEhEKCWlzU3Vj" + "Y2VzcxgCIAEoCCInChVHYXRoZXJTZWVkUmVxXzEwMTMwMjESDgoGc2VlZElk" + "GAEgASgJIjkKFkdhdGhlclNlZWRSZXNwXzEwMTMwMjESDAoEY29kZRgBIAEo" + "BRIRCglpc1N1Y2Nlc3MYAiABKAgihwIKGVNlYXJjaFNvaWxBbGxSZXNwXzEw" + "MTMwMjISDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSPAoHc29p" + "bEFsbBgDIAMoCzIrLlNlYXJjaFNvaWxBbGxSZXNwXzEwMTMwMjIuU29pbFJl" + "c3BfMTAxMzAyMxqKAQoQU29pbFJlc3BfMTAxMzAyMxIKCgJpZBgBIAEoCRIT" + "CgtmaXZlRWxlbWVudBgCIAEoBRINCgVsZXZlbBgDIAEoBRIMCgRuYW1lGAQg" + "ASgJEgwKBHJhbmsYBSABKAUSEgoKcmVjZWl2ZVZhbBgGIAEoARIWCg5jb252" + "ZXJzaW9uUmF0ZRgHIAEoASKUAgoaU2VhcmNoV2F0ZXJBbGxSZXNwXzEwMTMw" + "MjQSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSQwoId2F0ZXJB" + "bGwYAyADKAsyMS5TZWFyY2hXYXRlckFsbFJlc3BfMTAxMzAyNC5XYXRlcklu" + "Zm9SZXNwXzEwMTMwMjUajwEKFVdhdGVySW5mb1Jlc3BfMTAxMzAyNRIKCgJp" + "ZBgBIAEoCRIMCgRuYW1lGAIgASgJEhMKC2ZpdmVFbGVtZW50GAMgASgFEgwK" + "BHJhbmsYBCABKAUSDQoFbGV2ZWwYBSABKAUSEgoKcmVjZWl2ZVZhbBgGIAEo" + "ARIWCg5jb252ZXJzaW9uUmF0ZRgHIAEoASIqChdEaXNjYXJkV2F0ZXJSZXFf" + "MTAxMzAyNhIPCgd3YXRlcklkGAEgASgJIjsKGERpc2NhcmRXYXRlclJlc3Bf" + "MTAxMzAyNhIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCCLgAQoY" + "U2VhcmNoWGlSYW5nUmVzcF8xMDEzMDI3EgwKBGNvZGUYASABKAUSEQoJaXNT" + "dWNjZXNzGAIgASgIEjwKBnhpUmFuZxgDIAMoCzIsLlNlYXJjaFhpUmFuZ1Jl" + "c3BfMTAxMzAyNy5YaVJhbmdSZXNwXzEwMTMwMjgaZQoSWGlSYW5nUmVzcF8x" + "MDEzMDI4EgwKBHR5cGUYASABKAUSDAoEbmFtZRgCIAEoCRIMCgRyYW5rGAMg" + "ASgFEhYKDmNvbnZlcnNpb25SYXRlGAQgASgBEg0KBXByaWNlGAUgASgFIuoB" + "ChpTZWFyY2hMaW5nUXVhblJlc3BfMTAxMzAyORIMCgRjb2RlGAEgASgFEhEK" + "CWlzU3VjY2VzcxgCIAEoCBJCCghsaW5nUXVhbhgDIAMoCzIwLlNlYXJjaExp" + "bmdRdWFuUmVzcF8xMDEzMDI5LkxpbmdRdWFuUmVzcF8xMDEzMDMwGmcKFExp" + "bmdRdWFuUmVzcF8xMDEzMDMwEgwKBHR5cGUYASABKAUSDAoEbmFtZRgCIAEo" + "CRIMCgRyYW5rGAMgASgFEhYKDmNvbnZlcnNpb25SYXRlGAQgASgBEg0KBXBy" + "aWNlGAUgASgFQi0KH2NvbS5sb25nbW8uZ2FtZXNlcnZlci5uZXQucHJvdG9C" + "ClBsYW50UHJvdG9iBnByb3RvMw=="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[34]
		{
			new GeneratedClrTypeInfo(typeof(EnterPlantResp_1013001), EnterPlantResp_1013001.Parser, new string[4] { "Code", "IsSuccess", "Land", "IsOneKeyKaiKen" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterPlantResp_1013001.Types.LandResp_1013002), EnterPlantResp_1013001.Types.LandResp_1013002.Parser, new string[6] { "Id", "Status", "TimeRemaining", "TotalWasteTime", "PlantId", "PlantName" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(KaiKenLandReq_1013003), KaiKenLandReq_1013003.Parser, new string[1] { "LandIndex" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(KaiKenLandResp_1013003), KaiKenLandResp_1013003.Parser, new string[6] { "Code", "IsSuccess", "LandStatus", "TimeRemaining", "TotalWasteTime", "CurrentVigor" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterSoilReq_1013004), EnterSoilReq_1013004.Parser, new string[1] { "LandId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterSoilResp_1013004), EnterSoilResp_1013004.Parser, new string[4] { "Code", "IsSuccess", "CurrentSoil", "SoilAll" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterSoilResp_1013004.Types.SoilResp_1013005), EnterSoilResp_1013004.Types.SoilResp_1013005.Parser, new string[7] { "Id", "FiveElement", "Level", "Name", "Rank", "ReceiveVal", "ConversionRate" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(ChooseSoilReq_1013006), ChooseSoilReq_1013006.Parser, new string[2] { "LandId", "SoilId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChooseSoilResp_1013006), ChooseSoilResp_1013006.Parser, new string[3] { "Code", "IsSuccess", "LandStatus" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SoilDeployReq_1013007), SoilDeployReq_1013007.Parser, new string[2] { "GoodsId", "XiRangType" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SoilDeployResp_1013007), SoilDeployResp_1013007.Parser, new string[10] { "Code", "IsSuccess", "SoilId", "FiveElement", "Level", "Name", "Rank", "ReceiveVal", "ConversionRate", "CurrentShinyStone" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(DeployRingGoodsSelectReq_1013008), DeployRingGoodsSelectReq_1013008.Parser, new string[1] { "GoodsType" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(DeployRingGoodsSelectResp_1013008), DeployRingGoodsSelectResp_1013008.Parser, new string[3] { "Code", "IsSuccess", "GoodsInfo" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(DeployRingGoodsSelectResp_1013008.Types.GoodsInfoResp_1013009), DeployRingGoodsSelectResp_1013008.Types.GoodsInfoResp_1013009.Parser, new string[9] { "GoodsId", "Name", "FiveElement", "Rank", "Level", "Type", "ReceiveVal", "ConversionRate", "Count" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(DiscardSoilReq_1013010), DiscardSoilReq_1013010.Parser, new string[1] { "SoilId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(DiscardSoilResp_1013010), DiscardSoilResp_1013010.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterSeedResp_1013011), EnterSeedResp_1013011.Parser, new string[3] { "Code", "IsSuccess", "SeedInfo" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterSeedResp_1013011.Types.SeedInfoResp_1013012), EnterSeedResp_1013011.Types.SeedInfoResp_1013012.Parser, new string[5] { "Id", "Name", "FiveElement", "Rank", "Level" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(SeedChooseReq_1013013), SeedChooseReq_1013013.Parser, new string[2] { "LandId", "SeedId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SeedChooseResp_1013013), SeedChooseResp_1013013.Parser, new string[5] { "Code", "IsSuccess", "LandStatus", "TimeRemaining", "TotalWasteTime" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterIrrigateReq_1013014), EnterIrrigateReq_1013014.Parser, new string[1] { "LandId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterIrrigateResp_1013014), EnterIrrigateResp_1013014.Parser, new string[3] { "Code", "IsSuccess", "WaterInfo" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterIrrigateResp_1013014.Types.WaterInfoResp_1013015), EnterIrrigateResp_1013014.Types.WaterInfoResp_1013015.Parser, new string[8] { "Id", "Name", "FiveElement", "Rank", "Level", "ReceiveVal", "ConversionRate", "ReduceTime" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(WaterChooseReq_1013016), WaterChooseReq_1013016.Parser, new string[2] { "LandId", "WaterId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(WaterChooseResp_1013016), WaterChooseResp_1013016.Parser, new string[5] { "Code", "IsSuccess", "LandStatus", "TimeRemaining", "TotalWasteTime" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(WaterDeployReq_1013017), WaterDeployReq_1013017.Parser, new string[2] { "GoodsId", "WaterMaterialType" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(WaterDeployResp_1013017), WaterDeployResp_1013017.Parser, new string[10] { "Code", "IsSuccess", "WaterId", "Name", "FiveElement", "Rank", "Level", "ReceiveVal", "ConversionRate", "CurrentShinyStone" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(HarvestPlantReq_1013018), HarvestPlantReq_1013018.Parser, new string[1] { "LandId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(HarvestPlantResp_1013018), HarvestPlantResp_1013018.Parser, new string[5] { "Code", "IsSuccess", "PlantInfo", "LiveLevel", "LiveProficiency" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(HarvestPlantResp_1013018.Types.PlantInfoResp_1013019), HarvestPlantResp_1013018.Types.PlantInfoResp_1013019.Parser, new string[6] { "Id", "Name", "FiveElement", "Rank", "Level", "Type" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(ForgetSeedReq_1013020), ForgetSeedReq_1013020.Parser, new string[1] { "SeedId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ForgetSeedResp_1013020), ForgetSeedResp_1013020.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GatherSeedReq_1013021), GatherSeedReq_1013021.Parser, new string[1] { "SeedId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GatherSeedResp_1013021), GatherSeedResp_1013021.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SearchSoilAllResp_1013022), SearchSoilAllResp_1013022.Parser, new string[3] { "Code", "IsSuccess", "SoilAll" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(SearchSoilAllResp_1013022.Types.SoilResp_1013023), SearchSoilAllResp_1013022.Types.SoilResp_1013023.Parser, new string[7] { "Id", "FiveElement", "Level", "Name", "Rank", "ReceiveVal", "ConversionRate" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(SearchWaterAllResp_1013024), SearchWaterAllResp_1013024.Parser, new string[3] { "Code", "IsSuccess", "WaterAll" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(SearchWaterAllResp_1013024.Types.WaterInfoResp_1013025), SearchWaterAllResp_1013024.Types.WaterInfoResp_1013025.Parser, new string[7] { "Id", "Name", "FiveElement", "Rank", "Level", "ReceiveVal", "ConversionRate" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(DiscardWaterReq_1013026), DiscardWaterReq_1013026.Parser, new string[1] { "WaterId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(DiscardWaterResp_1013026), DiscardWaterResp_1013026.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SearchXiRangResp_1013027), SearchXiRangResp_1013027.Parser, new string[3] { "Code", "IsSuccess", "XiRang" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(SearchXiRangResp_1013027.Types.XiRangResp_1013028), SearchXiRangResp_1013027.Types.XiRangResp_1013028.Parser, new string[5] { "Type", "Name", "Rank", "ConversionRate", "Price" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(SearchLingQuanResp_1013029), SearchLingQuanResp_1013029.Parser, new string[3] { "Code", "IsSuccess", "LingQuan" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(SearchLingQuanResp_1013029.Types.LingQuanResp_1013030), SearchLingQuanResp_1013029.Types.LingQuanResp_1013030.Parser, new string[5] { "Type", "Name", "Rank", "ConversionRate", "Price" }, null, null, null)
			})
		}));
	}
}
