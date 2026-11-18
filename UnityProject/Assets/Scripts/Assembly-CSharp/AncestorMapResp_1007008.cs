using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class AncestorMapResp_1007008 : IMessage<AncestorMapResp_1007008>, IMessage, IEquatable<AncestorMapResp_1007008>, IDeepCloneable<AncestorMapResp_1007008>
{
	private static readonly MessageParser<AncestorMapResp_1007008> _parser = new MessageParser<AncestorMapResp_1007008>(() => new AncestorMapResp_1007008());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int IdFieldNumber = 3;

	private string id_ = "";

	public const int NameFieldNumber = 4;

	private string name_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<AncestorMapResp_1007008> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MapProtoReflection.Descriptor.MessageTypes[8];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Code
	{
		get
		{
			return code_;
		}
		set
		{
			code_ = value;
		}
	}

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
	public string Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public AncestorMapResp_1007008()
	{
	}

	[DebuggerNonUserCode]
	public AncestorMapResp_1007008(AncestorMapResp_1007008 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		id_ = other.id_;
		name_ = other.name_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AncestorMapResp_1007008 Clone()
	{
		return new AncestorMapResp_1007008(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AncestorMapResp_1007008);
	}

	[DebuggerNonUserCode]
	public bool Equals(AncestorMapResp_1007008 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Code != other.Code)
		{
			return false;
		}
		if (IsSuccess != other.IsSuccess)
		{
			return false;
		}
		if (Id != other.Id)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		if (Id.Length != 0)
		{
			num ^= Id.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
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
		if (Code != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Code);
		}
		if (IsSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsSuccess);
		}
		if (Id.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Id);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(Name);
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
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		if (Id.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Id);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AncestorMapResp_1007008 other)
	{
		if (other != null)
		{
			if (other.Code != 0)
			{
				Code = other.Code;
			}
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
			if (other.Id.Length != 0)
			{
				Id = other.Id;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
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
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 26u:
				Id = input.ReadString();
				break;
			case 34u:
				Name = input.ReadString();
				break;
			}
		}
	}
}
