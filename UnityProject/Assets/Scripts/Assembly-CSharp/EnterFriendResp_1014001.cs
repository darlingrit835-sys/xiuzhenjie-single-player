using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterFriendResp_1014001 : IMessage<EnterFriendResp_1014001>, IMessage, IEquatable<EnterFriendResp_1014001>, IDeepCloneable<EnterFriendResp_1014001>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class FriendResp_1014002 : IMessage<FriendResp_1014002>, IMessage, IEquatable<FriendResp_1014002>, IDeepCloneable<FriendResp_1014002>
		{
			private static readonly MessageParser<FriendResp_1014002> _parser = new MessageParser<FriendResp_1014002>(() => new FriendResp_1014002());

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

			public const int LiveTypeFieldNumber = 6;

			private static readonly FieldCodec<int> _repeated_liveType_codec = FieldCodec.ForInt32(50u);

			private readonly RepeatedField<int> liveType_ = new RepeatedField<int>();

			public const int SexFieldNumber = 7;

			private bool sex_;

			public const int IsOnLineFieldNumber = 8;

			private bool isOnLine_;

			[DebuggerNonUserCode]
			public static MessageParser<FriendResp_1014002> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterFriendResp_1014001.Descriptor.NestedTypes[0];

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
			public RepeatedField<int> LiveType => liveType_;

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

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public FriendResp_1014002()
			{
			}

			[DebuggerNonUserCode]
			public FriendResp_1014002(FriendResp_1014002 other)
				: this()
			{
				id_ = other.id_;
				name_ = other.name_;
				headPortraitId_ = other.headPortraitId_;
				level_ = other.level_;
				intimacy_ = other.intimacy_;
				liveType_ = other.liveType_.Clone();
				sex_ = other.sex_;
				isOnLine_ = other.isOnLine_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public FriendResp_1014002 Clone()
			{
				return new FriendResp_1014002(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as FriendResp_1014002);
			}

			[DebuggerNonUserCode]
			public bool Equals(FriendResp_1014002 other)
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
				if (!liveType_.Equals(other.liveType_))
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
				num ^= liveType_.GetHashCode();
				if (Sex)
				{
					num ^= Sex.GetHashCode();
				}
				if (IsOnLine)
				{
					num ^= IsOnLine.GetHashCode();
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
				liveType_.WriteTo(output, _repeated_liveType_codec);
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
				num += liveType_.CalculateSize(_repeated_liveType_codec);
				if (Sex)
				{
					num += 2;
				}
				if (IsOnLine)
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
			public void MergeFrom(FriendResp_1014002 other)
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
					liveType_.Add(other.liveType_);
					if (other.Sex)
					{
						Sex = other.Sex;
					}
					if (other.IsOnLine)
					{
						IsOnLine = other.IsOnLine;
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
					case 48u:
					case 50u:
						liveType_.AddEntriesFrom(input, _repeated_liveType_codec);
						break;
					case 56u:
						Sex = input.ReadBool();
						break;
					case 64u:
						IsOnLine = input.ReadBool();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterFriendResp_1014001> _parser = new MessageParser<EnterFriendResp_1014001>(() => new EnterFriendResp_1014001());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int FriendFieldNumber = 3;

	private static readonly FieldCodec<Types.FriendResp_1014002> _repeated_friend_codec = FieldCodec.ForMessage(26u, Types.FriendResp_1014002.Parser);

	private readonly RepeatedField<Types.FriendResp_1014002> friend_ = new RepeatedField<Types.FriendResp_1014002>();

	public const int StrangerCountFieldNumber = 4;

	private int strangerCount_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterFriendResp_1014001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[0];

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
	public RepeatedField<Types.FriendResp_1014002> Friend => friend_;

	[DebuggerNonUserCode]
	public int StrangerCount
	{
		get
		{
			return strangerCount_;
		}
		set
		{
			strangerCount_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterFriendResp_1014001()
	{
	}

	[DebuggerNonUserCode]
	public EnterFriendResp_1014001(EnterFriendResp_1014001 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		friend_ = other.friend_.Clone();
		strangerCount_ = other.strangerCount_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterFriendResp_1014001 Clone()
	{
		return new EnterFriendResp_1014001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterFriendResp_1014001);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterFriendResp_1014001 other)
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
		if (!friend_.Equals(other.friend_))
		{
			return false;
		}
		if (StrangerCount != other.StrangerCount)
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
		num ^= friend_.GetHashCode();
		if (StrangerCount != 0)
		{
			num ^= StrangerCount.GetHashCode();
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
		friend_.WriteTo(output, _repeated_friend_codec);
		if (StrangerCount != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(StrangerCount);
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
		num += friend_.CalculateSize(_repeated_friend_codec);
		if (StrangerCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(StrangerCount);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterFriendResp_1014001 other)
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
			friend_.Add(other.friend_);
			if (other.StrangerCount != 0)
			{
				StrangerCount = other.StrangerCount;
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
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 26u:
				friend_.AddEntriesFrom(input, _repeated_friend_codec);
				break;
			case 32u:
				StrangerCount = input.ReadInt32();
				break;
			}
		}
	}
}
