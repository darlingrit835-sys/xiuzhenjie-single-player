using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class IntoNeiMenResp_1015130 : IMessage<IntoNeiMenResp_1015130>, IMessage, IEquatable<IntoNeiMenResp_1015130>, IDeepCloneable<IntoNeiMenResp_1015130>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ZhangLaoInfoResp_1015131 : IMessage<ZhangLaoInfoResp_1015131>, IMessage, IEquatable<ZhangLaoInfoResp_1015131>, IDeepCloneable<ZhangLaoInfoResp_1015131>
		{
			private static readonly MessageParser<ZhangLaoInfoResp_1015131> _parser = new MessageParser<ZhangLaoInfoResp_1015131>(() => new ZhangLaoInfoResp_1015131());

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
			public static MessageParser<ZhangLaoInfoResp_1015131> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => IntoNeiMenResp_1015130.Descriptor.NestedTypes[0];

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
			public ZhangLaoInfoResp_1015131()
			{
			}

			[DebuggerNonUserCode]
			public ZhangLaoInfoResp_1015131(ZhangLaoInfoResp_1015131 other)
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
			public ZhangLaoInfoResp_1015131 Clone()
			{
				return new ZhangLaoInfoResp_1015131(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ZhangLaoInfoResp_1015131);
			}

			[DebuggerNonUserCode]
			public bool Equals(ZhangLaoInfoResp_1015131 other)
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
			public void MergeFrom(ZhangLaoInfoResp_1015131 other)
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

	private static readonly MessageParser<IntoNeiMenResp_1015130> _parser = new MessageParser<IntoNeiMenResp_1015130>(() => new IntoNeiMenResp_1015130());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ZhangLaoInfoFieldNumber = 3;

	private Types.ZhangLaoInfoResp_1015131 zhangLaoInfo_;

	public const int DiZiNumFieldNumber = 4;

	private int diZiNum_;

	public const int GongFaStageFieldNumber = 5;

	private int gongFaStage_;

	public const int FaQiStageFieldNumber = 6;

	private int faQiStage_;

	public const int FaShuStageFieldNumber = 7;

	private int faShuStage_;

	public const int PeiFaGongFaShengYuTimeFieldNumber = 9;

	private long peiFaGongFaShengYuTime_;

	public const int PeiFaFaQiDanShengYuTimeFieldNumber = 10;

	private long peiFaFaQiDanShengYuTime_;

	public const int PeiFaFaShuShengYuTimeFieldNumber = 11;

	private long peiFaFaShuShengYuTime_;

	public const int KeYongDiZiNumFieldNumber = 12;

	private int keYongDiZiNum_;

	public const int GongFaStockFieldNumber = 13;

	private int gongFaStock_;

	public const int FaQiStockFieldNumber = 14;

	private int faQiStock_;

	public const int FaShuStockFieldNumber = 15;

	private int faShuStock_;

	[DebuggerNonUserCode]
	public static MessageParser<IntoNeiMenResp_1015130> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[163];

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
	public Types.ZhangLaoInfoResp_1015131 ZhangLaoInfo
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
	public int GongFaStage
	{
		get
		{
			return gongFaStage_;
		}
		set
		{
			gongFaStage_ = value;
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
	public int FaShuStage
	{
		get
		{
			return faShuStage_;
		}
		set
		{
			faShuStage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long PeiFaGongFaShengYuTime
	{
		get
		{
			return peiFaGongFaShengYuTime_;
		}
		set
		{
			peiFaGongFaShengYuTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long PeiFaFaQiDanShengYuTime
	{
		get
		{
			return peiFaFaQiDanShengYuTime_;
		}
		set
		{
			peiFaFaQiDanShengYuTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long PeiFaFaShuShengYuTime
	{
		get
		{
			return peiFaFaShuShengYuTime_;
		}
		set
		{
			peiFaFaShuShengYuTime_ = value;
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

	[DebuggerNonUserCode]
	public int GongFaStock
	{
		get
		{
			return gongFaStock_;
		}
		set
		{
			gongFaStock_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FaQiStock
	{
		get
		{
			return faQiStock_;
		}
		set
		{
			faQiStock_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FaShuStock
	{
		get
		{
			return faShuStock_;
		}
		set
		{
			faShuStock_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public IntoNeiMenResp_1015130()
	{
	}

	[DebuggerNonUserCode]
	public IntoNeiMenResp_1015130(IntoNeiMenResp_1015130 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		zhangLaoInfo_ = ((other.zhangLaoInfo_ != null) ? other.zhangLaoInfo_.Clone() : null);
		diZiNum_ = other.diZiNum_;
		gongFaStage_ = other.gongFaStage_;
		faQiStage_ = other.faQiStage_;
		faShuStage_ = other.faShuStage_;
		peiFaGongFaShengYuTime_ = other.peiFaGongFaShengYuTime_;
		peiFaFaQiDanShengYuTime_ = other.peiFaFaQiDanShengYuTime_;
		peiFaFaShuShengYuTime_ = other.peiFaFaShuShengYuTime_;
		keYongDiZiNum_ = other.keYongDiZiNum_;
		gongFaStock_ = other.gongFaStock_;
		faQiStock_ = other.faQiStock_;
		faShuStock_ = other.faShuStock_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IntoNeiMenResp_1015130 Clone()
	{
		return new IntoNeiMenResp_1015130(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IntoNeiMenResp_1015130);
	}

	[DebuggerNonUserCode]
	public bool Equals(IntoNeiMenResp_1015130 other)
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
		if (GongFaStage != other.GongFaStage)
		{
			return false;
		}
		if (FaQiStage != other.FaQiStage)
		{
			return false;
		}
		if (FaShuStage != other.FaShuStage)
		{
			return false;
		}
		if (PeiFaGongFaShengYuTime != other.PeiFaGongFaShengYuTime)
		{
			return false;
		}
		if (PeiFaFaQiDanShengYuTime != other.PeiFaFaQiDanShengYuTime)
		{
			return false;
		}
		if (PeiFaFaShuShengYuTime != other.PeiFaFaShuShengYuTime)
		{
			return false;
		}
		if (KeYongDiZiNum != other.KeYongDiZiNum)
		{
			return false;
		}
		if (GongFaStock != other.GongFaStock)
		{
			return false;
		}
		if (FaQiStock != other.FaQiStock)
		{
			return false;
		}
		if (FaShuStock != other.FaShuStock)
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
		if (GongFaStage != 0)
		{
			num ^= GongFaStage.GetHashCode();
		}
		if (FaQiStage != 0)
		{
			num ^= FaQiStage.GetHashCode();
		}
		if (FaShuStage != 0)
		{
			num ^= FaShuStage.GetHashCode();
		}
		if (PeiFaGongFaShengYuTime != 0L)
		{
			num ^= PeiFaGongFaShengYuTime.GetHashCode();
		}
		if (PeiFaFaQiDanShengYuTime != 0L)
		{
			num ^= PeiFaFaQiDanShengYuTime.GetHashCode();
		}
		if (PeiFaFaShuShengYuTime != 0L)
		{
			num ^= PeiFaFaShuShengYuTime.GetHashCode();
		}
		if (KeYongDiZiNum != 0)
		{
			num ^= KeYongDiZiNum.GetHashCode();
		}
		if (GongFaStock != 0)
		{
			num ^= GongFaStock.GetHashCode();
		}
		if (FaQiStock != 0)
		{
			num ^= FaQiStock.GetHashCode();
		}
		if (FaShuStock != 0)
		{
			num ^= FaShuStock.GetHashCode();
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
		if (GongFaStage != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(GongFaStage);
		}
		if (FaQiStage != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(FaQiStage);
		}
		if (FaShuStage != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(FaShuStage);
		}
		if (PeiFaGongFaShengYuTime != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(PeiFaGongFaShengYuTime);
		}
		if (PeiFaFaQiDanShengYuTime != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(PeiFaFaQiDanShengYuTime);
		}
		if (PeiFaFaShuShengYuTime != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(PeiFaFaShuShengYuTime);
		}
		if (KeYongDiZiNum != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(KeYongDiZiNum);
		}
		if (GongFaStock != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(GongFaStock);
		}
		if (FaQiStock != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(FaQiStock);
		}
		if (FaShuStock != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(FaShuStock);
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
		if (GongFaStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GongFaStage);
		}
		if (FaQiStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FaQiStage);
		}
		if (FaShuStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FaShuStage);
		}
		if (PeiFaGongFaShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(PeiFaGongFaShengYuTime);
		}
		if (PeiFaFaQiDanShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(PeiFaFaQiDanShengYuTime);
		}
		if (PeiFaFaShuShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(PeiFaFaShuShengYuTime);
		}
		if (KeYongDiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(KeYongDiZiNum);
		}
		if (GongFaStock != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GongFaStock);
		}
		if (FaQiStock != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FaQiStock);
		}
		if (FaShuStock != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FaShuStock);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(IntoNeiMenResp_1015130 other)
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
				ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015131();
			}
			ZhangLaoInfo.MergeFrom(other.ZhangLaoInfo);
		}
		if (other.DiZiNum != 0)
		{
			DiZiNum = other.DiZiNum;
		}
		if (other.GongFaStage != 0)
		{
			GongFaStage = other.GongFaStage;
		}
		if (other.FaQiStage != 0)
		{
			FaQiStage = other.FaQiStage;
		}
		if (other.FaShuStage != 0)
		{
			FaShuStage = other.FaShuStage;
		}
		if (other.PeiFaGongFaShengYuTime != 0L)
		{
			PeiFaGongFaShengYuTime = other.PeiFaGongFaShengYuTime;
		}
		if (other.PeiFaFaQiDanShengYuTime != 0L)
		{
			PeiFaFaQiDanShengYuTime = other.PeiFaFaQiDanShengYuTime;
		}
		if (other.PeiFaFaShuShengYuTime != 0L)
		{
			PeiFaFaShuShengYuTime = other.PeiFaFaShuShengYuTime;
		}
		if (other.KeYongDiZiNum != 0)
		{
			KeYongDiZiNum = other.KeYongDiZiNum;
		}
		if (other.GongFaStock != 0)
		{
			GongFaStock = other.GongFaStock;
		}
		if (other.FaQiStock != 0)
		{
			FaQiStock = other.FaQiStock;
		}
		if (other.FaShuStock != 0)
		{
			FaShuStock = other.FaShuStock;
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
					ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015131();
				}
				input.ReadMessage(ZhangLaoInfo);
				break;
			case 32u:
				DiZiNum = input.ReadInt32();
				break;
			case 40u:
				GongFaStage = input.ReadInt32();
				break;
			case 48u:
				FaQiStage = input.ReadInt32();
				break;
			case 56u:
				FaShuStage = input.ReadInt32();
				break;
			case 72u:
				PeiFaGongFaShengYuTime = input.ReadInt64();
				break;
			case 80u:
				PeiFaFaQiDanShengYuTime = input.ReadInt64();
				break;
			case 88u:
				PeiFaFaShuShengYuTime = input.ReadInt64();
				break;
			case 96u:
				KeYongDiZiNum = input.ReadInt32();
				break;
			case 104u:
				GongFaStock = input.ReadInt32();
				break;
			case 112u:
				FaQiStock = input.ReadInt32();
				break;
			case 120u:
				FaShuStock = input.ReadInt32();
				break;
			}
		}
	}
}
