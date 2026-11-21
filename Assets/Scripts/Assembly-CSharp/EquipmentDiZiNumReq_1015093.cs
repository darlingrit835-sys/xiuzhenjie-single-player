using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EquipmentDiZiNumReq_1015093 : IMessage<EquipmentDiZiNumReq_1015093>, IMessage, IEquatable<EquipmentDiZiNumReq_1015093>, IDeepCloneable<EquipmentDiZiNumReq_1015093>
{
	private static readonly MessageParser<EquipmentDiZiNumReq_1015093> _parser = new MessageParser<EquipmentDiZiNumReq_1015093>(() => new EquipmentDiZiNumReq_1015093());

	private UnknownFieldSet _unknownFields;

	public const int DiZiNumFieldNumber = 1;

	private int diZiNum_;

	[DebuggerNonUserCode]
	public static MessageParser<EquipmentDiZiNumReq_1015093> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[113];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int DiZiNum
	{
		get
		{
			return diZiNum_;
		}
		set
		{
			diZiNum_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EquipmentDiZiNumReq_1015093()
	{
	}

	[DebuggerNonUserCode]
	public EquipmentDiZiNumReq_1015093(EquipmentDiZiNumReq_1015093 other)
		: this()
	{
		diZiNum_ = other.diZiNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EquipmentDiZiNumReq_1015093 Clone()
	{
		return new EquipmentDiZiNumReq_1015093(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EquipmentDiZiNumReq_1015093);
	}

	[DebuggerNonUserCode]
	public bool Equals(EquipmentDiZiNumReq_1015093 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (DiZiNum != other.DiZiNum)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (DiZiNum != 0)
		{
			num ^= DiZiNum.GetHashCode();
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
		if (DiZiNum != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(DiZiNum);
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
		if (DiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DiZiNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EquipmentDiZiNumReq_1015093 other)
	{
		if (other != null)
		{
			if (other.DiZiNum != 0)
			{
				DiZiNum = other.DiZiNum;
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
				DiZiNum = input.ReadInt32();
			}
		}
	}
}
