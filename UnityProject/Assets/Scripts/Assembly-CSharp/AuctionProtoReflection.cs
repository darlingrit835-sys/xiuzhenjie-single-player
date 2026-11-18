using System;
using Google.Protobuf.Reflection;

public static class AuctionProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static AuctionProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChJBdWN0aW9uUHJvdG8ucHJvdG8ihwIKF0VudGVyQXVjdGlvblJlcV8xMDA2" + "MDAxEg8KB3BhZ2VOdW0YASABKAUSEAoIcGFnZVNpemUYAiABKAUSDAoEbmFt" + "ZRgDIAEoCRIRCglnb29kc1R5cGUYBCABKAUSDAoEdHlwZRgFIAEoBRITCgtm" + "aXZlRWxlbWVudBgGIAEoBRINCgVzdGFnZRgHIAEoBRIVCg1taW5GaW5hbFBy" + "aWNlGAggASgDEhUKDW1heEZpbmFsUHJpY2UYCSABKAMSFgoOZmluYWxQcmlj" + "ZVNvcnQYCiABKAkSEAoIcmFua1NvcnQYCyABKAkSEAoIc2hvcFR5cGUYDCAB" + "KAUSDAoEcmFuaxgNIAEoBSKHAwoYRW50ZXJBdWN0aW9uUmVzcF8xMDA2MDAx" + "EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEg8KB3BhZ2VOdW0Y" + "AyABKAUSEAoIcGFnZVNpemUYBCABKAUSDQoFcGFnZXMYBSABKAUSQwoGb2Jq" + "ZWN0GAYgAygLMjMuRW50ZXJBdWN0aW9uUmVzcF8xMDA2MDAxLkF1Y3Rpb25P" + "YmplY3RSZXNwXzEwMDYwMDIa0gEKGUF1Y3Rpb25PYmplY3RSZXNwXzEwMDYw" + "MDISCgoCaWQYASABKAkSDwoHZ29vZHNJZBgCIAEoCRIMCgRuYW1lGAMgASgJ" + "EhEKCWdvb2RzVHlwZRgEIAEoBRITCgtmaXZlRWxlbWVudBgFIAEoBRISCgpm" + "aXhlZFByaWNlGAYgASgDEhMKC3JlbWFpbkhvdXJzGAcgASgFEg0KBXN0YWdl" + "GAggASgFEgwKBHJhbmsYCSABKAUSDQoFbGV2ZWwYCiABKAUSDQoFY291bnQY" + "CyABKAUiTAoZRW50ZXJNeUF1Y3Rpb25SZXFfMTAwNjAwMxIMCgR0eXBlGAEg" + "ASgFEg8KB3BhZ2VOdW0YAiABKAUSEAoIcGFnZVNpemUYAyABKAUi/gIKGkVu" + "dGVyTXlBdWN0aW9uUmVzcF8xMDA2MDAzEgwKBGNvZGUYASABKAUSEQoJaXNT" + "dWNjZXNzGAIgASgIEg8KB3BhZ2VOdW0YAyABKAUSEAoIcGFnZVNpemUYBCAB" + "KAUSDQoFcGFnZXMYBSABKAUSRwoGb2JqZWN0GAYgAygLMjcuRW50ZXJNeUF1" + "Y3Rpb25SZXNwXzEwMDYwMDMuTXlBdWN0aW9uT2JqZWN0UmVzcF8xMDA2MDA0" + "GsMBChtNeUF1Y3Rpb25PYmplY3RSZXNwXzEwMDYwMDQSCgoCaWQYASABKAkS" + "DAoEbmFtZRgCIAEoCRITCgtmaXZlRWxlbWVudBgDIAEoBRITCgtyZW1haW5I" + "b3VycxgEIAEoBRINCgVzdGFnZRgFIAEoBRIMCgRyYW5rGAYgASgFEg0KBWxl" + "dmVsGAcgASgFEhEKCWdvb2RzVHlwZRgIIAEoBRISCgpmaXhlZFByaWNlGAkg" + "ASgDEg0KBWNvdW50GAogASgFImEKGUVudHJ1c3RBdWN0aW9uUmVxXzEwMDYw" + "MDgSDwoHZ29vZHNJZBgBIAEoCRISCgpmaXhlZFByaWNlGAIgASgDEhAKCHNo" + "b3BUeXBlGAMgASgFEg0KBWNvdW50GAQgASgFImoKGkVudHJ1c3RBdWN0aW9u" + "UmVzcF8xMDA2MDA4EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgI" + "EhcKD25vd1JpbmdDYXBhY2l0eRgDIAEoBRISCgpzaGlueVN0b25lGAQgASgD" + "IjIKHUF1Y3Rpb25Hb29kc0NhbmNlbFJlcV8xMDA2MDA5EhEKCWF1Y3Rpb25J" + "ZBgBIAEoCSJBCh5BdWN0aW9uR29vZHNDYW5jZWxSZXNwXzEwMDYwMDkSDAoE" + "Y29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgiKgoVQXVjdGlvbkJ1eVJl" + "cV8xMDA2MDEwEhEKCWF1Y3Rpb25JZBgBIAEoCSJNChZBdWN0aW9uQnV5UmVz" + "cF8xMDA2MDEwEgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEhIK" + "CnNoaW55U3RvbmUYAyABKAMiLQocRW50ZXJUcmVhc3VyZVJvb21SZXFfMTAw" + "NjAxMRINCgVzdGFnZRgBIAEoBSKxAgodRW50ZXJUcmVhc3VyZVJvb21SZXNw" + "XzEwMDYwMTESDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSDQoF" + "c3RhZ2UYAyABKAUSGAoQdG9PblRoZVNoZWxmVGltZRgEIAEoBRIaChJ0b0Vu" + "dGVyQXVjdGlvblRpbWUYBSABKAUSRwoJZ29vZHNMaXN0GAYgAygLMjQuRW50" + "ZXJUcmVhc3VyZVJvb21SZXNwXzEwMDYwMTEuR29vZHNMaXN0UmVzcF8xMDA2" + "MDEyGmEKFUdvb2RzTGlzdFJlc3BfMTAwNjAxMhIRCglnb29kc05hbWUYASAB" + "KAkSEwoLZml2ZUVsZW1lbnQYAiABKAUSDAoEcmFuaxgDIAEoBRISCgpzdGFy" + "dFByaWNlGAQgASgFIkEKGk9uU2hlbGZUcmVhc3VyZVJlcV8xMDA2MDEzEg8K" + "B2dvb2RzSWQYASABKAkSEgoKc3RhcnRQcmljZRgCIAEoAyJXChtPblNoZWxm" + "VHJlYXN1cmVSZXNwXzEwMDYwMTMSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nl" + "c3MYAiABKAgSFwoPbm93UmluZ0NhcGFjaXR5GAMgASgFIrQCChdPblNoZWxm" + "TGlzdFJlc3BfMTAwNjAxNBIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgC" + "IAEoCBJECglnb29kc0xpc3QYAyADKAsyMS5PblNoZWxmTGlzdFJlc3BfMTAw" + "NjAxNC5PblNoZWxmR29vZHNSZXNwXzEwMDYwMTUasQEKGE9uU2hlbGZHb29k" + "c1Jlc3BfMTAwNjAxNRIKCgJpZBgBIAEoBRIMCgRuYW1lGAIgASgJEhMKC2Zp" + "dmVFbGVtZW50GAMgASgFEhMKC2F1Y3Rpb25UaW1lGAQgASgDEg0KBXN0YWdl" + "GAUgASgFEgwKBHJhbmsYBiABKAUSDQoFbGV2ZWwYByABKAUSEQoJZ29vZHNU" + "eXBlGAggASgFEhIKCnN0YXJ0UHJpY2UYCSABKAMiNAoeVHJlYXN1cmVHb29k" + "c0NhbmNlbFJlcV8xMDA2MDE2EhIKCnRyZWFzdXJlSWQYASABKAUiQgofVHJl" + "YXN1cmVHb29kc0NhbmNlbFJlc3BfMTAwNjAxNhIMCgRjb2RlGAEgASgFEhEK" + "CWlzU3VjY2VzcxgCIAEoCCIpChhFbnRlclRyZWFzdXJlUmVxXzEwMDYwMTcS" + "DQoFc3RhZ2UYASABKAUiewoZRW50ZXJUcmVhc3VyZVJlc3BfMTAwNjAxNxIM" + "CgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBISCgpzaGlueVN0b25l" + "GAMgASgFEhIKCnRyZWFzdXJlSWQYBCABKAUSFQoNaXNNYXlBZGRQcmljZRgF" + "IAEoCCKLAgodVHJlYXN1cmVHb29kc0xpc3RSZXNwXzEwMDYwMTgSDAoEY29k" + "ZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSDQoFc3RhZ2UYAyABKAUSRwoJ" + "Z29vZHNMaXN0GAQgAygLMjQuVHJlYXN1cmVHb29kc0xpc3RSZXNwXzEwMDYw" + "MTguR29vZHNMaXN0UmVzcF8xMDA2MDE5GnEKFUdvb2RzTGlzdFJlc3BfMTAw" + "NjAxORIRCglnb29kc05hbWUYASABKAkSEwoLZml2ZUVsZW1lbnQYAiABKAUS" + "DAoEcmFuaxgDIAEoBRISCgpmaW5hbFByaWNlGAQgASgDEg4KBnN0YXR1cxgF" + "IAEoBSI7ChZUcmVhc3VyZUJ1eVJlcV8xMDA2MDIwEg0KBXByaWNlGAEgASgD" + "EhIKCnRyZWFzdXJlSWQYAiABKAUiTgoXVHJlYXN1cmVCdXlSZXNwXzEwMDYw" + "MjASDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSEgoKc2hpbnlT" + "dG9uZRgDIAEoBSLEAgogVHJlYXN1cmVDdXJyZW50R29vZHNSZXNwXzEwMDYw" + "MjESDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSDwoHZ29vZHNJ" + "ZBgDIAEoCRIRCglnb29kc05hbWUYBCABKAkSEwoLZml2ZUVsZW1lbnQYBSAB" + "KAUSFQoNY3VycmVudFBlcnNvbhgGIAEoCRINCgVzdGFnZRgHIAEoBRIMCgRy" + "YW5rGAggASgFEg0KBWxldmVsGAkgASgFEhEKCWdvb2RzVHlwZRgKIAEoBRIU" + "CgxjdXJyZW50UHJpY2UYCyABKAMSEwoLcGVyc29uSW1hZ2UYDCABKAUSEQoJ" + "cGVyc29uU2V4GA0gASgIEhIKCnBlcnNvbk5hbWUYDiABKAkSCgoCaWQYDyAB" + "KAUSEgoKc3RhcnRQcmljZRgQIAEoA0IvCh9jb20ubG9uZ21vLmdhbWVzZXJ2" + "ZXIubmV0LnByb3RvQgxBdWN0aW9uUHJvdG9iBnByb3RvMw=="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[23]
		{
			new GeneratedClrTypeInfo(typeof(EnterAuctionReq_1006001), EnterAuctionReq_1006001.Parser, new string[13]
			{
				"PageNum", "PageSize", "Name", "GoodsType", "Type", "FiveElement", "Stage", "MinFinalPrice", "MaxFinalPrice", "FinalPriceSort",
				"RankSort", "ShopType", "Rank"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterAuctionResp_1006001), EnterAuctionResp_1006001.Parser, new string[6] { "Code", "IsSuccess", "PageNum", "PageSize", "Pages", "Object" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterAuctionResp_1006001.Types.AuctionObjectResp_1006002), EnterAuctionResp_1006001.Types.AuctionObjectResp_1006002.Parser, new string[11]
				{
					"Id", "GoodsId", "Name", "GoodsType", "FiveElement", "FixedPrice", "RemainHours", "Stage", "Rank", "Level",
					"Count"
				}, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(EnterMyAuctionReq_1006003), EnterMyAuctionReq_1006003.Parser, new string[3] { "Type", "PageNum", "PageSize" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterMyAuctionResp_1006003), EnterMyAuctionResp_1006003.Parser, new string[6] { "Code", "IsSuccess", "PageNum", "PageSize", "Pages", "Object" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterMyAuctionResp_1006003.Types.MyAuctionObjectResp_1006004), EnterMyAuctionResp_1006003.Types.MyAuctionObjectResp_1006004.Parser, new string[10] { "Id", "Name", "FiveElement", "RemainHours", "Stage", "Rank", "Level", "GoodsType", "FixedPrice", "Count" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(EntrustAuctionReq_1006008), EntrustAuctionReq_1006008.Parser, new string[4] { "GoodsId", "FixedPrice", "ShopType", "Count" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EntrustAuctionResp_1006008), EntrustAuctionResp_1006008.Parser, new string[4] { "Code", "IsSuccess", "NowRingCapacity", "ShinyStone" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(AuctionGoodsCancelReq_1006009), AuctionGoodsCancelReq_1006009.Parser, new string[1] { "AuctionId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(AuctionGoodsCancelResp_1006009), AuctionGoodsCancelResp_1006009.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(AuctionBuyReq_1006010), AuctionBuyReq_1006010.Parser, new string[1] { "AuctionId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(AuctionBuyResp_1006010), AuctionBuyResp_1006010.Parser, new string[3] { "Code", "IsSuccess", "ShinyStone" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterTreasureRoomReq_1006011), EnterTreasureRoomReq_1006011.Parser, new string[1] { "Stage" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterTreasureRoomResp_1006011), EnterTreasureRoomResp_1006011.Parser, new string[6] { "Code", "IsSuccess", "Stage", "ToOnTheShelfTime", "ToEnterAuctionTime", "GoodsList" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterTreasureRoomResp_1006011.Types.GoodsListResp_1006012), EnterTreasureRoomResp_1006011.Types.GoodsListResp_1006012.Parser, new string[4] { "GoodsName", "FiveElement", "Rank", "StartPrice" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(OnShelfTreasureReq_1006013), OnShelfTreasureReq_1006013.Parser, new string[2] { "GoodsId", "StartPrice" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(OnShelfTreasureResp_1006013), OnShelfTreasureResp_1006013.Parser, new string[3] { "Code", "IsSuccess", "NowRingCapacity" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(OnShelfListResp_1006014), OnShelfListResp_1006014.Parser, new string[3] { "Code", "IsSuccess", "GoodsList" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(OnShelfListResp_1006014.Types.OnShelfGoodsResp_1006015), OnShelfListResp_1006014.Types.OnShelfGoodsResp_1006015.Parser, new string[9] { "Id", "Name", "FiveElement", "AuctionTime", "Stage", "Rank", "Level", "GoodsType", "StartPrice" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(TreasureGoodsCancelReq_1006016), TreasureGoodsCancelReq_1006016.Parser, new string[1] { "TreasureId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TreasureGoodsCancelResp_1006016), TreasureGoodsCancelResp_1006016.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterTreasureReq_1006017), EnterTreasureReq_1006017.Parser, new string[1] { "Stage" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterTreasureResp_1006017), EnterTreasureResp_1006017.Parser, new string[5] { "Code", "IsSuccess", "ShinyStone", "TreasureId", "IsMayAddPrice" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TreasureGoodsListResp_1006018), TreasureGoodsListResp_1006018.Parser, new string[4] { "Code", "IsSuccess", "Stage", "GoodsList" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(TreasureGoodsListResp_1006018.Types.GoodsListResp_1006019), TreasureGoodsListResp_1006018.Types.GoodsListResp_1006019.Parser, new string[5] { "GoodsName", "FiveElement", "Rank", "FinalPrice", "Status" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(TreasureBuyReq_1006020), TreasureBuyReq_1006020.Parser, new string[2] { "Price", "TreasureId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TreasureBuyResp_1006020), TreasureBuyResp_1006020.Parser, new string[3] { "Code", "IsSuccess", "ShinyStone" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TreasureCurrentGoodsResp_1006021), TreasureCurrentGoodsResp_1006021.Parser, new string[16]
			{
				"Code", "IsSuccess", "GoodsId", "GoodsName", "FiveElement", "CurrentPerson", "Stage", "Rank", "Level", "GoodsType",
				"CurrentPrice", "PersonImage", "PersonSex", "PersonName", "Id", "StartPrice"
			}, null, null, null)
		}));
	}
}
