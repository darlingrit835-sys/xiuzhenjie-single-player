using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class OpenControllerMatrixResp_1005014 : IMessage<OpenControllerMatrixResp_1005014>, IMessage, IEquatable<OpenControllerMatrixResp_1005014>, IDeepCloneable<OpenControllerMatrixResp_1005014>
{
	private static readonly MessageParser<OpenControllerMatrixResp_1005014> _parser = new MessageParser<OpenControllerMatrixResp_1005014>(() => new OpenControllerMatrixResp_1005014());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int IsOpenGoldFieldNumber = 3;

	private bool isOpenGold_;

	public const int IsOpenWoodFieldNumber = 4;

	private bool isOpenWood_;

	public const int IsOpenWaterFieldNumber = 5;

	private bool isOpenWater_;

	public const int IsOpenFireFieldNumber = 6;

	private bool isOpenFire_;

	public const int IsOpenEarthFieldNumber = 7;

	private bool isOpenEarth_;

	[DebuggerNonUserCode]
	public static MessageParser<OpenControllerMatrixResp_1005014> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[18];

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
	public bool IsOpenGold
	{
		get
		{
			return isOpenGold_;
		}
		set
		{
			isOpenGold_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsOpenWood
	{
		get
		{
			return isOpenWood_;
		}
		set
		{
			isOpenWood_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsOpenWater
	{
		get
		{
			return isOpenWater_;
		}
		set
		{
			isOpenWater_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsOpenFire
	{
		get
		{
			return isOpenFire_;
		}
		set
		{
			isOpenFire_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsOpenEarth
	{
		get
		{
			return isOpenEarth_;
		}
		set
		{
			isOpenEarth_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public OpenControllerMatrixResp_1005014()
	{
	}

	[DebuggerNonUserCode]
	public OpenControllerMatrixResp_1005014(OpenControllerMatrixResp_1005014 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		isOpenGold_ = other.isOpenGold_;
		isOpenWood_ = other.isOpenWood_;
		isOpenWater_ = other.isOpenWater_;
		isOpenFire_ = other.isOpenFire_;
		isOpenEarth_ = other.isOpenEarth_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OpenControllerMatrixResp_1005014 Clone()
	{
		return new OpenControllerMatrixResp_1005014(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OpenControllerMatrixResp_1005014);
	}

	[DebuggerNonUserCode]
	public bool Equals(OpenControllerMatrixResp_1005014 other)
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
		if (IsOpenGold != other.IsOpenGold)
		{
			return false;
		}
		if (IsOpenWood != other.IsOpenWood)
		{
			return false;
		}
		if (IsOpenWater != other.IsOpenWater)
		{
			return false;
		}
		if (IsOpenFire != other.IsOpenFire)
		{
			return false;
		}
		if (IsOpenEarth != other.IsOpenEarth)
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
		if (IsOpenGold)
		{
			num ^= IsOpenGold.GetHashCode();
		}
		if (IsOpenWood)
		{
			num ^= IsOpenWood.GetHashCode();
		}
		if (IsOpenWater)
		{
			num ^= IsOpenWater.GetHashCode();
		}
		if (IsOpenFire)
		{
			num ^= IsOpenFire.GetHashCode();
		}
		if (IsOpenEarth)
		{
			num ^= IsOpenEarth.GetHashCode();
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
		if (IsOpenGold)
		{
			output.WriteRawTag(24);
			output.WriteBool(IsOpenGold);
		}
		if (IsOpenWood)
		{
			output.WriteRawTag(32);
			output.WriteBool(IsOpenWood);
		}
		if (IsOpenWater)
		{
			output.WriteRawTag(40);
			output.WriteBool(IsOpenWater);
		}
		if (IsOpenFire)
		{
			output.WriteRawTag(48);
			output.WriteBool(IsOpenFire);
		}
		if (IsOpenEarth)
		{
			output.WriteRawTag(56);
			output.WriteBool(IsOpenEarth);
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
		if (IsOpenGold)
		{
			num += 2;
		}
		if (IsOpenWood)
		{
			num += 2;
		}
		if (IsOpenWater)
		{
			num += 2;
		}
		if (IsOpenFire)
		{
			num += 2;
		}
		if (IsOpenEarth)
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
	public void MergeFrom(OpenControllerMatrixResp_1005014 other)
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
			if (other.IsOpenGold)
			{
				IsOpenGold = other.IsOpenGold;
			}
			if (other.IsOpenWood)
			{
				IsOpenWood = other.IsOpenWood;
			}
			if (other.IsOpenWater)
			{
				IsOpenWater = other.IsOpenWater;
			}
			if (other.IsOpenFire)
			{
				IsOpenFire = other.IsOpenFire;
			}
			if (other.IsOpenEarth)
			{
				IsOpenEarth = other.IsOpenEarth;
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
			case 24u:
				IsOpenGold = input.ReadBool();
				break;
			case 32u:
				IsOpenWood = input.ReadBool();
				break;
			case 40u:
				IsOpenWater = input.ReadBool();
				break;
			case 48u:
				IsOpenFire = input.ReadBool();
				break;
			case 56u:
				IsOpenEarth = input.ReadBool();
				break;
			}
		}
	}
}
