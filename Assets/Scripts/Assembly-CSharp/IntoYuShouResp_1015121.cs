using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class IntoYuShouResp_1015121 : IMessage<IntoYuShouResp_1015121>, IMessage, IEquatable<IntoYuShouResp_1015121>, IDeepCloneable<IntoYuShouResp_1015121>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ZhangLaoInfoResp_1015122 : IMessage<ZhangLaoInfoResp_1015122>, IMessage, IEquatable<ZhangLaoInfoResp_1015122>, IDeepCloneable<ZhangLaoInfoResp_1015122>
		{
			private static readonly MessageParser<ZhangLaoInfoResp_1015122> _parser = new MessageParser<ZhangLaoInfoResp_1015122>(() => new ZhangLaoInfoResp_1015122());

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
			public static MessageParser<ZhangLaoInfoResp_1015122> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => IntoYuShouResp_1015121.Descriptor.NestedTypes[0];

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
			public ZhangLaoInfoResp_1015122()
			{
			}

			[DebuggerNonUserCode]
			public ZhangLaoInfoResp_1015122(ZhangLaoInfoResp_1015122 other)
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
			public ZhangLaoInfoResp_1015122 Clone()
			{
				return new ZhangLaoInfoResp_1015122(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ZhangLaoInfoResp_1015122);
			}

			[DebuggerNonUserCode]
			public bool Equals(ZhangLaoInfoResp_1015122 other)
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
			public void MergeFrom(ZhangLaoInfoResp_1015122 other)
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

	private static readonly MessageParser<IntoYuShouResp_1015121> _parser = new MessageParser<IntoYuShouResp_1015121>(() => new IntoYuShouResp_1015121());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ZhangLaoInfoFieldNumber = 3;

	private Types.ZhangLaoInfoResp_1015122 zhangLaoInfo_;

	public const int DiZiNumFieldNumber = 4;

	private int diZiNum_;

	public const int LingShouStageFieldNumber = 5;

	private int lingShouStage_;

	public const int SiLingDanStageFieldNumber = 6;

	private int siLingDanStage_;

	public const int UpgradeLingShouShengYuTimeFieldNumber = 7;

	private long upgradeLingShouShengYuTime_;

	public const int UpgradeSiLingDanShengYuTimeFieldNumber = 8;

	private long upgradeSiLingDanShengYuTime_;

	public const int UpgradeHillShengYuTimeFieldNumber = 9;

	private long upgradeHillShengYuTime_;

	public const int HillStageFieldNumber = 10;

	private int hillStage_;

	public const int KeYongDiZiNumFieldNumber = 11;

	private int keYongDiZiNum_;

	[DebuggerNonUserCode]
	public static MessageParser<IntoYuShouResp_1015121> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[151];

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
	public Types.ZhangLaoInfoResp_1015122 ZhangLaoInfo
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
	public int DiZiNum
	{
		get
		{
			return diZiNum_;
		}
		set
		{
			diZiNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LingShouStage
	{
		get
		{
			return lingShouStage_;
		}
		set
		{
			lingShouStage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SiLingDanStage
	{
		get
		{
			return siLingDanStage_;
		}
		set
		{
			siLingDanStage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long UpgradeLingShouShengYuTime
	{
		get
		{
			return upgradeLingShouShengYuTime_;
		}
		set
		{
			upgradeLingShouShengYuTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long UpgradeSiLingDanShengYuTime
	{
		get
		{
			return upgradeSiLingDanShengYuTime_;
		}
		set
		{
			upgradeSiLingDanShengYuTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long UpgradeHillShengYuTime
	{
		get
		{
			return upgradeHillShengYuTime_;
		}
		set
		{
			upgradeHillShengYuTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HillStage
	{
		get
		{
			return hillStage_;
		}
		set
		{
			hillStage_ = value;
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
	public IntoYuShouResp_1015121()
	{
	}

	[DebuggerNonUserCode]
	public IntoYuShouResp_1015121(IntoYuShouResp_1015121 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		zhangLaoInfo_ = ((other.zhangLaoInfo_ != null) ? other.zhangLaoInfo_.Clone() : null);
		diZiNum_ = other.diZiNum_;
		lingShouStage_ = other.lingShouStage_;
		siLingDanStage_ = other.siLingDanStage_;
		upgradeLingShouShengYuTime_ = other.upgradeLingShouShengYuTime_;
		upgradeSiLingDanShengYuTime_ = other.upgradeSiLingDanShengYuTime_;
		upgradeHillShengYuTime_ = other.upgradeHillShengYuTime_;
		hillStage_ = other.hillStage_;
		keYongDiZiNum_ = other.keYongDiZiNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IntoYuShouResp_1015121 Clone()
	{
		return new IntoYuShouResp_1015121(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IntoYuShouResp_1015121);
	}

	[DebuggerNonUserCode]
	public bool Equals(IntoYuShouResp_1015121 other)
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
		if (DiZiNum != other.DiZiNum)
		{
			return false;
		}
		if (LingShouStage != other.LingShouStage)
		{
			return false;
		}
		if (SiLingDanStage != other.SiLingDanStage)
		{
			return false;
		}
		if (UpgradeLingShouShengYuTime != other.UpgradeLingShouShengYuTime)
		{
			return false;
		}
		if (UpgradeSiLingDanShengYuTime != other.UpgradeSiLingDanShengYuTime)
		{
			return false;
		}
		if (UpgradeHillShengYuTime != other.UpgradeHillShengYuTime)
		{
			return false;
		}
		if (HillStage != other.HillStage)
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
		if (DiZiNum != 0)
		{
			num ^= DiZiNum.GetHashCode();
		}
		if (LingShouStage != 0)
		{
			num ^= LingShouStage.GetHashCode();
		}
		if (SiLingDanStage != 0)
		{
			num ^= SiLingDanStage.GetHashCode();
		}
		if (UpgradeLingShouShengYuTime != 0L)
		{
			num ^= UpgradeLingShouShengYuTime.GetHashCode();
		}
		if (UpgradeSiLingDanShengYuTime != 0L)
		{
			num ^= UpgradeSiLingDanShengYuTime.GetHashCode();
		}
		if (UpgradeHillShengYuTime != 0L)
		{
			num ^= UpgradeHillShengYuTime.GetHashCode();
		}
		if (HillStage != 0)
		{
			num ^= HillStage.GetHashCode();
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
		if (DiZiNum != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(DiZiNum);
		}
		if (LingShouStage != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(LingShouStage);
		}
		if (SiLingDanStage != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(SiLingDanStage);
		}
		if (UpgradeLingShouShengYuTime != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(UpgradeLingShouShengYuTime);
		}
		if (UpgradeSiLingDanShengYuTime != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(UpgradeSiLingDanShengYuTime);
		}
		if (UpgradeHillShengYuTime != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(UpgradeHillShengYuTime);
		}
		if (HillStage != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(HillStage);
		}
		if (KeYongDiZiNum != 0)
		{
			output.WriteRawTag(88);
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
		if (DiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DiZiNum);
		}
		if (LingShouStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LingShouStage);
		}
		if (SiLingDanStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SiLingDanStage);
		}
		if (UpgradeLingShouShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UpgradeLingShouShengYuTime);
		}
		if (UpgradeSiLingDanShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UpgradeSiLingDanShengYuTime);
		}
		if (UpgradeHillShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UpgradeHillShengYuTime);
		}
		if (HillStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HillStage);
		}
		if (KeYongDiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(KeYongDiZiNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(IntoYuShouResp_1015121 other)
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
				ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015122();
			}
			ZhangLaoInfo.MergeFrom(other.ZhangLaoInfo);
		}
		if (other.DiZiNum != 0)
		{
			DiZiNum = other.DiZiNum;
		}
		if (other.LingShouStage != 0)
		{
			LingShouStage = other.LingShouStage;
		}
		if (other.SiLingDanStage != 0)
		{
			SiLingDanStage = other.SiLingDanStage;
		}
		if (other.UpgradeLingShouShengYuTime != 0L)
		{
			UpgradeLingShouShengYuTime = other.UpgradeLingShouShengYuTime;
		}
		if (other.UpgradeSiLingDanShengYuTime != 0L)
		{
			UpgradeSiLingDanShengYuTime = other.UpgradeSiLingDanShengYuTime;
		}
		if (other.UpgradeHillShengYuTime != 0L)
		{
			UpgradeHillShengYuTime = other.UpgradeHillShengYuTime;
		}
		if (other.HillStage != 0)
		{
			HillStage = other.HillStage;
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
					ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015122();
				}
				input.ReadMessage(ZhangLaoInfo);
				break;
			case 32u:
				DiZiNum = input.ReadInt32();
				break;
			case 40u:
				LingShouStage = input.ReadInt32();
				break;
			case 48u:
				SiLingDanStage = input.ReadInt32();
				break;
			case 56u:
				UpgradeLingShouShengYuTime = input.ReadInt64();
				break;
			case 64u:
				UpgradeSiLingDanShengYuTime = input.ReadInt64();
				break;
			case 72u:
				UpgradeHillShengYuTime = input.ReadInt64();
				break;
			case 80u:
				HillStage = input.ReadInt32();
				break;
			case 88u:
				KeYongDiZiNum = input.ReadInt32();
				break;
			}
		}
	}
}
