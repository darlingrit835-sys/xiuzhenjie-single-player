using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class PosionDownResp_9001003 : IMessage<PosionDownResp_9001003>, IMessage, IEquatable<PosionDownResp_9001003>, IDeepCloneable<PosionDownResp_9001003>
{
	private static readonly MessageParser<PosionDownResp_9001003> _parser = new MessageParser<PosionDownResp_9001003>(() => new PosionDownResp_9001003());

	private UnknownFieldSet _unknownFields;

	public const int CurrentPosionFieldNumber = 1;

	private int currentPosion_;

	[DebuggerNonUserCode]
	public static MessageParser<PosionDownResp_9001003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ServerInitiativeMsgProtoReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int CurrentPosion
	{
		get
		{
			return currentPosion_;
		}
		set
		{
			currentPosion_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public PosionDownResp_9001003()
	{
	}

	[DebuggerNonUserCode]
	public PosionDownResp_9001003(PosionDownResp_9001003 other)
		: this()
	{
		currentPosion_ = other.currentPosion_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PosionDownResp_9001003 Clone()
	{
		return new PosionDownResp_9001003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PosionDownResp_9001003);
	}

	[DebuggerNonUserCode]
	public bool Equals(PosionDownResp_9001003 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CurrentPosion != other.CurrentPosion)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CurrentPosion != 0)
		{
			num ^= CurrentPosion.GetHashCode();
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
		if (CurrentPosion != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(CurrentPosion);
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
		if (CurrentPosion != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPosion);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(PosionDownResp_9001003 other)
	{
		if (other != null)
		{
			if (other.CurrentPosion != 0)
			{
				CurrentPosion = other.CurrentPosion;
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
				CurrentPosion = input.ReadInt32();
			}
		}
	}
}
