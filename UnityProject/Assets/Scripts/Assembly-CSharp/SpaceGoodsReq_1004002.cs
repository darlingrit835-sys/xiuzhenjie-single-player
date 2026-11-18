using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SpaceGoodsReq_1004002 : IMessage<SpaceGoodsReq_1004002>, IMessage, IEquatable<SpaceGoodsReq_1004002>, IDeepCloneable<SpaceGoodsReq_1004002>
{
	private static readonly MessageParser<SpaceGoodsReq_1004002> _parser = new MessageParser<SpaceGoodsReq_1004002>(() => new SpaceGoodsReq_1004002());

	private UnknownFieldSet _unknownFields;

	public const int LocationFieldNumber = 1;

	private int location_;

	[DebuggerNonUserCode]
	public static MessageParser<SpaceGoodsReq_1004002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Location
	{
		get
		{
			return location_;
		}
		set
		{
			location_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SpaceGoodsReq_1004002()
	{
	}

	[DebuggerNonUserCode]
	public SpaceGoodsReq_1004002(SpaceGoodsReq_1004002 other)
		: this()
	{
		location_ = other.location_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SpaceGoodsReq_1004002 Clone()
	{
		return new SpaceGoodsReq_1004002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SpaceGoodsReq_1004002);
	}

	[DebuggerNonUserCode]
	public bool Equals(SpaceGoodsReq_1004002 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Location != other.Location)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Location != 0)
		{
			num ^= Location.GetHashCode();
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
		if (Location != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Location);
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
		if (Location != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Location);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SpaceGoodsReq_1004002 other)
	{
		if (other != null)
		{
			if (other.Location != 0)
			{
				Location = other.Location;
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
				Location = input.ReadInt32();
			}
		}
	}
}
