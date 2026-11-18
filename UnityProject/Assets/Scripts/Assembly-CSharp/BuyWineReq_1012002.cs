using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BuyWineReq_1012002 : IMessage<BuyWineReq_1012002>, IMessage, IEquatable<BuyWineReq_1012002>, IDeepCloneable<BuyWineReq_1012002>
{
	private static readonly MessageParser<BuyWineReq_1012002> _parser = new MessageParser<BuyWineReq_1012002>(() => new BuyWineReq_1012002());

	private UnknownFieldSet _unknownFields;

	public const int WineTypeFieldNumber = 2;

	private int wineType_;

	[DebuggerNonUserCode]
	public static MessageParser<BuyWineReq_1012002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ChatProtoReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int WineType
	{
		get
		{
			return wineType_;
		}
		set
		{
			wineType_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BuyWineReq_1012002()
	{
	}

	[DebuggerNonUserCode]
	public BuyWineReq_1012002(BuyWineReq_1012002 other)
		: this()
	{
		wineType_ = other.wineType_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BuyWineReq_1012002 Clone()
	{
		return new BuyWineReq_1012002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BuyWineReq_1012002);
	}

	[DebuggerNonUserCode]
	public bool Equals(BuyWineReq_1012002 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (WineType != other.WineType)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (WineType != 0)
		{
			num ^= WineType.GetHashCode();
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
		if (WineType != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(WineType);
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
		if (WineType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WineType);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BuyWineReq_1012002 other)
	{
		if (other != null)
		{
			if (other.WineType != 0)
			{
				WineType = other.WineType;
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
			if (num != 16)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				WineType = input.ReadInt32();
			}
		}
	}
}
