using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterBattleHomeResp_1005038 : IMessage<EnterBattleHomeResp_1005038>, IMessage, IEquatable<EnterBattleHomeResp_1005038>, IDeepCloneable<EnterBattleHomeResp_1005038>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class SkillResp_1005039 : IMessage<SkillResp_1005039>, IMessage, IEquatable<SkillResp_1005039>, IDeepCloneable<SkillResp_1005039>
		{
			private static readonly MessageParser<SkillResp_1005039> _parser = new MessageParser<SkillResp_1005039>(() => new SkillResp_1005039());

			public int ForceEffort { get; set; }
			public int PracticeAffix { get; set; }
			public int PendingAffix { get; set; }
			public int PracticeRevision { get; set; }
			public int BaseCastMS { get; set; }

			private UnknownFieldSet _unknownFields;

			public const int SkillIdFieldNumber = 1;

			private string skillId_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int ProficiencyFieldNumber = 3;

			private int proficiency_;

			public const int MaxProficiencyFieldNumber = 4;

			private int maxProficiency_;

			public const int LevelFieldNumber = 5;

			private int level_;

			public const int RankFieldNumber = 6;

			private int rank_;

			public const int FiveElementFieldNumber = 7;

			private int fiveElement_;

			public const int ProficiencyLevelFieldNumber = 8;

			private int proficiencyLevel_;

			public const int TypeFieldNumber = 9;

			private int type_;

			[DebuggerNonUserCode]
			public static MessageParser<SkillResp_1005039> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterBattleHomeResp_1005038.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public string SkillId
			{
				get
				{
					return skillId_;
				}
				set
				{
					skillId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
			public int Proficiency
			{
				get
				{
					return proficiency_;
				}
				set
				{
					proficiency_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int MaxProficiency
			{
				get
				{
					return maxProficiency_;
				}
				set
				{
					maxProficiency_ = value;
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
			public int ProficiencyLevel
			{
				get
				{
					return proficiencyLevel_;
				}
				set
				{
					proficiencyLevel_ = value;
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

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public SkillResp_1005039()
			{
			}

			[DebuggerNonUserCode]
			public SkillResp_1005039(SkillResp_1005039 other)
				: this()
			{
				skillId_ = other.skillId_;
				name_ = other.name_;
				proficiency_ = other.proficiency_;
				maxProficiency_ = other.maxProficiency_;
				level_ = other.level_;
				rank_ = other.rank_;
				fiveElement_ = other.fiveElement_;
				proficiencyLevel_ = other.proficiencyLevel_;
				type_ = other.type_;
				ForceEffort = other.ForceEffort;
				PracticeAffix = other.PracticeAffix;
				PendingAffix = other.PendingAffix;
				PracticeRevision = other.PracticeRevision;
				BaseCastMS = other.BaseCastMS;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public SkillResp_1005039 Clone()
			{
				return new SkillResp_1005039(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as SkillResp_1005039);
			}

			[DebuggerNonUserCode]
			public bool Equals(SkillResp_1005039 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (SkillId != other.SkillId)
				{
					return false;
				}
				if (Name != other.Name)
				{
					return false;
				}
				if (Proficiency != other.Proficiency)
				{
					return false;
				}
				if (MaxProficiency != other.MaxProficiency)
				{
					return false;
				}
				if (Level != other.Level)
				{
					return false;
				}
				if (Rank != other.Rank)
				{
					return false;
				}
				if (FiveElement != other.FiveElement)
				{
					return false;
				}
				if (ProficiencyLevel != other.ProficiencyLevel)
				{
					return false;
				}
				if (Type != other.Type)
				{
					return false;
				}
				if (ForceEffort != other.ForceEffort) return false;
				if (PracticeAffix != other.PracticeAffix) return false;
				if (PendingAffix != other.PendingAffix) return false;
				if (PracticeRevision != other.PracticeRevision) return false;
				if (BaseCastMS != other.BaseCastMS) return false;
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (SkillId.Length != 0)
				{
					num ^= SkillId.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (Proficiency != 0)
				{
					num ^= Proficiency.GetHashCode();
				}
				if (MaxProficiency != 0)
				{
					num ^= MaxProficiency.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
				}
				if (Rank != 0)
				{
					num ^= Rank.GetHashCode();
				}
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
				}
				if (ProficiencyLevel != 0)
				{
					num ^= ProficiencyLevel.GetHashCode();
				}
				if (Type != 0)
				{
					num ^= Type.GetHashCode();
				}
				if (ForceEffort != 0) num ^= ForceEffort.GetHashCode();
				if (PracticeAffix != 0) num ^= PracticeAffix.GetHashCode();
				if (PendingAffix != 0) num ^= PendingAffix.GetHashCode();
				if (PracticeRevision != 0) num ^= PracticeRevision.GetHashCode();
				if (BaseCastMS != 0) num ^= BaseCastMS.GetHashCode();
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
				if (SkillId.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(SkillId);
				}
				if (Name.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(Name);
				}
				if (Proficiency != 0)
				{
					output.WriteRawTag(24);
					output.WriteInt32(Proficiency);
				}
				if (MaxProficiency != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(MaxProficiency);
				}
				if (Level != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Level);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(48);
					output.WriteInt32(Rank);
				}
				if (FiveElement != 0)
				{
					output.WriteRawTag(56);
					output.WriteInt32(FiveElement);
				}
				if (ProficiencyLevel != 0)
				{
					output.WriteRawTag(64);
					output.WriteInt32(ProficiencyLevel);
				}
				if (Type != 0)
				{
					output.WriteRawTag(72);
					output.WriteInt32(Type);
				}
				if (ForceEffort != 0) { output.WriteTag(10, WireFormat.WireType.Varint); output.WriteInt32(ForceEffort); }
				if (PracticeAffix != 0) { output.WriteTag(11, WireFormat.WireType.Varint); output.WriteInt32(PracticeAffix); }
				if (PendingAffix != 0) { output.WriteTag(12, WireFormat.WireType.Varint); output.WriteInt32(PendingAffix); }
				if (PracticeRevision != 0) { output.WriteTag(13, WireFormat.WireType.Varint); output.WriteInt32(PracticeRevision); }
				if (BaseCastMS != 0) { output.WriteTag(14, WireFormat.WireType.Varint); output.WriteInt32(BaseCastMS); }
				if (_unknownFields != null)
				{
					_unknownFields.WriteTo(output);
				}
			}

			[DebuggerNonUserCode]
			public int CalculateSize()
			{
				int num = 0;
				if (SkillId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(SkillId);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (Proficiency != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Proficiency);
				}
				if (MaxProficiency != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(MaxProficiency);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (Rank != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
				}
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (ProficiencyLevel != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(ProficiencyLevel);
				}
				if (Type != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Type);
				}
				if (ForceEffort != 0) num += CodedOutputStream.ComputeTagSize(10) + CodedOutputStream.ComputeInt32Size(ForceEffort);
				if (PracticeAffix != 0) num += CodedOutputStream.ComputeTagSize(11) + CodedOutputStream.ComputeInt32Size(PracticeAffix);
				if (PendingAffix != 0) num += CodedOutputStream.ComputeTagSize(12) + CodedOutputStream.ComputeInt32Size(PendingAffix);
				if (PracticeRevision != 0) num += CodedOutputStream.ComputeTagSize(13) + CodedOutputStream.ComputeInt32Size(PracticeRevision);
				if (BaseCastMS != 0) num += CodedOutputStream.ComputeTagSize(14) + CodedOutputStream.ComputeInt32Size(BaseCastMS);
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(SkillResp_1005039 other)
			{
				if (other != null)
				{
					if (other.SkillId.Length != 0)
					{
						SkillId = other.SkillId;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.Proficiency != 0)
					{
						Proficiency = other.Proficiency;
					}
					if (other.MaxProficiency != 0)
					{
						MaxProficiency = other.MaxProficiency;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
					}
					if (other.Rank != 0)
					{
						Rank = other.Rank;
					}
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
					}
					if (other.ProficiencyLevel != 0)
					{
						ProficiencyLevel = other.ProficiencyLevel;
					}
					if (other.Type != 0)
					{
						Type = other.Type;
					}
					if (other.ForceEffort != 0) ForceEffort = other.ForceEffort;
					if (other.PracticeAffix != 0) PracticeAffix = other.PracticeAffix;
					if (other.PendingAffix != 0) PendingAffix = other.PendingAffix;
					if (other.PracticeRevision != 0) PracticeRevision = other.PracticeRevision;
					if (other.BaseCastMS != 0) BaseCastMS = other.BaseCastMS;
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
					case 80u: ForceEffort = input.ReadInt32(); break;
					case 88u: PracticeAffix = input.ReadInt32(); break;
					case 96u: PendingAffix = input.ReadInt32(); break;
					case 104u: PracticeRevision = input.ReadInt32(); break;
					case 112u: BaseCastMS = input.ReadInt32(); break;
					default:
						_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
						break;
					case 10u:
						SkillId = input.ReadString();
						break;
					case 18u:
						Name = input.ReadString();
						break;
					case 24u:
						Proficiency = input.ReadInt32();
						break;
					case 32u:
						MaxProficiency = input.ReadInt32();
						break;
					case 40u:
						Level = input.ReadInt32();
						break;
					case 48u:
						Rank = input.ReadInt32();
						break;
					case 56u:
						FiveElement = input.ReadInt32();
						break;
					case 64u:
						ProficiencyLevel = input.ReadInt32();
						break;
					case 72u:
						Type = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterBattleHomeResp_1005038> _parser = new MessageParser<EnterBattleHomeResp_1005038>(() => new EnterBattleHomeResp_1005038());

	public int CurrentVigor { get; set; }

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int SkillFieldNumber = 3;

	private static readonly FieldCodec<Types.SkillResp_1005039> _repeated_skill_codec = FieldCodec.ForMessage(26u, Types.SkillResp_1005039.Parser);

	private readonly RepeatedField<Types.SkillResp_1005039> skill_ = new RepeatedField<Types.SkillResp_1005039>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterBattleHomeResp_1005038> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[49];

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
	public RepeatedField<Types.SkillResp_1005039> Skill => skill_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterBattleHomeResp_1005038()
	{
	}

	[DebuggerNonUserCode]
	public EnterBattleHomeResp_1005038(EnterBattleHomeResp_1005038 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		skill_ = other.skill_.Clone();
		CurrentVigor = other.CurrentVigor;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterBattleHomeResp_1005038 Clone()
	{
		return new EnterBattleHomeResp_1005038(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterBattleHomeResp_1005038);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterBattleHomeResp_1005038 other)
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
		if (!skill_.Equals(other.skill_))
		{
			return false;
		}
		if (CurrentVigor != other.CurrentVigor) return false;
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
		num ^= skill_.GetHashCode();
		if (CurrentVigor != 0) num ^= CurrentVigor.GetHashCode();
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
		skill_.WriteTo(output, _repeated_skill_codec);
		if (CurrentVigor != 0) { output.WriteTag(4, WireFormat.WireType.Varint); output.WriteInt32(CurrentVigor); }
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
		num += skill_.CalculateSize(_repeated_skill_codec);
		if (CurrentVigor != 0) num += CodedOutputStream.ComputeTagSize(4) + CodedOutputStream.ComputeInt32Size(CurrentVigor);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterBattleHomeResp_1005038 other)
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
			skill_.Add(other.skill_);
			if (other.CurrentVigor != 0) CurrentVigor = other.CurrentVigor;
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
			case 32u: CurrentVigor = input.ReadInt32(); break;
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
				skill_.AddEntriesFrom(input, _repeated_skill_codec);
				break;
			}
		}
	}
}
