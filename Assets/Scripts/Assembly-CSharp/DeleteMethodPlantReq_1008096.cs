using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class DeleteMethodPlantReq_1008096 : IMessage<DeleteMethodPlantReq_1008096>, IMessage, IEquatable<DeleteMethodPlantReq_1008096>, IDeepCloneable<DeleteMethodPlantReq_1008096>
{
	private static readonly MessageParser<DeleteMethodPlantReq_1008096> _parser = new MessageParser<DeleteMethodPlantReq_1008096>(() => new DeleteMethodPlantReq_1008096());

	private UnknownFieldSet _unknownFields;

	public const int MethodPlantIdFieldNumber = 1;

	private string methodPlantId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<DeleteMethodPlantReq_1008096> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[120];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodPlantId
	{
		get
		{
			return methodPlantId_;
		}
		set
		{
			methodPlantId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public DeleteMethodPlantReq_1008096()
	{
	}

	[DebuggerNonUserCode]
	public DeleteMethodPlantReq_1008096(DeleteMethodPlantReq_1008096 other)
		: this()
	{
		methodPlantId_ = other.methodPlantId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DeleteMethodPlantReq_1008096 Clone()
	{
		return new DeleteMethodPlantReq_1008096(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DeleteMethodPlantReq_1008096);
	}

	[DebuggerNonUserCode]
	public bool Equals(DeleteMethodPlantReq_1008096 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodPlantId != other.MethodPlantId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MethodPlantId.Length != 0)
		{
			num ^= MethodPlantId.GetHashCode();
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
		if (MethodPlantId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodPlantId);
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
		if (MethodPlantId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodPlantId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DeleteMethodPlantReq_1008096 other)
	{
		if (other != null)
		{
			if (other.MethodPlantId.Length != 0)
			{
				MethodPlantId = other.MethodPlantId;
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
				MethodPlantId = input.ReadString();
			}
		}
	}
}
