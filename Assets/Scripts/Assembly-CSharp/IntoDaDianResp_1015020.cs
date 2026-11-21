using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class IntoDaDianResp_1015020 : IMessage<IntoDaDianResp_1015020>, IMessage, IEquatable<IntoDaDianResp_1015020>, IDeepCloneable<IntoDaDianResp_1015020>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ZongZhuInfoResp_1015021 : IMessage<ZongZhuInfoResp_1015021>, IMessage, IEquatable<ZongZhuInfoResp_1015021>, IDeepCloneable<ZongZhuInfoResp_1015021>
		{
			private static readonly MessageParser<ZongZhuInfoResp_1015021> _parser = new MessageParser<ZongZhuInfoResp_1015021>(() => new ZongZhuInfoResp_1015021());

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

			[DebuggerNonUserCode]
			public static MessageParser<ZongZhuInfoResp_1015021> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => IntoDaDianResp_1015020.Descriptor.NestedTypes[0];

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

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public ZongZhuInfoResp_1015021()
			{
			}

			[DebuggerNonUserCode]
			public ZongZhuInfoResp_1015021(ZongZhuInfoResp_1015021 other)
				: this()
			{
				roleId_ = other.roleId_;
				roleImage_ = other.roleImage_;
				roleName_ = other.roleName_;
				level_ = other.level_;
				sex_ = other.sex_;
				fighting_ = other.fighting_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ZongZhuInfoResp_1015021 Clone()
			{
				return new ZongZhuInfoResp_1015021(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ZongZhuInfoResp_1015021);
			}

			[DebuggerNonUserCode]
			public bool Equals(ZongZhuInfoResp_1015021 other)
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
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(ZongZhuInfoResp_1015021 other)
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
					}
				}
			}
		}
	}

	private static readonly MessageParser<IntoDaDianResp_1015020> _parser = new MessageParser<IntoDaDianResp_1015020>(() => new IntoDaDianResp_1015020());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ZongZhuInfoFieldNumber = 3;

	private Types.ZongZhuInfoResp_1015021 zongZhuInfo_;

	public const int StageFieldNumber = 4;

	private int stage_;

	public const int InfluenceFieldNumber = 5;

	private int influence_;

	public const int FightingFieldNumber = 6;

	private long fighting_;

	public const int FangYuFieldNumber = 7;

	private long fangYu_;

	public const int StoneKuCunFieldNumber = 8;

	private double stoneKuCun_;

	public const int StoneChanLiangFieldNumber = 9;

	private double stoneChanLiang_;

	public const int JadeKuCunFieldNumber = 10;

	private double jadeKuCun_;

	public const int JadeChanLiangFieldNumber = 11;

	private double jadeChanLiang_;

	public const int YaoCaiKuCunFieldNumber = 12;

	private long yaoCaiKuCun_;

	public const int YaoCaiChanLiangFieldNumber = 13;

	private int yaoCaiChanLiang_;

	public const int KuangShiKuCunFieldNumber = 14;

	private long kuangShiKuCun_;

	public const int KuangShiChanLiangFieldNumber = 15;

	private int kuangShiChanLiang_;

	public const int YuanCaiKuCunFieldNumber = 16;

	private long yuanCaiKuCun_;

	public const int YuanCaiChanLiangFieldNumber = 17;

	private int yuanCaiChanLiang_;

	public const int DanYaoKuCunFieldNumber = 18;

	private long danYaoKuCun_;

	public const int DanYaoChanLiangFieldNumber = 19;

	private int danYaoChanLiang_;

	public const int FaQiKuCunFieldNumber = 20;

	private long faQiKuCun_;

	public const int FaQiChanLiangFieldNumber = 21;

	private int faQiChanLiang_;

	public const int FuLuKuCunFieldNumber = 22;

	private long fuLuKuCun_;

	public const int FuLuChanLiangFieldNumber = 23;

	private int fuLuChanLiang_;

	public const int ZhenPanKuCunFieldNumber = 24;

	private long zhenPanKuCun_;

	public const int ZhenPanChanLiangFieldNumber = 25;

	private int zhenPanChanLiang_;

	public const int YaoShouKuCunFieldNumber = 26;

	private int yaoShouKuCun_;

	public const int YaoShouChanLiangFieldNumber = 27;

	private int yaoShouChanLiang_;

	public const int ZhangLaoShuLiangFieldNumber = 28;

	private int zhangLaoShuLiang_;

	public const int ZhangLaoZhanDouLiFieldNumber = 29;

	private long zhangLaoZhanDouLi_;

	public const int ZhangLaoFangYuLiFieldNumber = 30;

	private long zhangLaoFangYuLi_;

	public const int DiZiShuLiangFieldNumber = 31;

	private int diZiShuLiang_;

	public const int DiZiZhanDouLiFieldNumber = 32;

	private long diZiZhanDouLi_;

	public const int DiZiFangYuLiFieldNumber = 33;

	private long diZiFangYuLi_;

	public const int ZhiFaDiZiShuLiangFieldNumber = 34;

	private int zhiFaDiZiShuLiang_;

	public const int ZongMenHunLuanDuFieldNumber = 35;

	private int zongMenHunLuanDu_;

	public const int MeiXiaoShiSiWangFieldNumber = 36;

	private double meiXiaoShiSiWang_;

	public const int MeiXiaoShiRuZongFieldNumber = 37;

	private int meiXiaoShiRuZong_;

	public const int NeiMenDiZiShuLiangFieldNumber = 38;

	private int neiMenDiZiShuLiang_;

	public const int LianDanDiZiShuLiangFieldNumber = 39;

	private int lianDanDiZiShuLiang_;

	public const int LianQiDiZiShuLiangFieldNumber = 40;

	private int lianQiDiZiShuLiang_;

	public const int ZhiFuDiZiShuLiangFieldNumber = 41;

	private int zhiFuDiZiShuLiang_;

	public const int SiYangYaoShouDiZiShuLiangFieldNumber = 42;

	private int siYangYaoShouDiZiShuLiang_;

	public const int KaiCaiLingYuDiZiShuLiangFieldNumber = 43;

	private int kaiCaiLingYuDiZiShuLiang_;

	public const int KaiCaiLingShiDiZiShuLiangFieldNumber = 44;

	private int kaiCaiLingShiDiZiShuLiang_;

	public const int ZhongZhiDiZiShuLiangFieldNumber = 45;

	private int zhongZhiDiZiShuLiang_;

	public const int YeLianDiZiShuLiangFieldNumber = 46;

	private int yeLianDiZiShuLiang_;

	public const int ZhenPanDiZiShuLiangFieldNumber = 47;

	private int zhenPanDiZiShuLiang_;

	public const int GongFangDiZiShuLiangFieldNumber = 48;

	private int gongFangDiZiShuLiang_;

	[DebuggerNonUserCode]
	public static MessageParser<IntoDaDianResp_1015020> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[23];

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
	public Types.ZongZhuInfoResp_1015021 ZongZhuInfo
	{
		get
		{
			return zongZhuInfo_;
		}
		set
		{
			zongZhuInfo_ = value;
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
	public int Influence
	{
		get
		{
			return influence_;
		}
		set
		{
			influence_ = value;
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
	public long FangYu
	{
		get
		{
			return fangYu_;
		}
		set
		{
			fangYu_ = value;
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
	public long YaoCaiKuCun
	{
		get
		{
			return yaoCaiKuCun_;
		}
		set
		{
			yaoCaiKuCun_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int YaoCaiChanLiang
	{
		get
		{
			return yaoCaiChanLiang_;
		}
		set
		{
			yaoCaiChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long KuangShiKuCun
	{
		get
		{
			return kuangShiKuCun_;
		}
		set
		{
			kuangShiKuCun_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int KuangShiChanLiang
	{
		get
		{
			return kuangShiChanLiang_;
		}
		set
		{
			kuangShiChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long YuanCaiKuCun
	{
		get
		{
			return yuanCaiKuCun_;
		}
		set
		{
			yuanCaiKuCun_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int YuanCaiChanLiang
	{
		get
		{
			return yuanCaiChanLiang_;
		}
		set
		{
			yuanCaiChanLiang_ = value;
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
	public int YaoShouKuCun
	{
		get
		{
			return yaoShouKuCun_;
		}
		set
		{
			yaoShouKuCun_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int YaoShouChanLiang
	{
		get
		{
			return yaoShouChanLiang_;
		}
		set
		{
			yaoShouChanLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ZhangLaoShuLiang
	{
		get
		{
			return zhangLaoShuLiang_;
		}
		set
		{
			zhangLaoShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ZhangLaoZhanDouLi
	{
		get
		{
			return zhangLaoZhanDouLi_;
		}
		set
		{
			zhangLaoZhanDouLi_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ZhangLaoFangYuLi
	{
		get
		{
			return zhangLaoFangYuLi_;
		}
		set
		{
			zhangLaoFangYuLi_ = value;
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
	public double MeiXiaoShiSiWang
	{
		get
		{
			return meiXiaoShiSiWang_;
		}
		set
		{
			meiXiaoShiSiWang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MeiXiaoShiRuZong
	{
		get
		{
			return meiXiaoShiRuZong_;
		}
		set
		{
			meiXiaoShiRuZong_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int NeiMenDiZiShuLiang
	{
		get
		{
			return neiMenDiZiShuLiang_;
		}
		set
		{
			neiMenDiZiShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LianDanDiZiShuLiang
	{
		get
		{
			return lianDanDiZiShuLiang_;
		}
		set
		{
			lianDanDiZiShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LianQiDiZiShuLiang
	{
		get
		{
			return lianQiDiZiShuLiang_;
		}
		set
		{
			lianQiDiZiShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ZhiFuDiZiShuLiang
	{
		get
		{
			return zhiFuDiZiShuLiang_;
		}
		set
		{
			zhiFuDiZiShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SiYangYaoShouDiZiShuLiang
	{
		get
		{
			return siYangYaoShouDiZiShuLiang_;
		}
		set
		{
			siYangYaoShouDiZiShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int KaiCaiLingYuDiZiShuLiang
	{
		get
		{
			return kaiCaiLingYuDiZiShuLiang_;
		}
		set
		{
			kaiCaiLingYuDiZiShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int KaiCaiLingShiDiZiShuLiang
	{
		get
		{
			return kaiCaiLingShiDiZiShuLiang_;
		}
		set
		{
			kaiCaiLingShiDiZiShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ZhongZhiDiZiShuLiang
	{
		get
		{
			return zhongZhiDiZiShuLiang_;
		}
		set
		{
			zhongZhiDiZiShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int YeLianDiZiShuLiang
	{
		get
		{
			return yeLianDiZiShuLiang_;
		}
		set
		{
			yeLianDiZiShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ZhenPanDiZiShuLiang
	{
		get
		{
			return zhenPanDiZiShuLiang_;
		}
		set
		{
			zhenPanDiZiShuLiang_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int GongFangDiZiShuLiang
	{
		get
		{
			return gongFangDiZiShuLiang_;
		}
		set
		{
			gongFangDiZiShuLiang_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public IntoDaDianResp_1015020()
	{
	}

	[DebuggerNonUserCode]
	public IntoDaDianResp_1015020(IntoDaDianResp_1015020 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		zongZhuInfo_ = ((other.zongZhuInfo_ != null) ? other.zongZhuInfo_.Clone() : null);
		stage_ = other.stage_;
		influence_ = other.influence_;
		fighting_ = other.fighting_;
		fangYu_ = other.fangYu_;
		stoneKuCun_ = other.stoneKuCun_;
		stoneChanLiang_ = other.stoneChanLiang_;
		jadeKuCun_ = other.jadeKuCun_;
		jadeChanLiang_ = other.jadeChanLiang_;
		yaoCaiKuCun_ = other.yaoCaiKuCun_;
		yaoCaiChanLiang_ = other.yaoCaiChanLiang_;
		kuangShiKuCun_ = other.kuangShiKuCun_;
		kuangShiChanLiang_ = other.kuangShiChanLiang_;
		yuanCaiKuCun_ = other.yuanCaiKuCun_;
		yuanCaiChanLiang_ = other.yuanCaiChanLiang_;
		danYaoKuCun_ = other.danYaoKuCun_;
		danYaoChanLiang_ = other.danYaoChanLiang_;
		faQiKuCun_ = other.faQiKuCun_;
		faQiChanLiang_ = other.faQiChanLiang_;
		fuLuKuCun_ = other.fuLuKuCun_;
		fuLuChanLiang_ = other.fuLuChanLiang_;
		zhenPanKuCun_ = other.zhenPanKuCun_;
		zhenPanChanLiang_ = other.zhenPanChanLiang_;
		yaoShouKuCun_ = other.yaoShouKuCun_;
		yaoShouChanLiang_ = other.yaoShouChanLiang_;
		zhangLaoShuLiang_ = other.zhangLaoShuLiang_;
		zhangLaoZhanDouLi_ = other.zhangLaoZhanDouLi_;
		zhangLaoFangYuLi_ = other.zhangLaoFangYuLi_;
		diZiShuLiang_ = other.diZiShuLiang_;
		diZiZhanDouLi_ = other.diZiZhanDouLi_;
		diZiFangYuLi_ = other.diZiFangYuLi_;
		zhiFaDiZiShuLiang_ = other.zhiFaDiZiShuLiang_;
		zongMenHunLuanDu_ = other.zongMenHunLuanDu_;
		meiXiaoShiSiWang_ = other.meiXiaoShiSiWang_;
		meiXiaoShiRuZong_ = other.meiXiaoShiRuZong_;
		neiMenDiZiShuLiang_ = other.neiMenDiZiShuLiang_;
		lianDanDiZiShuLiang_ = other.lianDanDiZiShuLiang_;
		lianQiDiZiShuLiang_ = other.lianQiDiZiShuLiang_;
		zhiFuDiZiShuLiang_ = other.zhiFuDiZiShuLiang_;
		siYangYaoShouDiZiShuLiang_ = other.siYangYaoShouDiZiShuLiang_;
		kaiCaiLingYuDiZiShuLiang_ = other.kaiCaiLingYuDiZiShuLiang_;
		kaiCaiLingShiDiZiShuLiang_ = other.kaiCaiLingShiDiZiShuLiang_;
		zhongZhiDiZiShuLiang_ = other.zhongZhiDiZiShuLiang_;
		yeLianDiZiShuLiang_ = other.yeLianDiZiShuLiang_;
		zhenPanDiZiShuLiang_ = other.zhenPanDiZiShuLiang_;
		gongFangDiZiShuLiang_ = other.gongFangDiZiShuLiang_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IntoDaDianResp_1015020 Clone()
	{
		return new IntoDaDianResp_1015020(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IntoDaDianResp_1015020);
	}

	[DebuggerNonUserCode]
	public bool Equals(IntoDaDianResp_1015020 other)
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
		if (!object.Equals(ZongZhuInfo, other.ZongZhuInfo))
		{
			return false;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		if (Influence != other.Influence)
		{
			return false;
		}
		if (Fighting != other.Fighting)
		{
			return false;
		}
		if (FangYu != other.FangYu)
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
		if (YaoCaiKuCun != other.YaoCaiKuCun)
		{
			return false;
		}
		if (YaoCaiChanLiang != other.YaoCaiChanLiang)
		{
			return false;
		}
		if (KuangShiKuCun != other.KuangShiKuCun)
		{
			return false;
		}
		if (KuangShiChanLiang != other.KuangShiChanLiang)
		{
			return false;
		}
		if (YuanCaiKuCun != other.YuanCaiKuCun)
		{
			return false;
		}
		if (YuanCaiChanLiang != other.YuanCaiChanLiang)
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
		if (YaoShouKuCun != other.YaoShouKuCun)
		{
			return false;
		}
		if (YaoShouChanLiang != other.YaoShouChanLiang)
		{
			return false;
		}
		if (ZhangLaoShuLiang != other.ZhangLaoShuLiang)
		{
			return false;
		}
		if (ZhangLaoZhanDouLi != other.ZhangLaoZhanDouLi)
		{
			return false;
		}
		if (ZhangLaoFangYuLi != other.ZhangLaoFangYuLi)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(MeiXiaoShiSiWang, other.MeiXiaoShiSiWang))
		{
			return false;
		}
		if (MeiXiaoShiRuZong != other.MeiXiaoShiRuZong)
		{
			return false;
		}
		if (NeiMenDiZiShuLiang != other.NeiMenDiZiShuLiang)
		{
			return false;
		}
		if (LianDanDiZiShuLiang != other.LianDanDiZiShuLiang)
		{
			return false;
		}
		if (LianQiDiZiShuLiang != other.LianQiDiZiShuLiang)
		{
			return false;
		}
		if (ZhiFuDiZiShuLiang != other.ZhiFuDiZiShuLiang)
		{
			return false;
		}
		if (SiYangYaoShouDiZiShuLiang != other.SiYangYaoShouDiZiShuLiang)
		{
			return false;
		}
		if (KaiCaiLingYuDiZiShuLiang != other.KaiCaiLingYuDiZiShuLiang)
		{
			return false;
		}
		if (KaiCaiLingShiDiZiShuLiang != other.KaiCaiLingShiDiZiShuLiang)
		{
			return false;
		}
		if (ZhongZhiDiZiShuLiang != other.ZhongZhiDiZiShuLiang)
		{
			return false;
		}
		if (YeLianDiZiShuLiang != other.YeLianDiZiShuLiang)
		{
			return false;
		}
		if (ZhenPanDiZiShuLiang != other.ZhenPanDiZiShuLiang)
		{
			return false;
		}
		if (GongFangDiZiShuLiang != other.GongFangDiZiShuLiang)
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
		if (zongZhuInfo_ != null)
		{
			num ^= ZongZhuInfo.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (Influence != 0)
		{
			num ^= Influence.GetHashCode();
		}
		if (Fighting != 0L)
		{
			num ^= Fighting.GetHashCode();
		}
		if (FangYu != 0L)
		{
			num ^= FangYu.GetHashCode();
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
		if (YaoCaiKuCun != 0L)
		{
			num ^= YaoCaiKuCun.GetHashCode();
		}
		if (YaoCaiChanLiang != 0)
		{
			num ^= YaoCaiChanLiang.GetHashCode();
		}
		if (KuangShiKuCun != 0L)
		{
			num ^= KuangShiKuCun.GetHashCode();
		}
		if (KuangShiChanLiang != 0)
		{
			num ^= KuangShiChanLiang.GetHashCode();
		}
		if (YuanCaiKuCun != 0L)
		{
			num ^= YuanCaiKuCun.GetHashCode();
		}
		if (YuanCaiChanLiang != 0)
		{
			num ^= YuanCaiChanLiang.GetHashCode();
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
		if (YaoShouKuCun != 0)
		{
			num ^= YaoShouKuCun.GetHashCode();
		}
		if (YaoShouChanLiang != 0)
		{
			num ^= YaoShouChanLiang.GetHashCode();
		}
		if (ZhangLaoShuLiang != 0)
		{
			num ^= ZhangLaoShuLiang.GetHashCode();
		}
		if (ZhangLaoZhanDouLi != 0L)
		{
			num ^= ZhangLaoZhanDouLi.GetHashCode();
		}
		if (ZhangLaoFangYuLi != 0L)
		{
			num ^= ZhangLaoFangYuLi.GetHashCode();
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
		if (MeiXiaoShiSiWang != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(MeiXiaoShiSiWang);
		}
		if (MeiXiaoShiRuZong != 0)
		{
			num ^= MeiXiaoShiRuZong.GetHashCode();
		}
		if (NeiMenDiZiShuLiang != 0)
		{
			num ^= NeiMenDiZiShuLiang.GetHashCode();
		}
		if (LianDanDiZiShuLiang != 0)
		{
			num ^= LianDanDiZiShuLiang.GetHashCode();
		}
		if (LianQiDiZiShuLiang != 0)
		{
			num ^= LianQiDiZiShuLiang.GetHashCode();
		}
		if (ZhiFuDiZiShuLiang != 0)
		{
			num ^= ZhiFuDiZiShuLiang.GetHashCode();
		}
		if (SiYangYaoShouDiZiShuLiang != 0)
		{
			num ^= SiYangYaoShouDiZiShuLiang.GetHashCode();
		}
		if (KaiCaiLingYuDiZiShuLiang != 0)
		{
			num ^= KaiCaiLingYuDiZiShuLiang.GetHashCode();
		}
		if (KaiCaiLingShiDiZiShuLiang != 0)
		{
			num ^= KaiCaiLingShiDiZiShuLiang.GetHashCode();
		}
		if (ZhongZhiDiZiShuLiang != 0)
		{
			num ^= ZhongZhiDiZiShuLiang.GetHashCode();
		}
		if (YeLianDiZiShuLiang != 0)
		{
			num ^= YeLianDiZiShuLiang.GetHashCode();
		}
		if (ZhenPanDiZiShuLiang != 0)
		{
			num ^= ZhenPanDiZiShuLiang.GetHashCode();
		}
		if (GongFangDiZiShuLiang != 0)
		{
			num ^= GongFangDiZiShuLiang.GetHashCode();
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
		if (zongZhuInfo_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(ZongZhuInfo);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Stage);
		}
		if (Influence != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Influence);
		}
		if (Fighting != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(Fighting);
		}
		if (FangYu != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(FangYu);
		}
		if (StoneKuCun != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(StoneKuCun);
		}
		if (StoneChanLiang != 0.0)
		{
			output.WriteRawTag(73);
			output.WriteDouble(StoneChanLiang);
		}
		if (JadeKuCun != 0.0)
		{
			output.WriteRawTag(81);
			output.WriteDouble(JadeKuCun);
		}
		if (JadeChanLiang != 0.0)
		{
			output.WriteRawTag(89);
			output.WriteDouble(JadeChanLiang);
		}
		if (YaoCaiKuCun != 0L)
		{
			output.WriteRawTag(96);
			output.WriteInt64(YaoCaiKuCun);
		}
		if (YaoCaiChanLiang != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(YaoCaiChanLiang);
		}
		if (KuangShiKuCun != 0L)
		{
			output.WriteRawTag(112);
			output.WriteInt64(KuangShiKuCun);
		}
		if (KuangShiChanLiang != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(KuangShiChanLiang);
		}
		if (YuanCaiKuCun != 0L)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt64(YuanCaiKuCun);
		}
		if (YuanCaiChanLiang != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(YuanCaiChanLiang);
		}
		if (DanYaoKuCun != 0L)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt64(DanYaoKuCun);
		}
		if (DanYaoChanLiang != 0)
		{
			output.WriteRawTag(152, 1);
			output.WriteInt32(DanYaoChanLiang);
		}
		if (FaQiKuCun != 0L)
		{
			output.WriteRawTag(160, 1);
			output.WriteInt64(FaQiKuCun);
		}
		if (FaQiChanLiang != 0)
		{
			output.WriteRawTag(168, 1);
			output.WriteInt32(FaQiChanLiang);
		}
		if (FuLuKuCun != 0L)
		{
			output.WriteRawTag(176, 1);
			output.WriteInt64(FuLuKuCun);
		}
		if (FuLuChanLiang != 0)
		{
			output.WriteRawTag(184, 1);
			output.WriteInt32(FuLuChanLiang);
		}
		if (ZhenPanKuCun != 0L)
		{
			output.WriteRawTag(192, 1);
			output.WriteInt64(ZhenPanKuCun);
		}
		if (ZhenPanChanLiang != 0)
		{
			output.WriteRawTag(200, 1);
			output.WriteInt32(ZhenPanChanLiang);
		}
		if (YaoShouKuCun != 0)
		{
			output.WriteRawTag(208, 1);
			output.WriteInt32(YaoShouKuCun);
		}
		if (YaoShouChanLiang != 0)
		{
			output.WriteRawTag(216, 1);
			output.WriteInt32(YaoShouChanLiang);
		}
		if (ZhangLaoShuLiang != 0)
		{
			output.WriteRawTag(224, 1);
			output.WriteInt32(ZhangLaoShuLiang);
		}
		if (ZhangLaoZhanDouLi != 0L)
		{
			output.WriteRawTag(232, 1);
			output.WriteInt64(ZhangLaoZhanDouLi);
		}
		if (ZhangLaoFangYuLi != 0L)
		{
			output.WriteRawTag(240, 1);
			output.WriteInt64(ZhangLaoFangYuLi);
		}
		if (DiZiShuLiang != 0)
		{
			output.WriteRawTag(248, 1);
			output.WriteInt32(DiZiShuLiang);
		}
		if (DiZiZhanDouLi != 0L)
		{
			output.WriteRawTag(128, 2);
			output.WriteInt64(DiZiZhanDouLi);
		}
		if (DiZiFangYuLi != 0L)
		{
			output.WriteRawTag(136, 2);
			output.WriteInt64(DiZiFangYuLi);
		}
		if (ZhiFaDiZiShuLiang != 0)
		{
			output.WriteRawTag(144, 2);
			output.WriteInt32(ZhiFaDiZiShuLiang);
		}
		if (ZongMenHunLuanDu != 0)
		{
			output.WriteRawTag(152, 2);
			output.WriteInt32(ZongMenHunLuanDu);
		}
		if (MeiXiaoShiSiWang != 0.0)
		{
			output.WriteRawTag(161, 2);
			output.WriteDouble(MeiXiaoShiSiWang);
		}
		if (MeiXiaoShiRuZong != 0)
		{
			output.WriteRawTag(168, 2);
			output.WriteInt32(MeiXiaoShiRuZong);
		}
		if (NeiMenDiZiShuLiang != 0)
		{
			output.WriteRawTag(176, 2);
			output.WriteInt32(NeiMenDiZiShuLiang);
		}
		if (LianDanDiZiShuLiang != 0)
		{
			output.WriteRawTag(184, 2);
			output.WriteInt32(LianDanDiZiShuLiang);
		}
		if (LianQiDiZiShuLiang != 0)
		{
			output.WriteRawTag(192, 2);
			output.WriteInt32(LianQiDiZiShuLiang);
		}
		if (ZhiFuDiZiShuLiang != 0)
		{
			output.WriteRawTag(200, 2);
			output.WriteInt32(ZhiFuDiZiShuLiang);
		}
		if (SiYangYaoShouDiZiShuLiang != 0)
		{
			output.WriteRawTag(208, 2);
			output.WriteInt32(SiYangYaoShouDiZiShuLiang);
		}
		if (KaiCaiLingYuDiZiShuLiang != 0)
		{
			output.WriteRawTag(216, 2);
			output.WriteInt32(KaiCaiLingYuDiZiShuLiang);
		}
		if (KaiCaiLingShiDiZiShuLiang != 0)
		{
			output.WriteRawTag(224, 2);
			output.WriteInt32(KaiCaiLingShiDiZiShuLiang);
		}
		if (ZhongZhiDiZiShuLiang != 0)
		{
			output.WriteRawTag(232, 2);
			output.WriteInt32(ZhongZhiDiZiShuLiang);
		}
		if (YeLianDiZiShuLiang != 0)
		{
			output.WriteRawTag(240, 2);
			output.WriteInt32(YeLianDiZiShuLiang);
		}
		if (ZhenPanDiZiShuLiang != 0)
		{
			output.WriteRawTag(248, 2);
			output.WriteInt32(ZhenPanDiZiShuLiang);
		}
		if (GongFangDiZiShuLiang != 0)
		{
			output.WriteRawTag(128, 3);
			output.WriteInt32(GongFangDiZiShuLiang);
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
		if (zongZhuInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ZongZhuInfo);
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (Influence != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Influence);
		}
		if (Fighting != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Fighting);
		}
		if (FangYu != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FangYu);
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
		if (YaoCaiKuCun != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(YaoCaiKuCun);
		}
		if (YaoCaiChanLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(YaoCaiChanLiang);
		}
		if (KuangShiKuCun != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(KuangShiKuCun);
		}
		if (KuangShiChanLiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(KuangShiChanLiang);
		}
		if (YuanCaiKuCun != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(YuanCaiKuCun);
		}
		if (YuanCaiChanLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(YuanCaiChanLiang);
		}
		if (DanYaoKuCun != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(DanYaoKuCun);
		}
		if (DanYaoChanLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(DanYaoChanLiang);
		}
		if (FaQiKuCun != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(FaQiKuCun);
		}
		if (FaQiChanLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(FaQiChanLiang);
		}
		if (FuLuKuCun != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(FuLuKuCun);
		}
		if (FuLuChanLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(FuLuChanLiang);
		}
		if (ZhenPanKuCun != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(ZhenPanKuCun);
		}
		if (ZhenPanChanLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ZhenPanChanLiang);
		}
		if (YaoShouKuCun != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(YaoShouKuCun);
		}
		if (YaoShouChanLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(YaoShouChanLiang);
		}
		if (ZhangLaoShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ZhangLaoShuLiang);
		}
		if (ZhangLaoZhanDouLi != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(ZhangLaoZhanDouLi);
		}
		if (ZhangLaoFangYuLi != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(ZhangLaoFangYuLi);
		}
		if (DiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(DiZiShuLiang);
		}
		if (DiZiZhanDouLi != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(DiZiZhanDouLi);
		}
		if (DiZiFangYuLi != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(DiZiFangYuLi);
		}
		if (ZhiFaDiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ZhiFaDiZiShuLiang);
		}
		if (ZongMenHunLuanDu != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ZongMenHunLuanDu);
		}
		if (MeiXiaoShiSiWang != 0.0)
		{
			num += 10;
		}
		if (MeiXiaoShiRuZong != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(MeiXiaoShiRuZong);
		}
		if (NeiMenDiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(NeiMenDiZiShuLiang);
		}
		if (LianDanDiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(LianDanDiZiShuLiang);
		}
		if (LianQiDiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(LianQiDiZiShuLiang);
		}
		if (ZhiFuDiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ZhiFuDiZiShuLiang);
		}
		if (SiYangYaoShouDiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(SiYangYaoShouDiZiShuLiang);
		}
		if (KaiCaiLingYuDiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(KaiCaiLingYuDiZiShuLiang);
		}
		if (KaiCaiLingShiDiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(KaiCaiLingShiDiZiShuLiang);
		}
		if (ZhongZhiDiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ZhongZhiDiZiShuLiang);
		}
		if (YeLianDiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(YeLianDiZiShuLiang);
		}
		if (ZhenPanDiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ZhenPanDiZiShuLiang);
		}
		if (GongFangDiZiShuLiang != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(GongFangDiZiShuLiang);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(IntoDaDianResp_1015020 other)
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
		if (other.zongZhuInfo_ != null)
		{
			if (zongZhuInfo_ == null)
			{
				ZongZhuInfo = new Types.ZongZhuInfoResp_1015021();
			}
			ZongZhuInfo.MergeFrom(other.ZongZhuInfo);
		}
		if (other.Stage != 0)
		{
			Stage = other.Stage;
		}
		if (other.Influence != 0)
		{
			Influence = other.Influence;
		}
		if (other.Fighting != 0L)
		{
			Fighting = other.Fighting;
		}
		if (other.FangYu != 0L)
		{
			FangYu = other.FangYu;
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
		if (other.YaoCaiKuCun != 0L)
		{
			YaoCaiKuCun = other.YaoCaiKuCun;
		}
		if (other.YaoCaiChanLiang != 0)
		{
			YaoCaiChanLiang = other.YaoCaiChanLiang;
		}
		if (other.KuangShiKuCun != 0L)
		{
			KuangShiKuCun = other.KuangShiKuCun;
		}
		if (other.KuangShiChanLiang != 0)
		{
			KuangShiChanLiang = other.KuangShiChanLiang;
		}
		if (other.YuanCaiKuCun != 0L)
		{
			YuanCaiKuCun = other.YuanCaiKuCun;
		}
		if (other.YuanCaiChanLiang != 0)
		{
			YuanCaiChanLiang = other.YuanCaiChanLiang;
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
		if (other.YaoShouKuCun != 0)
		{
			YaoShouKuCun = other.YaoShouKuCun;
		}
		if (other.YaoShouChanLiang != 0)
		{
			YaoShouChanLiang = other.YaoShouChanLiang;
		}
		if (other.ZhangLaoShuLiang != 0)
		{
			ZhangLaoShuLiang = other.ZhangLaoShuLiang;
		}
		if (other.ZhangLaoZhanDouLi != 0L)
		{
			ZhangLaoZhanDouLi = other.ZhangLaoZhanDouLi;
		}
		if (other.ZhangLaoFangYuLi != 0L)
		{
			ZhangLaoFangYuLi = other.ZhangLaoFangYuLi;
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
		if (other.MeiXiaoShiSiWang != 0.0)
		{
			MeiXiaoShiSiWang = other.MeiXiaoShiSiWang;
		}
		if (other.MeiXiaoShiRuZong != 0)
		{
			MeiXiaoShiRuZong = other.MeiXiaoShiRuZong;
		}
		if (other.NeiMenDiZiShuLiang != 0)
		{
			NeiMenDiZiShuLiang = other.NeiMenDiZiShuLiang;
		}
		if (other.LianDanDiZiShuLiang != 0)
		{
			LianDanDiZiShuLiang = other.LianDanDiZiShuLiang;
		}
		if (other.LianQiDiZiShuLiang != 0)
		{
			LianQiDiZiShuLiang = other.LianQiDiZiShuLiang;
		}
		if (other.ZhiFuDiZiShuLiang != 0)
		{
			ZhiFuDiZiShuLiang = other.ZhiFuDiZiShuLiang;
		}
		if (other.SiYangYaoShouDiZiShuLiang != 0)
		{
			SiYangYaoShouDiZiShuLiang = other.SiYangYaoShouDiZiShuLiang;
		}
		if (other.KaiCaiLingYuDiZiShuLiang != 0)
		{
			KaiCaiLingYuDiZiShuLiang = other.KaiCaiLingYuDiZiShuLiang;
		}
		if (other.KaiCaiLingShiDiZiShuLiang != 0)
		{
			KaiCaiLingShiDiZiShuLiang = other.KaiCaiLingShiDiZiShuLiang;
		}
		if (other.ZhongZhiDiZiShuLiang != 0)
		{
			ZhongZhiDiZiShuLiang = other.ZhongZhiDiZiShuLiang;
		}
		if (other.YeLianDiZiShuLiang != 0)
		{
			YeLianDiZiShuLiang = other.YeLianDiZiShuLiang;
		}
		if (other.ZhenPanDiZiShuLiang != 0)
		{
			ZhenPanDiZiShuLiang = other.ZhenPanDiZiShuLiang;
		}
		if (other.GongFangDiZiShuLiang != 0)
		{
			GongFangDiZiShuLiang = other.GongFangDiZiShuLiang;
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
				if (zongZhuInfo_ == null)
				{
					ZongZhuInfo = new Types.ZongZhuInfoResp_1015021();
				}
				input.ReadMessage(ZongZhuInfo);
				break;
			case 32u:
				Stage = input.ReadInt32();
				break;
			case 40u:
				Influence = input.ReadInt32();
				break;
			case 48u:
				Fighting = input.ReadInt64();
				break;
			case 56u:
				FangYu = input.ReadInt64();
				break;
			case 65u:
				StoneKuCun = input.ReadDouble();
				break;
			case 73u:
				StoneChanLiang = input.ReadDouble();
				break;
			case 81u:
				JadeKuCun = input.ReadDouble();
				break;
			case 89u:
				JadeChanLiang = input.ReadDouble();
				break;
			case 96u:
				YaoCaiKuCun = input.ReadInt64();
				break;
			case 104u:
				YaoCaiChanLiang = input.ReadInt32();
				break;
			case 112u:
				KuangShiKuCun = input.ReadInt64();
				break;
			case 120u:
				KuangShiChanLiang = input.ReadInt32();
				break;
			case 128u:
				YuanCaiKuCun = input.ReadInt64();
				break;
			case 136u:
				YuanCaiChanLiang = input.ReadInt32();
				break;
			case 144u:
				DanYaoKuCun = input.ReadInt64();
				break;
			case 152u:
				DanYaoChanLiang = input.ReadInt32();
				break;
			case 160u:
				FaQiKuCun = input.ReadInt64();
				break;
			case 168u:
				FaQiChanLiang = input.ReadInt32();
				break;
			case 176u:
				FuLuKuCun = input.ReadInt64();
				break;
			case 184u:
				FuLuChanLiang = input.ReadInt32();
				break;
			case 192u:
				ZhenPanKuCun = input.ReadInt64();
				break;
			case 200u:
				ZhenPanChanLiang = input.ReadInt32();
				break;
			case 208u:
				YaoShouKuCun = input.ReadInt32();
				break;
			case 216u:
				YaoShouChanLiang = input.ReadInt32();
				break;
			case 224u:
				ZhangLaoShuLiang = input.ReadInt32();
				break;
			case 232u:
				ZhangLaoZhanDouLi = input.ReadInt64();
				break;
			case 240u:
				ZhangLaoFangYuLi = input.ReadInt64();
				break;
			case 248u:
				DiZiShuLiang = input.ReadInt32();
				break;
			case 256u:
				DiZiZhanDouLi = input.ReadInt64();
				break;
			case 264u:
				DiZiFangYuLi = input.ReadInt64();
				break;
			case 272u:
				ZhiFaDiZiShuLiang = input.ReadInt32();
				break;
			case 280u:
				ZongMenHunLuanDu = input.ReadInt32();
				break;
			case 289u:
				MeiXiaoShiSiWang = input.ReadDouble();
				break;
			case 296u:
				MeiXiaoShiRuZong = input.ReadInt32();
				break;
			case 304u:
				NeiMenDiZiShuLiang = input.ReadInt32();
				break;
			case 312u:
				LianDanDiZiShuLiang = input.ReadInt32();
				break;
			case 320u:
				LianQiDiZiShuLiang = input.ReadInt32();
				break;
			case 328u:
				ZhiFuDiZiShuLiang = input.ReadInt32();
				break;
			case 336u:
				SiYangYaoShouDiZiShuLiang = input.ReadInt32();
				break;
			case 344u:
				KaiCaiLingYuDiZiShuLiang = input.ReadInt32();
				break;
			case 352u:
				KaiCaiLingShiDiZiShuLiang = input.ReadInt32();
				break;
			case 360u:
				ZhongZhiDiZiShuLiang = input.ReadInt32();
				break;
			case 368u:
				YeLianDiZiShuLiang = input.ReadInt32();
				break;
			case 376u:
				ZhenPanDiZiShuLiang = input.ReadInt32();
				break;
			case 384u:
				GongFangDiZiShuLiang = input.ReadInt32();
				break;
			}
		}
	}
}
