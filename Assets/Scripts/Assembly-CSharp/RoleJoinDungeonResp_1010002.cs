using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class RoleJoinDungeonResp_1010002 : IMessage<RoleJoinDungeonResp_1010002>, IMessage, IEquatable<RoleJoinDungeonResp_1010002>, IDeepCloneable<RoleJoinDungeonResp_1010002>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class BattleSkillResp_1010003 : IMessage<BattleSkillResp_1010003>, IMessage, IEquatable<BattleSkillResp_1010003>, IDeepCloneable<BattleSkillResp_1010003>
		{
			private static readonly MessageParser<BattleSkillResp_1010003> _parser = new MessageParser<BattleSkillResp_1010003>(() => new BattleSkillResp_1010003());

			public int PracticeAffix { get; set; }

			public int ForgeResonancePercent { get; set; }

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

			public const int FireDamageAddFieldNumber = 19;

			private double fireDamageAdd_;

			public const int WoodDamageAddFieldNumber = 20;

			private double woodDamageAdd_;

			public const int WaterDamageAddFieldNumber = 21;

			private double waterDamageAdd_;

			public const int GoldDamageAddFieldNumber = 22;

			private double goldDamageAdd_;

			public const int EarthDamageAddFieldNumber = 23;

			private double earthDamageAdd_;

			public const int UnitConsumeFieldNumber = 24;

			private int unitConsume_;

			public const int ScopeFieldNumber = 25;

			private double scope_;

			[DebuggerNonUserCode]
			public static MessageParser<BattleSkillResp_1010003> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => RoleJoinDungeonResp_1010002.Descriptor.NestedTypes[0];

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

			[DebuggerNonUserCode]
			public double FireDamageAdd
			{
				get
				{
					return fireDamageAdd_;
				}
				set
				{
					fireDamageAdd_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double WoodDamageAdd
			{
				get
				{
					return woodDamageAdd_;
				}
				set
				{
					woodDamageAdd_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double WaterDamageAdd
			{
				get
				{
					return waterDamageAdd_;
				}
				set
				{
					waterDamageAdd_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double GoldDamageAdd
			{
				get
				{
					return goldDamageAdd_;
				}
				set
				{
					goldDamageAdd_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double EarthDamageAdd
			{
				get
				{
					return earthDamageAdd_;
				}
				set
				{
					earthDamageAdd_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int UnitConsume
			{
				get
				{
					return unitConsume_;
				}
				set
				{
					unitConsume_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double Scope
			{
				get
				{
					return scope_;
				}
				set
				{
					scope_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public BattleSkillResp_1010003()
			{
			}

			[DebuggerNonUserCode]
			public BattleSkillResp_1010003(BattleSkillResp_1010003 other)
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
				fireDamageAdd_ = other.fireDamageAdd_;
				woodDamageAdd_ = other.woodDamageAdd_;
				waterDamageAdd_ = other.waterDamageAdd_;
				goldDamageAdd_ = other.goldDamageAdd_;
				earthDamageAdd_ = other.earthDamageAdd_;
				unitConsume_ = other.unitConsume_;
				scope_ = other.scope_;
				PracticeAffix = other.PracticeAffix;
				ForgeResonancePercent = other.ForgeResonancePercent;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public BattleSkillResp_1010003 Clone()
			{
				return new BattleSkillResp_1010003(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as BattleSkillResp_1010003);
			}

			[DebuggerNonUserCode]
			public bool Equals(BattleSkillResp_1010003 other)
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
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(FireDamageAdd, other.FireDamageAdd))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(WoodDamageAdd, other.WoodDamageAdd))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(WaterDamageAdd, other.WaterDamageAdd))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(GoldDamageAdd, other.GoldDamageAdd))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(EarthDamageAdd, other.EarthDamageAdd))
				{
					return false;
				}
				if (UnitConsume != other.UnitConsume)
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Scope, other.Scope))
				{
					return false;
				}
				if (PracticeAffix != other.PracticeAffix) return false;
				if (ForgeResonancePercent != other.ForgeResonancePercent) return false;
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
				if (FireDamageAdd != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(FireDamageAdd);
				}
				if (WoodDamageAdd != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(WoodDamageAdd);
				}
				if (WaterDamageAdd != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(WaterDamageAdd);
				}
				if (GoldDamageAdd != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(GoldDamageAdd);
				}
				if (EarthDamageAdd != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(EarthDamageAdd);
				}
				if (UnitConsume != 0)
				{
					num ^= UnitConsume.GetHashCode();
				}
				if (Scope != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Scope);
				}
				if (PracticeAffix != 0) num ^= PracticeAffix.GetHashCode();
				if (ForgeResonancePercent != 0) num ^= ForgeResonancePercent.GetHashCode();
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
				if (FireDamageAdd != 0.0)
				{
					output.WriteRawTag(153, 1);
					output.WriteDouble(FireDamageAdd);
				}
				if (WoodDamageAdd != 0.0)
				{
					output.WriteRawTag(161, 1);
					output.WriteDouble(WoodDamageAdd);
				}
				if (WaterDamageAdd != 0.0)
				{
					output.WriteRawTag(169, 1);
					output.WriteDouble(WaterDamageAdd);
				}
				if (GoldDamageAdd != 0.0)
				{
					output.WriteRawTag(177, 1);
					output.WriteDouble(GoldDamageAdd);
				}
				if (EarthDamageAdd != 0.0)
				{
					output.WriteRawTag(185, 1);
					output.WriteDouble(EarthDamageAdd);
				}
				if (UnitConsume != 0)
				{
					output.WriteRawTag(192, 1);
					output.WriteInt32(UnitConsume);
				}
				if (Scope != 0.0)
				{
					output.WriteRawTag(201, 1);
					output.WriteDouble(Scope);
				}
				if (PracticeAffix != 0) { output.WriteTag(26, WireFormat.WireType.Varint); output.WriteInt32(PracticeAffix); }
				if (ForgeResonancePercent != 0) { output.WriteTag(27, WireFormat.WireType.Varint); output.WriteInt32(ForgeResonancePercent); }
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
				if (FireDamageAdd != 0.0)
				{
					num += 10;
				}
				if (WoodDamageAdd != 0.0)
				{
					num += 10;
				}
				if (WaterDamageAdd != 0.0)
				{
					num += 10;
				}
				if (GoldDamageAdd != 0.0)
				{
					num += 10;
				}
				if (EarthDamageAdd != 0.0)
				{
					num += 10;
				}
				if (UnitConsume != 0)
				{
					num += 2 + CodedOutputStream.ComputeInt32Size(UnitConsume);
				}
				if (Scope != 0.0)
				{
					num += 10;
				}
				if (PracticeAffix != 0) num += CodedOutputStream.ComputeTagSize(26) + CodedOutputStream.ComputeInt32Size(PracticeAffix);
				if (ForgeResonancePercent != 0) num += CodedOutputStream.ComputeTagSize(27) + CodedOutputStream.ComputeInt32Size(ForgeResonancePercent);
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(BattleSkillResp_1010003 other)
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
					if (other.FireDamageAdd != 0.0)
					{
						FireDamageAdd = other.FireDamageAdd;
					}
					if (other.WoodDamageAdd != 0.0)
					{
						WoodDamageAdd = other.WoodDamageAdd;
					}
					if (other.WaterDamageAdd != 0.0)
					{
						WaterDamageAdd = other.WaterDamageAdd;
					}
					if (other.GoldDamageAdd != 0.0)
					{
						GoldDamageAdd = other.GoldDamageAdd;
					}
					if (other.EarthDamageAdd != 0.0)
					{
						EarthDamageAdd = other.EarthDamageAdd;
					}
					if (other.UnitConsume != 0)
					{
						UnitConsume = other.UnitConsume;
					}
					if (other.Scope != 0.0)
					{
						Scope = other.Scope;
					}
					if (other.PracticeAffix != 0) PracticeAffix = other.PracticeAffix;
					if (other.ForgeResonancePercent != 0) ForgeResonancePercent = other.ForgeResonancePercent;
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
					case 208u: PracticeAffix = input.ReadInt32(); break;
					case 216u: ForgeResonancePercent = input.ReadInt32(); break;
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
					case 153u:
						FireDamageAdd = input.ReadDouble();
						break;
					case 161u:
						WoodDamageAdd = input.ReadDouble();
						break;
					case 169u:
						WaterDamageAdd = input.ReadDouble();
						break;
					case 177u:
						GoldDamageAdd = input.ReadDouble();
						break;
					case 185u:
						EarthDamageAdd = input.ReadDouble();
						break;
					case 192u:
						UnitConsume = input.ReadInt32();
						break;
					case 201u:
						Scope = input.ReadDouble();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<RoleJoinDungeonResp_1010002> _parser = new MessageParser<RoleJoinDungeonResp_1010002>(() => new RoleJoinDungeonResp_1010002());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int DungeonIdFieldNumber = 3;

	private string dungeonId_ = "";

	public const int RoleIdFieldNumber = 4;

	private string roleId_ = "";

	public const int NameFieldNumber = 5;

	private string name_ = "";

	public const int CurrentPhysiqueDefenseFieldNumber = 6;

	private long currentPhysiqueDefense_;

	public const int CurrentPureBloodDefenseFieldNumber = 7;

	private int currentPureBloodDefense_;

	public const int CurrentSuperAirFieldNumber = 8;

	private long currentSuperAir_;

	public const int CurrentPerceptionFieldNumber = 9;

	private int currentPerception_;

	public const int CurrentMoveSpeedFieldNumber = 10;

	private double currentMoveSpeed_;

	public const int LevelFieldNumber = 11;

	private int level_;

	public const int PhysiqueFiveElementFieldNumber = 12;

	private int physiqueFiveElement_;

	public const int SuperAirFiveElementFieldNumber = 13;

	private int superAirFiveElement_;

	public const int PerceptionFiveElementFieldNumber = 14;

	private int perceptionFiveElement_;

	public const int IconFieldNumber = 15;

	private int icon_;

	public const int SuperAirRecoverValueFieldNumber = 16;

	private double superAirRecoverValue_;

	public const int DefenseRecoverValueFieldNumber = 17;

	private double defenseRecoverValue_;

	public const int SuperAirRecoverValueOnBattleFieldNumber = 18;

	private double superAirRecoverValueOnBattle_;

	public const int DefenseRecoverValueOnBattleFieldNumber = 19;

	private double defenseRecoverValueOnBattle_;

	public const int SkillsFieldNumber = 20;

	private static readonly FieldCodec<Types.BattleSkillResp_1010003> _repeated_skills_codec = FieldCodec.ForMessage(162u, Types.BattleSkillResp_1010003.Parser);

	private readonly RepeatedField<Types.BattleSkillResp_1010003> skills_ = new RepeatedField<Types.BattleSkillResp_1010003>();

	public const int GoldDamageAvoidanceFieldNumber = 21;

	private double goldDamageAvoidance_;

	public const int WoodDamageAvoidanceFieldNumber = 22;

	private double woodDamageAvoidance_;

	public const int WaterDamageAvoidanceFieldNumber = 23;

	private double waterDamageAvoidance_;

	public const int FireDamageAvoidanceFieldNumber = 24;

	private double fireDamageAvoidance_;

	public const int EarthDamageAvoidanceFieldNumber = 25;

	private double earthDamageAvoidance_;

	public const int AttackDistanceFieldNumber = 26;

	private double attackDistance_;

	[DebuggerNonUserCode]
	public static MessageParser<RoleJoinDungeonResp_1010002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleProtoReflection.Descriptor.MessageTypes[3];

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
	public string DungeonId
	{
		get
		{
			return dungeonId_;
		}
		set
		{
			dungeonId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string RoleId
	{
		get
		{
			return roleId_;
		}
		set
		{
			roleId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public int PhysiqueFiveElement
	{
		get
		{
			return physiqueFiveElement_;
		}
		set
		{
			physiqueFiveElement_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SuperAirFiveElement
	{
		get
		{
			return superAirFiveElement_;
		}
		set
		{
			superAirFiveElement_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PerceptionFiveElement
	{
		get
		{
			return perceptionFiveElement_;
		}
		set
		{
			perceptionFiveElement_ = value;
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
	public RepeatedField<Types.BattleSkillResp_1010003> Skills => skills_;

	[DebuggerNonUserCode]
	public double GoldDamageAvoidance
	{
		get
		{
			return goldDamageAvoidance_;
		}
		set
		{
			goldDamageAvoidance_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double WoodDamageAvoidance
	{
		get
		{
			return woodDamageAvoidance_;
		}
		set
		{
			woodDamageAvoidance_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double WaterDamageAvoidance
	{
		get
		{
			return waterDamageAvoidance_;
		}
		set
		{
			waterDamageAvoidance_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double FireDamageAvoidance
	{
		get
		{
			return fireDamageAvoidance_;
		}
		set
		{
			fireDamageAvoidance_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double EarthDamageAvoidance
	{
		get
		{
			return earthDamageAvoidance_;
		}
		set
		{
			earthDamageAvoidance_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double AttackDistance
	{
		get
		{
			return attackDistance_;
		}
		set
		{
			attackDistance_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RoleJoinDungeonResp_1010002()
	{
	}

	[DebuggerNonUserCode]
	public RoleJoinDungeonResp_1010002(RoleJoinDungeonResp_1010002 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		dungeonId_ = other.dungeonId_;
		roleId_ = other.roleId_;
		name_ = other.name_;
		currentPhysiqueDefense_ = other.currentPhysiqueDefense_;
		currentPureBloodDefense_ = other.currentPureBloodDefense_;
		currentSuperAir_ = other.currentSuperAir_;
		currentPerception_ = other.currentPerception_;
		currentMoveSpeed_ = other.currentMoveSpeed_;
		level_ = other.level_;
		physiqueFiveElement_ = other.physiqueFiveElement_;
		superAirFiveElement_ = other.superAirFiveElement_;
		perceptionFiveElement_ = other.perceptionFiveElement_;
		icon_ = other.icon_;
		superAirRecoverValue_ = other.superAirRecoverValue_;
		defenseRecoverValue_ = other.defenseRecoverValue_;
		superAirRecoverValueOnBattle_ = other.superAirRecoverValueOnBattle_;
		defenseRecoverValueOnBattle_ = other.defenseRecoverValueOnBattle_;
		skills_ = other.skills_.Clone();
		goldDamageAvoidance_ = other.goldDamageAvoidance_;
		woodDamageAvoidance_ = other.woodDamageAvoidance_;
		waterDamageAvoidance_ = other.waterDamageAvoidance_;
		fireDamageAvoidance_ = other.fireDamageAvoidance_;
		earthDamageAvoidance_ = other.earthDamageAvoidance_;
		attackDistance_ = other.attackDistance_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoleJoinDungeonResp_1010002 Clone()
	{
		return new RoleJoinDungeonResp_1010002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoleJoinDungeonResp_1010002);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoleJoinDungeonResp_1010002 other)
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
		if (DungeonId != other.DungeonId)
		{
			return false;
		}
		if (RoleId != other.RoleId)
		{
			return false;
		}
		if (Name != other.Name)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(CurrentMoveSpeed, other.CurrentMoveSpeed))
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (PhysiqueFiveElement != other.PhysiqueFiveElement)
		{
			return false;
		}
		if (SuperAirFiveElement != other.SuperAirFiveElement)
		{
			return false;
		}
		if (PerceptionFiveElement != other.PerceptionFiveElement)
		{
			return false;
		}
		if (Icon != other.Icon)
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
		if (!skills_.Equals(other.skills_))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(GoldDamageAvoidance, other.GoldDamageAvoidance))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(WoodDamageAvoidance, other.WoodDamageAvoidance))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(WaterDamageAvoidance, other.WaterDamageAvoidance))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(FireDamageAvoidance, other.FireDamageAvoidance))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(EarthDamageAvoidance, other.EarthDamageAvoidance))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(AttackDistance, other.AttackDistance))
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
		if (DungeonId.Length != 0)
		{
			num ^= DungeonId.GetHashCode();
		}
		if (RoleId.Length != 0)
		{
			num ^= RoleId.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
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
		if (CurrentMoveSpeed != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(CurrentMoveSpeed);
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (PhysiqueFiveElement != 0)
		{
			num ^= PhysiqueFiveElement.GetHashCode();
		}
		if (SuperAirFiveElement != 0)
		{
			num ^= SuperAirFiveElement.GetHashCode();
		}
		if (PerceptionFiveElement != 0)
		{
			num ^= PerceptionFiveElement.GetHashCode();
		}
		if (Icon != 0)
		{
			num ^= Icon.GetHashCode();
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
		num ^= skills_.GetHashCode();
		if (GoldDamageAvoidance != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(GoldDamageAvoidance);
		}
		if (WoodDamageAvoidance != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(WoodDamageAvoidance);
		}
		if (WaterDamageAvoidance != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(WaterDamageAvoidance);
		}
		if (FireDamageAvoidance != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(FireDamageAvoidance);
		}
		if (EarthDamageAvoidance != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(EarthDamageAvoidance);
		}
		if (AttackDistance != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(AttackDistance);
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
		if (DungeonId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(DungeonId);
		}
		if (RoleId.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(RoleId);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(Name);
		}
		if (CurrentPhysiqueDefense != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(CurrentPhysiqueDefense);
		}
		if (CurrentPureBloodDefense != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(CurrentPureBloodDefense);
		}
		if (CurrentSuperAir != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(CurrentSuperAir);
		}
		if (CurrentPerception != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(CurrentPerception);
		}
		if (CurrentMoveSpeed != 0.0)
		{
			output.WriteRawTag(81);
			output.WriteDouble(CurrentMoveSpeed);
		}
		if (Level != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(Level);
		}
		if (PhysiqueFiveElement != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(PhysiqueFiveElement);
		}
		if (SuperAirFiveElement != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(SuperAirFiveElement);
		}
		if (PerceptionFiveElement != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(PerceptionFiveElement);
		}
		if (Icon != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(Icon);
		}
		if (SuperAirRecoverValue != 0.0)
		{
			output.WriteRawTag(129, 1);
			output.WriteDouble(SuperAirRecoverValue);
		}
		if (DefenseRecoverValue != 0.0)
		{
			output.WriteRawTag(137, 1);
			output.WriteDouble(DefenseRecoverValue);
		}
		if (SuperAirRecoverValueOnBattle != 0.0)
		{
			output.WriteRawTag(145, 1);
			output.WriteDouble(SuperAirRecoverValueOnBattle);
		}
		if (DefenseRecoverValueOnBattle != 0.0)
		{
			output.WriteRawTag(153, 1);
			output.WriteDouble(DefenseRecoverValueOnBattle);
		}
		skills_.WriteTo(output, _repeated_skills_codec);
		if (GoldDamageAvoidance != 0.0)
		{
			output.WriteRawTag(169, 1);
			output.WriteDouble(GoldDamageAvoidance);
		}
		if (WoodDamageAvoidance != 0.0)
		{
			output.WriteRawTag(177, 1);
			output.WriteDouble(WoodDamageAvoidance);
		}
		if (WaterDamageAvoidance != 0.0)
		{
			output.WriteRawTag(185, 1);
			output.WriteDouble(WaterDamageAvoidance);
		}
		if (FireDamageAvoidance != 0.0)
		{
			output.WriteRawTag(193, 1);
			output.WriteDouble(FireDamageAvoidance);
		}
		if (EarthDamageAvoidance != 0.0)
		{
			output.WriteRawTag(201, 1);
			output.WriteDouble(EarthDamageAvoidance);
		}
		if (AttackDistance != 0.0)
		{
			output.WriteRawTag(209, 1);
			output.WriteDouble(AttackDistance);
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
		if (DungeonId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(DungeonId);
		}
		if (RoleId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RoleId);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
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
		if (CurrentMoveSpeed != 0.0)
		{
			num += 9;
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (PhysiqueFiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PhysiqueFiveElement);
		}
		if (SuperAirFiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SuperAirFiveElement);
		}
		if (PerceptionFiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PerceptionFiveElement);
		}
		if (Icon != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Icon);
		}
		if (SuperAirRecoverValue != 0.0)
		{
			num += 10;
		}
		if (DefenseRecoverValue != 0.0)
		{
			num += 10;
		}
		if (SuperAirRecoverValueOnBattle != 0.0)
		{
			num += 10;
		}
		if (DefenseRecoverValueOnBattle != 0.0)
		{
			num += 10;
		}
		num += skills_.CalculateSize(_repeated_skills_codec);
		if (GoldDamageAvoidance != 0.0)
		{
			num += 10;
		}
		if (WoodDamageAvoidance != 0.0)
		{
			num += 10;
		}
		if (WaterDamageAvoidance != 0.0)
		{
			num += 10;
		}
		if (FireDamageAvoidance != 0.0)
		{
			num += 10;
		}
		if (EarthDamageAvoidance != 0.0)
		{
			num += 10;
		}
		if (AttackDistance != 0.0)
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
	public void MergeFrom(RoleJoinDungeonResp_1010002 other)
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
			if (other.DungeonId.Length != 0)
			{
				DungeonId = other.DungeonId;
			}
			if (other.RoleId.Length != 0)
			{
				RoleId = other.RoleId;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
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
			if (other.CurrentMoveSpeed != 0.0)
			{
				CurrentMoveSpeed = other.CurrentMoveSpeed;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.PhysiqueFiveElement != 0)
			{
				PhysiqueFiveElement = other.PhysiqueFiveElement;
			}
			if (other.SuperAirFiveElement != 0)
			{
				SuperAirFiveElement = other.SuperAirFiveElement;
			}
			if (other.PerceptionFiveElement != 0)
			{
				PerceptionFiveElement = other.PerceptionFiveElement;
			}
			if (other.Icon != 0)
			{
				Icon = other.Icon;
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
			skills_.Add(other.skills_);
			if (other.GoldDamageAvoidance != 0.0)
			{
				GoldDamageAvoidance = other.GoldDamageAvoidance;
			}
			if (other.WoodDamageAvoidance != 0.0)
			{
				WoodDamageAvoidance = other.WoodDamageAvoidance;
			}
			if (other.WaterDamageAvoidance != 0.0)
			{
				WaterDamageAvoidance = other.WaterDamageAvoidance;
			}
			if (other.FireDamageAvoidance != 0.0)
			{
				FireDamageAvoidance = other.FireDamageAvoidance;
			}
			if (other.EarthDamageAvoidance != 0.0)
			{
				EarthDamageAvoidance = other.EarthDamageAvoidance;
			}
			if (other.AttackDistance != 0.0)
			{
				AttackDistance = other.AttackDistance;
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
				DungeonId = input.ReadString();
				break;
			case 34u:
				RoleId = input.ReadString();
				break;
			case 42u:
				Name = input.ReadString();
				break;
			case 48u:
				CurrentPhysiqueDefense = input.ReadInt64();
				break;
			case 56u:
				CurrentPureBloodDefense = input.ReadInt32();
				break;
			case 64u:
				CurrentSuperAir = input.ReadInt64();
				break;
			case 72u:
				CurrentPerception = input.ReadInt32();
				break;
			case 81u:
				CurrentMoveSpeed = input.ReadDouble();
				break;
			case 88u:
				Level = input.ReadInt32();
				break;
			case 96u:
				PhysiqueFiveElement = input.ReadInt32();
				break;
			case 104u:
				SuperAirFiveElement = input.ReadInt32();
				break;
			case 112u:
				PerceptionFiveElement = input.ReadInt32();
				break;
			case 120u:
				Icon = input.ReadInt32();
				break;
			case 129u:
				SuperAirRecoverValue = input.ReadDouble();
				break;
			case 137u:
				DefenseRecoverValue = input.ReadDouble();
				break;
			case 145u:
				SuperAirRecoverValueOnBattle = input.ReadDouble();
				break;
			case 153u:
				DefenseRecoverValueOnBattle = input.ReadDouble();
				break;
			case 162u:
				skills_.AddEntriesFrom(input, _repeated_skills_codec);
				break;
			case 169u:
				GoldDamageAvoidance = input.ReadDouble();
				break;
			case 177u:
				WoodDamageAvoidance = input.ReadDouble();
				break;
			case 185u:
				WaterDamageAvoidance = input.ReadDouble();
				break;
			case 193u:
				FireDamageAvoidance = input.ReadDouble();
				break;
			case 201u:
				EarthDamageAvoidance = input.ReadDouble();
				break;
			case 209u:
				AttackDistance = input.ReadDouble();
				break;
			}
		}
	}
}
