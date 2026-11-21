using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class CleverUpResp_9001002 : IMessage<CleverUpResp_9001002>, IMessage, IEquatable<CleverUpResp_9001002>, IDeepCloneable<CleverUpResp_9001002>
{
	private static readonly MessageParser<CleverUpResp_9001002> _parser = new MessageParser<CleverUpResp_9001002>(() => new CleverUpResp_9001002());

	private UnknownFieldSet _unknownFields;

	public const int CurrentCleverFieldNumber = 1;

	private int currentClever_;

	[DebuggerNonUserCode]
	public static MessageParser<CleverUpResp_9001002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ServerInitiativeMsgProtoReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int CurrentClever
	{
		get
		{
			return currentClever_;
		}
		set
		{
			currentClever_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public CleverUpResp_9001002()
	{
	}

	[DebuggerNonUserCode]
	public CleverUpResp_9001002(CleverUpResp_9001002 other)
		: this()
	{
		currentClever_ = other.currentClever_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CleverUpResp_9001002 Clone()
	{
		return new CleverUpResp_9001002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CleverUpResp_9001002);
	}

	[DebuggerNonUserCode]
	public bool Equals(CleverUpResp_9001002 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CurrentClever != other.CurrentClever)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CurrentClever != 0)
		{
			num ^= CurrentClever.GetHashCode();
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
		if (CurrentClever != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(CurrentClever);
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
		if (CurrentClever != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentClever);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CleverUpResp_9001002 other)
	{
		if (other != null)
		{
			if (other.CurrentClever != 0)
			{
				CurrentClever = other.CurrentClever;
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
				CurrentClever = input.ReadInt32();
			}
		}
	}
}
