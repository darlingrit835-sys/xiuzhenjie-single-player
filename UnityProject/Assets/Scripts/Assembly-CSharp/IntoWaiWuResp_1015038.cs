using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class IntoWaiWuResp_1015038 : IMessage<IntoWaiWuResp_1015038>, IMessage, IEquatable<IntoWaiWuResp_1015038>, IDeepCloneable<IntoWaiWuResp_1015038>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ZhangLaoInfoResp_1015039 : IMessage<ZhangLaoInfoResp_1015039>, IMessage, IEquatable<ZhangLaoInfoResp_1015039>, IDeepCloneable<ZhangLaoInfoResp_1015039>
		{
			private static readonly MessageParser<ZhangLaoInfoResp_1015039> _parser = new MessageParser<ZhangLaoInfoResp_1015039>(() => new ZhangLaoInfoResp_1015039());

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
			public static MessageParser<ZhangLaoInfoResp_1015039> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => IntoWaiWuResp_1015038.Descriptor.NestedTypes[0];

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
			public ZhangLaoInfoResp_1015039()
			{
			}

			[DebuggerNonUserCode]
			public ZhangLaoInfoResp_1015039(ZhangLaoInfoResp_1015039 other)
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
			public ZhangLaoInfoResp_1015039 Clone()
			{
				return new ZhangLaoInfoResp_1015039(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ZhangLaoInfoResp_1015039);
			}

			[DebuggerNonUserCode]
			public bool Equals(ZhangLaoInfoResp_1015039 other)
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
			public void MergeFrom(ZhangLaoInfoResp_1015039 other)
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

		public sealed class XuanShangInfoResp_1015040 : IMessage<XuanShangInfoResp_1015040>, IMessage, IEquatable<XuanShangInfoResp_1015040>, IDeepCloneable<XuanShangInfoResp_1015040>
		{
			private static readonly MessageParser<XuanShangInfoResp_1015040> _parser = new MessageParser<XuanShangInfoResp_1015040>(() => new XuanShangInfoResp_1015040());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private int id_;

			public const int GoodsNameFieldNumber = 2;

			private string goodsName_ = "";

			public const int StoneFieldNumber = 3;

			private long stone_;

			public const int SuccessRateFieldNumber = 4;

			private int successRate_;

			public const int StatusFieldNumber = 5;

			private int status_;

			public const int ShengYuTimeFieldNumber = 6;

			private long shengYuTime_;

			[DebuggerNonUserCode]
			public static MessageParser<XuanShangInfoResp_1015040> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => IntoWaiWuResp_1015038.Descriptor.NestedTypes[1];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int Id
			{
				get
				{
					return id_;
				}
				set
				{
					id_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string GoodsName
			{
				get
				{
					return goodsName_;
				}
				set
				{
					goodsName_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public long Stone
			{
				get
				{
					return stone_;
				}
				set
				{
					stone_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int SuccessRate
			{
				get
				{
					return successRate_;
				}
				set
				{
					successRate_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int Status
			{
				get
				{
					return status_;
				}
				set
				{
					status_ = value;
				}
			}

			[DebuggerNonUserCode]
			public long ShengYuTime
			{
				get
				{
					return shengYuTime_;
				}
				set
				{
					shengYuTime_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public XuanShangInfoResp_1015040()
			{
			}

			[DebuggerNonUserCode]
			public XuanShangInfoResp_1015040(XuanShangInfoResp_1015040 other)
				: this()
			{
				id_ = other.id_;
				goodsName_ = other.goodsName_;
				stone_ = other.stone_;
				successRate_ = other.successRate_;
				status_ = other.status_;
				shengYuTime_ = other.shengYuTime_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public XuanShangInfoResp_1015040 Clone()
			{
				return new XuanShangInfoResp_1015040(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as XuanShangInfoResp_1015040);
			}

			[DebuggerNonUserCode]
			public bool Equals(XuanShangInfoResp_1015040 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (Id != other.Id)
				{
					return false;
				}
				if (GoodsName != other.GoodsName)
				{
					return false;
				}
				if (Stone != other.Stone)
				{
					return false;
				}
				if (SuccessRate != other.SuccessRate)
				{
					return false;
				}
				if (Status != other.Status)
				{
					return false;
				}
				if (ShengYuTime != other.ShengYuTime)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (Id != 0)
				{
					num ^= Id.GetHashCode();
				}
				if (GoodsName.Length != 0)
				{
					num ^= GoodsName.GetHashCode();
				}
				if (Stone != 0L)
				{
					num ^= Stone.GetHashCode();
				}
				if (SuccessRate != 0)
				{
					num ^= SuccessRate.GetHashCode();
				}
				if (Status != 0)
				{
					num ^= Status.GetHashCode();
				}
				if (ShengYuTime != 0L)
				{
					num ^= ShengYuTime.GetHashCode();
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
				if (Id != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(Id);
				}
				if (GoodsName.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(GoodsName);
				}
				if (Stone != 0L)
				{
					output.WriteRawTag(24);
					output.WriteInt64(Stone);
				}
				if (SuccessRate != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(SuccessRate);
				}
				if (Status != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Status);
				}
				if (ShengYuTime != 0L)
				{
					output.WriteRawTag(48);
					output.WriteInt64(ShengYuTime);
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
				if (Id != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Id);
				}
				if (GoodsName.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(GoodsName);
				}
				if (Stone != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(Stone);
				}
				if (SuccessRate != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(SuccessRate);
				}
				if (Status != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Status);
				}
				if (ShengYuTime != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(ShengYuTime);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(XuanShangInfoResp_1015040 other)
			{
				if (other != null)
				{
					if (other.Id != 0)
					{
						Id = other.Id;
					}
					if (other.GoodsName.Length != 0)
					{
						GoodsName = other.GoodsName;
					}
					if (other.Stone != 0L)
					{
						Stone = other.Stone;
					}
					if (other.SuccessRate != 0)
					{
						SuccessRate = other.SuccessRate;
					}
					if (other.Status != 0)
					{
						Status = other.Status;
					}
					if (other.ShengYuTime != 0L)
					{
						ShengYuTime = other.ShengYuTime;
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
					case 8u:
						Id = input.ReadInt32();
						break;
					case 18u:
						GoodsName = input.ReadString();
						break;
					case 24u:
						Stone = input.ReadInt64();
						break;
					case 32u:
						SuccessRate = input.ReadInt32();
						break;
					case 40u:
						Status = input.ReadInt32();
						break;
					case 48u:
						ShengYuTime = input.ReadInt64();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<IntoWaiWuResp_1015038> _parser = new MessageParser<IntoWaiWuResp_1015038>(() => new IntoWaiWuResp_1015038());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ZhangLaoInfoFieldNumber = 3;

	private Types.ZhangLaoInfoResp_1015039 zhangLaoInfo_;

	public const int XuanShangInfoFieldNumber = 4;

	private static readonly FieldCodec<Types.XuanShangInfoResp_1015040> _repeated_xuanShangInfo_codec = FieldCodec.ForMessage(34u, Types.XuanShangInfoResp_1015040.Parser);

	private readonly RepeatedField<Types.XuanShangInfoResp_1015040> xuanShangInfo_ = new RepeatedField<Types.XuanShangInfoResp_1015040>();

	public const int CurrentContributionFieldNumber = 5;

	private long currentContribution_;

	[DebuggerNonUserCode]
	public static MessageParser<IntoWaiWuResp_1015038> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[47];

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
	public Types.ZhangLaoInfoResp_1015039 ZhangLaoInfo
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
	public RepeatedField<Types.XuanShangInfoResp_1015040> XuanShangInfo => xuanShangInfo_;

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public IntoWaiWuResp_1015038()
	{
	}

	[DebuggerNonUserCode]
	public IntoWaiWuResp_1015038(IntoWaiWuResp_1015038 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		zhangLaoInfo_ = ((other.zhangLaoInfo_ != null) ? other.zhangLaoInfo_.Clone() : null);
		xuanShangInfo_ = other.xuanShangInfo_.Clone();
		currentContribution_ = other.currentContribution_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IntoWaiWuResp_1015038 Clone()
	{
		return new IntoWaiWuResp_1015038(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IntoWaiWuResp_1015038);
	}

	[DebuggerNonUserCode]
	public bool Equals(IntoWaiWuResp_1015038 other)
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
		if (!xuanShangInfo_.Equals(other.xuanShangInfo_))
		{
			return false;
		}
		if (CurrentContribution != other.CurrentContribution)
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
		num ^= xuanShangInfo_.GetHashCode();
		if (CurrentContribution != 0L)
		{
			num ^= CurrentContribution.GetHashCode();
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
		xuanShangInfo_.WriteTo(output, _repeated_xuanShangInfo_codec);
		if (CurrentContribution != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(CurrentContribution);
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
		num += xuanShangInfo_.CalculateSize(_repeated_xuanShangInfo_codec);
		if (CurrentContribution != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentContribution);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(IntoWaiWuResp_1015038 other)
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
				ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015039();
			}
			ZhangLaoInfo.MergeFrom(other.ZhangLaoInfo);
		}
		xuanShangInfo_.Add(other.xuanShangInfo_);
		if (other.CurrentContribution != 0L)
		{
			CurrentContribution = other.CurrentContribution;
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
					ZhangLaoInfo = new Types.ZhangLaoInfoResp_1015039();
				}
				input.ReadMessage(ZhangLaoInfo);
				break;
			case 34u:
				xuanShangInfo_.AddEntriesFrom(input, _repeated_xuanShangInfo_codec);
				break;
			case 40u:
				CurrentContribution = input.ReadInt64();
				break;
			}
		}
	}
}
