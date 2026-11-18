using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildChouJiangReq_1015156 : IMessage<GuildChouJiangReq_1015156>, IMessage, IEquatable<GuildChouJiangReq_1015156>, IDeepCloneable<GuildChouJiangReq_1015156>
{
	private static readonly MessageParser<GuildChouJiangReq_1015156> _parser = new MessageParser<GuildChouJiangReq_1015156>(() => new GuildChouJiangReq_1015156());

	private UnknownFieldSet _unknownFields;

	public const int IsContinuousFieldNumber = 1;

	private bool isContinuous_;

	public const int StageFieldNumber = 2;

	private int stage_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildChouJiangReq_1015156> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[198];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public bool IsContinuous
	{
		get
		{
			return isContinuous_;
		}
		set
		{
			isContinuous_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildChouJiangReq_1015156()
	{
	}

	[DebuggerNonUserCode]
	public GuildChouJiangReq_1015156(GuildChouJiangReq_1015156 other)
		: this()
	{
		isContinuous_ = other.isContinuous_;
		stage_ = other.stage_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildChouJiangReq_1015156 Clone()
	{
		return new GuildChouJiangReq_1015156(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildChouJiangReq_1015156);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildChouJiangReq_1015156 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (IsContinuous != other.IsContinuous)
		{
			return false;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (IsContinuous)
		{
			num ^= IsContinuous.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
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
		if (IsContinuous)
		{
			output.WriteRawTag(8);
			output.WriteBool(IsContinuous);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Stage);
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
		if (IsContinuous)
		{
			num += 2;
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildChouJiangReq_1015156 other)
	{
		if (other != null)
		{
			if (other.IsContinuous)
			{
				IsContinuous = other.IsContinuous;
			}
			if (other.Stage != 0)
			{
				Stage = other.Stage;
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
				IsContinuous = input.ReadBool();
				break;
			case 16u:
				Stage = input.ReadInt32();
				break;
			}
		}
	}
}
