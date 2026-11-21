using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RegisterMethodElixirReq_1008017 : IMessage<RegisterMethodElixirReq_1008017>, IMessage, IEquatable<RegisterMethodElixirReq_1008017>, IDeepCloneable<RegisterMethodElixirReq_1008017>
{
	private static readonly MessageParser<RegisterMethodElixirReq_1008017> _parser = new MessageParser<RegisterMethodElixirReq_1008017>(() => new RegisterMethodElixirReq_1008017());

	private UnknownFieldSet _unknownFields;

	public const int MethodElixirIdFieldNumber = 1;

	private string methodElixirId_ = "";

	public const int RenameFieldNumber = 2;

	private string rename_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RegisterMethodElixirReq_1008017> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[17];

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
	public RegisterMethodElixirReq_1008017()
	{
	}

	[DebuggerNonUserCode]
	public RegisterMethodElixirReq_1008017(RegisterMethodElixirReq_1008017 other)
		: this()
	{
		methodElixirId_ = other.methodElixirId_;
		rename_ = other.rename_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RegisterMethodElixirReq_1008017 Clone()
	{
		return new RegisterMethodElixirReq_1008017(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RegisterMethodElixirReq_1008017);
	}

	[DebuggerNonUserCode]
	public bool Equals(RegisterMethodElixirReq_1008017 other)
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
		if (MethodElixirId.Length != 0)
		{
			num ^= MethodElixirId.GetHashCode();
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
		if (MethodElixirId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodElixirId);
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
		if (MethodElixirId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodElixirId);
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
	public void MergeFrom(RegisterMethodElixirReq_1008017 other)
	{
		if (other != null)
		{
			if (other.MethodElixirId.Length != 0)
			{
				MethodElixirId = other.MethodElixirId;
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
				MethodElixirId = input.ReadString();
				break;
			case 18u:
				Rename = input.ReadString();
				break;
			}
		}
	}
}
