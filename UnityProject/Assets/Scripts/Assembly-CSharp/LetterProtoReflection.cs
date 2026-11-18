using System;
using Google.Protobuf.Reflection;

public static class LetterProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static LetterProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChFMZXR0ZXJQcm90by5wcm90byLhAwoXTGV0dGVyUXVlcnlSZXNwXzEwMDMw" + "MDESDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSQAoHbGV0dGVy" + "cxgDIAMoCzIvLkxldHRlclF1ZXJ5UmVzcF8xMDAzMDAxLkxldHRlckluZm9S" + "ZXNwXzEwMDMwMDIa4gIKFkxldHRlckluZm9SZXNwXzEwMDMwMDISCgoCaWQY" + "ASABKAkSDwoHY29udGVudBgCIAEoCRIQCghoYXNBd2FyZBgDIAEoCBIOCgZz" + "dGF0dXMYBCABKAgSDQoFdGl0bGUYBSABKAkSVwoGYXdhcmRzGAYgAygLMkcu" + "TGV0dGVyUXVlcnlSZXNwXzEwMDMwMDEuTGV0dGVySW5mb1Jlc3BfMTAwMzAw" + "Mi5MZXR0ZXJBd2FyZFJlc3BfMTAwMzAwMxIQCghpc0dldEFsbBgHIAEoCBqO" + "AQoXTGV0dGVyQXdhcmRSZXNwXzEwMDMwMDMSCgoCaWQYASABKAkSDwoHZ29v" + "ZHNJZBgCIAEoCRIRCglnb29kc05hbWUYAyABKAkSEQoJZ29vZHNUeXBlGAQg" + "ASgFEhIKCmdvb2RzQ291bnQYBSABKAMSDgoGc3RhdHVzGAYgASgIEgwKBHJh" + "bmsYByABKAUiJQoXRGVsZXRlTGV0dGVyUmVxXzEwMDMwMDQSCgoCaWQYASAD" + "KAkiIwoVUmVhZExldHRlclJlcV8xMDAzMDA1EgoKAmlkGAEgASgJIiYKGFJl" + "Y2VpdmVMZXR0ZXJSZXFfMTAwMzAwNhIKCgJpZBgBIAEoCSJjChlSZWNlaXZl" + "TGV0dGVyUmVzcF8xMDAzMDA2EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNz" + "GAIgASgIEhIKCnNoaW55U3RvbmUYAyABKAMSEQoJc2hpbnlKYWRlGAQgASgD" + "IhoKGExldHRlck5vdGlmeVJlc3BfMTAwMzAwN0IuCh9jb20ubG9uZ21vLmdh" + "bWVzZXJ2ZXIubmV0LnByb3RvQgtMZXR0ZXJQcm90b2IGcHJvdG8z"), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[6]
		{
			new GeneratedClrTypeInfo(typeof(LetterQueryResp_1003001), LetterQueryResp_1003001.Parser, new string[3] { "Code", "IsSuccess", "Letters" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(LetterQueryResp_1003001.Types.LetterInfoResp_1003002), LetterQueryResp_1003001.Types.LetterInfoResp_1003002.Parser, new string[7] { "Id", "Content", "HasAward", "Status", "Title", "Awards", "IsGetAll" }, null, null, new GeneratedClrTypeInfo[1]
				{
					new GeneratedClrTypeInfo(typeof(LetterQueryResp_1003001.Types.LetterInfoResp_1003002.Types.LetterAwardResp_1003003), LetterQueryResp_1003001.Types.LetterInfoResp_1003002.Types.LetterAwardResp_1003003.Parser, new string[7] { "Id", "GoodsId", "GoodsName", "GoodsType", "GoodsCount", "Status", "Rank" }, null, null, null)
				})
			}),
			new GeneratedClrTypeInfo(typeof(DeleteLetterReq_1003004), DeleteLetterReq_1003004.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ReadLetterReq_1003005), ReadLetterReq_1003005.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ReceiveLetterReq_1003006), ReceiveLetterReq_1003006.Parser, new string[1] { "Id" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ReceiveLetterResp_1003006), ReceiveLetterResp_1003006.Parser, new string[4] { "Code", "IsSuccess", "ShinyStone", "ShinyJade" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(LetterNotifyResp_1003007), LetterNotifyResp_1003007.Parser, null, null, null, null)
		}));
	}
}
