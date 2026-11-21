using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class IntoKuangMaiResp_1015042 : IMessage<IntoKuangMaiResp_1015042>, IMessage, IEquatable<IntoKuangMaiResp_1015042>, IDeepCloneable<IntoKuangMaiResp_1015042>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ZhangLaoInfoResp_1015043 : IMessage<ZhangLaoInfoResp_1015043>, IMessage, IEquatable<ZhangLaoInfoResp_1015043>, IDeepCloneable<ZhangLaoInfoResp_1015043>
		{
			private static readonly MessageParser<ZhangLaoInfoResp_1015043> _parser = new MessageParser<ZhangLaoInfoResp_1015043>(() => new ZhangLaoInfoResp_1015043());

			private UnknownFieldSet _unknownFields;

			public const int RoleIdFieldNumber = 1;

			private string roleId_ = "";

			public const int RoleImageFieldNumber = 2;

			private string roleImage_ = "";

			public const int RoleNameFieldNumber = 3;

			private string roleName_ = "";

			public const int LevelFieldNumber = 4;

			private int level_;

			public const int SexFieldNumber = 5;

			private bool sex_;

			public const int FightingFieldNumber = 6;

			private long fighting_;

			public const int TeXiaoValFieldNumber = 7;

			private int teXiaoVal_;

			[DebuggerNonUserCode]
			public static MessageParser<ZhangLaoInfoResp_1015043> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => IntoKuangMaiResp_1015042.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
			public string RoleImage
			{
				get
				{
					return roleImage_;
				}
				set
				{
					roleImage_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public string RoleName
			{
				get
				{
					return roleName_;
				}
				set
				{
					roleName_ = ProtoPreconditions.CheckNotNull(value, "value");
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
			public bool Sex
			{
				get
				{
					return sex_;
				}
				set
				{
					sex_ = value;
				}
			}

			[DebuggerNonUserCode]
			public long Fighting
			{
				get
				{
					return fighting_;
				}
				set
				{
					fighting_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int TeXiaoVal
			{
				get
				{
					return teXiaoVal_;
				}
				set
				{
					teXiaoVal_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public ZhangLaoInfoResp_1015043()
			{
			}

			[DebuggerNonUserCode]
			public ZhangLaoInfoResp_1015043(ZhangLaoInfoResp_1015043 other)
				: this()
			{
				roleId_ = other.roleId_;
				roleImage_ = other.roleImage_;
				roleName_ = other.roleName_;
				level_ = other.level_;
				sex_ = other.sex_;
				fighting_ = other.fighting_;
				teXiaoVal_ = other.teXiaoVal_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ZhangLaoInfoResp_1015043 Clone()
			{
				return new ZhangLaoInfoResp_1015043(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ZhangLaoInfoResp_1015043);
			}

			[DebuggerNonUserCode]
			public bool Equals(ZhangLaoInfoResp_1015043 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (RoleId != other.RoleId)
				{
					return false;
				}
				if (RoleImage != other.RoleImage)
				{
					return false;
				}
				if (RoleName != other.RoleName)
				{
					return false;
				}
				if (Level != other.Level)
				{
					return false;
				}
				if (Sex != other.Sex)
				{
					return false;
				}
				if (Fighting != other.Fighting)
				{
					return false;
				}
				if (TeXiaoVal != other.TeXiaoVal)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (RoleId.Length != 0)
				{
					num ^= RoleId.GetHashCode();
				}
				if (RoleImage.Length != 0)
				{
					num ^= RoleImage.GetHashCode();
				}
				if (RoleName.Length != 0)
				{
					num ^= RoleName.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
				}
				if (Sex)
				{
					num ^= Sex.GetHashCode();
				}
				if (Fighting != 0L)
				{
					num ^= Fighting.GetHashCode();
				}
				if (TeXiaoVal != 0)
				{
					num ^= TeXiaoVal.GetHashCode();
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
				if (RoleId.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(RoleId);
				}
				if (RoleImage.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(RoleImage);
				}
				if (RoleName.Length != 0)
				{
					output.WriteRawTag(26);
					output.WriteString(RoleName);
				}
				if (Level != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Level);
				}
				if (Sex)
				{
					output.WriteRawTag(40);
					output.WriteBool(Sex);
				}
				if (Fighting != 0L)
				{
					output.WriteRawTag(48);
					output.WriteInt64(Fighting);
				}
				if (TeXiaoVal != 0)
				{
					output.WriteRawTag(56);
					output.WriteInt32(TeXiaoVal);
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
				if (RoleId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(RoleId);
				}
				if (RoleImage.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(RoleImage);
				}
				if (RoleName.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(RoleName);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (Sex)
				{
					num += 2;
				}
				if (Fighting != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(Fighting);
				}
				if (TeXiaoVal != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(TeXiaoVal);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(ZhangLaoInfoResp_1015043 other)
			{
				if (other != null)
				{
					if (other.RoleId.Length != 0)
					{
						RoleId = other.RoleId;
					}
					if (other.RoleImage.Length != 0)
					{
						RoleImage = other.RoleImage;
					}
					if (other.RoleName.Length != 0)
					{
						RoleName = other.RoleName;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
					}
					if (other.Sex)
					{
						Sex = other.Sex;
					}
					if (other.Fighting != 0L)
					{
						Fighting = other.Fighting;
					}
					if (other.TeXiaoVal != 0)
					{
						TeXiaoVal = other.TeXiaoVal;
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
						RoleId = input.ReadString();
						break;
					case 18u:
						RoleImage = input.ReadString();
						break;
					case 26u:
						RoleName = input.ReadString();
						break;
					case 32u:
						Level = input.ReadInt32();
						break;
					case 40u:
						Sex = input.ReadBool();
						break;
					case 48u:
						Fighting = input.ReadInt64();
						break;
					case 56u:
						TeXiaoVal = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<IntoKuangMaiResp_1015042> _parser = new MessageParser<IntoKuangMaiResp_1015042>(() => new IntoKuangMaiResp_1015042());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ZhangLaoInfoFieldNumber = 3;

	private Types.ZhangLaoInfoResp_1015043 zhangLaoInfo_;

	public const int StoneKuCunFieldNumber = 4;

	private long stoneKuCun_;

	public const int JadeKuCunFieldNumber = 5;

	private long jadeKuCun_;

	public const int FaQiStageFieldNumber = 6;

	private int faQiStage_;

	public const int KaiCaiRateFieldNumber = 7;

	private int kaiCaiRate_;

	public const int StoneKaiCaiDiZiNumFieldNumber = 8;

	private int stoneKaiCaiDiZiNum_;

	public const int CurrentStoneChanLiangFieldNumber = 9;

	private int currentStoneChanLiang_;

	public const int MaxStoneChanLiangFieldNumber = 10;

	private int maxStoneChanLiang_;

	public const int StoneChuLiangFieldNumber = 11;

	private long stoneChuLiang_;

	public const int JadeKaiCaiDiZiNumFieldNumber = 12;

	private int jadeKaiCaiDiZiNum_;

	public const int CurrentJadeChanLiangFieldNumber = 13;

	private int currentJadeChanLiang_;

	public const int MaxJadeChanLiangFieldNumber = 14;

	private int maxJadeChanLiang_;

	public const int JadeChuLiangFieldNumber = 15;

	private long jadeChuLiang_;

	public const int UpgradeSpaceShengYuTimeFieldNumber = 16;

	private long upgradeSpaceShengYuTime_;

	public const int UpgradeFaQiShengYuTimeFieldNumber = 17;

	private long upgradeFaQiShengYuTime_;

	public const int SpaceStageFieldNumber = 18;

	private int spaceStage_;

	public const int KeYongDiZiNumFieldNumber = 19;

	private int keYongDiZiNum_;

	[DebuggerNonUserCode]
	public static MessageParser<IntoKuangMaiResp_1015042> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[52];

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
	public Types.ZhangLaoInfoResp_1015043 ZhangLaoInfo
	{
		get
		{
			return zhangLaoInfo_;
		}
		set
		{
			zhangLaoInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StoneKuCun
	{
		get
		{
			return stoneKuCun_;
		}
		set
		{
			stoneKuCun_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long JadeKuCun
	{
		get
		{
			return jadeKuCun_;
		}
		set
		{
			jadeKuCun_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FaQiStage
	{
		get
		{
			return faQiStage_;
		}
		set
		{
			faQiStage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int KaiCaiRate
	{
		get
		{
			return kaiCaiRate_;
		}
		set
		{
			kaiCaiRate_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int StoneKaiCaiDiZiNum
	{
		get
		{
			return stoneKaiCaiDiZiNum_;
		}
		set
		{
			stoneKaiCaiDiZiNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentStoneChanLiang
	{
		get
		{
			return currentStoneChanLiang_;
		}
		set
		{
			currentStoneChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxStoneChanLiang
	{
		get
		{
			return maxStoneChanLiang_;
		}
		set
		{
			maxStoneChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StoneChuLiang
	{
		get
		{
			return stoneChuLiang_;
		}
		set
		{
			stoneChuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int JadeKaiCaiDiZiNum
	{
		get
		{
			return jadeKaiCaiDiZiNum_;
		}
		set
		{
			jadeKaiCaiDiZiNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentJadeChanLiang
	{
		get
		{
			return currentJadeChanLiang_;
		}
		set
		{
			currentJadeChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxJadeChanLiang
	{
		get
		{
			return maxJadeChanLiang_;
		}
		set
		{
			maxJadeChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long JadeChuLiang
	{
		get
		{
			return jadeChuLiang_;
		}
		set
		{
			jadeChuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long UpgradeSpaceShengYuTime
	{
		get
		{
			return upgradeSpaceShengYuTime_;
		}
		set
		{
			upgradeSpaceShengYuTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long UpgradeFaQiShengYuTime
	{
		get
		{
			return upgradeFaQiShengYuTime_;
		}
		set
		{
			upgradeFaQiShengYuTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SpaceStage
	{
		get
		{
			return spaceStage_;
		}
		set
		{
			spaceStage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int KeYongDiZiNum
	{
		get
		{
			return keYongDiZiNum_;
		}
		set
		{
			keYongDiZiNum_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public IntoKuangMaiResp_1015042()
	{
	}

	[DebuggerNonUserCode]
	public IntoKuangMaiResp_1015042(IntoKuangMaiResp_1015042 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		zhangLaoInfo_ = ((other.zhangLaoInfo_ != null) ? other.zhangLaoInfo_.Clone() : null);
		stoneKuCun_ = other.stoneKuCun_;
		jadeKuCun_ = other.jadeKuCun_;
		faQiStage_ = other.faQiStage_;
		kaiCaiRate_ = other.kaiCaiRate_;
		stoneKaiCaiDiZiNum_ = other.stoneKaiCaiDiZiNum_;
		currentStoneChanLiang_ = other.currentStoneChanLiang_;
		maxStoneChanLiang_ = other.maxStoneChanLiang_;
		stoneChuLiang_ = other.stoneChuLiang_;
		jadeKaiCaiDiZiNum_ = other.jadeKaiCaiDiZiNum_;
		currentJadeChanLiang_ = other.currentJadeChanLiang_;
		maxJadeChanLiang_ = other.maxJadeChanLiang_;
		jadeChuLiang_ = other.jadeChuLiang_;
		upgradeSpaceShengYuTime_ = other.upgradeSpaceShengYuTime_;
		upgradeFaQiShengYuTime_ = other.upgradeFaQiShengYuTime_;
		spaceStage_ = other.spaceStage_;
		keYongDiZiNum_ = other.keYongDiZiNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IntoKuangMaiResp_1015042 Clone()
	{
		return new IntoKuangMaiResp_1015042(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IntoKuangMaiResp_1015042);
	}

	[DebuggerNonUserCode]
	public bool Equals(IntoKuangMaiResp_1015042 other)
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
		if (!object.Equals(ZhangLaoInfo, other.ZhangLaoInfo))
		{
			return false;
		}
		if (StoneKuCun != other.StoneKuCun)
		{
			return false;
		}
		if (JadeKuCun != other.JadeKuCun)
		{
			return false;
		}
		if (FaQiStage != other.FaQiStage)
		{
			return false;
		}
		if (KaiCaiRate != other.KaiCaiRate)
		{
			return false;
		}
		if (StoneKaiCaiDiZiNum != other.StoneKaiCaiDiZiNum)
		{
			return false;
		}
		if (CurrentStoneChanLiang != other.CurrentStoneChanLiang)
		{
			return false;
		}
		if (MaxStoneChanLiang != other.MaxStoneChanLiang)
		{
			return false;
		}
		if (StoneChuLiang != other.StoneChuLiang)
		{
			return false;
		}
		if (JadeKaiCaiDiZiNum != other.JadeKaiCaiDiZiNum)
		{
			return false;
		}
		if (CurrentJadeChanLiang != other.CurrentJadeChanLiang)
		{
			return false;
		}
		if (MaxJadeChanLiang != other.MaxJadeChanLiang)
		{
			return false;
		}
		if (JadeChuLiang != other.JadeChuLiang)
		{
			return false;
		}
		if (UpgradeSpaceShengYuTime != other.UpgradeSpaceShengYuTime)
		{
			return false;
		}
		if (UpgradeFaQiShengYuTime != other.UpgradeFaQiShengYuTime)
		{
			return false;
		}
		if (SpaceStage != other.SpaceStage)
		{
			return false;
		}
		if (KeYongDiZiNum != other.KeYongDiZiNum)
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
		if (zhangLaoInfo_ != null)
		{
			num ^= ZhangLaoInfo.GetHashCode();
		}
		if (StoneKuCun != 0L)
		{
			num ^= StoneKuCun.GetHashCode();
		}
		if (JadeKuCun != 0L)
		{
			num ^= JadeKuCun.GetHashCode();
		}
		if (FaQiStage != 0)
		{
			num ^= FaQiStage.GetHashCode();
		}
		if (KaiCaiRate != 0)
		{
			num ^= KaiCaiRate.GetHashCode();
		}
		if (StoneKaiCaiDiZiNum != 0)
		{
			num ^= StoneKaiCaiDiZiNum.GetHashCode();
		}
		if (CurrentStoneChanLiang != 0)
		{
			num ^= CurrentStoneChanLiang.GetHashCode();
		}
		if (MaxStoneChanLiang != 0)
		{
			num ^= MaxStoneChanLiang.GetHashCode();
		}
		if (StoneChuLiang != 0L)
		{
			num ^= StoneChuLiang.GetHashCode();
		}
		if (JadeKaiCaiDiZiNum != 0)
		{
			num ^= JadeKaiCaiDiZiNum.GetHashCode();
		}
		if (CurrentJadeChanLiang != 0)
		{
			num ^= CurrentJadeChanLiang.GetHashCode();
		}
		if (MaxJadeChanLiang != 0)
		{
			num ^= MaxJadeChanLiang.GetHashCode();
		}
		if (JadeChuLiang != 0L)
		{
			num ^= JadeChuLiang.GetHashCode();
		}
		if (UpgradeSpaceShengYuTime != 0L)
		{
			num ^= UpgradeSpaceShengYuTime.GetHashCode();
		}
		if (UpgradeFaQiShengYuTime != 0L)
		{
			num ^= UpgradeFaQiShengYuTime.GetHashCode();
		}
		if (SpaceStage != 0)
		{
			num ^= SpaceStage.GetHashCode();
		}
		if (KeYongDiZiNum != 0)
		{
			num ^= KeYongDiZiNum.GetHashCode();
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
		if (zhangLaoInfo_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(ZhangLaoInfo);
		}
		if (StoneKuCun != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(StoneKuCun);
		}
		if (JadeKuCun != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(JadeKuCun);
		}
		if (FaQiStage != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(FaQiStage);
		}
		if (KaiCaiRate != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(KaiCaiRate);
		}
		if (StoneKaiCaiDiZiNum != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(StoneKaiCaiDiZiNum);
		}
		if (CurrentStoneChanLiang != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(CurrentStoneChanLiang);
		}
		if (MaxStoneChanLiang != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(MaxStoneChanLiang);
		}
		if (StoneChuLiang != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(StoneChuLiang);
		}
		if (JadeKaiCaiDiZiNum != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(JadeKaiCaiDiZiNum);
		}
		if (CurrentJadeChanLiang != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(CurrentJadeChanLiang);
		}
		if (MaxJadeChanLiang != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(MaxJadeChanLiang);
		}
		if (JadeChuLiang != 0L)
		{
			output.WriteRawTag(120);
			output.WriteInt64(JadeChuLiang);
		}
		if (UpgradeSpaceShengYuTime != 0L)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt64(UpgradeSpaceShengYuTime);
		}
		if (UpgradeFaQiShengYuTime != 0L)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt64(UpgradeFaQiShengYuTime);
		}
		if (SpaceStage != 0)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt32(SpaceStage);
		}
		if (KeYongDiZiNum != 0)
		{
			output.WriteRawTag(152, 1);
			output.WriteInt32(KeYongDiZiNum);
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
		if (zhangLaoInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ZhangLaoInfo);
		}
		if (StoneKuCun != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StoneKuCun);
		}
		if (JadeKuCun != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(JadeKuCun);
		}
		if (FaQiStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FaQiStage);
		}
		if (KaiCaiRate != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(KaiCaiRate);
		}
		if (StoneKaiCaiDiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(StoneKaiCaiDiZiNum);
		}
		if (CurrentStoneChanLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentStoneChanLiang);
		}
		if (MaxStoneChanLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxStoneChanLiang);
		}
		if (StoneChuLiang != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StoneChuLiang);
		}
		if (JadeKaiCaiDiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(JadeKaiCaiDiZiNum);
		}
		if (CurrentJadeChanLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentJadeChanLiang);
		}
		if (MaxJadeChanLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxJadeChanLiang);
		}
		if (JadeChuLiang != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(JadeChuLiang);
		}
		if (UpgradeSpaceShengYuTime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(UpgradeSpaceShengYuTime);
		}
		if (UpgradeFaQiShengYuTime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(UpgradeFaQiShengYuTime);
		}
		if (SpaceStage != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(SpaceStage);
		}
		if (KeYongDiZiNum != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(KeYongDiZiNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(IntoKuangMaiResp_1015042 other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Code != 0)
		{
			Code = other.Code;
		}
		if (other.IsSuccess)
		{
			IsSuccess = other.IsSuccess;
		}
		if (other.zhangLaoInfo_ != null)
		{
			if (zhangLaoInfo_ == null)
			{
				ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015043();
			}
			ZhangLaoInfo.MergeFrom(other.ZhangLaoInfo);
		}
		if (other.StoneKuCun != 0L)
		{
			StoneKuCun = other.StoneKuCun;
		}
		if (other.JadeKuCun != 0L)
		{
			JadeKuCun = other.JadeKuCun;
		}
		if (other.FaQiStage != 0)
		{
			FaQiStage = other.FaQiStage;
		}
		if (other.KaiCaiRate != 0)
		{
			KaiCaiRate = other.KaiCaiRate;
		}
		if (other.StoneKaiCaiDiZiNum != 0)
		{
			StoneKaiCaiDiZiNum = other.StoneKaiCaiDiZiNum;
		}
		if (other.CurrentStoneChanLiang != 0)
		{
			CurrentStoneChanLiang = other.CurrentStoneChanLiang;
		}
		if (other.MaxStoneChanLiang != 0)
		{
			MaxStoneChanLiang = other.MaxStoneChanLiang;
		}
		if (other.StoneChuLiang != 0L)
		{
			StoneChuLiang = other.StoneChuLiang;
		}
		if (other.JadeKaiCaiDiZiNum != 0)
		{
			JadeKaiCaiDiZiNum = other.JadeKaiCaiDiZiNum;
		}
		if (other.CurrentJadeChanLiang != 0)
		{
			CurrentJadeChanLiang = other.CurrentJadeChanLiang;
		}
		if (other.MaxJadeChanLiang != 0)
		{
			MaxJadeChanLiang = other.MaxJadeChanLiang;
		}
		if (other.JadeChuLiang != 0L)
		{
			JadeChuLiang = other.JadeChuLiang;
		}
		if (other.UpgradeSpaceShengYuTime != 0L)
		{
			UpgradeSpaceShengYuTime = other.UpgradeSpaceShengYuTime;
		}
		if (other.UpgradeFaQiShengYuTime != 0L)
		{
			UpgradeFaQiShengYuTime = other.UpgradeFaQiShengYuTime;
		}
		if (other.SpaceStage != 0)
		{
			SpaceStage = other.SpaceStage;
		}
		if (other.KeYongDiZiNum != 0)
		{
			KeYongDiZiNum = other.KeYongDiZiNum;
		}
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
				if (zhangLaoInfo_ == null)
				{
					ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015043();
				}
				input.ReadMessage(ZhangLaoInfo);
				break;
			case 32u:
				StoneKuCun = input.ReadInt64();
				break;
			case 40u:
				JadeKuCun = input.ReadInt64();
				break;
			case 48u:
				FaQiStage = input.ReadInt32();
				break;
			case 56u:
				KaiCaiRate = input.ReadInt32();
				break;
			case 64u:
				StoneKaiCaiDiZiNum = input.ReadInt32();
				break;
			case 72u:
				CurrentStoneChanLiang = input.ReadInt32();
				break;
			case 80u:
				MaxStoneChanLiang = input.ReadInt32();
				break;
			case 88u:
				StoneChuLiang = input.ReadInt64();
				break;
			case 96u:
				JadeKaiCaiDiZiNum = input.ReadInt32();
				break;
			case 104u:
				CurrentJadeChanLiang = input.ReadInt32();
				break;
			case 112u:
				MaxJadeChanLiang = input.ReadInt32();
				break;
			case 120u:
				JadeChuLiang = input.ReadInt64();
				break;
			case 128u:
				UpgradeSpaceShengYuTime = input.ReadInt64();
				break;
			case 136u:
				UpgradeFaQiShengYuTime = input.ReadInt64();
				break;
			case 144u:
				SpaceStage = input.ReadInt32();
				break;
			case 152u:
				KeYongDiZiNum = input.ReadInt32();
				break;
			}
		}
	}
}
