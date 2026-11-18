using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class WaterDeployReq_1013017 : IMessage<WaterDeployReq_1013017>, IMessage, IEquatable<WaterDeployReq_1013017>, IDeepCloneable<WaterDeployReq_1013017>
{
	private static readonly MessageParser<WaterDeployReq_1013017> _parser = new MessageParser<WaterDeployReq_1013017>(() => new WaterDeployReq_1013017());

	private UnknownFieldSet _unknownFields;

	public const int GoodsIdFieldNumber = 1;

	private static readonly FieldCodec<string> _repeated_goodsId_codec = FieldCodec.ForString(10u);

	private readonly RepeatedField<string> goodsId_ = new RepeatedField<string>();

	public const int WaterMaterialTypeFieldNumber = 2;

	private int waterMaterialType_;

	[DebuggerNonUserCode]
	public static MessageParser<WaterDeployReq_1013017> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[20];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public RepeatedField<string> GoodsId => goodsId_;

	[DebuggerNonUserCode]
	public int WaterMaterialType
	{
		get
		{
			return waterMaterialType_;
		}
		set
		{
			waterMaterialType_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public WaterDeployReq_1013017()
	{
	}

	[DebuggerNonUserCode]
	public WaterDeployReq_1013017(WaterDeployReq_1013017 other)
		: this()
	{
		goodsId_ = other.goodsId_.Clone();
		waterMaterialType_ = other.waterMaterialType_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WaterDeployReq_1013017 Clone()
	{
		return new WaterDeployReq_1013017(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WaterDeployReq_1013017);
	}

	[DebuggerNonUserCode]
	public bool Equals(WaterDeployReq_1013017 other)
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
		if (WaterMaterialType != other.WaterMaterialType)
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
		if (WaterMaterialType != 0)
		{
			num ^= WaterMaterialType.GetHashCode();
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
		if (WaterMaterialType != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(WaterMaterialType);
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
		if (WaterMaterialType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WaterMaterialType);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WaterDeployReq_1013017 other)
	{
		if (other != null)
		{
			goodsId_.Add(other.goodsId_);
			if (other.WaterMaterialType != 0)
			{
				WaterMaterialType = other.WaterMaterialType;
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
				WaterMaterialType = input.ReadInt32();
				break;
			}
		}
	}
}
