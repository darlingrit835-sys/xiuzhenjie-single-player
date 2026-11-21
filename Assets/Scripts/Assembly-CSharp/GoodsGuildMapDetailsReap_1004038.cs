using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GoodsGuildMapDetailsReap_1004038 : IMessage<GoodsGuildMapDetailsReap_1004038>, IMessage, IEquatable<GoodsGuildMapDetailsReap_1004038>, IDeepCloneable<GoodsGuildMapDetailsReap_1004038>
{
	private static readonly MessageParser<GoodsGuildMapDetailsReap_1004038> _parser = new MessageParser<GoodsGuildMapDetailsReap_1004038>(() => new GoodsGuildMapDetailsReap_1004038());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int StageFieldNumber = 3;

	private int stage_;

	public const int HillCountFieldNumber = 4;

	private int hillCount_;

	public const int StoneCountFieldNumber = 5;

	private int stoneCount_;

	public const int JadeCountFieldNumber = 6;

	private int jadeCount_;

	public const int MonsterCountFieldNumber = 7;

	private int monsterCount_;

	public const int DaLuNameFieldNumber = 8;

	private string daLuName_ = "";

	public const int ZhouNameFieldNumber = 9;

	private string zhouName_ = "";

	public const int YeWaiNameFieldNumber = 10;

	private string yeWaiName_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<GoodsGuildMapDetailsReap_1004038> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[65];

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
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HillCount
	{
		get
		{
			return hillCount_;
		}
		set
		{
			hillCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int StoneCount
	{
		get
		{
			return stoneCount_;
		}
		set
		{
			stoneCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int JadeCount
	{
		get
		{
			return jadeCount_;
		}
		set
		{
			jadeCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MonsterCount
	{
		get
		{
			return monsterCount_;
		}
		set
		{
			monsterCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string DaLuName
	{
		get
		{
			return daLuName_;
		}
		set
		{
			daLuName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string ZhouName
	{
		get
		{
			return zhouName_;
		}
		set
		{
			zhouName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string YeWaiName
	{
		get
		{
			return yeWaiName_;
		}
		set
		{
			yeWaiName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsGuildMapDetailsReap_1004038()
	{
	}

	[DebuggerNonUserCode]
	public GoodsGuildMapDetailsReap_1004038(GoodsGuildMapDetailsReap_1004038 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		stage_ = other.stage_;
		hillCount_ = other.hillCount_;
		stoneCount_ = other.stoneCount_;
		jadeCount_ = other.jadeCount_;
		monsterCount_ = other.monsterCount_;
		daLuName_ = other.daLuName_;
		zhouName_ = other.zhouName_;
		yeWaiName_ = other.yeWaiName_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsGuildMapDetailsReap_1004038 Clone()
	{
		return new GoodsGuildMapDetailsReap_1004038(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsGuildMapDetailsReap_1004038);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsGuildMapDetailsReap_1004038 other)
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
		if (Stage != other.Stage)
		{
			return false;
		}
		if (HillCount != other.HillCount)
		{
			return false;
		}
		if (StoneCount != other.StoneCount)
		{
			return false;
		}
		if (JadeCount != other.JadeCount)
		{
			return false;
		}
		if (MonsterCount != other.MonsterCount)
		{
			return false;
		}
		if (DaLuName != other.DaLuName)
		{
			return false;
		}
		if (ZhouName != other.ZhouName)
		{
			return false;
		}
		if (YeWaiName != other.YeWaiName)
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
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (HillCount != 0)
		{
			num ^= HillCount.GetHashCode();
		}
		if (StoneCount != 0)
		{
			num ^= StoneCount.GetHashCode();
		}
		if (JadeCount != 0)
		{
			num ^= JadeCount.GetHashCode();
		}
		if (MonsterCount != 0)
		{
			num ^= MonsterCount.GetHashCode();
		}
		if (DaLuName.Length != 0)
		{
			num ^= DaLuName.GetHashCode();
		}
		if (ZhouName.Length != 0)
		{
			num ^= ZhouName.GetHashCode();
		}
		if (YeWaiName.Length != 0)
		{
			num ^= YeWaiName.GetHashCode();
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
		if (Stage != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Stage);
		}
		if (HillCount != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(HillCount);
		}
		if (StoneCount != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(StoneCount);
		}
		if (JadeCount != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(JadeCount);
		}
		if (MonsterCount != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(MonsterCount);
		}
		if (DaLuName.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(DaLuName);
		}
		if (ZhouName.Length != 0)
		{
			output.WriteRawTag(74);
			output.WriteString(ZhouName);
		}
		if (YeWaiName.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(YeWaiName);
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
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (HillCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HillCount);
		}
		if (StoneCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(StoneCount);
		}
		if (JadeCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(JadeCount);
		}
		if (MonsterCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MonsterCount);
		}
		if (DaLuName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(DaLuName);
		}
		if (ZhouName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ZhouName);
		}
		if (YeWaiName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(YeWaiName);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsGuildMapDetailsReap_1004038 other)
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
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			if (other.HillCount != 0)
			{
				HillCount = other.HillCount;
			}
			if (other.StoneCount != 0)
			{
				StoneCount = other.StoneCount;
			}
			if (other.JadeCount != 0)
			{
				JadeCount = other.JadeCount;
			}
			if (other.MonsterCount != 0)
			{
				MonsterCount = other.MonsterCount;
			}
			if (other.DaLuName.Length != 0)
			{
				DaLuName = other.DaLuName;
			}
			if (other.ZhouName.Length != 0)
			{
				ZhouName = other.ZhouName;
			}
			if (other.YeWaiName.Length != 0)
			{
				YeWaiName = other.YeWaiName;
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
				Stage = input.ReadInt32();
				break;
			case 32u:
				HillCount = input.ReadInt32();
				break;
			case 40u:
				StoneCount = input.ReadInt32();
				break;
			case 48u:
				JadeCount = input.ReadInt32();
				break;
			case 56u:
				MonsterCount = input.ReadInt32();
				break;
			case 66u:
				DaLuName = input.ReadString();
				break;
			case 74u:
				ZhouName = input.ReadString();
				break;
			case 82u:
				YeWaiName = input.ReadString();
				break;
			}
		}
	}
}
