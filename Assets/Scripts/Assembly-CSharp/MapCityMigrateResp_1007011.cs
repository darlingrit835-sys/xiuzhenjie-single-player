using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class MapCityMigrateResp_1007011 : IMessage<MapCityMigrateResp_1007011>, IMessage, IEquatable<MapCityMigrateResp_1007011>, IDeepCloneable<MapCityMigrateResp_1007011>
{
	private static readonly MessageParser<MapCityMigrateResp_1007011> _parser = new MessageParser<MapCityMigrateResp_1007011>(() => new MapCityMigrateResp_1007011());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentAgeFieldNumber = 3;

	private int currentAge_;

	public const int CurrentPowerFieldNumber = 4;

	private int currentPower_;

	[DebuggerNonUserCode]
	public static MessageParser<MapCityMigrateResp_1007011> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MapProtoReflection.Descriptor.MessageTypes[14];

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
	public int CurrentAge
	{
		get
		{
			return currentAge_;
		}
		set
		{
			currentAge_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentPower
	{
		get
		{
			return currentPower_;
		}
		set
		{
			currentPower_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public MapCityMigrateResp_1007011()
	{
	}

	[DebuggerNonUserCode]
	public MapCityMigrateResp_1007011(MapCityMigrateResp_1007011 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentAge_ = other.currentAge_;
		currentPower_ = other.currentPower_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MapCityMigrateResp_1007011 Clone()
	{
		return new MapCityMigrateResp_1007011(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MapCityMigrateResp_1007011);
	}

	[DebuggerNonUserCode]
	public bool Equals(MapCityMigrateResp_1007011 other)
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
		if (CurrentAge != other.CurrentAge)
		{
			return false;
		}
		if (CurrentPower != other.CurrentPower)
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
		if (CurrentAge != 0)
		{
			num ^= CurrentAge.GetHashCode();
		}
		if (CurrentPower != 0)
		{
			num ^= CurrentPower.GetHashCode();
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
		if (CurrentAge != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(CurrentAge);
		}
		if (CurrentPower != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(CurrentPower);
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
		if (CurrentAge != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentAge);
		}
		if (CurrentPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPower);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MapCityMigrateResp_1007011 other)
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
			if (other.CurrentAge != 0)
			{
				CurrentAge = other.CurrentAge;
			}
			if (other.CurrentPower != 0)
			{
				CurrentPower = other.CurrentPower;
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
				CurrentAge = input.ReadInt32();
				break;
			case 32u:
				CurrentPower = input.ReadInt32();
				break;
			}
		}
	}
}
