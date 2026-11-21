using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class KuangMaiJadeDiZiNumReq_1015045 : IMessage<KuangMaiJadeDiZiNumReq_1015045>, IMessage, IEquatable<KuangMaiJadeDiZiNumReq_1015045>, IDeepCloneable<KuangMaiJadeDiZiNumReq_1015045>
{
	private static readonly MessageParser<KuangMaiJadeDiZiNumReq_1015045> _parser = new MessageParser<KuangMaiJadeDiZiNumReq_1015045>(() => new KuangMaiJadeDiZiNumReq_1015045());

	private UnknownFieldSet _unknownFields;

	public const int JadeKaiCaiDiZiNumFieldNumber = 1;

	private int jadeKaiCaiDiZiNum_;

	[DebuggerNonUserCode]
	public static MessageParser<KuangMaiJadeDiZiNumReq_1015045> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[55];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int JadeKaiCaiDiZiNum
	{
		get
		{
			return jadeKaiCaiDiZiNum_;
		}
		set
		{
			jadeKaiCaiDiZiNum_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public KuangMaiJadeDiZiNumReq_1015045()
	{
	}

	[DebuggerNonUserCode]
	public KuangMaiJadeDiZiNumReq_1015045(KuangMaiJadeDiZiNumReq_1015045 other)
		: this()
	{
		jadeKaiCaiDiZiNum_ = other.jadeKaiCaiDiZiNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public KuangMaiJadeDiZiNumReq_1015045 Clone()
	{
		return new KuangMaiJadeDiZiNumReq_1015045(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as KuangMaiJadeDiZiNumReq_1015045);
	}

	[DebuggerNonUserCode]
	public bool Equals(KuangMaiJadeDiZiNumReq_1015045 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (JadeKaiCaiDiZiNum != other.JadeKaiCaiDiZiNum)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (JadeKaiCaiDiZiNum != 0)
		{
			num ^= JadeKaiCaiDiZiNum.GetHashCode();
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
		if (JadeKaiCaiDiZiNum != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(JadeKaiCaiDiZiNum);
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
		if (JadeKaiCaiDiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(JadeKaiCaiDiZiNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(KuangMaiJadeDiZiNumReq_1015045 other)
	{
		if (other != null)
		{
			if (other.JadeKaiCaiDiZiNum != 0)
			{
				JadeKaiCaiDiZiNum = other.JadeKaiCaiDiZiNum;
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
				JadeKaiCaiDiZiNum = input.ReadInt32();
			}
		}
	}
}
