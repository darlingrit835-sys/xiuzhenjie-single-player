using System;
using Google.Protobuf.Reflection;

public static class HuoDongProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static HuoDongProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChJIdW9Eb25nUHJvdG8ucHJvdG8iLQocRW50ZXJDaG91SmlhbmdDaGlSZXFf" + "MTAxNzAwMRINCgVzdGFnZRgBIAEoBSJuCh1FbnRlckNob3VKaWFuZ0NoaVJl" + "c3BfMTAxNzAwMRIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBId" + "ChVjdXJyZW50Q2hvdUppYW5nSmlGZW4YAyABKAMSDQoFY291bnQYBCABKAMi" + "OwoUQ2hvdUppYW5nUmVxXzEwMTcwMDISDQoFc3RhZ2UYASABKAUSFAoMaXND" + "b250aW51b3VzGAIgASgIIukCChVDaG91SmlhbmdSZXNwXzEwMTcwMDISDAoE" + "Y29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSPwoJZ29vZHNJbmZvGAMg" + "AygLMiwuQ2hvdUppYW5nUmVzcF8xMDE3MDAyLkdvb2RzSW5mb1Jlc3BfMTAx" + "NzAwMxIYChBjdXJyZW50U2hpbnlKYWRlGAQgASgDEh0KFWN1cnJlbnRDaG91" + "SmlhbmdKaUZlbhgFIAEoAxINCgVjb3VudBgGIAEoBRqlAQoVR29vZHNJbmZv" + "UmVzcF8xMDE3MDAzEg8KB2dvb2RzSWQYASABKAkSDAoEbmFtZRgCIAEoCRIT" + "CgtmaXZlRWxlbWVudBgDIAEoBRIMCgRyYW5rGAQgASgFEg0KBWxldmVsGAUg" + "ASgFEgwKBHR5cGUYBiABKAUSCgoCaWQYByABKAUSDQoFY291bnQYCCABKAUS" + "EgoKdGVtcGxhdGVJZBgJIAEoCSLPAQoWRW50ZXJKaUZlblJlc3BfMTAxNzAw" + "NBIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBI4CgVqaUZlbhgD" + "IAMoCzIpLkVudGVySmlGZW5SZXNwXzEwMTcwMDQuSmlGZW5SZXNwXzEwMTcw" + "MDUaWgoRSmlGZW5SZXNwXzEwMTcwMDUSCgoCaWQYASABKAUSDQoFamlGZW4Y" + "AiABKAUSDAoEbmFtZRgDIAEoCRINCgVjb3VudBgEIAEoBRINCgVpc0dldBgF" + "IAEoCCIhChNHZXRKaUZlblJlcV8xMDE3MDA2EgoKAmlkGAEgASgFIjcKFEdl" + "dEppRmVuUmVzcF8xMDE3MDA2EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNz" + "GAIgASgIQi8KH2NvbS5sb25nbW8uZ2FtZXNlcnZlci5uZXQucHJvdG9CDEh1" + "b0RvbmdQcm90b2IGcHJvdG8z"), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[7]
		{
			new GeneratedClrTypeInfo(typeof(EnterChouJiangChiReq_1017001), EnterChouJiangChiReq_1017001.Parser, new string[1] { "Stage" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterChouJiangChiResp_1017001), EnterChouJiangChiResp_1017001.Parser, new string[4] { "Code", "IsSuccess", "CurrentChouJiangJiFen", "Count" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChouJiangReq_1017002), ChouJiangReq_1017002.Parser, new string[2] { "Stage", "IsContinuous" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChouJiangResp_1017002), ChouJiangResp_1017002.Parser, new string[6] { "Code", "IsSuccess", "GoodsInfo", "CurrentShinyJade", "CurrentChouJiangJiFen", "Count" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(ChouJiangResp_1017002.Types.GoodsInfoResp_1017003), ChouJiangResp_1017002.Types.GoodsInfoResp_1017003.Parser, new string[9] { "GoodsId", "Name", "FiveElement", "Rank", "Level", "Type", "Id", "Count", "TemplateId" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(EnterJiFenResp_1017004), EnterJiFenResp_1017004.Parser, new string[3] { "Code", "IsSuccess", "JiFen" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterJiFenResp_1017004.Types.JiFenResp_1017005), EnterJiFenResp_1017004.Types.JiFenResp_1017005.Parser, new string[5] { "Id", "JiFen", "Name", "Count", "IsGet" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(GetJiFenReq_1017006), GetJiFenReq_1017006.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetJiFenResp_1017006), GetJiFenResp_1017006.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null)
		}));
	}
}
