using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterThunderCalamityResp_1002009 : IMessage<EnterThunderCalamityResp_1002009>, IMessage, IEquatable<EnterThunderCalamityResp_1002009>, IDeepCloneable<EnterThunderCalamityResp_1002009>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class BattleSkillResp_1002010 : IMessage<BattleSkillResp_1002010>, IMessage, IEquatable<BattleSkillResp_1002010>, IDeepCloneable<BattleSkillResp_1002010>
		{
			private static readonly MessageParser<BattleSkillResp_1002010> _parser = new MessageParser<BattleSkillResp_1002010>(() => new BattleSkillResp_1002010());

			private UnknownFieldSet _unknownFields;

			public const int IconFieldNumber = 1;

			private int icon_;

			public const int TypeFieldNumber = 2;

			private int type_;

			public const int SkillTypeFieldNumber = 12;

			private int skillType_;

			public const int NameFieldNumber = 3;

			private string name_ = "";

			public const int TemplateIdFieldNumber = 4;

			private string templateId_ = "";

			public const int CastTimeFieldNumber = 5;

			private int castTime_;

			public const int ContinueTimeFieldNumber = 6;

			private int continueTime_;

			public const int CoolDownFieldNumber = 7;

			private int coolDown_;

			public const int FiveElementFieldNumber = 8;

			private int fiveElement_;

			public const int ConversionRateFieldNumber = 9;

			private double conversionRate_;

			public const int BaseDamageFieldNumber = 10;

			private double baseDamage_;

			public const int UsedPerceptionFieldNumber = 11;

			private int usedPerception_;

			public const int LevelFieldNumber = 13;

			private int level_;

			public const int SkillIndexFieldNumber = 14;

			private int skillIndex_;

			public const int UsedSuperAirFieldNumber = 15;

			private long usedSuperAir_;

			public const int CanUseCountFieldNumber = 16;

			private int canUseCount_;

			public const int RankFieldNumber = 17;

			private int rank_;

			public const int UnitUsedFieldNumber = 18;

			private double unitUsed_;

			[DebuggerNonUserCode]
			public static MessageParser<BattleSkillResp_1002010> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterThunderCalamityResp_1002009.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
			public int SkillType
			{
				get
				{
					return skillType_;
				}
				set
				{
					skillType_ = value;
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
			public int CastTime
			{
				get
				{
					return castTime_;
				}
				set
				{
					castTime_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int ContinueTime
			{
				get
				{
					return continueTime_;
				}
				set
				{
					continueTime_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int CoolDown
			{
				get
				{
					return coolDown_;
				}
				set
				{
					coolDown_ = value;
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
			public double ConversionRate
			{
				get
				{
					return conversionRate_;
				}
				set
				{
					conversionRate_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double BaseDamage
			{
				get
				{
					return baseDamage_;
				}
				set
				{
					baseDamage_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int UsedPerception
			{
				get
				{
					return usedPerception_;
				}
				set
				{
					usedPerception_ = value;
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
			public int SkillIndex
			{
				get
				{
					return skillIndex_;
				}
				set
				{
					skillIndex_ = value;
				}
			}

			[DebuggerNonUserCode]
			public long UsedSuperAir
			{
				get
				{
					return usedSuperAir_;
				}
				set
				{
					usedSuperAir_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int CanUseCount
			{
				get
				{
					return canUseCount_;
				}
				set
				{
					canUseCount_ = value;
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
			public double UnitUsed
			{
				get
				{
					return unitUsed_;
				}
				set
				{
					unitUsed_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public BattleSkillResp_1002010()
			{
			}

			[DebuggerNonUserCode]
			public BattleSkillResp_1002010(BattleSkillResp_1002010 other)
				: this()
			{
				icon_ = other.icon_;
				type_ = other.type_;
				skillType_ = other.skillType_;
				name_ = other.name_;
				templateId_ = other.templateId_;
				castTime_ = other.castTime_;
				continueTime_ = other.continueTime_;
				coolDown_ = other.coolDown_;
				fiveElement_ = other.fiveElement_;
				conversionRate_ = other.conversionRate_;
				baseDamage_ = other.baseDamage_;
				usedPerception_ = other.usedPerception_;
				level_ = other.level_;
				skillIndex_ = other.skillIndex_;
				usedSuperAir_ = other.usedSuperAir_;
				canUseCount_ = other.canUseCount_;
				rank_ = other.rank_;
				unitUsed_ = other.unitUsed_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public BattleSkillResp_1002010 Clone()
			{
				return new BattleSkillResp_1002010(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as BattleSkillResp_1002010);
			}

			[DebuggerNonUserCode]
			public bool Equals(BattleSkillResp_1002010 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (Icon != other.Icon)
				{
					return false;
				}
				if (Type != other.Type)
				{
					return false;
				}
				if (SkillType != other.SkillType)
				{
					return false;
				}
				if (Name != other.Name)
				{
					return false;
				}
				if (TemplateId != other.TemplateId)
				{
					return false;
				}
				if (CastTime != other.CastTime)
				{
					return false;
				}
				if (ContinueTime != other.ContinueTime)
				{
					return false;
				}
				if (CoolDown != other.CoolDown)
				{
					return false;
				}
				if (FiveElement != other.FiveElement)
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(BaseDamage, other.BaseDamage))
				{
					return false;
				}
				if (UsedPerception != other.UsedPerception)
				{
					return false;
				}
				if (Level != other.Level)
				{
					return false;
				}
				if (SkillIndex != other.SkillIndex)
				{
					return false;
				}
				if (UsedSuperAir != other.UsedSuperAir)
				{
					return false;
				}
				if (CanUseCount != other.CanUseCount)
				{
					return false;
				}
				if (Rank != other.Rank)
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(UnitUsed, other.UnitUsed))
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (Icon != 0)
				{
					num ^= Icon.GetHashCode();
				}
				if (Type != 0)
				{
					num ^= Type.GetHashCode();
				}
				if (SkillType != 0)
				{
					num ^= SkillType.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (TemplateId.Length != 0)
				{
					num ^= TemplateId.GetHashCode();
				}
				if (CastTime != 0)
				{
					num ^= CastTime.GetHashCode();
				}
				if (ContinueTime != 0)
				{
					num ^= ContinueTime.GetHashCode();
				}
				if (CoolDown != 0)
				{
					num ^= CoolDown.GetHashCode();
				}
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
				}
				if (ConversionRate != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
				}
				if (BaseDamage != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(BaseDamage);
				}
				if (UsedPerception != 0)
				{
					num ^= UsedPerception.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
				}
				if (SkillIndex != 0)
				{
					num ^= SkillIndex.GetHashCode();
				}
				if (UsedSuperAir != 0L)
				{
					num ^= UsedSuperAir.GetHashCode();
				}
				if (CanUseCount != 0)
				{
					num ^= CanUseCount.GetHashCode();
				}
				if (Rank != 0)
				{
					num ^= Rank.GetHashCode();
				}
				if (UnitUsed != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(UnitUsed);
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
				if (Icon != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(Icon);
				}
				if (Type != 0)
				{
					output.WriteRawTag(16);
					output.WriteInt32(Type);
				}
				if (Name.Length != 0)
				{
					output.WriteRawTag(26);
					output.WriteString(Name);
				}
				if (TemplateId.Length != 0)
				{
					output.WriteRawTag(34);
					output.WriteString(TemplateId);
				}
				if (CastTime != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(CastTime);
				}
				if (ContinueTime != 0)
				{
					output.WriteRawTag(48);
					output.WriteInt32(ContinueTime);
				}
				if (CoolDown != 0)
				{
					output.WriteRawTag(56);
					output.WriteInt32(CoolDown);
				}
				if (FiveElement != 0)
				{
					output.WriteRawTag(64);
					output.WriteInt32(FiveElement);
				}
				if (ConversionRate != 0.0)
				{
					output.WriteRawTag(73);
					output.WriteDouble(ConversionRate);
				}
				if (BaseDamage != 0.0)
				{
					output.WriteRawTag(81);
					output.WriteDouble(BaseDamage);
				}
				if (UsedPerception != 0)
				{
					output.WriteRawTag(88);
					output.WriteInt32(UsedPerception);
				}
				if (SkillType != 0)
				{
					output.WriteRawTag(96);
					output.WriteInt32(SkillType);
				}
				if (Level != 0)
				{
					output.WriteRawTag(104);
					output.WriteInt32(Level);
				}
				if (SkillIndex != 0)
				{
					output.WriteRawTag(112);
					output.WriteInt32(SkillIndex);
				}
				if (UsedSuperAir != 0L)
				{
					output.WriteRawTag(120);
					output.WriteInt64(UsedSuperAir);
				}
				if (CanUseCount != 0)
				{
					output.WriteRawTag(128, 1);
					output.WriteInt32(CanUseCount);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(136, 1);
					output.WriteInt32(Rank);
				}
				if (UnitUsed != 0.0)
				{
					output.WriteRawTag(145, 1);
					output.WriteDouble(UnitUsed);
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
				if (Icon != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Icon);
				}
				if (Type != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Type);
				}
				if (SkillType != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(SkillType);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (TemplateId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(TemplateId);
				}
				if (CastTime != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(CastTime);
				}
				if (ContinueTime != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(ContinueTime);
				}
				if (CoolDown != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(CoolDown);
				}
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (ConversionRate != 0.0)
				{
					num += 9;
				}
				if (BaseDamage != 0.0)
				{
					num += 9;
				}
				if (UsedPerception != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(UsedPerception);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (SkillIndex != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(SkillIndex);
				}
				if (UsedSuperAir != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(UsedSuperAir);
				}
				if (CanUseCount != 0)
				{
					num += 2 + CodedOutputStream.ComputeInt32Size(CanUseCount);
				}
				if (Rank != 0)
				{
					num += 2 + CodedOutputStream.ComputeInt32Size(Rank);
				}
				if (UnitUsed != 0.0)
				{
					num += 10;
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(BattleSkillResp_1002010 other)
			{
				if (other != null)
				{
					if (other.Icon != 0)
					{
						Icon = other.Icon;
					}
					if (other.Type != 0)
					{
						Type = other.Type;
					}
					if (other.SkillType != 0)
					{
						SkillType = other.SkillType;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.TemplateId.Length != 0)
					{
						TemplateId = other.TemplateId;
					}
					if (other.CastTime != 0)
					{
						CastTime = other.CastTime;
					}
					if (other.ContinueTime != 0)
					{
						ContinueTime = other.ContinueTime;
					}
					if (other.CoolDown != 0)
					{
						CoolDown = other.CoolDown;
					}
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
					}
					if (other.ConversionRate != 0.0)
					{
						ConversionRate = other.ConversionRate;
					}
					if (other.BaseDamage != 0.0)
					{
						BaseDamage = other.BaseDamage;
					}
					if (other.UsedPerception != 0)
					{
						UsedPerception = other.UsedPerception;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
					}
					if (other.SkillIndex != 0)
					{
						SkillIndex = other.SkillIndex;
					}
					if (other.UsedSuperAir != 0L)
					{
						UsedSuperAir = other.UsedSuperAir;
					}
					if (other.CanUseCount != 0)
					{
						CanUseCount = other.CanUseCount;
					}
					if (other.Rank != 0)
					{
						Rank = other.Rank;
					}
					if (other.UnitUsed != 0.0)
					{
						UnitUsed = other.UnitUsed;
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
						Icon = input.ReadInt32();
						break;
					case 16u:
						Type = input.ReadInt32();
						break;
					case 26u:
						Name = input.ReadString();
						break;
					case 34u:
						TemplateId = input.ReadString();
						break;
					case 40u:
						CastTime = input.ReadInt32();
						break;
					case 48u:
						ContinueTime = input.ReadInt32();
						break;
					case 56u:
						CoolDown = input.ReadInt32();
						break;
					case 64u:
						FiveElement = input.ReadInt32();
						break;
					case 73u:
						ConversionRate = input.ReadDouble();
						break;
					case 81u:
						BaseDamage = input.ReadDouble();
						break;
					case 88u:
						UsedPerception = input.ReadInt32();
						break;
					case 96u:
						SkillType = input.ReadInt32();
						break;
					case 104u:
						Level = input.ReadInt32();
						break;
					case 112u:
						SkillIndex = input.ReadInt32();
						break;
					case 120u:
						UsedSuperAir = input.ReadInt64();
						break;
					case 128u:
						CanUseCount = input.ReadInt32();
						break;
					case 136u:
						Rank = input.ReadInt32();
						break;
					case 145u:
						UnitUsed = input.ReadDouble();
						break;
					}
				}
			}
		}

		public sealed class ThunderDamageResp_1002011 : IMessage<ThunderDamageResp_1002011>, IMessage, IEquatable<ThunderDamageResp_1002011>, IDeepCloneable<ThunderDamageResp_1002011>
		{
			private static readonly MessageParser<ThunderDamageResp_1002011> _parser = new MessageParser<ThunderDamageResp_1002011>(() => new ThunderDamageResp_1002011());

			private UnknownFieldSet _unknownFields;

			public const int DamageFieldNumber = 1;

			private long damage_;

			[DebuggerNonUserCode]
			public static MessageParser<ThunderDamageResp_1002011> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterThunderCalamityResp_1002009.Descriptor.NestedTypes[1];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public long Damage
			{
				get
				{
					return damage_;
				}
				set
				{
					damage_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public ThunderDamageResp_1002011()
			{
			}

			[DebuggerNonUserCode]
			public ThunderDamageResp_1002011(ThunderDamageResp_1002011 other)
				: this()
			{
				damage_ = other.damage_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ThunderDamageResp_1002011 Clone()
			{
				return new ThunderDamageResp_1002011(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ThunderDamageResp_1002011);
			}

			[DebuggerNonUserCode]
			public bool Equals(ThunderDamageResp_1002011 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (Damage != other.Damage)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (Damage != 0L)
				{
					num ^= Damage.GetHashCode();
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
				if (Damage != 0L)
				{
					output.WriteRawTag(8);
					output.WriteInt64(Damage);
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
				if (Damage != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(Damage);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(ThunderDamageResp_1002011 other)
			{
				if (other != null)
				{
					if (other.Damage != 0L)
					{
						Damage = other.Damage;
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
					if (num != 8)
					{
						_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
					}
					else
					{
						Damage = input.ReadInt64();
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterThunderCalamityResp_1002009> _parser = new MessageParser<EnterThunderCalamityResp_1002009>(() => new EnterThunderCalamityResp_1002009());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentPhysiqueDefenseFieldNumber = 3;

	private long currentPhysiqueDefense_;

	public const int CurrentPureBloodDefenseFieldNumber = 4;

	private int currentPureBloodDefense_;

	public const int CurrentSuperAirFieldNumber = 5;

	private long currentSuperAir_;

	public const int CurrentPerceptionFieldNumber = 6;

	private int currentPerception_;

	public const int DamageReductionFieldNumber = 7;

	private double damageReduction_;

	public const int DodgeCoolDownFieldNumber = 8;

	private double dodgeCoolDown_;

	public const int SkillsFieldNumber = 9;

	private static readonly FieldCodec<Types.BattleSkillResp_1002010> _repeated_skills_codec = FieldCodec.ForMessage(74u, Types.BattleSkillResp_1002010.Parser);

	private readonly RepeatedField<Types.BattleSkillResp_1002010> skills_ = new RepeatedField<Types.BattleSkillResp_1002010>();

	public const int ThunderDamageFieldNumber = 10;

	private static readonly FieldCodec<Types.ThunderDamageResp_1002011> _repeated_thunderDamage_codec = FieldCodec.ForMessage(82u, Types.ThunderDamageResp_1002011.Parser);

	private readonly RepeatedField<Types.ThunderDamageResp_1002011> thunderDamage_ = new RepeatedField<Types.ThunderDamageResp_1002011>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterThunderCalamityResp_1002009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[13];

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
	public long CurrentSuperAir
	{
		get
		{
			return currentSuperAir_;
		}
		set
		{
			currentSuperAir_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentPerception
	{
		get
		{
			return currentPerception_;
		}
		set
		{
			currentPerception_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double DamageReduction
	{
		get
		{
			return damageReduction_;
		}
		set
		{
			damageReduction_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double DodgeCoolDown
	{
		get
		{
			return dodgeCoolDown_;
		}
		set
		{
			dodgeCoolDown_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.BattleSkillResp_1002010> Skills => skills_;

	[DebuggerNonUserCode]
	public RepeatedField<Types.ThunderDamageResp_1002011> ThunderDamage => thunderDamage_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterThunderCalamityResp_1002009()
	{
	}

	[DebuggerNonUserCode]
	public EnterThunderCalamityResp_1002009(EnterThunderCalamityResp_1002009 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentPhysiqueDefense_ = other.currentPhysiqueDefense_;
		currentPureBloodDefense_ = other.currentPureBloodDefense_;
		currentSuperAir_ = other.currentSuperAir_;
		currentPerception_ = other.currentPerception_;
		damageReduction_ = other.damageReduction_;
		dodgeCoolDown_ = other.dodgeCoolDown_;
		skills_ = other.skills_.Clone();
		thunderDamage_ = other.thunderDamage_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterThunderCalamityResp_1002009 Clone()
	{
		return new EnterThunderCalamityResp_1002009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterThunderCalamityResp_1002009);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterThunderCalamityResp_1002009 other)
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
		if (CurrentPhysiqueDefense != other.CurrentPhysiqueDefense)
		{
			return false;
		}
		if (CurrentPureBloodDefense != other.CurrentPureBloodDefense)
		{
			return false;
		}
		if (CurrentSuperAir != other.CurrentSuperAir)
		{
			return false;
		}
		if (CurrentPerception != other.CurrentPerception)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(DamageReduction, other.DamageReduction))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(DodgeCoolDown, other.DodgeCoolDown))
		{
			return false;
		}
		if (!skills_.Equals(other.skills_))
		{
			return false;
		}
		if (!thunderDamage_.Equals(other.thunderDamage_))
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
		if (CurrentPhysiqueDefense != 0L)
		{
			num ^= CurrentPhysiqueDefense.GetHashCode();
		}
		if (CurrentPureBloodDefense != 0)
		{
			num ^= CurrentPureBloodDefense.GetHashCode();
		}
		if (CurrentSuperAir != 0L)
		{
			num ^= CurrentSuperAir.GetHashCode();
		}
		if (CurrentPerception != 0)
		{
			num ^= CurrentPerception.GetHashCode();
		}
		if (DamageReduction != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(DamageReduction);
		}
		if (DodgeCoolDown != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(DodgeCoolDown);
		}
		num ^= skills_.GetHashCode();
		num ^= thunderDamage_.GetHashCode();
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
		if (CurrentPhysiqueDefense != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(CurrentPhysiqueDefense);
		}
		if (CurrentPureBloodDefense != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(CurrentPureBloodDefense);
		}
		if (CurrentSuperAir != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(CurrentSuperAir);
		}
		if (CurrentPerception != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(CurrentPerception);
		}
		if (DamageReduction != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(DamageReduction);
		}
		if (DodgeCoolDown != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(DodgeCoolDown);
		}
		skills_.WriteTo(output, _repeated_skills_codec);
		thunderDamage_.WriteTo(output, _repeated_thunderDamage_codec);
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
		if (CurrentPhysiqueDefense != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentPhysiqueDefense);
		}
		if (CurrentPureBloodDefense != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPureBloodDefense);
		}
		if (CurrentSuperAir != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentSuperAir);
		}
		if (CurrentPerception != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPerception);
		}
		if (DamageReduction != 0.0)
		{
			num += 9;
		}
		if (DodgeCoolDown != 0.0)
		{
			num += 9;
		}
		num += skills_.CalculateSize(_repeated_skills_codec);
		num += thunderDamage_.CalculateSize(_repeated_thunderDamage_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterThunderCalamityResp_1002009 other)
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
			if (other.CurrentPhysiqueDefense != 0L)
			{
				CurrentPhysiqueDefense = other.CurrentPhysiqueDefense;
			}
			if (other.CurrentPureBloodDefense != 0)
			{
				CurrentPureBloodDefense = other.CurrentPureBloodDefense;
			}
			if (other.CurrentSuperAir != 0L)
			{
				CurrentSuperAir = other.CurrentSuperAir;
			}
			if (other.CurrentPerception != 0)
			{
				CurrentPerception = other.CurrentPerception;
			}
			if (other.DamageReduction != 0.0)
			{
				DamageReduction = other.DamageReduction;
			}
			if (other.DodgeCoolDown != 0.0)
			{
				DodgeCoolDown = other.DodgeCoolDown;
			}
			skills_.Add(other.skills_);
			thunderDamage_.Add(other.thunderDamage_);
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
				CurrentPhysiqueDefense = input.ReadInt64();
				break;
			case 32u:
				CurrentPureBloodDefense = input.ReadInt32();
				break;
			case 40u:
				CurrentSuperAir = input.ReadInt64();
				break;
			case 48u:
				CurrentPerception = input.ReadInt32();
				break;
			case 57u:
				DamageReduction = input.ReadDouble();
				break;
			case 65u:
				DodgeCoolDown = input.ReadDouble();
				break;
			case 74u:
				skills_.AddEntriesFrom(input, _repeated_skills_codec);
				break;
			case 82u:
				thunderDamage_.AddEntriesFrom(input, _repeated_thunderDamage_codec);
				break;
			}
		}
	}
}
