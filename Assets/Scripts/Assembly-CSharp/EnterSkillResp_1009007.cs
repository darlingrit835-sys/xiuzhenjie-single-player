using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterSkillResp_1009007 : IMessage<EnterSkillResp_1009007>, IMessage, IEquatable<EnterSkillResp_1009007>, IDeepCloneable<EnterSkillResp_1009007>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class SkillInfoResp_1009008 : IMessage<SkillInfoResp_1009008>, IMessage, IEquatable<SkillInfoResp_1009008>, IDeepCloneable<SkillInfoResp_1009008>
		{
			private static readonly MessageParser<SkillInfoResp_1009008> _parser = new MessageParser<SkillInfoResp_1009008>(() => new SkillInfoResp_1009008());

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

			public const int SettingTypeFieldNumber = 7;

			private int settingType_;

			public const int CombatSettingIdFieldNumber = 8;

			private int combatSettingId_;

			[DebuggerNonUserCode]
			public static MessageParser<SkillInfoResp_1009008> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterSkillResp_1009007.Descriptor.NestedTypes[0];

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
			public int SettingType
			{
				get
				{
					return settingType_;
				}
				set
				{
					settingType_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int CombatSettingId
			{
				get
				{
					return combatSettingId_;
				}
				set
				{
					combatSettingId_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public SkillInfoResp_1009008()
			{
			}

			[DebuggerNonUserCode]
			public SkillInfoResp_1009008(SkillInfoResp_1009008 other)
				: this()
			{
				goodsId_ = other.goodsId_;
				name_ = other.name_;
				fiveElement_ = other.fiveElement_;
				rank_ = other.rank_;
				level_ = other.level_;
				type_ = other.type_;
				settingType_ = other.settingType_;
				combatSettingId_ = other.combatSettingId_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public SkillInfoResp_1009008 Clone()
			{
				return new SkillInfoResp_1009008(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as SkillInfoResp_1009008);
			}

			[DebuggerNonUserCode]
			public bool Equals(SkillInfoResp_1009008 other)
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
				if (SettingType != other.SettingType)
				{
					return false;
				}
				if (CombatSettingId != other.CombatSettingId)
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
				if (SettingType != 0)
				{
					num ^= SettingType.GetHashCode();
				}
				if (CombatSettingId != 0)
				{
					num ^= CombatSettingId.GetHashCode();
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
				if (SettingType != 0)
				{
					output.WriteRawTag(56);
					output.WriteInt32(SettingType);
				}
				if (CombatSettingId != 0)
				{
					output.WriteRawTag(64);
					output.WriteInt32(CombatSettingId);
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
				if (SettingType != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(SettingType);
				}
				if (CombatSettingId != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(CombatSettingId);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(SkillInfoResp_1009008 other)
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
					if (other.SettingType != 0)
					{
						SettingType = other.SettingType;
					}
					if (other.CombatSettingId != 0)
					{
						CombatSettingId = other.CombatSettingId;
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
					case 56u:
						SettingType = input.ReadInt32();
						break;
					case 64u:
						CombatSettingId = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterSkillResp_1009007> _parser = new MessageParser<EnterSkillResp_1009007>(() => new EnterSkillResp_1009007());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int SetableSkillsFieldNumber = 3;

	private static readonly FieldCodec<Types.SkillInfoResp_1009008> _repeated_setableSkills_codec = FieldCodec.ForMessage(26u, Types.SkillInfoResp_1009008.Parser);

	private readonly RepeatedField<Types.SkillInfoResp_1009008> setableSkills_ = new RepeatedField<Types.SkillInfoResp_1009008>();

	public const int CanuseSkillsFieldNumber = 4;

	private static readonly FieldCodec<Types.SkillInfoResp_1009008> _repeated_canuseSkills_codec = FieldCodec.ForMessage(34u, Types.SkillInfoResp_1009008.Parser);

	private readonly RepeatedField<Types.SkillInfoResp_1009008> canuseSkills_ = new RepeatedField<Types.SkillInfoResp_1009008>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterSkillResp_1009007> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleSettingProtoReflection.Descriptor.MessageTypes[9];

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
	public RepeatedField<Types.SkillInfoResp_1009008> SetableSkills => setableSkills_;

	[DebuggerNonUserCode]
	public RepeatedField<Types.SkillInfoResp_1009008> CanuseSkills => canuseSkills_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterSkillResp_1009007()
	{
	}

	[DebuggerNonUserCode]
	public EnterSkillResp_1009007(EnterSkillResp_1009007 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		setableSkills_ = other.setableSkills_.Clone();
		canuseSkills_ = other.canuseSkills_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterSkillResp_1009007 Clone()
	{
		return new EnterSkillResp_1009007(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterSkillResp_1009007);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterSkillResp_1009007 other)
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
		if (!setableSkills_.Equals(other.setableSkills_))
		{
			return false;
		}
		if (!canuseSkills_.Equals(other.canuseSkills_))
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
		num ^= setableSkills_.GetHashCode();
		num ^= canuseSkills_.GetHashCode();
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
		setableSkills_.WriteTo(output, _repeated_setableSkills_codec);
		canuseSkills_.WriteTo(output, _repeated_canuseSkills_codec);
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
		num += setableSkills_.CalculateSize(_repeated_setableSkills_codec);
		num += canuseSkills_.CalculateSize(_repeated_canuseSkills_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterSkillResp_1009007 other)
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
			setableSkills_.Add(other.setableSkills_);
			canuseSkills_.Add(other.canuseSkills_);
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
				setableSkills_.AddEntriesFrom(input, _repeated_setableSkills_codec);
				break;
			case 34u:
				canuseSkills_.AddEntriesFrom(input, _repeated_canuseSkills_codec);
				break;
			}
		}
	}
}
