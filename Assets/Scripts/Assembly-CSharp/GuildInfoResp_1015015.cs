using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildInfoResp_1015015 : IMessage<GuildInfoResp_1015015>, IMessage, IEquatable<GuildInfoResp_1015015>, IDeepCloneable<GuildInfoResp_1015015>
{
	private static readonly MessageParser<GuildInfoResp_1015015> _parser = new MessageParser<GuildInfoResp_1015015>(() => new GuildInfoResp_1015015());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int NameFieldNumber = 3;

	private string name_ = "";

	public const int BelongHillNameFieldNumber = 4;

	private string belongHillName_ = "";

	public const int GuildStageFieldNumber = 5;

	private int guildStage_;

	public const int GuildNameFieldNumber = 6;

	private string guildName_ = "";

	public const int GuildSimpleNameFieldNumber = 7;

	private string guildSimpleName_ = "";

	public const int AiMemberCountFieldNumber = 8;

	private int aiMemberCount_;

	public const int MemberCountFieldNumber = 9;

	private int memberCount_;

	public const int ZhanDouLiFieldNumber = 10;

	private long zhanDouLi_;

	public const int YingXiangLiFieldNumber = 11;

	private long yingXiangLi_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildInfoResp_1015015> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[16];

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
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string BelongHillName
	{
		get
		{
			return belongHillName_;
		}
		set
		{
			belongHillName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int GuildStage
	{
		get
		{
			return guildStage_;
		}
		set
		{
			guildStage_ = value;
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
	public int AiMemberCount
	{
		get
		{
			return aiMemberCount_;
		}
		set
		{
			aiMemberCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MemberCount
	{
		get
		{
			return memberCount_;
		}
		set
		{
			memberCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ZhanDouLi
	{
		get
		{
			return zhanDouLi_;
		}
		set
		{
			zhanDouLi_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long YingXiangLi
	{
		get
		{
			return yingXiangLi_;
		}
		set
		{
			yingXiangLi_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildInfoResp_1015015()
	{
	}

	[DebuggerNonUserCode]
	public GuildInfoResp_1015015(GuildInfoResp_1015015 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		name_ = other.name_;
		belongHillName_ = other.belongHillName_;
		guildStage_ = other.guildStage_;
		guildName_ = other.guildName_;
		guildSimpleName_ = other.guildSimpleName_;
		aiMemberCount_ = other.aiMemberCount_;
		memberCount_ = other.memberCount_;
		zhanDouLi_ = other.zhanDouLi_;
		yingXiangLi_ = other.yingXiangLi_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildInfoResp_1015015 Clone()
	{
		return new GuildInfoResp_1015015(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildInfoResp_1015015);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildInfoResp_1015015 other)
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
		if (Name != other.Name)
		{
			return false;
		}
		if (BelongHillName != other.BelongHillName)
		{
			return false;
		}
		if (GuildStage != other.GuildStage)
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
		if (AiMemberCount != other.AiMemberCount)
		{
			return false;
		}
		if (MemberCount != other.MemberCount)
		{
			return false;
		}
		if (ZhanDouLi != other.ZhanDouLi)
		{
			return false;
		}
		if (YingXiangLi != other.YingXiangLi)
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
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (BelongHillName.Length != 0)
		{
			num ^= BelongHillName.GetHashCode();
		}
		if (GuildStage != 0)
		{
			num ^= GuildStage.GetHashCode();
		}
		if (GuildName.Length != 0)
		{
			num ^= GuildName.GetHashCode();
		}
		if (GuildSimpleName.Length != 0)
		{
			num ^= GuildSimpleName.GetHashCode();
		}
		if (AiMemberCount != 0)
		{
			num ^= AiMemberCount.GetHashCode();
		}
		if (MemberCount != 0)
		{
			num ^= MemberCount.GetHashCode();
		}
		if (ZhanDouLi != 0L)
		{
			num ^= ZhanDouLi.GetHashCode();
		}
		if (YingXiangLi != 0L)
		{
			num ^= YingXiangLi.GetHashCode();
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
		if (Name.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Name);
		}
		if (BelongHillName.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(BelongHillName);
		}
		if (GuildStage != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(GuildStage);
		}
		if (GuildName.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(GuildName);
		}
		if (GuildSimpleName.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(GuildSimpleName);
		}
		if (AiMemberCount != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(AiMemberCount);
		}
		if (MemberCount != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(MemberCount);
		}
		if (ZhanDouLi != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(ZhanDouLi);
		}
		if (YingXiangLi != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(YingXiangLi);
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
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (BelongHillName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(BelongHillName);
		}
		if (GuildStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GuildStage);
		}
		if (GuildName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GuildName);
		}
		if (GuildSimpleName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GuildSimpleName);
		}
		if (AiMemberCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AiMemberCount);
		}
		if (MemberCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MemberCount);
		}
		if (ZhanDouLi != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ZhanDouLi);
		}
		if (YingXiangLi != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(YingXiangLi);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildInfoResp_1015015 other)
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
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.BelongHillName.Length != 0)
			{
				BelongHillName = other.BelongHillName;
			}
			if (other.GuildStage != 0)
			{
				GuildStage = other.GuildStage;
			}
			if (other.GuildName.Length != 0)
			{
				GuildName = other.GuildName;
			}
			if (other.GuildSimpleName.Length != 0)
			{
				GuildSimpleName = other.GuildSimpleName;
			}
			if (other.AiMemberCount != 0)
			{
				AiMemberCount = other.AiMemberCount;
			}
			if (other.MemberCount != 0)
			{
				MemberCount = other.MemberCount;
			}
			if (other.ZhanDouLi != 0L)
			{
				ZhanDouLi = other.ZhanDouLi;
			}
			if (other.YingXiangLi != 0L)
			{
				YingXiangLi = other.YingXiangLi;
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
				Name = input.ReadString();
				break;
			case 34u:
				BelongHillName = input.ReadString();
				break;
			case 40u:
				GuildStage = input.ReadInt32();
				break;
			case 50u:
				GuildName = input.ReadString();
				break;
			case 58u:
				GuildSimpleName = input.ReadString();
				break;
			case 64u:
				AiMemberCount = input.ReadInt32();
				break;
			case 72u:
				MemberCount = input.ReadInt32();
				break;
			case 80u:
				ZhanDouLi = input.ReadInt64();
				break;
			case 88u:
				YingXiangLi = input.ReadInt64();
				break;
			}
		}
	}
}
