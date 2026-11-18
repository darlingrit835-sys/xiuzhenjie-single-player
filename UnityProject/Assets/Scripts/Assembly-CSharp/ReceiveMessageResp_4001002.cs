using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ReceiveMessageResp_4001002 : IMessage<ReceiveMessageResp_4001002>, IMessage, IEquatable<ReceiveMessageResp_4001002>, IDeepCloneable<ReceiveMessageResp_4001002>
{
	private static readonly MessageParser<ReceiveMessageResp_4001002> _parser = new MessageParser<ReceiveMessageResp_4001002>(() => new ReceiveMessageResp_4001002());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int RoleNameFieldNumber = 3;

	private string roleName_ = "";

	public const int TitleFieldNumber = 4;

	private string title_ = "";

	public const int MsgFieldNumber = 5;

	private string msg_ = "";

	public const int RoleIdFieldNumber = 6;

	private string roleId_ = "";

	public const int LevelFieldNumber = 7;

	private int level_;

	public const int SexFieldNumber = 8;

	private int sex_;

	public const int HeadPortraitIdFieldNumber = 9;

	private string headPortraitId_ = "";

	public const int ChatIdFieldNumber = 10;

	private string chatId_ = "";

	public const int TypeFieldNumber = 11;

	private int type_;

	public const int TitleFrameFieldNumber = 12;

	private int titleFrame_;

	public const int HeadFrameFieldNumber = 13;

	private int headFrame_;

	[DebuggerNonUserCode]
	public static MessageParser<ReceiveMessageResp_4001002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientChatProtoReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Code
	{
		get
		{
			return code_;
		}
		set
		{
			code_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsSuccess
	{
		get
		{
			return isSuccess_;
		}
		set
		{
			isSuccess_ = value;
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
	public string Msg
	{
		get
		{
			return msg_;
		}
		set
		{
			msg_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

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
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
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
	public ReceiveMessageResp_4001002()
	{
	}

	[DebuggerNonUserCode]
	public ReceiveMessageResp_4001002(ReceiveMessageResp_4001002 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		roleName_ = other.roleName_;
		title_ = other.title_;
		msg_ = other.msg_;
		roleId_ = other.roleId_;
		level_ = other.level_;
		sex_ = other.sex_;
		headPortraitId_ = other.headPortraitId_;
		chatId_ = other.chatId_;
		type_ = other.type_;
		titleFrame_ = other.titleFrame_;
		headFrame_ = other.headFrame_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ReceiveMessageResp_4001002 Clone()
	{
		return new ReceiveMessageResp_4001002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ReceiveMessageResp_4001002);
	}

	[DebuggerNonUserCode]
	public bool Equals(ReceiveMessageResp_4001002 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Code != other.Code)
		{
			return false;
		}
		if (IsSuccess != other.IsSuccess)
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
		if (Msg != other.Msg)
		{
			return false;
		}
		if (RoleId != other.RoleId)
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
		if (Type != other.Type)
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
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		if (RoleName.Length != 0)
		{
			num ^= RoleName.GetHashCode();
		}
		if (Title.Length != 0)
		{
			num ^= Title.GetHashCode();
		}
		if (Msg.Length != 0)
		{
			num ^= Msg.GetHashCode();
		}
		if (RoleId.Length != 0)
		{
			num ^= RoleId.GetHashCode();
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
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
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
		if (Code != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Code);
		}
		if (IsSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsSuccess);
		}
		if (RoleName.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(RoleName);
		}
		if (Title.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(Title);
		}
		if (Msg.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(Msg);
		}
		if (RoleId.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(RoleId);
		}
		if (Level != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Level);
		}
		if (Sex != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(Sex);
		}
		if (HeadPortraitId.Length != 0)
		{
			output.WriteRawTag(74);
			output.WriteString(HeadPortraitId);
		}
		if (ChatId.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(ChatId);
		}
		if (Type != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(Type);
		}
		if (TitleFrame != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(TitleFrame);
		}
		if (HeadFrame != 0)
		{
			output.WriteRawTag(104);
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
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		if (RoleName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RoleName);
		}
		if (Title.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Title);
		}
		if (Msg.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Msg);
		}
		if (RoleId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RoleId);
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
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
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
	public void MergeFrom(ReceiveMessageResp_4001002 other)
	{
		if (other != null)
		{
			if (other.Code != 0)
			{
				Code = other.Code;
			}
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
			if (other.RoleName.Length != 0)
			{
				RoleName = other.RoleName;
			}
			if (other.Title.Length != 0)
			{
				Title = other.Title;
			}
			if (other.Msg.Length != 0)
			{
				Msg = other.Msg;
			}
			if (other.RoleId.Length != 0)
			{
				RoleId = other.RoleId;
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
			if (other.Type != 0)
			{
				Type = other.Type;
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
			case 8u:
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 26u:
				RoleName = input.ReadString();
				break;
			case 34u:
				Title = input.ReadString();
				break;
			case 42u:
				Msg = input.ReadString();
				break;
			case 50u:
				RoleId = input.ReadString();
				break;
			case 56u:
				Level = input.ReadInt32();
				break;
			case 64u:
				Sex = input.ReadInt32();
				break;
			case 74u:
				HeadPortraitId = input.ReadString();
				break;
			case 82u:
				ChatId = input.ReadString();
				break;
			case 88u:
				Type = input.ReadInt32();
				break;
			case 96u:
				TitleFrame = input.ReadInt32();
				break;
			case 104u:
				HeadFrame = input.ReadInt32();
				break;
			}
		}
	}
}
