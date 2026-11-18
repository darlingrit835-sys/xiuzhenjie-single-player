using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildUpgradeReq_1015029 : IMessage<GuildUpgradeReq_1015029>, IMessage, IEquatable<GuildUpgradeReq_1015029>, IDeepCloneable<GuildUpgradeReq_1015029>
{
	private static readonly MessageParser<GuildUpgradeReq_1015029> _parser = new MessageParser<GuildUpgradeReq_1015029>(() => new GuildUpgradeReq_1015029());

	private UnknownFieldSet _unknownFields;

	public const int GoodsIdFieldNumber = 1;

	private string goodsId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<GuildUpgradeReq_1015029> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[188];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string GoodsId
	{
		get
		{
			return goodsId_;
		}
		set
		{
			goodsId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildUpgradeReq_1015029()
	{
	}

	[DebuggerNonUserCode]
	public GuildUpgradeReq_1015029(GuildUpgradeReq_1015029 other)
		: this()
	{
		goodsId_ = other.goodsId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildUpgradeReq_1015029 Clone()
	{
		return new GuildUpgradeReq_1015029(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildUpgradeReq_1015029);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildUpgradeReq_1015029 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (GoodsId != other.GoodsId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (GoodsId.Length != 0)
		{
			num ^= GoodsId.GetHashCode();
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
		if (GoodsId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(GoodsId);
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
		if (GoodsId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GoodsId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildUpgradeReq_1015029 other)
	{
		if (other != null)
		{
			if (other.GoodsId.Length != 0)
			{
				GoodsId = other.GoodsId;
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
				GoodsId = input.ReadString();
			}
		}
	}
}
