using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GuildListResp_1015001 : IMessage<GuildListResp_1015001>, IMessage, IEquatable<GuildListResp_1015001>, IDeepCloneable<GuildListResp_1015001>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class GuildResp_1015020 : IMessage<GuildResp_1015020>, IMessage, IEquatable<GuildResp_1015020>, IDeepCloneable<GuildResp_1015020>
		{
			private static readonly MessageParser<GuildResp_1015020> _parser = new MessageParser<GuildResp_1015020>(() => new GuildResp_1015020());

			private UnknownFieldSet _unknownFields;

			public const int GuildIdFieldNumber = 1;

			private int guildId_;

			public const int NameFieldNumber = 2;

			private string name_ = "";

			[DebuggerNonUserCode]
			public static MessageParser<GuildResp_1015020> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => GuildListResp_1015001.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int GuildId
			{
				get
				{
					return guildId_;
				}
				set
				{
					guildId_ = value;
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

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public GuildResp_1015020()
			{
			}

			[DebuggerNonUserCode]
			public GuildResp_1015020(GuildResp_1015020 other)
				: this()
			{
				guildId_ = other.guildId_;
				name_ = other.name_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public GuildResp_1015020 Clone()
			{
				return new GuildResp_1015020(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as GuildResp_1015020);
			}

			[DebuggerNonUserCode]
			public bool Equals(GuildResp_1015020 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (GuildId != other.GuildId)
				{
					return false;
				}
				if (Name != other.Name)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (GuildId != 0)
				{
					num ^= GuildId.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
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
				if (GuildId != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(GuildId);
				}
				if (Name.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(Name);
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
				if (GuildId != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(GuildId);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(GuildResp_1015020 other)
			{
				if (other != null)
				{
					if (other.GuildId != 0)
					{
						GuildId = other.GuildId;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
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
						GuildId = input.ReadInt32();
						break;
					case 18u:
						Name = input.ReadString();
						break;
					}
				}
			}
		}

		public sealed class SelfGuildResp_1015002 : IMessage<SelfGuildResp_1015002>, IMessage, IEquatable<SelfGuildResp_1015002>, IDeepCloneable<SelfGuildResp_1015002>
		{
			private static readonly MessageParser<SelfGuildResp_1015002> _parser = new MessageParser<SelfGuildResp_1015002>(() => new SelfGuildResp_1015002());

			private UnknownFieldSet _unknownFields;

			public const int GuildIdFieldNumber = 1;

			private int guildId_;

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int IsCreatedGuildFieldNumber = 3;

			private bool isCreatedGuild_;

			public const int MonsterNumFieldNumber = 4;

			private int monsterNum_;

			public const int RemainMonsterCountFieldNumber = 5;

			private int remainMonsterCount_;

			public const int GuildMapIdFieldNumber = 6;

			private string guildMapId_ = "";

			[DebuggerNonUserCode]
			public static MessageParser<SelfGuildResp_1015002> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => GuildListResp_1015001.Descriptor.NestedTypes[1];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int GuildId
			{
				get
				{
					return guildId_;
				}
				set
				{
					guildId_ = value;
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
			public bool IsCreatedGuild
			{
				get
				{
					return isCreatedGuild_;
				}
				set
				{
					isCreatedGuild_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int MonsterNum
			{
				get
				{
					return monsterNum_;
				}
				set
				{
					monsterNum_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int RemainMonsterCount
			{
				get
				{
					return remainMonsterCount_;
				}
				set
				{
					remainMonsterCount_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string GuildMapId
			{
				get
				{
					return guildMapId_;
				}
				set
				{
					guildMapId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public SelfGuildResp_1015002()
			{
			}

			[DebuggerNonUserCode]
			public SelfGuildResp_1015002(SelfGuildResp_1015002 other)
				: this()
			{
				guildId_ = other.guildId_;
				name_ = other.name_;
				isCreatedGuild_ = other.isCreatedGuild_;
				monsterNum_ = other.monsterNum_;
				remainMonsterCount_ = other.remainMonsterCount_;
				guildMapId_ = other.guildMapId_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public SelfGuildResp_1015002 Clone()
			{
				return new SelfGuildResp_1015002(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as SelfGuildResp_1015002);
			}

			[DebuggerNonUserCode]
			public bool Equals(SelfGuildResp_1015002 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (GuildId != other.GuildId)
				{
					return false;
				}
				if (Name != other.Name)
				{
					return false;
				}
				if (IsCreatedGuild != other.IsCreatedGuild)
				{
					return false;
				}
				if (MonsterNum != other.MonsterNum)
				{
					return false;
				}
				if (RemainMonsterCount != other.RemainMonsterCount)
				{
					return false;
				}
				if (GuildMapId != other.GuildMapId)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (GuildId != 0)
				{
					num ^= GuildId.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (IsCreatedGuild)
				{
					num ^= IsCreatedGuild.GetHashCode();
				}
				if (MonsterNum != 0)
				{
					num ^= MonsterNum.GetHashCode();
				}
				if (RemainMonsterCount != 0)
				{
					num ^= RemainMonsterCount.GetHashCode();
				}
				if (GuildMapId.Length != 0)
				{
					num ^= GuildMapId.GetHashCode();
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
				if (GuildId != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(GuildId);
				}
				if (Name.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(Name);
				}
				if (IsCreatedGuild)
				{
					output.WriteRawTag(24);
					output.WriteBool(IsCreatedGuild);
				}
				if (MonsterNum != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(MonsterNum);
				}
				if (RemainMonsterCount != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(RemainMonsterCount);
				}
				if (GuildMapId.Length != 0)
				{
					output.WriteRawTag(50);
					output.WriteString(GuildMapId);
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
				if (GuildId != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(GuildId);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (IsCreatedGuild)
				{
					num += 2;
				}
				if (MonsterNum != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(MonsterNum);
				}
				if (RemainMonsterCount != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(RemainMonsterCount);
				}
				if (GuildMapId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(GuildMapId);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(SelfGuildResp_1015002 other)
			{
				if (other != null)
				{
					if (other.GuildId != 0)
					{
						GuildId = other.GuildId;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.IsCreatedGuild)
					{
						IsCreatedGuild = other.IsCreatedGuild;
					}
					if (other.MonsterNum != 0)
					{
						MonsterNum = other.MonsterNum;
					}
					if (other.RemainMonsterCount != 0)
					{
						RemainMonsterCount = other.RemainMonsterCount;
					}
					if (other.GuildMapId.Length != 0)
					{
						GuildMapId = other.GuildMapId;
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
						GuildId = input.ReadInt32();
						break;
					case 18u:
						Name = input.ReadString();
						break;
					case 24u:
						IsCreatedGuild = input.ReadBool();
						break;
					case 32u:
						MonsterNum = input.ReadInt32();
						break;
					case 40u:
						RemainMonsterCount = input.ReadInt32();
						break;
					case 50u:
						GuildMapId = input.ReadString();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<GuildListResp_1015001> _parser = new MessageParser<GuildListResp_1015001>(() => new GuildListResp_1015001());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int GuildListFieldNumber = 3;

	private static readonly FieldCodec<Types.GuildResp_1015020> _repeated_guildList_codec = FieldCodec.ForMessage(26u, Types.GuildResp_1015020.Parser);

	private readonly RepeatedField<Types.GuildResp_1015020> guildList_ = new RepeatedField<Types.GuildResp_1015020>();

	public const int SelfGuildFieldNumber = 4;

	private Types.SelfGuildResp_1015002 selfGuild_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildListResp_1015001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[1];

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
	public RepeatedField<Types.GuildResp_1015020> GuildList => guildList_;

	[DebuggerNonUserCode]
	public Types.SelfGuildResp_1015002 SelfGuild
	{
		get
		{
			return selfGuild_;
		}
		set
		{
			selfGuild_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildListResp_1015001()
	{
	}

	[DebuggerNonUserCode]
	public GuildListResp_1015001(GuildListResp_1015001 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		guildList_ = other.guildList_.Clone();
		selfGuild_ = ((other.selfGuild_ != null) ? other.selfGuild_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildListResp_1015001 Clone()
	{
		return new GuildListResp_1015001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildListResp_1015001);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildListResp_1015001 other)
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
		if (!guildList_.Equals(other.guildList_))
		{
			return false;
		}
		if (!object.Equals(SelfGuild, other.SelfGuild))
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
		num ^= guildList_.GetHashCode();
		if (selfGuild_ != null)
		{
			num ^= SelfGuild.GetHashCode();
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
		guildList_.WriteTo(output, _repeated_guildList_codec);
		if (selfGuild_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(SelfGuild);
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
		num += guildList_.CalculateSize(_repeated_guildList_codec);
		if (selfGuild_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SelfGuild);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildListResp_1015001 other)
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
		guildList_.Add(other.guildList_);
		if (other.selfGuild_ != null)
		{
			if (selfGuild_ == null)
			{
				SelfGuild = new Types.SelfGuildResp_1015002();
			}
			SelfGuild.MergeFrom(other.SelfGuild);
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
				guildList_.AddEntriesFrom(input, _repeated_guildList_codec);
				break;
			case 34u:
				if (selfGuild_ == null)
				{
					SelfGuild = new Types.SelfGuildResp_1015002();
				}
				input.ReadMessage(SelfGuild);
				break;
			}
		}
	}
}
