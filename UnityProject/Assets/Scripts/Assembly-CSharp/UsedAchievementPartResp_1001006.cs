using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class UsedAchievementPartResp_1001006 : IMessage<UsedAchievementPartResp_1001006>, IMessage, IEquatable<UsedAchievementPartResp_1001006>, IDeepCloneable<UsedAchievementPartResp_1001006>
{
	private static readonly MessageParser<UsedAchievementPartResp_1001006> _parser = new MessageParser<UsedAchievementPartResp_1001006>(() => new UsedAchievementPartResp_1001006());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentLevelFieldNumber = 3;

	private int currentLevel_;

	public const int BasedConcentrationFieldNumber = 4;

	private double basedConcentration_;

	public const int PartCountFieldNumber = 5;

	private int partCount_;

	public const int DaoJingCountFieldNumber = 6;

	private int daoJingCount_;

	[DebuggerNonUserCode]
	public static MessageParser<UsedAchievementPartResp_1001006> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[6];

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
	public int CurrentLevel
	{
		get
		{
			return currentLevel_;
		}
		set
		{
			currentLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double BasedConcentration
	{
		get
		{
			return basedConcentration_;
		}
		set
		{
			basedConcentration_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PartCount
	{
		get
		{
			return partCount_;
		}
		set
		{
			partCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DaoJingCount
	{
		get
		{
			return daoJingCount_;
		}
		set
		{
			daoJingCount_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UsedAchievementPartResp_1001006()
	{
	}

	[DebuggerNonUserCode]
	public UsedAchievementPartResp_1001006(UsedAchievementPartResp_1001006 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentLevel_ = other.currentLevel_;
		basedConcentration_ = other.basedConcentration_;
		partCount_ = other.partCount_;
		daoJingCount_ = other.daoJingCount_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UsedAchievementPartResp_1001006 Clone()
	{
		return new UsedAchievementPartResp_1001006(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UsedAchievementPartResp_1001006);
	}

	[DebuggerNonUserCode]
	public bool Equals(UsedAchievementPartResp_1001006 other)
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
		if (CurrentLevel != other.CurrentLevel)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(BasedConcentration, other.BasedConcentration))
		{
			return false;
		}
		if (PartCount != other.PartCount)
		{
			return false;
		}
		if (DaoJingCount != other.DaoJingCount)
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
		if (CurrentLevel != 0)
		{
			num ^= CurrentLevel.GetHashCode();
		}
		if (BasedConcentration != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(BasedConcentration);
		}
		if (PartCount != 0)
		{
			num ^= PartCount.GetHashCode();
		}
		if (DaoJingCount != 0)
		{
			num ^= DaoJingCount.GetHashCode();
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
		if (CurrentLevel != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(CurrentLevel);
		}
		if (BasedConcentration != 0.0)
		{
			output.WriteRawTag(33);
			output.WriteDouble(BasedConcentration);
		}
		if (PartCount != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(PartCount);
		}
		if (DaoJingCount != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(DaoJingCount);
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
		if (CurrentLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentLevel);
		}
		if (BasedConcentration != 0.0)
		{
			num += 9;
		}
		if (PartCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PartCount);
		}
		if (DaoJingCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DaoJingCount);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UsedAchievementPartResp_1001006 other)
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
			if (other.CurrentLevel != 0)
			{
				CurrentLevel = other.CurrentLevel;
			}
			if (other.BasedConcentration != 0.0)
			{
				BasedConcentration = other.BasedConcentration;
			}
			if (other.PartCount != 0)
			{
				PartCount = other.PartCount;
			}
			if (other.DaoJingCount != 0)
			{
				DaoJingCount = other.DaoJingCount;
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
				CurrentLevel = input.ReadInt32();
				break;
			case 33u:
				BasedConcentration = input.ReadDouble();
				break;
			case 40u:
				PartCount = input.ReadInt32();
				break;
			case 48u:
				DaoJingCount = input.ReadInt32();
				break;
			}
		}
	}
}
