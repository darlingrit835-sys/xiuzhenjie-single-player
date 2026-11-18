using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class NeiWuSalarySettingReq_1015153 : IMessage<NeiWuSalarySettingReq_1015153>, IMessage, IEquatable<NeiWuSalarySettingReq_1015153>, IDeepCloneable<NeiWuSalarySettingReq_1015153>
{
	private static readonly MessageParser<NeiWuSalarySettingReq_1015153> _parser = new MessageParser<NeiWuSalarySettingReq_1015153>(() => new NeiWuSalarySettingReq_1015153());

	private UnknownFieldSet _unknownFields;

	public const int StoneSalaryFieldNumber = 1;

	private long stoneSalary_;

	public const int JadeSalaryFieldNumber = 2;

	private int jadeSalary_;

	public const int MatrixSalaryFieldNumber = 3;

	private int matrixSalary_;

	public const int ElixirSalaryFieldNumber = 4;

	private int elixirSalary_;

	public const int EquipmentSalaryFieldNumber = 5;

	private int equipmentSalary_;

	public const int SignSalaryFieldNumber = 6;

	private int signSalary_;

	[DebuggerNonUserCode]
	public static MessageParser<NeiWuSalarySettingReq_1015153> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[181];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public long StoneSalary
	{
		get
		{
			return stoneSalary_;
		}
		set
		{
			stoneSalary_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int JadeSalary
	{
		get
		{
			return jadeSalary_;
		}
		set
		{
			jadeSalary_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MatrixSalary
	{
		get
		{
			return matrixSalary_;
		}
		set
		{
			matrixSalary_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ElixirSalary
	{
		get
		{
			return elixirSalary_;
		}
		set
		{
			elixirSalary_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EquipmentSalary
	{
		get
		{
			return equipmentSalary_;
		}
		set
		{
			equipmentSalary_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SignSalary
	{
		get
		{
			return signSalary_;
		}
		set
		{
			signSalary_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public NeiWuSalarySettingReq_1015153()
	{
	}

	[DebuggerNonUserCode]
	public NeiWuSalarySettingReq_1015153(NeiWuSalarySettingReq_1015153 other)
		: this()
	{
		stoneSalary_ = other.stoneSalary_;
		jadeSalary_ = other.jadeSalary_;
		matrixSalary_ = other.matrixSalary_;
		elixirSalary_ = other.elixirSalary_;
		equipmentSalary_ = other.equipmentSalary_;
		signSalary_ = other.signSalary_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public NeiWuSalarySettingReq_1015153 Clone()
	{
		return new NeiWuSalarySettingReq_1015153(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as NeiWuSalarySettingReq_1015153);
	}

	[DebuggerNonUserCode]
	public bool Equals(NeiWuSalarySettingReq_1015153 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (StoneSalary != other.StoneSalary)
		{
			return false;
		}
		if (JadeSalary != other.JadeSalary)
		{
			return false;
		}
		if (MatrixSalary != other.MatrixSalary)
		{
			return false;
		}
		if (ElixirSalary != other.ElixirSalary)
		{
			return false;
		}
		if (EquipmentSalary != other.EquipmentSalary)
		{
			return false;
		}
		if (SignSalary != other.SignSalary)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (StoneSalary != 0L)
		{
			num ^= StoneSalary.GetHashCode();
		}
		if (JadeSalary != 0)
		{
			num ^= JadeSalary.GetHashCode();
		}
		if (MatrixSalary != 0)
		{
			num ^= MatrixSalary.GetHashCode();
		}
		if (ElixirSalary != 0)
		{
			num ^= ElixirSalary.GetHashCode();
		}
		if (EquipmentSalary != 0)
		{
			num ^= EquipmentSalary.GetHashCode();
		}
		if (SignSalary != 0)
		{
			num ^= SignSalary.GetHashCode();
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
		if (StoneSalary != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(StoneSalary);
		}
		if (JadeSalary != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(JadeSalary);
		}
		if (MatrixSalary != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(MatrixSalary);
		}
		if (ElixirSalary != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(ElixirSalary);
		}
		if (EquipmentSalary != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(EquipmentSalary);
		}
		if (SignSalary != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(SignSalary);
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
		if (StoneSalary != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StoneSalary);
		}
		if (JadeSalary != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(JadeSalary);
		}
		if (MatrixSalary != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MatrixSalary);
		}
		if (ElixirSalary != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ElixirSalary);
		}
		if (EquipmentSalary != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EquipmentSalary);
		}
		if (SignSalary != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SignSalary);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(NeiWuSalarySettingReq_1015153 other)
	{
		if (other != null)
		{
			if (other.StoneSalary != 0L)
			{
				StoneSalary = other.StoneSalary;
			}
			if (other.JadeSalary != 0)
			{
				JadeSalary = other.JadeSalary;
			}
			if (other.MatrixSalary != 0)
			{
				MatrixSalary = other.MatrixSalary;
			}
			if (other.ElixirSalary != 0)
			{
				ElixirSalary = other.ElixirSalary;
			}
			if (other.EquipmentSalary != 0)
			{
				EquipmentSalary = other.EquipmentSalary;
			}
			if (other.SignSalary != 0)
			{
				SignSalary = other.SignSalary;
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
				StoneSalary = input.ReadInt64();
				break;
			case 16u:
				JadeSalary = input.ReadInt32();
				break;
			case 24u:
				MatrixSalary = input.ReadInt32();
				break;
			case 32u:
				ElixirSalary = input.ReadInt32();
				break;
			case 40u:
				EquipmentSalary = input.ReadInt32();
				break;
			case 48u:
				SignSalary = input.ReadInt32();
				break;
			}
		}
	}
}
