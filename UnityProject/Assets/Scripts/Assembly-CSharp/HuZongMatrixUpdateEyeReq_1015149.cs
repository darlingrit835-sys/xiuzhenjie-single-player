using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class HuZongMatrixUpdateEyeReq_1015149 : IMessage<HuZongMatrixUpdateEyeReq_1015149>, IMessage, IEquatable<HuZongMatrixUpdateEyeReq_1015149>, IDeepCloneable<HuZongMatrixUpdateEyeReq_1015149>
{
	private static readonly MessageParser<HuZongMatrixUpdateEyeReq_1015149> _parser = new MessageParser<HuZongMatrixUpdateEyeReq_1015149>(() => new HuZongMatrixUpdateEyeReq_1015149());

	private UnknownFieldSet _unknownFields;

	public const int ZhenYanFieldNumber = 9;

	private int zhenYan_;

	[DebuggerNonUserCode]
	public static MessageParser<HuZongMatrixUpdateEyeReq_1015149> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[194];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int ZhenYan
	{
		get
		{
			return zhenYan_;
		}
		set
		{
			zhenYan_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public HuZongMatrixUpdateEyeReq_1015149()
	{
	}

	[DebuggerNonUserCode]
	public HuZongMatrixUpdateEyeReq_1015149(HuZongMatrixUpdateEyeReq_1015149 other)
		: this()
	{
		zhenYan_ = other.zhenYan_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public HuZongMatrixUpdateEyeReq_1015149 Clone()
	{
		return new HuZongMatrixUpdateEyeReq_1015149(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as HuZongMatrixUpdateEyeReq_1015149);
	}

	[DebuggerNonUserCode]
	public bool Equals(HuZongMatrixUpdateEyeReq_1015149 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ZhenYan != other.ZhenYan)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ZhenYan != 0)
		{
			num ^= ZhenYan.GetHashCode();
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
		if (ZhenYan != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(ZhenYan);
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
		if (ZhenYan != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ZhenYan);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(HuZongMatrixUpdateEyeReq_1015149 other)
	{
		if (other != null)
		{
			if (other.ZhenYan != 0)
			{
				ZhenYan = other.ZhenYan;
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
			if (num != 72)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				ZhenYan = input.ReadInt32();
			}
		}
	}
}
