using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class BaiShiTieResp_1014018 : IMessage<BaiShiTieResp_1014018>, IMessage, IEquatable<BaiShiTieResp_1014018>, IDeepCloneable<BaiShiTieResp_1014018>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class BaiShiDiZiResp_1014019 : IMessage<BaiShiDiZiResp_1014019>, IMessage, IEquatable<BaiShiDiZiResp_1014019>, IDeepCloneable<BaiShiDiZiResp_1014019>
		{
			private static readonly MessageParser<BaiShiDiZiResp_1014019> _parser = new MessageParser<BaiShiDiZiResp_1014019>(() => new BaiShiDiZiResp_1014019());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int HeadPortraitIdFieldNumber = 3;

			private string headPortraitId_ = "";

			public const int LevelFieldNumber = 4;

			private int level_;

			public const int SexFieldNumber = 7;

			private bool sex_;

			[DebuggerNonUserCode]
			public static MessageParser<BaiShiDiZiResp_1014019> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => BaiShiTieResp_1014018.Descriptor.NestedTypes[0];

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

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public BaiShiDiZiResp_1014019()
			{
			}

			[DebuggerNonUserCode]
			public BaiShiDiZiResp_1014019(BaiShiDiZiResp_1014019 other)
				: this()
			{
				id_ = other.id_;
				name_ = other.name_;
				headPortraitId_ = other.headPortraitId_;
				level_ = other.level_;
				sex_ = other.sex_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public BaiShiDiZiResp_1014019 Clone()
			{
				return new BaiShiDiZiResp_1014019(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as BaiShiDiZiResp_1014019);
			}

			[DebuggerNonUserCode]
			public bool Equals(BaiShiDiZiResp_1014019 other)
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
				if (Sex != other.Sex)
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
				if (Sex)
				{
					num ^= Sex.GetHashCode();
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
				if (Sex)
				{
					output.WriteRawTag(56);
					output.WriteBool(Sex);
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
				if (Sex)
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
			public void MergeFrom(BaiShiDiZiResp_1014019 other)
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
					if (other.Sex)
					{
						Sex = other.Sex;
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
					case 56u:
						Sex = input.ReadBool();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<BaiShiTieResp_1014018> _parser = new MessageParser<BaiShiTieResp_1014018>(() => new BaiShiTieResp_1014018());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int BaiShiDiZisFieldNumber = 3;

	private static readonly FieldCodec<Types.BaiShiDiZiResp_1014019> _repeated_baiShiDiZis_codec = FieldCodec.ForMessage(26u, Types.BaiShiDiZiResp_1014019.Parser);

	private readonly RepeatedField<Types.BaiShiDiZiResp_1014019> baiShiDiZis_ = new RepeatedField<Types.BaiShiDiZiResp_1014019>();

	[DebuggerNonUserCode]
	public static MessageParser<BaiShiTieResp_1014018> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[20];

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
	public RepeatedField<Types.BaiShiDiZiResp_1014019> BaiShiDiZis => baiShiDiZis_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BaiShiTieResp_1014018()
	{
	}

	[DebuggerNonUserCode]
	public BaiShiTieResp_1014018(BaiShiTieResp_1014018 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		baiShiDiZis_ = other.baiShiDiZis_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BaiShiTieResp_1014018 Clone()
	{
		return new BaiShiTieResp_1014018(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BaiShiTieResp_1014018);
	}

	[DebuggerNonUserCode]
	public bool Equals(BaiShiTieResp_1014018 other)
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
		if (!baiShiDiZis_.Equals(other.baiShiDiZis_))
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
		num ^= baiShiDiZis_.GetHashCode();
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
		baiShiDiZis_.WriteTo(output, _repeated_baiShiDiZis_codec);
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
		num += baiShiDiZis_.CalculateSize(_repeated_baiShiDiZis_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BaiShiTieResp_1014018 other)
	{
		if (other != null)
		{
			if (other.Code != 0)
			{
				Code = other.Code;
			}
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
			baiShiDiZis_.Add(other.baiShiDiZis_);
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
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 26u:
				baiShiDiZis_.AddEntriesFrom(input, _repeated_baiShiDiZis_codec);
				break;
			}
		}
	}
}
