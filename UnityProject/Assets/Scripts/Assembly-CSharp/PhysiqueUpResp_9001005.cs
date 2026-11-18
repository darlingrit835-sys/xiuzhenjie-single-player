using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class PhysiqueUpResp_9001005 : IMessage<PhysiqueUpResp_9001005>, IMessage, IEquatable<PhysiqueUpResp_9001005>, IDeepCloneable<PhysiqueUpResp_9001005>
{
	private static readonly MessageParser<PhysiqueUpResp_9001005> _parser = new MessageParser<PhysiqueUpResp_9001005>(() => new PhysiqueUpResp_9001005());

	private UnknownFieldSet _unknownFields;

	public const int CurrentPhysiqueFieldNumber = 1;

	private long currentPhysique_;

	public const int PhysiqueFieldNumber = 2;

	private long physique_;

	public const int CurrentPureBloodFieldNumber = 3;

	private int currentPureBlood_;

	public const int PureBloodFieldNumber = 4;

	private int pureBlood_;

	public const int PhysiqueRefiningValueFieldNumber = 5;

	private long physiqueRefiningValue_;

	public const int CurrentRunValueFieldNumber = 6;

	private double currentRunValue_;

	[DebuggerNonUserCode]
	public static MessageParser<PhysiqueUpResp_9001005> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ServerInitiativeMsgProtoReflection.Descriptor.MessageTypes[5];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public long CurrentPhysique
	{
		get
		{
			return currentPhysique_;
		}
		set
		{
			currentPhysique_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Physique
	{
		get
		{
			return physique_;
		}
		set
		{
			physique_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentPureBlood
	{
		get
		{
			return currentPureBlood_;
		}
		set
		{
			currentPureBlood_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PureBlood
	{
		get
		{
			return pureBlood_;
		}
		set
		{
			pureBlood_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long PhysiqueRefiningValue
	{
		get
		{
			return physiqueRefiningValue_;
		}
		set
		{
			physiqueRefiningValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double CurrentRunValue
	{
		get
		{
			return currentRunValue_;
		}
		set
		{
			currentRunValue_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public PhysiqueUpResp_9001005()
	{
	}

	[DebuggerNonUserCode]
	public PhysiqueUpResp_9001005(PhysiqueUpResp_9001005 other)
		: this()
	{
		currentPhysique_ = other.currentPhysique_;
		physique_ = other.physique_;
		currentPureBlood_ = other.currentPureBlood_;
		pureBlood_ = other.pureBlood_;
		physiqueRefiningValue_ = other.physiqueRefiningValue_;
		currentRunValue_ = other.currentRunValue_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PhysiqueUpResp_9001005 Clone()
	{
		return new PhysiqueUpResp_9001005(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PhysiqueUpResp_9001005);
	}

	[DebuggerNonUserCode]
	public bool Equals(PhysiqueUpResp_9001005 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CurrentPhysique != other.CurrentPhysique)
		{
			return false;
		}
		if (Physique != other.Physique)
		{
			return false;
		}
		if (CurrentPureBlood != other.CurrentPureBlood)
		{
			return false;
		}
		if (PureBlood != other.PureBlood)
		{
			return false;
		}
		if (PhysiqueRefiningValue != other.PhysiqueRefiningValue)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(CurrentRunValue, other.CurrentRunValue))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CurrentPhysique != 0L)
		{
			num ^= CurrentPhysique.GetHashCode();
		}
		if (Physique != 0L)
		{
			num ^= Physique.GetHashCode();
		}
		if (CurrentPureBlood != 0)
		{
			num ^= CurrentPureBlood.GetHashCode();
		}
		if (PureBlood != 0)
		{
			num ^= PureBlood.GetHashCode();
		}
		if (PhysiqueRefiningValue != 0L)
		{
			num ^= PhysiqueRefiningValue.GetHashCode();
		}
		if (CurrentRunValue != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(CurrentRunValue);
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
		if (CurrentPhysique != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(CurrentPhysique);
		}
		if (Physique != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(Physique);
		}
		if (CurrentPureBlood != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(CurrentPureBlood);
		}
		if (PureBlood != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(PureBlood);
		}
		if (PhysiqueRefiningValue != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(PhysiqueRefiningValue);
		}
		if (CurrentRunValue != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(CurrentRunValue);
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
		if (CurrentPhysique != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentPhysique);
		}
		if (Physique != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Physique);
		}
		if (CurrentPureBlood != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPureBlood);
		}
		if (PureBlood != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PureBlood);
		}
		if (PhysiqueRefiningValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(PhysiqueRefiningValue);
		}
		if (CurrentRunValue != 0.0)
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
	public void MergeFrom(PhysiqueUpResp_9001005 other)
	{
		if (other != null)
		{
			if (other.CurrentPhysique != 0L)
			{
				CurrentPhysique = other.CurrentPhysique;
			}
			if (other.Physique != 0L)
			{
				Physique = other.Physique;
			}
			if (other.CurrentPureBlood != 0)
			{
				CurrentPureBlood = other.CurrentPureBlood;
			}
			if (other.PureBlood != 0)
			{
				PureBlood = other.PureBlood;
			}
			if (other.PhysiqueRefiningValue != 0L)
			{
				PhysiqueRefiningValue = other.PhysiqueRefiningValue;
			}
			if (other.CurrentRunValue != 0.0)
			{
				CurrentRunValue = other.CurrentRunValue;
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
				CurrentPhysique = input.ReadInt64();
				break;
			case 16u:
				Physique = input.ReadInt64();
				break;
			case 24u:
				CurrentPureBlood = input.ReadInt32();
				break;
			case 32u:
				PureBlood = input.ReadInt32();
				break;
			case 40u:
				PhysiqueRefiningValue = input.ReadInt64();
				break;
			case 49u:
				CurrentRunValue = input.ReadDouble();
				break;
			}
		}
	}
}
