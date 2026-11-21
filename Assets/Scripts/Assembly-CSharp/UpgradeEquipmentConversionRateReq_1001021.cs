using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class UpgradeEquipmentConversionRateReq_1001021 : IMessage<UpgradeEquipmentConversionRateReq_1001021>, IMessage, IEquatable<UpgradeEquipmentConversionRateReq_1001021>, IDeepCloneable<UpgradeEquipmentConversionRateReq_1001021>
{
	private static readonly MessageParser<UpgradeEquipmentConversionRateReq_1001021> _parser = new MessageParser<UpgradeEquipmentConversionRateReq_1001021>(() => new UpgradeEquipmentConversionRateReq_1001021());

	private UnknownFieldSet _unknownFields;

	public const int EquipmentIdFieldNumber = 1;

	private string equipmentId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<UpgradeEquipmentConversionRateReq_1001021> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[25];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string EquipmentId
	{
		get
		{
			return equipmentId_;
		}
		set
		{
			equipmentId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UpgradeEquipmentConversionRateReq_1001021()
	{
	}

	[DebuggerNonUserCode]
	public UpgradeEquipmentConversionRateReq_1001021(UpgradeEquipmentConversionRateReq_1001021 other)
		: this()
	{
		equipmentId_ = other.equipmentId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UpgradeEquipmentConversionRateReq_1001021 Clone()
	{
		return new UpgradeEquipmentConversionRateReq_1001021(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UpgradeEquipmentConversionRateReq_1001021);
	}

	[DebuggerNonUserCode]
	public bool Equals(UpgradeEquipmentConversionRateReq_1001021 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (EquipmentId != other.EquipmentId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (EquipmentId.Length != 0)
		{
			num ^= EquipmentId.GetHashCode();
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
		if (EquipmentId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(EquipmentId);
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
		if (EquipmentId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(EquipmentId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UpgradeEquipmentConversionRateReq_1001021 other)
	{
		if (other != null)
		{
			if (other.EquipmentId.Length != 0)
			{
				EquipmentId = other.EquipmentId;
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				EquipmentId = input.ReadString();
			}
		}
	}
}
