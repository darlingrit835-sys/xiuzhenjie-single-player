using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class BattleRoleMoveReq_8001004 : IMessage<BattleRoleMoveReq_8001004>, IMessage, IEquatable<BattleRoleMoveReq_8001004>, IDeepCloneable<BattleRoleMoveReq_8001004>
{
	private static readonly MessageParser<BattleRoleMoveReq_8001004> _parser = new MessageParser<BattleRoleMoveReq_8001004>(() => new BattleRoleMoveReq_8001004());

	private UnknownFieldSet _unknownFields;

	public const int RoleIndexFieldNumber = 1;

	private int roleIndex_;

	public const int PosXFieldNumber = 2;

	private double posX_;

	public const int PosYFieldNumber = 3;

	private double posY_;

	public const int AngleFieldNumber = 4;

	private double angle_;

	[DebuggerNonUserCode]
	public static MessageParser<BattleRoleMoveReq_8001004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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

	[DebuggerNonUserCode]
	public double PosX
	{
		get
		{
			return posX_;
		}
		set
		{
			posX_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double PosY
	{
		get
		{
			return posY_;
		}
		set
		{
			posY_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Angle
	{
		get
		{
			return angle_;
		}
		set
		{
			angle_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BattleRoleMoveReq_8001004()
	{
	}

	[DebuggerNonUserCode]
	public BattleRoleMoveReq_8001004(BattleRoleMoveReq_8001004 other)
		: this()
	{
		roleIndex_ = other.roleIndex_;
		posX_ = other.posX_;
		posY_ = other.posY_;
		angle_ = other.angle_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleRoleMoveReq_8001004 Clone()
	{
		return new BattleRoleMoveReq_8001004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleRoleMoveReq_8001004);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleRoleMoveReq_8001004 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (RoleIndex != other.RoleIndex)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PosX, other.PosX))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PosY, other.PosY))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Angle, other.Angle))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (RoleIndex != 0)
		{
			num ^= RoleIndex.GetHashCode();
		}
		if (PosX != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosX);
		}
		if (PosY != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosY);
		}
		if (Angle != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Angle);
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
		if (RoleIndex != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(RoleIndex);
		}
		if (PosX != 0.0)
		{
			output.WriteRawTag(17);
			output.WriteDouble(PosX);
		}
		if (PosY != 0.0)
		{
			output.WriteRawTag(25);
			output.WriteDouble(PosY);
		}
		if (Angle != 0.0)
		{
			output.WriteRawTag(33);
			output.WriteDouble(Angle);
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
		if (RoleIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RoleIndex);
		}
		if (PosX != 0.0)
		{
			num += 9;
		}
		if (PosY != 0.0)
		{
			num += 9;
		}
		if (Angle != 0.0)
		{
			num += 9;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleRoleMoveReq_8001004 other)
	{
		if (other != null)
		{
			if (other.RoleIndex != 0)
			{
				RoleIndex = other.RoleIndex;
			}
			if (other.PosX != 0.0)
			{
				PosX = other.PosX;
			}
			if (other.PosY != 0.0)
			{
				PosY = other.PosY;
			}
			if (other.Angle != 0.0)
			{
				Angle = other.Angle;
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
				RoleIndex = input.ReadInt32();
				break;
			case 17u:
				PosX = input.ReadDouble();
				break;
			case 25u:
				PosY = input.ReadDouble();
				break;
			case 33u:
				Angle = input.ReadDouble();
				break;
			}
		}
	}
}
