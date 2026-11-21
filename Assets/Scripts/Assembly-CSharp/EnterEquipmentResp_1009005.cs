using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterEquipmentResp_1009005 : IMessage<EnterEquipmentResp_1009005>, IMessage, IEquatable<EnterEquipmentResp_1009005>, IDeepCloneable<EnterEquipmentResp_1009005>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class EquipmentInfoResp_1009006 : IMessage<EquipmentInfoResp_1009006>, IMessage, IEquatable<EquipmentInfoResp_1009006>, IDeepCloneable<EquipmentInfoResp_1009006>
		{
			private static readonly MessageParser<EquipmentInfoResp_1009006> _parser = new MessageParser<EquipmentInfoResp_1009006>(() => new EquipmentInfoResp_1009006());

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

			public const int IconFieldNumber = 7;

			private int icon_;

			public const int BlankIdFieldNumber = 8;

			private string blankId_ = "";

			public string BlankId
			{
				get => blankId_;
				set => blankId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}

			public const int IsLockFieldNumber = 10;

			public bool IsLock { get; set; }

			public const int TemplateIdFieldNumber = 9;

			private string templateId_ = "";

			public string TemplateId
			{
				get => templateId_;
				set => templateId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}


			[DebuggerNonUserCode]
			public static MessageParser<EquipmentInfoResp_1009006> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterEquipmentResp_1009005.Descriptor.NestedTypes[0];

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
			public int Icon
			{
				get
				{
					return icon_;
				}
				set
				{
					icon_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public EquipmentInfoResp_1009006()
			{
			}

			[DebuggerNonUserCode]
			public EquipmentInfoResp_1009006(EquipmentInfoResp_1009006 other)
				: this()
			{
				goodsId_ = other.goodsId_;
				name_ = other.name_;
				fiveElement_ = other.fiveElement_;
				rank_ = other.rank_;
				level_ = other.level_;
				type_ = other.type_;
				icon_ = other.icon_;
				blankId_ = other.blankId_;
				templateId_ = other.templateId_;
				IsLock = other.IsLock;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public EquipmentInfoResp_1009006 Clone()
			{
				return new EquipmentInfoResp_1009006(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as EquipmentInfoResp_1009006);
			}

			[DebuggerNonUserCode]
			public bool Equals(EquipmentInfoResp_1009006 other)
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
				if (Icon != other.Icon || BlankId != other.BlankId || TemplateId != other.TemplateId)
				{
					return false;
				}
				if (IsLock != other.IsLock) return false;
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
				if (Icon != 0)
				{
					num ^= Icon.GetHashCode();
				}
				if (BlankId.Length != 0) num ^= BlankId.GetHashCode();
				if (IsLock) num ^= IsLock.GetHashCode();
				if (TemplateId.Length != 0) num ^= TemplateId.GetHashCode();
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
				if (Icon != 0)
				{
					output.WriteRawTag(56);
					output.WriteInt32(Icon);
				}
				if (BlankId.Length != 0)
				{
					output.WriteRawTag(66);
					output.WriteString(BlankId);
				}
				if (TemplateId.Length != 0)
				{
					output.WriteRawTag(74);
					output.WriteString(TemplateId);
				}
				if (IsLock)
				{
					output.WriteTag(IsLockFieldNumber, WireFormat.WireType.Varint);
					output.WriteBool(true);
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
				if (Icon != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Icon);
				}
				if (BlankId.Length != 0) num += 1 + CodedOutputStream.ComputeStringSize(BlankId);
				if (IsLock) num += CodedOutputStream.ComputeTagSize(IsLockFieldNumber) + 1;
				if (TemplateId.Length != 0) num += 1 + CodedOutputStream.ComputeStringSize(TemplateId);
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(EquipmentInfoResp_1009006 other)
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
					if (other.Icon != 0)
					{
						Icon = other.Icon;
					}
					if (other.BlankId.Length != 0) BlankId = other.BlankId;
					if (other.IsLock) IsLock = true;
					if (other.TemplateId.Length != 0) TemplateId = other.TemplateId;
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
					case 66u:
						BlankId = input.ReadString();
						break;
					case 80u:
						IsLock = input.ReadBool();
						break;
					case 74u:
						TemplateId = input.ReadString();
						break;
					case 56u:
						Icon = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterEquipmentResp_1009005> _parser = new MessageParser<EnterEquipmentResp_1009005>(() => new EnterEquipmentResp_1009005());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int DressedEquipmentsFieldNumber = 3;

	private static readonly FieldCodec<Types.EquipmentInfoResp_1009006> _repeated_dressedEquipments_codec = FieldCodec.ForMessage(26u, Types.EquipmentInfoResp_1009006.Parser);

	private readonly RepeatedField<Types.EquipmentInfoResp_1009006> dressedEquipments_ = new RepeatedField<Types.EquipmentInfoResp_1009006>();

	public const int CurrentPhysiqueDefenseFieldNumber = 4;

	private long currentPhysiqueDefense_;

	public const int CurrentPureBloodDefenseFieldNumber = 5;

	private int currentPureBloodDefense_;

	public const int FullLoadPerceptionFieldNumber = 6;

	private int fullLoadPerception_;

	public const int CurrentMoveSpeedFieldNumber = 7;

	private double currentMoveSpeed_;

	public const int SuperAirRecoverValueFieldNumber = 8;

	private double superAirRecoverValue_;

	public const int DefenseRecoverValueFieldNumber = 9;

	private double defenseRecoverValue_;

	public const int SuperAirRecoverValueOnBattleFieldNumber = 10;

	private double superAirRecoverValueOnBattle_;

	public const int DefenseRecoverValueOnBattleFieldNumber = 11;

	private double defenseRecoverValueOnBattle_;

	public const int SuperAirPowerFieldNumber = 12;

	private double superAirPower_;

	public const int CanUsePhysiqueFieldNumber = 13;

	private int canUsePhysique_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterEquipmentResp_1009005> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleSettingProtoReflection.Descriptor.MessageTypes[8];

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
	public RepeatedField<Types.EquipmentInfoResp_1009006> DressedEquipments => dressedEquipments_;

	[DebuggerNonUserCode]
	public long CurrentPhysiqueDefense
	{
		get
		{
			return currentPhysiqueDefense_;
		}
		set
		{
			currentPhysiqueDefense_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentPureBloodDefense
	{
		get
		{
			return currentPureBloodDefense_;
		}
		set
		{
			currentPureBloodDefense_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FullLoadPerception
	{
		get
		{
			return fullLoadPerception_;
		}
		set
		{
			fullLoadPerception_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double CurrentMoveSpeed
	{
		get
		{
			return currentMoveSpeed_;
		}
		set
		{
			currentMoveSpeed_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double SuperAirRecoverValue
	{
		get
		{
			return superAirRecoverValue_;
		}
		set
		{
			superAirRecoverValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double DefenseRecoverValue
	{
		get
		{
			return defenseRecoverValue_;
		}
		set
		{
			defenseRecoverValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double SuperAirRecoverValueOnBattle
	{
		get
		{
			return superAirRecoverValueOnBattle_;
		}
		set
		{
			superAirRecoverValueOnBattle_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double DefenseRecoverValueOnBattle
	{
		get
		{
			return defenseRecoverValueOnBattle_;
		}
		set
		{
			defenseRecoverValueOnBattle_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double SuperAirPower
	{
		get
		{
			return superAirPower_;
		}
		set
		{
			superAirPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CanUsePhysique
	{
		get
		{
			return canUsePhysique_;
		}
		set
		{
			canUsePhysique_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterEquipmentResp_1009005()
	{
	}

	[DebuggerNonUserCode]
	public EnterEquipmentResp_1009005(EnterEquipmentResp_1009005 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		dressedEquipments_ = other.dressedEquipments_.Clone();
		currentPhysiqueDefense_ = other.currentPhysiqueDefense_;
		currentPureBloodDefense_ = other.currentPureBloodDefense_;
		fullLoadPerception_ = other.fullLoadPerception_;
		currentMoveSpeed_ = other.currentMoveSpeed_;
		superAirRecoverValue_ = other.superAirRecoverValue_;
		defenseRecoverValue_ = other.defenseRecoverValue_;
		superAirRecoverValueOnBattle_ = other.superAirRecoverValueOnBattle_;
		defenseRecoverValueOnBattle_ = other.defenseRecoverValueOnBattle_;
		superAirPower_ = other.superAirPower_;
		canUsePhysique_ = other.canUsePhysique_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterEquipmentResp_1009005 Clone()
	{
		return new EnterEquipmentResp_1009005(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterEquipmentResp_1009005);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterEquipmentResp_1009005 other)
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
		if (!dressedEquipments_.Equals(other.dressedEquipments_))
		{
			return false;
		}
		if (CurrentPhysiqueDefense != other.CurrentPhysiqueDefense)
		{
			return false;
		}
		if (CurrentPureBloodDefense != other.CurrentPureBloodDefense)
		{
			return false;
		}
		if (FullLoadPerception != other.FullLoadPerception)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(CurrentMoveSpeed, other.CurrentMoveSpeed))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(SuperAirRecoverValue, other.SuperAirRecoverValue))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(DefenseRecoverValue, other.DefenseRecoverValue))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(SuperAirRecoverValueOnBattle, other.SuperAirRecoverValueOnBattle))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(DefenseRecoverValueOnBattle, other.DefenseRecoverValueOnBattle))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(SuperAirPower, other.SuperAirPower))
		{
			return false;
		}
		if (CanUsePhysique != other.CanUsePhysique)
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
		num ^= dressedEquipments_.GetHashCode();
		if (CurrentPhysiqueDefense != 0L)
		{
			num ^= CurrentPhysiqueDefense.GetHashCode();
		}
		if (CurrentPureBloodDefense != 0)
		{
			num ^= CurrentPureBloodDefense.GetHashCode();
		}
		if (FullLoadPerception != 0)
		{
			num ^= FullLoadPerception.GetHashCode();
		}
		if (CurrentMoveSpeed != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(CurrentMoveSpeed);
		}
		if (SuperAirRecoverValue != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(SuperAirRecoverValue);
		}
		if (DefenseRecoverValue != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(DefenseRecoverValue);
		}
		if (SuperAirRecoverValueOnBattle != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(SuperAirRecoverValueOnBattle);
		}
		if (DefenseRecoverValueOnBattle != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(DefenseRecoverValueOnBattle);
		}
		if (SuperAirPower != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(SuperAirPower);
		}
		if (CanUsePhysique != 0)
		{
			num ^= CanUsePhysique.GetHashCode();
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
		dressedEquipments_.WriteTo(output, _repeated_dressedEquipments_codec);
		if (CurrentPhysiqueDefense != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(CurrentPhysiqueDefense);
		}
		if (CurrentPureBloodDefense != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(CurrentPureBloodDefense);
		}
		if (FullLoadPerception != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(FullLoadPerception);
		}
		if (CurrentMoveSpeed != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(CurrentMoveSpeed);
		}
		if (SuperAirRecoverValue != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(SuperAirRecoverValue);
		}
		if (DefenseRecoverValue != 0.0)
		{
			output.WriteRawTag(73);
			output.WriteDouble(DefenseRecoverValue);
		}
		if (SuperAirRecoverValueOnBattle != 0.0)
		{
			output.WriteRawTag(81);
			output.WriteDouble(SuperAirRecoverValueOnBattle);
		}
		if (DefenseRecoverValueOnBattle != 0.0)
		{
			output.WriteRawTag(89);
			output.WriteDouble(DefenseRecoverValueOnBattle);
		}
		if (SuperAirPower != 0.0)
		{
			output.WriteRawTag(97);
			output.WriteDouble(SuperAirPower);
		}
		if (CanUsePhysique != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(CanUsePhysique);
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
		num += dressedEquipments_.CalculateSize(_repeated_dressedEquipments_codec);
		if (CurrentPhysiqueDefense != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentPhysiqueDefense);
		}
		if (CurrentPureBloodDefense != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPureBloodDefense);
		}
		if (FullLoadPerception != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FullLoadPerception);
		}
		if (CurrentMoveSpeed != 0.0)
		{
			num += 9;
		}
		if (SuperAirRecoverValue != 0.0)
		{
			num += 9;
		}
		if (DefenseRecoverValue != 0.0)
		{
			num += 9;
		}
		if (SuperAirRecoverValueOnBattle != 0.0)
		{
			num += 9;
		}
		if (DefenseRecoverValueOnBattle != 0.0)
		{
			num += 9;
		}
		if (SuperAirPower != 0.0)
		{
			num += 9;
		}
		if (CanUsePhysique != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CanUsePhysique);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterEquipmentResp_1009005 other)
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
			dressedEquipments_.Add(other.dressedEquipments_);
			if (other.CurrentPhysiqueDefense != 0L)
			{
				CurrentPhysiqueDefense = other.CurrentPhysiqueDefense;
			}
			if (other.CurrentPureBloodDefense != 0)
			{
				CurrentPureBloodDefense = other.CurrentPureBloodDefense;
			}
			if (other.FullLoadPerception != 0)
			{
				FullLoadPerception = other.FullLoadPerception;
			}
			if (other.CurrentMoveSpeed != 0.0)
			{
				CurrentMoveSpeed = other.CurrentMoveSpeed;
			}
			if (other.SuperAirRecoverValue != 0.0)
			{
				SuperAirRecoverValue = other.SuperAirRecoverValue;
			}
			if (other.DefenseRecoverValue != 0.0)
			{
				DefenseRecoverValue = other.DefenseRecoverValue;
			}
			if (other.SuperAirRecoverValueOnBattle != 0.0)
			{
				SuperAirRecoverValueOnBattle = other.SuperAirRecoverValueOnBattle;
			}
			if (other.DefenseRecoverValueOnBattle != 0.0)
			{
				DefenseRecoverValueOnBattle = other.DefenseRecoverValueOnBattle;
			}
			if (other.SuperAirPower != 0.0)
			{
				SuperAirPower = other.SuperAirPower;
			}
			if (other.CanUsePhysique != 0)
			{
				CanUsePhysique = other.CanUsePhysique;
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
				dressedEquipments_.AddEntriesFrom(input, _repeated_dressedEquipments_codec);
				break;
			case 32u:
				CurrentPhysiqueDefense = input.ReadInt64();
				break;
			case 40u:
				CurrentPureBloodDefense = input.ReadInt32();
				break;
			case 48u:
				FullLoadPerception = input.ReadInt32();
				break;
			case 57u:
				CurrentMoveSpeed = input.ReadDouble();
				break;
			case 65u:
				SuperAirRecoverValue = input.ReadDouble();
				break;
			case 73u:
				DefenseRecoverValue = input.ReadDouble();
				break;
			case 81u:
				SuperAirRecoverValueOnBattle = input.ReadDouble();
				break;
			case 89u:
				DefenseRecoverValueOnBattle = input.ReadDouble();
				break;
			case 97u:
				SuperAirPower = input.ReadDouble();
				break;
			case 104u:
				CanUsePhysique = input.ReadInt32();
				break;
			}
		}
	}
}
