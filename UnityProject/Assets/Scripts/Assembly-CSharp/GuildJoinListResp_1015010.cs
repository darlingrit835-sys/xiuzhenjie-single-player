using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GuildJoinListResp_1015010 : IMessage<GuildJoinListResp_1015010>, IMessage, IEquatable<GuildJoinListResp_1015010>, IDeepCloneable<GuildJoinListResp_1015010>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class GuildRoleInfoResp_1015011 : IMessage<GuildRoleInfoResp_1015011>, IMessage, IEquatable<GuildRoleInfoResp_1015011>, IDeepCloneable<GuildRoleInfoResp_1015011>
		{
			private static readonly MessageParser<GuildRoleInfoResp_1015011> _parser = new MessageParser<GuildRoleInfoResp_1015011>(() => new GuildRoleInfoResp_1015011());

			private UnknownFieldSet _unknownFields;

			public const int RoleIdFieldNumber = 1;

			private string roleId_ = "";

			public const int RoleImageFieldNumber = 2;

			private string roleImage_ = "";

			public const int RoleNameFieldNumber = 3;

			private string roleName_ = "";

			public const int RoleAgeFieldNumber = 4;

			private int roleAge_;

			public const int Profession1FieldNumber = 5;

			private int profession1_;

			public const int Profession2FieldNumber = 6;

			private int profession2_;

			public const int LevelFieldNumber = 7;

			private int level_;

			public const int SexFieldNumber = 8;

			private bool sex_;

			[DebuggerNonUserCode]
			public static MessageParser<GuildRoleInfoResp_1015011> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => GuildJoinListResp_1015010.Descriptor.NestedTypes[0];

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
			public int RoleAge
			{
				get
				{
					return roleAge_;
				}
				set
				{
					roleAge_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int Profession1
			{
				get
				{
					return profession1_;
				}
				set
				{
					profession1_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int Profession2
			{
				get
				{
					return profession2_;
				}
				set
				{
					profession2_ = value;
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
			public GuildRoleInfoResp_1015011()
			{
			}

			[DebuggerNonUserCode]
			public GuildRoleInfoResp_1015011(GuildRoleInfoResp_1015011 other)
				: this()
			{
				roleId_ = other.roleId_;
				roleImage_ = other.roleImage_;
				roleName_ = other.roleName_;
				roleAge_ = other.roleAge_;
				profession1_ = other.profession1_;
				profession2_ = other.profession2_;
				level_ = other.level_;
				sex_ = other.sex_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public GuildRoleInfoResp_1015011 Clone()
			{
				return new GuildRoleInfoResp_1015011(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as GuildRoleInfoResp_1015011);
			}

			[DebuggerNonUserCode]
			public bool Equals(GuildRoleInfoResp_1015011 other)
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
				if (RoleAge != other.RoleAge)
				{
					return false;
				}
				if (Profession1 != other.Profession1)
				{
					return false;
				}
				if (Profession2 != other.Profession2)
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
				if (RoleAge != 0)
				{
					num ^= RoleAge.GetHashCode();
				}
				if (Profession1 != 0)
				{
					num ^= Profession1.GetHashCode();
				}
				if (Profession2 != 0)
				{
					num ^= Profession2.GetHashCode();
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
				if (RoleAge != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(RoleAge);
				}
				if (Profession1 != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Profession1);
				}
				if (Profession2 != 0)
				{
					output.WriteRawTag(48);
					output.WriteInt32(Profession2);
				}
				if (Level != 0)
				{
					output.WriteRawTag(56);
					output.WriteInt32(Level);
				}
				if (Sex)
				{
					output.WriteRawTag(64);
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
				if (RoleAge != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(RoleAge);
				}
				if (Profession1 != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Profession1);
				}
				if (Profession2 != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Profession2);
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
			public void MergeFrom(GuildRoleInfoResp_1015011 other)
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
					if (other.RoleAge != 0)
					{
						RoleAge = other.RoleAge;
					}
					if (other.Profession1 != 0)
					{
						Profession1 = other.Profession1;
					}
					if (other.Profession2 != 0)
					{
						Profession2 = other.Profession2;
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
						RoleId = input.ReadString();
						break;
					case 18u:
						RoleImage = input.ReadString();
						break;
					case 26u:
						RoleName = input.ReadString();
						break;
					case 32u:
						RoleAge = input.ReadInt32();
						break;
					case 40u:
						Profession1 = input.ReadInt32();
						break;
					case 48u:
						Profession2 = input.ReadInt32();
						break;
					case 56u:
						Level = input.ReadInt32();
						break;
					case 64u:
						Sex = input.ReadBool();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<GuildJoinListResp_1015010> _parser = new MessageParser<GuildJoinListResp_1015010>(() => new GuildJoinListResp_1015010());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int JoinListFieldNumber = 3;

	private static readonly FieldCodec<Types.GuildRoleInfoResp_1015011> _repeated_joinList_codec = FieldCodec.ForMessage(26u, Types.GuildRoleInfoResp_1015011.Parser);

	private readonly RepeatedField<Types.GuildRoleInfoResp_1015011> joinList_ = new RepeatedField<Types.GuildRoleInfoResp_1015011>();

	[DebuggerNonUserCode]
	public static MessageParser<GuildJoinListResp_1015010> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[10];

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
	public RepeatedField<Types.GuildRoleInfoResp_1015011> JoinList => joinList_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildJoinListResp_1015010()
	{
	}

	[DebuggerNonUserCode]
	public GuildJoinListResp_1015010(GuildJoinListResp_1015010 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		joinList_ = other.joinList_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildJoinListResp_1015010 Clone()
	{
		return new GuildJoinListResp_1015010(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildJoinListResp_1015010);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildJoinListResp_1015010 other)
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
		if (!joinList_.Equals(other.joinList_))
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
		num ^= joinList_.GetHashCode();
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
		joinList_.WriteTo(output, _repeated_joinList_codec);
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
		num += joinList_.CalculateSize(_repeated_joinList_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildJoinListResp_1015010 other)
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
			joinList_.Add(other.joinList_);
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
				joinList_.AddEntriesFrom(input, _repeated_joinList_codec);
				break;
			}
		}
	}
}
