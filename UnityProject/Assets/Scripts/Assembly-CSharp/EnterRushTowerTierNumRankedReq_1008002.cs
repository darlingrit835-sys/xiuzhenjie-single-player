using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EnterRushTowerTierNumRankedReq_1008002 : IMessage<EnterRushTowerTierNumRankedReq_1008002>, IMessage, IEquatable<EnterRushTowerTierNumRankedReq_1008002>, IDeepCloneable<EnterRushTowerTierNumRankedReq_1008002>
{
	private static readonly MessageParser<EnterRushTowerTierNumRankedReq_1008002> _parser = new MessageParser<EnterRushTowerTierNumRankedReq_1008002>(() => new EnterRushTowerTierNumRankedReq_1008002());

	private UnknownFieldSet _unknownFields;

	public const int StageFieldNumber = 1;

	private int stage_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterRushTowerTierNumRankedReq_1008002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public EnterRushTowerTierNumRankedReq_1008002()
	{
	}

	[DebuggerNonUserCode]
	public EnterRushTowerTierNumRankedReq_1008002(EnterRushTowerTierNumRankedReq_1008002 other)
		: this()
	{
		stage_ = other.stage_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterRushTowerTierNumRankedReq_1008002 Clone()
	{
		return new EnterRushTowerTierNumRankedReq_1008002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterRushTowerTierNumRankedReq_1008002);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterRushTowerTierNumRankedReq_1008002 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
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
		if (Stage != 0)
		{
			output.WriteRawTag(8);
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
	public void MergeFrom(EnterRushTowerTierNumRankedReq_1008002 other)
	{
		if (other != null)
		{
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
			if (num != 8)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				Stage = input.ReadInt32();
			}
		}
	}
}
