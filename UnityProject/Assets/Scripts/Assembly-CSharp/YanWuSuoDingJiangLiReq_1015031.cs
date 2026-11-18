using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class YanWuSuoDingJiangLiReq_1015031 : IMessage<YanWuSuoDingJiangLiReq_1015031>, IMessage, IEquatable<YanWuSuoDingJiangLiReq_1015031>, IDeepCloneable<YanWuSuoDingJiangLiReq_1015031>
{
	private static readonly MessageParser<YanWuSuoDingJiangLiReq_1015031> _parser = new MessageParser<YanWuSuoDingJiangLiReq_1015031>(() => new YanWuSuoDingJiangLiReq_1015031());

	private UnknownFieldSet _unknownFields;

	public const int JiangLi1FieldNumber = 1;

	private long jiangLi1_;

	public const int JiangLi2FieldNumber = 2;

	private int jiangLi2_;

	public const int JiangLi3FieldNumber = 3;

	private int jiangLi3_;

	[DebuggerNonUserCode]
	public static MessageParser<YanWuSuoDingJiangLiReq_1015031> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[33];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public long JiangLi1
	{
		get
		{
			return jiangLi1_;
		}
		set
		{
			jiangLi1_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int JiangLi2
	{
		get
		{
			return jiangLi2_;
		}
		set
		{
			jiangLi2_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int JiangLi3
	{
		get
		{
			return jiangLi3_;
		}
		set
		{
			jiangLi3_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public YanWuSuoDingJiangLiReq_1015031()
	{
	}

	[DebuggerNonUserCode]
	public YanWuSuoDingJiangLiReq_1015031(YanWuSuoDingJiangLiReq_1015031 other)
		: this()
	{
		jiangLi1_ = other.jiangLi1_;
		jiangLi2_ = other.jiangLi2_;
		jiangLi3_ = other.jiangLi3_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public YanWuSuoDingJiangLiReq_1015031 Clone()
	{
		return new YanWuSuoDingJiangLiReq_1015031(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as YanWuSuoDingJiangLiReq_1015031);
	}

	[DebuggerNonUserCode]
	public bool Equals(YanWuSuoDingJiangLiReq_1015031 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (JiangLi1 != other.JiangLi1)
		{
			return false;
		}
		if (JiangLi2 != other.JiangLi2)
		{
			return false;
		}
		if (JiangLi3 != other.JiangLi3)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (JiangLi1 != 0L)
		{
			num ^= JiangLi1.GetHashCode();
		}
		if (JiangLi2 != 0)
		{
			num ^= JiangLi2.GetHashCode();
		}
		if (JiangLi3 != 0)
		{
			num ^= JiangLi3.GetHashCode();
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
		if (JiangLi1 != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(JiangLi1);
		}
		if (JiangLi2 != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(JiangLi2);
		}
		if (JiangLi3 != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(JiangLi3);
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
		if (JiangLi1 != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(JiangLi1);
		}
		if (JiangLi2 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(JiangLi2);
		}
		if (JiangLi3 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(JiangLi3);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(YanWuSuoDingJiangLiReq_1015031 other)
	{
		if (other != null)
		{
			if (other.JiangLi1 != 0L)
			{
				JiangLi1 = other.JiangLi1;
			}
			if (other.JiangLi2 != 0)
			{
				JiangLi2 = other.JiangLi2;
			}
			if (other.JiangLi3 != 0)
			{
				JiangLi3 = other.JiangLi3;
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
				JiangLi1 = input.ReadInt64();
				break;
			case 16u:
				JiangLi2 = input.ReadInt32();
				break;
			case 24u:
				JiangLi3 = input.ReadInt32();
				break;
			}
		}
	}
}
