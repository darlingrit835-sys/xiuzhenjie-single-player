using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ChangeControllerMatrixReq_1001009 : IMessage<ChangeControllerMatrixReq_1001009>, IMessage, IEquatable<ChangeControllerMatrixReq_1001009>, IDeepCloneable<ChangeControllerMatrixReq_1001009>
{
	private static readonly MessageParser<ChangeControllerMatrixReq_1001009> _parser = new MessageParser<ChangeControllerMatrixReq_1001009>(() => new ChangeControllerMatrixReq_1001009());

	private UnknownFieldSet _unknownFields;

	public const int GoldIsOpenFieldNumber = 1;

	private bool goldIsOpen_;

	public const int WoodIsOpenFieldNumber = 2;

	private bool woodIsOpen_;

	public const int WaterIsOpenFieldNumber = 3;

	private bool waterIsOpen_;

	public const int FireIsOpenFieldNumber = 4;

	private bool fireIsOpen_;

	public const int EarthIsOpenFieldNumber = 5;

	private bool earthIsOpen_;

	[DebuggerNonUserCode]
	public static MessageParser<ChangeControllerMatrixReq_1001009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public bool GoldIsOpen
	{
		get
		{
			return goldIsOpen_;
		}
		set
		{
			goldIsOpen_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool WoodIsOpen
	{
		get
		{
			return woodIsOpen_;
		}
		set
		{
			woodIsOpen_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool WaterIsOpen
	{
		get
		{
			return waterIsOpen_;
		}
		set
		{
			waterIsOpen_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool FireIsOpen
	{
		get
		{
			return fireIsOpen_;
		}
		set
		{
			fireIsOpen_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool EarthIsOpen
	{
		get
		{
			return earthIsOpen_;
		}
		set
		{
			earthIsOpen_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ChangeControllerMatrixReq_1001009()
	{
	}

	[DebuggerNonUserCode]
	public ChangeControllerMatrixReq_1001009(ChangeControllerMatrixReq_1001009 other)
		: this()
	{
		goldIsOpen_ = other.goldIsOpen_;
		woodIsOpen_ = other.woodIsOpen_;
		waterIsOpen_ = other.waterIsOpen_;
		fireIsOpen_ = other.fireIsOpen_;
		earthIsOpen_ = other.earthIsOpen_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ChangeControllerMatrixReq_1001009 Clone()
	{
		return new ChangeControllerMatrixReq_1001009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ChangeControllerMatrixReq_1001009);
	}

	[DebuggerNonUserCode]
	public bool Equals(ChangeControllerMatrixReq_1001009 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (GoldIsOpen != other.GoldIsOpen)
		{
			return false;
		}
		if (WoodIsOpen != other.WoodIsOpen)
		{
			return false;
		}
		if (WaterIsOpen != other.WaterIsOpen)
		{
			return false;
		}
		if (FireIsOpen != other.FireIsOpen)
		{
			return false;
		}
		if (EarthIsOpen != other.EarthIsOpen)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (GoldIsOpen)
		{
			num ^= GoldIsOpen.GetHashCode();
		}
		if (WoodIsOpen)
		{
			num ^= WoodIsOpen.GetHashCode();
		}
		if (WaterIsOpen)
		{
			num ^= WaterIsOpen.GetHashCode();
		}
		if (FireIsOpen)
		{
			num ^= FireIsOpen.GetHashCode();
		}
		if (EarthIsOpen)
		{
			num ^= EarthIsOpen.GetHashCode();
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
		if (GoldIsOpen)
		{
			output.WriteRawTag(8);
			output.WriteBool(GoldIsOpen);
		}
		if (WoodIsOpen)
		{
			output.WriteRawTag(16);
			output.WriteBool(WoodIsOpen);
		}
		if (WaterIsOpen)
		{
			output.WriteRawTag(24);
			output.WriteBool(WaterIsOpen);
		}
		if (FireIsOpen)
		{
			output.WriteRawTag(32);
			output.WriteBool(FireIsOpen);
		}
		if (EarthIsOpen)
		{
			output.WriteRawTag(40);
			output.WriteBool(EarthIsOpen);
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
		if (GoldIsOpen)
		{
			num += 2;
		}
		if (WoodIsOpen)
		{
			num += 2;
		}
		if (WaterIsOpen)
		{
			num += 2;
		}
		if (FireIsOpen)
		{
			num += 2;
		}
		if (EarthIsOpen)
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
	public void MergeFrom(ChangeControllerMatrixReq_1001009 other)
	{
		if (other != null)
		{
			if (other.GoldIsOpen)
			{
				GoldIsOpen = other.GoldIsOpen;
			}
			if (other.WoodIsOpen)
			{
				WoodIsOpen = other.WoodIsOpen;
			}
			if (other.WaterIsOpen)
			{
				WaterIsOpen = other.WaterIsOpen;
			}
			if (other.FireIsOpen)
			{
				FireIsOpen = other.FireIsOpen;
			}
			if (other.EarthIsOpen)
			{
				EarthIsOpen = other.EarthIsOpen;
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
				GoldIsOpen = input.ReadBool();
				break;
			case 16u:
				WoodIsOpen = input.ReadBool();
				break;
			case 24u:
				WaterIsOpen = input.ReadBool();
				break;
			case 32u:
				FireIsOpen = input.ReadBool();
				break;
			case 40u:
				EarthIsOpen = input.ReadBool();
				break;
			}
		}
	}
}
