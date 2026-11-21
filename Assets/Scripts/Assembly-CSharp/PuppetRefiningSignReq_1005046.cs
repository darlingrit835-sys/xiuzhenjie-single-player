using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class PuppetRefiningSignReq_1005046 : IMessage<PuppetRefiningSignReq_1005046>, IMessage, IEquatable<PuppetRefiningSignReq_1005046>, IDeepCloneable<PuppetRefiningSignReq_1005046>
{
	private static readonly MessageParser<PuppetRefiningSignReq_1005046> _parser = new MessageParser<PuppetRefiningSignReq_1005046>(() => new PuppetRefiningSignReq_1005046());

	private UnknownFieldSet _unknownFields;

	public const int MethodSignIdFieldNumber = 1;

	private string methodSignId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<PuppetRefiningSignReq_1005046> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[60];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodSignId
	{
		get
		{
			return methodSignId_;
		}
		set
		{
			methodSignId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public PuppetRefiningSignReq_1005046()
	{
	}

	[DebuggerNonUserCode]
	public PuppetRefiningSignReq_1005046(PuppetRefiningSignReq_1005046 other)
		: this()
	{
		methodSignId_ = other.methodSignId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PuppetRefiningSignReq_1005046 Clone()
	{
		return new PuppetRefiningSignReq_1005046(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PuppetRefiningSignReq_1005046);
	}

	[DebuggerNonUserCode]
	public bool Equals(PuppetRefiningSignReq_1005046 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodSignId != other.MethodSignId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MethodSignId.Length != 0)
		{
			num ^= MethodSignId.GetHashCode();
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
		if (MethodSignId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodSignId);
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
		if (MethodSignId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodSignId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(PuppetRefiningSignReq_1005046 other)
	{
		if (other != null)
		{
			if (other.MethodSignId.Length != 0)
			{
				MethodSignId = other.MethodSignId;
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
				MethodSignId = input.ReadString();
			}
		}
	}
}
