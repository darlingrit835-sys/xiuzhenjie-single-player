using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RefiningSignReq_1005036 : IMessage<RefiningSignReq_1005036>, IMessage, IEquatable<RefiningSignReq_1005036>, IDeepCloneable<RefiningSignReq_1005036>
{
	private static readonly MessageParser<RefiningSignReq_1005036> _parser = new MessageParser<RefiningSignReq_1005036>(() => new RefiningSignReq_1005036());

	private UnknownFieldSet _unknownFields;

	public const int MethodSignIdFieldNumber = 1;

	private string methodSignId_ = "";

	public const int IsRefiningSuccessFieldNumber = 2;

	private bool isRefiningSuccess_;

	[DebuggerNonUserCode]
	public static MessageParser<RefiningSignReq_1005036> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[45];

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

	[DebuggerNonUserCode]
	public bool IsRefiningSuccess
	{
		get
		{
			return isRefiningSuccess_;
		}
		set
		{
			isRefiningSuccess_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RefiningSignReq_1005036()
	{
	}

	[DebuggerNonUserCode]
	public RefiningSignReq_1005036(RefiningSignReq_1005036 other)
		: this()
	{
		methodSignId_ = other.methodSignId_;
		isRefiningSuccess_ = other.isRefiningSuccess_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RefiningSignReq_1005036 Clone()
	{
		return new RefiningSignReq_1005036(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RefiningSignReq_1005036);
	}

	[DebuggerNonUserCode]
	public bool Equals(RefiningSignReq_1005036 other)
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
		if (IsRefiningSuccess != other.IsRefiningSuccess)
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
		if (IsRefiningSuccess)
		{
			num ^= IsRefiningSuccess.GetHashCode();
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
		if (IsRefiningSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsRefiningSuccess);
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
		if (IsRefiningSuccess)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RefiningSignReq_1005036 other)
	{
		if (other != null)
		{
			if (other.MethodSignId.Length != 0)
			{
				MethodSignId = other.MethodSignId;
			}
			if (other.IsRefiningSuccess)
			{
				IsRefiningSuccess = other.IsRefiningSuccess;
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
				MethodSignId = input.ReadString();
				break;
			case 16u:
				IsRefiningSuccess = input.ReadBool();
				break;
			}
		}
	}
}
