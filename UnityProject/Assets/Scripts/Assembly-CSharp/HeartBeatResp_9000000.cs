using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class HeartBeatResp_9000000 : IMessage<HeartBeatResp_9000000>, IMessage, IEquatable<HeartBeatResp_9000000>, IDeepCloneable<HeartBeatResp_9000000>
{
	private static readonly MessageParser<HeartBeatResp_9000000> _parser = new MessageParser<HeartBeatResp_9000000>(() => new HeartBeatResp_9000000());

	private UnknownFieldSet _unknownFields;

	public const int HeartBeatFieldNumber = 1;

	private int heartBeat_;

	[DebuggerNonUserCode]
	public static MessageParser<HeartBeatResp_9000000> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ServerInitiativeMsgProtoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int HeartBeat
	{
		get
		{
			return heartBeat_;
		}
		set
		{
			heartBeat_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public HeartBeatResp_9000000()
	{
	}

	[DebuggerNonUserCode]
	public HeartBeatResp_9000000(HeartBeatResp_9000000 other)
		: this()
	{
		heartBeat_ = other.heartBeat_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public HeartBeatResp_9000000 Clone()
	{
		return new HeartBeatResp_9000000(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as HeartBeatResp_9000000);
	}

	[DebuggerNonUserCode]
	public bool Equals(HeartBeatResp_9000000 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (HeartBeat != other.HeartBeat)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (HeartBeat != 0)
		{
			num ^= HeartBeat.GetHashCode();
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
		if (HeartBeat != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(HeartBeat);
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
		if (HeartBeat != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeartBeat);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(HeartBeatResp_9000000 other)
	{
		if (other != null)
		{
			if (other.HeartBeat != 0)
			{
				HeartBeat = other.HeartBeat;
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
				HeartBeat = input.ReadInt32();
			}
		}
	}
}
