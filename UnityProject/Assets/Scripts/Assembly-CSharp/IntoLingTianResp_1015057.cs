using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class IntoLingTianResp_1015057 : IMessage<IntoLingTianResp_1015057>, IMessage, IEquatable<IntoLingTianResp_1015057>, IDeepCloneable<IntoLingTianResp_1015057>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ZhangLaoInfoResp_1015058 : IMessage<ZhangLaoInfoResp_1015058>, IMessage, IEquatable<ZhangLaoInfoResp_1015058>, IDeepCloneable<ZhangLaoInfoResp_1015058>
		{
			private static readonly MessageParser<ZhangLaoInfoResp_1015058> _parser = new MessageParser<ZhangLaoInfoResp_1015058>(() => new ZhangLaoInfoResp_1015058());

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
			public static MessageParser<ZhangLaoInfoResp_1015058> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => IntoLingTianResp_1015057.Descriptor.NestedTypes[0];

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
			public ZhangLaoInfoResp_1015058()
			{
			}

			[DebuggerNonUserCode]
			public ZhangLaoInfoResp_1015058(ZhangLaoInfoResp_1015058 other)
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
			public ZhangLaoInfoResp_1015058 Clone()
			{
				return new ZhangLaoInfoResp_1015058(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ZhangLaoInfoResp_1015058);
			}

			[DebuggerNonUserCode]
			public bool Equals(ZhangLaoInfoResp_1015058 other)
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
			public void MergeFrom(ZhangLaoInfoResp_1015058 other)
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

	private static readonly MessageParser<IntoLingTianResp_1015057> _parser = new MessageParser<IntoLingTianResp_1015057>(() => new IntoLingTianResp_1015057());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ZhangLaoInfoFieldNumber = 3;

	private Types.ZhangLaoInfoResp_1015058 zhangLaoInfo_;

	public const int KuCunFieldNumber = 4;

	private long kuCun_;

	public const int DiZiNumFieldNumber = 5;

	private int diZiNum_;

	public const int CurrentChanLiangFieldNumber = 6;

	private int currentChanLiang_;

	public const int MaxChanLiangFieldNumber = 7;

	private int maxChanLiang_;

	public const int MatrixStageFieldNumber = 8;

	private int matrixStage_;

	public const int MatrixValFieldNumber = 9;

	private int matrixVal_;

	public const int UpgradeMatrixShengYuTimeFieldNumber = 10;

	private long upgradeMatrixShengYuTime_;

	public const int UpgradeSpaceShengYuTimeFieldNumber = 11;

	private long upgradeSpaceShengYuTime_;

	public const int UpgradeHillShengYuTimeFieldNumber = 12;

	private long upgradeHillShengYuTime_;

	public const int SpaceStageFieldNumber = 13;

	private int spaceStage_;

	public const int HillStageFieldNumber = 14;

	private int hillStage_;

	public const int KeYongDiZiNumFieldNumber = 15;

	private int keYongDiZiNum_;

	[DebuggerNonUserCode]
	public static MessageParser<IntoLingTianResp_1015057> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[67];

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
	public Types.ZhangLaoInfoResp_1015058 ZhangLaoInfo
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
	public long KuCun
	{
		get
		{
			return kuCun_;
		}
		set
		{
			kuCun_ = value;
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
	public int CurrentChanLiang
	{
		get
		{
			return currentChanLiang_;
		}
		set
		{
			currentChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxChanLiang
	{
		get
		{
			return maxChanLiang_;
		}
		set
		{
			maxChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MatrixStage
	{
		get
		{
			return matrixStage_;
		}
		set
		{
			matrixStage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MatrixVal
	{
		get
		{
			return matrixVal_;
		}
		set
		{
			matrixVal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long UpgradeMatrixShengYuTime
	{
		get
		{
			return upgradeMatrixShengYuTime_;
		}
		set
		{
			upgradeMatrixShengYuTime_ = value;
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
	public IntoLingTianResp_1015057()
	{
	}

	[DebuggerNonUserCode]
	public IntoLingTianResp_1015057(IntoLingTianResp_1015057 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		zhangLaoInfo_ = ((other.zhangLaoInfo_ != null) ? other.zhangLaoInfo_.Clone() : null);
		kuCun_ = other.kuCun_;
		diZiNum_ = other.diZiNum_;
		currentChanLiang_ = other.currentChanLiang_;
		maxChanLiang_ = other.maxChanLiang_;
		matrixStage_ = other.matrixStage_;
		matrixVal_ = other.matrixVal_;
		upgradeMatrixShengYuTime_ = other.upgradeMatrixShengYuTime_;
		upgradeSpaceShengYuTime_ = other.upgradeSpaceShengYuTime_;
		upgradeHillShengYuTime_ = other.upgradeHillShengYuTime_;
		spaceStage_ = other.spaceStage_;
		hillStage_ = other.hillStage_;
		keYongDiZiNum_ = other.keYongDiZiNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IntoLingTianResp_1015057 Clone()
	{
		return new IntoLingTianResp_1015057(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IntoLingTianResp_1015057);
	}

	[DebuggerNonUserCode]
	public bool Equals(IntoLingTianResp_1015057 other)
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
		if (KuCun != other.KuCun)
		{
			return false;
		}
		if (DiZiNum != other.DiZiNum)
		{
			return false;
		}
		if (CurrentChanLiang != other.CurrentChanLiang)
		{
			return false;
		}
		if (MaxChanLiang != other.MaxChanLiang)
		{
			return false;
		}
		if (MatrixStage != other.MatrixStage)
		{
			return false;
		}
		if (MatrixVal != other.MatrixVal)
		{
			return false;
		}
		if (UpgradeMatrixShengYuTime != other.UpgradeMatrixShengYuTime)
		{
			return false;
		}
		if (UpgradeSpaceShengYuTime != other.UpgradeSpaceShengYuTime)
		{
			return false;
		}
		if (UpgradeHillShengYuTime != other.UpgradeHillShengYuTime)
		{
			return false;
		}
		if (SpaceStage != other.SpaceStage)
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
		if (KuCun != 0L)
		{
			num ^= KuCun.GetHashCode();
		}
		if (DiZiNum != 0)
		{
			num ^= DiZiNum.GetHashCode();
		}
		if (CurrentChanLiang != 0)
		{
			num ^= CurrentChanLiang.GetHashCode();
		}
		if (MaxChanLiang != 0)
		{
			num ^= MaxChanLiang.GetHashCode();
		}
		if (MatrixStage != 0)
		{
			num ^= MatrixStage.GetHashCode();
		}
		if (MatrixVal != 0)
		{
			num ^= MatrixVal.GetHashCode();
		}
		if (UpgradeMatrixShengYuTime != 0L)
		{
			num ^= UpgradeMatrixShengYuTime.GetHashCode();
		}
		if (UpgradeSpaceShengYuTime != 0L)
		{
			num ^= UpgradeSpaceShengYuTime.GetHashCode();
		}
		if (UpgradeHillShengYuTime != 0L)
		{
			num ^= UpgradeHillShengYuTime.GetHashCode();
		}
		if (SpaceStage != 0)
		{
			num ^= SpaceStage.GetHashCode();
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
		if (KuCun != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(KuCun);
		}
		if (DiZiNum != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(DiZiNum);
		}
		if (CurrentChanLiang != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(CurrentChanLiang);
		}
		if (MaxChanLiang != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(MaxChanLiang);
		}
		if (MatrixStage != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(MatrixStage);
		}
		if (MatrixVal != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(MatrixVal);
		}
		if (UpgradeMatrixShengYuTime != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(UpgradeMatrixShengYuTime);
		}
		if (UpgradeSpaceShengYuTime != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(UpgradeSpaceShengYuTime);
		}
		if (UpgradeHillShengYuTime != 0L)
		{
			output.WriteRawTag(96);
			output.WriteInt64(UpgradeHillShengYuTime);
		}
		if (SpaceStage != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(SpaceStage);
		}
		if (HillStage != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(HillStage);
		}
		if (KeYongDiZiNum != 0)
		{
			output.WriteRawTag(120);
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
		if (KuCun != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(KuCun);
		}
		if (DiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DiZiNum);
		}
		if (CurrentChanLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentChanLiang);
		}
		if (MaxChanLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxChanLiang);
		}
		if (MatrixStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MatrixStage);
		}
		if (MatrixVal != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MatrixVal);
		}
		if (UpgradeMatrixShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UpgradeMatrixShengYuTime);
		}
		if (UpgradeSpaceShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UpgradeSpaceShengYuTime);
		}
		if (UpgradeHillShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UpgradeHillShengYuTime);
		}
		if (SpaceStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SpaceStage);
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
	public void MergeFrom(IntoLingTianResp_1015057 other)
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
				ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015058();
			}
			ZhangLaoInfo.MergeFrom(other.ZhangLaoInfo);
		}
		if (other.KuCun != 0L)
		{
			KuCun = other.KuCun;
		}
		if (other.DiZiNum != 0)
		{
			DiZiNum = other.DiZiNum;
		}
		if (other.CurrentChanLiang != 0)
		{
			CurrentChanLiang = other.CurrentChanLiang;
		}
		if (other.MaxChanLiang != 0)
		{
			MaxChanLiang = other.MaxChanLiang;
		}
		if (other.MatrixStage != 0)
		{
			MatrixStage = other.MatrixStage;
		}
		if (other.MatrixVal != 0)
		{
			MatrixVal = other.MatrixVal;
		}
		if (other.UpgradeMatrixShengYuTime != 0L)
		{
			UpgradeMatrixShengYuTime = other.UpgradeMatrixShengYuTime;
		}
		if (other.UpgradeSpaceShengYuTime != 0L)
		{
			UpgradeSpaceShengYuTime = other.UpgradeSpaceShengYuTime;
		}
		if (other.UpgradeHillShengYuTime != 0L)
		{
			UpgradeHillShengYuTime = other.UpgradeHillShengYuTime;
		}
		if (other.SpaceStage != 0)
		{
			SpaceStage = other.SpaceStage;
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
					ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015058();
				}
				input.ReadMessage(ZhangLaoInfo);
				break;
			case 32u:
				KuCun = input.ReadInt64();
				break;
			case 40u:
				DiZiNum = input.ReadInt32();
				break;
			case 48u:
				CurrentChanLiang = input.ReadInt32();
				break;
			case 56u:
				MaxChanLiang = input.ReadInt32();
				break;
			case 64u:
				MatrixStage = input.ReadInt32();
				break;
			case 72u:
				MatrixVal = input.ReadInt32();
				break;
			case 80u:
				UpgradeMatrixShengYuTime = input.ReadInt64();
				break;
			case 88u:
				UpgradeSpaceShengYuTime = input.ReadInt64();
				break;
			case 96u:
				UpgradeHillShengYuTime = input.ReadInt64();
				break;
			case 104u:
				SpaceStage = input.ReadInt32();
				break;
			case 112u:
				HillStage = input.ReadInt32();
				break;
			case 120u:
				KeYongDiZiNum = input.ReadInt32();
				break;
			}
		}
	}
}
