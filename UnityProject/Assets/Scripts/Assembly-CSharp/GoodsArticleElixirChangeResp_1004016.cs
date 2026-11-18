using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GoodsArticleElixirChangeResp_1004016 : IMessage<GoodsArticleElixirChangeResp_1004016>, IMessage, IEquatable<GoodsArticleElixirChangeResp_1004016>, IDeepCloneable<GoodsArticleElixirChangeResp_1004016>
{
	private static readonly MessageParser<GoodsArticleElixirChangeResp_1004016> _parser = new MessageParser<GoodsArticleElixirChangeResp_1004016>(() => new GoodsArticleElixirChangeResp_1004016());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int EffectValueFieldNumber = 3;

	private double effectValue_;

	public const int EatTimeFieldNumber = 4;

	private int eatTime_;

	public const int EffectFieldNumber = 5;

	private int effect_;

	[DebuggerNonUserCode]
	public static MessageParser<GoodsArticleElixirChangeResp_1004016> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[27];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Code
	{
		get
		{
			return code_;
		}
		set
		{
			code_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsSuccess
	{
		get
		{
			return isSuccess_;
		}
		set
		{
			isSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double EffectValue
	{
		get
		{
			return effectValue_;
		}
		set
		{
			effectValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EatTime
	{
		get
		{
			return eatTime_;
		}
		set
		{
			eatTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Effect
	{
		get
		{
			return effect_;
		}
		set
		{
			effect_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsArticleElixirChangeResp_1004016()
	{
	}

	[DebuggerNonUserCode]
	public GoodsArticleElixirChangeResp_1004016(GoodsArticleElixirChangeResp_1004016 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		effectValue_ = other.effectValue_;
		eatTime_ = other.eatTime_;
		effect_ = other.effect_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsArticleElixirChangeResp_1004016 Clone()
	{
		return new GoodsArticleElixirChangeResp_1004016(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsArticleElixirChangeResp_1004016);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsArticleElixirChangeResp_1004016 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Code != other.Code)
		{
			return false;
		}
		if (IsSuccess != other.IsSuccess)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(EffectValue, other.EffectValue))
		{
			return false;
		}
		if (EatTime != other.EatTime)
		{
			return false;
		}
		if (Effect != other.Effect)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		if (EffectValue != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(EffectValue);
		}
		if (EatTime != 0)
		{
			num ^= EatTime.GetHashCode();
		}
		if (Effect != 0)
		{
			num ^= Effect.GetHashCode();
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
		if (Code != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Code);
		}
		if (IsSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsSuccess);
		}
		if (EffectValue != 0.0)
		{
			output.WriteRawTag(25);
			output.WriteDouble(EffectValue);
		}
		if (EatTime != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(EatTime);
		}
		if (Effect != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Effect);
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
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		if (EffectValue != 0.0)
		{
			num += 9;
		}
		if (EatTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EatTime);
		}
		if (Effect != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Effect);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsArticleElixirChangeResp_1004016 other)
	{
		if (other != null)
		{
			if (other.Code != 0)
			{
				Code = other.Code;
			}
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
			if (other.EffectValue != 0.0)
			{
				EffectValue = other.EffectValue;
			}
			if (other.EatTime != 0)
			{
				EatTime = other.EatTime;
			}
			if (other.Effect != 0)
			{
				Effect = other.Effect;
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
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 25u:
				EffectValue = input.ReadDouble();
				break;
			case 32u:
				EatTime = input.ReadInt32();
				break;
			case 40u:
				Effect = input.ReadInt32();
				break;
			}
		}
	}
}
