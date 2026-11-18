using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class UpgradeGatherMatrixResp_1005013 : IMessage<UpgradeGatherMatrixResp_1005013>, IMessage, IEquatable<UpgradeGatherMatrixResp_1005013>, IDeepCloneable<UpgradeGatherMatrixResp_1005013>
{
	private static readonly MessageParser<UpgradeGatherMatrixResp_1005013> _parser = new MessageParser<UpgradeGatherMatrixResp_1005013>(() => new UpgradeGatherMatrixResp_1005013());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ShinyStoneFieldNumber = 3;

	private long shinyStone_;

	public const int RankFieldNumber = 4;

	private int rank_;

	public const int NewNameFieldNumber = 5;

	private string newName_ = "";

	public const int GatherMatrixLessTimeFieldNumber = 6;

	private long gatherMatrixLessTime_;

	public const int AchievementCircleValueFieldNumber = 7;

	private double achievementCircleValue_;

	[DebuggerNonUserCode]
	public static MessageParser<UpgradeGatherMatrixResp_1005013> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[17];

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
	public long ShinyStone
	{
		get
		{
			return shinyStone_;
		}
		set
		{
			shinyStone_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Rank
	{
		get
		{
			return rank_;
		}
		set
		{
			rank_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string NewName
	{
		get
		{
			return newName_;
		}
		set
		{
			newName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public long GatherMatrixLessTime
	{
		get
		{
			return gatherMatrixLessTime_;
		}
		set
		{
			gatherMatrixLessTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double AchievementCircleValue
	{
		get
		{
			return achievementCircleValue_;
		}
		set
		{
			achievementCircleValue_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UpgradeGatherMatrixResp_1005013()
	{
	}

	[DebuggerNonUserCode]
	public UpgradeGatherMatrixResp_1005013(UpgradeGatherMatrixResp_1005013 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		shinyStone_ = other.shinyStone_;
		rank_ = other.rank_;
		newName_ = other.newName_;
		gatherMatrixLessTime_ = other.gatherMatrixLessTime_;
		achievementCircleValue_ = other.achievementCircleValue_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UpgradeGatherMatrixResp_1005013 Clone()
	{
		return new UpgradeGatherMatrixResp_1005013(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UpgradeGatherMatrixResp_1005013);
	}

	[DebuggerNonUserCode]
	public bool Equals(UpgradeGatherMatrixResp_1005013 other)
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
		if (ShinyStone != other.ShinyStone)
		{
			return false;
		}
		if (Rank != other.Rank)
		{
			return false;
		}
		if (NewName != other.NewName)
		{
			return false;
		}
		if (GatherMatrixLessTime != other.GatherMatrixLessTime)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(AchievementCircleValue, other.AchievementCircleValue))
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
		if (ShinyStone != 0L)
		{
			num ^= ShinyStone.GetHashCode();
		}
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
		}
		if (NewName.Length != 0)
		{
			num ^= NewName.GetHashCode();
		}
		if (GatherMatrixLessTime != 0L)
		{
			num ^= GatherMatrixLessTime.GetHashCode();
		}
		if (AchievementCircleValue != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(AchievementCircleValue);
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
		if (ShinyStone != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(ShinyStone);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Rank);
		}
		if (NewName.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(NewName);
		}
		if (GatherMatrixLessTime != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(GatherMatrixLessTime);
		}
		if (AchievementCircleValue != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(AchievementCircleValue);
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
		if (ShinyStone != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShinyStone);
		}
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (NewName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(NewName);
		}
		if (GatherMatrixLessTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(GatherMatrixLessTime);
		}
		if (AchievementCircleValue != 0.0)
		{
			num += 9;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UpgradeGatherMatrixResp_1005013 other)
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
			if (other.ShinyStone != 0L)
			{
				ShinyStone = other.ShinyStone;
			}
			if (other.Rank != 0)
			{
				Rank = other.Rank;
			}
			if (other.NewName.Length != 0)
			{
				NewName = other.NewName;
			}
			if (other.GatherMatrixLessTime != 0L)
			{
				GatherMatrixLessTime = other.GatherMatrixLessTime;
			}
			if (other.AchievementCircleValue != 0.0)
			{
				AchievementCircleValue = other.AchievementCircleValue;
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
				ShinyStone = input.ReadInt64();
				break;
			case 32u:
				Rank = input.ReadInt32();
				break;
			case 42u:
				NewName = input.ReadString();
				break;
			case 48u:
				GatherMatrixLessTime = input.ReadInt64();
				break;
			case 57u:
				AchievementCircleValue = input.ReadDouble();
				break;
			}
		}
	}
}
