using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class IntoHuZongMatrixResp_1015048 : IMessage<IntoHuZongMatrixResp_1015048>, IMessage, IEquatable<IntoHuZongMatrixResp_1015048>, IDeepCloneable<IntoHuZongMatrixResp_1015048>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ZhangLaoInfoResp_1015049 : IMessage<ZhangLaoInfoResp_1015049>, IMessage, IEquatable<ZhangLaoInfoResp_1015049>, IDeepCloneable<ZhangLaoInfoResp_1015049>
		{
			private static readonly MessageParser<ZhangLaoInfoResp_1015049> _parser = new MessageParser<ZhangLaoInfoResp_1015049>(() => new ZhangLaoInfoResp_1015049());

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
			public static MessageParser<ZhangLaoInfoResp_1015049> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => IntoHuZongMatrixResp_1015048.Descriptor.NestedTypes[0];

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
			public ZhangLaoInfoResp_1015049()
			{
			}

			[DebuggerNonUserCode]
			public ZhangLaoInfoResp_1015049(ZhangLaoInfoResp_1015049 other)
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
			public ZhangLaoInfoResp_1015049 Clone()
			{
				return new ZhangLaoInfoResp_1015049(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ZhangLaoInfoResp_1015049);
			}

			[DebuggerNonUserCode]
			public bool Equals(ZhangLaoInfoResp_1015049 other)
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
			public void MergeFrom(ZhangLaoInfoResp_1015049 other)
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

	private static readonly MessageParser<IntoHuZongMatrixResp_1015048> _parser = new MessageParser<IntoHuZongMatrixResp_1015048>(() => new IntoHuZongMatrixResp_1015048());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ZhangLaoInfoFieldNumber = 3;

	private Types.ZhangLaoInfoResp_1015049 zhangLaoInfo_;

	public const int StageFieldNumber = 4;

	private int stage_;

	public const int StoneXiaoHaoFieldNumber = 5;

	private long stoneXiaoHao_;

	public const int JadeXiaoHaoFieldNumber = 6;

	private long jadeXiaoHao_;

	public const int IsKaiQiFieldNumber = 7;

	private bool isKaiQi_;

	public const int FangYuLiFieldNumber = 8;

	private long fangYuLi_;

	public const int ZhenYanFieldNumber = 9;

	private int zhenYan_;

	public const int UpgradeMatrixShengYuTimeFieldNumber = 10;

	private long upgradeMatrixShengYuTime_;

	[DebuggerNonUserCode]
	public static MessageParser<IntoHuZongMatrixResp_1015048> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[63];

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
	public Types.ZhangLaoInfoResp_1015049 ZhangLaoInfo
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
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StoneXiaoHao
	{
		get
		{
			return stoneXiaoHao_;
		}
		set
		{
			stoneXiaoHao_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long JadeXiaoHao
	{
		get
		{
			return jadeXiaoHao_;
		}
		set
		{
			jadeXiaoHao_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsKaiQi
	{
		get
		{
			return isKaiQi_;
		}
		set
		{
			isKaiQi_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long FangYuLi
	{
		get
		{
			return fangYuLi_;
		}
		set
		{
			fangYuLi_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ZhenYan
	{
		get
		{
			return zhenYan_;
		}
		set
		{
			zhenYan_ = value;
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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public IntoHuZongMatrixResp_1015048()
	{
	}

	[DebuggerNonUserCode]
	public IntoHuZongMatrixResp_1015048(IntoHuZongMatrixResp_1015048 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		zhangLaoInfo_ = ((other.zhangLaoInfo_ != null) ? other.zhangLaoInfo_.Clone() : null);
		stage_ = other.stage_;
		stoneXiaoHao_ = other.stoneXiaoHao_;
		jadeXiaoHao_ = other.jadeXiaoHao_;
		isKaiQi_ = other.isKaiQi_;
		fangYuLi_ = other.fangYuLi_;
		zhenYan_ = other.zhenYan_;
		upgradeMatrixShengYuTime_ = other.upgradeMatrixShengYuTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IntoHuZongMatrixResp_1015048 Clone()
	{
		return new IntoHuZongMatrixResp_1015048(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IntoHuZongMatrixResp_1015048);
	}

	[DebuggerNonUserCode]
	public bool Equals(IntoHuZongMatrixResp_1015048 other)
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
		if (Stage != other.Stage)
		{
			return false;
		}
		if (StoneXiaoHao != other.StoneXiaoHao)
		{
			return false;
		}
		if (JadeXiaoHao != other.JadeXiaoHao)
		{
			return false;
		}
		if (IsKaiQi != other.IsKaiQi)
		{
			return false;
		}
		if (FangYuLi != other.FangYuLi)
		{
			return false;
		}
		if (ZhenYan != other.ZhenYan)
		{
			return false;
		}
		if (UpgradeMatrixShengYuTime != other.UpgradeMatrixShengYuTime)
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
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (StoneXiaoHao != 0L)
		{
			num ^= StoneXiaoHao.GetHashCode();
		}
		if (JadeXiaoHao != 0L)
		{
			num ^= JadeXiaoHao.GetHashCode();
		}
		if (IsKaiQi)
		{
			num ^= IsKaiQi.GetHashCode();
		}
		if (FangYuLi != 0L)
		{
			num ^= FangYuLi.GetHashCode();
		}
		if (ZhenYan != 0)
		{
			num ^= ZhenYan.GetHashCode();
		}
		if (UpgradeMatrixShengYuTime != 0L)
		{
			num ^= UpgradeMatrixShengYuTime.GetHashCode();
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
		if (Stage != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Stage);
		}
		if (StoneXiaoHao != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(StoneXiaoHao);
		}
		if (JadeXiaoHao != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(JadeXiaoHao);
		}
		if (IsKaiQi)
		{
			output.WriteRawTag(56);
			output.WriteBool(IsKaiQi);
		}
		if (FangYuLi != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(FangYuLi);
		}
		if (ZhenYan != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(ZhenYan);
		}
		if (UpgradeMatrixShengYuTime != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(UpgradeMatrixShengYuTime);
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
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (StoneXiaoHao != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StoneXiaoHao);
		}
		if (JadeXiaoHao != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(JadeXiaoHao);
		}
		if (IsKaiQi)
		{
			num += 2;
		}
		if (FangYuLi != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FangYuLi);
		}
		if (ZhenYan != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ZhenYan);
		}
		if (UpgradeMatrixShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UpgradeMatrixShengYuTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(IntoHuZongMatrixResp_1015048 other)
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
				ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015049();
			}
			ZhangLaoInfo.MergeFrom(other.ZhangLaoInfo);
		}
		if (other.Stage != 0)
		{
			Stage = other.Stage;
		}
		if (other.StoneXiaoHao != 0L)
		{
			StoneXiaoHao = other.StoneXiaoHao;
		}
		if (other.JadeXiaoHao != 0L)
		{
			JadeXiaoHao = other.JadeXiaoHao;
		}
		if (other.IsKaiQi)
		{
			IsKaiQi = other.IsKaiQi;
		}
		if (other.FangYuLi != 0L)
		{
			FangYuLi = other.FangYuLi;
		}
		if (other.ZhenYan != 0)
		{
			ZhenYan = other.ZhenYan;
		}
		if (other.UpgradeMatrixShengYuTime != 0L)
		{
			UpgradeMatrixShengYuTime = other.UpgradeMatrixShengYuTime;
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
					ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015049();
				}
				input.ReadMessage(ZhangLaoInfo);
				break;
			case 32u:
				Stage = input.ReadInt32();
				break;
			case 40u:
				StoneXiaoHao = input.ReadInt64();
				break;
			case 48u:
				JadeXiaoHao = input.ReadInt64();
				break;
			case 56u:
				IsKaiQi = input.ReadBool();
				break;
			case 64u:
				FangYuLi = input.ReadInt64();
				break;
			case 72u:
				ZhenYan = input.ReadInt32();
				break;
			case 80u:
				UpgradeMatrixShengYuTime = input.ReadInt64();
				break;
			}
		}
	}
}
