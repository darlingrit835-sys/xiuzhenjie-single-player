using System;
using Google.Protobuf.Reflection;

public static class ClientChatProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static ClientChatProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChVDbGllbnRDaGF0UHJvdG8ucHJvdG8iswEKE0NoYXRKb2luUmVxXzQwMDEw" + "MDESDgoGcm9sZUlkGAEgASgJEhAKCHJvbGVOYW1lGAIgASgJEg0KBXRpdGxl" + "GAMgASgJEg0KBWxldmVsGAQgASgFEgsKA3NleBgFIAEoBRIYChBoZWFkX3Bv" + "cnRyYWl0X2lkGAYgASgJEg4KBmNoYXRJZBgHIAEoCRISCgp0aXRsZUZyYW1l" + "GAggASgFEhEKCWhlYWRGcmFtZRgJIAEoBSJ0ChRDaGF0Sm9pblJlc3BfNDAw" + "MTAwMRIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIUCgxjcmVk" + "aXRQb2ludHMYBiABKAUSDwoHdGltZUxhZxgHIAEoBRIUCgxuZXh0U2VuZFRp" + "bWUYCCABKAMi9gEKGlJlY2VpdmVNZXNzYWdlUmVzcF80MDAxMDAyEgwKBGNv" + "ZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEhAKCHJvbGVOYW1lGAMgASgJ" + "Eg0KBXRpdGxlGAQgASgJEgsKA21zZxgFIAEoCRIOCgZyb2xlSWQYBiABKAkS" + "DQoFbGV2ZWwYByABKAUSCwoDc2V4GAggASgFEhgKEGhlYWRfcG9ydHJhaXRf" + "aWQYCSABKAkSDgoGY2hhdElkGAogASgJEgwKBHR5cGUYCyABKAUSEgoKdGl0" + "bGVGcmFtZRgMIAEoBRIRCgloZWFkRnJhbWUYDSABKAUiNQoWU2VuZE1lc3Nh" + "Z2VSZXFfNDAwMTAwMxILCgNtc2cYASABKAkSDgoGY2hhdElkGAIgASgJIogB" + "ChdTZW5kTWVzc2FnZVJlc3BfNDAwMTAwMxIMCgRjb2RlGAEgASgFEhEKCWlz" + "U3VjY2VzcxgCIAEoCBIPCgdjaGF0TnVtGAMgASgFEhQKDG5leHRTZW5kVGlt" + "ZRgEIAEoAxIUCgxjcmVkaXRQb2ludHMYBSABKAUSDwoHdGltZUxhZxgGIAEo" + "BSIkChJXaW5lT3V0UmVxXzQwMDEwMDQSDgoGY2hhdElkGAEgASgJIjYKE1dp" + "bmVPdXRSZXNwXzQwMDEwMDQSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MY" + "AiABKAgiKwoZU2VhcmNoSXNPbkxpbmVSZXFfNDAwMTAwNRIOCgZyb2xlSWQY" + "ASADKAkiwQEKGlNlYXJjaElzT25MaW5lUmVzcF80MDAxMDA1EgwKBGNvZGUY" + "ASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEkYKCnJvbGVPbkxpbmUYAyADKAsy" + "Mi5TZWFyY2hJc09uTGluZVJlc3BfNDAwMTAwNS5Sb2xlT25MaW5lUmVzcF80" + "MDAxMDA2GjoKFlJvbGVPbkxpbmVSZXNwXzQwMDEwMDYSDgoGcm9sZUlkGAEg" + "ASgJEhAKCGlzT25MaW5lGAIgASgIIjkKGlNlbmRNZXNzYWdlUm9sZVJlcV80" + "MDAxMDA3EgsKA21zZxgBIAEoCRIOCgZyb2xlSWQYAiABKAkiTwobU2VuZE1l" + "c3NhZ2VSb2xlUmVzcF80MDAxMDA3EgwKBGNvZGUYASABKAUSEQoJaXNTdWNj" + "ZXNzGAIgASgIEg8KB21zZ1RpbWUYAyABKAMibwoeUmVjZWl2ZU1lc3NhZ2VS" + "b2xlUmVzcF80MDAxMDA4EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIg" + "ASgIEgsKA21zZxgFIAEoCRIOCgZyb2xlSWQYBiABKAkSDwoHbXNnVGltZRgE" + "IAEoA0IsChljb20ubG9uZ21vLmNoYXQubmV0LnByb3RvQg9DbGllbnRDaGF0" + "UHJvdG9iBnByb3RvMw=="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[12]
		{
			new GeneratedClrTypeInfo(typeof(ChatJoinReq_4001001), ChatJoinReq_4001001.Parser, new string[9] { "RoleId", "RoleName", "Title", "Level", "Sex", "HeadPortraitId", "ChatId", "TitleFrame", "HeadFrame" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChatJoinResp_4001001), ChatJoinResp_4001001.Parser, new string[5] { "Code", "IsSuccess", "CreditPoints", "TimeLag", "NextSendTime" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ReceiveMessageResp_4001002), ReceiveMessageResp_4001002.Parser, new string[13]
			{
				"Code", "IsSuccess", "RoleName", "Title", "Msg", "RoleId", "Level", "Sex", "HeadPortraitId", "ChatId",
				"Type", "TitleFrame", "HeadFrame"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(SendMessageReq_4001003), SendMessageReq_4001003.Parser, new string[2] { "Msg", "ChatId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SendMessageResp_4001003), SendMessageResp_4001003.Parser, new string[6] { "Code", "IsSuccess", "ChatNum", "NextSendTime", "CreditPoints", "TimeLag" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(WineOutReq_4001004), WineOutReq_4001004.Parser, new string[1] { "ChatId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(WineOutResp_4001004), WineOutResp_4001004.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SearchIsOnLineReq_4001005), SearchIsOnLineReq_4001005.Parser, new string[1] { "RoleId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SearchIsOnLineResp_4001005), SearchIsOnLineResp_4001005.Parser, new string[3] { "Code", "IsSuccess", "RoleOnLine" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(SearchIsOnLineResp_4001005.Types.RoleOnLineResp_4001006), SearchIsOnLineResp_4001005.Types.RoleOnLineResp_4001006.Parser, new string[2] { "RoleId", "IsOnLine" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(SendMessageRoleReq_4001007), SendMessageRoleReq_4001007.Parser, new string[2] { "Msg", "RoleId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SendMessageRoleResp_4001007), SendMessageRoleResp_4001007.Parser, new string[3] { "Code", "IsSuccess", "MsgTime" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ReceiveMessageRoleResp_4001008), ReceiveMessageRoleResp_4001008.Parser, new string[5] { "Code", "IsSuccess", "Msg", "RoleId", "MsgTime" }, null, null, null)
		}));
	}
}
