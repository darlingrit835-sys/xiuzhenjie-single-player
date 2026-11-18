using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RegisterMethodSkillReq_1008028 : IMessage<RegisterMethodSkillReq_1008028>, IMessage, IEquatable<RegisterMethodSkillReq_1008028>, IDeepCloneable<RegisterMethodSkillReq_1008028>
{
	private static readonly MessageParser<RegisterMethodSkillReq_1008028> _parser = new MessageParser<RegisterMethodSkillReq_1008028>(() => new RegisterMethodSkillReq_1008028());

	private UnknownFieldSet _unknownFields;

	public const int MethodSkillIdFieldNumber = 1;

	private string methodSkillId_ = "";

	public const int RenameFieldNumber = 2;

	private string rename_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RegisterMethodSkillReq_1008028> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[32];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodSkillId
	{
		get
		{
			return methodSkillId_;
		}
		set
		{
			methodSkillId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public RegisterMethodSkillReq_1008028()
	{
	}

	[DebuggerNonUserCode]
	public RegisterMethodSkillReq_1008028(RegisterMethodSkillReq_1008028 other)
		: this()
	{
		methodSkillId_ = other.methodSkillId_;
		rename_ = other.rename_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RegisterMethodSkillReq_1008028 Clone()
	{
		return new RegisterMethodSkillReq_1008028(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RegisterMethodSkillReq_1008028);
	}

	[DebuggerNonUserCode]
	public bool Equals(RegisterMethodSkillReq_1008028 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodSkillId != other.MethodSkillId)
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
		if (MethodSkillId.Length != 0)
		{
			num ^= MethodSkillId.GetHashCode();
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
		if (MethodSkillId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodSkillId);
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
		if (MethodSkillId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodSkillId);
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
	public void MergeFrom(RegisterMethodSkillReq_1008028 other)
	{
		if (other != null)
		{
			if (other.MethodSkillId.Length != 0)
			{
				MethodSkillId = other.MethodSkillId;
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
				MethodSkillId = input.ReadString();
				break;
			case 18u:
				Rename = input.ReadString();
				break;
			}
		}
	}
}
