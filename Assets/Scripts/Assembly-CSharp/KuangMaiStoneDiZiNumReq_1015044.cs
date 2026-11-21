using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class KuangMaiStoneDiZiNumReq_1015044 : IMessage<KuangMaiStoneDiZiNumReq_1015044>, IMessage, IEquatable<KuangMaiStoneDiZiNumReq_1015044>, IDeepCloneable<KuangMaiStoneDiZiNumReq_1015044>
{
	private static readonly MessageParser<KuangMaiStoneDiZiNumReq_1015044> _parser = new MessageParser<KuangMaiStoneDiZiNumReq_1015044>(() => new KuangMaiStoneDiZiNumReq_1015044());

	private UnknownFieldSet _unknownFields;

	public const int StoneKaiCaiDiZiNumFieldNumber = 1;

	private int stoneKaiCaiDiZiNum_;

	[DebuggerNonUserCode]
	public static MessageParser<KuangMaiStoneDiZiNumReq_1015044> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[53];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int StoneKaiCaiDiZiNum
	{
		get
		{
			return stoneKaiCaiDiZiNum_;
		}
		set
		{
			stoneKaiCaiDiZiNum_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public KuangMaiStoneDiZiNumReq_1015044()
	{
	}

	[DebuggerNonUserCode]
	public KuangMaiStoneDiZiNumReq_1015044(KuangMaiStoneDiZiNumReq_1015044 other)
		: this()
	{
		stoneKaiCaiDiZiNum_ = other.stoneKaiCaiDiZiNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public KuangMaiStoneDiZiNumReq_1015044 Clone()
	{
		return new KuangMaiStoneDiZiNumReq_1015044(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as KuangMaiStoneDiZiNumReq_1015044);
	}

	[DebuggerNonUserCode]
	public bool Equals(KuangMaiStoneDiZiNumReq_1015044 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (StoneKaiCaiDiZiNum != other.StoneKaiCaiDiZiNum)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (StoneKaiCaiDiZiNum != 0)
		{
			num ^= StoneKaiCaiDiZiNum.GetHashCode();
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
		if (StoneKaiCaiDiZiNum != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(StoneKaiCaiDiZiNum);
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
		if (StoneKaiCaiDiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(StoneKaiCaiDiZiNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(KuangMaiStoneDiZiNumReq_1015044 other)
	{
		if (other != null)
		{
			if (other.StoneKaiCaiDiZiNum != 0)
			{
				StoneKaiCaiDiZiNum = other.StoneKaiCaiDiZiNum;
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
				StoneKaiCaiDiZiNum = input.ReadInt32();
			}
		}
	}
}
