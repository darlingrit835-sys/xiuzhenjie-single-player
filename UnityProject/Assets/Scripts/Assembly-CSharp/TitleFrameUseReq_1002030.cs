using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class TitleFrameUseReq_1002030 : IMessage<TitleFrameUseReq_1002030>, IMessage, IEquatable<TitleFrameUseReq_1002030>, IDeepCloneable<TitleFrameUseReq_1002030>
{
	private static readonly MessageParser<TitleFrameUseReq_1002030> _parser = new MessageParser<TitleFrameUseReq_1002030>(() => new TitleFrameUseReq_1002030());

	private UnknownFieldSet _unknownFields;

	public const int TitleFrameFieldNumber = 1;

	private int titleFrame_;

	[DebuggerNonUserCode]
	public static MessageParser<TitleFrameUseReq_1002030> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[35];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int TitleFrame
	{
		get
		{
			return titleFrame_;
		}
		set
		{
			titleFrame_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public TitleFrameUseReq_1002030()
	{
	}

	[DebuggerNonUserCode]
	public TitleFrameUseReq_1002030(TitleFrameUseReq_1002030 other)
		: this()
	{
		titleFrame_ = other.titleFrame_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TitleFrameUseReq_1002030 Clone()
	{
		return new TitleFrameUseReq_1002030(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TitleFrameUseReq_1002030);
	}

	[DebuggerNonUserCode]
	public bool Equals(TitleFrameUseReq_1002030 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TitleFrame != other.TitleFrame)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (TitleFrame != 0)
		{
			num ^= TitleFrame.GetHashCode();
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
		if (TitleFrame != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(TitleFrame);
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
		if (TitleFrame != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TitleFrame);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TitleFrameUseReq_1002030 other)
	{
		if (other != null)
		{
			if (other.TitleFrame != 0)
			{
				TitleFrame = other.TitleFrame;
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
				TitleFrame = input.ReadInt32();
			}
		}
	}
}
