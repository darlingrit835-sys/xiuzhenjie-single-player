using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class PerceptionUpResp_9001006 : IMessage<PerceptionUpResp_9001006>, IMessage, IEquatable<PerceptionUpResp_9001006>, IDeepCloneable<PerceptionUpResp_9001006>
{
	private static readonly MessageParser<PerceptionUpResp_9001006> _parser = new MessageParser<PerceptionUpResp_9001006>(() => new PerceptionUpResp_9001006());

	private UnknownFieldSet _unknownFields;

	public const int CurrentPerceptionFieldNumber = 1;

	private long currentPerception_;

	public const int PerceptionFieldNumber = 2;

	private long perception_;

	public const int PerceptionRefiningValueFieldNumber = 3;

	private long perceptionRefiningValue_;

	public const int CurrentRunValueFieldNumber = 4;

	private double currentRunValue_;

	[DebuggerNonUserCode]
	public static MessageParser<PerceptionUpResp_9001006> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ServerInitiativeMsgProtoReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public long CurrentPerception
	{
		get
		{
			return currentPerception_;
		}
		set
		{
			currentPerception_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Perception
	{
		get
		{
			return perception_;
		}
		set
		{
			perception_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long PerceptionRefiningValue
	{
		get
		{
			return perceptionRefiningValue_;
		}
		set
		{
			perceptionRefiningValue_ = value;
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
	public PerceptionUpResp_9001006()
	{
	}

	[DebuggerNonUserCode]
	public PerceptionUpResp_9001006(PerceptionUpResp_9001006 other)
		: this()
	{
		currentPerception_ = other.currentPerception_;
		perception_ = other.perception_;
		perceptionRefiningValue_ = other.perceptionRefiningValue_;
		currentRunValue_ = other.currentRunValue_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PerceptionUpResp_9001006 Clone()
	{
		return new PerceptionUpResp_9001006(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PerceptionUpResp_9001006);
	}

	[DebuggerNonUserCode]
	public bool Equals(PerceptionUpResp_9001006 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CurrentPerception != other.CurrentPerception)
		{
			return false;
		}
		if (Perception != other.Perception)
		{
			return false;
		}
		if (PerceptionRefiningValue != other.PerceptionRefiningValue)
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
		if (CurrentPerception != 0L)
		{
			num ^= CurrentPerception.GetHashCode();
		}
		if (Perception != 0L)
		{
			num ^= Perception.GetHashCode();
		}
		if (PerceptionRefiningValue != 0L)
		{
			num ^= PerceptionRefiningValue.GetHashCode();
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
		if (CurrentPerception != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(CurrentPerception);
		}
		if (Perception != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(Perception);
		}
		if (PerceptionRefiningValue != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(PerceptionRefiningValue);
		}
		if (CurrentRunValue != 0.0)
		{
			output.WriteRawTag(33);
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
		if (CurrentPerception != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentPerception);
		}
		if (Perception != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Perception);
		}
		if (PerceptionRefiningValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(PerceptionRefiningValue);
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
	public void MergeFrom(PerceptionUpResp_9001006 other)
	{
		if (other != null)
		{
			if (other.CurrentPerception != 0L)
			{
				CurrentPerception = other.CurrentPerception;
			}
			if (other.Perception != 0L)
			{
				Perception = other.Perception;
			}
			if (other.PerceptionRefiningValue != 0L)
			{
				PerceptionRefiningValue = other.PerceptionRefiningValue;
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
				CurrentPerception = input.ReadInt64();
				break;
			case 16u:
				Perception = input.ReadInt64();
				break;
			case 24u:
				PerceptionRefiningValue = input.ReadInt64();
				break;
			case 33u:
				CurrentRunValue = input.ReadDouble();
				break;
			}
		}
	}
}
