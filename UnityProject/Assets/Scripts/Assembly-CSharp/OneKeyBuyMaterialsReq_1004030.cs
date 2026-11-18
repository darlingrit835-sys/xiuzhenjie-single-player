using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class OneKeyBuyMaterialsReq_1004030 : IMessage<OneKeyBuyMaterialsReq_1004030>, IMessage, IEquatable<OneKeyBuyMaterialsReq_1004030>, IDeepCloneable<OneKeyBuyMaterialsReq_1004030>
{
	private static readonly MessageParser<OneKeyBuyMaterialsReq_1004030> _parser = new MessageParser<OneKeyBuyMaterialsReq_1004030>(() => new OneKeyBuyMaterialsReq_1004030());

	private UnknownFieldSet _unknownFields;

	public const int MaterialIdFieldNumber = 1;

	private static readonly FieldCodec<string> _repeated_materialId_codec = FieldCodec.ForString(10u);

	private readonly RepeatedField<string> materialId_ = new RepeatedField<string>();

	[DebuggerNonUserCode]
	public static MessageParser<OneKeyBuyMaterialsReq_1004030> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[51];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public RepeatedField<string> MaterialId => materialId_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public OneKeyBuyMaterialsReq_1004030()
	{
	}

	[DebuggerNonUserCode]
	public OneKeyBuyMaterialsReq_1004030(OneKeyBuyMaterialsReq_1004030 other)
		: this()
	{
		materialId_ = other.materialId_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OneKeyBuyMaterialsReq_1004030 Clone()
	{
		return new OneKeyBuyMaterialsReq_1004030(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OneKeyBuyMaterialsReq_1004030);
	}

	[DebuggerNonUserCode]
	public bool Equals(OneKeyBuyMaterialsReq_1004030 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!materialId_.Equals(other.materialId_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= materialId_.GetHashCode();
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
		materialId_.WriteTo(output, _repeated_materialId_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += materialId_.CalculateSize(_repeated_materialId_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OneKeyBuyMaterialsReq_1004030 other)
	{
		if (other != null)
		{
			materialId_.Add(other.materialId_);
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				materialId_.AddEntriesFrom(input, _repeated_materialId_codec);
			}
		}
	}
}
