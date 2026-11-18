using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RechooseEquipmentEffectReq_1005044 : IMessage<RechooseEquipmentEffectReq_1005044>, IMessage, IEquatable<RechooseEquipmentEffectReq_1005044>, IDeepCloneable<RechooseEquipmentEffectReq_1005044>
{
	private static readonly MessageParser<RechooseEquipmentEffectReq_1005044> _parser = new MessageParser<RechooseEquipmentEffectReq_1005044>(() => new RechooseEquipmentEffectReq_1005044());

	private UnknownFieldSet _unknownFields;

	public const int GoodsIdFieldNumber = 1;

	private string goodsId_ = "";

	public const int PayTypeFieldNumber = 2;

	private int payType_;

	public const int IsLock1FieldNumber = 3;

	private bool isLock1_;

	public const int IsLock2FieldNumber = 4;

	private bool isLock2_;

	public const int IsLock3FieldNumber = 5;

	private bool isLock3_;

	[DebuggerNonUserCode]
	public static MessageParser<RechooseEquipmentEffectReq_1005044> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[56];

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

	[DebuggerNonUserCode]
	public int PayType
	{
		get
		{
			return payType_;
		}
		set
		{
			payType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsLock1
	{
		get
		{
			return isLock1_;
		}
		set
		{
			isLock1_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsLock2
	{
		get
		{
			return isLock2_;
		}
		set
		{
			isLock2_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsLock3
	{
		get
		{
			return isLock3_;
		}
		set
		{
			isLock3_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RechooseEquipmentEffectReq_1005044()
	{
	}

	[DebuggerNonUserCode]
	public RechooseEquipmentEffectReq_1005044(RechooseEquipmentEffectReq_1005044 other)
		: this()
	{
		goodsId_ = other.goodsId_;
		payType_ = other.payType_;
		isLock1_ = other.isLock1_;
		isLock2_ = other.isLock2_;
		isLock3_ = other.isLock3_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RechooseEquipmentEffectReq_1005044 Clone()
	{
		return new RechooseEquipmentEffectReq_1005044(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RechooseEquipmentEffectReq_1005044);
	}

	[DebuggerNonUserCode]
	public bool Equals(RechooseEquipmentEffectReq_1005044 other)
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
		if (PayType != other.PayType)
		{
			return false;
		}
		if (IsLock1 != other.IsLock1)
		{
			return false;
		}
		if (IsLock2 != other.IsLock2)
		{
			return false;
		}
		if (IsLock3 != other.IsLock3)
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
		if (PayType != 0)
		{
			num ^= PayType.GetHashCode();
		}
		if (IsLock1)
		{
			num ^= IsLock1.GetHashCode();
		}
		if (IsLock2)
		{
			num ^= IsLock2.GetHashCode();
		}
		if (IsLock3)
		{
			num ^= IsLock3.GetHashCode();
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
		if (PayType != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(PayType);
		}
		if (IsLock1)
		{
			output.WriteRawTag(24);
			output.WriteBool(IsLock1);
		}
		if (IsLock2)
		{
			output.WriteRawTag(32);
			output.WriteBool(IsLock2);
		}
		if (IsLock3)
		{
			output.WriteRawTag(40);
			output.WriteBool(IsLock3);
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
		if (PayType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PayType);
		}
		if (IsLock1)
		{
			num += 2;
		}
		if (IsLock2)
		{
			num += 2;
		}
		if (IsLock3)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RechooseEquipmentEffectReq_1005044 other)
	{
		if (other != null)
		{
			if (other.GoodsId.Length != 0)
			{
				GoodsId = other.GoodsId;
			}
			if (other.PayType != 0)
			{
				PayType = other.PayType;
			}
			if (other.IsLock1)
			{
				IsLock1 = other.IsLock1;
			}
			if (other.IsLock2)
			{
				IsLock2 = other.IsLock2;
			}
			if (other.IsLock3)
			{
				IsLock3 = other.IsLock3;
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
			case 10u:
				GoodsId = input.ReadString();
				break;
			case 16u:
				PayType = input.ReadInt32();
				break;
			case 24u:
				IsLock1 = input.ReadBool();
				break;
			case 32u:
				IsLock2 = input.ReadBool();
				break;
			case 40u:
				IsLock3 = input.ReadBool();
				break;
			}
		}
	}
}
