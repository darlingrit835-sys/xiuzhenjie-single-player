using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BuyXuanTianLouGoodsReq_1004035 : IMessage<BuyXuanTianLouGoodsReq_1004035>, IMessage, IEquatable<BuyXuanTianLouGoodsReq_1004035>, IDeepCloneable<BuyXuanTianLouGoodsReq_1004035>
{
	private static readonly MessageParser<BuyXuanTianLouGoodsReq_1004035> _parser = new MessageParser<BuyXuanTianLouGoodsReq_1004035>(() => new BuyXuanTianLouGoodsReq_1004035());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private int id_;

	[DebuggerNonUserCode]
	public static MessageParser<BuyXuanTianLouGoodsReq_1004035> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[58];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BuyXuanTianLouGoodsReq_1004035()
	{
	}

	[DebuggerNonUserCode]
	public BuyXuanTianLouGoodsReq_1004035(BuyXuanTianLouGoodsReq_1004035 other)
		: this()
	{
		id_ = other.id_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BuyXuanTianLouGoodsReq_1004035 Clone()
	{
		return new BuyXuanTianLouGoodsReq_1004035(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BuyXuanTianLouGoodsReq_1004035);
	}

	[DebuggerNonUserCode]
	public bool Equals(BuyXuanTianLouGoodsReq_1004035 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Id != other.Id)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Id != 0)
		{
			num ^= Id.GetHashCode();
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
		if (Id != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Id);
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
		if (Id != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Id);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BuyXuanTianLouGoodsReq_1004035 other)
	{
		if (other != null)
		{
			if (other.Id != 0)
			{
				Id = other.Id;
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
				Id = input.ReadInt32();
			}
		}
	}
}
