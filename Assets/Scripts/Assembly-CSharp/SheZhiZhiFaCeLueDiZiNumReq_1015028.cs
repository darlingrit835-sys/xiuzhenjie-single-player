using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SheZhiZhiFaCeLueDiZiNumReq_1015028 : IMessage<SheZhiZhiFaCeLueDiZiNumReq_1015028>, IMessage, IEquatable<SheZhiZhiFaCeLueDiZiNumReq_1015028>, IDeepCloneable<SheZhiZhiFaCeLueDiZiNumReq_1015028>
{
	private static readonly MessageParser<SheZhiZhiFaCeLueDiZiNumReq_1015028> _parser = new MessageParser<SheZhiZhiFaCeLueDiZiNumReq_1015028>(() => new SheZhiZhiFaCeLueDiZiNumReq_1015028());

	private UnknownFieldSet _unknownFields;

	public const int ZhiFaCeLueFieldNumber = 1;

	private bool zhiFaCeLue_;

	public const int ZhiFaDiZiNumFieldNumber = 2;

	private int zhiFaDiZiNum_;

	[DebuggerNonUserCode]
	public static MessageParser<SheZhiZhiFaCeLueDiZiNumReq_1015028> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[30];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public bool ZhiFaCeLue
	{
		get
		{
			return zhiFaCeLue_;
		}
		set
		{
			zhiFaCeLue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ZhiFaDiZiNum
	{
		get
		{
			return zhiFaDiZiNum_;
		}
		set
		{
			zhiFaDiZiNum_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SheZhiZhiFaCeLueDiZiNumReq_1015028()
	{
	}

	[DebuggerNonUserCode]
	public SheZhiZhiFaCeLueDiZiNumReq_1015028(SheZhiZhiFaCeLueDiZiNumReq_1015028 other)
		: this()
	{
		zhiFaCeLue_ = other.zhiFaCeLue_;
		zhiFaDiZiNum_ = other.zhiFaDiZiNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SheZhiZhiFaCeLueDiZiNumReq_1015028 Clone()
	{
		return new SheZhiZhiFaCeLueDiZiNumReq_1015028(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SheZhiZhiFaCeLueDiZiNumReq_1015028);
	}

	[DebuggerNonUserCode]
	public bool Equals(SheZhiZhiFaCeLueDiZiNumReq_1015028 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ZhiFaCeLue != other.ZhiFaCeLue)
		{
			return false;
		}
		if (ZhiFaDiZiNum != other.ZhiFaDiZiNum)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ZhiFaCeLue)
		{
			num ^= ZhiFaCeLue.GetHashCode();
		}
		if (ZhiFaDiZiNum != 0)
		{
			num ^= ZhiFaDiZiNum.GetHashCode();
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
		if (ZhiFaCeLue)
		{
			output.WriteRawTag(8);
			output.WriteBool(ZhiFaCeLue);
		}
		if (ZhiFaDiZiNum != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(ZhiFaDiZiNum);
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
		if (ZhiFaCeLue)
		{
			num += 2;
		}
		if (ZhiFaDiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ZhiFaDiZiNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SheZhiZhiFaCeLueDiZiNumReq_1015028 other)
	{
		if (other != null)
		{
			if (other.ZhiFaCeLue)
			{
				ZhiFaCeLue = other.ZhiFaCeLue;
			}
			if (other.ZhiFaDiZiNum != 0)
			{
				ZhiFaDiZiNum = other.ZhiFaDiZiNum;
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
				ZhiFaCeLue = input.ReadBool();
				break;
			case 16u:
				ZhiFaDiZiNum = input.ReadInt32();
				break;
			}
		}
	}
}
