using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterShiTuResp_1014012 : IMessage<EnterShiTuResp_1014012>, IMessage, IEquatable<EnterShiTuResp_1014012>, IDeepCloneable<EnterShiTuResp_1014012>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ShiFuResp_1014013 : IMessage<ShiFuResp_1014013>, IMessage, IEquatable<ShiFuResp_1014013>, IDeepCloneable<ShiFuResp_1014013>
		{
			private static readonly MessageParser<ShiFuResp_1014013> _parser = new MessageParser<ShiFuResp_1014013>(() => new ShiFuResp_1014013());

			private UnknownFieldSet _unknownFields;

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int HeadPortraitIdFieldNumber = 3;

			private string headPortraitId_ = "";

			public const int LevelFieldNumber = 4;

			private int level_;

			public const int IntimacyFieldNumber = 5;

			private int intimacy_;

			public const int SexFieldNumber = 7;

			private bool sex_;

			public const int IsOnLineFieldNumber = 8;

			private bool isOnLine_;

			public const int IsQingAnFieldNumber = 9;

			private bool isQingAn_;

			[DebuggerNonUserCode]
			public static MessageParser<ShiFuResp_1014013> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterShiTuResp_1014012.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
			public string HeadPortraitId
			{
				get
				{
					return headPortraitId_;
				}
				set
				{
					headPortraitId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
			public int Intimacy
			{
				get
				{
					return intimacy_;
				}
				set
				{
					intimacy_ = value;
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
			public bool IsOnLine
			{
				get
				{
					return isOnLine_;
				}
				set
				{
					isOnLine_ = value;
				}
			}

			[DebuggerNonUserCode]
			public bool IsQingAn
			{
				get
				{
					return isQingAn_;
				}
				set
				{
					isQingAn_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public ShiFuResp_1014013()
			{
			}

			[DebuggerNonUserCode]
			public ShiFuResp_1014013(ShiFuResp_1014013 other)
				: this()
			{
				name_ = other.name_;
				headPortraitId_ = other.headPortraitId_;
				level_ = other.level_;
				intimacy_ = other.intimacy_;
				sex_ = other.sex_;
				isOnLine_ = other.isOnLine_;
				isQingAn_ = other.isQingAn_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ShiFuResp_1014013 Clone()
			{
				return new ShiFuResp_1014013(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ShiFuResp_1014013);
			}

			[DebuggerNonUserCode]
			public bool Equals(ShiFuResp_1014013 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (Name != other.Name)
				{
					return false;
				}
				if (HeadPortraitId != other.HeadPortraitId)
				{
					return false;
				}
				if (Level != other.Level)
				{
					return false;
				}
				if (Intimacy != other.Intimacy)
				{
					return false;
				}
				if (Sex != other.Sex)
				{
					return false;
				}
				if (IsOnLine != other.IsOnLine)
				{
					return false;
				}
				if (IsQingAn != other.IsQingAn)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (HeadPortraitId.Length != 0)
				{
					num ^= HeadPortraitId.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
				}
				if (Intimacy != 0)
				{
					num ^= Intimacy.GetHashCode();
				}
				if (Sex)
				{
					num ^= Sex.GetHashCode();
				}
				if (IsOnLine)
				{
					num ^= IsOnLine.GetHashCode();
				}
				if (IsQingAn)
				{
					num ^= IsQingAn.GetHashCode();
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
				if (Name.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(Name);
				}
				if (HeadPortraitId.Length != 0)
				{
					output.WriteRawTag(26);
					output.WriteString(HeadPortraitId);
				}
				if (Level != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Level);
				}
				if (Intimacy != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Intimacy);
				}
				if (Sex)
				{
					output.WriteRawTag(56);
					output.WriteBool(Sex);
				}
				if (IsOnLine)
				{
					output.WriteRawTag(64);
					output.WriteBool(IsOnLine);
				}
				if (IsQingAn)
				{
					output.WriteRawTag(72);
					output.WriteBool(IsQingAn);
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
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (HeadPortraitId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(HeadPortraitId);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (Intimacy != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Intimacy);
				}
				if (Sex)
				{
					num += 2;
				}
				if (IsOnLine)
				{
					num += 2;
				}
				if (IsQingAn)
				{
					num += 2;
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(ShiFuResp_1014013 other)
			{
				if (other != null)
				{
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.HeadPortraitId.Length != 0)
					{
						HeadPortraitId = other.HeadPortraitId;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
					}
					if (other.Intimacy != 0)
					{
						Intimacy = other.Intimacy;
					}
					if (other.Sex)
					{
						Sex = other.Sex;
					}
					if (other.IsOnLine)
					{
						IsOnLine = other.IsOnLine;
					}
					if (other.IsQingAn)
					{
						IsQingAn = other.IsQingAn;
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
					case 18u:
						Name = input.ReadString();
						break;
					case 26u:
						HeadPortraitId = input.ReadString();
						break;
					case 32u:
						Level = input.ReadInt32();
						break;
					case 40u:
						Intimacy = input.ReadInt32();
						break;
					case 56u:
						Sex = input.ReadBool();
						break;
					case 64u:
						IsOnLine = input.ReadBool();
						break;
					case 72u:
						IsQingAn = input.ReadBool();
						break;
					}
				}
			}
		}

		public sealed class TuDiResp_1014014 : IMessage<TuDiResp_1014014>, IMessage, IEquatable<TuDiResp_1014014>, IDeepCloneable<TuDiResp_1014014>
		{
			private static readonly MessageParser<TuDiResp_1014014> _parser = new MessageParser<TuDiResp_1014014>(() => new TuDiResp_1014014());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int HeadPortraitIdFieldNumber = 3;

			private string headPortraitId_ = "";

			public const int LevelFieldNumber = 4;

			private int level_;

			public const int IntimacyFieldNumber = 5;

			private int intimacy_;

			public const int SexFieldNumber = 7;

			private bool sex_;

			public const int IsOnLineFieldNumber = 8;

			private bool isOnLine_;

			public const int IsTongMaiOrChuanGongFieldNumber = 9;

			private bool isTongMaiOrChuanGong_;

			[DebuggerNonUserCode]
			public static MessageParser<TuDiResp_1014014> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterShiTuResp_1014012.Descriptor.NestedTypes[1];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public string Id
			{
				get
				{
					return id_;
				}
				set
				{
					id_ = ProtoPreconditions.CheckNotNull(value, "value");
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
			public string HeadPortraitId
			{
				get
				{
					return headPortraitId_;
				}
				set
				{
					headPortraitId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
			public int Intimacy
			{
				get
				{
					return intimacy_;
				}
				set
				{
					intimacy_ = value;
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
			public bool IsOnLine
			{
				get
				{
					return isOnLine_;
				}
				set
				{
					isOnLine_ = value;
				}
			}

			[DebuggerNonUserCode]
			public bool IsTongMaiOrChuanGong
			{
				get
				{
					return isTongMaiOrChuanGong_;
				}
				set
				{
					isTongMaiOrChuanGong_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public TuDiResp_1014014()
			{
			}

			[DebuggerNonUserCode]
			public TuDiResp_1014014(TuDiResp_1014014 other)
				: this()
			{
				id_ = other.id_;
				name_ = other.name_;
				headPortraitId_ = other.headPortraitId_;
				level_ = other.level_;
				intimacy_ = other.intimacy_;
				sex_ = other.sex_;
				isOnLine_ = other.isOnLine_;
				isTongMaiOrChuanGong_ = other.isTongMaiOrChuanGong_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public TuDiResp_1014014 Clone()
			{
				return new TuDiResp_1014014(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as TuDiResp_1014014);
			}

			[DebuggerNonUserCode]
			public bool Equals(TuDiResp_1014014 other)
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
				if (Name != other.Name)
				{
					return false;
				}
				if (HeadPortraitId != other.HeadPortraitId)
				{
					return false;
				}
				if (Level != other.Level)
				{
					return false;
				}
				if (Intimacy != other.Intimacy)
				{
					return false;
				}
				if (Sex != other.Sex)
				{
					return false;
				}
				if (IsOnLine != other.IsOnLine)
				{
					return false;
				}
				if (IsTongMaiOrChuanGong != other.IsTongMaiOrChuanGong)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (Id.Length != 0)
				{
					num ^= Id.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (HeadPortraitId.Length != 0)
				{
					num ^= HeadPortraitId.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
				}
				if (Intimacy != 0)
				{
					num ^= Intimacy.GetHashCode();
				}
				if (Sex)
				{
					num ^= Sex.GetHashCode();
				}
				if (IsOnLine)
				{
					num ^= IsOnLine.GetHashCode();
				}
				if (IsTongMaiOrChuanGong)
				{
					num ^= IsTongMaiOrChuanGong.GetHashCode();
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
				if (Id.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(Id);
				}
				if (Name.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(Name);
				}
				if (HeadPortraitId.Length != 0)
				{
					output.WriteRawTag(26);
					output.WriteString(HeadPortraitId);
				}
				if (Level != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Level);
				}
				if (Intimacy != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Intimacy);
				}
				if (Sex)
				{
					output.WriteRawTag(56);
					output.WriteBool(Sex);
				}
				if (IsOnLine)
				{
					output.WriteRawTag(64);
					output.WriteBool(IsOnLine);
				}
				if (IsTongMaiOrChuanGong)
				{
					output.WriteRawTag(72);
					output.WriteBool(IsTongMaiOrChuanGong);
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
				if (Id.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Id);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (HeadPortraitId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(HeadPortraitId);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (Intimacy != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Intimacy);
				}
				if (Sex)
				{
					num += 2;
				}
				if (IsOnLine)
				{
					num += 2;
				}
				if (IsTongMaiOrChuanGong)
				{
					num += 2;
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(TuDiResp_1014014 other)
			{
				if (other != null)
				{
					if (other.Id.Length != 0)
					{
						Id = other.Id;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.HeadPortraitId.Length != 0)
					{
						HeadPortraitId = other.HeadPortraitId;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
					}
					if (other.Intimacy != 0)
					{
						Intimacy = other.Intimacy;
					}
					if (other.Sex)
					{
						Sex = other.Sex;
					}
					if (other.IsOnLine)
					{
						IsOnLine = other.IsOnLine;
					}
					if (other.IsTongMaiOrChuanGong)
					{
						IsTongMaiOrChuanGong = other.IsTongMaiOrChuanGong;
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
						Id = input.ReadString();
						break;
					case 18u:
						Name = input.ReadString();
						break;
					case 26u:
						HeadPortraitId = input.ReadString();
						break;
					case 32u:
						Level = input.ReadInt32();
						break;
					case 40u:
						Intimacy = input.ReadInt32();
						break;
					case 56u:
						Sex = input.ReadBool();
						break;
					case 64u:
						IsOnLine = input.ReadBool();
						break;
					case 72u:
						IsTongMaiOrChuanGong = input.ReadBool();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterShiTuResp_1014012> _parser = new MessageParser<EnterShiTuResp_1014012>(() => new EnterShiTuResp_1014012());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ShiFuFieldNumber = 3;

	private Types.ShiFuResp_1014013 shiFu_;

	public const int TuDisFieldNumber = 4;

	private static readonly FieldCodec<Types.TuDiResp_1014014> _repeated_tuDis_codec = FieldCodec.ForMessage(34u, Types.TuDiResp_1014014.Parser);

	private readonly RepeatedField<Types.TuDiResp_1014014> tuDis_ = new RepeatedField<Types.TuDiResp_1014014>();

	public const int ChuShiNumFieldNumber = 5;

	private int chuShiNum_;

	public const int QinChuanNumFieldNumber = 6;

	private int qinChuanNum_;

	public const int JiMingNumFieldNumber = 7;

	private int jiMingNum_;

	public const int BaiShiTieNumFieldNumber = 8;

	private int baiShiTieNum_;

	public const int IsGetTmcgFieldNumber = 9;

	private bool isGetTmcg_;

	public const int TmcgTypeFieldNumber = 10;

	private int tmcgType_;

	public const int TmcgNumFieldNumber = 11;

	private int tmcgNum_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterShiTuResp_1014012> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[14];

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
	public Types.ShiFuResp_1014013 ShiFu
	{
		get
		{
			return shiFu_;
		}
		set
		{
			shiFu_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.TuDiResp_1014014> TuDis => tuDis_;

	[DebuggerNonUserCode]
	public int ChuShiNum
	{
		get
		{
			return chuShiNum_;
		}
		set
		{
			chuShiNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int QinChuanNum
	{
		get
		{
			return qinChuanNum_;
		}
		set
		{
			qinChuanNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int JiMingNum
	{
		get
		{
			return jiMingNum_;
		}
		set
		{
			jiMingNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BaiShiTieNum
	{
		get
		{
			return baiShiTieNum_;
		}
		set
		{
			baiShiTieNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsGetTmcg
	{
		get
		{
			return isGetTmcg_;
		}
		set
		{
			isGetTmcg_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TmcgType
	{
		get
		{
			return tmcgType_;
		}
		set
		{
			tmcgType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TmcgNum
	{
		get
		{
			return tmcgNum_;
		}
		set
		{
			tmcgNum_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterShiTuResp_1014012()
	{
	}

	[DebuggerNonUserCode]
	public EnterShiTuResp_1014012(EnterShiTuResp_1014012 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		shiFu_ = ((other.shiFu_ != null) ? other.shiFu_.Clone() : null);
		tuDis_ = other.tuDis_.Clone();
		chuShiNum_ = other.chuShiNum_;
		qinChuanNum_ = other.qinChuanNum_;
		jiMingNum_ = other.jiMingNum_;
		baiShiTieNum_ = other.baiShiTieNum_;
		isGetTmcg_ = other.isGetTmcg_;
		tmcgType_ = other.tmcgType_;
		tmcgNum_ = other.tmcgNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterShiTuResp_1014012 Clone()
	{
		return new EnterShiTuResp_1014012(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterShiTuResp_1014012);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterShiTuResp_1014012 other)
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
		if (!object.Equals(ShiFu, other.ShiFu))
		{
			return false;
		}
		if (!tuDis_.Equals(other.tuDis_))
		{
			return false;
		}
		if (ChuShiNum != other.ChuShiNum)
		{
			return false;
		}
		if (QinChuanNum != other.QinChuanNum)
		{
			return false;
		}
		if (JiMingNum != other.JiMingNum)
		{
			return false;
		}
		if (BaiShiTieNum != other.BaiShiTieNum)
		{
			return false;
		}
		if (IsGetTmcg != other.IsGetTmcg)
		{
			return false;
		}
		if (TmcgType != other.TmcgType)
		{
			return false;
		}
		if (TmcgNum != other.TmcgNum)
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
		if (shiFu_ != null)
		{
			num ^= ShiFu.GetHashCode();
		}
		num ^= tuDis_.GetHashCode();
		if (ChuShiNum != 0)
		{
			num ^= ChuShiNum.GetHashCode();
		}
		if (QinChuanNum != 0)
		{
			num ^= QinChuanNum.GetHashCode();
		}
		if (JiMingNum != 0)
		{
			num ^= JiMingNum.GetHashCode();
		}
		if (BaiShiTieNum != 0)
		{
			num ^= BaiShiTieNum.GetHashCode();
		}
		if (IsGetTmcg)
		{
			num ^= IsGetTmcg.GetHashCode();
		}
		if (TmcgType != 0)
		{
			num ^= TmcgType.GetHashCode();
		}
		if (TmcgNum != 0)
		{
			num ^= TmcgNum.GetHashCode();
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
		if (shiFu_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(ShiFu);
		}
		tuDis_.WriteTo(output, _repeated_tuDis_codec);
		if (ChuShiNum != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(ChuShiNum);
		}
		if (QinChuanNum != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(QinChuanNum);
		}
		if (JiMingNum != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(JiMingNum);
		}
		if (BaiShiTieNum != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(BaiShiTieNum);
		}
		if (IsGetTmcg)
		{
			output.WriteRawTag(72);
			output.WriteBool(IsGetTmcg);
		}
		if (TmcgType != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(TmcgType);
		}
		if (TmcgNum != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(TmcgNum);
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
		if (shiFu_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ShiFu);
		}
		num += tuDis_.CalculateSize(_repeated_tuDis_codec);
		if (ChuShiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ChuShiNum);
		}
		if (QinChuanNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(QinChuanNum);
		}
		if (JiMingNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(JiMingNum);
		}
		if (BaiShiTieNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BaiShiTieNum);
		}
		if (IsGetTmcg)
		{
			num += 2;
		}
		if (TmcgType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TmcgType);
		}
		if (TmcgNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TmcgNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterShiTuResp_1014012 other)
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
		if (other.shiFu_ != null)
		{
			if (shiFu_ == null)
			{
				ShiFu = new Types.ShiFuResp_1014013();
			}
			ShiFu.MergeFrom(other.ShiFu);
		}
		tuDis_.Add(other.tuDis_);
		if (other.ChuShiNum != 0)
		{
			ChuShiNum = other.ChuShiNum;
		}
		if (other.QinChuanNum != 0)
		{
			QinChuanNum = other.QinChuanNum;
		}
		if (other.JiMingNum != 0)
		{
			JiMingNum = other.JiMingNum;
		}
		if (other.BaiShiTieNum != 0)
		{
			BaiShiTieNum = other.BaiShiTieNum;
		}
		if (other.IsGetTmcg)
		{
			IsGetTmcg = other.IsGetTmcg;
		}
		if (other.TmcgType != 0)
		{
			TmcgType = other.TmcgType;
		}
		if (other.TmcgNum != 0)
		{
			TmcgNum = other.TmcgNum;
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
				if (shiFu_ == null)
				{
					ShiFu = new Types.ShiFuResp_1014013();
				}
				input.ReadMessage(ShiFu);
				break;
			case 34u:
				tuDis_.AddEntriesFrom(input, _repeated_tuDis_codec);
				break;
			case 40u:
				ChuShiNum = input.ReadInt32();
				break;
			case 48u:
				QinChuanNum = input.ReadInt32();
				break;
			case 56u:
				JiMingNum = input.ReadInt32();
				break;
			case 64u:
				BaiShiTieNum = input.ReadInt32();
				break;
			case 72u:
				IsGetTmcg = input.ReadBool();
				break;
			case 80u:
				TmcgType = input.ReadInt32();
				break;
			case 88u:
				TmcgNum = input.ReadInt32();
				break;
			}
		}
	}
}
