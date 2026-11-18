using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class DeleteMethodMatrixReq_1008089 : IMessage<DeleteMethodMatrixReq_1008089>, IMessage, IEquatable<DeleteMethodMatrixReq_1008089>, IDeepCloneable<DeleteMethodMatrixReq_1008089>
{
	private static readonly MessageParser<DeleteMethodMatrixReq_1008089> _parser = new MessageParser<DeleteMethodMatrixReq_1008089>(() => new DeleteMethodMatrixReq_1008089());

	private UnknownFieldSet _unknownFields;

	public const int MethodMatrixIdFieldNumber = 1;

	private string methodMatrixId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<DeleteMethodMatrixReq_1008089> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[112];

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
	public DeleteMethodMatrixReq_1008089()
	{
	}

	[DebuggerNonUserCode]
	public DeleteMethodMatrixReq_1008089(DeleteMethodMatrixReq_1008089 other)
		: this()
	{
		methodMatrixId_ = other.methodMatrixId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DeleteMethodMatrixReq_1008089 Clone()
	{
		return new DeleteMethodMatrixReq_1008089(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DeleteMethodMatrixReq_1008089);
	}

	[DebuggerNonUserCode]
	public bool Equals(DeleteMethodMatrixReq_1008089 other)
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
	public void MergeFrom(DeleteMethodMatrixReq_1008089 other)
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
