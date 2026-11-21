using System;
using Google.Protobuf.Reflection;

public static class TaskProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static TaskProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("Cg9UYXNrUHJvdG8ucHJvdG8iIgogSW5mb3JtVGFza0NvbXBsZXRpb25SZXNw" + "XzEwMTYwMDEiKQoXUmVjZWl2ZUF3YXJkUmVxXzEwMTYwMDISDgoGdGFza0lk" + "GAEgASgFIksKGFJlY2VpdmVBd2FyZFJlc3BfMTAxNjAwMhIMCgRjb2RlGAEg" + "ASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIOCgZzdGF0dXMYAyABKAUiTQocSW5m" + "b3JtVGlhblNodUJlaVJlc3BfMTAxNjAwMxIMCgRjb2RlGAEgASgFEhEKCWlz" + "U3VjY2VzcxgCIAEoCBIMCgR0eXBlGAMgASgFIvYBChpPcGVuVGlhblNodUJl" + "aVJlc3BfMTAxNjAwNBIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEo" + "CBJMCg10aWFuU2h1QmVpT2JqGAMgAygLMjUuT3BlblRpYW5TaHVCZWlSZXNw" + "XzEwMTYwMDQuVGlhblNodUJlaU9ialJlc3BfMTAxNjAwNRppChlUaWFuU2h1" + "QmVpT2JqUmVzcF8xMDE2MDA1EgoKAmlkGAEgASgFEgwKBHR5cGUYAiABKAUS" + "DgoGbWF4VmFsGAMgASgFEhIKCmN1cnJlbnRWYWwYBCABKAUSDgoGc3RhdHVz" + "GAUgASgFIj0KIVRpYW5TaHVCZWlSZWNlaXZlQXdhcmRSZXFfMTAxNjAwNhIK" + "CgJpZBgBIAEoBRIMCgR0eXBlGAIgASgFInEKIlRpYW5TaHVCZWlSZWNlaXZl" + "QXdhcmRSZXNwXzEwMTYwMDYSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MY" + "AiABKAgSCgoCaWQYAyABKAUSDgoGbWF4VmFsGAQgASgFEg4KBnN0YXR1cxgF" + "IAEoBUIsCh9jb20ubG9uZ21vLmdhbWVzZXJ2ZXIubmV0LnByb3RvQglUYXNr" + "UHJvdG9iBnByb3RvMw=="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[7]
		{
			new GeneratedClrTypeInfo(typeof(InformTaskCompletionResp_1016001), InformTaskCompletionResp_1016001.Parser, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ReceiveAwardReq_1016002), ReceiveAwardReq_1016002.Parser, new string[1] { "TaskId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ReceiveAwardResp_1016002), ReceiveAwardResp_1016002.Parser, new string[3] { "Code", "IsSuccess", "Status" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(InformTianShuBeiResp_1016003), InformTianShuBeiResp_1016003.Parser, new string[3] { "Code", "IsSuccess", "Type" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(OpenTianShuBeiResp_1016004), OpenTianShuBeiResp_1016004.Parser, new string[3] { "Code", "IsSuccess", "TianShuBeiObj" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(OpenTianShuBeiResp_1016004.Types.TianShuBeiObjResp_1016005), OpenTianShuBeiResp_1016004.Types.TianShuBeiObjResp_1016005.Parser, new string[5] { "Id", "Type", "MaxVal", "CurrentVal", "Status" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(TianShuBeiReceiveAwardReq_1016006), TianShuBeiReceiveAwardReq_1016006.Parser, new string[2] { "Id", "Type" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TianShuBeiReceiveAwardResp_1016006), TianShuBeiReceiveAwardResp_1016006.Parser, new string[5] { "Code", "IsSuccess", "Id", "MaxVal", "Status" }, null, null, null)
		}));
	}
}
