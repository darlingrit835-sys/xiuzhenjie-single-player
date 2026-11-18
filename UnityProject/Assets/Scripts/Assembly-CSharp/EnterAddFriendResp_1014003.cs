using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterAddFriendResp_1014003 : IMessage<EnterAddFriendResp_1014003>, IMessage, IEquatable<EnterAddFriendResp_1014003>, IDeepCloneable<EnterAddFriendResp_1014003>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class StrangerResp_1014004 : IMessage<StrangerResp_1014004>, IMessage, IEquatable<StrangerResp_1014004>, IDeepCloneable<StrangerResp_1014004>
		{
			private static readonly MessageParser<StrangerResp_1014004> _parser = new MessageParser<StrangerResp_1014004>(() => new StrangerResp_1014004());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int HeadPortraitIdFieldNumber = 3;

			private string headPortraitId_ = "";

			public const int LevelFieldNumber = 4;

			private int level_;

			public const int LiveTypeFieldNumber = 5;

			private static readonly FieldCodec<int> _repeated_liveType_codec = FieldCodec.ForInt32(42u);

			private readonly RepeatedField<int> liveType_ = new RepeatedField<int>();

			public const int SexFieldNumber = 6;

			private bool sex_;

			public const int IsOnLineFieldNumber = 7;

			private bool isOnLine_;

			[DebuggerNonUserCode]
			public static MessageParser<StrangerResp_1014004> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterAddFriendResp_1014003.Descriptor.NestedTypes[0];

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
			public StrangerResp_1014004()
			{
			}

			[DebuggerNonUserCode]
			public StrangerResp_1014004(StrangerResp_1014004 other)
				: this()
			{
				id_ = other.id_;
				name_ = other.name_;
				headPortraitId_ = other.headPortraitId_;
				level_ = other.level_;
				liveType_ = other.liveType_.Clone();
				sex_ = other.sex_;
				isOnLine_ = other.isOnLine_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public StrangerResp_1014004 Clone()
			{
				return new StrangerResp_1014004(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as StrangerResp_1014004);
			}

			[DebuggerNonUserCode]
			public bool Equals(StrangerResp_1014004 other)
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
				liveType_.WriteTo(output, _repeated_liveType_codec);
				if (Sex)
				{
					output.WriteRawTag(48);
					output.WriteBool(Sex);
				}
				if (IsOnLine)
				{
					output.WriteRawTag(56);
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
			public void MergeFrom(StrangerResp_1014004 other)
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
					case 42u:
						liveType_.AddEntriesFrom(input, _repeated_liveType_codec);
						break;
					case 48u:
						Sex = input.ReadBool();
						break;
					case 56u:
						IsOnLine = input.ReadBool();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterAddFriendResp_1014003> _parser = new MessageParser<EnterAddFriendResp_1014003>(() => new EnterAddFriendResp_1014003());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int StrangerFieldNumber = 3;

	private static readonly FieldCodec<Types.StrangerResp_1014004> _repeated_stranger_codec = FieldCodec.ForMessage(26u, Types.StrangerResp_1014004.Parser);

	private readonly RepeatedField<Types.StrangerResp_1014004> stranger_ = new RepeatedField<Types.StrangerResp_1014004>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterAddFriendResp_1014003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[1];

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
	public RepeatedField<Types.StrangerResp_1014004> Stranger => stranger_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterAddFriendResp_1014003()
	{
	}

	[DebuggerNonUserCode]
	public EnterAddFriendResp_1014003(EnterAddFriendResp_1014003 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		stranger_ = other.stranger_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterAddFriendResp_1014003 Clone()
	{
		return new EnterAddFriendResp_1014003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterAddFriendResp_1014003);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterAddFriendResp_1014003 other)
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
		if (!stranger_.Equals(other.stranger_))
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
		num ^= stranger_.GetHashCode();
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
		stranger_.WriteTo(output, _repeated_stranger_codec);
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
		num += stranger_.CalculateSize(_repeated_stranger_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterAddFriendResp_1014003 other)
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
			stranger_.Add(other.stranger_);
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
				stranger_.AddEntriesFrom(input, _repeated_stranger_codec);
				break;
			}
		}
	}
}
