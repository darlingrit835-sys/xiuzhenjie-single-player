using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class PuppetRefiningElixirReq_1005045 : IMessage<PuppetRefiningElixirReq_1005045>, IMessage, IEquatable<PuppetRefiningElixirReq_1005045>, IDeepCloneable<PuppetRefiningElixirReq_1005045>
{
	private static readonly MessageParser<PuppetRefiningElixirReq_1005045> _parser = new MessageParser<PuppetRefiningElixirReq_1005045>(() => new PuppetRefiningElixirReq_1005045());

	private UnknownFieldSet _unknownFields;

	public const int MethodElixirIdFieldNumber = 1;

	private string methodElixirId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<PuppetRefiningElixirReq_1005045> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[58];

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
	public PuppetRefiningElixirReq_1005045()
	{
	}

	[DebuggerNonUserCode]
	public PuppetRefiningElixirReq_1005045(PuppetRefiningElixirReq_1005045 other)
		: this()
	{
		methodElixirId_ = other.methodElixirId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PuppetRefiningElixirReq_1005045 Clone()
	{
		return new PuppetRefiningElixirReq_1005045(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PuppetRefiningElixirReq_1005045);
	}

	[DebuggerNonUserCode]
	public bool Equals(PuppetRefiningElixirReq_1005045 other)
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
	public void MergeFrom(PuppetRefiningElixirReq_1005045 other)
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
