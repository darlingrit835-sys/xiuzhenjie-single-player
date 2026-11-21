using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RefiningMatrixResp_1005057 : IMessage<RefiningMatrixResp_1005057>, IMessage, IEquatable<RefiningMatrixResp_1005057>, IDeepCloneable<RefiningMatrixResp_1005057>
{
	private static readonly MessageParser<RefiningMatrixResp_1005057> _parser = new MessageParser<RefiningMatrixResp_1005057>(() => new RefiningMatrixResp_1005057());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CountFieldNumber = 3;

	private int count_;

	public const int RankFieldNumber = 4;

	private int rank_;

	public const int CurrentVigorFieldNumber = 5;

	private int currentVigor_;

	public const int LiveLevelFieldNumber = 6;

	private int liveLevel_;

	public const int LiveProficiencyFieldNumber = 7;

	private int liveProficiency_;

	public const int IsRefiningSuccessFieldNumber = 8;

	private bool isRefiningSuccess_;

	[DebuggerNonUserCode]
	public static MessageParser<RefiningMatrixResp_1005057> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[78];

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
	public int Count
	{
		get
		{
			return count_;
		}
		set
		{
			count_ = value;
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
	public int CurrentVigor
	{
		get
		{
			return currentVigor_;
		}
		set
		{
			currentVigor_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LiveLevel
	{
		get
		{
			return liveLevel_;
		}
		set
		{
			liveLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LiveProficiency
	{
		get
		{
			return liveProficiency_;
		}
		set
		{
			liveProficiency_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsRefiningSuccess
	{
		get
		{
			return isRefiningSuccess_;
		}
		set
		{
			isRefiningSuccess_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RefiningMatrixResp_1005057()
	{
	}

	[DebuggerNonUserCode]
	public RefiningMatrixResp_1005057(RefiningMatrixResp_1005057 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		count_ = other.count_;
		rank_ = other.rank_;
		currentVigor_ = other.currentVigor_;
		liveLevel_ = other.liveLevel_;
		liveProficiency_ = other.liveProficiency_;
		isRefiningSuccess_ = other.isRefiningSuccess_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RefiningMatrixResp_1005057 Clone()
	{
		return new RefiningMatrixResp_1005057(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RefiningMatrixResp_1005057);
	}

	[DebuggerNonUserCode]
	public bool Equals(RefiningMatrixResp_1005057 other)
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
		if (Count != other.Count)
		{
			return false;
		}
		if (Rank != other.Rank)
		{
			return false;
		}
		if (CurrentVigor != other.CurrentVigor)
		{
			return false;
		}
		if (LiveLevel != other.LiveLevel)
		{
			return false;
		}
		if (LiveProficiency != other.LiveProficiency)
		{
			return false;
		}
		if (IsRefiningSuccess != other.IsRefiningSuccess)
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
		if (Count != 0)
		{
			num ^= Count.GetHashCode();
		}
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
		}
		if (CurrentVigor != 0)
		{
			num ^= CurrentVigor.GetHashCode();
		}
		if (LiveLevel != 0)
		{
			num ^= LiveLevel.GetHashCode();
		}
		if (LiveProficiency != 0)
		{
			num ^= LiveProficiency.GetHashCode();
		}
		if (IsRefiningSuccess)
		{
			num ^= IsRefiningSuccess.GetHashCode();
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
		if (Count != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Count);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Rank);
		}
		if (CurrentVigor != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(CurrentVigor);
		}
		if (LiveLevel != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(LiveLevel);
		}
		if (LiveProficiency != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(LiveProficiency);
		}
		if (IsRefiningSuccess)
		{
			output.WriteRawTag(64);
			output.WriteBool(IsRefiningSuccess);
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
		if (Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Count);
		}
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (CurrentVigor != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentVigor);
		}
		if (LiveLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LiveLevel);
		}
		if (LiveProficiency != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LiveProficiency);
		}
		if (IsRefiningSuccess)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RefiningMatrixResp_1005057 other)
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
			if (other.Count != 0)
			{
				Count = other.Count;
			}
			if (other.Rank != 0)
			{
				Rank = other.Rank;
			}
			if (other.CurrentVigor != 0)
			{
				CurrentVigor = other.CurrentVigor;
			}
			if (other.LiveLevel != 0)
			{
				LiveLevel = other.LiveLevel;
			}
			if (other.LiveProficiency != 0)
			{
				LiveProficiency = other.LiveProficiency;
			}
			if (other.IsRefiningSuccess)
			{
				IsRefiningSuccess = other.IsRefiningSuccess;
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
				Count = input.ReadInt32();
				break;
			case 32u:
				Rank = input.ReadInt32();
				break;
			case 40u:
				CurrentVigor = input.ReadInt32();
				break;
			case 48u:
				LiveLevel = input.ReadInt32();
				break;
			case 56u:
				LiveProficiency = input.ReadInt32();
				break;
			case 64u:
				IsRefiningSuccess = input.ReadBool();
				break;
			}
		}
	}
}
