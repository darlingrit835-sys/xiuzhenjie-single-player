using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SoilDeployReq_1013007 : IMessage<SoilDeployReq_1013007>, IMessage, IEquatable<SoilDeployReq_1013007>, IDeepCloneable<SoilDeployReq_1013007>
{
	private static readonly MessageParser<SoilDeployReq_1013007> _parser = new MessageParser<SoilDeployReq_1013007>(() => new SoilDeployReq_1013007());

	private UnknownFieldSet _unknownFields;

	public const int GoodsIdFieldNumber = 1;

	private static readonly FieldCodec<string> _repeated_goodsId_codec = FieldCodec.ForString(10u);

	private readonly RepeatedField<string> goodsId_ = new RepeatedField<string>();

	public const int XiRangTypeFieldNumber = 2;

	private int xiRangType_;

	[DebuggerNonUserCode]
	public static MessageParser<SoilDeployReq_1013007> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[7];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public RepeatedField<string> GoodsId => goodsId_;

	[DebuggerNonUserCode]
	public int XiRangType
	{
		get
		{
			return xiRangType_;
		}
		set
		{
			xiRangType_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SoilDeployReq_1013007()
	{
	}

	[DebuggerNonUserCode]
	public SoilDeployReq_1013007(SoilDeployReq_1013007 other)
		: this()
	{
		goodsId_ = other.goodsId_.Clone();
		xiRangType_ = other.xiRangType_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SoilDeployReq_1013007 Clone()
	{
		return new SoilDeployReq_1013007(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SoilDeployReq_1013007);
	}

	[DebuggerNonUserCode]
	public bool Equals(SoilDeployReq_1013007 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!goodsId_.Equals(other.goodsId_))
		{
			return false;
		}
		if (XiRangType != other.XiRangType)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= goodsId_.GetHashCode();
		if (XiRangType != 0)
		{
			num ^= XiRangType.GetHashCode();
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
		goodsId_.WriteTo(output, _repeated_goodsId_codec);
		if (XiRangType != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(XiRangType);
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
		num += goodsId_.CalculateSize(_repeated_goodsId_codec);
		if (XiRangType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(XiRangType);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SoilDeployReq_1013007 other)
	{
		if (other != null)
		{
			goodsId_.Add(other.goodsId_);
			if (other.XiRangType != 0)
			{
				XiRangType = other.XiRangType;
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
			case 10u:
				goodsId_.AddEntriesFrom(input, _repeated_goodsId_codec);
				break;
			case 16u:
				XiRangType = input.ReadInt32();
				break;
			}
		}
	}
}
