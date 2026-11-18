using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GoodsArticleSecretAreaResp_1004017 : IMessage<GoodsArticleSecretAreaResp_1004017>, IMessage, IEquatable<GoodsArticleSecretAreaResp_1004017>, IDeepCloneable<GoodsArticleSecretAreaResp_1004017>
{
	private static readonly MessageParser<GoodsArticleSecretAreaResp_1004017> _parser = new MessageParser<GoodsArticleSecretAreaResp_1004017>(() => new GoodsArticleSecretAreaResp_1004017());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int AreaTypeFieldNumber = 3;

	private int areaType_;

	public const int MapNameFieldNumber = 4;

	private string mapName_ = "";

	public const int MapFiveElementFieldNumber = 5;

	private int mapFiveElement_;

	public const int MinLevelFieldNumber = 6;

	private int minLevel_;

	public const int MaxLevelFieldNumber = 7;

	private int maxLevel_;

	public const int VolumeFieldNumber = 8;

	private double volume_;

	[DebuggerNonUserCode]
	public static MessageParser<GoodsArticleSecretAreaResp_1004017> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[29];

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
	public int AreaType
	{
		get
		{
			return areaType_;
		}
		set
		{
			areaType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string MapName
	{
		get
		{
			return mapName_;
		}
		set
		{
			mapName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int MapFiveElement
	{
		get
		{
			return mapFiveElement_;
		}
		set
		{
			mapFiveElement_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MinLevel
	{
		get
		{
			return minLevel_;
		}
		set
		{
			minLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxLevel
	{
		get
		{
			return maxLevel_;
		}
		set
		{
			maxLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Volume
	{
		get
		{
			return volume_;
		}
		set
		{
			volume_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsArticleSecretAreaResp_1004017()
	{
	}

	[DebuggerNonUserCode]
	public GoodsArticleSecretAreaResp_1004017(GoodsArticleSecretAreaResp_1004017 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		areaType_ = other.areaType_;
		mapName_ = other.mapName_;
		mapFiveElement_ = other.mapFiveElement_;
		minLevel_ = other.minLevel_;
		maxLevel_ = other.maxLevel_;
		volume_ = other.volume_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsArticleSecretAreaResp_1004017 Clone()
	{
		return new GoodsArticleSecretAreaResp_1004017(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsArticleSecretAreaResp_1004017);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsArticleSecretAreaResp_1004017 other)
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
		if (AreaType != other.AreaType)
		{
			return false;
		}
		if (MapName != other.MapName)
		{
			return false;
		}
		if (MapFiveElement != other.MapFiveElement)
		{
			return false;
		}
		if (MinLevel != other.MinLevel)
		{
			return false;
		}
		if (MaxLevel != other.MaxLevel)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Volume, other.Volume))
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
		if (AreaType != 0)
		{
			num ^= AreaType.GetHashCode();
		}
		if (MapName.Length != 0)
		{
			num ^= MapName.GetHashCode();
		}
		if (MapFiveElement != 0)
		{
			num ^= MapFiveElement.GetHashCode();
		}
		if (MinLevel != 0)
		{
			num ^= MinLevel.GetHashCode();
		}
		if (MaxLevel != 0)
		{
			num ^= MaxLevel.GetHashCode();
		}
		if (Volume != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Volume);
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
		if (AreaType != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(AreaType);
		}
		if (MapName.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(MapName);
		}
		if (MapFiveElement != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(MapFiveElement);
		}
		if (MinLevel != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(MinLevel);
		}
		if (MaxLevel != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(MaxLevel);
		}
		if (Volume != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(Volume);
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
		if (AreaType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AreaType);
		}
		if (MapName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapName);
		}
		if (MapFiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MapFiveElement);
		}
		if (MinLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MinLevel);
		}
		if (MaxLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxLevel);
		}
		if (Volume != 0.0)
		{
			num += 9;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsArticleSecretAreaResp_1004017 other)
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
			if (other.AreaType != 0)
			{
				AreaType = other.AreaType;
			}
			if (other.MapName.Length != 0)
			{
				MapName = other.MapName;
			}
			if (other.MapFiveElement != 0)
			{
				MapFiveElement = other.MapFiveElement;
			}
			if (other.MinLevel != 0)
			{
				MinLevel = other.MinLevel;
			}
			if (other.MaxLevel != 0)
			{
				MaxLevel = other.MaxLevel;
			}
			if (other.Volume != 0.0)
			{
				Volume = other.Volume;
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
				AreaType = input.ReadInt32();
				break;
			case 34u:
				MapName = input.ReadString();
				break;
			case 40u:
				MapFiveElement = input.ReadInt32();
				break;
			case 48u:
				MinLevel = input.ReadInt32();
				break;
			case 56u:
				MaxLevel = input.ReadInt32();
				break;
			case 65u:
				Volume = input.ReadDouble();
				break;
			}
		}
	}
}
