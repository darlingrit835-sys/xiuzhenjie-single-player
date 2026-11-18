using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RegisterMethodEquipmentReq_1008072 : IMessage<RegisterMethodEquipmentReq_1008072>, IMessage, IEquatable<RegisterMethodEquipmentReq_1008072>, IDeepCloneable<RegisterMethodEquipmentReq_1008072>
{
	private static readonly MessageParser<RegisterMethodEquipmentReq_1008072> _parser = new MessageParser<RegisterMethodEquipmentReq_1008072>(() => new RegisterMethodEquipmentReq_1008072());

	private UnknownFieldSet _unknownFields;

	public const int MethodEquipmentIdFieldNumber = 1;

	private string methodEquipmentId_ = "";

	public const int RenameFieldNumber = 2;

	private string rename_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RegisterMethodEquipmentReq_1008072> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[85];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodEquipmentId
	{
		get
		{
			return methodEquipmentId_;
		}
		set
		{
			methodEquipmentId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public RegisterMethodEquipmentReq_1008072()
	{
	}

	[DebuggerNonUserCode]
	public RegisterMethodEquipmentReq_1008072(RegisterMethodEquipmentReq_1008072 other)
		: this()
	{
		methodEquipmentId_ = other.methodEquipmentId_;
		rename_ = other.rename_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RegisterMethodEquipmentReq_1008072 Clone()
	{
		return new RegisterMethodEquipmentReq_1008072(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RegisterMethodEquipmentReq_1008072);
	}

	[DebuggerNonUserCode]
	public bool Equals(RegisterMethodEquipmentReq_1008072 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodEquipmentId != other.MethodEquipmentId)
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
		if (MethodEquipmentId.Length != 0)
		{
			num ^= MethodEquipmentId.GetHashCode();
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
		if (MethodEquipmentId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodEquipmentId);
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
		if (MethodEquipmentId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodEquipmentId);
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
	public void MergeFrom(RegisterMethodEquipmentReq_1008072 other)
	{
		if (other != null)
		{
			if (other.MethodEquipmentId.Length != 0)
			{
				MethodEquipmentId = other.MethodEquipmentId;
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
				MethodEquipmentId = input.ReadString();
				break;
			case 18u:
				Rename = input.ReadString();
				break;
			}
		}
	}
}
