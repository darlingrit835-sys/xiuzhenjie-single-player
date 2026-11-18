using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GoodsMatrixUseReq_1004045 : IMessage<GoodsMatrixUseReq_1004045>, IMessage, IEquatable<GoodsMatrixUseReq_1004045>, IDeepCloneable<GoodsMatrixUseReq_1004045>
{
	private static readonly MessageParser<GoodsMatrixUseReq_1004045> _parser = new MessageParser<GoodsMatrixUseReq_1004045>(() => new GoodsMatrixUseReq_1004045());

	private UnknownFieldSet _unknownFields;

	public const int SkillIndexFieldNumber = 1;

	private int skillIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<GoodsMatrixUseReq_1004045> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[76];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int SkillIndex
	{
		get
		{
			return skillIndex_;
		}
		set
		{
			skillIndex_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsMatrixUseReq_1004045()
	{
	}

	[DebuggerNonUserCode]
	public GoodsMatrixUseReq_1004045(GoodsMatrixUseReq_1004045 other)
		: this()
	{
		skillIndex_ = other.skillIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsMatrixUseReq_1004045 Clone()
	{
		return new GoodsMatrixUseReq_1004045(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsMatrixUseReq_1004045);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsMatrixUseReq_1004045 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SkillIndex != other.SkillIndex)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (SkillIndex != 0)
		{
			num ^= SkillIndex.GetHashCode();
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
		if (SkillIndex != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(SkillIndex);
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
		if (SkillIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsMatrixUseReq_1004045 other)
	{
		if (other != null)
		{
			if (other.SkillIndex != 0)
			{
				SkillIndex = other.SkillIndex;
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
				SkillIndex = input.ReadInt32();
			}
		}
	}
}
