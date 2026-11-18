using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RegisterMethodMatrixReq_1008092 : IMessage<RegisterMethodMatrixReq_1008092>, IMessage, IEquatable<RegisterMethodMatrixReq_1008092>, IDeepCloneable<RegisterMethodMatrixReq_1008092>
{
	private static readonly MessageParser<RegisterMethodMatrixReq_1008092> _parser = new MessageParser<RegisterMethodMatrixReq_1008092>(() => new RegisterMethodMatrixReq_1008092());

	private UnknownFieldSet _unknownFields;

	public const int MethodMatrixIdFieldNumber = 1;

	private string methodMatrixId_ = "";

	public const int RenameFieldNumber = 2;

	private string rename_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RegisterMethodMatrixReq_1008092> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[110];

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

	[DebuggerNonUserCode]
	public string Rename
	{
		get
		{
			return rename_;
		}
		set
		{
			rename_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RegisterMethodMatrixReq_1008092()
	{
	}

	[DebuggerNonUserCode]
	public RegisterMethodMatrixReq_1008092(RegisterMethodMatrixReq_1008092 other)
		: this()
	{
		methodMatrixId_ = other.methodMatrixId_;
		rename_ = other.rename_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RegisterMethodMatrixReq_1008092 Clone()
	{
		return new RegisterMethodMatrixReq_1008092(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RegisterMethodMatrixReq_1008092);
	}

	[DebuggerNonUserCode]
	public bool Equals(RegisterMethodMatrixReq_1008092 other)
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
		if (Rename != other.Rename)
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
		if (Rename.Length != 0)
		{
			num ^= Rename.GetHashCode();
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
		if (Rename.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Rename);
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
		if (Rename.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Rename);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RegisterMethodMatrixReq_1008092 other)
	{
		if (other != null)
		{
			if (other.MethodMatrixId.Length != 0)
			{
				MethodMatrixId = other.MethodMatrixId;
			}
			if (other.Rename.Length != 0)
			{
				Rename = other.Rename;
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
			case 10u:
				MethodMatrixId = input.ReadString();
				break;
			case 18u:
				Rename = input.ReadString();
				break;
			}
		}
	}
}
