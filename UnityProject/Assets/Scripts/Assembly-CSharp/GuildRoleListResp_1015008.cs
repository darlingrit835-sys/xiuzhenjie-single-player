using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GuildRoleListResp_1015008 : IMessage<GuildRoleListResp_1015008>, IMessage, IEquatable<GuildRoleListResp_1015008>, IDeepCloneable<GuildRoleListResp_1015008>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class GuildRoleInfoResp_1015009 : IMessage<GuildRoleInfoResp_1015009>, IMessage, IEquatable<GuildRoleInfoResp_1015009>, IDeepCloneable<GuildRoleInfoResp_1015009>
		{
			private static readonly MessageParser<GuildRoleInfoResp_1015009> _parser = new MessageParser<GuildRoleInfoResp_1015009>(() => new GuildRoleInfoResp_1015009());

			private UnknownFieldSet _unknownFields;

			public const int RoleIdFieldNumber = 1;

			private string roleId_ = "";

			public const int RoleImageFieldNumber = 2;

			private string roleImage_ = "";

			public const int RoleNameFieldNumber = 3;

			private string roleName_ = "";

			public const int Profession1FieldNumber = 4;

			private int profession1_;

			public const int Profession2FieldNumber = 5;

			private int profession2_;

			public const int ZhanDouLiFieldNumber = 6;

			private long zhanDouLi_;

			public const int LevelFieldNumber = 7;

			private int level_;

			public const int SexFieldNumber = 8;

			private bool sex_;

			public const int IsOnLineFieldNumber = 9;

			private bool isOnLine_;

			public const int ZhiWeiFieldNumber = 10;

			private int zhiWei_;

			public const int Profession1StageFieldNumber = 11;

			private int profession1Stage_;

			public const int Profession2StageFieldNumber = 12;

			private int profession2Stage_;

			public const int PlantLiveStageFieldNumber = 13;

			private int plantLiveStage_;

			[DebuggerNonUserCode]
			public static MessageParser<GuildRoleInfoResp_1015009> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => GuildRoleListResp_1015008.Descriptor.NestedTypes[0];

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
			public long ZhanDouLi
			{
				get
				{
					return zhanDouLi_;
				}
				set
				{
					zhanDouLi_ = value;
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
			public int ZhiWei
			{
				get
				{
					return zhiWei_;
				}
				set
				{
					zhiWei_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int Profession1Stage
			{
				get
				{
					return profession1Stage_;
				}
				set
				{
					profession1Stage_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int Profession2Stage
			{
				get
				{
					return profession2Stage_;
				}
				set
				{
					profession2Stage_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int PlantLiveStage
			{
				get
				{
					return plantLiveStage_;
				}
				set
				{
					plantLiveStage_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public GuildRoleInfoResp_1015009()
			{
			}

			[DebuggerNonUserCode]
			public GuildRoleInfoResp_1015009(GuildRoleInfoResp_1015009 other)
				: this()
			{
				roleId_ = other.roleId_;
				roleImage_ = other.roleImage_;
				roleName_ = other.roleName_;
				profession1_ = other.profession1_;
				profession2_ = other.profession2_;
				zhanDouLi_ = other.zhanDouLi_;
				level_ = other.level_;
				sex_ = other.sex_;
				isOnLine_ = other.isOnLine_;
				zhiWei_ = other.zhiWei_;
				profession1Stage_ = other.profession1Stage_;
				profession2Stage_ = other.profession2Stage_;
				plantLiveStage_ = other.plantLiveStage_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public GuildRoleInfoResp_1015009 Clone()
			{
				return new GuildRoleInfoResp_1015009(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as GuildRoleInfoResp_1015009);
			}

			[DebuggerNonUserCode]
			public bool Equals(GuildRoleInfoResp_1015009 other)
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
				if (Profession1 != other.Profession1)
				{
					return false;
				}
				if (Profession2 != other.Profession2)
				{
					return false;
				}
				if (ZhanDouLi != other.ZhanDouLi)
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
				if (IsOnLine != other.IsOnLine)
				{
					return false;
				}
				if (ZhiWei != other.ZhiWei)
				{
					return false;
				}
				if (Profession1Stage != other.Profession1Stage)
				{
					return false;
				}
				if (Profession2Stage != other.Profession2Stage)
				{
					return false;
				}
				if (PlantLiveStage != other.PlantLiveStage)
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
				if (Profession1 != 0)
				{
					num ^= Profession1.GetHashCode();
				}
				if (Profession2 != 0)
				{
					num ^= Profession2.GetHashCode();
				}
				if (ZhanDouLi != 0L)
				{
					num ^= ZhanDouLi.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
				}
				if (Sex)
				{
					num ^= Sex.GetHashCode();
				}
				if (IsOnLine)
				{
					num ^= IsOnLine.GetHashCode();
				}
				if (ZhiWei != 0)
				{
					num ^= ZhiWei.GetHashCode();
				}
				if (Profession1Stage != 0)
				{
					num ^= Profession1Stage.GetHashCode();
				}
				if (Profession2Stage != 0)
				{
					num ^= Profession2Stage.GetHashCode();
				}
				if (PlantLiveStage != 0)
				{
					num ^= PlantLiveStage.GetHashCode();
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
				if (Profession1 != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Profession1);
				}
				if (Profession2 != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Profession2);
				}
				if (ZhanDouLi != 0L)
				{
					output.WriteRawTag(48);
					output.WriteInt64(ZhanDouLi);
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
				if (IsOnLine)
				{
					output.WriteRawTag(72);
					output.WriteBool(IsOnLine);
				}
				if (ZhiWei != 0)
				{
					output.WriteRawTag(80);
					output.WriteInt32(ZhiWei);
				}
				if (Profession1Stage != 0)
				{
					output.WriteRawTag(88);
					output.WriteInt32(Profession1Stage);
				}
				if (Profession2Stage != 0)
				{
					output.WriteRawTag(96);
					output.WriteInt32(Profession2Stage);
				}
				if (PlantLiveStage != 0)
				{
					output.WriteRawTag(104);
					output.WriteInt32(PlantLiveStage);
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
				if (Profession1 != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Profession1);
				}
				if (Profession2 != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Profession2);
				}
				if (ZhanDouLi != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(ZhanDouLi);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (Sex)
				{
					num += 2;
				}
				if (IsOnLine)
				{
					num += 2;
				}
				if (ZhiWei != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(ZhiWei);
				}
				if (Profession1Stage != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Profession1Stage);
				}
				if (Profession2Stage != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Profession2Stage);
				}
				if (PlantLiveStage != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(PlantLiveStage);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(GuildRoleInfoResp_1015009 other)
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
					if (other.Profession1 != 0)
					{
						Profession1 = other.Profession1;
					}
					if (other.Profession2 != 0)
					{
						Profession2 = other.Profession2;
					}
					if (other.ZhanDouLi != 0L)
					{
						ZhanDouLi = other.ZhanDouLi;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
					}
					if (other.Sex)
					{
						Sex = other.Sex;
					}
					if (other.IsOnLine)
					{
						IsOnLine = other.IsOnLine;
					}
					if (other.ZhiWei != 0)
					{
						ZhiWei = other.ZhiWei;
					}
					if (other.Profession1Stage != 0)
					{
						Profession1Stage = other.Profession1Stage;
					}
					if (other.Profession2Stage != 0)
					{
						Profession2Stage = other.Profession2Stage;
					}
					if (other.PlantLiveStage != 0)
					{
						PlantLiveStage = other.PlantLiveStage;
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
						Profession1 = input.ReadInt32();
						break;
					case 40u:
						Profession2 = input.ReadInt32();
						break;
					case 48u:
						ZhanDouLi = input.ReadInt64();
						break;
					case 56u:
						Level = input.ReadInt32();
						break;
					case 64u:
						Sex = input.ReadBool();
						break;
					case 72u:
						IsOnLine = input.ReadBool();
						break;
					case 80u:
						ZhiWei = input.ReadInt32();
						break;
					case 88u:
						Profession1Stage = input.ReadInt32();
						break;
					case 96u:
						Profession2Stage = input.ReadInt32();
						break;
					case 104u:
						PlantLiveStage = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<GuildRoleListResp_1015008> _parser = new MessageParser<GuildRoleListResp_1015008>(() => new GuildRoleListResp_1015008());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int RoleListFieldNumber = 3;

	private static readonly FieldCodec<Types.GuildRoleInfoResp_1015009> _repeated_roleList_codec = FieldCodec.ForMessage(26u, Types.GuildRoleInfoResp_1015009.Parser);

	private readonly RepeatedField<Types.GuildRoleInfoResp_1015009> roleList_ = new RepeatedField<Types.GuildRoleInfoResp_1015009>();

	[DebuggerNonUserCode]
	public static MessageParser<GuildRoleListResp_1015008> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[9];

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
	public RepeatedField<Types.GuildRoleInfoResp_1015009> RoleList => roleList_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildRoleListResp_1015008()
	{
	}

	[DebuggerNonUserCode]
	public GuildRoleListResp_1015008(GuildRoleListResp_1015008 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		roleList_ = other.roleList_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildRoleListResp_1015008 Clone()
	{
		return new GuildRoleListResp_1015008(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildRoleListResp_1015008);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildRoleListResp_1015008 other)
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
		if (!roleList_.Equals(other.roleList_))
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
		num ^= roleList_.GetHashCode();
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
		roleList_.WriteTo(output, _repeated_roleList_codec);
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
		num += roleList_.CalculateSize(_repeated_roleList_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildRoleListResp_1015008 other)
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
			roleList_.Add(other.roleList_);
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
				roleList_.AddEntriesFrom(input, _repeated_roleList_codec);
				break;
			}
		}
	}
}
