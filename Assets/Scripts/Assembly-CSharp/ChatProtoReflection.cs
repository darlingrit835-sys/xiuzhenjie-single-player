using System;
using Google.Protobuf.Reflection;

public static class ChatProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static ChatProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("Cg9DaGF0UHJvdG8ucHJvdG8iVwoVQnVpbGRDaGF0UmVzcF8xMDEyMDAxEgwK" + "BGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEg8KB2FkZHJlc3MYAyAB" + "KAkSDAoEcG9ydBgEIAEoBSImChJCdXlXaW5lUmVxXzEwMTIwMDISEAoId2lu" + "ZVR5cGUYAiABKAUibgoTQnV5V2luZVJlc3BfMTAxMjAwMhIMCgRjb2RlGAEg" + "ASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIPCgdjaGF0TnVtGAMgASgFEhIKCnNo" + "aW55U3RvbmUYBCABKAMSEQoJc2hpbnlKYWRlGAUgASgDIsUBChRXaW5lTGlz" + "dFJlc3BfMTAxMjAwMxIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEo" + "CBI0CgR3aW5lGAMgAygLMiYuV2luZUxpc3RSZXNwXzEwMTIwMDMuV2luZVJl" + "c3BfMTAxMjAwNBpWChBXaW5lUmVzcF8xMDEyMDA0Eg8KB2NoYXROdW0YASAB" + "KAUSEgoKc2hpbnlTdG9uZRgCIAEoAxIRCglzaGlueUphZGUYAyABKAMSCgoC" + "aWQYBCABKAUiJQoVQ2hhdEluZm9ybVJlcV8xMDEyMDA1EgwKBHR5cGUYASAB" + "KAVCLAofY29tLmxvbmdtby5nYW1lc2VydmVyLm5ldC5wcm90b0IJQ2hhdFBy" + "b3RvYgZwcm90bzM="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[5]
		{
			new GeneratedClrTypeInfo(typeof(BuildChatResp_1012001), BuildChatResp_1012001.Parser, new string[4] { "Code", "IsSuccess", "Address", "Port" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BuyWineReq_1012002), BuyWineReq_1012002.Parser, new string[1] { "WineType" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BuyWineResp_1012002), BuyWineResp_1012002.Parser, new string[5] { "Code", "IsSuccess", "ChatNum", "ShinyStone", "ShinyJade" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(WineListResp_1012003), WineListResp_1012003.Parser, new string[3] { "Code", "IsSuccess", "Wine" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(WineListResp_1012003.Types.WineResp_1012004), WineListResp_1012003.Types.WineResp_1012004.Parser, new string[4] { "ChatNum", "ShinyStone", "ShinyJade", "Id" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(ChatInformReq_1012005), ChatInformReq_1012005.Parser, new string[1] { "Type" }, null, null, null)
		}));
	}
}
