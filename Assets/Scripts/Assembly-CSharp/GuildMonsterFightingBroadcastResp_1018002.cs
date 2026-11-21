using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GuildMonsterFightingBroadcastResp_1018002 : IMessage<GuildMonsterFightingBroadcastResp_1018002>, IMessage, IEquatable<GuildMonsterFightingBroadcastResp_1018002>, IDeepCloneable<GuildMonsterFightingBroadcastResp_1018002>
{
	private static readonly MessageParser<GuildMonsterFightingBroadcastResp_1018002> _parser = new MessageParser<GuildMonsterFightingBroadcastResp_1018002>(() => new GuildMonsterFightingBroadcastResp_1018002());

	private UnknownFieldSet _unknownFields;

	public const int GuildIdFieldNumber = 1;

	private int guildId_;

	public const int DefensesFieldNumber = 2;

	private static readonly FieldCodec<int> _repeated_defenses_codec = FieldCodec.ForInt32(18u);

	private readonly RepeatedField<int> defenses_ = new RepeatedField<int>();

	public const int GuildAttackPowerFieldNumber = 3;

	private int guildAttackPower_;

	public const int GuildDefensesFieldNumber = 4;

	private int guildDefenses_;

	public const int TimesFieldNumber = 5;

	private int times_;

	public const int MonsterPowersFieldNumber = 6;

	private static readonly FieldCodec<int> _repeated_monsterPowers_codec = FieldCodec.ForInt32(50u);

	private readonly RepeatedField<int> monsterPowers_ = new RepeatedField<int>();

	public const int IsFinishedFieldNumber = 7;

	private bool isFinished_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildMonsterFightingBroadcastResp_1018002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildFightProtoReflection.Descriptor.MessageTypes[2];

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
	public RepeatedField<int> Defenses => defenses_;

	[DebuggerNonUserCode]
	public int GuildAttackPower
	{
		get
		{
			return guildAttackPower_;
		}
		set
		{
			guildAttackPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int GuildDefenses
	{
		get
		{
			return guildDefenses_;
		}
		set
		{
			guildDefenses_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Times
	{
		get
		{
			return times_;
		}
		set
		{
			times_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<int> MonsterPowers => monsterPowers_;

	[DebuggerNonUserCode]
	public bool IsFinished
	{
		get
		{
			return isFinished_;
		}
		set
		{
			isFinished_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightingBroadcastResp_1018002()
	{
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightingBroadcastResp_1018002(GuildMonsterFightingBroadcastResp_1018002 other)
		: this()
	{
		guildId_ = other.guildId_;
		defenses_ = other.defenses_.Clone();
		guildAttackPower_ = other.guildAttackPower_;
		guildDefenses_ = other.guildDefenses_;
		times_ = other.times_;
		monsterPowers_ = other.monsterPowers_.Clone();
		isFinished_ = other.isFinished_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightingBroadcastResp_1018002 Clone()
	{
		return new GuildMonsterFightingBroadcastResp_1018002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildMonsterFightingBroadcastResp_1018002);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildMonsterFightingBroadcastResp_1018002 other)
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
		if (!defenses_.Equals(other.defenses_))
		{
			return false;
		}
		if (GuildAttackPower != other.GuildAttackPower)
		{
			return false;
		}
		if (GuildDefenses != other.GuildDefenses)
		{
			return false;
		}
		if (Times != other.Times)
		{
			return false;
		}
		if (!monsterPowers_.Equals(other.monsterPowers_))
		{
			return false;
		}
		if (IsFinished != other.IsFinished)
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
		num ^= defenses_.GetHashCode();
		if (GuildAttackPower != 0)
		{
			num ^= GuildAttackPower.GetHashCode();
		}
		if (GuildDefenses != 0)
		{
			num ^= GuildDefenses.GetHashCode();
		}
		if (Times != 0)
		{
			num ^= Times.GetHashCode();
		}
		num ^= monsterPowers_.GetHashCode();
		if (IsFinished)
		{
			num ^= IsFinished.GetHashCode();
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
		defenses_.WriteTo(output, _repeated_defenses_codec);
		if (GuildAttackPower != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(GuildAttackPower);
		}
		if (GuildDefenses != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(GuildDefenses);
		}
		if (Times != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Times);
		}
		monsterPowers_.WriteTo(output, _repeated_monsterPowers_codec);
		if (IsFinished)
		{
			output.WriteRawTag(56);
			output.WriteBool(IsFinished);
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
		num += defenses_.CalculateSize(_repeated_defenses_codec);
		if (GuildAttackPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GuildAttackPower);
		}
		if (GuildDefenses != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GuildDefenses);
		}
		if (Times != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Times);
		}
		num += monsterPowers_.CalculateSize(_repeated_monsterPowers_codec);
		if (IsFinished)
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
	public void MergeFrom(GuildMonsterFightingBroadcastResp_1018002 other)
	{
		if (other != null)
		{
			if (other.GuildId != 0)
			{
				GuildId = other.GuildId;
			}
			defenses_.Add(other.defenses_);
			if (other.GuildAttackPower != 0)
			{
				GuildAttackPower = other.GuildAttackPower;
			}
			if (other.GuildDefenses != 0)
			{
				GuildDefenses = other.GuildDefenses;
			}
			if (other.Times != 0)
			{
				Times = other.Times;
			}
			monsterPowers_.Add(other.monsterPowers_);
			if (other.IsFinished)
			{
				IsFinished = other.IsFinished;
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
			case 16u:
			case 18u:
				defenses_.AddEntriesFrom(input, _repeated_defenses_codec);
				break;
			case 24u:
				GuildAttackPower = input.ReadInt32();
				break;
			case 32u:
				GuildDefenses = input.ReadInt32();
				break;
			case 40u:
				Times = input.ReadInt32();
				break;
			case 48u:
			case 50u:
				monsterPowers_.AddEntriesFrom(input, _repeated_monsterPowers_codec);
				break;
			case 56u:
				IsFinished = input.ReadBool();
				break;
			}
		}
	}
}
