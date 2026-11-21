using System.Collections.Generic;
using Google.Protobuf;

namespace XiuZhenJie
{
	internal sealed class FriendChatPage
	{
		internal readonly List<FriendEntity> Messages = new List<FriendEntity>();
		internal bool Success;
		internal int Code;
		internal bool HasMore;

		internal static SendMessageRoleReq_4001007 Request(string friendId, long before)
		{
			var request = new SendMessageRoleReq_4001007 { RoleId = friendId };
			if (before > 0)
			{
				// The paged query extends the original role selector with field 3.
				byte[] cursor = new byte[1 + CodedOutputStream.ComputeInt64Size(before)];
				var output = new CodedOutputStream(cursor);
				output.WriteTag(3, WireFormat.WireType.Varint);
				output.WriteInt64(before);
				output.Flush();
				request.MergeFrom(cursor);
			}
			return request;
		}

		internal static FriendChatPage Read(ByteString data)
		{
			var page = new FriendChatPage();
			var input = data.CreateCodedInput();
			uint tag;
			while ((tag = input.ReadTag()) != 0)
			{
				switch (tag)
				{
				case 8:
					page.Code = input.ReadInt32();
					break;
				case 16:
					page.Success = input.ReadBool();
					break;
				case 26:
					var row = new ReceiveMessageRoleResp_4001008();
					input.ReadMessage(row);
					page.Messages.Add(new FriendEntity
					{
						FromRoleId = row.RoleId, Content = row.Msg, Timestamp = row.MsgTime,
						ReceiveTime = CommonMethod.GetTimeStampTime((row.MsgTime / 1000).ToString()).ToString("yyyy/MM/dd  HH:mm")
					});
					break;
				case 32:
					page.HasMore = input.ReadBool();
					break;
				default:
					input.SkipLastField();
					break;
				}
			}
			return page;
		}
	}
}
