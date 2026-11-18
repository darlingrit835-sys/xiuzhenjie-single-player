using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RegisterMethodAchievementReq_1008036 : IMessage<RegisterMethodAchievementReq_1008036>, IMessage, IEquatable<RegisterMethodAchievementReq_1008036>, IDeepCloneable<RegisterMethodAchievementReq_1008036>
{
	private static readonly MessageParser<RegisterMethodAchievementReq_1008036> _parser = new MessageParser<RegisterMethodAchievementReq_1008036>(() => new RegisterMethodAchievementReq_1008036());

	private UnknownFieldSet _unknownFields;

	public const int MethodAchievementIdFieldNumber = 1;

	private string methodAchievementId_ = "";

	public const int RenameFieldNumber = 2;

	private string rename_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RegisterMethodAchievementReq_1008036> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[43];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodAchievementId
	{
		get
		{
			return methodAchievementId_;
		}
		set
		{
			methodAchievementId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public RegisterMethodAchievementReq_1008036()
	{
	}

	[DebuggerNonUserCode]
	public RegisterMethodAchievementReq_1008036(RegisterMethodAchievementReq_1008036 other)
		: this()
	{
		methodAchievementId_ = other.methodAchievementId_;
		rename_ = other.rename_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RegisterMethodAchievementReq_1008036 Clone()
	{
		return new RegisterMethodAchievementReq_1008036(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RegisterMethodAchievementReq_1008036);
	}

	[DebuggerNonUserCode]
	public bool Equals(RegisterMethodAchievementReq_1008036 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodAchievementId != other.MethodAchievementId)
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
		if (MethodAchievementId.Length != 0)
		{
			num ^= MethodAchievementId.GetHashCode();
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
		if (MethodAchievementId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodAchievementId);
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
		if (MethodAchievementId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodAchievementId);
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
	public void MergeFrom(RegisterMethodAchievementReq_1008036 other)
	{
		if (other != null)
		{
			if (other.MethodAchievementId.Length != 0)
			{
				MethodAchievementId = other.MethodAchievementId;
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
				MethodAchievementId = input.ReadString();
				break;
			case 18u:
				Rename = input.ReadString();
				break;
			}
		}
	}
}
