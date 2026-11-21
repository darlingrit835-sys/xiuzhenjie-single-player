using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class HeadFrameUseReq_1002031 : IMessage<HeadFrameUseReq_1002031>, IMessage, IEquatable<HeadFrameUseReq_1002031>, IDeepCloneable<HeadFrameUseReq_1002031>
{
	private static readonly MessageParser<HeadFrameUseReq_1002031> _parser = new MessageParser<HeadFrameUseReq_1002031>(() => new HeadFrameUseReq_1002031());

	private UnknownFieldSet _unknownFields;

	public const int HeadFrameFieldNumber = 1;

	private int headFrame_;

	[DebuggerNonUserCode]
	public static MessageParser<HeadFrameUseReq_1002031> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[37];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int HeadFrame
	{
		get
		{
			return headFrame_;
		}
		set
		{
			headFrame_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public HeadFrameUseReq_1002031()
	{
	}

	[DebuggerNonUserCode]
	public HeadFrameUseReq_1002031(HeadFrameUseReq_1002031 other)
		: this()
	{
		headFrame_ = other.headFrame_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public HeadFrameUseReq_1002031 Clone()
	{
		return new HeadFrameUseReq_1002031(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as HeadFrameUseReq_1002031);
	}

	[DebuggerNonUserCode]
	public bool Equals(HeadFrameUseReq_1002031 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (HeadFrame != other.HeadFrame)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (HeadFrame != 0)
		{
			num ^= HeadFrame.GetHashCode();
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
		if (HeadFrame != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(HeadFrame);
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
		if (HeadFrame != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeadFrame);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(HeadFrameUseReq_1002031 other)
	{
		if (other != null)
		{
			if (other.HeadFrame != 0)
			{
				HeadFrame = other.HeadFrame;
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
				HeadFrame = input.ReadInt32();
			}
		}
	}
}
