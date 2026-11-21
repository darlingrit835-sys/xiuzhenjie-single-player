using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class IntoZhiFaResp_1015026 : IMessage<IntoZhiFaResp_1015026>, IMessage, IEquatable<IntoZhiFaResp_1015026>, IDeepCloneable<IntoZhiFaResp_1015026>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ZhangLaoInfoResp_1015027 : IMessage<ZhangLaoInfoResp_1015027>, IMessage, IEquatable<ZhangLaoInfoResp_1015027>, IDeepCloneable<ZhangLaoInfoResp_1015027>
		{
			private static readonly MessageParser<ZhangLaoInfoResp_1015027> _parser = new MessageParser<ZhangLaoInfoResp_1015027>(() => new ZhangLaoInfoResp_1015027());

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
			public static MessageParser<ZhangLaoInfoResp_1015027> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => IntoZhiFaResp_1015026.Descriptor.NestedTypes[0];

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
			public ZhangLaoInfoResp_1015027()
			{
			}

			[DebuggerNonUserCode]
			public ZhangLaoInfoResp_1015027(ZhangLaoInfoResp_1015027 other)
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
			public ZhangLaoInfoResp_1015027 Clone()
			{
				return new ZhangLaoInfoResp_1015027(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ZhangLaoInfoResp_1015027);
			}

			[DebuggerNonUserCode]
			public bool Equals(ZhangLaoInfoResp_1015027 other)
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
			public void MergeFrom(ZhangLaoInfoResp_1015027 other)
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

	private static readonly MessageParser<IntoZhiFaResp_1015026> _parser = new MessageParser<IntoZhiFaResp_1015026>(() => new IntoZhiFaResp_1015026());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ZhangLaoInfoFieldNumber = 3;

	private Types.ZhangLaoInfoResp_1015027 zhangLaoInfo_;

	public const int DiZiShuLiangFieldNumber = 4;

	private int diZiShuLiang_;

	public const int DiZiZhanDouLiFieldNumber = 5;

	private long diZiZhanDouLi_;

	public const int DiZiFangYuLiFieldNumber = 6;

	private long diZiFangYuLi_;

	public const int ZhiFaDiZiShuLiangFieldNumber = 7;

	private int zhiFaDiZiShuLiang_;

	public const int ZongMenHunLuanDuFieldNumber = 8;

	private int zongMenHunLuanDu_;

	public const int SiWangLvFieldNumber = 9;

	private double siWangLv_;

	public const int ZhiFaCeLueFieldNumber = 10;

	private bool zhiFaCeLue_;

	public const int KeYongDiZiNumFieldNumber = 11;

	private int keYongDiZiNum_;

	[DebuggerNonUserCode]
	public static MessageParser<IntoZhiFaResp_1015026> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[29];

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
	public Types.ZhangLaoInfoResp_1015027 ZhangLaoInfo
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
	public int DiZiShuLiang
	{
		get
		{
			return diZiShuLiang_;
		}
		set
		{
			diZiShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long DiZiZhanDouLi
	{
		get
		{
			return diZiZhanDouLi_;
		}
		set
		{
			diZiZhanDouLi_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long DiZiFangYuLi
	{
		get
		{
			return diZiFangYuLi_;
		}
		set
		{
			diZiFangYuLi_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ZhiFaDiZiShuLiang
	{
		get
		{
			return zhiFaDiZiShuLiang_;
		}
		set
		{
			zhiFaDiZiShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ZongMenHunLuanDu
	{
		get
		{
			return zongMenHunLuanDu_;
		}
		set
		{
			zongMenHunLuanDu_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double SiWangLv
	{
		get
		{
			return siWangLv_;
		}
		set
		{
			siWangLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool ZhiFaCeLue
	{
		get
		{
			return zhiFaCeLue_;
		}
		set
		{
			zhiFaCeLue_ = value;
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
	public IntoZhiFaResp_1015026()
	{
	}

	[DebuggerNonUserCode]
	public IntoZhiFaResp_1015026(IntoZhiFaResp_1015026 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		zhangLaoInfo_ = ((other.zhangLaoInfo_ != null) ? other.zhangLaoInfo_.Clone() : null);
		diZiShuLiang_ = other.diZiShuLiang_;
		diZiZhanDouLi_ = other.diZiZhanDouLi_;
		diZiFangYuLi_ = other.diZiFangYuLi_;
		zhiFaDiZiShuLiang_ = other.zhiFaDiZiShuLiang_;
		zongMenHunLuanDu_ = other.zongMenHunLuanDu_;
		siWangLv_ = other.siWangLv_;
		zhiFaCeLue_ = other.zhiFaCeLue_;
		keYongDiZiNum_ = other.keYongDiZiNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IntoZhiFaResp_1015026 Clone()
	{
		return new IntoZhiFaResp_1015026(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IntoZhiFaResp_1015026);
	}

	[DebuggerNonUserCode]
	public bool Equals(IntoZhiFaResp_1015026 other)
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
		if (DiZiShuLiang != other.DiZiShuLiang)
		{
			return false;
		}
		if (DiZiZhanDouLi != other.DiZiZhanDouLi)
		{
			return false;
		}
		if (DiZiFangYuLi != other.DiZiFangYuLi)
		{
			return false;
		}
		if (ZhiFaDiZiShuLiang != other.ZhiFaDiZiShuLiang)
		{
			return false;
		}
		if (ZongMenHunLuanDu != other.ZongMenHunLuanDu)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(SiWangLv, other.SiWangLv))
		{
			return false;
		}
		if (ZhiFaCeLue != other.ZhiFaCeLue)
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
		if (DiZiShuLiang != 0)
		{
			num ^= DiZiShuLiang.GetHashCode();
		}
		if (DiZiZhanDouLi != 0L)
		{
			num ^= DiZiZhanDouLi.GetHashCode();
		}
		if (DiZiFangYuLi != 0L)
		{
			num ^= DiZiFangYuLi.GetHashCode();
		}
		if (ZhiFaDiZiShuLiang != 0)
		{
			num ^= ZhiFaDiZiShuLiang.GetHashCode();
		}
		if (ZongMenHunLuanDu != 0)
		{
			num ^= ZongMenHunLuanDu.GetHashCode();
		}
		if (SiWangLv != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(SiWangLv);
		}
		if (ZhiFaCeLue)
		{
			num ^= ZhiFaCeLue.GetHashCode();
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
		if (DiZiShuLiang != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(DiZiShuLiang);
		}
		if (DiZiZhanDouLi != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(DiZiZhanDouLi);
		}
		if (DiZiFangYuLi != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(DiZiFangYuLi);
		}
		if (ZhiFaDiZiShuLiang != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(ZhiFaDiZiShuLiang);
		}
		if (ZongMenHunLuanDu != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(ZongMenHunLuanDu);
		}
		if (SiWangLv != 0.0)
		{
			output.WriteRawTag(73);
			output.WriteDouble(SiWangLv);
		}
		if (ZhiFaCeLue)
		{
			output.WriteRawTag(80);
			output.WriteBool(ZhiFaCeLue);
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
		if (DiZiShuLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DiZiShuLiang);
		}
		if (DiZiZhanDouLi != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(DiZiZhanDouLi);
		}
		if (DiZiFangYuLi != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(DiZiFangYuLi);
		}
		if (ZhiFaDiZiShuLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ZhiFaDiZiShuLiang);
		}
		if (ZongMenHunLuanDu != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ZongMenHunLuanDu);
		}
		if (SiWangLv != 0.0)
		{
			num += 9;
		}
		if (ZhiFaCeLue)
		{
			num += 2;
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
	public void MergeFrom(IntoZhiFaResp_1015026 other)
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
				ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015027();
			}
			ZhangLaoInfo.MergeFrom(other.ZhangLaoInfo);
		}
		if (other.DiZiShuLiang != 0)
		{
			DiZiShuLiang = other.DiZiShuLiang;
		}
		if (other.DiZiZhanDouLi != 0L)
		{
			DiZiZhanDouLi = other.DiZiZhanDouLi;
		}
		if (other.DiZiFangYuLi != 0L)
		{
			DiZiFangYuLi = other.DiZiFangYuLi;
		}
		if (other.ZhiFaDiZiShuLiang != 0)
		{
			ZhiFaDiZiShuLiang = other.ZhiFaDiZiShuLiang;
		}
		if (other.ZongMenHunLuanDu != 0)
		{
			ZongMenHunLuanDu = other.ZongMenHunLuanDu;
		}
		if (other.SiWangLv != 0.0)
		{
			SiWangLv = other.SiWangLv;
		}
		if (other.ZhiFaCeLue)
		{
			ZhiFaCeLue = other.ZhiFaCeLue;
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
					ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015027();
				}
				input.ReadMessage(ZhangLaoInfo);
				break;
			case 32u:
				DiZiShuLiang = input.ReadInt32();
				break;
			case 40u:
				DiZiZhanDouLi = input.ReadInt64();
				break;
			case 48u:
				DiZiFangYuLi = input.ReadInt64();
				break;
			case 56u:
				ZhiFaDiZiShuLiang = input.ReadInt32();
				break;
			case 64u:
				ZongMenHunLuanDu = input.ReadInt32();
				break;
			case 73u:
				SiWangLv = input.ReadDouble();
				break;
			case 80u:
				ZhiFaCeLue = input.ReadBool();
				break;
			case 88u:
				KeYongDiZiNum = input.ReadInt32();
				break;
			}
		}
	}
}
