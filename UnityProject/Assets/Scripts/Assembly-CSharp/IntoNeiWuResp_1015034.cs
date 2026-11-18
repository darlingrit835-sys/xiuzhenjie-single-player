using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class IntoNeiWuResp_1015034 : IMessage<IntoNeiWuResp_1015034>, IMessage, IEquatable<IntoNeiWuResp_1015034>, IDeepCloneable<IntoNeiWuResp_1015034>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ZhangLaoInfoResp_1015035 : IMessage<ZhangLaoInfoResp_1015035>, IMessage, IEquatable<ZhangLaoInfoResp_1015035>, IDeepCloneable<ZhangLaoInfoResp_1015035>
		{
			private static readonly MessageParser<ZhangLaoInfoResp_1015035> _parser = new MessageParser<ZhangLaoInfoResp_1015035>(() => new ZhangLaoInfoResp_1015035());

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
			public static MessageParser<ZhangLaoInfoResp_1015035> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => IntoNeiWuResp_1015034.Descriptor.NestedTypes[0];

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
			public ZhangLaoInfoResp_1015035()
			{
			}

			[DebuggerNonUserCode]
			public ZhangLaoInfoResp_1015035(ZhangLaoInfoResp_1015035 other)
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
			public ZhangLaoInfoResp_1015035 Clone()
			{
				return new ZhangLaoInfoResp_1015035(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ZhangLaoInfoResp_1015035);
			}

			[DebuggerNonUserCode]
			public bool Equals(ZhangLaoInfoResp_1015035 other)
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
			public void MergeFrom(ZhangLaoInfoResp_1015035 other)
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

	private static readonly MessageParser<IntoNeiWuResp_1015034> _parser = new MessageParser<IntoNeiWuResp_1015034>(() => new IntoNeiWuResp_1015034());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ZhangLaoInfoFieldNumber = 3;

	private Types.ZhangLaoInfoResp_1015035 zhangLaoInfo_;

	public const int StoneKuCunFieldNumber = 4;

	private double stoneKuCun_;

	public const int StoneChanLiangFieldNumber = 5;

	private double stoneChanLiang_;

	public const int JadeKuCunFieldNumber = 6;

	private double jadeKuCun_;

	public const int JadeChanLiangFieldNumber = 7;

	private double jadeChanLiang_;

	public const int DanYaoKuCunFieldNumber = 8;

	private long danYaoKuCun_;

	public const int DanYaoChanLiangFieldNumber = 9;

	private int danYaoChanLiang_;

	public const int FaQiKuCunFieldNumber = 10;

	private long faQiKuCun_;

	public const int FaQiChanLiangFieldNumber = 11;

	private int faQiChanLiang_;

	public const int FuLuKuCunFieldNumber = 12;

	private long fuLuKuCun_;

	public const int FuLuChanLiangFieldNumber = 13;

	private int fuLuChanLiang_;

	public const int ZhenPanKuCunFieldNumber = 14;

	private long zhenPanKuCun_;

	public const int ZhenPanChanLiangFieldNumber = 15;

	private int zhenPanChanLiang_;

	public const int StoneShengYuCountFieldNumber = 16;

	private int stoneShengYuCount_;

	public const int JadeShengYuCountFieldNumber = 17;

	private int jadeShengYuCount_;

	public const int CurrentContributionFieldNumber = 18;

	private long currentContribution_;

	public const int StoneSalaryFieldNumber = 19;

	private long stoneSalary_;

	public const int JadeSalaryFieldNumber = 20;

	private int jadeSalary_;

	public const int MatrixSalaryFieldNumber = 21;

	private int matrixSalary_;

	public const int ElixirSalaryFieldNumber = 22;

	private int elixirSalary_;

	public const int EquipmentSalaryFieldNumber = 23;

	private int equipmentSalary_;

	public const int SignSalaryFieldNumber = 24;

	private int signSalary_;

	public const int YingXiangLiFieldNumber = 25;

	private int yingXiangLi_;

	public const int PreYingXiangLiFieldNumber = 26;

	private int preYingXiangLi_;

	[DebuggerNonUserCode]
	public static MessageParser<IntoNeiWuResp_1015034> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[44];

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
	public Types.ZhangLaoInfoResp_1015035 ZhangLaoInfo
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
	public double StoneKuCun
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
	public double StoneChanLiang
	{
		get
		{
			return stoneChanLiang_;
		}
		set
		{
			stoneChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double JadeKuCun
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
	public double JadeChanLiang
	{
		get
		{
			return jadeChanLiang_;
		}
		set
		{
			jadeChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long DanYaoKuCun
	{
		get
		{
			return danYaoKuCun_;
		}
		set
		{
			danYaoKuCun_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DanYaoChanLiang
	{
		get
		{
			return danYaoChanLiang_;
		}
		set
		{
			danYaoChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long FaQiKuCun
	{
		get
		{
			return faQiKuCun_;
		}
		set
		{
			faQiKuCun_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FaQiChanLiang
	{
		get
		{
			return faQiChanLiang_;
		}
		set
		{
			faQiChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long FuLuKuCun
	{
		get
		{
			return fuLuKuCun_;
		}
		set
		{
			fuLuKuCun_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FuLuChanLiang
	{
		get
		{
			return fuLuChanLiang_;
		}
		set
		{
			fuLuChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ZhenPanKuCun
	{
		get
		{
			return zhenPanKuCun_;
		}
		set
		{
			zhenPanKuCun_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ZhenPanChanLiang
	{
		get
		{
			return zhenPanChanLiang_;
		}
		set
		{
			zhenPanChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int StoneShengYuCount
	{
		get
		{
			return stoneShengYuCount_;
		}
		set
		{
			stoneShengYuCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int JadeShengYuCount
	{
		get
		{
			return jadeShengYuCount_;
		}
		set
		{
			jadeShengYuCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurrentContribution
	{
		get
		{
			return currentContribution_;
		}
		set
		{
			currentContribution_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StoneSalary
	{
		get
		{
			return stoneSalary_;
		}
		set
		{
			stoneSalary_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int JadeSalary
	{
		get
		{
			return jadeSalary_;
		}
		set
		{
			jadeSalary_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MatrixSalary
	{
		get
		{
			return matrixSalary_;
		}
		set
		{
			matrixSalary_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ElixirSalary
	{
		get
		{
			return elixirSalary_;
		}
		set
		{
			elixirSalary_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EquipmentSalary
	{
		get
		{
			return equipmentSalary_;
		}
		set
		{
			equipmentSalary_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SignSalary
	{
		get
		{
			return signSalary_;
		}
		set
		{
			signSalary_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int YingXiangLi
	{
		get
		{
			return yingXiangLi_;
		}
		set
		{
			yingXiangLi_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PreYingXiangLi
	{
		get
		{
			return preYingXiangLi_;
		}
		set
		{
			preYingXiangLi_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public IntoNeiWuResp_1015034()
	{
	}

	[DebuggerNonUserCode]
	public IntoNeiWuResp_1015034(IntoNeiWuResp_1015034 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		zhangLaoInfo_ = ((other.zhangLaoInfo_ != null) ? other.zhangLaoInfo_.Clone() : null);
		stoneKuCun_ = other.stoneKuCun_;
		stoneChanLiang_ = other.stoneChanLiang_;
		jadeKuCun_ = other.jadeKuCun_;
		jadeChanLiang_ = other.jadeChanLiang_;
		danYaoKuCun_ = other.danYaoKuCun_;
		danYaoChanLiang_ = other.danYaoChanLiang_;
		faQiKuCun_ = other.faQiKuCun_;
		faQiChanLiang_ = other.faQiChanLiang_;
		fuLuKuCun_ = other.fuLuKuCun_;
		fuLuChanLiang_ = other.fuLuChanLiang_;
		zhenPanKuCun_ = other.zhenPanKuCun_;
		zhenPanChanLiang_ = other.zhenPanChanLiang_;
		stoneShengYuCount_ = other.stoneShengYuCount_;
		jadeShengYuCount_ = other.jadeShengYuCount_;
		currentContribution_ = other.currentContribution_;
		stoneSalary_ = other.stoneSalary_;
		jadeSalary_ = other.jadeSalary_;
		matrixSalary_ = other.matrixSalary_;
		elixirSalary_ = other.elixirSalary_;
		equipmentSalary_ = other.equipmentSalary_;
		signSalary_ = other.signSalary_;
		yingXiangLi_ = other.yingXiangLi_;
		preYingXiangLi_ = other.preYingXiangLi_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IntoNeiWuResp_1015034 Clone()
	{
		return new IntoNeiWuResp_1015034(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IntoNeiWuResp_1015034);
	}

	[DebuggerNonUserCode]
	public bool Equals(IntoNeiWuResp_1015034 other)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(StoneKuCun, other.StoneKuCun))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(StoneChanLiang, other.StoneChanLiang))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(JadeKuCun, other.JadeKuCun))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(JadeChanLiang, other.JadeChanLiang))
		{
			return false;
		}
		if (DanYaoKuCun != other.DanYaoKuCun)
		{
			return false;
		}
		if (DanYaoChanLiang != other.DanYaoChanLiang)
		{
			return false;
		}
		if (FaQiKuCun != other.FaQiKuCun)
		{
			return false;
		}
		if (FaQiChanLiang != other.FaQiChanLiang)
		{
			return false;
		}
		if (FuLuKuCun != other.FuLuKuCun)
		{
			return false;
		}
		if (FuLuChanLiang != other.FuLuChanLiang)
		{
			return false;
		}
		if (ZhenPanKuCun != other.ZhenPanKuCun)
		{
			return false;
		}
		if (ZhenPanChanLiang != other.ZhenPanChanLiang)
		{
			return false;
		}
		if (StoneShengYuCount != other.StoneShengYuCount)
		{
			return false;
		}
		if (JadeShengYuCount != other.JadeShengYuCount)
		{
			return false;
		}
		if (CurrentContribution != other.CurrentContribution)
		{
			return false;
		}
		if (StoneSalary != other.StoneSalary)
		{
			return false;
		}
		if (JadeSalary != other.JadeSalary)
		{
			return false;
		}
		if (MatrixSalary != other.MatrixSalary)
		{
			return false;
		}
		if (ElixirSalary != other.ElixirSalary)
		{
			return false;
		}
		if (EquipmentSalary != other.EquipmentSalary)
		{
			return false;
		}
		if (SignSalary != other.SignSalary)
		{
			return false;
		}
		if (YingXiangLi != other.YingXiangLi)
		{
			return false;
		}
		if (PreYingXiangLi != other.PreYingXiangLi)
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
		if (StoneKuCun != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(StoneKuCun);
		}
		if (StoneChanLiang != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(StoneChanLiang);
		}
		if (JadeKuCun != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(JadeKuCun);
		}
		if (JadeChanLiang != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(JadeChanLiang);
		}
		if (DanYaoKuCun != 0L)
		{
			num ^= DanYaoKuCun.GetHashCode();
		}
		if (DanYaoChanLiang != 0)
		{
			num ^= DanYaoChanLiang.GetHashCode();
		}
		if (FaQiKuCun != 0L)
		{
			num ^= FaQiKuCun.GetHashCode();
		}
		if (FaQiChanLiang != 0)
		{
			num ^= FaQiChanLiang.GetHashCode();
		}
		if (FuLuKuCun != 0L)
		{
			num ^= FuLuKuCun.GetHashCode();
		}
		if (FuLuChanLiang != 0)
		{
			num ^= FuLuChanLiang.GetHashCode();
		}
		if (ZhenPanKuCun != 0L)
		{
			num ^= ZhenPanKuCun.GetHashCode();
		}
		if (ZhenPanChanLiang != 0)
		{
			num ^= ZhenPanChanLiang.GetHashCode();
		}
		if (StoneShengYuCount != 0)
		{
			num ^= StoneShengYuCount.GetHashCode();
		}
		if (JadeShengYuCount != 0)
		{
			num ^= JadeShengYuCount.GetHashCode();
		}
		if (CurrentContribution != 0L)
		{
			num ^= CurrentContribution.GetHashCode();
		}
		if (StoneSalary != 0L)
		{
			num ^= StoneSalary.GetHashCode();
		}
		if (JadeSalary != 0)
		{
			num ^= JadeSalary.GetHashCode();
		}
		if (MatrixSalary != 0)
		{
			num ^= MatrixSalary.GetHashCode();
		}
		if (ElixirSalary != 0)
		{
			num ^= ElixirSalary.GetHashCode();
		}
		if (EquipmentSalary != 0)
		{
			num ^= EquipmentSalary.GetHashCode();
		}
		if (SignSalary != 0)
		{
			num ^= SignSalary.GetHashCode();
		}
		if (YingXiangLi != 0)
		{
			num ^= YingXiangLi.GetHashCode();
		}
		if (PreYingXiangLi != 0)
		{
			num ^= PreYingXiangLi.GetHashCode();
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
		if (StoneKuCun != 0.0)
		{
			output.WriteRawTag(33);
			output.WriteDouble(StoneKuCun);
		}
		if (StoneChanLiang != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(StoneChanLiang);
		}
		if (JadeKuCun != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(JadeKuCun);
		}
		if (JadeChanLiang != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(JadeChanLiang);
		}
		if (DanYaoKuCun != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(DanYaoKuCun);
		}
		if (DanYaoChanLiang != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(DanYaoChanLiang);
		}
		if (FaQiKuCun != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(FaQiKuCun);
		}
		if (FaQiChanLiang != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(FaQiChanLiang);
		}
		if (FuLuKuCun != 0L)
		{
			output.WriteRawTag(96);
			output.WriteInt64(FuLuKuCun);
		}
		if (FuLuChanLiang != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(FuLuChanLiang);
		}
		if (ZhenPanKuCun != 0L)
		{
			output.WriteRawTag(112);
			output.WriteInt64(ZhenPanKuCun);
		}
		if (ZhenPanChanLiang != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(ZhenPanChanLiang);
		}
		if (StoneShengYuCount != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(StoneShengYuCount);
		}
		if (JadeShengYuCount != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(JadeShengYuCount);
		}
		if (CurrentContribution != 0L)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt64(CurrentContribution);
		}
		if (StoneSalary != 0L)
		{
			output.WriteRawTag(152, 1);
			output.WriteInt64(StoneSalary);
		}
		if (JadeSalary != 0)
		{
			output.WriteRawTag(160, 1);
			output.WriteInt32(JadeSalary);
		}
		if (MatrixSalary != 0)
		{
			output.WriteRawTag(168, 1);
			output.WriteInt32(MatrixSalary);
		}
		if (ElixirSalary != 0)
		{
			output.WriteRawTag(176, 1);
			output.WriteInt32(ElixirSalary);
		}
		if (EquipmentSalary != 0)
		{
			output.WriteRawTag(184, 1);
			output.WriteInt32(EquipmentSalary);
		}
		if (SignSalary != 0)
		{
			output.WriteRawTag(192, 1);
			output.WriteInt32(SignSalary);
		}
		if (YingXiangLi != 0)
		{
			output.WriteRawTag(200, 1);
			output.WriteInt32(YingXiangLi);
		}
		if (PreYingXiangLi != 0)
		{
			output.WriteRawTag(208, 1);
			output.WriteInt32(PreYingXiangLi);
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
		if (StoneKuCun != 0.0)
		{
			num += 9;
		}
		if (StoneChanLiang != 0.0)
		{
			num += 9;
		}
		if (JadeKuCun != 0.0)
		{
			num += 9;
		}
		if (JadeChanLiang != 0.0)
		{
			num += 9;
		}
		if (DanYaoKuCun != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(DanYaoKuCun);
		}
		if (DanYaoChanLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DanYaoChanLiang);
		}
		if (FaQiKuCun != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FaQiKuCun);
		}
		if (FaQiChanLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FaQiChanLiang);
		}
		if (FuLuKuCun != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FuLuKuCun);
		}
		if (FuLuChanLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FuLuChanLiang);
		}
		if (ZhenPanKuCun != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ZhenPanKuCun);
		}
		if (ZhenPanChanLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ZhenPanChanLiang);
		}
		if (StoneShengYuCount != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(StoneShengYuCount);
		}
		if (JadeShengYuCount != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(JadeShengYuCount);
		}
		if (CurrentContribution != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(CurrentContribution);
		}
		if (StoneSalary != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(StoneSalary);
		}
		if (JadeSalary != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(JadeSalary);
		}
		if (MatrixSalary != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(MatrixSalary);
		}
		if (ElixirSalary != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ElixirSalary);
		}
		if (EquipmentSalary != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(EquipmentSalary);
		}
		if (SignSalary != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(SignSalary);
		}
		if (YingXiangLi != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(YingXiangLi);
		}
		if (PreYingXiangLi != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(PreYingXiangLi);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(IntoNeiWuResp_1015034 other)
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
				ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015035();
			}
			ZhangLaoInfo.MergeFrom(other.ZhangLaoInfo);
		}
		if (other.StoneKuCun != 0.0)
		{
			StoneKuCun = other.StoneKuCun;
		}
		if (other.StoneChanLiang != 0.0)
		{
			StoneChanLiang = other.StoneChanLiang;
		}
		if (other.JadeKuCun != 0.0)
		{
			JadeKuCun = other.JadeKuCun;
		}
		if (other.JadeChanLiang != 0.0)
		{
			JadeChanLiang = other.JadeChanLiang;
		}
		if (other.DanYaoKuCun != 0L)
		{
			DanYaoKuCun = other.DanYaoKuCun;
		}
		if (other.DanYaoChanLiang != 0)
		{
			DanYaoChanLiang = other.DanYaoChanLiang;
		}
		if (other.FaQiKuCun != 0L)
		{
			FaQiKuCun = other.FaQiKuCun;
		}
		if (other.FaQiChanLiang != 0)
		{
			FaQiChanLiang = other.FaQiChanLiang;
		}
		if (other.FuLuKuCun != 0L)
		{
			FuLuKuCun = other.FuLuKuCun;
		}
		if (other.FuLuChanLiang != 0)
		{
			FuLuChanLiang = other.FuLuChanLiang;
		}
		if (other.ZhenPanKuCun != 0L)
		{
			ZhenPanKuCun = other.ZhenPanKuCun;
		}
		if (other.ZhenPanChanLiang != 0)
		{
			ZhenPanChanLiang = other.ZhenPanChanLiang;
		}
		if (other.StoneShengYuCount != 0)
		{
			StoneShengYuCount = other.StoneShengYuCount;
		}
		if (other.JadeShengYuCount != 0)
		{
			JadeShengYuCount = other.JadeShengYuCount;
		}
		if (other.CurrentContribution != 0L)
		{
			CurrentContribution = other.CurrentContribution;
		}
		if (other.StoneSalary != 0L)
		{
			StoneSalary = other.StoneSalary;
		}
		if (other.JadeSalary != 0)
		{
			JadeSalary = other.JadeSalary;
		}
		if (other.MatrixSalary != 0)
		{
			MatrixSalary = other.MatrixSalary;
		}
		if (other.ElixirSalary != 0)
		{
			ElixirSalary = other.ElixirSalary;
		}
		if (other.EquipmentSalary != 0)
		{
			EquipmentSalary = other.EquipmentSalary;
		}
		if (other.SignSalary != 0)
		{
			SignSalary = other.SignSalary;
		}
		if (other.YingXiangLi != 0)
		{
			YingXiangLi = other.YingXiangLi;
		}
		if (other.PreYingXiangLi != 0)
		{
			PreYingXiangLi = other.PreYingXiangLi;
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
					ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015035();
				}
				input.ReadMessage(ZhangLaoInfo);
				break;
			case 33u:
				StoneKuCun = input.ReadDouble();
				break;
			case 41u:
				StoneChanLiang = input.ReadDouble();
				break;
			case 49u:
				JadeKuCun = input.ReadDouble();
				break;
			case 57u:
				JadeChanLiang = input.ReadDouble();
				break;
			case 64u:
				DanYaoKuCun = input.ReadInt64();
				break;
			case 72u:
				DanYaoChanLiang = input.ReadInt32();
				break;
			case 80u:
				FaQiKuCun = input.ReadInt64();
				break;
			case 88u:
				FaQiChanLiang = input.ReadInt32();
				break;
			case 96u:
				FuLuKuCun = input.ReadInt64();
				break;
			case 104u:
				FuLuChanLiang = input.ReadInt32();
				break;
			case 112u:
				ZhenPanKuCun = input.ReadInt64();
				break;
			case 120u:
				ZhenPanChanLiang = input.ReadInt32();
				break;
			case 128u:
				StoneShengYuCount = input.ReadInt32();
				break;
			case 136u:
				JadeShengYuCount = input.ReadInt32();
				break;
			case 144u:
				CurrentContribution = input.ReadInt64();
				break;
			case 152u:
				StoneSalary = input.ReadInt64();
				break;
			case 160u:
				JadeSalary = input.ReadInt32();
				break;
			case 168u:
				MatrixSalary = input.ReadInt32();
				break;
			case 176u:
				ElixirSalary = input.ReadInt32();
				break;
			case 184u:
				EquipmentSalary = input.ReadInt32();
				break;
			case 192u:
				SignSalary = input.ReadInt32();
				break;
			case 200u:
				YingXiangLi = input.ReadInt32();
				break;
			case 208u:
				PreYingXiangLi = input.ReadInt32();
				break;
			}
		}
	}
}
