using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SearchCreateEquipmentMineralMaterialReq_1008079 : IMessage<SearchCreateEquipmentMineralMaterialReq_1008079>, IMessage, IEquatable<SearchCreateEquipmentMineralMaterialReq_1008079>, IDeepCloneable<SearchCreateEquipmentMineralMaterialReq_1008079>
{
	private static readonly MessageParser<SearchCreateEquipmentMineralMaterialReq_1008079> _parser = new MessageParser<SearchCreateEquipmentMineralMaterialReq_1008079>(() => new SearchCreateEquipmentMineralMaterialReq_1008079());

	private UnknownFieldSet _unknownFields;

	public const int MapIdFieldNumber = 1;

	private string mapId_ = "";

	public const int StageFieldNumber = 2;

	private int stage_;

	public const int FiveElementFieldNumber = 3;

	private int fiveElement_;

	public const int RenDuFieldNumber = 4;

	private int renDu_;

	public const int QiangDuFieldNumber = 5;

	private int qiangDu_;

	[DebuggerNonUserCode]
	public static MessageParser<SearchCreateEquipmentMineralMaterialReq_1008079> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[93];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
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

	[DebuggerNonUserCode]
	public int RenDu
	{
		get
		{
			return renDu_;
		}
		set
		{
			renDu_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int QiangDu
	{
		get
		{
			return qiangDu_;
		}
		set
		{
			qiangDu_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchCreateEquipmentMineralMaterialReq_1008079()
	{
	}

	[DebuggerNonUserCode]
	public SearchCreateEquipmentMineralMaterialReq_1008079(SearchCreateEquipmentMineralMaterialReq_1008079 other)
		: this()
	{
		mapId_ = other.mapId_;
		stage_ = other.stage_;
		fiveElement_ = other.fiveElement_;
		renDu_ = other.renDu_;
		qiangDu_ = other.qiangDu_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchCreateEquipmentMineralMaterialReq_1008079 Clone()
	{
		return new SearchCreateEquipmentMineralMaterialReq_1008079(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchCreateEquipmentMineralMaterialReq_1008079);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchCreateEquipmentMineralMaterialReq_1008079 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MapId != other.MapId)
		{
			return false;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		if (FiveElement != other.FiveElement)
		{
			return false;
		}
		if (RenDu != other.RenDu)
		{
			return false;
		}
		if (QiangDu != other.QiangDu)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MapId.Length != 0)
		{
			num ^= MapId.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (FiveElement != 0)
		{
			num ^= FiveElement.GetHashCode();
		}
		if (RenDu != 0)
		{
			num ^= RenDu.GetHashCode();
		}
		if (QiangDu != 0)
		{
			num ^= QiangDu.GetHashCode();
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
		if (MapId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MapId);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Stage);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(FiveElement);
		}
		if (RenDu != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(RenDu);
		}
		if (QiangDu != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(QiangDu);
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
		if (MapId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapId);
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (FiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
		}
		if (RenDu != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RenDu);
		}
		if (QiangDu != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(QiangDu);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchCreateEquipmentMineralMaterialReq_1008079 other)
	{
		if (other != null)
		{
			if (other.MapId.Length != 0)
			{
				MapId = other.MapId;
			}
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			if (other.FiveElement != 0)
			{
				FiveElement = other.FiveElement;
			}
			if (other.RenDu != 0)
			{
				RenDu = other.RenDu;
			}
			if (other.QiangDu != 0)
			{
				QiangDu = other.QiangDu;
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
				MapId = input.ReadString();
				break;
			case 16u:
				Stage = input.ReadInt32();
				break;
			case 24u:
				FiveElement = input.ReadInt32();
				break;
			case 32u:
				RenDu = input.ReadInt32();
				break;
			case 40u:
				QiangDu = input.ReadInt32();
				break;
			}
		}
	}
}
