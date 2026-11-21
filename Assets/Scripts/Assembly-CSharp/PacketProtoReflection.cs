using System;
using Google.Protobuf.Reflection;

public static class PacketProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static PacketProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChFQYWNrZXRQcm90by5wcm90byIuCgNNc2cSCwoDY21kGAEgASgFEgwKBG1z" + "aWQYAiABKAUSDAoEZGF0YRgDIAEoDEIuCh9jb20ubG9uZ21vLmdhbWVzZXJ2" + "ZXIubmV0LnByb3RvQgtQYWNrZXRQcm90b2IGcHJvdG8z"), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[1]
		{
			new GeneratedClrTypeInfo(typeof(Msg), Msg.Parser, new string[3] { "Cmd", "Msid", "Data" }, null, null, null)
		}));
	}
}
