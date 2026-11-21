using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class DiscardWaterReq_1013026 : IMessage<DiscardWaterReq_1013026>, IMessage, IEquatable<DiscardWaterReq_1013026>, IDeepCloneable<DiscardWaterReq_1013026>
{
	private static readonly MessageParser<DiscardWaterReq_1013026> _parser = new MessageParser<DiscardWaterReq_1013026>(() => new DiscardWaterReq_1013026());

	private UnknownFieldSet _unknownFields;

	public const int WaterIdFieldNumber = 1;

	private string waterId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<DiscardWaterReq_1013026> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[30];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string WaterId
	{
		get
		{
			return waterId_;
		}
		set
		{
			waterId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public DiscardWaterReq_1013026()
	{
	}

	[DebuggerNonUserCode]
	public DiscardWaterReq_1013026(DiscardWaterReq_1013026 other)
		: this()
	{
		waterId_ = other.waterId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DiscardWaterReq_1013026 Clone()
	{
		return new DiscardWaterReq_1013026(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DiscardWaterReq_1013026);
	}

	[DebuggerNonUserCode]
	public bool Equals(DiscardWaterReq_1013026 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (WaterId != other.WaterId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (WaterId.Length != 0)
		{
			num ^= WaterId.GetHashCode();
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
		if (WaterId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(WaterId);
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
		if (WaterId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(WaterId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DiscardWaterReq_1013026 other)
	{
		if (other != null)
		{
			if (other.WaterId.Length != 0)
			{
				WaterId = other.WaterId;
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				WaterId = input.ReadString();
			}
		}
	}
}
