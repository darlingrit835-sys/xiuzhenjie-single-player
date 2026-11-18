using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ChatJoinReq_4001001 : IMessage<ChatJoinReq_4001001>, IMessage, IEquatable<ChatJoinReq_4001001>, IDeepCloneable<ChatJoinReq_4001001>
{
	private static readonly MessageParser<ChatJoinReq_4001001> _parser = new MessageParser<ChatJoinReq_4001001>(() => new ChatJoinReq_4001001());

	private UnknownFieldSet _unknownFields;

	public const int RoleIdFieldNumber = 1;

	private string roleId_ = "";

	public const int RoleNameFieldNumber = 2;

	private string roleName_ = "";

	public const int TitleFieldNumber = 3;

	private string title_ = "";

	public const int LevelFieldNumber = 4;

	private int level_;

	public const int SexFieldNumber = 5;

	private int sex_;

	public const int HeadPortraitIdFieldNumber = 6;

	private string headPortraitId_ = "";

	public const int ChatIdFieldNumber = 7;

	private string chatId_ = "";

	public const int TitleFrameFieldNumber = 8;

	private int titleFrame_;

	public const int HeadFrameFieldNumber = 9;

	private int headFrame_;

	[DebuggerNonUserCode]
	public static MessageParser<ChatJoinReq_4001001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientChatProtoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string RoleId
	{
		get
		{
			return roleId_;
		}
		set
		{
			roleId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string RoleName
	{
		get
		{
			return roleName_;
		}
		set
		{
			roleName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Title
	{
		get
		{
			return title_;
		}
		set
		{
			title_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Sex
	{
		get
		{
			return sex_;
		}
		set
		{
			sex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string HeadPortraitId
	{
		get
		{
			return headPortraitId_;
		}
		set
		{
			headPortraitId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string ChatId
	{
		get
		{
			return chatId_;
		}
		set
		{
			chatId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int TitleFrame
	{
		get
		{
			return titleFrame_;
		}
		set
		{
			titleFrame_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HeadFrame
	{
		get
		{
			return headFrame_;
		}
		set
		{
			headFrame_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ChatJoinReq_4001001()
	{
	}

	[DebuggerNonUserCode]
	public ChatJoinReq_4001001(ChatJoinReq_4001001 other)
		: this()
	{
		roleId_ = other.roleId_;
		roleName_ = other.roleName_;
		title_ = other.title_;
		level_ = other.level_;
		sex_ = other.sex_;
		headPortraitId_ = other.headPortraitId_;
		chatId_ = other.chatId_;
		titleFrame_ = other.titleFrame_;
		headFrame_ = other.headFrame_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ChatJoinReq_4001001 Clone()
	{
		return new ChatJoinReq_4001001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ChatJoinReq_4001001);
	}

	[DebuggerNonUserCode]
	public bool Equals(ChatJoinReq_4001001 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (RoleId != other.RoleId)
		{
			return false;
		}
		if (RoleName != other.RoleName)
		{
			return false;
		}
		if (Title != other.Title)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (Sex != other.Sex)
		{
			return false;
		}
		if (HeadPortraitId != other.HeadPortraitId)
		{
			return false;
		}
		if (ChatId != other.ChatId)
		{
			return false;
		}
		if (TitleFrame != other.TitleFrame)
		{
			return false;
		}
		if (HeadFrame != other.HeadFrame)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (RoleId.Length != 0)
		{
			num ^= RoleId.GetHashCode();
		}
		if (RoleName.Length != 0)
		{
			num ^= RoleName.GetHashCode();
		}
		if (Title.Length != 0)
		{
			num ^= Title.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (Sex != 0)
		{
			num ^= Sex.GetHashCode();
		}
		if (HeadPortraitId.Length != 0)
		{
			num ^= HeadPortraitId.GetHashCode();
		}
		if (ChatId.Length != 0)
		{
			num ^= ChatId.GetHashCode();
		}
		if (TitleFrame != 0)
		{
			num ^= TitleFrame.GetHashCode();
		}
		if (HeadFrame != 0)
		{
			num ^= HeadFrame.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		if (RoleId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(RoleId);
		}
		if (RoleName.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(RoleName);
		}
		if (Title.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Title);
		}
		if (Level != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Level);
		}
		if (Sex != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Sex);
		}
		if (HeadPortraitId.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(HeadPortraitId);
		}
		if (ChatId.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(ChatId);
		}
		if (TitleFrame != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(TitleFrame);
		}
		if (HeadFrame != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(HeadFrame);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (RoleId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RoleId);
		}
		if (RoleName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RoleName);
		}
		if (Title.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Title);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (Sex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Sex);
		}
		if (HeadPortraitId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(HeadPortraitId);
		}
		if (ChatId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ChatId);
		}
		if (TitleFrame != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TitleFrame);
		}
		if (HeadFrame != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeadFrame);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ChatJoinReq_4001001 other)
	{
		if (other != null)
		{
			if (other.RoleId.Length != 0)
			{
				RoleId = other.RoleId;
			}
			if (other.RoleName.Length != 0)
			{
				RoleName = other.RoleName;
			}
			if (other.Title.Length != 0)
			{
				Title = other.Title;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.Sex != 0)
			{
				Sex = other.Sex;
			}
			if (other.HeadPortraitId.Length != 0)
			{
				HeadPortraitId = other.HeadPortraitId;
			}
			if (other.ChatId.Length != 0)
			{
				ChatId = other.ChatId;
			}
			if (other.TitleFrame != 0)
			{
				TitleFrame = other.TitleFrame;
			}
			if (other.HeadFrame != 0)
			{
				HeadFrame = other.HeadFrame;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 10u:
				RoleId = input.ReadString();
				break;
			case 18u:
				RoleName = input.ReadString();
				break;
			case 26u:
				Title = input.ReadString();
				break;
			case 32u:
				Level = input.ReadInt32();
				break;
			case 40u:
				Sex = input.ReadInt32();
				break;
			case 50u:
				HeadPortraitId = input.ReadString();
				break;
			case 58u:
				ChatId = input.ReadString();
				break;
			case 64u:
				TitleFrame = input.ReadInt32();
				break;
			case 72u:
				HeadFrame = input.ReadInt32();
				break;
			}
		}
	}
}
