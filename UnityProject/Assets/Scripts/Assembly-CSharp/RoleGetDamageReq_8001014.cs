using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RoleGetDamageReq_8001014 : IMessage<RoleGetDamageReq_8001014>, IMessage, IEquatable<RoleGetDamageReq_8001014>, IDeepCloneable<RoleGetDamageReq_8001014>
{
	private static readonly MessageParser<RoleGetDamageReq_8001014> _parser = new MessageParser<RoleGetDamageReq_8001014>(() => new RoleGetDamageReq_8001014());

	private UnknownFieldSet _unknownFields;

	public const int BeforeFieldNumber = 1;

	private long before_;

	public const int AfterFieldNumber = 2;

	private long after_;

	public const int DamageFieldNumber = 3;

	private long damage_;

	public const int RoleIndexFieldNumber = 4;

	private int roleIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<RoleGetDamageReq_8001014> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[13];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public long Before
	{
		get
		{
			return before_;
		}
		set
		{
			before_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long After
	{
		get
		{
			return after_;
		}
		set
		{
			after_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Damage
	{
		get
		{
			return damage_;
		}
		set
		{
			damage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RoleIndex
	{
		get
		{
			return roleIndex_;
		}
		set
		{
			roleIndex_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RoleGetDamageReq_8001014()
	{
	}

	[DebuggerNonUserCode]
	public RoleGetDamageReq_8001014(RoleGetDamageReq_8001014 other)
		: this()
	{
		before_ = other.before_;
		after_ = other.after_;
		damage_ = other.damage_;
		roleIndex_ = other.roleIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoleGetDamageReq_8001014 Clone()
	{
		return new RoleGetDamageReq_8001014(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoleGetDamageReq_8001014);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoleGetDamageReq_8001014 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Before != other.Before)
		{
			return false;
		}
		if (After != other.After)
		{
			return false;
		}
		if (Damage != other.Damage)
		{
			return false;
		}
		if (RoleIndex != other.RoleIndex)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Before != 0L)
		{
			num ^= Before.GetHashCode();
		}
		if (After != 0L)
		{
			num ^= After.GetHashCode();
		}
		if (Damage != 0L)
		{
			num ^= Damage.GetHashCode();
		}
		if (RoleIndex != 0)
		{
			num ^= RoleIndex.GetHashCode();
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
		if (Before != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(Before);
		}
		if (After != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(After);
		}
		if (Damage != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(Damage);
		}
		if (RoleIndex != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(RoleIndex);
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
		if (Before != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Before);
		}
		if (After != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(After);
		}
		if (Damage != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Damage);
		}
		if (RoleIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RoleIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RoleGetDamageReq_8001014 other)
	{
		if (other != null)
		{
			if (other.Before != 0L)
			{
				Before = other.Before;
			}
			if (other.After != 0L)
			{
				After = other.After;
			}
			if (other.Damage != 0L)
			{
				Damage = other.Damage;
			}
			if (other.RoleIndex != 0)
			{
				RoleIndex = other.RoleIndex;
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
				Before = input.ReadInt64();
				break;
			case 16u:
				After = input.ReadInt64();
				break;
			case 24u:
				Damage = input.ReadInt64();
				break;
			case 32u:
				RoleIndex = input.ReadInt32();
				break;
			}
		}
	}
}
