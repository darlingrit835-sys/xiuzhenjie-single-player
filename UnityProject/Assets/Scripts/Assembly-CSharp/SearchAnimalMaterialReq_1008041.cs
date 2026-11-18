using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SearchAnimalMaterialReq_1008041 : IMessage<SearchAnimalMaterialReq_1008041>, IMessage, IEquatable<SearchAnimalMaterialReq_1008041>, IDeepCloneable<SearchAnimalMaterialReq_1008041>
{
	private static readonly MessageParser<SearchAnimalMaterialReq_1008041> _parser = new MessageParser<SearchAnimalMaterialReq_1008041>(() => new SearchAnimalMaterialReq_1008041());

	private UnknownFieldSet _unknownFields;

	public const int MapIdFieldNumber = 1;

	private string mapId_ = "";

	public const int StageFieldNumber = 2;

	private int stage_;

	public const int FiveElementFieldNumber = 3;

	private int fiveElement_;

	public const int SiQiFieldNumber = 4;

	private int siQi_;

	public const int WuWeiFieldNumber = 5;

	private int wuWei_;

	public const int BuXieFieldNumber = 6;

	private int buXie_;

	public const int GuiJingFieldNumber = 7;

	private int guiJing_;

	[DebuggerNonUserCode]
	public static MessageParser<SearchAnimalMaterialReq_1008041> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[51];

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
	public int SiQi
	{
		get
		{
			return siQi_;
		}
		set
		{
			siQi_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WuWei
	{
		get
		{
			return wuWei_;
		}
		set
		{
			wuWei_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BuXie
	{
		get
		{
			return buXie_;
		}
		set
		{
			buXie_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int GuiJing
	{
		get
		{
			return guiJing_;
		}
		set
		{
			guiJing_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchAnimalMaterialReq_1008041()
	{
	}

	[DebuggerNonUserCode]
	public SearchAnimalMaterialReq_1008041(SearchAnimalMaterialReq_1008041 other)
		: this()
	{
		mapId_ = other.mapId_;
		stage_ = other.stage_;
		fiveElement_ = other.fiveElement_;
		siQi_ = other.siQi_;
		wuWei_ = other.wuWei_;
		buXie_ = other.buXie_;
		guiJing_ = other.guiJing_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchAnimalMaterialReq_1008041 Clone()
	{
		return new SearchAnimalMaterialReq_1008041(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchAnimalMaterialReq_1008041);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchAnimalMaterialReq_1008041 other)
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
		if (SiQi != other.SiQi)
		{
			return false;
		}
		if (WuWei != other.WuWei)
		{
			return false;
		}
		if (BuXie != other.BuXie)
		{
			return false;
		}
		if (GuiJing != other.GuiJing)
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
		if (SiQi != 0)
		{
			num ^= SiQi.GetHashCode();
		}
		if (WuWei != 0)
		{
			num ^= WuWei.GetHashCode();
		}
		if (BuXie != 0)
		{
			num ^= BuXie.GetHashCode();
		}
		if (GuiJing != 0)
		{
			num ^= GuiJing.GetHashCode();
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
		if (SiQi != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(SiQi);
		}
		if (WuWei != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(WuWei);
		}
		if (BuXie != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(BuXie);
		}
		if (GuiJing != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(GuiJing);
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
		if (SiQi != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SiQi);
		}
		if (WuWei != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WuWei);
		}
		if (BuXie != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuXie);
		}
		if (GuiJing != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GuiJing);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchAnimalMaterialReq_1008041 other)
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
			if (other.SiQi != 0)
			{
				SiQi = other.SiQi;
			}
			if (other.WuWei != 0)
			{
				WuWei = other.WuWei;
			}
			if (other.BuXie != 0)
			{
				BuXie = other.BuXie;
			}
			if (other.GuiJing != 0)
			{
				GuiJing = other.GuiJing;
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
				SiQi = input.ReadInt32();
				break;
			case 40u:
				WuWei = input.ReadInt32();
				break;
			case 48u:
				BuXie = input.ReadInt32();
				break;
			case 56u:
				GuiJing = input.ReadInt32();
				break;
			}
		}
	}
}
