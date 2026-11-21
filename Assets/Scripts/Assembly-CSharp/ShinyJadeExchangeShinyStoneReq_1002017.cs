using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ShinyJadeExchangeShinyStoneReq_1002017 : IMessage<ShinyJadeExchangeShinyStoneReq_1002017>, IMessage, IEquatable<ShinyJadeExchangeShinyStoneReq_1002017>, IDeepCloneable<ShinyJadeExchangeShinyStoneReq_1002017>
{
	private static readonly MessageParser<ShinyJadeExchangeShinyStoneReq_1002017> _parser = new MessageParser<ShinyJadeExchangeShinyStoneReq_1002017>(() => new ShinyJadeExchangeShinyStoneReq_1002017());

	private UnknownFieldSet _unknownFields;

	public const int ShinyJadeFieldNumber = 1;

	private long shinyJade_;

	[DebuggerNonUserCode]
	public static MessageParser<ShinyJadeExchangeShinyStoneReq_1002017> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[20];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public ShinyJadeExchangeShinyStoneReq_1002017()
	{
	}

	[DebuggerNonUserCode]
	public ShinyJadeExchangeShinyStoneReq_1002017(ShinyJadeExchangeShinyStoneReq_1002017 other)
		: this()
	{
		shinyJade_ = other.shinyJade_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ShinyJadeExchangeShinyStoneReq_1002017 Clone()
	{
		return new ShinyJadeExchangeShinyStoneReq_1002017(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ShinyJadeExchangeShinyStoneReq_1002017);
	}

	[DebuggerNonUserCode]
	public bool Equals(ShinyJadeExchangeShinyStoneReq_1002017 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
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
		if (ShinyJade != 0L)
		{
			output.WriteRawTag(8);
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
	public void MergeFrom(ShinyJadeExchangeShinyStoneReq_1002017 other)
	{
		if (other != null)
		{
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
			if (num != 8)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				ShinyJade = input.ReadInt64();
			}
		}
	}
}
