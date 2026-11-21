using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class PuppetRefiningMatrixReq_1005061 : IMessage<PuppetRefiningMatrixReq_1005061>, IMessage, IEquatable<PuppetRefiningMatrixReq_1005061>, IDeepCloneable<PuppetRefiningMatrixReq_1005061>
{
	private static readonly MessageParser<PuppetRefiningMatrixReq_1005061> _parser = new MessageParser<PuppetRefiningMatrixReq_1005061>(() => new PuppetRefiningMatrixReq_1005061());

	private UnknownFieldSet _unknownFields;

	public const int MethodMatrixIdFieldNumber = 1;

	private string methodMatrixId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<PuppetRefiningMatrixReq_1005061> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[83];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodMatrixId
	{
		get
		{
			return methodMatrixId_;
		}
		set
		{
			methodMatrixId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public PuppetRefiningMatrixReq_1005061()
	{
	}

	[DebuggerNonUserCode]
	public PuppetRefiningMatrixReq_1005061(PuppetRefiningMatrixReq_1005061 other)
		: this()
	{
		methodMatrixId_ = other.methodMatrixId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PuppetRefiningMatrixReq_1005061 Clone()
	{
		return new PuppetRefiningMatrixReq_1005061(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PuppetRefiningMatrixReq_1005061);
	}

	[DebuggerNonUserCode]
	public bool Equals(PuppetRefiningMatrixReq_1005061 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodMatrixId != other.MethodMatrixId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MethodMatrixId.Length != 0)
		{
			num ^= MethodMatrixId.GetHashCode();
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
		if (MethodMatrixId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodMatrixId);
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
		if (MethodMatrixId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodMatrixId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(PuppetRefiningMatrixReq_1005061 other)
	{
		if (other != null)
		{
			if (other.MethodMatrixId.Length != 0)
			{
				MethodMatrixId = other.MethodMatrixId;
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
				MethodMatrixId = input.ReadString();
			}
		}
	}
}
