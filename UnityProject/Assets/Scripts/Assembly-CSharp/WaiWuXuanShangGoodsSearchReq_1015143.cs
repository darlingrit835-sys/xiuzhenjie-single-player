using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class WaiWuXuanShangGoodsSearchReq_1015143 : IMessage<WaiWuXuanShangGoodsSearchReq_1015143>, IMessage, IEquatable<WaiWuXuanShangGoodsSearchReq_1015143>, IDeepCloneable<WaiWuXuanShangGoodsSearchReq_1015143>
{
	private static readonly MessageParser<WaiWuXuanShangGoodsSearchReq_1015143> _parser = new MessageParser<WaiWuXuanShangGoodsSearchReq_1015143>(() => new WaiWuXuanShangGoodsSearchReq_1015143());

	private UnknownFieldSet _unknownFields;

	public const int GoodsTypeFieldNumber = 1;

	private int goodsType_;

	public const int NameFieldNumber = 2;

	private string name_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<WaiWuXuanShangGoodsSearchReq_1015143> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[183];

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

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public WaiWuXuanShangGoodsSearchReq_1015143()
	{
	}

	[DebuggerNonUserCode]
	public WaiWuXuanShangGoodsSearchReq_1015143(WaiWuXuanShangGoodsSearchReq_1015143 other)
		: this()
	{
		goodsType_ = other.goodsType_;
		name_ = other.name_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WaiWuXuanShangGoodsSearchReq_1015143 Clone()
	{
		return new WaiWuXuanShangGoodsSearchReq_1015143(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WaiWuXuanShangGoodsSearchReq_1015143);
	}

	[DebuggerNonUserCode]
	public bool Equals(WaiWuXuanShangGoodsSearchReq_1015143 other)
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
		if (Name != other.Name)
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
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
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
		if (Name.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Name);
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
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WaiWuXuanShangGoodsSearchReq_1015143 other)
	{
		if (other != null)
		{
			if (other.GoodsType != 0)
			{
				GoodsType = other.GoodsType;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
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
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 8u:
				GoodsType = input.ReadInt32();
				break;
			case 18u:
				Name = input.ReadString();
				break;
			}
		}
	}
}
