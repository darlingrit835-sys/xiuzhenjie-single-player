using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class PowerUpResp_9001008 : IMessage<PowerUpResp_9001008>, IMessage, IEquatable<PowerUpResp_9001008>, IDeepCloneable<PowerUpResp_9001008>
{
	private static readonly MessageParser<PowerUpResp_9001008> _parser = new MessageParser<PowerUpResp_9001008>(() => new PowerUpResp_9001008());

	private UnknownFieldSet _unknownFields;

	public const int CurrentPowerFieldNumber = 1;

	private int currentPower_;

	[DebuggerNonUserCode]
	public static MessageParser<PowerUpResp_9001008> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ServerInitiativeMsgProtoReflection.Descriptor.MessageTypes[8];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int CurrentPower
	{
		get
		{
			return currentPower_;
		}
		set
		{
			currentPower_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public PowerUpResp_9001008()
	{
	}

	[DebuggerNonUserCode]
	public PowerUpResp_9001008(PowerUpResp_9001008 other)
		: this()
	{
		currentPower_ = other.currentPower_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PowerUpResp_9001008 Clone()
	{
		return new PowerUpResp_9001008(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PowerUpResp_9001008);
	}

	[DebuggerNonUserCode]
	public bool Equals(PowerUpResp_9001008 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CurrentPower != other.CurrentPower)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CurrentPower != 0)
		{
			num ^= CurrentPower.GetHashCode();
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
		if (CurrentPower != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(CurrentPower);
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
		if (CurrentPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPower);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(PowerUpResp_9001008 other)
	{
		if (other != null)
		{
			if (other.CurrentPower != 0)
			{
				CurrentPower = other.CurrentPower;
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
				CurrentPower = input.ReadInt32();
			}
		}
	}
}
