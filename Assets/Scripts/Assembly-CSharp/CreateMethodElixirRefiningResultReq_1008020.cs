using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class CreateMethodElixirRefiningResultReq_1008020 : IMessage<CreateMethodElixirRefiningResultReq_1008020>, IMessage, IEquatable<CreateMethodElixirRefiningResultReq_1008020>, IDeepCloneable<CreateMethodElixirRefiningResultReq_1008020>
{
	private static readonly MessageParser<CreateMethodElixirRefiningResultReq_1008020> _parser = new MessageParser<CreateMethodElixirRefiningResultReq_1008020>(() => new CreateMethodElixirRefiningResultReq_1008020());

	private UnknownFieldSet _unknownFields;

	public const int IsSuccessFieldNumber = 1;

	private bool isSuccess_;

	public const int MethodElixirIdFieldNumber = 2;

	private string methodElixirId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<CreateMethodElixirRefiningResultReq_1008020> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[21];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public bool IsSuccess
	{
		get
		{
			return isSuccess_;
		}
		set
		{
			isSuccess_ = value;
		}
	}

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
	public CreateMethodElixirRefiningResultReq_1008020()
	{
	}

	[DebuggerNonUserCode]
	public CreateMethodElixirRefiningResultReq_1008020(CreateMethodElixirRefiningResultReq_1008020 other)
		: this()
	{
		isSuccess_ = other.isSuccess_;
		methodElixirId_ = other.methodElixirId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CreateMethodElixirRefiningResultReq_1008020 Clone()
	{
		return new CreateMethodElixirRefiningResultReq_1008020(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CreateMethodElixirRefiningResultReq_1008020);
	}

	[DebuggerNonUserCode]
	public bool Equals(CreateMethodElixirRefiningResultReq_1008020 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (IsSuccess != other.IsSuccess)
		{
			return false;
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
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
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
		if (IsSuccess)
		{
			output.WriteRawTag(8);
			output.WriteBool(IsSuccess);
		}
		if (MethodElixirId.Length != 0)
		{
			output.WriteRawTag(18);
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
		if (IsSuccess)
		{
			num += 2;
		}
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
	public void MergeFrom(CreateMethodElixirRefiningResultReq_1008020 other)
	{
		if (other != null)
		{
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
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
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 8u:
				IsSuccess = input.ReadBool();
				break;
			case 18u:
				MethodElixirId = input.ReadString();
				break;
			}
		}
	}
}
