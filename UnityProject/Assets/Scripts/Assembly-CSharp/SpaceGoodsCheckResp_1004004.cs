using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SpaceGoodsCheckResp_1004004 : IMessage<SpaceGoodsCheckResp_1004004>, IMessage, IEquatable<SpaceGoodsCheckResp_1004004>, IDeepCloneable<SpaceGoodsCheckResp_1004004>
{
	private static readonly MessageParser<SpaceGoodsCheckResp_1004004> _parser = new MessageParser<SpaceGoodsCheckResp_1004004>(() => new SpaceGoodsCheckResp_1004004());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int Md5FieldNumber = 3;

	private string md5_ = "";

	public const int LocationFieldNumber = 4;

	private int location_;

	[DebuggerNonUserCode]
	public static MessageParser<SpaceGoodsCheckResp_1004004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[5];

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
	public string Md5
	{
		get
		{
			return md5_;
		}
		set
		{
			md5_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Location
	{
		get
		{
			return location_;
		}
		set
		{
			location_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SpaceGoodsCheckResp_1004004()
	{
	}

	[DebuggerNonUserCode]
	public SpaceGoodsCheckResp_1004004(SpaceGoodsCheckResp_1004004 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		md5_ = other.md5_;
		location_ = other.location_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SpaceGoodsCheckResp_1004004 Clone()
	{
		return new SpaceGoodsCheckResp_1004004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SpaceGoodsCheckResp_1004004);
	}

	[DebuggerNonUserCode]
	public bool Equals(SpaceGoodsCheckResp_1004004 other)
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
		if (Md5 != other.Md5)
		{
			return false;
		}
		if (Location != other.Location)
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
		if (Md5.Length != 0)
		{
			num ^= Md5.GetHashCode();
		}
		if (Location != 0)
		{
			num ^= Location.GetHashCode();
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
		if (Md5.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Md5);
		}
		if (Location != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Location);
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
		if (Md5.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Md5);
		}
		if (Location != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Location);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SpaceGoodsCheckResp_1004004 other)
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
			if (other.Md5.Length != 0)
			{
				Md5 = other.Md5;
			}
			if (other.Location != 0)
			{
				Location = other.Location;
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
				Md5 = input.ReadString();
				break;
			case 32u:
				Location = input.ReadInt32();
				break;
			}
		}
	}
}
