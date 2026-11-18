using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GradeRingResp_1004025 : IMessage<GradeRingResp_1004025>, IMessage, IEquatable<GradeRingResp_1004025>, IDeepCloneable<GradeRingResp_1004025>
{
	private static readonly MessageParser<GradeRingResp_1004025> _parser = new MessageParser<GradeRingResp_1004025>(() => new GradeRingResp_1004025());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int MaxCapacityFieldNumber = 3;

	private int maxCapacity_;

	public const int ShinyJadeFieldNumber = 4;

	private int shinyJade_;

	[DebuggerNonUserCode]
	public static MessageParser<GradeRingResp_1004025> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[44];

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
	public int MaxCapacity
	{
		get
		{
			return maxCapacity_;
		}
		set
		{
			maxCapacity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ShinyJade
	{
		get
		{
			return shinyJade_;
		}
		set
		{
			shinyJade_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GradeRingResp_1004025()
	{
	}

	[DebuggerNonUserCode]
	public GradeRingResp_1004025(GradeRingResp_1004025 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		maxCapacity_ = other.maxCapacity_;
		shinyJade_ = other.shinyJade_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GradeRingResp_1004025 Clone()
	{
		return new GradeRingResp_1004025(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GradeRingResp_1004025);
	}

	[DebuggerNonUserCode]
	public bool Equals(GradeRingResp_1004025 other)
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
		if (MaxCapacity != other.MaxCapacity)
		{
			return false;
		}
		if (ShinyJade != other.ShinyJade)
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
		if (MaxCapacity != 0)
		{
			num ^= MaxCapacity.GetHashCode();
		}
		if (ShinyJade != 0)
		{
			num ^= ShinyJade.GetHashCode();
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
		if (MaxCapacity != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(MaxCapacity);
		}
		if (ShinyJade != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(ShinyJade);
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
		if (MaxCapacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxCapacity);
		}
		if (ShinyJade != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ShinyJade);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GradeRingResp_1004025 other)
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
			if (other.MaxCapacity != 0)
			{
				MaxCapacity = other.MaxCapacity;
			}
			if (other.ShinyJade != 0)
			{
				ShinyJade = other.ShinyJade;
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
				MaxCapacity = input.ReadInt32();
				break;
			case 32u:
				ShinyJade = input.ReadInt32();
				break;
			}
		}
	}
}
