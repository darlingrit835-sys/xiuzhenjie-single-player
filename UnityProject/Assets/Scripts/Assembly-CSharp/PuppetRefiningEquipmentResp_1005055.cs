using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class PuppetRefiningEquipmentResp_1005055 : IMessage<PuppetRefiningEquipmentResp_1005055>, IMessage, IEquatable<PuppetRefiningEquipmentResp_1005055>, IDeepCloneable<PuppetRefiningEquipmentResp_1005055>
{
	private static readonly MessageParser<PuppetRefiningEquipmentResp_1005055> _parser = new MessageParser<PuppetRefiningEquipmentResp_1005055>(() => new PuppetRefiningEquipmentResp_1005055());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CountFieldNumber = 3;

	private int count_;

	public const int RankFieldNumber = 4;

	private int rank_;

	public const int PuppetCountFieldNumber = 5;

	private int puppetCount_;

	[DebuggerNonUserCode]
	public static MessageParser<PuppetRefiningEquipmentResp_1005055> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[73];

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
	public int PuppetCount
	{
		get
		{
			return puppetCount_;
		}
		set
		{
			puppetCount_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public PuppetRefiningEquipmentResp_1005055()
	{
	}

	[DebuggerNonUserCode]
	public PuppetRefiningEquipmentResp_1005055(PuppetRefiningEquipmentResp_1005055 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		count_ = other.count_;
		rank_ = other.rank_;
		puppetCount_ = other.puppetCount_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PuppetRefiningEquipmentResp_1005055 Clone()
	{
		return new PuppetRefiningEquipmentResp_1005055(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PuppetRefiningEquipmentResp_1005055);
	}

	[DebuggerNonUserCode]
	public bool Equals(PuppetRefiningEquipmentResp_1005055 other)
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
		if (PuppetCount != other.PuppetCount)
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
		if (PuppetCount != 0)
		{
			num ^= PuppetCount.GetHashCode();
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
		if (PuppetCount != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(PuppetCount);
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
		if (PuppetCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PuppetCount);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(PuppetRefiningEquipmentResp_1005055 other)
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
			if (other.PuppetCount != 0)
			{
				PuppetCount = other.PuppetCount;
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
				PuppetCount = input.ReadInt32();
				break;
			}
		}
	}
}
