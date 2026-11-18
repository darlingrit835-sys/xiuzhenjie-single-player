using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterTowerNextLevelResp_8001009 : IMessage<EnterTowerNextLevelResp_8001009>, IMessage, IEquatable<EnterTowerNextLevelResp_8001009>, IDeepCloneable<EnterTowerNextLevelResp_8001009>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ObjectInfoResp_8001010 : IMessage<ObjectInfoResp_8001010>, IMessage, IEquatable<ObjectInfoResp_8001010>, IDeepCloneable<ObjectInfoResp_8001010>
		{
			[DebuggerNonUserCode]
			public static class Types
			{
				public sealed class ObjectInfoSkillResp_8001012 : IMessage<ObjectInfoSkillResp_8001012>, IMessage, IEquatable<ObjectInfoSkillResp_8001012>, IDeepCloneable<ObjectInfoSkillResp_8001012>
				{
					private static readonly MessageParser<ObjectInfoSkillResp_8001012> _parser = new MessageParser<ObjectInfoSkillResp_8001012>(() => new ObjectInfoSkillResp_8001012());

					private UnknownFieldSet _unknownFields;

					public const int IconFieldNumber = 1;

					private int icon_;

					public const int TypeFieldNumber = 2;

					private int type_;

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

					public const int LevelFieldNumber = 13;

					private int level_;

					public const int SkillTypeFieldNumber = 12;

					private int skillType_;

					public const int UsedPerceptionFieldNumber = 11;

					private int usedPerception_;

					public const int SkillIndexFieldNumber = 14;

					private int skillIndex_;

					public const int UsedSuperAirFieldNumber = 15;

					private long usedSuperAir_;

					public const int CanUseCountFieldNumber = 16;

					private int canUseCount_;

					[DebuggerNonUserCode]
					public static MessageParser<ObjectInfoSkillResp_8001012> Parser => _parser;

					[DebuggerNonUserCode]
					public static MessageDescriptor Descriptor => ObjectInfoResp_8001010.Descriptor.NestedTypes[0];

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

					private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
					{
						return Descriptor;
					}

					[DebuggerNonUserCode]
					public ObjectInfoSkillResp_8001012()
					{
					}

					[DebuggerNonUserCode]
					public ObjectInfoSkillResp_8001012(ObjectInfoSkillResp_8001012 other)
						: this()
					{
						icon_ = other.icon_;
						type_ = other.type_;
						name_ = other.name_;
						templateId_ = other.templateId_;
						castTime_ = other.castTime_;
						continueTime_ = other.continueTime_;
						coolDown_ = other.coolDown_;
						fiveElement_ = other.fiveElement_;
						conversionRate_ = other.conversionRate_;
						baseDamage_ = other.baseDamage_;
						level_ = other.level_;
						skillType_ = other.skillType_;
						usedPerception_ = other.usedPerception_;
						skillIndex_ = other.skillIndex_;
						usedSuperAir_ = other.usedSuperAir_;
						canUseCount_ = other.canUseCount_;
						_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
					}

					[DebuggerNonUserCode]
					public ObjectInfoSkillResp_8001012 Clone()
					{
						return new ObjectInfoSkillResp_8001012(this);
					}

					[DebuggerNonUserCode]
					public override bool Equals(object other)
					{
						return Equals(other as ObjectInfoSkillResp_8001012);
					}

					[DebuggerNonUserCode]
					public bool Equals(ObjectInfoSkillResp_8001012 other)
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
						if (Level != other.Level)
						{
							return false;
						}
						if (SkillType != other.SkillType)
						{
							return false;
						}
						if (UsedPerception != other.UsedPerception)
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
						if (Level != 0)
						{
							num ^= Level.GetHashCode();
						}
						if (SkillType != 0)
						{
							num ^= SkillType.GetHashCode();
						}
						if (UsedPerception != 0)
						{
							num ^= UsedPerception.GetHashCode();
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
						if (Level != 0)
						{
							num += 1 + CodedOutputStream.ComputeInt32Size(Level);
						}
						if (SkillType != 0)
						{
							num += 1 + CodedOutputStream.ComputeInt32Size(SkillType);
						}
						if (UsedPerception != 0)
						{
							num += 1 + CodedOutputStream.ComputeInt32Size(UsedPerception);
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
						if (_unknownFields != null)
						{
							num += _unknownFields.CalculateSize();
						}
						return num;
					}

					[DebuggerNonUserCode]
					public void MergeFrom(ObjectInfoSkillResp_8001012 other)
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
							if (other.Level != 0)
							{
								Level = other.Level;
							}
							if (other.SkillType != 0)
							{
								SkillType = other.SkillType;
							}
							if (other.UsedPerception != 0)
							{
								UsedPerception = other.UsedPerception;
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
							}
						}
					}
				}
			}

			private static readonly MessageParser<ObjectInfoResp_8001010> _parser = new MessageParser<ObjectInfoResp_8001010>(() => new ObjectInfoResp_8001010());

			private UnknownFieldSet _unknownFields;

			public const int ObjectIndexFieldNumber = 1;

			private int objectIndex_;

			public const int PosXFieldNumber = 2;

			private double posX_;

			public const int PosYFieldNumber = 3;

			private double posY_;

			public const int AngleFieldNumber = 4;

			private double angle_;

			public const int FiveElementFieldNumber = 5;

			private int fiveElement_;

			public const int ObjectTypeFieldNumber = 6;

			private int objectType_;

			public const int NameFieldNumber = 7;

			private string name_ = "";

			public const int LevelFieldNumber = 8;

			private int level_;

			public const int CurrentPhysiqueDefenseFieldNumber = 9;

			private long currentPhysiqueDefense_;

			public const int CurrentPureBloodDefenseFieldNumber = 10;

			private int currentPureBloodDefense_;

			public const int CurrentSuperAirFieldNumber = 11;

			private long currentSuperAir_;

			public const int CurrentPerceptionFieldNumber = 12;

			private int currentPerception_;

			public const int CurrentMoveSpeedFieldNumber = 13;

			private double currentMoveSpeed_;

			public const int IconFieldNumber = 14;

			private int icon_;

			public const int SuperAirRecoverValueFieldNumber = 15;

			private double superAirRecoverValue_;

			public const int DefenseRecoverValueFieldNumber = 16;

			private double defenseRecoverValue_;

			public const int SuperAirRecoverValueOnBattleFieldNumber = 17;

			private double superAirRecoverValueOnBattle_;

			public const int DefenseRecoverValueOnBattleFieldNumber = 18;

			private double defenseRecoverValueOnBattle_;

			public const int SuperAirPowerFieldNumber = 19;

			private double superAirPower_;

			public const int SkillsFieldNumber = 20;

			private static readonly FieldCodec<Types.ObjectInfoSkillResp_8001012> _repeated_skills_codec = FieldCodec.ForMessage(162u, Types.ObjectInfoSkillResp_8001012.Parser);

			private readonly RepeatedField<Types.ObjectInfoSkillResp_8001012> skills_ = new RepeatedField<Types.ObjectInfoSkillResp_8001012>();

			public const int AttackDistanceFieldNumber = 21;

			private double attackDistance_;

			public const int UnusualLevelFieldNumber = 22;

			private int unusualLevel_;

			[DebuggerNonUserCode]
			public static MessageParser<ObjectInfoResp_8001010> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterTowerNextLevelResp_8001009.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int ObjectIndex
			{
				get
				{
					return objectIndex_;
				}
				set
				{
					objectIndex_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double PosX
			{
				get
				{
					return posX_;
				}
				set
				{
					posX_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double PosY
			{
				get
				{
					return posY_;
				}
				set
				{
					posY_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double Angle
			{
				get
				{
					return angle_;
				}
				set
				{
					angle_ = value;
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
			public int ObjectType
			{
				get
				{
					return objectType_;
				}
				set
				{
					objectType_ = value;
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
			public RepeatedField<Types.ObjectInfoSkillResp_8001012> Skills => skills_;

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

			[DebuggerNonUserCode]
			public int UnusualLevel
			{
				get
				{
					return unusualLevel_;
				}
				set
				{
					unusualLevel_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public ObjectInfoResp_8001010()
			{
			}

			[DebuggerNonUserCode]
			public ObjectInfoResp_8001010(ObjectInfoResp_8001010 other)
				: this()
			{
				objectIndex_ = other.objectIndex_;
				posX_ = other.posX_;
				posY_ = other.posY_;
				angle_ = other.angle_;
				fiveElement_ = other.fiveElement_;
				objectType_ = other.objectType_;
				name_ = other.name_;
				level_ = other.level_;
				currentPhysiqueDefense_ = other.currentPhysiqueDefense_;
				currentPureBloodDefense_ = other.currentPureBloodDefense_;
				currentSuperAir_ = other.currentSuperAir_;
				currentPerception_ = other.currentPerception_;
				currentMoveSpeed_ = other.currentMoveSpeed_;
				icon_ = other.icon_;
				superAirRecoverValue_ = other.superAirRecoverValue_;
				defenseRecoverValue_ = other.defenseRecoverValue_;
				superAirRecoverValueOnBattle_ = other.superAirRecoverValueOnBattle_;
				defenseRecoverValueOnBattle_ = other.defenseRecoverValueOnBattle_;
				superAirPower_ = other.superAirPower_;
				skills_ = other.skills_.Clone();
				attackDistance_ = other.attackDistance_;
				unusualLevel_ = other.unusualLevel_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ObjectInfoResp_8001010 Clone()
			{
				return new ObjectInfoResp_8001010(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ObjectInfoResp_8001010);
			}

			[DebuggerNonUserCode]
			public bool Equals(ObjectInfoResp_8001010 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (ObjectIndex != other.ObjectIndex)
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PosX, other.PosX))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PosY, other.PosY))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Angle, other.Angle))
				{
					return false;
				}
				if (FiveElement != other.FiveElement)
				{
					return false;
				}
				if (ObjectType != other.ObjectType)
				{
					return false;
				}
				if (Name != other.Name)
				{
					return false;
				}
				if (Level != other.Level)
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
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(SuperAirPower, other.SuperAirPower))
				{
					return false;
				}
				if (!skills_.Equals(other.skills_))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(AttackDistance, other.AttackDistance))
				{
					return false;
				}
				if (UnusualLevel != other.UnusualLevel)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (ObjectIndex != 0)
				{
					num ^= ObjectIndex.GetHashCode();
				}
				if (PosX != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosX);
				}
				if (PosY != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosY);
				}
				if (Angle != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Angle);
				}
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
				}
				if (ObjectType != 0)
				{
					num ^= ObjectType.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
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
				if (SuperAirPower != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(SuperAirPower);
				}
				num ^= skills_.GetHashCode();
				if (AttackDistance != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(AttackDistance);
				}
				if (UnusualLevel != 0)
				{
					num ^= UnusualLevel.GetHashCode();
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
				if (ObjectIndex != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(ObjectIndex);
				}
				if (PosX != 0.0)
				{
					output.WriteRawTag(17);
					output.WriteDouble(PosX);
				}
				if (PosY != 0.0)
				{
					output.WriteRawTag(25);
					output.WriteDouble(PosY);
				}
				if (Angle != 0.0)
				{
					output.WriteRawTag(33);
					output.WriteDouble(Angle);
				}
				if (FiveElement != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(FiveElement);
				}
				if (ObjectType != 0)
				{
					output.WriteRawTag(48);
					output.WriteInt32(ObjectType);
				}
				if (Name.Length != 0)
				{
					output.WriteRawTag(58);
					output.WriteString(Name);
				}
				if (Level != 0)
				{
					output.WriteRawTag(64);
					output.WriteInt32(Level);
				}
				if (CurrentPhysiqueDefense != 0L)
				{
					output.WriteRawTag(72);
					output.WriteInt64(CurrentPhysiqueDefense);
				}
				if (CurrentPureBloodDefense != 0)
				{
					output.WriteRawTag(80);
					output.WriteInt32(CurrentPureBloodDefense);
				}
				if (CurrentSuperAir != 0L)
				{
					output.WriteRawTag(88);
					output.WriteInt64(CurrentSuperAir);
				}
				if (CurrentPerception != 0)
				{
					output.WriteRawTag(96);
					output.WriteInt32(CurrentPerception);
				}
				if (CurrentMoveSpeed != 0.0)
				{
					output.WriteRawTag(105);
					output.WriteDouble(CurrentMoveSpeed);
				}
				if (Icon != 0)
				{
					output.WriteRawTag(112);
					output.WriteInt32(Icon);
				}
				if (SuperAirRecoverValue != 0.0)
				{
					output.WriteRawTag(121);
					output.WriteDouble(SuperAirRecoverValue);
				}
				if (DefenseRecoverValue != 0.0)
				{
					output.WriteRawTag(129, 1);
					output.WriteDouble(DefenseRecoverValue);
				}
				if (SuperAirRecoverValueOnBattle != 0.0)
				{
					output.WriteRawTag(137, 1);
					output.WriteDouble(SuperAirRecoverValueOnBattle);
				}
				if (DefenseRecoverValueOnBattle != 0.0)
				{
					output.WriteRawTag(145, 1);
					output.WriteDouble(DefenseRecoverValueOnBattle);
				}
				if (SuperAirPower != 0.0)
				{
					output.WriteRawTag(153, 1);
					output.WriteDouble(SuperAirPower);
				}
				skills_.WriteTo(output, _repeated_skills_codec);
				if (AttackDistance != 0.0)
				{
					output.WriteRawTag(169, 1);
					output.WriteDouble(AttackDistance);
				}
				if (UnusualLevel != 0)
				{
					output.WriteRawTag(176, 1);
					output.WriteInt32(UnusualLevel);
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
				if (ObjectIndex != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(ObjectIndex);
				}
				if (PosX != 0.0)
				{
					num += 9;
				}
				if (PosY != 0.0)
				{
					num += 9;
				}
				if (Angle != 0.0)
				{
					num += 9;
				}
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (ObjectType != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(ObjectType);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
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
				if (Icon != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Icon);
				}
				if (SuperAirRecoverValue != 0.0)
				{
					num += 9;
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
				if (SuperAirPower != 0.0)
				{
					num += 10;
				}
				num += skills_.CalculateSize(_repeated_skills_codec);
				if (AttackDistance != 0.0)
				{
					num += 10;
				}
				if (UnusualLevel != 0)
				{
					num += 2 + CodedOutputStream.ComputeInt32Size(UnusualLevel);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(ObjectInfoResp_8001010 other)
			{
				if (other != null)
				{
					if (other.ObjectIndex != 0)
					{
						ObjectIndex = other.ObjectIndex;
					}
					if (other.PosX != 0.0)
					{
						PosX = other.PosX;
					}
					if (other.PosY != 0.0)
					{
						PosY = other.PosY;
					}
					if (other.Angle != 0.0)
					{
						Angle = other.Angle;
					}
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
					}
					if (other.ObjectType != 0)
					{
						ObjectType = other.ObjectType;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
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
					if (other.SuperAirPower != 0.0)
					{
						SuperAirPower = other.SuperAirPower;
					}
					skills_.Add(other.skills_);
					if (other.AttackDistance != 0.0)
					{
						AttackDistance = other.AttackDistance;
					}
					if (other.UnusualLevel != 0)
					{
						UnusualLevel = other.UnusualLevel;
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
						ObjectIndex = input.ReadInt32();
						break;
					case 17u:
						PosX = input.ReadDouble();
						break;
					case 25u:
						PosY = input.ReadDouble();
						break;
					case 33u:
						Angle = input.ReadDouble();
						break;
					case 40u:
						FiveElement = input.ReadInt32();
						break;
					case 48u:
						ObjectType = input.ReadInt32();
						break;
					case 58u:
						Name = input.ReadString();
						break;
					case 64u:
						Level = input.ReadInt32();
						break;
					case 72u:
						CurrentPhysiqueDefense = input.ReadInt64();
						break;
					case 80u:
						CurrentPureBloodDefense = input.ReadInt32();
						break;
					case 88u:
						CurrentSuperAir = input.ReadInt64();
						break;
					case 96u:
						CurrentPerception = input.ReadInt32();
						break;
					case 105u:
						CurrentMoveSpeed = input.ReadDouble();
						break;
					case 112u:
						Icon = input.ReadInt32();
						break;
					case 121u:
						SuperAirRecoverValue = input.ReadDouble();
						break;
					case 129u:
						DefenseRecoverValue = input.ReadDouble();
						break;
					case 137u:
						SuperAirRecoverValueOnBattle = input.ReadDouble();
						break;
					case 145u:
						DefenseRecoverValueOnBattle = input.ReadDouble();
						break;
					case 153u:
						SuperAirPower = input.ReadDouble();
						break;
					case 162u:
						skills_.AddEntriesFrom(input, _repeated_skills_codec);
						break;
					case 169u:
						AttackDistance = input.ReadDouble();
						break;
					case 176u:
						UnusualLevel = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterTowerNextLevelResp_8001009> _parser = new MessageParser<EnterTowerNextLevelResp_8001009>(() => new EnterTowerNextLevelResp_8001009());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int MapMinXFieldNumber = 3;

	private double mapMinX_;

	public const int MapMaxXFieldNumber = 4;

	private double mapMaxX_;

	public const int MapMinYFieldNumber = 5;

	private double mapMinY_;

	public const int MapMaxYFieldNumber = 6;

	private double mapMaxY_;

	public const int PosXFieldNumber = 7;

	private double posX_;

	public const int PosYFieldNumber = 8;

	private double posY_;

	public const int AngleFieldNumber = 9;

	private double angle_;

	public const int ObjectsFieldNumber = 10;

	private static readonly FieldCodec<Types.ObjectInfoResp_8001010> _repeated_objects_codec = FieldCodec.ForMessage(82u, Types.ObjectInfoResp_8001010.Parser);

	private readonly RepeatedField<Types.ObjectInfoResp_8001010> objects_ = new RepeatedField<Types.ObjectInfoResp_8001010>();

	public const int RoleIndexFieldNumber = 11;

	private int roleIndex_;

	public const int OverTimeFieldNumber = 12;

	private long overTime_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterTowerNextLevelResp_8001009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[12];

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
	public double MapMinX
	{
		get
		{
			return mapMinX_;
		}
		set
		{
			mapMinX_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double MapMaxX
	{
		get
		{
			return mapMaxX_;
		}
		set
		{
			mapMaxX_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double MapMinY
	{
		get
		{
			return mapMinY_;
		}
		set
		{
			mapMinY_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double MapMaxY
	{
		get
		{
			return mapMaxY_;
		}
		set
		{
			mapMaxY_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double PosX
	{
		get
		{
			return posX_;
		}
		set
		{
			posX_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double PosY
	{
		get
		{
			return posY_;
		}
		set
		{
			posY_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Angle
	{
		get
		{
			return angle_;
		}
		set
		{
			angle_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.ObjectInfoResp_8001010> Objects => objects_;

	[DebuggerNonUserCode]
	public int RoleIndex
	{
		get
		{
			return roleIndex_;
		}
		set
		{
			roleIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long OverTime
	{
		get
		{
			return overTime_;
		}
		set
		{
			overTime_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterTowerNextLevelResp_8001009()
	{
	}

	[DebuggerNonUserCode]
	public EnterTowerNextLevelResp_8001009(EnterTowerNextLevelResp_8001009 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		mapMinX_ = other.mapMinX_;
		mapMaxX_ = other.mapMaxX_;
		mapMinY_ = other.mapMinY_;
		mapMaxY_ = other.mapMaxY_;
		posX_ = other.posX_;
		posY_ = other.posY_;
		angle_ = other.angle_;
		objects_ = other.objects_.Clone();
		roleIndex_ = other.roleIndex_;
		overTime_ = other.overTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterTowerNextLevelResp_8001009 Clone()
	{
		return new EnterTowerNextLevelResp_8001009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterTowerNextLevelResp_8001009);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterTowerNextLevelResp_8001009 other)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(MapMinX, other.MapMinX))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(MapMaxX, other.MapMaxX))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(MapMinY, other.MapMinY))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(MapMaxY, other.MapMaxY))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PosX, other.PosX))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PosY, other.PosY))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Angle, other.Angle))
		{
			return false;
		}
		if (!objects_.Equals(other.objects_))
		{
			return false;
		}
		if (RoleIndex != other.RoleIndex)
		{
			return false;
		}
		if (OverTime != other.OverTime)
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
		if (MapMinX != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(MapMinX);
		}
		if (MapMaxX != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(MapMaxX);
		}
		if (MapMinY != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(MapMinY);
		}
		if (MapMaxY != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(MapMaxY);
		}
		if (PosX != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosX);
		}
		if (PosY != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosY);
		}
		if (Angle != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Angle);
		}
		num ^= objects_.GetHashCode();
		if (RoleIndex != 0)
		{
			num ^= RoleIndex.GetHashCode();
		}
		if (OverTime != 0L)
		{
			num ^= OverTime.GetHashCode();
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
		if (MapMinX != 0.0)
		{
			output.WriteRawTag(25);
			output.WriteDouble(MapMinX);
		}
		if (MapMaxX != 0.0)
		{
			output.WriteRawTag(33);
			output.WriteDouble(MapMaxX);
		}
		if (MapMinY != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(MapMinY);
		}
		if (MapMaxY != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(MapMaxY);
		}
		if (PosX != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(PosX);
		}
		if (PosY != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(PosY);
		}
		if (Angle != 0.0)
		{
			output.WriteRawTag(73);
			output.WriteDouble(Angle);
		}
		objects_.WriteTo(output, _repeated_objects_codec);
		if (RoleIndex != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(RoleIndex);
		}
		if (OverTime != 0L)
		{
			output.WriteRawTag(96);
			output.WriteInt64(OverTime);
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
		if (MapMinX != 0.0)
		{
			num += 9;
		}
		if (MapMaxX != 0.0)
		{
			num += 9;
		}
		if (MapMinY != 0.0)
		{
			num += 9;
		}
		if (MapMaxY != 0.0)
		{
			num += 9;
		}
		if (PosX != 0.0)
		{
			num += 9;
		}
		if (PosY != 0.0)
		{
			num += 9;
		}
		if (Angle != 0.0)
		{
			num += 9;
		}
		num += objects_.CalculateSize(_repeated_objects_codec);
		if (RoleIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RoleIndex);
		}
		if (OverTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(OverTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterTowerNextLevelResp_8001009 other)
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
			if (other.MapMinX != 0.0)
			{
				MapMinX = other.MapMinX;
			}
			if (other.MapMaxX != 0.0)
			{
				MapMaxX = other.MapMaxX;
			}
			if (other.MapMinY != 0.0)
			{
				MapMinY = other.MapMinY;
			}
			if (other.MapMaxY != 0.0)
			{
				MapMaxY = other.MapMaxY;
			}
			if (other.PosX != 0.0)
			{
				PosX = other.PosX;
			}
			if (other.PosY != 0.0)
			{
				PosY = other.PosY;
			}
			if (other.Angle != 0.0)
			{
				Angle = other.Angle;
			}
			objects_.Add(other.objects_);
			if (other.RoleIndex != 0)
			{
				RoleIndex = other.RoleIndex;
			}
			if (other.OverTime != 0L)
			{
				OverTime = other.OverTime;
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
			case 25u:
				MapMinX = input.ReadDouble();
				break;
			case 33u:
				MapMaxX = input.ReadDouble();
				break;
			case 41u:
				MapMinY = input.ReadDouble();
				break;
			case 49u:
				MapMaxY = input.ReadDouble();
				break;
			case 57u:
				PosX = input.ReadDouble();
				break;
			case 65u:
				PosY = input.ReadDouble();
				break;
			case 73u:
				Angle = input.ReadDouble();
				break;
			case 82u:
				objects_.AddEntriesFrom(input, _repeated_objects_codec);
				break;
			case 88u:
				RoleIndex = input.ReadInt32();
				break;
			case 96u:
				OverTime = input.ReadInt64();
				break;
			}
		}
	}
}
