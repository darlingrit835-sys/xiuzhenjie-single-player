using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildInformRoleJoinSuccessResp_1015018 : IMessage<GuildInformRoleJoinSuccessResp_1015018>, IMessage, IEquatable<GuildInformRoleJoinSuccessResp_1015018>, IDeepCloneable<GuildInformRoleJoinSuccessResp_1015018>
{
	private static readonly MessageParser<GuildInformRoleJoinSuccessResp_1015018> _parser = new MessageParser<GuildInformRoleJoinSuccessResp_1015018>(() => new GuildInformRoleJoinSuccessResp_1015018());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int GuildIdFieldNumber = 3;

	private int guildId_;

	public const int GuildNameFieldNumber = 4;

	private string guildName_ = "";

	public const int GuildSimpleNameFieldNumber = 5;

	private string guildSimpleName_ = "";

	public const int TokenIndexFieldNumber = 6;

	private int tokenIndex_;

	public const int LastNameIndexFieldNumber = 7;

	private int lastNameIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildInformRoleJoinSuccessResp_1015018> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[21];

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
	public int GuildId
	{
		get
		{
			return guildId_;
		}
		set
		{
			guildId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string GuildName
	{
		get
		{
			return guildName_;
		}
		set
		{
			guildName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string GuildSimpleName
	{
		get
		{
			return guildSimpleName_;
		}
		set
		{
			guildSimpleName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int TokenIndex
	{
		get
		{
			return tokenIndex_;
		}
		set
		{
			tokenIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LastNameIndex
	{
		get
		{
			return lastNameIndex_;
		}
		set
		{
			lastNameIndex_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildInformRoleJoinSuccessResp_1015018()
	{
	}

	[DebuggerNonUserCode]
	public GuildInformRoleJoinSuccessResp_1015018(GuildInformRoleJoinSuccessResp_1015018 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		guildId_ = other.guildId_;
		guildName_ = other.guildName_;
		guildSimpleName_ = other.guildSimpleName_;
		tokenIndex_ = other.tokenIndex_;
		lastNameIndex_ = other.lastNameIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildInformRoleJoinSuccessResp_1015018 Clone()
	{
		return new GuildInformRoleJoinSuccessResp_1015018(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildInformRoleJoinSuccessResp_1015018);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildInformRoleJoinSuccessResp_1015018 other)
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
		if (GuildId != other.GuildId)
		{
			return false;
		}
		if (GuildName != other.GuildName)
		{
			return false;
		}
		if (GuildSimpleName != other.GuildSimpleName)
		{
			return false;
		}
		if (TokenIndex != other.TokenIndex)
		{
			return false;
		}
		if (LastNameIndex != other.LastNameIndex)
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
		if (GuildId != 0)
		{
			num ^= GuildId.GetHashCode();
		}
		if (GuildName.Length != 0)
		{
			num ^= GuildName.GetHashCode();
		}
		if (GuildSimpleName.Length != 0)
		{
			num ^= GuildSimpleName.GetHashCode();
		}
		if (TokenIndex != 0)
		{
			num ^= TokenIndex.GetHashCode();
		}
		if (LastNameIndex != 0)
		{
			num ^= LastNameIndex.GetHashCode();
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
		if (GuildId != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(GuildId);
		}
		if (GuildName.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(GuildName);
		}
		if (GuildSimpleName.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(GuildSimpleName);
		}
		if (TokenIndex != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(TokenIndex);
		}
		if (LastNameIndex != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(LastNameIndex);
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
		if (GuildId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GuildId);
		}
		if (GuildName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GuildName);
		}
		if (GuildSimpleName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GuildSimpleName);
		}
		if (TokenIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TokenIndex);
		}
		if (LastNameIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LastNameIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildInformRoleJoinSuccessResp_1015018 other)
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
			if (other.GuildId != 0)
			{
				GuildId = other.GuildId;
			}
			if (other.GuildName.Length != 0)
			{
				GuildName = other.GuildName;
			}
			if (other.GuildSimpleName.Length != 0)
			{
				GuildSimpleName = other.GuildSimpleName;
			}
			if (other.TokenIndex != 0)
			{
				TokenIndex = other.TokenIndex;
			}
			if (other.LastNameIndex != 0)
			{
				LastNameIndex = other.LastNameIndex;
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
			case 24u:
				GuildId = input.ReadInt32();
				break;
			case 34u:
				GuildName = input.ReadString();
				break;
			case 42u:
				GuildSimpleName = input.ReadString();
				break;
			case 48u:
				TokenIndex = input.ReadInt32();
				break;
			case 56u:
				LastNameIndex = input.ReadInt32();
				break;
			}
		}
	}
}
