using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class CleanMapResp_1010004 : IMessage<CleanMapResp_1010004>, IMessage, IEquatable<CleanMapResp_1010004>, IDeepCloneable<CleanMapResp_1010004>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class DropGoodsInfoResp_1010005 : IMessage<DropGoodsInfoResp_1010005>, IMessage, IEquatable<DropGoodsInfoResp_1010005>, IDeepCloneable<DropGoodsInfoResp_1010005>
		{
			private static readonly MessageParser<DropGoodsInfoResp_1010005> _parser = new MessageParser<DropGoodsInfoResp_1010005>(() => new DropGoodsInfoResp_1010005());

			private UnknownFieldSet _unknownFields;

			public const int GoodsIdFieldNumber = 1;

			private string goodsId_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int FiveElementFieldNumber = 3;

			private int fiveElement_;

			public const int RankFieldNumber = 4;

			private int rank_;

			public const int LevelFieldNumber = 5;

			private int level_;

			public const int TypeFieldNumber = 6;

			private int type_;

			public const int VolumeFieldNumber = 7;

			private double volume_;

			public const int CountFieldNumber = 8;

			private int count_;

			public const int TemplateIdFieldNumber = 9;

			private string templateId_ = "";

			public const int RecyclePriceFieldNumber = 10;

			private int recyclePrice_;

			[DebuggerNonUserCode]
			public static MessageParser<DropGoodsInfoResp_1010005> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => CleanMapResp_1010004.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public string GoodsId
			{
				get
				{
					return goodsId_;
				}
				set
				{
					goodsId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
			public int Rank
			{
				get
				{
					return rank_;
				}
				set
				{
					rank_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int Level
			{
				get
				{
					return level_;
				}
				set
				{
					level_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int Type
			{
				get
				{
					return type_;
				}
				set
				{
					type_ = value;
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

			[DebuggerNonUserCode]
			public int Count
			{
				get
				{
					return count_;
				}
				set
				{
					count_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string TemplateId
			{
				get
				{
					return templateId_;
				}
				set
				{
					templateId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public int RecyclePrice
			{
				get
				{
					return recyclePrice_;
				}
				set
				{
					recyclePrice_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public DropGoodsInfoResp_1010005()
			{
			}

			[DebuggerNonUserCode]
			public DropGoodsInfoResp_1010005(DropGoodsInfoResp_1010005 other)
				: this()
			{
				goodsId_ = other.goodsId_;
				name_ = other.name_;
				fiveElement_ = other.fiveElement_;
				rank_ = other.rank_;
				level_ = other.level_;
				type_ = other.type_;
				volume_ = other.volume_;
				count_ = other.count_;
				templateId_ = other.templateId_;
				recyclePrice_ = other.recyclePrice_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public DropGoodsInfoResp_1010005 Clone()
			{
				return new DropGoodsInfoResp_1010005(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as DropGoodsInfoResp_1010005);
			}

			[DebuggerNonUserCode]
			public bool Equals(DropGoodsInfoResp_1010005 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (GoodsId != other.GoodsId)
				{
					return false;
				}
				if (Name != other.Name)
				{
					return false;
				}
				if (FiveElement != other.FiveElement)
				{
					return false;
				}
				if (Rank != other.Rank)
				{
					return false;
				}
				if (Level != other.Level)
				{
					return false;
				}
				if (Type != other.Type)
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Volume, other.Volume))
				{
					return false;
				}
				if (Count != other.Count)
				{
					return false;
				}
				if (TemplateId != other.TemplateId)
				{
					return false;
				}
				if (RecyclePrice != other.RecyclePrice)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (GoodsId.Length != 0)
				{
					num ^= GoodsId.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
				}
				if (Rank != 0)
				{
					num ^= Rank.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
				}
				if (Type != 0)
				{
					num ^= Type.GetHashCode();
				}
				if (Volume != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Volume);
				}
				if (Count != 0)
				{
					num ^= Count.GetHashCode();
				}
				if (TemplateId.Length != 0)
				{
					num ^= TemplateId.GetHashCode();
				}
				if (RecyclePrice != 0)
				{
					num ^= RecyclePrice.GetHashCode();
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
				if (GoodsId.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(GoodsId);
				}
				if (Name.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(Name);
				}
				if (FiveElement != 0)
				{
					output.WriteRawTag(24);
					output.WriteInt32(FiveElement);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Rank);
				}
				if (Level != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Level);
				}
				if (Type != 0)
				{
					output.WriteRawTag(48);
					output.WriteInt32(Type);
				}
				if (Volume != 0.0)
				{
					output.WriteRawTag(57);
					output.WriteDouble(Volume);
				}
				if (Count != 0)
				{
					output.WriteRawTag(64);
					output.WriteInt32(Count);
				}
				if (TemplateId.Length != 0)
				{
					output.WriteRawTag(74);
					output.WriteString(TemplateId);
				}
				if (RecyclePrice != 0)
				{
					output.WriteRawTag(80);
					output.WriteInt32(RecyclePrice);
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
				if (GoodsId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(GoodsId);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (Rank != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (Type != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Type);
				}
				if (Volume != 0.0)
				{
					num += 9;
				}
				if (Count != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Count);
				}
				if (TemplateId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(TemplateId);
				}
				if (RecyclePrice != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(RecyclePrice);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(DropGoodsInfoResp_1010005 other)
			{
				if (other != null)
				{
					if (other.GoodsId.Length != 0)
					{
						GoodsId = other.GoodsId;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
					}
					if (other.Rank != 0)
					{
						Rank = other.Rank;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
					}
					if (other.Type != 0)
					{
						Type = other.Type;
					}
					if (other.Volume != 0.0)
					{
						Volume = other.Volume;
					}
					if (other.Count != 0)
					{
						Count = other.Count;
					}
					if (other.TemplateId.Length != 0)
					{
						TemplateId = other.TemplateId;
					}
					if (other.RecyclePrice != 0)
					{
						RecyclePrice = other.RecyclePrice;
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
						GoodsId = input.ReadString();
						break;
					case 18u:
						Name = input.ReadString();
						break;
					case 24u:
						FiveElement = input.ReadInt32();
						break;
					case 32u:
						Rank = input.ReadInt32();
						break;
					case 40u:
						Level = input.ReadInt32();
						break;
					case 48u:
						Type = input.ReadInt32();
						break;
					case 57u:
						Volume = input.ReadDouble();
						break;
					case 64u:
						Count = input.ReadInt32();
						break;
					case 74u:
						TemplateId = input.ReadString();
						break;
					case 80u:
						RecyclePrice = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<CleanMapResp_1010004> _parser = new MessageParser<CleanMapResp_1010004>(() => new CleanMapResp_1010004());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int DropGoodsFieldNumber = 3;

	private static readonly FieldCodec<Types.DropGoodsInfoResp_1010005> _repeated_dropGoods_codec = FieldCodec.ForMessage(26u, Types.DropGoodsInfoResp_1010005.Parser);

	private readonly RepeatedField<Types.DropGoodsInfoResp_1010005> dropGoods_ = new RepeatedField<Types.DropGoodsInfoResp_1010005>();

	public const int DropStoneFieldNumber = 4;

	private int dropStone_;

	public const int CurrentShinyStoneFieldNumber = 5;

	private long currentShinyStone_;

	public const int CurrentPowerFieldNumber = 6;

	private int currentPower_;

	public const int BattleIdFieldNumber = 7;

	private string battleId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<CleanMapResp_1010004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleProtoReflection.Descriptor.MessageTypes[5];

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
	public RepeatedField<Types.DropGoodsInfoResp_1010005> DropGoods => dropGoods_;

	[DebuggerNonUserCode]
	public int DropStone
	{
		get
		{
			return dropStone_;
		}
		set
		{
			dropStone_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurrentShinyStone
	{
		get
		{
			return currentShinyStone_;
		}
		set
		{
			currentShinyStone_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentPower
	{
		get
		{
			return currentPower_;
		}
		set
		{
			currentPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string BattleId
	{
		get
		{
			return battleId_;
		}
		set
		{
			battleId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public CleanMapResp_1010004()
	{
	}

	[DebuggerNonUserCode]
	public CleanMapResp_1010004(CleanMapResp_1010004 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		dropGoods_ = other.dropGoods_.Clone();
		dropStone_ = other.dropStone_;
		currentShinyStone_ = other.currentShinyStone_;
		currentPower_ = other.currentPower_;
		battleId_ = other.battleId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CleanMapResp_1010004 Clone()
	{
		return new CleanMapResp_1010004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CleanMapResp_1010004);
	}

	[DebuggerNonUserCode]
	public bool Equals(CleanMapResp_1010004 other)
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
		if (!dropGoods_.Equals(other.dropGoods_))
		{
			return false;
		}
		if (DropStone != other.DropStone)
		{
			return false;
		}
		if (CurrentShinyStone != other.CurrentShinyStone)
		{
			return false;
		}
		if (CurrentPower != other.CurrentPower)
		{
			return false;
		}
		if (BattleId != other.BattleId)
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
		num ^= dropGoods_.GetHashCode();
		if (DropStone != 0)
		{
			num ^= DropStone.GetHashCode();
		}
		if (CurrentShinyStone != 0L)
		{
			num ^= CurrentShinyStone.GetHashCode();
		}
		if (CurrentPower != 0)
		{
			num ^= CurrentPower.GetHashCode();
		}
		if (BattleId.Length != 0)
		{
			num ^= BattleId.GetHashCode();
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
		dropGoods_.WriteTo(output, _repeated_dropGoods_codec);
		if (DropStone != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(DropStone);
		}
		if (CurrentShinyStone != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(CurrentShinyStone);
		}
		if (CurrentPower != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(CurrentPower);
		}
		if (BattleId.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(BattleId);
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
		num += dropGoods_.CalculateSize(_repeated_dropGoods_codec);
		if (DropStone != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DropStone);
		}
		if (CurrentShinyStone != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentShinyStone);
		}
		if (CurrentPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPower);
		}
		if (BattleId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(BattleId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CleanMapResp_1010004 other)
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
			dropGoods_.Add(other.dropGoods_);
			if (other.DropStone != 0)
			{
				DropStone = other.DropStone;
			}
			if (other.CurrentShinyStone != 0L)
			{
				CurrentShinyStone = other.CurrentShinyStone;
			}
			if (other.CurrentPower != 0)
			{
				CurrentPower = other.CurrentPower;
			}
			if (other.BattleId.Length != 0)
			{
				BattleId = other.BattleId;
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
				dropGoods_.AddEntriesFrom(input, _repeated_dropGoods_codec);
				break;
			case 32u:
				DropStone = input.ReadInt32();
				break;
			case 40u:
				CurrentShinyStone = input.ReadInt64();
				break;
			case 48u:
				CurrentPower = input.ReadInt32();
				break;
			case 58u:
				BattleId = input.ReadString();
				break;
			}
		}
	}
}
