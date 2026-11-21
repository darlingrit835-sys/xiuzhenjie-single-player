using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class KaiKenLandReq_1013003 : IMessage<KaiKenLandReq_1013003>, IMessage, IEquatable<KaiKenLandReq_1013003>, IDeepCloneable<KaiKenLandReq_1013003>
{
	private static readonly MessageParser<KaiKenLandReq_1013003> _parser = new MessageParser<KaiKenLandReq_1013003>(() => new KaiKenLandReq_1013003());

	private UnknownFieldSet _unknownFields;

	public const int LandIndexFieldNumber = 1;

	private int landIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<KaiKenLandReq_1013003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int LandIndex
	{
		get
		{
			return landIndex_;
		}
		set
		{
			landIndex_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public KaiKenLandReq_1013003()
	{
	}

	[DebuggerNonUserCode]
	public KaiKenLandReq_1013003(KaiKenLandReq_1013003 other)
		: this()
	{
		landIndex_ = other.landIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public KaiKenLandReq_1013003 Clone()
	{
		return new KaiKenLandReq_1013003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as KaiKenLandReq_1013003);
	}

	[DebuggerNonUserCode]
	public bool Equals(KaiKenLandReq_1013003 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (LandIndex != other.LandIndex)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (LandIndex != 0)
		{
			num ^= LandIndex.GetHashCode();
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
		if (LandIndex != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(LandIndex);
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
		if (LandIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LandIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(KaiKenLandReq_1013003 other)
	{
		if (other != null)
		{
			if (other.LandIndex != 0)
			{
				LandIndex = other.LandIndex;
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
				LandIndex = input.ReadInt32();
			}
		}
	}
}
