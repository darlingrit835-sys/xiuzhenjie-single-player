using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class DeployRingGoodsSelectReq_1013008 : IMessage<DeployRingGoodsSelectReq_1013008>, IMessage, IEquatable<DeployRingGoodsSelectReq_1013008>, IDeepCloneable<DeployRingGoodsSelectReq_1013008>
{
	private static readonly MessageParser<DeployRingGoodsSelectReq_1013008> _parser = new MessageParser<DeployRingGoodsSelectReq_1013008>(() => new DeployRingGoodsSelectReq_1013008());

	private UnknownFieldSet _unknownFields;

	public const int GoodsTypeFieldNumber = 1;

	private int goodsType_;

	[DebuggerNonUserCode]
	public static MessageParser<DeployRingGoodsSelectReq_1013008> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int GoodsType
	{
		get
		{
			return goodsType_;
		}
		set
		{
			goodsType_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public DeployRingGoodsSelectReq_1013008()
	{
	}

	[DebuggerNonUserCode]
	public DeployRingGoodsSelectReq_1013008(DeployRingGoodsSelectReq_1013008 other)
		: this()
	{
		goodsType_ = other.goodsType_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DeployRingGoodsSelectReq_1013008 Clone()
	{
		return new DeployRingGoodsSelectReq_1013008(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DeployRingGoodsSelectReq_1013008);
	}

	[DebuggerNonUserCode]
	public bool Equals(DeployRingGoodsSelectReq_1013008 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (GoodsType != other.GoodsType)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (GoodsType != 0)
		{
			num ^= GoodsType.GetHashCode();
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
		if (GoodsType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(GoodsType);
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
		if (GoodsType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GoodsType);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DeployRingGoodsSelectReq_1013008 other)
	{
		if (other != null)
		{
			if (other.GoodsType != 0)
			{
				GoodsType = other.GoodsType;
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
				GoodsType = input.ReadInt32();
			}
		}
	}
}
