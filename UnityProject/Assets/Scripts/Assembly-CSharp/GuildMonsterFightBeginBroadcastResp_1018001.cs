using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GuildMonsterFightBeginBroadcastResp_1018001 : IMessage<GuildMonsterFightBeginBroadcastResp_1018001>, IMessage, IEquatable<GuildMonsterFightBeginBroadcastResp_1018001>, IDeepCloneable<GuildMonsterFightBeginBroadcastResp_1018001>
{
	private static readonly MessageParser<GuildMonsterFightBeginBroadcastResp_1018001> _parser = new MessageParser<GuildMonsterFightBeginBroadcastResp_1018001>(() => new GuildMonsterFightBeginBroadcastResp_1018001());

	private UnknownFieldSet _unknownFields;

	public const int GuildIdFieldNumber = 1;

	private int guildId_;

	public const int DefensesFieldNumber = 2;

	private static readonly FieldCodec<int> _repeated_defenses_codec = FieldCodec.ForInt32(18u);

	private readonly RepeatedField<int> defenses_ = new RepeatedField<int>();

	public const int GuildAttackPowerFieldNumber = 3;

	private int guildAttackPower_;

	public const int RoleAttackPowerFieldNumber = 4;

	private int roleAttackPower_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildMonsterFightBeginBroadcastResp_1018001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildFightProtoReflection.Descriptor.MessageTypes[0];

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
	public int RoleAttackPower
	{
		get
		{
			return roleAttackPower_;
		}
		set
		{
			roleAttackPower_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightBeginBroadcastResp_1018001()
	{
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightBeginBroadcastResp_1018001(GuildMonsterFightBeginBroadcastResp_1018001 other)
		: this()
	{
		guildId_ = other.guildId_;
		defenses_ = other.defenses_.Clone();
		guildAttackPower_ = other.guildAttackPower_;
		roleAttackPower_ = other.roleAttackPower_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightBeginBroadcastResp_1018001 Clone()
	{
		return new GuildMonsterFightBeginBroadcastResp_1018001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildMonsterFightBeginBroadcastResp_1018001);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildMonsterFightBeginBroadcastResp_1018001 other)
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
		if (RoleAttackPower != other.RoleAttackPower)
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
		if (RoleAttackPower != 0)
		{
			num ^= RoleAttackPower.GetHashCode();
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
		if (RoleAttackPower != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(RoleAttackPower);
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
		if (RoleAttackPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RoleAttackPower);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildMonsterFightBeginBroadcastResp_1018001 other)
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
			if (other.RoleAttackPower != 0)
			{
				RoleAttackPower = other.RoleAttackPower;
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
				RoleAttackPower = input.ReadInt32();
				break;
			}
		}
	}
}
