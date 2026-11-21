using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildDetailResp_1015003 : IMessage<GuildDetailResp_1015003>, IMessage, IEquatable<GuildDetailResp_1015003>, IDeepCloneable<GuildDetailResp_1015003>
{
	private static readonly MessageParser<GuildDetailResp_1015003> _parser = new MessageParser<GuildDetailResp_1015003>(() => new GuildDetailResp_1015003());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int GuildIdFieldNumber = 3;

	private int guildId_;

	public const int GuildNameFieldNumber = 4;

	private string guildName_ = "";

	public const int GuildRoleCountFieldNumber = 5;

	private int guildRoleCount_;

	public const int GuildLevelFieldNumber = 6;

	private int guildLevel_;

	public const int AiDiZiNumFieldNumber = 7;

	private int aiDiZiNum_;

	public const int ZhanDouLiFieldNumber = 8;

	private long zhanDouLi_;

	public const int YingXiangLiFieldNumber = 9;

	private long yingXiangLi_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildDetailResp_1015003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[3];

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
	public int GuildRoleCount
	{
		get
		{
			return guildRoleCount_;
		}
		set
		{
			guildRoleCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int GuildLevel
	{
		get
		{
			return guildLevel_;
		}
		set
		{
			guildLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int AiDiZiNum
	{
		get
		{
			return aiDiZiNum_;
		}
		set
		{
			aiDiZiNum_ = value;
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
	public GuildDetailResp_1015003()
	{
	}

	[DebuggerNonUserCode]
	public GuildDetailResp_1015003(GuildDetailResp_1015003 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		guildId_ = other.guildId_;
		guildName_ = other.guildName_;
		guildRoleCount_ = other.guildRoleCount_;
		guildLevel_ = other.guildLevel_;
		aiDiZiNum_ = other.aiDiZiNum_;
		zhanDouLi_ = other.zhanDouLi_;
		yingXiangLi_ = other.yingXiangLi_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildDetailResp_1015003 Clone()
	{
		return new GuildDetailResp_1015003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildDetailResp_1015003);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildDetailResp_1015003 other)
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
		if (GuildRoleCount != other.GuildRoleCount)
		{
			return false;
		}
		if (GuildLevel != other.GuildLevel)
		{
			return false;
		}
		if (AiDiZiNum != other.AiDiZiNum)
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
		if (GuildId != 0)
		{
			num ^= GuildId.GetHashCode();
		}
		if (GuildName.Length != 0)
		{
			num ^= GuildName.GetHashCode();
		}
		if (GuildRoleCount != 0)
		{
			num ^= GuildRoleCount.GetHashCode();
		}
		if (GuildLevel != 0)
		{
			num ^= GuildLevel.GetHashCode();
		}
		if (AiDiZiNum != 0)
		{
			num ^= AiDiZiNum.GetHashCode();
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
		if (GuildRoleCount != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(GuildRoleCount);
		}
		if (GuildLevel != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(GuildLevel);
		}
		if (AiDiZiNum != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(AiDiZiNum);
		}
		if (ZhanDouLi != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(ZhanDouLi);
		}
		if (YingXiangLi != 0L)
		{
			output.WriteRawTag(72);
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
		if (GuildId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GuildId);
		}
		if (GuildName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GuildName);
		}
		if (GuildRoleCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GuildRoleCount);
		}
		if (GuildLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GuildLevel);
		}
		if (AiDiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AiDiZiNum);
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
	public void MergeFrom(GuildDetailResp_1015003 other)
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
			if (other.GuildRoleCount != 0)
			{
				GuildRoleCount = other.GuildRoleCount;
			}
			if (other.GuildLevel != 0)
			{
				GuildLevel = other.GuildLevel;
			}
			if (other.AiDiZiNum != 0)
			{
				AiDiZiNum = other.AiDiZiNum;
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
			case 24u:
				GuildId = input.ReadInt32();
				break;
			case 34u:
				GuildName = input.ReadString();
				break;
			case 40u:
				GuildRoleCount = input.ReadInt32();
				break;
			case 48u:
				GuildLevel = input.ReadInt32();
				break;
			case 56u:
				AiDiZiNum = input.ReadInt32();
				break;
			case 64u:
				ZhanDouLi = input.ReadInt64();
				break;
			case 72u:
				YingXiangLi = input.ReadInt64();
				break;
			}
		}
	}
}
