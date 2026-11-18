using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RushTowerTierNumRankedAwardResp_1008037 : IMessage<RushTowerTierNumRankedAwardResp_1008037>, IMessage, IEquatable<RushTowerTierNumRankedAwardResp_1008037>, IDeepCloneable<RushTowerTierNumRankedAwardResp_1008037>
{
	private static readonly MessageParser<RushTowerTierNumRankedAwardResp_1008037> _parser = new MessageParser<RushTowerTierNumRankedAwardResp_1008037>(() => new RushTowerTierNumRankedAwardResp_1008037());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ShinyJadeFieldNumber = 3;

	private long shinyJade_;

	[DebuggerNonUserCode]
	public static MessageParser<RushTowerTierNumRankedAwardResp_1008037> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[46];

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
	public long ShinyJade
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
	public RushTowerTierNumRankedAwardResp_1008037()
	{
	}

	[DebuggerNonUserCode]
	public RushTowerTierNumRankedAwardResp_1008037(RushTowerTierNumRankedAwardResp_1008037 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		shinyJade_ = other.shinyJade_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RushTowerTierNumRankedAwardResp_1008037 Clone()
	{
		return new RushTowerTierNumRankedAwardResp_1008037(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RushTowerTierNumRankedAwardResp_1008037);
	}

	[DebuggerNonUserCode]
	public bool Equals(RushTowerTierNumRankedAwardResp_1008037 other)
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
		if (ShinyJade != 0L)
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
		if (ShinyJade != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(ShinyJade);
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
		if (ShinyJade != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShinyJade);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RushTowerTierNumRankedAwardResp_1008037 other)
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
			if (other.ShinyJade != 0L)
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
				ShinyJade = input.ReadInt64();
				break;
			}
		}
	}
}
