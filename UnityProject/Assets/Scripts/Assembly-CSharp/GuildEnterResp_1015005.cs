using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GuildEnterResp_1015005 : IMessage<GuildEnterResp_1015005>, IMessage, IEquatable<GuildEnterResp_1015005>, IDeepCloneable<GuildEnterResp_1015005>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class GuildHillResp_1015006 : IMessage<GuildHillResp_1015006>, IMessage, IEquatable<GuildHillResp_1015006>, IDeepCloneable<GuildHillResp_1015006>
		{
			private static readonly MessageParser<GuildHillResp_1015006> _parser = new MessageParser<GuildHillResp_1015006>(() => new GuildHillResp_1015006());

			private UnknownFieldSet _unknownFields;

			public const int TypeFieldNumber = 1;

			private int type_;

			public const int HillNameFieldNumber = 2;

			private string hillName_ = "";

			public const int StageFieldNumber = 3;

			private int stage_;

			[DebuggerNonUserCode]
			public static MessageParser<GuildHillResp_1015006> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => GuildEnterResp_1015005.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int Type
			{
				get
				{
					return type_;
				}
				set
				{
					type_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string HillName
			{
				get
				{
					return hillName_;
				}
				set
				{
					hillName_ = ProtoPreconditions.CheckNotNull(value, "value");
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

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public GuildHillResp_1015006()
			{
			}

			[DebuggerNonUserCode]
			public GuildHillResp_1015006(GuildHillResp_1015006 other)
				: this()
			{
				type_ = other.type_;
				hillName_ = other.hillName_;
				stage_ = other.stage_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public GuildHillResp_1015006 Clone()
			{
				return new GuildHillResp_1015006(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as GuildHillResp_1015006);
			}

			[DebuggerNonUserCode]
			public bool Equals(GuildHillResp_1015006 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (Type != other.Type)
				{
					return false;
				}
				if (HillName != other.HillName)
				{
					return false;
				}
				if (Stage != other.Stage)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (Type != 0)
				{
					num ^= Type.GetHashCode();
				}
				if (HillName.Length != 0)
				{
					num ^= HillName.GetHashCode();
				}
				if (Stage != 0)
				{
					num ^= Stage.GetHashCode();
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
				if (Type != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(Type);
				}
				if (HillName.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(HillName);
				}
				if (Stage != 0)
				{
					output.WriteRawTag(24);
					output.WriteInt32(Stage);
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
				if (Type != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Type);
				}
				if (HillName.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(HillName);
				}
				if (Stage != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(GuildHillResp_1015006 other)
			{
				if (other != null)
				{
					if (other.Type != 0)
					{
						Type = other.Type;
					}
					if (other.HillName.Length != 0)
					{
						HillName = other.HillName;
					}
					if (other.Stage != 0)
					{
						Stage = other.Stage;
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
						Type = input.ReadInt32();
						break;
					case 18u:
						HillName = input.ReadString();
						break;
					case 24u:
						Stage = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<GuildEnterResp_1015005> _parser = new MessageParser<GuildEnterResp_1015005>(() => new GuildEnterResp_1015005());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int IsGuildMasterFieldNumber = 3;

	private bool isGuildMaster_;

	public const int GuildHillsFieldNumber = 4;

	private static readonly FieldCodec<Types.GuildHillResp_1015006> _repeated_guildHills_codec = FieldCodec.ForMessage(34u, Types.GuildHillResp_1015006.Parser);

	private readonly RepeatedField<Types.GuildHillResp_1015006> guildHills_ = new RepeatedField<Types.GuildHillResp_1015006>();

	public const int FightingFieldNumber = 5;

	private long fighting_;

	public const int DefendFieldNumber = 6;

	private long defend_;

	public const int SectWarFieldNumber = 7;

	private long sectWar_;

	public const int BeastTideTimeFieldNumber = 8;

	private long beastTideTime_;

	public const int UpgradeGuildShengYuTimeFieldNumber = 9;

	private long upgradeGuildShengYuTime_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildEnterResp_1015005> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[6];

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
	public bool IsGuildMaster
	{
		get
		{
			return isGuildMaster_;
		}
		set
		{
			isGuildMaster_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.GuildHillResp_1015006> GuildHills => guildHills_;

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
	public long Defend
	{
		get
		{
			return defend_;
		}
		set
		{
			defend_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long SectWar
	{
		get
		{
			return sectWar_;
		}
		set
		{
			sectWar_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long BeastTideTime
	{
		get
		{
			return beastTideTime_;
		}
		set
		{
			beastTideTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long UpgradeGuildShengYuTime
	{
		get
		{
			return upgradeGuildShengYuTime_;
		}
		set
		{
			upgradeGuildShengYuTime_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildEnterResp_1015005()
	{
	}

	[DebuggerNonUserCode]
	public GuildEnterResp_1015005(GuildEnterResp_1015005 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		isGuildMaster_ = other.isGuildMaster_;
		guildHills_ = other.guildHills_.Clone();
		fighting_ = other.fighting_;
		defend_ = other.defend_;
		sectWar_ = other.sectWar_;
		beastTideTime_ = other.beastTideTime_;
		upgradeGuildShengYuTime_ = other.upgradeGuildShengYuTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildEnterResp_1015005 Clone()
	{
		return new GuildEnterResp_1015005(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildEnterResp_1015005);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildEnterResp_1015005 other)
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
		if (IsGuildMaster != other.IsGuildMaster)
		{
			return false;
		}
		if (!guildHills_.Equals(other.guildHills_))
		{
			return false;
		}
		if (Fighting != other.Fighting)
		{
			return false;
		}
		if (Defend != other.Defend)
		{
			return false;
		}
		if (SectWar != other.SectWar)
		{
			return false;
		}
		if (BeastTideTime != other.BeastTideTime)
		{
			return false;
		}
		if (UpgradeGuildShengYuTime != other.UpgradeGuildShengYuTime)
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
		if (IsGuildMaster)
		{
			num ^= IsGuildMaster.GetHashCode();
		}
		num ^= guildHills_.GetHashCode();
		if (Fighting != 0L)
		{
			num ^= Fighting.GetHashCode();
		}
		if (Defend != 0L)
		{
			num ^= Defend.GetHashCode();
		}
		if (SectWar != 0L)
		{
			num ^= SectWar.GetHashCode();
		}
		if (BeastTideTime != 0L)
		{
			num ^= BeastTideTime.GetHashCode();
		}
		if (UpgradeGuildShengYuTime != 0L)
		{
			num ^= UpgradeGuildShengYuTime.GetHashCode();
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
		if (IsGuildMaster)
		{
			output.WriteRawTag(24);
			output.WriteBool(IsGuildMaster);
		}
		guildHills_.WriteTo(output, _repeated_guildHills_codec);
		if (Fighting != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(Fighting);
		}
		if (Defend != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(Defend);
		}
		if (SectWar != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(SectWar);
		}
		if (BeastTideTime != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(BeastTideTime);
		}
		if (UpgradeGuildShengYuTime != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(UpgradeGuildShengYuTime);
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
		if (IsGuildMaster)
		{
			num += 2;
		}
		num += guildHills_.CalculateSize(_repeated_guildHills_codec);
		if (Fighting != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Fighting);
		}
		if (Defend != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Defend);
		}
		if (SectWar != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(SectWar);
		}
		if (BeastTideTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(BeastTideTime);
		}
		if (UpgradeGuildShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UpgradeGuildShengYuTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildEnterResp_1015005 other)
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
			if (other.IsGuildMaster)
			{
				IsGuildMaster = other.IsGuildMaster;
			}
			guildHills_.Add(other.guildHills_);
			if (other.Fighting != 0L)
			{
				Fighting = other.Fighting;
			}
			if (other.Defend != 0L)
			{
				Defend = other.Defend;
			}
			if (other.SectWar != 0L)
			{
				SectWar = other.SectWar;
			}
			if (other.BeastTideTime != 0L)
			{
				BeastTideTime = other.BeastTideTime;
			}
			if (other.UpgradeGuildShengYuTime != 0L)
			{
				UpgradeGuildShengYuTime = other.UpgradeGuildShengYuTime;
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
			case 24u:
				IsGuildMaster = input.ReadBool();
				break;
			case 34u:
				guildHills_.AddEntriesFrom(input, _repeated_guildHills_codec);
				break;
			case 40u:
				Fighting = input.ReadInt64();
				break;
			case 48u:
				Defend = input.ReadInt64();
				break;
			case 56u:
				SectWar = input.ReadInt64();
				break;
			case 64u:
				BeastTideTime = input.ReadInt64();
				break;
			case 72u:
				UpgradeGuildShengYuTime = input.ReadInt64();
				break;
			}
		}
	}
}
