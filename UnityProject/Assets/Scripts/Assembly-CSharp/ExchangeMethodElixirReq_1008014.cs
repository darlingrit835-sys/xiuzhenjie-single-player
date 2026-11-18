using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ExchangeMethodElixirReq_1008014 : IMessage<ExchangeMethodElixirReq_1008014>, IMessage, IEquatable<ExchangeMethodElixirReq_1008014>, IDeepCloneable<ExchangeMethodElixirReq_1008014>
{
	private static readonly MessageParser<ExchangeMethodElixirReq_1008014> _parser = new MessageParser<ExchangeMethodElixirReq_1008014>(() => new ExchangeMethodElixirReq_1008014());

	private UnknownFieldSet _unknownFields;

	public const int MethodElixirIdFieldNumber = 1;

	private string methodElixirId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ExchangeMethodElixirReq_1008014> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[13];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodElixirId
	{
		get
		{
			return methodElixirId_;
		}
		set
		{
			methodElixirId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ExchangeMethodElixirReq_1008014()
	{
	}

	[DebuggerNonUserCode]
	public ExchangeMethodElixirReq_1008014(ExchangeMethodElixirReq_1008014 other)
		: this()
	{
		methodElixirId_ = other.methodElixirId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ExchangeMethodElixirReq_1008014 Clone()
	{
		return new ExchangeMethodElixirReq_1008014(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ExchangeMethodElixirReq_1008014);
	}

	[DebuggerNonUserCode]
	public bool Equals(ExchangeMethodElixirReq_1008014 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodElixirId != other.MethodElixirId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MethodElixirId.Length != 0)
		{
			num ^= MethodElixirId.GetHashCode();
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
		if (MethodElixirId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodElixirId);
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
		if (MethodElixirId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodElixirId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ExchangeMethodElixirReq_1008014 other)
	{
		if (other != null)
		{
			if (other.MethodElixirId.Length != 0)
			{
				MethodElixirId = other.MethodElixirId;
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				MethodElixirId = input.ReadString();
			}
		}
	}
}
