using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ExamineElixirStepReq_1008009 : IMessage<ExamineElixirStepReq_1008009>, IMessage, IEquatable<ExamineElixirStepReq_1008009>, IDeepCloneable<ExamineElixirStepReq_1008009>
{
	private static readonly MessageParser<ExamineElixirStepReq_1008009> _parser = new MessageParser<ExamineElixirStepReq_1008009>(() => new ExamineElixirStepReq_1008009());

	private UnknownFieldSet _unknownFields;

	public const int MethodElixirIdFieldNumber = 1;

	private string methodElixirId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ExamineElixirStepReq_1008009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[7];

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
	public ExamineElixirStepReq_1008009()
	{
	}

	[DebuggerNonUserCode]
	public ExamineElixirStepReq_1008009(ExamineElixirStepReq_1008009 other)
		: this()
	{
		methodElixirId_ = other.methodElixirId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ExamineElixirStepReq_1008009 Clone()
	{
		return new ExamineElixirStepReq_1008009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ExamineElixirStepReq_1008009);
	}

	[DebuggerNonUserCode]
	public bool Equals(ExamineElixirStepReq_1008009 other)
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
	public void MergeFrom(ExamineElixirStepReq_1008009 other)
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
