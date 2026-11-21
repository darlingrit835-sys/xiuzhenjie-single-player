using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class UsedSecretResp_1004031 : IMessage<UsedSecretResp_1004031>, IMessage, IEquatable<UsedSecretResp_1004031>, IDeepCloneable<UsedSecretResp_1004031>
{
	private static readonly MessageParser<UsedSecretResp_1004031> _parser = new MessageParser<UsedSecretResp_1004031>(() => new UsedSecretResp_1004031());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int NowCapacityFieldNumber = 3;

	private int nowCapacity_;

	public const int MapIdFieldNumber = 4;

	private string mapId_ = "";

	public const int MapTypeFieldNumber = 5;

	private int mapType_;

	public const int FiveElementFieldNumber = 6;

	private int fiveElement_;

	[DebuggerNonUserCode]
	public static MessageParser<UsedSecretResp_1004031> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[54];

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
	public int NowCapacity
	{
		get
		{
			return nowCapacity_;
		}
		set
		{
			nowCapacity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string MapId
	{
		get
		{
			return mapId_;
		}
		set
		{
			mapId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int MapType
	{
		get
		{
			return mapType_;
		}
		set
		{
			mapType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FiveElement
	{
		get
		{
			return fiveElement_;
		}
		set
		{
			fiveElement_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UsedSecretResp_1004031()
	{
	}

	[DebuggerNonUserCode]
	public UsedSecretResp_1004031(UsedSecretResp_1004031 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		nowCapacity_ = other.nowCapacity_;
		mapId_ = other.mapId_;
		mapType_ = other.mapType_;
		fiveElement_ = other.fiveElement_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UsedSecretResp_1004031 Clone()
	{
		return new UsedSecretResp_1004031(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UsedSecretResp_1004031);
	}

	[DebuggerNonUserCode]
	public bool Equals(UsedSecretResp_1004031 other)
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
		if (NowCapacity != other.NowCapacity)
		{
			return false;
		}
		if (MapId != other.MapId)
		{
			return false;
		}
		if (MapType != other.MapType)
		{
			return false;
		}
		if (FiveElement != other.FiveElement)
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
		if (NowCapacity != 0)
		{
			num ^= NowCapacity.GetHashCode();
		}
		if (MapId.Length != 0)
		{
			num ^= MapId.GetHashCode();
		}
		if (MapType != 0)
		{
			num ^= MapType.GetHashCode();
		}
		if (FiveElement != 0)
		{
			num ^= FiveElement.GetHashCode();
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
		if (NowCapacity != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(NowCapacity);
		}
		if (MapId.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(MapId);
		}
		if (MapType != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(MapType);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(FiveElement);
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
		if (NowCapacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NowCapacity);
		}
		if (MapId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapId);
		}
		if (MapType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MapType);
		}
		if (FiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UsedSecretResp_1004031 other)
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
			if (other.NowCapacity != 0)
			{
				NowCapacity = other.NowCapacity;
			}
			if (other.MapId.Length != 0)
			{
				MapId = other.MapId;
			}
			if (other.MapType != 0)
			{
				MapType = other.MapType;
			}
			if (other.FiveElement != 0)
			{
				FiveElement = other.FiveElement;
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
			case 24u:
				NowCapacity = input.ReadInt32();
				break;
			case 34u:
				MapId = input.ReadString();
				break;
			case 40u:
				MapType = input.ReadInt32();
				break;
			case 48u:
				FiveElement = input.ReadInt32();
				break;
			}
		}
	}
}
