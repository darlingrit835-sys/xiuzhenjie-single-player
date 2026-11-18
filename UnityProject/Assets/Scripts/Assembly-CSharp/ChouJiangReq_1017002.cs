using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ChouJiangReq_1017002 : IMessage<ChouJiangReq_1017002>, IMessage, IEquatable<ChouJiangReq_1017002>, IDeepCloneable<ChouJiangReq_1017002>
{
	private static readonly MessageParser<ChouJiangReq_1017002> _parser = new MessageParser<ChouJiangReq_1017002>(() => new ChouJiangReq_1017002());

	private UnknownFieldSet _unknownFields;

	public const int StageFieldNumber = 1;

	private int stage_;

	public const int IsContinuousFieldNumber = 2;

	private bool isContinuous_;

	[DebuggerNonUserCode]
	public static MessageParser<ChouJiangReq_1017002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HuoDongProtoReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ChouJiangReq_1017002()
	{
	}

	[DebuggerNonUserCode]
	public ChouJiangReq_1017002(ChouJiangReq_1017002 other)
		: this()
	{
		stage_ = other.stage_;
		isContinuous_ = other.isContinuous_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ChouJiangReq_1017002 Clone()
	{
		return new ChouJiangReq_1017002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ChouJiangReq_1017002);
	}

	[DebuggerNonUserCode]
	public bool Equals(ChouJiangReq_1017002 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		if (IsContinuous != other.IsContinuous)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (IsContinuous)
		{
			num ^= IsContinuous.GetHashCode();
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
		if (Stage != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Stage);
		}
		if (IsContinuous)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsContinuous);
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
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (IsContinuous)
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
	public void MergeFrom(ChouJiangReq_1017002 other)
	{
		if (other != null)
		{
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			if (other.IsContinuous)
			{
				IsContinuous = other.IsContinuous;
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
				Stage = input.ReadInt32();
				break;
			case 16u:
				IsContinuous = input.ReadBool();
				break;
			}
		}
	}
}
