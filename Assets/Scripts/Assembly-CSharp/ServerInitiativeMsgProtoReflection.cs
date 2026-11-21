using System;
using Google.Protobuf.Reflection;

public static class ServerInitiativeMsgProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static ServerInitiativeMsgProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("Ch5TZXJ2ZXJJbml0aWF0aXZlTXNnUHJvdG8ucHJvdG8iKgoVSGVhcnRCZWF0" + "UmVzcF85MDAwMDAwEhEKCWhlYXJ0QmVhdBgBIAEoBSIrChNWaWdvclVwUmVz" + "cF85MDAxMDAxEhQKDGN1cnJlbnRWaWdvchgBIAEoBSItChRDbGV2ZXJVcFJl" + "c3BfOTAwMTAwMhIVCg1jdXJyZW50Q2xldmVyGAEgASgFIi8KFlBvc2lvbkRv" + "d25SZXNwXzkwMDEwMDMSFQoNY3VycmVudFBvc2lvbhgBIAEoBSKMAQoWU3Vw" + "ZXJBaXJVcFJlc3BfOTAwMTAwNBIXCg9jdXJyZW50U3VwZXJBaXIYASABKAMS" + "EAoIc3VwZXJBaXIYAiABKAMSDwoHaXNCcmVhaxgDIAEoCBIdChVzdXBlckFp" + "clJlZmluaW5nVmFsdWUYBCABKAMSFwoPY3VycmVudFJ1blZhbHVlGAUgASgB" + "IqgBChZQaHlzaXF1ZVVwUmVzcF85MDAxMDA1EhcKD2N1cnJlbnRQaHlzaXF1" + "ZRgBIAEoAxIQCghwaHlzaXF1ZRgCIAEoAxIYChBjdXJyZW50UHVyZUJsb29k" + "GAMgASgFEhEKCXB1cmVCbG9vZBgEIAEoBRIdChVwaHlzaXF1ZVJlZmluaW5n" + "VmFsdWUYBSABKAMSFwoPY3VycmVudFJ1blZhbHVlGAYgASgBIoMBChhQZXJj" + "ZXB0aW9uVXBSZXNwXzkwMDEwMDYSGQoRY3VycmVudFBlcmNlcHRpb24YASAB" + "KAMSEgoKcGVyY2VwdGlvbhgCIAEoAxIfChdwZXJjZXB0aW9uUmVmaW5pbmdW" + "YWx1ZRgDIAEoAxIXCg9jdXJyZW50UnVuVmFsdWUYBCABKAEiMAoRQWdlVXBS" + "ZXNwXzkwMDEwMDcSCwoDYWdlGAEgASgFEg4KBm1heEFnZRgCIAEoBSIrChNQ" + "b3dlclVwUmVzcF85MDAxMDA4EhQKDGN1cnJlbnRQb3dlchgBIAEoBSIWChRI" + "YW9ZdW5GdVJlc3BfOTAwMTAwOUI7Ch9jb20ubG9uZ21vLmdhbWVzZXJ2ZXIu" + "bmV0LnByb3RvQhhTZXJ2ZXJJbml0aWF0aXZlTXNnUHJvdG9iBnByb3RvMw=="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[10]
		{
			new GeneratedClrTypeInfo(typeof(HeartBeatResp_9000000), HeartBeatResp_9000000.Parser, new string[1] { "HeartBeat" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(VigorUpResp_9001001), VigorUpResp_9001001.Parser, new string[1] { "CurrentVigor" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(CleverUpResp_9001002), CleverUpResp_9001002.Parser, new string[1] { "CurrentClever" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(PosionDownResp_9001003), PosionDownResp_9001003.Parser, new string[1] { "CurrentPosion" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SuperAirUpResp_9001004), SuperAirUpResp_9001004.Parser, new string[5] { "CurrentSuperAir", "SuperAir", "IsBreak", "SuperAirRefiningValue", "CurrentRunValue" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(PhysiqueUpResp_9001005), PhysiqueUpResp_9001005.Parser, new string[6] { "CurrentPhysique", "Physique", "CurrentPureBlood", "PureBlood", "PhysiqueRefiningValue", "CurrentRunValue" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(PerceptionUpResp_9001006), PerceptionUpResp_9001006.Parser, new string[4] { "CurrentPerception", "Perception", "PerceptionRefiningValue", "CurrentRunValue" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(AgeUpResp_9001007), AgeUpResp_9001007.Parser, new string[2] { "Age", "MaxAge" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(PowerUpResp_9001008), PowerUpResp_9001008.Parser, new string[1] { "CurrentPower" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(HaoYunFuResp_9001009), HaoYunFuResp_9001009.Parser, null, null, null, null)
		}));
	}
}
